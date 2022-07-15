using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

using System.Web.UI.WebControls.WebParts;

public partial class om_view_engg : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, sp_cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld;
    int i0, i1, i2, i3, i4, v = 0; DateTime date1, date2; DataSet ds, ds3, oDS;
    DataTable dt, dt1, dt2, dt3, mdt, mdt1, vdt, dtPo, fmdt, dt_dist, dt_dist1, dticode, dtdrsim, dtm, dtm1, dt4, dt5, dt10, ph_tbl, dticode2 = new DataTable();
    DataRow dro, dr1, DataRow, dro1 = null;
    double month, to_cons, itot_stk, itv, db6, db5, db4, db3, db, db1, db2, db7, db8, db9; DataRow oporow, ROWICODE, ROWICODE2; DataView dv, mvdview, vdview, vdview1, dist1_view, sort_view;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1 = "";
    string frm_AssiID;
    string frm_UserID;
    string rmcBranch = "";
    string r10 = "";
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
                case "F10156":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F10139B":
                    SQuery = "select b.iname,b.ent_by,sum(SO) AS SO_Made,sum(pp) As Specs_MAde,sum(Stg_map) AS StgMap_Made,sum(bom)AS JobOrder_Made,trim(a.icode) As ERP_Code from (Select distinct icode,1 as SO,0 as pp,0 as bom,0 as Stg_map from somas where (branchcd='" + mbr + "' or MFGINBR='" + mbr + "') and type like '4%' and trim(icat)<>'Y'  union all Select distinct icode,0 as SO,1 as pp,0 as bom,0 as Stg_map from inspmst where branchcd='" + mbr + "' and type like '70%'  union all Select distinct Icode,0 as SO,0 as pp,1 as bom,0 as Stg_map from costestimate where branchcd='" + mbr + "' and type like '30%'  union all Select distinct icode,0 as SO,0 as pp,0 as bom,1 as Stg_map from itwstage where branchcd='" + mbr + "' and type like '10%') a, item b where trim(A.icode)=trim(B.icode) group by b.iname,b.ent_by,trim(A.icode) having sum(SO)>0 order by b.Iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item SO,Specs,Bom,Stg Mapping Tracker (1=Done,0=Pending) ", frm_qstr);
                    break;

                case "F10222":
                    SQuery = "select trim(a.icode) as icode,i.iname,i.cpartno,i.unit,i.cdrgno from (select distinct trim(icode) as icode,1 as qty from item where substr(trim(icode),1,1) in('7','9') and length(trim(icode))>4 and length(trim(nvl(deac_by,'-'))) <2 union all select distinct  trim(icode) as icode,-1 as qty from itemosp where substr(trim(icode),1,1)>=7 ) a,item i where trim(a.icode)=trim(i.icode) group by a.icode,i.iname,i.cpartno,i.unit,i.cdrgno having sum(qty)=0 order by icode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Items with BOM ", frm_qstr);
                    break;
                case "F10223":
                    SQuery = "select trim(a.icode) as icode,i.iname,i.cpartno,i.unit,i.cdrgno from (select distinct trim(icode) as icode,1 as qty from item where substr(trim(icode),1,1) in ('7','9') and length(trim(icode))>4 and length(trim(nvl(deac_by,'-'))) <2 union all select distinct  trim(icode) as icode,-1 as qty from itemosp where substr(trim(icode),1,1)>=7) a,item i where trim(a.icode)=trim(i.icode) group by a.icode,i.iname,i.cpartno,i.unit,i.cdrgno  having sum(qty)>0 order by icode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of Items without BOM ", frm_qstr);
                    break;
                case "F10224":
                    // ORIGINAL
                    //SQuery = "select trim(a.ibcode) as icode,i.iname,i.cpartno,i.unit,i.cdrgno,count(distinct a.icode) as count from itemosp a ,item i where trim(a.ibcode)=trim(i.icode) and length(Trim(A.icode))>4 group by a.ibcode,i.iname,i.cpartno,i.unit,i.cdrgno order by icode";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Items in Multiple BOMs ", frm_qstr);

                    // ON 18 JULY 2018 BY MADHVI ADD ENT_BY,EDT_BY , REMOVE length(Trim(A.icode))>4 AND SHOW DRILL DOWN 
                    fgen.drillQuery(0, "select trim(a.ibcode) as fstr, '-' as gstr,trim(a.ibcode) as icode,i.iname,i.cpartno as drg_no,i.unit,count(distinct a.icode) as count,to_char(i.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(i.edt_dt,'dd/mm/yyyy') as edt_dt from itemosp a ,item i where trim(a.ibcode)=trim(i.icode) group by a.ibcode,i.iname,i.cpartno,i.unit,i.cdrgno,to_char(i.ent_dt,'dd/mm/yyyy'),to_char(i.edt_dt,'dd/mm/yyyy') order by icode", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.icode) as fstr,trim(a.ibcode) as gstr,i.iname,i.cpartno as drwg_no,i.unit,a.ibqty as qty_used,a.main_issue_no as lotsize,trim(a.icode) as icode,a.vchnum,a.ibcode,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,i.irate from itemosp a ,item i where trim(a.icode)=trim(i.icode) order by vchnum", frm_qstr);
                    fgen.Fn_DrillReport("Items in Multiple BOMs ", frm_qstr);
                    break;
                case "F10225":
                    SQuery = "select count(distinct a.icode) as count ,a.ibcode,i.iname,i.cpartno,i.unit  from itemosp a,item i  where trim(a.ibcode)=trim(i.icode) and trim(a.ibcode)=trim(a.icode) and length(Trim(A.icode))>4 group by ibcode,i.iname,i.cpartno,i.unit";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("BOM where Parent/Child Match ", frm_qstr);
                    break;
                case "F10226":
                    SQuery = "select a.icode,i.iname,i.cpartno,i.unit,i.cdrgno from (select distinct trim(icode) as icode,1 as qty from itemosp where substr(trim(icode),1,1)>='9' union all select distinct trim(icode) as icode ,-1 as qty from somas where type like '4%')a ,item i where trim(a.icode)=trim(i.icode) and length(Trim(A.icode))>4 group by a.icode,i.iname,i.cpartno,i.unit,i.cdrgno having sum(qty)>0 order by icode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("BOM Items without Sales Order ", frm_qstr);
                    break;
                case "F10228":
                    SQuery = "Select b.iname as Item_Name,b.cpartno as Drwg_No,c.ibcode as ERP_Code ,c.cnt as Times_Used,B.Ent_Dt,B.edt_dt from (Select a.ibcode,count(a.ibcode) as cnt from itemosp a where a.type='BM' and a.branchcd!='DD' group by a.ibcode) c left outer join item b on trim(c.ibcode)=trim(b.icode) and length(Trim(b.icode))>4 where length(trim(nvl(b.deac_by,'-'))) >1 order by b.iname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("BOMs with Deactivated Items", frm_qstr);
                    break;
                case "F10229":
                    SQuery = "select b.Name as Main_Grp_name,a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.unit,a.icode as ERP_code,substr(a.icode,1,4) as subgrp,a.deac_by,a.deac_Dt from item a,type b where b.id='Y' and substr(a.icode,1,2)=b.type1 and b.id='Y' and a.branchcd!='DD' and length(Trim(A.icode))>4 and length(Trim(nvl(a.deac_by,'-')))>1  order by substr(a.icode,1,4),A.Iname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Master List which are deactivated", frm_qstr);
                    break;
                case "F10230":
                    SQuery = "select b.Name as Main_Grp_name,a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.unit,a.icode as ERP_code,substr(a.icode,1,4) as subgrp,a.Ent_by,a.ent_Dt,a.Edt_by,a.edt_Dt from item a,type b where b.id='Y' and substr(a.icode,1,2)=b.type1 and b.id='Y' and a.branchcd!='DD' and length(Trim(A.icode))>4 and length(Trim(nvl(a.app_by,'-')))<=1  order by substr(a.icode,1,4),A.Iname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Master List which are Not Approved", frm_qstr);
                    break;
                case "F10233":
                    SQuery = "select icode as Item_code, iname as Item_name,Cpartno as Part_No, unit,icode as ERP_Code, imin as Min, imax as Max, iord  as ROL from item where length(icode)>4 and imin=0 or imax=0 or iord=0 order by icode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Items without Min/ Max/ Reorder Level", frm_qstr);
                    break;
                case "F10234":
                    SQuery = "select b.icode as I_code, B.iname as Item_name,B.Cpartno as Part_No,sum(a.imin)as Min_level,sum(a.imax)as Max_level,sum(a.iord)as ReOrd_level,b.lead_time,B.unit,A.icode as ERP_Code from (Select icode, YR_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where branchcd='" + mbr + "' ) a , item b where trim(A.icode)=trim(B.icode) and length(Trim(A.icode))>4 group by b.icode,B.iname,B.Cpartno,b.lead_time,B.unit,A.icode having sum(a.imin)>0 or sum(a.imax)>0 or sum(a.iord)>0  ORDER BY B.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Min/ Max/ ROL of Items", frm_qstr);
                    break;
                case "F10235":
                    SQuery = "select b.iname,b.cpartno,a.icode,a.cnt from (select trim(icode) as icode,count(*) as Cnt from (Select distinct vchnum,Icode from itemosp) group by  trim(icode)) a, item b where trim(A.icode)=trim(b.icode) and length(Trim(A.icode))>4 and a.cnt>1 order by b.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Similar Parent Code in same BOM", frm_qstr);
                    break;
                case "F10236":
                    SQuery = "select distinct b.iname,b.cpartno,a.icode,a.cnt from (select trim(ibcode) as icode,count(*) as Cnt,vchnum from (Select distinct vchnum,Ibcode,srno from itemosp) group by  trim(ibcode),vchnum) a, item b where trim(A.icode)=trim(b.icode) and length(Trim(A.icode))>4 and a.cnt>1 order by b.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Similar Child Code in same BOM", frm_qstr);
                    break;
                case "F10237":
                    SQuery = "select b.iname,b.cpartno,b.unit,a.ibcode as ERP_CODe From (select distinct trim(ibcode) as ibcode from itemosp where icode like '9%' and ibcode like '7%')a, item b  where trim(A.ibcode)=trim(B.icode) and trim(a.ibcode) not in (Select distinct trim(icode) from itemosp where icode like '7%') order by b.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of FG Linked SF Items without BOM ", frm_qstr);
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
                case "F10194*":
                    SQuery = "select acref as fstr,name as stagename,acref as code,type1 from typeGRP where BRANCHCD='" + mbr + "' and ID='WI' and acref like '6%' order by type1";
                    header_n = "Select Stage for WIP Section";
                    break;
                case "F10183":
                case "F10184":
                case "F10184C":
                case "F10186":
                case "F10194":
                case "F10194E":
                case "F10194F":
                case "F10198":
                case "F10198W":
                case "F05125E":
                case "F05125D":
                    SQuery = "";
                    fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
                    break;

                case "F10304":
                case "F10307":
                case "F10309":
                case "F10310":
                    SQuery = "select mthnum as fstr,mthnum,mthname from mths";
                    // SQuery = "select trim(mthnum)||trim(mthname) as date_,mthname";
                    header_n = "Select Month";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F10304" || HCID == "F10307" || HCID == "F10309" || HCID == "F10310")
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
                    case "F10309":
                    case "F10304":
                        #region
                        dtm = new DataTable();
                        dtm.Columns.Add("Line", typeof(string)); //1
                        dtm.Columns.Add("Area", typeof(string)); //2
                        dtm.Columns.Add("Item_Code", typeof(string)); //3
                        dtm.Columns.Add("Part_Number", typeof(string)); //3
                        dtm.Columns.Add("Part_Name", typeof(string)); //4
                        dtm.Columns.Add("Process", typeof(string)); //5
                        dtm.Columns.Add("Unit", typeof(string)); //6
                        dtm.Columns.Add("Unit1", typeof(string)); //7
                        dtm.Columns.Add("Cycle_Time", typeof(string)); //8
                        dtm.Columns.Add("Cavity_No_OF_Pcs", typeof(string)); //9 
                        dtm.Columns.Add("Operating_Rate_Per", typeof(string)); //10
                        dtm.Columns.Add("Attendance_Per", typeof(string)); //11
                        dtm.Columns.Add("No_of_Manpower_Deployed", typeof(string)); //12
                        dtm.Columns.Add("Cycle_Piece", typeof(string)); //13
                        dtm.Columns.Add("Manhours", typeof(double)); //14

                        string next_year = "", dd2 = "";
                        string[] arr = value1.Split(',');
                        int counter = 0; string dd1 = "";
                        counter = arr.Length;

                        for (int l = 0; l < counter; l++)
                        {
                            if (Convert.ToInt32(arr[l].ToString().Replace("'", "")) <= 3)
                            {
                                dd2 = arr[l].ToString().Replace("'", "") + (Convert.ToInt32(year) + 1).ToString();
                            }
                            else
                            {
                                dd2 = arr[l].ToString().Replace("'", "") + year;
                            }
                            next_year = ",'" + dd2 + "'";
                            dd1 = dd1 + next_year;
                        }

                        dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
                        mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
                        string year1 = System.DateTime.Now.Year.ToString();
                        SQuery = "SELECT TRIM(ICODE) AS ICODE,TRIM(INAME) AS INAME,TRIM(CPARTNO) AS PART,TRIM(UNIT) AS UNIT FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1)>='9' AND  LENGTH(TRIM(ICODE))>=8 ORDER BY ICODE";
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery); ///item table

                        mq0 = "select name,type1 from typegrp where id='^7'";
                        dt1 = fgen.getdata(frm_qstr, co_cd, mq0); // for area

                        mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and trim(icode)='90010022' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and trim(icode)='90010022')) and a.branchcd='" + mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
                        mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9')) and a.branchcd='" + mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by icode,a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
                        //90010022

                        dt2 = fgen.getdata(frm_qstr, co_cd, mq1);  //UNION OF PSHEDULE AND MTHLYPLAN TABLE......main dt

                        mq3 = "SELECT A.ICODE,SUM(A.TARGET) AS TARGET,B.MTHNAME,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ FROM MTHLYPLAN A ,mths b  WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='10'  AND TO_CHAR(A.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=TRIM(B.MTHNUM)  GROUP BY A.ICODE,A.VCHDATE,B.MTHNAME ,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd ";
                        dt3 = fgen.getdata(frm_qstr, co_cd, mq3);  //plan dt

                        mq4 = "select trim(a.icode) as icode ,SUM(a.TOTAL) AS TOTAL,b.mthname ,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ from PSCHEDULE a ,mths b where a.BRANCHCD='" + mbr + "' and a.type='15' and TO_CHAR(a.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=trim(b.mthnum) group by trim(a.icode),b.mthname,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd"; //this qry used when need to show month
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, co_cd, mq4);  //schedule dt  

                        mq10 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='B26'", "params");

                        mq5 = "select name,type1 from type where id='1' and  type1>'69' and type1!='6R'";
                        dt5 = fgen.getdata(frm_qstr, co_cd, mq5); //for line, line no 

                        if (dt2.Rows.Count > 0)
                        {
                            counter = arr.Length;
                            for (int k = 0; k < counter; k++)
                            {
                                dtm.Columns.Add("Vulcanisation" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Transfer" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Transfer_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                            }

                            mq1 = "";
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                #region
                                dr1 = dtm.NewRow();
                                double db10 = 0, db11 = 0;
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; mq5 = ""; mq6 = "";
                                dr1["Area"] = fgen.seek_iname_dt(dt1, "type1='" + dt2.Rows[i]["area"].ToString().Trim() + "'", "name");
                                dr1["Line"] = fgen.seek_iname_dt(dt5, "type1='" + dt2.Rows[i]["Lineno"].ToString().Trim() + "'", "name"); //abi
                                dr1["Item_Code"] = dt2.Rows[i]["icode"].ToString().Trim();
                                dr1["Part_Number"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "PART");
                                dr1["Part_Name"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                                dr1["Process"] = dt2.Rows[i]["process"].ToString().Trim(); //fgen.seek_iname_dt(dt3, "icode='" + dtm1.Rows[i]["icode"].ToString().Trim() + "'", "process");
                                dr1["Unit"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "unit");
                                dr1["Unit1"] = dr1["unit"].ToString().Trim() + "/" + "PCE";
                                dr1["Cycle_Time"] = fgen.make_double(dt2.Rows[i]["cycle_time"].ToString().Trim());
                                dr1["Cavity_No_OF_Pcs"] = fgen.make_double(dt2.Rows[i]["CAVITY_PC"].ToString().Trim());
                                dr1["Operating_Rate_Per"] = fgen.make_double(dt2.Rows[i]["OP_RATE"].ToString().Trim());
                                dr1["Attendance_Per"] = mq10;
                                dr1["No_of_Manpower_Deployed"] = fgen.make_double(dt2.Rows[i]["NO_MAN"].ToString().Trim());
                                db4 = fgen.make_double(dr1["Cycle_Time"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Cavity_No_OF_Pcs"].ToString().Trim());
                                db6 = fgen.make_double(dr1["Operating_Rate_Per"].ToString().Trim());
                                db7 = fgen.make_double(dr1["Attendance_Per"].ToString().Trim());
                                db8 = fgen.make_double(dr1["No_of_Manpower_Deployed"].ToString().Trim());
                                if (db5 == 0 || db6 == 0 || db7 == 0)
                                {
                                    dr1["Cycle_Piece"] = 0;
                                }
                                else
                                {
                                    db9 = ((db4 / db5 / db6 / db7) * db8) * 100;
                                    dr1["Cycle_Piece"] = Math.Round(db9, 5);
                                }
                                db3 = fgen.make_double(dr1["Cycle_Piece"].ToString().Trim());

                                mq5 = "";
                                for (int k = 0; k < counter; k++)
                                {
                                    mq9 = "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + "";
                                    db = fgen.make_double(fgen.seek_iname_dt(dt4, mq9, "TOTAL")); //for  Vulcanisation
                                    db1 = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + " ", "TARGET"));//for transfer                                     
                                    dr1["Vulcanisation" + arr[k].Replace("'", "_") + ""] = db;
                                    dr1["Transfer" + arr[k].Replace("'", "_") + ""] = db1;
                                    if (db3 == 0)
                                    {
                                        dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                        dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                    }
                                    else
                                    {
                                        // dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db * db3 / 3600);
                                        dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                        dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db1 * db3 / 3600);
                                    }
                                    db10 = fgen.make_double(dr1["Transfer" + arr[k].Replace("'", "_") + ""].ToString());
                                    if (db3 == 0 || db10 == 0)
                                    {
                                        dr1["Manhours"] = 0;
                                    }
                                    else
                                    {
                                        dr1["Manhours"] = Math.Round((db3) * db10 / 3600, 5);
                                    }
                                }
                                dtm.Rows.Add(dr1);
                                #endregion
                            }
                        }
                        if (val == "F10309")
                        {
                            ph_tbl = new DataTable();
                            ph_tbl.Columns.Add("Area", typeof(string)); //2
                            ph_tbl.Columns.Add("Line", typeof(string)); //2
                            ph_tbl.Columns.Add("Item_Code", typeof(string)); //3
                            ph_tbl.Columns.Add("Part_Number", typeof(string)); //3
                            ph_tbl.Columns.Add("Part_Name", typeof(string)); //4                         
                            ph_tbl.Columns.Add("Unit", typeof(string)); //6                     
                            ph_tbl.Columns.Add("No_of_Manpower_Deployed", typeof(double)); //12
                            if (dtm.Rows.Count > 0)
                            {
                                DataView view1 = new DataView(dtm);
                                DataTable dtdrsim = new DataTable();
                                dtdrsim = view1.ToTable(true, "area"); //MAIN   
                                mq1 = "";
                                foreach (DataRow dr0 in dtdrsim.Rows)
                                {
                                    DataView viewim = new DataView(dtm, "area='" + dr0["area"] + "'", "", DataViewRowState.CurrentRows);
                                    dt4 = new DataTable();
                                    dt4 = viewim.ToTable();
                                    dr1 = ph_tbl.NewRow();
                                    db = 0; db1 = 0; db2 = 0;
                                    for (int i = 0; i < dt4.Rows.Count; i++)
                                    {
                                        dr1["Area"] = dt4.Rows[i]["Area"].ToString().Trim();
                                        dr1["Line"] = dt4.Rows[i]["Line"].ToString().Trim();
                                        dr1["Item_Code"] = dt4.Rows[i]["Item_Code"].ToString().Trim();
                                        dr1["Part_Number"] = dt4.Rows[i]["Part_Number"].ToString().Trim();
                                        dr1["Part_Name"] = dt4.Rows[i]["Part_Name"].ToString().Trim();
                                        dr1["Unit"] = dt4.Rows[i]["Unit"].ToString().Trim();
                                        db += fgen.make_double(dt4.Rows[i]["No_of_Manpower_Deployed"].ToString().Trim());
                                        dr1["No_of_Manpower_Deployed"] = db;
                                    }
                                    if (dt4.Rows.Count > 0)
                                    {
                                        ph_tbl.Rows.Add(dr1);
                                    }
                                }
                                dt3 = new DataTable();
                                dt3 = ph_tbl.Copy();
                                if (dt3.Rows.Count > 0)
                                {
                                    dt3.Columns.Remove("Line");
                                    dt3.Columns.Remove("Item_Code");
                                    dt3.Columns.Remove("Part_Number");
                                    dt3.Columns.Remove("Part_Name");
                                    dt3.Columns.Remove("Unit");
                                }
                            }
                            fgen.Fn_FillChart(co_cd, frm_qstr, "Man Power Planning- Area Wise For the Month (" + value1 + ")", "pie", "", "", dt3, "");
                        }
                        ////for add row on top for total
                        else
                        {
                            if (dtm.Rows.Count > 0)
                            {
                                dr1 = dtm.NewRow();
                                foreach (DataColumn dc in dtm.Columns)
                                {
                                    db1 = 0;
                                    if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 7 || dc.Ordinal == 8 || dc.Ordinal == 9 || dc.Ordinal == 10 || dc.Ordinal == 11 || dc.Ordinal == 12 || dc.Ordinal == 13 || dc.Ordinal == 14)
                                    { }
                                    else
                                    {
                                        mq1 = "sum(" + dc.ColumnName + ")";
                                        db1 += fgen.make_double(dtm.Compute(mq1, "").ToString());
                                        dr1[dc] = db1;
                                    }
                                }
                                dr1[2] = "TOTAL";
                                dtm.Rows.InsertAt(dr1, 0);
                            }
                            if (dtm.Rows.Count > 0)
                            {
                                Session["send_dt"] = dtm;
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                                fgen.Fn_open_rptlevel("Manpower Planning Report For the Month (" + value1 + ") ", frm_qstr);
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Found");
                            }
                        }
                        #endregion
                        break;

                    case "F10307":
                        #region
                        dtm = new DataTable();
                        dtm.Columns.Add("Line", typeof(string)); //1
                        dtm.Columns.Add("Area", typeof(string)); //2
                        dtm.Columns.Add("Item_Code", typeof(string)); //3
                        dtm.Columns.Add("Part_Number", typeof(string)); //3
                        dtm.Columns.Add("Part_Name", typeof(string)); //4
                        dtm.Columns.Add("Process", typeof(string)); //5
                        dtm.Columns.Add("Unit", typeof(string)); //6
                        dtm.Columns.Add("Unit1", typeof(string)); //7
                        dtm.Columns.Add("Cycle_Time", typeof(string)); //8
                        dtm.Columns.Add("Cavity_No_OF_Pcs", typeof(string)); //9 
                        dtm.Columns.Add("Operating_Rate_Per", typeof(string)); //10
                        dtm.Columns.Add("Attendance_Per", typeof(string)); //11
                        dtm.Columns.Add("No_of_Manpower_Deployed", typeof(string)); //12
                        dtm.Columns.Add("Cycle_Piece", typeof(string)); //13
                        dtm.Columns.Add("Manhours", typeof(string)); //14
                        next_year = ""; dd2 = "";
                        arr = value1.Split(',');
                        counter = 0; dd1 = "";
                        counter = arr.Length;
                        for (int l = 0; l < counter; l++)
                        {
                            if (Convert.ToInt32(arr[l].ToString().Replace("'", "")) <= 3)
                            {
                                dd2 = arr[l].ToString().Replace("'", "") + (Convert.ToInt32(year) + 1).ToString();
                            }
                            else
                            {
                                dd2 = arr[l].ToString().Replace("'", "") + year;
                            }
                            next_year = ",'" + dd2 + "'";
                            dd1 = dd1 + next_year;
                        }
                        dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
                        mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
                        year1 = System.DateTime.Now.Year.ToString();
                        SQuery = "SELECT TRIM(ICODE) AS ICODE,TRIM(INAME) AS INAME,TRIM(CPARTNO) AS PART,TRIM(UNIT) AS UNIT FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1)>='9' AND  LENGTH(TRIM(ICODE))>=8 ORDER BY ICODE";
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery); ///item table

                        mq0 = "select name,type1 from typegrp where id='^7'";//type change karni hai
                        dt1 = fgen.getdata(frm_qstr, co_cd, mq0); // for area

                        mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and trim(icode)='90010022' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and trim(icode)='90010022')) and a.branchcd='" + mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
                        mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9')) and a.branchcd='" + mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by icode,a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
                        //90010022

                        dt2 = fgen.getdata(frm_qstr, co_cd, mq1);  //UNION OF PSHEDULE AND MTHLYPLAN TABLE......main dt

                        mq3 = "SELECT A.ICODE,SUM(A.TARGET) AS TARGET,B.MTHNAME,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ FROM MTHLYPLAN A ,mths b  WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='10'  AND TO_CHAR(A.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=TRIM(B.MTHNUM)  GROUP BY A.ICODE,A.VCHDATE,B.MTHNAME ,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd ";
                        dt3 = fgen.getdata(frm_qstr, co_cd, mq3);  //plan dt

                        mq4 = "select trim(a.icode) as icode ,SUM(a.TOTAL) AS TOTAL,b.mthname ,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ from PSCHEDULE a ,mths b where a.BRANCHCD='" + mbr + "' and a.type='15' and TO_CHAR(a.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=trim(b.mthnum) group by trim(a.icode),b.mthname,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd"; //this qry used when need to show month
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, co_cd, mq4);  //schedule dt  

                        mq10 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='B26'", "params");

                        mq5 = "select name,type1 from type where id='1' and  type1>'69' and type1!='6R'";
                        dt5 = fgen.getdata(frm_qstr, co_cd, mq5); //for line, line no 

                        if (dt2.Rows.Count > 0)
                        {
                            counter = arr.Length;
                            for (int k = 0; k < counter; k++)
                            {
                                dtm.Columns.Add("Vulcanisation" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Transfer" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Transfer_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                            }
                            mq1 = "";
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                #region
                                dr1 = dtm.NewRow();
                                double db10 = 0, db11 = 0;
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; mq5 = ""; mq6 = "";
                                dr1["Area"] = fgen.seek_iname_dt(dt1, "type1='" + dt2.Rows[i]["area"].ToString().Trim() + "'", "name");
                                dr1["Line"] = fgen.seek_iname_dt(dt5, "type1='" + dt2.Rows[i]["Lineno"].ToString().Trim() + "'", "name"); //abi
                                dr1["Item_Code"] = dt2.Rows[i]["icode"].ToString().Trim();
                                dr1["Part_Number"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "PART");
                                dr1["Part_Name"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                                dr1["Process"] = dt2.Rows[i]["process"].ToString().Trim(); //fgen.seek_iname_dt(dt3, "icode='" + dtm1.Rows[i]["icode"].ToString().Trim() + "'", "process");
                                dr1["Unit"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "unit");
                                dr1["Unit1"] = dr1["unit"].ToString().Trim() + "/" + "PCE";
                                dr1["Cycle_Time"] = fgen.make_double(dt2.Rows[i]["cycle_time"].ToString().Trim());
                                dr1["Cavity_No_OF_Pcs"] = fgen.make_double(dt2.Rows[i]["CAVITY_PC"].ToString().Trim());
                                dr1["Operating_Rate_Per"] = fgen.make_double(dt2.Rows[i]["OP_RATE"].ToString().Trim());
                                dr1["Attendance_Per"] = mq10;
                                dr1["No_of_Manpower_Deployed"] = fgen.make_double(dt2.Rows[i]["NO_MAN"].ToString().Trim());
                                db4 = fgen.make_double(dr1["Cycle_Time"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Cavity_No_OF_Pcs"].ToString().Trim());
                                db6 = fgen.make_double(dr1["Operating_Rate_Per"].ToString().Trim());
                                db7 = fgen.make_double(dr1["Attendance_Per"].ToString().Trim());
                                db8 = fgen.make_double(dr1["No_of_Manpower_Deployed"].ToString().Trim());
                                if (db5 == 0 || db6 == 0 || db7 == 0)
                                {
                                    dr1["Cycle_Piece"] = 0;
                                }
                                else
                                {
                                    db9 = ((db4 / db5 / db6 / db7) * db8) * 100;
                                    dr1["Cycle_Piece"] = Math.Round(db9, 5);
                                }
                                db3 = fgen.make_double(dr1["Cycle_Piece"].ToString().Trim());

                                mq5 = "";
                                for (int k = 0; k < counter; k++)
                                {
                                    mq9 = "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + "";
                                    db = fgen.make_double(fgen.seek_iname_dt(dt4, mq9, "TOTAL")); //for  Vulcanisation
                                    db1 = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + " ", "TARGET"));//for transfer                                     
                                    dr1["Vulcanisation" + arr[k].Replace("'", "_") + ""] = db;
                                    dr1["Transfer" + arr[k].Replace("'", "_") + ""] = db1;
                                    if (db3 == 0)
                                    {
                                        dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                        dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                    }
                                    else
                                    {
                                        // dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db * db3 / 3600);
                                        dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                        dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db1 * db3 / 3600);
                                    }
                                    db10 = fgen.make_double(dr1["Transfer" + arr[k].Replace("'", "_") + ""].ToString());
                                    if (db3 == 0 || db10 == 0)
                                    {
                                        dr1["Manhours"] = 0;
                                    }
                                    else
                                    {
                                        dr1["Manhours"] = Math.Round((db3) * db10 / 3600, 5);
                                    }
                                }
                                dtm.Rows.Add(dr1);
                                #endregion
                            }
                        }
                        #region
                        ph_tbl = new DataTable();
                        ph_tbl.Columns.Add("Area", typeof(string)); //2
                        ph_tbl.Columns.Add("Line", typeof(string)); //2
                        ph_tbl.Columns.Add("Item_Code", typeof(string)); //3
                        ph_tbl.Columns.Add("Part_Number", typeof(string)); //3
                        ph_tbl.Columns.Add("Part_Name", typeof(string)); //4                         
                        ph_tbl.Columns.Add("Unit", typeof(string)); //6                     
                        ph_tbl.Columns.Add("No_of_Manpower_Deployed", typeof(double)); //12

                        if (dtm.Rows.Count > 0)
                        {
                            DataView view1 = new DataView(dtm);
                            DataTable dtdrsim = new DataTable();
                            dtdrsim = view1.ToTable(true, "area", "item_code"); //MAIN   
                            mq1 = "";
                            foreach (DataRow dr0 in dtdrsim.Rows)
                            {
                                DataView viewim = new DataView(dtm, "area='" + dr0["area"] + "' and item_code='" + dr0["item_code"] + "' ", "", DataViewRowState.CurrentRows);
                                dt4 = new DataTable();
                                dt4 = viewim.ToTable();
                                dr1 = ph_tbl.NewRow();
                                db = 0; db1 = 0; db2 = 0;
                                //if case is work when area is different
                                if (mq1 != dr0["area"].ToString())
                                {
                                    //this is for showing group name in diff row
                                    dr1["area"] = dr0["area"].ToString();
                                    ph_tbl.Rows.Add(dr1);
                                    ////
                                    dr1 = ph_tbl.NewRow();
                                    for (int i = 0; i < dt4.Rows.Count; i++)
                                    {
                                        dr1["Line"] = dt4.Rows[i]["Line"].ToString().Trim();
                                        dr1["Item_Code"] = dt4.Rows[i]["Item_Code"].ToString().Trim();
                                        dr1["Part_Number"] = dt4.Rows[i]["Part_Number"].ToString().Trim();
                                        dr1["Part_Name"] = dt4.Rows[i]["Part_Name"].ToString().Trim();
                                        dr1["Unit"] = dt4.Rows[i]["Unit"].ToString().Trim();
                                        db += fgen.make_double(dt4.Rows[i]["No_of_Manpower_Deployed"].ToString().Trim());
                                        dr1["No_of_Manpower_Deployed"] = db;
                                        mq1 = dt4.Rows[i]["AREA"].ToString().Trim();
                                    }
                                }
                                else //this work when area is same and doing sum on base of area and icode
                                {
                                    for (int i = 0; i < dt4.Rows.Count; i++)
                                    {
                                        dr1["Line"] = dt4.Rows[i]["Line"].ToString().Trim();
                                        dr1["Item_Code"] = dt4.Rows[i]["Item_Code"].ToString().Trim();
                                        dr1["Part_Number"] = dt4.Rows[i]["Part_Number"].ToString().Trim();
                                        dr1["Part_Name"] = dt4.Rows[i]["Part_Name"].ToString().Trim();
                                        dr1["Unit"] = dt4.Rows[i]["Unit"].ToString().Trim();
                                        db += fgen.make_double(dt4.Rows[i]["No_of_Manpower_Deployed"].ToString().Trim());
                                        dr1["No_of_Manpower_Deployed"] = db;
                                    }
                                }
                                ph_tbl.Rows.Add(dr1);
                            }
                        }
                        #endregion
                        //////==========================
                        if (ph_tbl.Rows.Count > 0)
                        {
                            Session["send_dt"] = ph_tbl;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("Manpower Planning-Area wise Summary For the Month (" + value1 + ") ", frm_qstr);
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Found");
                        }
                        #endregion
                        break;

                    case "F10310":
                        #region
                        dtm = new DataTable();
                        dtm.Columns.Add("Line", typeof(string)); //1
                        dtm.Columns.Add("Area", typeof(string)); //2
                        dtm.Columns.Add("Item_Code", typeof(string)); //3
                        dtm.Columns.Add("Part_Number", typeof(string)); //3
                        dtm.Columns.Add("Part_Name", typeof(string)); //4
                        dtm.Columns.Add("Process", typeof(string)); //5
                        dtm.Columns.Add("Unit", typeof(string)); //6
                        dtm.Columns.Add("Unit1", typeof(string)); //7
                        dtm.Columns.Add("Cycle_Time", typeof(string)); //8
                        dtm.Columns.Add("Cavity_No_OF_Pcs", typeof(string)); //9 
                        dtm.Columns.Add("Operating_Rate_Per", typeof(string)); //10
                        dtm.Columns.Add("Attendance_Per", typeof(string)); //11
                        dtm.Columns.Add("No_of_Manpower_Deployed", typeof(string)); //12
                        dtm.Columns.Add("Cycle_Piece", typeof(string)); //13
                        dtm.Columns.Add("Manhours", typeof(double)); //14                      

                        next_year = ""; dd2 = "";
                        arr = value1.Split(',');
                        counter = 0; dd1 = "";
                        counter = arr.Length;

                        for (int l = 0; l < counter; l++)
                        {
                            if (Convert.ToInt32(arr[l].ToString().Replace("'", "")) <= 3)
                            {
                                dd2 = arr[l].ToString().Replace("'", "") + (Convert.ToInt32(year) + 1).ToString();
                            }
                            else
                            {
                                dd2 = arr[l].ToString().Replace("'", "") + year;
                            }
                            next_year = ",'" + dd2 + "'";
                            dd1 = dd1 + next_year;
                        }

                        dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
                        mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
                        year1 = System.DateTime.Now.Year.ToString();
                        SQuery = "SELECT TRIM(ICODE) AS ICODE,TRIM(INAME) AS INAME,TRIM(CPARTNO) AS PART,TRIM(UNIT) AS UNIT FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1)>='9' AND  LENGTH(TRIM(ICODE))>=8 ORDER BY ICODE";
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery); ///item table

                        mq0 = "select name,type1 from typegrp where id='^7'";//type change karni hai
                        dt1 = fgen.getdata(frm_qstr, co_cd, mq0); // for area

                        mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and trim(icode)='90010022' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and trim(icode)='90010022')) and a.branchcd='" + mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
                        mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9')) and a.branchcd='" + mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
                        //90010022
                        dt2 = fgen.getdata(frm_qstr, co_cd, mq1);  //UNION OF PSHEDULE AND MTHLYPLAN TABLE......main dt

                        mq3 = "SELECT A.ICODE,SUM(A.TARGET) AS TARGET,B.MTHNAME,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ FROM MTHLYPLAN A ,mths b  WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='10'  AND TO_CHAR(A.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=TRIM(B.MTHNUM)  GROUP BY A.ICODE,A.VCHDATE,B.MTHNAME ,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd ";
                        dt3 = fgen.getdata(frm_qstr, co_cd, mq3);  //plan dt

                        mq4 = "select trim(a.icode) as icode ,SUM(a.TOTAL) AS TOTAL,b.mthname ,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ from PSCHEDULE a ,mths b where a.BRANCHCD='" + mbr + "' and a.type='15' and TO_CHAR(a.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=trim(b.mthnum) group by trim(a.icode),b.mthname,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd"; //this qry used when need to show month
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, co_cd, mq4);  //schedule dt  

                        mq10 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='B26'", "params");

                        mq5 = "select name,type1 from type where id='1' and  type1>'69' and type1!='6R'";
                        dt5 = fgen.getdata(frm_qstr, co_cd, mq5); //for line, line no 

                        if (dt2.Rows.Count > 0)
                        {
                            counter = arr.Length;
                            for (int k = 0; k < counter; k++)
                            {
                                dtm.Columns.Add("Vulcanisation" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Transfer" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                                dtm.Columns.Add("Transfer_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                            }

                            mq1 = "";
                            for (int i = 0; i < dt2.Rows.Count; i++)
                            {
                                #region
                                dr1 = dtm.NewRow();
                                double db10 = 0, db11 = 0;
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; mq5 = ""; mq6 = "";
                                dr1["Area"] = fgen.seek_iname_dt(dt1, "type1='" + dt2.Rows[i]["area"].ToString().Trim() + "'", "name");
                                dr1["Line"] = fgen.seek_iname_dt(dt5, "type1='" + dt2.Rows[i]["Lineno"].ToString().Trim() + "'", "name"); //abi
                                dr1["Item_Code"] = dt2.Rows[i]["icode"].ToString().Trim();
                                dr1["Part_Number"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "PART");
                                dr1["Part_Name"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                                dr1["Process"] = dt2.Rows[i]["process"].ToString().Trim(); //fgen.seek_iname_dt(dt3, "icode='" + dtm1.Rows[i]["icode"].ToString().Trim() + "'", "process");
                                dr1["Unit"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "unit");
                                dr1["Unit1"] = dr1["unit"].ToString().Trim() + "/" + "PCE";
                                dr1["Cycle_Time"] = fgen.make_double(dt2.Rows[i]["cycle_time"].ToString().Trim());
                                dr1["Cavity_No_OF_Pcs"] = fgen.make_double(dt2.Rows[i]["CAVITY_PC"].ToString().Trim());
                                dr1["Operating_Rate_Per"] = fgen.make_double(dt2.Rows[i]["OP_RATE"].ToString().Trim());
                                dr1["Attendance_Per"] = mq10;
                                dr1["No_of_Manpower_Deployed"] = fgen.make_double(dt2.Rows[i]["NO_MAN"].ToString().Trim());
                                db4 = fgen.make_double(dr1["Cycle_Time"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Cavity_No_OF_Pcs"].ToString().Trim());
                                db6 = fgen.make_double(dr1["Operating_Rate_Per"].ToString().Trim());
                                db7 = fgen.make_double(dr1["Attendance_Per"].ToString().Trim());
                                db8 = fgen.make_double(dr1["No_of_Manpower_Deployed"].ToString().Trim());
                                if (db5 == 0 || db6 == 0 || db7 == 0)
                                {
                                    dr1["Cycle_Piece"] = 0;
                                }
                                else
                                {
                                    db9 = ((db4 / db5 / db6 / db7) * db8) * 100;
                                    dr1["Cycle_Piece"] = Math.Round(db9, 5);
                                }
                                db3 = fgen.make_double(dr1["Cycle_Piece"].ToString().Trim());

                                mq5 = "";
                                for (int k = 0; k < counter; k++)
                                {
                                    mq9 = "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + "";
                                    db = fgen.make_double(fgen.seek_iname_dt(dt4, mq9, "TOTAL")); //for  Vulcanisation
                                    db1 = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + "", "TARGET"));//for transfer                                     
                                    dr1["Vulcanisation" + arr[k].Replace("'", "_") + ""] = db;
                                    dr1["Transfer" + arr[k].Replace("'", "_") + ""] = db1;
                                    if (db3 == 0)
                                    {
                                        dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                        dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                    }
                                    else
                                    {
                                        // dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db * db3 / 3600);
                                        dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                                        dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db1 * db3 / 3600);
                                    }
                                    db10 = fgen.make_double(dr1["Transfer" + arr[k].Replace("'", "_") + ""].ToString());
                                    if (db3 == 0 || db10 == 0)
                                    {
                                        dr1["Manhours"] = 0;
                                    }
                                    else
                                    {
                                        dr1["Manhours"] = Math.Round((db3) * db10 / 3600, 5);
                                    }
                                }
                                dtm.Rows.Add(dr1);
                                #endregion
                            }
                        }
                        string values = "";
                        ph_tbl = new DataTable();
                        if (dtm.Rows.Count > 0)
                        {
                            dr1 = dtm.NewRow();
                            ph_tbl = dtm.Clone();
                            foreach (DataColumn dc in dtm.Columns)
                            {
                                db1 = 0;
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 7 || dc.Ordinal == 8 || dc.Ordinal == 9 || dc.Ordinal == 10 || dc.Ordinal == 11 || dc.Ordinal == 12 || dc.Ordinal == 13 || dc.Ordinal == 14)
                                { }
                                else
                                {
                                    mq1 = "sum(" + dc.ColumnName + ")";
                                    db1 += fgen.make_double(dtm.Compute(mq1, "").ToString());
                                    dr1[dc] = db1;
                                    if (dc.ColumnName.Contains("Transfer_ManPower_Req_"))
                                    {
                                        values = values + "," + db1;
                                    }
                                }
                            }
                            dr1[2] = "TOTAL";
                            dtm.Rows.InsertAt(dr1, 0);
                            ph_tbl.ImportRow(dr1);
                        }
                        if (ph_tbl.Rows.Count > 0)
                        {
                            dt3 = new DataTable();
                            dt3 = fgen.getdata(frm_qstr, co_cd, "select mthnum,substr(mthname,1,3) as mthname from mths");
                            string values1 = values.TrimStart(',');
                            string[] hrs = values.TrimStart(',').Split(',');

                            dt5 = new DataTable();
                            dt5.Columns.Add("MONTH", typeof(string));
                            dt5.Columns.Add("HRS", typeof(double));

                            counter = arr.Length;
                            for (int k = 0; k < counter; k++)
                            {
                                dro = dt5.NewRow();
                                dro["month"] = fgen.seek_iname_dt(dt3, "mthnum='" + arr[k].Replace("'", "") + "'", "mthname");
                                dro["hrs"] = hrs[k].ToString();
                                dt5.Rows.Add(dro);
                            }
                            fgen.Fn_FillChart(co_cd, frm_qstr, "Man Hours Calculation For the Month (" + value1 + ") ", "line", "", "", dt5, "");
                        }
                        #endregion
                        break;

                    case "F10194":
                    case "F10194E":
                    case "F10194F":
                    case "F10198":
                    case "F10198W":
                    case "F05125E":
                    case "F05125D":
                        hfcode.Value = value1;
                        fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
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

                case "F10156":
                    string party_cd = "";
                    string part_cd = "";
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
                    SQuery = "select b.Name as Main_Grp_name,a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.unit,a.icode as ERP_code" +
                        ",substr(a.icode,1,4) as subgrp,a.icat, a.iweight as weight,a.Maker as Make_or_Brand, a.Ent_by" +
                        ",a.ent_Dt,a.Edt_by,a.edt_Dt from item a,type b where b.id='Y' and substr(a.icode,1,2)=b.type1 and b.id='Y' and a.branchcd!='DD' and length(Trim(A.icode))>4 and length(trim(nvl(a.deac_by,'-'))) <2 and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by substr(a.icode,1,4),A.Iname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Item Master List", frm_qstr,"");
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

                // CREATED AND MERGED BY MADHVI AND MERGING DATE IS 12 JAN 2018

                case "F10227":
                    SQuery = "select b.Name as Main_Grp_name,a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.unit,a.icode as ERP_code,substr(a.icode,1,4) as subgrp,a.Ent_by,a.ent_Dt,a.Edt_by,a.edt_Dt from item a,type b where b.id='Y' and substr(a.icode,1,2)=b.type1 and a.branchcd!='DD' and length(Trim(A.icode))>4 and trim(a.icode) not in (Select distinct trim(icode) from ivoucher where vchdate " + xprdrange + " union all Select distinct trim(icode) from pomas where orddt " + xprdrange + " union all Select distinct trim(icode) from Somas where orddt " + xprdrange + " )  order by substr(a.icode,1,4),A.Iname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Items Not Used for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F10184C":
                    //Valuation on Process Plan 
                    string branchWiseBM = "";

                    //*****************************mark it Y if company has branch wise BOM
                    if (co_cd == "SAGM") branchWiseBM = "Y";
                    branch_Cd = "BRANCHCD NOT IN ('DD','88')";
                    if (branchWiseBM == "Y") branch_Cd = "branchcd='" + mbr + "'";
                    //*****************************

                    string akmcode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1");
                    string aksubcode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2");
                    string akicode1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
                    string akicode2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");
                    cond = " like '%'";
                    if (akmcode.ToString().Trim() != "") cond = " like '" + akmcode.ToString().Trim() + "%'";
                    if (aksubcode.ToString().Trim() != "") cond = " like '" + aksubcode.ToString().Trim() + "%'";
                    if (akicode1.ToString().Trim() != "") { cond = " ='" + akicode1.ToString().Trim() + "' "; }
                    if (akicode2.ToString().Trim() != "") { cond = " between '" + akicode1.ToString().Trim() + "' and '" + akicode2.ToString().Trim() + "'"; }


                    mdt = new DataTable(); dt3 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); mdt1 = new DataTable(); vdt = new DataTable();

                    if ((HCID == "F10184C") && akmcode.ToString().Trim() == "-" && aksubcode.ToString().Trim() == "-" && akicode1.ToString().Trim() == "-" && akicode2.ToString().Trim() == "-") cond = " like '9%'";

                    //*****************************Filling Selected BOM

                    SQuery = "Select A.SRNO,a.branchcd,a.vchnum,a.vchdate,a.icode,a.ibcode,a.ibqty,a.main_issue_no,a.sub_issue_no,a.IBDIEPC,a.ibwt,a.br_stg,(case when B.IQD>0 then B.IQD else B.irate end) as itrate,b.iname as itemname,c.iname as piname,b.alloy from itemosp a,item b,item c where trim(a.ibcode)=trim(b.icode) and trim(A.icode)=trim(C.icodE) and trim(a.icode) " + cond + " AND a." + branch_Cd + " and substr(a.icode,1,1)>='7' order by a.srno,a.icode";
                    dt3 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt3.Rows.Count <= 0)
                    {
                        SQuery = "Select a.ICODE,A.IBCODE,A.IBQTY,A.MAIN_ISSUE_NO,A.IOQTY,a.srno,0 as irate,0 as val,'1' as lvl,(case when B.IQD>0 then B.IQD else B.irate end) as itrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) and trim(a.icode) " + cond + " AND a." + branch_Cd + " order by a.srno,a.icode";
                        dt3 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }
                    //*****************************

                    //*****************************Table to Fill Layered BOM, With Value
                    mdt.Columns.Add(new DataColumn("branchcd", typeof(string)));
                    mdt.Columns.Add(new DataColumn("lvl", typeof(string)));
                    mdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibqty", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("alloy", typeof(string)));
                    mdt.Columns.Add(new DataColumn("irate", typeof(string)));
                    mdt.Columns.Add(new DataColumn("val", typeof(string)));

                    mdt.Columns.Add(new DataColumn("stg_Wt", typeof(string)));

                    mdt.Columns.Add(new DataColumn("iname", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibname", typeof(string)));
                    mdt.Columns.Add(new DataColumn("jr", typeof(string)));
                    mdt.Columns.Add(new DataColumn("IBDIEPC", typeof(string)));
                    mdt.Columns.Add(new DataColumn("SUB_ISSUE_NO", typeof(string)));
                    mdt.Columns.Add(new DataColumn("wt_cnc", typeof(string)));
                    mdt.Columns.Add(new DataColumn("wt_rft", typeof(string)));
                    //*****************************

                    //*****************************Table to Fill Finish Good BOM, With Value
                    fmdt = new DataTable();
                    fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("val", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("JO_Val", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("srate", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("sqty", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("acode", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("lot_size", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("c_cost", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("wt_cnc", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("wt_rft", typeof(string)));
                    //*****************************

                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR9") == "Y") rmcBranch = "BRANCHCD NOT IN ('DD','88')";
                    else rmcBranch = "BRANCHCD='" + mbr + "'";


                    //*****************************Filling All BOM's                    
                    if (ViewState["vdt"] == null)
                    {
                        SQuery = "Select A.SRNO,a.branchcd,a.vchnum,a.vchdate,a.icode,a.ibcode,a.ibqty,a.main_issue_no,a.sub_issue_no,a.IBDIEPC,a.ibwt,a.br_stg,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,b.alloy  from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a." + branch_Cd + " order by a.srno,a.icode,a.ibcode";
                        vdt = fgen.getdata(frm_qstr, co_cd, SQuery); v = 0;
                        ViewState["vdt"] = vdt;
                    }
                    else vdt = (DataTable)ViewState["vdt"];
                    //*****************************


                    //*****************************Filling MRR for last 500 Days
                    dt2 = new DataTable();
                    string rateCond = " type like '0%' ";
                    if (ViewState["dt2" + frm_formID] == null)
                    {
                        SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where " + rmcBranch + " and " + rateCond + " and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY')  /*and icode like '9%'*/ order by icode,vdd desc";
                        //wtd avg rate
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") == "N")
                            SQuery = "Select trim(icode) as icode,round((sum(iqty_chl*ichgs) / sum(iqty_chl)) ,3) as rate,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and substr(icode,1,1)<7 /*and icode like '9%'*/ group by trim(icode) order by icode";
                        dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                        ViewState["dt2" + frm_formID] = dt2;
                    }
                    else dt2 = (DataTable)ViewState["dt2" + frm_formID];
                    //*****************************


                    //*****************************Filling PO's to find out Job Work Value (done for NIRM only)
                    dtPo = new DataTable();
                    SQuery = "SELECT distinct TRIM(ICODe) AS ICODE,PRATE,TO_CHAR(ORDDT,'YYYYMMDD') AS VDD FROM POMAS WHERE BRANCHCD='" + mbr + "' and type='53' and orddt>=(sysdate-500) and icode like '7%' /* and orddt " + xprdrange + "*/ order by vdd desc ";
                    if (co_cd == "NIRM") dtPo = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //*****************************
                    SQuery = "SELECT DISTINCT ICODE,MAINTDT,SRNO FROM INSPMST WHERE BRANCHCD='" + mbr + "' AND TYPE='70' AND SRNO=1 ORDER BY ICODE ";
                    DataTable insvchDT = fgen.getdata(frm_qstr, co_cd, SQuery);

                    //*****************************Making Distinct ICODE from Main BOM Table
                    dist1_view = new DataView(dt3);
                    dt_dist = new DataTable();
                    if (dist1_view.Count > 0)
                    {
                        dist1_view.Sort = "icode";
                        dt_dist = dist1_view.ToTable(true, "icode");
                    }
                    //*****************************

                    //*****************************Filling Itemospanx for DREM
                    DataTable bomanx = new DataTable();
                    if (co_cd == "DREM")
                    {
                        SQuery = "Select c.iname,b.name as names,a.* from itemospanx a,type b,item c  where b.id='1' and trim(a.icode)=trim(c.icode) and trim(a.stg_Cd)=trim(b.type1) and a.branchcd!='DD'";
                        bomanx = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }

                    DataTable dtItemBal = new DataTable();
                    dtItemBal = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,stagec as BSTGCD FROM itwstage WHERE BRANCHCD!='DD' ORDER BY ICODE");

                    DataTable dtMulvch = new DataTable();
                    dtMulvch = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,qty1 FROM MULTIVCH WHERE BRANCHCD='" + mbr + "' AND TYPE='PX' ");

                    //*****************************

                    double mainLotSize = 0;
                    string topickicode = "";
                    mq0 = "";
                    foreach (DataRow dt_dist_row in dt_dist.Rows)
                    {
                        mdt1 = new DataTable();
                        mdt1 = mdt.Clone();
                        mvdview = new DataView(dt3, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
                        dt = new DataTable();
                        mvdview.Sort = "srno,icode";
                        dt = mvdview.ToTable();

                        #region filling parent
                        //*****************************
                        foreach (DataRow drc in dt.Rows)
                        {
                            double cVa = 0;
                            dro = mdt1.NewRow();
                            dro["lvl"] = "1";
                            dro["branchcd"] = drc["branchcd"].ToString().Trim();
                            dro["icode"] = drc["icode"].ToString().Trim();
                            dro["iname"] = drc["piname"].ToString().Trim();
                            dro["ibname"] = drc["itemname"].ToString().Trim();
                            dro["pcode"] = drc["icode"].ToString().Trim();
                            dro["alloy"] = drc["alloy"].ToString().Trim();
                            mainLotSize = fgen.make_double(drc["main_issue_no"].ToString().Trim());
                            if (mainLotSize <= 0) mainLotSize = 1;
                            dro["ibqty"] = fgen.make_double(drc["ibqty"].ToString()) / mainLotSize;
                            dro["ibcode"] = drc["ibcode"].ToString().Trim();
                            dro["irate"] = drc["itrate"].ToString().Trim();
                            topickicode = "icode";

                            db9 = 0;
                            if (mq0 != drc[topickicode].ToString().Trim())
                            {
                                DataView dvStgFind = new DataView(dtItemBal, "ICODE='" + drc[topickicode].ToString().Trim() + "'", "BSTGCD", DataViewRowState.CurrentRows);
                                for (int d = 0; d < dvStgFind.Count; d++)
                                {
                                    mq0 = drc[topickicode].ToString().Trim();
                                    col1 = fgen.seek_iname_dt(dtMulvch, "ICODE='" + dvStgFind[d].Row["BSTGCD"].ToString().Trim() + "'", "QTY1");
                                    if (col1 != "0")
                                        db9 += col1.ToString().toDouble();
                                }
                                dro["stg_Wt"] = db9;
                            }
                            else dro["stg_Wt"] = 0;

                            dro["SUB_ISSUE_NO"] = drc["SUB_ISSUE_NO"].ToString();
                            dro["IBDIEPC"] = drc["IBDIEPC"].ToString();
                            if (drc["icode"].ToString().Trim().Substring(0, 1) == "9")
                            {
                                dro["wt_cnc"] = (drc["IBDIEPC"].ToString().toDouble() > 0 ? drc["IBDIEPC"].ToString().toDouble() : 1) * dro["stg_Wt"].ToString().toDouble();
                                dro["wt_rft"] = (drc["SUB_ISSUE_NO"].ToString().toDouble() > 0 ? drc["SUB_ISSUE_NO"].ToString().toDouble() : 1) * dro["stg_Wt"].ToString().toDouble();
                            }
                            if (dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7" && drc["ibcode"].ToString().Trim().Substring(0, 2) == "10")
                            {
                                dro["wt_cnc"] = drc["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                dro["wt_rft"] = drc["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                            }
                            dro["val"] = "0";
                            mdt1.Rows.Add(dro);
                        }
                        //*****************************
                        #endregion

                        #region filling Child with Recursive LOOP
                        //*****************************
                        i0 = 1; v = 0;
                        for (int i = v; i < mdt1.Rows.Count; i++)
                        {
                            //vipin
                            vdview = new DataView(vdt, "icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                            if (vdview.Count > 0)
                            {
                                vdview1 = new DataView(mdt1, "icode='" + mdt1.Rows[i]["icode"].ToString().Trim() + "' and ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "' and ibqty='" + mdt1.Rows[i]["ibqty"] + "'", "ibcode", DataViewRowState.CurrentRows);
                                if (vdview1.Count <= 0) vdview1 = new DataView(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibcode", DataViewRowState.CurrentRows);

                                for (int x = 0; x < vdview.Count; x++)
                                {
                                    if (mq0 != vdview[x].Row["icode"].ToString().Trim())
                                    {
                                        value3 = fgen.seek_iname_dt(mdt1, "IBCODE='" + vdview[x].Row["icode"].ToString().Trim() + "'", "LVL");
                                        if (value3 == "0")
                                            i0 += 1;
                                        else i0 = fgen.make_int(value3) + 1;
                                    }
                                    dro = mdt1.NewRow();
                                    dro["lvl"] = i0.ToString();
                                    dro["icode"] = vdview[x].Row["icode"].ToString().Trim();
                                    dro["branchcd"] = vdview[x].Row["branchcd"].ToString().Trim();
                                    mq0 = vdview[x].Row["icode"].ToString().Trim();
                                    double lotSize = fgen.make_double(vdview[x].Row["MAIN_ISSUE_NO"].ToString().Trim());
                                    if (lotSize <= 0) lotSize = 1;
                                    dro["ibqty"] = (Convert.ToDouble(vdview[x].Row["ibqty"]) * (Convert.ToDouble(vdview1[0].Row["ibqty"]) / lotSize)).ToString();

                                    dro["ibcode"] = vdview[x].Row["ibcode"].ToString().Trim();
                                    dro["alloy"] = vdview[x].Row["alloy"].ToString().Trim();

                                    if (dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7") topickicode = "icode";
                                    else topickicode = "ibcode";

                                    db9 = 0;
                                    DataView dvStgFind = new DataView(dtItemBal, "ICODE='" + vdview[x].Row[topickicode].ToString().Trim() + "'", "BSTGCD", DataViewRowState.CurrentRows);
                                    for (int d = 0; d < dvStgFind.Count; d++)
                                    {
                                        col1 = fgen.seek_iname_dt(dtMulvch, "ICODE='" + dvStgFind[d].Row["ICODE"].ToString().Trim() + "'", "QTY1");
                                        if (col1 != "0")
                                            db9 += col1.ToString().toDouble();
                                    }
                                    dro["stg_Wt"] = db9;

                                    dro["irate"] = vdview[x].Row["bchrate"];
                                    dro["ibname"] = vdview[x].Row["itemname"];

                                    dro["val"] = "0";
                                    if (mdt1.Rows[i]["lvl"].ToString() == "1")
                                    {
                                        mq7 = "";
                                        dro["pcode"] = mdt1.Rows[i]["icode"].ToString().Trim();
                                        mq7 = mdt1.Rows[i]["icode"].ToString().Trim();
                                    }
                                    else dro["pcode"] = mq7;

                                    if (vdview[x].Row["icode"].ToString().Trim().Substring(0, 1) == "7")
                                    {
                                        dro["wt_cnc"] = vdview[x].Row["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                        dro["wt_rft"] = vdview[x].Row["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                    }

                                    v++;

                                    mdt1.Rows.Add(dro);
                                } vdview1.Dispose();
                            } vdview.Dispose();
                        }
                        //*****************************
                        #endregion

                        //*****************************sorting on Parent Code,Level,Child Code
                        mdt1.DefaultView.Sort = "pcode,lvl,icode";
                        mdt1 = mdt1.DefaultView.ToTable();

                        #region seeking LC and update value
                        //*****************************
                        value1 = "";
                        for (int i = 0; i < mdt1.Rows.Count; i++)
                        {
                            vdview = new DataView(mdt1, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                            if (vdview.Count <= 0)
                            {
                                if (dt2.Rows.Count > 0)
                                {
                                    sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                    if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                    else
                                    {
                                        sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                        if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                    }
                                }
                            }
                            else mdt1.Rows[i]["irate"] = "0";
                            vdview.Dispose();
                            mdt1.Rows[i]["val"] = Convert.ToDouble(fgen.make_double(mdt1.Rows[i]["ibqty"].ToString()) * fgen.make_double(mdt1.Rows[i]["irate"].ToString()));
                            double dvl = 0;
                            if (co_cd == "NIRM")
                            {
                                dvl += fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "prate"));
                                if (fgen.make_double(mdt1.Rows[i]["jr"].ToString()) <= 0)
                                    mdt1.Rows[i]["jr"] = dvl;
                            }
                        }
                        //*****************************
                        #endregion

                        #region Filling Job Work Value for NIRM
                        //*****************************
                        mq0 = "0";
                        mq7 = "0";
                        mq10 = "0";
                        if (co_cd == "NIRM")
                        {
                            dist1_view = new DataView(mdt1);
                            dt_dist1 = new DataTable();
                            if (dist1_view.Count > 0)
                            {
                                dist1_view.Sort = "pcode";
                                dt_dist1 = dist1_view.ToTable(true, "pcode");
                            }
                            foreach (DataRow drdist1 in dt_dist1.Rows)
                            {
                                dro = mdt1.NewRow();
                                dro["icode"] = drdist1["pcode"].ToString().Trim();
                                dro["ibcode"] = drdist1["pcode"].ToString().Trim();
                                dro["pcode"] = drdist1["pcode"].ToString().Trim();
                                dro["ibqty"] = 0;
                                dro["irate"] = 0;
                                dro["val"] = 0;
                                dro["jr"] = fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + drdist1["pcode"].ToString().Trim() + "'", "prate"));
                                mdt1.Rows.Add(dro);
                            }
                        }
                        //*****************************
                        #endregion

                        #region Making Final Value of Job Work
                        //*****************************
                        double joVal = 0;
                        mq3 = "";
                        mq4 = "";
                        {
                            vdview = new DataView(mdt1, "pcode='" + dt_dist_row["icode"].ToString().Trim() + "'", "pcode", DataViewRowState.CurrentRows);
                            for (int i = 0; i < vdview.Count; i++)
                            {
                                if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(vdview[i].Row["val"].ToString().Trim()), 2).ToString();
                                else mq0 = vdview[i].Row["val"].ToString().Trim();

                                mq3 = Convert.ToString(mq3.toDouble() + vdview[i].Row["wt_cnc"].ToString().Trim().toDouble());
                                mq4 = Convert.ToString(mq4.toDouble() + vdview[i].Row["wt_rft"].ToString().Trim().toDouble());
                                mq10 = Convert.ToString(mq10.toDouble() + vdview[i].Row["stg_Wt"].ToString().Trim().toDouble());

                                if (co_cd == "NIRM")
                                {
                                    joVal += fgen.make_double(vdview[i].Row["jr"].ToString().Trim());
                                }
                            }
                        }
                        if (joVal <= 0)
                        {
                            //for (int i = 0; i < dt_dist.Rows.Count; i++)
                            {
                                double dvl = 0;
                                dvl = fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "prate"));
                                mdt1.Rows[0]["jr"] = dvl;
                                joVal += dvl;
                            }
                        }
                        //*****************************
                        #endregion

                        vdview.Dispose();

                        db6 = 0;
                        db5 = 0;
                        double mul_fact = 0;

                        for (int f = 0; f < mdt1.Rows.Count; f++)
                        {
                            mdt.ImportRow(mdt1.Rows[f]);
                        }

                        mdt1.Dispose();

                        // mdt is table which is having Bom in Expended Form
                        dro1 = fmdt.NewRow();
                        dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                        dro1["val"] = mq0;
                        dro1["c_cost"] = mq10;
                        dro1["wt_cnc"] = mq3;
                        dro1["wt_rft"] = mq4;
                        fmdt.Rows.Add(dro1);
                        // fmdt is table which is only having Parant Bom icode and Value                        
                    }
                    {
                        #region Costing Report
                        dro = null;
                        SQuery = "Select distinct a.icode,b.iname as product,b.cpartno,b.unit,a.qty as val,a.start2 as job_Val,(a.qty+a.start2) as tot_value from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        if (co_cd == "DREM") SQuery = "Select distinct a.icode as erpcode,b.iname as product,b.cpartno,b.unit,a.close1 as lot_size,(a.close1 * a.qty) as matl_cost_amt,a.qty as per_pcs_cost,(a.close1 * a.start2) as process_cost_amt,a.start2 as per_pcs_proc_value,(a.close1 * a.start2) + (a.close1 * a.qty) as total_cost,(a.qty+a.start2) as per_pcs_tot_cost from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        header_n = "";
                        #region FG Valuation on BOM Cost
                        if (frm_formID == "F10184*")
                        {
                            string CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                            string CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
                            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT1 + "','dd/mm/yyyy')-1";

                            string _yr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");

                            xprdrange = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                            branch_Cd = "BRANCHCD='" + mbr + "'";

                            mq0 = "Select Closing_Stk,erpcode from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " and substr(icode,1,1)='9' union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' and substr(icode,1,1)='9' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";

                            string grossWt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR11");
                            if (grossWt == "0")
                            {
                                grossWt = "wt_Rft";
                                header_n = "[Gross Weight]";
                            }
                            else
                            {
                                grossWt = "wt_cnc";
                                header_n = "[Net Weight]";
                            }
                            SQuery = "SELECT DISTINCT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,A.QTY AS BOM_COST,C.Closing_Stk,ROUND(C.CLOSING_STK*A.QTY,3) AS FG_STK_VALUE from EXTRUSION A,ITEM B, (" + mq0 + ") C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.erpcode) AND A.BRANCHCD='" + mbr + "' AND A.ENT_BY='" + uname + "' and C.Closing_Stk!=0 ORDER BY A.ICODE ";
                            SQuery = "SELECT DISTINCT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,A.QTY AS BOM_COST,C.Closing_Stk,round(a.qty * c.closing_stk,3) as closing_value,b.wt_rft as gross_wt,b.wt_cnc as net_wt,ROUND(C.CLOSING_STK*b." + grossWt + ",3) AS STK_Wt,round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) as conv_value,ROUND(round(a.qty * c.closing_stk,3) + round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) ,3 ) as stk_value_with_conv from EXTRUSION A,ITEM B, (" + mq0 + ") C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.erpcode) AND A.BRANCHCD='" + mbr + "' AND A.ENT_BY='" + uname + "' and C.Closing_Stk!=0 " + mq5;
                        }
                        if (frm_formID == "F10184C")
                        {
                            cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                            cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

                            //string _yr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");                            

                            r10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
                            cond = "";
                            if (r10.Length > 2) cond = " and vchdate>=to_Date('" + r10 + "','dd/mm/yyyy')";
                            if (frm_formID == "F10184C") cond = "";
                            xprd1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT1 + "','dd/mm/yyyy')-1";
                            xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1" + cond;
                            xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')" + cond;
                            branch_Cd = "BRANCHCD='" + mbr + "'";

                            if (frm_formID == "F10184C")
                                xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1" + cond;

                            //vipin
                            //col1 = fgen.seek_iname(frm_qstr, co_cd, "select vchdate,icode as stg,qty1 from multivch where branchcd='" + mbr + "' and type='PX' and trim(icode)='" + hfcode.Value + "'", "qty1");
                            string grossWt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR11");
                            if (grossWt == "0")
                            {
                                grossWt = "wt_Rft";
                                header_n = "[Gross Weight]";
                            }
                            else
                            {
                                grossWt = "wt_cnc";
                                header_n = "[Net Weight]";
                            }

                            mq3 = "select B.Iname as Item_Name,trim(a.Icode) as Erp_Code,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing,to_Char((sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))),'999999999.99') as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate " + xprdrange + "  and stage='" + hfcode.Value + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprd1 + " and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprd1 + " and type!='XX' and stage='" + hfcode.Value + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + hfcode.Value + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='XX' or stage='" + hfcode.Value + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)) having sum(opening)+sum(cdr)+sum(ccr)<>0  Order by substr(a.icode,1,4),B.iname";

                            mq0 = @"select B.Iname as Item_Name,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='" + mbr + "' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='" + mbr + "' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate " + xprdrange + " and stage='" + hfcode.Value + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprdrange1 + " and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and type!='XX' and stage='" + mbr + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + hfcode.Value + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='XX' or stage='" + hfcode.Value + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)) having sum(opening)+sum(cdr)+sum(ccr)<>0  ";

                            //mq5 = "UNION ALL SELECT ERPCODE,ITEM_NAME,CPARTNO,UNIT,RATES AS BOM_COST,Closing_Stk,WIP_VALUE,0 AS STK_WT,0 AS CONV_COST,0 AS CONV_VALUE,0 AS V1 FROM (" + mq0 + ") WHERE ERPCODE NOT IN (SELECT DISTINCT ICODE FROM EXTRUSION WHERE BRANCHCD='" + mbr + "' AND ENT_BY='" + uname + "') and Closing_Stk!=0 ";

                            SQuery = "SELECT DISTINCT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,A.QTY AS BOM_COST,C.Closing_Stk,round(a.qty * c.closing_stk,3) as closing_value,ROUND(C.CLOSING_STK*b." + grossWt + ",3) AS STK_Wt,round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) as conv_value,ROUND(round(a.qty * c.closing_stk,3) + round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) ,3 ) as stk_value_with_conv from EXTRUSION A,ITEM B, (" + mq0 + ") C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.erpcode) AND A.BRANCHCD='" + mbr + "' AND A.ENT_BY='" + uname + "' AND SUBSTR(A.ICODE,1,1)='9' and C.Closing_Stk!=0 " + mq5;
                            dt = new DataTable();
                            #region
                            dt.Columns.Add("Section", typeof(string));
                            dt.Columns.Add("Erpcode", typeof(string));
                            dt.Columns.Add("Product_Name", typeof(string));
                            dt.Columns.Add("Cpartno", typeof(string));
                            dt.Columns.Add("Gross_Wt", typeof(string));
                            dt.Columns.Add("Net_wt", typeof(string));
                            dt.Columns.Add("Gross_wt_pl", typeof(string));
                            dt.Columns.Add("RM_Cost", typeof(string));
                            dt.Columns.Add("Cost_with_ProcessLoss", typeof(string));
                            dt.Columns.Add("Unit", typeof(string));
                            dt.Columns.Add("Closing_stk", typeof(string));
                            dt.Columns.Add("Closing_value", typeof(string));
                            dt.Columns.Add("Stk_wt", typeof(string));
                            dt.Columns.Add("Conv_cost", typeof(string));
                            dt.Columns.Add("Conv_values", typeof(string));
                            //dt.Columns.Add("Conv_value_on_net", typeof(string));
                            dt.Columns.Add("Stock_value_with_conv", typeof(string));
                            dt.Columns.Add("ChildCode", typeof(string));
                            dt.Columns.Add("ChildName", typeof(string));
                            #endregion
                            DataRow dr;
                            dt1 = new DataTable();
                            if (frm_formID == "F10184C")
                            {
                                dt1.Columns.Add("stagename", typeof(string));
                                dt1.Columns.Add("code", typeof(string));

                                DataRow drr;
                                drr = dt1.NewRow();
                                drr["stagename"] = "FG Stock Valuation Report";
                                drr["code"] = "FG";
                                dt1.Rows.Add(drr);
                            }
                            else
                                dt1 = fgen.getdata(frm_qstr, co_cd, "select name as stagename,acref as code,acref from typeGRP where BRANCHCD='" + mbr + "' and ID='WI' and acref like '6%' order by type1");

                            DataTable dTpurRate = (DataTable)ViewState["dt2" + frm_formID];
                            DataTable dtSaleRate = new DataTable();
                            SQuery = "Select distinct ROUND((case when a.irate>0 then A.irate else b.irate end)-(CASE WHEN A.ICHGS>0 THEN ROUND(A.IRATE * (A.ICHGS/100),2) ELSE 0 END ),2) as rate,TRIM(a.icode) AS icode,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type like '4%' AND A.VCHDATE " + xprdrange + " and a.icode like '9%'  order by vdd desc,TRIM(a.icode)";
                            dtSaleRate = fgen.getdata(frm_qstr, co_cd, SQuery);

                            foreach (DataRow stages in dt1.Rows)
                            {
                                dt2 = new DataTable();
                                if (frm_formID == "F10184C")
                                {
                                    mq0 = "Select Closing_Stk,erpcode from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " and substr(icode,1,1)='9' union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' and substr(icode,1,1) in ('9','7') GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";
                                    mq0 = "Select icode as erpcode,Closing_Stk from (select a.icode,sum(a.opening) as opb,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange1 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr from IVOUCHER where branchcd='" + mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange + " GROUP BY trim(icode) ,branchcd) a GROUP BY a.icode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)>0 )";

                                    mq0 = "SELECT A.*,B.CPARTNO,B.UNIT,B.IRATE AS RATES,b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname FROM (" + mq0 + ") A,ITEM B WHERE TRIM(A.ERPCODE)=TRIM(b.ICODE) AND SUBSTR(A.ERPCODE,1,1)='9' ";
                                }
                                else
                                {
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='" + mbr + "' and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='" + mbr + "' and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate " + xprdrange + " and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprdrange1 + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and type!='XX' and stage='" + mbr + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc having sum(a.opening)+sum(a.cDr)-sum(a.cCr)<>0  order by trim(a.Icode) ";
                                    // type added only 02 and 07 AND REMOVED 30 , 40 , 50 SERIES ITEM CODE
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='" + mbr + "' and (type in ('02','07') or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='" + mbr + "' and (type in ('02','07') or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate " + xprdrange + " and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprdrange1 + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and type!='XX' and stage='" + mbr + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' AND SUBSTR(A.ICODE,1,2) NOT IN ('30','40','50') group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(a.opening)+sum(a.cDr)-sum(a.cCr)<>0  order by trim(a.Icode) ";
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='" + mbr + "' and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='" + mbr + "' and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0  Order by substr(a.icode,1,4),B.iname";
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='" + mbr + "' and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='" + mbr + "' and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='" + mbr + "' and  type='50' and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='" + mbr + "' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='" + mbr + "' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0  Order by substr(a.icode,1,4),B.iname";

                                    //mq0 = "select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='04' and  type='50' and vchdate  between to_Date('23/12/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')  and stage='63' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and vchdate  between to_date('01/03/2019','dd/mm/yyyy') and to_Date('31/03/2019','dd/mm/yyyy') AND VCHDATE>=TO_DATE('23/12/2018','dd/mm/yyyy') and type!='XX' and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate  between to_date('01/03/2019','dd/mm/yyyy') and to_Date('31/03/2019','dd/mm/yyyy') AND VCHDATE>=TO_DATE('23/12/2018','dd/mm/yyyy') and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='04' and (trim(acode)='XX' or stage='63') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='04' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0 and trim(a.icode)='72070022' Order by substr(a.icode,1,4),B.iname";
                                    //mq0 = "select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='04' and  type='50' and vchdate  between to_Date('23/12/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')  and stage='63' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and vchdate between to_Date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy') and vchdate>=to_Date('23/12/2018','dd/mm/yyyy') and type!='XX' and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate between to_Date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy') and vchdate>=to_Date('23/12/2018','dd/mm/yyyy') and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='04' and (trim(acode)='XX' or stage='63') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='04' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0 and trim(a.icode)='72070022' Order by substr(a.icode,1,4),B.iname";
                                }
                                dt2 = fgen.getdata(frm_qstr, co_cd, mq0);
                                CSR = "0";
                                double alloyrate = 0;
                                DataTable newDt2 = (DataTable)ViewState["dt2" + frm_formID];
                                foreach (DataRow dr2 in dt2.Rows)
                                {
                                    dr = dt.NewRow();
                                    dr["section"] = "Stage : " + stages["stagename"].ToString().Trim() + " [" + stages["code"].ToString().Trim() + "]";
                                    dr["erpcode"] = dr2["erpcode"].ToString().Trim();
                                    dr["Product_Name"] = dr2["item_name"].ToString().Trim();
                                    dr["cpartno"] = dr2["cpartno"].ToString().Trim();
                                    dr["unit"] = dr2["unit"].ToString().Trim();
                                    dr["Gross_Wt"] = dr2["wt_Rft"].ToString().Trim();
                                    dr["net_wt"] = dr2["wt_cnc"].ToString().Trim();
                                    dr["gross_wt_pl"] = Math.Round(dr2["wt_Rft"].ToString().Trim().toDouble() / .94, 3);
                                    col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "val");
                                    if (col1 != "0") dr["RM_Cost"] = col1.toDouble(5);
                                    else dr["RM_Cost"] = dr2["rates"].ToString().Trim();

                                    if (co_cd == "SAGM" && (dr2["erpcode"].ToString().Trim().Substring(0, 2) == "10" || dr2["erpcode"].ToString().Trim().Substring(0, 2) == "20" || dr2["erpcode"].ToString().Trim().Substring(0, 2) == "30" || dr2["erpcode"].ToString().Trim().Substring(0, 2) == "40"))
                                    {
                                        alloyrate = 0;
                                        if (newDt2.Rows.Count > 0)
                                        {
                                            sort_view = new DataView(newDt2, "trim(icode)='" + dr2["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                            if (sort_view.Count > 0)
                                            {
                                                if (dr2["mat4"].ToString().Trim().toDouble() > 0)
                                                    alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (dr2["mat4"].ToString().Trim().toDouble() / 100);
                                                else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                dr["RM_Cost"] = alloyrate;
                                            }
                                            else
                                            {
                                                sort_view = new DataView(newDt2, "trim(icode)='" + dr2["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                if (sort_view.Count > 0)
                                                {
                                                    if (dr2["mat4"].ToString().Trim().toDouble() > 0)
                                                        alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (dr2["mat4"].ToString().Trim().toDouble() / 100);
                                                    else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                    dr["RM_Cost"] = alloyrate;
                                                }
                                            }
                                        }
                                    }

                                    if (dr["RM_Cost"].ToString().toDouble() <= 0)
                                    {
                                        if (dr2["erpcode"].ToString().Trim().Substring(0, 1) == "9")
                                        {
                                            dr["RM_Cost"] = fgen.seek_iname_dt(dtSaleRate, "ICODE='" + dr2["erpcode"].ToString().Trim() + "'", "RATE");
                                        }
                                        else dr["RM_Cost"] = fgen.seek_iname_dt(dTpurRate, "ICODE='" + dr2["erpcode"].ToString().Trim() + "'", "RATE");
                                    }

                                    col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "c_cost");
                                    dr["conv_cost"] = col1;

                                    //dr["Cost_with_ProcessLoss"] = Math.Round(dr["cost"].ToString().toDouble() + (dr["gross_wt_pl"].ToString().toDouble() * (dr["cost"].ToString().toDouble())), 3);
                                    if (dr["erpcode"].ToString().Trim().Substring(0, 2) == "10" || dr["erpcode"].ToString().Trim().Substring(0, 2) == "20" || dr["erpcode"].ToString().Trim().Substring(0, 2) == "30" || dr["erpcode"].ToString().Trim().Substring(0, 2) == "40")
                                        dr["Cost_with_ProcessLoss"] = "0";
                                    else dr["Cost_with_ProcessLoss"] = Math.Round(dr["RM_Cost"].ToString().toDouble() / .94, 2);
                                    dr["closing_stk"] = dr2["closing_stk"].ToString().Trim();
                                    dr["closing_value"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * dr["Cost_with_ProcessLoss"].ToString().Trim().toDouble(), 3);

                                    dr["stk_wt"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * 1, 3);
                                    dr["Conv_values"] = Math.Round(dr["stk_wt"].ToString().toDouble() * col1.toDouble(), 3);

                                    //dr["Conv_value_on_gross"] = Math.Round(dr["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "wt_rft").toDouble(), 2);
                                    //dr["Conv_value_on_net"] = Math.Round(dr["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "wt_rft").toDouble(), 2);

                                    dr["stock_value_with_conv"] = Math.Round(dr["closing_value"].ToString().toDouble() + dr["Conv_values"].ToString().toDouble(), 2);

                                    dr["childCode"] = dr2["wip_code"].ToString().Trim();
                                    dr["childName"] = dr2["siname"].ToString().Trim();

                                    dt.Rows.Add(dr);
                                    CSR = "1";
                                }
                                if (CSR == "1")
                                {
                                    dr = dt.NewRow();
                                    dr[0] = "";
                                    dt.Rows.Add(dr);
                                }
                            }
                            SQuery = "";
                            if (dt.Rows.Count > 0)
                            {
                                dt.Columns.Remove("conv_cost");
                                dt.Columns.Remove("Net_wt");
                                dt.Columns.Remove("Stk_wt");
                                dt.Columns.Remove("gross_wt_pl");
                                //dt.Columns.Remove("Gross_Wt_Per_Pc");
                                dt.Columns.Remove("Cost_with_ProcessLoss");
                                dt.Columns.Remove("ChildCode");
                                dt.Columns.Remove("ChildName");
                            }
                            Session["send_dt"] = dt;
                        }
                        #endregion
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Cost Sheet of Products for the period " + fromdt + " and " + todt + "", frm_qstr);
                        #endregion
                    }
                    break;

                case "F10183":
                case "F10184":
                case "F10186":
                case "F10194":
                case "F10194E":
                case "F10194F":
                case "F10198":
                case "F10198W":
                case "F05125E":
                case "F05125D":
                    branchWiseBM = "";

                    //*****************************mark it Y if company has branch wise BOM
                    if (co_cd == "SAGM") branchWiseBM = "Y";
                    branch_Cd = "BRANCHCD NOT IN ('DD','88')";
                    if (branchWiseBM == "Y") branch_Cd = "branchcd='" + mbr + "'";
                    //*****************************

                    #region checking Cyclical BOM
                    SQuery = "select branchcd||'-'||trim(icode)||'-'||trim(ibcode) as bom_link,branchcd,type,vchnum,vchdate,ent_by,ent_dt,edt_by,edt_dt from itemosp where branchcd!='DD' and branchcd||'-'||trim(icode)||'-'||trim(ibcode) in (Select branchcd||'-'||trim(ibcode)||'-'||trim(icode) from itemosp where branchcd!='DD')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Cyclical Bom With   (parent -> child -> parent", frm_qstr);

                        return;
                    }

                    SQuery = "select branchcd||'-'||trim(ibcode)||'-'||trim(icode) as bom_link,branchcd,type,vchnum,vchdate,ent_by,ent_dt,edt_by,edt_dt from itemosp where branchcd!='DD' and branchcd||'-'||trim(ibcode)||'-'||trim(icode) in (Select branchcd||'-'||trim(icode)||'-'||trim(ibcode) from itemosp where branchcd!='DD')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Cyclical Bom With   (parent -> child -> parent", frm_qstr);

                        return;
                    }

                    SQuery = "select B.INAME ,B.cdrgno,A.vchnum,A.vchdate,a.icode,count(vchnum) as lines from itemosp A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and trim(A.icode)=trim(a.ibcode) AND A.type='BM' and A.branchcd='" + mbr + "' and A.vchnum<>'000000' group by B.INAME ,B.cdrgno,A.vchnum,A.vchdate,a.icode order by A.vchdate desc ,A.vchnum desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("Cyclical Bom With   (parent -> child -> parent", frm_qstr);

                        return;
                    }
                    #endregion

                    akmcode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1");
                    aksubcode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2");
                    akicode1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
                    akicode2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");
                    cond = " like '%'";
                    if (akmcode.ToString().Trim() != "") cond = " like '" + akmcode.ToString().Trim() + "%'";
                    if (aksubcode.ToString().Trim() != "") cond = " like '" + aksubcode.ToString().Trim() + "%'";
                    if (akicode1.ToString().Trim() != "") { cond = " ='" + akicode1.ToString().Trim() + "' "; }
                    if (akicode2.ToString().Trim() != "") { cond = " between '" + akicode1.ToString().Trim() + "' and '" + akicode2.ToString().Trim() + "'"; }

                    fgen.execute_cmd(frm_qstr, co_cd, "delete from extrusion where branchcd='" + mbr + "' and type='EX' AND TRIM(ENT_BY)='" + uname + "'");

                    mdt = new DataTable(); dt3 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); mdt1 = new DataTable(); vdt = new DataTable();

                    if (HCID == "29155" || HCID == "29155a" || HCID == "A2")
                    {
                        if (akmcode.ToString().Trim() == "-" && aksubcode.ToString().Trim() == "-" && akicode1.ToString().Trim() == "-" && akicode2.ToString().Trim() == "-")
                        {
                            SQuery = "Select min(icode) as icode1,max(icode) as icode2 from ivoucher where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and icode like '9%' ";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["icode1"].ToString().Trim().Length > 1)
                                    cond = " between '" + dt.Rows[0]["icode1"].ToString().Trim() + "' and '" + dt.Rows[0]["icode2"].ToString().Trim() + "' ";
                            }
                            dt.Dispose();
                        }
                    }
                    if ((HCID == "F10184" || HCID == "F10186" || HCID == "F10194" || HCID == "F10194E" || HCID == "F10194F" || HCID == "F10198" || HCID == "F10198W") && akmcode.ToString().Trim() == "-" && aksubcode.ToString().Trim() == "-" && akicode1.ToString().Trim() == "-" && akicode2.ToString().Trim() == "-") cond = " like '9%'";


                    //*****************************Filling Selected BOM

                    sp_cond = "";
                    if (co_cd == "SAGM")
                        sp_cond = "and substr(a.ibcode,1,2) not in ('30') and trim(a.ibcode)!='10030062'";

                    SQuery = "Select A.SRNO,a.branchcd,a.vchnum,a.vchdate,a.icode,a.ibcode,a.ibqty,a.main_issue_no,a.sub_issue_no,a.IBDIEPC,a.ibwt,a.br_stg,(case when is_number(f.birate)>0 then f.birate when B.IQD>0 then B.IQD else B.irate end) as itrate,b.iname as itemname,c.iname as piname,b.alloy from itemosp a,item b left outer join (select TRIM(icode) as icode,birate from itembal where branchcd='" + mbr + "' ) f on trim(b.icode)=trim(f.icodE),item c where trim(a.ibcode)=trim(b.icode) and trim(A.icode)=trim(C.icodE) and trim(a.icode) " + cond + " AND a." + branch_Cd + " and substr(a.icode,1,1)>='7' " + sp_cond + " order by a.srno,a.icode";
                    if (co_cd == "SAGM" && mbr == "06")
                        SQuery = "Select A.SRNO,a.branchcd,a.vchnum,a.vchdate,a.icode,a.ibcode,a.ibqty,a.main_issue_no,a.sub_issue_no,a.IBDIEPC,a.ibwt,a.br_stg,(case when is_number(f.birate)>0 then f.birate when B.IQD>0 then B.IQD else B.irate end) as itrate,b.iname as itemname,c.iname as piname,b.alloy from itemosp a,item b left outer join (select TRIM(icode) as icode,birate from itembal where branchcd='" + mbr + "' ) f on trim(b.icode)=trim(f.icodE),item c where trim(a.ibcode)=trim(b.icode) and trim(A.icode)=trim(C.icodE) and trim(a.icode) " + cond + " AND a.BRANCHCD='08' and substr(a.icode,1,1)>='7' " + sp_cond + " order by a.srno,a.icode";
                    dt3 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //*****************************


                    //*****************************Table to Fill Layered BOM, With Value
                    mdt.Columns.Add(new DataColumn("branchcd", typeof(string)));
                    mdt.Columns.Add(new DataColumn("lvl", typeof(string)));
                    mdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibnetwt", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibqty", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
                    mdt.Columns.Add(new DataColumn("alloy", typeof(string)));
                    mdt.Columns.Add(new DataColumn("irate", typeof(string)));
                    mdt.Columns.Add(new DataColumn("val_nt", typeof(string)));
                    mdt.Columns.Add(new DataColumn("val", typeof(string)));
                    mdt.Columns.Add(new DataColumn("c_cost", typeof(string)));

                    mdt.Columns.Add(new DataColumn("stg_Wt", typeof(string)));

                    mdt.Columns.Add(new DataColumn("iname", typeof(string)));
                    mdt.Columns.Add(new DataColumn("ibname", typeof(string)));
                    mdt.Columns.Add(new DataColumn("jr", typeof(string)));
                    mdt.Columns.Add(new DataColumn("IBDIEPC", typeof(string)));
                    mdt.Columns.Add(new DataColumn("SUB_ISSUE_NO", typeof(string)));
                    mdt.Columns.Add(new DataColumn("wt_cnc", typeof(string)));
                    mdt.Columns.Add(new DataColumn("wt_rft", typeof(string)));
                    mdt.Columns.Add(new DataColumn("gr_wt", typeof(string)));
                    mdt.Columns.Add(new DataColumn("nt_wt", typeof(string)));
                    //*****************************

                    //*****************************Table to Fill Finish Good BOM, With Value
                    fmdt = new DataTable();
                    fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("val", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("val_nt", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("JO_Val", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("srate", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("sqty", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("acode", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("lot_size", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("c_cost", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("wt_cnc", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("wt_rft", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("gr_wt", typeof(string)));
                    fmdt.Columns.Add(new DataColumn("nt_wt", typeof(string)));
                    //*****************************

                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR9") == "Y") rmcBranch = "BRANCHCD NOT IN ('DD','88')";
                    else rmcBranch = "BRANCHCD='" + mbr + "'";

                    //*****************************Filling All BOM's                    
                    sp_cond = "";
                    if (co_cd == "SAGM")
                        sp_cond = "and substr(a.ibcode,1,2) not in ('30') and trim(a.ibcode)!='10030062'";
                    if (ViewState["vdt"] == null)
                    {
                        SQuery = "Select A.SRNO,a.branchcd,a.vchnum,a.vchdate,a.icode,a.ibcode,a.ibqty,a.main_issue_no,a.sub_issue_no,a.IBDIEPC,a.ibwt,a.br_stg,(case when is_number(c.birate)>0 then c.birate when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,b.alloy  from itemosp a,item b left outer join (select TRIM(icode) as icode,birate from itembal where branchcd='" + mbr + "' ) c on trim(b.icode)=trim(c.icodE) where trim(a.ibcode)=trim(b.icode) AND a." + branch_Cd + " " + sp_cond + " order by a.srno,a.icode,a.ibcode";
                        if (co_cd == "KCLG") SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.alloy from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD = ('02') order by a.srno,a.icode,a.ibcode";
                        if (co_cd == "SAGM" && mbr == "06")
                            SQuery = "Select A.SRNO,a.branchcd,a.vchnum,a.vchdate,a.icode,a.ibcode,a.ibqty,a.main_issue_no,a.sub_issue_no,a.IBDIEPC,a.ibwt,a.br_stg,(case when is_number(c.birate)>0 then c.birate when B.IQD>0 then B.IQD else B.irate end) as bchrate,b.iname as itemname,b.alloy  from itemosp a,item b left outer join (select TRIM(icode) as icode,birate from itembal where branchcd='" + mbr + "' ) c on trim(b.icode)=trim(c.icodE) where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD='08' " + sp_cond + " order by a.srno,a.icode,a.ibcode";
                        vdt = fgen.getdata(frm_qstr, co_cd, SQuery); v = 0;
                        ViewState["vdt"] = vdt;
                    }
                    else vdt = (DataTable)ViewState["vdt"];
                    //*****************************


                    //*****************************Filling MRR for last 500 Days
                    dt2 = new DataTable();
                    rateCond = " type like '0%' ";
                    string finvno = "and trim(nvl(finvno,'-'))!='-'";
                    if (ViewState["dt2" + frm_formID] == null)
                    {
                        if (co_cd == "SAGM")
                        {
                            rateCond = "type in ('02','07','0U')";
                            finvno = "";
                        }
                        if (frm_formID != "F10194F")
                        {
                            SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where " + rmcBranch + " and " + rateCond + " " + finvno + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY')  /*and icode like '9%'*/ order by icode,vdd desc";
                            SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,trim(acode) as acode,trim(vchnum) as vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,to_Char(vchdate,'yyyymmdd') as vdd,TYPE from ivoucher where " + rmcBranch + " and " + rateCond + " and vchdate>=(to_date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_date('" + todt + "','dd/mm/yyyy') /*and icode like '9%'*/ order by icode,vdd desc";
                            if (co_cd == "BUPL") SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where " + rmcBranch + " and type in ('02','05','07') " + finvno + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and icode like '9%' order by icode,vdd desc";
                            //wtd avg rate
                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR10") == "N")
                            {
                                SQuery = "Select trim(icode) as icode,round((sum(is_number(iqty_chl)*is_number(ichgs)) / sum(is_number(iqty_chl))) ,3) as rate,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " " + finvno + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and is_number(substr(icode,1,1))<7 /*and icode like '9%'*/ group by trim(icode) order by icode";
                                SQuery = "select branchcd,icode, round((case when sum(iqty_chl * ichgs)>0 then (sum(iqty_chl * ichgs) / sum(iqty_chl)) else 0 end),3) as rate,acode,vchnum,vchdate,type,vdd from (Select '-' as branchcd,trim(icode) as icode,iqty_chl,ICHGS,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " " + finvno + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and substr(icode,1,1)<'7' union all Select '-' as branchcd,trim(A.icode) as icode,A.YR_" + year + ",b.irate,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ITEMBAL A,ITEM B WHERE A." + rmcBranch + " and TRIM(A.ICODE)=TRIM(b.ICODE) AND substr(A.icode,1,1)<'7' ) group by branchcd,icode,acode,vchnum,vchdate,type,vdd";
                                if (co_cd == "SAGM")
                                    SQuery = "select branchcd,icode, round((case when sum(iqty_chl * ichgs)>0 then (sum(iqty_chl * ichgs) / sum(iqty_chl)) else 0 end),3) as rate,acode,vchnum,vchdate,type,vdd from (Select '-' as branchcd,trim(icode) as icode,iqty_chl,ICHGS,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ivoucher where " + rmcBranch + " and " + rateCond + " " + finvno + " and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500)  and vchdate<=to_Date('" + todt + "','DD/MM/YYYY') and substr(icode,1,1)<'7' union all Select '-' as branchcd,trim(A.icode) as icode,A.YR_2019,b.irate,'-' as acode,'-' as vchnum,'-' as vchdate,'-' as type,1 AS VDD from ITEMBAL A,ITEM B WHERE A." + rmcBranch + " and TRIM(A.ICODE)=TRIM(b.ICODE) AND substr(A.icode,1,1)<'7' ) group by branchcd,icode,acode,vchnum,vchdate,type,vdd";
                            }
                            if (co_cd != "KCLG") dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                        }
                        else dt2 = Gen_stk_pl_S(todt, "");
                        ViewState["dt2" + frm_formID] = dt2;
                    }
                    else dt2 = (DataTable)ViewState["dt2" + frm_formID];
                    //*****************************


                    //*****************************Filling PO's to find out Job Work Value (done for NIRM only)
                    dtPo = new DataTable();
                    SQuery = "SELECT distinct TRIM(ICODe) AS ICODE,PRATE,TO_CHAR(ORDDT,'YYYYMMDD') AS VDD FROM POMAS WHERE BRANCHCD='" + mbr + "' and type='53' and orddt>=(sysdate-500) and icode like '7%' /* and orddt " + xprdrange + "*/ order by vdd desc ";
                    if (co_cd == "NIRM") dtPo = fgen.getdata(frm_qstr, co_cd, SQuery);
                    //*****************************

                    //*****************************Making Distinct ICODE from Main BOM Table
                    dist1_view = new DataView(dt3);
                    dt_dist = new DataTable();
                    if (dist1_view.Count > 0)
                    {
                        dist1_view.Sort = "icode";
                        dt_dist = dist1_view.ToTable(true, "icode");
                    }
                    //*****************************

                    //*****************************Filling Itemospanx for DREM
                    bomanx = new DataTable();
                    if (co_cd == "DREM")
                    {
                        SQuery = "Select c.iname,b.name as names,a.* from itemospanx a,type b,item c  where b.id='1' and trim(a.icode)=trim(c.icode) and trim(a.stg_Cd)=trim(b.type1) and a.branchcd!='DD'";
                        bomanx = fgen.getdata(frm_qstr, co_cd, SQuery);
                    }

                    dtItemBal = new DataTable();
                    dtItemBal = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,BSTGCD FROM ITEMBAL WHERE BRANCHCD='" + mbr + "' AND SUBSTR(ICODE,1,1) in ('7','9') ORDER BY ICODE");

                    dtMulvch = new DataTable();
                    dtMulvch = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,qty1 FROM MULTIVCH WHERE BRANCHCD='" + mbr + "' AND TYPE='PX' ");

                    //*****************************

                    mainLotSize = 0;
                    topickicode = "";
                    string calcBOM = "Y";
                    if (co_cd == "SAGM")
                    {
                        if (Session["mdt" + mbr] != null)
                        {
                            mdt = (DataTable)Session["mdt" + mbr];
                            fmdt = (DataTable)Session["fmdt" + mbr];
                            calcBOM = "N";
                        }
                    }
                    if (calcBOM == "Y")
                    {
                        foreach (DataRow dt_dist_row in dt_dist.Rows)
                        {
                            mdt1 = new DataTable();
                            mdt1 = mdt.Clone();
                            mvdview = new DataView(dt3, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
                            dt = new DataTable();
                            mvdview.Sort = "srno,icode";
                            dt = mvdview.ToTable();

                            #region filling parent
                            //*****************************
                            foreach (DataRow drc in dt.Rows)
                            {
                                double cVa = 0;
                                dro = mdt1.NewRow();
                                dro["lvl"] = "1";
                                dro["branchcd"] = drc["branchcd"].ToString().Trim();
                                dro["icode"] = drc["icode"].ToString().Trim();
                                dro["iname"] = drc["piname"].ToString().Trim();
                                dro["ibname"] = drc["itemname"].ToString().Trim();
                                dro["pcode"] = drc["icode"].ToString().Trim();
                                dro["alloy"] = drc["alloy"].ToString().Trim();
                                mainLotSize = fgen.make_double(drc["main_issue_no"].ToString().Trim());
                                if (mainLotSize <= 0) mainLotSize = 1;
                                dro["ibnetwt"] = fgen.make_double(drc["ibdiepc"].ToString()) / mainLotSize;
                                dro["ibqty"] = fgen.make_double(drc["ibqty"].ToString()) / mainLotSize;
                                dro["ibcode"] = drc["ibcode"].ToString().Trim();
                                dro["irate"] = drc["itrate"].ToString().Trim();
                                if (co_cd == "NIRM" && dtPo != null)
                                {
                                    dro["jr"] = cVa;
                                }
                                //col1 = fgen.seek_iname_dt(dtItemBal, "ICODE='" + drc["icode"].ToString().Trim() + "'", "BSTGCD");
                                //if (col1 != "0")
                                //{
                                //    col1 = fgen.seek_iname_dt(dtMulvch, "ICODE='" + col1 + "'", "QTY1");
                                //    if (col1 != "0")
                                //        dro["stg_Wt"] = col1;
                                //}                            
                                if (dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7") topickicode = "icode";
                                else topickicode = "ibcode";

                                topickicode = "icode";
                                col1 = fgen.seek_iname_dt(dtItemBal, "ICODE='" + drc[topickicode].ToString().Trim() + "'", "BSTGCD");
                                if (col1 != "0")
                                {
                                    col1 = fgen.seek_iname_dt(dtMulvch, "ICODE='" + col1 + "'", "QTY1");
                                    if (col1 != "0")
                                        dro["stg_Wt"] = dro["stg_Wt"].ToString().toDouble() + col1.ToString().toDouble();
                                }

                                if (drc["ibcode"].ToString().Trim().Substring(0, 2) == "30")
                                {
                                    dro["stg_Wt"] = 0;
                                }

                                dro["SUB_ISSUE_NO"] = drc["SUB_ISSUE_NO"].ToString();
                                dro["IBDIEPC"] = drc["IBDIEPC"].ToString();
                                //if (drc["ibcode"].ToString().Trim().Substring(0, 1) == "7")
                                //{
                                //    dro["wt_cnc"] = drc["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                //    dro["wt_rft"] = drc["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                //}
                                //if (dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7" && drc["ibcode"].ToString().Trim().Substring(0, 2) == "10")
                                //{
                                //    dro["wt_cnc"] = drc["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                //    dro["wt_rft"] = drc["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                //}
                                dro["wt_cnc"] = 0;
                                dro["wt_rft"] = 0;
                                if ((dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "9" || dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7") && (drc["ibcode"].ToString().Trim().Substring(0, 1) == "1" || drc["ibcode"].ToString().Trim().Substring(0, 1) == "7"))
                                {
                                    dro["wt_cnc"] = drc["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                    dro["wt_rft"] = drc["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                }
                                else
                                    dro["stg_Wt"] = 0;

                                dro["val"] = "0";
                                dro["val_nt"] = "0";
                                mdt1.Rows.Add(dro);
                            }
                            //*****************************
                            #endregion

                            #region filling Child with Recursive LOOP
                            //*****************************
                            i0 = 1; v = 0;
                            for (int i = v; i < mdt1.Rows.Count; i++)
                            {
                                //vipin
                                vdview = new DataView(vdt, "icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                                if (vdview.Count > 0)
                                {
                                    vdview1 = new DataView(mdt1, "icode='" + mdt1.Rows[i]["icode"].ToString().Trim() + "' and ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "' and ibqty='" + mdt1.Rows[i]["ibqty"] + "'", "ibcode", DataViewRowState.CurrentRows);
                                    if (vdview1.Count <= 0) vdview1 = new DataView(mdt1, "ibcode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "ibcode", DataViewRowState.CurrentRows);

                                    for (int x = 0; x < vdview.Count; x++)
                                    {
                                        if (mq0 != vdview[x].Row["icode"].ToString().Trim())
                                        {
                                            value3 = fgen.seek_iname_dt(mdt1, "IBCODE='" + vdview[x].Row["icode"].ToString().Trim() + "'", "LVL");
                                            if (value3 == "0")
                                                i0 += 1;
                                            else i0 = fgen.make_int(value3) + 1;
                                        }
                                        dro = mdt1.NewRow();
                                        dro["lvl"] = i0.ToString();
                                        dro["icode"] = vdview[x].Row["icode"].ToString().Trim();
                                        dro["branchcd"] = vdview[x].Row["branchcd"].ToString().Trim();
                                        mq0 = vdview[x].Row["icode"].ToString().Trim();
                                        double lotSize = fgen.make_double(vdview[x].Row["MAIN_ISSUE_NO"].ToString().Trim());
                                        if (lotSize <= 0) lotSize = 1;
                                        dro["ibnetwt"] = (Convert.ToDouble(vdview[x].Row["ibdiepc"]) * (Convert.ToDouble(vdview1[0].Row["ibdiepc"]) / lotSize)).ToString();
                                        dro["ibqty"] = (Convert.ToDouble(vdview[x].Row["ibqty"]) * (Convert.ToDouble(vdview1[0].Row["ibqty"]) / lotSize)).ToString();

                                        dro["ibcode"] = vdview[x].Row["ibcode"].ToString().Trim();
                                        dro["alloy"] = vdview[x].Row["alloy"].ToString().Trim();

                                        if (dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7") topickicode = "icode";
                                        else topickicode = "ibcode";

                                        topickicode = "icode";

                                        col1 = fgen.seek_iname_dt(dtItemBal, "ICODE='" + vdview[x].Row[topickicode].ToString().Trim() + "'", "BSTGCD");
                                        if (col1 != "0")
                                        {
                                            col1 = fgen.seek_iname_dt(dtMulvch, "ICODE='" + col1 + "'", "QTY1");
                                            if (col1 != "0")
                                                dro["stg_Wt"] = col1;
                                        }

                                        if (vdview[x].Row["ibcode"].ToString().Trim().Substring(0, 2) == "30")
                                        {
                                            dro["stg_Wt"] = 0;
                                        }

                                        //dro["stg_rt"] = fgen.seek_iname_dt(dtPo, "stg='" + vdview[x].Row["stg"].ToString().Trim() + "'", "qty1");

                                        dro["irate"] = vdview[x].Row["bchrate"];
                                        dro["ibname"] = vdview[x].Row["itemname"];

                                        if (co_cd == "NIRM" && dtPo != null)
                                            dro["jr"] = fgen.seek_iname_dt(dtPo, "icode='" + vdview[x].Row["ibcode"].ToString().Trim() + "'", "prate");

                                        dro["val"] = "0";
                                        dro["val_nt"] = "0";
                                        if (mdt1.Rows[i]["lvl"].ToString() == "1")
                                        {
                                            mq7 = "";
                                            dro["pcode"] = mdt1.Rows[i]["icode"].ToString().Trim();
                                            mq7 = mdt1.Rows[i]["icode"].ToString().Trim();
                                        }
                                        else dro["pcode"] = mq7;

                                        //if (vdview[x].Row[topickicode].ToString().Trim().Substring(0, 1) == "7")
                                        //{
                                        //    dro["wt_cnc"] = vdview[x].Row["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                        //    dro["wt_rft"] = vdview[x].Row["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                        //}

                                        dro["wt_cnc"] = vdview[x].Row["IBDIEPC"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();
                                        dro["wt_rft"] = vdview[x].Row["SUB_ISSUE_NO"].ToString().toDouble() * dro["stg_Wt"].ToString().toDouble();

                                        dro["SUB_ISSUE_NO"] = vdview[x].Row["SUB_ISSUE_NO"].ToString();
                                        dro["IBDIEPC"] = vdview[x].Row["IBDIEPC"].ToString();

                                        v++;

                                        mdt1.Rows.Add(dro);
                                    } vdview1.Dispose();
                                } vdview.Dispose();
                            }
                            //*****************************
                            #endregion

                            //*****************************sorting on Parent Code,Level,Child Code
                            mdt1.DefaultView.Sort = "pcode,lvl,icode";
                            mdt1 = mdt1.DefaultView.ToTable();

                            #region seeking LC and update value
                            //*****************************
                            value1 = "";
                            for (int i = 0; i < mdt1.Rows.Count; i++)
                            {
                                vdview = new DataView(mdt1, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                                if (vdview.Count <= 0)
                                {
                                    if (co_cd != "KCLG")
                                    {
                                        if (HCID != "29157")
                                        {
                                            if (dt2.Rows.Count > 0)
                                            {
                                                if (co_cd == "SAGM" && mdt1.Rows[i]["ibcode"].ToString().Trim().Substring(0, 2) == "10")
                                                {
                                                    sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                    if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                                    else
                                                    {
                                                        sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                        if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                                        else
                                                        {
                                                            sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                            if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                                            else
                                                            {
                                                                sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                                if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                    if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                                    else
                                                    {
                                                        sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                        if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else mdt1.Rows[i]["irate"] = "0";
                                vdview.Dispose();
                                mdt1.Rows[i]["val"] = Convert.ToDouble(fgen.make_double(mdt1.Rows[i]["ibqty"].ToString()) * fgen.make_double(mdt1.Rows[i]["irate"].ToString()));
                                mdt1.Rows[i]["val_nt"] = Convert.ToDouble(fgen.make_double(mdt1.Rows[i]["ibdiepc"].ToString()) * fgen.make_double(mdt1.Rows[i]["irate"].ToString()));
                                double dvl = 0;
                                if (co_cd == "NIRM")
                                {
                                    dvl += fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "prate"));
                                    if (fgen.make_double(mdt1.Rows[i]["jr"].ToString()) <= 0)
                                        mdt1.Rows[i]["jr"] = dvl;
                                }
                            }
                            //*****************************
                            #endregion

                            #region Filling Job Work Value for NIRM
                            //*****************************
                            mq0 = "0";
                            mq7 = "0";
                            mq10 = "0";
                            if (co_cd == "NIRM")
                            {
                                dist1_view = new DataView(mdt1);
                                dt_dist1 = new DataTable();
                                if (dist1_view.Count > 0)
                                {
                                    dist1_view.Sort = "pcode";
                                    dt_dist1 = dist1_view.ToTable(true, "pcode");
                                }
                                foreach (DataRow drdist1 in dt_dist1.Rows)
                                {
                                    dro = mdt1.NewRow();
                                    dro["icode"] = drdist1["pcode"].ToString().Trim();
                                    dro["ibcode"] = drdist1["pcode"].ToString().Trim();
                                    dro["pcode"] = drdist1["pcode"].ToString().Trim();
                                    dro["ibqty"] = 0;
                                    dro["irate"] = 0;
                                    dro["val"] = 0;
                                    dro["jr"] = fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + drdist1["pcode"].ToString().Trim() + "'", "prate"));
                                    mdt1.Rows.Add(dro);
                                }
                            }
                            //*****************************
                            #endregion

                            #region Making Final Value of Job Work
                            //*****************************
                            double joVal = 0;
                            mq3 = "";
                            mq4 = "";
                            mq1 = "";
                            {
                                vdview = new DataView(mdt1, "pcode='" + dt_dist_row["icode"].ToString().Trim() + "'", "pcode", DataViewRowState.CurrentRows);
                                for (int i = 0; i < vdview.Count; i++)
                                {
                                    if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(vdview[i].Row["val"].ToString().Trim()), 2).ToString();
                                    else mq0 = vdview[i].Row["val"].ToString().Trim();

                                    if (mq1.toDouble() > 0) mq1 = Math.Round(mq1.toDouble() + Convert.ToDouble(vdview[i].Row["val_nt"].ToString().toDouble()), 2).ToString();
                                    else mq1 = vdview[i].Row["val_nt"].ToString().toDouble().ToString();

                                    mq3 = Convert.ToString(mq3.toDouble() + vdview[i].Row["wt_cnc"].ToString().Trim().toDouble());
                                    mq4 = Convert.ToString(mq4.toDouble() + vdview[i].Row["wt_rft"].ToString().Trim().toDouble());
                                    mq10 = Convert.ToString(mq10.toDouble() + vdview[i].Row["stg_Wt"].ToString().Trim().toDouble());

                                    if (co_cd == "NIRM")
                                    {
                                        joVal += fgen.make_double(vdview[i].Row["jr"].ToString().Trim());
                                    }
                                }
                            }
                            if (joVal <= 0)
                            {
                                //for (int i = 0; i < dt_dist.Rows.Count; i++)
                                {
                                    double dvl = 0;
                                    dvl = fgen.make_double(fgen.seek_iname_dt(dtPo, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "prate"));
                                    mdt1.Rows[0]["jr"] = dvl;
                                    joVal += dvl;
                                }
                            }
                            //*****************************
                            #endregion

                            vdview.Dispose();

                            db6 = 0;
                            db5 = 0;
                            double mul_fact = 0;

                            #region Fatching Process Value from BOMANX for DREM
                            //*****************************
                            if (co_cd == "DREM")
                            {
                                if (bomanx.Rows.Count > 0)
                                {
                                    mul_fact = 0;
                                    vdview = new DataView(bomanx, "ICODE='" + mdt1.Rows[0]["PCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                    for (int i = 0; i < vdview.Count; i++)
                                    {
                                        if (mainLotSize > 0)
                                            mul_fact = fgen.make_double(mdt1.Rows[0]["IBQTY"].ToString().Trim(), 0);
                                        if (mul_fact < 1) mul_fact = 1;

                                        db5 = ((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact);
                                        db6 += db5;
                                    }
                                }
                            }
                            //*****************************
                            #endregion

                            for (int f = 0; f < mdt1.Rows.Count; f++)
                            {
                                mdt.ImportRow(mdt1.Rows[f]);

                                #region Fatching Process Value from BOMANX for DREM
                                //*****************************
                                if (co_cd == "DREM")
                                {
                                    if (bomanx.Rows.Count > 0)
                                    {
                                        mul_fact = 0;
                                        vdview = new DataView(bomanx, "ICODE='" + mdt1.Rows[f]["IBCODE"].ToString().Trim() + "'", "ICODE", DataViewRowState.CurrentRows);
                                        for (int i = 0; i < vdview.Count; i++)
                                        {
                                            if (mainLotSize > 0)
                                                mul_fact = fgen.make_double(mdt1.Rows[f]["IBQTY"].ToString().Trim(), 0);
                                            if (mul_fact < 1) mul_fact = 1;

                                            db5 = ((fgen.make_double(vdview[i].Row["costperk"].ToString()) / 1000) * mainLotSize * mul_fact);
                                            db6 += db5;
                                        }
                                    }
                                }
                                #endregion
                            }

                            mdt1.Dispose();

                            // mdt is table which is having Bom in Expended Form
                            dro1 = fmdt.NewRow();
                            dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                            dro1["val"] = mq0;
                            dro1["val_nt"] = mq1;
                            dro1["c_cost"] = mq10;
                            dro1["wt_cnc"] = mq3;
                            dro1["wt_rft"] = mq4;
                            if (co_cd == "NIRM")
                                dro1["jo_val"] = joVal;
                            if (co_cd == "DREM")
                            {
                                if (db6 > 0)
                                    dro1["jo_val"] = fgen.make_double(db6 / mainLotSize, 4);

                                dro1["lot_size"] = mainLotSize;
                            }
                            dro1["gr_wt"] = fgen.seek_iname_dt(dt, "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "'", "IBDIEPC");

                            if (dt_dist_row["icode"].ToString().Trim().Substring(0, 2) == "71")
                                dro1["gr_wt"] = dt.Compute("SUM(IBDIEPC)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "'");
                            else dro1["gr_wt"] = dt.Compute("SUM(IBDIEPC)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "' AND SUBSTRING(IBCODE,1,1)='7'");

                            if (dt_dist_row["icode"].ToString().Trim().Substring(0, 2) == "71")
                                dro1["nt_wt"] = dt.Compute("SUM(SUB_ISSUE_NO)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "'");
                            else dro1["nt_wt"] = dt.Compute("SUM(SUB_ISSUE_NO)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "' AND SUBSTRING(IBCODE,1,1)='7'");

                            fmdt.Rows.Add(dro1);

                            if (frm_formID == "F10194E" || frm_formID == "F10198" || frm_formID == "F10198W" || frm_formID == "F10194F")
                            {
                                dro1 = mdt.NewRow();
                                dro1["LVL"] = "0";
                                dro1["pcode"] = dt_dist_row["icode"].ToString().Trim();
                                dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                                dro1["val"] = mq0;
                                dro1["val_nt"] = mq1;
                                dro1["c_cost"] = mq10;
                                dro1["wt_cnc"] = mq3;
                                dro1["wt_rft"] = mq4;
                                dro1["gr_wt"] = fgen.seek_iname_dt(dt, "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "'", "IBDIEPC");

                                //if (dt_dist_row["icode"].ToString().Trim().Substring(0, 2) == "71")
                                //    dro1["gr_wt"] = dt.Compute("SUM(IBDIEPC)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "'");
                                //else dro1["gr_wt"] = dt.Compute("SUM(IBDIEPC)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "' AND SUBSTRING(IBCODE,1,1)='7'");

                                if (dt_dist_row["icode"].ToString().Trim().Substring(0, 2) == "71")
                                    dro1["nt_wt"] = dt.Compute("SUM(SUB_ISSUE_NO)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "'");
                                else dro1["nt_wt"] = dt.Compute("SUM(SUB_ISSUE_NO)", "ICODE='" + dt_dist_row["icode"].ToString().Trim() + "' AND SUBSTRING(IBCODE,1,1)='7'");

                                mdt.Rows.Add(dro1);
                            }

                            // fmdt is table which is only having Parant Bom icode and Value

                            double finRate = joVal + fgen.make_double(mq0);
                            if (co_cd == "NIRM")
                            {
                                fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE= '" + finRate + "' WHERE TRIM(ICODE)='" + dt_dist_row["icode"].ToString().Trim() + "'");
                            }
                            if (co_cd == "PRAG" || co_cd == "IAIJ" || co_cd == "DREM" || (co_cd == "BUPL" && dt_dist_row["icode"].ToString().Trim().Substring(0, 1) == "7"))
                            {
                                fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE= '" + mq0 + "' WHERE TRIM(ICODE)='" + dt_dist_row["icode"].ToString().Trim() + "'");
                            }
                        }
                    }
                    if (frm_formID == "29155" || frm_formID == "29155a" || frm_formID == "A2" || frm_formID == "F10186")
                    {
                        #region BOM vs Sales Value, Sch vs Dsp
                        mq7 = ""; mq5 = "-";
                        mdt1 = new DataTable();
                        mdt1 = fmdt.Clone();
                        dro1 = null;
                        dt = new DataTable();

                        //*****************************Fatching invoice Value within Time Period Selected
                        SQuery = "Select (case when a.irate>0 then A.irate else b.irate end) as rate,SUM(a.iqtyout) AS iqtyout,TRIM(a.acode) AS acode,TRIM(a.icode) AS icode from ivoucher a ,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type like '4%'  AND A.VCHDATE " + xprdrange + "  and a.icode like '9%' GROUP BY TRIM(a.acode),TRIM(a.icode),a.irate,B.irate order by TRIM(A.ACODE),TRIM(a.icode)";

                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            sort_view = new DataView();
                            sort_view = dt.DefaultView;
                            sort_view.Sort = "acode";
                            dt3 = new DataTable();
                            dt3 = sort_view.ToTable(true, "acode");
                        }
                        for (int i = 0; i < fmdt.Rows.Count; i++)
                        {
                            //foreach (DataRow drr3 in dt3.Rows)
                            {
                                int k = 0;
                                //vdview = new DataView(dt, "acode='" + drr3["acode"].ToString().Trim() + "' and icode='" + fmdt.Rows[i]["icode"].ToString().Trim() + "'", "acode", DataViewRowState.CurrentRows);
                                vdview = new DataView();
                                if (dt.Rows.Count > 0)
                                    vdview = new DataView(dt, "icode='" + fmdt.Rows[i]["icode"].ToString().Trim() + "'", "acode,icode", DataViewRowState.CurrentRows);
                                for (int x = 0; x < vdview.Count; x++)
                                {
                                    dro1 = mdt1.NewRow();
                                    dro1["icode"] = fmdt.Rows[i]["icode"].ToString().Trim();
                                    dro1["val"] = fmdt.Rows[i]["val"].ToString().Trim();
                                    //if (k == 0)
                                    dro1["srate"] = vdview[x].Row["rate"].ToString().Trim();
                                    //else dro1["srate"] = "0";
                                    dro1["sqty"] = vdview[x].Row["iqtyout"].ToString().Trim();
                                    dro1["acode"] = vdview[x].Row["acode"].ToString().Trim();
                                    mdt1.Rows.Add(dro1);

                                    //break;
                                    k = 1;
                                }
                            }
                        }

                        fmdt = new DataTable();
                        fmdt = mdt1;

                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, co_cd, "extrusion");
                        dro = null;
                        foreach (DataRow fdr in fmdt.Rows)
                        {
                            dro = oDS.Tables[0].NewRow();
                            dro["BRANCHCD"] = mbr;
                            dro["type"] = "EX";
                            dro["vchnum"] = "000000";
                            dro["vchdate"] = DateTime.Now;
                            dro["icode"] = fdr["icode"].ToString().Trim();
                            dro["qty"] = Math.Round(Convert.ToDouble(fdr["val"].ToString().Replace(" ", "0")), 2);
                            dro["btchno"] = Math.Round(Convert.ToDouble(fdr["srate"].ToString().Replace(" ", "0")), 2);
                            dro["start1"] = Math.Round(Convert.ToDouble(fdr["sqty"].ToString().Replace(" ", "0")), 2);
                            //dro["comments"] = fmdt.Rows[i]["ibqty"];
                            dro["chars"] = fdr["acode"];
                            dro["ent_by"] = uname;
                            dro["ent_dt"] = DateTime.Now;

                            dro["start2"] = 0;
                            dro["close1"] = 0;
                            dro["close2"] = 0;
                            dro["rpm1"] = 0;
                            dro["rpm2"] = 0;
                            dro["DISPERSION1"] = 0;
                            dro["DISPERSION2"] = 0;
                            dro["srno"] = 0;
                            dro["btchdt"] = DateTime.Now;
                            dro["extloss"] = 0;

                            oDS.Tables[0].Rows.Add(dro);
                        }
                        fgen.save_data(frm_qstr, co_cd, oDS, "extrusion");
                        oDS.Dispose(); mdt.Dispose(); fmdt.Dispose();
                        //-----------------------------------------------------
                        //----------------------------------------------------                        
                        #region BOM Cost, Sale Cost, BOX wt SPKS style
                        if (frm_formID == "F10186")
                        {
                            SQuery = "Select trim(a.icode) as erpcode,b.iname as product,b.cpartno,b.unit,b.iweight as box_wt,round(sum(to_number(replace(NVL(a.comments,0),'-','0'))),5) as bom_qty,(a.qty) as Bom_val,max(is_number(a.btchno)) as sal_rate from extrusion a,item b where trim(a.icode)=trim(b.icode) and A.BRANCHCD='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' group by trim(a.icode),a.chars,b.iname,b.cpartno,b.unit,a.qty,b.iweight order by trim(a.icode),b.iname";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Bom Cost vs Sales Cost for the Period " + fromdt + " to " + todt, frm_qstr);
                        }
                        #endregion
                        else
                        {
                            SQuery = "Select '" + fromdt + "' as fromdt,'" + todt + "' as todt, a.chars as partycode,c.aname as party,trim(a.icode) as erpcode,b.iname as product,b.cpartno,b.unit,round(sum(to_number(replace(NVL(a.comments,0),'-','0'))),5) as bom_qty,(a.qty) as Bom_val,TO_NUMBER(sum(a.start1)) as Sal_qty,max(is_number(a.btchno)) as sal_rate from extrusion a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.chars)=trim(c.acode) AND A.BRANCHCD='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' group by trim(a.icode),a.chars,b.iname,b.cpartno,b.unit,a.chars,c.aname,a.qty order by c.aname,trim(a.icode),b.iname";
                            if (frm_formID == "29155a") fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "bomvssal", "bomvssalp");
                            else fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "bomvssal", "bomvssal");
                        }
                        #endregion
                    }
                    else if (frm_formID == "29157")
                    {
                        #region Expnded BOM
                        dist1_view = new DataView(mdt);
                        dt = new DataTable();
                        dt = dist1_view.ToTable(true, "lvl");

                        dist1_view = new DataView(mdt, "LVL=1", "pcode", DataViewRowState.CurrentRows);
                        dt2 = new DataTable();
                        dt2 = dist1_view.ToTable(true, "pcode");

                        dt3 = new DataTable();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt3.Columns.Add("lvl" + (i + 1), typeof(double));
                            dt3.Columns.Add("pcode" + (i + 1), typeof(string));
                            dt3.Columns.Add("Icode" + (i + 1), typeof(string));
                            dt3.Columns.Add("iname" + (i + 1), typeof(string));
                            dt3.Columns.Add("qty" + (i + 1), typeof(double));
                        }

                        dro = null;

                        double lvl = 0;
                        string ibcode = "";
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            for (int k = 0; k < 1; k++)
                            {
                                DataView DV = new DataView(mdt, "PCODE='" + dt2.Rows[i]["pcode"].ToString().Trim() + "' AND lvl='" + (k + 1) + "'", "lvl,icode,ibcode", DataViewRowState.CurrentRows);

                                for (int l = 0; l < DV.Count; l++)
                                {
                                    lvl = (k + 1);
                                    {
                                        dro = dt3.NewRow();
                                        dro["lvl" + lvl] = DV[l].Row["LVL"].ToString().Trim();
                                        dro["pcode" + lvl] = DV[l].Row["icode"].ToString().Trim();
                                        dro["icode" + lvl] = DV[l].Row["ibcode"].ToString().Trim();
                                        dro["iname" + lvl] = DV[l].Row["ibname"].ToString().Trim();
                                        dro["qty" + lvl] = DV[l].Row["ibqty"].ToString().Trim();
                                        dt3.Rows.Add(dro);

                                        ibcode += "," + DV[l].Row["ibcode"].ToString().Trim();
                                    }

                                    v = 0;
                                    int j = 0;
                                    for (int m = v; m < dt3.Rows.Count; m++)
                                    {
                                        if (!ibcode.Contains(",")) ibcode = "," + ibcode;
                                        for (int o = j; o < ibcode.Split(',').Length; o++)
                                        {
                                            if (ibcode.Split(',')[o].ToString().Length > 2)
                                            {
                                                DataView DV2 = new DataView(mdt, "ICODE='" + ibcode.Split(',')[o].ToString() + "' AND LVL<>'" + (1) + "'", "icode", DataViewRowState.CurrentRows);
                                                if (DV2.Count > 0)
                                                {
                                                    for (int z = 0; z < DV2.Count; z++)
                                                    {
                                                        lvl = fgen.make_double(DV2[z].Row["LVL"].ToString().Trim());
                                                        dro = dt3.NewRow();
                                                        dro["lvl" + (lvl)] = DV2[z].Row["LVL"].ToString().Trim();
                                                        dro["pcode" + (lvl)] = DV2[z].Row["icode"].ToString().Trim();
                                                        dro["icode" + (lvl)] = DV2[z].Row["ibcode"].ToString().Trim();
                                                        dro["iname" + (lvl)] = DV2[z].Row["ibname"].ToString().Trim();
                                                        dro["qty" + (lvl)] = DV2[z].Row["ibqty"].ToString().Trim();
                                                        dt3.Rows.Add(dro);

                                                        if (!ibcode.Contains(DV2[z].Row["ibcode"].ToString().Trim()))
                                                            ibcode += "," + DV2[z].Row["ibcode"].ToString().Trim();

                                                        v++;
                                                    }
                                                }
                                                j++;
                                                m++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Session["send_dt"] = dt3;
                        fgen.Fn_open_rptlevel("Expended BOM", frm_qstr);
                        #endregion
                    }
                    else if (frm_formID == "F05125E" || frm_formID == "F05125D")
                    {
                        #region SAGM Variance Report
                        SQuery = "SELECT A.BRANCHCD,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,(A.IQTYOUT-a.iqtyin) as Quantity,(case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end) as rate,abs(round((a.iqtyin-A.IQTYOUT) * (case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end))) as value,'From Item Master Rate' as rate_from,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODe)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='" + (val == "F05125D" ? "36" : "3F") + "' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC ";
                        if (frm_formID == "F05125E")
                        {
                            SQuery = "SELECT A.BRANCHCD,A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.ACODE AS STG_CODE,C.NAME AS STAGE_NAME,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,(A.IQTYOUT-a.iqtyin) as Quantity,(case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end) as rate,abs(round((a.iqtyin-A.IQTYOUT) * (case when B.IRATe1>0 then B.IRATe1 when b.iqd>0 then b.iqd else b.irate end))) as value,'From Item Master Rate' as rate_from,to_char(a.vchdate,'yyyymmdd') as vdd FROM IVOUCHER A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODe)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACREF) AND C.ID='WI' AND C.BRANCHCD='" + mbr + "' AND A.BRANCHCD='" + mbr + "' AND A.TYPE='" + (val == "F05125D" ? "36" : "3F") + "' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC ";
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        // mrr table
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, co_cd, SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where BRANCHCD='" + mbr + "' and type in ('02','07') and trim(nvl(finvno,'-'))!='-' and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_date('" + todt + "','dd/mm/yyyy') /*and icode like '9%'*/ order by icode,vdd desc");
                        // sale table
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, co_cd, SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where BRANCHCD='" + mbr + "' and substr(type,1,1) in ('4') and vchdate>=round(to_Date('" + todt + "','dd/mm/yyyy')-500) and vchdate<=to_date('" + todt + "','dd/mm/yyyy') and icode like '9%' order by icode,vdd desc");
                        string mhd = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            mhd = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "c_cost");
                            if (mhd.toDouble() != 0)
                            {
                                dr["rate"] = mhd;
                                dr["value"] = Math.Round(mhd.toDouble() * dr["Quantity"].ToString().toDouble(), 2);
                                dr["rate_from"] = "BOM Value";
                            }
                            else
                            {
                                if (dr["erpcode"].ToString().Trim().Substring(0, 1) != "9")
                                {
                                    mhd = fgen.seek_iname_dt(dt2, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "rate");
                                    if (mhd.toDouble() != 0)
                                    {
                                        dr["rate"] = mhd;
                                        dr["value"] = Math.Round(mhd.toDouble() * dr["Quantity"].ToString().toDouble(), 2);
                                        dr["rate_from"] = "Latest MRR";
                                    }
                                }
                                if (dr["erpcode"].ToString().Trim().Substring(0, 1) == "9")
                                {
                                    mhd = fgen.seek_iname_dt(dt3, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "rate");
                                    if (mhd.toDouble() != 0)
                                    {
                                        dr["rate"] = mhd;
                                        dr["value"] = Math.Round(mhd.toDouble() * dr["Quantity"].ToString().toDouble(), 2);
                                        dr["rate_from"] = "Latest Invoice Rate";
                                    }
                                }
                            }
                            dr["value"] = Math.Round(dr["rate"].ToString().toDouble() * dr["Quantity"].ToString().toDouble(), 2);
                        }
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevel("" + (val == "F05125D" ? "Store Variance Details " : "WIP Variance Details ") + " of Products for the period " + fromdt + " and " + todt + "", frm_qstr);
                        #endregion
                    }
                    else if (frm_formID == "F10194E" || frm_formID == "F10198" || frm_formID == "F10198W" || frm_formID == "F10194F")
                    {
                        Session["mdt" + mbr] = mdt;
                        Session["fmdt" + mbr] = fmdt;

                        double alloyrate = 0;
                        DataTable newDt2;
                        if (dt2.Rows.Count > 0) newDt2 = dt2;
                        else newDt2 = (DataTable)ViewState["dt2" + frm_formID];
                        DataTable toShow = new DataTable();
                        toShow.Columns.Add("Level", typeof(string));
                        toShow.Columns.Add("PCODE", typeof(string));
                        toShow.Columns.Add("ERPCode", typeof(string));
                        toShow.Columns.Add("Name", typeof(string));
                        toShow.Columns.Add("ChildCode", typeof(string));
                        toShow.Columns.Add("ChildName", typeof(string));
                        toShow.Columns.Add("Gr.Wt(BOM)", typeof(string));
                        toShow.Columns.Add("Nt.Wt(BOM)", typeof(string));
                        toShow.Columns.Add("Qty", typeof(string));
                        toShow.Columns.Add("UOM", typeof(string));
                        toShow.Columns.Add("Price", typeof(string));
                        toShow.Columns.Add("Cost", typeof(string));
                        toShow.Columns.Add("RM_Cost", typeof(string));
                        toShow.Columns.Add("RM_Net", typeof(double));
                        //toShow.Columns.Add("Net_Rate", typeof(string));
                        //toShow.Columns.Add("Net_Cost", typeof(string));

                        toShow.Columns.Add("Gross_wt_pl", typeof(string));
                        toShow.Columns.Add("Cost_pl", typeof(string));
                        toShow.Columns.Add("Closing_stk", typeof(string));
                        toShow.Columns.Add("closing_value(Matl.Cost)", typeof(string));
                        toShow.Columns.Add("Stk_wt", typeof(string));

                        toShow.Columns.Add("Conv_Rate", typeof(string));
                        toShow.Columns.Add("Conv_Value(Nt)", typeof(string));
                        toShow.Columns.Add("Conv_Value(Gr)", typeof(string));
                        toShow.Columns.Add("Conv_value_on_gross", typeof(string));
                        toShow.Columns.Add("Conv_value_on_net", typeof(string));
                        toShow.Columns.Add("Stock_value_with_conv_GR", typeof(string));
                        toShow.Columns.Add("Stock_value_with_conv_NT", typeof(string));

                        DataTable dtItem = new DataTable();
                        dtItem = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(ICODE) AS ICODE,INAME,CPARTNO,UNIT,WT_RFT,iRATE FROM ITEM WHERE LENGTH(TRIM(ICODe))>4 ORDER BY ICODE ");

                        //DataTable dtFamst = new DataTable();
                        //dtFamst = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(aCODE) AS aCODE,ANAME FROM FAMST WHERE SUBSTR(ACODE,1,2) IN ('02','05','06','16') ORDER BY ACODE ");

                        //DataTable dtBranch = new DataTable();
                        //dtBranch = fgen.getdata(frm_qstr, co_cd, "SELECT TRIM(type1) AS type1,NAME FROM type WHERE ID='B' ORDER BY TYPE1 ");

                        DataRow drToShow;
                        double pL = 0;
                        pL = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='C10' ", "PARAMS").toDouble();

                        DataView dsor = new DataView(mdt, "", "LVL,PCODE,ICODE", DataViewRowState.CurrentRows);
                        mdt = new DataTable();
                        mdt = dsor.ToTable();

                        r10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
                        cond = "";
                        if (r10.Length > 2) cond = " and vchdate>=to_Date('" + r10 + "','dd/mm/yyyy')";

                        dt1 = new DataTable();
                        if (frm_formID == "F10198")
                            dt1 = fgen.getdata(frm_qstr, co_cd, "SELECT 'FG Store' AS stagename,'FG' AS CODE,'FG' AS acref FROM DUAL UNION ALL SELECT 'RM/BOP ' AS stagename,'RM' AS CODE,'RM' AS acref FROM DUAL ");
                        else if (frm_formID == "F10198W")
                            dt1 = fgen.getdata(frm_qstr, co_cd, "select * from (select name as stagename,acref as code,acref from typeGRP where BRANCHCD='" + mbr + "' and ID='WI' and acref like '6%' order by type1)");
                        else dt1 = fgen.getdata(frm_qstr, co_cd, "SELECT 'FG Store' AS stagename,'FG' AS CODE,'FG' AS acref FROM DUAL UNION ALL SELECT 'RM/BOP ' AS stagename,'RM' AS CODE,'RM' AS acref FROM DUAL UNION ALL select * from (select name as stagename,acref as code,acref from typeGRP where BRANCHCD='" + mbr + "' and ID='WI' and acref like '6%' order by type1)");

                        DataTable dtStock = new DataTable();
                        int indexNo = 0;

                        foreach (DataRow stages in dt1.Rows)
                        {
                            xprd1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT1 + "','dd/mm/yyyy')-1";
                            xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1" + "";
                            xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')" + "";

                            if (stages["acref"].ToString().Trim() == "FG")
                            {
                                mq0 = "Select icode as erpcode,Closing_Stk from (select a.icode,sum(a.opening) as opb,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange1 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange + " GROUP BY trim(icode) ,branchcd) a GROUP BY a.icode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)>0 )";
                                mq0 = "SELECT A.*,B.CPARTNO,B.UNIT,B.IRATE AS RATES,b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname FROM (" + mq0 + ") A,ITEM B WHERE TRIM(A.ERPCODE)=TRIM(b.ICODE) and substr(a.erpcode,1,1) in ('9','7') and A.CLOSING_STK!=0  ";
                            }
                            else if (stages["acref"].ToString().Trim() == "RM")
                            {
                                mq0 = "Select icode as erpcode,Closing_Stk from (select a.icode,sum(a.opening) as opb,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange1 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange + " GROUP BY trim(icode) ,branchcd) a GROUP BY a.icode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)>0 )";
                                mq0 = "SELECT A.*,B.CPARTNO,B.UNIT,B.IRATE AS RATES,b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname FROM (" + mq0 + ") A,ITEM B WHERE TRIM(A.ERPCODE)=TRIM(b.ICODE) and substr(a.erpcode,1,1) not in ('9','7') and A.CLOSING_STK!=0 ";
                            }
                            else
                            {
                                xprd1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT1 + "','dd/mm/yyyy')-1";
                                xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1" + cond;
                                xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')" + cond;
                                mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' and substr(a.icode,1,2) not in ('20','30','40')  group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0  Order by substr(a.icode,1,4),B.iname";
                            }
                            if (mq0 != "")
                                dtStock = fgen.getdata(frm_qstr, co_cd, mq0);
                            string mhd = "";
                            foreach (DataRow drStock in dtStock.Rows)
                            {
                                btfld = fgen.seek_iname_dt(mdt, "PCODE='" + drStock["erpcode"].ToString().Trim() + "'", "ICODE");
                                if (btfld != "0")
                                {
                                    if (mdt.Rows.Count > 0)
                                    {
                                        var filterdRow = mdt.Select("PCODE='" + btfld + "'").CopyToDataTable();
                                        double rmNet = 0;

                                        foreach (DataRow drr in filterdRow.Rows)
                                        {
                                            drToShow = toShow.NewRow();
                                            if (drr["LVL"] == "0") drToShow["Level"] = stages["stagename"];
                                            else
                                            {
                                                int pad = fgen.make_int(drr["LVL"].ToString());
                                                drToShow["Level"] = drr["LVL"].ToString().Trim().PadLeft((pad * 2), '_');
                                            }

                                            drToShow["PCODE"] = drr["PCODE"].ToString().Trim();
                                            drToShow["ERPCODE"] = drr["ICODE"].ToString().Trim();
                                            drToShow["Name"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drr["ICODE"].ToString().Trim() + "'", "INAME");
                                            if (drr["IBCODE"].ToString().Trim().Length > 0)
                                            {
                                                drToShow["childcode"] = drr["IBCODE"].ToString().Trim();
                                                drToShow["childname"] = fgen.seek_iname_dt(dtItem, "icode='" + drr["ibcode"].ToString().Trim() + "'", "iname");
                                                drToShow["uom"] = fgen.seek_iname_dt(dtItem, "icode='" + drr["ibcode"].ToString().Trim() + "'", "unit");
                                            }

                                            //drToShow["Gr.Wt(BOM)"] = drr["wt_Rft"].ToString().Trim();
                                            //drToShow["Nt.Wt(BOM)"] = drr["wt_cnc"].ToString().Trim();

                                            if (drr["LVL"] == "0")
                                            {
                                                drToShow["Nt.Wt(BOM)"] = fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "nt_wt");
                                                drToShow["Gr.Wt(BOM)"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "WT_RFT");
                                            }
                                            else
                                            {
                                                drToShow["Nt.Wt(BOM)"] = drr["sub_issue_no"];
                                                drToShow["Gr.Wt(BOM)"] = drr["ibdiepc"];
                                            }

                                            drToShow["gross_wt_pl"] = Math.Round(drToShow["Gr.Wt(BOM)"].ToString().Trim().toDouble() * ((100 + pL) / 100), 3);

                                            drToShow["Qty"] = drr["IBQTY"].ToString().Trim().toDouble(6);
                                            drToShow["Price"] = drr["IRATE"].ToString().Trim();
                                            drToShow["Cost"] = drr["VAL"].ToString().Trim().toDouble(5);

                                            if (drr["LVL"] == "0")
                                            {
                                                col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "val");
                                                if (col1 != "0")
                                                {
                                                    drToShow["RM_Cost"] = col1.toDouble(2).ToString("f");
                                                    if (drToShow["erpcode"].ToString().Trim().Substring(0, 1) == "7" || drToShow["erpcode"].ToString().Trim().Substring(0, 1) == "9")
                                                    {
                                                        drToShow["RM_Net"] = fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "val_nt").toDouble(2).ToString("f");

                                                        if (mbr == "06" || mbr == "08")
                                                            drToShow["RM_Net"] = (col1.toDouble(5) - (drToShow["Gr.Wt(BOM)"].ToString().toDouble() - drToShow["Nt.Wt(BOM)"].ToString().toDouble()) * 0.95 * (col1.toDouble(5) * 0.3)).toDouble(2).ToString("f");
                                                        else drToShow["RM_Net"] = (col1.toDouble(5) - (drToShow["Gr.Wt(BOM)"].ToString().toDouble() - drToShow["Nt.Wt(BOM)"].ToString().toDouble()) * 0.3 * (col1.toDouble(5) * 0.3)).toDouble(2).ToString("f");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (btfld == "73410056")
                                                {

                                                }
                                                mq4 = "0";
                                                if (drr["IBCODE"].ToString().Trim().Substring(0, 1) != "7")
                                                {
                                                    mhd = fgen.seek_iname_dt(filterdRow, "IBCODE='" + drr["IBCODE"].ToString().Trim() + "' AND IBNETWT='" + drr["IBNETWT"].ToString().Trim() + "'", "ICODE");
                                                    if (mhd.Substring(0, 1) == "7")
                                                    {
                                                        mq1 = fgen.seek_iname_dt(filterdRow, "IBCODE='" + mhd + "'", "ICODE");
                                                        if (mq1.Substring(0, 1) == "7")
                                                        {
                                                            mhd = fgen.seek_iname_dt(filterdRow, "IBCODE='" + mq1 + "'", "ICODE");
                                                            if (mhd.Substring(0, 1) == "7")
                                                            {
                                                                mq1 = fgen.seek_iname_dt(filterdRow, "IBCODE='" + mhd + "'", "ICODE");
                                                                if (mq1 == "0") mq4 = fgen.seek_iname_dt(filterdRow, "ICODE='" + mhd + "' AND LVL<>'0'", "SUB_ISSUE_NO");
                                                                else mq4 = fgen.seek_iname_dt(filterdRow, "ICODE='" + mq1 + "' AND IBCODE='" + mhd + "'", "IBNETWT");
                                                            }
                                                            else
                                                            {
                                                                mq4 = fgen.seek_iname_dt(filterdRow, "ICODE='" + mq1 + "' AND LVL<>'0'", "SUB_ISSUE_NO");
                                                            }
                                                        }
                                                        else
                                                            mq4 = fgen.seek_iname_dt(filterdRow, "IBCODE='" + mhd + "' AND LVL<>'0'", "IBNETWT");
                                                    }
                                                }

                                                drToShow["RM_Cost"] = drToShow["Cost"];

                                                if (mq4.toDouble() > 0)
                                                {
                                                    drToShow["RM_Net"] = (drToShow["Cost"].ToString().toDouble(5) - (drToShow["Gr.Wt(BOM)"].ToString().toDouble() - mq4.toDouble()) * 0.3 * (drToShow["price"].ToString().toDouble(5) * 0.3)).toDouble(2).ToString("f");
                                                }
                                                else
                                                    drToShow["RM_Net"] = (drToShow["Cost"].ToString().toDouble(5) - (drToShow["Gr.Wt(BOM)"].ToString().toDouble() - drToShow["Nt.Wt(BOM)"].ToString().toDouble()) * 0.3 * (drToShow["price"].ToString().toDouble(5) * 0.3)).toDouble(2).ToString("f");
                                            }

                                            drToShow["Conv_Rate"] = drr["stg_wt"];
                                            drToShow["Conv_Value(Nt)"] = drr["wt_rft"];
                                            drToShow["Conv_Value(Gr)"] = drr["wt_cnc"];

                                            if (co_cd == "SAGM" && (drToShow["erpcode"].ToString().Trim().Substring(0, 2) == "10"))
                                            {
                                                alloyrate = 0;
                                                if (newDt2.Rows.Count > 0)
                                                {
                                                    sort_view = new DataView(newDt2, "trim(icode)='" + drStock["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                    if (sort_view.Count > 0)
                                                    {
                                                        if (drStock["mat4"].ToString().Trim().toDouble() > 0)
                                                            alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (drStock["mat4"].ToString().Trim().toDouble() / 100);
                                                        else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                        drToShow["RM_Cost"] = alloyrate.toDouble(2).ToString("f");
                                                    }
                                                    else
                                                    {
                                                        sort_view = new DataView(newDt2, "trim(icode)='" + drStock["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                        if (sort_view.Count > 0)
                                                        {
                                                            if (drStock["mat4"].ToString().Trim().toDouble() > 0)
                                                                alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (drStock["mat4"].ToString().Trim().toDouble() / 100);
                                                            else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                            drToShow["RM_Cost"] = alloyrate.toDouble(2).ToString("f");
                                                        }
                                                    }
                                                }
                                            }

                                            if (drr["LVL"] == "0")
                                            {
                                                col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "c_cost");
                                                drToShow["conv_rate"] = col1.toDouble(2).ToString("f");
                                            }
                                            drToShow["cost_pl"] = Math.Round(drToShow["RM_Cost"].ToString().toDouble() * ((100 + pL) / 100), 2).toDouble(2).ToString("f");

                                            if (drr["LVL"].ToString() == "0")
                                            {
                                                drToShow["closing_stk"] = drStock["Closing_Stk"];
                                                drToShow["stk_wt"] = Math.Round(drToShow["closing_stk"].ToString().Trim().toDouble() * drToShow["Gr.Wt(BOM)"].ToString().Trim().toDouble(), 3);

                                                if (drToShow["RM_Net"].ToString().toDouble() <= 0)
                                                    drToShow["closing_value(Matl.Cost)"] = Math.Round(drToShow["closing_stk"].ToString().Trim().toDouble() * drToShow["RM_Cost"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");
                                                else drToShow["closing_value(Matl.Cost)"] = Math.Round(drToShow["closing_stk"].ToString().Trim().toDouble() * drToShow["RM_Net"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");

                                                drToShow["Conv_value_on_gross"] = Math.Round(drToShow["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "wt_cnc").toDouble(), 2).toDouble(2).ToString("f");
                                                drToShow["Conv_value_on_net"] = Math.Round(drToShow["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "wt_rft").toDouble(), 2).toDouble(2).ToString("f");

                                                drToShow["stock_value_with_conv_GR"] = Math.Round(drToShow["closing_value(Matl.Cost)"].ToString().toDouble() + drToShow["Conv_value_on_gross"].ToString().toDouble(), 2).toDouble(2).ToString("f");
                                                drToShow["stock_value_with_conv_NT"] = Math.Round(drToShow["closing_value(Matl.Cost)"].ToString().toDouble() + drToShow["Conv_value_on_net"].ToString().toDouble(), 2).toDouble(2).ToString("f");
                                            }

                                            if (drr["LVL"].ToString() != "0")
                                                rmNet += drToShow["RM_NET"].ToString().toDouble();

                                            toShow.Rows.Add(drToShow);
                                            indexNo++;
                                        }
                                        if (btfld != "0")
                                        {
                                            int newIndex = indexNo - (filterdRow.Rows.Count);
                                            //mhd = toShow.Compute("sum(RM_Net)", "PCODE='" + btfld + "' AND LEVEL<>'" + stages["stagename"] + "'").ToString();
                                            mhd = rmNet.ToString();
                                            if (mhd != "0")
                                            {
                                                toShow.Rows[newIndex]["RM_Net"] = mhd.toDouble();
                                                toShow.Rows[newIndex]["closing_value(Matl.Cost)"] = Math.Round(toShow.Rows[newIndex]["closing_stk"].ToString().Trim().toDouble() * toShow.Rows[newIndex]["RM_Net"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");
                                                toShow.Rows[newIndex]["stock_value_with_conv_NT"] = Math.Round(toShow.Rows[newIndex]["closing_value(Matl.Cost)"].ToString().toDouble() + toShow.Rows[newIndex]["Conv_value_on_net"].ToString().toDouble(), 2).toDouble(2).ToString("f");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (drStock["ERPCODE"].ToString().Trim() == "10650038")
                                    {

                                    }

                                    drToShow = toShow.NewRow();
                                    drToShow["Level"] = stages["stagename"];

                                    drToShow["ERPCODE"] = drStock["erpcode"].ToString().Trim();
                                    drToShow["Name"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drStock["erpcode"].ToString().Trim() + "'", "INAME");
                                    {
                                        drToShow["childcode"] = "";
                                        drToShow["childname"] = "";
                                        drToShow["uom"] = fgen.seek_iname_dt(dtItem, "icode='" + drStock["erpcode"].ToString().Trim() + "'", "unit");
                                    }

                                    //drToShow["Gr.Wt(BOM)"] = drr["wt_Rft"].ToString().Trim();
                                    //drToShow["Nt.Wt(BOM)"] = drr["wt_cnc"].ToString().Trim();

                                    {
                                        drToShow["Nt.Wt(BOM)"] = 0;
                                        drToShow["Gr.Wt(BOM)"] = 0;
                                    }

                                    drToShow["gross_wt_pl"] = 0;

                                    drToShow["Qty"] = drStock["closing_stk"].ToString();
                                    drToShow["Price"] = fgen.seek_iname_dt(newDt2, "ICODE='" + drStock["erpcode"].ToString() + "' ", "RATE");
                                    if (drToShow["Price"].ToString().toDouble() <= 0)
                                        drToShow["Price"] = drStock["rates"];
                                    if (drToShow["Price"].ToString().toDouble() <= 0)
                                        drToShow["Price"] = fgen.seek_iname_dt(dtItem, "ICODE='" + drStock["erpcode"].ToString() + "' ", "iRATE");
                                    drToShow["Cost"] = drToShow["Qty"].ToString().toDouble(5) * drToShow["PRICE"].ToString().toDouble(5);

                                    {
                                        drToShow["RM_Cost"] = drToShow["PRICE"];
                                        drToShow["RM_Net"] = drToShow["PRICE"];
                                    }


                                    drToShow["Conv_Rate"] = 0;
                                    drToShow["Conv_Value(Nt)"] = 0;
                                    drToShow["Conv_Value(Gr)"] = 0;

                                    if (co_cd == "SAGM" && (drToShow["erpcode"].ToString().Trim().Substring(0, 2) == "10"))
                                    {
                                        alloyrate = 0;
                                        if (newDt2.Rows.Count > 0)
                                        {
                                            sort_view = new DataView(newDt2, "trim(icode)='" + drStock["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                            if (sort_view.Count > 0)
                                            {
                                                if (drStock["mat4"].ToString().Trim().toDouble() > 0)
                                                    alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (drStock["mat4"].ToString().Trim().toDouble() / 100);
                                                else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                drToShow["RM_Cost"] = alloyrate.toDouble(2).ToString("f");
                                            }
                                            else
                                            {
                                                sort_view = new DataView(newDt2, "trim(icode)='" + drStock["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                if (sort_view.Count > 0)
                                                {
                                                    if (drStock["mat4"].ToString().Trim().toDouble() > 0)
                                                        alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (drStock["mat4"].ToString().Trim().toDouble() / 100);
                                                    else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                    drToShow["RM_Cost"] = alloyrate.toDouble(2).ToString("f");
                                                }
                                            }
                                        }
                                    }

                                    drToShow["conv_rate"] = 0;
                                    drToShow["cost_pl"] = Math.Round(drToShow["RM_Cost"].ToString().toDouble() * ((100 + pL) / 100), 2).toDouble(2).ToString("f");

                                    drToShow["closing_stk"] = drStock["CLOSING_STK"];
                                    drToShow["stk_wt"] = Math.Round(drToShow["closing_stk"].ToString().Trim().toDouble() * drToShow["Gr.Wt(BOM)"].ToString().Trim().toDouble(), 3);

                                    if (drToShow["RM_Net"].ToString().toDouble() <= 0)
                                        drToShow["closing_value(Matl.Cost)"] = Math.Round(drToShow["closing_stk"].ToString().Trim().toDouble() * drToShow["RM_Cost"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");
                                    else drToShow["closing_value(Matl.Cost)"] = Math.Round(drToShow["closing_stk"].ToString().Trim().toDouble() * drToShow["RM_Net"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");

                                    drToShow["Conv_value_on_gross"] = Math.Round(drToShow["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "wt_cnc").toDouble(), 2).toDouble(2).ToString("f");
                                    drToShow["Conv_value_on_net"] = Math.Round(drToShow["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + drToShow["erpcode"].ToString().Trim() + "'", "wt_rft").toDouble(), 2).toDouble(2).ToString("f");

                                    drToShow["stock_value_with_conv_GR"] = Math.Round(drToShow["closing_value(Matl.Cost)"].ToString().toDouble() + drToShow["Conv_value_on_gross"].ToString().toDouble(), 2).toDouble(2).ToString("f");
                                    drToShow["stock_value_with_conv_NT"] = Math.Round(drToShow["closing_value(Matl.Cost)"].ToString().toDouble() + drToShow["Conv_value_on_net"].ToString().toDouble(), 2).toDouble(2).ToString("f");

                                    toShow.Rows.Add(drToShow);

                                    indexNo++;
                                }
                            }
                        }


                        if (frm_formID == "F10198" || frm_formID == "F10198W")
                        {
                            string[] slab1 = new string[] { "0_to_30", "31_to_60", "61_to_90", "91_to_120", "121_to_180", "181_to_360" };
                            string allSlab = "";
                            string sumallSlab = "";
                            string todaysDt = "to_date('" + todt + "','dd/mm/yyyy')";
                            mq0 = "";
                            for (int s = 0; s < slab1.Length; s++)
                            {
                                mq0 = "R_" + slab1[s];
                                toShow.Columns.Add(mq0);
                                toShow.Columns.Add(mq0 + "_V");
                                allSlab += "," + "(case when (" + todaysDt + " - VCHDATE BETWEEN " + slab1[s].Replace("_", " ").Replace("to", "and") + ") THEN QTY END) as " + mq0;
                                sumallSlab += ", " + "sum(" + mq0 + ") as " + mq0;
                            }
                            toShow.Columns.Add("Others");
                            toShow.Columns.Add("Others_V");
                            if (allSlab != "")
                            {
                                allSlab = allSlab.TrimStart(',');
                                sumallSlab = sumallSlab.TrimStart(',');
                            }

                            // 10110635                                

                            DataTable dtMRR = new DataTable();
                            SQuery = "SELECT ICODE ," + sumallSlab + " FROM (SELECT ICODE, " + allSlab + " FROM (SELECT TRIM(ICODE) AS ICODE, VCHDATE, IQTYIN AS QTY FROM IVOUCHER WHERE " + branch_Cd + " AND (TYPE LIKE '0%' OR TYPE='36') AND VCHDATE BETWEEN TO_dATE('" + cDT1 + "','dd/mm/yyyy') AND TO_dATE('" + todt + "','dd/mm/yyyy') AND STORE='Y' UNION ALL SELECT TRIM(ICODE) AS ICODE, VCHDATE, IQTYIN AS QTY FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '1%' AND TYPE>'14' AND TYPE<'19' AND VCHDATE BETWEEN TO_dATE('" + cDT1 + "','dd/mm/yyyy') AND TO_dATE('" + todt + "','dd/mm/yyyy') AND STORE='Y' ) ) group by ICODE ";
                            if (frm_formID == "F10198W")
                                SQuery = "SELECT ICODE ," + sumallSlab + " FROM (SELECT ICODE, " + allSlab + " FROM (SELECT TRIM(ICODE) AS ICODE, VCHDATE, IQTYIN AS QTY FROM IVOUCHER WHERE " + branch_Cd + " AND (TYPE LIKE '3%' OR TYPE!='39') AND VCHDATE BETWEEN TO_dATE('" + cDT1 + "','dd/mm/yyyy') AND TO_dATE('" + todt + "','dd/mm/yyyy') AND STORE='Y' UNION ALL SELECT TRIM(ICODE) AS ICODE, VCHDATE, IQTYIN AS QTY FROM IVOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '1%' AND TYPE>'14' AND TYPE<'19' AND VCHDATE BETWEEN TO_dATE('" + cDT1 + "','dd/mm/yyyy') AND TO_dATE('" + todt + "','dd/mm/yyyy') AND STORE='W' ) ) group by ICODE ";
                            dtMRR = fgen.getdata(frm_qstr, co_cd, SQuery);
                            double fullQty = 0;
                            SQuery = "";
                            if (dt1.Rows.Count > 0)
                            {
                                foreach (DataRow drr in toShow.Rows)
                                {
                                    if (drr["closing_stk"].ToString().toDouble() > 0)
                                    {
                                        fullQty = drr["closing_stk"].ToString().toDouble();

                                        col1 = "";
                                        if (dtMRR.Rows.Count > 0)
                                        {
                                            col1 = fgen.seek_iname_dt(dtMRR, "ICODE='" + drr["erpcode"].ToString().TrimStart() + "'", "icode");
                                        }
                                        //if (col1.Length > 4)
                                        {
                                            col1 = "0";
                                            for (int s = 0; s < slab1.Length; s++)
                                            {
                                                if (drr["erpcode"].ToString().Trim() == "50060001")
                                                {

                                                }

                                                mq0 = "R_" + slab1[s];
                                                col1 = fgen.seek_iname_dt(dtMRR, "ICODE='" + drr["erpcode"].ToString().TrimStart() + "'", mq0);
                                                if (fullQty < col1.toDouble()) col1 = fullQty.toDouble(4).ToString();
                                                drr[mq0] = col1;
                                                drr[mq0 + "_V"] = (col1.toDouble() * drr["rm_net"].ToString().toDouble()).toDouble(4).ToString();

                                                fullQty = (fullQty - col1.toDouble()).toDouble(4);
                                                if (fullQty == 0) break;
                                            }
                                            if (fullQty > 0)
                                            {
                                                drr["OTHERS"] = fullQty.toDouble(4);
                                                drr["OTHERS_V"] = (fullQty.toDouble(4) * drr["rm_net"].ToString().toDouble()).toDouble(4).ToString();
                                            }
                                        }

                                    }
                                }
                            }
                        }


                        Session["send_dt"] = toShow;
                        fgen.exp_to_excel(toShow, "ms-excel", "xls", "Valuation " + co_cd + "_" + DateTime.Now.ToString().Trim());
                        //if (toShow.Rows.Count > 20000)
                        //{
                        //    fgen.exp_to_excel(toShow, "ms-excel", "xls", "Valuation " + co_cd + "_" + DateTime.Now.ToString().Trim());
                        //}
                        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                        //fgen.Fn_open_rptlevel("Working paper of FG / WIP / RM Valuation Report", frm_qstr);
                    }
                    else
                    {
                        #region Costing Report
                        string std_loss1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='B25' AND ENABLE_YN='Y'", "PARAMS");
                        if (fgen.make_double(std_loss1) <= 0) std_loss1 = "3";

                        dro = null;
                        if (frm_formID != "F10194" && frm_formID != "F10184" && frm_formID != "F10198")
                        {
                            oDS = new DataSet();
                            oDS = fgen.fill_schema(frm_qstr, co_cd, "extrusion");
                            foreach (DataRow fdr in fmdt.Rows)
                            {
                                dro = oDS.Tables[0].NewRow();
                                dro["BRANCHCD"] = mbr;
                                dro["type"] = "EX";
                                dro["vchnum"] = "000000";
                                dro["vchdate"] = DateTime.Now;
                                dro["icode"] = fdr["icode"].ToString().Trim();
                                dro["qty"] = Math.Round(Convert.ToDouble(fdr["val"].ToString().Trim()), 5);
                                dro["start1"] = 0;
                                dro["start2"] = 0;
                                dro["ent_by"] = uname;
                                dro["ent_dt"] = DateTime.Now;
                                if (co_cd == "NIRM" || co_cd == "DREM")
                                    dro["start2"] = fgen.make_double(fdr["jo_Val"].ToString().Trim(), 4);
                                if (co_cd == "DREM")
                                    dro["close1"] = fgen.make_double(fdr["lot_size"].ToString().Trim(), 4);
                                else dro["close1"] = 0;
                                dro["close2"] = fgen.make_double(fdr["c_cost"].ToString().Trim(), 4);
                                dro["rpm1"] = 0;
                                dro["rpm2"] = 0;
                                dro["DISPERSION1"] = 0;
                                dro["DISPERSION2"] = 0;
                                dro["srno"] = 0;
                                dro["btchdt"] = DateTime.Now;
                                dro["extloss"] = 0;
                                oDS.Tables[0].Rows.Add(dro);
                            }
                            fgen.save_data(frm_qstr, co_cd, oDS, "extrusion");
                            oDS.Dispose(); mdt.Dispose(); fmdt.Dispose();
                        }
                        SQuery = "Select distinct a.icode,b.iname as product,b.cpartno,b.unit,a.qty as val,a.start2 as job_Val,(a.qty+a.start2) as tot_value from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        if (co_cd == "DREM") SQuery = "Select distinct a.icode as erpcode,b.iname as product,b.cpartno,b.unit,a.close1 as lot_size,(a.close1 * a.qty) as matl_cost_amt,a.qty as per_pcs_cost,(a.close1 * a.start2) as process_cost_amt,a.start2 as per_pcs_proc_value,(a.close1 * a.start2) + (a.close1 * a.qty) as total_cost,(a.qty+a.start2) as per_pcs_tot_cost from extrusion a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.ent_by='" + uname.Trim() + "' and a.type='EX' order by a.icode";
                        header_n = "";
                        #region FG Valuation on BOM Cost
                        if (frm_formID == "F10184*")
                        {
                            string CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                            string CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
                            string xdt_Range = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + CDT1 + "','dd/mm/yyyy')-1";

                            string _yr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");

                            xprdrange = "between to_Date('" + CDT1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                            branch_Cd = "BRANCHCD='" + mbr + "'";

                            mq0 = "Select Closing_Stk,erpcode from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " and substr(icode,1,1)='9' union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' and substr(icode,1,1)='9' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";

                            string grossWt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR11");
                            if (grossWt == "0")
                            {
                                grossWt = "wt_Rft";
                                header_n = "[Gross Weight]";
                            }
                            else
                            {
                                grossWt = "wt_cnc";
                                header_n = "[Net Weight]";
                            }
                            SQuery = "SELECT DISTINCT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,A.QTY AS BOM_COST,C.Closing_Stk,ROUND(C.CLOSING_STK*A.QTY,3) AS FG_STK_VALUE from EXTRUSION A,ITEM B, (" + mq0 + ") C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.erpcode) AND A.BRANCHCD='" + mbr + "' AND A.ENT_BY='" + uname + "' and C.Closing_Stk!=0 ORDER BY A.ICODE ";
                            SQuery = "SELECT DISTINCT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,A.QTY AS BOM_COST,C.Closing_Stk,round(a.qty * c.closing_stk,3) as closing_value,b.wt_rft as gross_wt,b.wt_cnc as net_wt,ROUND(C.CLOSING_STK*b." + grossWt + ",3) AS STK_Wt,round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) as conv_value,ROUND(round(a.qty * c.closing_stk,3) + round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) ,3 ) as stk_value_with_conv from EXTRUSION A,ITEM B, (" + mq0 + ") C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.erpcode) AND A.BRANCHCD='" + mbr + "' AND A.ENT_BY='" + uname + "' and C.Closing_Stk!=0 " + mq5;
                        }
                        if (frm_formID == "F10194" || frm_formID == "F10184" || frm_formID == "F10198")
                        {


                            cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                            cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

                            //string _yr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");                            

                            r10 = "";
                            r10 = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
                            cond = "";
                            if (r10.Length > 2) cond = " and vchdate>=to_Date('" + r10 + "','dd/mm/yyyy')";
                            if (frm_formID == "F10184") cond = "";
                            xprd1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT1 + "','dd/mm/yyyy')-1";
                            xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1" + cond;
                            xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')" + cond;
                            //branch_Cd = "BRANCHCD='" + mbr + "'";                            

                            if (frm_formID == "F10184")
                                xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1" + cond;

                            //vipin
                            //col1 = fgen.seek_iname(frm_qstr, co_cd, "select vchdate,icode as stg,qty1 from multivch where branchcd='" + mbr + "' and type='PX' and trim(icode)='" + hfcode.Value + "'", "qty1");
                            string grossWt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR11");
                            if (grossWt == "0")
                            {
                                grossWt = "wt_Rft";
                                header_n = "[Gross Weight]";
                            }
                            else
                            {
                                grossWt = "wt_cnc";
                                header_n = "[Net Weight]";
                            }

                            mq3 = "select B.Iname as Item_Name,trim(a.Icode) as Erp_Code,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing,to_Char((sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))),'999999999.99') as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate " + xprdrange + "  and stage='" + hfcode.Value + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprd1 + " and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and type!='XX' and stage='" + hfcode.Value + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + hfcode.Value + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + hfcode.Value + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)) having sum(opening)+sum(cdr)+sum(ccr)<>0  Order by substr(a.icode,1,4),B.iname";

                            mq0 = @"select B.Iname as Item_Name,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate " + xprdrange + " and stage='" + hfcode.Value + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprdrange1 + " and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and type!='XX' and stage='" + mbr + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + hfcode.Value + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + hfcode.Value + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + hfcode.Value + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)) having sum(opening)+sum(cdr)+sum(ccr)<>0  ";

                            //mq5 = "UNION ALL SELECT ERPCODE,ITEM_NAME,CPARTNO,UNIT,RATES AS BOM_COST,Closing_Stk,WIP_VALUE,0 AS STK_WT,0 AS CONV_COST,0 AS CONV_VALUE,0 AS V1 FROM (" + mq0 + ") WHERE ERPCODE NOT IN (SELECT DISTINCT ICODE FROM EXTRUSION WHERE BRANCHCD='" + mbr + "' AND ENT_BY='" + uname + "') and Closing_Stk!=0 ";

                            SQuery = "SELECT DISTINCT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,B.UNIT,A.QTY AS BOM_COST,C.Closing_Stk,round(a.qty * c.closing_stk,3) as closing_value,ROUND(C.CLOSING_STK*b." + grossWt + ",3) AS STK_Wt,round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) as conv_value,ROUND(round(a.qty * c.closing_stk,3) + round(ROUND(C.CLOSING_STK*b." + grossWt + ",3) * a.close2) ,3 ) as stk_value_with_conv from EXTRUSION A,ITEM B, (" + mq0 + ") C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.erpcode) AND A." + branch_Cd + " AND A.ENT_BY='" + uname + "' and C.Closing_Stk!=0 " + mq5;
                            dt = new DataTable();
                            #region
                            dt.Columns.Add("Section", typeof(string));
                            dt.Columns.Add("Erpcode", typeof(string));
                            dt.Columns.Add("Product_Name", typeof(string));
                            dt.Columns.Add("Cpartno", typeof(string));
                            dt.Columns.Add("Gross_Wt", typeof(string));
                            dt.Columns.Add("Net_wt", typeof(string));
                            dt.Columns.Add("Gross_wt_pl", typeof(string));
                            dt.Columns.Add("RM_Cost", typeof(string));
                            dt.Columns.Add("RM_Net", typeof(string));
                            dt.Columns.Add("Cost_pl", typeof(string));
                            dt.Columns.Add("Unit", typeof(string));
                            dt.Columns.Add("Closing_stk", typeof(string));
                            dt.Columns.Add("Closing_value(Matl.Cost)", typeof(string));
                            dt.Columns.Add("Stk_wt", typeof(string));
                            dt.Columns.Add("Conv_cost", typeof(string));
                            dt.Columns.Add("Conv_value_on_gross", typeof(string));
                            dt.Columns.Add("Conv_value_on_net", typeof(string));
                            dt.Columns.Add("Stock_value_with_conv_GR", typeof(string));
                            dt.Columns.Add("Stock_value_with_conv_NT", typeof(string));
                            dt.Columns.Add("ChildCode", typeof(string));
                            dt.Columns.Add("ChildName", typeof(string));
                            #endregion
                            DataRow dr;
                            dt1 = new DataTable();
                            if (frm_formID == "F10184" || frm_formID == "F10198")
                            {
                                dt1.Columns.Add("stagename", typeof(string));
                                dt1.Columns.Add("code", typeof(string));

                                DataRow drr;
                                drr = dt1.NewRow();
                                drr["stagename"] = "FG Stock Valuation Report";
                                drr["code"] = "FG";
                                dt1.Rows.Add(drr);
                            }
                            else
                                dt1 = fgen.getdata(frm_qstr, co_cd, "select name as stagename,acref as code,acref from typeGRP where BRANCHCD='" + mbr + "' and ID='WI' and acref like '6%' order by type1");

                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR12") == "Y")
                            {
                                dt1 = new DataTable();
                                dt1 = fgen.getdata(frm_qstr, co_cd, "SELECT 'FG Stock Valuation Report' AS stagename,'FG' AS CODE,'FG' AS acref FROM DUAL UNION ALL select * from (select name as stagename,acref as code,acref from typeGRP where BRANCHCD='" + mbr + "' and ID='WI' and acref like '6%' order by type1)");
                            }

                            DataTable dTpurRate = (DataTable)ViewState["dt2" + frm_formID];
                            DataTable dtSaleRate = new DataTable();
                            SQuery = "Select distinct ROUND((case when a.irate>0 then A.irate else b.irate end)-(CASE WHEN A.ICHGS>0 THEN ROUND(A.IRATE * (A.ICHGS/100),2) ELSE 0 END ),2) as rate,TRIM(a.icode) AS icode,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a,item b where trim(a.icode)=trim(B.icode) and a." + branch_Cd + " and a.type like '4%' AND A.VCHDATE " + xprdrange + " and a.icode like '9%'  order by vdd desc,TRIM(a.icode)";
                            dtSaleRate = fgen.getdata(frm_qstr, co_cd, SQuery);

                            double processLoss = 0;
                            processLoss = fgen.seek_iname(frm_qstr, co_cd, "SELECT PARAMS FROM CONTROLS WHERE ID='C10' ", "PARAMS").toDouble();


                            foreach (DataRow stages in dt1.Rows)
                            {
                                dt2 = new DataTable();
                                if (frm_formID == "F10184" || frm_formID == "F10198")
                                {
                                    mq0 = "Select Closing_Stk,erpcode from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where " + branch_Cd + " and substr(icode,1,1)='9' union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' and substr(icode,1,1) in ('9','7') GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";
                                    mq0 = "Select icode as erpcode,Closing_Stk from (select a.icode,sum(a.opening) as opb,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange1 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange + " GROUP BY trim(icode) ,branchcd) a GROUP BY a.icode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)>0 )";

                                    mq0 = "SELECT A.*,B.CPARTNO,B.UNIT,B.IRATE AS RATES,b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname FROM (" + mq0 + ") A,ITEM B WHERE TRIM(A.ERPCODE)=TRIM(b.ICODE)";
                                }
                                else
                                {
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate " + xprdrange + " and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprdrange1 + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and type!='XX' and stage='" + mbr + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc having sum(a.opening)+sum(a.cDr)-sum(a.cCr)<>0  order by trim(a.Icode) ";
                                    // type added only 02 and 07 AND REMOVED 30 , 40 , 50 SERIES ITEM CODE
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type in ('02','07') or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type in ('02','07') or type in ('15x','16x')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('16/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate " + xprdrange + " and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate " + xprdrange1 + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and type!='XX' and stage='" + mbr + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' AND SUBSTR(A.ICODE,1,2) NOT IN ('30','40','50') group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(a.opening)+sum(a.cDr)-sum(a.cCr)<>0  order by trim(a.Icode) ";
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type like '0%' or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0  Order by substr(a.icode,1,4),B.iname";
                                    mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' and substr(a.icode,1,2) not in ('20','30','40') group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0  Order by substr(a.icode,1,4),B.iname";

                                    //mq0 = "select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='04' and  type='50' and vchdate  between to_Date('23/12/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')  and stage='63' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and vchdate  between to_date('01/03/2019','dd/mm/yyyy') and to_Date('31/03/2019','dd/mm/yyyy') AND VCHDATE>=TO_DATE('23/12/2018','dd/mm/yyyy') and type!='XX' and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate  between to_date('01/03/2019','dd/mm/yyyy') and to_Date('31/03/2019','dd/mm/yyyy') AND VCHDATE>=TO_DATE('23/12/2018','dd/mm/yyyy') and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='04' and (trim(acode)='XX' or stage='63') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='04' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0 and trim(a.icode)='72070022' Order by substr(a.icode,1,4),B.iname";
                                    //mq0 = "select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where branchcd='04' and (type like '0%' or type in ('15','16')) and vchdate  between to_Date('23/12/2018','dd/mm/yyyy')-365 and to_date('31/03/2019','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where branchcd='04' and  type='50' and vchdate  between to_Date('23/12/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')  and stage='63' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate  between to_date('23/12/2018','dd/mm/yyyy') and to_Date('01/03/2019','dd/mm/yyyy')-1 and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and substr(type,1,1) in ('3','1') and vchdate between to_Date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy') and vchdate>=to_Date('23/12/2018','dd/mm/yyyy') and type!='XX' and (trim(acode)='XX' or stage='63') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where branchcd='04' and type like '%' and vchdate between to_Date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy') and vchdate>=to_Date('23/12/2018','dd/mm/yyyy') and type!='XX' and stage='63' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where branchcd='04' and (trim(acode)='XX' or stage='63') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE BRANCHCD='04' AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0 and trim(a.icode)='72070022' Order by substr(a.icode,1,4),B.iname";
                                }
                                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR12") == "Y")
                                {
                                    if (stages["acref"].ToString().Trim() == "FG")
                                    {
                                        mq0 = "Select icode as erpcode,Closing_Stk from (select a.icode,sum(a.opening) as opb,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange1 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdrange + " GROUP BY trim(icode) ,branchcd) a GROUP BY a.icode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)>0 )";
                                        mq0 = "SELECT A.*,B.CPARTNO,B.UNIT,B.IRATE AS RATES,b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname FROM (" + mq0 + ") A,ITEM B WHERE TRIM(A.ERPCODE)=TRIM(b.ICODE)";
                                    }
                                    else
                                    {
                                        mq0 = @"select b.wt_rft,b.wt_cnc,B.Iname as Item_Name,B.alloy,b.mat4,b.wip_code,b.siname,trim(a.Icode) as erpcode,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cDr)-sum(a.cCr) as Closing_Stk,(sum(a.opening)+sum(a.cDr)-sum(a.cCr))*decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0))) as WIP_Value,decode(max(nvl(a.ichgs,0)),0,B.IRate,max(nvl(a.ichgs,0)) )as Rates,decode(max(nvl(a.ichgs,0)),0,'ITM_MAST','L/Cost' )as Rate_source,b.Unit,substr(a.icode,1,4) as Grp,B.CPARTNO,b.iweight,b.iweight*(sum(a.opening)+sum(a.cDr)-sum(a.cCr)) as Closing_Wt from (Select icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,ichgs from (SELECT distinct a.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos,nvl(a.ichgs,a.irate) as ichgs FROM ivoucher a inner join (SELECT icode, MAX(vchdate) max_date  FROM ivoucher where " + branch_Cd + " and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')  and (store='Y' or store='W') GROUP BY icode ) b ON  trim(a.icode) =trim(b.icode)  AND a.vchdate = b.max_Date where " + branch_Cd + " and (type in ('02','07','0U') or type in ('15x','16x')) and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy')-365 and to_date('" + todt + "','dd/mm/yyyy')   and (store='Y' or store='W') ) union all Select icode, iqtyin as opening,0 as cdr,0 as ccr,0 as clos,0 as ilcost from wipstk where " + branch_Cd + " and  type='50' and vchdate  between to_Date('" + r10 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')  and stage='" + stages["acref"].ToString().Trim() + "' union all  select icode,sum(iqtyout+(0))-sum(iqtyin) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and type!='XX' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate  between to_date('" + r10 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE union all select icode,0 as op,sum(iqtyout+(0)) as cdr,sum(iqtyin) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and substr(type,1,1) in ('3','1') and vchdate " + xprdrange + " and type!='XX' and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') and store in('Y','R') and trim(nvl(ordlineno,'-'))||substr(trim(nvl(o_Deptt,'-')),1,1)||trim(Stage)||trim(Store)!='61161R' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as ilcost from ivoucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and type!='XX' and stage='" + stages["acref"].ToString().Trim() + "' and store='W' and nvl(email_Status,'-')!='J' GROUP BY ICODE )a, item b,(SELECT DISTINCT TRIM(IBCODE) AS IBCODE FROM (SELECT DISTINCT IBCODE FROM ITEMOSP union all SELECT DISTINCT ICODE FROM ivoucher where " + branch_Cd + " and (trim(acode)='XX' or stage='" + stages["acref"].ToString().Trim() + "') UNION ALL SELECT DISTINCT ICODE FROM WIPSTK WHERE " + branch_Cd + " AND TYPE='50')) C  where 1=1 and trim(A.icode)=trim(B.icode) AND trim(A.icode)=trim(C.iBcode) and nvl(b.wipitm,'-')!='N' and substr(a.icode,1,2) not in ('20','30','40') group by B.iname,b.iweight,b.irate,b.iqd,B.CPARTNO,B.unit,trim(a.icode),substr(a.icode,1,4),decode(nvl(b.iqd,0),0,B.IRate,nvl(b.iqd,0)),b.wt_rft,b.wt_cnc,B.alloy,b.mat4,b.wip_code,b.siname having sum(opening)+sum(cdr)-sum(ccr)>0  Order by substr(a.icode,1,4),B.iname";
                                    }

                                }
                                dt2 = fgen.getdata(frm_qstr, co_cd, mq0);
                                CSR = "0";
                                double alloyrate = 0;
                                DataTable newDt2 = (DataTable)ViewState["dt2" + frm_formID];
                                foreach (DataRow dr2 in dt2.Rows)
                                {
                                    dr = dt.NewRow();
                                    dr["section"] = "Stage : " + stages["stagename"].ToString().Trim() + " [" + stages["code"].ToString().Trim() + "]";
                                    dr["erpcode"] = dr2["erpcode"].ToString().Trim();
                                    dr["Product_Name"] = dr2["item_name"].ToString().Trim();
                                    dr["cpartno"] = dr2["cpartno"].ToString().Trim();
                                    dr["unit"] = dr2["unit"].ToString().Trim();
                                    dr["Gross_Wt"] = dr2["wt_Rft"].ToString().Trim();
                                    dr["net_wt"] = dr2["wt_cnc"].ToString().Trim();
                                    col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "nt_wt");
                                    if (col1 != "0")
                                    {
                                        //sdr["Gross_Wt"] = col1;
                                        dr["net_wt"] = col1;
                                    }
                                    dr["gross_wt_pl"] = Math.Round(dr["Gross_Wt"].ToString().Trim().toDouble() * ((100 + processLoss) / 100), 3);
                                    col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "val");
                                    if (col1 != "0")
                                    {
                                        dr["RM_Cost"] = col1.toDouble(2).ToString("f");
                                        if (dr["erpcode"].ToString().Trim().Substring(0, 1) == "7" || dr["erpcode"].ToString().Trim().Substring(0, 1) == "9")
                                        {
                                            dr["RM_Net"] = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "val_nt").toDouble(2).ToString("f");

                                            if (mbr == "06" || mbr == "08")
                                                dr["RM_Net"] = (col1.toDouble(5) - (dr["Gross_Wt"].ToString().toDouble() - dr["net_wt"].ToString().toDouble()) * 0.95 * (col1.toDouble(5) * 0.3)).toDouble(2).ToString("f");
                                            else dr["RM_Net"] = (col1.toDouble(5) - (dr["Gross_Wt"].ToString().toDouble() - dr["net_wt"].ToString().toDouble()) * 0.3 * (col1.toDouble(5) * 0.3)).toDouble(2).ToString("f");
                                        }
                                    }
                                    else
                                    {
                                        col1 = fgen.seek_iname_dt(newDt2, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "rate");
                                        if (col1 != "0")
                                        {
                                            dr["RM_Cost"] = col1.toDouble(2).ToString("f");
                                            dr["RM_Net"] = col1.toDouble(2).ToString("f");
                                        }
                                        else
                                        {
                                            dr["RM_Cost"] = dr2["rates"].ToString().Trim().toDouble(2).ToString("f");
                                            dr["RM_Net"] = dr2["rates"].ToString().Trim().toDouble(2).ToString("f");
                                        }
                                    }

                                    if (co_cd == "SAGM" && (dr2["erpcode"].ToString().Trim().Substring(0, 2) == "10"))
                                    {
                                        alloyrate = 0;
                                        if (newDt2.Rows.Count > 0)
                                        {
                                            sort_view = new DataView(newDt2, "trim(icode)='" + dr2["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                            if (sort_view.Count > 0)
                                            {
                                                if (dr2["mat4"].ToString().Trim().toDouble() > 0)
                                                    alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (dr2["mat4"].ToString().Trim().toDouble() / 100);
                                                else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                dr["RM_Cost"] = alloyrate.toDouble(2).ToString("f");
                                            }
                                            else
                                            {
                                                sort_view = new DataView(newDt2, "trim(icode)='" + dr2["alloy"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                                                if (sort_view.Count > 0)
                                                {
                                                    if (dr2["mat4"].ToString().Trim().toDouble() > 0)
                                                        alloyrate = sort_view[0].Row["rate"].ToString().toDouble() * (dr2["mat4"].ToString().Trim().toDouble() / 100);
                                                    else alloyrate = sort_view[0].Row["rate"].ToString().toDouble();
                                                    dr["RM_Cost"] = alloyrate.toDouble(2).ToString("f");
                                                }
                                            }
                                        }
                                    }

                                    if (dr["RM_Cost"].ToString().toDouble() <= 0)
                                    {
                                        if (dr2["erpcode"].ToString().Trim().Substring(0, 1) == "9")
                                        {
                                            dr["RM_Cost"] = fgen.seek_iname_dt(dtSaleRate, "ICODE='" + dr2["erpcode"].ToString().Trim() + "'", "RATE").toDouble(2).ToString("f");
                                        }
                                        else dr["RM_Cost"] = fgen.seek_iname_dt(dTpurRate, "ICODE='" + dr2["erpcode"].ToString().Trim() + "'", "RATE").toDouble(2).ToString("f");
                                    }

                                    col1 = fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "c_cost");
                                    dr["conv_cost"] = col1.toDouble(2).ToString("f");

                                    //dr["cost_pl"] = Math.Round(dr["cost"].ToString().toDouble() + (dr["gross_wt_pl"].ToString().toDouble() * (dr["cost"].ToString().toDouble())), 3);
                                    if (dr["erpcode"].ToString().Trim().Substring(0, 2) == "10" || dr["erpcode"].ToString().Trim().Substring(0, 2) == "20" || dr["erpcode"].ToString().Trim().Substring(0, 2) == "30" || dr["erpcode"].ToString().Trim().Substring(0, 2) == "40")
                                        dr["cost_pl"] = "0";
                                    else
                                    {
                                        //dr["cost_pl"] = Math.Round(dr["RM_Cost"].ToString().toDouble() / .94, 2);
                                        dr["cost_pl"] = Math.Round(dr["RM_Cost"].ToString().toDouble() * ((100 + processLoss) / 100), 2).toDouble(2).ToString("f");
                                    }
                                    dr["closing_stk"] = dr2["closing_stk"].ToString().Trim().toDouble(2).ToString("f");
                                    //if (dr["cost_pl"].ToString().toDouble() <= 0)
                                    //    dr["closing_value"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * dr["RM_Cost"].ToString().Trim().toDouble(), 3);
                                    //else dr["closing_value"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * dr["cost_pl"].ToString().Trim().toDouble(), 3);

                                    if (dr["RM_Net"].ToString().toDouble() <= 0)
                                        dr["closing_value(Matl.Cost)"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * dr["RM_Cost"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");
                                    else dr["closing_value(Matl.Cost)"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * dr["RM_Net"].ToString().Trim().toDouble(), 3).toDouble(2).ToString("f");

                                    dr["stk_wt"] = Math.Round(dr["closing_stk"].ToString().Trim().toDouble() * dr["Gross_Wt"].ToString().Trim().toDouble(), 3);
                                    //dr["conv_value"] = Math.Round(dr["stk_wt"].ToString().toDouble() * col1.toDouble(), 3);

                                    dr["Conv_value_on_gross"] = Math.Round(dr["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "wt_cnc").toDouble(), 2).toDouble(2).ToString("f");
                                    dr["Conv_value_on_net"] = Math.Round(dr["closing_stk"].ToString().toDouble() * fgen.seek_iname_dt(fmdt, "ICODE='" + dr["erpcode"].ToString().Trim() + "'", "wt_rft").toDouble(), 2).toDouble(2).ToString("f");

                                    dr["stock_value_with_conv_GR"] = Math.Round(dr["closing_value(Matl.Cost)"].ToString().toDouble() + dr["Conv_value_on_gross"].ToString().toDouble(), 2).toDouble(2).ToString("f");
                                    dr["stock_value_with_conv_NT"] = Math.Round(dr["closing_value(Matl.Cost)"].ToString().toDouble() + dr["Conv_value_on_net"].ToString().toDouble(), 2).toDouble(2).ToString("f");

                                    dr["childCode"] = dr2["wip_code"].ToString().Trim();
                                    dr["childName"] = dr2["siname"].ToString().Trim();

                                    dt.Rows.Add(dr);
                                    CSR = "1";
                                }
                                if (CSR == "1")
                                {
                                    dr = dt.NewRow();
                                    dr[0] = "";
                                    dt.Rows.Add(dr);
                                }
                            }
                            SQuery = "";
                            if (dt.Rows.Count > 0)
                            {
                                dt.Columns.Remove("conv_cost");
                                dt.Columns.Remove("Cpartno");

                                dt.Columns.Remove("ChildCode");
                                dt.Columns.Remove("ChildName");
                            }

                            //foreach (DataRow drUPD in dt.Rows)
                            //{
                            //    fgen.execute_cmd(frm_qstr, co_cd, "UPDATE ITEM SET IRATE1='" + drUPD["cost"].ToString().Trim() + "' WHERE TRIM(ICODE)='" + drUPD["erpcode"].ToString().Trim() + "'");
                            //}                            

                            Session["send_dt"] = dt;
                            if (dt.Rows.Count > 50000)
                                fgen.exp_to_excel(dt, "excel", "xls", "SAGM_Rpt_" + DateTime.Now.ToString("dd_MM_yy"));
                        }
                        #endregion
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        if (frm_formID == "F10194") fgen.Fn_open_rptlevel("WIP Stock Valuation ", frm_qstr);
                        else if (frm_formID == "F10198") fgen.Fn_open_rptlevel("RM,FG Stock Ageing ", frm_qstr);
                        else fgen.Fn_open_rptlevel("Valuation Report ", frm_qstr);
                        #endregion
                    }
                    break;

                case "F10302":
                    SQuery = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(B.INAME) AS ITEM_NAME ,B.CPARTNO,B.UNIT,SUM(A.TARGET) AS TARGET_PLAN  FROM MTHLYPLAN A,ITEM B  WHERE TRIM(A.ICODE) NOT IN (SELECT DISTINCT TRIM(ICODE) AS ICODE  FROM ITWSTAGE WHERE BRANCHCD!='DD'  AND TYPE='10' ) AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='10' AND A.VCHDATE " + xprdrange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY TRIM(A.ICODE) ,TRIM(B.INAME) ,B.CPARTNO,B.UNIT   ORDER BY ICODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Plan Item not in stage mapping for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F10303":
                    SQuery = "SELECT DISTINCT TRIM(A.ICODE) AS ICODE,TRIM(B.INAME) AS ITEM_NAME ,B.CPARTNO,B.UNIT,SUM(A.TOTAL) AS VULCANSIATION  FROM PSCHEDULE  A,ITEM B  WHERE TRIM(A.ICODE) NOT IN (SELECT DISTINCT TRIM(ICODE) AS ICODE  FROM ITWSTAGE WHERE BRANCHCD!='DD'  AND TYPE='10' ) AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='15' AND A.VCHDATE " + xprdrange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY TRIM(A.ICODE) ,TRIM(B.INAME) ,B.CPARTNO,B.UNIT  ORDER BY ICODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Vulcanisation Plan Item not in stage mapping for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

            }
        }
    }
    public DataTable Gen_stk_pl_S(string xdt, string igrp) //this fun for closing summary
    {
        string frm_cocd = co_cd;
        string frm_mbr = mbr;
        double db10 = 0, db11 = 0, db12 = 0, db13 = 0;
        string frm_myear = year;

        xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
        xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

        cond = " and trim(a.icode)='10010002'";
        cond = "";
        string value1 = "";
        DataTable mrs = new DataTable(); DataRow mrrow = null; DataTable dt = new DataTable(); DataTable dt1 = new DataTable(); DataTable dt3 = new DataTable();
        DataTable dt6 = new DataTable(); DataTable dt7 = new DataTable(); DataTable dtm = new DataTable();
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
        fgen.execute_cmd(frm_qstr, frm_cocd, "Delete from itemvbal13 a where a.branchcd='" + frm_mbr + "' " + cond + "");
        opbalyr = "yr_" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_CHAR(TO_DATE(PARAMS,'DD/MM/YYYY'),'YYYY') AS params FROM CONTROLS WHERE ID='R02'", "params");
        param = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS AS params FROM CONTROLS WHERE ID='R02'", "params");
        xprd1 = "BETWEEN TO_dATE('" + param + "','dd/mm/yyyy') and to_Date('" + xdt + "','dd/mm/yyyy')-1 ";
        xprd2 = "BETWEEN TO_dATE('" + xdt + "','dd/mm/yyyy') and to_Date('" + xdt + "','dd/mm/yyyy') ";
        #region for closing
        SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' " + cond + " and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') " + cond + " and a.iqtyin>0 order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //changes by yogita
        //SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y'  and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') and substr(trim(a.icode),1,4)='0706' and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //for testing on single icode
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //this is mrr dt for only closing 
        //////////////////
        SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y' " + cond + " and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + fromdt + "','dd/mm/yyyy') " + cond + " and a.iqtyin>0 order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //changes by yogita
        //SQuery = "select a.type,a.VCHNUM,to_Char(a.VCHDATE,'dd/mm/yyyy') as VCHDATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.acode,trim(a.icode) as icode,a.iqtyin as balance,(Case when nvl(a.ichgs,0)=0 then decode(a.irate,0,b.irate,a.irate) else a.ichgs end) as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('04','09','08','0J')  and a.store='Y'  and a.vchdate>=to_DatE('" + param + "','DD/MM/YYYY') and a.vchdate<=to_DatE('" + xdt + "','dd/mm/yyyy') and substr(trim(a.icode),1,4)='0706' and a.iqtyin>0  order by trim(a.icode),a.vchdate desc,a.vchnum desc"; //for testing on single icode
        DataTable dt9 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //this is mrr dt for only opening 
        //////////////=================below is closing dt
        #endregion
        #region this is old query only for closing balance
        //mq0 = "select c.irate,trim(a.icode) as icode,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "' " + cond.Replace("a.", "") + " union all  ";      //by yogita    
        //mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='Y' " + cond.Replace("a.", "") + " GROUP BY ICODE union all ";//BY ME     
        //mq2 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + " and store='Y' " + cond.Replace("a.", "") + " GROUP BY ICODE )a, type b,item c  where trim(A.icode)=trim(c.icodE) and substr(a.icode,1,2)=b.type1 and b.id='Y' and nvl(b.rcnum,'Y')!='N' and substr(trim(a.icode),1,2)='" + mq4 + "' group by c.irate,trim(a.icode) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0  order by trim(a.icode)";
        //SQuery = mq0 + mq1 + mq2;
        #endregion
        SQuery = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing   from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal  where branchcd='" + frm_mbr + "' union all select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + "  and store='Y' GROUP BY ICODE union all  select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange + "  and store='Y' GROUP BY ICODE )a, item c  where trim(A.icode)=trim(c.icodE)  group by c.irate,trim(a.icode) /*having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0*/  order by trim(a.icode)";
        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //closing bal dt1
        ///////////////
        mq5 = "select c.irate,trim(a.icode) as icode,sum(a.opening) as opening from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "'  union all";
        mq6 = " select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + "  and store='Y' GROUP BY ICODE union all ";
        mq7 = " select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange + "  and store='Y' GROUP BY ICODE )a, type b,item c  where trim(A.icode)=trim(c.icodE) and substr(a.icode,1,2)=b.type1 and b.id='Y' and nvl(b.rcnum,'Y')!='N' group by c.irate,trim(a.icode) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>=0  order by trim(a.icode)";
        SQuery = mq5 + mq6 + mq7;
        //dt7 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for opening balance
        ////////////////
        SQuery = "select icode,iname from item where substr(trim(icode),1,2) in ('" + mq4 + "') and length(trim(icode))=4";
        DataTable dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for subgroup name and code
        SQuery = "select TYPE1 AS MGCODE,NAME AS MNAME from type where type1 IN ('" + mq4 + "') and id='Y'";
        DataTable dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for MAINGROUP name and code
        SQuery = "select distinct icode,iname,unit,irate from item where length(trim(icode))>=8 order by icode";
        DataTable dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt for main item or name
        SQuery = "select sum(iqtyin) as qty ,sum((iqtyin)*irate) as val,irate ,icode from ivoucher where branchcd='" + frm_mbr + "' and type like '0%'  and vchdate " + xprdrange + "  group by icode,irate,TYPE  ORDER BY TYPE";
        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt3 for inward qty and value
        SQuery = "select sum(iqtyin) as qty ,sum((iqtyin)*irate) as val,irate ,icode from ivoucher where branchcd='" + frm_mbr + "' and  type like '2%' OR TYPE LIKE '4%'  and vchdate " + xprdrange + "  group by icode,irate,TYPE ORDER BY TYPE";
        DataTable dt8 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //dt8 for inward qty and value
        ///////////      
        DataTable dtIvoucher = new DataTable();
        DataView dvIvoucher;
        string mhd = "";
        for (int k = 0; k < dt2.Rows.Count; k++)
        {
            #region end of foreach loop
            itot_stk = 0; to_cons = 0; itv = 0; db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; int m = 0; int n; db10 = 0; db11 = 0;
            to_cons = Convert.ToDouble(fgen.seek_iname_dt(dt1, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "closing"));
            db2 = to_cons;

            if (dt.Rows.Count > 0)
            {
                dtIvoucher = new DataTable();
                dvIvoucher = new DataView(dt, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                if (dvIvoucher.Count > 0)
                    dtIvoucher = dvIvoucher.ToTable();
            }

            foreach (DataRow stk_chk in dtIvoucher.Rows) //ivoucher dt
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
                dr3["mcode"] = mq4;
                dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                dr3["clos_val"] = db1;
                dr3["clos_qty"] = db2;
                dr3["avg_rate"] = db1 / db2;
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
                    dr3["mcode"] = mq4;
                    dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                    dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                    dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                    dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                    if (db1 <= 0)
                    {
                        db1 = dt2.Rows[k]["IRATE"].ToString().toDouble();
                        db1 = db1 * db2;
                    }
                    dr3["clos_val"] = db1;
                    dr3["clos_qty"] = db2;
                    dr3["avg_rate"] = db1 / db2;
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
            to_cons = Convert.ToDouble(fgen.seek_iname_dt(dt1, "icode='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "opening"));
            db9 = to_cons;

            if (dt9.Rows.Count > 0)
            {
                dtIvoucher = new DataTable();
                dvIvoucher = new DataView(dt9, "ICODE='" + dt2.Rows[k]["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                if (dvIvoucher.Count > 0)
                    dtIvoucher = dvIvoucher.ToTable();
            }

            foreach (DataRow stk_chk in dtIvoucher.Rows) //ivoucher dt
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
                        dr3["op_value"] = db7;
                        dr3["fromdt"] = fromdt;
                        dr3["todt"] = todt;
                        dr3["header"] = header_n;
                        dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                        dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                        dr3["mcode"] = mq4;
                        dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                        dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                        dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                        dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
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
                    dr3["op_value"] = db7;
                    dr3["fromdt"] = fromdt;
                    dr3["todt"] = todt;
                    dr3["header"] = header_n;
                    dr3["icode"] = dt2.Rows[k]["icode"].ToString().Trim();
                    dr3["iname"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "iname");
                    dr3["mcode"] = mq4;
                    dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                    dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                    dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                    dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
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
                    dr3["mcode"] = mq4;
                    dr3["mname"] = fgen.seek_iname_dt(dt5, "MGCODE='" + dr3["mcode"].ToString().Trim() + "'", "mname");
                    dr3["scode"] = dr3["icode"].ToString().Substring(0, 4);
                    dr3["sname"] = fgen.seek_iname_dt(dt4, "icode='" + dr3["scode"].ToString().Trim() + "'", "iname");
                    dr3["unit"] = fgen.seek_iname_dt(dt2, "icode='" + dr3["icode"].ToString().Trim() + "'", "unit");
                    dr3["inw_Qty"] = db4;
                    dr3["inw_val"] = db5;
                    dr3["out_qty"] = db10;
                    dr3["out_val"] = db11;
                    db12 = db9 + db4 - (db5 + db2); //op+inw qty-(inw val
                    db13 = db7 + db5 - (db10 + db1);
                    dr3["cons_qty"] = db12;
                    dr3["cons_val"] = db13;
                    ph_tbl.Rows.Add(dr3);
                }
                #endregion
            }
        }
            #endregion end of op loop


        dt2 = new DataTable();
        dt2.Columns.Add("BRANCHCD", typeof(string));
        dt2.Columns.Add("ICODE", typeof(string));
        dt2.Columns.Add("RATE", typeof(string));
        dt2.Columns.Add("ACODE", typeof(string));
        dt2.Columns.Add("VCHNUM", typeof(string));
        dt2.Columns.Add("VCHDATE", typeof(string));
        dt2.Columns.Add("TYPE", typeof(string));
        dt2.Columns.Add("VDD", typeof(string));
        DataRow dr2;

        foreach (DataRow pr in ph_tbl.Rows)
        {
            dr2 = dt2.NewRow();
            dr2["BRANCHCD"] = mbr;
            dr2["RATE"] = pr["avg_rate"].ToString().toDouble(4);
            dr2["ICODE"] = pr["ICODE"].ToString();
            dr2["ACODE"] = "-";
            dr2["VCHNUM"] = "-";
            dr2["VCHDATE"] = "-";
            dr2["TYPE"] = "-";
            dr2["VDD"] = "1";
            dt2.Rows.Add(dr2);
        }
        return dt2;
    }

}