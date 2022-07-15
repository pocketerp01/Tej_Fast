using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_sys : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
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
        { hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidate report'13'(No for branch wise)"); }
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F55132":
                case "F55133":
                case "F55134":
                case "F55126":
                case "F55127":
                    SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V' and type1='4F' ORDER BY code";
                    header_n = "Select Sale Type";
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F55132" || val == "F55126" || val == "F55127" || val == "F55128")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            #region this region add by yogita
            //THIS ELSE STATEMENT ADD BY YOGITA 
            else
            {
                switch (val)
                {
                    case "F55133":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Customers";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,A.ACODE as code,B.ANAME as name FROM ivoucher A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.TYPE='4F' order by code";      //ONLY WAHI PARTY JINKA SO BNA HUA H
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
                    case "F55134":
                        if (hf1.Value == "")
                        {
                            header_n = "Select Products";
                            hf1.Value = value1;
                            SQuery = "SELECT DISTINCT trim(A.ICODE) AS FSTR,A.ICODE as code,B.INAME as name FROM ivoucher A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.TYPE='4F' order by code";////ONLY WAHI ITEM JINKA SO BAN HUA H
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
                    default:
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
                case "F99163":
                    SQuery = "SELECT a.Username,a.Deptt,decode(trim(ulevel),'0','0:TOP LEVEL','1','1:Administrator','2','2:Department Head','2.5','2:View Rights','3:Operator','4:Secured') Rights,a.Can_ADD,a.Can_edit,a.Can_del,a.allowbr as Br_allowed,a.mdeptt as Multi_deptt,a.branchcd,a.userid,a.ent_by,a.ent_Dt,a.edt_by,a.edt_Dt,a.close_by,a.close_dt  FROM evas a, type b where a.branchcd=b.type1 and b.id='B' and a.branchcd!='DD'  order by a.Userid";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Tejaxo ERP User list ", frm_qstr);
                    break;
                case "F99231":
                    SQuery = "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name as Document_name,COUNT(*) Total_Created_Document FROM pomas A ," +
                        "TYPE B WHERE A.TYPE=B.TYPE1 AND B.ID = 'M' and a.ent_dt " + xprdrange + " GROUP BY a.TYPE,a.ENT_BY,B.NAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Data Entry Stats (Purchase) ", frm_qstr);
                    break;
                case "F99232":
                    SQuery = "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name as Document_name,COUNT(*) Total_Created_Document FROM IVOUCHER A ," +
                        "TYPE B WHERE A.TYPE=B.TYPE1 AND B.ID = 'M' and a.ent_dt " + xprdrange + " " +
                        "GROUP BY a.TYPE,a.ENT_BY,B.NAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Data Entry Stats (Store) ", frm_qstr);
                    break;
                case "F99241":
                    SQuery = "SELECT Document_Created_By,TYPE,Document_name,Total_Created_Document FROM(" +
                        "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name as Document_name,COUNT(*) " +
                        "Total_Created_Document FROM pomas A, TYPE B WHERE A.TYPE = B.TYPE1 AND B.ID = 'M' and a.ent_dt " + xprdrange + " " +
                        "GROUP BY a.TYPE,a.ENT_BY,B.NAME union all SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name as Document_name" +
                        ",COUNT(*) Total_Created_Document FROM IVOUCHER A, TYPE B WHERE A.TYPE = B.TYPE1 AND B.ID = 'M' and a.ent_dt " + xprdrange + " " +
                        "and a.type <> '40' GROUP BY a.TYPE,a.ENT_BY,B.NAME union all SELECT a.ENT_BY AS Document_Created_By,a.TYPE," +
                        "b.name || ' - INVOICE' as Document_name,COUNT(*) Total_Created_Document FROM IVOUCHER A, TYPE B WHERE A.TYPE = B.TYPE1 AND " +
                        "B.ID = 'M' and substr(a.type,1,1)= '4'  and a.ent_dt " + xprdrange + " GROUP BY a.TYPE,a.ENT_BY,B.NAME union all " +
                        "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name || ' - ORDER' as Document_name,COUNT(*) Total_Created_Document FROM SOMAS A" +
                        ", TYPE B WHERE A.TYPE = B.TYPE1 AND B.ID = 'M'  and a.ent_dt " + xprdrange + " GROUP BY a.TYPE,a.ENT_BY,B.NAME union all " +
                        "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name || ' - VOUCHER' as Document_name,COUNT(*) Total_Created_Document FROM VOUCHER A,TYPE B " +
                        "WHERE A.TYPE = B.TYPE1 AND B.ID = 'M' and a.ent_date " + xprdrange + " GROUP BY a.TYPE,a.ENT_BY,B.NAME)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Who Did What For The Period " + xprdrange + " ", frm_qstr);
                    break;
                case "F99233":
                    SQuery = "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name ||' - INVOICE' as Document_name,COUNT(*) Total_Created_Document FROM IVOUCHER A ," +
                        "TYPE B WHERE A.TYPE=B.TYPE1 AND B.ID = 'M' and a.ent_dt " + xprdrange + " and substr(a.type,1,1)='4' " +
                        "GROUP BY a.TYPE,a.ENT_BY,B.NAME union all SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name ||' - ORDER' as Document_name,COUNT(*) Total_Created_Document FROM SOMAS A ," +
                        "TYPE B WHERE A.TYPE=B.TYPE1 AND B.ID = 'M' and a.ent_dt " + xprdrange + " " +
                        "GROUP BY a.TYPE,a.ENT_BY,B.NAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Data Entry Stats (Sales) ", frm_qstr);
                    break;
                case "F99234":
                    SQuery = "SELECT a.ENT_BY AS Document_Created_By,a.TYPE,b.name as Document_name,COUNT(*) Total_Created_Document FROM VOUCHER A ," +
                        "TYPE B WHERE A.TYPE=B.TYPE1 AND B.ID = 'M' and a.ent_date " + xprdrange + " " +
                        "GROUP BY a.TYPE,a.ENT_BY,B.NAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Data Entry Stats (Accounts) ", frm_qstr);
                    break;

                //Exp sales checklist                                       
                case "F99126":
                    SQuery = "Select Iname as Item_Name,Icode as ERP_Code,Cpartno as Part_no,Cdrgno as Drg_No,Unit,HScode,Ent_by,Ent_Dt,Edt_by,Edt_Dt from item where to_date(to_char(ent_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + xprdrange + " order by ent_Dt,Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Items Opened During the Selected Period ", frm_qstr);
                    break;

                case "F99127":
                    SQuery = "Select Aname,Acode,Addr1,Addr2,Staten,GST_No,Ent_by,Ent_Dt,Edt_by,Edt_Dt from famst where to_date(to_char(ent_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + xprdrange + " order by ent_Dt,Aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Account Ledgers Opened During the Selected Period ", frm_qstr);
                    break;
                case "F99128":
                    SQuery = "Select Iname as Item_Name,Icode as ERP_Code,Cpartno as Part_no,Cdrgno as Drg_No,Unit,HScode,Ent_by,Ent_Dt,Edt_by,Edt_Dt from item where length(Trim(nvl(edt_by,'-')))>1 and to_date(to_char(edt_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + xprdrange + " order by edt_Dt,Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Items Edited During the Selected Period ", frm_qstr);
                    break;

                case "F99129":
                    SQuery = "Select Aname,Acode,Addr1,Addr2,Staten,GST_No,Ent_by,Ent_Dt,Edt_by,Edt_Dt from famst where length(Trim(nvl(edt_by,'-')))>1 and and to_date(to_char(edt_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + xprdrange + " order by edt_Dt,Aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Account Ledgers Edited During the Selected Period ", frm_qstr);
                    break;

                case "F99143":
                    SQuery = "Select * from FIN_MSYS order by id";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Finsys Web ERP Options ", frm_qstr);
                    break;

                case "F99141":
                    SQuery = "select lpad(rownum,3,'0') as Srno,terminal,to_char(logon_time,'dd/mm/yyyy HH24:MI:SS') as logon_time,sid,serial# from v$session where trim(status)<>'KILLED' and SCHEMANAME<>'SYS' order by terminal";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Finsys Web ERP Sessions ", frm_qstr);
                    break;

                case "F99142":
                    // open drill down form
                    fgen.drillQuery(0, "select upper(fcomment) as fstr,'-' as gstr,upper(fcomment) as Erp_Action,Count(Vchnum) as Actions,' ' as Lookup from  fininfo where VCHDATE " + xprdrange + " group by upper(fcomment) order by upper(fcomment)", frm_qstr);
                    fgen.drillQuery(1, "Select vchdate as fstr,upper(fcomment)  as gstr,Branchcd,Type,Vchnum as Track_no,to_char(Vchdate,'dd/mm/yyyy') as Track_Date,Ent_by as User_ID,Ent_dt as Dated,Terminal as Computer_Name,to_char(vchdate,'yyyymmdd') AS vdd from fininfo where VCHDATE " + xprdrange + " order by Ent_dt Desc,vdd desc,vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);

                    //fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    //fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    //fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    //fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    //fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    //fgen.Fn_DrillReport("", frm_qstr);
                    break;



            }
        }
    }
}