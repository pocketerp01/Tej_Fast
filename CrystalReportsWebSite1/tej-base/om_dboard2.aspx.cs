using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dboard2 : System.Web.UI.Page
{
    string DateRange, PrdRange, sQuery, chartScript;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2, frm_MDT1, frm_MDT2, DayRange;
    string val_legnd1, val_legnd2, val_legnd3, val_legnd4, popUpType = "";
    string squery1, squery2, squery3, squery4, squery5, squery6;
    string stitle1, stitle2, stitle3, stitle4, stitle5, stitle6;
    string leftHeading1, leftHeading2, leftHeading3, leftHeading4;
    string bottomHeading1, bottomHeading2, bottomHeading3, bottomHeading4;
    string gu1, gu2, gu3, gu4;
    string gl1, gl2, gl3, gl4;
    string chart1, chart2, chart3, chart4;
    string wdays;
    string Client_Code;
    string Prg_Id;
    DataTable iconDt = new DataTable();
    fgenDB fgen = new fgenDB();
    int kz = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
                else Response.Redirect("~/login.aspx");
            }
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (!Page.IsPostBack)
            {
                if (Prg_Id == "******")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "PARTY");
                }
                else fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");

                askPopUp();
                iconDt = fgen.getdata(frm_qstr, frm_cocd, "select id from FIN_MSYS where id between 'F05000' and 'F10000' order by id ");
                ViewState["icodeDt"] = iconDt;

                btnPause.Visible = true;
            }
        }
    }
    void askPopUp()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        popUpType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_POPUPTYPE");
        switch (Prg_Id)
        {
            case "S15115G":
                if (popUpType == "PARTY")
                {
                    sQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                    fgen.Fn_open_sseek("", frm_qstr);
                }
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;
            case "P17005A":
                if (popUpType == "PARTY")
                {
                    sQuery = "SELECT Vchnum,Name,Vchnum AS CODE,proj_refno as Ref FROM proj_dtl WHERE branchcd!='DD' ORDER BY Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_POPUPTYPE", "DT");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", sQuery);
                    fgen.Fn_open_sseek("", frm_qstr);
                }
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;

            default:
                if (popUpType == "DT")
                {
                    fgen.Fn_open_prddmp1("", frm_qstr);
                }
                break;
        }
    }
    void printHeads()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        topBoxes.Visible = false;
        switch (Prg_Id)
        {

            case "S06005F":
                lblBox1Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' ", "C1");
                lblBox1Header.Text = "Total Query Received";

                lblBox2Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and last_action!='-'", "C1");
                lblBox2Header.Text = "Total Action Taken";

                lblBox3Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and last_action like 'Cleared%'", "C1");
                lblBox3Header.Text = "Total Query Cleared";

                lblBox4Count.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(*) AS C1 FROM cquery_REg where branchcd!='DD' and trim(nvl(clo_by,'-'))!='-' ", "C1");
                lblBox4Header.Text = "Total Query Closed";
                break;
        }


    }
    void printGraph()
    {
        val_legnd1 = "";
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        frm_MDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        frm_MDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        DayRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DAYRANGE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        switch (Prg_Id)
        {
            case "F05000":
                chart1 = "column";
                leftHeading1 = "";
                bottomHeading1 = "Kgs";
                gu1 = "";
                gl1 = "";
                stitle1 = "Day Wise Corrugation Production";
                val_legnd1 = "Kgs";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,round(sum(a.qty/1000)) AS QTYOUT FROM (SELECT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.ICODE,(A.QTY*B.IWEIGHT) AS QTY  FROM costestimate A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and a.type='40' and a.vchdate " + DayRange + ") A  GROUP BY TO_cHAR(a.VCHDATE,'DD-Mon') order by TO_cHAR(a.VCHDATE,'DD-Mon')";

                chart2 = "bar";
                leftHeading2 = "";
                bottomHeading2 = "Kgs";
                gu2 = "";
                gl2 = "";
                stitle2 = "Month Wise Corrugation Production";
                val_legnd2 = "Kgs";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,round(sum(a.qty/1000)) AS QTYOUT FROM (SELECT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.ICODE,(A.QTY*B.IWEIGHT) AS QTY  FROM costestimate A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "'  and a.type='40' and a.vchdate " + PrdRange + ") A GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') order by TO_cHAR(a.VCHDATE,'YYYYMM')";

                chart3 = "spline";
                leftHeading3 = "";
                bottomHeading3 = "Mtr";
                gu3 = "";
                gl3 = "";
                stitle3 = "Day Wise Corrugation Production";
                val_legnd3 = "Mtr";
                squery3 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,round(sum(a.qty/1000)) AS QTYOUT FROM costestimate A WHERE a.BRANCHCD='" + frm_mbr + "'  and a.type='40' and a.vchdate " + DayRange + "  GROUP BY TO_cHAR(a.VCHDATE,'DD-Mon') order by TO_cHAR(a.VCHDATE,'DD-Mon')";

                chart4 = "pie";
                leftHeading4 = "";
                bottomHeading4 = "Mtr";
                gu4 = "";
                gl4 = "";
                stitle4 = "Month Wise Corrugation Production (Mtr)";
                squery4 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,round(sum(a.qty/1000)) AS Produce_Qty FROM costestimate A WHERE a.BRANCHCD='" + frm_mbr + "'  and a.type='40' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') order by TO_cHAR(a.VCHDATE,'YYYYMM')";
                val_legnd4 = "Mtr";
                break;
            case "F05100":
                chart1 = "line";
                leftHeading1 = "";
                bottomHeading1 = "Jobs in No.";
                stitle1 = "Day Wise Corrugation Machine Jobs Done";
                gu1 = "";
                gl1 = "";

                squery1 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,COUNT(VCHNUM) AS QTYOUT FROM (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE, ENQNO AS JOB_NO FROM COSTESTIMATE  WHERE TYPE='40' AND BRANCHCD='" + frm_mbr + "' and vchdate " + DayRange + ") A  GROUP BY TO_cHAR(VCHDATE,'DD-Mon'),TO_cHAR(VCHDATE,'DD') order by TO_cHAR(VCHDATE,'DD')";

                chart2 = "bar";
                leftHeading2 = "";
                bottomHeading2 = "Jobs";
                stitle2 = "Month Wise Corrugation Machine Jobs Done";
                gu2 = "";
                gl2 = "";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,COUNT(A.VCHNUM) AS QTY FROM  (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE, ENQNO AS JOB_NO FROM COSTESTIMATE  WHERE TYPE='40' AND BRANCHCD='" + frm_mbr + "' and TYPE='40' AND VCHDATE " + PrdRange + ") A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') order by TO_cHAR(a.VCHDATE,'YYYYMM')";

                chart3 = "column";
                leftHeading3 = "";
                bottomHeading3 = "Prod in Kgs";
                stitle3 = "Shift Wise Monthly Comparison Production";
                gu3 = "";
                gl3 = "";
                squery3 = "SELECT SHIFT,TO_cHAR(a.VCHDATE,'Month') AS YR,round(sum(a.prod_qty/1000)) as qty  FROM (SELECT a.vchdate ,TRIM(A.COL23) AS SHIFT,TRIM(A.COL25) AS MACHINE,SUM(A.QTY)*B.IWEIGHT AS PROD_QTY,0 AS Rej_Qty FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and A.TYPE='40' and a.vchdate " + PrdRange + " GROUP BY  TRIM(A.COL23) ,TRIM(A.COL25) ,B.IWEIGHT,a.vchdate)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),SHIFT order by TO_cHAR(a.VCHDATE,'YYYYMM')";
                squery3 = "SELECT YR,SUM(SHIFT_A) AS SHIFT_A,SUM(SHIFT_B) AS SHIFT_B FROM (SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,decode(upper(shift),'SHIFT A', round(sum(a.prod_qty/1000)),0) as SHIFT_a,decode(upper(shift),'SHIFT B', round(sum(a.prod_qty/1000)),0) as SHIFT_B  FROM (SELECT a.vchdate ,TRIM(A.COL23) AS SHIFT,TRIM(A.COL25) AS MACHINE,SUM(A.QTY)*B.IWEIGHT AS PROD_QTY,0 AS Rej_Qty FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and A.TYPE='40' and a.vchdate " + PrdRange + " GROUP BY  TRIM(A.COL23) ,TRIM(A.COL25) ,B.IWEIGHT,a.vchdate)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),SHIFT ) GROUP BY YR ORDER BY YR ";

                chart4 = "bar";
                leftHeading4 = "";
                bottomHeading4 = "Rej in Kgs";
                stitle4 = "Shift Wise Monthly Comparison Rejection";
                gu4 = "";
                gl4 = "";
                squery4 = "SELECT YR,SUM(SHIFT_A) AS SHIFT_A,SUM(SHIFT_B) AS SHIFT_B FROM (SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,DECODE(Shift_Name,'SHIFT A', round(sum(a.rej_qty/1000)),0) as SHIFT_A,DECODE(Shift_Name,'SHIFT B', round(sum(a.rej_qty/1000)),0) as SHIFT_B FROM (Select vchdate, OBSV15 as Shift_Name,Title as Machine,0 AS PROD_QTY, sum(qty8) as Rej_Qty from inspvch  where branchcd='" + frm_mbr + "' and type='45' and vchdate " + PrdRange + " group by vchdate, Obsv15,Title)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),Shift_Name ) GROUP BY YR ORDER BY YR ";
                break;
            case "F05101":
                chart1 = "area";
                leftHeading1 = "";
                bottomHeading1 = "In Min";
                stitle1 = "Machine Downtime Report Day Wise";
                gu1 = "";
                gl1 = "";

                squery1 = "SELECT vch,SUM(MINS) AS MINS FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'DD-Mon') AS VCH,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='55' and BRANCHCD='" + frm_mbr + "' AND vchdate " + DayRange + " GROUP BY TO_CHAR(VCHDATE,'DD-Mon'),BRANCHCD,COL2,TITLE,COL1) A GROUP BY vch having SUM(MINS)>0 order by vch ";

                leftHeading2 = "";
                bottomHeading2 = "In Min";
                chart2 = "bar";
                stitle2 = "Machine Downtime Month Wise";
                gu2 = "";
                gl2 = "";

                squery2 = "SELECT vch,SUM(MINS) AS MINS FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'Mon-YY') AS VCH,to_char(Vchdate,'yyyymm') as vchd,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='55' and BRANCHCD='" + frm_mbr + "' AND vchdate " + PrdRange + " GROUP BY TO_CHAR(VCHDATE,'Mon-YY'),BRANCHCD,COL2,TITLE,COL1,to_char(Vchdate,'yyyymm') ) A GROUP BY vch,vchd having SUM(MINS)>0 order by vchd ";

                leftHeading3 = "";
                bottomHeading3 = "In KG";
                chart3 = "column";
                stitle3 = "Rejection Day Wise";
                gu3 = "";
                gl3 = "";

                squery3 = "SELECT VCH,SUM(MINS) AS QTY FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'DD-Mon') AS VCH,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='45' and BRANCHCD='" + frm_mbr + "' AND VCHDATE " + DayRange + " GROUP BY TO_CHAR(VCHDATE,'DD-Mon'),BRANCHCD,COL2,TITLE,COL1) A GROUP BY VCH order by VCH ";

                leftHeading4 = "";
                bottomHeading4 = "In Min";
                chart4 = "pie";
                stitle4 = "Rejection Month Wise";
                gu4 = "";
                gl4 = "";

                squery4 = "SELECT VCH,SUM(MINS) AS QTY FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'Mon-YY') AS VCH,to_char(Vchdate,'yyyymm') as vchd,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='45' and BRANCHCD='" + frm_mbr + "' AND VCHDATE " + PrdRange + " GROUP BY TO_CHAR(VCHDATE,'Mon-YY'),BRANCHCD,COL2,TITLE,COL1,to_char(Vchdate,'yyyymm')) A GROUP BY VCH,vchd order by vchd ";
                break;
            case "F05102":
                chart1 = "scatter";
                leftHeading1 = "";
                bottomHeading1 = "Days";
                gu1 = "";
                gl1 = "";
                stitle1 = "Day Wise Corrugation Machine Trim Wastage";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,round(SUM(IS_NUMBER(TIME1))) AS WASTAGE FROM  (SELECT DISTINCT vchnum,vchdate,scrp1,scrp2,time1,time2,COL25 FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' and TYPE='40' AND VCHDATE " + DayRange + ") A  GROUP BY TO_cHAR(VCHDATE,'DD-Mon') having round(SUM(IS_NUMBER(TIME1)))>0 order by TO_cHAR(VCHDATE,'DD-Mon')";

                chart2 = "area";
                leftHeading2 = "";
                bottomHeading2 = "Months";
                gu2 = "";
                gl2 = "";
                stitle2 = "Month Wise Corrugation Machine Trim Wastage";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,round(SUM(IS_NUMBER(TIME1)/1000)) AS WASTAGE FROM  (SELECT DISTINCT vchnum,vchdate,scrp1,scrp2,time1,time2,COL25 FROM COSTESTIMATE WHERE  BRANCHCD='" + frm_mbr + "' and TYPE='40' AND VCHDATE " + PrdRange + ") A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') having round(SUM(IS_NUMBER(TIME1)/1000))>0 order by TO_cHAR(a.VCHDATE,'YYYYMM')";

                chart3 = "area";
                leftHeading3 = "";
                bottomHeading3 = "Days";
                gu3 = "";
                gl3 = "";
                stitle3 = "Day Wise Corrugation Machine Total Wastage";
                squery3 = "SELECT VCH,ROUND(SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2))) AS WASTAGE FROM  (SELECT DISTINCT TO_char(vchdate,'dd-Mon') as vch,scrp1,scrp2,time1,time2,COL25,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='40' AND VCHDATE " + DayRange + " ) GROUP BY VCH having ROUND(SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2)))>0 order by VCH";

                chart4 = "bar";
                leftHeading4 = "";
                bottomHeading4 = "Months";
                gu4 = "";
                gl4 = "";
                stitle4 = "Month Wise Corrugation Machine Total Wastage";
                squery4 = "SELECT VCH,SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2)) AS WASTAGE FROM  (SELECT DISTINCT TO_char(vchdate,'Mon-YYYY') as vch,scrp1,scrp2,time1,time2,COL25,TO_CHAR(VCHDATE,'YYYYMM') AS VDD FROM COSTESTIMATE WHERE BRANCHCD='01' AND TYPE='40' AND VCHDATE " + PrdRange + ") GROUP BY VCH,VDD having SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2))>0 order by VDD";
                break;
            case "F05103":
                chart1 = "spline";
                leftHeading1 = "";
                bottomHeading1 = "Box/Min";
                stitle1 = "Daily Average Cycle Time of 3 ply jobs";
                gu1 = "";
                gl1 = "";
                squery1 = "SELECT TO_cHAR(VCHDATE,'DD-Mon') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs  FROM  (SELECT  DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + DayRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='3 PLY' GROUP BY TO_cHAR(VCHDATE,'DD-Mon') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'DD-Mon')";

                chart2 = "column";
                leftHeading2 = "";
                bottomHeading2 = "Box/Min";
                stitle2 = "Monthly Average Cycle Time of 3 ply jobs";
                gu2 = "";
                gl2 = "";
                squery2 = "SELECT TO_cHAR(VCHDATE,'Month') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs  FROM  (SELECT  DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + PrdRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='3 PLY' GROUP BY TO_cHAR(VCHDATE,'Month'),TO_cHAR(VCHDATE,'yyyymm') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'yyyymm')";
                val_legnd2 = "Box";

                chart3 = "spline";
                leftHeading3 = "";
                bottomHeading3 = "Box/Min";
                stitle3 = "Daily Average Cycle Time of 5 ply jobs";
                gu3 = "";
                gl3 = "";
                squery3 = "SELECT TO_cHAR(VCHDATE,'DD-Mon') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs  FROM  (SELECT  DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + DayRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='5 PLY' GROUP BY TO_cHAR(VCHDATE,'DD-Mon') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'DD-Mon')";

                chart4 = "column";
                leftHeading4 = "";
                bottomHeading4 = "Box/Min";
                stitle4 = "Monthly Average Cycle Time of 5 ply jobs";
                gu4 = "";
                gl4 = "";
                squery4 = "SELECT TO_cHAR(VCHDATE,'Month') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs FROM (SELECT DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + PrdRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='5 PLY' GROUP BY TO_cHAR(VCHDATE,'Month'),TO_cHAR(VCHDATE,'yyyymm') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'yyyymm')";
                break;
            case "F05121":
                chart1 = "bar";
                leftHeading1 = "";
                bottomHeading1 = "Days";
                stitle1 = "Shift Wise Monthly Comparison Production";
                gu1 = "";
                gl1 = "";
                stitle1 = "Shift Wise Monthly Comparison Production";
                squery1 = "select yr,round(sum(shift_a/1000)) as shift_a,round(sum(shift_b/1000)) as shift_b from (SELECT TO_cHAR(a.VCHDATE,'YYYYMM') as vdd,TO_cHAR(a.VCHDATE,'Month') AS YR,decode(SHIFT,'SHIFT A',round(sum(a.prod_qty),2),0) as SHIFT_a,decode(SHIFT,'SHIFT B',round(sum(a.prod_qty),2),0) as SHIFT_b FROM (SELECT a.vchdate ,TRIM(A.COL23) AS SHIFT,TRIM(A.COL25) AS MACHINE,SUM(A.QTY)*B.IWEIGHT AS PROD_QTY,0 AS Rej_Qty FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and A.TYPE='40' and a.vchdate " + PrdRange + " GROUP BY  TRIM(A.COL23) ,TRIM(A.COL25) ,B.IWEIGHT,a.vchdate)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),shift) group by vdd,yr order by vdd";

                chart2 = "bar";
                leftHeading2 = "";
                bottomHeading2 = "Days";
                stitle2 = "Shift Wise Monthly Comparison Rejection";
                gu2 = "";
                gl2 = "";
                squery2 = "SELECT YR,round(SUM(SHIFT_A/1000)) AS SHIFT_A,round(SUM(SHIFT_B/1000)) AS SHIFT_B FROM (SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,DECODE(Shift_Name,'SHIFT A', round(sum(a.rej_qty/1000)),0) as SHIFT_A,DECODE(Shift_Name,'SHIFT B', round(sum(a.rej_qty/1000)),0) as SHIFT_B FROM (Select vchdate, OBSV15 as Shift_Name,Title as Machine,0 AS PROD_QTY, sum(qty8) as Rej_Qty from inspvch  where branchcd='" + frm_mbr + "' and type='45' and vchdate " + PrdRange + " group by vchdate, Obsv15,Title)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),Shift_Name ) GROUP BY YR ORDER BY YR ";

                chart3 = "bar";
                leftHeading3 = "";
                bottomHeading3 = "Days";
                stitle3 = "Shift Wise Monthly Comparison Job Completion";
                gu3 = "";
                gl3 = "";
                squery3 = "SELECT SHIFT,COUNT(VCHNUM) as qty  FROM (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE, ENQNO AS JOB_NO,TRIM(COL23) AS SHIFT FROM COSTESTIMATE  WHERE TYPE='40' AND BRANCHCD='" + frm_mbr + "' and vchdate " + DayRange + ")  A  GROUP BY SHIFT order by SHIFT";

                chart4 = "bar";
                leftHeading4 = "";
                bottomHeading4 = "Days";
                stitle4 = "Shift Wise Monthly Comparison Job Completion";
                gu4 = "";
                gl4 = "";
                squery4 = "select obsv15 as shift,round(sum(is_number(col3)),2) as downtime_mins from (select to_char(vchdate,'Mon-yyyy') as vch,obsv15,col3 from inspvch where branchcd='" + frm_mbr + "' and type='55' and vchdate " + DayRange + ") group by obsv15 order by shift";
                break;
            case "F05126":
                chart1 = "column";
                leftHeading1 = "Values";
                bottomHeading1 = "Days";
                stitle1 = "Prodn Planning Vs Prodn Day Wise";
                gu1 = "";
                gl1 = "";
                squery1 = "select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as job_dt,icode,iqtyout,0 as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='90' and VCHDATE " + DayRange + " union all select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as vchdate,icode,0 as iqtyout,iqtyin as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE " + DayRange + "";
                squery1 = "select to_char(a.vchdate,'DD-Mon') as vchdate,round(sum(a.iqtyout/1000)) as Plan_qty,round(sum(a.prodn/1000)) as Prodn_Qty from (" + squery1 + ") a group by to_char(a.vchdate,'DD-Mon') having (round(sum(a.iqtyout/1000)) + round(sum(a.prodn/1000)))>0 order by to_char(a.vchdate,'DD-Mon')";

                leftHeading3 = "Values";
                bottomHeading3 = "Days";
                chart3 = "column";
                stitle3 = "Prodn Vs Rejection Day Wise";
                gu3 = "";
                gl3 = "";
                squery3 = "select vchdate,vchnum,icode,iqtyin as prodn,0 as rejqty  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE " + DayRange + " union all select vchdate,vchnum,icode,0 as prodn,qty8 as rejqty  from inspvch where branchcd='" + frm_mbr + "' and type='45' and VCHDATE " + DayRange + "";
                squery3 = "select to_char(a.vchdate,'dd-Mon') as vchdate,round(sum(a.Prodn/1000)) as Prodn_qty,round(sum(a.Rejqty/1000)) as Rejn_Qty from (" + squery3 + ") a group by to_char(a.vchdate,'dd-Mon') order by to_char(a.vchdate,'dd-Mon')";
                break;
            case "F05127":
                leftHeading2 = "Values";
                bottomHeading2 = "Months";
                chart2 = "bar";
                stitle2 = "Prodn Planning Vs Prodn Month Wise";
                gu2 = "Prodn Planning Vs Prodn";
                gl2 = "During " + frm_CDT1 + " to " + frm_CDT2;
                squery2 = "select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as job_dt,icode,iqtyout,0 as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='90' and VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') union all select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as vchdate,icode,0 as iqtyout,iqtyin as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') ";
                squery2 = "select to_char(a.vchdate,'Mon-YY') as mth ,round(sum(a.iqtyout/1000)) as Plan_qty,round(sum(a.prodn/1000)) as Prodn_Qty from (" + squery2 + ") a group by to_char(a.vchdate,'Mon-YY') order by to_char(a.vchdate,'Mon-YY')";

                leftHeading4 = "Values";
                bottomHeading4 = "Months";
                chart4 = "bar";
                stitle4 = "Prodn Vs Rejection Month Wise";
                gu4 = "Prodn Vs Rejection";
                gl4 = "During " + frm_CDT1 + " to " + frm_CDT2;
                squery4 = "select vchdate,vchnum,icode,iqtyin as prodn,0 as rejqty  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE " + PrdRange + " union all select vchdate,vchnum,icode,0 as prodn,qty8 as rejqty  from inspvch where branchcd='" + frm_mbr + "' and type='45' and VCHDATE " + PrdRange + "";
                squery4 = "select to_char(a.vchdate,'Mon-YY') as vchdate,round(sum(a.Prodn/1000)) as Prodn_qty,round(sum(a.Rejqty/1000)) as Rejn_Qty from (" + squery4 + ") a group by to_char(a.vchdate,'Mon-YY') order by to_char(a.vchdate,'Mon-YY')";
                break;
            case "F05128":
                chart1 = "column";
                leftHeading1 = "Values";
                bottomHeading1 = "Days";
                stitle1 = "Capacity Vs Prodn Day Wise";
                gu1 = "";
                gl1 = "";

                squery1 = "select a.vchdate,a.vchnum,a.icode,a.iqtyin*b.iweight as prodn,0 as rejqty,a.iqtyin as Prodq,0 as rejq,b.iweight  from prod_Sheet a,item b  where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='88' and a.VCHDATE " + DayRange + " union all select a.vchdate,a.vchnum,a.icode,0 as prodn,a.qty8*b.iweight as rejqty,0 as prodq,a.qty8 as Rej_Qty,b.iweight  from inspvch a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='45' and a.VCHDATE " + DayRange + "";
                squery1 = "select to_char(a.vchdate,'dd-Mon') as vchdate ,round(sum(a.Prodn)/1000,0) as Prodn_Wt,round(sum(a.Rejqty)/1000,0) as Rejn_Wt from (" + squery1 + ") a group by to_char(a.vchdate,'dd-Mon') order by to_char(a.vchdate,'dd-Mon')";

                leftHeading3 = "Values";
                bottomHeading3 = "Days";
                chart3 = "column";
                stitle3 = "Prodn Vs Completion Day Wise";
                gu3 = "";
                gl3 = "";

                squery3 = "select to_char(a.dated,'dd-Mon') as dated ,sum(a.prodn) as Prodn_qty,(Case when sum(a.Job_Qty)>0 and sum(a.prodn)>0 then round((sum(a.prodn)/sum(a.job_Qty))*100,2) else 0 end) as Completion from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE a.vchdate " + DayRange + " and A.SRNO=0 AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,is_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.vchdate " + DayRange + " and a.branchcd='" + frm_mbr + "' and a.type='60')a, item b where trim(A.erp_Code)=trim(B.icode) group by to_char(a.dated,'dd-Mon') having sum(a.Job_Qty)-sum(a.prodn)>0 order by to_char(a.dated,'dd-Mon')";
                break;
            case "F05129":
                leftHeading2 = "Values";
                bottomHeading2 = "Months";
                chart2 = "spline";
                stitle2 = "Capacity Vs Prodn Month Wise";
                gu2 = "Capacity Vs Prodn";
                gl2 = "During " + frm_CDT1 + " to " + frm_CDT2;

                squery2 = "select a.vchdate,a.vchnum,a.icode,a.iqtyin*b.iweight as prodn,0 as rejqty,a.iqtyin as Prodq,0 as rejq,b.iweight  from prod_Sheet a,item b  where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='88' and a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') union all select a.vchdate,a.vchnum,a.icode,0 as prodn,a.qty8*b.iweight as rejqty,0 as prodq,a.qty8 as Rej_Qty,b.iweight  from inspvch a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='45' and a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy')";
                squery2 = "select to_char(a.vchdate,'Mon-YY') as vchdate ,round(sum(a.Prodn)/1000,0) as Prodn_Wt,round(sum(a.Rejqty)/1000,0) as Rejn_Wt from (" + squery2 + ") a group by to_char(a.vchdate,'Mon-YY') order by to_char(a.vchdate,'Mon-YY')";

                leftHeading4 = "Values";
                bottomHeading4 = "Months";
                chart4 = "bar";
                stitle4 = "Prodn Vs Completion Month Wise";
                gu4 = "Prodn Vs Completion";
                gl4 = "During " + frm_CDT1 + " to " + frm_CDT2;

                squery4 = "select to_char(a.dated,'Mon-yy') as dated ,sum(a.prodn) as Prodn_qty,(Case when sum(a.Job_Qty)>0 and sum(a.prodn)>0 then round((sum(a.prodn)/sum(a.job_Qty))*100,2) else 0 end) as Completion from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') and A.SRNO=0 AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,is_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='60')a, item b where trim(A.erp_Code)=trim(B.icode) group by to_char(a.dated,'Mon-yy') having sum(a.Job_Qty)-sum(a.prodn)>0 order by to_char(a.dated,'Mon-yy')";
                break;

            case "F05130":
                chart1 = "line";
                leftHeading1 = "";
                bottomHeading1 = "Values in K";
                stitle1 = "Day Wise Sales";
                gu1 = "";
                gl1 = "";

                squery1 = "select vch,round(sum(bill_tot/1000)) as output from (select  to_char(vchdate,'dd-Mon') as vch,bill_tot,to_char(vchdate,'yyyymmdd') as vdd from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + DayRange + ") group by vch order by vch";

                chart2 = "spline";
                leftHeading2 = "";
                bottomHeading2 = "Values in K";
                stitle2 = "Month Wise Sales";
                gu2 = "";
                gl2 = "";
                squery2 = "select vch,round(sum(bill_tot/1000)) as output from (select  to_char(vchdate,'Mon-yy') as vch,bill_tot,to_char(vchdate,'yyyymm') as vdd from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + PrdRange + ") group by vch,vdd order by vdd ";
                break;
        }

        chartDiv1.Attributes.Add("class", "col-lg-6");
        chartDiv2.Attributes.Add("class", "col-lg-6");
        chartDiv3.Attributes.Add("class", "col-lg-6");
        chartDiv4.Attributes.Add("class", "col-lg-6");
        chartDiv1.Visible = true; chartDiv2.Visible = true;
        chartDiv3.Visible = true; chartDiv4.Visible = true;

        lblChart1Header.Text = stitle1;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle1, chart1, gu1, gl1, squery1, val_legnd1, "chart1", bottomHeading1, leftHeading1);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart1", chartScript, false);
        else chartDiv1.Visible = false;

        lblChart2Header.Text = stitle2;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle2, chart2, gu2, gl2, squery2, val_legnd2, "chart2", bottomHeading2, leftHeading2);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart2", chartScript, false);
        else chartDiv2.Visible = false;

        lblChart3Header.Text = stitle3;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle3, chart3, gu3, gl3, squery3, val_legnd3, "chart3", bottomHeading3, leftHeading3);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart3", chartScript, false);
        else chartDiv3.Visible = false;

        lblChart4Header.Text = stitle4;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle4, chart4, gu4, gl4, squery4, val_legnd4, "chart4", bottomHeading4, leftHeading4);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart4", chartScript, false);
        else chartDiv4.Visible = false;

        if (chartDiv1.Visible == false) chartDiv2.Attributes.Add("class", "col-lg-12");
        if (chartDiv2.Visible == false) chartDiv1.Attributes.Add("class", "col-lg-12");
        if (chartDiv3.Visible == false) chartDiv4.Attributes.Add("class", "col-lg-12");
        if (chartDiv4.Visible == false) chartDiv3.Attributes.Add("class", "col-lg-12");
        if (chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
        if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }

        kz++;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ", kz.ToString());
        if (chartDiv1.Visible == false && chartDiv2.Visible == false && chartDiv3.Visible == false && chartDiv4.Visible == false) timer1_Tick("", EventArgs.Empty);
    }
    void printGraph(string Prg_Id)
    {
        val_legnd1 = "";
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        frm_MDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        frm_MDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        DayRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DAYRANGE");

        switch (Prg_Id)
        {
            case "P17006A":
                chart1 = "column";
                leftHeading1 = "";
                bottomHeading1 = "Kgs";
                gu1 = "";
                gl1 = "";
                stitle1 = "Day Wise Corrugation Production";
                val_legnd1 = "Kgs";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,round(sum(a.qty/1000)) AS QTYOUT FROM (SELECT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.ICODE,(A.QTY*B.IWEIGHT) AS QTY  FROM costestimate A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and a.type='40' and a.vchdate " + DayRange + ") A  GROUP BY TO_cHAR(a.VCHDATE,'DD-Mon') order by TO_cHAR(a.VCHDATE,'DD-Mon')";

                chart2 = "bar";
                leftHeading2 = "";
                bottomHeading2 = "Kgs";
                gu2 = "";
                gl2 = "";
                stitle2 = "Month Wise Corrugation Production";
                val_legnd2 = "Kgs";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,round(sum(a.qty/1000)) AS QTYOUT FROM (SELECT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.ICODE,(A.QTY*B.IWEIGHT) AS QTY  FROM costestimate A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "'  and a.type='40' and a.vchdate " + PrdRange + ") A GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') order by TO_cHAR(a.VCHDATE,'YYYYMM')";

                chart3 = "spline";
                leftHeading3 = "";
                bottomHeading3 = "Mtr";
                gu3 = "";
                gl3 = "";
                stitle3 = "Day Wise Corrugation Production";
                val_legnd3 = "Mtr";
                squery3 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,round(sum(a.qty/1000)) AS QTYOUT FROM costestimate A WHERE a.BRANCHCD='" + frm_mbr + "'  and a.type='40' and a.vchdate " + DayRange + "  GROUP BY TO_cHAR(a.VCHDATE,'DD-Mon') order by TO_cHAR(a.VCHDATE,'DD-Mon')";

                chart4 = "pie";
                leftHeading4 = "";
                bottomHeading4 = "Mtr";
                gu4 = "";
                gl4 = "";
                stitle4 = "Month Wise Corrugation Production (Mtr)";
                squery4 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,round(sum(a.qty/1000)) AS Produce_Qty FROM costestimate A WHERE a.BRANCHCD='" + frm_mbr + "'  and a.type='40' and a.vchdate " + PrdRange + "  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') order by TO_cHAR(a.VCHDATE,'YYYYMM')";
                val_legnd4 = "Mtr";
                break;
            case "P17006C":
                chart1 = "line";
                leftHeading1 = "";
                bottomHeading1 = "Jobs in No.";
                stitle1 = "Day Wise Corrugation Machine Jobs Done";
                gu1 = "";
                gl1 = "";

                squery1 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,COUNT(VCHNUM) AS QTYOUT FROM (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE, ENQNO AS JOB_NO FROM COSTESTIMATE  WHERE TYPE='40' AND BRANCHCD='" + frm_mbr + "' and vchdate " + DayRange + ") A  GROUP BY TO_cHAR(VCHDATE,'DD-Mon'),TO_cHAR(VCHDATE,'DD') order by TO_cHAR(VCHDATE,'DD')";

                chart2 = "bar";
                leftHeading2 = "";
                bottomHeading2 = "Jobs";
                stitle2 = "Month Wise Corrugation Machine Jobs Done";
                gu2 = "";
                gl2 = "";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,COUNT(A.VCHNUM) AS QTY FROM  (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE, ENQNO AS JOB_NO FROM COSTESTIMATE  WHERE TYPE='40' AND BRANCHCD='" + frm_mbr + "' and TYPE='40' AND VCHDATE " + PrdRange + ") A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') order by TO_cHAR(a.VCHDATE,'YYYYMM')";

                chart3 = "column";
                leftHeading3 = "";
                bottomHeading3 = "Prod in Kgs";
                stitle3 = "Shift Wise Monthly Comparison Production";
                gu3 = "";
                gl3 = "";
                squery3 = "SELECT SHIFT,TO_cHAR(a.VCHDATE,'Month') AS YR,round(sum(a.prod_qty/1000)) as qty  FROM (SELECT a.vchdate ,TRIM(A.COL23) AS SHIFT,TRIM(A.COL25) AS MACHINE,SUM(A.QTY)*B.IWEIGHT AS PROD_QTY,0 AS Rej_Qty FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and A.TYPE='40' and a.vchdate " + PrdRange + " GROUP BY  TRIM(A.COL23) ,TRIM(A.COL25) ,B.IWEIGHT,a.vchdate)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),SHIFT order by TO_cHAR(a.VCHDATE,'YYYYMM')";
                squery3 = "SELECT YR,SUM(SHIFT_A) AS SHIFT_A,SUM(SHIFT_B) AS SHIFT_B FROM (SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,decode(upper(shift),'SHIFT A', round(sum(a.prod_qty/1000)),0) as SHIFT_a,decode(upper(shift),'SHIFT B', round(sum(a.prod_qty/1000)),0) as SHIFT_B  FROM (SELECT a.vchdate ,TRIM(A.COL23) AS SHIFT,TRIM(A.COL25) AS MACHINE,SUM(A.QTY)*B.IWEIGHT AS PROD_QTY,0 AS Rej_Qty FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and A.TYPE='40' and a.vchdate " + PrdRange + " GROUP BY  TRIM(A.COL23) ,TRIM(A.COL25) ,B.IWEIGHT,a.vchdate)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),SHIFT ) GROUP BY YR ORDER BY YR ";

                chart4 = "bar";
                leftHeading4 = "";
                bottomHeading4 = "Rej in Kgs";
                stitle4 = "Shift Wise Monthly Comparison Rejection";
                gu4 = "";
                gl4 = "";
                squery4 = "SELECT YR,SUM(SHIFT_A) AS SHIFT_A,SUM(SHIFT_B) AS SHIFT_B FROM (SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,DECODE(Shift_Name,'SHIFT A', round(sum(a.rej_qty/1000)),0) as SHIFT_A,DECODE(Shift_Name,'SHIFT B', round(sum(a.rej_qty/1000)),0) as SHIFT_B FROM (Select vchdate, OBSV15 as Shift_Name,Title as Machine,0 AS PROD_QTY, sum(qty8) as Rej_Qty from inspvch  where branchcd='" + frm_mbr + "' and type='45' and vchdate " + PrdRange + " group by vchdate, Obsv15,Title)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),Shift_Name ) GROUP BY YR ORDER BY YR ";
                break;
            case "P17006E":
                chart1 = "area";
                leftHeading1 = "";
                bottomHeading1 = "In Min";
                stitle1 = "Machine Downtime Report Day Wise";
                gu1 = "";
                gl1 = "";

                squery1 = "SELECT vch,SUM(MINS) AS MINS FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'DD-Mon') AS VCH,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='55' and BRANCHCD='" + frm_mbr + "' AND vchdate " + DayRange + " GROUP BY TO_CHAR(VCHDATE,'DD-Mon'),BRANCHCD,COL2,TITLE,COL1) A GROUP BY vch having SUM(MINS)>0 order by vch ";

                leftHeading2 = "";
                bottomHeading2 = "In Min";
                chart2 = "bar";
                stitle2 = "Machine Downtime Month Wise";
                gu2 = "";
                gl2 = "";

                squery2 = "SELECT vch,SUM(MINS) AS MINS FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'Mon-YY') AS VCH,to_char(Vchdate,'yyyymm') as vchd,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='55' and BRANCHCD='" + frm_mbr + "' AND vchdate " + PrdRange + " GROUP BY TO_CHAR(VCHDATE,'Mon-YY'),BRANCHCD,COL2,TITLE,COL1,to_char(Vchdate,'yyyymm') ) A GROUP BY vch,vchd having SUM(MINS)>0 order by vchd ";

                leftHeading3 = "";
                bottomHeading3 = "In KG";
                chart3 = "column";
                stitle3 = "Rejection Day Wise";
                gu3 = "";
                gl3 = "";

                squery3 = "SELECT VCH,SUM(MINS) AS QTY FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'DD-Mon') AS VCH,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='45' and BRANCHCD='" + frm_mbr + "' AND VCHDATE " + DayRange + " GROUP BY TO_CHAR(VCHDATE,'DD-Mon'),BRANCHCD,COL2,TITLE,COL1) A GROUP BY VCH order by VCH ";

                leftHeading4 = "";
                bottomHeading4 = "In Min";
                chart4 = "pie";
                stitle4 = "Rejection Month Wise";
                gu4 = "";
                gl4 = "";

                squery4 = "SELECT VCH,SUM(MINS) AS QTY FROM (SELECT BRANCHCD,SUM(is_number(COL3)) AS MINS,TO_CHAR(VCHDATE,'Mon-YY') AS VCH,to_char(Vchdate,'yyyymm') as vchd,TITLE AS Machine_Name,COL2 AS REASON_CODE,COL1 FROM INSPVCH WHERE TYPE='45' and BRANCHCD='" + frm_mbr + "' AND VCHDATE " + PrdRange + " GROUP BY TO_CHAR(VCHDATE,'Mon-YY'),BRANCHCD,COL2,TITLE,COL1,to_char(Vchdate,'yyyymm')) A GROUP BY VCH,vchd order by vchd ";
                break;
            case "P17006G":
                chart1 = "scatter";
                leftHeading1 = "";
                bottomHeading1 = "Days";
                gu1 = "";
                gl1 = "";
                stitle1 = "Day Wise Corrugation Machine Trim Wastage";
                squery1 = "SELECT TO_cHAR(a.VCHDATE,'DD-Mon') AS YR,round(SUM(IS_NUMBER(TIME1))) AS WASTAGE FROM  (SELECT DISTINCT vchnum,vchdate,scrp1,scrp2,time1,time2,COL25 FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' and TYPE='40' AND VCHDATE " + DayRange + ") A  GROUP BY TO_cHAR(VCHDATE,'DD-Mon') having round(SUM(IS_NUMBER(TIME1)))>0 order by TO_cHAR(VCHDATE,'DD-Mon')";

                chart2 = "area";
                leftHeading2 = "";
                bottomHeading2 = "Months";
                gu2 = "";
                gl2 = "";
                stitle2 = "Month Wise Corrugation Machine Trim Wastage";
                squery2 = "SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,round(SUM(IS_NUMBER(TIME1)/1000)) AS WASTAGE FROM  (SELECT DISTINCT vchnum,vchdate,scrp1,scrp2,time1,time2,COL25 FROM COSTESTIMATE WHERE  BRANCHCD='" + frm_mbr + "' and TYPE='40' AND VCHDATE " + PrdRange + ") A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM') having round(SUM(IS_NUMBER(TIME1)/1000))>0 order by TO_cHAR(a.VCHDATE,'YYYYMM')";

                chart3 = "area";
                leftHeading3 = "";
                bottomHeading3 = "Days";
                gu3 = "";
                gl3 = "";
                stitle3 = "Day Wise Corrugation Machine Total Wastage";
                squery3 = "SELECT VCH,ROUND(SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2))) AS WASTAGE FROM  (SELECT DISTINCT TO_char(vchdate,'dd-Mon') as vch,scrp1,scrp2,time1,time2,COL25,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='40' AND VCHDATE " + DayRange + " ) GROUP BY VCH having ROUND(SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2)))>0 order by VCH";

                chart4 = "bar";
                leftHeading4 = "";
                bottomHeading4 = "Months";
                gu4 = "";
                gl4 = "";
                stitle4 = "Month Wise Corrugation Machine Total Wastage";
                squery4 = "SELECT VCH,SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2)) AS WASTAGE FROM  (SELECT DISTINCT TO_char(vchdate,'Mon-YYYY') as vch,scrp1,scrp2,time1,time2,COL25,TO_CHAR(VCHDATE,'YYYYMM') AS VDD FROM COSTESTIMATE WHERE BRANCHCD='01' AND TYPE='40' AND VCHDATE " + PrdRange + ") GROUP BY VCH,VDD having SUM(is_number(SCRP1))+SUM(is_number(SCRP2))+SUM(is_number(TIME1))+SUM(is_number(TIME2))>0 order by VDD";
                break;
            case "P17006I":
                chart1 = "spline";
                leftHeading1 = "";
                bottomHeading1 = "Box/Min";
                stitle1 = "Daily Average Cycle Time of 3 ply jobs";
                gu1 = "";
                gl1 = "";
                squery1 = "SELECT TO_cHAR(VCHDATE,'DD-Mon') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs  FROM  (SELECT  DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + DayRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='3 PLY' GROUP BY TO_cHAR(VCHDATE,'DD-Mon') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'DD-Mon')";

                chart2 = "column";
                leftHeading2 = "";
                bottomHeading2 = "Box/Min";
                stitle2 = "Monthly Average Cycle Time of 3 ply jobs";
                gu2 = "";
                gl2 = "";
                squery2 = "SELECT TO_cHAR(VCHDATE,'Month') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs  FROM  (SELECT  DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + PrdRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='3 PLY' GROUP BY TO_cHAR(VCHDATE,'Month'),TO_cHAR(VCHDATE,'yyyymm') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'yyyymm')";
                val_legnd2 = "Box";

                chart3 = "spline";
                leftHeading3 = "";
                bottomHeading3 = "Box/Min";
                stitle3 = "Daily Average Cycle Time of 5 ply jobs";
                gu3 = "";
                gl3 = "";
                squery3 = "SELECT TO_cHAR(VCHDATE,'DD-Mon') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs  FROM  (SELECT  DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + DayRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='5 PLY' GROUP BY TO_cHAR(VCHDATE,'DD-Mon') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'DD-Mon')";

                chart4 = "column";
                leftHeading4 = "";
                bottomHeading4 = "Box/Min";
                stitle4 = "Monthly Average Cycle Time of 5 ply jobs";
                gu4 = "";
                gl4 = "";
                squery4 = "SELECT TO_cHAR(VCHDATE,'Month') AS yr,ROUND(sum(a1)/sum(is_number(tslot)),2) as Boxs FROM (SELECT DISTINCT A.BRANCHCD,A.VCHNUM,A.VCHDATE, A.TSLOT,A.A1, SUBSTR( B.PLY,1,1)||' '||'PLY' as ply  FROM PROD_SHEET  A , (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE,ICODE,COL15 AS PLY FROM INSPMST) B WHERE A.BRANCHCD='" + frm_mbr + "' AND A.VCHDATE " + PrdRange + "  AND A.TYPE IN('88','86')  AND TRIM(a.ICODE)=TRIM(B.ICODE) ) where trim(PLY)='5 PLY' GROUP BY TO_cHAR(VCHDATE,'Month'),TO_cHAR(VCHDATE,'yyyymm') having ROUND(sum(a1)/sum(is_number(tslot)),2)>0 order by TO_cHAR(VCHDATE,'yyyymm')";
                break;
            case "P17006K":
                chart1 = "bar";
                leftHeading1 = "";
                bottomHeading1 = "Days";
                stitle1 = "Shift Wise Monthly Comparison Production";
                gu1 = "";
                gl1 = "";
                stitle1 = "Shift Wise Monthly Comparison Production";
                squery1 = "select yr,round(sum(shift_a/1000)) as shift_a,round(sum(shift_b/1000)) as shift_b from (SELECT TO_cHAR(a.VCHDATE,'YYYYMM') as vdd,TO_cHAR(a.VCHDATE,'Month') AS YR,decode(SHIFT,'SHIFT A',round(sum(a.prod_qty),2),0) as SHIFT_a,decode(SHIFT,'SHIFT B',round(sum(a.prod_qty),2),0) as SHIFT_b FROM (SELECT a.vchdate ,TRIM(A.COL23) AS SHIFT,TRIM(A.COL25) AS MACHINE,SUM(A.QTY)*B.IWEIGHT AS PROD_QTY,0 AS Rej_Qty FROM COSTESTIMATE A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.BRANCHCD='" + frm_mbr + "' and A.TYPE='40' and a.vchdate " + PrdRange + " GROUP BY  TRIM(A.COL23) ,TRIM(A.COL25) ,B.IWEIGHT,a.vchdate)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),shift) group by vdd,yr order by vdd";

                chart2 = "bar";
                leftHeading2 = "";
                bottomHeading2 = "Days";
                stitle2 = "Shift Wise Monthly Comparison Rejection";
                gu2 = "";
                gl2 = "";
                squery2 = "SELECT YR,round(SUM(SHIFT_A/1000)) AS SHIFT_A,round(SUM(SHIFT_B/1000)) AS SHIFT_B FROM (SELECT TO_cHAR(a.VCHDATE,'Month') AS YR,DECODE(Shift_Name,'SHIFT A', round(sum(a.rej_qty/1000)),0) as SHIFT_A,DECODE(Shift_Name,'SHIFT B', round(sum(a.rej_qty/1000)),0) as SHIFT_B FROM (Select vchdate, OBSV15 as Shift_Name,Title as Machine,0 AS PROD_QTY, sum(qty8) as Rej_Qty from inspvch  where branchcd='" + frm_mbr + "' and type='45' and vchdate " + PrdRange + " group by vchdate, Obsv15,Title)  A  GROUP BY TO_cHAR(VCHDATE,'Month'), TO_cHAR(a.VCHDATE,'YYYYMM'),Shift_Name ) GROUP BY YR ORDER BY YR ";

                chart3 = "bar";
                leftHeading3 = "";
                bottomHeading3 = "Days";
                stitle3 = "Shift Wise Monthly Comparison Job Completion";
                gu3 = "";
                gl3 = "";
                squery3 = "SELECT SHIFT,COUNT(VCHNUM) as qty  FROM (SELECT DISTINCT BRANCHCD,TYPE,VCHNUM,VCHDATE, ENQNO AS JOB_NO,TRIM(COL23) AS SHIFT FROM COSTESTIMATE  WHERE TYPE='40' AND BRANCHCD='" + frm_mbr + "' and vchdate " + DayRange + ")  A  GROUP BY SHIFT order by SHIFT";

                chart4 = "bar";
                leftHeading4 = "";
                bottomHeading4 = "Days";
                stitle4 = "Shift Wise Monthly Comparison Job Completion";
                gu4 = "";
                gl4 = "";
                squery4 = "select obsv15 as shift,round(sum(is_number(col3)),2) as downtime_mins from (select to_char(vchdate,'Mon-yyyy') as vch,obsv15,col3 from inspvch where branchcd='" + frm_mbr + "' and type='55' and vchdate " + DayRange + ") group by obsv15 order by shift";
                break;
            case "P17006M":
                chart1 = "column";
                leftHeading1 = "Values";
                bottomHeading1 = "Days";
                stitle1 = "Prodn Planning Vs Prodn Day Wise";
                gu1 = "";
                gl1 = "";
                squery1 = "select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as job_dt,icode,iqtyout,0 as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='90' and VCHDATE " + DayRange + " union all select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as vchdate,icode,0 as iqtyout,iqtyin as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE " + DayRange + "";
                squery1 = "select to_char(a.vchdate,'DD-Mon') as vchdate,round(sum(a.iqtyout/1000)) as Plan_qty,round(sum(a.prodn/1000)) as Prodn_Qty from (" + squery1 + ") a group by to_char(a.vchdate,'DD-Mon') having (round(sum(a.iqtyout/1000)) + round(sum(a.prodn/1000)))>0 order by to_char(a.vchdate,'DD-Mon')";

                leftHeading3 = "Values";
                bottomHeading3 = "Days";
                chart3 = "column";
                stitle3 = "Prodn Vs Rejection Day Wise";
                gu3 = "";
                gl3 = "";
                squery3 = "select vchdate,vchnum,icode,iqtyin as prodn,0 as rejqty  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE " + DayRange + " union all select vchdate,vchnum,icode,0 as prodn,qty8 as rejqty  from inspvch where branchcd='" + frm_mbr + "' and type='45' and VCHDATE " + DayRange + "";
                squery3 = "select to_char(a.vchdate,'dd-Mon') as vchdate,round(sum(a.Prodn/1000)) as Prodn_qty,round(sum(a.Rejqty/1000)) as Rejn_Qty from (" + squery3 + ") a group by to_char(a.vchdate,'dd-Mon') order by to_char(a.vchdate,'dd-Mon')";
                break;
            case "P17006N":
                leftHeading2 = "Values";
                bottomHeading2 = "Months";
                chart2 = "bar";
                stitle2 = "Prodn Planning Vs Prodn Month Wise";
                gu2 = "Prodn Planning Vs Prodn";
                gl2 = "During " + frm_CDT1 + " to " + frm_CDT2;
                squery2 = "select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as job_dt,icode,iqtyout,0 as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='90' and VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') union all select vchdate,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as vchdate,icode,0 as iqtyout,iqtyin as prodn  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') ";
                squery2 = "select to_char(a.vchdate,'Mon-YY') as mth ,round(sum(a.iqtyout/1000)) as Plan_qty,round(sum(a.prodn/1000)) as Prodn_Qty from (" + squery2 + ") a group by to_char(a.vchdate,'Mon-YY') order by to_char(a.vchdate,'Mon-YY')";

                leftHeading4 = "Values";
                bottomHeading4 = "Months";
                chart4 = "bar";
                stitle4 = "Prodn Vs Rejection Month Wise";
                gu4 = "Prodn Vs Rejection";
                gl4 = "During " + frm_CDT1 + " to " + frm_CDT2;
                squery4 = "select vchdate,vchnum,icode,iqtyin as prodn,0 as rejqty  from prod_Sheet where branchcd='" + frm_mbr + "' and type='88' and VCHDATE " + PrdRange + " union all select vchdate,vchnum,icode,0 as prodn,qty8 as rejqty  from inspvch where branchcd='" + frm_mbr + "' and type='45' and VCHDATE " + PrdRange + "";
                squery4 = "select to_char(a.vchdate,'Mon-YY') as vchdate,round(sum(a.Prodn/1000)) as Prodn_qty,round(sum(a.Rejqty/1000)) as Rejn_Qty from (" + squery4 + ") a group by to_char(a.vchdate,'Mon-YY') order by to_char(a.vchdate,'Mon-YY')";
                break;
            case "P17006O":
                chart1 = "column";
                leftHeading1 = "Values";
                bottomHeading1 = "Days";
                stitle1 = "Capacity Vs Prodn Day Wise";
                gu1 = "";
                gl1 = "";

                squery1 = "select a.vchdate,a.vchnum,a.icode,a.iqtyin*b.iweight as prodn,0 as rejqty,a.iqtyin as Prodq,0 as rejq,b.iweight  from prod_Sheet a,item b  where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='88' and a.VCHDATE " + DayRange + " union all select a.vchdate,a.vchnum,a.icode,0 as prodn,a.qty8*b.iweight as rejqty,0 as prodq,a.qty8 as Rej_Qty,b.iweight  from inspvch a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='45' and a.VCHDATE " + DayRange + "";
                squery1 = "select to_char(a.vchdate,'dd-Mon') as vchdate ,round(sum(a.Prodn)/1000,0) as Prodn_Wt,round(sum(a.Rejqty)/1000,0) as Rejn_Wt from (" + squery1 + ") a group by to_char(a.vchdate,'dd-Mon') order by to_char(a.vchdate,'dd-Mon')";

                leftHeading3 = "Values";
                bottomHeading3 = "Days";
                chart3 = "column";
                stitle3 = "Prodn Vs Completion Day Wise";
                gu3 = "";
                gl3 = "";

                squery3 = "select to_char(a.dated,'dd-Mon') as dated ,sum(a.prodn) as Prodn_qty,(Case when sum(a.Job_Qty)>0 and sum(a.prodn)>0 then round((sum(a.prodn)/sum(a.job_Qty))*100,2) else 0 end) as Completion from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE a.vchdate " + DayRange + " and A.SRNO=0 AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,is_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.vchdate " + DayRange + " and a.branchcd='" + frm_mbr + "' and a.type='60')a, item b where trim(A.erp_Code)=trim(B.icode) group by to_char(a.dated,'dd-Mon') having sum(a.Job_Qty)-sum(a.prodn)>0 order by to_char(a.dated,'dd-Mon')";
                break;
            case "P17006P":
                leftHeading2 = "Values";
                bottomHeading2 = "Months";
                chart2 = "spline";
                stitle2 = "Capacity Vs Prodn Month Wise";
                gu2 = "Capacity Vs Prodn";
                gl2 = "During " + frm_CDT1 + " to " + frm_CDT2;

                squery2 = "select a.vchdate,a.vchnum,a.icode,a.iqtyin*b.iweight as prodn,0 as rejqty,a.iqtyin as Prodq,0 as rejq,b.iweight  from prod_Sheet a,item b  where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='88' and a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') union all select a.vchdate,a.vchnum,a.icode,0 as prodn,a.qty8*b.iweight as rejqty,0 as prodq,a.qty8 as Rej_Qty,b.iweight  from inspvch a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='45' and a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy')";
                squery2 = "select to_char(a.vchdate,'Mon-YY') as vchdate ,round(sum(a.Prodn)/1000,0) as Prodn_Wt,round(sum(a.Rejqty)/1000,0) as Rejn_Wt from (" + squery2 + ") a group by to_char(a.vchdate,'Mon-YY') order by to_char(a.vchdate,'Mon-YY')";

                leftHeading4 = "Values";
                bottomHeading4 = "Months";
                chart4 = "bar";
                stitle4 = "Prodn Vs Completion Month Wise";
                gu4 = "Prodn Vs Completion";
                gl4 = "During " + frm_CDT1 + " to " + frm_CDT2;

                squery4 = "select to_char(a.dated,'Mon-yy') as dated ,sum(a.prodn) as Prodn_qty,(Case when sum(a.Job_Qty)>0 and sum(a.prodn)>0 then round((sum(a.prodn)/sum(a.job_Qty))*100,2) else 0 end) as Completion from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') and A.SRNO=0 AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,is_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.VCHDATE BETWEEN TO_DATE('" + frm_CDT1 + "','dd/mm/yyyy') and TO_DATE('" + frm_CDT2 + "','dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='60')a, item b where trim(A.erp_Code)=trim(B.icode) group by to_char(a.dated,'Mon-yy') having sum(a.Job_Qty)-sum(a.prodn)>0 order by to_char(a.dated,'Mon-yy')";
                break;
            case "P17006Q":

                break;
            case "P17006S":

                break;
            case "P17006U":

                break;
            case "P17006W":
                chart1 = "line";
                leftHeading1 = "";
                bottomHeading1 = "Values in K";
                stitle1 = "Day Wise Sales";
                gu1 = "";
                gl1 = "";

                squery1 = "select vch,round(sum(bill_tot/1000)) as output from (select  to_char(vchdate,'dd-Mon') as vch,bill_tot,to_char(vchdate,'yyyymmdd') as vdd from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + DayRange + ") group by vch order by vch";

                chart2 = "spline";
                leftHeading2 = "";
                bottomHeading2 = "Values in K";
                stitle2 = "Month Wise Sales";
                gu2 = "";
                gl2 = "";
                squery2 = "select vch,round(sum(bill_tot/1000)) as output from (select  to_char(vchdate,'Mon-yy') as vch,bill_tot,to_char(vchdate,'yyyymm') as vdd from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + PrdRange + ") group by vch,vdd order by vdd ";
                break;
        }

        chartDiv1.Attributes.Add("class", "col-lg-6");
        chartDiv2.Attributes.Add("class", "col-lg-6");
        chartDiv3.Attributes.Add("class", "col-lg-6");
        chartDiv4.Attributes.Add("class", "col-lg-6");
        chartDiv1.Visible = true; chartDiv2.Visible = true;
        chartDiv3.Visible = true; chartDiv4.Visible = true;

        lblChart1Header.Text = stitle1;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle1, chart1, gu1, gl1, squery1, val_legnd1, "chart1", bottomHeading1, leftHeading1);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart1", chartScript, false);
        else chartDiv1.Visible = false;

        lblChart2Header.Text = stitle2;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle2, chart2, gu2, gl2, squery2, val_legnd2, "chart2", bottomHeading2, leftHeading2);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart2", chartScript, false);
        else chartDiv2.Visible = false;

        lblChart3Header.Text = stitle3;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle3, chart3, gu3, gl3, squery3, val_legnd3, "chart3", bottomHeading3, leftHeading3);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart3", chartScript, false);
        else chartDiv3.Visible = false;

        lblChart4Header.Text = stitle4;
        chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle4, chart4, gu4, gl4, squery4, val_legnd4, "chart4", bottomHeading4, leftHeading4);
        if (chartScript.Length > 0) ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart4", chartScript, false);
        else chartDiv4.Visible = false;

        if (chartDiv1.Visible == false) chartDiv2.Attributes.Add("class", "col-lg-12");
        if (chartDiv2.Visible == false) chartDiv1.Attributes.Add("class", "col-lg-12");
        if (chartDiv3.Visible == false) chartDiv4.Attributes.Add("class", "col-lg-12");
        if (chartDiv4.Visible == false) chartDiv3.Attributes.Add("class", "col-lg-12");
        if (chartDiv1.Visible == false && chartDiv2.Visible == false) { chartDiv3.Attributes.Add("class", "col-lg-12"); chartDiv4.Attributes.Add("class", "col-lg-12"); }
        if (chartDiv3.Visible == false && chartDiv4.Visible == false) { chartDiv1.Attributes.Add("class", "col-lg-12"); chartDiv2.Attributes.Add("class", "col-lg-12"); }

        kz++;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ", kz.ToString());
        if (chartDiv1.Visible == false && chartDiv2.Visible == false && chartDiv3.Visible == false && chartDiv4.Visible == false) timer1_Tick("", EventArgs.Empty);
    }
    protected void btnBox1_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnBox2_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnBox3_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnBox4_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        askPopUp();
        Client_Code = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        printGraph();
        printHeads();
    }
    protected void timer1_Tick(object sender, EventArgs e)
    {
        iconDt = (DataTable)ViewState["icodeDt"];
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ"));
        for (int i = 0; i < iconDt.Rows.Count; i++)
        {
            if (i == kz)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", iconDt.Rows[i]["id"].ToString().Trim());
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID2", iconDt.Rows[i]["id"].ToString().Trim());
                printGraph();
                //upd.Update();                                
                break;
            }
            if (kz >= iconDt.Rows.Count)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ", "0");
                kz = 0;
                i = 0;
            }
        }
    }
    protected void btnPlay_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = true;
        btnPause.Visible = true;
        btnPlay.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID2");
        printGraph(Prg_Id);
        //btnNext.Disabled = false;
    }
    protected void btnPause_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = false;
        btnPlay.Visible = true;
        btnPause.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID2");
        printGraph(Prg_Id);
        //btnNext.Disabled = true;
    }
    protected void btnZoom1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnNext_ServerClick(object sender, EventArgs e)
    {
        iconDt = (DataTable)ViewState["icodeDt"];
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ"));
        for (int i = 0; i < iconDt.Rows.Count; i++)
        {
            if (i == kz)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", iconDt.Rows[i]["id"].ToString().Trim());
                printGraph();
                //upd.Update();                                
                break;
            }
            if (kz >= iconDt.Rows.Count)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ", "0");
                kz = 0;
                i = 0;
            }
        }
    }
    protected void btnLeft_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = false;
        btnPlay.Visible = true;
        btnPause.Visible = false;

        iconDt = (DataTable)ViewState["icodeDt"];
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ"));
        for (int i = 0; i < iconDt.Rows.Count; i++)
        {
            if (i == (kz - 1))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", iconDt.Rows[i]["id"].ToString().Trim());
                printGraph();
                //upd.Update();                                
                break;
            }
            if (kz >= iconDt.Rows.Count)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ", "0");
                kz = 0;
                i = 0;
            }
        }
    }
    protected void btnRight_Click(object sender, ImageClickEventArgs e)
    {
        timer1.Enabled = false;
        btnPlay.Visible = true;
        btnPause.Visible = false;

        iconDt = (DataTable)ViewState["icodeDt"];
        kz = Convert.ToInt32(fgenMV.Fn_Get_Mvar(frm_qstr, "U_KZ"));
        for (int i = 0; i < iconDt.Rows.Count; i++)
        {
            if (i == (kz + 1))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", iconDt.Rows[i]["id"].ToString().Trim());
                printGraph();
                //upd.Update();                                
                break;
            }
            if (kz >= iconDt.Rows.Count)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_KZ", "0");
                kz = 0;
                i = 0;
            }
        }
    }
}