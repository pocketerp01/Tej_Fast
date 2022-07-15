using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_purc : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2, dt4, dtm;
    double month, to_cons, itot_stk, itv, db1, db2, db3, db4, db5; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string party_cd, part_cd;
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
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
        {
            hfaskBranch.Value = "Y";
            fgen.msg("-", "CMSG", "Do you want to see consolidate report'13'(No for branch wise)");
        }
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
        //else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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

                case "M03012":
                    SQuery = "select code AS FSTR,CODE,CODE AS S from co";
                    header_n = "Select Code";
                    break;

                case "P15005B":
                    SQuery = "SELECT VCHNUM AS FSTR,NAME AS DEPARTMENT,VCHNUM AS CODE FROM PROJ_MAST WHERE TYPE='P8' ORDER BY VCHNUM";
                    header_n = "Select Department";
                    break;

                case "P15005Z":
                    SQuery = "select * from (SELECT '1' AS FSTR,'Resource Wise' AS REPORT,'-' AS S FROM DUAL UNION ALL SELECT '2' AS FSTR,'Department Wise' as report,'-' as s from dual union all select '3' as fstr,'Project Wise' as report,'-' as s from dual union all select '4' as fstr,'BU Wise' as report,'-' as s from dual union all select '5' as fstr,'Resource wise & Project Wise' as report,'-' as s from dual union all select '6' as fstr,'Resource wise & Activity Wise' as report,'-' as s from dual union all select '7' as fstr,'Department wise & Project Wise' as report,'-' as s from dual union all select '8' as fstr,'Resource wise & Actual Hrs Worked' as report,'-' as s from dual union all select '9' as fstr,'Reason wise downtime analysis' as report,'-' as s from dual )";
                    header_n = "Select Report Type";
                    break;

                case "F15126":
                case "F15128":
                case "F15129":
                case "F15308":
                case "F15302":
                case "F15180":
                case "F15181":
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15127":
                case "F15303":
                case "F15309":
                case "F15310":
                case "F15311":
                case "F15316":
                case "F15318":
                case "F15315":
                case "F15138":
                    SQuery = "select * from (SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' union all SELECT '5%' AS FSTR,'All PO Type' as NAME,'5%' AS CODE FROM dual) order by code ";
                    header_n = "Select PO Type";
                    break;

                case "F15304":
                case "F15305":
                case "F15306":
                case "F15307":
                case "F15314":
                    // BY MADHVI ON 14/03/2018
                    // fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", value1);
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15312":
                    header_n = "Select Vendor";
                    SQuery = "select trim(acode) as fstr, acode, aname as vendor_Name,addr1,staten from famst where substr(trim(acode),1,2) in ('05','06') order by aname";
                    break;

                case "F15313":
                    header_n = "Select Item";
                    SQuery = "select trim(icode) as fstr, icode, iname as Item_Name,unit,cpartno from item where substr(trim(icode),1,1)!= '9' and length(trim(icode))= 8 order by icode ";
                    break;

                case "F15241":
                    header_n = "Select Choice";
                    SQuery = "select '10'  as fstr,'10' as days from dual union all select '15' fstr,'15' as days from dual union all select '20' fstr,'20' as days from dual union all select '25' fstr,'25' as days from dual";
                    break;

                case "F15317":
                    header_n = "Select Days";
                    SQuery = "select '7' as fstr,'Delivery Date within 7 Days' as ms ,'7' as opt from dual  union all select '10' as fstr,'Delivery Date within 10 Days' ,'10' as opt from dual union all select '15' as fstr,'Delivery Date within 15 Days' ,'15' as opt from dual  union all select '20' as fstr,'Delivery Date within 20 Days' ,'20' as opt from dual";
                    break;

                case "F15182":
                    // TRACK AMENDMENT IN PO
                    SQuery = "SELECT TRIM(TYPE1) AS FSTR, NAME AS DOCUMENT_TYPE, TYPE1 AS CODE from type where ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                case "F15225":
                case "F15226":
                case "F15227":
                case "F15228":
                case "F15229":
                case "F15233":
                case "F15234":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                //merged and make by yogita
                case "F15286": //SOB REPORT
                    SQuery = "SELECT 'GROUP_LEVEL' AS FSTR,'GROUP LEVEL' AS  OPTION_  FROM DUAL UNION ALL SELECT 'SUB_GROUP_LEVEL' AS FSTR,'SUB GROUP LEVEL' AS  OPTION_  FROM DUAL UNION ALL SELECT 'ITEM_LEVEL' AS FSTR,'ITEM LEVEL' AS OPTION_  FROM DUAL";
                    header_n = "Select Choice";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F15312" || HCID == "F15313" || HCID == "F15317" || HCID == "F15286")
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
        // if coming after SEEK popup

        if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
        else branch_Cd = "branchcd='" + mbr + "'";

        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";

            switch (val)
            {
                case "F15127":
                case "F15303":
                case "F15309":
                case "F15310":
                case "F15316":
                case "F15311":
                case "F15315":
                case "F15138":
                    //case "F15317":
                    SQuery = "";
                    hfcode.Value = value1;
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F15312":
                case "F15313":
                case "1F5241":
                case "F15182":
                    //case "F15317":
                    hfcode.Value = value1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F15318":////need to ask mam....when choosse prev year then what will be xprange
                    // Purchase Order Delivery Expected during DTD
                    if (hf1.Value == "")
                    {
                        hf1.Value = value1;
                        header_n = "Select Days";
                        SQuery = "select '7' as fstr,'Delivery Date within 7 Days' as ms ,'7' as opt from dual  union all select '10' as fstr,'Delivery Date within 10 Days' ,'10' as opt from dual union all select '15' as fstr,'Delivery Date within 15 Days' ,'15' as opt from dual  union all select '20' as fstr,'Delivery Date within 20 Days' ,'20' as opt from dual";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_sseek(header_n, frm_qstr);
                    }
                    else
                    {
                        hfval.Value = value1;
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    }
                    break;

                case "F15286":
                    hfcode.Value = value1;
                    if (hfcode.Value == "GROUP_LEVEL")
                    {
                        SQuery = "SELECT A.TYPE1 AS FSTR,A.TYPE1 AS CODE,A.NAME as GroupName FROM TYPE A  WHERE   A. ID='Y'  and substr(trim(a.type1),0,1) in ('0','1','2','3')  ORDER BY FSTR";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Group Item", frm_qstr);
                        hf1.Value = "MAINGRP";
                    }
                    else if (hfcode.Value == "SUB_GROUP_LEVEL")
                    {
                        SQuery = "select  DISTINCT substr(TRIM(A.icode),1,4) AS FSTR,substr(TRIM(A.icode),1,4) AS SUBGRPCODE,A.INAME AS SUBGRPNAME  FROM ITEM A ,TYPE B WHERE  length(TRIM(A.icode))=4 AND B.ID='Y' and substr(TRIM(A.icode),0,1) in ('0','1','2','3') ORDER BY FSTR";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Sub Group Item", frm_qstr);
                        hf1.Value = "SUBGRP";
                    }
                    else if (hfcode.Value == "ITEM_LEVEL")
                    {
                        SQuery = "select  DISTINCT A.ICODE AS FSTR,A.ICODE,A.INAME AS ITEMNAME FROM ITEM A ,TYPE B WHERE  length(TRIM(A.icode))=8  AND substr(trim(a.icode),0,1) in ('0','1','2','3')  ORDER BY FSTR ";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Item", frm_qstr);
                        hf1.Value = "ITEM";
                    }
                    else
                    {
                        hfval.Value = value1; //FSTR VALUE FROM POPUP

                        fgen.Fn_open_prddmp1("-", frm_qstr);
                    }
                    break;

                case "F15317":
                    // Purchase Shcedule Delivery Expected during DTD
                    {
                        header_n = "Purchase Shedule Delivery Expected during DTD";
                        date1 = DateTime.Now.Date;
                        date2 = date1.AddDays(fgen.make_double(value1));
                        xprdrange = "between to_date('" + date1.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + date2.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        //SQuery = "SELECT A.ORDNO AS PO_NO,A.ORDDT,A.ACODE AS SUPP_CODE,F.ANAME AS SUPPLIER,A.ICODE AS ITEM_CODE,sum(a.ord) as qtyord, sum(a.ord)- sum(a.recd) as Bal_qty, I.INAME AS ITEM_NAME,I.CPARTNO AS PARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy') as delivery_date, A.TYPE AS PO_TYPE,T.NAME AS PO_NAME FROM (select branchcd, icode,max(del_date) del_date,acode,max(type) as type,ordno,orddt,sum(qtyord) as ord, sum(qtyrecd) as recd from (select branchcd, icode,del_date, acode,type,ordno,orddt,qtyord, 0 as qtyrecd from POMAS where  branchcd='" + mbr + "' and type like '5%' and pflag !=1 and del_date " + xprdrange + " union all select branchcd, icode, null as del_date, acode,potype,ponum,podate,0 as qtyord,iqtyin as qtyrecd from ivoucher where branchcd='" + mbr + "' and type in ('02','03','07')) group by branchcd, icode,acode,ordno,orddt)A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' having (sum(a.ord)- sum(a.recd)) >0 GROUP BY  A.ORDNO,A.ORDDT,A.ACODE,F.ANAME,A.ICODE, I.INAME,I.CPARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy'), A.TYPE ,T.NAME order by A.ORDNO";
                        SQuery = "SELECT A.ORDNO AS PO_NO,A.ORDDT,A.ACODE AS SUPP_CODE,trim(F.ANAME) AS SUPPLIER,A.ICODE AS ITEM_CODE,sum(a.ord) as qtyord, sum(a.ord)- sum(a.recd) as Bal_qty, trim(I.INAME) AS ITEM_NAME,trim(I.CPARTNO) AS PARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy') as delivery_date,trim(A.TYPE) AS PO_TYPE,trim(T.NAME) AS PO_NAME FROM  (select branchcd, icode,max(del_date) del_date,acode,max(type) as type,ordno,orddt,sum(qtyord) as ord, sum(qtyrecd) as recd from (select branchcd,trim(icode) as icode,del_date, trim(acode) as acode ,type,ordno,orddt,nvl(qtyord,0) as qtyord, 0 as qtyrecd from POMAS where  branchcd='" + mbr + "' and type like '5%' and pflag !=1 and del_date " + xprdrange + " union all select branchcd, trim(icode) as icode, null as del_date,trim(acode) as acode,potype,ponum,podate,0 as qtyord,nvl(iqtyin,0) as qtyrecd from ivoucher where branchcd='" + mbr + "' and type in ('02','03','07')) group by branchcd, icode,acode,ordno,orddt)A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' having (sum(a.ord)- sum(a.recd)) >0 GROUP BY A.ORDNO,A.ORDDT,A.ACODE,trim(F.ANAME),A.ICODE, trim(I.INAME),trim(I.CPARTNO),I.UNIT,to_char(a.del_date,'dd/mm/yyyy'), trim(A.TYPE),trim(T.NAME) order by A.ORDNO";
                        //SQuery = "SELECT A.ORDNO AS PO_NO,A.ORDDT,A.ACODE AS SUPP_CODE,F.ANAME AS SUPPLIER,A.ICODE AS ITEM_CODE,sum(a.ord) as qtyord, sum(a.ord)- sum(a.recd) as Bal_qty, I.INAME AS ITEM_NAME,I.CPARTNO AS PARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy') as delivery_date, A.TYPE AS PO_TYPE,T.NAME AS PO_NAME FROM (select branchcd, icode,max(del_date) del_date,acode,max(type) as type,ordno,orddt,sum(qtyord) as ord, sum(qtyrecd) as recd from (select branchcd, icode,del_date, acode,type,ordno,orddt,qtyord, 0 as qtyrecd from POMAS where type like '5%' and pflag !=1 and branchcd='"+ mbr +"' and del_date "+ xprdrange +" union all select branchcd, icode, null as del_date, acode,potype,ponum,podate,0 as qtyord,iqtyin as qtyrecd from ivoucher where branchcd='05' and type in ('02','03','07')) group by branchcd, icode,acode,ordno,orddt)A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND bal_qty > 0 GROUP BY A.ORDNO,A.ORDDT,A.ACODE,F.ANAME,A.ICODE, I.INAME,I.CPARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy'), A.TYPE ,T.NAME order by A.ORDNO ";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Purchase Schedule Delivery Expected for Delivery in next " + value1 + " Days", frm_qstr);
                    }
                    break;
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
                case "F47186":
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
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

            tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
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
                rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            // after prdDmp this will run            

            switch (val)
            {
                case "F15126": //
                    header_n = "Purchase Request Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='60' and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%'", xprdrange);
                    if (SQuery.Contains(", -")) // WRITTEN ON 05 JUNE 2018 AS IF TABLE NO 3 IS EMPTY THEN IT IS TAKING AS - SO TO REMOVE ERROR THIS HAS DONE BY MADHVI
                    {
                        SQuery = SQuery.Replace(", -", "");
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Purchase Request Checklist For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15127"://......
                    header_n = "Purchase Orders";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type like '5%' and a.acode like '" + party_cd + "%' and substr(trim(a.icode),1,2) like '" + part_cd + "%'", xprdrange);
                    }
                    else
                    {
                        SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and substr(trim(a.icode),1,2) like '" + part_cd + "%'", xprdrange);
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Purchase Orders (type " + hfcode.Value + ") Checklist For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15128":////...........
                    header_n = "Purchase Schedule Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='66' and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Purchase Schedule Checklist for the Period " + fromdt + " to " + todt + "", frm_qstr);
                    // fgen.Fn_open_rptlevel("Schedule (Day Wise) Checklist for the Month " + fromdt.Substring(3, 7) + "", frm_qstr);
                    break;

                case "F15129": ////...........
                    header_n = "Approved Price Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='10' and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Approved Price Checklist For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15138": //..............
                    // BY MADHVI ON 22/05/2018
                    header_n = "Pending Purchase Order Register w/o PO line No.";
                    // mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = "select a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(f.aname) as aname,trim(a.icode) as icode,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,nvl(a.prate,0) as prate,nvl(a.Qtyord,0) as qtyord,nvl(a.rcvdqty,0) as rcvqty,nvl(a.bal_qty,0) as bal_qty from WBVU_pending_po_old A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A." + branch_Cd + " AND  A.TYPE like '5%' and A.ORDDT  " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode LIKE '" + part_cd + "%' ORDER BY a.ordno";
                        header_n = "All PO Types (POs which are checked, approved and open)";
                    }
                    else
                    {
                        SQuery = "select a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(f.aname) as aname,trim(a.icode) as icode,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,nvl(a.prate,0) as prate,nvl(a.Qtyord,0) as qtyord,nvl(a.rcvdqty,0) as rcvqty,nvl(a.bal_qty,0) as bal_qty from WBVU_pending_po_old A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A." + branch_Cd + " and A.TYPE in (" + hfcode.Value + ") AND A.ORDDT  " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY a.ordno";
                        //SQuery = "select a.branchcd,a.acode,a.type,a.ordno,a.orddt,trim(a.ERP_code) as icode,max(A.PRATE) AS PRATE,SUM(a.Qtyord) as Ord_Qty,SUM(a.Soldqty) as Rcv_Qty,SUM(a.Qtyord)-SUM(a.Soldqty) as Bal_Qty from (select fstr,branchcd,type,ordno,orddt,trim(AcodE) as Acode,ERP_code,prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt,type from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and type='51' and branchcd='00' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate,potype from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and potype like '5%' AND  POTYPE='51' and branchcd='00')  group by fstr,ERP_code,trim(acode),branchcd,type,ordno,orddt,PRATE) a  GROUP BY a.branchcd,a.acode,a.type,a.ordno,a.orddt,trim(a.ERP_code) having sum(a.Qtyord)-SUM(a.Soldqty)>0 order by ordno";
                        header_n = "Showing types :" + hfcode.Value + "(Checked,appr & open POs)";
                        //fgen.seek_iname(frm_qstr, co_cd, "select name from type where id='M' and type1='" + hfcode.Value + "'", "name");
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Purchase Order Register w/o PO line No. for " + header_n + " For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15225":
                    header_n = "TAT PR VS PO";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "  where a." + branch_Cd + "  and a.type like '5%' and " + datefld + " " + xprdrange + " and " + joinfld + " and a.icode like '" + party_cd + "%' order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("TAT PR VS PO For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15226":
                    header_n = "TAT PO VS MRR";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type like '0%' and " + datefld + " " + xprdrange + " and " + joinfld + "  and a.icode like '" + party_cd + "%' order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("TAT PO VS MRR For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15227":
                    header_n = "TAT PR VS PO VS MRR";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type like '0%' and " + datefld + " " + xprdrange + " and " + joinfld + " and a.icode like '" + party_cd + "%' order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("TAT PR VS PO VS MRR For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15228":
                    header_n = "TAT MRR VS MRIR";
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type like '0%' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "SELECT A.VCHNUM AS MRR,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT, TO_CHAR(A.QC_DATE,'DD/MM/YYYY') AS QCDATE,TO_DATE(A.QC_DATE,'DD/MM/YYYY')-TO_DATE(A.VCHDATE,'DD/MM/YYYY') AS TAT_days,trim(B.INAME) as iname,trim(B.CPARTNO) as cpartno,trim(B.UNIT) as unit,nvl(A.IQTYIN,0) as IQTYIN,TRIM(A.ICODE) AS ICODE,TRIM(A.pname) as insp_by   FROM IVOUCHER A ,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND  A." + branch_Cd + " AND A.TYPE LIKE '0%'  AND A.VCHDATE " + xprdrange + " AND A.INSPECTED='Y' and trim(nvl(a.pname,'-'))!='-' and length(Trim(nvl(a.pname,'-')))>1 and a.icode like '" + party_cd + "%' ORDER BY A.VCHDATE,A.VCHNUM";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("TAT MRR VS MRIR For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15229":
                    header_n = "TAT PR APPROVAL VS PO";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //ORIGINAL QUERY.. ADD ICODE IN THE QUERY ON 15 NOV 2018  SQuery = "Select ordno as PR_NO,to_char(orddt,'dd/mm/yyyy') as PR_Dt,max(pr_App) as pr_App_Dt,max(pordno) as pono,to_char(max(porddt),'dd/mm/yyyy') as podate,MAX(POAPPBY) AS POAPPBY,MAX(POAPPDT) AS POAPPDT ,to_char(orddt,'yyyymmdd') as odd from (select ordno,orddt,decode(trim(nvl(app_by,'-')),'-',null,app_dt) as pr_App,null as pordno,null as porddt,NULL AS POAPPBY,NULL AS POAPPDT from pomas where " + branch_Cd + " and type ='60' and orddt " + xprdrange + " union all select pr_no,pr_Dt,null as pr_App,ordno as ordno,orddt as orddt,APP_BY,DECODE(TRIM(NVL(APP_BY,'-')),'-',NULL,APP_dT) AS APP_DT from pomas where " + branch_Cd + " and type like  '5%' and pr_dt " + xprdrange + " ) group by ordno,to_char(orddt,'dd/mm/yyyy'),to_char(orddt,'yyyymmdd') order by odd,ordno";
                    SQuery = "Select ordno as PR_NO,to_char(orddt,'dd/mm/yyyy') as PR_Dt,max(pr_App) as pr_App_Dt,max(pordno) as pono,to_char(max(porddt),'dd/mm/yyyy') as podate,MAX(POAPPBY) AS POAPPBY,MAX(POAPPDT) AS POAPPDT,icode,to_char(orddt,'yyyymmdd') as odd from (select icode,ordno,orddt,decode(trim(nvl(app_by,'-')),'-',null,app_dt) as pr_App,null as pordno,null as porddt,NULL AS POAPPBY,NULL AS POAPPDT from pomas where " + branch_Cd + " and type ='60' and orddt " + xprdrange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%' union all select icode,pr_no,pr_Dt,null as pr_App,ordno as ordno,orddt as orddt,APP_BY,DECODE(TRIM(NVL(APP_BY,'-')),'-',NULL,APP_dT) AS APP_DT from pomas where " + branch_Cd + " and type like  '5%' and pr_dt " + xprdrange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%') group by icode,ordno,to_char(orddt,'dd/mm/yyyy'),to_char(orddt,'yyyymmdd') order by odd,ordno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("TAT PR APPROVAL VS PO For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15233":
                    // GATE INWARD CHECKLIST
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='00' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate Inward Checklist For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15234":
                    // MATL. INWARD CHECKLIST
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " , " + table4 + "   where a." + branch_Cd + "  and a.type like '0%' and " + datefld + " " + xprdrange + " and " + joinfld + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Matl. Inward Checklist For the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F15302":
                    // PENDING PURCHASE REQUISITION
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "Select a.branchcd,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orrdt,trim(a.icode) as icode,trim(a.iname) as iname,trim(a.cpartno) as cpartno,a.unit,trim(a.deptt) as deptt,trim(b.type1) as Deptt_code,nvl(a.req_qty,0) as req_qty,nvl(a.ord_qty,0) as ord_qty,nvl(a.Bal_qty,0) as bal_qty from wbvu_pending_pr a, type b where trim(a.deptt)= trim(b.name) and b.id='M' and " + branch_Cd + " and b.type1 like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%' and a.orddt " + xprdrange + " order by a.orddt, a.ordno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Purchase Requisition For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15303":
                    // BY MADHVI ON 14/03/2018
                    header_n = "Pending Purchase Order Register";
                    // mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        SQuery = "select a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(f.aname) as aname,trim(a.icode) as icode,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,nvl(a.prate,0) as prate,nvl(a.Qtyord,0) as qtyord,nvl(a.rcvdqty,0) as rcvdqty,nvl(a.bal_qty,0) as bal_qty from WBVU_pending_PO A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A." + branch_Cd + " AND  A.TYPE like '5%' and A.ORDDT  " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode LIKE '" + part_cd + "%' ORDER BY a.ordno";
                        header_n = "All PO Types (POs which are checked, approved and open)";
                    }
                    else
                    {
                        SQuery = "select a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.acode) as acode,trim(f.aname) as aname,trim(a.icode) as icode,trim(i.iname) as iname,trim(i.cpartno) as cpartno,i.unit,nvl(a.prate,0) as prate,nvl(a.Qtyord,0) as qtyord,nvl(a.rcvdqty,0) as rcvdqty,nvl(a.bal_qty,0) as bal_qty from WBVU_pending_PO A,item i ,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and A." + branch_Cd + " and A.TYPE in (" + hfcode.Value + ") AND A.ORDDT  " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY a.ordno";
                        //SQuery = "select a.branchcd,a.acode,a.type,a.ordno,a.orddt,trim(a.ERP_code) as icode,max(A.PRATE) AS PRATE,SUM(a.Qtyord) as Ord_Qty,SUM(a.Soldqty) as Rcv_Qty,SUM(a.Qtyord)-SUM(a.Soldqty) as Bal_Qty from (select fstr,branchcd,type,ordno,orddt,trim(AcodE) as Acode,ERP_code,prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt,type from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and type='51' and branchcd='00' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate,potype from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and potype like '5%' AND  POTYPE='51' and branchcd='00')  group by fstr,ERP_code,trim(acode),branchcd,type,ordno,orddt,PRATE) a  GROUP BY a.branchcd,a.acode,a.type,a.ordno,a.orddt,trim(a.ERP_code) having sum(a.Qtyord)-SUM(a.Soldqty)>0 order by ordno";
                        header_n = "Showing types :" + hfcode.Value + "(POs which are checked, approved and open)";
                        //fgen.seek_iname(frm_qstr, co_cd, "select name from type where id='M' and type1='" + hfcode.Value + "'", "name");
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Purchase Order Register for " + header_n + " For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15304":
                    // BY MADHVI ON 14/03/2018
                    header_n = "Pending Schedule (Day Wise) Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //  SQuery = "select trim(a.acode) as acode,F.ANAME,trim(a.icode) as icode,I.INAME,I.CPARTNO,I.UNIT,sum(SchDay1)-sum(Day1) as Day1,sum(SchDay2)-sum(Day2) as Day2,sum(SchDay3)-sum(Day3) as Day3,sum(SchDay4)-sum(Day4) as Day4,sum(SchDay5)-sum(Day5) as Day5,sum(SchDay6)-sum(Day6) as Day6,sum(SchDay7)-sum(Day7) as Day7,sum(SchDay8)-sum(Day8) as Day8,sum(SchDay9)-sum(Day9) as Day9,sum(SchDay10)-sum(Day10) as Day10,sum(SchDay11)-sum(Day11) as Day11,sum(SchDay12)-sum(Day12) as Day12,sum(SchDay13)-sum(Day13) as Day13,sum(SchDay14)-sum(Day14) as Day14,sum(SchDay15)-sum(Day15) as Day15,sum(SchDay16)-sum(Day16) as Day16,sum(SchDay17)-sum(Day17) as Day17,sum(SchDay18)-sum(Day18) as Day18,sum(SchDay19)-sum(Day19) as Day19,sum(SchDay20)-sum(Day20) as Day20,sum(SchDay21)-sum(Day21) as Day21,sum(SchDay22)-sum(Day22) as Day22,sum(SchDay23)-sum(Day23) as Day23,sum(SchDay24)-sum(Day24) as Day24,sum(SchDay25)-sum(Day25) as Day25,sum(SchDay26)-sum(Day26) as Day26,sum(SchDay27)-sum(Day27) as Day27,sum(SchDay28)-sum(Day28) as Day28,sum(SchDay29)-sum(Day29) as Day29,sum(SchDay30)-sum(Day30) as Day30,sum(SchDay31)-sum(Day31) as Day31 from  (select trim(A.ACODE) as acode,trim(A.ICODE) as icode,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN A.DAY1 ELSE 0 END) AS SchDay1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN A.DAY2 ELSE 0 END) AS SchDay2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN A.DAY3 ELSE 0 END) AS SchDay3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN A.DAY4 ELSE 0 END) AS SchDay4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN A.DAY5 ELSE 0 END) AS SchDay5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN A.DAY6 ELSE 0 END) AS SchDay6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN A.DAY7 ELSE 0 END) AS SchDay7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN A.DAY8 ELSE 0 END) AS SchDay8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN A.DAY9 ELSE 0 END) AS SchDay9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN A.DAY10 ELSE 0 END) AS SchDay10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN A.DAY11 ELSE 0 END) AS SchDay11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN A.DAY12 ELSE 0 END) AS SchDay12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN A.DAY13 ELSE 0 END) AS SchDay13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN A.DAY14 ELSE 0 END) AS SchDay14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN A.DAY15 ELSE 0 END) AS SchDay15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN A.DAY16 ELSE 0 END) AS SchDay16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN A.DAY17 ELSE 0 END) AS SchDay17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN A.DAY18 ELSE 0 END) AS SchDay18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN A.DAY19 ELSE 0 END) AS SchDay19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN A.DAY20 ELSE 0 END) AS SchDay20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN A.DAY21 ELSE 0 END) AS SchDay21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN A.DAY22 ELSE 0 END) AS SchDay22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN A.DAY23 ELSE 0 END) AS SchDay23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN A.DAY24 ELSE 0 END) AS SchDay24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN A.DAY25 ELSE 0 END) AS SchDay25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN A.DAY26 ELSE 0 END) AS SchDay26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN A.DAY27 ELSE 0 END) AS SchDay27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN A.DAY28 ELSE 0 END) AS SchDay28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN A.DAY29 ELSE 0 END) AS SchDay29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN A.DAY30 ELSE 0 END) AS SchDay30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN A.DAY31 ELSE 0 END) AS SchDay31,0 AS Day1,0 AS Day2,0 AS Day3,0 as Day4,0 AS Day5,0 AS Day6,0 AS Day7,0 AS Day8,0 AS Day9,0 AS Day10,0 AS Day11,0 AS Day12,0 AS Day13,0 AS Day14,0 AS Day15,0 AS Day16,0 AS Day17,0 AS Day18,0 AS Day19,0 AS Day20,0 AS Day21,0 AS Day22,0 AS Day23,0 AS Day24,0 AS Day25,0 AS Day26,0 AS Day27,0 AS Day28,0 AS Day29,0 AS Day30,0 AS Day31 from schedule A WHERE a.branchcd='" + mbr + "' and a.type='66' and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' UNION ALL select TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,0 AS SchDay1,0 AS SchDay2,0 AS SchDay3,0 AS SchDay4,0 AS SchDay5,0 AS SchDay6,0 AS SchDay7,0 AS SchDay8,0 AS SchDay9,0 AS SchDay10,0 AS SchDay11,0 AS SchDay12,0 AS SchDay13,0 AS SchDay14,0 AS SchDay15,0 AS SchDay16,0 AS SchDay17,0 AS SchDay18,0 AS SchDay19,0 AS SchDay20,0 as SchDay21,0 AS SchDay22,0 AS SchDay23,0 AS SchDay24,0 AS SchDay25,0 AS SchDay26,0 AS SchDay27,0 AS SchDay28,0 AS SchDay29,0 AS SchDay30,0 AS SchDay31,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day31 from IVOUCHER A WHERE a.branchcd='" + mbr + "' and a.type in ('02','07') and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' and TRIM(a.icode) like '" + part_cd + "%')a,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE)  group by trim(a.acode),F.ANAME,trim(a.icode),I.INAME,I.CPARTNO,I.UNIT ORDER BY trim(a.icode)";
                    SQuery = "select trim(a.acode) as acode,trim(F.ANAME) as aname,trim(a.icode) as icode,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,I.UNIT,sum(SchDay1)-sum(Day1) as Day1,sum(SchDay2)-sum(Day2) as Day2,sum(SchDay3)-sum(Day3) as Day3,sum(SchDay4)-sum(Day4) as Day4,sum(SchDay5)-sum(Day5) as Day5,sum(SchDay6)-sum(Day6) as Day6,sum(SchDay7)-sum(Day7) as Day7,sum(SchDay8)-sum(Day8) as Day8,sum(SchDay9)-sum(Day9) as Day9,sum(SchDay10)-sum(Day10) as Day10,sum(SchDay11)-sum(Day11) as Day11,sum(SchDay12)-sum(Day12) as Day12,sum(SchDay13)-sum(Day13) as Day13,sum(SchDay14)-sum(Day14) as Day14,sum(SchDay15)-sum(Day15) as Day15,sum(SchDay16)-sum(Day16) as Day16,sum(SchDay17)-sum(Day17) as Day17,sum(SchDay18)-sum(Day18) as Day18,sum(SchDay19)-sum(Day19) as Day19,sum(SchDay20)-sum(Day20) as Day20,sum(SchDay21)-sum(Day21) as Day21,sum(SchDay22)-sum(Day22) as Day22,sum(SchDay23)-sum(Day23) as Day23,sum(SchDay24)-sum(Day24) as Day24,sum(SchDay25)-sum(Day25) as Day25,sum(SchDay26)-sum(Day26) as Day26,sum(SchDay27)-sum(Day27) as Day27,sum(SchDay28)-sum(Day28) as Day28,sum(SchDay29)-sum(Day29) as Day29,sum(SchDay30)-sum(Day30) as Day30,sum(SchDay31)-sum(Day31) as Day31 from  (select trim(A.ACODE) as acode,trim(A.ICODE) as icode,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN nvl(A.DAY1,0) ELSE 0 END) AS SchDay1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN nvl(A.DAY2,0) ELSE 0 END) AS SchDay2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN nvl(A.DAY3,0) ELSE 0 END) AS SchDay3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN nvl(A.DAY4,0) ELSE 0 END) AS SchDay4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN nvl(A.DAY5,0) ELSE 0 END) AS SchDay5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN nvl(A.DAY6,0) ELSE 0 END) AS SchDay6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN nvl(A.DAY7,0) ELSE 0 END) AS SchDay7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN nvl(A.DAY8,0) ELSE 0 END) AS SchDay8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN nvl(A.DAY9,0) ELSE 0 END) AS SchDay9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN nvl(A.DAY10,0) ELSE 0 END) AS SchDay10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN nvl(A.DAY11,0) ELSE 0 END) AS SchDay11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN nvl(A.DAY12,0) ELSE 0 END) AS SchDay12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN nvl(A.DAY13,0) ELSE 0 END) AS SchDay13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN nvl(A.DAY14,0) ELSE 0 END) AS SchDay14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN nvl(A.DAY15,0) ELSE 0 END) AS SchDay15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN nvl(A.DAY16,0) ELSE 0 END) AS SchDay16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN nvl(A.DAY17,0) ELSE 0 END) AS SchDay17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN nvl(A.DAY18,0) ELSE 0 END) AS SchDay18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN nvl(A.DAY19,0) ELSE 0 END) AS SchDay19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN nvl(A.DAY20,0) ELSE 0 END) AS SchDay20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN nvl(A.DAY21,0) ELSE 0 END) AS SchDay21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN nvl(A.DAY22,0) ELSE 0 END) AS SchDay22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN nvl(A.DAY23,0) ELSE 0 END) AS SchDay23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN nvl(A.DAY24,0) ELSE 0 END) AS SchDay24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN nvl(A.DAY25,0) ELSE 0 END) AS SchDay25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN nvl(A.DAY26,0) ELSE 0 END) AS SchDay26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN nvl(A.DAY27,0) ELSE 0 END) AS SchDay27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN nvl(A.DAY28,0) ELSE 0 END) AS SchDay28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN nvl(A.DAY29,0) ELSE 0 END) AS SchDay29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN nvl(A.DAY30,0) ELSE 0 END) AS SchDay30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN nvl(A.DAY31,0) ELSE 0 END) AS SchDay31,0 AS Day1,0 AS Day2,0 AS Day3,0 as Day4,0 AS Day5,0 AS Day6,0 AS Day7,0 AS Day8,0 AS Day9,0 AS Day10,0 AS Day11,0 AS Day12,0 AS Day13,0 AS Day14,0 AS Day15,0 AS Day16,0 AS Day17,0 AS Day18,0 AS Day19,0 AS Day20,0 AS Day21,0 AS Day22,0 AS Day23,0 AS Day24,0 AS Day25,0 AS Day26,0 AS Day27,0 AS Day28,0 AS Day29,0 AS Day30,0 AS Day31 from schedule A WHERE a.branchcd='" + mbr + "' and a.type='66' and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'  UNION ALL select TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,0 AS SchDay1,0 AS SchDay2,0 AS SchDay3,0 AS SchDay4,0 AS SchDay5,0 AS SchDay6,0 AS SchDay7,0 AS SchDay8,0 AS SchDay9,0 AS SchDay10,0 AS SchDay11,0 AS SchDay12,0 AS SchDay13,0 AS SchDay14,0 AS SchDay15,0 AS SchDay16,0 AS SchDay17,0 AS SchDay18,0 AS SchDay19,0 AS SchDay20,0 as SchDay21,0 AS SchDay22,0 AS SchDay23,0 AS SchDay24,0 AS SchDay25,0 AS SchDay26,0 AS SchDay27,0 AS SchDay28,0 AS SchDay29,0 AS SchDay30,0 AS SchDay31,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day31 from IVOUCHER A WHERE a.branchcd='" + mbr + "' and a.type in ('02','07') and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' and TRIM(a.icode) like '" + part_cd + "%')  a,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE)  group by trim(a.acode),trim(F.ANAME),trim(a.icode),trim(I.INAME),trim(I.CPARTNO),I.UNIT ORDER BY trim(a.icode)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Schedule (Day Wise) Checklist for the Month " + fromdt.Substring(3, 7) + "", frm_qstr);
                    break;

                case "F15305":
                    // BY MADHVI ON 14/03/2018
                    header_n = "Pending Schedule (Month Wise) Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    // SQuery = "select trim(a.acode) as acode,F.ANAME,trim(a.icode) as icode,I.INAME,I.CPARTNO,I.UNIT,sum(SchAPR)-sum(APR) as Apr,sum(SchMay)-sum(May) as May,sum(SchJun)-sum(Jun) as Jun,sum(SchJul)-sum(Jul) as Jul,sum(SchAug)-sum(Aug) as Aug,sum(SchSep)-sum(Sep) as Sep,sum(SchOct)-sum(Oct) as Oct,sum(SchNov)-sum(Nov) as Nov,sum(SchDec)-sum(Dec) as Dec,sum(SchJan)-sum(Jan) as Jan,sum(SchFeb)-sum(Feb) as Feb,sum(SchMar)-sum(Mar) as Mar from  (select trim(A.ACODE) as acode,trim(A.ICODE) as icode,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='04' THEN A.TOTAL ELSE 0 END) AS SchAPR,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='05' THEN A.TOTAL ELSE 0 END) AS SchMAY,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='06' THEN A.TOTAL ELSE 0 END) AS SchJUN,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='07' THEN A.TOTAL ELSE 0 END) AS SchJUL,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='08' THEN A.TOTAL ELSE 0 END) AS SchAUG ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='09' THEN A.TOTAL ELSE 0 END) AS SchSEP ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='10' THEN A.TOTAL ELSE 0 END) AS SchOCT ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='11' THEN A.TOTAL ELSE 0 END) AS SchNOV ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='12' THEN A.TOTAL ELSE 0 END) AS SchDEC ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='01' THEN A.TOTAL ELSE 0 END) AS SchJAN ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='02' THEN A.TOTAL ELSE 0 END) AS SchFEB ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='03' THEN A.TOTAL ELSE 0 END) AS SchMAR,0 as Apr,0 AS May,0 AS Jun,0 AS Jul,0 AS Aug,0 AS Sep,0 AS Oct,0 AS Nov,0 AS Dec,0 AS Jan,0 AS Feb,0 AS Mar from schedule A WHERE a.branchcd='" + mbr + "' and a.type='66' and a.vchdate " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' UNION ALL select TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,0 AS SchAPR,0 AS SchMay,0 AS SchJun,0 AS SchJul,0 AS SchAug,0 AS SchSep,0 AS SchOct,0 AS SchNov,0 AS SchDec,0 AS SchJan,0 AS SchFeb,0 AS SchMar,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='04' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS APR,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='05' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS MAY,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='06' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS JUN,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='07' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS JUL,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='08' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS AUG ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='09' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS SEP ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='10' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS OCT ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='11' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS NOV ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='12' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS DEC ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='01' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS JAN ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='02' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS FEB ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='03' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS MAR from IVOUCHER A WHERE a.branchcd='" + mbr + "' and a.type in ('02','07') and a.vchdate " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%')a,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE)  group by trim(a.acode),F.ANAME,trim(a.icode),I.INAME,I.CPARTNO,I.UNIT ORDER BY ICODE ";
                    SQuery = "select a.acode,trim(F.ANAME) as aname,a.icode,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,I.UNIT,sum(SchAPR)-sum(APR) as Apr,sum(SchMay)-sum(May) as May,sum(SchJun)-sum(Jun) as Jun,sum(SchJul)-sum(Jul) as Jul,sum(SchAug)-sum(Aug) as Aug,sum(SchSep)-sum(Sep) as Sep,sum(SchOct)-sum(Oct) as Oct,sum(SchNov)-sum(Nov) as Nov,sum(SchDec)-sum(Dec) as Dec,sum(SchJan)-sum(Jan) as Jan,sum(SchFeb)-sum(Feb) as Feb,sum(SchMar)-sum(Mar) as Mar from  (select trim(A.ACODE) as acode,trim(A.ICODE) as icode,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='04' THEN  nvl(a.total,0) ELSE 0 END) AS SchAPR,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='05' THEN nvl(a.total,0) ELSE 0 END) AS SchMAY,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='06' THEN nvl(a.total,0) ELSE 0 END) AS SchJUN,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='07' THEN nvl(a.total,0) ELSE 0 END) AS SchJUL,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='08' THEN nvl(a.total,0) ELSE 0 END) AS SchAUG ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='09' THEN nvl(a.total,0) ELSE 0 END) AS SchSEP ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='10' THEN nvl(a.total,0) ELSE 0 END) AS SchOCT ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='11' THEN nvl(a.total,0) ELSE 0 END) AS SchNOV ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='12' THEN nvl(a.total,0) ELSE 0 END) AS SchDEC ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='01' THEN nvl(a.total,0) ELSE 0 END) AS SchJAN ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='02' THEN nvl(a.total,0) ELSE 0 END) AS SchFEB ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='03' THEN nvl(a.total,0) ELSE 0 END) AS SchMAR,0 as Apr,0 AS May,0 AS Jun,0 AS Jul,0 AS Aug,0 AS Sep,0 AS Oct,0 AS Nov,0 AS Dec,0 AS Jan,0 AS Feb,0 AS Mar from schedule A WHERE a.branchcd='" + mbr + "' and a.type='66' and a.vchdate " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' UNION ALL  select TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,0 AS SchAPR,0 AS SchMay,0 AS SchJun,0 AS SchJul,0 AS SchAug,0 AS SchSep,0 AS SchOct,0 AS SchNov,0 AS SchDec,0 AS SchJan,0 AS SchFeb,0 AS SchMar,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='04' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS APR,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='05' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS MAY,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='06' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS JUN,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='07' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS JUL,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='08' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS AUG ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='09' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS SEP ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='10' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS OCT ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='11' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS NOV ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='12' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS DEC ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='01' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS JAN ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='02' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS FEB ,(CASE WHEN TO_CHAR(A.VCHDATE,'MM')='03' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS MAR from IVOUCHER A WHERE a.branchcd='" + mbr + "' and a.type in ('02','07') and a.vchdate " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%')  a,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE)  group by a.acode,trim(F.ANAME),a.icode,trim(I.INAME),trim(I.CPARTNO),I.UNIT ORDER BY ICODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Schedule (Month Wise) Checklist For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15306": ////
                    // BY MADHVI ON 14/03/2018
                    header_n = "Schedule (Day Wise) Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //SQuery = "select trim(A.ACODE) as acode,trim(b.aname) as aname,trim(A.ICODE) as icode,trim(c.iname) as iname,trim(c.cpartno) as part_no,c.unit ,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN nvl(A.DAY1,0) ELSE 0 END) AS SchDay1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN nvl(A.DAY2,0) ELSE 0 END) AS SchDay2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN nvl(A.DAY3,0) ELSE 0 END) AS SchDay3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN nvl(A.DAY4,0) ELSE 0 END) AS SchDay4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN nvl(A.DAY5,0) ELSE 0 END) AS SchDay5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN nvl(A.DAY6,0) ELSE 0 END) AS SchDay6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN nvl(A.DAY7,0) ELSE 0 END) AS SchDay7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN nvl(A.DAY8,0) ELSE 0 END) AS SchDay8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN nvl(A.DAY9,0) ELSE 0 END) AS SchDay9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN nvl(A.DAY10,0) ELSE 0 END) AS SchDay10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN nvl(A.DAY11,0) ELSE 0 END) AS SchDay11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN nvl(A.DAY12,0) ELSE 0 END) AS SchDay12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN nvl(A.DAY13,0) ELSE 0 END) AS SchDay13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN nvl(A.DAY14,0) ELSE 0 END) AS SchDay14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN nvl(A.DAY15,0) ELSE 0 END) AS SchDay15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN nvl(A.DAY16,0) ELSE 0 END) AS SchDay16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN nvl(A.DAY17,0) ELSE 0 END) AS SchDay17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN nvl(A.DAY18,0) ELSE 0 END) AS SchDay18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN nvl(A.DAY19,0) ELSE 0 END) AS SchDay19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN nvl(A.DAY20,0) ELSE 0 END) AS SchDay20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN nvl(A.DAY21,0) ELSE 0 END) AS SchDay21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN nvl(A.DAY22,0) ELSE 0 END) AS SchDay22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN nvl(A.DAY23,0) ELSE 0 END) AS SchDay23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN nvl(A.DAY24,0) ELSE 0 END) AS SchDay24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN nvl(A.DAY25,0) ELSE 0 END) AS SchDay25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN nvl(A.DAY26,0) ELSE 0 END) AS SchDay26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN nvl(A.DAY27,0) ELSE 0 END) AS SchDay27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN nvl(A.DAY28,0) ELSE 0 END) AS SchDay28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN nvl(A.DAY29,0) ELSE 0 END) AS SchDay29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN nvl(A.DAY30,0) ELSE 0 END) AS SchDay30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN nvl(A.DAY31,0) ELSE 0 END) AS SchDay31 from schedule A ,famst b,item c WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='66' and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by acode"; //isme case me dd condtion lagayi hai...but values is not matched
                    SQuery = "select trim(A.ACODE) as acode,trim(b.aname) as aname,trim(A.ICODE) as icode,trim(c.iname) as iname,trim(c.cpartno) as part_no,c.unit ,nvl(A.DAY1,0)  AS SchDay1, nvl(A.DAY2,0) AS SchDay2,nvl(A.DAY3,0) AS SchDay3,nvl(A.DAY4,0) AS SchDay4 ,nvl(a.day5,0) AS SchDay5,nvl(a.day6,0) AS SchDay6,nvl(a.day7,0) AS SchDay7,nvl(a.day8,0) AS SchDay8,nvl(a.day9,0) AS SchDay9,nvl(a.day10,0) AS SchDay10,nvl(a.day11,0) AS SchDay11,nvl(a.day12,0) AS SchDay12,nvl(a.day13,0) AS SchDay13,nvl(a.day14,0) AS SchDay14,nvl(a.day15,0) AS SchDay15,nvl(a.day16,0) AS SchDay16,nvl(a.day17,0) AS SchDay17,nvl(a.day18,0) AS SchDay18,nvl(a.day19,0) AS SchDay19,nvl(a.day20,0) AS SchDay20,nvl(a.day21,0) AS SchDay21,nvl(a.day22,0) AS SchDay22,nvl(a.day23,0) AS SchDay23,nvl(a.day24,0) AS SchDay24,nvl(a.day25,0) AS SchDay25,nvl(a.day26,0) AS SchDay26,nvl(a.day27,0) AS SchDay27,nvl(a.day28,0) AS SchDay28,nvl(a.day29,0) AS SchDay29,nvl(a.day30,0) AS SchDay30,nvl(a.day31,0) AS SchDay31  from schedule A ,famst b,item c WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='66' and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by acode"; //isme ony day wise pic kiya hai but value is mtached
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Schedule (Day Wise) Checklist for the Month " + fromdt.Substring(3, 7) + "", frm_qstr);
                    break;

                case "F15307":
                    // BY MADHVI ON 22/05/2018
                    header_n = "Pending Schedule (Vendor Wise) Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    //   SQuery = "select trim(a.acode) as acode,F.ANAME,trim(a.icode) as icode,I.INAME,I.CPARTNO,I.UNIT,sum(SchDay1)-sum(Day1) as Day1,sum(SchDay2)-sum(Day2) as Day2,sum(SchDay3)-sum(Day3) as Day3,sum(SchDay4)-sum(Day4) as Day4,sum(SchDay5)-sum(Day5) as Day5,sum(SchDay6)-sum(Day6) as Day6,sum(SchDay7)-sum(Day7) as Day7,sum(SchDay8)-sum(Day8) as Day8,sum(SchDay9)-sum(Day9) as Day9,sum(SchDay10)-sum(Day10) as Day10,sum(SchDay11)-sum(Day11) as Day11,sum(SchDay12)-sum(Day12) as Day12,sum(SchDay13)-sum(Day13) as Day13,sum(SchDay14)-sum(Day14) as Day14,sum(SchDay15)-sum(Day15) as Day15,sum(SchDay16)-sum(Day16) as Day16,sum(SchDay17)-sum(Day17) as Day17,sum(SchDay18)-sum(Day18) as Day18,sum(SchDay19)-sum(Day19) as Day19,sum(SchDay20)-sum(Day20) as Day20,sum(SchDay21)-sum(Day21) as Day21,sum(SchDay22)-sum(Day22) as Day22,sum(SchDay23)-sum(Day23) as Day23,sum(SchDay24)-sum(Day24) as Day24,sum(SchDay25)-sum(Day25) as Day25,sum(SchDay26)-sum(Day26) as Day26,sum(SchDay27)-sum(Day27) as Day27,sum(SchDay28)-sum(Day28) as Day28,sum(SchDay29)-sum(Day29) as Day29,sum(SchDay30)-sum(Day30) as Day30,sum(SchDay31)-sum(Day31) as Day31 from  (select trim(A.ACODE) as acode,trim(A.ICODE) as icode,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN A.DAY1 ELSE 0 END) AS SchDay1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN A.DAY2 ELSE 0 END) AS SchDay2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN A.DAY3 ELSE 0 END) AS SchDay3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN A.DAY4 ELSE 0 END) AS SchDay4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN A.DAY5 ELSE 0 END) AS SchDay5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN A.DAY6 ELSE 0 END) AS SchDay6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN A.DAY7 ELSE 0 END) AS SchDay7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN A.DAY8 ELSE 0 END) AS SchDay8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN A.DAY9 ELSE 0 END) AS SchDay9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN A.DAY10 ELSE 0 END) AS SchDay10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN A.DAY11 ELSE 0 END) AS SchDay11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN A.DAY12 ELSE 0 END) AS SchDay12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN A.DAY13 ELSE 0 END) AS SchDay13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN A.DAY14 ELSE 0 END) AS SchDay14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN A.DAY15 ELSE 0 END) AS SchDay15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN A.DAY16 ELSE 0 END) AS SchDay16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN A.DAY17 ELSE 0 END) AS SchDay17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN A.DAY18 ELSE 0 END) AS SchDay18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN A.DAY19 ELSE 0 END) AS SchDay19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN A.DAY20 ELSE 0 END) AS SchDay20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN A.DAY21 ELSE 0 END) AS SchDay21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN A.DAY22 ELSE 0 END) AS SchDay22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN A.DAY23 ELSE 0 END) AS SchDay23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN A.DAY24 ELSE 0 END) AS SchDay24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN A.DAY25 ELSE 0 END) AS SchDay25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN A.DAY26 ELSE 0 END) AS SchDay26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN A.DAY27 ELSE 0 END) AS SchDay27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN A.DAY28 ELSE 0 END) AS SchDay28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN A.DAY29 ELSE 0 END) AS SchDay29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN A.DAY30 ELSE 0 END) AS SchDay30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN A.DAY31 ELSE 0 END) AS SchDay31,0 AS Day1,0 AS Day2,0 AS Day3,0 as Day4,0 AS Day5,0 AS Day6,0 AS Day7,0 AS Day8,0 AS Day9,0 AS Day10,0 AS Day11,0 AS Day12,0 AS Day13,0 AS Day14,0 AS Day15,0 AS Day16,0 AS Day17,0 AS Day18,0 AS Day19,0 AS Day20,0 AS Day21,0 AS Day22,0 AS Day23,0 AS Day24,0 AS Day25,0 AS Day26,0 AS Day27,0 AS Day28,0 AS Day29,0 AS Day30,0 AS Day31 from schedule A WHERE a.branchcd='" + mbr + "' and a.type='66' and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' UNION ALL select TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,0 AS SchDay1,0 AS SchDay2,0 AS SchDay3,0 AS SchDay4,0 AS SchDay5,0 AS SchDay6,0 AS SchDay7,0 AS SchDay8,0 AS SchDay9,0 AS SchDay10,0 AS SchDay11,0 AS SchDay12,0 AS SchDay13,0 AS SchDay14,0 AS SchDay15,0 AS SchDay16,0 AS SchDay17,0 AS SchDay18,0 AS SchDay19,0 AS SchDay20,0 as SchDay21,0 AS SchDay22,0 AS SchDay23,0 AS SchDay24,0 AS SchDay25,0 AS SchDay26,0 AS SchDay27,0 AS SchDay28,0 AS SchDay29,0 AS SchDay30,0 AS SchDay31,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN A.IQTYIN+nvl(a.rej_rw,0) ELSE 0 END) AS Day31 from IVOUCHER A WHERE a.branchcd='" + mbr + "' and a.type in ('02','07') and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' )a,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE)  group by trim(a.acode),F.ANAME,trim(a.icode),I.INAME,I.CPARTNO,I.UNIT ORDER BY f.aname";
                    SQuery = "select trim(a.acode) as acode,trim(F.ANAME) as aname,trim(a.icode) as icode,trim(I.INAME) as iname,trim(I.CPARTNO) as part_no,I.UNIT,sum(SchDay1)-sum(Day1) as Day1,sum(SchDay2)-sum(Day2) as Day2,sum(SchDay3)-sum(Day3) as Day3,sum(SchDay4)-sum(Day4) as Day4,sum(SchDay5)-sum(Day5) as Day5,sum(SchDay6)-sum(Day6) as Day6,sum(SchDay7)-sum(Day7) as Day7,sum(SchDay8)-sum(Day8) as Day8,sum(SchDay9)-sum(Day9) as Day9,sum(SchDay10)-sum(Day10) as Day10,sum(SchDay11)-sum(Day11) as Day11,sum(SchDay12)-sum(Day12) as Day12,sum(SchDay13)-sum(Day13) as Day13,sum(SchDay14)-sum(Day14) as Day14,sum(SchDay15)-sum(Day15) as Day15,sum(SchDay16)-sum(Day16) as Day16,sum(SchDay17)-sum(Day17) as Day17,sum(SchDay18)-sum(Day18) as Day18,sum(SchDay19)-sum(Day19) as Day19,sum(SchDay20)-sum(Day20) as Day20,sum(SchDay21)-sum(Day21) as Day21,sum(SchDay22)-sum(Day22) as Day22,sum(SchDay23)-sum(Day23) as Day23,sum(SchDay24)-sum(Day24) as Day24,sum(SchDay25)-sum(Day25) as Day25,sum(SchDay26)-sum(Day26) as Day26,sum(SchDay27)-sum(Day27) as Day27,sum(SchDay28)-sum(Day28) as Day28,sum(SchDay29)-sum(Day29) as Day29,sum(SchDay30)-sum(Day30) as Day30,sum(SchDay31)-sum(Day31) as Day31 from  (select trim(A.ACODE) as acode,trim(A.ICODE) as icode,nvl(A.DAY1,0)  AS SchDay1, nvl(A.DAY2,0) AS SchDay2,nvl(A.DAY3,0) AS SchDay3,nvl(A.DAY4,0) AS SchDay4 ,nvl(a.day5,0) AS SchDay5,nvl(a.day6,0) AS SchDay6,nvl(a.day7,0) AS SchDay7,nvl(a.day8,0) AS SchDay8,nvl(a.day9,0) AS SchDay9,nvl(a.day10,0) AS SchDay10,nvl(a.day11,0) AS SchDay11,nvl(a.day12,0) AS SchDay12,nvl(a.day13,0) AS SchDay13,nvl(a.day14,0) AS SchDay14,nvl(a.day15,0) AS SchDay15,nvl(a.day16,0) AS SchDay16,nvl(a.day17,0) AS SchDay17,nvl(a.day18,0) AS SchDay18,nvl(a.day19,0) AS SchDay19,nvl(a.day20,0) AS SchDay20,nvl(a.day21,0) AS SchDay21,nvl(a.day22,0) AS SchDay22,nvl(a.day23,0) AS SchDay23,nvl(a.day24,0) AS SchDay24,nvl(a.day25,0) AS SchDay25,nvl(a.day26,0) AS SchDay26,nvl(a.day27,0) AS SchDay27,nvl(a.day28,0) AS SchDay28,nvl(a.day29,0) AS SchDay29,nvl(a.day30,0) AS SchDay30,nvl(a.day31,0) AS SchDay31,0 AS Day1,0 AS Day2,0 AS Day3,0 as Day4,0 AS Day5,0 AS Day6,0 AS Day7,0 AS Day8,0 AS Day9,0 AS Day10,0 AS Day11,0 AS Day12,0 AS Day13,0 AS Day14,0 AS Day15,0 AS Day16,0 AS Day17,0 AS Day18,0 AS Day19,0 AS Day20,0 AS Day21,0 AS Day22,0 AS Day23,0 AS Day24,0 AS Day25,0 AS Day26,0 AS Day27,0 AS Day28,0 AS Day29,0 AS Day30,0 AS Day31 from schedule a where a.branchcd='" + mbr + "' and a.type='66' and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%'  UNION ALL select TRIM(A.ACODE) AS ACODE,TRIM(A.ICODE) AS ICODE,0 AS SchDay1,0 AS SchDay2,0 AS SchDay3,0 AS SchDay4,0 AS SchDay5,0 AS SchDay6,0 AS SchDay7,0 AS SchDay8,0 AS SchDay9,0 AS SchDay10,0 AS SchDay11,0 AS SchDay12,0 AS SchDay13,0 AS SchDay14,0 AS SchDay15,0 AS SchDay16,0 AS SchDay17,0 AS SchDay18,0 AS SchDay19,0 AS SchDay20,0 as SchDay21,0 AS SchDay22,0 AS SchDay23,0 AS SchDay24,0 AS SchDay25,0 AS SchDay26,0 AS SchDay27,0 AS SchDay28,0 AS SchDay29,0 AS SchDay30,0 AS SchDay31,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='01' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='02' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='03' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='04' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day4,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='05' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='06' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='07' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='08' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='09' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='10' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='11' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='12' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='13' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='14' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='15' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='16' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='17' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='18' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='19' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='20' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='21' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='22' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='23' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='24' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='25' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='26' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='27' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='28' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='29' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='30' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.VCHDATE,'DD')='31' THEN nvl(A.IQTYIN,0)+nvl(a.rej_rw,0) ELSE 0 END) AS Day31 from IVOUCHER A WHERE a.branchcd='" + mbr + "' and a.type in ('02','07') and to_char(a.vchdate,'mm/yyyy')='" + fromdt.Substring(3, 7) + "' and a.acode like '" + party_cd + "%' )a,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE)  group by trim(a.acode),trim(F.ANAME),trim(a.icode),trim(I.INAME),trim(I.CPARTNO),I.UNIT ORDER BY aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Schedule (Vendor Wise) Checklist for the Month " + fromdt.Substring(3, 7) + "", frm_qstr);
                    break;

                case "F15308":
                    // BY MADHVI ON 14/03/2018
                    header_n = "Closed P.R. Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    // SQuery = "select a.branchcd,a.a_acode as code,b.name as supplier, a.pr_no,to_Char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.icode,c.iname,C.CPARTNO,C.UNIT,a.prqty,a.poqty,a.term as closure from (select fstr, branchcd,max(type),max(acode) as a_acode,pr_no,pr_dt,icode, sum(pr_qty) as PRQty , sum(po_qty) as POQty,max (pflag) as flag,max(term) as term from (select (trim(ordno)||'-'||to_Char( orddt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,type,acode,ordno as pr_no,orddt as pr_dt,icode, qtyord as pr_qty, 0 as po_qty,pflag,nvl(term,'-') as term from pomas where branchcd='" + mbr + "' and type='60' and acode like '" + party_cd + "%' and substr(icode,1,2) like '" + part_cd + "%' and pflag=0 and orddt " + xprdrange + " union all select (trim(pr_no)||'-'||to_Char( pr_dt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,null as type,null as acode,pr_no as pr_no,pr_dt as pr_dt,icode, 0 as pr_qty, qtyord as po_qty,0 as pflag,null as term from pomas where branchcd='" + mbr + "' and acode like '" + party_cd + "%' and substr(icode,1,2) like '" + part_cd + "%' and type like '5%' and pr_dt " + xprdrange + ") group by fstr, branchcd,pr_no,pr_dt,icode )a, type b, item c where a.flag=0 and trim(a.a_acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) order by a.pr_dt, a.pr_no";
                    SQuery = "select a.branchcd,a.a_acode as code,b.name as supplier, a.pr_no,to_Char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.icode,c.iname,C.CPARTNO,C.UNIT,a.prqty,a.poqty,a.term as closure from (select fstr, branchcd,max(type),max(acode) as a_acode,pr_no,pr_dt,icode, sum(pr_qty) as PRQty , sum(po_qty) as POQty,max (pflag) as flag,max(term) as term from (select (trim(ordno)||'-'||to_Char( orddt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,type,trim(acode) as acode,ordno as pr_no,orddt as pr_dt,trim(icode) as icode, nvl(qtyord,0) as pr_qty, 0 as po_qty,pflag,nvl(term,'-') as term from pomas where branchcd='" + mbr + "' and type='60' and trim(acode) like '" + party_cd + "%' and substr(trim(icode),1,2) like '" + part_cd + "%' and pflag=0 and orddt " + xprdrange + "  union all    select (trim(pr_no)||'-'||to_Char( pr_dt,'dd/mm/yyyy')||'-'||trim(icode)) as fstr, branchcd,null as type,null as acode,pr_no as pr_no,pr_dt as pr_dt,trim(icode) as icode, 0 as pr_qty, nvl(qtyord,0) as po_qty,0 as pflag,null as term from pomas where branchcd='" + mbr + "' and trim(acode) like '" + party_cd + "%' and substr(trim(icode),1,2) like '" + part_cd + "%' and type like '5%' and pr_dt " + xprdrange + " ) group by fstr, branchcd,pr_no,pr_dt,icode )a, type b, item c where a.flag=0 and trim(a.a_acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) order by a.pr_dt, a.pr_no";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Closed P.R. Checklist For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15309":
                    // BY MADHVI ON 14/03/2018
                    header_n = "Closed PO Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        //SQuery = "Select  TO_CHAR(a.orddt,'DD/MM/YYYY') as Dated,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.desp_to as Reason,a.Term,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,a.App_by as Closed_by,to_char(a.App_dt,'DD-MON') as App_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a." + branch_Cd + " and a.type LIKE '5%'  and a.orddt " + xprdrange + " and a.pflag=1 and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        SQuery = "Select  TO_CHAR(a.orddt,'DD/MM/YYYY') as Dated,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,trim(c.iname) as Item,trim(a.icode) as Code,trim(c.cpartno) as Drwg_No,trim(b.aname) as Supplier,a.desc_,a.pr_no,nvl(a.qtyord,0) as Ordered,nvl(a.prate,0) as Rate,trim(a.desp_to) as Reason,trim(a.Term) as term,nvl(a.pdisc,0) as Discount,nvl(a.pexc,0) as Excise,nvl(a.pcess,0) as cess,nvl(a.ptax,0) as Tax,a.payment,a.Pbasis,a.Freight,a.Ent_by,a.App_by as Closed_by,to_char(a.App_dt,'DD-MON') as App_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a." + branch_Cd + " and a.type LIKE '5%'  and a.orddt " + xprdrange + " and a.pflag=1 and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        header_n = "All PO Types";
                    }
                    else
                    {
                        //SQuery = "Select TO_CHAR(a.orddt,'DD/MM/YYYY') as Dated,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.desp_to as Reason,a.Term,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,a.App_by as Closed_by,to_char(a.App_dt,'DD-MON') as App_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a." + branch_Cd + " and a.type IN (" + hfcode.Value + ")  and a.orddt " + xprdrange + " and a.pflag=1 and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        SQuery = "Select  TO_CHAR(a.orddt,'DD/MM/YYYY') as Dated,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,trim(c.iname) as Item,trim(a.icode) as Code,trim(c.cpartno) as Drwg_No,trim(b.aname) as Supplier,a.desc_,a.pr_no,nvl(a.qtyord,0) as Ordered,nvl(a.prate,0) as Rate,trim(a.desp_to) as Reason,trim(a.Term) as term,nvl(a.pdisc,0) as Discount,nvl(a.pexc,0) as Excise,nvl(a.pcess,0) as cess,nvl(a.ptax,0) as Tax,a.payment,a.Pbasis,a.Freight,a.Ent_by,a.App_by as Closed_by,to_char(a.App_dt,'DD-MON') as App_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a." + branch_Cd + " and a.type IN (" + hfcode.Value + ")  and a.orddt " + xprdrange + " and a.pflag=1 and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        header_n = "Showing types :" + hfcode.Value;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Closed Purchase Orders Checklist for (" + header_n + ") For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15310":
                    // BY MADHVI ON 14/03/2018
                    header_n = "Cancelled PO Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        // SQuery = "Select a.branchcd,a.type,a.ordno as PO_NO,TO_CHAR(a.orddt,'DD/MM/YYYY') as Dated,a.icode as item_Code,c.iname as Item,c.cpartno as part_No,c.unit,a.acode as code,b.aname as Supplier,a.qtyord as Ordered_qty,a.prate as Rate,a.Ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt, a.app_by as Cancelled_by from pomas a, famst b , item c where a." + branch_Cd + " and a.type like '5%'  and a.orddt " + xprdrange + " and a.pflag=1 and trim(a.app_by) like '(C)%' and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        //SQuery = "Select to_char(a.orddt,'dd/mm/yyyy') as Dated,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.desp_to as Reason,a.Term,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,a.App_by,to_char(a.App_dt,'DD-MON') as App_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.pflag=1 and a." + branch_Cd + " and substr(A.type,1,1) like '5%' and a.orddt " + xprdrange + " and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        //SQuery= "Select IOPR AS Category,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.term as Reason,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.type like '5%' and a.term like '* * CANCELLED P.O.* *%' and a."+branch_Cd+" and substr(A.type,1,1) like '5%' and a.orddt  "+xprdrange +"  and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) order by a.orddt,a.ordno,a.srno";
                        SQuery = "Select IOPR AS Category,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,trim(c.iname) as Item,trim(a.icode) as Code,trim(c.cpartno) as Drwg_No,trim(b.aname) as Supplier,a.desc_,a.pr_no,nvl(a.qtyord,0) as Ordered,nvl(a.prate,0) as Rate,a.term as Reason,nvl(a.pdisc,0) as Discount,nvl(a.pexc,0) as Excise,nvl(a.pcess,0) as cess,nvl(a.ptax,0) as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.type like '5%' /*and a.term like '* * CANCELLED P.O.* *%'*/ and a." + branch_Cd + " and substr(A.type,1,1) like '5%' and a.orddt  " + xprdrange + "  and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        header_n = "All PO Types";
                    }
                    else
                    {
                        //SQuery = "Select a.branchcd,a.type,a.ordno as PO_NO,TO_CHAR(a.orddt,'DD/MM/YYYY') as Dated,a.icode as item_Code,c.iname as Item,c.cpartno as part_No,c.unit,a.acode as code,b.aname as Supplier,a.qtyord as Ordered_qty,a.prate as Rate,a.Ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt, a.app_by as Cancelled_by from pomas a, famst b , item c where a." + branch_Cd + " and a.type IN (" + hfcode.Value + ")  and a.orddt " + xprdrange + " and a.pflag=1 and trim(a.app_by) like '(C)%' and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        //SQuery = "Select to_char(a.orddt,'dd/mm/yyyy') as Dated,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.desp_to as Reason,a.Term,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,a.App_by,to_char(a.App_dt,'DD-MON') as App_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.pflag=1 and a." + branch_Cd + " and a.type IN (" + hfcode.Value + ") and a.orddt " + xprdrange + " and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        //SQuery = "Select IOPR AS Category,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.term as Reason,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from from pomas a, famst b , item c where a.type like '5%' and a.term like '* * CANCELLED P.O.* *%' and a.type IN (" + hfcode.Value + ") and a." + branch_Cd + " and substr(A.type,1,1) like '5%' and a.orddt  " + xprdrange + "  and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) order by a.orddt,a.ordno,a.srno";
                        SQuery = "Select IOPR AS Category,a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,trim(c.iname) as Item,trim(a.icode) as Code,trim(c.cpartno) as Drwg_No,trim(b.aname) as Supplier,a.desc_,a.pr_no,nvl(a.qtyord,0) as Ordered,nvl(a.prate,0) as Rate,a.term as Reason,nvl(a.pdisc,0) as Discount,nvl(a.pexc,0) as Excise,nvl(a.pcess,0) as cess,nvl(a.ptax,0) as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.type like '5%' /*and a.term like '* * CANCELLED P.O.* *%'*/ and a.type IN (" + hfcode.Value + ") and a." + branch_Cd + " and substr(A.type,1,1) like '5%' and a.orddt  " + xprdrange + "  and trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        header_n = "Showing types :" + hfcode.Value;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Cancelled Purchase Orders Checklist for (" + header_n + ") For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15311":
                    header_n = "PO Amendment History Checklist";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        //SQuery = "Select IOPR AS Category,DECODE(a.branchcd,'AM','Old_Ver', a.branchcd) as branchcd, a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.term as Reason,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.orignalbr='" + mbr + "' and a.type like '5%' and  a.orddt " + xprdrange + " and a.amdtno>1 and  trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        SQuery = "Select IOPR AS Category,DECODE(a.branchcd,'AM','Old_Ver', a.branchcd) as branchcd, a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,trim(c.iname) as Item,trim(a.icode) as Code,trim(c.cpartno) as Drwg_No,trim(b.aname) as Supplier,a.desc_,a.pr_no,nvl(a.qtyord,0) as Ordered,nvl(a.prate,0) as Rate,a.term as Reason,nvl(a.pdisc,0) as Discount,nvl(a.pexc,0) as Excise,nvl(a.pcess,0) as cess,nvl(a.ptax,0) as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where a.orignalbr='" + mbr + "' and a.type like '5%' and  a.orddt " + xprdrange + " and a.amdtno>1 and  trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        header_n = "All PO Types";
                    }
                    else
                    {
                        //SQuery = "Select IOPR AS Category,DECODE(a.branchcd,'AM','Old_Ver', a.branchcd) as branchcd, a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,c.iname as Item,a.icode as Code,c.cpartno as Drwg_No,b.aname as Supplier,a.desc_,a.pr_no,a.qtyord as Ordered,a.prate as Rate,a.term as Reason,a.pdisc as Discount,a.pexc as Excise,a.pcess as cess,a.ptax as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where  a.orignalbr='" + mbr + "' and a.type in (" + hfcode.Value + ") and  a.orddt " + xprdrange + " and a.amdtno>1 and  trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        SQuery = "Select IOPR AS Category,DECODE(a.branchcd,'AM','Old_Ver', a.branchcd) as branchcd, a.ordno||DECODE(A.Amdtno,0,'','.'||A.Amdtno) as PO_NO,trim(c.iname) as Item,trim(a.icode) as Code,trim(c.cpartno) as Drwg_No,trim(b.aname) as Supplier,a.desc_,a.pr_no,nvl(a.qtyord,0) as Ordered,nvl(a.prate,0) as Rate,a.term as Reason,nvl(a.pdisc,0) as Discount,nvl(a.pexc,0) as Excise,nvl(a.pcess,0) as cess,nvl(a.ptax,0) as Tax,a.Payment,a.Pbasis,a.Freight,a.Ent_by,to_char(a.orddt,'DD-MON') as Dated,a.Edt_by,to_char(a.Edt_dt,'DD-MON') as Edt_Dt,decode(nvl(pflag,0),1,'Closed','Current') as POStatus from pomas a, famst b , item c where  a.orignalbr='" + mbr + "' and a.type in (" + hfcode.Value + ") and  a.orddt " + xprdrange + " and a.amdtno>1 and  trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.orddt,a.ordno,a.srno";
                        header_n = "Showing types :" + hfcode.Value;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("PO Amendment History Checklist for (" + header_n + ") For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15312":
                    // VENDOR WISE 12 MONTHS RATE (MAX) TREND
                    //SQuery = "SELECT A.ACODE as Vendor_code,C.ANAME as Vendor_name,a.icode as item_code,b.INAME,b.cpartno,B.UNIT,A.TYPE as po_type from ( select  ACODE,TYPE,icode,(Case when to_char(ORDDT,'mm')='04' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as mar  from POMAS where " + branch_Cd + " and type like '5%' and ORDDT " + xprdrange + " and acode='" + hfcode.Value + "' ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,A.TYPE,C.ANAME,B.UNIT ORDER BY Vendor_code"; //
                    //SQuery = "SELECT A.ACODE as Vendor_code,C.ANAME as Vendor_name,a.icode as item_code,b.INAME,b.cpartno,B.UNIT,A.TYPE as po_type,max(a.apr) as apr,max(a.may) as may,max(a.jun) as jun,max(a.jul) as jul,max(a.aug) as aug,max(a.sep) as sep,max(a.oct) as oct,max(a.nov) as nov,max(a.dec) as dec,max(a.jan) as jan,max(a.feb) as feb,max(a.mar) as mar  from ( select  ACODE,TYPE,icode,(Case when to_char(ORDDT,'mm')='04' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as mar  from POMAS where " + branch_Cd + " and type like '5%' and ORDDT " + xprdrange + " and acode='" + hfcode.Value + "' ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,A.TYPE,C.ANAME,B.UNIT ORDER BY Vendor_code"; //
                    SQuery = "SELECT A.ACODE as Vendor_code,trim(C.ANAME) as Vendor_name,a.icode as item_code,trim(b.INAME),trim(b.hscode) as hscode,trim(b.cpartno) as Part_No,B.UNIT,A.TYPE as po_type,max(a.apr) as apr,max(a.may) as may,max(a.jun) as jun,max(a.jul) as jul,max(a.aug) as aug,max(a.sep) as sep,max(a.oct) as oct,max(a.nov) as nov,max(a.dec) as dec,max(a.jan) as jan,max(a.feb) as feb,max(a.mar) as mar  from ( select  trim(ACODE) as acode,TYPE,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as mar  from POMAS where " + branch_Cd + " and type like '5%' and ORDDT " + xprdrange + " and acode='" + hfcode.Value + "' ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,trim(b.iname),trim(b.cpartno),trim(b.hscode),A.ACODE,A.TYPE,trim(C.ANAME),B.UNIT ORDER BY Vendor_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Vendor Wise 12 Months Rate (Max) Trend For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15313":
                    // ITEM WISE 12 MONTHS RATE (MAX) TREND
                    //SQuery = "SELECT A.ACODE as Vendor_code,C.ANAME as Vendor_name,a.icode as item_code,b.INAME,b.cpartno,B.UNIT,A.TYPE as po_type,max(a.apr) as apr,max(a.may) as may,max(a.jun) as jun,max(a.jul) as jul,max(a.aug) as aug,max(a.sep) as sep,max(a.oct) as oct,max(a.nov) as nov,max(a.dec) as dec,max(a.jan) as jan,max(a.feb) as feb,max(a.mar) as mar  from ( select  ACODE,TYPE,icode,(Case when to_char(ORDDT,'mm')='04' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((prate*(100-pdisc)/100)-pdiscamt)   else 0 end) as mar  from POMAS where " + branch_Cd + " and type like '5%' and ORDDT " + xprdrange + " and icode='" + hfcode.Value + "' ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,A.TYPE,C.ANAME,B.UNIT ORDER BY item_code";
                    SQuery = "SELECT A.ACODE as Vendor_code,trim(C.ANAME) as Vendor_name,a.icode as item_code,trim(b.INAME) as Item_Name,trim(b.hscode) as hscode,trim(b.cpartno) as Part_No,B.UNIT,A.TYPE as po_type,max(a.apr) as apr,max(a.may) as may,max(a.jun) as jun,max(a.jul) as jul,max(a.aug) as aug,max(a.sep) as sep,max(a.oct) as oct,max(a.nov) as nov,max(a.dec) as dec,max(a.jan) as jan,max(a.feb) as feb,max(a.mar) as mar  from(select  trim(ACODE) as acode,TYPE,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then ((nvl(prate,0)*(100-nvl(pdisc,0))/100)-nvl(pdiscamt,0))   else 0 end) as mar  from POMAS where " + branch_Cd + " and type like '5%' and ORDDT " + xprdrange + " and icode='" + hfcode.Value + "' ) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,trim(b.iname),trim(b.cpartno),trim(b.hscode),A.ACODE,A.TYPE,trim(C.ANAME),B.UNIT ORDER BY item_code";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Wise 12 Months Rate (Max) Trend For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15314":////
                    // PR Vs PO Vs MRR Checklist
                    //SQuery = "select x.pr_no,to_char(x.pr_dt,'dd/mm/yyyy') as pr_dt,y.Iname,x.vendor,x.pr_Qty,x.po_Qty,x.mrr_qty,x.rej_qty,x.icode,y.unit,y.cpartno,x.quot,x.po_no,to_char(x.po_date,'dd/mm/yyyy') as po_dt,X.dlv_Date,x.mrr_no,to_char(x.mrr_Dt,'dd/mm/yyyy') as mrr_dt,x.deptt,x.PR_Status,x.app_by,X.Ent_by as PR_Ent_by,x.ent_dt,y.vat_code as Vref from (select M.Ent_By,m.pr_no,m.pr_dt,(Case when m.pflag=1 then 'Curr' else 'Closed' End) as PR_Status,m.app_by,m.quot,m.icode,m.pr_Qty,m.vendor,m.po_no,m.po_date,m.po_Qty,m.po_madeby,M.Dlv_Date,nvl(n.vchnum,'-') as mrr_no,nvl(n.vchdate,sysdate) as mrr_Dt,nvl(n.iqtyin,0) as mrr_qty,nvl(n.rej_Rw,0) as rej_qty,m.deptt,m.ent_Dt,n.genum,n.gedate from ( select x.ordno as pr_no,x.orddt as pr_Dt,x.Ent_By,x.app_by,x.quot,x.icode,x.pflag,x.qtyord as pr_qty,nvl(y.ordno,'-') as po_no,y.aname as Vendor,nvl(y.orddt,sysdate) as po_Date,nvl(y.qtyord,0) as po_qty,nvl(y.delv_item,'-') as Dlv_Date,y.po_madeby,x.deptt,x.ent_Dt from (Select ordno,orddt,icode,qtyord,st38no as quot,ent_Dt,ent_by,bank as Deptt,app_by,pflag,acode from pomas where branchcd='" + mbr + "' and substr(type,1,1)='6' and orddt  " + xprdrange + " )x left outer join (select a.ordno,a.orddt,a.pr_no,a.pr_dt,a.icode,a.QTYORD,a.delv_item,a.acode,b.aname,a.ent_by as po_madeby from pomas a, famst b  where trim(A.acode)=trim(B.acode) and a.branchcd='" + mbr + "' and substr(a.type,1,1)='5' and a.orddt " + xprdrange + "  ) y on trim(x.icode)=trim(y.icode) and trim(x.ordno)=trim(y.pr_no) and trim(x.orddt)=trim(y.pr_Dt) ) m left outer join (select vchnum,vchdate,ponum,podate,icode,iqtyin,rej_rw,genum,gedate from ivoucher where   branchcd='" + mbr + "' and substr(type,1,1)='0' and store='Y' and vchdate " + xprdrange + " ) n on m.icode=n.icode and m.po_no=n.ponum and m.po_date=n.podate order by m.pr_dt,m.pr_no,n.vchdate) x left outer join item y on trim(x.icode)=trim(y.icode) order by x.pr_dt,x.pr_no";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select x.pr_no,to_char(x.pr_dt,'dd/mm/yyyy') as pr_dt,trim(y.Iname) as iname,x.vendor,x.pr_Qty,x.po_Qty,x.mrr_qty,x.rej_qty,x.icode,y.unit,y.cpartno,x.quot,x.po_no,to_char(x.po_date,'dd/mm/yyyy') as po_dt,X.dlv_Date,x.mrr_no,to_char(x.mrr_Dt,'dd/mm/yyyy') as mrr_dt,x.deptt,x.PR_Status,x.app_by,X.Ent_by as PR_Ent_by,x.ent_dt,y.vat_code as Vref from (select M.Ent_By,m.pr_no,m.pr_dt,(Case when m.pflag=1 then 'Curr' else 'Closed' End) as PR_Status,m.app_by,m.quot,m.icode,m.pr_Qty,m.vendor,m.po_no,m.po_date,m.po_Qty,m.po_madeby,M.Dlv_Date,nvl(n.vchnum,'-') as mrr_no,nvl(n.vchdate,sysdate) as mrr_Dt,nvl(n.iqtyin,0) as mrr_qty,nvl(n.rej_Rw,0) as rej_qty,m.deptt,m.ent_Dt,n.genum,n.gedate from  (select x.ordno as pr_no,x.orddt as pr_Dt,x.Ent_By,x.app_by,x.quot,x.icode,x.pflag,x.qtyord as pr_qty,nvl(y.ordno,'-') as po_no,y.aname as Vendor,nvl(y.orddt,sysdate) as po_Date,nvl(y.qtyord,0) as po_qty,nvl(y.delv_item,'-') as Dlv_Date,y.po_madeby,x.deptt,x.ent_Dt from ( Select ordno,orddt,trim(icode) as icode,nvl(qtyord,0) as qtyord,st38no as quot,ent_Dt,ent_by,bank as Deptt,app_by,pflag,trim(acode) as acode from pomas where branchcd='" + mbr + "' and substr(type,1,1)='6' and orddt  " + xprdrange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%' )x left outer join (select a.ordno,a.orddt,a.pr_no,a.pr_dt,trim(a.icode) as icode,nvl(a.QTYORD,0) as qtyord,a.delv_item,trim(a.acode) as acode,trim(b.aname) as aname,a.ent_by as po_madeby from pomas a, famst b  where trim(A.acode)=trim(B.acode) and a.branchcd='" + mbr + "' and substr(a.type,1,1)='5' and a.orddt " + xprdrange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%') y on trim(x.icode)=trim(y.icode) and trim(x.ordno)=trim(y.pr_no) and trim(x.orddt)=trim(y.pr_Dt) ) m left outer join (select vchnum,vchdate,ponum,podate,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,nvl(rej_rw,0) as rej_Rw,genum,gedate from ivoucher where branchcd='" + mbr + "' and substr(type,1,1)='0' and store='Y' and vchdate " + xprdrange + " and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%') n on m.icode=n.icode and m.po_no=n.ponum and m.po_date=n.podate order by m.pr_dt,m.pr_no,n.vchdate ) x left outer join item y on trim(x.icode)=trim(y.icode) order by x.pr_dt,x.pr_no";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("PR Vs PO Vs MRR (Qty based) For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15315":
                    header_n = "PO Delivery Date Vs Rcpt Date Checklist";
                    // SQuery = "select a.acode,b.aname as party,a.icode,c.iname as item_name,c.cpartno as part,c.unit,a.type,a.ordno,a.qty as po_qty,a.recd as recd_qty,del as del_dt, vch as Recpt_dt, to_date(del,'dd/mm/yyyy')-to_date(vch,'dd/mm/yyyy') as diff_days from (select acode,icode,ordno,max(del_date) as del,max(vchdate) as vch,sum(qty) as qty,sum(recd) as recd, max(type) AS TYPE from (select trim(acode) as acode,trIm(icode) as icode,trim(type) as type,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,to_char(del_date,'dd/mm/yyyy') as del_date,null as vchdate,qtyord as qty, 0 as recd from pomas where branchcd='" + mbr + "' and  type like '5%' and orddt " + xprdrange + " union all select trim(acode) as acode,trim(icode) as icode,trim(potype) as type,trim(ponum) AS PONUM, to_char(podate,'dd/mm/yyyy') as podate ,null as del_date,to_char(vchdate,'dd/mm/yyyy') as vchdate,0 as qty, iqtyin as recd from ivoucher where branchcd='" + mbr + "'and substr(potype,1,1)='5' and vchdate " + xprdrange + " and store='Y') group by acode,icode,ordno,type) a,famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by ordno";
                    //SQuery = "select a.acode,b.aname as party,a.icode,c.iname as item_name,c.cpartno as part,c.unit,a.type,a.ordno,a.qty as po_qty,a.recd as recd_qty,del as del_dt, vch as Recpt_dt, to_date(del,'dd/mm/yyyy')-to_date(vch,'dd/mm/yyyy') as diff_days from (select acode,icode,ordno,del_date as del,vchdate as vch,sum(qty) as qty,sum(recd) as recd, max(type) AS TYPE from (select trim(acode) as acode,trIm(icode) as icode,trim(type) as type,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,to_char(del_date,'dd/mm/yyyy') as del_date,null as vchdate,qtyord as qty, 0 as recd from pomas where branchcd='" + mbr + "' and  type like '5%' and orddt " + xprdrange + " union all select trim(acode) as acode,trim(icode) as icode,trim(potype) as type,trim(ponum) AS PONUM, to_char(podate,'dd/mm/yyyy') as podate ,null as del_date,to_char(vchdate,'dd/mm/yyyy') as vchdate,0 as qty, iqtyin as recd from ivoucher where branchcd='" + mbr + "'and substr(potype,1,1)='5' and vchdate " + xprdrange + " and store='Y') group by acode,icode,ordno,del_date,vchdate) a,famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by ordno";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select a.acode,trim(b.aname) as party,a.icode,trim(c.iname) as item_name,trim(c.cpartno) as part,c.unit,a.type,a.ordno,a.qty as po_qty,a.recd as recd_qty,del as del_dt, vch as Recpt_dt, to_date(del,'dd/mm/yyyy')-to_date(vch,'dd/mm/yyyy') as diff_days from (select acode,icode,ordno,del_date as del,vchdate as vch,sum(qty) as qty,sum(recd) as recd, max(type) AS TYPE from (select trim(acode) as acode,trIm(icode) as icode,trim(type) as type,trim(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,to_char(del_date,'dd/mm/yyyy') as del_date,null as vchdate,nvl(qtyord,0) as qty, 0 as recd from pomas where branchcd='" + mbr + "' and  type like '5%' and orddt " + xprdrange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all  select trim(acode) as acode,trim(icode) as icode,trim(potype) as type,trim(ponum) AS PONUM, to_char(podate,'dd/mm/yyyy') as podate ,null as del_date,to_char(vchdate,'dd/mm/yyyy') as vchdate,0 as qty, nvl(iqtyin,0) as recd from ivoucher where branchcd='" + mbr + "'and substr(potype,1,1)='5' and vchdate " + xprdrange + " and store='Y' and acode like '" + party_cd + "%' and icode like '" + part_cd + "%')  group by acode,icode,ordno,del_date,vchdate ) a,famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) order by ordno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("PO Delivery Date Vs Rcpt Date Checklist For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15316":
                    // BY MADHVI ON 14/03/2018
                    header_n = "PO delivery Date based monthly Calender ";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (hfcode.Value.Contains("%"))
                    {
                        //SQuery = "SELECT A.ACODE AS SUPP_CODE,F.ANAME AS SUPPLIER,A.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,I.CPARTNO AS PARTNO,I.UNIT,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='01' THEN A.bal_qty ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='02' THEN A.bal_qty ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='03' THEN A.bal_qty ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='04' THEN A.bal_qty ELSE 0 END)  AS Day4,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='05' THEN A.bal_qty ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='06' THEN A.bal_qty ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='07' THEN A.bal_qty ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='08' THEN A.bal_qty ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='09' THEN A.bal_qty ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='10' THEN A.bal_qty ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='11' THEN A.bal_qty ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='12' THEN A.bal_qty ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='13' THEN A.bal_qty ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='14' THEN A.bal_qty ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='15' THEN A.bal_qty ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='16' THEN A.bal_qty ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='17' THEN A.bal_qty ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='18' THEN A.bal_qty ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='19' THEN A.bal_qty ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='20' THEN A.bal_qty ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='21' THEN A.bal_qty ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='22' THEN A.bal_qty ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='23' THEN A.bal_qty ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='24' THEN A.bal_qty ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='25' THEN A.bal_qty ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='26' THEN A.bal_qty ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='27' THEN A.bal_qty ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='28' THEN A.bal_qty ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='29' THEN A.bal_qty ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='30' THEN A.bal_qty ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='31' THEN A.bal_qty ELSE 0 END) AS Day31, A.TYPE AS PO_TYPE,T.NAME AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.DEL_DATE,'DD/MM/YYYY') AS PO_DT FROM wbvu_pending_po A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND  a." + branch_Cd + " and A.TYPE LIKE '5%' AND to_char(a.orddt,'yyyymm')=to_char(to_date('" + value1 + "','dd/mm/yyyy'),'yyyymm') ORDER BY PO_NO";
                        SQuery = "SELECT trim(A.ACODE) AS SUPP_CODE,trim(F.ANAME) AS SUPPLIER,trim(A.ICODE) AS ITEM_CODE,trim(I.INAME) AS ITEM_NAME,trim(I.CPARTNO) AS PARTNO,I.UNIT,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='01' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='02' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='03' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='04' THEN nvl(A.bal_qty,0) ELSE 0 END)  AS Day4,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='05' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='06' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='07' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='08' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='09' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='10' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='11' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='12' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='13' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='14' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='15' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='16' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='17' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='18' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='19' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='20' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='21' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='22' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='23' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='24' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='25' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='26' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='27' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='28' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='29' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='30' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='31' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day31, A.TYPE AS PO_TYPE,trim(T.NAME) AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.DEL_DATE,'DD/MM/YYYY') AS PO_DT FROM wbvu_pending_po A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND  a." + branch_Cd + " and A.TYPE LIKE '5%' AND to_char(a.orddt,'yyyymm')=to_char(to_date('" + value1 + "','dd/mm/yyyy'),'yyyymm') and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY PO_NO";
                        header_n = "All PO Types";
                    }
                    else
                    {
                        //SQuery = "SELECT A.ACODE AS SUPP_CODE,F.ANAME AS SUPPLIER,A.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,I.CPARTNO AS PARTNO,I.UNIT,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='01' THEN A.bal_qty ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='02' THEN A.bal_qty ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='03' THEN A.bal_qty ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='04' THEN A.bal_qty ELSE 0 END)  AS Day4,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='05' THEN A.bal_qty ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='06' THEN A.bal_qty ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='07' THEN A.bal_qty ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='08' THEN A.bal_qty ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='09' THEN A.bal_qty ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='10' THEN A.bal_qty ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='11' THEN A.bal_qty ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='12' THEN A.bal_qty ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='13' THEN A.bal_qty ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='14' THEN A.bal_qty ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='15' THEN A.bal_qty ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='16' THEN A.bal_qty ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='17' THEN A.bal_qty ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='18' THEN A.bal_qty ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='19' THEN A.bal_qty ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='20' THEN A.bal_qty ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='21' THEN A.bal_qty ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='22' THEN A.bal_qty ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='23' THEN A.bal_qty ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='24' THEN A.bal_qty ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='25' THEN A.bal_qty ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='26' THEN A.bal_qty ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='27' THEN A.bal_qty ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='28' THEN A.bal_qty ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='29' THEN A.bal_qty ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='30' THEN A.bal_qty ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='31' THEN A.bal_qty ELSE 0 END) AS Day31, A.TYPE AS PO_TYPE,T.NAME AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.DEL_DATE,'DD/MM/YYYY') AS PO_DT FROM wbvu_pending_po A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND a." + branch_Cd + " AND a.type IN (" + hfcode.Value + ") and to_char(a.orddt,'yyyymm')=to_char(to_date('" + value1 + "','dd/mm/yyyy'),'yyyymm') ORDER BY PO_NO";
                        SQuery = "SELECT trim(A.ACODE) AS SUPP_CODE,trim(F.ANAME) AS SUPPLIER,trim(A.ICODE) AS ITEM_CODE,trim(I.INAME) AS ITEM_NAME,trim(I.CPARTNO) AS PARTNO,I.UNIT,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='01' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day1,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='02' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day2,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='03' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day3,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='04' THEN nvl(A.bal_qty,0) ELSE 0 END)  AS Day4,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='05' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day5,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='06' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day6,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='07' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day7,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='08' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day8,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='09' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day9,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='10' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day10,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='11' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day11,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='12' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day12,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='13' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day13,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='14' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day14,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='15' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day15,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='16' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day16,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='17' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day17,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='18' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day18,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='19' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day19,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='20' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day20,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='21' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day21,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='22' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day22,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='23' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day23,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='24' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day24,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='25' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day25,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='26' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day26,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='27' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day27,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='28' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day28,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='29' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day29,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='30' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day30,(CASE WHEN TO_CHAR(A.DEL_DATE,'DD')='31' THEN nvl(A.bal_qty,0) ELSE 0 END) AS Day31, A.TYPE AS PO_TYPE,trim(T.NAME) AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.DEL_DATE,'DD/MM/YYYY') AS PO_DT FROM wbvu_pending_po A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND  a." + branch_Cd + " and a.type IN (" + hfcode.Value + ") AND to_char(a.orddt,'yyyymm')=to_char(to_date('" + value1 + "','dd/mm/yyyy'),'yyyymm') and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY PO_NO";
                        header_n = "Showing types :" + hfcode.Value;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending PO Qty (based on delivery Date for the Month " + value1.Substring(3, 7) + ") based monthly Calender " + header_n + "", frm_qstr);
                    break;

                case "F15318": // Purchase Order Delivery Expected during DTD
                    //need to ask mam....when choosse prev year then what will be xprange
                    header_n = "Purchase Order Delivery Expected during DTD";
                    date1 = DateTime.Now.Date;
                    date2 = date1.AddDays(fgen.make_double(hfval.Value));
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    xprdrange = "between to_date('" + date1.ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + date2.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                    if (hf1.Value.Contains("%"))
                    {
                        //SQuery = "SELECT A.TYPE AS PO_TYPE,T.NAME AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS PO_DT,A.ACODE AS SUPP_CODE,F.ANAME AS SUPPLIER,A.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,I.CPARTNO AS PARTNO,I.UNIT,SUM(A.QTYORD) AS QTYORD,to_char(a.del_date,'dd/mm/yyyy') as del_date FROM POMAS A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND a.branchcd='" + mbr + "' and A.TYPE like '5%' and a.del_date " + xprdrange + "  GROUP BY A.TYPE,T.NAME,A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY'),A.ACODE,F.ANAME,A.ICODE,I.INAME,I.CPARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy')  order by PO_NO";
                        SQuery = "SELECT A.TYPE AS PO_TYPE,trim(T.NAME) AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS PO_DT,trim(A.ACODE) AS SUPP_CODE,trim(F.ANAME) AS SUPPLIER,trim(A.ICODE) AS ITEM_CODE,trim(I.INAME) AS ITEM_NAME,trim(I.CPARTNO) AS PARTNO,I.UNIT,SUM(nvl(A.QTYORD,0)) AS QTYORD,to_char(a.del_date,'dd/mm/yyyy') as del_date FROM POMAS A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND a.branchcd='" + mbr + "' and A.TYPE like '5%' and a.del_date " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' group by  A.TYPE,trim(T.NAME),A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY'),trim(A.ACODE),trim(F.ANAME),trim(A.ICODE),trim(I.INAME),trim(I.CPARTNO),I.UNIT,to_char(a.del_date,'dd/mm/yyyy')  order by PO_NO";
                    }
                    else
                    {
                        //SQuery = "SELECT A.TYPE AS PO_TYPE,T.NAME AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS PO_DT,A.ACODE AS SUPP_CODE,F.ANAME AS SUPPLIER,A.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,I.CPARTNO AS PARTNO,I.UNIT,SUM(A.QTYORD) AS QTYORD,to_char(a.del_date,'dd/mm/yyyy') as del_date FROM POMAS A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND a.branchcd='" + mbr + "' and A.TYPE in (" + hf1.Value + ") and a.del_date " + xprdrange + "  GROUP BY A.TYPE,T.NAME,A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY'),A.ACODE,F.ANAME,A.ICODE,I.INAME,I.CPARTNO,I.UNIT,to_char(a.del_date,'dd/mm/yyyy')  order by PO_NO";
                        SQuery = "SELECT A.TYPE AS PO_TYPE,trim(T.NAME) AS PO_NAME,A.ORDNO AS PO_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS PO_DT,trim(A.ACODE) AS SUPP_CODE,trim(F.ANAME) AS SUPPLIER,trim(A.ICODE) AS ITEM_CODE,trim(I.INAME) AS ITEM_NAME,trim(I.CPARTNO) AS PARTNO,I.UNIT,SUM(nvl(A.QTYORD,0)) AS QTYORD,to_char(a.del_date,'dd/mm/yyyy') as del_date FROM POMAS A,ITEM I ,FAMST F,TYPE T WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND a.branchcd='" + mbr + "' and A.TYPE in (" + hf1.Value + ") and a.del_date " + xprdrange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' group by  A.TYPE,trim(T.NAME),A.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY'),trim(A.ACODE),trim(F.ANAME),trim(A.ICODE),trim(I.INAME),trim(I.CPARTNO),I.UNIT,to_char(a.del_date,'dd/mm/yyyy')  order by PO_NO";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Purchase Order Delivery Expected during DTD for Delivery Date within " + value1 + " Days", frm_qstr);
                    break;

                ///yaha se pending hai
                case "F15182": // MERGE BY MADHVI MADE BY AKSHAY ON 25 MAY 2018
                    // TRACK AMENDMENT IN PO
                    value1 = hfcode.Value;
                    SQuery = "select nvl(b.iname,'-') as Name,count(a.amdtno) as Amendments,sum(a.qtyord) as Total_Order,max(a.prate) as Max_Rate,trim(a.icode) as icode,b.cpartno from pomas a left outer join item b on a.icode=b.icode where a.type='" + value1 + "' and substr(a.type,1,1)='5' and a.orddt " + xprdrange + " and (a.prate<>a.o_prate or a.qtyord<>a.o_qty) group by b.iname,b.cpartno,a.icode order by count(a.amdtno) desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Track Amendment In PO From " + fromdt + " To " + todt, frm_qstr);
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

                case "F15180":
                    // open drill down form
                    fgen.drillQuery(0, "select trim(a.acode) as fstr,'' as gstr,trim(b.aname) as party_name,trim(a.ordno) as ORDER_NO, to_char(a.orddt,'dd/mm/yyyy') as ORDER_DT, sum(a.qtyord) as Qty_Ordered, trim(a.acode) as party_code from pomas a , famst b  where trim(a.acode)=trim(b.acode) and  a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + xprdrange + " group by a.ordno,a.orddt,a.acode,b.aname", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.acode)||trim(a.icode) as fstr,trim(a.acode) as gstr,trim(a.ordno) as ponum, to_char(a.orddt,'dd/mm/yyyy') as podt, trim(a.acode) as party_code,trim(b.aname) as party_name, sum(a.qtyord) as Qty,trim(a.icode) as item_code,trim(c.iname) as item_name from pomas a , famst b ,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.orddt " + xprdrange + " group by a.ordno,a.orddt,a.acode,b.aname,a.icode,c.iname", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.acode)||trim(a.icode) as gstr, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.icode) as item_code,c.iname as item_name,trim(a.acode) as party_code,trim(b.aname) as Party_name,a.iqtyin as qty,a.iamount as amount,trim(ponum) as ponum,to_char(podate,'dd/mm/yyyy') as podate from ivoucher a , famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and  a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + "", frm_qstr);
                    fgen.Fn_DrillReport("Purchase Order Tracking Drill Report for the period " + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15181":
                    // open drill down form
                    fgen.drillQuery(0, "SELECT trim(a.acode) as fstr,'' as gstr,trim(B.ANAME) AS PARTY_NAME,SUM(A.TOT_SCH) AS TOT_SCH,SUM(A.TOT_rcpt) AS TOTAL_rcpt,SUM(A.TOT_SCH)-SUM(A.TOT_rcpt) AS BAL, (case when sum(a.tot_sch)=0 then 0 else round((sum(a.tot_rcpt)/sum(a.tot_sch))*100,2) end) as bal_perc ,trim(A.ACODE) AS PARTY_CODE FROM (SELECT TYPE,VCHNUM,VCHDATE,ACODE,TOTAL AS TOT_SCH,0 AS TOT_rcpt FROM SCHEDULE WHERE BRANCHCD='" + mbr + "' AND TYPE='66' AND VCHDATE " + xprdrange + " UNION ALL SELECT TYPE,VCHNUM,VCHDATE,ACODE,0 AS TOT_SCH,iqtyin AS TOT_rcpt FROM ivoucher WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + ") A , FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) GROUP BY A.ACODE,B.ANAME", frm_qstr);
                    fgen.drillQuery(1, "SELECT trim(a.icode)||trim(a.acode) as fstr,trim(a.acode) as gstr,trim(B.ANAME) AS PARTY_NAME ,SUM(A.TOT_SCH) AS TOT_SCH,SUM(A.tot_rcpt) AS TOTAL_DISP,SUM(A.TOT_SCH)-SUM(A.tot_rcpt) AS BAL, (case when sum(a.tot_sch)=0 then 0 else round((sum(a.tot_rcpt)/sum(a.tot_sch))*100,2) end) as bal_perc,trim(A.ACODE) AS PARTY_CODE,trim(a.icode) as Item_code,trim(c.iname) as item_name FROM (SELECT TYPE,VCHNUM,VCHDATE,ACODE,icode,TOTAL AS TOT_SCH,0 AS TOT_rcpt FROM SCHEDULE WHERE BRANCHCD='" + mbr + "' AND TYPE='66' AND VCHDATE " + xprdrange + " UNION ALL SELECT TYPE,VCHNUM,VCHDATE,ACODE,icode,0 AS TOT_SCH,iqtyin AS TOT_rcpt FROM ivoucher WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + ") A , FAMST B, item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode)  GROUP BY A.ACODE,B.ANAME ,a.icode,c.iname", frm_qstr);
                    fgen.drillQuery(2, "select '' as fstr,trim(a.icode)||trim(a.acode) as gstr, trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as Party_code,trim(c.aname) as Party_name,trim(a.icode) as Item_code,trim(b.iname) as Item_name,a.iqtyin as qty,a.iamount as bill_amt,trim(a.invno) as Invno,to_char(a.invdate,'dd/mm/yyyy') as invdate from ivoucher a , item b ,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + "", frm_qstr);
                    fgen.Fn_DrillReport("Schedule VS Receipt Drill Report for the period " + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15286"://SOB REPORT
                    #region maingrp case
                    if (hf1.Value == "MAINGRP")
                    {
                        //SQuery = "SELECT  FSTR,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in ('" + hfSales.Value + "') AND STORE!='R' ) as total FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND STORE!='R' AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND TRIM(SUBSTR(A.ICODE,1,2)) IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME)";
                        SQuery = "SELECT icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE branchcd='" + mbr + "' AND TYPE LIKE '0%'  AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,2)) in (" + hfval.Value + ") AND STORE!='R' and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') ) as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.branchcd='" + mbr + "' AND A.TYPE LIKE '0%' AND A.vchdate " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(A.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,2)) IN (" + hfval.Value + ") GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dt.NewRow();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3)
                                {
                                    oporow[1] = "Total";
                                }
                                else
                                {
                                    double mysum = 0;
                                    foreach (DataRow drc in dt.Rows)
                                    {
                                        mysum += fgen.make_double(drc[dc].ToString());
                                        oporow[dc] = mysum;
                                    }
                                }
                                double total1 = fgen.make_double(dt.Rows[dt.Rows.Count - 1]["QTY"].ToString());
                            }
                            dt.Rows.Add(oporow);
                            Session["send_dt"] = dt;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("Main Group Level Report from " + fromdt + " To " + todt + "", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                    }
                    #endregion
                    //--------------------------------------------
                    #region sub group level
                    if (hf1.Value == "SUBGRP")
                    {
                        // SQuery = "SELECT  FSTR,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,4)) in ('" + hfSales.Value + "') AND STORE!='R') as total FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND STORE!='R' AND A." + branch_Cd + " AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND TRIM(SUBSTR(A.ICODE,1,4))  IN ('" + hfSales.Value + "') GROUP BY A.ACODE,B.ANAME)";
                        SQuery = "SELECT  icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE branchcd='" + mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(SUBSTR(icode,1,4)) in (" + hfval.Value + ") and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R') as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.branchcd='" + mbr + "' AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(a.ACODE),0,2) IN ('05','06') AND TRIM(SUBSTR(A.ICODE,1,4))  IN (" + hfval.Value + ") GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dt.NewRow();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3)
                                {
                                    oporow[1] = "Total";
                                }
                                else
                                {
                                    double mysum = 0;
                                    foreach (DataRow drc in dt.Rows)
                                    {
                                        mysum += fgen.make_double(drc[dc].ToString());
                                        oporow[dc] = mysum;
                                    }
                                }
                                double total1 = fgen.make_double(dt.Rows[dt.Rows.Count - 1]["QTY"].ToString());
                            }
                            dt.Rows.Add(oporow);
                            Session["send_dt"] = dt;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("Sub Group Level Report From " + fromdt + " To " + todt + "", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                    }
                    #endregion
                    //--------------------------------------------
                    #region ITEM LEVEL
                    if (hf1.Value == "ITEM")
                    {
                        //SQuery = "SELECT FSTR,FSTR AS PART_CODE,ANAME AS PARTY,QTY,ROUND((QTY/TOTAL*100),2) as SOB FROM (SELECT A.ACODE AS FSTR,B.ANAME,SUM(A.IQTYIN) AS QTY,a.icode,(SELECT SUM(IQTYIN) AS TOTAL FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(ICODE) in ('" + hfSales.Value + "') AND STORE!='R') AS TOTAL  FROM  IVOUCHER A,FAMST B WHERE A.ICODE in ('" + hfSales.Value + "') AND TRIM(A.ACODE)=TRIM(B.ACODE) AND STORE!='R'   AND A." + branch_Cd + " AND A.TYPE LIKE '0%' and A.VCHDATE " + xprdrange + "  GROUP BY B.ANAME,A.ACODE,a.icode)";
                        SQuery = "SELECT  icode as erp_code,iname as erp_name,FSTR AS PARTY_CODE,PARTY,qty,round((QTY/total*100),2) as sob FROM (SELECT A.ACODE AS FSTR,B.ANAME AS PARTY,c.iname,SUM(A.IQTYIN) AS QTY,(SELECT SUM(IQTYIN) AS QTY1 FROM IVOUCHER WHERE branchcd='" + mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + xprdrange + " AND TRIM(icode) in (" + hfval.Value + ") and SUBSTR(TRIM(ACODE),0,2) IN ('05','06') AND STORE!='R') as total,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.branchcd='" + mbr + "' AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " AND A.STORE!='R' and SUBSTR(TRIM(a.ACODE),0,2) IN ('05','06') AND TRIM(A.ICODE)  IN (" + hfval.Value + ") GROUP BY A.ACODE,B.ANAME,a.icode,c.iname)";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dt.NewRow();
                            foreach (DataColumn dc in dt.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3)
                                {
                                    oporow[1] = "Total";
                                }
                                else
                                {
                                    double mysum = 0;
                                    foreach (DataRow drc in dt.Rows)
                                    {
                                        mysum += fgen.make_double(drc[dc].ToString());
                                        oporow[dc] = mysum;
                                    }
                                }
                                double total1 = fgen.make_double(dt.Rows[dt.Rows.Count - 1]["QTY"].ToString());
                            }
                            dt.Rows.Add(oporow);
                            Session["send_dt"] = dt;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("Item Level Report From " + fromdt + " To " + todt + "", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                    }
                    #endregion
                    break;

                case "F47186":
                    #region ADVG
                    dtm = new DataTable();
                    dtm.Columns.Add("Payment_Dt", typeof(string));
                    dtm.Columns.Add("Party_Code", typeof(string));
                    dtm.Columns.Add("Party", typeof(string));
                    dtm.Columns.Add("Party_Country", typeof(string));
                    dtm.Columns.Add("Po_No", typeof(string));
                    dtm.Columns.Add("Po_Dt", typeof(string));
                    dtm.Columns.Add("So_No", typeof(string));
                    dtm.Columns.Add("So_Dt", typeof(string));
                    dtm.Columns.Add("Dest_Country", typeof(string));
                    dtm.Columns.Add("Inv_Type", typeof(string));
                    dtm.Columns.Add("Name", typeof(string));
                    dtm.Columns.Add("Inv_No", typeof(string));
                    dtm.Columns.Add("Inv_Dt", typeof(string));
                    dtm.Columns.Add("Amount", typeof(string));
                    dtm.Columns.Add("Comm_Agent1", typeof(string));
                    dtm.Columns.Add("Comm_Rate1", typeof(double));
                    dtm.Columns.Add("Comm_Value", typeof(double));
                    dtm.Columns.Add("Comm_Agent2", typeof(string));
                    dtm.Columns.Add("Comm_Rate2", typeof(double));
                    dtm.Columns.Add("Comm_Value2", typeof(string));

                    // ADD ON 01 NOV 2018 BY MADHVI BCOZ IF A SALES ORDER IS CREATED IN 2017 BUT INVOICE IS CREATED IN NEXT FIN. YEAR THEN IT IS NOT PICKING UP AS THE REPORT RUNS ON THE SELECTED DATE RANGE
                    ded2 = "";
                    ded2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(to_date('" + todt + "','dd/mm/yyyy')+300,'dd/mm/yyyy') as dated from dual", "dated");

                    // PAYMENT DATE
                    //ORIGINAL mq4 = "select branchcd,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(acode) as acode,trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate from voucher where " + branch_Cd + " and type like '1%' and substr(acode,0,2) in ('02','06','16','18') and invdate " + xprdrange + "";
                    mq4 = "select branchcd,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(acode) as acode,trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate from voucher where " + branch_Cd + " and SUBSTR(TRIM(TYPE),1,1) IN ('1','3') AND TYPE <'31' and substr(acode,0,2) in ('02','06','16','18') and invdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + ded2 + "','dd/mm/yyyy')";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);

                    // BASED ON INV NO AND DT GETS THE INV TOTAL
                    mq3 = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as vch,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.bill_tot,f.addr4 as country,t.name,a.type from type t,sale a,famst f where trim(a.type)=trim(t.type1) and t.id='V' and trim(a.acode)=trim(f.acode) and a." + branch_Cd + " and a.type like '4%' and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + ded2 + "','dd/mm/yyyy')";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    // BASE ON SO NO AND DT GET THE INV NO AND DT
                    mq2 = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as vch,trim(a.branchcd) as branchcd,trim(a.type) as type,trim(a.acode) as acode,trim(a.ponum) as ponum,to_char(a.podate,'dd/mm/yyyy') as podate from ivoucher a where a." + branch_Cd + " and a.type like '4%' and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + ded2 + "','dd/mm/yyyy')";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    // GETTING COMM. AGENT NAME AND RATE
                    mq1 = "select distinct a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.desc5,a.desc6,a.desc7,a.desc8 from somas a where a." + branch_Cd + " and a.type like '8%' and a.orddt " + xprdrange + "";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    // GETTING OTHER DETAILS FROM SOMAS TABLE
                    mq0 = "select distinct a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,a.pordno,to_char(a.porddt,'dd/mm/yyyy') as porddt,f.aname,f.addr4 as country,to_char(a.orddt,'yyyymmdd') as vdd  from somas a ,famst f where trim(a.acode)=trim(f.acode) and a." + branch_Cd + " and type like '4%' and orddt " + xprdrange + " order by vdd,a.ordno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    DataRow dr1 = null;
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1.ToTable(true, "branchcd", "type", "ordno", "orddt", "acode");
                        foreach (DataRow dr in dtdrsim.Rows)
                        {
                            DataView view2 = new DataView(dt, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='" + dr["type"].ToString().Trim() + "' and ordno='" + dr["ordno"].ToString().Trim() + "' and orddt='" + dr["orddt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = view2.ToTable();

                            dticode2 = new DataTable();
                            if (dt2.Rows.Count > 0)
                            {
                                dv = new DataView(dt2, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and type='" + dr["type"].ToString().Trim() + "' and ponum='" + dr["ordno"].ToString().Trim() + "' and podate='" + dr["orddt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode2 = dv.ToTable();
                            }
                            ded1 = "";
                            for (int i = 0; i < dticode2.Rows.Count; i++)
                            {
                                dr1 = dtm.NewRow();
                                dr1["Party_Code"] = dticode.Rows[0]["acode"].ToString();
                                dr1["Party"] = dticode.Rows[0]["aname"].ToString();
                                if (dr["type"].ToString().Trim() == "4F")
                                {
                                    dr1["Party_Country"] = dticode.Rows[0]["country"].ToString();
                                }
                                else
                                {
                                    dr1["Party_Country"] = "India";
                                }
                                dr1["Po_No"] = dticode.Rows[0]["pordno"].ToString();
                                dr1["Po_Dt"] = dticode.Rows[0]["porddt"].ToString();
                                dr1["So_No"] = dticode.Rows[0]["ordno"].ToString();
                                dr1["So_Dt"] = dticode.Rows[0]["orddt"].ToString();
                                ded1 = dticode2.Rows[i]["vch"].ToString().Trim();
                                if (dt3.Rows.Count > 0)
                                {
                                    dr1["Inv_Type"] = fgen.seek_iname_dt(dt3, "vch='" + ded1 + "'", "type");
                                    dr1["Name"] = fgen.seek_iname_dt(dt3, "vch='" + ded1.Trim() + "'", "name");
                                    dr1["Inv_No"] = fgen.seek_iname_dt(dt3, "vch='" + ded1.Trim() + "'", "vchnum");
                                    dr1["Inv_Dt"] = fgen.seek_iname_dt(dt3, "vch='" + ded1.Trim() + "'", "vchdate");
                                    dr1["Amount"] = fgen.make_double(fgen.seek_iname_dt(dt3, "vch='" + ded1.Trim() + "'", "bill_tot"));
                                    if (dr["type"].ToString() == "4F")
                                    {
                                        dr1["Dest_Country"] = fgen.seek_iname_dt(dt3, "vch='" + ded1.Trim() + "'", "country");
                                    }
                                    else
                                    {
                                        dr1["Dest_Country"] = "India";
                                    }
                                    if (dr1["Inv_Type"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Inv_Type"] = dr1["Inv_Type"].ToString().Trim().Replace("0", "-");
                                    }
                                    if (dr1["Name"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Name"] = dr1["Name"].ToString().Trim().Replace("0", "-");
                                    }
                                    if (dr1["Inv_No"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Inv_No"] = dr1["Inv_No"].ToString().Trim().Replace("0", "-");
                                    }
                                    if (dr1["Inv_Dt"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Inv_Dt"] = dr1["Inv_Dt"].ToString().Trim().Replace("0", "-");
                                    }
                                    if (dr1["Dest_Country"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Dest_Country"] = dr1["Dest_Country"].ToString().Trim().Replace("0", "-");
                                    }
                                    db1 = fgen.make_double(dr1["Amount"].ToString());
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    dr1["Comm_Agent1"] = fgen.seek_iname_dt(dt1, "branchcd='" + dr["branchcd"].ToString().Trim() + "' And ordno='" + dr["ordno"].ToString().Trim() + "' and orddt='" + dr["orddt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "desc5");
                                    dr1["Comm_Rate1"] = fgen.make_double(fgen.seek_iname_dt(dt1, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and ordno='" + dr["ordno"].ToString().Trim() + "' and orddt='" + dr["orddt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "desc6"));
                                    db2 = fgen.make_double(dr1["Comm_Rate1"].ToString());
                                    db3 = (db1 / 100) * db2;
                                    dr1["Comm_Value"] = db3;
                                    dr1["Comm_Agent2"] = fgen.seek_iname_dt(dt1, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and ordno='" + dr["ordno"].ToString().Trim() + "' and orddt='" + dr["orddt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "desc7");
                                    dr1["Comm_Rate2"] = fgen.make_double(fgen.seek_iname_dt(dt1, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and ordno='" + dr["ordno"].ToString().Trim() + "' and orddt='" + dr["orddt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "desc8"));
                                    db4 = fgen.make_double(dr1["Comm_Rate2"].ToString());
                                    db5 = (db1 / 100) * db4;
                                    dr1["Comm_Value2"] = db5;
                                    if (dr1["Comm_Agent1"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Comm_Agent1"] = dr1["Comm_Agent1"].ToString().Trim().Replace("0", "-");
                                    }
                                    if (dr1["Comm_Agent2"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Comm_Agent2"] = dr1["Comm_Agent2"].ToString().Trim().Replace("0", "-");
                                    }
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    dr1["Payment_Dt"] = fgen.seek_iname_dt(dt4, "branchcd='" + dr["branchcd"].ToString().Trim() + "' and invno='" + dr1["Inv_No"].ToString().Trim() + "' and invdate='" + dr1["Inv_Dt"].ToString().Trim() + "' and acode='" + dr["acode"].ToString().Trim() + "'", "vchdate");
                                    if (dr1["Payment_Dt"].ToString().Trim().Length == 1)
                                    {
                                        dr1["Payment_Dt"] = dr1["Payment_Dt"].ToString().Trim().Replace("0", "-");
                                    }
                                }
                                dtm.Rows.Add(dr1);
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Commission Report From " + fromdt + " To " + todt, frm_qstr);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Data Not Found");
                    }
                    #endregion
                    break;

                case "F15134A":
                    SQuery = "SELECT MSGTXT AS SUBJECT,MSgTO AS EMAILTO,MSGFROM AS SENDER,VCHDATE AS MAILDT,VCHNUM AS ENTRYNO,TO_CHAR(vCHDATE,'YYYYMMDD') AS VDD FROM MAILBOX3 WHERE BRANCHCD='" + mbr + "' AND TYPE='20' AND VCHDATE " + xprdrange + " order by VDD DESC,VCHNUM DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("PO Schedule Mail for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
            }
        }
    }
}