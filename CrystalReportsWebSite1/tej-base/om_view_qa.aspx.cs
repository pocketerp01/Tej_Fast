using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_qa : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID,party_cd, part_cd;
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
                case "F30128":
                case "F30142": // INWARD QA REPORT
                case "F30143": // INWARD QA REJECTION REPORT                 
                case "F30367": // ADVG DOSSIER INDEX                 
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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
            if (val == "M03012")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
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
                case "F30121":
                    // QA INWARD (TEMPLATE) CHECKLIST 
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F30121", branch_Cd, "a.type='20'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("QA Inward (Template) Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F30126":
                    // QA IN - PROC (TEMPLATE) CHECKLIST 
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F30126", branch_Cd, "a.type='40'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("QA In - Proc (Template) Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F30127":
                    // QA OUTWARD (TEMPLATE) CHECKLIST 
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F30127", branch_Cd, "a.type='10'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("QA Inward (Template) Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F30128":
                    // BASIC MRR PENDING QA 
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F25126", branch_Cd, "a.type like '0%' and a.inspected='N' and a.store='N' and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("MRR Pending QA ,Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F30142":
                    // INWARD QA REPORT
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type like '0%' and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' and a.store in ('Y','N') ", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Inward QA Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F30143":
                    // INWARD QA REJECTION REPORT
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type like '0%' and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Inward QA Rejection Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F30228":
                    // CHART SHOWING INSTANCES OF INWARD REJECTION
                    SQuery = "SELECT TRIM(I.INAME) AS FSTR,SUM(NVL(A.REJ_RW,0)) AS REJ,TRIM(I.INAME) AS INAME FROM IVOUCHER A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '0%' AND A.TYPE!='04' AND A.VCHDATE " + xprdrange + " AND A.STORE='Y' AND A.INSPECTED='Y' AND NVL(A.REJ_RW,0)>0 GROUP BY TRIM(I.INAME) ORDER BY INAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Inward Rejection For The Period " + fromdt + " To " + todt, "spline", "", "", SQuery, "");
                    break;

                case "F30229":
                    // CHART SHOWING INSTANCES OF LINE REJECTION
                    SQuery = "SELECT TRIM(I.INAME) AS FSTR,SUM(NVL(A.IQTYIN,0)) AS REJ,TRIM(I.INAME) AS INAME FROM IVOUCHER A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='14' AND A.VCHDATE " + xprdrange + " AND A.STORE='R' GROUP BY TRIM(I.INAME) ORDER BY INAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Line Rejection For The Period " + fromdt + " To " + todt, "spline", "", "", SQuery, "");
                    break;

                case "F30367":
                    mq0 = ""; mq1 = "";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Trim().Length <= 1)
                    {
                        mq0 = " and acode like '%'";
                    }
                    else
                    {
                        mq0 = " and acode='" + party_cd + "'";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        mq1 = " and cpartno like '%'";
                    }
                    else
                    {
                        mq1 = " and cpartno in (" + part_cd + ")";
                    }
                    //SQuery = "select b.cdrgno as line_item_no,b.cpartno as partno ,a.valve_tag , a.heading as doc_heading,a.vchnum as document_no,to_char(a.vchdate,'dd/mm/yyyy') as document_date, a.ent_by as created_BY,a.upload1 as Doc_Upload from (select distinct BTCHNO,acode,cpartno,obsv2 as Valve_tag,vchnum ,vchdate , ent_by,'' as upload1, 'Valve TC' as heading from inspvch where branchcd='" + mbr + "' and type='84' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") and srno>2  union all  select distinct BTCHNO,acode,cpartno,obsv2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Dimensional Report' as heading from inspvch where branchcd='" + mbr + "' and  type='82' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") and srno>2  union all  select distinct BTCHNO,acode,cpartno,col2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'PMI Report' as heading from inspvch where branchcd='" + mbr + "' and type='77' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all  select distinct BTCHNO,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Liquid Penetration Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='81' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all  select distinct BTCHNO,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Magnetic Particle Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='79' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") ) a, somas b where trim(a.acode)||trim(a.cpartno)||trim(a.btchno)=trim(b.acode)||trim(b.org_invno)||trim(b.cdrgno) order by  b.cdrgno, a.valve_tag  asc";
                    SQuery = "select b.cdrgno as line_item_no,b.cpartno as partno ,a.valve_tag , a.heading as doc_heading,a.vchnum as document_no,to_char(a.vchdate,'dd/mm/yyyy') as document_date, a.ent_by as created_BY,a.upload1 as Doc_Upload from (select distinct BTCHNO,acode,cpartno,obsv2 as Valve_tag,vchnum ,vchdate , ent_by,'' as upload1, 'Valve TC' as heading from inspvch where branchcd='" + mbr + "' and type='84' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") and srno>2 union all  select distinct BTCHNO,acode,cpartno,obsv2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Dimensional Report' as heading from inspvch where branchcd='" + mbr + "' and  type='82' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") and srno>2 union all  select distinct BTCHNO,acode,cpartno,col2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'PMI Report' as heading from inspvch where branchcd='" + mbr + "' and type='77' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all  select distinct BTCHNO,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Liquid Penetration Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='81' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all  select distinct BTCHNO,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Magnetic Particle Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='79' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all select distinct btchno,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Pickling And Passivation' as heading from wb_inspvch where branchcd='" + mbr + "' and type='85' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all select distinct btchno,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Surface Preparation Painting And Marking' as heading from wb_inspvch where branchcd='" + mbr + "' and type='86'  and acode='" + party_cd + "' and cpartno in (" + part_cd + ") union all select distinct '-' as btchno,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Test Certificate' as heading from inspvch where branchcd='" + mbr + "' and type='83' and acode='" + party_cd + "' and cpartno in (" + part_cd + ") ) a, somas b where trim(a.acode)||trim(a.cpartno)=trim(b.acode)||trim(b.org_invno) order by b.cdrgno, a.valve_tag asc";
                    SQuery = "select b.cdrgno as line_item_no,b.cpartno as partno ,a.valve_tag , a.heading as doc_heading,a.vchnum as document_no,to_char(a.vchdate,'dd/mm/yyyy') as document_date, a.ent_by as created_BY,a.upload1 as Doc_Upload from (select distinct BTCHNO,acode,cpartno,obsv2 as Valve_tag,vchnum ,vchdate , ent_by,'' as upload1, 'Valve TC' as heading from inspvch where branchcd='" + mbr + "' and type='84' " + mq0 + mq1 + " and srno>2 union all  select distinct BTCHNO,acode,cpartno,obsv2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Dimensional Report' as heading from inspvch where branchcd='" + mbr + "' and  type='82' " + mq0 + mq1 + " and srno>2 union all  select distinct BTCHNO,acode,cpartno,col2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'PMI Report' as heading from inspvch where branchcd='" + mbr + "' and type='77' " + mq0 + mq1 + " union all  select distinct BTCHNO,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Liquid Penetration Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='81' " + mq0 + mq1 + " union all  select distinct BTCHNO,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Magnetic Particle Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='79' " + mq0 + mq1 + " union all select distinct btchno,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Pickling And Passivation' as heading from wb_inspvch where branchcd='" + mbr + "' and type='85' " + mq0 + mq1 + " union all select distinct btchno,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Surface Preparation Painting And Marking' as heading from wb_inspvch where branchcd='" + mbr + "' and type='86' " + mq0 + mq1 + " union all select distinct '-' as btchno,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Test Certificate' as heading from inspvch where branchcd='" + mbr + "' and type='83' " + mq0 + mq1 + ") a, somas b where trim(a.acode)||trim(a.cpartno)=trim(b.acode)||trim(b.org_invno) order by b.cdrgno, a.valve_tag asc";
                    SQuery = "select a.cpartno as work_order,a.btchno as line_item_no,b.cpartno as figure,a.valve_tag,a.heading as doc_heading,a.vchnum as document_no,to_char(a.vchdate,'dd/mm/yyyy') as document_date, a.ent_by as created_BY,a.upload1 as Doc_Upload from (select distinct BTCHNO,grade,icode,acode,cpartno,obsv2 as Valve_tag,vchnum ,vchdate , ent_by,'' as upload1, 'Valve TC' as heading from inspvch where branchcd='" + mbr + "' and type='84' " + mq0 + mq1 + " and srno>2 union all  select distinct BTCHNO,grade,icode,acode,cpartno,obsv2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Dimensional Report' as heading from inspvch where branchcd='" + mbr + "' and  type='82' " + mq0 + mq1 + " and srno>2 union all  select distinct BTCHNO,grade,icode,acode,cpartno,col2 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'PMI Report' as heading from inspvch where branchcd='" + mbr + "' and type='77' " + mq0 + mq1 + " union all  select distinct BTCHNO,grade,icode,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Liquid Penetration Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='81' " + mq0 + mq1 + " union all  select distinct BTCHNO,grade,icode,acode,cpartno,obsv1 as Valve_tag,vchnum , vchdate , ent_by, '' as upload1,'Magnetic Particle Examination' as heading from inspvch where branchcd='" + mbr + "' and  type='79' " + mq0 + mq1 + " union all select distinct btchno,grade,icode,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Pickling And Passivation' as heading from wb_inspvch where branchcd='" + mbr + "' and type='85' " + mq0 + mq1 + " union all select distinct btchno,grade,icode,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Surface Preparation Painting And Marking' as heading from wb_inspvch where branchcd='" + mbr + "' and type='86' " + mq0 + mq1 + " union all select distinct TRIM(FOOTNOTE) as btchno,grade,icode,acode,cpartno,obsv1 as valve_tag,vchnum,vchdate,ent_by,'' as upload1,'Test Certificate' as heading from inspvch where branchcd='" + mbr + "' and type='83' " + mq0 + mq1 + ") a, somas b where trim(a.btchno)||trim(a.acode)||trim(a.cpartno)||trim(a.grade)=trim(b.cdrgno)||trim(b.acode)||trim(b.org_invno)||b.branchcd||trim(b.type)||trim(b.ordno)||to_char(b.orddt,'dd/mm/yyyy') order by b.cdrgno, a.valve_tag asc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Work Order Dossier Index", frm_qstr);
                    break;
            }
        }
    }
}