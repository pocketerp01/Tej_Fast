using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_reels : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, cond1, cond2, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, mq11, mq12, yr_fld, cDT1, cDT2, frm_myear, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joincond;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11, dt12, mdt, dticode, dticode2, ph_tbl;
    double month, to_cons, itot_stk, itv, db, db1, db2, db3, db4, db5, db6, db7; DataRow dr1, oporow, ROWICODE, ROWICODE2; DataView dv, dv1, view1, view2;
    string opbalyr, param, eff_Dt, xprdrange1, xprdRange1, cldt = "", frm_cDt1, frm_cDt2;
    string er1, er2, er3, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    string frm_AssiID;
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
                co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                cDT1 = frm_cDt1;
                cDT2 = frm_cDt2;
                fromdt = frm_cDt1;
                todt = frm_cDt2;
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
                case "F25398":
                    SQuery = "Select distinct a.kclreelno as fstr, B.Iname as Name,C.Aname,A.Kclreelno as Our_Reelno,A.coreelno as Co_Reel_no,a.reelwin as Wt_Rcv,a.vchnum,a.vchdate,a.icode from reelvch a, item b , famst c where trim(A.icode)=trim(b.icode) and trim(A.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.type like '0%' order by b.Iname,a.kclreelno";
                    reelViewsCreation();
                    SQuery = "select my_reel as fstr, Iname as Item_Name,my_reel as Our_reelno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel,bfactor ,psize,gsm,Icode,insp_done,origwt from reel_dstk_" + mbr + " where 1=1  order by igrp,psize,gsm";
                    if (co_cd == "MASS")
                    {
                        SQuery = "select my_reel as fstr, Iname as Item_Name,my_reel as Our_Batchno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel as Co_Batch,bfactor,psize,gsm,Icode,insp_done,origwt from reel_dstk_" + mbr + " where 1=1  order by igrp,psize,gsm";
                    }
                    fgen.send_cookie("XID", "FINSYS_S");
                    fgen.send_cookie("SRCHSQL", SQuery);
                    if (co_cd == "MASS")
                        fgen.Fn_open_sseek("Select Batch No", frm_qstr);
                    else
                        fgen.Fn_open_sseek("Select Reel No", frm_qstr);
                    break;
                case "F25399":
                case "F25383":
                case "F25387":
                case "F25385":
                case "F25243":
                case "F25381":
                case "F25243V":
                    // Reel Wise Issue Report
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F25397":
                case "F25396":
                case "F25395":
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '0%' order by type1";
                    header_n = "Select Matl. Inward Type";
                    break;
                case "F25198C":
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where id='M' and type1 like '1%' order by type1";
                    header_n = "Select Matl. Return Type";
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
            reelViewsCreation();
            switch (val)
            {
                case "F25381":
                    if (hf2.Value == "")
                    {
                        //fgen.msg("-", "FRMSG", "1 for Tie Up '13'2 for Reel Wise '13'3 for Summary '13'4 for Summary All");
                        fgen.msg("-", "PMSG", "1. Stock Tie Up Report (All items with closing stock)'13'2. Stock Tie Up Report (All items with and without closing stock)'13'3. for bit Reel");
                        hf2.Value = "STOCK";
                    }
                    else
                    {
                        mq5 = Request.Cookies["REPLY"].Value;
                        if (mq5 == "Y")
                        {
                            cond = "";
                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") != "0")
                            {
                                cond = " AND SUBSTR(ICODE,1,2) IN (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + ")";
                            }
                            SQuery = "select Iname as Item_Name,my_reel as Our_reelno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel,bfactor ,psize,gsm,Icode,insp_done,rlocn from reel_dstk_" + mbr + " where closing>0 " + cond + " order by igrp,psize,gsm";
                            if (co_cd == "MASS")
                            {
                                SQuery = "select Iname as Item_Name,my_reel as Our_Batchno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel as Co_Batch,bfactor ,psize,gsm,Icode,insp_done,rlocn from reel_dstk_" + mbr + " where closing>0 " + cond + " order by igrp,psize,gsm";
                            }
                            //SQuery = "select C.Aname as Vendor,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,acode from rgpmst where branchcd='" + mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Stock Tie Report With Stock ", frm_qstr);
                            hf2.Value = "";
                        }
                        else if (mq5 == "N")
                        {
                            SQuery = "select Iname as Item_Name,my_reel as Our_reelno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel,bfactor ,psize,gsm,Icode,insp_done,rlocn from reel_dstk_" + mbr + " where 1=1  order by igrp,psize,gsm";
                            //SQuery = "select C.Aname as Vendor,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,acode from rgpmst where branchcd='" + mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                            if (co_cd == "MASS")
                            {
                                SQuery = "select Iname as Item_Name,my_reel as Our_Batchno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel as Co_Batch,bfactor ,psize,gsm,Icode,insp_done,rlocn from reel_dstk_" + mbr + " where 1=1  order by igrp,psize,gsm";
                            }
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Stock Tie Report All Stock ", frm_qstr);
                            hf2.Value = "";
                        }
                        else
                        {
                            SQuery = "select Iname as Item_Name,my_reel as Our_reelno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel,bfactor ,psize,gsm,Icode,insp_done,origwt from reel_dstk_" + mbr + " where closing>0 and outw>0 and 1=1  order by igrp,psize,gsm";
                            //SQuery = "select C.Aname as Vendor,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,acode from rgpmst where branchcd='" + mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Stock Tie Report Bit Reel ", frm_qstr);
                            hf2.Value = "";
                        }
                    }

                    break;
                case "F25398":
                    SQuery = "select * from (Select B.Iname,C.Aname,A.Kclreelno as Our_Reelno,'Purch' as Status,A.coreelno as Co_Reel_no,a.reelwin as Wt_Rcv,0 as Wt_out,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.job_no,a.job_Dt,a.type,a.icode from reelvch a, item b , famst c where trim(A.icode)=trim(b.icode) and trim(A.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.kclreelno='" + col1 + "' union all Select B.Iname,'-' as Aname,A.Kclreelno as Our_Reelno,'Issue' as Status,A.coreelno as Co_Reel_no,0 as Wt_Rcv,a.reelwout as Wt_out,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.job_no,a.job_Dt,a.type,a.icode from reelvch a, item b  where trim(A.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and ((a.type like '3%') or (a.type like '7%' and a.reelwout>0)) and a.kclreelno='" + col1 + "' union all Select B.Iname,'-' as Aname,A.Kclreelno as Our_Reelno,'Return' as Status,A.coreelno as Co_Reel_no,a.reelwin as Wt_Rcv,0 as Wt_out,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.job_no,a.job_Dt,a.type,a.icode from reelvch a, item b  where trim(A.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and ((a.type like '1%') or (a.type like '7%' and a.reelwin>0)) and a.kclreelno='" + col1 + "') order by Vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Reel Tracking Report", frm_qstr);
                    break;
                case "F25397":
                case "F25396":
                case "F25395":
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as MRR_no,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Vendor,a.Invno,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,A.refnum as Chl_no,to_char(a.refdate,'dd/mm/yyyy') as chl_Dt,a.Genum as GE_No,to_char(a.gedate,'dd/mm/yyyy') as GE_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.pname as insp_by,a.finvno as vch_ref,to_Char(a.vchdate,'yyyymmdd') as vdd from IVOUCHER a,famst b where  a.branchcd='" + mbr + "' and a.type='" + value1 + "' AND a." + "vchdate" + " " + xprdrange + " and  trim(a.acode)=trim(B.acodE) and a.store!='R' order by vdd desc,a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                    break;
                case "F25198C":
                    if (col1 == "") return;
                    header_n = "Return Reel / Lot / Batch Sticker";
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from IVOUCHER a where a.branchcd='" + mbr + "' and a.type='" + value1 + "' AND a." + "vchdate" + " " + xprdrange + " and a.store!='R' order by vdd desc,a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                    fgen.Fn_open_mseek(header_n, frm_qstr);
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
                case "F25383":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT2");
                    if (party_cd.Length <= 1)
                    {
                        mq2 = " and nvl(a.acode,'-') like '%'";
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
                    if (Request.Cookies["REPLY"].Value == "Y")
                    {
                        SQuery = "select to_char(a.vchdate,'dd/mm/yyyy') as Vch_Dt,a.vchnum as Vch_no,b.aname as Supplier,c.iname as Item,a.reelwin as Qty_Inwd,a.kclreelno as Reel_No,a.coreelno as Co_Reel,a.Type,a.Job_no,a.Job_dt,a.icode,c.cpartno,c.oprate1 as psize,c.oprate3 as gsm,a.reel_Rejqty from reelvch a,famst b , item c where a.branchcd='" + mbr + "' and a.type in('02','07','09','08','0U') and a.vchdate  " + xprdrange + "  and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.posted='Y' " + mq2 + " " + mq3 + " order by A.vchdate,a.vchnum";
                    }
                    else
                    {
                        SQuery = "select a.vchdate as Vch_Dt,a.vchnum as Vch_no,b.aname as Supplier,c.iname as Item,a.reelwin as Qty_Inwd,a.kclreelno as Reel_No,a.coreelno as Co_Reel,a.Type,a.Job_no,a.Job_dt,a.icode,c.cpartno,c.oprate1 as psize,c.oprate3 as gsm,a.reel_Rejqty from reelvch a,famst b , item c where a.branchcd='" + mbr + "' and a.type in('02','07','09','08','0U') and a.vchdate  " + xprdrange + " and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.posted='Y' " + mq2 + " " + mq3 + " and a.reel_Rejqty>0 order by A.vchdate,a.vchnum";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Reel Wise Receipt Report  for the Period " + fromdt + " to " + todt, frm_qstr);

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
                case "F25381":
                    if (hf1.Value == "")
                    {
                        SQuery = "select type1 as fstr,name,type1 as code from type where Id='Y' and /*substr(type1,1,2) in ('07','08','70','80','81')*/ substr(type1,1,1)<'9' order by type1";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Type", frm_qstr);
                    }
                    else
                    {

                        if (hf2.Value == "")
                        {
                            mq7 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                            mq6 = Request.Cookies["REPLY"].Value;
                            if (mq6 == "Y")
                            {
                                SQuery = "SELECT * FROM (select b.Iname,sum(a.op)as Stk_op,sum(a.inwd) as Stk_in,sum(a.outw) as stk_out,sum(a.closing) as Stk_clo,sum(a.rop) as Reel_op,sum(a.rinwd) as Reel_inw,sum(a.routwd) as Reel_outw,sum(a.rclos) as Reel_clo,sum(a.closing)-sum(a.Rclos) as Stk_Diff,sum(A.reels) as Reels,sum(a.inqa) as UQaorRej,b.oprate1 as PSIZE,b.oprate3 as GSM,b.cpartno,trim(a.icode) as Icode,MAX(A.ACODE) AS ACODE,(case when sum(A.reels)>0 then round(sum(a.rclos)/sum(A.reels),0) else 0 end) as avg_wt from (select icode,op,inwd,outw,closing,0 as rop,0 as rinwd,0 as routwd,0 as rclos,NULL AS ACODE,0 as reels,inqa from pap_stk_" + mbr + " union all select icode,0 as rop,0 as rinwd,0 as routwd,0 as rclos,op,inwd,outw,closing,ACODE,0 as reels,0 as inqa from reel_Stk_" + mbr + " union all select icode,0 as rop,0 as rinwd,0 as routwd,0 as rclos,0 as op,0 as inwd,0 as outw,0 as closing,null as acode,reels,0 as inqa from reel_nos_" + mbr + " )a,item b where trim(A.icode)=trim(B.icode) group by b.cpartno,b.iname,b.oprate1,b.oprate3,trim(a.icode) )M WHERE  10=10  ORDER BY SUBSTR(M.ICODE,1,4),M.INAME";
                                //SQuery = "select C.Aname as Vendor,b.Iname as Item_Name,b.Cpartno as Part_no,(a.Qtyord) as RGP_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,b.Cdrgno,trim(a.ERP_code) as ERP_code,trim(a.Fstr) as CHL_link,a.irate as Prate,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,trim(acode) as Acode  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,acode from rgpmst where branchcd='" + mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode from ivoucher where branchcd='" + mbr + "' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  and trim(Acode) like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' )  group by fstr,trim(acode),ERP_code )a,item b,famst c where trim(a.erp_code)=trim(B.icode) and trim(a.acode)=trim(c.acode)  order by B.Iname,trim(a.fstr)";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("Stock Tie Report for the Period " + fromdt + " to " + todt, frm_qstr);
                            }

                            else if (mq6 == "N")
                            {

                                fgen.msg("-", "PMSG", "1 for With Stock Select '13'2 for All Stock Select '13'3 for Bit Reel");
                                hf2.Value = "STOCK1";
                            }
                        }
                    }
                    break;
                case "F25383":
                    fgen.msg("-", "CMSG", "1 for All Reels '13'2 for Rejn Reels");
                    break;
                case "F25399":
                case "F25387":
                case "F25385":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (party_cd.Length <= 1)
                    {
                        mq2 = " and nvl(a.acode,'-') like '%'";
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
                    if (val == "F25385")
                    {
                        SQuery = "select to_char(a.Vch_Dt,'dd/mm/yyyy') as vch_dt,a.Vch_No,b.aname as Supplier,a.Item,a.Qty_Out,a.reel_No,a.Co_Reel,a.Job_no,a.Job_dt,a.Reel_Size,a.Gsm,a.icode,a.cpartno,a.acode from (select a.vchdate as Vch_Dt,a.vchnum as Vch_No,c.iname as Item,a.reelwout as Qty_Out,a.kclreelno as reel_No,a.coreelno as Co_Reel,a.Job_no,a.Job_dt,c.oprate1 as Reel_Size,c.oprate3 as Gsm,a.icode,c.cpartno,(Case when trim(nvl(a.acode,'-'))='-' then '02N001' else acode end) as acode from reelvch a,item c where a.branchcd='" + mbr + "' and a.type in('31','32') and a.vchdate  " + xprdrange + "  and trim(a.icode)=trim(c.icode) " + mq2 + " " + mq3 + "  order by A.vchdate,a.vchnum) a left outer join famst b on trim(A.acode)=trim(B.acode) order by a.Vch_Dt,a.Vch_No";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Reel Wise Issue Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    }
                    else if (val == "F25387")
                    {
                        SQuery = "select to_char(a.vchdate,'dd/mm/yyyy') as Vch_Dt,a.vchnum as Vch_No,a.branchcd,c.iname as Item,a.reelwin as Qty_Return,a.kclreelno as Reel_No,a.coreelno as Co_Reel,a.irate,a.Job_no,a.Job_dt,c.oprate1 as Reel_Size,c.oprate3 as Gsm,a.icode,c.cpartno,A.RINSP_BY from reelvch a , item c where a.branchcd='" + mbr + "' and a.type in('11') and a.vchdate  " + xprdrange + "  and trim(a.icode)=trim(c.icode)  " + mq2 + "  " + mq3 + "  order by A.vchdate,a.vchnum";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Reel Wise Return Report  for the Period " + fromdt + " to " + todt, frm_qstr);
                    }
                    else if (val == "F25399")
                    {
                        SQuery = "select a.vchdate as Vch_Dt,a.vchnum as Vch_No,b.aname as Supplier,c.iname as Item,a.psize as Reel_Size,a.gsm as Gsm,a.reelwin as Qty_Inwd,a.reelwout as Qty_Out,a.kclreelno as Reel_No,a.Job_no,a.Job_dt,a.icode from reelvch a,famst b , item c where a.branchcd='" + mbr + "' and a.type in('02','07','31','32','11') and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.posted='Y' " + mq2 + "  " + mq3 + "  order by a.kclreelno,A.vchdate,a.type,a.vchnum";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Reel Ledger Report  for the Period " + fromdt + " to " + todt, frm_qstr);
                    }
                    break;
                case "F25243":
                case "F25243V":
                    cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT2");
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (party_cd.Length <= 1)
                    {
                        mq2 = " ";
                    }
                    else
                    {
                        mq2 = " and substr(d.icode,1,2)='" + party_cd + "'";
                    }

                    if (part_cd.Length <= 1)
                    {
                        mq3 = " ";
                    }
                    else
                    {
                        mq3 = " and substr(d.icode,1,4)='" + part_cd + "'";
                    }
                    string icodecond = "" + mq2 + " " + mq3 + " ";
                    if (frm_formID == "F25243V")
                    {
                        SQuery = "SELECT * FROM (select b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill,max(a.irate) as irate from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill,0 as irate from reelvch where branchcd='" + mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill,max(irate) as irate from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill,max(irate) as irate from reelvch where branchcd='" + mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill,max(irate) as irate from reelvch where branchcd='" + mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill,max(irate) as irate from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%' ";
                        SQuery = "select e.aname as Party,d.Iname as Item_Name,d.my_reel as Lot_No,d.op as Opening_qty,(d.op*d.irate) as Opening_Value,d.pwd as Purchase_Qty,(d.pwd*d.irate) as Purchase_Value,d.outw  as Issue_Qty,(d.outw*d.irate) as Issue_Value,d.inwd as return_Qty,(d.inwd*d.irate) as return_value,d.closing as Closing_Qty,(d.closing*d.irate) as Closing_Value,d.co_Reel as comp_batch,d.Icode,d.insp_done,d.rlocn,d.irate as Rate,d.unit,d.no_proc as sec_unit from (" + SQuery + ") d left join famst e on trim(d.acode)=trim(e.acode) where 1=1 " + icodecond + "  order by d.icode,d.my_reel ";
                    }
                    else
                    {
                        SQuery = "SELECT * FROM (select b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from reelvch where branchcd='" + mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,max(ACODE) As ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%' ";
                        SQuery = "select e.aname as Party,d.Iname as Item_Name,d.my_reel as Lot_No,d.op as Opening_qty,d.pwd as Purchase_Qty,d.outw as Issue_Qty,d.inwd as return_Qty,d.closing as Closing_Qty,d.co_Reel as comp_batch,d.Icode,d.oprate1 as I_Width,d.oprate2 as I_Length,d.oprate3 as I_GSM,d.insp_done,d.rlocn,d.unit,d.no_proc as sec_unit from (" + SQuery + ") d left join famst e on trim(d.acode)=trim(e.acode) where 1=1 " + icodecond + "  order by d.icode,d.my_reel ";

                        // with total on top - mpac - 29/05/2021
                        SQuery = "SELECT * FROM (select b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from reelvch where branchcd='" + mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,max(ACODE) As ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%' ";
                        SQuery = "select 'Total' as Party,'Total' as Item_Name,'-' as Lot_No,sum(d.op) as Opening_qty,sum(d.pwd) as Purchase_Qty,sum(d.outw) as Issue_Qty,sum(d.inwd) as return_Qty,sum(d.closing) as Closing_Qty,'-' as comp_batch,'-' Icode,0 as I_Width,0 as I_Length,0 as I_GSM,'-' insp_done,'-' rlocn,'-' unit,'-' sec_unit from (" + SQuery + ") d union all select e.aname as Party,d.Iname as Item_Name,d.my_reel as Lot_No,d.op as Opening_qty,d.pwd as Purchase_Qty,d.outw as Issue_Qty,d.inwd as return_Qty,d.closing as Closing_Qty,d.co_Reel as comp_batch,d.Icode,d.oprate1 as I_Width,d.oprate2 as I_Length,d.oprate3 as I_GSM,d.insp_done,d.rlocn,d.unit,d.no_proc as sec_unit from (" + SQuery + ") d left join famst e on trim(d.acode)=trim(e.acode) where 1=1 " + icodecond + "";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("RM Lot Wise Report With Stock for " + fromdt + " to " + todt + " ", frm_qstr);
                    break;
                default:
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
            }
        }
    }

    void reelViewsCreation()
    {
        cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
        cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT2");
        xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
        if (fromdt == "0") fromdt = cDT1;
        if (todt == "0") todt = cDT2;
        if (xprdrange == "0") xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_DATERANGE");
        xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
        xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
        string xprd3 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
        string typstring = "'07','08','09'";
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") != "")
            typstring = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        string icodecond = "and substr(icode,1,2) in (" + typstring + ") ";
        string reel_V_tbl = "reelvch";
        string mq2 = "";
        string mq3 = "";

        mq0 = "select trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,sum(a.inqa) as inqa from (Select icode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,0 as inqa from ITEMBAL where branchcd='" + mbr + "' " + icodecond + " union all  ";
        mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' " + icodecond + " GROUP BY ICODE union all ";
        mq2 = "select icode,0 as op,0 as cdr,0 as ccr,0 as clos,sum(iqtyin) as inqa from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd3 + " and store in ('R','N') " + icodecond + " GROUP BY ICODE union all ";
        mq3 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as inqa from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and store='Y' " + icodecond + " GROUP BY ICODE )a group by trim(a.icode) having sum(opening)+sum(cdr)+sum(ccr)+sum(a.inqa)<>0 ";
        SQuery = "create or replace view PAP_STK_" + mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + mq3 + "))";
        fgen.execute_cmd(frm_qstr, co_cd, SQuery);

        //mq0 = "select trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE from (Select kclreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)='REELOP*' and 1=2 union all  ";
        //mq1 = "select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)!='REELOP*' GROUP BY kclreelno,ICODE having sum(reelwin)-sum(reelwout)!= 0 union all ";
        //mq2 = "select kclreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE )a group by trim(a.icode) having sum(opening)+sum(cdr)+sum(ccr)<>0 ";
        //SQuery = "create or replace view REEL_STK_" + mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + "))";

        mq0 = "select trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE from (Select kclreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)='REELOP*' and 1=2 union all  ";
        mq1 = "select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)!='REELOP*' GROUP BY kclreelno,ICODE having sum(reelwin)-sum(reelwout)!= 0 union all select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE from " + reel_V_tbl + "_op where branchcd='" + mbr + "' and type like '%' and posted='Y' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)!='REELOP*' GROUP BY kclreelno,ICODE having sum(reelwin)-sum(reelwout)!= 0 union all ";
        mq2 = "select kclreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE )a group by trim(a.icode) having sum(opening)+sum(cdr)+sum(ccr)<>0 ";
        SQuery = "create or replace view REEL_STK_" + mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + "))";
        fgen.execute_cmd(frm_qstr, co_cd, SQuery);

        mq0 = "select icode,count(reelno) as reels,sum(closing) as Closing from (select trim(a.kclreelno) as Reelno,trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE from (Select '-' as kclreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE from REELVCH_OP where branchcd='" + mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,7)='REELOP*' and 1=2 union all  ";
        mq1 = "select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE union all select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE from " + reel_V_tbl + "_op where branchcd='" + mbr + "' and type like '%' and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE union all ";
        mq2 = "select kclreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE  )a group by trim(a.kclreelno),trim(a.icode) having sum(opening)+sum(cdr)-sum(ccr)>0)group by Icode ";
        SQuery = "create or replace view REEL_NOS_" + mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + "))";
        fgen.execute_cmd(frm_qstr, co_cd, SQuery);

        mq0 = "select b.iname,b.cpartno,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from " + reel_V_tbl + " where branchcd='" + mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  ";
        mq1 = "select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from " + reel_V_tbl + "_op where branchcd='" + mbr + "' and type like '%' and posted='Y' " + icodecond + " GROUP BY type,kclreelno,coreelno,ICODE union all ";
        mq2 = "select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from " + reel_V_tbl + " where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY type,kclreelno,coreelno,ICODE  )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.cpartno,b.pur_uom,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) having sum(opening)+sum(cdr)+sum(ccr)<>0 ";
        SQuery = "create or replace view REEL_DSTK_" + mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + ")m where 1=1 and nvl(m.aCODE,'%') like '%' )";
        fgen.execute_cmd(frm_qstr, co_cd, SQuery);
    }
}