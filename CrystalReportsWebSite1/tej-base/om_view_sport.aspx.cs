using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_cport7 : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = ""; int li = 0;
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID;
    string party_cd = "", part_cd = "";
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
                case "F78145":
                    SQuery = "";
                    fgen.Fn_open_prddmp1("", frm_qstr);
                    break;
                case "F78126":
                case "F78127":
                    SQuery = "select mthnum as fstr,mthnum,mthname  from mths order by mthnum";
                    header_n = "Select Month";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F79141" || HCID == "F79142")
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
            else if (fgenMV.Fn_Get_Mvar(frm_qstr, "ANP").ToString().Trim() == "Y")
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                switch (val)
                {
                    case "F78145":
                        if (col1.Length < 2) return;
                        hfcode.Value = col1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;
                }
            }
            #region
            //THIS ELSE STATEMENT ADD BY YOGITA 
            else
            {
                switch (val)
                {
                    case "F78126"://done
                        #region P.O. Dt Vs Rcpt Dt.(Portal)
                        //uname = "06A129";
                        mq1 = value1;
                        li = 0;
                        if (Convert.ToInt32(mq1) < 3)
                        {
                            li = Convert.ToInt32(year) + 1;
                        }
                        else
                        {
                            li = Convert.ToInt32(year);
                        }
                        mq2 = value1 + "/" + li;
                        //SQuery = "select a.fstr,a.header,a.acode,b.aname as supplier_name,a.icode,c.iname as item_name ,a.po_date1 as day1,a.po_date2 as day2,a.po_date3 as day3,a.po_date4 as day4,a.po_date5 as day5,a.po_date6  as day6,a.po_date7 as day7,a.po_date8 as day8,a.po_date9 as day9,a.po_date10 as day10,a.po_date11 as day11,a.po_date12 as day12,a.po_date13 as day13,a.po_date14 as day14,a.po_date15 as day15,a.po_date16 as day16,a.po_date17 as day17,a.po_date18 as day18,a.po_date19 as day19,a.po_date20 as day20,a.po_date21 as day21,a.po_date22 as day22,a.po_date23 as day23,a.po_date24 as day24,a.po_date25 as day25,a.po_date26 as day26,a.po_date27 as day27,a.po_date28 as day28,a.po_date29 as day29,a.po_date30 as day30,a.po_date31 as day31  from (SELECT 'Po Dt.' as header,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(orddt,'yyyymmdd') as fstr,(Case when to_char(orddt,'dd')='01' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(orddt,'dd')='02' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(orddt,'dd')='03' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(orddt,'dd')='04' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(orddt,'dd')='05' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(orddt,'dd')='06' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(orddt,'dd')='07' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(orddt,'dd')='08' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(orddt,'dd')='09' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(orddt,'dd')='10' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(orddt,'dd')='11' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(orddt,'dd')='12' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(orddt,'dd')='13' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(orddt,'dd')='14' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(orddt,'dd')='15' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(orddt,'dd')='16' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(orddt,'dd')='17' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(orddt,'dd')='18' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(orddt,'dd')='19' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(orddt,'dd')='20' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(orddt,'dd')='21' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(orddt,'dd')='22' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(orddt,'dd')='23' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(orddt,'dd')='24' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(orddt,'dd')='25' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(orddt,'dd')='26' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(orddt,'dd')='27' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(orddt,'dd')='28' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(orddt,'dd')='29' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(orddt,'dd')='30' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(orddt,'dd')='31' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date31 from pomas where branchcd!='DD' and type like '5%' and to_char(orddt,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "' union all SELECT 'Rcpt Dt.' as header,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from ivoucher where branchcd!='DD' and type like '2%' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "') a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by a.header,a.acode";  //from pomas and ivch table                        
                        //SQuery = "select a.fstr,a.header,a.branchcd,d.name as branch_name,a.acode,b.aname as supplier_name,a.icode,c.iname as item_name ,a.po_date1 as day1,a.po_date2 as day2,a.po_date3 as day3,a.po_date4 as day4,a.po_date5 as day5,a.po_date6  as day6,a.po_date7 as day7,a.po_date8 as day8,a.po_date9 as day9,a.po_date10 as day10,a.po_date11 as day11,a.po_date12 as day12,a.po_date13 as day13,a.po_date14 as day14,a.po_date15 as day15,a.po_date16 as day16,a.po_date17 as day17,a.po_date18 as day18,a.po_date19 as day19,a.po_date20 as day20,a.po_date21 as day21,a.po_date22 as day22,a.po_date23 as day23,a.po_date24 as day24,a.po_date25 as day25,a.po_date26 as day26,a.po_date27 as day27,a.po_date28 as day28,a.po_date29 as day29,a.po_date30 as day30,a.po_date31 as day31  from (SELECT 'Po Dt.' as header,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(orddt,'yyyymmdd') as fstr,(Case when to_char(orddt,'dd')='01' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(orddt,'dd')='02' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(orddt,'dd')='03' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(orddt,'dd')='04' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(orddt,'dd')='05' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(orddt,'dd')='06' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(orddt,'dd')='07' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(orddt,'dd')='08' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(orddt,'dd')='09' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(orddt,'dd')='10' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(orddt,'dd')='11' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(orddt,'dd')='12' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(orddt,'dd')='13' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(orddt,'dd')='14' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(orddt,'dd')='15' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(orddt,'dd')='16' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(orddt,'dd')='17' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(orddt,'dd')='18' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(orddt,'dd')='19' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(orddt,'dd')='20' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(orddt,'dd')='21' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(orddt,'dd')='22' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(orddt,'dd')='23' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(orddt,'dd')='24' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(orddt,'dd')='25' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(orddt,'dd')='26' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(orddt,'dd')='27' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(orddt,'dd')='28' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(orddt,'dd')='29' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(orddt,'dd')='30' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(orddt,'dd')='31' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date31 from pomas where branchcd!='DD' and type like '5%' and to_char(orddt,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "' union all SELECT 'Rcpt Dt.' as header,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from ivoucher where branchcd!='DD' and type like '2%' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "') a,famst b,item c,type d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(d.type1) and d.id='B' order by a.header,a.branchcd,a.acode";  //from pomas and ivch table                           ..............old
                        //  SQuery = "select a.fstr,a.header,a.branchcd,d.name as branch_name,a.ordno,a.orddt,a.wono,a.acode,b.aname as supplier_name,a.icode,c.iname as item_name ,a.po_date1 as day1,a.po_date2 as day2,a.po_date3 as day3,a.po_date4 as day4,a.po_date5 as day5,a.po_date6  as day6,a.po_date7 as day7,a.po_date8 as day8,a.po_date9 as day9,a.po_date10 as day10,a.po_date11 as day11,a.po_date12 as day12,a.po_date13 as day13,a.po_date14 as day14,a.po_date15 as day15,a.po_date16 as day16,a.po_date17 as day17,a.po_date18 as day18,a.po_date19 as day19,a.po_date20 as day20,a.po_date21 as day21,a.po_date22 as day22,a.po_date23 as day23,a.po_date24 as day24,a.po_date25 as day25,a.po_date26 as day26,a.po_date27 as day27,a.po_date28 as day28,a.po_date29 as day29,a.po_date30 as day30,a.po_date31 as day31  from (SELECT 'Po Dt.' as header,pordno as wono,ordno,to_Char(orddt,'dd/mm/yyyy') as orddt,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(orddt,'yyyymmdd') as fstr,(Case when to_char(orddt,'dd')='01' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(orddt,'dd')='02' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(orddt,'dd')='03' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(orddt,'dd')='04' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(orddt,'dd')='05' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(orddt,'dd')='06' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(orddt,'dd')='07' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(orddt,'dd')='08' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(orddt,'dd')='09' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(orddt,'dd')='10' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(orddt,'dd')='11' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(orddt,'dd')='12' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(orddt,'dd')='13' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(orddt,'dd')='14' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(orddt,'dd')='15' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(orddt,'dd')='16' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(orddt,'dd')='17' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(orddt,'dd')='18' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(orddt,'dd')='19' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(orddt,'dd')='20' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(orddt,'dd')='21' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(orddt,'dd')='22' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(orddt,'dd')='23' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(orddt,'dd')='24' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(orddt,'dd')='25' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(orddt,'dd')='26' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(orddt,'dd')='27' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(orddt,'dd')='28' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(orddt,'dd')='29' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(orddt,'dd')='30' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(orddt,'dd')='31' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date31 from pomas where branchcd!='DD' and type like '5%' and to_char(orddt,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "' union all SELECT 'Rcpt Dt.' as header,'-' as wono,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from ivoucher where branchcd!='DD' and type like '2%' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "') a,famst b,item c,type d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(d.type1) and d.id='B' order by a.header,a.branchcd,a.acode";  //from pomas and ivch table                        
                        SQuery = "select a.fstr,a.header,a.branchcd,d.name as branch_name,a.ordno,a.orddt,a.wono,a.acode,b.aname as supplier_name,a.icode,c.iname as item_name ,a.po_date1 as day1,a.po_date2 as day2,a.po_date3 as day3,a.po_date4 as day4,a.po_date5 as day5,a.po_date6  as day6,a.po_date7 as day7,a.po_date8 as day8,a.po_date9 as day9,a.po_date10 as day10,a.po_date11 as day11,a.po_date12 as day12,a.po_date13 as day13,a.po_date14 as day14,a.po_date15 as day15,a.po_date16 as day16,a.po_date17 as day17,a.po_date18 as day18,a.po_date19 as day19,a.po_date20 as day20,a.po_date21 as day21,a.po_date22 as day22,a.po_date23 as day23,a.po_date24 as day24,a.po_date25 as day25,a.po_date26 as day26,a.po_date27 as day27,a.po_date28 as day28,a.po_date29 as day29,a.po_date30 as day30,a.po_date31 as day31  from (SELECT 'Po Dt.' as header,DEL_sCH as wono,ordno,to_Char(orddt,'dd/mm/yyyy') as orddt,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(orddt,'yyyymmdd') as fstr,(Case when to_char(orddt,'dd')='01' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(orddt,'dd')='02' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(orddt,'dd')='03' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(orddt,'dd')='04' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(orddt,'dd')='05' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(orddt,'dd')='06' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(orddt,'dd')='07' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(orddt,'dd')='08' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(orddt,'dd')='09' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(orddt,'dd')='10' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(orddt,'dd')='11' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(orddt,'dd')='12' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(orddt,'dd')='13' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(orddt,'dd')='14' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(orddt,'dd')='15' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(orddt,'dd')='16' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(orddt,'dd')='17' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(orddt,'dd')='18' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(orddt,'dd')='19' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(orddt,'dd')='20' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(orddt,'dd')='21' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(orddt,'dd')='22' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(orddt,'dd')='23' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(orddt,'dd')='24' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(orddt,'dd')='25' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(orddt,'dd')='26' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(orddt,'dd')='27' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(orddt,'dd')='28' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(orddt,'dd')='29' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(orddt,'dd')='30' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(orddt,'dd')='31' then to_char(orddt,'dd/mm/yyyy')  else '0' end) as po_date31 from pomas where branchcd!='DD' and type like '5%' and to_char(orddt,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "' union all SELECT 'Rcpt Dt.' as header,'-' as wono,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from ivoucher where branchcd!='DD' and type like '2%' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "') a,famst b,item c,type d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(d.type1) and d.id='B' order by a.header,a.branchcd,a.acode";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevelJS("P.O. Dt Vs Rcpt Dt.(Portal) for the Month " + mq2 + "", frm_qstr);
                        #endregion
                        break;

                    case "F78127":
                        #region Sch. Dt Vs Rcpt Dt.(Portal)
                        // uname = "06V413";
                        mq1 = value1;
                        li = 0;
                        if (Convert.ToInt32(mq1) < 3)
                        {
                            li = Convert.ToInt32(year) + 1;
                        }
                        else
                        {
                            li = Convert.ToInt32(year);
                        }
                        mq2 = value1 + "/" + li;
                        //uname = "06C056";
                        //SQuery = "select a.fstr,a.header,a.acode,b.aname as supplier_name,a.icode,c.iname as item_name ,a.po_date1 as day1,a.po_date2 as day2,a.po_date3 as day3,a.po_date4 as day4,a.po_date5 as day5,a.po_date6  as day6,a.po_date7 as day7,a.po_date8 as day8,a.po_date9 as day9,a.po_date10 as day10,a.po_date11 as day11,a.po_date12 as day12,a.po_date13 as day13,a.po_date14 as day14,a.po_date15 as day15,a.po_date16 as day16,a.po_date17 as day17,a.po_date18 as day18,a.po_date19 as day19,a.po_date20 as day20,a.po_date21 as day21,a.po_date22 as day22,a.po_date23 as day23,a.po_date24 as day24,a.po_date25 as day25,a.po_date26 as day26,a.po_date27 as day27,a.po_date28 as day28,a.po_date29 as day29,a.po_date30 as day30,a.po_date31 as day31 from (SELECT 'Sch Dt.' as header,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from schedule where branchcd!='DD' and type='66' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "' union all SELECT 'Rcpt Dt.' as header,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from ivoucher where branchcd!='DD' and type like '2%' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "') a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by a.header,a.acode";  //from pomas and ivch table                        
                        SQuery = "select a.fstr,a.header,a.branchcd,d.name as branch_name,a.acode,b.aname as supplier_name,a.icode,c.iname as item_name ,a.po_date1 as day1,a.po_date2 as day2,a.po_date3 as day3,a.po_date4 as day4,a.po_date5 as day5,a.po_date6  as day6,a.po_date7 as day7,a.po_date8 as day8,a.po_date9 as day9,a.po_date10 as day10,a.po_date11 as day11,a.po_date12 as day12,a.po_date13 as day13,a.po_date14 as day14,a.po_date15 as day15,a.po_date16 as day16,a.po_date17 as day17,a.po_date18 as day18,a.po_date19 as day19,a.po_date20 as day20,a.po_date21 as day21,a.po_date22 as day22,a.po_date23 as day23,a.po_date24 as day24,a.po_date25 as day25,a.po_date26 as day26,a.po_date27 as day27,a.po_date28 as day28,a.po_date29 as day29,a.po_date30 as day30,a.po_date31 as day31 from (SELECT 'Sch Dt.' as header,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from schedule where branchcd!='DD' and type='66' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "' union all SELECT 'Rcpt Dt.' as header,branchcd,trim(acode) as acode,trim(icode) as icode,trim(icode)||to_char(vchdate,'yyyymmdd') as fstr,(Case when to_char(vchdate,'dd')='01' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date1,(Case when to_char(vchdate,'dd')='02' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date2,(Case when to_char(vchdate,'dd')='03' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date3,(Case when to_char(vchdate,'dd')='04' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date4,(Case when to_char(vchdate,'dd')='05' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date5,(Case when to_char(vchdate,'dd')='06' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date6,(Case when to_char(vchdate,'dd')='07' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date7,(Case when to_char(vchdate,'dd')='08' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date8,(Case when to_char(vchdate,'dd')='09' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date9,(Case when to_char(vchdate,'dd')='10' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date10,(Case when to_char(vchdate,'dd')='11' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date11,(Case when to_char(vchdate,'dd')='12' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date12,(Case when to_char(vchdate,'dd')='13' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date13,(Case when to_char(vchdate,'dd')='14' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date14,(Case when to_char(vchdate,'dd')='15' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date15,(Case when to_char(vchdate,'dd')='16' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date16,(Case when to_char(vchdate,'dd')='17' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date17,(Case when to_char(vchdate,'dd')='18' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date18,(Case when to_char(vchdate,'dd')='19' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date19,(Case when to_char(vchdate,'dd')='20' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date20,(Case when to_char(vchdate,'dd')='21' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date21,(Case when to_char(vchdate,'dd')='22' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date22,(Case when to_char(vchdate,'dd')='23' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date23,(Case when to_char(vchdate,'dd')='24' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date24,(Case when to_char(vchdate,'dd')='25' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date25,(Case when to_char(vchdate,'dd')='26' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date26,(Case when to_char(vchdate,'dd')='27' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date27,(Case when to_char(vchdate,'dd')='28' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date28,(Case when to_char(vchdate,'dd')='29' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date29,(Case when to_char(vchdate,'dd')='30' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date30,(Case when to_char(vchdate,'dd')='31' then to_char(vchdate,'dd/mm/yyyy')  else '0' end) as po_date31 from ivoucher where branchcd!='DD' and type like '2%' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and trim(acode)='" + uname + "') a,famst b,item c,type d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(d.type1) and d.id='B' order by a.header,a.branchcd,a.acode";  //from pomas and ivch table                        
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevelJS("Sch. Dt Vs Rcpt Dt.(Portal) for the Month " + mq2 + "", frm_qstr);
                        #endregion
                        break;

                    case "F78145":
                        hfcode.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "N");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70240");
                        fgen.fin_acct_reps(frm_qstr);
                        break;

                    default:
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;
                }
            }
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
                case "F78136"://icon not found..........update  fin_msys set prd='Y',BRN='N',web_Action='../tej-base/om_view_sport.aspx'  where id='F78136'
                    #region Supply Sch.Cheklist
                    SQuery = "select a.branchcd,D.NAME AS BRANCH_NAME,a.vchnum as sch_no,to_char(a.vchdate,'dd/mm/yyyy') as sch_Date,trim(a.acode) as Supp_code,trim(b.aname) as supp_name,trim(a.icode) as icode,trim(c.iname) as item_name,c.unit,c.cpartno,sum(a.total) as total,sum(a.day1) as day1,sum(a.day2) as day2,sum(a.day3) as day3,sum(a.day4) as day4,sum(a.day5) as day5,sum(a.day6) as day6,sum(a.day7) as day7,sum(a.day8) as day8,sum(a.day9) as day9,sum(a.day10) as day10,sum(a.day11) as day11,sum(a.day12) as day12,sum(a.day13) as day13,sum(a.day14) as day14,sum(a.day15) as day15,sum(a.day16)  as day16,sum(a.day17) as day17,sum(a.day18) as day18,sum(a.day19) as day19,sum(a.day20) as day20,sum(a.day21) as day21,sum(a.day22) as day22,sum(a.day23) as day23,sum(a.day24) as day24,sum(a.day25) as day25,sum(a.day26) as day26,sum(a.day27) as day27,sum(a.day28) as day28,sum(a.day29) as day29,sum(a.day30) as day30,sum(a.day31) as day31  from schedule a,famst b,item c,TYPE D where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(D.type1) AND D.ID='B' and  a.branchcd!='DD' and a.type='66' and a.vchdate " + xprdrange + " and a.acode='" + uname + "' group by a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') ,trim(a.acode),trim(b.aname),trim(a.icode) ,trim(c.iname),c.unit,c.cpartno,a.branchcd,D.NAME order by a.branchcd,Sch_No desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Supply Sch.Cheklist from " + fromdt + " to " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "F78135"://icon not found...........update  fin_msys set prd='Y',BRN='N',web_Action='../tej-base/om_view_sport.aspx'  where id='F78135'          
                    #region Supply P.O. Cheklist
                    // uname = "06P115";
                    SQuery = "select a.branchcd,C.NAME AS BRANCH_NAME, a.ordno as order_no,to_char(a.orddt,'dd/mm/yyyy') as order_Date,trim(a.acode) as supp_code,trim(b.aname) as supp_name,a.rate_cd as po_total,a.mode_tpt as transport,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.app_by,a.type from pomas a,famst b,TYPE C where trim(a.acode)=trim(b.acode) and trim(a.branchcd)=trim(c.type1) and c.id='B'  AND  a.branchcd!='DD' and a.type like '5%' and a.orddt " + xprdrange + " and trim(a.acode)='" + uname + "' order by A.BRANCHCD, order_no desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Supply P.O. Cheklist from " + fromdt + " to " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "F78137"://update fin_msys set web_Action='../tej-base/om_view_sport.aspx' ,prd='Y',BRN='N'  WHERE ID='F78137';              
                    #region Order Vs Reciept Cheklist
                  //  uname = "06I050";//for testing
                    //SQuery = "select distinct a.branchcd,c.name as branch_name,trim(a.ordno)||trim(a.type) as grp,a.fstr,a.erp_Code,a.ordno,A.VD as orddt,a.mrr,a.mrrdt,a.qtyord,a.soldqty,a.prate,a.acode,a.party,trim(b.iname) as iname,trim(B.CPARTNO) as cpartno,b.unit,a.counter,A.TYPE from (sELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,branchcd,trim(Icode) as ERP_code,ordno,to_char(orddt,'dd/MM/yyyy') as vd,null as mrr,null as mrrdt,nvl(Qtyord,0) as qtyord,0 as Soldqty,((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0)) as prate,null as party,trim(acode) as acode,'1' as counter,TYPE from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + uname + "%' union all SELECT trim(a.icode)||'-'||to_ChaR(a.podate,'YYYYMMDD')||'-'||a.ponum||'-'||trim(a.ordlineno) as fstr,A.branchcd,trim(a.Icode) as ERP_code,trim(a.ponum) as ordno,to_char(a.podate,'dd/MM/yyyy') as vd,trim(a.vchnum) as mrr,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,0 as Qtyord,nvl(a.iqty_chl,0) as qtyord,0 as irate, trim(b.aname) as party,trim(a.acode) as acode,'2' as counter,A.TYPE from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='DD' and a.type like '0%' and type!='04' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(a.Acode) like '" + uname + "%' and length(trim(a.ponum))=6 and trim(a.ponum)!='000000') a,item b,type c where trim(a.erp_code)=trim(b.icode) and trim(a.branchcd)=trim(c.type1) and c.id='B'  and to_date(a.vd,'dd/mm/yyyy') " + xprdrange + "  and A.acode like '" + uname + "%' ORDER BY a.branchcd,A.ACODE,A.ERP_code,A.ORDNO, a.counter,a.type"; //old
                    SQuery = "select distinct a.branchcd,c.name as branch_name,trim(a.ordno)||trim(a.type) as grp,a.fstr,a.erp_Code,a.ordno,A.VD as orddt,a.wono as work_order_no,a.mrr,a.mrrdt" +
                        ",a.qtyord,a.soldqty,a.prate,a.acode,f.aname as party,trim(b.iname) as iname,trim(B.CPARTNO) as cpartno,b.unit,a.counter,A.TYPE from (" +
                        "sELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,DEL_sCH as wono,branchcd,trim(Icode) as ERP_code,ordno," +
                        "to_char(orddt,'dd/MM/yyyy') as vd,null as mrr,null as mrrdt,nvl(Qtyord,0) as qtyord,0 as Soldqty,((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0)) as prate" +
                        ",null as party,trim(acode) as acode,'1' as counter,TYPE from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or " +
                        "trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + uname + "%' union all " +
                        "SELECT trim(a.icode)||'-'||to_ChaR(a.podate,'YYYYMMDD')||'-'||a.ponum||'-'||trim(a.ordlineno) as fstr,'-' as wono,A.branchcd,trim(a.Icode) as ERP_code,trim(a.ponum) " +
                        "as ordno,to_char(a.podate,'dd/MM/yyyy') as vd,trim(a.vchnum) as mrr,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,0 as Qtyord,nvl(a.iqty_chl,0) as qtyord,0 as irate" +
                        ", trim(b.aname) as party,trim(a.acode) as acode,'2' as counter,A.TYPE from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='DD' and a.type " +
                        "like '0%' and type!='04' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(a.Acode) like '" + uname + "%' and length(trim(a.ponum))=6 and " +
                        "trim(a.ponum)!='000000') a,item b,type c,famst f where trim(a.acode)=trim(f.acode) and trim(a.erp_code)=trim(b.icode) and trim(a.branchcd)=trim(c.type1) and c.id='B'  and to_date(a.vd,'dd/mm/yyyy') " +
                        "" + xprdrange + "  and A.acode like '" + uname + "%' ORDER BY a.branchcd,A.ACODE,A.ERP_code,A.ORDNO, a.counter,a.type";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("Order Vs Reciept Checklists from " + fromdt + " to " + todt + "", frm_qstr);
                    #endregion
                    break;
                case "F78108":
                case "F78110":
                case "F78112":
                    if (val == "F78108")// for my Shipment Your mRR
                    {
                        cond = "";
                        header_n = "My Shipment (Your MRR)";
                    }
                    if (val == "F78110") // for Goods Pending in Qc
                    {
                        cond = "AND A.STORE!='Y'";
                        header_n = "Goods Pending In Qc";
                    }
                    if (val == "F78112") // goods with material Shortage
                    {
                        cond = "AND A.IQTY_CHL!=A.IDIAMTR";
                        header_n = "Goods With Material Shortage";
                    }
                    SQuery = "select trim(a.genum) as genum,to_Char(a.gedate,'dd/mm/yyyy') as gedate,trim(a.vchnum) as mrr_no,to_Char(a.vchdate,'dd/mm/yyyy') as mrr_Date,trim(a.icode) as item_code,trim(b.iname) as item_name,trim(a.ponum) as ponum,to_char(a.podate,'dd/mm/yyyy') as podate,trim(a.invno) as inv_no,to_Char(a.invdate,'dd/mm/yyyy') as invdate,a.iqty_chl as qty_challan,a.idiamtr as qty_actual,a.acpt_ud as quality_pass,a.rej_rw as reject,(Case when nvl(a.finvno,'-')='-' then 'Not Passed' else a.finvno end) as purch_voucher ,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b  where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + " AND TRIM(A.ACODE)='" + uname + "' " + cond + " ORDER BY vdd desc,trim(a.vchnum) desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("" + header_n + " for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F78145":
                    SQuery = "SELECT DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as party,a.acode AS CODE,b.email FROM voucher a, famst b where trim(a.acode)=trim(b.acodE) and substr(a.acode,1,2) in ('05','06') and nvl(trim(b.email),'-')!='-' and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " and a.type like '%' and trim(a.acode)='" + uname + "' ORDER BY b.aname";
                    if (co_cd == "PRIN")
                        SQuery = "Select /*+ INDEX_DESC(voucher ind_VCH_DATE) */ trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as party,a.acode AS CODE,b.email,a.cramt,a.type from voucher a,famst b where a.vchdate " + xprdrange + " and TRIM(a.RCODE)= TRIM(b.acode) and a.type<>'20' and substr(a.type,1,1)='2' and cramt>0 and a.type like '%' and a.branchcd='" + mbr + "' and nvl(trim(b.email),'-')!='-' and trim(a.acode)='" + uname + "' order by vchdate desc,vchnum desc";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "Y");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "N");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                    break;
                case "F78141":
                case "F78142":
                    cond = "";
                    if (val == "F78141")
                    {
                        header_n = "My Debit Note";
                        cond = "and a.type='31' and a.rcode='" + uname + "'";
                        SQuery = "select trim(a.vchnum) as debit_note_no,to_char(a.vchdate,'dd/mm/yyyy') as debit_note_date,b.aname as party,a.cramt,a.naration from voucher a, famst b where trim(a.rcode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " " + cond + "";
                    }
                    else
                    {
                        header_n = "My Credit Note";
                        cond = "and a.type='32' and a.rcode='" + uname + "'";
                        SQuery = "select trim(a.vchnum) as debit_note_no,to_char(a.vchdate,'dd/mm/yyyy') as debit_note_date,b.aname as party,a.dramt,a.naration from voucher a, famst b where trim(a.rcode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " " + cond + "";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelJS("" + header_n + " for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
            }
        }
    }
}