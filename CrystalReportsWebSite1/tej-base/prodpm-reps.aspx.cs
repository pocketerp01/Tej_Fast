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

public partial class prodpm_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, pdfView = "", data_found = "", WB_TABNAME = "";
    string mq2 = "", mq3 = "";
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    DataRow oporow; DataView view1im, dv, view2;
    double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0, db9 = 0, db10 = 0, db11 = 0;
    string mq6 = "", mq7 = "", mq8 = "", mq9 = "", mq10 = "", mq11 = "", mq12 = "", cond = "", cond1 = "", party_cd, part_cd;
    string ded1 = "", ded2 = "", ded3 = "", ded4 = "", ded5 = "", ded6 = "", branchcd = "";
    string header_n = "";
    DataTable ph_tbl, dt1, dt2, dt3, dt4, dt5;

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
                    branchcd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");

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
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm, mdt, dtdrsim, dticode, ph_tbl;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        string mq10, mq1, mq0;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string vartype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");
        data_found = "Y";

        switch (iconID)
        {
            case "F39152":
                if (frm_cocd == "KESR")
                {
                    #region KESR MODILNG PLAN REPORT
                    header_n = "Moulding Production Plan Report";
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("fromdt", typeof(string));
                    ph_tbl.Columns.Add("todt", typeof(string));
                    ph_tbl.Columns.Add("header", typeof(string));
                    ph_tbl.Columns.Add("mch_no", typeof(string));
                    ph_tbl.Columns.Add("vchnum", typeof(string));
                    ph_tbl.Columns.Add("date", typeof(string));
                    ph_tbl.Columns.Add("vdd", typeof(string));
                    ph_tbl.Columns.Add("shiftcode", typeof(string));
                    ph_tbl.Columns.Add("shift", typeof(string));
                    ph_tbl.Columns.Add("customer", typeof(string));
                    ph_tbl.Columns.Add("partname", typeof(string));
                    ph_tbl.Columns.Add("partsize", typeof(string));
                    ph_tbl.Columns.Add("compound_code", typeof(string));
                    ph_tbl.Columns.Add("mold_no", typeof(string));

                    ph_tbl.Columns.Add("plan_qty", typeof(double));
                    ph_tbl.Columns.Add("cavity", typeof(double));
                    ph_tbl.Columns.Add("cycle_tym_Sec", typeof(double));
                    ph_tbl.Columns.Add("strip_Wt_gms", typeof(double));
                    ph_tbl.Columns.Add("tot_pln_shot", typeof(double));
                    ph_tbl.Columns.Add("Shots_per_hr", typeof(double));//
                    ph_tbl.Columns.Add("Total_Prod_hr", typeof(double));
                    ph_tbl.Columns.Add("Tot_PLN_SHOT_SHIFT", typeof(double));

                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                    db = 0; db4 = 0; db5 = 0; db6 = 0; db1 = 0; db2 = 0; db7 = 0;
                    mq11 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    mq0 = "select  to_char(a.vchdate,'yyyyMMdd') as vdd,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.cust_ref,A.JOB_NO,A.job_dt,trim(a.acode) as acode,trim(a.icode) as icode,b.iname,b.maker,a.a1 as plan_qty,A.A3 AS CAVITY,a.a4 as shots,a.a5 as tot_prod_hr,b.wt_NET,a.shftcode,c.name as shift,a.ename,replace(replace(replace(trim(a.remarks),chr(13),''),chr(9),''),chr(10),'')  as machine from  prod_sheet a ,item b ,type c where trim(a.icode)=trim(b.icode) and trim(a.shftcode)=trim(c.type1) and c.id='D' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.shftcode)='" + mq11 + "' order by A.SHFTCODE,srno,machine,VDD";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq0); //mold prod plan dt 

                    mq1 = "select distinct trim(a.icode) as icode,trim(a.ibcode) as ibcode,trim(b.iname) as iname,nvl(A.IBQTY,0) AS main_qty from itemosp a,item b where trim(a.ibcode)=trim(b.icode) and a.branchcd='" + frm_mbr + "'  and a.type='BM' AND SUBSTR(a.ICODE,1,1)= '9' order by icode";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1); //bom dt 

                    mq2 = "SELECT DISTINCT  A.TYPE,TRIM(A.MCHNUM)||replace(replace(replace(trim(a.mchname),chr(13),''),chr(9),''),chr(10),'')  AS MACHINE,replace(trim(A.MCHNAME),chr(13),'') as mch ,A.CAVITY,A.SHOTS_DAY AS SHOT_PER_HR,TRIM(A.ICODE) AS ICODE,TRIM(A.STATIONNO) AS STATIONNO,replace(trim(B.INAME), chr(13),'') as iname  FROM MACHMST A,ITEM B  WHERE TRIM(A.STATIONNO)=TRIM(B.ICODE)  and a.branchcd='" + frm_mbr + "'  AND a.TYPE='01' order by machine";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2); //MACHINE MASTER DT

                    mq3 = "SELECT DISTINCT TRIM(ACODE) AS ACODE,TRIM(ANAME) AS CUST,buycode FROM FAMST";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);

                    db7 = 0; db8 = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["header"] = header_n;
                        dr1["mch_no"] = dt.Rows[i]["MACHINE"].ToString().Trim();///acc to ashok sir
                        dr1["vchnum"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr1["date"] = dt.Rows[i]["vchdate"].ToString().Trim();
                        dr1["vdd"] = dt.Rows[i]["vdd"].ToString().Trim();
                        dr1["shiftcode"] = dt.Rows[i]["shftcode"].ToString().Trim();
                        dr1["shift"] = dt.Rows[i]["SHIFT"].ToString().Trim();
                        dr1["customer"] = fgen.seek_iname_dt(dt3, "acode='" + dt.Rows[i]["cust_ref"].ToString().Trim() + "'", "buycode");
                        dr1["partname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["partsize"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["compound_code"] = fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                        dr1["mold_no"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                        dr1["cavity"] = dt.Rows[i]["cavity"].ToString().Trim();
                        dr1["plan_qty"] = dt.Rows[i]["plan_qty"].ToString().Trim();
                        db = fgen.make_double(dr1["cavity"].ToString().Trim());//cavity
                        db4 = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "main_qty"));
                        dr1["strip_Wt_gms"] = db * db4 * 1000;
                        db5 = fgen.make_double(dr1["plan_qty"].ToString().Trim());
                        dr1["Shots_per_hr"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "SHOT_PER_HR"));
                        db1 = fgen.make_double(dr1["Shots_per_hr"].ToString().Trim());
                        db6 = fgen.make_double(dr1["Shots_per_hr"].ToString().Trim());
                        if (db6 != 0)
                        {
                            dr1["cycle_tym_Sec"] = 3600 / fgen.make_double(dr1["Shots_per_hr"].ToString().Trim());
                        }
                        else
                        {
                            dr1["cycle_tym_Sec"] = 0;
                        }
                        if (db != 0)
                        {
                            dr1["tot_pln_shot"] = db5 / db; //planqty divide by cavity
                        }
                        else
                        {
                            dr1["tot_pln_shot"] = 0;
                        }

                        dr1["Total_Prod_hr"] = dt.Rows[i]["tot_prod_hr"].ToString().Trim();
                        db7 = fgen.make_double(dr1["Shots_per_hr"].ToString().Trim()) * fgen.make_double(dr1["Total_Prod_hr"].ToString().Trim());

                        if (db1 > 0)
                        {
                            dr1["cycle_tym_Sec"] = 3600 / db1;
                        }
                        else
                        {
                            dr1["cycle_tym_Sec"] = 0;
                        }
                        dr1["Tot_PLN_SHOT_SHIFT"] = db7;

                        ph_tbl.Rows.Add(dr1);
                    }

                    if (ph_tbl.Rows.Count > 0)
                    {
                        dsRep = new DataSet();
                        ph_tbl.TableName = "Prepcur";
                        dsRep.Tables.Add(ph_tbl);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "Prod_Plan_Mold_KESR", "Prod_Plan_Mold_KESR", dsRep, "");
                    }
                    #endregion
                }
                if (frm_cocd == "AGRM")
                {
                    #region agrm MODILNG PLAN REPORT
                    header_n = "Moulding Production Plan Report Vs Actual Plan";
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("fromdt", typeof(string));
                    ph_tbl.Columns.Add("todt", typeof(string));
                    ph_tbl.Columns.Add("header", typeof(string));
                    ph_tbl.Columns.Add("operator", typeof(string));//from mould plan web form
                    ph_tbl.Columns.Add("mch_no", typeof(string));
                    ph_tbl.Columns.Add("vchnum", typeof(string));
                    ph_tbl.Columns.Add("date", typeof(string));
                    ph_tbl.Columns.Add("vdd", typeof(string));
                    ph_tbl.Columns.Add("shiftcode", typeof(string));
                    ph_tbl.Columns.Add("shift", typeof(string));
                    ph_tbl.Columns.Add("partname", typeof(string));
                    ph_tbl.Columns.Add("compound_name", typeof(string)); //sf name from bom
                    ph_tbl.Columns.Add("partno", typeof(string));
                    ph_tbl.Columns.Add("compound_code", typeof(string));
                    ph_tbl.Columns.Add("mold_no", typeof(string));
                    ph_tbl.Columns.Add("sch_qty", typeof(double));//plan_qty
                    ph_tbl.Columns.Add("cavity", typeof(double));
                    ph_tbl.Columns.Add("gross_wt_each_comp", typeof(double));
                    ph_tbl.Columns.Add("req_mat_comp_kg", typeof(double));//schqty*gross_wt_each_comp
                    ph_tbl.Columns.Add("tot_pln_shot", typeof(double));
                    ph_tbl.Columns.Add("Shots_per_hr", typeof(double));//
                    ph_tbl.Columns.Add("Total_Prod_hr", typeof(double));
                    ph_tbl.Columns.Add("Tot_PLN_SHOT_SHIFT", typeof(double));

                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                    db = 0; db4 = 0; db5 = 0; db6 = 0; db1 = 0; db2 = 0; db7 = 0;
                    mq11 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    mq0 = "select  to_char(a.vchdate,'yyyyMMdd') as vdd,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.remarks2 as operator,a.cust_ref,A.JOB_NO,A.job_dt,trim(a.acode) as acode,trim(a.icode) as icode,b.iname,b.cpartno,b.maker,a.a1 as plan_qty,A.A3 AS CAVITY,a.a4 as shots,a.a5 as tot_prod_hr,b.wt_NET,a.shftcode,c.name as shift,a.ename,replace(replace(replace(trim(a.remarks),chr(13),''),chr(9),''),chr(10),'')  as machine from  prod_sheet a ,item b ,type c where trim(a.icode)=trim(b.icode) and trim(a.shftcode)=trim(c.type1) and c.id='D' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.shftcode)='" + mq11 + "' order by A.SHFTCODE,srno,machine,VDD";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq0); //mold prod plan dt 

                    mq1 = "select distinct trim(a.icode) as icode,trim(a.ibcode) as ibcode,trim(b.iname) as iname,nvl(A.IBQTY,0) AS main_qty from itemosp a,item b where trim(a.ibcode)=trim(b.icode) and a.branchcd='" + frm_mbr + "'  and a.type='BM' AND SUBSTR(a.ICODE,1,1)= '9' order by icode";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1); //bom dt 

                    mq2 = "SELECT DISTINCT  A.TYPE,TRIM(A.MCHNUM)||replace(replace(replace(trim(a.mchname),chr(13),''),chr(9),''),chr(10),'') AS MACHINE,replace(trim(A.MCHNAME),chr(13),'') as mch ,A.CAVITY,A.SHOTS_DAY AS SHOT_PER_HR,round(is_number(a.CAVITY) * is_numbeR(a.shots_day)) as capicity,TRIM(A.ICODE) AS ICODE,TRIM(A.STATIONNO) AS STATIONNO,replace(trim(B.INAME), chr(13),'') as iname  FROM MACHMST A,ITEM B  WHERE TRIM(A.STATIONNO)=TRIM(B.ICODE)  and a.branchcd='" + frm_mbr + "'  AND a.TYPE='01' order by machine";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2); //MACHINE MASTER DT

                    mq3 = "SELECT DISTINCT TRIM(ACODE) AS ACODE,TRIM(ANAME) AS CUST,buycode FROM FAMST";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);

                    db7 = 0; db8 = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        db = 0; db1 = 0; db2 = 0; db4 = 0; db5 = 0; db6 = 0;
                        dr1 = ph_tbl.NewRow();
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["header"] = header_n;
                        dr1["operator"] = dt.Rows[i]["operator"].ToString().Trim();
                        dr1["mch_no"] = dt.Rows[i]["MACHINE"].ToString().Trim();///acc to ashok sir
                        dr1["vchnum"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr1["date"] = dt.Rows[i]["vchdate"].ToString().Trim();
                        dr1["vdd"] = dt.Rows[i]["vdd"].ToString().Trim();
                        dr1["shiftcode"] = dt.Rows[i]["shftcode"].ToString().Trim();
                        dr1["shift"] = dt.Rows[i]["SHIFT"].ToString().Trim();
                        dr1["partname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["compound_code"] = fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "ibcode");
                        dr1["compound_name"] = fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                        dr1["partno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                        dr1["mold_no"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                        dr1["cavity"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "CAVITY"));// fgen.make_double(dt.Rows[i]["cavity"].ToString().Trim());
                        dr1["sch_qty"] = fgen.make_double(dt.Rows[i]["plan_qty"].ToString().Trim());
                        db = fgen.make_double(dr1["cavity"].ToString().Trim());//cavity
                        db4 = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "main_qty"));  //pahle wt_net pic krasya tha but ab compound code ki main qty p[ic ki h acc to ashok sir
                        dr1["gross_wt_each_comp"] = db4;
                        dr1["req_mat_comp_kg"] = db4 * fgen.make_double(dr1["sch_qty"].ToString().Trim());
                        db5 = fgen.make_double(dr1["sch_qty"].ToString().Trim());
                        db2 = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and mch='" + dt.Rows[i]["MACHINE"].ToString().Trim() + "'", "capicity"));
                        dr1["Shots_per_hr"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "' and mch='" + dt.Rows[i]["MACHINE"].ToString().Trim() + "'", "SHOT_PER_HR"));

                        if (db != 0)
                        {
                            dr1["tot_pln_shot"] = db5 / db; //planqty divide by cavity
                        }
                        else
                        {
                            dr1["tot_pln_shot"] = 0;
                        }
                        dr1["Total_Prod_hr"] = db5 / db2;
                        db7 = fgen.make_double(dr1["Shots_per_hr"].ToString().Trim()) * fgen.make_double(dr1["Total_Prod_hr"].ToString().Trim());
                        dr1["Tot_PLN_SHOT_SHIFT"] = db7;
                        ph_tbl.Rows.Add(dr1);
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        dsRep = new DataSet();
                        ph_tbl.TableName = "Prepcur";
                        dsRep.Tables.Add(ph_tbl);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "Prod_Plan_Mold_AGRM", "Prod_Plan_Mold_AGRM", dsRep, "");
                    }
                    #endregion
                }
                break;

            case "F39223"://okmg
                header_n = "Machine Utilisation Chart";
                SQuery = "SELECT DISTINCT '" + header_n + "' AS HEADER ,'" + fromdt + "' as fmdt,'" + todt + "' as todt, E.NAME AS SHIFTNAME, A.*,B.INAME AS RNAME,D.INAME AS COMP_NAME,(A.IQTYIN+A.MLT_LOSS) AS PROD,C.BTCHNO AS LOT_NO FROM  PROD_SHEET A,ITEM B,IVOUCHER C ,ITEM D,TYPE E WHERE TRIM(A.VCHNUM)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=TRIM(C.VCHNUM)||to_Char(c.vchdate,'dd/mm/yyyy')||trim(c.rcode) AND TRIM(A.SHFTCODE)=TRIM(E.TYPE1) and TRIM(A.ICODE)=TRIM(D.ICODE) AND TRIM(B.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='90' AND C.TYPE='39' AND E.ID='D' AND A.VCHDATE " + xprdRange + " ORDER BY A.VCHNUM";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "CrptMachUtiliz", "CrptMachUtiliz", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39224"://okmg as this report ties up with rcode in ivoucher that is avlid for disp only as raw materials icode is being saved as rcode in ivoucher.
                #region Daily Production Report
                mq1 = "SELECT DISTINCT 'Daily Production Report' AS HEADER, E.NAME AS SHIFTNAME, A.*,B.INAME AS RNAME,D.INAME AS COMP_NAME,(A.IQTYIN+A.MLT_LOSS) AS PROD,C.BTCHNO AS LOT_NO,F.BTCHNO FROM  PROD_SHEET A,ITEM B,IVOUCHER C ,ITEM D,TYPE E,IVOUCHER F WHERE TRIM(A.VCHNUM)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=TRIM(C.VCHNUM)||to_Char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) AND TRIM(A.SHFTCODE)=TRIM(E.TYPE1) and TRIM(A.ICODE)=TRIM(D.ICODE) AND TRIM(B.ICODE)=TRIM(C.ICODE) AND A." + frm_mbr + " AND A.TYPE='" + vartype + "' AND C.TYPE='39' AND E.ID='D' AND A.VCHDATE " + xprdRange + " and F.TYPE='15' ORDER BY A.VCHNUM";
                SQuery = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit as cunit,to_char(a.vchdate,'dd/mm/yyyy') as vch from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('15') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + "  order by a.vchnum,a.srno ";
                mq0 = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit,to_char(a.vchdate,'dd/mm/yyyy') as vch from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('39') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + " and trim(a.naration)!='LUMPS' order by a.vchnum,a.srno ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                dt3 = new DataTable();
                mq2 = "Select a.* from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type in ('15') and a.stage='6R' and a.store='W' and upper(Trim(a.naration))='RUNNER' and a.vchdate " + xprdRange + "  order by a.vchnum,a.srno ";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                mdt = new DataTable();
                mdt.Columns.Add(new DataColumn("vchnum", typeof(string)));
                mdt.Columns.Add(new DataColumn("mcno", typeof(string)));
                mdt.Columns.Add(new DataColumn("mouldno", typeof(string)));
                mdt.Columns.Add(new DataColumn("compname", typeof(string)));
                mdt.Columns.Add(new DataColumn("rmat", typeof(string)));
                mdt.Columns.Add(new DataColumn("rmatcontrol", typeof(string)));
                mdt.Columns.Add(new DataColumn("totalCavity", typeof(double)));
                mdt.Columns.Add(new DataColumn("runCavity", typeof(double)));
                mdt.Columns.Add(new DataColumn("totalShots", typeof(double)));
                mdt.Columns.Add(new DataColumn("qtyprod", typeof(double)));
                mdt.Columns.Add(new DataColumn("qtyRej", typeof(double)));
                mdt.Columns.Add(new DataColumn("okComp", typeof(string)));
                mdt.Columns.Add(new DataColumn("brkdown", typeof(string)));
                mdt.Columns.Add(new DataColumn("brkdownCode", typeof(string)));
                mdt.Columns.Add(new DataColumn("mchFrom", typeof(string)));
                mdt.Columns.Add(new DataColumn("mchTo", typeof(string)));
                mdt.Columns.Add(new DataColumn("comp", typeof(double)));
                mdt.Columns.Add(new DataColumn("runner", typeof(double)));
                mdt.Columns.Add(new DataColumn("lumps", typeof(double)));
                mdt.Columns.Add(new DataColumn("remarks", typeof(string)));
                mdt.Columns.Add(new DataColumn("vchdate", typeof(DateTime)));
                mdt.Columns.Add(new DataColumn("grp", typeof(string)));
                mdt.Columns.Add(new DataColumn("vch", typeof(string)));
                oporow = null;
                if (dt.Rows.Count > 0 && dt2.Rows.Count > 0)
                {
                    //DataTable dtxx = new DataTable();
                    //dv = new DataView(dt2);
                    //dtxx = dv.ToTable(true, "VCHNUM", "VCHDATE", "acode", "RINAME", "CAVITY");

                    foreach (DataRow dr in dt2.Rows)
                    {
                        oporow = mdt.NewRow();
                        oporow["mcno"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(MCHCODE) AS FSTR,MCHNAME AS MACHINE_NAME,MCHCODE AS MACHINE_CODE,ACODE FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND TRIM(MCHCODE)='" + dr["acode"].ToString().Trim() + "'", "MACHINE_NAME");
                        oporow["mouldno"] = fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "riname");
                        string mould = fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "riname");
                        oporow["GRP"] = dr["vchdate"].ToString().Trim() + mould + dr["Riname"].ToString().Trim() + dr["cavity"].ToString().Trim();
                        oporow["compname"] = dr["riname"].ToString().Trim();
                        oporow["rmat"] = dr["iname"].ToString().Trim();
                        oporow["rmatcontrol"] = dr["BTCHNO"].ToString().Trim();
                        oporow["totalCavity"] = fgen.make_double(dr["ipack"].ToString().Trim());
                        oporow["runCavity"] = fgen.make_double(dr["cavity"].ToString().Trim());
                        oporow["totalShots"] = fgen.make_double(dr["shots"].ToString().Trim());
                        double d1 = fgen.make_double(dr["cavity"].ToString().Trim()) * fgen.make_double(dr["shots"].ToString().Trim());
                        oporow["qtyprod"] = d1;
                        oporow["qtyRej"] = fgen.make_double(dr["rej_rw"].ToString().Trim());
                        oporow["okComp"] = (d1 - fgen.make_double(dr["rej_rw"].ToString().Trim())).ToString();
                        oporow["brkdown"] = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(is_number(col3)) as sec from inspvch where branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='55' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "' ", "sec");
                        oporow["brkdownCode"] = fgen.seek_iname(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') as sec from inspvch where branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='55' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "' ", "sec");
                        oporow["mchFrom"] = dr["mtime"].ToString().Trim();
                        oporow["mchTo"] = dr["REVIS_NO"].ToString().Trim();
                        oporow["comp"] = dr["IQTYOUT"].ToString().Trim(); //fgen.make_double(fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "iqtyin")) * fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select iweight from item where trim(icodE)='" + dr["rcode"].ToString().Trim() + "'", "iweight"));
                        oporow["runner"] = fgen.seek_iname_dt(dt3, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "iqtyin").toDouble();
                        oporow["lumps"] = fgen.make_double(fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "rej_sdp"));
                        oporow["remarks"] = dr["acode"].ToString().Trim();
                        oporow["vchdate"] = dr["vchdate"].ToString().Trim();
                        oporow["vch"] = dr["vch"].ToString().Trim();
                        oporow["vchnum"] = dr["vchnum"].ToString().Trim();
                        mdt.Rows.Add(oporow);
                    }
                }
                view1im = new DataView(mdt);
                dtdrsim = new DataTable();
                dtdrsim = view1im.ToTable();
                if (dtdrsim.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dtdrsim.TableName = "Prepcur";
                    dsRep.Tables.Add(dtdrsim);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "CrptDispoSafe", "CrptDispoSafe", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39225"://okmg as this report ties up with rcode in ivoucher that is valid for disp only as raw materials icode is being saved as rcode in ivoucher.
                #region Shift Production Reprot
                ded1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = "SELECT DISTINCT 'Daily Production Report' AS HEADER, E.NAME AS SHIFTNAME, A.*,B.INAME AS RNAME,D.INAME AS COMP_NAME,(A.IQTYIN+A.MLT_LOSS) AS PROD,C.BTCHNO AS LOT_NO,F.BTCHNO FROM  PROD_SHEET A,ITEM B,IVOUCHER C ,ITEM D,TYPE E,IVOUCHER F WHERE TRIM(A.VCHNUM)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=TRIM(C.VCHNUM)||to_Char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) AND TRIM(A.SHFTCODE)=TRIM(E.TYPE1) and TRIM(A.ICODE)=TRIM(D.ICODE) AND TRIM(B.ICODE)=TRIM(C.ICODE) AND A." + frm_mbr + " AND A.TYPE='" + vartype + "' AND C.TYPE='39' AND E.ID='D' AND A.VCHDATE " + xprdRange + " and F.TYPE='15' ORDER BY A.VCHNUM";
                SQuery = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCH from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('15') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + " and o_deptt='" + ded1 + "' order by a.srno ";
                mq0 = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('39') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + " and o_deptt='" + ded1 + "' order by a.srno ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                mdt = new DataTable();
                //mdt.Columns.Add(new DataColumn("mcno", typeof(string)));
                //mdt.Columns.Add(new DataColumn("mcno", typeof(string)));
                mdt.Columns.Add(new DataColumn("mcno", typeof(string)));
                mdt.Columns.Add(new DataColumn("mouldno", typeof(string)));
                mdt.Columns.Add(new DataColumn("compname", typeof(string)));
                mdt.Columns.Add(new DataColumn("rmat", typeof(string)));
                mdt.Columns.Add(new DataColumn("rmatcontrol", typeof(string)));
                mdt.Columns.Add(new DataColumn("totalCavity", typeof(double)));
                mdt.Columns.Add(new DataColumn("runCavity", typeof(double)));
                mdt.Columns.Add(new DataColumn("totalShots", typeof(double)));
                mdt.Columns.Add(new DataColumn("qtyprod", typeof(double)));
                mdt.Columns.Add(new DataColumn("qtyRej", typeof(double)));
                mdt.Columns.Add(new DataColumn("okComp", typeof(string)));
                mdt.Columns.Add(new DataColumn("brkdown", typeof(string)));
                mdt.Columns.Add(new DataColumn("brkdownCode", typeof(string)));
                mdt.Columns.Add(new DataColumn("mchFrom", typeof(string)));
                mdt.Columns.Add(new DataColumn("mchTo", typeof(string)));
                mdt.Columns.Add(new DataColumn("comp", typeof(double)));
                mdt.Columns.Add(new DataColumn("runner", typeof(double)));
                mdt.Columns.Add(new DataColumn("lumps", typeof(double)));
                mdt.Columns.Add(new DataColumn("remarks", typeof(string)));
                mdt.Columns.Add(new DataColumn("shift", typeof(string)));
                mdt.Columns.Add(new DataColumn("vchdate", typeof(DateTime)));
                mdt.Columns.Add(new DataColumn("pname", typeof(string)));
                mdt.Columns.Add(new DataColumn("VCH", typeof(string)));
                oporow = null;
                if (dt.Rows.Count > 0 && dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        oporow = mdt.NewRow();
                        oporow["mcno"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(MCHCODE) AS FSTR,MCHNAME AS MACHINE_NAME,MCHCODE AS MACHINE_CODE,ACODE FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND TRIM(MCHCODE)='" + dr["acode"].ToString().Trim() + "'", "MACHINE_NAME");
                        oporow["mouldno"] = dr["riname"].ToString().Trim();
                        oporow["compname"] = dr["iname"].ToString().Trim();
                        oporow["rmat"] = fgen.seek_iname_dt(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "iname");
                        oporow["pname"] = dr["pname"].ToString().Trim();
                        oporow["rmatcontrol"] = fgen.seek_iname_dt(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "btchno");
                        oporow["totalCavity"] = fgen.make_double(dr["ipack"].ToString().Trim());
                        oporow["runCavity"] = fgen.make_double(dr["cavity"].ToString().Trim());
                        oporow["totalShots"] = fgen.make_double(dr["shots"].ToString().Trim());
                        double d1 = fgen.make_double(dr["cavity"].ToString().Trim()) * fgen.make_double(dr["shots"].ToString().Trim());
                        oporow["qtyprod"] = d1;
                        oporow["qtyRej"] = fgen.make_double(dr["rej_rw"].ToString().Trim());
                        oporow["okComp"] = (d1 - fgen.make_double(dr["rej_rw"].ToString().Trim())).ToString();
                        oporow["brkdown"] = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(is_number(col3)) as sec from inspvch where branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='55' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "' ", "sec");
                        oporow["brkdownCode"] = fgen.seek_iname(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') as sec from inspvch where branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='55' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "' ", "sec");
                        oporow["mchFrom"] = dr["mtime"].ToString().Trim();
                        oporow["mchTo"] = dr["REVIS_NO"].ToString().Trim();
                        oporow["comp"] = fgen.make_double(dr["iqtyin"].ToString().Trim()) * fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select iweight from item where trim(icodE)='" + dr["icode"].ToString().Trim() + "'", "iweight"));
                        oporow["runner"] = fgen.make_double(dr["iqtyin"].ToString().Trim()) * fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ioqty from itemosp where trim(icodE)='" + dr["icode"].ToString().Trim() + "'", "ioqty"));
                        oporow["lumps"] = fgen.make_double(dr["rej_sdp"].ToString().Trim());
                        oporow["remarks"] = dr["acode"].ToString().Trim();
                        oporow["vchdate"] = dr["vchDATE"].ToString().Trim();
                        oporow["vch"] = dr["vch"].ToString().Trim();
                        mq2 = "SELECT TRIM(TYPE1) AS FSTR,NAME AS SHIFT,TYPE1 AS CODE,place as shft_min,round(case when place>0 then place/60 else 0 end) as shft_hrs FROM TYPE WHERE ID='D' AND TYPE1 LIKE '" + dr["O_DEPTT"].ToString().Trim() + "%' ";
                        oporow["shift"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(TYPE1) AS FSTR,NAME AS SHIFT,TYPE1 AS CODE,place as shft_min,round(case when place>0 then place/60 else 0 end) as shft_hrs FROM TYPE WHERE ID='D' AND TYPE1 LIKE '" + dr["O_DEPTT"].ToString().Trim() + "%' ", "SHIFT");
                        mdt.Rows.Add(oporow);
                    }
                }
                dv = new DataView(mdt);
                dv.Sort = "mouldno,mchFrom";
                mdt = new DataTable();
                mdt = dv.ToTable();
                dtm = new DataTable();
                dtm = mdt.Clone();
                if (mdt.Rows.Count > 0)
                {
                    view1im = new DataView(mdt);
                    dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "vchdate", "mouldno", "compname", "runCavity", "mchFrom", "mchto");
                    oporow = null;
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        oporow = dtm.NewRow();
                        view2 = new DataView(mdt, "vchdate='" + dr0["vchdate"].ToString().Trim() + "' AND mouldno='" + dr0["mouldno"].ToString().Trim() + "' AND compname='" + dr0["compname"].ToString().Trim() + "' AND runCavity='" + dr0["runCavity"].ToString().Trim() + "' AND mchfrom='" + dr0["mchfrom"].ToString().Trim() + "' AND mchto='" + dr0["mchto"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dticode = new DataTable();
                        dticode = view2.ToTable();
                        db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0;
                        ded1 = ""; ded2 = ""; ded3 = ""; ded4 = ""; ded5 = ""; ded6 = "";
                        for (int i = 0; i < dticode.Rows.Count; i++)
                        {
                            // oporow = dtm.NewRow();
                            if (ded1.TrimStart('/') == dticode.Rows[i]["mcno"].ToString().Trim())
                            {
                                ded1 = dticode.Rows[i]["mcno"].ToString().Trim();
                            }
                            else
                            {
                                ded1 += "/" + dticode.Rows[i]["mcno"].ToString().Trim();
                            }
                            oporow["mcno"] = ded1.TrimStart('/');
                            oporow["mouldno"] = dticode.Rows[i]["mouldno"].ToString().Trim();
                            //  oporow["GRP"] = dticode.Rows[i]["GRP"].ToString().Trim();
                            oporow["compname"] = dticode.Rows[i]["compname"].ToString().Trim();

                            if (ded2.TrimStart('/') == dticode.Rows[i]["rmat"].ToString().Trim())
                            {
                                ded2 = dticode.Rows[i]["rmat"].ToString().Trim();
                            }
                            else
                            {
                                ded2 += "/" + dticode.Rows[i]["rmat"].ToString().Trim();
                            }
                            oporow["rmat"] = ded2.TrimStart('/');
                            if (ded3.TrimStart('/') == dticode.Rows[i]["rmatcontrol"].ToString().Trim())
                            {
                                ded3 = dticode.Rows[i]["rmatcontrol"].ToString().Trim();
                            }
                            else
                            {
                                ded3 += "/" + dticode.Rows[i]["rmatcontrol"].ToString().Trim();
                            }
                            oporow["rmatcontrol"] = ded3.TrimStart('/');
                            db1 = fgen.make_double(dticode.Rows[i]["totalCavity"].ToString().Trim());
                            oporow["totalCavity"] = db1;
                            oporow["runCavity"] = dticode.Rows[i]["runCavity"].ToString().Trim();
                            db2 = fgen.make_double(dticode.Rows[i]["totalShots"].ToString().Trim());
                            oporow["totalShots"] = db2;
                            // double d1 = fgen.make_double(dticode.Rows[i]["runCavity"].ToString().Trim()) * fgen.make_double(dticode.Rows[i]["totalShots"].ToString().Trim());
                            db3 += fgen.make_double(dticode.Rows[i]["qtyprod"].ToString().Trim());
                            oporow["qtyprod"] = db3;
                            db4 += fgen.make_double(dticode.Rows[i]["qtyRej"].ToString().Trim());
                            oporow["qtyRej"] = db4;
                            db5 += fgen.make_double(dticode.Rows[i]["okComp"].ToString().Trim());
                            oporow["okComp"] = db5;
                            db6 += fgen.make_double(dticode.Rows[i]["brkdown"].ToString().Trim());
                            oporow["brkdown"] = db6;
                            if (ded4.TrimStart('/') == dticode.Rows[i]["brkdownCode"].ToString().Trim())
                            {
                                ded4 = dticode.Rows[i]["brkdownCode"].ToString().Trim();
                            }
                            else
                            {
                                ded4 += "/" + dticode.Rows[i]["brkdownCode"].ToString().Trim();
                            }

                            oporow["brkdownCode"] = ded4.TrimStart('/');
                            if (ded5.TrimStart('/') == dticode.Rows[i]["pname"].ToString().Trim())
                            {
                                ded5 = dticode.Rows[i]["pname"].ToString().Trim();
                            }
                            else
                            {
                                ded5 += "/" + dticode.Rows[i]["pname"].ToString().Trim();
                            }
                            oporow["pname"] = ded5.TrimStart('/');
                            oporow["mchFrom"] = dticode.Rows[i]["mchFrom"].ToString().Trim();
                            oporow["mchTo"] = dticode.Rows[i]["mchTo"].ToString().Trim();
                            db9 += fgen.make_double(dticode.Rows[i]["comp"].ToString().Trim());
                            oporow["comp"] = db9;
                            db10 += fgen.make_double(dticode.Rows[i]["runner"].ToString().Trim());
                            oporow["runner"] = db10;
                            db11 += fgen.make_double(dticode.Rows[i]["lumps"].ToString().Trim());
                            oporow["lumps"] = db11;
                            oporow["remarks"] = dticode.Rows[i]["remarks"].ToString().Trim();
                            oporow["vchdate"] = dticode.Rows[i]["vchdate"].ToString().Trim();
                            oporow["vch"] = dticode.Rows[i]["vch"].ToString().Trim();
                            if (ded6.TrimStart('/') == dticode.Rows[i]["shift"].ToString().Trim())
                            {
                                ded6 = dticode.Rows[i]["shift"].ToString().Trim();
                            }
                            else
                            {
                                ded6 += "/" + dticode.Rows[i]["shift"].ToString().Trim();
                            }
                            oporow["shift"] = ded6.TrimStart('/');
                        }
                        dtm.Rows.Add(oporow);
                    }
                }
                view1im = new DataView(dtm);
                view1im.Sort = "mcno,mchFrom";
                dtdrsim = new DataTable();
                dtdrsim = view1im.ToTable();
                if (dtdrsim.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dtdrsim.TableName = "Prepcur";
                    dsRep.Tables.Add(dtdrsim);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "CrptDispoShiftProd", "CrptDispoShiftProd", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39226"://okmg for disp prod sheet type='90' other moldings '61' changed yogita to change rpt
                header_n = "Production Summary Report";
                if (frm_cocd == "DISP")
                {
                    vartype = "90";
                }
                SQuery = "SELECT DISTINCT '" + header_n + "' AS HEADER, E.NAME AS SHIFTNAME, A.*,B.INAME AS RNAME,D.INAME AS COMP_NAME,(A.IQTYIN+A.MLT_LOSS) AS PROD,C.BTCHNO AS LOT_NO FROM  PROD_SHEET A,ITEM B,IVOUCHER C ,ITEM D,TYPE E WHERE TRIM(A.VCHNUM)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=TRIM(C.VCHNUM)||to_Char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) AND TRIM(A.SHFTCODE)=TRIM(E.TYPE1) and TRIM(A.ICODE)=TRIM(D.ICODE) AND TRIM(B.ICODE)=TRIM(C.ICODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND C.TYPE='39' AND E.ID='D' AND A.VCHDATE " + xprdRange + " ORDER BY A.VCHNUM";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "CrptDispoShiftProd", "CrptDispoShiftProd", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39227"://okmg change type 61 for molding
                header_n = "Rejection Analysis (Moulding)";
                if (frm_cocd == "DISP")
                {
                    vartype = "90";
                }
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' AS HEADER, A.BRANCHCD,A.MCHCODE,B.INAME,SUM(A.A1) AS Sort,SUM(A.A2) AS blck,SUM(A.A3) AS airbuble,SUM(A.A4) AS Airtrap,SUM(A.A5) AS Flas,SUM(A.A6) AS brnmsk,SUM(A.A7) AS silvr,SUM(A.A8) AS scrach,SUM(A.A9) AS part,SUM(A.A10) AS color,SUM(A.A11) AS rustmrk,SUM(A.A12)  AS qc, SUM(A.A13) AS other,SUM(A.A1+A.A2+A.A3+A.A4+A.A5+A.A6+A.A7+A.A8+A.A9+A.A10+A.A11+A.A12+A.A13) AS REJ,SUM(A.IQTYIN) AS QTY FROM PROD_SHEET A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " GROUP BY  A.BRANCHCD ,A.MCHCODE,B.INAME ORDER BY A.MCHCODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "REJECTION_DISP", "REJECTION_DISP", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39228"://okmg not running on riki no type 55 in inspvch
                #region Monthly BreakDown
                ded1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                string next_year = "";
                if (Convert.ToInt32(ded1) <= 3)
                {
                    int a = Convert.ToInt32(frm_myear);
                    a = a + 1;
                    next_year = Convert.ToString(a);
                }
                else
                {
                    next_year = frm_myear;
                }
                int days1 = DateTime.DaysInMonth(Convert.ToInt32(next_year), Convert.ToInt32(ded1));
                if (frm_cocd == "DISP")
                {
                    vartype = "90";
                }
                SQuery = "SELECT DISTINCT  A.BRANCHCD AS FSTR,to_char(A.vchdate,'dd') as day,'" + ded1 + "' as mon,C.MCHNAME,B.COL1,B.COL2,SUM(B.COL3) AS COL3 FROM PROD_SHEET A,INSPVCH B ,PMAINT C WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND TO_CHAR(A.VCHDATE,'MM/YYYY')='" + ded1 + "/" + next_year + "' AND  TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(C.MCHCODE)=TRIM(A.MCHCODE)  AND B.TYPE='55' AND C.TYPE='10' GROUP BY A.BRANCHCD ,to_char(A.vchdate,'dd'),C.MCHNAME,B.COL1,B.COL2  ORDER BY B.COL1";
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
                        //ded1 = ft["day"].ToString();
                        ded2 = ft["day"].ToString();
                        ft["mon"] = dt.Rows[0]["mon"].ToString().Trim();
                        dt1.Rows.Add(ft);
                    }
                }
                mq0 = ""; mq1 = ""; mq2 = "";
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (mq0.Length > 0)
                    {
                        mq0 = mq0 + ",sum(decode(TO_CHAR(A.VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',B.COL3,0)) as DAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq0 = "sum(decode(TO_CHAR(A.VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',B.COL3,0)) as DAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq1.Length > 0)
                    {
                        mq1 = mq1 + ",sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ") AS DAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq1 = "sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ") AS DAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    if (mq2.Length > 0)
                    {
                        mq2 = mq2 + "+sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ")";
                    }
                    else
                    {
                        mq2 = "sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ")";
                    }
                }
                SQuery = "SELECT G.MTHNAME,F.ENT_BY, F.MCHNAME,F.COL1,F.COL2,sum(F.COL3) as COL3,F.MTH,'" + next_year + "' as month_year," + mq1 + "," + mq2 + " as total,ROUND((" + mq2 + ")/31,4) AS AVERGAE  FROM  (SELECT A.BRANCHCD AS FSTR,B.COL1 ,B.COL2,SUM(B.COL3) AS COL3,A.ENT_BY,'" + ded1 + "' AS MTH,C.MCHNAME," + mq0 + "  FROM PROD_SHEET A,INSPVCH B,PMAINT C WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND TO_CHAR(A.VCHDATE,'MM/YYYY')='" + ded1 + "/" + next_year + "' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(A.MCHCODE)=TRIM(C.MCHCODE) AND B.TYPE='55' AND C.TYPE='10' GROUP BY A.BRANCHCD,B.COL1,B.COL2,TO_CHAR(A.VCHDATE,'DD'),C.MCHNAME,A.ENT_BY) F,MTHS G WHERE TRIM(F.MTH)=TRIM(G.MTHNUM) group by G.MTHNAME, F.MCHNAME,F.COL1,F.COL2,F.MTH,F.ENT_BY ORDER BY to_number(F.COL1)";
                dt5 = new DataTable();
                dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt5.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt5.TableName = "Prepcur";
                    dsRep.Tables.Add(dt5);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "monthly_breakdown", "monthly_breakdown", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39230"://okmg not running on riki
                #region Rejection Transfer Slip
                SQuery = "Select a.*,b.iname,B.IWEIGHT AS bIWEIGHT,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit AS CUNIT1,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCH from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('15') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + "  order by a.srno";
                mq0 = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit as cunit from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "'  and a.type in ('39') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + " AND IQTYOUT>0 order by a.srno";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                mdt = new DataTable();
                mdt.Columns.Add(new DataColumn("Item_Code", typeof(string)));
                mdt.Columns.Add(new DataColumn("Item_Name", typeof(string)));
                mdt.Columns.Add(new DataColumn("RawMaterial", typeof(string)));
                mdt.Columns.Add(new DataColumn("Lot", typeof(string)));
                mdt.Columns.Add(new DataColumn("MANO", typeof(string)));
                mdt.Columns.Add(new DataColumn("Component", typeof(double)));
                mdt.Columns.Add(new DataColumn("Runner", typeof(double)));
                mdt.Columns.Add(new DataColumn("Lumps", typeof(double)));
                mdt.Columns.Add(new DataColumn("Remarks", typeof(string)));
                mdt.Columns.Add(new DataColumn("F", typeof(string)));
                mdt.Columns.Add(new DataColumn("T", typeof(string)));
                mdt.Columns.Add(new DataColumn("SlipNo", typeof(string)));
                mdt.Columns.Add(new DataColumn("Vchdate", typeof(string)));
                mdt.Columns.Add(new DataColumn("VCH", typeof(string)));
                oporow = null;
                if (dt.Rows.Count > 0 && dt2.Rows.Count > 0)
                {
                    db1 = 0; db2 = 0; db3 = 0; db4 = 0; ded1 = ""; ded2 = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        oporow = mdt.NewRow();
                        oporow["Item_Code"] = dr["icode"].ToString().Trim();
                        oporow["Item_Name"] = dr["iname"].ToString().Trim();
                        oporow["RawMaterial"] = fgen.seek_iname_dt(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "iname");
                        oporow["Lot"] = fgen.seek_iname_dt(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "btchno");
                        db1 = fgen.make_double(dr["rej_rw"].ToString().Trim());
                        db2 = fgen.make_double(dr["biweight"].ToString().Trim());
                        oporow["Component"] = Math.Round(db1 * db2, 6);
                        db3 = fgen.make_double(dr["shots"].ToString().Trim());//act shots
                        db4 = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ioqty from itemosp where trim(icodE)='" + dr["icode"].ToString().Trim() + "'", "ioqty")); //rrwt/pc
                        db5 = fgen.make_double(dr["ipack"].ToString().Trim());//cavity
                        //oporow["Runner"] = Math.Round(db3 * db4, 6); //OLD
                        oporow["Runner"] = Math.Round(db3 * db4 * db5, 6); //Change  1-nov-2019
                        oporow["Lumps"] = fgen.make_double(dr["rej_sdp"].ToString().Trim());
                        oporow["Remarks"] = "";
                        oporow["vchdate"] = dr["vchDATE"].ToString().Trim();
                        oporow["vch"] = dr["vch"].ToString().Trim();
                        mdt.Rows.Add(oporow);
                    }
                }
                if (mdt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    mdt.TableName = "Prepcur";
                    dsRep.Tables.Add(mdt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "crtpDisp_Rejection", "crtpDisp_Rejection", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;


            case "F39231"://okmg not for agrm as componenet store is not there
                #region Moulding to component Store
                SQuery = "Select a.*,b.iname,B.IWEIGHT AS bIWEIGHT,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit AS CUNIT1,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCH from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd= '" + frm_mbr + "' and a.type in ('15') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + " order by a.srno ";
                mq0 = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('39') and a.stage='61' and a.store='W' and a.vchdate " + xprdRange + " and  IQTYOUT>0 order by a.srno ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                mdt = new DataTable();
                mdt.Columns.Add(new DataColumn("Name_of_Component", typeof(string)));
                mdt.Columns.Add(new DataColumn("OK_Qty", typeof(double)));
                mdt.Columns.Add(new DataColumn("Lot", typeof(string)));
                mdt.Columns.Add(new DataColumn("Control", typeof(string)));
                mdt.Columns.Add(new DataColumn("Remarks", typeof(string)));
                mdt.Columns.Add(new DataColumn("SlipNo", typeof(string)));
                mdt.Columns.Add(new DataColumn("Vchdate", typeof(string)));
                mdt.Columns.Add(new DataColumn("VCH", typeof(string)));
                oporow = null;
                if (dt.Rows.Count > 0 && dt2.Rows.Count > 0)
                {
                    db1 = 0; db2 = 0; db3 = 0; db4 = 0; ded1 = ""; ded2 = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        oporow = mdt.NewRow();
                        oporow["Name_of_Component"] = dr["iname"].ToString().Trim();
                        oporow["Control"] = fgen.seek_iname_dt(dt2, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "btchno");
                        db1 = fgen.make_double(dr["iqtyin"].ToString().Trim());
                        oporow["Ok_Qty"] = db1;
                        oporow["Lot"] = dr["btchno"].ToString().Trim();
                        oporow["Remarks"] = "";
                        oporow["vchdate"] = dr["vchDATE"].ToString().Trim();
                        oporow["vch"] = dr["vch"].ToString().Trim();
                        mdt.Rows.Add(oporow);
                    }
                }
                if (mdt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    mdt.TableName = "Prepcur";
                    dsRep.Tables.Add(mdt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "crtpDisp_Component", "crtpDisp_Component", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39232"://okmg
                #region  Mould Utilization report
                ded1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (Convert.ToInt32(ded1) <= 3)
                {
                    int a = Convert.ToInt32(frm_myear);
                    a = a + 1;
                    next_year = Convert.ToString(a);
                }
                else
                {
                    next_year = frm_myear;
                }
                int days = DateTime.DaysInMonth(Convert.ToInt32(next_year), Convert.ToInt32(ded1));
                if (frm_cocd == "DISP")
                {
                    vartype = "90";
                }
                SQuery = "SELECT distinct branchcd as fstr,to_char(vchdate,'dd') as day,sum(iqtyin) as iqtyin,mchcode,ename as machine,LMD,BCD FROM PROD_SHEET WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + vartype + "' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + ded1 + "/" + next_year + "' group by to_char(vchdate,'dd'),mchcode,ename,branchcd,LMD,BCD";
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
                        ded2 = ft["day"].ToString();
                        dt1.Rows.Add(ft);
                    }
                }
                mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = ""; mq12 = "";
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (mq0.Length > 0)
                    {
                        mq0 = mq0 + ",sum(decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',BCD,0)) as BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq0 = "sum(decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',BCD,0)) as BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq3.Length > 0)
                    {
                        mq3 = mq3 + ",sum(decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',LMD,0)) as LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq3 = "sum(decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',LMD,0)) as LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }


                    if (mq1.Length > 0)
                    {

                        mq1 += ",(case when LDAY" + dt1.Rows[j]["day"].ToString().Trim() + ">0 then  Round((BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "/LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "*100),4) else 0 end) AS Day" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq1 = "(case when LDAY" + dt1.Rows[j]["day"].ToString().Trim() + ">0 then  Round((BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "/LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "*100),4) else 0 end) AS Day" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    if (mq2.Length > 0)
                    {

                        mq2 += ",sum(case when LDAY" + dt1.Rows[j]["day"].ToString().Trim() + ">0 then  Round((BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "/LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "*100),4) else 0 end) AS Day" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq2 = "sum(case when LDAY" + dt1.Rows[j]["day"].ToString().Trim() + ">0 then  Round((BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "/LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "*100),4) else 0 end) AS Day" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq6.Length > 0)
                    {
                        mq6 = mq6 + ",BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq6 = "BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq7.Length > 0)
                    {
                        mq7 = mq7 + ",LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq7 = "LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq11.Length > 0)
                    {
                        mq11 = mq11 + ",sum(BDAY" + dt1.Rows[j]["day"].ToString().Trim() + ") as BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq11 = "sum(BDAY" + dt1.Rows[j]["day"].ToString().Trim() + ") as BDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq12.Length > 0)
                    {
                        mq12 = mq12 + ",sum(LDAY" + dt1.Rows[j]["day"].ToString().Trim() + ") as LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq12 = "sum(LDAY" + dt1.Rows[j]["day"].ToString().Trim() + ") as LDAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq8.Length > 0)
                    {
                        mq8 = mq8 + ",DAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq8 = "DAY" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }

                    if (mq9.Length > 0)
                    {
                        mq9 = mq9 + "+sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ")";
                    }
                    else
                    {
                        mq9 = "sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ")";
                    }

                    if (mq10.Length > 0)
                    {
                        mq10 = mq10 + ",sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ") AS DAY" + dt1.Rows[j]["day"].ToString().Trim() + " ";
                    }
                    else
                    {
                        mq10 = "sum(DAY" + dt1.Rows[j]["day"].ToString().Trim() + ") AS DAY" + dt1.Rows[j]["day"].ToString().Trim() + " ";
                    }
                }
                SQuery = "SELECT MTHNAME,ENAME,month_year,SUM(WORKDAY) AS WORKDAY,MTH," + mq9 + " AS TOTAL," + mq10 + " from (SELECT B.MTHNAME,A.ENAME,A.month_year,COUNT(DISTINCT A.VCHDATE) AS WORKDAY,A.MTH, " + mq1 + " FROM (select FSTR, VCHDATE, month_year, MTH,ename," + mq11 + "," + mq12 + "   FROM (select DISTINCT branchcd as fstr,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,'" + next_year + "' as month_year,'" + ded1 + "' AS MTH,ename," + mq0 + "," + mq3 + " from prod_sheet where BRANCHCD='" + frm_mbr + "' and type='" + vartype + "' and to_char(vchdate,'MM/yyyy')='" + ded1 + "/" + next_year + "' GROUP BY BRANCHCD,ICODE,ENAME,TO_CHAR(VCHDATE,'DD/MM/YYYY') ) GROUP BY FSTR, VCHDATE,ename, month_year, MTH )A ,MTHS B WHERE TRIM(A.MTH)=TRIM(MTHNUM) GROUP BY A.ENAME,A.month_year,A.MTH,B.MTHNAME," + mq6 + "," + mq7 + ") group by MTHNAME,ENAME,month_year,MTH order by ename";
                dt5 = new DataTable();
                dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt5.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt5.TableName = "Prepcur";
                    dsRep.Tables.Add(dt5);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "MOULD_UTIL", "MOULD_UTIL", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39144":
                header_n = "Details Of DownTime";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' AS HEADER,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, a.ename,a.icode,round(((num1+num2+num3+num4+num5+num6+num7+num8+num9+num10+num11+num12)/60),2) as time , round((((num1+num2+num3+num4+num5+num6+num7+num8+num9+num10+num11+num12)/60)*100),2) as val,b.cpartno FROM PROD_SHEET a , item b WHERE trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + vartype + "'  AND a.VCHDATE " + xprdRange + " ORDER BY VCHNUM";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Details_Downtime", "std_Details_Downtime", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            #region Made by Madhvi on 03 Aug 2018
            case "F39233": // Month Wise
            case "F39234": // Item Wise
                if (hfhcid.Value == "F39233")
                {
                    header_n = "Rejection Reason Analysis (Month Wise)";
                }
                else
                {
                    header_n = "Rejection Reason Analysis (Item Wise)";
                }
                //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,TRIM(a.ICODE) AS ICODE,I.INAME,I.CPARTNO,I.UNIT,ROUND((((SUM(A.IQTYIN+A.MLT_LOSS))-(SUM(A.IQTYIN)))/SUM(A.IQTYIN+A.MLT_LOSS))*1000000,0) AS PPM,round(SUM(A.IQTYIN+A.MLT_LOSS),2) AS TOTPROD,round(SUM(A.A1),2)+round(SUM(A.A2),2)+round(SUM(A.A3),2)+round(SUM(A.A4),2)+round(SUM(A.A5),2)+round(SUM(A.A6),2)+round(SUM(A.A7),2)+round(SUM(A.A8),2)+round(SUM(A.A9),2)+round(SUM(A.A10),2)+round(SUM(A.A11),2)+round(SUM(A.A12),2)+round(SUM(A.A13),2)+round(SUM(A.A14),2)+round(SUM(A.A15),2)+round(SUM(A.A16),2)+round(SUM(A.A17),2)+round(SUM(A.A18),2)+round(SUM(A.A19),2)+round(SUM(A.A20),2) AS TOTREJ,round(SUM(A.A1),2) AS A1,round(SUM(A.A2),2) AS A2,round(SUM(A.A3),2) AS A3,round(SUM(A.A4),2) AS A4,round(SUM(A.A5),2) AS A5,round(SUM(A.A6),2) AS A6,round(SUM(A.A7),2) AS A7,round(SUM(A.A8),2) AS A8,round(SUM(A.A9),2) AS A9,round(SUM(A.A10),2) AS A10,round(SUM(A.A11),2) AS A11/*,SUM(A.A12) AS A12,SUM(A.A13) AS A13,SUM(A.A14) AS A14,SUM(A.A15) AS A15*/,round((SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20)),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),TRIM(A.ICODE),I.INAME,I.CPARTNO,I.UNIT HAVING SUM(TO_NUMBER(A.TEMPR)*A.TOTAL*A.BCD)>0 AND SUM(A.IQTYIN+A.MLT_LOSS)>0 ORDER BY INAME";///original
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,TRIM(a.ICODE) AS ICODE,I.INAME,I.CPARTNO,I.UNIT,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11/*,SUM(nvl(A.A12,0)) AS A12,SUM(nvl(A.A13,0)) AS A13,SUM(nvl(A.A14,0)) AS A14,SUM(nvl(A.A15,0)) AS A15*/,round((SUM(nvl(A.A12,0))+SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0))),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.brachcd'" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),TRIM(A.ICODE),I.INAME,I.CPARTNO,I.UNIT HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY INAME";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,TRIM(a.ICODE) AS ICODE,I.INAME,I.CPARTNO,I.UNIT,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11/*,SUM(nvl(A.A12,0)) AS A12,SUM(nvl(A.A13,0)) AS A13,SUM(nvl(A.A14,0)) AS A14,SUM(nvl(A.A15,0)) AS A15*/,round((SUM(nvl(A.A12,0))+SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0))),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.branchcd='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),TRIM(A.ICODE),I.INAME,I.CPARTNO,I.UNIT HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY INAME";

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
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='RJC61' and rownum<12 order by code");
                    int k = 11;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {
                            for (int i = 0; i < k; i++)
                            {
                                dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                            }
                        }
                        catch { }
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    if (hfhcid.Value == "F39233")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "RejReason_Anly_MonthWise", "RejReason_Anly_MonthWise", dsRep, "");
                    }
                    else if (hfhcid.Value == "F39234")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "RejReason_Anly_ItemWise", "RejReason_Anly_ItemWise", dsRep, "");
                    }
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39235":
                header_n = "Rejection Reason Analysis (Machine Wise)";
                //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,A.ENAME AS MACHINE,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(A.IQTYIN+A.MLT_LOSS))-(SUM(A.IQTYIN)))/SUM(A.IQTYIN+A.MLT_LOSS))*1000000,0) AS PPM,SUM(A.IQTYIN+A.MLT_LOSS) AS TOTPROD,round(SUM(A.IQTYIN+A.MLT_LOSS),2) AS TOTPROD,round(SUM(A.A1),2)+round(SUM(A.A2),2)+round(SUM(A.A3),2)+round(SUM(A.A4),2)+round(SUM(A.A5),2)+round(SUM(A.A6),2)+round(SUM(A.A7),2)+round(SUM(A.A8),2)+round(SUM(A.A9),2)+round(SUM(A.A10),2)+round(SUM(A.A11),2)+round(SUM(A.A12),2)+round(SUM(A.A13),2)+round(SUM(A.A14),2)+round(SUM(A.A15),2)+round(SUM(A.A16),2)+round(SUM(A.A17),2)+round(SUM(A.A18),2)+round(SUM(A.A19),2)+round(SUM(A.A20),2) AS TOTREJ,round(SUM(A.A1),2) AS A1,round(SUM(A.A2),2) AS A2,round(SUM(A.A3),2) AS A3,round(SUM(A.A4),2) AS A4,round(SUM(A.A5),2) AS A5,round(SUM(A.A6),2) AS A6,round(SUM(A.A7),2) AS A7,round(SUM(A.A8),2) AS A8,round(SUM(A.A9),2) AS A9,round(SUM(A.A10),2) AS A10,round(SUM(A.A11),2) AS A11/*,SUM(A.A12) AS A12,SUM(A.A13) AS A13,SUM(A.A14) AS A14,SUM(A.A15) AS A15*/,round((SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20)),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.ENAME HAVING SUM(TO_NUMBER(A.TEMPR)*A.TOTAL*A.BCD)>0 AND SUM(A.IQTYIN+A.MLT_LOSS)>0 ORDER BY MACHINE";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,A.ENAME AS MACHINE,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)) AS TOTPROD,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD1,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,round((SUM(nvl(A.A12,0))+SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0))),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.ENAME HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY MACHINE";
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
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='RJC61' and rownum<12 order by code");
                    int k = 11;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {
                            for (int i = 0; i < k; i++)
                            {
                                dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                            }
                        }
                        catch { }
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "RejReason_Anly_MachineWise", "RejReason_Anly_MachineWise", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39236":
                header_n = "Rejection Reason Analysis (Shift Wise)";
                //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,A.VAR_CODE AS SHIFT,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(A.IQTYIN+A.MLT_LOSS))-(SUM(A.IQTYIN)))/SUM(A.IQTYIN+A.MLT_LOSS))*1000000,0) AS PPM,round(SUM(A.IQTYIN+A.MLT_LOSS),2) AS TOTPROD,round(SUM(A.A1),2)+round(SUM(A.A2),2)+round(SUM(A.A3),2)+round(SUM(A.A4),2)+round(SUM(A.A5),2)+round(SUM(A.A6),2)+round(SUM(A.A7),2)+round(SUM(A.A8),2)+round(SUM(A.A9),2)+round(SUM(A.A10),2)+round(SUM(A.A11),2)+round(SUM(A.A12),2)+round(SUM(A.A13),2)+round(SUM(A.A14),2)+round(SUM(A.A15),2)+round(SUM(A.A16),2)+round(SUM(A.A17),2)+round(SUM(A.A18),2)+round(SUM(A.A19),2)+round(SUM(A.A20),2) AS TOTREJ,round(SUM(A.A1),2) AS A1,round(SUM(A.A2),2) AS A2,round(SUM(A.A3),2) AS A3,round(SUM(A.A4),2) AS A4,round(SUM(A.A5),2) AS A5,round(SUM(A.A6),2) AS A6,round(SUM(A.A7),2) AS A7,round(SUM(A.A8),2) AS A8,round(SUM(A.A9),2) AS A9,round(SUM(A.A10),2) AS A10,round(SUM(A.A11),2) AS A11/*,SUM(A.A12) AS A12,SUM(A.A13) AS A13,SUM(A.A14) AS A14,SUM(A.A15) AS A15*/,round((SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20)),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.VAR_CODE HAVING SUM(TO_NUMBER(A.TEMPR)*A.TOTAL*A.BCD)>0 AND SUM(A.IQTYIN+A.MLT_LOSS)>0 ORDER BY SHIFT";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header, A.VAR_CODE AS SHIFT,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,round((SUM(nvl(A.A12,0))+SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0))),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.VAR_CODE HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY SHIFT";
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
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='RJC61' and rownum<12 order by code");
                    int k = 11;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {
                            for (int i = 0; i < k; i++)
                            {
                                dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                            }
                        }
                        catch { }
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "RejReason_Anly_ShiftWise", "RejReason_Anly_ShiftWise", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39237":
                header_n = "Rejection Reason Analysis (Sub Group Wise)";
                //SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,TRIM(a.ICODE) AS ICODE,I.INAME,I.CPARTNO,I.UNIT,SUBSTR(TRIM(A.ICODE),1,4) AS SUBGRP,B.INAME AS SUBNAME,ROUND((((SUM(A.IQTYIN+A.MLT_LOSS))-(SUM(A.IQTYIN)))/SUM(A.IQTYIN+A.MLT_LOSS))*1000000,0) AS PPM,round(SUM(A.IQTYIN+A.MLT_LOSS),2) AS TOTPROD,round(SUM(A.A1),2)+round(SUM(A.A2),2)+round(SUM(A.A3),2)+round(SUM(A.A4),2)+round(SUM(A.A5),2)+round(SUM(A.A6),2)+round(SUM(A.A7),2)+round(SUM(A.A8),2)+round(SUM(A.A9),2)+round(SUM(A.A10),2)+round(SUM(A.A11),2)+round(SUM(A.A12),2)+round(SUM(A.A13),2)+round(SUM(A.A14),2)+round(SUM(A.A15),2)+round(SUM(A.A16),2)+round(SUM(A.A17),2)+round(SUM(A.A18),2)+round(SUM(A.A19),2)+round(SUM(A.A20),2) AS TOTREJ,round(SUM(A.A1),2) AS A1,round(SUM(A.A2),2) AS A2,round(SUM(A.A3),2) AS A3,round(SUM(A.A4),2) AS A4,round(SUM(A.A5),2) AS A5,round(SUM(A.A6),2) AS A6,round(SUM(A.A7),2) AS A7,round(SUM(A.A8),2) AS A8,round(SUM(A.A9),2) AS A9,round(SUM(A.A10),2) AS A10,round(SUM(A.A11),2) AS A11/*,SUM(A.A12) AS A12,SUM(A.A13) AS A13,SUM(A.A14) AS A14,SUM(A.A15) AS A15*/,round((SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20)),2) AS OTH FROM PROD_SHEET A,ITEM I,ITEM B WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,4)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),SUBSTR(TRIM(A.ICODE),1,4),B.INAME,TRIM(a.ICODE),I.INAME,I.CPARTNO,I.UNIT HAVING SUM(TO_NUMBER(A.TEMPR)*A.TOTAL*A.BCD)>0 AND SUM(A.IQTYIN+A.MLT_LOSS)>0 ORDER BY SUBNAME";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,TRIM(a.ICODE) AS ICODE,I.INAME,I.CPARTNO,I.UNIT,SUBSTR(TRIM(A.ICODE),1,4) AS SUBGRP,B.INAME AS SUBNAME,ROUND((((SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))-(SUM(nvl(A.IQTYIN,0))))/SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)))*1000000,0) AS PPM,round(SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)),2) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,round(SUM(nvl(A.A1,0)),2) AS A1,round(SUM(nvl(A.A2,0)),2) AS A2,round(SUM(nvl(A.A3,0)),2) AS A3,round(SUM(nvl(A.A4,0)),2) AS A4,round(SUM(nvl(A.A5,0)),2) AS A5,round(SUM(nvl(A.A6,0)),2) AS A6,round(SUM(nvl(A.A7,0)),2) AS A7,round(SUM(nvl(A.A8,0)),2) AS A8,round(SUM(nvl(A.A9,0)),2) AS A9,round(SUM(nvl(A.A10,0)),2) AS A10,round(SUM(nvl(A.A11,0)),2) AS A11,round((SUM(nvl(A.A12,0))+SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0))),2) AS OTH  FROM PROD_SHEET A,ITEM I,ITEM B WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,4)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + vartype + "' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),SUBSTR(TRIM(A.ICODE),1,4),B.INAME,TRIM(a.ICODE),I.INAME,I.CPARTNO,I.UNIT HAVING SUM(TO_NUMBER(A.TEMPR)*nvl(A.TOTAL,0)*nvl(A.BCD,0))>0 AND SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0))>0 ORDER BY SUBNAME";
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
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='RJC61' and rownum<12 order by code");
                    int k = 11;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {
                            for (int i = 0; i < k; i++)
                            {
                                dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                            }
                        }
                        catch { }
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "RejReason_Anly_SubGrpWise", "RejReason_Anly_SubGrpWise", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;
            #endregion

            case "F39238":
                #region production summary report shift and mach wise
                //need icon for this report ...
                header_n = "Prod. Summary- Shift & M/c Wise";
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("fromdt", typeof(string));
                ph_tbl.Columns.Add("todt", typeof(string));
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("machine", typeof(string));
                ph_tbl.Columns.Add("shift", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("target", typeof(double));
                ph_tbl.Columns.Add("tot_prod", typeof(double));
                ph_tbl.Columns.Add("rejn", typeof(double));
                ph_tbl.Columns.Add("net_prod", typeof(double));
                ph_tbl.Columns.Add("Prodp", typeof(double));

                //SQuery = "select A.ENAME AS MACH,A.VAR_CODE AS SHIFT,TRIM(A.ICODE) AS ICODE,B.INAME,B.CPARTNO,round(sum(to_number(a.tempr)*a.total*a.bcd),2) as target,round(sum(a.iqtyin+a.mlt_loss),2) as TOT_PRD,round(sum(a.mlt_loss),2) as REJ,round(sum(a.iqtyin),2) as net_prod,(case when (sum(to_number(a.tempr)*a.total*a.bcd) > 0) then round((sum(a.iqtyin)/sum(to_number(a.tempr)*a.total*a.bcd))*100,2) else 0 end) as Prodp  from prod_sheet a, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdRange + " group by A.ENAME,A.VAR_CODE,TRIM(A.ICODE),B.INAME,B.CPARTNO order by A.VAR_CODE";
                SQuery = "select A.ENAME AS MACH,A.VAR_CODE AS SHIFT,TRIM(A.ICODE) AS ICODE,B.INAME,B.CPARTNO,round(sum(to_number(a.tempr)*nvl(a.total,0)*nvl(a.bcd,0)),2) as target,round(sum(nvl(a.iqtyin,0)+nvl(a.mlt_loss,0)),2) as TOT_PRD,round(sum(nvl(a.mlt_loss,0)),2) as REJ,round(sum(nvl(a.iqtyin,0)),2) as net_prod,(case when (sum(to_number(a.tempr)*nvl(a.total,0)*nvl(a.bcd,0)) > 0) then round((sum(nvl(a.iqtyin,0))/sum(to_number(a.tempr)*nvl(a.total,0)*nvl(a.bcd,0)))*100,2) else 0 end) as Prodp  from prod_sheet a, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdRange + " group by A.ENAME,A.VAR_CODE,TRIM(A.ICODE),B.INAME,B.CPARTNO order by A.VAR_CODE";
                ////SQuery = "SELECT TRIM(A.ICODE) AS ICODE,B.INAME,B.CPARTNO,A.ENAME AS MACH,A.VAR_CODE AS SHIFT,SUM(A.UN_MELT) AS MELT,SUM(A.MLT_LOSS) AS REJ ,SUM(IQTYIN) AS TOT_PRD  FROM PROD_SHEET A ,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND TYPE='61' AND VCHDate " + xprdRange + "  GROUP BY TRIM(A.ICODE),B.INAME,B.CPARTNO,A.ENAME,A.VAR_CODE ORDER BY shift desc";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
                    dr1 = ph_tbl.NewRow();
                    dr1["fromdt"] = fromdt;
                    dr1["todt"] = todt;
                    dr1["header"] = header_n;
                    dr1["machine"] = dt.Rows[i]["MACH"].ToString().Trim();
                    dr1["shift"] = dt.Rows[i]["shift"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["CPARTNO"].ToString().Trim();
                    db = fgen.make_double(dt.Rows[i]["TOT_PRD"].ToString().Trim());
                    db1 = fgen.make_double(dt.Rows[i]["rej"].ToString().Trim());
                    db2 = fgen.make_double(dt.Rows[i]["target"].ToString().Trim());
                    db3 = fgen.make_double(dt.Rows[i]["prodp"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["net_prod"].ToString().Trim());
                    dr1["tot_prod"] = db;
                    dr1["rejn"] = db1;
                    dr1["target"] = db2;
                    dr1["net_prod"] = db4;
                    dr1["prodp"] = db3;
                    ph_tbl.Rows.Add(dr1);
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "mold_prd_smry", "mold_prd_smry", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39239":
                #region yogita 04.08.2018
                header_n = "Prodn,Rej,OEE(M/c,Item,Year)";
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("fromdt", typeof(string));
                ph_tbl.Columns.Add("todt", typeof(string));
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("mch", typeof(string));
                ph_tbl.Columns.Add("target_prd", typeof(double));
                ph_tbl.Columns.Add("tot_prd", typeof(double));

                ph_tbl.Columns.Add("rej", typeof(double));
                ph_tbl.Columns.Add("ok_prd", typeof(double));
                ph_tbl.Columns.Add("ppm", typeof(double));
                ph_tbl.Columns.Add("prd_ef", typeof(double));
                ph_tbl.Columns.Add("oee", typeof(double));
                ph_tbl.Columns.Add("hr_work", typeof(double));

                ph_tbl.Columns.Add("non_prd", typeof(double));
                ph_tbl.Columns.Add("util_ratio", typeof(double));

                //SQuery = "select a.Ename as Machine,sum(to_number(tempr)*total*bcd) as TargetPrd,round(sum(a.iqtyin+a.mlt_loss),2) as Tot_prod,round(sum(a.mlt_loss),2) as Rejection,round(sum(a.iqtyin),2) as OK_prod,(case when (sum(a.iqtyin+a.mlt_loss)) > 0 then round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) else 0 end) as PPM,(case when ((sum(to_number(tempr)*total*bcd)) > 0) then round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) else 0 end) as Prod_ef, case when (sum(to_number(tempr)*total*bcd)=0 or sum(to_number(tempr)*total*lmd)=0) then 0 else (((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100) end as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,case when sum(a.total*a.fm_fact)> 0 then round(round(((sum(a.total*a.fm_fact)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total*a.fm_fact),2)*100 else 0 end as Util_Ratio from prod_sheet a where a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdRange + "  group by a.branchcd,a.Ename order by a.Ename";
                SQuery = "select a.Ename as Machine,sum(to_number(tempr)*nvl(total,0)*nvl(bcd,0)) as TargetPrd,round(sum(nvl(a.iqtyin,0)+nvl(a.mlt_loss,0)),2) as Tot_prod,round(sum(nvl(a.mlt_loss,0)),2) as Rejection,round(sum(nvl(a.iqtyin,0)),2) as OK_prod,(case when (sum(nvl(a.iqtyin,0)+nvl(a.mlt_loss,0))) > 0 then round((((sum(nvl(a.iqtyin,0)+nvl(a.mlt_loss,0)))-(sum(nvl(a.iqtyin,0))))/sum(nvl(a.iqtyin,0)+nvl(a.mlt_loss,0)))*1000000,0) else 0 end) as PPM,(case when ((sum(to_number(tempr)*nvl(total,0)*nvl(bcd,0))) > 0) then round(((sum(nvl(a.iqtyin,0)+0))/(sum(to_number(tempr)*nvl(total,0)*nvl(bcd,0))))*100,2) else 0 end) as Prod_ef, case when (sum(to_number(tempr)*nvl(total,0)*nvl(bcd,0))=0 or sum(to_number(tempr)*nvl(total,0)*nvl(lmd,0))=0) then 0 else (((round(((sum(to_number(tempr)*nvl(total,0)*nvl(bcd,0))))/((sum(to_number(tempr)*nvl(total,0)*nvl(lmd,0)))),2)*100)*(round((sum(nvl(iqtyin,0)))/((sum(to_number(tempr)*nvl(total,0)*nvl(bcd,0)))),2)*100))/100) end as OEE,sum(nvl(a.total,0)*nvl(a.fm_fact,0)) as Hr_worked,round((sum(nvl(a.num1,0)+nvl(a.num2,0)+nvl(a.num3,0)+nvl(a.num4,0)+nvl(a.num5,0)+nvl(a.num6,0)+nvl(a.num7,0)+nvl(a.num8,0)+nvl(a.num9,0)+nvl(a.num10,0)+nvl(a.num11,0)+nvl(a.num12,0))/60),2) as Non_Prod,case when sum(nvl(a.total,0)*nvl(a.fm_fact,0))> 0 then round(round(((sum(nvl(a.total,0)*nvl(a.fm_fact,0))-sum(nvl(a.num1,0)+nvl(a.num2,0)+nvl(a.num3,0)+nvl(a.num4,0)+nvl(a.num5,0)+nvl(a.num6,0)+nvl(a.num7,0)+nvl(a.num8,0)+nvl(a.num9,0)+nvl(a.num10,0)+nvl(a.num11,0)+nvl(a.num12,0))/60)),2)/sum(nvl(a.total,0)*nvl(a.fm_fact,0)),2)*100 else 0 end as Util_Ratio from prod_sheet a where a.branchcd='" + frm_mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdRange + "  group by a.branchcd,a.Ename order by a.Ename";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["header"] = header_n;
                        dr1["mch"] = dt.Rows[i]["machine"].ToString().Trim();
                        dr1["target_prd"] = fgen.make_double(dt.Rows[i]["TargetPrd"].ToString().Trim());
                        dr1["tot_prd"] = fgen.make_double(dt.Rows[i]["Tot_prod"].ToString().Trim());

                        dr1["rej"] = fgen.make_double(dt.Rows[i]["Rejection"].ToString().Trim());
                        dr1["ok_prd"] = fgen.make_double(dt.Rows[i]["OK_prod"].ToString().Trim());
                        dr1["ppm"] = fgen.make_double(dt.Rows[i]["ppm"].ToString().Trim());
                        dr1["prd_ef"] = fgen.make_double(dt.Rows[i]["Prod_ef"].ToString().Trim());
                        dr1["oee"] = fgen.make_double(dt.Rows[i]["oee"].ToString().Trim());
                        dr1["hr_work"] = fgen.make_double(dt.Rows[i]["Hr_worked"].ToString().Trim());
                        dr1["non_prd"] = fgen.make_double(dt.Rows[i]["Non_Prod"].ToString().Trim());
                        dr1["util_ratio"] = fgen.make_double(dt.Rows[i]["Util_Ratio"].ToString().Trim());
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "prd_rejn_ppm_oee", "prd_rejn_ppm_oee", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39240":
                #region Casting Report
                header_n = "Casting Report";// thid is moulding form print btn rpt fils
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                //SQuery = "select '" + header_n + "' as header,A.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.naration as rmk,a.lmd,a.bcd,b.iname,b.cpartno as part,a.exc_time as driver,SUM(A.A1)+SUM(A.A2)+SUM(A.A3)+SUM(A.A4)+SUM(A.A5)+SUM(A.A6)+SUM(A.A7)+SUM(A.A8)+SUM(A.A9)+SUM(A.A10)+SUM(A.A11)+SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20) AS rejn,SUM(A.A1) as a1,SUM(A.A2) a2,SUM(A.A3) as a3,SUM(A.A4) as a4,SUM(A.A5) as a5,SUM(A.A6) as a6, SUM(A.A7)+SUM(A.A8)+SUM(A.A9)+SUM(A.A10)+SUM(A.A11)+SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20) as oth,sum(total) as total,sum(a.un_melt) as prdn_tgt_shot,sum(a.noups) as act_prd_shot,a.iqtyin as ok_prd,a.ename,a.var_code as shift,SUM(a.NUM1) AS  num1 ,SUM(a.NUM2) AS  num2,SUM(a.NUM3) AS num3,SUM(a.NUM4) AS num4 ,SUM(a.NUM5) AS  num5,SUM(a.NUM6) AS num6,SUM(a.NUM7)+SUM(a.NUM8)+SUM(a.NUM9)+SUM(a.NUM10)+SUM(a.NUM11)+SUM(a.NUM12) as oth1  from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq0 + "' group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.icode,a.naration,b.iname,b.cpartno,a.exc_time,a.ename,a.var_Code ,a.lmd,a.bcd ,a.iqtyin order by a.icode";
                SQuery = "select '" + header_n + "' as header,A.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.naration as rmk,nvl(a.lmd,0) as lmd,nvl(a.bcd,0) as bcd,b.iname,b.cpartno as part,a.exc_time as driver,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS rejn,SUM(nvl(A.A1,0)) as a1,SUM(nvl(A.A2,0)) a2,SUM(nvl(A.A3,0)) as a3,SUM(nvl(A.A4,0)) as a4,SUM(nvl(A.A5,0)) as a5,SUM(nvl(A.A6,0)) as a6, SUM(nvl(A.A7,0))+SUM(nvl(A.A8,0))+SUM(nvl(A.A9,0))+SUM(nvl(A.A10,0))+SUM(nvl(A.A11,0))+SUM(nvl(A.A12,0))+SUM(nvl(A.A13,0))+SUM(nvl(A.A14,0))+SUM(nvl(A.A15,0))+SUM(nvl(A.A16,0))+SUM(nvl(A.A17,0))+SUM(nvl(A.A18,0))+SUM(nvl(A.A19,0))+SUM(nvl(A.A20,0)) as oth,sum(nvl(total,0)) as total,sum(nvl(a.un_melt,0)) as prdn_tgt_shot,sum(nvl(a.noups,0)) as act_prd_shot,nvl(a.iqtyin,0) as ok_prd,a.ename,a.var_code as shift,SUM(nvl(a.NUM1,0)) AS  num1 ,SUM(nvl(a.NUM2,0)) AS  num2,SUM(nvl(a.NUM3,0)) AS num3,SUM(nvl(a.NUM4,0)) AS num4 ,SUM(nvl(a.NUM5,0)) AS  num5,SUM(nvl(a.NUM6,0)) AS num6,SUM(nvl(a.NUM7,0))+SUM(nvl(a.NUM8,0))+SUM(nvl(a.NUM9,0))+SUM(nvl(a.NUM10,0))+SUM(nvl(a.NUM11,0))+SUM(nvl(a.NUM12,0)) as oth1  from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq0 + "' group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.icode,a.naration,b.iname,b.cpartno,a.exc_time,a.ename,a.var_Code ,nvl(a.lmd,0),nvl(a.bcd,0) ,a.iqtyin order by a.icode";
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

                    //FOR DOWN TIME
                    dt.Columns.Add("DH0", typeof(string));
                    dt.Columns.Add("DH1", typeof(string));
                    dt.Columns.Add("DH2", typeof(string));
                    dt.Columns.Add("DH3", typeof(string));
                    dt.Columns.Add("DH4", typeof(string));
                    dt.Columns.Add("DH5", typeof(string));

                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='RJC61' and rownum<7 order by type1");
                    dt2 = new DataTable(); //rej headings
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='DTC61' and rownum<7 order by type1");
                    int k = 6; //down time HEADINGS
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {
                            for (int i = 0; i < k; i++)
                            {
                                dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                                dt.Rows[l]["DH" + i] = dt2.Rows[i]["name"].ToString();
                            }
                        }
                        catch { };
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "mold_prd", "mold_prd", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39241":
                header_n = "Production Summary";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(a.BCD),TRIM(B.INAME) AS INAME,SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, case when (sum(a.noups*A.BCD) > 0 ) then round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) else 0 end AS rej_PERC, case when (sum(a.un_melt*A.BCD)> 0) then round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) else 0 end AS prod_perc, case when (sum(a.noups*A.BCD) > 0) then round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) else 0 end AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode)  AND A.BRANCHCD='" + frm_mbr + "'  AND  a.type='" + vartype + "' AND  a.vchdate " + xprdRange + " group by trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.iname),TRIM(b.cpartno) ORDER BY VAR_CODE,iname ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(nvl(a.BCD,0)),TRIM(B.INAME) AS INAME,SUM(nvl(A.UN_MELT,0)*nvl(A.BCD,0)) AS TARGET , sum(nvl(A.NOUPS,0)*nvl(A.BCD,0)) AS TOTL_PRODN ,sum(nvl(a.mlt_loss,0)) AS REJN,sum(nvl(a.iqtyin,0)) AS NET_PRODN, case when (sum(nvl(a.noups,0)*nvl(A.BCD,0)) > 0 ) then round((sum(nvl(a.mlt_loss,0))/sum(nvl(a.noups,0)*nvl(A.BCD,0))*100),2) else 0 end AS rej_PERC, case when (sum(nvl(a.un_melt,0)*nvl(A.BCD,0))> 0) then round((sum(nvl(a.iqtyin,0))/sum(nvl(a.un_melt,0)*nvl(A.BCD,0))*100),2) else 0 end AS prod_perc, case when (sum(nvl(a.noups,0)*nvl(A.BCD,0)) > 0) then round((sum(nvl(a.mlt_loss,0))/sum(nvl(a.noups,0)*nvl(A.BCD,0))*1000000),2) else 0 end AS ppm,SUM(nvl(A.IRATE,0)*nvl(A.UN_MELT,0)) AS PLAN_VALUE , SUM(nvl(A.IRATE,0)*nvl(A.MLT_LOSS,0)) AS COPQ,SUM(nvl(A.IRATE,0)*nvl(A.IQTYIN,0)) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode)  AND A.BRANCHCD='" + frm_mbr + "'  AND  a.type='" + vartype + "' AND  a.vchdate " + xprdRange + " group by trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.iname),TRIM(b.cpartno) ORDER BY VAR_CODE,iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Production_summary", "std_Production_summary", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39242":
                header_n = "Details of Items Produced ";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,  A.VCHNUM , A.VCHDATE , to_char(a.vchdate,'dd') as vdd , trim(A.ICODE) AS CODE , A.IQTYIN AS PROD_QTY , B.INAME , trim(B.CPARTNO) as cpartno, (A.IQTYIN*A.IRATE) AS PROD_VALUE  FROM PROD_SHEET A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND A.IQTYIN!='0'  AND A.BRANCHCD='" + frm_mbr + "'  AND  a.type='" + vartype + "'  AND A.VCHDATE " + xprdRange + "  ORDER BY A.VCHDATE,A.VCHNUM,B.INAME ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, A.VCHNUM , A.VCHDATE , to_char(a.vchdate,'dd') as vdd , trim(A.ICODE) AS CODE , nvl(A.IQTYIN,0) AS PROD_QTY , B.INAME , trim(B.CPARTNO) as cpartno, (nvl(A.IQTYIN,0)*nvl(A.IRATE,0)) AS PROD_VALUE  FROM PROD_SHEETK A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND nvl(A.IQTYIN,0)!='0'  AND A.BRANCHCD='" + frm_mbr + "'  AND  a.type='" + vartype + "'  AND A.VCHDATE " + xprdRange + " ORDER BY A.VCHDATE,A.VCHNUM,B.INAME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Items_Produced", "std_Items_Produced", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39243":  //Production with Rej % Itemwise
                header_n = "Production with Rej % Itemwise ";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, SUM(A.MLT_LOSS) AS REJN_QTY , SUM(A.MLT_LOSS*A.IRATE) AS REJN_VALUE  , trim(A.ICODE) AS CODE , SUM(A.IQTYIN) AS PROD_QTY , B.INAME , trim(B.CPARTNO) as cpartno , SUM(A.IQTYIN*A.IRATE) AS PROD_VALUE,ROUND(((SUM(A.MLT_LOSS)/SUM(A.IQTYIN))*100),2)  AS REJ_PERC FROM PROD_SHEET A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "'  AND  a.type='" + vartype + "'  AND A.VCHDATE " + xprdRange + "  AND A.IQTYIN!='0'  AND A.MLT_LOSS!='0'  GROUP BY B.INAME,B.CPARTNO,A.ICODE ORDER BY B.INAME,A.ICODE ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,SUM(nvl(A.MLT_LOSS,0)) AS REJN_QTY , SUM(nvl(A.MLT_LOSS,0)*nvl(A.IRATE,0)) AS REJN_VALUE  , trim(A.ICODE) AS CODE , SUM(nvl(A.IQTYIN,0)) AS PROD_QTY , B.INAME , trim(B.CPARTNO) as cpartno ,SUM(nvl(A.IQTYIN,0)*nvl(A.IRATE,0)) AS PROD_VALUE,ROUND(((SUM(nvl(A.MLT_LOSS,0))/SUM(nvl(A.IQTYIN,0)))*100),2)  AS REJ_PERC FROM PROD_SHEET A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "'  AND  a.type='" + vartype + "'  AND A.VCHDATE " + xprdRange + "  AND nvl(A.IQTYIN,0)!='0'  AND nvl(A.MLT_LOSS,0)!='0'  GROUP BY B.INAME,B.CPARTNO,A.ICODE ORDER BY B.INAME,A.ICODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Prod_with_Reje", "std_Prod_with_Reje", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39245":
                header_n = "Production Summary (Month Wise)";
                //   SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, var_code as shift ,sum(target) as target_t,VCHDATE as vdd , vhd, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND(((SUM(REJN))/(SUM(TOTL_PRODN))*100),2) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  to_char(a.vchdate,'Month/ yyyy') as vhd ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE ,SUM(a.BCD),SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "'  AND type='" + vartype + "' and vchdate " + xprdRange + " group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(b.cpartno) ORDER BY VAR_CODE) group by var_code,VCHDATE,vhd order by VCHDATE";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, var_code as shift ,sum(target) as target_t,VCHDATE as vdd , vhd, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND(((SUM(REJN))/(SUM(TOTL_PRODN))*100),2) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from (select  to_char(a.vchdate,'Month/ yyyy') as vhd ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE ,SUM(nvl(a.BCD,0)),SUM(nvl(A.UN_MELT,0)*nvl(A.BCD,0)) AS TARGET,sum(nvl(A.NOUPS,0)*nvl(A.BCD,0)) AS TOTL_PRODN ,sum(nvl(a.mlt_loss,0)) AS REJN,sum(nvl(a.iqtyin,0)) AS NET_PRODN, round((sum(nvl(a.mlt_loss,0))/sum(nvl(a.noups,0)*nvl(A.BCD,0))*100),2) AS rej_PERC, round((sum(nvl(a.iqtyin,0))/sum(nvl(a.un_melt,0)*nvl(A.BCD,0))*100),2) AS prod_perc, round((sum(nvl(a.mlt_loss,0))/sum(nvl(a.noups,0)*nvl(A.BCD,0))*1000000),2) AS ppm,SUM(nvl(A.IRATE,0)*nvl(A.UN_MELT,0)) AS PLAN_VALUE , SUM (nvl(A.IRATE,0)*nvl(A.MLT_LOSS,0)) AS COPQ,SUM(nvl(A.IRATE,0)*nvl(A.IQTYIN,0)) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "'  AND type='" + vartype + "' and vchdate " + xprdRange + " group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(b.cpartno) ORDER BY VAR_CODE) group by var_code,VCHDATE,vhd order by VCHDATE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Mth_Prod_Smry", "std_Mth_Prod_Smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39246":
                header_n = "Production Summary (M/C Wise)";
                //SQuery = "SELECT '" + header_n + "' as header,  var_code as shift ,ename as machine,sum(target) as target_t,VCHDATE, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND((SUM(REJN))/(SUM(TOTL_PRODN))*100) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(a.BCD),SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode) AND  type like '61' and a.branchcd='" + frm_mbr + "' AND  vchdate BETWEEN to_date('01/04/2017','dd/mm/yyyy') AND  to_date('30/04/2017','dd/mm/yyyy') group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.cpartno) ORDER BY VAR_CODE,ename ) group by var_code,ename,VCHDATE order by ename,VCHDATE ";
                //SQuery = "SELECT '" + header_n + "' as header,  var_code as shift ,ename as machine,sum(target) as target_t,VCHDATE, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND((SUM(REJN))/(SUM(TOTL_PRODN))*100) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(a.BCD),SUM(A.UN_MELT*A.BCD) AS TARGET , sum(A.NOUPS*A.BCD) AS TOTL_PRODN ,sum(a.mlt_loss) AS REJN,sum(a.iqtyin) AS NET_PRODN, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*100),2) AS rej_PERC, round((sum(a.iqtyin)/sum(a.un_melt*A.BCD)*100),2) AS prod_perc, round((sum(a.mlt_loss)/sum(a.noups*A.BCD)*1000000),2) AS ppm,SUM(A.IRATE*A.UN_MELT) AS PLAN_VALUE , SUM (A.IRATE*A.MLT_LOSS) AS COPQ,SUM(A.IRATE*A.IQTYIN) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode)  and a.branchcd='" + frm_mbr + "' AND a.type='" + vartype + "' AND  vchdate " + xprdRange + " group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.cpartno) ORDER BY VAR_CODE,ename ) group by var_code,ename,VCHDATE order by ename,VCHDATE";
                SQuery = "SELECT '" + header_n + "' as header, var_code as shift ,ename as machine,sum(target) as target_t,VCHDATE, sum(totl_prodn) as totl_prodn,sum(rejn) as rejn,sum(net_prodn) as net_prodn,sum(copq) as copq,sum(plan_value) as plan_value,sum(prod_value) as prod_vaLue , ROUND(((SUM(NET_PRODN))/(SUM(TARGET))*100),2) AS PROD_PERCN,ROUND((SUM(REJN))/(SUM(TOTL_PRODN))*100) AS REJ_PERC , ROUND(((SUM(REJN)/SUM(TOTL_PRODN))*1000000),2)  AS PPM from ( select  TO_CHAR(A.VCHDATE,'DD/MM/YYYY')AS VCHDATE,trim(a.icode) as icode,trim(A.VAR_CODE) as VAR_CODE , TRIM(A.ENAME) AS ENAME,SUM(nvl(a.BCD,0)),SUM(nvl(A.UN_MELT,0)*nvl(A.BCD,0)) AS TARGET , sum(nvl(A.NOUPS,0)*nvl(A.BCD,0)) AS TOTL_PRODN ,sum(nvl(a.mlt_loss,0)) AS REJN,sum(nvl(a.iqtyin,0)) AS NET_PRODN, round((sum(nvl(a.mlt_loss,0))/sum(nvl(a.noups,0)*nvl(A.BCD,0))*100),2) AS rej_PERC, round((sum(nvl(a.iqtyin,0))/sum(nvl(a.un_melt,0)*nvl(A.BCD,0))*100),2) AS prod_perc, round((sum(nvl(a.mlt_loss,0))/sum(nvl(a.noups,0)*nvl(A.BCD,0))*1000000),2) AS ppm,SUM(nvl(A.IRATE,0)*nvl(A.UN_MELT,0)) AS PLAN_VALUE ,SUM (nvl(A.IRATE,0)*nvl(A.MLT_LOSS,0)) AS COPQ,SUM(nvl(A.IRATE,0)*nvl(A.IQTYIN,0)) AS PROD_VALUE ,TRIM(b.cpartno) AS CPARTNO from prod_sheet a,item b where trim(a.icode)=trim(b.icode)  and a.branchcd='" + frm_mbr + "' AND a.type='" + vartype + "' AND  vchdate " + xprdRange + "  group by A.VCHDATE, trim(a.icode),TRIM(a.var_code),TRIM(a.ename),TRIM(b.cpartno) ORDER BY VAR_CODE,ename ) group by var_code,ename,VCHDATE order by ename,VCHDATE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Mach_Prod_Smry", "std_Mach_Prod_Smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39247":
                header_n = "Details of Items Rejected";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,  A.VCHNUM , A.VCHDATE , to_char(a.vchdate,'dd') as vdd , TRIM(A.ICODE) AS CODE , A.MLT_LOSS AS REJN_QTY, B.INAME ,TRIM(B.CPARTNO) AS Part_Code,(A.MLT_LOSS*A.IRATE) AS REJN_VALUE FROM PROD_SHEET A ,ITEM B WHERE TRIM (A.ICODE) = TRIM (B.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND a.TYPE='"+vartype+"' AND A.MLT_LOSS!='0' AND A.VCHDATE " + xprdRange + " ORDER BY A.VCHDATE, A.VCHNUM,B.INAME ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, A.VCHNUM , A.VCHDATE , to_char(a.vchdate,'dd') as vdd , TRIM(A.ICODE) AS CODE , nvl(A.MLT_LOSS,0) AS REJN_QTY, B.INAME ,TRIM(B.CPARTNO) AS Part_Code,(nvl(A.MLT_LOSS,0)*nvl(A.IRATE,0)) AS REJN_VALUE FROM PROD_SHEET A ,ITEM B WHERE TRIM (A.ICODE) = TRIM (B.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + vartype + "' AND A.MLT_LOSS!='0' AND A.VCHDATE " + xprdRange + " ORDER BY A.VCHDATE, A.VCHNUM,B.INAME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Items_Rejected", "std_Items_Rejected", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F39248"://MONTH WISE
            case "F39249": //,MONTH,MACHINE WISE
            case "F39250": //,MONTH,MACHINE WISE,item wise
                #region
                // SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,A.ENAME AS MACHINE,TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDD,MAX(I.CPARTNO) AS CPARTNO,ROUND((((SUM(A.IQTYIN+A.MLT_LOSS))-(SUM(A.IQTYIN)))/SUM(A.IQTYIN+A.MLT_LOSS))*1000000,0) AS PPM,SUM(A.IQTYIN+A.MLT_LOSS) AS TOTPROD,round(SUM(A.IQTYIN+A.MLT_LOSS),2) AS TOTPROD,round(SUM(A.A1),2)+round(SUM(A.A2),2)+round(SUM(A.A3),2)+round(SUM(A.A4),2)+round(SUM(A.A5),2)+round(SUM(A.A6),2)+round(SUM(A.A7),2)+round(SUM(A.A8),2)+round(SUM(A.A9),2)+round(SUM(A.A10),2)+round(SUM(A.A11),2)+round(SUM(A.A12),2)+round(SUM(A.A13),2)+round(SUM(A.A14),2)+round(SUM(A.A15),2)+round(SUM(A.A16),2)+round(SUM(A.A17),2)+round(SUM(A.A18),2)+round(SUM(A.A19),2)+round(SUM(A.A20),2) AS TOTREJ,round(SUM(A.A1),2) AS A1,round(SUM(A.A2),2) AS A2,round(SUM(A.A3),2) AS A3,round(SUM(A.A4),2) AS A4,round(SUM(A.A5),2) AS A5,round(SUM(A.A6),2) AS A6,round(SUM(A.A7),2) AS A7,round(SUM(A.A8),2) AS A8,round(SUM(A.A9),2) AS A9,round(SUM(A.A10),2) AS A10,round(SUM(A.A11),2) AS A11/*,SUM(A.A12) AS A12,SUM(A.A13) AS A13,SUM(A.A14) AS A14,SUM(A.A15) AS A15*/,round((SUM(A.A12)+SUM(A.A13)+SUM(A.A14)+SUM(A.A15)+SUM(A.A16)+SUM(A.A17)+SUM(A.A18)+SUM(A.A19)+SUM(A.A20)),2) AS OTH FROM PROD_SHEET A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='61' AND A.VCHDATE " + xprdRange + " AND substr(a.ICODE,1,1) in ('9','7') GROUP BY TO_CHAR(A.VCHDATE,'Month'),TO_CHAR(A.VCHDATE,'YYYYMM'),A.ENAME HAVING SUM(TO_NUMBER(A.TEMPR)*A.TOTAL*A.BCD)>0 AND SUM(A.IQTYIN+A.MLT_LOSS)>0 ORDER BY MACHINE";
                if (iconID == "F39248")
                {
                    header_n = "DOWN TIME (Month Wise)";
                    //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,ename as machine, TO_CHAR(VCHDATE,'Month') AS MONTH,TO_CHAR(VCHDATE,'YYYYMM') AS VDd,round(SUM(NUM1),2)+round(SUM(NUM2),2) +round(SUM(NUM3),2)+round(SUM(NUM4),2)+round(SUM(NUM5),2)+round(SUM(NUM6),2) +round(SUM(NUM7),2)+round(SUM(NUM8),2)+round(SUM(NUM9),2)+round(SUM(NUM10),2)+round(SUM(NUM11),2)+round(SUM(NUM12),2) AS TOTAL,round(SUM(NUM11),2)+round(SUM(NUM12),2) as oth,round(SUM(NUM1),2) AS  A1 ,round(SUM(NUM2),2) AS  A2,round(SUM(NUM3),2) AS  A3,round(SUM(NUM4),2) AS  A4 ,round(SUM(NUM5),2) AS  A5,round(SUM(NUM6),2) AS  A6,round(SUM(NUM7),2) AS  A7,round(SUM(NUM8),2) AS  A8,round(SUM(NUM9),2) AS  A9,round(SUM(NUM10),2) AS  A10,round(SUM(NUM11),2) AS  A11,round(SUM(NUM12),2) AS  A12   from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type='"+vartype+"' and VCHDATE " + xprdRange + " GROUP BY ename,TO_CHAR(VCHDATE,'Month'),TO_CHAR(VCHDATE,'YYYYMM') order by vdd";
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,ename as machine, TO_CHAR(VCHDATE,'Month') AS MONTH,TO_CHAR(VCHDATE,'YYYYMM') AS VDd,round(SUM(nvl(NUM1,0)),2)+round(SUM(nvl(NUM2,0)),2) +round(SUM(nvl(NUM3,0)),2)+round(SUM(nvl(NUM4,0)),2)+round(SUM(nvl(NUM5,0)),2)+round(SUM(nvl(NUM6,0)),2) +round(SUM(nvl(NUM7,0)),2)+round(SUM(nvl(NUM8,0)),2)+round(SUM(nvl(NUM9,0)),2)+round(SUM(nvl(NUM10,0)),2)+round(SUM(nvl(NUM11,0)),2)+round(SUM(nvl(NUM12,0)),2) AS TOTAL,round(SUM(nvl(NUM11,0)),2)+round(SUM(nvl(NUM12,0)),2) as oth,round(SUM(nvl(NUM1,0)),2) AS  A1 ,round(SUM(nvl(NUM2,0)),2) AS  A2,round(SUM(nvl(NUM3,0)),2) AS  A3,round(SUM(nvl(NUM4,0)),2) AS  A4 ,round(SUM(nvl(NUM5,0)),2) AS A5,round(SUM(nvl(NUM6,0)),2) AS  A6,round(SUM(nvl(NUM7,0)),2) AS  A7,round(SUM(nvl(NUM8,0)),2) AS  A8,round(SUM(nvl(NUM9,0)),2) AS  A9,round(SUM(nvl(NUM10,0)),2) AS  A10,round(SUM(nvl(NUM11,0)),2) AS  A11,round(SUM(nvl(NUM12,0)),2) AS  A12 from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type='" + vartype + "' and VCHDATE " + xprdRange + " GROUP BY ename,TO_CHAR(VCHDATE,'Month'),TO_CHAR(VCHDATE,'YYYYMM') order by vdd";
                }
                if (iconID == "F39249")
                {
                    header_n = "DOWN TIME (Month,M/c Wise)";
                    //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,ename as machine, TO_CHAR(VCHDATE,'Month') AS MONTH,TO_CHAR(VCHDATE,'YYYYMM') AS VDd,round(SUM(NUM1),2)+round(SUM(NUM2),2) +round(SUM(NUM3),2)+round(SUM(NUM4),2)+round(SUM(NUM5),2)+round(SUM(NUM6),2) +round(SUM(NUM7),2)+round(SUM(NUM8),2)+round(SUM(NUM9),2)+round(SUM(NUM10),2)+round(SUM(NUM11),2)+round(SUM(NUM12),2) AS TOTAL,round(SUM(NUM11),2)+round(SUM(NUM12),2) as oth,round(SUM(NUM1),2) AS  A1 ,round(SUM(NUM2),2) AS  A2,round(SUM(NUM3),2) AS  A3,round(SUM(NUM4),2) AS  A4 ,round(SUM(NUM5),2) AS  A5,round(SUM(NUM6),2) AS  A6,round(SUM(NUM7),2) AS  A7,round(SUM(NUM8),2) AS  A8,round(SUM(NUM9),2) AS  A9,round(SUM(NUM10),2) AS  A10,round(SUM(NUM11),2) AS  A11,round(SUM(NUM12),2) AS  A12   from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type='" + vartype + "' and VCHDATE " + xprdRange + " GROUP BY ename,TO_CHAR(VCHDATE,'Month'),TO_CHAR(VCHDATE,'YYYYMM') order by machine,vdd";
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,ename as machine, TO_CHAR(VCHDATE,'Month') AS MONTH,TO_CHAR(VCHDATE,'YYYYMM') AS VDd,round(SUM(nvl(NUM1,0)),2)+round(SUM(nvl(NUM2,0)),2) +round(SUM(nvl(NUM3,0)),2)+round(SUM(nvl(NUM4,0)),2)+round(SUM(nvl(NUM5,0)),2)+round(SUM(nvl(NUM6,0)),2) +round(SUM(nvl(NUM7,0)),2)+round(SUM(nvl(NUM8,0)),2)+round(SUM(nvl(NUM9,0)),2)+round(SUM(nvl(NUM10,0)),2)+round(SUM(nvl(NUM11,0)),2)+round(SUM(nvl(NUM12,0)),2) AS TOTAL,round(SUM(nvl(NUM11,0)),2)+round(SUM(nvl(NUM12,0)),2) as oth,round(SUM(nvl(NUM1,0)),2) AS  A1 ,round(SUM(nvl(NUM2,0)),2) AS  A2,round(SUM(nvl(NUM3,0)),2) AS  A3,round(SUM(nvl(NUM4,0)),2) AS  A4 ,round(SUM(nvl(NUM5,0)),2) AS A5,round(SUM(nvl(NUM6,0)),2) AS  A6,round(SUM(nvl(NUM7,0)),2) AS  A7,round(SUM(nvl(NUM8,0)),2) AS  A8,round(SUM(nvl(NUM9,0)),2) AS  A9,round(SUM(nvl(NUM10,0)),2) AS  A10,round(SUM(nvl(NUM11,0)),2) AS  A11,round(SUM(nvl(NUM12,0)),2) AS  A12 from prod_sheet where BRANCHCD='" + frm_mbr + "' AND type='" + vartype + "' and VCHDATE " + xprdRange + " GROUP BY ename,TO_CHAR(VCHDATE,'Month'),TO_CHAR(VCHDATE,'YYYYMM') order by machine,vdd";
                }
                if (iconID == "F39250")
                {
                    header_n = "DOWN TIME (Month,Item,M/c Wise)";
                    //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,SUM(A.IQTYIN+A.MLT_LOSS) AS TOTPROD,round(SUM(A.A1),2)+round(SUM(A.A2),2)+round(SUM(A.A3),2)+round(SUM(A.A4),2)+round(SUM(A.A5),2)+round(SUM(A.A6),2)+round(SUM(A.A7),2)+round(SUM(A.A8),2)+round(SUM(A.A9),2)+round(SUM(A.A10),2)+round(SUM(A.A11),2)+round(SUM(A.A12),2)+round(SUM(A.A13),2)+round(SUM(A.A14),2)+round(SUM(A.A15),2)+round(SUM(A.A16),2)+round(SUM(A.A17),2)+round(SUM(A.A18),2)+round(SUM(A.A19),2)+round(SUM(A.A20),2) AS TOTREJ,a.ename as machine,trim(a.icode) as icode,b.iname,MAX(b.CPARTNO) AS CPARTNO, TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDd,round(SUM(a.NUM1),2)+round(SUM(a.NUM2),2) +round(SUM(a.NUM3),2)+round(SUM(a.NUM4),2)+round(SUM(a.NUM5),2)+round(SUM(a.NUM6),2) +round(SUM(a.NUM7),2)+round(SUM(a.NUM8),2)+round(SUM(a.NUM9),2)+round(SUM(a.NUM10),2)+round(SUM(a.NUM11),2)+round(SUM(a.NUM12),2) AS TOTAL,round(SUM(a.NUM11),2)+round(SUM(a.NUM12),2) as oth,round(SUM(a.NUM1),2) AS  A1 ,round(SUM(a.NUM2),2) AS  A2,round(SUM(a.NUM3),2) AS  A3,round(SUM(a.NUM4),2) AS  A4 ,round(SUM(a.NUM5),2) AS  A5,round(SUM(a.NUM6),2) AS  A6,round(SUM(a.NUM7),2) AS  A7,round(SUM(a.NUM8),2) AS  A8,round(SUM(a.NUM9),2) AS  A9,round(SUM(a.NUM10),2) AS  A10,round(SUM(a.NUM11),2) AS  A11,round(SUM(a.NUM12),2) AS  A12   from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='" + vartype + "' and a.VCHDATE " + xprdRange + " GROUP BY a.ename,TO_CHAR(a.VCHDATE,'Month'),TO_CHAR(a.VCHDATE,'YYYYMM'),trim(a.icode),b.iname order by machine,vdd";
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todate,'" + header_n + "' as header,SUM(nvl(A.IQTYIN,0)+nvl(A.MLT_LOSS,0)) AS TOTPROD,round(SUM(nvl(A.A1,0)),2)+round(SUM(nvl(A.A2,0)),2)+round(SUM(nvl(A.A3,0)),2)+round(SUM(nvl(A.A4,0)),2)+round(SUM(nvl(A.A5,0)),2)+round(SUM(nvl(A.A6,0)),2)+round(SUM(nvl(A.A7,0)),2)+round(SUM(nvl(A.A8,0)),2)+round(SUM(nvl(A.A9,0)),2)+round(SUM(nvl(A.A10,0)),2)+round(SUM(nvl(A.A11,0)),2)+round(SUM(nvl(A.A12,0)),2)+round(SUM(nvl(A.A13,0)),2)+round(SUM(nvl(A.A14,0)),2)+round(SUM(nvl(A.A15,0)),2)+round(SUM(nvl(A.A16,0)),2)+round(SUM(nvl(A.A17,0)),2)+round(SUM(nvl(A.A18,0)),2)+round(SUM(nvl(A.A19,0)),2)+round(SUM(nvl(A.A20,0)),2) AS TOTREJ,a.ename as machine,trim(a.icode) as icode,b.iname,MAX(b.CPARTNO) AS CPARTNO, TO_CHAR(a.VCHDATE,'Month') AS MONTH,TO_CHAR(a.VCHDATE,'YYYYMM') AS VDd,round(SUM(nvl(a.NUM1,0)),2)+round(SUM(nvl(a.NUM2,0)),2) +round(SUM(nvl(a.NUM3,0)),2)+round(SUM(nvl(a.NUM4,0)),2)+round(SUM(nvl(a.NUM5,0)),2)+round(SUM(nvl(a.NUM6,0)),2) +round(SUM(nvl(a.NUM7,0)),2)+round(SUM(nvl(a.NUM8,0)),2)+round(SUM(nvl(a.NUM9,0)),2)+round(SUM(nvl(a.NUM10,0)),2)+round(SUM(nvl(a.NUM11,0)),2)+round(SUM(nvl(a.NUM12,0)),2) AS TOTAL,round(SUM(nvl(a.NUM11,0)),2)+round(SUM(nvl(a.NUM12,0)),2) as oth,round(SUM(nvl(a.NUM1,0)),2) AS  A1 ,round(SUM(nvl(a.NUM2,0)),2) AS  A2,round(SUM(nvl(a.NUM3,0)),2) AS  A3,round(SUM(nvl(a.NUM4,0)),2) AS  A4 ,round(SUM(nvl(a.NUM5,0)),2) AS  A5,round(SUM(nvl(a.NUM6,0)),2) AS  A6,round(SUM(nvl(a.NUM7,0)),2) AS  A7,round(SUM(nvl(a.NUM8,0)),2) AS  A8,round(SUM(nvl(a.NUM9,0)),2) AS  A9,round(SUM(nvl(a.NUM10,0)),2) AS  A10,round(SUM(nvl(a.NUM11,0)),2) AS  A11,round(SUM(nvl(a.NUM12,0)),2) AS  A12  from prod_sheet a,item b where trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='" + vartype + "' and a.VCHDATE " + xprdRange + " GROUP BY a.ename,TO_CHAR(a.VCHDATE,'Month'),TO_CHAR(a.VCHDATE,'YYYYMM'),trim(a.icode),b.iname order by machine,vdd";

                }
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

                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(type1) as code,name FROM TYPEWIP WHERE ID='DTC61' and rownum<13 order by type1");
                    int k = 11;
                    for (int l = 0; l < dt.Rows.Count; l++)
                    {
                        try
                        {
                            for (int i = 0; i < k; i++)
                            {
                                dt.Rows[l]["HH" + i] = dt1.Rows[i]["name"].ToString();
                            }
                        }
                        catch { }
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    if (iconID == "F39248")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "DownTime_MachineWise", "DownTime_MachineWise", dsRep, "");///this is month wise rpt
                    }
                    if (iconID == "F39249")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "DownTime_MTH_MachineWise", "DownTime_MTH_MachineWise", dsRep, "");///this is month wise rpt
                    }
                    if (iconID == "F39250")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "downTime_mch_mth_itm", "downTime_mch_mth_itm", dsRep, "");///this is month wise rpt
                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F39251":  //imported accevure III
                #region   //Good Imported Annexure -III
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd != "")
                {
                    if (part_cd == "YES" || part_cd == "")
                    {
                        mq0 = ""; mq1 = "";
                        mq0 = "SELECT A.T_GRNO,A.T_GRDT FROM IVCHCTRL A,IVOUCHER B WHERE TRIM(B.TC_NO)='" + party_cd + "' AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')";
                        cond = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRNO");
                        cond1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRDT");
                        // hfOpen.Value = fgen.seek_iname(frm_qstr,frm_cocd, mq0, "T_GRNO"); //BILL OF ENTRY NO                              
                        //hfParty.Value = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRDT"); //BILL OF ENTRY DATE                              

                        //SQuery = "SELECT  '" + fromdt + "' AS FRMDATE, '" + todt + "' AS TODATE,'" + hfOpen.Value + "' AS BILLNO,'" + hfParty.Value + "' AS BILLDATE, B.INAME,  A.TC_NO,A.ICODE,SUM(A.QTY) AS QTY,SUM(A.ISSUE) AS ISSUE FROM (SELECT TRIM( ICODE) AS ICODE,TC_NO,SUM(IQTYIN) AS QTY ,0 AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE='07'  AND VCHDATE " + xprdRange + "  GROUP BY ICODE,TC_NO UNION ALL SELECT TRIM( ICODE) AS ICODE,TC_NO,0 AS QTY ,SUM(IQTYOUT) AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE LIKE '3%'  AND VCHDATE " + xprdRange + " GROUP BY ICODE,TC_NO) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TC_NO='" + hfHead.Value + "'   GROUP BY A.ICODE,A.TC_NO,B.INAME";  //after change
                        SQuery = "SELECT  '" + fromdt + "' AS FRMDATE, '" + todt + "' AS TODATE,'" + cond + "' AS BILLNO,'" + cond1 + "' AS BILLDATE, B.INAME,  A.TC_NO,A.ICODE,SUM(A.QTY) AS QTY,SUM(A.ISSUE) AS ISSUE FROM (SELECT TRIM( ICODE) AS ICODE,TC_NO,SUM(IQTYIN) AS QTY ,0 AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE='07'  AND VCHDATE " + xprdRange + "  GROUP BY ICODE,TC_NO UNION ALL SELECT TRIM( ICODE) AS ICODE,TC_NO,0 AS QTY ,SUM(IQTYOUT) AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE LIKE '3%'  AND VCHDATE " + xprdRange + " GROUP BY ICODE,TC_NO) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TC_NO='" + party_cd + "'   GROUP BY A.ICODE,A.TC_NO,B.INAME";  //after change
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            dt.TableName = "Prepcur";
                            dsRep.Tables.Add(dt);
                            Print_Report_BYDS(frm_cocd, frm_mbr, "Bill_Of_Entry", "Bill_Of_Entry", dsRep, header_n);
                        }
                        else
                        {
                            data_found = "N";
                        }
                    }
                    else if (part_cd == "NO")
                    {
                        mq0 = ""; mq1 = "";
                        mq0 = "SELECT A.T_GRNO,A.T_GRDT FROM IVCHCTRL A,IVOUCHER B WHERE TRIM(B.TC_NO)='" + party_cd + "' AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')";
                        //hfOpen.Value = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRNO"); //BILL OF ENTRY NO                              
                        //hfParty.Value = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRDT"); //BILL OF ENTRY DATE  
                        cond = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRNO"); //BILL OF ENTRY NO
                        cond1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "T_GRDT"); //BILL OF ENTRY DATE

                        //SQuery = "SELECT  '" + fromdt + "' AS FRMDATE, '" + todt + "' AS TODATE,'" + hfOpen.Value + "' AS BILLNO,'" + hfParty.Value + "' AS BILLDATE, B.INAME,  A.TC_NO,A.ICODE,SUM(A.QTY) AS QTY,SUM(A.ISSUE) AS ISSUE FROM (SELECT TRIM( ICODE) AS ICODE,TC_NO,SUM(IQTYIN) AS QTY ,0 AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE='07'  AND VCHDATE " + xprdRange + "  GROUP BY ICODE,TC_NO UNION ALL SELECT TRIM( ICODE) AS ICODE,TC_NO,0 AS QTY ,SUM(IQTYOUT) AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE LIKE '3%'  AND VCHDATE " + xprdRange + " GROUP BY ICODE,TC_NO) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TC_NO='" + hfHead.Value + "'   GROUP BY A.ICODE,A.TC_NO,B.INAME HAVING SUM(QTY-ISSUE)>0";  //after change
                        SQuery = "SELECT  '" + fromdt + "' AS FRMDATE, '" + todt + "' AS TODATE,'" + cond + "' AS BILLNO,'" + cond1 + "' AS BILLDATE, B.INAME,  A.TC_NO,A.ICODE,SUM(A.QTY) AS QTY,SUM(A.ISSUE) AS ISSUE FROM (SELECT TRIM( ICODE) AS ICODE,TC_NO,SUM(IQTYIN) AS QTY ,0 AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE='07'  AND VCHDATE " + xprdRange + "  GROUP BY ICODE,TC_NO UNION ALL SELECT TRIM( ICODE) AS ICODE,TC_NO,0 AS QTY ,SUM(IQTYOUT) AS ISSUE FROM IVOUCHER WHERE BRANCHCD = '" + frm_mbr + "' AND TYPE LIKE '3%'  AND VCHDATE " + xprdRange + " GROUP BY ICODE,TC_NO) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TC_NO='" + party_cd + "'   GROUP BY A.ICODE,A.TC_NO,B.INAME HAVING SUM(QTY-ISSUE)>0";  //after change
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            dt.TableName = "Prepcur";
                            dsRep.Tables.Add(dt);
                            Print_Report_BYDS(frm_cocd, frm_mbr, "Bill_Of_Entry", "Bill_Of_Entry", dsRep, header_n);
                        }
                        else
                        {
                            data_found = "N";
                        }
                    }
                }
                #endregion
                break;

            case "F39192": //DONE
                if (frm_cocd == "SPIR" || frm_cocd == "STLC")
                {
                    WB_TABNAME = "prod_sheetK";
                    col1 = "86";
                }
                else
                {
                    WB_TABNAME = "prod_sheet";
                    col1 = "61";
                }
                header_n = "Details of Items Rejected";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,  TRIM(A.VCHNUM) AS VCHNUM ,A.VCHDATE , to_char(a.vchdate,'dd') as vdd , TRIM(A.ICODE) AS CODE , A.MLT_LOSS AS REJN_QTY, B.INAME ,TRIM(B.CPARTNO) AS Part_Code,(A.MLT_LOSS*A.IRATE) AS REJN_VALUE FROM " + WB_TABNAME + " A ,ITEM B WHERE TRIM (A.ICODE) = TRIM (B.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + col1 + "' AND A.MLT_LOSS!='0' AND A.VCHDATE " + xprdRange + " ORDER BY A.VCHDATE, A.VCHNUM,B.INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Items_Rejected", "std_Items_Rejected", dsRep, header_n);
                }
                break;

            case "F39190":
                if (frm_cocd == "SPIR" || frm_cocd == "STLC")
                {
                    WB_TABNAME = "prod_sheetK";
                    col1 = "86";
                }
                else
                {
                    WB_TABNAME = "prod_sheet";
                    col1 = "90";
                }
                header_n = "Details of Items Produced ";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,  A.VCHNUM , A.VCHDATE , to_char(a.vchdate,'dd') as vdd , trim(A.ICODE) AS CODE , A.IQTYIN AS PROD_QTY , B.INAME , trim(B.CPARTNO) as cpartno, (A.IQTYIN*A.IRATE) AS PROD_VALUE  FROM " + WB_TABNAME + " A , ITEM B  WHERE TRIM(A.ICODE) =TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND TYPE = '" + col1 + "' AND A.IQTYIN!='0'  AND A.VCHDATE " + xprdRange + "   ORDER BY A.VCHDATE,A.VCHNUM,B.INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Items_Produced", "std_Items_Produced", dsRep, header_n);
                }
                break;

            case "F39255":
                header_n = "Reason Wise Rejections";
                #region
                mq0 = "select trim(a.col4) as col4,sum(a.qty1) as qty,trim(t.type1) as code from multivch a,typegrp t where trim(a.col4)=trim(t.name) and t.id='R1' and a.branchcd='" + frm_mbr + "' and a.TYPE='RR' AND to_date(a.btchdt,'dd/mm/yyyy') " + xprdRange + " group by trim(col4),trim(t.type1) order by qty desc,col4";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = " i.bfactor like '%'";
                }
                else
                {
                    party_cd = " i.bfactor='" + party_cd + "'";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = " a.icode like '%'";
                }
                else
                {
                    part_cd = " a.icode in (" + part_cd + ")";
                }
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,trim(a.icode) as icode,trim(i.iname) as item,trim(i.bfactor) as family_code,trim(t.name) as family,trim(a.col4) as reasons,sum(a.qty1) as qty,trim(t1.type1) as code FROM MULTIVCH a,item i,typegrp t,typegrp t1 WHERE trim(a.icode)=trim(i.icode) and trim(i.bfactor)=trim(t.type1) and trim(a.col4)=trim(t1.name) and t1.id='R1' and t.id='^8' and a.branchcd='" + frm_mbr + "' and a.TYPE='RR' AND to_date(a.btchdt,'dd/mm/yyyy') " + xprdRange + " and " + party_cd + " and " + part_cd + " group by trim(a.icode),trim(i.iname),trim(i.bfactor),trim(t.name),trim(a.col4),trim(t1.type1) ORDER BY family_code,icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                oporow = null;
                int count = 1;
                mdt = new DataTable();
                mdt = dt.Clone();
                mdt.Columns.Add("H1", typeof(string));
                mdt.Columns.Add("H2", typeof(string));
                mdt.Columns.Add("H3", typeof(string));
                mdt.Columns.Add("H4", typeof(string));
                mdt.Columns.Add("H5", typeof(string));
                mdt.Columns.Add("H6", typeof(string));
                mdt.Columns.Add("H7", typeof(string));
                mdt.Columns.Add("H8", typeof(string));
                mdt.Columns.Add("H9", typeof(string));
                mdt.Columns.Add("H10", typeof(string));
                mdt.Columns.Add("H11", typeof(string));
                mdt.Columns.Add("H12", typeof(string));
                mdt.Columns.Add("H13", typeof(string));
                mdt.Columns.Add("H14", typeof(string));
                mdt.Columns.Add("H15", typeof(string));

                mdt.Columns.Add("Z1", typeof(double));
                mdt.Columns.Add("Z2", typeof(double));
                mdt.Columns.Add("Z3", typeof(double));
                mdt.Columns.Add("Z4", typeof(double));
                mdt.Columns.Add("Z5", typeof(double));
                mdt.Columns.Add("Z6", typeof(double));
                mdt.Columns.Add("Z7", typeof(double));
                mdt.Columns.Add("Z8", typeof(double));
                mdt.Columns.Add("Z9", typeof(double));
                mdt.Columns.Add("Z10", typeof(double));
                mdt.Columns.Add("Z11", typeof(double));
                mdt.Columns.Add("Z12", typeof(double));
                mdt.Columns.Add("Z13", typeof(double));
                mdt.Columns.Add("Z14", typeof(double));
                mdt.Columns.Add("Z15", typeof(double));
                int index = 25;
                foreach (DataRow dr in dt1.Rows)
                {
                    if (count <= 14)
                    {
                        mdt.Columns[index].ColumnName = "Z" + dr["code"].ToString().Trim();
                    }
                    count++;
                    index++;
                }

                double totqty = 0;
                if (dt.Rows.Count > 0)
                {
                    dv = new DataView(dt);
                    dticode = new DataTable();
                    dticode = dv.ToTable(true, "family_code", "icode");
                    foreach (DataRow dr2 in dticode.Rows)
                    {
                        DataView view1 = new DataView(dt, "family_code='" + dr2["family_code"].ToString().Trim() + "' and icode='" + dr2["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        DataTable dticode2 = new DataTable();
                        dticode2 = view1.ToTable();
                        totqty = 0;
                        for (int i = 0; i < dticode2.Rows.Count; i++)
                        {
                            count = 1;
                            if (i == 0)
                            {
                                oporow = mdt.NewRow();
                                oporow["fromdt"] = dticode2.Rows[i]["fromdt"].ToString().Trim();
                                oporow["todt"] = dticode2.Rows[i]["todt"].ToString().Trim();
                                oporow["header"] = dticode2.Rows[i]["header"].ToString().Trim();
                                oporow["icode"] = dticode2.Rows[i]["icode"].ToString().Trim();
                                oporow["item"] = dticode2.Rows[i]["item"].ToString().Trim();
                                oporow["family_code"] = dticode2.Rows[i]["family_code"].ToString().Trim();
                                oporow["family"] = dticode2.Rows[i]["family"].ToString().Trim();

                                foreach (DataRow dr in dt1.Rows)
                                {
                                    if (count <= 14)
                                    {
                                        oporow["H" + count] = dr["col4"].ToString().Trim();
                                    }
                                    count++;
                                }
                            }
                            totqty += fgen.make_double(dticode2.Rows[i]["qty"].ToString().Trim());
                            oporow["qty"] = totqty;
                            try
                            {
                                oporow["Z" + dticode2.Rows[i]["code"].ToString().Trim()] = dticode2.Rows[i]["qty"].ToString().Trim();
                            }
                            catch { }
                        }
                        mdt.Rows.Add(oporow);
                    }
                }

                if (mdt.Rows.Count > 0)
                {
                    mdt.Columns[25].ColumnName = "Z1";
                    mdt.Columns[26].ColumnName = "Z2";
                    mdt.Columns[27].ColumnName = "Z3";
                    mdt.Columns[28].ColumnName = "Z4";
                    mdt.Columns[29].ColumnName = "Z5";
                    mdt.Columns[30].ColumnName = "Z6";
                    mdt.Columns[31].ColumnName = "Z7";
                    mdt.Columns[32].ColumnName = "Z8";
                    mdt.Columns[33].ColumnName = "Z9";
                    mdt.Columns[34].ColumnName = "Z10";
                    mdt.Columns[35].ColumnName = "Z11";
                    mdt.Columns[36].ColumnName = "Z12";
                    mdt.Columns[37].ColumnName = "Z13";
                    mdt.Columns[38].ColumnName = "Z14";
                    mdt.Columns[39].ColumnName = "Z15";
                    pdfView = "N";
                    mdt.TableName = "Prepcur";
                    dsRep.Tables.Add(mdt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "svpl_rej_resn", "svpl_rej_resn", dsRep, "");
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