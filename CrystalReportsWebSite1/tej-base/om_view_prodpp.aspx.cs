using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_prodpp : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, DateRange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld, WB_TABNAME;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm, mdt, dticode, dticode2, dtdrsim, dtm1, dtraw;
    double month, to_cons, itot_stk, itv, d, iqtyout_sum; DataRow oporow, ROWICODE, ROWICODE2, dr2; DataView dv, view1im;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0, db9 = 0, db10 = 0, db11 = 0, db12 = 0, db13 = 0, db14 = 0, db15 = 0, db16 = 0, db17 = 0, db18 = 0, db19 = 0, db20 = 0, db21 = 0, db22 = 0, db23 = 0, db24 = 0, db25 = 0, db26 = 0, db27 = 0, db28 = 0, db29 = 0, db30 = 0;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1, r10 = "";
    int cnt = 0, cnt1 = 0;
    string frm_AssiID;
    string party_cd, part_cd;
    string frm_UserID, inspvchtab;
    fgenDB fgen = new fgenDB();
    DataSet dsRep;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["mymst"] != null)
        {
            if (Session["mymst"].ToString() == "Y")
                this.Page.MasterPageFile = "~/tej-base/myNewMaster.master";
            else this.Page.MasterPageFile = "~/tej-base/Fin_Master.master";
        }
    }
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
                cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
            }

            hfhcid.Value = frm_formID;

            if (!Page.IsPostBack)
            {
                col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT BRN||'~'||PRD AS PP FROM fin_msys WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
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
                case "F40128"://DOWN TIME CHECKLIST
                case "F40129"://REJECTION CHECKLIST
                case "89554":
                case "F60121":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "22610A":
                case "22610B":
                case "F40328":
                case "F40329":
                    //fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    fgen.msg("-", "CMSG", "Do Want to See All'13'(No for Only Stacked Material)");
                    break;

                case "P15005Y":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", HCID);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F15127":
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                ////MADE BY AKSHAY...MERGE BY YOGITA ON 2 APRIL 2018
                case "F35128"://Planning CHECKLIST
                case "F40351":// prod sheet process plan dump
                    SQuery = "";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F40350":
                    SQuery = "";
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;

                case "F40062":
                    SQuery = "select mthnum as fstr,mthnum ,mthname from mths";
                    break;

                case "F40063":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                #region ABOX Reports
                case "F40302":
                    SQuery = "select type1 as fstr ,Name as Name,type1 as Type,ADDR1 AS Store_type  from type where  id='Y' and substr(trim(type1),1,1)<='7' order by type1";
                    header_n = "Select Item Main Groups";
                    break;

                case "F40126":
                case "F40314":
                    SQuery = "select mthnum as fstr ,mthnum as Month_code,mthname as Month_Name from mths";
                    header_n = "Select Month";
                    break;

                case "F40311":
                    fgen.msg("-", "CMSG", "Want to See Current Orders ? Press No for All Orders.");
                    break;
                #endregion
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID == "F40302")
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
            if (val == "M03012" || val == "P15005B" || val == "P15005Z" || val == "F15127" || val == "F40302" || val == "F40311")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgen.Fn_open_prddmp1("-", frm_qstr);
            }
            else
            {
                switch (val)
                {
                    /////MADE BY AKSHAY...MERGED BY YOGITA ON 2 APRIL 2018                 
                    case "F40126":
                        if (co_cd == "SPIR" || co_cd == "STLC")
                        {
                            header_n = "31 Day Prodn Analysis ";
                            mq0 = value1;  //selected month value
                            if (Convert.ToInt32(mq0) > 3 && Convert.ToInt32(mq0) <= 12)
                            { }
                            else { year = (Convert.ToInt32(year) + 1).ToString(); }
                            mq1 = "";
                            mq1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where mthnum='" + mq0 + "'", "mthname");
                            SQuery = "SELECT icode as item_code,Item as Item_Name,Partno, sum(day_01+day_02+day_03+day_04+day_05+day_06+day_07+day_08+day_09+day_10+day_11+day_12+day_13+day_14+day_15+day_16+day_17+day_18+day_19+day_20+day_21+day_22+day_23+day_24+day_26+day_27+day_28+day_29+day_30+day_31) as total , sum(day_01) as day_01, sum(day_02) as day_02,sum(day_03) as day_03,sum(day_04) as day_04,sum(day_05) as day_05,sum(day_06) as day_06,sum(day_07) as day_07,sum(day_08) as day_08,sum(day_09) as day_09,sum(day_10) as day_10,sum(day_11) as day_11,sum(day_12) as day_12,sum(day_13) as day_13,sum(day_14) as day_14,sum(day_15) as day_15,sum(day_16) as day_16,sum(day_17) as day_17,sum(day_18) as day_18,sum(day_19) as day_19,sum(day_20) as day_20,sum(day_21) as day_21,sum(day_22) as day_22,sum(day_23) as day_23,sum(day_24) as day_24,sum(day_25) as day_25,sum(day_26) as day_26,sum(day_27) as day_27,sum(day_28) as day_28,sum(day_29) as day_29,sum(day_30) as day_30,sum(day_31) as day_31 from ( Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'dd'),'01',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_01, decode(to_chaR(vchdate,'dd'),'02',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_02, decode(to_chaR(vchdate,'dd'),'03',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_03, decode(to_chaR(vchdate,'dd'),'04',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_04, decode(to_chaR(vchdate,'dd'),'05',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_05, decode(to_chaR(vchdate,'dd'),'06',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_06, decode(to_chaR(vchdate,'dd'),'07',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_07, decode(to_chaR(vchdate,'dd'),'08',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_08, decode(to_chaR(vchdate,'dd'),'09',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_09, decode(to_chaR(vchdate,'dd'),'10',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_10, decode(to_chaR(vchdate,'dd'),'11',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_11, decode(to_chaR(vchdate,'dd'),'12',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_12, decode(to_chaR(vchdate,'dd'),'13',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_13, decode(to_chaR(vchdate,'dd'),'14',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_14, decode(to_chaR(vchdate,'dd'),'15',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_15, decode(to_chaR(vchdate,'dd'),'16',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_16, decode(to_chaR(vchdate,'dd'),'17',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_17, decode(to_chaR(vchdate,'dd'),'18',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_18, decode(to_chaR(vchdate,'dd'),'19',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_19, decode(to_chaR(vchdate,'dd'),'20',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_20, decode(to_chaR(vchdate,'dd'),'21',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_21, decode(to_chaR(vchdate,'dd'),'22',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_22, decode(to_chaR(vchdate,'dd'),'23',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_23, decode(to_chaR(vchdate,'dd'),'24',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_24, decode(to_chaR(vchdate,'dd'),'25',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_25, decode(to_chaR(vchdate,'dd'),'26',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_26, decode(to_chaR(vchdate,'dd'),'27',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_27, decode(to_chaR(vchdate,'dd'),'28',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_28,decode(to_chaR(vchdate,'dd'),'29',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_29,decode(to_chaR(vchdate,'dd'),'30',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_30, decode(to_chaR(vchdate,'dd'),'31',sum(a.iqtyin+nvl(a.rej_rw,'0')),0) as Day_31, a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "' and substr(a.type,1,2)='15' and TO_CHAR(a.vchdate,'MM/YYYY')='" + mq0 + "/" + year + "'   group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'dd')  ) group by item,partno,icode order by item";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Daily Production Checklist for Month  " + mq1 + "", frm_qstr);
                        }
                        else
                        {
                            header_n = "31 Day Prodn Analysis ";
                            mq0 = value1;  //selected month value
                            mq1 = "";
                            mq1 = fgen.seek_iname(frm_qstr, co_cd, "select mthname from mths where mthnum='" + mq0 + "'", "mthname");
                            //SQuery = "SELECT Item,Partno, sum(day_01+day_02+day_03+day_04+day_05+day_06+day_07+day_08+day_09+day_10+day_11+day_12+day_13+day_14+day_15+day_16+day_17+day_18+day_19+day_20+day_21+day_22+day_23+day_24+day_26+day_27+day_28+day_29+day_30+day_31) as total , sum(day_01) as day_01, sum(day_02) as day_02,sum(day_03) as day_03,sum(day_04) as day_04,sum(day_05) as day_05,sum(day_06) as day_06,sum(day_07) as day_07,sum(day_08) as day_08,sum(day_09) as day_09,sum(day_10) as day_10,sum(day_11) as day_11,sum(day_12) as day_12,sum(day_13) as day_13,sum(day_14) as day_14,sum(day_15) as day_15,sum(day_16) as day_16,sum(day_17) as day_17,sum(day_18) as day_18,sum(day_19) as day_19,sum(day_20) as day_20,sum(day_21) as day_21,sum(day_22) as day_22,sum(day_23) as day_23,sum(day_24) as day_24,sum(day_25) as day_25,sum(day_26) as day_26,sum(day_27) as day_27,sum(day_28) as day_28,sum(day_29) as day_29,sum(day_30) as day_30,sum(day_31) as day_31  , icode from ( Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'dd'),'01',sum(a.iqtyin),0) as Day_01, decode(to_chaR(vchdate,'dd'),'02',sum(a.iqtyin),0) as Day_02, decode(to_chaR(vchdate,'dd'),'03',sum(a.iqtyin),0) as Day_03, decode(to_chaR(vchdate,'dd'),'04',sum(a.iqtyin),0) as Day_04, decode(to_chaR(vchdate,'dd'),'05',sum(a.iqtyin),0) as Day_05, decode(to_chaR(vchdate,'dd'),'06',sum(a.iqtyin),0) as Day_06, decode(to_chaR(vchdate,'dd'),'07',sum(a.iqtyin),0) as Day_07, decode(to_chaR(vchdate,'dd'),'08',sum(a.iqtyin),0) as Day_08, decode(to_chaR(vchdate,'dd'),'09',sum(a.iqtyin),0) as Day_09, decode(to_chaR(vchdate,'dd'),'10',sum(a.iqtyin),0) as Day_10, decode(to_chaR(vchdate,'dd'),'11',sum(a.iqtyin),0) as Day_11, decode(to_chaR(vchdate,'dd'),'12',sum(a.iqtyin),0) as Day_12, decode(to_chaR(vchdate,'dd'),'13',sum(a.iqtyin),0) as Day_13, decode(to_chaR(vchdate,'dd'),'14',sum(a.iqtyin),0) as Day_14, decode(to_chaR(vchdate,'dd'),'15',sum(a.iqtyin),0) as Day_15, decode(to_chaR(vchdate,'dd'),'16',sum(a.iqtyin),0) as Day_16, decode(to_chaR(vchdate,'dd'),'17',sum(a.iqtyin),0) as Day_17, decode(to_chaR(vchdate,'dd'),'18',sum(a.iqtyin),0) as Day_18, decode(to_chaR(vchdate,'dd'),'19',sum(a.iqtyin),0) as Day_19, decode(to_chaR(vchdate,'dd'),'20',sum(a.iqtyin),0) as Day_20, decode(to_chaR(vchdate,'dd'),'21',sum(a.iqtyin),0) as Day_21, decode(to_chaR(vchdate,'dd'),'22',sum(a.iqtyin),0) as Day_22, decode(to_chaR(vchdate,'dd'),'23',sum(a.iqtyin),0) as Day_23, decode(to_chaR(vchdate,'dd'),'24',sum(a.iqtyin),0) as Day_24, decode(to_chaR(vchdate,'dd'),'25',sum(a.iqtyin),0) as Day_25, decode(to_chaR(vchdate,'dd'),'26',sum(a.iqtyin),0) as Day_26, decode(to_chaR(vchdate,'dd'),'27',sum(a.iqtyin),0) as Day_27,	 decode(to_chaR(vchdate,'dd'),'28',sum(a.iqtyin),0) as Day_28,decode(to_chaR(vchdate,'dd'),'29',sum(a.iqtyin),0) as Day_29,decode(to_chaR(vchdate,'dd'),'30',sum(a.iqtyin),0) as Day_30, decode(to_chaR(vchdate,'dd'),'31',sum(a.iqtyin),0) as Day_31, a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "' and substr(a.type,1,2)='15' and TO_CHAR(a.vchdate,'MM/YYYY')='" + mq0 + "/" + year + "'   group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'dd')  ) group by item,partno,icode order by item";
                            SQuery = "SELECT icode as item_code,Item as Item_Name,Partno, sum(day_01+day_02+day_03+day_04+day_05+day_06+day_07+day_08+day_09+day_10+day_11+day_12+day_13+day_14+day_15+day_16+day_17+day_18+day_19+day_20+day_21+day_22+day_23+day_24+day_26+day_27+day_28+day_29+day_30+day_31) as total , sum(day_01) as day_01, sum(day_02) as day_02,sum(day_03) as day_03,sum(day_04) as day_04,sum(day_05) as day_05,sum(day_06) as day_06,sum(day_07) as day_07,sum(day_08) as day_08,sum(day_09) as day_09,sum(day_10) as day_10,sum(day_11) as day_11,sum(day_12) as day_12,sum(day_13) as day_13,sum(day_14) as day_14,sum(day_15) as day_15,sum(day_16) as day_16,sum(day_17) as day_17,sum(day_18) as day_18,sum(day_19) as day_19,sum(day_20) as day_20,sum(day_21) as day_21,sum(day_22) as day_22,sum(day_23) as day_23,sum(day_24) as day_24,sum(day_25) as day_25,sum(day_26) as day_26,sum(day_27) as day_27,sum(day_28) as day_28,sum(day_29) as day_29,sum(day_30) as day_30,sum(day_31) as day_31 from ( Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'dd'),'01',sum(a.iqtyin),0) as Day_01, decode(to_chaR(vchdate,'dd'),'02',sum(a.iqtyin),0) as Day_02, decode(to_chaR(vchdate,'dd'),'03',sum(a.iqtyin),0) as Day_03, decode(to_chaR(vchdate,'dd'),'04',sum(a.iqtyin),0) as Day_04, decode(to_chaR(vchdate,'dd'),'05',sum(a.iqtyin),0) as Day_05, decode(to_chaR(vchdate,'dd'),'06',sum(a.iqtyin),0) as Day_06, decode(to_chaR(vchdate,'dd'),'07',sum(a.iqtyin),0) as Day_07, decode(to_chaR(vchdate,'dd'),'08',sum(a.iqtyin),0) as Day_08, decode(to_chaR(vchdate,'dd'),'09',sum(a.iqtyin),0) as Day_09, decode(to_chaR(vchdate,'dd'),'10',sum(a.iqtyin),0) as Day_10, decode(to_chaR(vchdate,'dd'),'11',sum(a.iqtyin),0) as Day_11, decode(to_chaR(vchdate,'dd'),'12',sum(a.iqtyin),0) as Day_12, decode(to_chaR(vchdate,'dd'),'13',sum(a.iqtyin),0) as Day_13, decode(to_chaR(vchdate,'dd'),'14',sum(a.iqtyin),0) as Day_14, decode(to_chaR(vchdate,'dd'),'15',sum(a.iqtyin),0) as Day_15, decode(to_chaR(vchdate,'dd'),'16',sum(a.iqtyin),0) as Day_16, decode(to_chaR(vchdate,'dd'),'17',sum(a.iqtyin),0) as Day_17, decode(to_chaR(vchdate,'dd'),'18',sum(a.iqtyin),0) as Day_18, decode(to_chaR(vchdate,'dd'),'19',sum(a.iqtyin),0) as Day_19, decode(to_chaR(vchdate,'dd'),'20',sum(a.iqtyin),0) as Day_20, decode(to_chaR(vchdate,'dd'),'21',sum(a.iqtyin),0) as Day_21, decode(to_chaR(vchdate,'dd'),'22',sum(a.iqtyin),0) as Day_22, decode(to_chaR(vchdate,'dd'),'23',sum(a.iqtyin),0) as Day_23, decode(to_chaR(vchdate,'dd'),'24',sum(a.iqtyin),0) as Day_24, decode(to_chaR(vchdate,'dd'),'25',sum(a.iqtyin),0) as Day_25, decode(to_chaR(vchdate,'dd'),'26',sum(a.iqtyin),0) as Day_26, decode(to_chaR(vchdate,'dd'),'27',sum(a.iqtyin),0) as Day_27, decode(to_chaR(vchdate,'dd'),'28',sum(a.iqtyin),0) as Day_28,decode(to_chaR(vchdate,'dd'),'29',sum(a.iqtyin),0) as Day_29,decode(to_chaR(vchdate,'dd'),'30',sum(a.iqtyin),0) as Day_30, decode(to_chaR(vchdate,'dd'),'31',sum(a.iqtyin),0) as Day_31, a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "' and substr(a.type,1,2)='15' and TO_CHAR(a.vchdate,'MM/YYYY')='" + mq0 + "/" + year + "'   group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'dd')  ) group by item,partno,icode order by item";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Daily Production Checklist for Month  " + mq1 + "", frm_qstr);
                        }
                        break;

                    case "15250I":// wfinsys_erp id // ABOX REPORT
                    case "F40310":
                        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                        //if (Request.Cookies["mq0"].Value == "Result")
                        if (hf1.Value == "")
                        {
                            hf1.Value = value1;
                            // fgen.send_cookie("mq0", "Result1");
                            SQuery = "SELECT trim(C.ICODE) AS FSTR,trim(C.ICODE) as Icode,C.INAME,sum(A.IQTYIN) AS QTYIN,sum(iamount) as Value FROM IVOUCHER A ,ITEM C  WHERE substr(TRIM(A.ICODE),1,4)=TRIM(C.ICODE) AND A.branchcd='" + mbr + "' AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + "  and trim(a.acode)='" + hf1.Value + "' group by A.ACODE,c.ICODE,C.INAME ,trim(C.ICODE) ORDER BY FSTR";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Party Wise Purchase between " + fromdt + " and " + todt + "", frm_qstr);
                        }
                        // else if (Request.Cookies["mq0"].Value == "Result1")
                        else
                        {
                            mq10 = "AND TRIM(A.ACODE)='" + hf1.Value + "' AND SUBSTR(TRIM(A.ICODE),1,4) IN (" + value1 + ")";
                            SQuery = "SELECT vchnum as MRR_No,to_char(vchdate,'dd/MM/yyyy') as MRR_DT,a.invno as Bill_No,to_char(a.invdate,'dd/MM/yyyy') as Bill_Dt,A.ACODE,B.ANAME,A.ICODE,C.INAME,sum(A.IQTYIN) AS QTYIN,sum(iamount) as Value FROM IVOUCHER A ,FAMST B,ITEM C WHERE TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.ACODE)=TRIM(B.ACODE) AND A.branchcd='" + mbr + "'  AND  A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + " " + mq10 + " group by A.ACODE,B.ANAME,A.ICODE,C.INAME,a.vchnum,to_char(vchdate,'dd/MM/yyyy'),invno,to_char(a.invdate,'dd/MM/yyyy') order by MRR_No";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("Party Wise Purchase between " + fromdt + " and " + todt + "", frm_qstr);
                        }
                        break;

                    case "15163E":// wfinsys_erp id // ABOX REPORT
                    case "F40314":
                        #region Downtime Reason Wise
                        yr_fld = year;
                        if (Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim()) < 4)
                        //  if (Convert.ToInt16(hfcode.Value) < 4)
                        {
                            //mq0 = hfcode.Value + (Convert.ToInt64(yr_fld) + 1).ToString();
                            mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim() + (Convert.ToInt64(yr_fld) + 1).ToString();
                        }
                        else
                        {
                            mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim() + yr_fld;
                        }

                        if (Convert.ToInt16(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim()) < 4)
                        {
                            mq1 = (Convert.ToInt64(yr_fld.Remove(0, 2)) + 1).ToString() + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim();
                        }
                        else
                        {
                            mq1 = yr_fld.Remove(0, 2) + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim();
                        }

                        SQuery = "SELECT A.MACHINE_NAME AS MACHINE,B.NAME AS REASON_NAME,SUM(A.M01)+SUM(A.M02)+SUM(A.M03)+SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(M13)+SUM(A.M14)+SUM(A.M15)+SUM(A.M16)+SUM(A.M17)+SUM(A.M18)+SUM(A.M19)+SUM(A.M20)+SUM(A.M21)+SUM(A.M22)+SUM(A.M23)+SUM(A.M24)+SUM(A.M25)+SUM(M26)+SUM(A.M27)+SUM(A.M28)+SUM(A.M29) +SUM(A.M30)+SUM(A.M31) AS TOTAL_TIME,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03,SUM(A.M04) AS M04,SUM(A.M05) AS M05,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(M13) AS M13,SUM(A.M14) AS M14,SUM(A.M15) AS M15,SUM(A.M16) AS M16,SUM(A.M17) AS M17,SUM(A.M18) AS M18,SUM(A.M19) AS M19,SUM(A.M20) AS M20,SUM(A.M21) AS M21,SUM(A.M22) AS M22,SUM(A.M23) AS M23,SUM(A.M24) AS M24,SUM(A.M25) AS M25,SUM(M26) AS M26,SUM(A.M27) AS M27,SUM(A.M28) AS M28,SUM(A.M29) AS M29,SUM(A.M30) AS M30,SUM(A.M31) AS M31 FROM (SELECT DISTINCT BRANCHCD,DECODE(TO_CHAR(VCHDATE,'DD'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'DD'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'DD'),'03', is_number(COL3),0) as M03,DECODE(TO_CHAR(VCHDATE,'DD'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'DD'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'DD'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'DD'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'DD'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'DD'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'DD'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'DD'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'DD'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'DD'),'13', is_number(COL3),0) as M13,DECODE(TO_CHAR(VCHDATE,'DD'),'14', is_number(COL3),0) as M14,DECODE(TO_CHAR(VCHDATE,'DD'),'15', is_number(COL3),0) as M15,DECODE(TO_CHAR(VCHDATE,'DD'),'16', is_number(COL3),0) as M16,DECODE(TO_CHAR(VCHDATE,'DD'),'17', is_number(COL3),0) as M17,DECODE(TO_CHAR(VCHDATE,'DD'),'18', is_number(COL3),0) as M18,DECODE(TO_CHAR(VCHDATE,'DD'),'19', is_number(COL3),0) as M19,DECODE(TO_CHAR(VCHDATE,'DD'),'20', is_number(COL3),0) as M20,DECODE(TO_CHAR(VCHDATE,'DD'),'21', is_number(COL3),0) as M21,DECODE(TO_CHAR(VCHDATE,'DD'),'22', is_number(COL3),0) as M22,DECODE(TO_CHAR(VCHDATE,'DD'),'23', is_number(COL3),0) as M23,DECODE(TO_CHAR(VCHDATE,'DD'),'24', is_number(COL3),0) as M24,DECODE(TO_CHAR(VCHDATE,'DD'),'25', is_number(COL3),0) as M25,DECODE(TO_CHAR(VCHDATE,'DD'),'26', is_number(COL3),0) as M26,DECODE(TO_CHAR(VCHDATE,'DD'),'27', is_number(COL3),0) as M27,DECODE(TO_CHAR(VCHDATE,'DD'),'28', is_number(COL3),0) as M28,DECODE(TO_CHAR(VCHDATE,'DD'),'29', is_number(COL3),0) as M29,DECODE(TO_CHAR(VCHDATE,'DD'),'30', is_number(COL3),0) as M30,DECODE(TO_CHAR(VCHDATE,'DD'),'31', is_number(COL3),0) as M31,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE " + branch_Cd + " AND TYPE='55'  and  TO_CHAR(VCHDATE,'MMYYYY')='" + mq0 + "') A,TYPEWIP B WHERE B.id='DTC61' and b.branchcd='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1)  GROUP BY A.MACHINE_NAME,B.NAME";
                        SQuery = "SELECT '-' AS MACHINE,'-' AS REASON_NAME,SUM(A.M01)+SUM(A.M02)+SUM(A.M03)+SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(M13)+SUM(A.M14)+SUM(A.M15)+SUM(A.M16)+SUM(A.M17)+SUM(A.M18)+SUM(A.M19)+SUM(A.M20)+SUM(A.M21)+SUM(A.M22)+SUM(A.M23)+SUM(A.M24)+SUM(A.M25)+SUM(M26)+SUM(A.M27)+SUM(A.M28)+SUM(A.M29) +SUM(A.M30)+SUM(A.M31) AS TOTAL_TIME,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03,SUM(A.M04) AS M04,SUM(A.M05) AS M05,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(M13) AS M13,SUM(A.M14) AS M14,SUM(A.M15) AS M15,SUM(A.M16) AS M16,SUM(A.M17) AS M17,SUM(A.M18) AS M18,SUM(A.M19) AS M19,SUM(A.M20) AS M20,SUM(A.M21) AS M21,SUM(A.M22) AS M22,SUM(A.M23) AS M23,SUM(A.M24) AS M24,SUM(A.M25) AS M25,SUM(M26) AS M26,SUM(A.M27) AS M27,SUM(A.M28) AS M28,SUM(A.M29) AS M29,SUM(A.M30) AS M30,SUM(A.M31) AS M31 FROM (SELECT  BRANCHCD,DECODE(TO_CHAR(VCHDATE,'DD'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'DD'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'DD'),'03', is_number(COL3),0) as M03,DECODE(TO_CHAR(VCHDATE,'DD'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'DD'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'DD'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'DD'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'DD'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'DD'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'DD'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'DD'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'DD'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'DD'),'13', is_number(COL3),0) as M13,DECODE(TO_CHAR(VCHDATE,'DD'),'14', is_number(COL3),0) as M14,DECODE(TO_CHAR(VCHDATE,'DD'),'15', is_number(COL3),0) as M15,DECODE(TO_CHAR(VCHDATE,'DD'),'16', is_number(COL3),0) as M16,DECODE(TO_CHAR(VCHDATE,'DD'),'17', is_number(COL3),0) as M17,DECODE(TO_CHAR(VCHDATE,'DD'),'18', is_number(COL3),0) as M18,DECODE(TO_CHAR(VCHDATE,'DD'),'19', is_number(COL3),0) as M19,DECODE(TO_CHAR(VCHDATE,'DD'),'20', is_number(COL3),0) as M20,DECODE(TO_CHAR(VCHDATE,'DD'),'21', is_number(COL3),0) as M21,DECODE(TO_CHAR(VCHDATE,'DD'),'22', is_number(COL3),0) as M22,DECODE(TO_CHAR(VCHDATE,'DD'),'23', is_number(COL3),0) as M23,DECODE(TO_CHAR(VCHDATE,'DD'),'24', is_number(COL3),0) as M24,DECODE(TO_CHAR(VCHDATE,'DD'),'25', is_number(COL3),0) as M25,DECODE(TO_CHAR(VCHDATE,'DD'),'26', is_number(COL3),0) as M26,DECODE(TO_CHAR(VCHDATE,'DD'),'27', is_number(COL3),0) as M27,DECODE(TO_CHAR(VCHDATE,'DD'),'28', is_number(COL3),0) as M28,DECODE(TO_CHAR(VCHDATE,'DD'),'29', is_number(COL3),0) as M29,DECODE(TO_CHAR(VCHDATE,'DD'),'30', is_number(COL3),0) as M30,DECODE(TO_CHAR(VCHDATE,'DD'),'31', is_number(COL3),0) as M31,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE branchcd='" + mbr + "' AND TYPE='55' and TO_CHAR(VCHDATE,'MMYYYY')='" + mq0 + "') A,TYPEWIP B WHERE B.id='DTC61' and b.branchcd='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1) union all SELECT A.MACHINE_NAME AS MACHINE,B.NAME AS REASON_NAME,SUM(A.M01)+SUM(A.M02)+SUM(A.M03)+SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(M13)+SUM(A.M14)+SUM(A.M15)+SUM(A.M16)+SUM(A.M17)+SUM(A.M18)+SUM(A.M19)+SUM(A.M20)+SUM(A.M21)+SUM(A.M22)+SUM(A.M23)+SUM(A.M24)+SUM(A.M25)+SUM(M26)+SUM(A.M27)+SUM(A.M28)+SUM(A.M29) +SUM(A.M30)+SUM(A.M31) AS TOTAL_TIME,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03,SUM(A.M04) AS M04,SUM(A.M05) AS M05,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(M13) AS M13,SUM(A.M14) AS M14,SUM(A.M15) AS M15,SUM(A.M16) AS M16,SUM(A.M17) AS M17,SUM(A.M18) AS M18,SUM(A.M19) AS M19,SUM(A.M20) AS M20,SUM(A.M21) AS M21,SUM(A.M22) AS M22,SUM(A.M23) AS M23,SUM(A.M24) AS M24,SUM(A.M25) AS M25,SUM(M26) AS M26,SUM(A.M27) AS M27,SUM(A.M28) AS M28,SUM(A.M29) AS M29,SUM(A.M30) AS M30,SUM(A.M31) AS M31 FROM (SELECT  BRANCHCD,DECODE(TO_CHAR(VCHDATE,'DD'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'DD'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'DD'),'03', is_number(COL3),0) as M03,DECODE(TO_CHAR(VCHDATE,'DD'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'DD'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'DD'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'DD'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'DD'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'DD'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'DD'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'DD'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'DD'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'DD'),'13', is_number(COL3),0) as M13,DECODE(TO_CHAR(VCHDATE,'DD'),'14', is_number(COL3),0) as M14,DECODE(TO_CHAR(VCHDATE,'DD'),'15', is_number(COL3),0) as M15,DECODE(TO_CHAR(VCHDATE,'DD'),'16', is_number(COL3),0) as M16,DECODE(TO_CHAR(VCHDATE,'DD'),'17', is_number(COL3),0) as M17,DECODE(TO_CHAR(VCHDATE,'DD'),'18', is_number(COL3),0) as M18,DECODE(TO_CHAR(VCHDATE,'DD'),'19', is_number(COL3),0) as M19,DECODE(TO_CHAR(VCHDATE,'DD'),'20', is_number(COL3),0) as M20,DECODE(TO_CHAR(VCHDATE,'DD'),'21', is_number(COL3),0) as M21,DECODE(TO_CHAR(VCHDATE,'DD'),'22', is_number(COL3),0) as M22,DECODE(TO_CHAR(VCHDATE,'DD'),'23', is_number(COL3),0) as M23,DECODE(TO_CHAR(VCHDATE,'DD'),'24', is_number(COL3),0) as M24,DECODE(TO_CHAR(VCHDATE,'DD'),'25', is_number(COL3),0) as M25,DECODE(TO_CHAR(VCHDATE,'DD'),'26', is_number(COL3),0) as M26,DECODE(TO_CHAR(VCHDATE,'DD'),'27', is_number(COL3),0) as M27,DECODE(TO_CHAR(VCHDATE,'DD'),'28', is_number(COL3),0) as M28,DECODE(TO_CHAR(VCHDATE,'DD'),'29', is_number(COL3),0) as M29,DECODE(TO_CHAR(VCHDATE,'DD'),'30', is_number(COL3),0) as M30,DECODE(TO_CHAR(VCHDATE,'DD'),'31', is_number(COL3),0) as M31,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE branchcd='" + mbr + "' AND TYPE='55' and TO_CHAR(VCHDATE,'MMYYYY')='" + mq0 + "') A,TYPEWIP B WHERE B.id='DTC61' and b.branchcd='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1)  GROUP BY A.MACHINE_NAME,B.NAME";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                        foreach (DataColumn dc in dt.Columns)
                        {
                            int abc = dc.Ordinal;
                            if (abc == 0 || abc == 1 || abc == 2) { }
                            else
                            {
                                string myname = dt.Columns[abc].ColumnName.Remove(0, 1);
                                if (myname != "0")
                                {
                                    dt.Columns[abc].ColumnName = myname;
                                }
                            }
                        }

                        Session["send_dt"] = dt;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Downtime Reason wise For the Month of " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "" + year, frm_qstr);
                        //fgen.open_rptlevel_hd("Downtime Reason wise for the month of " + fgen.return_Month(mq1) + "");
                        #endregion
                        break;


                    case "F40062":
                        #region
                        dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dtm = new DataTable();
                        //=====================
                        dtm.Columns.Add("Dated", typeof(string));
                        //for shift A
                        dtm.Columns.Add("MC_01_A_InPcs", typeof(double));//HERE A FOR SHIFT A....cnt
                        dtm.Columns.Add("MC_01_A_Amt", typeof(double));//cnt1
                        dtm.Columns.Add("MC_02_A_InPcs", typeof(double));//cnt2                    
                        dtm.Columns.Add("MC_02_A_Amt", typeof(double));//cnt3
                        dtm.Columns.Add("MC_03_A_InPcs", typeof(double));//cnt4
                        dtm.Columns.Add("MC_03_A_Amt", typeof(double));//cnt5
                        dtm.Columns.Add("MC_04_A_InPcs", typeof(double));//cnt6
                        dtm.Columns.Add("MC_04_A_Amt", typeof(double));//cnt7
                        //for shift B
                        dtm.Columns.Add("MC_01_B_InPcs", typeof(double)); //cnt8
                        dtm.Columns.Add("MC_01_B_Amt", typeof(double));//cnt9
                        dtm.Columns.Add("MC_02_B_InPcs", typeof(double));//cnt10
                        dtm.Columns.Add("MC_02_B_Amt", typeof(double));//cnt11
                        dtm.Columns.Add("MC_03_B_InPcs", typeof(double));//cnt12
                        dtm.Columns.Add("MC_03_B_Amt", typeof(double));//cnt13
                        dtm.Columns.Add("MC_04_B_InPcs", typeof(double));//cnt14
                        dtm.Columns.Add("MC_04_B_Amt", typeof(double));//cnt15
                        ///LAST COLUMNS
                        dtm.Columns.Add("Total_Shift_A_In_Pcs", typeof(double));
                        dtm.Columns.Add("Total_Shift_A_Amt", typeof(double));
                        dtm.Columns.Add("Total_Shift_A_Percentage", typeof(double));
                        dtm.Columns.Add("Total_Shift_B_In_Pcs", typeof(double));
                        dtm.Columns.Add("Total_Shift_B_Amt", typeof(double));
                        dtm.Columns.Add("Total_Shift_B_Percentage", typeof(double));
                        dtm.Columns.Add("A_B_Total_InPcs", typeof(double));
                        dtm.Columns.Add("A_B_Total_Amt", typeof(double));
                        dtm.Columns.Add("EFF_Total_A_B", typeof(double));

                        int Myear;
                        if (Convert.ToInt32(value1) < 3)
                        {
                            Myear = DateTime.Today.Year + 1;
                        }
                        else
                        {
                            Myear = Convert.ToInt32(year);
                        }
                        dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable();
                        SQuery = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT FROM PROD_SHEET A,TYPE c  WHERE TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND to_char(a.vchdate,'mm/yyyy')='" + value1 + "/" + Myear + "' AND A.MCHCODE IN ('1A/1','1A/2','1A/3','1A/4')  AND A.SHFTCODE IN ('11','12','13','18') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD,MCHCODE";
                        dt = fgen.getdata(frm_qstr, co_cd, SQuery);//THIS DT FOR ALLL SHIFT DATA...MAIN DT FOR LOOP

                        mq0 = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND to_char(a.vchdate,'mm/yyyy')='" + value1 + "/" + Myear + "' AND A.MCHCODE IN ('1A/1','1A/2','1A/3','1A/4')  AND A.SHFTCODE IN ('11','12') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,b.iname,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD,MCHCODE";
                        dt1 = fgen.getdata(frm_qstr, co_cd, mq0);//MAIN DT FROM JOB WISE PRODUCTION...........FOR SHIFT A ONLY

                        mq1 = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND to_char(a.vchdate,'mm/yyyy')='" + value1 + "/" + Myear + "' AND A.MCHCODE IN ('1A/1','1A/2','1A/3','1A/4')  AND A.SHFTCODE IN ('13','18') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,b.iname,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD,MCHCODE";
                        dt2 = fgen.getdata(frm_qstr, co_cd, mq1);//MAIN DT FROM JOB WISE PRODUCTION...........FOR SHIFT B ONLY

                        mq0 = "SELECT distinct  BRANCHCD,VCHNUM AS JOB_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS JOB_dT,substr(convdate,1,20) as so_Detail   FROM COSTESTIMATE WHERE branchcd='" + mbr + "' and TYPE='30'";
                        dt5 = fgen.getdata(frm_qstr, co_cd, mq0); //jobcard dt

                        mq1 = "SELECT trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as sodetails,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,sum(qtyord) as so_qty,irate FROM SOMAS WHERE  type like '4%'  group by ordno,to_char(orddt,'dd/mm/yyyy'),trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy'),irate order by sodetails"; //BRanchcd='" + mbr + "' and and orddt " + DateRange + "
                        dt4 = fgen.getdata(frm_qstr, co_cd, mq1);//sale order kabi b ban skte...isley no need of daterange
                        header_n = "LASUR CUT MACHINE EFFICIENCY FOR MONTH - " + value1 + "/" + Myear + "";

                        if (dt.Rows.Count > 0)
                        {
                            view1im = new DataView(dt);
                            dtdrsim = view1im.ToTable(true, "vchdate"); //MAIN                        
                            foreach (DataRow dr0 in dtdrsim.Rows)
                            {
                                #region
                                DataView viewim = new DataView(dt, "vchdate='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt3 = viewim.ToTable();
                                db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0;
                                cnt = 0; cnt1 = 0; double db22 = 0, db23 = 0; ;
                                dr2 = dtm.NewRow();
                                for (int i = 0; i < dt3.Rows.Count; i++)
                                {
                                    mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; db1 = 0;
                                    db = 3500;
                                    mq2 = dt3.Rows[i]["SHFTCODE"].ToString().Trim();
                                    mq3 = dt3.Rows[i]["mchcode"].ToString().Trim();
                                    mq4 = fgen.seek_iname_dt(dt5, "job_no='" + dt3.Rows[i]["JOB_NO"].ToString().Trim() + "' and job_dt='" + dt3.Rows[i]["JOB_dT"].ToString().Trim() + "'", "so_Detail");
                                    mq5 = fgen.seek_iname_dt(dt4, "sodetails='" + mq4 + "'", "ordno");
                                    db1 = fgen.make_double(fgen.seek_iname_dt(dt4, "sodetails='" + mq4 + "'", "irate"));
                                    dr2["Dated"] = dt3.Rows[i]["vchdate"].ToString().Trim();
                                    switch (mq2)
                                    {
                                        #region for shift a
                                        case "11":
                                        case "12":
                                            if (mq3 == "1A/1")
                                            {
                                                db6 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_01_A_InPcs"] = db6; //for date wise sum in one row
                                                db2 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//for total pcs shift a
                                                db7 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_01_A_Amt"] = db7;//for date wise sum in one row
                                                db22 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1; //total amt for shift a
                                                db3 += db22;
                                                cnt = 1;
                                            }
                                            else if (mq3 == "1A/2")
                                            {
                                                db8 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_02_A_InPcs"] = db8;
                                                db2 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//for total pcs shift a
                                                db9 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_02_A_Amt"] = db9;
                                                db22 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1; //total amt for shift a
                                                db3 += db22;
                                            }
                                            else if (mq3 == "1A/3")
                                            {
                                                db10 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_03_A_InPcs"] = db10;
                                                db2 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//for total pcs shift a
                                                db11 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_03_A_Amt"] = db11;
                                                db22 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1; //total amt for shift a
                                                db3 += db22;
                                            }
                                            else if (mq3 == "1A/4")
                                            {
                                                db12 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_04_A_InPcs"] = db12;
                                                db2 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//for total pcs shift a
                                                db13 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_04_A_Amt"] = db13;
                                                db22 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1; //total amt for shift a
                                                db3 += db22;
                                            }
                                        #endregion
                                            break;
                                        #region for shift b
                                        case "13":
                                        case "18":
                                            if (mq3 == "1A/1")
                                            {
                                                db14 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_01_B_InPcs"] = db14;
                                                db4 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//total pcs for shift b
                                                db15 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_01_B_Amt"] = db15;
                                                db23 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1; //total amt for shift b
                                                db5 += db23;
                                            }
                                            else if (mq3 == "1A/2")
                                            {
                                                db16 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_02_B_InPcs"] = db16;
                                                db4 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//total pcs for shift b
                                                db17 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_02_B_Amt"] = db17;
                                                db23 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;//total amt for shift b
                                                db5 += db23;
                                            }
                                            else if (mq3 == "1A/3")
                                            {
                                                db18 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_03_B_InPcs"] = db18;
                                                db4 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//total pcs for shift b
                                                db19 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_03_B_Amt"] = db19;
                                                db23 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;//total amt for shift b
                                                db5 += db23;
                                            }
                                            else if (mq3 == "1A/4")
                                            {
                                                db20 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());
                                                dr2["MC_04_B_InPcs"] = db20;
                                                db4 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim());//total pcs for shift b
                                                db21 += fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1;
                                                dr2["MC_04_B_Amt"] = db21;
                                                db23 = fgen.make_double(dt3.Rows[i]["ok_qty"].ToString().Trim()) * db1; //total amt for shift b
                                                db5 += db23;
                                            }
                                        #endregion
                                            break;
                                    }
                                }
                                dr2["Total_Shift_A_In_Pcs"] = db2;
                                dr2["Total_Shift_A_Amt"] = db3;
                                /////========================
                                dr2["Total_Shift_B_In_Pcs"] = db4;
                                dr2["Total_Shift_B_Amt"] = db5;
                                dr2["A_B_Total_InPcs"] = db2 + db4;
                                dr2["A_B_Total_Amt"] = db3 + db5;
                                dtm.Rows.Add(dr2);

                                #region////   for count no of machine working per day (row wise count).......for shift A
                                if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_01_A_InPcs"].ToString().Trim()) > 0 || fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_02_A_InPcs"].ToString().Trim()) > 0 || fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_03_A_InPcs"].ToString().Trim()) > 0 || fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_04_A_InPcs"].ToString().Trim()) > 0)
                                {
                                    for (int k = dtm.Rows.Count - 1; k < dtm.Rows.Count; k++)
                                    {
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_01_A_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_02_A_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_03_A_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_04_A_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                    }
                                    db = cnt * 3500;
                                    cnt1 = cnt;
                                    dtm.Rows[dtm.Rows.Count - 1]["Total_Shift_A_Percentage"] = Math.Round(db3 / db * 100, 3);
                                }
                                #endregion

                                #region    for count no of machine working per day (row wise count).......for shift B
                                db = 0; cnt = 0;
                                if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_01_B_InPcs"].ToString().Trim()) > 0 || fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_02_B_InPcs"].ToString().Trim()) > 0 || fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_03_B_InPcs"].ToString().Trim()) > 0 || fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_04_B_InPcs"].ToString().Trim()) > 0)
                                {
                                    for (int k = dtm.Rows.Count - 1; k < dtm.Rows.Count; k++)
                                    {
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_01_B_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_02_B_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_03_B_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                        if (fgen.make_int(dtm.Rows[dtm.Rows.Count - 1]["MC_04_B_InPcs"].ToString().Trim()) > 0)
                                        {
                                            cnt = cnt + 1;
                                        }
                                    }
                                    db = cnt * 3500;
                                    dtm.Rows[dtm.Rows.Count - 1]["Total_Shift_B_Percentage"] = Math.Round(db5 / db * 100, 3);
                                }
                                #endregion
                                db = 0;
                                cnt1 += cnt; //TOTAL MACHINE IN SINGLE ROW
                                db = cnt1 * 3500;
                                dtm.Rows[dtm.Rows.Count - 1]["EFF_Total_A_B"] = Math.Round((db3 + db5) / db * 100, 3);//Math.Round((db3 + db5) / db * 100, 3);
                                #endregion
                            }
                        }
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0;

                        if (dtm.Rows.Count > 0)
                        {
                            oporow = null;
                            oporow = dtm.NewRow();
                            foreach (DataColumn dc in dtm.Columns)
                            {
                                to_cons = 0; db = 0;
                                if (dc.Ordinal == 0)
                                {
                                }
                                else
                                {
                                    mq1 = "sum(" + dc.ColumnName + ")";
                                    mq2 = dc.ColumnName;
                                    mq3 = "count(" + dc.ColumnName + ")"; //for  count no of days 
                                    to_cons += fgen.make_double(dtm.Compute(mq1, "").ToString());//for sum
                                    oporow[dc] = Math.Round(to_cons, 3);
                                    //===================
                                    db += fgen.make_double(dtm.Compute(mq3, "").ToString());//for count
                                    if (mq2 == "MC_01_A_InPcs")
                                    {
                                        db1 = db;
                                    }
                                    if (mq2 == "MC_01_A_Amt")
                                    {
                                        db2 = db;
                                    }
                                    if (mq2 == "MC_02_A_InPcs")
                                    {
                                        db3 = db;
                                    }
                                    if (mq2 == "MC_02_A_Amt")
                                    {
                                        db4 = db;
                                    }
                                    if (mq2 == "MC_03_A_InPcs")
                                    {
                                        db5 = db;
                                    }
                                    if (mq2 == "MC_03_A_Amt")
                                    {
                                        db6 = db;
                                    }
                                    if (mq2 == "MC_04_A_InPcs")
                                    {
                                        db7 = db;
                                    }
                                    if (mq2 == "MC_04_A_Amt")
                                    {
                                        db8 = db;
                                    }
                                    //FOR SHIFT B
                                    if (mq2 == "MC_01_B_InPcs")
                                    {
                                        db9 = db;
                                    }
                                    if (mq2 == "MC_01_B_Amt")
                                    {
                                        db10 = db;
                                    }
                                    if (mq2 == "MC_02_B_InPcs")
                                    {
                                        db11 = db;
                                    }
                                    if (mq2 == "MC_02_B_Amt")
                                    {
                                        db12 = db;
                                    }
                                    if (mq2 == "MC_03_B_InPcs")
                                    {
                                        db13 = db;
                                    }
                                    if (mq2 == "MC_03_B_Amt")
                                    {
                                        db14 = db;
                                    }
                                    if (mq2 == "MC_04_B_InPcs")
                                    {
                                        db15 = db;
                                    }
                                    if (mq2 == "MC_04_B_Amt")
                                    {
                                        db16 = db;
                                    }
                                }
                            }
                            oporow["Dated"] = "Total";
                            dtm.Rows.Add(oporow);
                        }
                        for (int k = 0; k < 1; k++)
                        {
                            db = 0;
                            dr2 = dtm.NewRow();
                            dr2["Dated"] = "AVG/TILLDATE";
                            if (db1 > 0)
                            {
                                dr2["MC_01_A_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_01_A_InPcs"].ToString().Trim()) / db1, 3);
                            }
                            if (db2 > 0)
                            {
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_01_A_Amt"].ToString().Trim()) / db2;
                                db17 = Math.Round(db / 3500 * 100, 3);//percentage formula
                                dr2["MC_01_A_Amt"] = Math.Round(db, 3);
                            }
                            if (db3 > 0)
                            {
                                dr2["MC_02_A_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_02_A_InPcs"].ToString().Trim()) / db3, 3);
                            }
                            if (db4 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_02_A_Amt"].ToString().Trim()) / db4;
                                db18 = Math.Round(db / 3500 * 100, 3);//percentage formula
                                dr2["MC_02_A_Amt"] = Math.Round(db, 3);
                            }
                            if (db5 > 0)
                            {
                                dr2["MC_03_A_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_03_A_InPcs"].ToString().Trim()) / db5, 3);
                            }
                            if (db6 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_03_A_Amt"].ToString().Trim()) / db6;
                                db19 = Math.Round(db / 3500 * 100, 3);//percentage formula
                                dr2["MC_03_A_Amt"] = Math.Round(db, 3);
                            }
                            if (db7 > 0)
                            {
                                dr2["MC_04_A_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_04_A_InPcs"].ToString().Trim()) / db7, 3);
                            }
                            if (db8 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_04_A_Amt"].ToString().Trim()) / db8;
                                db20 = Math.Round(db / 3500 * 100, 3);//percentage formula
                                dr2["MC_04_A_Amt"] = Math.Round(db, 2);
                            }
                            ///shift b
                            if (db9 > 0)
                            {
                                dr2["MC_01_B_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_01_B_InPcs"].ToString().Trim()) / db9, 3);
                            }
                            if (db10 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_01_B_Amt"].ToString().Trim()) / db10;
                                db21 = db / 3500 * 100;//percentage formula
                                dr2["MC_01_B_Amt"] = Math.Round(db, 3);
                            }
                            if (db11 > 0)
                            {
                                dr2["MC_02_B_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_02_B_InPcs"].ToString().Trim()) / db11, 3);
                            }
                            if (db12 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_02_B_Amt"].ToString().Trim()) / db12;
                                db22 = db / 3500 * 100;//percentage formula
                                dr2["MC_02_B_Amt"] = Math.Round(db, 3);
                            }
                            if (db13 > 0)
                            {
                                dr2["MC_03_B_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_03_B_InPcs"].ToString().Trim()) / db13, 3);
                            }
                            if (db14 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_03_B_Amt"].ToString().Trim()) / db14;
                                db23 = Math.Round(db / 3500 * 100, 3);//percentage formula
                                dtm.Rows[dtm.Rows.Count - 1]["MC_03_B_Amt"] = Math.Round(db23, 3);
                            }
                            if (db15 > 0)
                            {
                                dr2["MC_04_B_InPcs"] = Math.Round(fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_04_B_InPcs"].ToString().Trim()) / db15, 3);
                            }
                            if (db16 > 0)
                            {
                                db = 0;
                                db = fgen.make_double(dtm.Rows[dtm.Rows.Count - 1]["MC_04_B_Amt"].ToString().Trim()) / db16;
                                db24 = Math.Round(db / 3500 * 100, 3);//percentage formula
                                dr2["MC_04_B_Amt"] = Math.Round(db, 3);
                            }
                            dtm.Rows.Add(dr2);
                        }
                        //============= LOGIC FOR AVG/TILL DATE
                        dr2 = dtm.NewRow();
                        dr2["dated"] = "Per Day Target";
                        dr2["MC_01_A_Amt"] = "3500";
                        dr2["MC_02_A_Amt"] = "3500";
                        dr2["MC_03_A_Amt"] = "3500";
                        dr2["MC_04_A_Amt"] = "3500";
                        dr2["MC_01_B_Amt"] = "3500";
                        dr2["MC_02_B_Amt"] = "3500";
                        dr2["MC_03_B_Amt"] = "3500";
                        dr2["MC_04_B_Amt"] = "3500";
                        dtm.Rows.Add(dr2);
                        ///================
                        dr2 = dtm.NewRow();
                        dr2["dated"] = "Percentage";
                        dr2["MC_01_A_Amt"] = db17;
                        dr2["MC_02_A_Amt"] = db18;
                        dr2["MC_03_A_Amt"] = db19;
                        dr2["MC_04_A_Amt"] = db20;
                        dr2["MC_01_B_Amt"] = db21;
                        dr2["MC_02_B_Amt"] = db22;
                        dr2["MC_03_B_Amt"] = db23;
                        dr2["MC_04_B_Amt"] = db24;
                        dtm.Rows.Add(dr2);
                        ///=======
                        if (dtm.Rows.Count > 0)
                        {
                            Session["send_dt"] = dtm;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("" + header_n + "", frm_qstr);
                        }

                        #endregion
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

                case "F40311": // ABOX REPORT
                    // if (Request.Cookies["mq0"].Value.ToString().Trim() != "OK")
                    // if (Request.Cookies["REPLY"].Value.ToString().Trim() != "OK")
                    if (hf1.Value == "")
                    {
                        hf1.Value = value1;
                        SQuery = "select distinct type1 AS FSTR, type1 AS type ,name from type  where id='V' AND TYPE1 LIKE '4%' ORDER BY TYPE1";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Type", frm_qstr);
                    }
                    else
                    {
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                    }
                    break;

                case "F40328": // ABOX REPORT
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value1);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "22610A":// wfinsys_erp id // ABOX REPORT
                case "F40329":
                    #region Location Wise Reel Number Wise Stock / Reel Number Wise Location Wise Stock
                    mq0 = value1;
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    //      mq0 = hfcode.Value;
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R40'", "params");
                    if (m1 == "0") m1 = cDT1;
                    xprdrange = "between to_Date('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    mq1 = "Location Wise Reel Number Wise Stock";
                    if (co_cd == "SVPL")
                        mq1 = "Batch Wise Stock";
                    SQuery = "select a.icode as erpcode,d.iname as product,d.cpartno as part_no,a.kclreelno AS our_reel_no,replace(nvl(a.RLOCN,'-'),'-','-') as location,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance from (select branchcd,icode,kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout,max(rlocn) as rlocn from (select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " union all select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch_op where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " ) group by branchcd,icode,kclreelno  having sum(reelwin)-sum(reelwout)>0) a,item d where trim(a.icode)=trim(d.icodE) order by erpcode";
                    if (mq0 == "N")
                        SQuery = "select a.icode as erpcode,d.iname as product,d.cpartno as part_no,a.kclreelno AS our_reel_no,replace(nvl(a.RLOCN,'-'),'-','-') as location,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance from (select branchcd,icode,kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout,max(rlocn) as rlocn from (select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " union all select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch_op where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " ) group by branchcd,icode,kclreelno  having sum(reelwin)-sum(reelwout)>0) a,item d where trim(a.icode)=trim(d.icodE) and nvl(a.RLOCN,'-')!='-' order by erpcode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;
                //------------------------till here
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

            tbl_flds = fgen.seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + val + "' and srno=0", "fstr");
            if (tbl_flds.Trim().Length > 1)
            {
                datefld = tbl_flds.Split('@')[0].ToString();
                sortfld = tbl_flds.Split('@')[1].ToString();
                joinfld = tbl_flds.Split('@')[2].ToString();

                table1 = tbl_flds.Split('@')[3].ToString();
                table2 = tbl_flds.Split('@')[4].ToString();
                table3 = tbl_flds.Split('@')[5].ToString();
                table4 = tbl_flds.Split('@')[6].ToString();

                sortfld = sortfld.Replace("`", "'");
                joinfld = joinfld.Replace("`", "'");
                rep_flds = fgen.seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + val + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
                rep_flds = rep_flds.Replace("`", "'");
            }

            // after prdDmp this will run   

            switch (val)
            {
                case "F20121":
                    // Gate Inward Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='00'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate Inward Checklist For the Period " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F20126":
                    // Gate Outward Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='2G'", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate Outward Checklist For the Period " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F20127":
                    // Gate PO Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate PO Checklist For the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F20128":
                    // Gate RGP Checklist
                    SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "", xprdrange);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Gate RGP Checklist For the Period " + value1 + " To " + value2, frm_qstr);
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
                ////MADE BY AKSHAY ....MERGED BY YOGITA

                //case "F40126": //done
                //        SQuery = "SELECT A.MSEQ, A.TYPE ,trim(B.INAME) as Item_NAME ,A.NUM1 AS M_RDY, trim(A.VCHNUM) as voucher_number ,to_char(A.VCHDATE,'dd/mm/yyyy') as Voucher_date, to_char((A.iqtyin+a.mlt_loss),'999,999,999,999.99') AS PRODN , to_char(A.MLT_LOSS,'999,999,999,999') as rejn,to_char(A.IQTYIN,'999,999,999,999.99') AS NET_PRODN ,to_char(A.IQTYOUT,'999,999,999,999.99') AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,to_char(ROUND((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000)),'999,999,999,999.99') as ppm ,ROUND((((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000))/10000),2) as ppm_prc FROM PROD_SHEET A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN('86','88') AND TRIM(NVL(A.MLT_LOSS,'0'))<>'0' AND TRIM(NVL(A.IQTYIN,'0'))<>'0' and trim(nvl(a.iqtyin+a.mlt_loss,'0'))<>'0'  AND A.VCHDATE " + xprdrange + "  ORDER BY A.VCHDATE,A.PREVCODE ";
                //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //        fgen.Fn_open_rptlevel("Daily Prodn Checklist(PP) For the Period " + fromdt + " To " + todt, frm_qstr);
                //    break;

                case "F40127": //done
                    //SQuery = "SELECT (sum(april)+sum(may)+sum(june)+sum(july)+sum(august)+sum(sept)+sum(oct)+sum(nov)+sum(dec)+sum(jan)+sum(feb)+sum(mar)) as total ,Item,Partno,sum(April) as April, sum(May) as May,sum(June) as June,  sum(July) as July, sum(August) as August,sum(Sept) as Sept,sum(oct) as Oct,sum(Nov) as Nov,sum(Dec) as Dec,sum(Jan) as Jan,sum(Feb) as Feb,sum(Mar) as Mar,icode from (Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'yyyymm'),201704,sum(a.iqtyin),0) as April, decode(to_chaR(vchdate,'yyyymm'),201705,sum(a.iqtyin),0) as May, decode(to_chaR(vchdate,'yyyymm'),201706,sum(a.iqtyin),0) as June, decode(to_chaR(vchdate,'yyyymm'),201707,sum(a.iqtyin),0) as July, decode(to_chaR(vchdate,'yyyymm'),201708,sum(a.iqtyin),0) as August,decode(to_chaR(vchdate,'yyyymm'),201709, sum(a.iqtyin),0) as Sept, decode(to_chaR(vchdate,'yyyymm'),201710,sum(a.iqtyin),0) as Oct, decode(to_chaR(vchdate,'yyyymm'),201711,sum(a.iqtyin),0) as Nov, decode(to_chaR(vchdate,'yyyymm'),201712,sum(a.iqtyin),0) as Dec , decode(to_chaR(vchdate,'yyyymm'),201801,sum(a.iqtyin),0) as Jan, decode(to_chaR(vchdate,'yyyymm'),201802,sum(a.iqtyin),0) as Feb, decode(to_chaR(vchdate,'yyyymm'),201803,sum(a.iqtyin),0) as Mar,      a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "'  and substr(a.type,1,2)='15' and a.vchdate " + xprdrange + "  group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'yyyymm')  ) group by item,partno,icode order by item  ";
                    SQuery = "SELECT trim(icode) as item_code,trim(Item) as Item_Name,Partno,to_chaR((sum(april)+sum(may)+sum(june)+sum(july)+sum(august)+sum(sept)+sum(oct)+sum(nov)+sum(dec)+sum(jan)+sum(feb)+sum(mar)),'999,999,999,999.99') as total ,to_char(sum(April),'999,999,999,999.99') as April, to_char(sum(May),'999,999,999,999.99') as May,to_char(sum(June),'999,999,999,999.99') as June,  to_char(sum(July),'999,999,999,999.99') as July, to_char(sum(August),'999,999,999,999.99') as August,to_char(sum(Sept),'999,999,999,999.99') as Sept,to_char(sum(oct),'999,999,999,999.99') as Oct,to_char(sum(Nov),'999,999,999,999.99') as Nov,to_char(sum(Dec),'999,999,999,999.99') as Dec,to_char(sum(Jan),'999,999,999,999.99') as Jan,to_char(sum(Feb),'999,999,999,999.99') as Feb,to_char(sum(Mar),'999,999,999,999.99') as Mar from (Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname) as Item,trim(b.cpartno) as PArtno, decode(to_chaR(vchdate,'yyyymm'),201704,sum(a.iqtyin),0) as April, decode(to_chaR(vchdate,'yyyymm'),201705,sum(a.iqtyin),0) as May, decode(to_chaR(vchdate,'yyyymm'),201706,sum(a.iqtyin),0) as June, decode(to_chaR(vchdate,'yyyymm'),201707,sum(a.iqtyin),0) as July, decode(to_chaR(vchdate,'yyyymm'),201708,sum(a.iqtyin),0) as August,decode(to_chaR(vchdate,'yyyymm'),201709, sum(a.iqtyin),0) as Sept, decode(to_chaR(vchdate,'yyyymm'),201710,sum(a.iqtyin),0) as Oct, decode(to_chaR(vchdate,'yyyymm'),201711,sum(a.iqtyin),0) as Nov, decode(to_chaR(vchdate,'yyyymm'),201712,sum(a.iqtyin),0) as Dec , decode(to_chaR(vchdate,'yyyymm'),201801,sum(a.iqtyin),0) as Jan, decode(to_chaR(vchdate,'yyyymm'),201802,sum(a.iqtyin),0) as Feb, decode(to_chaR(vchdate,'yyyymm'),201803,sum(a.iqtyin),0) as Mar,a.icode from IVOUCHER a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.branchcd = '" + mbr + "'  and substr(a.type,1,2)='15' and a.vchdate " + xprdrange + "  group by a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'yyyymm')  ) group by item,partno,icode order by item ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Mthly Prodn Checklist(PP) Checklist For the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F35128":
                    //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    party_cd = "%";
                    part_cd = "%";

                    SQuery = "SELECT a.job_no as vchnum,a.job_Dt as vchdate,a.icode,b.iname,b.cpartno,b.unit,a.stage_name,a.job_qty as qty,a.planned,a.job_qty-a.planned as Balance,a.Req_time,a.stage_code from (select trim(job_no) as job_no,trim(job_Dt) as job_Dt,max(Name) as Stage_name,trim(Stage_code) as Stage_code,trim(icode) as icode,sum(qty) as job_Qty,sum(planned) as planned,round(max(Req_time),2) as Req_time,max(srno) as Jsrno from (select c.name,b.stagec as Stage_code,round(b.mtime1,2) as Req_time,a.vchnum as job_no,to_char(a.vchdate,'dd/mm/yyyy') as job_Dt,a.icode,a.qty,0 as planned,b.srno from costestimate a,itwstage b,type c where trim(b.stagec)=trim(c.type1) and c.id='K' and trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type='30' and a.vchdate " + xprdrange + " and a.srno=1 and b.stagec!='08' and trim(nvl(a.app_by,'-'))!='-' and trim(nvl(a.status,'-'))!='Y' and trim(nvl(a.status,'-'))!='Y' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' union all select null as name,stage,0 as Req_time,job_no,job_Dt,icode,0 as jobqty,a1 as pla_qty ,null as srno from prod_Sheet where branchcd='" + mbr + "' and type='90' and vchdate " + xprdrange + ") group by trim(Stage_code),trim(job_no),trim(job_Dt),trim(icode) ) a,item b where trim(a.icode)=trim(b.icode) order by a.job_no,a.job_dt,b.iname,a.Jsrno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Stage Wise Job Planning Status For the Period " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40128":
                    if (co_cd == "SPIR" || co_cd == "STLC")
                    {
                        WB_TABNAME = "INSPVCHK";
                    }
                    else
                    {
                        WB_TABNAME = "INSPVCH";
                    }
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //THIS TABLE IS NOT JOINED TO PARTY AND ITEM...AND REPS CONFIG NEED 3 TABLE
                    //SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='55' and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%'", xprdrange);
                    // SQuery = "select a.vchnum,a.vchdate,a.title,a.acode,a.icode,a.cpartno,a.col1,a.col2,a.col3  from inspvch a  where a.branchcd='" + mbr + "' and a.type='55' and a.vchdate " + xprdrange + " order by a.vchnum";
                    SQuery = "SELECT Title as Machine,col1 as DownTime_Reason,sum(qty8) as Mins_Lost,round(sum(qty8)/60,2) as Hrs_Lost from " + WB_TABNAME + "  where branchcd='" + mbr + "' and type='55' and vchdate " + xprdrange + " group by Title,col1 having sum(qty8)>0 order by col1,title ";//MADE BY AKSHY
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Down Time Checklist(PP) For the Period " + fromdt + " To " + todt, frm_qstr);

                    //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    ////THIS TABLE IS NOT JOINED TO PARTY AND ITEM...AND REPS CONFIG NEED 3 TABLE
                    //SQuery = "SELECT Title as Machine,col1 as DownTime_Reason,sum(qty8) as Mins_Lost,round(sum(qty8)/60,2) as Hrs_Lost from inspvch  where branchcd='" + mbr + "' and type='55' and vchdate " + xprdrange + " and acode like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%' group by Title,col1 order by col1,title";//MADE BY AKSHY
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Down Time Checklist(PP) For the Period " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40129":
                    if (co_cd == "SPIR" || co_cd == "STLC")
                    {
                        WB_TABNAME = "INSPVCHK";
                    }
                    else
                    {
                        WB_TABNAME = "INSPVCH";
                    }
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    //THIS TABLE IS NOT JOINED TO PARTY AND ITEM...AND REPS CONFIG NEED 3 TABLE
                    //SQuery = fgen.makeRepQuery(frm_qstr, co_cd, val, branch_Cd, "a.type='55' and a.acode like '" + party_cd + "%' and substr(a.icode,1,2) like '" + part_cd + "%'", xprdrange);
                    //  SQuery = "select a.vchnum,a.vchdate,a.title,a.acode,a.icode,a.cpartno,a.col1,a.col2,a.col3  from inspvch a  where a.branchcd='" + mbr + "' and a.type='45' and a.vchdate " + xprdrange + " order by a.vchnum";
                    SQuery = "SELECT Title as Machine,col1 as Rejn_Reason,sum(sampqty) As prodn,sum(qty8) as Rej_Qty,(Case when sum(sampqty)>0 then round((sum(qty8)/sum(sampqty))*100,2) else 0 end) As Rejn_perc from " + WB_TABNAME + "  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + "  group by Title,col1 having sum(qty8)>0 order by col1,title "; //MADE BY AKSHAY
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Rejection Checklist(PP) For the Period " + fromdt + " To " + todt, frm_qstr);


                    //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    ////THIS TABLE IS NOT JOINED TO PARTY AND ITEM...AND REPS CONFIG NEED 3 TABLE
                    //SQuery = "SELECT Title as Machine,col1 as Rejn_Reason,sum(sampqty) As prodn,sum(qty8) as Rej_Qty,(Case when sum(sampqty)>0 then round((sum(qty8)/sum(sampqty))*100,2) else 0 end) As Rejn_perc from inspvch  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + " and acode like '" + party_cd + "%' and trim(icode) like '" + part_cd + "%'  group by Title,col1 order by col1,title"; //MADE BY AKSHAY
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Rejection Checklist(PP) For the Period " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40130":  //Operator Wise production Rejection
                    SQuery = "SELECT obsv16 AS OPERATOR,Title as Machine,col1 as Rejn_Reason,sum(sampqty) As prodn,sum(qty8) as Rej_Qty,(Case when sum(sampqty)>0 then round((sum(qty8)/sum(sampqty))*100,2) else 0 end) As Rejn_perc from inspvch  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + " group by Title,col1,obsv16 order by col1,title ASC";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Operator Wise production Rejection For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;


                case "F40136": //done
                    SQuery = "SELECT TO_CHAR(a.vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(sum(a.Prodn),'999,999,999') as Prodn_qty,to_char(sum(a.Rejqty),'999,999,999') as Rejn_Qty,(Case when sum(a.Prodn)>0 then round((sum(a.rejqty)/sum(a.Prodn))*100,2) else 0 end) as Rejn_Perc,to_char(sum(prodwt),'999,999,999,999.99') as prodwt,to_char(sum(rejnwt),'999,999,999,999.99') as rejnwt from (select a.vchdate,a.vchnum,a.icode,a.iqtyin as prodn,0 as rejqty,a.iqtyin*b.iweight as prodwt,0 as rejnwt  from prod_Sheet a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type='88' and a.VCHDATE  " + xprdrange + "  union all select a.vchdate,a.vchnum,a.icode,0 as prodn,a.qty8 as rejqty,0 as prodwt,a.qty8*b.iweight as rejnwt  from inspvch a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type='45' and a.VCHDATE  " + xprdrange + " ) a group by a.vchdate order by a.vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Corr.Prodn Vs Rejection Data For the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40137": //done
                    if (co_cd == "SPIR" || co_cd == "STLC")
                    {
                        WB_TABNAME = "INSPVCHK";
                    }
                    else
                    {
                        WB_TABNAME = "INSPVCH";
                    }
                    SQuery = "SELECT to_Char(vchdate,'YYYY MONTH') as Month_Name,trim(Title) as Machine,trim(col1) as Rejn_Reason,to_char(sum(qty8),'999,999,999') as Rej_Qty,to_Char(vchdate,'YYYYMM') as Mth_char from " + WB_TABNAME + "  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + " group by Title,col1,to_Char(vchdate,'YYYY MONTH'),to_Char(vchdate,'YYYYMM') HAVING sum(qty8)>0 order by to_Char(vchdate,'YYYYMM'),col1,title ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(" Trend of Rejection(D-T-D) For the Period  " + fromdt + " To " + todt, frm_qstr);
                    //SQuery = "SELECT to_Char(vchdate,'YYYY MONTH') as Month_Name,trim(Title) as Machine,trim(col1) as Rejn_Reason,to_char(sum(qty8),'999,999,999') as Rej_Qty,to_Char(vchdate,'YYYYMM') as Mth_char from inspvch  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + "  group by Title,col1,to_Char(vchdate,'YYYY MONTH'),to_Char(vchdate,'YYYYMM') order by to_Char(vchdate,'YYYYMM'),col1,title ";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel(" Trend of Rejection(D-T-D) For the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40138": //done

                    if (co_cd == "SPIR" || co_cd == "STLC")
                    {
                        WB_TABNAME = "INSPVCHK";
                    }
                    else
                    {
                        WB_TABNAME = "INSPVCH";
                    }
                    SQuery = "SELECT to_Char(vchdate,'YYYYMONTH') as Month_Name,TRIM(Title) as Machine,TRIM(col1) as DownTime_Reason,TO_CHAR(sum(qty8),'999,999,999') as Mins_Lost,to_Char(vchdate,'YYYYMM') as Mth_char from " + WB_TABNAME + "  where branchcd='" + mbr + "' and type='55' and vchdate " + xprdrange + "  group by Title,col1,to_Char(vchdate,'YYYYMONTH'),to_Char(vchdate,'YYYYMM') HAVING sum(qty8)>0 order by to_Char(vchdate,'YYYYMM'),col1,title";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Trend of DownTime(D-T-D) For the Period  " + fromdt + " To " + todt, frm_qstr);
                    //SQuery = "SELECT to_Char(vchdate,'YYYYMONTH') as Month_Name,TRIM(Title) as Machine,TRIM(col1) as DownTime_Reason,TO_CHAR(sum(qty8),'999,999,999') as Mins_Lost,to_Char(vchdate,'YYYYMM') as Mth_char from inspvch  where branchcd='" + mbr + "' and type='55' and vchdate " + xprdrange + "  group by Title,col1,to_Char(vchdate,'YYYYMONTH'),to_Char(vchdate,'YYYYMM') order by to_Char(vchdate,'YYYYMM'),col1,title";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Trend of DownTime(D-T-D) For the Period  " + fromdt + " To " + todt, frm_qstr);
                    break;

                #region ABOX Reports Made By Akshay
                //------------- ABOX view report 07/08/2018 
                case "15250A":  // wfinsys_erp id
                case "F40302": //comma done
                    #region Estimate Projection
                    // changed by akshay 
                    //m1 = fgen.seek_iname(co_cd, "select params from controls where id='R02'", "params");                   
                    //xprd1 = "between to_date('" + m1 + "','dd/mm/yyyy')and  to_date('" + hffromdt.Value + "','dd/mm/yyyy')-1";
                    //xprd2 = "between to_date('" + m1 + "','dd/mm/yyyy')and  to_date('" + hftodt.Value + "','dd/mm/yyyy')";
                    //xprdrange1 = "between to_date('" + cDT1 + "','dd/mm/yyyy')and  to_date('" + hffromdt.Value + "','dd/mm/yyyy')-1";                    
                    //xprdrange = "between to_date('" + hffromdt.Value + "','dd/mm/yyyy')and  to_date('" + hftodt.Value + "','dd/mm/yyyy')";
                    //mq5 = "between to_date('" + cDT1 + "','dd/mm/yyyy')and  to_date('" + hftodt.Value + "','dd/mm/yyyy')";
                    // changed by akshay
                    mq9 = "";
                    //hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Trim();
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R02'", "params");
                    xprd1 = "between to_date('" + m1 + "','dd/mm/yyyy')and  to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    xprd2 = "between to_date('" + m1 + "','dd/mm/yyyy')and  to_date('" + todt + "','dd/mm/yyyy')";
                    xprdrange1 = "between to_date('" + cDT1 + "','dd/mm/yyyy')and  to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    xprdrange = "between to_date ('" + fromdt + "','dd/mm/yyyy')and to_date('" + todt + "','dd/mm/yyyy')";

                    mq5 = "between to_date('" + cDT1 + "','dd/mm/yyyy')and  to_date('" + todt + "','dd/mm/yyyy')";    //changed by akshay                    
                    mq10 = "select sum(b.qty) as qty from item a ,costestimate b where trim(a.icode)=trim(b.icode) and b." + branch_Cd + " and   b.VCHDATE " + xprdrange + " and trim(b.type)='40'";
                    mq10 = fgen.seek_iname(frm_qstr, co_cd, mq10, "qty");
                    if (mq10 == "0") { mq10 = "1"; }
                    mq9 = "select  sum(Net_Used) as Net_Used from ( select a.icode,c.iname,c.unit,nvl(sum(a.opening),0) as opening,sum(wip_op) as wip_Op,nvl(sum(a.qtyin),0) as Recv,sum(a.send) as send,nvl(sum(a.qtyout),0) as Issued,sum(a.retn) as retn,sum(a.wip_cl) as WIP_CL,round(sum(a.qtyout)-SUM(a.retn),0) as Net_Used,sum(a.opening)+sum(a.qtyin)+sum(retn)-sum(a.qtyout)-sum(send) as Closing,b.irate as Basic_Rate ,nvl(sum(a.qtyout)-SUM(a.retn),0) as Total_Value,round(sum(a.qtyout)-SUM(a.retn),4) as Per_Kg_Value from  item c, ( select branchcd, trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(send) as send,sum(retn) as retn,sum(opening)+sum(cdr)-sum(ccr) as clqty ,0 as wip_op,0 as wip_cl  from (Select A.branchcd,trim(A.icode) as icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos  from itembal a,item b  where trim(a.icode)=trim(b.icode) and A." + branch_Cd + "  union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos from IVOUCHER  where  " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE,branchcd,type union all select branchcd,icode,0 as op,(case when substr(type,1,1)!='1' then sum(iqtyin) else 0 end) as cdr,(case when type not like '2%' then sum(iqtyout) else 0 end) as ccr,(case when type like '2%' then sum(iqtyout) else 0 end) as send,(case when substr(type,1,1)='1' then sum(iqtyin) else 0 end) as retn ,0 as clos from  IVOUCHER where " + branch_Cd + " and type like '%'   and vchdate  " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type)group by branchcd,icode union all select branchcd,trim(icode) as icode,0 as opening,0 as qtyin,0 as qtyout,0 as send,0 as retn,0 as clqty,sum(wip_op) as wip_op,sum(wip_cl) as wip_cl from (select TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,sum(qtyin)-sum(qtyout) as wip_op,0 as wip_cl from  (SELECT branchcd, TRIM(ICODE) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE " + branch_Cd + " and TYPE LIKE '3%'  and VCHDATE " + xprd1 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='25' and VCHDATE " + xprd1 + " ) group by branchcd,icode union all select branchcd,icode,0 as wip_op,sum(qtyin)-sum(qtyout) as wip_cl from  (SELECT TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE " + branch_Cd + " and TYPE LIKE '3%'  and VCHDATE " + xprd2 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE TYPE='25' and " + branch_Cd + " and VCHDATE " + xprd2 + " )  group by branchcd,icode ) GROUP BY BRANCHCD,ICODE ) a left outer join (select a.branchcd,b.iname ,b.unit,B.ISSU_UOM,a.icode,round(sum(a.iqtyin*(case when  a.ichgs=0  or a.ichgs is null then 1  else a.ichgs end))/sum(a.iqtyin),2) as irate from item b left outer join ivoucher a on  trim(a.icode)=trim(b.icode)  where substr(a.type,1,2)<'08'  and a.VCHDATE " + mq5 + "  and a.iqtyin<>0 group by a.branchcd,a.icode,b.iname,b.unit,B.ISSU_UOM) b on trim(a.icode)=trim(b.icode) where trim(a.icode)=trim(c.icode) and substr(trim(a.icode),1,2) in('07')  and length(trim(c.icode))='8' group by a.icode,c.iname,b.irate,c.unit)";
                    mq9 = fgen.seek_iname(frm_qstr, co_cd, mq9, "net_used");
                    if (mq9 == "0")
                    {
                        mq9 = "1";
                    }
                    mq3 = "select branchcd,icode,sum(iqtyin) as iqtyin,round(sum(basicval)/sum(iqtyin),4) as brate,round(sum(lvalue)/sum(iqtyin),4) as lcrate from (select branchcd,icode,vchnum,vchdate,iqtyin,irate*iqtyin as basicval,ichgs*iqtyin as lvalue from ivoucher where " + branch_Cd + " and trim(type) in ('02','05','07') and vchnum like'%' and vchdate " + mq5 + ") group by branchcd,icode";
                    DataTable dtrate1 = new DataTable();
                    dtrate1 = fgen.getdata(frm_qstr, co_cd, mq3);
                    cond = " in (" + hfcode.Value + ")";
                    mq2 = "select  substr(trim(a.icode),1,2) as mgrp, A.icode as icode,A.INAME as iname,nvl(sum(A.opening),0) as opening,sum(A.wip_op) as wip_Op,nvl(sum(A.Recv),0) as Recv,sum(A.send) as send,nvl(sum(A.Issued),0) as Issued,sum(retn) as retn,sum(A.wip_cl) as WIP_CL,round(nvl(sum(A.Net_Used),0),0) as Net_Used,sum(A.Closing) as Closing,0 as brate,0 as lcrate ,sum(A.Total_Value) as Total_Value,sum(A.Per_Kg_Value) as Per_Kg_Value from ( select a.icode,c.iname,c.unit,nvl(sum(a.opening),0) as opening,sum(wip_op) as wip_Op,nvl(sum(a.qtyin),0) as Recv,sum(a.send) as send,nvl(sum(a.qtyout),0) as Issued,sum(a.retn) as retn,sum(a.wip_cl) as WIP_CL,round(sum(a.qtyout)-SUM(a.retn),0) as Net_Used,sum(a.opening)+sum(a.qtyin)+sum(retn)-sum(a.qtyout)-sum(send) as Closing,b.irate as Basic_Rate ,nvl(sum(a.qtyout)-SUM(a.retn),0) as Total_Value,round(nvl(sum(a.qtyout)-SUM(a.retn),0)/" + mq9 + ",10) as Per_Kg_Value from  item c, ( select branchcd, trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(send) as send,sum(retn) as retn,sum(opening)+sum(cdr)-sum(ccr) as clqty ,0 as wip_op,0 as wip_cl  from (Select A.branchcd,trim(A.icode) as icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos  from itembal a,item b  where trim(a.icode)=trim(b.icode) and A." + branch_Cd + "  union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos from IVOUCHER  where  " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE,branchcd,type union all select branchcd,icode,0 as op,(case when substr(type,1,1)!='1' then sum(iqtyin) else 0 end) as cdr,(case when type not like '2%' then sum(iqtyout) else 0 end) as ccr,(case when type like '2%' then sum(iqtyout) else 0 end) as send,(case when substr(type,1,1)='1' then sum(iqtyin) else 0 end) as retn ,0 as clos from  IVOUCHER where " + branch_Cd + " and type like '%'   and vchdate  " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type)group by branchcd,icode union all select branchcd,trim(icode) as icode,0 as opening,0 as qtyin,0 as qtyout,0 as send,0 as retn,0 as clqty,sum(wip_op) as wip_op,sum(wip_cl) as wip_cl from (select TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,sum(qtyin)-sum(qtyout) as wip_op,0 as wip_cl from  (SELECT branchcd, TRIM(ICODE) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE  " + branch_Cd + " and TYPE LIKE '3%'  and VCHDATE " + xprd1 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='25' and VCHDATE " + xprd1 + " ) group by branchcd,icode union all select branchcd,icode,0 as wip_op,sum(qtyin)-sum(qtyout) as wip_cl from  (SELECT TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE " + branch_Cd + " and TYPE LIKE '3%' and VCHDATE " + xprd2 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='25' and  VCHDATE " + xprd2 + " )  group by branchcd,icode ) GROUP BY BRANCHCD,ICODE ) a left outer join (select a.branchcd,b.iname ,b.unit,B.ISSU_UOM,a.icode,round(sum(a.iqtyin*(case when  a.ichgs=0  or a.ichgs is null then 1  else a.ichgs end))/sum(a.iqtyin),2) as irate from item b left outer join ivoucher a on  trim(a.icode)=trim(b.icode)  where substr(a.type,1,2)<'08'  and a.VCHDATE " + mq5 + "  and a.iqtyin<>0 group by a.branchcd,a.icode,b.iname,b.unit,B.ISSU_UOM) b on trim(a.icode)=trim(b.icode) where trim(a.icode)=trim(c.icode) and (substr(trim(a.icode),1,2) in('03','07') OR trim(B.ISSU_UOM)='Y') and substr(trim(a.icode),1,2) " + cond + " and length(trim(c.icode))='8' group by a.icode,c.iname,b.irate,c.unit) a,type b where trim(substr(icode,1,2))=trim(b.type1) and b.id='Y' group by a.icode,A.INAME,substr(trim(a.icode),1,2)";
                    //  mq2 = "select  substr(trim(a.icode),1,2) as mgrp, A.icode as icode,A.INAME as iname,nvl(sum(A.opening),0) as opening,sum(A.wip_op) as wip_Op,nvl(sum(A.Recv),0) as Recv,sum(A.send) as send,nvl(sum(A.Issued),0) as Issued,sum(retn) as retn,sum(A.wip_cl) as WIP_CL,round(nvl(sum(A.Net_Used),0),0) as Net_Used,sum(A.Closing) as Closing,0 as brate,0 as lcrate ,sum(A.Total_Value) as Total_Value,sum(A.Per_Kg_Value) as Per_Kg_Value from (select a.icode,c.iname,c.unit,nvl(sum(a.opening),0) as opening,sum(wip_op) as wip_Op,nvl(sum(a.qtyin),0) as Recv,sum(a.send) as send,nvl(sum(a.qtyout),0) as Issued,sum(a.retn) as retn,sum(a.wip_cl) as WIP_CL,round(sum(a.qtyout)-SUM(a.retn),0) as Net_Used,sum(a.opening)+sum(a.qtyin)+sum(retn)-sum(a.qtyout)-sum(send) as Closing,b.irate as Basic_Rate ,nvl(sum(a.qtyout)-SUM(a.retn),0) as Total_Value,round(nvl(sum(a.qtyout)-SUM(a.retn),0)/" + mq9 + ",10) as Per_Kg_Value from  item c, ( select branchcd, trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(send) as send,sum(retn) as retn,sum(opening)+sum(cdr)-sum(ccr) as clqty ,0 as wip_op,0 as wip_cl  from (Select A.branchcd,trim(A.icode) as icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos  from itembal a,item b  where trim(a.icode)=trim(b.icode) and A." + branch_Cd + "  union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos from IVOUCHER  where  " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE,branchcd,type union all select branchcd,icode,0 as op,(case when substr(type,1,1)!='1' then sum(iqtyin) else 0 end) as cdr,(case when type not like '2%' then sum(iqtyout) else 0 end) as ccr,(case when type like '2%' then sum(iqtyout) else 0 end) as send,(case when substr(type,1,1)='1' then sum(iqtyin) else 0 end) as retn ,0 as clos from  IVOUCHER where " + branch_Cd + " and type like '%'   and vchdate  " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type)group by branchcd,icode union all select branchcd,trim(icode) as icode,0 as opening,0 as qtyin,0 as qtyout,0 as send,0 as retn,0 as clqty,sum(wip_op) as wip_op,sum(wip_cl) as wip_cl from (select TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,sum(qtyin)-sum(qtyout) as wip_op,0 as wip_cl from  (SELECT branchcd, TRIM(ICODE) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE  " + branch_Cd + " and TYPE LIKE '3%'  and VCHDATE " + xprd1 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='25' and  VCHDATE " + xprd1 + " ) group by branchcd,icode union all select branchcd,icode,0 as wip_op,sum(qtyin)-sum(qtyout) as wip_cl from  (SELECT TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE " + branch_Cd + " and TYPE LIKE '3%' and VCHDATE " + xprd2 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='25' and VCHDATE " + xprd2 + " )  group by branchcd,icode ) GROUP BY BRANCHCD,ICODE ) a left outer join (select a.branchcd,b.iname ,b.unit,B.ISSU_UOM,B.icode,round(sum(a.iqtyin*(case when  a.ichgs=0  or a.ichgs is null then 1  else a.ichgs end))/sum(a.iqtyin),2) as irate from item b left outer join  (SELECT * FROM ivoucher A  where substr(a.type,1,2)<'08'  and a.VCHDATE " + mq5 + "  and a.iqtyin<>0 )a on  trim(a.icode)=trim(b.icode)  group by a.branchcd,B.icode,b.iname,b.unit,B.ISSU_UOM) b on trim(a.icode)=trim(b.icode) where trim(a.icode)=trim(c.icode) and (substr(trim(a.icode),1,2) in('03','07') OR trim(B.ISSU_UOM)='Y') and substr(trim(a.icode),1,2) " + cond + " and length(trim(c.icode))='8' group by a.icode,c.iname,b.irate,c.unit) a,type b where trim(substr(icode,1,2))=trim(b.type1) and b.id='Y' group by a.icode,A.INAME,substr(trim(a.icode),1,2)";

                    mq2 = "select  substr(trim(a.icode),1,2) as mgrp, A.icode as icode,A.INAME as iname,to_char(nvl(sum(A.opening),0),'999,999,999.99') as opening,to_char(sum(A.wip_op),'999,999,999,999.99') as wip_Op,to_char(nvl(sum(A.Recv),0),'999,999,999,999.99') as Recv,sum(A.send) as send,TO_CHAR(nvl(sum(A.Issued),0),'999,999,999,999.99') as Issued,TO_CHAR(sum(retn),'999,999,999,999.99') as retn,TO_CHAR(sum(A.wip_cl),'999,999,999,999.99')  as WIP_CL,TO_CHAR(round(nvl(sum(A.Net_Used),0),0),'999,999,999,999.99')  as Net_Used,TO_CHAR(sum(A.Closing),'999,999,999,999.99')  as Closing,0 as brate,0 as lcrate ,TO_CHAR(sum(A.Total_Value),'999,999,999,999.99')  as Total_Value,TO_CHAR(sum(A.Per_Kg_Value),'999,999,999.999')  as Per_Kg_Value FROM (select a.icode,c.iname,c.unit,nvl(sum(a.opening),0) as opening,sum(wip_op) as wip_Op,nvl(sum(a.qtyin),0) as Recv,sum(a.send) as send,nvl(sum(a.qtyout),0) as Issued,sum(a.retn) as retn,sum(a.wip_cl) as WIP_CL,round(sum(a.qtyout)-SUM(a.retn),0) as Net_Used,sum(a.opening)+sum(a.qtyin)+sum(retn)-sum(a.qtyout)-sum(send) as Closing,b.irate as Basic_Rate ,nvl(sum(a.qtyout)-SUM(a.retn),0) as Total_Value,round(nvl(sum(a.qtyout)-SUM(a.retn),0)/" + mq9 + ",10) as Per_Kg_Value from  item c, ( select branchcd, trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(send) as send,sum(retn) as retn,sum(opening)+sum(cdr)-sum(ccr) as clqty ,0 as wip_op,0 as wip_cl  from (Select A.branchcd,trim(A.icode) as icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos  from itembal a,item b  where trim(a.icode)=trim(b.icode) and A." + branch_Cd + "  union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as send,0 as retn,0 as clos from IVOUCHER  where  " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE,branchcd,type union all select branchcd,icode,0 as op,(case when substr(type,1,1)!='1' then sum(iqtyin) else 0 end) as cdr,(case when type not like '2%' then sum(iqtyout) else 0 end) as ccr,(case when type like '2%' then sum(iqtyout) else 0 end) as send,(case when substr(type,1,1)='1' then sum(iqtyin) else 0 end) as retn ,0 as clos from  IVOUCHER where " + branch_Cd + " and type like '%'   and vchdate  " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type)group by branchcd,icode union all select branchcd,trim(icode) as icode,0 as opening,0 as qtyin,0 as qtyout,0 as send,0 as retn,0 as clqty,sum(wip_op) as wip_op,sum(wip_cl) as wip_cl from (select TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,sum(qtyin)-sum(qtyout) as wip_op,0 as wip_cl from  (SELECT branchcd, TRIM(ICODE) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE  " + branch_Cd + " and TYPE LIKE '3%'  and VCHDATE " + xprd1 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='25' and  VCHDATE " + xprd1 + " ) group by branchcd,icode union all select branchcd,icode,0 as wip_op,sum(qtyin)-sum(qtyout) as wip_cl from  (SELECT TRIM(branchcd) AS BRANCHCD,TRIM(icode) AS ICODE,IQTYOUT AS QTYIN,0 AS QTYOUT  FROM IVOUCHER WHERE " + branch_Cd + " and TYPE LIKE '3%' and VCHDATE " + xprd2 + "  UNION ALL SELECT branchcd,TRIM(ICODE) AS ICODE,0 AS QTYIN ,IS_NUMBER(COL4) AS QTYOUT  FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='25' and VCHDATE " + xprd2 + " )  group by branchcd,icode ) GROUP BY BRANCHCD,ICODE ) a left outer join (select a.branchcd,b.iname ,b.unit,B.ISSU_UOM,B.icode,round(sum(a.iqtyin*(case when  a.ichgs=0  or a.ichgs is null then 1  else a.ichgs end))/sum(a.iqtyin),2) as irate from item b left outer join  (SELECT * FROM ivoucher A  where substr(a.type,1,2)<'08'  and a.VCHDATE " + mq5 + "  and a.iqtyin<>0 )a on  trim(a.icode)=trim(b.icode)  group by a.branchcd,B.icode,b.iname,b.unit,B.ISSU_UOM) b on trim(a.icode)=trim(b.icode) where trim(a.icode)=trim(c.icode) and (substr(trim(a.icode),1,2) in('03','07') OR trim(B.ISSU_UOM)='Y') and substr(trim(a.icode),1,2) " + cond + " and length(trim(c.icode))='8' group by a.icode,c.iname,b.irate,c.unit) a,type b where trim(substr(icode,1,2))=trim(b.type1) and b.id='Y' group by a.icode,A.INAME,substr(trim(a.icode),1,2)";
                    SQuery = "select * from (" + mq1 + " union all " + mq2 + ") order by icode ";
                    //Session["squery"] = SQuery;

                    SQuery = "select * from(" + mq2 + ") order by icode ";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt3 = new DataTable();
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["brate"] = fgen.seek_iname_dt(dtrate1, "icode='" + dr["icode"].ToString() + "'", "brate");
                        dr["lcrate"] = fgen.seek_iname_dt(dtrate1, "icode='" + dr["icode"].ToString() + "'", "lcrate");
                        //dr["total_value"] = fgen.return_double(fgen.seek_iname_dt(dtrate1, "icode='" + dr["icode"].ToString() + "'", "lcrate")) * fgen.return_double(dr["total_value"].ToString());
                        //dr["per_kg_value"] = Math.Round(fgen.return_double(fgen.seek_iname_dt(dtrate1, "icode='" + dr["icode"].ToString() + "'", "lcrate")) * fgen.return_double(dr["per_kg_value"].ToString()), 4);

                        // changed by akshay
                        dr["total_value"] = fgen.make_double(fgen.seek_iname_dt(dtrate1, "icode='" + dr["icode"].ToString() + "'", "lcrate")) * fgen.make_double(dr["total_value"].ToString());
                        dr["per_kg_value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dtrate1, "icode='" + dr["icode"].ToString() + "'", "lcrate")) * fgen.make_double(dr["per_kg_value"].ToString()), 4);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im1 = new DataView(dt);
                        DataTable dtdrsim2 = new DataTable();
                        dtdrsim2 = view1im1.ToTable(true, "mgrp");

                        dt3 = dt.Clone();
                        string check;
                        foreach (DataRow dr0_1 in dtdrsim2.Rows)
                        {
                            DataView viewim1 = new DataView(dt, "mgrp='" + dr0_1["mgrp"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim1.ToTable();
                            double tot = 0;
                            DataRow dr0 = dt3.NewRow();

                            foreach (DataColumn dc in dt1.Columns)
                            {
                                double total = 0;

                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 12 || dc.Ordinal == 13)
                                {

                                }
                                else
                                {
                                    foreach (DataRow drrr in dt1.Rows)
                                    {
                                        //total += fgen.return_double(drrr[dc.ToString()].ToString());
                                        total += fgen.make_double(drrr[dc.ToString()].ToString());  // changed by akshay                                       

                                    }
                                    if (total > 0)
                                    {
                                        check = total.ToString("###,###,###.###");
                                    }
                                    else
                                    {
                                        check = "0";
                                    }

                                    dr0[dc.ColumnName] = check;
                                    //dr0[dc.ColumnName] = total;
                                }
                            }
                            dr0["mgrp"] = dr0_1["mgrp"].ToString();

                            dt3.Rows.Add(dr0);
                        }

                        dt3.Columns.Remove("brate");
                        dt3.Columns.Remove("lcrate");
                        dt3.Columns.Remove("icode");
                        dt3.Columns.Remove("iname");

                        DataRow dr5 = dt3.NewRow();
                        string check1;
                        foreach (DataColumn dc in dt3.Columns)
                        {
                            double total = 0;
                            if (dc.Ordinal == 0)
                            { }
                            else
                            {
                                foreach (DataRow drrr in dt3.Rows)
                                {
                                    //total += fgen.return_double(drrr[dc.ToString()].ToString());
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());  // changed by akshay                                     
                                }

                            }
                            if (total > 0)
                            {
                                check1 = total.ToString("###,###,###");
                            }
                            else
                            {
                                check1 = "0";
                            }
                            dr5[dc] = check1;
                            //dr5[dc] = total;
                        }

                        dt3.Rows.InsertAt(dr5, 0);
                    }
                    Session["send_dt"] = dt3;

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Estimate Projection Between For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15250B":// wfinsys_erp id
                case "F40304": // comma done
                    #region Date Wise Consumption
                    if (co_cd == "MCPL")
                    {
                        SQuery = " select VCHDATE AS  DATE_,ICODE,INAME,UNIT,JOB_NO,JOB_DT,ICODE,REELNO,CONSUMED,ORDDT from (SELECT '-' AS VCHDATE,'-' as  ICODE,'-' AS INAME,'-' AS UNIT,SUM(IS_NUMBER(COL4)) AS CONSUMED ,'-' AS REELNO ,'-' AS JOB_NO,'-' AS JOB_DT, '-' AS ORDDT  FROM COSTESTIMATE WHERE " + branch_Cd + " AND  TYPE='25' AND VCHDATE " + xprdrange + "  union all SELECT TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCHDATE,a.ICODE,B.INAME,B.UNIT,SUM(IS_NUMBER(A.COL4)) AS CONSUMED ,a.COL6 AS REELNO ,A.ENQNO AS JOB_NO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS JOB_DT ,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS ORDDT FROM COSTESTIMATE A ,ITEM B  WHERE A." + branch_Cd + " AND  A.TYPE='25' AND A.VCHDATE " + xprdrange + " AND TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.ICODE,A.VCHDATE,A.ENQNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') ,A.COL6,B.INAME,B.UNIT,TO_CHAR(A.VCHDATE,'YYYYMMDD')) order by ORDDT,JOB_NO";
                    }
                    else
                    {
                        mq1 = "SELECT '-' AS Date_,'-' as vdd,to_char(sum(a.qtyout),'999,999,999.99') as Prod_qty,to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)),'999,999,999.99') AS STD_WT_rQD,to_char((sum(a.rej_qty)+(sum(a.scrp2) +sum(a.time2) +sum(a.time1) +sum(a.scrp1))),'999,999,999.99') as wastage,to_char(sum(A.QTYIN),'999,999,999.99') AS WT_CONSUME,to_char((sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3))+(sum(a.rej_qty)+(sum(a.scrp2) +sum(a.time2) +sum(a.time1) +sum(a.scrp1)))-sum(A.QTYIN)),'999,999,999.99') as diff_PPlan_vs_Act,to_char(sum(a.scrp2),'999,999,999.99') as fadda,to_char(sum(a.time2),'999,999,999.99') as cbamboo,to_char(sum(a.time1),'999,999,999.99') as tore,to_char(sum(a.scrp1),'999,999,999.99') as gsm_var  ,to_char(sum(rej_qty),'999,999,999.99') as rejection, round(((sum(a.rej_qty)+(sum(a.scrp2) +sum(a.time2) +sum(a.time1) +sum(a.scrp1))) / sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)) )*100,2) as waste_perc	 FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,c.IRATE AS SALERATE FROM ( select vchnum,vchdate,enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL,sum(rej_qty) as rej_qty from ( select a.vchnum,a.vchdate, A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS iQTYIN,SUM(A.ITATE*B.IRATE) AS VAL,0 as rej_qty from costestimate A,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A." + branch_Cd + " AND A.type='25' AND B.TYPE='02' and A.vchdate " + xprdrange + " group by a.vchnum,a.vchdate,A.enqno,A.enqdt,TRIM(A.aCODE) union all select a.vchnum,a.vchdate, a.enqno,a.enqdt,TRIM(a.aCODE) AS ACODE,0 as qtyin,sum(a.qty) as qtyout,to_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL,sum(is_number(b.col3)) as rej_qty from ( select branchcd,type,vchnum,vchdate,enqno,enqdt,acode,sum(qty) as qty,sum(is_number(col3)) as col3,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2 from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + " group by  branchcd,type,vchnum,vchdate,enqno,enqdt,acode ) a left outer join (select a.branchcd,a.vchnum,a.vchdate,sum(IS_NUMBER(a.col3))*c.irate as col3   from inspvch a,(SELECT DISTINCT BRANCHCD,VCHNUM,VCHDATE,TYPE,ICODE,ENQNO,ENQDT FROM costestimate) B,(SELECT DISTINCT BRANCHCD,VCHNUM,VCHDATE,ICODE,IRATE FROM  COSTESTIMATE  WHERE TYPE='30' ) C where a." + branch_Cd + " and a.type='45' and a.vchdate " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND TRIM(B.TYPE)='40' and trim(B.ENQNO)=TRIM(C.VCHNUM) AND TRIM(B.ENQDT)=TRIM(C.VCHDATE) group by a.branchcd,a.vchnum,a.vchdate,c.irate ) b  on trim(a.branchcd)=trim(b.branchcd) and trim(a.vchnum)=trim(b.vchnum) and trim(a.vchdate)=trim(b.vchdate) where a." + branch_Cd + " and a.type='40' and a.vchdate " + xprdrange + "  group by a.vchnum,a.vchdate, a.enqno,a.enqdt,TRIM(a.aCODE),to_number(replace(nvl(a.COL3,'0'),'-','0')) ) a group by a.vchnum,a.vchdate,a.enqno,a.enqdt,a.acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b." + branch_Cd + "  AND B.TYPE='30' and  a.vchdate " + xprdrange + " AND B.SRNO=0) A,inspmst c WHERE trim(A.acode)=trim(c.icode) and c.type='70' and c.srno=10";
                        mq2 = "SELECT TO_CHAR(A.vchdate,'DD/MM/YYYY') AS Date_,to_char(a.vchdate,'yyyymmdd') as vdd,to_char(sum(a.qtyout),'999,999,999.99') as Prod_qty,to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)),'999,999,999.99') AS STD_WT_rQD,to_char((sum(a.rej_qty)+(sum(a.scrp2) +sum(a.time2) +sum(a.time1) +sum(a.scrp1))),'999,999,999.99') as wastage,to_char(sum(A.QTYIN),'999,999,999.99') AS WT_CONSUME,to_char((sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3))+(sum(a.rej_qty)+(sum(a.scrp2) +sum(a.time2) +sum(a.time1) +sum(a.scrp1)))-sum(A.QTYIN)),'999,999,999.99') as diff_PPlan_vs_Act,to_char(sum(a.scrp2),'999,999,999.99') as fadda,to_char(sum(a.time2),'999,999,999.99') as cbamboo,to_char(sum(a.time1),'999,999,999.99') as tore,to_char(sum(a.scrp1),'999,999,999.99') as gsm_var  ,to_char(sum(rej_qty),'999,999,999.99') as rejection, round(((sum(a.rej_qty)+(sum(a.scrp2) +sum(a.time2) +sum(a.time1) +sum(a.scrp1))) / sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)) )*100,2) as waste_perc	 FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,c.IRATE AS SALERATE FROM ( select vchnum,vchdate,enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL,sum(rej_qty) as rej_qty from ( select a.vchnum,a.vchdate, A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS iQTYIN,SUM(A.ITATE*B.IRATE) AS VAL,0 as rej_qty from costestimate A,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A." + branch_Cd + " AND A.type='25' AND B.TYPE='02' and A.vchdate " + xprdrange + " group by a.vchnum,a.vchdate,A.enqno,A.enqdt,TRIM(A.aCODE) union all select a.vchnum,a.vchdate, a.enqno,a.enqdt,TRIM(a.aCODE) AS ACODE,0 as qtyin,sum(a.qty) as qtyout,to_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL,sum(is_number(b.col3)) as rej_qty from ( select branchcd,type,vchnum,vchdate,enqno,enqdt,acode,sum(qty) as qty,sum(is_number(col3)) as col3,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2 from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + " group by  branchcd,type,vchnum,vchdate,enqno,enqdt,acode ) a left outer join (select a.branchcd,a.vchnum,a.vchdate,sum(IS_NUMBER(a.col3))*c.irate as col3   from inspvch a,(SELECT DISTINCT BRANCHCD,VCHNUM,VCHDATE,TYPE,ICODE,ENQNO,ENQDT FROM costestimate) B,(SELECT DISTINCT BRANCHCD,VCHNUM,VCHDATE,ICODE,IRATE FROM  COSTESTIMATE  WHERE TYPE='30' ) C where a." + branch_Cd + " and a.type='45' and a.vchdate " + xprdrange + " and trim(a.icode)=trim(b.icode) and trim(a.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND TRIM(B.TYPE)='40' and trim(B.ENQNO)=TRIM(C.VCHNUM) AND TRIM(B.ENQDT)=TRIM(C.VCHDATE) group by a.branchcd,a.vchnum,a.vchdate,c.irate ) b  on trim(a.branchcd)=trim(b.branchcd) and trim(a.vchnum)=trim(b.vchnum) and trim(a.vchdate)=trim(b.vchdate) where a." + branch_Cd + " and a.type='40' and a.vchdate " + xprdrange + "  group by a.vchnum,a.vchdate, a.enqno,a.enqdt,TRIM(a.aCODE),to_number(replace(nvl(a.COL3,'0'),'-','0')) ) a group by a.vchnum,a.vchdate,a.enqno,a.enqdt,a.acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b." + branch_Cd + " and B.TYPE='30' AND a.vchdate " + xprdrange + " AND  B.SRNO=0) A,inspmst c WHERE trim(A.acode)=trim(c.icode) and c.type='70' and c.srno=10 group by TO_CHAR(A.vchdate,'DD/MM/YYYY'),to_char(a.vchdate,'yyyymmdd')";
                        SQuery = "select a.* from (" + mq1 + " union all " + mq2 + ")a order by vdd";
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns.Remove("vdd");
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Date Wise Consumption For the Period " + fromdt + " To " + todt, frm_qstr);
                    #endregion
                    break;

                case "15250C_old":// wfinsys_erp id
                case "F40305_old": //comma done
                    #region Percentage Party & BF Wise Consumption
                    mq0 = "select distinct oprate3 as gsm from item where substr(icode,1,2)='07' and trim(oprate3) is not null and trim(oprate3)<>'-' order by oprate3";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    DataTable dtm = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dtm.Columns.Add("PARTY", typeof(string));
                        dtm.Columns.Add("BF", typeof(string));
                        dtm.Columns.Add("QTY", typeof(string));
                        dtm.Columns.Add("_%AGE", typeof(string));

                        foreach (DataRow dr in dt.Rows)
                        {
                            dtm.Columns.Add("G" + dr[0], typeof(string));
                        }

                        mq1 = "select trim(c.bfactor) as bfactor,trim(e.aname) as acode,trim(c.oprate3) as oprate3,sum(is_number(a.col4)) as col4 from costestimate a,item c,REELVCH d,famst e  where trim(d.acode)=trim(e.acode) and  trim(a.col6)=trim(d.kclreelno) and d.type='02' and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type='25' and a.vchdate " + xprdrange + "  group by trim(c.bfactor),e.aname,trim(c.oprate3) order by bfactor,oprate3,acode";
                        dt = fgen.getdata(frm_qstr, co_cd, mq1);
                        if (dt.Rows.Count > 0)
                        {
                            view1im = new DataView(dt);
                            dtdrsim = view1im.ToTable(true, "bfactor", "acode");
                            iqtyout_sum = Convert.ToDouble(dt.Compute("Sum ( col4 ) ", ""));

                            foreach (DataRow dr0 in dtdrsim.Rows)
                            {
                                DataRow drrow = dtm.NewRow();
                                DataView viewim = new DataView(dt, "bfactor='" + dr0["bfactor"] + "' and acode='" + dr0["acode"] + "'", "", DataViewRowState.CurrentRows);
                                dt1 = viewim.ToTable();
                                double tot = 0;
                                if (dt1.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        string mm = dt1.Rows[i]["oprate3"].ToString().Trim();
                                        drrow["G" + mm] = dt1.Rows[i]["col4"];
                                        //tot = tot + fgen.return_double(dt1.Rows[i]["col4"].ToString());
                                        tot = tot + fgen.make_double(dt1.Rows[i]["col4"].ToString());
                                    }
                                    drrow["Party"] = dt1.Rows[0]["acode"];
                                    drrow["BF"] = dt1.Rows[0]["bfactor"];
                                    drrow["qty"] = tot.ToString("###,###.## ");
                                    drrow["_%AGE"] = Math.Round((tot / iqtyout_sum) * 100, 2).ToString("###,###.##");
                                    dtm.Rows.Add(drrow);
                                }
                            }

                            dr2 = dtm.NewRow();
                            d = 0;

                            foreach (DataColumn dc in dtm.Columns)
                            {
                                double total = 0;
                                if (dc.Ordinal == 0 || dc.Ordinal == 1) { }
                                else
                                {
                                    foreach (DataRow drrr in dtm.Rows)
                                    {
                                        //total += fgen.return_double(drrr[dc.ToString()].ToString());
                                        total += fgen.make_double(drrr[dc.ToString()].ToString());

                                    }
                                    string check = total.ToString("###,###.##");
                                    dr2[dc] = check;
                                    //dr2[dc] = total;
                                }
                            }
                            dr2["Party"] = '-';
                            dr2["BF"] = '-';
                            dtm.Rows.InsertAt(dr2, 0);
                        }
                    }
                    Session["send_dt"] = dtm;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Percentage Party & BF Wise Consumption For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15250C":// wfinsys_erp id
                case "F40305": //comma done
                    #region Percentage Party & BF Wise Consumption
                    mq0 = "select distinct oprate3 as gsm from item where substr(icode,1,2)='07' and trim(oprate3) is not null and trim(oprate3)<>'-' order by oprate3";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm = new DataTable();
                    dtm.Columns.Add("PARTY", typeof(string));
                    dtm.Columns.Add("BF", typeof(string));
                    dtm.Columns.Add("QTY", typeof(string));
                    dtm.Columns.Add("_%AGE", typeof(string));
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt2.Rows)
                        {
                            dtm.Columns.Add("G" + dr[0], typeof(string));
                        }
                    }
                    mq1 = "select trim(c.bfactor) as bfactor,trim(e.aname) as acode,trim(c.oprate3) as oprate3,sum(is_number(a.col4)) as col4 from costestimate a,item c,REELVCH d,famst e  where trim(d.acode)=trim(e.acode) and  trim(a.col6)=trim(d.kclreelno) and d.type='02' and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type='25' and a.vchdate " + xprdrange + "  group by trim(c.bfactor),e.aname,trim(c.oprate3) order by bfactor,oprate3,acode";
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "bfactor", "acode");
                        iqtyout_sum = Convert.ToDouble(dt.Compute("Sum ( col4 ) ", ""));

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow = dtm.NewRow();
                            DataView viewim = new DataView(dt, "bfactor='" + dr0["bfactor"] + "' and acode='" + dr0["acode"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    string mm = dt1.Rows[i]["oprate3"].ToString().Trim();
                                    if (dt2.Rows.Count > 0)
                                    {
                                        drrow["G" + mm] = dt1.Rows[i]["col4"];
                                    }
                                    //tot = tot + fgen.return_double(dt1.Rows[i]["col4"].ToString());
                                    tot = tot + fgen.make_double(dt1.Rows[i]["col4"].ToString());
                                }
                                drrow["Party"] = dt1.Rows[0]["acode"];
                                drrow["BF"] = dt1.Rows[0]["bfactor"];
                                drrow["qty"] = tot.ToString("###,###.## ");
                                drrow["_%AGE"] = Math.Round((tot / iqtyout_sum) * 100, 2).ToString("###,###.##");
                                dtm.Rows.Add(drrow);
                            }
                        }

                        dr2 = dtm.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm.Columns)
                        {
                            double total = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1) { }
                            else
                            {
                                foreach (DataRow drrr in dtm.Rows)
                                {
                                    //total += fgen.return_double(drrr[dc.ToString()].ToString());
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());

                                }
                                string check = total.ToString("###,###.##");
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        dr2["Party"] = '-';
                        dr2["BF"] = '-';
                        dtm.Rows.InsertAt(dr2, 0);
                    }

                    Session["send_dt"] = dtm;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Percentage Party & BF Wise Consumption For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15250D":// wfinsys_erp id // RPTLEVEL HD
                case "F40306": // comma done
                    #region BF Wise Consumption
                    SQuery = "SELECT * FROM (SELECT (CASE WHEN b.bfactor<>'-' THEN 'BF'||B.BFACTOR ELSE 'RCT'||B.MQTY9 END) AS BF,to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)),'999,999,999.99') AS STD_WT_rQD,to_char(sum(A.QTYIN),'999,999,999.99') AS WT_CONSUME, to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3))-sum(A.QTYIN),'999,999,999.99') AS Tot_Diff, to_char(sum(a.scrp2),'999,999,999.99') as fadda,to_char(sum(a.time2),'999,999,999.99') as cbamboo,to_char(sum(a.time1),'999,999,999.99') as tore,to_char(sum(a.scrp1),'999,999,999.99') as gsm_var FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,c.IRATE AS SALERATE FROM (select MAX(RMICODE) AS RMICODE,vchnum,vchdate,enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from ( select A.ICODE AS RMICODE,a.vchnum,a.vchdate, A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS iQTYIN,SUM(A.ITATE*B.IRATE) AS VAL from costestimate A,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A." + branch_Cd + " AND A.type='25' AND B.TYPE='02' and A.vchdate " + xprdrange + " group by a.vchnum,a.vchdate,A.enqno,A.enqdt,TRIM(A.aCODE),A.ICODE union all select  '-' AS RMICODE,vchnum,vchdate, enqno,enqdt,TRIM(aCODE) AS ACODE,0 as qtyin,sum(qty) as qtyout,to_number(replace(nvl(COL3,'0'),'-','0')) as col3,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + " group by vchnum,vchdate, enqno,enqdt,TRIM(aCODE),to_number(replace(nvl(COL3,'0'),'-','0'))) group by vchnum,vchdate,enqno,enqdt,acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b." + branch_Cd + "  AND B.TYPE='30' and a.vchdate " + xprdrange + " AND B.SRNO=0 ) A,ITEM B,inspmst c WHERE TRIM(A.RMICODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and c.type='70' and c.srno=10 group by (CASE WHEN b.bfactor<>'-' THEN 'BF'||B.BFACTOR ELSE 'RCT'||B.MQTY9 END) UNION ALL SELECT '-' AS BF,to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)),'999,999,999.99') AS STD_WT_rQD,to_char(sum(A.QTYIN),'999,999,999.99')  AS WT_CONSUME,to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3))-sum(A.QTYIN),'999,999,999.99') AS Tot_Diff,to_char(sum(a.scrp2),'999,999,999.99') as fadda,to_char(sum(a.time2),'999,999,999.99') as cbamboo,to_char(sum(a.time1),'999,999,999.99') as tore,to_char(sum(a.scrp1),'999,999,999.99') as gsm_var FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,c.IRATE AS SALERATE FROM (select MAX(RMICODE) AS RMICODE,vchnum,vchdate,enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from ( select A.ICODE AS RMICODE,a.vchnum,a.vchdate, A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS iQTYIN,SUM(A.ITATE*B.IRATE) AS VAL from costestimate A,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A." + branch_Cd + " AND A.type='25' AND B.TYPE='02' and A.vchdate " + xprdrange + " group by a.vchnum,a.vchdate,A.enqno,A.enqdt,TRIM(A.aCODE),A.ICODE union all select  '-' AS RMICODE,vchnum,vchdate, enqno,enqdt,TRIM(aCODE) AS ACODE,0 as qtyin,sum(qty) as qtyout,to_number(replace(nvl(COL3,'0'),'-','0')) as col3,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + " group by vchnum,vchdate, enqno,enqdt,TRIM(aCODE),to_number(replace(nvl(COL3,'0'),'-','0'))) group by vchnum,vchdate,enqno,enqdt,acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b." + branch_Cd + " AND B.TYPE='30' and a.vchdate " + xprdrange + " AND B.SRNO=0 ) A,ITEM B,inspmst c WHERE TRIM(A.RMICODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and c.type='70' and c.srno=10) ORDER BY BF";
                    Session["send_dt"] = fgen.getdata(frm_qstr, co_cd, SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("BF Wise Consumption For the Period " + fromdt + " To" + todt, frm_qstr);
                    #endregion
                    break;

                case "15250E": // wfinsys_erp id
                case "F40331":  //comma done
                    #region Corrugation Order Balance
                    SQuery = "";
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    xprd1 = "between to_date('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    SQuery = "CREATE OR REPLACE FORCE VIEW PENDING_SO_VU as (select branchcd,type,max(Closed) as closed,max(ciname) as ciname,max(cpartno) as cpartno,max(pordno) as pordno,max(porddt) porddt,acode,icode,ordno,orddt,sum(qtyord) as qtyord,sum(sale) as qty_out,sum(qtyord)-sum(sale) as bal from (select branchcd, type,cu_chldt,icat as Closed,ciname,cpartno,pordno,porddt,acode,icode,ordno,orddt,qtyord,0 as sale from somas where " + branch_Cd + " and type like '4%' and orddt " + xprd1 + "  union all select branchcd ,type,null as cu_chldt,null as icat,null as ciname,null as cpartno,null as pordno,null as porddt,acode,icode,ponum,podate,0 as qtyord,iqtyout as sale from ivoucher where " + branch_Cd + " and type like '4%' and vchdate " + xprd1 + ")group by BRANCHCD,type,acode,icode,ordno,orddt)";

                    fgen.execute_cmd(frm_qstr, co_cd, SQuery);
                    mq5 = "SELECT A.ICODE,A.INAME ,B.BRANCHCD,B.PLY_SIZE,B.DECKLE,B.MEASURES,B.UPS,b.ply FROM ITEM A LEFT OUTER JOIN (SELECT DISTINCT  BRANCHCD,ICODE,col15 as ply, COL16 AS PLY_SIZE,MAINTDT AS DECKLE,IS_NUMBER(BTCHDT) AS MEASURES ,NVL(REJQTY,1) AS UPS FROM INSPMST WHERE TYPE='70' AND BRANCHCD<>'DD') B ON TRIM(A.ICODE)=TRIM(B.ICODE)";
                    SQuery = "select * from (select '-' as Customer,'-' as Ordno,'-' as Ord_Dt,'-' as Cust_POrd,'-' as Cust_POrddt,'-' as type,'-' as ERP_code,'-' as Part_Number,'-' as Item_Name,'-' as unit,to_char(sum(qtyord),'999,999,999.99') as Qtyord, to_char(sum(qty_out),'999,999,999.99') as Qty_out, to_char(sum(bal),'999,999,999.99') as bal,'-' as ply_size,'-' as deckle,0 as measures,to_char(sum(Required_2Ply),'999,999,999.99') as Required_2Ply,to_char(sum(Required_Mtrs),'999,999,999.99') as Required_Mtrs,'-' as  Cust_Dlv_Dt,'-' as  orddtc from (select B.Aname as Customer,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.ply||' - '||e.ply_size,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as Required_2Ply,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from pending_so_vu a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) AND TRIM(a.icode)=trim(d.icode) and a.branchcd=trim(d.icode) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type like '4%' and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by  B.Aname,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy'),d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.ply_size,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,e.ply  ORDER BY a.ordno,orddtc,a.type,a.icode desc) union all  select B.Aname as Customer,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,to_char(sum(a.qtyord),'999,999,999.99') as Qtyord, to_char(sum(a.qty_out),'999,999,999.99') as Qty_out, to_char((sum(a.qtyord)-sum(a.qty_out)),'999,999,999.99') as bal,e.ply||' - '||e.ply_size,e.deckle,e.measures,to_char(round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0),'999,999,999.99') as Required_2Ply,to_char(round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0),'999,999,999.99') as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from pending_so_vu a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type like '4%' and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by  B.Aname,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy'),d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.ply_size,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,e.ply )  ORDER BY ordno,ord_dt,erp_code desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Corrugation Order Balance For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    //fgen.open_rptlevel_hd("Corrugation Order Balance for the period " + fromdt + " and " + todt + "");            
                    #endregion
                    break;

                case "15250F": // wfinsys_erp id
                case "F40307": // comma done
                    #region Date,Party,Item Wise Consumption
                    mq0 = "SELECT TO_CHAR(A.vchdate,'DD/MM/YYYY') AS Consume_Date,a.acode as icode,a.enqno as jobno,to_char(a.enqdt,'dd/mm/yyyy') as jobdt,b.iname as Item_name,d.aname as Part_Name,to_char(sum(A.QTYOUT),'999,999,999.99') as Prod_qty,to_char(sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)),'999,999,999.99') AS STD_WT_rQD,to_char(sum(A.QTYIN),'999,999,999.99') AS WT_CONSUME,to_char((sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3))-sum(A.QTYIN)),'999,999,999.99') as diff_,round(((sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3))-sum(A.QTYIN))/sum(round(A.QTYOUT * (round((a.enr2/a.STD_SHT_RQ)/c.rejqty,3)),3)))*100,2) as perc_  FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,c.IRATE AS SALERATE FROM ( select vchnum,vchdate,enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from ( select a.vchnum,a.vchdate, A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS iQTYIN,SUM(A.ITATE*B.IRATE) AS VAL from costestimate A,REELVCH B where TRIM(A.ICODE)||TRIM(A.COL6)=TRIM(B.ICODE)||TRIM(B.KCLREELNO) AND A." + branch_Cd + " AND A.type='25' AND B.TYPE='02' and A.vchdate " + xprdrange + " group by a.vchnum,a.vchdate,A.enqno,A.enqdt,TRIM(A.aCODE) union all select vchnum,vchdate, enqno,enqdt,TRIM(aCODE) AS ACODE,0 as qtyin,sum(qty) as qtyout,to_number(replace(nvl(COL3,'0'),'-','0')) as col3,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate where " + branch_Cd + " and type='40' and vchdate " + xprdrange + " group by vchnum,vchdate, enqno,enqdt,TRIM(aCODE),to_number(replace(nvl(COL3,'0'),'-','0')) ) group by vchnum,vchdate,enqno,enqdt,acode ) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b." + branch_Cd + " AND B.TYPE='30' and a.vchdate " + xprdrange + "  AND B.SRNO=0) A,ITEM B,inspmst c,famst d WHERE trim(c.acode)=trim(d.acode) and  TRIM(A.ACODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and c.type='70' and c.srno=10 group by TO_CHAR(A.vchdate,'DD/MM/YYYY'),a.acode,a.enqno,a.enqdt,b.iname,d.aname";
                    SQuery = "select * from( " + mq0 + ")  order by Consume_Date,jobno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Date,Party,Item Wise Consumption For the PeriodFor the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15250G":// wfinsys_erp id  // RPTLEVEL HD
                case "F40308":  //done
                    #region Daily Stock Report
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R02'", "params");
                    xprd1 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";
                    xprd2 = "between to_date('" + m1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1 ";
                    //mq3 = "create or replace view splrmk (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.PONUM,A.PODATE,A.POTYPE ,C.SPLRMK FROM IVOUCHER A ,(SELECT MAX(VCHNUM)  AS VCHNUM,MAX(VCHDATE) AS VCHDATE ,ICODE FROM IVOUCHER WHERE TYPE LIKE '0%' AND " + branch_Cd + " and  vchdate " + xprd1 + " GROUP BY ICODE) B, POMAS C WHERE  TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE)  AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.PONUM)=TRIM(C.ORDNO) AND TRIM(A.PODATE)=TRIM(C.ORDDT) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.POTYPE)=TRIM(C.TYPE) AND TRIM(A.ICODE)=TRIM(C.ICODE)";

                    mq1 = "select '-' AS ICODE,null iname,null AS NICODE,null AS NAME,null as mill_name,null as rsize,null as gsm,null as BF,to_char(sum(z.obal),'999,999,999.99' ) as obal,to_char(sum(z.bal),'999,999,999.99' ) as Reel,to_char(sum(z.partbal),'999,999,999.99' ) as Partreel,to_char(sum(z.rvcdqty),'999,999,999.99' ) as Recvd_qty,to_char(sum(z.rvcdqty_ret),'999,999,999.99' ) as Recvd_qty_ret,to_char(sum(z.rcvdreel),'999,999,999.99' ) as Recv_Reel,to_char(sum(z.issqty),'999,999,999.99' ) as Iss_qty,to_char(sum(z.issreel),'999,999,999.99' ) as Iss_Reel,to_char(sum(z.obal)+sum(z.rvcdqty)+sum(z.rvcdqty_ret)-sum(z.issqty),'999,999,999.99' ) as Clos_qty,null as Recv_date,null as Iss_date,null as Purpose_to,null AS Location_  from item X, item Y,(select ICODE,(case when sum(tot)=sum(iqtyin) then count(batchno) else 0 end) as bal,(case when sum(tot)=sum(iqtyin) then 0 else count(batchno) end) as partbal,sum(tot) as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,null rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from (select sum(iqtyin) as iqtyin, sum(tot) as tot,batchno,max(vchdate) as vchdate,ICODE as ICODE from (select distinct vchdate, trim(kclreelno) as Batchno,sum(nvl(reelwin,0)) as iqtyin,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where posted='Y' AND " + branch_Cd + " and  vchdate " + xprd2 + " group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm) where tot!=0 group by batchno,ICODE) where tot<>0 group by ICODE union all select ICODE,0 as bal,0 as partbal,0 as obal,sum(iqtyin) as rvcdqty,sum(iqtyin_ret) as rvcdqty_ret,count(batchno) rcvdreel,0 as issqty,0 as issreel,null rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from  (select distinct vchdate, trim(kclreelno) as Batchno,(case when type like'1%' or type='36' then 0 else sum(nvl(reelwin,0)) end) as iqtyin,(case when type like'1%' or type='36' then sum(nvl(reelwin,0)) else 0 end) as iqtyin_ret,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where  " + branch_Cd + " and substr(type,1,1) in ('0','1') AND vchdate " + xprd1 + " and  posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm,type) group by ICODE union all select ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,sum(tot) as issqty,count(batchno) as issreel,null rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from  (select distinct vchdate, trim(kclreelno) as Batchno,sum(nvl(reelwin,0)) as iqtyin,sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where " + branch_Cd + " and type like '3%'  AND vchdate " + xprd1 + " and  posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm)  group by ICODE union all select ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,max(vchdate) rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from  (select distinct vchdate, trim(kclreelno) as Batchno,sum(nvl(reelwin,0)) as iqtyin,sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where  " + branch_Cd + " and type like '0%' AND vchdate " + xprd1 + " and  posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm)  group by ICODE Union all select  DISTINCT A.ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,null rcvddt,A.vchdate as issdt,C.REELSPEC2 as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F, null as locn from (SELECT MAX(VCHNUM)  AS VCHNUM,MAX(VCHDATE) AS VCHDATE ,ICODE ,TYPE ,BRANCHCD FROM reelvch WHERE  " + branch_Cd + " and TYPE LIKE '3%'  AND vchdate " + xprd1 + " AND posted='Y' GROUP BY ICODE,TYPE ,BRANCHCD )A,REELVCH C  WHERE C." + branch_Cd + " and C.TYPE LIKE '3%'  AND C.vchdate " + xprd1 + " AND C.posted='Y' AND TRIM(A.BRANCHCD)=TRIM(C.BRANCHCD) AND TRIM(a.VCHNUM)=TRIM(C.VCHNUM) AND TRIM(A.TYPE)=TRIM(C.TYPE) AND A.VCHDATE=TRIM(C.VCHDATE) union all  sELECT  ICODE AS ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,null rcvddt,NULL as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null AS B_F,rtrim(xmlagg(xmlelement(e,RLOCN||',')).extract('//text()').extract('//text()'),',') as locn FROM(SELECT BRANCHCD,RLOCN,ICODE,SUM(REELWIN-REELWOUT)  AS BAL FROM REELVCH  where posted='Y' AND " + branch_Cd + " and  vchdate >sysdate-500  GROUP BY branchcd,ICODE,rlocn)   GROUP BY ICODE )Z WHERE TRIM(X.ICODE)=TRIM(Z.icode) and TRIM(substr(X.ICODE,0,4))=TRIM(y.ICODE)";
                    mq2 = "select trim(X.icode) AS ICODE,x.iname,trim(Y.ICODE) AS NICODE,trim(Y.INAME) AS NAME,X.PUR_UOM as mill_name,round((X.OPRATE1),1) as rsize,X.OPRATE3 as gsm,x.bfactor as BF,to_char(sum(z.obal),'999,999,999.99' ) as obal,to_char(sum(z.bal),'999,999,999.99' ) as Reel,to_char(sum(z.partbal),'999,999,999.99' ) as Partreel,to_char(sum(z.rvcdqty),'999,999,999.99' ) as Recvd_qty,to_char(sum(z.rvcdqty_ret),'999,999,999.99' ) as Recvd_qty_ret,to_char(sum(z.rcvdreel),'999,999,999.99' ) as Recv_Reel,to_char(sum(z.issqty),'999,999,999.99' ) as Iss_qty,to_char(sum(z.issreel),'999,999,999.99' ) as Iss_Reel,to_char(sum(z.obal)+sum(z.rvcdqty)+sum(z.rvcdqty_ret)-sum(z.issqty),'999,999,999.99' ) as Clos_qty,to_char(max(z.rcvddt),'dd/mm/yyyy') as Recv_date,to_char(max(z.issdt),'dd/mm/yyyy') as Iss_date,max(z.purpose) as Purpose_to,max(z.locn) AS Location_   from item X, item Y,(select ICODE,sum(bal) as bal,sum(partbal) as partbal,sum(obal) as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,null rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from (select ICODE,batchno,(case when sum(tot)=sum(iqtyin) then 1 else 0 end) as bal,(case when sum(tot)=sum(iqtyin) then 0 else 1 end) as partbal,sum(tot) as obal  from (select sum(iqtyin) as iqtyin, sum(tot) as tot,batchno,max(vchdate) as vchdate,ICODE as ICODE from (select distinct vchdate, trim(kclreelno) as Batchno,sum(nvl(reelwin,0)) as iqtyin,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where " + branch_Cd + " and  vchdate " + xprd2 + " AND posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm) where tot!=0 group by batchno,ICODE) where tot<>0 group by ICODE,batchno)group by icode union all select ICODE,0 as bal,0 as partbal,0 as obal,sum(iqtyin) as rvcdqty,sum(iqtyin_ret) as rvcdqty_ret,count(batchno) rcvdreel,0 as issqty,0 as issreel,null rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from  (select distinct vchdate, trim(kclreelno) as Batchno,(case when type like'1%' or type='36' then 0 else sum(nvl(reelwin,0)) end) as iqtyin,(case when type like'1%' or type='36' then sum(nvl(reelwin,0)) else 0 end) as iqtyin_ret,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where " + branch_Cd + " and substr(type,1,1) in ('0','1')  AND vchdate " + xprd1 + " and  posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm,type) group by ICODE union all select ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,sum(tot) as issqty,count(batchno) as issreel,null rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from  (select distinct vchdate, trim(kclreelno) as Batchno,sum(nvl(reelwin,0)) as iqtyin,sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where " + branch_Cd + " and type like '3%'  AND  vchdate " + xprd1 + " and  posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm)  group by ICODE union all select ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,max(vchdate) rcvddt,null as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F,null as locn from  (select distinct vchdate, trim(kclreelno) as Batchno,sum(nvl(reelwin,0)) as iqtyin,sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,TRIM(Coreelno) AS Coreelno from reelvch where " + branch_Cd + " and type like '0%' AND  vchdate " + xprd1 + " and  posted='Y' group by icode,trim(kclreelno),TRIM(Coreelno),vchdate,trim(acode),psize,gsm)  group by ICODE Union all select  DISTINCT A.ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,null rcvddt,A.vchdate as issdt,C.REELSPEC2 as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null as B_F, null as locn from (SELECT MAX(VCHNUM)  AS VCHNUM,MAX(VCHDATE) AS VCHDATE ,ICODE ,TYPE ,BRANCHCD FROM reelvch WHERE " + branch_Cd + " and TYPE LIKE '3%'  AND  vchdate " + xprd1 + " AND posted='Y' GROUP BY ICODE,TYPE ,BRANCHCD )A,REELVCH C  WHERE C." + branch_Cd + " and C.TYPE LIKE '3%'  AND C.vchdate " + xprd1 + " AND C.posted='Y' AND TRIM(A.BRANCHCD)=TRIM(C.BRANCHCD) AND TRIM(a.VCHNUM)=TRIM(C.VCHNUM) AND TRIM(A.TYPE)=TRIM(C.TYPE) AND A.VCHDATE=TRIM(C.VCHDATE) union all  sELECT  ICODE AS ICODE,0 as bal,0 as partbal,0 as obal,0 as rvcdqty,0 as rvcdqty_ret,0 rcvdreel,0 as issqty,0 as issreel,null rcvddt,NULL as issdt,null as purpose,null as purpose_for,null AS DECLE,null AS MEASURE,null AS PLY,null AS UPS ,null AS B_F,rtrim(xmlagg(xmlelement(e,RLOCN||',')).extract('//text()').extract('//text()'),',') as locn FROM(SELECT BRANCHCD,RLOCN,ICODE,SUM(REELWIN-REELWOUT)  AS BAL FROM REELVCH  where posted='Y' AND " + branch_Cd + " and  vchdate >sysdate-500  GROUP BY branchcd,ICODE,rlocn)   GROUP BY ICODE )Z WHERE TRIM(X.ICODE)=TRIM(Z.icode) and TRIM(substr(X.ICODE,0,4))=TRIM(y.ICODE) group by trim(X.icode),trim(Y.ICODE),trim(Y.INAME),X.PUR_UOM,round((X.OPRATE1),1),X.OPRATE3,x.bfactor,x.iname,X.BINNO   order by icode,rsize,gsm";

                    SQuery = "select * from (" + mq1 + " union all " + mq2 + " ) order by icode";
                    //SQuery = "select * from ( " + SQuery + " ) where trim(icode)='07130149' ";

                    //Session["squery"] = null;
                    //dt1 = fgen.getdata(co_cd, SQuery);
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt1.Columns.Add("purpose_for1", typeof(string));
                    dt1.Columns.Add("DECLE1", typeof(string));
                    dt1.Columns.Add("MEASURE1", typeof(string));
                    dt1.Columns.Add("PLY1", typeof(string));
                    dt1.Columns.Add("UPS1", typeof(string));
                    dt1.Columns.Add("FLUTE1", typeof(string));
                    dt1.Columns.Add("carton_qty1", typeof(string));
                    dt1.Columns.Add("purpose_for2", typeof(string));
                    dt1.Columns.Add("DECLE2", typeof(string));
                    dt1.Columns.Add("MEASURE2", typeof(string));
                    dt1.Columns.Add("PLY2", typeof(string));
                    dt1.Columns.Add("UPS2", typeof(string));
                    dt1.Columns.Add("FLUTE2", typeof(string));
                    dt1.Columns.Add("carton_qty2", typeof(string));
                    dt1.Columns.Add("purpose_for3", typeof(string));
                    dt1.Columns.Add("DECLE3", typeof(string));
                    dt1.Columns.Add("MEASURE3", typeof(string));
                    dt1.Columns.Add("PLY3", typeof(string));
                    dt1.Columns.Add("UPS3", typeof(string));
                    dt1.Columns.Add("FLUTE3", typeof(string));
                    dt1.Columns.Add("carton_qty3", typeof(string));
                    dt1.Columns.Add("purpose_for4", typeof(string));
                    dt1.Columns.Add("DECLE4", typeof(string));
                    dt1.Columns.Add("MEASURE4", typeof(string));
                    dt1.Columns.Add("PLY4", typeof(string));
                    dt1.Columns.Add("UPS4", typeof(string));
                    dt1.Columns.Add("FLUTE4", typeof(string));
                    dt1.Columns.Add("carton_qty4", typeof(string));

                    DataTable dtm2 = new DataTable();
                    dtm2 = dt1.Clone();

                    //mq6 = "SELECT * FROM (SELECT DISTINCT A.MCODE AS ICODE, c.iname as purpose_for,B.MAINTDT AS DECLE,B.BTCHDT AS MEASURE,B.COL15 AS PLY,B.REJQTY AS UPS ,B.GRADE AS B_F ,B.BTCHDT,B.REJQTY  FROM (SELECT DISTINCT TRIM(CCENT) AS ICODE,ICODE AS MCODE  FROM (SELECT DISTINCT VCHNUM,VCHDATE,trim(CCENT)AS CCENT ,ICODE FROM ivoucher where " + branch_Cd + " and type like'0%' and vchnum like '%' and vchdate >sysdate-500  AND TRIM(CCENT)<>'-' ORDER BY VCHDATE DESC) ) A,INSPMST B,item c  WHERE b." + branch_Cd + " and B.TYPE='70' and b.vchnum like '%' and b.vchdate >sysdate-500 AND trim(b.icode)=trim(c.icode) and  TRIM(A.ICODE)=TRIM(B.ICODE))";
                    // CHNAGE vchdate >sysdate-500 TO vchdate >sysdate-1000 BY MADHVI ON 10 APR 2018 S
                    mq6 = "SELECT * FROM (SELECT DISTINCT A.MCODE AS ICODE, NVL(c.NO_PROC,'-') as purpose_for,B.MAINTDT AS DECLE,B.BTCHDT AS MEASURE,B.COL15 AS PLY,B.REJQTY AS UPS ,B.GRADE AS B_F ,B.BTCHDT,B.REJQTY  FROM (SELECT DISTINCT TRIM(CCENT) AS ICODE,ICODE AS MCODE  FROM (SELECT DISTINCT VCHNUM,VCHDATE,trim(CCENT)AS CCENT ,ICODE FROM ivoucher where " + branch_Cd + " and type like'0%' and vchnum like '%' and vchdate >sysdate-1000  AND TRIM(CCENT)<>'-' ORDER BY VCHDATE DESC) ) A,INSPMST B,item c  WHERE b." + branch_Cd + " and B.TYPE='70' and b.vchnum like '%'  AND trim(b.icode)=trim(c.icode) and  TRIM(A.ICODE)=TRIM(B.ICODE))";
                    DataTable dtjob = new DataTable();
                    dtjob = fgen.getdata(frm_qstr, co_cd, mq6);

                    mq7 = "SELECT DISTINCT ordno,orddt,trim(ICODE) as icode,splrmk as purpose_for,TO_CHAR(ORDDT,'YYYYMMDD') AS VDD FROM pomas where " + branch_Cd + " and type like'5%' and ordno like '%' and orddt >sysdate-1000 and nvl(splrmk,'-')!='-' ORDER BY VDD DESC";
                    dtdrsim = new DataTable();
                    dtdrsim = fgen.getdata(frm_qstr, co_cd, mq7);

                    foreach (DataRow dr in dt1.Rows)
                    {
                        DataRow drm1 = dtm2.NewRow();
                        foreach (DataColumn dc1 in dtm2.Columns)
                        {
                            drm1[dc1.ColumnName] = dr[dc1.ColumnName];
                        }

                        mq1 = dr["icode"].ToString().Trim();
                        if (dr["icode"].ToString().Trim() == "07100115")
                        {
                            int a = 0;
                        }
                        //mq6 = "SELECT DISTINCT c.iname as purpose_for,B.MAINTDT AS DECLE,B.BTCHDT AS MEASURE,B.COL15 AS PLY,B.REJQTY AS UPS,b.BTCHDT ,B.GRADE AS B_F ,round( " + dr["Clos_qty"] + "/ ((" + dr["rsize"] + "*B.BTCHDT*" + dr["gsm"] + ")/10000000)*B.REJQTY,2) as carton_qty  FROM (SELECT DISTINCT TRIM(DESC_) AS ICODE  FROM (SELECT DISTINCT VCHNUM,VCHDATE,DESC_ FROM ivoucher where " + branch_Cd + " and type like'0%' and vchnum like '%' and vchdate >sysdate-500 AND TRIM(ICODE)='" + dr["icode"].ToString().Trim() + "' AND TRIM(DESC_)<>'-' ORDER BY VCHDATE DESC)  WHERE ROWNUM<4) A,INSPMST B,item c  WHERE b." + branch_Cd + " and B.TYPE='70' and b.vchnum like '%' and b.vchdate >sysdate-500 AND trim(b.icode)=trim(c.icode) and  TRIM(A.ICODE)=TRIM(B.ICODE)";
                        //mq6 = "SELECT DISTINCT c.iname as purpose_for,B.MAINTDT AS DECLE,B.BTCHDT AS MEASURE,B.COL15 AS PLY,B.REJQTY AS UPS ,B.GRADE AS B_F  FROM (SELECT DISTINCT TRIM(DESC_) AS ICODE  FROM (SELECT DISTINCT VCHNUM,VCHDATE,DESC_ FROM ivoucher  where  type like'0%' AND TRIM(ICODE)='" + dr["icode"].ToString().Trim() + "' AND TRIM(DESC_)<>'-' ORDER BY VCHDATE DESC)  WHERE ROWNUM<4) A,INSPMST B,item c  WHERE B.TYPE='70' AND trim(b.icode)=trim(c.icode) and  TRIM(A.ICODE)=TRIM(B.ICODE)";

                        //   dt = fgen.getdata(co_cd, mq6);
                        if (mq1.Trim() == "-") { dt = null; }
                        else
                        {
                            dt = fgen.searchDataTable(mq1, dtjob);
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            try
                            {
                                if (dtdrsim.Rows.Count > 0)
                                {
                                    drm1["purpose_for" + (i + 1).ToString()] = fgen.seek_iname_dt(dtdrsim, "icode='" + mq1.Trim() + "'", "purpose_for");
                                }
                                if (drm1["purpose_for" + (i + 1).ToString()].ToString().Length == 1)
                                {
                                    drm1["purpose_for" + (i + 1).ToString()] = "-";
                                }
                            }
                            catch { }
                            try
                            {
                                // ORIGINAL .... COMMENTED ON 26 APRIL 2018 BY MADHVI AS USER WANTS THE VALUE IRRESPECTIVE OF PROCESS PLAN
                                //drm1["purpose_for" + (i + 1).ToString()] = dt.Rows[i]["purpose_for"].ToString();
                                drm1["DECLE" + (i + 1).ToString()] = dt.Rows[i]["DECLE"].ToString();
                                drm1["MEASURE" + (i + 1).ToString()] = dt.Rows[i]["MEASURE"].ToString();
                                drm1["PLY" + (i + 1).ToString()] = dt.Rows[i]["PLY"].ToString();
                                drm1["UPS" + (i + 1).ToString()] = dt.Rows[i]["UPS"].ToString();
                                drm1["FLUTE" + (i + 1).ToString()] = dt.Rows[i]["B_F"].ToString();
                                // drm1["carton_qty" + (i + 1).ToString()] = dt.Rows[i]["carton_qty"].ToString();

                                Double closqty = fgen.make_double(dr["Clos_qty"].ToString());
                                Double rsize = fgen.make_double(dr["rsize"].ToString());
                                Double btchdt = fgen.make_double(dt.Rows[i]["BTCHDT"].ToString());
                                Double gsm = fgen.make_double(dr["gsm"].ToString());
                                Double rejqty = fgen.make_double(dt.Rows[i]["REJQTY"].ToString());
                                drm1["carton_qty" + (i + 1).ToString()] = Math.Round(closqty / ((rsize * btchdt * gsm) / 10000000) * rejqty, 2);
                            }
                            catch
                            {
                                //  drm1["purpose_for" + (i + 1).ToString()] = "-";
                                drm1["DECLE" + (i + 1).ToString()] = "-";
                                drm1["MEASURE" + (i + 1).ToString()] = "-";
                                drm1["PLY" + (i + 1).ToString()] = "-";
                                drm1["UPS" + (i + 1).ToString()] = "-";
                                drm1["FLUTE" + (i + 1).ToString()] = "-";
                                drm1["carton_qty" + (i + 1).ToString()] = "-";
                            }
                        }
                        dtm2.Rows.Add(drm1);
                    }
                    Session["send_dt"] = dtm2;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Daily Stock Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    #endregion
                    break;

                case "15250I":// wfinsys_erp id
                case "F40310":  // comma done
                    #region Party Wise Purchase Qty
                    if (hfcode.Value == "") mq0 = "";
                    else mq0 = "and SUBSTR(TRIM(A.ICODE),1,2) in (" + hfcode.Value + ")";
                    //fgen.send_cookie("mq0", "Result");
                    SQuery = "SELECT A.ACODE,B.ANAME,to_char(SUM(A.IQTYIN),'999,999,999.99') AS QTYIN,to_char(round(sum(iamount)/SUM(A.IQTYIN),2),'999,999,999.99') as avg_rate,to_char(sum(iamount),'999,999,999.99') as Value FROM IVOUCHER A ,FAMST B WHERE A." + branch_Cd + "  AND A.TYPE LIKE '0%' AND A.VCHDATE " + xprdrange + "  AND TRIM(A.ACODE)=TRIM(B.ACODE) " + mq0 + "  GROUP BY A.ACODE,B.ANAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Party Wise Purchase For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15250J":// wfinsys_erp id // RPTLEVEL HD
                case "F40311": //COMMA DONE
                    #region Corrugation Order Vs Prduction Balance
                    SQuery = "";
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    xprd1 = "between to_date('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    SQuery = "CREATE OR REPLACE FORCE VIEW PENDING_SO_VU as (select branchcd,type,max(Closed) as closed,max(ciname) as ciname,max(cpartno) as cpartno,max(pordno) as pordno,max(porddt) porddt,acode,icode,ordno,orddt,sum(qtyord) as qtyord,sum(sale) as qty_out,sum(qtyord)-sum(sale) as bal from (select branchcd, type,cu_chldt,icat as Closed,ciname,cpartno,pordno,porddt,acode,icode,ordno,orddt,qtyord,0 as sale from somas where " + branch_Cd + " and type like '4%' and orddt " + xprd1 + "  union all select branchcd ,type,null as cu_chldt,null as icat,null as ciname,null as cpartno,null as pordno,null as porddt,acode,icode,ponum,podate,0 as qtyord,iqtyout as sale from ivoucher where " + branch_Cd + " and type like '4%' and vchdate " + xprd1 + ")group by BRANCHCD,type,acode,icode,ordno,orddt)";

                    fgen.execute_cmd(frm_qstr, co_cd, SQuery);

                    if (hfcode.Value == "")
                    {
                        mq0 = "and a.type like '4%'";
                    }
                    else
                    {
                        mq0 = "and a.type in(" + hfcode.Value + ")";
                    }

                    if (hf1.Value == "Y") mq3 = " and trim(d.ICAT)='N'";
                    else mq3 = "";

                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    xprd1 = "between to_date('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    mq5 = "SELECT A.ICODE,A.INAME ,B.BRANCHCD,B.PLY_SIZE,B.DECKLE,B.MEASURES,B.UPS,b.ply FROM ITEM A LEFT OUTER JOIN (SELECT DISTINCT  BRANCHCD,ICODE,col15 as ply, COL16 AS PLY_SIZE,MAINTDT AS DECKLE,BTCHDT AS MEASURES ,REJQTY AS UPS FROM INSPMST WHERE TYPE='70' AND BRANCHCD<>'DD') B ON TRIM(A.ICODE)=TRIM(B.ICODE)";
                    mq6 = "select A.BRANCHCD,max(A.job_no) AS JOB_NO,max(A.job_dt) AS JOB_DT,max(a.ACODE) as acode,A.ICODE AS ICODE,sum(QTYORD) as qtyord,SUM(PRDQTY) AS QTY_OUT,sum(QTYORD)-SUM(PRDQTY) AS BAL ,SONUM AS ORDNO,TO_DATE(SODATE,'DD/MM/YYYY') AS ORDDT,SOTYPE AS TYPE from (SELECT A.BRANCHCD,A.ENQNO AS JOB_NO,A.ENQDT AS JOB_DT,null as ACODE,trim(A.ICODE) AS ICODE,0 AS QTYORD,SUM(PRDQTY) AS PRDQTY ,MAX(SONUM) AS SONUM,MAX(SODATE) AS SODATE,MAX(SOTYPE) AS SOTYPE FROM (SELECT A.BRANCHCD,A.ENQNO,A.ENQDT,TRIM(A.ICODE) AS ICODE,SUM(A.QTY) AS PRDQTY,NULL AS SONUM,NULL AS SODATE,NULL AS SOTYPE  FROM COSTESTIMATE  A WHERE  A." + branch_Cd + " and type='40' AND VCHDATE " + xprdrange + " GROUP BY  A.BRANCHCD,A.ENQNO,A.ENQDT,TRIM(A.ICODE) UNION ALL SELECT DISTINCT BRANCHCD,VCHNUM,VCHDATE AS VCHDATE,TRIM(ICODE) AS ICODE,0 AS PRDQTY, SUBSTR(TRIM(CONVDATE),5,6) AS SONUM,SUBSTR(TRIM(CONVDATE),11,10) AS SODATE,SUBSTR(TRIM(CONVDATE),3,2) AS SOTYPE  FROM COSTESTIMATE A WHERE " + branch_Cd + " AND TYPE='30' AND VCHDATE " + xprdrange + "  )  A  GROUP BY A.BRANCHCD,A.ENQNO,A.ENQDT,A.ICODE UNION ALL SELECT BRANCHCD,NULL AS JOB_NO,NULL AS JOB_DT,ACODE,trim(ICODE) as icode,SUM(QTYORD) AS QTYORD ,0 AS PRDQTY ,ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY') AS ORDDT,TYPE FROM SOMAS a WHERE " + branch_Cd + " " + mq0 + "  " + mq3.Replace("d.", "a.") + " AND ORDDT " + xprdrange + "  GROUP BY BRANCHCD,ACODE,ICODE,ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY'),TYPE) A GROUP BY A.BRANCHCD,A.ICODE,A.SONUM,A.SODATE,A.SOTYPE";
                    //SQuery = "select * from (select null as Customer,null AS Ordno,null as Ord_Dt,null as Cust_POrd,null as Cust_POrddt,null AS job_no, null AS job_dt,null AS type,null as ERP_code,null as Part_Number,null as Item_Name,null AS unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,null AS deckle,null AS measures,round((sum(a.qtyord)-sum(a.qty_out))/sum(e.ups),0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/sum(e.ups),0)*sum(e.measures))/100,0) as Required_Mtrs,null as Cust_Dlv_Dt,null as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 UNION ALL select * from (select B.Aname as Customer,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.job_no,a.job_dt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by  B.Aname,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy'),d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,a.job_no,a.job_dt order by orddtc ) )";
                    //SQuery = "select * from (select null as Customer,null AS Ordno,null as Ord_Dt,null as Cust_POrd,null as Cust_POrddt,null AS job_no, null AS job_dt,null AS type,null as ERP_code,null as Part_Number,null as Item_Name,null AS unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,null AS deckle,null AS measures,round((sum(a.qtyord)-sum(a.qty_out))/sum(e.ups),0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/sum(e.ups),0)*sum(e.measures))/100,0) as Required_Mtrs,null as Cust_Dlv_Dt,null as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) " + mq3 + " and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 UNION ALL select * from (select B.Aname as Customer,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.job_no,a.job_dt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) " + mq3 + " and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by  B.Aname,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy'),d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,a.job_no,a.job_dt order by orddtc ) )";

                    //SQuery = "select '-' Customer,'-' Ordno,NULL Ord_Dt,'-' Cust_POrd, NULL Cust_POrddt,'-' job_no,NULL  job_dt,'-' type,'-' ERP_code,'-' Part_Number,'-' Item_Name,'-' unit,sum(Qtyord) as Qtyord,sum(bal) as bal, NULL deckle,NULL measures,sum(REQUIRED_CUT) as REQUIRED_CUT,sum(Required_Mtrs) as Required_Mtrs,NULL Cust_Dlv_Dt,'-' orddtc from (select A.* from (select d.branchcd ,B.Aname as Customer,a.Ordno,a.Orddt as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.job_no,a.job_dt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) " + mq3 + " and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by d.branchcd, B.Aname,a.Ordno,a.Orddt,d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,a.job_no,a.job_dt order by orddtc ) a , pending_so_vu b where trim(a.branchcd)=trim(b.branchcd) and a.type=b.type and  trim(a.ordno)=trim(b.ordno) and trim(a.ord_dt)=trim(b.orddt) and trim(a.ERP_CODE)=trim(b.icode) and trim(A.qtyord)=trim(b.qtyord)  and NVL(b.bal,0)>0 order by A.ordno) union all select Customer,Ordno,to_char(Ord_Dt,'DD/MM/YYYY') AS Ord_Dt,Cust_POrd,Cust_POrddt,job_no,TO_char(job_dt,'DD/MM/YYYY') AS job_dt,type,ERP_code,Part_Number,Item_Name,unit,Qtyord, bal,deckle,measures,REQUIRED_CUT,Required_Mtrs, Cust_Dlv_Dt,orddtc from (select A.* from (select d.branchcd ,B.Aname as Customer,a.Ordno,a.Orddt as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.job_no,a.job_dt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) " + mq3 + " and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by d.branchcd, B.Aname,a.Ordno,a.Orddt,d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,a.job_no,a.job_dt order by orddtc ) a , pending_so_vu b where trim(a.branchcd)=trim(b.branchcd) and a.type=b.type and  trim(a.ordno)=trim(b.ordno) and trim(a.ord_dt)=trim(b.orddt) and trim(a.ERP_CODE)=trim(b.icode) and trim(A.qtyord)=trim(b.qtyord)  and NVL(b.bal,0)>0 order by A.ordno)";
                    SQuery = "select '-' Customer,'-' Ordno,NULL Ord_Dt,'-' Cust_POrd, NULL Cust_POrddt,'-' job_no,NULL  job_dt,'-' type,'-' ERP_code,'-' Part_Number,'-' Item_Name,'-' unit,to_char(sum(Qtyord),'999,999,999.99') as Qtyord,to_char(sum(bal),'999,999,999.99') as bal, NULL deckle,NULL measures,to_char(sum(REQUIRED_CUT),'999,999,999.99') as REQUIRED_CUT,to_char(sum(Required_Mtrs),'999,999,999.99') as Required_Mtrs,NULL Cust_Dlv_Dt,'-' orddtc from (select A.* from (select d.branchcd ,B.Aname as Customer,a.Ordno,a.Orddt as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.job_no,a.job_dt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) " + mq3 + " and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by d.branchcd, B.Aname,a.Ordno,a.Orddt,d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,a.job_no,a.job_dt order by orddtc ) a , pending_so_vu b where trim(a.branchcd)=trim(b.branchcd) and a.type=b.type and  trim(a.ordno)=trim(b.ordno) and trim(a.ord_dt)=trim(b.orddt) and trim(a.ERP_CODE)=trim(b.icode) and trim(A.qtyord)=trim(b.qtyord)  and NVL(b.bal,0)>0 order by A.ordno) union all select Customer,Ordno,to_char(Ord_Dt,'DD/MM/YYYY') AS Ord_Dt,Cust_POrd,Cust_POrddt,job_no,TO_char(job_dt,'DD/MM/YYYY') AS job_dt,type,ERP_code,Part_Number,Item_Name,unit,to_char(Qtyord,'999,999,999.99') as Qtyord, to_char(bal,'999,999,999.99') as bal,deckle,measures,to_char(REQUIRED_CUT,'999,999,999.99') as REQUIRED_CUT ,to_char(Required_Mtrs,'999,999,999.99') AS Required_Mtrs , Cust_Dlv_Dt,orddtc from (select A.* from (select d.branchcd ,B.Aname as Customer,a.Ordno,a.Orddt as Ord_Dt,d.pordno as Cust_POrd,to_char(d.porddt,'dd/mm/yyyy') as Cust_POrddt,a.job_no,a.job_dt,a.type,A.Icode as ERP_code,nvl(C.Cpartno,'-') as Part_Number,C.iname as Item_Name,c.unit,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,e.deckle,e.measures,round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0) as REQUIRED_CUT,round((round((sum(a.qtyord)-sum(a.qty_out))/e.ups,0)*e.measures)/100,0) as Required_Mtrs,to_char(d.cu_chldt,'dd/mm/yyyy')as Cust_Dlv_Dt,to_char(a.orddt,'yyyymmdd') as orddtc from (" + mq6 + ") a,famst b,item c,somas d,(" + mq5 + ") e  where trim(a.icode)=trim(e.icode) and  trim(a.ordno)=trim(d.ordno) and trim(a.icode)=trim(d.icode) and trim(a.branchcd)=trim(d.branchcd) and trim(a.orddt)=trim(d.orddt) and trim(a.type)=trim(d.type) and  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) " + mq3 + " and a." + branch_Cd + " " + mq0 + " and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by d.branchcd, B.Aname,a.Ordno,a.Orddt,d.pordno,to_char(d.porddt,'dd/mm/yyyy') ,A.Icode,nvl(C.Cpartno,'-'),C.iname,c.unit,e.deckle,e.measures,e.ups,to_char(d.cu_chldt,'dd/mm/yyyy'),to_char(a.orddt,'yyyymmdd'),a.type,a.job_no,a.job_dt order by orddtc ) a , pending_so_vu b where trim(a.branchcd)=trim(b.branchcd) and a.type=b.type and  trim(a.ordno)=trim(b.ordno) and trim(a.ord_dt)=trim(b.orddt) and trim(a.ERP_CODE)=trim(b.icode) and trim(A.qtyord)=trim(b.qtyord)  and NVL(b.bal,0)>0 order by A.ordno)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.open_rptlevel_hd("Corrugation Order Vs Production Balance for the period " + fromdt + " and " + todt + "");
                    fgen.Fn_open_rptlevel("Corrugation Order Vs Prduction Balance For the period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15163A":// wfinsys_erp id // RPTLEVEL HD
                case "F40312": // done 
                    #region Corr.Reason Monthly Downtime Report
                    inspvchtab = "INSPVCH";
                    if (co_cd == "SKYP")
                    {
                        inspvchtab = "INSPVCHK";
                    }
                    // fgen.execute_cmd(co_cd, " CREATE or REPLACE FUNCTION if not exist is_number (p_string IN VARCHAR2) RETURN NUMBER IS v_new_num NUMBER;BEGIN v_new_num := TO_NUMBER(p_string); RETURN p_string; EXCEPTION WHEN VALUE_ERROR THEN  RETURN 0; END is_number;");
                    //fgen.execute_cmd(co_cd, "commit");
                    SQuery = "SELECT VCHDATE,MACHINE_NAME,REJECTION AS REASON,REJ_CODE AS REASON_CODE,SUM(IS_NUMBER(DOWNTIME)) AS DOWNTIME FROM (SELECT DISTINCT BRANCHCD,TO_CHAR(VCHDATE,'MM/YYYY') AS VCHDATE,TITLE AS Machine_Name,COL1 as Rejection,COL2 as Rej_Code ,COL3 as Downtime FROM " + inspvchtab + " WHERE " + branch_Cd + " and  TYPE='55' and  vchdate " + xprdrange + " ) GROUP BY VCHDATE,MACHINE_NAME,REJECTION,REJ_CODE";
                    mq0 = "select distinct type1 from typewip where id='DTC61' /*AND BRANCHCD='" + mbr + "'*/ ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();

                    dtm1.Columns.Add("Month", typeof(string));
                    dtm1.Columns.Add("Machine", typeof(string));
                    dtm1.Columns.Add("Total", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    mq1 = "SELECT SUM(DOWNTIME) AS DOWNTIME ,MACHINE,REASON_CODE,MONTH,VCHDATEORD FROM(SELECT VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE FROM (SELECT BRANCHCD,TYPE,VCHNUM,TO_CHAR(VCHDATE,'YYYYMM') AS VCHDATEORD,TO_CHAR(VCHDATE,'Month-YYYY') AS VCHDATE,TITLE AS Machine_Name,COL2 AS REASON_CODE,is_number(COL3) as DOWNTIME FROM " + inspvchtab + " WHERE  " + branch_Cd + " and TYPE='55' and vchdate " + xprdrange + " ORDER BY VCHDATEORD)) GROUP BY MACHINE,REASON_CODE,MONTH,VCHDATEORD ORDER BY VCHDATEORD";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    view1im = new DataView(dt);
                    dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "MONTH", "MACHINE");
                    iqtyout_sum = Convert.ToDouble(dt.Compute("Sum ( DOWNTIME ) ", ""));

                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataRow drrow1 = dtm1.NewRow();
                        DataView viewim = new DataView(dt, "MONTH='" + dr0["MONTH"] + "' and MACHINE='" + dr0["MACHINE"] + "'", "", DataViewRowState.CurrentRows);
                        dt1 = viewim.ToTable();
                        double tot = 0;

                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                            try
                            {
                                drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                            }
                            catch { }
                        }
                        drrow1["MONTH"] = dt1.Rows[0]["MONTH"];
                        drrow1["MACHINE"] = dt1.Rows[0]["MACHINE"];
                        drrow1["TOTAL"] = tot.ToString("###,###,###");
                        dtm1.Rows.Add(drrow1);
                    }

                    dr2 = dtm1.NewRow();
                    d = 0;

                    foreach (DataColumn dc in dtm1.Columns)
                    {
                        double total = 0;
                        if (dc.Ordinal == 0 || dc.Ordinal == 1) { }
                        else
                        {
                            foreach (DataRow drrr in dtm1.Rows)
                            {
                                total += fgen.make_double(drrr[dc.ToString()].ToString());
                            }
                            string check = total.ToString("###,###,###,###");
                            dr2[dc] = check;
                            //dr2[dc] = total;
                        }
                    }

                    dr2["MONTH"] = '-';
                    dr2["MACHINE"] = '-';

                    dtm1.Rows.InsertAt(dr2, 0);
                    //    dt1 = fgen.getdata(co_cd, SQuery);
                    mq0 = "select distinct type1,name from typewip where id='DTC61' /*and branchcd='" + mbr + "'*/";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    foreach (DataColumn dc in dtm1.Columns)
                    {
                        int abc = dc.Ordinal;
                        string name = dc.ToString().Remove(0, 1);
                        string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                        if (myname != "0")
                        {
                            dtm1.Columns[abc].ColumnName = myname;
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr.Reason Monthly Downtime Report (Data is in Minutes) For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "15163C":// wfinsys_erp id // RPTLEVEL HD
                case "F40313": //DONE 
                    #region Corr.Reason Yearly Downtime Report
                    //SQuery = "SELECT '-' as MACHINE_NAME,'-' AS REASON_NAME,SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(A.M01)+SUM(A.M02)+SUM(A.M03) AS TOTAL_TIME,SUM(A.M04) AS M04,SUM(A.M05) AS M05,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03 FROM (SELECT BRANCHCD,DECODE(TO_CHAR(VCHDATE,'MM'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'MM'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'MM'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'MM'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'MM'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'MM'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'MM'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'MM'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'MM'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'MM'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'MM'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'MM'),'03', is_number(COL3),0) as M03,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE  " + branch_Cd + " AND TYPE='55'  and VCHDATE " + xprdrange + ") A,TYPEWIP B WHERE B.id='DTC61' and B.BRANCHCD='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1) union all SELECT A.MACHINE_NAME AS MACHINE,B.NAME AS REASON_NAME,SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(A.M01)+SUM(A.M02)+SUM(A.M03) AS TOTAL_TIME,SUM(A.M04) AS M04,SUM(A.M05) AS M045,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03 FROM (SELECT BRANCHCD,DECODE(TO_CHAR(VCHDATE,'MM'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'MM'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'MM'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'MM'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'MM'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'MM'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'MM'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'MM'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'MM'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'MM'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'MM'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'MM'),'03', is_number(COL3),0) as M03,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE  " + branch_Cd + " AND TYPE='55' and VCHDATE " + xprdrange + ") A,TYPEWIP B WHERE B.id='DTC61' AND B.branchcd='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1)  GROUP BY A.MACHINE_NAME,B.NAME";
                    SQuery = "SELECT '-' as MACHINE_NAME,'-' AS REASON_NAME,SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(A.M01)+SUM(A.M02)+SUM(A.M03) AS TOTAL_TIME, SUM(A.M04) AS M04,SUM(A.M05) AS M05,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03 FROM (SELECT BRANCHCD,DECODE(TO_CHAR(VCHDATE,'MM'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'MM'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'MM'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'MM'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'MM'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'MM'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'MM'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'MM'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'MM'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'MM'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'MM'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'MM'),'03', is_number(COL3),0) as M03,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE  " + branch_Cd + " AND TYPE='55'  and VCHDATE " + xprdrange + ") A,TYPEWIP B WHERE B.id='DTC61' and B.BRANCHCD='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1) union all SELECT A.MACHINE_NAME AS MACHINE,B.NAME AS REASON_NAME,SUM(A.M04)+SUM(A.M05)+SUM(A.M06)+SUM(A.M07)+SUM(A.M08)+SUM(A.M09)+SUM(A.M10)+SUM(A.M11)+SUM(A.M12)+SUM(A.M01)+SUM(A.M02)+SUM(A.M03) AS TOTAL_TIME,SUM(A.M04) AS M04,SUM(A.M05) AS M045,SUM(A.M06) AS M06,SUM(A.M07) AS M07,SUM(A.M08) AS M08,SUM(A.M09) AS M09,SUM(A.M10) AS M10,SUM(A.M11) AS M11,SUM(A.M12) AS M12,SUM(A.M01) AS M01,SUM(A.M02) AS M02,SUM(A.M03) AS M03 FROM (SELECT BRANCHCD,DECODE(TO_CHAR(VCHDATE,'MM'),'04', is_number(COL3),0) as M04,DECODE(TO_CHAR(VCHDATE,'MM'),'05', is_number(COL3),0) as M05,DECODE(TO_CHAR(VCHDATE,'MM'),'06', is_number(COL3),0) as M06,DECODE(TO_CHAR(VCHDATE,'MM'),'07', is_number(COL3),0) as M07,DECODE(TO_CHAR(VCHDATE,'MM'),'08', is_number(COL3),0) as M08,DECODE(TO_CHAR(VCHDATE,'MM'),'09', is_number(COL3),0) as M09,DECODE(TO_CHAR(VCHDATE,'MM'),'10', is_number(COL3),0) as M10,DECODE(TO_CHAR(VCHDATE,'MM'),'11', is_number(COL3),0) as M11,DECODE(TO_CHAR(VCHDATE,'MM'),'12', is_number(COL3),0) as M12,DECODE(TO_CHAR(VCHDATE,'MM'),'01', is_number(COL3),0) as M01,DECODE(TO_CHAR(VCHDATE,'MM'),'02', is_number(COL3),0) as M02,DECODE(TO_CHAR(VCHDATE,'MM'),'03', is_number(COL3),0) as M03,TITLE AS Machine_Name,COL2 AS REASON_CODE FROM INSPVCH WHERE  " + branch_Cd + " AND TYPE='55' and VCHDATE " + xprdrange + ") A,TYPEWIP B WHERE B.id='DTC61' AND B.branchcd='" + mbr + "' AND TRIM(A.REASON_CODE)=TRIM(B.TYPE1)  GROUP BY A.MACHINE_NAME,B.NAME";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, "select mthnum,mthname from mths");

                    foreach (DataColumn dc in dt.Columns)
                    {
                        int abc = dc.Ordinal;
                        string name = dc.ToString().Remove(0, 1);
                        string myname = fgen.seek_iname_dt(dt1, "mthnum='" + name + "'", "mthname");
                        if (myname != "0")
                        {
                            dt.Columns[abc].ColumnName = myname;
                        }
                    }
                    Session["send_dt"] = dt;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr.Reason Yearly Downtime Report (Data is in Minutes) For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    //fgen.open_rptlevel_hd("Corr.Reason Yearly Downtime Report between " + fromdt + " and " + todt + " ");
                    #endregion
                    break;

                case "15163L":// wfinsys_erp id  // RPTLEVEL HD
                case "F40315":  // comma done
                    #region Corr. Job Wise Rejection Reason Report
                    inspvchtab = "INSPVCH";
                    if (co_cd == "SKYP")
                    {
                        inspvchtab = "INSPVCHK";
                    }
                    mq0 = "select distinct type1 from typewip where id='RJC61' and branchcd='" + mbr + "' ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("ENTRY_NO", typeof(string));
                    dtm1.Columns.Add("VCHDATE", typeof(string));
                    dtm1.Columns.Add("JOB_NO", typeof(string));
                    dtm1.Columns.Add("JOB_DATE", typeof(string));
                    dtm1.Columns.Add("ICODE", typeof(string));
                    dtm1.Columns.Add("INAME", typeof(string));
                    dtm1.Columns.Add("Machine", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));
                    dtm1.Columns.Add("Total_Nos", typeof(string));
                    dtm1.Columns.Add("Total_WT", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    dtm1.Columns.Add("OK_QTY", typeof(string));
                    dtm1.Columns.Add("Tot_Rej", typeof(string));
                    dtm1.Columns.Add("Rej_Perc", typeof(string));

                    mq1 = "SELECT A.ICODE,B.INAME,B.boxWT*SUM(A.DOWNTIME) AS DOWNTIME_WT,A.VCHNUM AS ENTRY_NO,SUM(A.DOWNTIME) AS DOWNTIME ,A.MACHINE,A.REASON_CODE,A.MONTH AS VCHDATE,A.VCHDATEORD,A.ENQNO AS JOB_NO,A.ENQDT AS JOB_DATE,TO_CHAR(sum(A.prdqty),'999,999,999,999') as prdqty,TO_CHAR(MAX(b.JOB_QTY),'999,999,999,999') AS JOB_QTY FROM (SELECT DISTINCT BRANCHCD, VCHNUM,ICODE,VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE,ENQNO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS ENQDT,prdqty FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATEORD,VCHDATE,MAX(Machine_Name) AS Machine_Name,REASON_CODE AS REASON_CODE,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt,max(prdqty) as prdqty from (SELECT A.BRANCHCD,A.ICODE,A.VCHNUM,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VCHDATEORD,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.TITLE AS Machine_Name,A.COL2 AS REASON_CODE,is_number(A.COL3) as DOWNTIME,B.ENQNO,B.ENQDT,B.prdqty FROM " + inspvchtab + "  A,( SELECT DISTINCT  BRANCHCD,ICODE,VCHNUM,VCHDATE,ENQNO,ENQDT,sum(qty) as prdqty FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " group BY BRANCHCD,ICODE,VCHNUM,VCHDATE,ENQNO,ENQDT) B WHERE TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND A." + branch_Cd + " and A.TYPE='45' and  A.vchdate " + xprdrange + "  ) group by REASON_CODE,BRANCHCD,ICODE,VCHNUM,VCHDATEORD,VCHDATE) WHERE ENQNO IS NOT NULL ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,a.irate as boxWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " ) B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ENQNO)=TRIM(B.VCHNUM) AND TRIM(A.ENQDT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.MACHINE,A.REASON_CODE,A.MONTH,A.VCHDATEORD,B.INAME,B.boxWT,A.VCHNUM,A.ICODE,A.ENQNO,A.ENQDT  ORDER BY A.VCHDATEORD,JOB_NO";
                    // mq1 = "select * from (" + mq1 + ") where job_no='000401'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "ENTRY_NO", "VCHDATE", "JOB_NO", "JOB_DATE", "MACHINE", "ICODE", "INAME");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "VCHDATE='" + dr0["VCHDATE"] + "' AND ENTRY_NO='" + dr0["ENTRY_NO"] + "' AND ICODE='" + dr0["ICODE"] + "' AND INAME='" + dr0["INAME"] + "' AND JOB_NO='" + dr0["JOB_NO"] + "' AND JOB_DATE='" + dr0["JOB_DATE"] + "' and MACHINE='" + dr0["MACHINE"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double prdtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());

                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                    prdtot = prdtot + fgen.make_double(dt1.Rows[i]["prdqty"].ToString());
                                }
                                catch { }
                            }
                            drrow1["TOTAL_Nos"] = tot.ToString("###,###,###.##");
                            drrow1["Ok_qty"] = prdtot.ToString("###,###,###.##");
                            drrow1["Tot_Rej"] = tot.ToString("###,###,###.##");
                            drrow1["Rej_perc"] = Math.Round(tot / (prdtot + tot) * 100, 2);

                            tot = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME_WT"].ToString());

                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME_WT"].ToString());
                                }
                                catch { }
                            }

                            drrow1["ENTRY_NO"] = dt1.Rows[0]["ENTRY_NO"];
                            drrow1["VCHDATE"] = dt1.Rows[0]["VCHDATE"];
                            drrow1["ICODE"] = dt1.Rows[0]["ICODE"];
                            drrow1["INAME"] = dt1.Rows[0]["INAME"];
                            drrow1["JOB_NO"] = dt1.Rows[0]["JOB_NO"];
                            drrow1["JOB_DATE"] = dt1.Rows[0]["JOB_DATE"];
                            drrow1["MACHINE"] = dt1.Rows[0]["MACHINE"];
                            drrow1["Job_Qty"] = dt1.Rows[0]["JOB_QTY"];
                            drrow1["TOTAL_WT"] = tot.ToString("###,###,###.##");
                            dtm1.Rows.Add(drrow1);
                        }

                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());

                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        dr2["JOB_DATE"] = '-';
                        dr2["MACHINE"] = '-';

                        dtm1.Rows.InsertAt(dr2, 0);
                        dtm1.Rows[0]["Rej_Perc"] = Math.Round((fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) / (fgen.make_double(dtm1.Rows[0]["OK_qty"].ToString()) + fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()))) * 100, 2);

                        mq0 = "select distinct type1,name from typewip where id='RJC61' and branchcd='" + mbr + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + "(NOS)";
                                }
                            }
                            else if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + "(KGS)";
                                }
                            }
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr. Job Wise Rejection Reason Report For the Period " + fromdt + " To " + todt + " (Corrugaton)", frm_qstr);
                    //fgen.open_rptlevel_hd("Corr. Job Wise Rejection  Reason Report between " + fromdt + " and " + todt + " (Corrugaton)");
                    #endregion
                    break;

                case "15163L_1":// wfinsys_erp id // RPTLEVEL HD
                case "F40316":  // comma done
                    #region Corr. Item Wise Rejection Reason Report
                    inspvchtab = "INSPVCH";
                    if (co_cd == "SKYP")
                    {
                        inspvchtab = "INSPVCHK";
                    }
                    mq0 = "select distinct type1 from typewip where id='RJC61' and branchcd='" + mbr + "' ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("ICODE", typeof(string));
                    dtm1.Columns.Add("INAME", typeof(string));
                    dtm1.Columns.Add("Total_Nos", typeof(string));
                    dtm1.Columns.Add("Total_WT", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string)); dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }

                    dtm1.Columns.Add("OK_QTY", typeof(string));
                    dtm1.Columns.Add("Tot_Rej", typeof(string));
                    dtm1.Columns.Add("Rej_Perc", typeof(string));

                    mq1 = "SELECT A.ICODE,a.INAME,A.REASON_CODE,to_char(SUM(A.DOWNTIME_WT),'999,999,999,999.99') AS DOWNTIME_WT,SUM(A.DOWNTIME) AS DOWNTIME,to_char(sum(A.prdqty),'999,999,999,999.99') as prdqty,to_char(sum(prdqty_wt),'999,999,999,999.99') as prdqty_wt from (SELECT A.ICODE,B.INAME,B.boxWT*SUM(A.DOWNTIME) AS DOWNTIME_WT,A.VCHNUM AS ENTRY_NO,SUM(A.DOWNTIME) AS DOWNTIME ,A.MACHINE,A.REASON_CODE,A.MONTH AS VCHDATE,A.VCHDATEORD,A.ENQNO AS JOB_NO,A.ENQDT AS JOB_DATE,sum(A.prdqty) as prdqty,B.boxWT*sum(a.prdqty) as prdqty_wt ,MAX(B.JOB_QTY) AS JOB_QTY FROM (SELECT DISTINCT BRANCHCD, VCHNUM,ICODE,VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE,ENQNO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS ENQDT,prdqty FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATEORD,VCHDATE,MAX(Machine_Name) AS Machine_Name,REASON_CODE AS REASON_CODE,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt,max(prdqty) as prdqty from ( SELECT A.BRANCHCD,A.ICODE,A.VCHNUM,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VCHDATEORD,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.TITLE AS Machine_Name,A.COL2 AS REASON_CODE,is_number(A.COL3) as DOWNTIME,B.ENQNO,B.ENQDT,B.prdqty FROM " + inspvchtab + "  A,( SELECT DISTINCT  BRANCHCD,ICODE,VCHNUM,VCHDATE,ENQNO,ENQDT,sum(qty) as prdqty FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='40' and  vchdate " + xprdrange + " group BY BRANCHCD,ICODE,VCHNUM,VCHDATE,ENQNO,ENQDT) B WHERE TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND A." + branch_Cd + " and A.TYPE='45' and A.vchdate " + xprdrange + ") group by REASON_CODE,BRANCHCD,ICODE,VCHNUM,VCHDATEORD,VCHDATE) WHERE ENQNO IS NOT NULL ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,a.irate as boxWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " ) B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ENQNO)=TRIM(B.VCHNUM) AND TRIM(A.ENQDT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.MACHINE,A.REASON_CODE,A.MONTH,A.VCHDATEORD,B.INAME,B.boxWT,A.VCHNUM,A.ICODE,A.ENQNO,A.ENQDT  ORDER BY A.VCHDATEORD) A group by A.ICODE,a.INAME,A.REASON_CODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "ICODE", "INAME");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "ICODE='" + dr0["ICODE"] + "' AND INAME='" + dr0["INAME"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double prdtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());

                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                    prdtot = prdtot + fgen.make_double(dt1.Rows[i]["prdqty"].ToString());
                                }
                                catch { }
                            }
                            drrow1["TOTAL_Nos"] = tot.ToString("###,###,###,###.##");
                            drrow1["Ok_qty"] = prdtot.ToString("###,###,###,###.##");
                            drrow1["Tot_Rej"] = tot.ToString("###,###,###,###.##");
                            drrow1["Rej_perc"] = Math.Round(tot / (tot + prdtot) * 100, 2);

                            tot = 0;
                            prdtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME_WT"].ToString());

                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME_WT"].ToString());
                                    prdtot = prdtot + fgen.make_double(dt1.Rows[i]["prdqty_wt"].ToString());
                                }
                                catch { }
                            }
                            drrow1["TOTAL_WT"] = tot.ToString("###,###,###,###.##");
                            drrow1["ICODE"] = dt1.Rows[0]["ICODE"];
                            drrow1["INAME"] = dt1.Rows[0]["INAME"];
                            dtm1.Rows.Add(drrow1);
                        }

                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());
                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }

                        dtm1.Rows.InsertAt(dr2, 0);
                        dtm1.Rows[0]["Rej_Perc"] = Math.Round((fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) / (fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) + fgen.make_double(dtm1.Rows[0]["OK_qty"].ToString()))) * 100, 2);

                        mq0 = "select distinct type1,name from typewip where id='RJC61' and branchcd='" + mbr + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + "(NOS)";
                                }
                            }
                            else if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + "(KGS)";
                                }
                            }
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr. Item Wise Rejection Reason Report For the Period " + fromdt + " To " + todt + " ", frm_qstr);

                    #endregion
                    break;

                case "15163N":// wfinsys_erp id // RPTLEVEL HD
                case "F40317": //comma done
                    #region Corr. Monthly Rejection Reason Report
                    inspvchtab = "INSPVCH";
                    if (co_cd == "SKYP")
                    {
                        inspvchtab = "INSPVCHK";
                    }
                    mq0 = "select distinct type1 from typewip where id='RJC61' and branchcd='" + mbr + "' ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    dtm1 = new DataTable();

                    dtm1.Columns.Add("Month", typeof(string));
                    dtm1.Columns.Add("Machine", typeof(string));
                    dtm1.Columns.Add("Total_Nos", typeof(string));
                    dtm1.Columns.Add("Total_WT", typeof(string));

                    foreach (DataRow dr in dt.Rows) { dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string)); dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string)); }
                    dtm1.Columns.Add("OK_QTY", typeof(string));
                    dtm1.Columns.Add("Tot_Rej", typeof(string));
                    dtm1.Columns.Add("Rej_Perc", typeof(string));

                    mq1 = "SELECT to_CHAR(VCHDATEORD,'Month-YYYY') as month,MACHINE,SUM(DOWNTIME) AS DOWNTIME,to_char(SUM(DOWNTIME_WT),'999,999,999,999.99') AS DOWNTIME_WT,REASON_CODE,TO_CHAR(VCHDATEORD,'YYYYMM') AS  VCHDATEORD,to_char(SUM(JOB_QTY),'999,999,999,999.99') AS JOB_QTY,to_char(SUM(JOB_QTY_WT),'999,999,999,999.99')  AS JOB_QTY_WT  FROM (SELECT A.ICODE,B.INAME,B.BOXWT*SUM(A.DOWNTIME) AS DOWNTIME_WT,A.VCHNUM AS ENTRY_NO,SUM(A.DOWNTIME) AS DOWNTIME ,A.MACHINE,A.REASON_CODE,A.MONTH AS VCHDATE,A.VCHDATEORD,A.ENQNO AS JOB_NO,A.ENQDT AS JOB_DATE,SUM(A.PRDQTY) AS JOB_QTY,SUM(A.PRDQTY)*B.BOXWT  AS JOB_QTY_WT FROM (SELECT DISTINCT BRANCHCD, VCHNUM,ICODE,VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE,ENQNO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS ENQDT,PRDQTY FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATEORD,VCHDATE,MAX(Machine_Name) AS Machine_Name,REASON_CODE AS REASON_CODE,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt,max(PRDQTY) AS PRDQTY from (SELECT A.BRANCHCD,A.ICODE,A.VCHNUM,A.VCHDATE AS VCHDATEORD,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.TITLE AS Machine_Name,A.COL2 AS REASON_CODE,is_number(A.COL3) as DOWNTIME,B.ENQNO,B.ENQDT ,B.PRDQTY FROM " + inspvchtab + "  A,( SELECT DISTINCT  BRANCHCD,ICODE,VCHNUM,VCHDATE,ENQNO,ENQDT ,SUM(QTY) AS PRDQTY FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " GROUP BY BRANCHCD,ICODE,VCHNUM,VCHDATE,ENQNO,ENQDT) B WHERE TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND A." + branch_Cd + " and A.TYPE='45' and A.vchdate " + xprdrange + " ) group by REASON_CODE,BRANCHCD,ICODE,VCHNUM,VCHDATEORD,VCHDATE) WHERE ENQNO IS NOT NULL ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " ) B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ENQNO)=TRIM(B.VCHNUM) AND TRIM(A.ENQDT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.MACHINE,A.REASON_CODE,A.MONTH,A.VCHDATEORD,B.INAME,B.BOXWT,A.VCHNUM,A.ICODE,A.ENQNO,A.ENQDT  ORDER BY A.VCHDATEORD) GROUP BY to_CHAR(VCHDATEORD,'Month-YYYY'),REASON_CODE,MACHINE,VCHDATEORD ORDER BY VCHDATEORD";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Month", "Machine");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Month='" + dr0["month"] + "' and machine='" + dr0["machine"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double prdtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                String mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());

                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                    prdtot = prdtot + fgen.make_double(dt1.Rows[i]["job_qty"].ToString());
                                }
                                catch { }
                            }

                            drrow1["Month"] = dt1.Rows[0]["Month"];
                            drrow1["Machine"] = dt1.Rows[0]["Machine"];

                            drrow1["Ok_qty"] = prdtot.ToString("###,###,###,###.##");
                            drrow1["Tot_Rej"] = tot.ToString("###,###,###,###.##");
                            drrow1["Rej_perc"] = Math.Round(tot / (tot + prdtot) * 100, 2);
                            drrow1["TOTAL_Nos"] = tot.ToString("###,###,###,###.##");
                            tot = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME_WT"].ToString());
                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME_WT"].ToString());
                                }
                                catch { }
                            }
                            drrow1["TOTAL_WT"] = tot.ToString("###,###,###,###.##");
                            dtm1.Rows.Add(drrow1);
                        }

                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());
                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                // dr2[dc] = total;
                            }
                        }
                        dr2["Month"] = '-';
                        dr2["Machine"] = '-';
                        dtm1.Rows.InsertAt(dr2, 0);
                        dtm1.Rows[0]["Rej_Perc"] = Math.Round((fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) / (fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) + fgen.make_double(dtm1.Rows[0]["OK_qty"].ToString()))) * 100, 2);

                        mq0 = "select distinct type1,name from typewip where id='RJC61' and branchcd='" + mbr + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + "(NOS)";
                                }
                            }
                            else if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + "(KGS)";
                                }
                            }

                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr. Monthly Rejection Reason Report For the Period " + fromdt + " To " + todt + " (Corrugation) ", frm_qstr);
                    #endregion
                    break;

                case "15163O":// wfinsys_erp id  // RPTLEVEL HD
                case "F40318"://comma done
                    #region Corr. Job Wise Downtime Report
                    inspvchtab = "INSPVCH";
                    if (co_cd == "SKYP")
                    {
                        inspvchtab = "INSPVCHK";
                    }

                    mq0 = "select distinct type1 from typewip where id='DTC61' and branchcd='" + mbr + "' ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("ENTRY_NO", typeof(string));
                    dtm1.Columns.Add("VCHDATE", typeof(string));
                    dtm1.Columns.Add("JOB_NO", typeof(string));
                    dtm1.Columns.Add("JOB_DATE", typeof(string));
                    dtm1.Columns.Add("ICODE", typeof(string));
                    dtm1.Columns.Add("INAME", typeof(string));
                    dtm1.Columns.Add("Machine", typeof(string));
                    dtm1.Columns.Add("Total", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }

                    mq1 = "SELECT TRIM(A.ICODE) AS ICODE,TRIM(B.INAME) AS INAME,B.IWEIGHT*SUM(A.DOWNTIME) AS DOWNTIME_WT,TRIM(A.VCHNUM) AS ENTRY_NO,SUM(A.DOWNTIME) AS DOWNTIME ,TRIM(A.MACHINE) AS MACHINE,TRIM(A.REASON_CODE) AS REASON_CODE,A.MONTH AS VCHDATE,A.VCHDATEORD,TRIM(ENQNO) AS JOB_NO,ENQDT AS JOB_DATE FROM (SELECT DISTINCT srno, VCHNUM,ICODE,VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE,ENQNO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS ENQDT FROM (SELECT DISTINCT A.BRANCHCD,a.srno,A.ICODE,A.VCHNUM,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VCHDATEORD,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.TITLE AS Machine_Name,A.COL2 AS REASON_CODE,is_number(A.COL3) as DOWNTIME,B.ENQNO,B.ENQDT FROM INSPVCH a,(select distinct BRANCHCD,VCHNUM,VCHDATE,ENQDT,ENQNO FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + ") B WHERE  A." + branch_Cd + " and A.TYPE='55'  and A.vchdate " + xprdrange + " AND A.BRANCHCD=B.BRANCHCD AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND TRIM(A.VCHNUM)=TRIM(B.VCHNUM) ) WHERE ENQNO IS NOT NULL ) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE)  GROUP BY A.MACHINE,A.REASON_CODE,A.MONTH,A.VCHDATEORD,B.INAME,B.IWEIGHT,A.VCHNUM,A.ICODE,ENQNO,ENQDT ORDER BY a.vchnum, A.VCHDATEORD ";
                    mq1 = "SELECT TRIM(A.ICODE) AS ICODE,TRIM(B.INAME) AS INAME,TRIM(A.VCHNUM) AS ENTRY_NO,SUM(A.DOWNTIME) AS DOWNTIME ,TRIM(A.MACHINE) AS MACHINE,TRIM(A.REASON_CODE) AS REASON_CODE,A.MONTH AS VCHDATE,A.VCHDATEORD,TRIM(ENQNO) AS JOB_NO,ENQDT AS JOB_DATE FROM (SELECT DISTINCT srno, VCHNUM,ICODE,VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE,ENQNO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS ENQDT FROM (SELECT DISTINCT A.BRANCHCD,a.srno,A.ICODE,A.VCHNUM,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VCHDATEORD,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.TITLE AS Machine_Name,A.COL2 AS REASON_CODE,is_number(A.COL3) as DOWNTIME,B.ENQNO,B.ENQDT FROM INSPVCH a,(select distinct BRANCHCD,VCHNUM,VCHDATE,ENQDT,ENQNO FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + ") B WHERE A." + branch_Cd + " and  A.TYPE='55' and A.vchdate " + xprdrange + " AND A.BRANCHCD=B.BRANCHCD AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND TRIM(A.VCHNUM)=TRIM(B.VCHNUM) )) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE)  GROUP BY A.MACHINE,A.REASON_CODE,A.MONTH,A.VCHDATEORD,B.INAME,A.VCHNUM,A.ICODE,ENQNO,ENQDT ORDER BY a.vchnum, A.VCHDATEORD ";
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "ENTRY_NO", "VCHDATE", "JOB_NO", "JOB_DATE", "MACHINE", "ICODE", "INAME");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "VCHDATE='" + dr0["VCHDATE"] + "' AND ENTRY_NO='" + dr0["ENTRY_NO"] + "' AND ICODE='" + dr0["ICODE"] + "' AND INAME='" + dr0["INAME"] + "' AND JOB_NO='" + dr0["JOB_NO"] + "' AND JOB_DATE='" + dr0["JOB_DATE"] + "' and MACHINE='" + dr0["MACHINE"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;

                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                    try
                                    {
                                        drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());

                                        tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                    }
                                    catch { }
                                }

                                drrow1["ENTRY_NO"] = dt1.Rows[0]["ENTRY_NO"];
                                drrow1["VCHDATE"] = dt1.Rows[0]["VCHDATE"];
                                drrow1["ICODE"] = dt1.Rows[0]["ICODE"];
                                drrow1["INAME"] = dt1.Rows[0]["INAME"];
                                drrow1["JOB_NO"] = dt1.Rows[0]["JOB_NO"];
                                drrow1["JOB_DATE"] = dt1.Rows[0]["JOB_DATE"];
                                drrow1["MACHINE"] = dt1.Rows[0]["MACHINE"];
                                drrow1["TOTAL"] = tot.ToString();
                                dtm1.Rows.Add(drrow1);
                            }
                        }

                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());
                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                // dr2[dc] = total;

                            }
                        }
                        dr2["JOB_DATE"] = '-';
                        dtm1.Rows.InsertAt(dr2, 0);
                        mq0 = "select distinct type1,name from typewip where id='DTC61' and branchcd='" + mbr + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string name = dc.ToString().Remove(0, 1);
                            string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                            if (myname != "0")
                            {
                                dtm1.Columns[abc].ColumnName = myname;
                            }
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr. Job Wise Downtime Report For the Period " + fromdt + " To " + todt + " (Corrugation) ", frm_qstr);
                    #endregion
                    break;

                case "15163P":// wfinsys_erp id // RPTLEVEL HD
                case "F40319": // comma done
                    #region Corr. Date Wise Reason Downtime Report
                    inspvchtab = "INSPVCH";
                    if (co_cd == "SKYP")
                    {
                        inspvchtab = "INSPVCHK";
                    }

                    mq0 = "select distinct type1 from typewip where id='DTC61' and branchcd='" + mbr + "'ORDER BY TYPE1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Total", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }

                    mq1 = "SELECT SUM(DOWNTIME) AS DOWNTIME ,MACHINE,REASON_CODE,MONTH AS JOB_DATE,VCHDATEORD FROM(SELECT VCHDATEORD,VCHDATE AS MONTH,MACHINE_NAME AS MACHINE,DOWNTIME as DOWNTIME,REASON_CODE FROM (SELECT BRANCHCD,TYPE,VCHNUM,TO_CHAR(VCHDATE,'YYYYMMDD') AS VCHDATEORD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TITLE AS Machine_Name,COL2 AS REASON_CODE,is_number(COL3) as DOWNTIME FROM " + inspvchtab + " WHERE  " + branch_Cd + " and TYPE='55'  and vchdate " + xprdrange + " ORDER BY VCHDATEORD  ) ) GROUP BY MACHINE,REASON_CODE,MONTH,VCHDATEORD ORDER BY VCHDATEORD";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "JOB_DATE");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "JOB_DATE='" + dr0["JOB_DATE"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["REASON_CODE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());

                                    tot = tot + fgen.make_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                }
                                catch { }
                            }

                            drrow1["JOB_DATE"] = dt1.Rows[0]["JOB_DATE"];
                            drrow1["TOTAL"] = tot.ToString("###,###,###.##");
                            dtm1.Rows.Add(drrow1);
                        }

                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());
                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        dr2["JOB_DATE"] = '-';
                        dtm1.Rows.InsertAt(dr2, 0);
                        //    dt1 = fgen.getdata(co_cd, SQuery);
                        mq0 = "select distinct type1,name from typewip where id='DTC61' and branchcd='" + mbr + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string name = dc.ToString().Remove(0, 1);
                            string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                            if (myname != "0")
                            {
                                dtm1.Columns[abc].ColumnName = myname;
                            }
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Corr. Date Wise Reason Downtime Report For the Period " + fromdt + " To " + todt + " (Corrugation) ", frm_qstr);
                    #endregion
                    break;

                case "15163R":// wfinsys_erp id  // RPTLEVEL HD
                case "F40320":// comma done
                    #region Job Wise All Stage Production Report
                    mq0 = "SELECT DISTINCT A.STAGE,(CASE WHEN LENGTH(TRIM(B.ADDR2))=2  THEN TRIM(B.ADDR2)  ELSE B.TYPE1 END ) AS SHRT FROM(SELECT DISTINCT branchcd ,ENQNO as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,icode, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='45'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='55'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE TYPE='40' and " + branch_Cd + " and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode union all sELECT  BRANCHCD,JOB_NO,JOB_DT,ICODE,trim(STAGE) as stage,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE TYPE='86' AND  " + branch_Cd + " and vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE union all select branchcd,ENQNO AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  type='60'   and " + branch_Cd + " and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate)A,TYPE B WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND TRIM(B.ID)='K' ORDER BY SHRT";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_No", typeof(string));
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Icode", typeof(string));
                    dtm1.Columns.Add("Item_Name", typeof(string));
                    dtm1.Columns.Add("Part_No", typeof(string));
                    dtm1.Columns.Add("Order_Qty", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    mq1 = "SELECT A.JOB_NO,A.JOB_DT as Job_date,A.ICODE,B.INAME as Item_Name,B.CPARTNO as Part_No,to_char(B.QTYORD,'999,999,999,999.99') as Order_qty,to_char(B.JOB_QTY,'999,999,999,999.99') as JOB_QTY ,A.STAGE,to_char(SUM(A.PRDQTY),'999,999,999,999.99')  AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,to_char((SUM(A.PRDQTY)*B.BOXWT),'999,999,999,999.99') AS PRDQTY_WT,to_char((SUM(A.REJQTY)*B.BOXWT),'999,999,999,999.99') AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='45' and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='55'  and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='40' and  vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE " + branch_Cd + " and TYPE='86' AND vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  " + branch_Cd + " and  type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,a.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT ORDER BY JOB_NO,ICODE";
                    // mq1 = "select * from (" + mq1 + ") where trim(icode)='91050048'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Job_No", "Job_date", "Icode", "Item_Name", "Part_No", "Order_qty", "Job_qty");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Job_No='" + dr0["Job_No"] + "' and Job_date='" + dr0["Job_date"] + "' and icode='" + dr0["icode"] + "' and Item_Name='" + dr0["Item_Name"] + "' and Part_No='" + dr0["Part_No"] + "' AND Order_qty='" + dr0["Order_qty"] + "' and Job_Qty='" + dr0["Job_qty"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double jobtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["PRDQTY"].ToString());
                                    // tot = tot + fgen.return_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                }
                                catch { }
                            }
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["PRDQTY_WT"].ToString());
                                    // tot = tot + fgen.return_double(dt1.Rows[i]["DOWNTIME"].ToString());
                                }
                                catch { }
                            }
                            if (dt1.Rows.Count > 0)
                            {
                                drrow1["Job_No"] = dt1.Rows[0]["Job_No"];
                                drrow1["Job_date"] = dt1.Rows[0]["Job_date"];
                                drrow1["Icode"] = dt1.Rows[0]["Icode"];
                                drrow1["Item_name"] = dt1.Rows[0]["Item_name"];
                                drrow1["Part_No"] = dt1.Rows[0]["Part_No"];
                                drrow1["Order_qty"] = dt1.Rows[0]["Order_qty"];
                                drrow1["Job_qty"] = dt1.Rows[0]["Job_qty"];
                            }
                            dtm1.Rows.Add(drrow1);
                        }
                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += Math.Round(fgen.make_double(drrr[dc.ToString()].ToString()), 2);
                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        dtm1.Rows.InsertAt(dr2, 0);

                        mq0 = "SELECT A.TYPE1,A.NAME FROM TYPE A WHERE A.ID='K'  AND TRIM(A.TYPE1) IN(SELECT DISTINCT STAGEC FROM ITWSTAGE) ORDER BY A.TYPE1";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + " (NOS)";
                                }
                            }
                            if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + " (KGS)";
                                }
                            }
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Job Wise All Stage Production Report For the Period " + fromdt + " To " + todt + " ", frm_qstr);

                    #endregion
                    break;

                case "15163S":// wfinsys_erp id // RPTLEVEL HD
                case "F40321":
                    #region Item Wise All Stage Production Report
                    mq0 = "SELECT DISTINCT A.STAGE,(CASE WHEN LENGTH(TRIM(B.ADDR2))=2  THEN TRIM(B.ADDR2)  ELSE B.TYPE1 END ) AS SHRT FROM(SELECT DISTINCT branchcd ,ENQNO as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,icode, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='55' and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode union all sELECT  BRANCHCD,JOB_NO,JOB_DT,ICODE,trim(STAGE) as stage,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE  " + branch_Cd + " and TYPE='86' AND vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE union all select branchcd,ENQNO AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where " + branch_Cd + " and type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate)A,TYPE B WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND TRIM(B.ID)='K' ORDER BY SHRT";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_No", typeof(string));
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Icode", typeof(string));
                    dtm1.Columns.Add("Item_Name", typeof(string));
                    dtm1.Columns.Add("Part_No", typeof(string));
                    dtm1.Columns.Add("Order_Qty", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    dtraw = new DataTable();
                    dtraw.Columns.Add("Icode", typeof(string));
                    dtraw.Columns.Add("Item_Name", typeof(string));
                    dtraw.Columns.Add("Part_No", typeof(string));
                    dtraw.Columns.Add("Job_Qty", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtraw.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(double));
                        dtraw.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(double));
                    }
                    mq1 = "SELECT A.JOB_NO,A.JOB_DT as Job_date,A.ICODE,B.INAME as Item_Name,B.CPARTNO as Part_No,to_char(B.QTYORD,'999,999,999,999') as Order_qty,to_char(B.JOB_QTY,'999,999,999,999') as JOB_QTY,A.STAGE,to_char(SUM(A.PRDQTY),'999,999,999,999') AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,to_char((SUM(A.PRDQTY)*B.BOXWT),'999,999,999,999.99') AS PRDQTY_WT,to_char((SUM(A.REJQTY)*B.BOXWT),'999,999,999,999.99') AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='45'  and  vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='55'  and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE " + branch_Cd + " and TYPE='86' AND vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where " + branch_Cd + " and  type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,a.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT ORDER BY JOB_NO,ICODE";
                    // mq1 = "select * from (" + mq1 + ") where trim(icode)='91050048'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Job_No", "Job_date", "Icode", "Item_Name", "Part_No", "Order_qty", "Job_qty");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Job_No='" + dr0["Job_No"] + "' and Job_date='" + dr0["Job_date"] + "' and icode='" + dr0["icode"] + "' and Item_Name='" + dr0["Item_Name"] + "' and Part_No='" + dr0["Part_No"] + "' AND Order_qty='" + dr0["Order_qty"] + "' and Job_Qty='" + dr0["Job_qty"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double jobtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["PRDQTY"].ToString());
                                }
                                catch { }
                            }
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["PRDQTY_WT"].ToString());
                                }
                                catch { }
                            }
                            drrow1["Job_No"] = dt1.Rows[0]["Job_No"];
                            drrow1["Job_date"] = dt1.Rows[0]["Job_date"];
                            drrow1["Icode"] = dt1.Rows[0]["Icode"];
                            drrow1["Item_name"] = dt1.Rows[0]["Item_name"];
                            drrow1["Part_No"] = dt1.Rows[0]["Part_No"];
                            drrow1["Order_qty"] = dt1.Rows[0]["Order_qty"];
                            drrow1["Job_qty"] = dt1.Rows[0]["Job_qty"];
                            dtm1.Rows.Add(drrow1);
                        }
                        ////////////////////////////

                        dtm1.Columns.Remove("job_no");
                        dtm1.Columns.Remove("job_date");
                        dtm1.Columns.Remove("order_qty");
                        double sum = 0;
                        view1im = new DataView(dtm1);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Icode");

                        foreach (DataRow dr in dtdrsim.Rows)
                        {
                            if (dr["icode"].ToString().Trim() == "92030016") { }
                            DataView viewim = new DataView(dtm1, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            DataRow drraw = dtraw.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                sum = 0;

                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2)
                                {
                                    drraw["icode"] = dt1.Rows[0]["icode"].ToString();
                                    drraw["Item_Name"] = dt1.Rows[0]["Item_Name"].ToString();
                                    drraw["PART_NO"] = dt1.Rows[0]["PART_NO"].ToString();
                                }
                                else
                                {
                                    foreach (DataRow drr in dt1.Rows)
                                    {
                                        sum += fgen.make_double(drr[dc].ToString());
                                    }
                                    drraw[dc.ColumnName] = sum;
                                }
                            }
                            dtraw.Rows.Add(drraw);
                        }

                        dr2 = dtraw.NewRow();
                        d = 0;
                        //////////////////

                        //dr2 = dtm1.NewRow();
                        //d = 0;
                        string check;
                        foreach (DataColumn dc in dtraw.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2) { }
                            else
                            {
                                foreach (DataRow drrr in dtraw.Rows)
                                {
                                    total += Math.Round(fgen.make_double(drrr[dc.ToString()].ToString()), 2);
                                }
                                if (total > 0)
                                {
                                    check = total.ToString("###,###,###,###.##");
                                }
                                else
                                {
                                    check = "0";
                                }
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        dtraw.Rows.InsertAt(dr2, 0);


                        mq0 = "SELECT A.TYPE1,A.NAME FROM TYPE A WHERE A.ID='K'  AND TRIM(A.TYPE1) IN(SELECT DISTINCT STAGEC FROM ITWSTAGE) ORDER BY A.TYPE1";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtraw.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtraw.Columns[abc].ColumnName = myname + " (NOS)";
                                }
                            }
                            if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtraw.Columns[abc].ColumnName = myname + " (KGS)";
                                }
                            }
                        }
                    }
                    Session["send_dt"] = dtraw;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Item Wise All Stage Production Report For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "15163T":// wfinsys_erp id // RPTLEVEL HD
                case "F40322":
                    #region Job/Stage Wise Pending Stock Report
                    mq0 = "SELECT DISTINCT A.STAGE,(CASE WHEN LENGTH(TRIM(B.ADDR2))=2  THEN TRIM(B.ADDR2)  ELSE B.TYPE1 END ) AS SHRT FROM(SELECT DISTINCT branchcd ,ENQNO as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,icode, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='45'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='55'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE TYPE='40' and " + branch_Cd + " and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode union all sELECT  BRANCHCD,JOB_NO,JOB_DT,ICODE,trim(STAGE) as stage,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE TYPE='86' AND  " + branch_Cd + " and vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE union all select branchcd,ENQNO AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  type='60'   and " + branch_Cd + " and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate)A,TYPE B WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND TRIM(B.ID)='K' ORDER BY SHRT";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_No", typeof(string));
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Icode", typeof(string));
                    dtm1.Columns.Add("Item_Name", typeof(string));
                    dtm1.Columns.Add("Part_No", typeof(string));
                    dtm1.Columns.Add("Order_Qty", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    dtm1.Columns.Add("Total_NOS", typeof(string));
                    dtm1.Columns.Add("Total_WT", typeof(string));
                    dtm1.Columns.Add("Total_Value", typeof(string));

                    mq1 = "SELECT * FROM(select a.* ,b.srno from(SELECT A.JOB_NO,A.JOB_DT as Job_date,A.ICODE,B.INAME as Item_Name,B.CPARTNO as Part_No,B.QTYORD as Order_qty,B.JOB_QTY,TRIM(A.STAGE) AS STAGE,SUM(A.PRDQTY) AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,SUM(A.PRDQTY)*B.BOXWT AS PRDQTY_WT,SUM(A.REJQTY)*B.BOXWT AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, TRIM('02') as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='55'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,TRIM(STAGE) AS STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE TYPE='86' AND  " + branch_Cd + " and vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  " + branch_Cd + " and type='60' and  vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate UNION ALL SELECT BRANCHCD,INVNO,TO_CHAR(INVDATE,'DD/MM/YYYY') AS INVDATE,TRIM(ICODE) AS ICODE,'51' AS STAGE,(SUM(IQTYIN)) AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME FROM IVOUCHER WHERE  " + branch_Cd + " and TYPE='16' and vchdate " + xprdrange + " GROUP BY BRANCHCD,INVNO,TO_CHAR(INVDATE,'DD/MM/YYYY'),ICODE) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + "  )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT ORDER BY JOB_NO,ICODE) a LEFT OUTER JOIN itwstage b ON trim(a.icode)=trim(b.icode) and trim(a.stage)=trim(b.stagec)  order by a.job_no,a.icode,b.srno)";
                    mq1 = "SELECT * FROM(select a.* ,b.srno from(SELECT A.JOB_NO,A.JOB_DT as Job_date,A.ICODE,B.INAME as Item_Name,B.CPARTNO as Part_No,B.QTYORD as Order_qty,B.IRATE,B.JOB_QTY,TRIM(A.STAGE) AS STAGE,SUM(A.PRDQTY) AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,SUM(A.PRDQTY)*B.BOXWT AS PRDQTY_WT,SUM(A.REJQTY)*B.BOXWT AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, TRIM('02') as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='45' and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='55' and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,TRIM(STAGE) AS STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE  " + branch_Cd + " and TYPE='86' AND vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where " + branch_Cd + " and type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate UNION ALL SELECT BRANCHCD,INVNO,TO_CHAR(INVDATE,'DD/MM/YYYY') AS INVDATE,TRIM(ICODE) AS ICODE,'51' AS STAGE,(SUM(IQTYIN)) AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME FROM IVOUCHER WHERE " + branch_Cd + " and TYPE='16' and vchdate " + xprdrange + " GROUP BY BRANCHCD,INVNO,TO_CHAR(INVDATE,'DD/MM/YYYY'),ICODE) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT,B.IRATE FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + "  )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT,B.IRATE ORDER BY JOB_NO,ICODE) a LEFT OUTER JOIN itwstage b ON trim(a.icode)=trim(b.icode) and trim(a.stage)=trim(b.stagec)  order by a.job_no,a.icode,b.srno) ";
                    //  mq1 = "select * from (" + mq1 + ") where trim(icode)='90010001'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Job_No", "Job_date", "Icode", "Item_Name", "Part_No", "Order_qty", "Job_qty", "irate");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Job_No='" + dr0["Job_No"] + "' and Job_date='" + dr0["Job_date"] + "' and icode='" + dr0["icode"] + "' and Item_Name='" + dr0["Item_Name"] + "' and Part_No='" + dr0["Part_No"] + "' AND Order_qty='" + dr0["Order_qty"] + "' and Job_Qty='" + dr0["Job_qty"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0, last = 0;
                            double mcount = 0, lastval = 0, currval = 0;
                            Int64 laststage = 0;
                            mcount = dt1.Rows.Count;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                Int64 mm = Convert.ToInt64(dt1.Rows[i]["STAGE"].ToString().Trim());
                                try
                                {
                                    currval = fgen.make_double(dt1.Rows[i]["PRDQTY"].ToString());
                                    if (i == 0)
                                    {
                                        drrow1["R" + fgen.padlc(mm, 2)] = Math.Round(currval, 2);
                                        //  tot = tot + Math.Round(currval, 2);
                                        last = Math.Round(currval, 2);
                                    }
                                    else if (i == mcount - 1)
                                    {
                                        drrow1["R" + (fgen.padlc(laststage, 2).ToString().Trim())] = Math.Round(lastval - currval);
                                        drrow1["R" + fgen.padlc(mm, 2).ToString().Trim()] = Math.Round(currval, 2);
                                        //tot = tot + Math.Round(currval, 2);
                                    }
                                    else
                                    {
                                        drrow1["R" + (fgen.padlc(laststage, 2).ToString().Trim())] = Math.Round(lastval - currval);
                                        //  tot = tot + Math.Round(lastval - currval);
                                    }
                                    lastval = fgen.make_double(dt1.Rows[i]["PRDQTY"].ToString());
                                }
                                catch { }
                                laststage = mm;
                            }
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                Int64 mm = Convert.ToInt64(dt1.Rows[i]["STAGE"].ToString().Trim());
                                try
                                {
                                    currval = fgen.make_double(dt1.Rows[i]["PRDQTY_WT"].ToString());
                                    if (i == 0)
                                    {
                                        drrow1["W" + fgen.padlc(mm, 2)] = Math.Round(currval, 2);
                                    }
                                    else if (i == mcount - 1)
                                    {
                                        drrow1["W" + (fgen.padlc(laststage, 2).ToString().Trim())] = Math.Round(lastval - currval, 2);
                                        drrow1["W" + fgen.padlc(mm, 2).ToString().Trim()] = Math.Round(currval, 2);
                                    }
                                    else
                                    {
                                        drrow1["W" + (fgen.padlc(laststage, 2).ToString().Trim())] = Math.Round(lastval - currval, 2);
                                    }
                                    lastval = fgen.make_double(dt1.Rows[i]["PRDQTY_WT"].ToString());
                                }
                                catch { }
                                laststage = mm;
                            }

                            drrow1["Job_No"] = dt1.Rows[0]["Job_No"];
                            drrow1["Job_date"] = dt1.Rows[0]["Job_date"];
                            drrow1["Icode"] = dt1.Rows[0]["Icode"];
                            drrow1["Item_name"] = dt1.Rows[0]["Item_name"];
                            drrow1["Part_No"] = dt1.Rows[0]["Part_No"];
                            drrow1["Order_qty"] = dt1.Rows[0]["Order_qty"];
                            drrow1["Job_qty"] = dt1.Rows[0]["Job_qty"];
                            drrow1["Total_Value"] = dr0["IRATE"].ToString();
                            dtm1.Rows.Add(drrow1);
                        }

                        dr2 = dtm1.NewRow();
                        d = 0;

                        //////////////////
                        foreach (DataRow drma in dtm1.Rows)
                        {
                            double total = 0, total1 = 0;
                            Boolean ok = true;
                            foreach (DataColumn dc in dtm1.Columns)
                            {
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6) { }
                                else
                                {
                                    if (ok)
                                    {
                                        total += Math.Round(fgen.make_double(drma[dc.ToString()].ToString()), 2);
                                        ok = false;
                                    }
                                    else
                                    {
                                        total1 += Math.Round(fgen.make_double(drma[dc.ToString()].ToString()), 2);
                                        ok = true;
                                    }
                                }
                            }

                            drma["Total_nos"] = total;
                            drma["Total_wt"] = total1;
                            string s1 = drma["Total_Value"].ToString();
                            drma["Total_Value"] = fgen.make_double(s1) * fgen.make_double(total, 2);
                        }
                        //////////////////
                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += Math.Round(fgen.make_double(drrr[dc.ToString()].ToString()), 2);
                                }

                                dr2[dc] = total;
                            }
                        }
                        dtm1.Rows.InsertAt(dr2, 0);

                        mq0 = "SELECT A.TYPE1,A.NAME FROM TYPE A WHERE A.ID='K'  AND TRIM(A.TYPE1) IN(SELECT DISTINCT STAGEC FROM ITWSTAGE) ORDER BY A.TYPE1";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + " (NOS)";
                                }
                            }
                            if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + " (KGS)";
                                }
                            }
                        }
                        DataView dv1 = dtm1.DefaultView;
                        dv1.Sort = "icode asc";
                        dtm1 = dv1.ToTable();
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Job/Stage Wise Pending Stock Report For the Period " + fromdt + " and " + todt + " ", frm_qstr);
                    //fgen.open_rptlevel_hd("Job/Stage Wise Pending Stock Report between " + fromdt + " and " + todt + " ");                    
                    #endregion
                    break;

                case "15163U":// wfinsys_erp id // RPTLEVEL HD
                case "F40323":
                    #region Item / Stage Wise Pending Stock Report
                    mq0 = "SELECT DISTINCT A.STAGE,(CASE WHEN LENGTH(TRIM(B.ADDR2))=2  THEN TRIM(B.ADDR2)  ELSE B.TYPE1 END ) AS SHRT FROM(SELECT DISTINCT branchcd ,ENQNO as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,icode, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='55'  and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode union all sELECT  BRANCHCD,JOB_NO,JOB_DT,ICODE,trim(STAGE) as stage,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE   " + branch_Cd + " and TYPE='86' AND vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE union all select branchcd,ENQNO AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  " + branch_Cd + " and type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate)A,TYPE B WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND TRIM(B.ID)='K' ORDER BY SHRT";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_No", typeof(string));
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Icode", typeof(string));
                    dtm1.Columns.Add("Item_Name", typeof(string));
                    dtm1.Columns.Add("Part_No", typeof(string));
                    dtm1.Columns.Add("Order_Qty", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    dtraw = new DataTable();
                    dtraw.Columns.Add("Icode", typeof(string));
                    dtraw.Columns.Add("Item_Name", typeof(string));
                    dtraw.Columns.Add("Part_No", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtraw.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(double));
                        dtraw.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(double));
                    }

                    mq1 = "SELECT * FROM(select a.* ,b.srno from(SELECT A.JOB_NO,A.JOB_DT as Job_date,trim(A.ICODE) as ICODE,B.INAME as Item_Name,B.CPARTNO as Part_No,B.QTYORD as Order_qty,B.JOB_QTY,A.STAGE,SUM(A.PRDQTY) AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,SUM(A.PRDQTY)*B.BOXWT AS PRDQTY_WT,SUM(A.REJQTY)*B.BOXWT AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='55'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='40' and  vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE  " + branch_Cd + " and TYPE='86' AND  vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  " + branch_Cd + " and type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate UNION ALL SELECT BRANCHCD,INVNO,TO_CHAR(INVDATE,'DD/MM/YYYY') AS INVDATE,TRIM(ICODE) AS ICODE,'51' AS STAGE,(SUM(IQTYIN)) AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME FROM IVOUCHER WHERE TYPE='16' and " + branch_Cd + " and vchdate " + xprdrange + " GROUP BY BRANCHCD,INVNO,TO_CHAR(INVDATE,'DD/MM/YYYY'),ICODE) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " AND TRIM(A.STATUS)<>'Y' )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,trim(A.ICODE),B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT ORDER BY JOB_NO,ICODE) a LEFT OUTER JOIN itwstage b ON trim(a.icode)=trim(b.icode) and trim(a.stage)=trim(b.stagec)  order by a.job_no,a.icode,b.srno) order by  job_no,icode,srno";
                    // mq1 = "select * from (" + mq1 + ") where trim(icode)='90010001' or trim(icode)='92030016'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Job_No", "Job_date", "Icode", "Item_Name", "Part_No", "Order_qty", "Job_qty");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Job_No='" + dr0["Job_No"] + "' and Job_date='" + dr0["Job_date"] + "' and icode='" + dr0["icode"] + "' and Item_Name='" + dr0["Item_Name"] + "' and Part_No='" + dr0["Part_No"] + "' AND Order_qty='" + dr0["Order_qty"] + "' and Job_Qty='" + dr0["Job_qty"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;

                            double mcount = 0, lastval = 0, currval = 0;
                            string laststage = "";
                            mcount = dt1.Rows.Count;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    currval = fgen.make_double(dt1.Rows[i]["PRDQTY"].ToString());
                                    if (i == 0)
                                    {
                                        drrow1["R" + mm] = Math.Round(currval, 2);
                                    }
                                    else if (i == mcount - 1)
                                    {
                                        drrow1["R" + laststage] = Math.Round(lastval - currval, 2);
                                        drrow1["R" + mm] = Math.Round(currval, 2);

                                    }
                                    else
                                    {
                                        drrow1["R" + laststage] = Math.Round(lastval - currval, 2);
                                    }
                                    lastval = fgen.make_double(dt1.Rows[i]["PRDQTY"].ToString());

                                }
                                catch { }
                                laststage = mm;
                            }
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    currval = fgen.make_double(dt1.Rows[i]["PRDQTY_WT"].ToString());
                                    if (i == 0)
                                    {
                                        drrow1["W" + mm] = Math.Round(currval, 2);
                                    }
                                    else if (i == mcount - 1)
                                    {
                                        drrow1["W" + laststage] = Math.Round(lastval - currval, 2);
                                        drrow1["W" + mm] = Math.Round(currval, 2);

                                    }
                                    else
                                    {
                                        drrow1["W" + laststage] = Math.Round(lastval - currval, 2);
                                    }
                                    lastval = fgen.make_double(dt1.Rows[i]["PRDQTY_WT"].ToString());

                                }
                                catch { }
                                laststage = mm;
                            }
                            drrow1["Job_No"] = dt1.Rows[0]["Job_No"];
                            drrow1["Job_date"] = dt1.Rows[0]["Job_date"];
                            drrow1["Icode"] = dt1.Rows[0]["Icode"];
                            drrow1["Item_name"] = dt1.Rows[0]["Item_name"];
                            drrow1["Part_No"] = dt1.Rows[0]["Part_No"];
                            drrow1["Order_qty"] = dt1.Rows[0]["Order_qty"];
                            drrow1["Job_qty"] = dt1.Rows[0]["Job_qty"];
                            dtm1.Rows.Add(drrow1);
                        }

                        ///////////////////////

                        dtm1.Columns.Remove("job_no");
                        dtm1.Columns.Remove("job_date");
                        dtm1.Columns.Remove("order_qty");
                        dtm1.Columns.Remove("job_qty");
                        double sum = 0;
                        view1im = new DataView(dtm1);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Icode");

                        foreach (DataRow dr in dtdrsim.Rows)
                        {
                            if (dr["icode"].ToString().Trim() == "92030016") { }
                            DataView viewim = new DataView(dtm1, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            DataRow drraw = dtraw.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                sum = 0;
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2)
                                {
                                    drraw["icode"] = dt1.Rows[0]["icode"].ToString();
                                    drraw["Item_Name"] = dt1.Rows[0]["Item_Name"].ToString();
                                    drraw["PART_NO"] = dt1.Rows[0]["PART_NO"].ToString();
                                }
                                else
                                {
                                    foreach (DataRow drr in dt1.Rows)
                                    {
                                        sum += fgen.make_double(drr[dc].ToString());
                                    }
                                    drraw[dc.ColumnName] = sum;
                                }
                            }
                            dtraw.Rows.Add(drraw);
                        }

                        dr2 = dtraw.NewRow();
                        d = 0;
                        //////////////////
                        foreach (DataColumn dc in dtraw.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    total += Math.Round(fgen.make_double(drrr[dc.ToString()].ToString()), 2);
                                }

                                dr2[dc.ColumnName] = total;
                            }
                        }

                        dtraw.Rows.InsertAt(dr2, 0);
                        mq0 = "SELECT A.TYPE1,A.NAME FROM TYPE A WHERE A.ID='K'  AND TRIM(A.TYPE1) IN(SELECT DISTINCT STAGEC FROM ITWSTAGE) ORDER BY A.TYPE1";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtraw.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtraw.Columns[abc].ColumnName = myname + " (NOS)";
                                }
                            }
                            if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtraw.Columns[abc].ColumnName = myname + " (KGS)";
                                }
                            }
                        }
                        DataView dv1 = dtm1.DefaultView;
                        dv1.Sort = "icode asc";

                        dtm1 = dv1.ToTable();
                    }
                    Session["send_dt"] = dtraw;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Item / Stage Wise Pending Stock Report For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "15163V":// wfinsys_erp id  // RPTLEVEL HD
                case "F40324": //DONE 
                    #region Job Wise All Stage Rejection
                    mq0 = "SELECT DISTINCT A.STAGE,(CASE WHEN LENGTH(TRIM(B.ADDR2))=2  THEN TRIM(B.ADDR2)  ELSE B.TYPE1 END ) AS SHRT FROM(SELECT DISTINCT branchcd ,ENQNO as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,icode, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='55' and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode union all sELECT  BRANCHCD,JOB_NO,JOB_DT,ICODE,trim(STAGE) as stage,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE  " + branch_Cd + " and TYPE='86' AND  vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE union all select branchcd,ENQNO AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where " + branch_Cd + " and  type='60'   and  vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate)A,TYPE B WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND TRIM(B.ID)='K' ORDER BY SHRT";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_No", typeof(string));
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Icode", typeof(string));
                    dtm1.Columns.Add("Item_Name", typeof(string));
                    dtm1.Columns.Add("Part_No", typeof(string));
                    dtm1.Columns.Add("Order_Qty", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    dtm1.Columns.Add("OK_QTY", typeof(string));
                    dtm1.Columns.Add("Tot_Rej", typeof(string));
                    dtm1.Columns.Add("Rej_Perc", typeof(string));
                    dtm1.Columns.Add("Totl_Rej_Value", typeof(string));

                    mq1 = "SELECT A.JOB_NO,A.JOB_DT as Job_date,trim(A.ICODE) as icode,B.INAME as Item_Name,B.CPARTNO as Part_No,B.QTYORD as Order_qty,B.JOB_QTY,A.STAGE,SUM(A.PRDQTY) AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,SUM(A.PRDQTY)*B.BOXWT AS PRDQTY_WT,SUM(A.REJQTY)*B.BOXWT AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='45'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE TYPE='55'  and " + branch_Cd + " and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE TYPE='40' and " + branch_Cd + " and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE TYPE='86' AND  " + branch_Cd + " and vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  type='60'   and " + branch_Cd + " and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT ORDER BY ICODE";
                    mq1 = "SELECT A.JOB_NO,A.JOB_DT as Job_date,trim(A.ICODE) as icode,B.INAME as Item_Name,B.CPARTNO as Part_No,TO_CHAR(B.QTYORD,'999,999,999,999') as Order_qty,b.irate,TO_CHAR(B.JOB_QTY,'999,999,999,999') AS JOB_QTY,A.STAGE,TO_CHAR(SUM(A.PRDQTY),'999,999,999,999.99') AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,TO_CHAR((SUM(A.PRDQTY)*B.BOXWT),'999,999,999,999.99') AS PRDQTY_WT,TO_CHAR((SUM(A.REJQTY)*B.BOXWT),'999,999,999,999.99') AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='45'  and  vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='55' and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE " + branch_Cd + " and TYPE='86' AND vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  " + branch_Cd + " and  type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,b.irate,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " ) B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT,b.irate ORDER BY ICODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Job_No", "Job_date", "Icode", "Item_Name", "Part_No", "Job_qty", "irate");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Job_No='" + dr0["Job_No"] + "' and Job_date='" + dr0["Job_date"] + "' and icode='" + dr0["icode"] + "' and Item_Name='" + dr0["Item_Name"] + "' and Part_No='" + dr0["Part_No"] + "' and Job_Qty='" + dr0["Job_qty"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double prdtot = 0;

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["REJQTY"].ToString());
                                    tot = tot + fgen.make_double(dt1.Rows[i]["REJQTY"].ToString());
                                    prdtot = fgen.make_double(dt1.Rows[0]["prdqty"].ToString());
                                }
                                catch { }
                            }

                            drrow1["Ok_qty"] = prdtot.ToString("###,###,###,###.##");
                            drrow1["Tot_Rej"] = tot.ToString("###,###,###,###.##");
                            drrow1["Rej_perc"] = Math.Round(tot / (prdtot + tot) * 100, 2);

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["REJQTY_WT"].ToString());
                                }
                                catch { }
                            }
                            drrow1["Job_No"] = dt1.Rows[0]["Job_No"];
                            drrow1["Job_date"] = dt1.Rows[0]["Job_date"];
                            drrow1["Icode"] = dt1.Rows[0]["Icode"];
                            drrow1["Item_name"] = dt1.Rows[0]["Item_name"];
                            drrow1["Part_No"] = dt1.Rows[0]["Part_No"];
                            drrow1["Order_qty"] = dt1.Rows[0]["Order_qty"];
                            drrow1["Job_qty"] = dt1.Rows[0]["Job_qty"];
                            drrow1["Totl_Rej_Value"] = fgen.make_double(drrow1["Tot_Rej"].ToString()) * fgen.make_double(dr0["irate"].ToString());
                            dtm1.Rows.Add(drrow1);
                        }
                        dr2 = dtm1.NewRow();
                        d = 0;

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4) { }
                            else
                            {
                                foreach (DataRow drrr in dtm1.Rows)
                                {
                                    //total += fgen.return_double(drrr[dc.ToString()].ToString());
                                    //total += fgen.make_double(drrr[dc.ToString()].ToString());
                                    total += Math.Round(fgen.make_double(drrr[dc.ToString()].ToString()), 3);
                                }
                                string check = total.ToString("###,###,###,###.##");
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        //dr2["JOB_DATE"] = '-';
                        //dr2["MACHINE"] = '-';
                        dtm1.Rows.InsertAt(dr2, 0);
                        dtm1.Rows[0]["Rej_Perc"] = Math.Round((fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) / (fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) + fgen.make_double(dtm1.Rows[0]["OK_qty"].ToString()))) * 100, 2);

                        mq0 = "SELECT A.TYPE1,A.NAME FROM TYPE A WHERE A.ID='K'  AND TRIM(A.TYPE1) IN(SELECT DISTINCT STAGEC FROM ITWSTAGE) ORDER BY A.TYPE1";

                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtm1.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + " (NOS)";
                                }
                            }
                            if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtm1.Columns[abc].ColumnName = myname + " (KGS)";
                                }
                            }
                        }
                    }
                    Session["send_dt"] = dtm1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Job Wise All Stage Rejection For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "15163W":// wfinsys_erp id // RPTLEVEL HD
                case "F40325": //done
                    #region Item Wise All Stage Rejection
                    mq0 = "";
                    mq0 = "SELECT DISTINCT A.STAGE,(CASE WHEN LENGTH(TRIM(B.ADDR2))=2  THEN TRIM(B.ADDR2)  ELSE B.TYPE1 END ) AS SHRT FROM(SELECT DISTINCT branchcd ,ENQNO as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,icode, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='55'  and vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode union all sELECT  BRANCHCD,JOB_NO,JOB_DT,ICODE,trim(STAGE) as stage,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE  " + branch_Cd + " and TYPE='86' AND  vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE union all select branchcd,ENQNO AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where  " + branch_Cd + " and  type='60' and vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate)A,TYPE B WHERE TRIM(A.STAGE)=TRIM(B.TYPE1) AND TRIM(B.ID)='K' ORDER BY SHRT";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Job_No", typeof(string));
                    dtm1.Columns.Add("Job_Date", typeof(string));
                    dtm1.Columns.Add("Icode", typeof(string));
                    dtm1.Columns.Add("Item_Name", typeof(string));
                    dtm1.Columns.Add("Part_No", typeof(string));
                    dtm1.Columns.Add("Order_Qty", typeof(string));
                    dtm1.Columns.Add("Job_Qty", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(string));
                        dtm1.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(string));
                    }
                    dtm1.Columns.Add("OK_QTY", typeof(string));
                    dtm1.Columns.Add("Tot_Rej", typeof(string));
                    dtm1.Columns.Add("Rej_Perc", typeof(string));

                    dtraw = new DataTable();
                    dtraw.Columns.Add("Icode", typeof(string));
                    dtraw.Columns.Add("Item_Name", typeof(string));
                    dtraw.Columns.Add("Part_No", typeof(string));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtraw.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(double));
                        dtraw.Columns.Add("W".Trim() + dr[0].ToString().Trim(), typeof(double));
                    }
                    dtraw.Columns.Add("OK_QTY", typeof(string));
                    dtraw.Columns.Add("Tot_Rej", typeof(string));
                    dtraw.Columns.Add("Rej_Perc", typeof(string));

                    mq1 = "SELECT A.JOB_NO AS JOB_NO,A.JOB_DT as Job_date,trim(A.ICODE) as icode,TRIM(B.INAME) as Item_Name,B.CPARTNO as Part_No, to_char(B.QTYORD,'999,999,999,999') as Order_qty,to_char(B.JOB_QTY,'999,999,999,999') AS JOB_QTY,A.STAGE,to_char(SUM(A.PRDQTY),'999,999,999,999.99') AS PRDQTY ,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,to_char((SUM(A.PRDQTY)*B.BOXWT),'999,999,999,999') AS PRDQTY_WT,to_char((SUM(A.REJQTY)*B.BOXWT),'999,999,999,999.99') AS REJQTY_WT FROM( SELECT DISTINCT branchcd ,TRIM(ENQNO) as JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(icode) AS ICODE, '02' as stage,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime FROM (select BRANCHCD,ICODE,VCHNUM,VCHDATE,Machine_Name,SUM(PRDQTY) AS PRDQTY,sum(rejqty) as rejqty,sum(DOWNTIME) as downtime,max(ENQNO) as enqno,max(ENQDT) as enqdt from (SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,is_number(COL3) as REJQTY,0 AS DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE  " + branch_Cd + " and TYPE='45'  and vchdate " + xprdrange + " UNION ALL  SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(TITLE) AS Machine_Name,0 AS PRDQTY,0 AS REJQTY,IS_number(COL3) as DOWNTIME,NULL AS ENQNO,NULL AS ENQDT FROM inspvch WHERE " + branch_Cd + " and TYPE='55'  and  vchdate " + xprdrange + " UNION ALL SELECT BRANCHCD,ICODE,VCHNUM,VCHDATE,TRIM(COL25) as mc_name,QTY AS PRDQTY,0 AS REJQTY,0 AS DOWNTIME,ENQNO,ENQDT FROM COSTESTIMATE WHERE  " + branch_Cd + " and TYPE='40' and vchdate " + xprdrange + " ) group by BRANCHCD,ICODE,VCHNUM,MACHINE_NAME,VCHDATE) WHERE ENQNO IS NOT NULL  group by branchcd ,ENQNO,ENQDT,icode  union all sELECT  BRANCHCD,TRIM(JOB_NO) AS JOB_NO,JOB_DT,TRIM(ICODE) AS ICODE,STAGE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,sum(downtime) as downtime  FROM(SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO,JOB_DT,ICODE,SUM(PRDQTY) AS PRDQTY,SUM(REJQTY) AS REJQTY,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME,SUM(DOWNTIME) AS DOWNTIME FROM (SELECT BRANCHCD,TYPE,VCHNUM,VCHDATE,trim(JOB_NO) as JOB_NO,TRIM(JOB_DT) AS JOB_DT,TRIM(ICODE) AS ICODE,IQTYIN AS PRDQTY,MLT_LOSS AS REJQTY,STAGE,MCHCODE,PREVCODE AS SHIFT ,SHFTCODE,ENAME AS MCNAME,A.NUM1+A.NUM2+A.NUM3+A.NUM4+A.NUM5+A.NUM6+A.NUM7+A.NUM8+A.NUM9+A.NUM10+A.NUM11+A.NUM12+A.A11+A.A12+A.A13+A.A14+A.A15+A.A16+A.A17+A.A18+A.A19+A.A20 as downtime  FROM PROD_SHEET a WHERE " + branch_Cd + " and  TYPE='86' AND  vchdate " + xprdrange + " ) GROUP BY BRANCHCD,TYPE,VCHNUM,VCHDATE,JOB_NO, JOB_DT,ICODE,STAGE,MCHCODE,SHIFT ,SHFTCODE,MCNAME) group by BRANCHCD,JOB_NO,JOB_DT,ICODE,STAGE  union all select branchcd,TRIM(ENQNO) AS JOB_NO,TO_CHAR(ENQDT,'DD/MM/YYYY') AS JOB_DT,TRIM(ICODE) AS ICODE, '08' as stage,sum(is_number(COL4)) AS PRDQTY ,sum(is_number(COL5)) AS REJQTY,0 AS DOWNTIME  from costestimate  where " + branch_Cd + " and type='60'   and  vchdate " + xprdrange + " group by branchcd,enqno,enqdt,icode,convdate ) A,(SELECT DISTINCT  A.BRANCHCD,A.VCHNUM,A.VCHDATE,B.ORDNO,B.ORDDT,A.ACODE,A.ICODE,A.QTY AS JOB_QTY,B.QTYORD,C.INAME,C.CPARTNO,C.IWEIGHT,A.IRATE AS BOXWT FROM COSTESTIMATE A,SOMAS B,ITEM C WHERE A.TYPE='30' AND A.VCHDATE > SYSDATE-365 AND A.CONVDATE=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.ORDNO)||TO_CHAR(B.ORDDT,'DD/MM/YYYY')||TRIM(B.SRNO) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A." + branch_Cd + " )B WHERE TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.JOB_NO)=TRIM(B.VCHNUM) AND TRIM(A.JOB_DT)=TO_CHAR(B.VCHDATE,'DD/MM/YYYY') GROUP BY A.JOB_NO,A.JOB_DT,A.ICODE,B.INAME,B.QTYORD,B.JOB_QTY,A.STAGE,B.CPARTNO,B.BOXWT ORDER BY ICODE";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Job_No", "Job_date", "Icode", "Item_Name", "Part_No", "Job_qty");
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "Job_No='" + dr0["Job_No"] + "' and Job_date='" + dr0["Job_date"] + "' and icode='" + dr0["icode"] + "' and Item_Name='" + dr0["Item_Name"] + "' and Part_No='" + dr0["Part_No"] + "' and Job_Qty='" + dr0["Job_qty"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            double prdtot = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["REJQTY"].ToString());
                                    tot = tot + fgen.make_double(dt1.Rows[i]["REJQTY"].ToString());
                                    prdtot = fgen.make_double(dt1.Rows[0]["prdqty"].ToString());
                                }
                                catch { }
                            }

                            drrow1["Ok_qty"] = prdtot.ToString("###,###,###,###.##");
                            drrow1["Tot_Rej"] = tot.ToString("###,###,###,###.##");
                            drrow1["Rej_perc"] = Math.Round(tot / (prdtot + tot) * 100, 2);

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                string mm = dt1.Rows[i]["STAGE"].ToString().Trim();
                                try
                                {
                                    drrow1["W" + mm] = fgen.make_double(dt1.Rows[i]["REJQTY_WT"].ToString());
                                }
                                catch { }
                            }
                            drrow1["Job_No"] = dt1.Rows[0]["Job_No"];
                            drrow1["Job_date"] = dt1.Rows[0]["Job_date"];
                            drrow1["Icode"] = dt1.Rows[0]["Icode"];
                            drrow1["Item_name"] = dt1.Rows[0]["Item_name"];
                            drrow1["Part_No"] = dt1.Rows[0]["Part_No"];
                            drrow1["Order_qty"] = dt1.Rows[0]["Order_qty"];
                            drrow1["Job_qty"] = dt1.Rows[0]["Job_qty"];
                            dtm1.Rows.Add(drrow1);
                        }
                        ///////////////////////

                        dtm1.Columns.Remove("job_no");
                        dtm1.Columns.Remove("job_date");
                        dtm1.Columns.Remove("order_qty");
                        dtm1.Columns.Remove("job_qty");
                        double sum = 0;
                        view1im = new DataView(dtm1);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "Icode");

                        foreach (DataRow dr in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dtm1, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            DataRow drraw = dtraw.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                sum = 0;
                                if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2)
                                {
                                    drraw["icode"] = dt1.Rows[0]["icode"].ToString();
                                    drraw["Item_Name"] = dt1.Rows[0]["Item_Name"].ToString();
                                    drraw["PART_NO"] = dt1.Rows[0]["PART_NO"].ToString();
                                }
                                else
                                {
                                    foreach (DataRow drr in dt1.Rows)
                                    {
                                        sum += fgen.make_double(drr[dc].ToString());
                                    }
                                    drraw[dc.ColumnName] = sum;
                                }
                            }
                            dtraw.Rows.Add(drraw);
                        }
                        dr2 = dtraw.NewRow();
                        d = 0;

                        //////////////////////////////////////

                        //dr2 = dtm1.NewRow();
                        //d = 0;
                        string check;
                        foreach (DataColumn dc in dtraw.Columns)
                        {
                            double total = 0;

                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2) { }
                            else
                            {
                                foreach (DataRow drrr in dtraw.Rows)
                                {
                                    total += fgen.make_double(drrr[dc.ToString()].ToString());
                                }
                                if (total > 0)
                                {
                                    check = total.ToString("###,###,###,###.##");
                                }
                                else
                                {
                                    check = "0";
                                }
                                dr2[dc] = check;
                                //dr2[dc] = total;
                            }
                        }
                        //dr2["JOB_DATE"] = '-';
                        //dr2["MACHINE"] = '-';
                        dtraw.Rows.InsertAt(dr2, 0);
                        dtraw.Rows[0]["Rej_Perc"] = Math.Round((fgen.make_double(dtraw.Rows[0]["tot_rej"].ToString()) / (fgen.make_double(dtm1.Rows[0]["tot_rej"].ToString()) + fgen.make_double(dtraw.Rows[0]["OK_qty"].ToString()))) * 100, 2);

                        mq0 = "SELECT A.TYPE1,A.NAME FROM TYPE A WHERE A.ID='K' AND TRIM(A.TYPE1) IN(SELECT DISTINCT STAGEC FROM ITWSTAGE) ORDER BY A.TYPE1";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, mq0);

                        foreach (DataColumn dc in dtraw.Columns)
                        {
                            int abc = dc.Ordinal;
                            string rejtype = dc.ToString().Substring(0, 1);
                            if (rejtype == "R")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtraw.Columns[abc].ColumnName = myname + " (NOS)";
                                }
                            }
                            if (rejtype == "W")
                            {
                                string name = dc.ToString().Remove(0, 1);
                                string myname = fgen.seek_iname_dt(dt, "type1='" + name + "'", "name");
                                if (myname != "0")
                                {
                                    dtraw.Columns[abc].ColumnName = myname + " (KGS)";
                                }
                            }
                        }
                    }
                    Session["send_dt"] = dtraw;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("Item Wise All Stage Rejection For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    #endregion
                    break;

                case "15219":// wfinsys_erp id
                case "F40326": //comma done
                    #region Physical Verification RM
                    cond = "SP";
                    cond = "RV";

                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    mq1 = "SELECT * FROM (select TRIM(A.icode) AS ICODE,B.INAME AS ITEM_NAME,sum(A.opening)+sum(A.cdr)-sum(A.ccr) as  BOOK_BAL,0 AS PHY_BAL from (Select A.branchcd,TRIM(A.icode) AS ICODE, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,b.iopbal as opval,0 as inval,0 as outval,0 as clval from itembal a,item b  where trim(a.icode)=trim(b.icode) and a." + branch_Cd + "    union all select branchcd,TRIM(icode) AS ICODE,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) -sum(iqtyout*ichgs) as opval,0 as inval,0 as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY TRIM(ICODE) ,branchcd,type union all select branchcd,TRIM(icode) AS ICODE,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos, 0 as opval,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) as inval,sum(iqtyout*ichgs) as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + " and type like '%'   and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type ) A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1)='9' group by A.branchcd,A.icode,B.INAME) WHERE BOOK_BAL<>0";
                    mq2 = "SELECT A.ICODE,NULL AS INAME,0 AS BOOK_BAL,SUM(A.IQTYIN) AS PHY_BAL FROM (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.MAINCODE,A.IQTYIN FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and trim(type)='" + cond + "' ) A GROUP BY A.ICODE";
                    SQuery = "SELECT ICODE,MAX(ITEM_NAME) AS ITEM_NAME,SUM(BOOK_BAL) AS BOOK_BAL,SUM(PHY_BAL) AS PHY_BAL FROM (" + mq1 + " UNION ALL " + mq2 + ") GROUP BY ICODE ";
                    mq4 = "SELECT A.VCHNUM,to_char(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ICODE,maincode as Roll_Code ,count(A.MAINCODE) as cnt FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and type='" + cond + "' group BY VCHNUM,VCHDATE,ICODE,MAINCODE HAVING COUNT(MAINCODE)>1 ORDER BY ICODE,MAINCODE";

                    mq1 = "SELECT * FROM (select TRIM(A.icode) AS ICODE,B.INAME AS ITEM_NAME,sum(A.opening)+sum(A.cdr)-sum(A.ccr) as  BOOK_BAL,0 AS PHY_BAL from (Select A.branchcd,TRIM(A.icode) AS ICODE, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,b.iopbal as opval,0 as inval,0 as outval,0 as clval from itembal a,item b  where trim(a.icode)=trim(b.icode) and a." + branch_Cd + "    union all select branchcd,TRIM(icode) AS ICODE,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) -sum(iqtyout*ichgs) as opval,0 as inval,0 as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY TRIM(ICODE) ,branchcd,type union all select branchcd,TRIM(icode) AS ICODE,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos, 0 as opval,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) as inval,sum(iqtyout*ichgs) as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + " and type like '%'   and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type ) A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1)!='9' group by A.branchcd,A.icode,B.INAME) WHERE BOOK_BAL<>0";
                    mq2 = "SELECT A.ICODE,NULL AS INAME,0 AS BOOK_BAL,SUM(A.IQTYIN) AS PHY_BAL FROM (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.MAINCODE,A.IQTYIN FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and trim(type)='" + cond + "' ) A GROUP BY A.ICODE";
                    SQuery = "SELECT ICODE,MAX(ITEM_NAME) AS ITEM_NAME,to_char(SUM(BOOK_BAL),'999,999,999,999') AS BOOK_BAL,to_char(SUM(PHY_BAL),'999,999,999,999') AS PHY_BAL FROM (" + mq1 + " UNION ALL " + mq2 + ") WHERE SUBSTR(TRIM(ICODE),1,1)!='9'  GROUP BY ICODE HAVING SUM(PHY_BAL)<>'0' ";
                    mq4 = "SELECT A.VCHNUM,to_char(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ICODE,maincode as Roll_Code ,count(A.MAINCODE) as count FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and type='" + cond + "' AND SUBSTR(TRIM(ICODE),1,1)!='9' group BY VCHNUM,VCHDATE,ICODE,MAINCODE HAVING COUNT(MAINCODE)>1 ORDER BY ICODE,MAINCODE";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, co_cd, mq4);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    SQuery = mq4;
                    //    mq1 = "Warning !! Repeated Records Found Please Correct this First";
                    //}
                    //else
                    { mq1 = "Physical Verification RM For the Period " + fromdt + " To " + todt + ""; }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;
                case "F40326R":
                case "F40326M":
                    #region Physical Verification REEL
                    cond = "SP";
                    cond = "RV";

                    string r40 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R40'", "params");
                    if (r40.Length > 5)
                        r40 = " and vchdate >= to_date('" + r40 + "','dd/mm/yyyy')";
                    else r40 = "";
                    xprdrange1 = " between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') " + r40;

                    SQuery = "select a.icode as erpcode,d.iname as product,d.cpartno as part_no,a.kclreelno,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance,bk_bal as phy_stock from (select trim(icode) as icode,trim(kclreelno) as kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout,sum(bk_bal) as bk_bal from (select icode,kclreelno,reelwin,reelwout,0 as bk_bal from (select branchcd,icode,kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout from (select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " union all select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout from reelvch_op where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " ) group by branchcd,icode,kclreelno  having sum(reelwin)-sum(reelwout)>0) union all select trim(icode) as icode,trim(maincode) as kclreelno,0 as reelwin,0 as reelqout,iqtyin as bk_bal from wipstk where branchcd='" + mbr + "' and type='RV' and vchdate " + xprdrange + ") group by trim(icode),trim(kclreelno) ) a,item d where trim(a.icode)=trim(d.icodE) order by erpcode";
                    mq1 = "Physical Verification Reel For the Period " + fromdt + " To " + todt + "";
                    if (val == "F40326M")
                    {
                        SQuery = "select a.icode as erpcode,d.iname as product,d.cpartno as part_no,a.kclreelno,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance,bk_bal as phy_stock from (select trim(icode) as icode,trim(kclreelno) as kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout,sum(bk_bal) as bk_bal from (select icode,kclreelno,reelwin,reelwout,0 as bk_bal from (select branchcd,icode,kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout from (select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " union all select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout from reelvch_op where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " ) group by branchcd,icode,kclreelno  having sum(reelwin)-sum(reelwout)>0) union all select trim(icode) as icode,trim(maincode) as kclreelno,0 as reelwin,0 as reelqout,iqtyin as bk_bal from wipstk where branchcd='" + mbr + "' and type='RV' and vchdate " + xprdrange + ") group by trim(icode),trim(kclreelno) ) a,item d where trim(a.icode)=trim(d.icodE) and bk_bal<=0 order by erpcode";
                        mq1 = "Missing Reel (Not Physical Verified) For the Period " + fromdt + " To " + todt + "";
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    break;
                case "15219A": // wfinsys_erp id
                case "F40327": //done
                    #region Physical Verification FG
                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";

                    mq1 = "SELECT * FROM (select TRIM(A.icode) AS ICODE,B.INAME AS ITEM_NAME,NULL AS CATG,sum(A.opening)+sum(A.cdr)-sum(A.ccr) as  BOOK_BAL,0 AS PHY_BAL from (Select A.branchcd,TRIM(A.icode) AS ICODE, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,b.iopbal as opval,0 as inval,0 as outval,0 as clval from itembal a,item b  where trim(a.icode)=trim(b.icode) and a." + branch_Cd + "    union all select branchcd,TRIM(icode) AS ICODE,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) -sum(iqtyout*ichgs) as opval,0 as inval,0 as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY TRIM(ICODE) ,branchcd,type union all select branchcd,TRIM(icode) AS ICODE,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos, 0 as opval,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) as inval,sum(iqtyout*ichgs) as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + " and type like '%'   and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type ) A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1)='9' group by A.branchcd,A.icode,B.INAME) WHERE BOOK_BAL<>0";
                    mq2 = "SELECT A.ICODE,MAX(B.INAME) AS INAME,A.WOLINK AS CATG,0 AS BOOK_BAL,SUM(A.IQTYIN) AS PHY_BAL FROM (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.MAINCODE,A.IQTYIN,A.WOLINK FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and trim(type)='FP' ) A ,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.ICODE,A.WOLINK";
                    SQuery = "SELECT ICODE,MAX(ITEM_NAME) AS ITEM_NAME,MAX(CATG) AS CATG,SUM(BOOK_BAL) AS BOOK_BAL,SUM(PHY_BAL) AS PHY_BAL FROM (" + mq1 + " UNION ALL " + mq2 + ") GROUP BY ICODE HAVING SUM(PHY_BAL)<>'0'";
                    mq4 = "SELECT TRIM(A.VCHNUM) AS VCHNUM,to_char(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE ,maincode AS Roll_code ,count(A.MAINCODE) as cnt FROM WIPSTK A where BRANCHCD ='" + mbr + "' and vchdate " + xprdrange + " and type='FP' group BY TRIM(VCHNUM),VCHDATE,TRIM(ICODE),MAINCODE HAVING COUNT(MAINCODE)>1 ORDER BY ICODE,MAINCODE";

                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, co_cd, mq4);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    SQuery = mq4;
                    //    mq1 = "Warning !! Repeated Records Found Please Correct this First";
                    //}
                    //else 

                    { mq1 = "Physical Verification FG For the Period " + fromdt + " To " + todt + ""; }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;

                case "22610B": // wfinsys_erp id // RPTLEVEL HD
                case "F40328": //Comma done
                    #region FG Item Wise Location Wise Stock / Location Wise FG Item Wise Stock
                    mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    // mq0 = hfcode.Value;
                    mq1 = "FG Item Wise Location Wise Stock";
                    SQuery = "SELECT TRIM(B.INAME) AS PRODUCT,TRIM(B.CPARTNO) AS PART_NO,B.UNIT,TRIM(A.ICODE) AS ERPCODE,TRIM(TO_CHAR(A.TOT,'999,999,999,999')) AS BAL,replace(nvl(A.RLOCN,'-'),'-','-') as R_LOCN FROM (select sum(nvl(iqtyin,0))-sum(replace(nvl(st_modv,0),'-',0)) as tot,TRIM(icode) AS ICODE,trim(ordlineno) as rlocn from ivoucher where BRANCHCD='" + mbr + "' AND type='16' and store='Y' and vchdate " + xprdrange + "  group by icode,trim(ordlineno) ) a ,item b WHERE trim(a.icode)=trim(b.icode) order by a.icode";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;

                case "F40328R":
                    mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    // mq0 = hfcode.Value;
                    mq1 = "RM Item Wise Location Wise Stock";
                    SQuery = "SELECT TRIM(B.INAME) AS PRODUCT,TRIM(B.CPARTNO) AS PART_NO,B.UNIT,TRIM(A.ICODE) AS ERPCODE,TRIM(TO_CHAR(A.TOT,'999,999,999,999')) AS Qty,replace(nvl(A.RLOCN,'-'),'-','-') as Location,BTCHNO AS BATCH_Number FROM (select sum(nvl(iqtyin,0))-sum(replace(nvl(st_modv,0),'-',0)) as tot,TRIM(icode) AS ICODE,trim(CCENT) as rlocn,BTCHNO from ivoucher where BRANCHCD='" + mbr + "' AND type like '0%' and vchdate " + xprdrange + "  group by icode,trim(CCENT),BTCHNO) a ,item b WHERE trim(a.icode)=trim(b.icode) order by a.icode";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    break;

                case "15219B":// wfinsys_erp id
                case "F40332": //Comma done
                    #region Physical Verification Records
                    SQuery = "SELECT TRIM(A.BRANCHCD) AS BRANCHCD ,TRIM(A.TYPE) AS TYPE,TRIM(VCHNUM) AS VOUCHER_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VOUCHER_DT,TRIM(A.ICODE) AS ITEM_CODE,TRIM(B.INAME) AS ITEM_NAME,TRIM(a.MAINCODE) AS MAINCODE ,trim(A.WOLINK) AS Category,TRIM(TO_CHAR(IQTYIN,'999,999,999,999')) AS PHY_BALANCE FROM WIPSTK A,ITEM B  WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE='RV' AND A.VCHDATE " + xprdrange + "  AND TRIM(A.ICODE)=TRIM(B.ICODE) ORDER BY A.VCHDATE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Physical Verification Records For the Period " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;
                //-----------------------------------------------------end here
                    #endregion

                #region 04/09/2018  by akshay
                case "F40333"://sale Projection VS Production Qty
                    SQuery = "Select TRIM(c.iname) as Product,TRIM(c.cpartno) as Part_no,trim(a.Icode) as ERP_icode,c.unit ,to_char(sum(target),'999,999,999,999.99') as Proj_qty,to_char(sum(Qty),'999,999,999,999.99') as Prodn_qty,to_char((sum(target)-sum(Qty)),'999,999,999,999.99') as GAP_Qty from (Select Icode,target,0 as Qty,0 as irate from mthlyplan where branchcd='" + mbr + "' and type='25' and vchdate " + xprdrange + " union all Select Icode,0 as target,iqtyin as Qty,irate from ivoucher where branchcd='" + mbr + "' and type in ('15','16','17') and vchdate " + xprdrange + " and icode like '9%') a , item c where trim(A.icode)=trim(c.icode) group by c.iname,c.unit,c.cpartno,trim(a.Icode) order by C.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Projection vs Production Qty For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40334": //Job Order not planned
                    SQuery = "Select TRIM(X.maker) as Machine,TRIM(x.Item_Name) as Item_name,X.icode,TO_CHAR(X.dated,'DD/MM/YYYY') as Job_dt,X.Job_no,trim(x.cpartno) as Part_No,TO_CHAR(x.delv_Dt,'DD/MM/YYYY') AS delv_Dt,TO_CHAR(X.qty,'999,999,999,999.99') as Job_qty,x.COMMENTS2 as Job_Type,x.status,x.acode,x.convdate as fstr from (select distinct B.INAME as Item_Name,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Qty,a.icode,b.Cpartno,decode(a.Status,'Y','Closed by User','Job Card Open') as Status,a.acode,a.convdate,a.COMMENTS2,b.maker,a.enqdt as Delv_Dt  from costestimate A,ITEM B  WHERE trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + mbr + "' and a.type='30' and a.ENQNO!='Y' and a.vchdate " + xprdrange + " AND  A.vchnum<>'000000' and a.status<>'Y' ) x where (X.Job_no,X.dated) not in (Select distinct trim(job_no) as job_no,to_Date(job_Dt,'dd/mm/yyyy') as dated from prod_Sheet where branchcd='" + mbr + "' and type='90' and vchdate >=to_date('" + fromdt + "','dd/mm/yyyy') ) and upper(x.cpartno) not like '%CORR%' and upper(x.Item_Name) not like '%SLIP%' order by X.dated desc ,X.Job_no desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Job Order not planned For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "need to merge in smktg rep on F47166": //Sales projection vs sales qty
                    SQuery = "Select TRIM(c.iname) as Product,TRIM(c.cpartno) as Part_no,trim(a.Icode) as ERP_icode,TO_CHAR(sum(target),'999,999,999,999.99')  as Proj_qty,TO_CHAR(sum(Qty),'999,999,999,999.99')  as Sold_qty,TO_CHAR((sum(target)-sum(Qty)),'999,999,999,999.99')  as GAP_Qty,TO_CHAR(((sum(target)-sum(Qty))*max(a.irate)),'999,999,999,999.99')  as Gap_Val from (Select Icode,target,0 as Qty,0 as irate from mthlyplan where branchcd='" + mbr + "' and type='25' and vchdate " + xprdrange + " union all Select Icode,0 as target,iqtyout as Qty,irate from ivoucher where branchcd='" + mbr + "' and type like '4%' and type not in ('45','47') and vchdate " + xprdrange + "  and icode like '9%') a , item c where trim(A.icode)=trim(c.icode) group by c.iname,c.cpartno,trim(a.Icode) order by C.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Projection vs Sales Qty For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40335": //Delivery Plan dated (all)
                    SQuery = "SELECT * FROM (Select trim(nvl(c.maker,'-'))||':'||a.socat  as Category,TRIM(B.aname) as Customer,TRIM(C.iname) as Item,TRIM(C.cpartno) as Partno ,TO_CHAR(a.dlv_date,'DD/MM/YYYY') as Delv_date,TO_CHAR(BUDGETCOST,'999,999,999,999.99') as Delv_Qty,TO_CHAR(ACTUALCOST,'999,999,999,999.99') as Print_Qty,TRIM(A.ICODE) AS ICODE ,trim(a.solink)||trim(a.srno) as solink,a.SoRemarks,TO_CHAR(a.jobcardqty,'999,999,999,999.99') AS jobcardqty ,a.jobcardno,TO_CHAR(a.dlv_date,'DD/MM/YYYY') as delvdt,a.rowid as Iden,a.jobcardrqd,Req_Closedby,a.vchnum as Ordno,TO_CHAR(a.vchdate,'DD/MM/YYYY') as Orddt,a.app_dt from budgmst a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='46' and 1=1 and  a.dlv_Date  " + xprdrange + "  and trim(a.acode) like '%' and trim(a.icode) like '%' and a.socat like '%' and a.jobcardrqd='Y' and a.ACTUALCOST>0 and 1=1 order by a.dlv_date,B.aname,c.CPARTNO) WHERE trim(icode)||SUBSTR(solink,1,20) NOT IN (sELECT trim(icode)||BRANCHCD||TYPE||ORDNO||TO_CHAR(ORDDT,'DD/MM/YYYY') FROM SOMAS WHERE BRANCHCD='" + mbr + "' AND type like '4%' and trim(ICAT)='Y' )";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Delivery Plan Dated For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40336": //Delivery Plan dated(pending)
                    SQuery = "SELECT * FROM ( Select trim(nvl(c.maker,'-'))||':'||a.socat  as Category,TRIM(B.aname) as Customer,TRIM(C.iname) as Item,TRIM(C.cpartno) as Partno ,to_char(a.dlv_date,'dd/mm/yyyy') as Delv_date,TO_CHAR(BUDGETCOST,'999,999,999,999.99') as Delv_Qty,TO_CHAR(ACTUALCOST,'999,999,999,999.99') as Print_Qty,TRIM(a.icode) AS icode,trim(a.solink)||trim(a.srno) as solink,a.SoRemarks,TO_CHAR(a.jobcardqty,'999,999,999,999.99') AS jobcardqty,a.jobcardno,TO_CHAR(a.dlv_date,'DD/MM/YYYY') as delvdt,a.rowid as Iden,a.jobcardrqd,Req_Closedby,a.vchnum as Ordno,TO_CHAR(a.vchdate,'DD/MM/YYYY') as Orddt,TO_CHAR((a.ACTUALCOST-a.jobcardqty),'999,999,999,999.99') as Balance_job,substr(a.jobcardno,6) as job_Cardno,a.app_dt from budgmst a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type='46' and 1=1 and a.dlv_Date " + xprdrange + "  and trim(a.acode) like '%' and trim(a.icode) like '%' and a.socat like '%' and a.ACTUALcost>a.jobcardqty and a.jobcardrqd='Y' and a.ACTUALCOST>0 and length(Trim(nvl(a.req_Closedby,'-')))<=1 and 1=1 order by a.dlv_date,B.aname,c.CPARTNO ) WHERE trim(icode)||SUBSTR(solink,1,20) NOT IN ( sELECT trim(icode)||BRANCHCD||TYPE||ORDNO||TO_CHAR(ORDDT,'DD/MM/YYYY') FROM SOMAS WHERE BRANCHCD='" + mbr + "' AND type like '4%' and trim(ICAT)='Y' )";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Delivery Plan Dated(pending) For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;
                #endregion

                #region
                case "F40337": //Shift Wise Production Rejection
                    SQuery = "Select TRIM(OBSV15) as Shift_Name,TRIM(Title) as Machine,col1 as Rejn_Reason,TO_CHAR(sum(NVL(qty8,0)),'999,999,999,999.99') as Rej_Qty from inspvch  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + " group by Obsv15,Title,col1 order by Obsv15,col1,title ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Shift Wise Production Rejection For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40338": //Production vs Despatch Report
                    SQuery = "Select TRIM(b.iname) AS INAME ,TRIM(b.cpartno) AS PARTNO,TO_CHAR(sum(NVL(a.iqtyin,0)),'999,999,999,999.99') as Corr_prodn,TO_CHAR(sum(NVL(a.snp,0)),'999,999,999,999.99') as Sort_pack,TO_CHAR(sum(NVL(a.sales,0)),'999,999,999,999.99') as Sold_qty,trim(a.icode)As ERP_Code from (Select icode,iqtyin,0 as snp,0 as sales from prod_sheet where branchcd='" + mbr + "' and type='88' and vchdate " + xprdrange + " union all Select icode,0 as iqtyin,iqtyin as snp,0 as sales from ivoucher where branchcd='" + mbr + "' and type='16' and vchdate " + xprdrange + " union all Select icode,0 as iqtyin,0 as snp,iqtyout as sales from ivoucher where branchcd='" + mbr + "' and type like '4%' and vchdate " + xprdrange + " and type not in ('45','47'))a,item b where trim(A.icode)=trim(B.icode) group by b.iname,b.cpartno,trim(A.icode) order by b.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production vs Despatch Report For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40339": //Daily Issuance vs. Consumption
                    SQuery = "SElect TRIM(b.Iname) AS ITEM_NAME ,TO_CHAR(a.vchdate,'DD/MM/YYYY') AS VOUCHER_DATE,a.Sub_grp AS SUB_GROUP,TO_CHAR(sum(a.iqtyout),'999,999,999,999.99') as Qty_Issue,TO_CHAR(sum(a.qused),'999,999,999,999.99') as Qty_used,TO_CHAR(sum(a.qretu),'999,999,999,999.99') as Qty_Retu,TO_CHAR(((sum(a.iqtyout)-sum(a.qretu))-sum(a.qused)),'999,999,999,999.99') as Floor_Wip from (Select vchdate,substr(icode,1,4) as Sub_Grp,iqtyout,0 as qused,0 as qretu from ivoucher where branchcd='" + mbr + "' and type='31' and vchdate " + xprdrange + " and icode like '07%' union all Select vchdate,substr(icode,1,4),0 as iqtyout,itate as qused,0 as qretu from costestimate where branchcd='" + mbr + "' and type='25' and vchdate " + xprdrange + " and icode like '07%' union all Select vchdate,substr(icode,1,4),0 as iqtyout,0 as qused,iqtyin as qretu from ivoucher where branchcd='" + mbr + "' and type='11' and vchdate " + xprdrange + "  and icode like '07%')a, item b where trim(A.sub_Grp)=trim(B.icode) group by a.vchdate,a.Sub_grp,b.iname order by a.vchdate,a.Sub_grp";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Daily Issuance vs. Consumption For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40340": //Job Wise Wastage Report
                    SQuery = "Select TRIM(b.Iname) AS INAME,TRIM(a.icode) AS ICODE ,TRIM(b.cpartno) AS PART_NO,TO_CHAR(a.qty,'999,999,999,999.99') AS qTY,TO_CHAR(a.scrp1,'999,999,999,999.99') as GSM_Var,TO_CHAR(NVL(a.scrp2,0),'999,999,999,999.99') as Fala,TO_CHAR(a.TIME1,'999,999,999,999.99') as Tore,TO_CHAR(a.time2,'999,999,999,999.99') as Core,TO_CHAR((a.scrp2+a.TIME1+a.time2),'999,999,999,999.99') as Totl_wstg,TRIM(a.enqno) as Job_no,TO_CHAR(a.enqdt,'DD/MM/YYYY') as Job_dt,TO_CHAR(a.vchdate,'DD/MM/YYYY') AS VCHDATE,TRIM(a.vchnum) AS VCHNUM from costestimate a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type='40' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Job Wise Wastage Report For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40341": //Capacity vs. production
                    SQuery = "select TO_CHAR(a.vchdate,'DD/MM/YYYY') AS VCHDATE,TRIM(a.vchnum) AS VCHNUM,TRIM(a.icode) AS ICODE,TO_CHAR((a.iqtyin*b.iweight),'999,999,999,999.99') as prodn,TO_CHAR(0,'999,999,999,999.99') as rejqty,TO_CHAR(a.iqtyin,'999,999,999,999.99') as Prodq,TO_CHAR(0,'999,999,999,999.99') as rejq,TO_CHAR(b.iweight,'999,999,999.999') AS IWEIGHT  from prod_Sheet a,item b  where trim(A.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type='88' and a.VCHDATE " + xprdrange + "  union all select TO_CHAR(a.vchdate,'DD/MM/YYYY') AS VCHDATE,a.vchnum,a.icode,TO_CHAR(0,'999,999,999,999.99') as prodn,TO_CHAR((a.qty8*b.iweight),'999,999,999,999.99') as rejqty,TO_CHAR(0,'999,999,999,999.99') as prodq,TO_CHAR(a.qty8,'999,999,999,999.99') as Rej_Qty,TO_CHAR(b.iweight,'999,999,999.999') AS WEIGHT  from inspvch a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + mbr + "' and a.type='45' and a.VCHDATE " + xprdrange + " ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Capacity vs. production For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40342": //Plan vs. production
                    SQuery = "select A.vchdate,TO_CHAR(sum(a.iqtyout),'999,999,999,999.99') as Plan_qty,TO_CHAR(sum(a.prodn),'999,999,999,999.99') as Prod0n_Qty,TO_CHAR((sum(a.iqtyout)-sum(a.prodn)),'999,999,999,999.99') as Short_prdn from (select TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as job_dt,icode,iqtyout,0 as prodn  from prod_Sheet where branchcd='" + mbr + "' and type='90' and VCHDATE " + xprdrange + " union all select TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as vchdate,icode,0 as iqtyout,iqtyin as prodn from prod_Sheet where branchcd='" + mbr + "' and type='88' and VCHDATE " + xprdrange + " ) a group by A.vchdate order by a.vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Plan vs. production For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40355": // corr wt summary
                    SQuery = "Select to_Char(a.vchdate,'YYYY MONTH') as Month_Name,a.col25 as Machine,round(sum(a.qty),0) as Prodn_Qty,round(sum(a.qty*b.iweight)/1000,2) as Prodn_Tons,to_Char(a.vchdate,'YYYYMM') as Mth_char from costestimate a, item b where trim(A.icode)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type='40' and a.vchdate " + xprdrange + " group by a.col25,to_Char(a.vchdate,'YYYY MONTH'),to_Char(a.vchdate,'YYYYMM') order by to_Char(a.vchdate,'YYYYMM'),col25";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Trend of Prodn For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40352": // job wise issue , consumption
                    //SQuery = "select A.vchdate,TO_CHAR(sum(a.iqtyout),'999,999,999,999.99') as Plan_qty,TO_CHAR(sum(a.prodn),'999,999,999,999.99') as Prod0n_Qty,TO_CHAR((sum(a.iqtyout)-sum(a.prodn)),'999,999,999,999.99') as Short_prdn from (select TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as job_dt,icode,iqtyout,0 as prodn  from prod_Sheet where branchcd='" + mbr + "' and type='90' and VCHDATE " + xprdrange + " union all select TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,job_no as vchnum,to_DaTE(job_Dt,'dd/mm/yyyy') as vchdate,icode,0 as iqtyout,iqtyin as prodn from prod_Sheet where branchcd='" + mbr + "' and type='88' and VCHDATE " + xprdrange + " ) a group by A.vchdate order by a.vchdate";

                    SQuery = "Select null as fgc,invno as job_no,vchdate,iqtyout,0 as qused,0 as qretu,0 as wstg from ivoucher where branchcd='" + mbr + "' and type='31' and vchdate " + xprdrange + " and icode like '07%' union all Select null as fgc,enqno as job_no,vchdate,0 as iqtyout,itate as qused,0 as qretu,0 as wstg from costestimate where branchcd='" + mbr + "' and type='25' and vchdate " + xprdrange + " and icode like '07%' "
                    + " union all Select icode,vchnum as job_no,vchdate,0 as iqtyout,0 as qused,0 as qretu,0 as wstg from costestimate where branchcd='" + mbr + "' and type='30' and vchdate " + xprdrange + " and srno=1 union all Select null as fgc,enqno as job_no,vchdate,0 as iqtyout,0 as qused,0 as qretu,nvl(scrp1,0)+nvl(scrp2,0)+nvl(time1,0)+nvl(time2,0) as wstg from costestimate where branchcd='" + mbr + "' and type='40' and vchdate " + xprdrange + " union all Select null as fgc,invno,vchdate,0 as iqtyout,0 as qused,iqtyin as qretu,0 as wstg from ivoucher where branchcd='" + mbr + "' and type='11' and vchdate " + xprdrange + " and icode like '07%'";
                    SQuery = "Select max(fgc) as FG_Code,trim(a.job_no) as job_no,sum(a.iqtyout)as Qty_Issue,sum(a.qused)as Qty_used,sum(a.wstg)as wstg,sum(a.qretu) as Qty_Retu,(sum(a.iqtyout)-sum(a.qretu))-sum(a.qused)-sum(a.wstg)as Floor_Wip from (" + SQuery + ")a  group by trim(a.job_no) having sum(a.iqtyout)>0 order by trim(a.job_no) ";
                    SQuery = "select b.Iname,a.job_no,a.Qty_Issue,a.Qty_used,a.Wstg,a.Qty_Retu,a.Floor_Wip,a.FG_Code from (" + SQuery + ") a,item b where trim(A.FG_Code)=trim(b.icode) order by Job_no";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Job Wise Issue, Consumption For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F40343": //Production vs. Completion
                    SQuery = "select TRIM(b.iname) AS INAME,TRIM(b.cpartno) AS PARTNO,a.job_no,TO_CHAR(a.Dated,'DD/MM/YYYY') AS DATED,TO_CHAR(sum(a.Job_Qty),'999,999,999,999.99') as Job_qty,TO_CHAR(sum(a.prodn),'999,999,999,999.99') as Prodn_qty,(Case when sum(a.Job_Qty)>0 and sum(a.prodn)>0 then round((sum(a.prodn)/sum(a.job_Qty))*100,2) else 0 end) as Completion, TO_CHAR(MAX(A.PRODDT),'DD/MM/YYYY') AS PROD_DT,a.erp_Code,a.acode from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE a.branchcd='" + mbr + "' and a.type='30' and a.vchdate " + xprdrange + " and A.SRNO=0 AND  trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + mbr + "' and a.type='60' AND  a.vchdate " + xprdrange + ")a, item b where trim(A.erp_Code)=trim(B.icode) group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production vs. Completion For the Period" + fromdt + " To " + todt, frm_qstr);
                    break;


                //////////////////27 sep...........yogita

                case "F40348":
                    mq0 = "SELECT DISTINCT to_Char(vchdate,'YYYYMONTH')||TRIM(Title) AS FSTR,'-' as gstr,to_Char(vchdate,'YYYYMONTH') as Month_Name,TO_CHAR(sum(qty8),'999,999,999') AS MINS_LOST FROM inspvch  where branchcd='" + mbr + "' and type='55' and vchdate  " + xprdrange + " GROUP BY to_Char(vchdate,'YYYYMONTH')||TRIM(Title),to_Char(vchdate,'YYYYMONTH')  order by to_Char(vchdate,'YYYYMONTH') asc";
                    mq1 = "SELECT DISTINCT to_Char(vchdate,'DD/MONTH/YYYY')||TRIM(obsv15) AS FSTR, to_Char(vchdate,'YYYYMONTH')||TRIM(Title) AS GSTR,to_Char(vchdate,'DD/MONTH/YYYY') as Month_Name,trim(obsv15) as shift,TO_CHAR(sum(qty8),'999,999,999') AS MINS_LOST from inspvch  where branchcd='" + mbr + "' and type='55' and vchdate " + xprdrange + " GROUP BY  to_Char(vchdate,'DD/MONTH/YYYY')||TRIM(obsv15), to_Char(vchdate,'YYYYMONTH')||TRIM(Title),to_Char(vchdate,'DD/MONTH/YYYY'),trim(obsv15)   order by to_Char(vchdate,'DD/MONTH/YYYY') asc";
                    mq2 = "SELECT TRIM(Title) AS FSTR,to_Char(vchdate,'DD/MONTH/YYYY')||TRIM(obsv15) AS GSTR,obsv15 as shift,TRIM(col1) as DownTime_Reason,TO_CHAR(sum(qty8),'999,999,999') as Mins_Lost,to_Char(vchdate,'YYYYMM') as Mth_char,COL4 AS START_TIME,COL5 AS END_TIME from inspvch  where branchcd='" + mbr + "' and type='55' and vchdate " + xprdrange + " group by Title,col1,to_Char(vchdate,'YYYYMONTH'), to_Char(vchdate,'DD/MONTH/YYYY'),to_Char(vchdate,'YYYYMM'),COL4,COL5,obsv15 order by to_Char(vchdate,'YYYYMM'),col1,title";
                    fgen.drillQuery(0, mq0, frm_qstr);
                    fgen.drillQuery(1, mq1, frm_qstr);
                    fgen.drillQuery(2, mq2, frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                //rejection 
                case "F40347":
                    mq0 = "SELECT DISTINCT to_Char(vchdate,'YYYYMONTH')||TRIM(Title) AS FSTR,'-' as gstr,to_Char(vchdate,'YYYYMONTH') as Month_Name,TO_CHAR(sum(qty8),'999,999,999') AS MINS_LOST FROM inspvch  where branchcd='" + mbr + "' and type='45' and vchdate  " + xprdrange + " GROUP BY to_Char(vchdate,'YYYYMONTH')||TRIM(Title),to_Char(vchdate,'YYYYMONTH')  order by to_Char(vchdate,'YYYYMONTH') asc";
                    mq1 = "SELECT DISTINCT to_Char(vchdate,'DD/MONTH/YYYY')||TRIM(obsv15) AS FSTR, to_Char(vchdate,'YYYYMONTH')||TRIM(Title) AS GSTR,to_Char(vchdate,'DD/MONTH/YYYY') as Month_Name,trim(obsv15) as shift,TO_CHAR(sum(qty8),'999,999,999') AS MINS_LOST from inspvch  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + " GROUP BY  to_Char(vchdate,'DD/MONTH/YYYY')||TRIM(obsv15), to_Char(vchdate,'YYYYMONTH')||TRIM(Title),to_Char(vchdate,'DD/MONTH/YYYY'),trim(obsv15)   order by to_Char(vchdate,'DD/MONTH/YYYY') asc";
                    mq2 = "SELECT TRIM(Title) AS FSTR,to_Char(vchdate,'DD/MONTH/YYYY')||TRIM(obsv15) AS GSTR,obsv15 as shift,TRIM(col1) as DownTime_Reason,TO_CHAR(sum(qty8),'999,999,999') as Mins_Lost,to_Char(vchdate,'YYYYMM') as Mth_char,COL4 AS START_TIME,COL5 AS END_TIME from inspvch  where branchcd='" + mbr + "' and type='45' and vchdate " + xprdrange + " group by Title,col1,to_Char(vchdate,'YYYYMONTH'), to_Char(vchdate,'DD/MONTH/YYYY'),to_Char(vchdate,'YYYYMM'),COL4,COL5,obsv15 order by to_Char(vchdate,'YYYYMM'),col1,title";

                    fgen.drillQuery(0, mq0, frm_qstr);
                    fgen.drillQuery(1, mq1, frm_qstr);
                    fgen.drillQuery(2, mq2, frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    break;

                case "F40349":
                    #region code for daily production report
                    mq0 = "select distinct ename as fstr,'-' as gstr,ename as machine, sum(prodn) as production,sum(rejn) as rejection,sum(net_prodn) as net_production ,ename from (SELECT  A.MSEQ,A.TYPE ,B.INAME ,A.NUM1 AS M_RDY, A.VCHNUM ,A.VCHDATE, A.iqtyin+a.mlt_loss AS PRODN , A.MLT_LOSS as rejn,A.IQTYIN AS NET_PRODN ,A.IQTYOUT AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,ROUND((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000)) as ppm ,ROUND((((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000))/10000),2) as ppm_prc FROM PROD_SHEET A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN('86','88') AND TRIM(NVL(A.MLT_LOSS,'0'))<>'0' AND TRIM(NVL(A.IQTYIN,'0'))<>'0' and trim(nvl(a.iqtyin+a.mlt_loss,'0'))<>'0'  AND A.VCHDATE " + xprdrange + "  ORDER BY A.VCHDATE,A.PREVCODE ) group by ename";
                    mq1 = "select distinct trim(ename)||trim(shift) as fstr,ename as gstr,ename as machine, sum(prodn) as production,sum(rejn) as rejection,sum(net_prodn) as net_production ,ename,shift from (SELECT  A.MSEQ,A.TYPE ,B.INAME ,A.NUM1 AS M_RDY, A.VCHNUM ,A.VCHDATE, A.iqtyin+a.mlt_loss AS PRODN , A.MLT_LOSS as rejn,A.IQTYIN AS NET_PRODN ,A.IQTYOUT AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,ROUND((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000)) as ppm ,ROUND((((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000))/10000),2) as ppm_prc FROM PROD_SHEET A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN('86','88') AND TRIM(NVL(A.MLT_LOSS,'0'))<>'0' AND TRIM(NVL(A.IQTYIN,'0'))<>'0' and trim(nvl(a.iqtyin+a.mlt_loss,'0'))<>'0'  AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHDATE,A.PREVCODE ) group by ename,shift";
                    mq2 = "select TRIM(INAME) AS FSTR,trim(ename)||trim(shift) AS GSTR,A.* from (SELECT  A.MSEQ,A.TYPE ,B.INAME ,A.NUM1 AS M_RDY, A.VCHNUM ,TO_CHAR(A.VCHDATE,'dd/MM/yyyy') as Vchdate, A.iqtyin+a.mlt_loss AS PRODN , A.MLT_LOSS as rejn,A.IQTYIN AS NET_PRODN ,A.IQTYOUT AS PLAN_QTY ,A.JOB_NO ,A.PREVCODE AS SHIFT ,A.TSLOT ,A.MCSTART ,A.MCSTOP  , A.OPR_DTL  ,A.ENAME,A.REMARKS2 ,ROUND((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000)) as ppm ,ROUND((((A.MLT_LOSS/( A.iqtyin+a.mlt_loss)*1000000))/10000),2) as ppm_prc FROM PROD_SHEET A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE IN('86','88') AND TRIM(NVL(A.MLT_LOSS,'0'))<>'0' AND TRIM(NVL(A.IQTYIN,'0'))<>'0' and trim(nvl(a.iqtyin+a.mlt_loss,'0'))<>'0'  AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHDATE,A.PREVCODE ) A";
                    fgen.drillQuery(0, mq0, frm_qstr);
                    fgen.drillQuery(1, mq1, frm_qstr);
                    fgen.drillQuery(2, mq2, frm_qstr);
                    fgen.Fn_DrillReport("", frm_qstr);
                    #endregion

                    break;

                #endregion



                //**************************************
                case "F40350":
                    mq0 = "";
                    mq0 = fgen.seek_iname(frm_qstr, co_cd, "select WIPSTDT from TYPE WHERE ID='B' AND TYPE1='" + mbr + "'", "WIPSTDT");
                    r10 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R10'", "params");
                    r10 = mq0.Length > 1 ? mq0 : r10;
                    xprdrange = "<= to_date('" + todt + "','dd/mm/yyyy') and " + (r10.Length > 2 ? " a.vchdate>=to_Date('" + r10 + "','dd/mm/yyyy') " : " a.vchdate>=to_Date('" + cDT1 + "','dd/mm/yyyy')");
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "SELECT DISTINCT TYPE1,name,place FROM TYPE WHERE ID='1' ORDER BY place,TYPE1");
                    mq0 = ""; mq10 = ""; mq1 = ""; mq2 = "";
                    var t = from type in dt.AsEnumerable()
                            select new
                            {
                                type1 = type.Field<string>("type1").Replace("(", "").Replace(")", "").Replace("/", "_").Replace(" ", "_"),
                                name = type.Field<string>("name").Replace("(", "").Replace(")", "").Replace("/", "_").Replace(" ", "_").Replace("&", "and")
                            };
                    foreach (var r in t)
                    {
                        mq0 += (mq0.Length > 0 ? "," : "") + "DECODE(TRIM(A.STAGE),'" + r.type1 + "',round(sum(a.iqtyin-a.iqtyout)),0) AS " + r.name;
                        mq10 += (mq10.Length > 0 ? "," : "") + "0 AS " + r.name;
                        mq1 += (mq1.Length > 0 ? "," : "") + "SUM(" + r.name + ")  AS " + r.name;
                        mq2 += (mq2.Length > 0 ? "+" : "") + "SUM(" + r.name + ")";
                    }

                    SQuery = "SELECT trim(A.icode) as fstr,B.INAME AS ITEM_NAME,B.CPARTNO AS PART_NO," + mq1 + ",sum(a.rej) as tot_rej,(" + mq2 + ") AS Total,trim(A.icode) as erpcode from (" +
                        "SELECT TRIM(A.ICODe) AS ICODE," + mq0 + ",0 as rej FROM IVOUCHER A WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " and a.store='W' group by trim(a.icode),trim(a.stage) union all " +
                        "SELECT TRIM(A.ICODe) AS ICODE," + mq0 + ",0 as rej FROM WIPSTK A WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '50%' AND A.VCHDATE " + xprdrange + " group by trim(a.icode),trim(a.stage) union all " +
                        "SELECT TRIM(A.rCODe) AS ICODE," + mq10 + ",sum(a.iqtyin) as rej FROM IVOUCHER A WHERE A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " and a.store='R' group by trim(a.rcode),trim(a.stage)" +
                        ") a,item b where trim(a.icodE)=trim(b.icodE) group by trim(a.icode),b.iname,b.cpartno having (" + mq2 + ")>0 order by trim(A.icode) ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("WIP Columnar Stock Report (as on " + fromdt + ")", frm_qstr);
                    break;

                case "F40351":
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                    if (party_cd.Length <= 1)
                    {
                        fgen.msg("-", "AMSG", "Please Select Customer");
                        return;
                    }
                    mq1 = "select distinct trim(col1)||'_'||'L'||srno as col1,trim(col1) as name,srno from inspmst where branchcd='" + mbr + "' and type='70' and acode like '" + party_cd + "%' order by srno";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    SQuery = "SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode as cust_code,f.aname as cust,a.icode ,i.iname ,i.cpartno,a.picode as parent,b.iname as parent_name,b.cpartno as parent_cpartno,a.col12 as die,a.col13 as modeltype,a.rejqty as ups,a.col14 as ctn_size_od,a.col16 as ctn_size_id,a.col15 as ply,a.grade as flutecode,a.col18 as clr,a.maintdt as sheetw,a.btchdt as sheetl,a.col17 as pref_mc,a.col18 as std_wstg,trim(a.col1)||'_'||'L'||srno as col1,a.col2 as col2,to_char(a.vchdate,'yyyymmdd') as vdd FROM item i,famst f,Inspmst a left join item b on trim(a.picode)=trim(b.icode) where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and a.branchcd='" + mbr + "' and a.type='70' AND a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and vchdate " + xprdrange + " order by vdd,a.vchnum,a.acode,a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    dt2 = new DataTable();
                    dt2 = dt.Clone();

                    foreach (DataRow dr in dt1.Rows)
                    {
                        // DYNAMIC HEADING
                        dt2.Columns.Add(dr["col1"].ToString().Trim(), typeof(string));
                    }
                    oporow = null;

                    if (dt.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt);
                        mdt = new DataTable();
                        mdt = view1.ToTable(true, "icode", "cust_code");

                        foreach (DataRow dr1 in mdt.Rows)
                        {
                            DataView view2 = new DataView(dt, "icode='" + dr1["icode"].ToString().Trim() + "' and cust_code='" + dr1["cust_code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = view2.ToTable();

                            oporow = dt2.NewRow();
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                oporow["vchnum"] = dticode.Rows[i]["vchnum"].ToString().Trim();
                                oporow["vchdate"] = dticode.Rows[i]["vchdate"].ToString().Trim();
                                oporow["cust_code"] = dticode.Rows[i]["cust_code"].ToString().Trim();
                                oporow["cust"] = dticode.Rows[i]["cust"].ToString().Trim();
                                oporow["icode"] = dticode.Rows[i]["icode"].ToString().Trim();
                                oporow["iname"] = dticode.Rows[i]["iname"].ToString().Trim();
                                oporow["cpartno"] = dticode.Rows[i]["cpartno"].ToString().Trim();
                                oporow["parent"] = dticode.Rows[i]["parent"].ToString().Trim();
                                oporow["parent_name"] = dticode.Rows[i]["parent_name"].ToString().Trim();
                                oporow["parent_cpartno"] = dticode.Rows[i]["parent_cpartno"].ToString().Trim();
                                oporow["die"] = dticode.Rows[i]["die"].ToString().Trim();
                                oporow["modeltype"] = dticode.Rows[i]["modeltype"].ToString().Trim();
                                oporow["ups"] = dticode.Rows[i]["ups"].ToString().Trim();
                                oporow["ctn_size_od"] = dticode.Rows[i]["ctn_size_od"].ToString().Trim();
                                oporow["ctn_size_id"] = dticode.Rows[i]["ctn_size_id"].ToString().Trim();
                                oporow["ply"] = dticode.Rows[i]["ply"].ToString().Trim();
                                oporow["flutecode"] = dticode.Rows[i]["flutecode"].ToString().Trim();
                                oporow["clr"] = dticode.Rows[i]["clr"].ToString().Trim();
                                oporow["sheetw"] = dticode.Rows[i]["sheetw"].ToString().Trim();
                                oporow["sheetl"] = dticode.Rows[i]["sheetl"].ToString().Trim();
                                oporow["pref_mc"] = dticode.Rows[i]["pref_mc"].ToString().Trim();
                                oporow["std_wstg"] = dticode.Rows[i]["std_wstg"].ToString().Trim();

                                try
                                {
                                    oporow[dticode.Rows[i]["col1"].ToString().Trim()] = dticode.Rows[i]["col2"].ToString().Trim();
                                }
                                catch { }
                            }
                            dt2.Rows.Add(oporow);
                        }
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        dt2.Columns.Remove("col1");
                        dt2.Columns.Remove("col2");
                        dt2.Columns.Remove("vdd");
                    }
                    Session["send_dt"] = dt2;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "");
                    fgen.Fn_open_rptlevel("Corrugation Process Plan From " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "GIPL_RPT":
                case "F35227":
                    #region
                    header_n = "Work Order Production Report";
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dtm = new DataTable();
                    dtm.Columns.Add("Marketing_Remark", typeof(string));
                    dtm.Columns.Add("Job_Type", typeof(string));
                    dtm.Columns.Add("Cust", typeof(string));
                    dtm.Columns.Add("Job_Clos", typeof(string));
                    dtm.Columns.Add("Job_No", typeof(string));
                    dtm.Columns.Add("Job_dt", typeof(string));
                    dtm.Columns.Add("Dlv_dt", typeof(string));
                    dtm.Columns.Add("Part_No", typeof(string));
                    dtm.Columns.Add("Icode", typeof(string));
                    dtm.Columns.Add("FGS", typeof(string));
                    dtm.Columns.Add("Name", typeof(string));
                    dtm.Columns.Add("Job_Qty", typeof(double));
                    dtm.Columns.Add("Job_UPS", typeof(string));
                    dtm.Columns.Add("Job_Sheet", typeof(string));
                    dtm.Columns.Add("So_no", typeof(string));
                    dtm.Columns.Add("S_N_P", typeof(string));
                    dtm.Columns.Add("Pref_M_C", typeof(string));
                    dtm.Columns.Add("Type_Of_Paper", typeof(string));
                    dtm.Columns.Add("Plate", typeof(string));
                    dtm.Columns.Add("Ink", typeof(string));
                    dtm.Columns.Add("Varnish", typeof(string));
                    dtm.Columns.Add("Board_Size", typeof(string));
                    dtm.Columns.Add("Cut_Size", typeof(string));
                    dtm.Columns.Add("Lamination", typeof(string));
                    dtm.Columns.Add("Liner", typeof(string));
                    dtm.Columns.Add("No_of_Ply", typeof(string));
                    dtm.Columns.Add("Reel_Size_X_Cut_Size", typeof(string));
                    dtm.Columns.Add("Side_Pasting_Glue", typeof(string));
                    dtm.Columns.Add("Lock_Bottom_Glue", typeof(string));
                    dtm.Columns.Add("Die", typeof(string));
                    dtm.Columns.Add("Embossing", typeof(string));
                    dtm.Columns.Add("Foiling", typeof(string));
                    dtm.Columns.Add("Window_Patching", typeof(string));
                    dtm.Columns.Add("Colour_Reference", typeof(string));
                    dtm.Columns.Add("OD_Size", typeof(string));
                    dtm.Columns.Add("Prt_Qty", typeof(string));
                    #endregion
                    #region
                    SQuery = "SELECT upper(a.col2) as heading,trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,a.vchnum as jobno,to_char(a.vchdate,'dd/mm/yyyy') as jobdt,a.*,trim(b.iname)  as iname,b.cpartno,trim(c.aname) as cust FROM COSTESTIMATE a,item b,famst c WHERE trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + mbr + "' and a.TYPE='30'  AND a.vchdate " + xprdrange + " order by a.vchnum,a.srno";
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);

                    SQuery = "select a.* from inspmst a where a.branchcd='" + mbr + "' and type='70'";
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    mq1 = "SELECT distinct A.NAME AS STAGE,A.TYPE1 FROM TYPE A,ITWSTAGE B,costestimate c  WHERE A.ID='K'  AND TRIM(A.TYPE1)=TRIM(B.STAGEC) and trim(b.icode)=trim(c.icode) and c.branchcd='" + mbr + "' and c.type='30' and c.vchdate " + xprdrange + "  order by type1";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq1);
                    dt4 = new DataTable();

                    mq3 = fgen.seek_iname(frm_qstr, co_cd, "select to_Date('" + fromdt + "','dd/mm/yyyy')+400 as todt from dual", "todt");
                    mq4 = Convert.ToDateTime(mq3).ToString("dd/MM/yyyy");//add 400 days as per mayuri mam for jobcard production
                    DateRange = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + mq4 + "','dd/mm/yyyy')";
                    mq2 = "select distinct UPPER(b.name) AS NAME,a.job_no,a.job_Dt,sum(nvl(a.a2,0)) as net_prod,trim(a.icode) as icode,a.stage from prod_sheet a ,type b  where trim(a.stage)=trim(b.type1) and b.id='K' and a.type='88' and a.branchcd='" + mbr + "' and a.vchdate " + DateRange + " group by a.job_no,a.job_dt,trim(a.icode),a.stage,b.name";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "select enqno,enqdt,icode,sum(sort_qty) as sort_qty,sum(col5) as paper_cut_qty from ( select enqno,to_char(enqdt,'dd/mm/yyyy') as enqdt,trim(icode) as icode,is_number(col5) as col5,is_number(col13) as sort_qty  from costestimate  where branchcd='" + mbr + "' and type='60' and vchdate " + DateRange + " ) group by enqno,enqdt,icode order by enqno";
                    dt6 = new DataTable();
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq3);
                    DataTable dt7 = new DataTable();
                    dr2 = null;
                    header_n = "";
                    if (dt1.Rows.Count > 0)
                    {
                        view1im = new DataView(dt1);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "jobno", "jobdt", "icode"); //MAIN                      
                        for (int j = 0; j < dt3.Rows.Count; j++)
                        {
                            dtm.Columns.Add(dt3.Rows[j]["STAGE"].ToString().Trim(), typeof(string));      //for dynamic column                 
                        }
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt1, "jobno='" + dr0["jobno"].ToString().Trim() + "' and jobdt='" + dr0["jobdt"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt = viewim.ToTable();//main dt
                            //=============
                            if (dt4.Rows.Count > 0)
                            {
                                DataView viewim2 = new DataView(dt4, "job_no='" + dr0["jobno"].ToString().Trim() + "' and job_dt='" + dr0["jobdt"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt5 = viewim2.ToTable(); //for all stages value
                            }
                            if (dt6.Rows.Count > 0)//for sort and packing only
                            {
                                DataView viewim3 = new DataView(dt6, "enqno='" + dr0["jobno"].ToString().Trim() + "' and enqdt='" + dr0["jobdt"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt7 = viewim3.ToTable();
                            }
                            //========================
                            dr2 = dtm.NewRow();
                            string fstr = "";
                            int sr = 0;
                            mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; string mq11 = "", mq12 = "", mq13 = "", mq14 = "", prt = "";
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                #region
                                fstr = dt.Rows[i]["fstr"].ToString().Trim();
                                mq2 = dt.Rows[i]["heading"].ToString().Trim();
                                sr = Convert.ToInt32(dt.Rows[i]["srno"].ToString().Trim());//for srno
                                //===================
                                dr2["Marketing_Remark"] = dt.Rows[i]["col12"].ToString().Trim();
                                dr2["Job_Type"] = dt.Rows[i]["col24"].ToString().Trim();
                                dr2["Cust"] = dt.Rows[i]["cust"].ToString().Trim();
                                dr2["Job_Clos"] = dt.Rows[i]["status"].ToString().Trim();
                                dr2["Job_No"] = dt.Rows[i]["jobno"].ToString().Trim();
                                dr2["Job_dt"] = dt.Rows[i]["jobdt"].ToString().Trim();
                                if (dt.Rows[i]["srno"].ToString().Trim() == "0")
                                {
                                    dr2["Dlv_dt"] = dt.Rows[i]["col21"].ToString().Trim();
                                }
                                dr2["Part_No"] = dt.Rows[i]["cpartno"].ToString().Trim();
                                dr2["Icode"] = dt.Rows[i]["icode"].ToString().Trim();
                                dr2["FGS"] = "";//comes from process plan on base of icode
                                dr2["Name"] = dt.Rows[i]["iname"].ToString().Trim();
                                dr2["Job_Qty"] = fgen.make_double(dt.Rows[i]["qty"].ToString().Trim());
                                dr2["Job_UPS"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "REJQTY");
                                dr2["Job_Sheet"] = dt.Rows[i]["col14"].ToString().Trim();
                                dr2["So_no"] = dt.Rows[i]["convdate"].ToString().Trim().Substring(4, 16);

                                dr2["Pref_M_C"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "col17");
                                if (mq2.Contains("TYPE OF PAPER") || (mq2.Contains("ANY") && (sr == 2 || sr == 3 || sr == 4 || sr == 5)))
                                {
                                    if (mq1 == "")
                                    {
                                        mq1 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq1 = mq1 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Type_Of_Paper"] = mq1; //and need ask is only one column should come there
                                }
                                else if (mq2.Contains("VARNISH"))
                                {
                                    if (mq3 == "")
                                    {
                                        mq3 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq3 = mq3 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Varnish"] = mq3;
                                }
                                else if (mq2.Contains("CUT"))
                                {
                                    if (mq4 == "")
                                    {
                                        mq4 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq4 = mq4 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Cut_Size"] = mq4;
                                }
                                else if (mq2.Contains("INK"))
                                {
                                    if (mq5 == "")
                                    {
                                        mq5 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq5 = mq5 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["ink"] = mq5;
                                }
                                else if (mq2.Contains("BOARD SIZE"))
                                {
                                    if (mq6 == "")
                                    {
                                        mq6 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq6 = mq6 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Board_Size"] = mq6;
                                }
                                else if (mq2.Contains("LINER"))
                                {
                                    if (mq7 == "")
                                    {
                                        mq7 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq7 = mq7 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Liner"] = mq7;
                                }
                                else if (mq2.Contains("NO. OF PLY"))
                                {
                                    if (mq8 == "")
                                    {
                                        mq8 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq8 = mq8 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["No_of_Ply"] = mq8;
                                }
                                else if (mq2.Contains("REEL SIZE X"))
                                {
                                    if (mq9 == "")
                                    {
                                        mq9 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq9 = mq9 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Reel_Size_X_Cut_Size"] = mq9;
                                }
                                else if (mq2.Contains("LOCK BOTTOM GLUE"))
                                {
                                    if (mq10 == "")
                                    {
                                        mq10 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq10 = mq10 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Lock_Bottom_Glue"] = mq10;
                                }
                                else if (mq2.Contains("SIDE PASTING GLUE"))
                                {
                                    if (mq11 == "")
                                    {
                                        mq11 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq11 = mq11 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Side_Pasting_Glue"] = mq11;
                                }
                                else if (mq2.Contains("LAMINATION"))
                                {
                                    if (mq12 == "")
                                    {
                                        mq12 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq12 = mq12 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Lamination"] = mq12;
                                }
                                else if (mq2.Contains("WINDOW PATCH"))
                                {
                                    if (mq13 == "")
                                    {
                                        mq13 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq13 = mq13 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Window_Patching"] = mq13;
                                }
                                else if (mq2.Contains("REMARKS1"))//CHANGE REMARK BY REMARK1 AS PER ASHOK SIR BECOZ REMARK KAI BAR AA SKTA HAI BUT REMARK1 IS USE ONYL FOR COLOR
                                {
                                    if (mq14 == "")
                                    {
                                        mq14 = dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq14 = mq14 + "," + dt.Rows[i]["col3"].ToString().Trim();
                                    }
                                    dr2["Colour_Reference"] = mq14;
                                }
                                else if (mq2.Contains("FOILING"))
                                {
                                    dr2["Foiling"] = dt.Rows[i]["col3"].ToString().Trim();
                                }
                                else if (mq2 == "ANY" && sr == 13)
                                {
                                    dr2["Plate"] = dt.Rows[i]["col3"].ToString().Trim();
                                }
                                dr2["Die"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "col12");
                                dr2["OD_Size"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "col14");
                                dr2["Embossing"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "col15");
                                prt = dt.Rows[i]["col12"].ToString().Trim();
                                if (prt.Length > 1)
                                {
                                    if (prt.Contains(";"))
                                    {
                                        dr2["prt_qty"] = prt.Split(';')[0].ToString();
                                    }
                                    else if (prt.Contains(":"))
                                    {
                                        dr2["prt_qty"] = prt.Split(':')[0].ToString();
                                    }
                                    else if (prt.Contains(","))
                                    {
                                        dr2["prt_qty"] = prt.Split(',')[0].ToString();
                                    }
                                }
                                else
                                {
                                    dr2["prt_qty"] = "";
                                }
                                dr2["S_N_P"] = "";
                                #endregion
                            }
                            if (dt5 != null && dt5.Rows.Count > 0)
                            {
                                for (int j = 0; j < dt5.Rows.Count; j++)
                                {
                                    dr2[dt5.Rows[j]["name"].ToString().Trim()] = dt5.Rows[j]["net_prod"].ToString().Trim();
                                }
                            }
                            if (dt3.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt3.Rows.Count; k++)
                                {
                                    mq2 = dt3.Rows[k]["stage"].ToString().Trim();
                                    if (mq2.Contains("SORTING AND PACKING"))
                                    {
                                        dr2[mq2] = fgen.seek_iname_dt(dt7, "enqno='" + dr0["jobno"].ToString().Trim() + "' and enqdt='" + dr0["jobdt"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "sort_qty");
                                    }
                                    if (mq2.Contains("PAPER CUTTING"))
                                    {
                                        dr2[mq2] = fgen.seek_iname_dt(dt7, "enqno='" + dr0["jobno"].ToString().Trim() + "' and enqdt='" + dr0["jobdt"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "paper_cut_qty");
                                    }
                                }
                            }
                            dtm.Rows.Add(dr2);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F40060":
                    #region
                    //   header_n = "Day Wise,Stage Wise,Shift Wise,Operator Wise and Reason Wise Report";
                    header_n = "Woven Label Rejection Detail Report";
                    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable();
                    mq0 = ""; mq1 = ""; mq2 = "";
                    dtm.Columns.Add("Dated", typeof(string));
                    dtm.Columns.Add("Label_Name", typeof(string));
                    dtm.Columns.Add("Job_No", typeof(string));
                    dtm.Columns.Add("SO_No", typeof(string));
                    dtm.Columns.Add("Order_Qty", typeof(double));
                    dtm.Columns.Add("Mfg_Qty", typeof(double));  //value comes form okqty field in prodn screen and stage not in ('19')
                    dtm.Columns.Add("Ok_Qty", typeof(double));  //value comes form okqty field in prodn screen and stage in ('19')
                    dtm.Columns.Add("Rejecion_Qty", typeof(double));
                    dtm.Columns.Add("Rate", typeof(double));
                    dtm.Columns.Add("Amount", typeof(double));
                    dtm.Columns.Add("Mc_No", typeof(string));
                    dtm.Columns.Add("Operator", typeof(string));
                    ////FOR ADD YELLO PORTION
                    SQuery = "SELECT * FROM (SELECT TYPE1,NAME FROM TYPE WHERE ID='8' ORDER BY TYPE1) WHERE ROWNUM<11";
                    dt5 = fgen.getdata(frm_qstr, co_cd, SQuery);//REASON DT
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        dtm.Columns.Add("" + dt5.Rows[i]["name"].ToString().Trim() + "_Percentage", typeof(double));
                    }

                    SQuery = "SELECT c.NAME AS STAGE_NAME,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.opr_dtl as operator,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY,A.JOB_NO,A.JOB_dT,sum(a.a11) as a11,sum(a.a12) as a12,sum(a.a13) as a13,sum(a.a14) as a14,sum(a.a15) as a15,sum(a.a16) as a16,sum(a.a17) as a17,sum(a.a18) as a18,sum(a.a19) as a19,sum(a.a20) as a20 FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND A.VCHDATE " + xprdrange + " GROUP BY c.NAME,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,a.opr_dtl,b.iname,A.JOB_NO,A.JOB_dT ORDER BY VDD";//new 
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//MAIN DT for all stages

                    // mq2 = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT,sum(a.a11) as a11,sum(a.a12) as a12,sum(a.a13) as a13,sum(a.a14) as a14,sum(a.a15) as a15,sum(a.a16) as a16,sum(a.a17) as a17,sum(a.a18) as a18,sum(a.a19) as a19,sum(a.a20) as a20 FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND A.VCHDATE " + xprdrange + " and a.stage in ('17','18') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,b.iname,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD";//old
                    mq2 = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT,sum(a.a11) as a11,sum(a.a12) as a12,sum(a.a13) as a13,sum(a.a14) as a14,sum(a.a15) as a15,sum(a.a16) as a16,sum(a.a17) as a17,sum(a.a18) as a18,sum(a.a19) as a19,sum(a.a20) as a20 FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND A.VCHDATE " + xprdrange + " and a.stage in ('19') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,b.iname,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD";//new as per sushant sir
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq2); //for ok qty

                    mq3 = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT,sum(a.a11) as a11,sum(a.a12) as a12,sum(a.a13) as a13,sum(a.a14) as a14,sum(a.a15) as a15,sum(a.a16) as a16,sum(a.a17) as a17,sum(a.a18) as a18,sum(a.a19) as a19,sum(a.a20) as a20 FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND A.VCHDATE " + xprdrange + " and a.stage not in ('19') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,b.iname,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD";//new as per sushant sir
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq3); //for mfg qty

                    mq0 = "SELECT distinct  BRANCHCD,VCHNUM AS JOB_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS JOB_dT,substr(convdate,1,20) as so_Detail   FROM COSTESTIMATE WHERE branchcd='" + mbr + "' and TYPE='30'";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0); //jobcard dt

                    mq1 = "SELECT trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as sodetails,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,sum(qtyord) as so_qty,irate FROM SOMAS WHERE  type like '4%'  group by ordno,to_char(orddt,'dd/mm/yyyy'),trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy'),irate order by sodetails"; //BRanchcd='" + mbr + "' and and orddt " + DateRange + "
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);//sale order kabi b ban skte hai agar jobcard me last yr ka so link hua then kyq krna hai...
                    dt3 = new DataTable();

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "JOB_NO", "JOB_dT"); //MAIN    
                        int cnt = 0;
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "JOB_NO='" + dr0["JOB_NO"].ToString().Trim() + "' and JOB_dT='" + dr0["JOB_dT"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = viewim.ToTable();
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                mq2 = ""; mq3 = ""; db = 0; db1 = 0; cnt = 0; db2 = 0; db3 = 0; db4 = 0;
                                dr2 = dtm.NewRow();
                                dr2["Dated"] = dt3.Rows[i]["vchdate"].ToString().Trim();
                                dr2["Job_No"] = dt3.Rows[i]["JOB_NO"].ToString().Trim();
                                dr2["Mc_No"] = dt3.Rows[i]["ename"].ToString().Trim();
                                dr2["Label_Name"] = dt3.Rows[i]["label"].ToString().Trim();
                                mq2 = fgen.seek_iname_dt(dt1, "job_no='" + dr0["JOB_NO"].ToString().Trim() + "' and job_dt='" + dr0["JOB_dT"].ToString().Trim() + "'", "so_Detail");
                                mq3 = fgen.seek_iname_dt(dt2, "sodetails='" + mq2 + "'", "ordno");
                                db = fgen.make_double(fgen.seek_iname_dt(dt2, "sodetails='" + mq2 + "'", "so_qty"));
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "sodetails='" + mq2 + "'", "irate"));
                                dr2["SO_No"] = mq3;
                                dr2["Order_Qty"] = db;
                                dr2["Mfg_Qty"] = fgen.make_double(fgen.seek_iname_dt(dt6, "job_no='" + dr0["JOB_NO"].ToString().Trim() + "' and job_dt='" + dr0["JOB_dT"].ToString().Trim() + "'", "ok_qty"));//dt3.Rows[i]["ok_qty"].ToString().Trim();
                                db3 = fgen.make_double(dr2["Mfg_Qty"].ToString().Trim());
                                dr2["Ok_Qty"] = fgen.make_double(fgen.seek_iname_dt(dt4, "job_no='" + dr0["JOB_NO"].ToString().Trim() + "' and job_dt='" + dr0["JOB_dT"].ToString().Trim() + "'", "ok_qty"));//dt3.Rows[i]["ok_qty"].ToString().Trim();
                                dr2["Rejecion_Qty"] = dt3.Rows[i]["REJ_QTY"].ToString().Trim();
                                db2 = fgen.make_double(dr2["Mfg_Qty"].ToString().Trim());
                                dr2["Rate"] = db1;
                                dr2["Amount"] = db1 * fgen.make_double(dr2["Rejecion_Qty"].ToString().Trim());
                                dr2["Operator"] = dt3.Rows[i]["operator"].ToString().Trim();
                                cnt = 11;
                                for (int j = 0; j < dt5.Rows.Count; j++)
                                {
                                    if (db2 > 0)
                                    {
                                        db4 = Math.Round((fgen.make_double(dt3.Rows[i]["a" + cnt + ""].ToString().Trim().Replace("NaN", "0")) / db2 * 100), 3);
                                    }
                                    else
                                    {
                                        db4 = 0;
                                    }
                                    dr2["" + dt5.Rows[j]["name"].ToString().Trim() + "_Percentage"] = db4;
                                    cnt++;
                                }
                                if (fgen.make_double(dr2["Rejecion_Qty"].ToString().Trim()) > 0)
                                {
                                    dtm.Rows.Add(dr2);
                                }
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F40061":
                    #region ye summary h as first report is detail but need to ask to client that isme rejection reason konse stage ke aayenge
                    //  header_n = "Day Wise,Stage Wise,Shift Wise,Operator Wise and Reason Wise Report";
                    header_n = "Woven Label Rejection Summary Report";
                    dtm = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                    mq0 = ""; mq1 = ""; mq2 = "";
                    dtm.Columns.Add("Dated", typeof(string));

                    SQuery = "SELECT * FROM (SELECT TYPE1,NAME FROM TYPE WHERE ID='8' ORDER BY TYPE1) WHERE ROWNUM<11";
                    dt5 = new DataTable();
                    dt5 = fgen.getdata(frm_qstr, co_cd, SQuery);//REASON DT
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        dtm.Columns.Add("" + dt5.Rows[i]["name"].ToString().Trim() + "_Pcs", typeof(double));
                        dtm.Columns.Add("" + dt5.Rows[i]["name"].ToString().Trim() + "_Amt", typeof(double));
                    }
                    dtm.Columns.Add("Total_Pcs", typeof(double));
                    dtm.Columns.Add("Total_Amt", typeof(double));
                    //=====================                    
                    SQuery = "SELECT to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.JOB_NO,A.JOB_dT,sum(a.a11) as a11,sum(a.a12) as a12,sum(a.a13) as a13,sum(a.a14) as a14,sum(a.a15) as a15,sum(a.a16) as a16,sum(a.a17) as a17,sum(a.a18) as a18,sum(a.a19) as a19,sum(a.a20) as a20 FROM PROD_SHEET A  WHERE   A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND A.VCHDATE  " + xprdrange + " and a.stage not in ('17','18') GROUP BY to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),A.JOB_NO,A.JOB_dT ORDER BY VDD";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//MAIN  DT FROM JOB WISE PRODUCTION

                    mq2 = "SELECT c.NAME AS STAGE_NAME,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_Char(a.vchdate,'yyyymmdd') as vdd,a.mchcode,a.ename,b.iname as label,sum(a.a1) as job_qty,sum(a.a2) as ok_qty,sum(a.a4) AS REJ_QTY, A.PREVCODE AS SHIFT_NAME,A.SHFTCODE,A.JOB_NO,A.JOB_dT,sum(a.a11) as a11,sum(a.a12) as a12,sum(a.a13) as a13,sum(a.a14) as a14,sum(a.a15) as a15,sum(a.a16) as a16,sum(a.a17) as a17,sum(a.a18) as a18,sum(a.a19) as a19,sum(a.a20) as a20 FROM PROD_SHEET A,item b,TYPE c  WHERE  TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STAGE)=TRIM(c.TYPE1) AND c.ID='K' AND A.BRANCHCD='" + mbr + "'  AND A.TYPE='86' AND A.VCHDATE " + xprdrange + " and a.stage in ('17','18') GROUP BY c.NAME ,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),to_Char(a.vchdate,'yyyymmdd'),a.mchcode,a.ename,b.iname,A.PREVCODE,A.SHFTCODE,A.JOB_NO,A.JOB_dT ORDER BY VDD";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq2);//main dt only for final qty where stges in 17 ,18 

                    mq0 = "SELECT distinct  BRANCHCD,VCHNUM AS JOB_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS JOB_dT,substr(convdate,1,20) as so_Detail   FROM COSTESTIMATE WHERE branchcd='" + mbr + "' and TYPE='30'";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0); //jobcard dt

                    mq1 = "SELECT trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as sodetails,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,sum(qtyord) as so_qty,irate FROM SOMAS WHERE  type like '4%'  group by ordno,to_char(orddt,'dd/mm/yyyy'),trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy'),irate order by sodetails"; //BRanchcd='" + mbr + "' and orddt " + DateRange + "
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);
                    dt3 = new DataTable();

                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "JOB_NO", "JOB_dT"); //MAIN    
                        int cnt = 0;
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt, "JOB_NO='" + dr0["JOB_NO"].ToString().Trim() + "' and JOB_dT='" + dr0["JOB_dT"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt3 = viewim.ToTable();
                            for (int i = 0; i < dt3.Rows.Count; i++)
                            {
                                mq2 = ""; mq3 = ""; db = 0; db1 = 0; cnt = 0; db2 = 0; db3 = 0; db4 = 0;
                                dr2 = dtm.NewRow();
                                //  dr2["sno"] = i;
                                dr2["Dated"] = dt3.Rows[i]["vchdate"].ToString().Trim();
                                mq2 = fgen.seek_iname_dt(dt1, "job_no='" + dr0["JOB_NO"].ToString().Trim() + "' and job_dt='" + dr0["JOB_dT"].ToString().Trim() + "'", "so_Detail");
                                mq3 = fgen.seek_iname_dt(dt2, "sodetails='" + mq2 + "'", "ordno");
                                db = fgen.make_double(fgen.seek_iname_dt(dt2, "sodetails='" + mq2 + "'", "so_qty")); //pcs value
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "sodetails='" + mq2 + "'", "irate"));
                                db2 = db * db1;//amt                       
                                cnt = 11;
                                for (int j = 0; j < dt5.Rows.Count; j++)
                                {
                                    dr2["" + dt5.Rows[j]["name"].ToString().Trim() + "_Pcs"] = Math.Round(fgen.make_double(dt3.Rows[i]["a" + cnt + ""].ToString().Trim()));
                                    db3 += Math.Round(fgen.make_double(dt3.Rows[i]["a" + cnt + ""].ToString().Trim()));
                                    dr2["" + dt5.Rows[j]["name"].ToString().Trim() + "_Amt"] = Math.Round(fgen.make_double(dt3.Rows[i]["a" + cnt + ""].ToString().Trim())) * db1;
                                    db4 += Math.Round(fgen.make_double(dt3.Rows[i]["a" + cnt + ""].ToString().Trim())) * db1;
                                    cnt++;
                                }
                                dr2["Total_Pcs"] = db3;
                                dr2["Total_Amt"] = db4;
                                dtm.Rows.Add(dr2);
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F40063":
                    #region
                    mq3 = ""; mq4 = "";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");//dept
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE"); //machine
                    if (party_cd.Length > 1)
                    {
                        mq3 = "AND COL4 IN (" + party_cd + ")";
                    }
                    else
                    {
                        mq3 = "AND COL4 LIKE '%'";
                    }
                    if (part_cd.Length > 1)
                    {
                        mq4 = "AND COL2 IN (" + part_cd + ")";
                    }
                    else
                    {
                        mq4 = "AND COL2 LIKE '%'";
                    }
                    mq0 = "select col2,COL3 AS MACH, col4,col12 as complnt,sum(num1) as hrs,sum(num2) as min,acode from scratch where branchcd='" + mbr + "' and type='MN' and vchdate " + xprdrange + " " + mq3 + " " + mq4 + " and acode in ('11','12','13','18') group by col2,COL3, col4,col12,acode order by acode";//acode for shift a and b
                    //col2 for mach code....col4 for shift......
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq0);//main dt
                    //=============================================
                    dtm = new DataTable();
                    dtm.Columns.Add("MC_No", typeof(string));
                    dtm.Columns.Add("MC_Name", typeof(string));
                    SQuery = "select type1,name from typeGRP  WHERE ID='MN' ORDER BY TYPE1";//macine problems from master
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//REASON DT
                    //FOR SHIT A
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtm.Columns.Add("" + dt.Rows[i]["name"].ToString().Trim() + "_A", typeof(double));
                    }
                    dtm.Columns.Add("TOTAL_A", typeof(double));//cnt1
                    //==============again make column for shift b
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtm.Columns.Add("" + dt.Rows[i]["name"].ToString().Trim() + "_B", typeof(double));
                    }
                    dtm.Columns.Add("TOTAL_B", typeof(double));//cnt2        
                    //======FOR LAST
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtm.Columns.Add("" + dt.Rows[i]["name"].ToString().Trim() + "", typeof(double));
                    }
                    dtm.Columns.Add("TOTAL_A_B", typeof(double));//cnt3
                    if (dt3.Rows.Count > 0)
                    {
                        view1im = new DataView(dt3);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "col2"); //machine
                        // int cnt = 0;
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt3, "col2='" + dr0["col2"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            dr2 = dtm.NewRow();
                            db = 0; db1 = 0; db2 = 0; db3 = 0; db6 = 0; db7 = 0;
                            db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0;
                            db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db22 = 0; //for shift a
                            db23 = 0; db24 = 0; db25 = 0; db26 = 0; db27 = 0; db28 = 0; db29 = 0;//for shift b
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                db4 = 0; db5 = 0;
                                mq1 = ""; mq2 = "";
                                mq1 = dt1.Rows[i]["complnt"].ToString().Trim();
                                mq2 = dt1.Rows[i]["acode"].ToString().Trim();
                                db2 = fgen.make_double(dt1.Rows[i]["hrs"].ToString().Trim());
                                db3 = fgen.make_double(dt1.Rows[i]["min"].ToString().Trim());
                                if (db3 > 0)
                                {
                                    db4 = db2 + db3 / 60; //.............min /60
                                }
                                else
                                {
                                    db4 = db2 + db3;
                                }
                                dr2["MC_Name"] = dt1.Rows[i]["MACH"].ToString().Trim();
                                dr2["MC_No"] = dt1.Rows[i]["COL2"].ToString().Trim();//PAHLE COL2 THA ..but user want machine name  and also need to update this code on mam laptop
                                switch (mq2)
                                {
                                    #region for shift a
                                    case "11":
                                    case "12":
                                        if (mq1 == "ELECTRICAL PROBLEM")
                                        {
                                            db16 += db4;
                                            dr2["ELECTRICAL PROBLEM_A"] = Math.Round(db16, 2);
                                            db6 += db4;
                                            db8 += db4;
                                        }
                                        else if (mq1 == "MECHANICAL PROBLEM")
                                        {
                                            db17 += db4;
                                            dr2["MECHANICAL PROBLEM_A"] = Math.Round(db17, 2);
                                            db6 += db4;
                                            db9 += db4;
                                        }
                                        else if (mq1 == "HARNESS PROBLEM")
                                        {
                                            db18 += db4;
                                            dr2["HARNESS PROBLEM_A"] = Math.Round(db18, 2);
                                            db6 += db4;
                                            db10 += db4;
                                        }
                                        else if (mq1 == "ORDER SHORTAGE")
                                        {
                                            db19 += db4;
                                            dr2["ORDER SHORTAGE_A"] = Math.Round(db19, 2);
                                            db6 += db4;
                                            db11 += db4;
                                        }
                                        else if (mq1 == "OTHER PROBLEMS")
                                        {
                                            db20 += db4;
                                            dr2["OTHER PROBLEMS_A"] = Math.Round(db20, 2);
                                            db6 += db4;
                                            db12 += db4;
                                        }
                                        else if (mq1 == "SAMPLING PROCESS")
                                        {
                                            db21 += db4;
                                            dr2["SAMPLING PROCESS_A"] = Math.Round(db21, 0);
                                            db6 += db4;
                                            db13 += db4;
                                        }
                                        else if (mq1 == "KNOTTING PROCESS")
                                        {
                                            db22 += db4;
                                            dr2["KNOTTING PROCESS_A"] = Math.Round(db22, 2);
                                            db6 += db4;
                                            db14 += db4;
                                        }
                                        break;
                                    #endregion
                                    #region for shift b
                                    case "13":
                                    case "18":
                                        if (mq1 == "ELECTRICAL PROBLEM")
                                        {
                                            db23 += db4;
                                            dr2["ELECTRICAL PROBLEM_B"] = Math.Round(db23, 2);
                                            db7 += db4;
                                            db8 += db4;
                                        }
                                        else if (mq1 == "MECHANICAL PROBLEM")
                                        {
                                            db24 += db4;
                                            dr2["MECHANICAL PROBLEM_B"] = Math.Round(db24, 2);
                                            db7 += db4;
                                            db9 += db4;
                                        }
                                        else if (mq1 == "HARNESS PROBLEM")
                                        {
                                            db25 += db4;
                                            dr2["HARNESS PROBLEM_B"] = Math.Round(db25, 2);
                                            db7 += db4;
                                            db10 += db4;
                                        }
                                        else if (mq1 == "ORDER SHORTAGE")
                                        {
                                            db26 += db4;
                                            dr2["ORDER SHORTAGE_B"] = Math.Round(db26, 2);
                                            db7 += db4;
                                            db11 += db4;
                                        }
                                        else if (mq1 == "OTHER PROBLEMS")
                                        {
                                            db27 += db4;
                                            dr2["OTHER PROBLEMS_B"] = Math.Round(db27, 2);
                                            db7 += db4;
                                            db12 += db4;
                                        }
                                        else if (mq1 == "SAMPLING PROCESS")
                                        {
                                            db28 += db4;
                                            dr2["SAMPLING PROCESS_B"] = Math.Round(db28, 2);
                                            db7 += db4;
                                            db13 += db4;
                                        }
                                        else if (mq1 == "KNOTTING PROCESS")
                                        {
                                            db29 += db4;
                                            dr2["KNOTTING PROCESS_B"] = Math.Round(db29, 2);
                                            db7 += db4;
                                            db14 += db4;
                                        }
                                        break;
                                    #endregion
                                }
                            }
                            dr2["TOTAL_A"] = Math.Round(db6, 2);
                            dr2["TOTAL_B"] = Math.Round(db7, 2);
                            dr2["TOTAL_A_B"] = Math.Round((db6 + db7), 2);
                            if (db8 > 0)
                            {
                                dr2["ELECTRICAL PROBLEM"] = Math.Round(db8, 2);
                            }
                            if (db9 > 0)
                            {
                                dr2["MECHANICAL PROBLEM"] = Math.Round(db9, 2);
                            }
                            if (db10 > 0)
                            {
                                dr2["HARNESS PROBLEM"] = Math.Round(db10, 2);
                            }
                            if (db11 > 0)
                            {
                                dr2["ORDER SHORTAGE"] = Math.Round(db11, 2);
                            }
                            if (db12 > 0)
                            {
                                dr2["OTHER PROBLEMS"] = Math.Round(db12, 2);
                            }
                            if (db13 > 0)
                            {
                                dr2["SAMPLING PROCESS"] = Math.Round(db13, 2);
                            }
                            if (db14 > 0)
                            {
                                dr2["KNOTTING PROCESS"] = Math.Round(db14, 2);
                            }
                            ///if no value in hours in row then row will not add in cursor
                            if (db4 > 0)
                            {
                                dtm.Rows.Add(dr2);
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F40353":
                    dt = new DataTable();
                    dt.Columns.Add("JOBNO", typeof(string));
                    dt.Columns.Add("JOBDT", typeof(string));
                    dt.Columns.Add("PARTY", typeof(string));
                    dt.Columns.Add("ERPCODE", typeof(string));
                    dt.Columns.Add("PRODUCT", typeof(string));
                    dt.Columns.Add("PARTNO", typeof(string));
                    dt.Columns.Add("PLAN_QTY", typeof(double));
                    dt.Columns.Add("BOX_WT_AS_PER_SPEC_KG", typeof(double));
                    dt.Columns.Add("BOX_WT_AS_PER_JOB_CARD_KG", typeof(double));
                    dt.Columns.Add("CORRUGATION_SHEETS_ACTUALLY_PRODUCED_NOS", typeof(double));
                    dt.Columns.Add("LINEAR_MTR", typeof(double));
                    dt.Columns.Add("PAPER_WT_REQD_AS_PER_SPEC_KG", typeof(double));
                    dt.Columns.Add("PAPER_WT_REQD_AS_PER_JOB_CARD_KG", typeof(double));
                    dt.Columns.Add("PAPER_WT_DIFF-SPEC_VS_JOB_CARD_KG", typeof(double));
                    dt.Columns.Add("PAPER_CONSUMED_ACTUAL_KG", typeof(double));
                    dt.Columns.Add("PAPER_Cost", typeof(double));
                    dt.Columns.Add("EXCESS/SHORT_PAPER_CONSUMED_AGAINST_JOBCARD_KG", typeof(double));
                    dt.Columns.Add("NO_OF_SHEETS_SUPPOSED_TO_BE_PRODUCED_ASPER_JOBCARD_WT_NOS", typeof(double));
                    dt.Columns.Add("CORRUGATION_REJECTION_NOS", typeof(double));
                    dt.Columns.Add("REJECTION_SHEET_WT_AS_PER_JOBCARD_KG", typeof(double));
                    dt.Columns.Add("REJECTION_SHEET_WT+PAPER_WT_DIFF_AS_PER_JOB_CARD_KG", typeof(double));
                    dt.Columns.Add("CORR_WASTAGE_%", typeof(double));
                    dt.Columns.Add("CONVERSION_REJ_QTY_NOS", typeof(double));
                    dt.Columns.Add("CONVERSION_REJ_WT_KG", typeof(double));
                    dt.Columns.Add("FINAL_BOX_QTY_NOS", typeof(double));
                    dt.Columns.Add("TOTAL_REJECTION_WT_KG", typeof(double));
                    dt.Columns.Add("FINAL_BOX_WT_KG", typeof(double));
                    dt.Columns.Add("HYPO_BOX_LOST/GAINED_DUE_TO_WASTAGE_NOS", typeof(double));
                    dt.Columns.Add("WT_WISE_REJECTION_%", typeof(double));
                    dt.Columns.Add("BOX_WISE_REJECTION_%", typeof(double));
                    dt.Columns.Add("REMARKS_FROM_CORRUGATION", typeof(string));
                    dt.Columns.Add("REMARKS_FROM_CONVERSION", typeof(string));
                    SQuery = "";
                    SQuery = "SELECT a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty,sum(b.numwt) as bwt_spec,a.WT_BOX_per_JC,a.Corrg_sheet_actual_produce,a.LINEAR_MTR,a.Corrg_sheet_actual_produce* sum(b.numwt) as paperWt_as_per_spec,a.PAPER_WT_REQ_JCRD, (a.PAPER_WT_REQ_JCRD-(a.Corrg_sheet_actual_produce* sum(b.numwt))) as paperWt_diff ,a.PAPR_CONSME_ACT,  a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD as excess_shot_paper_consumed, round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/ (case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),3) AS NO_OF_SHEET_TO_PRODUCED,a.CORR_STG_REJ,a.CORR_WT_LOSS ,((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS) AS DIFF_AS_PER_JBCARD, ROUND(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS)/ a.PAPR_CONSME_ACT*100,3) AS CORR_WASTAGE_PER,a.sorting_packing_rej,a.sorting_packing_loss , a.job_vALUE,a.WT_DIFF AS TOT_RJ_WT,round(a.WT_DIFF/a.WT_BOX_per_JC,4) as hypo_box,a.WT_WISE_REJ_PER, ROUND(((round(a.PAPR_CONSME_ACT/ a.WT_BOX_per_JC,2)-round(a.FINAL_BOX_PROD,2))/round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/(case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),2))*100,3)  as BOX_WISE_REJ,a.FINAL_BOX_PROD,A.COMMENTS3 FROM (SELECT trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy') as fstr,A.ENQNO AS JOBNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS JOBDT,A.ACODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO AS PARTNO,A.TOT_BOX_RCV as plan_qty,round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) AS WT_BOX_per_JC,A.QTYOUT AS Corrg_sheet_actual_produce,(CASE WHEN is_number(C.BTCHDT)>0 THEN ROUND((A.QTYOUT*is_number(C.BTCHDT))/100,2) ELSE 0 END) AS LINEAR_MTR,round(round(a.enr2/a.STD_SHT_RQ,4) * (CASE WHEN A.COL13>0 THEN round(A.QTYOUT/A.COL13,2) ELSE A.COL3 END),3) AS PAPER_WT_REQ_JCRD,A.QTYIN AS PAPR_CONSME_ACT,d.CORR_STG_REJ,d.CORR_STG_REJ*round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) as CORR_WT_LOSS,d.snp as sorting_packing_rej,(d.snp * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4)) as  sorting_packing_loss ,A.IQTYIN AS FINAL_BOX_PROD,A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4) as WT_DIFF,ROUND((A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4)) / (CASE WHEN A.QTYIN>0 THEN A.QTYIN ELSE 1 END)  * 100 , 3) AS WT_WISE_REJ_PER,round(((A.QTYOUT - A.IQTYIN) / (case when A.QTYOUT>0 then a.qtyout else 1 end)) * 100,3) AS BOX_WISE_REJ, (A.IQTYIN*A.SALERATE) as job_vALUE,A.COMMENTS3 FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,B.COL13,c.IRATE AS SALERATE,B.COMMENTS3 FROM (select enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from (select INVNO AS ENQNO,INVDATE AS ENQDT,TRIM(ICODE) AS ACODE,0 as qtyin,0 as qtyout,0 AS COL3,0 as scrp1,0 as scrp2,0 as time1,0 as time2,SUM(IQTYIN) AS IQTYIN,0 AS VAL from IVOUCHER where BRANCHCD='" + mbr + "' AND type='16' and INVDATE " + xprdrange + " group by INVNO,INVDATE,TRIM(ICODE) union all select A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS QTYIN1,SUM((case when b.irate>0 then ROUND(is_number(A.col4)*B.IRATE,2) else ROUND(is_number(A.col4)*c.IRATE,2) end)) AS VAL from item c,costestimate A left outer join REELVCH B on A.BRANCHCD||TRIM(A.ICODe)||TRIM(A.COL6)=B.BRANCHCD||TRIM(B.ICODe)||TRIM(B.KCLREELNO) and b.type in ('02','07') where trim(a.icode)=trim(c.icodE) and A.BRANCHCD='" + mbr + "' AND A.type='25' and A.enqdt " + xprdrange + " group by A.enqno,A.enqdt,TRIM(A.aCODE) union all select a.enqno,a.enqdt,TRIM(a.aCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join (SELECT VCHNUM,VCHDATE,SUM(is_number(replace(nvl(COL3,'0'),'-','0'))) AS COL3 from inspvch WHERE BRANCHCD='" + mbr + "' and type='45' group by vchnum,vchdate) b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') where a.BRANCHCD='" + mbr + "' and a.type='40' and a.enqdt " + xprdrange + " group by a.enqno,a.enqdt,TRIM(a.aCODE),is_number(replace(nvl(a.COL3,'0'),'-','0')) ) group by enqno,enqdt,acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b.BRANCHCD='" + mbr + "' AND B.TYPE='30' AND B.SRNO=0) A,ITEM B,inspmst c,(SELECT SUM(A.MLT_LOSS) AS MLT_LOSS,SUM(A.MLT_LOSS1) AS MLT_LOSS1,SUM(A.MLT_LOSS2) AS MLT_LOSS2,sum(a.CORR_STG_REJ) AS CORR_STG_REJ,sum(a.snp) as snp,A.job_no,to_char(to_date(A.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy') as job_dt,A.icode FROM (select sum(A.mlt_loss) as mlt_loss,0 AS mlt_loss1,0 AS mlt_loss2 ,0  AS CORR_STG_REJ,0 as snp,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A ,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='09' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss, sum(A.mlt_loss) as mlt_loss1,0 AS mlt_loss2,0  AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS ICODE from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='06' group by A.job_no,A.job_Dt,TRIM(A.ICODE) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,sum(A.mlt_loss) as mlt_loss2,0 AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='11' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,sum(is_number(A.A4)) AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A where a.type='88' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,0 AS CORR_STG_REJ ,sum(is_number(a.col5)) as snp,A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode) AS icode from costestimate A where a.type='60' group by A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode)) A GROUP BY A.job_no,to_date(A.job_Dt,'dd/mm/yyyy'),A.icode) D WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy')||trim(a.acode)=trim(D.job_no)||to_char(to_Date(D.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy')||trim(D.icode) and c.type='70' and c.srno=10 ORDER BY TO_CHAR(A.ENQDT,'YYYYmmdd') desc,a.enqno desc) a, inspmst b  where trim(a.ERPCODE)=trim(b.icode) group by a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty ,a.WT_BOX_per_JC,a.Corrg_sheet_actual_produce, a.LINEAR_MTR ,a.PAPER_WT_REQ_JCRD,a.PAPR_CONSME_ACT,a.CORR_STG_REJ,a.CORR_WT_LOSS,a.sorting_packing_rej,a.sorting_packing_loss ,a.job_vALUE,a.WT_DIFF,a.WT_WISE_REJ_PER,a.FINAL_BOX_PROD,A.COMMENTS3";
                    SQuery = "SELECT a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty,round(sum(b.numwt),3) as bwt_spec,round(a.WT_BOX_per_JC,3) as WT_BOX_per_JC,round(a.Corrg_sheet_actual_produce,4) as Corrg_sheet_actual_produce,a.LINEAR_MTR,round(a.Corrg_sheet_actual_produce* sum(b.numwt),3) as paperWt_as_per_spec,round(a.PAPER_WT_REQ_JCRD,3) as PAPER_WT_REQ_JCRD,round((a.PAPER_WT_REQ_JCRD-(a.Corrg_sheet_actual_produce* sum(b.numwt))),3) as paperWt_diff ,round(a.PAPR_CONSME_ACT,3) as PAPR_CONSME_ACT,round(a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD,3) as excess_shot_paper_consumed, round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/ (case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),4) AS NO_OF_SHEET_TO_PRODUCED,round(a.CORR_STG_REJ,4) as CORR_STG_REJ,round(a.CORR_WT_LOSS,3) as CORR_WT_LOSS,round(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS),3) AS DIFF_AS_PER_JBCARD,/* ROUND(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS)/ a.PAPR_CONSME_ACT*100,2) AS CORR_WASTAGE_PER,*/round(a.sorting_packing_rej,4) as sorting_packing_rej,round(a.sorting_packing_loss,3) as sorting_packing_loss, a.job_vALUE,round(a.WT_DIFF,3) AS TOT_RJ_WT,/*round(a.WT_DIFF/a.WT_BOX_per_JC,4) as hypo_box,*/round(a.WT_WISE_REJ_PER,2) as WT_WISE_REJ_PER,/* ROUND(((round(a.PAPR_CONSME_ACT/ a.WT_BOX_per_JC,2)-round(a.FINAL_BOX_PROD,2))/round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/(case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),2))*100,2)  as BOX_WISE_REJ,*/round(a.FINAL_BOX_PROD,4) as FINAL_BOX_PROD,A.COMMENTS3,a.paper_cost FROM (SELECT trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy') as fstr,A.ENQNO AS JOBNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS JOBDT,A.ACODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO AS PARTNO,A.TOT_BOX_RCV as plan_qty,round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) AS WT_BOX_per_JC,A.QTYOUT AS Corrg_sheet_actual_produce,(CASE WHEN is_number(C.BTCHDT)>0 THEN ROUND((A.QTYOUT*is_number(C.BTCHDT))/100,2) ELSE 0 END) AS LINEAR_MTR,round(round(a.enr2/a.STD_SHT_RQ,4) * (CASE WHEN A.COL13>0 THEN round(A.QTYOUT/A.COL13,2) ELSE A.COL3 END),3) AS PAPER_WT_REQ_JCRD,A.QTYIN AS PAPR_CONSME_ACT,a.VAL as paper_cost,d.CORR_STG_REJ,d.CORR_STG_REJ*round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) as CORR_WT_LOSS,d.snp as sorting_packing_rej,(d.snp * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4)) as  sorting_packing_loss ,A.IQTYIN AS FINAL_BOX_PROD,A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4) as WT_DIFF,ROUND((A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4)) / (CASE WHEN A.QTYIN>0 THEN A.QTYIN ELSE 1 END)  * 100 , 3) AS WT_WISE_REJ_PER,round(((A.QTYOUT - A.IQTYIN) / (case when A.QTYOUT>0 then a.qtyout else 1 end)) * 100,3) AS BOX_WISE_REJ, (A.IQTYIN*A.SALERATE) as job_vALUE,A.COMMENTS3 FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,B.COL13,c.IRATE AS SALERATE,B.COMMENTS3 FROM (select enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from (select INVNO AS ENQNO,INVDATE AS ENQDT,TRIM(ICODE) AS ACODE,0 as qtyin,0 as qtyout,0 AS COL3,0 as scrp1,0 as scrp2,0 as time1,0 as time2,SUM(IQTYIN) AS IQTYIN,0 AS VAL from IVOUCHER where BRANCHCD='" + mbr + "' AND type='16' and INVDATE " + xprdrange + " group by INVNO,INVDATE,TRIM(ICODE) union all select A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS QTYIN1,SUM((case when b.irate>0 then ROUND(is_number(A.col4)*B.IRATE,2) else ROUND(is_number(A.col4)*c.IRATE,2) end)) AS VAL from item c,costestimate A left outer join REELVCH B on A.BRANCHCD||TRIM(A.ICODe)||TRIM(A.COL6)=B.BRANCHCD||TRIM(B.ICODe)||TRIM(B.KCLREELNO) and b.type in ('02','07') where trim(a.icode)=trim(c.icodE) and A.BRANCHCD='" + mbr + "' AND A.type='25' and A.enqdt " + xprdrange + " group by A.enqno,A.enqdt,TRIM(A.aCODE) union all select a.enqno,a.enqdt,TRIM(a.iCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join (SELECT VCHNUM,VCHDATE,SUM(is_number(replace(nvl(COL3,'0'),'-','0'))) AS COL3 from inspvch WHERE BRANCHCD='" + mbr + "' and type='45' group by vchnum,vchdate) b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') where a.BRANCHCD='" + mbr + "' and a.type='40' and a.enqdt " + xprdrange + " group by a.enqno,a.enqdt,TRIM(a.iCODE),is_number(replace(nvl(a.COL3,'0'),'-','0')) ) group by enqno,enqdt,acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b.BRANCHCD='" + mbr + "' AND B.TYPE='30' AND B.SRNO=0) A,ITEM B,inspmst c,(SELECT SUM(A.MLT_LOSS) AS MLT_LOSS,SUM(A.MLT_LOSS1) AS MLT_LOSS1,SUM(A.MLT_LOSS2) AS MLT_LOSS2,sum(a.CORR_STG_REJ) AS CORR_STG_REJ,sum(a.snp) as snp,A.job_no,to_char(to_date(A.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy') as job_dt,A.icode FROM (select sum(A.mlt_loss) as mlt_loss,0 AS mlt_loss1,0 AS mlt_loss2 ,0  AS CORR_STG_REJ,0 as snp,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A ,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='09' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss, sum(A.mlt_loss) as mlt_loss1,0 AS mlt_loss2,0  AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS ICODE from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='06' group by A.job_no,A.job_Dt,TRIM(A.ICODE) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,sum(A.mlt_loss) as mlt_loss2,0 AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='11' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,sum(is_number(A.A4)) AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A where a.type='88' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,0 AS CORR_STG_REJ ,sum(is_number(a.col5)) as snp,A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode) AS icode from costestimate A where a.type='60' group by A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode)) A GROUP BY A.job_no,to_date(A.job_Dt,'dd/mm/yyyy'),A.icode) D WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy')||trim(a.acode)=trim(D.job_no)||to_char(to_Date(D.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy')||trim(D.icode) and c.type='70' and c.srno=10 ORDER BY TO_CHAR(A.ENQDT,'YYYYmmdd') desc,a.enqno desc) a, inspmst b  where trim(a.ERPCODE)=trim(b.icode) group by a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty ,a.WT_BOX_per_JC,a.Corrg_sheet_actual_produce, a.LINEAR_MTR ,a.PAPER_WT_REQ_JCRD,a.PAPR_CONSME_ACT,a.CORR_STG_REJ,a.CORR_WT_LOSS,a.sorting_packing_rej,a.sorting_packing_loss ,a.job_vALUE,a.WT_DIFF,a.WT_WISE_REJ_PER,a.FINAL_BOX_PROD,A.COMMENTS3,a.paper_cost order by a.fstr";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);

                    mq2 = "select distinct trim(enqno) as jobno,to_char(enqdt,'dd/mm/yyyy') as jobdt, remarks from costestimate where branchcd='" + mbr + "' and type='25' and enqdt " + xprdrange + "";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "select distinct trim(job_no) as jobno,job_dt as jobdt,naration from prod_sheet where branchcd='" + mbr + "' and type='86' and to_date(job_dt,'dd/mm/yyyy') " + xprdrange + "";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    mq4 = "select trim(icode) as icode,sum(numwt) as numwt from inspmst where branchcd='" + mbr + "' group by trim(icode) order by icode";
                    mdt = new DataTable();
                    mdt = fgen.getdata(frm_qstr, co_cd, mq4);


                    dt6 = new DataTable();
                    dt6 = fgen.getdata(frm_qstr, co_cd, "Select distinct a.acode,b.aname,trim(a.vchnum) as jobno,to_char(a.vchdate,'dd/mm/yyyy') as jobdt from costestimate a,famst b where trim(a.acode)=trim(B.acodE) and a.branchcd='" + mbr + "' and a.type='30' and a.vchdate " + xprdrange + " ");

                    oporow = null;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
                        oporow = dt.NewRow();
                        oporow["JOBNO"] = dr["JOBNO"].ToString().Trim();
                        oporow["JOBDT"] = dr["JOBDT"].ToString().Trim();
                        oporow["PARTY"] = fgen.seek_iname_dt(dt6, "jobno='" + dr["JOBNO"].ToString().Trim() + "' and jobdt='" + dr["JOBDT"].ToString().Trim() + "'", "aname");
                        oporow["ERPCODE"] = dr["ERPCODE"].ToString().Trim();
                        oporow["PRODUCT"] = dr["PRODUCT"].ToString().Trim();
                        oporow["PARTNO"] = dr["PARTNO"].ToString().Trim();
                        oporow["PLAN_QTY"] = fgen.make_double(dr["plan_qty"].ToString().Trim());
                        oporow["BOX_WT_as_per_Spec_KG"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(mdt, "icode='" + dr["ERPCODE"].ToString().Trim() + "'", "numwt")), 3);
                        oporow["BOX_WT_as_per_JOB_CARD_KG"] = Math.Round(fgen.make_double(dr["WT_BOX_per_JC"].ToString().Trim()), 3);
                        oporow["Corrugation_Sheets_Actually_Produced_NOS"] = fgen.make_double(dr["Corrg_sheet_actual_produce"].ToString().Trim());
                        oporow["LINEAR_MTR"] = fgen.make_double(dr["LINEAR_MTR"].ToString().Trim());
                        db1 = fgen.make_double(oporow["Corrugation_Sheets_Actually_Produced_NOS"].ToString().Trim()) * fgen.make_double(fgen.seek_iname_dt(mdt, "icode='" + dr["ERPCODE"].ToString().Trim() + "'", "numwt"));
                        oporow["Paper_wt_reqd_as_per_spec_KG"] = Math.Round(db1, 3);
                        db2 = fgen.make_double(oporow["Corrugation_Sheets_Actually_Produced_NOS"].ToString().Trim()) * fgen.make_double(oporow["BOX_WT_as_per_JOB_CARD_KG"].ToString().Trim());
                        //dog
                        oporow["Paper_wt_reqd_as_per_JOB_CARD_KG"] = Math.Round(db2, 3);
                        oporow["Paper_wt_diff-Spec_vs_Job_card_KG"] = Math.Round(db2 - db1, 3);
                        oporow["Paper_consumed_Actual_KG"] = Math.Round(fgen.make_double(dr["PAPR_CONSME_ACT"].ToString().Trim()), 3);
                        oporow["Paper_Cost"] = Math.Round(fgen.make_double(dr["paper_cost"].ToString().Trim()), 3);
                        oporow["excess/short_paper_consumed_against_Jobcard_KG"] = Math.Round(fgen.make_double(oporow["Paper_consumed_Actual_KG"].ToString().Trim()) - db2, 3);
                        oporow["NO_OF_SHEETs_supposed_TO_be_PRODUCED_asper_JobCard_Wt_NOS"] = fgen.make_double(oporow["Paper_consumed_Actual_KG"].ToString().Trim()) / fgen.make_double(oporow["BOX_WT_as_per_JOB_CARD_KG"].ToString().Trim());
                        oporow["Corrugation_Rejection_NOS"] = fgen.make_double(dr["CORR_STG_REJ"].ToString().Trim());
                        db3 = fgen.make_double(oporow["Corrugation_Rejection_NOS"].ToString().Trim()) * fgen.make_double(oporow["BOX_WT_as_per_JOB_CARD_KG"].ToString().Trim());
                        oporow["Rejection_sheet_wt_as_per_JobCard_KG"] = Math.Round(db3, 3);
                        db4 = db3 + (fgen.make_double(oporow["Paper_consumed_Actual_KG"].ToString().Trim()) - db2);
                        oporow["Rejection_sheet_wt+Paper_wt_diff_as_per_Job_Card_KG"] = Math.Round(db4, 3);
                        oporow["CORR_WASTAGE_%"] = Math.Round((db4 / fgen.make_double(oporow["Paper_consumed_Actual_KG"].ToString())) * 100, 2);
                        oporow["Conversion_Rej_Qty_NOS"] = fgen.make_double(dr["sorting_packing_rej"].ToString().Trim());
                        oporow["Conversion_Rej_Wt_KG"] = Math.Round(fgen.make_double(oporow["Conversion_Rej_Qty_NOS"].ToString().Trim()) * fgen.make_double(oporow["BOX_WT_as_per_JOB_CARD_KG"].ToString().Trim()), 3);
                        oporow["Final_box_Qty_NOS"] = fgen.make_double(oporow["Corrugation_Sheets_Actually_Produced_NOS"].ToString().Trim()) - fgen.make_double(oporow["Corrugation_Rejection_NOS"].ToString().Trim()) - fgen.make_double(oporow["Conversion_Rej_Qty_NOS"].ToString().Trim());
                        db5 = fgen.make_double(oporow["Conversion_Rej_Wt_KG"].ToString().Trim()) + fgen.make_double(oporow["excess/short_paper_consumed_against_Jobcard_KG"].ToString().Trim()) + fgen.make_double(oporow["Rejection_sheet_wt_as_per_JobCard_KG"].ToString().Trim());
                        oporow["Total_Rejection_Wt_KG"] = Math.Round(db5, 3);
                        oporow["FINAL_BOX_WT_KG"] = fgen.make_double(oporow["Final_box_Qty_NOS"].ToString()) * fgen.make_double(oporow["BOX_WT_as_per_JOB_CARD_KG"].ToString());
                        oporow["Hypo_Box_Lost/Gained_Due_to_Wastage_NOS"] = Math.Round(db5 / fgen.make_double(oporow["BOX_WT_as_per_JOB_CARD_KG"].ToString().Trim()), 0);
                        oporow["Wt_wise_rejection_%"] = Math.Round((db5 / fgen.make_double(oporow["Paper_consumed_Actual_KG"].ToString().Trim())) * 100, 2);
                        oporow["BOX_WISE_rejection_%"] = Math.Round((fgen.make_double(oporow["Hypo_Box_Lost/Gained_Due_to_Wastage_NOS"].ToString().Trim()) / fgen.make_double(oporow["NO_OF_SHEETs_supposed_TO_be_PRODUCED_asper_JobCard_Wt_NOS"].ToString().Trim())) * 100, 2);
                        oporow["REMARKS_FROM_CONVERSION"] = fgen.seek_iname_dt(dt3, "jobno='" + dr["JOBNO"].ToString().Trim() + "' and jobdt='" + dr["JOBDT"].ToString().Trim() + "'", "naration");
                        oporow["REMARKS_FROM_CORRUGATION"] = fgen.seek_iname_dt(dt2, "jobno='" + dr["JOBNO"].ToString().Trim() + "' and jobdt='" + dr["JOBDT"].ToString().Trim() + "'", "remarks");
                        dt.Rows.Add(oporow);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        oporow = dt.NewRow();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            itot_stk = 0; to_cons = 0; itv = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 7 || dc.Ordinal == 29 || dc.Ordinal == 30 || dc.Ordinal == 15)
                            {

                            }
                            else if (dc.Ordinal == 17)
                            {
                                mq1 = "sum([" + dc.ColumnName + "])";
                                itot_stk += fgen.make_double(dt.Compute(mq1, "").ToString());
                                oporow[dc] = Math.Round(itot_stk, 0);
                            }
                            else if (dc.Ordinal == 21)
                            {
                                mq1 = "sum([Rejection_sheet_wt+Paper_wt_diff_as_per_Job_Card_KG])";
                                to_cons = fgen.make_double(dt.Compute(mq1, "").ToString());
                                mq1 = "sum([Paper_consumed_Actual_KG])";
                                itv = fgen.make_double(dt.Compute(mq1, "").ToString());
                                oporow[dc] = Math.Round((to_cons / itv) * 100, 2);
                            }
                            else if (dc.Ordinal == 28)
                            {
                                mq1 = "sum([Total_Rejection_Wt_KG])";
                                to_cons = fgen.make_double(dt.Compute(mq1, "").ToString());
                                mq1 = "sum([Paper_consumed_Actual_KG])";
                                itv = fgen.make_double(dt.Compute(mq1, "").ToString());
                                oporow[dc] = Math.Round((to_cons / itv) * 100, 2);
                            }
                            else
                            {
                                try
                                {
                                    mq1 = "sum([" + dc.ColumnName + "])";
                                    itot_stk += fgen.make_double(dt.Compute(mq1, "").ToString());
                                    oporow[dc] = itot_stk;
                                }
                                catch
                                {

                                }
                            }
                        }
                        oporow["PRODUCT"] = "GRAND TOTAL";
                        dt.Rows.InsertAt(oporow, 0);
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Production Report (As Per Specification) for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;
                #endregion

                case "F40329D":
                case "F40329E":
                    mq0 = value1;
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R40'", "params");
                    if (m1 == "0") m1 = cDT1;
                    xprdrange = "between to_Date('" + m1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    mq1 = "Location Wise Reel Number Wise Stock";
                    SQuery = "select ' 'as empty,a.icode as erpcode,d.iname as product,trim(d.cpartno) as part_no,d.oprate3||' GSM' as gsm,a.kclreelno AS our_reel_no,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance,d.bfactor from (select branchcd,icode,kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout,max(rlocn) as rlocn from (select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " union all select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch_op where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " ) group by branchcd,icode,kclreelno  having sum(reelwin)-sum(reelwout)>0) a,item d where trim(a.icode)=trim(d.icodE) order by erpcode";
                    SQuery = "select ' 'as empty,a.icode as erpcode,e.iname as subgroup,d.iname as product,trim(d.cpartno) as part_no,d.oprate3||' GSM' as gsm,a.kclreelno AS our_reel_no,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance,d.bfactor from (select branchcd,icode,kclreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout,max(rlocn) as location from (select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " union all select branchcd,trim(icode) as icode,kclreelno,reelwin,reelwout,trim(rlocn) as rlocn from reelvch_op where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " ) group by branchcd,icode,kclreelno  having sum(reelwin)-sum(reelwout)>0) a,item d,item e where trim(a.icode)=trim(d.icodE) and substr(trim(a.icode),1,4)=trim(e.icode) and length(trim(e.icodE))=4 order by erpcode";
                    cond = "";
                    header_n = "Reel Summary Report GSM, Size Wise";
                    if (frm_formID == "F40329E")
                    {
                        cond = ",BFACTOR,SUBGROUP";
                        header_n = "Reel Summary Report GSM, Size, BF Wise";
                    }
                    SQuery = "SELECT PART_NO" + cond + ",GSM,SUM(balance) AS balance,COUNT(ERPCODE) AS NUM1 FROM (" + SQuery + ") GROUP BY PART_NO" + cond + ",GSM ORDER BY GSM" + cond + ",PART_NO ";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    //fgen.Fn_Print_Report(co_cd, frm_qstr, mbr, SQuery, "cross", "cross");

                    DataView myDv = new DataView(dt, "", "GSM" + cond + ",PART_NO", DataViewRowState.CurrentRows);
                    dt1 = new DataTable();
                    if (cond != "") dt1 = myDv.ToTable(true, "PART_NO", "BFACTOR", "SUBGROUP");
                    else dt1 = myDv.ToTable(true, "PART_NO");

                    dt2 = new DataTable();
                    dt2 = myDv.ToTable(true, "GSM");

                    DataTable myDt = new DataTable();
                    myDt.Columns.Add("SIZE", typeof(string));
                    if (cond != "")
                    {
                        myDt.Columns.Add("BF", typeof(string));
                        myDt.Columns.Add("SUBGROUP", typeof(string));
                    }
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        myDt.Columns.Add(dt2.Rows[i]["GSM"].ToString().Trim() + " Reels", typeof(string));
                        myDt.Columns.Add(dt2.Rows[i]["GSM"].ToString().Trim() + " Stock", typeof(string));
                    }
                    DataRow mydr = null;
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        mydr = myDt.NewRow();
                        mydr["SIZE"] = dt1.Rows[i]["PART_NO"].ToString().Trim();
                        if (cond != "")
                        {
                            mydr["BF"] = dt1.Rows[i]["BFACTOR"].ToString().Trim() + " BF";
                            mydr["SUBGROUP"] = dt1.Rows[i]["SUBGROUP"].ToString().Trim();
                        }
                        if (cond != "")
                            myDv = new DataView(dt, "PART_NO='" + mydr["size"].ToString().Trim() + "' AND BFACTOR='" + mydr["BF"].ToString().Replace("BF", "").Trim() + "' AND SUBGROUP='" + mydr["SUBGROUP"].ToString().Trim() + "' ", "GSM", DataViewRowState.CurrentRows);
                        else myDv = new DataView(dt, "PART_NO='" + mydr["size"].ToString().Trim() + "'", "GSM", DataViewRowState.CurrentRows);
                        for (int x = 0; x < myDv.Count; x++)
                        {
                            mydr[myDv[x].Row["GSM"].ToString().Trim() + " Reels"] = myDv[x].Row["NUM1"].ToString().Trim();
                            mydr[myDv[x].Row["GSM"].ToString().Trim() + " Stock"] = myDv[x].Row["balance"].ToString().Trim();
                        }
                        myDt.Rows.Add(mydr);
                    }

                    Session["send_dt"] = myDt;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                    break;
                case "F35229OLD":
                    #region
                    header_n = "Paper Variation Report Code";
                    dt3 = new DataTable();
                    dt3.Columns.Add("jobno", typeof(string));
                    dt3.Columns.Add("jobdt", typeof(string));
                    dt3.Columns.Add("Product", typeof(string));
                    dt3.Columns.Add("Ply", typeof(string));
                    dt3.Columns.Add("Jobcard Qty", typeof(string));
                    dt3.Columns.Add("Paper Required As per Spec", typeof(string));
                    dt3.Columns.Add("Paper_Qty_Req_as_per_spec", typeof(string));
                    dt3.Columns.Add("Paper Required As per Job Card", typeof(string));
                    dt3.Columns.Add("Paper_Qty_as_per_JC", typeof(double));
                    dt3.Columns.Add("Paper Consumed", typeof(string));
                    dt3.Columns.Add("Qty_Used", typeof(double));
                    dt3.Columns.Add("DiffKgs", typeof(double));

                    dt = new DataTable();
                    //SQuery = "select trim(vchnum) as jobno,trim(vchdate) as jobdt,(col9) as col9,(iname) as iname,(icode) as icode,sum(col4) as col4,sum(prodqty) as prodqty,col2,qty as totqty,jcqty as jcqty from (select a.vchnum,a.vchdate,c.iname as col9,b.iname as iname,trim(a.icode) as icode,sum(is_number(a.col4)) as col4,0 as prodqty,null as col2,is_number(nvl(a.col7,0)) as qty,a.qty as jcqty , 1 as ordered from costestimate a ,item c , item b where trim(a.icode)=trim(b.icode) and trim(a.col9)=trim(c.icode) and substr(a.col9,1,2) in ('07','08','09')  and a.branchcd='" + mbr + "' and a.type='30'  and a.vchdate " + xprdrange + "  group by a.vchnum,a.vchdate,c.iname,a.icode,b.iname,is_number(nvl(a.col7,0)),a.qty union all select enqno vchnum,enqdt vchdate,null col9,null as icode,null as iname,0 as col4,sum(is_number(col4)) as prodqty,col2,0 as qty,0 as jcqty,2 as ordered from costestimate where  branchcd='" + mbr + "' and type='25'  and enqdt " + xprdrange + "  group by enqno,enqdt,col2 order by  vchnum desc ) group by trim(vchnum),trim(vchdate),col2,iname,icode,col9,qty,jcqty order by jobno desc, min(ordered)";
                    SQuery = "select trim(vchnum) as jobno,trim(vchdate) as jobdt,(col9) as col9,(iname) as iname,(icode) as icode,(col4) as col4,(prodqty) as prodqty,col2,qty as totqty,jcqty as jcqty,cicode from (select a.vchnum,a.vchdate,a.col9 as cicode,c.iname as col9,b.iname as iname,trim(a.icode) as icode,(is_number(a.col4)) as col4,0 as prodqty,null as col2,is_number(nvl(a.col5,0)) as qty,a.qty as jcqty , 1 as ordered from costestimate a ,item c , item b where trim(a.icode)=trim(b.icode) and trim(a.col9)=trim(c.icode) and substr(a.col9,1,2) in ('07','08','09') and a.branchcd='" + mbr + "' and a.type='30'  and a.vchdate " + xprdrange + " union all select enqno vchnum,enqdt vchdate,null as cicode,null col9,null as icode,null as iname,0 as col4,sum(is_number(col4)) as prodqty,col2,0 as qty,0 as jcqty,2 as ordered from costestimate where branchcd='" + mbr + "' and type='25'  and enqdt " + xprdrange + " group by enqno,enqdt,col2 order by  vchnum desc ) order by jobno desc, ordered";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//jobcard dt...main dt

                    dt2 = new DataTable();
                    //SQuery = "select distinct col15 as ply,icode from inspmst where branchcd='" + mbr + "' and type='70' ";
                    SQuery = "select a.acode,a.grade,a.col16,a.col15 as ply,a.col13,a.btchno,a.maintdt,a.BTCHDT,a.numwt,nvl(a.col1,'-') as col1,nvl(a.col18,'-') as col18,nvl(a.col2,'-') as col2,nvl(a.col3,'-') as col3,nvl(a.col4,'-') as col4,nvl(a.col5,'-') as col5,nvl(a.col10,'-') as col10,nvl(a.col11,'-') as col11,a.rejqty,a.recalib,a.icode,b.iname from inspmst a,item b where trim(a.col5)=trim(b.icode) and substr(col5,1,2) in ('07','08','09') and a.BRANCHCD='" + mbr + "' AND a.type='70' and a.vchnum<>'000000' AND nvl(a.col5,'-')!='-' order by a.srno ";
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);//cosnumed 

                    dt4 = new DataTable();
                    SQuery = "SELECT ENQNO,ENQDT,ACODE,SUM(QTYOUT) AS QTYOUT FROM (select a.enqno,a.enqdt,TRIM(a.aCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join (SELECT VCHNUM,VCHDATE,SUM(is_number(replace(nvl(COL3,'0'),'-','0'))) AS COL3 from inspvch WHERE BRANCHCD='" + mbr + "' and type='45' group by vchnum,vchdate) b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') where a.BRANCHCD='" + mbr + "' and a.type='40' group by a.enqno,a.enqdt,TRIM(a.aCODE),is_number(replace(nvl(a.COL3,'0'),'-','0')) ) GROUP BY ENQNO,ENQDT,ACODE";
                    dt4 = fgen.getdata(frm_qstr, co_cd, SQuery);//cosnumed 

                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtDistJC = new DataTable();
                        //dtDistJC

                        DataView distJCView = new DataView(dt, "", "", DataViewRowState.CurrentRows);
                        dtDistJC = distJCView.ToTable(true, "jobno", "jobdt");

                        foreach (DataRow drdistJC in dtDistJC.Rows)
                        {
                            DataView jcview = new DataView(dt, "jobno='" + drdistJC["jobno"].ToString() + "' and jobdt='" + drdistJC["jobdt"].ToString() + "' ", "", DataViewRowState.CurrentRows);
                            int startingIndex = 0, endIndex = 0;
                            int startingIndex1 = 0, endIndex1 = 0;
                            for (int i = 0; i < jcview.Count; i++)
                            {
                                if (i == 0) startingIndex = dt3.Rows.Count;
                                if (jcview[i].Row["icode"].ToString().Trim().Length > 5)
                                {
                                    dr2 = dt3.NewRow();
                                    dr2["jobno"] = jcview[i].Row["jobno"].ToString().Trim();
                                    dr2["jobdt"] = jcview[i].Row["jobdt"].ToString().Trim();
                                    double totpr1 = 0;
                                    totpr1 = fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + jcview[i].Row["icode"].ToString().Trim() + "' and enqno='" + dr2["jobno"].ToString().Trim() + "' AND enqdt='" + dr2["jobdt"].ToString().Trim() + "'  ", "qtyout"));
                                    if (jcview[i].Row["iname"].ToString().Trim().Length > 5)
                                    {
                                        db1 = 0; db2 = 0; db3 = 0;
                                        if (col1 != dr2["jobno"].ToString())
                                        {
                                            col1 = dr2["jobno"].ToString();
                                            dr2["Product"] = jcview[i].Row["iname"].ToString().Trim();
                                            dr2["Ply"] = fgen.seek_iname_dt(dt2, "icode='" + jcview[i].Row["icode"].ToString().Trim() + "'", "ply");
                                            //dr2["Paper_Qty_Req_as_per_spec"] = jcview[i].Row["iname"].ToString().Trim();
                                            dr2["Jobcard Qty"] = jcview[i].Row["jcqty"].ToString().Trim();

                                            DataView vdview = new DataView(dt, "JOBNO='" + dr2["jobno"] + "' AND JOBDT='" + dr2["jobDT"] + "'", "", DataViewRowState.CurrentRows);
                                            for (int x = 0; x < vdview.Count; x++)
                                            {
                                                db1 += fgen.make_double(vdview[x].Row["totqty"].ToString().Trim());
                                                db2 += fgen.make_double(vdview[x].Row["prodqty"].ToString().Trim());
                                                //db2 += fgen.make_double(vdview[x].Row["numwt"].ToString().Trim()) * totpr1;
                                            }

                                            DataView vdview3 = new DataView(dt2, "ICODE='" + jcview[i].Row["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                            for (int y = 0; y < vdview3.Count; y++)
                                            {
                                                db3 += fgen.make_double(vdview3[y].Row["numwt"].ToString().Trim()) * totpr1;
                                            }

                                            dr2["DiffKgs"] = Math.Round(db2 - db3, 2);
                                        }
                                    }
                                    dr2["Paper Required As per Job Card"] = jcview[i].Row["col9"].ToString().Trim();
                                    //dr2["Paper_Qty_as_per_JC"] = fgen.make_double(jcview[i].Row["totqty"].ToString().Trim());
                                    //cond = "";
                                    //cond = fgen.seek_iname_dt(dt2, "ICODE='" + jcview[i].Row["icode"].ToString().Trim() + "' AND COL5='" + jcview[i].Row["cicode"].ToString().Trim() + "' ", "numwt");
                                    //if (cond != "")
                                    //    dr2["Paper_Qty_as_per_JC"] = totpr1 * fgen.make_double(cond);
                                    //else 
                                    dr2["Paper_Qty_as_per_JC"] = fgen.make_double(jcview[i].Row["totqty"].ToString().Trim());

                                    //dr2["Paper Consumed"] = jcview[i].Row["col2"].ToString().Trim();
                                    //dr2["Qty_Used"] = fgen.make_double(jcview[i].Row["prodqty"].ToString().Trim());

                                    dt3.Rows.Add(dr2);
                                }
                            }


                            if (jcview.Count > 0)
                            {
                                if (startingIndex == -1) startingIndex = 0;
                                endIndex = startingIndex + jcview.Count;
                                DataView vdview2 = new DataView(dt2, "ICODE='" + jcview[0].Row["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                DataView vdview7 = new DataView(dt, "JOBNO='" + jcview[0].Row["jobno"].ToString().Trim() + "' AND PRODQTY>0", "", DataViewRowState.CurrentRows);
                                int myInd = 0, myindd = 0;
                                double totpr = 0;

                                totpr = fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + jcview[0].Row["icode"].ToString().Trim() + "'  and enqno='" + jcview[0].Row["jobno"].ToString().Trim() + "' AND enqdt='" + jcview[0].Row["jobdt"].ToString().Trim() + "' ", "qtyout"));

                                for (int i = startingIndex; i < endIndex; i++)
                                {
                                    if (vdview2.Count > myInd)
                                    {
                                        dt3.Rows[i]["Paper Required As per Spec"] = vdview2[myInd].Row["iname"].ToString().Trim();
                                        dt3.Rows[i]["Paper_Qty_Req_as_per_spec"] = Math.Round(fgen.make_double(vdview2[myInd].Row["numwt"].ToString().Trim()) * totpr, 2);

                                        myInd++;
                                    }

                                    //if (vdview7.Count > myindd)
                                    //{

                                    //    dt3.Rows[i]["Paper Consumed"] = vdview7[myindd].Row["col2"].ToString().Trim();
                                    //    dt3.Rows[i]["Qty_Used"] = fgen.make_double(vdview7[myindd].Row["prodqty"].ToString().Trim());

                                    //    myindd++;
                                    //}
                                }
                                DataView distcountv = new DataView(dt3, "JOBNO='" + jcview[0].Row["jobno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                int rowtoadd = vdview7.Count - distcountv.Count;
                                for (int i = 0; i < rowtoadd; i++)
                                {
                                    dr2 = dt3.NewRow();
                                    dr2["jobno"] = jcview[0].Row["jobno"].ToString().Trim();
                                    dr2["jobdt"] = jcview[0].Row["jobdt"].ToString().Trim();
                                    dt3.Rows.Add(dr2);
                                }
                                myindd = 0;
                                if (vdview7.Count > 0)
                                {
                                    for (int i = startingIndex; i < (startingIndex + vdview7.Count); i++)
                                    {
                                        dt3.Rows[i]["Paper Consumed"] = vdview7[myindd].Row["col2"].ToString().Trim();
                                        dt3.Rows[i]["Qty_Used"] = fgen.make_double(vdview7[myindd].Row["prodqty"].ToString().Trim());
                                        myindd++;
                                    }
                                }
                            }
                        }

                        if (dt3.Rows.Count > 0)
                        {
                            Session["send_dt"] = dt3;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                        }
                    }
                    else fgen.msg("-", "AMSG", "No Data Found!!");
                    #endregion
                    break;
                case "F35229":
                    #region
                    header_n = "Paper Variation Report Code";
                    dt3 = new DataTable();
                    dt3.Columns.Add("jobno", typeof(string));
                    dt3.Columns.Add("jobdt", typeof(string));
                    dt3.Columns.Add("Product", typeof(string));
                    dt3.Columns.Add("Ply", typeof(string));
                    dt3.Columns.Add("Jobcard Qty", typeof(string));
                    dt3.Columns.Add("Prod_Qty", typeof(string));//new
                    dt3.Columns.Add("Paper Required As per Spec", typeof(string));
                    dt3.Columns.Add("Paper_Qty_Req_as_per_spec", typeof(string));
                    dt3.Columns.Add("Paper Required As per Job Card", typeof(string));
                    dt3.Columns.Add("Paper_Qty_as_per_JC", typeof(double));
                    dt3.Columns.Add("Paper Consumed", typeof(string));
                    dt3.Columns.Add("Qty_Used", typeof(double));
                    dt3.Columns.Add("DiffKgs", typeof(double));

                    dt = new DataTable();
                    //SQuery = "select trim(vchnum) as jobno,trim(vchdate) as jobdt,(col9) as col9,(iname) as iname,(icode) as icode,sum(col4) as col4,sum(prodqty) as prodqty,col2,qty as totqty,jcqty as jcqty from (select a.vchnum,a.vchdate,c.iname as col9,b.iname as iname,trim(a.icode) as icode,sum(is_number(a.col4)) as col4,0 as prodqty,null as col2,is_number(nvl(a.col7,0)) as qty,a.qty as jcqty , 1 as ordered from costestimate a ,item c , item b where trim(a.icode)=trim(b.icode) and trim(a.col9)=trim(c.icode) and substr(a.col9,1,2) in ('07','08','09')  and a.branchcd='" + mbr + "' and a.type='30'  and a.vchdate " + xprdrange + "  group by a.vchnum,a.vchdate,c.iname,a.icode,b.iname,is_number(nvl(a.col7,0)),a.qty union all select enqno vchnum,enqdt vchdate,null col9,null as icode,null as iname,0 as col4,sum(is_number(col4)) as prodqty,col2,0 as qty,0 as jcqty,2 as ordered from costestimate where  branchcd='" + mbr + "' and type='25'  and enqdt " + xprdrange + "  group by enqno,enqdt,col2 order by  vchnum desc ) group by trim(vchnum),trim(vchdate),col2,iname,icode,col9,qty,jcqty order by jobno desc, min(ordered)";
                    SQuery = "select trim(vchnum) as jobno,trim(vchdate) as jobdt,(col9) as col9,(iname) as iname,(icode) as icode,(col4) as col4,(prodqty) as prodqty,col2,qty as totqty,jcqty as jcqty,cicode from (select a.vchnum,a.vchdate,a.col9 as cicode,c.iname as col9,b.iname as iname,trim(a.icode) as icode,(is_number(a.col4)) as col4,0 as prodqty,null as col2,is_number(nvl(a.col5,0)) as qty,a.qty as jcqty , 1 as ordered from costestimate a ,item c , item b where trim(a.icode)=trim(b.icode) and trim(a.col9)=trim(c.icode) and substr(a.col9,1,2) in ('07','08','09') and a.branchcd='" + mbr + "' and a.type='30'  and a.vchdate " + xprdrange + " union all select enqno vchnum,enqdt vchdate,null as cicode,null col9,null as icode,null as iname,0 as col4,sum(is_number(col4)) as prodqty,col2,0 as qty,0 as jcqty,2 as ordered from costestimate where branchcd='" + mbr + "' and type='25'  and enqdt " + xprdrange + " group by enqno,enqdt,col2 order by  vchnum desc )  order by jobno desc, ordered";//where trim(vchnum)='000886' 
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);//jobcard dt...main dt

                    SQuery = "SELECT a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty,round(sum(b.numwt),3) as bwt_spec,round(a.WT_BOX_per_JC,3) as WT_BOX_per_JC,round(a.Corrg_sheet_actual_produce,4) as Corrg_sheet_actual_produce,a.LINEAR_MTR,round(a.Corrg_sheet_actual_produce* sum(b.numwt),3) as paperWt_as_per_spec,round(a.PAPER_WT_REQ_JCRD,3) as PAPER_WT_REQ_JCRD,round((a.PAPER_WT_REQ_JCRD-(a.Corrg_sheet_actual_produce* sum(b.numwt))),3) as paperWt_diff ,round(a.PAPR_CONSME_ACT,3) as PAPR_CONSME_ACT,round(a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD,3) as excess_shot_paper_consumed, round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/ (case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),4) AS NO_OF_SHEET_TO_PRODUCED,round(a.CORR_STG_REJ,4) as CORR_STG_REJ,round(a.CORR_WT_LOSS,3) as CORR_WT_LOSS,round(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS),3) AS DIFF_AS_PER_JBCARD,/* ROUND(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS)/ a.PAPR_CONSME_ACT*100,2) AS CORR_WASTAGE_PER,*/round(a.sorting_packing_rej,4) as sorting_packing_rej,round(a.sorting_packing_loss,3) as sorting_packing_loss, a.job_vALUE,round(a.WT_DIFF,3) AS TOT_RJ_WT,/*round(a.WT_DIFF/a.WT_BOX_per_JC,4) as hypo_box,*/round(a.WT_WISE_REJ_PER,2) as WT_WISE_REJ_PER,/* ROUND(((round(a.PAPR_CONSME_ACT/ a.WT_BOX_per_JC,2)-round(a.FINAL_BOX_PROD,2))/round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/(case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),2))*100,2)  as BOX_WISE_REJ,*/round(a.FINAL_BOX_PROD,4) as FINAL_BOX_PROD,A.COMMENTS3,a.paper_cost FROM (SELECT trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy') as fstr,A.ENQNO AS JOBNO,TO_CHAR(A.ENQDT,'DD/MM/YYYY') AS JOBDT,A.ACODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO AS PARTNO,A.TOT_BOX_RCV as plan_qty,round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) AS WT_BOX_per_JC,A.QTYOUT AS Corrg_sheet_actual_produce,(CASE WHEN is_number(C.BTCHDT)>0 THEN ROUND((A.QTYOUT*is_number(C.BTCHDT))/100,2) ELSE 0 END) AS LINEAR_MTR,round(round(a.enr2/a.STD_SHT_RQ,4) * (CASE WHEN A.COL13>0 THEN round(A.QTYOUT/A.COL13,2) ELSE A.COL3 END),3) AS PAPER_WT_REQ_JCRD,A.QTYIN AS PAPR_CONSME_ACT,a.VAL as paper_cost,d.CORR_STG_REJ,d.CORR_STG_REJ*round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) as CORR_WT_LOSS,d.snp as sorting_packing_rej,(d.snp * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4)) as  sorting_packing_loss ,A.IQTYIN AS FINAL_BOX_PROD,A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4) as WT_DIFF,ROUND((A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4)) / (CASE WHEN A.QTYIN>0 THEN A.QTYIN ELSE 1 END)  * 100 , 3) AS WT_WISE_REJ_PER,round(((A.QTYOUT - A.IQTYIN) / (case when A.QTYOUT>0 then a.qtyout else 1 end)) * 100,3) AS BOX_WISE_REJ, (A.IQTYIN*A.SALERATE) as job_vALUE,A.COMMENTS3 FROM (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,B.COL13,c.IRATE AS SALERATE,B.COMMENTS3 FROM (select enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from (select INVNO AS ENQNO,INVDATE AS ENQDT,TRIM(ICODE) AS ACODE,0 as qtyin,0 as qtyout,0 AS COL3,0 as scrp1,0 as scrp2,0 as time1,0 as time2,SUM(IQTYIN) AS IQTYIN,0 AS VAL from IVOUCHER where BRANCHCD='" + mbr + "' AND type='16' and INVDATE " + xprdrange + " group by INVNO,INVDATE,TRIM(ICODE) union all select A.enqno,A.enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS QTYIN1,SUM((case when b.irate>0 then ROUND(is_number(A.col4)*B.IRATE,2) else ROUND(is_number(A.col4)*c.IRATE,2) end)) AS VAL from item c,costestimate A left outer join REELVCH B on A.BRANCHCD||TRIM(A.ICODe)||TRIM(A.COL6)=B.BRANCHCD||TRIM(B.ICODe)||TRIM(B.KCLREELNO) and b.type in ('02','07') where trim(a.icode)=trim(c.icodE) and A.BRANCHCD='" + mbr + "' AND A.type='25' and A.enqdt " + xprdrange + " group by A.enqno,A.enqdt,TRIM(A.aCODE) union all select a.enqno,a.enqdt,TRIM(a.iCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join (SELECT VCHNUM,VCHDATE,SUM(is_number(replace(nvl(COL3,'0'),'-','0'))) AS COL3 from inspvch WHERE BRANCHCD='" + mbr + "' and type='45' group by vchnum,vchdate) b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') where a.BRANCHCD='" + mbr + "' and a.type='40' and a.enqdt " + xprdrange + " group by a.enqno,a.enqdt,TRIM(a.iCODE),is_number(replace(nvl(a.COL3,'0'),'-','0')) ) group by enqno,enqdt,acode) A  ,COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||TO_CHAR(A.ENQDT,'DD/MM/YYYY')=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b.BRANCHCD='" + mbr + "' AND B.TYPE='30' AND B.SRNO=0) A,ITEM B,inspmst c,(SELECT SUM(A.MLT_LOSS) AS MLT_LOSS,SUM(A.MLT_LOSS1) AS MLT_LOSS1,SUM(A.MLT_LOSS2) AS MLT_LOSS2,sum(a.CORR_STG_REJ) AS CORR_STG_REJ,sum(a.snp) as snp,A.job_no,to_char(to_date(A.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy') as job_dt,A.icode FROM (select sum(A.mlt_loss) as mlt_loss,0 AS mlt_loss1,0 AS mlt_loss2 ,0  AS CORR_STG_REJ,0 as snp,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A ,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='09' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss, sum(A.mlt_loss) as mlt_loss1,0 AS mlt_loss2,0  AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS ICODE from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='06' group by A.job_no,A.job_Dt,TRIM(A.ICODE) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,sum(A.mlt_loss) as mlt_loss2,0 AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='11' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,sum(is_number(A.A4)) AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A where a.type='88' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,0 AS CORR_STG_REJ ,sum(is_number(a.col5)) as snp,A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode) AS icode from costestimate A where a.type='60' group by A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode)) A GROUP BY A.job_no,to_date(A.job_Dt,'dd/mm/yyyy'),A.icode) D WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and trim(a.enqno)||to_char(a.enqdt,'dd/mm/yyyy')||trim(a.acode)=trim(D.job_no)||to_char(to_Date(D.job_Dt,'dd/mm/yyyy'),'dd/mm/yyyy')||trim(D.icode) and c.type='70' and c.srno=10 ORDER BY TO_CHAR(A.ENQDT,'YYYYmmdd') desc,a.enqno desc) a, inspmst b  where trim(a.ERPCODE)=trim(b.icode) group by a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty ,a.WT_BOX_per_JC,a.Corrg_sheet_actual_produce, a.LINEAR_MTR ,a.PAPER_WT_REQ_JCRD,a.PAPR_CONSME_ACT,a.CORR_STG_REJ,a.CORR_WT_LOSS,a.sorting_packing_rej,a.sorting_packing_loss ,a.job_vALUE,a.WT_DIFF,a.WT_WISE_REJ_PER,a.FINAL_BOX_PROD,A.COMMENTS3,a.paper_cost order by a.fstr";
                    SQuery = "SELECT a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty,round(sum(b.numwt),3) as bwt_spec,round(a.WT_BOX_per_JC,3) as WT_BOX_per_JC,round(a.Corrg_sheet_actual_produce,4) as Corrg_sheet_actual_produce,a.LINEAR_MTR,round(a.Corrg_sheet_actual_produce* sum(b.numwt),3) as paperWt_as_per_spec,round(a.PAPER_WT_REQ_JCRD,3) as PAPER_WT_REQ_JCRD,round((a.PAPER_WT_REQ_JCRD-(a.Corrg_sheet_actual_produce* sum(b.numwt))),3) as paperWt_diff ,round(a.PAPR_CONSME_ACT,3) as PAPR_CONSME_ACT,round(a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD,3) as excess_shot_paper_consumed, round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/ (case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),4) AS NO_OF_SHEET_TO_PRODUCED,round(a.CORR_STG_REJ,4) as CORR_STG_REJ,round(a.CORR_WT_LOSS,3) as CORR_WT_LOSS,round(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS),3) AS DIFF_AS_PER_JBCARD,/* ROUND(((a.PAPR_CONSME_ACT-a.PAPER_WT_REQ_JCRD)+a.CORR_WT_LOSS)/ a.PAPR_CONSME_ACT*100,2) AS CORR_WASTAGE_PER,*/round(a.sorting_packing_rej,4) as sorting_packing_rej,round(a.sorting_packing_loss,3) as sorting_packing_loss, a.job_vALUE,round(a.WT_DIFF,3) AS TOT_RJ_WT,/*round(a.WT_DIFF/a.WT_BOX_per_JC,4) as hypo_box,*/round(a.WT_WISE_REJ_PER,2) as WT_WISE_REJ_PER,/* ROUND(((round(a.PAPR_CONSME_ACT/ a.WT_BOX_per_JC,2)-round(a.FINAL_BOX_PROD,2))/round((case when nvl(a.PAPR_CONSME_ACT,0)=0 then 1 else nvl(a.PAPR_CONSME_ACT,0) end)/(case when nvl(a.WT_BOX_per_JC,0)=0 then 1 else nvl(a.WT_BOX_per_JC,0) end),2))*100,2)  as BOX_WISE_REJ,*/round(a.FINAL_BOX_PROD,4) as FINAL_BOX_PROD,A.COMMENTS3,a.paper_cost FROM  ( SELECT trim(a.enqno)||a.enqdt as fstr,A.ENQNO AS JOBNO,A.ENQDT AS JOBDT,A.ACODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO AS PARTNO,A.TOT_BOX_RCV as plan_qty,round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) AS WT_BOX_per_JC,A.QTYOUT AS Corrg_sheet_actual_produce,(CASE WHEN is_number(C.BTCHDT)>0 THEN ROUND((A.QTYOUT*is_number(C.BTCHDT))/100,2) ELSE 0 END) AS LINEAR_MTR,round(round(a.enr2/a.STD_SHT_RQ,4) * (CASE WHEN A.COL13>0 THEN round(A.QTYOUT/A.COL13,2) ELSE A.COL3 END),3) AS PAPER_WT_REQ_JCRD,A.QTYIN AS PAPR_CONSME_ACT,a.VAL as paper_cost,d.CORR_STG_REJ,d.CORR_STG_REJ*round((a.enr2/a.STD_SHT_RQ)/A.COL13,4) as CORR_WT_LOSS,d.snp as sorting_packing_rej,(d.snp * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4)) as  sorting_packing_loss ,A.IQTYIN AS FINAL_BOX_PROD,A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4) as WT_DIFF,ROUND((A.QTYIN - round(A.IQTYIN * round((a.enr2/a.STD_SHT_RQ)/A.COL13,4),4)) / (CASE WHEN A.QTYIN>0 THEN A.QTYIN ELSE 1 END)  * 100 , 3) AS WT_WISE_REJ_PER,round(((A.QTYOUT - A.IQTYIN) / (case when A.QTYOUT>0 then a.qtyout else 1 end)) * 100,3) AS BOX_WISE_REJ, (A.IQTYIN*A.SALERATE) as job_vALUE,A.COMMENTS3 FROM  (SELECT A.*,B.QTY AS TOT_BOX_RCV,(B.COL14+B.COL15) AS STD_SHT_RQ,b.enr1,b.enr2,B.COL13,c.IRATE AS SALERATE,B.COMMENTS3 FROM  (select enqno,enqdt,acode,sum(qtyin) as qtyin,sum(qtyout) as qtyout,sum(scrp1) as scrp1,sum(scrp2) as scrp2,sum(time1) as time1,sum(time2) as time2,sum(COL3) as col3,SUM(IQTYIN) AS IQTYIN,SUM(VAL) AS VAL from  (select INVNO AS ENQNO,to_char(INVDATE,'dd-MON-yy') as enqdt,TRIM(ICODE) AS ACODE,0 as qtyin,0 as qtyout,0 AS COL3,0 as scrp1,0 as scrp2,0 as time1,0 as time2,SUM(IQTYIN) AS IQTYIN,0 AS VAL from IVOUCHER where BRANCHCD='" + mbr + "' AND type='16' and INVDATE " + xprdrange + " group by INVNO,INVDATE,TRIM(ICODE) union all select A.enqno,to_Char(a.enqdt,'dd-MON-yy') as enqdt,TRIM(A.aCODE) AS ACODE,sum(A.itate) as qtyin,0 as qtyout,0 AS COL3,sum(A.scrp1) as scrp1,sum(A.scrp2) as scrp2,sum(A.time1) as time1,sum(A.time2) as time2,0 AS QTYIN1,SUM((case when b.irate>0 then ROUND(is_number(A.col4)*B.IRATE,2) else ROUND(is_number(A.col4)*c.IRATE,2) end)) AS VAL from item c,costestimate A left outer join REELVCH B on A.BRANCHCD||TRIM(A.ICODe)||TRIM(A.COL6)=B.BRANCHCD||TRIM(B.ICODe)||TRIM(B.KCLREELNO) and b.type in ('02','07') where trim(a.icode)=trim(c.icodE) and A.BRANCHCD='" + mbr + "' AND A.type='25' and A.enqdt " + xprdrange + " group by A.enqno,A.enqdt,TRIM(A.aCODE) union all  select a.enqno,to_Char(a.enqdt,'dd-MON-yy') as enqdt,TRIM(a.iCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join (SELECT VCHNUM,VCHDATE,SUM(is_number(replace(nvl(COL3,'0'),'-','0'))) AS COL3 from inspvch WHERE BRANCHCD='" + mbr + "' and type='45' group by vchnum,vchdate) b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') where a.BRANCHCD='" + mbr + "' and a.type='40' and a.enqdt " + xprdrange + " group by a.enqno,a.enqdt,TRIM(a.iCODE),is_number(replace(nvl(a.COL3,'0'),'-','0'))   ) group by enqno,enqdt,acode) A  , COSTESTIMATE B,SOMAS C WHERE TRIM(A.ENQNO)||A.ENQDT=TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'dd-MON-yy') AND TRIM(SUBSTR(B.CONVDATE,1,20))||TRIM(B.ACODE)||TRIM(B.ICODE)=C.BRANCHCD||C.TYPE||TRIM(C.ORDNO)||TO_CHAR(C.ORDDT,'DD/MM/YYYY')||TRIM(C.ACODE)||TRIM(C.ICODE) and b.BRANCHCD='" + mbr + "' AND B.TYPE='30' AND B.SRNO=0) A,ITEM B,inspmst c, (SELECT SUM(A.MLT_LOSS) AS MLT_LOSS,SUM(A.MLT_LOSS1) AS MLT_LOSS1,SUM(A.MLT_LOSS2) AS MLT_LOSS2,sum(a.CORR_STG_REJ) AS CORR_STG_REJ,sum(a.snp) as snp,A.job_no,to_char(to_date(A.job_Dt,'dd/mm/yyyy'),'dd-MON-yy') as job_dt,A.icode FROM  ( select sum(A.mlt_loss) as mlt_loss,0 AS mlt_loss1,0 AS mlt_loss2 ,0  AS CORR_STG_REJ,0 as snp,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A ,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='09' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss, sum(A.mlt_loss) as mlt_loss1,0 AS mlt_loss2,0  AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS ICODE from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='06' group by A.job_no,A.job_Dt,TRIM(A.ICODE) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,sum(A.mlt_loss) as mlt_loss2,0 AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A,TYPE B where A.BRANCHCD='" + mbr + "' AND TRIM(A.STAGE)=TRIM(B.TYPE1) AND B.ID='K' AND A.type='86' and B.RCNUM='11' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,sum(is_number(A.A4)) AS CORR_STG_REJ,0 as snp ,A.job_no,A.job_Dt,TRIM(A.icode) AS icode from prod_sheet A where a.type='88' group by A.job_no,A.job_Dt,TRIM(A.icode) UNION ALL select 0 AS mlt_loss,0 AS mlt_loss1,0 as mlt_loss2,0 AS CORR_STG_REJ ,sum(is_number(a.col5)) as snp,A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode) AS icode from costestimate A where a.type='60' group by A.enqno,to_char(a.enqdt,'dd/mm/yyyy'),TRIM(A.icode) ) A GROUP BY A.job_no,to_char(to_date(A.job_Dt,'dd/mm/yyyy'),'dd-MON-yy'),A.icode) D WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(A.acode)=trim(c.icode) and trim(a.enqno)||a.enqdt||trim(a.acode)=trim(D.job_no)||D.job_Dt||trim(D.icode) and c.type='70' and c.srno=10 ORDER BY TO_CHAR(A.ENQDT,'YYYYmmdd') desc,a.enqno desc) a, inspmst b  where trim(a.ERPCODE)=trim(b.icode) group by a.fstr,a.JOBNO,a.JOBDT,a.ERPCODE,a.PRODUCT,a.PARTNO,a.plan_qty ,a.WT_BOX_per_JC,a.Corrg_sheet_actual_produce, a.LINEAR_MTR ,a.PAPER_WT_REQ_JCRD,a.PAPR_CONSME_ACT,a.CORR_STG_REJ,a.CORR_WT_LOSS,a.sorting_packing_rej,a.sorting_packing_loss ,a.job_vALUE,a.WT_DIFF,a.WT_WISE_REJ_PER,a.FINAL_BOX_PROD,A.COMMENTS3,a.paper_cost order by a.fstr";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);//new

                    dt2 = new DataTable();
                    //SQuery = "select distinct col15 as ply,icode from inspmst where branchcd='" + mbr + "' and type='70' ";
                    SQuery = "select a.acode,a.grade,a.col16,a.col15 as ply,a.col13,a.btchno,a.maintdt,a.BTCHDT,a.numwt,nvl(a.col1,'-') as col1,nvl(a.col18,'-') as col18,nvl(a.col2,'-') as col2,nvl(a.col3,'-') as col3,nvl(a.col4,'-') as col4,nvl(a.col5,'-') as col5,nvl(a.col10,'-') as col10,nvl(a.col11,'-') as col11,a.rejqty,a.recalib,a.icode,b.iname from inspmst a,item b where trim(a.col5)=trim(b.icode) and substr(col5,1,2) in ('07','08','09') and a.BRANCHCD='" + mbr + "' AND a.type='70' and a.vchnum<>'000000' AND nvl(a.col5,'-')!='-' order by a.srno ";
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);//cosnumed 

                    dt4 = new DataTable();
                    SQuery = "SELECT ENQNO,ENQDT,ACODE,SUM(QTYOUT) AS QTYOUT FROM (select a.enqno,a.enqdt,TRIM(a.aCODE) AS ACODE,0 as qtyin,sum(a.qty + is_number(replace(nvl(b.COL3,'0'),'-','0')) ) as qtyout,is_number(replace(nvl(a.COL3,'0'),'-','0')) as col3,sum(a.scrp1) as scrp1,sum(a.scrp2) as scrp2,sum(a.time1) as time1,sum(a.time2) as time2,0 AS QTYIN1,0 AS VAL from costestimate a left outer join (SELECT VCHNUM,VCHDATE,SUM(is_number(replace(nvl(COL3,'0'),'-','0'))) AS COL3 from inspvch WHERE BRANCHCD='" + mbr + "' and type='45' group by vchnum,vchdate) b on trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') where a.BRANCHCD='" + mbr + "' and a.type='40' group by a.enqno,a.enqdt,TRIM(a.aCODE),is_number(replace(nvl(a.COL3,'0'),'-','0')) ) GROUP BY ENQNO,ENQDT,ACODE";
                    dt4 = fgen.getdata(frm_qstr, co_cd, SQuery);//cosnumed 

                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtDistJC = new DataTable();
                        //dtDistJC

                        DataView distJCView = new DataView(dt, "", "", DataViewRowState.CurrentRows);
                        dtDistJC = distJCView.ToTable(true, "jobno", "jobdt");

                        foreach (DataRow drdistJC in dtDistJC.Rows)
                        {
                            DataView jcview = new DataView(dt, "jobno='" + drdistJC["jobno"].ToString() + "' and jobdt='" + drdistJC["jobdt"].ToString() + "' ", "", DataViewRowState.CurrentRows);
                            int startingIndex = 0, endIndex = 0;
                            int startingIndex1 = 0, endIndex1 = 0;
                            double jobqty = 0; double prodqty = 0; double Paper_Qty_as_per_JC = 0;//new var
                            for (int i = 0; i < jcview.Count; i++)
                            {
                                if (i == 0) startingIndex = dt3.Rows.Count;
                                if (jcview[i].Row["icode"].ToString().Trim().Length > 5)
                                {
                                    dr2 = dt3.NewRow();
                                    dr2["jobno"] = jcview[i].Row["jobno"].ToString().Trim();
                                    dr2["jobdt"] = jcview[i].Row["jobdt"].ToString().Trim();
                                    double totpr1 = 0;
                                    totpr1 = fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + jcview[i].Row["icode"].ToString().Trim() + "' and enqno='" + dr2["jobno"].ToString().Trim() + "' AND enqdt='" + dr2["jobdt"].ToString().Trim() + "'  ", "qtyout"));
                                    if (jcview[i].Row["iname"].ToString().Trim().Length > 5)
                                    {
                                        db1 = 0; db2 = 0; db3 = 0;
                                        if (col1 != dr2["jobno"].ToString())
                                        {
                                            col1 = dr2["jobno"].ToString();
                                            dr2["Product"] = jcview[i].Row["iname"].ToString().Trim();
                                            dr2["Ply"] = fgen.seek_iname_dt(dt2, "icode='" + jcview[i].Row["icode"].ToString().Trim() + "'", "ply");
                                            //dr2["Paper_Qty_Req_as_per_spec"] = jcview[i].Row["iname"].ToString().Trim();
                                            dr2["Jobcard Qty"] = jcview[i].Row["jcqty"].ToString().Trim();
                                            jobqty = fgen.make_double(jcview[i].Row["jcqty"].ToString().Trim());
                                            dr2["Prod_Qty"] = fgen.seek_iname_dt(dt4, "acode='" + jcview[i].Row["icode"].ToString().Trim() + "' and enqno='" + jcview[i].Row["jobno"].ToString().Trim() + "' and enqdt='" + jcview[i].Row["jobdt"].ToString().Trim() + "'", "QTYOUT");//new
                                            prodqty = fgen.make_double(dr2["Prod_Qty"].ToString().Trim());
                                            //=====need to set jobdate also in below
                                            Paper_Qty_as_per_JC = fgen.make_double(fgen.seek_iname_dt(dt1, "erpcode='" + jcview[i].Row["icode"].ToString().Trim() + "' and jobno='" + jcview[i].Row["jobno"].ToString().Trim() + "' and jobdt='" + jcview[i].Row["jobdt"].ToString().Trim() + "'", "WT_BOX_per_JC"));
                                            //Paper_Qty_as_per_JC = fgen.make_double(fgen.seek_iname_dt(dt1, "erpcode='" + jcview[i].Row["icode"].ToString().Trim() + "' and jobno='" + jcview[i].Row["jobno"].ToString().Trim() + "'", "WT_BOX_per_JC"));
                                            dr2["Paper_Qty_as_per_JC"] = Math.Round(prodqty * Paper_Qty_as_per_JC, 3);

                                            DataView vdview = new DataView(dt, "JOBNO='" + dr2["jobno"] + "' AND JOBDT='" + dr2["jobDT"] + "'", "", DataViewRowState.CurrentRows);
                                            for (int x = 0; x < vdview.Count; x++)
                                            {
                                                db1 += fgen.make_double(vdview[x].Row["totqty"].ToString().Trim());
                                                db2 += fgen.make_double(vdview[x].Row["prodqty"].ToString().Trim());
                                                //db2 += fgen.make_double(vdview[x].Row["numwt"].ToString().Trim()) * totpr1;
                                            }

                                            DataView vdview3 = new DataView(dt2, "ICODE='" + jcview[i].Row["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                            for (int y = 0; y < vdview3.Count; y++)
                                            {
                                                db3 += fgen.make_double(vdview3[y].Row["numwt"].ToString().Trim()) * totpr1;
                                            }

                                            dr2["DiffKgs"] = Math.Round(db2 - db3, 2);
                                        }
                                    }
                                    dr2["Paper Required As per Job Card"] = jcview[i].Row["col9"].ToString().Trim();
                                    //==========paper qty as per jc = paper qty as per jc/ (jobcard qty * prod qty)
                                    //  Paper_Qty_as_per_JC = fgen.make_double(jcview[i].Row["totqty"].ToString().Trim());
                                    // dr2["Paper_Qty_as_per_JC"] = Math.Round(Paper_Qty_as_per_JC / jobqty * prodqty, 4);//new
                                    // dr2["Paper_Qty_as_per_JC"] = fgen.make_double(jcview[i].Row["totqty"].ToString().Trim());//old

                                    //dr2["Paper Consumed"] = jcview[i].Row["col2"].ToString().Trim();
                                    //dr2["Qty_Used"] = fgen.make_double(jcview[i].Row["prodqty"].ToString().Trim());

                                    dt3.Rows.Add(dr2);
                                }
                            }


                            if (jcview.Count > 0)
                            {
                                if (startingIndex == -1) startingIndex = 0;
                                endIndex = startingIndex + jcview.Count;
                                DataView vdview2 = new DataView(dt2, "ICODE='" + jcview[0].Row["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                DataView vdview7 = new DataView(dt, "JOBNO='" + jcview[0].Row["jobno"].ToString().Trim() + "' AND PRODQTY>0", "", DataViewRowState.CurrentRows);
                                int myInd = 0, myindd = 0;
                                double totpr = 0;

                                totpr = fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + jcview[0].Row["icode"].ToString().Trim() + "'  and enqno='" + jcview[0].Row["jobno"].ToString().Trim() + "' AND enqdt='" + jcview[0].Row["jobdt"].ToString().Trim() + "' ", "qtyout"));

                                for (int i = startingIndex; i < endIndex; i++)
                                {
                                    if (vdview2.Count > myInd)
                                    {
                                        dt3.Rows[i]["Paper Required As per Spec"] = vdview2[myInd].Row["iname"].ToString().Trim();
                                        dt3.Rows[i]["Paper_Qty_Req_as_per_spec"] = Math.Round(fgen.make_double(vdview2[myInd].Row["numwt"].ToString().Trim()) * totpr, 2);

                                        myInd++;
                                    }
                                }
                                DataView distcountv = new DataView(dt3, "JOBNO='" + jcview[0].Row["jobno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                int rowtoadd = vdview7.Count - distcountv.Count;
                                for (int i = 0; i < rowtoadd; i++)
                                {
                                    dr2 = dt3.NewRow();
                                    dr2["jobno"] = jcview[0].Row["jobno"].ToString().Trim();
                                    dr2["jobdt"] = jcview[0].Row["jobdt"].ToString().Trim();
                                    dt3.Rows.Add(dr2);
                                }
                                myindd = 0;
                                if (vdview7.Count > 0)
                                {
                                    for (int i = startingIndex; i < (startingIndex + vdview7.Count); i++)
                                    {
                                        dt3.Rows[i]["Paper Consumed"] = vdview7[myindd].Row["col2"].ToString().Trim();
                                        dt3.Rows[i]["Qty_Used"] = fgen.make_double(vdview7[myindd].Row["prodqty"].ToString().Trim());
                                        myindd++;
                                    }
                                }
                            }
                        }

                        if (dt3.Rows.Count > 0)
                        {
                            Session["send_dt"] = dt3;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("" + header_n + " From " + fromdt + " To " + todt, frm_qstr);
                        }
                    }
                    else fgen.msg("-", "AMSG", "No Data Found!!");
                    #endregion
                    break;

                //*************************
                #region PPRM Reports

                case "F40175":
                    #region Issue Casting Wt
                    // RATE PER KG
                    dt3 = new DataTable();
                    mq3 = "select TRIM(ICODE) AS ICODE,COL4,ACODE FROM SCRATCH WHERE " + branch_Cd + " AND TYPE='VC'  AND VCHDATE " + xprdrange + "";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    // CASTING WT
                    dt2 = new DataTable();
                    mq2 = "SELECT TRIM(A.INVNO) AS BOM,TRIM(A.ICODE) AS ICODE,A.IQTY_WT FROM IVOUCHER A WHERE A." + branch_Cd + " AND A.TYPE='30' AND A.VCHDATE " + xprdrange + "";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    // OTHER DETAILS
                    dt = new DataTable();
                    mq0 = "SELECT DISTINCT TRIM(A.ORG_INVNO) AS BOM,A.ACODE,F.ANAME,A.DESC1 AS ASSEMBLY,TRIM(A.ICODE) AS ICODE,I.INAME,0 AS CASTING_WT,I.IWEIGHT AS BOM_WT,0 AS WASTAGE,0 AS KG_RATE,0 AS TOTAL  FROM SOMAS  A ,ITEM I, FAMST F  WHERE  TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + mbr + "' AND  A.TYPE LIKE '4%'  AND ORDDT " + xprdrange + " ORDER BY ICODE";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    foreach (DataRow dr in dt.Rows)
                    {
                        db1 = 0; db2 = 0; db3 = 0; db4 = 0;
                        if (dt3.Rows.Count > 0)
                        {
                            db3 = fgen.make_double(fgen.seek_iname_dt(dt3, "ICODE='" + dr["icode"].ToString().Trim() + "'", "COL4"));
                        }
                        if (dt2.Rows.Count > 0)
                        {
                            db2 = fgen.make_double(fgen.seek_iname_dt(dt2, "ICODE='" + dr["icode"].ToString().Trim() + "' AND BOM='" + dr["BOM"].ToString().Trim() + "'", "IQTY_WT"));
                        }
                        db1 = fgen.make_double(dr["BOM_WT"].ToString().Trim());
                        dr["CASTING_WT"] = db2;
                        dr["KG_RATE"] = db3;
                        dr["WASTAGE"] = db1 - db2;
                    }

                    Session["send_dt"] = dt;
                    fgen.Fn_open_rptlevel("Issue Casting Wt VS Bom Casting Wt From " + fromdt + " To" + todt + "", frm_qstr);
                    #endregion
                    break;

                #endregion                   
            }

        }
        Session["mymst"] = null;
    }
}