using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_prodpm : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR, ptype;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joincond;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID, party_cd, part_cd;
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
                ptype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MLD_PTYPE");
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
                case "89554":
                case "F60121":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F39225":
                    SQuery = "SELECT TRIM(TYPE1) AS FSTR,NAME AS SHIFT,TYPE1 AS CODE,place as shft_min,round(case when place='-' then 1 when place>0 then place/60 else 0 end) as shft_hrs FROM TYPE WHERE ID='D' AND TYPE1 LIKE '1%' ORDER BY code";
                    header_n = "Select Shift";
                    break; ;

                case "F39228":
                case "F39232":
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                    header_n = "Select Month";
                    break;
                case "F39240":
                    SQuery = "select distinct trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,trim(vchnum)||trim(naration) as vch_ref,to_char(vchdate,'dd/mm/yyyy') as vchdate,var_Code as shift,type  from prod_sheet where branchcd='" + mbr + "' and type='" + ptype + "' and vchdate " + xprdrange + " order by vch_ref desc";
                    header_n = "Select Production Entry";
                    break;
                case "F39152":
                    if (co_cd == "AGRM")
                    {
                        SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.shftcode) as fstr,  a.vchnum AS PLAN_NO,to_char(a.vchdate,'dd/mm/yyyy') as PLAN_DT,b.name as depttname,a.shftcode,c.name as shift  from prod_sheet a,type c ,type b where a.branchcd='" + mbr + "' and a.type='11' and trim(a.acode)=trim(B.type1) and b.id='1' and a.vchdate " + xprdrange + "  and trim(a.shftcode)=trim(c.type1) and c.id='D' ORDER BY PLAN_NO DESC";
                    }
                    else
                    {
                        SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.shftcode) as fstr,  a.vchnum AS PLAN_NO,to_char(a.vchdate,'dd/mm/yyyy') as PLAN_DT,a.shftcode,c.name as shift  from prod_sheet a,type c where a.branchcd='" + mbr + "' and a.type='12' and a.vchdate " + xprdrange + "  and trim(a.shftcode)=trim(c.type1) and c.id='D' ORDER BY PLAN_NO DESC";
                    }
                    header_n = "Select Shift";
                    break;
                case "F39251":
                case "F39255":
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F39225")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F39228":
                    case "F39232":
                    case "F39240":
                    case "F39152":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_prodpm_reps(frm_qstr);
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
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

            tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4)||'@'||trim(join_cond) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                table1 = tbl_flds.Split('@')[2].ToString();
                table2 = tbl_flds.Split('@')[3].ToString();
                table3 = tbl_flds.Split('@')[4].ToString();
                table4 = tbl_flds.Split('@')[5].ToString();
                joincond = tbl_flds.Split('@')[6].ToString();
                sortfld = sortfld.Replace("`", "'");
                rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            // after prdDmp this will run            
            switch (val)
            {
                case "F39152":
                case "F39223":
                case "F39224":
                case "F39225":
                case "F39226":
                case "F39227":
                case "F39230":
                case "F39231":
                case "F39144":
                case "F39141":
                // Made by Madhvi on 03 Aug 2018
                case "F39233":
                case "F39234":
                case "F39235":
                case "F39236":
                case "F39237":
                case "F39238"://yogita 03.08.18
                case "F39239"://yogita 04.08.18
                //case "F39240":
                case "F39241":
                case "F39242":
                case "F39243":
                case "F39245":
                case "F39246":
                case "F39247":
                case "F39240":
                case "F39248":
                case "F39249":
                case "F39250":
                case "F39252":
                case "F39190":
                case "F39192":
                case "F39255":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_prodpm_reps(frm_qstr);
                    break;
                case "F39251":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (party_cd == "")
                    { fgen.msg("Alert", co_cd, "Please select MRR to run report!!"); }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_prodpm_reps(frm_qstr);
                    }
                    break;
                case "F39275":
                    #region
                    DataTable dtm = new DataTable();
                    SQuery = "select trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy') as fstr,trim(a.icode) as icode,trim(b.iname) as iname,b.no_proc as guage ,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.manpwr,nvl(a.qty,0) as output_qty, a.col23 as shift,a.col25 as machine  from costestimatek a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type='40' and a.col21='64' and A.VCHDATE " + xprdrange + " ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//main dt

                    mq0 = "select distinct trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy') as fstr, a.col24 as start_time,a.col25 as end_time from costestimatek a where a.branchcd='" + mbr + "'  and a.type='25' and a.col21='64' and a.vchdate " + xprdrange + "";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);//for start time and end time

                    mq0 = "select  trim(a.branchcd)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.job_no)||a.job_dt as fstr,a.mchcode,a.prevstage as linecode  from prod_sheetk a where a.branchcd='" + mbr + "' and a.type='86' AND a.acode='64' AND a.vchdate " + xprdrange + "";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq0);//prodsheet .

                    mq0 = "select  ICODE,QTY1 AS MANPOWER,QTY2 AS HRLY_PROD,COL3 AS MCHCODE from multivch where branchcd='" + mbr + "' and type='/D'";
                    dt3 = new DataTable();//dt for STD_UPPH value............STD_UPPH=qty1*qty2
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq0); //machine master.....

                    dtm.Columns.Add("sno", typeof(string));
                    dtm.Columns.Add("Entry_No", typeof(string));
                    dtm.Columns.Add("Entry_Dt", typeof(string));
                    dtm.Columns.Add("Shift", typeof(string));
                    dtm.Columns.Add("Line_No", typeof(string));
                    dtm.Columns.Add("Part_Name", typeof(string));
                    dtm.Columns.Add("Guage", typeof(string));
                    dtm.Columns.Add("Resource_Name", typeof(string)); //machine name
                    dtm.Columns.Add("ManPower_Used", typeof(double));
                    dtm.Columns.Add("Actual_Working_hrs", typeof(string));//end time-start time
                    dtm.Columns.Add("Ok_Production_hrs", typeof(double));
                    dtm.Columns.Add("UPPH", typeof(double));//units produces per manpower hr.............qty/mancount*?
                    dtm.Columns.Add("STD_UPPH", typeof(double));
                    dtm.Columns.Add("Production_Efficiency", typeof(double));
                    DataRow dr1;

                    if (dt.Rows.Count > 0)
                    {
                        string fstr = ""; TimeSpan dd = TimeSpan.Zero; TimeSpan ts;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            double db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0;
                            mq1 = ""; mq2 = ""; fstr = ""; mq3 = "";
                            mq4 = ""; mq5 = "";
                            fstr = dt.Rows[i]["fstr"].ToString().Trim();
                            mq1 = fgen.seek_iname_dt(dt1, "fstr='" + fstr + "'", "start_time");
                            mq2 = fgen.seek_iname_dt(dt1, "fstr='" + fstr + "'", "end_time");
                            mq3 = fgen.seek_iname_dt(dt2, "fstr='" + fstr + "'", "linecode");
                            mq4 = fgen.seek_iname_dt(dt2, "fstr='" + fstr + "'", "mchcode");
                            mq5 = dt.Rows[i]["icode"].ToString().Trim().Substring(0, 4);

                            if (Convert.ToDateTime(mq2) > Convert.ToDateTime(mq1))
                            {
                                dd = Convert.ToDateTime(mq2) - Convert.ToDateTime(mq1);
                            }
                            else
                            {
                                TimeSpan time1 = TimeSpan.FromHours(24);
                                ts = Convert.ToDateTime(mq2) - Convert.ToDateTime(mq1);
                                dd = ts.Add(time1);
                            }
                            dr1 = dtm.NewRow();
                            dr1["sno"] = i + 1;
                            dr1["Entry_No"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            dr1["Entry_Dt"] = dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["Shift"] = dt.Rows[i]["shift"].ToString().Trim();
                            dr1["Line_No"] = mq3;
                            dr1["Part_Name"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["Guage"] = dt.Rows[i]["guage"].ToString().Trim();
                            dr1["Resource_Name"] = dt.Rows[i]["machine"].ToString().Trim();
                            dr1["ManPower_Used"] = dt.Rows[i]["manpwr"].ToString().Trim();
                            dr1["Actual_Working_hrs"] = dd;
                            dr1["Ok_Production_hrs"] = fgen.make_double(dt.Rows[i]["output_qty"].ToString().Trim());
                            db1 = fgen.make_double(dr1["Ok_Production_hrs"].ToString().Trim());
                            db2 = fgen.make_double(dr1["ManPower_Used"].ToString().Trim());
                            if (db1 > 0)
                            {
                                db3 = db2 * dd.TotalHours;
                                db8 = db1 / db3;
                                dr1["UPPH"] = Math.Round(db8, 2);
                            }
                            else
                            {
                                dr1["UPPH"] = 0;
                            }

                            db6 = fgen.make_double(fgen.seek_iname_dt(dt3, "MCHCODE='" + mq4 + "' and icode='" + mq5 + "'", "HRLY_PROD"));
                            dr1["STD_UPPH"] = db6;

                            db4 = fgen.make_double(dr1["STD_UPPH"].ToString().Trim());
                            if (db4 > 0)
                            {
                                dr1["Production_Efficiency"] = Math.Round(db8 / db4 * 100, 2);
                            }
                            else
                            {
                                dr1["Production_Efficiency"] = 0;
                            }
                            dtm.Rows.Add(dr1);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("ASSY LINE PRODUCTION,MIS Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;
            }
        }
    }
}