using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_prod : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, dt4, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv, db1, db2, db, db3; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
    DataRow dr1;
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

                cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
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
                case "F75197":
                case "F75160":
                case "F75158":
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                    header_n = "Select Month";
                    break;

                //made and merged by yogita
                case "F75185":
                case "F75186":
                case "F75187":
                case "F75206":

                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;

                case "F75176": // Tool Wise Month Wise Shot Details only date range to be asked
                    dt = new DataTable();
                    dt.Columns.Add("SERIAL_NO", typeof(string));
                    dt.Columns.Add("TYPE", typeof(string));
                    dt.Columns.Add("MOULD_CODE", typeof(string));
                    dt.Columns.Add("PARTNO", typeof(string));

                    dt.Columns.Add("MODEL_NAME", typeof(string));
                    dt.Columns.Add("PART_NAME", typeof(string));
                    dt.Columns.Add("MOULD_ID_NO", typeof(string));
                    dt.Columns.Add("PLASTIC_RAW_MATERIAL", typeof(string));
                    dt.Columns.Add("FIRST_HM_COUNT", typeof(string));
                    dt.Columns.Add("FREQ_PM", typeof(string));
                    dt.Columns.Add("FREQ_HM", typeof(string));

                    dt.Columns.Add("APRIL", typeof(string));
                    dt.Columns.Add("MAY", typeof(string));
                    dt.Columns.Add("JUNE", typeof(string));
                    dt.Columns.Add("JULY", typeof(string));
                    dt.Columns.Add("AUGUST", typeof(string));
                    dt.Columns.Add("SEPTEMBER", typeof(string));
                    dt.Columns.Add("OCTOBER", typeof(string));
                    dt.Columns.Add("NOVEMBER", typeof(string));
                    dt.Columns.Add("DECEMBER", typeof(string));
                    dt.Columns.Add("JANUARY", typeof(string));
                    dt.Columns.Add("FEBRUARY", typeof(string));
                    dt.Columns.Add("MARCH", typeof(string));
                    dt.Columns.Add("TOTAL", typeof(string));
                    dt.Columns.Add("SHOT_TILL_ACQISTION", typeof(string));
                    dt.Columns.Add("CO_OPENING_SHOT", typeof(string));
                    dt.Columns.Add("CUMMULATIVE_SHOT_UPTO_LASTYR", typeof(string));
                    dt.Columns.Add("GRAND_TOTAL", typeof(string));

                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select opt_start from fin_rsys_opt_pw where branchcd='" + mbr + "' and opt_id='W1078' and opt_enable='Y'", "opt_start");
                    if (mq0.Length > 1)
                    {
                        mq1 = "";
                        if (Convert.ToDateTime(mq0) < Convert.ToDateTime(cDT1))
                        {
                            mq1 = cDT1;
                        }
                        else
                        {
                            mq1 = mq0;
                        }

                        SQuery = "select b.col1 as type,a.pvchnum as mould_code,b.cpartno as partno,c.name as model_name,b.col4 as part_name,b.col9 as mould_id_no,b.col14 as Plastic_raw_material,b.num13 as first_hm_count,b.num6 as freq_pm,b.col15 as freq_hm ,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun ) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb ,sum(a.mar) as mar, sum(a.apr)+sum(a.may)+sum(a.jun )+sum(a.jul)+sum(a.aug)+sum(a.sep)+sum(a.oct)+sum(a.nov)+sum(a.dec)+sum(a.jan)+sum(a.feb)+sum(a.mar) as total,b.num7 as shot_till_acqistion,b.num10 as co_opening_shot from (select pvchnum,branchcd,type, decode(to_char(vchdate,'mm'),'04',(iqtyin+mlt_loss)*fm_fact,'0') as apr,decode(to_char(vchdate,'mm'),'05',(iqtyin+mlt_loss)*fm_fact,'0') as may,decode(to_char(vchdate,'mm'),'06',(iqtyin+mlt_loss)*fm_fact,'0') as jun,decode(to_char(vchdate,'mm'),'07',(iqtyin+mlt_loss)*fm_fact,'0') as jul,decode(to_char(vchdate,'mm'),'08',(iqtyin+mlt_loss)*fm_fact,'0') as aug,decode(to_char(vchdate,'mm'),'09',(iqtyin+mlt_loss)*fm_fact,'0') as sep,decode(to_char(vchdate,'mm'),'10',(iqtyin+mlt_loss)*fm_fact,'0') as oct,decode(to_char(vchdate,'mm'),'11',(iqtyin+mlt_loss)*fm_fact,'0') as nov,decode(to_char(vchdate,'mm'),'12',(iqtyin+mlt_loss)*fm_fact,'0') as dec,decode(to_char(vchdate,'mm'),'01',(iqtyin+mlt_loss)*fm_fact,'0') as jan,decode(to_char(vchdate,'mm'),'02',(iqtyin+mlt_loss)*fm_fact,'0') as feb,decode(to_char(vchdate,'mm'),'03',(iqtyin+mlt_loss)*fm_fact,'0') as mar from prod_sheet  where branchcd='" + mbr + "' and type='90' and vchdate between to_date('" + mq1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy') ) a , wb_master b ,typegrp c where trim(a.pvchnum) =trim(b.col5) and  trim(upper(b.col5))||trim(b.branchcd)=trim(upper(c.acref))||trim(c.branchcd) and a.branchcd='" + mbr + "' and b.id='MM01' and c.id='MM' and nvl(b.col2,'-')!='Y' group by a.pvchnum,b.cpartno,c.name,b.col4,b.col9,b.col14,b.num6 ,b.col15,b.num7,b.num10,b.col1,b.num13,a.branchcd";
                        SQuery = "select trim(b.col1) as type,trim(a.pvchnum) as mould_code,trim(b.col5) as partno,trim(c.name) as model_name,trim(b.col4) as part_name,trim(b.col9) as mould_id_no,trim(b.col14) as Plastic_raw_material,b.num13 as first_hm_count,b.num6 as freq_pm,to_number(b.col15) as freq_hm ,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun ) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb ,sum(a.mar) as mar, sum(a.apr)+sum(a.may)+sum(a.jun )+sum(a.jul)+sum(a.aug)+sum(a.sep)+sum(a.oct)+sum(a.nov)+sum(a.dec)+sum(a.jan)+sum(a.feb)+sum(a.mar) as total,b.num7 as shot_till_acqistion,b.num10 as co_opening_shot from (select pvchnum,branchcd,type, decode(to_char(vchdate,'mm'),'04',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as apr,decode(to_char(vchdate,'mm'),'05',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as may,decode(to_char(vchdate,'mm'),'06',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as jun,decode(to_char(vchdate,'mm'),'07',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as jul,decode(to_char(vchdate,'mm'),'08',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as aug,decode(to_char(vchdate,'mm'),'09',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as sep,decode(to_char(vchdate,'mm'),'10',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as oct,decode(to_char(vchdate,'mm'),'11',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as nov,decode(to_char(vchdate,'mm'),'12',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as dec,decode(to_char(vchdate,'mm'),'01',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as jan,decode(to_char(vchdate,'mm'),'02',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as feb,decode(to_char(vchdate,'mm'),'03',(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0),'0') as mar from prod_sheet  where branchcd='" + mbr + "' and type='90' and vchdate between to_date('" + mq1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy') ) a , wb_master b ,typegrp c where trim(a.pvchnum) =trim(b.cpartno) and  trim(upper(b.cpartno))||trim(b.branchcd)=trim(upper(c.acref))||trim(c.branchcd) and a.branchcd='" + mbr + "' and b.id='MM01' and c.id='MM' and nvl(b.col2,'-')!='Y' group by trim(b.col1),trim(a.pvchnum) ,trim(b.col5) ,trim(c.name) ,trim(b.col4) ,trim(b.col9) ,trim(b.col14) ,b.num13 ,b.num6 ,to_number(b.col15) ,b.num7 ,b.num10 ";
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);

                        if (dt1.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                dr1 = dt.NewRow();
                                dr1["SERIAL_NO"] = i + 1;
                                dr1["TYPE"] = dt1.Rows[i]["TYPE"].ToString().Trim();
                                dr1["MOULD_CODE"] = dt1.Rows[i]["MOULD_CODE"].ToString().Trim();
                                dr1["PARTNO"] = dt1.Rows[i]["PARTNO"].ToString().Trim();
                                dr1["MODEL_NAME"] = dt1.Rows[i]["MODEL_NAME"].ToString().Trim();
                                dr1["PART_NAME"] = dt1.Rows[i]["PART_NAME"].ToString().Trim();
                                dr1["MOULD_ID_NO"] = dt1.Rows[i]["MOULD_ID_NO"].ToString().Trim();
                                dr1["PLASTIC_RAW_MATERIAL"] = dt1.Rows[i]["PLASTIC_RAW_MATERIAL"].ToString().Trim();
                                dr1["FIRST_HM_COUNT"] = dt1.Rows[i]["FIRST_HM_COUNT"].ToString().Trim();
                                dr1["FREQ_PM"] = dt1.Rows[i]["FREQ_PM"].ToString().Trim();
                                dr1["FREQ_HM"] = dt1.Rows[i]["FREQ_HM"].ToString().Trim();

                                dr1["APRIL"] = dt1.Rows[i]["APR"].ToString().Trim();
                                dr1["MAY"] = dt1.Rows[i]["MAY"].ToString().Trim();
                                dr1["JUNE"] = dt1.Rows[i]["JUN"].ToString().Trim();
                                dr1["JULY"] = dt1.Rows[i]["JUL"].ToString().Trim();
                                dr1["AUGUST"] = dt1.Rows[i]["AUG"].ToString().Trim();
                                dr1["SEPTEMBER"] = dt1.Rows[i]["SEP"].ToString().Trim();
                                dr1["OCTOBER"] = dt1.Rows[i]["OCT"].ToString().Trim();
                                dr1["NOVEMBER"] = dt1.Rows[i]["NOV"].ToString().Trim();
                                dr1["DECEMBER"] = dt1.Rows[i]["DEC"].ToString().Trim();
                                dr1["JANUARY"] = dt1.Rows[i]["JAN"].ToString().Trim();
                                dr1["FEBRUARY"] = dt1.Rows[i]["FEB"].ToString().Trim();
                                dr1["MARCH"] = dt1.Rows[i]["MAR"].ToString().Trim();
                                dr1["TOTAL"] = dt1.Rows[i]["TOTAL"].ToString().Trim();
                                dr1["SHOT_TILL_ACQISTION"] = dt1.Rows[i]["SHOT_TILL_ACQISTION"].ToString().Trim();
                                dr1["CO_OPENING_SHOT"] = dt1.Rows[i]["CO_OPENING_SHOT"].ToString().Trim();

                                if (Convert.ToDateTime(mq0) > Convert.ToDateTime(cDT1))
                                {
                                    dr1["CUMMULATIVE_SHOT_UPTO_LASTYR"] = "0";
                                }
                                else
                                {
                                    SQuery = "select pvchnum,branchcd,type, sum((iqtyin+mlt_loss)*fm_fact) as prodn from prod_sheet  where branchcd='" + mbr + "' and type='90' and vchdate < to_date('" + mq0 + "','dd/mm/yyyy') and vchdate > to_date('" + cDT1 + "','dd/mm/yyyy')-1 group by pvchnum,branchcd,type";
                                    dt2 = new DataTable();
                                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                                    mq3 = fgen.seek_iname_dt(dt2, "pvchnum='" + dt1.Rows[i]["mould_code"].ToString().Trim() + "'", "prodn");
                                    dr1["CUMMULATIVE_SHOT_UPTO_LASTYR"] = mq3;
                                }

                                dr1["GRAND_TOTAL"] = fgen.make_double(dr1["TOTAL"].ToString()) + fgen.make_double(dr1["SHOT_TILL_ACQISTION"].ToString()) + fgen.make_double(dr1["CO_OPENING_SHOT"].ToString()) + fgen.make_double(dr1["CUMMULATIVE_SHOT_UPTO_LASTYR"].ToString());
                                dt.Rows.Add(dr1);
                            }
                        }
                        SQuery = ""; // IT IS SET TO DASH SO THAT IT DOES NOT GO THIS LINE "if (SQuery.Length > 1)"
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = dt;
                        fgen.Fn_open_rptlevelJS("Mould Wise Month Wise Shot Details For the Period : " + mq0 + " to " + cDT2, frm_qstr);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Start Date for Mould Maintenance is not entered in plant config.");
                    }
                    break;

                case "F75188":
                    SQuery = "select trim(type1) as type1,name as mould,acref as mould_code,p_Acode as partycode,p_icode as itemcode,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt from typegrp where branchcd='" + mbr + "' and id='MM' and trim(type1) not in (select trim(col1) from wb_master where branchcd='" + mbr + "' and id='MM01' and nvl(col2,'-')!='Y') order by mould";
                    header_n = "Moulds Which Are In Main Master But Their Detailed Specification Is Not Entered.";
                    break;

                case "F75191":
                case "F75192":
                case "F75193":
                    fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);
                    break;

                case "F75209":
                    SQuery = "select trim(a.col1)||trim(a.plan_month)||trim(a.plan_date) as fstr,c.name AS MOULD,c.acref AS MOULD_no,trim(a.col1) as CODE,a.plan_date,a.plan_month from (select distinct col1,to_char(date1,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM02' and nvl(trim(grade),'-')!='Y' union all select distinct col1,to_char(date2,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,-1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM04') a ,wb_master b,typegrp c where trim(a.col1)=trim(b.col1) and b.id='MM01' and trim(a.col1)=trim(c.type1) and b.branchcd='" + mbr + "' and c.branchcd='" + mbr + "' and c.id='MM' AND NVL(B.COL2,'-')!='Y'  group by c.name,c.acref,b.num12,trim(a.col1),b.col7,a.plan_date,a.plan_month,c.provision having sum(qty)>0 order by mould";
                    header_n = "Mould Preventive Planned but not Maintained";
                    break;

                case "F75210":
                    SQuery = "select trim(a.col1)||trim(a.plan_month)||trim(a.plan_date) as fstr,c.name AS MOULD,c.acref AS MOULD_no,trim(a.col1) as CODE,a.plan_date,a.plan_month from (select distinct col1,to_char(date1,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM03' and nvl(trim(grade),'-')!='Y' union all select distinct col1,to_char(date2,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,-1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM05') a ,wb_master b,typegrp c where trim(a.col1)=trim(b.col1) and b.id='MM01' and trim(a.col1)=trim(c.type1) and b.branchcd='" + mbr + "' and c.branchcd='" + mbr + "' and c.id='MM' AND NVL(B.COL2,'-')!='Y'  group by c.name,c.acref,b.num15,trim(a.col1),b.col8,a.plan_date,a.plan_month having sum(qty)>0 order by mould";
                    header_n = "Mould Health Planned but not Maintained";
                    break;
                //21april2020===made by yogita
                case "F75126":
                    SQuery = "SELECT 'PEND' AS FSTR,'PENDING' AS SELECTION,'PENDING' AS CHOICE FROM DUAL UNION ALL SELECT 'CLOS' AS FSTR,'CLOSED' AS SELECTION,'CLOSED' AS CHOICE FROM DUAL UNION ALL SELECT 'ALL' AS FSTR,'ALL' AS SELECTION,'ALL' AS CHOICE FROM DUAL";
                    break;
            }

            if (SQuery.Length > 1)
            {
                if (HCID == "F75176" || HCID == "F75188")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n, frm_qstr);
                }
                else if (HCID == "F75209" || HCID == "F75210")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                }
                else
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127" || val == "F75126")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    case "F75197":
                        mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                        if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                        {

                        }
                        else { year = (Convert.ToInt32(year) + 1).ToString(); }
                        mq1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                        SQuery = "select a.col1 as mould_part_no,b.name as mould_name,a.col5 as mould_id,a.col14 as material_used,a.num15 as fst_hlth_mon,a.num5 as hlth_checkup_as_per_shot,'' as cummulative_shots_upto_today,'' as total_shot_previous_hlth_chk,'' as previous_hlth_doneDt from wb_master a , typegrp b where trim(a.col1)=trim(b.type1) and a.branchcd='" + mbr + "' and a.id='MM01' and b.id='MM' and nvl(a.col2,'-')!='Y' and to_char(a.vchdate,'mm/yyyy')='" + mq0 + "/" + year + "'";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Mould Health Checklist For the Month : " + mq1 + "/" + year + " ", frm_qstr);
                        break;

                    case "F75160":
                        mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                        mq1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                        if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                        {

                        }
                        else { year = (Convert.ToInt32(year) + 1).ToString(); }
                        SQuery = "select distinct  a.icode as code,b.name as mould_name ,decode(to_char(a.date2,'dd'),'01','P','') as day1,decode(to_char(a.date2,'dd'),'02','P','') as day2,decode(to_char(a.date2,'dd'),'03','P','') as day3,decode(to_char(a.date2,'dd'),'04','P','') as day4,decode(to_char(a.date2,'dd'),'05','P','') as day5,decode(to_char(a.date2,'dd'),'06','P','') as day6,decode(to_char(a.date2,'dd'),'07','P','') as day7,decode(to_char(a.date2,'dd'),'08','P','') as day8,decode(to_char(a.date2,'dd'),'09','P','') as day9,decode(to_char(a.date2,'dd'),'10','P','') as day10,decode(to_char(a.date2,'dd'),'11','P','') as day11,decode(to_char(a.date2,'dd'),'12','P','') as day12,decode(to_char(a.date2,'dd'),'13','P','') as day13,decode(to_char(a.date2,'dd'),'14','P','') as day14,decode(to_char(a.date2,'dd'),'15','P','') as day15,decode(to_char(a.date2,'dd'),'16','P','') as day16,decode(to_char(a.date2,'dd'),'17','P','') as day17,decode(to_char(a.date2,'dd'),'18','P','') as day18,decode(to_char(a.date2,'dd'),'19','P','') as day19,decode(to_char(a.date2,'dd'),'20','P','') as day20,decode(to_char(a.date2,'dd'),'21','P','') as day21,decode(to_char(a.date2,'dd'),'22','P','') as day22,decode(to_char(a.date2,'dd'),'23','P','') as day23,decode(to_char(a.date2,'dd'),'24','P','') as day24,decode(to_char(a.date2,'dd'),'25','P','') as day25,decode(to_char(a.date2,'dd'),'26','P','') as day26,decode(to_char(a.date2,'dd'),'27','P','') as day27,decode(to_char(a.date2,'dd'),'28','P','') as day28,decode(to_char(a.date2,'dd'),'29','P','') as day29,decode(to_char(a.date2,'dd'),'30','P','') as day30,decode(to_char(a.date2,'dd'),'31','P','') as day31 from wb_maint a ,typegrp b where trim(a.icode)=trim(b.acref) and a.branchcd='" + mbr + "' and a.type='MM02' and b.id='MM' and to_char(a.vchdate,'mm/yyyy')='" + mq0 + "/" + year + "'";
                        SQuery = "select distinct a.COL1 as code,b.name as mould_name,B.ACREF AS MOULD_CODE ,decode(to_char(date1,'dd'),'01','P','') as day1,decode(to_char(date1,'dd'),'02','P','') as day2,decode(to_char(date1,'dd'),'03','P','') as day3,decode(to_char(date1,'dd'),'04','P','') as day4,decode(to_char(date1,'dd'),'05','P','') as day5,decode(to_char(date1,'dd'),'06','P','') as day6,decode(to_char(date1,'dd'),'07','P','') as day7,decode(to_char(date1,'dd'),'08','P','') as day8,decode(to_char(date1,'dd'),'09','P','') as day9,decode(to_char(date1,'dd'),'10','P','') as day10,decode(to_char(date1,'dd'),'11','P','') as day11,decode(to_char(date1,'dd'),'12','P','') as day12,decode(to_char(date1,'dd'),'13','P','') as day13,decode(to_char(date1,'dd'),'14','P','') as day14,decode(to_char(date1,'dd'),'15','P','') as day15,decode(to_char(date1,'dd'),'16','P','') as day16,decode(to_char(date1,'dd'),'17','P','') as day17,decode(to_char(date1,'dd'),'18','P','') as day18,decode(to_char(date1,'dd'),'19','P','') as day19,decode(to_char(date1,'dd'),'20','P','') as day20,decode(to_char(date1,'dd'),'21','P','') as day21,decode(to_char(date1,'dd'),'22','P','') as day22,decode(to_char(date1,'dd'),'23','P','') as day23,decode(to_char(date1,'dd'),'24','P','') as day24,decode(to_char(date1,'dd'),'25','P','') as day25,decode(to_char(date1,'dd'),'26','P','') as day26,decode(to_char(date1,'dd'),'27','P','') as day27,decode(to_char(date1,'dd'),'28','P','') as day28,decode(to_char(date1,'dd'),'29','P','') as day29,decode(to_char(date1,'dd'),'30','P','') as day30,decode(to_char(date1,'dd'),'31','P','') as day31 from wb_maint a ,typegrp b where trim(a.COL1)=trim(b.TYPE1) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) and a.branchcd='" + mbr + "' and a.type='MM02' and b.id='MM' and to_char(a.vchdate,'mm/yyyy')='" + mq0 + "/" + year + "' ORDER BY MOULD_NAME";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Mould PM Plan Checklist For the Month : " + mq1 + "/" + year + " ", frm_qstr);
                        break;

                    case "F75158":
                        mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); //SELECTED MONTH
                        mq1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + mq0 + "'", "MTHNAME");
                        if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                        { }
                        else { year = (Convert.ToInt32(year) + 1).ToString(); }
                        SQuery = "select distinct a.icode as code,b.name as mould_name ,decode(to_char(a.date2,'dd'),'01','P','') as day1,decode(to_char(a.date2,'dd'),'02','P','') as day2,decode(to_char(a.date2,'dd'),'03','P','') as day3,decode(to_char(a.date2,'dd'),'04','P','') as day4,decode(to_char(a.date2,'dd'),'05','P','') as day5,decode(to_char(a.date2,'dd'),'06','P','') as day6,decode(to_char(a.date2,'dd'),'07','P','') as day7,decode(to_char(a.date2,'dd'),'08','P','') as day8,decode(to_char(a.date2,'dd'),'09','P','') as day9,decode(to_char(a.date2,'dd'),'10','P','') as day10,decode(to_char(a.date2,'dd'),'11','P','') as day11,decode(to_char(a.date2,'dd'),'12','P','') as day12,decode(to_char(a.date2,'dd'),'13','P','') as day13,decode(to_char(a.date2,'dd'),'14','P','') as day14,decode(to_char(a.date2,'dd'),'15','P','') as day15,decode(to_char(a.date2,'dd'),'16','P','') as day16,decode(to_char(a.date2,'dd'),'17','P','') as day17,decode(to_char(a.date2,'dd'),'18','P','') as day18,decode(to_char(a.date2,'dd'),'19','P','') as day19,decode(to_char(a.date2,'dd'),'20','P','') as day20,decode(to_char(a.date2,'dd'),'21','P','') as day21,decode(to_char(a.date2,'dd'),'22','P','') as day22,decode(to_char(a.date2,'dd'),'23','P','') as day23,decode(to_char(a.date2,'dd'),'24','P','') as day24,decode(to_char(a.date2,'dd'),'25','P','') as day25,decode(to_char(a.date2,'dd'),'26','P','') as day26,decode(to_char(a.date2,'dd'),'27','P','') as day27,decode(to_char(a.date2,'dd'),'28','P','') as day28,decode(to_char(a.date2,'dd'),'29','P','') as day29,decode(to_char(a.date2,'dd'),'30','P','') as day30,decode(to_char(a.date2,'dd'),'31','P','') as day31 from wb_maint a ,typegrp b where trim(a.icode)=trim(b.acref) and a.branchcd='" + mbr + "' and a.type='MM03' and b.id='MM' and to_char(a.vchdate,'mm/yyyy')='" + mq0 + "/" + year + "'";
                        SQuery = "select distinct a.COL1 as code,b.name as mould_name,B.ACREF AS MOULD_CODE ,decode(to_char(date1,'dd'),'01','P','') as day1,decode(to_char(date1,'dd'),'02','P','') as day2,decode(to_char(date1,'dd'),'03','P','') as day3,decode(to_char(date1,'dd'),'04','P','') as day4,decode(to_char(date1,'dd'),'05','P','') as day5,decode(to_char(date1,'dd'),'06','P','') as day6,decode(to_char(date1,'dd'),'07','P','') as day7,decode(to_char(date1,'dd'),'08','P','') as day8,decode(to_char(date1,'dd'),'09','P','') as day9,decode(to_char(date1,'dd'),'10','P','') as day10,decode(to_char(date1,'dd'),'11','P','') as day11,decode(to_char(date1,'dd'),'12','P','') as day12,decode(to_char(date1,'dd'),'13','P','') as day13,decode(to_char(date1,'dd'),'14','P','') as day14,decode(to_char(date1,'dd'),'15','P','') as day15,decode(to_char(date1,'dd'),'16','P','') as day16,decode(to_char(date1,'dd'),'17','P','') as day17,decode(to_char(date1,'dd'),'18','P','') as day18,decode(to_char(date1,'dd'),'19','P','') as day19,decode(to_char(date1,'dd'),'20','P','') as day20,decode(to_char(date1,'dd'),'21','P','') as day21,decode(to_char(date1,'dd'),'22','P','') as day22,decode(to_char(date1,'dd'),'23','P','') as day23,decode(to_char(date1,'dd'),'24','P','') as day24,decode(to_char(date1,'dd'),'25','P','') as day25,decode(to_char(date1,'dd'),'26','P','') as day26,decode(to_char(date1,'dd'),'27','P','') as day27,decode(to_char(date1,'dd'),'28','P','') as day28,decode(to_char(date1,'dd'),'29','P','') as day29,decode(to_char(date1,'dd'),'30','P','') as day30,decode(to_char(date1,'dd'),'31','P','') as day31 from wb_maint a ,typegrp b where trim(a.COL1)=trim(b.TYPE1) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) and a.branchcd='" + mbr + "' and a.type='MM03' and b.id='MM' and to_char(a.vchdate,'mm/yyyy')='" + mq0 + "/" + year + "' ORDER BY MOULD_NAME";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Mould HM Plan Checklist For the Month : " + mq1 + "/" + year + " " + todt, frm_qstr);
                        break;

                    case "F75209":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        SQuery = "update wb_maint set grade='Y' where branchcd='" + mbr + "' and type='MM02' and trim(col1)||trim(obsv2)||to_char(date1,'dd/mm/yyyy') in (" + col1.ToString().Trim() + ")";
                        fgen.execute_cmd(frm_qstr, co_cd, SQuery);
                        fgen.msg("-", "AMSG", "Flag Updation Done");
                        break;

                    case "F75210":
                        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        SQuery = "update wb_maint set grade='Y' where branchcd='" + mbr + "' and type='MM03' and trim(col1)||trim(obsv2)||to_char(date1,'dd/mm/yyyy') in (" + col1.ToString().Trim() + ")";
                        fgen.execute_cmd(frm_qstr, co_cd, SQuery);
                        fgen.msg("-", "AMSG", "Flag Updation Done");
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
            dt = new DataTable(); dt1 = new DataTable(); dt3 = new DataTable(); DataTable dt4 = new DataTable(); DataTable dt5 = new DataTable();


            if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
            else branch_Cd = "branchcd='" + mbr + "'";

            // after prdDmp this will run   

            switch (val)
            {
                case "F39131":
                    // Gate Inward Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type>'14' and a.type like '1%'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production (Std) Entry Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F75200":
                    //Mould Breakdown Monthly Instances Graph
                    // open graph
                    SQuery = "select month_name,count(*) as  Instances from (select distinct substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,vchnum as tot_bas,vchnum as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth,type||vchnum||vchdate as fstr from wb_maint a where a.branchcd='" + mbr + "'  and a.type='MM06' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Mould Breakdown Monthly Instances Graph", "column", "Month Wise", "-", SQuery, "");
                    break;

                case "F75202":
                    //Mould Maintenance Monthly Cost Graph
                    // open graph
                    SQuery = "select month_name,round(sum(tot_val),0) as  Cost_incurred from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,nvl(num3,0) as tot_val,to_Char(a.vchdate,'YYYYMM') as mth from wb_maint a where a.branchcd='" + mbr + "'  and a.type='MM07' and a.vchdate " + xprdrange + " ) group by month_name ,mth   order by mth";
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    fgen.Fn_FillChart(co_cd, frm_qstr, "Mould Maintenance Monthly Cost Graph", "column", "Month Wise", "-", SQuery, "");
                    break;

                case "F75204":
                    //Mould Maintenance Monthly Count,Cost Data
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,to_char(vchdate,'MONTH-yyyy') as Month_Name,sum(nvl(num3,0)) as Total_Cost_Incurred,sum(nvl(num1,0)) as Total_Qty_consumed,sum(nvl(num3,0)) as Value_total from wb_maint where branchcd='" + mbr + "' and type='MM07' group by to_char(vchdate,'MONTH-yyyy'),to_char(vchdate,'yyyymm') order by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(vchnum) as fstr,to_char(vchdate,'yyyymm') as gstr,a.Vchnum,to_char(vchdate,'dd/mm/yyyy') as Dated,trim(upper(Title)) as Mould_Name,sum(nvl(num3,0)) as gros_tot,sum(nvl(num1,0)) as bas_tot from wb_maint where branchcd='" + mbr + "' and type='MM07' group by a.Vchnum,to_char(vchdate,'dd/mm/yyyy'),trim(upper(Title)),to_char(vchdate,'yyyymm'),vchnum,trim(vchnum) order by to_char(vchdate,'yyyymm'),vchnum", frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
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
                //
                case "F75157":// Mould History with parts consume
                    SQuery = "select a.vchnum as Break_down_entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,to_date(A.COL11,'dd/mm/yyyy') AS BREAKDOWN_DATE,A.COL2 AS TOOL_ERP_CODE,B.CPARTNO as part_no,B.NAME AS Model_Name,b.col4 as part_name,b.col5 as Mould_id_No,to_char(b.date1,'dd/MM/yyyy') as Mould_Commission_date,b.col6 as Mould_size,b.num2 as number_of_cavities,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when nvl(trim(a.edt_by),'-')!='-' then to_char(a.edt_dt,'dd/mm/yyyy') else '-' end) as edt_dt  from wb_maint a,wb_master b where trim(a.col2)=trim(b.col1) and b.id='MM01' and trim(a.branchcd)='" + mbr + "' AND trim(a.type)='MM06' AND  a.vchdate " + xprdrange + "";
                    SQuery = "select a.vchnum as Maint_ent_no,to_char(a.vchdate,'dd/mm/yyyy') as Maint_ent_dt,to_char(A.date1,'dd/mm/yyyy') AS Maint_dt,A.COL1 AS Mould_Code,B.CPARTNO as Part_no,B.NAME AS Model_Name,b.col4 as Part_name,b.col5 as Mould_id_No,to_char(b.date1,'dd/MM/yyyy') as Mould_Comm_dt,b.col6 as Mould_size,b.num2 as No_of_cavities,(case when trim(a.type)='MM04' then 'Preventive' when trim(a.type)='MM05' then 'Health' when trim(a.type)='MM07' then 'Break-down' end) as Status,a.srno, a.icode as item_code, c.iname as item_name,a.num3 as Qty, a.num4 as Rate, a. num5 as Amount, a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when nvl(trim(a.edt_by),'-')!='-' then to_char(a.edt_dt,'dd/mm/yyyy') else '-' end) as edt_dt ,to_char(a.vchdate,'yyyymmdd') as vdd  from wb_maint a,wb_master b, item c where trim(a.col1)=trim(b.col1) and trim(a.icode)=trim(c.icode) and b.id='MM01' and trim(a.branchcd)='" + mbr + "' AND (trim(a.type)='MM04'or trim(a.type)='MM05' or trim(a.type)='MM07') AND  a.vchdate " + xprdrange + " and nvl(b.col2,'-')!='Y' order by vdd desc, a.vchnum, a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Mould History with parts consumed for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F75183":
                    //NEW Nould Installed
                    //date1
                    SQuery = "select A.VCHNUM AS MOULD_ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MOULD_ENTRY_DT,to_char(date1,'dd/mm/yyyy') as Installation_date,TRIM(A.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,A.COL1 AS MOULD_CODE, A.COL4 AS MOULD_NAME,A.CPARTNO AS PARTNO, A.COL5 AS MOULD_ID,A.COL6 AS MOULD_SIZE,A.COL14 AS MATERIAL,A.COL9 AS MOULD_NAME_ID from wb_master A,FAMST C where TRIM(A.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.id='MM01' AND A.DATE1 " + xprdrange + " and nvl(a.col2,'-')!='Y' ORDER BY MOULD_ENTRY_NO desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("New Mould Installed Report for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F75184":
                    //MOULD BREAKDOWN REPORT
                    SQuery = "select a.vchnum AS ENTRY_NO,to_char(a.vchdate,'dd/mm/yyyy') as ENTRY_dT,A.COL1 AS MOULD,B.NAME AS MOULD_NAME,a.btchno as machno,a.title as machine_name,to_char(A.date1,'dd/MM/yyyy') AS BRK_DWN_DT,A.COL12 AS BRK_dWN_TIME,A.REMARKS,to_char(date1,'yyyymmdd') as vdd  from WB_MAINT a,typegrp b WHERE trim(a.col1)=trim(b.type1) and A.BRANCHCD='" + mbr + "' AND B.BRANCHCD='" + mbr + "' AND a.TYPE='MM06' and b.id='MM'  AND date1 " + xprdrange + " and nvl(a.col2,'-')!='Y' ORDER BY vdd desc, a.VCHNUM";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Mould BreakDown Report for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F75185":
                    #region
                    //Balance PM Life
                    dt2 = new DataTable();
                    dt2.Columns.Add("Mould_Code", typeof(string));
                    dt2.Columns.Add("Mould_Id", typeof(string));
                    dt2.Columns.Add("Mould_Name", typeof(string));
                    dt2.Columns.Add("Customer_Name", typeof(string));
                    dt2.Columns.Add("Commission_Date", typeof(string));
                    dt2.Columns.Add("PM_Life", typeof(string));
                    dt2.Columns.Add("Alert", typeof(double));
                    dt2.Columns.Add("Shots_Utilised", typeof(double));
                    dt2.Columns.Add("Balance_Life", typeof(double));
                    dt2.Columns.Add("Last_Maint_Date", typeof(string));
                    dt2.Columns.Add("PM_Life_Status", typeof(string));



                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + mbr + "'", "OPT_START");// check date for mm start date
                    xprdrange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value1 + "','dd/mm/yyyy')";

                    SQuery = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(provision) as Mnt_life,sum(prodn) as shots_utilised,max(provision)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(provision) as provision from typegrp where branchcd='" + mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as provision from wb_maint a where a.branchcd='" + mbr + "' and a.type='MM04' and a.date1 Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value1 + "','dd/mm/yyyy') union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as provision from prod_sheet where branchcd='" + mbr + "' and type='90' and vchdate " + xprdrange + ") group by acref order by acref ";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //balance life query

                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM04' ) group by mould";
                    mq4 = "select mould,to_char(to_date(vchdate,'yyyymmdd'),'dd/mm/yyyy') as vchdate from (select mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'yyyymmdd') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM04') group by mould) order by mould";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);//maintenenace checking data

                    mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, b.col1 AS MOULD_CODE,trim(a.name) AS MOULD_NAME,b.num6 AS PM_life,B.NUM14 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col7 as last_pm_dt FROM typegrp a, WB_MASTER B,FAMST C WHERE TRIM(B.ACODE)=TRIM(C.ACODE) AND a.branchcd|| trim(a.acref)||trim(a.type1)=b.branchcd||TRIM(B.cpartno)||trim(b.col1) and a.id='MM' and B.BRANCHCD='" + mbr + "' AND B.ID='MM01' and nvl(b.col2,'-')!='Y' order by mould_code"; // b.col1='0134'
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //main master picking data

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        db = 0; db3 = 0; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = "";
                        db = fgen.make_double(dt1.Rows[i]["alert"].ToString().Trim());
                        db3 = Convert.ToDouble(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        dr1 = dt2.NewRow();
                        dr1["Mould_Code"] = dt1.Rows[i]["MOULD_CODE"].ToString().Trim();
                        dr1["Mould_Id"] = dt1.Rows[i]["acref"].ToString().Trim();
                        dr1["Mould_Name"] = dt1.Rows[i]["MOULD_NAME"].ToString().Trim();
                        dr1["Customer_Name"] = dt1.Rows[i]["PARTY"].ToString().Trim();
                        dr1["PM_Life"] = dt1.Rows[i]["PM_life"].ToString().Trim();
                        dr1["Alert"] = fgen.make_double(dt1.Rows[i]["ALERT"].ToString().Trim());
                        dr1["Commission_Date"] = dt1.Rows[i]["comm_dt"].ToString().Trim();
                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dr1["Mould_Code"].ToString().Trim() + "'", "vchdate");
                        mq6 = mq5;
                        if (mq5.Trim().Length <= 1)
                        {
                            mq5 = dt1.Rows[i]["last_pm_dt"].ToString().Trim();
                            mq6 = mq5;
                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                            {
                            }
                            else
                            {
                                mq5 = mq0;
                            }
                        }
                        mq7 = fgen.seek_iname(frm_qstr, co_cd, " Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + mbr + "' and type='90' and pvchnum='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('" + value1 + "','dd/MM/yyyy') group by trim(pvchnum)", "totp"); ;
                        dr1["Shots_Utilised"] = fgen.make_double(mq7);
                        dr1["Last_Maint_Date"] = mq6;
                        //dr1["Balance_Life"] = fgen.make_double(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        mq8 = dt1.Rows[i]["PM_life"].ToString().Trim();
                        dr1["Balance_Life"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                        dr1["PM_Life_Status"] = "OK";


                        if (fgen.make_double(mq8) > 0)
                        {
                            if (((fgen.make_double(mq8) - fgen.make_double(mq7)) / fgen.make_double(mq8)) * 100 <= 25)
                            {
                                dr1["PM_Life_Status"] = "CHECK";
                            }
                        }

                        dt2.Rows.Add(dr1);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt2;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("Balance PM Life as on " + value1 + "", frm_qstr);
                    }
                    #endregion
                    break;
                case "F75206":
                    #region
                    //Balance Total Life
                    dt2 = new DataTable();
                    dt2.Columns.Add("Mould_Code", typeof(string));
                    dt2.Columns.Add("Mould_Id", typeof(string));
                    dt2.Columns.Add("Mould_Name", typeof(string));
                    dt2.Columns.Add("Customer_Name", typeof(string));
                    dt2.Columns.Add("Commission_Date", typeof(string));
                    dt2.Columns.Add("Tot_life", typeof(string));
                    dt2.Columns.Add("Alert", typeof(double));
                    dt2.Columns.Add("Shots_Utilised", typeof(double));
                    dt2.Columns.Add("Balance_Life", typeof(double));
                    dt2.Columns.Add("Last_Maint_Date", typeof(string));
                    dt2.Columns.Add("Tot_Life_Status", typeof(string));



                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + mbr + "'", "OPT_START");// check date for mm start date
                    xprdrange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value2 + "','dd/mm/yyyy')";

                    SQuery = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(provision) as Mnt_life,sum(prodn) as shots_utilised,max(provision)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(provision) as provision from typegrp where branchcd='" + mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as provision from wb_maint a where a.branchcd='" + mbr + "' and a.type='MM04' and a.date1 Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value1 + "','dd/mm/yyyy') union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as provision from prod_sheet where branchcd='" + mbr + "' and type='90' and vchdate " + xprdrange + ") group by acref order by acref ";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //balance life query

                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM04' ) group by mould";
                    mq4 = "select mould,to_char(to_date(vchdate,'yyyymmdd'),'dd/mm/yyyy') as vchdate from (select mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'yyyymmdd') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM04') group by mould) order by mould";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);//maintenenace checking data

                    mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, b.col1 AS MOULD_CODE,trim(a.name) AS MOULD_NAME,b.num4 AS tot_life,B.NUM14 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col7 as last_pm_dt FROM typegrp a, WB_MASTER B,FAMST C WHERE TRIM(B.ACODE)=TRIM(C.ACODE) AND a.branchcd|| trim(a.acref)||trim(a.type1)=b.branchcd||TRIM(B.cpartno)||trim(b.col1) and a.id='MM' and B.BRANCHCD='" + mbr + "' AND B.ID='MM01' and nvl(b.col2,'-')!='Y' order by mould_code"; // b.col1='0134'
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //main master picking data

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        db = 0; db3 = 0; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = "";
                        db = fgen.make_double(dt1.Rows[i]["alert"].ToString().Trim());
                        db3 = Convert.ToDouble(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        dr1 = dt2.NewRow();
                        dr1["Mould_Code"] = dt1.Rows[i]["MOULD_CODE"].ToString().Trim();
                        dr1["Mould_Id"] = dt1.Rows[i]["acref"].ToString().Trim();
                        dr1["Mould_Name"] = dt1.Rows[i]["MOULD_NAME"].ToString().Trim();
                        dr1["Customer_Name"] = dt1.Rows[i]["PARTY"].ToString().Trim();
                        dr1["Tot_life"] = dt1.Rows[i]["tot_life"].ToString().Trim();
                        dr1["Alert"] = fgen.make_double(dt1.Rows[i]["ALERT"].ToString().Trim());
                        dr1["Commission_Date"] = dt1.Rows[i]["comm_dt"].ToString().Trim();
                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dr1["Mould_Code"].ToString().Trim() + "'", "vchdate");
                        mq6 = mq5;
                        if (mq5.Trim().Length <= 1)
                        {
                            mq5 = dt1.Rows[i]["last_pm_dt"].ToString().Trim();
                            mq6 = mq5;
                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                            {
                            }
                            else
                            {
                                mq5 = mq0;
                            }
                        }
                        mq8 = " Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + mbr + "' and type='90' and pvchnum='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "' and vchdate " + xprdrange + " group by trim(pvchnum)";
                        mq7 = fgen.seek_iname(frm_qstr, co_cd, mq8, "totp"); ;
                        dr1["Shots_Utilised"] = fgen.make_double(mq7);
                        dr1["Last_Maint_Date"] = mq6;
                        //dr1["Balance_Life"] = fgen.make_double(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        mq8 = dt1.Rows[i]["Tot_life"].ToString().Trim();
                        dr1["Balance_Life"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                        dr1["Tot_Life_Status"] = "OK";


                        if (fgen.make_double(mq8) > 0)
                        {
                            if (((fgen.make_double(mq8) - fgen.make_double(mq7)) / fgen.make_double(mq8)) * 100 <= 25)
                            {
                                dr1["Tot_Life_Status"] = "CHECK";
                            }
                        }

                        dt2.Rows.Add(dr1);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt2;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("Balance Life as on " + value2 + "", frm_qstr);
                    }
                    #endregion
                    break;

                case "F75186_old":
                    //Balance HM Life
                    dt2 = new DataTable();
                    dt2.Columns.Add("Mould_Code", typeof(string));
                    dt2.Columns.Add("Mould_Id", typeof(string));
                    dt2.Columns.Add("Mould_Name", typeof(string));
                    dt2.Columns.Add("Customer_Name", typeof(string));
                    dt2.Columns.Add("Commission_Date", typeof(string));
                    dt2.Columns.Add("First_HM_Count", typeof(string));
                    dt2.Columns.Add("HM_Life", typeof(string));
                    dt2.Columns.Add("Alert", typeof(double));
                    dt2.Columns.Add("Shots_Utilised", typeof(double));
                    dt2.Columns.Add("Balance_Life", typeof(double));
                    dt2.Columns.Add("Last_Maint_Date", typeof(string));
                    dt2.Columns.Add("HM_Health_Status", typeof(string));

                    dt = new DataTable(); dt1 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();

                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + mbr + "'", "OPT_START");// check date for mm start date
                    xprdrange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value1 + "','dd/mm/yyyy')";

                    SQuery = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + mbr + "' and a.type='MM05' and a.date1 " + xprdrange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + mbr + "' and type='90' and vchdate " + xprdrange + ") group by acref"; //new
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //balance life query

                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM05' ) group by mould";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);//maintenenace checking data


                    mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, trim(b.num13) as first_hm, to_number(trim(b.col12)) as op, b.col1 AS MOULD_CODE,trim(b.col4) AS MOULD_NAME,to_number(b.col15) AS HM_life,B.NUM5 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col8 as last_hm_dt  FROM  WB_MASTER B ,FAMST C WHERE  TRIM(B.ACODE)=TRIM(C.ACODE) AND B.BRANCHCD='" + mbr + "' AND B.ID='MM01' and nvl(b.col2,'-')!='Y' order by mould_code"; // b.col1='0134'
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //master picking  dt

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        db = 0; db3 = 0; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = "";
                        db = fgen.make_double(dt1.Rows[i]["alert"].ToString().Trim());
                        db3 = Convert.ToDouble(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        dr1 = dt2.NewRow();
                        dr1["Mould_Code"] = dt1.Rows[i]["MOULD_CODE"].ToString().Trim();
                        dr1["Mould_Id"] = dt1.Rows[i]["acref"].ToString().Trim();
                        dr1["Mould_Name"] = dt1.Rows[i]["MOULD_NAME"].ToString().Trim();
                        dr1["Customer_Name"] = dt1.Rows[i]["PARTY"].ToString().Trim();
                        dr1["Commission_Date"] = dt1.Rows[i]["comm_dt"].ToString().Trim();
                        dr1["First_HM_Count"] = dt1.Rows[i]["first_hm"].ToString().Trim();
                        dr1["HM_Life"] = dt1.Rows[i]["HM_life"].ToString().Trim();
                        dr1["Alert"] = fgen.make_double(dt1.Rows[i]["ALERT"].ToString().Trim());
                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dr1["Mould_Code"].ToString().Trim() + "'", "vchdate");
                        mq6 = mq5;
                        mq8 = dt1.Rows[i]["HM_life"].ToString().Trim();
                        if (mq5.Trim().Length <= 1)
                        {
                            mq5 = dt1.Rows[i]["last_hm_dt"].ToString().Trim();
                            mq6 = mq5;
                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                            {
                            }
                            else
                            {
                                mq5 = mq0;
                            }
                            if (Convert.ToDateTime(mq5) == Convert.ToDateTime(dt1.Rows[i]["comm_dt"].ToString().Trim()))
                            {
                                mq8 = dt1.Rows[i]["first_hm"].ToString().Trim();
                            }
                            else
                            {
                            }
                        }
                        mq7 = fgen.seek_iname(frm_qstr, co_cd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + mbr + "' and type='90' and pvchnum='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('" + value1 + "','dd/MM/yyyy') group by trim(pvchnum)", "totp"); ;
                        dr1["Shots_Utilised"] = fgen.make_double(mq7);
                        dr1["Last_Maint_Date"] = mq6;
                        //dr1["Balance_Life"] = fgen.make_double(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        dr1["Balance_Life"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                        dr1["HM_Health_Status"] = "OK";
                        if (fgen.make_double(mq8) > 0)
                        {
                            if (((fgen.make_double(mq8) - fgen.make_double(mq7)) / fgen.make_double(mq8)) * 100 <= 25)
                            {
                                dr1["HM_Health_Status"] = "CHECK";
                            }
                        }
                        dt2.Rows.Add(dr1);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt2;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("Balance HM Life as on " + value1 + "", frm_qstr);
                    }
                    break;

                case "F75186":
                    //Balance HM Life
                    dt2 = new DataTable();
                    dt2.Columns.Add("Mould_Code", typeof(string));
                    dt2.Columns.Add("Mould_Id", typeof(string));
                    dt2.Columns.Add("Mould_Name", typeof(string));
                    dt2.Columns.Add("Customer_Name", typeof(string));
                    dt2.Columns.Add("Commission_Date", typeof(string));
                    dt2.Columns.Add("First_HM_Count", typeof(string));
                    dt2.Columns.Add("HM_Life", typeof(string));
                    dt2.Columns.Add("Alert", typeof(double));
                    dt2.Columns.Add("Shots_Utilised", typeof(double));
                    dt2.Columns.Add("Balance_Life", typeof(double));
                    dt2.Columns.Add("Last_Maint_Date", typeof(string));
                    dt2.Columns.Add("HM_Health_Status", typeof(string));

                    dt = new DataTable(); dt1 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable();

                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + mbr + "'", "OPT_START");// check date for mm start date
                    xprdrange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value1 + "','dd/mm/yyyy')";

                    SQuery = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + mbr + "' and a.type='MM05' and a.date1 " + xprdrange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + mbr + "' and type='90' and vchdate " + xprdrange + ") group by acref"; //new
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //balance life query

                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM05' ) group by mould";
                    mq4 = "select mould,to_char(to_date(vchdate,'yyyymmdd'),'dd/mm/yyyy') as vchdate from (select mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'yyyymmdd') as vchdate from wb_maint where branchcd='" + mbr + "' and type='MM05') group by mould) order by mould";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);//maintenenace checking data

                    mq5 = "select trim(type1) as mould,name,trim(acref) as acref,acref2 as cavity from typegrp where id='MM' and branchcd='" + mbr + "' order by mould";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq5);

                    mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, trim(b.num13) as first_hm, to_number(trim(b.col12)) as op, b.col1 AS MOULD_CODE,trim(b.col4) AS MOULD_NAME,to_number(b.col15) AS HM_life,B.NUM5 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col8 as last_hm_dt  FROM  WB_MASTER B ,FAMST C WHERE  TRIM(B.ACODE)=TRIM(C.ACODE) AND B.BRANCHCD='" + mbr + "' AND B.ID='MM01' and nvl(b.col2,'-')!='Y' order by mould_code"; // b.col1='0134'
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //master picking  dt

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        db = 0; db3 = 0; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; db1 = 0; db2 = 0;
                        db = fgen.make_double(dt1.Rows[i]["alert"].ToString().Trim());
                        db3 = Convert.ToDouble(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        dr1 = dt2.NewRow();
                        dr1["Mould_Code"] = dt1.Rows[i]["MOULD_CODE"].ToString().Trim();
                        dr1["Mould_Id"] = dt1.Rows[i]["acref"].ToString().Trim();
                        dr1["Mould_Name"] = dt1.Rows[i]["MOULD_NAME"].ToString().Trim();
                        dr1["Customer_Name"] = dt1.Rows[i]["PARTY"].ToString().Trim();
                        dr1["Commission_Date"] = dt1.Rows[i]["comm_dt"].ToString().Trim();
                        dr1["First_HM_Count"] = dt1.Rows[i]["first_hm"].ToString().Trim();
                        dr1["HM_Life"] = dt1.Rows[i]["HM_life"].ToString().Trim();
                        dr1["Alert"] = fgen.make_double(dt1.Rows[i]["ALERT"].ToString().Trim());
                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dr1["Mould_Code"].ToString().Trim() + "'", "vchdate");
                        db2 = fgen.make_double(fgen.seek_iname_dt(dt3, "mould ='" + dr1["Mould_Code"].ToString().Trim() + "'", "cavity"));
                        mq6 = mq5;
                        mq8 = dt1.Rows[i]["HM_life"].ToString().Trim();
                        if (mq5.Trim().Length <= 1)
                        {
                            mq5 = dt1.Rows[i]["last_hm_dt"].ToString().Trim();
                            mq6 = mq5;
                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                            {
                                db1 = 0;
                            }
                            else
                            {
                                mq5 = mq0;
                                db1 = fgen.make_double(dt1.Rows[i]["op"].ToString().Trim());
                            }
                            if (Convert.ToDateTime(mq5) == Convert.ToDateTime(dt1.Rows[i]["comm_dt"].ToString().Trim()))
                            {
                                mq8 = dt1.Rows[i]["first_hm"].ToString().Trim();
                            }
                            else
                            {
                            }
                        }
                        mq7 = (fgen.make_double(fgen.seek_iname(frm_qstr, co_cd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + mbr + "' and type='90' and pvchnum='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('" + value1 + "','dd/MM/yyyy') group by trim(pvchnum)", "totp")) + db1).ToString();
                        dr1["Shots_Utilised"] = (fgen.make_double(mq7) / db2).ToString().Replace("Infinity", "0").Replace("NaN", "0");
                        dr1["Last_Maint_Date"] = mq6;
                        //dr1["Balance_Life"] = fgen.make_double(fgen.seek_iname_dt(dt, "Mould_No='" + dt1.Rows[i]["ACREF"].ToString().Trim() + "'", "Bal_mainT_life"));
                        dr1["Balance_Life"] = fgen.make_double(mq8) - fgen.make_double(dr1["Shots_Utilised"].ToString());
                        dr1["HM_Health_Status"] = "OK";
                        if (fgen.make_double(mq8) > 0)
                        {
                            if (((fgen.make_double(mq8) - fgen.make_double(mq7)) / fgen.make_double(mq8)) * 100 <= 25)
                            {
                                dr1["HM_Health_Status"] = "CHECK";
                            }
                        }
                        dt2.Rows.Add(dr1);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        Session["send_dt"] = dt2;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("Balance HM Life as on " + value1 + "", frm_qstr);
                    }
                    break;

                case "F75187":
                    //Balance production Life REPORT
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + mbr + "'", "OPT_START");// check date for mm start date
                    xprdrange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('" + value1 + "','dd/mm/yyyy')";
                    SQuery = "Select max(a.Name) as Mould_Name ,a.Acref as Mould_No,max(a.lineno) as Tool_life,sum(a.op) as Opening_shots, sum(a.prodn) as shots_utilised,max(a.lineno)-(sum(a.prodn)+ sum(a.op)) as Bal_Prod_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as op, 0 as prodn from typegrp where branchcd='" + mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num8,0 as prodn from wb_master a where a.branchcd='" + mbr + "' and a.date1 " + xprdrange + " and nvl(a.col2,'-')!='Y' union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp from prod_sheet where branchcd='" + mbr + "' and type='90' and vchdate " + xprdrange + ")a group by acref";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Balance Production Life as on " + value1 + "", frm_qstr);
                    break;
                case "F75191":
                    SQuery = "select SUBSTR(trim(a.entry),0,6) as ENTRY_NO,SUBSTR(trim(a.entry),7,16) as ENTRY_DATE,a.mould_code as code,b.name as mould_name,a.breakdown_date,BD_TIME AS BREAKDOWN_TIME,max(a.btchno) As mc_code,max(a.title) As mc_name from (select distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry , col1 as mould_code,to_char(date1,'dd/mm/yyyy') as breakdown_date,COL12 AS BD_TIME,1 as qty,btchno,title,REMARKS from wb_maint where branchcd='" + mbr + "' and type='MM06' and vchdate " + xprdrange + " union all select distinct col11 as entry ,col1 as mould_code,to_char(date1,'dd/mm/yyyy') as breakdown_date,COL12 AS BD_TIME,-1 as qty,btchno,title,'' AS REMARKS from wb_maint where branchcd='" + mbr + "' and type='MM07' and vchdate " + xprdrange + ") a,typegrp b where trim(a.mould_code)=trim(b.type1) and b.branchcd='" + mbr + "' and b.id='MM' group by trim(a.entry),a.mould_code,b.name,a.breakdown_date,BD_TIME having sum(qty)>0 order by b.name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Mould Breakdown Pending OK for Production for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F75192":
                    SQuery = "select SUBSTR(trim(a.entry),0,6) as ENTRY_NO,SUBSTR(trim(a.entry),7,16) AS ENTRY_DATE,b.acref as mould_id,b.name as mould ,trim(a.col1) as code,  trim(a.entry) as entry_details,A.TITLE,A.BTCHNO,A.ICODE,C.INAME,A.COL12 AS DOWNTIME,A.DATE1 from (SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry ,trim(col1) as col1,TITLE,BTCHNO,ICODE,COL12,DATE1,1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM07' and vchdate " + xprdrange + " union all select distinct trim(col11) as entry,trim(col1) as col1,'' AS TITLE,'' AS BTCHNO,'' AS ICODE,COL12,DATE1,-1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM08' and vchdate " + xprdrange + " and upper(trim(nvl(result,'-')))='Y')a,typegrp b,ITEM C where trim(a.col1)=trim(b.type1) AND TRIM(A.ICODE)=TRIM(C.ICODE)  and b.branchcd='" + mbr + "' and b.id='MM' group by trim(a.entry),trim(a.col1),b.acref,b.name, trim(a.entry),A.TITLE,A.BTCHNO,A.ICODE,C.INAME,A.COL12,A.DATE1  having sum(qty)>0";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Mould OK pending Production Approval for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
                case "F75193":
                    SQuery = "select SUBSTR(trim(a.entry),0,6) as ENTRY_NO,SUBSTR(trim(a.entry),7,16) AS ENTRY_DATE,b.acref as mould_id,b.name as mould ,trim(a.col1) as code , trim(a.entry) as entry_details from (SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry,trim(col1) as col1,1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM08' and vchdate " + xprdrange + " and trim(upper(nvl(result,'-')))='Y' union all select distinct trim(col11) as entry,trim(col1) as col1,-1 as qty from wb_maint where branchcd='" + mbr + "' and type='MM09' and vchdate " + xprdrange + " and upper(trim(nvl(result,'-')))='Y')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + mbr + "' and b.id='MM' group by trim(a.entry),trim(a.col1),b.acref,b.name, trim(a.entry) having sum(qty)>0";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Mould OK pending Quality Approval for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                ///////=============21 april 2020====made by yogita
                case "F75141"://section wise b/dsummary=============NEEDTO CHANGE IN QRY
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select col17 as fstr,'-' as gstr, Particulars,Instances,Tot_BDtime,Tot_Cost,round(Tot_BDtime/Instances,2) as Avg_BDTime from (select col17,col17 as Particulars,Count(vchnum) as Instances,round(((Sum(COL8) * 60) + Sum(COL9)) / 60, 2) As tot_BDtime,SUM(NVL(NUM5,0)) AS Tot_Cost from scratch where branchcd='" + mbr + "' and substr(type,1,2)='MN' and vchdate " + xprdrange + " group by col17) order by Instances desc ", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr, col17 as gstr,vchnum as Com_ID,TO_CHAR(vchdate,'DD/MM/YYYY') as Comp_Dated,col3 as Machine,col2 as MCHID,col1 as Complaint,col5 as Identification,col6 as Attend_By,col7 as Spare_used,col10 as Action_date,col12 as Reason,Remarks as Remedy,to_char(num3)||':'||to_char(num4)||'Hrs' as Reported_at,to_char(num1)||':'||to_char(num2)||'Hrs' as Cleared_at,round((((COL8) * 60) + (COL9)) / 60, 2) as BD_TIME From scratch where branchcd='" + mbr + "'  and type='MN' and vchdate " + xprdrange + " order by vchdate desc,vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("SECTION WISE B/D SUMMARY For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F75142"://DEPT WISE B/DSUMMARY
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select col4 as fstr,'-' as gstr, Particulars,Instances,Tot_BDtime,Tot_Cost,round(Tot_BDtime/Instances,2) as Avg_BDTime from (select col4,col18 as Particulars,Count(vchnum) as Instances,round(((Sum(COL8) * 60) + Sum(COL9)) / 60, 2) As tot_BDtime,SUM(NVL(NUM5,0)) AS Tot_Cost from scratch where branchcd='" + mbr + "' and substr(type,1,2)='MN' and vchdate " + xprdrange + " group by col4,col18) order by Instances desc ", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr,col4 as gstr,vchnum as Com_ID,TO_CHAR(vchdate,'DD/MM/YYYY') as Comp_Dated,col3 as Machine,col2 as MCHID,col1 as Complaint,col5 as Identification,col6 as Attend_By,col7 as Spare_used,col10 as Action_date,col12 as Reason,Remarks as Remedy,to_char(num3)||':'||to_char(num4)||'Hrs' as Reported_at,to_char(num1)||':'||to_char(num2)||'Hrs' as Cleared_at,round((((COL8) * 60) + (COL9)) / 60, 2) as BD_TIME From scratch where branchcd='" + mbr + "'  and type='MN' and vchdate " + xprdrange + " order by vchdate desc,vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("DEPT  WISE B/D SUMMARY For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;


                case "F75143"://M/C WISE B/D SUMMARY
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select col2 as fstr,'-' as gstr, Particulars,Instances,Tot_BDtime,Tot_Cost,round(Tot_BDtime/Instances,2) as Avg_BDTime from (select col2,col3 as Particulars,Count(vchnum) as Instances,round(((Sum(COL8) * 60) + Sum(COL9)) / 60, 2) As tot_BDtime,SUM(NVL(NUM5,0)) AS Tot_Cost from scratch where branchcd='" + mbr + "' and substr(type,1,2)='MN' and vchdate " + xprdrange + " group by col2,COL3) order by Instances desc ", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr, col2 as gstr,vchnum as Com_ID,TO_CHAR(vchdate,'DD/MM/YYYY') as Comp_Dated,col3 as Machine,col2 as MCHID,col1 as Complaint,col5 as Identification,col6 as Attend_By,col7 as Spare_used,col10 as Action_date,col12 as Reason,Remarks as Remedy,to_char(num3)||':'||to_char(num4)||'Hrs' as Reported_at,to_char(num1)||':'||to_char(num2)||'Hrs' as Cleared_at,round((((COL8) * 60) + (COL9)) / 60, 2) as BD_TIME From scratch where branchcd='" + mbr + "'  and type='MN' and vchdate " + xprdrange + " order by vchdate desc,vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("MACHINE WISE B/D SUMMARY For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;


                case "F75144"://Reason wise b/dsummary
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select COL12 as fstr,'-' as gstr, Particulars,Instances,Tot_BDtime,Tot_Cost,round(Tot_BDtime/Instances,2) as Avg_BDTime from (select COL12,col12 as Particulars,Count(vchnum) as Instances,round(((Sum(COL8) * 60) + Sum(COL9)) / 60, 2) As tot_BDtime,SUM(NVL(NUM5,0)) AS Tot_Cost from scratch where branchcd='" + mbr + "' and substr(type,1,2)='MN' and vchdate " + xprdrange + " group by col12) order by Instances desc ", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr, COL12 as gstr,vchnum as Com_ID,TO_CHAR(vchdate,'DD/MM/YYYY') as Comp_Dated,col3 as Machine,col2 as MCHID,col1 as Complaint,col5 as Identification,col6 as Attend_By,col7 as Spare_used,col10 as Action_date,col12 as Reason,Remarks as Remedy,to_char(num3)||':'||to_char(num4)||'Hrs' as Reported_at,to_char(num1)||':'||to_char(num2)||'Hrs' as Cleared_at,round((((COL8) * 60) + (COL9)) / 60, 2) as BD_TIME From scratch where branchcd='" + mbr + "'  and type='MN' and vchdate " + xprdrange + " order by vchdate desc,vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("REASON WISE B/D SUMMARY For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F75126"://comp tracking report
                    //
                    if (hfcode.Value == "PEND")
                    {
                        SQuery = "select to_char(vchdate,'dd/mm/yyyy')as vchdate,vchnum||'  '||decode(trim(nvl(col22,'-')),'-','(Pending)','(Closed)') as com_id,col3 as machine, col1 as complaint,col5 as identification,col6  as attend_by,col12 as reason,remarks as remedy,num3||':'||num4||'Hrs' as reported_At,col11 as prod_loss,num1||':'||num2||'Hrs' as cleared_At,col8||'.'||col9 as hrs ,num5 as cost,vchnum,col22 as action,col23 as col_by,col24 as clo_dt,col19 as compl_by   from scratch where branchcd='" + mbr + "' and type='MN' AND nvl(col22,'-')='-' AND vchdate " + xprdrange + " ORDER BY VCHNUM DESC";
                        header_n = "Pending Complaints";
                    }
                    if (hfcode.Value == "CLOS")
                    {
                        SQuery = "select to_char(vchdate,'dd/mm/yyyy')as vchdate,vchnum||'  '||decode(trim(nvl(col22,'-')),'-','(Pending)','(Closed)') as com_id,col3 as machine, col1 as complaint,col5 as identification,col6  as attend_by,col12 as reason,remarks as remedy,num3||':'||num4||'Hrs' as reported_At,col11 as prod_loss,num1||':'||num2||'Hrs' as cleared_At,col8||'.'||col9 as hrs ,num5 as cost,vchnum,col22 as action,col23 as col_by,col24 as clo_dt,col19 as compl_by   from scratch where branchcd='" + mbr + "' and type='MN' AND nvl(col22,'-')!='-' AND vchdate " + xprdrange + "ORDER BY VCHNUM DESC";
                        header_n = "Closed Complaints";
                    }
                    if (hfcode.Value == "ALL")
                    {
                        SQuery = "select to_char(vchdate,'dd/mm/yyyy')as vchdate,vchnum||'  '||decode(trim(nvl(col22,'-')),'-','(Pending)','(Closed)') as com_id,col3 as machine, col1 as complaint,col5 as identification,col6  as attend_by,col12 as reason,remarks as remedy,num3||':'||num4||'Hrs' as reported_At,col11 as prod_loss,num1||':'||num2||'Hrs' as cleared_At,col8||'.'||col9 as hrs ,num5 as cost,vchnum,col22 as action,col23 as col_by,col24 as clo_dt,col19 as compl_by   from scratch where branchcd='" + mbr + "' and type='MN' AND vchdate " + xprdrange + "ORDER BY VCHNUM DESC";
                        header_n = "All Complaints Logs";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("" + header_n + " Report for the Period " + value1 + " to " + value2, frm_qstr);
                    break;
            }
        }
    }
}