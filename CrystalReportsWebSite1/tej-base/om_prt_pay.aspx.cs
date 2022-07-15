using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_pay : System.Web.UI.Page
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
                case "89554":
                case "F60121":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "22610A":
                case "22610B":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    break;

                //MADE BY AKSHAY...MERGED BY YOGITA
                case "F40132": //Daily Prodn Report(PP)  
                ///made and merged by yogita             
                //case "F82503"://category wise summary report            
                    SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                    header_n = "Select Month";
                    break;

                //MADE BY AKSHAY...MERGED BY YOGITA
                case "F85147": //Date of Joining List
                case "F85148"://LIST OF BIRTHDAY
                case "F85149": //Join/List of Leaving
                case "F85150": //Attendece register
                ///made and merged by yogitA
                case "F82512"://LIST OF MOBILE NUMBERS
                case "F82530": //address list
                case "F82501"://list of blood group
                case "F82510"://list of lanlines               
                case "F82505": //identity card              
                case "F82507": //NEW joining card
                case "F82519"://SALARY RATE REPORT
                //case "EMP_SRCH"://in main Tejaxo Report is direct opening after click on icon...but i have add a grade popup on this report
                case "F82525": //DEPT WISE PAY SUMMARY               
                case "F82514"://PAY SUMMARY //PAY SUMMARY MONTH WISE //SALARY REQUEST ICON IN HR MODULE=>REPORTS
                case "F82515"://PAY REG QUARTERLY //Quarterly pay register...hr module-salary reprot-advanced reprot-quarterly report
                case "F82516": //PAY TREND SECTION WISE 
                case "F85231"://FOR ANNIVERSARY LIST...NEW ICON
                case "F82573"://list of leaving
                case "F82574":// new joining
                case "F82575":// appraisal
                case "F82576":// confirmation
                case "F82577":// appointment
                case "F82578":// gross sal
                case "F82579":// hr strength
                case "F82582":// ot incentive
                case "F82584":// annual income
                case "F82585":// welfare
                case "F82586":// salary compare
                case "F82587":// late coming
                case "F82588":// 31 day late coming
                case "F85152":
                case "F82517"://PAY TREND DEPT WISE
                case "F82523"://GROSS PAY TREND DEPT WSIE
                case "F82503"://category wise summary report    
                case "F85158":
                case "F85161":
                case "F85162":
                    SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I' AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                    header_n = "Select Grade";
                    break;

                case "F82521"://combined wise pay summary
                    SQuery = "Select Type1 as fstr,type1 as code,name from type where id='B' order by fstr";
                    break;
                                               
                case "F85232":
                    SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I'  AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                    header_n = "Select Grade";
                    break;

                case "F85142"://SALARY REGISTER
                case "F82527": //salary slip
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
                    if (mq0 == "Y")
                    {
                        SQuery = "select trim(type1) AS FSTR,NAME AS BRANCH,TYPE1 AS CODE FROM TYPE WHERE ID='B' ORDER BY CODE";
                        header_n = "Select Branch";
                    }
                    else
                    {
                        SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I' AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                        header_n = "Select Grade";
                    }
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F82521" || HCID == "F82585" || HCID == "F85162")
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15133")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    //////////MADE BY AKSHAY
                    case "F85150":// ATTENDANCE ENTRY
                    case "F82519"://SALARY RATE REPORT
                    //MADE BY YOGITA
                    case "F82525": //PAY SUMMARY DEPT WISE                                      
                    case "F82514"://PAY SUMMARY
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82576":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select  empcode as fstr,empcode,to_char(d_o_b,'dd/mm/yyyy') as d_o_b,name as emp_name,fhname as father_name,desg_text,conf_Dt  from empmas where branchcd='" + mbr + "' and dtjoin " + xprdrange + " and grade='" + value1 + "' and length(conf_dt)>1 order by empcode";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Employee", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "select 'YES' as fstr,'YES' as choice_,'Do You Want to Print on Letter Pad?' as option_ from dual union all select 'NO' as fstr,'NO'  as choice_,'Print on Plain Paper' as option_ from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Slect Choice", frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// selected empcode 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", value1);//choice
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82577":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select empcode as fstr,empcode,trim(branchcd)||trim(empcode) as cardno,to_char(d_o_b,'dd/mm/yyyy') as d_o_b,name as emp_name,fhname as father_name,desg_Text,deptt_text,to_char(dtjoin,'dd/mm/yyyy') as dtjoin from empmas where branchcd='" + mbr + "' and dtjoin " + xprdrange + " and grade='" + value1 + "' order by empcode";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Employee", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "select 'YES' as fstr,'YES' as choice_,'Do You Want to Print on Letter Pad?' as option_ from dual union all select 'NO' as fstr,'NO'  as choice_,'Print on Plain Paper' as option_ from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// selected empcode 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", value1);//choice
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82578":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;//month
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82579":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE  from type where id='B' ORDER BY CODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Branch", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1;//branchcd
                            SQuery = "select 'Y'  as fstr,'Yes' as optionn,'Do you want  gross sale' as choice from dual union all  select 'N'  as fstr,'No' as optionn,'Do you want  Basic sale' as choice from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfval.Value);// for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);//selected branchcd
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1);//selected option
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82582":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;//month
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            m1 = m1 + " " + year;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);//mth name
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82575":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select empcode as fstr,empcode,to_char(d_o_b,'dd/mm/yyyy') as dob,name as emp_name,fhname  as father_name,desg_text,deptt_text from empmas where branchcd='" + mbr + "' and grade='" + value1 + "' and nvl(trim(leaving_dt),'-')='-' order by empcode";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Employee", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1; //empcode
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F82584":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "select empcode as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg as desig,deptt as department from empmas where branchcd='" + mbr + "' and grade='" + value1 + "' order by fstr";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Employee", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1; //empcode
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F82585":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE  from type where id='B' ORDER BY CODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Branch", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1;//branchcd
                            SQuery = "select  'YES' as fstr,'YES' as choice,'Show Yearly Statement' as Option_ from dual union all select  'NO' as fstr,'NO' as choice,'Show Select Period Statement' as Option_ from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Show Yearly Statement", frm_qstr);
                        }
                        else if (hf2.Value == "")
                        {
                            hf2.Value = value1;//selection
                            if (hf2.Value == "YES")
                            {
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfval.Value);// for grade
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);//selected branchcd
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1);//selected option
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                                fgen.fin_pay_reps(frm_qstr);
                            }
                            else
                            {
                                fgen.Fn_open_prddmp1("-", frm_qstr);
                            }
                        }
                        break;

                    case "F82586":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;//month
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82574":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select 'YES' as fstr,'Do You Want Detailed Report' as header,'YES' AS Selection FROM DUAL UNION ALL   select 'NO' as fstr,'Do You Want Short Report' as header,'NO' AS Selection FROM DUAL";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1; //seletion value
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F82573":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select 'DOL' as fstr, 'Date of Leaving' as header,'DOL' AS choice from dual union all select 'POL' as fstr, 'Period of Leaving' as header,'POL' AS choice from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1; //choice
                            if (hf1.Value == "DOL") //DIRECT REPORT IS OPEN
                            {
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfval.Value);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hf1.Value);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                                fgen.fin_pay_reps(frm_qstr);
                            }
                            if (hf1.Value == "POL")
                            {
                                hf1.Value = value1; //for all grades or not 
                                fgen.Fn_open_prddmp1("-", frm_qstr);
                            }
                        }
                        break;

                    case "F82588":
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1;//month
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                            m1 = m1 + " " + year;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82521"://combined pay summary
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//branch
                            SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I'  AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Grade", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1;//grade
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + value1 + "'", "mthname");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for branchcd
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", value1);// for month
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", m1);//month name       
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", value1);//selected mth     
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82516": //PAY TREND SECTION WISE
                        //pay trend section wise
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1; //grade saved in it
                            SQuery = "SELECT 'Y' AS FSTR,'YES' AS SELECTION,'Gross Pay Data' as CHOICE FROM DUAL UNION ALL SELECT 'N' AS FSTR,'NO' AS SELECTION,'Net Pay Data' as choice from dual";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else
                        {
                            hf1.Value = value1; //selected Y OR NO is saved in it
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F82587":
                        hf1.Value = value1; //GRADE
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F85231":
                        hf1.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//for grade                           
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_pay_reps(frm_qstr);
                        break;

                    case "F82517":
                    case "F82523":
                        if (hfval.Value == "")
                        {
                            SQuery = "SELECT 'Y' AS FSTR,'YES' AS SELECTION,'Gross Pay Data' as CHOICE FROM DUAL UNION ALL SELECT 'N' AS FSTR,'NO' AS SELECTION,'Net Pay Data' as choice from dual";
                            header_n = "Select Choice";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                            hfval.Value = value1;
                        }
                        else
                        {
                            hf1.Value = value1;
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "F82515"://PAY REG QUARTERLY
                        hfval.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F82527"://salary slip...made by yogita
                        mq0 = fgen.seek_iname(frm_qstr, co_cd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
                        if (mq0 == "Y")
                        {
                            if (hf_branchact.Value == "")
                            {
                                hf_branchact.Value = value1;
                                SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I' AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Grade", frm_qstr);
                            }
                            else if (hfval.Value == "")
                            {
                                hfval.Value = value1;
                                SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Month", frm_qstr);
                            }
                            else if (hf1.Value == "")
                            {
                                hf1.Value = value1;
                                SQuery = "select empcode as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg as desig,deptt as department from empmas where branch_act='" + mbr + "' and grade='" + hfval.Value + "' and nvl(trim(leaving_dt),'-')='-' order by empcode";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Employee", frm_qstr);
                            }
                            else
                            {
                                m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1); //empcode
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", hf_branchact.Value);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                                fgen.fin_pay_reps(frm_qstr);
                            }
                        }
                        else
                        {
                            if (hfval.Value == "")
                            {
                                hfval.Value = value1;
                                SQuery = "SELECT MTHNUM AS FSTR,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Month", frm_qstr);
                            }
                            else if (hf1.Value == "")
                            {
                                hf1.Value = value1;
                                SQuery = "select empcode as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg as desig,deptt as department from empmas where branchcd='" + mbr + "' and grade='" + hfval.Value + "' and nvl(trim(leaving_dt),'-')='-' order by empcode";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Employee", frm_qstr);
                            }
                            else
                            {
                                m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hf1.Value + "'", "mthname");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month value 
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1); //empcode
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                                fgen.fin_pay_reps(frm_qstr);
                            }
                        }
                        break;

                    case "F82505": //Identity Card
                    case "F82507": //NEW joining card                   
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1; //grade
                            SQuery = "select empcode as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg as desig,deptt as department from empmas where branchcd='" + mbr + "' and grade='" + hfval.Value + "' order by empcode";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Employee", frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", hfval.Value); //grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", value1); // empcode
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F82512"://LIST OF MOBILE NUMBERS
                    case "F82530"://LIST OF ADDRESS
                    case "F82501": //list of blood group                                          
                    case "F82510"://list of lanlines      
                        hfval.Value = value1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_pay_reps(frm_qstr);
                        break;

                    case "F82503"://category wise summary report
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;
                            SQuery = "select mthnum as fstr,mthnum as code,mthname as month from mths";
                            header_n = "Select Month";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + value1 + "'", "mthname");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", value1);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F85142"://PAY/SALARY REGISTER
                        //PAY REGISTER
                        mq0 = fgen.seek_iname(frm_qstr, co_cd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
                        if (mq0 == "Y")
                        {
                            if (hf_branchact.Value == "")
                            {
                                hf_branchact.Value = value1;
                                SQuery = "select TYPE1 AS FSTR,NAME AS GRADE_NAME,TYPE1 AS GRADE_CODE FROM TYPE WHERE ID='I' AND SUBSTR(TYPE1,1,1)<'2' ORDER BY FSTR";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Grade", frm_qstr);
                            }
                            else if (hfval.Value == "")
                            {
                                hfval.Value = value1;
                                SQuery = "select distinct to_char(date_,'mm/yyyy') as fstr,vchnum as entry_no, to_char(date_,'mm/yyyy') as month_year,ent_by,to_char(date_,'yyyymmdd') as vdd from pay where branch_act='" + mbr + "' and grade='" + hfval.Value.Trim() + "' and date_ " + xprdrange + " order by vdd desc,entry_no desc";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Entry", frm_qstr);
                            }
                            else
                            {
                                hf1.Value = value1;
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month/year 
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hf_branchact.Value);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85142");
                                fgen.fin_pay_reps(frm_qstr);
                            }
                        }
                        else
                        {
                            if (hfval.Value == "")
                            {
                                hfval.Value = value1;
                                SQuery = "select distinct to_char(date_,'mm/yyyy') as fstr,vchnum as entry_no, to_char(date_,'mm/yyyy') as month_year,ent_by,to_char(date_,'yyyymmdd') as vdd from pay where branchcd='" + mbr + "' and grade='" + hfval.Value.Trim() + "' and date_ " + xprdrange + " order by vdd desc,entry_no desc";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_sseek("Select Entry", frm_qstr);
                            }
                            else
                            {
                                hf1.Value = value1;
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//for grade
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hf1.Value);// for month/year 
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85142");
                                fgen.fin_pay_reps(frm_qstr);
                            }
                        }
                        break;

                    case "F85147"://Date of Joining List                   
                        #region
                        if (hfval.Value == "")
                        {
                            hfval.Value = value1;//grade
                            SQuery = "select 'DOJ' AS FSTR,'DATE OF JOINING (DOJ)' AS CHOICE_,'DOJ' AS SELECTION FROM DUAL UNION ALL select 'POJ' AS FSTR,'PERIOD OF JOINING (POJ)' AS CHOICE_,'POJ' AS SELECTION  FROM DUAL";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Choice", frm_qstr);
                        }
                        else if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "select 'Y' AS FSTR,'Do You Want All Joinees' AS CHOICE_,'Yes' AS SELECTION  FROM DUAL UNION ALL select 'N' AS FSTR,'No for Current Employees' AS CHOICE_,'No'  AS SELECTION  FROM DUAL";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Selection", frm_qstr);
                        }
                        else
                        {
                            if (hf1.Value == "DOJ")
                            {
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);//for selection (either DOJ or POJ)
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hfval.Value);//for grade    
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", value1);
                                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85147");
                                fgen.fin_pay_reps(frm_qstr);
                            }
                            else
                            {
                                hf2.Value = value1;
                                fgen.Fn_open_prddmp1("-", frm_qstr);
                            }
                        }
                        #endregion
                        break;

                    case "F85148"://LIST OF BIRTHDAY
                        hf1.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//for grade                           
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85148");
                        fgen.fin_pay_reps(frm_qstr);
                        break;

                    case "F85149"://LIST OF BIRTHDAY
                        hf1.Value = value1; //                                                    
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//for grade                           
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85149");
                        fgen.fin_pay_reps(frm_qstr);
                        break;

                    case "F85152":
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,A.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as dated,A.type,a.ent_by,a.branchcd,a.grade as grade_code,t.name as grade,to_Char(a.vchdate,'yyyymmdd') as vdd from wbpayh A,type t where trim(a.grade)=trim(t.type1) and t.id='I' and a.grade='" + value1.Trim() + "' and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + "  order by vdd desc,A.vchnum desc";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek(header_n, frm_qstr);
                        }
                        else
                        {
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hf1.Value); //GRADE
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1); // vchnum & vchdate
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F85232":
                        if (hf1.Value == "")
                        {
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNUM,MTHNAME FROM MTHS";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                            hf1.Value = value1;
                        }
                        else if (hfval.Value == "")
                        {
                            if (Convert.ToInt32(value1) > 3)
                            {

                            }
                            else
                            {
                                i0 = Convert.ToInt32(year) + 1;
                                year = Convert.ToString(i0);
                            }
                            SQuery = "SELECT a.empcode as fstr, A.VCHNUM,A.EMPCODE,B.NAME AS EMPNAME,TO_CHAR(DATE_,'DD/MM/YYYY') AS VCHDATE,b.email FROM PAY A ,EMPMAS B  WHERE trim(a.branchcd)||TRIM(A.EMPCODE)=trim(b.branchcd)||TRIM(B.EMPCODE) and a.grade='" + hf1.Value + "' AND a.branchcd='" + mbr + "' and TO_CHAR(DATE_,'MM/YYYY')='" + value1 + "/" + year + "' order by empcode";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek(header_n, frm_qstr);
                            hfval.Value = value1;
                        }
                        else
                        {
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + hfval.Value + "'", "mthname");
                            //m1 = m1 + " " + year;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfval.Value);//mth
                            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfval.Value + "/" + year);// for month value 
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1); //empcode
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", hf1.Value); //grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F85158":
                        hf1.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "F85161":
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            SQuery = "SELECT MTHNUM AS FSTR,MTHNUM,MTHNAME FROM MTHS";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Month", frm_qstr);
                        }
                        else
                        {
                            m1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where trim(mthnum)='" + value1 + "'", "mthname");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hf1.Value); //grade
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", value1); //month
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", m1); //month name
                            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                            fgen.fin_pay_reps(frm_qstr);
                        }
                        break;

                    case "F85162":
                        hf1.Value = value1;//SELECTED GRADE                                                                            
                        fgen.Fn_open_dtbox("-", frm_qstr);
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

            // after prdDmp this will run            
            switch (val)
            {
                case "zz":
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE");
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "zz");
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F82517"://PAY TREND DEPT WISE
                case "F82523"://GROSS PAY TREND DEPT WSIE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value); //yes or no
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F82515"://PAY REG QUARTERLY
                    DateTime dd1, dd2;
                    dd1 = Convert.ToDateTime(fromdt);
                    dd2 = Convert.ToDateTime(todt);
                    TimeSpan ts = dd2 - dd1;
                    int dys = ts.Days / 30;
                    if (dys > 3)
                    {
                        fgen.msg("-", "AMSG", "Please Select Upto a Quarter (Max 3 Months Alllowed)!!");
                        return;
                    }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfval.Value); //yes or no
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_pay_reps(frm_qstr);
                    }
                    break;

                case "F82516": //PAY TREND SECTION WISE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfval.Value); //grade
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hf1.Value); //yes or no
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F82587":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hf1.Value); //GRADE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F82573":
                case "F82574":
                case "F82575":
                case "F82584":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hfval.Value); //GRADE
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hf1.Value); //selection pod                  
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F82585":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", hfval.Value);// for grade
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);//selected branchcd
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", value1);//selected option
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F85147":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value);//for selection (either DOJ or POJ)
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", hfval.Value);//for grade    
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85147");
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F85158":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);//grade
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "F85162":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf1.Value);// for grade
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", value1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_pay_reps(frm_qstr);
                    break;
            }
        }
    }
}