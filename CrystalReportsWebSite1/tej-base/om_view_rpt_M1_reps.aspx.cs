using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_rpt_M1_reps : System.Web.UI.Page
{
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joincond;
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, cond, cond1, ratefld, ratefldexc, wt_rate, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2, mhd;
    string xprd2, opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID, frm_AssiID, frm_UserID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, tco_cd, mlvl, cdt1, cdt2, col1;
    int i0, i1, i2, i3, i4, v;
    double month, to_cons, itot_stk, itv;
    double total, total1, total2, total3, total4, total5, total6, total7;
    double db1, db2, db3, db4, db5, db6, db7, db8, db9, db10, db11, db12, db13, db14, db15, db16, db17, db18, db19, db20, db21, db22, db23, db24, db25, db26, db27, db28, db29, db30 = 0;
    DateTime date1, date2;
    DataSet ds, ds3;
    DataTable dt, dt1, dt2, dt3, mdt, dtraw, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dtm, dtm1, dtm11, dtdrsim2, dt11, dt12, dt13, dt14, dt15, dt16, dt17, dt18, dt19, dt20, dtraw1, dtjob1, dtmdt1, dtOp;
    DataTable dticode, dticode2, dtdrsim, vdt, fmdt, mdt1, dt_dist;
    DataTable dtbal1, dtbal2, dtbal3, dtbal4, dtbal5, dtbal6, dtbal7, dtbal8, dtbal9, dtbal10;
    DataRow oporow, drrow1, dro1, dro, dr31, dr1, dr2, dr4, dr5, dr3, dr6, dr7, dr8, dr9, dr10;
    DataRow ROWICODE, ROWICODE2;
    DataView dv, view1im, view1, view2, dv2, dv3, dv4, dv5, dv6, dv7, dv8, dv9, dv10, dv11, dv12, dv13, dv14, dv15, dv16, dv17, dv18, dv19, dv20, mvdview, vdview, sort_view, vdview1;

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
                case "15192":
                    SQuery = "SELECT NAME AS FSTR,NAME, TYPE1 AS CODE FROM TYPE WHERE ID='1' AND TYPE1 LIKE '6%' ORDER BY TYPE1";
                    header_n = "Select Option";
                    i4 = 1;
                    break;
                case "15194":
                case "15195":
                case "15196":
                case "15197":
                    SQuery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,TYPE1,NAME FROM TYPEGRP WHERE ID='JL' ORDER BY TYPE1";
                    header_n = "Select Departments";
                    break;
                //case "15197":
                //    SQuery = "SELECT '30' AS FSTR,'30 Days' as Days,'-' FROM DUAL UNION ALL SELECT '60' AS FSTR,'60 Days' as Days,'-' FROM DUAL UNION ALL SELECT '90' AS FSTR,'90 Days' as Days,'-' FROM DUAL UNION ALL SELECT '180' AS FSTR,'180 Days' as Days,'-' FROM DUAL UNION ALL SELECT 'OTHERS' AS FSTR,'OTHERS' as Days,'-' FROM DUAL";
                //    header_n = "Select Days";
                //    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "15192" || HCID == "15194" || HCID == "15195" || HCID == "15196" || HCID == "15197")
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
                    #region SFLG
                    case "15192":
                        if (hfval.Value == "")
                        {
                            header_n = "Select Report With Item Detail";
                            // hfval.Value = "VPFIN";
                            hfval.Value = value1;
                            SQuery = "SELECT 'YES' AS FSTR,'YES' AS CHOICE,'REPORT WITH ITEM DETAILS' AS MESSAGE FROM DUAL UNION ALL SELECT 'NO' AS FSTR,'NO' AS CHOICE,'REPORT WITHOUT ITEM DETAILS' AS MESSAGE FROM DUAL";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "15194":
                    case "15195":
                    case "15196":
                        if (hfHead.Value == "")
                        {
                            hfHead.Value = value1;
                            header_n = "Select Groups";
                            SQuery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else if (hfSales.Value == "")
                        {
                            header_n = "Select Sub Groups";
                            hfSales.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS SUB_GRP,INAME AS NAME FROM ITEM WHERE SUBSTR(ICODE,0,2) IN (" + hfSales.Value + ") AND LENGTH(TRIM(ICODE))=4 ORDER BY SUB_GRP";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else if (hfOpen.Value == "")
                        {
                            header_n = "Select Items";
                            hfOpen.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS CODE,INAME AS NAME,CPARTNO AS PARTNO FROM ITEM WHERE SUBSTR(ICODE,0,4) IN (" + hfOpen.Value + ") AND LENGTH(TRIM(ICODE))=8 ORDER BY CODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            hfParty.Value = value1;
                            if (val == "15194") fgen.Fn_open_prddmp1("-", frm_qstr);
                            else fgen.Fn_open_dtbox("-", frm_qstr);
                        }
                        break;

                    case "15197":
                        if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                        else branch_Cd = "branchcd='" + mbr + "'";
                        if (hfDeptt.Value == "")
                        {
                            header_n = "Select Days";
                            hfDeptt.Value = value1;
                            SQuery = "SELECT '30' AS FSTR,'0-30 Days' as Days,'-' FROM DUAL UNION ALL SELECT '60' AS FSTR,'31-60 Days' as Days,'-' FROM DUAL UNION ALL SELECT '90' AS FSTR,'61-90 Days' as Days,'-' FROM DUAL UNION ALL SELECT '180' AS FSTR,'91-180 Days' as Days,'-' FROM DUAL UNION ALL SELECT 'OTHERS' AS FSTR,'180 Days & Above' as Days,'-' FROM DUAL";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else if (hfOpen.Value == "")
                        {
                            header_n = "Select Parties";
                            hfOpen.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,A.ACODE AS CODE,F.ANAME AS NAME FROM RGPMST A ,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A." + branch_Cd + " AND A.TYPE='21' ORDER BY CODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else if (hfSales.Value == "")
                        {
                            header_n = "Select Groups";
                            hfSales.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else if (hfParty.Value == "")
                        {
                            header_n = "Select Sub Groups";
                            hfParty.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS SUB_GRP,INAME AS NAME FROM ITEM WHERE SUBSTR(ICODE,0,2) IN (" + hfParty.Value + ") AND LENGTH(TRIM(ICODE))=4 ORDER BY SUB_GRP";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else if (hfHead.Value == "")
                        {
                            header_n = "Select Items";
                            hfHead.Value = value1;
                            SQuery = "SELECT DISTINCT TRIM(I.ICODE) AS FSTR,I.ICODE AS CODE,I.INAME AS NAME,CPARTNO AS PARTNO FROM ITEM I WHERE SUBSTR(TRIM(I.ICODE),0,4) IN (" + hfHead.Value + ") AND LENGTH(TRIM(ICODE))=8 ORDER BY CODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                        }
                        else
                        {
                            cond = "";
                            switch (hfOpen.Value)
                            {
                                case "30":
                                    cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 0 and 30";
                                    er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 0 and 30";
                                    break;
                                case "60":
                                    cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 31 and 60";
                                    er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 31 and 60";
                                    break;
                                case "90":
                                    cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 61 and 90";
                                    er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 61 and 90";
                                    break;
                                case "180":
                                    cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 91 and 180";
                                    er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 91 and 180";
                                    break;
                                case "Others":
                                    cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') >=181";
                                    er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') >=181";
                                    break;
                                case " LIKE '%'":
                                    cond = " ";
                                    er1 = " ";
                                    hfOpen.Value = "All";
                                    break;
                            }
                            er2 = ""; cond1 = ""; er3 = "";
                            if (hfSales.Value.Contains("LIKE") && value1.Contains("LIKE"))
                            {
                                if (hfHead.Value != " LIKE '%'" && value1 == " LIKE '%'")
                                {
                                    er2 = "and substr(trim(a.icode),0,4) in (" + hfHead.Value + ") and a.acode " + hfSales.Value + "";
                                }
                                else
                                {
                                    er2 = "and a.icode " + value1 + " and a.acode " + hfSales.Value + "";
                                }
                            }
                            else if (hfSales.Value.Contains("LIKE"))
                            {
                                if (hfHead.Value != " LIKE '%'" && value1 == " LIKE '%'")
                                {
                                    er2 = "and substr(trim(a.icode),0,4) in (" + hfHead.Value + ") and a.acode " + hfSales.Value + "";
                                }
                                else
                                {
                                    er2 = "and a.icode in (" + value1 + ") and a.acode " + hfSales.Value + "";
                                }
                            }
                            else if (value1.Contains("LIKE"))
                            {
                                if (hfHead.Value != " LIKE '%'" && value1 == " LIKE '%'")
                                {
                                    er2 = "and substr(trim(a.icode),0,4) in (" + hfHead.Value + ") and a.acode in (" + hfSales.Value + ")";
                                }
                                else
                                {
                                    er2 = "and a.icode " + value1 + " and a.acode in (" + hfSales.Value + ")";
                                }
                            }
                            else
                            {
                                er2 = "and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ")";
                            }
                            if (hfDeptt.Value.Contains("LIKE"))
                            {
                                er3 = "and nvl(a.isize,'-') " + hfDeptt.Value + "";
                            }
                            else
                            {
                                er3 = "and nvl(a.isize,'-') in (" + hfDeptt.Value + ")";
                            }
                            // SQuery = "select a.vchnum as challan_no,a.vchdate as challan_date,a.icode as item_code,i.iname as item,i.cpartno as partno,a.acode as party_code,f.aname as party,sum(a.iqtyout)-sum(a.iqtyin) as bal, sum(a.iqtyout) as out_qty,sum(a.iqtyin) as in_qty,sum(a.iqtyout*a.out_irate) as out_amt,sum(a.iqtyin*a.in_irate) as in_amt from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND a.VCHDATE " + xprdrange + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and type='09' and rgpdate " + xprdrange + " AND STORE<>'R') a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) " + cond + " and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ") group by a.icode,a.acode,a.vchnum,a.vchdate,f.aname,i.iname,i.cpartno having sum(a.iqtyout)-sum(a.iqtyin)>0 order by a.vchnum";
                            // ORIGINal SQuery = "select a.vchnum as challan_no,a.vchdate as challan_date,a.icode as item_code,i.iname as item,trim(i.cpartno) as partno,a.acode as party_code,f.aname as party,sum(a.iqtyout)-sum(a.iqtyin) as bal,sum(a.iqtyout) as out_qty,sum(a.iqtyin) as in_qty,a.vdd from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,0 as iqtyin,to_char(a.vchdate,'yyyymmdd') as vdd FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,to_char(a.rgpdate,'yyyymmdd') as vdd from ivoucher a where A." + branch_Cd + " and type='09' " + er1 + " AND STORE<>'R' ) a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ") group by a.icode,a.acode,a.vchnum,a.vchdate,f.aname,i.iname,trim(i.cpartno),a.vdd having sum(a.iqtyout)-sum(a.iqtyin)>0 order by a.vdd,challan_no";
                           //query before bal_val SQuery = "select a.vchnum as challan_no,a.vchdate as challan_date,a.icode as item_code,i.iname as item,trim(i.cpartno) as partno,a.acode as party_code,f.aname as party,sum(a.iqtyout)-sum(a.iqtyin) as bal,sum(a.iqtyout) as out_qty,sum(a.iqtyin) as in_qty,a.vdd from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,0 as iqtyin,to_char(a.vchdate,'yyyymmdd') as vdd FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' " + cond + " and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ") union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,to_char(a.rgpdate,'yyyymmdd') as vdd from ivoucher a where A." + branch_Cd + " and type='09' " + er1 + " AND STORE<>'R' and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ")) a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) group by a.icode,a.acode,a.vchnum,a.vchdate,f.aname,i.iname,trim(i.cpartno),a.vdd having sum(a.iqtyout)-sum(a.iqtyin)>0 order by a.vdd,challan_no";
                            SQuery = "select a.vchnum as challan_no,a.vchdate as challan_date,a.icode as item_code,i.iname as item,trim(i.cpartno) as partno,a.acode as party_code,f.aname as party,sum(a.tot) as bal,0 as bal_val,sum(a.out_qty) as out_qty,(sum(out_amt)/sum(out_qty))*sum(tot) as out_amt,sum(a.in_qty) as in_qty,sum(a.in_amt) as in_amt,a.vdd from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt,vdd from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate,to_char(a.vchdate,'yyyymmdd') as vdd FROM RGPMST a WHERE a." + branch_Cd + " and a.type='21' " + cond + " " + er2 + " " + er3 + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate ,to_char(a.rgpdate,'yyyymmdd') as vdd from ivoucher a where A." + branch_Cd + " and A.type='09' " + er1 + " AND STORE<>'R' " + er2 + " ) group by icode,acode,vchnum,vchdate,vdd having sum(iqtyout)-sum(iqtyin)>0) a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) group by a.icode,a.acode,a.vchnum,a.vchdate,f.aname,i.iname,trim(i.cpartno),a.vdd having sum(a.out_qty)-sum(a.in_qty)>0 order by party_code,a.vdd,challan_no";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            dr1 = null;
                            dr1 = dt.NewRow();
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                db1 = 0; db2 = 0; db6 = 0; db7 = 0;
                                db1 = fgen.make_double(dt.Rows[i]["out_amt"].ToString());
                                db2 = fgen.make_double(dt.Rows[i]["in_amt"].ToString());
                                db6 = db1 + db2;
                                // db7 = db6 / fgen.make_double(dt.Rows[i]["bal"].ToString());
                                dt.Rows[i]["bal_val"] = Math.Round(db6, 2); ;
                            }

                            foreach (DataColumn dc in dt.Columns)
                            {
                                total = 0;
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 13)
                                {

                                }
                                else
                                {
                                    mq1 = "sum(" + dc.ColumnName + ")";
                                    total += fgen.make_double(dt.Compute(mq1, "").ToString());
                                    dr1[dc] = total;
                                }
                            }
                            if (dt.Rows.Count > 0)
                            {
                                dr1["Item"] = "Total";
                                dt.Columns.Remove("vdd");
                                dt.Columns.Remove("out_amt");
                                dt.Columns.Remove("in_amt");
                                dt.Rows.InsertAt(dr1, 0);
                            }
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            Session["send_dt"] = dt;
                            fgen.Fn_open_rptlevel("Jobwork Pending Summary Challan Wise for " + hfOpen.Value + " Days", frm_qstr);
                        }
                        break;
                    #endregion
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
        else
        {
            switch (val)
            {
                //AFTER PRESSING ESC IT WILL GO HERE
                #region SFLG
                case "15194":
                case "15195":
                case "15196":
                    if (hfHead.Value == "")
                    {
                        hfHead.Value = " LIKE '%'";
                        header_n = "Select Groups";
                        SQuery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else if (hfSales.Value == "")
                    {
                        header_n = "Select Sub Groups";
                        hfSales.Value = " LIKE '%'";
                        SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS SUB_GRP,INAME AS NAME FROM ITEM WHERE ICODE " + hfSales.Value + " AND LENGTH(TRIM(ICODE))=4 ORDER BY SUB_GRP";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else if (hfOpen.Value == "")
                    {
                        header_n = "Select Items";
                        hfOpen.Value = " LIKE '%'";
                        SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS CODE,INAME AS NAME,CPARTNO AS PARTNO FROM ITEM WHERE ICODE " + hfOpen.Value + " AND LENGTH(TRIM(ICODE))=8 ORDER BY CODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else
                    {
                        hfParty.Value = " LIKE '%'";
                        if (val == "15194") fgen.Fn_open_prddmp1("-", frm_qstr);
                        else fgen.Fn_open_dtbox("-", frm_qstr);
                    }
                    break;

                case "15197":
                    if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                    else branch_Cd = "branchcd='" + mbr + "'";
                    if (hfDeptt.Value == "")
                    {
                        header_n = "Select Days";
                        hfDeptt.Value = " LIKE '%'";
                      //  SQuery = "SELECT '30' AS FSTR,'30 Days' as Days,'-' FROM DUAL UNION ALL SELECT '60' AS FSTR,'60 Days' as Days,'-' FROM DUAL UNION ALL SELECT '90' AS FSTR,'90 Days' as Days,'-' FROM DUAL UNION ALL SELECT '180' AS FSTR,'180 Days' as Days,'-' FROM DUAL UNION ALL SELECT 'OTHERS' AS FSTR,'OTHERS' as Days,'-' FROM DUAL";
                        SQuery = "SELECT '30' AS FSTR,'0-30 Days' as Days,'-' FROM DUAL UNION ALL SELECT '60' AS FSTR,'31-60 Days' as Days,'-' FROM DUAL UNION ALL SELECT '90' AS FSTR,'61-90 Days' as Days,'-' FROM DUAL UNION ALL SELECT '180' AS FSTR,'91-180 Days' as Days,'-' FROM DUAL UNION ALL SELECT 'OTHERS' AS FSTR,'180 Days & Above' as Days,'-' FROM DUAL";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_sseek(header_n, frm_qstr);
                    }
                    else if (hfOpen.Value == "")
                    {
                        header_n = "Select Parties";
                        hfOpen.Value = " LIKE '%'";
                        SQuery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,A.ACODE AS CODE,F.ANAME AS NAME FROM RGPMST A ,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A." + branch_Cd + " AND A.TYPE='21' ORDER BY CODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else if (hfSales.Value == "")
                    {
                        header_n = "Select Groups";
                        hfSales.Value = " LIKE '%'";
                        SQuery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else if (hfParty.Value == "")
                    {
                        header_n = "Select Sub Groups";
                        hfParty.Value = " LIKE '%'";
                        SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS SUB_GRP,INAME AS NAME FROM ITEM WHERE SUBSTR(ICODE,0,2) " + hfParty.Value + " AND LENGTH(TRIM(ICODE))=4 ORDER BY SUB_GRP";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else if (hfHead.Value == "")
                    {
                        header_n = "Select Items";
                        hfHead.Value = " LIKE '%'";
                        SQuery = "SELECT DISTINCT TRIM(I.ICODE) AS FSTR,I.ICODE AS CODE,I.INAME AS NAME,CPARTNO AS PARTNO FROM ITEM I WHERE SUBSTR(TRIM(I.ICODE),0,4) " + hfHead.Value + " AND LENGTH(TRIM(ICODE))=8 ORDER BY CODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek(header_n, frm_qstr);
                    }
                    else
                    {
                        cond = "";
                        switch (hfOpen.Value)
                        {
                            case "30":
                                cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 0 and 30";
                                er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 0 and 30";
                                break;
                            case "60":
                                cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 31 and 60";
                                er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 31 and 60";
                                break;
                            case "90":
                                cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 61 and 90";
                                er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 61 and 90";
                                break;
                            case "180":
                                cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 91 and 180";
                                er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') between 91 and 180";
                                break;
                            case "Others":
                                cond = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.vchdate,'dd/mm/yyyy'),'dd/mm/yyyy') >=181";
                                er1 = " and to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(to_char(a.rgpdate,'dd/mm/yyyy'),'dd/mm/yyyy') >=181";
                                break;
                            case " LIKE '%'":
                                cond = " ";
                                er1 = " ";
                                hfOpen.Value = "All";
                                break;
                        }
                        // ORIGINAL SQuery = "select a.vchnum as challan_no,a.vchdate as challan_date,a.icode as item_code,i.iname as item,trim(i.cpartno) as partno,a.acode as party_code,f.aname as party,sum(a.iqtyout)-sum(a.iqtyin) as bal,sum(a.iqtyout) as out_qty,sum(a.iqtyin) as in_qty,a.vdd from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,0 as iqtyin,to_char(a.vchdate,'yyyymmdd') as vdd FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,to_char(a.rgpdate,'yyyymmdd') as vdd from ivoucher a where A." + branch_Cd + " and type='09' " + er1 + " AND STORE<>'R' ) a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ") group by a.icode,a.acode,a.vchnum,a.vchdate,f.aname,i.iname,trim(i.cpartno),a.vdd having sum(a.iqtyout)-sum(a.iqtyin)>0 order by a.vdd,challan_no";
                        er2 = ""; cond1 = ""; er3 = "";
                        value1 = " LIKE '%'";
                        er2 = ""; cond1 = ""; er3 = "";
                        if (hfSales.Value.Contains("LIKE") && value1.Contains("LIKE"))
                        {
                            if (hfHead.Value != " LIKE '%'" && value1 == " LIKE '%'")
                            {
                                er2 = "and substr(trim(a.icode),0,4) in (" + hfHead.Value + ") and a.acode " + hfSales.Value + "";
                            }
                            else
                            {
                                er2 = "and a.icode " + value1 + " and a.acode " + hfSales.Value + "";
                            }
                        }
                        else if (hfSales.Value.Contains("LIKE"))
                        {
                            if (hfHead.Value != " LIKE '%'" && value1 == " LIKE '%'")
                            {
                                er2 = "and substr(trim(a.icode),0,4) in (" + hfHead.Value + ") and a.acode " + hfSales.Value + "";
                            }
                            else
                            {
                                er2 = "and a.icode in (" + value1 + ") and a.acode " + hfSales.Value + "";
                            }
                        }
                        else if (value1.Contains("LIKE"))
                        {
                            if (hfHead.Value != " LIKE '%'" && value1 == " LIKE '%'")
                            {
                                er2 = "and substr(trim(a.icode),0,4) in (" + hfHead.Value + ") and a.acode in (" + hfSales.Value + ")";
                            }
                            else
                            {
                                er2 = "and a.icode " + value1 + " and a.acode in (" + hfSales.Value + ")";
                            }
                        }
                        else
                        {
                            er2 = "and a.icode in (" + value1 + ") and a.acode in (" + hfSales.Value + ")";
                        }
                        if (hfDeptt.Value.Contains("LIKE"))
                        {
                            er3 = "and nvl(a.isize,'-') " + hfDeptt.Value + "";
                        }
                        else
                        {
                            er3 = "and nvl(a.isize,'-') in (" + hfDeptt.Value + ")";
                        }
                        SQuery = "select a.vchnum as challan_no,a.vchdate as challan_date,a.icode as item_code,i.iname as item,trim(i.cpartno) as partno,a.acode as party_code,f.aname as party,sum(a.tot) as bal,0 as bal_val,sum(a.out_qty) as out_qty,(sum(out_amt)/sum(out_qty))*sum(tot) as out_amt,sum(a.in_qty) as in_qty,sum(a.in_amt) as in_amt,a.vdd from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt,vdd from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate,to_char(a.vchdate,'yyyymmdd') as vdd FROM RGPMST a WHERE a." + branch_Cd + " and a.type='21' " + cond + " " + er2 + " " + er3 + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate ,to_char(a.rgpdate,'yyyymmdd') as vdd from ivoucher a where A." + branch_Cd + " and A.type='09' " + er1 + " AND STORE<>'R' " + er2 + " ) group by icode,acode,vchnum,vchdate,vdd having sum(iqtyout)-sum(iqtyin)>0) a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) group by a.icode,a.acode,a.vchnum,a.vchdate,f.aname,i.iname,trim(i.cpartno),a.vdd having sum(a.out_qty)-sum(a.in_qty)>0 order by party_code,a.vdd,challan_no";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        dr1 = null;
                        dr1 = dt.NewRow();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            db1 = 0; db2 = 0; db6 = 0; db7 = 0;
                            db1 = fgen.make_double(dt.Rows[i]["out_amt"].ToString());
                            db2 = fgen.make_double(dt.Rows[i]["in_amt"].ToString());
                            db6 = db1 + db2;
                            // db7 = db6 / fgen.make_double(dt.Rows[i]["bal"].ToString());
                            dt.Rows[i]["bal_val"] = Math.Round(db6, 2); ;
                        }

                        foreach (DataColumn dc in dt.Columns)
                        {
                            total = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 13)
                            {

                            }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                total += fgen.make_double(dt.Compute(mq1, "").ToString());
                                dr1[dc] = total;
                            }
                        }
                        if (dt.Rows.Count > 0)
                        {
                            dr1["Item"] = "Total";
                            dt.Columns.Remove("vdd");
                            dt.Columns.Remove("out_amt");
                            dt.Columns.Remove("in_amt");
                            dt.Rows.InsertAt(dr1, 0);
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Jobwork Pending Summary Challan Wise for " + hfOpen.Value + " Days", frm_qstr);
                    }
                    break;
                #endregion
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

            //tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4)||'@'||trim(join_cond) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            //if (tbl_flds.Trim().Length > 1)
            //{
            //    datefld = tbl_flds.Split('@')[0].ToString();
            //    sortfld = tbl_flds.Split('@')[1].ToString();
            //    table1 = tbl_flds.Split('@')[2].ToString();
            //    table2 = tbl_flds.Split('@')[3].ToString();
            //    table3 = tbl_flds.Split('@')[4].ToString();
            //    table4 = tbl_flds.Split('@')[5].ToString();
            //    joincond = tbl_flds.Split('@')[6].ToString();
            //    joincond = joincond.Replace("`", "'");
            //    sortfld = sortfld.Replace("`", "'");
            //    rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
            //    rep_flds = rep_flds.Replace("`", "'");
            //}
            // after prdDmp this will run
            switch (val)
            {
                #region SFLG
                case "15192":
                     #region Prod Status
                    ded4 = ""; ded5 = ""; ded6 = ""; ded7 = ""; ded8 = ""; ded9 = "";
                    ded6 = todt;
                    ded4 = todt.Substring(3, 7);
                    ded9 = fromdt;
                    er3 = "SELECT TO_CHAR(ADD_MONTHS(TO_DATE('" + ded6 + "','DD/MM/YYYY'),1),'MONYYYY') AS NXTMNTH FROM DUAL";
                    er5 = fgen.seek_iname(frm_qstr, co_cd, er3, "NXTMNTH");
                    ded5 = "SELECT TRIM(TO_CHAR(TO_DATE('" + ded6.Substring(3, 2) + "', 'MM'), 'MON'))||TRIM(TO_CHAR(TO_DATE('" + ded6.Substring(6, 4) + "', 'YYYY'), 'YYYY'))  AS CURRMNTH FROM DUAL";
                    ded7 = fgen.seek_iname(frm_qstr, co_cd, ded5, "CURRMNTH");
                    dtm11 = new DataTable();
                    dtm11.Columns.Add("ITEMCODE", typeof(string));
                    dtm11.Columns.Add("ITEMNAME", typeof(string));
                    dtm11.Columns.Add("CPARTNO", typeof(string));
                    dtm11.Columns.Add("SALE_PLAN_" + ded7 + "", typeof(double));
                    dtm11.Columns.Add("SALE_PLAN_" + ded7 + "_VAL", typeof(double));
                    dtm11.Columns.Add("FGSTOCK", typeof(double));
                    dtm11.Columns.Add("FGSTOCK_VAL", typeof(double));
                    dtm11.Columns.Add("PROD_PLAN_" + ded7 + "", typeof(double));
                    dtm11.Columns.Add("PROD_PLAN_" + ded7 + "_VAL", typeof(double));
                    dtm11.Columns.Add("PROD_PLAN_" + er5 + "", typeof(double));
                    dtm11.Columns.Add("PROD_PLAN_" + er5 + "_VAL", typeof(double));
                    dtm11.Columns.Add("WIP", typeof(double));
                    dtm11.Columns.Add("WIP_VAL", typeof(double));
                    dtm11.Columns.Add("BLANKSTORE", typeof(double));
                    dtm11.Columns.Add("BLANKSTORE_VAL", typeof(double));
                    dtm11.Columns.Add("TOTAL", typeof(double));
                    dtm11.Columns.Add("TOTAL_VAL", typeof(double));
                    dtm11.Columns.Add("BLOCK_1_INVENTORY", typeof(double));
                    dtm11.Columns.Add("BLOCK_1_INVENTORY_VAL", typeof(double));
                    dtm11.Columns.Add("BLANKS_RECEIVED_IN_STORE", typeof(double));
                    dtm11.Columns.Add("BLANKS_RECEIVED_IN_STORE_VAL", typeof(double));
                    dtm11.Columns.Add("BLANKS_AVAILABLE_IN_STORE", typeof(double));
                    dtm11.Columns.Add("BLANKS_AVAILABLE_IN_STORE_VAL", typeof(double));
                    dtm11.Columns.Add("BLANK_ISSUED", typeof(double));
                    dtm11.Columns.Add("BLANK_ISSUED_VAL", typeof(double));
                    dtm11.Columns.Add("JWSTORE", typeof(double));
                    dtm11.Columns.Add("JWSTORE_VAL", typeof(double));
                    dtm11.Columns.Add("BLANK_SHORT", typeof(double));
                    dtm11.Columns.Add("BLANK_SHORT_VAL", typeof(double));
                    dtm11.Columns.Add("BLANK_EXP_ON", typeof(double));
                    dtm11.Columns.Add("BLANK_EXP_ON_VAL", typeof(double));
                    dtm11.Columns.Add("RUNNING_WIP", typeof(double));
                    dtm11.Columns.Add("RUNNING_WIP_VAL", typeof(double));
                    SQuery = "";
                    SQuery = "SELECT DISTINCT TYPE1,NAME FROM TYPE WHERE ID='1' AND SUBSTR(TYPE1,1,1)='6' AND TYPE1 NOT IN ('61','62')  ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    for (i1 = 0; i1 < dt.Rows.Count; i1++)
                    {
                        dtm11.Columns.Add("S".Trim() + dt.Rows[i1]["type1"].ToString(), typeof(double));
                        dtm11.Columns.Add("Y".Trim() + dt.Rows[i1]["type1"].ToString() + "_VAL", typeof(double));
                        dtm11.Columns.Add("X".Trim() + dt.Rows[i1]["type1"].ToString() + "TOTAL", typeof(double));
                        dtm11.Columns.Add("Z".Trim() + dt.Rows[i1]["type1"].ToString() + "_TOTALVAL", typeof(double));
                    }
                    dtm11.Columns.Add("FGS", typeof(double));
                    dtm11.Columns.Add("FGS_VAL", typeof(double));
                    dtm11.Columns.Add("FGS_TOTAL", typeof(double));
                    dtm11.Columns.Add("FGS_TOTALVAL", typeof(double));
                    dtm11.Columns.Add("D3", typeof(double));
                    dtm11.Columns.Add("D3_VAL", typeof(double));
                    dtm11.Columns.Add("D3_TOTAL", typeof(double));
                    dtm11.Columns.Add("D3_TOTALVAL", typeof(double));
                    dtm11.Columns.Add("BAL_FG_PLAN", typeof(double));
                    dtm11.Columns.Add("BAL_FG_PLAN_VAL", typeof(double));
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; ded1 = ""; ded2 = ""; ded3 = ""; ded8 = ""; ded10 = ""; ded11 = ""; ded12 = "";
                    cDT1 = fgen.seek_iname(frm_qstr,co_cd, "select to_char(fmdate,'dd/mm/yyyy') as fromdt from co where code='" + co_cd + year + "'", "fromdt");
                    cDT2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(todate,'dd/mm/yyyy') as todate from co where code='" + co_cd + year + "'", "todate");
                    fromdt = ded6;
                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')";
                    // WIP STOCK AS ON FIRST DAY OF MONTH
                    ded11 = "SELECT TO_CHAR(TO_DATE('" + ded6.Substring(3, 7) + "','MM/YYYY')-1,'DD/MM/YYYY') AS lastmnth FROM DUAL";
                    ded12 = fgen.seek_iname(frm_qstr, co_cd, ded11, "lastmnth");
                    string TODATECOPY = todt;
                    todt = ded12;
                    wip_stk_vw();
                    mq9 = "SELECT TOTAL,icode,STG01+STG02 as inv FROM wipcolstkw_" + mbr + " WHERE SUBSTR(ICODE,1,1) in ('1','7') order by icode";
                    dt9 = new DataTable();
                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9);

                    // BL STORE AND OSP/VD PLUS
                    todt = TODATECOPY;
                    wip_stk_vw();
                    mdt = new DataTable();
                    //mdt = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(icode) AS ICODE,STG01,STG02,STG01+STG02 as inv,TOTAL FROM wipcolstkw_" + mbr + " WHERE SUBSTR(ICODE,1,1) IN ('1','7') order by icode");
                    mdt = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(icode) AS ICODE,STG01,STG02,STG01+STG02+TOTAL as inv,TOTAL FROM wipcolstkw_" + mbr + " WHERE SUBSTR(ICODE,1,1) IN ('1','7') order by icode");

                    //// RUNNING WIP
                    //todt = TODATECOPY;
                    //wip_stk_vw();
                    //dt12 = new DataTable();
                    //dt12 = fgen.getdata(frm_qstr, co_cd, "SELECT icode,TOTAL,STG01,STG02 FROM wipcolstkw_" + mbr + " WHERE SUBSTR(ICODE,1,1) = '7' order by icode");

                    // BLANK GAYA IN STORE
                    //xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded6.Trim() + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN to_date('" + ded9.Trim() + "','dd/mm/yyyy') AND to_date('" + ded6.Trim() + "','dd/mm/yyyy')";
                    val = "SELECT TRIM(BRANCHCD) AS BRANCHCD,TRIM(ICODE) AS ICODE,SUM(IQTYOUT) AS QTYOUT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '2%' AND NVL(TRIM(REVIS_NO),'-')='01' AND VCHDATE " + xprdrange + " GROUP BY TRIM(BRANCHCD),TRIM(ICODE) ORDER BY ICODE";
                    dt13 = new DataTable();
                    dt13 = fgen.getdata(frm_qstr, co_cd, val);

                    // BLANK ISSUED IN STORE
                    //xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded9.Trim() + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN to_date('01/" + todt.Substring(3, 7) + "','dd/mm/yyyy') AND to_date('" + todt + "','dd/mm/yyyy')";
                    value1 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,TRIM(ICODE) AS ICODE,SUM(IQTYOUT) AS QTYOUT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='32' AND NVL(TRIM(REVIS_NO),'-')='01' AND VCHDATE " + xprdrange + " GROUP BY TRIM(BRANCHCD),TRIM(ICODE) ORDER BY ICODE";
                    dt14 = new DataTable();
                    dt14 = fgen.getdata(frm_qstr, co_cd, value1);

                    // JW STORE
                    //xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded9.Trim() + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN to_date('" + ded9.Trim() + "','dd/mm/yyyy') AND to_date('" + ded6.Trim() + "','dd/mm/yyyy')";
                    //value2 = "SELECT TRIM(A.ICODE) AS ICODE,A. branchcd, A.opening,A.qtyin,A. qtyout, A.cl FROM (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchnum like '%' and vchdate " + xprdrange1 + " and store='Y' AND NVL(REVIS_NO,'-')='02' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchnum like '%' and vchdate " + xprdrange + "  and store='Y' AND NVL(REVIS_NO,'-')='02' GROUP BY ICODE,branchcd ) group by branchcd,trim(icode)) A ORDER BY A.ICODE";
                    //value2 = "select trim(branchcd) as branchcd,trim(icode) as icode,sum(issue) as issue,sum(return) as return,sum(challan) as challan,sum(issue)+sum(return)-sum(challan) as cl from(select branchcd,icode,iqtyout as issue,0 as return,0 as challan from ivoucher where branchcd='" + mbr + "' and type='32' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(revis_no,'-')='02' union all select branchcd,icode,0 as issue,iqtyin as return,0 as challan from ivoucher where branchcd='" + mbr + "' and type='13' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(revis_no,'-')='02' union all select branchcd,icode,0 as issue,0 as return,iqtyout as challan from ivoucher where branchcd='" + mbr + "' and type like '2%' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(revis_no,'-')='02')group by trim(icode),trim(branchcd) order by icode";
                    value2 = "select trim(branchcd) as branchcd,trim(icode) as icode,sum(issue) as issue,sum(return) as return,sum(challan) as challan,sum(mrr) as mrr,(sum(return)+sum(mrr))-(sum(challan)+sum(issue)) as cl from (select branchcd,icode,0 as issue,iqtyin as return,0 as challan,0 as mrr from ivoucher where branchcd='" + mbr + "' and type='11' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(trim(revis_no),'-')='02' union all select branchcd,icode,0 as issue,0 as return,0 as challan,iqtyin as mrr from ivoucher where branchcd='" + mbr + "' and type='09' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(trim(revis_no),'-')='02' union all select branchcd,icode,0 as issue,0 as return,iqtyout as challan,0 as mrr from ivoucher where branchcd='" + mbr + "' and type like '2%' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(trim(revis_no),'-')='02' union all select branchcd,icode,iqtyout as issue,0 as return,0 as challan,0 as mrr from ivoucher where branchcd='" + mbr + "' and type='32' and vchdate " + xprdrange + " and store='Y' and substr(trim(icode),1,1)='7' and nvl(trim(revis_no),'-')='02')group by trim(icode),trim(branchcd) order by icode";
                    dt15 = new DataTable();
                    dt15 = fgen.getdata(frm_qstr, co_cd, value2);

                    // SALES PLAN
                    mq1 = "SELECT TRIM(BRANCHCD) AS BRANCHCD, sum(TARGET) as target,TRIM(ICODE) AS ICODE FROM MTHLYPLAN WHERE BRANCHCD='" + mbr + "' AND TYPE='10' AND TO_CHAR(VCHDATE,'MM/YYYY')= '" + ded4.Trim() + "' group by icode,BRANCHCD order by icode";
                    DataTable dtMTHLYPLAN = new DataTable();
                    dtMTHLYPLAN = fgen.getdata(frm_qstr, co_cd, mq1);

                    // PRODUCTION PLAN
                    mq2 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,SUM(A6) AS A6,TRIM(ICODE) AS ICODE,sum(A12) as A12 FROM PROD_SHEET WHERE BRANCHCD='" + mbr + "' AND TYPE='10'  AND TO_CHAR(VCHDATE,'MM/YYYY')= '" + ded4.Trim() + "' group by icode,BRANCHCD order by icode";
                    mq2 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,SUM(A6) AS A6,TRIM(ICODE) AS ICODE,sum(A12) as A12,TRIM(FILM_CODE) AS FILM_CODE FROM PROD_SHEET WHERE BRANCHCD='" + mbr + "' AND TYPE='10' AND TO_CHAR(VCHDATE,'MM/YYYY')= '" + ded4.Trim() + "' group by icode,BRANCHCD,FILM_CODE order by icode";
                    DataTable dtProd = new DataTable();
                    dtProd = fgen.getdata(frm_qstr, co_cd, mq2);

                    // BLANK STOCK AS ON FIRST OF MONTH
                    //xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded12.Trim() + "','dd/mm/yyyy')-1";
                    //xprdrange = "BETWEEN to_date('" + ded12.Trim() + "','dd/mm/yyyy') AND to_date('" + ded12.Trim() + "','dd/mm/yyyy')";

                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded9.Trim() + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN to_date('" + ded9.Trim() + "','dd/mm/yyyy') AND to_date('" + ded6.Trim() + "','dd/mm/yyyy')";

                    mq5 = "SELECT TRIM(A.ICODE) AS ICODE,A. branchcd, A.opening,A.qtyin,A. qtyout, A.cl FROM (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchnum like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchnum like '%' and vchdate " + xprdrange + "  and store='Y'  GROUP BY ICODE,branchcd ) group by branchcd,trim(icode)) A WHERE a.branchcd='" + mbr + "' and substr(a.icode,0,1)='7'  ORDER BY A.ICODE";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq5);

                    // BLANK STOCK RECEIVED
                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded9.Trim() + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN to_date('" + ded9.Trim() + "','dd/mm/yyyy') AND to_date('" + ded6.Trim() + "','dd/mm/yyyy')";
                   // ORIGINAL QUERY COMMENTED ON 19 MAY 2018 mq6 = "SELECT TRIM(A.ICODE) AS ICODE,A. branchcd, A.opening,A.qtyin,A. qtyout, A.cl FROM (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%' and vchdate " + xprdrange + " and store='Y'  GROUP BY ICODE,branchcd ) group by branchcd,trim(icode)) A WHERE a.branchcd='" + mbr + "' and substr(a.icode,0,1)='7' ORDER BY A.ICODE";
                    mq6 = "SELECT TRIM(A.ICODE) AS ICODE,A. branchcd, A.opening,A.qtyin,A. qtyout, A.cl FROM (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%' and vchdate " + xprdrange1 + " and store='Y' AND NVL(TRIM(REVIS_NO),'-')!='02' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%' and vchdate " + xprdrange + " and store='Y' AND NVL(TRIM(REVIS_NO),'-')!='02'  GROUP BY ICODE,branchcd ) group by branchcd,trim(icode)) A WHERE a.branchcd='" + mbr + "' and substr(a.icode,0,1)='7' ORDER BY A.ICODE";
                    dt6 = new DataTable();
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq6);

                    // STAGE WISE DATA
                   //ORIGINAL mq7 = "SELECT  TRIM(BRANCHCD) AS BRANCHCD,ACODE,(CASE WHEN ACODE='69' THEN SUM(IQTYIN) ELSE SUM(IQTYOUT) END) AS QTYIN,TRIM(ICODE) AS ICODE  FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='3A' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + ded6.Trim() + "' AND SUBSTR(ICODE,1,1)='7'  GROUP BY ACODE,ICODE ,BRANCHCD ORDER BY ICODE";
                    mq7 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,ACODE,(CASE WHEN ACODE='69' THEN SUM(IQTYIN) WHEN ACODE='6R' THEN SUM(IQTYIN) ELSE SUM(IQTYOUT) END) AS QTYIN,TRIM(ICODE) AS ICODE  FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='3A' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + ded6.Trim() + "' AND SUBSTR(ICODE,1,1)='7'  GROUP BY ACODE,ICODE ,BRANCHCD ORDER BY ICODE";
                    dt7 = new DataTable();
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7);

                    // STAGE WISE DATA TOTAL
                    // ORIGINAL mq8 = "SELECT DISTINCT TRIM(BRANCHCD) AS BRANCHCD,ACODE,(CASE WHEN ACODE='69' THEN SUM(IQTYIN) ELSE SUM(IQTYOUT) END) AS QTYIN,TRIM(ICODE) AS ICODE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='3A' AND VCHDATE BETWEEN TO_DATE('" + ded9 + "','DD/MM/YYYY') AND TO_DATE('" + ded6.Trim() + "','DD/MM/YYYY') AND SUBSTR(ICODE,1,1)='7' GROUP BY BRANCHCD,ACODE,ICODE ORDER BY ICODE"; ;
                    mq8 = "SELECT DISTINCT TRIM(BRANCHCD) AS BRANCHCD,ACODE,(CASE WHEN ACODE='69' THEN SUM(IQTYIN) WHEN ACODE='6R' THEN SUM(IQTYIN) ELSE SUM(IQTYOUT) END) AS QTYIN,TRIM(ICODE) AS ICODE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='3A' AND VCHDATE BETWEEN TO_DATE('" + ded9 + "','DD/MM/YYYY') AND TO_DATE('" + ded6.Trim() + "','DD/MM/YYYY') AND SUBSTR(ICODE,1,1)='7' GROUP BY BRANCHCD,ACODE,ICODE ORDER BY ICODE"; ;
                    dt8 = new DataTable();
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8);

                    // D3 
                    er1 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,SUM(IQTY_CHL) AS D3,TRIM(ICODE) AS ICODE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + ded6.Trim() + "' AND O_DEPTT='02' GROUP BY BRANCHCD,ICODE ORDER BY ICODE";
                    DataTable dtD3 = new DataTable();
                    dtD3 = fgen.getdata(frm_qstr, co_cd, er1);

                    // D3 TOTAL
                    er2 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,SUM(IQTY_CHL) AS D3,TRIM(ICODE) AS ICODE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND VCHDATE BETWEEN TO_DATE('" + ded9 + "','DD/MM/YYYY') AND TO_DATE('" + ded6.Trim() + "','DD/MM/YYYY')  AND O_DEPTT='02' GROUP BY BRANCHCD,ICODE ORDER BY ICODE";
                    DataTable dtD3TOT = new DataTable();
                    dtD3TOT = fgen.getdata(frm_qstr, co_cd, er2);

                    // FGS
                    mq10 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,SUM(IQTY_CHL) AS FGS,TRIM(ICODE) AS ICODE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + ded6.Trim() + "' AND O_DEPTT!='02' GROUP BY BRANCHCD,ICODE ORDER BY ICODE";
                    DataTable dtFGS = new DataTable();
                    dtFGS = fgen.getdata(frm_qstr, co_cd, mq10);

                    // FGS TOTAL
                    ded10 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,SUM(IQTY_CHL) AS FGS,TRIM(ICODE) AS ICODE FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND VCHDATE BETWEEN TO_DATE('" + ded9 + "','DD/MM/YYYY') AND TO_DATE('" + ded6.Trim() + "','DD/MM/YYYY')  AND O_DEPTT!='02' GROUP BY BRANCHCD,ICODE ORDER BY ICODE";
                    DataTable dtFGSTOT = new DataTable();
                    dtFGSTOT = fgen.getdata(frm_qstr, co_cd, ded10);

                    // FIND BOM OF PARTICULAR ITEM OF 7 SERIES
                    mq4 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,TRIM(IBCODE) AS IBCODE,TRIM(ICODE) AS ICODE  FROM ITEMOSP WHERE BRANCHCD='" + mbr + "' AND TYPE='BM' AND SUBSTR(TRIM(IBCODE),1,1)='7'";
                    mq4 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,TRIM(IBCODE) AS IBCODE,TRIM(ICODE) AS ICODE  FROM ITEMOSP WHERE BRANCHCD='" + mbr + "' AND TYPE='BM' AND SUBSTR(TRIM(IBCODE),1,1)='7' AND SRNO='0'";
                    dt5 = new DataTable();
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq4);

                    // BLOCK-1 INVENTORY
                    // FIND 1 SERIES ITEM FROM BOM
                 //  ORIGINAL COMMENTED ON 21 APR 2018 AS USER WANTS TO SEE ALL FR  ded1 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,TRIM(IBCODE) AS IBCODE,TRIM(ICODE) AS ICODE  FROM ITEMOSP WHERE BRANCHCD='" + mbr + "' AND TYPE='BM' AND SUBSTR(TRIM(IBCODE),1,1)='1' AND IBWT=0";
                    ded1 = "SELECT TRIM(BRANCHCD) AS BRANCHCD,TRIM(IBCODE) AS IBCODE,TRIM(ICODE) AS ICODE  FROM ITEMOSP WHERE BRANCHCD='" + mbr + "' AND TYPE='BM' AND SUBSTR(TRIM(IBCODE),1,1)='1' ORDER BY SRNO";
                    DataTable dtSeries1 = new DataTable();
                    dtSeries1 = fgen.getdata(frm_qstr, co_cd, ded1);

                    //ded8 = "SELECT  TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.ICODE) AS ICODE,(CASE WHEN A.ICHGS=0 THEN I.IRATE ELSE A.ICHGS END) AS IRATE,A.VCHDATE FROM IVOUCHER A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + mbr + "' AND SUBSTR(A.TYPE,0,1)='0' AND A.TYPE NOT IN ('0J','0S','0T','0W','09','0R')  AND SUBSTR(A.ICODE,0,1)='1'  ORDER BY A.VCHDATE DESC";
                    //DataTable dt11 = new DataTable();
                    //dt11 = fgen.getdata(frm_qstr, co_cd, ded8);

                    // SALE RATE FOR 9 SERIES ITEM
                    ded3 = "SELECT TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.ICODE) AS ICODE,A.IRATE AS IRATE2,(CASE WHEN TRIM(A.TYPE)='4F' THEN A.CURR_RATE*A.IRATE  ELSE A.IRATE END) AS IRATE FROM SOMAS A ,(SELECT TRIM(BRANCHCD) AS BRANCHCD,MAX(ORDDT) AS ORDDT, TRIM(ICODE) AS ICODE FROM SOMAS WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND SUBSTR(ICODE,0,1)='9' GROUP BY BRANCHCD,ICODE) B WHERE TRIM (A.ORDDT)=TRIM(B.ORDDT) AND TRIM(A.ICODE)=TRIM(B.ICODE) ORDER BY ICODE";
                    dt10 = new DataTable();
                    dt10 = fgen.getdata(frm_qstr, co_cd, ded3);

                    //SALE RATE FOR 1 SERIES ITEM	
                    ded8 = "SELECT  TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.ICODE) AS ICODE,(CASE WHEN A.ICHGS=0 THEN I.IRATE ELSE A.ICHGS END) AS IRATE,A.VCHDATE FROM IVOUCHER A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + mbr + "' AND SUBSTR(A.TYPE,0,1)='0' AND A.TYPE NOT IN ('0J','0S','0T','0W','09','0R')  AND SUBSTR(A.ICODE,0,1)='1'  ORDER BY A.VCHDATE DESC";
                    dt11 = new DataTable();
                    dt11 = fgen.getdata(frm_qstr, co_cd, ded8);

                    //xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded12 + "','dd/mm/yyyy')-1";
                    //xprdrange = "BETWEEN TO_DATE('" + ded12 + "','DD/MM/YYYY') AND TO_DATE('" + ded12 + "','DD/MM/YYYY')";

                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + ded9.Trim() + "','dd/mm/yyyy')-1";
                    xprdrange = "BETWEEN to_date('" + ded9.Trim() + "','dd/mm/yyyy') AND to_date('" + ded6.Trim() + "','dd/mm/yyyy')";

                   // mq0 = "SELECT TRIM(A.ICODE) AS ICODE,I.INAME,I.CPARTNO,A.branchcd, A.opening,A.qtyin,A.qtyout, A.cl FROM (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%'  and vchdate " + xprdrange + " and store='Y'  GROUP BY ICODE,branchcd ) group by branchcd,trim(icode)) A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) and a.branchcd='" + mbr + "' and trim(i.wip_code) in (" + hfcode.Value + ")  AND SUBSTR(i.ICODE,1,1)='9' AND TRIM(i.ICODE) NOT LIKE '97%' ORDER BY ICODE";
                    mq0 = "SELECT TRIM(A.ICODE) AS ICODE,I.INAME,I.CPARTNO,A.branchcd, A.opening,A.qtyin,A.qtyout, A.cl FROM (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchnum like '%'  and vchdate " + xprdrange + " and store='Y'  GROUP BY ICODE,branchcd ) group by branchcd,trim(icode)) A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) and a.branchcd='" + mbr + "' and trim(i.wip_code) in (" + hfval.Value + ")  AND SUBSTR(i.ICODE,1,1)='9' AND TRIM(i.ICODE) NOT LIKE '97%' ORDER BY ICODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr,co_cd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "BRANCHCD", "ICODE", "INAME", "CPARTNO", "opening");
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            drrow1 = dtm11.NewRow();
                            DataView viewim = new DataView(dt, "BRANCHCD='" + dr0["BRANCHCD"].ToString().Trim() + "' AND ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = viewim.ToTable();
                            double SALESPLAN = 0, MONTHLYPLAN = 0, NEXTMONTHLYPLAN = 0, BLANKOP = 0, BLANKRECEIVED = 0, BLANKAVAIL = 0, BLANKISSUED = 0, BLANKSHORT = 0, TOTAL = 0, WIP = 0, BLOCK1INV = 0, FGS = 0, FGSTOTAL = 0, BAL = 0, BL_OSPVD = 0, SERIES_RATE = 0, TOTAL_BLOCK1_INV = 0, FINAL_BLOCK1_INV = 0, FINAL_1SERIES_RATE = 0, D3 = 0, D3_TOT = 0, RUNNING_WIP = 0, JWSTORE = 0;
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                #region Loop
                                er4 = fgen.seek_iname_dt(dt5, "TRIM(ICODE)='" + dr0["icode"].ToString().Trim() + "'", "IBCODE");
                                ded2 = fgen.seek_iname_dt(dtSeries1, "TRIM(ICODE)='" + er4.Trim() + "'", "IBCODE");
                                if (dtMTHLYPLAN.Rows.Count > 0)
                                {
                                    SALESPLAN = fgen.make_double(fgen.seek_iname_dt(dtMTHLYPLAN, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "target"));
                                }
                                if (dtProd.Rows.Count > 0)
                                {
                                    MONTHLYPLAN = fgen.make_double(fgen.seek_iname_dt(dtProd, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and film_code='" + er4 + "'", "A6"));
                                    NEXTMONTHLYPLAN = fgen.make_double(fgen.seek_iname_dt(dtProd, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and film_code='" + er4 + "'", "A12"));
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    //  BLANKOP = fgen.make_double(fgen.seek_iname_dt(dt4, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "CL"));
                                    BLANKOP = fgen.make_double(fgen.seek_iname_dt(dt4, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "OPENING"));
                                }
                                if (dt9.Rows.Count > 0)
                                {
                                    WIP = fgen.make_double(fgen.seek_iname_dt(dt9, "icode='" + er4.Trim() + "'", "total"));
                                }
                                //if (dt12.Rows.Count > 0)
                                //{
                                //    RUNNING_WIP = fgen.make_double(fgen.seek_iname_dt(dt12, "icode='" + er4.Trim() + "'", "total"));
                                //}
                                if (mdt.Rows.Count > 0)
                                {
                                    RUNNING_WIP = fgen.make_double(fgen.seek_iname_dt(mdt, "icode='" + er4.Trim() + "'", "total"));
                                }
                                if (dtSeries1.Rows.Count > 0)
                                {
                                    DataView viewim9 = new DataView(dtSeries1, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "", DataViewRowState.CurrentRows);
                                    DataTable dt9stages = new DataTable();
                                    dt9stages = viewim9.ToTable();
                                    for (int k = 0; k < dt9stages.Rows.Count; k++)
                                    {
                                        BL_OSPVD = 0;
                                        //if (dt9.Rows.Count > 0)
                                        //{
                                        //    BLOCK1INV = fgen.make_double(fgen.seek_iname_dt(dt9, "icode='" + dt9stages.Rows[k]["ibcode"].ToString().Trim() + "'", "inv"));
                                        //}
                                        if (mdt.Rows.Count > 0)
                                        {
                                            //BL_OSPVD = fgen.make_double(fgen.seek_iname_dt(mdt, "icode='" + dt9stages.Rows[k]["ibcode"].ToString().Trim() + "'", "inv"));
                                            BL_OSPVD = fgen.make_double(fgen.seek_iname_dt(mdt, "icode='" + dt9stages.Rows[k]["ibcode"].ToString().Trim() + "'", "inv"));
                                            BL_OSPVD += fgen.make_double(fgen.seek_iname_dt(mdt, "icode='" + er4 + "'", "STG02"));
                                        }
                                        //TOTAL_BLOCK1_INV = BLOCK1INV + BL_OSPVD;
                                        TOTAL_BLOCK1_INV = BL_OSPVD;
                                        if (dt11.Rows.Count > 0)
                                        {
                                            SERIES_RATE = TOTAL_BLOCK1_INV * fgen.make_double(fgen.seek_iname_dt(dt11, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dt9stages.Rows[k]["ibcode"].ToString().Trim() + "'", "irate"));
                                        }
                                        FINAL_BLOCK1_INV = FINAL_BLOCK1_INV + TOTAL_BLOCK1_INV;
                                        FINAL_1SERIES_RATE = FINAL_1SERIES_RATE + SERIES_RATE;
                                    }
                                }
                                TOTAL = BLANKOP + WIP;
                                if (dt6.Rows.Count > 0)
                                {
                                    BLANKRECEIVED = fgen.make_double(fgen.seek_iname_dt(dt6, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "qtyin"));
                                   // ON 21 MAY 2018 BLANKISSUED = fgen.make_double(fgen.seek_iname_dt(dt6, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "qtyout"));
                                    //BLANKAVAIL = (BLANKOP + BLANKRECEIVED) - BLANKISSUED;
                                    // ON 21 MAY 2018 BLANKAVAIL = BLANKRECEIVED - BLANKISSUED;
                                    // ON 21 MAY 2018 BLANKSHORT = MONTHLYPLAN - (WIP + BLANKISSUED + BLANKAVAIL);
                                }
                                if (dt14.Rows.Count > 0)
                                {
                                    BLANKISSUED = fgen.make_double(fgen.seek_iname_dt(dt14, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "qtyout"));
                                }
                                if (dt13.Rows.Count > 0)
                                {
                                    BLOCK1INV = fgen.make_double(fgen.seek_iname_dt(dt13, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "qtyout"));
                                }
                                BLANKAVAIL = (BLANKOP + BLANKRECEIVED) - (BLOCK1INV + BLANKISSUED);
                                BLANKSHORT = MONTHLYPLAN - (WIP + BLANKISSUED + BLANKAVAIL);
                                //if (dt12.Rows.Count > 0)
                                //{
                                //    BLANKAVAIL = fgen.make_double(fgen.seek_iname_dt(dt12, "icode='" + er4.Trim() + "'", "STG01"));
                                //}

                                //if (dt13.Rows.Count > 0)
                                //{
                                //    BLANKAVAIL = fgen.make_double(fgen.seek_iname_dt(dt13, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "cl"));
                                //}

                                if (dt15.Rows.Count > 0)
                                {
                                    JWSTORE = fgen.make_double(fgen.seek_iname_dt(dt15, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "cl"));
                                }

                                if (dt7.Rows.Count > 0)
                                {
                                    DataView viewim7 = new DataView(dt7, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "", DataViewRowState.CurrentRows);
                                    DataTable dtstages = new DataTable();
                                    dtstages = viewim7.ToTable();

                                    for (int f = 0; f < dtstages.Rows.Count; f++)
                                    {
                                        double tot = 0;
                                        string CODE = dtstages.Rows[f]["ACODE"].ToString().Trim();
                                        try
                                        {
                                            drrow1["S" + CODE] = fgen.make_double(dtstages.Rows[f]["QTYIN"].ToString());
                                            drrow1["Y" + CODE + "_VAL"] = fgen.make_double(dtstages.Rows[f]["QTYIN"].ToString()) * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                            tot = tot + fgen.make_double(dtstages.Rows[f]["QTYIN"].ToString());
                                        }
                                        catch { }
                                    }
                                }
                                if (dt8.Rows.Count > 0)
                                {
                                    DataView viewim8 = new DataView(dt8, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + er4.Trim() + "'", "", DataViewRowState.CurrentRows);
                                    DataTable dtstagestotal = new DataTable();
                                    dtstagestotal = viewim8.ToTable();

                                    for (int g = 0; g < dtstagestotal.Rows.Count; g++)
                                    {
                                        string CODE = dtstagestotal.Rows[g]["ACODE"].ToString().Trim();
                                        try
                                        {
                                            drrow1["X" + CODE + "TOTAL"] = fgen.make_double(dtstagestotal.Rows[g]["QTYIN"].ToString());
                                            drrow1["Z" + CODE + "_TOTALVAL"] = fgen.make_double(dtstagestotal.Rows[g]["QTYIN"].ToString()) * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                        }
                                        catch { }
                                    }
                                }
                                if (dtFGS.Rows.Count > 0)
                                {
                                    FGS = fgen.make_double(fgen.seek_iname_dt(dtFGS, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "FGS"));
                                }
                                if (dtFGSTOT.Rows.Count > 0)
                                {
                                    FGSTOTAL = fgen.make_double(fgen.seek_iname_dt(dtFGSTOT, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "FGS"));
                                }
                                if (dtD3.Rows.Count > 0)
                                {
                                    D3 = fgen.make_double(fgen.seek_iname_dt(dtD3, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "D3"));
                                }
                                if (dtD3TOT.Rows.Count > 0)
                                {
                                    D3_TOT = fgen.make_double(fgen.seek_iname_dt(dtD3TOT, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "D3"));
                                }
                                BAL = MONTHLYPLAN - FGSTOTAL;
                                drrow1["ITEMCODE"] = dr0["ICODE"].ToString();
                                drrow1["ITEMNAME"] = dr0["INAME"].ToString();
                                drrow1["CPARTNO"] = dr0["CPARTNO"].ToString();
                                drrow1["SALE_PLAN_" + ded7 + ""] = SALESPLAN;
                                drrow1["SALE_PLAN_" + ded7 + "_VAL"] = SALESPLAN * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                //drrow1["FGSTOCK"] = dr0["CL"].ToString();
                                drrow1["FGSTOCK"] = dr0["opening"].ToString();
                                //  drrow1["FGSTOCK_VAL"] = fgen.make_double(dr0["CL"].ToString()) * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["FGSTOCK_VAL"] = fgen.make_double(dr0["opening"].ToString()) * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["PROD_PLAN_" + ded7 + ""] = MONTHLYPLAN;
                                drrow1["PROD_PLAN_" + ded7 + "_VAL"] = MONTHLYPLAN * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["PROD_PLAN_" + er5 + ""] = NEXTMONTHLYPLAN;
                                drrow1["PROD_PLAN_" + er5 + "_VAL"] = NEXTMONTHLYPLAN * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["BLANKSTORE"] = BLANKOP;
                                drrow1["BLANKSTORE_VAL"] = BLANKOP * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["TOTAL"] = TOTAL;
                                drrow1["TOTAL_VAL"] = TOTAL * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["WIP"] = WIP;
                                drrow1["WIP_VAL"] = WIP * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["BLOCK_1_INVENTORY"] = FINAL_BLOCK1_INV;
                                drrow1["BLOCK_1_INVENTORY_VAL"] = FINAL_1SERIES_RATE;

                                drrow1["RUNNING_WIP"] = RUNNING_WIP;
                                drrow1["RUNNING_WIP_VAL"] = RUNNING_WIP * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                
                                drrow1["BLANKS_RECEIVED_IN_STORE"] = BLANKRECEIVED;
                                drrow1["BLANKS_RECEIVED_IN_STORE_VAL"] = BLANKRECEIVED * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["BLANKS_AVAILABLE_IN_STORE"] = BLANKAVAIL;
                                drrow1["BLANKS_AVAILABLE_IN_STORE_VAL"] = BLANKAVAIL * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["BLANK_ISSUED"] = BLANKISSUED;
                                drrow1["BLANK_ISSUED_VAL"] = BLANKISSUED * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["BLANK_SHORT"] = BLANKSHORT;
                                drrow1["BLANK_SHORT_VAL"] = BLANKSHORT * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["BLANK_EXP_ON"] = 0;
                                drrow1["BLANK_EXP_ON_VAL"] = 0 * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));

                                drrow1["JWSTORE"] = JWSTORE;
                                drrow1["JWSTORE_VAL"] = JWSTORE * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                
                                drrow1["FGS"] = FGS;
                                drrow1["FGS_VAL"] = FGS * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["FGS_TOTAL"] = FGSTOTAL;
                                drrow1["FGS_TOTALVAL"] = FGSTOTAL * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));

                                drrow1["D3"] = D3;
                                drrow1["D3_VAL"] = D3 * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                drrow1["D3_TOTAL"] = D3_TOT;
                                drrow1["D3_TOTALVAL"] = D3_TOT * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));

                                drrow1["BAL_FG_PLAN"] = BAL;
                                drrow1["BAL_FG_PLAN_VAL"] = BAL * fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dticode.Rows[i]["branchcd"].ToString().Trim() + "' AND icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                                dtm11.Rows.Add(drrow1);
                                #endregion
                            }
                        }
                    }
                    dr31 = dtm11.NewRow();
                    dro = dtm11.NewRow();
                    foreach (DataColumn dc in dtm11.Columns)
                    {
                        double total = 0;
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2) { }
                        else
                        {
                            string var1 = "sum(" + dc.ColumnName + ")";
                            total += Math.Round(fgen.make_double(dtm11.Compute(var1, "").ToString()), 2);
                            dr31[dc] = total;
                            if (dc.ColumnName.Contains("_VAL"))
                            {
                                dro[dc.Ordinal - 1] = total;
                            }
                            if (dc.ColumnName.Contains("_TOTALVAL"))
                            {
                                dro[dc.Ordinal - 1] = total;
                            }
                        }
                    }
                    #region Columns Remove
                    if (dtm11.Rows.Count > 0)
                    {
                        if (hf1.Value == "YES")
                        {
                            dr31["ITEMCODE"] = "G. TOTAL";
                            dro["ITEMCODE"] = "G. VALUE IN LACS";
                        }
                        else
                        {
                            dtm11.Columns.Remove("ITEMCODE");
                            dtm11.Columns.Remove("ITEMNAME");
                            dr31["CPARTNO"] = "G. TOTAL";
                            dro["CPARTNO"] = "G. VALUE IN LACS";
                        }
                        dtm11.Rows.InsertAt(dr31, 0);
                        dtm11.Rows.InsertAt(dro, 0);
                        dtm11.Columns.Remove("SALE_PLAN_" + ded7 + "_VAL");
                        dtm11.Columns.Remove("FGSTOCK_VAL");
                        dtm11.Columns.Remove("PROD_PLAN_" + ded7 + "_VAL");
                        dtm11.Columns.Remove("PROD_PLAN_" + er5 + "_VAL");
                        dtm11.Columns.Remove("WIP_VAL");
                        dtm11.Columns.Remove("BLANKSTORE_VAL");
                        dtm11.Columns.Remove("TOTAL_VAL");
                        dtm11.Columns.Remove("BLOCK_1_INVENTORY_VAL");
                        dtm11.Columns.Remove("BLANKS_RECEIVED_IN_STORE_VAL");
                        dtm11.Columns.Remove("BLANKS_AVAILABLE_IN_STORE_VAL");
                        dtm11.Columns.Remove("BLANK_ISSUED_VAL");
                        dtm11.Columns.Remove("BLANK_SHORT_VAL");
                        dtm11.Columns.Remove("BLANK_EXP_ON_VAL");
                        dtm11.Columns.Remove("FGS_VAL");
                        dtm11.Columns.Remove("FGS_TOTALVAL");
                        dtm11.Columns.Remove("D3_VAL");
                        dtm11.Columns.Remove("D3_TOTALVAL");
                        dtm11.Columns.Remove("BAL_FG_PLAN_VAL");
                        dtm11.Columns.Remove("RUNNING_WIP_VAL");
                        dtm11.Columns.Remove("JWSTORE_VAL");
                        try
                        {
                            dtm11.Columns.Remove("Y63_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y64_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y65_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y66_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y67_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y68_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y69_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6A_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6B_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6C_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6D_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6E_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6F_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Y6R_VAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z63_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z64_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z65_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z66_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z67_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z68_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z69_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6A_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6B_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6C_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6D_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6E_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6F_TOTALVAL");
                        }
                        catch { }
                        try
                        {
                            dtm11.Columns.Remove("Z6R_TOTALVAL");
                        }
                        catch { }
                    }
#endregion
                    mq0 = "SELECT DISTINCT TYPE1,NAME FROM TYPE WHERE ID='1' AND SUBSTR(TYPE1,1,1)='6' AND TYPE1 NOT IN ('61','62')  ORDER BY TYPE1";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    foreach (DataColumn dc in dtm11.Columns)
                    {
                        int abc = dc.Ordinal;
                        string rejtype = dc.ToString().Substring(0, 1);
                        if (rejtype == "S")
                        {
                            string name = dc.ToString().Remove(0, 1);
                            string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                            if (myname != "0")
                            {
                                dtm11.Columns[abc].ColumnName = myname;
                            }
                        }
                        if (rejtype == "X")
                        {
                            string name = dc.ToString().Remove(0, 1);
                            int lenght = name.Length;
                            string secname = name.Substring(0, 2);
                            string myname = fgen.seek_iname_dt(dt, "type1='" + secname + "'", "name");
                            if (myname != "0")
                            {
                                dtm11.Columns[abc].ColumnName = myname + "_TOTAL";
                            }
                        }
                    }
                    Session["send_dt"] = dtm11;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Production Schedule Status Report", frm_qstr);
                    #endregion                    
                    break;

                case "15193":
                    #region Daily Prod
                   // BOM();
                    mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; ded1 = ""; ded2 = ""; ded3 = ""; ded8 = ""; ded9 = ""; ded10 = ""; ded11 = ""; ded12 = ""; ded4 = ""; ded5 = ""; ded6 = ""; ded7 = ""; ded8 = "";
                    ded6 = todt;
                    ded4 = todt.Substring(3, 7);
                    dtm11 = new DataTable();
                    dtm11.Columns.Add("WIPCODE", typeof(string));
                    dtm11.Columns.Add("ICODE", typeof(string));
                    dtm11.Columns.Add("MAKER", typeof(string));
                    dtm11.Columns.Add("PRODPLAN", typeof(string));
                    dtm11.Columns.Add("PROD_PLAN_VAL", typeof(string));
                    dtm11.Columns.Add("TOTAL_QTY", typeof(string));
                    dtm11.Columns.Add("TODAY_VAL", typeof(string));
                    dtm11.Columns.Add("WEEKTILLDATEACHVDQTY", typeof(string));
                    dtm11.Columns.Add("WEEKTILLDATEACHVDVALUE", typeof(string));
                    dtm11.Columns.Add("MONTHTILLDATEQTY", typeof(string));
                    dtm11.Columns.Add("TILLDATEVAL", typeof(string));
                    dtm11.Columns.Add("VALUEADDITION", typeof(string));
                    dtm11.Columns.Add("MTDPRODVALUE", typeof(string));
                    dtm11.Columns.Add("TODAYPRODVALUE", typeof(string));

                    DateTime DATEWEEK = Convert.ToDateTime(todt);
                    int diff333 = DATEWEEK.DayOfWeek - DayOfWeek.Monday;
                    if (diff333 < 0)
                    {
                        diff333 += 7;
                    }
                    DateTime WEEK = DATEWEEK.AddDays(-1 * diff333).Date;
                    string WeekDate = WEEK.ToString("dd/MM/yyyy");
                    // PRODUCTION PLAN
                    mq2 = "SELECT sum(A.A6) as A6,TRIM(A.icode) AS ICODE FROM PROD_SHEET A WHERE  A.BRANCHCD='" + mbr + "' AND A.TYPE='10'  AND TO_CHAR(A.VCHDATE,'MM/YYYY')= '" + ded4.Trim() + "' group by A.icode order by A.icode";
                    dtProd = new DataTable();
                    dtProd = fgen.getdata(frm_qstr, co_cd, mq2);

                    // PRDOUCTION VAL
                    //mq3 = "SELECT ICODE,SUM(PRODVAL) AS PRODVAL FROM (SELECT A.ICODE,SUM(A.A6)*S.IRATE AS PRODVAL FROM PROD_SHEET A, SOMAS S WHERE TRIM(S.ICODE)=TRIM(A.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='10' AND S.TYPE LIKE '4%'  AND  TO_CHAR(A.VCHDATE,'MM/YYYY')= '" + ded4.Trim() + "' AND S.ICODE LIKE '9%'  GROUP BY A.ICODE,S.IRATE) GROUP BY ICODE ORDER BY ICODE";
                    mq3 = "SELECT DISTINCT TRIM(IRATE) AS BASIC_IRATE,TRIM(ICODE) AS ICODE,S.ORDDT,(CASE WHEN  TRIM(TYPE)='4F' THEN CURR_RATE*IRATE  ELSE IRATE  END) AS IRATE FROM SOMAS S WHERE S.BRANCHCD='" + mbr + "' AND  S.TYPE LIKE '4%' AND S.ORDDT>(SYSDATE-500)  ORDER BY S.ORDDT DESC";
                    DataTable dtProdVal = new DataTable();
                    dtProdVal = fgen.getdata(frm_qstr, co_cd, mq3);

                    // TODAY QTY
                    mq4 = "SELECT ICODE,SUM(IQTY_CHL) AS IQTYIN  FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND  TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + todt + "'  GROUP BY ICODE ORDER BY ICODE";
                    DataTable dtToday = new DataTable();
                    dtToday = fgen.getdata(frm_qstr, co_cd, mq4);

                    // WEEK QTY
                    mq6 = "SELECT TRIM(ICODE) AS ICODE,SUM(IQTY_CHL) AS IQTYIN  FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND  VCHDATE = TO_DATE('" + WeekDate + "','DD/MM/YYYY') GROUP BY ICODE ORDER BY ICODE";
                    DataTable dtWeek = new DataTable();
                    dtWeek = fgen.getdata(frm_qstr, co_cd, mq6);

                    // MONTH QTY
                    mq8 = "SELECT TRIM(ICODE) AS ICODE,SUM(IQTY_CHL) AS IQTYIN  FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16'  AND  TO_CHAR(VCHDATE,'MM/YYYY')='" + ded4 + "'  GROUP BY ICODE ORDER BY ICODE";
                    DataTable dtMonth = new DataTable();
                    dtMonth = fgen.getdata(frm_qstr, co_cd, mq8);

                    // PROD VALE ADDITION
                    // FIND BOM OF PARTICULAR ITEM OF 7 SERIES
                    mq1 = "SELECT TRIM(IBCODE) AS IBCODE,TRIM(ICODE) AS ICODE  FROM ITEMOSP WHERE BRANCHCD='" + mbr + "' AND TYPE='BM' AND  ICODE LIKE '9%' ORDER BY ICODE";
                    DataTable dtSeries7 = new DataTable();
                    dtSeries7 = fgen.getdata(frm_qstr, co_cd, mq1);

                    // PICK 1 SERIES ITEM
                    mq10 = "SELECT DISTINCT TRIM(IBCODE) AS IBCODE,TRIM(ICODE) AS ICODE,TRIM(IBQTY) AS QTY FROM  ITEMOSP WHERE BRANCHCD='" + mbr + "' AND TYPE='BM'  AND ICODE LIKE '7%' ORDER BY ICODE";
                    dtSeries1 = new DataTable();
                    dtSeries1 = fgen.getdata(frm_qstr, co_cd, mq10);
                    //ORIGINAL COMMENTED ON 23 APR 2018 ded3 = "SELECT DISTINCT TRIM(IRATE) AS IRATE,TRIM(ICODE) AS ICODE FROM ITEM WHERE TRIM(ICODE) LIKE '1%' and length(trim(icode))>4";
                    ded3 = "SELECT DISTINCT (CASE WHEN NVL(IQD,'0')!='0' THEN IQD ELSE IRATE END) AS IRATE,TRIM(ICODE) AS ICODE FROM ITEM WHERE TRIM(ICODE) LIKE '1%' and length(trim(icode))>4";
                    DataTable dt1SeriesCost = new DataTable();
                    dt1SeriesCost = fgen.getdata(frm_qstr, co_cd, ded3);
                    //************************************//
                    mq0 = "SELECT DISTINCT TYPE1,NAME AS WIP_CODE,TRIM(I.ICODE) AS ICODE,TRIM(I.INAME) AS INAME, TRIM(I.MAKER) AS MAKER  FROM TYPE A ,ITEM I  WHERE A.ID='1' AND SUBSTR(A.TYPE1,1,1)='6'  AND TRIM(A.NAME)=TRIM(I.WIP_CODE)  AND  SUBSTR(I.ICODE,1,1) = ('9') AND LENGTH(tRIM(I.ICODE))>=8 AND TRIM(i.ICODE) NOT LIKE '97%' ORDER BY ICODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "ICODE", "iname", "WIP_CODE", "MAKER");
                        drrow1 = null;
                        double MnthlyProd = 0, MNTHLYPRODVAL = 0, todayqty = 0, todayval = 0, weekqty = 0, weekval = 0, monthqty = 0, monthval = 0, series9cost = 0, series1cost = 0, diff = 0, prodvaladd = 0, MTDPRODVAL = 0, MTDDIFF = 0, TODAYPRODVAL = 0, TODAYPRODDIFF = 0, SERIESMTD9 = 0, SERIESMTD1 = 0, SERIESTODAY9 = 0, SERIESTODAY1 = 0;
                        double MNTHIRATE = 0, SERIES1RATE = 0;
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            drrow1 = dtm11.NewRow();
                            DataView viewim = new DataView(dt, "ICODE='" + dr0["ICODE"] + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = viewim.ToTable();
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                if (dtProd.Rows.Count > 0)
                                {
                                    MnthlyProd = fgen.make_double(fgen.seek_iname_dt(dtProd, "icode='" + dr0["icode"].ToString().Trim() + "'", "a6"));
                                }
                                if (dtToday.Rows.Count > 0)
                                {
                                    todayqty = fgen.make_double(fgen.seek_iname_dt(dtToday, "icode='" + dr0["icode"].ToString().Trim() + "'", "IQTYIN"));
                                }
                                if (dtWeek.Rows.Count > 0)
                                {
                                    weekqty = fgen.make_double(fgen.seek_iname_dt(dtWeek, "icode='" + dr0["icode"].ToString().Trim() + "'", "IQTYIN"));
                                }
                                if (dtMonth.Rows.Count > 0)
                                {
                                    monthqty = fgen.make_double(fgen.seek_iname_dt(dtMonth, "icode='" + dr0["icode"].ToString().Trim() + "'", "IQTYIN"));
                                }
                                ded7 = fgen.seek_iname_dt(dtSeries7, "icode='" + dr0["icode"].ToString().Trim() + "'", "IBCODE");
                                ded1 = fgen.seek_iname_dt(dtSeries1, "icode='" + ded7.Trim() + "'", "IBCODE");
                                if (dtProdVal.Rows.Count > 0)
                                {
                                    MNTHIRATE = fgen.make_double(fgen.seek_iname_dt(dtProdVal, "icode='" + dr0["icode"].ToString().Trim() + "'", "IRATE"));
                                    MNTHLYPRODVAL = MnthlyProd * MNTHIRATE;
                                    todayval = todayqty * MNTHIRATE;
                                    weekval = weekqty * MNTHIRATE;
                                    monthval = monthqty * MNTHIRATE;
                                    series9cost = fgen.make_double(fgen.seek_iname_dt(dtProdVal, "icode='" + dr0["icode"].ToString().Trim() + "'", "IRATE"));
                                }

                                series1cost = fgen.make_double(fgen.seek_iname_dt(dt1SeriesCost, "icode='" + ded1.Trim() + "'", "IRATE"));

                                diff = series9cost - series1cost;
                                prodvaladd = diff * MnthlyProd;
                                MTDDIFF = series9cost - series1cost;
                                MTDPRODVAL = MTDDIFF * monthqty;
                                TODAYPRODDIFF = series9cost - series1cost;
                                TODAYPRODVAL = TODAYPRODDIFF * todayqty;
                                drrow1["WIPCODE"] = dr0["WIP_CODE"].ToString();
                                drrow1["ICODE"] = dr0["ICODE"].ToString();
                                drrow1["MAKER"] = dr0["MAKER"].ToString();
                                drrow1["PRODPLAN"] = Math.Round(MnthlyProd, 2);
                                drrow1["PROD_PLAN_VAL"] = Math.Round(MNTHLYPRODVAL, 2);
                                drrow1["TOTAL_QTY"] = todayqty;
                                drrow1["TODAY_VAL"] = todayval;
                                drrow1["WEEKTILLDATEACHVDQTY"] = weekqty;
                                drrow1["WEEKTILLDATEACHVDVALUE"] = weekval;
                                drrow1["MONTHTILLDATEQTY"] = monthqty;
                                drrow1["TILLDATEVAL"] = Math.Round(monthval, 2);
                                drrow1["VALUEADDITION"] = Math.Round(prodvaladd, 2);
                                drrow1["MTDPRODVALUE"] = Math.Round(MTDPRODVAL, 2);
                                drrow1["TODAYPRODVALUE"] = Math.Round(TODAYPRODVAL, 2);
                                dtm11.Rows.Add(drrow1);
                            }
                        }
                    }

                    // FOR MAKING FINAL DT SO THAT ALL ITEMS OF SAME CATEGORY COMES UNDER ONE FRAME
                    DataTable dtm11Final = new DataTable();
                    dtm11Final.Columns.Add("WIPCODE", typeof(string));
                    dtm11Final.Columns.Add("header1", typeof(string));
                    dtm11Final.Columns.Add("header2", typeof(string));
                    dtm11Final.Columns.Add("wd", typeof(string));
                    dtm11Final.Columns.Add("daysleft", typeof(string));
                    dtm11Final.Columns.Add("MAKER", typeof(string));
                    dtm11Final.Columns.Add("PRODPLAN", typeof(string));
                    dtm11Final.Columns.Add("PROD_PLAN_VAL", typeof(string));
                    dtm11Final.Columns.Add("TOTAL_QTY", typeof(string));
                    dtm11Final.Columns.Add("TODAY_VAL", typeof(string));
                    dtm11Final.Columns.Add("WEEKTILLDATEACHVDQTY", typeof(string));
                    dtm11Final.Columns.Add("WEEKTILLDATEACHVDVALUE", typeof(string));
                    dtm11Final.Columns.Add("MONTHTILLDATEQTY", typeof(string));
                    dtm11Final.Columns.Add("TILLDATEVAL", typeof(string));
                    dtm11Final.Columns.Add("VALUEADDITION", typeof(string));
                    dtm11Final.Columns.Add("MTDPRODVALUE", typeof(string));
                    dtm11Final.Columns.Add("TODAYPRODVALUE", typeof(string));

                    DataView view1imFinal = new DataView(dtm11);
                    DataTable dtdrsimFinal = new DataTable();
                    dtdrsimFinal = view1imFinal.ToTable(true, "MAKER", "WIPCODE");
                    DataRow drrow1Final = null;
                    foreach (DataRow dr0 in dtdrsimFinal.Rows)
                    {
                        drrow1Final = dtm11Final.NewRow();
                        DataView viewim = new DataView(dtm11, "MAKER='" + dr0["MAKER"] + "' AND WIPCODE='" + dr0["WIPCODE"] + "'", "", DataViewRowState.CurrentRows);
                        dticode = new DataTable();
                        dticode = viewim.ToTable();
                        double MnthlyProdFinal = 0, MNTHLYPRODVALFinal = 0, todayqtyFinal = 0, todayvalFinal = 0, weekqtyFinal = 0, weekvalFinal = 0, monthqtyFinal = 0, monthvalFinal = 0, series9costFinal = 0, series1costFinal = 0, diffFinal = 0, prodvaladdFinal = 0, MTDPRODVALFinal = 0, MTDDIFFFinal = 0, TODAYPRODVALFinal = 0, TODAYPRODDIFFFinal = 0, SERIESMTD9Final = 0, SERIESMTD1Final = 0, SERIESTODAY9Final = 0, SERIESTODAY1Final = 0;
                        for (int i = 0; i < dticode.Rows.Count; i++)
                        {
                            MnthlyProdFinal += fgen.make_double(dticode.Rows[i]["PRODPLAN"].ToString());
                            MNTHLYPRODVALFinal += fgen.make_double(dticode.Rows[i]["PROD_PLAN_VAL"].ToString());
                            todayqtyFinal += fgen.make_double(dticode.Rows[i]["TOTAL_QTY"].ToString());
                            todayvalFinal += fgen.make_double(dticode.Rows[i]["TODAY_VAL"].ToString());
                            weekqtyFinal += fgen.make_double(dticode.Rows[i]["WEEKTILLDATEACHVDQTY"].ToString());
                            weekvalFinal += fgen.make_double(dticode.Rows[i]["WEEKTILLDATEACHVDVALUE"].ToString());
                            monthqtyFinal += fgen.make_double(dticode.Rows[i]["MONTHTILLDATEQTY"].ToString());
                            monthvalFinal += fgen.make_double(dticode.Rows[i]["TILLDATEVAL"].ToString());
                            prodvaladdFinal += fgen.make_double(dticode.Rows[i]["VALUEADDITION"].ToString());
                            MTDPRODVALFinal += fgen.make_double(dticode.Rows[i]["MTDPRODVALUE"].ToString());
                            TODAYPRODVALFinal += fgen.make_double(dticode.Rows[i]["TODAYPRODVALUE"].ToString());
                        }
                        int date = int.Parse(todt.Substring(3, 2));
                        Int16 Curryear = Convert.ToInt16(year);
                        int days = DateTime.DaysInMonth(Curryear, date);
                        int daysleft = days - int.Parse(todt.Substring(0, 2));
                        string header1 = "PLAN / DAILY PRODUCTION REPORT (F.G.S)";
                        string header2 = "PRODUCTION REPORT DATE : " + todt + "";
                        drrow1Final["WIPCODE"] = dr0["WIPCODE"].ToString();
                        drrow1Final["wd"] = "W.D :- " + (days - 4).ToString();
                        drrow1Final["daysleft"] = "No. Days Left - " + daysleft.ToString();
                        drrow1Final["MAKER"] = dr0["MAKER"].ToString();
                        drrow1Final["PRODPLAN"] = MnthlyProdFinal;
                        drrow1Final["PROD_PLAN_VAL"] = MNTHLYPRODVALFinal;
                        drrow1Final["TOTAL_QTY"] = todayqtyFinal;
                        drrow1Final["TODAY_VAL"] = todayvalFinal;
                        drrow1Final["WEEKTILLDATEACHVDQTY"] = weekqtyFinal;
                        drrow1Final["WEEKTILLDATEACHVDVALUE"] = weekvalFinal;
                        drrow1Final["MONTHTILLDATEQTY"] = monthqtyFinal;
                        drrow1Final["TILLDATEVAL"] = monthvalFinal;
                        drrow1Final["VALUEADDITION"] = prodvaladdFinal;
                        drrow1Final["MTDPRODVALUE"] = MTDPRODVALFinal;
                        drrow1Final["TODAYPRODVALUE"] = TODAYPRODVALFinal;
                        drrow1Final["Header1"] = header1;
                        drrow1Final["Header2"] = header2;
                        dtm11Final.Rows.Add(drrow1Final);
                    }
                    ds = new DataSet();
                    ds.Tables.Add(dtm11Final);
                    fgen.Print_Report_BYDS(co_cd, frm_qstr, mbr, "crptSFLGDProd", "crptSFLGDProd", ds, "");
                    #endregion
                    break;

                case "15194":
                    #region Jobwork Pending List Item Wise
                    mq1 = ""; mq2 = ""; cond = ""; cond1 = "";
                    dt = new DataTable();
                    if (hfHead.Value.Contains("LIKE") && hfParty.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode " + hfParty.Value + " and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and a.icode " + hfParty.Value + "";
                        }
                    }
                    else if (hfHead.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode in (" + hfParty.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and a.icode in (" + hfParty.Value + ")";
                        }
                    }
                    else if (hfParty.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode " + hfParty.Value + " and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                            cond1 = "and a.icode " + hfParty.Value + "";
                        }
                    }
                    else
                    {
                        cond = "and a.icode in (" + hfParty.Value + ") and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                        cond1 = "and a.icode in (" + hfParty.Value + ")";
                    }
                    mq0 = "select distinct a.acode from rgpmst a where a." + branch_Cd + " and a.type='21' " + cond + " order by a.acode";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm = new DataTable();
                    dtm.Columns.Add("ICODE", typeof(string));
                    dtm.Columns.Add("INAME", typeof(string));
                    dtm.Columns.Add("PARTNO", typeof(string));

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtm.Columns.Add("R" + dt.Rows[i]["acode"].ToString().Trim(), typeof(double));
                    }

                    dtm.Columns.Add("TOTAL_QTY", typeof(double));
                    dtm.Columns.Add("TOTAL_VALUE", typeof(double));

                    dt1 = new DataTable();
                    //mq2 = "select a.branchcd,trim(a.icode) as icode,i.iname,trim(i.cpartno) as partno,trim(a.acode) as acode,sum(a.iqtyout) as total_qty,sum(a.iamount) as total_value from rgpmst a,item i where trim(a.icode)=trim(i.icode) and a." + branch_Cd + " and a.type='21' and a.vchdate " + xprdrange + " and a.icode in (" + hfParty.Value + ") and a.isize in (" + hfHead.Value + ") group by a.branchcd,trim(a.icode),trim(a.acode),i.iname,trim(i.cpartno) order by icode";
                    // ORIGINAL mq2 = "select a.icode,a.acode,i.iname,trim(i.cpartno) as partno,sum(in_qty) as in_qty,sum(out_qty) as out_qty,sum(outamt) as outamt,sum(out_qty)-sum(in_qty) as tot from (select vchnum,vchdate,trim(icode) as icode,trim(acode) as acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout * out_irate) as outamt from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND VCHDATE " + xprdrange + " and a.isize in (" + hfHead.Value + ") union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and type='09' and rgpdate " + xprdrange + " AND STORE<>'R' ) group by vchnum,vchdate,icode,acode  having sum(iqtyout)-sum(iqtyin)>0) a,item i where trim(a.icode)=trim(i.icode) and a.icode in (" + hfParty.Value + ") group by a.icode,a.acode,i.iname,trim(i.cpartno)  order by icode";
                    mq2 = "select a.icode,a.acode,i.iname,trim(i.cpartno) as partno,sum(in_qty) as in_qty,sum(out_qty) as out_qty,sum(outamt) as outamt,sum(out_qty)-sum(in_qty) as tot from (select vchnum,vchdate,trim(icode) as icode,trim(acode) as acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout * out_irate) as outamt from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND VCHDATE " + xprdrange + " " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and type='09' and rgpdate " + xprdrange + " AND STORE<>'R' " + cond1 + " ) group by vchnum,vchdate,icode,acode  having sum(iqtyout)-sum(iqtyin)>0) a,item i where trim(a.icode)=trim(i.icode) group by a.icode,a.acode,i.iname,trim(i.cpartno)  order by icode";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq2);
                    if (dt1.Rows.Count > 0)
                    {
                        view1 = new DataView(dt1);
                        dtdrsim = new DataTable();
                        dtdrsim = view1.ToTable(true, "icode");
                        foreach (DataRow dr in dtdrsim.Rows)
                        {
                            view2 = new DataView(dt1, "icode='" + dr["icode"].ToString().Trim() + "' ", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = view2.ToTable();
                            dr2 = dtm.NewRow();
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                dr2["icode"] = dticode.Rows[i]["icode"].ToString().Trim();
                                dr2["iname"] = dticode.Rows[i]["iname"].ToString().Trim();
                                dr2["partno"] = dticode.Rows[i]["partno"].ToString().Trim();
                                try
                                {
                                    dr2["R" + dticode.Rows[i]["ACODE"].ToString().Trim()] = dticode.Rows[i]["tot"].ToString().Trim();
                                }
                                catch { }
                                db1 += fgen.make_double(dticode.Rows[i]["tot"].ToString().Trim());
                                db2 = fgen.make_double(dticode.Rows[i]["outamt"].ToString().Trim());
                                db3 = fgen.make_double(dticode.Rows[i]["out_qty"].ToString().Trim());
                                db4 = db2 / db3;
                                db5 += db4 * fgen.make_double(dticode.Rows[i]["tot"].ToString().Trim());
                                dr2["total_qty"] = db1;
                                dr2["total_value"] = Math.Round(db5, 2);
                            }
                            dtm.Rows.Add(dr2);
                        }
                    }

                    dr1 = null;
                    dr1 = dtm.NewRow();
                    foreach (DataColumn dc in dtm.Columns)
                    {
                        total = 0;
                        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2)
                        {

                        }
                        else
                        {
                            mq3 = "sum(" + dc.ColumnName + ")";
                            total += fgen.make_double(dtm.Compute(mq3, "").ToString());
                            dr1[dc] = total;
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        dr1["PartNo"] = "TOTAL";
                        dtm.Rows.InsertAt(dr1, 0);
                        // REMOVING PARTIES WHICH HAVE TOTAL =0
                        dt1 = new DataTable();
                        dt1 = dtm;
                        string[] col11 = new string[dt1.Columns.Count];
                        for (int i = 0; i < dt1.Columns.Count; i++)
                        {
                            if (i > 2)
                            {
                                if (fgen.make_double(dt1.Rows[0][i].ToString().Trim()) <= 0)
                                {
                                    col11[i] = dt1.Columns[i].ColumnName;
                                }
                            }
                        }

                        for (int kk = 0; kk < col11.Length; kk++)
                        {
                            if (col11[kk] != null)
                            {
                                dtm.Columns.Remove(col11[kk].ToString().Trim());
                            }
                        }
                    }

                    dt = new DataTable();
                    mq0 = "select distinct a.acode,f.aname from rgpmst a,famst f where trim(a.acode)=trim(f.acode) and a." + branch_Cd + " and a.type='21' " + cond1 + " order by a.acode";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    foreach (DataColumn dc in dtm.Columns)
                    {
                        int abc = dc.Ordinal;
                        if (abc > 2)
                        {
                            string name = dc.ToString().Substring(1, 6);

                            string myname = fgen.seek_iname_dt(dt, "acode='" + name + "'", "aname");
                            try
                            {
                                if (myname != "0")
                                {
                                    dtm.Columns[abc].ColumnName = myname;
                                }
                            }
                            catch
                            {
                                dtm.Columns[abc].ColumnName = myname + " (" + name + ")";
                            }
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dtm;
                    fgen.Fn_open_rptlevel("Jobwork Pending List Item Wise for the Period " + fromdt + " to " + todt, frm_qstr);
                    #endregion
                    break;

                case "15195":
                    #region Jobwork Pending Summary (Detailed)
                    cond = ""; cond1 = "";
                    if (hfHead.Value.Contains("LIKE") && hfParty.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode " + hfParty.Value + " and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and a.icode " + hfParty.Value + "";
                        }
                    }
                    else if (hfHead.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode in (" + hfParty.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and a.icode in (" + hfParty.Value + ")";
                        }
                    }
                    else if (hfParty.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode " + hfParty.Value + " and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                            cond1 = "and a.icode " + hfParty.Value + "";
                        }
                    }
                    else
                    {
                        cond = "and a.icode in (" + hfParty.Value + ") and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                        cond1 = "and a.icode in (" + hfParty.Value + ")";
                    }
                   // SQuery = "select A.acode AS PARTY_CODE,F.ANAME AS PARTY,A.icode AS ITEM_CODE,I.INAME AS ITEM,TRIM(I.CPARTNO) AS PARTNO,sum(tot) as bal,0 as bal_val,sum(days30) as days30,sum(value30) as value30,sum(days60) as days60,sum(value60) as value60,sum(days90) as days90,sum(value90) as value90,sum(days180) as days180,sum(value180) as value180,sum(qty_others) as qty_more_than_180,sum(value_others) as val_more_than_180 from (select acode,icode,tot,(out_amt/out_qty)*tot as val,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then tot end) as Days30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then (out_amt/out_qty)*tot end) as Value30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then tot end) as Days60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then (out_amt/out_qty)*tot end) as Value60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then tot end) as Days90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then (out_amt/out_qty)*tot end) as Value90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then tot end) as Days180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then (out_amt/out_qty)*tot end) as Value180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >181) then tot end) as Qty_others,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >181) then (out_amt/out_qty)*tot end) as Value_others from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt  from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND a.VCHDATE between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and A.type='09' and A.rgpdate between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and a.store<>'R' " + cond1 + ") group by icode,acode,vchnum,vchdate having sum(iqtyout)-sum(iqtyin)>0))A, ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) group by A.acode,A.icode,F.ANAME,I.INAME,TRIM(I.CPARTNO) order by PARTY_CODE";
                    SQuery = "select A.acode AS PARTY_CODE,F.ANAME AS PARTY,A.icode AS ITEM_CODE,I.INAME AS ITEM,TRIM(I.CPARTNO) AS PARTNO,sum(tot) as bal,0 as bal_val,sum(days30) as upto_30days,sum(value30) as value_upto_30days,sum(days60) as upto_31_60Days,sum(value60) as value_upto_31_60Days,sum(days90) as upto_61_90days,sum(value90) as value_upto_61_90days,sum(days180) as upto_91_180days,sum(value180) as value_upto_91_180days,sum(qty_others) as days_181_and_above,sum(value_others) as value_181_and_above_Days from (select acode,icode,tot,(out_amt/out_qty)*tot as val,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then tot end) as Days30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then (out_amt/out_qty)*tot end) as Value30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then tot end) as Days60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then (out_amt/out_qty)*tot end) as Value60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then tot end) as Days90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then (out_amt/out_qty)*tot end) as Value90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then tot end) as Days180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then (out_amt/out_qty)*tot end) as Value180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >=181) then tot end) as Qty_others,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >=181) then (out_amt/out_qty)*tot end) as Value_others from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt  from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND a.VCHDATE between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and A.type='09' and A.rgpdate between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and a.store<>'R' " + cond1 + ") group by icode,acode,vchnum,vchdate having sum(iqtyout)-sum(iqtyin)>0))A, ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) group by A.acode,A.icode,F.ANAME,I.INAME,TRIM(I.CPARTNO) order by PARTY_CODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0;
                        db1 = fgen.make_double(dt.Rows[i]["value_upto_30days"].ToString());
                        db2 = fgen.make_double(dt.Rows[i]["value_upto_31_60Days"].ToString());
                        db3 = fgen.make_double(dt.Rows[i]["value_upto_61_90days"].ToString());
                        db4 = fgen.make_double(dt.Rows[i]["value_upto_91_180days"].ToString());
                        db5 = fgen.make_double(dt.Rows[i]["value_181_and_above_Days"].ToString());
                        db6 = db1 + db2 + db3 + db4 + db5;
                        //db7 = db6 / fgen.make_double(dt.Rows[i]["bal"].ToString());
                        dt.Rows[i]["bal_val"] = Math.Round(db6, 2); ;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Jobwork Pending Summary (Detailed) as on " + todt, frm_qstr);
                    #endregion
                    break;

                case "15196":
                    #region Jobwork Pending Summary
                    cond = ""; cond1 = "";
                    if (hfHead.Value.Contains("LIKE") && hfParty.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode " + hfParty.Value + " and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and a.icode " + hfParty.Value + "";
                        }
                    }
                    else if (hfHead.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode in (" + hfParty.Value + ") and nvl(a.isize,'-') " + hfHead.Value + "";
                            cond1 = "and a.icode in (" + hfParty.Value + ")";
                        }
                    }
                    else if (hfParty.Value.Contains("LIKE"))
                    {
                        if (hfOpen.Value != " LIKE '%'" && hfParty.Value == " LIKE '%'")
                        {
                            cond = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ") and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                            cond1 = "and substr(trim(a.icode),0,4) in (" + hfOpen.Value + ")";
                        }
                        else
                        {
                            cond = "and a.icode " + hfParty.Value + " and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                            cond1 = "and a.icode " + hfParty.Value + "";
                        }
                    }
                    else
                    {
                        cond = "and a.icode in (" + hfParty.Value + ") and nvl(a.isize,'-') in (" + hfHead.Value + ")";
                        cond1 = "and a.icode in (" + hfParty.Value + ")";
                    }


                    SQuery = "select A.acode AS PARTY_CODE,F.ANAME AS PARTY,sum(tot) as bal,0 as bal_val,sum(days30) as days30,sum(value30) as value30,sum(days60) as days60,sum(value60) as value60,sum(days90) as days90,sum(value90) as value90,sum(days180) as days180,sum(value180) as value180,sum(qty_others) as qty_more_than_180,sum(value_others) as val_more_than_180 from (select acode,icode,tot,(out_amt/out_qty)*tot as val,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then tot end) as Days30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then (out_amt/out_qty)*tot end) as Value30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then tot end) as Days60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then (out_amt/out_qty)*tot end) as Value60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then tot end) as Days90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then (out_amt/out_qty)*tot end) as Value90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then tot end) as Days180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then (out_amt/out_qty)*tot end) as Value180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >181) then tot end) as Qty_others,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >181) then (out_amt/out_qty)*tot end) as Value_others from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt  from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND a.VCHDATE between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and A.type='09' and A.rgpdate between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and a.store<>'R' " + cond1 + ") group by icode,acode,vchnum,vchdate having sum(iqtyout)-sum(iqtyin)>0))A, ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) group by A.acode,F.ANAME order by PARTY_CODE";

                    //original  SQuery = "select A.acode AS PARTY_CODE,F.ANAME AS PARTY,sum(tot) as bal,sum(days30) as days30,sum(value30) as value30,sum(days60) as days60,sum(value60) as value60,sum(days90) as days90,sum(value90) as value90,sum(days180) as days180,sum(value180) as value180,sum(qty_others) as qty_more_than_180,sum(value_others) as val_more_than_180 from (select acode,icode,tot,(out_amt/out_qty)*tot as val,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then tot end) as Days30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then (out_amt/out_qty)*tot end) as Value30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then tot end) as Days60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then (out_amt/out_qty)*tot end) as Value60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then tot end) as Days90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then (out_amt/out_qty)*tot end) as Value90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then tot end) as Days180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then (out_amt/out_qty)*tot end) as Value180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >181) then tot end) as Qty_others,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >181) then (out_amt/out_qty)*tot end) as Value_others from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt  from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND a.VCHDATE between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') AND A.ISIZE IN (" + hfHead.Value + ") union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and A.type='09' and A.rgpdate between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and a.store<>'R') group by icode,acode,vchnum,vchdate having sum(iqtyout)-sum(iqtyin)>0))A, ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) and TRIM(a.icode) in (" + hfParty.Value + ")  group by A.acode,F.ANAME order by PARTY_CODE";
                    SQuery = "select A.acode AS PARTY_CODE,F.ANAME AS PARTY,sum(tot) as bal,0 as bal_val,sum(days30) as upto_30days,sum(value30) as value_upto_30days,sum(days60) as upto_31_60Days,sum(value60) as value_upto_31_60Days,sum(days90) as upto_61_90days,sum(value90) as value_upto_61_90days,sum(days180) as upto_91_180days,sum(value180) as value_upto_91_180days,sum(qty_others) as days_181_and_above,sum(value_others) as value_181_and_above_Days from (select acode,icode,tot,(out_amt/out_qty)*tot as val,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then tot end) as Days30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 0 and 30) then (out_amt/out_qty)*tot end) as Value30,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then tot end) as Days60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 31 and 60) then (out_amt/out_qty)*tot end) as Value60,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then tot end) as Days90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 61 and 90) then (out_amt/out_qty)*tot end) as Value90,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then tot end) as Days180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') between 91 and 180) then (out_amt/out_qty)*tot end) as Value180,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >=181) then tot end) as Qty_others,(case when (to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_date(vchdate,'dd/mm/yyyy') >=181) then (out_amt/out_qty)*tot end) as Value_others from (select vchnum,vchdate,icode,acode,sum(iqtyout)-sum(iqtyin) as tot, sum(iqtyout) as out_qty,sum(iqtyin) as in_qty,sum(iqtyout*out_irate) as out_amt,sum(iqtyin*in_irate) as in_amt  from (SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,a.iqtyout,a.IRATE as out_irate,0 as iqtyin,0 as in_irate FROM RGPMST a WHERE a." + branch_Cd + " AND A.TYPE='21' AND a.VCHDATE between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') " + cond + " union all select a.rgpnum,to_char(a.rgpdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,0 as iqtyout,0 as out_irate,a.iqtyin+NVL(A.REJ_RW,'0') AS IQTYIN,a.IRATE as in_irate from ivoucher a where A." + branch_Cd + " and A.type='09' and A.rgpdate between to_date('01/04/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and a.store<>'R' " + cond1 + ") group by icode,acode,vchnum,vchdate having sum(iqtyout)-sum(iqtyin)>0))A, ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) group by A.acode,F.ANAME order by PARTY_CODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0;
                        db1 = fgen.make_double(dt.Rows[i]["value_upto_30days"].ToString());
                        db2 = fgen.make_double(dt.Rows[i]["value_upto_31_60Days"].ToString());
                        db3 = fgen.make_double(dt.Rows[i]["value_upto_61_90days"].ToString());
                        db4 = fgen.make_double(dt.Rows[i]["value_upto_91_180days"].ToString());
                        db5 = fgen.make_double(dt.Rows[i]["value_181_and_above_Days"].ToString());
                        db6 = db1 + db2 + db3 + db4 + db5;
                        //db7 = db6 / fgen.make_double(dt.Rows[i]["bal"].ToString());
                        dt.Rows[i]["bal_val"] = Math.Round(db6, 2); ;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Jobwork Pending Summary as on " + todt, frm_qstr);
                    #endregion
                    break;
                #endregion
            }
        }
    }

    void wip_stk_vw()
    {
        xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
        xprd1 = "BETWEEN TO_DATE('01/04/2010','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
        mq10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
        xprd2 = "between to_Date('" + mq10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

        mq0 = "select C.ac_acode,C.iname as Item_Name,C.IRATE1,c.irate,C.Cpartno as Part_No,c.iweight,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,sum(a.iqtyin) as Input,sum(a.iqtyout) as Output,sum(a.iqtyin)-sum(a.iqtyout) as Balance,trim(a.stage) as stage,a.icode,a.wolink from (select type,stage,maincode,icode,iqtyin,iqtyout,'op' as wolink From wipstk where branchcd='" + mbr + "' and type='50' and vchdate " + xprd2 + " union all select type,stage,icode,icode,iqtyin,iqtyout,'WIP' as wolink From ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and store='W' union all select '50' as type,'61' as stage,icode,icode,yr_" + year + " as iqtyin,0 as iqtyout,'itmbal' as wolink From itembal where branchcd='" + mbr + "' and yr_" + year + ">0 and substr(icode,1,1) in ('1','7') union all select type,'61' as stage,icode,icode,iqtyin,iqtyout,'IVCH' as wolink From ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store!='R' and store!='W' and substr(icode,1,1) in ('1','7') union all select type,'62' as stage,icode,icode,iqtyout,0 as iqtyin,'RGP' as wolink From RGPMST where branchcd='" + mbr + "' and type ='21' and vchdate " + xprd1 + "  union all select type,'62' as stage,icode,icode,0 as iqtyout,iqtyin+nvl(rej_rw,0),'MRR' as wolink From ivoucher where branchcd='" + mbr + "' and type ='09' and vchdate " + xprd1 + " and store<>'R' ) a,(Select icode,iname,cpartno,wt_net,mat5,mat6,mat7,salloy,IRATE1,irate,iweight,ac_acode from item where substr(icode,1,1) in ('1','7'))c where trim(a.icode)=trim(c.icode) group by C.ac_acode,C.iname,C.cpartno,C.IRATE1,c.irate,c.iweight,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,trim(a.stage),a.icode,a.wolink";
        //SQuery = "select trim(Item_name) as Item_name,trim(Part_No) as Part_No,sum(stg01) as stg01,sum(stg02) as stg02,sum(stg03) as stg03,sum(stg04) as stg04,sum(stg05) as stg05,sum(stg06) as stg06,sum(stg07) as stg07,sum(stg08) as stg08,sum(stg09) as stg09,sum(stg11) as stg11,sum(stg12) as stg12,sum(stg13) as stg13,sum(stg14) as stg14,sum(stg15) as stg15,sum(stg16) as stg16,sum(stg03)+sum(stg04)+sum(stg05)+sum(stg06)+sum(stg07)+sum(stg08)+sum(stg09)+sum(stg11)+sum(stg12)+sum(stg13)+sum(stg14)+sum(stg15) as total,trim(icode)as  icode,iweight,wt_net,'-' as wolink,ac_acode,irate,IRATE1 from (select ac_acode,Item_Name,Part_No,IRATE1,irate,iweight,wt_net,mat5,mat6,mat7,salloy,decode(stage,'61',Balance,0) as Stg01,decode(stage,'62',Balance,0) as Stg02,decode(stage,'63',balance,0) as Stg03,decode(stage,'64',balance,0) as Stg04,decode(stage,'65',balance,0) as Stg05,decode(stage,'66',balance,0) as Stg06,decode(stage,'67',balance,0) as Stg07,decode(stage,'68',balance,0) as Stg08,decode(stage,'69',balance,0)as Stg09,decode(stage,'6A',balance,0) as Stg11,decode(stage,'6B',balance,0) as Stg12,decode(stage,'6C',balance,0) as Stg13,decode(stage,'6D',balance,0) as Stg14,decode(stage,'6E',balance,0) as Stg15,decode(stage,'6R',balance,0) as Stg16,icode,'-' as wolink  from (" + mq0 + ")) group by ac_acode,trim(Item_Name),trim(Part_No),trim(Icode),iweight,irate,IRATE1,wt_net,mat5,mat6,mat7,salloy order by trim(Item_Name)";
        SQuery = "select trim(Item_name) as Item_name,trim(Part_No) as Part_No,sum(stg01) as stg01,sum(stg02) as stg02,sum(stg03) as stg03,sum(stg04) as stg04,sum(stg05) as stg05,sum(stg06) as stg06,sum(stg07) as stg07,sum(stg08) as stg08,sum(stg09) as stg09,sum(stg11) as stg11,sum(stg12) as stg12,sum(stg13) as stg13,sum(stg14) as stg14,sum(stg15) as stg15,sum(stg16) as stg16,sum(stg17) as stg17,sum(stg03)+sum(stg04)+sum(stg05)+sum(stg06)+sum(stg07)+sum(stg08)+sum(stg09)+sum(stg11)+sum(stg12)+sum(stg13)+sum(stg14)+sum(stg15)+sum(stg16) as total,trim(icode)as  icode,iweight,wt_net,'-' as wolink,ac_acode,irate,IRATE1 from (select ac_acode,Item_Name,Part_No,IRATE1,irate,iweight,wt_net,mat5,mat6,mat7,salloy,decode(stage,'61',Balance,0) as Stg01,decode(stage,'62',Balance,0) as Stg02,decode(stage,'63',balance,0) as Stg03,decode(stage,'64',balance,0) as Stg04,decode(stage,'65',balance,0) as Stg05,decode(stage,'66',balance,0) as Stg06,decode(stage,'67',balance,0) as Stg07,decode(stage,'68',balance,0) as Stg08,decode(stage,'69',balance,0)as Stg09,decode(stage,'6A',balance,0) as Stg11,decode(stage,'6B',balance,0) as Stg12,decode(stage,'6C',balance,0) as Stg13,decode(stage,'6D',balance,0) as Stg14,decode(stage,'6E',balance,0) as Stg15,decode(stage,'6F',balance,0) as Stg16,decode(stage,'6R',balance,0) as Stg17,icode,'-' as wolink  from (" + mq0 + ")) group by ac_acode,trim(Item_Name),trim(Part_No),trim(Icode),iweight,irate,IRATE1,wt_net,mat5,mat6,mat7,salloy order by trim(Item_Name)";
        fgen.execute_cmd(frm_qstr, co_cd, "create or replace view wipcolstkw_" + mbr + " as(SELECT * FROM (" + SQuery + "))");
    }

    public void BOM()
    {
        fgen.execute_cmd(frm_qstr, co_cd, "delete from extrusion where branchcd='" + mbr + "' and type='EX' AND TRIM(ENT_BY)='" + uname + "'");
        mdt = new DataTable(); dt3 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
        cond = "";
        SQuery = "Select a.icode,a.ibcode,a.ibqty,a.srno,(case when B.IQD>0 then B.IQD else B.irate end) as itrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD='" + mbr + "' order by a.srno,a.icode";
        if (co_cd == "KHEM" || co_cd == "BUPL" || co_cd == "XDIL") SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as itrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) and trim(a.icode) " + cond + " AND a.BRANCHCD not in ('DD','88') order by a.srno,a.icode";
        dt3 = fgen.getdata(frm_qstr, co_cd, SQuery);
        vdt = new DataTable();
        mdt.Columns.Add(new DataColumn("lvl", typeof(string)));
        mdt.Columns.Add(new DataColumn("icode", typeof(string)));
        mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
        mdt.Columns.Add(new DataColumn("ibqty", typeof(string)));
        mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
        mdt.Columns.Add(new DataColumn("irate", typeof(string)));
        mdt.Columns.Add(new DataColumn("val", typeof(string)));

        mdt.Columns.Add(new DataColumn("iname", typeof(string)));
        mdt.Columns.Add(new DataColumn("ibname", typeof(string)));

        fmdt = new DataTable();
        fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
        fmdt.Columns.Add(new DataColumn("val", typeof(string)));

        fmdt.Columns.Add(new DataColumn("srate", typeof(string)));
        fmdt.Columns.Add(new DataColumn("sqty", typeof(string)));
        fmdt.Columns.Add(new DataColumn("acode", typeof(string)));

        SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD='" + mbr + "' order by a.srno,a.icode,a.ibcode";
        if (co_cd == "KHEM" || co_cd == "BUPL" || co_cd == "XDIL" || co_cd == "GTCF") SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD not in ('DD','88') order by a.srno,a.icode,a.ibcode";
        vdt = fgen.getdata(frm_qstr, co_cd, SQuery); v = 0;
        dt2 = new DataTable();
        SQuery = "Select trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where branchcd='" + mbr + "' and type like '0%' and trim(nvl(finvno,'-'))!='-' and vchdate>=(sysdate-500)  and icode like '9%' order by icode,vdd desc";
        if (co_cd == "BUPL") SQuery = "Select trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where branchcd='" + mbr + "' and type in ('02','05','07') and trim(nvl(finvno,'-'))!='-' and vchdate>=(sysdate-500)  and icode like '9%' order by icode,vdd desc";
        dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
        DataView dist1_view = new DataView(dt3);
        dt_dist = new DataTable();
        if (dist1_view.Count > 0)
        {
            dist1_view.Sort = "icode";
            dt_dist = dist1_view.ToTable(true, "icode");
        }
        foreach (DataRow dt_dist_row in dt_dist.Rows)
        {
            mdt1 = new DataTable();
            mdt1 = mdt.Clone();
            mvdview = new DataView(dt3, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
            dt = new DataTable();
            mvdview.Sort = "srno,icode";
            dt = mvdview.ToTable();
            // filling parent
            foreach (DataRow drc in dt.Rows)
            {
                dro = mdt1.NewRow();
                dro["lvl"] = "1";
                dro["icode"] = drc["icode"].ToString().Trim();
                dro["pcode"] = drc["icode"].ToString().Trim();
                dro["ibqty"] = drc["ibqty"];
                dro["ibcode"] = drc["ibcode"].ToString().Trim();
                dro["irate"] = drc["itrate"].ToString().Trim();
                dro["val"] = "0";
                mdt1.Rows.Add(dro);
            }
            i0 = 1; v = 0;
            for (int i = v; i < mdt1.Rows.Count; i++)
            {
                i0 = 1;
                vdview = new DataView(vdt, "icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                if (vdview.Count > 0)
                {
                    vdview1 = new DataView(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibcode", DataViewRowState.CurrentRows);
                    for (int x = 0; x < vdview.Count; x++)
                    {
                        if (mq0 != vdview[x].Row["icode"].ToString().Trim()) i0 += 1;
                        dro = mdt1.NewRow();
                        dro["lvl"] = i0.ToString();
                        dro["icode"] = vdview[x].Row["icode"].ToString().Trim();
                        mq0 = vdview[x].Row["icode"].ToString().Trim();
                        dro["ibqty"] = (Convert.ToDouble(vdview[x].Row["ibqty"]) * Convert.ToDouble(vdview1[0].Row["ibqty"])).ToString();
                        dro["ibcode"] = vdview[x].Row["ibcode"].ToString().Trim();
                        //dro["irate"] = vdview[0].Row["bchrate"];
                        dro["irate"] = vdview[x].Row["bchrate"];
                        dro["val"] = "0";
                        if (mdt1.Rows[i]["lvl"].ToString() == "1")
                        {
                            mq7 = "";
                            dro["pcode"] = mdt1.Rows[i]["icode"].ToString().Trim();
                            mq7 = mdt1.Rows[i]["icode"].ToString().Trim();
                        }
                        else dro["pcode"] = mq7;
                        v++;

                        mdt1.Rows.Add(dro);
                    } vdview1.Dispose();
                } vdview.Dispose();
            }

            sort_view = new DataView();
            sort_view = mdt1.DefaultView;
            sort_view.Sort = "pcode,lvl,icode";
            mdt1 = new DataTable();
            mdt1 = sort_view.ToTable(true);
            sort_view.Dispose();

            // seeling LC and update value
            for (int i = 0; i < mdt1.Rows.Count; i++)
            {
                vdview = new DataView(mdt1, "icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                if (vdview.Count <= 0)
                {
                    sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                    if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                }
                else mdt1.Rows[i]["irate"] = "0";
                vdview.Dispose();
                mdt1.Rows[i]["val"] = Convert.ToDouble(Convert.ToDouble(mdt1.Rows[i]["ibqty"]) * Convert.ToDouble(mdt1.Rows[i]["irate"]));
            }

            mq0 = "0";
            // making final value
            vdview = new DataView(mdt1, "pcode='" + dt_dist_row["icode"].ToString().Trim() + "'", "pcode", DataViewRowState.CurrentRows);
            for (int i = 0; i < vdview.Count; i++)
            {
                if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(vdview[i].Row["val"].ToString().Trim()), 2).ToString();
                else mq0 = vdview[i].Row["val"].ToString().Trim();
            }
            vdview.Dispose();

            for (int f = 0; f < mdt1.Rows.Count; f++)
            {
                mdt.ImportRow(mdt1.Rows[f]);
            }
            mdt1.Dispose();
            // mdt is table which is having Bom in Expended Form
            dro1 = fmdt.NewRow();
            dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
            dro1["val"] = mq0;
            fmdt.Rows.Add(dro1);
            // fmdt is table which is only having Parant Bom icode and Value

            if (co_cd == "NIRM" || co_cd == "PRAG" || co_cd == "IAIJ" || co_cd == "SFLG" || (co_cd == "BUPL" && dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7"))
            {
                fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE= '" + mq0 + "' WHERE TRIM(ICODE)='" + dt_dist_row["icode"].ToString().Trim() + "'");
            }
        }
    }
}