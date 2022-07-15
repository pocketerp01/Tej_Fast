using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_prodpm : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2, dtdrsim;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2, dr1; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID;
    DataView view1im;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
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
                co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
            }

            hfhcid.Value = frm_formID;

            if (!Page.IsPostBack)
            {
                col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT BRN||'~'||PRD AS PP FROM FIN_MSYS WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
                if (col1.Length > 1)
                {
                    hfaskBranch.Value = col1.Split('~')[0].ToString();
                    hfaskPrdRange.Value = col1.Split('~')[1].ToString();
                }
                show_data();
            }
        }
    }

    public void show_data()
    {
        HCID = hfhcid.Value.Trim(); SQuery = ""; fgen.send_cookie("MPRN", "N");
        fgen.send_cookie("REPLY", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", "");

        // asking for Branch Consolidate Popup
        if (hfaskBranch.Value == "Y")
        { hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidate report'13'(No for branch wise)"); }
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "89554":
                case "F60121":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "22610A":
                case "22610B":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    break;

                case "F43135":
                case "P15005Y":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", HCID);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F15127":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(header_n, frm_qstr);
            }
        }
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127" || val == "F39221" || val == "F39222" || val == "F39229")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
        }
        // else if branch selection box opens then it comes here
        else if (Request.Cookies["REPLY"].Value.Length > 0)
        {
            value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            switch (val)
            {
                default:
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (hfaskBranch.Value == "Y")
                    {
                        if (value1 == "Y") hfbr.Value = "ABR";
                        else hfbr.Value = "";
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                    }
                    break;
                case "25156":
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                // if we want to ask another popup's
                // Month Popup Instead of Date Range *************
                case "89553":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                    fgen.send_cookie("xid", "FINSYS_S");
                    fgen.send_cookie("srchSql", SQuery);
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    //fgen.Fn_open_sseek("Select Month");
                    break;

            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            value3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

            fromdt = value1;
            todt = value2;
            cldt = value3;

            cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
            cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

            xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
            xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
            xprd2 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
            yr_fld = year;

            co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");


            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            else branch_Cd = "branchcd='" + mbr + "'";

            string vartype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");

            // after prdDmp this will run   

            switch (val)
            {
                case "F39131":
                    // Gate Inward Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type>'14' and a.type like '1%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production (Std) Entry Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;


                case "S06005E":
                    // open graph
                    SQuery = "select month_name,count(*) as  tot_bas,count(*) as tot_qty,mth from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from cquery_Reg a where a.branchcd!='DD'  and a.type='CQ' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Query Graph", "line", "Month Wise", "-", SQuery, "");
                    break;

                case "S15115I":
                    // open drill down form
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                case "F39221"://FOR DISP ONLY
                    SQuery = "Select icode,qty2 as Std_Prodn,0 as Act_prodn,qty1 as Std_Mpwr,0 as Act_Mpwr,0 as Time_Taken from multivch where branchcd!='DD' and type='PD' and col3='A' union all Select substr(icode,1,4),0 as Std_Prodn,qty as Act_prodn,0 as Std_Mpwr,manpwr as Act_Mpwr,time1 as Time_taken from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + "";
                    //SQuery = "Select b.Iname as Prod_Name,trim(a.icode) as Product,sum(Act_prodn) as Act_prodn,sum(Act_Mpwr) as Act_Mpwr,sum(round(Time_Taken/60,2)) as Time_Taken,round((sum(Act_prodn)/sum(round(Time_Taken/60,2)))/sum(Act_Mpwr),0) as Avg_Hr_output,  sum(Std_Prodn) As Std_Hrly_Prodn,round((round((sum(Act_prodn)/sum(round(Time_Taken/60,2)))/sum(Act_Mpwr),0)/ sum(Std_Prodn))*100,2) as Efficiency  from (" + SQuery + ")a, item b where trim(A.icode)=trim(B.icode) group by b.iname,trim(a.icode) order by b.Iname,trim(a.icode)";
                    SQuery = "Select b.Iname as Prod_Name,trim(a.icode) as Product,sum(Act_prodn) as Act_prodn,sum(Act_Mpwr) as Act_Mpwr,sum(round(Time_Taken/60,2)) as Time_Taken ,(CASE WHEN sum(Act_Mpwr)>0 THEN round((sum(Act_prodn)/sum(round(Time_Taken/60,2)))/sum(Act_Mpwr),0) ELSE 0 END) as Avg_Hr_output ,  sum(Std_Prodn) As Std_Hrly_Prodn,(CASE WHEN sum(Act_Mpwr)>0 THEN round((round((sum(Act_prodn)/sum(round(Time_Taken/60,2)))/sum(Act_Mpwr),0)/ sum(Std_Prodn))*100,2) ELSE 0 END ) as Efficiency from (" + SQuery + ")a, item b where trim(A.icode)=trim(B.icode) group by b.iname,trim(a.icode) order by b.Iname,trim(a.icode)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Manpower Efficiency Report During " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F39222":// FOR DISP ONLY
                    SQuery = "Select icode,col3 as Machine,qty2 as Std_Prodn,0 as Act_prodn,qty1 as Std_Mpwr,0 as Act_Mpwr,0 as Time_Taken from multivch where " + branch_Cd + " and type='/D'  union all Select substr(icode,1,4),col20,0 as Std_Prodn,qty as Act_prodn,0 as Std_Mpwr,manpwr as Act_Mpwr,time1 as Time_taken from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + "";
                    SQuery = "Select b.Iname as Prod_Name,trim(a.Machine) as MAchine,sum(Act_prodn) as Act_prodn,sum(round(Time_Taken/60,2)) as Time_Taken,sum(Std_Prodn) As Std_Hrly_Prodn,sum(Std_Prodn)*sum(round(Time_Taken/60,2)) as Std_Prodn,(case when (sum(Std_Prodn)*sum(round(Time_Taken/60,2)))>0 then round((sum(Act_prodn)/(sum(Std_Prodn)*sum(round(Time_Taken/60,2))))*100,2) else 0 end) as Efficiency,trim(a.icode) as Product  from (" + SQuery + ")a, item b where trim(A.icode)=trim(B.icode) group by b.iname,trim(a.icode),trim(a.Machine) order by b.Iname,trim(a.icode)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("M/C Efficiency Report During " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F39229":// FOR DISP ONLY

                    DataTable dtm = new DataTable();
                    dtm.Columns.Add("FSTR", typeof(string));
                    dtm.Columns.Add("SRNO", typeof(string));
                    dtm.Columns.Add("Mould_No", typeof(string));
                    dtm.Columns.Add("Component Name", typeof(string));
                    dtm.Columns.Add("Guage", typeof(string));
                    dtm.Columns.Add("Run Cavity", typeof(string));
                    dtm.Columns.Add("Total Cavity", typeof(string));
                    dtm.Columns.Add("Component Wt.", typeof(string));
                    dtm.Columns.Add("Runner Wt.", typeof(string));
                    dtm.Columns.Add("Gross Wt.", typeof(string));
                    dtm.Columns.Add("Component/kgs", typeof(string));
                    dtm.Columns.Add("Total Shots", typeof(string));
                    dtm.Columns.Add("Total Component Wt.", typeof(string));
                    dtm.Columns.Add("Total Runner Wt.", typeof(string));
                    dtm.Columns.Add("Total raw used", typeof(string));
                    dtm.Columns.Add("Raw Name", typeof(string));
                    SQuery = "SELECT  DISTINCT A.BRANCHCD,SUM(C.IOQTY) AS RUNNER_WIDTH, rtrim(xmlagg(xmlelement(e,d.iname||',')).extract('//text()').extract('//text()'),',') as raw_name,B.INAME,B.IWEIGHT AS NET_WT,B.NO_PROC AS GUAGE,rtrim(xmlagg(xmlelement(e,c.ibcode||',')).extract('//text()').extract('//text()'),',') as ibcode,A.ICODE,A.ENAME AS MOULDNO,A.NOUPS AS ACTUAL_SHOT,SUM(A.LMD) AS CAVITY,SUM(A.BCD) AS RUN_CAVITY,SUM((A.IQTYIN)+(A.MLT_LOSS)) AS TOTAL_PRODUCTION  FROM  PROD_SHEET A,ITEM B,ITEMOSP C,ITEM D WHERE A." + branch_Cd + " AND A.TYPE='90'  AND A.VCHDATE  " + xprdrange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.IBCODE)=TRIM(D.ICODE) AND C.TYPE='BM' GROUP BY B.INAME,B.IWEIGHT,B.NO_PROC,A.BRANCHCD,A.ICODE,A.ENAME ,A.NOUPS";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    mq0 = "SELECT BRANCHCD,RCODE,SUM(IQTYOUT) AS input  FROM IVOUCHER WHERE " + branch_Cd + " and  TYPE='39'  AND  vchdate " + xprdrange + " GROUP BY BRANCHCD,RCODE";//REAL                 
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "BRANCHCD", "ICODE"); //                      
                        int i5 = 1;
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "BRANCHCD='" + dr0["BRANCHCD"] + "' AND  ICODE='" + dr0["ICODE"] + "'", "", DataViewRowState.CurrentRows);
                            DataTable dty = new DataTable();
                            dty = viewim.ToTable();
                            DataRow dr1 = dtm.NewRow();
                            Double db3 = 0, db2 = 0, db9 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db1 = 0; string ded1 = "";
                            dr1["SRNO"] = i5++;
                            for (int i = 0; i < dty.Rows.Count; i++)
                            {
                                dr1["Mould_No"] = dty.Rows[i]["MOULDNO"].ToString().Trim();
                                dr1["Component Name"] = dty.Rows[i]["INAME"].ToString().Trim();
                                dr1["Guage"] = dty.Rows[i]["GUAGE"].ToString().Trim();
                                dr1["Run Cavity"] = dty.Rows[i]["RUN_CAVITY"].ToString().Trim();
                                dr1["Total Cavity"] = dty.Rows[i]["CAVITY"].ToString().Trim();
                                dr1["Component Wt."] = dty.Rows[i]["NET_WT"].ToString().Trim();
                                db1 = fgen.make_double(dr1["Component Wt."].ToString().Replace("Infinity", "0").Trim());
                                dr1["Runner Wt."] = dty.Rows[i]["RUNNER_WIDTH"].ToString().Trim();
                                db2 = fgen.make_double(dr1["Runner Wt."].ToString().Trim());
                                dr1["Gross Wt."] = db1 + db2;
                                dr1["Total Shots"] = dty.Rows[i]["TOTAL_PRODUCTION"].ToString().Trim();
                                db3 = fgen.make_double(dr1["Total Shots"].ToString().Trim());
                                dr1["Component/kgs"] = Math.Round(1000 / db1, 5).ToString().Replace("Infinity", "0");
                                dr1["Total Component Wt."] = Math.Round(db3 * db1, 5).ToString().Replace("Infinity", "0");
                                dr1["Total Runner Wt."] = Math.Round(db3 * db2, 5);
                                ded1 = fgen.seek_iname_dt(dt, "icode='" + dty.Rows[i]["icode"].ToString().Trim() + "'", "icode"); //fg item
                                db4 = fgen.make_double(fgen.seek_iname_dt(dt1, "rcode='" + ded1 + "'", "input"));
                                dr1["Total raw used"] = db4;
                                dr1["Raw Name"] = dty.Rows[i]["RAW_NAME"].ToString().Trim();
                            }
                            dtm.Rows.Add(dr1);
                        }
                    }
                    Session["seeksql"] = "";
                    Session["Data11"] = dtm;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dtm;
                    fgen.Fn_open_rptlevelJS("Total Runner Consumption Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                #region Made by Madhvi On 03 Aug 2018
                case "F39153":
                    header_n = "Prod,Rej,OEE Day Wise Report";
                    SQuery = "select to_CHAR(a.vchdate,'dd/mm/yyyy') as Voucher_Date,sum(to_number(tempr)*total*bcd) as Target_Production,sum(a.iqtyin+a.mlt_loss) as Total_production,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_production,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Production_efficiency,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hours_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Utilization_Ratio from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " group by a.branchcd,a.vchdate order by a.branchcd,a.vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE Day Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39154":
                    header_n = "Prod,Rej,OEE Shift Wise Report";
                    SQuery = "select to_CHAR(a.vchdate,'dd/mm/yyyy') as Dated,a.Var_Code as shift_Name,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,a.var_Code,a.vchdate order by a.branchcd,a.vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE Shift Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39155":
                    header_n = "Prod,Rej,OEE Month Wise Report";
                    SQuery = "select to_char(a.vchdate,'YYYY MONTH') as Mth_name,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Tot_Dntime,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio,to_char(a.vchdate,'YYYYMM') as mth_sr,round((sum(a.num1)/60),2) as dt_rsn1,round((sum(a.num2)/60),2) as dt_rsn2,round((sum(a.num3)/60),2) as dt_rsn3,round((sum(a.num4)/60),2) as dt_rsn4,round((sum(a.num5)/60),2) as dt_rsn5,round((sum(a.num6)/60),2) as dt_rsn6,round((sum(a.num7)/60),2) as dt_rsn7,round((sum(a.num8)/60),2) as dt_rsn8,round((sum(a.num9)/60),2) as dt_rsn9,round((sum(a.num10)/60),2) as dt_rsn10,round((sum(a.num11)/60),2) as dt_rsn11,round((sum(a.num12)/60),2) as dt_rsn12 from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,to_char(a.vchdate,'YYYYMM'),to_char(a.vchdate,'YYYY MONTH') order by a.branchcd,to_char(a.vchdate,'YYYYMM')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE Month Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39156":
                    header_n = "Prod,Rej,OEE Day+Supr Wise Report";
                    SQuery = "select to_CHAR(a.vchdate,'dd/mm/yyyy') as Dated,trim(a.exc_time) as Super_Visor,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio,to_CHAR(a.vchdate,'YYYYMMDD') AS VDD from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,to_CHAR(a.vchdate,'dd/mm/yyyy'),trim(a.exc_time),to_CHAR(a.vchdate,'YYYYMMDD') order by a.branchcd,VDD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE Day+Supr Wise Report From " + fromdt + "  To " + todt + "", frm_qstr);
                    break;

                case "F39157":
                    header_n = "Prod,Rej,OEE M/C Wise Report";
                    SQuery = "select a.Ename as Machine,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,a.Ename order by a.Ename";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE M/C Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39158":
                    header_n = "Prod,Rej,OEE Item Wise Report";
                    SQuery = "select b.Iname as Item_Name,b.Cpartno,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio,trim(a.icodE) as ERP_code from prod_sheet a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,b.Iname,trim(a.icode),b.cpartno having sum(to_number(tempr)*total*bcd)>0 and sum(a.iqtyin+a.mlt_loss)>0 order by b.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE Item Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39159":
                    header_n = "Prod,Rej,OEE Year Wise Report";
                    SQuery = "select to_char(a.vchdate,'YYYY') as year_name,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,to_char(a.vchdate,'YYYY') order by a.branchcd,to_char(a.vchdate,'YYYY')";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE Year Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39160":
                    header_n = "Prod,Rej,OEE M/C+Item Wise Report";
                    SQuery = "select a.Ename,b.Iname as Item_Name,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio,b.Cpartno,trim(a.icodE) as ERP_code from prod_sheet a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + "  group by a.branchcd,a.ename,b.Iname,trim(a.icode),b.cpartno having sum(to_number(tempr)*total*bcd)>0 and sum(a.iqtyin+a.mlt_loss)>0 order by a.ename,b.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE M/C+Item Wise Report From " + fromdt + "  To " + todt + "", frm_qstr);
                    break;

                case "F39161":
                    header_n = "Prod,Rej,OEE M/C+Item+Shift Wise Report";
                    SQuery = "select to_CHAR(a.vchdate,'dd/mm/yyyy') as dated,a.Var_Code as shift_Name,a.Ename,b.Iname as Item_Name,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,round((((sum(a.iqtyin+a.mlt_loss))-(sum(a.iqtyin)))/sum(a.iqtyin+a.mlt_loss))*1000000,0) as PPM,round(((sum(a.iqtyin+0))/(sum(to_number(tempr)*total*bcd)))*100,2) as Prod_ef,((round(((sum(to_number(tempr)*total*bcd)))/((sum(to_number(tempr)*total*lmd))),2)*100)*(round((sum(iqtyin))/((sum(to_number(tempr)*total*bcd))),2)*100))/100 as OEE,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod,round(round(((sum(a.total)-sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60)),2)/sum(a.total),2)*100 as Util_Ratio,b.Cpartno,sum(is_number(nvl(a.glue_code,'0')))  as MPWr,trim(a.icodE) as ERP_code from prod_sheet a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " group by a.branchcd,a.vchdate,a.Var_Code,a.ename,b.Iname,trim(a.icode),b.cpartno having sum(to_number(tempr)*total*bcd)>0 and sum(a.iqtyin+a.mlt_loss)>0 order by a.vchdate,a.var_Code,a.ename,b.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Prod,Rej,OEE M/C+Item+Shift Wise Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39162":
                    header_n = "Work Order Compliance Report";
                    SQuery = "select to_char(a.Vchdate,'dd/mm/yyyy') as Dated,sum(nvl(plan,0)) as WO_Qty,sum(nvl(prodn,0)) as Prd_qty,sum(nvl(rejn,0)) as Rej_Qty,round(sum(nvl(prodn,0))/decode(sum(nvl(plan,0)),0,sum(nvl(prodn,0)),sum(nvl(plan,0))),2)*100 as WO_Compliance  from (Select vchdate,Icode,iqtyout as plan,0 as prodn,0 as rejn from prod_sheet a where branchcd='" + mbr + "' and type='20' and vchdate " + xprdrange + " union all Select vchdate,Icode,0 as plan,iqtyin as prodn,mlt_loss as rejn from prod_sheet a where branchcd='" + mbr + "' and type='61' and vchdate " + xprdrange + ") a group by a.vchdate order by a.vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39163":
                    header_n = "Rejection (First + Second Stage) Report";
                    SQuery = "select b.iname as Item_Name,sum(iqtyin)+sum(r1) as Tot_Prod,sum(r1) as Mld_Rejn,sum(r2) as Vend_Rejn,sum(r3) as Inh_DfRejn,sum(rwrk)as Rework,sum(r1)+sum(r2)+sum(r3)-sum(rwrk) AS Tot_rejn,sum(iqtyin)+sum(Rwrk)+sum(r1)-(sum(r1)+sum(r2)+sum(r3)) as Net_prodn,(sum(iqtyin*b.irate)+sum(Rwrk*b.irate)+sum(r1*b.irate)-(sum(r1*b.irate)+sum(r2*b.irate)+sum(r3*b.irate)))/100000 as Net_prd_val,(sum(r1*b.irate)+sum(r2*b.irate)+sum(r3*b.irate)-sum(rwrk*b.irate)) as Rejn_Val,(Case when sum(iqtyin)+sum(r1)>0  then round(((sum(r1)+sum(r2)+sum(r3)-sum(rwrk))/(sum(iqtyin)+sum(r1)))*100,2) else 0 end) as RPercent,b.irate,b.cpartno as Part_No,trim(a.icode) as ERP_code  from (Select icode,iqtyin,mlt_loss as r1,0 as r2,0 as r3,0 as rwrk from prod_sheet where branchcd='" + mbr + "' and type='62' and vchdate " + xprdrange + " and mlt_loss>0  union all Select icode,0 as iqtyin,0 as r1,0 as r2,0 as r3,iqtyout as rwrk from ivoucher where branchcd='" + mbr + "' and type='3A' and stage='6R' and vchdate " + xprdrange + " and iqtyout>0  union all Select icode,0 as iqtyin,0 as r1,mlt_loss as r2,0 as r3,0 as rwrk from prod_sheet where branchcd='" + mbr + "' and type='91' and vchdate " + xprdrange + " and mlt_loss>0 union all Select icode,0 as iqtyin,0 as r1,0 as r2,irate as r3,0 as rwrk from costestimate where branchcd='" + mbr + "' and type in ('6A','6B','6C','6D','6E') and vchdate " + xprdrange + " and irate>0) a , item b where trim(A.icode)=trim(B.icode) group by b.iname,b.cpartno,b.irate,trim(a.icode) order by B.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39164":
                    header_n = "Consumption Done Report";
                    SQuery = "SELECT b.INAME AS Produced_Item,C.iname as Consumed_item,a.iqty_chlwt as Prod_pcs,a.iqty_wt as Wt_pc,A.iqtyout as Wt_used,b.cpartno,a.vchnum,a.vchdate,A.rcode as Prod_code,A.icode as RM_code from ivoucher a, item b,item c where trim(a.rcodE)=trim(b.icode) and trim(a.icodE)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='39' and a.vchdate " + xprdrange + " and a.iopr='61' order by a.vchdate,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39165":
                    header_n = "Month Wise Productin Quantitative Analysis Report";
                    SQuery = "Select Item,Partno,to_char(sum(April)+sum(may)+sum(june)+sum(july)+sum(august)+sum(sept)+sum(oct)+sum(nov)+sum(dec)+sum(jan)+sum(feb)+sum(mar),'99,99,99,999') as Total,to_char(sum(April),'99,99,99,999') as April,to_char(sum(May),'99,99,99,999') as May,to_char(sum(June),'99,99,99,999') as June,to_Char(sum(July),'99,99,99,999') as July,to_char(sum(August),'99,99,99,999') as August,to_Char(sum(Sept),'99,99,99,999') as Sept,to_char(sum(oct),'99,99,99,999') as Oct,to_Char(sum(Nov),'99,99,99,999') as Nov,to_char(sum(Dec),'99,99,99,999') as Dec,to_Char(sum(Jan),'99,99,99,999') as Jan,to_char(sum(Feb),'99,99,99,999') as Feb,to_Char(sum(Mar),'99,99,99,999') as Mar,icode from (Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno,decode(to_chaR(vchdate,'yyyymm'),201804,sum(a.iqtyin),0) as April,decode(to_chaR(vchdate,'yyyymm'),201805,sum(a.iqtyin),0) as May,decode(to_chaR(vchdate,'yyyymm'),201806,sum(a.iqtyin),0) as June,decode(to_chaR(vchdate,'yyyymm'),201807,sum(a.iqtyin),0) as July,decode(to_chaR(vchdate,'yyyymm'),201808,sum(a.iqtyin),0) as August,decode(to_chaR(vchdate,'yyyymm'),201809,sum(a.iqtyin),0) as Sept,decode(to_chaR(vchdate,'yyyymm'),201810,sum(a.iqtyin),0) as Oct,decode(to_chaR(vchdate,'yyyymm'),201811,sum(a.iqtyin),0) as Nov,decode(to_chaR(vchdate,'yyyymm'),201812,sum(a.iqtyin),0) as Dec ,decode(to_chaR(vchdate,'yyyymm'),201901,sum(a.iqtyin),0) as Jan,decode(to_chaR(vchdate,'yyyymm'),201902,sum(a.iqtyin),0) as Feb,decode(to_chaR(vchdate,'yyyymm'),201903,sum(a.iqtyin),0) as Mar,a.icode from prod_sheet a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd ='" + mbr + "' and a.vchdate " + xprdrange + " and substr(a.type,1,2)='" + vartype + "' group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'yyyymm')  ) group by item,partno,icode order by item";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " From " + fromdt + " To " + todt + "", frm_qstr);
                    break;
                #endregion

                // Made by Akshay On 03 Aug 2018
                case "F39166":
                case "F39141":
                    header_n = "Production Register";
                    SQuery = "Select A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ename,b.iname,b.cpartno as Partno,a.iqtyin+mlt_loss as total,a.iqtyin as Okqty,a.mlt_loss as Rej_qty,a.oee_R,a.icode,a.ent_by,a.mseq as Stk_phr from prod_sheet a, item b where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " and trim(a.icode)=trim(b.icode) order by a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production Register From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;

                case "F39167":
                    header_n = "Month Wise Tool Change";
                    SQuery = "select MthsName,sum(No_Of_Tc) as No_Of_Tc,sum(Time_for_TC) as Time_for_TC,round(Avg(Avg_Tc_Time),2) as Avg_Tc_Time,mths from (Select B.cpartno AS Part_no,B.Iname as Part_Name,A.Ename as Machine_Name,(count(a.num1)*a.fm_fact) as No_Of_Tc,sum(a.num1) Time_for_TC,round(sum(a.num1)/(count(a.num1)*a.fm_fact),2) as Avg_Tc_Time,trim(A.icode) as ERP_Code,to_chaR(a.vchdate,'yyyymm') as mths,to_chaR(a.vchdate,'yyyy') as yrs,to_chaR(a.vchdate,'yyyy Month') as mthsname from prod_sheet a,item b where a.num1>0 and a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " and trim(A.icode)=trim(B.icode) GROUP BY A.ename,a.fm_fact,b.cpartno,b.iname,trim(A.icode),to_chaR(a.vchdate,'yyyymm'),to_chaR(a.vchdate,'yyyy'),to_chaR(a.vchdate,'yyyy Month') ) Group by mths,MthsName order by Mths";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Month Wise Tool Change From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;

                case "F39168":
                    header_n = "Year Wise Tool Change";
                    SQuery = "select Yrs as Year_Heading,sum(No_Of_Tc) as No_Of_Tc,sum(Time_for_TC) as Time_for_TC,round(Avg(Avg_Tc_Time),2) as Avg_Tc_Time  from (Select B.cpartno AS Part_no,B.Iname as Part_Name,A.Ename as Machine_Name,(count(a.num1)*a.fm_fact) as No_Of_Tc,sum(a.num1) Time_for_TC,round(sum(a.num1)/(count(a.num1)*a.fm_fact),2) as Avg_Tc_Time,trim(A.icode) as ERP_Code,to_chaR(a.vchdate,'yyyymm') as mths,to_chaR(a.vchdate,'yyyy') as yrs,to_chaR(a.vchdate,'yyyy Month') as mthsname from prod_sheet a,item b where a.num1>0 and a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " and trim(A.icode)=trim(B.icode) GROUP BY A.ename,a.fm_fact,b.cpartno,b.iname,trim(A.icode),to_chaR(a.vchdate,'yyyymm'),to_chaR(a.vchdate,'yyyy'),to_chaR(a.vchdate,'yyyy Month') ) Group by Yrs order by Yrs";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Year Wise Tool Change From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;

                case "F39169":
                    header_n = "Item Wise Tool Change";
                    //SQuery = "select Part_no,Part_Name,Machine_Name,sum(No_Of_Tc) as No_Of_Tool_change,sum(Time_for_TC) as Time_for_Tool_change,round(Avg(Avg_Tc_Time),2) as Avg_Tool_change_Time  from (Select B.cpartno AS Part_no,B.Iname as Part_Name,A.Ename as Machine_Name,(count(a.num1)*a.fm_fact) as No_Of_Tool_change,sum(a.num1) Time_for_TC,round(sum(a.num1)/(count(a.num1)*a.fm_fact),2) as Avg_Tool_change_Time,trim(A.icode) as ERP_Code,to_chaR(a.vchdate,'yyyymm') as mths,to_chaR(a.vchdate,'yyyy') as yrs,to_chaR(a.vchdate,'yyyy Month') as mthsname from prod_sheet a,item b where a.num1>0 and a.branchcd='" + mbr + "' and a.type='"+vartype+"' and a.vchdate " + xprdrange + " and trim(A.icode)=trim(B.icode) GROUP BY A.ename,a.fm_fact,b.cpartno,b.iname,trim(A.icode),to_chaR(a.vchdate,'yyyymm'),to_chaR(a.vchdate,'yyyy'),to_chaR(a.vchdate,'yyyy Month') ) Group by Part_no,Part_Name,Machine_Name order by Part_no,Part_Name,Machine_Name";
                    SQuery = "select Part_no,Part_Name,Machine_Name,sum(No_Of_Tool_change) as No_Of_Tool_change,sum(Time_for_TC) as Time_for_Tool_change,round(Avg(Avg_Tool_change_Time),2) as Avg_Tool_change_Time  from (Select B.cpartno AS Part_no,B.Iname as Part_Name,A.Ename as Machine_Name,(count(a.num1)*a.fm_fact) as No_Of_Tool_change,sum(a.num1) Time_for_TC,round(sum(a.num1)/(count(a.num1)*a.fm_fact),2) as Avg_Tool_change_Time,trim(A.icode) as ERP_Code,to_chaR(a.vchdate,'yyyymm') as mths,to_chaR(a.vchdate,'yyyy') as yrs,to_chaR(a.vchdate,'yyyy Month') as mthsname from prod_sheet a,item b where a.num1>0 and a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " and trim(A.icode)=trim(B.icode) GROUP BY A.ename,a.fm_fact,b.cpartno,b.iname,trim(A.icode),to_chaR(a.vchdate,'yyyymm'),to_chaR(a.vchdate,'yyyy'),to_chaR(a.vchdate,'yyyy Month') ) Group by Part_no,Part_Name,Machine_Name order by Part_no,Part_Name,Machine_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Wise Tool Change From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;

                case "F39170":
                    header_n = "Supervisor Wise Report";
                    SQuery = "Select trim(Employee)as Employee,sum(TargetPrd) as TargetPrd,sum(Tot_prod) as Tot_prod,round((sum(Tot_prod)/sum(TargetPrd))*100,2) as Prd_Eff,sum(Rejection) as Rejection,sum(OK_prod) as OK_prod,sum(Hr_worked) as Hr_worked,sum(Non_Prod ) as Non_Prod from ( select a.SUBCODE as Employee,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod  from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " group by a.branchcd,a.SUBCODE ) group by trim(Employee) order by trim(Employee)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supervisor Wise Report From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;

                case "F39181":
                    header_n = "Supervisor ,Shift Wise Report";
                    SQuery = "Select trim(Employee)as Employee,var_code as Shift_cd,sum(TargetPrd) as TargetPrd,sum(Tot_prod) as Tot_prod,round((sum(Tot_prod)/sum(TargetPrd))*100,2) as Prd_Eff,sum(Rejection) as Rejection,sum(OK_prod) as OK_prod,sum(Hr_worked) as Hr_worked,sum(Non_Prod ) as Non_Prod from ( select a.subcode as Employee,a.var_code,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod  from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " group by a.branchcd,a.var_code,a.subcode  ) group by trim(Employee),var_code order by trim(Employee),var_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supervisor Shift Wise Report From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;

                case "F39182":
                    header_n = "Supervisor ,Shift ,Machine Wise Report";
                    SQuery = "Select trim(Employee)as Employee,var_code as Shift_cd,Ename as Machine,sum(TargetPrd) as TargetPrd,sum(Tot_prod) as Tot_prod,round((sum(Tot_prod)/sum(TargetPrd))*100,2) as Prd_Eff,sum(Rejection) as Rejection,sum(OK_prod) as OK_prod,sum(Hr_worked) as Hr_worked,sum(Non_Prod ) as Non_Prod from ( select a.subcode as Employee,a.ename,a.var_code,sum(to_number(tempr)*total*bcd) as TargetPrd,sum(a.iqtyin+a.mlt_loss) as Tot_prod,sum(a.mlt_loss) as Rejection,sum(a.iqtyin) as OK_prod,sum(a.total*a.fm_fact) as Hr_worked,round((sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10+a.num11+a.num12)/60),2) as Non_Prod  from prod_sheet a where a.branchcd='" + mbr + "' and a.type='" + vartype + "' and a.vchdate " + xprdrange + " group by a.branchcd,a.var_code,a.subcode,a.ename ) group by trim(Employee),var_code,ename order by trim(Employee),var_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supervisor Shift , Machnine Wise Report From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;


                case "F39183": // Item Below Min. Level (Component Store)
                    wip_stk_vw_disp();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,STG03 AS COMP FROM wipcolstkw_" + mbr + " WHERE SUBSTR(ICODE,1,1) in ( '7','9')  order by icode");
                    mq0 = "SELECT TRIM(ICODE) AS ICODE,INAME AS ITEM,CPARTNO AS PART_NO,IMIN AS MIN_LEVEL,0 AS STOCK,0 AS SHORT FROM ITEM WHERE LENGTH(TRIM(ICODE))=8 AND SUBSTR(TRIM(ICODE),1,1) IN ('7','9') ORDER BY ICODE";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        double db1 = 0; double db2 = 0; double db3 = 0;
                        db1 = fgen.make_double(fgen.seek_iname_dt(dt, "ICODE='" + dr["ICODE"].ToString().Trim() + "'", "COMP"));
                        dr["STOCK"] = db1;
                        db2 = fgen.make_double(dr["MIN_LEVEL"].ToString());
                        db3 = db2 - db1;
                        dr["SHORT"] = db3;
                    }
                    Session["send_dt"] = dt1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevelJS("Item Below Min. Level (Component Store)", frm_qstr);
                    break;

                case "F43135_old": // OLD OEE REPORT - BUPL
                    #region
                    dt1 = new DataTable();
                    dt1.Columns.Add("VOUCHER_NO", typeof(string));
                    dt1.Columns.Add("VOUCHER_DATE", typeof(string));
                    dt1.Columns.Add("SHIFT", typeof(string));
                    dt1.Columns.Add("SHIFTCODE", typeof(double));
                    dt1.Columns.Add("PROCESS", typeof(string));
                    //dt1.Columns.Add("AVAILABILITY", typeof(double));
                    //dt1.Columns.Add("PERFORMANCE", typeof(double));
                    //dt1.Columns.Add("QUALITY", typeof(double));
                    dt1.Columns.Add("SHIFT_TIME", typeof(double));

                    dt1.Columns.Add("ALLOWED_DNTM", typeof(double));
                    dt1.Columns.Add("AVAILABLE_TIME", typeof(double));
                    dt1.Columns.Add("TOTPRODN_TIME", typeof(double));
                    dt1.Columns.Add("AVAIL_PRODN_TIM", typeof(double));
                    dt1.Columns.Add("DOWN_TIME", typeof(double));
                    dt1.Columns.Add("PROD_CYCLE_TIME", typeof(double));
                    dt1.Columns.Add("TOT_PROD", typeof(double));
                    dt1.Columns.Add("TOT_REJ", typeof(double));
                    dt1.Columns.Add("CYCLE_TIME", typeof(double));
                    //dt1.Columns.Add("FEED_ALLDT", typeof(double));
                    dt1.Columns.Add("AVAILABILITY_RATE", typeof(double));
                    dt1.Columns.Add("PERFORMANCE_RATE", typeof(double));
                    dt1.Columns.Add("QUALITY_RATE", typeof(double));
                    dt1.Columns.Add("OEE_TOTAL", typeof(double));

                    //SQuery = "select vchdate ,Name,(round((prdn_t2/AVAILT)*1,2)*round(((XTOTPR+XTOTRJ)/(PRD_CYCLET*prdn_t2)),2)*round(((XTOTPR-XTOTRJ)/XTOTPR)*1,2))*100 as Tot_OEE,round((prdn_t2/AVAILT)*100,2) as Availability,round(((XTOTPR+XTOTRJ)/(PRD_CYCLET*prdn_t2))*100,2) as Performance,round(((XTOTPR-XTOTRJ)/XTOTPR)*100,2) as Quality,shf_tm as Shift_time,all_dt as Allowed_Dntm,AVAILT as Available_time,PRDn_t as TotProdn_time,PRDn_t2 Avail_prodn_tim,DNT as Down_time,PRD_CYCLET as Prod_cycle_time,XTOTPR as Tot_prod,XTOTRJ as Tot_rej,shftcode,feed_alldt from (select a.vchnum,a.vchdate ,b.name,a.shftcode,max(to_number(b.place)) as shf_tm,max(to_number(b.balop)) as all_dt,max(to_number(a.lmd)) as feed_alldt,max(to_number(b.place)-to_number(b.balop)) as AVAILT,sum((case when a.a5>0 then a.un_melt else 0 end )) as PRDn_t,(sum((case when a.a5>0 then a.bcd else 0 end ))) as PRDn_t2,sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10) as DNT,sum(round(a.un_melt/a.a5,3)) as PRD_CYCLET,sum(a.a5*1) as XTOTPR,sum(a.a4*1) as XTOTRJ from prod_Sheet a, type b where a.branchcd='06' and a.type='86' and a.vchdate  between to_Date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')  and trim(A.shftcode)=trim(B.type1) and b.id='D' group by a.vchnum,b.name,a.vchdate ,a.shftcode order by a.vchdate) where AVAILT>0 order by vchdate ,shftcode";
                    SQuery = "select vchdate ,Name,(round((prdn_t2/AVAILT)*1,2)*round(((XTOTPR+XTOTRJ)/(PRD_CYCLET*prdn_t2)),2)*round(((XTOTPR-XTOTRJ)/XTOTPR)*1,2))*100 as Tot_OEE,round((prdn_t2/AVAILT)*100,2) as Availability,round(((XTOTPR+XTOTRJ)/(PRD_CYCLET*prdn_t2))*100,2) as Performance,round(((XTOTPR-XTOTRJ)/XTOTPR)*100,2) as Quality,shf_tm as Shift_time,all_dt as Allowed_Dntm,AVAILT as Available_time,PRDn_t as TotProdn_time,PRDn_t2 as Avail_prodn_tim,DNT as Down_time,PRD_CYCLET as Prod_cycle_time,XTOTPR as Tot_prod,XTOTRJ as Tot_rej,shftcode,feed_alldt,mtime from (select a.vchnum,a.vchdate ,b.name,a.shftcode,max(to_number(b.place)) as shf_tm,max(to_number(b.balop)) as all_dt,max(to_number(a.lmd)) as feed_alldt,max(to_number(b.place)-to_number(b.balop)) as AVAILT,sum((case when a.a5>0 then a.un_melt else 0 end )) as PRDn_t,(sum((case when a.a5>0 then a.bcd else 0 end ))) as PRDn_t2,sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10) as DNT,sum(round(a.un_melt/a.a5,3)) as PRD_CYCLET,sum(a.a5*1) as XTOTPR,sum(a.a4*1) as XTOTRJ,c.mtime from prod_Sheet a, type b, itwstage c where a.branchcd='" + mbr + "' and a.type='86' and a.vchdate between to_Date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')  and trim(A.shftcode)=trim(B.type1) and trim(a.stage)=trim(c.stagec) and b.id='D' group by a.vchnum,b.name,a.vchdate ,a.shftcode,c.mtime order by a.vchdate) where AVAILT>0 order by vchdate ,shftcode";
                    SQuery = "select vchdate ,Name,STAGE_NAME,(round((prdn_t2/AVAILT)*1,2)*round(((XTOTPR+XTOTRJ)/(PRD_CYCLET*prdn_t2)),2)*round(((XTOTPR-XTOTRJ)/XTOTPR)*1,2))*100 as Tot_OEE,round((prdn_t2/AVAILT)*100,2) as Availability,round(((XTOTPR+XTOTRJ)/(PRD_CYCLET*prdn_t2))*100,2) as Performance,round(((XTOTPR-XTOTRJ)/XTOTPR)*100,2) as Quality,shf_tm as Shift_time,all_dt as Allowed_Dntm,AVAILT as Available_time,PRDn_t as TotProdn_time,PRDn_t2 as Avail_prodn_tim,DNT as Down_time,PRD_CYCLET as Prod_cycle_time,XTOTPR as Tot_prod,XTOTRJ as Tot_rej,shftcode,feed_alldt,mtime from (select a.vchnum,a.vchdate ,b.name,D.NAME AS STAGE_NAME,a.shftcode,max(to_number(b.place)) as shf_tm,max(to_number(b.balop)) as all_dt,max(to_number(a.lmd)) as feed_alldt,max(to_number(b.place)-to_number(b.balop)) as AVAILT,sum((case when a.a5>0 then a.un_melt else 0 end )) as PRDn_t,(sum((case when a.a5>0 then a.bcd else 0 end ))) as PRDn_t2,sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10) as DNT,sum(round(a.un_melt/a.a5,3)) as PRD_CYCLET,sum(a.a5*1) as XTOTPR,sum(a.a4*1) as XTOTRJ,c.mtime from prod_Sheet a, type b, itwstage c,TYPE D where a.branchcd='" + mbr + "' and a.type='86' and a.vchdate " + xprdrange + " and trim(A.shftcode)=trim(B.type1) and trim(a.stage)=trim(c.stagec) AND TRIM(A.STAGE)=TRIM(D.TYPE1) and b.id='D' AND D.ID='K' group by a.vchnum,b.name,a.vchdate ,a.shftcode,c.mtime,D.NAME  order by a.vchdate) where AVAILT>0 order by vchdate ,shftcode";
                    SQuery = "select vchnum,vchdate ,Name,STAGE_NAME,(round((prdn_t2/(case when AVAILT>0 then availt else 1 end))*1,2)*round(((XTOTPR+XTOTRJ)/(case when (PRD_CYCLET*prdn_t2) >0 then PRD_CYCLET*prdn_t2 else 1 end)),2)*round(((XTOTPR-XTOTRJ)/(case when XTOTPR >0 then XTOTPR else 1 end))*1,2))*100 as Tot_OEE,round((prdn_t2/(case when AVAILT >0 then AVAILT else 1 end))*100,2) as Availability,round(((XTOTPR+XTOTRJ)/(case when ( PRD_CYCLET*prdn_t2)>0 then PRD_CYCLET*prdn_t2 else 1 end))*100,2) as Performance,round(((XTOTPR-XTOTRJ)/(case when XTOTPR>0 then XTOTPR else 1 end))*100,2) as Quality,shf_tm as Shift_time,all_dt as Allowed_Dntm,AVAILT as Available_time,PRDn_t as TotProdn_time,PRDn_t2 as Avail_prodn_tim,DNT as Down_time,PRD_CYCLET as Prod_cycle_time,XTOTPR as Tot_prod,XTOTRJ as Tot_rej,shftcode,feed_alldt,mtime from (select a.vchnum,a.vchdate ,b.name,D.NAME AS STAGE_NAME,a.shftcode,max(to_number(b.place)) as shf_tm,max(to_number(b.balop)) as all_dt,max(to_number(a.lmd)) as feed_alldt,max(to_number(b.place)-to_number(b.balop)) as AVAILT,sum((case when a.a5>0 then a.un_melt else 0 end )) as PRDn_t,(sum((case when a.a5>0 then a.bcd else 0 end ))) as PRDn_t2,sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10) as DNT,sum(round(a.un_melt/(case when a.a5>0 then a.a5  else 1 end))) as PRD_CYCLET,sum(a.a5*1) as XTOTPR,sum(a.a4*1) as XTOTRJ,c.mtime from prod_Sheet a, type b, itwstage c,TYPE D where a.branchcd='" + mbr + "' and a.type='86' and a.vchdate " + xprdrange + " and trim(A.shftcode)=trim(B.type1) and trim(a.stage)||trim(a.icode)=trim(c.stagec)||trim(c.icode) AND TRIM(A.STAGE)=TRIM(D.TYPE1) and b.id='D' AND D.ID='K' group by a.vchnum,b.name,a.vchdate ,a.shftcode,c.mtime,D.NAME order by a.vchdate) where AVAILT>0 order by vchnum,vchdate ,shftcode";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["VOUCHER_NO"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr1["VOUCHER_DATE"] = Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        dr1["SHIFT"] = dt.Rows[i]["Name"].ToString().Trim().ToUpper();
                        dr1["SHIFTCODE"] = fgen.make_double(dt.Rows[i]["shftcode"].ToString().Trim().ToUpper());
                        dr1["PROCESS"] = dt.Rows[i]["STAGE_NAME"].ToString().Trim().ToUpper();
                        //dr1["AVAILABILITY"] = fgen.make_double(dt.Rows[i]["Availability"].ToString().Trim().ToUpper());
                        //dr1["PERFORMANCE"] = fgen.make_double(dt.Rows[i]["Performance"].ToString().Trim().ToUpper());
                        //dr1["QUALITY"] = fgen.make_double(dt.Rows[i]["Quality"].ToString().Trim().ToUpper());
                        dr1["SHIFT_TIME"] = fgen.make_double(dt.Rows[i]["Shift_time"].ToString().Trim().ToUpper());
                        dr1["ALLOWED_DNTM"] = fgen.make_double(dt.Rows[i]["Allowed_Dntm"].ToString().Trim().ToUpper());
                        dr1["AVAILABLE_TIME"] = fgen.make_double(dt.Rows[i]["Available_time"].ToString().Trim().ToUpper());
                        dr1["TOTPRODN_TIME"] = fgen.make_double(dt.Rows[i]["TotProdn_time"].ToString().Trim().ToUpper());

                        dr1["AVAIL_PRODN_TIM"] = fgen.make_double(dt.Rows[i]["Avail_prodn_tim"].ToString().Trim().ToUpper());
                        dr1["DOWN_TIME"] = fgen.make_double(dt.Rows[i]["DOWN_TIME"].ToString().Trim().ToUpper());
                        dr1["PROD_CYCLE_TIME"] = fgen.make_double(dt.Rows[i]["Prod_cycle_time"].ToString().Trim().ToUpper());
                        dr1["TOT_PROD"] = fgen.make_double(dt.Rows[i]["Tot_prod"].ToString().Trim().ToUpper());
                        dr1["TOT_REJ"] = fgen.make_double(dt.Rows[i]["Tot_rej"].ToString().Trim().ToUpper());
                        dr1["CYCLE_TIME"] = fgen.make_double(dt.Rows[i]["mtime"].ToString().Trim().ToUpper());
                        double db1, db2, db3, db4, db5;
                        // calculation part start
                        db1 = fgen.make_double(dt.Rows[i]["Allowed_Dntm"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["DOWN_TIME"].ToString().Trim());
                        db2 = (fgen.make_double(dt.Rows[i]["Available_time"].ToString().Trim()) - db1) / fgen.make_double(dt.Rows[i]["Available_time"].ToString().Trim());
                        dr1["AVAILABILITY_RATE"] = Math.Round(db2, 3);
                        db3 = fgen.make_double(dt.Rows[i]["mtime"].ToString().Trim().ToUpper());
                        db4 = (fgen.make_double(dt.Rows[i]["Available_time"].ToString().Trim()) - db1) * 60 / db3;
                        //dr1["PERFORMANCE_RATE"] = Math.Round((fgen.make_double(dt.Rows[i]["Quality"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["Tot_rej"].ToString().Trim())) / db4, 3);
                        //dr1["QUALITY_RATE"] = Math.Round(fgen.make_double(dt.Rows[i]["Quality"].ToString().Trim()) / (fgen.make_double(dt.Rows[i]["Quality"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["Tot_rej"].ToString().Trim())), 3);
                        dr1["PERFORMANCE_RATE"] = Math.Round((fgen.make_double(dt.Rows[i]["Tot_prod"].ToString().Trim())) / db4, 3);
                        dr1["QUALITY_RATE"] = Math.Round((fgen.make_double(dt.Rows[i]["Tot_prod"].ToString().Trim()) - fgen.make_double(dt.Rows[i]["Tot_rej"].ToString().Trim())) / (fgen.make_double(dt.Rows[i]["Tot_prod"].ToString().Trim())), 3);
                        //dr1["OEE_TOTAL"] = Math.Round(fgen.make_double(dr1["AVAILABILITY_RATE"].ToString()) * fgen.make_double(dr1["PERFORMANCE_RATE"].ToString()) * fgen.make_double(dr1["QUALITY_RATE"].ToString()) * 100, 3);  acc to old formula 
                        dr1["OEE_TOTAL"] = Math.Round(fgen.make_double(dr1["AVAILABILITY_RATE"].ToString()) + fgen.make_double(dr1["PERFORMANCE_RATE"].ToString()) + fgen.make_double(dr1["QUALITY_RATE"].ToString()), 3);
                        //calculation part end

                        dt1.Rows.Add(dr1);
                    }
                    Session["send_dt"] = dt1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevelJS("OEE Report For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "F43135": // OEE REPORT - BUPL
                    dt1 = new DataTable();
                    dt1.Columns.Add("DATE", typeof(string));
                    dt1.Columns.Add("SHIFT", typeof(string));
                    dt1.Columns.Add("STAGE", typeof(string));
                    dt1.Columns.Add("PERFORMANCE_RATE", typeof(double));
                    dt1.Columns.Add("QUALITY_RATE", typeof(double));
                    dt1.Columns.Add("AVAILABILITY_RATE", typeof(double));
                    dt1.Columns.Add("OEE", typeof(string));
                    dt = new DataTable();
                    SQuery = "select STAGE_CD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,Name,STAGE_NAME,shf_tm as Shift_time,all_dt as Allowed_Dntm,AVAILT as Available_time,PRDn_t as TotProdn_time,DNT as Down_time,PRD_CYCLET as Prod_cycle_time,XTOTPR as Tot_prod,XTOTRJ as Tot_rej,shftcode,mtime from (select trim(a.stage) AS STAGE_CD,A.VCHDATE,b.name,D.NAME AS STAGE_NAME,a.shftcode,max(to_number(b.place)) as shf_tm,max(to_number(b.balop)) as all_dt,max(to_number(b.place)-to_number(b.balop)) as AVAILT,sum((case when a.a5>0 then a.un_melt else 0 end )) as PRDn_t,(sum((case when a.a5>0 then a.bcd else 0 end ))) as PRDn_t2,sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+a.num7+a.num8+a.num9+a.num10) as DNT,sum(round(a.un_melt/(case when a.a5>0 then a.a5  else 1 end))) as PRD_CYCLET,sum(a.a5*1) as XTOTPR,sum(a.a4*1) as XTOTRJ,c.mtime,a.icode from prod_Sheet a, type b, itwstage c,TYPE D where a.branchcd='" + mbr + "' and a.type='86' and a.vchdate " + xprdrange + " and trim(A.shftcode)=trim(B.type1) and trim(a.stage)||trim(a.icode)=trim(c.stagec)||trim(c.icode) AND TRIM(A.STAGE)=TRIM(D.TYPE1) and b.id='D' AND D.ID='K' group by trim(a.stage),A.VCHDATE,b.name,a.shftcode,c.mtime,D.NAME,a.icode) where AVAILT>0 order by VCHDATE,shftcode";

                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "STAGE_CD", "VCHDATE");
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "STAGE_CD='" + dr0["STAGE_CD"].ToString().Trim() + "' AND  VCHDATE='" + dr0["VCHDATE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            DataTable dty = new DataTable();
                            dty = viewim.ToTable();
                            DataRow dr1 = dt1.NewRow();
                            double db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0;
                            if (dr0["VCHDATE"].ToString().Trim() == "30/12/2019")
                            {

                            }
                            for (int i = 0; i < dty.Rows.Count; i++)
                            {
                                dr1["DATE"] = dty.Rows[i]["VCHDATE"].ToString().Trim();
                                dr1["SHIFT"] = dty.Rows[i]["Name"].ToString().Trim();
                                dr1["STAGE"] = dty.Rows[i]["STAGE_NAME"].ToString().Trim();
                                db1 += (fgen.make_double(dty.Rows[i]["Tot_prod"].ToString().Trim()) * fgen.make_double(dty.Rows[i]["mtime"].ToString().Trim())) / 60;
                                db2 += fgen.make_double(dty.Rows[i]["DOWN_TIME"].ToString().Trim());
                                if (dr0["STAGE_CD"].ToString().Trim() == "02" || dr0["STAGE_CD"].ToString().Trim() == "03" || dr0["STAGE_CD"].ToString().Trim() == "11" || dr0["STAGE_CD"].ToString().Trim() == "12" || dr0["STAGE_CD"].ToString().Trim() == "13")
                                {
                                    db3 = fgen.make_double(dty.Rows[i]["Shift_time"].ToString().Trim());
                                }
                                else
                                {
                                    db3 = fgen.make_double(dty.Rows[i]["Available_time"].ToString().Trim());
                                }
                                db4 += fgen.make_double(dty.Rows[i]["Tot_prod"].ToString().Trim());
                                db5 += fgen.make_double(dty.Rows[i]["Tot_rej"].ToString().Trim());
                            }
                            dr1["PERFORMANCE_RATE"] = Math.Round(db1 / (db3 - db2) * 100, 3); // (tot_prodn*cycletime/60)/(aval_time-down_time)*100
                            dr1["QUALITY_RATE"] = Math.Round(db4 / (db4 + db5) * 100, 3); //(tot_prodn/(tot_prodn+Tot_rej)*100)
                            dr1["AVAILABILITY_RATE"] = Math.Round((db3 - db2) / db3 * 100, 3); //(Available_time-DOWN_TIME)/Available_time*100
                            dr1["OEE"] = Math.Round(fgen.make_double(dr1["Performance_Rate"].ToString()) * fgen.make_double(dr1["Quality_Rate"].ToString()) * fgen.make_double(dr1["Availability_Rate"].ToString()) / 10000, 3);
                            dt1.Rows.Add(dr1);
                        }
                    }
                    Session["send_dt"] = dt1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevelJS("OEE Production Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F39224A":
                    #region Daily Production Report
                    vartype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");
                    mq1 = "SELECT DISTINCT 'Daily Production Report' AS HEADER, E.NAME AS SHIFTNAME, A.*,B.INAME AS RNAME,D.INAME AS COMP_NAME,(A.IQTYIN+A.MLT_LOSS) AS PROD,C.BTCHNO AS LOT_NO,F.BTCHNO FROM  PROD_SHEET A,ITEM B,IVOUCHER C ,ITEM D,TYPE E,IVOUCHER F WHERE TRIM(A.VCHNUM)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=TRIM(C.VCHNUM)||to_Char(c.vchdate,'dd/mm/yyyy')||trim(c.icode) AND TRIM(A.SHFTCODE)=TRIM(E.TYPE1) and TRIM(A.ICODE)=TRIM(D.ICODE) AND TRIM(B.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " AND A.TYPE='" + vartype + "' AND C.TYPE='39' AND E.ID='D' AND A.VCHDATE " + xprdrange + " and F.TYPE='15' ORDER BY A.VCHNUM";
                    SQuery = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit as cunit,to_char(a.vchdate,'dd/mm/yyyy') as vch from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('15') and a.stage='61' and a.store='W' and a.vchdate " + xprdrange + "  order by a.vchnum,a.srno ";
                    mq0 = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit,to_char(a.vchdate,'dd/mm/yyyy') as vch from ivoucher a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('39') and a.stage='61' and a.store='W' and a.vchdate " + xprdrange + " and trim(a.naration)!='LUMPS' order by a.vchnum,a.srno ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq0);
                    dt3 = new DataTable();
                    mq2 = "Select a.* from ivoucher a where a.branchcd='" + mbr + "' and a.type in ('15') and a.stage='6R' and a.store='W' and upper(Trim(a.naration))='RUNNER' and a.vchdate " + xprdrange + "  order by a.vchnum,a.srno ";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq2);
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
                    mdt.Columns.Add(new DataColumn("actualShots", typeof(double)));
                    mdt.Columns.Add(new DataColumn("qtyprod", typeof(double)));
                    mdt.Columns.Add(new DataColumn("qtyRej", typeof(double)));
                    mdt.Columns.Add(new DataColumn("okComp", typeof(string)));
                    mdt.Columns.Add(new DataColumn("brkdown", typeof(string)));
                    mdt.Columns.Add(new DataColumn("brkdownCode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("mchFrom", typeof(string)));
                    mdt.Columns.Add(new DataColumn("mchTo", typeof(string)));
                    mdt.Columns.Add(new DataColumn("totTime", typeof(string)));
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
                        foreach (DataRow dr in dt2.Rows)
                        {
                            oporow = mdt.NewRow();
                            oporow["mcno"] = fgen.seek_iname(frm_qstr, co_cd, "SELECT TRIM(MCHCODE) AS FSTR,MCHNAME AS MACHINE_NAME,MCHCODE AS MACHINE_CODE,ACODE FROM PMAINT WHERE BRANCHCD='" + mbr + "' AND TYPE='10' AND TRIM(MCHCODE)='" + dr["acode"].ToString().Trim() + "'", "MACHINE_NAME");
                            oporow["mouldno"] = fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "riname");
                            string mould = fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "riname");
                            oporow["GRP"] = dr["vchdate"].ToString().Trim() + mould + dr["Riname"].ToString().Trim() + dr["cavity"].ToString().Trim();
                            oporow["compname"] = dr["riname"].ToString().Trim();
                            oporow["rmat"] = dr["iname"].ToString().Trim();
                            oporow["rmatcontrol"] = dr["BTCHNO"].ToString().Trim();
                            oporow["totalCavity"] = fgen.make_double(dr["ipack"].ToString().Trim());
                            oporow["runCavity"] = fgen.make_double(dr["cavity"].ToString().Trim());
                            oporow["actualShots"] = fgen.make_double(dr["shots"].ToString().Trim());
                            oporow["totalShots"] = fgen.make_double(dr["rlprc"].ToString().Trim());
                            double d1 = fgen.make_double(dr["cavity"].ToString().Trim()) * fgen.make_double(dr["shots"].ToString().Trim());
                            oporow["qtyprod"] = d1;
                            oporow["qtyRej"] = fgen.make_double(dr["rej_rw"].ToString().Trim());
                            oporow["okComp"] = (d1 - fgen.make_double(dr["rej_rw"].ToString().Trim())).ToString();
                            oporow["brkdown"] = fgen.seek_iname(frm_qstr, co_cd, "select sum(is_number(col3)) as sec from inspvch where branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='55' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "' ", "sec");
                            oporow["brkdownCode"] = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(xmlagg(xmlelement(e,col1||',')).extract('//text()').extract('//text()'),',') as sec from inspvch where branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='55' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "' ", "sec");
                            oporow["mchFrom"] = dr["mtime"].ToString().Trim();
                            oporow["mchTo"] = dr["REVIS_NO"].ToString().Trim();
                            oporow["totTime"] = dr["et_topay"].ToString().Trim();
                            oporow["comp"] = fgen.make_double(fgen.seek_iname_dt(dt, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "iqtyin")) * fgen.make_double(fgen.seek_iname(frm_qstr, co_cd, "select iweight from item where trim(icodE)='" + dr["rcode"].ToString().Trim() + "'", "iweight"));
                            oporow["runner"] = fgen.seek_iname_dt(dt3, "vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString().Trim() + "'", "iqtyin").toDouble(5);
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
                    Session["send_dt"] = dtdrsim;
                    fgen.Fn_open_rptlevelJS("Daily Production Report for the period of " + fromdt + " and " + todt + " ", frm_qstr);
                    #endregion
                    break;

                ////svpl reports ...18.09.2020

                case "F39257":
                    #region
                    dtm = new DataTable();
                    //dtm.Columns.Add(new DataColumn("FSTR", typeof(string)));
                    //dtm.Columns.Add(new DataColumn("GSTR", typeof(string)));
                    dtm.Columns.Add(new DataColumn("BRANCHCD", typeof(string)));
                    dtm.Columns.Add(new DataColumn("TYPE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ENTRY_NO", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ENTRY_DT", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SHIFT", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ZONE_CODE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ZONE_NAME", typeof(string)));
                    dtm.Columns.Add(new DataColumn("LINECODE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("LINE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("PARTCODE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("PARTNAME", typeof(string)));
                    dtm.Columns.Add(new DataColumn("DOWNTIME_MIN", typeof(string)));
                    dtm.Columns.Add(new DataColumn("TOT_REJECTION", typeof(double)));
                    dtm.Columns.Add(new DataColumn("SHIFT_SUPERVISOR_CODE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SHIFT_SUPERVISOR_NAME", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SHIFT_IC_CODE", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SHIFT_IC_NAME", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SHIFT_TIME", typeof(string)));
                    dtm.Columns.Add(new DataColumn("CAPCITY", typeof(double)));
                    dtm.Columns.Add(new DataColumn("LINE_EFFICIENCY", typeof(double)));
                    dtm.Columns.Add(new DataColumn("UNACCOUNTD_LOSS_MIN", typeof(double)));
                    dtm.Columns.Add(new DataColumn("BOTTLE_NECK_CT", typeof(double)));
                    dtm.Columns.Add(new DataColumn("OK_PROD_QTY", typeof(double)));
                    dtm.Columns.Add(new DataColumn("SHIFT_CLOSE_STATUS", typeof(string)));
                    dtm.Columns.Add(new DataColumn("MACHNG_REJ", typeof(double)));
                    dtm.Columns.Add(new DataColumn("CASTING_REJ", typeof(double)));
                    dtm.Columns.Add(new DataColumn("Unprocessed_CR", typeof(double)));
                    ///==============DOWNTIME====headings are hardcode as per MG Mam
                    #region
                    dtm.Columns.Add(new DataColumn("BD01", typeof(string)));//BD
                    dtm.Columns.Add(new DataColumn("BD02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("BD03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("BD04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ST01", typeof(string)));//ST
                    dtm.Columns.Add(new DataColumn("ST02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ST03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ST04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SU01", typeof(string)));//SU
                    dtm.Columns.Add(new DataColumn("SU02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SU03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("SU04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ML01", typeof(string)));//ML
                    dtm.Columns.Add(new DataColumn("ML02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ML03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("ML04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("DL01", typeof(string)));//DL
                    dtm.Columns.Add(new DataColumn("DL02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("DL03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("DL04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("MS01", typeof(string)));//MS
                    dtm.Columns.Add(new DataColumn("MS02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("MS03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("MS04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("OL01", typeof(string)));//OL
                    dtm.Columns.Add(new DataColumn("OL02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("OL03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("OL04", typeof(string)));
                    dtm.Columns.Add(new DataColumn("PF01", typeof(string)));//PF
                    dtm.Columns.Add(new DataColumn("PF02", typeof(string)));
                    dtm.Columns.Add(new DataColumn("PF03", typeof(string)));
                    dtm.Columns.Add(new DataColumn("PF04", typeof(string)));
                    #endregion
                    //SQuery = "SELECT a.branchcd,a.type,a.vchnum as entry_no,to_chaR(a.vchdate,'dd/mm/yyyy') as entry_dt,a.shift, a.zcode as zone_Code,a.zone as zone_name,a.linecd as linecode,a.line, a.pcode as partcode,a.part as partname,a.num1 as downtime_min,a.num2 as tot_rejection,a.supcd as supervisor_code,a.supv as supervisor_name,a.shift_ic as shift_ic_code,a.ic_name as shift_ic_name,a.DISP_SHIFT as shift_time,a.capcity,a.line_Eff as line_efficiency,a.disp_loss as unaccountd_loss_min,a.num3 as bottle_neck_ct,a.prod_qty as ok_prod_Qty,a.col16 as shift_close_Status,a.CAST_REJ as casting_rejection,a.MCH_REJ as machine_rejection,a.UNPROCREJ as unprocessed_rejection,b.Col1 as category,b.col3 as detail,b.col4 as losscode,b.QTY8 as  time_in_min ,b.col5 as catgcode FROM wb_prod_svp a,inspvch b WHERE a.BRANCHCD='" + mbr + "' AND a.TYPE='DE' and b.type='55' AND trim(A.COL19)=TRIM(B.COL6) and a.vchdate " + xprdrange + "  and a.vchnum='000733' order by a.vchnum desc";
                    //SQuery = "SELECT a.col19 as fstr,'-' AS GSTR,a.branchcd,a.type,a.vchnum as entry_no,to_chaR(a.vchdate,'dd/mm/yyyy') as entry_dt,a.shift, a.zcode as zone_Code,a.zone as zone_name,a.linecd as linecode,a.line, a.pcode as partcode,a.part as partname,sum(a.num1) as downtime_min,sum(a.num2) as tot_rejection,a.supcd as supervisor_code,a.supv as supervisor_name,a.shift_ic as shift_ic_code,a.ic_name as shift_ic_name,sum(a.capcity) as capcity ,sum(a.line_Eff) as line_efficiency,sum(a.disp_loss) as unaccountd_loss_min,sum(a.num3) as bottle_neck_ct,sum(a.prod_qty) as ok_prod_Qty,a.DISP_SHIFT as shift_time,a.col16 as shift_close_Status,sum(a.CAST_REJ) as casting_rejection,sum(a.MCH_REJ) as machine_rejection,sum(a.UNPROCREJ) as unprocessed_rejection,b.Col1 as category,b.col3 as detail,b.col4 as losscode,b.QTY8 as  time_in_min ,b.col5 as catgcode FROM wb_prod_svp a,inspvch b WHERE a.BRANCHCD='" + mbr + "' AND a.TYPE='DE' and b.type='55' AND trim(A.COL19)=TRIM(B.COL6) and a.vchdate " + xprdrange + " group by  a.branchcd,a.type,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') ,a.shift, a.zcode ,a.zone,a.linecd ,a.line, a.pcode ,a.part,a.supcd ,a.supv ,a.shift_ic ,a.ic_name ,a.DISP_SHIFT,a.col16 ,b.Col1 ,b.col3 ,b.col4,b.col5,a.col19,b.QTY8  order by a.vchnum desc";
                    SQuery = "SELECT a.col19 as fstr,'-' AS GSTR,a.branchcd,a.type,a.vchnum as entry_no,to_chaR(a.vchdate,'dd/mm/yyyy') as entry_dt,a.shift, a.zcode as zone_Code,a.zone as zone_name,a.linecd as linecode,a.line, a.pcode as partcode,a.part as partname,a.num1 as downtime_min,a.num2 as tot_rejection,a.supcd as supervisor_code,a.supv as supervisor_name,a.shift_ic as shift_ic_code,a.ic_name as shift_ic_name,a.capcity as capcity ,a.line_Eff as line_efficiency,a.disp_loss as unaccountd_loss_min,a.num3 as bottle_neck_ct,a.prod_qty as ok_prod_Qty,a.DISP_SHIFT as shift_time,a.col16 as shift_close_Status,a.CAST_REJ as casting_rejection,a.MCH_REJ as machine_rejection,a.UNPROCREJ as unprocessed_rejection,b.Col1 as category,b.col3 as detail,b.col4 as losscode,b.QTY8 as  time_in_min ,b.col5 as catgcode FROM wb_prod_svp a,inspvch b WHERE a.BRANCHCD='" + mbr + "' AND a.TYPE='DE' and b.type='55' AND trim(A.COL19)=TRIM(B.COL6) and a.vchdate " + xprdrange + " order by a.vchnum desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "BRANCHCD", "entry_no", "entry_dt"); //                                             
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "BRANCHCD='" + dr0["BRANCHCD"] + "' AND  entry_no='" + dr0["entry_no"] + "' AND  entry_dt='" + dr0["entry_dt"] + "'", "", DataViewRowState.CurrentRows);
                            DataTable dty = new DataTable();
                            dty = viewim.ToTable();
                            string ded1 = "";
                            DataRow dr1 = dtm.NewRow();
                            dr1["BRANCHCD"] = dty.Rows[0]["branchcd"].ToString().Trim();
                            dr1["TYPE"] = dty.Rows[0]["type"].ToString().Trim();
                            dr1["ENTRY_NO"] = dty.Rows[0]["entry_no"].ToString().Trim();
                            dr1["ENTRY_DT"] = dty.Rows[0]["entry_dt"].ToString().Trim();
                            dr1["SHIFT"] = dty.Rows[0]["shift"].ToString().Trim();
                            dr1["ZONE_CODE"] = dty.Rows[0]["zone_Code"].ToString().Trim();
                            dr1["ZONE_NAME"] = dty.Rows[0]["zone_name"].ToString().Trim();
                            dr1["LINECODE"] = dty.Rows[0]["linecode"].ToString().Trim();
                            dr1["LINE"] = dty.Rows[0]["line"].ToString().Trim();
                            dr1["PARTCODE"] = dty.Rows[0]["partcode"].ToString().Trim();
                            dr1["PARTNAME"] = dty.Rows[0]["PARTNAME"].ToString().Trim();
                            dr1["DOWNTIME_MIN"] = dty.Rows[0]["DOWNTIME_MIN"].ToString().Trim();
                            dr1["TOT_REJECTION"] = fgen.make_double(dty.Rows[0]["TOT_REJECTION"].ToString().Trim());
                            dr1["SHIFT_SUPERVISOR_CODE"] = dty.Rows[0]["supervisor_code"].ToString().Trim();
                            dr1["SHIFT_SUPERVISOR_NAME"] = dty.Rows[0]["supervisor_name"].ToString().Trim();
                            dr1["SHIFT_IC_CODE"] = dty.Rows[0]["shift_ic_code"].ToString().Trim();
                            dr1["SHIFT_IC_NAME"] = dty.Rows[0]["shift_ic_name"].ToString().Trim();
                            dr1["SHIFT_TIME"] = dty.Rows[0]["shift_time"].ToString().Trim();
                            dr1["CAPCITY"] = fgen.make_double(dty.Rows[0]["capcity"].ToString().Trim());
                            dr1["LINE_EFFICIENCY"] = fgen.make_double(dty.Rows[0]["line_efficiency"].ToString().Trim());
                            dr1["UNACCOUNTD_LOSS_MIN"] = fgen.make_double(dty.Rows[0]["unaccountd_loss_min"].ToString().Trim());
                            dr1["BOTTLE_NECK_CT"] = fgen.make_double(dty.Rows[0]["bottle_neck_ct"].ToString().Trim());
                            dr1["OK_PROD_QTY"] = fgen.make_double(dty.Rows[0]["ok_prod_Qty"].ToString().Trim());
                            dr1["SHIFT_CLOSE_STATUS"] = dty.Rows[0]["shift_close_Status"].ToString().Trim();
                            dr1["MACHNG_REJ"] = fgen.make_double(dty.Rows[0]["machine_rejection"].ToString().Trim());
                            dr1["CASTING_REJ"] = fgen.make_double(dty.Rows[0]["casting_rejection"].ToString().Trim());
                            dr1["Unprocessed_CR"] = fgen.make_double(dty.Rows[0]["unprocessed_rejection"].ToString().Trim());
                            for (int i = 0; i < dty.Rows.Count; i++)
                            {
                                ded1 = "";
                                ded1 = dty.Rows[i]["catgcode"].ToString().Trim();
                                switch (ded1)
                                {//BD catgeory
                                    case "BD01":
                                        dr1["BD01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "BD02":
                                        dr1["BD02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "BD03":
                                        dr1["BD03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "BD04":
                                        dr1["BD04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //ST category
                                    case "ST01":
                                        dr1["ST01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "ST02":
                                        dr1["ST02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "ST03":
                                        dr1["ST03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "ST04":
                                        dr1["ST04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //SU Category
                                    case "SU01":
                                        dr1["SU01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "SU02":
                                        dr1["SU02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "SU03":
                                        dr1["SU03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "SU04":
                                        dr1["SU04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //ML
                                    case "ML01":
                                        dr1["ML01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "ML02":
                                        dr1["ML02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "ML03":
                                        dr1["ML03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "ML04":
                                        dr1["ML04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //DL
                                    case "DL01":
                                        dr1["DL01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "DL02":
                                        dr1["DL02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "DL03":
                                        dr1["DL03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "DL04":
                                        dr1["DL04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //MS
                                    case "MS01":
                                        dr1["MS01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "MS02":
                                        dr1["MS01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "MS03":
                                        dr1["MS03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "MS04":
                                        dr1["MS04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //OL
                                    case "OL01":
                                        dr1["OL01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "OL02":
                                        dr1["OL02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "OL03":
                                        dr1["OL03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "OL04":
                                        dr1["OL04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    //PF
                                    case "PF01":
                                        dr1["PF01"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "PF02":
                                        dr1["PF02"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "PF03":
                                        dr1["PF03"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                    case "PF04":
                                        dr1["PF04"] = dty.Rows[i]["time_in_min"].ToString().Trim();
                                        break;
                                }
                            }
                            dtm.Rows.Add(dr1);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Downtime Details for the Period of " + fromdt + " to " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F39258":
                    SQuery = "SELECT a.branchcd,a.type,a.vchnum as entry_no,to_chaR(a.vchdate,'dd/mm/yyyy') as entry_dt,a.shift, a.zcode as zone_Code,a.zone as zone_name,a.linecd as linecode,a.line, a.pcode as partcode,a.part as partname,a.num1 as downtime_min,a.num2 as tot_rejection,a.capcity,a.line_Eff as line_efficiency,a.prod_qty as ok_prod_Qty,a.col4 as operation_code,a.col5 as machine_code,a.col2 as process_name,a.col6 as emp_code  FROM wb_prod_svp a WHERE a.BRANCHCD='" + mbr + "' AND a.TYPE='DE' and a.vchdate " + xprdrange + " order by a.vchnum desc"; // as per feedback file...after rmv some columns from report //old
                    SQuery = "SELECT a.branchcd,a.type,a.vchnum as entry_no,to_chaR(a.vchdate,'dd/mm/yyyy') as entry_dt,a.shift, a.zcode as zone_Code,a.zone as zone_name,a.linecd as linecode,a.line, a.pcode as partcode,a.part as partname,a.num1 as downtime_min,a.num2 as tot_rejection,a.capcity,a.line_Eff as line_efficiency,a.prod_qty as ok_prod_Qty,a.col4 as machine,a.col5 as op_code,a.col2 as process_name,a.col6 as emp_code  FROM wb_prod_svp a WHERE a.BRANCHCD='" + mbr + "' AND a.TYPE='DE' and a.vchdate " + xprdrange + " order by a.vchnum desc"; // as per feedback file...after rmv some columns from report
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Operator Details Report From " + fromdt + " and To " + todt + "", frm_qstr);
                    break;
            }
        }
    }

    void wip_stk_vw_disp()
    {
        xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
        xprd1 = "BETWEEN TO_DATE('01/04/2010','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
        mq10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT DISTINCT WIPSTDT,TYPE1 FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "'", "WIPSTDT");
        if (mq10 == "0")
        {
            mq10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
        }

        xprd2 = "between to_Date('" + mq10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

        SQuery = "select trim(Item_name) as Item_name,trim(Part_No) as Part_No,sum(stg01) as stg01,sum(stg02) as stg02,sum(stg03) as stg03,sum(stg04) as stg04,sum(stg05) as stg05,sum(stg06) as stg06,sum(stg07) as stg07,sum(stg08) as stg08,sum(stg09) as stg09,sum(stg11) as stg11,sum(stg12) as stg12,sum(stg13) as stg13,sum(stg14) as stg14,sum(stg15) as stg15,sum(stg16) as stg16,sum(stg01)+sum(stg02)+sum(stg03)+sum(stg04)+sum(stg05)+sum(stg06)+sum(stg07)+sum(stg08)+sum(stg09)+sum(stg11)+sum(stg12)+sum(stg13)+sum(stg14)+sum(stg15) as total,trim(icode)as  icode,wolink as batchno from (select Item_Name,Part_No,iweight,wt_net,mat5,mat6,mat7,salloy,decode(stage,'61',Balance,0) as Stg01,decode(stage,'62',Balance,0) as Stg02,decode(stage,'63',balance,0) as Stg03,decode(stage,'64',balance,0) as Stg04,decode(stage,'65',balance,0) as Stg05,decode(stage,'66',balance,0) as Stg06,decode(stage,'67',balance,0) as Stg07,decode(stage,'68',balance,0) as Stg08,decode(stage,'69',balance,0) as Stg09,decode(stage,'6A',balance,0) as Stg11,decode(stage,'6B',balance,0) as Stg12,decode(stage,'6C',balance,0) as Stg13,decode(stage,'6D',balance,0) as Stg14,decode(stage,'6E',balance,0) as Stg15,decode(stage,'6R',balance,0) as Stg16,icode,wolink  from (select a.type,C.iname as Item_Name,C.Cpartno as Part_No,c.iweight,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,sum(a.iqtyin) as Input,sum(a.iqtyout) as Output,sum(a.iqtyin)-sum(a.iqtyout) as Balance,trim(a.stage) as stage,a.icode,a.wolink from (select type,stage,maincode,icode,iqtyin,iqtyout,'-' as wolink From wipstk where branchcd='" + mbr + "' and type='50' and vchdate  " + xprd2 + "  and substr(icode,1,1) in ('9','7') union all select type,stage,icode,icode,iqtyin,iqtyout,'-' as wolink From ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  " + xprd2 + "  and store='W' and substr(icode,1,1) in ('7','9') union all select type,stage,icode,icode,iqtyout,iqtyin,'-' as wolink From ivoucher where branchcd='" + mbr + "' and type in('30','31','11','XX') and vchdate  " + xprd2 + "  and store='Y' and substr(icode,1,1) in ('7','9')) a,item c where trim(a.icode)=trim(c.icode) group by C.iname,C.cpartno,c.iweight,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,trim(a.stage),a.icode,a.type,a.wolink)) group by trim(Item_Name),trim(Part_No),trim(Icode),iweight,wt_net,mat5,mat6,mat7,salloy,wolink order by trim(Item_Name)";
        fgen.execute_cmd(frm_qstr, co_cd, "create or replace view wipcolstkw_" + mbr + " as(SELECT * FROM (" + SQuery + "))");
    }

}