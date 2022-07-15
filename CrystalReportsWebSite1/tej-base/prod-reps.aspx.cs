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

public partial class prod_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, pdfView = "";
    string mq2 = "", mq3 = "", DateRange = "", frm_IndType = "";
    double db1, db2, db3;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet(); DataSet ds;
    FileStream FilStr = null; BinaryReader BinRed = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    frm_IndType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");

            }
            if (!Page.IsPostBack)
            {
                pdfView = "Y";
                printCrpt(hfhcid.Value);
                CrystalReportViewer1.Focus();
            }
        }
        //catch (Exception ex)
        {
            //fgen.FILL_ERR(ex.Message);
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
        switch (iconID)
        {
            case "F39201":
                #region Issue reqd
                SQuery = "select 'Material Issue Request' as header,'Material Issue Request' as h1,'Issue Agst Job Card' as h2, C.NAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM wb_iss_req A, ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND C.ID='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "STD_ISS_rq", "STD_ISS_rq", dsRep, "Store Issue Request");
                #endregion
                break;
            case "F39211":
                #region job work  reqd
                SQuery = "select 'Job Work Request' as header,'Job Work Request' as h1,'Issue Agst Job Card' as h2, C.ANAME AS DPT_NAME,I.INAME,I.CPARTNO,I.UNIT AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM wb_iss_req A, ITEM I ,famst C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.acode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                }
                Print_Report_BYDS(frm_cocd, frm_mbr, "STD_CHL_rq", "STD_CHL_rq", dsRep, "Store Issue Request");
                #endregion
                break;

            //prodn entry
            case "F50114":

            case "F39119":
                #region prodn
                dsRep = new DataSet();
                dt = new DataTable();
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0011", "OPT_ENABLE");
                SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,A.*,B.INAME,B.CPARTNO,B.UNIT as iunit FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum,A.MORDER";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }

                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select aname,addr1,addr2,addr3,staten,email,website,gst_no from famst where trim(acode)='120000'");
                    dt.TableName = "FAMST";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_prod", "std_prod", dsRep, "Production Note Report");
                }
                #endregion
                break;

            case "F20132":
                // Gate Inward Register
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,A.IQTY_CHL,A.NARATION,I.INAME,I.UNIT,I.CPARTNO AS PARTNO,F.ANAME,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,A.MODE_TPT,A.DESC_ FROM IVOUCHERP A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + xprdRange + " ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_GE_REG", "std_GE_REG", dsRep, "Gate Inward Register");
                }
                break;

            case "F20133":
                // Gate Outward Register
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,A.IQTY_CHL,A.NARATION,I.INAME,I.CPARTNO AS PARTNO,I.UNIT,F.ANAME,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,a.mode_tpt,a.desc_ FROM IVOUCHERP A,item I,famst f WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='2G' AND A.VCHDATE " + xprdRange + " ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_GE_OutWard_REG", "std_GE_OutWard_REG", dsRep, "Gate Outward Register");
                }
                break;
            case "F35101":
                if (frm_IndType == "12")
                {
                    //SQuery = "select trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,'" + header_n + "' AS HEADER,null as pordno,null as pordt,null as saleid,null as ppcdt, b.aname,c.iname,c.cpartno,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,a.status,a.convdate,substr(A.convdate,5,6)||'  '||substr(A.convdate,11,20)  as orderno,a.srno,nvl(a.qty,0) as qty,a.remarks as peqty,a.col1,a.col2 as spec,a.col3 as particulr,a.col5 as bom_Qty,a.col6 as extra_qty,a.col7 as read_Qty, a.col9 as erpcde,a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,a.col14,a.col17,a.col24,a.itate,a.remarks,a.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_Dt,nvl(c.imagef,'-') as imagef,trim(c.cdrgno) as refno,C.UNIT from costestimate a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.vchnum,a.srno";
                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,'JOB ORDER' AS HEADER,s.pordno,s.porddt,s.busi_expect as saleid, b.aname,c.iname,c.cpartno,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,a.status,a.convdate,substr(A.convdate,5,6)||'  '||substr(A.convdate,11,20)  as orderno,a.comments as rmk,nvl(a.app_by,'-') as appby,a.srno,nvl(a.qty,0) as qty,a.remarks as peqty,a.col1,a.col2 as spec,a.col3 as particulr,a.col5 as bom_Qty,a.col6 as extra_qty,a.col7 as read_Qty, a.col9 as erpcde,a.enqno,to_char(a.enqdt,'dd/mm/yyyy') as enqdt,a.col14,a.col17,a.col24,a.itate,a.remarks,a.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_Dt,nvl(c.imagef,'-') as imagef,trim(c.cdrgno) as refno,C.UNIT,to_char(d.vchdate,'dd/mm/yyyy') as ppcdt from costestimate a left outer join somas s on trim(a.convdate)=trim(s.branchcd)||trim(s.type)||trim(s.ordno)||to_char(s.orddt,'dd/mm/yyyy') ,famst b,item c,INSPMST D where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.vchnum,a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt = fgen.addBarCode(dt, "fstr", true);
                        dt.TableName = "Prepcur";
                        dt.Columns.Add("ImgPath", typeof(string));
                        dt.Columns.Add("jcImg", typeof(System.Byte[]));
                        FileStream FilStr;
                        BinaryReader BinRed;
                        foreach (DataRow dr in dt.Rows)
                        {
                            #region
                            dr["ImgPath"] = "-";
                            try
                            {
                                fpath = dr["imagef"].ToString().Trim();
                                if (fpath != "-")
                                {
                                    FilStr = new FileStream(fpath, FileMode.Open);
                                    BinRed = new BinaryReader(FilStr);
                                    dr["ImgPath"] = fpath;
                                    dr["jcImg"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                    FilStr.Close();
                                    BinRed.Close();
                                }
                            }
                            catch { }
                            #endregion
                        }
                        //===
                        ds = new DataSet();
                        ds.Tables.Add(dt);
                        //===============
                        mq1 = "SELECT distinct trim(c.branchcd)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) as fstr1,A.NAME AS STAGE,b.icode as code,c.vchnum as job_no,to_char(c.vchdate,'dd/mm/yyyy') as job_dt,a.type1 FROM TYPE A,ITWSTAGE B,costestimate c WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) AND  trim(c.branchcd)='" + frm_mbr + "' and trim(c.type)='" + frm_vty + "' and trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by fstr1,b.icode,a.type1";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        dt4 = new DataTable();
                        dt4.Columns.Add("fstr1", typeof(string));
                        dt4.Columns.Add("stage", typeof(string));

                        if (dt3.Rows.Count > 0)
                        {
                            //  DataRow dr6 = new DataRow();
                            // foreach(DataRow dr3 in dt3.Rows)
                            mq1 = ""; mq2 = "";
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                mq1 += dt3.Rows[i]["STAGE"].ToString().Trim() + ">";
                                mq2 = dt3.Rows[i]["fstr1"].ToString().Trim();
                            }
                            //for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                dr1 = dt4.NewRow();
                                dr1["STAGE"] = mq1;
                                dr1["fstr1"] = mq2;
                                dt4.Rows.Add(dr1);
                            }
                        }
                        //============   
                        dt4.TableName = "stages";
                        ds.Tables.Add(dt4);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_jc_poly", "std_jc_poly", ds, "");
                    }
                }
                else
                {
                    #region job card print
                    dt2 = new DataTable();
                    dt2.Columns.Add("col_1", typeof(string));
                    dt2.Columns.Add("col_2", typeof(string));
                    dt2.Columns.Add("col_3", typeof(string));
                    dt2.Columns.Add("col_4", typeof(string));
                    dt2.Columns.Add("col_5", typeof(string));
                    dt2.Columns.Add("col_6", typeof(string));
                    dt2.Columns.Add("col_7", typeof(string));
                    dt2.Columns.Add("col_8", typeof(string));
                    dt2.Columns.Add("col_9", typeof(string));
                    dt2.Columns.Add("col_10", typeof(string));
                    dt2.Columns.Add("col_11", typeof(string));
                    dt2.Columns.Add("col_12", typeof(string));
                    ////
                    dt2.Columns.Add("ENTBY1", typeof(string));
                    dt2.Columns.Add("ENTDT1", typeof(string));
                    dt2.Columns.Add("vchnum", typeof(string));
                    dt2.Columns.Add("vchdate", typeof(string));
                    dt2.Columns.Add("CONVDATE", typeof(string));
                    dt2.Columns.Add("sotype", typeof(string));
                    dt2.Columns.Add("icode", typeof(string));
                    dt2.Columns.Add("Qty", typeof(string));
                    dt2.Columns.Add("entby2", typeof(string));
                    dt2.Columns.Add("entdt2", typeof(string));
                    dt2.Columns.Add("sheets", typeof(string));
                    dt2.Columns.Add("wstg", typeof(string));
                    dt2.Columns.Add("wt_shet", typeof(double));
                    dt2.Columns.Add("edt_by", typeof(string));
                    dt2.Columns.Add("edt_dt", typeof(string));
                    dt2.Columns.Add("iname", typeof(string));
                    dt2.Columns.Add("cpartno", typeof(string));
                    dt2.Columns.Add("party", typeof(string));
                    dt2.Columns.Add("mkt_rmk", typeof(string));
                    dt2.Columns.Add("app_by", typeof(string));
                    dt2.Columns.Add("prod_type", typeof(string));
                    dt2.Columns.Add("OD", typeof(string));
                    dt2.Columns.Add("PLY", typeof(double));
                    dt2.Columns.Add("ID", typeof(string));
                    dt2.Columns.Add("Corrug", typeof(string));
                    dt2.Columns.Add("UPS", typeof(double));
                    dt2.Columns.Add("DIE", typeof(string));
                    dt2.Columns.Add("FSTR", typeof(string));
                    dt2.Columns.Add("PPRMK", typeof(string));
                    dt2.Columns.Add("SOTOLR", typeof(string));
                    dt2.Columns.Add("REMARKS", typeof(string));
                    dt2.Columns.Add("LIN_MTR", typeof(string));
                    dt2.Columns.Add("CLOSE_RMK", typeof(string));
                    dt2.Columns.Add("ALL_WST", typeof(double));
                    ///===============new filds
                    dt2.Columns.Add("spec_no", typeof(string));
                    dt2.Columns.Add("cust_dlvdt", typeof(string));
                    dt2.Columns.Add("ppc_dlvdt", typeof(string));
                    dt2.Columns.Add("cylinder_z", typeof(string));
                    dt2.Columns.Add("gap_acros", typeof(string));
                    dt2.Columns.Add("gap_Around", typeof(string));
                    dt2.Columns.Add("lbl_acros", typeof(string));
                    dt2.Columns.Add("lbl_Around", typeof(string));
                    dt2.Columns.Add("lbl_hght", typeof(string));
                    dt2.Columns.Add("lbl_wdth", typeof(string));

                    SQuery = "select DISTINCT substr(trim(a.col1),1,50) as col1,substr(trim(a.col2),1,50) as col2,substr(trim(a.col3),1,50) as col3,substr(trim(a.col4),1,50) as col4,substr(trim(a.col5),1,50) as col5,substr(trim(a.col6),1,50) as col6,substr(trim(a.col7),1,50) as col7,substr(trim(a.col8),1,50) as col8,substr(trim(a.col9),1,40) as col9,substr(trim(a.col10),1,40) as col10,substr(trim(a.col11),1,40) as col11,substr(trim(a.col12),1,40) as col12, A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.cdrgno,nvl(b.imagef,'-') as imagef,b.cpartno,c.aname as party from costestimate a,item b,famst c,inspmst d where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) and d.type='70' and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.srno";
                    dt2.Columns.Add("imagef", typeof(string));
                    SQuery = "select DISTINCT A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR, to_Char(a.vchdate,'dd/mm/yyyy') as vch,a.* ,b.iname,b.iweight,b.cdrgno,b.cpartno,c.aname as party,nvl(b.imagef,'-') as imagef from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and trim(a.branchcd)='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "' and trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by a.vchnum,a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    DataTable dtm3 = new DataTable();
                    dtm3 = fgen.getdata(frm_qstr, frm_cocd, "SELECT branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr, branchcd,type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,icode, (case when qtyord>0 then round(qtysupp/qtyord*100,2) else 0 end) as so_tol  FROM somas  where branchcd='" + frm_mbr + "' and type='40' and orddt " + DateRange + "");
                    DataTable dt7 = new DataTable();
                    dt7 = fgen.getdata(frm_qstr, frm_cocd, "select distinct trim(d.icode) as icode, d.ent_by as entby,to_char(d.ent_Dt,'dd/mm/yyyy') as entdt,d.col13 as prd_typ,D.COL12,D.REJQTY,d.col14 as od,d.col15 as ply,d.col16 as id,d.col17 as corrug,trim(d.TITLE) as title,trim(d.REMARK2) as REMARK2,trim(d.REMARK3) as REMARK3,trim(d.REMARK4) as REMARK4 from inspmst d where d.branchcd='" + frm_mbr + "' and d.type='70' and vchdate " + DateRange + "  order by icode");
                    DataTable dt8 = new DataTable();
                    // dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT icode,btchdt FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and vchdate " + DateRange + ""); //old
                    dt8 = fgen.getdata(frm_qstr, frm_cocd, "SELECT vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,icode,btchdt,col13 as cylinder,col14 as lbl_AROUND,grade as gap_acros,col15 as lbl_acros,col16 as gap_around,maintdt as lbl_width FROM INSPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' and vchdate " + DateRange + "");//after add some new fileds

                    ds = new DataSet();
                    dt1 = dt2.Clone();
                    dt3 = dt2.Clone();
                    dt4 = dt2.Clone();
                    dt6 = new DataTable();//for more thn 47 rows in job card
                    dt5 = dt2.Clone();
                    dt6 = dt2.Clone();
                    double papergiven = 0;
                    double jcqty1 = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) <= 15)
                        {
                            dr1["col_1"] = dt.Rows[i]["col2"].ToString().Trim();
                            dr1["col_2"] = dt.Rows[i]["col3"].ToString().Trim();
                            // dr1["col_3"] = dt.Rows[i]["col5"].ToString().Trim();
                            dr1["col_3"] = dt.Rows[i]["col7"].ToString().Trim().toDouble(3).ToString("f");
                            dr1["ENTBY1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entby");//d
                            dr1["ENTDT1"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "entDT");//d
                            dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                            dr1["VCHDATE"] = dt.Rows[i]["VCH"].ToString().Trim();
                            dr1["CONVDATE"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(4, 6) + " " + dt.Rows[i]["convdate"].ToString().Trim().Substring(10, 10);
                            dr1["sotype"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(2, 2);
                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["Qty"] = dt.Rows[i]["Qty"].ToString().Trim();
                            dr1["entby2"] = dt.Rows[i]["ent_by"].ToString().Trim();
                            dr1["entdt2"] = dt.Rows[i]["ent_dt"].ToString().Trim();
                            dr1["sheets"] = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim());
                            dr1["wstg"] = fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                            // old field     //dr1["wt_shet"] = dt.Rows[i]["irate"].ToString().Trim();                                               
                            //changed by vipin
                            if (dt.Rows[i]["col9"].ToString().Trim().Length > 1)
                            {
                                if (dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "07" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "80" || dt.Rows[i]["col9"].ToString().Trim().Substring(0, 2) == "81")
                                    papergiven += dt.Rows[i]["col7"].ToString().toDouble();
                                else papergiven += dt.Rows[i]["col7"].ToString().toDouble() * fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["col9"].ToString().Trim() + "'", "IWEIGHT").toDouble();
                            }
                            if (jcqty1 <= 0)
                                jcqty1 = dt.Rows[i]["qty"].ToString().toDouble() + dt.Rows[i]["col15"].ToString().toDouble() * dt.Rows[i]["col13"].ToString().toDouble();
                            //dr1["wt_shet"] = Math.Round((dt.Rows[i]["col7"].ToString().Trim().toDouble() + dt.Rows[i]["iweight"].ToString().Trim().toDouble()) / (dt.Rows[i]["qty"].ToString().Trim().toDouble() + dt.Rows[i]["col13"].ToString().Trim().toDouble() + dt.Rows[i]["col15"].ToString().Trim().toDouble()), 3);
                            dr1["edt_by"] = dt.Rows[i]["edt_by"].ToString().Trim();
                            dr1["edt_dt"] = dt.Rows[i]["edt_Dt"].ToString().Trim();
                            dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            dr1["party"] = dt.Rows[i]["party"].ToString().Trim();
                            dr1["mkt_rmk"] = dt.Rows[i]["col12"].ToString().Trim();
                            dr1["app_by"] = dt.Rows[i]["app_by"].ToString().Trim();
                            dr1["prod_type"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "prd_typ");//d
                            dr1["OD"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "OD");//d
                            dr1["ID"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ID");//d
                            dr1["Corrug"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "corrug");//d
                            dr1["UPS"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "REJQTY"));//d
                            dr1["DIE"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "COL12"); //d
                            dr1["PLY"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ply"));//d
                            dr1["PPRMK"] = fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "title") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark2") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark3") + " , " + fgen.seek_iname_dt(dt7, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "remark4");
                            dr1["REMARKS"] = dt.Rows[i]["REMARKS"].ToString().Trim();
                            dr1["ALL_WST"] = dt.Rows[i]["COL22"].ToString().Trim().toDouble();
                            db1 = fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt"));
                            db2 = fgen.make_double(dt.Rows[i]["col14"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["col15"].ToString().Trim());
                            db3 = (db1 / 100) * db2;
                            dr1["LIN_MTR"] = db3;
                            dr1["SOTOLR"] = fgen.seek_iname_dt(dtm3, "fstr='" + dt.Rows[i]["convdate"].ToString().Trim().Substring(0, 20) + "'", "so_tol");
                            dr1["CLOSE_RMK"] = dt.Rows[i]["COMMENTS5"].ToString().Trim();
                            dr1["FSTR"] = dt.Rows[i]["FSTR"].ToString().Trim();
                            dr1["imagef"] = dt.Rows[i]["imagef"].ToString().Trim();
                            //=======new fields
                            dr1["spec_no"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchnum");
                            dr1["cust_dlvdt"] = Convert.ToDateTime(dt.Rows[i]["enqdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                            dr1["ppc_dlvdt"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "vchdate");
                            dr1["cylinder_z"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "cylinder");
                            dr1["gap_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_acros");
                            dr1["gap_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "gap_around");
                            dr1["lbl_acros"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_acros");
                            dr1["lbl_Around"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_AROUND");
                            dr1["lbl_hght"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "btchdt");
                            dr1["lbl_wdth"] = fgen.seek_iname_dt(dt8, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "lbl_width");
                            dt1.Rows.Add(dr1);
                        }
                    }
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        dt1.Rows[i]["wt_shet"] = (papergiven / jcqty1).toDouble(3);
                    }
                    #region Process Plan Date Filling
                    ///for 2
                    int index = 0;
                    for (int i = 16; i < dt.Rows.Count; i++)
                    {
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) > 15 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 31)
                        {
                            dt1.Rows[index]["col_4"] = dt.Rows[i]["col2"].ToString().Trim();
                            dt1.Rows[index]["col_5"] = dt.Rows[i]["col3"].ToString().Trim();
                            dt1.Rows[index]["col_6"] = dt.Rows[i]["col5"].ToString().Trim();
                            index++;
                        }
                    }
                    index = 0;
                    for (int i = 32; i < dt.Rows.Count; i++)
                    {
                        if (int.Parse(dt.Rows[i]["srno"].ToString()) > 31 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 47)
                        {
                            dt1.Rows[index]["col_7"] = dt.Rows[i]["col2"].ToString().Trim();
                            dt1.Rows[index]["col_8"] = dt.Rows[i]["col3"].ToString().Trim();
                            dt1.Rows[index]["col_9"] = dt.Rows[i]["col5"].ToString().Trim();
                            index++;
                        }
                    }
                    ///for 4 th column
                    index = 0;
                    if (dt.Rows.Count > 47)
                    {
                        for (int i = 47; i < dt.Rows.Count; i++)
                        {
                            if (int.Parse(dt.Rows[i]["srno"].ToString()) > 47 && int.Parse(dt.Rows[i]["srno"].ToString()) <= 63)
                            {
                                dt1.Rows[index]["col_10"] = dt.Rows[i]["col2"].ToString().Trim();
                                dt1.Rows[index]["col_11"] = dt.Rows[i]["col3"].ToString().Trim();
                                dt1.Rows[index]["col_12"] = dt.Rows[i]["col5"].ToString().Trim();
                                index++;
                            }
                        }
                    }
                    #endregion
                    if (dt1.Rows.Count > 0)
                    {
                        dt1 = fgen.addBarCode(dt1, "fstr", true);
                        dt1.TableName = "Prepcur";
                        dt1.Columns.Add("ImgPath", typeof(string));
                        dt1.Columns.Add("jcImg", typeof(System.Byte[]));
                        FileStream FilStr;
                        BinaryReader BinRed;
                        foreach (DataRow dr in dt1.Rows)
                        {
                            dr["ImgPath"] = "-";
                            try
                            {
                                fpath = dr["imagef"].ToString().Trim();
                                if (fpath != "-")
                                {
                                    FilStr = new FileStream(fpath, FileMode.Open);
                                    BinRed = new BinaryReader(FilStr);
                                    dr["ImgPath"] = fpath;
                                    dr["jcImg"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                    FilStr.Close();
                                    BinRed.Close();
                                }
                            }
                            catch { }
                        }
                    }
                    ds.Tables.Add(dt1);
                    #endregion
                    if (dt1.Rows.Count > 0)
                    {
                        #region
                        mq1 = "SELECT distinct trim(c.branchcd)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) as fstr,A.NAME AS STAGE,A.TYPE1,b.icode,b.srno,c.vchnum as job_no,to_char(c.vchdate,'dd/mm/yyyy') as job_dt FROM TYPE A,ITWSTAGE B,costestimate c WHERE A.ID='K' AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) AND  trim(c.branchcd)='" + frm_mbr + "' and  trim(c.type)='" + frm_vty + "' and trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') in (" + barCode + ") order by fstr,b.icode,b.srno"; //27jan
                        dtm = new DataTable();
                        dtm = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        //mq2 = "select icode,a2 AS NET_PRODN,a4 AS REJ,job_no, JOB_DT,STAGE,mchcode,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ")";
                        mq2 = "select icode,sum(a2) AS NET_PRODN,sum(a4) AS REJ,job_no, JOB_DT,STAGE,type from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type in ('86','88') and job_no||job_dt in (" + barCode + ") group by icode,job_no, JOB_DT,STAGE,type";
                        DataTable dtm1 = new DataTable();
                        dtm1 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        DataTable dtm2 = new DataTable();
                        mq3 = "select icode,IS_NUMBER(col4) AS NET_PRODN,IS_NUMBER(col5) AS REJ,enqno AS JOB_NO,TO_CHAR(enqdt,'DD/MM/YYYY') AS JOB_DT from costestimate WHERE BRANCHCD='" + frm_mbr + "' AND type='60' and trim(enqno)||to_char(enqdt,'dd/mm/yyyy') in (" + barCode + ")";
                        dtm2 = fgen.getdata(frm_qstr, frm_cocd, mq3);
                        dtm.Columns.Add("MACHINENAME", typeof(double));
                        dtm.Columns.Add("NET_PRODN", typeof(double));
                        dtm.Columns.Add("REJ", typeof(double));
                        if (dtm.Rows.Count > 0)
                        {
                            if (dtm1.Rows.Count > 0 || dtm2.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtm.Rows.Count; i++)
                                {
                                    if (dtm.Rows[i]["type1"].ToString().Trim() == "08")
                                    {
                                        if (dtm2.Rows.Count > 0)
                                        {
                                            dtm.Rows[i]["NET_PRODN"] = fgen.seek_iname_dt(dtm2, "icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and JOB_NO='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "NET_PRODN");
                                            dtm.Rows[i]["REJ"] = fgen.seek_iname_dt(dtm2, "icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and JOB_NO='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "REJ");
                                        }
                                    }
                                    else
                                    {
                                        if (dtm1.Rows.Count > 0)
                                        {
                                            dtm.Rows[i]["NET_PRODN"] = fgen.seek_iname_dt(dtm1, "STAGE='" + dtm.Rows[i]["TYPE1"].ToString().Trim() + "' and icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and job_no='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "NET_PRODN");
                                            dtm.Rows[i]["REJ"] = fgen.seek_iname_dt(dtm1, "STAGE='" + dtm.Rows[i]["TYPE1"].ToString().Trim() + "' and icode='" + dtm.Rows[i]["icode"].ToString().Trim() + "' and job_no='" + dtm.Rows[i]["job_no"].ToString().Trim() + "' and job_dt='" + dtm.Rows[i]["job_dt"].ToString().Trim() + "'", "REJ");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            mq0 = "";
                            mq0 = "SELECT '-' as fstr,'-' as STAGE,'-' as TYPE1,'-' as icode,'-' as srno,'-' as job_no,'-' as job_dt,'-' as NET_PRODN,'-' as REJ,'-' as machinename from dual";
                            dtm = new DataTable();
                            dtm = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        }
                        //============
                        dtm.TableName = "type1";
                        ds.Tables.Add(dtm);
                        switch (frm_IndType)
                        {
                            case "13":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_jc_Label", "std_jc_Label", ds, "");//FOR LSBEL RPT
                                break;
                            case "12":
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_jc_Flex", "std_jc_Flex", ds, ""); //FOR FLEX REPT                            
                                break;
                            default:
                                Print_Report_BYDS(frm_cocd, frm_mbr, "std_jobCard", "std_jobCard", ds, "");
                                break;
                        }
                        #endregion
                    }
                }
                break;

            case "F35106":
            case "F35107": // Machine Planning
                #region planning sheet
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("fstr", typeof(string));
                dtm.Columns.Add("jobno", typeof(string));
                dtm.Columns.Add("jobdt", typeof(string));
                dtm.Columns.Add("fromdt", typeof(string));
                dtm.Columns.Add("todt", typeof(string));
                dtm.Columns.Add("vchnum", typeof(string));
                dtm.Columns.Add("vchdate", typeof(string));
                dtm.Columns.Add("icode", typeof(string));
                dtm.Columns.Add("partno", typeof(string));
                dtm.Columns.Add("iname", typeof(string));
                dtm.Columns.Add("desc", typeof(string));
                dtm.Columns.Add("sheet", typeof(string));
                dtm.Columns.Add("ups", typeof(string));
                dtm.Columns.Add("flute", typeof(string));
                dtm.Columns.Add("std_bs", typeof(string));
                dtm.Columns.Add("std_cs", typeof(string));
                dtm.Columns.Add("std_board", typeof(string));
                dtm.Columns.Add("reel_cm", typeof(string));
                dtm.Columns.Add("LIN_MTR", typeof(string));
                dtm.Columns.Add("TOT_WT", typeof(double));
                dtm.Columns.Add("cut_mm", typeof(string));
                dtm.Columns.Add("OD", typeof(string));
                dtm.Columns.Add("joint", typeof(string));
                dtm.Columns.Add("ID", typeof(string));
                dtm.Columns.Add("col1", typeof(string));
                dtm.Columns.Add("col2", typeof(string));
                dtm.Columns.Add("col3", typeof(string));
                dtm.Columns.Add("col4", typeof(string));
                dtm.Columns.Add("col5", typeof(string));
                dtm.Columns.Add("color", typeof(string));
                dtm.Columns.Add("tot_plan_wt", typeof(double));

                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,b.iname,b.cpartno,a1 as sheet,a.job_no,a.job_dt,trim(a.job_no)||trim(a.job_dt) as fstr,a.ename ,a.tempr from prod_sheet A ,item b where  TRIM(a.icode)=trim(b.icode)  AND /*a.branchcd='" + frm_mbr + "' and a.type='90' and a.vchdate " + xprdRange + "*/ a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ")  order by a.srno,a.vchnum";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt1 = new DataTable();
                SQuery = "select  col1,col2,col14,COL16,icode,MAINTDT,REJQTY AS UPS from inspmst where branchcd='" + frm_mbr + "' and type='70' and vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') order by srno"; ///process plan 
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                DataTable DT23 = new DataTable();
                SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col7,COL17 AS TOT_WT ,icode  from costestimate where branchcd='" + frm_mbr + "' and  type='30' and vchdate " + DateRange + " order by srno";
                DT23 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //jobcard query
                double d = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr0 = dtm.NewRow();
                        dr0["header"] = "Planning Sheet";
                        dr0["vchnum"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr0["vchdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                        dr0["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr0["partno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                        dr0["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr0["desc"] = dt.Rows[i]["tempr"].ToString().Trim();
                        dr0["sheet"] = dt.Rows[i]["sheet"].ToString().Trim();
                        dr0["jobno"] = dt.Rows[i]["job_no"].ToString().Trim();
                        dr0["jobdt"] = dt.Rows[i]["job_dt"].ToString().Trim();
                        dr0["fromdt"] = dt.Rows[i]["frmdt"].ToString().Trim();
                        dr0["todt"] = dt.Rows[i]["todt"].ToString().Trim();
                        dr0["fstr"] = dt.Rows[i]["fstr"].ToString().Trim();

                        dt2 = new DataTable();
                        if (dt1.Rows.Count > 0)
                        {
                            DataView viewim = new DataView(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt2 = viewim.ToTable();
                        }
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            if (j == 0)
                            {
                                dr0["OD"] = dt2.Rows[j]["col14"].ToString().Trim();
                                dr0["ID"] = dt2.Rows[j]["col16"].ToString().Trim();
                                dr0["reel_cm"] = dt2.Rows[j]["MAINTDT"].ToString().Trim();
                                dr0["ups"] = dt2.Rows[j]["ups"].ToString().Trim();
                            }
                            mq2 = "";
                            mq2 = dt2.Rows[j]["col1"].ToString().Trim();
                            if (mq2 == "FLUTE TYPE")
                            {
                                dr0["flute"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            //////////////////
                            if (mq2 == "BURSTING STRENGTH")
                            {
                                dr0["std_bs"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            //////////////////////
                            if (mq2 == "COMPR.STRENGTH")
                            {
                                dr0["std_cs"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            ////////////////
                            if (mq2 == "BOARD GSM")
                            {
                                dr0["std_board"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            //////////////
                            if (mq2 == "CUT SIZE (MM)")
                            {
                                dr0["cut_mm"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            if (mq2.Contains("COLOR") || mq2.Contains("COLOUR"))
                            {
                                dr0["color"] += dt2.Rows[j]["col2"].ToString().Trim() + ",";
                            }
                            if (mq2 == "STAPLING/GLUING")
                            {
                                dr0["JOINT"] = dt2.Rows[j]["col2"].ToString().Trim();
                            }
                            ///////////////                                                                       

                        }
                        int l = 1;
                        dt3 = new DataTable();
                        ////////from jobcard
                        if (DT23.Rows.Count > 0)
                        {
                            DataView viewim1 = new DataView(DT23, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and vchnum='" + dt.Rows[i]["job_no"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = viewim1.ToTable();
                        }
                        if (dt3.Rows.Count > 0)
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                dr0["TOT_WT"] = fgen.make_double(dt3.Rows[0]["TOT_WT"].ToString().Trim());
                                if (dt3.Rows.Count > j)
                                {
                                    mq3 = "";
                                    mq3 = dt3.Rows[j]["col7"].ToString().Trim();
                                    if (mq3 != "0")
                                    {
                                        dr0["col" + l] = "[" + dt3.Rows[j]["col2"].ToString().Trim() + "]" + dt3.Rows[j]["col3"].ToString().Trim() + " [" + dt3.Rows[j]["col7"].ToString().Trim() + "kg]";
                                    }
                                }
                                l++;
                            }

                        }

                        dtm.Rows.Add(dr0);
                    }
                    //////for bar code
                    dtm = fgen.addBarCode(dtm, "fstr", true);
                }
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_ControlPlanning_sheet", "std_ControlPlanning_sheet", dsRep, "");
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
        repDoc.Close();
        repDoc.Dispose();
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