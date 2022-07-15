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

public partial class pay_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, DateRange, fromdt, todt, header_n, cond = "";
    fgenDB fgen = new fgenDB();
    string data_found = "Y"; string myear;
    string xprd1 = "", firm, xhtml_tag, subj; string mq10;
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, ph_tbl;

    protected void Page_Load(object sender, EventArgs e)
    {
        // try
        // {
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
                DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            }
            else Response.Redirect("~/login.aspx");
        }
        //if (!Page.IsPostBack)
        //{
        //  printCrpt(hfhcid.Value);
        //  CrystalReportViewer1.Focus();
        //}
        if (!Page.IsPostBack)
        {
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "USEND_MAIL") == "Y") tremail.Visible = true;
            else tremail.Visible = false;

            printCrpt(hfhcid.Value);
            if (data_found == "N")
            {
                No_Data_Found.Visible = true;
            }
            else
            {
                CrystalReportViewer1.RefreshReport();
                CrystalReportViewer1.Focus();
            }
        }
        // }
        // catch (Exception ex)
        // {
        //     fgen.FILL_ERR(ex.Message);
        //}
    }

    void printCrpt(string iconID)
    {
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        string party_cd = "";
        string part_cd = "";
        string mq10, mq1, mq0, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq11, mq12, ded1;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "";
        data_found = "Y";
        switch (iconID)
        {
            case "F85147":
                #region joining List
                header_n = "Date of Joining List ";
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                if (mq0 == "DOJ")
                {
                    if (mq2 == "Y")
                    {
                        SQuery = "SELECT '" + header_n + "' as header,  A.EMPCODE,A.NAME,A.FHNAME,A.DESG_TEXT,A.DTJOIN,B.NAME AS PLANT,A.LEAVING_DT,A.DEPTT_TEXT FROM EMPMAS A ,TYPE B WHERE TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B'  AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' ORDER BY A.NAME";
                    }
                    else
                    {
                        SQuery = "SELECT '" + header_n + "' as header,  A.EMPCODE,A.NAME,A.FHNAME,A.DESG_TEXT,A.DTJOIN,B.NAME AS PLANT,A.LEAVING_DT,A.DEPTT_TEXT FROM EMPMAS A ,TYPE B WHERE TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B'  AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' AND NVL(TRIM(A.LEAVING_DT),'-')='-' ORDER BY A.NAME";
                    }
                }
                else
                {
                    if (mq2 == "Y")
                    {
                        SQuery = "SELECT '" + header_n + "' as header,A.EMPCODE,A.NAME,A.FHNAME,A.DESG_TEXT,A.DTJOIN,B.NAME AS PLANT,A.LEAVING_DT,A.DEPTT_TEXT FROM EMPMAS A ,TYPE B WHERE TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B'  AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' and a.DTJOIN " + xprdRange + " ORDER BY A.NAME";
                    }
                    else
                    {
                        SQuery = "SELECT '" + header_n + "' as header,A.EMPCODE,A.NAME,A.FHNAME,A.DESG_TEXT,A.DTJOIN,B.NAME AS PLANT,A.LEAVING_DT,A.DEPTT_TEXT FROM EMPMAS A ,TYPE B WHERE TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B'  AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' and a.DTJOIN " + xprdRange + " AND NVL(TRIM(A.LEAVING_DT),'-')='-' ORDER BY A.NAME";
                    }
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Join", "std_Join", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85148":
                #region birth list
                header_n = " Date of Birth Register";
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT '" + header_n + "' as header,A.EMPCODE,B.NAME AS PLANT ,A.NAME,A.DEPTT_TEXT,A.DESG_TEXT,A.D_O_B AS BIRTH_DATE FROM EMPMAS A ,TYPE B WHERE TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq0 + "' and nvl(trim(a.leaving_dt),'-')='-' ORDER BY A.EMPCODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Bday_List", "std_Bday_List", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85149":
                #region join/date of leaving reg
                header_n = " Date of Join/Date of Leaving Register (Sort on PF)";
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT '" + header_n + "' as header, branchcd,empcode,name,fhname,dtjoin,pfno,leaving_dt , leaving_why,conf_dt from empmas where  branchcd='" + frm_mbr + "' and grade ='" + mq0 + "' order by pfno  asc ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Join_Leaving_Register", "std_Join_Leaving_Register", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85150":
                #region  Attendence Register
                //Monthly attendence sheet by Akshay dt.09/03/2018
                header_n = "Attendence Register";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                SQuery = "SELECT '" + mq3 + " " + frm_myear + "' as monthname,'" + header_n + "' as header,A.EMPCODE,A.EL,A.CL,A.SL,A.OFFDAYS,B.NAME,B.FHNAME,a.present,a.totdays,a.absent FROM PAY A , EMPMAS B WHERE TRIM (A.EMPCODE) = TRIM (B.EMPCODE)  AND  TRIM(A.BRANCHCD) =TRIM(B.BRANCHCD) AND TRIM(A.GRADE) =TRIM(B.GRADE) AND A.BRANCHCD='" + frm_mbr + "' AND TO_CHAR(A.DATE_,'MM/YYYY')='" + mq2 + "/" + frm_myear + "' AND A.GRADE='" + mq1 + "' ORDER BY A.EMPCODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Attendence_Register", "std_Attendence_Register", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82527":
                #region FOR ALL CLIENTS Pay Slip
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("HEADER", typeof(string));
                ph_tbl.Columns.Add("monthname", typeof(string));
                ph_tbl.Columns.Add("EMPCODE", typeof(string));
                ph_tbl.Columns.Add("EMPNAME", typeof(string));
                ph_tbl.Columns.Add("FNAME", typeof(string));
                ph_tbl.Columns.Add("SPOUSE", typeof(string));
                ph_tbl.Columns.Add("DESGN", typeof(string));
                ph_tbl.Columns.Add("DEPTT", typeof(string));
                ph_tbl.Columns.Add("PFNO", typeof(string));
                ph_tbl.Columns.Add("UINNO", typeof(string));
                ph_tbl.Columns.Add("EL", typeof(double));
                ph_tbl.Columns.Add("SL", typeof(double));
                ph_tbl.Columns.Add("CL", typeof(double));
                ph_tbl.Columns.Add("DAYS_PAID", typeof(double));
                ph_tbl.Columns.Add("PRESENT", typeof(double));
                ph_tbl.Columns.Add("OFF_DAYS", typeof(double));
                ph_tbl.Columns.Add("ABS", typeof(string));
                ph_tbl.Columns.Add("TOT_SAL", typeof(double));
                ph_tbl.Columns.Add("DEDUCTION", typeof(double));
                ph_tbl.Columns.Add("NET_SAL", typeof(double));
                ph_tbl.Columns.Add("TDS", typeof(double));
                ph_tbl.Columns.Add("WRKHRS", typeof(double));
                ph_tbl.Columns.Add("HOLIDAYS", typeof(double));
                ph_tbl.Columns.Add("ESI", typeof(string));
                ph_tbl.Columns.Add("DTJOIN", typeof(string));
                ph_tbl.Columns.Add("ER1", typeof(double));
                ph_tbl.Columns.Add("ER2", typeof(double));
                ph_tbl.Columns.Add("ER3", typeof(double));
                ph_tbl.Columns.Add("ER4", typeof(double));
                ph_tbl.Columns.Add("ER5", typeof(double));
                ph_tbl.Columns.Add("ER6", typeof(double));
                ph_tbl.Columns.Add("ER7", typeof(double));
                ph_tbl.Columns.Add("ER8", typeof(double));
                ph_tbl.Columns.Add("ER9", typeof(double));
                ph_tbl.Columns.Add("ER10", typeof(double));
                ph_tbl.Columns.Add("AR1", typeof(double));
                ph_tbl.Columns.Add("AR2", typeof(double));
                ph_tbl.Columns.Add("AR3", typeof(double));
                ph_tbl.Columns.Add("AR4", typeof(double));
                ph_tbl.Columns.Add("AR5", typeof(double));
                ph_tbl.Columns.Add("AR6", typeof(double));
                ph_tbl.Columns.Add("AR7", typeof(double));
                ph_tbl.Columns.Add("AR8", typeof(double));
                ph_tbl.Columns.Add("AR9", typeof(double));
                ph_tbl.Columns.Add("AR10", typeof(double));
                ph_tbl.Columns.Add("OT_HRS", typeof(double));
                ph_tbl.Columns.Add("SPL_OT_HRS", typeof(double));

                header_n = "Pay Slip";
                dt = new DataTable(); dt1 = new DataTable();
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                mq4 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5"); //empcode
                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

               mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
               if (mq0 == "Y")
               {
                   mq8 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                   SQuery = "select '" + mq3 + " " + frm_myear + "' as monthname,'" + header_n + "' as header,empcode,grade,branchcd,to_char(date_,'dd/mm/yyyy') as date_,nvl(totdays,0) as totdays,nvl(absent,0) as absent,nvl(present,0) as present,nvl(shl,0) as holidays,((nvl(TOTDAYS,0)-nvl(ABSENT,0))-nvl(PRESENT,0)) AS OFF_DAYS,nvl(cl,0) as cl,nvl(el,0) as el,nvl(sl,0) as sl,nvl(er1,0) as er1,nvl(er2,0) as er2,nvl(er3,0) as er3,nvl(er4,0) as er4,nvl(er5,0) as er5,nvl(er6,0) as er6,nvl(er7,0) as er7,nvl(er8,0) as er8,nvl(er9,0) as er9,nvl(er10,0) as er10,nvl(erate1,0) as erate1,nvl(erate2,0) as erate2,nvl(erate3,0) as erate3,nvl(erate4,0) as erate4,nvl(erate5,0) as erate5,nvl(erate6,0) as erate6,nvl(erate7,0) as erate7,nvl(erate8,0) as erate8,nvl(erate9,0) as erate9,nvl(erate10,0) as erate10,nvl(ded1,0) as ded1,nvl(ded2,0) as ded2,nvl(ded3,0) ded3,nvl(ded4,0) as ded4,nvl(ded5,0) as ded5,nvl(ded6,0) as ded6,nvl(ded7,0) as ded7,nvl(ded8,0) as ded8,nvl(ded9,0) as ded9,nvl(ded10,0) as ded10,nvl(totern,0) as totern,nvl(totded,0) as totded,nvl(netslry,0) as netslry,nvl(totsal,0) as totsal,nvl(tds,0) as tds,nvl(wrkhrs,0) as wrkhrs,nvl(ar1,0) as ar1,nvl(ar2,0) as ar2,nvl(ar3,0) as ar3,nvl(ar4,0) as ar4,nvl(ar5,0) as ar5,nvl(ar6,0) as ar6,nvl(ar7,0) as ar7,nvl(ar8,0) as ar8,nvl(ar9,0) as ar9,nvl(ar10,0) as ar10,nvl(hours,0) as hours,nvl(hours2,0) as hours2 from pay where branch_act='" + mq8 + "' and grade='" + mq1 + "' and empcode='" + mq4 + "' and TO_CHAR(DATE_,'MM/YYYY')='" + mq2 + "/" + frm_myear + "' and type='10'";
                   dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//maindt ....paydt

                   mq5 = ""; mq6 = "";
                   mq6 = "select name,empcode,d_o_b,wrkhour,deptt_text,desg_text,fhname,pfno,addr1,addr2,city,country,married,uinno,adharno,esino,to_char(dtjoin,'dd/mm/yyyy') as dtjoin from empmas where empcode='" + mq4 + "' and grade='" + mq1 + "' and branch_act='" + mq8 + "'";
                   dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6); //empmas dt
               }
               else
               {
                   SQuery = "select '" + mq3 + " " + frm_myear + "' as monthname,'" + header_n + "' as header,empcode,grade,branchcd,to_char(date_,'dd/mm/yyyy') as date_,nvl(totdays,0) as totdays,nvl(absent,0) as absent,nvl(present,0) as present,nvl(shl,0) as holidays,((nvl(TOTDAYS,0)-nvl(ABSENT,0))-nvl(PRESENT,0)) AS OFF_DAYS,nvl(cl,0) as cl,nvl(el,0) as el,nvl(sl,0) as sl,nvl(er1,0) as er1,nvl(er2,0) as er2,nvl(er3,0) as er3,nvl(er4,0) as er4,nvl(er5,0) as er5,nvl(er6,0) as er6,nvl(er7,0) as er7,nvl(er8,0) as er8,nvl(er9,0) as er9,nvl(er10,0) as er10,nvl(erate1,0) as erate1,nvl(erate2,0) as erate2,nvl(erate3,0) as erate3,nvl(erate4,0) as erate4,nvl(erate5,0) as erate5,nvl(erate6,0) as erate6,nvl(erate7,0) as erate7,nvl(erate8,0) as erate8,nvl(erate9,0) as erate9,nvl(erate10,0) as erate10,nvl(ded1,0) as ded1,nvl(ded2,0) as ded2,nvl(ded3,0) ded3,nvl(ded4,0) as ded4,nvl(ded5,0) as ded5,nvl(ded6,0) as ded6,nvl(ded7,0) as ded7,nvl(ded8,0) as ded8,nvl(ded9,0) as ded9,nvl(ded10,0) as ded10,nvl(totern,0) as totern,nvl(totded,0) as totded,nvl(netslry,0) as netslry,nvl(totsal,0) as totsal,nvl(tds,0) as tds,nvl(wrkhrs,0) as wrkhrs,nvl(ar1,0) as ar1,nvl(ar2,0) as ar2,nvl(ar3,0) as ar3,nvl(ar4,0) as ar4,nvl(ar5,0) as ar5,nvl(ar6,0) as ar6,nvl(ar7,0) as ar7,nvl(ar8,0) as ar8,nvl(ar9,0) as ar9,nvl(ar10,0) as ar10,nvl(hours,0) as hours,nvl(hours2,0) as hours2 from pay where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and empcode='" + mq4 + "' and TO_CHAR(DATE_,'MM/YYYY')='" + mq2 + "/" + frm_myear + "' and type='10'";
                   dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//maindt ....paydt

                   mq5 = ""; mq6 = "";
                   mq6 = "select name,empcode,d_o_b,wrkhour,deptt_text,desg_text,fhname,pfno,addr1,addr2,city,country,married,uinno,adharno,esino,to_char(dtjoin,'dd/mm/yyyy') as dtjoin from empmas where empcode='" + mq4 + "' and grade='" + mq1 + "' and branchcd='" + frm_mbr + "'";
                   dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6); //empmas dt
               }
                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,10) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                mq7 = "select distinct ed_fld,substr(trim(ed_name),1,10) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'DED%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq7);

                if (dt.Rows.Count > 0)
                {
                    #region
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["monthname"] = dt.Rows[0]["monthname"].ToString().Trim();
                    dr1["EMPCODE"] = dt.Rows[0]["empcode"].ToString().Trim();
                    dr1["abs"] = dt.Rows[0]["absent"].ToString().Trim();
                    dr1["EMPNAME"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "name");
                    dr1["FNAME"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "fhname");
                    dr1["spouse"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "married");
                    dr1["desgn"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "desg_text");
                    dr1["deptt"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "deptt_text");
                    dr1["pfno"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "pfno");
                    dr1["uinno"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "uinno");
                    dr1["esi"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "esino");
                    dr1["dtjoin"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "dtjoin");
                    dr1["EL"] = dt.Rows[0]["el"].ToString().Trim();
                    dr1["sl"] = dt.Rows[0]["sl"].ToString().Trim();
                    dr1["cl"] = dt.Rows[0]["cl"].ToString().Trim();
                    dr1["DAYS_PAID"] = dt.Rows[0]["totdays"].ToString().Trim();
                    dr1["PRESENT"] = dt.Rows[0]["present"].ToString().Trim();
                    dr1["OFF_DAYS"] = dt.Rows[0]["holidays"].ToString().Trim();
                    dr1["tot_sal"] = dt.Rows[0]["totsal"].ToString().Trim();
                    dr1["deduction"] = dt.Rows[0]["totded"].ToString().Trim();
                    dr1["net_sal"] = dt.Rows[0]["netslry"].ToString().Trim();
                    dr1["tds"] = dt.Rows[0]["tds"].ToString().Trim();
                    dr1["wrkhrs"] = dt.Rows[0]["wrkhrs"].ToString().Trim();//
                    dr1["holidays"] = dt.Rows[0]["holidays"].ToString().Trim();
                    dr1["ot_hrs"] = dt.Rows[0]["hours"].ToString().Trim();
                    dr1["spl_ot_hrs"] = dt.Rows[0]["hours2"].ToString().Trim();

                    for (int i = 1; i < 11; i++)
                    {
                        ph_tbl.Columns.Add("ername_" + i + "", typeof(string));
                        dr1["ername_" + i + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + i + ""] + "' ", "ed_name");
                        ph_tbl.Columns.Add("dedname_" + i + "", typeof(string));
                        ph_tbl.Columns.Add("ded" + i + "", typeof(double));
                        dr1["dedname_" + i + ""] = fgen.seek_iname_dt(dt3, "ed_fld='" + ph_tbl.Columns["DED" + i + ""] + "' ", "ed_name");
                        ph_tbl.Columns.Add("erate" + i + "", typeof(double));
                        dr1["erate" + i + ""] = fgen.make_double(dt.Rows[0]["erate" + i + ""].ToString().Trim());
                        dr1["er" + i + ""] = fgen.make_double(dt.Rows[0]["er" + i + ""].ToString().Trim());
                        dr1["ded" + i + ""] = fgen.make_double(dt.Rows[0]["ded" + i + ""].ToString().Trim());
                        dr1["ar" + i + ""] = fgen.make_double(dt.Rows[0]["ar" + i + ""].ToString().Trim());
                    }
                    ph_tbl.Rows.Add(dr1);
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    //Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sal_Slip", "std_Sal_Slip", dsRep, header_n); //old
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sal_Slip_N", "std_Sal_Slip_N", dsRep, header_n);//new
                    #endregion
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82525":
                #region Pay Summary (Deptt Wise)
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("monthname", typeof(string));
                ph_tbl.Columns.Add("dept_Code", typeof(string));
                ph_tbl.Columns.Add("dept_name", typeof(string));
                ph_tbl.Columns.Add("tot_ern", typeof(double));
                ph_tbl.Columns.Add("tot_Ded", typeof(double));
                ph_tbl.Columns.Add("net_sal", typeof(double));
                ph_tbl.Columns.Add("EPF", typeof(double));
                //esi me ...totern ka 4.75 % but the coming value by this formula is not matched
                header_n = "Pay Summary (Deptt Wise)";
                dt = new DataTable(); dt1 = new DataTable();
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                mq4 = mq2 + "/" + frm_myear;
                SQuery = "select '" + mq4 + " " + frm_myear + "' as monthname,'" + header_n + "' as header,b.deptt, b.deptt_text as dept, a.grade,to_char(a.date_,'dd/mm/yyyy') as date_,sum(nvl(a.er1,0)) as er1,sum(nvl(a.er2,0)) as er2,sum(nvl(a.er3,0)) as er3,sum(nvl(a.er4,0)) as er4,sum(nvl(a.er5,0)) as er5,sum(nvl(a.er6,0)) as er6,sum(nvl(a.er7,0)) as er7,sum(nvl(a.er8,0)) as er8,sum(nvl(a.er9,0)) as er9,sum(nvl(a.er10,0)) as er10,sum(nvl(a.ded1,0)) as ded1,sum(nvl(a.ded2,0)) as ded2,sum(nvl(a.ded3,0)) ded3,sum(nvl(a.ded4,0)) as ded4,sum(nvl(a.ded5,0)) as ded5,sum(nvl(a.ded6,0)) as ded6,sum(nvl(a.ded7,0)) as ded7,sum(nvl(a.ded8,0)) as ded8,sum(nvl(a.ded9,0)) as ded9,sum(nvl(a.ded10,0)) as ded10,sum(nvl(a.totern,0)) as totern,sum(nvl(a.totded,0)) as totded,sum(nvl(a.netslry,0)) as netslry,sum(nvl(a.totsal,0)) as totsal from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq1 + "'  and TO_CHAR(a.DATE_,'MM/YYYY')='" + mq4 + "' and a.type='10' group by a.grade,to_char(a.date_,'dd/mm/yyyy'),b.deptt_text,b.deptt  order by dept";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//maindt ....paydt

                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                mq7 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'DED%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq7);

                if (dt.Rows.Count > 0)
                {
                    #region
                    //add column
                    for (int i = 1; i < 11; i++)
                    {
                        ph_tbl.Columns.Add("ername_" + i + "", typeof(string));
                        ph_tbl.Columns.Add("dedname_" + i + "", typeof(string));
                        ph_tbl.Columns.Add("ded" + i + "", typeof(double));
                        ph_tbl.Columns.Add("er" + i + "", typeof(double));
                    }
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["header"] = header_n;
                        dr1["monthname"] = mq3;
                        dr1["dept_Code"] = dt.Rows[j]["deptt"].ToString().Trim();
                        dr1["dept_name"] = dt.Rows[j]["dept"].ToString().Trim();
                        dr1["tot_ern"] = dt.Rows[j]["totern"].ToString().Trim();
                        dr1["tot_ded"] = dt.Rows[j]["totded"].ToString().Trim();
                        dr1["net_sal"] = dt.Rows[j]["netslry"].ToString().Trim();
                        for (int i = 1; i < 11; i++)
                        {
                            dr1["ername_" + i + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + i + ""] + "' ", "ed_name");
                            dr1["dedname_" + i + ""] = fgen.seek_iname_dt(dt3, "ed_fld='" + ph_tbl.Columns["DED" + i + ""] + "' ", "ed_name");
                            dr1["er" + i + ""] = fgen.make_double(dt.Rows[j]["er" + i + ""].ToString().Trim());
                            dr1["ded" + i + ""] = fgen.make_double(dt.Rows[j]["ded" + i + ""].ToString().Trim());
                        }
                        dr1["EPF"] = fgen.make_double(dt.Rows[j]["er1"].ToString().Trim()) * 12 / 100; //12% of basic
                        ph_tbl.Rows.Add(dr1);
                    }
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pay_Summ_deptwise", "std_Pay_Summ_deptwise", dsRep, header_n);
                    #endregion
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82514":
                #region pay summary
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                header_n = "Pay Summary : " + mq3 + " ";
                dt = new DataTable();
                SQuery = "SELECT '" + header_n + "' AS HEADER, A.EMPCODE,B.NAME,B.FHNAME,B.DEPTT,B.DEPTT_TEXT,B.IFSC_CODE AS RMK,B.BNKACNO  AS BANK_aC , SUM(A.NETSLRY) AS NET  FROM PAY A,EMPMAS B  WHERE TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' AND TO_CHAR(DATE_,'MM/yyyy')='" + mq2 + "/" + frm_myear + "' GROUP BY A.EMPCODE,B.NAME,B.FHNAME,B.DEPTT,B.DEPTT_TEXT,B.IFSC_CODE,B.BNKACNO ORDER BY A.EMPCODE";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sal_Rqst", "std_Sal_Rqst", dsRep, header_n);
                }
                #endregion
                break;

            case "F82515":
                #region Quarterly Pay Register
                mq1 = fromdt.Substring(3, 2);
                int a = Convert.ToInt32(mq1) + 1;
                if (a < 10)
                {
                    mq2 = Convert.ToString("0" + a);
                }
                else
                {
                    mq2 = Convert.ToString(a);
                }
                mq7 = "";
                mq3 = todt.Substring(3, 2);
                if (Convert.ToInt32(mq3) > 3 && Convert.ToInt32(mq3) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                mq6 = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(trim(mthname),1,3) as mthname from mths where mthnum='" + mq1 + "'", "mthname"); //first mth name
                mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(trim(mthname),1,3) as mthname from mths where mthnum='" + mq2 + "'", "mthname"); //second mth name
                mq8 = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(trim(mthname),1,3) as mthname from mths where mthnum='" + mq3 + "'", "mthname"); //third
                mq9 = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='B' and type1='" + frm_mbr + "'", "NAME");
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                header_n = "Quarterly Pay Register " + mq8 + " " + frm_myear;
                dt = new DataTable();
                SQuery = "select '" + header_n + "' as header,'" + fromdt.Substring(0, 2) + " " + mq6 + " " + frm_myear + "' as year1,'" + todt.Substring(0, 2) + " " + mq8 + " " + frm_myear + "' as year2,'" + mq9 + "' as brnchname,a.EMPNAME,a.empcode as empcode,'" + mq6 + "' AS H1,'" + mq7 + "' AS H2,'" + mq8 + "' AS H3,sum(a." + mq6 + "+a." + mq7 + "+a." + mq8 + ") as total,sum(a." + mq6 + ") as F1,sum(a." + mq7 + ") as F2,sum(a." + mq8 + ") as F3 from (select trim(b.NAME) as EMPNAME,a.empcode,a.grade,decode(to_char(a.date_,'mm'),'" + mq1 + "',a.netslry,0) as " + mq6 + ",decode(to_char(a.date_,'mm'),'" + mq2 + "',a.netslry,0) as " + mq7 + ",decode(to_char(a.date_,'mm'),'" + mq3 + "',a.netslry,0) as " + mq8 + " from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) AND A.BRANCHCD='" + frm_mbr + "' and  a.date_ " + xprdRange + " and a.grade='" + mq5 + "') a group by a.EMPNAME,a.empcode order by empcode";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Qtrly_Pay_Reg", "std_Qtrly_Pay_Reg", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82521":
                #region Combined Pay Summary
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("monthname", typeof(string));
                // ph_tbl.Columns.Add("dept_Code", typeof(string));
                ph_tbl.Columns.Add("dept_name", typeof(string));
                ph_tbl.Columns.Add("tot_ern", typeof(double));
                ph_tbl.Columns.Add("tot_Ded", typeof(double));
                ph_tbl.Columns.Add("net_sal", typeof(double));
                ph_tbl.Columns.Add("EPF", typeof(double));
                header_n = "Combined Pay Summary";
                dt = new DataTable(); dt1 = new DataTable();
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//mbr
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//grade
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//month val
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//mth name
                mq6 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                mq7 = mq6 + "/" + frm_myear;
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='B' AND TYPE1='" + frm_mbr + "'", "NAME");
                SQuery = "select '" + mq3 + " " + frm_myear + "' as monthname,'" + header_n + "' as header,'" + mq4 + "' AS BRANCHNAME ,a.grade,to_char(a.date_,'dd/mm/yyyy') as date_,sum(nvl(a.er1,0)) as er1,sum(nvl(a.er2,0)) as er2,sum(nvl(a.er3,0)) as er3,sum(nvl(a.er4,0)) as er4,sum(nvl(a.er5,0)) as er5,sum(nvl(a.er6,0)) as er6,sum(nvl(a.er7,0)) as er7,sum(nvl(a.er8,0)) as er8,sum(nvl(a.er9,0)) as er9,sum(nvl(a.er10,0)) as er10,sum(nvl(a.ded1,0)) as ded1,sum(nvl(a.ded2,0)) as ded2,sum(nvl(a.ded3,0)) ded3,sum(nvl(a.ded4,0)) as ded4,sum(nvl(a.ded5,0)) as ded5,sum(nvl(a.ded6,0)) as ded6,sum(nvl(a.ded7,0)) as ded7,sum(nvl(a.ded8,0)) as ded8,sum(nvl(a.ded9,0)) as ded9,sum(nvl(a.ded10,0)) as ded10,sum(nvl(a.totern,0)) as totern,sum(nvl(a.totded,0)) as totded,sum(nvl(a.netslry,0)) as netslry,sum(nvl(a.totsal,0)) as totsal from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd in (" + mq0 + ") and a.grade='" + mq1 + "' and TO_CHAR(a.DATE_,'MM/YYYY')='" + mq7 + "' and a.type='10' group by a.grade,to_char(a.date_,'dd/mm/yyyy')";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//maindt ....paydt

                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                mq7 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'DED%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq7);

                if (dt.Rows.Count > 0)
                {
                    #region
                    //add column
                    for (int i = 1; i < 11; i++)
                    {
                        ph_tbl.Columns.Add("ername_" + i + "", typeof(string));
                        ph_tbl.Columns.Add("dedname_" + i + "", typeof(string));
                        ph_tbl.Columns.Add("ded" + i + "", typeof(double));
                        ph_tbl.Columns.Add("er" + i + "", typeof(double));
                    }
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["header"] = header_n;
                        dr1["monthname"] = dt.Rows[j]["monthname"].ToString().Trim();
                        //   dr1["dept_Code"] = dt.Rows[j]["deptt"].ToString().Trim();
                        dr1["dept_name"] = dt.Rows[j]["BRANCHNAME"].ToString().Trim();
                        dr1["tot_ern"] = dt.Rows[j]["totern"].ToString().Trim();
                        dr1["tot_ded"] = dt.Rows[j]["totded"].ToString().Trim();
                        dr1["net_sal"] = dt.Rows[j]["netslry"].ToString().Trim();
                        for (int i = 1; i < 11; i++)
                        {
                            dr1["ername_" + i + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + i + ""] + "' ", "ed_name");
                            dr1["dedname_" + i + ""] = fgen.seek_iname_dt(dt3, "ed_fld='" + ph_tbl.Columns["DED" + i + ""] + "' ", "ed_name");
                            dr1["er" + i + ""] = fgen.make_double(dt.Rows[j]["er" + i + ""].ToString().Trim());
                            dr1["ded" + i + ""] = fgen.make_double(dt.Rows[j]["ded" + i + ""].ToString().Trim());
                        }
                        dr1["EPF"] = fgen.make_double(dt.Rows[j]["er1"].ToString().Trim()) * 12 / 100; //12% of basic
                        ph_tbl.Rows.Add(dr1);
                    }
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pay_Summ_Combined", "std_Pay_Summ_Combined", dsRep, header_n);
                }
                    #endregion
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82517":
                #region PAY TREND DEPT WISE
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (mq1 == "N")
                {
                    header_n = "Net Pay Trend Deptt Wise";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.deptt_text,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select b.deptt_text, a.grade,decode(to_char(a.date_,'mm'),'03',a.netslry,0) as mar,decode(to_char(a.date_,'mm'),'04',a.netslry,0) as apr,decode(to_char(a.date_,'mm'),'05',a.netslry,0) as may,decode(to_char(a.date_,'mm'),'06',a.netslry,0) as jun,decode(to_char(a.date_,'mm'),'07',a.netslry,0) as jul,decode(to_char(a.date_,'mm'),'08',a.netslry,0) as aug,decode(to_char(a.date_,'mm'),'09',a.netslry,0) as sept,decode(to_char(a.date_,'mm'),'10',a.netslry,0) as oct,decode(to_char(a.date_,'mm'),'11',a.netslry,0) as nov,decode(to_char(a.date_,'mm'),'12',a.netslry,0) as dec,decode(to_char(a.date_,'mm'),'01',a.netslry,0) as jan,decode(to_char(a.date_,'mm'),'02',a.netslry,0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq2 + "' and a.date_ " + xprdRange + ") a group by a.deptt_text";
                }
                else
                {
                    header_n = "Gross Pay Trend Deptt Wise";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.deptt_text,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select b.deptt_text, a.grade,decode(to_char(a.date_,'mm'),'03',a.totern,0) as mar,decode(to_char(a.date_,'mm'),'04',a.totern,0) as apr,decode(to_char(a.date_,'mm'),'05',a.totern,0) as may,decode(to_char(a.date_,'mm'),'06',a.totern,0) as jun,decode(to_char(a.date_,'mm'),'07',a.totern,0) as jul,decode(to_char(a.date_,'mm'),'08',a.totern,0) as aug,decode(to_char(a.date_,'mm'),'09',a.totern,0) as sept,decode(to_char(a.date_,'mm'),'10',a.totern,0) as oct,decode(to_char(a.date_,'mm'),'11',a.totern,0) as nov,decode(to_char(a.date_,'mm'),'12',a.totern,0) as dec,decode(to_char(a.date_,'mm'),'01',a.totern,0) as jan,decode(to_char(a.date_,'mm'),'02',a.totern,0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq2 + "' and a.date_ " + xprdRange + " ) a group by a.deptt_text";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Grs_Pay_trend_deptwise", "std_Grs_Pay_trend_deptwise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82523"://GROSS PAY TREND DEPT WSIE
                //code field is pending in report
                #region Payroll Cost trend dept/desg wise
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (mq1 == "Y")
                {
                    header_n = "Gross Pay Trend Deptt/Desg Wise";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.deptt_text,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select trim(b.deptt_Text)||' / '||trim(b.desg_text) as deptt_text, a.grade,decode(to_char(a.date_,'mm'),'03',a.totern,0) as mar,decode(to_char(a.date_,'mm'),'04',a.totern,0) as apr,decode(to_char(a.date_,'mm'),'05',a.totern,0) as may,decode(to_char(a.date_,'mm'),'06',a.totern,0) as jun,decode(to_char(a.date_,'mm'),'07',a.totern,0) as jul,decode(to_char(a.date_,'mm'),'08',a.totern,0) as aug,decode(to_char(a.date_,'mm'),'09',a.totern,0) as sept,decode(to_char(a.date_,'mm'),'10',a.totern,0) as oct,decode(to_char(a.date_,'mm'),'11',a.totern,0) as nov,decode(to_char(a.date_,'mm'),'12',a.totern,0) as dec,decode(to_char(a.date_,'mm'),'01',a.totern,0) as jan,decode(to_char(a.date_,'mm'),'02',a.totern,0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq2 + "' and a.date_ " + xprdRange + " ) a group by a.deptt_text order by a.deptt_text";
                }
                else
                {
                    header_n = "Net Pay Trend Deptt/Desg Wise";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.deptt_text,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select trim(b.deptt_Text)||' / '||trim(b.desg_text) as deptt_text, a.grade,decode(to_char(a.date_,'mm'),'03',a.netslry,0) as mar,decode(to_char(a.date_,'mm'),'04',a.netslry,0) as apr,decode(to_char(a.date_,'mm'),'05',a.netslry,0) as may,decode(to_char(a.date_,'mm'),'06',a.netslry,0) as jun,decode(to_char(a.date_,'mm'),'07',a.netslry,0) as jul,decode(to_char(a.date_,'mm'),'08',a.netslry,0) as aug,decode(to_char(a.date_,'mm'),'09',a.netslry,0) as sept,decode(to_char(a.date_,'mm'),'10',a.netslry,0) as oct,decode(to_char(a.date_,'mm'),'11',a.netslry,0) as nov,decode(to_char(a.date_,'mm'),'12',a.netslry,0) as dec,decode(to_char(a.date_,'mm'),'01',a.netslry,0) as jan,decode(to_char(a.date_,'mm'),'02',a.netslry,0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq2 + "' and a.date_ " + xprdRange + ") a group by a.deptt_text order by a.deptt_text";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Grs_Pay_trend_deptwise", "std_Grs_Pay_trend_deptwise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82519"://SALARY RATE REPORT
                #region
                header_n = "Salary Rate Report";//this is gross salary paid report also........both are same
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("monthname", typeof(string));
                ph_tbl.Columns.Add("EMPCODE", typeof(string));
                ph_tbl.Columns.Add("EMPNAME", typeof(string));
                ph_tbl.Columns.Add("FNAME", typeof(string));
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");

                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                SQuery = "select '" + mq3 + " " + frm_myear + "' as monthname,'" + header_n + "' as header,empcode,grade,branchcd,to_char(date_,'dd/mm/yyyy') as date_,nvl(totdays,0) as totdays,nvl(absent,0) as absent,nvl(present,0) as present,nvl(shl,0) as holidays,((nvl(TOTDAYS,0)-nvl(ABSENT,0))-nvl(PRESENT,0)) AS OFF_DAYS,nvl(cl,0) as cl,nvl(el,0) as el,nvl(sl,0) as sl,nvl(er1,0) as er1,nvl(er2,0) as er2,nvl(er3,0) as er3,nvl(er4,0) as er4,nvl(er5,0) as er5,nvl(er6,0) as er6,nvl(er7,0) as er7,nvl(er8,0) as er8,nvl(er9,0) as er9,nvl(er10,0) as er10,nvl(erate1,0) as erate1,nvl(erate2,0) as erate2,nvl(erate3,0) as erate3,nvl(erate4,0) as erate4,nvl(erate5,0) as erate5,nvl(erate6,0) as erate6,nvl(erate7,0) as erate7,nvl(erate8,0) as erate8,nvl(erate9,0) as erate9,nvl(erate10,0) as erate10,nvl(ded1,0) as ded1,nvl(ded2,0) as ded2,nvl(ded3,0) ded3,nvl(ded4,0) as ded4,nvl(ded5,0) as ded5,nvl(ded6,0) as ded6,nvl(ded7,0) as ded7,nvl(ded8,0) as ded8,nvl(ded9,0) as ded9,nvl(ded10,0) as ded10,nvl(totern,0) as totern,nvl(totded,0) as totded,nvl(netslry,0) as netslry,nvl(totsal,0) as totsal,nvl(tds,0) as tds,nvl(wrkhrs,0) as wrkhrs from pay where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and TO_CHAR(DATE_,'MM/YYYY')='" + mq2 + "/" + frm_myear + "' and type='10' order by empcode";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//maindt ....paydt

                mq6 = "select name,empcode,deptt_text,desg_text,fhname from empmas where grade='" + mq1 + "' and branchcd='" + frm_mbr + "'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6); //empmas dt       

                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i < 11; i++)
                    {
                        dr1 = ph_tbl.NewRow();
                        ph_tbl.Columns.Add("er" + i + "", typeof(double));
                        ph_tbl.Columns.Add("ername_" + i + "", typeof(string));
                    }
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["header"] = header_n;
                        dr1["monthname"] = dt.Rows[j]["monthname"].ToString().Trim();
                        dr1["EMPCODE"] = dt.Rows[j]["empcode"].ToString().Trim();
                        dr1["EMPNAME"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "name");
                        dr1["FNAME"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "fhname");
                        for (int i = 1; i < 11; i++)
                        {
                            dr1["er" + i + ""] = fgen.make_double(dt.Rows[j]["erate" + i + ""].ToString().Trim());
                            dr1["ername_" + i + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + i + ""] + "' ", "ed_name");
                        }
                        ph_tbl.Rows.Add(dr1);
                    }
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_pay_rate", "std_pay_rate", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85142":
            case "F85106":
            case "F85107":
                //SALARY REGISTER..........MADE BY AKSHAY...merged by yogita on 8jan2019 and make a 10 header rpt by yogita               
                #region Pay Register
                mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                header_n = "Pay Register";
                if (frm_cocd == "HGLO" && hfhcid.Value.Trim() == "F85107")
                {
                    mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); // VCHNUM & VCHDATE 
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"); // GRADE
                    SQuery = "SELECT '" + header_n + "' AS HEADER,A.BRANCHCD, TO_CHAR(A.DATE_,'MONTH/YYYY') AS DATE_ ,A.GRADE,B.EMPCODE,B.NAME,B.FHNAME,B.DESG_TEXT,B.DEPTT_TEXT,SUBSTR(B.PFNO,1,10) AS PFNO ,B.ESINO,A.ERATE1 AS RATE1,A.ERATE2 AS RATE2 ,A.ERATE3 AS RATE3 ,A.ERATE4 AS RATE4 ,A.ERATE5 AS RATE5,A.ERATE6 AS RATE6,A.ERATE7 AS RATE7,A.ERATE8 AS RATE8 ,A.ERATE9 AS RATE9 ,A.ERATE10 AS RATE10,B.UINNO,A.PRESENT,(A.TOTDAYS-A.ABSENT) AS DAYS_PAID, ((A.TOTDAYS-A.ABSENT)-A.PRESENT) AS OFF_DAYS,A.ER1 AS EARNING1,A.ER2 AS EARNING2,A.ER3 AS EARNING3,A.ER4 AS EARNING4,A.ER5 AS EARNING5,A.ER6 AS EARNING6,A.ER7 AS EARNING7,A.ER8 AS EARNING8,A.ER9 AS EARNING9,A.ER10 AS EARNING10,A.DED1,A.DED2,A.DED3,A.DED4,A.DED5,A.DED6,A.DED7,A.DED8,A.DED9,A.DED10,A.OT,A.TOTERN,A.TOTDED,A.NETSLRY,A.AR1,A.AR2,A.AR3,A.AR4,A.AR5,A.AR6,A.AR7,A.AR8,A.AR9,A.AR10 FROM PAY A,EMPMAS B WHERE TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.GRADE)=TRIM(B.GRADE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='10' AND A.GRADE='" + mq1 + "' AND trim(a.mastvch)='" + mq0 + "' ORDER BY EMPCODE";
                }
                else
                {
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"); // GRADE
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"); // MONTH
                    //mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4"); // MONTH NAME
                    //if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                    //{

                    //}
                    //else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    mq4 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
                    if (mq4 == "Y")
                    {
                        mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                        SQuery = "SELECT '" + header_n + "' AS HEADER,A.BRANCHCD, TO_CHAR(A.DATE_,'MONTH/YYYY') AS DATE_ ,A.GRADE,B.EMPCODE,B.NAME,B.FHNAME,B.DESG_TEXT,B.DEPTT_TEXT,SUBSTR(B.PFNO,1,10) AS PFNO ,B.ESINO,A.ERATE1 AS RATE1,A.ERATE2 AS RATE2 ,A.ERATE3 AS RATE3 ,A.ERATE4 AS RATE4 ,A.ERATE5 AS RATE5,A.ERATE6 AS RATE6,A.ERATE7 AS RATE7,A.ERATE8 AS RATE8 ,A.ERATE9 AS RATE9 ,A.ERATE10 AS RATE10,B.UINNO,A.PRESENT,(A.TOTDAYS-A.ABSENT) AS DAYS_PAID, ((A.TOTDAYS-A.ABSENT)-A.PRESENT) AS OFF_DAYS,A.ER1 AS EARNING1,A.ER2 AS EARNING2,A.ER3 AS EARNING3,A.ER4 AS EARNING4,A.ER5 AS EARNING5,A.ER6 AS EARNING6,A.ER7 AS EARNING7,A.ER8 AS EARNING8,A.ER9 AS EARNING9,A.ER10 AS EARNING10,A.DED1,A.DED2,A.DED3,A.DED4,A.DED5,A.DED6,A.DED7,A.DED8,A.DED9,A.DED10,A.OT,A.TOTERN,A.TOTDED,A.NETSLRY,A.AR1,A.AR2,A.AR3,A.AR4,A.AR5,A.AR6,A.AR7,A.AR8,A.AR9,A.AR10,hours,hours2 FROM PAY A,EMPMAS B WHERE TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCH_ACT) AND TRIM(A.GRADE)=TRIM(B.GRADE) AND A.BRANCH_ACT='" + frm_mbr + "' AND A.TYPE='10' AND A.GRADE='" + mq1 + "' AND TO_CHAR(A.DATE_,'MM/YYYY')='" + mq2 + "' ORDER BY EMPCODE";
                    }
                    else
                    {
                        SQuery = "SELECT '" + header_n + "' AS HEADER,A.BRANCHCD, TO_CHAR(A.DATE_,'MONTH/YYYY') AS DATE_ ,A.GRADE,B.EMPCODE,B.NAME,B.FHNAME,B.DESG_TEXT,B.DEPTT_TEXT,SUBSTR(B.PFNO,1,10) AS PFNO ,B.ESINO,A.ERATE1 AS RATE1,A.ERATE2 AS RATE2 ,A.ERATE3 AS RATE3 ,A.ERATE4 AS RATE4 ,A.ERATE5 AS RATE5,A.ERATE6 AS RATE6,A.ERATE7 AS RATE7,A.ERATE8 AS RATE8 ,A.ERATE9 AS RATE9 ,A.ERATE10 AS RATE10,B.UINNO,A.PRESENT,(A.TOTDAYS-A.ABSENT) AS DAYS_PAID, ((A.TOTDAYS-A.ABSENT)-A.PRESENT) AS OFF_DAYS,A.ER1 AS EARNING1,A.ER2 AS EARNING2,A.ER3 AS EARNING3,A.ER4 AS EARNING4,A.ER5 AS EARNING5,A.ER6 AS EARNING6,A.ER7 AS EARNING7,A.ER8 AS EARNING8,A.ER9 AS EARNING9,A.ER10 AS EARNING10,A.DED1,A.DED2,A.DED3,A.DED4,A.DED5,A.DED6,A.DED7,A.DED8,A.DED9,A.DED10,A.OT,A.TOTERN,A.TOTDED,A.NETSLRY,A.AR1,A.AR2,A.AR3,A.AR4,A.AR5,A.AR6,A.AR7,A.AR8,A.AR9,A.AR10,hours,hours2 FROM PAY A,EMPMAS B WHERE TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.GRADE)=TRIM(B.GRADE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='10' AND A.GRADE='" + mq1 + "' AND TO_CHAR(A.DATE_,'MM/YYYY')='" + mq2 + "' ORDER BY EMPCODE";
                    }
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.TableName = "Prepcur";
                dsRep.Tables.Add(dt);
                if (dt.Rows.Count > 0)
                {
                    dt2 = new DataTable();
                    dt2.Columns.Add("enam1", typeof(string));
                    dt2.Columns.Add("enam2", typeof(string));
                    dt2.Columns.Add("enam3", typeof(string));
                    dt2.Columns.Add("enam4", typeof(string));
                    dt2.Columns.Add("enam5", typeof(string));
                    dt2.Columns.Add("enam6", typeof(string));
                    dt2.Columns.Add("enam7", typeof(string));
                    dt2.Columns.Add("enam8", typeof(string));
                    dt2.Columns.Add("enam9", typeof(string));
                    dt2.Columns.Add("enam10", typeof(string));
                    dt2.Columns.Add("dnam1", typeof(string));
                    dt2.Columns.Add("dnam2", typeof(string));
                    dt2.Columns.Add("dnam3", typeof(string));
                    dt2.Columns.Add("dnam4", typeof(string));
                    dt2.Columns.Add("dnam5", typeof(string));
                    dt2.Columns.Add("dnam6", typeof(string));
                    dt2.Columns.Add("dnam7", typeof(string));
                    dt2.Columns.Add("dnam8", typeof(string));
                    dt2.Columns.Add("dnam9", typeof(string));
                    dt2.Columns.Add("dnam10", typeof(string));

                    mq0 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    mq2 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'DED%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq2);

                    dr1 = null;
                    dr1 = dt2.NewRow();
                    int k = 1;
                    for (int i = 1; i < 11; i++)
                    {
                        mq5 = fgen.seek_iname_dt(dt1, "ed_fld='ER" + i + "'", "ed_name");
                        if (mq5.Length == 1)
                        {
                            mq5 = "-";
                        }
                        dr1["enam" + k] = mq5;

                        mq6 = fgen.seek_iname_dt(dt3, "ed_fld='DED" + i + "'", "ed_name");
                        if (mq6.Length == 1)
                        {
                            mq6 = "-";
                        }
                        dr1["dnam" + k] = mq6;
                        k++;
                    }
                    dt2.Rows.Add(dr1);
                    dt2.TableName = "Heading";
                    dsRep.Tables.Add(dt2);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Pay_Register", "Pay_Register", dsRep, header_n);
                }
                #endregion
                break;

            case "F82512":
                #region
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //grade
                header_n = "Mobile Phone Book";
                SQuery = "select  distinct '" + header_n + "' as header,A.BRANCHCD,SUBSTR(TRIM(B.NAME),1,10) AS  PLANT, A.empcode,A.name,A.fhname,A.deptt_text,A.desg_text,A.mobile from empmas A ,TYPE B where TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq1 + "'  AND  a.leaving_Dt='-' order by A.empcode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mobile_list", "std_mobile_list", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82530":
                #region List of Addresses
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //grade
                header_n = "List of Addresses";
                SQuery = "select distinct '" + header_n + "' as header, A.BRANCHCD,SUBSTR(TRIM(B.NAME),1,10) AS  PLANT, A.empcode,A.name,A.fhname,A.addr1,a.addr2,a.city,a.state ,A.GRADE from empmas A ,TYPE B where TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq1 + "' and nvl(trim(leaving_dt),'-')='-' order by a.name";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Add_List", "std_Add_List", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82501":
                #region
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //grade
                header_n = "List of Blood Groups";
                SQuery = "select distinct '" + header_n + "' as header, A.BRANCHCD,SUBSTR(TRIM(B.NAME),1,10) AS  PLANT, A.empcode,A.name,A.fhname,a.desg_text,a.deptt_text,(case when nvl(a.bloodgrp,'-')='-' then '-' else nvl(a.bloodgrp,'-') end) as bloodgrp from empmas A ,TYPE B where TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq1 + "' AND  a.leaving_Dt='-' order by bloodgrp desc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_blood_grp_list", "std_blood_grp_list", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82510":
                #region
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //grade
                header_n = "Land Line Phone Book";
                SQuery = "select distinct '" + header_n + "' as header,A.BRANCHCD,SUBSTR(TRIM(B.NAME),1,15) AS  PLANT, A.empcode,A.name,A.fhname,a.desg_text,a.deptt_text,a.phone from empmas A ,TYPE B where TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq1 + "' AND  a.leaving_Dt='-' order by a.empcode ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Landline_List", "std_Landline_List", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82505":
                #region
                mq1 = ""; mq2 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9"); //grade
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL10"); //empcode
                header_n = "Identity Card";
                SQuery = "select distinct '" + header_n + "' as header, A.empcode,A.name,A.fhname,a.desg_text,a.deptt_text,a.empimg from empmas A where A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq1 + "' and a.empcode in (" + mq2 + ") order by a.empcode desc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("mLogo", typeof(System.Byte[]));
                    foreach (DataRow dr in dt.Rows)
                    {
                        mq5 = "";
                        mq5 = dr["empimg"].ToString().Trim();
                        // if (dt.Rows[0]["empimg"].ToString().Trim() != "" || dt.Rows[0]["empimg"].ToString().Trim() != "-" || dt.Rows[0]["empimg"].ToString().Trim() != " ")
                        if (mq5.Length > 1)
                        {
                            fpath = dr["empimg"].ToString().Trim();
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dr["mLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                    }
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Identity_Card", "std_Identity_Card", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82507":
                #region
                mq1 = ""; mq2 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9"); //grade
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL10"); //empcode
                header_n = "New Employee";
                SQuery = "select distinct '" + header_n + "' as header, A.empcode,A.name,A.fhname,a.desg_text,a.deptt_text,TO_CHAr(a.dtjoin,'dd/mm/yyyy') as dtjoin from empmas A where A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq1 + "' and a.empcode in (" + mq2 + ") order by a.empcode desc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Join_Card", "std_Join_Card", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82503":
                #region
                dt = new DataTable();
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq2) > 3 && Convert.ToInt32(mq2) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                header_n = "Category Wise Summary Report";
                SQuery = "select  distinct '" + header_n + "' as header,'" + mq3 + " " + frm_myear + "' as monthname,b.name as grade_name,a.grade,sum(a.totern) as earning,sum(a.totded) as deduction,sum(a.netslry) as netsal  from pay a,type b where trim(a.grade)=trim(b.type1) and b.id='I' and  a.branchcd='" + frm_mbr + "' and to_char(a.date_,'mm/yyyy')='" + mq2 + "/" + frm_myear + "' and a.grade='" + mq1 + "' group by a.grade,b.name";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Catg_Sumry", "std_Catg_Sumry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82516":
                #region pay trend print reprot section wise
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4"); //grade
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5"); //date
                dt = new DataTable();
                if (mq2 == "Y")
                {
                    header_n = "Gross Pay Trend Statement(Section Wise)";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'CODE' as code, a.section,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select trim(b.NAME) as EMPNAME,b.fpfnominee as section,a.grade,decode(to_char(a.date_,'mm'),'03',a.totern,0) as mar,decode(to_char(a.date_,'mm'),'04',a.totern,0) as apr,decode(to_char(a.date_,'mm'),'05',a.totern,0) as may,decode(to_char(a.date_,'mm'),'06',a.totern,0) as jun,decode(to_char(a.date_,'mm'),'07',a.totern,0) as jul,decode(to_char(a.date_,'mm'),'08',a.totern,0) as aug,decode(to_char(a.date_,'mm'),'09',a.totern,0) as sept,decode(to_char(a.date_,'mm'),'10',a.totern,0) as oct,decode(to_char(a.date_,'mm'),'11',a.totern,0) as nov,decode(to_char(a.date_,'mm'),'12',a.totern,0) as dec,decode(to_char(a.date_,'mm'),'01',a.totern,0) as jan,decode(to_char(a.date_,'mm'),'02',a.totern,0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' and a.date_ " + xprdRange + " ) a group by a.section";
                }
                else
                {
                    header_n = "Net Pay Trend Statement(Section Wise)";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'code' as code,a.section,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select trim(b.NAME) as EMPNAME,b.fpfnominee as section,a.grade,decode(to_char(a.date_,'mm'),'03',a.netslry,0) as mar,decode(to_char(a.date_,'mm'),'04',a.netslry,0) as apr,decode(to_char(a.date_,'mm'),'05',a.netslry,0) as may,decode(to_char(a.date_,'mm'),'06',a.netslry,0) as jun,decode(to_char(a.date_,'mm'),'07',a.netslry,0) as jul,decode(to_char(a.date_,'mm'),'08',a.netslry,0) as aug,decode(to_char(a.date_,'mm'),'09',a.netslry,0) as sept,decode(to_char(a.date_,'mm'),'10',a.netslry,0) as oct,decode(to_char(a.date_,'mm'),'11',a.netslry,0) as nov,decode(to_char(a.date_,'mm'),'12',a.netslry,0) as dec,decode(to_char(a.date_,'mm'),'01',a.netslry,0) as jan,decode(to_char(a.date_,'mm'),'02',a.netslry,0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' AND A.GRADE='" + mq1 + "' and  a.date_  " + xprdRange + " ) a group by a.section";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_trend_section_wise", "std_trend_section_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85101"://ATTN ENTRY FORM PRINT
                #region
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                header_n = "Attendance Report";
                if (frm_cocd == "HGLO")
                {
                    //SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.branchcd,a.type,a.grade,a.empcode,b.desg_text,b.name as empname,nvl(a.timeinhr,0) as timeinhr,nvl(a.timeinmin,0) as timeinmin,nvl(a.timeouthr,0) as timeouthr,nvl(a.timeoutmin,0) as timeoutmin,nvl(a.hrwrk,0) as hrwrk,nvl(a.minwrk,0) as minwrk,nvl(a.dt1,0) as dt1,nvl(a.dt2,0) as dt2,nvl(a.dt3,0) as dt3,nvl(a.dt4,0) as dt4,nvl(a.dt5,0) as dt5  from attn a ,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode) =trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and trim(a.branchcd)||trim(a.grade)||trim(a.empcode)='" + mq1 + "' and a.vchdate " + xprdRange + " and a.type='10'";
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.branchcd,a.type,a.grade,a.empcode,b.desg_text,b.name as empname,nvl(a.timeinhr,0) as timeinhr,nvl(a.timeinmin,0) as timeinmin,nvl(a.timeouthr,0) as timeouthr,nvl(a.timeoutmin,0) as timeoutmin,nvl(a.hrwrk,0) as hrwrk,nvl(a.minwrk,0) as minwrk,nvl(a.dt1,0) as dt1,nvl(a.dt2,0) as dt2,nvl(a.dt3,0) as dt3,nvl(a.dt4,0) as dt4,nvl(a.dt5,0) as dt5  from attn a ,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode) =trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and trim(a.branchcd)||trim(a.grade)||trim(a.empcode)='" + mq1 + "' and a.vchdate " + xprdRange + " and a.type='10'";
                }
                else
                {
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                    mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                    SQuery = "select '" + header_n + "' as header,'" + mq2 + "' as mth,a.branchcd,a.type,a.grade,D.NAME,a.empcode,b.desg_text,b.name as empname,nvl(a.timeinhr,0) as timeinhr,nvl(a.timeinmin,0) as timeinmin,nvl(a.timeouthr,0) as timeouthr,nvl(a.timeoutmin,0) as timeoutmin,nvl(a.hrwrk,0) as hrwrk,nvl(a.minwrk,0) as minwrk,nvl(a.dt1,0) as dt1,nvl(a.dt2,0) as dt2,nvl(a.dt1,0) + nvl(a.dt2,0) as dt3,0 as dt4,0 as dt5  from attn a ,empmas b,TYPE d where trim(a.branchcd)||trim(a.grade)||trim(a.empcode) =trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and trim(a.grade)=trim(d.type1) and d.id='I' and trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "' AND A.GRADE='" + mq3 + "' ORDER BY A.empcode ";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    if (frm_cocd == "HGLO")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "attn_print", "attn_print", dsRep, header_n);
                    }
                    else
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "attn_print_N", "attn_print_N", dsRep, header_n); //FOR ALL CLIENTS
                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85101_old"://ATTN ENTRY FORM PRINT
                #region
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                header_n = "Attendance Report";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.branchcd,a.type,a.grade,a.empcode,b.desg_text,b.name as empname,nvl(a.timeinhr,0) as timeinhr,nvl(a.timeinmin,0) as timeinmin,nvl(a.timeouthr,0) as timeouthr,nvl(a.timeoutmin,0) as timeoutmin,nvl(a.hrwrk,0) as hrwrk,nvl(a.minwrk,0) as minwrk,nvl(a.dt1,0) as dt1,nvl(a.dt2,0) as dt2,nvl(a.dt3,0) as dt3,nvl(a.dt4,0) as dt4,nvl(a.dt5,0) as dt5  from attn a ,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode) =trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and trim(a.branchcd)||trim(a.grade)||trim(a.empcode)='" + mq1 + "' and a.vchdate " + xprdRange + " and a.type='10'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "attn_print", "attn_print", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85107*": // SALARY PRINT HGLO
            case "F85152":
                #region
                dtm = new DataTable();
                dtm.Columns.Add("sno", typeof(int));
                dtm.Columns.Add("empcode", typeof(string));
                dtm.Columns.Add("empname", typeof(string));
                dtm.Columns.Add("plant", typeof(string));
                dtm.Columns.Add("dept", typeof(string));
                dtm.Columns.Add("desg", typeof(string));
                dtm.Columns.Add("catg", typeof(string));
                dtm.Columns.Add("List", typeof(string));
                dtm.Columns.Add("tot_prsnt_hr", typeof(double));
                dtm.Columns.Add("tot_ot_hr", typeof(double));
                dtm.Columns.Add("tot_2d_dedn_hr", typeof(double));
                dtm.Columns.Add("tot_lt_cmng_hr", typeof(double));
                dtm.Columns.Add("tot_fine", typeof(double));
                dtm.Columns.Add("tot_sleping", typeof(double));
                dtm.Columns.Add("tot_oth_dedn", typeof(double));
                dtm.Columns.Add("wrkng_hr", typeof(double));
                dtm.Columns.Add("2_5_day_lv", typeof(double));
                dtm.Columns.Add("sun_pyble_Day", typeof(double));
                dtm.Columns.Add("tot_pyble_hr", typeof(double));
                dtm.Columns.Add("net_pyble_slry", typeof(double));
                dtm.Columns.Add("2_5_day_amt", typeof(double));
                dtm.Columns.Add("attn_bons", typeof(double));
                dtm.Columns.Add("tot_ot_amt", typeof(double));
                dtm.Columns.Add("tot_2d_dedn", typeof(double));
                dtm.Columns.Add("tot_lt_cmng", typeof(double));
                dtm.Columns.Add("tot_Fine_1", typeof(double));
                dtm.Columns.Add("tot_sleping_1", typeof(double));
                dtm.Columns.Add("tot_oth_dedn_1", typeof(double));
                dtm.Columns.Add("Prev_mth_addn", typeof(double));
                dtm.Columns.Add("adv", typeof(double));
                dtm.Columns.Add("fooding", typeof(double));
                dtm.Columns.Add("Prev_mth_addn_1", typeof(double));
                dtm.Columns.Add("Prev_mth_subt", typeof(double));
                dtm.Columns.Add("spcl_addn", typeof(double));
                dtm.Columns.Add("spcl_subt", typeof(double));
                dtm.Columns.Add("grs_sal", typeof(double));
                dtm.Columns.Add("esi", typeof(double));
                dtm.Columns.Add("pf", typeof(double));
                dtm.Columns.Add("staff_welf", typeof(double));
                dtm.Columns.Add("tds", typeof(double));
                dtm.Columns.Add("adv_oth", typeof(double));
                dtm.Columns.Add("dedn", typeof(double));
                dtm.Columns.Add("net_sal", typeof(double));
                dtm.Columns.Add("bank_sal", typeof(double));
                dtm.Columns.Add("cash_sal", typeof(double));
                dtm.Columns.Add("mth", typeof(string));

                mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"); // grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); // vchnum & vchdate

                mq2 = "SELECT trim(a.branchcd)||trim(a.grade)||trim(a.empcode) as fstr,to_char(a.vchdate,'mm/yyyy') as mnth,a.* FROM WBPAYH a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' and a.grade='" + mq0 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "' order by srno";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);//main dt from wbpayh

                mq3 = "select trim(branchcd)||trim(grade)||trim(empcode) as fstr,grade,empcode,name,fhname,deptt,desg,esino,wrkhour,deptt_text,desg_text,leaving_dt,plant,list from empmas where branchcd='" + frm_mbr + "' and grade='" + mq0 + "'";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq3);//empmas dt

                mq4 = "select trim(branchcd)||trim(grade)||trim(empcode) as fstr,vchnum,ded3 as  esi,ded1 as pf,ded6 as staf_welf,ded4 as tds,ded5 as adv_other,ded3+ded1+ded6+ded4+ded5 as dedn,trim(mastvch) as mstvch from pay where branchcd='" + frm_mbr + "' and grade='" + mq0 + "' and trim(mastvch)='" + mq1 + "'";
                dt4 = fgen.getdata(frm_qstr, frm_cocd, mq4);//paydt
                if (dt1.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt1);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "fstr"); //MAIN                        
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView viewim = new DataView(dt1, "fstr='" + dr0["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt3 = viewim.ToTable();
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            dr1 = dtm.NewRow();
                            dr1["sno"] = i;
                            dr1["mth"] = dt3.Rows[i]["mnth"].ToString().Trim();
                            dr1["empcode"] = dt3.Rows[i]["empcode"].ToString().Trim();
                            dr1["empname"] = fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "name");
                            dr1["plant"] = fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "plant");
                            dr1["dept"] = fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "deptt_text");
                            dr1["desg"] = fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "desg_text");
                            dr1["catg"] = dt3.Rows[i]["grade"].ToString().Trim();
                            dr1["List"] = fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "list");
                            dr1["tot_prsnt_hr"] = fgen.make_double(dt3.Rows[i]["pr_hrs"].ToString().Trim());
                            dr1["tot_ot_hr"] = fgen.make_double(dt3.Rows[i]["ot_hrs"].ToString().Trim());
                            dr1["tot_2d_dedn_hr"] = fgen.make_double(dt3.Rows[i]["tot_2d_hrs"].ToString().Trim());
                            dr1["tot_lt_cmng_hr"] = fgen.make_double(dt3.Rows[i]["tot_late_hrs"].ToString().Trim());
                            dr1["tot_fine"] = fgen.make_double(dt3.Rows[i]["tot_fine_hrs"].ToString().Trim());
                            dr1["tot_sleping"] = fgen.make_double(dt3.Rows[i]["tot_sleep_hrs"].ToString().Trim());
                            dr1["tot_oth_dedn"] = fgen.make_double(dt3.Rows[i]["tot_other_ded_hrs"].ToString().Trim());
                            dr1["wrkng_hr"] = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "wrkhour"));
                            dr1["2_5_day_lv"] = fgen.make_double(dr1["wrkng_hr"].ToString().Trim()) * 2.5;
                            dr1["sun_pyble_Day"] = fgen.make_double(dt3.Rows[i]["sunday"].ToString().Trim());
                            dr1["tot_pyble_hr"] = fgen.make_double(dt3.Rows[i]["pay_hrs"].ToString().Trim());
                            dr1["net_pyble_slry"] = fgen.make_double(dt3.Rows[i]["pay_sal"].ToString().Trim());
                            dr1["2_5_day_amt"] = fgen.make_double(dt3.Rows[i]["days_"].ToString().Trim());
                            dr1["attn_bons"] = fgen.make_double(dt3.Rows[i]["attn"].ToString().Trim());
                            dr1["tot_ot_amt"] = fgen.make_double(dt3.Rows[i]["ot"].ToString().Trim());
                            dr1["tot_2d_dedn"] = fgen.make_double(dt3.Rows[i]["ded_2d"].ToString().Trim());
                            dr1["tot_lt_cmng"] = fgen.make_double(dt3.Rows[i]["late"].ToString().Trim());
                            dr1["tot_Fine_1"] = fgen.make_double(dt3.Rows[i]["fine"].ToString().Trim());
                            dr1["tot_sleping_1"] = fgen.make_double(dt3.Rows[i]["sleep"].ToString().Trim());
                            dr1["tot_oth_dedn_1"] = fgen.make_double(dt3.Rows[i]["oth_ded"].ToString().Trim());
                            dr1["Prev_mth_addn"] = fgen.make_double(dt3.Rows[i]["prev_mth_add"].ToString().Trim());
                            dr1["adv"] = fgen.make_double(dt3.Rows[i]["advance"].ToString().Trim());
                            dr1["fooding"] = fgen.make_double(dt3.Rows[i]["fooding"].ToString().Trim());
                            dr1["Prev_mth_addn_1"] = 0;//again prev mth.....no need in rpt file
                            dr1["Prev_mth_subt"] = fgen.make_double(dt3.Rows[i]["prev_mth_sub"].ToString().Trim());
                            dr1["spcl_addn"] = fgen.make_double(dt3.Rows[i]["spl_add"].ToString().Trim());
                            dr1["spcl_subt"] = fgen.make_double(dt3.Rows[i]["spl_sub"].ToString().Trim());
                            dr1["grs_sal"] = fgen.make_double(dt3.Rows[i]["gross"].ToString().Trim());
                            dr1["esi"] = fgen.make_double(fgen.seek_iname_dt(dt4, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "esi"));
                            dr1["pf"] = fgen.make_double(fgen.seek_iname_dt(dt4, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "pf"));
                            dr1["staff_welf"] = fgen.make_double(fgen.seek_iname_dt(dt4, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "staf_welf"));
                            dr1["tds"] = fgen.make_double(fgen.seek_iname_dt(dt4, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "tds"));
                            dr1["adv_oth"] = fgen.make_double(fgen.seek_iname_dt(dt4, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "adv_other"));
                            dr1["dedn"] = fgen.make_double(fgen.seek_iname_dt(dt4, "fstr='" + dt3.Rows[i]["fstr"].ToString().Trim() + "'", "dedn"));
                            dr1["net_sal"] = 0;
                            dr1["bank_sal"] = 0;
                            dr1["cash_sal"] = 0;
                            dtm.Rows.Add(dr1);
                        }
                    }
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "salary_chart_HGLO", "salary_chart_HGLO", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85132": // EMPLOYEE MASTER
                #region Employee Master
                if (frm_cocd == "HGLO")
                {
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = "";
                    mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");//EMPCODE
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//GRADE
                    dt1 = new DataTable();
                    mq2 = "SELECT ED_FLD as er,nvl(SUBSTR(TRIM(ED_NAME),1,15),'-') AS NAME FROM WB_SELMAST WHERE branchcd='" + frm_mbr + "' and GRADE='" + mq1 + "' AND ED_FLD LIKE 'ER%' and nvl(icat,'-')!='Y' ORDER BY ED_FLD";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);//dt for headings
                    dtm = new DataTable();
                    dr1 = dtm.NewRow();
                    for (int j = 1; j < 11; j++)
                    {
                        dtm.Columns.Add("ername_" + j + "", typeof(string));
                        dr1["ername_" + j + ""] = fgen.seek_iname_dt(dt1, "er='er" + j + "'", "NAME");
                    }
                    dtm.Rows.Add(dr1);
                    dt = new DataTable();
                    mq3 = "select a.*,substr(trim(a.appr_by),4,length(trim(a.appr_by))) as approved_by,t.name as grade_name from empmas a,type t where trim(a.grade)=trim(t.type1) and t.id='I' and a.branchcd||a.grade||a.empcode in (" + mq0 + ") order by empcode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq3);

                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Add("emp_logo", typeof(System.Byte[]));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            mq4 = dt.Rows[i]["empimg"].ToString().Trim(); //image path
                            //for image
                            if (mq4 != "" && mq4 != "-")
                            {
                                fpath = mq4;
                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);
                                dt.Rows[i]["emp_logo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                FilStr.Close();
                                BinRed.Close();
                            }
                        }

                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);
                        dtm.TableName = "Headings";
                        dsRep.Tables.Add(dtm);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "Empmas_HGLO", "Empmas_HGLO", dsRep, header_n);
                    }
                    else
                    {
                        data_found = "N";
                    }
                }
                else
                {
                    dtm = new DataTable();
                    #region Headings
                    dtm.Columns.Add("header", typeof(string));
                    dtm.Columns.Add("EMPCODE", typeof(string));
                    dtm.Columns.Add("EMPNAME", typeof(string));
                    dtm.Columns.Add("FNAME", typeof(string));
                    dtm.Columns.Add("grade", typeof(string));
                    dtm.Columns.Add("SPOUSE", typeof(string));
                    dtm.Columns.Add("DESGN", typeof(string));
                    dtm.Columns.Add("DEPTT", typeof(string));
                    dtm.Columns.Add("PFNO", typeof(string));
                    dtm.Columns.Add("DOB", typeof(string));
                    dtm.Columns.Add("DOM", typeof(string));
                    dtm.Columns.Add("LEAVING_DT", typeof(string));
                    dtm.Columns.Add("UINNO", typeof(string));
                    dtm.Columns.Add("BANK", typeof(string));
                    dtm.Columns.Add("BANKAC", typeof(string));
                    dtm.Columns.Add("GENDER", typeof(string));
                    dtm.Columns.Add("JOINDT", typeof(string));
                    dtm.Columns.Add("WRKHR", typeof(string));
                    dtm.Columns.Add("PFCUT", typeof(string));
                    dtm.Columns.Add("CUTVPF", typeof(string));
                    dtm.Columns.Add("CARDNO", typeof(string));
                    dtm.Columns.Add("COUNTRY", typeof(string));
                    dtm.Columns.Add("MARRIED", typeof(string));
                    dtm.Columns.Add("MOB", typeof(string));
                    dtm.Columns.Add("TELPH", typeof(string));
                    dtm.Columns.Add("ADDR1", typeof(string));
                    dtm.Columns.Add("ADDR2", typeof(string));
                    dtm.Columns.Add("CITY", typeof(string));
                    dtm.Columns.Add("PIN", typeof(string));
                    dtm.Columns.Add("PADDR1", typeof(string));
                    dtm.Columns.Add("PADDR2", typeof(string));
                    dtm.Columns.Add("PCITY", typeof(string));
                    dtm.Columns.Add("PPIN", typeof(string));
                    dtm.Columns.Add("ADHARNO", typeof(string));
                    dtm.Columns.Add("ESINO", typeof(string));
                    dtm.Columns.Add("EMAIL", typeof(string));
                    dtm.Columns.Add("LEAVING_WHY", typeof(string));
                    dtm.Columns.Add("PROB_MTH", typeof(string));
                    dtm.Columns.Add("BLOODGRP", typeof(string));
                    dtm.Columns.Add("QUALIFIC", typeof(string));
                    dtm.Columns.Add("ESICUT", typeof(string));
                    dtm.Columns.Add("PAN", typeof(string));
                    dtm.Columns.Add("EL", typeof(double));
                    dtm.Columns.Add("CL", typeof(double));
                    dtm.Columns.Add("SL", typeof(double));
                    dtm.Columns.Add("EXP", typeof(double));
                    dtm.Columns.Add("OPF", typeof(double));
                    dtm.Columns.Add("PFLIM", typeof(string));
                    dtm.Columns.Add("ESIDESP", typeof(string));
                    dtm.Columns.Add("MLOGO", typeof(System.Byte[]));
                    dtm.Columns.Add("ERNAME_1", typeof(string));
                    dtm.Columns.Add("ERNAME_2", typeof(string));
                    dtm.Columns.Add("ERNAME_3", typeof(string));
                    dtm.Columns.Add("ERNAME_4", typeof(string));
                    dtm.Columns.Add("ERNAME_5", typeof(string));
                    dtm.Columns.Add("ERNAME_6", typeof(string));
                    dtm.Columns.Add("ERNAME_7", typeof(string));
                    dtm.Columns.Add("ERNAME_8", typeof(string));
                    dtm.Columns.Add("ERNAME_9", typeof(string));
                    dtm.Columns.Add("ERNAME_10", typeof(string));
                    dtm.Columns.Add("ERNAME_11", typeof(string));
                    dtm.Columns.Add("ERNAME_12", typeof(string));
                    dtm.Columns.Add("ERNAME_13", typeof(string));
                    dtm.Columns.Add("ER1", typeof(double));
                    dtm.Columns.Add("ER2", typeof(double));
                    dtm.Columns.Add("ER3", typeof(double));
                    dtm.Columns.Add("ER4", typeof(double));
                    dtm.Columns.Add("ER5", typeof(double));
                    dtm.Columns.Add("ER6", typeof(double));
                    dtm.Columns.Add("ER7", typeof(double));
                    dtm.Columns.Add("ER8", typeof(double));
                    dtm.Columns.Add("ER9", typeof(double));
                    dtm.Columns.Add("ER10", typeof(double));
                    dtm.Columns.Add("ER11", typeof(double));
                    dtm.Columns.Add("ER12", typeof(double));
                    dtm.Columns.Add("ER13", typeof(double));
                    dtm.Columns.Add("DEDNAME_1", typeof(string));
                    dtm.Columns.Add("DEDNAME_2", typeof(string));
                    dtm.Columns.Add("DEDNAME_3", typeof(string));
                    dtm.Columns.Add("DEDNAME_4", typeof(string));
                    dtm.Columns.Add("DEDNAME_5", typeof(string));
                    dtm.Columns.Add("DEDNAME_6", typeof(string));
                    dtm.Columns.Add("DEDNAME_7", typeof(string));
                    dtm.Columns.Add("DEDNAME_8", typeof(string));
                    dtm.Columns.Add("DEDNAME_9", typeof(string));
                    dtm.Columns.Add("DEDNAME_10", typeof(string));
                    dtm.Columns.Add("DEDNAME_11", typeof(string));
                    dtm.Columns.Add("DEDNAME_12", typeof(string));
                    dtm.Columns.Add("DEDNAME_13", typeof(string));
                    dtm.Columns.Add("DED1", typeof(double));
                    dtm.Columns.Add("DED2", typeof(double));
                    dtm.Columns.Add("DED3", typeof(double));
                    dtm.Columns.Add("DED4", typeof(double));
                    dtm.Columns.Add("DED5", typeof(double));
                    dtm.Columns.Add("DED6", typeof(double));
                    dtm.Columns.Add("DED7", typeof(double));
                    dtm.Columns.Add("DED8", typeof(double));
                    dtm.Columns.Add("DED9", typeof(double));
                    dtm.Columns.Add("DED10", typeof(double));
                    dtm.Columns.Add("DED11", typeof(double));
                    dtm.Columns.Add("DED12", typeof(double));
                    dtm.Columns.Add("DED13", typeof(double));
                    #endregion
                    header_n = "Employee Master";
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"); //GRADE
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //EMPCODE
                    mq3 = ""; mq4 = ""; mq5 = ""; mq6 = "";

                    dt = new DataTable();
                    mq3 = "select to_char(a.d_o_m,'dd/mm/yyyy') as dom,to_char(a.d_o_b,'dd/mm/yyyy') as dob,to_char(a.dtjoin,'dd/mm/yyyy') as joindt,a.*,(case when nvl(a.mnthinc,0)=1 then 'Y' else 'N' end) as pflim from empmas a where a.branchcd||trim(a.grade)||trim(a.empcode) in (" + mq1 + ")";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq3);

                    dt1 = new DataTable();
                    mq4 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq2 + "' and ed_fld like 'ER%' AND rownum<14 and NVL(TRIM(ICAT),'-')!='Y' order by morder";//
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq4); // EARNINGS HEADING

                    dt2 = new DataTable();
                    mq5 = "select distinct ed_fld,ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq2 + "' and ed_fld like 'DED%' AND rownum<14 and NVL(TRIM(ICAT),'-')!='Y' order by morder";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5); // DEDUCTION HEADING

                    if (dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {
                            #region
                            dr1 = dtm.NewRow();
                            dr1["HEADER"] = header_n;
                            dr1["EMPCODE"] = dt.Rows[j]["empcode"].ToString().Trim();
                            dr1["EMPNAME"] = dt.Rows[j]["name"].ToString().Trim();
                            dr1["grade"] = dt.Rows[j]["grade"].ToString().Trim();
                            dr1["FNAME"] = dt.Rows[j]["fhname"].ToString().Trim();
                            dr1["SPOUSE"] = dt.Rows[j]["married"].ToString().Trim();
                            dr1["DESGN"] = dt.Rows[j]["desg_text"].ToString().Trim();
                            dr1["DEPTT"] = dt.Rows[j]["deptt_text"].ToString().Trim();
                            dr1["PFNO"] = dt.Rows[j]["pfno"].ToString().Trim();
                            dr1["UINNO"] = dt.Rows[j]["uinno"].ToString().Trim();
                            dr1["bank"] = dt.Rows[j]["bank"].ToString().Trim();
                            dr1["bankac"] = dt.Rows[j]["bnkacno"].ToString().Trim();
                            dr1["gender"] = dt.Rows[j]["sex"].ToString().Trim();
                            dr1["dob"] = dt.Rows[j]["dob"].ToString().Trim();
                            dr1["dom"] = dt.Rows[j]["dom"].ToString().Trim();
                            dr1["joindt"] = dt.Rows[j]["joindt"].ToString().Trim();
                            dr1["wrkhr"] = dt.Rows[j]["wrkhour"].ToString().Trim();
                            dr1["pfcut"] = dt.Rows[j]["pfcut"].ToString().Trim();
                            dr1["cutvpf"] = dt.Rows[j]["cutvpf"].ToString().Trim();
                            dr1["cardno"] = dt.Rows[j]["cardno"].ToString().Trim();
                            dr1["country"] = dt.Rows[j]["country"].ToString().Trim();
                            dr1["married"] = dt.Rows[j]["married"].ToString().Trim();
                            dr1["mob"] = dt.Rows[j]["mobile"].ToString().Trim();
                            dr1["telph"] = dt.Rows[j]["phone"].ToString().Trim();
                            dr1["addr1"] = dt.Rows[j]["addr1"].ToString().Trim();
                            dr1["addr2"] = dt.Rows[j]["addr2"].ToString().Trim();
                            dr1["city"] = dt.Rows[j]["city"].ToString().Trim();
                            dr1["pin"] = dt.Rows[j]["pin"].ToString().Trim();
                            dr1["paddr1"] = dt.Rows[j]["paddr1"].ToString().Trim();
                            dr1["paddr2"] = dt.Rows[j]["paddr2"].ToString().Trim();
                            dr1["pcity"] = dt.Rows[j]["pcity"].ToString().Trim();
                            dr1["ppin"] = dt.Rows[j]["ppin"].ToString().Trim();
                            dr1["adharno"] = dt.Rows[j]["adharno"].ToString().Trim();
                            dr1["esino"] = dt.Rows[j]["esino"].ToString().Trim();
                            dr1["leaving_why"] = dt.Rows[j]["leaving_why"].ToString().Trim();
                            dr1["email"] = dt.Rows[j]["email"].ToString().Trim();
                            dr1["prob_mth"] = dt.Rows[j]["deptt2"].ToString().Trim();
                            dr1["bloodgrp"] = dt.Rows[j]["bloodgrp"].ToString().Trim();
                            dr1["QUALIFIC"] = dt.Rows[j]["QUALIFIC"].ToString().Trim();
                            dr1["LEAVING_DT"] = dt.Rows[j]["LEAVING_DT"].ToString().Trim();
                            dr1["esicut"] = dt.Rows[j]["esicut"].ToString().Trim();
                            dr1["PAN"] = dt.Rows[j]["trade"].ToString().Trim();
                            dr1["el"] = fgen.make_double(dt.Rows[j]["el"].ToString().Trim());
                            dr1["cl"] = fgen.make_double(dt.Rows[j]["cl"].ToString().Trim());
                            dr1["sl"] = fgen.make_double(dt.Rows[j]["sl"].ToString().Trim());
                            dr1["opf"] = fgen.make_double(dt.Rows[j]["op_coff"].ToString().Trim());
                            dr1["exp"] = fgen.make_double(dt.Rows[j]["DEPTT1"].ToString().Trim());
                            dr1["pflim"] = dt.Rows[j]["pflim"].ToString().Trim();
                            dr1["esidesp"] = dt.Rows[j]["esi_disp"].ToString().Trim();

                            for (int i = 1; i < 14; i++)
                            {
                                dr1["er" + i + ""] = fgen.make_double(dt.Rows[j]["er" + i + ""].ToString().Trim());
                                dr1["ded" + i + ""] = fgen.make_double(dt.Rows[j]["ded" + i + ""].ToString().Trim());

                                mq5 = fgen.seek_iname_dt(dt1, "ed_fld='" + dtm.Columns["er" + i + ""] + "' ", "ed_name");
                                if (mq5.Length == 1)
                                {
                                    mq5 = "-";
                                }
                                dr1["ername_" + i + ""] = mq5;
                                mq6 = fgen.seek_iname_dt(dt2, "ed_fld='" + dtm.Columns["ded" + i + ""] + "' ", "ed_name");
                                if (mq6.Length == 1)
                                {
                                    mq6 = "-";
                                }
                                dr1["dedname_" + i + ""] = mq6;
                            }
                            mq4 = dt.Rows[j]["empimg"].ToString().Trim(); //image path saved in this
                            //for image
                            if (mq4 != "" && mq4 != "-")
                            {
                                fpath = mq4;
                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);
                                dr1["mLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                FilStr.Close();
                                BinRed.Close();
                            }
                            dtm.Rows.Add(dr1);
                            #endregion
                        }
                        dtm.TableName = "Prepcur";
                        dsRep.Tables.Add(dtm);
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_Emp", "std_Emp", dsRep, header_n);
                    }
                    else
                    {
                        data_found = "N";
                    }
                }
                #endregion
                break;

            case "F85231":
                #region
                header_n = "Date of Marriage Register";
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT '" + header_n + "' as header,  A.EMPCODE,B.NAME AS PLANT ,A.NAME,A.DEPTT_TEXT,A.DESG_TEXT,(case when a.d_o_m is null then '-'  else to_char(d_o_m,'dd/mm/yyyy') end) as Marriage_DATE FROM EMPMAS A ,TYPE B WHERE TRIM(A.BRANCHCD)=TRIM(B.TYPE1) AND B.ID='B' AND A.BRANCHCD='" + frm_mbr + "' AND A.GRADE='" + mq0 + "' and nvl(trim(leaving_dt),'-')='-' ORDER BY A.EMPCODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Anniversary_List", "std_Anniversary_List", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82587":
                #region late coming register date wise
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                header_n = "Late Coming Register";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,A.EMPCODE,B.NAME,B.FHNAME,A.TIMEINHR,A.TIMEINMIN,TIMEOUTHR,TIMEOUTMIN,HRWRK,MINWRK,SHFTINHR,SHFTINMIN   from attn A,EMPMAS B where TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.grade='" + mq0 + "' AND A.VCHDATE " + xprdRange + "";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Lt_coming", "Lt_coming", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82588":
                #region 31 day late coming report
                mq6 = ""; mq7 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//mth
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//mthname
                mq6 = fgen.seek_iname(frm_qstr, frm_cocd, "select  NAME  FROM TYPE WHERE ID='I'  AND SUBSTR(TYPE1,1,1)<'2'  and type1='" + mq0 + "'", "NAME");
                header_n = "Showing Late Coming For " + mq6 + " ";
                if (Convert.ToInt32(mq1) > 3)
                {        //same financial year     
                    myear = frm_myear;
                }
                else
                {
                    int d = Convert.ToInt32(frm_myear) + 1;
                    myear = Convert.ToString(d);
                }
                mq7 = mq1 + "/" + myear;
                SQuery = "select '" + header_n + "' as header,'" + mq2 + "' as mthname, TO_CHar(a.vchdate,'yyyyMMdd') as vdd,a.empcode,a.timeinhr,a.timeinmin,a.timeouthr,a.timeoutmin,a.hrwrk,a.minwrk,a.srno,a.refr,a.shftinhr,a.shftinmin,a.extinhr,a.extinmin,a.shfttag,B.NAME,B.FHNAME from attn A,EMPMAS B where TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND A.BRANCHCD='" + frm_mbr + "' AND to_char(A.vchdate,'mm/yyyy')='" + mq7 + "' and A.grade='" + mq0 + "' ORDER BY vdd";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Lt_coming_Daywise", "Lt_coming_Daywise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82573":
                #region
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//dol or pol option                             
                if (mq1 == "DOL")
                {
                    header_n = "Date of Leaving Register:";
                    SQuery = "select '" + header_n + "' as header,empcode,name,fhname,trim(deptt_Text) as dept,trim(desg_text) as desg,leaving_dt,to_date(leaving_dt,'dd/mm/yyyy') as vdd  from empmas where branchcd='" + frm_mbr + "' and grade='" + mq0 + "' and length(nvl(leaving_Dt,'-'))>1 order by vdd";
                }
                else
                {
                    header_n = "Leaving Between " + fromdt + " To " + todt + " Grades:" + mq0 + "";
                    SQuery = "select '" + header_n + "' as header,empcode,name,fhname,trim(deptt_Text) as dept,trim(desg_text) as desg,leaving_dt,to_date(leaving_dt,'dd/mm/yyyy') as vdd  from empmas where branchcd='" + frm_mbr + "' and grade='" + mq0 + "' and to_date(leaving_Dt,'dd/mm/yyyy') " + xprdRange + " and length(nvl(leaving_Dt,'-'))>1 order by vdd";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Leaving_List", "Leaving_List", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82574":
                #region
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//selection      
                SQuery = "select  b.dtjoin,a.*,b.name from attn a,empmas b  where trim(a.empcode)=trim(b.empcode) and a.vchdate between to_date('01/04/2018','dd/mm/yyyy') and to_Date('31/03/2019','dd/mm/yyyy') and a.empcode='020043' and b.dtjoin between to_date('01/04/2018','dd/mm/yyyy') and to_Date('31/03/2019','dd/mm/yyyy')";
                //hold this report as per mayuri mam.................
                #endregion
                break;

            case "F82575":
                #region last incr dt and incr value is pending to place in report...not find
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//empcode            
                header_n = "Performance Appraisal";
                SQuery = "SELECT '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as toddt,A.EMPCODE,A.NAME,A.FHNAME,A.DEPTT_TEXT,A.DESG_TEXT,A.QUALIFIC,SUM(B.TOTDAYS) AS TOTDAYS,SUM(B.PRESENT) AS PRENT,SUM(B.EL+B.CL+B.SL) AS CL,SUM(B.ABSENT) AS ABSENT,SUM(B.OFFDAYS) AS OFFDAYS,to_char(a.dtjoin,'dd/mm/yyyy') as aptt_dt,sum(a.er1+a.er2+a.er3+a.er4+a.er5+a.er6+a.er7+a.er8+a.er9+a.er10) as present_Sal FROM  PAY B,EMPMAS A WHERE TRIM(A.BRANCHCD)||TRIM(A.GRADE)||TRIM(A.EMPCODE)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||TRIM(B.EMPCODE) and b.branchcd='" + frm_mbr + "' AND A.GRADE='" + mq0 + "' AND A.EMPCODE in (" + mq1 + ") AND B.DATE_ " + xprdRange + " GROUP BY  A.EMPCODE,A.NAME,A.FHNAME,A.DEPTT_TEXT,A.DESG_TEXT,A.QUALIFIC,to_char(a.dtjoin,'dd/mm/yyyy')";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Appraisal_rpt", "Appraisal_rpt", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82576":
                #region confirmation Letter
                mq0 = ""; mq1 = ""; mq2 = ""; dt = new DataTable();
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//empcode       
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//selection  
                SQuery = "select  grade,trim(branchcd)||trim(empcode) as cardno,empcode,name,fhname,sex as gender,deptt_Text,desg_Text,conf_Dt ,'" + mq2 + "' as option_ from empmas where BRANCHCD='" + frm_mbr + "' AND GRADE='" + mq0 + "' AND empcode in (" + mq1 + ")";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Conf_letter", "Conf_letter", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82577":
                #region Appointment Letter...no need continued next page on report as per mayuri mam
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("option_", typeof(string));
                ph_tbl.Columns.Add("grade", typeof(string));
                ph_tbl.Columns.Add("EMPCODE", typeof(string));
                ph_tbl.Columns.Add("name", typeof(string));
                ph_tbl.Columns.Add("fhname", typeof(string));
                ph_tbl.Columns.Add("addr1", typeof(string));
                ph_tbl.Columns.Add("addr2", typeof(string));
                ph_tbl.Columns.Add("spouse", typeof(string));
                ph_tbl.Columns.Add("desg_Text", typeof(string));
                ph_tbl.Columns.Add("deptt_text", typeof(string));
                ph_tbl.Columns.Add("dtjoin", typeof(string));
                ph_tbl.Columns.Add("er1", typeof(double));
                ph_tbl.Columns.Add("er2", typeof(double));
                ph_tbl.Columns.Add("er3", typeof(double));
                ph_tbl.Columns.Add("er4", typeof(double));
                ph_tbl.Columns.Add("er5", typeof(double));
                ph_tbl.Columns.Add("er6", typeof(double));
                ph_tbl.Columns.Add("er7", typeof(double));
                ph_tbl.Columns.Add("er8", typeof(double));
                ph_tbl.Columns.Add("er9", typeof(double));
                ph_tbl.Columns.Add("er10", typeof(double));
                mq0 = ""; mq1 = ""; mq2 = ""; dt = new DataTable();
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//empcode       
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//selection  
                dt = new DataTable();
                SQuery = "select '" + mq2 + "' as option_,empcode,grade,addr1,addr2,trim(branchcd)||trim(empcode) as cardno,to_char(d_o_b,'dd/mm/yyyy') as d_o_b,name,married,fhname,desg_Text,deptt_text,to_char(dtjoin,'dd/mm/yyyy') as dtjoin,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10  from empmas where branchcd='" + frm_mbr + "' AND GRADE='" + mq0 + "' and empcode in (" + mq1 + ") order by empcode";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq0 + "' and ed_fld like 'ER%' AND rownum<11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["option_"] = dt.Rows[i]["option_"].ToString().Trim();
                    dr1["grade"] = dt.Rows[i]["grade"].ToString().Trim();
                    dr1["EMPCODE"] = dt.Rows[i]["empcode"].ToString().Trim();
                    dr1["name"] = dt.Rows[i]["name"].ToString().Trim();
                    dr1["addr1"] = dt.Rows[i]["addr1"].ToString().Trim();
                    dr1["addr2"] = dt.Rows[i]["addr2"].ToString().Trim();
                    dr1["fhname"] = dt.Rows[i]["fhname"].ToString().Trim();
                    dr1["spouse"] = dt.Rows[i]["married"].ToString().Trim();
                    dr1["desg_Text"] = dt.Rows[i]["desg_Text"].ToString().Trim();
                    dr1["deptt_text"] = dt.Rows[i]["deptt_text"].ToString().Trim();
                    dr1["dtjoin"] = dt.Rows[i]["dtjoin"].ToString().Trim();
                    dr1["er1"] = fgen.make_double(dt.Rows[i]["er1"].ToString().Trim());
                    dr1["er2"] = fgen.make_double(dt.Rows[i]["er2"].ToString().Trim());
                    dr1["er3"] = fgen.make_double(dt.Rows[i]["er3"].ToString().Trim());
                    dr1["er4"] = fgen.make_double(dt.Rows[i]["er4"].ToString().Trim());
                    dr1["er5"] = fgen.make_double(dt.Rows[i]["er5"].ToString().Trim());
                    dr1["er6"] = fgen.make_double(dt.Rows[i]["er6"].ToString().Trim());
                    dr1["er7"] = fgen.make_double(dt.Rows[i]["er7"].ToString().Trim());
                    dr1["er8"] = fgen.make_double(dt.Rows[i]["er8"].ToString().Trim());
                    dr1["er9"] = fgen.make_double(dt.Rows[i]["er9"].ToString().Trim());
                    dr1["er10"] = fgen.make_double(dt.Rows[i]["er10"].ToString().Trim());
                    for (int j = 1; j < 11; j++)
                    {
                        if (i == 0)
                        {
                            ph_tbl.Columns.Add("ername_" + j + "", typeof(string));
                        }
                        dr1["ername_" + j + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + j + ""] + "' ", "ed_name");
                    }
                    ph_tbl.Rows.Add(dr1);
                    #endregion
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "appoint_letter", "appoint_letter", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82578":
                #region
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("er1", typeof(double));
                ph_tbl.Columns.Add("er2", typeof(double));
                ph_tbl.Columns.Add("er3", typeof(double));
                ph_tbl.Columns.Add("er4", typeof(double));
                ph_tbl.Columns.Add("er5", typeof(double));
                ph_tbl.Columns.Add("er6", typeof(double));
                ph_tbl.Columns.Add("er7", typeof(double));
                ph_tbl.Columns.Add("er8", typeof(double));
                ph_tbl.Columns.Add("er9", typeof(double));
                ph_tbl.Columns.Add("er10", typeof(double));
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//month      
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//mth name
                if (Convert.ToInt32(mq1) > 3 && Convert.ToInt32(mq1) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                mq7 = mq1 + "/" + frm_myear;
                header_n = "Gross Pay Register : " + mq2 + " " + frm_myear;
                SQuery = "select '" + header_n + "' as header,a.empcode as code,b.name,b.fhname,b.married,a.er1,a.er2,a.er3,a.er4,a.er5,a.er6,a.er7,a.er8,a.er9,a.er10  from pay a,empmas b where trim(a.empcode)=trim(b.empcode) and a.branchcd='" + frm_mbr + "' and to_char(a.date_,'mm/yyyy')='" + mq7 + "' and a.grade='" + mq0 + "' order by code";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq0 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);
                //======================
                dr1 = ph_tbl.NewRow();
                for (int j = 1; j < 11; j++)
                {
                    ph_tbl.Columns.Add("ername_" + j + "", typeof(string));
                    dr1["ername_" + j + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + j + ""] + "' ", "ed_name");
                }
                ph_tbl.Rows.Add(dr1);
                //======================
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    ph_tbl.TableName = "headings";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Gross_Sal_paid", "Gross_Sal_paid", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82579":
                #region
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("mbr", typeof(string));
                dtm.Columns.Add("plant", typeof(string));
                dtm.Columns.Add("month", typeof(string));
                dtm.Columns.Add("vdd", typeof(string));
                dtm.Columns.Add("gross_sale", typeof(double));
                dtm.Columns.Add("gross_pay", typeof(double));
                dtm.Columns.Add("percent", typeof(double));
                dtm.Columns.Add("employee", typeof(double));
                dtm.Columns.Add("avg_sale", typeof(double));

                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//branchcd
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//option
                if (mq2 == "Y") //for gross sale
                {
                    mq3 = "bill_tot";
                }
                else //for basic sale
                {
                    mq3 = "amt_sale";
                }
                //SQuery = "select a.branchcd,b.name as plant,sum(a.amt_Sale) as gross_sale,sum(a.grosspay) as grosspay,a.date_,a.vdd  from (select  branchcd,sum(" + mq3 + ") as amt_Sale,0 as grosspay ,to_char(vchdate,'month/yyyy') as date_,to_char(vchdate,'mm/yyyy') as vdd  from sale where branchcd in (" + mq1 + ") and vchdate " + DateRange + " and type like '4%'  group by branchcd,to_char(vchdate,'month/yyyy'),to_char(vchdate,'mm/yyyy')  union all select branchcd,0 as amt_Sale,sum(totern) as grosspay,to_char(date_,'month/yyyy') as date_ ,to_char(date_,'mm/yyyy') as vdd from pay where branchcd in (" + mq1 + ") and date_ " + DateRange + " and grade='" + mq0 + "' group by  branchcd,to_char(date_,'month/yyyy'),to_char(date_,'mm/yyyy') ) a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.date_,a.branchcd,a.vdd,b.name order by A.vdd";
                SQuery = "select a.branchcd,b.name as plant,sum(a.amt_Sale) as gross_sale,sum(a.grosspay) as grosspay,a.date_,a.vdd  from (select  branchcd,sum(" + mq3 + ") as amt_Sale,0 as grosspay ,to_char(vchdate,'MONTHyyyy') as date_,to_char(vchdate,'mm/yyyy') as vdd  from sale where branchcd in (" + mq1 + ") and vchdate " + DateRange + " and type like '4%'  group by branchcd,to_char(vchdate,'MONTHyyyy'),to_char(vchdate,'mm/yyyy')  union all select branchcd,0 as amt_Sale,sum(totern) as grosspay,to_char(date_,'MONTHyyyy') as date_ ,to_char(date_,'mm/yyyy') as vdd from pay where branchcd in (" + mq1 + ") and date_ " + DateRange + " and grade='" + mq0 + "' group by  branchcd,to_char(date_,'MONTHyyyy'),to_char(date_,'mm/yyyy') ) a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' group by a.date_,a.branchcd,a.vdd,b.name order by A.vdd";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//main dt

                SQuery = "select count(empcode) as employee,to_char(date_,'mm/yyyy') as vdd,branchcd from pay where branchcd in (" + mq1 + ") and grade='" + mq0 + "' and date_  " + DateRange + " group by  to_char(date_,'mm/yyyy'),branchcd";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                if (dt.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "vdd", "branchcd"); //MAIN                        
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView viewim = new DataView(dt, "vdd='" + dr0["vdd"].ToString().Trim() + "' and branchcd='" + dr0["branchcd"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt3 = viewim.ToTable();
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            if (fgen.make_double(dt3.Rows[i]["gross_sale"].ToString().Trim()) > 0)//when gross sale na ho to pay wali row b add na ho
                            {
                                dr1 = dtm.NewRow();
                                dr1["header"] = "Monthly Gross Sale & No.of Employees";
                                dr1["mbr"] = dt3.Rows[i]["branchcd"].ToString().Trim();
                                dr1["plant"] = dt3.Rows[i]["plant"].ToString().Trim();
                                dr1["month"] = dt3.Rows[i]["date_"].ToString().Trim();
                                dr1["vdd"] = dt3.Rows[i]["vdd"].ToString().Trim();
                                dr1["gross_sale"] = fgen.make_double(dt3.Rows[i]["gross_sale"].ToString().Trim());
                                dr1["gross_pay"] = fgen.make_double(dt3.Rows[i]["grosspay"].ToString().Trim());
                                dr1["percent"] = fgen.make_double(dr1["gross_pay"].ToString().Trim()) / fgen.make_double(dr1["gross_sale"].ToString().Trim()) * 100;
                                dr1["employee"] = fgen.seek_iname_dt(dt1, "branchcd='" + dt3.Rows[i]["branchcd"].ToString().Trim() + "' and vdd='" + dt3.Rows[i]["vdd"].ToString().Trim() + "'", "employee");
                                dr1["avg_sale"] = fgen.make_double(dr1["gross_sale"].ToString().Trim()) / fgen.make_double(dr1["employee"].ToString().Trim());
                                dtm.Rows.Add(dr1);
                            }
                        }
                    }
                }
                if (dtm.Rows.Count > 0)
                {
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_HR_VS_SALES", "std_HR_VS_SALES", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82582":
                #region OT & INCENTIVES
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//month      
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//mth name
                if (Convert.ToInt32(mq1) > 3)
                {
                    myear = frm_myear;
                }
                else
                {
                    int d = Convert.ToInt32(frm_myear) + 1;
                    myear = Convert.ToString(d);
                }
                mq7 = mq1 + "/" + myear;
                header_n = "Payment Sheet";
                SQuery = "select '" + header_n + "' as header,'" + mq2 + "' as mthname,'" + mq7 + "' as mth,a.workdays,a.offdays,a.vero as over_time,a.empcode,b.name,b.fhname,a.er1 as basic,a.netslry,a.ot from pay a,empmas b where trim(a.empcode)=trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq0 + "' and to_char(a.date_,'mm/yyyy')='" + mq7 + "' order by a.empcode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "OT_Incentives", "OT_Incentives", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82584":
                #region
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("er1", typeof(double));
                ph_tbl.Columns.Add("er2", typeof(double));
                ph_tbl.Columns.Add("er3", typeof(double));
                ph_tbl.Columns.Add("er4", typeof(double));
                ph_tbl.Columns.Add("er5", typeof(double));
                ph_tbl.Columns.Add("er6", typeof(double));
                ph_tbl.Columns.Add("er7", typeof(double));
                ph_tbl.Columns.Add("er8", typeof(double));
                ph_tbl.Columns.Add("er9", typeof(double));
                ph_tbl.Columns.Add("er10", typeof(double));
                ph_tbl.Columns.Add("ded1", typeof(double));
                ph_tbl.Columns.Add("ded2", typeof(double));
                ph_tbl.Columns.Add("ded3", typeof(double));
                ph_tbl.Columns.Add("ded4", typeof(double));
                ph_tbl.Columns.Add("ded5", typeof(double));
                ph_tbl.Columns.Add("ded6", typeof(double));
                ph_tbl.Columns.Add("ded7", typeof(double));
                ph_tbl.Columns.Add("ded8", typeof(double));
                ph_tbl.Columns.Add("ded9", typeof(double));
                ph_tbl.Columns.Add("ded10", typeof(double));
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//grade
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//employee
                header_n = "Yearly Income Statement";
                SQuery = "SELECT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header, a.empcode,a.er1,a.er2,a.er3,a.er4,a.er5,a.er6,a.er7,a.er8,a.er9,a.er10,a.ded1,a.ded2,a.ded3,a.ded4,a.ded5,a.ded6,a.ded7,a.ded8,a.ded9,a.ded10,b.name,b.fhname,a.workdays,to_char(a.date_,'Month YYYY') as monthname,to_char(a.date_,'yyyymm') as monthcode,t.name as grade_name FROM PAY a,empmas b,type t WHERE TRIM(A.BRANCHCD)||TRIM(A.GRADE)||trim(a.empcode)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||trim(b.empcode) and trim(a.grade)=trim(t.type1) and t.id='I' and a.branchcd='" + frm_mbr + "' and a.DATE_ " + xprdRange + " AND a.GRADE='" + mq1 + "'  and a.empcode='" + mq2 + "' order by monthcode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_mbr, SQuery);

                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                mq7 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq1 + "' and ed_fld like 'DED%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq7);

                dr1 = ph_tbl.NewRow();
                for (int j = 1; j < 11; j++)
                {
                    ph_tbl.Columns.Add("ername_" + j + "", typeof(string));
                    dr1["ername_" + j + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + j + ""] + "' ", "ed_name");
                    ph_tbl.Columns.Add("dedname_" + j + "", typeof(string));
                    dr1["dedname_" + j + ""] = fgen.seek_iname_dt(dt3, "ed_fld='" + ph_tbl.Columns["DED" + j + ""] + "' ", "ed_name");
                }
                ph_tbl.Rows.Add(dr1);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    ph_tbl.TableName = "headings";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "annual_income_smry", "annual_income_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82585":
                #region 
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//BRANCHCD
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");//SELECTION
                header_n = "Welfare Fund Statement";
                dt = new DataTable();
                if (mq2 == "YES")
                {
                    SQuery = "select '" + frm_cDt1 + "' as fromdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,a.branchcd||'/'||trim(a.empcode) as empcode,b.name,b.fhname,to_char(b.dtjoin,'dd/mm/yyyy') as dtjoin,sum(a.ded6) as emp_share,(sum(a.ded6)*2) as employer_share,count(a.date_) as date_ from pay a,empmas b where TRIM(A.BRANCHCD)||TRIM(A.GRADE)||trim(a.empcode)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||trim(b.empcode) and  a.branchcd in (" + mq1 + ") and a.grade in (" + mq0 + ") and a.date_ " + xprdRange + " and nvl(a.ded6,0)!=0 group by a.branchcd||'/'||trim(a.empcode),b.name,b.fhname,to_char(b.dtjoin,'dd/mm/yyyy') order by b.name";
                }
                else
                {
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,a.branchcd||'/'||trim(a.empcode) as empcode,b.name,b.fhname,to_char(b.dtjoin,'dd/mm/yyyy') as dtjoin,sum(a.ded6) as emp_share,(sum(a.ded6)*2) as employer_share,count(a.date_) as date_ from pay a,empmas b where TRIM(A.BRANCHCD)||TRIM(A.GRADE)||trim(a.empcode)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||trim(b.empcode) and  a.branchcd in (" + mq1 + ")  and a.grade in (" + mq0 + ") and a.date_ " + xprdRange + " and nvl(a.ded6,0)!=0 group by a.branchcd||'/'||trim(a.empcode),b.name,b.fhname,to_char(b.dtjoin,'dd/mm/yyyy') order by b.name";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Welfare_fund", "Welfare_fund", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F82586":
                #region
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("er1", typeof(string));
                ph_tbl.Columns.Add("er2", typeof(double));
                ph_tbl.Columns.Add("er3", typeof(double));
                ph_tbl.Columns.Add("er4", typeof(double));
                ph_tbl.Columns.Add("er5", typeof(double));
                ph_tbl.Columns.Add("er6", typeof(double));
                ph_tbl.Columns.Add("er7", typeof(double));
                ph_tbl.Columns.Add("er8", typeof(double));
                ph_tbl.Columns.Add("er9", typeof(double));
                ph_tbl.Columns.Add("er10", typeof(double));

                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");//grade
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");//mth
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");//mth name
                if (Convert.ToInt32(mq1) > 3 && Convert.ToInt32(mq1) <= 12)
                {

                }
                else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }

                mq7 = mq1 + "/" + frm_myear;//curr mth value
                int g = Convert.ToInt32(mq1) - 1;
                if (g == 0)
                {
                    mq9 = "12";
                    mq8 = mq9 + "/" + frm_myear;//last mth value
                }
                else
                {
                    if (Convert.ToString(g).Length < 2)
                    {
                        mq9 = "0" + Convert.ToString(g);
                    }
                    else
                    {
                        mq9 = Convert.ToString(g);
                    }
                    mq8 = mq9 + "/" + frm_myear;//last mth value
                }
                header_n = "Comparison of Current Month With Last Month,Person wise Total Rate";

                SQuery = "SELECT  '" + header_n + "' as header,EMPCODE,NAME,FHNAME,MAX(DATES) AS DATES,MAX(DATE1) AS DATE1,SUM(CERATE1) AS ERATE1,SUM(CERATE2) AS ERATE2,SUM(CERATE3) AS ERATE3,SUM(CERATE4) AS ERATE4,SUM(CERATE5) AS ERATE5,SUM(CERATE6) AS ERATE6,SUM(CERATE7) AS ERATE7,SUM(CERATE8) AS ERATE8,SUM(CERATE9) AS ERATE9,SUM(CERATE10) AS ERATE10,SUM(CTOTSAL) AS TOTSAL,SUM(PER1) AS PER1,SUM(PER2) AS PER2,SUM(PER3) AS PER3,SUM(PER4) AS PER4,SUM(PER5) AS PER5,SUM(PER6) AS PER6,SUM(PER7) AS PER7,SUM(PER8) AS PER8,SUM(PER9) AS PER9,SUM(PER10) AS PER10,SUM(PTOT) AS PTOT FROM (select  a.empcode,b.name,b.fhname,to_char(date_,'dd/mm/yyyy') as dates,NULL AS DATE1,a.erate1 as CERATE1,a.erate2 as cerate2,a.erate3 as cerate3,a.erate4 as cerate4,a.erate5 as cerate5,a.erate6 as cerate6,a.erate7 as cerate7,a.erate8 as cerate8,a.erate9 as cerate9,a.erate10 as cerate10,a.totsal as ctotsal,0 as per1,0 as per2,0 as per3,0 as per4, 0 as per5,0 as per6,0 as per7,0 as per8,0 as per9,0 as per10,0 as ptot from pay a,empmas b where TRIM(A.BRANCHCD)||TRIM(A.GRADE)||trim(a.empcode)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq0 + "' and to_char(a.date_,'mm/yyyy')='" + mq7 + "' UNION ALL select a.empcode,b.name,b.fhname,NULL AS DATES,to_char(date_,'dd/mm/yyyy') as date1,0 as CERATE1,0 as cerate2,0 as cerate3,0 as cerate4,0 as cerate5,0as cerate6,0 as cerate7,0 as cerate8,0 as cerate9,0 as cerate10,0 as ctotsal,a.erate1 as per1,a.erate2 as per2,a.erate3 as per3,a.erate4 as per4,a.erate5 as per5,a.erate6 as per6,a.erate7 as per7,a.erate8 as per8,a.erate9 as per9,a.erate10 as per10,a.totsal as ptot  from pay a,empmas b where TRIM(A.BRANCHCD)||TRIM(A.GRADE)||trim(a.empcode)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq0 + "' and to_char(a.date_,'mm/yyyy')='" + mq8 + "' ) GROUP BY EMPCODE,NAME,FHNAME HAVING SUM(CTOTSAL-PTOT)>0 order by NAME";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq5 = "select distinct ed_fld,substr(trim(ed_name),1,7) as ed_name,morder from WB_selmast where branchcd='" + frm_mbr + "' and grade='" + mq0 + "' and ed_fld like 'ER%' AND morder<=11 and nvl(icat,'-')!='Y' order by morder";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5);

                dr1 = ph_tbl.NewRow();
                for (int j = 1; j < 11; j++)
                {
                    ph_tbl.Columns.Add("ername_" + j + "", typeof(string));
                    dr1["ername_" + j + ""] = fgen.seek_iname_dt(dt2, "ed_fld='" + ph_tbl.Columns["ER" + j + ""] + "' ", "ed_name");
                }
                ph_tbl.Rows.Add(dr1);

                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    ph_tbl.TableName = "headings";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Sal_comp_Curr_mth_Cs_Last_Mth", "Sal_comp_Curr_mth_Cs_Last_Mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85232":// SPECIALLY CREATED FOR SAGM
                #region FOR ALL CLIENTS Pay Slip
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("HEADER", typeof(string));
                ph_tbl.Columns.Add("p_email", typeof(string));
                ph_tbl.Columns.Add("monthname", typeof(string));
                ph_tbl.Columns.Add("EMPCODE", typeof(string));
                ph_tbl.Columns.Add("EMPNAME", typeof(string));
                ph_tbl.Columns.Add("FNAME", typeof(string));
                ph_tbl.Columns.Add("SPOUSE", typeof(string));
                ph_tbl.Columns.Add("DESGN", typeof(string));
                ph_tbl.Columns.Add("DEPTT", typeof(string));
                ph_tbl.Columns.Add("PFNO", typeof(string)); //pfno from empmas
                ph_tbl.Columns.Add("UINNO", typeof(string)); //
                ph_tbl.Columns.Add("EL", typeof(double));
                ph_tbl.Columns.Add("SL", typeof(double));
                ph_tbl.Columns.Add("CL", typeof(double));
                ph_tbl.Columns.Add("DAYS_PAID", typeof(double));
                ph_tbl.Columns.Add("PRESENT", typeof(double));
                ph_tbl.Columns.Add("OFF_DAYS", typeof(double));
                ph_tbl.Columns.Add("ABS", typeof(string));
                ph_tbl.Columns.Add("TOT_SAL", typeof(double));
                ph_tbl.Columns.Add("DEDUCTION", typeof(double));
                ph_tbl.Columns.Add("NET_SAL", typeof(double));
                ph_tbl.Columns.Add("TDS", typeof(double));
                ph_tbl.Columns.Add("WRKHRS", typeof(double));
                ph_tbl.Columns.Add("HOLIDAYS", typeof(double));
                ph_tbl.Columns.Add("ESI", typeof(string));
                ph_tbl.Columns.Add("DTJOIN", typeof(string));
                ph_tbl.Columns.Add("ER1", typeof(double));
                ph_tbl.Columns.Add("ER2", typeof(double));
                ph_tbl.Columns.Add("ER3", typeof(double));
                ph_tbl.Columns.Add("ER4", typeof(double));
                ph_tbl.Columns.Add("ER5", typeof(double));
                ph_tbl.Columns.Add("ER6", typeof(double));
                ph_tbl.Columns.Add("ER7", typeof(double));
                ph_tbl.Columns.Add("ER8", typeof(double));
                ph_tbl.Columns.Add("ER9", typeof(double));
                ph_tbl.Columns.Add("ER10", typeof(double));
                ph_tbl.Columns.Add("AR1", typeof(double));
                ph_tbl.Columns.Add("AR2", typeof(double));
                ph_tbl.Columns.Add("AR3", typeof(double));
                ph_tbl.Columns.Add("AR4", typeof(double));
                ph_tbl.Columns.Add("AR5", typeof(double));
                ph_tbl.Columns.Add("AR6", typeof(double));
                ph_tbl.Columns.Add("AR7", typeof(double));
                ph_tbl.Columns.Add("AR8", typeof(double));
                ph_tbl.Columns.Add("AR9", typeof(double));
                ph_tbl.Columns.Add("AR10", typeof(double));

                header_n = "Pay Slip";
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0018", "OPT_ENABLE");
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");//grade
               // mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                mq4 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5"); //empcode
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (Convert.ToInt32(mq5) > 3)
                {
                    myear = frm_myear;
                }
                else
                {
                    int d = Convert.ToInt32(frm_myear) + 1;
                    myear = Convert.ToString(d);
                }
                mq6 = mq5 + "/" + myear;
                SQuery = "select '" + mq3 + "/" + myear + "' as monthname,'" + header_n + "' as header,offdays,empcode,grade,branchcd,to_char(date_,'dd/mm/yyyy') as date_,nvl(totdays,0) as totdays,nvl(absent,0) as absent,nvl(present,0) as present,nvl(shl,0) as holidays,((nvl(TOTDAYS,0)-nvl(ABSENT,0))-nvl(PRESENT,0)) AS OFF_DAYS,nvl(cl,0) as cl,nvl(el,0) as el,nvl(sl,0) as sl,nvl(er1,0) as er1,nvl(er2,0) as er2,nvl(er3,0) as er3,nvl(er4,0) as er4,nvl(er5,0) as er5,nvl(er6,0) as er6,nvl(er7,0) as er7,nvl(er8,0) as er8,nvl(er9,0) as er9,nvl(er10,0) as er10,nvl(erate1,0) as erate1,nvl(erate2,0) as erate2,nvl(erate3,0) as erate3,nvl(erate4,0) as erate4,nvl(erate5,0) as erate5,nvl(erate6,0) as erate6,nvl(erate7,0) as erate7,nvl(erate8,0) as erate8,nvl(erate9,0) as erate9,nvl(erate10,0) as erate10,nvl(ded1,0) as ded1,nvl(ded2,0) as ded2,nvl(ded3,0) ded3,nvl(ded4,0) as ded4,nvl(ded5,0) as ded5,nvl(ded6,0) as ded6,nvl(ded7,0) as ded7,nvl(ded8,0) as ded8,nvl(ded9,0) as ded9,nvl(ded10,0) as ded10,nvl(totern,0) as totern,nvl(totded,0) as totded,nvl(netslry,0) as netslry,nvl(totsal,0) as totsal,nvl(tds,0) as tds,nvl(wrkhrs,0) as wrkhrs,nvl(ar1,0) as ar1,nvl(ar2,0) as ar2,nvl(ar3,0) as ar3,nvl(ar4,0) as ar4,nvl(ar5,0) as ar5,nvl(ar6,0) as ar6,nvl(ar7,0) as ar7,nvl(ar8,0) as ar8,nvl(ar9,0) as ar9,nvl(ar10,0) as ar10 from pay where branchcd='" + frm_mbr + "' and type='10' and grade='" + mq1 + "' and empcode in (" + mq4 + ") and TO_CHAR(DATE_,'MM/YYYY')='" + mq6 + "'";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//maindt ....paydt
                mq5 = ""; mq6 = "";
                mq6 = "select name,empcode,email,d_o_b,wrkhour,deptt_text,desg_text,fhname,pfno,addr1,addr2,city,country,married,uinno,adharno,esino,to_char(dtjoin,'dd/mm/yyyy') as dtjoin from empmas where empcode in (" + mq4 + ") and grade='" + mq1 + "' and branchcd='" + frm_mbr + "'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6); //empmas dt       
                mq5 = "select  distinct grade,er,ename,ded,dname from selmas where grade='" + mq1 + "' order by er";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq5); //for heading

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    #region
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["monthname"] = mq3 + "/" + myear;
                    dr1["EMPCODE"] = dt.Rows[j]["empcode"].ToString().Trim();
                    dr1["abs"] = dt.Rows[j]["absent"].ToString().Trim();
                    dr1["EMPNAME"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "name");
                    dr1["p_email"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "email");
                    dr1["FNAME"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "fhname");
                    dr1["spouse"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "married");
                    dr1["desgn"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "desg_text");
                    dr1["deptt"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "deptt_text");
                    dr1["pfno"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "pfno");
                    dr1["uinno"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "uinno");
                    dr1["esi"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "esino");
                    dr1["dtjoin"] = fgen.seek_iname_dt(dt1, "empcode='" + dr1["EMPCODE"].ToString().Trim() + "'", "dtjoin");
                    dr1["EL"] = dt.Rows[j]["el"].ToString().Trim();
                    dr1["sl"] = dt.Rows[j]["sl"].ToString().Trim();
                    dr1["cl"] = dt.Rows[j]["cl"].ToString().Trim();
                    dr1["DAYS_PAID"] = dt.Rows[j]["totdays"].ToString().Trim();
                    dr1["PRESENT"] = dt.Rows[j]["present"].ToString().Trim();
                    dr1["OFF_DAYS"] = dt.Rows[j]["offdays"].ToString().Trim(); /// holidays
                    dr1["tot_sal"] = dt.Rows[j]["totsal"].ToString().Trim();
                    dr1["deduction"] = dt.Rows[j]["totded"].ToString().Trim();
                    dr1["net_sal"] = dt.Rows[j]["netslry"].ToString().Trim();
                    dr1["tds"] = dt.Rows[j]["tds"].ToString().Trim();
                    dr1["wrkhrs"] = dt.Rows[j]["wrkhrs"].ToString().Trim();//
                    dr1["holidays"] = dt.Rows[j]["holidays"].ToString().Trim();

                    for (int i = 1; i < 11; i++)
                    {
                        if (j == 0)
                        {
                            ph_tbl.Columns.Add("ername_" + i + "", typeof(string));
                            ph_tbl.Columns.Add("dedname_" + i + "", typeof(string));
                            ph_tbl.Columns.Add("ded" + i + "", typeof(double));
                            ph_tbl.Columns.Add("erate" + i + "", typeof(double));
                        }
                        dr1["ername_" + i + ""] = fgen.seek_iname_dt(dt2, "er='" + ph_tbl.Columns["er" + i + ""] + "' ", "ename");
                        dr1["dedname_" + i + ""] = fgen.seek_iname_dt(dt2, "ded='" + ph_tbl.Columns["ded" + i + ""] + "' ", "dname");
                        dr1["erate" + i + ""] = fgen.make_double(dt.Rows[j]["erate" + i + ""].ToString().Trim());
                        dr1["er" + i + ""] = fgen.make_double(dt.Rows[j]["er" + i + ""].ToString().Trim());
                        dr1["ded" + i + ""] = fgen.make_double(dt.Rows[j]["ded" + i + ""].ToString().Trim());
                        dr1["ar" + i + ""] = fgen.make_double(dt.Rows[j]["ar" + i + ""].ToString().Trim());
                    }
                    ph_tbl.Rows.Add(dr1);
                    #endregion
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sal_Slip", "std_Sal_Slip", dsRep, "Salary Slip", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F85158":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select 'PF Register' as heading,'" + fromdt + "' as fromdt,'" + todt + "' as todt,trim(a.empcode) as empcode,(case when nvl(a.age,0)>58 then b.name ||' *' else b.name end) as empname,b.uinno,a.pf_sal,a.ded1,a.ded2,a.ded1+a.ded2 as tot,round((a.ded1/(case when nvl(a.pf_rt_cs,0)=0 then 1 else a.pf_rt_cs/100 end))*(3.67/100)) as epf,0 as fpf,a.pf_amt_cs,to_char(a.date_,'yyyymmdd') as vdd from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.type='10' and a.grade='" + mq1 + "' and a.date_ " + xprdRange + " order by empcode,vdd";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["fpf"] = fgen.make_double(dt.Rows[i]["pf_amt_cs"].ToString()) - fgen.make_double(dt.Rows[i]["epf"].ToString().Trim());
                    }
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "PF_Reg", "PF_Reg", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F85161":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq2) > 3)
                {
                    myear = frm_myear;
                }
                else
                {
                    myear = (Convert.ToInt32(frm_myear) + 1).ToString();
                }
                mq4 = mq2 + "/" + myear;
                SQuery = "select 'ESI Return (Form 6) Return of Contribution Regulation 26)' as heading,'' as fromdt,'' as todt,'" + mq3 + " " + myear + "' as monthname,trim(a.empcode) as empcode,b.name,b.esino,b.esi_disp,a.workdays,a.esi_sal,a.ded3 from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.grade='" + mq1 + "' and to_char(a.date_,'mm/yyyy')='" + mq4 + "' and nvl(a.ded3,0)>0 order by empcode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {                    
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ESI_Ret", "ESI_Ret", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F85162":
                mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1= fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Gratuity Report As On " + mq1 + "";
                SQuery = "select distinct '" + header_n + "' as header, a.empcode,a.name as empname,to_char(a.d_o_b,'dd/mm/yyyy') as dob,to_char(A.dtjoin,'dd/mm/yyyy') as joindt,a.sex as gender,nvl(a.er1,0) as basic from empmas a where a.branchcd='" + frm_mbr + "' and (a.dtjoin<to_date('" + mq1 + "','dd/mm/yyyy') and nvl(trim(a.leaving_Dt),'-')='-') and a.grade in (" + mq0 + ") order by empname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if(dt.Rows.Count>0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Curr_Emp", "std_Curr_Emp", dsRep, header_n);
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
            //conv_pdf(data_set, rptfile);
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
            //conv_pdf(data_set, rptfile);
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

    public void html_body(string party_name, string oth_var1)
    {
        firm = fgenCO.chk_co(frm_cocd); xhtml_tag = "";
        firm = firm.Replace("XXXX", frm_cocd);

        xhtml_tag = xhtml_tag + "<br>Mr./Ms. " + party_name + "<br>";
        if (frm_formID == "F70285")
        {
            xhtml_tag = xhtml_tag + "<h4><B> Dear Sir/Madam, </B></h4>";
        }
        else
        {
            xhtml_tag = xhtml_tag + "<h4><B> Dear Sir/Mam, </B></h4>";
        }
        switch (frm_formID)
        {
            case "F85232":
                subj = "Tejaxo ERP:Salary/Pay Slip from " + firm + "";
                // xhtml_tag = xhtml_tag + "<BR>Please find attached the Pay Slip";
                xhtml_tag = xhtml_tag + "<BR>Enclosed please find the monthly Pay Slip";
                break;
        }
        xhtml_tag = xhtml_tag + "<br><br><b>Thanks & Regards,</b>";
        xhtml_tag = xhtml_tag + "<br><b>" + firm + "</b>";
        if (frm_formID == "F70285")
        {
            xhtml_tag += "<br><br><b> Note:Please do not reply as this is system generated report. For any discrepancy / clarification kindly get in touch with the concerned official's.</b>";
        }
        else
        {
            xhtml_tag = xhtml_tag + "<br><br><br>Note: This is an automatically generated email from Tejaxo ERP, Please do not reply";
        }
        xhtml_tag = xhtml_tag + "</body></html>";
    }

    protected void btnsendmail_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string aname1 = "", mq1 = ""; mq10 = "";
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            DataTable dt = new DataTable();
            DataTable mdt = new DataTable();
            DataTable fdt = new DataTable();
            DataSet data_set = new DataSet();
            data_set = (DataSet)Session["data_set"];
            DataView dv = new DataView(data_set.Tables[0], "", "empcode", DataViewRowState.CurrentRows);
            fdt = data_set.Tables[0];
            mdt = dv.ToTable(true, "empcode", "p_email");
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
                    dv = new DataView(fdt, "empcode='" + dr1["empcode"].ToString().Trim() + "'", "empcode", DataViewRowState.CurrentRows);
                    dt1 = dv.ToTable();
                    foreach (DataRow drdt1 in dt1.Rows)
                    {
                        dr = dt.NewRow();
                        aname1 = drdt1["empname"].ToString().Trim();
                        foreach (DataColumn dcdt in dt.Columns)
                        {
                            if (drdt1[dcdt.ColumnName] == null) dr[dcdt.ColumnName] = 0;
                            else dr[dcdt.ColumnName] = drdt1[dcdt.ColumnName];
                        }
                        dt.Rows.Add(dr);
                    }
                    string repname = "";
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    if (frm_formID == "F85232") repname = "std_Sal_Slip";
                    html_body(aname1, mq1);
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, repname, repname, dsRep, "", "Y");
                    Attachment atchfile = new Attachment(repDoc.ExportToStream(ExportFormatType.PortableDocFormat), frm_cocd + "_" + subj.Replace(" ", "_") + ".pdf");
                    fgen.send_mail(frm_qstr, frm_cocd, "Tejaxo ERP", dr1["p_email"].ToString().Trim(), txtemailcc.Text, txtemailbcc.Text, subj, xhtml_tag, atchfile, "2");
                    // fgen.send_mail(frm_qstr, frm_cocd, "Tejaxo ERP", "mayuri@pocketdriver.in", txtemailcc.Text, txtemailbcc.Text, subj, xhtml_tag, atchfile, "2");
                    repDoc.Close(); repDoc.Dispose(); CrystalReportViewer1.Dispose();
                }
            }
            fgen.send_cookie("Send_Mail", "N");
            // fgen.send_cookie()
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnsendmail');", true);
        }
        catch (Exception ex)
        {
        }
    }
}