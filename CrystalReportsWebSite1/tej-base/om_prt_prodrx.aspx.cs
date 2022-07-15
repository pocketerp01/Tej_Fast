using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_prodrx : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;

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
                case "F38501":
                    //   SQuery = "SELECT 'TB' AS FSTR,'Printing' as Choice,'TB' as type from dual union all SELECT 'GB' AS FSTR,'Pigment' as Choice,'GB' as type from dual  union all  SELECT 'MB' AS FSTR,'Mixing-70' as Choice,'MB' as type from dual union all  SELECT 'MB' AS FSTR,'Mixing-71' as Choice,'MB' as type from dual union all  SELECT 'MB' AS FSTR,'Mixing-72' as Choice,'MB' as type from dual union all  SELECT 'MB' AS FSTR,'Mixing-73' as Choice,'MB' as type from dual"; //NEW FOR MOB APP                 
                    SQuery = "SELECT 'TB' AS FSTR,'Printing' as Choice,'TB' as type from dual union all SELECT 'GR' AS FSTR,'Pigment' as Choice,'GR' as type from dual  union all  SELECT 'MR' AS FSTR,'Mixing' as Choice,'MR' as type from dual union all  SELECT 'MB' AS FSTR,'Mixing Return' as Choice,'MB' as type from dual union all  SELECT 'MJ' AS FSTR,'Vessel Tfr Sticker' as Choice,'MJ' as type from dual"; //OLD                  

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
            value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            hfcode.Value = "";
            if (val == "")
            {
                // bydefault it will ask for prdRange popup

                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F38501":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            mq1 = value1;

                            //SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,a.refnum as ticket_no,to_char(a.refdate,'dd/mm/yyyy') as ticket_date from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + mbr + "' and type='" + mq1 + "' and a.vchdate " + xprdrange + " and trim(a.iopr)='MC' ORDER BY A.VCHNUM DESC ";//old .....FOR MOB APP

                            if (mq1 == "MR")
                            {
                                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,a.refnum as ticket_no,to_char(a.refdate,'dd/mm/yyyy') as ticket_dt from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + mbr + "' and type='" + mq1 + "' and a.vchdate " + xprdrange + " and trim(a.iopr)='MC' ORDER BY A.VCHNUM DESC ";//old .....FOR MOB APP
                            }
                            if (mq1 == "MB" || mq1 == "MJ")
                            {
                                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,a.refnum as ticket_no,to_char(a.refdate,'dd/mm/yyyy') as ticket_dt from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + mbr + "' and type='" + mq1 + "' and a.vchdate " + xprdrange + " ORDER BY A.VCHNUM DESC ";//old .....FOR MOB APP
                            }
                            if (mq1 == "GR")
                            {
                                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,a.refnum as ticket_no,to_char(a.refdate,'dd/mm/yyyy') as ticket_dt from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + mbr + "' and type='" + mq1 + "' and a.vchdate " + xprdrange + " and trim(a.iopr)='GC' ORDER BY A.VCHNUM DESC ";//old .....FOR MOB APP
                            }
                            if (mq1 == "TB")
                            {
                                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,a.refnum as ticket_no,to_char(a.refdate,'dd/mm/yyyy') as ticket_dt from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + mbr + "' and type='" + mq1 + "' and a.vchdate " + xprdrange + " and trim(a.iopr)='TC'  ORDER BY A.VCHNUM ";//old 
                            }
                            header_n = "-";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfval.Value);//type
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", hf1.Value);//type
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_prodrx_reps(frm_qstr);
                        }
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
                case "":
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                // if we want to ask another popup's
                // Month Popup Instead of Date Range *************
                case "  ":
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
                case "F38501":
                    mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as qr ,a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + mbr + "' and type='" + mq1 + "' and a.vchdate " + xprdrange + " ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    header_n = "Select Entry";
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "":
                    // open drill down form
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

            }
        }
    }
}