using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Text;


public partial class DeskDash2 : System.Web.UI.Page
{

    string popvar, tco_cd, co_cd, cdt1, cdt2, Drill_Caption, modeid, mbr, year, uname, ulvl;
    string category, rowdata1, rowdata2, getstr, getstr1, getstr2, getstr3, getstr4;
    string yr_op;
    string query1, query2, query3, query4, query5;
    double run_Tot, AVG_CUST, AVG_CUST1, AVG_CUST2, NMTH, NMTH1, NMTH2, CUST, CUST1, CUST2;
    string ACUST, ACUST1, ACUST2, MCUST, MCUST1, MCUST2, var1, var2, var3, var4, var5, var6;
    int i;

    OracleConnection consql = new OracleConnection();
    OracleDataAdapter da;
    OracleDataReader dr;
    OracleCommand command1;
    StringBuilder sbgauge = new StringBuilder();
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    DataTable dt2 = new DataTable();
    DataTable dt3 = new DataTable();
    DataTable dt4 = new DataTable();

    fgenDB fgen = new fgenDB();
    string frm_url, frm_PageName, frm_qstr, frm_formID;
    string frm_prodsheet, frm_inspvch;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
            if (frm_url.Contains("STR"))
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
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    cdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!IsPostBack)
            {
                lblhead.Text = "MIS: Finance Dashboard - I";
                GetChartData("35101");
                MultiView1.ActiveViewIndex = 0;
            }

            frm_prodsheet = "prod_sheet";
            frm_inspvch = "inspvch";
            if (co_cd == "HPPI" || co_cd == "SPPI")
            {
                frm_prodsheet = "prod_sheetk";
                frm_inspvch = "inspvchk";
            }
        }
    }

    public void RowtoColumnData(string dquery, DataTable mydt, string mode)
    {

        dquery = dquery + " from " + frm_prodsheet + " a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> 'DD' and a.type in ('86','88') ";

        DataTable dtn1 = new DataTable();
        dtn1 = fgen.getdata(frm_qstr, co_cd, dquery);

        DataTable dtn2 = new DataTable();
        if (mode == "MD2") query1 = "select * from(Select TYPE1,name from type where id='4'  order by type1) where rownum<13";
        else if (mode == "MD1") query1 = "select * from(Select TYPE1,name from type where id='8'  order by type1) where rownum<13";
        dtn2 = fgen.getdata(frm_qstr, co_cd, query1);

        mydt.Columns.Add(new DataColumn("month_Name", typeof(String)));
        mydt.Columns.Add(new DataColumn("TOT_BAS", typeof(Decimal)));

        int jq = 0;
        for (int i = 0; i < dtn2.Rows.Count; i++)
        {
            try
            {
                if (Convert.ToDouble(dtn1.Rows[0][jq].ToString().Trim()) <= 0) { }
                else
                {
                    DataRow nrow = mydt.NewRow();
                    nrow["month_Name"] = dtn2.Rows[jq]["name"].ToString().Trim();
                    nrow["tot_bas"] = dtn1.Rows[0][jq].ToString().Trim();
                    mydt.Rows.Add(nrow);
                }
                jq = jq + 1;
            }
            catch { }
        }
    }

    public void GetChartData(string pageid)
    {
        query1 = ""; query2 = ""; query3 = ""; query4 = ""; query5 = "";
        switch (pageid)
        {
            case "35101":
                query1 = "select b.aname as Mont_Name, ROUND(sum(a.amt_sale)/100000,2) as saleamt,sum(a.TOTqty) as qty,a.acode from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> 'DD'  and a.type like '0%' and a.type<'08' and a.type!='04' group by b.aname,a.acode order by saleamt desc";

                query2 = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> '88'  and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";

                query3 = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,ROUND((sum(cramt)-sum(Dramt))/100000,2) as collection,sum(cramt)-sum(Dramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> '88'  and substr(type,1,1)='1' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";

                query4 = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,ROUND((sum(a.dramt)-sum(A.cramt))/100000,2) as expense,count(a.vchnum) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> '88' and substr(A.acode,1,1)='3' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";

                query5 = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,ROUND(sum(a.amt_sale)/100000,2)  as sale,sum(a.bill_qty) as qty,to_Char(a.vchdate,'YYYYMM') as mth from sale a where a.vchdate between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> '88' and a.type!='47' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
                break;

            case "35102":
                query1 = "Select b.deptt_text as deptt,ROUND(sum(a.totern)/100000,2) as totl from pay a,empmas b where  a.branchcd <> 'DD'  and a.date_  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode group by b.deptt_text order by totl desc ";

                query2 = "Select b.desg_text as desg,ROUND(sum(a.totern)/100000,2) as totl from pay a,empmas b where  a.branchcd <> 'DD'  and a.date_  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode group by b.desg_text order by totl desc ";

                query3 = "Select c.name,B.grade,ROUND(sum(a.totern)/100000,2) as totl from pay a,empmas b,type c  WHERE a.grade=c.type1 and c.id='I' and type1 like '0%' and  a.branchcd <> 'DD'  and a.date_  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and a.branchcd||a.grade||a.empcode=b.branchcd||b.grade||b.empcode  group by B.GRADE,c.name order by totl desc ";

                query4 = "select * from (select upper(month_name) as month_name,ROUND(sum(past_yr)/100000,2) as past_yr,ROUND(sum(curr_yr)/100000,2) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.date_,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.totern) as curr_yr,to_Char(a.date_,'MM') as mth,0 from pay a where a.date_   between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and   a.branchcd <> 'DD'    group by to_Char(a.date_,'MM') ,substr(to_Char(a.date_,'MONTH'),1,3) union all select substr(to_Char(a.date_,'MONTH'),1,3) as Month_Name,sum(a.totern) as past_yr,0 as curr_yr,to_Char(a.date_,'MM') as mth,0 from pay a where a.date_   between to_date('01/04/2012','dd/mm/yyyy')-1 and to_Date('" + cdt1 + "','dd/mm/yyyy')-1 and   a.branchcd <> 'DD'    group by to_Char(a.date_,'MM') ,substr(to_Char(a.date_,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";
                break;

            case "35103":
                query1 = "select b.aname as Mont_Name,ROUND(sum(a.amt_sale)/100000,2) as saleamt,sum(a.bill_qty) as qty,a.acode from sale a,famst b where trim(a.acode)=trim(b.acode) and a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> 'DD'  and a.type!='47' group by b.aname,a.acode order by saleamt desc ";

                ACUST = ""; MCUST = "";
                CUST = 0; NMTH = 0; AVG_CUST = 0;

                query2 = "Select mth,count(*) as customers from (Select distinct to_Char(vchdate,'YYYYMM') as mth,acode from sale where branchcd <> 'DD' and vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy')) group by mth";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, query2);
                foreach (DataRow dr3 in dt.Rows)
                {
                    CUST = CUST + Convert.ToDouble(dr3["customers"].ToString().Trim());
                    NMTH = NMTH + 1;
                }

                AVG_CUST = System.Math.Round(CUST / NMTH, 0);
                if (AVG_CUST.ToString() == "NaN") AVG_CUST = 0;
                query2 = "";
                query2 = "Select ROUND((count(*) /" + AVG_CUST + ")*100,0) as CUST_TCH from (Select distinct to_Char(vchdate,'YYYYMM') as mth,acode from sale where branchcd <> 'DD' and TO_CHAr(vchdate,'YYYYMM')= '201402')";

                try
                {
                    ACUST = Convert.ToString(AVG_CUST).ToString().Replace(",", "");
                    ACUST = Spell.SpellAmount.comma(Convert.ToDecimal(ACUST));
                }
                catch { }


                query3 = "Select to_Char(vchdate,'YYYYMM') as mth,sum(amt_sale) as tot from sale where branchcd <> 'DD' and type not in ('47','4A') and vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') group by to_Char(vchdate,'YYYYMM')";



                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, query3);
                foreach (DataRow dr3 in dt.Rows)
                {
                    CUST1 = CUST1 + Convert.ToDouble(dr3["tot"].ToString().Trim());
                    NMTH1 = NMTH1 + 1;
                }

                AVG_CUST1 = System.Math.Round(CUST1 / NMTH1, 0);

                if (AVG_CUST1.ToString() == "NaN") AVG_CUST1 = 0;

                query3 = "";
                query3 = "Select ROUND((tot /" + AVG_CUST1 + ")*100,2) as CUST_TCH , tot from (Select to_Char(vchdate,'YYYYMM') as mth,sum(amt_sale) as tot from sale where branchcd <> 'DD' and type not in ('47','4A') and TO_CHAr(vchdate,'YYYYMM')= '201402')";

                try
                {
                    ACUST1 = Convert.ToString(AVG_CUST1).ToString().Replace(",", "");
                    ACUST1 = Spell.SpellAmount.comma(Convert.ToDecimal(ACUST1));
                }
                catch { }

                var3 = "Current Month Sales = " + MCUST1 + "";
                var4 = "Average Monthly Sales = " + ACUST1 + "";

                query4 = "Select to_Char(vchdate,'YYYYMM') as mth,sum(amt_sale) as tot from sale where branchcd <> 'DD' and type not in ('47','4A') and vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') group by to_Char(vchdate,'YYYYMM')";


                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, query4);
                foreach (DataRow dr3 in dt.Rows)
                {
                    CUST2 = CUST2 + Convert.ToDouble(dr3["tot"].ToString().Trim());
                    NMTH2 = NMTH2 + 1;
                }

                AVG_CUST2 = System.Math.Round(CUST2 / NMTH2, 0);
                double proj_12mth = System.Math.Round(AVG_CUST2 * 12, 0);

                MCUST2 = fgen.seek_iname(frm_qstr, co_cd, "Select sum(amt_sale) as tot from sale where branchcd <> 'DD' and type not in ('47','4A') and vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('08/02/2014','dd/mm/yyyy') ", "tot");


                if (MCUST2.ToString() == "NaN" || MCUST2 == "") MCUST2 = "0";
                if (proj_12mth.ToString() == "NaN") proj_12mth = 0;

                query4 = "";
                query4 = "Select ROUND((" + MCUST2 + "/" + proj_12mth + ")* 100,2) as CUST_TCH from dual";

                try
                {
                    ACUST2 = Convert.ToString(AVG_CUST2).ToString().Replace(",", "");
                    ACUST2 = Spell.SpellAmount.comma(Convert.ToDecimal(ACUST2));
                }
                catch { }

                try
                {
                    NMTH2 = System.Math.Round(Convert.ToDouble(MCUST2), 0);
                    MCUST2 = Convert.ToString(NMTH2).Replace(",", "");
                    MCUST2 = Spell.SpellAmount.comma(Convert.ToDecimal(MCUST2));
                }
                catch { }

                var5 = " YTD Sales = " + MCUST + "";
                var6 = "Target = Average Monthly Sales x 12 = " + ACUST + "";

                break;

            case "35104":

                query1 = "select Month_Name,ROUND(sum(sales)/100000,2) as sales,ROUND(sum(collection)/100000,2)  as collection, mth  from (select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,0 as sales,sum(cramt)-sum(Dramt) as collection,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> '88'  and substr(type,1,1)='1' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(dramt)-sum(cramt) as sales,0 as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> '88'  and substr(type,1,1)='4' and substr(acode,1,2) IN('16') and type!='47' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) ) group by Month_Name,mth order by mth";

                query2 = "select * from (select upper(month_name) as month_name,ROUND(sum(past_yr)/100000,2) as past_yr,ROUND(sum(curr_yr)/100000,2) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.ORDDT,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.qtyord*a.irate) as curr_yr,to_Char(a.ORDDT,'MM') as mth,0 from sOMAS a where a.ORDDT   between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and   a.branchcd <> 'DD'    group by to_Char(a.ORDDT,'MM') ,substr(to_Char(a.ORDDT,'MONTH'),1,3) union all select substr(to_Char(a.ORDDT,'MONTH'),1,3) as Month_Name,sum(a.qtyord*a.irate) as past_yr,0 as curr_yr,to_Char(a.ORDDT,'MM') as mth,0 from sOMAS a where a.ORDDT   between to_date('01/04/2012','dd/mm/yyyy')-1 and to_Date('" + cdt1 + "','dd/mm/yyyy')-1 and   a.branchcd <> 'DD'    group by to_Char(a.ORDDT,'MM') ,substr(to_Char(a.ORDDT,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";

                query3 = "select * from (select upper(month_name) as month_name,ROUND(sum(past_yr)/100000,2) as past_yr,ROUND(sum(curr_yr)/100000,2) as curr_yr,mthnum,max(mthsno) as mthsno  from (select substr(mthname,1,3) as month_name,0 as past_yr,0 as curr_yr,mthnum,mthsno from mths union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,0 as past_yr,sum(a.amt_sale) as curr_yr,to_Char(a.vchdate,'MM') as mth,0 from sale a where a.vchdate   between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and   a.branchcd <> 'DD'    group by to_Char(a.vchdate,'MM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) union all select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(a.amt_sale) as past_yr,0 as curr_yr,to_Char(a.vchdate,'MM') as mth,0 from sale a where a.vchdate   between to_date('01/04/2012','dd/mm/yyyy')-1 and to_Date('" + cdt1 + "','dd/mm/yyyy')-1 and   a.branchcd <> 'DD'    group by to_Char(a.vchdate,'MM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) ) group by upper(month_name),mthnum ) order by mthsno";

                query4 = "Select * from(Select * from(select substr(b.aname,1,10) as aname, trim(a.acode) as acode,sum(a.schedule) as schedule,sum(a.sale) as sales from (select trim(acode) as acode,trim(icode) as icode,sum(schedule)*max(irate) as schedule,sum(sale)*max(iratE) as sale from( select acode,icode,BUDGETCOST as schedule,0 as sale,0 as irate from BUDGMST a where branchcd!='DD' and type='46' and to_chaR(vchdate,'yyyymm') ='201402' union all select acode,icode,0 as schedule,iqtyout as sale,0 as irate from ivoucher a where branchcd!='DD' and type like '4%' and to_chaR(vchdate,'yyyymm')='201402' union all select distinct acode,icode,0 as schedule,0 as sale,irate from somas a where branchcd!='DD' and type like '4%' and trim(nvl(icat,'-'))<>'Y') group by trim(Acode),trim(icode))a,famst b where trim(A.acodE)=trim(B.acode) group by b.aname,trim(a.Acode) having sum(a.schedule)>0 order by b.aname)order by schedule desc )where rownum<11 ";

                string xstr = "", kyrstr = "";

                query1 = "Select to_char(fmdate,'dd/mm/yyyy') as fmdate,to_char(todate,'dd/mm/yyyy') as todate from co where substr(code,1,length(trim(code))-4) like 'JACL' and fmdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('10/02/2014','dd/mm/yyyy') order by fmdate";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, co_cd, query1);
                foreach (DataRow dr in dt.Rows)
                {
                    kyrstr = dr["fmdate"].ToString().Trim().Substring(8, 2) + "-" + dr["todate"].ToString().Trim().Substring(8, 2);
                    xstr = xstr + "SELECT 'F.Y :'||'" + kyrstr + "' as Month_Name,round(sum(a.Amt_sale)/10000000,2) as tot_bas,round(sum(a.bill_tot)/10000000,2) as tot_Gross from sale a where a.branchcd <> 'DD' and a.type!='47' and a.vchdate between to_DATE('" + dr["fmdate"].ToString().Trim() + "','dd/mm/yyyy') and to_DATE('" + dr["todate"].ToString().Trim() + "','dd/mm/yyyy') group by 'F.Y :'||'" + kyrstr + "' union all ";
                }
                query5 = xstr + " SELECT '-' as yrstr,0 as Bas_tot,0 as gr_tot from sale where 1=2 ";
                break;

            case "35105":

                query1 = "select round(sum(a.a1),2) as rzn1,round(sum(a.a2),2) as rzn2,round(sum(a.a3),2) as rzn3,round(sum(a.a4),2) as rzn4,round(sum(a.a5),2) as rzn5,round(sum(a.a6),2) as rzn6,round(sum(a.a7),2) as rzn7,round(sum(a.a8),2) as rzn8,round(sum(a.a9),2) as rzn9,round(sum(a.a10),2) as rzn10,round(sum(a.a11),2) as rzn11,round(sum(a.a12),2) as rzn12 ";

                query2 = "select round(sum(a.num1)/60,2) as rzn1,round(sum(a.num2)/60,2) as rzn2,round(sum(a.num3)/60,2) as rzn3,round(sum(a.num4)/60,2) as rzn4,round(sum(a.num5)/60,2) as rzn5,round(sum(a.num6)/60,2) as rzn6,round(sum(a.num7)/60,2) as rzn7,round(sum(a.num8)/60,2) as rzn8,round(sum(a.num9)/60,2) as rzn9,round(sum(a.num10)/60,2) as rzn10,round(sum(a.num11)/60,2) as rzn11,round(sum(a.num12)/60,2) as rzn12 ";

                query3 = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round((((sum(nvl(a.a2,0)+nvl(a.a4,0)))-(sum(nvl(a.a2,0))))/sum(nvl(a.a2,0)+nvl(a.a4,0)))*1000000,0) as tot_bas,to_Char(a.vchdate,'YYYYMM') as mth from " + frm_prodsheet + " a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> 'DD'  and a.type in ('86','88') group by to_Char(a.vchdate,'YYYYMM'),substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";

                query4 = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round(sum(a.num1+a.num2+a.num3+a.num4+a.num5+a.num6+ a.num7+a.num8+a.num9+a.num10+a.num11+ a.num12)/60,2) as tot_bas,to_Char(a.vchdate,'YYYYMM') as mth from " + frm_prodsheet + " a where a.vchdate  between to_date('" + cdt1 + "','dd/mm/yyyy') and to_Date('" + cdt2 + "','dd/mm/yyyy') and  a.branchcd <> 'DD'  and a.type in ('86','88') group by to_Char(a.vchdate,'YYYYMM'),substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";

                break;

        }

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, co_cd, query1);

        dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, co_cd, query2);

        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, co_cd, query3);

        dt3 = new DataTable();
        dt3 = fgen.getdata(frm_qstr, co_cd, query4);

        if (pageid == "35102" || pageid == "35103" || pageid == "35105") { }
        else
        {
            dt4 = new DataTable();
            dt4 = fgen.getdata(frm_qstr, co_cd, query5);
        }

        string errormsg = "No Record Exist";

        modeid = "";

        switch (pageid)
        {

            case "35101":


                if (dt.Rows.Count == 0) chart1.Visible = false;
                else
                {
                    rowdata1 = ""; getstr = ""; Drill_Caption = "";

                    Drill_Caption = "Pie chart: Purchase breakup";

                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);

                    rowdata1 = HelperClass.DataTableToJSArray(dt, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr = DChart.openPiechart("ContentPlaceHolder1_chart1", "pie", Drill_Caption, dt.Columns[1].ColumnName, rowdata1, modeid);
                }

                if (dt1.Rows.Count == 0) chart2.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr1 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Month wise Debtor Closing Balance";

                    yr_op = fgen.seek_iname(frm_qstr, co_cd, "Select sum(yr_" + year + ") as opt from famstbal a where  a.branchcd <> 'DD'   and substr(a.Acode,1,2)='16'", "opt");
                    if (yr_op == "") yr_op = "0";

                    dt1.Columns.Add(new DataColumn("cum_tot", typeof(Decimal)));

                    foreach (DataRow dr in dt1.Rows)
                    {
                        if (i == 0)
                            run_Tot = run_Tot + (Convert.ToDouble(yr_op) + Convert.ToDouble(dr["tot_bas"].ToString()));
                        else
                            run_Tot = run_Tot + Convert.ToDouble(dr["tot_bas"].ToString());

                        run_Tot = System.Math.Round((run_Tot / 100000), 2);

                        dr["cum_tot"] = run_Tot;
                        i++;
                    }
                    dt1.Columns.RemoveAt(3);
                    dt1.Columns.RemoveAt(2);
                    dt1.Columns.RemoveAt(1);

                    rowdata1 = WrapprClass.DataTableToJSArray(dt1, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt1, 0, "").Trim();

                    getstr1 = DChart.openColumnchart("ContentPlaceHolder1_chart2", "column", Drill_Caption, "", category, "Amount", dt1.Columns[1].ColumnName, "", rowdata1, "", "MWCB@");
                }

                if (dt2.Rows.Count == 0) chart3.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr2 = ""; category = ""; Drill_Caption = "";


                    Drill_Caption = "Collection Trend: YTD";

                    dt2.Columns.RemoveAt(3);
                    dt2.Columns.RemoveAt(2);

                    rowdata1 = WrapprClass.DataTableToJSArray(dt2, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt2, 0, "").Trim();

                    getstr2 = DChart.openLinechart("ContentPlaceHolder1_chart3", "", Drill_Caption, "", category, "Amount", dt2.Columns[1].ColumnName, "", rowdata1, "", modeid);
                }

                if (dt3.Rows.Count == 0) chart4.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr3 = ""; category = ""; Drill_Caption = "";
                    Drill_Caption = "Expense Trend: YTD";

                    dt3.Columns.RemoveAt(3);
                    dt3.Columns.RemoveAt(2);

                    rowdata1 = WrapprClass.DataTableToJSArray(dt3, 1, "");
                    category = WrapprClass.DataTableToJSArray(dt3, 0, "");

                    getstr3 = DChart.openBColumnchart("ContentPlaceHolder1_chart4", "bar", Drill_Caption, "", category, "Amount", dt3.Columns[1].ColumnName, "", rowdata1, "", modeid);
                }
                if (dt4.Rows.Count == 0) chart5.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr4 = ""; category = ""; Drill_Caption = "";
                    Drill_Caption = "Sales Trend";

                    dt4.Columns.RemoveAt(3);
                    dt4.Columns.RemoveAt(2);

                    rowdata1 = WrapprClass.DataTableToJSArray(dt4, 1, "");
                    category = WrapprClass.DataTableToJSArray(dt4, 0, "");

                    getstr4 = DChart.openAreachart("ContentPlaceHolder1_chart5", "areaspline", Drill_Caption, category, "Amount", dt4.Columns[1].ColumnName, "", rowdata1, "");
                }
                break;
            case "35102":

                if (dt.Rows.Count == 0) Div1.Visible = false;
                else
                {
                    rowdata1 = ""; getstr = ""; Drill_Caption = "";

                    Drill_Caption = "Pie chart: Salary Breakup Dept wise";

                    rowdata1 = HelperClass.DataTableToJSArray(dt, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr = DChart.openPiechart("ContentPlaceHolder1_Div1", "pie", Drill_Caption, dt.Columns[1].ColumnName, rowdata1, "DPTW");
                }

                if (dt1.Rows.Count == 0) Div2.Visible = false;
                else
                {
                    rowdata1 = ""; getstr1 = ""; Drill_Caption = "";

                    Drill_Caption = "Pie chart: Salary Breakup Desg wise";

                    rowdata1 = HelperClass.DataTableToJSArray(dt1, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr1 = DChart.openPiechart("ContentPlaceHolder1_Div2", "pie", Drill_Caption, dt1.Columns[1].ColumnName, rowdata1, "DSGW");

                }

                if (dt2.Rows.Count == 0) Div3.Visible = false;
                else
                {
                    rowdata1 = ""; getstr2 = ""; Drill_Caption = "";

                    Drill_Caption = "Pie chart: Salary Breakup Grade wise";

                    dt2.Columns.RemoveAt(1);
                    rowdata1 = HelperClass.DataTableToJSArray(dt2, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr2 = DChart.openPiechart("ContentPlaceHolder1_Div3", "pie", Drill_Caption, dt2.Columns[1].ColumnName, rowdata1, "GRDW");
                }

                if (dt3.Rows.Count == 0) Div4.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr3 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Month wise Debtor Closing Balance";

                    dt3.Columns.RemoveAt(4);
                    dt3.Columns.RemoveAt(3);

                    rowdata1 = WrapprClass.DataTableToJSArray(dt3, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt3, 0, "").Trim();
                    rowdata2 = WrapprClass.DataTableToJSArray(dt3, 2, "");

                    getstr3 = DChart.openColumnchart("ContentPlaceHolder1_Div4", "column", Drill_Caption, "", category, "Amount", dt3.Columns[1].ColumnName, dt3.Columns[2].ColumnName, rowdata1, rowdata2, "MWCB");
                }
                break;
            case "35103":
                if (dt.Rows.Count == 0) Div5.Visible = false;
                else
                {
                    rowdata1 = ""; getstr = ""; Drill_Caption = "";

                    Drill_Caption = "Pie chart: Sales breakup";

                    dt.Columns.RemoveAt(3);
                    dt.Columns.RemoveAt(2);

                    rowdata1 = HelperClass.DataTableToJSArray(dt, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr = DChart.openPiechart("ContentPlaceHolder1_Div5", "pie", Drill_Caption, dt.Columns[1].ColumnName, rowdata1, "");

                }
                if (dt1.Rows.Count == 0) Div6.Visible = false;
                else
                {
                    MCUST = dt1.Rows[0][0].ToString().Trim();
                    MCUST = MCUST.Replace(",", "");
                    MCUST = Spell.SpellAmount.comma(Convert.ToDecimal(MCUST));

                    var1 = "Current Month customers served = " + System.Math.Round((Convert.ToDouble(MCUST) * Convert.ToDouble(ACUST) / 100), 0) + "";
                    var2 = "Average number of customers served = " + ACUST + "";

                    rowdata1 = ""; getstr1 = ""; Drill_Caption = "";

                    Drill_Caption = "Speedometer: Average number of active Customers in a month";

                    rowdata1 = HelperClass.DataTableToJSArray(dt1, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr1 = DChart.openGaugechart1("ContentPlaceHolder1_Div6", "gauge", Drill_Caption, var1 + "<br>" + var2, "% Yr.Target", rowdata1);
                }

                if (dt2.Rows.Count == 0) Div7.Visible = false;
                else
                {

                    rowdata1 = ""; getstr2 = ""; Drill_Caption = "";

                    Drill_Caption = "Speedometer: Sales by value percent of monthly average";

                    rowdata1 = HelperClass.DataTableToJSArray(dt2, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr2 = DChart.openGaugechart1("ContentPlaceHolder1_Div7", "gauge", Drill_Caption, var3 + "<br>" + var4, "% Mth Avg", rowdata1);
                }
                if (dt3.Rows.Count == 0) Div8.Visible = false;
                else
                {

                    rowdata1 = ""; getstr3 = ""; Drill_Caption = "";

                    Drill_Caption = "Speedometer: Sales by value percent of monthly average";

                    rowdata1 = HelperClass.DataTableToJSArray(dt3, "").Trim();

                    rowdata1 = rowdata1.Remove(0, 1);
                    getstr3 = DChart.openGaugechart1("ContentPlaceHolder1_Div8", "gauge", Drill_Caption, var5 + "<br>" + var6, "% Yr.Target", rowdata1);
                }
                break;
            case "35104":
                if (dt.Rows.Count == 0) Div9.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Sales vs Collection Month Wise";



                    rowdata1 = WrapprClass.DataTableToJSArray(dt, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt, 0, "").Trim();
                    rowdata2 = WrapprClass.DataTableToJSArray(dt, 2, "");

                    getstr = DChart.openColumnchart("ContentPlaceHolder1_Div9", "column", Drill_Caption, "", category, "Amount", dt.Columns[1].ColumnName, dt.Columns[2].ColumnName, rowdata1, rowdata2, "MWCB1");
                }
                if (dt1.Rows.Count == 0) Div10.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr1 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "New SO Received Trend";



                    rowdata1 = WrapprClass.DataTableToJSArray(dt1, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt1, 0, "").Trim();
                    rowdata2 = WrapprClass.DataTableToJSArray(dt1, 2, "");

                    getstr1 = DChart.openColumnchart("ContentPlaceHolder1_Div10", "column", Drill_Caption, "", category, "Amount", dt1.Columns[1].ColumnName, dt1.Columns[2].ColumnName, rowdata1, rowdata2, "MWCB1");
                }
                if (dt2.Rows.Count == 0) Div11.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr2 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Comparison of CY sales to last year (Month in Month)";



                    rowdata1 = WrapprClass.DataTableToJSArray(dt2, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt2, 0, "").Trim();
                    rowdata2 = WrapprClass.DataTableToJSArray(dt2, 2, "");

                    getstr2 = DChart.openColumnchart("ContentPlaceHolder1_Div11", "column", Drill_Caption, "", category, "Amount", dt2.Columns[1].ColumnName, dt2.Columns[2].ColumnName, rowdata1, rowdata2, "MWCB1");
                }
                if (dt3.Rows.Count == 0) Div12.Visible = false;
                else
                {
                    rowdata1 = ""; rowdata2 = ""; getstr3 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Schedule vs Dispatch";



                    rowdata1 = WrapprClass.DataTableToJSArray(dt3, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt3, 0, "").Trim();
                    rowdata2 = WrapprClass.DataTableToJSArray(dt3, 2, "");

                    getstr3 = DChart.openColumnchart("ContentPlaceHolder1_Div12", "column", Drill_Caption, "", category, "Amount", dt3.Columns[1].ColumnName, dt3.Columns[2].ColumnName, rowdata1, rowdata2, "MWCB1");
                }
                if (dt4.Rows.Count == 0) Div13.Visible = false;
                else
                {
                    rowdata1 = ""; getstr4 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Comparison of CY sales to last year (Totals)";



                    rowdata1 = WrapprClass.DataTableToJSArray(dt4, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt4, 0, "").Trim();

                    getstr4 = DChart.openColumnchart("ContentPlaceHolder1_Div13", "column", Drill_Caption, "", category, "Amount", dt4.Columns[1].ColumnName, "", rowdata1, "", "");
                }
                break;
            case "35105":
                if (dt.Rows.Count == 0) Div14.Visible = false;
                else
                {
                    rowdata1 = ""; getstr = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Main Quality Problems";

                    rowdata1 = WrapprClass.DataTableToJSArray(dt, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt, 0, "").Trim();

                    getstr = DChart.openColumnchart("ContentPlaceHolder1_Div14", "column", Drill_Caption, "", category, "Amount", dt.Columns[1].ColumnName, "", rowdata1, "", "MWCBL");
                }
                if (dt1.Rows.Count == 0) Div15.Visible = false;
                else
                {
                    rowdata1 = ""; getstr1 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Main Down Time reasons";

                    rowdata1 = WrapprClass.DataTableToJSArray(dt1, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt1, 0, "").Trim();

                    getstr1 = DChart.openColumnchart("ContentPlaceHolder1_Div15", "column", Drill_Caption, "", category, "Amount", dt1.Columns[1].ColumnName, "", rowdata1, "", "MWCBL");
                }
                if (dt2.Rows.Count == 0) Div16.Visible = false;
                else
                {
                    rowdata1 = ""; getstr2 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Monthly Production PPM";

                    rowdata1 = WrapprClass.DataTableToJSArray(dt2, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt2, 0, "").Trim();

                    getstr2 = DChart.openColumnchart("ContentPlaceHolder1_Div16", "column", Drill_Caption, "", category, "Amount", dt2.Columns[1].ColumnName, "", rowdata1, "", "MWCB@");
                }
                if (dt3.Rows.Count == 0) Div17.Visible = false;
                else
                {
                    rowdata1 = ""; getstr3 = ""; category = ""; Drill_Caption = "";

                    Drill_Caption = "Monthly Down Time in Hrs";

                    rowdata1 = WrapprClass.DataTableToJSArray(dt3, 1, "").Trim();
                    category = WrapprClass.DataTableToJSArray(dt3, 0, "").Trim();

                    getstr3 = DChart.openColumnchart("ContentPlaceHolder1_Div17", "column", Drill_Caption, "", category, "Amount", dt3.Columns[1].ColumnName, "", rowdata1, "", "MWCB@");
                }
                break;
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "KCall1", getstr, false);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "LCall1", getstr1, false);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "MCall1", getstr2, false);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "NCall1", getstr3, false);
        if (pageid == "35102" || pageid == "35103" || pageid == "35105") { }
        else ScriptManager.RegisterStartupScript(this, this.GetType(), "OCall1", getstr4, false);
    }
    protected void imgprev_Click(object sender, ImageClickEventArgs e)
    {
        int i = MultiView1.ActiveViewIndex;
        hfval.Value = "";
        lblhead.Text = "";

        switch (i)
        {
            case 5:
                lblhead.Text = "MIS: Production Dashboard";
                hfval.Value = "35105";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 4;
                break;
            case 4:
                lblhead.Text = "MIS: Sales Dashboard - I";
                hfval.Value = "35104";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 3;
                break;
            case 3:
                lblhead.Text = "MIS: Salary Dashboard";
                hfval.Value = "35103";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 2;
                break;
            case 2:
                lblhead.Text = "MIS: Finance Dashboard - II";
                hfval.Value = "35102";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 1;
                break;
            case 1:
                lblhead.Text = "MIS: Finance Dashboard - I";
                hfval.Value = "35101";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 0;
                break;
        }
    }
    protected void imgnext_Click(object sender, ImageClickEventArgs e)
    {
        movenext();
    }
    protected void timer1_Tick(object sender, EventArgs e)
    {
        movenext();
    }
    void movenext()
    {
        int i = MultiView1.ActiveViewIndex;
        hfval.Value = "";
        lblhead.Text = "";

        switch (i)
        {
            case 0:
                lblhead.Text = "MIS: Finance Dashboard - II";
                hfval.Value = "35102";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 1;
                break;
            case 1:
                lblhead.Text = "MIS: Salary Dashboard";
                hfval.Value = "35103";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 2;
                break;
            case 2:
                lblhead.Text = "MIS: Sales Dashboard - I";
                hfval.Value = "35104";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 3;
                break;
            case 3:
                lblhead.Text = "MIS: Production Dashboard";
                hfval.Value = "35105";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 4;
                break;
            case 4:
                lblhead.Text = "MIS: Finance Dashboard - I";
                hfval.Value = "35101";
                GetChartData(hfval.Value);
                MultiView1.ActiveViewIndex = 0;
                break;
        }
    }
}
public class HelperClass
{

    public static string DataTableToJSSEEKArray(DataTable dt, string modeid)
    {
        StringBuilder sb = new StringBuilder();
        string rowDataStr = "";
        double icount = 0;
        if (dt.Rows.Count > 0)
        {
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0)
                        rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                        rowDataStr += dr[dc].ToString();
                    else
                        rowDataStr += "'" + dr[dc].ToString().Replace("'", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'";
                }
                sb.Append(",");
                sb.Append("[" + rowDataStr + "]");
            }
        }
        return sb.ToString();
    }

    public static string DataTableToJSArray(DataTable dt, string modeid)
    {
        StringBuilder sb = new StringBuilder();
        string rowDataStr = "";
        double icount = 0;
        int nflg = 0;
        if (dt.Rows.Count > 0)
        {
            for (int count = 0; count < dt.Rows.Count; count++)
            {
                DataRow dr = dt.Rows[count];
                rowDataStr = "";
                foreach (DataColumn dc in dt.Columns)
                {
                    if (rowDataStr.Length > 0)
                        rowDataStr += ",";
                    if (dr[dc].GetType() == typeof(Int32) || dr[dc].GetType() == typeof(Double) || dr[dc].GetType() == typeof(Decimal))
                    {
                        if (count > 15)
                        {
                            icount = icount + Convert.ToDouble(dr[dc].ToString().Trim());
                            nflg = 1;
                        }
                        else
                            rowDataStr += dr[dc].ToString().Trim();
                    }
                    else
                    {
                        if (count > 15) { }
                        else
                        {
                            try
                            {
                                Double temp = Convert.ToDouble(dr[dc]);
                                rowDataStr += temp.ToString();
                            }
                            catch (Exception ex)
                            {
                                rowDataStr += "'" + dr[dc].ToString().Trim().Replace("'", "").Replace("\r\n", "").Replace("\n", "").Replace("\r", "") + "'";
                            }
                        }
                    }
                }
                if (icount > 0 || nflg == 1) { }
                else
                {
                    sb.Append(",");
                    sb.Append("[" + rowDataStr + "]");
                }
            }
        }
        if (icount == 0 && nflg == 0) { }
        else
        {
            rowDataStr = "";
            rowDataStr += "'OTHERS'";
            rowDataStr += ",";
            rowDataStr += icount.ToString();

            sb.Append(",");
            sb.Append("[" + rowDataStr + "]");
        }
        return sb.ToString();
    }
}

public class DChart
{
    public static StringBuilder sb = new StringBuilder();

    public static string openSolidGauge(string divname, string chartname, string title, string subtitle, string col1, string rowdata1)
    {
        sb = new StringBuilder();

        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: 'gauge',");
        sb.Append(@"plotBackgroundColor: '#FFF',");
        sb.Append(@"plotBackgroundImage: null,");
        sb.Append(@"plotBorderWidth: 0,");
        sb.Append(@"plotShadow: true");
        sb.Append(@"},");

        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");

        sb.Append(@"pane: {");
        sb.Append(@"startAngle: -90,");
        sb.Append(@"endAngle: 90,");
        sb.Append(@"center: ['50%', '100%'],");
        sb.Append(@"background: [{");
        sb.Append(@"backgroundColor: {");
        sb.Append(@"linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },");
        sb.Append(@"stops: [");
        sb.Append(@"[0, '#FFF'],");
        sb.Append(@"[1, '#333']");
        sb.Append(@"]");
        sb.Append(@"},");
        sb.Append(@"borderWidth: 10,");
        sb.Append(@"outerRadius: '50%'");
        sb.Append(@"}, {");
        sb.Append(@"backgroundColor: {");
        sb.Append(@"linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },");
        sb.Append(@"stops: [");
        sb.Append(@"[0, '#333'],");
        sb.Append(@"[1, '#FFF']");
        sb.Append(@"]");
        sb.Append(@"},");
        sb.Append(@"borderWidth: 1,");
        sb.Append(@"outerRadius: '50%'");
        sb.Append(@"}, {");
        // default background
        sb.Append(@"}, {");
        sb.Append(@"backgroundColor: '#DDD',");
        sb.Append(@"borderWidth: 0,");
        sb.Append(@"outerRadius: '50%',");
        sb.Append(@"innerRadius: '50%'");
        sb.Append(@"}]");
        sb.Append(@"},");

        // the value axis
        sb.Append(@"yAxis: {");
        sb.Append(@"min: 0,");
        sb.Append(@"max: 3,");

        sb.Append(@"minorTickInterval: 'auto',");
        sb.Append(@"minorTickWidth: 1,");
        sb.Append(@"minorTickLength: 10,");
        sb.Append(@"minorTickPosition: 'inside',");
        sb.Append(@"minorTickColor: '#666',");

        sb.Append(@"tickPixelInterval: 30,");
        sb.Append(@"tickWidth: 2,");
        sb.Append(@"tickPosition: 'inside',");
        sb.Append(@"tickLength: 10,");
        sb.Append(@"tickColor: '#666',");
        sb.Append(@"labels: {");
        sb.Append(@"step: 2,");
        sb.Append(@"rotation: 'auto'");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + col1 + "'");
        sb.Append(@"},");
        sb.Append(@"plotBands: [{");
        sb.Append(@"from: 0,");
        sb.Append(@"to: 1.10,");
        sb.Append(@"color: '#ff3f00'");
        sb.Append(@"}, {");
        sb.Append(@"from: 1.10,");
        sb.Append(@"to: 1.33,");
        sb.Append(@"color: '#ffaa00'");
        sb.Append(@"}, {");
        sb.Append(@"from: 1.33,");
        sb.Append(@"to: 3,");
        sb.Append(@"color: '#008000'");
        sb.Append(@"}]");
        sb.Append(@"},");

        sb.Append(@"series: [{");
        sb.Append(@"name: 'Ratios',");
        sb.Append(@"data: [" + rowdata1 + "],");
        sb.Append(@"tooltip: {");
        sb.Append(@"valueSuffix: ' Ratios'");
        sb.Append(@"}");
        sb.Append(@"}]");
        // Add some life
        sb.Append(@"});");
        sb.Append(@"});");
        sb.Append(@"</script>");
        return sb.ToString();

    }

    public static string openLinechart(string divname, string chartname, string title, string subtitle, string category, string vaxistext, string col1, string col2, string rowdata1, string rowdata2, string modeid)
    {
        sb = new StringBuilder();

        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "',");
        sb.Append(@"x: -20 ");
        sb.Append(@"},");
        sb.Append(@"subtitle: {");
        sb.Append(@"text: '" + subtitle + "',");
        sb.Append(@"x: -20");
        sb.Append(@"},");
        sb.Append(@"xAxis: {");
        sb.Append(@"categories: " + category + "");
        sb.Append(@"},");
        sb.Append(@"yAxis: {");
        sb.Append(@"title: {");
        sb.Append(@"text: 'Amount'");
        sb.Append(@"},");
        sb.Append(@"plotLines: [{");



        sb.Append(@"value: 0,");
        sb.Append(@"width: 1,");
        sb.Append(@"color: '#ED561B'");
        sb.Append(@"}]");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");
        sb.Append(@"valueSuffix: ''");
        sb.Append(@"},");
        sb.Append(@"legend: {");
        sb.Append(@"layout: 'vertical',");
        sb.Append(@"align: 'right',");
        sb.Append(@"verticalAlign: 'middle',");
        sb.Append(@"borderWidth: 0");
        sb.Append(@"},");
        sb.Append(@"series: [{");

        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: " + rowdata1 + "");

        sb.Append(@"}]");
        sb.Append(@"});");
        sb.Append(@"});");
        sb.Append(@"</script>");
        return sb.ToString();
    }

    public static string openBColumnchart(string divname, string chartname, string title, string subtitle, string category, string vaxistext, string col1, string col2, string rowdata1, string rowdata2, string modeid)
    {
        sb = new StringBuilder();

        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: '" + chartname + "'");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");
        sb.Append(@"subtitle: {");
        sb.Append(@"text: ''");
        sb.Append(@"},");
        sb.Append(@"xAxis: {");
        sb.Append(@"categories: " + category + ",");
        sb.Append(@"title: {");
        sb.Append(@"text: null");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"yAxis: {");
        sb.Append(@"min: 0,");
        sb.Append(@"title: {");
        sb.Append(@"text: 'Sales',");
        sb.Append(@"align: 'high'");
        sb.Append(@"},");
        sb.Append(@"labels: {");
        sb.Append(@"overflow: 'justify'");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");
        sb.Append(@"valueSuffix: ' Percentage'");
        sb.Append(@"},");
        sb.Append(@"plotOptions: {");


        sb.Append(@"series: {");
        sb.Append(@"colorByPoint: true");
        sb.Append(@"},");

        sb.Append(@"bar: {");
        sb.Append(@"dataLabels: {");
        sb.Append(@"enabled: true");
        sb.Append(@"}");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"legend: {");
        sb.Append(@"layout: 'vertical',");
        sb.Append(@"align: 'right',");
        sb.Append(@"verticalAlign: 'top',");
        sb.Append(@"x: -40,");
        sb.Append(@"y: 100,");
        sb.Append(@"floating: true,");
        sb.Append(@"borderWidth: 1,");
        sb.Append(@"backgroundColor: '#FFFFFF',");
        sb.Append(@"shadow: true");
        sb.Append(@"},");
        sb.Append(@"credits: {");
        sb.Append(@"enabled: false");
        sb.Append(@"},");
        sb.Append(@"series: [{");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: " + rowdata1 + "");
        sb.Append(@"}]");
        sb.Append(@"});");
        sb.Append(@"});");
        sb.Append(@"</script>");
        return sb.ToString();
    }


    public static string openColumnchart(string divname, string chartname, string title, string subtitle, string category, string vaxistext, string col1, string col2, string rowdata1, string rowdata2, string modeid)
    {
        sb = new StringBuilder();
        //sb.Append(@"radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },");

        sb.Append(@"<script type='text/javascript'>");

        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: '" + chartname + "'");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");
        sb.Append(@"subtitle: {");
        sb.Append(@"text: '" + subtitle + "'");
        sb.Append(@"},");
        sb.Append(@"xAxis: {");
        sb.Append(@"categories: ");
        sb.Append(@"" + category + "");

        if (modeid == "MWCBL")
        {
            sb.Append(@",labels: {");
            sb.Append(@"rotation: -25,");
            sb.Append(@"align: 'right',");
            sb.Append(@"}");
        }

        sb.Append(@"},");

        sb.Append(@"yAxis: {");
        sb.Append(@"min: 0,");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + vaxistext + "'");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");

        //sb.Append(@"headerFormat: <span style = 'font-size:10px' >{point.key}</span><table>,");
        //sb.Append(@"pointFormat: <tr><td style = 'color:{series.color};padding:0' >{series.name}: </td> +");
        //sb.Append(@"<td style= 'padding:0' ><b>{point.y:.1f} mm</b></td></tr>,");
        //sb.Append(@"footerFormat: </table>,");

        sb.Append(@"shared: true,");
        sb.Append(@"useHTML: true");
        sb.Append(@"},");
        sb.Append(@"plotOptions: {");

        if (modeid == "MWCB" || modeid == "MWCB@" || modeid == "MWCBL")
        {
            sb.Append(@"series: {");
            sb.Append(@"colorByPoint: true");
            sb.Append(@"},");
        }


        sb.Append(@"column: {");
        sb.Append(@"dataLabels: {");
        sb.Append(@"enabled: true");
        sb.Append(@"},");
        sb.Append(@"pointPadding: 0.55,");
        sb.Append(@"borderWidth: 5");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"series: [{");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: " + rowdata1 + "");

        if (modeid == "MWCB" || modeid == "MWCB1")
        {
            sb.Append(@"}, {");
            sb.Append(@"name: '" + col2 + "',");
            sb.Append(@"data: " + rowdata2 + "");
        }


        sb.Append(@"}]");
        sb.Append(@"});");
        sb.Append(@"});");


        sb.Append(@"</script>");
        return sb.ToString();
    }

    public static string openFunnelchart(string divname, string chartname, string title, string col1, string rowdata1)
    {

        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: '" + chartname + "',");
        sb.Append(@"marginRight: 100");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "',");
        sb.Append(@"x: -50");
        sb.Append(@"},");
        sb.Append(@"plotOptions: {");
        sb.Append(@"series: {");
        sb.Append(@"dataLabels: {");
        sb.Append(@"enabled: true,");
        sb.Append(@"format: '<b>{point.name}</b> ({point.y:,.0f})',");
        sb.Append(@"color: 'black',");
        sb.Append(@"softConnector: true");
        sb.Append(@"},");
        sb.Append(@"neckWidth: '30%',");
        sb.Append(@"neckHeight: '25%'");

        //-- Other available options
        // height: pixels or percent
        // width: pixels or percent
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"legend: {");
        sb.Append(@"enabled: true");
        sb.Append(@"},");
        sb.Append(@"series: [{");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: [");
        sb.Append(@"" + rowdata1 + "");
        sb.Append(@"]");
        sb.Append(@"}]");
        sb.Append(@"});");
        sb.Append(@"});");
        sb.Append(@"</script>");
        return sb.ToString();
    }

    public static string openPiechart(string divname, string chartname, string title, string col1, string rowdata1, string modeid)
    {
        sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");

        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"plotBackgroundColor: null,");
        sb.Append(@"plotBorderWidth: null,");
        sb.Append(@"plotShadow: false");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");
        sb.Append(@"formatter: function() {");
        sb.Append(@"return 'Vendor ' +  this.point.name + '('+ this.y + ')';");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"plotOptions: {");
        sb.Append(@"pie: {");
        sb.Append(@"allowPointSelect: true,");
        if (modeid == "DPTW" || modeid == "DSGW") { }
        else
        {
            sb.Append(@"innerSize: '30%',");
        }
        sb.Append(@"cursor: 'pointer',");
        sb.Append(@"dataLabels: {");
        sb.Append(@"enabled: true,");
        sb.Append(@"color: '#000000',");
        sb.Append(@"connectorColor: '#000000',");
        sb.Append(@"formatter: function () {");
        sb.Append(@"return this.point.name + ': ' + Highcharts.numberFormat(this.percentage, 1) + '% (' + this.y + ') ';");
        sb.Append(@"}");

        sb.Append(@"}");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"series: [{");
        sb.Append(@"type: '" + chartname + "',");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: [");

        sb.Append(@"" + rowdata1 + "");

        sb.Append(@",{");
        sb.Append(@"y: 8,");
        sb.Append(@"sliced: true,");
        sb.Append(@"selected: true");
        sb.Append(@"},");


        sb.Append(@"]");
        sb.Append(@"}]");
        sb.Append(@"});");
        sb.Append(@"});");
        sb.Append(@"</script>");
        return sb.ToString();
    }


    public static string openGaugechart1(string divname, string chartname, string title, string subtitle, string col1, string rowdata1)
    {
        sb = new StringBuilder();

        sb.Append(@"<script type='text/javascript'>");


        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: 'gauge',");
        sb.Append(@"plotBackgroundColor: '#FFF',");
        sb.Append(@"plotBackgroundImage: null,");
        sb.Append(@"plotBorderWidth: 0,");
        sb.Append(@"plotShadow: true");
        sb.Append(@"},");

        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");
        sb.Append(@"subtitle: {");
        sb.Append(@"text: '" + subtitle + "'");
        sb.Append(@"},");

        sb.Append(@"pane: {");
        sb.Append(@"startAngle: -150,");
        sb.Append(@"endAngle: 150,");
        sb.Append(@"background: [{");
        sb.Append(@"backgroundColor: {");
        sb.Append(@"linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },");
        sb.Append(@"stops: [");
        sb.Append(@"[0, '#FFF'],");
        sb.Append(@"[1, '#333']");
        sb.Append(@"]");
        sb.Append(@"},");
        sb.Append(@"borderWidth: 10,");
        sb.Append(@"outerRadius: '109%'");
        sb.Append(@"}, {");
        sb.Append(@"backgroundColor: {");
        sb.Append(@"linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },");
        sb.Append(@"stops: [");
        sb.Append(@"[0, '#333'],");
        sb.Append(@"[1, '#FFF']");
        sb.Append(@"]");
        sb.Append(@"},");
        sb.Append(@"borderWidth: 1,");
        sb.Append(@"outerRadius: '107%'");
        sb.Append(@"}, {");
        // default background
        sb.Append(@"}, {");
        sb.Append(@"backgroundColor: '#DDD',");
        sb.Append(@"borderWidth: 0,");
        sb.Append(@"outerRadius: '105%',");
        sb.Append(@"innerRadius: '103%'");
        sb.Append(@"}]");
        sb.Append(@"},");

        // the value axis
        sb.Append(@"yAxis: {");
        sb.Append(@"min: 0,");
        sb.Append(@"max: 100,");

        sb.Append(@"minorTickInterval: 'auto',");
        sb.Append(@"minorTickWidth: 1,");
        sb.Append(@"minorTickLength: 10,");
        sb.Append(@"minorTickPosition: 'inside',");
        sb.Append(@"minorTickColor: '#666',");

        sb.Append(@"tickPixelInterval: 30,");
        sb.Append(@"tickWidth: 2,");
        sb.Append(@"tickPosition: 'inside',");
        sb.Append(@"tickLength: 10,");
        sb.Append(@"tickColor: '#666',");
        sb.Append(@"labels: {");
        sb.Append(@"step: 2,");
        sb.Append(@"rotation: 'auto'");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + col1 + "'");
        sb.Append(@"},");
        sb.Append(@"plotBands: [{");
        sb.Append(@"from: 75,");
        sb.Append(@"to: 100,");
        sb.Append(@"color: '#008000'");
        sb.Append(@"}, {");
        sb.Append(@"from: 25,");
        sb.Append(@"to: 75,");
        sb.Append(@"color: '#ffaa00'");
        sb.Append(@"}, {");
        sb.Append(@"from: 0,");
        sb.Append(@"to: 25,");
        sb.Append(@"color: '#ff3f00'");
        sb.Append(@"}]");
        sb.Append(@"},");

        sb.Append(@"series: [{");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: " + rowdata1 + ",");
        sb.Append(@"tooltip: {");
        sb.Append(@"valueSuffix: ' %'");
        sb.Append(@"}");
        sb.Append(@"}]");
        // Add some life
        sb.Append(@"});");
        sb.Append(@"});");
        sb.Append(@"</script>");
        return sb.ToString();
    }

    public static string openGaugechart(string divname, string chartname, string title, string col1, string rowdata1)
    {
        sb = new StringBuilder();

        sb.Append(@"<script type='text/javascript'>");


        sb.Append(@"$(function () {");
        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: '" + chartname + "',");
        sb.Append(@"alignTicks: false,");
        sb.Append(@"plotBackgroundColor: null,");
        sb.Append(@"plotBackgroundImage: null,");
        sb.Append(@"plotBorderWidth: 0,");
        sb.Append(@"plotShadow: false");
        sb.Append(@"},");

        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");

        sb.Append(@"pane: {");
        sb.Append(@"startAngle: -150,");
        sb.Append(@"endAngle: 150");
        sb.Append(@"},");

        sb.Append(@"yAxis: [{");
        sb.Append(@"min: 0,");
        sb.Append(@"max: 200,");
        sb.Append(@"lineColor: '#339',");
        sb.Append(@"tickColor: '#339',");
        sb.Append(@"minorTickColor: '#339',");
        sb.Append(@"offset: -25,");
        sb.Append(@"lineWidth: 2,");
        sb.Append(@"labels: {");
        sb.Append(@"distance: -20,");
        sb.Append(@"rotation: 'auto'");
        sb.Append(@" },");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + col1 + "'");
        sb.Append(@"},");
        sb.Append(@"tickLength: 5,");
        sb.Append(@"minorTickLength: 5,");
        sb.Append(@"endOnTick: false");
        sb.Append(@"}, {");
        sb.Append(@"min: 0,");
        sb.Append(@"max: 124,");
        sb.Append(@"tickPosition: 'outside',");
        sb.Append(@"lineColor: '#933',");
        sb.Append(@"lineWidth: 2,");
        sb.Append(@"minorTickPosition: 'outside',");
        sb.Append(@"tickColor: '#933',");
        sb.Append(@"minorTickColor: '#933',");
        sb.Append(@"tickLength: 5,");
        sb.Append(@"minorTickLength: 5,");
        sb.Append(@"labels: {");
        sb.Append(@"distance: 12,");
        sb.Append(@"rotation: 'auto'");
        sb.Append(@"},");
        sb.Append(@"offset: -20,");
        sb.Append(@"endOnTick: false");
        sb.Append(@"}],");

        sb.Append(@"series: [{");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: " + rowdata1 + ",");
        sb.Append(@"dataLabels: {");
        sb.Append(@"formatter: function () {");
        sb.Append(@"var kmh = this.y,");
        sb.Append(@"mph = Math.round(kmh * 0.621);");
        sb.Append(@"return '<span style= color: #339>'+ kmh + ' km/h</span><br/>' +");
        sb.Append(@"'<span style = color: #933 > ' + mph + ' mph</span>';");
        sb.Append(@"},");
        sb.Append(@"backgroundColor: {");
        sb.Append(@"linearGradient: {");
        sb.Append(@"x1: 0,");
        sb.Append(@"y1: 0,");
        sb.Append(@"x2: 0,");
        sb.Append(@"y2: 1");
        sb.Append(@"},");
        sb.Append(@"stops: [");
        sb.Append(@"[0, '#DDD'],");
        sb.Append(@"[1, '#FFF']");
        sb.Append(@"]");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");
        sb.Append(@"valueSuffix: ' km/h'");
        sb.Append(@"}");
        sb.Append(@"}]");

        sb.Append(@"},");

        sb.Append(@"function(chart) {");
        sb.Append(@"setInterval(function() {");
        sb.Append(@"var point = chart.series[0].points[0],");
        sb.Append(@"newVal, inc = Math.round((Math.random() - 0.5) * 20);");

        sb.Append(@"newVal = point.y + inc;");
        sb.Append(@"if (newVal < 0 || newVal > 200) {");
        sb.Append(@"newVal = point.y - inc;");
        sb.Append(@"}");

        sb.Append(@"point.update(newVal);");

        sb.Append(@"}, 3000);");

        sb.Append(@"});");
        sb.Append(@"});");

        sb.Append(@"</script>");
        return sb.ToString();
    }

    public static string openAreachart(string divname, string chartname, string title, string category, string vaxistext, string col1, string col2, string rowdata1, string rowdata2)
    {
        sb = new StringBuilder();

        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");

        sb.Append(@"$('#" + divname + "').highcharts({");
        sb.Append(@"chart: {");
        sb.Append(@"type: '" + chartname + "'");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + title + "'");
        sb.Append(@"},");
        sb.Append(@"legend: {");
        sb.Append(@"layout: 'vertical',");
        sb.Append(@"align: 'left',");
        sb.Append(@"verticalAlign: 'top',");
        sb.Append(@"x: 150,");
        sb.Append(@"y: 100,");
        sb.Append(@"floating: true,");
        sb.Append(@"borderWidth: 1,");
        sb.Append(@"backgroundColor: '#FFFFFF'");
        sb.Append(@"},");
        sb.Append(@"xAxis: {");
        sb.Append(@"categories: ");
        sb.Append(@"" + category + ",");

        sb.Append(@"plotBands: [{ ");
        sb.Append(@"from: 4.5,");
        sb.Append(@"to: 6.5,");
        sb.Append(@"color: 'rgba(68, 170, 213, .2)'");
        sb.Append(@"}]");
        sb.Append(@"},");
        sb.Append(@"yAxis: {");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + vaxistext + "'");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");
        sb.Append(@"shared: true,");
        sb.Append(@"valueSuffix: 'Amount'");
        sb.Append(@"},");
        sb.Append(@"credits: {");
        sb.Append(@"enabled: false");
        sb.Append(@"},");
        sb.Append(@"plotOptions: {");
        sb.Append(@"areaspline: {");
        sb.Append(@"fillOpacity: 0.5");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"series: [{");
        sb.Append(@"name: '" + col1 + "',");
        sb.Append(@"data: " + rowdata1 + "");

        ////
        //sb.Append(@"}, {");
        //sb.Append(@"name: '" + col2 + "',");
        //sb.Append(@"data: " + rowdata2 + "");
        ////

        sb.Append(@"}]");
        sb.Append(@"});");
        sb.Append(@"});");

        sb.Append(@"</script>");
        return sb.ToString();
    }

}
public class WrapprClass
{
    public static string DataTableToJSArray(DataTable dt, int index, string modeid)
    {
        StringBuilder sb = new StringBuilder();

        if (dt.Rows.Count > 0)
        {
            string colStr = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (colStr.Length > 0)
                    colStr += ",";
                if (dr[index].GetType() == typeof(Int32) || dr[index].GetType() == typeof(Double) || dr[index].GetType() == typeof(Decimal))
                    colStr += System.Math.Abs(Convert.ToDecimal(dr[index].ToString()));
                else
                {
                    if (modeid == "LBG2")
                        colStr += "'" + dr[index].ToString().Trim().Substring(5, 3) + "'";
                    else
                    {
                        try
                        {
                            Double temp = Convert.ToDouble(dr[index]);
                            colStr += temp.ToString();
                        }
                        catch (Exception ex)
                        {
                            colStr += "'" + dr[index].ToString().Trim() + "'";
                        }
                    }
                }
            }
            sb.Append("[" + colStr + "]");

        }

        return sb.ToString();
    }
}
