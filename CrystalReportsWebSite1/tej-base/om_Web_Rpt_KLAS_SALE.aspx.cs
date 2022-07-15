using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


//WEB_SALE

public partial class om_Web_Rpt_KLAS_SALE : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, frm_cDt1, frm_cDt2;
    int i0, i1, i2, i3, i4, v = 0; DateTime date1, date2; DataSet ds, ds3, oDS;
    DataTable dt, ph_tbl, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dtm, dtm1, mdt, mdt1, vdt, dtPo, fmdt, dt_dist, dt_dist1, dticode, dtdrsim, dticode2 = new DataTable();
    DataRow dro, dr1,dr2, dro1 = null;
    double month, to_cons, itot_stk, itv,db10, db9, db8, db7, db6, db5, db4, db3, db2, db1, db; DataRow oporow, ROWICODE, ROWICODE2;
    DataView dv, mvdview, view1im, vdview, vdview1, dist1_view, sort_view, view1, view2;
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
        //HCID = hfhcid.Value.Trim(); 
        HCID = hfid.Value.Trim(); 
        SQuery = ""; fgen.send_cookie("MPRN", "N");
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

                case "RPT1"://GENERAL ITEM LIST                    
                case "RPT2"://FG Item List
                    SQuery = "SELECT 'Y' AS FSTR,'YES' AS CHOICE_,'Do You Want to Select Item Grp' as selection from dual union all  SELECT 'N' AS FSTR,'No' AS CHOICE_,'Do You Want to Select Item Grp' as selection from dual";
                    break;

                case "RPT4":
                    SQuery = "Select DISTINCT COL12 AS fstr,COL12 AS CODE,TRIM(COL25) AS Machine_Name from costestimate where BRANCHCD='" + mbr + "'  and type='40' and vchdate " + xprdrange + " order by code";
                    break;    
                                    
                case "RPT10":
                case "RPT13":
                case "RPT9":
                case "RPT12":
                    SQuery = "select mchname as fstr,mchcode as Machine_code,mchname as Machine_Name from pmaint where branchcd='" + mbr + "' and type='10' order by fstr";
                    header_n = "Select Machine";
                    break;

                case "RPT11":
                case "RPT14":
                    SQuery = "SELECT TYPE1 AS FSTR,TYPE1,NAME   FROM TYPE WHERE ID='1' AND  TYPE1 between '62' and '66' order by type1 ";
                    header_n = "Select Stage Name";
                    break;     

                case "RPT15":
                    SQuery = " SELECT * FROM (SELECT COL6 AS FSTR,COL6 AS JUMBO_ROLL,to_Char(Vchdate,'yyyymmdd') as vdd,VCHNUM FROM COSTESTIMATE WHERE BRANCHCD='" + mbr + "' AND TYPE='40' and vchdate>=sysdate-500 union all SELECT COL6 AS FSTR,COL6 AS JUMBO_ROLL,to_Char(Vchdate,'yyyymmdd') as vdd,VCHNUM FROM COSTESTIMATEK WHERE BRANCHCD='" + mbr + "' AND TYPE='40' and vchdate>=sysdate-500) ORDER BY vdd desc,vchnum desc";
                    header_n = "Select Job Roll";
                    break;

                case"RPT16":
                    m1 = fgen.seek_iname(frm_qstr,co_cd, "select params from controls where id='R10'", "params");
                    xprdrange = "between to_Date('" + m1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')";
                    SQuery = "SELECT a.icode as fstr, A.ICODE,A.TOT,replace(nvl(A.RLOCN,'-'),'-','Not Defined') as RLOCN ,B.INAME FROM( select sum(tot) as tot,max(rlocn) as rlocn ,icode from(select DISTINCT  trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE,'-' as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + "  group by icode,trim(kclreelno),TRIM(Coreelno),trim(acode) union all select  DISTINCT  trim(kclreelno) as Batchno,0 as tot,TRIM(icode) AS ICODE,max(rlocn) as rlocn from reelvch where posted='Y' AND BRANCHCD='" + mbr + "' and  vchdate " + xprdrange + " group by trim(kclreelno),trim(icode)) group by icode ) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT<>0  order by a.icode";
                    SQuery = "SELECT a.icode as fstr, A.ICODE,A.TOT,B.INAME FROM( select sum(tot) as tot,icode from(select trim(kclreelno) as Batchno,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(icode) AS ICODE from reelvch where BRANCHCD='" + mbr + "' and vchdate " + xprdrange + " group by icode,trim(kclreelno),TRIM(Coreelno)) group by icode ) a ,item b  WHERE  trim(a.icode)=trim(b.icode) and a.TOT>0  order by a.icode";
                    break;
                  
                case "RPT17":
                    fgen.msg("-", "CMSG", "Group By Item Code (No for Group By Location Name)");
                    break;

                case "RPT19":
                    SQuery = "select TRIM(TYPE1) AS FSTR,TYPE1 AS GRP_CODE,NAME from TYPE WHERE id='Y' ORDER BY TYPE1";
                    break;

                case"RPT28":
                case "RPT29":
                    SQuery = "SELECT TRIM(A.TYPE1) AS FSTR,A.NAME,A.TYPE1 AS CODE FROM TYPEGRP A WHERE A.ID='A' AND A.TYPE1 LIKE '16%' ORDER BY A.TYPE1";
                    header_n = "Select Schedule";// (Esc for All)
                    break;              

                case "RPT8":
                case "RPT30":
                case "RPT31":
                case "RPT32":
                case"RPT33":
                case "RPT34":
                case "RPT35":
                case "RPT36":
                   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "APopUP", "OpenSingle('om_klas_val.aspx','280px','270px','Pocketdriver Limited');", true);                 
                    break;

                case "RPT37":
                    fgen.msg("-", "CMSG", "Do You want to select group'13'(No for all group)");
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                if (HCID=="RPT28" ||HCID=="RPT29"||HCID=="RPT4")
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
        val = hfid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            value3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
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
                    case "RPT13":
                    case "RPT9":
                    case "RPT12":
                    case "RPT10":
                    case "RPT19":
                        hf2.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "RPT11":
                    case "RPT14":
                        if (hf2.Value == "")
                        {
                            hf2.Value = value1;//stage
                            SQuery = "select mchname as fstr,mchcode as Machine_code,mchname as Machine_Name from pmaint where type='10' AND UPPER(mchname) LIKE '" + value3 + "%' order by fstr";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_sseek("Select Machine Name", frm_qstr);
                        }
                        else
                        {
                            hfcode.Value = value1;//machine
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    #region  General Item List
                    case "RPT1":
                        if (value1 == "Y")
                        {
                            SQuery = "select type1 as fstr,Name as Name,type1 as Type,ADDR1 AS Store_type  from type where  id='Y' and substr(trim(type1),1,1)<'7'order by type1";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Item Main Group", frm_qstr);
                            hfid.Value = "RPT1_ITEM";
                        }
                        if (value1 == "N")
                        {
                            SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICAT AS ITEM_CATEGORY,A.ICODE AS ITEM_CODE,A.INAME AS ITEM_NAME,A.UNIT,A.IWEIGHT AS CONV_FACT,A.MQTY1 AS WIDTH,A.MQTY2 AS STD_PASS,A.NO_PROC AS SEC_UNIT,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CPARTNO AS PART_NAME,A.MAKER AS BRAND_CATG,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) AND B.ID='Y' and substr(trim(type1),1,1)<'7' AND LENGTH(TRIM(A.ICODE))>6 order by ITEM_CODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("-", frm_qstr);
                        }
                        break;

                    case "RPT1_ITEM":
                        mq0 = "and b.type1 in (" + value1 + ")";
                        SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICAT AS ITEM_CATEGORY,A.ICODE AS ITEM_CODE,A.INAME AS ITEM_NAME,A.UNIT,A.IWEIGHT AS CONV_FACT,A.MQTY1 AS WIDTH,A.MQTY2 AS STD_PASS,A.NO_PROC AS SEC_UNIT,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CPARTNO AS PART_NAME,A.MAKER AS BRAND_CATG,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) and length(trim(a.icode))>6 AND B.ID='Y'  " + mq0 + " ORDER BY A.ICODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("-", frm_qstr);
                        break;
                    #endregion
                    #region  FG Item List
                    case "RPT2":
                        if (value1 == "Y")
                        {
                            SQuery = "select type1 as fstr ,Name as Name,type1 as Type,ADDR1 AS Store_type  from type where  id='Y' and substr(trim(type1),1,1)>='7' order by type1";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Item Main Group", frm_qstr);
                            hfid.Value = "RPT2_ITEM";
                        }
                        if (value1 == "N")
                        {
                            SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICODE,A.INAME,A.UNIT,A.IWEIGHT AS GSM,A.WT_NET AS WIDTH,A.WT_RR AS THK,A.PACKSIZE AS ROLL_LEN,A.WT_GROSS AS WT_MTR,A.MQTY2 AS STD_PAS,A.IRATE AS RATE,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CINAME AS FGNAME,A.CPARTNO AS FGCODE,A.CDRGNO AS R_PAPER,A.BINNO AS  PRINT,A.SALLOY  AS EMBOSS,A.NO_PROC AS FABRIC,A.MAKER AS SHADE,A.HSCODE  AS HSCODE,A.MAT10 AS OTHER_REF,A.ALLOY AS TRIAL_LC,A.OPRATE1 AS G1_RATE,A.OPRATE4 AS G1B_RATE,A.OPRATE5 AS NS_RATE,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A ,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) AND B.ID='Y' and length(trim(a.icode))>6  and substr(trim(A.ICODE),1,1)>='7'  ORDER BY A.ICODE";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("-", frm_qstr);
                        }

                        break;

                    case "RPT2_ITEM":
                        mq0 = "and b.type1 in (" + value1 + ")";
                        SQuery = "SELECT B.NAME AS MAIN_GROUP,C.INAME AS SUB_GROUP,A.ICODE,A.INAME,A.UNIT,A.IWEIGHT AS GSM,A.WT_NET AS WIDTH,A.WT_RR AS THK,A.PACKSIZE AS ROLL_LEN,A.WT_GROSS AS WT_MTR,A.MQTY2 AS STD_PAS,A.IRATE AS RATE,A.TARRIFNO||','||A.TARRIFRATE AS TARRIF_NO_RATE,A.CINAME AS FGNAME,A.CPARTNO AS FGCODE,A.CDRGNO AS R_PAPER,A.BINNO AS  PRINT,A.SALLOY  AS EMBOSS,A.NO_PROC AS FABRIC,A.MAKER AS SHADE,A.HSCODE  AS HSCODE,A.MAT10 AS OTHER_REF,A.ALLOY AS TRIAL_LC,A.OPRATE1 AS G1_RATE,A.OPRATE4 AS G1B_RATE,A.OPRATE5 AS NS_RATE,a.bin2 as deact_by,a.nsp_dt as deact_dt FROM ITEM A ,TYPE B ,ITEM C  WHERE SUBSTR(A.ICODE,1,4)=TRIM(C.ICODE) AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(B.TYPE1) AND B.ID='Y' and length(trim(a.icode))>6  " + mq0 + " ORDER BY A.ICODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("-", frm_qstr);
                        break;
                    #endregion

                    case "RPT3":
                        #region Process Parameter line wise
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, "select count(to_char(a.vchdate,'dd/mm/yyyy')) AS CNT,to_char(a.vchdate,'dd/mm/yyyy') as m_th,to_char(a.vchdate,'YYYYMMDD') as ORDDT,a.icode,b.iname from inspvch a,item b where trim(a.icode)=trim(B.icode) and a.TYPE='45' AND a.BRANCHCD||TRIM(a.ICODE)='" + value1.Trim() + "' and nvl(trim(a.col3),'-')!='-' and  a.vchdate " + xprdrange + " group by to_char(a.vchdate,'dd/mm/yyyy'),to_char(a.vchdate,'YYYYMMDD'),a.icode,b.iname order by ORDDT ");
                        if (dt.Rows.Count > 0)
                        {
                            mq0 = dt.Rows[0]["cnt"].ToString().Trim();
                            mq3 = dt.Rows[0]["m_th"].ToString().Trim();
                            mq4 = dt.Rows[0]["m_th"].ToString().Trim();
                            mq5 = dt.Rows[0]["icode"].ToString().Trim();
                            mq6 = dt.Rows[0]["iname"].ToString().Trim();
                        }

                        mq1 = ""; mq10 = ""; mq5 = "";
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (mq1.Length > 0) mq1 = mq1 + ", decode(to_char(a.vchdate,'dd/mm/yyyy'),'" + dt.Rows[i]["m_th"].ToString().Trim() + "',rtrim(xmlagg(xmlelement(e,upper(trim(a.col3))||'')).extract('//text()').extract('//text()'),','),'-') as DT_" + dt.Rows[i]["m_th"].ToString().Trim().Replace("/", "_");
                            else mq1 = "decode(to_char(a.vchdate,'dd/mm/yyyy'),'" + dt.Rows[i]["m_th"].ToString().Trim() + "',rtrim(xmlagg(xmlelement(e,upper(trim(a.col3))||'')).extract('//text()').extract('//text()'),','),'-') as DT_" + dt.Rows[i]["m_th"].ToString().Trim().Replace("/", "_");

                            if (mq10.Length > 0) mq10 = mq10 + ", decode(to_char(a.vchdate,'dd/mm/yyyy'),'" + dt.Rows[i]["m_th"].ToString().Trim() + "',rtrim(xmlagg(xmlelement(e,upper(substr(trim(a.col6),0,9))||'')).extract('//text()').extract('//text()'),','),'-') as DT_" + dt.Rows[i]["m_th"].ToString().Trim().Replace("/", "_");
                            else mq10 = "decode(to_char(a.vchdate,'dd/mm/yyyy'),'" + dt.Rows[i]["m_th"].ToString().Trim() + "',rtrim(xmlagg(xmlelement(e,upper(substr(trim(a.col6),0,9))||'')).extract('//text()').extract('//text()'),','),'-') as DT_" + dt.Rows[i]["m_th"].ToString().Trim().Replace("/", "_");
                        }

                        SQuery = "select * from( select '-' AS LINENAME, 0 as srno, 'Product Code:'||a.icode as specifications,'Product Name: " + mq6 + "' as Standard ," + mq10 + " from (select distinct branchcd,icode,type,vchnum,vchdate,substr(trim(col6),0,9) as col6  from costestimate order by vchnum) A WHERE A.TYPE='40' AND A.BRANCHCD||TRIM(A.ICODE)='" + value1.Trim() + "' and a.vchdate " + xprdrange + " GROUP BY  a.icode,to_char(a.vchdate,'dd/mm/yyyy'),a.vchnum  union all SELECT B.COL25 AS LINENAME,a.srno, trim(A.COL1) as specifications,trim(A.COL2) as Standard," + mq1 + " FROM ( select branchcd,srno,type,icode,vchnum,vchdate,col1,col2,col3 from INSPVCH order by vchnum) A ,(select distinct branchcd,icode,type,vchnum,vchdate,substr(trim(col6),0,9) as col6,TRIM(COL25) AS COL25  from costestimate WHERE  TYPE='40' AND BRANCHCD||TRIM(ICODE)='" + value1.Trim() + "' and vchdate " + xprdrange + "  order by vchnum) B WHERE A.TYPE='45'  AND TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) AND A.BRANCHCD||TRIM(A.ICODE)='" + value1.Trim() + "' and a.vchdate " + xprdrange + " GROUP BY a.srno,trim(A.COL1),trim(A.COL2),to_char(a.vchdate,'dd/mm/yyyy') ,a.vchnum,B.COL25) order by srno ";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, co_cd, SQuery);
                        dtm = new DataTable();
                        dtm = dt3.Clone();
                         view1im = new DataView(dt3);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "SPECIFICATIONS", "STANDARD");

                        foreach (DataRow dr in dtdrsim.Rows)
                        {
                            DataTable dtn = new DataTable();
                            dtn = dt3.Clone();
                            DataRow drm = dtm.NewRow();
                            DataRow[] results = dt3.Select("SPECIFICATIONS = '" + dr["SPECIFICATIONS"] + "' AND STANDARD = '" + dr["STANDARD"] + "'");
                            foreach (DataRow row in results)
                            {
                                dtn.ImportRow(row);
                            }
                            //dtn.Rows.Add(results);
                            drm["SPECIFICATIONS"] = dr["SPECIFICATIONS"];
                            drm["STANDARD"] = dr["STANDARD"];
                            foreach (DataColumn dc in dtn.Columns)
                            {
                                foreach (DataRow dtrn1 in dtn.Rows)
                                {
                                    if (dtrn1[dc.ColumnName].ToString().Trim() != "-")
                                    {
                                        drm[dc.ColumnName] = dtrn1[dc.ColumnName].ToString().Trim();
                                    }
                                }
                            }
                            dtm.Rows.Add(drm);
                            dtn.Dispose();
                        }
                        if (dtm.Rows.Count > 0)
                        {
                            Session["send_dt"] = dtm;
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                            fgen.Fn_open_rptlevel("Parameter History for Product From " + fromdt + " To " + todt, frm_qstr);
                        }
                        #endregion
                        break;
                  
                    case "RPT16":
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

                    
                    case "RPT18":
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT18");
                        fgen.fin_prodrx_reps(frm_qstr);
                        break;

                    case "RPT21":
                         fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT21");
                        fgen.fin_prodrx_reps(frm_qstr);
                        break;

                    case "RPT23":
                         fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT23");
                        fgen.fin_prodrx_reps(frm_qstr);
                        break;

                    case"RPT28":
                    case"RPT29":
                        if (hf2.Value == "")
                        {
                           // hfval.Value = "VPFIN";
                            cond = "";
                            hf2.Value = value1;//for bssch code
                            if (value1.Length > 1) cond = " and a.bssch in (" + value1 + ")";
                            SQuery = "SELECT TRIM(A.ACODE) AS FSTR,A.ANAME AS PARTY,A.ACODE AS CODE FROM FAMST A WHERE A.ACODE LIKE '16%' " + cond + " order by trim(a.acode)";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("Select Party", frm_qstr);               // (Esc for All)             
                        }
                        else
                        {
                            hfcode.Value = value1;//party
                            fgen.Fn_open_prddmp1("-", frm_qstr);
                        }
                        break;

                    case "RPT37":
                        hfcode.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
                        break;

                    case "RPT15":
                         fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", value1);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT15");
                        fgen.fin_prodrx_reps(frm_qstr);
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
                case "RPT17":
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

                case "RPT37":
                    hfval.Value = value1;
                        if (value1 == "Y")
                        {                           
                            SQuery = "Select type1 as fstr,name,type1 as code from typegrp where id='A' and substr(type1,1,2)='16' order by type1";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_mseek("-", frm_qstr);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "APopUP", "OpenSingle('om_klas_val.aspx','280px','270px','Pocketdriver Limited');", true);
                            hfid.Value = "RPT37_1";
                        }
                    
                    break;

                case "RPT37_1":
                    hfcode.Value = Request.Cookies["Value1"].Value.Trim();
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                   
                    break;
            }
        }
        else if (Request.Cookies["Value1"].Value.Length > 0)
        {
            value1 = Request.Cookies["Value1"].Value.ToString().Trim();
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

                case "RPT8":
                case "RPT30":
                case "RPT31":
                case "RPT32":
                case "RPT33":
                case "RPT34":
                case "RPT35":
                case "RPT36":              
                    hf2.Value = value1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
            }
        }
        //when press escape
        else
        {
            // ADD BY MADHVI FOR SHOWING THE DATE RANGE WHEN USER PRESS ESC 
            switch (val)
            {
                case "F50277":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "COL4", value1);//SELECTED PARTY
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F50276":
                    hfcode.Value = value1;
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }
            //fgen.Fn_open_prddmp1("-", frm_qstr);
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

                case "RPT3":
                   // hfval.Value = "VPFIN";
                    SQuery = "Select distinct A.BRANCHCD||TRIM(A.ICODe) AS FSTR,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS PROD_DT /*,a.col6 as jr_no*/ ,to_Char(A.vchdate,'yyyymmdd') as vdd from costestimate a,item b where trim(A.icodE)=trim(b.icode) and a.branchcd='" + mbr + "' and a.type='40' and a.vchdate " + xprdrange + " order by vdd desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Select Product for Process History Report", frm_qstr);
                    break;

                case "RPT4":
                    #region  Process Parameter jumbo roll wise
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "select DISTINCT SRNO, col1 from inspvch  WHERE TYPE='45'  AND  VCHDATE>TO_DATE('01/04/2016','DD/MM/YYYY')  order by srno");
                    dtm = new DataTable();
                    dtm.Columns.Add("Date_", typeof(string));
                    dtm.Columns.Add("Erp_Code", typeof(string));
                    dtm.Columns.Add("Iname", typeof(string));
                    dtm.Columns.Add("Jumbo_roll_No", typeof(string));
                    dtm.Columns.Add("Shift", typeof(string));
                    dtm.Columns.Add("Specification", typeof(string));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm.Columns.Add("(" + dr["srno"].ToString().Trim() + ")" + dr["col1"].ToString().Trim(), typeof(string));
                    }
                    mq10 = hf2.Value;
                    mq0 = "select * from (SELECT TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCHDATE,B.ICODE,B.COL6,a.srno,B.COL23 AS SHIFT,C.INAME, trim(A.COL1) as specifications,trim(A.COL2) as std ,'0' as flg ,TO_CHAR(B.VCHDATE,'YYYYMMDD') AS orddt,'Standard' as name FROM (select branchcd,srno,type,icode,vchnum,vchdate,col1,col2,col3 from INSPVCH WHERE TYPE='45'  AND  VCHDATE>=TO_DATE('01/04/2016','DD/MM/YYYY')  order by srno) A ,(select distinct branchcd,icode,type,vchnum,vchdate,substr(trim(col6),0,9) as col6,TRIM(COL23) AS COL23,TRIM(COL25) AS COL25  from costestimate WHERE " + branch_Cd + " AND TYPE='40' and vchdate " + xprdrange + "  and trim(col12) in (" + mq10 + ") order by vchnum) B ,ITEM C  WHERE B." + branch_Cd + " AND  A.TYPE='45' AND TRIM(B.ICODE)=TRIM(C.ICODE) AND TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) and a.vchdate " + xprdrange + " union all SELECT TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCHDATE,B.ICODE,B.COL6,a.srno,B.COL23 AS SHIFT,C.INAME, trim(A.COL1) as specifications,trim(A.COL3) as std ,'1' as flg,TO_CHAR(B.VCHDATE,'YYYYMMDD') AS orddt,'Actual' as name FROM (select branchcd,srno,type,icode,vchnum,vchdate,col1,col2,col3 from INSPVCH WHERE TYPE='45'  AND  VCHDATE>=TO_DATE('01/04/2016','DD/MM/YYYY')  order by srno) A ,(select distinct branchcd,icode,type,vchnum,vchdate,substr(trim(col6),0,9) as col6,TRIM(COL23) AS COL23,TRIM(COL25) AS COL25  from costestimate WHERE " + branch_Cd + " AND TYPE='40' and vchdate " + xprdrange + " and trim(col12) in (" + mq10 + ") order by vchnum) B ,ITEM C  WHERE B." + branch_Cd + " AND  A.TYPE='45' AND TRIM(B.ICODE)=TRIM(C.ICODE) AND TRIM(A.VCHNUM)=TRIM(B.VCHNUM) AND TRIM(A.VCHDATE)=TRIM(B.VCHDATE) and a.vchdate " + xprdrange + "  ) order by ORDDT,ICODE,COL6,SRNO,FLG";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "vchdate", "col6", "flg");

                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow = dtm.NewRow();
                            DataView viewim = new DataView(dt, "vchdate='" + dr0["vchdate"] + "' and col6='" + dr0["col6"] + "' and flg='" + dr0["flg"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double tot = 0;
                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    String mm = "(" + dt1.Rows[i]["srno"].ToString().Trim() + ")" + dt1.Rows[i]["specifications"].ToString().Trim();
                                    drrow[mm] = dt1.Rows[i]["STD"];
                                }
                                if (dt1.Rows[0]["name"].ToString().Trim() == "Actual")
                                {
                                    drrow["Date_"] = dt1.Rows[0]["vchdate"];
                                    drrow["erp_code"] = dt1.Rows[0]["icode"];
                                    drrow["iname"] = "-";
                                    drrow["shift"] = "-";
                                    drrow["Jumbo_roll_No"] = "-";
                                    drrow["Specification"] = dt1.Rows[0]["name"];
                                }
                                else
                                {
                                    drrow["Date_"] = dt1.Rows[0]["vchdate"];
                                    drrow["erp_code"] = dt1.Rows[0]["icode"];
                                    drrow["iname"] = dt1.Rows[0]["iname"];
                                    drrow["shift"] = dt1.Rows[0]["shift"];
                                    drrow["Jumbo_roll_No"] = dt1.Rows[0]["col6"];
                                    drrow["Specification"] = dt1.Rows[0]["name"];
                                }
                                dtm.Rows.Add(drrow);
                            }
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Parameter History for Product From " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "RPT5":
                    #region  RM Physical verification report
                    cond = "SP";
                    if (co_cd == "ABOX") cond = "RV";
                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    mq1 = "SELECT * FROM (select TRIM(A.icode) AS ICODE,B.INAME AS ITEM_NAME,sum(A.opening)+sum(A.cdr)-sum(A.ccr) as  BOOK_BAL,0 AS PHY_BAL from (Select A.branchcd,TRIM(A.icode) AS ICODE, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,b.iopbal as opval,0 as inval,0 as outval,0 as clval from itembal a,item b  where trim(a.icode)=trim(b.icode) and a." + branch_Cd + "    union all select branchcd,TRIM(icode) AS ICODE,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) -sum(iqtyout*ichgs) as opval,0 as inval,0 as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY TRIM(ICODE) ,branchcd,type union all select branchcd,TRIM(icode) AS ICODE,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos, 0 as opval,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) as inval,sum(iqtyout*ichgs) as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + " and type like '%'   and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type ) A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1)='9' group by A.branchcd,A.icode,B.INAME) WHERE BOOK_BAL<>0";
                    mq2 = "SELECT A.ICODE,NULL AS INAME,0 AS BOOK_BAL,SUM(A.IQTYIN) AS PHY_BAL FROM (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.MAINCODE,A.IQTYIN FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and trim(type)='" + cond + "' ) A GROUP BY A.ICODE";
                    SQuery = "SELECT ICODE,MAX(ITEM_NAME) AS ITEM_NAME,SUM(BOOK_BAL) AS BOOK_BAL,SUM(PHY_BAL) AS PHY_BAL FROM (" + mq1 + " UNION ALL " + mq2 + ") GROUP BY ICODE ";
                    mq4 = "SELECT A.VCHNUM,to_char(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ICODE,maincode as Roll_Code ,count(A.MAINCODE) as cnt FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and type='" + cond + "' group BY VCHNUM,VCHDATE,ICODE,MAINCODE HAVING COUNT(MAINCODE)>1 ORDER BY ICODE,MAINCODE";

                    mq1 = "SELECT * FROM (select TRIM(A.icode) AS ICODE,B.INAME AS ITEM_NAME,sum(A.opening)+sum(A.cdr)-sum(A.ccr) as  BOOK_BAL,0 AS PHY_BAL from (Select A.branchcd,TRIM(A.icode) AS ICODE, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,b.iopbal as opval,0 as inval,0 as outval,0 as clval from itembal a,item b  where trim(a.icode)=trim(b.icode) and a." + branch_Cd + "    union all select branchcd,TRIM(icode) AS ICODE,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) -sum(iqtyout*ichgs) as opval,0 as inval,0 as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY TRIM(ICODE) ,branchcd,type union all select branchcd,TRIM(icode) AS ICODE,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos, 0 as opval,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) as inval,sum(iqtyout*ichgs) as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + " and type like '%'   and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type ) A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1)!='9' group by A.branchcd,A.icode,B.INAME) WHERE BOOK_BAL<>0";
                    mq2 = "SELECT A.ICODE,NULL AS INAME,0 AS BOOK_BAL,SUM(A.IQTYIN) AS PHY_BAL FROM (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.MAINCODE,A.IQTYIN FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and trim(type)='" + cond + "' ) A GROUP BY A.ICODE";
                    SQuery = "SELECT ICODE,MAX(ITEM_NAME) AS ITEM_NAME,SUM(BOOK_BAL) AS BOOK_BAL,SUM(PHY_BAL) AS PHY_BAL FROM (" + mq1 + " UNION ALL " + mq2 + ") WHERE SUBSTR(TRIM(ICODE),1,1)!='9'  GROUP BY ICODE HAVING SUM(PHY_BAL)<>'0' ";
                    mq4 = "SELECT A.VCHNUM,to_char(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ICODE,maincode as Roll_Code ,count(A.MAINCODE) as cnt FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and type='" + cond + "' AND SUBSTR(TRIM(ICODE),1,1)!='9' group BY VCHNUM,VCHDATE,ICODE,MAINCODE HAVING COUNT(MAINCODE)>1 ORDER BY ICODE,MAINCODE";

                    dt = fgen.getdata(frm_qstr, co_cd, mq4);
                    if (dt.Rows.Count > 0)
                    {
                        SQuery = mq4;
                        mq1 = "Warning !! Repeated Records Found Please Correct this First";
                    }
                    else { mq1 = "Physical Verification RM from " + fromdt + " to " + todt + ""; }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;

                case "RPT6":
                    #region FG Physical verification report
                    xprdrange1 = "between to_Date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    mq1 = "SELECT * FROM (select TRIM(A.icode) AS ICODE,B.INAME AS ITEM_NAME,NULL AS CATG,sum(A.opening)+sum(A.cdr)-sum(A.ccr) as  BOOK_BAL,0 AS PHY_BAL from (Select A.branchcd,TRIM(A.icode) AS ICODE, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos,b.iopbal as opval,0 as inval,0 as outval,0 as clval from itembal a,item b  where trim(a.icode)=trim(b.icode) and a." + branch_Cd + "    union all select branchcd,TRIM(icode) AS ICODE,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) -sum(iqtyout*ichgs) as opval,0 as inval,0 as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + "  and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY TRIM(ICODE) ,branchcd,type union all select branchcd,TRIM(icode) AS ICODE,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos, 0 as opval,(case when type='07' then sum(iqtyin*ichgs) else sum(iqtyin*ichgs) end) as inval,sum(iqtyout*ichgs) as outval,0 as clval from (select ichgs,type,store,branchcd,vchnum,vchdate,TRIM(icode) AS ICODE,IQTYIN,IQTYOUT FROM IVOUCHER) where " + branch_Cd + " and type like '%'   and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd,type ) A,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND SUBSTR(TRIM(A.ICODE),1,1)='9' group by A.branchcd,A.icode,B.INAME) WHERE BOOK_BAL<>0";
                    mq2 = "SELECT A.ICODE,MAX(B.INAME) AS INAME,A.WOLINK AS CATG,0 AS BOOK_BAL,SUM(A.IQTYIN) AS PHY_BAL FROM (SELECT A.VCHNUM,A.VCHDATE,A.ICODE,A.MAINCODE,A.IQTYIN,A.WOLINK FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and trim(type)='FP' ) A ,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.ICODE,A.WOLINK";
                    SQuery = "SELECT ICODE,MAX(ITEM_NAME) AS ITEM_NAME,MAX(CATG) AS CATG,SUM(BOOK_BAL) AS BOOK_BAL,SUM(PHY_BAL) AS PHY_BAL FROM (" + mq1 + " UNION ALL " + mq2 + ") GROUP BY ICODE HAVING SUM(PHY_BAL)<>'0'";
                    mq4 = "SELECT A.VCHNUM,to_char(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ICODE,maincode AS Roll_code ,count(A.MAINCODE) as cnt FROM WIPSTK A where " + branch_Cd + " and vchdate " + xprdrange + " and type='FP' group BY VCHNUM,VCHDATE,ICODE,MAINCODE HAVING COUNT(MAINCODE)>1 ORDER BY ICODE,MAINCODE";
                    dt = fgen.getdata(frm_qstr, co_cd, mq4);
                    if (dt.Rows.Count > 0)
                    {
                        SQuery = mq4;
                        mq1 = "Warning !! Repeated Records Found Please Correct this First";
                    }
                    else { mq1 = "Physical Verification FG from " + fromdt + " to " + todt + ""; }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(mq1, frm_qstr);
                    #endregion
                    break;

                case "RPT7":
                    #region  Physical Verification Records
                     SQuery = "SELECT A.BRANCHCD ,A.TYPE,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ICODE,B.INAME,a.MAINCODE,trim(A.WOLINK) AS Category_,IQTYIN AS PHY_BAL FROM WIPSTK A,ITEM B  WHERE A." + branch_Cd + "  AND A.VCHDATE " + xprdrange + "  AND TRIM(A.ICODE)=TRIM(B.ICODE) ORDER BY A.VCHDATE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Physical Verification Records from " + fromdt + " to " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "RPT8":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT8");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT9":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT9");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT10":
                    #region  Time Statement Summary
                    mq10 = "PROD_SHEET";                  
                    string inspvchtab = "";
                    string Mcname = "";
                    mq0 = "select distinct type1 from typewip where id='DTC61' /*AND BRANCHCD='" + mbr + "'*/ ORDER BY TYPE1";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    DataTable dtm1 = new DataTable();
                    dtm1.Columns.Add("Date", typeof(String));
                    dtm1.Columns.Add("PPC Target", typeof(String));
                    dtm1.Columns.Add("Total Production", typeof(String));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("F".Trim() + dr[0].ToString().Trim(), typeof(String));
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(String));
                    }
                    mq0 = hf2.Value;
                    SQuery = "SELECT BRANCHCD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,MACHINE,sum(is_number(PLANQTY)) as PLANQTY,SUM(is_number(QTY)) AS QTY,SUM(is_number(BOXES)) AS BOXES,SUM(is_number(TSLOT)) AS TSLOT,SUM(is_number(dttime)) AS dttime,sum(is_number(ptime)) as ptime,COUNT(is_number(dttime)) as dtcnt,vchdate as orddt from  (select BRANCHCD,VCHDATE,ICODE,SHIFT,MACHINE,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,STIME,ETIME,PLANQTY,QTY,BOXES,TSLOT,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN TRIM(a.SHIFT)='SHIFT B'  AND A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEET C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A )  where  VCHDATE " + xprdrange + ") GROUP BY BRANCHCD,VCHDATE,TO_CHAR(VCHDATE,'DD/MM/YYYY'),ICODE,MACHINE order by orddt";
                    SQuery = "SELECT BRANCHCD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,MACHINE,sum(is_number(PLANQTY)) as PLANQTY,SUM(is_number(QTY)) AS QTY,SUM(is_number(BOXES)) AS BOXES,SUM(is_number(TSLOT)) AS TSLOT,SUM(is_number(dttime)) AS dttime,sum(is_number(ptime)) as ptime,COUNT(is_number(dttime)) as dtcnt,vchdate as orddt from  (select BRANCHCD,VCHDATE,ICODE,SHIFT,MACHINE,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,STIME,ETIME,0 as PLANQTY,QTY,BOXES,TSLOT,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN TRIM(a.SHIFT)='SHIFT B'  AND A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEET C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A )  where  VCHDATE " + xprdrange + " union all SELECT BRANCHCD,VCHDATE,TRIM(ICODE),'-' AS SHIFT ,ENAME,'0' AS NSTIME ,'0' AS STIME,'0' AS ETIME,IS_NUMBER(A1) AS PLAN,0 AS QTY,0 AS BOX,'0' as tslot ,0 AS DTTIME,0 AS PTIME from prod_sheet where branchcd='" + mbr + "' and type='12' and vchnum like'%' and VCHDATE " + xprdrange + " and TRIM(ename)='" + mq0 + "') GROUP BY BRANCHCD,VCHDATE,TO_CHAR(VCHDATE,'DD/MM/YYYY'),ICODE,MACHINE order by orddt";
                    SQuery = "SELECT BRANCHCD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,MACHINE,sum(is_number(PLANQTY)) as PLANQTY,SUM(is_number(QTY)) AS QTY,SUM(is_number(BOXES)) AS BOXES,SUM(is_number(TSLOT)) AS TSLOT,SUM(is_number(dttime)) AS dttime,sum(is_number(ptime)) as ptime,COUNT(is_number(dttime)) as dtcnt,vchdate as orddt from  (select BRANCHCD,VCHDATE,ICODE,SHIFT,MACHINE,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,STIME,ETIME,0 as PLANQTY,QTY,BOXES,TSLOT,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A," + mq10 + " C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdrange + " GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdrange + ") A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdrange + " GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdrange + ") A ) union all SELECT BRANCHCD,VCHDATE,TRIM(ICODE),'-' AS SHIFT ,ENAME,'0' AS NSTIME ,'0' AS STIME,'0' AS ETIME,IS_NUMBER(A1) AS PLAN,0 AS QTY,0 AS BOX,'0' as tslot ,0 AS DTTIME,0 AS PTIME from " + mq10 + " where branchcd='" + mbr + "' and type='12' and vchnum like'%' and VCHDATE " + xprdrange + " and TRIM(ename)='" + mq0 + "') GROUP BY BRANCHCD,VCHDATE,TO_CHAR(VCHDATE,'DD/MM/YYYY'),ICODE,MACHINE order by orddt";

                    if (dt.Rows.Count <= 0) return;
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        view1im = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "BRANCHCD", "VCHDATE", "MACHINE");
                        dtm1.Columns.Add("Total Downtime", typeof(String));
                        dtm1.Columns.Add("Net Production Time", typeof(String));
                        dtm1.Columns.Add("Total ChangeOver Time", typeof(String));
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataRow drrow1 = dtm1.NewRow();
                            DataView viewim = new DataView(dt, "BRANCHCD='" + dr0["BRANCHCD"] + "' and VCHDATE='" + dr0["VCHDATE"] + "'  and MACHINE='" + dr0["MACHINE"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = viewim.ToTable();
                            double totdtime = 0, totchange = 0;
                            int totcnt = 0;
                            double totplan = 0, totprod = 0, totprodtime = 0, speedrate = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                String mm = dt1.Rows[i]["ICODE"].ToString().Trim();
                                try
                                {
                                    if (mm.Trim().Length == 3)
                                    {
                                        drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                        drrow1["F" + mm] = fgen.make_double(dt1.Rows[i]["DTCNT"].ToString());
                                        totdtime = totdtime + fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                        if (mm == "100" || mm == "101" || mm == "107" || mm == "108" || mm == "109" || mm == "113" || mm == "115")
                                        {
                                            totchange = totchange + fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                        }
                                    }
                                    else if (mm.Trim().Length == 8)
                                    {
                                        totprodtime = totprodtime + fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                        totprod = totprod + fgen.make_double(dt1.Rows[i]["QTY"].ToString());
                                        totplan = totplan + fgen.make_double(dt1.Rows[i]["PLANQTY"].ToString());
                                    }
                                }
                                catch { }
                            }
                            drrow1["Date"] = dt1.Rows[0]["VCHDATE"];
                            drrow1["PPC Target"] = totplan;
                            drrow1["Total Production"] = totprod;
                            drrow1["Total Downtime"] = totdtime.ToString();
                            drrow1["Net Production Time"] = totprodtime;
                            drrow1["Total ChangeOver Time"] = totchange;
                            Mcname = dt1.Rows[0]["MACHINE"].ToString();
                            dtm1.Rows.Add(drrow1);
                        }
                        dr2 = dtm1.NewRow();
                        int d = 0;
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

                                dr2[dc] = total;
                            }
                        }
                        dr2["Date"] = '-';
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
                                if (dc.ToString().Substring(0, 1) == "R")
                                {
                                    dtm1.Columns[abc].ColumnName = myname;
                                }
                                else
                                {
                                    dtm1.Columns[abc].ColumnName = name + "_Freq.";
                                }
                            }
                        }
                    }
                    //                    dtm1.Columns.Remove("Speed");
                    if (dtm1.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Downtime Summary between " + fromdt + " and " + todt + " for Machine :" + Mcname, frm_qstr);
                    }                    
                    #endregion
                    break;

                case "RPT11":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT11");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT12":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT12");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT13":
                    #region
                    mq10 = "PROD_SHEET";
                    if (val == "15220E") mq10 = "PROD_SHEETK";
                    inspvchtab = "";
                    Mcname = "";
                    mq0 = "select distinct type1 from typewip where id='DTC61' /*AND BRANCHCD='" + mbr + "'*/ ORDER BY TYPE1";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);
                    dtm1 = new DataTable();
                    dtm1.Columns.Add("Date", typeof(String));
                    dtm1.Columns.Add("PPC Target", typeof(String));
                    dtm1.Columns.Add("Total Production", typeof(String));

                    foreach (DataRow dr in dt.Rows)
                    {
                        dtm1.Columns.Add("F".Trim() + dr[0].ToString().Trim(), typeof(String));
                        dtm1.Columns.Add("R".Trim() + dr[0].ToString().Trim(), typeof(String));
                    }
                    mq0 = hf2.Value;
                    SQuery = "SELECT BRANCHCD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,MACHINE,sum(is_number(PLANQTY)) as PLANQTY,SUM(is_number(QTY)) AS QTY,SUM(is_number(BOXES)) AS BOXES,SUM(is_number(TSLOT)) AS TSLOT,SUM(is_number(dttime)) AS dttime,sum(is_number(ptime)) as ptime,COUNT(is_number(dttime)) as dtcnt,vchdate as orddt from  (select BRANCHCD,VCHDATE,ICODE,SHIFT,MACHINE,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,STIME,ETIME,PLANQTY,QTY,BOXES,TSLOT,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN TRIM(a.SHIFT)='SHIFT B'  AND A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEET C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A )  where  VCHDATE " + xprdrange + ") GROUP BY BRANCHCD,VCHDATE,TO_CHAR(VCHDATE,'DD/MM/YYYY'),ICODE,MACHINE order by orddt";
                    SQuery = "SELECT BRANCHCD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,MACHINE,sum(is_number(PLANQTY)) as PLANQTY,SUM(is_number(QTY)) AS QTY,SUM(is_number(BOXES)) AS BOXES,SUM(is_number(TSLOT)) AS TSLOT,SUM(is_number(dttime)) AS dttime,sum(is_number(ptime)) as ptime,COUNT(is_number(dttime)) as dtcnt,vchdate as orddt from  (select BRANCHCD,VCHDATE,ICODE,SHIFT,MACHINE,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,STIME,ETIME,0 as PLANQTY,QTY,BOXES,TSLOT,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN TRIM(a.SHIFT)='SHIFT B'  AND A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEET C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(TO_CHAR((CASE WHEN A.ETIME BETWEEN '00:00' AND '09:00' THEN A.VCHDATE-1 ELSE A.VCHDATE END),'DD/MM/YYYY') ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "') A )  where  VCHDATE " + xprdrange + " union all SELECT BRANCHCD,VCHDATE,TRIM(ICODE),'-' AS SHIFT ,ENAME,'0' AS NSTIME ,'0' AS STIME,'0' AS ETIME,IS_NUMBER(A1) AS PLAN,0 AS QTY,0 AS BOX,'0' as tslot ,0 AS DTTIME,0 AS PTIME from prod_sheet where branchcd='" + mbr + "' and type='12' and vchnum like'%' and VCHDATE " + xprdrange + " and TRIM(ename)='" + mq0 + "') GROUP BY BRANCHCD,VCHDATE,TO_CHAR(VCHDATE,'DD/MM/YYYY'),ICODE,MACHINE order by orddt";
                    SQuery = "SELECT BRANCHCD,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ICODE,MACHINE,sum(is_number(PLANQTY)) as PLANQTY,SUM(is_number(QTY)) AS QTY,SUM(is_number(BOXES)) AS BOXES,SUM(is_number(TSLOT)) AS TSLOT,SUM(is_number(dttime)) AS dttime,sum(is_number(ptime)) as ptime,COUNT(is_number(dttime)) as dtcnt,vchdate as orddt from  (select BRANCHCD,VCHDATE,ICODE,SHIFT,MACHINE,(CASE WHEN TRIM(SHIFT)='SHIFT B'  AND STIME BETWEEN '00:00' AND '09:00' THEN (IS_NUMBER(SUBSTR(STIME,1,2))+24)||':'||SUBSTR(STIME,4,6) ELSE STIME END) AS NSTIME,STIME,ETIME,0 as PLANQTY,QTY,BOXES,TSLOT,is_number((case when nvl(jroll,'-')='-' then tslot else '0' end)) as dttime ,is_number((case when nvl(jroll,'-')='-' then '0' else tslot end)) as ptime from (SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A," + mq10 + " C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdrange + " GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCH a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdrange + ") A union all SELECT  A.BRANCHCD,A.VCHNUM,TO_DATE(A.SHFT_DT ,'DD/MM/YYYY') AS VCHDATE,A.ICODE,A.iname,A.SHIFT,A.MACHINE,A.STIME,A.ETIME,A.PLANQTY,A.QTY,A.BOXES,A.TSLOT, (CASE WHEN IS_NUMBER(TSLOT)>0 THEN ROUND(A.QTY/IS_NUMBER(TSLOT),2) ELSE 0 END) AS EFF,enqno,enqdt,jroll FROM ( SELECT  C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,A.INAME ,C.PREVCODE AS SHIFT,C.ENAME AS MACHINE,C.MCSTART AS STIME,C.MCSTOP AS ETIME,SUM(IS_NUMBER(C.A7)) AS BOXES ,SUM(IS_NUMBER(C.IQTYIN)) AS QTY ,C.JOB_NO AS ENQNO,C.JOB_DT AS ENQDT,SUM(IS_NUMBER(C.A8)) AS PLANQTY,MAX(C.TSLOT) AS TSLOT,trim(substr(c.remarks2,1,9)) as jroll FROM ITEM A,PROD_SHEETK C WHERE c.branchcd='" + mbr + "' and TRIM(C.ENAME)='" + mq0 + "' and  TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(C.TYPE)='86' and to_date(C.shft_dt,'DD/MM/YYYY') " + xprdrange + " GROUP BY  trim(substr(c.remarks2,1,9)),A.INAME,C.BRANCHCD,C.VCHNUM,C.VCHDATE,C.SHFT_DT,C.ICODE,C.PREVCODE,C.ENAME,C.MCSTART,C.MCSTOP,C.JOB_NO,C.JOB_DT UNION ALL SELECT  a.BRANCHCD,a.VCHNUM,a.VCHDATE,A.SHFT_DT,TRIM(A.COL2) AS ICODE,a.COL1 AS REASON,TRIM(a.OBSV15) AS SHIFT,TRIM(a.TITLE) AS MACHINE,a.COL4 AS STIME,a.COL5  AS ETIME ,0 AS BOX,0 AS QTY,NULL AS ENQNO,NULL AS ENQDT,NULL AS PLANQTY,a.col3 AS TSLOT,null as jroll	 FROM  INSPVCHK a WHERE  a.TYPE='58' and a.branchcd='" + mbr + "' and TRIM(a.TITLE)='" + mq0 + "' and to_date(a.shft_dt,'DD/MM/YYYY') " + xprdrange + ") A ) union all SELECT BRANCHCD,VCHDATE,TRIM(ICODE),'-' AS SHIFT ,ENAME,'0' AS NSTIME ,'0' AS STIME,'0' AS ETIME,IS_NUMBER(A1) AS PLAN,0 AS QTY,0 AS BOX,'0' as tslot ,0 AS DTTIME,0 AS PTIME from " + mq10 + " where branchcd='" + mbr + "' and type='12' and vchnum like'%' and VCHDATE " + xprdrange + " and TRIM(ename)='" + mq0 + "') GROUP BY BRANCHCD,VCHDATE,TO_CHAR(VCHDATE,'DD/MM/YYYY'),ICODE,MACHINE order by orddt";
                    if (dt.Rows.Count <= 0) return;
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                     view1im = new DataView(dt);
                    dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "BRANCHCD", "VCHDATE", "MACHINE");

                    dtm1.Columns.Add("Total Downtime", typeof(String));
                    dtm1.Columns.Add("Net Production Time", typeof(String));
                    dtm1.Columns.Add("Total ChangeOver Time", typeof(String));
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataRow drrow1 = dtm1.NewRow();
                        DataView viewim = new DataView(dt, "BRANCHCD='" + dr0["BRANCHCD"] + "' and VCHDATE='" + dr0["VCHDATE"] + "'  and MACHINE='" + dr0["MACHINE"] + "'", "", DataViewRowState.CurrentRows);
                        dt1 = viewim.ToTable();
                        double totdtime = 0, totchange = 0;
                        int totcnt = 0;
                        double totplan = 0, totprod = 0, totprodtime = 0, speedrate = 0;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            String mm = dt1.Rows[i]["ICODE"].ToString().Trim();
                            try
                            {
                                if (mm.Trim().Length == 3)
                                {
                                    drrow1["R" + mm] = fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                    drrow1["F" + mm] = fgen.make_double(dt1.Rows[i]["DTCNT"].ToString());
                                    totdtime = totdtime + fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                    if (mm == "100" || mm == "101" || mm == "107" || mm == "108" || mm == "109" || mm == "113" || mm == "115")
                                    {
                                        totchange = totchange + fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());

                                    }
                                }
                                else if (mm.Trim().Length == 8)
                                {
                                    totprodtime = totprodtime + fgen.make_double(dt1.Rows[i]["TSLOT"].ToString());
                                    totprod = totprod + fgen.make_double(dt1.Rows[i]["QTY"].ToString());
                                    totplan = totplan + fgen.make_double(dt1.Rows[i]["PLANQTY"].ToString());
                                }
                            }
                            catch { }
                        }
                        drrow1["Date"] = dt1.Rows[0]["VCHDATE"];
                        drrow1["PPC Target"] = totplan;
                        drrow1["Total Production"] = totprod;
                        drrow1["Total Downtime"] = totdtime.ToString();
                        drrow1["Net Production Time"] = totprodtime;
                        drrow1["Total ChangeOver Time"] = totchange;
                        Mcname = dt1.Rows[0]["MACHINE"].ToString();
                        dtm1.Rows.Add(drrow1);
                    }
                    dr2 = dtm1.NewRow();
                 //   d = 0;

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
                            dr2[dc] = total;
                        }
                    }
                    dr2["Date"] = '-';
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
                            if (dc.ToString().Substring(0, 1) == "R")
                            {
                                dtm1.Columns[abc].ColumnName = myname;
                            }
                            else
                            {
                                dtm1.Columns[abc].ColumnName = name + "_Freq.";
                            }
                        }
                    }
                    //                    dtm1.Columns.Remove("Speed");                   
                    if (dtm1.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Downtime Summary between " + fromdt + " and " + todt + " for Machine :" + Mcname, frm_qstr);
                    }
                    #endregion
                    break;

                case "RPT14":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT14");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT15":

                    break;

            
                case "RPT17":

                    break;

                case "RPT18":
                    SQuery = "Select distinct a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,A.TYPE,a.Vchnum as entry_no,to_char(a.Vchdate,'dd/mm/yyyy') as entry_dt,a.BTCHNO as Reel_No,a.ICODE AS iTEM_ICODE ,B.INAME AS Item_Name,a.MRRNUM,a.MRRDATE,A.ACODE ,C.ANAME from MULTIVCH A,ITEM B ,FAMST C where A.branchcd='" + mbr + "' and A.type='FI' and A.vchdate " + xprdrange + "  AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) order by A.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek("-", frm_qstr);
                    break;

                case "RPT19":
                    #region Fabric Planning Sheet
                    dtm = new DataTable();
                    dtm.Columns.Add("ITEM_CODE", typeof(string));
                    dtm.Columns.Add("ITEM_NAME", typeof(string));
                    dtm.Columns.Add("CONVERSION_RATE", typeof(double));
                    dtm.Columns.Add("EXPECTED_CONS", typeof(double));
                    dtm.Columns.Add("ACTUAL_CONS", typeof(double));
                    dtm.Columns.Add("TO_BE_PLANNED", typeof(double));
                    dtm.Columns.Add("IN_STOCK_KG", typeof(double));
                    dtm.Columns.Add("IN_STOCK_MTR", typeof(double));
                    dtm.Columns.Add("BALANCE_IN_KG", typeof(double));
                    dtm.Columns.Add("BALANCE_IN_MTR", typeof(double));
                    dtm.Columns.Add("ORDER_PENDING_KG_WITH_PENDING_RGP", typeof(double));
                    dtm.Columns.Add("ORDER_PENDING_MTR_WITH_PENDING_RGP", typeof(double));
                    dtm.Columns.Add("QA_PENDING", typeof(double));
                    dtm.Columns.Add("REJECTED", typeof(double));
                    dtm.Columns.Add("BALANCE_TO_ORDER_KG", typeof(double));
                    dtm.Columns.Add("BALANCE_TO_ORDER_MTR", typeof(double));
                    dtm.Columns.Add("PENDING_SO", typeof(double));

                    er1 = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    er2 = ""; er3 = "0";

                    //if (Convert.ToInt32(DateTime.Now.ToString("MM")) > 3) txtyear.Value = DateTime.Now.ToString("yyyy");
                    //else txtyear.Value = (Convert.ToInt32(DateTime.Now.ToString("yyyy")) - 1).ToString();

                    // PREVIOUS 3 MONTHS
                    mq8 = "SELECT NXTMNTH,TO_CHAR(TO_DATE(NXTMNTH_YR,'MM/YYYY'),'DD/MM/YYYY') AS DATED FROM(SELECT TO_CHAR(ADD_MONTHS(TO_DATE('" + er1 + "','DD/MM/YYYY'),-3),'MON') AS NXTMNTH,TO_CHAR(ADD_MONTHS(TO_DATE('" + er1 + "','DD/MM/YYYY'),-3),'MON/YYYY') AS NXTMNTH_YR FROM DUAL union all SELECT TO_CHAR(ADD_MONTHS(TO_DATE('" + er1 + "','DD/MM/YYYY'),-2),'MON') AS NXTMNTH,TO_CHAR(ADD_MONTHS(TO_DATE('" + er1 + "','DD/MM/YYYY'),-2),'MON/YYYY') AS NXTMNTH_YR FROM DUAL union all SELECT TO_CHAR(ADD_MONTHS(TO_DATE('" + er1 + "','DD/MM/YYYY'),-1),'MON') AS NXTMNTH,TO_CHAR(ADD_MONTHS(TO_DATE('" + er1 + "','DD/MM/YYYY'),-1),'MON/YYYY') AS NXTMNTH_YR FROM DUAL)";
                    dt8 = new DataTable();
                    dt8 = fgen.getdata(frm_qstr,co_cd, mq8);
                    for (int i = 0; i < dt8.Rows.Count; i++)
                    {
                        er2 = dt8.Rows[0]["DATED"].ToString().Trim();
                        dtm.Columns.Add(dt8.Rows[i]["NXTMNTH"].ToString().Trim(), typeof(double));
                    }
                    dtm.Columns.Add("AVG", typeof(double));

                    // RGP PENDING QTY
                    mq6 = "SELECT ICODE,SUM(BAL) AS BAL FROM (SELECT VCHNUM,VCHDATE,ICODE,ACODE,SUM(IQTYOUT)-SUM(IQTYIN) AS BAL,SUM(IQTYOUT) AS OUT,SUM(IQTYIN) AS INQTY FROM (select vchnum,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,TRIM(icode) AS ICODE,TRIM(acode) AS ACODE,iqtyout,0 as Iqtyin from rgpmst where " + branch_Cd + " and nvl(segment_,0)!=1 and type like '2%' AND VCHDATE >=to_DatE('01/04/2016','dd/mm/yyyy') union all select rgpnum,TO_CHAR(rgpdate,'DD/MM/YYYY') AS VCHDATE,TRIM(icode) AS ICODE,TRIM(acode) AS ACODE,0 as iqtyout,iqtyin+NVL(REJ_RW,'0') from ivoucher where " + branch_Cd + " and type like '0%' AND RGPDATE >=to_DatE('01/04/2016','dd/mm/yyyy') and store <>'R') GROUP BY ICODE,ACODE,VCHNUM,VCHDATE HAVING SUM(IQTYOUT)-SUM(IQTYIN) >0) GROUP BY ICODE";
                    dt6 = new DataTable();
                    dt6 = fgen.getdata(frm_qstr,co_cd, mq6);

                    //PENDING SO
                    string xxprd = " between to_Date('01/01/2016','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";
                    string query1 = "select branchcd,type,ciname,cpartno,pordno,porddt,acode,icode,ordno,orddt,qtyord,0 as sale,upper(packinst) as packinst,cu_chldt from somas where branchcd='" + mbr + "' and type like '4%' and TYPE!='4A' and TYPE!='4C' AND  orddt " + xxprd + " AND TRIM(NVL(ICAT,'-'))<>'Y' AND TRIM(NVL(APP_BY,'-'))<>'-' and trim(desc_)='1' union all select branchcd,type,null as ciname,null as cpartno,null as pordno,null as porddt,acode,icode,ponum,podate,0 as qtyord,iqtyout as sale,null as packinst,null as cu_Chldt from ivoucher where branchcd='" + mbr + "' and type like '4%' and TYPE!='4A' and TYPE!='4C' AND vchdate " + xxprd + " and store='Y' ";
                    query1 = "select sum(a.bal) as bal,a.no_proc from (select b.Aname,trim(c.no_proc) as no_proc,max(nvl(A.ciname,'-')) as cINAME,max(nvl(A.cu_Chldt,a.porddt)) as cu_Chldt,(case when max(trim(nvl(A.packinst,'-')))='-' then 'Other Pending Orders' else max(trim(nvl(A.packinst,'-'))) end ) as packinst,sum(a.qtyord) as qtyord,sum(a.sale) as qty_out,sum(a.qtyord)-sum(a.sale) as bal,c.Unit,max(nvl(a.cpartno,'-')) as Part_no,max(nvl(a.pordno,'-')) as PO_NO,max(a.porddt) as PO_DT,a.ordno,a.orddt,trim(a.acode) as Acode,trim(a.icode) as Icode,a.type,a.branchcd from (" + query1 + ")a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.Icode) group by a.branchcd,a.type,b.aname,c.unit,c.iname,trim(a.acode),trim(a.icode),a.ordno,a.orddt,c.no_proc having sum(a.qtyord)-sum(a.sale)>0 order by A.ORDNO,B.aname)a where (A.PACKINST LIKE '%[MKT]%' OR A.PACKINST='Other Pending Orders'  OR A.PACKINST LIKE '%PLAN CHANGE%' OR A.PACKINST LIKE '%U/P%' OR A.PACKINST LIKE '%PASTE MADE NOT PRODUCED%' OR A.PACKINST LIKE '%[PPC]%' OR A.PACKINST LIKE '%[ACC]%') group by a.no_proc order by no_proc";
                    dt5 = new DataTable();
                    dt5 = fgen.getdata(frm_qstr,co_cd, query1);

                    // REJECTED
                    mq10 = "select params from controls where id='R24'";
                    mq9 = fgen.seek_iname(frm_qstr,co_cd, mq10, "params").Replace("-", "01/04/2017");
                    xprdrange1 = "BETWEEN TO_DATE('" + mq9 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    //  mq4 = " select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a." + branch_Cd + " union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and store='R'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where " + branch_Cd + " and type like '%'  and vchdate " + xprdrange + " and store='R' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";
                    mq4 = "select * From (select a.branchcd,trim(a.icode) as icode,nvl(sum(a.op),0) as opening,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,sum(a.op)+sum(a.cdr)-sum(a.ccr) as cl from (select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and store='R' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where " + branch_Cd + " and type like '%'  and vchdate " + xprdrange + " and store='R' GROUP BY ICODE,branchcd) a where LENGTH(tRIM(a.ICODE))>=8 group by a.branchcd,trim(a.icode) HAVING sum(a.op)+sum(a.cdr)-sum(a.ccr)>0) where abs(opening)+abs(qtyin)+abs(qtyout)<>0 ORDER BY ICODE";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr,co_cd, mq4);

                    // PENDING ORDER KG
                    SQuery = "(select a.branchcd,a.type,a.amdtno,a.ent_by,a.wk3,a.splrmk,a.desc_,a.landcost,a.ordno,a.orddt,a.del_Sch,a.inst,a.iopr,a.pr_no,a.pr_dt,a.acode,a.aname,a.ciname,a.icode,a.iname,a.cpartno,a.unit,a.prate,a.netrate,A.PDISC,a.qtyord,a.pflag,nvl(b.qtyin,0) Qty_In,a.qtyord-nvl(b.qtyin,0) bal, nvl(b.REJ_RW,0) REJ_In,nvl(b.iexc_addl,0) asit_In,A.PEXC,A.PCESS,a.delv_item,a.del_Date,a.st38no from (select p.branchcd,p.type,p.ordno,p.splrmk,p.amdtno,p.ent_by,p.wk3,p.desc_,p.landcost,p.iopr,p.pr_no,p.pr_dt,p.orddt,trim(p.acode) as acode,p.ciname,p.del_sch,substr(p.inst,1,15) as inst, f.aname,trim(p.icode) as icode,i.iname ,i.cpartno,i.unit,((p.prate*(100-p.pdisc)/100))-p.pdiscamt as netrate,p.prate, P.PDISC,p.qtyord,P.PEXC,P.PCESS,p.st38no,p.del_Date,p.delv_item,p.pflag from pomas p , famst f, item i where  p.branchcd!='AM' and substr(p.type,1,1)='5' and p.orddt >=to_DatE('01/04/2010','dd/mm/yyyy')  and p.pflag<>1 and trim(p.icode)=trim(i.icode) and trim(f.acode)=trim(p.acode) ) a ,(select branchcd,podate,ponum,trim(acode) as acode,trim(icode) as icode,sum(iqtyin) qtyin,sum(REJ_RW) AS REJ_RW,sum(iexc_addl) AS iexc_addl from ivoucher where branchcd!='DD' and substr(type,1,1)='0' and vchdate >=to_DatE('01/04/2010','dd/mm/yyyy') group by branchcd,ponum,podate,trim(acode),trim(icode) ) b where a.ordno=b.ponum(+) and a.acode=b.acode(+) and a.icode=b.icode(+) and a.branchcd=b.branchcd(+) and a.orddt=b.podate(+)) order by a.orddt desc,a.ordno desc";
                    fgen.execute_cmd(frm_qstr,co_cd, "create or replace view PENDING_POVIEW_" + mbr + " as(SELECT * FROM (" + SQuery + "))");
                    mq3 = "SELECT SUM(BAL) AS PENDING,TRIM(ICODE) AS ICODE,TRIM(BRANCHCD) AS BRANCHCD  FROM PENDING_POVIEW_" + mbr + " WHERE " + branch_Cd + " AND BAL>0 GROUP BY TRIM(BRANCHCD),TRIM(ICODE) ORDER BY ICODE";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr,co_cd, mq3);

                    // MAIN STOCK
                    dt2 = new DataTable();
                    xprdrange1 = "BETWEEN TO_DATE('" + cDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a." + branch_Cd + " union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where " + branch_Cd + " and type like '%'  and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";
                    dt2 = fgen.getdata(frm_qstr,co_cd, mq2);

                    //ACTUAL CONS LAST 3 MONTHS
                    // CHANGED THE LOGIC ON 11 APR 2018 AFTER FINANCIAL YEAR CHANGE DATA IS NOT SHOWING OF JAN,FEB,MAR
                    xprdrange1 = "BETWEEN TO_DATE('" + er2 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    //if (er2.Length > 1)
                    //{
                    //    er3 = er2.Substring(6, 4);
                    //}
                    //i1 = Convert.ToInt16(er3);
                    //i1 = Convert.ToInt16(year) + 1;
                    if (Convert.ToInt32(System.DateTime.Now.Year) == Convert.ToInt16(year))
                    {
                        i1 = Convert.ToInt16(year);
                    }
                    else { i1 = Convert.ToInt16(year) + 1; }
                    mq9 = "Select a.Item,nvl(b.closing,0) as stk,to_char(sum(a.April)+sum(a.may)+sum(a.june)+sum(a.july)+sum(a.august)+sum(a.sept)+sum(a.oct)+sum(a.nov)+sum(a.dec)+sum(a.jan)+sum(a.feb)+sum(a.mar),'99,99,99,999') as Totals,to_char(sum(a.April),'99,99,99,999.99') as Apr,to_char(sum(a.May),'99,99,99,999.99') as May,to_char(sum(a.June),'99,99,99,999.99') as Jun,to_Char(sum(a.July),'99,99,99,999.99') as Jul,to_char(sum(a.August),'99,99,99,999.99') as Aug,to_Char(sum(a.Sept),'99,99,99,999.99') as Sep,to_char(sum(a.oct),'99,99,99,999.99') as Oct,to_Char(sum(a.Nov),'99,99,99,999.99') as Nov,to_char(sum(a.Dec),'99,99,99,999.99') as Dec,to_Char(sum(a.Jan),'99,99,99,999.99') as Jan,to_char(sum(a.Feb),'99,99,99,999.99') as Feb,to_Char(sum(a.Mar),'99,99,99,999.99') as Mar,a.unit,a.Partno,a.icode,substr(a.icode,1,2) as grpx from (Select trim(b.Iname) as Item,b.unit,trim(b.cpartno) as PArtno,decode(to_chaR(vchdate,'yyyymm')," + year + "04,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as April,decode(to_chaR(vchdate,'yyyymm')," + year + "05,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as May,decode(to_chaR(vchdate,'yyyymm')," + year + "06,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as June,decode(to_chaR(vchdate,'yyyymm')," + year + "07,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as July,decode(to_chaR(vchdate,'yyyymm')," + year + "08,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as August,decode(to_chaR(vchdate,'yyyymm')," + year + "09,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + year + "10,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + year + "11,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + year + "12,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + i1 + "01,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + i1 + "02,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + i1 + "03 ,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Mar,a.icode from ivoucher a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.BRANCHCD='" + mbr + "' and a.vchdate between to_date('" + er2 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and substr(a.type,1,1) IN ('3','1') AND A.TYPE NOT IN ('36','37','15','16','17','18','19') AND A.STORE in('Y','R') and substr(a.icode,1,4) like '%' group by a.icode,trim(b.Iname),b.unit,trim(b.cpartno),to_char(vchdate,'yyyymm'))a left outer join (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as closing from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchdate between to_date('" + er2 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE) b on trim(a.icode)=trim(B.icode) group by b.closing,substr(a.icode,1,2),a.item,a.unit,a.partno,a.icode order by substr(a.icode,1,2),a.item";
                    dt9 = new DataTable();
                    dt9 = fgen.getdata(frm_qstr,co_cd, mq9);

                    //ORIGINAL QUERY AND LOGIC 
                    //xprdrange1 = "BETWEEN TO_DATE('" + cDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    //i1 = Convert.ToInt16(year) + 1;
                    //mq9 = "Select a.Item,nvl(b.closing,0) as stk,to_char(sum(a.April)+sum(a.may)+sum(a.june)+sum(a.july)+sum(a.august)+sum(a.sept)+sum(a.oct)+sum(a.nov)+sum(a.dec)+sum(a.jan)+sum(a.feb)+sum(a.mar),'99,99,99,999') as Totals,to_char(sum(a.April),'99,99,99,999.99') as April,to_char(sum(a.May),'99,99,99,999.99') as May,to_char(sum(a.June),'99,99,99,999.99') as June,to_Char(sum(a.July),'99,99,99,999.99') as July,to_char(sum(a.August),'99,99,99,999.99') as August,to_Char(sum(a.Sept),'99,99,99,999.99') as Sept,to_char(sum(a.oct),'99,99,99,999.99') as Oct,to_Char(sum(a.Nov),'99,99,99,999.99') as Nov,to_char(sum(a.Dec),'99,99,99,999.99') as Dec,to_Char(sum(a.Jan),'99,99,99,999.99') as Jan,to_char(sum(a.Feb),'99,99,99,999.99') as Feb,to_Char(sum(a.Mar),'99,99,99,999.99') as Mar,a.unit,a.Partno,a.icode,substr(a.icode,1,2) as grpx from (Select trim(b.Iname) as Item,b.unit,trim(b.cpartno) as PArtno,decode(to_chaR(vchdate,'yyyymm')," + year + "04,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as April,decode(to_chaR(vchdate,'yyyymm')," + year + "05,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as May,decode(to_chaR(vchdate,'yyyymm')," + year + "06,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as June,decode(to_chaR(vchdate,'yyyymm')," + year + "07,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as July,decode(to_chaR(vchdate,'yyyymm')," + year + "08,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as August,decode(to_chaR(vchdate,'yyyymm')," + year + "09,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + year + "10,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + year + "11,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + year + "12,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + i1 + "01,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + i1 + "02,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + i1 + "03 ,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Mar,a.icode from ivoucher a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.BRANCHCD='" + mbr + "' and a.vchdate between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and substr(a.type,1,1) IN ('3','1') AND A.TYPE NOT IN ('36','37','15','16','17','18','19') AND A.STORE in('Y','R') and substr(a.icode,1,4) like '%' group by a.icode,trim(b.Iname),b.unit,trim(b.cpartno),to_char(vchdate,'yyyymm'))a left outer join (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as closing from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchdate between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE) b on trim(a.icode)=trim(B.icode) group by b.closing,substr(a.icode,1,2),a.item,a.unit,a.partno,a.icode order by substr(a.icode,1,2),a.item";
                    //dt9 = new DataTable();
                    //dt9 = fgen.getdata(co_cd, mq9);

                    //ACTUAL CONS
                    xprdrange1 = "BETWEEN TO_DATE('" + cDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    i1 = Convert.ToInt16(year) + 1;
                    mq7 = "Select a.Item,nvl(b.closing,0) as stk,to_char(sum(a.April)+sum(a.may)+sum(a.june)+sum(a.july)+sum(a.august)+sum(a.sept)+sum(a.oct)+sum(a.nov)+sum(a.dec)+sum(a.jan)+sum(a.feb)+sum(a.mar),'99,99,99,999') as Totals,to_char(sum(a.April),'99,99,99,999.99') as April,to_char(sum(a.May),'99,99,99,999.99') as May,to_char(sum(a.June),'99,99,99,999.99') as June,to_Char(sum(a.July),'99,99,99,999.99') as July,to_char(sum(a.August),'99,99,99,999.99') as August,to_Char(sum(a.Sept),'99,99,99,999.99') as Sept,to_char(sum(a.oct),'99,99,99,999.99') as Oct,to_Char(sum(a.Nov),'99,99,99,999.99') as Nov,to_char(sum(a.Dec),'99,99,99,999.99') as Dec,to_Char(sum(a.Jan),'99,99,99,999.99') as Jan,to_char(sum(a.Feb),'99,99,99,999.99') as Feb,to_Char(sum(a.Mar),'99,99,99,999.99') as Mar,a.unit,a.Partno,a.icode,substr(a.icode,1,2) as grpx from (Select trim(b.Iname) as Item,b.unit,trim(b.cpartno) as PArtno,decode(to_chaR(vchdate,'yyyymm')," + year + "04,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as April,decode(to_chaR(vchdate,'yyyymm')," + year + "05,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as May,decode(to_chaR(vchdate,'yyyymm')," + year + "06,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as June,decode(to_chaR(vchdate,'yyyymm')," + year + "07,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as July,decode(to_chaR(vchdate,'yyyymm')," + year + "08,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as August,decode(to_chaR(vchdate,'yyyymm')," + year + "09,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + year + "10,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + year + "11,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + year + "12,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + i1 + "01,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + i1 + "02,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + i1 + "03 ,sum(a.IQTYOUT-NVL(A.IQTYIN,0)),0) as Mar,a.icode from ivoucher a left outer join item b on TRIM(a.ICODE)=TRIM(B.ICODE) where a.BRANCHCD='" + mbr + "' and a.vchdate " + xprdrange + " and substr(a.type,1,1) IN ('3','1') AND A.TYPE NOT IN ('36','37','15','16','17','18','19') AND A.STORE in('Y','R') and substr(a.icode,1,4) like '%' group by a.icode,trim(b.Iname),b.unit,trim(b.cpartno),to_char(vchdate,'yyyymm'))a left outer join (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as closing from (Select A.branchcd,A.icode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.BRANCHCD='" + mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where BRANCHCD='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where BRANCHCD='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE) b on trim(a.icode)=trim(B.icode) group by b.closing,substr(a.icode,1,2),a.item,a.unit,a.partno,a.icode order by substr(a.icode,1,2),a.item";
                    dt7 = new DataTable();
                    dt7 = fgen.getdata(frm_qstr,co_cd, mq7);

                    // QA PENDING
                    string fromdate = (Convert.ToDateTime(cDT1).AddDays(-30)).ToString("dd/MM/yyyy");
                    mq1 = "select TRIM(BRANCHCD) AS BRANCHCD ,TRIM(ICODE) AS ICODE,sum(iqtyin) as qty from ivoucher where " + branch_Cd + " and type like '0%' and type!='0V' and vchdate between to_date('" + fromdate + "','dd/mm/yyyy') and to_date('" + todt + "','dd/MM/yyyy') and store='N' GROUP BY trim(BRANCHCD),trim(ICODE) ORDER BY ICODE";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr,co_cd, mq1);

                    // 01 SERIES ITEM
                    mq0 = "SELECT DISTINCT TRIM(I.ICODE) AS ICODE,TRIM(I.INAME) AS INAME,I.IWEIGHT,I.LABRCHG,I.UNIT FROM ITEM I WHERE SUBSTR(I.ICODE,0,2) IN (" + hf2.Value + ") AND LENGTH(TRIM(I.ICODE))=8 AND I.DEAC_BY='-' AND NVL(I.LABRCHG,'0')!='0'  ORDER BY ICODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr,co_cd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        view1 = new DataView(dt);
                        dtdrsim = new DataTable();
                        dtdrsim = view1.ToTable(true, "ICODE");
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            dr1 = dtm.NewRow();
                            view2 = new DataView(dt, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dticode = new DataTable();
                            dticode = view2.ToTable();
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; ded1 = ""; ded2 = "";
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                dr1["ITEM_CODE"] = dticode.Rows[i]["ICODE"].ToString().Trim();
                                dr1["ITEM_NAME"] = dticode.Rows[i]["INAME"].ToString().Trim();
                                db1 = fgen.make_double(dticode.Rows[i]["IWEIGHT"].ToString().Trim());
                                dr1["CONVERSION_RATE"] = db1;
                                db2 = fgen.make_double(dticode.Rows[i]["labrchg"].ToString().Trim());
                                dr1["EXPECTED_CONS"] = db2;
                                if (dt7.Rows.Count > 0)
                                {
                                    db3 = fgen.make_double(fgen.seek_iname_dt(dt7, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "TOTALS"));
                                }
                                if (dt9.Rows.Count > 0)
                                {
                                    db7 = fgen.make_double(fgen.seek_iname_dt(dt9, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", dtm.Columns[17].ColumnName));
                                    db8 = fgen.make_double(fgen.seek_iname_dt(dt9, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", dtm.Columns[18].ColumnName));
                                    db9 = fgen.make_double(fgen.seek_iname_dt(dt9, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", dtm.Columns[19].ColumnName));
                                }
                                dr1["ACTUAL_CONS"] = db3;
                                dr1[17] = db7;
                                dr1[18] = db8;
                                dr1[19] = db9;
                                dr1["AVG"] = Math.Round((db7 + db8 + db9) / 3, 2);
                                dr1["TO_BE_PLANNED"] = db2 - db3;
                                if (dt2.Rows.Count > 0)
                                {
                                    db4 = fgen.make_double(fgen.seek_iname_dt(dt2, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "CL"));
                                }
                                if (dticode.Rows[i]["UNIT"].ToString().Trim().Contains("KG"))
                                {
                                    dr1["IN_STOCK_KG"] = db4;
                                    dr1["IN_STOCK_MTR"] = db4 * db1;
                                    //CHANGED ON 13TH SEPT 2017 ON THEIR FINAL REQUEST
                                    // dr1["BALANCE_IN_KG"] = fgen.return_double(dr1["IN_STOCK_KG"].ToString()) - fgen.return_double(dr1["TO_BE_PLANNED"].ToString());
                                    //dr1["BALANCE_IN_MTR"] = fgen.return_double(dr1["IN_STOCK_MTR"].ToString()) - fgen.return_double(dr1["TO_BE_PLANNED"].ToString());
                                    dr1["BALANCE_IN_KG"] = fgen.make_double(dr1["TO_BE_PLANNED"].ToString()) - fgen.make_double(dr1["IN_STOCK_KG"].ToString());
                                    dr1["BALANCE_IN_MTR"] = fgen.make_double(dr1["BALANCE_IN_KG"].ToString()) * db1;
                                }
                                else
                                {
                                    dr1["IN_STOCK_KG"] = 0;
                                    dr1["IN_STOCK_MTR"] = db4;
                                    dr1["BALANCE_IN_KG"] = 0;
                                    //CHANGED ON 13TH SEPT 2017 ON THEIR FINAL REQUEST
                                    //dr1["BALANCE_IN_MTR"] = fgen.return_double(dr1["IN_STOCK_MTR"].ToString()) - fgen.return_double(dr1["TO_BE_PLANNED"].ToString());
                                    dr1["BALANCE_IN_MTR"] = fgen.make_double(dr1["TO_BE_PLANNED"].ToString()) - fgen.make_double(dr1["IN_STOCK_MTR"].ToString());
                                }
                                if (dt3.Rows.Count > 0)
                                {
                                    db5 = fgen.make_double(fgen.seek_iname_dt(dt3, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "PENDING"));
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dr1["QA_PENDING"] = fgen.make_double(fgen.seek_iname_dt(dt1, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "QTY"));
                                }
                                if (dt6.Rows.Count > 0)
                                {
                                    db6 = fgen.make_double(fgen.seek_iname_dt(dt6, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "BAL")); ;
                                }
                                if (dticode.Rows[i]["UNIT"].ToString().Trim().Contains("KG"))
                                {
                                    // COMMENT ON 20TH DEC 2017 AS THEY WANT TO SEE PENDING RGP QTY ALSO WITH ORDER PENDING KG QTY
                                    //dr1["ORDER_PENDING_KG"] = db5;
                                    //dr1["ORDER_PENDING_MTR"] = db5 * db1;


                                    dr1["ORDER_PENDING_KG_WITH_PENDING_RGP"] = db5 + db6;
                                    dr1["ORDER_PENDING_MTR_WITH_PENDING_RGP"] = (db5 + db6) * db1;

                                    //CHANGED ON 13TH SEPT 2017 ON THEIR FINAL REQUEST
                                    //dr1["BALANCE_TO_ORDER_KG"] = fgen.return_double(dr1["BALANCE_IN_KG"].ToString().Trim()) + fgen.return_double(dr1["ORDER_PENDING_KG"].ToString().Trim());
                                    //dr1["BALANCE_TO_ORDER_MTR"] = fgen.return_double(dr1["BALANCE_IN_MTR"].ToString().Trim()) + fgen.return_double(dr1["ORDER_PENDING_MTR"].ToString().Trim());
                                    dr1["BALANCE_TO_ORDER_KG"] = fgen.make_double(dr1["BALANCE_IN_KG"].ToString().Trim()) - fgen.make_double(dr1["ORDER_PENDING_KG_WITH_PENDING_RGP"].ToString().Trim()) - fgen.make_double(dr1["QA_PENDING"].ToString());
                                    dr1["BALANCE_TO_ORDER_MTR"] = fgen.make_double(dr1["BALANCE_TO_ORDER_KG"].ToString().Trim()) * db1;
                                }
                                else
                                {
                                    // COMMENT ON 20TH DEC 2017 AS THEY WANT TO SEE PENDING RGP QTY ALSO WITH ORDER PENDING KG QTY
                                    //dr1["ORDER_PENDING_KG"] = 0;
                                    //dr1["ORDER_PENDING_MTR"] = db5;

                                    dr1["ORDER_PENDING_MTR_WITH_PENDING_RGP"] = db5 + db6;


                                    // dr1["BALANCE_TO_ORDER_KG"] = fgen.return_double(dr1["BALANCE_IN_KG"].ToString().Trim()) + fgen.return_double(dr1["ORDER_PENDING_KG"].ToString().Trim());
                                    dr1["BALANCE_TO_ORDER_KG"] = 0;
                                    //CHANGED ON 13TH SEPT 2017 ON THEIR FINAL REQUEST
                                    // dr1["BALANCE_TO_ORDER_MTR"] = fgen.return_double(dr1["BALANCE_IN_MTR"].ToString().Trim()) + fgen.return_double(dr1["ORDER_PENDING_MTR"].ToString().Trim());
                                    dr1["BALANCE_TO_ORDER_MTR"] = fgen.make_double(dr1["BALANCE_IN_MTR"].ToString().Trim()) - fgen.make_double(dr1["ORDER_PENDING_MTR_WITH_PENDING_RGP"].ToString().Trim()) - fgen.make_double(dr1["QA_PENDING"].ToString());
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    dr1["REJECTED"] = fgen.make_double(fgen.seek_iname_dt(dt4, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "CL"));
                                }
                                if (dt5.Rows.Count > 0)
                                {
                                    ded2 = dticode.Rows[i]["INAME"].ToString().Trim();
                                    dr1["PENDING_SO"] = fgen.make_double(fgen.seek_iname_dt(dt5, "NO_PROC='" + ded2.Trim() + "'", "BAL"));
                                }
                            }
                            dtm.Rows.Add(dr1);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        dtm.Columns[17].ColumnName = dtm.Columns[17].ColumnName + "_CONS";
                        dtm.Columns[18].ColumnName = dtm.Columns[18].ColumnName + "_CONS";
                        dtm.Columns[19].ColumnName = dtm.Columns[19].ColumnName + "_CONS";
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevel("Fabric Plannning Sheet From " + fromdt + " To " + todt + "", frm_qstr);
                    }                
                    #endregion
                    break;                   

                case "RPT20":                       
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT20");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT21":
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as insp_no,to_Char(a.vchdate,'dd/mm/yyyy') as insp_dt from ivoucher a where a.branchcd='" + mbr + "' and a.type='16' and a.vchdate " + xprdrange + " order by a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;

                case "RPT22":
                     fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT22");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT23":
                    xprdrange1 = " and TO_DATE(to_char(a.ent_Dt,'dd/mm/yyyy hh24:mi:ss'),'dd/mm/yyyy hh24:mi:ss') between TO_DATE('" + fromdt + " 08:00:00','dd/mm/yyyy hh24:mi:ss') and TO_DATE('" + todt + " 08:00:00','dd/mm/yyyy hh24:mi:ss')";
                    xprdrange1 = " ";
                    SQuery = "SELECT DISTINCT substr(A.BTCHNO,1,9) AS FSTR,substr(A.BTCHNO,1,9) AS JR_NO,A.ICODE AS PRODUCT_CODE,B.INAME AS PRODUCT FROM IVOUCHER A ,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.branchcd='" + mbr + "' and a.type='16' and a.vchdate " + xprdrange + " order by substr(A.BTCHNO,1,9)";
                  //  hfval.Value = "VPFIN";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek("Select Jr No.", frm_qstr);
                    break;

                case "RPT24":
                        #region
                    SQuery = "";
                    fgen.execute_cmd(frm_qstr,co_cd, "CREATE OR REPLACE FORCE VIEW PENDING_SO_VU (BRANCHCD, TYPE, ORDNO, ORDDT, PORDNO, PORDDT, ACODE, ANAME, ICODE, CINAME, CPARTNO, ST_TYPE, AMDT1, TARRIFNO, TARRIFRATE, NSP_FLAG, THRU, CDISC, IPACK, IRATE, PACKPERCT, QTYORD, QTY_OUT, BAL) AS (select a.branchcd,a.type,a.ordno,a.orddt,a.pordno,a.porddt,a.acode,a.aname,a.icode,a.ciname,a.cpartno,a.st_type,a.amdt1,a.tarrifno,A.TARRIFRATE ,a.nsp_flag,a.thru,a.cdisc,a.ipack,a.irate,a.class as Packperct,a.qtyord,nvl(b.qtyout,0) Qty_out,a.qtyord-nvl(b.qtyout,0) bal from (select s.branchcd,s.type,s.ordno,s.orddt,s.pordno,s.porddt,s.acode, f.aname,s.icode,s.ciname ,s.cpartno,s.thru,s.st_type,s.amdt1,s.cdisc,s.ipack,s.class,s.irate,s.qtyord,i.tarrifno,I.TARRIFRATE,i.nsp_flag from somas s , famst f,item i  where trim(s.icode)=trim(i.icode) and s.branchcd<>'AM' and trim(f.acode)=trim(s.acode) and substr(s.type,1,1)='4')a ,(select type,branchcd,podate,ponum,acode,icode,sum(iqtyout) qtyout from ivoucher group by branchcd,type,ponum,podate,acode,icode) b where a.type=b.type(+) and a.ordno=b.ponum(+) and a.orddt=b.podate(+) and  A.ACODE=b.acode(+) and TRIM(a.ICODE)=b.icode(+) and a.branchcd=b.branchcd(+)) order by orddt ,ordno");
                    if (ulvl == "M") cond = "and trim(a.acode) like '" + uname.Trim() + "%'";
                    if (co_cd == "NEOP")
                    {
                        if (ulvl == "0") SQuery = "select A.Icode as ERP_code,replace(A.Ciname,'&','') as Item_Name,a.type,c.unit,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,B.Aname as Customer,nvl(A.Cpartno,'-') as Part_Number,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,A.IRATE AS RATE,A.Qty_out*A.IRATE AS VALUE,A.Pordno as Cust_ordno,to_char(A.Porddt,'dd/mm/yyyy')as Cust_orddt,to_char(a.orddt,'yyyymmdd') as orddtc from pending_so_vu a,famst b,item c  where  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type like '4%' and a.orddt " + xprdrange + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by a.acode,b.aname,a.Ordno,a.type,c.unit, to_char(a.Orddt,'dd/mm/yyyy'),A.Ciname,A.IRATE,A.ICODE,A.Pordno,A.Cpartno, to_char(A.Porddt,'dd/mm/yyyy'),A.Qty_out*A.IRATE,to_char(a.orddt,'yyyymmdd') ORDER BY a.ordno,orddtc desc ";
                        else
                        {
                            value1 = ""; value2 = "";
                            value1 = fgen.seek_iname(frm_qstr,co_cd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + uname + "'", "icons");
                            if (value1.Length > 1)
                            {
                                string[] word = value1.Split(',');
                                foreach (string vp in word)
                                {
                                    if (value2.Length > 0) value2 = value2 + "," + "'" + vp.ToString().Trim() + "'";
                                    else value2 = "'" + vp.ToString().Trim() + "'";
                                }
                                if (value1 != "0") SQuery = "select A.Icode as ERP_code,replace(A.Ciname,'&','') as Item_Name,a.type,c.unit,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,B.Aname as Customer,nvl(A.Cpartno,'-') as Part_Number,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,A.IRATE AS RATE,A.Qty_out*A.IRATE AS VALUE,A.Pordno as Cust_ordno,to_char(A.Porddt,'dd/mm/yyyy')as Cust_orddt,to_char(a.orddt,'yyyymmdd') as orddtc from pending_so_vu a,famst b,item c  where  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type like '4%' and trim(b.bssch) in (" + value2 + ") and a.orddt " + xprdrange + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by a.acode,b.aname,a.Ordno,a.type,c.unit, to_char(a.Orddt,'dd/mm/yyyy'),A.Ciname,A.IRATE,A.ICODE,A.Pordno,A.Cpartno, to_char(A.Porddt,'dd/mm/yyyy'),A.Qty_out*A.IRATE,to_char(a.orddt,'yyyymmdd') ORDER BY a.ordno,orddtc desc ";
                            }
                        }
                    }
                    else SQuery = "select A.Icode as ERP_code,replace(A.Ciname,'&','') as Item_Name,a.type,c.unit,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_Dt,B.Aname as Customer,nvl(A.Cpartno,'-') as Part_Number,sum(a.qtyord) as Qtyord, sum(a.qty_out) as Qty_out, sum(a.qtyord)-sum(a.qty_out) as bal,A.IRATE AS RATE,A.Qty_out*A.IRATE AS VALUE,A.Pordno as Cust_ordno,to_char(A.Porddt,'dd/mm/yyyy')as Cust_orddt,to_char(a.orddt,'yyyymmdd') as orddtc from pending_so_vu a,famst b,item c  where  trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a." + branch_Cd + " and a.type like '4%' and a.orddt " + xprdrange + " " + cond + " and decode(a.QTYORD,0,length(Trim(a.icode)),a.bal)>0 group by a.acode,b.aname,a.Ordno,a.type,c.unit,to_char(a.Orddt,'dd/mm/yyyy'),A.Ciname,A.IRATE,A.ICODE,A.Pordno,A.Cpartno, to_char(A.Porddt,'dd/mm/yyyy'),A.Qty_out*A.IRATE,to_char(a.orddt,'yyyymmdd') ORDER BY a.ordno,orddtc desc ";
                    if (SQuery.Length > 0)
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("PENDING S.O for the period " + fromdt + " and " + todt + "", frm_qstr);
                    }
                    else fgen.msg("-", "AMSG", "No Data Exist");
#endregion
                    break;

                case "RPT25":
                      mq0 = "select B.aname,C.cpartno as Product,c.cdrgno as Grain,c.maker as Color,a.irate,sum(a.qtyord)as qtyord,sum(a.sold) As sold,sum(a.qtyord)-sum(a.sold) as Balance,d.porddt,max(a.dispdt) as dispdt,d.pordno,a.ordno,a.orddt,trim(a.acode) as acode,trim(a.icode) as icode from (select type,ordno,orddt,orddt as dispdt,acode,icode,irate,qtyord,0 as sold from somas a where " + branch_Cd + " AND TYPE!='4C' AND ORDDT " + xprdrange + " union all select type,ponum,podate,vchdate as dispdt,acode,icode,irate,0 as qtyord,iqtyout as sold from ivoucher a where " + branch_Cd + " and type like '4%' AND PODATE " + xprdrange + ") a, famst b , item c,somas d where d." + branch_Cd + "  AND ";
                    mq0 = mq0 + " d.type||d.ordno||trim(d.acode)||trim(d.icode)||to_char(d.orddt,'dd/mm/yyyy')=a.type||a.ordno||trim(a.acode)||trim(a.icode)||to_char(A.orddt,'dd/mm/yyyy') and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(C.icode) and d." + branch_Cd + " group by b.aname,c.cpartno,c.cdrgno,c.maker,a.type,a.ordno,a.orddt,trim(a.acode),trim(a.icode),a.irate,d.porddt,d.pordno ";
                    mq0 = " select 'NODRILL' as fstr, X.aname as Customer,X.Product,X.Grain,X.Color,X.irate,X.qtyord as Tot_Ord,X.sold as Dispatch,x.Balance,nvl(y.gr_1,0) as G1Stock,to_char(X.porddt,'dd/mm/yyyy') as Po_dt,to_char(X.dispdt,'dd/mm/yyyy') as Last_Disp,X.pordno as PO_NO from (" + mq0 + ") x left outer join (Select * from gr_w_stk where Gr_1>0) y on trim(x.icode)=trim(y.icode) where x.Balance >0";
                    SQuery = mq0;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Confirmed order report for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;

                case "RPT26":
                    mq3 = "((a.invdate+to_number(b.payment))-to_datE('" + fromdt + "','dd/mm/yyyy'))";
                    mq0 = "select n.acode  as fstr, n.aname as Party,to_char(sum(n.total),'99,99,99,999.99') as Total_Outstanding,to_char(sum(n.slab0),'99,99,99,999.99') as OVER_DUE,to_char(sum(n.slab1),'99,99,99,999.99') as Day_1,to_char(sum(n.slab2),'99,99,99,999.99') as Day_2,to_char(sum(n.slab3),'99,99,99,999.99') as Day_3,to_char(sum(n.slab4),'99,99,99,999.99') as Day_4,to_char(sum(n.slab5),'99,99,99,999.99') as Day_5,to_char(sum(n.slab6),'99,99,99,999.99') as After_5,n.acode,n.payment,substr(n.acode,1,2) as grp from (SELECT b.aname,b.payment,a.acode,a.dramt-a.cramt as total,";
                    mq1 = " (CASE WHEN (" + mq3 + " <=0) THEN a.dramt-a.cramt END) as slab0,(CASE WHEN (" + mq3 + " BETWEEN 1 AND 2) THEN a.dramt-a.cramt END) as slab1  ,(CASE WHEN (" + mq3 + " > 2 AND " + mq3 + " <= 3) THEN a.dramt-a.cramt END) as slab2,(CASE WHEN (" + mq3 + " > 3 AND " + mq3 + " <= 4) THEN a.dramt-a.cramt END) as slab3,(CASE WHEN (" + mq3 + " > 4 AND " + mq3 + " <= 5) THEN a.dramt-a.cramt END) as slab4,(CASE WHEN (" + mq3 + " > 5 AND " + mq3 + " <= 6) THEN a.dramt-a.cramt END) as slab5,(CASE WHEN (" + mq3 + " > 6 and " + mq3 + " <= 30 ) THEN a.dramt-a.cramt END) as slab6 from  recdata a ,famst b where trim(a.acode)=trim(b.acode) ) n where substr(n.acode,1,2) in ('16') group by n.aname,n.payment,n.acode having sum(n.total)<> 0  ";
                    mq2 = mq0 + mq1;
                    mq0 = "select n.acode  as Code, n.aname as Party,sum(n.total) as Total_Outstanding,sum(n.slab0) as OVER_DUE,sum(n.slab1) as Day_1,sum(n.slab2) as Day_2,sum(n.slab3) as Day_3,sum(n.slab4) as Day_4,sum(n.slab5) as Day_5,sum(n.slab6) as After_5,n.acode,n.payment,substr(n.acode,1,2) as grp from (SELECT b.aname,b.payment,a.acode,a.dramt-a.cramt as total,";
                    SQuery = mq0 + mq1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Bills for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;

                case "RPT27":
                     SQuery = "select C.Iname as Product,sum(A.Qlty1) as Gr_1,sum(A.Qlty2) as Gr_1B,sum(A.Qlty3) as Gr_ns,sum(A.Qlty1)+sum(A.Qlty2)+sum(A.Qlty3) as Grade_Tot,C.MAker as Color,C.Cpartno,A.Icode FROM (select decode(trim(desc_),'1',sum(bal),0) as Qlty1,decode(trim(desc_),'1B',sum(bal),0) as Qlty2,decode(trim(desc_),'NS',sum(bal),0) as Qlty3,icode from( select trim(icode)as icode,TRIM(DESC_) AS DESC_,trim(invno) as Roll_no,sum(iqtyin)-sum(outq) as bal from (Select icode,invno,iqtyin,0 as outq,TRIM(DESC_) AS DESC_ From ivoucher where " + branch_Cd + " and type='16' and vchdate<=to_DatE('" + fromdt + "','dd/mm/yyyy') union all Select icode,no_bdls,0 as iqtyin,qtysupp,FDUE From despatch where " + branch_Cd + " and substr(type,1,1)='4' and packdate<=to_DatE('" + fromdt + "','dd/mm/yyyy'))GROUP BY trim(icode),TRIM(DESC_),trim(invno))GROUP BY ICODE,trim(desc_)) A, ITEM C WHERE TRIM(a.ICODE)=TRIM(C.ICODe)  GROUP BY C.Iname,C.MAker,C.Cpartno,A.Icode  ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Finished Stock Grade Wise for the period " + fromdt + " and " + todt + "", frm_qstr);
                    break;
               
                case "RPT28":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT28");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT29":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT29");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT30":                    
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT30");
                    fgen.fin_prodrx_reps(frm_qstr);               
                    break;

              

                case "RPT31":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT31");
                    fgen.fin_prodrx_reps(frm_qstr);               
                    break;

                case "RPT32":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT32");
                    fgen.fin_prodrx_reps(frm_qstr);               
                    break;

                case "RPT33":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT33");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT34":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT34");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT35":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT35");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;

                case "RPT36":
                     fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT36");
                    fgen.fin_prodrx_reps(frm_qstr);
                    break;


                case "RPT37":
                case "RPT37_1":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfval.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "RPT37");
                    fgen.fin_prodrx_reps(frm_qstr);               
                    break;

                case "RPT39":
                    SQuery = "select icode as ERPCode,iname as Product,cpartno as partno,unit,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,bin2 as Deactivate_by,nsp_dt as deactivate_Dt from item where length(Trim(nsp_dt))>1 and length(trim(icodE))>4 order by icode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Deactivated Item List", frm_qstr);
                    break;

            }
        }
    }

    protected void rep1_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT1";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT1");
        show_data();
    }
    protected void rep2_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT2";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT1");
        show_data();       
    }
    protected void rep3_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT3";
        hf2.Value = "";
        fgen.Fn_open_prddmp1("-", frm_qstr);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT3");        
    }
    protected void rep4_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT4";
        hf2.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT4");
        show_data();  
    }
    protected void rep5_ServerClick(object sender, EventArgs e)
    {       
        hfid.Value = "RPT5";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT5");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep6_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT6";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT6");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep7_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT7";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT7");
        fgen.Fn_open_prddmp1("-", frm_qstr);      
    }
    protected void rep8_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT8";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT8");
        show_data();
    }
    protected void rep9_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT9";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT1");
        show_data();
    }
    protected void rep10_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "";     
        hfid.Value = "RPT10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT10");
        show_data();
    }
    protected void rep11_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT11";
        hf2.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT11");
        show_data();
    }
    protected void rep12_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT12";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT12");
        show_data();
    }
    protected void rep13_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT13");
        show_data();
    }
    protected void rep14_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT14";
        hf2.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT14");
        show_data();
    }
    protected void rep15_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT15");
        show_data();
    }
    protected void rep16_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT16";
        hf2.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT16");
        show_data();
    }
    protected void rep17_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT17";
        hf2.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT17");
        show_data();
    }
    protected void rep18_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT18");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep19_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT19";
        hf1.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT19");
        show_data();
    }
    protected void rep20_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT20";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT20");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep21_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT21";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT21");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep22_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT22";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT22");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }  
    protected void rep23_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT23";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT23");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep24_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT24";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT24");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep25_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT25";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT25");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep26_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT26";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT26");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep27_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT27";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT27");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep28_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT28";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT28");
        show_data();
    }
    protected void rep29_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT29";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT29");
        show_data();
    }
    protected void rep30_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT30";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT30");
        show_data();
    }
    protected void rep31_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT31";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT31");
        show_data();
    }
    protected void rep32_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT32";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT32");
        show_data();
    }
    protected void rep33_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT33";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT33");
        show_data();
    }
    protected void rep34_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT34";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT34");
        show_data();
    }
    protected void rep35_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT35";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT35");
        show_data();
    }
    protected void rep36_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT36";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT36");
        show_data();
    }
    protected void rep37_ServerClick(object sender, EventArgs e)
    {
        hf2.Value = "";
        hfid.Value = "RPT37";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT37");
        show_data();
    }
    protected void rep38_ServerClick(object sender, EventArgs e)
    {

    }  
    protected void rep39_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "RPT39";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "RPT39");
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void rep40_ServerClick(object sender, EventArgs e)
    {

    }
   
   
}