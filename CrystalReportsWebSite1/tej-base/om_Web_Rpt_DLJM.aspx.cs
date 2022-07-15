using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

//  F25374

public partial class om_Web_Rpt_DLJM : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld;
    int i0, i1, i2, i3, i4, v = 0; DateTime date1, date2; DataSet ds, ds3, oDS;
    DataTable dt, ph_tbl, dt1, dt2, dt3, dt4, dt5, dtm, mdt, mdt1, vdt, dtPo, fmdt, dt_dist, dt_dist1, dticode, dticode2 = new DataTable();
    DataRow dro, dr1, dro1 = null;
    DataView view1imx;
    DataTable dtdrsimx;
    double month, to_cons, itot_stk, itv, db9, db8, db7, db6, db5, db4, db3, db2, db1, db; DataRow oporow, ROWICODE, ROWICODE2; DataView dv, mvdview, vdview, vdview1, dist1_view, sort_view;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string party_cd = "";
    string part_cd = "";
    string frm_UserID;
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
        HCID = hfid.Value.Trim();
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
                case "RPT1":
                case "RPT4":
                case "RPT6":
                case "RPT11":
                case "RPT13":
                case "RPT14":
                case "RPT15":
                case "RPT16":
                case "RPT17":
                case "RPT18":
                case "RPT19":
                case "RPT20":
                case "RPT21":
                case "RPT25":
                case "RPT26":
                case "RPT27":
                case "RPT28":
                    SQuery = "select DISTINCT a.TYPE AS FSTR,a.TYPE AS CODE,b.NAME from sale a,type b where trim(a.type)=trim(b.type1) and b.id='V' and a.branchcd='" + mbr + "' and a.type in ('4F','42','43','4J') and a.vchdate " + xprdrange + "  ordER BY FSTR";
                    header_n = "Select Type";
                    i0 = 1;
                    break;

            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (i0 == 1)
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
        val = hfid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";
            if (val == "M03012" || val == "P15005B" || val == "P15005Z")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "RPT1":
                    case "RPT4":
                    case "RPT6":
                    case "RPT11":
                    case "RPT13":
                    case "RPT14":
                    case "RPT15":
                    case "RPT16":
                    case "RPT17":
                    case "RPT18":
                    case "RPT19":
                    case "RPT20":
                    case "RPT21":
                    case "RPT25":
                    case "RPT26":
                    case "RPT27":
                    case "RPT28":
                        hf2.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", val);
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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
            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        val = hfid.Value.Trim();
        //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            value3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLS_2RESIZE", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLS_WIDTHS", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLS_2RALIGN", "");

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

            tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                table1 = tbl_flds.Split('@')[2].ToString();
                table2 = tbl_flds.Split('@')[3].ToString();
                table3 = tbl_flds.Split('@')[4].ToString();
                table4 = tbl_flds.Split('@')[5].ToString();
                sortfld = sortfld.Replace("`", "'");
                rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            // after prdDmp this will run            
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID + val);
            switch (val)
            {

                case "RPT1":
                    #region Party Wise Month(Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq3 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        mq3 = "b.bssch like '%' ";
                        party_cd = "c.bssch like '%' ";
                    }
                    else
                    {
                        mq3 = "b.bssch in (" + party_cd + ") ";
                        party_cd = "c.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "A.aCODE like '%' ";
                    }
                    else
                    {
                        part_cd = "A.aCODE in (" + part_cd + ") ";
                    }
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' sch_name,round(sum(a.iamount),2) as total,sum((Case when to_char(vchdate,'mm')='04' then round(a.iamount,2)   else 0 end)) as Apr,sum((Case when to_char(vchdate,'mm')='05' then round(a.iamount,2)  else 0 end)) as may,sum((Case when to_char(vchdate,'mm')='06' then round(a.iamount,2)  else 0 end)) as jun,sum((Case when to_char(vchdate,'mm')='07' then round(a.iamount,2)   else 0 end)) as jul,sum((Case when to_char(vchdate,'mm')='08' then round(a.iamount,2)   else 0 end)) as aug,sum((Case when to_char(vchdate,'mm')='09' then round(a.iamount,2)   else 0 end)) as sep,sum((Case when to_char(vchdate,'mm')='10' then round(a.iamount,2)   else 0 end)) as oct,sum((Case when to_char(vchdate,'mm')='11' then round(a.iamount,2)   else 0 end)) as nov,sum((Case when to_char(vchdate,'mm')='12' then round(a.iamount,2)   else 0 end)) as dec,sum((Case when to_char(vchdate,'mm')='01' then round(a.iamount,2)   else 0 end)) as jan,sum((Case when to_char(vchdate,'mm')='02' then round(a.iamount,2)   else 0 end)) as feb,sum((Case when to_char(vchdate,'mm')='03' then round(a.iamount,2)   else 0 end)) as mar, 1 as setno  from IVOUCHER a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + part_cd + " AND " + mq3 + " and substr(trim(icode),1,2)='93'  union all  select trim(c.bssch) as fstr,'-' as gstr,trim(d.name) as sch_name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar),2) as total_amt,round(sum(a.apr),2) as apr,round(sum(a.may),2) as may,round(sum(a.jun),2) as jun,round(sum(a.jul),2) as jul,round(sum(a.aug),2) as aug,round(sum(a.sep),2) as sep,round(sum(a.oct),2) as oct,round(sum(a.nov),2) as nov,round(sum(a.dec),2) as dec,round(sum(a.jan),2) as jan,round(sum(a.feb),2) as feb,round(sum(a.mar),2) as mar, 2 as setno from ( select type,ACODE,icode,(Case when to_char(vchdate,'mm')='04' then iamount   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount   else 0 end) as mar,morder from IVOUCHER where branchcd='" + mbr + "' and type IN (" + hf2.Value + ") and vchdate " + xprdrange + " and substr(trim(icode),1,2)='93' ) a,FAMST C,typegrp d where TRIM(A.ACODE)=TRIM(C.ACODE) and trim(c.bssch)=trim(d.type1) and d.id='A' and " + party_cd + " AND " + part_cd + " group by trim(d.name),trim(c.bssch)) order by setno,sch_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "4#5#6#7#8#9#10#11#12#13#14#15#16#", "3#4#", "230#100#");
                    SQuery = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' Party_name,round(sum(a.iamount),2) as total ,sum((Case when to_char(vchdate,'mm')='04' then round(a.iamount,2)  else 0 end)) as Apr,sum((Case when to_char(vchdate,'mm')='05' then round(a.iamount,2)   else 0 end)) as may,sum((Case when to_char(vchdate,'mm')='06' then round(a.iamount,2)   else 0 end)) as jun,sum((Case when to_char(vchdate,'mm')='07' then round(a.iamount,2)   else 0 end)) as jul,sum((Case when to_char(vchdate,'mm')='08' then round(a.iamount,2)   else 0 end)) as aug,sum((Case when to_char(vchdate,'mm')='09' then round(a.iamount,2)   else 0 end)) as sep,sum((Case when to_char(vchdate,'mm')='10' then round(a.iamount,2)   else 0 end)) as oct,sum((Case when to_char(vchdate,'mm')='11' then round(a.iamount,2)   else 0 end)) as nov,sum((Case when to_char(vchdate,'mm')='12' then round(a.iamount,2)   else 0 end)) as dec,sum((Case when to_char(vchdate,'mm')='01' then round(a.iamount,2)   else 0 end)) as jan,sum((Case when to_char(vchdate,'mm')='02' then round(a.iamount,2)   else 0 end)) as feb,sum((Case when to_char(vchdate,'mm')='03' then round(a.iamount,2)   else 0 end)) as mar, 1 as setno from IVOUCHER a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate  " + xprdrange + " and " + part_cd + " AND " + mq3 + " and substr(trim(icode),1,2)='93'  union all  select trim(c.bssch)||trim(a.acode) as fstr,trim(c.bssch) as gstr,trim(c.aname) as Party_name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar),2) as total_amt,round(sum(a.apr),2) as apr,round(sum(a.may),2) as may,round(sum(a.jun),2) as jun,round(sum(a.jul),2) as jul,round(sum(a.aug),2) as aug,round(sum(a.sep),2) as sep,round(sum(a.oct),2) as oct,round(sum(a.nov),2) as nov,round(sum(a.dec),2) as dec,round(sum(a.jan),2) as jan,round(sum(a.feb),2) as feb,round(sum(a.mar),2) as mar,2 as setno from ( select type,ACODE,icode,(Case when to_char(vchdate,'mm')='04' then iamount   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount   else 0 end) as mar,morder from IVOUCHER where branchcd='" + mbr + "' and type IN (" + hf2.Value + ") and vchdate " + xprdrange + " and substr(trim(icode),1,2)='93' ) a,FAMST C where TRIM(A.ACODE)=TRIM(C.ACODE) and " + party_cd + "  AND " + part_cd + " group by trim(c.aname),trim(c.bssch),trim(a.acode) ) order by Party_name";
                    fgen.drillQuery(1, SQuery, frm_qstr, "4#5#6#7#8#9#10#11#12#13#14#15#16#", "3#4#", "230#100#");
                    mq2 = "select '-' as fstr,trim(b.bssch)||trim(a.acode) as gstr,a.type,a.vchnum as inv_no,to_char(A.vchdate,'dd/mm/yyyy') as invdate,sum(a.iamount) as value from ivoucher a,famst b where trim(A.acode)=trim(b.acode) and A.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and A.vchdate  " + xprdrange + " and A.aCODE like '%' and substr(trim(icode),1,2)='93' GROUP BY trim(a.acode),trim(b.bssch),a.vchnuM,to_char(A.vchdate,'dd/mm/yyyy'),a.type order by a.vchnum";
                    fgen.drillQuery(2, mq2, frm_qstr, "6#", "", "");
                    fgen.Fn_DrillReport("All Party Wise Month(Sales Analysis-Value) from " + fromdt + " and to " + todt + " ", frm_qstr, "3#4#5#6#7#8#9#10#11#12#13#14#15#16#17#", "3#4#5#6#7#8#9#10#11#12#13#14#15#16#17#", "100#500#20#500#20#500#20#500#20#500#20#500#20#10#17#");
                    #endregion
                    break;

                case "RPT4":
                    #region  Item Group GroupWise (Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq1 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        mq1 = "substr(trim(a.icode),1,2) like '%'";
                        party_cd = "A.mgcode like '%' ";
                    }
                    else
                    {
                        mq1 = "substr(trim(a.icode),1,2) in (" + party_cd + ")";
                        party_cd = "A.mgcode in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "b.bssch like '%' ";
                    }
                    else
                    {
                        part_cd = "b.bssch in (" + part_cd + ") ";
                    }
                    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as sch_name,sum(a.IQTYOUT) as total_Qty ,sum((Case when to_char(vchdate,'mm')='04' then a.IQTYOUT   else 0 end)) as Apr,sum((Case when to_char(vchdate,'mm')='05' then a.IQTYOUT   else 0 end)) as may,sum((Case when to_char(vchdate,'mm')='06' then a.IQTYOUT   else 0 end)) as jun,sum((Case when to_char(vchdate,'mm')='07' then a.IQTYOUT   else 0 end)) as jul,sum((Case when to_char(vchdate,'mm')='08' then a.IQTYOUT   else 0 end)) as aug,sum((Case when to_char(vchdate,'mm')='09' then a.IQTYOUT   else 0 end)) as sep,sum((Case when to_char(vchdate,'mm')='10' then a.IQTYOUT   else 0 end)) as oct,sum((Case when to_char(vchdate,'mm')='11' then a.IQTYOUT   else 0 end)) as nov,sum((Case when to_char(vchdate,'mm')='12' then a.IQTYOUT   else 0 end)) as dec,sum((Case when to_char(vchdate,'mm')='01' then a.IQTYOUT   else 0 end)) as jan,sum((Case when to_char(vchdate,'mm')='02' then a.IQTYOUT   else 0 end)) as feb,sum((Case when to_char(vchdate,'mm')='03' then a.IQTYOUT   else 0 end)) as mar, 1 as setno from IVOUCHER a ,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + part_cd + " and " + mq1 + " and substr(trim(icode),1,2)='93' union all  select trim(a.bssch) as fstr,'-' as gstr,a.sch_name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar, 2 as setno  from (select trim(b.bssch) as bssch,trim(c.name) as sch_name,a.type,  a.mgcode,a.acode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar, 2 as setno from (select type,substr(trim(icode),1,2) as mgcode,acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + part_cd + " ) a where " + party_cd + " group by a.sch_name,trim(a.bssch)) order by setno,sch_name";
                    fgen.drillQuery(0, mq2, frm_qstr, "4#5#6#7#8#9#10#11#15#16#", "3#4#", "230#100#");

                    mq3 = "select '-' as fstr,'-' as gstr,'TOTAL' as ITEM_GROUP_NAME,sum(a.IQTYOUT) as total_Qty ,sum((Case when to_char(vchdate,'mm')='04' then a.IQTYOUT   else 0 end)) as Apr,sum((Case when to_char(vchdate,'mm')='05' then a.IQTYOUT   else 0 end)) as may,sum((Case when to_char(vchdate,'mm')='06' then a.IQTYOUT   else 0 end)) as jun,sum((Case when to_char(vchdate,'mm')='07' then a.IQTYOUT   else 0 end)) as jul,sum((Case when to_char(vchdate,'mm')='08' then a.IQTYOUT   else 0 end)) as aug,sum((Case when to_char(vchdate,'mm')='09' then a.IQTYOUT   else 0 end)) as sep,sum((Case when to_char(vchdate,'mm')='10' then a.IQTYOUT   else 0 end)) as oct,sum((Case when to_char(vchdate,'mm')='11' then a.IQTYOUT   else 0 end)) as nov,sum((Case when to_char(vchdate,'mm')='12' then a.IQTYOUT   else 0 end)) as dec,sum((Case when to_char(vchdate,'mm')='01' then a.IQTYOUT   else 0 end)) as jan,sum((Case when to_char(vchdate,'mm')='02' then a.IQTYOUT   else 0 end)) as feb,sum((Case when to_char(vchdate,'mm')='03' then a.IQTYOUT   else 0 end)) as mar from IVOUCHER a ,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + mq1 + " union all  select trim(a.bssch)||trim(a.mgcode) as fstr,trim(a.bssch) as gstr,b.NAME AS ITEM_GROUP_NAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar  from (select trim(b.bssch) as bssch,a.type,  a.mgcode,a.acode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select type,substr(trim(icode),1,2) as mgcode,acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,famst b where trim(a.acode)=trim(b.acode)) a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and " + party_cd + " and b.id='Y' group by B.NAME,trim(a.bssch),trim(a.mgcode)";
                    fgen.drillQuery(1, mq3, frm_qstr, "4#5#6#7#8#9#10#11#12#13#14#15#16#", "3#4#", "230#100#");

                    SQuery = "select '-' as fstr,trim(b.bssch)||substr(trim(a.icode),1,2) as gstr,a.type,a.vchnum as inv_no,to_char(A.vchdate,'dd/mm/yyyy') as invdate,sum(a.iqtyout) as Qty from ivoucher a,famst b where trim(A.acode)=trim(b.acode) and A.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " GROUP BY trim(b.bssch),a.vchnuM,to_char(A.vchdate,'dd/mm/yyyy'),a.type,substr(trim(a.icode),1,2) order by a.vchnum ";
                    fgen.drillQuery(2, SQuery, frm_qstr, "6#", "", "");
                    fgen.Fn_DrillReport("Item Group Wise (Qty) from " + fromdt + " to " + todt + "", frm_qstr);

                    #endregion

                    break;

                case "RPT2":
                    #region One Party All Items(Sales Analysis)..this is old name but no party selection in this ..so name changed by schedule
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq1 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "B.BSSCH like '%' ";
                    }
                    else
                    {
                        party_cd = "B.BSSCH in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        mq1 = "a.TYPE like '4%' ";
                        part_cd = "a.TYPE like '4%' ";
                    }
                    else
                    {
                        mq1 = "a.TYPE in (" + part_cd + ") ";
                        part_cd = "a.TYPE in (" + part_cd + ") ";
                    }
                    //    SQuery = "select 'TOTAL' as TYPE,'-' AS sch_name,'-' as item_Details,'-' as unit,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_Value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where a.branchcd='" + mbr + "' and " + mq1 + " and a.vchdate " + xprdrange + "  and "+party_cd+" union all select a.type,a.sch_name,trim(b.iname) as item_Details,b.unit,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from (select trim(c.name) as sch_name,a.type, a.iqtyout,a.iamount,A.acode,a.icode  FROM (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + " ) a,FAMST B,TYPEGRP C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND trim(b.bssch)=trim(c.type1) and c.id='A' AND " + party_cd + " ) A,item b where trim(A.icode)=trim(b.icode) group by trim(b.iname),b.unit,a.sch_name,a.type"; //with type
                    //     SQuery = "select 'TOTAL' AS sch_name,'-' as item_Details,'-' as unit,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_Value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq1 + " and a.vchdate " + xprdrange + "  and "+party_cd+" union all select a.sch_name,trim(b.iname) as item_Details,b.unit,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from (select trim(c.name) as sch_name,a.type, a.iqtyout,a.iamount,A.acode,a.icode  FROM (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + " ) a,FAMST B,TYPEGRP C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND trim(b.bssch)=trim(c.type1) and c.id='A' AND " + party_cd + " ) A,item b where trim(A.icode)=trim(b.icode) group by trim(b.iname),b.unit,a.sch_name "; //without type
                    //==============drill report
                    mq1 = "select '-' AS FSTR,'-' AS GSTR,'TOTAL' AS sch_name,'-' as item_Details,'-' as unit,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_Value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + part_cd + "  and a.vchdate " + xprdrange + " and " + party_cd + " union all select TRIM(B.BSSCH)||TRIM(A.ICODE) AS FSTR,'-' AS GSTR,trim(c.name) as sch_name,trim(d.iname) as item_Details,d.unit,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price  FROM ivoucher a,famst b,item d,typegrp c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(d.icode) AND trim(b.bssch)=trim(c.type1) and c.id='A' and a.branchcd='" + mbr + "' and " + part_cd + " AND A.VCHDATE " + xprdrange + " AND " + party_cd + " group by trim(d.iname),D.unit,trim(c.name),TRIM(B.BSSCH),trim(a.icode)";
                    fgen.drillQuery(0, mq1, frm_qstr);
                    mq2 = "select '-' as fstr,TRIM(B.BSSCH)||TRIM(A.ICODE) as gstr,a.type,a.vchnum as bill_no,to_char(A.vchdate,'dd/mm/yyyy') as bill_date,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price  from ivoucher a,famst b where trim(A.acode)=trim(b.acode) and A.branchcd='" + mbr + "' and " + part_cd + " and A.vchdate " + xprdrange + " GROUP BY trim(a.icode),trim(b.bssch),a.vchnuM,to_char(A.vchdate,'dd/mm/yyyy'),a.type order by a.vchnum";
                    fgen.drillQuery(1, mq2, frm_qstr);
                    fgen.Fn_DrillReport("One Schedule All Items(Sales Analysis) from " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "RPT11":
                    #region  One Item All Party(Sales Analysis).............One Item Group Sales(Sales Analysis)--oldname
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq3 = ""; mq4 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "substr(trim(a.icode),1,2)='93' ";//
                    }
                    else
                    {
                        party_cd = "A.ICODE in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        mq3 = "b.bssch like '%' ";
                        part_cd = "c.bssch like '%'";
                    }
                    else
                    {
                        mq3 = "b.bssch in (" + part_cd + ")";
                        part_cd = "c.bssch in (" + part_cd + ")";
                    }
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL'  as item,'-' as Party,sum(a.iqtyout) as net_Sale_Qty,ROUND(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price,1 as setno from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.TYPE in (" + hf2.Value + ") and a.vchdate " + xprdrange + " AND " + party_cd + " and " + mq3 + "   union all select trim(a.acode)||trim(a.icode) as fstr,'-' as gstr,TRIM(B.INAME) AS ITEM,trim(c.aname) as Party,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price,2 as setno from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,item b,famst c  where trim(A.icode)=trim(b.icode) and trim(A.acode)=trim(c.acode) AND " + party_cd + " and " + part_cd + " group by b.unit,trim(c.aname),trim(a.acode),trim(a.icode),TRIM(B.INAME)) order by setno, item";
                    fgen.drillQuery(0, mq1, frm_qstr, "5#6#7#", "3#4#", "380#380#");
                    mq2 = "select '-' as fstr,trim(a.acode)||trim(a.icode) as gstr,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.TYPE in (" + hf2.Value + ") and a.vchdate " + xprdrange + " AND " + party_cd + " and " + mq3 + " group by a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) ";
                    fgen.drillQuery(1, mq2, frm_qstr, "6#7#8#", "", "");
                    fgen.Fn_DrillReport("One Item All Party Sales from " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "RPT12":
                    #region  Market Group wise Item wise /// earlier Sales res all items sale(Sales Analysis)=====all item group of party
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq4 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = " b.bssch like '%' ";
                    }
                    else
                    {
                        party_cd = " b.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        mq4 = "a.TYPE like '4%' ";
                        part_cd = "TYPE like '4%' ";
                    }
                    else
                    {
                        mq4 = "a.TYPE in (" + part_cd + ") ";
                        part_cd = "TYPE in (" + part_cd + ") ";
                    }
                    //=================new
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_group_name,round(sum(a.iqtyout),2) as net_sale_qty ,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq4 + " and a.vchdate " + xprdrange + " AND " + party_cd + " union all select trim(b.bssch) as fstr,'-' as gstr,trim(c.name) as account_group_name,round(sum(a.qty),2) as net_sale_qty,round(sum(a.amt),2) as net_sale_Amt,(case when SUM(nvl(a.qty,0))!=0 then ROUND(SUM(nvl(a.amt,0))/SUM(nvl(a.qty,0)),2) else 0 end) as avg_price, 2 as setno  from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' AND " + party_cd + " group by trim(b.bssch),TRIM(c.name)) order by setno,account_group_name";
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_group_name,round(sum(a.iqtyout),2) as net_sale_qty ,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq4 + " and a.vchdate " + xprdrange + " AND " + party_cd + " and substr(trim(a.icode),1,2)='93' union all select trim(b.bssch) as fstr,'-' as gstr,trim(c.name) as account_group_name,round(sum(a.qty),2) as net_sale_qty,round(sum(a.amt),2) as net_sale_Amt,(case when SUM(nvl(a.qty,0))!=0 then ROUND(SUM(nvl(a.amt,0))/SUM(nvl(a.qty,0)),2) else 0 end) as avg_price, 2 as setno  from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' AND " + party_cd + " and substr(trim(a.icode),1,2)='93' group by trim(b.bssch),TRIM(c.name)) order by setno,account_group_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "4#5#6#", "3#", "700#");
                    mq2 = "SELECT TRIM(A.ICODE)||TRIM(B.BSSCH) AS FSTR,TRIM(B.BSSCH) AS GSTR,TRIM(C.INAME) AS ITEM,SUM(A.IQTYOUT) AS QTY,SUM(A.IAMOUNT) AS VALUE,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND " + mq4 + " and A.vchdate " + xprdrange + " AND " + party_cd + " GROUP BY TRIM(A.ICODE),TRIM(B.BSSCH) ,TRIM(C.INAME) ,C.UNIT order by ITEM";
                    mq2 = "SELECT TRIM(A.ICODE)||TRIM(B.BSSCH) AS FSTR,TRIM(B.BSSCH) AS GSTR,TRIM(C.INAME) AS ITEM,SUM(A.IQTYOUT) AS QTY,SUM(A.IAMOUNT) AS VALUE,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND " + mq4 + " and A.vchdate " + xprdrange + " AND " + party_cd + " and substr(trim(a.icode),1,2)='93' GROUP BY TRIM(A.ICODE),TRIM(B.BSSCH) ,TRIM(C.INAME) ,C.UNIT order by ITEM";
                    fgen.drillQuery(1, mq2, frm_qstr, "4#5#6#", "", "");
                    mq3 = "SELECT '-' as fstr,TRIM(A.ICODE)||TRIM(B.BSSCH) AS GSTR,TRIM(C.INAME) AS ITEM,a.type,A.VCHNUM AS BILL_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS BILL_dATE,SUM(A.IQTYOUT) AS QTY,round(SUM(A.IAMOUNT),2) AS VALUE,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND " + mq4 + " and A.vchdate " + xprdrange + " AND " + party_cd + " GROUP BY A.TYPE,TRIM(A.ICODE),TRIM(B.BSSCH) ,TRIM(C.INAME) ,C.UNIT,A.VCHNUM,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') order by item,TRIM(C.INAME)";
                    mq3 = "SELECT '-' as fstr,TRIM(A.ICODE)||TRIM(B.BSSCH) AS GSTR,TRIM(C.INAME) AS ITEM,a.type,A.VCHNUM AS BILL_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS BILL_dATE,SUM(A.IQTYOUT) AS QTY,round(SUM(A.IAMOUNT),2) AS VALUE,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND " + mq4 + " and A.vchdate " + xprdrange + " AND " + party_cd + " and substr(trim(a.icode),1,2)='93' GROUP BY A.TYPE,TRIM(A.ICODE),TRIM(B.BSSCH) ,TRIM(C.INAME) ,C.UNIT,A.VCHNUM,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') order by item,TRIM(C.INAME)";
                    fgen.drillQuery(2, mq3, frm_qstr);
                    fgen.Fn_DrillReport("Market Group wise Item wise From " + fromdt + " and to " + todt + " ", frm_qstr);

                    #endregion
                    break;

                case "RPT13":
                    #region  One Party All Items (Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq3 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        mq3 = "b.bssch like '%' ";
                        party_cd = "d.bssch like '%' ";
                    }
                    else
                    {
                        mq3 = "b.bssch in (" + party_cd + ") ";
                        party_cd = "d.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "a.acode like '%' ";
                    }
                    else
                    {
                        part_cd = "a.acode in (" + part_cd + ") ";
                    }
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as Account_name,'-' as sch_name,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + part_cd + " and " + mq3 + " union all select trim(d.bssch)||trim(a.acode) as fstr,'-' as gstr,trim(d.aname) as Account_name,trim(t.name) as sch_name,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 2 as setno from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate  " + xprdrange + ") a,FAMST D,typegrp t where trim(a.acode)=trim(d.acode) and trim(d.bssch)=trim(t.type1) and t.id='A' and " + part_cd + " and d.bssch like '%'  group by trim(d.aname),a.type,trim(t.name),trim(a.acode) ,d.bssch) order by setno, Account_name";
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as Account_name,'-' as sch_name,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + part_cd + " and " + mq3 + " and substr(trim(a.icode),1,2)='93' union all select trim(d.bssch)||trim(a.acode) as fstr,'-' as gstr,trim(d.aname) as Account_name,trim(t.name) as sch_name,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 2 as setno from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate  " + xprdrange + ") a,FAMST D,typegrp t where trim(a.acode)=trim(d.acode) and trim(d.bssch)=trim(t.type1) and t.id='A' and " + part_cd + " and d.bssch like '%' and substr(trim(a.icode),1,2)='93' group by trim(d.aname),a.type,trim(t.name),trim(a.acode) ,d.bssch) order by setno, Account_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "5#6#7#", "3#4#", "380#380#");
                    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as Item_Name,sum(a.iqtyout) as net_Sale_Qty,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate  " + xprdrange + " and " + part_cd + "  and " + mq3 + " union all select trim(d.bssch)||trim(a.acode)||trim(a.icode) as fstr,trim(d.bssch)||trim(a.acode) as gstr,trim(c.iname) as Item_Name,sum(a.iqtyout) as net_Sale_Qty,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 2 as setno from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,FAMST D,item c where trim(a.acode)=trim(d.acode) and trim(a.icode)=trim(c.icode) and " + part_cd + " and " + party_cd + "  group by a.type,trim(a.acode),trim(c.iname),trim(a.icode),trim(d.bssch),c.unit) order by setno,Item_Name";
                    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as Item_Name,sum(a.iqtyout) as net_Sale_Qty,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate  " + xprdrange + " and " + part_cd + "  and " + mq3 + " and substr(trim(a.icode),1,2)='93' union all select trim(d.bssch)||trim(a.acode)||trim(a.icode) as fstr,trim(d.bssch)||trim(a.acode) as gstr,trim(c.iname) as Item_Name,sum(a.iqtyout) as net_Sale_Qty,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 2 as setno from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,FAMST D,item c where trim(a.acode)=trim(d.acode) and trim(a.icode)=trim(c.icode) and " + part_cd + " and " + party_cd + " and substr(trim(a.icode),1,2)='93' group by a.type,trim(a.acode),trim(c.iname),trim(a.icode),trim(d.bssch),c.unit) order by setno,Item_Name";
                    fgen.drillQuery(1, mq2, frm_qstr, "4#5#6#", "3#", "600#");
                    SQuery = "select '-' as fstr,trim(b.bssch)||trim(a.acode)||trim(a.icode) as gstr,a.type,a.vchnum AS Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_date,sum(a.iqtyout) as net_Sale_Qty,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where trim(a.acode)=trim(b.acode)  and a.branchcd='02' and a.TYPE in (" + hf2.Value + ") and a.vchdate " + xprdrange + " group by a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode),trim(b.bssch),trim(a.acode)";
                    fgen.drillQuery(2, SQuery, frm_qstr);
                    fgen.Fn_DrillReport("One Party All Items (Sales Analysis) from " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "RPT14":
                    #region  One Party All Items Grp(Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq3 = "";
                    if (party_cd.Trim().Length <= 1)
                    {
                        mq3 = "b.bssch like '%' ";
                        party_cd = "d.bssch like '%' ";
                    }
                    else
                    {
                        mq3 = "b.bssch in (" + party_cd + ") ";
                        party_cd = "d.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "a.acode like '%' ";
                    }
                    else
                    {
                        part_cd = "a.acode in (" + part_cd + ") ";
                    }
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as party,'-' as sch_name,round(sum(a.iqtyout),2) as net_sale_qty, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + mq3 + " AND " + part_cd + " union all select TRIM(a.acode)||trim(d.bssch) as fstr,'-' as gstr,TRIM(D.ANAME) AS PARTY,trim(t.name) as sch_name,round(sum(a.iqtyout),2) as net_Sale_Qty, 2 as setno from (select  type,trim(acode) as acode,substr(trim(icode),1,2) as mgcode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,FAMST D,typegrp t where TRIM(A.ACODE)=TRIM(D.ACODE) AND trim(d.bssch)=trim(t.type1) and t.id='A' AND " + party_cd + "  and " + part_cd + " group by TRIM(D.ANAME),A.ACODE,trim(t.name),TRIM(a.acode),trim(d.bssch)) order by setno,party";
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as party,'-' as sch_name,round(sum(a.iqtyout),2) as net_sale_qty, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + mq3 + " AND " + part_cd + "  and substr(trim(a.icode),1,2)='93' union all select TRIM(a.acode)||trim(d.bssch) as fstr,'-' as gstr,TRIM(D.ANAME) AS PARTY,trim(t.name) as sch_name,round(sum(a.iqtyout),2) as net_Sale_Qty, 2 as setno from (select  type,trim(acode) as acode,substr(trim(icode),1,2) as mgcode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,FAMST D,typegrp t where TRIM(A.ACODE)=TRIM(D.ACODE) AND trim(d.bssch)=trim(t.type1) and t.id='A' AND " + party_cd + "  and " + part_cd + " and substr(trim(a.icode),1,2)='93'  group by TRIM(D.ANAME),A.ACODE,trim(t.name),TRIM(a.acode),trim(d.bssch)) order by setno,party";
                    fgen.drillQuery(0, mq1, frm_qstr, "5#", "4#", "420#");
                    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as item_group_name,'-' as mgcode,sum(a.iqtyout) as net_sale_qty, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + "  and " + mq3 + " AND " + part_cd + " union all select TRIM(a.acode)||trim(d.bssch)||trim(a.mgcode) as fstr,TRIM(a.acode)||trim(d.bssch) as gstr,c.name as item_group_name,a.mgcode,sum(a.iqtyout) as net_Sale_Qty, 2 as setno from (select  type,trim(acode) as acode,substr(trim(icode),1,2) as mgcode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,FAMST D,type c  where TRIM(A.ACODE)=TRIM(D.ACODE) AND  trim(a.mgcode)=trim(c.type1) and c.id='Y' AND " + party_cd + " and " + part_cd + " group by C.name,TRIM(D.ANAME),A.ACODE,TRIM(a.acode),trim(d.bssch),a.mgcode)order by setno,item_group_name";
                    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as item_group_name,'-' as mgcode,sum(a.iqtyout) as net_sale_qty, 1 as setno  from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + "  and " + mq3 + " AND " + part_cd + " and substr(trim(a.icode),1,2)='93' union all select TRIM(a.acode)||trim(d.bssch)||trim(a.mgcode) as fstr,TRIM(a.acode)||trim(d.bssch) as gstr,c.name as item_group_name,a.mgcode,sum(a.iqtyout) as net_Sale_Qty, 2 as setno from (select  type,trim(acode) as acode,substr(trim(icode),1,2) as mgcode,trim(icode) as icode,nvl(iqtyout,0) as iqtyout from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,FAMST D,type c  where TRIM(A.ACODE)=TRIM(D.ACODE) AND  trim(a.mgcode)=trim(c.type1) and c.id='Y' AND " + party_cd + " and " + part_cd + " and substr(trim(a.icode),1,2)='93' group by C.name,TRIM(D.ANAME),A.ACODE,TRIM(a.acode),trim(d.bssch),a.mgcode)order by setno,item_group_name";
                    fgen.drillQuery(1, mq2, frm_qstr);
                    SQuery = "SELECT '-' AS FSTR,TRIM(a.acode)||trim(b.bssch)||substr(trim(a.icode),1,2) AS GSTR,TRIM(C.INAME) AS ITEM_NAME,SUM(A.IQTYOUT) AS QTY,substr(trim(a.icode),1,2) as main_grp,TRIM(A.ICODE) AS ICODE FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " GROUP BY TRIM(C.INAME) ,substr(trim(a.icode),1,2),tRIM(a.acode),trim(b.bssch),TRIM(A.ICODE) order by TRIM(C.INAME)";

                    fgen.drillQuery(2, SQuery, frm_qstr);
                    fgen.Fn_DrillReport("One Party All Items Grp(Sales Analysis) from " + fromdt + " and to " + todt + " ", frm_qstr);

                    #endregion
                    break;

                case "RPT15":
                    #region  One tems All Party(Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "substr(trim(A.ICODE),1,2)='93'";
                    }
                    else
                    {
                        party_cd = "A.ICODE in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "b.bssch like '%' ";
                    }
                    else
                    {
                        part_cd = "b.bssch in (" + part_cd + ") ";
                    }
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_group_name,round(sum(a.iqtyout),2) as net_sale_qty ,round(sum(a.iamount),2) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, 1 as setno from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " AND " + party_cd + " and " + part_cd + " union all select trim(b.bssch) as fstr,'-' as gstr,trim(c.name) as account_group_name,round(sum(a.qty),2) as net_sale_qty,round(sum(a.amt),2) as net_sale_Amt,(case when SUM(nvl(a.qty,0))!=0 then ROUND(SUM(nvl(a.amt,0))/SUM(nvl(a.qty,0)),2) else 0 end) as avg_price, 2 as setno  from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' AND " + party_cd + " and " + part_cd + " group by trim(b.bssch),TRIM(c.name)) order by setno, account_group_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "4#5#6#", "3#", "700#");
                    mq2 = "SELECT TRIM(A.ICODE)||TRIM(B.BSSCH) AS FSTR,TRIM(B.BSSCH) AS gSTR,TRIM(C.INAME) AS ITEM,SUM(A.IQTYOUT) AS QTY,round(SUM(A.IAMOUNT),2) AS VALUE,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " AND " + party_cd + " and " + part_cd + " GROUP BY A.TYPE,TRIM(A.ICODE),TRIM(B.BSSCH) ,TRIM(C.INAME) order by ITEM";
                    fgen.drillQuery(1, mq2, frm_qstr, "4#5#6#", "", "");
                    mq3 = "select * from (SELECT '-' as fstr,TRIM(A.ICODE)||TRIM(B.BSSCH) AS GSTR,TRIM(C.INAME) AS ITEM,A.VCHNUM AS BILL_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS BILL_dATE,SUM(A.IQTYOUT) AS QTY,round(SUM(A.IAMOUNT),2) AS VALUE,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price FROM IVOUCHER A,famst b,ITEM C WHERE  TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " AND " + party_cd + " GROUP BY A.TYPE,TRIM(A.ICODE),TRIM(C.INAME) ,A.VCHNUM ,TO_CHAR(a.VCHDATE,'DD/MM/YYYY'),TRIM(B.BSSCH)) order by item";
                    fgen.drillQuery(2, mq3, frm_qstr);
                    fgen.Fn_DrillReport("One Item Group of Party(Sales Analysis) From " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "RPT19":
                    #region Party Grp Std (Sales Analysis) // earlier rpt17 called
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "b.bssch like '%' ";
                    }
                    else
                    {
                        party_cd = "b.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "A.aCODE like '%' ";
                    }
                    else
                    {
                        part_cd = "A.aCODE in (" + part_cd + ") ";
                    }
                    //mq1 = "select '-' as fstr,'-' as gstr,'TOTAL' AS account_group_name,sum(a.iqtyout) as Net_Sale_Qty,sum(A.iamount) as net_sale_Amt from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " AND " + party_cd + " and " + part_cd + " union all select trim(b.bssch) as fstr,'-' as gstr,TRIM(d.name) as account_group_name,sum(a.qty) as net_sale_qty,sum(A.amt) as net_sale_Amt  from (select TYPE,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp d where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(d.type1) and d.id='A' and  " + party_cd + " and " + part_cd + "  group by TRIM(d.name),trim(b.bssch)";
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_group_name,sum(a.iqtyout) as Net_Sale_Qty,round(sum(A.iamount),2) as net_sale_Amt,1 as setno from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ")  and A.vchdate " + xprdrange + " AND " + party_cd + " and " + part_cd + "  union all select trim(b.bssch) as fstr,'-' as gstr,TRIM(d.name) as account_group_name,sum(a.iqtyout) as net_sale_qty,round(sum(A.iamount),2) as net_sale_Amt,2 as setno from IVOUCHER a,famst B,typegrp d where a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(d.type1) and d.id='A' and  " + party_cd + " and " + part_cd + "  group by TRIM(d.name),trim(b.bssch) )order by setno,account_group_name";
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_group_name,sum(a.iqtyout) as Net_Sale_Qty,round(sum(A.iamount),2) as net_sale_Amt,1 as setno from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ")  and A.vchdate " + xprdrange + " AND " + party_cd + " and " + part_cd + "  and substr(trim(a.icode),1,2)='93' union all select trim(b.bssch) as fstr,'-' as gstr,TRIM(d.name) as account_group_name,sum(a.iqtyout) as net_sale_qty,round(sum(A.iamount),2) as net_sale_Amt,2 as setno from IVOUCHER a,famst B,typegrp d where a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(d.type1) and d.id='A' and  " + party_cd + " and " + part_cd + " and substr(trim(a.icode),1,2)='93' group by TRIM(d.name),trim(b.bssch) )order by setno,account_group_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "4#5#", "3#", "850#");
                    mq2 = "SELECT trim(a.acode)||trim(b.bssch) as fstr,trim(b.bssch) as gstr,trim(b.aname) as party,sum(a.iqtyout) as net_Sale_Qty,round(sum(A.iamount),2) as Net_Sale_Amt FROM ivoucher A ,famst b where A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) AND " + part_cd + " and " + party_cd + "  group by trim(a.acode),trim(b.aname),trim(b.bssch) order by trim(b.aname) ";
                    mq2 = "SELECT trim(a.acode)||trim(b.bssch) as fstr,trim(b.bssch) as gstr,trim(b.aname) as party,sum(a.iqtyout) as net_Sale_Qty,round(sum(A.iamount),2) as Net_Sale_Amt FROM ivoucher A ,famst b where A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) AND " + part_cd + " and " + party_cd + " and substr(trim(a.icode),1,2)='93' group by trim(a.acode),trim(b.aname),trim(b.bssch) order by trim(b.aname) ";
                    fgen.drillQuery(1, mq2, frm_qstr, "4#5#", "", "");
                    mq3 = "select '-' as fstr,trim(a.acode)||trim(b.bssch) as gstr,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,sum(a.iqtyout) as net_Sale_Qty,round(sum(A.iamount),2) as Net_Sale_Amt FROM  ivoucher A ,famst b where A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) AND " + part_cd + " group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(b.bssch),trim(a.acode) order by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') ";
                    mq3 = "select '-' as fstr,trim(a.acode)||trim(b.bssch) as gstr,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,sum(a.iqtyout) as net_Sale_Qty,round(sum(A.iamount),2) as Net_Sale_Amt FROM  ivoucher A ,famst b where A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) AND " + part_cd + "  and substr(trim(a.icode),1,2)='93' group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(b.bssch),trim(a.acode) order by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') ";
                    fgen.drillQuery(2, mq3, frm_qstr, "6#", "", "");
                    fgen.Fn_DrillReport("Party Grp Std (Sales Analysis) From " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "RPT21":
                    #region Party Wise Std Date(Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "b.bssch like '%' ";
                    }
                    else
                    {
                        party_cd = "b.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "A.aCODE like '%' ";
                    }
                    else
                    {
                        part_cd = "A.aCODE in (" + part_cd + ") ";
                    }
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_name,'-' AS Account_Code,round(sum(A.iamount),2) as net_sale_Amt, 1 as setno from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " AND " + part_cd + " and " + party_cd + " union all select trim(a.acode) as fstr,'-' as gstr,TRIM(B.aname) as account_name,trim(a.acode) as Account_Code,round(sum(A.amt),2) as net_sale_Amt, 2 as setno  from (select TYPE,trim(acode) as acode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type IN (" + hf2.Value + ") and vchdate  " + xprdrange + " ) a,famst B  where trim(a.acode)=trim(b.acode) and " + part_cd + " and " + party_cd + "  group by TRIM(b.aname),trim(a.acode)) order by setno,account_name";
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS account_name,'-' AS Account_Code,round(sum(A.iamount),2) as net_sale_Amt, 1 as setno from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " AND " + part_cd + " and " + party_cd + " and substr(trim(a.icode),1,2)='93' union all select trim(a.acode) as fstr,'-' as gstr,TRIM(B.aname) as account_name,trim(a.acode) as Account_Code,round(sum(A.amt),2) as net_sale_Amt, 2 as setno  from (select TYPE,trim(acode) as acode,trim(icode) as icode ,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type IN (" + hf2.Value + ") and vchdate  " + xprdrange + " ) a,famst B  where trim(a.acode)=trim(b.acode) and " + part_cd + " and " + party_cd + " and substr(trim(icode),1,2)='93' group by TRIM(b.aname),trim(a.acode)) order by setno,account_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "5#", "", "");
                    mq2 = "select '-' as fstr,trim(a.acode) as gstr,a.TYPE,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,round(sum(nvl(a.iamount,0)),2) as value from IVOUCHER  a,famst B where a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) and " + party_cd + " group by a.type,trim(A.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') order by a.vchnum  ";
                    mq2 = "select '-' as fstr,trim(a.acode) as gstr,a.TYPE,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,round(sum(nvl(a.iamount,0)),2) as value from IVOUCHER  a,famst B where a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) and " + party_cd + " and substr(trim(a.icode),1,2)='93' group by a.type,trim(A.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') order by a.vchnum  ";
                    fgen.drillQuery(1, mq2, frm_qstr, "6#", "", "");
                    fgen.Fn_DrillReport("Distributors Wise Sales From " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "RPT23":
                    #region Party Wise Area Mngr Wise(Sales Analysis)
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "b.bssch like '%' ";
                    }
                    else
                    {
                        party_cd = "b.bssch in (" + party_cd + ") ";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "a.TYPE like '4%' ";
                    }
                    else
                    {
                        part_cd = "a.tYPE in (" + part_cd + ") ";
                    }
                    #region old code
                    //mq1 = "select '-' as fstr,'-' as gstr,'TOTAL' AS grp_name,'-' as acode,'-' as account_name,sum(nvl(a.bill_tot,0)) as Net_amt from sale A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and " + part_cd + " and A.vchdate " + xprdrange + " and " + party_cd + " union all select TRIM(A.ACODE)||TRIM(B.BSSCH) AS FSTR,'-' AS GSTR,TRIM(c.name) as grp_name, trim(a.acode) as acode, b.aname||'('||b.city||')' as account_name,sum(nvl(a.bill_tot,0)) as Net_amt from sale a,famst b,typegrp c  where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " group by A.TYPE,a.acode,b.aname,b.city,TRIM(c.name),TRIM(B.BSSCH)";
                    //fgen.drillQuery(0, mq1, frm_qstr);
                    //mq2 = "SELECT '-' as fstr,TRIM(A.ACODE)||TRIM(B.BSSCH) as gstr,a.type,trim(b.aname) as party,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,sum(nvl(a.bill_tot,0)) as Net_amt FROM sale A ,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + part_cd + " and A.vchdate " + xprdrange + " and " + party_cd + " group by trim(a.acode),trim(b.bssch),a.vchnum,a.type,to_char(a.vchdate,'dd/mm/yyyy') ,trim(b.aname)";
                    //fgen.drillQuery(1, mq2, frm_qstr);
                    //fgen.Fn_DrillReport("Party Wise Area Magr Wise(Sales Analysis) From " + fromdt + " and to " + todt + " ", frm_qstr);   
                    #endregion
                    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' AS grp_name,'-' as grp_Code,round(sum(nvl(a.bill_tot,0)),2) as Net_amt, 1 as setno from sale A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + "  and a.type IN (" + hf2.Value + ") union all  select TRIM(B.BSSCH) AS FSTR,'-' AS GSTR,TRIM(c.name) as grp_name,TRIM(B.BSSCH) as grp_Coderound,round(sum(nvl(a.bill_tot,0)),2) as Net_amt, 2 as setno from sale a,famst b,typegrp c  where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " and a.type IN (" + hf2.Value + ") group by TRIM(c.name),TRIM(B.BSSCH)) order by setno,grp_name";
                    fgen.drillQuery(0, mq1, frm_qstr, "5#", "3#", "850#");
                    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as acode,'-' as account_name,round(sum(nvl(a.bill_tot,0)),2) as Net_amt, 1 as setno from sale A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " and a.type IN (" + hf2.Value + ") union all select TRIM(A.ACODE)||TRIM(B.BSSCH) AS FSTR,TRIM(B.BSSCH) AS GSTR,trim(a.acode) as acode, b.aname||'('||b.city||')' as account_name,round(sum(nvl(a.bill_tot,0)),2) as Net_amt,2 as setno from sale a,famst b  where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " and a.type IN (" + hf2.Value + ") group by a.acode,b.aname,b.city,TRIM(B.BSSCH)) order by setno,account_name ";
                    fgen.drillQuery(1, mq2, frm_qstr, "5#", "3#4#", "200#800#");
                    mq3 = "SELECT '-' as fstr,TRIM(A.ACODE)||TRIM(B.BSSCH) as gstr,a.type,trim(b.aname) as party,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,round(sum(nvl(a.bill_tot,0)),2) as Net_amt FROM sale A ,famst b where trim(a.acode)=trim(b.acode) and a.type IN (" + hf2.Value + ") and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " group by trim(a.acode),trim(b.bssch),a.vchnum,a.type,to_char(a.vchdate,'dd/mm/yyyy') ,trim(b.aname) order by a.vchnum";
                    fgen.drillQuery(2, mq3, frm_qstr, "7#", "3#4#5#", "180#600#100#");
                    fgen.Fn_DrillReport("Party Wise Area Magr Wise(Sales Analysis) From " + fromdt + " and to " + todt + " ", frm_qstr);
                    #endregion
                    break;

                #region old_report_code_removed
                //case "RPT24":
                //    #region Party Wise Columnar Report==doing changes in qry
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "b.bssch in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "a.TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        part_cd = "a.TYPE in (" + part_cd + ") ";
                //    }
                //    ///===drill report
                //    //mq1 = "select '-' as fstr,'-' as gstr,'TOTAL' AS sch_name,'-' as acode,'-' as Account_Name,SUM(a.IAMOUNT) AS Total_Qty from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " union all select TRIM(A.ACODE)||TRIM(B.BSSCH) as fstr,'-' as gstr,trim(c.name) as sch_name,trim(a.acode) as acode, trim(b.aname)||'('||trim(b.city)||')' as Account_Name,sUM(a.IAMOUNT) AS Total_Qty from IVOUCHER a,famst b,typegrp c  where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " GROUP BY a.acode, b.aname,b.CITY,A.TYPE,trim(c.name) ,TRIM(B.BSSCH)";
                //    mq1 = "select  '-' as fstr,'-' as gstr,'TOTAL' AS sch_name,'-' AS sch_Code,SUM(a.IAMOUNT) AS Total_Qty from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " union all select TRIM(B.BSSCH) as fstr,'-' as gstr,trim(c.name) as sch_name,trim(b.bssch) as sch_Code,sUM(a.IAMOUNT) AS Total_Qty  from IVOUCHER a,famst b,typegrp c  where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + " GROUP BY trim(c.name) ,TRIM(B.BSSCH)";

                //    fgen.drillQuery(0, mq1, frm_qstr);

                //    //mq2 = "SELECT '-' as fstr,TRIM(A.ACODE)||TRIM(B.BSSCH) as gstr,a.type,trim(b.aname) as party,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,SUM(a.IAMOUNT) AS FINISHED_GOODS,SUM(a.IAMOUNT) AS Total_Qty from IVOUCHER A ,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + part_cd + " and A.vchdate " + xprdrange + " and " + party_cd + " group by trim(a.acode),trim(b.bssch),a.vchnum,a.type,to_char(a.vchdate,'dd/mm/yyyy') ,trim(b.aname)";
                //    mq2 = "select TRIM(A.ACODE)||TRIM(B.BSSCH) as fstr,TRIM(B.BSSCH) as gstr, trim(b.aname)||'('||trim(b.city)||')' as Account_Name,trim(a.acode) as acode,sUM(a.IAMOUNT) AS Total_Qty from IVOUCHER a,famst b,typegrp c  where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " and " + party_cd + "  GROUP BY a.acode, b.aname,b.CITY ,TRIM(B.BSSCH)";
                //    fgen.drillQuery(1, mq2, frm_qstr);
                //    mq3 = "select '-' as fstr,TRIM(A.ACODE)||TRIM(B.BSSCH) as gstr,a.type,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,SUM(a.IAMOUNT) AS FINISHED_GOODS,SUM(a.IAMOUNT) AS Total_Qty from IVOUCHER a,famst b,typegrp c  where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "'  and " + part_cd + " and a.vchdate " + xprdrange + " group by trim(a.acode),trim(b.bssch),a.vchnum,a.type,to_char(a.vchdate,'dd/mm/yyyy')";
                //    fgen.drillQuery(2, mq3, frm_qstr);
                //    fgen.Fn_DrillReport("Party Wise Column(Sales Analysis) From " + fromdt + " and to " + todt + " ", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT25":////
                //    #region Items Sales Analysis Report==doing changes in qry
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    mq3 = "";
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        mq3 = "b.bssch like '%' ";
                //        party_cd = "C.bssch like '%' ";
                //    }
                //    else
                //    {
                //        mq3 = "b.bssch in (" + party_cd + ") ";
                //        party_cd = "C.bssch in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "a.acode like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "a.acode in (" + part_cd + ") ";
                //    }
                //    mq1 = "select '-' as fstr,'-' as gstr,'TOTAL' AS ITEM_NAME,'-' as unit,sum(a.iqtyout) as Net_Sale_Qty,sum(A.iamount) as net_sale_Amt from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate  " + xprdrange + " AND " + part_cd + " and " + mq3 + "  union all SELECT trim(A.ICODE) as fstr,'-' as gstr,A.ITEM_NAME,A.UNIT,A.Net_Sale_Qty,A.Net_Sale_Amt FROM (select trim(a.icode) as icode,trim(b.iname) as item_name,b.unit,SUM(a.iqtyout) AS Net_Sale_Qty,SUM(a.IAMOUNT) AS Net_Sale_Amt from IVOUCHER a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "'  and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + party_cd + "  AND " + part_cd + " GROUP BY a.icode,b.iname,b.unit) A ";
                //    fgen.drillQuery(0, mq1, frm_qstr);
                //    //mq2 = "SELECT '-' as fstr,trim(a.icode) as gstr,trim(b.aname) as party,a.type,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,sum(a.iqtyout) as net_Sale_Qty,sum(A.iamount) as Net_Sale_Amt FROM ivoucher A ,famst b where trim(a.acode)=trim(b.acode) and " + part_cd + " AND " + mq3 + " and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " group by trim(a.icode),trim(b.aname),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.type order by a.vchnum desc";
                //    mq2 = "SELECT trim(a.acode)||trim(a.icode) as fstr,trim(a.icode) as gstr,trim(b.aname) as party,sum(a.iqtyout) as net_Sale_Qty,sum(A.iamount) as Net_Sale_Amt FROM ivoucher A ,famst b where trim(a.acode)=trim(b.acode) and " + part_cd + " AND " + mq3 + " and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " group by trim(a.icode),trim(b.aname),trim(a.acode) order by fstr";
                //    fgen.drillQuery(1, mq2, frm_qstr);
                //    mq3 = "SELECT  '-' as fstr,trim(a.acode)||trim(a.icode) as gstr,a.type,a.vchnum as bill_no,to_char(a.vchdate,'dd/mm/yyyy') as bill_date,sum(a.iqtyout) as net_Sale_Qty,sum(A.iamount) as Net_Sale_Amt FROM ivoucher A ,famst b where trim(a.acode)=trim(b.acode) and " + part_cd + " AND " + mq3 + " and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " group by trim(a.icode),trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.type order by bill_no desc";
                //    fgen.drillQuery(2, mq3, frm_qstr);
                //    fgen.Fn_DrillReport("Items Sales Analysis From " + fromdt + " and to " + todt + " ", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT26":
                //    #region Sales Report==doing changes in qry
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "A.ICODE like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "A.ICODE in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "b.bssch in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select trim(b.aname) as party,A.Icode,C.iNAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,morder from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " ) a,famst b,Item C where trim(a.acode)=trim(b.acode) and TRIM(A.Icode)=TRIM(C.icode) and " + party_cd + "  group by A.icode,trim(b.aname),C.iNAME order by C.iNAME";
                //    SQuery = "select trim(b.aname) as party,A.Icode,C.iNAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,morder from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst b,Item C,TYPEGRP T where trim(a.acode)=trim(b.acode) and TRIM(A.Icode)=TRIM(C.icode) and trim(b.bssch)=trim(t.type1) and t.id='A' and " + party_cd + " AND " + part_cd + " group by A.icode,trim(b.aname),C.iNAME order by C.iNAME";
                //    SQuery = "select a.type,trim(t.name) as sch_name,trim(b.aname) as party,A.Icode,C.iNAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select type,trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,morder from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst b,Item C,TYPEGRP T where trim(a.acode)=trim(b.acode) and TRIM(A.Icode)=TRIM(C.icode) and trim(b.bssch)=trim(t.type1) and t.id='A' and " + party_cd + " AND " + part_cd + " group by A.icode,trim(b.aname),C.iNAME,a.type,trim(t.name) order by a.type,C.iNAME";
                //    //=====
                //    SQuery = "select '-' AS type,'TOTAL' as sch_name,'-' as party,'-' as Icode,'-' as iNAME,sum((Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end)) as apr,sum((Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end)) as may,sum((Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end)) as jun,sum((Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end)) as jul,sum((Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end)) as aug,sum((Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end)) as sep,sum((Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end)) as oct,sum((Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end)) as nov,sum((Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end)) as dec,sum((Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end)) as jan,sum((Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end)) as feb,sum((Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end)) as mar,sum(a.iqtyout) as total from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " and " + party_cd + " AND " + part_cd + " union all select a.type,trim(t.name) as sch_name,trim(b.aname) as party,A.Icode,C.iNAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select type,trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,morder from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst b,Item C,TYPEGRP T where trim(a.acode)=trim(b.acode) and TRIM(A.Icode)=TRIM(C.icode) and trim(b.bssch)=trim(t.type1) and t.id='A' and " + party_cd + " AND " + part_cd + "  group by A.icode,trim(b.aname),C.iNAME,a.type,trim(t.name) ";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Sales 2 (Sales Analyis)", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT27":
                //    #region Item Grp Wise (Sales Analysis)
                //    ph_tbl = new DataTable();
                //    ph_tbl.Columns.Add("Type", typeof(string));
                //    ph_tbl.Columns.Add("Sch_name", typeof(string));
                //    ph_tbl.Columns.Add("Item_Group", typeof(string));
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "A.mgcode like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "A.mgcode in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "b.bssch in (" + part_cd + ") ";
                //    }
                //    dr1 = null;
                //    // SQuery = "select a.mgcode,C.NAME,a.acode,b.aname,sum(a.qty) as qty from (select substr(trim(a.icode),1,2) as mgcode,trim(a.acode) as acode,nvl(a.iqtyout,0) as qty from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + ") a,famst b,type c where trim(A.acode)=trim(b.acode) and trim(a.mgcode)=trim(c.type1) and c.id='Y' and " + party_cd + " group by a.mgcode,a.acode,c.name,b.aname order by a.mgcode";
                //    SQuery = "select a.mgcode,C.NAME,a.acode,b.aname,sum(a.qty) as qty from (select substr(trim(a.icode),1,2) as mgcode,trim(a.acode) as acode,nvl(a.iqtyout,0) as qty from ivoucher a where a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + ") a,famst b,type c,typegrp d where trim(A.acode)=trim(b.acode) and trim(a.mgcode)=trim(c.type1) and c.id='Y' and trim(b.bssch)=trim(d.type1) and d.id='A' and " + party_cd + " AND " + part_cd + " group by a.mgcode,a.acode,c.name,b.aname order by a.mgcode";
                //    SQuery = "select A.TYPE,TRIM(D.NAME) as sch_name,a.mgcode,C.NAME,a.acode,b.aname,sum(a.qty) as qty from (select type,substr(trim(a.icode),1,2) as mgcode,trim(a.acode) as acode,nvl(a.iqtyout,0) as qty from ivoucher a where a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + ") a,famst b,type c,typegrp d where trim(A.acode)=trim(b.acode) and trim(a.mgcode)=trim(c.type1) and c.id='Y' and trim(b.bssch)=trim(d.type1) and d.id='A' and " + party_cd + " AND " + part_cd + " group by a.mgcode,a.acode,c.name,b.aname ,A.TYPE,TRIM(D.NAME) order by a.type,a.mgcode";
                //    dt = new DataTable();
                //    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                //    if (dt.Rows.Count > 0)
                //    {
                //        //=============================loop for dynamic columns
                //        view1imx = new DataView(dt);
                //        dtdrsimx = new DataTable();
                //        dtdrsimx = view1imx.ToTable(true, "aname"); //
                //        for (int j = 0; j < dtdrsimx.Rows.Count; j++)
                //        {
                //            ph_tbl.Columns.Add(dtdrsimx.Rows[j]["aname"].ToString().Trim() + "_Qty", typeof(double));
                //        }
                //        ph_tbl.Columns.Add("Total_Qty", typeof(double));

                //        if (dt.Rows.Count > 0)
                //        {
                //            DataView view1im = new DataView(dt);
                //            DataTable dtdrsim = new DataTable();
                //            dtdrsim = view1im.ToTable(true, "mgcode"); //MAIN                        
                //            foreach (DataRow dr0 in dtdrsim.Rows)
                //            {
                //                dr1 = ph_tbl.NewRow();
                //                dr1["Type"] = fgen.seek_iname_dt(dt, "mgcode='" + dr0["mgcode"].ToString().Trim() + "'", "type");
                //                dr1["Sch_name"] = fgen.seek_iname_dt(dt, "mgcode='" + dr0["mgcode"].ToString().Trim() + "'", "sch_name");
                //                dr1["Item_Group"] = fgen.seek_iname_dt(dt, "mgcode='" + dr0["mgcode"].ToString().Trim() + "'", "NAME");
                //                DataView viewim = new DataView(dt, "mgcode='" + dr0["mgcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //                dt3 = viewim.ToTable();
                //                db = 0;
                //                for (int m = 0; m < dt3.Rows.Count; m++)
                //                {
                //                    dr1[dt3.Rows[m]["aname"].ToString().Trim() + "_Qty"] = fgen.make_double(dt3.Rows[m]["qty"].ToString().Trim());
                //                    db += fgen.make_double(dt3.Rows[m]["qty"].ToString().Trim());//for maingrp wise total qty
                //                }
                //                dr1["Total_Qty"] = db;
                //                ph_tbl.Rows.Add(dr1);
                //            }
                //        }

                //        if (ph_tbl.Rows.Count > 0)
                //        {
                //            Session["send_dt"] = ph_tbl;
                //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                //            fgen.Fn_open_rptlevel("Item Grp Wise (Sales Analysis)", frm_qstr);
                //        }
                //    }
                //    else
                //    {
                //        fgen.msg("-", "AMSG", "Data Not Found");
                //    }
                //    #endregion
                //    break;

                //case "RPT28":
                //    #region Party Wise Sales Analysis====new report
                //    ph_tbl = new DataTable();
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "a.acode like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "a.acode in (" + part_cd + ") ";
                //    }
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "b.bssch in (" + party_cd + ") ";
                //    }
                //    dr1 = null;
                //    //SQuery = "select a.icode,C.iNAME,a.acode,b.aname,sum(a.qty) as qty from (select trim(a.icode) as icode,trim(a.acode) as acode,nvl(a.iqtyout,0) as qty from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + ") a,famst b,item c where trim(A.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and " + party_cd + " group by a.icode,a.acode,c.iname,b.aname order by a.acode";
                //    SQuery = "select a.icode,C.iNAME,a.acode,b.aname,sum(a.qty) as qty from (select trim(a.icode) as icode,trim(a.acode) as acode,nvl(a.iqtyout,0) as qty from ivoucher a where a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + ") a,famst b,item c,typegrp d where trim(A.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(b.bssch)=trim(d.type1) and d.id='A' and " + party_cd + " and " + part_cd + " group by a.icode,a.acode,c.iname,b.aname order by a.acode";
                //    SQuery = "select a.type,trim(d.name) as sch_name,a.icode,C.iNAME,a.acode,b.aname,sum(a.qty) as qty from (select type,trim(a.icode) as icode,trim(a.acode) as acode,nvl(a.iqtyout,0) as qty from ivoucher a where a.branchcd='" + mbr + "' and a.type IN (" + hf2.Value + ") and a.vchdate " + xprdrange + ") a,famst b,item c,typegrp d where trim(A.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(b.bssch)=trim(d.type1) and d.id='A' and " + party_cd + " and " + part_cd + " group by a.icode,a.acode,c.iname,b.aname,a.type,trim(d.name) order by a.type,a.acode";
                //    dt = new DataTable();
                //    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                //    //=============================loop for dynamic columns
                //    if (dt.Rows.Count > 0)
                //    {
                //        ph_tbl.Columns.Add("Type", typeof(string));
                //        ph_tbl.Columns.Add("Sch_Name", typeof(string));
                //        ph_tbl.Columns.Add("Party", typeof(string));
                //        view1imx = new DataView(dt);
                //        dtdrsimx = new DataTable();
                //        dtdrsimx = view1imx.ToTable(true, "iname"); //
                //        for (int j = 0; j < dtdrsimx.Rows.Count; j++)
                //        {
                //            ph_tbl.Columns.Add(dtdrsimx.Rows[j]["iname"].ToString().Trim(), typeof(double));
                //        }
                //        ph_tbl.Columns.Add("Total_Qty", typeof(double));
                //        //======================
                //        if (dt.Rows.Count > 0)
                //        {
                //            DataView view1im = new DataView(dt);
                //            DataTable dtdrsim = new DataTable();
                //            dtdrsim = view1im.ToTable(true, "acode"); //MAIN                        
                //            foreach (DataRow dr0 in dtdrsim.Rows)
                //            {
                //                dr1 = ph_tbl.NewRow();
                //                dr1["type"] = fgen.seek_iname_dt(dt, "acode='" + dr0["acode"].ToString().Trim() + "'", "type");
                //                dr1["sch_name"] = fgen.seek_iname_dt(dt, "acode='" + dr0["acode"].ToString().Trim() + "'", "sch_name");
                //                dr1["Party"] = fgen.seek_iname_dt(dt, "acode='" + dr0["acode"].ToString().Trim() + "'", "aNAME");
                //                DataView viewim = new DataView(dt, "acode='" + dr0["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //                dt3 = viewim.ToTable();
                //                db = 0;
                //                for (int m = 0; m < dt3.Rows.Count; m++)
                //                {
                //                    dr1[dt3.Rows[m]["iname"].ToString().Trim()] = fgen.make_double(dt3.Rows[m]["qty"].ToString().Trim());//FILL VALUE IN DYNAMIC COLM
                //                    db += fgen.make_double(dt3.Rows[m]["qty"].ToString().Trim());//for acode wise total qty
                //                }
                //                dr1["Total_Qty"] = db;
                //                ph_tbl.Rows.Add(dr1);
                //            }
                //        }
                //        if (ph_tbl.Rows.Count > 0)
                //        {
                //            Session["send_dt"] = ph_tbl;
                //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                //            fgen.Fn_open_rptlevel("Party Wise Sales Analysis Report From " + fromdt + " To " + todt + "", frm_qstr);
                //        }
                //    }
                //    else
                //    {
                //        fgen.msg("-", "AMSG", "Data Not Found");
                //    }
                //    #endregion
                //    break;

                //case "RPT3":
                //    #region  Group Item Std Date Report
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    mq3 = ""; mq4 = "";
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        mq4 = "f.BSSCH like '%' ";
                //        party_cd = "B.BSSCH like '%' ";
                //    }
                //    else
                //    {
                //        mq4 = "f.BSSCH in (" + party_cd + ") ";
                //        party_cd = "B.BSSCH in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        mq3 = "a.TYPE like '4%' ";
                //        part_cd = "TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        mq3 = "a.TYPE in (" + part_cd + ") ";
                //        part_cd = "TYPE in (" + part_cd + ") ";
                //    }
                //    //   SQuery = "select 'TOTAL' as TYPE,'-' AS sch_name,'-' as Main_Grp,'-' as unit,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq1 + " and " + party_cd + " and a.vchdate " + xprdrange + " union all select a.type,a.sch_name,TRIM(c.name) as Main_Grp,b.unit,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from (select trim(c.name) as sch_name,a.type,a.mgcode,a.icode,a.iqtyout,a.iamount,a.avg_price,a.acode from (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount,(case when nvl(iqtyout,0)!=0 then ROUND(nvl(iamount,0)/nvl(iqtyout,0),2) else 0 end) as avg_price from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,typeGRP c where TRIM(A.ACODE)=TRIM(B.ACODE) AND trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ") a,item b,type c where trim(A.icode)=trim(b.icode) and trim(A.mgcode)=trim(c.type1) and c.iD='Y' group by A.mgcode,TRIM(c.name),b.unit,a.sch_name,a.type ";//real                  
                //    mq1 = "select  '-' as fstr,'-' as gstr,'TOTAL' sch_name,'-' as Main_Grp,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and " + party_cd + " and a.vchdate " + xprdrange + " union all select trim(a.bssch) as fstr,'-' as gstr,a.sch_name,TRIM(c.name) as Main_Grp,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price from (select trim(c.name) as sch_name,a.type,a.mgcode,a.icode,a.iqtyout,a.iamount,a.avg_price,a.acode,b.bssch from (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount,(case when nvl(iqtyout,0)!=0 then ROUND(nvl(iamount,0)/nvl(iqtyout,0),2) else 0 end) as avg_price from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,typeGRP c where TRIM(A.ACODE)=TRIM(B.ACODE) AND trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ") a,type c where trim(A.mgcode)=trim(c.type1) and c.iD='Y' group by A.mgcode,TRIM(c.name),a.sch_name,a.type,trim(a.bssch) ";
                //    mq1 = "select * from (select  '-' as fstr,'-' as gstr,'TOTAL' sch_name,'-' as Main_Grp,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, '1' as set_no from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and " + party_cd + " and a.vchdate " + xprdrange + " union all select trim(a.mgcode)||trim(a.bssch) as fstr,'-' as gstr,trim(a.sch_name) as sch_name,TRIM(c.name) as Main_Grp,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, '2' as set_no from (select trim(c.name) as sch_name,a.type,a.mgcode,a.icode,a.iqtyout,a.iamount,a.avg_price,a.acode,b.bssch from (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount,(case when nvl(iqtyout,0)!=0 then ROUND(nvl(iamount,0)/nvl(iqtyout,0),2) else 0 end) as avg_price from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,typeGRP c where TRIM(A.ACODE)=TRIM(B.ACODE) AND trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ") a,type c where trim(A.mgcode)=trim(c.type1) and c.iD='Y' group by A.mgcode,TRIM(c.name),trim(a.sch_name) ,trim(a.mgcode)||trim(a.bssch)) order by set_no,sch_name,Main_Grp ";
                //    mq1 = "select * from (select  '-' as fstr,'-' as gstr,'TOTAL' as Main_Grp,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, '1' as set_no from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and " + party_cd + " and a.vchdate " + xprdrange + " union all select trim(a.mgcode) as fstr,'-' as gstr,TRIM(c.name) as Main_Grp,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, '2' as set_no from (select a.type,a.mgcode,a.icode,a.iqtyout,a.iamount,a.avg_price,a.acode,b.bssch from (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount,(case when nvl(iqtyout,0)!=0 then ROUND(nvl(iamount,0)/nvl(iqtyout,0),2) else 0 end) as avg_price from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,typeGRP c where TRIM(A.ACODE)=TRIM(B.ACODE) AND trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ") a,type c where trim(A.mgcode)=trim(c.type1) and c.iD='Y' group by A.mgcode,TRIM(c.name) ,trim(a.mgcode)) order by set_no,Main_Grp ";
                //    fgen.drillQuery(0, mq1, frm_qstr);
                //    mq2 = "select * from (select  '-' as fstr,'-' as gstr,'TOTAL' as sub_Grp_name,sum(a.iqtyout) as total_Qty,sum(a.iamount) as total_value,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, '1' as set_no from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and " + party_cd + " and a.vchdate " + xprdrange + "  union all select  trim(a.icode) as fstr, trim(a.mgcode) as gstr,TRIM(a.iname) as sub_Grp_name,sum(a.iqtyout) as net_Sale_Qty,sum(a.iamount) as net_sale_Amt,(case when SUM(nvl(a.iqtyout,0))!=0 then ROUND(SUM(nvl(a.iamount,0))/SUM(nvl(a.iqtyout,0)),2) else 0 end) as avg_price, '2' as set_no from (select a.type,a.mgcode,substr(trim(a.icode),1,4) as subgrp,a.icode,c.iname,a.iqtyout,a.iamount,a.avg_price,a.acode,b.bssch from (select type,substr(trim(icode),1,2) as mgcode,acode,icode,nvl(iqtyout,0) as iqtyout,nvl(iamount,0) as iamount,(case when nvl(iqtyout,0)!=0 then ROUND(nvl(iamount,0)/nvl(iqtyout,0),2) else 0 end) as avg_price from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,item c where TRIM(A.ACODE)=TRIM(B.ACODE) AND substr(trim(a.icode),1,4)=trim(c.icode) and length(trim(c.icode))=4  and " + party_cd + ") a  group by A.mgcode,TRIM(a.iname) ,trim(a.icode),trim(a.mgcode)) order by set_no,sub_Grp_name ";
                //    fgen.drillQuery(1, mq2, frm_qstr);
                //    SQuery = "select '-' as fstr,trim(a.icode) as gstr,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(b.iname) as item_name,b.unit,sum(a.iqtyout) as qty,sum(a.iamount) as value from ivoucher a,FAMST F,item b where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(b.icode) AND A.branchcd='" + mbr + "' and " + mq3 + "  and a.vchdate " + xprdrange + " and " + mq4 + "  group by a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') ,trim(b.iname) ,b.unit,trim(a.icode)";
                //    fgen.drillQuery(2, SQuery, frm_qstr);

                //    fgen.Fn_DrillReport("FG Main Grp Sales Analysis from " + fromdt + " and to " + todt + " ", frm_qstr);
                //    #endregion
                //    break;
                //case "RPT5":
                //    #region  Item Group Qtr (Sales Analysis)
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "B.BSSCH like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "B.BSSCH in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        part_cd = "TYPE in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select trim(b.NAME) AS ITEM_GROUP_NAME,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select substr(trim(icode),1,2) as mgcode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + ") a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and b.id='Y' group by A.mgcode,trim(B.NAME) order by ITEM_GROUP_NAME";
                //    SQuery = "select trim(b.NAME) AS ITEM_GROUP_NAME,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select a.mgcode,a.acode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select substr(trim(icode),1,2) as mgcode,acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " )a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and b.id='Y' group by A.mgcode,trim(B.NAME) order by ITEM_GROUP_NAME";
                //    SQuery = "select  a.type,a.sch_name,trim(b.NAME) AS ITEM_GROUP_NAME,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select a.type, trim(c.name) as sch_name,a.mgcode,a.acode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select type, substr(trim(icode),1,2) as mgcode,acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " )a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and b.id='Y' group by A.mgcode,trim(B.NAME), a.sch_name,a.type order by a.type,ITEM_GROUP_NAME";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Item Group Qtr (Sales Analysis)", frm_qstr);
                //    #endregion
                //    break;
                //case "RPT6":
                //    #region  Item Group Monthly Sale Qty Report
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "A.mgcode like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "A.mgcode in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "b.bssch in (" + part_cd + ") ";
                //    }
                //    // SQuery = "select b.NAME AS ITEM_GROUP_NAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select substr(trim(icode),1,2) as mgcode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " ) a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and b.id='Y' and " + party_cd + " group by B.NAME order by item_group_name";
                //    SQuery = "select b.NAME AS ITEM_GROUP_NAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select a.mgcode,a.acode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select substr(trim(icode),1,2) as mgcode,acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " )a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + part_cd + " ) a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and b.id='Y' and " + party_cd + " group by B.NAME order by item_group_name";
                //    SQuery = "select a.type,a.sch_name,b.NAME AS ITEM_GROUP_NAME,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select a.mgcode,a.acode,a.type,trim(c.name) as sch_name,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select type,substr(trim(icode),1,2) as mgcode,acode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " )a,famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + part_cd + " ) a,TYPE B where trim(a.mgcode)=trim(b.TYPE1) and b.id='Y' and " + party_cd + " group by B.NAME ,a.type,a.sch_name order by a.type,item_group_name";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Item Group Monthly Sale Qty Report", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT7":
                //    #region  Item Group-wise Sales Analysis
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "B.BSSCH like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "B.BSSCH in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        part_cd = "TYPE in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select TRIM(B.NAME) AS ITEM_GROUP,sum(A.amt) as SUNDRY_DEBTORS,SUM(A.QTY) AS TOTAL_QTY from (select trim(icode) as icode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as scode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " ) a,type B  where trim(a.mcode)=trim(b.type1) and b.id='Y'  group by TRIM(B.NAME),A.MCODE,A.SCODE order by A.MCODE,A.SCODE";
                //    SQuery = "select TRIM(B.NAME) AS ITEM_GROUP,sum(A.amt) as SUNDRY_DEBTORS,SUM(A.QTY) AS TOTAL_QTY from (SELECT A.ICODE,A.ACODE,A.MCODE,A.SCODE,A.QTY,A.AMT FROM (select trim(icode) as icode,ACODE,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as scode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + " ) A,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ")a,type B  where trim(a.mcode)=trim(b.type1) and b.id='Y'  group by TRIM(B.NAME),A.MCODE,A.SCODE order by A.MCODE,A.SCODE";
                //    SQuery = "select a.type,a.sch_name,d.iname as sname,TRIM(B.NAME) AS ITEM_GROUP,sum(A.amt) as SUNDRY_DEBTORS,SUM(A.QTY) AS TOTAL_QTY from (SELECT trim(c.name) as sch_name,a.type,A.ICODE,A.ACODE,A.MCODE,A.SCODE,A.QTY,A.AMT FROM (select trim(icode) as icode,ACODE,substr(trim(icode),1,2) as mcode,type,substr(trim(icode),1,4) as scode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + " ) A,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ")a,type B ,item d  where trim(a.mcode)=trim(b.type1) and b.id='Y' and trim(a.scode)=trim(d.icode) and length(trim(d.icode))=4  group by TRIM(B.NAME),A.MCODE,A.SCODE,a.type,a.sch_name ,d.iname order by a.type,A.MCODE,A.SCODE";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Item Group-wise Qtr(Sales Analysis)", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT8":
                //    #region  Item Sub Group Wise Std(Sales Analysis)
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    mq1 = "";
                //    mq3 = ""; mq4 = "";
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        mq4 = "f.BSSCH like '%' ";
                //        party_cd = "B.BSSCH like '%' ";
                //    }
                //    else
                //    {
                //        mq4 = "f.BSSCH in (" + party_cd + ") ";
                //        party_cd = "B.BSSCH in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        mq3 = "a.TYPE like '4%' ";
                //        part_cd = "TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        mq3 = "a.TYPE in (" + part_cd + ") ";
                //        part_cd = "TYPE in (" + part_cd + ") ";
                //    }
                //    //mq1 = "select * from (select '-' as fstr,'-' as gstr,'-' as ITEM_GROUP_NAME,'TOTAL' as sub_grp,sum(a.iqtyout) as total_Qty,1 as setno from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and a.vchdate " + xprdrange + " AND " + party_cd + "  union all  select trim(a.SCODE) as fstr,'-' as gstr,TRIM(B.NAME) AS ITEM_GROUP_NAME,d.iname as sub_grp,SUM(A.QTY) AS NET_SALE_QTY,2 as setno from (SELECT a.type,A.ICODE,a.acode,a.mcode,a.scode,a.qty,a.amt,b.bssch FROM (select type,trim(icode) as icode,ACODE,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as scode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt  from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate  " + xprdrange + " ) A,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " ) a,ITEM C,type B,item d  where TRIM(A.ICODE)=TRIM(C.ICODE) AND trim(a.mcode)=trim(b.type1) and b.id='Y' and trim(a.scode)=trim(d.icode) and length(trim(d.icode))=4 group by TRIM(B.NAME),d.iname,trim(a.SCODE)) order by setno,item_group_name,sub_grp";//OLD
                //    mq1 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as ITEM_GROUP_NAME,sum(a.iqtyout) as total_Qty,1 as setno from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + "  and a.vchdate " + xprdrange + " AND " + party_cd + "  union all  select trim(a.mcode) as fstr,'-' as gstr,TRIM(B.NAME) AS ITEM_GROUP_NAME,SUM(A.QTY) AS NET_SALE_QTY,2 as setno from (SELECT a.type,A.ICODE,a.acode,a.mcode,a.scode,a.qty,a.amt,b.bssch FROM (select type,trim(icode) as icode,ACODE,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as scode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt  from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + "  and vchdate   " + xprdrange + ") A,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + ") a,ITEM C,type B where TRIM(A.ICODE)=TRIM(C.ICODE) AND trim(a.mcode)=trim(b.type1) and b.id='Y' group by TRIM(B.NAME),trim(a.mcode)) order by setno,item_group_name";
                //    fgen.drillQuery(0, mq1, frm_qstr);
                //    mq2 = "select * from (select '-' as fstr,'-' as gstr,'TOTAL' as sub_grp_name,sum(a.iqtyout) as total_Qty,1 as setno from ivoucher a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + "  and a.vchdate " + xprdrange + " AND " + party_cd + "  union all  select trim(a.bssch)||trim(a.Scode) as fstr,trim(a.mcode) as gstr,c.iname as sub_grp_name,SUM(A.QTY) AS NET_SALE_QTY,2 as setno from (SELECT a.type,A.ICODE,a.acode,a.mcode,a.scode,a.qty,a.amt,b.bssch FROM (select type,trim(icode) as icode,ACODE,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as scode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt  from IVOUCHER where branchcd='" + mbr + "'  and " + part_cd + " and vchdate  " + xprdrange + " ) A,famst B where trim(a.acode)=trim(b.acode)  and " + party_cd + ") a,ITEM C where TRIM(A.SCODE)=TRIM(C.ICODE) and length(trim(c.icode))=4  group by trim(a.mcode),c.iname,trim(a.Scode),trim(a.bssch) ) order by setno,sub_grp_name";
                //    fgen.drillQuery(1, mq2, frm_qstr);
                //    mq3 = "select '-' as fstr,trim(f.bssch)||substr(trim(a.icode),1,4) as gstr,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(b.iname) as item_name,b.unit,sum(a.iqtyout) as qty from ivoucher a,FAMST F,item b where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(b.icode)  AND A.branchcd='" + mbr + "' and " + mq3 + " and A.vchdate " + xprdrange + "  and " + mq4 + " group by substr(trim(a.icode),1,4),a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') ,trim(b.iname) ,b.unit,trim(f.bssch)";
                //    fgen.drillQuery(2, mq3, frm_qstr);
                //    fgen.Fn_DrillReport("Item Sub Group Wise Std(Sales Analysis) from " + fromdt + " and to " + todt + " ", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT9":
                //    #region  Item Std Qtr Report
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "B.BSSCH like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "B.BSSCH in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        part_cd = "TYPE in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select trim(b.iNAME) AS ITEM_detail,b.unit,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + ") a,item B where trim(a.icode)=trim(b.icode)  group by trim(B.iNAME),b.unit order by item_Detail";
                //    SQuery = "select trim(b.iNAME) AS ITEM_detail,b.unit,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select a.acode,a.icode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " ) a,item B where trim(a.icode)=trim(b.icode)  group by trim(B.iNAME),b.unit order by item_Detail";
                //    SQuery = "select a.type,a.sch_name,trim(b.iNAME) AS ITEM_detail,b.unit,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_qty  from (select a.type,trim(c.name) as sch_name, a.acode,a.icode,a.apr,a.may,a.jun,a.jul,a.aug,a.sep,a.oct,a.nov,a.dec,a.jan,a.feb,a.mar from (select type,acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + " and vchdate " + xprdrange + ") a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " ) a,item B where trim(a.icode)=trim(b.icode) group by trim(B.iNAME),b.unit,a.type,a.sch_name order by a.type,item_Detail";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Item Std Qtr Report", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT10":
                //    #region  Item Wise Columnar
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    mq3 = "";
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "B.BSSCH like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "B.BSSCH in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        mq3 = "a.TYPE like '4%' ";
                //        part_cd = "TYPE like '4%' ";
                //    }
                //    else
                //    {
                //        mq3 = "a.TYPE in (" + part_cd + ") ";
                //        part_cd = "TYPE in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select TRIM(d.iname) as item,d.unit,sum(a.amt) as sundry_Debtors,sum(a.qty) as net_total_qty from (select trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " ) a,item d where trim(a.icode)=trim(d.icode)  group by TRIM(d.iname),d.unit order by item";
                //    SQuery = "select TRIM(d.iname) as item,d.unit,sum(a.amt) as sundry_Debtors,sum(a.qty) as net_total_qty from (select a.acode,a.icode,a.qty,a.amt from (select trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + "  and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " )a,item d where trim(a.icode)=trim(d.icode)  group by TRIM(d.iname),d.unit order by item";
                //    SQuery = "select a.type,a.sch_name,TRIM(d.iname) as item,d.unit,sum(a.amt) as Value,sum(a.qty) as total_qty from (select a.type,trim(c.name) as sch_name, a.acode,a.icode,a.qty,a.amt from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + "  and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " )a,item d where trim(a.icode)=trim(d.icode)  group by TRIM(d.iname),d.unit,a.type,a.sch_name order by a.type,item";
                //    //====================                   
                //    mq1 = "select '-' as fstr,'-' as gstr,'TOTAL' as type,'-' as sch_name,'-' as item,'-' as unit,sum(a.iamount) as value,sum(a.iqtyout) as total_qty from ivoucher a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and " + party_cd + " and a.vchdate " + xprdrange + "  union all  select trim(a.bssch) as fstr,'-' as gstr,a.type,a.sch_name,TRIM(d.iname) as item,d.unit,sum(a.amt) as Value,sum(a.qty) as total_qty from (select a.type,trim(c.name) as sch_name, a.acode,a.icode,a.qty,a.amt,trim(b.bssch) as bssch from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + "  and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " )a,item d where trim(a.icode)=trim(d.icode)  group by trim(a.bssch),TRIM(d.iname),d.unit,a.type,a.sch_name ";//with type
                //    mq1 = "select '-' as fstr,'-' as gstr,'TOTAL'  sch_name,'-' as item,'-' as unit,sum(a.iamount) as value,sum(a.iqtyout) as total_qty from ivoucher a,famst b where trim(A.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and " + mq3 + " and " + party_cd + " and a.vchdate " + xprdrange + "  union all  select trim(a.bssch) as fstr,'-' as gstr,a.sch_name,TRIM(d.iname) as item,d.unit,sum(a.amt) as Value,sum(a.qty) as total_qty from (select a.type,trim(c.name) as sch_name, a.acode,a.icode,a.qty,a.amt,trim(b.bssch) as bssch from (select type,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and " + part_cd + "  and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " )a,item d where trim(a.icode)=trim(d.icode)  group by trim(a.bssch),TRIM(d.iname),d.unit,a.sch_name ";//without type
                //    fgen.drillQuery(0, mq1, frm_qstr);
                //    mq2 = "select '-' as fstr,trim(b.bssch) as gstr,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,sum(a.iamount) as value,sum(a.iqtyout) as qty from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + mbr + "' and " + mq3 + "  and a.vchdate " + xprdrange + "  group by trim(b.bssch),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') ,to_char(a.vchdate,'yyyymmdd') ,a.type order by to_char(a.vchdate,'yyyymmdd')";
                //    fgen.drillQuery(1, mq2, frm_qstr);
                //    fgen.Fn_DrillReport("Item Wise Columnar Report from " + fromdt + " and to " + todt + " ", frm_qstr);

                //    #endregion
                //    break;

                //case "RPT16":
                //    #region Party Group Qtr(Sales Analysis)
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "A.aCODE like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "A.aCODE in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "b.bssch in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select TRIM(c.name) as account_group_name,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " group by TRIM(c.name) order by account_group_name";
                //    SQuery = "select TRIM(c.name) as account_group_name,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " and " + part_cd + " group by TRIM(c.name) order by account_group_name";
                //    SQuery = "select a.type,trim(c.name) as sch_name,TRIM(c.name) as account_group_name,sum(a.apr+a.may+a.jun) as Q_1,sum(a.jul+a.aug+a.sep) as Q_2,sum(a.oct+a.nov+a.dec) as Q_3,sum(a.jan+a.feb+a.mar) as Q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select TYPE,trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " and " + part_cd + " group by TRIM(c.name),a.type,trim(c.name)  order by A.TYPE,account_group_name";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Party Group Qtr(Sales Analysis)", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT17":
                //    #region Party Group Std. all Group(Sales Analysis)
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "b.bssch in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "A.aCODE like '16%' ";
                //    }
                //    else
                //    {
                //        part_cd = "A.aCODE in (" + part_cd + ") ";
                //    }
                //    //  SQuery = "select  a.type,TRIM(c.name) as account_group_name,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select TYPE,trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " and " + part_cd + " group by a.type,trim(c.name) order by A.TYPE,account_group_name";
                //    //////========new  ===============                 
                //    SQuery = "select * from ( select 'TOTAL' as account_group_name,sum((Case when to_char(vchdate,'mm')='04' then iamount else 0 end)) as apr,sum((Case when to_char(vchdate,'mm')='05' then iamount else 0 end)) as may,sum((Case when to_char(vchdate,'mm')='06' then iamount else 0 end)) as jun,sum((Case when to_char(vchdate,'mm')='07' then iamount else 0 end)) as jul,sum((Case when to_char(vchdate,'mm')='08' then iamount else 0 end)) as aug,sum((Case when to_char(vchdate,'mm')='09' then iamount else 0 end)) as sep,sum((Case when to_char(vchdate,'mm')='10' then iamount else 0 end)) as oct,sum((Case when to_char(vchdate,'mm')='11' then iamount else 0 end)) as nov,sum((Case when to_char(vchdate,'mm')='12' then iamount else 0 end)) as dec,sum((Case when to_char(vchdate,'mm')='01' then iamount else 0 end)) as jan,sum((Case when to_char(vchdate,'mm')='02' then iamount else 0 end)) as feb,sum((Case when to_char(vchdate,'mm')='03' then iamount else 0 end)) as mar ,sum(a.iamount) as total_AMT,1 as setno from ivoucher a,famst b where trim(a.acode)=trim(b.aCODE) and a.branchcd='" + mbr + "' and a.type in (" + hf2.Value + ") and a.vchdate " + xprdrange + " and " + party_cd + " and " + part_cd + " union all select  tRIM(c.name) as account_group_name,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT,2 as setno  from (select type,trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " and " + part_cd + " group by trim(c.name)) order by setno,account_group_name ";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Party Group Std. all Group(Sales Analysis)", frm_qstr);
                //    #endregion
                //    break;

                //case "RPT18":
                //    #region Party Group Wise Std Report
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "b.bssch in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "A.aCODE like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "A.aCODE in (" + part_cd + ") ";
                //    }
                //    // SQuery = "select TRIM(d.name) as account_group_name,sum(a.qty) as net_sale_qty,c.unit,sum(A.amt) as net_sale_Amt  from (select trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,famst B,item c,typegrp d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(b.bssch)=trim(d.type1) and d.id='A' and " + party_cd + "  and " + part_cd + " group by TRIM(d.name),c.unit order by account_group_name";
                //    // SQuery = "select A.TYPE,TRIM(d.name) as account_group_name,sum(a.qty) as net_sale_qty,c.unit,sum(A.amt) as net_sale_Amt  from (select TYPE,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,famst B,item c,typegrp d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(b.bssch)=trim(d.type1) and d.id='A' and " + party_cd + "  and " + part_cd + " group by TRIM(d.name),c.unit,A.TYPE order by A.TYPE,account_group_name";
                //    ///=======NEW CHANGE
                //    SQuery = "select 'TOTAL' AS account_group_name,sum(a.iqtyout) as net_sale_qty,'-' AS UNIT,sum(A.iamount) as net_sale_Amt from IVOUCHER A,FAMST B where TRIM(a.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "' and A.type in (" + hf2.Value + ") and A.vchdate " + xprdrange + " AND " + party_cd + " and " + part_cd + " UNION ALL select TRIM(d.name) as account_group_name,sum(a.qty) as net_sale_qty,c.unit,sum(A.amt) as net_sale_Amt  from (select TYPE,trim(acode) as acode,trim(icode) as icode,nvl(iqtyout,0) as qty,nvl(iamount,0) as amt from IVOUCHER where branchcd='" + mbr + "' and type in (" + hf2.Value + ") and vchdate " + xprdrange + ") a,famst B,item c,typegrp d where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(b.bssch)=trim(d.type1) and d.id='A' AND " + party_cd + " and " + part_cd + "  group by TRIM(d.name),c.unit,A.TYPE ";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Party Group Wise Std Report", frm_qstr);
                //    #endregion
                //    break;
                //case "RPT20":
                //    #region Party Wise Qtr(Sales Analysis)
                //    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //    if (party_cd.Trim().Length <= 1)
                //    {
                //        party_cd = "A.aCODE like '%' ";
                //    }
                //    else
                //    {
                //        party_cd = "A.aCODE in (" + party_cd + ") ";
                //    }
                //    if (part_cd.Trim().Length <= 1)
                //    {
                //        part_cd = "b.bssch like '%' ";
                //    }
                //    else
                //    {
                //        part_cd = "b.bssch in (" + part_cd + ") ";
                //    }
                //    //SQuery = "select TRIM(b.aname)||'('||trim(b.zncode)||')'  as party,sum(a.apr+a.may+a.jun) as q_1,sum(a.jul+a.aug+a.sep) as q_2,sum(a.oct+a.nov+a.dec) as q_3,sum(a.jan+a.feb+a.mar) as q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " ) a,famst B where trim(a.acode)=trim(b.acode) and " + party_cd + "  group by TRIM(b.aname)||'('||trim(b.zncode)||')'  order by party";
                //    SQuery = "select TRIM(b.aname)||'('||trim(b.zncode)||')'  as party,sum(a.apr+a.may+a.jun) as q_1,sum(a.jul+a.aug+a.sep) as q_2,sum(a.oct+a.nov+a.dec) as q_3,sum(a.jan+a.feb+a.mar) as q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type IN (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " AND " + part_cd + " group by TRIM(b.aname)||'('||trim(b.zncode)||')'  order by party";
                //    SQuery = "select a.type,trim(c.name) as sch_name,TRIM(b.aname)||'('||trim(b.zncode)||')'  as party,sum(a.apr+a.may+a.jun) as q_1,sum(a.jul+a.aug+a.sep) as q_2,sum(a.oct+a.nov+a.dec) as q_3,sum(a.jan+a.feb+a.mar) as q_4,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total_AMT  from (select TYPE,trim(acode) as acode,(Case when to_char(vchdate,'mm')='04' then iamount else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iamount else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iamount else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar from IVOUCHER where branchcd='" + mbr + "' and type IN (" + hf2.Value + ") and vchdate " + xprdrange + " ) a,famst B,typegrp c where trim(a.acode)=trim(b.acode) and trim(b.bssch)=trim(c.type1) and c.id='A' and " + party_cd + " AND " + part_cd + " group by TRIM(b.aname)||'('||trim(b.zncode)||')',a.type,trim(c.name)  order by a.type,party";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel("Party Wise Qtr(Sales Analysis)", frm_qstr);
                //    #endregion
                //    break;
                #endregion

            }
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Server.ClearError();
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr, false);
    }
    protected void rep1_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT1";
        show_data();
    }
    protected void rep2_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT2";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT2");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep3_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT3";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT3");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep4_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT4";
        show_data();
    }
    protected void rep5_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT5";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT5");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep6_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT6";
        show_data();
    }
    protected void rep7_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT7";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT7");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep8_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT8";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT8");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep9_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT9";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT9");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep10_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT10");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep11_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT11";
        show_data();
    }
    protected void rep12_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT12";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT12");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep13_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT13";
        show_data();
    }
    protected void rep14_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT14";
        show_data();
    }
    protected void rep15_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT15";
        show_data();
    }
    /// ==done
    protected void rep16_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT16";
        show_data();
    }
    protected void rep17_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT17";
        show_data();
    }
    protected void rep18_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT18";
        show_data();
    }
    protected void rep19_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT19";
        show_data();
    }
    protected void rep20_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT20";
        show_data();
    }
    protected void rep21_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT21";
        show_data();
    }
    protected void rep22_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT22";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT22");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    //==================================suman's report but doing changes in all
    protected void rep23_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT23";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT23");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep24_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT24";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT24");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep25_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT25";
        show_data();
    }
    protected void rep26_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT26";
        show_data();
    }
    protected void rep27_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT27";
        show_data();
    }
    protected void rep28_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT28";
        show_data();
    }
    protected void rep29_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT29";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT29");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep30_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT30";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT30");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep31_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT31";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT31");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void rep32_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT32";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT32");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
}