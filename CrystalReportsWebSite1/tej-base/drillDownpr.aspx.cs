using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.ComponentModel;
using System.Windows;

using System.Globalization;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public partial class drillDownpr : System.Web.UI.Page
{
    fgenDB fgen = new fgenDB();
    string Squery, co_cd, frm_url, frm_uname, frm_qstr, frm_formID; DataTable dt;
    int col_count = 0;
    string myq = "", branch_Cd = "", year;
    string frm_mbr = "";
    string ind_curr = "Y";
    int totCol = 50;
    string gvSortExpression { get; set; }
    bool runOnce = false;
    int totWidthPR = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //CultureInfo myCultureInfo = new CultureInfo("en-IN");
        Page.Culture = "en-In";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            //-----------------
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                }
            }
            //--------------------------                                    
            //Squery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMDRILLID", frm_formID);


            if (frm_formID == "F70713") tkrow.Text = "10000";
            if (frm_formID == "F10111" || frm_formID == "F10116") tkrow.Text = "100000";
            finOpt.Visible = false;
            if (!Page.IsPostBack)
            {
                try
                {
                    setDrillLevel(0, 0, "");
                }
                catch { }
            }
            txtsearch.Attributes["onkeydown"] = "if (event.keyCode == 40) { $('[TabIndex=1]').focus(); }";

            txtsearch.Focus();

            if (frm_formID == "F15314A")
            {
                txtSpan.InnerHtml = "1. Critical item is highlighted in yellow color. <br />2. Item below minimum level is highlighted in red color.";
            }
            if (frm_formID == "F45152" || frm_formID == "F77115")
            {
                string extraLine = "<br />4. Your username = " + frm_uname + " is not linked with any Salesman Code, hence it will show all ";
                string saleMancode = fgen.seek_iname(frm_qstr, co_cd, "SELECT SMAN_IDEN AS ID FROM EVAS WHERE USERNAME='" + frm_uname + "'", "ID");
                if (saleMancode.Length > 2)
                {
                    extraLine = "<br />4. Your username = " + frm_uname + " is linked with Salesman Code - " + saleMancode + ".";
                }
                txtSpan.InnerHtml = "1. If Achievement is less than 25% then Showing in <span style='background-color:pink'>Red</span> <br />2. If achievement is between 26 % to 75% then <span style='background-color:Yellow'>Yellow</span> <br />3. and if Achievement is over 76% of target, then <span style='background-color:#84df84'>Green</span> " + extraLine;
            }
            if (frm_formID == "F70189" || frm_formID == "F70189A") divPL.Visible = true;
            else divPL.Visible = false;
            if (frm_formID == "F70156") divBS.Visible = true;
            else divBS.Visible = false;

            divArab.Visible = false;
            if ((frm_formID == "F70713" || frm_formID == "F70717") && co_cd == "SGRP")
                divArab.Visible = true;
        }
    }
    public void fill_grid(string gridQuery)
    {
        hfqry.Value = ""; dt = new DataTable();

        if (gridQuery.Length > 10)
        {
            string mhc = "";
            dt = fgen.getdata(frm_qstr, co_cd, "select * from ( " + gridQuery + " ) where rownum<=" + tkrow.Text.Trim() + "");
            //mhc = (ind_curr == "INR" || ind_curr == "IND") ? "en-IN" : "en-US";
            //CultureInfo myCultureInfo = new CultureInfo("" + mhc + "");
            //dt.Locale = myCultureInfo;
            string numbr_fmt = "999,999,999.99";
            string numbr_fmt2 = "999,999,999";
            string mhd = fgen.seek_iname(frm_qstr, co_cd, "SELECT BR_CURREN||'~'||'1000'||'~'||'000'||'~'||NUM_FMT1||'~'||NUM_FMT2 AS FSTR FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "' ", "FSTR");
            if (mhd != "0")
            {
                numbr_fmt = (mhd.Split('~')[3] == "" || mhd.Split('~')[3] == "-" || mhd.Split('~')[3] == "0") ? numbr_fmt : mhd.Split('~')[3];
                numbr_fmt2 = (mhd.Split('~')[4] == "" || mhd.Split('~')[4] == "-" || mhd.Split('~')[4] == "0") ? numbr_fmt2 : mhd.Split('~')[4];
            }
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");
            double totDiff = 0;
            switch (frm_formID)
            {
                case "F70156":
                case "F70189":
                case "F70189A":
                    if (hfLevel.Value == "0")
                    {
                        if (frm_formID == "F70156")
                        {
                            string branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "BR_COND");
                            string frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                            string frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                            string fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                            string todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");



                            string xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                            string xprd1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                            string xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                            string xprdrange = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

                            string mq = "select '-' AS FSTR,'-' AS GSTR,'-' as grpname,'-' as grp_Code,'0' as lib,to_char(sum(cl),'99999999999.00') as cl from (select frmdt,todt,header,subgrpcode,mgcode,sub_grp,mgname,'-' as branchcd,acode,aname,opening,drmt,crmt,cl,(case when opening>0 then opening else 0 end ) as op_debit,(case when opening<0 then abs(opening) else 0 end ) as op_crdt,(case when (cl)>0 then (abs(cl)) else 0 end ) as cl_debit,(case when (cl)<0 then (abs(cl)) else 0 end ) as cl_crdt from (select '01/01/2021' as frmdt,'01/07/2021' as todt,'Trial Balance 4 Column' as header, b.bssch as subgrpcode,c.type1 as mgcode, d.name as sub_grp,c.name as mgname, trim(a.acode) as acode,b.aname,sum(nvl(a.opening,0)) as opening,sum(nvl(a.cdr,0)) as drmt,sum(nvl(a.ccr,0)) as crmt,sum(nvl(a.opening,0))+sum(nvl(a.cdr,0))-sum(nvl(a.ccr,0)) as cl from (Select branchcd,trim(acode) as acode, nvl(yr_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos from famstbal where (" + branch_Cd + ") union all select branchcd,trim(acode),nvl(dramt,0)-nvl(cramt,0) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " union all select branchcd,trim(acode),0 as op,nvl(dramt,0) as cdr,nvl(cramt,0) as ccr,0 as clos from VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " ) a,famst b,type c,typegrp d where TRIM(b.bssch)=trim(d.type1) and substr(TRIM(b.grp),1,2)=trim(c.type1) and d.id='A' and c.id='Z' and trim(a.acode)=trim(b.acode)  and trim(b.bssch) like '%' and substr(b.grp,1,2) like '%' group by trim(a.acode),b.aname,c.name,d.name,b.bssch,c.type1,b.aname having abs(sum(nvl(a.opening,0)))+sum(nvl(a.cdr,0))+sum(nvl(a.ccr,0))!=0 )) ";
                            totDiff = fgen.seek_iname(frm_qstr, co_cd, mq, "CL").toDouble();
                        }


                        DataTable dt1 = new DataTable();
                        dt1 = dt;
                        dt = new DataTable();
                        dt.Columns.Add("fstr");
                        dt.Columns.Add("gstr");
                        dt.Columns.Add("0");
                        dt.Columns.Add("1");
                        dt.Columns.Add("2");
                        dt.Columns.Add("3");
                        dt.Columns.Add("4");
                        dt.Columns.Add("5");
                        DataRow oporow = null;
                        double labamt = 0, astamt = 0;
                        for (int i = 0; i < (dt1.Rows.Count + 3); i++)
                        {
                            oporow = dt.NewRow();
                            dt.Rows.Add(oporow);
                        }
                        int xx = 0, xx2 = 0;
                        double totvalCol1 = 0, totvalCol2 = 0;
                        if (frm_formID == "F70189" || frm_formID == "F70189A")
                        {
                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                if (dr1["group_code"].ToString().Substring(0, 1) != "2")
                                {
                                    dt.Rows[xx]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx]["0"] = dr1["group_name"];
                                    labamt = dr1["expenses"].ToString().toDouble() - dr1["incomes"].ToString().toDouble();
                                    dt.Rows[xx]["1"] = numWithComma(labamt.ToString(), numbr_fmt);
                                    dt.Rows[xx]["4"] = dr1["group_code"];
                                    xx++;
                                    totvalCol1 += labamt.toDouble(2);
                                }
                                else
                                {
                                    dt.Rows[xx2]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx2]["2"] = dr1["group_name"];
                                    astamt = dr1["incomes"].ToString().toDouble() - dr1["expenses"].ToString().toDouble();
                                    dt.Rows[xx2]["3"] = numWithComma(astamt.ToString(), numbr_fmt);
                                    dt.Rows[xx2]["5"] = dr1["group_code"];
                                    xx2++;
                                    totvalCol2 += astamt.toDouble(2);
                                }
                            }
                        }
                        else
                        {
                            // balance sheet
                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                if (dr1["group_code"].ToString().Substring(0, 1) == "0")
                                {
                                    dt.Rows[xx]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx]["0"] = dr1["group_name"];
                                    labamt = dr1["Liabilities"].ToString().toDouble() - dr1["assets"].ToString().toDouble();
                                    dt.Rows[xx]["1"] = numWithComma(labamt.ToString(), numbr_fmt);
                                    dt.Rows[xx]["4"] = dr1["group_code"];
                                    xx++;
                                    totvalCol1 += labamt.toDouble(2);
                                }
                                else
                                {
                                    dt.Rows[xx2]["fstr"] = dr1["group_code"].ToString();
                                    dt.Rows[xx2]["2"] = dr1["group_name"];
                                    astamt = dr1["assets"].ToString().toDouble() - dr1["Liabilities"].ToString().toDouble();
                                    dt.Rows[xx2]["3"] = numWithComma(astamt.ToString(), numbr_fmt);
                                    dt.Rows[xx2]["5"] = dr1["group_code"];
                                    xx2++;
                                    totvalCol2 += astamt.toDouble(2);
                                }
                            }
                        }
                        double prof_fig = 0;
                        prof_fig = Math.Round(totvalCol2 - totvalCol1, 2);
                        astamt = 0;
                        labamt = 0;
                        if (prof_fig != 0)
                        {
                            if (prof_fig > 0)
                            {
                                dt.Rows[xx]["fstr"] = "-";
                                dt.Rows[xx]["0"] = "Profit/Surplus";
                                labamt = prof_fig;
                                dt.Rows[xx]["1"] = numWithComma((labamt - totDiff).ToString(), numbr_fmt);
                                dt.Rows[xx]["4"] = "-";
                                xx++;
                            }
                            else
                            {
                                dt.Rows[xx2]["fstr"] = "-";
                                dt.Rows[xx2]["2"] = "Loss/Deficit";
                                astamt = prof_fig;
                                dt.Rows[xx2]["3"] = numWithComma((astamt + totDiff).ToString(), numbr_fmt);
                                dt.Rows[xx2]["5"] = "-";
                                xx2++;
                            }
                        }

                        totvalCol1 += Math.Abs(labamt).toDouble(2);
                        totvalCol2 += Math.Abs(astamt).toDouble(2);

                        if (frm_formID == "F70156")
                        {
                            if (labamt != 0)
                            {
                                xx = xx + 2;

                                dt.Rows[xx]["fstr"] = "-";
                                dt.Rows[xx]["0"] = "Difference in Trial Balance";
                                labamt = prof_fig;
                                dt.Rows[xx]["1"] = numWithComma((totDiff).ToString(), numbr_fmt);
                                dt.Rows[xx]["4"] = "-";
                            }
                            if (astamt != 0)
                            {
                                xx2 = xx2 + 2;

                                dt.Rows[xx2]["fstr"] = "-";
                                dt.Rows[xx2]["2"] = "Difference in Trial Balance";
                                dt.Rows[xx2]["3"] = numWithComma((totDiff).ToString(), numbr_fmt);
                                dt.Rows[xx2]["5"] = "-";
                            }
                        }

                        dt.Rows[dt.Rows.Count + 2 - 5]["1"] = "----------------------------";
                        dt.Rows[dt.Rows.Count + 2 - 4]["1"] = numWithComma(totvalCol1.toDouble(2).ToString(), numbr_fmt);
                        dt.Rows[dt.Rows.Count + 2 - 3]["1"] = "----------------------------";

                        dt.Rows[dt.Rows.Count + 2 - 5]["3"] = "----------------------------";
                        dt.Rows[dt.Rows.Count + 2 - 4]["3"] = numWithComma(totvalCol2.toDouble(2).ToString(), numbr_fmt);
                        dt.Rows[dt.Rows.Count + 2 - 3]["3"] = "----------------------------";
                        if (frm_formID == "F70189")
                            lblGPNP.Text = " N.P %  =  " + (prof_fig != 0 && totvalCol1 != 0 ? Math.Round(prof_fig / totvalCol1 * 100, 2) : 0);
                        if (frm_formID == "F70189A")
                            lblGPNP.Text = " G.P %  =  " + (prof_fig != 0 && totvalCol1 != 0 ? Math.Round(prof_fig / totvalCol1 * 100, 2) : 0);

                    }
                    break;
                case "F70556":
                    // month cumm total
                    if (hfLevel.Value.toDouble() > 0)
                    {
                        hfOpening.Value = acBal(hfvalSelected.Value.Trim(), "");
                        double OpBal = hfOpening.Value.toDouble();
                        double cumTot = hfOpening.Value.toDouble();
                        double totDr = 0;
                        double totCr = 0;
                        string totalDRCR = "";

                        finOpt.Visible = true;
                        if (hfLevel.Value.toDouble() == 1)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                cumTot += (dr["DEBITS"].ToString().Trim().toDouble() - dr["CREDITS"].ToString().Trim().toDouble());
                                totDr += (dr["DEBITS"].ToString().Trim().toDouble(2));
                                totCr += (dr["CREDITS"].ToString().Trim().toDouble(2));

                                dr[6] = cumTot.toDouble(2);
                            }
                        }
                        else
                        {
                            totalDRCR = fgen.seek_iname(frm_qstr, co_cd, "SELECT SUM(DEBIT)||'~'||SUM(CREDITS) AS BAL FROM (" + gridQuery + ") ", "BAL");
                            if (totalDRCR != "0")
                            {
                                totDr = totalDRCR.Split('~')[0].toDouble(2);
                                totCr = totalDRCR.Split('~')[1].toDouble(2);
                            }
                        }


                        lblOpBal.Text = "Opening Balance : " + (OpBal > 0 ? OpBal + " Dr " : Math.Abs(OpBal.toDouble(2)) + " Cr");
                        lblTotDr.Text = "Total Debit : " + Math.Abs(totDr.toDouble(2)) + " Dr";
                        lblTotCr.Text = "Total Credit : " + Math.Abs(totCr.toDouble(2)) + " Cr";
                        lblTotBal.Text = "Balance : " + ((OpBal + (totDr - totCr)) > 0 ? Math.Round(OpBal + (totDr - totCr), 2) + " Dr " : Math.Abs((OpBal + (totDr - totCr)).toDouble(2)) + " Cr");
                    }
                    break;
                // margin Report
                case "F05405M_MARGINALL":
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtJobCard = new DataTable();
                        dtJobCard = fgen.getdata(frm_qstr, co_cd, "select col21,sum((QTY*IRATE)-(0)) as fg_Val,round(sum((QTY*IRATE)-(0))/sum(qty),5)  as fg_rate  ,sum((QTY*IRATE)-(NUM1+NUM2+NUM3+NUM4)) as raw_mat_Val,round(sum((QTY*IRATE)-(NUM1+NUM2+NUM3+NUM4))/sum(qty),5) as raw_mat_rate,TRIM(enqno) AS JOB_NO,TO_CHAR(enqdt,'DD/MM/YYYY') AS JOB_DT,to_char(Vchdate,'yyyymmdd') as vdd from costestimate where branchcd='" + frm_mbr + "' AND type='40' AND VCHDATE>=TO_DATE('01/01/2021','DD/MM/YYYY') and qty>0 and irate>0 and substr(icode,1,1)='9' group by TRIM(enqno),TO_CHAR(enqdt,'DD/MM/YYYY'),to_char(Vchdate,'yyyymmdd'),col21 order by vdd");
                        if (dtJobCard.Rows.Count > 0)
                        {
                            foreach (DataRow drx in dt.Rows)
                            {
                                DataView dv = new DataView(dtJobCard, "JOB_NO='" + drx["JOB_NO"].ToString().Trim() + "' AND JOB_dT='" + drx["JOB_dT"].ToString().Trim() + "' ", "vdd desc", DataViewRowState.CurrentRows);
                                if (dv.Count > 0)
                                {
                                    for (int i = 0; i < dv.Count; i++)
                                    {
                                        drx["RM_Actual_Rate"] = dv[i].Row["RAW_MAT_rATE"].ToString().toDouble();

                                        if (drx["RM_Actual_Rate"].ToString().toDouble() > 0 && drx["RM_Estimation_Rate"].ToString().toDouble() > 0)
                                            drx["margin_per_4"] = Math.Abs(Math.Round((drx["RM_Estimation_Rate"].ToString().toDouble() - drx["RM_Actual_Rate"].ToString().toDouble()) / drx["RM_Estimation_Rate"].ToString().toDouble(), 2)).ToString();

                                        if (drx["RM_Actual_Rate"].ToString().toDouble() > 0 && drx["so_Rate"].ToString().toDouble() > 0)
                                            drx["margin_per_5"] = Math.Round((drx["so_rate"].ToString().toDouble() - drx["RM_Actual_Rate"].ToString().toDouble()) / drx["so_rate"].ToString().toDouble(), 2).ToString();
                                    }
                                }
                            }
                        }
                        lblGPNP.Text = "** Per1 = SO RATE VS TOTAL EST , Per2 = SO RATE VS  RM EST , Per3 = TOTAL EST VS TOTAL ACTUAL , Per4 = RM EST VS RM ACTUAL , Per5 = SO Rate vs Total_Actual";
                        lblGPNP.Style.Add("background-color", "yellow");
                    }
                    break;
            }
        }
        else
        {
            dt = (DataTable)Session["send_dt"];
        }
        if (dt.Rows.Count > 0)
        {
            fillPMGrid(dt);

            switch (frm_formID)
            {
                case "F70156":
                case "F70189":
                case "F70189A":
                case "F70230A":
                case "F70175":

                    break;
            }
        }
        else
        {
        }
        hfqry.Value = Squery;
        if (Squery == "SEND_DT" || hfqry.Value == "-") lblTotcount.InnerText = "Total Rows : " + dt.Rows.Count;
        // commented for few days on 31/01/2022
        //else
        //    lblTotcount.InnerText = "Total Rows : " + fgen.seek_iname(frm_qstr, co_cd, "SELECT COUNT(*) as cc FROM (" + Squery + ")", "cc");
    }
    void searchFunc()
    {
        DataTable dt1 = new DataTable();
        Squery = hfqry.Value;
        if (hfqry.Value.Length > 10)
        {
            Squery = hfqry.Value;
        }
        else if (Session["send_dt"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["send_dt"];
        }
        if (txtsearch.Text.Length <= 0) Squery = "select * from (" + Squery + ") where rownum<=" + fgen.make_double(tkrow.Text.Trim()) + "";

        if (txtsearch.Text == "")
        {
            if (hfqry.Value.Length > 10)
            {
                Squery = "select * from ( " + Squery + " ) where rownum<=" + tkrow.Text.Trim() + "";
                dt1 = fgen.getdata(frm_qstr, co_cd, Squery);
            }
            else
            {
                dt1 = fgen.searchDataTable(txtsearch.Text, dt);
            }
        }
        else
        {
            if (hfqry.Value.Length > 10)
            {
                dt1 = fgen.search_vip(frm_qstr, co_cd, Squery, txtsearch.Text.Trim().ToUpper());
            }
            else
            {
                dt1 = fgen.searchDataTable(txtsearch.Text, dt);
            }
        }

        //ddSorting.DataSource = null;
        {
            dt = dt1;
            fillPMGrid(dt);
            Squery = hfqry.Value;
            if (Squery == "SEND_DT") lblTotcount.InnerText = "Total Rows : " + dt.Rows.Count;
            else
                lblTotcount.InnerText = "Total Rows : " + fgen.seek_iname(frm_qstr, co_cd, "SELECT COUNT(*) as cc FROM (" + Squery + ")", "cc");
        }
        dt1.Dispose();
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        searchFunc();
    }

    void fillPMGrid(DataTable myDt)
    {
        if (!dt.Columns.Contains("FSTR")) dt.Columns.Add("FSTR").SetOrdinal(0);
        if (!dt.Columns.Contains("GSTR")) dt.Columns.Add("GSTR").SetOrdinal(1);
        dt = myDt;
        ViewState["sg1"] = myDt;
        if (dt == null) return;
        string data = GetJson(dt);
        string header = header_name(dt);
        string filters = getfilters(dt);
        string colsToSumm = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLS_SUM").ToUpper();
        if (colsToSumm == "" || colsToSumm == "-" || colsToSumm == "0")
            colsToSumm = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLS_SUM" + hfLevel.Value).ToUpper();
        string summJsFun = "";
        string summDataModel = "";
        string fileName = "";
        int col_tot = 0;
        col_tot = dt.Columns.Count;

        DataTable dtHead = new DataTable();
        string formid = frm_formID;
        if (Session["dt_menu" + frm_qstr] == null)
        {
            dtHead = fgen.fill_icon_grid(co_cd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_ICONTAB"), "", frm_qstr);
        }
        else dtHead = (DataTable)Session["dt_menu" + frm_qstr];
        fileName = fgen.seek_iname_dt(dtHead, "ID='" + formid + "'", "TEXT");
        if (fileName == "0" || fileName == "" || fileName == "-")
            fileName = co_cd + "_" + DateTime.Now.ToString().Trim();

        if (colsToSumm.Length > 3)
        {
            string cols = "", variables = "", showCols = "";
            string vars = "";
            foreach (string varsx in colsToSumm.Split(','))
            {
                vars = varsx.ToUpper();
                if (variables == "") variables = "var " + vars + "C = 0";
                else variables += "," + vars + "C = 0";
                if (cols == "") cols = vars + "C += row." + vars + ".replace(',','') * 1;";
                else cols += "" + vars + "C += row." + vars + ".replace(',','') * 1;";
                if (showCols == "") showCols = vars + " : " + vars + "C";
                else showCols += ", " + vars + " : " + vars + "C";
            }
            variables += " , data = this.option('dataModel.data'), len = data.length;";

            summDataModel = "dataReady: calculateSummary, change: calculateSummary, ";
            summJsFun = "function calculateSummary() { " + variables + " data.forEach(function(row) { " + cols + " }); var totalData = { " + showCols + ", pq_rowcls: 'green', summaryRow: true }; this.option('summaryData', [totalData]); } ";
        }

        StringBuilder sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(document).ready(function () { ");
        sb.Append(@"var data = " + data.Trim() + ";");
        if (summJsFun != "")
        {
            sb.Append(summJsFun);
        }
        sb.Append(@"var colModel = " + header + " ; ");
        sb.Append(@"var dataModel = { data: data };");

        string jsClickWorking = "var address = ui.rowData; document.getElementById('HiddenField1').value= JSON.stringify( address ); document.getElementById('btnGrid').click(); ";
        string sseekJS = ",cellKeyDown : function(evt, ui) {if(evt.keyCode == 13){" + jsClickWorking + " } } ,cellDblClick : function(evt, ui) {" + jsClickWorking + "}  ";
        string afit_cond = "";
        if (col_tot <= 9 || totWidthPR < 1200)
        {
            afit_cond = "scrollModel: { autoFit: true } , ";
        }
        sb.Append(@"var obj = { load : function () { var grid = this; var filter; " + filters + " }, " + afit_cond + " height: 600,dataModel: dataModel, create: function () {this.flex();} " + sseekJS + " ,pageModel: { type: 'local', rPP: 1000, rPPOptions: [10, 50, 100, 200, 500, 1000, 2000, 5000, 10000, 50000, 100000, 500000, 1000000] },colModel: colModel, " + summDataModel + " selectionModel: { type: 'row' },toolbar: {style: 'text-align:left',items: [ { type: 'select', label: 'Format: ',attr: 'id=export_format',options: [{ xlsx: 'Excel', csv: 'Csv', json: 'Json'}]},{type: 'button',label: 'Export File &nbsp;&nbsp;',icon: 'ui-icon-arrowthickstop-1-s',listener: function () {var format = $('#export_format').val(), blob = this.exportData({format: format,render: true});if(typeof blob === 'string'){blob = new Blob([blob]);}saveAs(blob, '" + fileName + ".'+ format );}},  {type: 'button',icon: 'ui-icon-print',label: 'Print',listener: function () {var exportHtml = this.exportData({ title: '" + fileName + "', format: 'htm', render: true }),newWin = window.open('', '', 'width=1200, height=700'),doc = newWin.document.open();doc.write(exportHtml);doc.close();newWin.print();}},{type: 'select',label: '&nbsp;&nbsp;&nbsp;&nbsp; | &nbsp;&nbsp;&nbsp;&nbsp;Number of frozen/fix columns: ',options: [2, 3, 4, 5], listener: function (evt) {this.option('freezeCols', $(evt.target).val());this.refresh();}},{ type: 'separator' },{ type: 'checkbox',style: 'margin-left:5px;',attr: 'checked=unchecked', label: 'Wrap Text',listener: function (evt) {if ($(evt.target).prop('checked')) {this.option({ wrap: true, hwrap: true });}else {this.option({ wrap: false, hwrap: false });}this.refresh();}}]},numberCell: { show : false },title:'',resizable: true,editable: false , filterModel: { on: true, mode: 'AND', header: true }} ;");
        sb.Append(@"$('#gridDiv').pqGrid(obj); ");

        sb.Append(@"");

        sb.Append(@"});");

        sb.Append(@"</script>");
        dt.Dispose();

        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLS_SUM", "");
    }
    public string GetJson(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int count = 0; count < dt.Rows.Count; count++)
        {
            DataRow dr = dt.Rows[count];
            string rowDataStr = "";
            foreach (DataColumn dc in dt.Columns)
            {
                if (rowDataStr.Length > 0) rowDataStr += ",";
                if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                    rowDataStr += "" + dc.ColumnName.Replace("\r\n", "").Replace("&", "").Replace("'", "").Replace("-", "_").Replace(".", "").Replace(",", "") + " : '" + dr[dc].ToString().Replace("\r\n", "").Replace("\n", "").Replace("'", "`").Replace(@"\", "\\").Trim() + "'";
                else rowDataStr += " " + dc.ColumnName.Replace("\r\n", "").Replace("&", "").Replace("'", "").Replace("-", "_").Replace(".", "").Replace(",", "") + " : '" + dr[dc].ToString().Replace("\r\n", "").Replace("\n", "").Replace("'", "`").Replace(@"\", "\\").Trim() + "'";
            }
            if (count > 0) sb.Append(",{" + rowDataStr + "}");
            else sb.Append("{" + rowDataStr + "}");
        }
        sb.Append("]"); dt.Dispose();
        return sb.ToString();
    }
    public string header_name(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        string colStr = "";
        int width = 0;

        string colsToSumm = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLS_SUM").ToUpper();
        if (colsToSumm == "" || colsToSumm == "-" || colsToSumm == "0")
            colsToSumm = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLS_SUM" + hfLevel.Value).ToUpper();

        string col_name = "";
        string col_aln = "";
        string col_dtype = "";
        string col_func = "";
        int xx = 0;
        string hiddenScript = "hidden : true,";
        foreach (DataColumn dc in dt.Columns)
        {
            if (colStr.Length > 0) colStr += ",";

            if (dt.Rows.Count > 0)
            {
                width = dt.Rows[0][dc.ColumnName.ToUpper()].ToString().Length * 10;

                if (width < 80) width = 80;
                if (dt.Columns.Count < 9)
                {
                    width = width * (dt.Columns.Count < 4 ? 4 : 3);
                }
                if (width > 120) width = 120;
                col_name = dc.ColumnName.ToUpper().ToString();
                totWidthPR += width;
                bool b = colsToSumm.Contains(col_name);
                col_aln = "left";
                col_dtype = "string";
                col_func = "";
                if (b == true)
                {
                    col_aln = "right";
                    col_dtype = "float";
                    col_func = ",render: function (ui) {return $.paramquery.formatCurrency(ui.rowData[ui.dataIndx]);}";
                }


            }
            if (xx > 1) hiddenScript = "";

            //colStr += "{ title : '" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "' , dataIndx : '" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "' , minWidth: " + width + " ,filter: { type: 'select', on: true ,attr: 'multiple' , style : 'height:18px;' , condition: 'range', valueIndx:'" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "' , labelIndx:'" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "' ,prepend: { '': '' } , listeners: ['change'] , init: function () {$(this).pqSelect({ checkbox: true, radio: true, width: '100%' }); } } }";
            colStr += "{ title : '" + dc.ColumnName.Replace("\r\n", "").Replace("&", "").Replace("'", "").Replace("-", "_").Replace(".", "").Replace(",", "") + "' , dataType: '" + col_dtype + "', align: '" + col_aln + "', dataIndx : '" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "' " + col_func + ", minWidth: " + width + " , " + hiddenScript + " filter: { type: 'textbox' , condition: 'contain', listeners: ['keyup']  }}";

            xx++;
        }
        sb.Append("" + colStr + "");
        sb.Append("]");
        dt.Dispose();
        return sb.ToString();
    }
    public string getfilters(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();

        string colStr = "";
        int width = 0;
        foreach (DataColumn dc in dt.Columns)
        {
            //if (colStr.Length > 0) colStr += ",";
            if (dt.Rows.Count > 0)
            {
                width = dt.Rows[0][dc.ColumnName].ToString().Length * 10;
                if (width > 150) width = 150;
            }
            colStr += " filter = grid.getColumn({ dataIndx: '" + dc.ColumnName.Replace("\r\n", "").Replace("&", "").Replace("'", "").Replace("-", "_").Replace(".", "").Replace(",", "") + "' }).filter; filter.cache = null; filter.options=grid.getData({ dataIndx : '" + dc.ColumnName.Replace("\r\n", "").Replace("'", "`") + "' }); ";
        }
        sb.Append("" + colStr + "");

        dt.Dispose();
        return sb.ToString();
    }

    void fillGrid() { }
    //{
    //    if (dt != null)
    //    {
    //        if (!dt.Columns.Contains("FSTR")) dt.Columns.Add("FSTR").SetOrdinal(0);
    //        if (!dt.Columns.Contains("GSTR")) dt.Columns.Add("GSTR").SetOrdinal(1);
    //        DataTable neWDt = dt.Copy();
    //        ViewState["sg1"] = neWDt;
    //        if (frm_formID == "F70156" && hfLevel.Value == "2")
    //            makeSum(dt);
    //        makeColNameAsMine(dt);
    //        sg1.DataSource = dt;
    //        sg1.DataBind();
    //        hideAndRenameCol();
    //        // 
    //        if (frm_formID == "F99147")
    //        {
    //            string mhd = "";
    //            DataTable dtType = new DataTable();
    //            dtType = fgen.getdata(frm_qstr, co_cd, "SELECT TYPE1,vchnum FROM TYPE WHERE ID='B'");
    //            for (int x = 0; x < sg1.Columns.Count; x++)
    //            {
    //                mhd = fgen.seek_iname_dt(dtType, "TYPE1='" + sg1.HeaderRow.Cells[x].Text.Right(2) + "'", "VCHNUM");
    //                if (mhd != "0")
    //                    sg1.HeaderRow.Cells[x].Text = mhd.Replace(" ", "_") + "_" + sg1.HeaderRow.Cells[x].Text.Right(2);
    //            }
    //        }


    //        if (ddSorting.Items.Count <= 0 || ddSorting.DataSource == null)
    //        {
    //            DataTable dtDDCol = new DataTable();
    //            dtDDCol.Columns.Add("COL");
    //            dtDDCol.Columns.Add("VAL");
    //            DataRow drDDCol = null;
    //            for (int i = 0; i < neWDt.Columns.Count; i++)
    //            {
    //                if (neWDt.Columns[i].ColumnName.ToString().Trim().ToUpper() == "FSTR" || neWDt.Columns[i].ColumnName.ToString().Trim().ToUpper() == "GSTR") { }
    //                else
    //                {
    //                    drDDCol = dtDDCol.NewRow();
    //                    drDDCol["COL"] = neWDt.Columns[i].ColumnName.ToString().Trim().toProper();
    //                    drDDCol["VAL"] = dt.Columns[i].ColumnName.ToString().Trim().toProper();
    //                    dtDDCol.Rows.Add(drDDCol);
    //                }
    //            }
    //            if (dtDDCol.Rows.Count > 0)
    //            {
    //                ddSorting.DataSource = dtDDCol;
    //                ddSorting.DataValueField = "Val";
    //                ddSorting.DataTextField = "Col";
    //                ddSorting.DataBind();
    //            }
    //        }
    //        if (ViewState["ddSelectedIndex"] != null)
    //            ddSorting.SelectedIndex = (int)ViewState["ddSelectedIndex"];
    //    }
    //    else
    //    {
    //        sg1.DataSource = null;
    //        sg1.DataBind();
    //    }
    //}

    void makeColNameAsMine(DataTable dtColNameTable)
    { }

    void hideAndRenameCol()
    { }
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0)
        {
            if (frm_formID == "F70717") fgen.exp_to_excel_sp(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim(), frm_formID);
            else
                fgen.exp_to_excel(dt, "ms-excel", "xls", co_cd + "_" + DateTime.Now.ToString().Trim());

            fillPMGrid(dt);
        }
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnexptopdf_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        MakeStdRpt();
        //if (dt.Rows.Count > 0) fgen.exp_to_pdf(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        //else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_word(dt, co_cd + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        int drillBack = fgen.make_int(hfLevel.Value);
        string selVal = "";
        for (int z = 0; z < lblMsgSel.InnerText.Split(':').Length - 1; z++)
        {
            selVal += lblMsgSel.InnerText.Split(':')[z].ToString();
        }
        lblMsgSel.InnerText = selVal;
        drillBack = drillBack - 1;
        if (drillBack == 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LVAL", "");
        }
        if (drillBack < 0)
        {
            //if (drillBack == -1)
            //{
            //    hfLevel.Value = "-1";
            //    lblMsg.InnerText = "Press Esc/Back Button one more time to Exit";
            //}
            //if (drillBack < -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyClose();", true);
            }
        }
        else setDrillLevel(drillBack, drillBack + 1, "");
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e) { }
    //{
    //    txtsearch.Text = "";
    //    var grid = (GridView)sender;
    //    GridViewRow row = sg1.SelectedRow;
    //    int rowIndex = grid.SelectedIndex;
    //    int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
    //    if (selectedCellIndex < 0) selectedCellIndex = 0;
    //    string mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
    //    if (selectedCellIndex > 0) selectedCellIndex -= 1;

    //    string Value1 = row.Cells[1].Text.Trim();
    //    string Value2 = row.Cells[3].Text.Trim();
    //    string Value3 = row.Cells[4].Text.Trim();
    //    if (Convert.ToInt32(hfLevel.Value.toDouble().ToString()) == -1) hfLevel.Value = "0";
    //    int drillPost = Convert.ToInt32(hfLevel.Value.toDouble().ToString());
    //    drillPost = drillPost + 1;
    //    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");
    //    switch (frm_formID)
    //    {
    //        case "F70156":
    //        case "F70189":
    //        case "F70189A":
    //            if ((drillPost - 1) == 0)
    //            {
    //                if (selectedCellIndex < 4)
    //                {
    //                    Value2 = row.Cells[3].Text.Trim();
    //                    Value1 = row.Cells[7].Text.Trim();
    //                }
    //                else
    //                {
    //                    Value1 = row.Cells[8].Text.Trim();
    //                    Value2 = row.Cells[5].Text.Trim();
    //                }
    //            }
    //            break;
    //    }

    //    if (setDrillLevel(drillPost, drillPost - 1, Value1).Length > 2)
    //    {
    //        if (Value2 != "-") lblMsgSel.InnerText += " : " + Value2.Replace("&amp;", "&");
    //        lblMsgSel.InnerText = lblMsgSel.InnerText.Replace(": All", "");
    //    }
    //    else
    //    {
    //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "N");
    //        // here to add any condition
    //        if (((frm_formID == "F70556" || frm_formID == "F70230A") && drillPost == 3) || (frm_formID == "F70189" || frm_formID == "F70189A" || frm_formID == "F70156" && drillPost == 5))
    //        {
    //            hideAndRenameCol();
    //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
    //            if (Value1.Length > 2)
    //            {
    //                string myFormName = "", myFormID = "";
    //                switch (Value1.Substring(2, 1))
    //                {
    //                    case "5":
    //                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
    //                        myFormName = "../tej-base/om_pinv_entry.aspx";
    //                        myFormID = "@F70116";
    //                        break;
    //                    case "4":
    //                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
    //                        myFormName = "../tej-base/om_inv_entry.aspx";
    //                        myFormID = "@F50101";
    //                        break;
    //                    case "2":
    //                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
    //                        myFormName = "../tej-base/om_rcpt_vch.aspx";
    //                        myFormID = "@F70106";
    //                        break;
    //                    case "1":
    //                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
    //                        myFormName = "../tej-base/om_rcpt_vch.aspx";
    //                        myFormID = "@F70101";
    //                        break;
    //                    default:
    //                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
    //                        myFormName = "../tej-base/om_mul_vch.aspx";
    //                        myFormID = "@F70116";
    //                        break;
    //                }
    //                //|| frm_formID == "F70156" || frm_formID == "F70189" || frm_formID == "F70189A"
    //                if (frm_formID == "F70230A")
    //                {
    //                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
    //                    myFormName = "../tej-base/om_mul_vch.aspx";
    //                    myFormID = "@F70116";
    //                }
    //                else
    //                {
    //                    if (Convert.ToDateTime(fgen.make_def_Date(Value3, DateTime.Now.ToString("dd/MM/yyyy"))) < Convert.ToDateTime("13/01/2021") && (co_cd == "SGRP" || co_cd == "UATS" || co_cd == "UAT2"))
    //                    {
    //                        myFormID = "@F70111";
    //                        myFormName = "../tej-base/om_jour_vch.aspx";
    //                    }
    //                }
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + myFormName + "?STR=" + frm_qstr + myFormID + "','98%','98%','');", true);
    //            }
    //        }
    //    }
    //}
    string setDrillLevel(int dLevel, int oldDLevel, string valSelected)
    {
        if (dLevel == 0) lblMsgSel.InnerText = " : All";

        divChkview.Visible = false;
        if ((frm_formID == "F70230A" || frm_formID == "F70556") && dLevel == 2)
        {
            divChkview.Visible = true;
        }
        trDetail.Visible = false;
        if (frm_formID == "F45152" && dLevel == 2)
        {
            string numbr_fmt = "999,999,999.99";
            string numbr_fmt2 = "999,999,999";
            string mhd = fgen.seek_iname(frm_qstr, co_cd, "SELECT BR_CURREN||'~'||'1000'||'~'||'000'||'~'||NUM_FMT1||'~'||NUM_FMT2 AS FSTR FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "' ", "FSTR");
            if (mhd != "0")
            {
                numbr_fmt = (mhd.Split('~')[3] == "" || mhd.Split('~')[3] == "-" || mhd.Split('~')[3] == "0") ? numbr_fmt : mhd.Split('~')[3];
                numbr_fmt2 = (mhd.Split('~')[4] == "" || mhd.Split('~')[4] == "-" || mhd.Split('~')[4] == "0") ? numbr_fmt2 : mhd.Split('~')[4];
            }
            string cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
            string cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
            string xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
            string DateRange = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + cDT2 + "','dd/mm/yyyy') ";
            string mthtgt = fgenMV.Fn_Get_Mvar(frm_qstr, "MTH_TGT");
            string branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "BR_COND");
            string br_cond = fgenMV.Fn_Get_Mvar(frm_qstr, "BR_CONDSO");
            switch (valSelected.Left(1))
            {
                case "A": // Company wise
                    Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' as affiliate_Code, '-' AS affiliate_name,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,(case when nvl(A.MFGINBR,'-')!='-' then A.MFGINBR else a.branchcd end) as bcd from somas a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,A.BRANCHCD from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,A.BRANCHCD from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + "  union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,A.BRANCHCD from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + " ) a where trim(a.seg_name)='" + valSelected.Right(4) + "' having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+sum(a.sale_tgt)+SUM(A.sale_book))>0 union all (SELECT 'A'||A.SEG_NAME as fstr,'A'||A.SEG_NAME as gstr, A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,a.bcd as affiliate_Code, C.NAME AS affiliate_name,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,(case when nvl(A.MFGINBR,'-')!='-' then A.MFGINBR else a.branchcd end) as bcd from somas a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,A.BRANCHCD from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,A.BRANCHCD from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + "  union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,A.BRANCHCD from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + " ) a,typegrp b,type c where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) and trim(a.bcd)=trim(c.type1) and c.id='B' GROUP BY A.SEG_NAME,B.NAME,c.name,a.bcd  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+sum(a.sale_tgt)+SUM(A.sale_book))>0 )";
                    break;
                case "B": // Customer wise
                    Squery = " SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS CODE, '-' AS CUSTOMER,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,TRIM(A.ACODE) AS ACODE from somas a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,TRIM(A.ACODE) from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,trim(a.icode) from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + "  union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,trim(a.icode) from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + " ) a where trim(a.seg_name)='" + valSelected.Right(4) + "' UNION ALL (SELECT 'B'||A.SEG_NAME as fstr,'B'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.ACODE AS CODE, A.ANAME AS CUSTOMER,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,TRIM(A.ACODE) AS ACODE from somas a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,TRIM(A.ACODE) from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,trim(a.icode) from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + "  union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,trim(a.icode) from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + " ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME, A.ANAME,A.ACODE  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0 )";
                    break;
                case "X":// Product Catg wise
                    //Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS ERPCODE, '-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,substr(a.sale_rep,1,4) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,substr(a.sale_Rep,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C5%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C6%' and a.vchdate " + xprdrange + "  ) a  where trim(a.seg_name)='" + valSelected.Right(4) + "'  UNION ALL (SELECT 'C'||A.SEG_NAME as fstr,'C'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.ICODE AS ERPCODE, A.INAME AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,substr(a.sale_rep,1,4) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,substr(a.sale_Rep,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C5%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C6%' and a.vchdate " + xprdrange + "  ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,A.ICODE, A.INAME  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0 ) ";
                    Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a  where trim(a.seg_name)='" + valSelected.Right(4) + "'  UNION ALL (SELECT 'X'||A.SEG_NAME as fstr,'X'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.REP_DIM10 AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,C.REP_DIM10 from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,C.REP_DIM10 from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,C.REP_DIM10 from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,C.REP_DIM10 from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,a.REP_DIM10 having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0 ) ";
                    Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book from somas a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a  where trim(a.seg_name)='" + valSelected.Right(4) + "'  UNION ALL (SELECT 'X'||A.SEG_NAME as fstr,'X'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.REP_DIM10||' : '||V.NAME AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,substr(C.REP_DIM10,1,3) as REP_DIM10 from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,substr(c.rep_dim10,1,3) as REP_DIM10 from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,trim(a.CATG_CODE) from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode)  and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,trim(a.CATG_CODE) from wb_budg_ctrl a,famst b where trim(A.icode)=trim(B.acode) and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a,typegrp b,typegrp v where trim(b.id)='EM' and v.id='#9' AND TRIM(A.REP_DIM10)=TRIM(V.TYPE1) and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,a.REP_DIM10,V.NAME having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0 ) ";
                    break;
                case "C": // Product wise
                    //Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS ERPCODE, '-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,substr(a.sale_rep,1,4) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,substr(a.sale_Rep,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C5%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C6%' and a.vchdate " + xprdrange + "  ) a  where trim(a.seg_name)='" + valSelected.Right(4) + "'  UNION ALL (SELECT 'C'||A.SEG_NAME as fstr,'C'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.ICODE AS ERPCODE, A.INAME AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,substr(a.sale_rep,1,4) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,substr(a.sale_Rep,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C5%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C6%' and a.vchdate " + xprdrange + "  ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,A.ICODE, A.INAME  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0 ) ";
                    Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS ERPCODE, '-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a  where trim(a.seg_name)='" + valSelected.Right(4) + "'  UNION ALL (SELECT 'C'||A.SEG_NAME as fstr,'C'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.ICODE AS ERPCODE, A.INAME AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,A.ICODE, A.INAME  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0 ) ";
                    break;
                case "D": // Customer , Product wise
                    Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS CODE,'-' as customer,'-' AS ERPCODE,'-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,substr(a.sale_rep,1,4) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,substr(a.sale_Rep,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C5%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C6%' and a.vchdate " + xprdrange + "  ) a where trim(a.seg_name)='" + valSelected.Right(4) + "' UNION ALL (SELECT 'D'||A.SEG_NAME as fstr,'D'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.ACODE AS CODE,a.aname as customer,A.ICODE AS ERPCODE, A.INAME AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,substr(a.sale_rep,1,4) as Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,substr(a.sale_Rep,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C5%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') and a.type like 'C6%' and a.vchdate " + xprdrange + "  ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,A.ICODE, A.INAME,a.aname,a.acode  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0)";
                    Squery = "SELECT '" + valSelected + "' as fstr,'" + valSelected + "' as gstr, 'Grand Total' AS sale_person_CODE,'-' AS SALES_PERSON_NAME,'-' AS CODE,'-' as customer,'-' AS ERPCODE,'-' AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a where trim(a.seg_name)='" + valSelected.Right(4) + "' UNION ALL (SELECT 'D'||A.SEG_NAME as fstr,'D'||A.SEG_NAME as gstr,A.SEG_NAME AS sale_person_CODE,B.NAME AS SALES_PERSON_NAME,A.ACODE AS CODE,a.aname as customer,A.ICODE AS ERPCODE, A.INAME AS PRODUCT_Catg,to_char(sum(a.ord_tgt),'" + numbr_fmt2 + "') as target_order_value,to_char(SUM(A.ORD_BOOK),'" + numbr_fmt2 + "') AS actual_order_book,(case when sum(a.ord_tgt)>0 then round((sum(a.ORD_BOOK)/sum(a.ord_tgt))*100,2) else 0 end) as Percent_order,to_Char(sum(a.sale_tgt),'" + numbr_fmt2 + "') as dispatch_target,to_char(SUM(A.sale_book),'" + numbr_fmt2 + "') AS Actual_Dispatch_value,(case when sum(a.sale_tgt)>0 then round((sum(a.sale_book)/sum(a.sale_tgt))*100,2) else 0 end) as Percent_Dispatch FROM (select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,a.irate*a.qtyord as ord_book,0 as sale_tgt,0 as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from somas a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(C.ICODe) and length(Trim(c.icode))=4 and a.branchcd not in ('DD','88') " + br_cond + " and type like '4%' and a.orddt " + xprdrange + " union all select B.ANAME,(case when nvl(A.sale_Rep,'-')!='-' then substr(a.sale_rep,1,4) else substr(b.mktggrp,1,4) end) as  Seg_Name,0 as ord_tgt,0 as ord_book,0 as sale_tgt,a.iamount as sale_book,C.INAME,TRIM(c.ICODE) AS ICODE,trim(a.acode) as acode from ivoucher a,famst b,ITEM C where trim(A.acode)=trim(B.acode) AND substr(TRIM(a.ICODE),1,4)=TRIM(c.ICODE) and length(Trim(c.icode))=4 and " + branch_Cd + " and type like '4%' and a.vchdate " + xprdrange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name," + mthtgt + " as ord_tgt,0 as ord_book,0 as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C5%' and a.vchdate " + DateRange + " union all select B.ANAME,substr(a.acode,1,4) as Seg_Name,0 as ord_tgt,0 as ord_book," + mthtgt + " as sale_tgt,0 as sale_book,c.INAME,TRIM(c.ICODE) AS ICODE,trim(a.icode) as acode from wb_budg_ctrl a,famst b,item c where trim(A.icode)=trim(B.acode) and trim(a.CATG_CODE)=substr(c.rep_dim10,1,3) and length(Trim(c.icode))=4 and " + branch_Cd + " and a.type like 'C6%' and a.vchdate " + DateRange + "  ) a,typegrp b where trim(b.id)='EM' and trim(A.SEG_NAME)=trim(b.type1) GROUP BY A.SEG_NAME,B.NAME,A.ICODE, A.INAME,a.aname,a.acode  having (sum(a.ord_tgt)+SUM(A.ORD_BOOK)+SUM(A.sale_book)+SUM(A.sale_tgt))>0)";
                    break;
            }
        }
        else if ((frm_formID == "F70556" || frm_formID == "F70650") && dLevel != 0)
        {
            Squery = myQuery(dLevel, frm_qstr, valSelected);

            if (dLevel == 2) trDetail.Visible = true;
        }
        else Squery = fgen.getDrillQuery(dLevel, frm_qstr);

        btnBack.Visible = true;
        if (Squery == "" || Squery == "0")
        {
            //"No Drill Further"       
        }
        else if (Squery.ToUpper() == "SEND_DT")
        {
            hfLevel.Value = "0";
            fill_grid("");
        }
        else
        {
            lblMsg.InnerText = "Drill Level - " + (dLevel + 1);
            string fstrGstr = "GSTR";
            if (oldDLevel > dLevel) fstrGstr = "GSTR";
            if (Squery.Contains("='GSTR'") || Squery.Contains("='FSTR'"))
            {
                if (valSelected == "") valSelected = fgenMV.Fn_Get_Mvar(frm_qstr, "U_LVAL" + dLevel);
                if (valSelected == "0" || valSelected == "") Squery = "" + Squery;
                else
                {
                    if (dLevel == 2 && valSelected == "00-")
                    {
                        Squery = Squery.Replace("GSTR='FSTR'", "ACODE='" + hfACode.Value + "' ");
                    }
                    else
                    {
                        Squery = Squery.Replace("='GSTR'", "='" + valSelected + "'");
                        Squery = Squery.Replace("='FSTR'", "='" + valSelected + "'");
                    }
                    Squery = "select * from (" + Squery + ") ";
                }
            }
            else
            {
                if (valSelected == "") valSelected = fgenMV.Fn_Get_Mvar(frm_qstr, "U_LVAL" + dLevel);
                if (valSelected == "0" || valSelected == "") Squery = "" + Squery;
                else Squery = "select * from (" + Squery + ") where " + fstrGstr + "='" + valSelected + "' ";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LVAL" + dLevel, valSelected);
            hfLevel.Value = dLevel.ToString();
            hfvalSelected.Value = valSelected;
            fill_grid(Squery);
        }
        return Squery;
    }
    protected void txtsearch_TextChanged(object sender, EventArgs e)
    {
        searchFunc();
    }
    void makeSum(DataTable newDt)
    { }
    protected void btnexptocsv_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0)
        {
            string zipFilePath = "c:\\TEJ_erp\\Upload\\" + co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";
            fgen.CreateCSVFile(dt, zipFilePath);
            Session["FileName"] = co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";
            Session["FilePath"] = co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".csv";

            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");

        }
        else fgen.msg("-", "AMSG", "No Data to Export");
        dt.Dispose();
    }
    protected void sg1_Sorting(object sender, GridViewSortEventArgs e)
    { }

    private void MakeStdRpt() { }
    //{
    //    dt = new DataTable();
    //    dt = (DataTable)ViewState["sg1"];
    //    if (dt.Columns.Contains("FSTR")) dt.Columns.Remove("FSTR");
    //    if (dt.Columns.Contains("GSTR")) dt.Columns.Remove("GSTR");
    //    DataTable dt10Col = new DataTable();
    //    dt10Col.Columns.Add("Header", typeof(string));
    //    dt10Col.Columns.Add("FromDt", typeof(string));
    //    dt10Col.Columns.Add("ToDt", typeof(string));
    //    dt10Col.Columns.Add("F1", typeof(string));
    //    dt10Col.Columns.Add("F2", typeof(string));
    //    dt10Col.Columns.Add("F3", typeof(string));
    //    dt10Col.Columns.Add("F4", typeof(string));
    //    dt10Col.Columns.Add("F5", typeof(string));
    //    dt10Col.Columns.Add("F6", typeof(string));
    //    dt10Col.Columns.Add("F7", typeof(string));
    //    dt10Col.Columns.Add("F8", typeof(string));
    //    dt10Col.Columns.Add("F9", typeof(string));
    //    dt10Col.Columns.Add("F10", typeof(string));
    //    dt10Col.Columns.Add("H1", typeof(string));
    //    dt10Col.Columns.Add("H2", typeof(string));
    //    dt10Col.Columns.Add("H3", typeof(string));
    //    dt10Col.Columns.Add("H4", typeof(string));
    //    dt10Col.Columns.Add("H5", typeof(string));
    //    dt10Col.Columns.Add("H6", typeof(string));
    //    dt10Col.Columns.Add("H7", typeof(string));
    //    dt10Col.Columns.Add("H8", typeof(string));
    //    dt10Col.Columns.Add("H9", typeof(string));
    //    dt10Col.Columns.Add("H10", typeof(string));
    //    int colCount = 1, colIndex = 1, dtColIndex = 0;
    //    colCount = dt.Columns.Count;
    //    string fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
    //    string todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
    //    string header_n = fgenMV.Fn_Get_Mvar(frm_qstr, "U_HEADER");
    //    if (colCount > 10)
    //    {
    //        colCount = 10;
    //    }
    //    if (header_n == "0")
    //    {
    //        if (Session["dt_menu" + frm_qstr] != null)
    //        {
    //            DataTable dtx = new DataTable();
    //            dtx = (DataTable)Session["dt_menu" + frm_qstr];

    //            header_n = fgen.seek_iname_dt(dtx, "ID='" + frm_formID + "'", "TEXT");
    //        }
    //    }
    //    DataRow dr1 = null;
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        dr1 = dt10Col.NewRow();
    //        dr1["Header"] = header_n;
    //        dr1["FromDt"] = fromdt;
    //        dr1["ToDt"] = todt;
    //        colIndex = 1;
    //        dtColIndex = 0;
    //        for (int k = 1; k <= colCount; k++)
    //        {
    //            dr1["F" + k] = dt.Rows[i][dtColIndex].ToString().Trim(); // field's data
    //            dr1["H" + k] = dt.Columns[dtColIndex].ColumnName; // field column name
    //            colIndex++;
    //            dtColIndex++;
    //        }
    //        dt10Col.Rows.Add(dr1);
    //    }

    //    string xml = "10ColStd";
    //    DataSet data_set = new DataSet();
    //    string report = "10ColStd";
    //    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
    //    dt10Col.TableName = "Prepcur";
    //    data_set.Tables.Add(dt10Col);
    //    string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
    //    string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
    //    data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_cd, frm_mbr));
    //    data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
    //    if (data_set.Tables[0].Rows.Count > 0)
    //    {
    //        CrystalReportViewer1.DisplayPage = true;
    //        CrystalReportViewer1.DisplayToolbar = true;
    //        CrystalReportViewer1.DisplayGroupTree = false;
    //        CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
    //        CrystalReportViewer1.DataBind();
    //        string frm_FileName = co_cd + "_" + DateTime.Now.ToString().Trim();
    //        repDoc = GetReportDocument(data_set, rptfile);
    //        repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
    //    }
    //}
    ReportDocument repDoc = new ReportDocument();
    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        repDoc.Close();
        repDoc.Dispose();
    }
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
    protected void sg1_PageIndexChanged(object sender, EventArgs e)
    {

    }
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    { }
    protected void ddSorting_SelectedIndexChanged(object sender, EventArgs e)
    {
        sortin();
    }
    void sortin()
    {
        var D = (DataTable)ViewState["sg1"];
        if (D.Columns.Count > 2)
        {
        }
    }
    protected void radsor_SelectedIndexChanged(object sender, EventArgs e)
    {
        sortin();
    }
    string numWithComma(string valueToChange, string format)
    {
        return fgen.seek_iname(frm_qstr, co_cd, "SELECT TO_CHAR(" + valueToChange + ",'" + format + "') as val from DUAL", "VAL");
    }
    protected void btnRep1_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", frm_mbr);
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70241");
        fgen.fin_acct_reps(frm_qstr);
    }
    protected void btnRep2_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", frm_mbr);
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70242");
        fgen.fin_acct_reps(frm_qstr);
    }
    protected void btnRep3_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "'" + frm_mbr + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70230");
        fgen.fin_acct_reps(frm_qstr);
    }
    protected void btnRep4_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "'" + frm_mbr + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70229");
        fgen.fin_acct_reps(frm_qstr);
    }
    protected void btnRep5_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", frm_mbr);
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70148");
        fgen.fin_acct_reps(frm_qstr);
    }
    protected void btnRep6_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", frm_mbr);
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70149");
        fgen.fin_acct_reps(frm_qstr);
    }
    protected void btnRep7_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "2") hffield.Value = "1";
        else hffield.Value = "2";

        if (hfLevel.Value == "0")
        {
            Squery = fgen.getDrillQuery(hfLevel.Value.toInt(), frm_qstr);
            fill_grid(Squery);
        }
    }
    protected void btnExcel2_Click(object sender, ImageClickEventArgs e)
    {
        Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        Excel.Range xlRange;
        string xlFilePath;
        xlFilePath = "";
        double g = 0, h = 0, i = 0, j = 0;
        string nickName = fgen.seek_iname(frm_qstr, co_cd, "Select vchnum from type where id='B' AND TYPE1='" + frm_mbr + "'", "VCHNUM");
        switch (frm_formID)
        {
            case "F70713":
                xlFilePath = Server.MapPath("//tej-base//myFiles//" + nickName + "-RCV.xlsx");
                //xlFilePath = "c://tej_Erp//" + nickName + "-RCV.xlsx";
                break;
            case "F70717":
                xlFilePath = Server.MapPath("//tej-base//myFiles//" + nickName + "-PAY.xlsx");
                //xlFilePath = "c://tej_Erp//" + nickName + "-PAY.xlsx";
                break;
        }

        int iRow;

        xlApp = new Excel.Application();
        xlWorkBook = xlApp.Workbooks.Open(xlFilePath);

        xlWorkSheet = (Excel.Worksheet)xlApp.ActiveSheet;

        var withBlock = xlWorkSheet;

        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];

        DataTable dt1 = new DataTable();
        string SQuery = "select a.icode_grp, a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.EXC_rATE,b.name as mgname,c.iname as name,c.nl_iname from (select substr(trim(icode),1,2) as icode_grp, invno,invdate,EXC_rATE,vchdate,vchnum,trim(icode) as icode from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' order by invno,vchdate desc) a , type b, item c where substr(a.icode,1,4)=trim(c.icode) and b.id='Y' and a.icode_grp=trim(b.type1) ";
        dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);
        string col1 = "", col2 = "", col3 = "";
        double mq0 = 0;

        withBlock.Cells[2, 10] = DateTime.Now.ToShortDateString() + " Report";
        string frmdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        string todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        string nl_iname = "";
        //=============
        string xprdrange = "between to_date('" + frmdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
        //========yogita...24.06.2021  for purchase voucher no                  
        //        Squery = "select distinct a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as vchdate,max(a.invno) invno,to_char(a.invdate,'dd-mm-yyyy') as invdate,a.acode,a.rcode,a.srno ,trim(b.aname) as supp from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.vchdate " + xprdrange + " and a.srno=1 group by a.vchnum, to_Char(a.vchdate,'dd/mm/yyyy'),to_char(a.invdate,'dd-mm-yyyy'),a.acode,a.rcode,a.srno,trim(b.aname) order by invno desc";//old
        //  Squery = "select distinct a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as vchdate,max(a.invno) invno,to_char(a.invdate,'dd-mm-yyyy') as invdate,a.acode,a.rcode,a.srno ,trim(b.aname) as supp,d.mattype,max(d.exc_rate) as rate from voucher a,famst b,ivoucher d where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_Char(d.vchdate,'dd/mm/yyyy') and trim(a.rcode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.vchdate " + xprdrange + " and a.srno=1 group by a.vchnum, to_Char(a.vchdate,'dd/mm/yyyy'),to_char(a.invdate,'dd-mm-yyyy'),a.acode,a.rcode,a.srno,trim(b.aname),d.mattype order by invno desc";
        //  Squery = "select distinct a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as vchdate,max(a.invno) invno,to_char(a.invdate,'dd-mm-yyyy') as invdate,a.acode,a.rcode,a.srno ,trim(b.aname) as supp,d.mattype,max(d.exc_rate) as rate,trim(b.gst_no) as gstno,sum(a.fcrate1) as tot_pur from voucher a left join ivoucher d on trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_Char(d.vchdate,'dd/mm/yyyy') ,famst b where  trim(a.rcode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.vchdate  " + xprdrange + " and a.srno=1 group by a.vchnum, to_Char(a.vchdate,'dd/mm/yyyy'),to_char(a.invdate,'dd-mm-yyyy'),a.acode,a.rcode,a.srno,trim(b.aname),d.mattype,trim(b.gst_no) order by invno desc";
        Squery = "select distinct a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as vchdate,max(a.invno) invno,to_char(a.invdate,'dd-mm-yyyy') as invdate,a.acode,a.rcode,sum(wb.amt_SAle) as tot_pur,sum(wb.amt_exc) as vat_Amt,sum(wb.bill_tot) as billtot,a.srno ,trim(b.aname) as supp,d.mattype,max(d.exc_rate) as rate,trim(b.gst_no) as gstno,sum(a.fcrate1) as tot_pur,sum(wb.fob_tot) as inv_Val from voucher a left join ivoucher d on trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_Char(d.vchdate,'dd/mm/yyyy') ,famst b,wb_pv_head wb where  trim(a.rcode)=trim(b.acode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(wb.branchcd)||trim(wb.type)||trim(wb.vchnum)||to_Char(wb.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.vchdate  " + xprdrange + " and a.srno=1 group by a.vchnum, to_Char(a.vchdate,'dd/mm/yyyy'),to_char(a.invdate,'dd-mm-yyyy'),a.acode,a.rcode,a.srno,trim(b.aname),d.mattype,trim(b.gst_no) order by invno desc";

        DataTable purdt = new DataTable();
        purdt = fgen.getdata(frm_qstr, co_cd, Squery); //pur voucher dt
        Squery = "select type1 as fstr,type1 as code,name as vat_category from type where ID='}'  ORDER BY TYPE1";
        DataTable vatcatg = new DataTable();
        vatcatg = fgen.getdata(frm_qstr, co_cd, Squery); //vat category dt
        string purvchno, vchdt = "", vat_catg = "", supplier = "", enganame = "", gstno = ""; double rate = 0, tot_pur = 0, vat_Amt = 0, billtot = 0, inv_Val = 0;
        //=============


        if (frm_formID == "F70717")
        { //this is add by yogita as name is coming sale on pur report...as per sumit sir
            withBlock.Cells[3, 3] = "Purchase Register Report For The Period From : " + frmdt + " To " + todt + "";
        }
        else
        {
            withBlock.Cells[3, 3] = "Sales Register Report For The Period From : " + frmdt + " To " + todt + "";
        }

        if (frm_formID == "F70717") //by yogita
        {
            withBlock.Cells[4, 3] = " " + frmdt + " " + ":" + "ل " + "" + todt + "" + "  : سجل الشراء الشهري للفترة من";//by yogita
        }
        else
        {
            withBlock.Cells[4, 3] = frmdt + "" + ":" + "الى  " + "" + todt + "" + "  : سجل المبيعات  الشهرى للفتره  من";//old
        }

        int cnt = 0;
        for (iRow = 0; iRow < dt.Rows.Count; iRow++)
        {

            withBlock.Cells[iRow + 7, 1] = iRow + 1;
            if (frm_formID == "F70717") //this is add by yogita due to need voucher no,datein excel show in colm 2,3 in excel as per sumit sir
            {
                purvchno = ""; vchdt = ""; supplier = ""; gstno = ""; tot_pur = 0; vat_Amt = 0; billtot = 0; inv_Val = 0;
                if (dt.Rows[iRow]["inv_no"].ToString().Length > 1)
                {
                    purvchno = fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "vchnum");
                    vchdt = fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "vchdate");
                    supplier = fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "supp");

                    gstno = fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "gstno");
                    tot_pur = fgen.make_double(fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "tot_pur"));
                    vat_Amt = fgen.make_double(fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "vat_Amt"));
                    billtot = fgen.make_double(fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "billtot"));
                    inv_Val = fgen.make_double(fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "inv_Val"));
                    withBlock.Cells[iRow + 7, 2] = purvchno + "" + "";
                    withBlock.Cells[iRow + 7, 3] = vchdt + "" + "";
                    withBlock.Cells[iRow + 7, 4] = supplier + "";
                    withBlock.Cells[iRow + 7, 5] = gstno + "" + "";
                    vat_catg = fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "mattype");
                    withBlock.Cells[iRow + 7, 14] = fgen.seek_iname_dt(vatcatg, "code='" + vat_catg + "'", "vat_category");
                    rate = fgen.make_double(fgen.seek_iname_dt(purdt, "invno='" + dt.Rows[iRow]["pv_no"].ToString() + "' and invdate='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "rate"));//percentage in excel
                    withBlock.Cells[iRow + 7, 13] = rate + "" + "";
                }
            }
            else
            {
                if (dt.Rows[iRow]["inv_no"].ToString().Length > 3)
                    withBlock.Cells[iRow + 7, 2] = dt.Rows[iRow]["inv_no"].ToString() + "" + "";
                else
                    withBlock.Cells[iRow + 7, 2] = dt.Rows[iRow]["inv_no"].ToString();
                withBlock.Cells[iRow + 7, 3] = Convert.ToDateTime(dt.Rows[iRow]["inv_dt"].ToString()).ToString("dd-MMM-yy");
                withBlock.Cells[iRow + 7, 4] = dt.Rows[iRow]["Party_arabic"].ToString();

                withBlock.Cells[iRow + 7, 14] = dt.Rows[iRow]["party_name"].ToString();
                withBlock.Cells[iRow + 7, 15] = dt.Rows[iRow]["party_code"].ToString();
            }
            if (frm_formID != "F70717")//this by yogita because vat no filled in above case
            {
                if (dt.Rows[iRow]["Vat_number"].ToString().Length > 3)
                    withBlock.Cells[iRow + 7, 5] = dt.Rows[iRow]["Vat_number"].ToString() + "" + "";

                else
                    withBlock.Cells[iRow + 7, 5] = dt.Rows[iRow]["Vat_number"].ToString();
            }
            if (dt1.Rows.Count > 0)
            {
                if (frm_formID == "F70717")
                {
                    if (dt.Rows[iRow]["pv_dt"].ToString().Length > 0)
                    {
                        col1 = fgen.seek_iname_dt(dt1, "pv_no='" + dt.Rows[iRow]["pv_no"].ToString() + "' and pv_dt='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "name");
                        nl_iname = fgen.seek_iname_dt(dt1, "pv_no='" + dt.Rows[iRow]["pv_no"].ToString() + "' and pv_dt='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "nl_iname");
                        col3 = fgen.seek_iname_dt(dt1, "pv_no='" + dt.Rows[iRow]["pv_no"].ToString() + "' and pv_dt='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "vat_cat");
                        if (col1 == "0")
                        {
                            col2 = dt.Rows[iRow]["pv_no"].ToString().Right(6);
                            col1 = fgen.seek_iname_dt(dt1, "trim(pv_no)='" + col2.Trim() + "' and pv_dt='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "name");
                        }
                        if (col3 == "0")
                        {
                            col2 = dt.Rows[iRow]["pv_no"].ToString().Right(6);
                            col3 = fgen.seek_iname_dt(dt1, "trim(pv_no)='" + col2.Trim() + "' and pv_dt='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "vat_cat");
                        }
                        mq0 = fgen.make_double(fgen.seek_iname_dt(dt1, "pv_no='" + dt.Rows[iRow]["pv_no"].ToString() + "' and pv_dt='" + Convert.ToDateTime(dt.Rows[iRow]["pv_dt"].ToString()).ToShortDateString() + "'", "EXC_rATE"));

                    }
                    else
                    {
                        col1 = "";
                        mq0 = 0;
                        col3 = "";
                    }
                }
                else
                {
                    if (dt.Rows[iRow]["inv_dt"].ToString().Length > 0)
                    {
                        col1 = fgen.seek_iname_dt(dt1, "invno='" + dt.Rows[iRow]["inv_no"].ToString() + "' and invdate='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "name");
                        nl_iname = fgen.seek_iname_dt(dt1, "invno='" + dt.Rows[iRow]["inv_no"].ToString() + "' and invdate='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "nl_iname");
                        col3 = fgen.seek_iname_dt(dt1, "invno='" + dt.Rows[iRow]["inv_no"].ToString() + "' and invdate='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "vat_cat");
                        if (col1 == "0")
                        {
                            col2 = dt.Rows[iRow]["inv_no"].ToString().Right(6);
                            col1 = fgen.seek_iname_dt(dt1, "trim(invno)='" + col2.Trim() + "' and invdate='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "name");
                        }
                        if (col3 == "0")
                        {
                            col2 = dt.Rows[iRow]["inv_no"].ToString().Right(6);
                            col3 = fgen.seek_iname_dt(dt1, "trim(invno)='" + col2.Trim() + "' and invdate='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "vat_cat");
                        }
                        mq0 = fgen.make_double(fgen.seek_iname_dt(dt1, "invno='" + dt.Rows[iRow]["inv_no"].ToString() + "' and invdate='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "EXC_rATE"));
                        if (mq0 == 0)
                            mq0 = fgen.make_double(fgen.seek_iname_dt(dt, "inv_no='" + dt.Rows[iRow]["inv_no"].ToString() + "' and inv_dt='" + dt.Rows[iRow]["inv_dt"].ToString() + "'", "vat_percent"));
                    }
                    else
                    {
                        col1 = "";
                        mq0 = 0;
                        col3 = "";
                    }
                }

            }
            if (col1 == "0") col1 = "-";
            if (col3 == "0" || col3 == "") col3 = "-";
            if (frm_formID == "F70717")
            {
                withBlock.Cells[iRow + 7, 6] = dt.Rows[iRow]["pv_no"].ToString() + "" + ""; //this is add by yogita as per sumit sir show here invno
                withBlock.Cells[iRow + 7, 8] = tot_pur + "" + "";
                withBlock.Cells[iRow + 7, 11] = vat_Amt + "" + "";
                withBlock.Cells[iRow + 7, 12] = inv_Val + "" + "";
            }
            else
            {
                withBlock.Cells[iRow + 7, 6] = nl_iname;
                if (frm_formID == "F70713") withBlock.Cells[iRow + 7, 8] = dt.Rows[iRow]["sale_without_vat"].ToString();
                else
                    withBlock.Cells[iRow + 7, 8] = dt.Rows[iRow]["sale_without_vat"].ToString();
                withBlock.Cells[iRow + 7, 11] = mq0 + "%";
                withBlock.Cells[iRow + 7, 12] = col3;
            }

            if (frm_formID == "F70713")
            {
                withBlock.Cells[iRow + 7, 7] = dt.Rows[iRow]["sale_subject_to_vat"].ToString();

                withBlock.Cells[iRow + 7, 12] = dt.Rows[iRow]["VAT_CATEGORY"].ToString();
            }
            else
                withBlock.Cells[iRow + 7, 7] = dt.Rows[iRow]["total_value"].ToString();
            withBlock.Cells[iRow + 7, 9] = dt.Rows[iRow]["Total_Vat"].ToString();
            if (frm_formID == "F70713")
            {
                withBlock.Cells[iRow + 7, 10] = fgen.make_double(dt.Rows[iRow]["total_value"].ToString());
            }
            else
            {
                withBlock.Cells[iRow + 7, 10] = fgen.make_double(dt.Rows[iRow]["total_value"].ToString()) + fgen.make_double(dt.Rows[iRow]["Total_Vat"].ToString());
            }

            withBlock.Cells[iRow + 7, 13] = col1;

            cnt++;


            if (frm_formID == "F70713")
            {
                try
                {
                    g += (withBlock.Cells[iRow + 7, 7] as Excel.Range).Value.ToString().toDouble();
                    h += (withBlock.Cells[iRow + 7, 8] as Excel.Range).Value.ToString().toDouble();
                    i += (withBlock.Cells[iRow + 7, 9] as Excel.Range).Value.ToString().toDouble();
                    j += (withBlock.Cells[iRow + 7, 10] as Excel.Range).Value.ToString().toDouble();
                }
                catch { }
            }
        }


        if (g > 0 || h > 0 || i > 0 || j > 0)
        {
            withBlock.Cells[iRow + 7 + 2, 6] = "Total";
            withBlock.Cells[iRow + 7 + 2, 7] = g;
            withBlock.Cells[iRow + 7 + 2, 8] = h;
            withBlock.Cells[iRow + 7 + 2, 9] = i;
            withBlock.Cells[iRow + 7 + 2, 10] = j;
        }

        string fileNamexxx = "";
        switch (frm_formID)
        {
            case "F70713":
                fileNamexxx = nickName + "-RCV_1.xlsx";
                break;
            case "F70717":
                fileNamexxx = nickName + "-PAY_1.xlsx";
                break;
        }

        if (File.Exists("c:\\TEJ_erp\\Upload\\" + fileNamexxx)) File.Delete("c:\\TEJ_erp\\Upload\\" + fileNamexxx);
        xlWorkSheet.SaveAs("c:\\TEJ_erp\\Upload\\" + fileNamexxx);

        Session["FileName"] = fileNamexxx;
        Session["FilePath"] = fileNamexxx;

        Response.Write("<script>");
        Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
        Response.Write("</script>");
        //xlApp.Visible = true;
        dt.Dispose();

        xlWorkBook.Close();
    }

    string acBal(string selecAcode, string selectedMonth)
    {
        ind_curr = fgen.seek_iname(frm_qstr, co_cd, "SELECT br_Curren as curr FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "curr");

        string frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
        string frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
        string prdRange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
        if (prdRange == "0")
            prdRange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_DATERANGE");
        string xprd1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt1 + "','dd/mm/yyyy') -1";
        string myMnth = "", myMnth2 = "";
        if (selecAcode.Length > 7)
        {
            myMnth = "01/" + selecAcode.Left(2);
            if (ind_curr == "INR")
            {
                if (selecAcode.Left(2).toDouble() < 12)
                {
                    myMnth = "01/" + selecAcode.Left(2) + "/" + Convert.ToDateTime(frm_cDt2).ToString("yyyy");
                }
                else myMnth = "01/" + selecAcode.Left(2) + "/" + Convert.ToDateTime(frm_cDt1).ToString("yyyy");
            }
            else myMnth = "01/" + selecAcode.Left(2) + "/" + Convert.ToDateTime(frm_cDt1).ToString("yyyy");

            xprd1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + myMnth + "','dd/mm/yyyy') -1";

            myMnth2 = Convert.ToDateTime(myMnth).AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy");
            prdRange = "between to_date('" + myMnth + "','dd/mm/yyyy') and to_date('" + myMnth2 + "','dd/mm/yyyy')";
        }
        if (selecAcode.Length == 8) selecAcode = selecAcode.Right(6);
        if (selecAcode.Length == 9) selecAcode = selecAcode.Right(7);
        if (selecAcode.Length < 6) selecAcode = hfACode.Value;

        //cmd = seek_iname2("SELECT TO_DATE(0||1||TO_CHAR(ADD_MONTHS(SYSDATE,1),'mmyyyy'),'ddmmyyyy')AS xdt1, (LAST_DAY(ADD_MONTHS(SYSDATE,1))) AS xdt2 FROM DUAL ", "xdt1", "xdt2")
        string SQueryx = "select sum(opb) as bal from (select sum(yr_" + year + ") as opb,0 as inbal,0 as outbal from famstbal where branchcd IN ('" + frm_mbr + "') and acode  in ('" + selecAcode + "') group by acode union all select sum(nvl(DRAMT,0))-sum(nvl(CRAMT,0)) as obal,0 as inbal,0 as outbal from voucher where branchcd IN ('" + frm_mbr + "') and VCHDATE " + xprd1 + " and acode  in ('" + selecAcode + "') union all select 0 as opbal,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then ABS(sum(A.DRAMT)-sum(A.CRAMT)) else 0 end) AS IQTYIN,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then 0 else abs(sum(A.DRAMT)-sum(A.CRAMT)) end) AS IQTYOUT from voucher A where a.branchcd IN ('" + frm_mbr + "') and A.VCHDATE " + prdRange + " AND A.ACODE IN ('" + selecAcode + "') )";
        return fgen.seek_iname(frm_qstr, co_cd, SQueryx, "BAL");
    }
    protected void btnGrid_Click(object sender, EventArgs e)
    {
        txtsearch.Text = "";

        string[] myArr = new string[11];
        if (HiddenField1.Value.Length > 0)
        {
            string json = HiddenField1.Value;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(json);
            var dicddd = new Dictionary<string, object>();
            int brkLoop = 0;
            foreach (var cols in JSONObj)
            {
                if (brkLoop > 10) break;
                if (cols.Key.ToString() == "pq_rowselect") break;
                myArr[brkLoop] = cols.Value.ToString();
                brkLoop++;
            }
        }

        string Value1 = myArr[0];
        string Value2 = myArr[2];
        string Value3 = myArr[3];
        if (Convert.ToInt32(hfLevel.Value.toDouble().ToString()) == -1) hfLevel.Value = "0";
        int drillPost = Convert.ToInt32(hfLevel.Value.toDouble().ToString());
        if (drillPost == 0)
            hfACode.Value = Value1;
        drillPost = drillPost + 1;
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID");

        if (setDrillLevel(drillPost, drillPost - 1, Value1).Length > 2)
        {
            if (Value2 != "-") lblMsgSel.InnerText += " : " + Value2.Replace("&amp;", "&");
            lblMsgSel.InnerText = lblMsgSel.InnerText.Replace(": All", "");
        }
        else
        {
            if (frm_formID == "F77132")
            {
                //dt = new DataTable();
                //dt = (DataTable)ViewState["sg1"];
                //fillPMGrid(dt);

                //fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
                //fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70335");
                //fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", "-");
                //fgenMV.Fn_Set_Mvar(frm_qstr, "U_III", "-");
                //fgen.fin_acct_reps(frm_qstr);
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "N");
            // here to add any condition
            if (((frm_formID == "F70556" || frm_formID == "F70230A" || frm_formID == "F70650") && drillPost == 3) || (frm_formID == "F70189" || frm_formID == "F70189A" || frm_formID == "F70156" && drillPost == 5))
            {
                hideAndRenameCol();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
                if (Value1.Length > 2)
                {
                    string myFormName = "", myFormID = "";
                    switch (Value1.Substring(2, 1))
                    {
                        case "5":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_pinv_entry.aspx";
                            myFormID = "@F70116";
                            break;
                        case "4":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_inv_entry.aspx";
                            myFormID = "@F50101";
                            break;
                        case "2":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_rcpt_vch.aspx";
                            myFormID = "@F70106";
                            break;
                        case "1":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_rcpt_vch.aspx";
                            myFormID = "@F70101";
                            break;
                        default:
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_jour_vch.aspx";
                            myFormID = "@F70111";
                            break;
                    }
                    switch (Value1.Substring(2, 2))
                    {
                        case "5A":
                        case "5B":
                        case "57":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_pinv_entry.aspx";
                            myFormID = "@F70112";
                            break;
                        case "5S":
                        case "5P":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_pinv_entry.aspx";
                            myFormID = "@F70122";
                            break;
                        case "31":
                        case "59":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_pinv_entry.aspx";
                            myFormID = "@F70108";
                            break;
                        case "32":
                        case "58":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_pinv_entry.aspx";
                            myFormID = "@F70110";
                            break;
                        case "5U":
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                            myFormName = "../tej-base/om_jour_vch.aspx";
                            myFormID = "@F70111";
                            break;
                    }
                    if (chkVchView.Checked)
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                        myFormName = "../tej-base/om_jour_vch.aspx";
                        myFormID = "@F70111";
                    }

                    //|| frm_formID == "F70156" || frm_formID == "F70189" || frm_formID == "F70189A"
                    if (frm_formID == "F70230Ax")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "Y");
                        myFormName = "../tej-base/om_mul_vch.aspx";
                        myFormID = "@F70116";
                    }
                    else
                    {
                        if (Convert.ToDateTime(fgen.make_def_Date(Value1.Right(10), DateTime.Now.ToString("dd/MM/yyyy"))) < Convert.ToDateTime("13/01/2021") && (co_cd == "SGRP" || co_cd == "UATS" || co_cd == "UAT2"))
                        {
                            myFormID = "@F70111";
                            myFormName = "../tej-base/om_jour_vch.aspx";
                        }
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + myFormName + "?STR=" + frm_qstr + myFormID + "','98%','98%','');", true);
                }
            }
        }
    }



    string myQuery(int level, string frm_qstr, string selecValfstr)
    {
        string myLedgType = "TB";
        string cond = "";
        string xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
        if (xprdrange == "0")
            xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_DATERANGE");
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBRCOND") == "ABR") branch_Cd = "BRANCHCD!='DD'";
        else branch_Cd = "BRANCHCD='" + frm_mbr + "'";
        switch (level)
        {
            case 1:
                myq = "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,to_char(SUM(DRAMT),'999999999.99') AS DEBITS,to_char(SUM(CRAMT),'999999999.99') AS CREDITS,sum(mthsno) as srno,0 as Cumulative_Total FROM (SELECT TRIM(MTHNUM) AS FSTR,'-' AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ACODE)='FSTR' ) GROUP BY FSTR,MTHNAME order by srno";
                break;
            case 2:
                cond = "(GSTR='FSTR' or SUBSTR(GSTR,1,2)='FSTR')";
                myq = "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,to_char(A.DRAMT,'999999999.99') AS DEBIT,to_char(a.CRAMT,'999999999.99') AS CREDITS,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,A.TYPE,A.VCHNUM,A.NARATION as Narration,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.CCENT,A.BANK_DATE,A.ST_ENTFORM,A.BRANCHCD as PL_CODE,A.ACODE,A.ENT_BY,A.Ent_Date,A.EDT_BY,(Case when length(Trim(a.edt_by))>1 then to_char(A.EDt_Date,'dd/mm/yyyy :hh24:mi:ss') else '-' end ) as Edt_Dated,to_char(a.vchdate,'yyyymmdd') as vdd FROM VOUCHER A,FAMST B WHERE a.branchcd!='88' and a.branchcd!='DD' and TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY vdd,a.type,A.VCHNUM) WHERE " + cond + "";
                break;
        }
        return myq;
    }
    protected void btnRep12_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCH_CD", "branchcd='" + frm_mbr + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", hfACode.Value);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70298");
        fgen.fin_acct_reps(frm_qstr);
        reFill();
    }
    protected void btnRep22_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_BRANCH_CD", "branchcd='" + frm_mbr + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", "'" + hfACode.Value + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", "");
        string grp = fgen.seek_iname(frm_qstr, co_cd, "SELECT GRP FROM FAMST WHERE TRIM(ACODE)='" + hfACode.Value + "' ", "GRP");
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", (grp == "16" ? "F70608" : "F70610"));
        fgen.fin_acct_reps(frm_qstr);
        reFill();
    }
    protected void btnRep32_Click(object sender, EventArgs e)
    {
        reFill();
    }
    protected void btnRep42_Click(object sender, EventArgs e)
    {
        string DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        string SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F25126", "branchcd='" + frm_mbr + "'", "a.type like '0%' and a.acode like '" + hfACode.Value + "%' ", "" + DateRange);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevelJS("MRR Entry Checklist for the Period " + DateRange, frm_qstr);
        reFill();
    }

    void reFill()
    {
        if (ViewState["sg1"] != null)
        {
            dt = (DataTable)ViewState["sg1"];

            if (dt.Rows.Count > 0)
            {
                fillPMGrid(dt);
            }
        }
    }
    protected void btnreFill_Click(object sender, ImageClickEventArgs e)
    {
        int drillBack = fgen.make_int(hfLevel.Value);
        string selVal = "";
        for (int z = 0; z < lblMsgSel.InnerText.Split(':').Length - 1; z++)
        {
            selVal += lblMsgSel.InnerText.Split(':')[z].ToString();
        }
        lblMsgSel.InnerText = selVal;
        drillBack = drillBack - 0;
        if (drillBack == 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_LVAL", "");
        }
        if (drillBack < 0)
        {
            //if (drillBack == -1)
            //{
            //    hfLevel.Value = "-1";
            //    lblMsg.InnerText = "Press Esc/Back Button one more time to Exit";
            //}
            //if (drillBack < -1)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyClose();", true);
            }
        }
        else setDrillLevel(drillBack, drillBack + 1, "");
    }
}