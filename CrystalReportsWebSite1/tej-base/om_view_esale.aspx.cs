using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_esale : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow dr1, oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
    string frm_UserID;
    string party_cd;
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

                case "F55523":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F55522":
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from wb_exp_frt a where a.branchcd='" + mbr + "' and a.type='10' and vchdate " + xprdrange + " order by vdd desc,a.vchnum desc";
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
                    case "F55522":
                        hfcode.Value = value1;
                        col1 = hfcode.Value;
                        dt1 = new DataTable();
                        dt1.Columns.Add("INVOICE_NO", typeof(string));
                        dt1.Columns.Add("VALUE", typeof(double));
                        dt1.Columns.Add("CUSTOMER", typeof(string));
                        dt1.Columns.Add("SHIPPING_LINE", typeof(string));
                        dt1.Columns.Add("DISPATCH_DATE", typeof(string));
                        dt1.Columns.Add("B_L", typeof(string));
                        dt1.Columns.Add("CONATINER", typeof(string));
                        dt1.Columns.Add("STUFFING_DATE", typeof(string));
                        dt1.Columns.Add("RAILOUT_DATE", typeof(string));
                        dt1.Columns.Add("NAME_OF_OCEAN_VESSEL", typeof(string));
                        dt1.Columns.Add("SOB_DATE", typeof(string));
                        dt1.Columns.Add("ETA", typeof(string));
                        dt1.Columns.Add("DOCS_SENT_ON", typeof(string));
                        dt1.Columns.Add("DO_DATE", typeof(string));
                        dt1.Columns.Add("WAREHOUSE_DEL_DATE", typeof(string));
                        dt1.Columns.Add("NO_DAYS", typeof(double));

                        SQuery = "select DISTINCT a.vchnum,C.ACODE,A.CONT_SIZE ,B.NAME AS SHIPPING_LINE,trim(A.invno) as invoice_no,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS invdate,A.iamount as value,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS  shipment_date, A.DATE1 AS DISPATCH_DATE,A.OBSV3 AS B_L ,A.container_no,A.DATE2 AS STUFFING_DATE,A.DATE3 AS RAILOUT_DATE,A.CINAME ,A.DATE4 AS SOB_DATE,A.DATE5 AS ETA,A.DATE6 AS DOC_SENT_ON,A.DATE7 AS do_date, a.date8 as WH_DEL_DATE from wb_exp_frt A, TYPEGRP B,IVOUCHERP C WHERE TRIM(A.OBSV2) = TRIM(B.TYPE1) AND TRIM(A.INVNO)||TO_CHAR(A.INVDATE,'DD/MM/YYYY')=TRIM(C.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND B.ID='^A' AND A.BRANCHCD='" + mbr + "' AND A.TYPE='10' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.vchnum";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["INVOICE_NO"] = dt.Rows[i]["invoice_no"].ToString().Trim();
                            dr1["VALUE"] = dt.Rows[i]["value"].ToString().Trim();
                            dr1["CUSTOMER"] = fgen.seek_iname(frm_qstr, co_cd, "SELECT TRIM(ANAME) AS PARTY_NAME FROM FAMST WHERE TRIM(ACODE)='" + dt.Rows[i]["ACODE"].ToString().Trim() + "'", "PARTY_NAME");
                            dr1["SHIPPING_LINE"] = dt.Rows[i]["SHIPPING_LINE"].ToString().Trim();
                            dr1["DISPATCH_DATE"] = dt.Rows[i]["DISPATCH_DATE"].ToString().Trim();
                            dr1["B_L"] = dt.Rows[i]["B_L"].ToString().Trim();
                            dr1["CONATINER"] = dt.Rows[i]["CONTAINER_NO"].ToString().Trim();
                            dr1["STUFFING_DATE"] = dt.Rows[i]["STUFFING_DATE"].ToString().Trim();
                            dr1["RAILOUT_DATE"] = dt.Rows[i]["RAILOUT_DATE"].ToString().Trim();
                            dr1["NAME_OF_OCEAN_VESSEL"] = dt.Rows[i]["CINAME"].ToString().Trim();
                            dr1["SOB_DATE"] = dt.Rows[i]["SOB_DATE"].ToString().Trim();
                            dr1["ETA"] = dt.Rows[i]["ETA"].ToString().Trim();
                            dr1["DOCS_SENT_ON"] = dt.Rows[i]["DOC_SENT_ON"].ToString().Trim();
                            dr1["DO_DATE"] = dt.Rows[i]["DO_DATE"].ToString().Trim();
                            dr1["WAREHOUSE_DEL_DATE"] = dt.Rows[i]["WH_DEL_DATE"].ToString().Trim();
                            if (dt.Rows[i]["DISPATCH_DATE"].ToString().Trim().Length > 1 || dt.Rows[i]["WH_DEL_DATE"].ToString().Trim().Length > 1)
                            {
                                DateTime date1 = Convert.ToDateTime(dt.Rows[i]["DISPATCH_DATE"].ToString());
                                DateTime date2 = Convert.ToDateTime(dt.Rows[i]["WH_DEL_DATE"].ToString());
                                TimeSpan date3 = (date2 - date1);
                                dr1["NO_DAYS"] = date3.Days;
                            }
                            else
                            {
                                dr1["NO_DAYS"] = "0";
                            }
                            dt1.Rows.Add(dr1);
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            Session["send_dt"] = dt1;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("Shipment Tracking Report For the Period " + fromdt + " To " + todt, frm_qstr);
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
                //Exp sales checklist                                       
                case "F55132":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Data Search(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F55133":
                    if (hfcode.Value.Length > 0)
                    {
                        SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ACODE IN (" + hfcode.Value + ") and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    else
                    {
                        SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ACODE like '%' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Sales(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F55134":
                    if (hfcode.Value.Length > 0)
                    {
                        SQuery = "Select " + rep_flds + "  from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ICODE IN (" + hfcode.Value + ") and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    else
                    {
                        SQuery = "Select " + rep_flds + "  from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ICODE like '%' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Product. Wise Sales(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                //Export order checklist
                case "F55126":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Order Data Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F55127":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Order Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                // ON HOLD AS PER PUNEET SIR'S INSTRUCTION
                case "F55128":  //in reps config join condintion not saved the full joining condition becoz of length
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " ," + table4 + "  where a." + branch_Cd + "  and a.type='46' and b.type like '4%' and b.type not in ('47','45') and " + datefld + " " + xprdrange + " and " + joinfld + " AND trim(a.acode)=trim(c.acode)  and trim(a.icode)=trim(d.icode) group by A.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),c.aname,d.iname,d.cpartno,a.irate,d.UNIT,A.ICODE,A.ent_by,to_char(a.ent_dt,'dd/MM/yyyy'),to_char(a.vchdate,'yyyyMMdddd') order by " + sortfld;
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Pending Schedule Search(Dom.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F55523":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    SQuery = "SELECT TRIM(A.ACODE) AS PARTY_CODE,TRIM(B.ANAME) AS PARTY_NAME,SUM(A.IAMOUNT) AS TOTAL_VALUE_EXPORT_FC,SUM(A.NUM18) AS TOTAL_FREIGHT_FC,ROUND((SUM(A.NUM18/A.IAMOUNT))*100,2) AS RATIO_perc,TRIM(C.NAME) AS NATURE_oF_SHIPMENT,COUNT(CONTAINER_NO) AS NO_OF_CONTAINER,d.name as freight_name FROM WB_EXP_FRT A , FAMST B,TYPEGRP C,typegrp d  WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(A.OBSV1)=TRIM(C.TYPE1) and TRIM(A.OBSV4)=TRIM(d.TYPE1) AND d.ID='^9' AND C.ID='^B' AND A.BRANCHCD='04' AND A.TYPE='10' AND A.VCHDATE " + xprdrange + " AND TRIM(A.ACODE) IN (" + party_cd + ") GROUP BY TRIM(A.ACODE),TRIM(B.ANAME),TRIM(C.NAME),d.name ORDER BY PARTY_CODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Freight Chart for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                
            }
        }
    }
}