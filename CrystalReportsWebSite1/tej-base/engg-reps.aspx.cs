using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;

using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.Collections.Generic;

public partial class engg_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdrange1, xprd1, xprd2, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, branch_Cd, header_n = "";
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, cond = " ", pdfView = "", data_found = "";
    DataTable ph_tbl;
    DataRow dr, dro, dro1, dr2;
    DataView vdview = new DataView();
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
            No_Data_Found.Visible = false;
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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");

            }
            if (!Page.IsPostBack)
            {
                printCrpt(hfhcid.Value);
                if (data_found == "N")
                {
                    No_Data_Found.Visible = true;
                    divReportViewer.Visible = false;
                }
                else
                {
                    divReportViewer.Visible = true;
                    CrystalReportViewer1.RefreshReport();
                    CrystalReportViewer1.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            fgen.FILL_ERR(ex.Message);
        }
    }

    void printCrpt(string iconID)
    {
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        string mq10, mq1, mq0;
        int repCount = 1;
        data_found = "Y";
        string opt = "";
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        switch (iconID)
        {
            //GE
            case "F1001":
                #region GE
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.*,B.INAME,B.CPARTNO,B.UNIT FROM IVOUCHERP A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(a.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "' ORDER BY A.MORDER");
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }
                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select aname,addr1,addr2,addr3,staten,email,website,gst_no from famst where trim(acode)='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'");
                    dt.TableName = "FAMST";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_ge", frm_rptName, dsRep, "Gate Entry Report");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            //BOM Layered
            case "F10131L":
            case "FB3055L":
                #region BOM
                dsRep = new DataSet();

                //********************                
                DataTable mdt = new DataTable(); dt3 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); DataTable mdt1 = new DataTable();
                SQuery = "Select A.BRANCHCD,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,A.ICODE" +
                    ",A.IBCODE,b.MAKER,A.naration,A.LINKAGE,A.IBQTY," +
                    "(case when B.IQD>0 then B.IQD else B.irate end) AS itrate,b.iname as ibname" +
                    ",b.cpartno as bcpartno,b.unit as bunit,substr(a.ibcat,2,6) as ibcat,a.main_issue_no,a.sub_issue_no" +
                    ",a.st_type,a.ibwt,c.iname as iname,c.cpartno as cpartno,c.unit,a.ent_by,a.ent_dt from itemosp a,item b" +
                    ",item c where trim(a.ibcode)=trim(b.icode) and trim(A.icodE)=trim(c.icode) AND a.BRANCHCD='" + frm_mbr + "' " +
                    "and a.type='BM' and a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "' order by a.srno,a.icode";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                DataTable vdt = new DataTable();
                mdt.Columns.Add(new DataColumn("branchcd", typeof(string)));
                mdt.Columns.Add(new DataColumn("type", typeof(string)));

                mdt.Columns.Add(new DataColumn("srno", typeof(double)));
                mdt.Columns.Add(new DataColumn("vchnum", typeof(string)));
                mdt.Columns.Add(new DataColumn("vchdate", typeof(string)));

                mdt.Columns.Add(new DataColumn("bvchnum", typeof(string)));
                mdt.Columns.Add(new DataColumn("bvchdate", typeof(string)));

                mdt.Columns.Add(new DataColumn("lvl", typeof(double)));
                mdt.Columns.Add(new DataColumn("icode", typeof(string)));
                mdt.Columns.Add(new DataColumn("MAKER", typeof(string)));
                mdt.Columns.Add(new DataColumn("pcode", typeof(string)));
                mdt.Columns.Add(new DataColumn("mqty", typeof(double)));
                mdt.Columns.Add(new DataColumn("ibqty", typeof(double)));
                mdt.Columns.Add(new DataColumn("ibcode", typeof(string)));
                mdt.Columns.Add(new DataColumn("linkage", typeof(string)));
                mdt.Columns.Add(new DataColumn("naration", typeof(string)));
                mdt.Columns.Add(new DataColumn("irate", typeof(double)));
                mdt.Columns.Add(new DataColumn("val", typeof(double)));
                mdt.Columns.Add(new DataColumn("ibcat", typeof(string)));

                mdt.Columns.Add(new DataColumn("iname", typeof(string)));
                mdt.Columns.Add(new DataColumn("sname", typeof(string)));
                mdt.Columns.Add(new DataColumn("cpartno", typeof(string)));
                mdt.Columns.Add(new DataColumn("unit", typeof(string)));

                mdt.Columns.Add(new DataColumn("ibname", typeof(string)));
                mdt.Columns.Add(new DataColumn("bcpartno", typeof(string)));
                mdt.Columns.Add(new DataColumn("bunit", typeof(string)));

                mdt.Columns.Add(new DataColumn("ent_by", typeof(string)));
                mdt.Columns.Add(new DataColumn("ent_dt", typeof(DateTime)));

                mdt.Columns.Add(new DataColumn("star", typeof(string)));

                DataTable fmdt = new DataTable();
                fmdt.Columns.Add(new DataColumn("icode", typeof(string)));
                fmdt.Columns.Add(new DataColumn("val", typeof(string)));

                //SQuery = "Select a.*,(case when B.IQD>0 then B.IQD else B.irate end) as bchrate from itemosp a,item b where trim(a.ibcode)=trim(b.icode) AND a.BRANCHCD='" + frm_mbr + "' order by a.srno,a.icode,a.ibcode";
                //vdt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                int v = 0;
                int srno = 1;
                dt2 = new DataTable();
                //SQuery = "Select branchcd,trim(icode) as icode,(case when ichgs>0 then ichgs else irate end) as rate,to_Char(vchdate,'yyyymmdd') as vdd,vchdate from ivoucher where branchcd='" + mbr + "' and type like '0%' and trim(nvl(finvno,'-'))!='-' and vchdate>=(sysdate-500)  /*and icode like '9%'*/ order by icode,vdd desc";                
                DataView dist1_view = new DataView(dt3);
                DataTable dt_dist = new DataTable();
                if (dist1_view.Count > 0)
                {
                    dist1_view.Sort = "icode";
                    dt_dist = dist1_view.ToTable(true, "icode");
                }
                foreach (DataRow dt_dist_row in dt_dist.Rows)
                {
                    mdt1 = new DataTable();
                    mdt1 = mdt.Clone();
                    DataView mvdview = new DataView(dt3, "icode='" + dt_dist_row["icode"].ToString().Trim() + "'", "icode,ibcode", DataViewRowState.CurrentRows);
                    dt = new DataTable();
                    mvdview.Sort = "srno,icode";
                    dt = mvdview.ToTable();
                    int lvl = 1;
                    List<string> Myprents = new List<string>();
                    Myprents.Add(dt_dist_row["icode"].ToString().Trim());
                    // filling parent
                    foreach (DataRow drc in dt.Rows)
                    {
                        dro = mdt1.NewRow();
                        dro["lvl"] = lvl;
                        dro["branchcd"] = drc["branchcd"].ToString().Trim();

                        dro["srno"] = srno;
                        dro["vchnum"] = drc["vchnum"].ToString().Trim();
                        dro["vchdate"] = drc["vchdate"].ToString().Trim();

                        dro["bvchnum"] = "**********";
                        dro["bvchdate"] = "**********";

                        dro["icode"] = drc["icode"].ToString().Trim();
                        dro["pcode"] = drc["icode"].ToString().Trim();
                        dro["ibqty"] = drc["ibqty"];
                        dro["ibcode"] = drc["ibcode"].ToString().Trim();
                        dro["linkage"] = drc["linkage"].ToString().Trim();
                        dro["naration"] = drc["naration"].ToString().Trim();
                        dro["irate"] = drc["itrate"].ToString().Trim();
                        dro["ibcat"] = drc["ibcat"].ToString().Trim();
                        dro["MAKER"] = drc["MAKER"].ToString().Trim();

                        dro["iname"] = drc["iname"].ToString().Trim();
                        dro["cpartno"] = drc["cpartno"].ToString().Trim();
                        dro["unit"] = drc["unit"].ToString().Trim();

                        dro["ibname"] = drc["ibname"].ToString().Trim();
                        dro["bcpartno"] = drc["bcpartno"].ToString().Trim();
                        dro["bunit"] = drc["bunit"].ToString().Trim();

                        dro["ent_by"] = drc["ent_by"].ToString().Trim();
                        dro["ent_dt"] = drc["ent_dt"].ToString().Trim();

                        dro["sname"] = drc["iname"].ToString().Trim();
                        dro["mqty"] = drc["main_issue_no"];

                        dro["val"] = "0";
                        mdt1.Rows.Add(dro);
                        string icode = drc["ibcode"].ToString().Trim();
                        if (icode.Substring(0, 1) == "7" || icode.Substring(0, 1) == "8"
                     || icode.Substring(0, 1) == "9")
                        {

                            make_bom_print(mdt1, icode, lvl + 1, drc["ibqty"].ToString(),Myprents);
                        }
                      
                    }
                 
                                       

              


                  

                    //DataView sort_view = new DataView();
                    //sort_view = mdt1.DefaultView;
                    //sort_view.Sort = "lvl,srno,pcode,icode";
                    //mdt1 = new DataTable();
                    //mdt1 = sort_view.ToTable(true);
                    //sort_view.Dispose();

                    //// seeking LC and update value
                    //for (int i = 0; i < mdt1.Rows.Count; i++)
                    //{
                    //    vdview = new DataView(mdt1, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and icode='" + mdt1.Rows[i]["ibcode"] + "'", "icode", DataViewRowState.CurrentRows);
                    //    if (vdview.Count <= 0)
                    //    {
                    //        if (dt2.Rows.Count > 0)
                    //        {
                    //            sort_view = new DataView(dt2, "branchcd='" + mdt1.Rows[i]["branchcd"].ToString().Trim() + "' and trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                    //            if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                    //            else
                    //            {
                    //                sort_view = new DataView(dt2, "trim(icode)='" + mdt1.Rows[i]["ibcode"].ToString().Trim() + "'", "vdd desc", DataViewRowState.CurrentRows);
                    //                if (sort_view.Count > 0) mdt1.Rows[i]["irate"] = sort_view[0].Row["rate"].ToString().Trim();
                    //            }
                    //        }
                    //    }
                    //    else mdt1.Rows[i]["irate"] = "0";
                    //    vdview.Dispose();
                    //    mdt1.Rows[i]["val"] = Convert.ToDouble(Convert.ToDouble(mdt1.Rows[i]["ibqty"]) * Convert.ToDouble(mdt1.Rows[i]["irate"]));
                    //}

                    //mq0 = "0";
                    //// making final value
                    //vdview = new DataView(mdt1, "pcode='" + dt_dist_row["icode"].ToString().Trim() + "'", "pcode", DataViewRowState.CurrentRows);
                    //for (int i = 0; i < vdview.Count; i++)
                    //{
                    //    if (Convert.ToDouble(mq0) > 0) mq0 = Math.Round(Convert.ToDouble(mq0) + Convert.ToDouble(vdview[i].Row["val"].ToString().Trim()), 2).ToString();
                    //    else mq0 = vdview[i].Row["val"].ToString().Trim();
                    //}
                    //vdview.Dispose();

                    //for (int f = 0; f < mdt1.Rows.Count; f++)
                    //{
                    //    mdt.ImportRow(mdt1.Rows[f]);
                    //}

                    ////has child
                    //if (mdt.Rows.Count > 0)
                    //{
                    //    dist1_view = new DataView(mdt1, "", "", DataViewRowState.CurrentRows);
                    //    dt_dist = new DataTable();
                    //    dt_dist = dist1_view.ToTable(true, "icode");

                    //    foreach (DataRow dr in dt_dist.Rows)
                    //    {
                    //        for (int f = 0; f < mdt.Rows.Count; f++)
                    //        {
                    //            if (mdt.Rows[f]["ibcode"].ToString().Trim() == dr["icode"].ToString().Trim())
                    //            {
                    //                mdt.Rows[f]["star"] = "*";
                    //            }
                    //        }
                    //    }
                    //}


                    //mdt1.Dispose();
                    //// mdt is table which is having Bom in Expended Form
                    //dro1 = fmdt.NewRow();
                    //dro1["icode"] = dt_dist_row["icode"].ToString().Trim();
                    //dro1["val"] = mq0;
                    //fmdt.Rows.Add(dro1);
                    //// fmdt is table which is only having Parant Bom icode and Value                        
                }

                //********************
                if (mdt1.Rows.Count > 0)
                {
                    mdt1.TableName = "Prepcur";
                    dsRep.Tables.Add(mdt1);
                }
               
                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    if (frm_cocd == "MASS" || frm_cocd == "MAST")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "bom_entry_LMASS", "bom_entry_LMASS", dsRep, "BOM Entry Report");
                    }
                    else
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "bom_entry_L", "bom_entry_L", dsRep, "BOM Entry Report");

                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "FB3055":
            case "F10131":
                break;
            case "F10188":
                SQuery = "select a.*,b.aname as aname,C.Iname from scratch2 a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icodE)=trim(c.icode) and a.type='LC' and A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode.Trim() + "'";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    if (dsRep.Tables[0].Rows.Count > 0)
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "lcostsheet", "lcostsheet", dsRep, "Costing Sheet");
                    }
                }
                break;
            case "F10055":
                dsRep = new DataSet();
                dt = new DataTable();
                frm_rptName = "cnitc";
                if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI")
                {
                    frm_rptName = "csydb";
                    SQuery = "Select a.*,(case when trim(nvl(b.INAME,'-'))='-' then a.t121 else b.INAME end) as INAME from (select a.*,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (Select * from somas_anx a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode.Trim() + "') a left outer join famst b on trim(a.acode)=trim(b.acode)) a left outer join item b on trim(a.icode)=trim(b.icode) ";
                }
                else SQuery = "select a.*,b.aname,c.iname from somas_anx a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icodE)=trim(c.icode) and a.type='PN' and A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode.Trim() + "'";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                }
                if (dsRep.Tables[0].Rows.Count > 0)
                {
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cnitc", frm_rptName, dsRep, "Costing Sheet");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10135":
                pdfView = "Y";
                header_n = "Process Plan";
                SQuery = "SELECT B.INAME AS ITEMNAME,B.CDRGNO AS CUST_IT_CODE,C.ANAME AS CUSTOEMR,a.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,A.TITLE as Remarks,A.ACODE,A.ICODE,A.CPARTNO,A.SRNO,A.BTCHNO AS SR,COL1 AS PROCESS,A.COL2 AS SPECIFICATION,A.COL3 AS Reqmt,A.COL4 as RMK, A.COL5 AS ERPCODE,A.COL6 AS UOM,A.COL9 AS COBB_IN,A.COL10 AS FLUTE,A.COL11 AS HEIGHT,A.COL12 AS DIENO,A.COL13 AS TYPE_OF_ITEM,A.COL14 AS CTN_SIZE_OD,A.COL15 as PLy,A.COL16 AS CTN_SIZE_ID,A.COL17,A.COL18 AS Std_Rej_Allow,A.REJQTY  AS UPS,A.REMARK2,REMARK3,REMARK4,A.ENT_BY,TO_cHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.APP_BY,A.APP_DT,A.EDT_BY,TO_CHAR(A.EDT_DT,'DD/MM/YYYY') AS EDT_DT,A.AMDCOMMENT AS AMEN1,A.AMDDT AS AMDT1,A.AMDCOMMENT2 AS AMEN2 ,A.AMDDT2,A.AMDCOMMENT3 AS AMEN3,A.AMDDT3,A.AMDCOMMENT4 AS AMEN4,A.AMDDT4,A.AMDCOMMENT5 AS AMEN5,A.AMDDT5,A.AMDNO,nvl(b.IMAGEF,'-') as IMAGEF FROM  INSPMST  A,ITEM B ,FAMST C WHERE A.BRANCHCD='" + frm_mbr + "' AND A .TYPE='70' AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("planImg", typeof(System.Byte[]));
                    FileStream FilStr;
                    BinaryReader BinRed;
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            fpath = dr["imagef"].ToString().Trim();
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dr["planImg"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                        catch { }
                    }

                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Process_Plan", "Process_Plan", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10133":
                header_n = "Item Stage Mapping";
                SQuery = "SELECT DISTINCT C.NAME AS STAGES,B.VCHNUM,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCHDATE,B.STAGEC,B.ICODE,D.INAME,B.MTIME1,B.SRNO,A.MCHCODE,A.MCHNAME AS SATGE_NAME FROM ITWSTAGE B LEFT OUTER JOIN PMAINT A ON TRIM(A.ACODE)||'/'||TRIM(A.SRNO)=TRIM(B.OPCODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='10', TYPE C,ITEM D WHERE  B.BRANCHCD='" + frm_mbr + "' AND B.TYPE='10'  AND TRIM(B.STAGEC)=TRIM(C.TYPE1) AND C.ID='K' AND TRIM(B.ICODE)=TRIM(D.ICODE) AND  TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')='" + barCode + "' ORDER BY B.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_StageMapping", "std_StageMapping", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10144":
            case "F10149":
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                header_n = "Box Costing";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (iconID == "F10144")
                {
                    SQuery = "SELECT '" + header_n + "' as header, a.code,a.aname,a.iname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.trannum,a.lt,a.wd,a.ht,a.ply,a.flute,a.cs,a.caliper,a.z,a.rqect,a.rqbs,a.rqgsm,a.deckle,a.length,a.area,a.minect,a.maxect,a.avgect,a.mincs,a.maxcs,a.avgcs,a.mingsm,a.maxgsm,a.avggsm,a.minbs,a.maxbs,a.avgbs,a.minwt,a.maxwt,a.avgwt,a.contribution,a.contamt,a.tconcst as conver_cost,a.cstpkg as cost_kg,a.papcst as papercost,a.pawastage as paper_wastg,a.pawastageamt as pap_wstg_amt,a.boxcost,a.h_16,a.n_16,a.h_18,a.n_18,a.h_20,a.n_20,a.h_22,a.n_22,a.h_24,a.n_24,a.h_28,a.n_28,a.h_35,a.n_35,a.h_45,a.n_45   FROM wb_corrcst_TRANS a WHERE a.branchcd='" + frm_mbr + "' and trim(a.branchcd)||trim(a.TRANNUM)='" + barCode + "'  and vchdate " + xprdRange + " ";
                }
                else
                {
                    SQuery = "SELECT '" + header_n + "' as header, a.code,a.aname,a.iname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.trannum,a.lt,a.wd,a.ht,a.ply,a.flute,a.cs,a.caliper,a.z,a.rqect,a.rqbs,a.rqgsm,a.deckle,a.length,a.area,a.minect,a.maxect,a.avgect,a.mincs,a.maxcs,a.avgcs,a.mingsm,a.maxgsm,a.avggsm,a.minbs,a.maxbs,a.avgbs,a.minwt,a.maxwt,a.avgwt,a.contribution,a.contamt,a.tconcst as conver_cost,a.cstpkg as cost_kg,a.papcst as papercost,a.pawastage as paper_wastg,a.pawastageamt as pap_wstg_amt,a.boxcost,a.h_16,a.n_16,a.h_18,a.n_18,a.h_20,a.n_20,a.h_22,a.n_22,a.h_24,a.n_24,a.h_28,a.n_28,a.h_35,a.n_35,a.h_45,a.n_45   FROM wb_corrcst_TRANS a WHERE a.branchcd='" + frm_mbr + "' and trim(a.branchcd)||trim(a.TRANNUM)='" + mq0 + "'  and vchdate " + xprdRange + "";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ////dt1 for layers in rpt
                #region colm for left side  in rpt
                dtm = new DataTable();
                dtm.Columns.Add("gsm_0", typeof(string));
                dtm.Columns.Add("gsm_1", typeof(string));
                dtm.Columns.Add("gsm_2", typeof(string));
                dtm.Columns.Add("gsm_3", typeof(string));
                dtm.Columns.Add("gsm_4", typeof(string));

                dtm.Columns.Add("bf_0", typeof(string));
                dtm.Columns.Add("bf_1", typeof(string));
                dtm.Columns.Add("bf_2", typeof(string));
                dtm.Columns.Add("bf_3", typeof(string));
                dtm.Columns.Add("bf_4", typeof(string));

                dtm.Columns.Add("rctgrade_0", typeof(string));
                dtm.Columns.Add("rctgrade_1", typeof(string));
                dtm.Columns.Add("rctgrade_2", typeof(string));
                dtm.Columns.Add("rctgrade_3", typeof(string));
                dtm.Columns.Add("rctgrade_4", typeof(string));

                dtm.Columns.Add("rct_0", typeof(string));
                dtm.Columns.Add("rct_1", typeof(string));
                dtm.Columns.Add("rct_2", typeof(string));
                dtm.Columns.Add("rct_3", typeof(string));
                dtm.Columns.Add("rct_4", typeof(string));

                dtm.Columns.Add("t_rct_0", typeof(string));
                dtm.Columns.Add("t_rct_1", typeof(string));
                dtm.Columns.Add("t_rct_2", typeof(string));
                dtm.Columns.Add("t_rct_3", typeof(string));
                dtm.Columns.Add("t_rct_4", typeof(string));


                dtm.Columns.Add("cost_0", typeof(string));
                dtm.Columns.Add("cost_1", typeof(string));
                dtm.Columns.Add("cost_2", typeof(string));
                dtm.Columns.Add("cost_3", typeof(string));
                dtm.Columns.Add("cost_4", typeof(string));

                dtm.Columns.Add("tot_t_Rct", typeof(string));
                dtm.Columns.Add("tot_cost", typeof(string));
                #endregion

                #region for right side in rpt
                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("strch_rt", typeof(string)); //00
                ph_tbl.Columns.Add("strch_flg", typeof(string));
                ph_tbl.Columns.Add("strch_amt", typeof(string));
                ph_tbl.Columns.Add("pva_rt", typeof(string));//01
                ph_tbl.Columns.Add("pva_flg", typeof(string));
                ph_tbl.Columns.Add("pva_amt", typeof(string));
                ph_tbl.Columns.Add("power_rt", typeof(string));//02
                ph_tbl.Columns.Add("power_flg", typeof(string));
                ph_tbl.Columns.Add("power_amt", typeof(string));
                ph_tbl.Columns.Add("fuel_rt", typeof(string));//03
                ph_tbl.Columns.Add("fuel_flg", typeof(string));
                ph_tbl.Columns.Add("fuel_amt", typeof(string));
                ph_tbl.Columns.Add("pins_rt", typeof(string));//04
                ph_tbl.Columns.Add("pins_flg", typeof(string));
                ph_tbl.Columns.Add("pins_amt", typeof(string));
                ph_tbl.Columns.Add("ink_rt", typeof(string));//05
                ph_tbl.Columns.Add("ink_flg", typeof(string));
                ph_tbl.Columns.Add("ink_amt", typeof(string));
                ph_tbl.Columns.Add("labr_rt", typeof(string));//06
                ph_tbl.Columns.Add("labr_flg", typeof(string));
                ph_tbl.Columns.Add("labr_amt", typeof(string));
                ph_tbl.Columns.Add("admin_rt", typeof(string));//07
                ph_tbl.Columns.Add("admin_flg", typeof(string));
                ph_tbl.Columns.Add("admin_amt", typeof(string));
                ph_tbl.Columns.Add("trans_rt", typeof(string));//08
                ph_tbl.Columns.Add("trans_flg", typeof(string));
                ph_tbl.Columns.Add("trans_amt", typeof(string));
                ph_tbl.Columns.Add("mat_rt", typeof(string));//09
                ph_tbl.Columns.Add("mat_flg", typeof(string));
                ph_tbl.Columns.Add("mat_amt", typeof(string));

                #endregion
                if (iconID == "F10144")
                {
                    mq1 = "SELECT a.code,a.srno,a.trannum,a.trandt,a.gsm,a.bf,a.rctgrade,a.rct,a.t_rct,a.cost,a.desc_ as layer,a.totrct,a.totcost  FROM wb_CORRCST_LAYER a WHERE  trim(a.TRANNUM)='" + barCode + "'";
                }
                else
                {
                    mq1 = "SELECT a.code,a.srno,a.trannum,a.trandt,a.gsm,a.bf,a.rctgrade,a.rct,a.t_rct,a.cost,a.desc_ as layer,a.totrct,a.totcost  FROM wb_CORRCST_LAYER a WHERE  trim(a.TRANNUM)='" + mq0 + "'";
                }
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                /////
                if (iconID == "F10144")
                {
                    mq2 = "SELECT a.code,a.trannum,a.trandt, a.srno,a.rate,a.flag,a.amt,a.desc_ as item FROM wb_CORRCST_CONVC a WHERE trim(a.TRANNUM)='" + barCode + "'";
                }
                else
                {
                    mq2 = "SELECT a.code,a.trannum,a.trandt, a.srno,a.rate,a.flag,a.amt,a.desc_ as item FROM wb_CORRCST_CONVC a WHERE trim(a.TRANNUM)='" + mq0 + "'";
                }

                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                if (dt.Rows.Count > 0)
                {
                    #region

                    dr1 = dtm.NewRow();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (dt1.Rows[i]["srno"].ToString() == "00")
                        {
                            dr1["gsm_0"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_0"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_0"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_0"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_0"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_0"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "01")
                        {
                            dr1["gsm_1"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_1"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_1"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_1"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_1"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_1"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "02")
                        {
                            dr1["gsm_2"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_2"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_2"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_2"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_2"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_2"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "03")
                        {
                            dr1["gsm_3"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_3"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_3"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_3"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_3"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_3"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                        else if (dt1.Rows[i]["srno"].ToString() == "04")
                        {
                            dr1["gsm_4"] = dt1.Rows[i]["gsm"].ToString().Trim();
                            dr1["bf_4"] = dt1.Rows[i]["bf"].ToString().Trim();
                            dr1["rctgrade_4"] = dt1.Rows[i]["rctgrade"].ToString().Trim();
                            dr1["rct_4"] = dt1.Rows[i]["rct"].ToString().Trim();
                            dr1["t_rct_4"] = dt1.Rows[i]["t_rct"].ToString().Trim();
                            dr1["cost_4"] = dt1.Rows[i]["cost"].ToString().Trim();
                            dr1["tot_t_Rct"] = dt1.Rows[i]["totrct"].ToString().Trim();
                            dr1["tot_cost"] = dt1.Rows[i]["totcost"].ToString().Trim();
                        }
                    }
                    dtm.Rows.Add(dr1);
                    #endregion
                }
                if (dt2.Rows.Count > 0)
                {
                    #region
                    dr2 = ph_tbl.NewRow();
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        if (dt2.Rows[j]["srno"].ToString() == "00")
                        {
                            dr2["strch_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["strch_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["strch_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "01")
                        {
                            dr2["pva_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["pva_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["pva_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "02")
                        {
                            dr2["power_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["power_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["power_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "03")
                        {
                            dr2["fuel_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["fuel_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["fuel_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "04")
                        {
                            dr2["pins_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["pins_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["pins_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "05")
                        {
                            dr2["ink_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["ink_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["ink_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "06")
                        {
                            dr2["labr_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["labr_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["labr_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "07")
                        {
                            dr2["admin_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["admin_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["admin_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "08")
                        {
                            dr2["trans_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["trans_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["trans_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                        if (dt2.Rows[j]["srno"].ToString() == "09")
                        {
                            dr2["mat_rt"] = dt2.Rows[j]["rate"].ToString().Trim();
                            dr2["mat_flg"] = dt2.Rows[j]["flag"].ToString().Trim();
                            dr2["mat_amt"] = dt2.Rows[j]["amt"].ToString().Trim();
                        }
                    }
                    ph_tbl.Rows.Add(dr2);
                    #endregion
                }
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dtm.TableName = "layers";
                    dsRep.Tables.Add(dtm);
                    ph_tbl.TableName = "conversion_cost";
                    dsRep.Tables.Add(ph_tbl);
                    //  dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "box_dimsn", "box_dimsn", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;


            case "F10150":
            case "F10145": //for form
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                header_n = "CSBS Estimation";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = "";
                mq2 = "SELECT distinct boxtypecode ,flute,trim(imagepath) as fstr FROM  wb_corrcst_flutem  where branchcd !='DD' and trim(boxtypecode) !='-'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                if (iconID == "F10145")
                {
                    SQuery = "select '" + header_n + "' as header,a.* ,b.name as box_name from wb_corrcst_csbs a, wb_corrcst_flutem b where trim(a.boxtypecode)=trim(b.boxtypecode) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + barCode + "'";
                }
                else
                {
                    SQuery = "select '" + header_n + "' as header,a.* from wb_corrcst_csbs a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + mq0 + "'";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("mLogo", typeof(System.Byte[]));
                if (dt.Rows.Count > 0)
                {
                    mq1 = fgen.seek_iname_dt(dt1, " boxtypecode='" + dt.Rows[0]["boxtypecode"].ToString().Trim() + "'", "fstr");
                    if (mq1 != "")
                    {
                        try
                        {
                            fpath = mq1;
                            FilStr = new FileStream(fpath, FileMode.Open);
                            BinRed = new BinaryReader(FilStr);
                            dt.Rows[0]["mLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                            FilStr.Close();
                            BinRed.Close();
                        }
                        catch { }
                    }
                    ////////////////
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cal_req_comp", "cal_req_comp", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F10256":
                header_n = "Costing Sheet";
                SQuery = "select '" + header_n + "' as header, a.* from wb_tran_cost a where a.branchcd||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ")";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "Y";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cost_print_SURY", "cost_print_SURY", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
            case "F10185":
                header_n = "Pre Costing Report";
                //if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI" || frm_cocd == "MAYU" || frm_cocd == "KCLG" || frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "VPAC")
                {
                    SQuery = "Select a.*,(case when trim(nvl(b.INAME,'-'))='-' then a.t121 else b.INAME end) as INAME from (select a.*,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (Select * from somas_anx a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "') a left outer join famst b on trim(a.acode)=trim(b.acode)) a left outer join item b on trim(a.icode)=trim(b.icode)";
                }
                //else
                //{
                //    SQuery = "select a.*,b.aname,c.iname from somas_anx a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icodE)=trim(c.icode) and a.type='PN' and A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + barCode + "'";
                //}
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    frm_rptName = "cnitc";
                    if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI" || frm_cocd == "MAYU" || frm_cocd == "KCLG") frm_rptName = "csydb";
                    else if (frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "VPAC") frm_rptName = "costingbest";

                    Print_Report_BYDS(frm_cocd, frm_mbr, "cnitc", frm_rptName, dsRep, header_n, "Y");
                }
                break;
            case "F10134":
                #region laminate bom
                header_n = "Laminate Bom";
                mq10 = ""; dt = new DataTable();
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT '" + header_n + "' as header, a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as parent_icode,trim(b.iname) as p_iname,b.cpartno as prod_Code,c.irate,trim(a.icode) as child_icode,a.sampqty as qty,a.col1 as child_iname, nvl(a.qty1,0) as thick,nvl(a.qty2,0) as density,nvl(a.qty3,0) as gsm_wet,nvl(a.qty4,0) as solid,nvl(a.qty5,0) as gsm_Dry,nvl(a.qty6,0) as percentage,nvl(a.qty7,0) as grid_width,nvl(a.qty8,0) as grid_qty,a.srno,a.obsv1 as slit_reel_wdth,a.obsv2 as reel_weight,a.obsv3 as core_size_inch,a.obsv4 as core_type,a.obsv5 as pack_type,a.obsv6 as widht,a.obsv7 as trim_wstg,a.obsv8 as std_wstg,a.obsv9 as tot_wstg,a.obsv10 as sqm_lami,a.amdtno FROM INSPVCH A,ITEM B,item c WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) AND TRIM(A.BRANCHCD)='" + frm_mbr + "' and TRIM(A.TYPE)='" + frm_vty + "' and TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Laminate_Bom", "std_Laminate_Bom", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F10134A":
                #region poly bom
                header_n = "Ploy Bom";
                mq10 = ""; dt = new DataTable();
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT  '" + header_n + "' as header, a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as parent_icode,trim(b.iname) as p_iname,b.cpartno as prod_Code,c.irate,trim(a.icode) as child_icode,a.sampqty as qty,a.col1 as child_iname, nvl(a.qty1,0) as thick,nvl(a.qty2,0) as density,nvl(a.qty3,0) as gsm_wet,nvl(a.qty4,0) as solid,nvl(a.qty5,0) as gsm_Dry,nvl(a.qty6,0) as percentage,nvl(a.qty7,0) as grid_width,nvl(a.qty8,0) as grid_qty,a.srno,a.obsv1 as slit_reel_wdth,a.obsv2 as reel_weight,a.obsv3 as core_size_inch,a.obsv4 as core_type,a.obsv5 as pack_type,a.obsv6 as widht,a.obsv7 as trim_wstg,a.obsv8 as std_wstg,a.obsv9 as tot_wstg,a.obsv10 as sqm_lami,a.amdtno FROM INSPVCH A,ITEM B,item c WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) AND TRIM(A.BRANCHCD)='" + frm_mbr + "' and TRIM(A.TYPE)='" + frm_vty + "' and TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Poly_Bom", "std_Poly_Bom", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F10134B":
                #region pouch bom
                header_n = "Pouch-Bom";
                mq10 = ""; dt = new DataTable();
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "SELECT  '" + header_n + "' as header,a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as parent_icode,trim(b.iname) as p_iname,b.cpartno as prod_Code,c.irate,trim(a.icode) as child_icode,a.sampqty as qty,a.col1 as child_iname, nvl(a.qty1,0) as thick,nvl(a.qty2,0) as gsm,nvl(a.qty3,0) as p_length,nvl(a.qty4,0) as p_width,nvl(a.qty5,0) as p_area,nvl(a.qty6,0) as p_Area_s,nvl(a.qty7,0) as wights,nvl(a.qty8,0) as qty_lamk,a.srno,a.obsv1 as slit_reel_wdth,a.obsv2 as reel_weight,a.obsv3 as core_size_inch,a.obsv4 as core_type,a.obsv5 as pack_type,a.obsv6 as widht,a.obsv7 as trim_wstg,a.obsv8 as std_wstg,a.obsv9 as tot_wstg,a.obsv10 as sqm_lami,a.amdtno FROM INSPVCH A,ITEM B, item c WHERE TRIM(A.ACODE)=TRIM(B.ICODE) and trim(a.icode)=trim(c.icode) AND TRIM(A.BRANCHCD)='" + frm_mbr + "' and TRIM(A.TYPE)='" + frm_vty + "' and TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + barCode + ") order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pouch_Bom", "std_Pouch_Bom", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F10351":
            case "F10352":
            case "F10353":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Service Req Entry";
                if (iconID == "F10351")
                {
                    //SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,c.iname from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    frm_rptName = "serv_req_entry";
                }
                else if (iconID == "F10352")
                {
                    // SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname from wb_service a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,c.iname from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    frm_rptName = "serv_req_entry";
                }
                else
                {
                    //SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,(case when nvl(a.chk_by,'-')='-' then 'OPEN' ELSE 'CLOSE' END) AS status from wb_service a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    SQuery = "select '" + header_n + "' as header,'" + iconID + "' as iconid, a.*,b.aname,c.iname,(case when nvl(a.chk_by,'-')='-' then 'OPEN' ELSE 'CLOSE' END) AS status from wb_service a left outer join famst b on trim(a.acode)=trim(b.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                    frm_rptName = "serv_req_entry_eng";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, "");
                }
                break;

            case "F10196": ///label costing mlab
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Label Costing";
                //SQuery = "select '" + header_n + "' as header, a.*,b.aname,c.iname from wb_CYLINDER a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                SQuery = "select '" + header_n + "' as header, a.* from wb_CYLINDER a where a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";  //WITHOUT JOINING      
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "lbl_costing_MLAB", "lbl_costing_MLAB", dsRep, header_n);
                }
                break;

            case "F10199": //SPPI OFFSET LABEL COSTING PRINT             
            case "F10197": //SPPI LABEL COSTING PRINT             with cyl
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Offset-Label Costing";
                SQuery = "select '" + header_n + "' as header,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.icode,is_number(a.t1) as lbl_wid,is_number(a.t2) as lbl_hyt,is_number(a.t3) as qty,is_number(a.t4) as acros,is_number(a.t5) as arnd,is_number(a.t6) as ups,is_number(a.t7) as actl_wid_matl,is_number(a.t8) as seting_wstg_color,is_number(a.t9) as rung_mtr_mtr,is_number(a.t10) as tot_sqm,is_number(a.t11) as passes,is_number(a.t12) as req_wid,is_number(a.t13) as color,is_number(a.t14) as tot_wstg,is_number(a.t15) as gap_Acros,is_number(a.t16) as gap_Arnd,is_number(a.t17) as diff,is_number(a.t18) as rung_mtr_mm,is_number(a.t19) as tot_rmtr_used,is_number(a.t20) as prod_cost,is_number(a.t21) as margin_percent,is_number(a.t22) as margin_aed,is_number(a.t23) as total,is_number(a.t24) as vat_percent,is_number(a.t25) as vat_val,is_number(a.t26) as gd_tot,a.t27,is_number(a.t28) as matl1_rate,a.t29,is_number(a.t30) as matl2_rate,a.t31,is_number(a.t32) as matl3_rate,a.t33,is_number(a.t34) as matl4_tot,a.t35,is_number(a.t36) as ink_rt,a.t37,a.t38,is_number(a.t39) as ink_cost,a.t40,is_number(a.t41) as plate_rt,is_number(a.t42) as plate_cost,a.t43,is_number(a.t44) as var_rt,is_number(a.t45) as t45, is_number(a.t46) as var_cost,a.t47,is_number(a.t48) as die_rt,is_number(a.t49) as t49,is_number(a.t50) as t50,is_number(a.t51) as t51,is_number(a.t52) as t52,is_number(a.t53) as die_cost,a.t54,is_number(a.t55) as emb_rt,is_number(a.t56) as t56,is_number(a.t57) as t57,is_number(a.t58) as t58,is_number(a.t59)  as t59,is_number(a.t60) as emb_cost,a.t61,is_number(a.t62) as emb_white_rt,is_number(a.t63) as t63,is_number(a.t64) as t64,is_number(a.t65) as t65,is_number(a.t66) as t66,is_number(a.t67) as emb_whitw_cost,a.t68,is_number(a.t69) as mach1_cost,a.t70,is_number(a.t71) as mch2_cost,is_number(a.t72) as t72,is_number(a.t73) as t73,is_number(a.t74) as t74,a.t75 as mcha_Code,a.t76 as mch2_code,is_number(a.t77) as t77,is_number(a.t78) as t78 , b.aname,c.iname from SOMAS_ANX a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + mq2 + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Offset_lbl_costing_SPPI", "Offset_lbl_costing_SPPI", dsRep, header_n);
                }
                break;

            case "F10186C":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                header_n = "Detailed Flexible Costing";
                SQuery = "select '" + header_n + "' as header,a.vchnum as vch,to_char(a.vchdate,'dd/mm/yyyy') as vchd,A.* from wb_precost a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Pre_Cost_SPPI", "Pre_Cost_SPPI", dsRep, header_n);
                }
                break;
            case "F10135S":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = "SHADE / SPECIAL PRODUCT DEVLOPMENT REQUEST";
                SQuery = "Select '" + header_n + "' as header,A.ACODE,E.USERNAME,I.COL1 AS MASTER,a.branchcd,A.ebr,(CASE when nvl(A.PROD_cAT,'-')='LP' THEN '(Liquid Paint Division)' ELSE '(Powder Coating Division)' END) AS PROD_CATG, nvl(A.PROD_cAT,'LP') as PROD_cAT,nvl(A.PROD_NAME,'-') as PROD_NAME,nvl(A.HO_STATUS,'-') as HO_STATUS,A.COL4 AS MDNAME,A.col56,nvl(A.col57,'-') as col57, nvl(A.num1,0) as num1,nvl(A.num2,0) as num2,A.ENQ_STATUS,(CASE WHEN trim(NVL(A.col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(A.EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(A.col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(A.col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,A.invno,to_char(A.invdate,'dd/mm/yyyy') as invdate, NVL(A.COL51,'-') AS COL51,NVL(A.COL52,'0') AS COL52,NVL(A.COL53,'-') AS COL53,A.COL54,A.vchnum,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,A.col1,A.col2,A.col3,A.col21, A.col11,A.col5,A.col59 AS COL4,A.col6,A.col7,A.col8,A.col9,A.col10,A.col12,A.col27,A.col15,A.col18,nvl(A.col16,'-') as col16,nvl(A.col17,'-') as col17,nvl(A.col19,'-') as col19,A.col28,A.col22,A.col35,A.col37,A.col39,A.col25,A.col20,A.col13,A.col14,A.remarks,A.col40,NVL(A.col30,'-') AS COL30,NVL(A.col31,'-') AS COL31,NVL(A.col32,'-') AS COL32,NVL(A.col33,'-') AS COL33,NVL(A.col34,'-') AS col34,NVL(A.col36,'-') AS col36,NVL(A.col38,'-') AS col38,A.col41,A.col42,NVL(A.col43,'-') AS col43,TO_CHAR(NVL(A.docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(A.col44,'-') AS  col44,NVL(A.col45,'-') AS col45,NVL(A.col46,'-') AS col46,NVL(A.COL47,'-') AS col47,NVL(A.col48,'-') AS col48,NVL(A.col49,'-') AS col49,TO_CHAR(NVL(A.COL50,SYSDATE),'DD/MM/YYYY') as col50,A.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_dt,NVL(A.col60,'-') AS col60,NVL(A.col61,'-') AS col61,NVL(A.col62,'-') AS col62,NVL(A.col63,'-') AS col63,NVL(A.col64,'-') AS col64,NVL(A.col65,'-') AS col65,NVL(A.col66,'-') AS col66,NVL(A.col67,'-') AS col67,NVL(A.col68,'-') AS col68,NVL(A.col69,'-') AS col69, NVL(A.col70,'-') AS col70,NVL(A.col71,'-') AS col71,NVL(A.col72,'-') AS col72,NVL(A.col73,'-') AS col73,NVL(A.col74,'-') AS col74,NVL(A.col75,'-') AS col75,NVL(A.col76,'-') AS col76,NVL(A.col77,'-') AS col77,NVL(A.col78,'-') AS col78,NVL(A.col79,'-') AS col79,NVL(A.col80,'-') AS col80,NVL(A.col81,'-') AS col81,NVL(A.col82,'-') AS col82,NVL(A.col83,'-') AS col83,NVL(A.col84,'-') AS col84,NVL(A.col85,'-') AS col85,NVL(A.col86,'-') AS col86,NVL(A.col87,'-') AS col87,A.SDR_NO,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from EVAS E,scratch A left join inspmst i on trim(a.col30)=trim(i.acode) and i.type='SF' where TRIM(A.ACODE)=TRIM(E.USERID) AND A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_CHAr(A.vchdate,'DD/MM/YYYY') in '" + mq1 + "' order by a.col30";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SDR", "SDR", dsRep, header_n);
                }
                break;
            case "F10125":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = "SHADE / SPECIAL PRODUCT DEVLOPMENT REQUEST";
                SQuery = "Select * from typegrp where id='BN' and branchcd='" + frm_mbr + "' AND trim(VCHNUM)||to_Char(vchdate,'dd/mm/yyyy') in (" + mq1 + ") order by TYPE1";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dt = fgen.addBarCode(dt, "name", true);
                    dsRep.Tables.Add(dt);

                    Print_Report_BYDS(frm_cocd, frm_mbr, "BIN_Stkr", "BIN_Stkr", dsRep, header_n);
                }
                break;
        }
    }

    public void make_bom_print(DataTable FinalDT, string Curricode, int lvl, string MQTY,List<string> MyParents)
    {

        
        DataTable CurrentDT = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.BRANCHCD,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.srno,A.ICODE," +
              "A.IBCODE,A.LINKAGE,A.naration,A.IBQTY,(case when B.IQD>0 then B.IQD else B.irate end) AS BCHRATE,b.iname as ibname,b.MAKER,b.cpartno as bcpartno," +
              "b.unit as bunit,substr(a.ibcat,2,6) as ibcat,a.main_issue_no,a.sub_issue_no,a.st_type,a.ibwt" +
              ",C.UNIT AS PUNIT ,C.MAKER AS PMAKER,c.CPARTNO AS PPARTNO,C.INAME  AS PINAME" +
              "" +
              " FROM ITEMOSP A" +
              ",ITEM B ,item C WHERE TRIM(A.ICODE)=TRIM(C.ICODE)   AND TRIM(A.IBCODE)=TRIM(B.ICODE) " +
              "AND TRIM(A.ICODE)='" + Curricode + "' order by a.srno");


        for (int x = 0; x < CurrentDT.Rows.Count; x++)
        {
            dro = FinalDT.NewRow();
            dro["lvl"] = lvl;
            dro["srno"] = 1;
            dro["icode"] = CurrentDT.Rows[x]["icode"].ToString().Trim();
            dro["branchcd"] = CurrentDT.Rows[x]["branchcd"].ToString().Trim();
            dro["ibcat"] = CurrentDT.Rows[x]["ibcat"].ToString().Trim();

            dro["vchnum"] = FinalDT.Rows[0]["vchnum"].ToString().Trim();
            dro["vchdate"] = FinalDT.Rows[0]["vchdate"].ToString().Trim();

            dro["bvchnum"] = CurrentDT.Rows[x]["vchnum"].ToString().Trim();
            dro["bvchdate"] = CurrentDT.Rows[x]["vchdate"].ToString().Trim();


            //dro["ibqty"] = (Convert.ToDouble(dt2.Rows[x]["ibqty"]) * Convert.ToDouble(vdview1[0].Row["ibqty"])).ToString();
            dro["ibqty"] = CurrentDT.Rows[x]["ibqty"];
            dro["ibcode"] = CurrentDT.Rows[x]["ibcode"].ToString().Trim();
            dro["linkage"] = CurrentDT.Rows[x]["linkage"].ToString().Trim();
            dro["naration"] = CurrentDT.Rows[x]["naration"].ToString().Trim();
            dro["irate"] = CurrentDT.Rows[x]["bchrate"];


            //from Parent
            dro["iname"] = CurrentDT.Rows[0]["Piname"].ToString().Trim();

            dro["cpartno"] = CurrentDT.Rows[0]["PPARTNO"].ToString().Trim();
            dro["unit"] = CurrentDT.Rows[0]["Punit"].ToString().Trim();

            dro["sname"] = CurrentDT.Rows[0]["Piname"].ToString().Trim();
            dro["mqty"] = MQTY;
            dro["pcode"] = CurrentDT.Rows[0]["icode"].ToString().Trim();
            ///End Parent

            dro["MAKER"] = CurrentDT.Rows[0]["MAKER"].ToString().Trim();

            dro["ibname"] = CurrentDT.Rows[x]["ibname"].ToString().Trim();
            dro["bcpartno"] = CurrentDT.Rows[x]["bcpartno"].ToString().Trim();
            dro["bunit"] = CurrentDT.Rows[x]["bunit"].ToString().Trim();

            dro["ent_by"] = FinalDT.Rows[0]["ent_by"].ToString().Trim();
            dro["ent_dt"] = FinalDT.Rows[0]["ent_dt"].ToString().Trim();

            dro["val"] = "0";
            FinalDT.Rows.Add(dro);
            string icode = CurrentDT.Rows[x]["ibcode"].ToString().Trim();
            if (icode.Substring(0, 1) == "7" || icode.Substring(0, 1) == "8"
         || icode.Substring(0, 1) == "9")
            {
                if (MyParents.Contains(icode)) {
                    dro["naration"] = "XXXX Wrong Item XXXX";
                }
                else
                {
                    MyParents.Add(icode);
                    make_bom_print(FinalDT, icode, lvl + 1, CurrentDT.Rows[x]["ibqty"].ToString(), MyParents);
                }
            }
        }
    }
    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));
        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
        }
        else
        {
        }
        data_set.Dispose();
    }

    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";

        if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr, "Y"));
        else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));

        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
        }
        else
        {
        }
        data_set.Dispose();
    }

    public override void VerifyRenderingInServerForm(Control control)
    { return; }

    private ReportDocument GetReportDocument(DataSet rptDS, string rptFileName)
    {
        string repFilePath = Server.MapPath("" + rptFileName + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath);
        repDoc.Refresh();
        repDoc.SetDataSource(rptDS);
        rptDS.Dispose();
        return repDoc;
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch (Exception ex) { fgen.FILL_ERR(ex.Message.ToString().Trim() + "==> dprint ==> At the Time of Page UnLoad."); }
    }

    protected override void OnUnload(EventArgs e)
    {
        try
        {
            base.OnUnload(e);
            this.Unload += new EventHandler(Report_Default_Unload);
        }
        catch { }
    }

    void Report_Default_Unload(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch { }
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        if (data_found == "N")
        {
            return;
        }
        else
        {
            repDoc.Close();
            repDoc.Dispose();
        }
    }

    public void conv_pdf(DataSet dataSet, string rptFile)
    {
        //if (1 == 2)
        {
            repDoc = GetReportDocument(dataSet, rptFile);
            Stream oStream = repDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            byte[] byteArray = null;
            byteArray = new byte[oStream.Length];
            oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(byteArray);

            Response.Flush();
            Response.Close();
            repDoc.Clone();
            repDoc.Dispose();
        }
    }

    public void del_file(string path)
    {
        try
        {
            fpath = Server.MapPath(path);
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }

    protected void btnexp_Click(object sender, EventArgs e)
    {
        DataSet ds = (DataSet)Session["data_set"];
        if (ds.Tables[0].Rows.Count > 0)
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            fgen.exp_to_excel(ds.Tables[0], "ms-excel", "xls", frm_FileName);
        }
    }

    protected void btnexptopdf_Click(object sender, EventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnprint1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            conv_pdf(ds, rpt);
        }
        catch (Exception ex) { ex.Message.ToString(); }
    }

}