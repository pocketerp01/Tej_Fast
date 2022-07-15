using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_smktg : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, frm_cocd, SQuery, xprdrange,xdaterange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID, frm_UserID, mult_Sel_Val, MV_CLIENT_GRP="";
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
                frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xdaterange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
            }

            hfhcid.Value = frm_formID;

            if (!Page.IsPostBack)
            {
                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BRN||'~'||PRD AS PP FROM FIN_MSYS WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
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
        MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP");
        
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
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);// main line cmnt by some reason
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F47132":
                case "F47133":
                    SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V' AND type1 like '4%' AND TYPE1 NOT IN ('4F','47') ORDER BY code";
                    header_n = "Select Sale Type";
                    break;
                case "F47134":
                case "F47135":
                case "F47136":
                case "F47153":
                case "F47154":
                case "F47155":
                case "F47156":
                case "F47322":
                case "F77114":
                case "F77116":
                case "F77118":
                case "F77203":
                case "F77206":
                case "F77207":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F77109":
                case "F77112":
                    SQuery = "SELECT 'Mth'||trim(mthsno) as fstr, mthnum as code,mthname as name FROM MTHS order by mthsno";
                    header_n = "Select Month";
                    break;

                case "F47235"://SALE VS DEP REPROT FOR KPFL ...
                    SQuery = "SELECT trim(mthnum) as fstr, mthnum as code,mthname as name FROM MTHS order by mthsno";
                    header_n = "Select Month";
                    break;
            }
            if (SQuery.Length > 1)
            {

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F47235")
                {
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                }
                else
                {
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                }
            }
        }
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popupx  xxxxx
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            if (value1.Contains("Mth") && (val == "F77109" || val == "F77112"))
            {
                mult_Sel_Val = value1.Replace("','", "+");
                hmlt_mth.Value = mult_Sel_Val.Replace("'", "");
            }

                
            hfcode.Value = "";


            if (val == "F47132" || val == "F47133")
            {
                hf1.Value = value1;
                fgen.Fn_open_Act_itm_prd("-", frm_qstr);
            }
            //else
            //{
            //    fgen.Fn_open_prddmp1("-", frm_qstr);
            //}
            #region
            //THIS ELSE STATEMENT ADD BY YOGITA 
            else
            {
                switch (val)
                {
                    case "F47235"://sale vc desp report for KPFL...
                        if (Convert.ToInt32(value1) <= 3)
                        {
                            i1 = Convert.ToInt16(year) + 1;
                        }
                        else
                        {
                            i1 = Convert.ToInt16(year);
                        }
                        hf1.Value = value1 + "/" + i1;
                        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT mthname as name FROM MTHS WHERE MTHNUM='" + value1 + "'", "name");
                        SQuery = "select a.type1,trim(a.acode) as acode,b.aname,trim(a.icode) as icode,c.iname,c.cpartno,c.unit,SUM(a.Rday1+a.rday2+a.rday3+a.rday4+a.rday5+a.rday6+a.rday7+a.rday8+a.rday9+a.rday10+a.rday11+a.rday12+a.rday13+a.rday14+a.rday15+a.rday16+a.rday17+a.rday18+a.rday19+a.rday20+a.rday21+a.rday22+a.rday23+a.rday24+a.rday25+a.rday26+a.rday27+a.rday28+a.rday29+a.rday30+a.rday31) as total,  sum(A.Rday1) as day1,sum(A.Rday2) as day2,sum(A.Rday3) as day3,sum(A.Rday4) as day4,sum(A.Rday5) as day5,sum(A.Rday6) as day6,sum(A.Rday7) as day7,sum(A.Rday8) as day8,sum(A.Rday9) as day9, sum(A.Rday10) as day10,sum(A.Rday11) as day11,sum(A.Rday12) as day12,sum(A.Rday13) as day13,sum(A.Rday14) as day14,sum(A.Rday15) as day15,sum(A.Rday16) as day16,sum(A.Rday17) as day17,sum(A.Rday18) as day18,sum(A.Rday19) as day19,sum(A.Rday20) as day20,sum(A.Rday21) as day21,sum(A.Rday22) as day22,sum(A.Rday23) as day23,sum(A.Rday24) as day24,sum(A.Rday25) as day25,sum(A.Rday26) as day26,sum(A.Rday27) as day27,sum(A.Rday28) as day28,sum(A.Rday29) as day29,sum(A.Rday30) as day30,sum(A.Rday31) as day31  from  (SELECT acode,icode,'sale' as type1,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + mbr + "' and type like '4%' and to_Char(vchdate,'mm/yyyy')='" + hf1.Value + "' and nvl(iqtyout,0)>0 union all SELECT ACODE,icode,'Sch' as type1,(Case when to_char(DLV_DATE,'dd')='01' then BUDGETCOST else 0 end) as Rday1,(Case when to_char(DLV_DATE,'dd')='02' then BUDGETCOST else 0 end) as Rday2,(Case when to_char(DLV_DATE,'dd')='03' then BUDGETCOST else 0 end) as Rday3,(Case when to_char(DLV_DATE,'dd')='04' then BUDGETCOST else 0 end) as Rday4,(Case when to_char(DLV_DATE,'dd')='05' then BUDGETCOST else 0 end) as Rday5,(Case when to_char(DLV_DATE,'dd')='06' then BUDGETCOST else 0 end) as Rday6 ,(Case when to_char(DLV_DATE,'dd')='07' then BUDGETCOST else 0 end) as Rday7,(Case when to_char(DLV_DATE,'dd')='08' then BUDGETCOST else 0 end) as Rday8,(Case when to_char(DLV_DATE,'dd')='09' then BUDGETCOST else 0 end) as Rday9,(Case when to_char(DLV_DATE,'dd')='10' then BUDGETCOST else 0 end) as Rday10,(Case when to_char(DLV_DATE,'dd')='11' then BUDGETCOST else 0 end) as Rday11,(Case when to_char(DLV_DATE,'dd')='12' then BUDGETCOST else 0 end) as Rday12,(Case when to_char(DLV_DATE,'dd')='13' then BUDGETCOST else 0 end) as Rday13,(Case when to_char(DLV_DATE,'dd')='14' then BUDGETCOST else 0 end) as Rday14,(Case when to_char(DLV_DATE,'dd')='15' then BUDGETCOST else 0 end) as Rday15,(Case when to_char(DLV_DATE,'dd')='16' then BUDGETCOST else 0 end) as Rday16,(Case when to_char(DLV_DATE,'dd')='17' then BUDGETCOST else 0 end) as Rday17,(Case when to_char(DLV_DATE,'dd')='18' then BUDGETCOST else 0 end) as Rday18,(Case when to_char(DLV_DATE,'dd')='19' then BUDGETCOST else 0 end) as Rday19,(Case when to_char(DLV_DATE,'dd')='20' then BUDGETCOST  else 0 end) as Rday20,(Case when to_char(DLV_DATE,'dd')='21' then BUDGETCOST else 0 end) as Rday21,(Case when to_char(DLV_DATE,'dd')='22' then BUDGETCOST  else 0 end) as Rday22,(Case when to_char(DLV_DATE,'dd')='23' then BUDGETCOST else 0 end) as Rday23,(Case when to_char(DLV_DATE,'dd')='24' then BUDGETCOST  else 0 end) as Rday24,(Case when to_char(DLV_DATE,'dd')='25' then BUDGETCOST  else 0 end) as Rday25,(Case when to_char(DLV_DATE,'dd')='26' then BUDGETCOST else 0 end) as Rday26,(Case when to_char(DLV_DATE,'dd')='27' then BUDGETCOST else 0 end) as Rday27,(Case when to_char(DLV_DATE,'dd')='28'  then BUDGETCOST  else 0 end) as Rday28,(Case when to_char(DLV_DATE,'dd')='29'  then BUDGETCOST  else 0 end) as Rday29,(Case when to_char(DLV_DATE,'dd')='30'  then BUDGETCOST  else 0 end) as Rday30,(Case when to_char(DLV_DATE,'dd')='31'  then BUDGETCOST  else 0 end) as Rday31 from budgmst where branchcd='" + mbr + "' and type='46' and to_Char(DLV_DATE,'mm/yyyy')='" + hf1.Value + "'  and nvl(BUDGETCOST,0)>0 ) a,famst b ,item c where Trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  group by a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,a.type1 order by icode"; fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Sale Schedule Vs Despatch Report for Month '" + mq1 + " / " + i1 + "'", frm_qstr);
                        break;

                    default:
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;
                }
            }
            // ELSE STATEMENT IS ENDING HERE
            #endregion

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
            }
        }
        else
        {
            // ADD BY MADHVI FOR SHOWING THE DATE RANGE WHEN USER PRESS ESC 
            fgen.Fn_open_prddmp1("-", frm_qstr);
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        string party_cd = "";
        string part_cd = "";
        string db_fld = "";
        string my_rep_head="";
        val = hfhcid.Value.Trim();
        string ordfld = "";
        string numbr_fmt2 = "999,999,999,999";
        string br_cond = "";
        string br_check = "";
        br_check = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_pw where branchcd='" + mbr + "' and OPT_ID='W1100'", "fstr");
        br_cond = "branchcd='" + mbr + "'";
        if (br_check == "Y")
        {
            br_cond = "branchcd='00' and trim(mfginbr)='"+ mbr +"'";
        }
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

            frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");

            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            else branch_Cd = "branchcd='" + mbr + "'";

            tbl_flds = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                joinfld = tbl_flds.Split('@')[2].ToString();

                table1 = tbl_flds.Split('@')[3].ToString();
                table2 = tbl_flds.Split('@')[4].ToString();
                table3 = tbl_flds.Split('@')[5].ToString();
                table4 = tbl_flds.Split('@')[6].ToString();

                sortfld = sortfld.Replace("`", "'");
                joinfld = joinfld.Replace("`", "'");
                rep_flds = fgen.seek_iname(frm_qstr, frm_cocd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }
            string br_Count="";
            br_Count = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(trim(countrynm)) as cntry from type where id='B' and trim(type1)='" + mbr + "' ", "cntry");
            
            string sman_condi = "ent_by='" + uname + "'";
            if (ulvl.toDouble() <= 1)
            { sman_condi = "1=1"; }

            // after prdDmp this will run             
            string mq1 = "", mq2 = "", mq3 = "";
            switch (val)
            {
                case "F47132":
                    // hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TYPSTRING");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    hfcode.Value = hf1.Value;
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type like '4%' and a.type!='4F' and  a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    else
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.type!='4F' and  a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Master SO Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F47133":
                    //hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TYPSTRING");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    hfcode.Value = hf1.Value;
                    party_cd = ulvl == "M" ? uname : party_cd;
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type like '4%' and a.type!='4F' and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    else
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.type!='4F' and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supply SO Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F47134":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type='46' and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supply Schedule Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F47135":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = "Select b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by trim(a.acode),trim(A.icode) union all  Select trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + xprdrange + " group by trim(a.acode),trim(a.Icode)) a, famst b,item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by b.Aname,c.Iname,c.Unit,c.cpartno,trim(A.icode),trim(A.acode) Order by B.aname,c.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule Vs Despatch Data Search(Dom.) Summary for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F47136":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = "Select a.Ordno as SO_No,to_char(A.orddt,'dd/mm/yyyy') as SO_DT,b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number," +
                        "a.qtyord as Order_Qty,a.Soldqty as Despatch_Qty,a.bal_qty as Pend_Qty,c.Unit,round(a.bal_qty*a.srate,2) as Pend_Value,a.Pordno as Cust_po_no,to_char(nvl(a.PORDDT,sysdate),'dd/MM/YYYY') as Cust_PO_Dt,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode,to_chaR(a.orddt,'yyyymmdd') as VDD from wbvu_pending_so a, famst b,item c where a.branchcd='" + mbr + "' and a.orddt " + xprdrange + " and trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' and a.bal_qty>0 Order by VDD,a.ordno,B.aname,c.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Order Search(Dom.) Summary for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F47153":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "qtyord";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then " + db_fld + "  else 0 end) as mar  from sOMAS where branchcd='" + mbr + "' and type like '4%' and type!='4F' and ORDDT " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Order Qty Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F47154":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "(qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then " + db_fld + "  else 0 end) as mar  from sOMAS where branchcd='" + mbr + "' and type like '4%' and type!='4F' and ORDDT " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Order Value Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F47155":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total,0)";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(vchdate,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then " + db_fld + "  else 0 end) as mar  from schedule where branchcd='" + mbr + "' and type like '46%' and vchdate " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Schedule Qty Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F47156":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total*irate,0)";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(vchdate,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then " + db_fld + "  else 0 end) as mar  from schedule where branchcd='" + mbr + "' and type like '46%' and vchdate " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Schedule Value Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F77109":
                    value1 = value1.Replace("','", "+");

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total*irate,0)";

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");

                    SQuery = "select icode as Seg_Code,(" + hmlt_mth.Value + ") as seg_book,0 as ord_book,0 as sale_book from wb_budg_Ctrl where branchcd='" + mbr + "' and type='C1' and vchdate " + xprdrange + "  union all select substr(b.segname,1,3) as Seg_Name,0 as seg_book,a.irate*a.qtyord as ord_book,0 as sale_book from somas a,famst b where trim(A.acode)=trim(B.acode) and (a.branchcd='" + mbr + "' or trim(a.mfginbr)='" + mbr + "') and type like '4%' and a.orddt " + xprdrange + "   union all select substr(b.segname,1,3) as Seg_Name,0 as seg_book,0 as ord_book,a.iamount as sale_book from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + mbr + "' and type like '4%' and a.vchdate " + xprdrange + "";

                    SQuery = "select b.Name as Segment_Name,trim(a.Seg_Code) As Segment_Code,sum(a.seg_book) as Target_Value,sum(a.ord_book) as Orders_Booked,sum(a.sale_book) as Sales_Achieved from (" + SQuery + ")a,typegrp b where trim(b.id)='SM' and trim(A.seg_code)=trim(b.type1) group by b.Name,trim(a.Seg_Code) order by trim(a.Seg_Code)";


                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Segment Wise Target Vs Sales Order Vs Sales Invoice for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F77112":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total*irate,0)";

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");

                    SQuery = "select icode as Seg_Code,(" + hmlt_mth.Value + ") as seg_book,0 as ord_book,0 as sale_book from wb_budg_Ctrl where branchcd='" + mbr + "' and type='C2' and vchdate " + xprdrange + "  union all select substr(a.sale_rep,1,4) as Seg_Name,0 as seg_book,a.irate*a.qtyord as ord_book,0 as sale_book from somas a,famst b where trim(A.acode)=trim(B.acode) and (a.branchcd='" + mbr + "' or trim(a.mfginbr)='" + mbr + "') and type like '4%' and a.orddt " + xprdrange + "   union all select substr(a.sale_Rep,1,4) as Seg_Name,0 as seg_book,0 as ord_book,a.iamount as sale_book from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + mbr + "' and type like '4%' and a.vchdate " + xprdrange + "";
                    SQuery = "select b.Name as SalesPerson_Name,trim(a.Seg_Code) As Sale_Code,sum(a.seg_book) as Target_Value,sum(a.ord_book) as Orders_Booked,sum(a.sale_book) as Sales_Achieved from (" + SQuery + ")a,typegrp b where trim(b.id)='EM' and trim(A.seg_code)=trim(b.type1) group by b.Name,trim(a.Seg_Code) order by trim(a.Seg_Code)";


                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Person Wise Target Vs Sales Order Vs Sales Invoice for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F77114":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total*irate,0)";

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");

                    mq1 = "select icode as Seg_Code,Mth1,Mth2,Mth3,Mth4,Mth5,Mth6,Mth7,Mth8,Mth9,Mth10,Mth11,Mth12 ,0 as sMth1,0 as sMth2,0 as sMth3,0 as sMth4,0 as sMth5,0 as sMth6,0 as sMth7,0 as sMth8,0 as sMth9,0 as sMth10,0 as sMth11,0 as sMth12 from wb_budg_Ctrl where branchcd='" + mbr + "' and type='C2' and vchdate " + xprdrange + "  union all ";
                    mq2 = "select substr(sale_Rep,1,4) as Seg_Code,0 as Mth1,0 as Mth2,0 as Mth3,0 as Mth4,0 as Mth5,0 as Mth6,0 as Mth7,0 as Mth8,0 as Mth9,0 as Mth10,0 as Mth11,0 as Mth12 ,(case when to_Char(vchdate,'mm')='01' then iamount else 0 end) as sMth1,(case when to_Char(vchdate,'mm')='02' then iamount else 0 end) as sMth2,(case when to_Char(vchdate,'mm')='03' then iamount else 0 end) as sMth3,(case when to_Char(vchdate,'mm')='04' then iamount else 0 end) as sMth4,(case when to_Char(vchdate,'mm')='05' then iamount else 0 end) as sMth5,(case when to_Char(vchdate,'mm')='06' then iamount else 0 end) as sMth6,(case when to_Char(vchdate,'mm')='07' then iamount else 0 end) as sMth7,(case when to_Char(vchdate,'mm')='08' then iamount else 0 end) as sMth8,(case when to_Char(vchdate,'mm')='09' then iamount else 0 end) as sMth9,(case when to_Char(vchdate,'mm')='10' then iamount else 0 end) as sMth10,(case when to_Char(vchdate,'mm')='11' then iamount else 0 end) as sMth11,(case when to_Char(vchdate,'mm')='12' then iamount else 0 end) as sMth12 from ivoucher where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + "  ";
                    SQuery = "select b.Name as SalesPerson_Name,trim(a.Seg_Code) As Sale_Code,sum(Mth1)+sum(Mth2)+sum(Mth3)+sum(Mth4)+sum(Mth5)+sum(Mth6)+sum(Mth7)+sum(Mth8)+sum(Mth9)+sum(Mth10)+sum(Mth11)+sum(Mth12) as Total_Target,sum(sMth1)+sum(sMth2)+sum(sMth3)+sum(sMth4)+sum(sMth5)+sum(sMth6)+sum(sMth7)+sum(sMth8)+sum(sMth9)+sum(sMth10)+sum(sMth11)+sum(sMth12) as Total_Sales,(sum(Mth1)+sum(Mth2)+sum(Mth3)+sum(Mth4)+sum(Mth5)+sum(Mth6)+sum(Mth7)+sum(Mth8)+sum(Mth9)+sum(Mth10)+sum(Mth11)+sum(Mth12))-(sum(sMth1)+sum(sMth2)+sum(sMth3)+sum(sMth4)+sum(sMth5)+sum(sMth6)+sum(sMth7)+sum(sMth8)+sum(sMth9)+sum(sMth10)+sum(sMth11)+sum(sMth12)) as Difference,sum(Mth1) As Tgt_mth1,sum(sMth1) As Act_mth1,sum(Mth2) As Tgt_mth2,sum(sMth2) As Act_mth2,sum(Mth3) As Tgt_mth3,sum(sMth3) As Act_mth3,sum(Mth4) As Tgt_mth4,sum(sMth4) As Act_mth4,sum(Mth5) As Tgt_mth5,sum(sMth5) As Act_mth5,sum(Mth6) As Tgt_mth6,sum(sMth6) As Act_mth6,sum(Mth7) As Tgt_mth7,sum(sMth7) As Act_mth7,sum(Mth8) As Tgt_mth8,sum(sMth8) As Act_mth8,sum(Mth9) As Tgt_mth9,sum(sMth9) As Act_mth9,sum(Mth10) As Tgt_mth10,sum(sMth10) As Act_mth10,sum(Mth11) As Tgt_mth11,sum(sMth11) As Act_mth11,sum(Mth12) As Tgt_mth12,sum(sMth12) As Act_mth12 from (" + mq1 + mq2 + ")a,typegrp b where trim(b.id)='EM' and trim(A.seg_code)=trim(b.type1) group by b.Name,trim(a.Seg_Code) order by trim(a.Seg_Code)";

                    my_rep_head = "Sales Person Wise Target Vs Sales Order Vs Sales Invoice for the Period " + value1 + " to " + value2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#9#10#11#12#13#14#15#16#17#18#19#20#21#22#23#24#25#26#27#28#29#30#31#32", "3#4#5#6#7#8#9#10#11#12#13#14#15#16#17#18#19#20#21#22#23#24#25#26#27#28#29#30#31#32", "200#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100#100");
                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;
                case "F77203":
                    SQuery = "select Sman_Name,nvl(day1,0)+nvl(day2,0)+nvl(day3,0)+nvl(day4,0)+nvl(day5,0)+nvl(day6,0)+nvl(day7,0)+nvl(day8,0)+nvl(day9,0)+nvl(day10,0)+nvl(day11,0)+nvl(day12,0)+nvl(day13,0)+nvl(day14,0)+nvl(day15,0)+nvl(day16,0)+nvl(day17,0)+nvl(day18,0)+nvl(day19,0)+nvl(day20,0)+nvl(day21,0)+nvl(day22,0)+nvl(day23,0)+nvl(day24,0)+nvl(day25,0)+nvl(day26,0)+nvl(day27,0)+nvl(day28,0)+nvl(day29,0)+nvl(day30,0)+nvl(day31,0) Total_CSS,day1,day2,day3,day4,day5,day6,day7,day8,day9,day10,day11,day12,day13,day14,day15,day16,day17,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 from (WITH pivot_data AS (SELECT to_Char(a.sdwdt,'dd') as  Mth_no, trim(a.ent_by) as Sman_Name, count(a.sdwno)  as sal FROM wb_sman_log a where a.branchcd='" + mbr + "' and a.sdwdt " + xprdrange + " and " + sman_condi + " group by to_Char(a.sdwdt,'dd'),trim(a.ent_by) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('01' as Day1,'02' as Day2,'03' as Day3,'04' as Day4,'05' as Day5,'06' as Day6,'07' as Day7,'08' as Day8,'09' as Day9,'10' as Day10,'11' as Day11,'12' as Day12,'13' as Day13,'14' as Day14,'15' as Day15,'16' as Day16,'17' as Day17,'18' as Day18,'19' as Day19,'20' as Day20,'21' as Day21,'22' as Day22,'23' as Day23,'24' as Day24,'25' as Day25,'26' as Day26,'27' as Day27,'28' as Day28,'29' as Day29,'30' as Day30,'31' as Day31 ))) order by Sman_Name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Day Wise Sales Person Wise Reporting during " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F77206":
                    if (br_Count=="INDIA")
                    {
                        SQuery = "select Sman_Name,nvl(Apr,0)+nvl(May,0)+nvl(Jun,0)+nvl(Jul,0)+nvl(Aug,0)+nvl(Sep,0)+nvl(Oct,0)+nvl(Nov,0)+nvl(Dec,0)+nvl(Jan,0)+nvl(Feb,0)+nvl(Mar,0) as Total_SDW,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,Jan,Feb,Mar from (WITH pivot_data AS (SELECT to_Char(a.sdwdt,'mm') as  Mth_no, trim(a.ent_by) as Sman_Name, sum(a.EXPECT_VAL)  as sal FROM wb_sman_log a where a.branchcd='" + mbr + "' and a.sdwdt " + xprdrange + " and " + sman_condi + " group by to_Char(a.sdwdt,'mm'),trim(a.ent_by) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('04' as Apr,'05' as May,'06' as Jun,'07' as Jul,'08' as Aug,'09' as Sep,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))) order by Sman_Name";
                    }
                    else
                    {
                        SQuery = "select Sman_Name,nvl(Apr,0)+nvl(May,0)+nvl(Jun,0)+nvl(Jul,0)+nvl(Aug,0)+nvl(Sep,0)+nvl(Oct,0)+nvl(Nov,0)+nvl(Dec,0)+nvl(Jan,0)+nvl(Feb,0)+nvl(Mar,0) as Total_SDW,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec from (WITH pivot_data AS (SELECT to_Char(a.sdwdt,'mm') as  Mth_no, trim(a.ent_by) as Sman_Name, sum(a.EXPECT_VAL)  as sal FROM wb_sman_log a where a.branchcd='" + mbr + "' and a.sdwdt " + xprdrange + " and " + sman_condi + "group by to_Char(a.sdwdt,'mm'),trim(a.ent_by) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('04' as Apr,'05' as May,'06' as Jun,'07' as Jul,'08' as Aug,'09' as Sep,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))) order by Sman_Name";
                    }
                    
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Month Wise Sales Person wise Reporting during " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F77207":
                    SQuery = "select SDWNO as Doc_No, to_char(SDWDT,'dd/mm/yyyy') as Doc_Dt, CL_SRC as Lead_Source, CL_VERT as Lead_Industry, CL_CATG as Lead_Category, CL_INTEREST as Interesed_In, CL_CONAME as Company_Name, CL_PERSON as Person_name, CL_PHONE as Phone_No, CL_EMAIL as Email_ID, CL_DESIG as Designation, CREMARKS as Lead_Remarks, OREMARKS as Our_Remarks, EXPECT_VAL as Approx_value, EXPENSE_VAL as Exp_done,to_char(SDWdt,'yyyymmdd') as vdd,ent_by,ent_Dt from WB_SMAN_LOG where branchcd='" + mbr + "' and SDWDT " + xprdrange + " and " + sman_condi+" order by vdd, SDWNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Sales Person wise Visit Reporting during " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F77118":
                    if (br_Count == "INDIA")
                    {
                        SQuery = "select Sman_Name,nvl(Apr,0)+nvl(May,0)+nvl(Jun,0)+nvl(Jul,0)+nvl(Aug,0)+nvl(Sep,0)+nvl(Oct,0)+nvl(Nov,0)+nvl(Dec,0)+nvl(Jan,0)+nvl(Feb,0)+nvl(Mar,0) as Total_Exp,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec,Jan,Feb,Mar from (WITH pivot_data AS (SELECT to_Char(a.sdwdt,'mm') as  Mth_no, trim(a.ent_by) as Sman_Name, sum(a.expense_Val)  as sal FROM wb_sman_log a where a.branchcd='" + mbr + "' and a.sdwdt " + xprdrange + "  group by to_Char(a.sdwdt,'mm'),trim(a.ent_by) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('04' as Apr,'05' as May,'06' as Jun,'07' as Jul,'08' as Aug,'09' as Sep,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))) order by Sman_Name";
                    }
                    else
                    {
                        SQuery = "select Sman_Name,nvl(Apr,0)+nvl(May,0)+nvl(Jun,0)+nvl(Jul,0)+nvl(Aug,0)+nvl(Sep,0)+nvl(Oct,0)+nvl(Nov,0)+nvl(Dec,0)+nvl(Jan,0)+nvl(Feb,0)+nvl(Mar,0) as Total_Exp,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec from (WITH pivot_data AS (SELECT to_Char(a.sdwdt,'mm') as  Mth_no, trim(a.ent_by) as Sman_Name, sum(a.expense_Val)  as sal FROM wb_sman_log a where a.branchcd='" + mbr + "' and a.sdwdt " + xprdrange + " group by to_Char(a.sdwdt,'mm'),trim(a.ent_by) )SELECT * From pivot_data PIVOT ( max(sal) FOR Mth_no IN  ('04' as Apr,'05' as May,'06' as Jun,'07' as Jul,'08' as Aug,'09' as Sep,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))) order by Sman_Name";
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Showing Month Wise Sales Person wise Expense during " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F77116":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total*irate,0)";

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    ordfld = "round(QTYORD*(irate*((100-cdisc)/100))*decode(CURR_RATE,0,1,curr_Rate),0)";
                    
                    
                    if (br_cond=="Y")
                    {

                    }
                    mq1 = "Select upper(trim(sale_rep)) as Mktg_person,(iamount) as today,0 as mtd,0 as ytd,branchcd,0 as tdtord,0 as mtdord,0 as ytdord from ivoucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and vchdate=to_DatE('" + value2 + "','dd/mm/yyyy') union all Select upper(trim(sale_Rep)),0 as today,(iamount) as mtd,0 as ytd,branchcd,0 as tdtord,0 as mtdord,0 as ytdord  from ivoucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and  to_char(vchdate,'yyyymm')=to_char(to_Date('" + value2 + "','dd/mm/yyyy'),'yyyymm') union all Select upper(trim(sale_rep)),0 as today,0 as mtd,(iamount) as ytd,branchcd,0 as tdtord,0 as mtdord,0 as ytdord  from ivoucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and  vchdate " + xdaterange + " and vchdate<=to_DatE('" + value2 + "','dd/mm/yyyy') union all ";
                    mq2 = " Select upper(trim(sale_rep)) as Mktg_person,0 as today,0 as mtd,0 as ytd,branchcd,(" + ordfld + ") as tdtord,0 as mtdord,0 as ytdord from somas where (" + br_cond + ") and type like '4%' and type!='47' and orddt=to_DatE('" + value2 + "','dd/mm/yyyy') union all Select upper(trim(sale_rep)),0 as today,0 as mtd,0 as ytd,branchcd,0 as tdtord,(" + ordfld + ") as mtdord,0 as ytdord  from somas where (" + br_cond + ") and type like '4%' and type!='47' and  to_char(orddt,'yyyymm')=to_char(to_Date('" + value2 + "','dd/mm/yyyy'),'yyyymm') union all Select upper(trim(sale_Rep)),0 as today,0 as mtd,0 as ytd,branchcd,0 as tdtord,0 as mtdord,(" + ordfld + ") as ytdord  from somas where (" + br_cond + ") and type like '4%' and type!='47' and  orddt " + xdaterange + " and orddt<=to_DatE('" + value2 + "','dd/mm/yyyy') ";

                    SQuery = "select a.Mktg_person,to_char(sum(today),'" + numbr_fmt2 + "') as Today_Sale,to_char(sum(mtd),'" + numbr_fmt2 + "') as MTD_Sale,to_char(sum(ytd),'" + numbr_fmt2 + "') as Ytd_Sale,to_char(sum(tdtord),'" + numbr_fmt2 + "') as Today_ord,to_char(sum(mtdord),'" + numbr_fmt2 + "') as MTD_ord,to_char(sum(ytdord),'" + numbr_fmt2 + "') as Ytd_ord from (" + mq1 + mq2 + ")a group by a.Mktg_person order by a.Mktg_person";

                    my_rep_head = "Sales Person Wise Day/Month/Year Report for the Period " + value1 + " to " + value2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#9#10", "3#4#5#6#7#8#9#10", "250#150#150#150#150#150#150#150");
                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;

                
                case "F47239":
                    cond = ulvl == "M" ? "and trim(a.acode) like '" + uname + "'" : "";
                    SQuery = "select a.vchnum as inv_no,to_char(a.vchdate,'dd/mm/yyyy') as inv_dt,b.aname as customer,a.icode as code,c.cpartno as part_no,a.purpose as part_name,a.iqtyout as qty_sold,a.irate,a.exc_rate as exc_rate,a.exc_amt as excise,a.cess_pu as cess,a.she_cess,a.iamount as basic,a.finvno as po_ref,a.binno as ref_fld,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_Dt,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.no_cases as tarr_no  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.type like '4%' and a." + branch_Cd + " and a.vchdate " + xprdrange + " " + cond + " order by a.vchnum desc,to_char(a.vchdate,'dd/mm/yyyy') desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Invoice List for the period for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F47322":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq0 = "SELECT * FROM(SELECT DISTINCT TYPE,TRIM(PORDNO) AS PORDNO,TRIM(ACODE) AS ACODE,TRIM(ICODE) AS ICODE FROM WB_SORFQ WHERE BRANCHCD='" + mbr + "' AND  TYPE='RF' AND trim(ACODE) LIKE '" + party_cd + "%' UNION ALL SELECT DISTINCT TYPE,TRIM(PORDNO) AS PORDNO,TRIM(ACODE) AS ACODE,TRIM(ICODE) AS ICODE FROM WB_SORFQ WHERE BRANCHCD='" + mbr + "' AND TYPE='MC' AND trim(ACODE) LIKE '" + party_cd + "%' UNION ALL SELECT DISTINCT TYPE,TRIM(PBASIS) AS PBASIS,TRIM(ACODE) AS ACODE,TRIM(ICODE) AS ICODE FROM WB_CACOST WHERE BRANCHCD='" + mbr + "' AND TYPE='CA01' AND trim(ACODE) LIKE '" + party_cd + "%' UNION ALL SELECT DISTINCT TYPE,TRIM(DESP_TO) AS DESP_TO,TRIM(ACODE) AS ACODE,TRIM(ICODE) AS ICODE FROM SOMASQ WHERE BRANCHCD='" + mbr + "' AND TYPE='FQ' AND trim(ACODE) LIKE '" + party_cd + "%')";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    // SQuery = "SELECT DISTINCT BRANCHCD||TYPE||TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY') AS FSTR, DECODE(A.Type,'ER','Enquiry Entry','EC','ECN Entry') as Entry_Name ,TRIM(A.ORDNO) AS ENQ_ECN_NO, TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ENQ_ECN_DATE, TRIM(A.ACODE) AS CUSTOMER_CODE, TRIM(B.ANAME) AS CUSTOMER_NAME,TRIM(A.ICODE) AS ITEM_CODE,TRIM(C.ICODE) AS ITEM_NAME, A.TEST AS STATUS ,DECODE(A.TEST,'Q','FINAL QUOTATION DONE','C','COSTING DONE','M','MACHINING SHOP DONE','F','FOUNDARY DONE','A','QUOTATION APPROVED') AS FINAL_STATUS FROM WB_SORFQ A,FAMST B ,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND  A.TYPE IN ('ER','EC') AND a.ORDDT " + xprdrange + " AND trim(A.ACODE) LIKE '" + party_cd + "%' ORDER BY ENQ_ECN_NO";
                    SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR, DECODE(A.Type,'ER','Enquiry Entry','EC','ECN Entry') as Entry_Name ,TRIM(A.ORDNO) AS ENQ_ECN_NO, TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ENQ_ECN_DATE, TRIM(A.ACODE) AS CUSTOMER_CODE, TRIM(B.ANAME) AS CUSTOMER_NAME,TRIM(A.ICODE) AS ITEM_CODE,TRIM(C.INAME) AS ITEM_NAME FROM WB_SORFQ A,FAMST B ,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND  A.TYPE IN ('ER','EC') AND a.ORDDT " + xprdrange + " AND trim(A.ACODE) LIKE '" + party_cd + "%' ORDER BY ENQ_ECN_NO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.Columns.Add("RESPOND_FOUNDRY", typeof(string));
                    dt.Columns.Add("MC_SHOP_FOUNDRY", typeof(string));
                    dt.Columns.Add("COSTING_ENTRY", typeof(string));
                    dt.Columns.Add("QUOTATION_ENTRY", typeof(string));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dticode = new DataTable();
                        if (dt3.Rows.Count > 0)
                        {
                            dv = new DataView(dt3, "PORDNO='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = dv.ToTable();
                        }
                        for (int k = 0; k < dticode.Rows.Count; k++)
                        {
                            mq1 = dticode.Rows[k]["type"].ToString();
                            switch (mq1)
                            {
                                case "RF":
                                    dt.Rows[i]["RESPOND_FOUNDRY"] = "DONE";
                                    break;

                                case "MC":
                                    dt.Rows[i]["MC_SHOP_FOUNDRY"] = "DONE";
                                    break;

                                case "CA01":
                                    dt.Rows[i]["COSTING_ENTRY"] = "DONE";
                                    break;

                                case "FQ":
                                    dt.Rows[i]["QUOTATION_ENTRY"] = "DONE";
                                    break;
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("fstr");
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Status Report For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    break;
                case "F47118R":
                    DataTable dtm = new DataTable();
                    dtm.Columns.Add("SNo", typeof(string));
                    dtm.Columns.Add("State", typeof(string));
                    dtm.Columns.Add("VB_Name", typeof(string));
                    dtm.Columns.Add("InvNo_VB_To_SAPL", typeof(string));
                    dtm.Columns.Add("INV_DATE", typeof(string));
                    dtm.Columns.Add("Basic_Val", typeof(double));
                    dtm.Columns.Add("GST_Val", typeof(double));
                    dtm.Columns.Add("Tot_Inv_Val", typeof(double));
                    dtm.Columns.Add("CD@2%_On_Basic_Inv_Value", typeof(double));
                    dtm.Columns.Add("Amount_Payable_to_VB_After_Less_CD", typeof(double));
                    dtm.Columns.Add("Paid_Amount", typeof(double));
                    dtm.Columns.Add("Balance", typeof(double));
                    dtm.Columns.Add("Payable_To_VB-Within_15_Days_From_Inv_Date", typeof(double));
                    dtm.Columns.Add("Actual_Payment_Date_To_VB", typeof(double));
                    dtm.Columns.Add("Aging", typeof(double));
                    dtm.Columns.Add("Sub-Dealer_Retailer", typeof(string));
                    dtm.Columns.Add("Destination", typeof(string));
                    dtm.Columns.Add("Finsys_InvNo", typeof(string));
                    dtm.Columns.Add("Inv_Date", typeof(string));
                    dtm.Columns.Add("Basic_Inv_Val", typeof(double));
                    dtm.Columns.Add("GST", typeof(double));
                    dtm.Columns.Add("Tot_Invoice_Val", typeof(double));
                    dtm.Columns.Add("Payment_Due_Receivable_7Days", typeof(string));
                    dtm.Columns.Add("CD@3%_If_Payment_Within_7Days", typeof(double));
                    dtm.Columns.Add("CD_Calculate_Amt", typeof(double));
                    dtm.Columns.Add("Payment_Due_Receivable_As_Per_CD", typeof(double));
                    dtm.Columns.Add("Payment_Due_Receivable_Without_CD", typeof(double));
                    dtm.Columns.Add("Payment_Received", typeof(double));
                    dtm.Columns.Add("Balance_Payment", typeof(double));
                    dtm.Columns.Add("Payment_Receipt_Date", typeof(string));
                    dtm.Columns.Add("Aging_Payment_Receipt(Due-Actual)", typeof(double));
                    dtm.Columns.Add("Status_For_Payment_Closure_From_Retailer/SubDealer_To_SAPL", typeof(double));
                    dtm.Columns.Add("Margin", typeof(double));
                    dtm.Columns.Add("Freight_Charges", typeof(double));

                    mq0 = "select a.branchcd,a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,sum(a.cgst)+sum(a.sgst) as gst,sum(basic) as basic,trim(a.acode) as acode,f.aname,f.staten,to_char(vchdate,'yyyymmdd') as vdd from (select branchcd,type,vchnum,vchdate,exc_amt as cgst,cess_pu as sgst,iamount as basic,acode from ivoucher where branchcd!='DD' and branchcd>40 and type='02' and vchdate " + xprdrange + " union all select branchcd,type,vchnum,vchdate,amt_exc as cgst,rvalue as sgst,amt_sale as basic,acode from sale where branchcd!='DD' and branchcd>40 and type like '4%' and vchdate " + xprdrange + ")a,famst f where trim(A.acode)=trim(f.acode) group by a.branchcd,a.type,trim(a.vchnum),to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),f.aname,f.staten,to_char(vchdate,'yyyymmdd') order by branchcd,vdd,vchnum";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    mq1 = "select branchcd,type,trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,trim(acode) as acode,sum(cramt) as cramt,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd from voucher where branchcd!='88' and branchcd>40 and (substr(type,1,1)='1' or type='30') and invdate " + xprdrange + " and acode like '16%' group by branchcd,type,trim(invno),to_char(invdate,'dd/mm/yyyy'),trim(acode),to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') order by branchcd,invno,invdate,vdd";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                    int count = 1;
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt);
                        dticode = new DataTable();
                        dticode = view1.ToTable(true, "branchcd", "vchnum", "vchdate");
                        foreach (DataRow dr in dticode.Rows)
                        {
                            DataView view2 = new DataView(dt, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and vchnum='" + dr["vchnum"].ToString().Trim() + "' and vchdate='" + dr["vchdate"].ToString() + "'", "", DataViewRowState.CurrentRows);
                            dticode2 = new DataTable();
                            dticode2 = view2.ToTable();
                            oporow = dtm.NewRow();
                            for (int i = 0; i < dticode2.Rows.Count; i++)
                            {
                                oporow["SNo"] = count;
                                if (dticode2.Rows[i]["type"].ToString().Trim() == "02")
                                {
                                    oporow["State"] = dticode2.Rows[i]["staten"].ToString().Trim();
                                    oporow["VB_Name"] = dticode2.Rows[i]["aname"].ToString().Trim();
                                    oporow["InvNo_VB_To_SAPL"] = dticode2.Rows[i]["vchnum"].ToString().Trim();
                                    oporow["INV_DATE"] = dticode2.Rows[i]["vchdate"].ToString().Trim();
                                    oporow["BASIC_VAL"] = fgen.make_double(dticode2.Rows[i]["basic"].ToString().Trim());
                                    oporow["GST_VAL"] = fgen.make_double(dticode2.Rows[i]["gst"].ToString().Trim());
                                    oporow["TOT_INV_VAL"] = Math.Round(fgen.make_double(dticode2.Rows[i]["basic"].ToString().Trim()) + fgen.make_double(dticode2.Rows[i]["gst"].ToString().Trim()), 2);
                                    oporow["CD@2%_On_Basic_Inv_Value"] = Math.Round((fgen.make_double(oporow["BASIC_VAL"].ToString()) * 2) / 100, 2);
                                    oporow["Amount_Payable_to_VB_After_Less_CD"] = Math.Round(fgen.make_double(oporow["TOT_INV_VAL"].ToString()) - fgen.make_double(oporow["CD@2%_On_Basic_Inv_Value"].ToString()), 2);
                                    oporow["Balance"] = fgen.make_double(oporow["Amount_Payable_to_VB_After_Less_CD"].ToString());
                                }
                                else if (dticode2.Rows[i]["type"].ToString().Trim().Substring(0, 1) == "4")
                                {
                                    oporow["Sub-Dealer_Retailer"] = dticode2.Rows[i]["aname"].ToString().Trim();
                                    oporow["Destination"] = dticode2.Rows[i]["staten"].ToString().Trim();
                                    oporow["Finsys_InvNo"] = dticode2.Rows[i]["vchnum"].ToString().Trim();
                                    oporow["Inv_Date"] = dticode2.Rows[i]["vchdate"].ToString().Trim();
                                    oporow["Basic_Inv_Val"] = fgen.make_double(dticode2.Rows[i]["basic"].ToString().Trim());
                                    oporow["GST"] = fgen.make_double(dticode2.Rows[i]["gst"].ToString().Trim());
                                    oporow["Tot_Invoice_Val"] = Math.Round(fgen.make_double(dticode2.Rows[i]["basic"].ToString().Trim()) + fgen.make_double(dticode2.Rows[i]["gst"].ToString().Trim()), 2);
                                    oporow["Payment_Due_Receivable_7Days"] = (Convert.ToDateTime(dticode2.Rows[i]["vchdate"].ToString().Trim()).AddDays(7)).ToString("dd/MM/yyyy");
                                    oporow["CD@3%_If_Payment_Within_7Days"] = 3;
                                    oporow["CD_Calculate_Amt"] = Math.Round((fgen.make_double(dticode2.Rows[i]["basic"].ToString().Trim()) * fgen.make_double(oporow["CD@3%_If_Payment_Within_7Days"].ToString())) / 100, 2);
                                    oporow["Payment_Due_Receivable_As_Per_CD"] = Math.Round(fgen.make_double(oporow["Tot_Invoice_Val"].ToString()) - fgen.make_double(oporow["CD_Calculate_Amt"].ToString()), 2);
                                    oporow["Payment_Due_Receivable_Without_CD"] = fgen.make_double(oporow["Tot_Invoice_Val"].ToString());
                                }
                                oporow["Payment_Received"] = fgen.make_double(fgen.seek_iname_dt(dt1, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and invno='" + dr["vchnum"].ToString().Trim() + "' and invdate='" + dr["vchdate"].ToString() + "'", "cramt"));
                                er1 = fgen.seek_iname_dt(dt1, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and invno='" + dr["vchnum"].ToString().Trim() + "' and invdate='" + dr["vchdate"].ToString() + "'", "vchdate");
                                if (er1.Length == 1)
                                {
                                    oporow["Payment_Receipt_Date"] = er1.Replace("0", "-");
                                }
                                else
                                {
                                    oporow["Payment_Receipt_Date"] = er1;
                                }
                            }
                            count++;
                            dtm.Rows.Add(oporow);
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dtm;
                    fgen.Fn_open_rptlevel("Profitability Report For The Period " + fromdt + " To " + todt, frm_qstr);
                    break;
            }
        }
    }
}