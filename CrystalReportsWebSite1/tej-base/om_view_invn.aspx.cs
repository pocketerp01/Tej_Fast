using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_invn : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, cond1, cond2, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, mq11, mq12, yr_fld, cDT1, cDT2, frm_myear, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joincond;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, mdt, dticode, dticode2, ph_tbl;
    double month, to_cons, itot_stk, itv, db, db1, db2, db3, db4, db5, db6, db7; DataRow dr1, oporow, ROWICODE, ROWICODE2; DataView dv, dv1, view1, view2;
    string opbalyr, param, eff_Dt, xprdrange1, xprdRange1, cldt = "", frm_cDt1, frm_cDt2;
    string er1, er2, er3, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID, frm_cocd, frm_mbr, xprdRange;
    string frm_UserID, party_cd, part_cd;
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
                frm_cocd = co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                frm_mbr = mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
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
        i0 = 0;
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
        { hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidated report'13'(No for branch wise)"); }
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F25126":
                    // Matl. Inward Checklist
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '0%' order by type1";
                    header_n = "Select Matl. Inward Type";
                    i0 = 1;
                    break;

                case "F25127":
                    // Matl. Outward Checklist
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '2%' order by type1";
                    header_n = "Select Mal. Outward Type";
                    i0 = 1;
                    break;

                case "F25128":
                    // Matl. Issue Checklist
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '3%' and type1<>'36' order by type1";
                    header_n = "Select Matl. Issue Type";
                    i0 = 1;
                    break;

                case "F25129":
                    // Matl. Return Checklist
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '1%' and type1<'15' order by type1";
                    header_n = "Select Matl. Return Type";
                    i0 = 1;
                    break;

                case "F25245A":
                    SQuery = "SELECT trim(A.BRANCHCD)||trim(A.tYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,B.INAME AS PRODUCT,A.ICODE ERPCODE,A.ENT_BY,TO_CHAr(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,TO_CHAr(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='16' AND A.VCHDATE " + xprdrange + " /*and a.store!='Y'*/ ORDER BY VDD DESC, A.VCHNUM DESC ";
                    if (co_cd == "SACL")
                        SQuery = "SELECT trim(A.BRANCHCD)||trim(A.tYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,B.INAME AS PRODUCT,A.ICODE ERPCODE,A.ENT_BY,TO_CHAr(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,TO_CHAr(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='15' AND A.VCHDATE " + xprdrange + " /*and a.store!='Y'*/ ORDER BY VDD DESC, A.VCHNUM DESC ";
                    header_n = "Select FG Entry";
                    i0 = 1;
                    break;

                case "F25245S":
                    SQuery = "SELECT trim(A.BRANCHCD)||trim(A.tYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,B.INAME AS PRODUCT,A.ICODE ERPCODE,A.ENT_BY,TO_CHAr(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,TO_CHAr(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHERW A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='40' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC, A.VCHNUM DESC ";
                    header_n = "Select Rcv Entry";
                    i0 = 1;
                    break;

                case "F25245R":
                case "F25245RA":
                    SQuery = "SELECT trim(A.BRANCHCD)||trim(A.tYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,B.INAME AS PRODUCT,A.ICODE ERPCODE,A.ENT_BY,TO_CHAr(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,TO_CHAr(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='10' AND A.VCHDATE " + xprdrange + " and substr(trim(a.icode),1,2) in ('01','02') ORDER BY VDD DESC, A.VCHNUM DESC ";
                    header_n = "Select Store Reutrn Entry";
                    i0 = 1;
                    break;

                case "F25247": // Rejection Summary PartyWise
                case "F25248": // Supplier Rejection Item Movement 
                case "F25138":
                case "F25260":
                case "F25261":
                case "F25263":
                case "F25265":
                case "F25265A":
                case "F25152":
                case "F25156":
                case "F25165":
                case "F25165A":
                case "F25266":
                case "F25159":
                case "F25160":
                case "F25162":
                case "F25162D":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F25162E":
                    fgen.Fn_open_dtbox("Select Date", frm_qstr);
                    break;
                case "F25132A":
                case "F15314A":
                case "F15314B":
                case "F25244C":
                    SQuery = "";
                    fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
                    break;
                case "F25245":
                case "F25169":
                case "F25135A":
                case "F76004":
                case "F58004":
                case "F25170":
                case "F25163":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F25119":
                    fgen.msg("-", "CMSG", "Do You Want to See Missing Reels in ERP Stock'13'(No for Missing in Phy. Verification)");
                    break;

                case "F25123":
                    fgen.msg("-", "CMSG", "Do You Want to see all reels'13'(No for only Phy. Verified Reels)");
                    break;
                case "F25130":
                    SQuery = "SELECT '1' AS FSTR,'PRINTING STORE' AS NAME,'1' AS CODE FROM DUAL UNION ALL SELECT '2' AS FSTR,'PIGMENT STORE' AS NAME,'2' AS CODE FROM DUAL UNION ALL SELECT '3' AS FSTR,'MIXING STORE' AS NAME,'3' AS CODE FROM DUAL";
                    break;
                case "F70125":
                    SQuery = "";
                    fgen.msg("-", "CMSG", "Are you sure, you want to run posting.");
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (i0 == 1) fgen.Fn_open_mseek(header_n, frm_qstr);
                else fgen.Fn_open_sseek(header_n, frm_qstr);
            }
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        col1 = "";
        val = hfhcid.Value.Trim();
        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";
            if (val == "F25126" || val == "F25127" || val == "F25128" || val == "F25129" || val == "F25130")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                if (val == "F25130") fgen.Fn_open_prddmp1("-", frm_qstr);
                else fgen.Fn_open_Act_itm_prd("-", frm_qstr);
            }
            switch (val)
            {
                case "F25245A":
                case "F25245R":
                case "F25245S":
                case "F25245RA":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "F25120":
                    if (value1 == "A")
                    {
                        mq1 = "Select a.icode as erpcode,b.iname as reelname,a.kclreelno as reel_no,a.stk as erp_stk,a.phy as phy_veri from (Select icode,kclreelno,sum(stk) as stk,sum(phy) as phy from (Select trim(a.icode) as icode,trim(a.kclreelno) as kclreelno,a.tot as stk,0 as phy from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + mbr + "' group by kclreelno,trim(icode)) group by trim(icode) ,kclreelno) a where a.tot>0 union all select trim(icode) as icode,trim(acode) as acode,0 as stk,num1 as phy from scratch where branchcd='" + mbr + "' and type='RL' and vchdate " + xprdrange + ") group by icode,kclreelno ) a,item b where trim(a.icode)=trim(b.icode) order by a.icode,a.kclreelno";
                        SQuery = "Select * from (" + mq1 + ") where erp_stk>0 and phy_veri=0";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Reel Stock Report", frm_qstr);
                    }
                    else
                    {
                        SQuery = "Select * from (" + mq1 + ") where phy_veri>0";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Phy. Verified Reels Report", frm_qstr);
                    }
                    break;
                case "F25162":
                    #region
                    value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    cond = " and 1=1";
                    if (value1 == "Y")
                        cond = " and (a.Qtyord)-(a.Soldqty)>0";
                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                    cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
                    yr_fld = year;

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)"; //this is old by sir
                    //SQuery = "select substr(a.Fstr,19,6)||substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4)||trim(a.acode)||trim(a.erp_code) as fstr,'-' as gSTR,C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,substr(a.Fstr,19,6) as MRR_No,substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4) as mrr_dt,chl_no,chl_dt,(case when billable='Y' then 'Billable' else 'Non-Billable' end) as billable from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode,max(billable) as billable,max(chl_no) as chl_no,max(chl_dt) as chl_dt from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode,trim(t_Deptt) as billable,NULL AS CHL_NO,null as CHL_DT from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode,null as billable,trim(Vchnum) as chl_no,to_Char(Vchdate,'dd/mm/yyyy') as chl_dt from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and refdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) " + cond + " order by B.Iname,trim(a.fstr)";//
                    //chnage by yogita on 16june21
                    SQuery = "select substr(a.Fstr,19,6)||substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4)||trim(a.acode)||trim(a.erp_code) as fstr,'-' as gSTR,C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,substr(a.Fstr,19,6) as MRR_No,substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4) as mrr_dt,chl_no,chl_dt,(case when billable='Y' then 'Billable' else 'Non-Billable' end) as billable from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode,max(billable) as billable,max(chl_no) as chl_no,max(chl_dt) as chl_dt from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode,trim(t_Deptt) as billable,NULL AS CHL_NO,null as CHL_DT from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(a.icode)||'-'||to_ChaR(a.refdate,'YYYYMMDD')||'-'||trim(a.REfnum)||'-'||trim(a.btchno) as fstr,trim(a.Icode) as ERP_code,0 as Qtyord,a.iqtyout as qtyord,0 as irate,a.acode,null as billable,trim(a.Vchnum) as chl_no,to_Char(a.Vchdate,'dd/mm/yyyy') as chl_dt from ivoucher a LEFT OUTER JOIN (select B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AS FSTR,X.FULL_INVNO,B.VCHNUM,B.VCHDATE FROM IVOUCHER B, sale X WHERE B.BRANCHCD||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode)=x.BRANCHCD||x.type||trim(x.vchnum)||to_char(x.vchdate,'dd/mm/yyyy')||trim(x.acode) AND B.BRANCHCD='00' AND B.TYPE='41' ) B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.FSTR    where a.branchcd='" + mbr + "' and a.type ='25' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and a.refdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(a.Acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) " + cond + " order by B.Iname,trim(a.fstr)";

                    fgen.drillQuery(0, SQuery, frm_qstr);
                    mq1 = "SELECT A.FSTR as gstr,A.FSTR, B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(REFNUM)||TO_CHAR(REFDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(REFNUM) AS VCHNUM,TO_CHAR(REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,0 AS MRR_QTY,IQTYOUT AS CHL_QTY,VCHNUM,TO_CHAr(VCHDATE,'DD/MM/YYYY') AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT";
                    fgen.drillQuery(1, mq1, frm_qstr);
                    fgen.Fn_DrillReport("Customer Job Work Register as on " + todt, frm_qstr);
                    cond = "";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Customer Job Work Register as on " + todt, frm_qstr);
                    #endregion
                    break;
                case "F25162E":
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                    cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
                    yr_fld = year;

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    SQuery = "select substr(a.Fstr,19,6)||substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4)||trim(a.acode)||trim(a.erp_code) as fstr,'-' as gSTR,C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,substr(a.Fstr,19,6) as MRR_No,substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4) as mrr_dt,chl_no,chl_dt,(case when billable='Y' then 'Billable' else 'Non-Billable' end) as billable, round(to_date('" + todt + "','dd/mm/yyyy')-  to_Date(substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4),'dd/mm/yyyy')) as pending_days from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode,max(billable) as billable,max(chl_no) as chl_no,max(chl_dt) as chl_dt from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode,trim(t_Deptt) as billable,NULL AS CHL_NO,null as CHL_DT from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode,null as billable,trim(Vchnum) as chl_no,to_Char(Vchdate,'dd/mm/yyyy') as chl_dt from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and refdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) and (a.Qtyord)-(a.Soldqty)!=0 and round(to_date('" + todt + "','dd/mm/yyyy')-  to_Date(substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4),'dd/mm/yyyy'))>" + value1.toDouble() + " order by pending_days DESC, B.Iname,trim(a.fstr)";

                    fgen.drillQuery(0, SQuery, frm_qstr);
                    fgen.drillQuery(1, "SELECT A.FSTR as gstr,A.FSTR, B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(REFNUM)||TO_CHAR(REFDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(REFNUM) AS VCHNUM,TO_CHAR(REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,0 AS MRR_QTY,IQTYOUT AS CHL_QTY,VCHNUM,TO_CHAr(VCHDATE,'DD/MM/YYYY') AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT", frm_qstr);
                    fgen.Fn_DrillReport("Customer Job Work Register as on " + todt, frm_qstr);
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

                case "F25119":
                case "F25123":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                    hf1.Value = value1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F70125":
                    updItemPosting();
                    break;
                case "F25162":
                    value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    cond = " and 1=1";
                    if (value1 == "Y")
                        cond = " and (a.Qtyord)-(a.Soldqty)>0";

                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                    cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
                    yr_fld = year;

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)"; //this is old by sir
                    //SQuery = "select substr(a.Fstr,19,6)||substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4)||trim(a.acode)||trim(a.erp_code) as fstr,'-' as gSTR,C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,substr(a.Fstr,19,6) as MRR_No,substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4) as mrr_dt,chl_no,chl_dt,(case when billable='Y' then 'Billable' else 'Non-Billable' end) as billable from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode,max(billable) as billable,max(chl_no) as chl_no,max(chl_dt) as chl_dt from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode,trim(t_Deptt) as billable,NULL AS CHL_NO,null as CHL_DT from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode,null as billable,trim(Vchnum) as chl_no,to_Char(Vchdate,'dd/mm/yyyy') as chl_dt from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and refdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) " + cond + " order by B.Iname,trim(a.fstr)";//

                    SQuery = "select substr(a.Fstr,19,6)||substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4)||trim(a.acode)||trim(a.erp_code) as fstr,'-' as gSTR,C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,substr(a.Fstr,19,6) as MRR_No,substr(a.Fstr,16,2)||'/'||substr(a.Fstr,14,2)||'/'||substr(a.Fstr,10,4) as mrr_dt,chl_no,chl_dt,(case when billable='Y' then 'Billable' else 'Non-Billable' end) as billable from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode,max(billable) as billable,max(chl_no) as chl_no,max(chl_dt) as chl_dt from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode,trim(t_Deptt) as billable,NULL AS CHL_NO,null as CHL_DT from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(a.icode)||'-'||to_ChaR(a.refdate,'YYYYMMDD')||'-'||trim(a.REfnum)||'-'||trim(a.btchno) as fstr,trim(a.Icode) as ERP_code,0 as Qtyord,a.iqtyout as qtyord,0 as irate,a.acode,null as billable,trim(a.Vchnum) as chl_no,to_Char(a.Vchdate,'dd/mm/yyyy') as chl_dt from ivoucher a LEFT OUTER JOIN (select B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AS FSTR,X.FULL_INVNO,B.VCHNUM,B.VCHDATE FROM IVOUCHER B, sale X WHERE B.BRANCHCD||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode)=x.BRANCHCD||x.type||trim(x.vchnum)||to_char(x.vchdate,'dd/mm/yyyy')||trim(x.acode) AND B.BRANCHCD='00' AND B.TYPE='41' ) B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.FSTR    where a.branchcd='" + mbr + "' and a.type ='25' and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and a.refdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(a.Acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) " + cond + " order by B.Iname,trim(a.fstr)";

                    fgen.drillQuery(0, SQuery, frm_qstr);
                    mq1 = "SELECT A.FSTR as gstr,A.FSTR, B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(REFNUM)||TO_CHAR(REFDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(REFNUM) AS VCHNUM,TO_CHAR(REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,0 AS MRR_QTY,IQTYOUT AS CHL_QTY,VCHNUM,TO_CHAr(VCHDATE,'DD/MM/YYYY') AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT";
                    fgen.drillQuery(1, mq1, frm_qstr);
                    fgen.Fn_DrillReport("Customer Job Work Register as on " + todt, frm_qstr);
                    cond = "";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Customer Job Work Register as on " + todt, frm_qstr);
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

            string xbstring = "", my_rep_head = "", s_code1 = "", s_code2 = "";

            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            else branch_Cd = "branchcd='" + mbr + "'";

            // COMMENTED BY MADHVI ON 9TH APR 2018 AS QUERIES MADE THROUGH REP CONFIG ARE GETTING THROUGH THIS FUNCTION "fgen.makeRepQuery"

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
                case "F25152":
                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select C.Aname as Vendor,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,trim(a.ERP_code) as ERP_code,Desc_,substr(trim(a.Fstr),28,2) as CHL_Type,substr(trim(a.Fstr),19,6) as CHL_No,substr(trim(a.Fstr),16,2)||'/'||substr(trim(a.Fstr),14,2)||'/'||substr(trim(a.Fstr),10,4) as CHL_Dt,a.irate as Prate,b.Cdrgno,trim(a.Fstr) as CHL_link,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode ,max(desc_) as Desc_ from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,acode,desc_ from rgpmst where branchcd='" + mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode,null as desc_ from ivoucher where branchcd='" + mbr + "' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Job Work Register for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25156":
                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "select C.Aname as Vendor,b.Iname as Item_Name,b.Cpartno as Part_no,sum(a.Qtyord) as RGP_Qty,sum(a.Soldqty) as Rcv_Qty,sum(a.Qtyord)-sum(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code  from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,acode from rgpmst where branchcd='" + mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by C.Aname,b.Iname,b.Cpartno,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) order by c.aname,B.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Job Work Register for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25162":
                    fgen.msg("-", "CMSG", "Do You want to see Pending data'13'(No for all)");
                    break;
                case "F25162E":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgen.Fn_ValueBox("Enter Days here to see pending list", frm_qstr);
                    break;
                case "F25162D":
                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (party_cd == "0") party_cd = "";
                    if (part_cd == "0") part_cd = "";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,trim(a.acode) as Ac_Code,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                    SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as MRR_Qty,(a.Soldqty) as Chl_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type ='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and Store!='R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type ='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";

                    SQuery = "SELECT B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(REFNUM)||TO_CHAR(REFDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(REFNUM) AS VCHNUM,TO_CHAR(REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,0 AS MRR_QTY,IQTYOUT AS CHL_QTY,VCHNUM,TO_CHAr(VCHDATE,'DD/MM/YYYY') AS CHL_DT FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='25' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT";

                    SQuery = "SELECT B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT,A.INVNO,A.INVDATE FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT,NULL AS INVNO,NULL AS invdate FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')||Trim(A.acode)||Trim(A.icode) AS FSTR,TRIM(A.REFNUM) AS VCHNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.aCODE) AS ACODE,TRIM(A.ICODe) AS ICODE,0 AS MRR_QTY,A.IQTYOUT AS CHL_QTY,A.VCHNUM,TO_CHAr(A.VCHDATE,'DD/MM/YYYY') AS CHL_DT,(CASE WHEN NVL(B.FULL_INVNO,'-')!='-' THEN B.FULL_INVNO ELSE B.VCHNUM END) as invno,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS invdate FROM IVOUCHER A LEFT OUTER JOIN (select B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AS FSTR,X.FULL_INVNO,B.VCHNUM,B.VCHDATE FROM IVOUCHER B, sale X WHERE B.BRANCHCD||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode)=x.BRANCHCD||x.type||trim(x.vchnum)||to_char(x.vchdate,'dd/mm/yyyy')||trim(x.acode) AND B.BRANCHCD='" + mbr + "' AND B.TYPE='41' ) B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.FSTR WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='25' and A.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and A.vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT,A.INVNO,A.INVDATE";
                    SQuery = "SELECT B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT,A.INVNO,A.INVDATE FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT,NULL AS INVNO,NULL AS invdate FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')||Trim(A.acode)||Trim(A.icode) AS FSTR,TRIM(A.REFNUM) AS VCHNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.aCODE) AS ACODE,TRIM(A.ICODe) AS ICODE,0 AS MRR_QTY,A.IQTYOUT AS CHL_QTY,A.VCHNUM,TO_CHAr(A.VCHDATE,'DD/MM/YYYY') AS CHL_DT,B.VCHNUM as invno,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS invdate FROM IVOUCHER A LEFT OUTER JOIN IVOUCHER B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AND B.TYPE='41' WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='25' and A.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and A.vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT,A.INVNO,A.INVDATE";
                    SQuery = "SELECT B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.BTCHNO AS CUST_CHL_NO,A.BTCHDT AS CUST_CHL_dT,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT,(Case when nvl(a.billable,'-')='N' then 'No Inv Req' else A.INVNO end) as INVNO,(Case when nvl(a.billable,'-')='N' then 'No Inv Req' else A.INVDATE end) as invdate,nvl(a.billable,'-') as billable FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT,NULL AS INVNO,NULL AS invdate,TRIM(REFNUM) BTCHNO,TO_cHAR(REFDATE,'DD/MM/YYYY') BTCHDT,trim(t_Deptt) as billable FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') UNION ALL SELECT TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')||Trim(A.acode)||Trim(A.icode) AS FSTR,TRIM(A.REFNUM) AS VCHNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.aCODE) AS ACODE,TRIM(A.ICODe) AS ICODE,0 AS MRR_QTY,A.IQTYOUT AS CHL_QTY,A.VCHNUM,TO_CHAr(A.VCHDATE,'DD/MM/YYYY') AS CHL_DT,(CASE WHEN NVL(B.FULL_INVNO,'-')!='-' THEN B.FULL_INVNO ELSE B.VCHNUM END) as invno,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS invdate,TRIM(A.BTCHNO) AS BTCHNO,A.BTCHDT,trim(t_deptt) as billable FROM IVOUCHER A LEFT OUTER JOIN (select B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AS FSTR,X.FULL_INVNO,B.VCHNUM,B.VCHDATE FROM IVOUCHER B, sale X WHERE B.BRANCHCD||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode)=x.BRANCHCD||x.type||trim(x.vchnum)||to_char(x.vchdate,'dd/mm/yyyy')||trim(x.acode) AND B.BRANCHCD='" + mbr + "' AND B.TYPE='41' ) B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.FSTR WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='25' and A.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and A.vchdate<=to_Date('" + todt + "','dd/mm/yyyy') ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT,A.INVNO,A.INVDATE,A.BTCHNO,A.BTCHDT,nvl(a.billable,'-')";//old

                    SQuery = "SELECT B.ANAME AS CUSTOMER,C.INAME AS ITEM_NAME,C.CPARTNO AS PART_NO,sum(A.MRR_QTY) AS MRR_qTY,sum(A.CHL_QTY) AS CHL_qTY,A.BTCHNO AS CUST_CHL_NO,A.BTCHDT AS CUST_CHL_dT,A.VCHNUM AS MRR_NO,A.VCHDATE AS MRR_dT,A.CHL_No,A.CHL_DT,(Case when nvl(a.billable,'-')='N' then 'No Inv Req' else A.INVNO end) as INVNO,(Case when nvl(a.billable,'-')='N' then 'No Inv Req' else A.INVDATE end) as invdate,nvl(a.billable,'-') as billable FROM (SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||Trim(acode)||Trim(icode) AS FSTR,TRIM(vCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(aCODE) AS ACODE,TRIM(ICODe) AS ICODE,IQTYIN AS MRR_QTY,0 AS CHL_QTY,NULL AS CHL_NO,NULL AS CHL_DT,NULL AS INVNO,NULL AS invdate,TRIM(REFNUM) BTCHNO,TO_cHAR(REFDATE,'DD/MM/YYYY') BTCHDT,trim(t_Deptt) as billable FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='08' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<=to_Date('" + todt + "','dd/mm/yyyy') and store<>'R' and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' UNION ALL SELECT TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')||Trim(A.acode)||Trim(A.icode) AS FSTR,TRIM(A.REFNUM) AS VCHNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.aCODE) AS ACODE,TRIM(A.ICODe) AS ICODE,0 AS MRR_QTY,A.IQTYOUT AS CHL_QTY,A.VCHNUM,TO_CHAr(A.VCHDATE,'DD/MM/YYYY') AS CHL_DT,(CASE WHEN NVL(B.FULL_INVNO,'-')!='-' THEN B.FULL_INVNO ELSE B.VCHNUM END) as invno,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS invdate,TRIM(A.BTCHNO) AS BTCHNO,A.BTCHDT,trim(t_deptt) as billable FROM IVOUCHER A LEFT OUTER JOIN (select B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AS FSTR,X.FULL_INVNO,B.VCHNUM,B.VCHDATE FROM IVOUCHER B, sale X WHERE B.BRANCHCD||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode)=x.BRANCHCD||x.type||trim(x.vchnum)||to_char(x.vchdate,'dd/mm/yyyy')||trim(x.acode) AND B.BRANCHCD='" + mbr + "' AND B.TYPE='41' ) B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.FSTR WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='25' and A.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and A.refdate<=to_Date('" + todt + "','dd/mm/yyyy') and trim(a.Acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODe) AND TRIM(a.ICODE)=TRIM(C.ICODE) and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' group by A.FSTR,A.FSTR, B.ANAME,C.INAME,C.CPARTNO,A.VCHNUM,A.VCHDATE,A.CHL_No,A.CHL_DT,A.INVNO,A.INVDATE,A.BTCHNO,A.BTCHDT,nvl(a.billable,'-')";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Job Work Detail as on " + todt, frm_qstr);
                    break;

                case "F25165":
                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    // SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,sum(a.Qtyord) as Rcvd_Qty,sum(a.Soldqty) as Sent_Qty,sum(a.Qtyord)-sum(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.acode) as Ac_Code  from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(Refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('08') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and store<>'R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('25') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and vchdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by C.Aname,b.Iname,b.Cpartno,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code),trim(a.acode) order by c.aname,B.Iname";

                    SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,sum(a.Qtyord) as Rcvd_Qty,sum(a.Soldqty) as Sent_Qty,sum(a.Qtyord)-sum(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.acode) as Ac_Code  from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(Refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('08') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and store<>'R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('25') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and refdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by C.Aname,b.Iname,b.Cpartno,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code),trim(a.acode) order by c.aname,B.Iname";
                    //SQuery = "select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,sum(a.Qtyord) as Rcvd_Qty,sum(a.Soldqty) as Sent_Qty,sum(a.Qtyord)-sum(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.acode) as Ac_Code  from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(Refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('08') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and store<>'R' union all SELECT trim(a.icode)||'-'||to_ChaR(a.refdate,'YYYYMMDD')||'-'||trim(a.REfnum)||'-'||trim(a.btchno) as fstr,trim(a.Icode) as ERP_code,0 as Qtyord,a.iqtyout as qtyord,0 as irate,a.acode from ivoucher a LEFT OUTER JOIN (select B.BRANCHCD||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)||trim(b.icode)||b.iqtyout AS FSTR,X.FULL_INVNO,B.VCHNUM,B.VCHDATE FROM IVOUCHER B, sale X WHERE B.BRANCHCD||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.acode)=x.BRANCHCD||x.type||trim(x.vchnum)||to_char(x.vchdate,'dd/mm/yyyy')||trim(x.acode) AND B.BRANCHCD='" + mbr + "' AND B.TYPE='41' ) B ON A.BRANCHCD||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||trim(a.icode)||a.IQTYOUT=B.FSTR  where a.branchcd='" + mbr + "' and a.type in ('25') and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and a.refdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(a.Acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  group by C.Aname,b.Iname,b.Cpartno,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code),trim(a.acode) order by c.aname,B.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Job Work Summary for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25165A":
                    // Job work report
                    //SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joincond + "  order by " + sortfld;
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    string mprd1 = " between to_date('01/04/2021','dd/mm/yyyy')-1 and to_Date('" + fromdt + "','dd/mm/yyyy')-1 ";
                    string mprd2 = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy') ";

                    mq0 = "select c.aname as Customer_Name,b.iname as Item_Name,b.unit as uom,sum(a.opening) as Opening,sum(a.cdr) as Qty_Rcvd,sum(a.ccr) as Qty_Sent,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Qty_balance,trim(a.icode) as Icode,(case when sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0 then 'Pending' else 'Cleared' end) as chk_Stat,c.acode,b.cpartno from (Select '-' as acode,'-' as store,icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,0 as inqa from ITEMBAL where 1=2 union all  ";
                    mq1 = "select acode,store,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,2) in('08','25') and vchdate " + mprd1 + "  and acode like '" + party_cd + "%'  GROUP BY acode,store,ICODE union all ";
                    mq3 = "select acode,store,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,2) in('08','25') and vchdate " + mprd2 + "  and acode like '" + party_cd + "%'  GROUP BY acode,store,ICODE )a,item b,(select distinct a.acode,b.aname from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.type in ('08','25'))c where trim(A.icode)=trim(B.icode) and trim(A.acode)=trim(c.acode) group by b.iname,b.cpartno,trim(a.icode),c.aname,c.acode,b.unit having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by c.acode,b.iname";

                    SQuery = "SELECT 'Total' as customer_name,'-' as item_name,'-' as uom,sum(opening) as opening,sum(Qty_Rcvd) as Qty_Rcvd,sum(Qty_Sent) as Qty_Sent,sum(Qty_balance) as Qty_balance,'-' as icode,'-' as chk_Stat,'-' as acode,'-' as cpartno from (" + mq0 + mq1 + mq3 + ") UNION ALL SELECT * FROM (" + mq0 + mq1 + mq3 + ")";
                    //"select C.Aname as Customer,b.Iname as Item_Name,b.Cpartno as Part_no,sum(a.Qtyord) as RGP_Qty,sum(a.Soldqty) as Rcv_Qty,sum(a.Qtyord)-sum(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.acode) as Ac_Code  from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(Refnum) as fstr,trim(Icode) as ERP_code,iqtyin as Qtyord,0 as Soldqty,irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('08') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' and store<>'R' union all SELECT trim(icode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(REfnum)||'-'||trim(btchno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('25') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and vchdate<= to_date('" + todt + "','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode) group by C.Aname,b.Iname,b.Cpartno,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code),trim(a.acode) order by c.aname,B.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Job Work Stock Status for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25260":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    string icode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_III");
                    string jobno = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IV");
                    string jobno1 = "";
                    if (icode.Length < 3) icode = "";
                    else icode = " AND TRIM(ICODE)='" + icode + "'";
                    if (jobno.Length < 3) jobno = "";
                    else
                    {
                        jobno1 = " AND TRIM(jobno)||to_Char(jobdt,'dd/mm/yyyy')='" + jobno + "'";
                        jobno = " AND TRIM(invno)||to_Char(invdate,'dd/mm/yyyy')='" + jobno + "'";
                    }
                    SQuery = "select b.Name as Deptt_Name,a.type,a.vchnum as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as req_dt,c.Iname,sum(a.iqty_chl) as Req_Qty,sum(a.issued) as Issue_Qty,sum(a.iqty_chl)-sum(a.issued) as Pending_Qty,c.Unit,c.Cpartno,trim(A.icode) as erp_Code,max(a.ent_by) As Request_by,round(sysdate-a.vchdate,0) as Pend_Days,trim(a.acode) as Dept_Cd,trim(A.stage) as WIP_Stg  from (SELECT type,acode,stage,vchnum,vchdate,icode,ent_by,'-' as jobno,vchdate as jobdt,req_qty as iqty_chl,0 as issued from wb_iss_req where branchcd='" + mbr + "' and type like '3%' and vchdate " + xprdrange + " and nvl(closed,'-')!='Y' and acode like '" + party_cd + "%' and substr(icode,1,2) like '" + part_cd + "%' " + icode + " " + jobno1 + " union all SELECT type,acode,stage,refnum,refdate,icode,null as entby,'-' as invno,vchdate,0 as iqty_chl,iqtyout as issued from ivoucher where branchcd='" + mbr + "' and type like '3%' and vchdate " + xprdrange + " and substr(icode,1,2) like '" + part_cd + "%' " + icode + " " + jobno + ")a,type b,item c where b.id='M' and trim(A.acode)=trim(B.type1) and trim(A.icode)=trim(c.icode) group by trim(A.icode),c.iname,c.unit,c.cpartno,a.type,to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,b.Name,trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate,trim(A.stage) having sum(a.iqty_chl)-sum(a.issued)>0 order by a.vchdate,a.type,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Checklist of Issue Requests Pending Store Issue for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25261":
                    SQuery = "select b.Name as Deptt_Name,a.type,a.vchnum as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as req_dt,c.Iname,sum(a.iqty_chl) as Req_Qty,sum(a.issued) as Return_Qty,sum(a.iqty_chl)-sum(a.issued) as Pending_Qty,c.Unit,c.Cpartno,max(a.ent_by) As Request_by,round(sysdate-a.vchdate,0) as Pend_Days,trim(a.acode) as Dept_Cd,trim(A.stage) as WIP_Stg  from (SELECT type,acode,stage,vchnum,vchdate,icode,ent_by,'-' as jobno,vchdate as jobdt,req_qty as iqty_chl,0 as issued from wb_iss_req where branchcd='" + mbr + "' and type like '1%' and vchdate " + xprdrange + " and nvl(closed,'-')!='Y' union all SELECT type,acode,stage,refnum,refdate,icode,null as entby,'-' as invno,vchdate,0 as iqty_chl,iqtyin as issued from ivoucher where branchcd='" + mbr + "' and type like '1%' and type<'15' and vchdate " + xprdrange + ")a,type b,item c where b.id='M' and trim(A.acode)=trim(B.type1) and trim(A.icode)=trim(c.icode) group by c.iname,c.unit,c.cpartno,a.type,to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,b.Name,trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate,trim(A.stage) having sum(a.iqty_chl)-sum(a.issued)>0 order by a.vchdate,a.type,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Checklist of Return Requests Pending Store Return for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25263":
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F25126", "branchcd='" + mbr + "'", "a.type like '0%' and a.store = 'N' and trim(nvl(a.pname,'-')) = '-' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ", "" + xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("MRR Entry Pending Q.A Action for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25265":
                case "F25265A":
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, "F25126", "branchcd='" + mbr + "'", "a.type like '0%' and a.store = 'Y' and trim(nvl(a.finvno,'-')) = '-' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ", "" + xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("MRR Entry Pending Accounts Action for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25194":
                    SQuery = "select distinct 'GEMRR' as xtype,type,vchnum from ivoucherp where branchcd='" + mbr + "' and type ='00' and vchdate " + xprdrange + " union all select distinct 'ALLMRR' as xtype,type,vchnum from ivoucher where branchcd='" + mbr + "' and type like '0%' and vchdate " + xprdrange + " and store<>'R' union all select distinct 'QCMRR' as xtype,type,vchnum from ivoucher where branchcd='" + mbr + "' and type like '0%' and vchdate " + xprdrange + " and inspected='Y' union all select distinct 'FINMRR' as xtype,type,vchnum from ivoucher where branchcd='" + mbr + "' and type like '0%' and vchdate " + xprdrange + " and length(Trim(finvno))>2";
                    SQuery = "select type,decode(xtype,'GEMRR',count(Vchnum),0) as GE_MRR,decode(xtype,'ALLMRR',count(Vchnum),0) as all_MRR,decode(xtype,'QCMRR',count(Vchnum),0) as QC_MRR,decode(xtype,'FINMRR',count(Vchnum),0) as FIN_MRR from (" + SQuery + ") group by xtype,type";
                    SQuery = "select nvl(b.Name,'G.E.') as Name,sum(a.GE_MRR) as GE_DONE,sum(a.all_MRR) as MRR_Made,sum(a.QC_MRR)as QC_Done,sum(a.FIN_MRR) as Vch_Made,a.type as Type_of_MRR from (" + SQuery + ") a left outer join (Select type1,name from type where id='M' and substr(type1,1,1)='0') b on a.type=b.type1 group by b.name,a.type order by a.type";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Stores Rcpt Transaction Tracking for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25138":
                    SQuery = "SELECT A.VCHNUM AS GE_Number,to_Char(a.VCHDATE,'dd/mm/yyyy') as Ge_Date,B.Aname as Supplier,b.addr1 as Address,c.Iname,c.Cpartno,a.iqty_chl as GE_Qty,c.unit,a.Invno as Inv_no,A.Refnum as Chl_no,b.Staten,a.prnum,to_Char(A.Invdate,'dd/mm/yyyy') as Inv_Dt,a.Icode,a.Acode,to_Char(a.vchdate,'yyyymmdd') as GE_Dt FROM IVOUCHERP a, famst b ,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and TRIM(A.iCODE)=TRIM(c.iCODE)  and a.BRANCHCD='" + mbr + "' AND a.VCHDATE  " + xprdrange + " AND a.TYPE='00'  AND (a.VCHNUM||to_char(a.vchdate,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select distinct a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC from ivoucherp a  where branchcd='" + mbr + "' and a.VCHDATE   " + xprdrange + " AND a.type='00' and a.vchnum<>'000000' UNION ALL select distinct a.GENUM||to_char(a.gedate,'yyyymm') as genum,a.type,1 AS ABC from ivoucher a where branchcd='" + mbr + "' and substr(a.type,1,1)='0' and a.VCHDATE  " + xprdrange + " AND a.vchnum<>'000000' ) X GROUP BY X.VCHNUM) WHERE CNT=1) order by to_Char(a.vchdate,'yyyymmdd') desc,A.VCHNUM desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Checklist of Gate Entry Pending MRR for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25126":
                    // Matl. Inward Checklist
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Matl. Inward Checklist of (" + hfcode.Value + ") for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25127":
                    // Matl. Outward Checklist
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Matl. Outward Checklist of (" + hfcode.Value + ") for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25128":
                    // Matl. Issue Checklist
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Matl. Issue Checklist of (" + hfcode.Value + ") for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25129":
                    // Matl. Return Checklist
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type in (" + hfcode.Value + ") and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Matl. Return Checklist of (" + hfcode.Value + ") for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25247": // ON 14 MAY 2018 BY MADHVI
                    // Rejection Summary PartyWise
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    //   SQuery = "SELECT NVL(B.ANAME,'-') AS NAME,COUNT(A.ICODE) AS TIMES_RCVD,SUM(A.IQTYIN+NVL(A.REJ_RW,0)) AS TOTAL_INWARD,SUM(A.REJ_RW) AS TOTAL_REJ,ROUND((SUM(A.REJ_RW)/(SUM(A.IQTYIN+NVL(A.REJ_RW,0))))*100 ,2) AS REJ_PER,A.ACODE FROM IVOUCHER A LEFT OUTER JOIN FAMST B ON A.ACODE=B.ACODE WHERE A." + branch_Cd + " AND SUBSTR(A.TYPE,1,1)='0' AND A.VCHDATE " + xprdrange + " AND A.STORE<>'R' AND A.ACODE LIKE '" + party_cd + "%' GROUP BY B.ANAME,A.ACODE ORDER BY B.ANAME"; //original qry
                    SQuery = "SELECT NVL(B.ANAME,'-') AS NAME,COUNT(A.ICODE) AS TIMES_RCVD,SUM(NVL(A.IQTYIN,0)+NVL(A.REJ_RW,0)) AS TOTAL_INWARD,SUM(NVL(A.REJ_RW,0)) AS TOTAL_REJ,ROUND((SUM(NVL(A.REJ_RW,0))/(SUM(NVL(A.IQTYIN,0)+NVL(A.REJ_RW,0))))*100 ,2) AS REJ_PER,trim(A.ACODE) as acode FROM IVOUCHER A LEFT OUTER JOIN FAMST B ON TRIM(A.ACODE)=TRIM(B.ACODE) WHERE A." + branch_Cd + " AND SUBSTR(A.TYPE,1,1)='0' AND A.VCHDATE " + xprdrange + " AND A.STORE<>'R' AND trim(A.ACODE) LIKE '" + party_cd + "%' GROUP BY NVL(B.ANAME,'-'),TRIM(A.ACODE) ORDER BY NAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Rejection Summary PartyWise for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25248": // ON 14 MAY 2018 BY MADHVI
                    // Supplier Rejection Item Movement 
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    // SQuery = "SELECT A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,F.ANAME AS SUPPLIER,A.ICODE,I.INAME AS ITEM,I.CPARTNO,I.UNIT,A.IQTYIN,A.IQTYOUT,A.IRATE,A.DESC_,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT FROM IVOUCHER A,ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A." + branch_Cd + " AND (A.TYPE LIKE '0%' OR A.TYPE ='47') AND A.VCHDATE " + xprdrange + " AND A.ACODE LIKE '" + party_cd + "%' AND A.ICODE LIKE '" + part_cd + "%' AND A.STORE='R' ORDER BY F.ANAME,I.INAME,A.VCHNUM"; //original qry
                    SQuery = "SELECT TRIM(A.TYPE) AS TYPE,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(F.ANAME) AS SUPPLIER,TRIM(A.ICODE) AS ICODE,TRIM(I.INAME) AS ITEM,TRIM(I.CPARTNO) AS CPARTNO,TRIM(I.UNIT) AS UNIT,NVL(A.IQTYIN,0) AS IQTYIN,NVL(A.IQTYOUT,0) AS IQTYOUT,NVL(A.IRATE,0) AS IRATE,TRIM(A.DESC_) AS DESC_,TRIM(A.INVNO) AS INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,TRIM(A.ENT_BY) AS ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT FROM IVOUCHER A,ITEM I ,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A." + branch_Cd + " AND (A.TYPE LIKE '0%' OR A.TYPE ='47') AND A.VCHDATE " + xprdrange + " AND trim(A.ACODE) LIKE '" + party_cd + "%' AND trim(A.ICODE) LIKE '" + part_cd + "%' AND A.STORE='R' ORDER BY SUPPLIER,ITEM,VCHNUM";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Supplier Rejection Item Movement for the Period " + fromdt + " to " + todt, frm_qstr);
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

                case "F25245":
                    // SQuery = fgen.makeRepQuery(frm_qstr, co_cd, frm_formID, mbr, "type like '0%'", xprdrange);
                    header_n = "Location Wise Report";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    xprdrange1 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    //SQuery = "select substr(a.icode,1,2) as maingrp,a.icode,trim(b.iname) as iname,b.cpartno,b.unit,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,b.cdrgno,b.iweight,b.tarrifno,b.imin,b.hscode,b.abc_class,b.deac_by,B.BINNO AS BINNO1 from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 and substr(trim(icode),1,2) like '" + party_cd + "%' union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='Y' and substr(trim(icode),1,2) like '" + party_cd + "%' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='Y' and substr(trim(icode),1,2) like '" + party_cd + "%' GROUP BY trim(icode) ,branchcd ) a,item b where trim(a.icode)=trim(b.icode) GROUP BY substr(a.icode,1,2),a.icode,trim(b.iname),b.cpartno,b.unit,b.cdrgno,b.iweight,b.tarrifno,b.imin,b.hscode,b.abc_class,b.deac_by,B.BINNO having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    dt.Columns.Add("BINNO", typeof(string));
                    //}

                    //mq3 = "select distinct trim(icode) as icode,binno from itembal where branchcd='" + mbr + "'";
                    //dt1 = new DataTable(); ;
                    //dt1 = fgen.getdata(frm_qstr, co_cd, mq3);

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    if (dt1.Rows.Count > 0)
                    //    {
                    //        dt.Rows[i]["binno"] = fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "binno");
                    //        if (dt.Rows[i]["binno"].ToString().Trim().Length <= 1)
                    //        {
                    //            dt.Rows[i]["binno"] = dt.Rows[i]["binno1"].ToString().Trim();
                    //        }
                    //    }
                    //}
                    //if (dt.Rows.Count > 0)
                    //{
                    //    dt.Columns.Remove("binno1");
                    //}
                    mq0 = "";
                    SQuery = "SELECT A.ICODE  ,A.BATCHNO,A.TOT,replace(nvl(A.RLOCN,'-'),'-','-') as RLOCN ,B.INAME FROM( select batchno,sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where BRANCHCD='" + mbr + "' and vchdate " + xprdrange + " group by icode,trim(kclreelno),TRIM(Coreelno),trim(acode) union all select  DISTINCT  trim(kclreelno) as Batchno,0 as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where branchcd='" + mbr + "' and vchdate " + xprdrange + " group by trim(kclreelno),trim(icode)) group by icode,batchno ) a ,item b WHERE trim(a.icode)=trim(b.icode) and a.TOT>0 " + mq0 + " order by a.batchno,a.rlocn ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    Session["send_dt"] = null;
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25246P":
                    fgen.drillQuery(0, "SELECT FSTR,'-' AS GSTR,SUM(IQTYOUT) AS PROD,SUM(IQTYIN) AS RCV,SUM(qtymrcv) AS MANUALRCV,SUM(IQTYOUT-(IQTYIN+qtymrcv)) AS BAL_QTY FROM (SELECT BRANCHCD||TYPE AS FSTR,iqtyin as IQTYOUT,0 AS IQTYIN,0 as qtymrcv FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16' AND VCHDATE " + xprdrange + " UNION ALL SELECT BRANCHCD||'16' AS FIR,0 AS IQTYOUT,IQTYIN,0 as qtymrcv FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE in ('17') AND VCHDATE " + xprdrange + " AND STORE='Y' UNION ALL SELECT BRANCHCD||'16' AS FIR,0 AS IQTYOUT,0 as IQTYIN,IQTYIN as qtymrcv FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE in ('16') AND VCHDATE " + xprdrange + " AND STORE='Y') GROUP BY FSTR", frm_qstr);
                    fgen.drillQuery(1, "SELECT a.ICODE as  FSTR,a.fstr AS GSTR,a.icode as erpcode,b.iname as product,b.cpartno as partno,b.unit,SUM(a.IQTYOUT) AS PROD,SUM(a.IQTYIN) AS RCV,sum(a.MANUALRCV) as MANUALRCV,SUM(a.IQTYOUT-(a.IQTYIN+a.MANUALRCV)) AS BAL_QTY FROM (SELECT BRANCHCD||TYPE AS FSTR,to_char(VCHDATE,'DD/mm/yyyy') as vchd,trim(icodE) as icode,iqtyin as IQTYOUT,0 AS IQTYIN,0 AS MANUALRCV FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16' AND VCHDATE " + xprdrange + " AND STORE!='Y' UNION ALL SELECT BRANCHCD||TYPE AS FSTR,to_char(VCHDATE,'DD/mm/yyyy') as vchd,trim(icodE) as icode,iqtyin as IQTYOUT,0 AS IQTYIN,0 AS MANUALRCV FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE='16' AND VCHDATE " + xprdrange + " AND STORE='Y' UNION ALL SELECT BRANCHCD||'16' AS FIR,to_char(VCHDATE,'DD/mm/yyyy') as vchd,trim(icodE) as icode,0 AS IQTYOUT,IQTYIN,0 AS MANUALRCV FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE in ('17') AND VCHDATE " + xprdrange + " AND STORE='Y' UNION ALL SELECT BRANCHCD||'16' AS FIR,to_char(VCHDATE,'DD/mm/yyyy') as vchd,trim(icodE) as icode,0 AS IQTYOUT,0 as IQTYIN,IQTYIN AS MANUALRCV FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE in ('16') AND VCHDATE " + xprdrange + " AND STORE='Y' ) a,item b where trim(a.icode)=trim(B.icode) group by a.vchd,a.fstr,a.icode,b.iname,b.cpartno,b.unit order by a.vchd", frm_qstr);
                    fgen.drillQuery(2, "select '-' as  FSTR,a.icode AS GSTR,a.vchnum as entryno,a.vchdate as entrydt,b.iname as product,b.cpartno as partno,a.iqtyin as prod,a.manualrcv,a.iqtyout as rcvd from (select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(icode) as icode,iqtyin,0 as iqtyout,0 as manualrcv,ent_by,ent_dt from ivoucher where branchcd='" + mbr + "' and type='16' and vchdate " + xprdrange + " and store!='Y' UNION ALL select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(icode) as icode,iqtyin,0 as iqtyout,0 as manualrcv,ent_by,ent_dt from ivoucher where branchcd='" + mbr + "' and type='16' and vchdate " + xprdrange + " and store='Y' union all select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(icode) as icode,0 ,iqtyin,0 as manualrcv ,pname,ent_dt from ivoucher where branchcd='" + mbr + "' and type in ('17') and vchdate " + xprdrange + " and store='Y'  union all select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(icode) as icode,0 , 0 as iqtyin,iqtyin as manualrcv ,pname,ent_dt from ivoucher where branchcd='" + mbr + "' and type in ('16') and vchdate " + xprdrange + " and store='Y') a,item b where trim(a.icode)=trim(b.icode) order by a.vchnum,a.icode", frm_qstr);
                    fgen.Fn_DrillReport("Production vs Rcvd Qty for the period " + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25149B": ///8.8.18
                    // SQuery = "select pvno,pvdate,recv_date,a.icode as Item_code,b.iname as Item_name,a.qc_done,a.recd_bond,a.bal from (select vchnum as pvno,max(vchdate) as pvdate,max(recv_date) as recv_date,icode ,sum(iqtyin) as qc_done,sum(iqtyout) as recd_bond ,sum(manualrcv) as manualrcv,(sum(iqtyin)-(sum(iqtyout)+sum(manualrcv))) as bal from (select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store!='Y' UNION ALL select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store='Y' union all select vchnum,null as vchdate,to_char(vchdate,'dd/mm/yyyy') as recv_date,trim(icode) as icode,0 ,iqtyin,0 as manualrcv from ivoucher where " + branch_Cd + " and type in ('17') and vchdate " + xprdrange + " and store='Y' union all select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,0 , 0 as iqtyin,iqtyin as manualrcv from ivoucher where " + branch_Cd + " and type in ('16') and vchdate " + xprdrange + " and store='Y' ) group by vchnum,icode having (sum(iqtyin)-(sum(iqtyout)+sum(manualrcv)))>0) a,item b where trim(a.icodE)=trim(B.icode) order by a.icode";
                    // SQuery = "select pvno,pvdate,recv_date,a.icode as Item_code,trim(b.iname) as Item_name,a.qc_done,a.recd_bond,a.bal from (select vchnum as pvno,max(vchdate) as pvdate,max(recv_date) as recv_date,icode ,sum(iqtyin) as qc_done,sum(iqtyout) as recd_bond ,sum(manualrcv) as manualrcv,(sum(iqtyin)-(sum(iqtyout)+sum(manualrcv))) as bal from (select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate "+xprdrange+" and store!='Y' UNION ALL select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where "+branch_Cd+"  and type='16' and vchdate  "+xprdrange+" and store='Y' union all select vchnum,null as vchdate,to_char(vchdate,'dd/mm/yyyy') as recv_date,trim(icode) as icode,0 ,nvl(iqtyin,0) as iqtyin,0 as manualrcv from ivoucher where "+branch_Cd+" and type in ('17') and vchdate "+xprdrange+" and store='Y' union all select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,0 , 0 as iqtyin,nvl(iqtyin,0) as manualrcv from ivoucher where "+branch_Cd+" and type in ('16') and vchdate "+xprdrange+" and store='Y' ) group by vchnum,icode having (sum(iqtyin)-(sum(iqtyout)+sum(manualrcv)))>0) a,item b where trim(a.icodE)=trim(B.icode) order by a.icode"; //real qry
                    SQuery = "select pvno,pvdate,recv_date,trim(a.icode) as Item_code,trim(b.iname) as Item_name,a.qc_done,a.recd_bond,a.bal from (select vchnum as pvno,max(vchdate) as pvdate,max(recv_date) as recv_date,icode ,sum(nvl(iqtyin,0)) as qc_done,sum(nvl(iqtyout,0)) as recd_bond ,sum(nvl(manualrcv,0)) as manualrcv,(sum(nvl(iqtyin,0))-(sum(nvl(iqtyout,0))+sum(nvl(manualrcv,0)))) as bal from (select trim(vchnum) as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store!='Y' UNION ALL select trim(vchnum) as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store='Y' union all select trim(vchnum) as vchnum,null as vchdate,to_char(vchdate,'dd/mm/yyyy') as recv_date,trim(icode) as icode,0 as iqtyin,nvl(iqtyin,0) as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type in ('17') and vchdate " + xprdrange + " and store='Y' union all select trim(vchnum) as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,0 as iqtyin, 0 as iqtyout,nvl(iqtyin,0) as manualrcv from ivoucher where " + branch_Cd + " and type in ('16') and vchdate " + xprdrange + " and store='Y' ) group by vchnum,icode having (sum(iqtyin)-(sum(iqtyout)+sum(manualrcv)))>0) a,item b where trim(a.icodE)=trim(B.icode) order by a.icode";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Pending for Bonded Store Wise";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25149S"://8.8.18
                    //SQuery = "select a.icode as Item_code,b.iname as Item_name,a.iqtyin as qc_done,a.iqtyout as recd_bond, a.bal as balance from (select icode,sum(iqtyin) as iqtyin,sum(iqtyout) as iqtyout,sum(manualrcv) as manualrcv,sum(bal) as bal from (select vchnum,max(vchdate) as vchdate,max(recv_date) as recv_date,icode,sum(iqtyin) as iqtyin,sum(iqtyout) as iqtyout ,sum(manualrcv) as manualrcv,(sum(iqtyin)-(sum(iqtyout)+sum(manualrcv))) as bal from (select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store!='Y' UNION ALL select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store='Y' union all select vchnum,null as vchdate,to_char(vchdate,'dd/mm/yyyy') as recv_date,trim(icode) as icode,0 ,iqtyin,0 as manualrcv from ivoucher where " + branch_Cd + " and type in ('17') and vchdate " + xprdrange + " and store='Y'  union all select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,0 , 0 as iqtyin,iqtyin as manualrcv from ivoucher where " + branch_Cd + " and type in ('16') and vchdate " + xprdrange + " and store='Y' ) group by vchnum,icode having (sum(iqtyin)-(sum(iqtyout)+sum(manualrcv)))>0) group by icode ) a,item b where trim(a.icodE)=trim(B.icode) order by a.icode";
                    SQuery = "select a.icode as Item_code,b.iname as Item_name,a.iqtyin as qc_done,a.iqtyout as recd_bond, a.bal as balance from (select icode,sum(iqtyin) as iqtyin,sum(iqtyout) as iqtyout,sum(manualrcv) as manualrcv,sum(bal) as bal from (select vchnum,max(vchdate) as vchdate,max(recv_date) as recv_date,icode,sum(iqtyin) as iqtyin,sum(iqtyout) as iqtyout ,sum(manualrcv) as manualrcv,(sum(iqtyin)-(sum(iqtyout)+sum(manualrcv))) as bal from (select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store!='Y' UNION ALL select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,nvl(iqtyin,0) as iqtyin,0 as iqtyout,0 as manualrcv from ivoucher where " + branch_Cd + " and type='16' and vchdate " + xprdrange + " and store='Y' union all select vchnum,null as vchdate,to_char(vchdate,'dd/mm/yyyy') as recv_date,trim(icode) as icode,0 ,nvl(iqtyin,0) as iqtyin,0 as manualrcv from ivoucher where " + branch_Cd + " and type in ('17') and vchdate " + xprdrange + " and store='Y'  union all select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,null as recv_date,trim(icode) as icode,0 , 0 as iqtyin,nvl(iqtyin,0) as manualrcv from ivoucher where " + branch_Cd + " and type in ('16') and vchdate " + xprdrange + " and store='Y' ) group by vchnum,icode having (sum(iqtyin)-(sum(iqtyout)+sum(manualrcv)))>0) group by icode ) a,item b where trim(a.icodE)=trim(B.icode) order by a.icode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Pending for Bonded Summary";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25150":///////8.8.18
                    SQuery = "SELECT A.VCHNUM AS PV_NO ,A.VCHDATE AS PV_DATE ,trim(A.ICODE) AS ITEM_CODE ,trim(B.INAME) AS PRODUCT, SUM(A.PVQTY) AS PVQTY, SUM(A.QCDONE) AS QCDONE,SUM(A.PVQTY)-SUM(A.QCDONE) AS PENDING_QC ,A.ENT_BY,A.ENT_DT FROM (SELECT ICODE,vchnum,TO_CHAR(VCHDATE,'YYYYmmdd') AS Vdd,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ENT_BY,TO_CHAR(ENT_DT,'DD/MM/YYYY') AS ENT_DT ,nvl(IQTYIN,0) AS PVQTY, 0 AS QCDONE FROM IVOUCHER  WHERE " + branch_Cd + " AND TYPE ='16' AND VCHDATE  " + xprdrange + " UNION ALL SELECT ICODE,vchnum,TO_CHAR(VCHDATE,'YYYYmmdd') AS Vdd,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ENT_BY,TO_CHAR(ENT_DT,'DD/MM/YYYY') AS ENT_DT,0 AS PVQTY,nvl(IQTYIN,0) AS QCDONE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE = '16' and inspected='Y' AND VCHDATE " + xprdrange + ") A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.ENT_BY,A.ENT_DT,trim(A.ICODE),trim(B.INAME),A.VCHNUM,A.VCHDATE,a.vdd  HAVING SUM(A.PVQTY)-SUM(A.QCDONE)>0 ORDER By vdd desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Pending for Quality Voucher Wise";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25150S"://////8/8/18
                    SQuery = "SELECT  TRIM(A.ICODE) AS ITEM_CODE,TRIM(B.INAME) AS ITEM_NAME, SUM(A.PVQTY) AS PVQTY, SUM(A.QCDONE) AS QCDONE,SUM(A.PVQTY)-SUM(A.QCDONE) AS PENDING_QC FROM (SELECT ICODE, NVL(IQTYIN,0) AS PVQTY, 0 AS QCDONE FROM IVOUCHER  WHERE " + branch_Cd + " AND TYPE ='16' AND VCHDATE " + xprdrange + " UNION ALL SELECT ICODE,0 AS PVQTY, NVL(IQTYIN,0) AS QCDONE FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE = '16' and inspected='Y' AND VCHDATE " + xprdrange + ") A , ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE)  GROUP BY TRIM(A.ICODE),TRIM(B.INAME) HAVING SUM(A.PVQTY)-SUM(A.QCDONE)>'0'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Pending for Quality Summary";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25231": //////8/8/18
                    SQuery = "select  nvl(trim(b.aname),'-') as Name,count(a.icode) as Times_Rcvd,sum(nvl(a.iqtyin,0)+nvl(a.rej_rw,0)) as Total_Inward,sum(nvl(a.rej_rw,0)) as Total_Rej,round((sum(nvl(a.rej_rw,0))/(sum(nvl(a.iqtyin,0)+nvl(a.rej_rw,0))))*100 ,2) as Rej_per,a.acode from ivoucher a left outer join famst b on a.acode=b.acode where a.branchcd='" + mbr + "' and substr(a.type,1,1)='0' and a.vchdate " + xprdrange + "  and a.store<>'R'  group by b.aname,a.acode having sum(nvl(a.rej_rw,0))>0 order by b.aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Rejn.Stock Summary Vendor Wise for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25255": ////// MADE BY MADHVI, MERGED 19.01.18, SVPL, WIP RECONCILIATION
                    DataTable dtm = new DataTable();
                    dtm.Columns.Add("Code", typeof(string));
                    dtm.Columns.Add("Item_Name", typeof(string));
                    dtm.Columns.Add("Unit", typeof(string));
                    dtm.Columns.Add("RM_WIP_Op", typeof(double));
                    dtm.Columns.Add("SF_WIP_Op", typeof(double));
                    dtm.Columns.Add("SF_Store_Opening", typeof(double));
                    dtm.Columns.Add("TP_Opening", typeof(double));
                    dtm.Columns.Add("Total_Opening", typeof(double));
                    dtm.Columns.Add("RM_Rcpt_Qty", typeof(double));
                    dtm.Columns.Add("CRM", typeof(double));
                    dtm.Columns.Add("Total_Receipt", typeof(double));
                    dtm.Columns.Add("To_Bond", typeof(double));
                    dtm.Columns.Add("Return_Note", typeof(double));
                    dtm.Columns.Add("CR_Rejn", typeof(double));
                    dtm.Columns.Add("MR_Rejn", typeof(double));
                    dtm.Columns.Add("Total", typeof(double));
                    dtm.Columns.Add("SF_Store_Closing", typeof(double));
                    dtm.Columns.Add("TP_Closing", typeof(double));
                    dtm.Columns.Add("WIP_Closing", typeof(double));
                    dtm.Columns.Add("Rate", typeof(double));
                    dtm.Columns.Add("Value", typeof(double));
                    dtm.Columns.Add("Bfactor", typeof(string));

                    // FAMILY CODE
                    dt12 = new DataTable();
                    mq12 = "SELECT TRIM(TYPE1) AS TYPE1,NAME FROM TYPEGRP WHERE ID='^8' ORDER BY TYPE1";
                    dt12 = fgen.getdata(frm_qstr, co_cd, mq12);

                    // LATEST MRR RATE FOR FINDING VALUE .... PICKED FROM MRR COSTING
                    dt11 = new DataTable();
                    er1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R06'", "params");
                    mq11 = "SELECT VCHDATE,TRIM(ICODE) AS ICODE,ICHGS AS IRATE/*(CASE WHEN TRIM(TYPE)='07' THEN CAVITY*IRATE ELSE IRATE END) AS IRATE*/ FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%' AND /*VCHDATE>(SYSDATE-500)*/ VCHDATE>=TO_DATE('" + er1 + "','DD/MM/YYYY') AND SUBSTR(TRIM(ICODE),1,1) <'1' AND STORE='Y' ORDER BY VCHDATE DESC";
                    dt11 = fgen.getdata(frm_qstr, co_cd, mq11);

                    // PARENT ICODE OF 9 SERIES ITEM WITH THEIR CHILD CODE
                    dt10 = new DataTable();
                    mq10 = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(A.IBCODE) AS IBCODE,A.IBQTY,I.UNIT FROM ITEMOSP A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1) ='9' AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') ORDER BY ICODE";
                    dt10 = fgen.getdata(frm_qstr, co_cd, mq10);

                    // FOR ALL CHILD PARTS OF 9 SERIES
                    dt9 = new DataTable();
                    mq9 = "SELECT DISTINCT TRIM(A.IBCODE) AS IBCODE,I.INAME,I.UNIT,TRIM(I.BFACTOR) AS BFACTOR FROM ITEMOSP A,ITEM I WHERE TRIM(A.IBCODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1) ='9' AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') AND (NVL(A.IBQTY,0)>0 OR NVL(A.IBWT,0)>0) ORDER BY BFACTOR,IBCODE";
                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9);

                    // SF WIP OPENING
                    //ded4 = "SELECT TO_CHAR(TO_DATE('01/" + todt.Substring(3, 7) + "','DD/MM/YYYY')-1,'DD/MM/YYYY') AS lastmnth FROM DUAL";
                    ded4 = "SELECT TO_CHAR(TO_DATE('" + fromdt + "','DD/MM/YYYY')-1,'DD/MM/YYYY') AS lastmnth FROM DUAL";
                    ded5 = fgen.seek_iname(frm_qstr, co_cd, ded4, "lastmnth"); // PREVIOUS MONTH DATE
                    wip_stk_vw_SVPL(ded5);
                    dt8 = new DataTable();
                    mq8 = "select trim(icode) as icode,total from wipcolstkw_" + mbr + " order by icode";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8);

                    // RM WIP OPENING
                    wiptotstk_SVPL();
                    dt7 = new DataTable();
                    mq7 = "select trim(erp_code) as icode,opening,closing from wiptotstkw_" + mbr + " order by icode";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7);

                    //RETURN NOTE
                    dt6 = new DataTable();
                    mq6 = "Select trim(a.Icode) as iCode,sum(a.iqty_chl) as Req_Qty,sum(a.iqtyin) as ret_Qty from ivoucher a where a.branchcd='" + mbr + "' and substr(A.type,1,2) like '1%' and a.vchdate " + xprdrange + " and  a.store!='N' group by trim(a.Icode) order by icode";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq6);

                    // TP STOCK
                    string TP_starting_dt = "";
                    TP_starting_dt = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    xprdrange1 = " BETWEEN TO_DATE('" + TP_starting_dt + "','DD/MM/YYYY')-1 AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    dt5 = new DataTable();
                    // mq5 = "select trim(icode) as icode,sum(opening) as opening,sum(qty_sent) as qty_sent,sum(qty_rcvd) as qty_rcvd ,sum(qty_balance) as qty_balance from (select b.iname,trim(a.icode) as Icode,sum(a.opening) as Opening,sum(a.cdr) as Qty_Sent,sum(a.ccr) as Qty_Rcvd,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Qty_balance,c.aname,c.acode,b.cpartno from (Select '-' as acode,'-' as store,icode, YR_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,0 as inqa from ITEMBAL where 1=2 union all  select acode,store,icode,sum(iqtyout)-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in('0','2') and vchdate between to_date('01/04/2010','dd/mm/yyyy')-1 and to_Date('" + fromdt + "','dd/mm/yyyy')-1  and acode like '%%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE union all select acode,store,icode,0 as op,sum(iqtyout) as cdr,sum(iqtyin) as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in('0','2') and vchdate " + xprdrange + "  and acode like '%%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE )a,item b,(select distinct a.acode,b.aname from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.type in ('21','23'))c where trim(A.icode)=trim(B.icode) and trim(A.acode)=trim(c.acode) group by b.iname,b.cpartno,trim(a.icode),c.aname,c.acode /*having sum(a.opening)+sum(a.cdr)+sum(a.ccr) >0*/ order by c.acode,b.iname) group by icode";
                    mq5 = "select b.iname as Item_Name,sum(a.opening) as Opening,sum(a.cdr) as Qty_Sent,sum(a.ccr) as Qty_Rcvd,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Qty_balance,trim(a.icode) as Icode,b.cpartno from (Select '-' as acode,'-' as store,icode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,0 as inqa from ITEMBAL where 1=2 union all select acode,store,icode,sum(iqtyout)-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,2) in('09','21','0J','23') and vchdate " + xprdrange1 + " and acode like '%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE union all select acode,store,icode,0 as op,sum(iqtyout) as cdr,sum(iqtyin) as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,2) in('09','21','0J','23') and vchdate " + xprdrange + " and acode like '%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE )a,item b,(select distinct a.acode,b.aname from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.type in ('21','23'))c where trim(A.icode)=trim(B.icode) and trim(A.acode)=trim(c.acode) group by b.iname,b.cpartno,trim(a.icode) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by Icode";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq5);

                    // REJECTION STOCK
                    string starting_rej_store_dt = "";
                    starting_rej_store_dt = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R24'", "params");
                    xprdrange1 = " BETWEEN TO_DATE('" + starting_rej_store_dt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";

                    dt4 = new DataTable();
                    mq4 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as opening,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='R' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='R' GROUP BY trim(icode) ,branchcd) a GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);

                    // MAIN STOCK
                    xprdrange1 = " BETWEEN TO_DATE('" + cDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    dt3 = new DataTable();
                    mq3 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    // PARENT ICODE OF 82,84 SERIES ITEM
                    dt2 = new DataTable();
                    mq2 = "SELECT DISTINCT TRIM(ICODE) AS ICODE,TRIM(IBCODE) AS IBCODE FROM ITEMOSP WHERE SUBSTR(TRIM(IBCODE),1,2) IN ('82','84') ORDER BY ICODE";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    // PARENT ICODE OF 01 AND 02 SERIES ITEM WITH THEIR CHILD ICODE
                    dt1 = new DataTable();
                    mq1 = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(A.IBCODE) AS IBCODE,A.IBQTY,I.UNIT FROM ITEMOSP A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2) IN ('82','84') ORDER BY ICODE";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    // FOR ALL CHILD PARTS OF 82 AND 84 GROUP
                    dt = new DataTable();
                    mq0 = "SELECT DISTINCT TRIM(A.IBCODE) AS IBCODE,I.INAME,I.UNIT,TRIM(I.BFACTOR) AS BFACTOR FROM ITEMOSP A,ITEM I WHERE TRIM(A.IBCODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2) IN ('82','84') AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') AND (NVL(A.IBQTY,0)>0 OR NVL(A.IBWT,0)>0) ORDER BY BFACTOR,IBCODE";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    oporow = null; double Total_Receipt = 0, Total_Rejn = 0, Total_Op = 0, Closing_WIP = 0, RM_Rcpt_Qty = 0, SF_Store_Opening = 0, SF_Store_Closing = 0, CR_Rejn = 0, MR_Rejn = 0, TP_Opening = 0, TP_Closing = 0, Return_Note = 0, RM_WIP_Op = 0, SF_WIP_Op = 0, CRM = 0, To_Bond = 0, Bom_Qty = 0, To_Bond_Exists = 0;
                    string To_Bond_YN = "";
                    DataTable SFParent = new DataTable();
                    #region For 82,84 Groups
                    if (dt.Rows.Count > 0)
                    {
                        view1 = new DataView(dt);
                        mdt = new DataTable();
                        mdt = view1.ToTable(true, "IBCODE", "INAME", "UNIT", "BFACTOR");
                        foreach (DataRow dr in mdt.Rows)
                        {
                            dticode = new DataTable();
                            dticode2 = new DataTable();
                            SFParent = new DataTable();
                            if (dt1.Rows.Count > 0)
                            {
                                // BASED ON RAW MATERIAL FIND ITS PARENT CODE I.E. SF CODE
                                view2 = new DataView(dt1, "ibcode='" + dr["ibcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode = view2.ToTable();
                            }
                            Total_Receipt = 0; Total_Rejn = 0; Total_Op = 0; Closing_WIP = 0; RM_Rcpt_Qty = 0; SF_Store_Opening = 0; SF_Store_Closing = 0; CR_Rejn = 0; MR_Rejn = 0; TP_Opening = 0; TP_Closing = 0; Return_Note = 0; RM_WIP_Op = 0; SF_WIP_Op = 0; CRM = 0; To_Bond = 0; Bom_Qty = 0;
                            oporow = dtm.NewRow();
                            oporow["Code"] = dr["ibcode"].ToString().Trim();
                            oporow["Item_Name"] = dr["iname"].ToString().Trim();
                            oporow["Unit"] = dr["unit"].ToString().Trim();
                            oporow["Bfactor"] = dr["bfactor"].ToString().Trim(); // FAMILY CODE

                            if (dt3.Rows.Count > 0)
                            {
                                RM_Rcpt_Qty = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Issued"));
                            }
                            if (dt4.Rows.Count > 0)
                            {
                                CR_Rejn = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Rcpt"));
                            }
                            if (dt6.Rows.Count > 0)
                            {
                                Return_Note += fgen.make_double(fgen.seek_iname_dt(dt6, "icode='" + dr["ibcode"].ToString().Trim() + "'", "ret_Qty"));
                            }
                            if (dt7.Rows.Count > 0)
                            {
                                RM_WIP_Op = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr["ibcode"].ToString().Trim() + "'", "opening"));
                            }

                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                if (dt2.Rows.Count > 0)
                                {
                                    // BASED ON SF CODE FIND ITS PARENT CODE I.E FG CODE
                                    DataView view3 = new DataView(dt2, "ibcode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dticode2 = view3.ToTable();
                                }

                                if (dt3.Rows.Count > 0)
                                {
                                    SF_Store_Opening += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "opening"));
                                    SF_Store_Closing += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    MR_Rejn += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                }
                                if (dt5.Rows.Count > 0)
                                {
                                    TP_Opening += fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Opening"));
                                    TP_Closing += fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Qty_balance"));
                                }
                                if (dt8.Rows.Count > 0)
                                {
                                    SF_WIP_Op += fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "total"));
                                }
                                for (int k = 0; k < dticode2.Rows.Count; k++)
                                {
                                    To_Bond_Exists = 0; To_Bond_YN = "N";
                                    // IF THE PARENT CODE OF RM ITEM DOES NOT HAVING 9 SERIES ITEM AS ITS PARENT
                                    if (dticode2.Rows[k]["icode"].ToString().Trim().Substring(0, 1) != "9")
                                    {
                                        if (dt3.Rows.Count > 0)
                                        {
                                            SF_Store_Opening += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "opening"));
                                            SF_Store_Closing += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                        }
                                        if (dt4.Rows.Count > 0)
                                        {
                                            MR_Rejn += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Rcpt"));
                                        }
                                        if (dt5.Rows.Count > 0)
                                        {
                                            TP_Opening += fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Opening"));
                                            TP_Closing += fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Qty_balance"));
                                        }
                                        if (dt8.Rows.Count > 0)
                                        {
                                            SF_WIP_Op += fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "total"));
                                        }
                                        if (dt2.Rows.Count > 0)
                                        {
                                            // BASED ON SF CODE FIND ITS PARENT CODE I.E FG CODE
                                            DataView view4 = new DataView(dt2, "ibcode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                            SFParent = view4.ToTable();
                                        }
                                        for (int l = 0; l < SFParent.Rows.Count; l++)
                                        {
                                            To_Bond_Exists = 0; To_Bond_YN = "N";
                                            if (dt4.Rows.Count > 0)
                                            {
                                                CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + SFParent.Rows[l]["icode"].ToString().Trim() + "'", "Rcpt"));
                                            }
                                            To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + SFParent.Rows[l]["icode"].ToString().Trim() + "'", "Rcpt"));
                                        }
                                    }
                                    else
                                    {
                                        if (dt4.Rows.Count > 0)
                                        {
                                            CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Rcpt"));
                                        }
                                        // CHECKING TO_BOND HAS VALUE OR NOT
                                        To_Bond_Exists = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Rcpt"));
                                        if (To_Bond_Exists != 0)
                                        {
                                            // IF TO_BOND HAS VALUE THEN SET IT TO Y FOR MULTIPLICATION
                                            To_Bond_YN = "Y";
                                        }
                                        To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode2.Rows[k]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    if (dr["unit"].ToString().Trim() != dticode.Rows[i]["unit"].ToString().Trim())
                                    {
                                        if (To_Bond_YN == "Y")
                                        {
                                            To_Bond = To_Bond * fgen.make_double(dticode.Rows[i]["ibqty"].ToString().Trim());
                                        }
                                    }
                                }
                                Total_Receipt = RM_Rcpt_Qty + CRM;
                                Total_Op = RM_WIP_Op + SF_WIP_Op + SF_Store_Opening + TP_Opening;
                                Total_Rejn = Return_Note + To_Bond + CR_Rejn + MR_Rejn;
                            }
                            oporow["RM_Rcpt_Qty"] = Math.Round(RM_Rcpt_Qty, 0);
                            oporow["SF_Store_Opening"] = Math.Round(SF_Store_Opening, 0);
                            oporow["SF_Store_Closing"] = Math.Round(SF_Store_Closing, 0);
                            oporow["CR_Rejn"] = Math.Round(CR_Rejn, 0);
                            oporow["MR_Rejn"] = Math.Round(MR_Rejn, 0);
                            oporow["TP_Opening"] = Math.Round(TP_Opening, 0);
                            oporow["TP_Closing"] = Math.Round(TP_Closing, 0);
                            oporow["Return_Note"] = Math.Round(Return_Note, 0);
                            oporow["RM_WIP_Op"] = Math.Round(RM_WIP_Op, 0);
                            oporow["SF_WIP_Op"] = Math.Round(SF_WIP_Op, 0);
                            oporow["CRM"] = Math.Round(CRM, 0);
                            oporow["To_Bond"] = Math.Round(To_Bond, 0);
                            oporow["Total_Opening"] = Math.Round(Total_Op, 0);
                            oporow["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                            oporow["Total"] = Math.Round(Total_Rejn, 0);
                            Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;
                            oporow["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                            if (RM_Rcpt_Qty + SF_Store_Opening + CR_Rejn + MR_Rejn + TP_Opening + Return_Note + RM_WIP_Op + SF_WIP_Op + CRM + To_Bond > 0)
                            {
                                dtm.Rows.Add(oporow);
                            }
                        }
                    }
                    #endregion
                    #region For 9 Series
                    if (dt9.Rows.Count > 0)
                    {
                        view1 = new DataView(dt9);
                        mdt = new DataTable();
                        mdt = view1.ToTable(true, "IBCODE", "INAME", "UNIT", "BFACTOR");
                        ded1 = ""; ded2 = "";
                        foreach (DataRow dr in mdt.Rows)
                        {
                            dticode = new DataTable();
                            er1 = "";
                            if (dt10.Rows.Count > 0)
                            {
                                // BASED ON RAW MATERIAL FIND ITS PARENT CODE I.E. FG CODE
                                view2 = new DataView(dt10, "ibcode='" + dr["ibcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode = view2.ToTable();
                            }
                            Total_Receipt = 0; Total_Rejn = 0; Total_Op = 0; Closing_WIP = 0; RM_Rcpt_Qty = 0; SF_Store_Opening = 0; SF_Store_Closing = 0; CR_Rejn = 0; MR_Rejn = 0; TP_Opening = 0; TP_Closing = 0; Return_Note = 0; RM_WIP_Op = 0; SF_WIP_Op = 0; CRM = 0; To_Bond = 0; Bom_Qty = 0;

                            // IF ONE RM HAS MORE THAN ONE PARENT THEN IT WILL MERGE THEM IN TO ONE
                            er1 = "";
                            er1 = fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "code");
                            int index = 0;
                            if (er1.Length > 1)
                            {
                                DataRow[] rows = dtm.Select("code = '" + dr["ibcode"].ToString().Trim() + "'");
                                if (rows.Length > 0)
                                {
                                    index = dtm.Rows.IndexOf(rows[0]); // FOR FINDING ROW INDEX
                                }
                                RM_Rcpt_Qty = 0;
                                CR_Rejn = 0;
                                Return_Note = 0;
                                RM_WIP_Op = 0;
                                Closing_WIP = 0;
                                SF_Store_Opening = 0;
                                SF_Store_Closing = 0;
                                MR_Rejn = 0;
                                TP_Opening = 0;
                                TP_Closing = 0;
                                SF_WIP_Op = 0;

                                Total_Receipt = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "Total_Receipt"));
                                Total_Rejn = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "Total"));
                                Total_Op = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "Total_Opening"));
                                SF_Store_Closing = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "SF_Store_Closing"));
                                TP_Closing = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "TP_Closing"));

                                for (int i = 0; i < dticode.Rows.Count; i++)
                                {
                                    Bom_Qty = fgen.make_double(dticode.Rows[i]["ibqty"].ToString().Trim());
                                    if (dt4.Rows.Count > 0)
                                    {
                                        CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    if (Bom_Qty == 0)
                                    {
                                        To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    else
                                    {
                                        To_Bond += Bom_Qty * fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                }
                                Total_Receipt = Total_Receipt + CRM;
                                Total_Rejn = Total_Rejn + To_Bond;
                                CRM += fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "crm"));
                                To_Bond += fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "to_bond"));
                                dtm.Rows[index]["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                                dtm.Rows[index]["Total"] = Math.Round(Total_Rejn, 0);
                                dtm.Rows[index]["CRM"] = Math.Round(CRM, 0);
                                dtm.Rows[index]["To_Bond"] = Math.Round(To_Bond, 0);

                                Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;
                                dtm.Rows[index]["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                            }
                            else
                            {
                                if (dt3.Rows.Count > 0)
                                {
                                    RM_Rcpt_Qty = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Issued"));
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    CR_Rejn = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Rcpt"));
                                }
                                if (dt6.Rows.Count > 0)
                                {
                                    Return_Note += fgen.make_double(fgen.seek_iname_dt(dt6, "icode='" + dr["ibcode"].ToString().Trim() + "'", "ret_Qty"));
                                }
                                if (dt7.Rows.Count > 0)
                                {
                                    RM_WIP_Op = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr["ibcode"].ToString().Trim() + "'", "opening"));
                                    Closing_WIP = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr["ibcode"].ToString().Trim() + "'", "closing"));
                                }
                                SF_Store_Opening = 0;
                                SF_Store_Closing = 0;
                                MR_Rejn = 0;
                                TP_Opening = 0;
                                TP_Closing = 0;
                                SF_WIP_Op = 0;
                                for (int i = 0; i < dticode.Rows.Count; i++)
                                {
                                    Bom_Qty = fgen.make_double(dticode.Rows[i]["ibqty"].ToString().Trim());
                                    if (dt4.Rows.Count > 0)
                                    {
                                        CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    if (Bom_Qty == 0)
                                    {
                                        To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    else
                                    {
                                        To_Bond += Bom_Qty * fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                }
                                Total_Receipt = RM_Rcpt_Qty + CRM;
                                Total_Op = RM_WIP_Op + SF_WIP_Op + SF_Store_Opening + TP_Opening;
                                Total_Rejn = Return_Note + To_Bond + CR_Rejn + MR_Rejn;
                                oporow = dtm.NewRow();
                                oporow["Code"] = dr["ibcode"].ToString().Trim();
                                oporow["Item_Name"] = dr["iname"].ToString().Trim();
                                oporow["Unit"] = dr["unit"].ToString().Trim();
                                oporow["Bfactor"] = dr["bfactor"].ToString().Trim(); // FAMILY CODE
                                oporow["RM_Rcpt_Qty"] = Math.Round(RM_Rcpt_Qty, 0);
                                oporow["SF_Store_Opening"] = Math.Round(SF_Store_Opening, 0);
                                oporow["SF_Store_Closing"] = Math.Round(SF_Store_Closing, 0);
                                oporow["CR_Rejn"] = Math.Round(CR_Rejn, 0);
                                oporow["MR_Rejn"] = Math.Round(MR_Rejn, 0);
                                oporow["TP_Opening"] = Math.Round(TP_Opening, 0);
                                oporow["TP_Closing"] = Math.Round(TP_Closing, 0);
                                oporow["Return_Note"] = Math.Round(Return_Note, 0);
                                oporow["RM_WIP_Op"] = Math.Round(RM_WIP_Op, 0);
                                oporow["SF_WIP_Op"] = Math.Round(SF_WIP_Op, 0);
                                oporow["CRM"] = Math.Round(CRM, 0);
                                oporow["To_Bond"] = Math.Round(To_Bond, 0);
                                oporow["Total_Opening"] = Math.Round(Total_Op, 0);
                                oporow["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                                oporow["Total"] = Math.Round(Total_Rejn, 0);
                                Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;
                                oporow["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                                if (RM_Rcpt_Qty + SF_Store_Opening + CR_Rejn + MR_Rejn + TP_Opening + Return_Note + RM_WIP_Op + SF_WIP_Op + CRM + To_Bond > 0)
                                {
                                    dtm.Rows.Add(oporow);
                                }
                            }
                        }
                    }
                    #endregion

                    // VALUE IS FETCHED HERE SO THAT AGAIN AND AGAIN CALCULATION CAN BE RESTRICTED                   
                    foreach (DataRow dr1 in dtm.Rows)
                    {
                        db1 = 0;
                        db1 = fgen.make_double(fgen.seek_iname_dt(dt11, "icode='" + dr1["Code"].ToString().Trim() + "'", "irate"));
                        dr1["Rate"] = Math.Round(db1, 0);
                        dr1["Value"] = Math.Round(fgen.make_double(dr1["Wip_Closing"].ToString().Trim()) * db1, 0);
                    }

                    if (dtm.Rows.Count > 0)
                    {
                        // SORTING ON THE BASIS OF FAMILY
                        view1 = new DataView(dtm);
                        dticode2 = new DataTable();
                        dticode2 = view1.ToTable(true, "BFACTOR");
                        mdt = new DataTable();
                        mdt = dtm.Clone();
                        db1 = 0;
                        foreach (DataRow dr1 in dticode2.Rows)
                        {
                            view2 = new DataView(dtm, "bfactor='" + dr1["bfactor"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = view2.ToTable();
                            ded1 = "";
                            oporow = mdt.NewRow();
                            ded1 = fgen.seek_iname_dt(dt12, "type1='" + dr1["bfactor"].ToString().Trim() + "'", "name");
                            if (ded1.Trim() == "0")
                            {
                                oporow["Item_Name"] = "-";
                            }
                            else
                            {
                                oporow["Item_Name"] = ded1;
                            }
                            mdt.Rows.Add(oporow);
                            // FAMILY WISE TOTAL
                            ROWICODE = dticode.NewRow();
                            foreach (DataColumn dc in dticode.Columns)
                            {
                                to_cons = 0;
                                if (dc.Ordinal == 20)
                                {
                                    mq1 = "sum(" + dc.ColumnName + ")";
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db1 += to_cons; // TOTAL OF VALUE REQUIRED DURING GRAND TOTAL
                                }
                            }
                            ROWICODE["Item_Name"] = oporow["Item_Name"].ToString() + " (TOTAL)";
                            dticode.Rows.Add(ROWICODE);
                            mdt.Merge(dticode);
                        }
                        mdt.Columns.Remove("BFACTOR");
                        oporow = null;
                        oporow = mdt.NewRow();
                        foreach (DataColumn dc in mdt.Columns)
                        {
                            to_cons = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 19)
                            {

                            }
                            else if (dc.Ordinal == 20)
                            {
                                oporow[dc] = Math.Round(db1, 0);
                            }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                to_cons += fgen.make_double(mdt.Compute(mq1, "").ToString());
                                oporow[dc] = to_cons;
                            }
                        }
                        oporow["Item_Name"] = "Grand Total";
                        mdt.Rows.InsertAt(oporow, 0);
                    }
                    Session["send_dt"] = mdt;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    fgen.Fn_open_rptlevel("WIP Report From " + fromdt + " To " + todt + "", frm_qstr);
                    break;

                case "F25266":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    SQuery = "SELECT TRIM(B.ICODE) AS SUBGRP,B.INAME AS SUBGRP_NAME,A.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,I.UNIT,A.NO_BDLS AS BATCH,A.MFGDT1 AS MFGDT,A.EXPDT1 AS EXPDT,sum(a.iqty) as opening,SUM(A.IQTY) AS RECEIPT,SUM(A.OQTY) AS ISSUE,sum(a.iqty)-sum(a.oqty) as closing FROM(select DISTINCT TRIM(ICODE) AS ICODE,TRIM(NO_BDLS) AS NO_BDLS,MFGDT1,EXPDT1,IQTY,0 AS OQTY from excvch where " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL select TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,MFGDT,EXPDT,0 AS IQTY,IQTYOUT from ivoucher where " + branch_Cd + " and type like '4%' and vchdate " + xprdrange + ") A,ITEM I ,ITEM B WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),0,4)=TRIM(B.ICODE) AND LENGTH(TRIM(B.ICODE))=4 AND A.ICODE IN (" + part_cd + ") GROUP BY A.ICODE,A.NO_BDLS,I.INAME,A.MFGDT1,A.EXPDT1,B.ICODE,B.INAME,I.UNIT HAVING SUM(A.IQTY)-SUM(A.OQTY)>0 ORDER BY A.ICODE,A.NO_BDLS";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Stock Statement Report For The Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25119":
                case "F25123":
                    mq1 = hf1.Value;
                    if (val == "F25123")
                    {
                        if (mq1.Trim() == "Y")
                        {
                            cond = "";
                        }
                        else
                        {
                            cond = " and a.phy>0";
                        }
                    }
                    else if (val == "F25119")
                    {
                        if (mq1.Trim() == "Y")
                        {
                            cond = " and a.stk=0 and a.phy>0";
                        }
                        else
                        {
                            cond = " and a.phy=0";
                        }
                    }
                    SQuery = "Select a.icode as erpcode,b.iname as reelname,a.kclreelno as reel_no,a.stk as erp_stk,a.phy as phy_veri,(a.stk-a.phy) as diff from (Select icode,kclreelno,sum(stk) as stk,sum(phy) as phy from (Select trim(a.icode) as icode,trim(a.kclreelno) as kclreelno,a.tot as stk,0 as phy from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + mbr + "' group by kclreelno,trim(icode)) group by trim(icode) ,kclreelno) a where a.tot>0 union all select trim(icode) as icode,trim(maincode) as acode,0 as stk,iqtyin as phy from wipstk where branchcd='" + mbr + "' and type='RV' and vchdate " + xprdrange + ") group by icode,kclreelno ) a,item b where trim(a.icode)=trim(b.icode) " + cond + " order by a.icode,a.kclreelno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    if (val == "F25123") fgen.Fn_open_rptlevel("Reel Stock Vs Physical Verification Report", frm_qstr);
                    else if (val == "F25119") fgen.Fn_open_rptlevel("Missing Reels in Phy. Verification", frm_qstr);
                    break;

                case "F25120":
                    SQuery = "SELECT trim(FSTR) as fstr,DESC_,REELS,WEIGHT FROM (Select 'A' as fstr,'Total Reel found in ERP' as Desc_, count(a.stk) as Reels,sum(a.stk) as weight from (Select icode,kclreelno,sum(stk) as stk,sum(phy) as phy from (Select trim(a.icode) as icode,trim(a.kclreelno) as kclreelno,a.tot as stk,0 as phy from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and BRANCHCD='" + mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and BRANCHCD='" + mbr + "' group by kclreelno,trim(icode)) group by trim(icode) ,kclreelno) a where a.tot>0 union all select trim(icode) as icode,trim(acode) as acode,0 as stk,num1 as phy from scratch where BRANCHCD='" + mbr + "' and type='RL' and vchdate " + xprdrange + ") group by icode,kclreelno ) a union all Select 'P' as fstr,'Total Reel found in Phy. Verifiaction' as Desc_, count(a.acode) as Reels,sum(a.phy) as weight from (Select acode,sum(num1) as phy from scratch where BRANCHCD='" + mbr + "' and type='RL' and vchdate " + xprdrange + " group by acode) a having sum(a.phy)>0 )";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Reel Stock Vs Physical Verification Summary Report", frm_qstr);
                    break;
                case "F25130":
                    mq0 = "MR";
                    if (hfcode.Value == "3") mq0 = "MR";
                    SQuery = "SELECT DISTINCT A.VCHNUM AS VOUCHER_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VOUCHER_DT, f.wono AS ERPCODE,c.INAME AS ITME_NAME,a.planno,a.plandt,F.CONTPLAN AS PLAN_QTY,d.refnum as ticket_no,to_Char(d.refdate,'dd/mm/yyyy') as ticket_dt,a.qty as TICKET_qty,e.name as paste_type,a.icode as add_item_code,b.iname as add_itemname,a.start1 as add_item_qty,substr(A.COMMENTS,5,6) as mrr_no,substr(A.COMMENTS,17,2)||'/'||substr(A.COMMENTS,15,2)||'/'||substr(A.COMMENTS,11,4) as mrr_dt,A.ENT_BY,A.ENT_dT,to_Char(A.vchdate,'yyyymmdd') as vdd FROM EXTRUSION A,ITEM B,ITEM C,IVOUCHERW D,type e,inspvch f WHERE TRIM(A.ICODe)=TRIM(B.ICODE) AND A.BRANCHCD||TRIM(A.PLANNO)||TRIM(a.PLANDT)||trim(a.btchno)=F.BRANCHCD||TRIM(F.BTCHNO)||TRIM(F.BTCHDT)||trim(f.obsv16) AND F.TYPE='BI' AND F.SRNO=1 AND TRIM(f.wono)=TRIM(C.ICODE) and SUBSTR(A.BTCHNO,1,2)=TRIM(E.TYPE1) AND E.ID='Y' AND A.BRANCHCD||TRIM(A.PLANNO)||TRIM(A.PLANDT)||TRIM(A.BTCHNO)=D.BRANCHCD||TRIM(D.INVNO)||TO_CHAR(D.INVDATE,'DD/MM/YYYY')||TRIM(D.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='" + hfcode.Value + "C' AND A.VCHDATE " + xprdrange + " AND D.type='" + mq0 + "' order by VDD DESC,A.VCHNUM DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Store Tagging Report ", frm_qstr);
                    break;

                //case "F25133A"://NAHR REPORT=========this old code with logical changes
                #region
                //    int cnt;
                //    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();
                //    dt6 = new DataTable(); dt7 = new DataTable(); dt8 = new DataTable(); dt9 = new DataTable();
                //    dtm.Columns.Add("sno", typeof(int));
                //    dtm.Columns.Add("Txn_Date", typeof(string));
                //    dtm.Columns.Add("Document_No", typeof(string));
                //    dtm.Columns.Add("Party_Name", typeof(string));
                //    dtm.Columns.Add("Opening_Stock", typeof(double));
                //    dtm.Columns.Add("Purchase_QTY", typeof(double));
                //    dtm.Columns.Add("Sale_Return", typeof(double));
                //    dtm.Columns.Add("Pur_Return_Qty", typeof(double));
                //    dtm.Columns.Add("Issue_Qty", typeof(double));
                //    dtm.Columns.Add("Closing_Stock", typeof(double));
                //    //======================********************================================================
                //    cond2 = "and substr(trim(icode),1,2) in ('07','02')";// AS PER ARVIND SIR
                //    header_n = "RM Stock Report Inw/Out Wt";
                //    int yr = Convert.ToInt32(frm_myear);
                //    //int yr1 = yr++;
                //    xprd3 = ""; string xprdrange2 = "";
                //    DataTable lydt = new DataTable();
                //    if (yr > 2019)
                //    {
                //        int yrr = yr - 1;
                //        //xprd3 = "between to_date('01/04/" + yrr + "','dd/MM/yyyy') and to_Date('01/04/" + yrr + "','dd/MM/yyyy')-1";
                //        //xprdrange2 = " between to_date('01/04/" + yrr + "','dd/mm/yyyy') and to_date('31/03/" + yr + "','dd/mm/yyyy')";
                //        xprd3 = "between to_date('01/04/2019','dd/MM/yyyy') and to_Date('01/04/2019','dd/MM/yyyy')-1";
                //        xprdrange2 = " between to_date('01/04/2019','dd/mm/yyyy') and to_date('31/03/" + yr + "','dd/mm/yyyy')";
                //        // SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from ( select sum(a.opening) as opening,0 as Rcpt,0 as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2)='07'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y' and substr(trim(icode),1,2)='07' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + "  and store='Y' and substr(trim(icode),1,2)='07'  GROUP BY trim(icode) ,branchcd) a having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 union all select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) union all select 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') and a.store='Y' )   GROUP BY invno,invdt,acode,party  order by invno,invdt) )"; //old
                //        //  SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,Sum((a.opening+a.Rcpt)-a.Issued) as Closing_Stk from (select sum((case when substr(trim(a.icode),1,2)='02' then round((a.opening * b.iweight),3) else a.opening  end )) as opening,0 as Rcpt,0 as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2)='07'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y' and substr(trim(icode),1,2) in ('02','07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + "  and store='Y' and substr(trim(icode),1,2)='07'  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) ) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0   union all select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) union all select 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum((case when substr(trim(a.icode),1,2)='02' then round((a.iqtyout * c.iweight),3) else a.iqtyout  end )) as pur_Ret_qty FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') and a.store='Y' )   GROUP BY invno,invdt,acode,party  order by invno,invdt) )";
                //        SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,Sum((a.opening+a.Rcpt)-a.Issued) as Closing_Stk from (select sum((case when substr(trim(a.icode),1,2)='02' then round((a.opening * b.iweight),3) else a.opening  end )) as opening,0 as Rcpt,0 as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2) in ('02','07')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y' and substr(trim(icode),1,2) in ('02','07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + "  and store='Y' and substr(trim(icode),1,2) in ('02','07')  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) ) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0   union all select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) union all select 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum((case when substr(trim(a.icode),1,2)='02' then round((a.iqtyout * c.iweight),3) else a.iqtyout  end )) as pur_Ret_qty FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') and a.store='Y' )   GROUP BY invno,invdt,acode,party  order by invno,invdt) )";
                //        lydt = new DataTable();
                //        lydt = fgen.getdata(frm_qstr, co_cd, SQuery);
                //    }
                //    //
                //    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                //    SQuery = "SELECT distinct TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,to_char(a.vchdate,'yyyyMMdd') as vdd FROM IVOUCHER A WHERE A.BRANCHCD='" + mbr + "' AND (A.TYPE IN ('02','05','07','47','24') OR SUBSTR(A.TYPE,1,1)='4' ) AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR  SUBSTR(TRIM(A.ICODE),1,1)='9' ) AND A.VCHDATE " + xprdrange + " order by vdd";
                //    ph_tbl = new DataTable();
                //    ph_tbl = fgen.getdata(frm_qstr, co_cd, SQuery); //dt for view on date
                //    //////////////////////////
                //    if (yr < 2020)
                //    {
                //        mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + "  GROUP BY trim(icode) ,branchcd) a having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                //        mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,Sum((a.opening+a.Rcpt)-a.Issued) as Closing_Stk from (select sum((case when substr(trim(a.icode),1,2)='02' then round((a.opening * b.iweight),3) else a.opening  end )) as opening,sum( (case when substr(trim(a.icode),1,2)='02' then round((a.cdr * b.iweight),3) else a.cdr  end )) as Rcpt,sum((case when substr(trim(a.icode),1,2)='02' then round((a.ccr * b.iweight),3) else a.ccr  end )) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + "  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode)) a having sum(a.opening)+sum(a.Rcpt)+sum(a.Issued)<>0  ";
                //        dt = fgen.getdata(frm_qstr, co_cd, mq1);//stock dt for 07 mg === 02 ka iweight se multiply + 07 as it is
                //        ///=============
                //        mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y'  and substr(trim(icode),1,1)='9'  GROUP BY trim(icode) ,branchcd) a  having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                //        mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";
                //        //mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  ( select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,(case when substr(trim(a.icode),1,2)='02' then round((a.yr_" + year + " * b.iweight),3) else a.yr_" + year + "  end ) as opening  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";                       
                //        dt9 = fgen.getdata(frm_qstr, co_cd, mq1);//STOCK DT FOR ICODE LIKE 9 mg === ok remove 96
                //    }
                //    //=============MRR DT
                //    mq2 = "SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc";
                //    dt1 = fgen.getdata(frm_qstr, co_cd, mq2);//mrr dt.......02,05,07 only
                //    //============================
                //    mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange + "  and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc";
                //    dt7 = fgen.getdata(frm_qstr, co_cd, mq5);//mrr dt.......04 only
                //    //==================
                //    //  mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE " + xprdrange + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                //    mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum((case when substr(trim(a.icode),1,2)='02' then round((a.iqtyout * c.iweight),3) else a.iqtyout  end )) as pur_Ret_qty FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE " + xprdrange + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                //    dt2 = fgen.getdata(frm_qstr, co_cd, mq3);//pur retrun dt....47 and 24 only
                //    //====================issue dt                   
                //    mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " and a.store='Y' AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt";
                //    dt3 = fgen.getdata(frm_qstr, co_cd, mq4);///issue dt
                //    cnt = 0;
                //    #region below qry changed by MG Mam
                //    //=================================***********===============================             
                //    //  cond2 = "and substr(trim(a.icode),1,2) in ('07','02')";// AS PER ARVIND SIR
                //    //  header_n = "RM Stock Report Inw/Out Wt";
                //    //  xprdRange1 = "between to_date('01/04/2019','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                //    ////  xprdRange1 = "between to_date('"+frm_cDt1+"','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                //    //  SQuery = "SELECT distinct TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,to_char(a.vchdate,'yyyyMMdd') as vdd FROM IVOUCHER A WHERE A.BRANCHCD='" + mbr + "' AND (a.type in ('02','05','07','04','24') or (substr(a.type,1,1)='4' and a.type not in ('45','49'))) AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.VCHDATE " + xprdrange + " order by vdd";
                //    //  ph_tbl = new DataTable();
                //    //  ph_tbl = fgen.getdata(frm_qstr, co_cd, SQuery); //dt for view on date
                //    //  //////////////////////////
                //    //  mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select a.branchcd,trim(a.icode) as icode,(case when substr(trim(a.icode),1,2)='02' then round((a.yr_2019 * b.iweight),3) else a.yr_2019  end ) as opening,0 as cdr,0 as ccr from itembal a, item b where a.branchcd='" + mbr + "'  and trim(a.icode)=trim(b.icode) and length(trim(a.icode))>4  " + cond2 + "  union all select a.branchcd,trim(a.icode) as icode,sum(nvl(a.iqtyin,0))-sum(nvl(a.iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER a where a.branchcd='" + mbr + "' AND a.VCHDATE " + xprdRange1 + "  and a.store='Y' " + cond2 + " and (a.type in ('02','05','07','04','24') or (substr(a.type,1,1)='4' and a.type not in ('45','49'))) GROUP BY trim(a.icode),a.branchcd union all select a.branchcd,trim(a.icode) as icode,0 as op,sum(nvl(a.iqtyin,0)) as cdr,sum(nvl(a.iqtyout,0)) as ccr from IVOUCHER a where a.branchcd='" + mbr + "' AND a.vchdate " + xprdrange + " and a.store='Y' " + cond2 + " and (a.type in ('02','05','07','04','24','47')) GROUP BY trim(a.icode) ,a.branchcd) a ";
                //    //  dt = fgen.getdata(frm_qstr, co_cd, mq1);//stock dt for 07 mg === 02 ka iweight se multiply + 07 as it is
                //    //  ///=============
                //    //  mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y'  and substr(trim(icode),1,1)='9'  GROUP BY trim(icode) ,branchcd) a  having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                //    //  mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') and (type in ('04','24') or (substr(type,1,1)='4' and type not in ('45','49'))) GROUP BY trim(icode),branchcd having sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) <0 union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";
                //    //  mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') and (type in ('04','24') or (substr(type,1,1)='4' and type not in ('45','49'))) GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";
                //    //  dt9 = fgen.getdata(frm_qstr, co_cd, mq1);//STOCK DT FOR ICODE LIKE 9 mg === ok remove 96
                //    //  //=============MRR DT
                //    //  mq2 = "SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','05','07') AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.VCHDATE " + xprdrange + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc";
                //    //  dt1 = fgen.getdata(frm_qstr, co_cd, mq2);//mrr dt.......02,05,07 only
                //    //  //============================
                //    //  mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' and (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.VCHDATE " + xprdrange + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc";
                //    //  mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' and (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.VCHDATE " + xprdrange + " and a.store='Y'  GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc"; //
                //    //  dt7 = fgen.getdata(frm_qstr, co_cd, mq5);//mrr dt.......04 only
                //    //  //==================
                //    //  mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.VCHDATE " + xprdrange + "  GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                //    //  mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.VCHDATE " + xprdrange + " and a.store='Y'  GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                //    //  dt2 = fgen.getdata(frm_qstr, co_cd, mq3);//pur retrun dt....47 and 24 only
                //    //  //====================issue dt                   
                //    //  mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,(case when substr(trim(a.icode),1,2)='07' then nvl(a.iqtyout,0) else nvl(a.iqtyout,0)*nvl(c.iweight,0)  end ) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) )   GROUP BY invno,invdt,acode,party  order by invno,invdt";
                //    //  mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,(case when substr(trim(a.icode),1,2)='07' then nvl(a.iqtyout,0) else nvl(a.iqtyout,0)*nvl(c.iweight,0)  end ) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR ( SUBSTR(TRIM(A.ICODE),1,1)='9' and SUBSTR(A.ICODE,1,2) not in ('99','96'))) AND A.Store='Y'  )   GROUP BY invno,invdt,acode,party  order by invno,invdt";
                //    //  dt3 = fgen.getdata(frm_qstr, co_cd, mq4);///issue dt
                //    //  cnt = 0;
                //    #endregion
                //    if (ph_tbl.Rows.Count > 0)
                //    {
                //        DataView view1im = new DataView(ph_tbl);
                //        DataTable dtdrsim = new DataTable();
                //        dtdrsim = view1im.ToTable(true, "vchdate"); //MAIN      
                //        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0;
                //        //===================
                //        dr1 = dtm.NewRow();
                //        //   dr1["Opening_Stock"] = fgen.make_double(dt.Rows[0]["opening"].ToString().Trim());
                //        if (yr > 2019)
                //        {
                //            dr1["Opening_Stock"] = fgen.make_double(lydt.Rows[0]["Closing_Stk"].ToString().Trim());//+ fgen.make_double(dt9.Rows[0]["opening"].ToString().Trim());
                //            db1 = fgen.make_double(dr1["Opening_Stock"].ToString().Trim());
                //        }
                //        else
                //        {
                //            dr1["Opening_Stock"] = fgen.make_double(dt.Rows[0]["opening"].ToString().Trim()) + fgen.make_double(dt9.Rows[0]["opening"].ToString().Trim());
                //            db1 = fgen.make_double(dr1["Opening_Stock"].ToString().Trim());
                //            //  dr1["Closing_Stock"] = fgen.make_double(dt.Rows[0]["Closing_Stk"].ToString().Trim());////only
                //            dr1["Closing_Stock"] = fgen.make_double(dt.Rows[0]["Closing_Stk"].ToString().Trim()) + fgen.make_double(dt9.Rows[0]["Closing_Stk"].ToString().Trim());
                //        }
                //        dtm.Rows.Add(dr1);
                //        foreach (DataRow dr0 in dtdrsim.Rows)
                //        {
                //            #region
                //            dr1 = dtm.NewRow();
                //            if (dt1.Rows.Count > 0)
                //            {
                //                DataView viewim = new DataView(dt1, "MRRDT='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //                dt4 = viewim.ToTable();//mrr view
                //            }
                //            if (dt7.Rows.Count > 0)   ///sale return view
                //            {
                //                DataView viewim3 = new DataView(dt7, "MRRDT='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //                dt8 = viewim3.ToTable();
                //            }
                //            if (dt2.Rows.Count > 0)  ////pur retrun  view
                //            {
                //                DataView viewim1 = new DataView(dt2, "pur_ret_DT='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //                dt5 = viewim1.ToTable();
                //            }
                //            if (dt3.Rows.Count > 0)   ///issue view
                //            {
                //                DataView viewim2 = new DataView(dt3, "invdt='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //                dt6 = viewim2.ToTable();
                //            }
                //            //filling value                                                                             
                //            for (int i = 0; i < dt4.Rows.Count; i++)
                //            {
                //                #region mrr
                //                dr1 = dtm.NewRow();
                //                dr1["sno"] = cnt + 1;
                //                if (i == 0)
                //                {
                //                    dr1["Txn_Date"] = dt4.Rows[i]["MRRDT"].ToString().Trim();
                //                }
                //                else
                //                {
                //                    dr1["Txn_Date"] = "";
                //                }
                //                dr1["Document_No"] = dt4.Rows[i]["BILLNO"].ToString().Trim();
                //                dr1["Party_Name"] = dt4.Rows[i]["party"].ToString().Trim();
                //                dr1["Purchase_QTY"] = fgen.make_double(dt4.Rows[i]["PUR_QTY"].ToString().Trim());
                //                db2 = fgen.make_double(dr1["Purchase_QTY"].ToString().Trim()); //a
                //                dr1["Sale_Return"] = 0;//new
                //                dr1["Pur_Return_Qty"] = 0;
                //                dr1["Issue_Qty"] = 0;
                //                if (i == 0)
                //                {
                //                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                else
                //                {
                //                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                dtm.Rows.Add(dr1);
                //                cnt++;
                //                #endregion
                //            }
                //            for (int i = 0; i < dt8.Rows.Count; i++)
                //            {
                //                #region mrr
                //                dr1 = dtm.NewRow();
                //                dr1["sno"] = cnt + 1;
                //                if (i == 0)
                //                {
                //                    dr1["Txn_Date"] = dt8.Rows[i]["MRRDT"].ToString().Trim();
                //                }
                //                else
                //                {
                //                    dr1["Txn_Date"] = "";
                //                }
                //                dr1["Document_No"] = "SaleReturn" + dt8.Rows[i]["billno"].ToString().Trim();
                //                dr1["Party_Name"] = dt8.Rows[i]["party"].ToString().Trim();
                //                dr1["Purchase_QTY"] = 0;
                //                dr1["Sale_Return"] = fgen.make_double(dt8.Rows[i]["sale_retrun_QTY"].ToString().Trim());//new
                //                db2 = fgen.make_double(dr1["Sale_Return"].ToString().Trim()); //a
                //                dr1["Pur_Return_Qty"] = 0;
                //                dr1["Issue_Qty"] = 0;
                //                if (i == 0)
                //                {
                //                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                else
                //                {
                //                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                dtm.Rows.Add(dr1);
                //                cnt++;
                //                #endregion
                //            }
                //            for (int i = 0; i < dt5.Rows.Count; i++)
                //            {
                //                #region pur return
                //                db2 = 0; db3 = 0;
                //                dr1 = dtm.NewRow();
                //                dr1["sno"] = cnt + 1;
                //                if (i == 0)
                //                {
                //                    dr1["Txn_Date"] = dt5.Rows[i]["pur_ret_DT"].ToString().Trim();
                //                }
                //                else
                //                {
                //                    dr1["Txn_Date"] = "";
                //                }
                //                dr1["Document_No"] = "PurReturn" + dt5.Rows[i]["pur_ret_no"].ToString().Trim();
                //                dr1["Party_Name"] = dt5.Rows[i]["party"].ToString().Trim();
                //                dr1["Purchase_QTY"] = 0;
                //                dr1["Sale_Return"] = 0;
                //                db2 = fgen.make_double(dr1["Purchase_QTY"].ToString().Trim());
                //                dr1["Pur_Return_Qty"] = dt5.Rows[i]["pur_Ret_qty"].ToString().Trim();
                //                db3 = fgen.make_double(dr1["Pur_Return_Qty"].ToString().Trim()); //b
                //                dr1["Issue_Qty"] = 0;
                //                if (i == 0)
                //                {
                //                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - db3, 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                else
                //                {
                //                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - db3, 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                dtm.Rows.Add(dr1);
                //                cnt++;
                //                #endregion
                //            }
                //            for (int i = 0; i < dt6.Rows.Count; i++)
                //            {
                //                db2 = 0; db3 = 0; db4 = 0;
                //                #region issue view
                //                dr1 = dtm.NewRow();
                //                dr1["sno"] = cnt + 1;
                //                if (i == 0)
                //                {
                //                    dr1["Txn_Date"] = dt6.Rows[i]["invdt"].ToString().Trim();
                //                }
                //                else
                //                {
                //                    dr1["Txn_Date"] = "";
                //                }
                //                dr1["Document_No"] = "Issue" + dt6.Rows[i]["invno"].ToString().Trim();
                //                dr1["Party_Name"] = dt6.Rows[i]["party"].ToString().Trim();
                //                dr1["Purchase_QTY"] = 0;
                //                dr1["Sale_Return"] = 0;
                //                dr1["Pur_Return_Qty"] = 0;
                //                dr1["Issue_Qty"] = dt6.Rows[i]["issue"].ToString().Trim();
                //                db2 = fgen.make_double(dr1["Purchase_QTY"].ToString().Trim());
                //                db3 = fgen.make_double(dr1["Pur_Return_Qty"].ToString().Trim());
                //                db4 = fgen.make_double(dr1["Issue_Qty"].ToString().Trim()); //c                               
                //                if (i == 0)
                //                {
                //                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - (db3 + db4), 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                else
                //                {
                //                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - (db3 + db4), 3);
                //                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                //                }
                //                dtm.Rows.Add(dr1);
                //                cnt++;
                //                #endregion
                //            }
                //            #endregion
                //        }
                //    }
                //    if (dtm.Rows.Count > 0)
                //    {
                //        oporow = null;
                //        oporow = dtm.NewRow();
                //        foreach (DataColumn dc in dtm.Columns)
                //        {
                //            to_cons = 0;
                //            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 9)
                //            {
                //            }
                //            else
                //            {
                //                mq1 = "sum(" + dc.ColumnName + ")";
                //                to_cons += fgen.make_double(dtm.Compute(mq1, "").ToString());
                //                oporow[dc] = to_cons;
                //            }
                //        }
                //        oporow["Txn_Date"] = "Grand Total";
                //        oporow["Closing_Stock"] = dtm.Rows[dtm.Rows.Count - 1]["Closing_Stock"].ToString().Trim();
                //        dtm.Rows.Add(oporow);
                //    }
                //    /////
                //    if (dtm.Rows.Count > 0)
                //    {
                //        Session["send_dt"] = dtm;
                //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                //        fgen.Fn_open_rptlevelJS("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                //    }
                //    else
                //    {
                //        fgen.msg("-", "AMSG", "Data Not Found");
                //    }
                #endregion
                //    break;



                case "F25133A"://NAHR REPORT=========this code taken from MG Mam 10/09/2020
                    #region
                    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();
                    dt6 = new DataTable(); dt7 = new DataTable(); dt8 = new DataTable(); dt9 = new DataTable();
                    dtm.Columns.Add("sno", typeof(int));
                    dtm.Columns.Add("Txn_Date", typeof(string));
                    dtm.Columns.Add("Document_No", typeof(string));
                    dtm.Columns.Add("Party_Name", typeof(string));
                    dtm.Columns.Add("Opening_Stock", typeof(double));
                    dtm.Columns.Add("Purchase_QTY", typeof(double));
                    dtm.Columns.Add("Sale_Return", typeof(double));
                    dtm.Columns.Add("Pur_Return_Qty", typeof(double));
                    dtm.Columns.Add("Issue_Qty", typeof(double));
                    dtm.Columns.Add("Closing_Stock", typeof(double));
                    //======================********************================================================
                    cond2 = "and substr(trim(icode),1,2) in ('07')";// AS PER ARVIND SIR
                    header_n = "RM Stock Report Inw/Out Wt";
                    int yr = Convert.ToInt32(frm_myear);
                    //int yr1 = yr++;
                    xprd3 = ""; string xprdrange2 = "";
                    DataTable lydt = new DataTable();
                    if (yr > 2019)
                    {
                        int yrr = yr - 1;
                        //xprd3 = "between to_date('01/04/" + yrr + "','dd/MM/yyyy') and to_Date('01/04/" + yrr + "','dd/MM/yyyy')-1";
                        //xprdrange2 = " between to_date('01/04/" + yrr + "','dd/mm/yyyy') and to_date('31/03/" + yr + "','dd/mm/yyyy')";
                        xprd3 = "between to_date('01/04/2019','dd/MM/yyyy') and to_Date('01/04/2019','dd/MM/yyyy')-1";
                        xprdrange2 = " between to_date('01/04/2019','dd/mm/yyyy') and to_date('31/03/" + yr + "','dd/mm/yyyy')";
                        // SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from ( select sum(a.opening) as opening,0 as Rcpt,0 as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2)='07'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y' and substr(trim(icode),1,2)='07' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + "  and store='Y' and substr(trim(icode),1,2)='07'  GROUP BY trim(icode) ,branchcd) a having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 union all select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) union all select 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') and a.store='Y' )   GROUP BY invno,invdt,acode,party  order by invno,invdt) )"; //old
                        //  SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,Sum((a.opening+a.Rcpt)-a.Issued) as Closing_Stk from (select sum((case when substr(trim(a.icode),1,2)='02' then round((a.opening * b.iweight),3) else a.opening  end )) as opening,0 as Rcpt,0 as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2)='07'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y' and substr(trim(icode),1,2) in ('02','07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + "  and store='Y' and substr(trim(icode),1,2)='07'  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) ) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0   union all select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) union all select 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum((case when substr(trim(a.icode),1,2)='02' then round((a.iqtyout * c.iweight),3) else a.iqtyout  end )) as pur_Ret_qty FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') and a.store='Y' )   GROUP BY invno,invdt,acode,party  order by invno,invdt) )";
                        //SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,Sum((a.opening+a.Rcpt)-a.Issued) as Closing_Stk from (select sum((case when substr(trim(a.icode),1,2)='02' then round((a.opening * b.iweight),3) else a.opening  end )) as opening,0 as Rcpt,0 as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2) in ('02','07')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y' and substr(trim(icode),1,2) in ('02','07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + "  and store='Y' and substr(trim(icode),1,2) in ('02','07')  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) ) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0   union all select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate  " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) union all select 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum((case when substr(trim(a.icode),1,2)='02' then round((a.iqtyout * c.iweight),3) else a.iqtyout  end )) as pur_Ret_qty FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE  " + xprdrange2 + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') and a.store='Y' )   GROUP BY invno,invdt,acode,party  order by invno,invdt) )";
                        //this is as per client to check last yr closing
                        // SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk  from (select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2) in ('07')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + " and store='Y' and substr(trim(icode),1,2) in ('07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange2 + " and store='Y' and substr(trim(icode),1,2) in ('07')  GROUP BY trim(icode) ,branchcd) A) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0 UNION ALL select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) UNION ALL SELECT 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + ") GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange2 + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all  select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE " + xprdrange2 + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE  " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt))";
                        //SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk  from (select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2) in ('07')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + " and store='Y' and substr(trim(icode),1,2) in ('07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange2 + " and store='Y' and substr(trim(icode),1,2) in ('07')  GROUP BY trim(icode) ,branchcd) A) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0 UNION ALL select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) UNION ALL SELECT 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','05','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + ") GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange2 + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all  select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE " + xprdrange2 + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE  " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt))";//old having iweight
                        SQuery = "select sum(opening) as opening,sum(Rcpt) as Rcpt,sum(Issued) as Issued,(sum(opening)+sum(Rcpt))-sum(Issued) as Closing_Stk from (select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk  from (select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2019 as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,2) in ('07')  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + " and store='Y' and substr(trim(icode),1,2) in ('07') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange2 + " and store='Y' and substr(trim(icode),1,2) in ('07')  GROUP BY trim(icode) ,branchcd) A) a having sum(a.opening)+sum(a.rcpt)+sum(a.Issued)<>0 UNION ALL select sum(opening) as opening,0  as rcpt,0 as issued,sum(closing_stk) as closing_stk from (select a.icode,sum(a.opening*b.FG_FIX_WT) as opening,sum(a.cdr*b.FG_FIX_WT) as Rcpt,sum(a.ccr*b.FG_FIX_WT) as Issued,Sum(((a.opening*b.FG_FIX_WT)+(a.cdr*b.FG_FIX_WT))-(a.ccr*b.FG_FIX_WT)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_2019  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprd3 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange2 + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) UNION ALL SELECT 0 as opening,sum(pur_qty) as rcpt,0 as issued,0 as closing_stk from (SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.FG_FIX_WT,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','05','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange2 + ") GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc) union all select 0 as opening,sum(sale_retrun_QTY) as rcpt,0 as issued,0 as closing_stk from (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.FG_FIX_WT,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange2 + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc) union all  select 0 as opening,0 as rcpt,sum(pur_Ret_qty) as issued,0 as closing_stk  from (SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE " + xprdrange2 + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc) union all select 0 as opening,0 as rcpt,sum(issue) as issued,0 as closing_stk from (select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.FG_FIX_WT,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE  " + xprdrange2 + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt))";
                        lydt = new DataTable();
                        lydt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }

                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    SQuery = "SELECT distinct TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,to_char(a.vchdate,'yyyyMMdd') as vdd FROM IVOUCHER A WHERE A.BRANCHCD='" + mbr + "' AND (A.TYPE IN ('02','05','07','47','24') OR SUBSTR(A.TYPE,1,1)='4' ) AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR  SUBSTR(TRIM(A.ICODE),1,1)='9' ) AND A.VCHDATE " + xprdrange + " order by vdd";
                    ph_tbl = new DataTable();
                    ph_tbl = fgen.getdata(frm_qstr, co_cd, SQuery); //dt for view on date
                    //////////////////////////
                    if (yr < 2020)
                    {
                        // mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + "  GROUP BY trim(icode) ,branchcd) a having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                        // mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,Sum((a.opening+a.Rcpt)-a.Issued) as Closing_Stk from (select sum((case when substr(trim(a.icode),1,2)='02' then round((a.opening * b.iweight),3) else a.opening  end )) as opening,sum( (case when substr(trim(a.icode),1,2)='02' then round((a.cdr * b.iweight),3) else a.cdr  end )) as Rcpt,sum((case when substr(trim(a.icode),1,2)='02' then round((a.ccr * b.iweight),3) else a.ccr  end )) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + "  GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode)) a having sum(a.opening)+sum(a.Rcpt)+sum(a.Issued)<>0  ";
                        //dt = fgen.getdata(frm_qstr, co_cd, mq1);//stock dt for 07 mg === 02 ka iweight se multiply + 07 as it is
                        //as per client mam===client old op matching
                        mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + "  GROUP BY trim(icode) ,branchcd) a having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                        dt = fgen.getdata(frm_qstr, co_cd, mq1);//stock dt for 07 mg === 02 ka iweight se multiply + 07 as it is
                        ///=============

                        //mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";                        
                        //dt9 = fgen.getdata(frm_qstr, co_cd, mq1);//STOCK DT FOR ICODE LIKE 9 mg === ok remove 96
                        //as per client mam===client old op matching
                        // mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) "; //old iweight
                        mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.FG_FIX_WT) as opening,sum(a.cdr*b.FG_FIX_WT) as Rcpt,sum(a.ccr*b.FG_FIX_WT) as Issued,Sum(((a.opening*b.FG_FIX_WT)+(a.cdr*b.FG_FIX_WT))-(a.ccr*b.FG_FIX_WT)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";
                        dt9 = fgen.getdata(frm_qstr, co_cd, mq1);//STOCK DT FOR ICODE LIKE 9 mg === ok remove 96
                    }
                    #region olddd
                    ////=============MRR DT
                    //mq2 = "SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange + " and a.store='Y') GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc";
                    //dt1 = fgen.getdata(frm_qstr, co_cd, mq2);//mrr dt.......02,05,07 only
                    ////============================
                    //mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange + "  and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc";
                    //dt7 = fgen.getdata(frm_qstr, co_cd, mq5);//mrr dt.......04 only
                    ////==================
                    ////  mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE " + xprdrange + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                    //mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum((case when substr(trim(a.icode),1,2)='02' then round((a.iqtyout * c.iweight),3) else a.iqtyout  end )) as pur_Ret_qty FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2) in ('02','07') AND A.VCHDATE " + xprdrange + " and a.store='Y' GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                    //dt2 = fgen.getdata(frm_qstr, co_cd, mq3);//pur retrun dt....47 and 24 only
                    ////====================issue dt                   
                    //mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " and a.store='Y' AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt";
                    //dt3 = fgen.getdata(frm_qstr, co_cd, mq4);///issue dt
                    #endregion

                    //mq2 = "SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','05','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange + ") GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc";//old...iweght in this
                    mq2 = "SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.FG_FIX_WT,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','05','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange + ") GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq2);//mrr dt.......02,05,07 only
                    //============================
                    //mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc";//old iweight
                    mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.FG_FIX_WT,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq5);//mrr dt.......04 only
                    //==================
                    mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE " + xprdrange + "  GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq3);//pur retrun dt....47 and 24 only
                    //====================issue dt                   
                    //mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt";//old iweght
                    mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.FG_FIX_WT,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq4);///issue dt
                    int cnt = 0;
                    #region below qry changed by MG Mam...correect
                    //cond2 = "and substr(trim(icode),1,2)='07'";// AS PER ARVIND SIR
                    //header_n = "RM Stock Report Inw/Out Wt";
                    //xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    //SQuery = "SELECT distinct TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,to_char(a.vchdate,'yyyyMMdd') as vdd FROM IVOUCHER A WHERE A.BRANCHCD='" + mbr + "' AND (A.TYPE IN ('02','05','07','47','24') OR SUBSTR(A.TYPE,1,1)='4' ) AND (SUBSTR(TRIM(A.ICODE),1,2) in ('07','02') OR  SUBSTR(TRIM(A.ICODE),1,1)='9' ) AND A.VCHDATE " + xprdrange + " order by vdd";
                    //ph_tbl = new DataTable();
                    //ph_tbl = fgen.getdata(frm_qstr, co_cd, SQuery); //dt for view on date
                    ////////////////////////////
                    //mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + "  GROUP BY trim(icode) ,branchcd) a having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                    //dt = fgen.getdata(frm_qstr, co_cd, mq1);//stock dt for 07 mg === 02 ka iweight se multiply + 07 as it is
                    /////=============
                    //mq1 = "select sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9'  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y'  and substr(trim(icode),1,1)='9'  GROUP BY trim(icode) ,branchcd) a  having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 ";
                    //mq1 = "select sum(opening) as opening,sum(rcpt) as rcpt,sum(issued) as issued,sum(closing_stk) as closing_stk from  (select a.icode,sum(a.opening*b.iweight) as opening,sum(a.cdr*b.iweight) as Rcpt,sum(a.ccr*b.iweight) as Issued,Sum(((a.opening*b.iweight)+(a.cdr*b.iweight))-(a.ccr*b.iweight)) as Closing_Stk from  (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y'  and substr(trim(icode),1,1)='9' and substr(icode,1,2) not in ('99','96') GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' /*and substr(trim(icode),1,1)='9'*/ and substr(icode,1,2)!='99' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) group by a.icode ) ";
                    //dt9 = fgen.getdata(frm_qstr, co_cd, mq1);//STOCK DT FOR ICODE LIKE 9 mg === ok remove 96
                    ////=============MRR DT

                    //mq2 = "SELECT MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party,SUM(PUR_QTY_OLD) AS PUR_QTY_OLD,SUM(PUR_QTY) AS PUR_QTY FROM (SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,A.IQTYIN AS PUR_QTY_OLD,(case when SUBSTR(TRIM(A.ICODE),1,2)='02' then nvl(a.iqtyin,0)*nvl(c.iweight,0) else nvl(a.iqtyin,0) end) as PUR_QTY  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('02','07') AND SUBSTR(TRIM(A.ICODE),1,2) IN ('02','07') AND A.VCHDATE " + xprdrange + ") GROUP BY  MRRNO,MRRDT,BILLNO,BILLDT,ACODE,party order by MRRDT asc";
                    //dt1 = fgen.getdata(frm_qstr, co_cd, mq2);//mrr dt.......02,05,07 only
                    ////============================
                    //mq5 = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,A.INVNO AS BILLNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS BILLDT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,SUM(nvl(A.IQTYIN,0)*nvl(c.iweight,0)) AS sale_retrun_QTY  FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='04' AND A.VCHDATE " + xprdrange + " GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,A.INVNO ,TO_CHAR(A.INVDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by MRRDT asc";
                    //dt7 = fgen.getdata(frm_qstr, co_cd, mq5);//mrr dt.......04 only
                    ////==================
                    //mq3 = "SELECT trim(A.VCHNUM)  AS pur_ret_no,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS pur_ret_DT,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,sum(a.iqtyout) as pur_Ret_qty FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN ('47','24') AND SUBSTR(TRIM(A.ICODE),1,2)='07' AND A.VCHDATE " + xprdrange + "  GROUP BY  A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),trim(b.aname) order by pur_ret_DT asc";
                    //dt2 = fgen.getdata(frm_qstr, co_cd, mq3);//pur retrun dt....47 and 24 only
                    ////====================issue dt                   
                    //mq4 = "select invno,invdt,acode,party,sum(issue) as issue from (SELECT trim(A.VCHNUM)  AS invno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS invdt ,TRIM(A.ACODE) AS ACODE,trim(b.aname) as party,nvl(a.iqtyout,0)*nvl(c.iweight,0) as issue ,trim(a.icode) as icode FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + mbr + "' AND substr(a.type,1,1)='4' and a.type not in ('47','45','49') AND A.VCHDATE " + xprdrange + " AND (SUBSTR(A.ICODE,1,2)!='96' and SUBSTR(A.ICODE,1,1)!='7') )   GROUP BY invno,invdt,acode,party  order by invno,invdt";
                    //dt3 = fgen.getdata(frm_qstr, co_cd, mq4);///issue dt
                    //cnt = 0;
                    //=================================***********===============================
                    #endregion
                    if (ph_tbl.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(ph_tbl);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "vchdate"); //MAIN      
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0;
                        //===================
                        dr1 = dtm.NewRow();
                        //   dr1["Opening_Stock"] = fgen.make_double(dt.Rows[0]["opening"].ToString().Trim());
                        if (yr > 2019)
                        {
                            dr1["Opening_Stock"] = Math.Round(fgen.make_double(lydt.Rows[0]["Closing_Stk"].ToString().Trim()), 3);//+ fgen.make_double(dt9.Rows[0]["opening"].ToString().Trim());
                            db1 = fgen.make_double(dr1["Opening_Stock"].ToString().Trim());
                        }
                        else
                        {
                            dr1["Opening_Stock"] = Math.Round(fgen.make_double(dt.Rows[0]["opening"].ToString().Trim()) + fgen.make_double(dt9.Rows[0]["opening"].ToString().Trim()), 3);
                            db1 = fgen.make_double(dr1["Opening_Stock"].ToString().Trim());
                            // dr1["Closing_Stock"] = fgen.make_double(dt.Rows[0]["Closing_Stk"].ToString().Trim());////only
                            dr1["Closing_Stock"] = Math.Round(fgen.make_double(dt.Rows[0]["Closing_Stk"].ToString().Trim()) + fgen.make_double(dt9.Rows[0]["Closing_Stk"].ToString().Trim()), 3);
                        }
                        dtm.Rows.Add(dr1);
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            #region
                            dr1 = dtm.NewRow();
                            if (dt1.Rows.Count > 0)
                            {
                                DataView viewim = new DataView(dt1, "MRRDT='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt4 = viewim.ToTable();//mrr view
                            }
                            if (dt7.Rows.Count > 0)   ///sale return view
                            {
                                DataView viewim3 = new DataView(dt7, "MRRDT='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt8 = viewim3.ToTable();
                            }
                            if (dt2.Rows.Count > 0)  ////pur retrun  view
                            {
                                DataView viewim1 = new DataView(dt2, "pur_ret_DT='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt5 = viewim1.ToTable();
                            }
                            if (dt3.Rows.Count > 0)   ///issue view
                            {
                                DataView viewim2 = new DataView(dt3, "invdt='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt6 = viewim2.ToTable();
                            }
                            //filling value                                                                             
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                #region mrr
                                dr1 = dtm.NewRow();
                                dr1["sno"] = cnt + 1;
                                if (i == 0)
                                {
                                    dr1["Txn_Date"] = dt4.Rows[i]["MRRDT"].ToString().Trim();
                                }
                                else
                                {
                                    dr1["Txn_Date"] = "";
                                }
                                dr1["Document_No"] = dt4.Rows[i]["BILLNO"].ToString().Trim();
                                dr1["Party_Name"] = dt4.Rows[i]["party"].ToString().Trim();
                                dr1["Purchase_QTY"] = fgen.make_double(dt4.Rows[i]["PUR_QTY"].ToString().Trim());
                                db2 = fgen.make_double(dr1["Purchase_QTY"].ToString().Trim()); //a
                                dr1["Sale_Return"] = 0;//new
                                dr1["Pur_Return_Qty"] = 0;
                                dr1["Issue_Qty"] = 0;
                                if (i == 0)
                                {
                                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                else
                                {
                                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                dtm.Rows.Add(dr1);
                                cnt++;
                                #endregion
                            }
                            for (int i = 0; i < dt8.Rows.Count; i++)
                            {
                                #region mrr
                                dr1 = dtm.NewRow();
                                dr1["sno"] = cnt + 1;
                                if (i == 0)
                                {
                                    dr1["Txn_Date"] = dt8.Rows[i]["MRRDT"].ToString().Trim();
                                }
                                else
                                {
                                    dr1["Txn_Date"] = "";
                                }
                                dr1["Document_No"] = "SaleReturn" + dt8.Rows[i]["billno"].ToString().Trim();
                                dr1["Party_Name"] = dt8.Rows[i]["party"].ToString().Trim();
                                dr1["Purchase_QTY"] = 0;
                                dr1["Sale_Return"] = fgen.make_double(dt8.Rows[i]["sale_retrun_QTY"].ToString().Trim());//new
                                db2 = fgen.make_double(dr1["Sale_Return"].ToString().Trim()); //a
                                dr1["Pur_Return_Qty"] = 0;
                                dr1["Issue_Qty"] = 0;
                                if (i == 0)
                                {
                                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                else
                                {
                                    dr1["Closing_Stock"] = Math.Round(db1 + db2, 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                dtm.Rows.Add(dr1);
                                cnt++;
                                #endregion
                            }
                            for (int i = 0; i < dt5.Rows.Count; i++)
                            {
                                #region pur return
                                db2 = 0; db3 = 0;
                                dr1 = dtm.NewRow();
                                dr1["sno"] = cnt + 1;
                                if (i == 0)
                                {
                                    dr1["Txn_Date"] = dt5.Rows[i]["pur_ret_DT"].ToString().Trim();
                                }
                                else
                                {
                                    dr1["Txn_Date"] = "";
                                }
                                dr1["Document_No"] = "PurReturn" + dt5.Rows[i]["pur_ret_no"].ToString().Trim();
                                dr1["Party_Name"] = dt5.Rows[i]["party"].ToString().Trim();
                                dr1["Purchase_QTY"] = 0;
                                dr1["Sale_Return"] = 0;
                                db2 = fgen.make_double(dr1["Purchase_QTY"].ToString().Trim());
                                dr1["Pur_Return_Qty"] = dt5.Rows[i]["pur_Ret_qty"].ToString().Trim();
                                db3 = fgen.make_double(dr1["Pur_Return_Qty"].ToString().Trim()); //b
                                dr1["Issue_Qty"] = 0;
                                if (i == 0)
                                {
                                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - db3, 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                else
                                {
                                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - db3, 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                dtm.Rows.Add(dr1);
                                cnt++;
                                #endregion
                            }
                            for (int i = 0; i < dt6.Rows.Count; i++)
                            {
                                db2 = 0; db3 = 0; db4 = 0;
                                #region issue view
                                dr1 = dtm.NewRow();
                                dr1["sno"] = cnt + 1;
                                if (i == 0)
                                {
                                    dr1["Txn_Date"] = dt6.Rows[i]["invdt"].ToString().Trim();
                                }
                                else
                                {
                                    dr1["Txn_Date"] = "";
                                }
                                dr1["Document_No"] = "Issue" + dt6.Rows[i]["invno"].ToString().Trim();
                                dr1["Party_Name"] = dt6.Rows[i]["party"].ToString().Trim();
                                dr1["Purchase_QTY"] = 0;
                                dr1["Sale_Return"] = 0;
                                dr1["Pur_Return_Qty"] = 0;
                                dr1["Issue_Qty"] = dt6.Rows[i]["issue"].ToString().Trim();
                                db2 = fgen.make_double(dr1["Purchase_QTY"].ToString().Trim());
                                db3 = fgen.make_double(dr1["Pur_Return_Qty"].ToString().Trim());
                                db4 = fgen.make_double(dr1["Issue_Qty"].ToString().Trim()); //c                               
                                if (i == 0)
                                {
                                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - (db3 + db4), 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                else
                                {
                                    dr1["Closing_Stock"] = Math.Round((db1 + db2) - (db3 + db4), 3);
                                    db1 = fgen.make_double(dr1["Closing_Stock"].ToString().Trim());
                                }
                                dtm.Rows.Add(dr1);
                                cnt++;
                                #endregion
                            }
                            #endregion
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        oporow = null;
                        oporow = dtm.NewRow();
                        foreach (DataColumn dc in dtm.Columns)
                        {
                            to_cons = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 9)
                            {
                            }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                to_cons += fgen.make_double(dtm.Compute(mq1, "").ToString());
                                oporow[dc] = to_cons;
                            }
                        }
                        oporow["Txn_Date"] = "Grand Total";
                        oporow["Closing_Stock"] = dtm.Rows[dtm.Rows.Count - 1]["Closing_Stock"].ToString().Trim();
                        dtm.Rows.Add(oporow);
                    }
                    /////
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Data Not Found");
                    }
                    #endregion
                    break;

                case "F25169":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Length <= 4)
                    {
                        mq0 = "and trim(a.acode) like '%'";
                    }
                    else { mq0 = "and trim(a.acode) in (" + party_cd + ")"; }
                    SQuery = "select a.icode as item_code,trim(b.iname) as description,a.desc_ as job,a.unit,a.iqtyout as qty_rcv,a.iqty_wt as wt_rcv,a.irate as job_rate,a.iamount as job_value,a.no_bdls as WO_no, a.exc_amt as  matl_unit,to_char(a.rtn_date,'dd/mm/yyyy') as tent_dlv_dt,c.st_entform as eway_bill_no from rgpmst a, item b ,ivoucher c where trim(a.icode)=trim(b.icode) and trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(c.type)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy') and a.type LIKE '2%' and a.branchcd='" + mbr + "' " + mq0 + " and a.vchdate " + xprdrange + "";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Incoming Material Checklist", frm_qstr);
                    break;

         

                case "F25132A":
                case "F25135A":
                case "F15314A":
                case "F15314B":
                case "F76004":
                case "F58004":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Trim().Length <= 1)
                    {
                        party_cd = "%";
                    }
                    if (part_cd.Trim().Length <= 1)
                    {
                        part_cd = "%";
                    }
                    string criticalItems = "N";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") == "N") criticalItems = "Y";

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";


                    xbstring = "branchcd='" + mbr + "'";

                    string cond_store = "Y", cond_ibal = "1=1", cond_head = "Main Stores ";
                    if (val == "F25135A")
                    {
                        cond_store = "R";
                        cond_ibal = "1=2";
                        cond_head = "Rejection Stores ";
                    }
                    my_rep_head = cond_head + "Stock Summary During " + value1 + " To " + value2 + "";

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    if (val == "F15314B") {


                        DataTable dtt = Gen_stk_pl_S(todt, cond);
                    }
                    else if (val == "F76004" || val == "F58004")
                    {
                        cond_store = "Y";
                        cond_ibal = "1=1";
                        cond_head = "Finished Goods Stock";
                        s_code1 = "90000000";
                        s_code2 = "99999999";

                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) s_code1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                        {
                            s_code1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
                            s_code2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");
                        }

                        mq0 = "select trim(a.Icode) as FSTR,'-' as GSTR,(case when trim(A.Icode)='Total' then 'Report Total' else b.Iname end) as Item_Name,sum(a.opening) as Opening_Bal,sum(a.cdr) as Inward_Qty,sum(a.ccr) as Outward_Qty,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,max(a.Locn) as Bin_Location,b.Unit,b.no_proc as sec_unit,c.Aname as Primary_Customer,b.prefx as Cust_ref,b.Cpartno,trim(a.Icode) as Icode,substr(trim(a.Icode),1,2) as Grp,substr(trim(a.Icode),1,4) as Sub_Grp,a.Dset,b.SERVICABLE as critical_item from ( ";
                        mq1 = "Select 'Total' as Icode, nvl(YR_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos,'-' as locn,'S1' as Dset from itembal where " + branch_Cd + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' and " + cond_ibal + " union all select 'Total' as Icode,iqtyin-iqtyout as op,0 as cdr,0 as ccr,0 as clos,'-' as locn,'S1' as Dset from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' and store='" + cond_store + "' union all select 'Total' as Icode,0 as op,iqtyin as cdr,iqtyout as ccr,0 as clos,'-' as locn,'S1' as Dset from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd2 + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' and trim(icode) like '" + part_cd + "' and store='" + cond_store + "' union all ";
                        mq2 = "Select Icode, nvl(YR_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos,nvl(binno,'-') as Bin_Locn,'S2' as Dset from itembal where " + branch_Cd + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' and trim(icode) like '" + part_cd + "%' union all select Icode,iqtyin-iqtyout as op,0 as cdr,0 as ccr,0 as clos,null as locn,'S2' as Dset from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and icode between '" + s_code1 + "' and '" + s_code2 + "'  and store='" + cond_store + "' union all select icode,0 as op,iqtyin as cdr,iqtyout as ccr,0 as clos,null as locn,'S2' as Dset from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd2 + " and icode between '" + s_code1 + "' and '" + s_code2 + "'  and store='" + cond_store + "' )a left outer join item b on trim(A.icode)=trim(B.icodE) left outer join famst c on trim(b.prefx)=trim(c.acodE) group by Dset,c.aname,b.prefx,b.iname,b.unit,b.no_proc,b.cpartno,trim(a.icode),substr(trim(a.icode),1,2),substr(trim(a.icode),1,4),b.SERVICABLE  having sum(abs(a.opening))+sum(a.cdr)+sum(a.ccr)!=0 order by Dset,substr(trim(a.icode),1,4),b.Iname";
                    }
                    else
                    {
                        s_code1 = "00000000";
                        s_code2 = "99999999";

                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) s_code1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                        {
                            s_code1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
                            s_code2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");
                        }

                        mq0 = "select trim(a.Icode) as FSTR,'-' as GSTR,(case when trim(A.Icode)='Total' then 'Report Total' else b.Iname end) as Item_Name,sum(a.opening) as Opening_Bal," +
                            "sum(a.cdr) as Inward_Qty,sum(a.ccr) as Outward_Qty,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,max(a.Locn) as Bin_Location,b.Unit,b.no_proc as sec_unit," +
                            "b.Cpartno,max(b.maker) as Make,trim(a.Icode) as Icode,sum(a.imin) AS Min_lvl,sum(a.imax) As Max_lvl,sum(a.iord) as Re_ord_lvl,substr(trim(a.Icode),1,2) as " +
                            "Grp,substr(trim(a.Icode),1,4) as Sub_Grp,a.Dset,b.SERVICABLE as critical_item from ( ";
                        mq1 = "Select 'Total' as Icode, nvl(YR_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos,'-' as locn,'S1' as Dset,0 as imin,0 as imax,0 as iord from itembal where " + branch_Cd + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' and " + cond_ibal + " and trim(icode) like '" + part_cd + "%' union all select 'Total' as Icode,iqtyin-iqtyout as op,0 as cdr,0 as ccr,0 as clos,'-' as locn,'S1' as Dset,0 as minl,0 as mxl,0 as mrol from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' and store='" + cond_store + "' and trim(icode) like '" + part_cd + "%' union all select 'Total' as Icode,0 as op,iqtyin as cdr,iqtyout as ccr,0 as clos,'-' as locn,'S1' as Dset,0 as minl,0 as mxl,0 as mrol from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd2 + " and Icode between '" + s_code1 + "' and '" + s_code2 + "'  and store='" + cond_store + "' and trim(icode) like '" + part_cd + "%' union all ";
                        mq2 = "Select Icode, nvl(YR_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos,nvl(binno,'-') " +
                            "as Bin_Locn,'S2' as Dset,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from " +
                            "itembal where " + branch_Cd + " and Icode between '" + s_code1 + "' and '" + s_code2 + "' " +
                            "and trim(icode) like '" + part_cd + "%' and " + cond_ibal + " union all select Icode," +
                            "iqtyin-iqtyout as op,0 as cdr,0 as ccr,0 as clos,null as locn,'S2' as Dset,0 as minl,0 as mxl," +
                            "0 as mrol from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and icode between '" + s_code1 + "' and '" + s_code2 + "'  and store='" + cond_store + "' and trim(icode) like '" + part_cd + "%' union all select icode,0 as op,iqtyin as cdr,iqtyout as ccr,0 as clos,null as locn,'S2' as Dset,0 as minl,0 as mxl,0 as mrol from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd2 + " and icode between '" + s_code1 + "' and '" + s_code2 + "'  and store='" + cond_store + "' and trim(icode) like '" + part_cd + "%')a left outer join item b on trim(A.icode)=trim(B.icodE) and trim(a.icode) like '" + part_cd + "%' group by Dset,b.iname,b.unit,b.no_proc,b.cpartno,trim(a.icode),substr(trim(a.icode),1,2),substr(trim(a.icode),1,4),b.SERVICABLE having sum(abs(a.opening))+sum(a.cdr)+sum(a.ccr)!=0 order by Dset,substr(trim(a.icode),1,4),b.Iname";

                    }



                    fgen.drillQuery(0, mq0 + mq1 + mq2, frm_qstr, "4#5#6#7#", "3#4#5#6#7#", "400#100#100#100#100#");
                    fgen.drillQuery(1, "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS Inward_Qty,SUM(CRAMT) AS Outward_Qty,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(ICODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(iqtyin) as debits,(iqtyout) as credits,0 as mthsno FROM iVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ICODE)='FSTR' and store='" + cond_store + "' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr, "4#5#6#", "3#4#5#6#", "200#200#400");

                    fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ICODE) AS GSTR,(case when nvl(b.aname,'-')='-' then x.name else b.aname end) AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.iqtyin) AS Inward_Qty,(a.iqtyout) AS Outward_Qty,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.ENT_BY,A.BRANCHCD as PL_CODE FROM IVOUCHER A left outer join FAMST B on TRIM(a.ACODE)=TRIM(b.ACODE) left outer join type x on trim(a.type)=trim(x.type1) and x.id='M' where A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " and a.store='" + cond_store + "' ORDER BY A.VCHNUM) WHERE GSTR='FSTR' ", frm_qstr, "5#6#", "3#4#5#6#7#8#9#10", "220#70#100#100#30#50#200#30#");

                    cond = "";

                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";

                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;


                case "F25170":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Length <= 4)
                    {
                        mq0 = "and trim(a.acode) like '%'";
                    }
                    else { mq0 = "and trim(a.acode) in (" + party_cd + ")"; }
                    SQuery = "select trim(a.vchnum) as packing_no ,to_char(a.vchdate,'dd/mm/yyyy') as packing_date,trim(b.aname) as buyer_name,trim(c.aname) as consignee,trim(a.col1) as pkg_frm_to,trim(a.col2) as sku_code,trim(a.col3) as main_heading,trim(a.col4) as description,trim(a.col6) as data_sheet,trim(a.col7) as tag_no,trim(a.col8) as box_dimension,trim(a.col9) as qty_box,trim(a.col10) as net_wt,trim(a.col21) as gross_wt,trim(a.col22) as l,trim(a.col27) as b,trim(a.col28) as h,trim(a.col29) as unit,trim(a.col30) as box_cbm from scratch a, famst b , csmst c where trim(a.acode)=trim(b.acode) and trim(a.cscode)=trim(c.acode) and  a.branchcd='" + mbr + "' and a.type='PN' " + mq0 + " and a.vchdate " + xprdrange + " order by packing_no,srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Packing Note Checklist", frm_qstr);
                    break;

                case "F25160":
                    #region SEFL Job Work Finished Goods
                    dtm = new DataTable();
                    dtm.Columns.Add("DATE", typeof(string));
                    dtm.Columns.Add("OUT_CHL_NO_VCH_NO", typeof(string));
                    dtm.Columns.Add("PARTICULARS", typeof(string));
                    dtm.Columns.Add("INW_CHL_NO_OUT_BILL_NO", typeof(string));
                    dtm.Columns.Add("INW_CHL_NO_OUT_BILL_DATE", typeof(string));
                    dtm.Columns.Add("UNIT_TYPE", typeof(string));
                    dtm.Columns.Add("RECEIPT_QTY", typeof(double));
                    dtm.Columns.Add("ISSUE_QTY", typeof(double));
                    dtm.Columns.Add("BALANCE", typeof(double));
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (party_cd.Length <= 1)
                    {
                        mq2 = " and a.acode like '%'";
                    }
                    else
                    {
                        mq2 = " and a.acode='" + party_cd + "'";
                    }

                    if (part_cd.Length <= 1)
                    {
                        mq3 = " and a.icode like '%'";
                    }
                    else
                    {
                        mq3 = " and a.icode='" + part_cd + "'";
                    }
                    mq1 = "select trim(vchnum) as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,trim(tc_no) as challan,to_char(refdate,'dd/mm/yyyy') as chldate from ivoucher where branchcd='" + mbr + "' and type='41' and vchdate " + xprdrange + "";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    mq0 = "select a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.aname,a.icode,a.iname,sum(a.iqtyin) as iqtyin,sum(a.iqtyout) as iqtyout,a.desc_,trim(a.genum) as genum,to_char(a.gedate,'dd/mm/yyyy') as gedate,to_char(a.vchdate,'yyyymmdd') as vdd from (select a.type,trim(a.vchnum) as vchnum,a.vchdate,a.acode,t.name as aname,trim(a.icode) as icode,i.iname,0 as iqtyout,a.iqtyin,a.desc_,trim(a.genum) as genum,a.gedate from ivoucher a,type t,item i where trim(a.type)=trim(t.type1) and t.id='M' and trim(a.icode)=trim(i.icode) and a.branchcd='" + mbr + "' and a.type='13' and a.vchdate " + xprdrange + mq3 + " and nvl(trim(a.genum),'-')!='-' union all select a.type,trim(a.vchnum) as vchnum,a.vchdate,a.acode,f.aname,trim(a.icode) as icode,i.iname,a.iqtyout,0 as iqtyin,a.desc_,trim(a.invno) as invno,a.invdate from ivoucher a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + mbr + "' and a.type='25' and a.vchdate " + xprdrange + mq2 + mq3 + ")a group by a.type,trim(a.vchnum),to_char(a.vchdate,'dd/mm/yyyy'),a.acode,a.aname,a.icode,a.iname,a.desc_,trim(a.genum),to_char(a.gedate,'dd/mm/yyyy'),to_char(a.vchdate,'yyyymmdd') order by vdd";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    string[] challan;
                    oporow = null;

                    if (dt.Rows.Count > 0)
                    {
                        view1 = new DataView(dt);
                        dticode = new DataTable();
                        dticode = view1.ToTable(true, "icode", "iname");
                        foreach (DataRow dr in dticode.Rows)
                        {
                            DataView view3 = new DataView(dt, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = new DataTable();
                            dt3 = view3.ToTable(true, "genum", "gedate", "icode");
                            db6 = 0;
                            foreach (DataRow dr1 in dt3.Rows)
                            {
                                view2 = new DataView(dt, "genum='" + dr1["genum"].ToString().Trim() + "' and gedate='" + dr1["gedate"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode2 = new DataTable();
                                dticode2 = view2.ToTable();
                                dt2 = new DataTable();

                                db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
                                for (int i = 0; i < dticode2.Rows.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        oporow = dtm.NewRow();
                                        oporow["Particulars"] = dticode2.Rows[i]["iname"].ToString().Trim() + " (" + dticode2.Rows[i]["icode"].ToString().Trim() + ")";
                                        dtm.Rows.Add(oporow);
                                    }
                                    oporow = dtm.NewRow();
                                    oporow["out_chl_no_vch_no"] = dticode2.Rows[i]["vchnum"].ToString().Trim();// vchnum of both 13 and 25 type
                                    oporow["date"] = dticode2.Rows[i]["vchdate"].ToString().Trim();
                                    oporow["particulars"] = dticode2.Rows[i]["aname"].ToString().Trim();
                                    if (dticode2.Rows[i]["type"].ToString().Trim() == "13")
                                    {
                                        challan = dticode2.Rows[i]["desc_"].ToString().Trim().Split('|');
                                        oporow["inw_chl_no_out_bill_no"] = challan[0];
                                        oporow["inw_chl_no_out_bill_date"] = challan[1];
                                    }
                                    else
                                    {
                                        oporow["inw_chl_no_out_bill_no"] = fgen.seek_iname_dt(dt1, " challan='" + dticode2.Rows[i]["vchnum"].ToString().Trim() + "' and chldate='" + dticode2.Rows[i]["vchdate"].ToString().Trim() + "'", "vchnum");
                                        oporow["inw_chl_no_out_bill_date"] = fgen.seek_iname_dt(dt1, " challan='" + dticode2.Rows[i]["vchnum"].ToString().Trim() + "' and chldate='" + dticode2.Rows[i]["vchdate"].ToString().Trim() + "'", "vchdate");
                                    }
                                    oporow["unit_type"] = dticode2.Rows[i]["type"].ToString().Trim();
                                    db1 = fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString().Trim());
                                    db4 += db1;
                                    oporow["receipt_qty"] = fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString().Trim());
                                    oporow["issue_qty"] = fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString().Trim());
                                    db2 = fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString().Trim());
                                    db5 += db2;
                                    if (i == 0)
                                    {
                                        if (dticode2.Rows[i]["type"].ToString().Trim() == "13")
                                        {
                                            db3 = fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString().Trim());
                                        }
                                        else
                                        {
                                            db3 = fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString().Trim());
                                        }
                                    }
                                    else
                                    {
                                        if (dticode2.Rows[i]["type"].ToString().Trim() == "13")
                                        {
                                            db3 += db1;
                                        }
                                        else
                                        {
                                            db3 = db3 - db2;
                                        }
                                    }
                                    oporow["balance"] = db3;
                                    dtm.Rows.Add(oporow);
                                }
                                oporow = dtm.NewRow();
                                oporow["unit_type"] = "TOTAL";
                                oporow["receipt_qty"] = db4;
                                oporow["issue_qty"] = db5;
                                oporow["balance"] = db4 - db5;
                                db6 += fgen.make_double(oporow["Balance"].ToString().Trim());
                                dtm.Rows.Add(oporow);
                            }
                        }
                        oporow = dtm.NewRow();
                        oporow["particulars"] = "CLOSING BALANCE";
                        oporow["balance"] = db6;
                        dtm.Rows.Add(oporow);
                    }
                    Session["send_dt"] = dtm;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Job Work Finished Goods From " + fromdt + " To " + todt, frm_qstr);
                    #endregion
                    break;

                case "F25159":
                    #region SEFL Job Work Raw Material
                    dtm = new DataTable();
                    dtm.Columns.Add("DATE", typeof(string));
                    dtm.Columns.Add("MRR_VOUCHER", typeof(string));
                    dtm.Columns.Add("PARTICULARS", typeof(string));
                    dtm.Columns.Add("CHALLAN_NO", typeof(string));
                    dtm.Columns.Add("CHALLAN_DATE", typeof(string));
                    dtm.Columns.Add("UNIT_TYPE", typeof(string));
                    dtm.Columns.Add("OPENING_QTY", typeof(double));
                    dtm.Columns.Add("RECEIPT_QTY", typeof(double));
                    dtm.Columns.Add("ISSUE_QTY", typeof(double));
                    dtm.Columns.Add("BALANCE", typeof(double));
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (party_cd.Length <= 1)
                    {
                        mq2 = " and a.acode like '%'";
                    }
                    else
                    {
                        mq2 = " and a.acode='" + party_cd + "'";
                    }

                    if (part_cd.Length <= 1)
                    {
                        mq3 = " and a.icode like '%'";
                        cond = " and icode like '%'";
                    }
                    else
                    {
                        mq3 = " and a.icode='" + part_cd + "'";
                        cond = " and icode='" + part_cd + "'";
                    }
                    mq0 = "select a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,f.aname,trim(a.icode) as icode,i.iname,a.iqtyin,trim(a.refnum) as refnum,to_char(a.refdate,'dd/mm/yyyy') as refdate,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + mbr + "' and a.type='08' and a.vchdate " + xprdrange + mq2 + mq3 + " order by vdd,vchnum";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    mq1 = "select a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,t.name as aname,trim(a.icode) as icode,a.iqtyout,a.desc_,trim(a.genum) as genum,to_char(a.gedate,'dd/mm/yyyy') as gedate,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,type t where trim(a.type)=trim(t.type1) and t.id='M' and a.branchcd='" + mbr + "' and a.type='38' and a.vchdate " + xprdrange + mq3 + " order by vdd";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    mq2 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R01'", "params");
                    xprdrange1 = "between to_Date('" + mq2 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";

                    mq4 = "select trim(a.icode) as icode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4  " + cond + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdrange1 + "  and store='Y' " + cond + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond + "  GROUP BY trim(icode) ,branchcd) a GROUP BY trim(a.icode) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                    mq4 = "select trim(a.icode) as icode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdrange1 + "  and store='Y' " + cond + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond + "  GROUP BY trim(icode) ,branchcd) a GROUP BY trim(a.icode) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                    dt5 = new DataTable();
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq4);//stock dt
                    oporow = null;
                    if (dt.Rows.Count > 0)
                    {
                        view1 = new DataView(dt);
                        dticode = new DataTable();
                        dticode = view1.ToTable(true, "icode", "iname");
                        foreach (DataRow dr in dticode.Rows)
                        {
                            DataView view3 = new DataView(dt, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = new DataTable();
                            dt3 = view3.ToTable(true, "vchnum", "vchdate", "refnum", "refdate");

                            oporow = dtm.NewRow();
                            oporow["Particulars"] = dr["iname"].ToString().Trim() + " (" + dr["icode"].ToString().Trim() + ")";
                            dtm.Rows.Add(oporow);

                            db5 = 0;
                            foreach (DataRow dr1 in dt3.Rows)
                            {
                                view2 = new DataView(dt, "vchnum='" + dr1["vchnum"].ToString().Trim() + "' and vchdate='" + dr1["vchdate"].ToString().Trim() + "' and refnum='" + dr1["refnum"].ToString().Trim() + "' and refdate='" + dr1["refdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode2 = new DataTable();
                                dticode2 = view2.ToTable();
                                dt2 = new DataTable();

                                if (dt1.Rows.Count > 0)
                                {
                                    dv = new DataView(dt1, "genum='" + dr1["vchnum"].ToString().Trim() + "' and gedate='" + dr1["vchdate"].ToString().Trim() + "'  and desc_='" + dr1["refnum"].ToString().Trim() + " " + "|" + " " + dr1["refdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dt2 = dv.ToTable();
                                }

                                db1 = 0; db2 = 0; db3 = 0; db4 = 0; db6 = 0; db7 = 0;
                                for (int i = 0; i < dticode2.Rows.Count; i++)
                                {
                                    oporow = dtm.NewRow();
                                    oporow["mrr_voucher"] = dticode2.Rows[i]["vchnum"].ToString().Trim();
                                    oporow["date"] = dticode2.Rows[i]["vchdate"].ToString().Trim();
                                    oporow["particulars"] = dticode2.Rows[i]["aname"].ToString().Trim();
                                    oporow["challan_no"] = dticode2.Rows[i]["refnum"].ToString().Trim();
                                    oporow["challan_date"] = dticode2.Rows[i]["refdate"].ToString().Trim();
                                    oporow["unit_type"] = dticode2.Rows[i]["type"].ToString().Trim();
                                    db6 += fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString().Trim());
                                    oporow["OPENING_QTY"] = fgen.seek_iname_dt(dt5, "icode='" + dr["icode"].ToString().Trim() + "'", "Closing_Stk");
                                    oporow["receipt_qty"] = fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString().Trim());
                                    oporow["issue_qty"] = 0;
                                    oporow["balance"] = Math.Round(fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString().Trim()) + fgen.make_double(oporow["OPENING_QTY"].ToString().Trim()), 2);
                                    db1 += fgen.make_double(oporow["balance"].ToString().Trim());
                                    db7 += fgen.make_double(oporow["OPENING_QTY"].ToString().Trim());
                                    er1 = dticode2.Rows[i]["refnum"].ToString().Trim();
                                    er2 = dticode2.Rows[i]["refdate"].ToString().Trim();
                                    dtm.Rows.Add(oporow);
                                }

                                for (int k = 0; k < dt2.Rows.Count; k++)
                                {
                                    oporow = dtm.NewRow();
                                    oporow["mrr_voucher"] = dt2.Rows[k]["vchnum"].ToString().Trim();
                                    oporow["date"] = dt2.Rows[k]["vchdate"].ToString().Trim();
                                    oporow["particulars"] = dt2.Rows[k]["aname"].ToString().Trim();
                                    oporow["challan_no"] = er1;
                                    oporow["challan_date"] = er2;
                                    oporow["unit_type"] = dt2.Rows[k]["type"].ToString().Trim();
                                    oporow["receipt_qty"] = 0;
                                    oporow["issue_qty"] = fgen.make_double(dt2.Rows[k]["iqtyout"].ToString().Trim());
                                    db2 = fgen.make_double(dt2.Rows[k]["iqtyout"].ToString().Trim());
                                    if (k == 0)
                                    {
                                        db3 = db1 - db2;
                                    }
                                    else
                                    {
                                        db3 = db3 - db2;
                                    }
                                    db4 += db2;
                                    oporow["balance"] = Math.Round(db3, 2);
                                    dtm.Rows.Add(oporow);
                                }
                                oporow = dtm.NewRow();
                                oporow["unit_type"] = "TOTAL";
                                oporow["OPENING_QTY"] = db7;
                                oporow["receipt_qty"] = db6;
                                oporow["issue_qty"] = db4;
                                oporow["balance"] = Math.Round((db7 + db6) - db4, 2);
                                db5 += fgen.make_double(oporow["Balance"].ToString().Trim());
                                dtm.Rows.Add(oporow);
                            }
                            oporow = dtm.NewRow();
                            oporow["particulars"] = "TOTAL CLOSING BALANCE";
                            oporow["balance"] = Math.Round(db5, 2);
                            dtm.Rows.Add(oporow);
                        }
                    }
                    Session["send_dt"] = dtm;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Job Work Raw Material From " + fromdt + " To " + todt, frm_qstr);
                    #endregion
                    break;

                case "F25163": // gcap fg stock summary and valuation   
                    #region
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("Item_Code", typeof(string));
                    ph_tbl.Columns.Add("Item_Name", typeof(string));
                    ph_tbl.Columns.Add("OP_QTY", typeof(double));
                    ph_tbl.Columns.Add("OP_RATE", typeof(double));
                    ph_tbl.Columns.Add("OP_VAL", typeof(double));
                    ph_tbl.Columns.Add("RCPT_QTY", typeof(double));
                    ph_tbl.Columns.Add("RCPT_RATE", typeof(double));
                    ph_tbl.Columns.Add("RCPT_VAL", typeof(double));
                    ph_tbl.Columns.Add("ISS_QTY", typeof(double));
                    ph_tbl.Columns.Add("ISS_RATE", typeof(double));
                    ph_tbl.Columns.Add("ISS_VAL", typeof(double));
                    ph_tbl.Columns.Add("CLOS_QTY", typeof(double));
                    ph_tbl.Columns.Add("CLOS_RATE", typeof(double));
                    ph_tbl.Columns.Add("CLOS_VAL", typeof(double));

                    cond1 = "";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd.Length < 2)
                    {
                        cond = " substr(trim(icode),1,1)= '9'";
                    }
                    else
                    {
                        cond = " substr(trim(icode),1,2) in (" + party_cd + ")";
                    }
                    if (part_cd.Length < 2)
                    {
                        cond1 = " substr(trim(icode),1,1) = '9'";
                    }
                    else
                    {
                        cond1 = " substr(trim(icode),1,4) in (" + part_cd + ")";
                    }

                    xprdrange1 = "BETWEEN TO_DATE('" + frm_cDt1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    dt = new DataTable();
                    mq1 = "select TRIM(ICODE) AS ICODE,sum(opening) as opening,sum(cdr) as Rcpt,sum(ccr) as Issued,Sum((opening+cdr)-ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='Y' GROUP BY trim(icode) ,branchcd)  WHERE " + cond + " and " + cond1 + " GROUP BY TRIM(ICODE) ORDER BY ICODE";
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);
                    dt1 = new DataTable();
                    mq2 = "select trim(icode) as icode,sum(iamount) as amount,sum(iqtyout) as qty from ivoucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and " + cond + " and " + cond1 + " and vchdate " + xprdrange + " GROUP BY trim(icode)";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq2);
                    dt2 = new DataTable();
                    mq0 = @"replace(TRIM(INAME),'""','') AS INAME";
                    mq3 = "SELECT TRIM(ICODE) AS ICODE," + mq0 + ",IRATE FROM ITEM WHERE length(trim(icode))>4 and " + cond + " and " + cond1 + "";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq3);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = ph_tbl.NewRow();
                        dr1["Item_Code"] = dt.Rows[i]["ICODE"].ToString().Trim();
                        dr1["Item_Name"] = fgen.seek_iname_dt(dt2, "ICODE='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "INAME");
                        dr1["OP_QTY"] = dt.Rows[i]["opening"].ToString().Trim();
                        dr1["RCPT_QTY"] = dt.Rows[i]["Rcpt"].ToString().Trim();
                        dr1["ISS_QTY"] = dt.Rows[i]["Issued"].ToString().Trim();
                        dr1["CLOS_QTY"] = dt.Rows[i]["Closing_Stk"].ToString().Trim();

                        db1 = fgen.make_double(fgen.seek_iname_dt(dt1, "ICODE='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "amount"));
                        db2 = fgen.make_double(fgen.seek_iname_dt(dt1, "ICODE='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "qty"));
                        db3 = db1 / db2;

                        if (fgen.make_double(dr1["OP_QTY"].ToString()) > 0)
                        {
                            dr1["OP_RATE"] = fgen.seek_iname_dt(dt2, "ICODE='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "IRATE");
                            dr1["OP_VAL"] = Math.Round(fgen.make_double(dr1["OP_QTY"].ToString()) * fgen.make_double(dr1["OP_RATE"].ToString()), 2).ToString().Replace("NaN", "0").Replace("Infinity", "0");
                        }
                        else
                        {
                            dr1["OP_RATE"] = 0;
                            dr1["OP_VAL"] = 0;
                        }

                        if (fgen.make_double(dr1["ISS_QTY"].ToString()) > 0)
                        {
                            dr1["ISS_RATE"] = (Math.Round(db3, 2)).ToString().Replace("NaN", "0").Replace("Infinity", "0");
                            dr1["ISS_VAL"] = fgen.seek_iname_dt(dt1, "ICODE='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "amount");
                        }
                        else
                        {
                            dr1["ISS_RATE"] = 0;
                            dr1["ISS_VAL"] = 0;
                        }

                        if (fgen.make_double(dr1["RCPT_QTY"].ToString()) > 0)
                        {
                            dr1["RCPT_RATE"] = (Math.Round(db3, 2)).ToString().Replace("NaN", "0").Replace("Infinity", "0");
                            dr1["RCPT_VAL"] = (Math.Round(fgen.make_double(dt.Rows[i]["Rcpt"].ToString().Trim()) * db3, 2)).ToString().Replace("NaN", "0").Replace("Infinity", "0");
                        }
                        else
                        {
                            dr1["RCPT_RATE"] = 0;
                            dr1["RCPT_VAL"] = 0;
                        }

                        db4 = (fgen.make_double(dr1["OP_VAL"].ToString()) + fgen.make_double(dr1["RCPT_VAL"].ToString())) - fgen.make_double(dr1["ISS_VAL"].ToString());
                        db5 = (fgen.make_double(dr1["OP_QTY"].ToString()) + fgen.make_double(dr1["RCPT_QTY"].ToString())) - fgen.make_double(dr1["ISS_QTY"].ToString());
                        if (db5 != 0)
                        {
                            dr1["CLOS_VAL"] = (Math.Round(db4, 2)).ToString().Replace("NaN", "0").Replace("Infinity", "0");
                            dr1["CLOS_RATE"] = (Math.Round(fgen.make_double(dr1["CLOS_VAL"].ToString()) / fgen.make_double(dr1["CLOS_QTY"].ToString()), 2)).ToString().Replace("NaN", "0").Replace("Infinity", "0");
                        }
                        else
                        {
                            dr1["CLOS_VAL"] = 0;
                            dr1["CLOS_RATE"] = 0;

                        }
                        if (fgen.make_double(dr1["OP_QTY"].ToString()) == 0 && fgen.make_double(dr1["ISS_QTY"].ToString()) == 0 && fgen.make_double(dr1["ISS_QTY"].ToString()) == 0)
                        { }
                        else
                        {
                            ph_tbl.Rows.Add(dr1);
                        }
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("FG Stock Summary with Valuation For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F25151"://////7/10/19
                    SQuery = "SELECT A.VCHNUM AS RET_REQ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS RET_DT,A.ACODE AS CODE,B.NAME AS DEPT_NAME,A.ICODE AS ERPCODE,C.INAME,C.CPARTNO,A.IQTY_CHL AS QTY,A.ENT_BY,A.ENT_dT,A.EDT_BY,A.EDT_dT,TO_CHAR(a.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,TYPE B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " AND B.ID='M' AND A.TYPE LIKE '1%' AND A.TYPE<'15' AND A.VCHDATE " + xprdrange + " AND NVL(A.STORE,'-')!='Y' AND NVL(A.INSPECTED,'-')!='Y' ORDER BY A.VCHNUM DESC,VDD DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Pending for Return";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F25151L"://////7/10/19
                    SQuery = "SELECT A.VCHNUM AS RET_REQ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS RET_DT,A.ACODE AS CODE,B.NAME AS DEPT_NAME,A.ICODE AS ERPCODE,C.INAME,C.CPARTNO,A.IQTY_CHL AS req_QTY,a.iqtyin as ret_qty,A.ENT_BY as req_by,A.ENT_dT,A.EDT_BY,A.EDT_dT,a.pname as ret_by,TO_CHAR(a.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,TYPE B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " AND B.ID='M' AND A.TYPE LIKE '1%' AND A.TYPE<'15' AND A.VCHDATE " + xprdrange + "  ORDER BY A.VCHNUM DESC,VDD DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Return List All (with Pending)";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F25130V":
                    SQuery = "SELECT a.VCHNUM AS DOC_nO,to_char(A.VCHDATE,'dd/mm/yyyy') as doc_dt,trim(A.ICODE) as erpcode,B.INAME as product,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as INVDATE,A.REFNUM,to_char(A.REFDATE,'dd/mm/yyyy') as REFDATE,A.IQTYIN as qty,A.NARATION as barcode,A.PURPOSE as from_loc,A.THRU as to_loc,A.TC_NO as ticket_barcode,A.BINNO as mac_Address,to_char(a.vchdate,'yyyymmdd') as vdd from IVOUCHERW a,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='MJ' AND A.VCHDATE " + xprdrange + " AND NVL(A.BTCHNO,'-')!='-' ORDER BY vdd desc,A.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Vessel Transfer List";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25144A":
                    xprd1 = " between to_date('01/04/2016','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')-1";
                    xprd2 = xprdrange;
                    cond = "";
                    string bincode = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='I88'", "params");
                    if (bincode.Length > 2) cond1 = "and trim(icode)='" + bincode + "'";
                    if (ulvl == "M")
                    {
                        cond = "and trim(acode)='" + uname + "'";
                    }

                    mq0 = "select B.Aname as Customer,B.Addr1 as Address,sum(opening) as Crate_Op,sum(cdr) as Crate_Issue,sum(ccr) as Crate_return,sum(opening)+sum(cdr)-sum(ccr)as Crate_Bal,b.flevel as Crates_Allowed,(Case when b.flevel>0 then b.flevel-(sum(opening)+sum(cdr)-sum(ccr)) else 0 end) as More_allowed,trim(a.acode) as E_Acode,trim(a.icode) as E_Icode from (select acode,icode,clqty as opening,0 as cdr,0 as ccr,0 as clos from crate_bal where branchcd='" + mbr + "' " + cond + " " + cond1 + " union all ";
                    mq1 = "select acode,icode,sum(iqtyout)-sum(iqtyin) as opening,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " " + cond + " " + cond1 + " and store='Y' GROUP BY acode,ICODE union all ";
                    mq2 = "select acode,icode,0 as op,sum(iqtyout) as cdr,sum(iqtyin) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " " + cond + " " + cond1 + " and store='Y' GROUP BY acode,ICODE )a, famst b where trim(A.acodE)=trim(B.acode)  group by trim(a.acode),trim(a.icode),b.flevel,b.aname,b.addr1 order by b.aname";


                    SQuery = mq0 + mq1 + mq2;
                    //SQuery = "Select Type,Vchnum,Vchdate,iqtyout,iqtyin,ent_by,ent_dt,edT_by,edt_dt from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + "  " + cond + " " + cond1 + " and store='Y' order by vchdate,vchnum";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Crate List";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F25256": ////// MADE BY MADHVI, MERGED 04 March 2020, SVPL, WIP RECONCILIATION SECOND REPORT
                    dtm = new DataTable();
                    dtm.Columns.Add("Code", typeof(string));
                    dtm.Columns.Add("Item_Name", typeof(string));
                    dtm.Columns.Add("Unit", typeof(string));
                    dtm.Columns.Add("RM_WIP_Op", typeof(double));
                    dtm.Columns.Add("SF_WIP_Op", typeof(double));
                    dtm.Columns.Add("SF_Store_Opening", typeof(double));
                    dtm.Columns.Add("TP_Opening", typeof(double));
                    dtm.Columns.Add("Total_Opening", typeof(double));
                    dtm.Columns.Add("RM_Rcpt_Qty", typeof(double));
                    dtm.Columns.Add("CRM", typeof(double));
                    dtm.Columns.Add("Total_Receipt", typeof(double));
                    dtm.Columns.Add("To_Bond", typeof(double));
                    dtm.Columns.Add("Return_Note", typeof(double));
                    dtm.Columns.Add("CR_Rejn", typeof(double));
                    dtm.Columns.Add("MR_Rejn", typeof(double));
                    dtm.Columns.Add("Total", typeof(double));
                    dtm.Columns.Add("SF_Store_Closing", typeof(double));
                    dtm.Columns.Add("TP_Closing", typeof(double));
                    dtm.Columns.Add("WIP_Closing", typeof(double));
                    dtm.Columns.Add("SF_Store_Closing_Value", typeof(double));
                    dtm.Columns.Add("TP_Closing_Value", typeof(double));
                    dtm.Columns.Add("WIP_Closing_Value", typeof(double));
                    dtm.Columns.Add("Rate", typeof(double));
                    //dtm.Columns.Add("Value", typeof(double));
                    dtm.Columns.Add("Bfactor", typeof(string));
                    dtm.Columns.Add("Sfcode", typeof(string));

                    // FAMILY CODE
                    dt12 = new DataTable();
                    mq12 = "SELECT TRIM(TYPE1) AS TYPE1,NAME FROM TYPEGRP WHERE ID='^8' ORDER BY TYPE1";
                    dt12 = fgen.getdata(frm_qstr, co_cd, mq12);

                    // LATEST MRR RATE FOR FINDING VALUE .... PICKED FROM MRR COSTING
                    dt11 = new DataTable();
                    er1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R06'", "params");
                    mq11 = "SELECT VCHDATE,TRIM(ICODE) AS ICODE,ICHGS AS IRATE/*(CASE WHEN TRIM(TYPE)='07' THEN CAVITY*IRATE ELSE IRATE END) AS IRATE*/ FROM IVOUCHER WHERE BRANCHCD='" + mbr + "' AND TYPE LIKE '0%' AND /*VCHDATE>(SYSDATE-500)*/ VCHDATE>=TO_DATE('" + er1 + "','DD/MM/YYYY') AND SUBSTR(TRIM(ICODE),1,1) <'1' AND STORE='Y' ORDER BY VCHDATE DESC";
                    dt11 = fgen.getdata(frm_qstr, co_cd, mq11);

                    // PARENT ICODE OF 9 SERIES ITEM WITH THEIR CHILD CODE
                    dt10 = new DataTable();
                    mq10 = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(A.IBCODE) AS IBCODE,A.IBQTY,I.UNIT FROM ITEMOSP A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1) ='9' AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') ORDER BY ICODE";
                    dt10 = fgen.getdata(frm_qstr, co_cd, mq10);

                    // FOR ALL CHILD PARTS OF 9 SERIES
                    dt9 = new DataTable();
                    mq9 = "SELECT DISTINCT TRIM(A.IBCODE) AS IBCODE,I.INAME,I.UNIT,TRIM(I.BFACTOR) AS BFACTOR FROM ITEMOSP A,ITEM I WHERE TRIM(A.IBCODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1) ='9' AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') AND (NVL(A.IBQTY,0)>0 OR NVL(A.IBWT,0)>0) ORDER BY BFACTOR,IBCODE";
                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9);

                    // SF WIP OPENING
                    //ded4 = "SELECT TO_CHAR(TO_DATE('01/" + todt.Substring(3, 7) + "','DD/MM/YYYY')-1,'DD/MM/YYYY') AS lastmnth FROM DUAL";
                    ded4 = "SELECT TO_CHAR(TO_DATE('" + fromdt + "','DD/MM/YYYY')-1,'DD/MM/YYYY') AS lastmnth FROM DUAL";
                    ded5 = fgen.seek_iname(frm_qstr, co_cd, ded4, "lastmnth"); // PREVIOUS MONTH DATE
                    wip_stk_vw_SVPL(ded5);
                    dt8 = new DataTable();
                    mq8 = "select trim(icode) as icode,total from wipcolstkw_" + mbr + " order by icode";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8);

                    // RM WIP OPENING
                    wiptotstk_SVPL();
                    dt7 = new DataTable();
                    mq7 = "select trim(erp_code) as icode,opening,closing from wiptotstkw_" + mbr + " order by icode";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7);

                    //RETURN NOTE
                    dt6 = new DataTable();
                    mq6 = "Select trim(a.Icode) as iCode,sum(a.iqty_chl) as Req_Qty,sum(a.iqtyin) as ret_Qty from ivoucher a where a.branchcd='" + mbr + "' and substr(A.type,1,2) like '1%' and a.vchdate " + xprdrange + " and  a.store!='N' group by trim(a.Icode) order by icode";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq6);

                    // TP STOCK
                    TP_starting_dt = "";
                    TP_starting_dt = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    xprdrange1 = " BETWEEN TO_DATE('" + TP_starting_dt + "','DD/MM/YYYY')-1 AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    dt5 = new DataTable();
                    // mq5 = "select trim(icode) as icode,sum(opening) as opening,sum(qty_sent) as qty_sent,sum(qty_rcvd) as qty_rcvd ,sum(qty_balance) as qty_balance from (select b.iname,trim(a.icode) as Icode,sum(a.opening) as Opening,sum(a.cdr) as Qty_Sent,sum(a.ccr) as Qty_Rcvd,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Qty_balance,c.aname,c.acode,b.cpartno from (Select '-' as acode,'-' as store,icode, YR_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,0 as inqa from ITEMBAL where 1=2 union all  select acode,store,icode,sum(iqtyout)-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in('0','2') and vchdate between to_date('01/04/2010','dd/mm/yyyy')-1 and to_Date('" + fromdt + "','dd/mm/yyyy')-1  and acode like '%%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE union all select acode,store,icode,0 as op,sum(iqtyout) as cdr,sum(iqtyin) as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in('0','2') and vchdate " + xprdrange + "  and acode like '%%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE )a,item b,(select distinct a.acode,b.aname from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.type in ('21','23'))c where trim(A.icode)=trim(B.icode) and trim(A.acode)=trim(c.acode) group by b.iname,b.cpartno,trim(a.icode),c.aname,c.acode /*having sum(a.opening)+sum(a.cdr)+sum(a.ccr) >0*/ order by c.acode,b.iname) group by icode";
                    mq5 = "select b.iname as Item_Name,sum(a.opening) as Opening,sum(a.cdr) as Qty_Sent,sum(a.ccr) as Qty_Rcvd,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Qty_balance,trim(a.icode) as Icode,b.cpartno from (Select '-' as acode,'-' as store,icode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,0 as inqa from ITEMBAL where 1=2 union all select acode,store,icode,sum(iqtyout)-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,2) in('09','21','0J','23') and vchdate " + xprdrange1 + " and acode like '%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE union all select acode,store,icode,0 as op,sum(iqtyout) as cdr,sum(iqtyin) as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and substr(type,1,2) in('09','21','0J','23') and vchdate " + xprdrange + " and acode like '%' and substr(icode,1,2) in('82','84') GROUP BY acode,store,ICODE )a,item b,(select distinct a.acode,b.aname from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.type in ('21','23'))c where trim(A.icode)=trim(B.icode) and trim(A.acode)=trim(c.acode) group by b.iname,b.cpartno,trim(a.icode) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by Icode";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq5);

                    // REJECTION STOCK
                    starting_rej_store_dt = "";
                    starting_rej_store_dt = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R24'", "params");
                    xprdrange1 = " BETWEEN TO_DATE('" + starting_rej_store_dt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";

                    dt4 = new DataTable();
                    mq4 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as opening,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='R' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='R' GROUP BY trim(icode) ,branchcd) a GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);

                    // MAIN STOCK
                    xprdrange1 = " BETWEEN TO_DATE('" + cDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    dt3 = new DataTable();
                    mq3 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    // PARENT ICODE OF 82,84 SERIES ITEM
                    dt2 = new DataTable();
                    mq2 = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(A.IBCODE) AS IBCODE,I.UNIT,A.IBQTY FROM ITEMOSP A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('82','84') ORDER BY IBCODE";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    // PARENT ICODE OF 01 AND 02 SERIES ITEM WITH THEIR CHILD ICODE
                    dt1 = new DataTable();
                    mq1 = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(A.IBCODE) AS IBCODE,I.INAME,I.UNIT,TRIM(I.BFACTOR) AS BFACTOR FROM ITEMOSP A,ITEM I WHERE TRIM(A.IBCODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2) IN ('82','84') AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') ORDER BY ICODE";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    //// FOR ALL CHILD PARTS OF 82 AND 84 GROUP
                    //dt = new DataTable();
                    //mq0 = "SELECT DISTINCT TRIM(A.IBCODE) AS IBCODE,I.INAME,I.UNIT,TRIM(I.BFACTOR) AS BFACTOR FROM ITEMOSP A,ITEM I WHERE TRIM(A.IBCODE)=TRIM(I.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2) IN ('82','84') AND SUBSTR(TRIM(A.IBCODE),1,2) IN ('01','02') AND (NVL(A.IBQTY,0)>0 OR NVL(A.IBWT,0)>0) AND A.IBCODE='01060013' ORDER BY BFACTOR,IBCODE";
                    //dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    oporow = null; Total_Receipt = 0; Total_Rejn = 0; Total_Op = 0; Closing_WIP = 0; RM_Rcpt_Qty = 0; SF_Store_Opening = 0; SF_Store_Closing = 0; CR_Rejn = 0; MR_Rejn = 0; TP_Opening = 0; TP_Closing = 0; Return_Note = 0; RM_WIP_Op = 0; SF_WIP_Op = 0; CRM = 0; To_Bond = 0; Bom_Qty = 0; To_Bond_Exists = 0;
                    To_Bond_YN = "";
                    // string parent = "";
                    #region For 82,84 Groups
                    if (dt2.Rows.Count > 0)
                    {
                        view1 = new DataView(dt2);
                        mdt = new DataTable();
                        mdt = view1.ToTable(true, "ICODE", "IBCODE", "UNIT");
                        DataTable dtMultiFG;
                        foreach (DataRow dr in mdt.Rows)
                        {
                            dticode = new DataTable();
                            dticode2 = new DataTable();
                            SFParent = new DataTable();
                            dtMultiFG = new DataTable();
                            dv = new DataView(dt2, "ibcode='" + dr["ibcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dtMultiFG = dv.ToTable();
                            db = 0;
                            Total_Receipt = 0; Total_Rejn = 0; Total_Op = 0; Closing_WIP = 0; RM_Rcpt_Qty = 0; SF_Store_Opening = 0; SF_Store_Closing = 0; CR_Rejn = 0; MR_Rejn = 0; TP_Opening = 0; TP_Closing = 0; Return_Note = 0; RM_WIP_Op = 0; SF_WIP_Op = 0; CRM = 0; To_Bond = 0; Bom_Qty = 0;

                            if (dr["icode"].ToString().Trim() == "84040058" || dr["icode"].ToString().Trim() == "99080026" || dr["icode"].ToString().Trim() == "99080031" || dr["icode"].ToString().Trim() == "99080032" || dr["icode"].ToString().Trim() == "99080033")
                            {

                            }

                            // IF PARENT CODE OF SF IS NOT OF 9 SERIES
                            if (dr["icode"].ToString().Trim().Substring(0, 1) == "8")
                            {
                                // parent = fgen.seek_iname_dt(dt2, "ibcode='" + dr["icode"].ToString().Trim() + "'", "icode");                                
                                db = fgen.make_double(fgen.seek_iname_dt(dt2, "ibcode='" + dr["icode"].ToString().Trim() + "'", "ibqty"));
                                if (dt3.Rows.Count > 0)
                                {
                                    SF_Store_Opening = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["icode"].ToString().Trim() + "'", "opening"));
                                    SF_Store_Closing = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    MR_Rejn = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr["icode"].ToString().Trim() + "'", "Rcpt"));
                                }
                                if (dt5.Rows.Count > 0)
                                {
                                    TP_Opening = fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dr["icode"].ToString().Trim() + "'", "Opening"));
                                    TP_Closing = fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dr["icode"].ToString().Trim() + "'", "Qty_balance"));
                                }
                                if (dt8.Rows.Count > 0)
                                {
                                    SF_WIP_Op = fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr["icode"].ToString().Trim() + "'", "total"));
                                }
                                dv = new DataView(dt2, "ibcode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dtMultiFG = dv.ToTable();
                                for (int z = 0; z < dtMultiFG.Rows.Count; z++)
                                {
                                    if (dt4.Rows.Count > 0)
                                    {
                                        CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dtMultiFG.Rows[z]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    // CHECKING TO_BOND HAS VALUE OR NOT
                                    To_Bond_Exists = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dtMultiFG.Rows[z]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    if (To_Bond_Exists != 0)
                                    {
                                        // IF TO_BOND HAS VALUE THEN SET IT TO Y FOR MULTIPLICATION
                                        To_Bond_YN = "Y";
                                    }
                                    To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dtMultiFG.Rows[z]["icode"].ToString().Trim() + "'", "Rcpt"));
                                }
                            }
                            else
                            {
                                // parent = dr["icode"].ToString().Trim();
                                if (dt4.Rows.Count > 0)
                                {
                                    CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr["icode"].ToString().Trim() + "'", "Rcpt"));
                                }
                                // CHECKING TO_BOND HAS VALUE OR NOT
                                To_Bond_Exists = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["icode"].ToString().Trim() + "'", "Rcpt"));
                                if (To_Bond_Exists != 0)
                                {
                                    // IF TO_BOND HAS VALUE THEN SET IT TO Y FOR MULTIPLICATION
                                    To_Bond_YN = "Y";
                                }
                                To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["icode"].ToString().Trim() + "'", "Rcpt"));
                            }
                            // BASED ON SF FIND ITS CHILD CODE
                            if (dt1.Rows.Count > 0)
                            {
                                view2 = new DataView(dt1, "icode='" + dr["ibcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode = view2.ToTable();
                            }

                            //if (dticode.Rows.Count > 1)
                            //{
                            db += fgen.make_double(fgen.seek_iname_dt(dt2, "ibcode='" + dr["ibcode"].ToString().Trim() + "'", "ibqty"));
                            if (dt3.Rows.Count > 0)
                            {
                                SF_Store_Opening += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["ibcode"].ToString().Trim() + "'", "opening"));
                                SF_Store_Closing += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Closing_Stk"));
                            }
                            if (dt4.Rows.Count > 0)
                            {
                                //CRM = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + parent + "'", "Rcpt"));
                                MR_Rejn += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Rcpt"));
                            }
                            if (dt5.Rows.Count > 0)
                            {
                                TP_Opening += fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Opening"));
                                TP_Closing += fgen.make_double(fgen.seek_iname_dt(dt5, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Qty_balance"));
                            }
                            if (dt8.Rows.Count > 0)
                            {
                                SF_WIP_Op += fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr["ibcode"].ToString().Trim() + "'", "total"));
                            }
                            //// CHECKING TO_BOND HAS VALUE OR NOT
                            //To_Bond_Exists = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + parent + "'", "Rcpt"));
                            //if (To_Bond_Exists != 0)
                            //{
                            //    // IF TO_BOND HAS VALUE THEN SET IT TO Y FOR MULTIPLICATION
                            //    To_Bond_YN = "Y";
                            //}
                            //To_Bond = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + parent + "'", "Rcpt"));
                            for (int m = 0; m < dticode.Rows.Count; m++)
                            {
                                if (m == 0)
                                {
                                    oporow = dtm.NewRow();
                                    oporow["Code"] = dticode.Rows[m]["ibcode"].ToString().Trim();
                                    oporow["Item_Name"] = dticode.Rows[m]["iname"].ToString().Trim();
                                    oporow["Unit"] = dticode.Rows[m]["unit"].ToString().Trim();
                                    oporow["Bfactor"] = dticode.Rows[m]["bfactor"].ToString().Trim(); // FAMILY CODE
                                }
                                if (dticode.Rows[m]["ibcode"].ToString().Trim() == "01070010")
                                {

                                }

                                if (dt3.Rows.Count > 0)
                                {
                                    RM_Rcpt_Qty += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[m]["ibcode"].ToString().Trim() + "'", "Issued"));
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    CR_Rejn += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[m]["ibcode"].ToString().Trim() + "'", "Rcpt"));
                                }
                                if (dt6.Rows.Count > 0)
                                {
                                    Return_Note += fgen.make_double(fgen.seek_iname_dt(dt6, "icode='" + dticode.Rows[m]["ibcode"].ToString().Trim() + "'", "ret_Qty"));
                                }
                                if (dt7.Rows.Count > 0)
                                {
                                    RM_WIP_Op += fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dticode.Rows[m]["ibcode"].ToString().Trim() + "'", "opening"));
                                }
                                if (dticode.Rows[m]["unit"].ToString().Trim() != dr["unit"].ToString().Trim())
                                {
                                    if (To_Bond_YN == "Y")
                                    {
                                        To_Bond = To_Bond * db;
                                    }
                                }
                                db1 += fgen.make_double(fgen.seek_iname_dt(dt11, "icode='" + dticode.Rows[m]["ibcode"].ToString().Trim() + "'", "irate"));
                            }
                            Total_Receipt = RM_Rcpt_Qty + CRM;
                            Total_Op = RM_WIP_Op + SF_WIP_Op + SF_Store_Opening + TP_Opening;
                            Total_Rejn = Return_Note + To_Bond + CR_Rejn + MR_Rejn;
                            Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;

                            //oporow["Rate"] = Math.Round(db1, 0);
                            //db2 = db1 / dticode.Rows.Count;
                            //oporow["SF_Store_Closing_Value"] = Math.Round(SF_Store_Closing * db2, 0);
                            //oporow["TP_Closing_Value"] = Math.Round(TP_Closing * db2, 0);
                            //oporow["WIP_Closing_Value"] = Math.Round(Closing_WIP * db2, 0);                               

                            if (RM_Rcpt_Qty + SF_Store_Opening + CR_Rejn + MR_Rejn + TP_Opening + Return_Note + RM_WIP_Op + SF_WIP_Op + CRM + To_Bond > 0)
                            {
                                if (dticode.Rows.Count > 0)
                                {
                                    oporow["RM_Rcpt_Qty"] = Math.Round(RM_Rcpt_Qty, 0);
                                    oporow["SF_Store_Opening"] = Math.Round(SF_Store_Opening, 0);
                                    oporow["SF_Store_Closing"] = Math.Round(SF_Store_Closing, 0);
                                    oporow["CR_Rejn"] = Math.Round(CR_Rejn, 0);
                                    oporow["MR_Rejn"] = Math.Round(MR_Rejn, 0);
                                    oporow["TP_Opening"] = Math.Round(TP_Opening, 0);
                                    oporow["TP_Closing"] = Math.Round(TP_Closing, 0);
                                    oporow["Return_Note"] = Math.Round(Return_Note, 0);
                                    oporow["RM_WIP_Op"] = Math.Round(RM_WIP_Op, 0);
                                    oporow["SF_WIP_Op"] = Math.Round(SF_WIP_Op, 0);
                                    oporow["CRM"] = Math.Round(CRM, 0);
                                    oporow["To_Bond"] = Math.Round(To_Bond, 0);
                                    oporow["Total_Opening"] = Math.Round(Total_Op, 0);
                                    oporow["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                                    oporow["Total"] = Math.Round(Total_Rejn, 0);
                                    oporow["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                                    oporow["SFCODE"] = dr["ibcode"].ToString().Trim();
                                    dtm.Rows.Add(oporow);
                                }
                            }
                            //}
                        }
                    }
                    #endregion

                    #region For 9 Series
                    if (dt9.Rows.Count > 0)
                    {
                        view1 = new DataView(dt9);
                        mdt = new DataTable();
                        mdt = view1.ToTable(true, "IBCODE", "INAME", "UNIT", "BFACTOR");
                        ded1 = ""; ded2 = "";
                        foreach (DataRow dr in mdt.Rows)
                        {
                            dticode = new DataTable();
                            er1 = "";
                            if (dt10.Rows.Count > 0)
                            {
                                // BASED ON RAW MATERIAL FIND ITS PARENT CODE I.E. FG CODE
                                view2 = new DataView(dt10, "ibcode='" + dr["ibcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dticode = view2.ToTable();
                            }
                            Total_Receipt = 0; Total_Rejn = 0; Total_Op = 0; Closing_WIP = 0; RM_Rcpt_Qty = 0; SF_Store_Opening = 0; SF_Store_Closing = 0; CR_Rejn = 0; MR_Rejn = 0; TP_Opening = 0; TP_Closing = 0; Return_Note = 0; RM_WIP_Op = 0; SF_WIP_Op = 0; CRM = 0; To_Bond = 0; Bom_Qty = 0;

                            // IF ONE RM HAS MORE THAN ONE PARENT THEN IT WILL MERGE THEM IN TO ONE
                            er1 = "";
                            er1 = fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "code");
                            int index = 0;
                            if (er1.Length > 1)
                            {
                                DataRow[] rows = dtm.Select("code = '" + dr["ibcode"].ToString().Trim() + "'");
                                if (rows.Length > 0)
                                {
                                    index = dtm.Rows.IndexOf(rows[0]); // FOR FINDING ROW INDEX
                                }
                                RM_Rcpt_Qty = 0;
                                CR_Rejn = 0;
                                Return_Note = 0;
                                RM_WIP_Op = 0;
                                Closing_WIP = 0;
                                SF_Store_Opening = 0;
                                SF_Store_Closing = 0;
                                MR_Rejn = 0;
                                TP_Opening = 0;
                                TP_Closing = 0;
                                SF_WIP_Op = 0;

                                Total_Receipt = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "Total_Receipt"));
                                Total_Rejn = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "Total"));
                                Total_Op = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "Total_Opening"));
                                SF_Store_Closing = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "SF_Store_Closing"));
                                TP_Closing = fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "TP_Closing"));

                                for (int i = 0; i < dticode.Rows.Count; i++)
                                {
                                    Bom_Qty = fgen.make_double(dticode.Rows[i]["ibqty"].ToString().Trim());
                                    if (dt4.Rows.Count > 0)
                                    {
                                        CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    if (Bom_Qty == 0)
                                    {
                                        To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    else
                                    {
                                        To_Bond += Bom_Qty * fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                }
                                Total_Receipt = Total_Receipt + CRM;
                                Total_Rejn = Total_Rejn + To_Bond;
                                CRM += fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "crm"));
                                To_Bond += fgen.make_double(fgen.seek_iname_dt(dtm, "code='" + dr["ibcode"].ToString().Trim() + "'", "to_bond"));
                                dtm.Rows[index]["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                                dtm.Rows[index]["Total"] = Math.Round(Total_Rejn, 0);
                                dtm.Rows[index]["CRM"] = Math.Round(CRM, 0);
                                dtm.Rows[index]["To_Bond"] = Math.Round(To_Bond, 0);

                                Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;
                                dtm.Rows[index]["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                            }
                            else
                            {
                                if (dt3.Rows.Count > 0)
                                {
                                    RM_Rcpt_Qty = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Issued"));
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    CR_Rejn = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr["ibcode"].ToString().Trim() + "'", "Rcpt"));
                                }
                                if (dt6.Rows.Count > 0)
                                {
                                    Return_Note += fgen.make_double(fgen.seek_iname_dt(dt6, "icode='" + dr["ibcode"].ToString().Trim() + "'", "ret_Qty"));
                                }
                                if (dt7.Rows.Count > 0)
                                {
                                    RM_WIP_Op = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr["ibcode"].ToString().Trim() + "'", "opening"));
                                    Closing_WIP = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr["ibcode"].ToString().Trim() + "'", "closing"));
                                }
                                SF_Store_Opening = 0;
                                SF_Store_Closing = 0;
                                MR_Rejn = 0;
                                TP_Opening = 0;
                                TP_Closing = 0;
                                SF_WIP_Op = 0;
                                for (int i = 0; i < dticode.Rows.Count; i++)
                                {
                                    Bom_Qty = fgen.make_double(dticode.Rows[i]["ibqty"].ToString().Trim());
                                    if (dt4.Rows.Count > 0)
                                    {
                                        CRM += fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    if (Bom_Qty == 0)
                                    {
                                        To_Bond += fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                    else
                                    {
                                        To_Bond += Bom_Qty * fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "Rcpt"));
                                    }
                                }
                                Total_Receipt = RM_Rcpt_Qty + CRM;
                                Total_Op = RM_WIP_Op + SF_WIP_Op + SF_Store_Opening + TP_Opening;
                                Total_Rejn = Return_Note + To_Bond + CR_Rejn + MR_Rejn;
                                oporow = dtm.NewRow();
                                oporow["Code"] = dr["ibcode"].ToString().Trim();
                                oporow["Item_Name"] = dr["iname"].ToString().Trim();
                                oporow["Unit"] = dr["unit"].ToString().Trim();
                                oporow["Bfactor"] = dr["bfactor"].ToString().Trim(); // FAMILY CODE
                                oporow["RM_Rcpt_Qty"] = Math.Round(RM_Rcpt_Qty, 0);
                                oporow["SF_Store_Opening"] = Math.Round(SF_Store_Opening, 0);
                                oporow["SF_Store_Closing"] = Math.Round(SF_Store_Closing, 0);
                                oporow["CR_Rejn"] = Math.Round(CR_Rejn, 0);
                                oporow["MR_Rejn"] = Math.Round(MR_Rejn, 0);
                                oporow["TP_Opening"] = Math.Round(TP_Opening, 0);
                                oporow["TP_Closing"] = Math.Round(TP_Closing, 0);
                                oporow["Return_Note"] = Math.Round(Return_Note, 0);
                                oporow["RM_WIP_Op"] = Math.Round(RM_WIP_Op, 0);
                                oporow["SF_WIP_Op"] = Math.Round(SF_WIP_Op, 0);
                                oporow["CRM"] = Math.Round(CRM, 0);
                                oporow["To_Bond"] = Math.Round(To_Bond, 0);
                                oporow["Total_Opening"] = Math.Round(Total_Op, 0);
                                oporow["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                                oporow["Total"] = Math.Round(Total_Rejn, 0);
                                Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;
                                oporow["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                                if (RM_Rcpt_Qty + SF_Store_Opening + CR_Rejn + MR_Rejn + TP_Opening + Return_Note + RM_WIP_Op + SF_WIP_Op + CRM + To_Bond > 0)
                                {
                                    dtm.Rows.Add(oporow);
                                }
                            }
                        }
                    }
                    #endregion

                    if (dtm.Rows.Count > 0)
                    {
                        view1 = new DataView(dtm);
                        mdt = new DataTable();
                        mdt = view1.ToTable(true, "CODE");
                        dticode2 = new DataTable();
                        dticode2 = dtm.Clone(); oporow = null;
                        foreach (DataRow dr in mdt.Rows)
                        {
                            dticode = new DataTable();
                            view2 = new DataView(dtm, "code='" + dr["code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = view2.ToTable();
                            er12 = "";
                            Total_Receipt = 0; Total_Rejn = 0; Total_Op = 0; Closing_WIP = 0; RM_Rcpt_Qty = 0; SF_Store_Opening = 0; SF_Store_Closing = 0; CR_Rejn = 0; MR_Rejn = 0; TP_Opening = 0; TP_Closing = 0; Return_Note = 0; RM_WIP_Op = 0; SF_WIP_Op = 0; CRM = 0; To_Bond = 0; Bom_Qty = 0;
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                if (dticode.Rows.Count == 1)
                                {
                                    oporow = dticode2.NewRow();
                                    oporow["Code"] = dticode.Rows[i]["code"].ToString().Trim();
                                    oporow["Item_Name"] = dticode.Rows[i]["item_name"].ToString().Trim();
                                    oporow["Unit"] = dticode.Rows[i]["unit"].ToString().Trim();
                                    oporow["Bfactor"] = dticode.Rows[i]["bfactor"].ToString().Trim(); // FAMILY CODE
                                    RM_Rcpt_Qty = fgen.make_double(dticode.Rows[i]["RM_Rcpt_Qty"].ToString().Trim());
                                    SF_Store_Opening = fgen.make_double(dticode.Rows[i]["SF_Store_Opening"].ToString().Trim());
                                    SF_Store_Closing = fgen.make_double(dticode.Rows[i]["SF_Store_Closing"].ToString().Trim());
                                    CR_Rejn = fgen.make_double(dticode.Rows[i]["CR_Rejn"].ToString().Trim());
                                    MR_Rejn = fgen.make_double(dticode.Rows[i]["MR_Rejn"].ToString().Trim());
                                    TP_Opening = fgen.make_double(dticode.Rows[i]["TP_Opening"].ToString().Trim());
                                    TP_Closing = fgen.make_double(dticode.Rows[i]["TP_Closing"].ToString().Trim());
                                    Return_Note = fgen.make_double(dticode.Rows[i]["Return_Note"].ToString().Trim());
                                    RM_WIP_Op = fgen.make_double(dticode.Rows[i]["RM_WIP_Op"].ToString().Trim());
                                    SF_WIP_Op = fgen.make_double(dticode.Rows[i]["SF_WIP_Op"].ToString().Trim());
                                    CRM = fgen.make_double(dticode.Rows[i]["CRM"].ToString().Trim());
                                    To_Bond = fgen.make_double(dticode.Rows[i]["To_Bond"].ToString().Trim());
                                    Total_Op = fgen.make_double(dticode.Rows[i]["Total_Opening"].ToString().Trim());
                                    Total_Receipt = fgen.make_double(dticode.Rows[i]["Total_Receipt"].ToString().Trim());
                                    Total_Rejn = fgen.make_double(dticode.Rows[i]["Total"].ToString().Trim());
                                }
                                else
                                {
                                    if (dticode.Rows.Count > 2)
                                    {

                                    }
                                    if (i == 0)
                                    {
                                        oporow = dticode2.NewRow();
                                        oporow["Code"] = dticode.Rows[i]["code"].ToString().Trim();
                                        oporow["Item_Name"] = dticode.Rows[i]["item_name"].ToString().Trim();
                                        oporow["Unit"] = dticode.Rows[i]["unit"].ToString().Trim();
                                        oporow["Bfactor"] = dticode.Rows[i]["bfactor"].ToString().Trim(); // FAMILY CODE
                                        RM_Rcpt_Qty = fgen.make_double(dticode.Rows[i]["RM_Rcpt_Qty"].ToString().Trim());
                                        CR_Rejn = fgen.make_double(dticode.Rows[i]["CR_Rejn"].ToString().Trim());
                                        Return_Note = fgen.make_double(dticode.Rows[i]["Return_Note"].ToString().Trim());
                                        RM_WIP_Op = fgen.make_double(dticode.Rows[i]["RM_WIP_Op"].ToString().Trim());
                                    }

                                    if (er12 != dticode.Rows[i]["sfcode"].ToString().Trim())
                                    {
                                        SF_Store_Opening += fgen.make_double(dticode.Rows[i]["SF_Store_Opening"].ToString().Trim());
                                        SF_Store_Closing += fgen.make_double(dticode.Rows[i]["SF_Store_Closing"].ToString().Trim());
                                        MR_Rejn += fgen.make_double(dticode.Rows[i]["MR_Rejn"].ToString().Trim());
                                        TP_Opening += fgen.make_double(dticode.Rows[i]["TP_Opening"].ToString().Trim());
                                        TP_Closing += fgen.make_double(dticode.Rows[i]["TP_Closing"].ToString().Trim());
                                        SF_WIP_Op += fgen.make_double(dticode.Rows[i]["SF_WIP_Op"].ToString().Trim());
                                    }
                                    CRM += fgen.make_double(dticode.Rows[i]["CRM"].ToString().Trim());
                                    To_Bond += fgen.make_double(dticode.Rows[i]["To_Bond"].ToString().Trim());
                                    Total_Op = RM_WIP_Op + SF_WIP_Op + SF_Store_Opening + TP_Opening;
                                    Total_Receipt = RM_Rcpt_Qty + CRM;
                                    Total_Rejn = Return_Note + To_Bond + CR_Rejn + MR_Rejn;
                                    er12 = dticode.Rows[i]["sfcode"].ToString().Trim();
                                }
                            }
                            oporow["RM_Rcpt_Qty"] = Math.Round(RM_Rcpt_Qty, 0);
                            oporow["SF_Store_Opening"] = Math.Round(SF_Store_Opening, 0);
                            oporow["SF_Store_Closing"] = Math.Round(SF_Store_Closing, 0);
                            oporow["CR_Rejn"] = Math.Round(CR_Rejn, 0);
                            oporow["MR_Rejn"] = Math.Round(MR_Rejn, 0);
                            oporow["TP_Opening"] = Math.Round(TP_Opening, 0);
                            oporow["TP_Closing"] = Math.Round(TP_Closing, 0);
                            oporow["Return_Note"] = Math.Round(Return_Note, 0);
                            oporow["RM_WIP_Op"] = Math.Round(RM_WIP_Op, 0);
                            oporow["SF_WIP_Op"] = Math.Round(SF_WIP_Op, 0);
                            oporow["CRM"] = Math.Round(CRM, 0);
                            oporow["To_Bond"] = Math.Round(To_Bond, 0);
                            oporow["Total_Opening"] = Math.Round(Total_Op, 0);
                            oporow["Total_Receipt"] = Math.Round(Total_Receipt, 0);
                            oporow["Total"] = Math.Round(Total_Rejn, 0);
                            Closing_WIP = (Total_Op + Total_Receipt) - Total_Rejn - SF_Store_Closing - TP_Closing;
                            oporow["WIP_Closing"] = Math.Round(Closing_WIP, 0);
                            dticode2.Rows.Add(oporow);
                        }
                    }

                    // VALUE IS FETCHED HERE SO THAT AGAIN AND AGAIN CALCULATION CAN BE RESTRICTED                   
                    foreach (DataRow dr1 in dticode2.Rows)
                    {
                        db1 = 0;
                        db1 = fgen.make_double(fgen.seek_iname_dt(dt11, "icode='" + dr1["Code"].ToString().Trim() + "'", "irate"));
                        dr1["Rate"] = Math.Round(db1, 0);
                        dr1["SF_Store_Closing_Value"] = Math.Round(fgen.make_double(dr1["SF_Store_Closing"].ToString().Trim()) * db1, 0);
                        dr1["TP_Closing_Value"] = Math.Round(fgen.make_double(dr1["TP_Closing"].ToString().Trim()) * db1, 0);
                        dr1["WIP_Closing_Value"] = Math.Round(fgen.make_double(dr1["Wip_Closing"].ToString().Trim()) * db1, 0);
                    }

                    if (dticode2.Rows.Count > 0)
                    {
                        // SORTING ON THE BASIS OF FAMILY
                        view1 = new DataView(dticode2);
                        dt10 = new DataTable();
                        view1.Sort = "bfactor";
                        dt10 = view1.ToTable(true, "BFACTOR");
                        mdt = new DataTable();
                        mdt = dticode2.Clone();
                        db1 = 0; db2 = 0; db3 = 0;
                        double db8 = 0, db9 = 0, db10 = 0, db11 = 0, db12 = 0, db13 = 0, db14 = 0, db15 = 0, db16 = 0, db17 = 0, db18 = 0, db19 = 0, db20 = 0;
                        foreach (DataRow dr1 in dt10.Rows)
                        {
                            view2 = new DataView(dticode2, "bfactor='" + dr1["bfactor"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            view2.Sort = "code";
                            dticode = view2.ToTable();
                            ded1 = "";
                            oporow = mdt.NewRow();
                            ded1 = fgen.seek_iname_dt(dt12, "type1='" + dr1["bfactor"].ToString().Trim() + "'", "name");
                            if (ded1.Trim() == "0")
                            {
                                oporow["Item_Name"] = "-";
                            }
                            else
                            {
                                oporow["Item_Name"] = ded1;
                            }
                            mdt.Rows.Add(oporow);
                            // FAMILY WISE TOTAL
                            ROWICODE = dticode.NewRow();

                            foreach (DataColumn dc in dticode.Columns)
                            {
                                to_cons = 0;
                                mq1 = "sum(" + dc.ColumnName + ")";
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 22 || dc.Ordinal == 23 || dc.Ordinal == 24)
                                {

                                }
                                else if (dc.Ordinal == 3)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db2 += to_cons;
                                }
                                else if (dc.Ordinal == 4)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db3 += to_cons;
                                }
                                else if (dc.Ordinal == 5)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db4 += to_cons;
                                }
                                else if (dc.Ordinal == 6)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db5 += to_cons;
                                }
                                else if (dc.Ordinal == 7)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db6 += to_cons;
                                }
                                else if (dc.Ordinal == 8)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db7 += to_cons;
                                }
                                else if (dc.Ordinal == 9)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db8 += to_cons;
                                }
                                else if (dc.Ordinal == 10)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db9 += to_cons;
                                }
                                else if (dc.Ordinal == 11)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db10 += to_cons;
                                }
                                else if (dc.Ordinal == 12)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db11 += to_cons;
                                }
                                else if (dc.Ordinal == 13)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db12 += to_cons;
                                }
                                else if (dc.Ordinal == 14)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db13 += to_cons;
                                }
                                else if (dc.Ordinal == 15)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db14 += to_cons;
                                }
                                else if (dc.Ordinal == 16)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db15 += to_cons;
                                }
                                else if (dc.Ordinal == 17)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db16 += to_cons;
                                }
                                else if (dc.Ordinal == 18)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db17 += to_cons;
                                }
                                else if (dc.Ordinal == 19)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db18 += to_cons; // TOTAL OF SF STORE CLOSING VALUE REQUIRED DURING GRAND TOTAL
                                }
                                else if (dc.Ordinal == 20)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db19 += to_cons; // TOTAL OF TP CLOSING VALUE REQUIRED DURING GRAND TOTAL
                                }
                                else if (dc.Ordinal == 21)
                                {
                                    to_cons += fgen.make_double(dticode.Compute(mq1, "").ToString());
                                    ROWICODE[dc] = to_cons;
                                    db20 += to_cons; // TOTAL OF WIP CLOSING VALUE REQUIRED DURING GRAND TOTAL
                                }
                            }
                            ROWICODE["Item_Name"] = oporow["Item_Name"].ToString() + " (TOTAL)";
                            dticode.Rows.Add(ROWICODE);
                            mdt.Merge(dticode);
                        }
                        mdt.Columns.Remove("BFACTOR");
                        mdt.Columns.Remove("SFCODE");
                        oporow = null;
                        oporow = mdt.NewRow();
                        foreach (DataColumn dc in mdt.Columns)
                        {
                            to_cons = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 22)
                            {

                            }
                            else if (dc.Ordinal == 3)
                            {
                                oporow[dc] = Math.Round(db2, 0);
                            }
                            else if (dc.Ordinal == 4)
                            {
                                oporow[dc] = Math.Round(db3, 0);
                            }
                            else if (dc.Ordinal == 5)
                            {
                                oporow[dc] = Math.Round(db4, 0);
                            }
                            else if (dc.Ordinal == 6)
                            {
                                oporow[dc] = Math.Round(db5, 0);
                            }
                            else if (dc.Ordinal == 7)
                            {
                                oporow[dc] = Math.Round(db6, 0);
                            }
                            else if (dc.Ordinal == 8)
                            {
                                oporow[dc] = Math.Round(db7, 0);
                            }
                            else if (dc.Ordinal == 9)
                            {
                                oporow[dc] = Math.Round(db8, 0);
                            }
                            else if (dc.Ordinal == 10)
                            {
                                oporow[dc] = Math.Round(db9, 0);
                            }
                            else if (dc.Ordinal == 11)
                            {
                                oporow[dc] = Math.Round(db10, 0);
                            }
                            else if (dc.Ordinal == 12)
                            {
                                oporow[dc] = Math.Round(db11, 0);
                            }
                            else if (dc.Ordinal == 13)
                            {
                                oporow[dc] = Math.Round(db12, 0);
                            }
                            else if (dc.Ordinal == 14)
                            {
                                oporow[dc] = Math.Round(db13, 0);
                            }
                            else if (dc.Ordinal == 15)
                            {
                                oporow[dc] = Math.Round(db14, 0);
                            }
                            else if (dc.Ordinal == 16)
                            {
                                oporow[dc] = Math.Round(db15, 0);
                            }
                            else if (dc.Ordinal == 17)
                            {
                                oporow[dc] = Math.Round(db16, 0);
                            }
                            else if (dc.Ordinal == 18)
                            {
                                oporow[dc] = Math.Round(db17, 0);
                            }
                            else if (dc.Ordinal == 19)
                            {
                                oporow[dc] = Math.Round(db18, 0);
                            }
                            else if (dc.Ordinal == 20)
                            {
                                oporow[dc] = Math.Round(db19, 0);
                            }
                            else if (dc.Ordinal == 21)
                            {
                                oporow[dc] = Math.Round(db20, 0);
                            }
                        }
                        oporow["Item_Name"] = "Grand Total";
                        mdt.Rows.InsertAt(oporow, 0);
                    }
                    Session["send_dt"] = mdt;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    fgen.Fn_open_rptlevelJS("WIP Report II From " + fromdt + " To " + todt + "", frm_qstr);
                    break;
                case "F25270":
                    SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.RCODE AS ERPCODE_TOCONVERT,B.INAME AS PRODUCT_TOCONVERT,A.ICODE AS PRODUCED_CODE,C.INAME AS PRODUCED_PRODUCT,A.IQTY_CHL AS QTY_PROD,D.KCLREELNO AS BATCHNO,A.ENT_BY,A.ENT_dT,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,ITEM B,ITEM C,REELVCH D WHERE TRIM(A.RCODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE)||A.MORDER= D.BRANCHCD||D.TYPE||TRIM(D.VCHNUM)||TO_CHAR(D.VCHDATE,'DD/MM/YYYY')||TRIM(D.ICODE)||D.SRNO AND A.BRANCHCD='" + mbr + "' AND A.TYPE='10' AND A.VCHDATE " + xprdrange + " ORDER BY vdd DESC,A.VCHNUM DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Conversion report";
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                //for checking purpose only

                case "FG_RWPL":
                    #region this is working sheet for mail no 401 for fg stock value
                    string mhd = System.DateTime.Now.Date.ToString("MM/yyyy");
                    string dd = System.DateTime.Now.Date.AddDays(-1).ToString("dd/MM/yyyy");
                    string xxp = "", xxp1 = "";
                    xxp = "vchdate between to_date('01/" + mhd + "','dd/MM/yyyy') and to_Date('01/" + mhd + "','dd/MM/yyyy')-1";
                    xxp1 = "vchdate between to_date('01/" + mhd + "','dd/mm/yyyy') and to_date('" + dd + "','dd/mm/yyyy') ";

                    dtm = new DataTable();
                    dtm.Columns.Add("icode", typeof(string));
                    dtm.Columns.Add("irate", typeof(double));
                    dtm.Columns.Add("qty", typeof(double));
                    dtm.Columns.Add("stock", typeof(double));

                    SQuery = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing   from (Select icode, yr_2021 as opening,0 as cdr,0 as ccr,0 as clos from itembal  where branchcd='" + mbr + "' and substr(icode,1,1)='9' union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and " + xxp + " and store='Y' and substr(icode,1,1)='9' GROUP BY ICODE union all  select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and " + xxp1 + " and store='Y' and substr(icode,1,1)='9' GROUP BY ICODE ) a, item c  where trim(A.icode)=trim(c.icodE) and length(trim(a.icode))>4 group by c.irate,trim(a.icode) /*having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0*/  order by trim(a.icode)";
                    DataTable fgstkdt = new DataTable();
                    fgstkdt = fgen.getdata(frm_qstr, co_cd, SQuery); ;//fg stock dt
                    DataTable ivchdt = new DataTable();
                    //squery = "select distinct icode,irate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where branchcd='00' and type like '4%' and vchdate between to_Date(sysdate-500,'dd/mm/yyyy') and to_Date('28/04/2021','dd/mm/yyyy' )and icode like '9%' order by vdd desc";
                    SQuery = "select distinct icode,irate,to_Char(orddt,'yyyymmdd') as vdd,orddt from somas where branchcd='" + mbr + "' and type like '4%' and orddt between to_Date(sysdate-500,'dd/mm/yyyy') and to_Date('" + dd + "','dd/mm/yyyy' )and icode like '9%' order by vdd desc";
                    ivchdt = fgen.getdata(frm_qstr, co_cd, SQuery);//sale rate
                    SQuery = "select icode,irate from item where substr(trim(icode),1,1)='9' and length(trim(icode))>4 order by icode";
                    DataTable itemdt = new DataTable();
                    itemdt = fgen.getdata(frm_qstr, co_cd, SQuery);//item rate

                    double papstkval = 0, rate = 0, fgstockval = 0; string icod = "";
                    for (int i = 0; i < fgstkdt.Rows.Count; i++)
                    {
                        db1 = 0; db = 0;
                        icod = fgstkdt.Rows[i]["icode"].ToString().Trim();
                        db = fgen.make_double(fgstkdt.Rows[i]["closing"].ToString().Trim());
                        rate = fgen.make_double(fgen.seek_iname_dt(ivchdt, "icode='" + icod.ToString().Trim() + "' ", "irate"));
                        if (rate == 0)
                        {
                            rate = fgen.make_double(fgen.seek_iname_dt(itemdt, "icode='" + icod.ToString().Trim() + "' ", "irate"));
                        }
                        fgstockval += db * rate;
                        dr1 = dtm.NewRow();
                        dr1["icode"] = icod;
                        dr1["irate"] = rate;
                        dr1["qty"] = db;
                        dr1["stock"] = db * rate;
                        db1 = db * rate;
                        if (db1 > 0)
                        {
                            dtm.Rows.Add(dr1);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("FG Report From " + fromdt + " To " + todt + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "RM_RWPL":
                    #region this is working sheet for mail no 401 for paper stock value--07 icode
                    mhd = System.DateTime.Now.Date.ToString("MM/yyyy");
                    dd = System.DateTime.Now.Date.AddDays(-1).ToString("dd/MM/yyyy");
                    xxp = ""; xxp1 = "";
                    dtm = new DataTable();
                    dtm.Columns.Add("icode", typeof(string));
                    dtm.Columns.Add("irate", typeof(double));
                    dtm.Columns.Add("stock", typeof(double));
                    dtm.Columns.Add("VALUE", typeof(double));
                    xxp = "vchdate between to_date('01/" + mhd + "','dd/MM/yyyy') and to_Date('01/" + mhd + "','dd/MM/yyyy')-1";
                    xxp1 = "vchdate between to_date('01/" + mhd + "','dd/mm/yyyy') and to_date('" + dd + "','dd/mm/yyyy') ";
                    //query1 = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing   from (Select icode, yr_2021 as opening,0 as cdr,0 as ccr,0 as clos from itembal  where branchcd='00' and substr(icode,1,2)='07' union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='00' and type like '%' and vchdate between to_date('01/04/2021','dd/MM/yyyy') and to_Date('01/04/2021','dd/MM/yyyy')-1  and store='Y' and substr(icode,1,2)='07' GROUP BY ICODE union all  select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='00' and type like '%' and vchdate between to_date('01/04/2021','dd/mm/yyyy') and to_date('27/04/2021','dd/mm/yyyy')  and store='Y' and substr(icode,1,2)='07' GROUP BY ICODE ) a, item c  where trim(A.icode)=trim(c.icodE)  group by c.irate,trim(a.icode) /*having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0*/  order by trim(a.icode)";
                    SQuery = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing   from (Select icode, yr_2021 as opening,0 as cdr,0 as ccr,0 as clos from itembal  where branchcd='" + mbr + "' and substr(icode,1,2)='07' union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and " + xxp + "  and store='Y' and substr(icode,1,2)='07' GROUP BY ICODE union all  select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and " + xxp1 + " and store='Y' and substr(icode,1,2)='07' GROUP BY ICODE ) a, item c  where trim(A.icode)=trim(c.icodE) and length(trim(a.icode))>4 group by c.irate,trim(a.icode) /*having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0*/  order by trim(a.icode)";
                    DataTable papstkdt = new DataTable();
                    papstkdt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //query2 = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='00' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' and substr(a.icode,1,2)='07' and a.vchdate>=to_DatE('01/04/2020','DD/MM/YYYY') and a.vchdate<=to_DatE('27/04/2021','dd/mm/yyyy') and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc";
                    // SQuery = "select distinct to_Char(a.VCHDATE,'yyyymmdd') as vdd,trim(a.icode) as icode,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + dr["type1"].ToString().Trim() + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' and substr(a.icode,1,2)='07' and a.vchdate>=to_DatE('01/" + mhd + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + dd + "','dd/mm/yyyy') and a.iqtyin>0  order by icode,vdd desc"; //old
                    SQuery = "select distinct to_Char(a.VCHDATE,'yyyymmdd') as vdd,trim(a.icode) as icode,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' and substr(a.icode,1,2)='07' and a.vchdate>=to_DatE('01/04/2020','DD/MM/YYYY') and a.vchdate<=to_DatE('" + dd + "','dd/mm/yyyy') and a.iqtyin>0  order by icode,vdd desc";
                    DataTable mrr_dt = new DataTable();
                    mrr_dt = fgen.getdata(frm_qstr, co_cd, SQuery); ;//mrr dt for rate
                    papstkval = 0; rate = 0; icod = "";
                    //loop for paper stock value
                    for (int i = 0; i < papstkdt.Rows.Count; i++)
                    {
                        db = 0; db1 = 0;
                        db = fgen.make_double(papstkdt.Rows[i]["closing"].ToString().Trim());
                        icod = papstkdt.Rows[i]["icode"].ToString().Trim();
                        rate = fgen.make_double(fgen.seek_iname_dt(mrr_dt, "icode='" + icod.ToString().Trim() + "' ", "irate"));
                        papstkval += db * rate;

                        dr1 = dtm.NewRow();
                        dr1["icode"] = icod;
                        dr1["irate"] = rate;
                        dr1["stock"] = db;
                        dr1["VALUE"] = db * rate;
                        db1 = db * rate;
                        if (db1 > 0)
                        {
                            dtm.Rows.Add(dr1);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("RM Report From " + fromdt + " To " + todt + "", frm_qstr);
                    }
                    #endregion

                    break;
                case "F25244C":
                    DataTable dtBranch = new DataTable();
                    dtBranch = fgen.getdata(frm_qstr, co_cd, "SELECT TYPE1,VCHNUM AS NICK_NAME FROM TYPE WHERE ID='B' ORDER BY TYPE1");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    string PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    mq0 = ""; mq1 = "";
                    foreach (DataRow drBrn in dtBranch.Rows)
                    {
                        mq0 += ",decode(a.branchcd,'" + drBrn["type1"].ToString().TrimStart() + "',sum(a.opening)+sum(a.cdr)-sum(a.ccr),0) as " + drBrn["NICK_NAME"].ToString().Replace(" ", "_") + "_stk";
                        mq1 += ",sum(" + drBrn["NICK_NAME"].ToString().Replace(" ", "_") + "_stk) as " + drBrn["NICK_NAME"].ToString().Replace(" ", "_") + "_stk";
                    }
                    string icodeIn = "";

                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) icodeIn = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) icodeIn = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                    SQuery = "SELECT item_name,Icode AS ERPCODE " + mq1 + " FROM (select b.Iname Item_Name,sum(a.opening) as Opening_Bal,sum(a.cdr) as Inward_Qty,sum(a.ccr) as Outward_Qty,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,b.Unit,b.no_proc as sec_unit,b.Cpartno,trim(a.Icode) as Icode,substr(trim(a.Icode),1,2) as Grp,substr(trim(a.Icode),1,4) as Sub_Grp,a.branchcd " + mq0 + "  from ( Select Icode, nvl(YR_" + frm_myear + ",0) as opening,0 as cdr,0 as ccr,0 as clos,nvl(binno,'-') as Bin_Locn,'S2' as Dset,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord,branchcd from itembal where trim(icode) like '%%' and 1=1 union all select Icode,iqtyin-iqtyout as op,0 as cdr,0 as ccr,0 as clos,null as locn,'S2' as Dset,0 as minl,0 as mxl,0 as mrol,branchcd from ivoucher where type like '%' and vchdate between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1  and store='Y' and trim(icode) like '%%' union all select icode,0 as op,iqtyin as cdr,iqtyout as ccr,0 as clos,null as locn,'S2' as Dset,0 as minl,0 as mxl,0 as mrol,branchcd from ivoucher where type like '%' and vchdate " + PrdRange + " and store='Y' and trim(icode) like '%%')a left outer join item b on trim(A.icode)=trim(B.icodE) where 1=1 " + icodeIn + " group by Dset,b.iname,b.unit,b.no_proc,b.cpartno,trim(a.icode),substr(trim(a.icode),1,2),substr(trim(a.icode),1,4),b.SERVICABLE,a.branchcd having sum(abs(a.opening))+sum(a.cdr)+sum(a.ccr)!=0 ) GROUP BY item_name,Icode ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Stock Ledger (All Branch) for the period " + fromdt + " to " + todt + " ", frm_qstr);
                    break;
            }
        }
    }

    private void wiptotstk_SVPL()
    {
        mq0 = "";
        mq0 = fgen.seek_iname(frm_qstr, co_cd, "select wipstdt from type where id='B' and type1='" + mbr + "'", "wipstdt");
        //SQuery = "(SELECT ITEM_NAME,ERP_CODE,OPENING,RCPTS,ISSUES,CLOSING,WIP_VALUE,RATES,RATE_SOURCE,UNIT,GRP,CPARTNO,IWEIGHT,CLOSING_WT FROM (select B.Iname as Item_Name,trim(a.Icode) as Erp_Code,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing,to_Char((sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))),'999999999.99') as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.iqd,a.irate) as ichgs FROM item a where length(Trim(a.icode))>4 and nvl(a.iqd,a.irate)>0 ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate  between to_Date('" + mq0 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='61' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + mq0 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='0' or stage='61') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  between to_date('" + mq0 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='61' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate  between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy') AND VCHDATE>=TO_DATE('" + mq0 + "','dd/mm/yyyy') and type!='XX' and (trim(acode)='0' or stage='61') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy') AND VCHDATE>=TO_DATE('" + mq0 + "','dd/mm/yyyy') and type!='XX' and stage='61' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='0' or stage='61') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)) having sum(opening)+sum(cdr)-sum(ccr)>0 ))";
        SQuery = "(SELECT ITEM_NAME,ERP_CODE,OPENING,RCPTS,ISSUES,CLOSING,WIP_VALUE,RATES,RATE_SOURCE,UNIT,GRP,CPARTNO,IWEIGHT,CLOSING_WT FROM (select B.Iname as Item_Name,trim(a.Icode) as Erp_Code,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing,to_Char((sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))),'999999999.99') as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.iqd,a.irate) as ichgs FROM item a where length(Trim(a.icode))>4 and nvl(a.iqd,a.irate)>0 ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate  between to_Date('" + mq0 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')  and stage='61' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + mq0 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='0' or stage='61') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  between to_date('" + mq0 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='61' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate  between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy') AND VCHDATE>=TO_DATE('" + mq0 + "','dd/mm/yyyy') and type!='XX' and (trim(acode)='0' or stage='61') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy') AND VCHDATE>=TO_DATE('" + mq0 + "','dd/mm/yyyy') and type!='XX' and stage='61' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='0' or stage='61') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)) ))";
        fgen.execute_cmd(frm_qstr, co_cd, "create or replace view wiptotstkw_" + mbr + " as(SELECT * FROM (" + SQuery + "))");
    }

    void wip_stk_vw_SVPL(string TODATE)
    {
        //todt = TODATE;
        mq10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT WIPSTDT FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "'", "WIPSTDT");
        // xprd2 = "between to_Date('" + mq10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
        xprd2 = "between to_Date('" + mq10 + "','dd/mm/yyyy') and to_date('" + TODATE + "','dd/mm/yyyy')";
        //SQuery = "select trim(Item_name) as Item_name,trim(Part_No) as Part_No,sum(stg01) as stg01,sum(stg02) as stg02,sum(stg03) as stg03,sum(stg04) as stg04,sum(stg05) as stg05,sum(stg06) as stg06,sum(stg07) as stg07,sum(stg08) as stg08,sum(stg09) as stg09,sum(stg11) as stg11,sum(stg12) as stg12,sum(stg13) as stg13,sum(stg14) as stg14,sum(stg15) as stg15,sum(stg16) as stg16,sum(stg01)+sum(stg02)+sum(stg03)+sum(stg04)+sum(stg05)+sum(stg06)+sum(stg07)+sum(stg08)+sum(stg09)+sum(stg11)+sum(stg12)+sum(stg13)+sum(stg14)+sum(stg15) as total,trim(icode)as  icode,iweight,wt_net,'-' as wolink,ac_acode,irate from (select ac_acode,Item_Name,Part_No,irate,iweight,wt_net,mat5,mat6,mat7,salloy,decode(stage,'61',Balance,0) as Stg01,decode(stage,'62',Balance,0) as Stg02,decode(stage,'63',balance,0) as Stg03,decode(stage,'64',balance,0) as Stg04,decode(stage,'65',balance,0) as Stg05,decode(stage,'66',balance,0) as Stg06,decode(stage,'67',balance,0) as Stg07,decode(stage,'68',balance,0) as Stg08,decode(stage,'69',balance,0)as Stg09,decode(stage,'6A',balance,0) as Stg11,decode(stage,'6B',balance,0) as Stg12,decode(stage,'6C',balance,0) as Stg13,decode(stage,'6D',balance,0) as Stg14,decode(stage,'6E',balance,0) as Stg15,decode(stage,'6R',balance,0) as Stg16,icode,'-' as wolink  from (select A.TYPE,C.ac_acode,C.iname as Item_Name,c.irate,c.cpartno as Part_No,c.iweight,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,sum(a.iqtyin) as Input,sum(a.iqtyout) as Output,sum(a.iqtyin)-sum(a.iqtyout) as Balance,trim(a.stage) as stage,a.icode,a.wolink from (select type,stage,maincode,icode,iqtyin,iqtyout,'op' as wolink From wipstk where branchcd='" + mbr + "' and type='50' and vchdate  " + xprd2 + "  union all select type,stage,icode,icode,iqtyin,iqtyout,'WIP' as wolink From ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + "  and store='W' union all select '50' as type,'6C' as stage,icode,icode,closing as iqtyin,0 as iqtyout,'itmbal' as wolink From MS_STK_00 where substr(icode,1,1) in ('9','7') union all select type,stage,icode,icode,iqtyout ,iqtyin,'IVCH' as wolink From ivoucher where branchcd='" + mbr + "' and (type like '3%' or type like '1%') and vchdate " + xprdrange1 + "  and store='Y'  and substr(icode,1,1) in ('7') and stage like '6%'  union all (select '00' AS type,stage,TRIM(MCCD) AS MCCD,TRIM(icode) AS ICODE,SUM(iqtyout),SUM(iqtyin),'-' AS wolink FROM (select type,'64' as stage,icode AS MCCD,icode,iqtyout,0 as iqtyin,'RGP' as wolink From RGPMST where branchcd='" + mbr + "' and type ='21' and vchdate " + xprd2 + "  union all select type,'62' as stage,icode,icode,0 as iqtyout,iqtyin+nvl(rej_rw,0),'MRR' as wolink From ivoucher where branchcd='" + mbr + "' and type ='09' and vchdate  " + xprd2 + "  and store<>'R') GROUP BY STAGE,TRIM(MCCD),TRIM(ICODE) HAVING SUM(iqtyout)>SUM(iqtyin)) ) a,(Select icode,iname,cpartno,cdrgno,wt_net,mat5,mat6,mat7,salloy,irate,iweight,ac_acode from item where trim(nvl(pur_uom,'Y'))='Y' and substr(icode,1,1) in ('9','7'))c where trim(a.icode)=trim(c.icode) group by C.ac_acode,C.iname,c.cpartno,c.irate,c.iweight,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,A.TYPE,trim(a.stage),a.icode,a.wolink)) group by ac_acode,trim(Item_Name),trim(Part_No),trim(Icode),iweight,irate,wt_net,mat5,mat6,mat7,salloy order by trim(Item_Name)";
        SQuery = "(SELECT ITEM_NAME,PART_NO,STG01,STG02,STG03,STG04,STG05,STG06,STG07,STG08,STG09,STG11,STG12,STG13,STG14,STG15,STG16,TOTAL,ICODE,IWEIGHT,WT_NET,WOLINK,PCRT FROM (select trim(Item_name) as Item_name,trim(Part_No) as Part_No,sum(stg01) as stg01,sum(stg02) as stg02,sum(stg03) as stg03,sum(stg04) as stg04,sum(stg05) as stg05,sum(stg06) as stg06,sum(stg07) as stg07,sum(stg08) as stg08,sum(stg09) as stg09,sum(stg11) as stg11,sum(stg12) as stg12,sum(stg13) as stg13,sum(stg14) as stg14,sum(stg15) as stg15,sum(stg16) as stg16,sum(stg01)+sum(stg02)+sum(stg03)+sum(stg04)+sum(stg05)+sum(stg06)+sum(stg07)+sum(stg08)+sum(stg09)+sum(stg11)+sum(stg12)+sum(stg13)+sum(stg14)+sum(stg15)+sum(stg16) as total,trim(icode)as  icode,iweight,wt_net,wolink,iqd as Pcrt from (select Item_Name,Part_No,iweight,wt_net,iqd,mat5,mat6,mat7,salloy,decode(stage,'61',Balance,0) as Stg01,decode(stage,'62',Balance,0) as Stg02,decode(stage,'63',balance,0) as Stg03,decode(stage,'64',balance,0) as Stg04,decode(stage,'65',balance,0) as Stg05,decode(stage,'66',balance,0) as Stg06,decode(stage,'67',balance,0) as Stg07,decode(stage,'68',balance,0) as Stg08,decode(stage,'69',balance,0) as Stg09,decode(stage,'6A',balance,0) as Stg11,decode(stage,'6B',balance,0) as Stg12,decode(stage,'6C',balance,0) as Stg13,decode(stage,'6D',balance,0) as Stg14,decode(stage,'6E',balance,0) as Stg15,decode(stage,'6R',balance,0) as Stg16,icode,wolink  from (select C.iname as Item_Name,C.Cpartno as Part_No,c.iweight,c.iqd,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,sum(a.iqtyin) as Input,sum(a.iqtyout) as Output,sum(a.iqtyin)-sum(a.iqtyout) as Balance,trim(a.stage) as stage,a.icode,'-' AS WOLINK from (select type,stage,maincode,icode,iqtyin,iqtyout,NVL(wolink,'-') AS WOLINK From wipstk where branchcd='" + mbr + "' and type='50' and vchdate " + xprd2 + " union all select type,(case when trim(stage)='-' then '6D' else stage end) as stage,icode,icode,iqtyin,iqtyout,NVL(ccent,'-') as wolink From ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and store='W' and substr(icode,1,2) in ('82','84') union all select type,(case when trim(stage)='-' then '6D' else stage end) as stage,icode,icode,iqtyout,iqtyin,NVL(ccent,'-') as wolink From ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('1','3') and type not in ('15','16','17','36') and vchdate " + xprd2 + " and store='Y' and substr(icode,1,2) in ('82','84')) a,(Select icode,iname,cpartno,iqd,wt_net,mat5,mat6,mat7,salloy,iweight from item where substr(icode,1,1) in ('7','8','9'))c where trim(a.icode)=trim(c.icode) group by C.iname,C.cpartno,c.iweight,c.iqd,c.wt_net,c.mat5,c.mat6,c.mat7,c.salloy,trim(a.stage),a.icode )) group by trim(Item_Name),trim(Part_No),trim(Icode),iweight,wt_net,iqd,mat5,mat6,mat7,salloy,wolink having abs(sum(stg01))+abs(sum(stg02))+abs(sum(stg03))+abs(sum(stg04))+abs(sum(stg05))+abs(sum(stg06))+abs(sum(stg07))+abs(sum(stg08))+abs(sum(stg09))+sum(stg11)+sum(stg12)+sum(stg13)+sum(stg14)+sum(stg15)+sum(stg16)<>0 order by trim(Item_Name)))";
        fgen.execute_cmd(frm_qstr, co_cd, "create or replace view wipcolstkw_" + mbr + " as(SELECT * FROM (" + SQuery + "))");
    }

    void updItemPosting()
    {
        string lastyr, cyear = "YR_" + year;
        int clen;

        SQuery = "UPDATE ITEMBAL SET BR_ICODE=BRANCHCD||TRIM(ICODE)";
        fgen.execute_cmd(frm_qstr, co_cd, SQuery);

        clen = co_cd.Length;
        lastyr = fgen.seek_iname(frm_qstr, co_cd, "select trim(code)||'~'||to_char(fmdate,'dd/mm/yyyy')||'~'||to_Char(todate,'dd/mm/yyyy')||'~'||'Yr_'||substr(trim(code),-4) as yrx from co where substr(code,1," + clen + ") like '" + co_cd + "%' and trim(code)<'" + co_cd + year + "' order by fmdate desc", "yrx");
        if (lastyr != "0")
        {
            xprd1 = " between to_date('" + lastyr.Split('~')[1] + "','dd/mm/yyyy') and to_Date('" + lastyr.Split('~')[1] + "','dd/mm/yyyy')-1";
            xprd2 = " between to_date('" + lastyr.Split('~')[1] + "','dd/mm/yyyy') and to_Date('" + lastyr.Split('~')[2] + "','dd/mm/yyyy')";
            mq0 = "select substr(trim(icode),1,8) as icode,sum(opening) as opening,sum(cdr) as CDBTS,sum(ccr) as CCDTS,sum(opening)+sum(cdr)-sum(ccr) as closing from (Select icode, " + lastyr.Split('~')[3] + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + mbr + "' and length(trim(icode))>=8 union all  ";
            mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' and length(trim(icode))>=8 GROUP BY ICODE union all ";
            mq2 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and store='Y' and length(trim(icode))>=8 GROUP BY ICODE ) group by substr(trim(icode),1,8) having sum(opening)+sum(cdr)-sum(ccr)<>0 order by substr(trim(icode),1,8)";
            SQuery = mq0 + mq1 + mq2;

            DataTable rsitms, rssample = new DataTable();

            fgen.execute_cmd(frm_qstr, co_cd, "update itembal set " + cyear + "=0 where branchcd='" + mbr + "'");

            rsitms = fgen.getdata(frm_qstr, co_cd, SQuery);
            SQuery = "Select icode," + cyear + " as curryr from itembal where branchcd='" + mbr + "'";
            rssample = fgen.getdata(frm_qstr, co_cd, SQuery);
            string mhd = "";
            foreach (DataRow dr in rsitms.Rows)
            {
                if (rssample.Rows.Count <= 0)
                {
                    SQuery = "insert into itembal(branchcd,icode,br_icode," + cyear + ")values('" + mbr + "','" + dr["icode"].ToString().Trim() + "','" + mbr + dr["ICODE"].ToString().Trim() + "'," + dr["closing"].ToString().Trim() + ")";
                    fgen.execute_cmd(frm_qstr, co_cd, SQuery);
                }
                else
                {

                    mhd = fgen.seek_iname_dt(rssample, "ICODE='" + dr["ICODE"].ToString().Trim() + "'", "ICODE");
                    if (mhd != "0")
                    {
                        SQuery = "update itembal set " + cyear + "='" + dr["closing"].ToString().Trim() + "' where branchcd='" + mbr + "' and trim(icode)='" + dr["icode"].ToString().Trim() + "'";
                        fgen.execute_cmd(frm_qstr, co_cd, SQuery);
                    }
                    else
                    {
                        string mmcode = "";
                        mmcode = dr["ICODE"].ToString().Trim();
                        SQuery = "insert into itembal(branchcd,icode,br_icode," + cyear + ")values('" + mbr + "','" + mmcode + "','" + mbr + mmcode + "'," + dr["closing"].ToString().Trim() + ")";
                        fgen.execute_cmd(frm_qstr, co_cd, SQuery);
                    }
                }
            }
            fgen.msg("-", "AMSG", "Posting completed");
            fgen.save_Mailbox2(frm_qstr, co_cd, "Item Posting", mbr, "Item Posting done on (" + DateTime.Now.ToString("dd/MM/yyyy") + "), by " + uname + "", uname, "");
        }
    }

    public DataTable Gen_stk_pl_S(string xdt, string igrp) //this fun for closing summary
    {
        DataTable ph_tbl, dtoutflow, dtinflow, dtindr, mrs, dt7, dt8, dt9, dt10, dt11, dtBarCode1, mainDt1;
        int cnt = 0, cntr = 0, d = 0, j = 0, a = 0, n = 0; double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0, db9 = 0, db10 = 0, db11 = 0, db12 = 0, db13 = 0, db14 = 0, itot_stk = 0, to_cons = 0, itv = 0; int m = 0;

        header_n = "RM Closing Stock Summary";

        mq4 = "type1='" + mq1 + "'";
        cond = "substr(a.icode,1,2)='" + mq1 + "'";
        cond1 = "substr(icode,1,2)='" + mq1 + "'";

        //cond = "substr(a.icode,1,8)='02030100'";
        //cond1 = "substr(icode,1,8)='02030100'";

        header_n = "RM Closing Stock Summary";
        value1 = "";
        mrs = new DataTable(); DataRow mrrow = null; DataTable dt = new DataTable(); DataTable dt1 = new DataTable(); DataTable dt3 = new DataTable();
        DataTable dt6 = new DataTable();
        dt7 = new DataTable(); DataTable dtm = new DataTable(); DataTable cons_Dt = new DataTable();
        string opbalyr = "", param = "", mq0 = "", xprd2 = "";
        ////////////ph_tbl is final cursor for report
        #region
        ph_tbl = new DataTable();
        ph_tbl.Columns.Add(new DataColumn("fromdt", typeof(DateTime)));
        ph_tbl.Columns.Add(new DataColumn("todt", typeof(DateTime)));
        ph_tbl.Columns.Add(new DataColumn("header", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("mcode", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("mname", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("scode", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("sname", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("icode", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("iname", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("unit", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("cpartno", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("maker", typeof(string)));
        ph_tbl.Columns.Add(new DataColumn("opening", typeof(double)));//op bal in this
        ph_tbl.Columns.Add(new DataColumn("op_value", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("inw_Qty", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("inw_val", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("out_qty", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("out_val", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("cons_qty", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("cons_val", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("clos_qty", typeof(double)));//show clos bal in this 
        ph_tbl.Columns.Add(new DataColumn("clos_val", typeof(double)));
        ph_tbl.Columns.Add(new DataColumn("avg_rate", typeof(double)));
        ///////////////////////mrs is cursor for detail report
        mrs.Columns.Add(new DataColumn("Icode", typeof(string)));
        mrs.Columns.Add(new DataColumn("Iname", typeof(string)));
        mrs.Columns.Add(new DataColumn("mrr", typeof(string)));
        mrs.Columns.Add(new DataColumn("mrrdt", typeof(DateTime)));
        mrs.Columns.Add(new DataColumn("qty", typeof(double)));
        mrs.Columns.Add(new DataColumn("rate", typeof(double)));
        mrs.Columns.Add(new DataColumn("stock", typeof(double)));
        ////////////
        dtm.Columns.Add(new DataColumn("Icode", typeof(string)));
        dtm.Columns.Add(new DataColumn("Iname", typeof(string)));
        dtm.Columns.Add(new DataColumn("mrr", typeof(string)));
        dtm.Columns.Add(new DataColumn("mrrdt", typeof(DateTime)));
        dtm.Columns.Add(new DataColumn("qty", typeof(double)));
        dtm.Columns.Add(new DataColumn("rate", typeof(double)));
        dtm.Columns.Add(new DataColumn("stock", typeof(double)));
        #endregion
        DataRow dr4 = mrs.NewRow();
        DataRow dr3 = ph_tbl.NewRow();
        //fgen.execute_cmd(frm_qstr, frm_cocd, "Delete from itemvbal13 a where a.branchcd='" + frm_mbr + "' " + cond + "");
        opbalyr = "yr_" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_CHAR(TO_DATE(PARAMS,'DD/MM/YYYY'),'YYYY') AS params FROM CONTROLS WHERE ID='R02'", "params");
        param = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS AS params FROM CONTROLS WHERE ID='R02'", "params");
        xprd1 = "BETWEEN TO_dATE('" + param + "','dd/mm/yyyy') and to_Date('" + xdt + "','dd/mm/yyyy')-1 ";
        xprd2 = "BETWEEN TO_dATE('" + xdt + "','dd/mm/yyyy') and to_Date('" + xdt + "','dd/mm/yyyy') ";


        opbalyr = "yr_" + frm_myear;
        xprd1 = "BETWEEN TO_dATE('" + frm_cDt1 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 ";
        xprd2 = "BETWEEN TO_dATE('" + fromdt + "','dd/mm/yyyy') and to_Date('" + xdt + "','dd/mm/yyyy') ";

        #region for closing
        //SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' " + cond + " and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') " + cond + " and a.iqtyin>0 and substr(trim(a.icode),1,2)='" + mq4 + "' order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //changes by yogita...old
        SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' and " + cond + " and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //changes by yogita........
        //SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y'  and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') and substr(trim(a.icode),1,4)='0706' and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //for testing on single icode
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //this is mrr dt for only closing 
        //////////////////
        // SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' " + cond + " and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + fromdt + "','dd/mm/yyyy') " + cond + " and a.iqtyin>0 and substr(trim(a.icode),1,2)='" + mq4 + "' order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //changes by yogita.....old
        SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' and " + cond + " and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + fromdt + "','dd/mm/yyyy') and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //changes by yogita
        //SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y'  and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') and substr(trim(a.icode),1,4)='0706' and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //for testing on single icode
        dt9 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //this is mrr dt for only opening 
        //////////////=================below is closing dt
        #endregion
        #region this is old query only for closing balance
        //mq0 = "select c.irate,trim(a.icode) as icode,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "' " + cond.Replace("a.", "") + " union all  ";      //by yogita    
        //mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='Y' " + cond.Replace("a.", "") + " GROUP BY ICODE union all ";//BY ME     
        //mq2 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + " and store='Y' " + cond.Replace("a.", "") + " GROUP BY ICODE )a, type b,item c  where trim(A.icode)=trim(c.icodE) and substr(a.icode,1,2)=b.type1 and b.id='Y' and nvl(b.rcnum,'Y')!='N' and substr(trim(a.icode),1,2)='" + mq4 + "' group by c.irate,trim(a.icode) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0  order by trim(a.icode)";
        //SQuery = mq0 + mq1 + mq2;
        #endregion
        //SQuery = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing   from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal  where branchcd='" + frm_mbr + "' and substr(trim(icode),1,2)='" + mq4 + "'  union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + "  and store='Y' and substr(trim(icode),1,2)='" + mq4 + "' GROUP BY ICODE union all  select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + "  and store='Y' and substr(trim(icode),1,2)='" + mq4 + "' GROUP BY ICODE )a, item c  where trim(A.icode)=trim(c.icodE)  group by c.irate,trim(a.icode) /*having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0*/  order by trim(a.icode)";//old
        SQuery = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing   from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal  where branchcd='" + frm_mbr + "' and " + cond1 + " union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + "  and store='Y' and " + cond1 + " GROUP BY ICODE union all  select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + "  and store='Y' and " + cond1 + " GROUP BY ICODE ) a, item c  where trim(A.icode)=trim(c.icodE)  group by c.irate,trim(a.icode) /*having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0*/  order by trim(a.icode)";
        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //closing bal dt1
        ///////////////
        //mq5 = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "' and substr(trim(icode),1,2)='" + mq4 + "'  union all";
        mq5 = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "' and " + cond1 + " union all";
        //mq6 = " select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + "  and store='Y' and substr(trim(icode),1,2)='" + mq4 + "' GROUP BY ICODE union all ";//old
        mq6 = " select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + "  and store='Y' and " + cond1 + " GROUP BY ICODE union all ";
        //mq7 = " select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + "  and store='Y' and substr(trim(icode),1,2)='" + mq4 + "' GROUP BY ICODE )a, type b,item c  where trim(A.icode)=trim(c.icodE) and substr(a.icode,1,2)=b.type1 and b.id='Y' and nvl(b.rcnum,'Y')!='N' group by c.irate,trim(a.icode) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>=0  order by trim(a.icode)";
        mq7 = " select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + "  and store='Y' and " + cond1 + " GROUP BY ICODE )a, type b,item c  where trim(A.icode)=trim(c.icodE) and substr(a.icode,1,2)=b.type1 and b.id='Y' and nvl(b.rcnum,'Y')!='N' group by c.irate,trim(a.icode) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>=0  order by trim(a.icode)";
        SQuery = mq5 + mq6 + mq7;
        dt7 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for opening balance
        ////////////////
        //SQuery = "select icode,iname from item where substr(trim(icode),1,2) in ('" + mq4 + "') and length(trim(icode))=4";//old
        SQuery = "select icode,iname from item where " + cond1 + " and length(trim(icode))=4";
        DataTable dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for subgroup name and code
        SQuery = "select TYPE1 AS MGCODE,NAME AS MNAME from type where " + mq4 + " and id='Y'";////-------------need chnages in this
        DataTable dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for MAINGROUP name and code
        //SQuery = "select distinct icode,iname,unit from item where length(trim(icode))>=8 and substr(trim(icode),1,2) in ('" + mq4 + "') order by icode";//old
        SQuery = "select distinct icode,iname,unit,cpartno,maker from item where length(trim(icode))>=8 and " + cond1 + "  order by icode"; //and icode='10060001' 
        DataTable dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for main item or name
        //////=====dt3 me mne without store data pic kiya hua tha then main me dekha to usme summry me store Y ka data aara tha but detail (25168) me main me Y,N both store ka data aara tha..agar user kahega ki detail se mtch kro then is qry me dono store pas kr denge
        //SQuery = "select sum(iqtyin) as qty ,sum((iqtyin)*irate) as val,irate ,icode from ivoucher where branchcd='" + frm_mbr + "' and type like '0%'  and vchdate " + xprdRange + "  group by icode,irate,TYPE  ORDER BY TYPE";//OLD IN THIS STORE Y AND N DATA IS COMING
        //SQuery = "select sum(iqtyin) as qty ,sum((iqtyin)*irate) as val,irate ,icode from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1) in ('0','1') AND STORE='Y' and vchdate " + xprdRange + "  group by icode,irate,TYPE  ORDER BY TYPE"; //SET STORE=Y BECAUSE IN MAIN Tejaxo ONLY STORE Y DATA IS COMING IN THIS REPORT
        SQuery = "select sum(a.iqtyin) as qty ,sum((a.iqtyin)*(case when a.irate>0 then a.irate else b.irate end) ) as val,(case when a.irate>0 then a.irate else b.irate end) irate ,a.icode from ivoucher a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1) in ('0','1') AND a.STORE='Y' and a.vchdate " + xprdRange + " and " + cond + "  group by a.icode,a.irate,a.TYPE,b.irate  ORDER BY a.TYPE"; //SET STORE=Y BECAUSE IN MAIN Tejaxo ONLY STORE Y DATA IS COMING IN THIS REPORT
        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt3 for inward qty and value

        //SQuery = "select sum(iqtyin) as qty ,sum((iqtyin)*irate) as val,irate ,icode from ivoucher where branchcd='" + frm_mbr + "' and  type like '2%' OR TYPE LIKE '4%' and vchdate " + xprdRange + "  group by icode,irate,TYPE ORDER BY TYPE";
        SQuery = "select sum(a.iqtyin) as qty ,sum((a.iqtyin)* (case when a.irate>0 then a.irate else b.irate end) ) as val,(case when a.irate>0 then a.irate else b.irate end) as irate ,a.icode from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and (a.type like '2%' OR a.TYPE LIKE '4%') and A.STORE='Y' and a.vchdate " + xprdRange + " and " + cond + " group by a.icode,a.irate,a.TYPE,b.irate ORDER BY a.TYPE";
        dt8 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt8 for inward qty and value

        //========consumption dt
        SQuery = "select trim(a.icode) as icode,sum(nvl(a.iqtyout,0)) as cons_qty,sum(nvl(a.iqtyout,0)*(case when a.irate>0 then a.irate else b.irate end)) as cons_val  from ivoucher a,item b where  trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '3%' and " + cond + " and A.STORE='Y' AND a.vchdate " + xprdRange + " group by trim(a.icode)";
        cons_Dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        ///////////      
        for (int k = 0; k < dt2.Rows.Count; k++)
        {
            // foreach (DataRow drrstk in dt1.Rows) //stock dt...this is for closing bal fill in dt
            //foreach (DataRow drrstk in dt2.Rows) //dt2 for all item for selected group
            //{
            #region end of foreach loop
            itot_stk = 0; to_cons = 0; itv = 0; db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; m = 0;

            // to_cons = Convert.ToDouble(drrstk["closing"]);
            to_cons = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "closing"));
            db2 = to_cons;
            foreach (DataRow stk_chk in dt.Rows) //ivoucher dt
            {
                /// //dt.Select(drrstk["icode"].ToString());
                #region for details of closing balance
                if (dt.Rows.Count == 0)
                {
                    #region
                    // fgen.execute_cmd(frm_qstr, frm_cocd, "insert into itemvbal13(irate,branchcd,type,vchnum,vchdate,invno,invdate,acode,icode,iqtyin)values(" + Convert.ToInt32(stk_chk["irate"]) + ",'" + frm_mbr + "','" + stk_chk["type"].ToString() + "','" + stk_chk["vchnum"].ToString() + "',to_datE('" + stk_chk["vchdate"].ToString() + "','dd/mm/yyyy'),'" + value1 + "',to_Date('" + stk_chk["invdate"].ToString() + "','dd/mm/yyyy'),'" + stk_chk["acode"].ToString().Trim() + "','" + stk_chk["icode"].ToString().Trim() + "'," + to_cons + ")");
                    dr4 = mrs.NewRow();
                    dr4["icode"] = stk_chk["icode"].ToString().Trim();
                    dr4["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr4["icode"].ToString().Trim() + "'", "iname");
                    dr4["mrr"] = stk_chk["vchnum"].ToString().Trim();
                    dr4["mrrdt"] = stk_chk["vchdate"].ToString().Trim();
                    dr4["qty"] = stk_chk["balance"].ToString().Trim();
                    dr4["rate"] = stk_chk["irate"].ToString().Trim();
                    db1 += fgen.make_double(dr4["qty"].ToString().Trim()) * fgen.make_double(dr4["rate"].ToString().Trim()); //for closing val
                    db3 += fgen.make_double(dr4["qty"].ToString().Trim());
                    dr4["stock"] = db2;
                    mrs.Rows.Add(dr4);
                    to_cons = 0;
                    #endregion
                }
                else
                {
                    #region
                    if (stk_chk["icode"].ToString().Trim() == dt2.Rows[k]["icode"].ToString().Trim())
                    {
                        if (Convert.ToDouble(stk_chk["balance"].ToString()) >= to_cons && to_cons > 0)
                        {
                            itot_stk = itot_stk + (to_cons * Convert.ToInt32(stk_chk["irate"]));
                            itv = itv + (to_cons * Convert.ToInt32(stk_chk["irate"]));
                            value1 = stk_chk["invno"].ToString();
                            if (value1.ToString().Trim().Length > 10) value1 = value1.Trim().Substring(0, 10);
                            dr4 = mrs.NewRow();
                            dr4["icode"] = stk_chk["icode"].ToString().Trim();
                            dr4["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr4["icode"].ToString().Trim() + "'", "iname");
                            dr4["mrr"] = stk_chk["vchnum"].ToString().Trim();
                            dr4["mrrdt"] = stk_chk["vchdate"].ToString().Trim();
                            dr4["qty"] = to_cons;
                            dr4["rate"] = stk_chk["irate"].ToString().Trim();
                            dr4["stock"] = db2;// drrstk["closing"].ToString().Trim();
                            db1 += fgen.make_double(dr4["qty"].ToString().Trim()) * fgen.make_double(dr4["rate"].ToString().Trim()); //for closing val
                            db3 += fgen.make_double(dr4["qty"].ToString().Trim());
                            mrs.Rows.Add(dr4);
                            to_cons = 0;
                            // db1 = 0;
                        }
                        else
                        {
                            if (to_cons > 0)
                            {
                                value1 = stk_chk["invno"].ToString();
                                if (value1.ToString().Trim().Length > 10) value1 = value1.Trim().Substring(0, 10);
                                dr4 = mrs.NewRow();
                                dr4["icode"] = stk_chk["icode"].ToString().Trim();
                                dr4["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr4["icode"].ToString().Trim() + "'", "iname");
                                dr4["mrr"] = stk_chk["vchnum"].ToString().Trim();
                                dr4["mrrdt"] = stk_chk["vchdate"].ToString().Trim();
                                dr4["rate"] = stk_chk["irate"].ToString().Trim();
                                dr4["stock"] = db2;// drrstk["closing"].ToString().Trim();
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                itot_stk = itot_stk + (Convert.ToInt32(stk_chk["balance"]) * Convert.ToInt32(stk_chk["irate"]));
                                itv = itv + (Convert.ToInt32(stk_chk["balance"]) * Convert.ToInt32(stk_chk["irate"]));
                                to_cons = to_cons - Convert.ToInt32(stk_chk["balance"]);
                                dr4["qty"] = stk_chk["balance"].ToString().Trim();
                                db3 += fgen.make_double(dr4["qty"].ToString().Trim());
                                db1 += fgen.make_double(dr4["qty"].ToString().Trim()) * fgen.make_double(dr4["rate"].ToString().Trim()); //for closing val
                                mrs.Rows.Add(dr4);
                            }
                        }
                        #endregion
                    }
                    #endregion end of foreach loop
                }
            }
            #region fill data into final cursor
            if (db3 == db2 && db2 != 0)
            {
                db12 = 0; db13 = 0;
                dr3 = ph_tbl.NewRow();
                dr3["fromdt"] = fromdt;
                dr3["todt"] = todt;
                dr3["header"] = header_n;
                dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                dr3["mcode"] = mq1;
                dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                dr3["cpartno"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "cpartno");
                dr3["maker"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "maker");
                dr3["opening"] = fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "opening");
                dr3["op_value"] = fgen.make_double(dr3["opening"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                dr3["cons_qty"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_qty");
                dr3["cons_val"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_val");
                // dr3["clos_val"] = db1;
                dr3["clos_qty"] = db2;
                dr3["clos_val"] = db2 * fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                // dr3["avg_rate"] = db1 / db2; //old
                dr3["avg_rate"] = fgen.make_double(dr3["clos_val"].ToString().Trim()) / db2;
                #region
                dt6 = new DataTable();
                //view isley lagaya qki same icode pe more thAN 1 ROW THI USKO 1 ROW BANANE KE LIYE
                db4 = 0; db5 = 0;
                if (dt3.Rows.Count > 0)
                {
                    DataView view2 = new DataView(dt3, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    dt6 = view2.ToTable();
                    for (int x = 0; x < dt6.Rows.Count; x++)
                    {
                        db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                        db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                    }
                }
                dr3["inw_Qty"] = db4;
                dr3["inw_val"] = db5;
                //  ph_tbl.Rows.Add(dr3);
                //////for outward qty and value
                db4 = 0; db5 = 0; dt6 = new DataTable();
                if (dt8.Rows.Count > 0)
                {
                    DataView view2 = new DataView(dt8, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    dt6 = view2.ToTable();
                    for (int x = 0; x < dt6.Rows.Count; x++)
                    {
                        db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                        db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                    }
                }
                dr3["out_qty"] = db4;
                dr3["out_val"] = db5;

                #endregion
                ph_tbl.Rows.Add(dr3);
            }
            #region
            else
            {
                if (db2 != 0)
                {
                    #region
                    dr3 = ph_tbl.NewRow();
                    dr3["fromdt"] = fromdt;
                    dr3["todt"] = todt;
                    dr3["header"] = header_n;
                    dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                    dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                    dr3["mcode"] = mq1;
                    dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                    dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                    dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                    dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                    dr3["cpartno"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "cpartno");
                    dr3["maker"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "maker");
                    dr3["opening"] = fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "opening");
                    dr3["op_value"] = fgen.make_double(dr3["opening"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                    dr3["cons_qty"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_qty");
                    dr3["cons_val"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_val");
                    dr3["clos_qty"] = db2;
                    // dr3["clos_val"] = db1;
                    dr3["clos_val"] = db2 * fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                    dr3["avg_rate"] = fgen.make_double(dr3["clos_val"].ToString().Trim()) / db2;
                    #region
                    dt6 = new DataTable();
                    //view isley lagaya qki same icode pe more thAN 1 ROW THI USKO 1 ROW BANANE KE LIYE
                    db4 = 0; db5 = 0;
                    if (dt3.Rows.Count > 0)
                    {
                        DataView view2 = new DataView(dt3, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt6 = view2.ToTable();
                        for (int x = 0; x < dt6.Rows.Count; x++)
                        {
                            db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                            db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                        }
                    }
                    dr3["inw_Qty"] = db4;
                    dr3["inw_val"] = db5;
                    //   ph_tbl.Rows.Add(dr3);
                    //////for outward qty and value
                    db10 = 0; db11 = 0; dt6 = new DataTable();
                    if (dt8.Rows.Count > 0)
                    {
                        DataView view2 = new DataView(dt8, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt6 = view2.ToTable();
                        for (int x = 0; x < dt6.Rows.Count; x++)
                        {
                            db10 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                            db11 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                        }
                    }
                    if (db10 != 0 && db11 != 0)
                    {
                        // dr3 = ph_tbl.NewRow();
                        dr3["out_qty"] = db10;
                        dr3["out_val"] = db11;
                    }
                    ph_tbl.Rows.Add(dr3);
                }
                #endregion
                #endregion
            }


            #endregion

            #endregion
            #endregion
            // }
            //////////for opening summsry and detail

            #region  opening blance loop
            itot_stk = 0; to_cons = 0; itv = 0; db = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0;

            // to_cons = Convert.ToDouble(fgen.seek_iname_dt(dt7, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "opening"));
            to_cons = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "opening"));
            db9 = to_cons;
            foreach (DataRow stk_chk in dt9.Rows) //ivoucher dt
            {
                #region for details of opening balance
                if (dt9.Rows.Count == 0)
                {
                    // fgen.execute_cmd(frm_qstr, frm_cocd, "insert into itemvbal13(irate,branchcd,type,vchnum,vchdate,invno,invdate,acode,icode,iqtyin)values(" + Convert.ToInt32(stk_chk["irate"]) + ",'" + frm_mbr + "','" + stk_chk["type"].ToString() + "','" + stk_chk["vchnum"].ToString() + "',to_datE('" + stk_chk["vchdate"].ToString() + "','dd/mm/yyyy'),'" + value1 + "',to_Date('" + stk_chk["invdate"].ToString() + "','dd/mm/yyyy'),'" + stk_chk["acode"].ToString().Trim() + "','" + stk_chk["icode"].ToString().Trim() + "'," + to_cons + ")");
                    mrrow = dtm.NewRow();
                    mrrow["icode"] = stk_chk["icode"].ToString().Trim();
                    mrrow["iname"] = fgen.seek_iname_dt(dt2, "icode='" + mrrow["icode"].ToString().Trim() + "'", "iname");
                    mrrow["mrr"] = stk_chk["vchnum"].ToString().Trim();
                    mrrow["mrrdt"] = stk_chk["vchdate"].ToString().Trim();
                    mrrow["qty"] = stk_chk["balance"].ToString().Trim();
                    mrrow["rate"] = stk_chk["irate"].ToString().Trim();
                    db7 += fgen.make_double(mrrow["qty"].ToString().Trim()) * fgen.make_double(mrrow["rate"].ToString().Trim()); //for closing val
                    db8 += fgen.make_double(mrrow["qty"].ToString().Trim());
                    mrrow["stock"] = db9;
                    dtm.Rows.Add(mrrow);
                    to_cons = 0;
                }
                else
                {
                    #region
                    //     if (stk_chk["icode"].ToString().Trim() == dropstk["icode"].ToString().Trim())
                    if (stk_chk["icode"].ToString().Trim() == dt2.Rows[k]["icode"].ToString().Trim())
                    {
                        if (Convert.ToDouble(stk_chk["balance"].ToString()) >= to_cons && to_cons > 0)
                        {
                            itot_stk = itot_stk + (to_cons * Convert.ToInt32(stk_chk["irate"]));
                            itv = itv + (to_cons * Convert.ToInt32(stk_chk["irate"]));
                            value1 = stk_chk["invno"].ToString();
                            if (value1.ToString().Trim().Length > 10) value1 = value1.Trim().Substring(0, 10);
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into itemvbal13(irate,branchcd,type,vchnum,vchdate,invno,invdate,acode,icode,iqtyin)values(" + Convert.ToInt32(stk_chk["irate"]) + ",'" + mbr + "','" + stk_chk["type"].ToString() + "','" + stk_chk["vchnum"].ToString() + "',to_datE('" + stk_chk["vchdate"].ToString() + "','dd/mm/yyyy'),'" + value1 + "',to_Date('" + stk_chk["invdate"].ToString() + "','dd/mm/yyyy'),'" + stk_chk["acode"].ToString().Trim() + "','" + stk_chk["icode"].ToString().Trim() + "'," + to_cons + ")");
                            mrrow = dtm.NewRow();
                            mrrow["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                            mrrow["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "iname");
                            mrrow["mrr"] = stk_chk["vchnum"].ToString().Trim();
                            mrrow["mrrdt"] = stk_chk["vchdate"].ToString().Trim();
                            mrrow["qty"] = to_cons;
                            mrrow["rate"] = stk_chk["irate"].ToString().Trim();
                            mrrow["stock"] = db9;// dropstk["opening"].ToString().Trim();
                            db7 += fgen.make_double(mrrow["qty"].ToString().Trim()) * fgen.make_double(mrrow["rate"].ToString().Trim()); //for closing val
                            db8 += fgen.make_double(mrrow["qty"].ToString().Trim());
                            dtm.Rows.Add(mrrow);
                            to_cons = 0;
                            // db1 = 0;
                        }
                        else
                        {
                            if (to_cons > 0)
                            {
                                value1 = stk_chk["invno"].ToString();
                                if (value1.ToString().Trim().Length > 10) value1 = value1.Trim().Substring(0, 10);
                                //  SQuery = "insert into itemvbal13(irate,branchcd,type,vchnum,vchdate,invno,invdate,acode,icode,iqtyin)values(" + Convert.ToInt32(stk_chk["irate"]) + ",'" + mbr + "','" + stk_chk["typE"].ToString() + "','" + stk_chk["vchnum"].ToString() + "',to_datE('" + stk_chk["vchdate"] + "','dd/mm/yyyy'),'" + value1 + "',to_Date('" + stk_chk["invdate"].ToString() + "','dd/mm/yyyy'),'" + stk_chk["acode"].ToString().Trim() + "','" + stk_chk["icode"].ToString().Trim() + "'," + Convert.ToInt32(stk_chk["balance"]) + ")";
                                mrrow = dtm.NewRow();
                                mrrow["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                                mrrow["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "iname");
                                mrrow["mrr"] = stk_chk["vchnum"].ToString().Trim();
                                mrrow["mrrdt"] = stk_chk["vchdate"].ToString().Trim();
                                mrrow["rate"] = stk_chk["irate"].ToString().Trim();
                                mrrow["stock"] = db9;// dropstk["opening"].ToString().Trim();
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                itot_stk = itot_stk + (Convert.ToInt32(stk_chk["balance"]) * Convert.ToInt32(stk_chk["irate"]));
                                itv = itv + (Convert.ToInt32(stk_chk["balance"]) * Convert.ToInt32(stk_chk["irate"]));
                                to_cons = to_cons - Convert.ToInt32(stk_chk["balance"]);
                                mrrow["qty"] = stk_chk["balance"].ToString().Trim();
                                db8 += fgen.make_double(mrrow["qty"].ToString().Trim()); //
                                db7 += fgen.make_double(mrrow["qty"].ToString().Trim()) * fgen.make_double(mrrow["rate"].ToString().Trim()); //for opening value
                                dtm.Rows.Add(mrrow);
                            }
                        }
                        #endregion end of foreach loop
                    }
                    #endregion
                }
            }
            #region merging op in clos wali dt me
            //yaha mne op bal ko ph_tbl me fill kiya h agar closing ki ek row hai or op ki 2 hai to uske liye if else ka use kiya h
            m = 0; n = 0;
            m = ph_tbl.Rows.Count;
            n = dtm.Rows.Count;
            if (db8 == db9 && db9 != 0) //if value and op stock is matched
            {
                #region
                // if (m >= n && k!=0)//this is for row in tBLE ..AGAR PHTBL ME jyada row h to usi me add krne ke liye
                //  {
                if (m > 0)
                {
                    if (ph_tbl.Rows[m - 1]["icode"].ToString().Trim() == dt2.Rows[k]["icode"].ToString().Trim())
                    {
                        if (k != 0)
                        {
                            #region
                            ph_tbl.Rows[m - 1]["opening"] = db9;
                            ph_tbl.Rows[m - 1]["op_value"] = db7;
                            db4 = 0; db5 = 0; dt6 = new DataTable();
                            if (dt3.Rows.Count > 0)
                            {
                                DataView view2 = new DataView(dt3, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt6 = view2.ToTable();
                                for (int x = 0; x < dt6.Rows.Count; x++)
                                {
                                    db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                                    db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                                }
                            }
                            ph_tbl.Rows[m - 1]["inw_Qty"] = db4;
                            ph_tbl.Rows[m - 1]["inw_val"] = db5;
                            //////for outward qty and value
                            db10 = 0; db11 = 0; dt6 = new DataTable();
                            if (dt8.Rows.Count > 0)
                            {
                                DataView view2 = new DataView(dt8, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt6 = view2.ToTable();
                                for (int x = 0; x < dt6.Rows.Count; x++)
                                {
                                    db10 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                                    db11 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                                }
                            }
                            ph_tbl.Rows[m - 1]["out_qty"] = db10;
                            ph_tbl.Rows[m - 1]["out_val"] = db11;
                            db12 = db9 + db4 - (db5 + db2); //op+inw qty-(inw val
                            db13 = db7 + db5 - (db10 + db1);
                            ph_tbl.Rows[m - 1]["cons_qty"] = db12;
                            ph_tbl.Rows[m - 1]["cons_val"] = db13;
                            #endregion
                        }
                        if (k == 0)
                        {
                            #region
                            ph_tbl.Rows[m - 1]["opening"] = db9;
                            ph_tbl.Rows[m - 1]["op_value"] = db7;
                            db4 = 0; db5 = 0; dt6 = new DataTable();
                            if (dt3.Rows.Count > 0)
                            {
                                DataView view2 = new DataView(dt3, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt6 = view2.ToTable();
                                for (int x = 0; x < dt6.Rows.Count; x++)
                                {
                                    db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                                    db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                                }
                            }
                            ph_tbl.Rows[m - 1]["inw_Qty"] = db4;
                            ph_tbl.Rows[m - 1]["inw_val"] = db5;
                            //////for outward qty and value
                            db10 = 0; db11 = 0; dt6 = new DataTable();
                            if (dt8.Rows.Count > 0)
                            {
                                DataView view2 = new DataView(dt8, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt6 = view2.ToTable();
                                for (int x = 0; x < dt6.Rows.Count; x++)
                                {
                                    db10 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                                    db11 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                                }
                            }
                            ph_tbl.Rows[m - 1]["out_qty"] = db10;
                            ph_tbl.Rows[m - 1]["out_val"] = db11;
                            db12 = db9 + db4 - (db5 + db2); //op+inw qty-(inw val
                            db13 = db7 + db5 - (db10 + db1);
                            ph_tbl.Rows[m - 1]["cons_qty"] = db12;
                            ph_tbl.Rows[m - 1]["cons_val"] = db13;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        dr3 = ph_tbl.NewRow();
                        dr3["opening"] = db9;
                        // dr3["op_value"] = db7;
                        dr3["fromdt"] = fromdt;
                        dr3["todt"] = todt;
                        dr3["header"] = header_n;
                        dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                        dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                        dr3["mcode"] = mq1;
                        dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                        dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                        dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                        dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                        dr3["cpartno"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "cpartno");
                        dr3["maker"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "maker");
                        dr3["op_value"] = fgen.make_double(dr3["opening"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                        dr3["cons_qty"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_qty");
                        dr3["cons_val"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_val");
                        dr3["clos_qty"] = fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "closing");
                        dr3["clos_val"] = fgen.make_double(dr3["clos_qty"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                        dr3["avg_rate"] = fgen.make_double(dr3["clos_val"].ToString().Trim()) / fgen.make_double(dr3["clos_qty"].ToString().Trim());
                        //view isley lagaya qki same icode pe more thAN 1 ROW THI USKO 1 ROW BANANE KE LIYE
                        #region
                        dt6 = new DataTable();
                        db4 = 0; db5 = 0;
                        if (dt3.Rows.Count > 0)
                        {
                            DataView view2 = new DataView(dt3, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt6 = view2.ToTable();
                            for (int x = 0; x < dt6.Rows.Count; x++)
                            {
                                db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                                db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                            }
                        }
                        dr3["inw_Qty"] = db4;
                        dr3["inw_val"] = db5;
                        //////for outward qty and value
                        db10 = 0; db11 = 0; dt6 = new DataTable();
                        if (dt8.Rows.Count > 0)
                        {
                            DataView view2 = new DataView(dt8, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt6 = view2.ToTable();
                            for (int x = 0; x < dt6.Rows.Count; x++)
                            {
                                db10 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                                db11 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                            }
                        }
                        dr3["out_qty"] = db10;
                        dr3["out_val"] = db11;
                        db12 = db9 + db4 - (db5 + db2); //op+inw qty-(inw val
                        db13 = db7 + db5 - (db10 + db1);
                        dr3["cons_qty"] = db12;
                        dr3["cons_val"] = db13;
                        #endregion
                        ph_tbl.Rows.Add(dr3);
                        #endregion
                    }
                }

                else
                {
                    #region
                    dr3 = ph_tbl.NewRow();
                    dr3["opening"] = db9;
                    //  dr3["op_value"] = db7;
                    dr3["fromdt"] = fromdt;
                    dr3["todt"] = todt;
                    dr3["header"] = header_n;
                    dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                    dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                    dr3["mcode"] = mq1;
                    dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                    dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                    dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                    dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                    dr3["cpartno"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "cpartno");
                    dr3["maker"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "maker");
                    dr3["op_value"] = fgen.make_double(dr3["opening"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                    dr3["cons_qty"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_qty");
                    dr3["cons_val"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_val");
                    dr3["clos_qty"] = fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "closing");
                    dr3["clos_val"] = fgen.make_double(dr3["clos_qty"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                    dr3["avg_rate"] = fgen.make_double(dr3["clos_val"].ToString().Trim()) / fgen.make_double(dr3["clos_qty"].ToString().Trim());
                    //view isley lagaya qki same icode pe more thAN 1 ROW THI USKO 1 ROW BANANE KE LIYE
                    #region
                    dt6 = new DataTable();
                    db4 = 0; db5 = 0;
                    if (dt3.Rows.Count > 0)
                    {
                        DataView view2 = new DataView(dt3, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt6 = view2.ToTable();
                        for (int x = 0; x < dt6.Rows.Count; x++)
                        {
                            db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                            db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                        }
                    }
                    dr3["inw_Qty"] = db4;
                    dr3["inw_val"] = db5;
                    //////for outward qty and value
                    db10 = 0; db11 = 0; dt6 = new DataTable();
                    if (dt8.Rows.Count > 0)
                    {
                        DataView view2 = new DataView(dt8, "ICODE='" + dr3["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt6 = view2.ToTable();
                        for (int x = 0; x < dt6.Rows.Count; x++)
                        {
                            db10 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                            db11 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                        }
                    }
                    dr3["out_qty"] = db10;
                    dr3["out_val"] = db11;
                    db12 = db9 + db4 - (db5 + db2); //op+inw qty-(inw val
                    db13 = db7 + db5 - (db10 + db1);
                    dr3["cons_qty"] = db12;
                    dr3["cons_val"] = db13;
                    #endregion
                    ph_tbl.Rows.Add(dr3);
                    #endregion
                }
            }

            #endregion

            if (db2 == 0 && db9 == 0) //agar clos aND OPENING DONO 0 HAI TO ..THIS IS ONLY FOR CHK INWARD AND OUTWARD
            {
                #region
                db4 = 0; db5 = 0; dt6 = new DataTable();
                if (dt3.Rows.Count > 0)
                {
                    DataView view2 = new DataView(dt3, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    dt6 = view2.ToTable();
                    for (int x = 0; x < dt6.Rows.Count; x++)
                    {
                        db4 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                        db5 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                    }
                }
                //////for outward qty and value
                db10 = 0; db11 = 0; dt6 = new DataTable();
                if (dt8.Rows.Count > 0)
                {
                    DataView view2 = new DataView(dt8, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    dt6 = view2.ToTable();
                    for (int x = 0; x < dt6.Rows.Count; x++)
                    {
                        db10 += fgen.make_double(dt6.Rows[x]["qty"].ToString().Trim());
                        db11 += fgen.make_double(dt6.Rows[x]["val"].ToString().Trim());
                    }
                }
                if (db4 != 0 || db5 != 0 || db10 != 0 || db11 != 0)
                {
                    //  d = ph_tbl.Rows.Count;
                    dr3 = ph_tbl.NewRow();
                    //   ph_tbl.Rows.InsertAt(dr3, d + 1);        
                    dr3["fromdt"] = fromdt;
                    dr3["todt"] = todt;
                    dr3["header"] = header_n;
                    dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                    dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                    dr3["mcode"] = mq1;
                    dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                    dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                    dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                    dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                    dr3["cpartno"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "cpartno");
                    dr3["maker"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "maker");
                    dr3["opening"] = fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "opening");
                    dr3["op_value"] = fgen.make_double(dr3["opening"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                    dr3["cons_qty"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_qty");
                    dr3["cons_val"] = fgen.seek_iname_dt(cons_Dt, "icode='" + dr3["icode"].ToString().Trim() + "'", "cons_val");
                    dr3["clos_qty"] = db2;
                    //  dr3["clos_val"] = db2;                   
                    dr3["clos_val"] = db2 * fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "'", "irate"));
                    if (db2 > 0)
                        dr3["avg_rate"] = fgen.make_double(dr3["clos_val"].ToString().Trim()) / db2;
                    dr3["inw_Qty"] = db4;
                    dr3["inw_val"] = db5;
                    dr3["out_qty"] = db10;
                    dr3["out_val"] = db11;
                    db12 = db9 + db4 - (db5 + db2); //op+inw qty-(inw val
                    db13 = db7 + db5 - (db10 + db1);
                    // dr3["cons_qty"] = db12;
                    dr3["cons_val"] = db13;
                    ph_tbl.Rows.Add(dr3);
                }
                #endregion
            }
        }
        #endregion end of op loop

        return ph_tbl;
    }
}