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

public partial class qa_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdrange1, xprd1, xprd2, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, branch_Cd, header_n, footer_n;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, cond = " ", pdfView = "", data_found = "";
    DataRow dr;
    double db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0;
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
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");
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
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF"); data_found = "Y";
        data_found = "Y";

        switch (iconID)
        {
            case "F30101":
                #region INW INSP. TEMP
                SQuery = "select i.iname,i.unit,i.cpartno as icpart,i.cdrgno,a.* from inspmst a,item i  where  trim(i.icode)=trim(a.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') IN (" + barCode + ") order by srno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InwardInsTemp", "std_InwardInsTemp", dsRep, "INW INSP. TEMP");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30106":
            case "F30133":
                header_n = "QA In-Proc";
                SQuery = "SELECT '" + header_n + "' as header,F.ANAME,I.INAME,I.CPARTNO AS ICPARTNO,I.CDRGNO AS DRAG,I.UNIT AS IUNIT,A.* FROM INSPMST A,FAMST F, ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') IN (" + barCode + ") ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InprocInsTemp", "std_InprocInsTemp", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30108":
                #region OUT INSP. TEMP
                SQuery = "select i.iname,i.unit,i.cdrgno,I.CPARTNO AS ICPARTNO,a.* from inspmst a,item i  where  trim(i.icode)=trim(a.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") ORDER BY SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_OutwardInsTemplate", "std_OutwardInsTemplate", dsRep, "INW INSP. TEMP");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30111":
                #region INW INSP. RPT
                SQuery = "SELECT 'Inward Inspection Report' AS HEADER , F.ANAME,I.INAME,I.CPARTNO AS ICPARTNO,A.* FROM INSPVCH A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("minb", typeof(double));
                dt.Columns.Add("maxb", typeof(double));
                double min = 0, max = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db1 = fgen.make_double(dt.Rows[i]["obsv1"].ToString().Trim());
                    db2 = fgen.make_double(dt.Rows[i]["obsv2"].ToString().Trim());
                    db3 = fgen.make_double(dt.Rows[i]["obsv3"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["obsv4"].ToString().Trim());
                    db5 = fgen.make_double(dt.Rows[i]["obsv5"].ToString().Trim());
                    //for min and max
                    min = Math.Min(db1, Math.Min(db2, Math.Min(db3, Math.Min(db4, db5))));
                    max = Math.Max(db1, Math.Max(db2, Math.Max(db3, Math.Max(db4, db5))));
                    dt.Rows[i]["minb"] = min;
                    dt.Rows[i]["maxb"] = max;
                }

                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_InwardInwReport", "std_InwardInwReport_New_10col", dsRep, "INW INSP. RPT", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30113":
                #region OUT INSP. RPT
                SQuery = "SELECT 'Pre Dispatch Inspection Report' AS HEADER , F.ANAME,F.ADDR1 AS FDDR,I.INAME,I.CPARTNO AS ICPARTNO,i.unit as iunit,A.* FROM inspvch A,FAMST F, ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(I.ICODE)=TRIM(A.ICODE) AND a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") ORDER BY A.SRNO";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_OutwardInsReport", "std_OutwardInsReport", dsRep, "OUT INSP. RPT", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30132":
                header_n = "Quality Inward Register";
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES, A.O_DEPTT,A.VCHNUM AS MRRNO,A.TYPE,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDATE,trim(A.REFNUM) AS CHALLANO,A.REFDATE AS CHALDATE,A.GENUM AS GENO,A.GEDATE AS GEDT,trim(A.ICODE) as icode ,trim(A.ACODE) as acode,trim(B.ANAME) as aname,nvl(A.IQTYIN,0) AS ACPT_QTY, A.INVNO,A.INVDATE,A.PONUM,A.PODATE,nvl(A.IRATE,0) as irate,nvl(A.IAMOUNT,0) as iamount,A.NARATION,'RGP:'||A.RGPNUM AS RGPNUM,TO_CHAR(A.RGPDATE,'DD/MM/YYYY') AS RGPDATE,A.DESC_,trim(C.INAME) as iname,C.UNIT AS CUNIT,C.CPARTNO,A.FINVNO,A.MODE_TPT AS VECH,A.PNAME,TO_CHAR(A.QC_DATE,'DD/MM/YYYY') AS QC_DATE,nvl(a.rej_rw,0) as rej_rw,nvl(a.iqty_chl,0) as tot FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE)  AND  A.BRANCHCD='" + frm_mbr + "' and A.TYPE like '0%' AND A.VCHDATE " + xprdRange + " and a.inspected='Y' and a.store in ('Y','N') ORDER BY A.MORDER";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_MRR_REG_QA", "std_MRR_REG_QA", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30141":
                header_n = "Basic QA";
                SQuery = "select a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'' AS btoprint,f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,a.* from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.store!='R' and trim(a.type)=trim(t.type1) and t.id='M' AND a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||trim(a.vchnum) in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mrr_qa", "std_mrr_qa", dsRep, "M.R.R Report", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30225":
                header_n = "Suppliers 12 Month Rejection Trend";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ACODE,C.ANAME,a.icode as item_code,b.INAME,b.cpartno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar  from (select ACODE,icode,(Case when to_char(VCHDATE,'mm')='04' then IQTYIN else 0 end) as Apr,(Case when to_char(VCHDATE,'mm')='05' then IQTYIN else 0 end) as may,(Case when to_char(VCHDATE,'mm')='06' then IQTYIN   else 0 end) as jun,(Case when to_char(VCHDATE,'mm')='07' then IQTYIN else 0 end) as jul,(Case when to_char(VCHDATE,'mm')='08' then IQTYIN else 0 end) as aug,(Case when to_char(VCHDATE,'mm')='09' then IQTYIN else 0 end) as sep,(Case when to_char(VCHDATE,'mm')='10' then IQTYIN else 0 end) as oct,(Case when to_char(VCHDATE,'mm')='11' then IQTYIN else 0 end) as nov,(Case when to_char(VCHDATE,'mm')='12' then IQTYIN  else 0 end) as dec,(Case when to_char(VCHDATE,'mm')='01' then IQTYIN  else 0 end) as jan,(Case when to_char(VCHDATE,'mm')='02' then IQTYIN  else 0 end) as feb,(Case when to_char(VCHDATE,'mm')='03' then IQTYIN  else 0 end) as mar  from IVOUCHER where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE " + xprdRange + " AND STORE='R') a,ITEM b,FAMST C,type d where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND substr(trim(a.icode),1,2)=trim(D.type1) and D.id='Y' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME ORDER BY A.ACODE ";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Supp_Mth_Rej_Trend", "std_Supp_Mth_Rej_Trend", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30226":
                header_n = "Group, Item Wise 12 Month Rejection Trend";
                footer_n = "";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + footer_n + "' as footer,A.ACODE,C.ANAME,a.icode as item_code,b.INAME,substr(trim(a.icode),1,2) as mg,D.NAME AS GRPNAME,b.cpartno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar  from (select ACODE,icode,(Case when to_char(VCHDATE,'mm')='04' then IQTYIN else 0 end) as Apr,(Case when to_char(VCHDATE,'mm')='05' then IQTYIN else 0 end) as may,(Case when to_char(VCHDATE,'mm')='06' then IQTYIN   else 0 end) as jun,(Case when to_char(VCHDATE,'mm')='07' then IQTYIN else 0 end) as jul,(Case when to_char(VCHDATE,'mm')='08' then IQTYIN else 0 end) as aug,(Case when to_char(VCHDATE,'mm')='09' then IQTYIN else 0 end) as sep,(Case when to_char(VCHDATE,'mm')='10' then IQTYIN else 0 end) as oct,(Case when to_char(VCHDATE,'mm')='11' then IQTYIN else 0 end) as nov,(Case when to_char(VCHDATE,'mm')='12' then IQTYIN  else 0 end) as dec,(Case when to_char(VCHDATE,'mm')='01' then IQTYIN  else 0 end) as jan,(Case when to_char(VCHDATE,'mm')='02' then IQTYIN  else 0 end) as feb,(Case when to_char(VCHDATE,'mm')='03' then IQTYIN  else 0 end) as mar  from IVOUCHER where branchcd='" + frm_mbr + "' and type like '0%' and VCHDATE " + xprdRange + " AND STORE='R') a,ITEM b,FAMST C,type d where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND substr(trim(a.icode),1,2)=trim(D.type1) and D.id='Y' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,substr(trim(a.icode),1,2),D.NAME,D.TYPE1 ORDER BY A.ACODE,MG";
                dsRep = new DataSet();
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

            case "F30227":
                header_n = "Deptt, Item Wise 12 Month Rejection Trend";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ACODE,C.name as dept,a.icode,TRIM(b.INAME) AS INAME,b.cpartno,b.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select ACODE,icode,(Case when to_char(VCHDATE,'mm')='04' then IQTYIN else 0 end) as Apr,(Case when to_char(VCHDATE,'mm')='05' then IQTYIN else 0 end) as may,(Case when to_char(VCHDATE,'mm')='06' then IQTYIN   else 0 end) as jun,(Case when to_char(VCHDATE,'mm')='07' then IQTYIN else 0 end) as jul,(Case when to_char(VCHDATE,'mm')='08' then IQTYIN else 0 end) as aug,(Case when to_char(VCHDATE,'mm')='09' then IQTYIN else 0 end) as sep,(Case when to_char(VCHDATE,'mm')='10' then IQTYIN else 0 end) as oct,(Case when to_char(VCHDATE,'mm')='11' then IQTYIN else 0 end) as nov,(Case when to_char(VCHDATE,'mm')='12' then IQTYIN  else 0 end) as dec,(Case when to_char(VCHDATE,'mm')='01' then IQTYIN  else 0 end) as jan,(Case when to_char(VCHDATE,'mm')='02' then IQTYIN  else 0 end) as feb,(Case when to_char(VCHDATE,'mm')='03' then IQTYIN  else 0 end) as mar  from IVOUCHER where branchcd='" + frm_mbr + "' and type='14' and VCHDATE " + xprdRange + " AND STORE='R')  a,ITEM b,type C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.Type1) and c.id='M' group by a.icode,b.iname,b.cpartno,b.unit,a.acode,c.NAME ORDER BY A.iCODE";
                dsRep = new DataSet();
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

            case "F30150":
                header_n = "Request Sampling And Quality Approval Of Raw Material";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,a.vchnum , to_char(a.vchdate,'dd/mm/yyyy') as entry_date, a.acode as supp_code,b.aname as matlsupp,a.refnum as mrrnum ,a.refdate as mrrdate,a.icode as raw_matl_code, trim(c.iname) as matl_name,a.vcode as unld_sup,a.invno as bill_no, a.invdate as bill_date,a.t_deptt as lcnstr,a.o_deptt as pack_size,a.iqty_chl as qtyrcv,a.iqtyout,a.mtime,a.mfgdt,a.expdt,a.naration,a.ent_by,a.ent_dt  from ivoucher a , famst b,item c   where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and  a.type= '33' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "om_Samp_Req", "om_Samp_Req", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30352":
                #region LPE
                header_n = "Liquid Penetrant Examination Report";
                mq4 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='042'", "");
                if (mq4 == "0") { mq4 = "-"; }
                SQuery = "select '" + mq4 + "' as IS_NO,a.*,f.aname from inspvch a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "LPE", "LPE", dsRep, header_n, "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30355":
                #region MPE
                header_n = "Magnetic Particle Exmination Report";
                mq4 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='043'", "");
                if (mq4 == "0") { mq4 = "-"; }
                SQuery = "select '" + mq4 + "' as IS_NO,a.*,trim(b.aname) as customer from inspvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "MPE", "MPE", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30357":
                header_n = "Positive Material Identification Report";
                mq4 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='044'", "");
                if (mq4 == "0") { mq4 = "-"; }
                dt1 = new DataTable();
                mq0 = "select a.obsv6,a.obsv16 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt1.Rows.Count > 0)
                {
                    mq5 = dt1.Rows[0]["obsv6"].ToString().Trim();
                    mq6 = dt1.Rows[0]["obsv16"].ToString().Trim();
                    if (mq5.Length == 1) { mq5 = "--"; }
                    if (mq6.Length == 1) { mq6 = "--"; }
                }
                SQuery = "select a.*,trim(b.aname) as customer,'" + mq5 + "' as open_field1_req,'" + mq6 + "' as open_field2_req,'" + mq4 + "' as IS_NO from inspvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno>1 order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "PMI", "PMI", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30359":
                header_n = "Dual Plate Check Valve";
                mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq9 = ""; mq10 = ""; mq1 = ""; mq2 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='045'", "");
                if (mq4 == "0") { mq4 = "-"; }
                mq5 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv18 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv18");
                if (mq5 == "0") { mq5 = "--"; }
                mq9 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv12 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv12");
                if (mq9 == "0") { mq9 = "--"; }
                mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv13 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv13");
                if (mq10 == "0") { mq10 = "--"; }
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv14 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv14");
                if (mq1 == "0") { mq1 = "--"; }
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv15 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv15");
                if (mq2 == "0") { mq2 = "--"; }

                mq6 = "select OBSV1 as req,OBSV4 as outer_dia,OBSV5 as face,OBSV6 as inner_dia,OBSV21 as valve_min,OBSV8 as raised,OBSV9 as height,OBSV10 as pcd,OBSV12 as tapped_hole,OBSV13 as tapped_size,OBSV14 as thru_holes,OBSV15 as thru_size,OBSV16 as tot_holes,OBSV22 as facing,OBSV17 as grid_finish,OBSV18 as grid_,OBSV19 as remarks from inspvch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + barCode + "' and srno='2'";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6);
                if (dt1.Rows.Count > 0)
                {
                    mq7 = " ,'" + dt1.Rows[0]["req"].ToString().Trim() + "' as req,'" + dt1.Rows[0]["outer_dia"].ToString().Trim() + "' as outer_dia ,'" + dt1.Rows[0]["face"].ToString().Trim() + "' as face,'" + dt1.Rows[0]["inner_dia"].ToString().Trim() + "' as inner_dia,'" + dt1.Rows[0]["valve_min"].ToString().Trim() + "' as valve_min,'" + dt1.Rows[0]["raised"].ToString().Trim() + "' as raised,'" + dt1.Rows[0]["height"].ToString().Trim() + "' as height,'" + dt1.Rows[0]["pcd"].ToString().Trim() + "' as pcd,'" + dt1.Rows[0]["tapped_hole"].ToString().Trim() + "' as tapped_hole,'" + dt1.Rows[0]["tapped_size"].ToString().Trim() + "' as tapped_size,'" + dt1.Rows[0]["thru_holes"].ToString().Trim() + "' as thru_holes,'" + dt1.Rows[0]["thru_size"].ToString().Trim() + "' as thru_size,'" + dt1.Rows[0]["tot_holes"].ToString().Trim() + "' as tot_holes,'" + dt1.Rows[0]["facing"].ToString().Trim() + "' as facing,'" + dt1.Rows[0]["grid_finish"].ToString().Trim() + "' as grid_finish,'" + dt1.Rows[0]["grid_"].ToString().Trim() + "' as grid_,'" + dt1.Rows[0]["remarks"].ToString().Trim() + "' as remarks";
                }
                SQuery = "select a.*,trim(b.aname) as customer,'" + mq5 + "' as grid_heading,'" + mq4 + "' as IS_NO" + mq7 + ",'" + mq9 + "' as gridh_tappedno,'" + mq10 + "' as gridh_size,'" + mq1 + "' as gridh_thruno,'" + mq2 + "' as gridh_thrusize from inspvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno>2 order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "DP", "DP", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30361":
                header_n = "BT / BD / BF";
                mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq1 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='046'", "");
                if (mq4 == "0") { mq4 = "-"; }
                mq5 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv19 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv19");
                if (mq5 == "0") { mq5 = "--"; }
                mq8 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv14 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv14");
                if (mq8 == "0") { mq8 = "--"; }
                mq9 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.equip_id from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "equip_id");
                if (mq9 == "0") { mq9 = "--"; }
                mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.finish from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "finish");
                if (mq10 == "0") { mq10 = "--"; }
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "select a.obsv17 from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno='1'", "obsv17");
                if (mq1 == "0") { mq1 = "--"; }

                mq6 = "select OBSV1 as req,OBSV4 as a,OBSV5 as b,OBSV6 as c,OBSV7 as ØD,OBSV8 as e,OBSV9 as Øf,OBSV10 as g,OBSV11 as i,OBSV12 as j,OBSV13 as pcd,OBSV14 as tapped_holes,EQUIP_ID as depth,FINISH as thru_holes,OBSV17 as dia,OBSV18 as tot_holes,OBSV22 as facing,OBSV19 as grid_,OBSV21 as remarks from inspvch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + barCode + "' and srno='2'";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6);
                if (dt1.Rows.Count > 0)
                {
                    mq7 = " ,'" + dt1.Rows[0]["req"].ToString().Trim() + "' as req,'" + dt1.Rows[0]["a"].ToString().Trim() + "' as a ,'" + dt1.Rows[0]["b"].ToString().Trim() + "' as b,'" + dt1.Rows[0]["c"].ToString().Trim() + "' as c,'" + dt1.Rows[0]["ØD"].ToString().Trim() + "' as ØD,'" + dt1.Rows[0]["e"].ToString().Trim() + "' as e,'" + dt1.Rows[0]["Øf"].ToString().Trim() + "' as Øf,'" + dt1.Rows[0]["g"].ToString().Trim() + "' as g,'" + dt1.Rows[0]["i"].ToString().Trim() + "' as i,'" + dt1.Rows[0]["j"].ToString().Trim() + "' as j,'" + dt1.Rows[0]["pcd"].ToString().Trim() + "' as pcd,'" + dt1.Rows[0]["tapped_holes"].ToString().Trim() + "' as tapped_holes,'" + dt1.Rows[0]["depth"].ToString().Trim() + "' as depth,'" + dt1.Rows[0]["thru_holes"].ToString().Trim() + "' as thru_holes,'" + dt1.Rows[0]["dia"].ToString().Trim() + "' as dia,'" + dt1.Rows[0]["tot_holes"].ToString().Trim() + "' as tot_holes,'" + dt1.Rows[0]["facing"].ToString().Trim() + "' as facing,'" + dt1.Rows[0]["grid_"].ToString().Trim() + "' as grid_,'" + dt1.Rows[0]["remarks"].ToString().Trim() + "' as remarks";
                }
                SQuery = "select a.*,trim(b.aname) as customer,'" + mq5 + "' as grid_heading,'" + mq4 + "' as IS_NO" + mq7 + ",'" + mq8 + "' as gridh_tappholes,'" + mq9 + "' as gridh_sizex,'" + mq10 + "' as gridh_thruholes,'" + mq1 + "' as gridh_diathru from inspvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' and a.srno>2 order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "BT_BD_BF", "BT_BD_BF", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;
            case "F30362":
                header_n = "Pickling And Passivation Report";
                mq4 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='048'", "");
                if (mq4 == "0") { mq4 = "-"; }
                SQuery = "SELECT A.vchnum,TO_CHAR(A.vchdate,'dd/MM/yyyy') AS VCHDATE,A.TITLE,A.ACODE,C.ANAME,A.ICODE,B.INAME,A.CPARTNO,A.GRADE,A.OBSV20,A.COL3,A.COL5,A.COL6,TRIM(A.OBSV8) AS OBSV8,TRIM(A.OBSV9) AS OBSV9,TRIM(A.OBSV11) AS OBSV11,TRIM(A.OBSV13) AS OBSV13,A.COL4,A.LINKFILE,TO_CHAR(A.DOC_DT,'DD/MM/YYYY') AS DOC_DT,A.COL2,TRIM(A.OBSV2) AS OBSV2,TRIM(A.OBSV10) AS OBSV10,TRIM(A.OBSV12) AS OBSV12,TRIM(A.OBSV14) AS OBSV14,TRIM(A.OBSV21) AS OBSV21,A.DTR1,TRIM(A.OBSV1) AS OBSV1,TRIM(A.OBSV5) AS OBSV5,A.COL1,TRIM(A.OBSV6) AS OBSV6,TRIM(A.OBSV3) AS OBSV3,TRIM(A.OBSV4) AS OBSV4,'" + mq4 + "' AS IOS_NO,TRIM(A.OBSV19) AS OBSV19,TRIM(A.OBSV16) AS OBSV16 FROM WB_INSPVCH A ,ITEM B, FAMST C WHERE TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Pick_Pass", "Pick_Pass", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30363":
                header_n = "Surface Preparation,Painting And Marking Report";
                mq4 = ""; mq5 = "";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='047'", "");
                if (mq4 == "0") { mq4 = "-"; }
                mq5 = fgen.seek_iname(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(obsv1,'-',null)||',')).extract('//text()').extract('//text()'),',') as tagno from wb_inspvch a where a.branchcd||a.type||TRIM(A.VCHNUM)||TO_CHAR(a.vchdate,'DD/MM/YYYY')='" + barCode + "' order by a.srno", "tagno");
                SQuery = "select a.*,b.aname,c.iname,'" + mq4 + "' AS IOS_NO,'" + mq5 + "' as tagno from wb_inspvch a, famst b,item c  where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||TRIM(A.VCHNUM)||TO_CHAR(a.vchdate,'DD/MM/YYYY')='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Surf_prep", "Surf_prep", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F79155": //MAIN CODE FOR 5 TEST CERTIFICATE WITH NEW LOGIC
                #region
                mq1 = ""; mq2 = ""; mq3 = ""; mq4 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");//tag value
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");// Third char
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//mbr
                if (frm_uname.Substring(0, 2) == "16" || frm_uname.Substring(0, 2) == "18")
                {
                    cond = " and a.acode='" + frm_uname + "'";
                }
                else
                {
                    cond = "";
                }
                switch (mq3.ToUpper())
                {
                    case "G":
                        header_n = "DOUBLE ECCENTRIC BUTTERFLY VALVE";
                        frm_rptName = "BT_Test_Certificate"; //rpt name
                        mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='037'", "");
                        if (mq4 == "0") { mq4 = "-"; }
                        SQuery = "SELECT '" + header_n + "' as header,'" + mq4 + "' as IS_NO,b.aname as customer,a.*,'Seat' as Seat FROM INSPVCH a,famst b WHERE trim(a.acode)=trim(b.acode) and a.branchcd='" + mq2 + "' and a.type='83' and UPPER(trim(a.omax))='" + mq1.Trim().ToUpper() + "' " + cond + " order by a.srno";
                        break;
                    case "H":
                        header_n = "TRIPLE ECCENTRIC BUTTERFLY VALVE";
                        frm_rptName = "BT_Test_Certificate"; //rpt name
                        mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='039'", "");
                        if (mq4 == "0") { mq4 = "-"; }
                        SQuery = "SELECT '" + header_n + "' as header,'" + mq4 + "' as IS_NO,b.aname as customer,a.*,'Disc Seat/Seat Ring' as Seat FROM INSPVCH a,famst b WHERE trim(a.acode)=trim(b.acode) and a.branchcd='" + mq2 + "' and a.type='83' and UPPER(trim(a.omax))='" + mq1.Trim().ToUpper() + "' " + cond + " order by a.srno";
                        break;
                    case "A":
                        header_n = "BALANCING VALVE";
                        frm_rptName = "BV_Test_Certificate"; //rpt name
                        mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='041'", "");
                        if (mq4 == "0") { mq4 = "-"; }
                        SQuery = "SELECT distinct '" + header_n + "' as header,'" + mq4 + "' as IS_NO,b.aname as customer,trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,c.kcode,c.mcname,c.dia,c.gauge,c.mktrate,c.setrate,a.* FROM INSPVCH a,famst b,KNITVCH C  WHERE  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||a.srno=trim(c.branchcd)||trim(c.type)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy')||c.srno  AND  trim(a.acode)=trim(b.acode) and a.branchcd='" + mq2 + "' and a.type='93' and UPPER(trim(a.omax))='" + mq1.Trim().ToUpper() + "' " + cond + " order by a.srno,c.kcode";
                        break;
                    case "D":
                        header_n = "DUAL PLATE CHECK VALVE";
                        frm_rptName = "DPCV_Test_Certificate";//rpt name
                        mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='040'", "");
                        if (mq4 == "0") { mq4 = "-"; }
                        SQuery = "SELECT '" + header_n + "' as header,'" + mq4 + "' as IS_NO,b.aname as customer,a.*,'Seat' as Seat FROM INSPVCH a,famst b WHERE trim(a.acode)=trim(b.acode) and a.branchcd='" + mq2 + "' and a.type='83' and UPPER(trim(a.omax))='" + mq1.Trim().ToUpper() + "' " + cond + " order by a.srno";
                        break;
                    case "B":
                        header_n = "BUTTERFLY CONCENTRIC VALVE";
                        frm_rptName = "BF_Test_Certificate";//rpt name
                        mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(col2)) as col2 from multivch where branchcd!='DD' and type='IS' and icode='038'", "");
                        if (mq4 == "0") { mq4 = "-"; }
                        SQuery = "SELECT '" + header_n + "' as header,'" + mq4 + "' as IS_NO,b.aname as customer,a.*,'Seat' as Seat FROM INSPVCH a,famst b WHERE trim(a.acode)=trim(b.acode) and a.branchcd='" + mq2 + "' and a.type='83' and UPPER(trim(a.omax))='" + mq1.Trim().ToUpper() + "' " + cond + " order by a.srno";
                        break;
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    pdfView = "Y";
                    dt.TableName = "Prepcur";
                    mq0 = "SELECT OMAX AS OMAX_H ,OMIN AS OMIN_H,MFGDATE AS MFG_H,EXPDATE AS EXP_H,FINISH AS FIN_H, RESULT AS RES_H FROM INSPVCH WHERE SRNO='0' AND TRIM(branchcd)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dt.Rows[0]["BRANCHCD"].ToString() + dt.Rows[0]["TYPE"].ToString() + dt.Rows[0]["VCHNUM"].ToString() + Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString()).ToString("dd/MM/yyyy") + "'";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dt1.TableName = "Prephead";
                    dsRep.Tables.Add(dt1);
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, mq2, frm_rptName, frm_rptName, dsRep, header_n, "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F30114":
                header_n = "Work Order Production Report";
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select '" + header_n + "' as header,A.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.naration as rmk,nvl(a.lmd,0) as lmd,nvl(a.bcd,0) as bcd,b.iname,b.cpartno as part,a.exc_time as driver,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS rejn,SUM(nvl(A.A1,0)) as a1,SUM(nvl(A.A2,0)) a2,SUM(nvl(A.A3,0)) as a3,SUM(nvl(A.A4,0)) as a4,SUM(nvl(A.A5,0)) as a5,SUM(nvl(A.A6,0)) as a6,SUM(nvl(A.A7,0)) as a7,SUM(nvl(A.A8,0)) as a8,SUM(nvl(A.A9,0)) as a9,SUM(nvl(A.A10,0)) as a10,SUM(nvl(A.A11,0)) as a11,SUM(nvl(A.A12,0)) as a12,SUM(nvl(A.A13,0)) as a13,SUM(nvl(A.A14,0)) as a14,SUM(nvl(A.A15,0)) as a15,SUM(nvl(A.A16,0)) as a16,SUM(nvl(A.A17,0)) as a17,SUM(nvl(A.A18,0)) as a18,SUM(nvl(A.A19,0)) as a19,SUM(nvl(A.A20,0)) as a20,SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0)) as oth,sum(nvl(total,0)) as total,sum(nvl(a.un_melt,0)) as prdn_tgt_shot,sum(nvl(a.noups,0)) as act_prd_shot,nvl(a.iqtyin,0) as ok_prd,a.ename,a.var_code as shift,SUM(nvl(a.NUM1,0)) AS num1,SUM(nvl(a.NUM2,0)) AS  num2,SUM(nvl(a.NUM3,0)) AS num3,SUM(nvl(a.NUM4,0)) AS num4 ,SUM(nvl(a.NUM5,0)) AS  num5,SUM(nvl(a.NUM6,0)) AS num6,SUM(nvl(a.NUM7,0)) AS num7,SUM(nvl(a.NUM8,0)) AS num8,SUM(nvl(a.NUM9,0)) AS num9,SUM(nvl(a.NUM10,0)) AS num10,SUM(nvl(a.NUM11,0)) AS num11,SUM(nvl(a.NUM12,0)) AS num12,SUM(nvl(A.num13,0))+SUM(nvl(A.num14,0))+SUM(nvl(A.num15,0)) as oth1,a.oee from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq0 + "' group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.icode,a.naration,b.iname,b.cpartno,a.exc_time,a.ename,a.var_Code ,nvl(a.lmd,0),nvl(a.bcd,0) ,a.iqtyin,a.oee order by a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("HH0", typeof(string));
                    dt.Columns.Add("HH1", typeof(string));
                    dt.Columns.Add("HH2", typeof(string));
                    dt.Columns.Add("HH3", typeof(string));
                    dt.Columns.Add("HH4", typeof(string));
                    dt.Columns.Add("HH5", typeof(string));
                    dt.Columns.Add("HH6", typeof(string));
                    dt.Columns.Add("HH7", typeof(string));
                    dt.Columns.Add("HH8", typeof(string));
                    dt.Columns.Add("HH9", typeof(string));
                    dt.Columns.Add("HH10", typeof(string));
                    dt.Columns.Add("HH11", typeof(string));
                    dt.Columns.Add("HH12", typeof(string));
                    dt.Columns.Add("HH13", typeof(string));
                    dt.Columns.Add("HH14", typeof(string));
                    dt.Columns.Add("HH15", typeof(string));
                    dt.Columns.Add("HH16", typeof(string));
                    dt.Columns.Add("HH17", typeof(string));
                    dt.Columns.Add("HH18", typeof(string));
                    dt.Columns.Add("HH19", typeof(string));

                    //FOR DOWN TIME
                    dt.Columns.Add("DH0", typeof(string));
                    dt.Columns.Add("DH1", typeof(string));
                    dt.Columns.Add("DH2", typeof(string));
                    dt.Columns.Add("DH3", typeof(string));
                    dt.Columns.Add("DH4", typeof(string));
                    dt.Columns.Add("DH5", typeof(string));
                    dt.Columns.Add("DH6", typeof(string));
                    dt.Columns.Add("DH7", typeof(string));
                    dt.Columns.Add("DH8", typeof(string));
                    dt.Columns.Add("DH9", typeof(string));
                    dt.Columns.Add("DH10", typeof(string));
                    dt.Columns.Add("DH11", typeof(string));
                    dt.Columns.Add("DH12", typeof(string));

                    dt1 = new DataTable();
                    dt2 = new DataTable(); //rej headings
                    int k = 0, rownum = 0, rej_count = 0, downtime_count = 0;

                    if (mq0.Substring(2, 2) == "90")
                    {
                        mq1 = " and substr(trim(type1),1,1) in ('0','1')";
                    }
                    else if (mq0.Substring(2, 2) == "91")
                    {
                        mq1 = " and type1 like '2%'";
                    }
                    else
                    {
                        mq1 = " and type1 like '6%'";
                    }
                    if (frm_cocd == "JEPL")
                    {
                        k = 20;
                        rownum = 21;
                        frm_rptName = "mold_prd_qa_JEPL";
                    }
                    else
                    {
                        k = 12;
                        rownum = 13;
                        frm_rptName = "mold_prd_qa";
                    }
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "select code,name from (SELECT trim(type1) as code,name FROM TYPE WHERE ID='8' " + mq1 + " order by type1) where rownum<" + rownum);
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, "select code,name from (SELECT trim(type1) as code,name FROM TYPE WHERE ID='4' " + mq1 + " order by type1) where rownum<13");
                    rej_count = dt1.Rows.Count; downtime_count = dt2.Rows.Count;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        for (int i = 0; i < k; i++)
                        {
                            try
                            {
                                if (i < rej_count)
                                {
                                    dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                                }
                                else
                                {
                                    dt.Rows[l]["HH" + i] = "";
                                }
                            }
                            catch { }
                            try
                            {
                                if (i < downtime_count)
                                {
                                    dt.Rows[l]["DH" + i] = dt2.Rows[i]["name"].ToString();
                                }
                                else
                                {
                                    dt.Rows[l]["DH" + i] = "";
                                }
                            }
                            catch { }
                        }
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "mold_prd_qa", frm_rptName, dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F30114a":
                #region
                header_n = "Work Order Production Report";
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                //SQuery = "select '" + header_n + "' as header,A.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.naration as rmk,nvl(a.lmd,0) as lmd,nvl(a.bcd,0) as bcd,b.iname,b.cpartno as part,a.exc_time as driver,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS rejn,SUM(nvl(A.A1,0)) as a1,SUM(nvl(A.A2,0)) a2,SUM(nvl(A.A3,0)) as a3,SUM(nvl(A.A4,0)) as a4,SUM(nvl(A.A5,0)) as a5,SUM(nvl(A.A6,0)) as a6,SUM(nvl(A.A7,0)) as a7,SUM(nvl(A.A8,0)) as a8,SUM(nvl(A.A9,0)) as a9,SUM(nvl(A.A10,0)) as a10,SUM(nvl(A.A11,0)) as a11,SUM(nvl(A.A12,0)) as a12, SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0)) as oth,sum(nvl(total,0)) as total,sum(nvl(a.un_melt,0)) as prdn_tgt_shot,sum(nvl(a.noups,0)) as act_prd_shot,nvl(a.iqtyin,0) as ok_prd,a.ename,a.var_code as shift,SUM(nvl(a.NUM1,0)) AS  num1 ,SUM(nvl(a.NUM2,0)) AS  num2,SUM(nvl(a.NUM3,0)) AS num3,SUM(nvl(a.NUM4,0)) AS num4 ,SUM(nvl(a.NUM5,0)) AS  num5,SUM(nvl(a.NUM6,0)) AS num6,SUM(nvl(a.NUM7,0)) AS num7,SUM(nvl(a.NUM8,0)) AS num8,SUM(nvl(a.NUM9,0)) AS num9,SUM(nvl(a.NUM10,0)) AS num10,SUM(nvl(a.NUM11,0)) AS num11,SUM(nvl(a.NUM12,0)) AS num12,SUM(nvl(A.num13,0))+SUM(nvl(A.num14,0))+SUM(nvl(A.num15,0)) as oth1,a.oee from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq0 + "' group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.icode,a.naration,b.iname,b.cpartno,a.exc_time,a.ename,a.var_Code ,nvl(a.lmd,0),nvl(a.bcd,0) ,a.iqtyin,a.oee order by a.icode"; //old
                SQuery = "select '" + header_n + "' as header,A.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.naration as rmk,nvl(a.lmd,0) as lmd,nvl(a.bcd,0) as bcd,b.iname,b.cpartno as part,a.exc_time as driver,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS rejn,SUM(nvl(A.A1,0)) as a1,SUM(nvl(A.A2,0)) a2,SUM(nvl(A.A3,0)) as a3,SUM(nvl(A.A4,0)) as a4,SUM(nvl(A.A5,0)) as a5,SUM(nvl(A.A6,0)) as a6,SUM(nvl(A.A7,0)) as a7,SUM(nvl(A.A8,0)) as a8,SUM(nvl(A.A9,0)) as a9,SUM(nvl(A.A10,0)) as a10,SUM(nvl(A.A11,0)) as a11,SUM(nvl(A.A12,0)) as a12, SUM(nvl(A.A13,0)) as a13,SUM(nvl(A.A14,0)) as a14,SUM(nvl(A.A15,0)) as a15,SUM(nvl(A.A16,0)) as a16,SUM(nvl(A.A17,0)) as a17,SUM(nvl(A.A18,0)) as a18,SUM(nvl(A.A19,0)) as a19,SUM(nvl(A.A20,0)) as a20,sum(nvl(total,0)) as total,sum(nvl(a.un_melt,0)) as prdn_tgt_shot,sum(nvl(a.noups,0)) as act_prd_shot,nvl(a.iqtyin,0) as ok_prd,a.ename,a.var_code as shift,SUM(nvl(a.NUM1,0)) AS  num1 ,SUM(nvl(a.NUM2,0)) AS  num2,SUM(nvl(a.NUM3,0)) AS num3,SUM(nvl(a.NUM4,0)) AS num4 ,SUM(nvl(a.NUM5,0)) AS  num5,SUM(nvl(a.NUM6,0)) AS num6,SUM(nvl(a.NUM7,0)) AS num7,SUM(nvl(a.NUM8,0)) AS num8,SUM(nvl(a.NUM9,0)) AS num9,SUM(nvl(a.NUM10,0)) AS num10,SUM(nvl(a.NUM11,0)) AS num11,SUM(nvl(a.NUM12,0)) AS num12,SUM(nvl(A.num13,0))+SUM(nvl(A.num14,0))+SUM(nvl(A.num15,0)) as oth1,a.oee from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq0 + "' group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.icode,a.naration,b.iname,b.cpartno,a.exc_time,a.ename,a.var_Code ,nvl(a.lmd,0),nvl(a.bcd,0) ,a.iqtyin,a.oee order by a.icode";//new
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("HH0", typeof(string));
                    dt.Columns.Add("HH1", typeof(string));
                    dt.Columns.Add("HH2", typeof(string));
                    dt.Columns.Add("HH3", typeof(string));
                    dt.Columns.Add("HH4", typeof(string));
                    dt.Columns.Add("HH5", typeof(string));
                    dt.Columns.Add("HH6", typeof(string));
                    dt.Columns.Add("HH7", typeof(string));
                    dt.Columns.Add("HH8", typeof(string));
                    dt.Columns.Add("HH9", typeof(string));
                    dt.Columns.Add("HH10", typeof(string));
                    dt.Columns.Add("HH11", typeof(string));
                    dt.Columns.Add("HH12", typeof(string));
                    dt.Columns.Add("HH13", typeof(string));
                    dt.Columns.Add("HH14", typeof(string));
                    dt.Columns.Add("HH15", typeof(string));
                    dt.Columns.Add("HH16", typeof(string));
                    dt.Columns.Add("HH17", typeof(string));
                    dt.Columns.Add("HH18", typeof(string));
                    dt.Columns.Add("HH19", typeof(string));

                    //FOR DOWN TIME
                    dt.Columns.Add("DH0", typeof(string));
                    dt.Columns.Add("DH1", typeof(string));
                    dt.Columns.Add("DH2", typeof(string));
                    dt.Columns.Add("DH3", typeof(string));
                    dt.Columns.Add("DH4", typeof(string));
                    dt.Columns.Add("DH5", typeof(string));
                    dt.Columns.Add("DH6", typeof(string));
                    dt.Columns.Add("DH7", typeof(string));
                    dt.Columns.Add("DH8", typeof(string));
                    dt.Columns.Add("DH9", typeof(string));
                    dt.Columns.Add("DH10", typeof(string));
                    dt.Columns.Add("DH11", typeof(string));
                    dt.Columns.Add("DH12", typeof(string));

                    dt1 = new DataTable();
                    dt2 = new DataTable(); //rej headings
                    int k = 20; //down time HEADINGS
                    int m = 0;
                    if (mq0.Substring(2, 2) == "90")
                    {
                        mq1 = " and substr(trim(type1),1,1) in ('0','1')";
                    }
                    else if (mq0.Substring(2, 2) == "91")
                    {
                        mq1 = " and type1 like '2%'";
                    }
                    else
                    {
                        mq1 = " and type1 like '6%'";
                    }
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "select code,name from (SELECT trim(type1) as code,name FROM TYPE WHERE ID='8' " + mq1 + " order by type1) where rownum<21");
                    //    dt2 = fgen.getdata(frm_qstr, frm_cocd, "select code,name from (SELECT trim(type1) as code,name FROM TYPE WHERE ID='4' " + mq1 + " order by type1) where rownum<13");
                    m = dt1.Rows.Count;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {

                            for (int i = 0; i < k; i++)
                            {
                                if (i < m)
                                {
                                    dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                                    //  dt.Rows[l]["DH" + i] = dt2.Rows[i]["name"].ToString();
                                }
                                else
                                {
                                    dt.Rows[l]["HH" + i] = "-";
                                }
                            }
                        }
                        catch { };
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    //  Print_Report_BYDS(frm_cocd, frm_mbr, "mold_prd_qa", "mold_prd_qa", dsRep, "");
                }
                else
                {
                    //   data_found = "N";
                }
                #endregion
                break;
            case "F30233":
                // SQuery = "select '" + fromdt + "' AS FRMDATE,'" + todt + "' AS TODATE,b.aname,b.addr1,b.addr2,b.addr3,c.iname,a.pname,c.unit as itm_unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,sum(nvl(a.iqtyin,0)) as rcvd,sum(nvl(a.iqty_chl,0)) as chlqty,sum(nvl(a.acpt_ud,0)) as acpt,sum(nvl(a.rej_rw,0)) as rej,a.purpose,a.desc_,a.naration,a.genum,a.gedate,a.ponum,a.podate,a.invno,a.invdate,a.unit,a.iweight,a.iamount,a.irate from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + xprdRange + " and a.inspected='Y' and a.store IN ('Y','N') group by b.aname,b.addr1,b.addr2,b.addr3,c.iname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.acode,a.icode,a.purpose,a.desc_,a.naration,a.genum,a.gedate,a.ponum,a.podate,a.invno,a.invdate,a.unit,a.iweight,a.iamount,a.irate,a.pname,c.unit order by a.vchnum asc";//with sum
                SQuery = "select '" + fromdt + "' AS FRMDATE,'" + todt + "' AS TODATE,a.qcdate,b.aname,b.addr1,b.addr2,b.addr3,c.iname,a.pname,c.unit as itm_unit,a.vchnum,a.type,to_char(a.vchdate,'yyyymmdd') as vdd,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,c.cpartno,nvl(a.iqtyin,0) as rcvd,nvl(a.iqty_chl,0) as chlqty,nvl(a.acpt_ud,0) as acpt,nvl(a.rej_rw,0) as rej,a.purpose,a.desc_,a.naration,a.genum,a.gedate,a.ponum,a.podate,a.invno,a.invdate,a.unit,a.iweight,a.iamount,a.irate,to_date(a.qc_date,'dd/mm/yyyy') as qcdateee,a.qc_date as qcdate1 from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + xprdRange + " and a.inspected='Y' and a.store IN ('Y','N') order by a.qcdate,a.vchdate,a.type,a.vchnum,a.icode";//without sum
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Inspection_Register", "std_Inspection_Register", dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;
            case "F70201":
            case "F70203":
                #region paper inspection report
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //fstr              
                SQuery = "select trim(b.aname) as suppluer,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.mrrnum,to_char(a.mrrdate,'dd/mm/yyyy') as mrrdate,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.mrrtype,a.acode,a.icode,a.srno,a.reelno,a.reel_rej,a.reelwt,a.reeldia as cm_size,a.mrrsize as cm_obsv1,a.mrrgsm as actual_Std,a.actgsm1 as gsm_obsv1,a.actgsm2 as gsm_obsv2,a.avggsm as gsm_avg,a.bs1,a.bs2,a.bs3,a.bs4,a.avgbs,a.bfactor,a.moisture,a.stifness,a.colbvalue,a.fold,a.phvalue,SUBSTR(TRIM(a.caliper),1,6) as shade,a.appearance,a.remarks,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt,a.rct1,a.rct2,a.rct3,a.actsize,a.co_re_el,substr(trim(a.icode),1,4) as subgrp,c.mat4,c.mat5,c.mat6,c.mat8 AS MAT7,c.mqty9  from papinsp a,famst b,item c where trim(a.acode)=trim(b.acode) and substr(trim(a.icode),1,4)=trim(c.icode) and length(trim(c.icode))=4 and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "' ORDER BY A.reelno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//main dt
                dt.TableName = "prepcur";
                dsRep = new DataSet();
                dsRep.Tables.Add(dt);
                dt1 = new DataTable();
                dtm = new DataTable();
                ///================            
                dtm.Columns.Add("wt_REcvd", typeof(double));
                dtm.Columns.Add("reels_Recvd", typeof(double));
                dtm.Columns.Add("itemcode", typeof(string));
                dtm.Columns.Add("itemname", typeof(string));
                int cnt = 0;
                if (dt.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dt);
                    dt4 = new DataTable();
                    dt4 = view1.ToTable(true, "mrrnum", "mrrdate");
                    dr1 = null; mq2 = "";
                    foreach (DataRow dr2 in dt4.Rows)
                    {
                        DataView view2 = new DataView(dt4, "mrrnum='" + dr2["mrrnum"].ToString().Trim() + "' and mrrdate='" + dr2["mrrdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt5 = new DataTable();
                        dt5 = view2.ToTable();

                        db1 = 0; db2 = 0; db3 = 0;
                        for (int i = 0; i < dt5.Rows.Count; i++)
                        {
                            mq2 = "select sum(a.reelwin) as wt_REcvd,count(a.srno) as reels_Recvd,a.icode as itemcode,trim(b.iname) as itemname from reelvch a,item b where trim(a.icode)=trim(b.icode) and a.vchnum='" + dt5.Rows[0]["mrrnum"].ToString().Trim() + "' and to_char(a.vchdate,'dd/mm/yyyy')='" + dt5.Rows[0]["mrrdate"].ToString().Trim() + "'  group by a.icode,trim(b.iname)";
                            dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        }

                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            dr1 = dtm.NewRow();
                            dr1["wt_REcvd"] = dt1.Rows[j]["wt_REcvd"].ToString().Trim();
                            dr1["reels_Recvd"] = dt1.Rows[j]["reels_Recvd"].ToString().Trim();
                            dr1["itemcode"] = dt1.Rows[j]["itemcode"].ToString().Trim();
                            dr1["itemname"] = dt1.Rows[j]["itemname"].ToString().Trim();
                            if (cnt < 10)
                            {
                                dtm.Rows.Add(dr1);
                            }
                            cnt = dtm.Rows.Count;
                        }


                    }

                    dtm.TableName = "subreport";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pap_Insp", "std_Pap_Insp", dsRep, "", "Y");
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
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
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