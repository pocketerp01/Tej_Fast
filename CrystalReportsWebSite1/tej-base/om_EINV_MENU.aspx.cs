using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

//  F25374
//EINV_MENU

public partial class om_EINV_MENU : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld;
    int i0, i1, i2, i3, i4, v = 0; DateTime date1, date2; DataSet ds, ds3, oDS;
    DataTable dt, ph_tbl, dt1, dt2, dt3, dt4, dt5, dtm, mdt, mdt1, vdt, dtPo, fmdt, dt_dist, dt_dist1, dticode, dticode2 = new DataTable();
    DataRow dro, dr1, dro1 = null;
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
        { hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidated report'13'(No for branch wise)"); }
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F10156":
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
                    case "RPT4":
                        hf2.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "ITEM_MASTER":
                        if (value1.Trim().Length <= 1)
                        {
                            mq1 = "A.icode like '%' ";
                        }
                        else
                        {
                            mq1 = "trim(substr(b.icode,1,2)) in (" + value1 + ")";
                        }
                        SQuery = "select trim(b.icode) as icode,trim(b.iname) as iname,trim(b.hscode) as hscode,trim(a.name) as HSN_Desc,a.num4 as SGST_Rate,a.num5 as CGST_Rate,a.num6 as IGST_Rate,trim(b.unit) as unit from typegrp a,item b where a.id='T1' and trim(a.acref)=trim(b.hscode) and length(trim(b.icode))=8 and " + mq1 + " order by trim(b.icode) desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Item Master- Details for Einvoice Management", frm_qstr);
                        break;

                    case "CANCEL_IRN_24HR":
                        hf2.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
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
            switch (val)
            {
                case "CANCEL_INV_FINS_LIST":
                    SQuery = "select trim(a.vchnum) as Inv_no,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,trim(b.aname) as Party_name,trim(a.acode) as party_code,a.type,trim(nvl(a.einv_no,'-')) as IRN from sale a, famst b where a.branchcd='" + mbr + "' and trim(a.naration)='CANCELLED INVOICE' and a.bill_qty=0 and amt_sale=0 and bill_tot=0 and STA_AMT =0 and AMT_EXTEXC=0 and amt_exc=0 and rvalue=0 and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) order by a.vchdate DESC,trim(a.vchnum) desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Cancelled Invoice(in Finsys through Block/Cancel Invoice option) List ", frm_qstr);
                    break;
               
                case "CANCEL_DRCR_FINS_LIST":
                    SQuery = "select distinct trim(a.vchnum) as Note_no,to_char(a.vchdate,'dd/mm/yyyy') as Note_Date,trim(b.aname) as Party_name,trim(a.acode) as party_code,a.type,trim(nvl(a.gstvchnum,'-')) as IRN from ivoucher a, famst b where a.branchcd='" + mbr + "' and a.type in ('58','59') and substr(trim(a.naration),1,10)='*CANCELLED' and a.iamount=0 and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) order by to_char(a.vchdate,'dd/mm/yyyy'),trim(a.vchnum) desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Cancelled Dr/Cr List", frm_qstr);
                    break;

                case "CANCEL_IRN_THRU_FINS_LIST":
                    SQuery = "select trim(a.doc_no) as Inv_no,to_char(a.doc_dt,'dd/mm/yyyy') as Inv_Date,a.doc_type,trim(a.vchnum) as IRN_doc,to_char(a.vchdate,'dd/mm/yyyy') as IRNDoc_Date ,trim(a.irn_no) as IRN,trim(b.aname) as Party_name,trim(a.acode) as party_code,trim(a.can_by) as Cancelled_by,trim(can_dt) as Canellation_dt from einv_rec a, famst b where a.branchcd='" + mbr + "' and substr(trim(a.doc_type),1,1) = '4' and a.vchdate " + xprdrange + " and trim(nvl(a.irn_no,'-')) <> '-' and trim(nvl(a.irn_stat,'-')) = 'C' and trim(a.acode)=trim(b.acode) order by a.vchdate DESC,trim(a.vchnum) desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Cancelled IRN List cancelled through Finsys Einvoice Management", frm_qstr);
                    break;

                case "CANCEL_IRN_24HR":
                    if (hf2.Value == "INV")
                    {
                        SQuery = "select trim(a.vchnum)as Inv_no,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.type,trim(a.acode) as party_code,trim(a.einv_no) as IRN,a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(b.aname) as Party_name from sale a, famst b where a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " and trim(nvl(a.einv_no,'-')) <> '-'  and trim(a.acode)=trim(b.acode) order by a.vchdate DESC,trim(a.vchnum) desc";
                    }
                    else
                    {
                        SQuery = "select distinct trim(a.vchnum) as Note_no,to_char(a.vchdate,'dd/mm/yyyy') as Note_dt,a.type,trim(a.acode) as party_code,trim(a.gstvchnum) as IRN,trim(b.aname) as Party_name,a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a, famst b where a.branchcd='" + mbr + "' and a.type in ('58','59') and a.vchdate " + xprdrange + " and trim(a.acode)=trim(b.acode) and trim(nvl(a.gstvchnum,'-')) != '-' order by to_char(a.vchdate,'yyyymmdd') DESC,trim(a.vchnum) desc";
                    }
                    break;
            }
        }
    }  
      
    protected void fetch_irn_dtl_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "FETCH_IRN_DETAILS";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "FETCH_IRN_DETAILS");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }

    protected void print_IRN_ServerClick(object sender, EventArgs e)
    {

    }
    protected void Cancel_irn_24hr_ServerClick(object sender, EventArgs e)
    {       
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "FETCH_IRN_DETAILS");
        //fgen.Fn_open_prddmp1("-", frm_qstr);
        SQuery = "select 'INV' as fstr,'INV' as TYPE,'Invoice' as doc from dual UNION ALL select 'DR' as fstr,'DR/CR' as TYPE,'DR/CR NOTE' as doc from dual";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_sseek("Select Code", frm_qstr);
        hfid.Value = "CANCEL_IRN_24HR";
    }
    protected void Cancel_irn_finsyslist_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "CANCEL_IRN_THRU_FINS_LIST";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "CANCEL_IRN_THRU_FINS_LIST");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void cancel_drcr_fins_list_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "CANCEL_DRCR_FINS_LIST";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "CANCEL_DRCR_FINS_LIST");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void Cancel_Inv_fins_list_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "CANCEL_INV_FINS_LIST";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "CANCEL_INV_FINS_LIST");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void incorect_hsn_rate_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select trim(a.acref) as hscode,trim(a.type1) as code,trim(a.name) as HSN_Desc,a.num4 as SGST_Rate,a.num5 as CGST_Rate,a.num6 as IGST_Rate from typegrp a,(select * from ( select acref,count(*) as cnt from typegrp where id='T1' group by acref) where cnt>1)b where a.id='T1' and trim(a.acref)=trim(b.acref) order by trim(a.acref)";
         fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
         fgen.Fn_open_rptlevel("Incorrect HS Codes- Details for Einvoice Management", frm_qstr);               
    }
    protected void unit_master_dtl_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select type1, trim(name) as name,trim(exc_tarrif) as GST_UQC_unit from type where id='U' order by trim(name)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Unit Master Details", frm_qstr);
    }
    protected void item_master_dtl_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select TYPE1 AS FSTR,NAME ,TYPE1 AS CODE from type where ID='Y' order by fstr";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_mseek("Select Code", frm_qstr);
        hfid.Value = "ITEM_MASTER";
    }
    protected void party_master_dtl_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select trim(a.aname) as aname,a.acode,trim(a.addr1) as addr1, trim(nvl(a.addr2,'-')) as addr2, trim(nvl(a.addr3,'-')) as addr3, trim(nvl(a.country,'-')) as addr4,trim(nvl(a.district,'-')) as district,trim(nvl(a.pincode,'-')) as pincode, trim(nvl(a.gst_no,'-')) as gstin,trim(nvl(a.telnum,'-')) as Phone, trim(nvl(a.email,'-')) as Email,(case when nvl(b.brdist_kms,0)= 0 then nvl(a.dist_kms,0) else nvl(b.brdist_kms,0) end) as distance,(case when a.gstoversea !='Y' and length(trim(a.gst_no))!=15 then 'B2C' else 'B2B' end) as Sale_type from famst a, famstbal b where b.branchcd='" + mbr + "' and trim(a.acode)=trim(b.acode)  and substr(trim(a.acode),1,2) in ('02','06','16') order by trim(a.aname)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Party Master Details for Einvoice Management", frm_qstr);
    }
    protected void port_master_dtl_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select TRIM(a.name) as Port_code,trim(a.acref) as name, trim(nvl(a.acref2,'-')) as Addr2, trim(nvl(a.acref3,'-')) as City, trim(nvl(a.acref4,'-')) as statecode,nvl(a.num6,0) as pincode from typegrp a where a.id='^M' order by trim(a.acref)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Consignee Master Details for Einvoice Management", frm_qstr);
    }
    protected void Consignee_master_dtl_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select trim(a.aname) as aname,a.acode,trim(a.addr1) as addr1, trim(nvl(a.addr2,'-')) as addr2, trim(nvl(a.addr3,'-')) as addr3, trim(nvl(a.addr4,'-')) as addr4,trim(nvl(a.pincode,'-')) as pincode, trim(nvl(a.gst_no,'-')) as gstin,trim(nvl(a.telnum,'-')) as Phone, trim(nvl(a.email,'-')) as Email,nvl(a.cs_distance,0) as distance from csmst a order by trim(a.aname)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Port Master Details for Einvoice Management", frm_qstr);
    }
}