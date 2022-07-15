using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_pay : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange,frm_cDt1,frm_cDt2, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld, fileName, filepath, zipFilePath, zipFileName;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string party_cd, part_cd;
    string frm_UserID;
    fgenDB fgen = new fgenDB();
    DataSet dsRep;

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

                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
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

                case "P15005Y":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", HCID);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F15127":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                ////MADE BY AKSHAY...MERGE BY YOGITA ON 2 APRIL 2018
                case "F40128"://DOWN TIME CHECKLIST
                case "F40129"://REJECTION CHECKLIST
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F82552":// SEARCH EMPLOYEE MASTER          
                case "F82565":// DEDUCTION TREND
                case "F82567":// PAY SUMAMRY
                case "F82569":// CATEGORY DESIGNATION DEPT WISE
                case "F82581":// SECTION WISE SUMMARY 
                case "F85157":// EMPLOYEE PENDING CONFIRMATION LIST
                case "F85159":
                case "F85160":
                case "F85163":
                    SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I' AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                    header_n = "Select Grade";
                    break;

                case "F82571"://WELFARE REPORT
                case "F85151"://welfare contribution report
                    SQuery = "Select x.Name,x.fhname,x.sex as Gender,x.sp_relashn,TO_char(x.dtjoin,'dd/mm/yyyy') as dtjoin,x.leaving_Dt,x.empcode,x.mobile,to_char(x.D_O_B,'dd/mm/yyyy') as D_O_B, x.Adharno,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Employee_shr,to_char(2*(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar)),'99,99,99,999.99') as Emplr_share,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar  from (Select b.Name ,b.fhname,b.sex,b.sp_relashn,b.dtjoin,b.leaving_Dt,b.empcode,b.mobile,b.D_O_B, b.Adharno,decode(to_chaR(date_,'yyyymm'),'" + year + "04',sum(a.ded6),0) as April,decode(to_chaR(date_,'yyyymm'),'" + year + "05',sum(a.ded6),0) as May,decode(to_chaR(date_,'yyyymm'),'" + year + "06',sum(a.ded6),0) as June,decode(to_chaR(date_,'yyyymm'),'" + year + "07',sum(a.ded6),0) as July,decode(to_chaR(date_,'yyyymm'),'" + year + "08',sum(a.ded6),0) as August,decode(to_chaR(date_,'yyyymm'),'" + year + "09',sum(a.ded6),0) as Sept,decode(to_chaR(date_,'yyyymm'),'" + year + "10',sum(a.ded6),0) as Oct,decode(to_chaR(date_,'yyyymm'),'" + year + "11',sum(a.ded6),0) as Nov,decode(to_chaR(date_,'yyyymm'),'" + year + "12',sum(a.ded6),0) as Dec ,decode(to_chaR(date_,'yyyymm'),'" + (fgen.make_double(year) + 1) + "01',sum(a.ded6),0) as Jan,decode(to_chaR(date_,'yyyymm'),'" + (fgen.make_double(year) + 1) + "02',sum(a.ded6),0) as Feb,decode(to_chaR(date_,'yyyymm'),'" + (fgen.make_double(year) + 1) + "03',sum(a.ded6),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd='" + mbr + "' and a.date_ " + xprdrange + "  group by b.mobile,a.date_,b.grade,b.empcode,b.Name ,b.fhname,b.D_O_B, b.Adharno,b.sex,b.sp_relashn,b.dtjoin,b.leaving_Dt) x group by x.mobile,x.Name ,x.empcode,x.fhname,x.sex,x.sp_relashn,to_char(x.dtjoin,'dd/mm/yyyy'),x.leaving_Dt,x.D_O_B, x.Adharno order by x.name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Welfare Report", frm_qstr);
                    SQuery = "";
                    break;

                case "F82555"://EL RECORD
                    SQuery = "SELECT 'CODE' AS CODE,'CODE'  AS CHOICE ,'SORTING OPTION' AS SELECTION  FROM DUAL UNION ALL SELECT 'NAME' AS CODE,'NAME' AS CHOICE,'SORTING OPTION' AS SELECTION FROM DUAL";
                    header_n = "Select Choice";
                    break;

                case "F82562":
                    SQuery = "Select x.Grp,x.Rep,x.Account,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar  from (Select b.desg_Text as Account,'Salary' as Rep,b.deptt_text as grp,decode(to_chaR(date_,'mm'),04,sum(a.totern),0) as April,decode(to_chaR(date_,'mm'),05,sum(a.totern),0) as May,decode(to_chaR(date_,'mm'),06,sum(a.totern),0) as June,decode(to_chaR(date_,'mm'),07,sum(a.totern),0) as July,decode(to_chaR(date_,'mm'),08,sum(a.totern),0) as August,decode(to_chaR(date_,'mm'),09,sum(a.totern),0) as Sept,decode(to_chaR(date_,'mm'),10,sum(a.totern),0) as Oct,decode(to_chaR(date_,'mm'),11,sum(a.totern),0) as Nov,decode(to_chaR(date_,'mm'),12,sum(a.totern),0) as Dec ,decode(to_chaR(date_,'mm'),01,sum(a.totern),0) as Jan,decode(to_chaR(date_,'mm'),02,sum(a.totern),0) as Feb,decode(to_chaR(date_,'mm'),03,sum(a.totern),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd = '" + mbr + "' and a.date_  " + xprdrange + "  group by b.desg_Text,b.deptt_text,to_chaR(date_,'mm') union all  Select b.desg_Text as Account,'H/C' as Rep,b.deptt_text as grp,decode(to_chaR(date_,'mm'),04,count(a.empcode),0) as April,decode(to_chaR(date_,'mm'),05,count(a.empcode),0) as May,decode(to_chaR(date_,'mm'),06,count(a.empcode),0) as June,decode(to_chaR(date_,'mm'),07,count(a.empcode),0) as July,decode(to_chaR(date_,'mm'),08,count(a.empcode),0) as August,decode(to_chaR(date_,'mm'),09,count(a.empcode),0) as Sept,decode(to_chaR(date_,'mm'),10,count(a.empcode),0) as Oct,decode(to_chaR(date_,'mm'),11,count(a.empcode),0) as Nov,decode(to_chaR(date_,'mm'),12,count(a.empcode),0) as Dec ,decode(to_chaR(date_,'mm'),01,count(a.empcode),0) as Jan,decode(to_chaR(date_,'mm'),02,count(a.empcode),0) as Feb,decode(to_chaR(date_,'mm'),03,count(a.empcode),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd = '" + mbr + "' and a.date_  " + xprdrange + "  group by b.desg_Text,b.deptt_text,to_chaR(date_,'mm') ) x group by x.account,x.rep,x.grp order by x.grp,x.account,x.rep";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Payroll Trend- Deptt/Desg Wise,With H/C", frm_qstr);
                    break;

                //case "F82565": //SECTION WISE SUMAMRY DRILL DOWN REPORT (NOT REQUIRED AS IT IS DUPLICATE ICON NOW ASSIGN TO DEDUCTION TREND REPORT)
                //    SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I'  AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                //    header_n = "Select Grade";
                //    break;

                //case "WF_UPL":// welfare fund upload csv made by akshay
                //    SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I'  AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                //    header_n = "Select Type";
                //    break;

                case "F82580":
                    fgen.drillQuery(0, "select upper(fcomment) as fstr,'' as gstr,upper(fcomment) as Erp_Action,Count(Vchnum) as Actions,' ' as Lookup from  fininfo where trim(fcomment)='Pay Master Data Edited/Saved' group by upper(fcomment)  order by upper(fcomment)", frm_qstr);
                    fgen.drillQuery(1, "Select '' as fstr,upper(fcomment) as gstr,trim(Branchcd) as Branchcd,Type,trim(Vchnum) as vchnum,to_char(Vchdate,'dd/mm/yyyy') as vchdate,Ent_by as User_ID,Ent_dt as Dated,Terminal as Computer_Name from fininfo order by Ent_dt Desc,vchdate desc,vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("Master Update Log", frm_qstr);
                    break;

                case "F85234":
                case "F85165":
                    SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I'  AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                    header_n = "Select Grade";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F85234"||HCID=="F85165")
                {
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                }
                else
                {
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                }
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    /////MADE BY AKSHAY...MERGED BY YOGITA ON 2 APRIL 2018                 
                    case "F40126":
                        header_n = "31 Day Prodn Analysis ";
                        mq0 = value1;  //selected month value
                        mq1 = "";
                        mq1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where mthnum='" + mq0 + "'", "mthname");
                        //SQuery = "SELECT Item,Partno, sum(day_01+day_02+day_03+day_04+day_05+day_06+day_07+day_08+day_09+day_10+day_11+day_12+day_13+day_14+day_15+day_16+day_17+day_18+day_19+day_20+day_21+day_22+day_23+day_24+day_26+day_27+day_28+day_29+day_30+day_31) as total , sum(day_01) as day_01, sum(day_02) as day_02,sum(day_03) as day_03,sum(day_04) as day_04,sum(day_05) as day_05,sum(day_06) as day_06,sum(day_07) as day_07,sum(day_08) as day_08,sum(day_09) as day_09,sum(day_10) as day_10,sum(day_11) as day_11,sum(day_12) as day_12,sum(day_13) as day_13,sum(day_14) as day_14,sum(day_15) as day_15,sum(day_16) as day_16,sum(day_17) as day_17,sum(day_18) as day_18,sum(day_19) as day_19,sum(day_20) as day_20,sum(day_21) as day_21,sum(day_22) as day_22,sum(day_23) as day_23,sum(day_24) as day_24,sum(day_25) as day_25,sum(day_26) as day_26,sum(day_27) as day_27,sum(day_28) as day_28,sum(day_29) as day_29,sum(day_30) as day_30,sum(day_31) as day_31  , icode from ( Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'dd'),'01',sum(a.iqtyin),0) as Day_01, decode(to_chaR(vchdate,'dd'),'02',sum(a.iqtyin),0) as Day_02, decode(to_chaR(vchdate,'dd'),'03',sum(a.iqtyin),0) as Day_03, decode(to_chaR(vchdate,'dd'),'04',sum(a.iqtyin),0) as Day_04, decode(to_chaR(vchdate,'dd'),'05',sum(a.iqtyin),0) as Day_05, decode(to_chaR(vchdate,'dd'),'06',sum(a.iqtyin),0) as Day_06, decode(to_chaR(vchdate,'dd'),'07',sum(a.iqtyin),0) as Day_07, decode(to_chaR(vchdate,'dd'),'08',sum(a.iqtyin),0) as Day_08, decode(to_chaR(vchdate,'dd'),'09',sum(a.iqtyin),0) as Day_09, decode(to_chaR(vchdate,'dd'),'10',sum(a.iqtyin),0) as Day_10, decode(to_chaR(vchdate,'dd'),'11',sum(a.iqtyin),0) as Day_11, decode(to_chaR(vchdate,'dd'),'12',sum(a.iqtyin),0) as Day_12, decode(to_chaR(vchdate,'dd'),'13',sum(a.iqtyin),0) as Day_13, decode(to_chaR(vchdate,'dd'),'14',sum(a.iqtyin),0) as Day_14, decode(to_chaR(vchdate,'dd'),'15',sum(a.iqtyin),0) as Day_15, decode(to_chaR(vchdate,'dd'),'16',sum(a.iqtyin),0) as Day_16, decode(to_chaR(vchdate,'dd'),'17',sum(a.iqtyin),0) as Day_17, decode(to_chaR(vchdate,'dd'),'18',sum(a.iqtyin),0) as Day_18, decode(to_chaR(vchdate,'dd'),'19',sum(a.iqtyin),0) as Day_19, decode(to_chaR(vchdate,'dd'),'20',sum(a.iqtyin),0) as Day_20, decode(to_chaR(vchdate,'dd'),'21',sum(a.iqtyin),0) as Day_21, decode(to_chaR(vchdate,'dd'),'22',sum(a.iqtyin),0) as Day_22, decode(to_chaR(vchdate,'dd'),'23',sum(a.iqtyin),0) as Day_23, decode(to_chaR(vchdate,'dd'),'24',sum(a.iqtyin),0) as Day_24, decode(to_chaR(vchdate,'dd'),'25',sum(a.iqtyin),0) as Day_25, decode(to_chaR(vchdate,'dd'),'26',sum(a.iqtyin),0) as Day_26, decode(to_chaR(vchdate,'dd'),'27',sum(a.iqtyin),0) as Day_27,	 decode(to_chaR(vchdate,'dd'),'28',sum(a.iqtyin),0) as Day_28,decode(to_chaR(vchdate,'dd'),'29',sum(a.iqtyin),0) as Day_29,decode(to_chaR(vchdate,'dd'),'30',sum(a.iqtyin),0) as Day_30, decode(to_chaR(vchdate,'dd'),'31',sum(a.iqtyin),0) as Day_31, a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "' and substr(a.type,1,2)='15' and TO_CHAR(a.vchdate,'MM/YYYY')='" + mq0 + "/" + year + "'   group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'dd')  ) group by item,partno,icode order by item";
                        SQuery = "SELECT icode as item_code,Item as Item_Name,Partno, sum(day_01+day_02+day_03+day_04+day_05+day_06+day_07+day_08+day_09+day_10+day_11+day_12+day_13+day_14+day_15+day_16+day_17+day_18+day_19+day_20+day_21+day_22+day_23+day_24+day_26+day_27+day_28+day_29+day_30+day_31) as total , sum(day_01) as day_01, sum(day_02) as day_02,sum(day_03) as day_03,sum(day_04) as day_04,sum(day_05) as day_05,sum(day_06) as day_06,sum(day_07) as day_07,sum(day_08) as day_08,sum(day_09) as day_09,sum(day_10) as day_10,sum(day_11) as day_11,sum(day_12) as day_12,sum(day_13) as day_13,sum(day_14) as day_14,sum(day_15) as day_15,sum(day_16) as day_16,sum(day_17) as day_17,sum(day_18) as day_18,sum(day_19) as day_19,sum(day_20) as day_20,sum(day_21) as day_21,sum(day_22) as day_22,sum(day_23) as day_23,sum(day_24) as day_24,sum(day_25) as day_25,sum(day_26) as day_26,sum(day_27) as day_27,sum(day_28) as day_28,sum(day_29) as day_29,sum(day_30) as day_30,sum(day_31) as day_31 from ( Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'dd'),'01',sum(a.iqtyin),0) as Day_01, decode(to_chaR(vchdate,'dd'),'02',sum(a.iqtyin),0) as Day_02, decode(to_chaR(vchdate,'dd'),'03',sum(a.iqtyin),0) as Day_03, decode(to_chaR(vchdate,'dd'),'04',sum(a.iqtyin),0) as Day_04, decode(to_chaR(vchdate,'dd'),'05',sum(a.iqtyin),0) as Day_05, decode(to_chaR(vchdate,'dd'),'06',sum(a.iqtyin),0) as Day_06, decode(to_chaR(vchdate,'dd'),'07',sum(a.iqtyin),0) as Day_07, decode(to_chaR(vchdate,'dd'),'08',sum(a.iqtyin),0) as Day_08, decode(to_chaR(vchdate,'dd'),'09',sum(a.iqtyin),0) as Day_09, decode(to_chaR(vchdate,'dd'),'10',sum(a.iqtyin),0) as Day_10, decode(to_chaR(vchdate,'dd'),'11',sum(a.iqtyin),0) as Day_11, decode(to_chaR(vchdate,'dd'),'12',sum(a.iqtyin),0) as Day_12, decode(to_chaR(vchdate,'dd'),'13',sum(a.iqtyin),0) as Day_13, decode(to_chaR(vchdate,'dd'),'14',sum(a.iqtyin),0) as Day_14, decode(to_chaR(vchdate,'dd'),'15',sum(a.iqtyin),0) as Day_15, decode(to_chaR(vchdate,'dd'),'16',sum(a.iqtyin),0) as Day_16, decode(to_chaR(vchdate,'dd'),'17',sum(a.iqtyin),0) as Day_17, decode(to_chaR(vchdate,'dd'),'18',sum(a.iqtyin),0) as Day_18, decode(to_chaR(vchdate,'dd'),'19',sum(a.iqtyin),0) as Day_19, decode(to_chaR(vchdate,'dd'),'20',sum(a.iqtyin),0) as Day_20, decode(to_chaR(vchdate,'dd'),'21',sum(a.iqtyin),0) as Day_21, decode(to_chaR(vchdate,'dd'),'22',sum(a.iqtyin),0) as Day_22, decode(to_chaR(vchdate,'dd'),'23',sum(a.iqtyin),0) as Day_23, decode(to_chaR(vchdate,'dd'),'24',sum(a.iqtyin),0) as Day_24, decode(to_chaR(vchdate,'dd'),'25',sum(a.iqtyin),0) as Day_25, decode(to_chaR(vchdate,'dd'),'26',sum(a.iqtyin),0) as Day_26, decode(to_chaR(vchdate,'dd'),'27',sum(a.iqtyin),0) as Day_27, decode(to_chaR(vchdate,'dd'),'28',sum(a.iqtyin),0) as Day_28,decode(to_chaR(vchdate,'dd'),'29',sum(a.iqtyin),0) as Day_29,decode(to_chaR(vchdate,'dd'),'30',sum(a.iqtyin),0) as Day_30, decode(to_chaR(vchdate,'dd'),'31',sum(a.iqtyin),0) as Day_31, a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "' and substr(a.type,1,2)='15' and TO_CHAR(a.vchdate,'MM/YYYY')='" + mq0 + "/" + year + "'   group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'dd')  ) group by item,partno,icode order by item";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Daily Production Checklist for Month  " + mq1 + "", frm_qstr);
                        break;

                    ///made and merged by yogita 19-jan-2019
                    case "F82555"://EL RECORD
                        hfval.Value = value1;
                        fgen.Fn_open_dtbox("-", frm_qstr);
                        break;

                    case "F82567"://PAY SUMAMRY
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            if (Convert.ToInt32(value1) > 3 && Convert.ToInt32(value1) <= 12)
                            {

                            }
                            else { year = (Convert.ToInt32(year) + 1).ToString(); }
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            SQuery = "select a.empcode,b.name,a.present,a.hours,a.workdays as paydays,a.er1 as basic,a.er2 as conveyance,a.totern,a.totded,a.netslry from pay a,empmas b where trim(a.branchcd)||trim(a.empcode)=trim(b.branchcd)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and to_char(a.date_,'mm/yyyy')='" + value1 + "/" + year + "' order by b.name";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Pay Summary for the Month " + m1 + " " + year, frm_qstr);
                        }
                        break;

                    //case "F82565":
                    //if (hfval.Value == "")
                    //{
                    //    hfval.Value = value1;
                    //    SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    //}
                    //else
                    //{
                    //    if (Convert.ToInt32(value1) > 3 && Convert.ToInt32(value1) <= 12)
                    //    {

                    //    }
                    //    else { year = (Convert.ToInt32(year) + 1).ToString(); }
                    //    fgen.drillQuery(0, "select b.fpfnominee as fstr,'-' as gstr, b.fpfnominee as section,sum(a.workdays) as paydays,count(a.empcode) as employees,sum(a.totern) as gross from pay a,empmas b  where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "'  and to_char(a.date_,'mm/yyyy')='" + value1 + "/" + year + "' group by b.fpfnominee", frm_qstr);
                    //    fgen.drillQuery(1, "select a.empcode as fstr, b.fpfnominee as gstr,b.fpfnominee as section,b.name,a.empcode,sum(a.workdays) as paydays,sum(a.totern) as gross from pay a,empmas b  where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and to_char(a.date_,'mm/yyyy')='" + value1 + "/" + year + "'  group by b.fpfnominee,b.name,a.empcode order by b.name", frm_qstr);
                    //    fgen.Fn_DrillReport("Section Wise Summary (Drill-Down Report)", frm_qstr);
                    //}
                    //break;

                    case "F82569":
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "SELECT 'Y' AS FSTR,'YES' AS SELECTION,'Desg' as CHOICE FROM DUAL UNION ALL SELECT 'N' AS FSTR,'NO' AS SELECTION,'Dept' as choice from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else
                        {
                            hfval.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F82565":
                        mq1 = "";
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "select trim(ed_fld) as fstr,ed_fld as code,ed_name from WB_selmast where branchcd='" + mbr + "' and grade='" + hfval.Value + "' and ed_fld like 'DED%' and nvl(icat,'-')!='Y' order by morder";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Deduction Head", frm_qstr);
                        }
                        else
                        {
                            if (value1 == "Y")
                            {
                                mq1 = value1;
                                mq2 = fgen.seek_iname(frm_qstr, co_cd, "select ed_name from WB_selmast where branchcd='" + mbr + "' and grade='" + hfval.Value + "' and ed_fld like 'DED%' and nvl(icat,'-')!='Y'", "ed_name");
                                //SQuery = "select a.EMPNAME as Account,'" + mq2 + "' as REP,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select trim(b.NAME) as EMPNAME,a.grade,decode(to_char(a.date_,'mm'),'03',a." + mq1 + ",0) as mar,decode(to_char(a.date_,'mm'),'04',a." + mq1 + ",0) as apr,decode(to_char(a.date_,'mm'),'05',a." + mq1 + ",0) as may,decode(to_char(a.date_,'mm'),'06',a." + mq1 + ",0) as jun,decode(to_char(a.date_,'mm'),'07',a." + mq1 + ",0) as jul,decode(to_char(a.date_,'mm'),'08',a." + mq1 + ",0) as aug,decode(to_char(a.date_,'mm'),'09',a." + mq1 + ",0) as sept,decode(to_char(a.date_,'mm'),'10',a." + mq1 + ",0) as oct,decode(to_char(a.date_,'mm'),'11',a." + mq1 + ",0) as nov,decode(to_char(a.date_,'mm'),'12',a." + mq1 + ",0) as dec,decode(to_char(a.date_,'mm'),'01',a." + mq1 + ",0) as jan,decode(to_char(a.date_,'mm'),'02',a." + mq1 + ",0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd!='DD' AND  a.date_ " + xprdrange + ") a group by a.EMPNAME"; //by me
                                SQuery = "Select x.Account,x.grp,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar  from (Select b.Name as Account,'" + mq2 + "' as Rep,b.fhname as grp,decode(to_chaR(date_,'yyyymm')," + year + "04,sum(a." + hf1.Value + "),0) as April,decode(to_chaR(date_,'yyyymm')," + year + "05,sum(a." + hf1.Value + "),0) as May,decode(to_chaR(date_,'yyyymm')," + year + "06,sum(a." + hf1.Value + "),0) as June,decode(to_chaR(date_,'yyyymm')," + year + "07,sum(a." + hf1.Value + "),0) as July,decode(to_chaR(date_,'yyyymm')," + year + "08,sum(a." + hf1.Value + "),0) as August,decode(to_chaR(date_,'yyyymm')," + year + "09,sum(a." + hf1.Value + "),0) as Sept,decode(to_chaR(date_,'yyyymm')," + year + "10,sum(a." + hf1.Value + "),0) as Oct,decode(to_chaR(date_,'yyyymm')," + year + "11,sum(a." + hf1.Value + "),0) as Nov,decode(to_chaR(date_,'yyyymm')," + year + "12,sum(a." + hf1.Value + "),0) as Dec ,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "01,sum(a." + hf1.Value + "),0) as Jan,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "02,sum(a." + hf1.Value + "),0) as Feb,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "03,sum(a." + hf1.Value + "),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd!='DD' and a.date_ between to_Date('01/04/" + year + "','dd/mm/yyyy') and to_date('31/03/" + (Convert.ToInt32(year) + 1) + "','dd/mm/yyyy')  group by b.Name,b.fhname,to_chaR(date_,'yyyymm') union all  Select b.desg_Text as Account,'H/C' as Rep,b.deptt_text as grp,decode(to_chaR(date_,'yyyymm')," + year + "04,count(a.empcode),0) as April,decode(to_chaR(date_,'yyyymm')," + year + "05,count(a.empcode),0) as May,decode(to_chaR(date_,'yyyymm')," + year + "06,count(a.empcode),0) as June,decode(to_chaR(date_,'yyyymm')," + year + "07,count(a.empcode),0) as July,decode(to_chaR(date_,'yyyymm')," + year + "08,count(a.empcode),0) as August,decode(to_chaR(date_,'yyyymm')," + year + "09,count(a.empcode),0) as Sept,decode(to_chaR(date_,'yyyymm')," + year + "10,count(a.empcode),0) as Oct,decode(to_chaR(date_,'yyyymm')," + year + "11,count(a.empcode),0) as Nov,decode(to_chaR(date_,'yyyymm')," + year + "12,count(a.empcode),0) as Dec ,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "01,count(a.empcode),0) as Jan,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "02,count(a.empcode),0) as Feb,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "03,count(a.empcode),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd = 'CCCCC' and a.date_  between to_Date('01/04/" + year + "','dd/mm/yyyy') and to_date('31/03/" + (Convert.ToInt32(year) + 1) + "','dd/mm/yyyy')  group by b.desg_Text,b.deptt_text,to_chaR(date_,'yyyymm') ) x group by x.account,x.rep,x.grp order by x.grp,x.account,x.rep"; // by mam
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Month Wise Deduction Trend Report", frm_qstr);
                            }
                            else if (value1 == "N")
                            {
                                mq1 = value1;
                                mq2 = fgen.seek_iname(frm_qstr, co_cd, "select ed_name from WB_selmast where branchcd='" + mbr + "' and grade='" + hfval.Value + "' and ed_fld like 'DED%' and nvl(icat,'-')!='Y'", "ed_name");
                                //SQuery = "select a.EMPNAME as Account,'" + mq2 + "' as REP,sum(a.mar+a.apr+a.may+a.jun+a.jul+a.aug+a.sept+a.oct+a.nov+a.dec+a.jan+a.feb) as total,sum(a.mar) as mar,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sept) as sept,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb from (select trim(b.NAME) as EMPNAME,a.grade,decode(to_char(a.date_,'mm'),'03',a." + mq1 + ",0) as mar,decode(to_char(a.date_,'mm'),'04',a." + mq1 + ",0) as apr,decode(to_char(a.date_,'mm'),'05',a." + mq1 + ",0) as may,decode(to_char(a.date_,'mm'),'06',a." + mq1 + ",0) as jun,decode(to_char(a.date_,'mm'),'07',a." + mq1 + ",0) as jul,decode(to_char(a.date_,'mm'),'08',a." + mq1 + ",0) as aug,decode(to_char(a.date_,'mm'),'09',a." + mq1 + ",0) as sept,decode(to_char(a.date_,'mm'),'10',a." + mq1 + ",0) as oct,decode(to_char(a.date_,'mm'),'11',a." + mq1 + ",0) as nov,decode(to_char(a.date_,'mm'),'12',a." + mq1 + ",0) as dec,decode(to_char(a.date_,'mm'),'01',a." + mq1 + ",0) as jan,decode(to_char(a.date_,'mm'),'02',a." + mq1 + ",0) as feb from pay a,empmas b  where trim(a.branchcd)||trim(b.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and  a.date_ " + xprdrange + ") a group by a.EMPNAME"; //by me
                                SQuery = "Select x.Account,x.grp,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar  from (Select b.Name as Account,'" + mq2 + "' as Rep,b.fhname as grp,decode(to_chaR(date_,'yyyymm')," + year + "04,sum(a." + hf1.Value + "),0) as April,decode(to_chaR(date_,'yyyymm')," + year + "05,sum(a." + hf1.Value + "),0) as May,decode(to_chaR(date_,'yyyymm')," + year + "06,sum(a." + hf1.Value + "),0) as June,decode(to_chaR(date_,'yyyymm')," + year + "07,sum(a." + hf1.Value + "),0) as July,decode(to_chaR(date_,'yyyymm')," + year + "08,sum(a." + hf1.Value + "),0) as August,decode(to_chaR(date_,'yyyymm')," + year + "09,sum(a." + hf1.Value + "),0) as Sept,decode(to_chaR(date_,'yyyymm')," + year + "10,sum(a." + hf1.Value + "),0) as Oct,decode(to_chaR(date_,'yyyymm')," + year + "11,sum(a." + hf1.Value + "),0) as Nov,decode(to_chaR(date_,'yyyymm')," + year + "12,sum(a." + hf1.Value + "),0) as Dec ,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "01,sum(a." + hf1.Value + "),0) as Jan,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "02,sum(a." + hf1.Value + "),0) as Feb,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "03,sum(a." + hf1.Value + "),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd='" + mbr + "' and a.date_ between to_Date('01/04/" + year + "','dd/mm/yyyy') and to_date('31/03/" + (Convert.ToInt32(year) + 1) + "','dd/mm/yyyy')  group by b.Name,b.fhname,to_chaR(date_,'yyyymm') union all  Select b.desg_Text as Account,'H/C' as Rep,b.deptt_text as grp,decode(to_chaR(date_,'yyyymm')," + year + "04,count(a.empcode),0) as April,decode(to_chaR(date_,'yyyymm')," + year + "05,count(a.empcode),0) as May,decode(to_chaR(date_,'yyyymm')," + year + "06,count(a.empcode),0) as June,decode(to_chaR(date_,'yyyymm')," + year + "07,count(a.empcode),0) as July,decode(to_chaR(date_,'yyyymm')," + year + "08,count(a.empcode),0) as August,decode(to_chaR(date_,'yyyymm')," + year + "09,count(a.empcode),0) as Sept,decode(to_chaR(date_,'yyyymm')," + year + "10,count(a.empcode),0) as Oct,decode(to_chaR(date_,'yyyymm')," + year + "11,count(a.empcode),0) as Nov,decode(to_chaR(date_,'yyyymm')," + year + "12,count(a.empcode),0) as Dec ,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "01,count(a.empcode),0) as Jan,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "02,count(a.empcode),0) as Feb,decode(to_chaR(date_,'yyyymm')," + (Convert.ToInt32(year) + 1) + "03,count(a.empcode),0) as Mar from pay a left outer join empmas b on  TRIM(A.branchcd)||TRIM(A.grade)||TRIM(A.empcode)=TRIM(b.branchcd)||TRIM(b.grade)||TRIM(b.empcode) where a.branchcd = 'CCCCC' and a.date_  between to_Date('01/04/" + year + "','dd/mm/yyyy') and to_date('31/03/" + (Convert.ToInt32(year) + 1) + "','dd/mm/yyyy')  group by b.desg_Text,b.deptt_text,to_chaR(date_,'yyyymm') ) x group by x.account,x.rep,x.grp order by x.grp,x.account,x.rep"; // by mam
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Month Wise Deduction Trend Report", frm_qstr);
                            }
                            else
                            {
                                hf1.Value = value1;
                                SQuery = "SELECT 'Y' AS FSTR,'ALL' AS SELECTION,'All Locn Wise' as CHOICE FROM DUAL UNION ALL SELECT 'N' AS FSTR,'CURR' AS SELECTION,'Locn' as choice from dual";
                                header_n = "Select Choice";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek(header_n, frm_qstr);
                            }
                        }
                        break;

                    //case "WF_UPL":
                    //    if (hf1.Value == "")
                    //    {
                    //        hf1.Value = value1;
                    //        SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                    //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //        fgen.Fn_open_sseek(header_n, frm_qstr);
                    //    }
                    //    else
                    //    {
                    //        hfval.Value = value1;
                    //        m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hfval.Value + "'", "mthname");
                    //        m1 = m1 + " " + year;
                    //        if (Convert.ToDateTime(m1) < Convert.ToDateTime("06/2019"))
                    //        {
                    //            fgen.msg("-", "AMSG", "this is wrong month"); return;
                    //        }
                    //        else
                    //        {
                    //            mq1 = hfval.Value + "/" + year;
                    //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value + "/" + year);// for month value
                    //            dt = new DataTable();
                    //            SQuery = "select trim(b.adharno) as adharno,trim(b.name) as empname, nvl(a.wf_sal,0) as wf_wages from pay a,empmas b where trim(a.empcode)=trim(b.empcode) and a.grade='" + hf1.Value + "' and to_char(a.date_,'MM/YYYY')='" + mq1 + "'";
                    //            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //            if (dt.Rows.Count > 0)
                    //            {
                    //                Session["send_dt"] = dt;
                    //                string fileName = co_cd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";
                    //                string filepath = Server.MapPath("~/tej-base/Upload/") + fileName;
                    //                //fgen.CreateCSVFile(dt, filepath);
                    //                fgen.CreateCSVFile(dt, @"c:\TEJ_ERP\UPLOAD\" + fileName);

                    //                Session["FilePath"] = fileName;
                    //                Session["FileName"] = fileName;
                    //                Response.Write("<script>");
                    //                Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                    //                Response.Write("</script>");
                    //                fgen.msg("-", "AMSG", "The file has been downloaded!!");

                    //                //fgen.Fn_open_rptlevel("Welfare Fund Upload", frm_qstr);
                    //            }
                    //            else { fgen.msg("-", "AMSG", "Data Not Found"); }
                    //        }
                    //    }
                    //    break;

                    case "F82581":
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select MOnth", frm_qstr);
                        }
                        else
                        {
                            string myear;
                            hfval.Value = value1;
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hfval.Value + "'", "mthname");
                            if (Convert.ToInt32(value1) > 3)
                            {        //same financial year     
                                myear = year;
                            }
                            else
                            {
                                int d = Convert.ToInt32(year) + 1;
                                myear = Convert.ToString(d);
                            }
                            mq0 = value1 + "/" + myear;
                            fgen.drillQuery(0, "Select trim(b.fpfnominee) as fstr,'-' as gstr, trim(b.fpfnominee) as Section_Name,sum(A.Workdays) as PayDays,count(a.empcode) as Employees,sum(a.totern) as Gross_Earning from pay a,empmas b where a.grade='" + hf1.Value + "' and trim(A.BRANCHCD)||trim(A.GRADE)||trim(a.empcode)=trim(B.BRANCHCD)||trim(B.GRADE)||trim(B.empcode) and a.branchcd='" + mbr + "' and to_char(a.date_,'mm/yyyy')='" + mq0 + "' group by b.fpfnominee order by b.fpfnominee", frm_qstr);
                            fgen.drillQuery(1, "Select '' as fstr, trim(b.fpfnominee) as gstr, b.fpfnominee as Section_Name,b.Name,sum(A.Workdays) as PayDays,a.Empcode,sum(a.totern) as Gross_Earning from pay a,empmas b where a.grade='" + hf1.Value + "' and A.BRANCHCD||A.GRADE||a.empcode=B.BRANCHCD||B.GRADE||B.empcode and a.branchcd='" + mbr + "' and to_char(a.date_,'mm/yyyy')='" + mq0 + "' group by b.fpfnominee,B.Name,a.empcode order by b.fpfnominee,B.Name", frm_qstr);
                            fgen.drillQuery(2, "", frm_qstr);
                            fgen.Fn_DrillReport("Section Wise Summary", frm_qstr);
                        }
                        break;

                    case "F82552":
                        SQuery = "select a.empcode as empcode,a.name as employee,a.fhname as Father_Husband_Name,b.name as grade,to_char(a.dtjoin,'dd/mm/yyyy') as joint_Dt,nvl(a.er1,0) as er1,nvl(a.er2,0) as er2,a.pfcut,a.esicut,A.INC_DT1 AS LAST_INCR ,a.desg_text as designation,a.deptt_text as department from empmas a ,type b where trim(a.grade)=trim(b.type1) and b.id= 'I' and a.branchcd='" + mbr + "' and a.grade ='" + value1 + "' order by employee";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Employee Search", frm_qstr);
                        break;

                    case "F85234":
                        #region
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            m1 = m1 + " " + year;
                            if (Convert.ToInt32(hf1.Value) < 3)
                            {
                                int j = Convert.ToInt32(year) + 1;
                                year = Convert.ToString(j);
                            }
                            mq1 = hf1.Value + "/" + year;
                            //SQuery = "select DISTINCT b.adharno,b.name,sum(a.netslry) as netsalary  from pay a,empmas b where  trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and to_char(date_,'mm/yyyy')='" + mq1 + "' group by  b.adharno,b.name order by b.name"; //netslry
                            SQuery = "select DISTINCT trim(a.empcode) as empcode,b.adharno,b.name,sum(nvl(a.er1,0)+nvl(a.er2,0)+nvl(a.er3,0)+nvl(a.er4,0)+nvl(a.er5,0)+nvl(a.er6,0)+nvl(a.er7,0)+nvl(a.er8,0)+nvl(a.er9,0)+nvl(a.er10,0)+nvl(a.er11,0)+nvl(a.er12,0)+nvl(a.er13,0)+nvl(a.er14,0)+nvl(a.er15,0)+nvl(a.er16,0)+nvl(a.er17,0)+nvl(a.er18,0)+nvl(a.er19,0)+nvl(a.er20,0)) as Gross_Salary from pay a,empmas b where  trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade in (" + hfval.Value + ") and to_char(date_,'mm/yyyy')='" + mq1 + "' group by trim(a.empcode),b.adharno,b.name order by b.name";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                Session["send_dt"] = dt;
                                fileName = co_cd + "_WELFARE" + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";
                                string filepath = Server.MapPath("~/tej-base/Upload/") + fileName;
                                //fgen.CreateCSVFile(dt, filepath);
                                fgen.CreateCSVFile(dt, @"c:\TEJ_ERP\UPLOAD\" + fileName);
                                Session["FilePath"] = fileName;
                                Session["FileName"] = fileName;
                                Response.Write("<script>");
                                Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                                Response.Write("</script>");
                                fgen.msg("-", "AMSG", "The file has been downloaded!!");
                            }
                            else { fgen.msg("-", "AMSG", "Data Not Found"); }
                            #region
                            //for grid report..........as per discussion with mayuri mam open rptlevel report
                            // fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            //  fgen.Fn_open_rptlevel("Welfare file for the Month '" + m1 + "'", frm_qstr);                          
                            #endregion
                        }
                        #endregion
                        break;

                    case "F85157":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            if (Convert.ToInt32(value1) > 3)
                            {
                                mq1 = year;
                            }
                            else
                            {
                                int d = Convert.ToInt32(year) + 1;
                                mq1 = Convert.ToString(d);
                            }
                            mq0 = value1 + "/" + mq1;
                            SQuery = "select empcode,name as empname,fhname as father_name,TO_CHAR(dtjoin,'dd/MM/yyyy') as join_dt,deptt2 as prob_mth,to_char(add_months(dtjoin,deptt2)-1,'dd/mm/yyyy') as conf_dur_dt,conf_dt as act_conf_Dt  from empmas where grade='" + hfval.Value + "' and branchcd='" + mbr + "' and substr(trim(conf_Dt),4,7)='" + mq0 + "' order by empcode";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Employee Pending Confirmation List", frm_qstr);
                        }
                        break;

                    case "F85159":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            if (Convert.ToInt32(value1) > 3)
                            {
                                mq1 = year;
                            }
                            else
                            {
                                mq1 = (Convert.ToInt32(year) + 1).ToString();
                            }
                            mq0 = value1 + "/" + mq1;
                            SQuery = "select trim(b.uinno) as memder_uan,trim(b.name) as member_name,a.totern as gross_wage,a.pf_sal as epf_wages,(case when nvl(a.age,0)<58 then a.pf_sal else 0 end) as eps_wages,(case when nvl(a.age,0)<58 then a.pf_sal else 0 end) as edli_wages,a.ded1 as epf_contri,round((a.ded1/(case when nvl(a.pf_rt_cs,0)=0 then 1 else a.pf_rt_cs/100 end))*(3.67/100)) as eps_contri,0 as er_contri,a.totdays-a.workdays as ncp_days,0 as refund_of_Adv from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and to_char(a.date_,'mm/yyyy')='" + mq0 + "'  and nvl(a.ded1,0)>0 order by member_name";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["er_contri"] = fgen.make_double(dt.Rows[i]["epf_contri"].ToString()) - fgen.make_double(dt.Rows[i]["eps_contri"].ToString().Trim());
                            }                            
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            Session["send_dt"] = dt;
                            fgen.Fn_open_rptlevel("Member Wise Salary Report", frm_qstr);
                        }
                        break;

                    case "F85160":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            if (Convert.ToInt32(value1) > 3)
                            {
                                mq1 = year;
                            }
                            else
                            {
                                mq1 = (Convert.ToInt32(year) + 1).ToString();
                            }
                            mq0 = value1 + "/" + mq1;
                            SQuery = "select trim(b.uinno) as memder_uan,trim(b.name) as member_name,a.totern as gross_wage,a.pf_sal as epf_wages,(case when nvl(a.age,0)<58 then a.pf_sal else 0 end) as eps_wages,(case when nvl(a.age,0)<58 then a.pf_sal else 0 end) as edli_wages,a.ded1 as epf_contri,0 as er_contri,round((a.ded1/(case when nvl(a.pf_rt_cs,0)=0 then 1 else a.pf_rt_cs/100 end))*(3.67/100)) as eps_contri,a.totdays-a.workdays as ncp_days,0 as refund_of_Adv from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and to_char(a.date_,'mm/yyyy')='" + mq0 + "' and nvl(a.ded1,0)>0 order by member_name";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["er_contri"] = fgen.make_double(dt.Rows[i]["epf_contri"].ToString()) - fgen.make_double(dt.Rows[i]["eps_contri"].ToString().Trim());
                            }
                            fileName = co_cd + "_" + mq3 + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xls";
                            filepath = Server.MapPath("~/tej-base/Upload/") + fileName;
                            fgen.CreateCSVFile(dt, @"c:\TEJ_ERP\UPLOAD\" + fileName, "#~#");
                            Session["FilePath"] = fileName;
                            Session["FileName"] = fileName;
                            Response.Write("<script>");
                            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                            Response.Write("</script>");
                            fgen.msg("-", "AMSG", "The file has been downloaded!!");;
                        }
                        break;

                    case "F85163":
                        hfval.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F85165":
                        // SQuery = "SELECT TO_CHAR(A.DATE_,'MM/YYYY') AS MONTH_,C.MTHNAME,A.GRADE,A.EMPCODE,B.NAME AS EMPNAME,B.FHNAME AS FATHER_NAME,SUM(A.TDS) AS TDS,TO_CHAR(A.DATE_,'yyyyMMdd') AS vdd FROM PAY A,EMPMAS B,MTHS C WHERE TRIM(A.BRANCHCD)||TRIM(A.GRADE)||TRIM(A.EMPCODE)=TRIM(B.BRANCHCD)||TRIM(B.GRADE)||TRIM(B.EMPCODE) AND TO_CHAR(DATE_,'MM')=TRIM(C.MTHNUM) AND A.BRANCHCD='" + mbr + "' AND A.GRADE IN (" + value1 + ") GROUP BY TO_CHAR(A.DATE_,'MM/YYYY') ,A.GRADE,A.EMPCODE,B.NAME,B.FHNAME,C.MTHNAME,TO_CHAR(A.DATE_,'yyyyMMdd')  ORDER BY vdd";//OLD
                        SQuery = "select a.mth,a.empcode,b.name,b.fhname,a.grade,sum(a.apr) as aPR,sum(a.may) as may,sum(a.june) as june,sum(a.july) as july,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar  from (select branchcd,EMPCODE,GRADE,TO_CHAR(DATE_,'MM/YYYY') AS MTH, (case when to_char(DATE_,'mm')='04' then TDS else 0 end) as apr,(case when to_char(DATE_,'mm')='05' then TDS else 0 end) as may,(case when to_char(DATE_,'mm')='06' then TDS else 0 end) as june,(case when to_char(DATE_,'mm')='07' then TDS else 0 end) as july,(case when to_char(DATE_,'mm')='08' then TDS else 0 end) as aug,(case when to_char(DATE_,'mm')='09' then TDS else 0 end) as sep,(case when to_char(DATE_,'mm')='10' then TDS else 0 end) as oct,(case when to_char(DATE_,'mm')='11' then TDS else 0 end) as nov,(case when to_char(DATE_,'mm')='12' then TDS else 0 end) as dec,(case when to_char(DATE_,'mm')='01' then TDS else 0 end) as jan,(case when to_char(DATE_,'mm')='02' then TDS else 0 end) as feb,(case when to_char(DATE_,'mm')='03' then TDS else 0 end) as mar from PAY where branchcd='" + mbr + "' and DATE_  " + xprdrange + "  ) a, empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) group by a.mth,a.empcode,b.name,b.fhname,a.grade ORDER BY A.MTH";
                        SQuery = "select a.empcode,b.name,b.fhname,a.grade,sum(a.apr) as aPR,sum(a.may) as may,sum(a.june) as june,sum(a.july) as july,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.june+a.july+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_tds from (select branchcd,EMPCODE,GRADE,TO_CHAR(DATE_,'MM/YYYY') AS MTH, (case when to_char(DATE_,'mm')='04' then TDS else 0 end) as apr,(case when to_char(DATE_,'mm')='05' then TDS else 0 end) as may,(case when to_char(DATE_,'mm')='06' then TDS else 0 end) as june,(case when to_char(DATE_,'mm')='07' then TDS else 0 end) as july,(case when to_char(DATE_,'mm')='08' then TDS else 0 end) as aug,(case when to_char(DATE_,'mm')='09' then TDS else 0 end) as sep,(case when to_char(DATE_,'mm')='10' then TDS else 0 end) as oct,(case when to_char(DATE_,'mm')='11' then TDS else 0 end) as nov,(case when to_char(DATE_,'mm')='12' then TDS else 0 end) as dec,(case when to_char(DATE_,'mm')='01' then TDS else 0 end) as jan,(case when to_char(DATE_,'mm')='02' then TDS else 0 end) as feb,(case when to_char(DATE_,'mm')='03' then TDS else 0 end) as mar from PAY where branchcd='" + mbr + "' and DATE_  " + xprdrange + "  ) a, empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) group by a.empcode,b.name,b.fhname,a.grade ORDER BY b.name";
                         fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                         fgen.Fn_open_rptlevel("EMPLOYEE TDS TREND FROM " + frm_cDt1 + " TO " + frm_cDt2 + " ", frm_qstr);
                         break;
                }
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
            
            // after prdDmp this will run   

            switch (val)
            {
                // GATE,PRODUCTION MODULE RELATED REPORTS WERE ADDED. THEY ARE REMOVED ON 28 NOV 2019.             
                ///made and merged by yogita on 9-jan-2019

                case "F82572"://Summary of Present ,EL,CL,SL Emp Wise 
                    SQuery = "select a.name ,a.fhname,sum(b.present) as present,sum(b.el) as el,sum(b.cl) as cl,sum(b.SL) AS SL,sum(b.netslry) as net_salary_paid ,a.empcode,a.old_empc from  empmas a,pay b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) AND A.BRANCHCD='" + mbr + "' and b.date_ " + xprdrange + " group by a.name,a.fhname,a.empcode,a.old_empc order by a.name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Summary of Present ,EL,CL,SL Emp Wise for the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F82555"://EL RECORD
                    if (hfval.Value == "CODE")
                    {
                        SQuery = "select b.Name,B.FHNAME,sum(a.opel) as op_EL,sum(a.el_Accru) as EL_Accrued,sum(a.taken) as EL_taken,sum(a.opel)+sum(a.el_Accru)-sum(a.taken) as EL_bal,a.Grade,a.Empcode,b.leaving_dt,a.branchcd,sum(a.day4calc) as Calc_on_dayz from (select branchcd,grade,empcode,0 as el_Accru,0 as taken,0 as day4calc,el as opel From empmas where branchcd='" + mbr + "' union all select branchcd,grade,empcode,el_Accru,0 as taken,day4calc as dayz,0 as opel  From pay_el where branchcd='" + mbr + "' and vchdatE>=to_DatE('01/07/2018','dd/mm/yyyy') and vchdate<=to_datE('" + value1 + "','dd/mm/yyyy') union all select branchcd,grade,empcode,0 as el_Accru,el,0 as dayz,0 as opel From pay where branchcd='" + mbr + "' and date_>=to_DatE('01/07/2018','dd/mm/yyyy') and date_<=to_datE('" + value1 + "','dd/mm/yyyy')) a,empmas b where length(trim(b.leaving_dt))<5 and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode group by a.branchcd,a.grade,b.leaving_dt,a.empcode,b.name,B.FHNAME ORDER BY a.EMPCODE,B.NAME";
                    }
                    else
                    {
                        SQuery = "select b.Name,B.FHNAME,sum(a.opel) as op_EL,sum(a.el_Accru) as EL_Accrued,sum(a.taken) as EL_taken,sum(a.opel)+sum(a.el_Accru)-sum(a.taken) as EL_bal,a.Grade,a.Empcode,b.leaving_dt,a.branchcd,sum(a.day4calc) as Calc_on_dayz from (select branchcd,grade,empcode,0 as el_Accru,0 as taken,0 as day4calc,el as opel From empmas where branchcd='" + mbr + "' union all select branchcd,grade,empcode,el_Accru,0 as taken,day4calc as dayz,0 as opel  From pay_el where branchcd='" + mbr + "' and vchdatE>=to_DatE('01/07/2018','dd/mm/yyyy') and vchdate<=to_datE('" + value1 + "','dd/mm/yyyy') union all select branchcd,grade,empcode,0 as el_Accru,el,0 as dayz,0 as opel From pay where branchcd='" + mbr + "' and date_>=to_DatE('01/07/2018','dd/mm/yyyy') and date_<=to_datE('" + value1 + "','dd/mm/yyyy')) a,empmas b where length(trim(b.leaving_dt))<5 and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode group by a.branchcd,a.grade,b.leaving_dt,a.empcode,b.name,B.FHNAME ORDER BY B.NAME,a.EMPCODE";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("EL Record As On " + value1 + " starting 01/07/2018", frm_qstr);
                    break;

                case "F82557"://Summary of Absentism
                    SQuery = "select b.name,sum(a.absent) as absent,a.grade,a.branchcd from pay a,type b where trim(a.grade)=trim(b.type1) and b.id='I' AND  a.branchcd='" + mbr + "' and a.date_ " + xprdrange + "  group by a.grade,a.branchcd,b.name ORDER BY A.GRADE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Grade Wise Absentism for the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F82559": //summary of leaves taken
                    SQuery = "select b.name,sum(a.el) as el,sum(a.cl) as cl,sum(a.sl) as sl,a.grade,a.branchcd from pay a,type b where trim(a.grade)=trim(b.type1) and b.id='I' AND a.branchcd='" + mbr + "' and a.date_ " + xprdrange + "  group by a.grade,a.branchcd,b.name ORDER BY A.GRADE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Grade Wise Absentism for the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F82569"://Category Designation dept wise //made by yogita in hr module            
                    if (hfval.Value == "Y")
                    {
                        // FOR DESIGNATION
                        fgen.drillQuery(0, "select a.grade as fstr,'-' as gstr,nvl(c.Name,'-') as Name,count(a.empcode) as Emp_Count,sum(b.netslry) as Net_salary,A.Grade from empmas a ,pay b ,(SELECT NAME ,TYPE1 FROM TYPE WHERE ID='I' AND substr(type1,1,1) in ('0','1'))c where a.grade=c.type1 and b.grade=c.type1 and a.branchcd||trim(a.grade)||trim(a.empcode)=b.branchcd||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hf1.Value + "' and b.date_  " + xprdrange + "  group by a.grade,c.name", frm_qstr);
                        fgen.drillQuery(1, "Select b.desg_text as fstr,a.grade as gstr, b.Desg_Text,Count(a.Empcode) as Emp_Count,sum(A.netslry) as Salary from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.date_ " + xprdrange + " group by b.Desg_Text,a.grade", frm_qstr);
                        fgen.Fn_DrillReport("", frm_qstr);
                    }
                    else
                    {
                        // FOR DEPARTMENT
                        fgen.drillQuery(0, "select a.grade as fstr,'-' as gstr,nvl(c.Name,'-') as Name,count(a.empcode) as Emp_Count,sum(b.netslry) as Net_salary,A.Grade from empmas a ,pay b ,(SELECT NAME ,TYPE1 FROM TYPE WHERE ID='I' AND substr(type1,1,1) in ('0','1'))c where a.grade=c.type1 and b.grade=c.type1 and a.branchcd||trim(a.grade)||trim(a.empcode)=b.branchcd||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hf1.Value + "' and b.date_  " + xprdrange + "  group by a.grade,c.name", frm_qstr);
                        fgen.drillQuery(1, "Select b.deptt_text as fstr,a.grade as gstr, b.Deptt_Text,Count(a.Empcode) as Emp_Count,sum(A.netslry) as Salary from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + mbr + "' and a.date_ " + xprdrange + " group by b.Deptt_Text,a.grade", frm_qstr);
                        fgen.Fn_DrillReport("", frm_qstr);
                    }
                    break;

                case "F85163":
                    if ((Math.Round(Convert.ToDateTime(todt).Subtract(Convert.ToDateTime(fromdt)).Days / (365.25 / 12), 0)) > 12)
                    {
                        fgen.msg("-", "AMSG", "Please Select Upto 12 Months");
                        return;
                    }                    
                    SQuery = "select empcode,name,father_husband_name,adharno,esino,epf_no,gender,mobile,date_of_birth,sum(totern) as gross_wage,date_of_joining, date_of_relieving,0 as total_Month,sum(ded6) as employee_share,sum(wf_amt_cs) as employer_share,sum(ded6+wf_amt_cs) as total,leaving_Dt from (select a.empcode,b.name,b.fhname as father_husband_name,b.adharno,b.esino,b.uinno as epf_no,b.sex as gender,b.mobile,to_char(b.d_o_b,'dd/mm/yyyy') as date_of_birth,a.totern,to_char(b.dtjoin,'dd/mm/yyyy') as date_of_joining,(case when nvl(trim(b.leaving_dt),'-')='-' then 'Till Present' else b.leaving_dt end) as date_of_relieving,a.ded6,a.wf_amt_cs,trim(b.leaving_dt) as leaving_Dt from pay a,empmas b where trim(a.branchcd)||trim(a.grade)||Trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||Trim(b.empcode) and a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and a.date_ " + xprdrange + " and b.dtjoin<to_date('" + todt + "','dd/mm/yyyy') and a.empcode in (select empcode from empmas a where a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and nvl(trim(a.leaving_dt),'-')='-' union all select empcode from empmas a where a.branchcd='" + mbr + "' and a.grade='" + hfval.Value + "' and length(nvl(trim(leaving_dt),'-'))=10 AND to_date(trim(leaving_dt),'dd/mm/yyyy') " + xprdrange + ")) group by empcode,name,father_husband_name,adharno,esino,epf_no,gender,mobile,date_of_birth,date_of_joining,date_of_relieving,leaving_Dt order by name";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    string stdt, enddt = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stdt = ""; enddt = "";
                        if (Convert.ToDateTime(dt.Rows[i]["date_of_joining"].ToString().Trim()) < Convert.ToDateTime(fromdt))
                        {
                            stdt = fromdt;
                        }
                        else
                        {
                            stdt = dt.Rows[i]["date_of_joining"].ToString().Trim();
                        }

                        if (dt.Rows[i]["leaving_dt"].ToString().Trim() == "-")
                        {
                            enddt = todt;
                        }
                        else
                        {
                            enddt = dt.Rows[i]["leaving_dt"].ToString().Trim();
                        }
                        dt.Rows[i]["total_month"] = Math.Round(Convert.ToDateTime(enddt).Subtract(Convert.ToDateTime(stdt)).Days / (365.25 / 12), 0);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("leaving_Dt");
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Welfare Report for the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;
            }
        }
    }
}