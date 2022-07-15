using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_view_prodrx : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
    double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
    string party_cd, part_cd, frm_cDt1, frm_cDt2;
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
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
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
                    //==========Klassik Reports
                case "RPT1"://GENERAL ITEM LIST ....need icon for this                   
                case "RPT2"://FG Item List....need icon for this      
                    SQuery = "SELECT 'Y' AS FSTR,'YES' AS CHOICE_,'Do You Want to Select Item Grp' as selection from dual union all  SELECT 'N' AS FSTR,'No' AS CHOICE_,'Do You Want to Select Item Grp' as selection from dual";
                    break;

                case "RPT3":
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R10'", "params");
                    xprdrange = "between to_Date('" + m1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')";
                    SQuery = "SELECT a.icode as fstr, A.ICODE,A.TOT,replace(nvl(A.RLOCN,'-'),'-','Not Defined') as RLOCN ,B.INAME FROM( select sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + "  group by icode,trim(kclreelno),TRIM(Coreelno),trim(acode) union all select  DISTINCT  trim(kclreelno) as Batchno,0 as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + " group by trim(kclreelno),trim(icode)) group by icode ) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT<>0  order by a.icode";
                    SQuery = "SELECT a.icode as fstr, A.ICODE,A.TOT,B.INAME FROM( select sum(tot) as tot,icode from(select trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE from reelvch where BRANCHCD='" + mbr + "' and vchdate " + xprdrange + " group by icode,trim(kclreelno),TRIM(Coreelno)) group by icode ) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT>0  order by a.icode";
                    break;

                case "RPT4":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
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
            if (val == "")
            {
                // bydefault it will ask for prdRange popup
                hfcode.Value = value1;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);//for SELECTED TYPE                                                 
                fgen.Fn_open_prddmp1("-", frm_qstr);

            }
            else if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
            {
                value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                hfcode.Value = "";
                hfcode.Value = value1;
                col1 = value1;
                switch (val)
                {

                    case "F20205":
                        col1 = hfcode.Value;
                        string imgname = fgen.seek_iname(frm_qstr, co_cd, "select vchnum,username,imagepath  from wb_sa_img where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'", "imagepath");
                        //string filePath = "../tej-base/dp/" + imgname + "";
                        string filePath = Server.MapPath("../tej-base/dp/" + imgname + "");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                        break;

                    #region  General Item List
                    case "RPT1":
                        if (hfid.Value == "")
                        {
                            if (value1 == "Y")
                            {
                                SQuery = "select type1 as fstr,Name as Name,type1 as Type,ADDR1 AS Store_type  from type where  id='Y' and substr(trim(type1),1,1)<'7'order by type1";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_mseek("Select Item Main Group", frm_qstr);
                                hfid.Value = value1;
                            }
                            if (value1 == "N")
                            {
                                SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICAT AS ITEM_CATEGORY,A.ICODE AS ITEM_CODE,A.INAME AS ITEM_NAME,A.UNIT,A.IWEIGHT AS CONV_FACT,A.MQTY1 AS WIDTH,A.MQTY2 AS STD_PASS,A.NO_PROC AS SEC_UNIT,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CPARTNO AS PART_NAME,A.MAKER AS BRAND_CATG,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) AND B.ID='Y' and substr(trim(type1),1,1)<'7' AND LENGTH(TRIM(A.ICODE))>6 order by ITEM_CODE";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("General Item List", frm_qstr);
                                hfid.Value = "";
                            }
                        }
                        else
                        {
                            mq0 = "and b.type1 in (" + value1 + ")";
                            SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICAT AS ITEM_CATEGORY,A.ICODE AS ITEM_CODE,A.INAME AS ITEM_NAME,A.UNIT,A.IWEIGHT AS CONV_FACT,A.MQTY1 AS WIDTH,A.MQTY2 AS STD_PASS,A.NO_PROC AS SEC_UNIT,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CPARTNO AS PART_NAME,A.MAKER AS BRAND_CATG,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) and length(trim(a.icode))>6 AND B.ID='Y'  " + mq0 + " ORDER BY A.ICODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("General Item List", frm_qstr);
                        }
                        break;
                    #endregion

                    #region  FG Item List
                    case "RPT2":
                        if (hfid.Value == "")
                        {
                            if (value1 == "Y")
                            {
                                SQuery = "select type1 as fstr ,Name as Name,type1 as Type,ADDR1 AS Store_type  from type where  id='Y' and substr(trim(type1),1,1)>='7' order by type1";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_mseek("Select Item Main Group", frm_qstr);
                                hfid.Value = value1;
                            }
                            if (value1 == "N")
                            {
                                SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICODE,A.INAME,A.UNIT,A.IWEIGHT AS GSM,A.WT_NET AS WIDTH,A.WT_RR AS THK,A.PACKSIZE AS ROLL_LEN,A.WT_GROSS AS WT_MTR,A.MQTY2 AS STD_PAS,A.IRATE AS RATE,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CINAME AS FGNAME,A.CPARTNO AS FGCODE,A.CDRGNO AS R_PAPER,A.BINNO AS  PRINT,A.SALLOY  AS EMBOSS,A.NO_PROC AS FABRIC,A.MAKER AS SHADE,A.HSCODE  AS HSCODE,A.MAT10 AS OTHER_REF,A.ALLOY AS TRIAL_LC,A.OPRATE1 AS G1_RATE,A.OPRATE4 AS G1B_RATE,A.OPRATE5 AS NS_RATE,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A ,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) AND B.ID='Y' and length(trim(a.icode))>6  and substr(trim(A.ICODE),1,1)>='7'  ORDER BY A.ICODE";
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                                fgen.Fn_open_rptlevel("FG Item List", frm_qstr);
                            }
                        }
                        else
                        {
                            mq0 = "and b.type1 in (" + value1 + ")";
                            SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICODE,A.INAME,A.UNIT,A.IWEIGHT AS GSM,A.WT_NET AS WIDTH,A.WT_RR AS THK,A.PACKSIZE AS ROLL_LEN,A.WT_GROSS AS WT_MTR,A.MQTY2 AS STD_PAS,A.IRATE AS RATE,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CINAME AS FGNAME,A.CPARTNO AS FGCODE,A.CDRGNO AS R_PAPER,A.BINNO AS  PRINT,A.SALLOY  AS EMBOSS,A.NO_PROC AS FABRIC,A.MAKER AS SHADE,A.HSCODE  AS HSCODE,A.MAT10 AS OTHER_REF,A.ALLOY AS TRIAL_LC,A.OPRATE1 AS G1_RATE,A.OPRATE4 AS G1B_RATE,A.OPRATE5 AS NS_RATE,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A ,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) AND B.ID='Y' and length(trim(a.icode))>6  " + mq0 + " ORDER BY A.ICODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("FG Item List", frm_qstr);
                        }
                        break;
                    #endregion

                    case "RPT3":
                        #region  ITEM WISE REEL LOCATION
                        m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R10'", "params");
                        xprdrange = "between to_Date('" + m1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')";
                        hfcode.Value = value1;
                        if (hfcode.Value.Length > 2) mq0 = " AND trim(a.icode)='" + hfcode.Value + "'";
                        SQuery = "SELECT A.ICODE  ,A.BATCHNO,A.TOT,replace(nvl(A.RLOCN,'-'),'-','Not Defined') as RLOCN ,B.INAME FROM( select batchno,sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and vchdate " + xprdrange + " group by icode,trim(kclreelno),TRIM(Coreelno),trim(acode) union all select  DISTINCT  trim(kclreelno) as Batchno,0 as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where posted='Y' and branchcd='" + mbr + "' and vchdate " + xprdrange + " group by trim(kclreelno),trim(icode)) group by icode,batchno ) a ,item b WHERE trim(a.icode)=trim(b.icode) and a.TOT>0 " + mq0 + " order by a.batchno,a.rlocn ";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("Reel in Item Code " + hfcode.Value, frm_qstr);
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

                case "RPT4":
                    #region  REEL WISE REEL LOCATION
                    mq0 = value1;
                    m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R10'", "params");
                    xprdrange = "between to_Date('" + m1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')";
                    if (mq0 == "Y")
                    {
                        mq1 = "Reel Number Wise Location Wise Stock";
                        SQuery = "SELECT A.ICODE  ,A.BATCHNO,A.TOT,replace(nvl(A.RLOCN,'-'),'-','Not Defined') as RLOCN ,B.INAME FROM( select batchno,sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + "  group by icode,trim(kclreelno),TRIM(Coreelno),trim(acode) union all select  DISTINCT  trim(kclreelno) as Batchno,0 as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where posted='Y' AND  branchcd='" + mbr + "'    and  vchdate " + xprdrange + " group by trim(kclreelno),trim(icode)) group by icode,batchno ) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT<>0 order by a.batchno,a.rlocn ";
                    }
                    if (mq0 == "N")
                    {
                        mq1 = "Location Wise Reel Number Wise Stock";
                        SQuery = "SELECT a.ICODE  ,A.BATCHNO,a.TOT,replace(nvl(A.RLOCN,'-'),'-','Not Defined') as RLOCN ,trim(B.INAME) as iname FROM( select sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + "  group by icode union all select  DISTINt '0' as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where posted='Y' AND  branchcd='" + mbr + "'    and  vchdate " + xprdrange + " group by trim(icode)) group by icode) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT<>0 order by a.rlocn,a.batchno ";
                    }
                    mq1 = "Location Wise Reel Number Wise Stock";
                    SQuery = "SELECT a.ICODE  ,A.BATCHNO,A.TOT,(CASE WHEN TRIM(A.RLOCN)='-' THEN 'Not Defined' else A.RLOCN END) as RLOCN ,B.INAME FROM( select batchno,sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + "  group by icode,trim(kclreelno),TRIM(Coreelno),trim(acode) union all select  DISTINCT  trim(kclreelno) as Batchno,0 as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where posted='Y' AND  branchcd='" + mbr + "'    and  vchdate " + xprdrange + " group by trim(kclreelno),trim(icode)) group by icode,batchno ) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT<>0 order by a.batchno,a.rlocn ";
                    SQuery = "select a.icode as erpcode,d.iname as product,d.cpartno as part_no,a.kclreelno,a.coreelno,replace(nvl(b.RLOCN,'-'),'-','Not Defined') as RLOCN,a.reelwin as inqty,a.reelwout as outqty,(a.reelwin-a.reelwout) as balance,b.acode,c.aname as vendor,b.vchnum as mrrno,to_char(b.vchdate,'dd/mm/yyyy') as mrrdt from (select branchcd,icode,kclreelno,coreelno,sum(reelwin) as reelwin,sum(reelwout) as reelwout from (select branchcd,trim(icode) as icode,kclreelno,coreelno,reelwin,0 as reelwout from reelvch where branchcd='" + mbr + "' and substr(type,1,1) in ('0','1') and vchdate " + xprdrange + " union all select branchcd,trim(icode) as icode,kclreelno,coreelno,0 as reelwin,reelwout from reelvch where branchcd='" + mbr + "' and substr(type,1,1) in ('3') and vchdate " + xprdrange + " ) group by branchcd,icode,kclreelno,coreelno having sum(reelwin)-sum(reelwout)>0) a,reelvch b,famst c,item d where a.branchcd||a.kclreelno||a.coreelno=b.branchcd||b.kclreelno||b.coreelno and trim(b.acode)=trim(c.acode) and trim(a.icode)=trim(d.icodE) and b.type like '0%' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;
                // if we want to ask another popup's
                // Month Popup Instead of Date Range *************
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

                case "S15115I":
                    // open drill down form
                    fgen.drillQuery(0, "select to_char(vchdate,'yyyymm') as fstr,'-' as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot from sale group by to_char(vchdate,'yyyymm')", frm_qstr);
                    fgen.drillQuery(1, "select trim(Acode) as fstr,to_char(vchdate,'yyyymm') as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode from sale group by to_char(vchdate,'yyyymm'),acode,trim(Acode)", frm_qstr);
                    fgen.drillQuery(2, "select type as fstr,trim(Acode) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,acode", frm_qstr);
                    fgen.drillQuery(3, "select st_type as fstr,trim(type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode", frm_qstr);
                    fgen.drillQuery(4, "select vchdate as fstr,trim(st_type) as gstr,sum(bill_tot) as gros_tot,sum(amt_Sale) as bas_tot,acode,type,st_type,vchdate from sale group by to_char(vchdate,'yyyymm'),trim(Acode),type,st_type,acode,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("Gate Outward Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F38502":
                    SQuery = "SELECT A.VCHNUM AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS PLAN_DT,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,A.COMMENTS AS QRCODE,A.ENT_BY,A.ENT_DT,TO_cHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM EXTRUSION A,ITEM B WHERE TRIM(A.ICODe)=TRIM(b.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='1C' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC,A.VCHNUM  ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production Printing Store", frm_qstr);
                    break;
                case "F38503":
                    SQuery = "SELECT A.VCHNUM AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS PLAN_DT,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,A.COMMENTS AS QRCODE,A.ENT_BY,A.ENT_DT,TO_cHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM EXTRUSION A,ITEM B WHERE TRIM(A.ICODe)=TRIM(b.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE=2C' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC,A.VCHNUM  ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production Pigment Store", frm_qstr);
                    break;
                case "F38504":
                    SQuery = "SELECT A.VCHNUM AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS PLAN_DT,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,A.COMMENTS AS QRCODE,A.ENT_BY,A.ENT_DT,TO_cHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM EXTRUSION A,ITEM B WHERE TRIM(A.ICODe)=TRIM(b.ICODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='3C' AND A.VCHDATE " + xprdrange + " ORDER BY VDD DESC,A.VCHNUM  ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Production Main Mixing Store", frm_qstr);
                    break;
            }
        }
    }
}
