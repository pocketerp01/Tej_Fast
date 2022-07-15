using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_emktg : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, frm_cocd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID, co_cd;
    DataRow dr1;
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
                co_cd = frm_cocd;
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
                case "F49132":
                case "F49133":
                    SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V' AND type1 like '4%' AND TYPE1 IN ('4F','4E','4C') ORDER BY code";
                    header_n = "Select Sale Type";
                    break;
                case "F49202":
                case "F49203":
                    SQuery = "select type1 as fstr,name ,type1 as type from type where id='V' and type1 like '4%' order by type1";
                    header_n = "Select Type";
                    break;

                case "F49207": // for order acceptance2  //
                    SQuery = "select acode as fstr,aname as party_name,acode as party_code ,branchcd as branchcd from famst  where substr(acode,1,2) in ('16','02') order by acode";
                    header_n = "Select Party";
                    i0 = 1;
                    break;
                case "F49134":
                case "F49135":
                case "F49136":
                case "F49153":
                case "F49154":
                case "F49155":
                case "F49156":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_mseek(header_n, frm_qstr);
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


            if (val == "F49132" || val == "F49133")
            {
                hf1.Value = value1;
                fgen.Fn_open_Act_itm_prd("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F49133":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Customers";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,A.ACODE AS CODE,B.ANAME AS NAME FROM somas A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.TYPE='4F' ORDER BY CODE"; //ONLY WAHI PARTY JINKA SO BNA HUA H
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hfcode.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;
                    case "F49134":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Products";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ICODE) AS FSTR,A.ICODE AS CODE,B.INAME AS NAME FROM somas A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.TYPE='4F' ORDER BY CODE";//ONLY WAHI ITEM JINKA SO BAN HUA H
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hfcode.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F49202":
                    case "F49203":
                        hf1.Value = value1;
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                        break;

                    case "F49207":
                        hf1.Value = value1;
                        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                        SQuery = "select a.pordno as po_no,to_char(a.porddt,'dd/mm/yyyy') as po_order_dt,trim(a.cpartno) as sage_part_no,a.acode,b.aname as party_name, a.qtyord as order_qty,a.qtysupp as qty_planned ,to_char(a.cu_chldt,'dd/mm/yyyy') as delivery_required_by_PArty,'' as deliver_accepted_by_sage,b.dlvtime as shipment_from_factory_by,'' as sailing_from,a.remark  from somas a , famst b  where trim(a.acode)=trim(b.acode) and  a.branchcd='00' and a.type='4F' and porddt " + xprdrange + " and a.acode in (" + hf1.Value + ") order by po_no";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Order Acceptance detailed for the Period " + fromdt + " to " + todt, frm_qstr);
                        break;

                    default:
                        break;
                }
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            // ELSE STATEMENT IS ENDING HERE
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

            // after prdDmp this will run             
            switch (val)
            {
                case "F49132":
                    // hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TYPSTRING");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    hfcode.Value = hf1.Value;
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type like '4%' and a.type in ('4F','4E','4C') and  a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    else
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.type in ('4F','4E','4C') and  a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Master SO Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49133":
                    //hfcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TYPSTRING");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    hfcode.Value = hf1.Value;
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type like '4%' and a.type in ('4F','4E','4C') and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    else
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.type in ('4F','4E','4C') and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supply SO Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49134":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, val, branch_Cd, "a.type='46' and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supply Schedule Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49135":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    SQuery = "Select b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,Sum(a.Prd1) as Schedule_Qty,sum(a.prd2)as Despatch_Qty,(Sum(a.Prd1)-sum(a.prd2)) as Difference,c.Unit,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Completed' else 'Pending' end) as Sch_Position,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode from (Select trim(a.acode) as Acode,trim(a.Icode) as Icode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + mbr + "' and a.type='46' and a.vchdate " + xprdrange + " group by trim(a.acode),trim(A.icode) union all  Select trim(a.acode) as Acode,trim(a.Icode) as Icode,0 as prd1,Sum(a.iqtyout) as prd2 from ivoucher a where a.branchcd='" + mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + xprdrange + " group by trim(a.acode),trim(a.Icode)) a, famst b,item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by b.Aname,c.Iname,c.Unit,c.cpartno,trim(A.icode),trim(A.acode) Order by B.aname,c.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule Vs Despatch Data Search(Dom.) Summary for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F49136":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    party_cd = ulvl == "M" ? uname : party_cd;
                    SQuery = "Select a.Ordno as SO_No,to_char(A.orddt,'dd/mm/yyyy') as SO_DT,b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,a.qtyord as Order_Qty,a.Soldqty as Despatch_Qty,a.bal_qty as Pend_Qty,c.Unit,round(a.bal_qty*a.srate,2) as Pend_Value,a.Pordno as Cust_po_no,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode,to_chaR(a.orddt,'yyyymmdd') as VDD from wbvu_pending_so a, famst b,item c where a.branchcd='" + mbr + "' and a.orddt " + xprdrange + " and trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' and a.bal_qty>0 Order by VDD,a.ordno,B.aname,c.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Order Search(Dom.) Summary for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49153":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "qtyord";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then " + db_fld + "  else 0 end) as mar  from sOMAS where branchcd='" + mbr + "' and type like '4%' and type in ('4F','4E','4C') and ORDDT " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Order Qty Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F49154":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "(qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(ORDDT,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then " + db_fld + "  else 0 end) as mar  from sOMAS where branchcd='" + mbr + "' and type like '4%' and type in ('4F','4E','4C') and ORDDT " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Order Value Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F49155":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total,0)";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(vchdate,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then " + db_fld + "  else 0 end) as mar  from schedule where branchcd='" + mbr + "' and type like '46%' and vchdate " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Schedule Qty Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F49156":

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    db_fld = "nvl(total*irate,0)";
                    SQuery = "SELECT C.ANAME as Customer_Name,b.INAME as Item_Name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as Total_Qty,sum(a.apr) as Apr,sum(a.may) as May,sum(a.jun) as Jun,sum(a.jul) as Jul,sum(a.aug) as Aug,sum(a.sep) as Sep,sum(a.oct) as Oct,sum(a.nov) as Nov,sum(a.dec) as Dec,sum(a.jan) as Jan,sum(a.feb) as Feb,sum(a.mar) as Mar,b.cpartno,b.hscode,a.icode as Item_code,A.ACODE from ( select  ACODE,icode,(Case when to_char(vchdate,'mm')='04' then " + db_fld + "   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then " + db_fld + " else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then " + db_fld + " else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then " + db_fld + "  else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then " + db_fld + "  else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then " + db_fld + "  else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then " + db_fld + "  else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then " + db_fld + "  else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then " + db_fld + "  else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then " + db_fld + "  else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then " + db_fld + "  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then " + db_fld + "  else 0 end) as mar  from schedule where branchcd='" + mbr + "' and type like '46%' and vchdate " + xprdrange + "  ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) and a.acode like '" + party_cd + "%' and substr(a.icode,1,8) like '" + part_cd + "%' group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,B.UNIT ORDER BY C.ANAME,b.INAME";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("12 Month Customer,Item Wise Sales Schedule Value Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49239":
                    cond = ulvl == "M" ? "and trim(a.acode) like '" + uname + "'" : "";
                    SQuery = "select a.vchnum as inv_no,to_char(a.vchdate,'dd/mm/yyyy') as inv_dt,b.aname as customer,a.icode as code,c.cpartno as part_no,a.purpose as part_name,a.iqtyout as qty_sold,a.irate,a.exc_rate as exc_rate,a.exc_amt as excise,a.cess_pu as cess,a.she_cess,a.iamount as basic,a.finvno as po_ref,a.binno as ref_fld,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_Dt,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.no_cases as tarr_no  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.type like '4%' and a." + branch_Cd + " and a.vchdate " + xprdrange + " " + cond + " order by a.vchnum desc,to_char(a.vchdate,'dd/mm/yyyy') desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Invoice List for the period for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49208":
                    #region
                    dt2 = new DataTable(); dt = new DataTable();
                    dt2.Columns.Add("header", typeof(string));
                    dt2.Columns.Add("fromdt", typeof(string));
                    dt2.Columns.Add("todt", typeof(string));
                    dt2.Columns.Add("po", typeof(string));
                    dt2.Columns.Add("part", typeof(string));
                    dt2.Columns.Add("poqty", typeof(double));
                    dt2.Columns.Add("price", typeof(double));
                    dt2.Columns.Add("podt", typeof(string));

                    dt2.Columns.Add("req_dt", typeof(string));
                    dt2.Columns.Add("lt_as_pr_po", typeof(string));
                    dt2.Columns.Add("sage_Std_lt", typeof(string));
                    dt2.Columns.Add("del_as_pr_Std_lt", typeof(string));
                    dt2.Columns.Add("act_del_dt", typeof(string));
                    dt2.Columns.Add("no_of_delay", typeof(string));

                    header_n = "Shipment Plan";

                    //somas me mbr pass krna hai ya branchcd='00'
                    // SQuery = "SELECT trim(a.acode),trim(a.icode) as icode,trim(b.iname) as part,0 /*a.dlvtime*/ as sage_lt,nvl(qtyord,0) as po_qty,a.irate,to_char(a.porddt,'dd/mm/yyyy') as podt,a.pordno as pono,to_char(a.cu_chldt,'dd/mm/yyyy') as req_dt  FROM SOMAS a,item b,famst c WHERE  trim(a.icode)=trim(b.icode)  and trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.TYPE='4F' and a.orddt " + xprdrange + "";
                    SQuery = "SELECT trim(a.acode),trim(a.icode) as icode,trim(a.ciname) as part,0 /*a.dlvtime*/ as sage_lt,nvl(qtyord,0) as po_qty,a.irate,to_char(a.porddt,'dd/mm/yyyy') as podt,a.pordno as pono,to_char(a.cu_chldt,'dd/mm/yyyy') as req_dt  FROM SOMAS a,famst c WHERE trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.TYPE='4F' and a.orddt " + xprdrange + "";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq1 = ""; mq0 = ""; mq2 = "";
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["po"] = dt.Rows[i]["pono"].ToString().Trim();
                        dr1["part"] = dt.Rows[i]["part"].ToString().Trim();
                        dr1["poqty"] = fgen.make_double(dt.Rows[i]["po_qty"].ToString().Trim());
                        dr1["price"] = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim());
                        dr1["podt"] = dt.Rows[i]["podt"].ToString().Trim();
                        dr1["req_dt"] = dt.Rows[i]["req_dt"].ToString().Trim();
                        double ddiff = 0;
                        try
                        {
                            DateTime dtreq_dt = Convert.ToDateTime(dt.Rows[i]["req_dt"].ToString().Trim()).Date;
                            DateTime dtpodt = Convert.ToDateTime(dt.Rows[i]["podt"].ToString().Trim()).Date;
                            TimeSpan om = dtreq_dt - dtpodt;
                            ddiff = Convert.ToDouble(om.TotalDays);
                        }
                        catch
                        {
                            ddiff = 0;
                        }
                        dr1["lt_as_pr_po"] = ddiff;
                        dr1["sage_Std_lt"] = dt.Rows[i]["sage_lt"].ToString().Trim();
                        DateTime dd1 = Convert.ToDateTime(dr1["podt"].ToString().Trim()).AddDays(Convert.ToInt32(dr1["sage_Std_lt"].ToString().Trim()));
                        dr1["del_as_pr_Std_lt"] = dd1;
                        dr1["act_del_dt"] = dt.Rows[i]["podt"].ToString().Trim();
                        double ddelay = 0;
                        try
                        {
                            DateTime dtpodt = Convert.ToDateTime(dt.Rows[i]["podt"].ToString().Trim());
                            TimeSpan t1 = dd1 - dtpodt;
                            ddelay = Convert.ToDouble(t1.TotalDays);
                        }
                        catch
                        {
                            ddelay = 0;
                        }
                        dr1["no_of_delay"] = ddelay;
                        dt2.Rows.Add(dr1);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt2;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Shipment Plan Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F49211":
                    #region
                    dt2 = new DataTable(); dt = new DataTable();
                    dt2.Columns.Add("header", typeof(string));
                    dt2.Columns.Add("fromdt", typeof(string));
                    dt2.Columns.Add("todt", typeof(string));
                    dt2.Columns.Add("price", typeof(double));
                    dt2.Columns.Add("plant", typeof(string));
                    dt2.Columns.Add("po", typeof(string));
                    dt2.Columns.Add("podt", typeof(string));
                    dt2.Columns.Add("req_dt", typeof(string));
                    dt2.Columns.Add("part", typeof(string));
                    dt2.Columns.Add("poqty", typeof(double));
                    header_n = "Goods Status";

                    SQuery = "select trim(name) as plant,type1 as code from type where id='B'";
                    dt3 = new DataTable();  // for stock values  // iqd for rate 
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    SQuery = "SELECT trim(a.acode),(case when length(trim(a.weight))=2 then a.weight else a.branchcd end ) as branchcd,trim(a.icode) as icode,trim(b.iname) as part,nvl(qtyord,0) as po_qty,a.irate,to_char(a.porddt,'dd/mm/yyyy') as podt,a.pordno as pono,to_char(a.cu_chldt,'dd/mm/yyyy') as req_dt  FROM SOMAS a,item b,famst c WHERE  trim(a.icode)=trim(b.icode)  and trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.TYPE='4F' and a.orddt " + xprdrange + "";//sage
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq1 = ""; mq0 = ""; mq2 = "";
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        // mq0 = fgen.seek_iname(frm_qstr, co_cd, "select trim(name) as plant from type where id='B' AND TYPE1='" + dt.Rows[i]["branchcd"].ToString().Trim() + "'", "plant");
                        mq0 = fgen.seek_iname_dt(dt3, "code='" + dt.Rows[i]["branchcd"].ToString().Trim() + "'", "plant");
                        dr1["plant"] = mq0;
                        dr1["price"] = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim());
                        dr1["podt"] = dt.Rows[i]["podt"].ToString().Trim();
                        dr1["req_dt"] = dt.Rows[i]["req_dt"].ToString().Trim();
                        dr1["part"] = dt.Rows[i]["part"].ToString().Trim();
                        dr1["poqty"] = fgen.make_double(dt.Rows[i]["po_qty"].ToString().Trim());
                        dt2.Rows.Add(dr1);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt2;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F49202": // for Export Wise Order Analysis Qty
                    mq1 = hf1.Value;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq0 = party_cd;
                    if (mq0.Length <= 1)
                    {
                        cond = " and trim(a.acode) like '%'";
                    }
                    else
                    {
                        cond = " and trim(a.acode) in (" + party_cd + ")";
                    }
                    SQuery = "select party_name,sum(april) as april,sum(may) as may,sum(june) as june,sum(july) as july, sum(august) as aug,sum(sept) as sept,sum(october) as oct,sum(november) as nov,sum(december) as dec,sum(jan) as jan,sum(feb) as feb,sum(march) as mar from (SELECT b.aname as party_name ,decode(to_char(a.VCHDATE,'mm'),'04',a.IQTYOUT,'0') as APRIL,decode(to_char(a.VCHDATE,'mm'),'05',a.IQTYOUT,'0') as MAY,decode(to_char(a.VCHDATE,'mm'),'06',a.IQTYOUT,'0') as JUNE,decode(to_char(a.VCHDATE,'mm'),'07',a.IQTYOUT,'0') as JULY,decode(to_char(a.VCHDATE,'mm'),'08',a.IQTYOUT,'0') as AUGUST,decode(to_char(a.VCHDATE,'mm'),'09',a.IQTYOUT,'0') as SEPT,decode(to_char(a.VCHDATE,'mm'),'10',a.IQTYOUT,'0') as OCTOBER,decode(to_char(a.VCHDATE,'mm'),'11',a.IQTYOUT,'0') as NOVEMBER,decode(to_char(a.VCHDATE,'mm'),'12',a.IQTYOUT,'0') as DECEMBER,decode(to_char(a.VCHDATE,'mm'),'01',a.IQTYOUT,'0') as JAN,decode(to_char(a.VCHDATE,'mm'),'02',a.IQTYOUT,'0') as FEB,decode(to_char(a.VCHDATE,'mm'),'03',a.IQTYOUT,'0') as MARCH FROM IVOUCHER a ,famst b  WHERE trim(a.acode)=trim(b.acode) and a.BRANCHCD='" + mbr + "' AND a.TYPE in (" + mq1 + ")  " + cond + " and b.country like '" + part_cd + "%' AND a.VCHDATE " + xprdrange + ") group by party_name order by party_name";
                    //SQuery = "select party_name,sum(april) as april,sum(may) as may,sum(june) as june,sum(july) as july, sum(august) as aug,sum(sept) as sept,sum(october) as oct,sum(november) as nov,sum(december) as dec,sum(jan) as jan,sum(feb) as feb,sum(march) as mar from (SELECT b.aname as party_name ,decode(to_char(a.VCHDATE,'mm'),'04',a.IQTYOUT,'0') as APRIL,decode(to_char(a.VCHDATE,'mm'),'05',a.IQTYOUT,'0') as MAY,decode(to_char(a.VCHDATE,'mm'),'06',a.IQTYOUT,'0') as JUNE,decode(to_char(a.VCHDATE,'mm'),'07',a.IQTYOUT,'0') as JULY,decode(to_char(a.VCHDATE,'mm'),'08',a.IQTYOUT,'0') as AUGUST,decode(to_char(a.VCHDATE,'mm'),'09',a.IQTYOUT,'0') as SEPT,decode(to_char(a.VCHDATE,'mm'),'10',a.IQTYOUT,'0') as OCTOBER,decode(to_char(a.VCHDATE,'mm'),'11',a.IQTYOUT,'0') as NOVEMBER,decode(to_char(a.VCHDATE,'mm'),'12',a.IQTYOUT,'0') as DECEMBER,decode(to_char(a.VCHDATE,'mm'),'01',a.IQTYOUT,'0') as JAN,decode(to_char(a.VCHDATE,'mm'),'02',a.IQTYOUT,'0') as FEB,decode(to_char(a.VCHDATE,'mm'),'03',a.IQTYOUT,'0') as MARCH FROM IVOUCHER a ,famst b  WHERE trim(a.acode)=trim(b.acode) and a.BRANCHCD='" + mbr + "' AND a.TYPE ='" + mq1 + "'  " + cond + " and b.country like '" + part_cd + "%' AND a.VCHDATE " + xprdrange + ") group by party_name order by party_name";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Export Wise Order Analysis Qty For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    break;

                case "F49203"://for view Reports  values export
                    mq1 = hf1.Value;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    mq0 = party_cd;
                    if (mq0.Length <= 1)
                    {
                        cond = " and trim(a.acode) like '%'";
                    }
                    else
                    {
                        cond = " and trim(a.acode) in (" + party_cd + ")";
                    }
                    SQuery = "select party_name,sum(april) as april, sum(may) as may,sum(june) as june ,sum(july) as july,sum(august) as aug,sum(sept) as sept ,sum(october) as oct,sum(november) as nov,sum(december) as dec,sum(jan) as jan,sum(feb) as feb,sum(march) as mar from (SELECT b.aname as party_name ,decode(to_char(a.VCHDATE,'mm'),'04',(a.IQTYOUT*a.iqty_chlwt),'0') as APRIL,decode(to_char(a.VCHDATE,'mm'),'05',(a.IQTYOUT*a.iqty_chlwt),'0') as MAY,decode(to_char(a.VCHDATE,'mm'),'06',(a.IQTYOUT*a.iqty_chlwt),'0') as JUNE,decode(to_char(a.VCHDATE,'mm'),'07',(a.IQTYOUT*a.iqty_chlwt),'0') as JULY,decode(to_char(a.VCHDATE,'mm'),'08',(a.IQTYOUT*a.iqty_chlwt),'0') as AUGUST,decode(to_char(a.VCHDATE,'mm'),'09',(a.IQTYOUT*a.iqty_chlwt),'0') as SEPT,decode(to_char(a.VCHDATE,'mm'),'10',(a.IQTYOUT*a.iqty_chlwt),'0') as OCTOBER,decode(to_char(a.VCHDATE,'mm'),'11',(a.IQTYOUT*a.iqty_chlwt),'0') as NOVEMBER,decode(to_char(a.VCHDATE,'mm'),'12',(a.IQTYOUT*a.iqty_chlwt),'0') as DECEMBER,decode(to_char(a.VCHDATE,'mm'),'01',(a.IQTYOUT*a.iqty_chlwt),'0') as JAN,decode(to_char(a.VCHDATE,'mm'),'02',(a.IQTYOUT*a.iqty_chlwt),'0') as FEB,decode(to_char(a.VCHDATE,'mm'),'03',(a.IQTYOUT*a.iqty_chlwt),'0') as MARCH FROM IVOUCHER a ,famst b   WHERE trim(a.acode)=trim(b.acode) and  a.BRANCHCD='" + mbr + "' AND a.TYPE in (" + mq1 + ")  " + cond + " and b.country like '" + part_cd + "%' AND a.VCHDATE " + xprdrange + " ) group by party_name order by party_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Export Wise Order Analysis Value For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    break;

            }
        }
    }
}


