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

public partial class maint_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdRange1, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, pdfView = "", data_found = "";
    string mq2 = "", mq3 = "", DateRange = "", header_n = "";
    double db, db1, db2, db3, db4, db5, db6;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet(); DataSet ds;
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

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
        string mq10 = "", mq1 = "", mq0 = "";
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "";
        data_found = "Y";

        switch (iconID)
        {
            case "F75145":
            case "F75146":
            case "F75147":
                string FIELD_VAL = "";
                FIELD_VAL = "SUBSTR(MAINTDT,1,2)";
                if (iconID == "F75145")
                {
                    header_n = "Maintenance Plan";
                    SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,MCHNAME ,decode(" + FIELD_VAL + ",'01','P','') as DAY1,decode(" + FIELD_VAL + ",'02','P','') as DAY2,decode(" + FIELD_VAL + ",'03','P','') as DAY3,decode(" + FIELD_VAL + ",'04','P','') as DAY4,decode(" + FIELD_VAL + ",'05','P','') as DAY5,decode(" + FIELD_VAL + ",'06','P','') as DAY6,decode(" + FIELD_VAL + ",'07','P','') as DAY7,decode(" + FIELD_VAL + ",'08','P','') as DAY8,decode(" + FIELD_VAL + ",'09','P','') as DAY9,decode(" + FIELD_VAL + ",'10','P','') as DAY10,decode(" + FIELD_VAL + ",'11','P','') as DAY11,decode(" + FIELD_VAL + ",'12','P','') as DAY12,decode(" + FIELD_VAL + ",'13','P','') as DAY13,decode(" + FIELD_VAL + ",'14','P','') as DAY14,decode(" + FIELD_VAL + ",'15','P','') as DAY15,decode(" + FIELD_VAL + ",'16','P','') as DAY16,decode(" + FIELD_VAL + ",'17','P','') as DAY17,decode(" + FIELD_VAL + ",'18','P','') as DAY18,decode(" + FIELD_VAL + ",'19','P','') as DAY19,decode(" + FIELD_VAL + ",'20','P','') as DAY20,decode(" + FIELD_VAL + ",'21','P','') as DAY21,decode(" + FIELD_VAL + ",'22','P','') as DAY22,decode(" + FIELD_VAL + ",'23','P','') as DAY23,decode(" + FIELD_VAL + ",'24','P','') as DAY24,decode(" + FIELD_VAL + ",'25','P','') as DAY25,decode(" + FIELD_VAL + ",'26','P','') as DAY26,decode(" + FIELD_VAL + ",'27','P','') as DAY27,decode(" + FIELD_VAL + ",'28','P','') as DAY28,decode(" + FIELD_VAL + ",'29','P','') as DAY29,decode(" + FIELD_VAL + ",'30','P','') as DAY30,decode(" + FIELD_VAL + ",'31','P','') as DAY31 FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='66' and vchdate " + xprdRange + " ORDER BY MCHNAME";
                }
                else if (iconID == "F75146")
                {
                    header_n = "Maintenance Done";
                    SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,MCHNAME ,decode(" + FIELD_VAL + ",'01','D','') as DAY1,decode(" + FIELD_VAL + ",'02','D','') as DAY2,decode(" + FIELD_VAL + ",'03','D','') as DAY3,decode(" + FIELD_VAL + ",'04','D','') as DAY4,decode(" + FIELD_VAL + ",'05','D','') as DAY5,decode(" + FIELD_VAL + ",'06','D','') as DAY6,decode(" + FIELD_VAL + ",'07','D','') as DAY7,decode(" + FIELD_VAL + ",'08','D','') as DAY8,decode(" + FIELD_VAL + ",'09','D','') as DAY9,decode(" + FIELD_VAL + ",'10','D','') as DAY10,decode(" + FIELD_VAL + ",'11','D','') as DAY11,decode(" + FIELD_VAL + ",'12','D','') as DAY12,decode(" + FIELD_VAL + ",'13','D','') as DAY13,decode(" + FIELD_VAL + ",'14','D','') as DAY14,decode(" + FIELD_VAL + ",'15','D','') as DAY15,decode(" + FIELD_VAL + ",'16','D','') as DAY16,decode(" + FIELD_VAL + ",'17','D','') as DAY17,decode(" + FIELD_VAL + ",'18','D','') as DAY18,decode(" + FIELD_VAL + ",'19','D','') as DAY19,decode(" + FIELD_VAL + ",'20','D','') as DAY20,decode(" + FIELD_VAL + ",'21','D','') as DAY21,decode(" + FIELD_VAL + ",'22','D','') as DAY22,decode(" + FIELD_VAL + ",'23','D','') as DAY23,decode(" + FIELD_VAL + ",'24','D','') as DAY24,decode(" + FIELD_VAL + ",'25','D','') as DAY25,decode(" + FIELD_VAL + ",'26','D','') as DAY26,decode(" + FIELD_VAL + ",'27','D','') as DAY27,decode(" + FIELD_VAL + ",'28','D','') as DAY28,decode(" + FIELD_VAL + ",'29','D','') as DAY29,decode(" + FIELD_VAL + ",'30','D','') as DAY30,decode(" + FIELD_VAL + ",'31','D','') as DAY31 FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='20' and vchdate " + xprdRange + " ORDER BY MCHNAME";
                }
                else
                {
                    header_n = "Maintenance Plan/Done";
                    SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, HEADER,MCHNAME,DAY1,DAY2,DAY3,DAY4,DAY5,DAY6,DAY7,DAY8,DAY9,DAY10,DAY11,DAY12,DAY13,DAY14,DAY15,DAY16,DAY17,DAY18,DAY19,DAY20,DAY21,DAY22,DAY23,DAY24,DAY25,DAY26,DAY27,DAY28,DAY29,DAY30,DAY31 FROM (SELECT 'MAINT_DONE' AS HEADER,MCHNAME ,decode(" + FIELD_VAL + ",'01','X','') as DAY1,decode(" + FIELD_VAL + ",'02','X','') as DAY2,decode(" + FIELD_VAL + ",'03','X','') as DAY3,decode(" + FIELD_VAL + ",'04','X','') as DAY4,decode(" + FIELD_VAL + ",'05','X','') as DAY5,decode(" + FIELD_VAL + ",'06','X','') as DAY6,decode(" + FIELD_VAL + ",'07','X','') as DAY7,decode(" + FIELD_VAL + ",'08','X','') as DAY8,decode(" + FIELD_VAL + ",'09','X','') as DAY9,decode(" + FIELD_VAL + ",'10','X','') as DAY10,decode(" + FIELD_VAL + ",'11','X','') as DAY11,decode(" + FIELD_VAL + ",'12','X','') as DAY12,decode(" + FIELD_VAL + ",'13','X','') as DAY13,decode(" + FIELD_VAL + ",'14','X','') as DAY14,decode(" + FIELD_VAL + ",'15','X','') as DAY15,decode(" + FIELD_VAL + ",'16','X','') as DAY16,decode(" + FIELD_VAL + ",'17','X','') as DAY17,decode(" + FIELD_VAL + ",'18','X','') as DAY18,decode(" + FIELD_VAL + ",'19','X','') as DAY19,decode(" + FIELD_VAL + ",'20','X','') as DAY20,decode(" + FIELD_VAL + ",'21','X','') as DAY21,decode(" + FIELD_VAL + ",'22','X','') as DAY22,decode(" + FIELD_VAL + ",'23','X','') as DAY23,decode(" + FIELD_VAL + ",'24','X','') as DAY24,decode(" + FIELD_VAL + ",'25','X','') as DAY25,decode(" + FIELD_VAL + ",'26','X','') as DAY26,decode(" + FIELD_VAL + ",'27','X','') as DAY27,decode(" + FIELD_VAL + ",'28','X','') as DAY28,decode(" + FIELD_VAL + ",'29','X','') as DAY29,decode(" + FIELD_VAL + ",'30','X','') as DAY30,decode(" + FIELD_VAL + ",'31','X','') as DAY31 FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='20' AND VCHDATE " + xprdRange + " UNION ALL SELECT 'MAINT_PLAN' AS HEADER,MCHNAME ,decode(" + FIELD_VAL + ",'01','P','') as DAY1,decode(" + FIELD_VAL + ",'02','P','') as DAY2,decode(" + FIELD_VAL + ",'03','P','') as DAY3,decode(" + FIELD_VAL + ",'04','P','') as DAY4,decode(" + FIELD_VAL + ",'05','P','') as DAY5,decode(" + FIELD_VAL + ",'06','P','') as DAY6,decode(" + FIELD_VAL + ",'07','P','') as DAY7,decode(" + FIELD_VAL + ",'08','P','') as DAY8,decode(" + FIELD_VAL + ",'09','P','') as DAY9,decode(" + FIELD_VAL + ",'10','P','') as DAY10,decode(" + FIELD_VAL + ",'11','P','') as DAY11,decode(" + FIELD_VAL + ",'12','P','') as DAY12,decode(" + FIELD_VAL + ",'13','P','') as DAY13,decode(" + FIELD_VAL + ",'14','P','') as DAY14,decode(" + FIELD_VAL + ",'15','P','') as DAY15,decode(" + FIELD_VAL + ",'16','P','') as DAY16,decode(" + FIELD_VAL + ",'17','P','') as DAY17,decode(" + FIELD_VAL + ",'18','P','') as DAY18,decode(" + FIELD_VAL + ",'19','P','') as DAY19,decode(" + FIELD_VAL + ",'20','P','') as DAY20,decode(" + FIELD_VAL + ",'21','P','') as DAY21,decode(" + FIELD_VAL + ",'22','P','') as DAY22,decode(" + FIELD_VAL + ",'23','P','') as DAY23,decode(" + FIELD_VAL + ",'24','P','') as DAY24,decode(" + FIELD_VAL + ",'25','P','') as DAY25,decode(" + FIELD_VAL + ",'26','P','') as DAY26,decode(" + FIELD_VAL + ",'27','P','') as DAY27,decode(" + FIELD_VAL + ",'28','P','') as DAY28,decode(" + FIELD_VAL + ",'29','P','') as DAY29,decode(" + FIELD_VAL + ",'30','P','') as DAY30,decode(" + FIELD_VAL + ",'31','P','') as DAY31 FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='66' AND VCHDATE " + xprdRange + ") ORDER BY MCHNAME";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Maint_Plan", "Maint_Plan", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F75163": // HM Monthly Planning Report
            case "F75164": // PM Monthly Planning Report
            case "F75150": // PM Monthly Planning Report form
            case "F75155": // HM Monthly Planning Report form
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                mq2 = mq1 + " " + frm_myear;
                if (iconID == "F75163" || iconID == "F75155")
                {
                    header_n = "HM Monthly Planning Report";
                    SQuery = "select sum(day1+day2+day3+day4+day5+day6+day7+day8+day9+day10+day11+day12+day13+day14+day15+day16+day17+day18+day19+day20+day21+day22+day23+day24+day25+day26+day27+day28+day29+day30+day31) as tot from (Select col1 as code,decode(to_char(date1,'dd'),'01',1,0) as day1,decode(to_char(date1,'dd'),'02',1,0) as day2,decode(to_char(date1,'dd'),'03',1,0) as day3,decode(to_char(date1,'dd'),'04',1,0) as day4,decode(to_char(date1,'dd'),'05',1,0) as day5,decode(to_char(date1,'dd'),'06',1,0) as day6,decode(to_char(date1,'dd'),'07',1,0) as day7,decode(to_char(date1,'dd'),'08',1,0) as day8,decode(to_char(date1,'dd'),'09',1,0) as day9,decode(to_char(date1,'dd'),'10',1,0) as day10,decode(to_char(date1,'dd'),'11',1,0) as day11,decode(to_char(date1,'dd'),'12',1,0) as day12,decode(to_char(date1,'dd'),'13',1,0) as day13,decode(to_char(date1,'dd'),'14',1,0) as day14,decode(to_char(date1,'dd'),'15',1,0) as day15,decode(to_char(date1,'dd'),'16',1,0) as day16,decode(to_char(date1,'dd'),'17',1,0) as day17,decode(to_char(date1,'dd'),'18',1,0) as day18,decode(to_char(date1,'dd'),'19',1,0) as day19,decode(to_char(date1,'dd'),'20',1,0) as day20,decode(to_char(date1,'dd'),'21',1,0) as day21,decode(to_char(date1,'dd'),'22',1,0) as day22,decode(to_char(date1,'dd'),'23',1,0) as day23,decode(to_char(date1,'dd'),'24',1,0) as day24,decode(to_char(date1,'dd'),'25',1,0) as day25,decode(to_char(date1,'dd'),'26',1,0) as day26,decode(to_char(date1,'dd'),'27',1,0) as day27,decode(to_char(date1,'dd'),'28',1,0) as day28,decode(to_char(date1,'dd'),'29',1,0) as day29,decode(to_char(date1,'dd'),'30',1,0) as day30,decode(to_char(date1,'dd'),'31',1,0) as day31 from wb_maint where branchcd='" + frm_mbr + "' and type='MM03' and obsv2='" + mq0 + "/" + frm_myear + "')";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt1.Rows.Count > 0)
                    {
                        SQuery = "Select '" + mq2 + "' as month_,'" + header_n + "' as header, a.col1 as code,b.name as mould_name ,c.col9 as acref," + dt1.Rows[0]["tot"].ToString().Trim() + " as total_planned,decode(to_char(a.date1,'dd'),'01','1','') as day1,decode(to_char(a.date1,'dd'),'02','1','') as day2,decode(to_char(a.date1,'dd'),'03','1','') as day3,decode(to_char(a.date1,'dd'),'04','1','') as day4,decode(to_char(a.date1,'dd'),'05','1','') as day5,decode(to_char(a.date1,'dd'),'06','1','') as day6,decode(to_char(a.date1,'dd'),'07','1','') as day7,decode(to_char(a.date1,'dd'),'08','1','') as day8,decode(to_char(a.date1,'dd'),'09','1','') as day9,decode(to_char(a.date1,'dd'),'10','1','') as day10,decode(to_char(a.date1,'dd'),'11','1','') as day11,decode(to_char(a.date1,'dd'),'12','1','') as day12,decode(to_char(a.date1,'dd'),'13','1','') as day13,decode(to_char(a.date1,'dd'),'14','1','') as day14,decode(to_char(a.date1,'dd'),'15','1','') as day15,decode(to_char(a.date1,'dd'),'16','1','') as day16,decode(to_char(a.date1,'dd'),'17','1','') as day17,decode(to_char(a.date1,'dd'),'18','1','') as day18,decode(to_char(a.date1,'dd'),'19','1','') as day19,decode(to_char(a.date1,'dd'),'20','1','') as day20,decode(to_char(a.date1,'dd'),'21','1','') as day21,decode(to_char(a.date1,'dd'),'22','1','') as day22,decode(to_char(a.date1,'dd'),'23','1','') as day23,decode(to_char(a.date1,'dd'),'24','1','') as day24,decode(to_char(a.date1,'dd'),'25','1','') as day25,decode(to_char(a.date1,'dd'),'26','1','') as day26,decode(to_char(a.date1,'dd'),'27','1','') as day27,decode(to_char(a.date1,'dd'),'28','1','') as day28,decode(to_char(a.date1,'dd'),'29','1','') as day29,decode(to_char(a.date1,'dd'),'30','1','') as day30,decode(to_char(a.date1,'dd'),'31','1','') as day31 from wb_maint a ,typegrp b,wb_master c where trim(a.branchcd)||trim(a.col1)=trim(b.branchcd)||trim(b.type1) and trim(a.branchcd)||trim(a.col1)=trim(c.branchcd)||trim(c.col1) and c.id='MM01' and a.branchcd='" + frm_mbr + "' and a.type='MM03' and b.id='MM' and nvl(c.col2,'-')!='Y' and a.obsv2='" + mq0 + "/" + frm_myear + "' order by mould_name";
                    }
                }
                else if (iconID == "F75164" || iconID == "F75150")
                {
                    header_n = "PM Monthly Planning Report";
                    SQuery = "select sum(day1+day2+day3+day4+day5+day6+day7+day8+day9+day10+day11+day12+day13+day14+day15+day16+day17+day18+day19+day20+day21+day22+day23+day24+day25+day26+day27+day28+day29+day30+day31) as tot from (Select col1 as code,decode(to_char(date1,'dd'),'01',1,0) as day1,decode(to_char(date1,'dd'),'02',1,0) as day2,decode(to_char(date1,'dd'),'03',1,0) as day3,decode(to_char(date1,'dd'),'04',1,0) as day4,decode(to_char(date1,'dd'),'05',1,0) as day5,decode(to_char(date1,'dd'),'06',1,0) as day6,decode(to_char(date1,'dd'),'07',1,0) as day7,decode(to_char(date1,'dd'),'08',1,0) as day8,decode(to_char(date1,'dd'),'09',1,0) as day9,decode(to_char(date1,'dd'),'10',1,0) as day10,decode(to_char(date1,'dd'),'11',1,0) as day11,decode(to_char(date1,'dd'),'12',1,0) as day12,decode(to_char(date1,'dd'),'13',1,0) as day13,decode(to_char(date1,'dd'),'14',1,0) as day14,decode(to_char(date1,'dd'),'15',1,0) as day15,decode(to_char(date1,'dd'),'16',1,0) as day16,decode(to_char(date1,'dd'),'17',1,0) as day17,decode(to_char(date1,'dd'),'18',1,0) as day18,decode(to_char(date1,'dd'),'19',1,0) as day19,decode(to_char(date1,'dd'),'20',1,0) as day20,decode(to_char(date1,'dd'),'21',1,0) as day21,decode(to_char(date1,'dd'),'22',1,0) as day22,decode(to_char(date1,'dd'),'23',1,0) as day23,decode(to_char(date1,'dd'),'24',1,0) as day24,decode(to_char(date1,'dd'),'25',1,0) as day25,decode(to_char(date1,'dd'),'26',1,0) as day26,decode(to_char(date1,'dd'),'27',1,0) as day27,decode(to_char(date1,'dd'),'28',1,0) as day28,decode(to_char(date1,'dd'),'29',1,0) as day29,decode(to_char(date1,'dd'),'30',1,0) as day30,decode(to_char(date1,'dd'),'31',1,0) as day31 from wb_maint where branchcd='" + frm_mbr + "' and type='MM02' and obsv2='" + mq0 + "/" + frm_myear + "')";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt1.Rows.Count > 0)
                    {
                        SQuery = "Select '" + mq2 + "' as month_,'" + header_n + "' as header, a.col1 as code,b.name as mould_name,c.col9 as acref," + dt1.Rows[0]["tot"].ToString().Trim() + " as total_planned,decode(to_char(a.date1,'dd'),'01','1','') as day1,decode(to_char(a.date1,'dd'),'02','1','') as day2,decode(to_char(a.date1,'dd'),'03','1','') as day3,decode(to_char(a.date1,'dd'),'04','1','') as day4,decode(to_char(a.date1,'dd'),'05','1','') as day5,decode(to_char(a.date1,'dd'),'06','1','') as day6,decode(to_char(a.date1,'dd'),'07','1','') as day7,decode(to_char(a.date1,'dd'),'08','1','') as day8,decode(to_char(a.date1,'dd'),'09','1','') as day9,decode(to_char(a.date1,'dd'),'10','1','') as day10,decode(to_char(a.date1,'dd'),'11','1','') as day11,decode(to_char(a.date1,'dd'),'12','1','') as day12,decode(to_char(a.date1,'dd'),'13','1','') as day13,decode(to_char(a.date1,'dd'),'14','1','') as day14,decode(to_char(a.date1,'dd'),'15','1','') as day15,decode(to_char(a.date1,'dd'),'16','1','') as day16,decode(to_char(a.date1,'dd'),'17','1','') as day17,decode(to_char(a.date1,'dd'),'18','1','') as day18,decode(to_char(a.date1,'dd'),'19','1','') as day19,decode(to_char(a.date1,'dd'),'20','1','') as day20,decode(to_char(a.date1,'dd'),'21','1','') as day21,decode(to_char(a.date1,'dd'),'22','1','') as day22,decode(to_char(a.date1,'dd'),'23','1','') as day23,decode(to_char(a.date1,'dd'),'24','1','') as day24,decode(to_char(a.date1,'dd'),'25','1','') as day25,decode(to_char(a.date1,'dd'),'26','1','') as day26,decode(to_char(a.date1,'dd'),'27','1','') as day27,decode(to_char(a.date1,'dd'),'28','1','') as day28,decode(to_char(a.date1,'dd'),'29','1','') as day29,decode(to_char(a.date1,'dd'),'30','1','') as day30,decode(to_char(a.date1,'dd'),'31','1','') as day31 from wb_maint a ,typegrp b,wb_master c where trim(a.branchcd)||trim(a.col1)=trim(b.branchcd)||trim(b.type1) and trim(a.branchcd)||trim(a.col1)=trim(c.branchcd)||trim(c.col1) and c.id='MM01' and a.branchcd='" + frm_mbr + "' and a.type='MM02' and b.id='MM' and nvl(c.col2,'-')!='Y' and a.obsv2='" + mq0 + "/" + frm_myear + "' order by mould_name";
                    }
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "mould_plan";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mnth_Plan", "Mnth_Plan", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F75167":
                header_n = "HM : Plan Vs Actual"; //YEARLY
                SQuery = "select sum(a.p_apr+a.p_may+a.p_jun+a.p_jul+a.p_aug+a.p_sept+a.p_oct+a.p_nov+a.p_dec+a.p_jan+a.p_feb+a.p_mar) as planning,sum(a.d_apr+a.d_may+a.d_jun+a.d_jul+a.d_aug+a.d_sept+a.d_oct+a.d_nov+a.d_dec+a.d_jan+a.d_feb+a.d_mar) as actual from (select distinct col1 as code,decode(to_char(date1,'mm'),'03',1,0) as p_mar,decode(to_char(date1,'mm'),'04',1,0) as p_apr,decode(to_char(date1,'mm'),'05',1,0) as p_may,decode(to_char(date1,'mm'),'06',1,0) as p_jun,decode(to_char(date1,'mm'),'07',1,0) as p_jul,decode(to_char(date1,'mm'),'08',1,0) as p_aug ,decode(to_char(date1,'mm'),'09',1,0) as p_sept,decode(to_char(date1,'mm'),'10',1,0) as p_oct,decode(to_char(date1,'mm'),'11',1,0) as p_nov,decode(to_char(date1,'mm'),'12',1,0) as p_dec,decode(to_char(date1,'mm'),'01',1,0) as p_jan,decode(to_char(date1,'mm'),'02',1,0) as p_feb,0 as d_mar,0 as d_apr,0 as d_may,0 as d_jun,0 as d_jul,0 as d_aug,0 as d_sept,0 as d_oct,0 as d_nov,0 as d_dec,0 as d_jan,0 as d_feb from wb_maint where branchcd='" + frm_mbr + "' and type='MM03' and date1 " + xprdRange + " union all select distinct  col1 as code,0 as p_mar,0 as p_apr,0 as p_may,0 as p_jun,0 as p_jul,0 as p_aug,0 as p_sept,0 as p_oct,0 as p_nov,0 as p_dec,0 as p_jan,0 as p_feb, decode(to_char(date1,'mm'),'03',1,0) as d_mar,decode(to_char(date1,'mm'),'04',1,0) as d_apr,decode(to_char(date1,'mm'),'05',1,0) as d_may,decode(to_char(date1,'mm'),'06',1,0) as d_jun,decode(to_char(date1,'mm'),'07',1,0) as d_jul,decode(to_char(date1,'mm'),'08',1,0) as d_aug,decode(to_char(date1,'mm'),'09',1,0) as d_sept,decode(to_char(date1,'mm'),'10',1,0) as d_oct,decode(to_char(date1,'mm'),'11',1,0) as d_nov,decode(to_char(date1,'mm'),'12',1,0) as  d_dec,decode(to_char(date1,'mm'),'01',1,0) as d_jan,decode(to_char(date1,'mm'),'02',1,0) as d_feb from  wb_maint where branchcd='" + frm_mbr + "' and type='MM05' and date1 " + xprdRange + " ) a";
                dt1 = new DataTable();
                dt = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + dt1.Rows[0]["planning"].ToString().Trim() + "' as planning,'" + dt1.Rows[0]["actual"].ToString().Trim() + "' as actual,c.name as model, trim(b.name) as mouldname,c.col9 as acref ,trim(a.code) as mouldcode,trim(c.cpartno) as partno,c.col14 as material,c.num15 as pm_frequency_as_per_month,c.num13 as first_hm_count,c.col15 as pm_frequency_as_per_shots,c.num5 as max_limit_for_alarm ,sum(a.p_mar) as p_mar,sum(a.d_mar) as d_mar,sum(a.p_apr) as p_apr,sum(a.d_apr) as d_apr,sum(a.p_may) as p_may,sum(a.d_may) as d_may,sum(a.p_jun) as p_jun,sum(a.d_jun) as d_jun,sum(a.p_jul) as p_jul,sum(a.d_jul) as d_jul,sum(a.p_aug) as p_aug,sum(a.d_aug) as d_aug,sum(a.p_sept) as p_sept,sum(a.d_sept) as d_sept,sum(a.p_oct) as p_oct,sum(a.d_oct) as d_oct,sum(a.p_nov) as p_nov,sum(a.d_nov) as d_nov,sum(a.p_dec) as p_dec,sum(a.d_dec) as d_dec,sum(a.p_jan) as p_jan,sum(a.d_jan) as d_jan,sum(a.p_feb) as p_feb,sum(a.d_feb) as d_feb from (select distinct col1 as code,decode(to_char(date1,'mm'),'03',1,0) as p_mar,decode(to_char(date1,'mm'),'04',1,0) as p_apr,decode(to_char(date1,'mm'),'05',1,0) as p_may,decode(to_char(date1,'mm'),'06',1,0) as p_jun,decode(to_char(date1,'mm'),'07',1,0) as p_jul,decode(to_char(date1,'mm'),'08',1,0) as p_aug ,decode(to_char(date1,'mm'),'09',1,0) as p_sept,decode(to_char(date1,'mm'),'10',1,0) as p_oct,decode(to_char(date1,'mm'),'11',1,0) as p_nov,decode(to_char(date1,'mm'),'12',1,0) as p_dec,decode(to_char(date1,'mm'),'01',1,0) as p_jan,decode(to_char(date1,'mm'),'02',1,0) as p_feb,0 as d_mar,0 as d_apr,0 as d_may,0 as d_jun,0 as d_jul,0 as d_aug,0 as d_sept,0 as d_oct,0 as d_nov,0 as d_dec,0 as d_jan,0 as d_feb from wb_maint where branchcd='" + frm_mbr + "' and type='MM03' and date1 " + xprdRange + " union all select distinct  col1 as code,0 as p_mar,0 as p_apr,0 as p_may,0 as p_jun,0 as p_jul,0 as p_aug,0 as p_sept,0 as p_oct,0 as p_nov,0 as p_dec,0 as p_jan,0 as p_feb, decode(to_char(date1,'mm'),'03',1,0) as d_mar,decode(to_char(date1,'mm'),'04',1,0) as d_apr,decode(to_char(date1,'mm'),'05',1,0) as d_may,decode(to_char(date1,'mm'),'06',1,0) as d_jun,decode(to_char(date1,'mm'),'07',1,0) as d_jul,decode(to_char(date1,'mm'),'08',1,0) as d_aug,decode(to_char(date1,'mm'),'09',1,0) as d_sept,decode(to_char(date1,'mm'),'10',1,0) as d_oct,decode(to_char(date1,'mm'),'11',1,0) as d_nov,decode(to_char(date1,'mm'),'12',1,0) as  d_dec,decode(to_char(date1,'mm'),'01',1,0) as d_jan,decode(to_char(date1,'mm'),'02',1,0) as d_feb from  wb_maint where branchcd='" + frm_mbr + "' and type='MM05' and date1 " + xprdRange + " ) a,wb_master c , typegrp b where trim(a.code)=trim(c.col1) and trim(a.code)=trim(b.type1) and c.id='MM01' and b.id='MM' and c.branchcd='" + frm_mbr + "' and b.branchcd='" + frm_mbr + "' and nvl(c.col2,'-')!='Y' group by c.name, trim(b.name),c.col9,trim(a.code),trim(c.cpartno),c.col14,c.num15 ,c.num13,c.col15,c.num5 order by mouldcode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_HM_Plan_vs_Act", "std_HM_Plan_vs_Act", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F75168":
                header_n = "PM : Plan Vs Actual";//YEARLY 
                dt1 = new DataTable(); dt = new DataTable();
                SQuery = "select col1 as code,decode(to_char(date1,'mm'),'03',1,0) as p_mar,decode(to_char(date1,'mm'),'04',1,0) as p_apr,decode(to_char(date1,'mm'),'05',1,0) as p_may,decode(to_char(date1,'mm'),'06',1,0) as p_jun,decode(to_char(date1,'mm'),'07',1,0) as p_jul,decode(to_char(date1,'mm'),'08',1,0) as p_aug ,decode(to_char(date1,'mm'),'09',1,0) as p_sept,decode(to_char(date1,'mm'),'10',1,0) as p_oct,decode(to_char(date1,'mm'),'11',1,0) as p_nov,decode(to_char(date1,'mm'),'12',1,0) as p_dec,decode(to_char(date1,'mm'),'01',1,0) as p_jan,decode(to_char(date1,'mm'),'02',1,0) as p_feb,0 as d_mar,0 as d_apr,0 as d_may,0 as d_jun,0 as d_jul,0 as d_aug,0 as d_sept,0 as d_oct,0 as d_nov,0 as d_dec,0 as d_jan,0 as d_feb from wb_maint where branchcd='" + frm_mbr + "' and type='MM02' and date1 " + xprdRange + " union all select distinct  col1 as code,0 as p_mar,0 as p_apr,0 as p_may,0 as p_jun,0 as p_jul,0 as p_aug,0 as p_sept,0 as p_oct,0 as p_nov,0 as p_dec,0 as p_jan,0 as p_feb, decode(to_char(date1,'mm'),'03',1,0) as d_mar,decode(to_char(date1,'mm'),'04',1,0) as d_apr,decode(to_char(date1,'mm'),'05',1,0) as d_may,decode(to_char(date1,'mm'),'06',1,0) as d_jun,decode(to_char(date1,'mm'),'07',1,0) as d_jul,decode(to_char(date1,'mm'),'08',1,0) as d_aug,decode(to_char(date1,'mm'),'09',1,0) as d_sept,decode(to_char(date1,'mm'),'10',1,0) as d_oct,decode(to_char(date1,'mm'),'11',1,0) as d_nov,decode(to_char(date1,'mm'),'12',1,0) as  d_dec,decode(to_char(date1,'mm'),'01',1,0) as d_jan,decode(to_char(date1,'mm'),'02',1,0) as d_feb from  wb_maint where branchcd='" + frm_mbr + "' and type='MM04' and date1 " + xprdRange + " ";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view WBVU_PM_v_ACT as (" + SQuery + ")");

                mq0 = "";
                mq0 = "select sum(a.p_apr+a.p_may+a.p_jun+a.p_jul+a.p_aug+a.p_sept+a.p_oct+a.p_nov+a.p_dec+a.p_jan+a.p_feb+a.p_mar) as planning,sum(a.d_apr+a.d_may+a.d_jun+a.d_jul+a.d_aug+a.d_sept+a.d_oct+a.d_nov+a.d_dec+a.d_jan+a.d_feb+a.d_mar) as actual from (select * from WBVU_PM_v_ACT) a,wb_master c , typegrp b where trim(a.code)=trim(c.col1) and trim(a.code)=trim(b.type1) and c.id='MM01' and b.id='MM' and c.branchcd='" + frm_mbr + "' and b.branchcd='" + frm_mbr + "'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0); //this dt contain total of planning and actual
                if (dt1.Rows.Count > 0)
                {
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + dt1.Rows[0]["planning"].ToString().Trim() + "' as planning,'" + dt1.Rows[0]["actual"].ToString().Trim() + "' as actual,c.name as model,trim(b.name) as mouldname,c.col9 as acref,trim(a.code) as mouldcode,trim(c.cpartno) as partno,c.col14 as material,c.num12 as pm_frequency_as_per_month,c.num6 as pm_frequency_as_per_shots,c.num14 as max_limit_for_alarm,sum(a.p_mar) as p_mar,sum(a.d_mar) as d_mar,sum(a.p_apr) as p_apr,sum(a.d_apr) as d_apr,sum(a.p_may) as p_may,sum(a.d_may) as d_may,sum(a.p_jun) as p_jun,sum(a.d_jun) as d_jun,sum(a.p_jul) as p_jul,sum(a.d_jul) as d_jul,sum(a.p_aug) as p_aug,sum(a.d_aug) as d_aug,sum(a.p_sept) as p_sept,sum(a.d_sept) as d_sept,sum(a.p_oct) as p_oct,sum(a.d_oct) as d_oct,sum(a.p_nov) as p_nov,sum(a.d_nov) as d_nov,sum(a.p_dec)  as p_dec,sum(a.d_dec) as d_dec,sum(a.p_jan) as p_jan,sum(a.d_jan) as d_jan,sum(a.p_feb) as p_feb,sum(a.d_feb) as d_feb from (select * from WBVU_PM_v_ACT) a,wb_master c , typegrp b where trim(a.code)=trim(c.col1) and trim(a.code)=trim(b.type1) and c.id='MM01' and b.id='MM' and c.branchcd='" + frm_mbr + "' and b.branchcd='" + frm_mbr + "' and nvl(c.col2,'-')!='Y' group by c.name,trim(b.name),c.col9,trim(a.code),trim(c.cpartno),c.col14,c.num12,c.num6,c.num14 order by mouldcode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PM_Plan_vs_Act", "std_PM_Plan_vs_Act", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F75169":
                header_n = "HM : Plan Vs Actual Month Wise Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                {
                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                mq2 = mq1 + " " + frm_myear;
                //THIS QRY SHOWS TOTAL OF PLANNING AND ACTUAL
                SQuery = "SELECT sum(A.DAY_01+A.DAY_02+A.DAY_03+A.DAY_04+A.DAY_05+A.DAY_06+A.DAY_07+A.DAY_08+A.DAY_09+A.DAY_10+A.DAY_11+A.DAY_12+A.DAY_13+A.DAY_14+A.DAY_15+ A.DAY_16+A.DAY_17+A.DAY_18+A.DAY_19+A.DAY_20+A.DAY_21+A.DAY_22+A.DAY_23+A.DAY_24+A.DAY_25+A.DAY_26+A.DAY_27+ A.DAY_28+A.DAY_29+A.DAY_30+A.DAY_31) as planning,sum(A.RDAY1+A.RDAY2+A.RDAY3+A.RDAY4+A.RDAY5+ A.RDAY6+A.RDAY7+A.RDAY8+A.RDAY9+A.RDAY10+A.RDAY11+ A.RDAY12+ A.RDAY13+A.RDAY14+A.RDAY15+A.RDAY16+A.RDAY17+A.RDAY18+A.RDAY19+A.RDAY20+A.RDAY21+A.RDAY22+A.RDAY23+A.RDAY24+A.RDAY25+A.RDAY26+A.RDAY27+A.RDAY28+A.RDAY29+A.RDAY30+A.RDAY31) as actual FROM (SELECT DISTINCT col1 as code,decode(to_char(date1,'DD'),'01',1,0) as DAY_01,decode( to_char(date1,'DD'),'02',1,0) as DAY_02,decode(to_char(date1,'DD'),'03',1,0) as DAY_03,decode(to_char(date1,'DD'),'04',1,0) as DAY_04,decode(to_char(date1,'DD'),'05',1,0) as DAY_05,decode(to_char(date1,'DD'),'06',1,0) as DAY_06,decode(to_char(date1,'DD'),'07',1,0) as DAY_07,decode(to_char(date1,'DD'),'08',1,0) as DAY_08,decode(to_char(date1,'DD'),'09',1,0) as DAY_09,decode(to_char(date1,'DD'),'10',1,0) as DAY_10,decode(to_char(date1,'DD'),'11',1,0) as DAY_11,decode(to_char(date1,'DD'),'12',1,0) as DAY_12,decode(to_char(date1,'DD'),'13',1,0) as DAY_13,decode(to_char(date1,'DD'),'14',1,0) as DAY_14,decode(to_char(date1,'DD'),'15',1,0) as DAY_15,decode(to_char(date1,'DD'),'16',1,0) as DAY_16,decode(to_char(date1,'DD'),'17',1,0) as DAY_17,decode(to_char(date1,'DD'),'18',1,0) as DAY_18,decode(to_char(date1,'DD'),'19',1,0) as DAY_19,decode(to_char(date1,'DD'),'20',1,0) as DAY_20,decode(to_char(date1,'DD'),'21',1,0) as DAY_21,decode(to_char(date1,'DD'),'22',1,0) as DAY_22,decode(to_char(date1,'DD'),'23',1,0) as DAY_23,decode(to_char(date1,'DD'),'24',1,0) as DAY_24,decode(to_char(date1,'DD'),'25',1,0) as DAY_25,decode(to_char(date1,'DD'),'26',1,0) as DAY_26,decode(to_char(date1,'DD'),'27',1,0) as DAY_27,decode(to_char(date1,'DD'),'28',1,0) as DAY_28,decode(to_char(date1,'DD'),'29',1,0) as DAY_29,decode(to_char(date1,'DD'),'30',1,0) as DAY_30,decode(to_char(date1,'DD'),'31',1,0) as DAY_31,0 AS RDAY1,0 AS RDAY2,0 AS RDAY3,0 AS RDAY4,0 AS RDAY5,0 AS RDAY6,0 AS RDAY7,0 AS RDAY8,0 AS RDAY9,0 AS RDAY10,0 AS RDAY11,0 AS RDAY12,0 AS RDAY13,0 AS RDAY14,0 AS RDAY15,0 AS RDAY16,0 AS RDAY17,0 AS RDAY18,0 AS RDAY19,0 AS RDAY20,0 AS RDAY21,0 AS RDAY22,0 AS RDAY23,0 AS RDAY24,0 AS RDAY25,0 AS RDAY26,0 AS RDAY27,0 AS RDAY28,0 AS RDAY29,0 AS RDAY30,0 AS RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM03' and obsv2='" + mq0 + "/" + frm_myear + "' union all SELECT DISTINCT COL1 as code,0 AS DAY_01,0 AS DAY_02,0 AS DAY_03,0 AS  DAY_04,0 AS  DAY_05,0 AS  DAY_06,0 AS  DAY_07,0 AS  DAY_08,0 AS  DAY_09,0 AS  DAY_10,0 AS  DAY_11,0 AS  DAY_12,0 AS  DAY_13,0 AS  DAY_14,0 AS  DAY_15,0 AS  DAY_16,0 AS  DAY_17,0 AS  DAY_18,0 AS  DAY_19,0 AS  DAY_20,0 AS  DAY_21,0 AS  DAY_22,0 AS  DAY_23,0 AS  DAY_24,0 AS  DAY_25,0 AS  DAY_26,0 AS  DAY_27,0 AS  DAY_28,0 AS  DAY_29,0 AS  DAY_30,0 AS  DAY_31,decode(to_char(date1,'DD'),'01',1,0) as RDAY1,decode(to_char(date1,'DD'),'02',1,0) as RDAY2,decode(to_char(date1,'DD'),'03',1,0) as RDAY3,decode(to_char(date1,'DD'),'04',1,0) as RDAY4,decode(to_char(date1,'DD'),'05',1,0) as RDAY5,decode(to_char(date1,'DD'),'06',1,0) as RDAY6,decode(to_char(date1,'DD'),'07',1,0) as RDAY7,decode(to_char(date1,'DD'),'08',1,0) as RDAY8,decode(to_char(date1,'DD'),'09',1,0) as RDAY9,decode(to_char(date1,'DD'),'10',1,0) as RDAY10,decode(to_char(date1,'DD'),'11',1,0) as RDAY11,decode(to_char(date1,'DD'),'12',1,0) as RDAY12,decode(to_char(date1,'DD'),'13',1,0) as RDAY13,decode(to_char(date1,'DD'),'14',1,0) as RDAY14,decode(to_char(date1,'DD'),'15',1,0) as RDAY15,decode(to_char(date1,'DD'),'16',1,0) as RDAY16,decode(to_char(date1,'DD'),'17',1,0) as RDAY17,decode(to_char(date1,'DD'),'18',1,0) as RDAY18,decode(to_char(date1,'DD'),'19',1,0) as RDAY19,decode(to_char(date1,'DD'),'20',1,0) as RDAY20,decode(to_char(date1,'DD'),'21',1,0) as RDAY21,decode(to_char(date1,'DD'),'22',1,0) as RDAY22,decode(to_char(date1,'DD'),'23',1,0) as RDAY23,decode(to_char(date1,'DD'),'24',1,0) as RDAY24,decode(to_char(date1,'DD'),'25',1,0) as RDAY25,decode(to_char(date1,'DD'),'26',1,0) as RDAY26,decode(to_char(date1,'DD'),'27',1,0) as RDAY27,decode(to_char(date1,'DD'),'28',1,0) as RDAY28,decode(to_char(date1,'DD'),'29',1,0) as RDAY29,decode(to_char(date1,'DD'),'30',1,0) as RDAY30,decode(to_char(date1,'DD'),'31',1,0) as RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM05' and TO_CHar(date1,'mm/yyyy')='" + mq0 + "/" + frm_myear + "' ) a";
                dt1 = new DataTable();
                dt = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    SQuery = "SELECT  '" + mq2 + "' as month_,'" + header_n + "' as header,'" + dt1.Rows[0]["planning"].ToString().Trim() + "' as palnning,'" + dt1.Rows[0]["actual"].ToString().Trim() + "' as actual, c.name as model,trim(b.name) as mouldname,c.col9 as acref,trim(a.code) as mouldcode,SUM(A.DAY_01) AS DAY_01,SUM(A.DAY_02) AS DAY_02,SUM(A.DAY_03) AS DAY_03,SUM(A.DAY_04) AS DAY_04,SUM(A.DAY_05) AS DAY_05,SUM(A.DAY_06) AS DAY_06,SUM(A.DAY_07) AS DAY_07,SUM(A.DAY_08) AS DAY_08,SUM(A.DAY_09) AS DAY_09,SUM(A.DAY_10) AS DAY_10,SUM(A.DAY_11) AS DAY_11,SUM(A.DAY_12) AS DAY_12,SUM(A.DAY_13) AS DAY_13,SUM(A.DAY_14) AS DAY_14,SUM(A.DAY_15) AS DAY_15,SUM(A.DAY_16) AS DAY_16,SUM(A.DAY_17) AS DAY_17,SUM(A.DAY_18) AS DAY_18,SUM(A.DAY_19) AS DAY_19,SUM(A.DAY_20) AS DAY_20,SUM(A.DAY_21) AS DAY_21,SUM(A.DAY_22) AS DAY_22,SUM(A.DAY_23) AS DAY_23,SUM(A.DAY_24) AS DAY_24,SUM(A.DAY_25) AS DAY_25,SUM(A.DAY_26) AS DAY_26,SUM(A.DAY_27) AS DAY_27,SUM(A.DAY_28) AS DAY_28,SUM(A.DAY_29) AS DAY_29,SUM(A.DAY_30) AS DAY_30,SUM(A.DAY_31) AS DAY_31,SUM(A.RDAY1) AS RDAY1,SUM(A.RDAY2) AS RDAY2,SUM(A.RDAY3) AS RDAY3,SUM(A.RDAY4) AS RDAY4,SUM(A.RDAY5) AS RDAY5,sUM(A.RDAY6) AS RDAY6,SUM(A.RDAY7) AS RDAY7,SUM(A.RDAY8) AS RDAY8,SUM(A.RDAY9) AS RDAY9,SUM(A.RDAY10) AS RDAY10,SUM(A.RDAY11) AS RDAY11,SUM(A.RDAY12) AS RDAY12,SUM(A.RDAY13) AS RDAY13,SUM(A.RDAY14) AS RDAY14,SUM(A.RDAY15) AS RDAY15,SUM(A.RDAY16) AS RDAY16,SUM(A.RDAY17) AS RDAY17,SUM(A.RDAY18) AS RDAY18,SUM(A.RDAY19) AS RDAY19,SUM(A.RDAY20) AS RDAY20,SUM(A.RDAY21) AS RDAY21,SUM(A.RDAY22) AS RDAY22,SUM(A.RDAY23) AS RDAY23,SUM(A.RDAY24) AS RDAY24,SUM(A.RDAY25) AS RDAY25,SUM(A.RDAY26) AS RDAY26,SUM(A.RDAY27) AS RDAY27,SUM(A.RDAY28) AS RDAY28,SUM(A.RDAY29) AS RDAY29,SUM(A.RDAY30) AS RDAY30,SUM(A.RDAY31) AS RDAY31 FROM (SELECT col1 as code,decode(to_char(date1,'DD'),'01',1,0) as DAY_01,decode( to_char(date1,'DD'),'02',1,0) as DAY_02,decode(to_char(date1,'DD'),'03',1,0) as DAY_03,decode(to_char(date1,'DD'),'04',1,0) as DAY_04,decode(to_char(date1,'DD'),'05',1,0) as DAY_05,decode(to_char(date1,'DD'),'06',1,0) as DAY_06,decode(to_char(date1,'DD'),'07',1,0) as DAY_07,decode(to_char(date1,'DD'),'08',1,0) as DAY_08,decode(to_char(date1,'DD'),'09',1,0) as DAY_09,decode(to_char(date1,'DD'),'10',1,0) as DAY_10,decode(to_char(date1,'DD'),'11',1,0) as DAY_11,decode(to_char(date1,'DD'),'12',1,0) as DAY_12,decode(to_char(date1,'DD'),'13',1,0) as DAY_13,decode(to_char(date1,'DD'),'14',1,0) as DAY_14,decode(to_char(date1,'DD'),'15',1,0) as DAY_15,decode(to_char(date1,'DD'),'16',1,0) as DAY_16,decode(to_char(date1,'DD'),'17',1,0) as DAY_17,decode(to_char(date1,'DD'),'18',1,0) as DAY_18,decode(to_char(date1,'DD'),'19',1,0) as DAY_19,decode(to_char(date1,'DD'),'20',1,0) as DAY_20,decode(to_char(date1,'DD'),'21',1,0) as DAY_21,decode(to_char(date1,'DD'),'22',1,0) as DAY_22,decode(to_char(date1,'DD'),'23',1,0) as DAY_23,decode(to_char(date1,'DD'),'24',1,0) as DAY_24,decode(to_char(date1,'DD'),'25',1,0) as DAY_25,decode(to_char(date1,'DD'),'26',1,0) as DAY_26,decode(to_char(date1,'DD'),'27',1,0) as DAY_27,decode(to_char(date1,'DD'),'28',1,0) as DAY_28,decode(to_char(date1,'DD'),'29',1,0) as DAY_29,decode(to_char(date1,'DD'),'30',1,0) as DAY_30,decode(to_char(date1,'DD'),'31',1,0) as DAY_31,0 AS RDAY1,0 AS RDAY2,0 AS RDAY3,0 AS RDAY4,0 AS RDAY5,0 AS RDAY6,0 AS RDAY7,0 AS RDAY8,0 AS RDAY9,0 AS RDAY10,0 AS RDAY11,0 AS RDAY12,0 AS RDAY13,0 AS RDAY14,0 AS RDAY15,0 AS RDAY16,0 AS RDAY17,0 AS RDAY18,0 AS RDAY19,0 AS RDAY20,0 AS RDAY21,0 AS RDAY22,0 AS RDAY23,0 AS RDAY24,0 AS RDAY25,0 AS RDAY26,0 AS RDAY27,0 AS RDAY28,0 AS RDAY29,0 AS RDAY30,0 AS RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM03' and obsv2='" + mq0 + "/" + frm_myear + "' union all SELECT distinct col1 as code,0 AS DAY_01,0 AS DAY_02,0 AS DAY_03,0 AS  DAY_04,0 AS  DAY_05,0 AS  DAY_06,0 AS  DAY_07,0 AS  DAY_08,0 AS  DAY_09,0 AS  DAY_10,0 AS  DAY_11,0 AS  DAY_12,0 AS  DAY_13,0 AS  DAY_14,0 AS  DAY_15,0 AS  DAY_16,0 AS  DAY_17,0 AS  DAY_18,0 AS  DAY_19,0 AS  DAY_20,0 AS  DAY_21,0 AS  DAY_22,0 AS  DAY_23,0 AS  DAY_24,0 AS  DAY_25,0 AS  DAY_26,0 AS  DAY_27,0 AS  DAY_28,0 AS  DAY_29,0 AS  DAY_30,0 AS  DAY_31,decode(to_char(date1,'DD'),'01',1,0) as RDAY1,decode(to_char(date1,'DD'),'02',1,0) as RDAY2,decode(to_char(date1,'DD'),'03',1,0) as RDAY3,decode(to_char(date1,'DD'),'04',1,0) as RDAY4,decode(to_char(date1,'DD'),'05',1,0) as RDAY5,decode(to_char(date1,'DD'),'06',1,0) as RDAY6,decode(to_char(date1,'DD'),'07',1,0) as RDAY7,decode(to_char(date1,'DD'),'08',1,0) as RDAY8,decode(to_char(date1,'DD'),'09',1,0) as RDAY9,decode(to_char(date1,'DD'),'10',1,0) as RDAY10,decode(to_char(date1,'DD'),'11',1,0) as RDAY11,decode(to_char(date1,'DD'),'12',1,0) as RDAY12,decode(to_char(date1,'DD'),'13',1,0) as RDAY13,decode(to_char(date1,'DD'),'14',1,0) as RDAY14,decode(to_char(date1,'DD'),'15',1,0) as RDAY15,decode(to_char(date1,'DD'),'16',1,0) as RDAY16,decode(to_char(date1,'DD'),'17',1,0) as RDAY17,decode(to_char(date1,'DD'),'18',1,0) as RDAY18,decode(to_char(date1,'DD'),'19',1,0) as RDAY19,decode(to_char(date1,'DD'),'20',1,0) as RDAY20,decode(to_char(date1,'DD'),'21',1,0) as RDAY21,decode(to_char(date1,'DD'),'22',1,0) as RDAY22,decode(to_char(date1,'DD'),'23',1,0) as RDAY23,decode(to_char(date1,'DD'),'24',1,0) as RDAY24,decode(to_char(date1,'DD'),'25',1,0) as RDAY25,decode(to_char(date1,'DD'),'26',1,0) as RDAY26,decode(to_char(date1,'DD'),'27',1,0) as RDAY27,decode(to_char(date1,'DD'),'28',1,0) as RDAY28,decode(to_char(date1,'DD'),'29',1,0) as RDAY29,decode(to_char(date1,'DD'),'30',1,0) as RDAY30,decode(to_char(date1,'DD'),'31',1,0) as RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM05' and to_char(date1,'mm/yyyy')='" + mq0 + "/" + frm_myear + "') a,wb_master c,typegrp b where trim(a.code)=trim(c.col1) and trim(a.code)=trim(b.type1) and c.id='MM01' and b.id='MM' and c.branchcd='" + frm_mbr + "' and b.branchcd='" + frm_mbr + "' and nvl(c.col2,'-')!='Y' GROUP BY c.name, trim(b.name),c.col9,trim(a.code) order by mouldcode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_HM_Pln_vs_Act_mth", "std_HM_Pln_vs_Act_mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F75170":
                header_n = "PM : Plan Vs Actual Month Wise Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                {
                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                mq2 = mq1 + " " + frm_myear;
                //THIS QRY SHOWS TOTAL OF PLANNING AND ACTUAL
                SQuery = "select sum(A.DAY_01+A.DAY_02+A.DAY_03+A.DAY_04+A.DAY_05+A.DAY_06+A.DAY_07+A.DAY_08+A.DAY_09+A.DAY_10+A.DAY_11+A.DAY_12+A.DAY_13+A.DAY_14+A.DAY_15+ A.DAY_16+A.DAY_17+A.DAY_18+A.DAY_19+A.DAY_20+A.DAY_21+A.DAY_22+A.DAY_23+A.DAY_24+A.DAY_25+A.DAY_26+A.DAY_27+ A.DAY_28+A.DAY_29+A.DAY_30+A.DAY_31) as planning,sum(A.RDAY1+A.RDAY2+A.RDAY3+A.RDAY4+A.RDAY5+ A.RDAY6+A.RDAY7+A.RDAY8+A.RDAY9+A.RDAY10+A.RDAY11+ A.RDAY12+ A.RDAY13+A.RDAY14+A.RDAY15+A.RDAY16+A.RDAY17+A.RDAY18+A.RDAY19+A.RDAY20+A.RDAY21+A.RDAY22+A.RDAY23+A.RDAY24+A.RDAY25+A.RDAY26+A.RDAY27+A.RDAY28+A.RDAY29+A.RDAY30+A.RDAY31) as actual FROM (SELECT DISTINCT col1 as code,decode(to_char(date1,'DD'),'01',1,0) as DAY_01,decode( to_char(date1,'DD'),'02',1,0) as DAY_02,decode(to_char(date1,'DD'),'03',1,0) as DAY_03,decode(to_char(date1,'DD'),'04',1,0) as DAY_04,decode(to_char(date1,'DD'),'05',1,0) as DAY_05,decode(to_char(date1,'DD'),'06',1,0) as DAY_06,decode(to_char(date1,'DD'),'07',1,0) as DAY_07,decode(to_char(date1,'DD'),'08',1,0) as DAY_08,decode(to_char(date1,'DD'),'09',1,0) as DAY_09,decode(to_char(date1,'DD'),'10',1,0) as DAY_10,decode(to_char(date1,'DD'),'11',1,0) as DAY_11,decode(to_char(date1,'DD'),'12',1,0) as DAY_12,decode(to_char(date1,'DD'),'13',1,0) as DAY_13,decode(to_char(date1,'DD'),'14',1,0) as DAY_14,decode(to_char(date1,'DD'),'15',1,0) as DAY_15,decode(to_char(date1,'DD'),'16',1,0) as DAY_16,decode(to_char(date1,'DD'),'17',1,0) as DAY_17,decode(to_char(date1,'DD'),'18',1,0) as DAY_18,decode(to_char(date1,'DD'),'19',1,0) as DAY_19,decode(to_char(date1,'DD'),'20',1,0) as DAY_20,decode(to_char(date1,'DD'),'21',1,0) as DAY_21,decode(to_char(date1,'DD'),'22',1,0) as DAY_22,decode(to_char(date1,'DD'),'23',1,0) as DAY_23,decode(to_char(date1,'DD'),'24',1,0) as DAY_24,decode(to_char(date1,'DD'),'25',1,0) as DAY_25,decode(to_char(date1,'DD'),'26',1,0) as DAY_26,decode(to_char(date1,'DD'),'27',1,0) as DAY_27,decode(to_char(date1,'DD'),'28',1,0) as DAY_28,decode(to_char(date1,'DD'),'29',1,0) as DAY_29,decode(to_char(date1,'DD'),'30',1,0) as DAY_30,decode(to_char(date1,'DD'),'31',1,0) as DAY_31,0 AS RDAY1,0 AS RDAY2,0 AS RDAY3,0 AS RDAY4,0 AS RDAY5,0 AS RDAY6,0 AS RDAY7,0 AS RDAY8,0 AS RDAY9,0 AS RDAY10,0 AS RDAY11,0 AS RDAY12,0 AS RDAY13,0 AS RDAY14,0 AS RDAY15,0 AS RDAY16,0 AS RDAY17,0 AS RDAY18,0 AS RDAY19,0 AS RDAY20,0 AS RDAY21,0 AS RDAY22,0 AS RDAY23,0 AS RDAY24,0 AS RDAY25,0 AS RDAY26,0 AS RDAY27,0 AS RDAY28,0 AS RDAY29,0 AS RDAY30,0 AS RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM02' and obsv2='" + mq0 + "/" + frm_myear + "' union all SELECT DISTINCT col1 as code,0 AS DAY_01,0 AS DAY_02,0 AS DAY_03,0 AS  DAY_04,0 AS  DAY_05,0 AS  DAY_06,0 AS  DAY_07,0 AS  DAY_08,0 AS  DAY_09,0 AS  DAY_10,0 AS  DAY_11,0 AS  DAY_12,0 AS  DAY_13,0 AS  DAY_14,0 AS  DAY_15,0 AS  DAY_16,0 AS  DAY_17,0 AS  DAY_18,0 AS  DAY_19,0 AS  DAY_20,0 AS  DAY_21,0 AS  DAY_22,0 AS  DAY_23,0 AS  DAY_24,0 AS  DAY_25,0 AS  DAY_26,0 AS  DAY_27,0 AS  DAY_28,0 AS  DAY_29,0 AS  DAY_30,0 AS  DAY_31,decode(to_char(date1,'DD'),'01',1,0) as RDAY1,decode(to_char(date1,'DD'),'02',1,0) as RDAY2,decode(to_char(date1,'DD'),'03',1,0) as RDAY3,decode(to_char(date1,'DD'),'04',1,0) as RDAY4,decode(to_char(date1,'DD'),'05',1,0) as RDAY5,decode(to_char(date1,'DD'),'06',1,0) as RDAY6,decode(to_char(date1,'DD'),'07',1,0) as RDAY7,decode(to_char(date1,'DD'),'08',1,0) as RDAY8,decode(to_char(date1,'DD'),'09',1,0) as RDAY9,decode(to_char(date1,'DD'),'10',1,0) as RDAY10,decode(to_char(date1,'DD'),'11',1,0) as RDAY11,decode(to_char(date1,'DD'),'12',1,0) as RDAY12,decode(to_char(date1,'DD'),'13',1,0) as RDAY13,decode(to_char(date1,'DD'),'14',1,0) as RDAY14,decode(to_char(date1,'DD'),'15',1,0) as RDAY15,decode(to_char(date1,'DD'),'16',1,0) as RDAY16,decode(to_char(date1,'DD'),'17',1,0) as RDAY17,decode(to_char(date1,'DD'),'18',1,0) as RDAY18,decode(to_char(date1,'DD'),'19',1,0) as RDAY19,decode(to_char(date1,'DD'),'20',1,0) as RDAY20,decode(to_char(date1,'DD'),'21',1,0) as RDAY21,decode(to_char(date1,'DD'),'22',1,0) as RDAY22,decode(to_char(date1,'DD'),'23',1,0) as RDAY23,decode(to_char(date1,'DD'),'24',1,0) as RDAY24,decode(to_char(date1,'DD'),'25',1,0) as RDAY25,decode(to_char(date1,'DD'),'26',1,0) as RDAY26,decode(to_char(date1,'DD'),'27',1,0) as RDAY27,decode(to_char(date1,'DD'),'28',1,0) as RDAY28,decode(to_char(date1,'DD'),'29',1,0) as RDAY29,decode(to_char(date1,'DD'),'30',1,0) as RDAY30,decode(to_char(date1,'DD'),'31',1,0) as RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM04' and TO_CHar(date1,'mm/yyyy')='" + mq0 + "/" + frm_myear + "') a";
                dt1 = new DataTable();
                dt = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    SQuery = "SELECT '" + mq2 + "' as month_,'" + header_n + "' as header,'" + dt1.Rows[0]["planning"].ToString().Trim() + "' as palnning,'" + dt1.Rows[0]["actual"].ToString().Trim() + "' as actual, c.name as model, trim(b.name) as mouldname,c.col9 as acref ,trim(a.code) as mouldcode,SUM(A.DAY_01) AS DAY_01,SUM(A.DAY_02) AS DAY_02,SUM(A.DAY_03) AS DAY_03,SUM(A.DAY_04) AS DAY_04,SUM(A.DAY_05) AS DAY_05,SUM(A.DAY_06) AS DAY_06,SUM(A.DAY_07) AS DAY_07,SUM(A.DAY_08) AS DAY_08,SUM(A.DAY_09) AS DAY_09,SUM(A.DAY_10) AS DAY_10,SUM(A.DAY_11) AS DAY_11,SUM(A.DAY_12) AS DAY_12,SUM(A.DAY_13) AS DAY_13,SUM(A.DAY_14) AS DAY_14,SUM(A.DAY_15) AS DAY_15,SUM(A.DAY_16) AS DAY_16,SUM(A.DAY_17) AS DAY_17,SUM(A.DAY_18) AS DAY_18,SUM(A.DAY_19) AS DAY_19,SUM(A.DAY_20) AS DAY_20,SUM(A.DAY_21) AS DAY_21,SUM(A.DAY_22) AS DAY_22,SUM(A.DAY_23) AS DAY_23,SUM(A.DAY_24) AS DAY_24,SUM(A.DAY_25) AS DAY_25,SUM(A.DAY_26) AS DAY_26,SUM(A.DAY_27) AS DAY_27,SUM(A.DAY_28) AS DAY_28,SUM(A.DAY_29) AS DAY_29,SUM(A.DAY_30) AS DAY_30,SUM(A.DAY_31) AS DAY_31,SUM(A.RDAY1) AS RDAY1,SUM(A.RDAY2) AS RDAY2,SUM(A.RDAY3) AS RDAY3,SUM(A.RDAY4) AS RDAY4,SUM(A.RDAY5) AS RDAY5,sUM(A.RDAY6) AS RDAY6,SUM(A.RDAY7) AS RDAY7,SUM(A.RDAY8) AS RDAY8,SUM(A.RDAY9) AS RDAY9,SUM(A.RDAY10) AS RDAY10,SUM(A.RDAY11) AS RDAY11,SUM(A.RDAY12) AS RDAY12,SUM(A.RDAY13) AS RDAY13,SUM(A.RDAY14) AS RDAY14,SUM(A.RDAY15) AS RDAY15,SUM(A.RDAY16) AS RDAY16,SUM(A.RDAY17) AS RDAY17,SUM(A.RDAY18) AS RDAY18,SUM(A.RDAY19) AS RDAY19,SUM(A.RDAY20) AS RDAY20,SUM(A.RDAY21) AS RDAY21,SUM(A.RDAY22) AS RDAY22,SUM(A.RDAY23) AS RDAY23,SUM(A.RDAY24) AS RDAY24,SUM(A.RDAY25) AS RDAY25,SUM(A.RDAY26) AS RDAY26,SUM(A.RDAY27) AS RDAY27,SUM(A.RDAY28) AS RDAY28,SUM(A.RDAY29) AS RDAY29,SUM(A.RDAY30) AS RDAY30,SUM(A.RDAY31) AS RDAY31 FROM (SELECT DISTINCT col1 as code,decode(to_char(date1,'DD'),'01',1,0) as DAY_01,decode( to_char(date1,'DD'),'02',1,0) as DAY_02,decode(to_char(date1,'DD'),'03',1,0) as DAY_03,decode(to_char(date1,'DD'),'04',1,0) as DAY_04,decode(to_char(date1,'DD'),'05',1,0) as DAY_05,decode(to_char(date1,'DD'),'06',1,0) as DAY_06,decode(to_char(date1,'DD'),'07',1,0) as DAY_07,decode(to_char(date1,'DD'),'08',1,0) as DAY_08,decode(to_char(date1,'DD'),'09',1,0) as DAY_09,decode(to_char(date1,'DD'),'10',1,0) as DAY_10,decode(to_char(date1,'DD'),'11',1,0) as DAY_11,decode(to_char(date1,'DD'),'12',1,0) as DAY_12,decode(to_char(date1,'DD'),'13',1,0) as DAY_13,decode(to_char(date1,'DD'),'14',1,0) as DAY_14,decode(to_char(date1,'DD'),'15',1,0) as DAY_15,decode(to_char(date1,'DD'),'16',1,0) as DAY_16,decode(to_char(date1,'DD'),'17',1,0) as DAY_17,decode(to_char(date1,'DD'),'18',1,0) as DAY_18,decode(to_char(date1,'DD'),'19',1,0) as DAY_19,decode(to_char(date1,'DD'),'20',1,0) as DAY_20,decode(to_char(date1,'DD'),'21',1,0) as DAY_21,decode(to_char(date1,'DD'),'22',1,0) as DAY_22,decode(to_char(date1,'DD'),'23',1,0) as DAY_23,decode(to_char(date1,'DD'),'24',1,0) as DAY_24,decode(to_char(date1,'DD'),'25',1,0) as DAY_25,decode(to_char(date1,'DD'),'26',1,0) as DAY_26,decode(to_char(date1,'DD'),'27',1,0) as DAY_27,decode(to_char(date1,'DD'),'28',1,0) as DAY_28,decode(to_char(date1,'DD'),'29',1,0) as DAY_29,decode(to_char(date1,'DD'),'30',1,0) as DAY_30,decode(to_char(date1,'DD'),'31',1,0) as DAY_31,0 AS RDAY1,0 AS RDAY2,0 AS RDAY3,0 AS RDAY4,0 AS RDAY5,0 AS RDAY6,0 AS RDAY7,0 AS RDAY8,0 AS RDAY9,0 AS RDAY10,0 AS RDAY11,0 AS RDAY12,0 AS RDAY13,0 AS RDAY14,0 AS RDAY15,0 AS RDAY16,0 AS RDAY17,0 AS RDAY18,0 AS RDAY19,0 AS RDAY20,0 AS RDAY21,0 AS RDAY22,0 AS RDAY23,0 AS RDAY24,0 AS RDAY25,0 AS RDAY26,0 AS RDAY27,0 AS RDAY28,0 AS RDAY29,0 AS RDAY30,0 AS RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM02' and obsv2='" + mq0 + "/" + frm_myear + "' union all SELECT DISTINCT col1 as code,0 AS DAY_01,0 AS DAY_02,0 AS DAY_03,0 AS  DAY_04,0 AS  DAY_05,0 AS  DAY_06,0 AS  DAY_07,0 AS  DAY_08,0 AS  DAY_09,0 AS  DAY_10,0 AS  DAY_11,0 AS  DAY_12,0 AS  DAY_13,0 AS  DAY_14,0 AS  DAY_15,0 AS  DAY_16,0 AS  DAY_17,0 AS  DAY_18,0 AS  DAY_19,0 AS  DAY_20,0 AS  DAY_21,0 AS  DAY_22,0 AS  DAY_23,0 AS  DAY_24,0 AS  DAY_25,0 AS  DAY_26,0 AS  DAY_27,0 AS  DAY_28,0 AS  DAY_29,0 AS  DAY_30,0 AS  DAY_31,decode(to_char(date1,'DD'),'01',1,0) as RDAY1,decode(to_char(date1,'DD'),'02',1,0) as RDAY2,decode(to_char(date1,'DD'),'03',1,0) as RDAY3,decode(to_char(date1,'DD'),'04',1,0) as RDAY4,decode(to_char(date1,'DD'),'05',1,0) as RDAY5,decode(to_char(date1,'DD'),'06',1,0) as RDAY6,decode(to_char(date1,'DD'),'07',1,0) as RDAY7,decode(to_char(date1,'DD'),'08',1,0) as RDAY8,decode(to_char(date1,'DD'),'09',1,0) as RDAY9,decode(to_char(date1,'DD'),'10',1,0) as RDAY10,decode(to_char(date1,'DD'),'11',1,0) as RDAY11,decode(to_char(date1,'DD'),'12',1,0) as RDAY12,decode(to_char(date1,'DD'),'13',1,0) as RDAY13,decode(to_char(date1,'DD'),'14',1,0) as RDAY14,decode(to_char(date1,'DD'),'15',1,0) as RDAY15,decode(to_char(date1,'DD'),'16',1,0) as RDAY16,decode(to_char(date1,'DD'),'17',1,0) as RDAY17,decode(to_char(date1,'DD'),'18',1,0) as RDAY18,decode(to_char(date1,'DD'),'19',1,0) as RDAY19,decode(to_char(date1,'DD'),'20',1,0) as RDAY20,decode(to_char(date1,'DD'),'21',1,0) as RDAY21,decode(to_char(date1,'DD'),'22',1,0) as RDAY22,decode(to_char(date1,'DD'),'23',1,0) as RDAY23,decode(to_char(date1,'DD'),'24',1,0) as RDAY24,decode(to_char(date1,'DD'),'25',1,0) as RDAY25,decode(to_char(date1,'DD'),'26',1,0) as RDAY26,decode(to_char(date1,'DD'),'27',1,0) as RDAY27,decode(to_char(date1,'DD'),'28',1,0) as RDAY28,decode(to_char(date1,'DD'),'29',1,0) as RDAY29,decode(to_char(date1,'DD'),'30',1,0) as RDAY30,decode(to_char(date1,'DD'),'31',1,0) as RDAY31 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='MM04' and to_char(date1,'mm/yyyy')='" + mq0 + "/" + frm_myear + "') a,wb_master c,typegrp b where trim(a.code)=trim(c.col1) and trim(a.code)=trim(b.type1) and c.id='MM01' and b.id='MM' and c.branchcd='" + frm_mbr + "' and b.branchcd='" + frm_mbr + "' GROUP BY c.name, trim(b.name),c.col9,trim(a.code) order by mouldcode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_HM_Pln_vs_Act_mth", "std_HM_Pln_vs_Act_mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F75189":

                header_n = "PM Monthly Pending Maintenance Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                {
                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                mq2 = mq1 + "/" + frm_myear;

                SQuery = "select sum(day1+day2+day3+day4+day5+day6+day7+day8+day9+day10+day11+day12+day13+day14+day15+day16+day17+day18+day19+day20+day21+day22+day23+day24+day25+day26+day27+day28+day29+day30+day31) as tot from (Select col1 as code,decode(to_char(date1,'dd'),'01',1,0) as day1,decode(to_char(date1,'dd'),'02',1,0) as day2,decode(to_char(date1,'dd'),'03',1,0) as day3,decode(to_char(date1,'dd'),'04',1,0) as day4,decode(to_char(date1,'dd'),'05',1,0) as day5,decode(to_char(date1,'dd'),'06',1,0) as day6,decode(to_char(date1,'dd'),'07',1,0) as day7,decode(to_char(date1,'dd'),'08',1,0) as day8,decode(to_char(date1,'dd'),'09',1,0) as day9,decode(to_char(date1,'dd'),'10',1,0) as day10,decode(to_char(date1,'dd'),'11',1,0) as day11,decode(to_char(date1,'dd'),'12',1,0) as day12,decode(to_char(date1,'dd'),'13',1,0) as day13,decode(to_char(date1,'dd'),'14',1,0) as day14,decode(to_char(date1,'dd'),'15',1,0) as day15,decode(to_char(date1,'dd'),'16',1,0) as day16,decode(to_char(date1,'dd'),'17',1,0) as day17,decode(to_char(date1,'dd'),'18',1,0) as day18,decode(to_char(date1,'dd'),'19',1,0) as day19,decode(to_char(date1,'dd'),'20',1,0) as day20,decode(to_char(date1,'dd'),'21',1,0) as day21,decode(to_char(date1,'dd'),'22',1,0) as day22,decode(to_char(date1,'dd'),'23',1,0) as day23,decode(to_char(date1,'dd'),'24',1,0) as day24,decode(to_char(date1,'dd'),'25',1,0) as day25,decode(to_char(date1,'dd'),'26',1,0) as day26,decode(to_char(date1,'dd'),'27',1,0) as day27,decode(to_char(date1,'dd'),'28',1,0) as day28,decode(to_char(date1,'dd'),'29',1,0) as day29,decode(to_char(date1,'dd'),'30',1,0) as day30,decode(to_char(date1,'dd'),'31',1,0) as day31 from wb_maint where branchcd='" + frm_mbr + "' and type='MM02' and obsv2='" + mq0 + "/" + frm_myear + "')";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    SQuery = "Select '" + mq2 + "' as month_,'" + header_n + "' as header, a.col1 as code,b.name as mould_name,c.col9 as acref," + dt1.Rows[0]["tot"].ToString().Trim() + " as total_planned,decode(to_char(a.date1,'dd'),'01','1','') as day1,decode(to_char(a.date1,'dd'),'02','1','') as day2,decode(to_char(a.date1,'dd'),'03','1','') as day3,decode(to_char(a.date1,'dd'),'04','1','') as day4,decode(to_char(a.date1,'dd'),'05','1','') as day5,decode(to_char(a.date1,'dd'),'06','1','') as day6,decode(to_char(a.date1,'dd'),'07','1','') as day7,decode(to_char(a.date1,'dd'),'08','1','') as day8,decode(to_char(a.date1,'dd'),'09','1','') as day9,decode(to_char(a.date1,'dd'),'10','1','') as day10,decode(to_char(a.date1,'dd'),'11','1','') as day11,decode(to_char(a.date1,'dd'),'12','1','') as day12,decode(to_char(a.date1,'dd'),'13','1','') as day13,decode(to_char(a.date1,'dd'),'14','1','') as day14,decode(to_char(a.date1,'dd'),'15','1','') as day15,decode(to_char(a.date1,'dd'),'16','1','') as day16,decode(to_char(a.date1,'dd'),'17','1','') as day17,decode(to_char(a.date1,'dd'),'18','1','') as day18,decode(to_char(a.date1,'dd'),'19','1','') as day19,decode(to_char(a.date1,'dd'),'20','1','') as day20,decode(to_char(a.date1,'dd'),'21','1','') as day21,decode(to_char(a.date1,'dd'),'22','1','') as day22,decode(to_char(a.date1,'dd'),'23','1','') as day23,decode(to_char(a.date1,'dd'),'24','1','') as day24,decode(to_char(a.date1,'dd'),'25','1','') as day25,decode(to_char(a.date1,'dd'),'26','1','') as day26,decode(to_char(a.date1,'dd'),'27','1','') as day27,decode(to_char(a.date1,'dd'),'28','1','') as day28,decode(to_char(a.date1,'dd'),'29','1','') as day29,decode(to_char(a.date1,'dd'),'30','1','') as day30,decode(to_char(a.date1,'dd'),'31','1','') as day31 from wb_maint a ,typegrp b,wb_master c where trim(a.branchcd)||trim(a.col1)=trim(b.branchcd)||trim(b.type1) and trim(a.branchcd)||trim(a.col1)=trim(c.branchcd)||trim(c.col1) and c.id='MM01' and a.branchcd='" + frm_mbr + "' and a.type='MM02' and b.id='MM' and nvl(c.col2,'-')!='Y' and a.obsv2='" + mq0 + "/" + frm_myear + "' order by mould_name";
                }

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "mould_plan";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mnth_Plan", "Mnth_Plan", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
            case "F75113":
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select b.name as shift,a.* from scratch a,TYPE B where TRIM(A.ACODE)=trim(b.type1) and b.id='D' AND a.branchcd='" + frm_mbr + "' and a.type='MN' AND  a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') in (" + mq0 + ")";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_compcard", "std_compcard", dsRep, header_n);
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