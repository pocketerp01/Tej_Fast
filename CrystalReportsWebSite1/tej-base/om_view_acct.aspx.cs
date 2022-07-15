using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class fin_acct_reps_om_view_acct : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, cond, CSR, xprdrange1, SQuery1, SQuery2, SQuery3;
    string mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, mq11, mq12, mq13, mq14, mq15, mq16, mq17, mq18, mq19, mq20, mq21, mq22, mq23, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
    int m, l; DataTable dt, dt1, dt2, dt3, dtm, dtm1, dtDummy, dt4, dt5, dt6, dt7, dt8, dt9, dt10, ph_tbl, dt11, dt24, dt12, dt13, dt14, dt15, dt16, dt17, dt18, dt19, dt20, dt21, dt22, dt23, dtdrsim, dticode, mdt;
    string frm_qstr, frm_formID, frm_vty, xprdRange1, frm_cDt1, frm_cDt2, frm_cocd, er1, er2;
    string col1 = "", date_, cond1; string hscode = "", eff_flag = "";
    string s_code1 = "", s_code2 = "";
    DataRow dr1, dr2, oporow;
    double db, db1, db2, db3, db4, db5, db6, db7, db8, db9, db0, db10, db11, db12, db13, db14, db15, db16, db17, db18, db19, db20, db21, db22, db23, db24, db25, db26, db27, db28, db29, db_op;
    DataView View1, View2;
    int cnt; DataView dv, view1, view2;
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
                frm_cocd = co_cd;
                uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
            }

            hfhcid.Value = frm_formID;

            if (!Page.IsPostBack)
            {
                branch_Cd = "branchcd='" + mbr + "'";
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
        branch_Cd = "branchcd='" + mbr + "'";
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
        else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);//THIS LINE IS CMNT BY ME FOR VIEW EXPORT SO CHECKLIST
        else
        {
            // else if we want to ask another query / another msg / date range etc.
            header_n = "";
            switch (HCID)
            {
                case "F70444":
                    #region
                    header_n = "Monthly Depreciation-Sumamry Report";
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add(new DataColumn("Category", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Asset_Value", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("LY_BF", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "January", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "February", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "March", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "April", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "May", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "June", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "July", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "August", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "September", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "October", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "November", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "December", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("Grand_Total", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("Closing", typeof(double)));//op bal in this

                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
                    // mq0 = "select branchcd||acode as fstr,branchcd, vchnum as pur_entry,to_char(vchdate,'dd/mm/yyyy') as vchdate,type, grpcode,grp,acode,assetid,assetname,basiccost,op_dep ,deprpday,original_cost as asset_Value from wb_fa_pur  where branchcd='" + mbr + "' and type='10' and vchdate " + xprdrange + " order by fstr";//old
                    mq0 = "select branchcd||acode as fstr,grpcode,acode,sum(original_cost) as asset_Value from wb_fa_pur  where branchcd='" + mbr + "' and type='10' and vchdate " + xprdrange + " group by  branchcd||acode ,grpcode,acode order by FSTR";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0); //dt for aset value.......this is main dt for loop

                    mq1 = "select fstr,grpcode,acode,sum(op_Dep) as LY_BF FROM (select branchcd||acode as fstr,grpcode,acode,op_dep from wb_fa_pur  where branchcd='" + mbr + "' and type='10' and vchdate " + xprdrange + "  union all  select branchcd||acode as fstr,grpcode,acode,cramt as op_Dep from wb_fa_vch  where branchcd='" + mbr + "' and type='30' and vchdate<to_date('" + frm_cDt1 + "','dd/mm/yyyy')) GROUP BY fstr,grpcode,acode ORDER BY FSTR";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //this dt for type 30 and summary for op_dep

                    mq2 = "select fstr,grpcode,acode,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar,sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct,sum(nov) as nov,sum(dec) as dec from (select branchcd||acode as fstr,grpcode,acode,(case when to_char(vchdate,'MM/YYYY')='01/" + year + "' then cramt else 0 end) as jan,(case when to_char(vchdate,'MM/YYYY')='02/" + year + "' then cramt else 0 end) as feb,(case when to_char(vchdate,'MM/YYYY')='03/" + year + "' then cramt else 0 end) as mar,(case when to_char(vchdate,'MM/YYYY')='04/" + year + "' then cramt else 0 end) as apr,(case when to_char(vchdate,'MM/YYYY')='05/" + year + "' then cramt else 0 end) as may,(case when to_char(vchdate,'MM/YYYY')='06/" + year + "' then cramt else 0 end) as jun,(case when to_char(vchdate,'MM/YYYY')='07/" + year + "' then cramt else 0 end) as jul,(case when to_char(vchdate,'MM/YYYY')='08/" + year + "' then cramt else 0 end) as aug,(case when to_char(vchdate,'MM/YYYY')='09/" + year + "' then cramt else 0 end) as sep,(case when to_char(vchdate,'MM/YYYY')='10/" + year + "' then cramt else 0 end) as oct,(case when to_char(vchdate,'MM/YYYY')='11/" + year + "' then cramt else 0 end) as nov,(case when to_char(vchdate,'MM/YYYY')='12/" + year + "' then cramt else 0 end) as dec from wb_fa_vch where branchcd='" + mbr + "' and type='30' ) group by fstr,grpcode,acode order by fstr";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2); ///this dt for month wise depr

                    mq3 = "select type1, name from typegrp where id='FA' and branchcd <> 'DD' ";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);//for grpname 
                    // dr2 = new DataRow();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region
                            dr2 = ph_tbl.NewRow();
                            db13 = 0; db14 = 0; db15 = 0;
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0;
                            dr2["Category"] = fgen.seek_iname_dt(dt3, "type1='" + dt.Rows[i]["grpcode"].ToString().Trim() + "'", "name");//assest name
                            db13 = fgen.make_double(dt.Rows[i]["asset_Value"].ToString().Trim());
                            dr2["Asset_Value"] = Math.Round(db13, 2);
                            db14 = fgen.make_double(fgen.seek_iname_dt(dt1, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "LY_BF"));
                            dr2["LY_BF"] = Math.Round(db14, 2);
                            db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "jan"));
                            dr2["" + year + "January"] = Math.Round(db1, 2);
                            db2 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "feb"));
                            dr2["" + year + "February"] = Math.Round(db2, 2);
                            db3 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "mar"));
                            dr2["" + year + "March"] = Math.Round(db3, 2);
                            db4 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "apr"));
                            dr2["" + year + "April"] = Math.Round(db4, 2);
                            db5 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "may"));
                            dr2["" + year + "May"] = Math.Round(db5, 2);
                            db6 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "jun"));
                            dr2["" + year + "June"] = Math.Round(db6, 2);
                            db7 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "jul"));
                            dr2["" + year + "July"] = Math.Round(db7, 2);
                            db8 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "aug"));
                            dr2["" + year + "August"] = Math.Round(db8, 2);
                            db9 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "sep"));
                            dr2["" + year + "September"] = Math.Round(db9, 2);
                            db10 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "oct"));
                            dr2["" + year + "October"] = Math.Round(db10, 2);
                            db11 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "nov"));
                            dr2["" + year + "November"] = Math.Round(db11, 2);
                            db12 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "dec"));
                            dr2["" + year + "December"] = Math.Round(db12, 2);
                            db15 = Math.Round((db1 + db2 + db3 + db4 + db5 + db6 + db7 + db8 + db9 + db10 + db11 + db12), 2);
                            dr2["Grand_Total"] = Math.Round(db15, 2);
                            dr2["Closing"] = Math.Round((db13 - (db14 + db15)), 2);
                            ph_tbl.Rows.Add(dr2);
                            #endregion
                        }
                    }
                    //again cursor for summary
                    dtm = new DataTable();
                    dtm.Columns.Add(new DataColumn("Category", typeof(string)));
                    dtm.Columns.Add(new DataColumn("Asset_Value", typeof(double)));
                    dtm.Columns.Add(new DataColumn("LY_BF", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "January", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "February", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "March", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "April", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "May", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "June", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "July", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "August", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "September", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "October", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "November", typeof(double)));
                    dtm.Columns.Add(new DataColumn("" + year + "December", typeof(double)));
                    dtm.Columns.Add(new DataColumn("Grand_Total", typeof(double)));
                    dtm.Columns.Add(new DataColumn("Closing", typeof(double)));
                    if (ph_tbl.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(ph_tbl);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1.ToTable(true, "Category"); //MAIN                  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(ph_tbl, "Category='" + dr0["Category"] + "'", "", DataViewRowState.CurrentRows);
                            dt4 = viewim.ToTable();
                            dr2 = dtm.NewRow();
                            db13 = 0; db14 = 0; db15 = 0; db = 0;
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0;
                            dr2["Category"] = dt4.Rows[0]["Category"].ToString().Trim();
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                db1 += fgen.make_double(dt4.Rows[i]["Asset_Value"].ToString().Trim());
                                db2 += fgen.make_double(dt4.Rows[i]["LY_BF"].ToString().Trim());
                                db3 += fgen.make_double(dt4.Rows[i]["" + year + "January"].ToString().Trim());
                                db4 += fgen.make_double(dt4.Rows[i]["" + year + "February"].ToString().Trim());
                                db5 += fgen.make_double(dt4.Rows[i]["" + year + "March"].ToString().Trim());
                                db6 += fgen.make_double(dt4.Rows[i]["" + year + "April"].ToString().Trim());
                                db7 += fgen.make_double(dt4.Rows[i]["" + year + "May"].ToString().Trim());
                                db8 += fgen.make_double(dt4.Rows[i]["" + year + "June"].ToString().Trim());
                                db9 += fgen.make_double(dt4.Rows[i]["" + year + "July"].ToString().Trim());
                                db10 += fgen.make_double(dt4.Rows[i]["" + year + "August"].ToString().Trim());
                                db11 += fgen.make_double(dt4.Rows[i]["" + year + "September"].ToString().Trim());
                                db12 += fgen.make_double(dt4.Rows[i]["" + year + "October"].ToString().Trim());
                                db13 += fgen.make_double(dt4.Rows[i]["" + year + "November"].ToString().Trim());
                                db14 += fgen.make_double(dt4.Rows[i]["" + year + "December"].ToString().Trim());
                                db15 += fgen.make_double(dt4.Rows[i]["Grand_Total"].ToString().Trim());
                                db += fgen.make_double(dt4.Rows[i]["Closing"].ToString().Trim());
                            }
                            dr2["Asset_Value"] = Math.Round(db1, 2);
                            dr2["LY_BF"] = Math.Round(db2, 2);
                            dr2["" + year + "January"] = Math.Round(db3, 2);
                            dr2["" + year + "February"] = Math.Round(db4, 2);
                            dr2["" + year + "March"] = Math.Round(db5, 2);
                            dr2["" + year + "April"] = Math.Round(db6, 2);
                            dr2["" + year + "May"] = Math.Round(db7, 2);
                            dr2["" + year + "June"] = Math.Round(db8, 2);
                            dr2["" + year + "July"] = Math.Round(db9, 2);
                            dr2["" + year + "August"] = Math.Round(db10, 2);
                            dr2["" + year + "September"] = Math.Round(db11, 2);
                            dr2["" + year + "October"] = Math.Round(db12, 2);
                            dr2["" + year + "November"] = Math.Round(db13, 2);
                            dr2["" + year + "December"] = Math.Round(db14, 2);
                            dr2["Grand_Total"] = Math.Round(db15, 2);
                            dr2["Closing"] = Math.Round(db, 2);
                            dtm.Rows.Add(dr2);
                        }
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("" + header_n + " " + frm_cDt1 + " To " + frm_cDt2 + " ", frm_qstr);
                    }
                    #endregion
                    break;

                case "F70443":
                    #region
                    header_n = "Monthly Depreciation-Detailed Report";
                    ph_tbl = new DataTable();
                    mq12 = "";
                    mq12 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                    ph_tbl.Columns.Add(new DataColumn("Category", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Asset_Id", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Asset_Name", typeof(string)));
                    ph_tbl.Columns.Add(new DataColumn("Asset_Value", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("LY_BF", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "January", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "February", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "March", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "April", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "May", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "June", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "July", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "August", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "September", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "October", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "November", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("" + year + "December", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("Grand_Total", typeof(double)));
                    ph_tbl.Columns.Add(new DataColumn("Closing", typeof(double)));//op bal in this

                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
                    // mq0 = "select branchcd||acode as fstr,branchcd, vchnum as pur_entry,to_char(vchdate,'dd/mm/yyyy') as vchdate,type, grpcode,grp,acode,assetid,assetname,basiccost,op_dep ,deprpday,original_cost as asset_Value from wb_fa_pur  where branchcd='" + mbr + "' and type='10' and vchdate " + xprdrange + " order by fstr";//old
                    mq0 = "select branchcd||acode as fstr,grpcode,acode,sum(original_cost) as asset_Value,assetid,assetname from wb_fa_pur  where branchcd='" + mbr + "' and type='10' and vchdate " + xprdrange + " group by  branchcd||acode ,grpcode,acode,assetid,assetname order by FSTR";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0); //dt for aset value.......this is main dt for loop

                    mq1 = "select fstr,grpcode,acode,sum(op_Dep) as LY_BF FROM (select branchcd||acode as fstr,grpcode,acode,op_dep from wb_fa_pur  where branchcd='" + mbr + "' and type='10' and vchdate " + xprdrange + "  union all  select branchcd||acode as fstr,grpcode,acode,cramt as op_Dep from wb_fa_vch  where branchcd='" + mbr + "' and type='30' and vchdate<to_date('" + frm_cDt1 + "','dd/mm/yyyy')) GROUP BY fstr,grpcode,acode ORDER BY FSTR";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1); //this dt for type 30 and summary for op_dep

                    mq2 = "select fstr,grpcode,acode,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar,sum(apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct,sum(nov) as nov,sum(dec) as dec from (select branchcd||acode as fstr,grpcode,acode,(case when to_char(vchdate,'MM/YYYY')='01/" + year + "' then cramt else 0 end) as jan,(case when to_char(vchdate,'MM/YYYY')='02/" + year + "' then cramt else 0 end) as feb,(case when to_char(vchdate,'MM/YYYY')='03/" + year + "' then cramt else 0 end) as mar,(case when to_char(vchdate,'MM/YYYY')='04/" + year + "' then cramt else 0 end) as apr,(case when to_char(vchdate,'MM/YYYY')='05/" + year + "' then cramt else 0 end) as may,(case when to_char(vchdate,'MM/YYYY')='06/" + year + "' then cramt else 0 end) as jun,(case when to_char(vchdate,'MM/YYYY')='07/" + year + "' then cramt else 0 end) as jul,(case when to_char(vchdate,'MM/YYYY')='08/" + year + "' then cramt else 0 end) as aug,(case when to_char(vchdate,'MM/YYYY')='09/" + year + "' then cramt else 0 end) as sep,(case when to_char(vchdate,'MM/YYYY')='10/" + year + "' then cramt else 0 end) as oct,(case when to_char(vchdate,'MM/YYYY')='11/" + year + "' then cramt else 0 end) as nov,(case when to_char(vchdate,'MM/YYYY')='12/" + year + "' then cramt else 0 end) as dec from wb_fa_vch where branchcd='" + mbr + "' and type='30' ) group by fstr,grpcode,acode order by fstr";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2); ///this dt for month wise depr

                    mq3 = "select type1, name from typegrp where id='FA' and branchcd <> 'DD' ";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);//for grpname 
                    // dr2 = new DataRow();
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            #region
                            dr2 = ph_tbl.NewRow();
                            db13 = 0; db14 = 0; db15 = 0;
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0;
                            dr2["Category"] = fgen.seek_iname_dt(dt3, "type1='" + dt.Rows[i]["grpcode"].ToString().Trim() + "'", "name");
                            dr2["Asset_Id"] = dt.Rows[i]["assetid"].ToString().Trim();
                            dr2["Asset_Name"] = dt.Rows[i]["assetname"].ToString().Trim();
                            db13 = fgen.make_double(dt.Rows[i]["asset_Value"].ToString().Trim());
                            dr2["Asset_Value"] = Math.Round(db13, 2);
                            db14 = fgen.make_double(fgen.seek_iname_dt(dt1, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "LY_BF"));
                            dr2["LY_BF"] = Math.Round(db14, 2);
                            db1 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "jan"));
                            dr2["" + year + "January"] = Math.Round(db1, 2);
                            db2 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "feb"));
                            dr2["" + year + "February"] = Math.Round(db2, 2);
                            db3 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "mar"));
                            dr2["" + year + "March"] = Math.Round(db3, 2);
                            db4 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "apr"));
                            dr2["" + year + "April"] = Math.Round(db4, 2);
                            db5 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "may"));
                            dr2["" + year + "May"] = Math.Round(db5, 2);
                            db6 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "jun"));
                            dr2["" + year + "June"] = Math.Round(db6, 2);
                            db7 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "jul"));
                            dr2["" + year + "July"] = Math.Round(db7, 2);
                            db8 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "aug"));
                            dr2["" + year + "August"] = Math.Round(db8, 2);
                            db9 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "sep"));
                            dr2["" + year + "September"] = Math.Round(db9, 2);
                            db10 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "oct"));
                            dr2["" + year + "October"] = Math.Round(db10, 2);
                            db11 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "nov"));
                            dr2["" + year + "November"] = Math.Round(db11, 2);
                            db12 = fgen.make_double(fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[i]["fstr"].ToString().Trim() + "'", "dec"));
                            dr2["" + year + "December"] = Math.Round(db12, 2);
                            db15 = Math.Round((db1 + db2 + db3 + db4 + db5 + db6 + db7 + db8 + db9 + db10 + db11 + db12), 2);
                            dr2["Grand_Total"] = Math.Round(db15, 2);
                            dr2["Closing"] = Math.Round((db13 - (db14 + db15)), 2);
                            ph_tbl.Rows.Add(dr2);
                            #endregion
                        }
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("" + header_n + " " + frm_cDt1 + " To " + frm_cDt2 + " ", frm_qstr);
                    }
                    #endregion
                    break;

                case "F49132":
                case "F49133":
                case "F49134":
                    SQuery = "select TRIM(type1) as fstr,name,type1 as code from type where id='V' and type1='4F' ORDER BY code";
                    header_n = "Select Sale Type";
                    break;
                //case "F70162":
                //    mq0 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                //    mq1 = mq0.Substring(6, 4);
                //    int cyr = Convert.ToInt32(mq1) + 1;
                //    SQuery = "";
                //    string m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                //    string eff_Dt = " a.invdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and a.invdate<= to_date('" + mq0 + "','dd/mm/yyyy')";
                //    SQuery = "SELECT ACODE AS PARTY_CODE,PARTY,SUM(APR) AS APRIL,SUM(MAY) AS MAY,SUM(JUNE) AS JUNE,SUM(JULY) AS JULY,SUM(AUG) AS AUG,SUM(SEP) AS SEP,SUM(OCT) AS OCT,SUM(NOV) AS NOV,SUM(DEC) AS DEC,SUM(JAN) AS JAN,SUM(FEB) AS FEB,SUM(MAR) AS MAR,PAYMENT,SUM(NET) AS NET FROM (select acode,PARTY,payment,SUM(net) as NET ,DECODE (TO_CHAR(ODUEDAYS,'MM/YYYY'),'04/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS APR,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'05/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS may,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'06/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS JUNE,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'07/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS JULY,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'08/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS AUG,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'09/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS SEP,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'10/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS OCT,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'11/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS NOV,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'12/" + mq1 + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS DEC,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'01/" + cyr + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS JAN,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'02/" + cyr + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS FEB,DECODE (TO_CHAR(oduedays,'MM/YYYY'),'03/" + cyr + "', SUM(DRAMT)-SUM(CRAMT) ,'0') AS MAR from (select TRIM(a.ACODE) as acode,a.branchcd,B.PAYMENT,B.aname as party ,A.invdate,add_months(A.INVDATE,CEIL((CASE WHEN B.PAY_NUM=0 THEN 1 ELSE B.PAY_NUM END) / 30)) as ODUEdays,A.dramt,A.cramt,A.dramt-A.cramt as net from recdata A,FAMST B where TRIM(a.ACODE)=TRIM(b.ACODE) and A.branchcd='" + mbr + "' and " + eff_Dt + ")  GROUP BY acode,PARTY,payment,oduedays ) GROUP BY ACODE ,PARTY,PAYMENT HAVING SUM(APR)+SUM(MAY)+SUM(JUNE)+SUM(JULY)+SUM(AUG)+SUM(SEP)+SUM(OCT)+SUM(NOV)+SUM(DEC)+SUM(JAN)+SUM(FEB)+SUM(MAR)!=0 ORDER BY ACODE ";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    if (SQuery.Length > 0)
                //    {
                //        fgen.Fn_open_rptlevel("Monthly Payable Report As On " + mq0 + "", frm_qstr);
                //    }
                //    else
                //    {
                //        fgen.msg("-", "AMSG", "No Data Exist");
                //    }
                //    break;
                case "F70162":
                    mq0 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                    mq1 = mq0.Substring(6, 4);
                    int cyr = Convert.ToInt32(mq1) + 1;
                    SQuery = "";
                    string m1 = fgen.seek_iname(frm_qstr, co_cd, "select params from controls where id='R01'", "params");
                    string eff_Dt = " a.vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and a.vchdate<= to_date('" + mq0 + "','dd/mm/yyyy')";
                    mq15 = " and a.invdate>=to_date('" + m1.Trim() + "','dd/mm/yyyy')-60";

                    #region
                    dt = new DataTable();
                    SQuery = "select trim(a.acode) as acode,a.branchcd,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate ,a.dramt,a.cramt,a.net,CEIL((CASE WHEN B.PAY_NUM=0 THEN 1 ELSE B.PAY_NUM END) / 30) as pay_num ,trim(b.aname) as aname,'' as vchdate from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD'  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') and type like '5%' GROUP BY branchcd,ACODE,INVNO,INVDATE union all SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' and SUBSTR(ACODE,1,2)IN('02','05','06','16') and type = '32'  GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16')  GROUP BY branchcd,ACODE,INVNO,INVDATE) a , famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' " + mq15 + " order by acode";
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);

                    dt1 = new DataTable();
                    mq0 = "select distinct branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,invno,to_char(invdate,'dd/mm/yyyy') as invdate from voucher where branchcd='" + mbr + "' and  type like '5%' and to_char(vchdate,'dd/mm/yyyy')>'" + m1 + "' order by acode";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);

                    if (dt.Rows.Count > 0)
                    {
                        dt.Columns["vchdate"].MaxLength = 15;
                        dt.Columns.Add("pay_terms", typeof(string));
                        dt.Columns.Add("vmnth", typeof(string));
                        dt.Columns.Add("duedays", typeof(string));

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            mq1 = fgen.seek_iname_dt(dt1, "invno='" + dt.Rows[i]["invno"].ToString().Trim() + "' and invdate='" + dt.Rows[i]["invdate"].ToString().Trim() + "' and acode ='" + dt.Rows[i]["acode"].ToString().Trim() + "'", "vchdate");
                            //dt.Rows[i]["vchdate"] = fgen.seek_iname_dt(dt1, "invno='" + dt.Rows[i]["invno"].ToString().Trim() + "' and invdate='" + dt.Rows[i]["invdate"].ToString().Trim() + "'", "vchdate");
                            if (mq1 != "0")
                            {
                                dt.Rows[i]["vchdate"] = Convert.ToDateTime(mq1).ToString("dd/MM/yyyy");
                                DateTime datetimeee = Convert.ToDateTime(mq1);
                                DateTime dtetime;
                                int month = Convert.ToInt32(dt.Rows[i]["PAY_NUM"].ToString().Trim());
                                dtetime = datetimeee.AddMonths(month);
                                dt.Rows[i]["duedays"] = dtetime.ToString("dd/MM/yyyy");
                                dt.Rows[i]["vmnth"] = dtetime.ToString("MM/yyyy");
                            }
                            else
                            {
                                dt.Rows[i]["vchdate"] = dt.Rows[i]["invdate"].ToString().Trim();
                                DateTime datetimeee = Convert.ToDateTime(dt.Rows[i]["vchdate"]);
                                DateTime dtetime;
                                int month = Convert.ToInt32(dt.Rows[i]["PAY_NUM"].ToString().Trim());
                                dtetime = datetimeee.AddMonths(month);
                                dt.Rows[i]["duedays"] = dtetime.ToString("dd/MM/yyyy");
                                dt.Rows[i]["vmnth"] = dtetime.ToString("MM/yyyy");
                            }
                        }

                        dt5 = new DataTable();
                        dt5.Columns.Add("ACODE", typeof(string));
                        dt5.Columns.Add("ANAME", typeof(string));
                        dt5.Columns.Add("PAY_TERM", typeof(string));
                        dt5.Columns.Add("APRIL", typeof(double));
                        dt5.Columns.Add("MAY", typeof(double));
                        dt5.Columns.Add("JUNE", typeof(double));
                        dt5.Columns.Add("JULY", typeof(double));
                        dt5.Columns.Add("AUG", typeof(double));
                        dt5.Columns.Add("SEP", typeof(double));
                        dt5.Columns.Add("OCT", typeof(double));
                        dt5.Columns.Add("NOV", typeof(double));
                        dt5.Columns.Add("DEC", typeof(double));
                        dt5.Columns.Add("JAN", typeof(double));
                        dt5.Columns.Add("FEB", typeof(double));
                        dt5.Columns.Add("MARCH", typeof(double));
                        dt5.Columns.Add("Total", typeof(double));

                        DataView view1 = new DataView(dt);
                        dt6 = new DataTable();
                        //dt6 = view1.ToTable(true, "acode","vmnth");
                        dt6 = view1.ToTable(true, "acode");
                        foreach (DataRow dr1 in dt6.Rows)
                        {
                            //DataView view2 = new DataView(dt, "acode='" + dr1["acode"].ToString().Trim() + "' and vmnth='"+dr1["vmnth"].ToString().Trim()+"'", "", DataViewRowState.CurrentRows);
                            DataView view2 = new DataView(dt, "acode='" + dr1["acode"].ToString().Trim() + "'   ", "", DataViewRowState.CurrentRows);
                            dt7 = new DataTable();
                            dt7 = view2.ToTable();
                            DataRow oporow = dt5.NewRow();
                            double tot = 0;
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; eff_flag = "";
                            for (int j = 0; j < dt7.Rows.Count; j++)
                            {
                                if (Convert.ToDateTime(dt7.Rows[j]["vchdate"]) >= Convert.ToDateTime(m1))
                                {
                                    eff_flag = "Y";
                                    oporow["acode"] = dt7.Rows[j]["acode"].ToString().Trim();
                                    oporow["ANAME"] = dt7.Rows[j]["ANAME"].ToString().Trim();
                                    oporow["PAY_TERM"] = Convert.ToDouble(dt7.Rows[j]["PAY_NUM"].ToString().Trim()) * 30;

                                    mq10 = dt7.Rows[j]["vmnth"].ToString().Trim().Substring(0, 2);
                                    switch (mq10)
                                    {
                                        case "04":
                                            db4 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["APRIL"] = Math.Round(db4, 2);
                                            break;
                                        case "05":
                                            db5 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["MAY"] = Math.Round(db5, 2);
                                            break;
                                        case "06":
                                            db6 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["JUNE"] = Math.Round(db6, 2);
                                            break;
                                        case "07":
                                            db7 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["JULY"] = Math.Round(db7, 2);
                                            break;
                                        case "08":
                                            db8 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["AUG"] = Math.Round(db8, 2);
                                            break;
                                        case "09":
                                            db9 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["SEP"] = Math.Round(db9, 2);
                                            break;
                                        case "10":
                                            db10 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["OCT"] = Math.Round(db10, 2);
                                            break;
                                        case "11":
                                            db11 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["NOV"] = Math.Round(db11, 2);
                                            break;
                                        case "12":
                                            db12 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["DEC"] = Math.Round(db12, 2);
                                            break;
                                        case "01":
                                            db1 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["JAN"] = Math.Round(db1, 2);
                                            break;
                                        case "02":
                                            db2 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["FEB"] = Math.Round(db2, 2);
                                            break;
                                        case "03":
                                            db3 += Convert.ToDouble(dt7.Rows[j]["net"].ToString().Trim());
                                            oporow["MARCH"] = Math.Round(db3, 2);
                                            break;
                                    }
                                    tot = db4 + db5 + db6 + db7 + db8 + db9 + db10 + db11 + db12 + db1 + db2 + db3;
                                    oporow["Total"] = tot;
                                }
                                else { }
                            }
                            if (eff_flag == "Y")
                            {
                                dt5.Rows.Add(oporow);
                            }
                        }
                    #endregion
                        SQuery = "";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = dt5;
                        fgen.Fn_open_rptlevelJS("Monthly Payable Report(Spl. Customized)", frm_qstr);
                    }
                    break;
                case "F70124":
                    SQuery = "";
                    updateGSTClass(frm_qstr, co_cd);
                    break;
                case "F70175":
                    SQuery = "SELECT Name,type1,id from type where id='#' order by type1";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    SQuery = "";
                    fgen.Fn_open_rptlevel("Nature of Accounts (Top level)", frm_qstr);
                    break;
                case "F70555":
                case "F70240*":
                case "F70145":
                    SQuery = "select TRIM(type1) as fstr,name,type1 as code from type where id='V' and type1 like '2%' ORDER BY code";
                    header_n = "Select Type";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "N");
                    break;
                case "F70240":
                    SQuery = "";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "A2":
                    SQuery = "SELECT TRIM(GST_NO) AS FSTR,NAME,GST_NO from type where ID='B' ORDER BY TYPE1";
                    header_n = "Select Type";
                    break;

                case "F70225":
                    SQuery = "select trim(type1) as fstr,type1 as code,name from type where type1 like '5%' and id='V' and type1!='54' order by type1";
                    header_n = "Select Payment Voucher Type";
                    break;

                case "F70415":
                    SQuery = "select distinct branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr, vchnum,to_char(vchdate,'dd/mm/yyyy')  as vchdate, Ent_by, to_char(ent_dt,'dd/mm/yyyy') as Ent_dt  from wb_fa_vch where branchcd= '" + mbr + "' and type='30' and vchdate " + xprdrange + " and cramt>0 order by vchnum desc";
                    header_n = "Select Depreciation Voucher to Write Back";
                    break;

                case "F70422":
                    SQuery = "select TRIM(ent_by) as fstr,TRIM(ent_by) as ent_by from ast_reco where type='RV' ORDER BY TRIM(ent_by)";
                    header_n = "Select User_Id who scanned assets for Reco";
                    break;

                case "F70247":
                    SQuery = "SELECT TYPE1 AS FSTR,TYPE1 AS CODE,TRIM(NAME) AS NAME FROM TYPE WHERE ID='V'  ORDER BY TYPE1";
                    header_n = "Select Voucher";
                    break;

                case "P70106D":
                case "P70106C":
                case "P70110C":
                case "P70110D":
                    if (frm_formID == "P70106D") SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS VCH_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT,A.ACODE AS CODE,B.ANAME AS PARTY,A.ENT_BY,TO_CHAR(A.ENT_DaTe,'DD/MM/YYYY') AS ENT_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM VOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A." + branch_Cd + " AND A.TYPE='59' AND A.VCHDATE " + xprdrange + " AND SUBSTR(A.ACODE,1,2) NOT IN ('07','20') ORDER BY VDD DESC, A.VCHNUM DESC ";
                    else if (frm_formID == "P70110C") SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS VCH_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT,A.ACODE AS CODE,B.ANAME AS PARTY,A.ENT_BY,TO_CHAR(A.ENT_DaTe,'DD/MM/YYYY') AS ENT_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM VOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A." + branch_Cd + " AND A.TYPE in ('32') AND A.VCHDATE " + xprdrange + " AND SUBSTR(A.ACODE,1,2) NOT IN ('07','20') ORDER BY VDD DESC, A.VCHNUM DESC ";
                    else if (frm_formID == "P70110D") SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS VCH_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT,A.ACODE AS CODE,B.ANAME AS PARTY,A.ENT_BY,TO_CHAR(A.ENT_DaTe,'DD/MM/YYYY') AS ENT_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM VOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A." + branch_Cd + " AND A.TYPE in ('31') AND A.VCHDATE " + xprdrange + " AND SUBSTR(A.ACODE,1,2) NOT IN ('07','20') ORDER BY VDD DESC, A.VCHNUM DESC ";
                    else SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS VCH_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT,A.ACODE AS CODE,B.ANAME AS PARTY,A.ENT_BY,TO_CHAR(A.ENT_DaTe,'DD/MM/YYYY') AS ENT_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM VOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A." + branch_Cd + " AND A.TYPE='58' AND A.VCHDATE " + xprdrange + " AND SUBSTR(A.ACODE,1,2) NOT IN ('07','20') ORDER BY VDD DESC, A.VCHNUM DESC ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_RangeBox("-", frm_qstr);
                    break;
                case "F70426":
                case "F70375":
                    //ASSET Insurance REPORT
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F70710":
                case "F70712":
                case "F70714":
                case "F70716":
                case "F70600":
                case "F70602":
                case "F70604":
                case "F70606":
                case "F05349":
                case "F70650":
                case "F70652":
                case "F70680":
                case "F70438":
                case "F70439":
                case "F70440":
                case "F70441":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "F70291"://FG REG
                case "F70293"://RMREG
                case "F70295"://FG SUMMARY
                case "F70296"://RM SUMMARY
                    fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
                    break;
                case "F70374":
                    SQuery = "SELECT A.type,A.VCHNUM AS VOUCHER_NO,A.VCHDATE AS VOUCHER_dT,A.ACODE AS CODE,B.ANAME as party from (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.BRANCHCD,A.TYPE,A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,1 AS QTY FROM VOUCHER A WHERE A.BRANCHCD='" + mbr + "' and a.vchdate " + xprdrange + " and substr(a.acode,1,2) in ('02','05','06','16')  UNION ALL SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,-1 AS QTY FROM ATCHVCH A WHERE A.BRANCHCD='" + mbr + "'  and a.vchdate " + xprdrange + ") a,famst b where trim(a.acodE)=trim(B.acodE) group by a.fstr,a.vchnum,a.vchdate,a.acode,b.aname,A.type having sum(qty)>0 order by a.type,a.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    SQuery = "";
                    fgen.Fn_open_rptlevel("Pending Voucher List to Upload", frm_qstr);
                    break;
                //case "F70293":
                //    fgen.Fn_open_dtbox("-", frm_qstr);
                //    break;

                case "F70377":
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    break;
                case "F70126":
                case "F70127":
                case "F70128":
                case "F70129":
                case "F70130":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;

                case "M1":
                    SQuery = "SELECT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.GRPCODE,T.NAME AS GROUP_NAME,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,SUM(A.CRAMT) AS CRAMT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM WB_FA_VCH A,TYPEGRP T WHERE TRIM(A.GRPCODE)=TRIM(T.TYPE1) AND T.ID='FA' AND A.BRANCHCD='" + mbr + "' AND A.TYPE='30' GROUP BY TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),TRIM(A.VCHNUM),TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),A.GRPCODE,T.NAME,TO_CHAR(A.VCHDATE,'YYYYMMDD') ORDER BY VDD,VCHNUM,A.GRPCODE";
                    header_n = "Select Entry";
                    break;
            }
            if (SQuery.Length > 1)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                if (HCID == "F70422")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_MSEEKSQL", SQuery);
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                }
                else
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek(header_n, frm_qstr);
                }
            }
        }
    }

    void updateGSTClass(string _frm_qstr, string _frm_cocd)
    {
        dt = new DataTable();
        SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR, A.MATTYPE,A.POTYPE,A.RCODE,A.ACODE,b.rcode as vrcode FROM IVOUCHER A,VOUCHER B WHERE A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAr(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)=B.BRANCHCD||B.TYPE||TRIM(b.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')||TRIM(B.ACODe) AND TRIM(a.MATTYPE)!=TRIM(B.DEPCD) AND A.TYPE in ('58','59') and trim(B.DEPCD)=trim(b.branchcd) ";
        dt = fgen.getdata(_frm_qstr, _frm_cocd, SQuery);

        foreach (DataRow dr in dt.Rows)
        {
            //GST Class
            SQuery = "UPDATE VOUCHER SET DEPCD='" + dr["MATTYPE"].ToString().Trim() + "' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dr["fstr"].ToString().Trim() + "'";
            fgen.execute_cmd(_frm_qstr, _frm_cocd, SQuery);

            //Reason
            if (dr["POTYPE"].ToString().Trim().Length < 2)
            {
                SQuery = "UPDATE IVOUCHER SET POTYPE='07' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dr["fstr"].ToString().Trim() + "'";
                fgen.execute_cmd(_frm_qstr, _frm_cocd, SQuery);
            }

            //RCODE
            if (dr["RCODE"].ToString().Trim().Length < 2)
            {
                if (dr["vrcode"].ToString().Trim().Length > 1)
                {
                    SQuery = "UPDATE IVOUCHER SET RCODE='" + dr["vrcode"].ToString().Trim() + "' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dr["fstr"].ToString().Trim() + "'";
                    fgen.execute_cmd(_frm_qstr, _frm_cocd, SQuery);
                }
            }
        }
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        val = hfhcid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "ANP").ToString().Trim() == "Y")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            switch (val)
            {
                case "F70240":
                case "F70145":
                    if (col1.Length < 2) return;
                    hfcode.Value = col1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F70556":
                    if (value1 == "Y") hfbr.Value = "ABR";
                    else hfbr.Value = "";
                    btnhideF_s_Click("", EventArgs.Empty);
                    break;
            }
        }
        else if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            hfcode.Value = "";
            hfcode.Value = value1;
            col1 = value1;

            switch (val)
            {
                case "P70106D":
                case "P70106C":
                case "P70110C":
                case "P70110D":
                    cond = "N";
                    if (col1.Length < 2) return;
                    if (val == "P70106D") value3 = "59";
                    else if (val == "P70110C") value3 = "32";
                    else if (val == "P70110D") value3 = "31";
                    else value3 = "58";
                    hf1.Value = value3;
                    if (co_cd == "PPAP" || co_cd == "SAIP" || co_cd == "SAIL")
                    {
                        fgen.msg("-", "SMSG", "Do You Want To Print 2 Copies");
                        return;
                    }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", value3);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                        fgen.fin_acct_reps(frm_qstr);
                    }
                    break;

                case "F70555":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    SQuery = "SELECT trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as vch_no,to_Char(a.vchdate,'dd/mm/yyyy') as vch_dt,b.aname as party,a.acode as code,a.refnum,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.acode)=trim(B.acode) and a.branchcd='" + mbr + "' and a.type='" + col1 + "' and a.vchdate " + xprdrange + " and SUBSTR(A.ACODE,1,2) NOT IN ('03','12') order by vdd desc ,a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS_S");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                    break;

                //case "F70240":
                //case "F70145":
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                //    SQuery = "SELECT DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as party,a.acode AS CODE,b.email FROM voucher a, famst b where trim(a.acode)=trim(b.acodE) and substr(a.acode,1,2) in ('05','06') and nvl(trim(b.email),'-')!='-' and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " and a.type='" + col1 + "' ORDER BY b.aname";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "Y");
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_mseek(header_n, frm_qstr);
                //    break;

                case "F70240":
                case "F70145":
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "N");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "A2":
                case "F70225":
                case "F70422":
                    hfcode.Value = value1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "F70415":
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, "select trim(acode) as acode from wb_fa_vch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "'");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        fgen.execute_cmd(frm_qstr, co_cd, "update wb_fa_vch set depr_wbk=cramt,type='31' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "' and acode='" + dt.Rows[i]["acode"].ToString().Trim() + "'");
                        fgen.execute_cmd(frm_qstr, co_cd, "update wb_fa_vch set cramt=0 where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "' and acode='" + dt.Rows[i]["acode"].ToString().Trim() + "'");
                    }
                    fgen.save_info(frm_qstr, co_cd, mbr, value1.Substring(4, 6), value1.Substring(10, 10), uname, frm_vty, "Depreciation written back");
                    fgen.msg("-", "AMSG", "Selected Entry depriciation has been written back");
                    break;

                case "F70247"://list of vouchers
                    hfcode.Value = value1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "F70206"://akshay
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    hf1.Value = value1;
                    if (hf1.Value == "Y")
                    {
                        mq0 = "and nvl(trim(A.check_by),'-')!='-' and nvl(trim(A.app_by),'-')='-'";
                    }
                    else if (hf1.Value == "N")
                    {
                        mq0 = "and nvl(trim(A.check_by),'-')='-'";
                    }
                    else if (hf1.Value == "ALL")
                    {
                        mq0 = "and nvl(trim(A.app_by),'-')='-' and nvl(trim(A.check_by),'-')='-'";
                    }

                    cond = " and ((case when substr(type,1,1) in ('2','3','4') then a.tfccr when type='59' then a.tfccr when substr(type,1,1) in ('1','5','6') then a.tfcdr end)>0 or (case when substr(type,1,1) in ('2','3','4') then a.cramt when type='59' then a.cramt when substr(type,1,1) in ('1','5','6') then a.dramt end)>0)  ";
                    cond1 = "(case when substr(type,1,1) in ('2','3','4') then a.cramt when type='59' then a.cramt when substr(type,1,1) in ('1','5','6') then a.dramt end)";

                    SQuery = "select A.branchcd as branchcd,A.type,trim(A.vchnum) as voucher_no,to_char(A.vchdate,'dd/mm/yyyy') as voucher_date,A.ACODE AS CODE,B.ANAME AS PARTY," + cond1 + " as amount, A.ent_by,to_char(A.ent_date,'dd/mm/yyyy') as entered_Date,A.check_by,to_char(A.check_date,'dd/mm/yyyy') as check_date,A.app_by,to_char(A.app_date,'dd/mm/yyyy') as Approved_date,TO_CHAr(A.VCHDATE,'YYYYMMDD') AS VDD from voucher A  ,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND  A.branchcd='" + mbr + "' AND A.TYPE LIKE '%' and A.vchdate " + xprdrange + " " + mq0 + " " + cond + " and a.srno=1 order by VDD DESC,a.type,trim(A.vchnum)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Pending Voucher Data (for Checking, Approval) For the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "M1":
                    hfval.Value = value1;
                    mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                    hf1.Value = mq0;
                    fgen.Fn_open_dtbox("-", frm_qstr);
                    hfcode.Value = xprdrange;
                    break;

                default:
                    break;
            }
        }
        // else if branch selection box opens then it comes here
        else if (Request.Cookies["REPLY"].Value.Length > 0)
        {
            value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (value1 == "Y")
            {
                hfbr.Value = "ABR";
                branch_Cd = "BRANCHCD NOT IN ('DD','88')";
            }
            else
            {
                hfbr.Value = "";
                branch_Cd = "BRANCHCD='" + mbr + "'";
            }
            switch (val)
            {
                default:
                    // After Branch Consolidate Report  **************
                    // it will ask prdDmp after branch code selection
                    if (hfaskBranch.Value == "Y")
                    {
                        if (value1 == "Y") hfbr.Value = "ABR";
                        else hfbr.Value = "";
                        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    }
                    break;

                //case "F70556":
                //    if (value1 == "Y") hfbr.Value = "ABR";
                //    else hfbr.Value = "";
                //    btnhideF_s_Click("", EventArgs.Empty);
                //    break;                
            }
        }
        else
        {
            // ADD BY MADHVI FOR SHOWING THE DATE RANGE WHEN USER PRESS ESC             
            fgen.Fn_open_prddmp1("-", frm_qstr);

        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        string party_cd = "";
        string part_cd = "";
        string xbstring = "";
        string my_rep_head = "";
        val = hfhcid.Value.Trim();

        string OVERSEAS = "N";
        int ADDER = 1;
        OVERSEAS = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + mbr + "' and OPT_ID='W2027'", "fstr");
        if (OVERSEAS == "Y")
        {
            ADDER = 0;
        }
        string m1 = "", m2 = "", m3 = "", m4 = "", m5 = "", m6 = "", m7 = "", m8 = "", m9 = "", m10 = "", m11 = "", m12 = "";

        m1 = (year.toDouble() + ADDER).ToString().Trim() + "01";
        m2 = (year.toDouble() + ADDER).ToString().Trim() + "02";
        m3 = (year.toDouble() + ADDER).ToString().Trim() + "03";
        m4 = (year.toDouble()).ToString().Trim() + "04";
        m5 = (year.toDouble()).ToString().Trim() + "05";
        m6 = (year.toDouble()).ToString().Trim() + "06";
        m7 = (year.toDouble()).ToString().Trim() + "07";
        m8 = (year.toDouble()).ToString().Trim() + "08";
        m9 = (year.toDouble()).ToString().Trim() + "09";
        m10 = (year.toDouble()).ToString().Trim() + "10";
        m11 = (year.toDouble()).ToString().Trim() + "11";
        m12 = (year.toDouble()).ToString().Trim() + "12";


        //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            value3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

            string mhd = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", "INR");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,99,99,999.99");

            mhd = fgen.seek_iname(frm_qstr, co_cd, "select opt_param from fin_rsys_opt_pw where branchcd='" + mbr + "' and opt_id='W2015'", "opt_param");
            if (mhd != "0" && mhd != "-") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_CURREN", mhd);

            mhd = fgen.seek_iname(frm_qstr, co_cd, "select opt_param from fin_rsys_opt_pw where branchcd='" + mbr + "' and opt_id='W2016'", "opt_param");
            if (mhd != "0" && mhd != "-" && mhd != "I") fgenMV.Fn_Set_Mvar(frm_qstr, "U_BR_COMMA", "999,999,999,999.99");

            string coma_sepr;
            coma_sepr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BR_COMMA");




            fromdt = value1;
            todt = value2;
            string base_type = "";
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
                case "F70422":
                    SQuery = "Select a.  from   where  " + xprdrange + " and  order by ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Data Search(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49132":
                    SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hfcode.Value + "' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Sales Data Search(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49133":
                    if (hfcode.Value.Length > 0)
                    {
                        SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ACODE in (" + hfcode.Value + ") and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    else
                    {
                        SQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ACODE like '%' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Customer Wise Sales(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "F49134":
                    if (hfcode.Value.Length > 0)
                    {
                        SQuery = "Select " + rep_flds + "  from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ICODE in (" + hfcode.Value + ") and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    else
                    {
                        SQuery = "Select " + rep_flds + "  from " + table1 + " , " + table2 + ", " + table3 + "   where a." + branch_Cd + "  and a.type='" + hf1.Value + "' AND A.ICODE like '%' and " + datefld + " " + xprdrange + " and " + joinfld + "  order by " + sortfld;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Product. Wise Sales(Exp.) Checklist for the Period " + value1 + " to " + value2, frm_qstr);
                    break;

                case "P70106D":
                case "P70106C":
                case "P70110C":
                case "P70110D":
                    if (col1.Length < 2) return;
                    col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                case "F70555":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                    fgen.fin_acct_reps(frm_qstr);
                    break;

                //case "F70240":
                //case "F70145":
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "N");
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
                //    fgen.fin_acct_reps(frm_qstr);
                //    break;

                case "F70240":
                case "F70145":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
                    SQuery = "SELECT DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,round(SUM(a.dramt)-sum(a.cramt),2) as amt,b.aname as party,a.acode AS CODE,b.email,to_char(a.vchdate,'yyyymmdd') as vdd FROM voucher a, famst b where trim(a.acode)=trim(b.acodE) and substr(a.acode,1,2) in ('05','06') and nvl(trim(b.email),'-')!='-' and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " and a.type like '2%' GROUP BY trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode),TRIM(A.VCHNUM),to_char(a.vchdate,'dd/mm/yyyy'),b.aname,A.ACODE,B.EMAIL,to_char(a.vchdate,'yyyymmdd') ORDER BY vdd desc,trim(a.vchnum) desc,b.aname";
                    if (co_cd == "PRIN" || co_cd == "SYDB" || co_cd == "SYDP" || co_cd == "PIPL")
                        SQuery = "Select /*+ INDEX_DESC(voucher ind_VCH_DATE) */ trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.rcode) as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as party,a.acode AS CODE,b.email,a.cramt as amt,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where a.vchdate " + xprdrange + " and TRIM(a.RCODE)= TRIM(b.acode) and a.type<>'20' and substr(a.type,1,1)='2' and cramt>0 and a.type like '2%' and a.branchcd='" + mbr + "' and nvl(trim(b.email),'-')!='-' order by vdd desc,vchnum desc";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "Y");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "N");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek(header_n, frm_qstr);
                    break;
                ////
                ///RCPTS CHECKLIST
                case "F70126":
                case "F70127":
                case "F70128":
                case "F70129":
                case "F70130":

                    switch (val)
                    {
                        case "F70126":
                            base_type = "1";
                            mq0 = "Receipt";
                            break;
                        case "F70127":
                            base_type = "2";
                            mq0 = "Payment";
                            break;
                        case "F70128":
                            base_type = "3";
                            mq0 = "Journal";
                            break;
                        case "F70130":
                            base_type = "4";
                            mq0 = "Sales";
                            break;
                        case "F70129":
                            base_type = "5";
                            mq0 = "Purchase";
                            break;
                    }

                    header_n = mq0 + " Vouchers List";
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    string type_var = "-";
                    if (party_cd.Trim().Length <= 1)
                    {
                        type_var = "substr(type,1,1) like '" + base_type + "%'";
                    }
                    else
                    {
                        type_var = "type in (" + party_cd + ")";
                    }

                    SQuery = "select b.aname  as Account_Name,c.aname as Ref_Ac_Name,a.vchnum as Vch_no,to_char(a.vchdate,'dd/mm/yyyy') as Vch_Date,trim(a.acode) as Acode,trim(a.rcode) as Rcode,a.dramt as Dr_Amt,a.cramt as Cr_Amt,a.Naration,a.branchcd,a.type,a.invno as Inv_No,to_chaR(a.invdate,'dd/mm/yyyy') as Inv_Dt,a.refnum as Ref_No,to_chaR(a.refdate,'dd/mm/yyyy') as Ref_Dt,a.ent_by,a.ent_Date from voucher a,famst b,famst c where trim(a.acodE)=trim(b.acodE) and trim(a.rcode)=trim(c.acode) and a.branchcd='" + mbr + "' and  " + type_var + " and a.vchdate " + xprdrange + " order by a.type,a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;


                case "P70099R":
                    header_n = "List of Vouchers Uploaded vs Saved";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    SQuery = "select FSTR AS VAL1,SUM(AMT) AS UPL_AMT,SUM(AMT2) AS IVCH_AMT,SUM(AMT3) AS VCH_AMT FROM (SELECT branchcd||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') AS fstr ,SUM(IS_NUMBER(col6)*IS_NUMBER(col9)) AS AMT,0 AS AMT2,0 AS AMT3 from scratch2 where branchcd='" + mbr + "' and type='DC' and vchdate " + xprdrange + " group by branchcd||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') union all SELECT a.branchcd||trim(a.col1) as fstr,0 AS AMT,SUM(a.IAMOUNT) AS AMT2,sum(b.dramt+b.cramt) AS AMT3 FROM IVOUCHER a ,voucher b where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) =b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.Rcode) and a.BRANCHCD='" + mbr + "' and a.type in ('58','59') and a.vchdate " + xprdrange + " and b.srno='50' group by a.branchcd||trim(a.col1)) GROUP BY FSTR ORDER BY FSTR";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70107":
                    header_n = "Debit Note";
                    SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4 AS HSCODE,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,b.iamount as Basic_Amt, (case when trim(b.iopr)='CG' then b.exc_rate else 0 end) as cgst_rate, (case when trim(b.iopr)='CG' then b.exc_amt else 0 end) as cgst_amount,(Case when trim(b.iopr)='CG' then b.cess_percent else 0 end) as SGST_Rate,(Case when trim(b.iopr)='CG' then b.cess_pu else 0 end) as SGST_amt,(Case when trim(b.iopr)='IG' then b.exc_rate else 0 end) as IGST_Rate,(Case when trim(b.iopr)='IG' then b.exc_amt else 0 end) as IGST_amt,b.spexc_amt as TOT_VAL,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode) AND A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BTCHNO) and trim(A.acode)=trim(c.acode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='DC' AND A.VCHDATE " + xprdrange + " and b.type in ('58','59') and a.num10>0 ORDER BY A.COL33";
                    SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4 AS HSCODE,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,b.iamount as Basic_Amt, (case when trim(b.iopr)='CG' then b.exc_rate else 0 end) as cgst_rate, (case when trim(b.iopr)='CG' then b.exc_amt else 0 end) as cgst_amount,(Case when trim(b.iopr)='CG' then b.cess_percent else 0 end) as SGST_Rate,(Case when trim(b.iopr)='CG' then b.cess_pu else 0 end) as SGST_amt,(Case when trim(b.iopr)='IG' then b.exc_rate else 0 end) as IGST_Rate,(Case when trim(b.iopr)='IG' then b.exc_amt else 0 end) as IGST_amt,b.spexc_amt as TOT_VAL,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode) AND A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BTCHNO) and trim(A.acode)=trim(c.acode) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='DC' AND A.VCHDATE " + xprdrange + " and b.type in ('58','59') and a.num10>0 ORDER BY A.COL33";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "A2": // MERGE BY MADHVI MADE BY AKSHAY ON 25 MAY 2018 .... START
                    // EXPENSE A/C
                    header_n = "Expense A/C";
                    value1 = hfcode.Value;
                    SQuery = "Select Particulars,Acode,sum(Day1) as Day1,sum(Day2) as Day2,sum(Day3) as Day3  ,sum(Day4) as Day4,sum(Day5) as Day5,sum(Day6) as Day6,sum(Day7) as Day7,sum(Day8) as Day8,sum(Day9) as Day9,sum(Day10) as Day10, sum(Day11) as Day11,sum(Day12) as Day12,sum(Day13) as Day13  ,sum(Day14) as Day14,sum(Day15) as Day15,sum(Day16) as Day16,sum(Day17) as Day17,sum(Day18) as Day18,sum(Day19) As Day19,sum(Day20) as Day20, sum(Day21) as Day21,sum(Day22) as Day22,sum(Day23) as Day23  ,sum(Day24) as Day24,sum(Day25) as Day25,sum(Day26) as Day26,sum(Day27) as Day27,sum(Day28) as Day28,sum(Day29) as Day29,sum(Day30) as Day30,sum(Day31) As Day31 from (Select trim(b.aname) as Particulars,a.acode,decode(to_chaR(vchdate,'DD'),'01',sum(A.dramt)-sum(A.cramt),0) as Day1,decode(to_chaR(vchdate,'DD'),'02',sum(A.dramt)-sum(A.cramt),0) as Day2,decode(to_chaR(vchdate,'DD'),'03',sum(A.dramt)-sum(A.cramt),0) as Day3 ,decode(to_chaR(vchdate,'DD'),'04',sum(A.dramt)-sum(A.cramt),0) as Day4,decode(to_chaR(vchdate,'DD'),'05',sum(A.dramt)-sum(A.cramt),0) as Day5,decode(to_chaR(vchdate,'DD'),'06',sum(A.dramt)-sum(A.cramt),0) as Day6,decode(to_chaR(vchdate,'DD'),'07',sum(A.dramt)-sum(A.cramt),0) as Day7,decode(to_chaR(vchdate,'DD'),'08',sum(A.dramt)-sum(A.cramt),0) as Day8,decode(to_chaR(vchdate,'DD'),'09',sum(A.dramt)-sum(A.cramt),0) as Day9,decode(to_chaR(vchdate,'DD'),'10',sum(A.dramt)-sum(A.cramt),0) as Day10, decode(to_chaR(vchdate,'DD'),'11',sum(A.dramt)-sum(A.cramt),0) as Day11,decode(to_chaR(vchdate,'DD'),'12',sum(A.dramt)-sum(A.cramt),0) as Day12,decode(to_chaR(vchdate,'DD'),'13',sum(A.dramt)-sum(A.cramt),0) as Day13 ,decode(to_chaR(vchdate,'DD'),'14',sum(A.dramt)-sum(A.cramt),0) as Day14,decode(to_chaR(vchdate,'DD'),'15',sum(A.dramt)-sum(A.cramt),0) as Day15,decode(to_chaR(vchdate,'DD'),'16',sum(A.dramt)-sum(A.cramt),0) as Day16,decode(to_chaR(vchdate,'DD'),'17',sum(A.dramt)-sum(A.cramt),0) as Day17,decode(to_chaR(vchdate,'DD'),'18',sum(A.dramt)-sum(A.cramt),0) as Day18,decode(to_chaR(vchdate,'DD'),'19',sum(A.dramt)-sum(A.cramt),0) as Day19,decode(to_chaR(vchdate,'DD'),'20',sum(A.dramt)-sum(A.cramt),0) as Day20, decode(to_chaR(vchdate,'DD'),'21',sum(A.dramt)-sum(A.cramt),0) as Day21,decode(to_chaR(vchdate,'DD'),'22',sum(A.dramt)-sum(A.cramt),0) as Day22,decode(to_chaR(vchdate,'DD'),'23',sum(A.dramt)-sum(A.cramt),0) as Day23 ,decode(to_chaR(vchdate,'DD'),'24',sum(A.dramt)-sum(A.cramt),0) as Day24,decode(to_chaR(vchdate,'DD'),'25',sum(A.dramt)-sum(A.cramt),0) as Day25,decode(to_chaR(vchdate,'DD'),'26',sum(A.dramt)-sum(A.cramt),0) as Day26,decode(to_chaR(vchdate,'DD'),'27',sum(A.dramt)-sum(A.cramt),0) as Day27,decode(to_chaR(vchdate,'DD'),'28',sum(A.dramt)-sum(A.cramt),0) as Day28,decode(to_chaR(vchdate,'DD'),'29',sum(A.dramt)-sum(A.cramt),0) as Day29,decode(to_chaR(vchdate,'DD'),'30',sum(A.dramt)-sum(A.cramt),0) as Day30,decode(to_chaR(vchdate,'DD'),'31',sum(A.dramt)-sum(A.cramt),0) as Day31  from voucher a,famst b,type c where a.branchcd=c.type1 and c.id='B' and trim(c.gst_no)='" + value1.Trim() + "' and a.branchcd!='88' and trim(a.acode)=trim(b.acode) and a.vchdate " + xprdrange + "  and (substr(a.acode,1,1)>'2' or (substr(a.acode,1,6)='120000' and a.depcd='10'))  group by a.acode,trim(b.aname),to_char(vchdate,'DD')) group by Particulars,Acode order by Acode";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "A3":
                    // DEBIT CREDIT
                    header_n = "Debit Credit";
                    SQuery = "select a.branchcd,a.type,b.aname,trim(a.acode) as Cust_code,trim(a.invno) as Inv_no,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,sum(a.iamount)as Ivch_amt,sum(a.acctgamt) as Acct_amt,sum(a.iamount)-sum(a.acctgamt) as Diff_amt,a.vchnum,a.vchdate from (Select branchcd,type,acode,invno,invdate,iamount,0 as acctgamt,vchnum,vchdate from ivoucher where branchcd!='DD' and type in ('58*','59') and vchdate " + xprdrange + "  union all Select branchcd,type,rcode,(case when length(trim(nvl(originv_no,'-')))<=1 then invno else originv_no end) as inv,(case when length(trim(nvl(originv_no,'-')))<=1 then invdate else originv_dt end) as inv_dt,0 as iamount,cramt as acctgamt,vchnum,vchdate from voucher where branchcd!='DD' and type in ('58*','59') and vchdate " + xprdrange + "  and acode like '2%')a,famst b where trim(A.acode)=trim(B.acode) group by a.branchcd,a.type,trim(a.acode),trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),a.vchnum,a.vchdate,b.aname having sum(a.iamount)-sum(a.acctgamt)<>0 order by a.branchcd,a.vchnum,a.vchdate";
                    SQuery = "select b.aname,trim(a.acode) as Cust_code,trim(a.invno) as Inv_no,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,sum(a.iamount)as Ivch_amt,sum(a.acctgamt) as Acct_amt,sum(a.iamount)-sum(a.acctgamt) as Diff_amt,a.vchnum,a.vchdate,a.type,a.branchcd from (Select branchcd,type,acode,invno,invdate,iamount,0 as acctgamt,vchnum,vchdate from ivoucher where branchcd!='DD' and type in ('58*','59') and vchdate " + xprdrange + "  union all Select branchcd,type,rcode,(case when length(trim(nvl(originv_no,'-')))<=1 then invno else originv_no end) as inv,(case when length(trim(nvl(originv_no,'-')))<=1 then invdate else originv_dt end) as inv_dt,0 as iamount,cramt as acctgamt,vchnum,vchdate from voucher where branchcd!='DD' and type in ('58*','59') and vchdate " + xprdrange + "  and acode like '2%')a,famst b where trim(A.acode)=trim(B.acode) group by a.branchcd,a.type,trim(a.acode),trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),a.vchnum,a.vchdate,b.aname having sum(a.iamount)-sum(a.acctgamt)<>0 order by a.branchcd,a.vchnum,a.vchdate";
                    SQuery = "select a.branchcd,a.type,b.aname,trim(a.acode) as Cust_code,trim(a.invno) as Inv_no,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,sum(a.iamount)as Ivc_amt,sum(a.acctgamt) as Acct_amt,sum(a.iamount)-sum(a.acctgamt) as Diff_amt,a.vchnum,a.vchdate from (Select branchcd,type,acode,invno,invdate,iamount,0 as acctgamt,vchnum,vchdate from ivoucher where branchcd!='DD' and type in ('58*','59') and vchdate " + xprdrange + "  union all Select branchcd,type,rcode,(case when length(trim(nvl(originv_no,'-')))<=1 then invno else originv_no end) as inv,(case when length(trim(nvl(originv_no,'-')))<=1 then invdate else originv_dt end) as inv_dt,0 as iamount,cramt as acctgamt,vchnum,vchdate from voucher where branchcd!='DD' and type in ('58*','59') and vchdate " + xprdrange + "  and acode like '2%')a,famst b where trim(A.acode)=trim(B.acode) group by a.branchcd,a.type,trim(a.acode),trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),a.vchnum,a.vchdate,b.aname having sum(a.iamount)-sum(a.acctgamt)<>0 order by a.branchcd,a.vchnum,a.vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70188":
                    // GST RATE SALE INVOICE WISE SUMMARY
                    header_n = "GST Rate Sale Invoice Wise Summary";
                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + mbr + "' and OPT_ID='W2027'", "fstr");
                    if (mq0 == "Y")
                    {
                        SQuery = "Select a.aname as Customer,a.HSCODE,a.vat_rate,sum(a.vat_amt) as vat_amt,a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.gst_no as trn_no,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.Invno as Ven_Inv,to_char(a.Invdate,'dd/mm/yyyy') as Ven_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,a.exc_Rate as vat_rate,a.exc_Amt as vat_amt,a.Invno,A.Invdate,a.type,c.unit from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a, sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.vat_rate,a.type,a.invno,a.invdate,a.unit,b.bill_Tot order by a.vchnum,a.vchdate";
                    }
                    else
                    {
                        SQuery = "Select a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.aname as Customer,a.gst_no as GST_No,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.HSCODE,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.Invno as Ven_Inv,a.Invdate as Ven_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a, sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.invno,a.invdate,a.unit,b.bill_Tot order by a.vchnum,a.vchdate";
                        SQuery = "Select a.aname as Customer,a.HSCODE,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.gst_no as GST_No,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.Invno as Ven_Inv,a.Invdate as Ven_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a, sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.invno,a.invdate,a.unit,b.bill_Tot order by a.vchnum,a.vchdate";
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "A5":
                    // HSN WISE NON MRR PURCHASE INVOICE
                    header_n = "HSN Wise Non MRR Purchase Invoice";
                    SQuery = "Select A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,c.iname,c.hscode,a.iqtyin,a.irate,a.iamount,round(a.iqtyin*a.irate,2) as QtyXRate,round(a.exp_punit,2) as Txb_Chgs,a.iopr as TX_type,(Case when trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,a.Location as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno";
                    SQuery = "Select b.aname,c.iname,c.hscode,(Case when trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt,b.staten as state,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.gst_no,b.staffcd as St_code,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.iqtyin,a.irate,a.iamount,round(a.iqtyin*a.irate,2) as QtyXRate,round(a.exp_punit,2) as Txb_Chgs,a.iopr as TX_type,a.icode,a.type,a.Location as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '5%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70153":
                    // TAX RATE WISE BASIC, TAX SUMMARY
                    header_n = "Tax Rate Wise Basic, Tax Summary";
                    SQuery = "Select to_Char(sum(iamount),'999,99,99,999.99') as Basic_Val,to_Char(sum(CGST_amt),'999,99,99,999.99') as CGST_amt,to_Char(sum(SGST_amt),'999,99,99,999.99') as SGST_amt,to_Char(sum(IGST_amt),'999,99,99,999.99') as IGST_amt,CGST_RT,SGST_Rate,IGST_Rt,sum(Tool_Cost) as Tool_Cost,sum(pack_Cost) as pack_Cost,sum(frt_Cost) as frt_Cost from (Select A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.morder) group by CGST_RT,SGST_Rate,IGST_Rt order by CGST_RT";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;
                // --------------------------- END 

                case "F70138":  // Mg 02.06.18
                    header_n = "HSN Wise FG Stock Summary Checklist";
                    xprdrange1 = " between to_date('" + todt + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 ";
                    SQuery = "select hscode,unit,sum(Op_Bal) as Op_Bal,sum(Inw_Qty) as Inw_Qty,sum(Outw_Qty) as Outw_Qty,sum(Closing_qty) as Closing_qty FROM (select b.iname,b.hscode,sum(opening) as Op_Bal,sum(a.cdr) as Inw_Qty,sum(a.ccr) as Outw_Qty,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_qty,b.unit,b.cpartno,a.icode from (Select icode, YR_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + mbr + "' union all  select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE )a, item b where trim(A.icode)=trim(B.icode) and trim(A.icode) between '90000000' and '99999999' group by b.iname,b.cpartno,b.hscode,b.unit,a.icode having sum(a.opening)+sum(a.cdr)-sum(a.ccr)<>0)GROUP BY hscode,unit ORDER BY hscode,unit";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                //case "F70139":  // Mg 02.06.18
                //    header_n = "Item Wise FG Stock Summary Checklist";
                //    xprdrange1 = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1 ";
                //    //  SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, hscode,unit,sum(Op_Bal) as Op_Bal,sum(Inw_Qty) as Inw_Qty,sum(Outw_Qty) as Outw_Qty,sum(Closing_qty) as Closing_qty FROM (select b.iname,b.hscode,sum(opening) as Op_Bal,sum(a.cdr) as Inw_Qty,sum(a.ccr) as Outw_Qty,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_qty,b.unit,b.cpartno,a.icode from (Select icode, YR_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + mbr + "' union all  select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE )a, item b where trim(A.icode)=trim(B.icode) and trim(A.icode) between '90000000' and '99999999' group by b.iname,b.cpartno,b.hscode,b.unit,a.icode having sum(a.opening)+sum(a.cdr)-sum(a.ccr)<>0)GROUP BY hscode,unit ORDER BY hscode,unit ";
                //    //  SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, hscode,unit,sum(Op_Bal) as Op_Bal,sum(Inw_Qty) as Inw_Qty,sum(Outw_Qty) as Outw_Qty,sum(Closing_qty) as Closing_qty FROM (select b.iname,b.hscode,sum(opening) as Op_Bal,sum(a.cdr) as Inw_Qty,sum(a.ccr) as Outw_Qty,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_qty,b.unit,b.cpartno,a.icode from (Select icode, YR_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + mbr + "' union all  select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' GROUP BY ICODE union all select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + mbr + "' and type like '%' and vchdate " + xprdrange + " and store='Y' GROUP BY ICODE )a, item b where trim(A.icode)=trim(B.icode) and trim(A.icode) between '90000000' and '99999999' group by b.iname,b.cpartno,b.hscode,b.unit,a.icode having sum(a.opening)+sum(a.cdr)-sum(a.ccr)<>0)GROUP BY hscode,unit ORDER BY hscode,unit ";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                //    fgen.Fn_open_rptlevel(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
                //    break;

                case "F70224":
                    // SALES REGISTER
                    #region Sales Register
                    header_n = "Sales Register";
                    dtDummy = new DataTable();
                    int m = 0;
                    dtDummy.Columns.Add("Invno", typeof(string));
                    dtDummy.Columns.Add("Invdate", typeof(string));
                    dtDummy.Columns.Add("Acode", typeof(string));
                    dtDummy.Columns.Add("Aname", typeof(string));
                    dtDummy.Columns.Add("vdd", typeof(string));
                    dtDummy.Columns.Add("Z1", typeof(double));
                    dtDummy.Columns.Add("Z2", typeof(double));
                    dtDummy.Columns.Add("Z3", typeof(double));
                    dtDummy.Columns.Add("Z4", typeof(double));
                    dtDummy.Columns.Add("Z5", typeof(double));
                    dtDummy.Columns.Add("Z6", typeof(double));

                    mq0 = "";
                    mq0 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and  a.type like '4%' and a.vchdate " + xprdrange + " and a.cramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1)='2'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    m = 5;
                    for (int k = 0; k < 6; k++)
                    {
                        try
                        {
                            dtDummy.Columns[m].ColumnName = "Z" + dt.Rows[k]["acode"].ToString().Trim();
                            m++;
                        }
                        catch { }
                    }

                    dtDummy.Columns.Add("ZOthers", typeof(double));

                    dtDummy.Columns.Add("Y1", typeof(double));
                    dtDummy.Columns.Add("Y2", typeof(double));
                    dtDummy.Columns.Add("Y3", typeof(double));
                    dtDummy.Columns.Add("Y4", typeof(double));
                    dtDummy.Columns.Add("Y5", typeof(double));
                    dtDummy.Columns.Add("Y6", typeof(double));

                    mq1 = "";
                    mq1 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and  a.type like '4%' and a.vchdate " + xprdrange + " and a.cramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1) in ('0','3')";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);
                    m = 12;
                    for (int k = 0; k < 6; k++)
                    {
                        try
                        {
                            dtDummy.Columns[m].ColumnName = "Y" + dt1.Rows[k]["acode"].ToString().Trim();
                            m++;
                        }
                        catch { }
                    }

                    dtDummy.Columns.Add("YOthers", typeof(double));
                    dtDummy.Columns.Add("TOT", typeof(double));
                    mq2 = "select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + mbr + "' and  a.type like '4%' and a.vchdate " + xprdrange + " and a.cramt>0 and  substr(a.acode,1,2) not in ('16')  group by a.acode,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),b.aname,to_char(a.vchdate,'yyyymmdd') order by vchnum";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt2);
                        dt4 = new DataTable();
                        dt4 = view1.ToTable(true, "vchnum", "vchdate", "rcode");
                        dr1 = null;
                        foreach (DataRow dr2 in dt4.Rows)
                        {
                            DataView view2 = new DataView(dt2, "vchnum='" + dr2["vchnum"].ToString().Trim() + "' and vchdate='" + dr2["vchdate"].ToString().Trim() + "' and rcode='" + dr2["rcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt5 = new DataTable();
                            dt5 = view2.ToTable();

                            dr1 = dtDummy.NewRow();
                            db1 = 0; db2 = 0; db3 = 0;
                            for (int i = 0; i < dt5.Rows.Count; i++)
                            {
                                dr1["INVNO"] = dt5.Rows[i]["vchnum"].ToString();
                                dr1["INVdate"] = dt5.Rows[i]["vchdate"].ToString();
                                dr1["vdd"] = dt5.Rows[i]["vdd"].ToString();
                                dr1["acode"] = dt5.Rows[i]["rcode"].ToString();
                                dr1["aname"] = dt5.Rows[i]["aname"].ToString();
                                mq4 = dt5.Rows[i]["acode"].ToString().Trim().Substring(0, 1);
                                // ORIGINAL COND  if (mq4.Contains("2") || mq4.Contains("3"))
                                if (mq4.Contains("2"))
                                {
                                    try
                                    {
                                        dr1["Z" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                    }
                                    catch
                                    {
                                        db1 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                        dr1["ZOthers"] = db1;
                                    }
                                    db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString()); ;
                                }
                                else if (mq4.Contains("0") || mq4.Contains("3")) // ORIGINALLY THERE WAS ONLY ELSE CONDITION
                                {
                                    try
                                    {
                                        dr1["Y" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                    }
                                    catch
                                    {
                                        db2 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                        dr1["YOthers"] = db2;
                                    }
                                    db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString()); ;
                                }
                                // db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString()); ;
                            }
                            dr1["tot"] = db3;
                            dtDummy.Rows.Add(dr1);
                        }
                    }

                    if (dtDummy.Rows.Count > 0)
                    {
                        dr1 = dtDummy.NewRow();
                        foreach (DataColumn dc in dtDummy.Columns)
                        {
                            db1 = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 7)
                            { }
                            else
                            {
                                mq4 = "sum(" + dc.ColumnName + ")";
                                db1 += fgen.make_double(dtDummy.Compute(mq4, "").ToString());
                                dr1[dc] = db1;
                            }
                        }
                        dr1[0] = "TOTAL";
                        dtDummy.Rows.InsertAt(dr1, 0);

                        dtDummy.Columns[5].ColumnName = "Z1"; // SALES
                        dtDummy.Columns[6].ColumnName = "Z2";
                        dtDummy.Columns[7].ColumnName = "Z3";
                        dtDummy.Columns[8].ColumnName = "Z4";
                        dtDummy.Columns[9].ColumnName = "Z5";
                        dtDummy.Columns[10].ColumnName = "Z6";

                        dtDummy.Columns[12].ColumnName = "Y1"; // TAX
                        dtDummy.Columns[13].ColumnName = "Y2";
                        dtDummy.Columns[14].ColumnName = "Y3";
                        dtDummy.Columns[15].ColumnName = "Y4";
                        dtDummy.Columns[16].ColumnName = "Y5";
                        dtDummy.Columns[17].ColumnName = "Y6";

                        l = 1;
                        for (int k = 0; k < 6; k++)
                        {
                            try
                            {
                                dtDummy.Columns["Z" + l].ColumnName = dt.Rows[k]["aname"].ToString().Trim();
                            }
                            catch { }
                            l++;
                        }
                        l = 1;
                        for (int k = 0; k < 6; k++)
                        {
                            try
                            {
                                dtDummy.Columns["Y" + l].ColumnName = dt1.Rows[k]["aname"].ToString().Trim();
                            }
                            catch { }
                            l++;
                        }

                        dtDummy.Columns.Remove("vdd");
                        try
                        {
                            if (dtDummy.Columns["Z1"].ColumnName == "Z1")
                            {
                                dtDummy.Columns.Remove("Z1");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z2"].ColumnName == "Z2")
                            {
                                dtDummy.Columns.Remove("Z2");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z3"].ColumnName == "Z3")
                            {
                                dtDummy.Columns.Remove("Z3");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z4"].ColumnName == "Z4")
                            {
                                dtDummy.Columns.Remove("Z4");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z5"].ColumnName == "Z5")
                            {
                                dtDummy.Columns.Remove("Z5");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z6"].ColumnName == "Z6")
                            {
                                dtDummy.Columns.Remove("Z6");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y1"].ColumnName == "Y1")
                            {
                                dtDummy.Columns.Remove("Y1");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y2"].ColumnName == "Y2")
                            {
                                dtDummy.Columns.Remove("Y2");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y3"].ColumnName == "Y3")
                            {
                                dtDummy.Columns.Remove("Y3");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y4"].ColumnName == "Y4")
                            {
                                dtDummy.Columns.Remove("Y4");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y5"].ColumnName == "Y5")
                            {
                                dtDummy.Columns.Remove("Y5");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y6"].ColumnName == "Y6")
                            {
                                dtDummy.Columns.Remove("Y6");
                            }
                        }
                        catch { }
                        Session["send_dt"] = dtDummy;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevelJS(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    #endregion
                    break;

                case "F70225":
                    // PURCHASE REGISTER
                    #region Purchase Register
                    header_n = "Purchase Register";
                    dtDummy = new DataTable();
                    dtDummy.Columns.Add("Vchno", typeof(string));
                    dtDummy.Columns.Add("Vchdate", typeof(string));
                    //dtDummy.Columns.Add("vdd", typeof(string));
                    dtDummy.Columns.Add("Acode", typeof(string));
                    dtDummy.Columns.Add("Aname", typeof(string));
                    dtDummy.Columns.Add("MRRNO", typeof(string));
                    dtDummy.Columns.Add("MRRDT", typeof(string));
                    dtDummy.Columns.Add("INVNO", typeof(string));
                    dtDummy.Columns.Add("INVDT", typeof(string));

                    dtDummy.Columns.Add("Z1", typeof(double));
                    dtDummy.Columns.Add("Z2", typeof(double));
                    dtDummy.Columns.Add("Z3", typeof(double));
                    dtDummy.Columns.Add("Z4", typeof(double));
                    dtDummy.Columns.Add("Z5", typeof(double));
                    dtDummy.Columns.Add("Z6", typeof(double));
                    mq0 = "";

                    frm_vty = hfcode.Value;
                    // mq0 sir = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type like '5%' and a.vchdate " + xprdRange + " and a.dramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1) >='2' ";
                    //old     mq0 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname,A.MRNNUM,TO_CHAR(A.MRNDATE,'DD/MM/YYYY') AS MRNDATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type like '5%' and a.vchdate " + xprdRange + " and a.dramt>0 group by a.acode,b.aname,A.INVNO,A.INVDATE,A.MRNNUM,A.MRNDATE) order by drtot desc)a where substr(acode,1,1) >='2' ";
                    // LAST ORIGINAL QUERY  mq0 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type ='"+frm_vty+"' and a.vchdate " + xprdRange + " and a.dramt>0 group by a.acode,b.aname) order by drtot desc)a where substr(acode,1,1) >='2' ";
                    mq0 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and  a.type ='" + frm_vty + "' and a.vchdate " + xprdrange + " and a.dramt>0 group by a.acode,b.aname) order by drtot desc)a where substr(acode,1,1) IN ('1','3') AND SUBSTR(ACODE,1,2)!='17'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    m = 8;
                    for (int k = 0; k < 6; k++)
                    {
                        try
                        {
                            dtDummy.Columns[m].ColumnName = "Z" + dt.Rows[k]["acode"].ToString().Trim();
                            m++;
                        }
                        catch { }
                    }

                    dtDummy.Columns.Add("ZOthers", typeof(double));
                    dtDummy.Columns.Add("Y1", typeof(double));
                    dtDummy.Columns.Add("Y2", typeof(double));
                    dtDummy.Columns.Add("Y3", typeof(double));
                    dtDummy.Columns.Add("Y4", typeof(double));
                    dtDummy.Columns.Add("Y5", typeof(double));
                    dtDummy.Columns.Add("Y6", typeof(double));

                    mq1 = "";
                    //mq1 sir = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type like '5%' and a.vchdate " + xprdRange + " and a.dramt>0 group by a.acode,b.aname) order by crtot desc)a where substr(acode,1,1) >='2'";
                    //old  mq1 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,A.MRNNUM,TO_CHAR(A.MRNDATE,'DD/MM/YYYY') AS MRNDATE ,A. INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type like '5%' and a.vchdate " + xprdRange + " and a.dramt>0 group by a.acode,b.aname,A.INVNO,A.INVDATE,MRNNUM,MRNDATE) order by drtot desc)a where substr(acode,1,2) ='07'";
                    // LAST RUNNING QUERY mq1 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type ='" + frm_vty + "' and a.vchdate " + xprdRange + " and a.dramt>0 group by a.acode,b.aname) order by drtot desc)a where substr(acode,1,2) ='07'";
                    mq1 = "select rownum,a.* from (select acode,aname,drtot,crtot from (select sum(a.dramt) As drtot,sum(A.cramt) as crtot,a.acode,b.aname from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and  a.type ='" + frm_vty + "' and a.vchdate " + xprdrange + " and a.dramt>0 group by a.acode,b.aname) order by drtot desc)a where substr(acode,1,2) IN ('07','17')";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);
                    m = 15;
                    for (int k = 0; k < 6; k++)
                    {
                        try
                        {
                            dtDummy.Columns[m].ColumnName = "Y" + dt1.Rows[k]["acode"].ToString().Trim();
                            m++;
                        }
                        catch { }
                    }

                    dtDummy.Columns.Add("YOthers", typeof(double));
                    dtDummy.Columns.Add("TOT", typeof(double));

                    // LAST ORIGINAL QUERY  mq2 = "select sum(a.dramt)-sum(A.cramt) as crtot, A. INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,A.MRNNUM,to_char(A.MRNDATE,'DD/MM/YYYY') AS MRNDATE ,a.acode,b.aname,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and  a.type ='" + frm_vty + "' and a.vchdate " + xprdRange + " and a.dramt>0 and  substr(a.acode,1,2) not in ('16') group by a.acode,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),A.MRNNUM,A.MRNDATE,b.aname,to_char(a.vchdate,'yyyymmdd') ,A.INVNO,A.INVDATE order by vchnum";
                    mq2 = "select sum(a.dramt)-sum(A.cramt) as crtot, A. INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,A.MRNNUM,to_char(A.MRNDATE,'DD/MM/YYYY') AS MRNDATE ,a.acode,b.aname,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.rcode)=trim(b.acode) and a.branchcd='" + mbr + "' and  a.type ='" + frm_vty + "' and a.vchdate " + xprdrange + " and  substr(a.acode,1,2) not in ('16','06','05') group by a.acode,a.rcode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),A.MRNNUM,A.MRNDATE,b.aname,to_char(a.vchdate,'yyyymmdd') ,A.INVNO,A.INVDATE order by vchnum";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    if (dt2.Rows.Count > 0)
                    {
                        DataView view1 = new DataView(dt2);
                        dt4 = new DataTable();
                        dt4 = view1.ToTable(true, "vchnum", "vchdate", "rcode");
                        dr1 = null;
                        foreach (DataRow dr2 in dt4.Rows)
                        {
                            DataView view2 = new DataView(dt2, "vchnum='" + dr2["vchnum"].ToString().Trim() + "' and vchdate='" + dr2["vchdate"].ToString().Trim() + "' and rcode='" + dr2["rcode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt5 = new DataTable();
                            dt5 = view2.ToTable();

                            dr1 = dtDummy.NewRow();
                            db1 = 0; db2 = 0; db3 = 0;
                            for (int i = 0; i < dt5.Rows.Count; i++)
                            {
                                dr1["Vchno"] = dt5.Rows[i]["vchnum"].ToString();
                                dr1["vchdate"] = dt5.Rows[i]["vchdate"].ToString();
                                // dr1["vdd"] = dt5.Rows[i]["vdd"].ToString();
                                dr1["acode"] = dt5.Rows[i]["rcode"].ToString();
                                dr1["aname"] = dt5.Rows[i]["aname"].ToString();
                                dr1["MRRNO"] = dt5.Rows[i]["MRNNUM"].ToString();
                                dr1["MRRDT"] = dt5.Rows[i]["MRNDATE"].ToString();
                                dr1["INVNO"] = dt5.Rows[i]["INVNO"].ToString();
                                dr1["INVDT"] = dt5.Rows[i]["INVDATE"].ToString();

                                mq4 = dt5.Rows[i]["acode"].ToString().Trim().Substring(0, 1);
                                mq5 = dt5.Rows[i]["acode"].ToString().Trim().Substring(0, 2);
                                //ORIGINAL COND if (mq4.Contains("2") || mq4.Contains("3"))
                                if (mq5 != "07" && mq5 != "17")
                                {
                                    try
                                    {
                                        dr1["Z" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                    }
                                    catch
                                    {
                                        db1 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                        dr1["ZOthers"] = db1;
                                    }
                                }
                                else if (mq5.Contains("07") || mq5.Contains("17")) // ORIGINALLY THERE WAS ONLY ELSE CONDITION
                                {
                                    try
                                    {
                                        dr1["Y" + dt5.Rows[i]["acode"].ToString().Trim()] = fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                    }
                                    catch
                                    {
                                        db2 += fgen.make_double(dt5.Rows[i]["crtot"].ToString());
                                        dr1["YOthers"] = db2;
                                    }
                                }
                                db3 += fgen.make_double(dt5.Rows[i]["crtot"].ToString()); ;
                            }
                            dr1["tot"] = db3;
                            //dr1["fromdt"] = fromdt;
                            //dr1["todt"] = todt;                   
                            dtDummy.Rows.Add(dr1);
                        }
                    }
                    if (dtDummy.Rows.Count > 0)
                    {
                        dr1 = dtDummy.NewRow();
                        foreach (DataColumn dc in dtDummy.Columns)
                        {
                            db1 = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 7)
                            { }
                            else
                            {
                                mq4 = "sum(" + dc.ColumnName + ")";
                                db1 += fgen.make_double(dtDummy.Compute(mq4, "").ToString());
                                dr1[dc] = db1;
                            }
                        }
                        dr1[0] = "TOTAL";
                        dtDummy.Rows.InsertAt(dr1, 0);

                        dtDummy.Columns[8].ColumnName = "Z1"; // SALES
                        dtDummy.Columns[9].ColumnName = "Z2";
                        dtDummy.Columns[10].ColumnName = "Z3";
                        dtDummy.Columns[11].ColumnName = "Z4";
                        dtDummy.Columns[12].ColumnName = "Z5";
                        dtDummy.Columns[13].ColumnName = "Z6";

                        dtDummy.Columns[15].ColumnName = "Y1"; // TAX
                        dtDummy.Columns[16].ColumnName = "Y2";
                        dtDummy.Columns[17].ColumnName = "Y3";
                        dtDummy.Columns[18].ColumnName = "Y4";
                        dtDummy.Columns[19].ColumnName = "Y5";
                        dtDummy.Columns[20].ColumnName = "Y6";
                        dtDummy.Columns["ZOthers"].ColumnName = "Others";
                        dtDummy.Columns["YOthers"].ColumnName = "TaxOthers";
                        l = 1;
                        for (int k = 0; k < 6; k++)
                        {
                            try
                            {
                                dtDummy.Columns["Z" + l].ColumnName = dt.Rows[k]["aname"].ToString().Trim();
                            }
                            catch { }
                            l++;
                        }
                        l = 1;
                        for (int k = 0; k < 6; k++)
                        {
                            try
                            {
                                dtDummy.Columns["Y" + l].ColumnName = dt1.Rows[k]["aname"].ToString().Trim();
                            }
                            catch { }
                            l++;
                        }

                        // dtDummy.Columns.Remove("vdd");
                        //dtDummy.Columns.Remove("fromdt");
                        //dtDummy.Columns.Remove("todt");
                        try
                        {
                            if (dtDummy.Columns["Z1"].ColumnName == "Z1")
                            {
                                dtDummy.Columns.Remove("Z1");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z2"].ColumnName == "Z2")
                            {
                                dtDummy.Columns.Remove("Z2");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z3"].ColumnName == "Z3")
                            {
                                dtDummy.Columns.Remove("Z3");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z4"].ColumnName == "Z4")
                            {
                                dtDummy.Columns.Remove("Z4");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z5"].ColumnName == "Z5")
                            {
                                dtDummy.Columns.Remove("Z5");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Z6"].ColumnName == "Z6")
                            {
                                dtDummy.Columns.Remove("Z6");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y1"].ColumnName == "Y1")
                            {
                                dtDummy.Columns.Remove("Y1");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y2"].ColumnName == "Y2")
                            {
                                dtDummy.Columns.Remove("Y2");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y3"].ColumnName == "Y3")
                            {
                                dtDummy.Columns.Remove("Y3");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y4"].ColumnName == "Y4")
                            {
                                dtDummy.Columns.Remove("Y4");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y5"].ColumnName == "Y5")
                            {
                                dtDummy.Columns.Remove("Y5");
                            }
                        }
                        catch { }
                        try
                        {
                            if (dtDummy.Columns["Y6"].ColumnName == "Y6")
                            {
                                dtDummy.Columns.Remove("Y6");
                            }
                        }
                        catch { }
                        Session["send_dt"] = dtDummy;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevelJS(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    #endregion
                    break;

                case "F70227":
                    // NET SALES REPORT                    
                    SQuery = "select SUBSTR(A.TYPE,1,1) AS TYPE,B.ANAME,SUM(A.CRAMT) AS SALES_AMOUNT ,SUM(A.DRAMT) AS ADJ_RETURNS , SUM(A.CRAMT)- SUM(A.DRAMT) as Net_amt FROM VOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.ACODE LIKE '20%' AND A.VCHDATE " + xprdrange + " GROUP BY SUBSTR(A.TYPE,1,1),B.ANAME order by TYPE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Net Sales Report for the Period " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F70317":
                    // HSN SALES SUMMARY BY AKSHAY
                    fgen.drillQuery(0, "select trim(hscode) as fstr, '-' as gstr, TRIM(hscode) AS HSCODE, sum(turnovr_igst) as turnover_igst, sum(igst_rt) as igST_rate, sum(igst_amt) as igst_amt, sum(turnovr_cgst) as turnovr_cgst, sum(CGST_RT) as CGST_RATE, sum(CGST_amt) as CGST_amt,sum(SGST_rate) as SGST_rate, sum(SGST_amt) as sgst_amt from (select B.HSCODE,(Case when trim(A.IOPR)='IG' then a.iamount else 0 end) as turnovr_igst,(Case when trim(A.IOPR)='CG' then a.iamount else 0 end) as turnovr_cgst,(Case when trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt from ivoucher A , ITEM B  where TRIM(A.ICODE)=TRIM(B.ICODE) AND  a.branchcd='" + mbr + "' and a.type like '4%' and type!='47' and a.vchdate " + xprdrange + " and a.vchdate>=to_date('01/07/2017','dd/mm/yyyy')) group by hscode", frm_qstr);
                    fgen.drillQuery(1, "select trim(a.hscode)||trim(a.acode) as fstr, trim(a.hscode) as gstr, TRIM(a.hscode) AS HSCODE, b.aname,TRIM(a.acode) AS ACODE,TRIM(b.gst_no) as gstIn,sum(a.turnovr_igst) as turnover_igst, sum(a.igst_rt) as igST_rate, sum(a.igst_amt) as igst_amt, sum(a.turnovr_cgst) as turnovr_cgst, sum(a.CGST_RT) as CGST_RATE, sum(a.CGST_amt) as CGST_amt,sum(a.SGST_rate) as SGST_rate, sum(a.SGST_amt) as sgst_amt from (select B.HSCODE,a.acode,(Case when trim(A.IOPR)='IG' then a.iamount else 0 end) as turnovr_igst,(Case when trim(A.IOPR)='CG' then a.iamount else 0 end) as turnovr_cgst,(Case when trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt from ivoucher A , ITEM B  where TRIM(A.ICODE)=TRIM(B.ICODE) AND  a.branchcd='" + mbr + "' and a.type like '4%' and type!='47' and a.vchdate " + xprdrange + " and a.vchdate>=to_date('01/07/2017','dd/mm/yyyy')) a, famst b where trim(a.acode)=trim(b.acode) group by a.hscode,b.aname,a.acode,b.gst_no order by aname,acode", frm_qstr);
                    fgen.drillQuery(2, "select trim(a.acode)||trim(a.vchnum)||a.vchdate as fstr,trim(a.hscode)||trim(a.acode) as gstr, TRIM(a.hscode) AS HSCODE, b.aname,TRIM(a.acode) AS ACODE,TRIM(a.vchnum) AS VCHNUM,a.vchdate,TRIM(b.gst_no) as gstIn,sum(a.turnovr_igst) as turnover_igst, sum(a.igst_rt) as igST_rate, sum(a.igst_amt) as igst_amt, sum(a.turnovr_cgst) as turnovr_cgst, sum(a.CGST_RT) as CGST_RATE, sum(a.CGST_amt) as CGST_amt,sum(a.SGST_rate) as SGST_rate, sum(a.SGST_amt) as sgst_amt from (select B.HSCODE,a.acode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,(Case when trim(A.IOPR)='IG' then a.iamount else 0 end) as turnovr_igst,(Case when trim(A.IOPR)='CG' then a.iamount else 0 end) as turnovr_cgst,(Case when trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt  from ivoucher A , ITEM B  where TRIM(A.ICODE)=TRIM(B.ICODE) AND  a.branchcd='" + mbr + "' and a.type like '4%' and type!='47' and a.vchdate " + xprdrange + " and a.vchdate>=to_date('01/07/2017','dd/mm/yyyy') ) a, famst b where trim(a.acode)=trim(b.acode) group by a.hscode,b.aname,a.acode,b.gst_no,a.vchnum,a.vchdate order by aname,acode", frm_qstr);
                    fgen.Fn_DrillReport("HSN Sales Summary For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F70318":
                    // SALES SUMMARY BY AKSHAY
                    fgen.drillQuery(0, "SELECT trim(a.acode) as fstr, '-' as gstr, a.branchcd,a.acode,B.ANAME,SUM(A.SALES) AS SALES,SUM(A.DEBIT_NOTE) AS DEBIT, SUM(A.CREDIT_NOTE) AS CREDIT ,((SUM(A.SALES)+SUM(A.DEBIT_NOTE))-SUM(A.CREDIT_NOTE)) AS TOTAL FROM (select branchcd,acode,sum(dramt) as sales, 0 as debit_note , 0 as credit_note from voucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd ,acode UNION ALL select branchcd,acode,0 as sales, sum(dramt) as debit_note , 0 as credit_note  from voucher where branchcd='" + mbr + "' and type ='59' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode UNION ALL select branchcd, acode, 0 as sales,0 as debit_note , sum(cramt) as credit_note from voucher where branchcd='" + mbr + "' and type ='58' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode) A , FAMST B WHERE TRIM(A.ACODE) =TRIM(B.ACODE) GROUP BY A.ACODE,B.ANAME,a.branchcd ORDER BY acode,ANAME", frm_qstr);
                    fgen.drillQuery(1, "SELECT trim(a.acode)||trim(a.type) as fstr, a.acode as gstr ,a.branchcd,trim(A.acode) as acode,a.type,B.ANAME, SUM(A.SALES) AS SALES,SUM(A.DEBIT_NOTE) AS DEBIT, SUM(A.CREDIT_NOTE) AS CREDIT ,((SUM(A.SALES)+SUM(A.DEBIT_NOTE))-SUM(A.CREDIT_NOTE)) AS TOTAL  FROM (select branchcd,acode,type,sum(dramt) as sales,vchnum ,to_char(vchdate,'dd/mm/yyyy') as vchdate, 0 as debit_note , 0 as credit_note from voucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode,type,vchnum,to_char(vchdate,'dd/mm/yyyy') UNION ALL select branchcd,acode,type, 0 as sales, vchnum ,to_char(vchdate,'dd/mm/yyyy')as vchdate, sum(dramt) as debit_note , 0 as credit_note  from voucher where branchcd='" + mbr + "' and type ='59' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode, type,vchnum,to_char(vchDATE,'dd/mm/yyyy') UNION ALL select branchcd,acode, type,0 as sales, vchnum , to_char(vchDATE,'dd/mm/yyyy') as vchdate,0 as debit_note , sum(cramt) as credit_note from voucher where branchcd='" + mbr + "' and type ='58' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode,type,vchnum,to_char(VchDATE,'dd/mm/yyyy')) A , FAMST B WHERE TRIM(A.ACODE) =TRIM(B.ACODE) GROUP BY a.branchcd,A.ACODE,B.ANAME,a.type ORDER BY ANAME,type", frm_qstr);
                    fgen.drillQuery(2, "SELECT TRIM(A.TYPE)||TRIM(A.VCHNUM)||TRIM(A.VCHDATE) AS FSTR, trim(a.acode)||TRIM(A.TYPE) AS GSTR ,a.branchcd,A.acode,A.rcode,a.type,B.ANAME,A.vchnum,A.vchDATE, SUM(A.SALES) AS SALES,SUM(A.DEBIT_NOTE) AS DEBIT, SUM(A.CREDIT_NOTE) AS CREDIT ,((SUM(A.SALES)+SUM(A.DEBIT_NOTE))-SUM(A.CREDIT_NOTE)) AS TOTAL  FROM (select branchcd,acode,rcode,type,sum(dramt) as sales,vchnum ,to_char(vchdate,'dd/mm/yyyy') as vchdate, 0 as debit_note , 0 as credit_note from voucher where branchcd='" + mbr + "' and type like '4%' and type!='47' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode, rcode,type,vchnum,to_char(vchdate,'dd/mm/yyyy') UNION ALL select branchcd,acode,rcode,type, 0 as sales, vchnum ,to_char(vchdate,'dd/mm/yyyy')as vchdate, sum(dramt) as debit_note , 0 as credit_note  from voucher where branchcd='" + mbr + "' and type ='59' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode, rcode,type,vchnum,to_char(vchDATE,'dd/mm/yyyy') UNION ALL select branchcd,acode,rcode, type,0 as sales, vchnum , to_char(vchDATE,'dd/mm/yyyy') as vchdate,0 as debit_note , sum(cramt) as credit_note from voucher where branchcd='" + mbr + "' and type ='58' and vchdate " + xprdrange + " and substr(rcode,1,2)='20' group by branchcd,acode, rcode,type,vchnum,to_char(VchDATE,'dd/mm/yyyy')) A , FAMST B WHERE TRIM(A.ACODE) =TRIM(B.ACODE)  GROUP BY a.branchcd,A.ACODE, A.RCODE,A.vchnum,A.vchDATE,B.ANAME,a.type ORDER BY ANAME,type", frm_qstr);
                    fgen.Fn_DrillReport("Sales Summary For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F70556":
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    if (part_cd.Length <= 1) part_cd = "substr(acode,1,4) like '%'";
                    else part_cd = "substr(acode,1,4) in (" + part_cd + ")";
                    if (party_cd.Length <= 2) party_cd = "and substr(acode,1,2) like '%'";
                    else party_cd = "and substr(acode,1,2) in (" + party_cd + ")";
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    SQuery = "SELECT ACODE AS FSTR,'-' AS GSTR,ANAME,ACODE,ADDR1,ADDR2,GRP,MKTGGRP,PNAME,TELNUM,PERSON,STATEN,DISTRICT,MOBILE,EMAIL FROM FAMST where " + part_cd + " " + party_cd + " ORDER BY ANAME,ACODE";
                    fgen.drillQuery(0, SQuery, frm_qstr);
                    SQuery1 = "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS DEBITS,SUM(CRAMT) AS CREDITS,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprd2 + " AND ACODE='FSTR' ) GROUP BY FSTR,MTHNAME order by srno";
                    fgen.drillQuery(1, SQuery1, frm_qstr);
                    SQuery2 = "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.DRAMT) AS DEBIT,(a.CRAMT) AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.TAX,A.STAX,A.ST_ENTFORM,A.BRANCHCD PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprd2 + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'";
                    fgen.drillQuery(2, SQuery2, frm_qstr);
                    cond = "";
                    if (hfbr.Value == "ABR") cond = "(Consolidated)";
                    else cond = "Branch Wise(" + mbr + ")";
                    fgen.Fn_DrillReport("Ledger Account " + cond + " for the period from " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F70134"://j.v. reg 
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select distinct a.branchcd||trim(a.acode)||trim(a.type) as fstr,'-' as gstr, trim(a.acode) as acode,b.aname ,sum(a.dramt) as dr_Amt,sum(a.cramt) as cr_Amt,a.ent_by from voucher a,famst b where trim(a.acodE)=trim(b.acodE) and a.branchcd='" + mbr + "' and  type='30' and a.vchdate " + xprdrange + " group by trim(a.acode),b.aname,a.branchcd,a.branchcd||trim(a.acode)||trim(a.type),A.ENT_BY order by acode", frm_qstr);
                    fgen.drillQuery(1, "select distinct '-' as fstr,a.branchcd||trim(a.acode)||trim(a.type) as gstr, a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.acode) as acode,b.aname ,trim(a.rcode) as Rcode,c.aname as RNAME ,sum(a.dramt) as dr_Amt,sum(a.cramt) as cr_Amt,a.ent_by from voucher a,famst b,famst c where trim(a.acodE)=trim(b.acodE) and trim(a.rcode)=trim(c.acode) and a.branchcd='" + mbr + "' and  type='30' and a.vchdate " + xprdrange + "  group by trim(a.acode),b.aname,c.aname,trim(a.rcode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.type,a.ent_by,a.branchcd order by a.type,a.vchnum,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("J.V.Register For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F70135"://PURCHASE reg 
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select distinct a.branchcd||trim(a.acode)||trim(a.type) as fstr,'-' as gstr, trim(a.acode) as acode,b.aname ,sum(a.dramt) as dr_Amt,sum(a.cramt) as cr_Amt, A.ENT_BY from voucher a,famst b where trim(a.acodE)=trim(b.acodE) and a.branchcd='" + mbr + "' and  type LIKE '5%' and a.vchdate " + xprdrange + " group by  trim(a.acode),b.aname,a.branchcd,a.branchcd||trim(a.acode)||trim(a.type),A.ENT_BY order by acode", frm_qstr);
                    fgen.drillQuery(1, "select distinct '-' as fstr,a.branchcd||trim(a.acode)||trim(a.type) as gstr, a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.acode) as acode,b.aname ,trim(a.rcode) as Rcode,c.aname as RNAME ,sum(a.dramt) as dr_Amt,sum(a.cramt) as cr_Amt,a.ent_by from voucher a,famst b,famst c where trim(a.acodE)=trim(b.acodE) and trim(a.rcode)=trim(c.acode) and a.branchcd='" + mbr + "' and  type LIKE '5%' and a.vchdate " + xprdrange + "  group by trim(a.acode),b.aname,c.aname,trim(a.rcode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.type,a.ent_by,a.branchcd order by a.type,a.vchnum,vchdate", frm_qstr);
                    fgen.Fn_DrillReport("Purchase Register For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "70422":
                    // asset tie up report mg 19.08.18
                    fgen.drillQuery(0, "Select a.vchnum as Scan_No,to_char(a.vchdate,'dd/mm/yyyy') as Scan_Dt,a.asset_id as Asset_code from ast_reco a where a.branchcd='" + mbr + "' and a.type='10' and a.vchdate " + xprdrange + "  order by a.vchdate,a.vchnum", frm_qstr);
                    fgen.drillQuery(0, "Select a.vchnum as Scan_No,to_char(a.vchdate,'dd/mm/yyyy') as Scan_Dt,a.asset_id as Asset_code from ast_reco a where a.branchcd='" + mbr + "' and a.type='10' and a.vchdate " + xprdrange + "  order by a.vchdate,a.vchnum", frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Asset Tie up Report-assets scanned for Reco From " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F70503": //PV MRR Tie up Report
                    mq1 = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode) as fstr,nvl(depcd,'-') as gst_class from voucher where type in ('50','51','52','53','54','55','56') and vchdate > = to_date('01/07/2018','dd/mm/yyyy')";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            fgen.execute_cmd(frm_qstr, co_cd, "UPDATE SCRATCH a SET col39='" + dt.Rows[i]["gst_class"].ToString().Trim() + "' WHERE trim(a.branchcd)||trim(a.col32)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)='" + dt.Rows[i]["fstr"].ToString().Trim() + "'");
                        }
                    }
                    SQuery = "select trim(b.name) as plant,trim(a.col32) as Voucher_type, to_char(a.vchdate,'dd/mm/yyyy') as voucher_dt, substr(trim(nvl(col27,'-')),1,6) as MRR_No,substr(trim(nvl(col27,'-')),7,10) as MRR_dt,trim(nvl(col33,'-')) as Inv_dt, trim(nvl(col25,'-')) as Inv_No,trim(c.aname) as Supplier_Name,trim(c.gst_no) as Supplier_GSTIN, trim(c.staten) as Supplier_State, trim(a.icode) as icode , trim(nvl(a.col1,'-')) as Item_name, trim(d.hscode) as HSN_Code, trim(nvl(a.col2,0)) as Recd_Qty, trim(nvl(a.col7,0)) as Recd_Wt, trim(a.col8) as PO_Type, trim(d.unit) as UOM,trim(nvl(a.col3,0)) as Pass_Rate, trim(nvl(a.col4,0)) as Inventory_rate,trim(nvl(a.col6,0)) as Basic_Rate,(case when trim(nvl(a.col32,0))='56' then nvl(a.num3,0) else 0 end)  as Import_Taxable_Value,(case when substr(b.gst_no,1,2)= trim(c.staffcd) then (nvl(a.num5,0)) else 0 end) as CGST_Rate,(case when substr(b.gst_no,1,2)= trim(c.staffcd) then (nvl(a.num6,0)) else 0 end) as CGST_Amt ,(case when substr(b.gst_no,1,2)= trim(c.staffcd) then (nvl(a.num5,0)) else 0 end)as SGST_Rate, (case when substr(b.gst_no,1,2)= trim(c.staffcd) then (nvl(a.num6,0)) else 0 end)as SGST_Amt, (case when substr(b.gst_no,1,2)= trim(c.staffcd) then 0 else nvl(a.num5,0) end) as IGST_Rate,(case when substr(b.gst_no,1,2)= trim(c.staffcd) then 0 else a.num6 end) as IGST_Amt,trim(nvl(a.col39,'-')) as GST_Class from scratch a,type b,famst c, item d where a.branchcd='" + mbr + "' and a.type='VC' and a.vchdate " + xprdrange + "  and b.id='B' and trim(a.branchcd)=trim(b.type1) and trim(a.icode)=trim(d.icode)  and trim(a.acode)=trim(c.acode) and c.staffcd is not null and nvl(trim(a.col27),'-')!='-'  order by mrr_no desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("PV MRR Tie up Report From " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F70273": // MADE ON 25.8.18
                    header_n = "Net Purchase Report";
                    SQuery = "select SUBSTR(A.TYPE,1,1) AS TYPE,TRIM(C.NAME) AS NAME,TRIM(B.ANAME) AS ANAME,SUM(NVL(A.dRAMT,0)) AS CRamt ,SUM(NVL(A.CRAMT,0)) AS DRamt ,SUM(NVL(A.dRAMT,0))- SUM(NVL(A.cRAMT,0)) as Net_amt FROM VOUCHER A, FAMST B , TYPE C  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.BRANCHCD)=TRIM(C.TYPE1) AND A.BRANCHCD='" + mbr + "' AND A.ACODE LIKE '30%' and c.id='B' AND A.VCHDATE " + xprdrange + " GROUP BY SUBSTR(A.TYPE,1,1),B.ANAME,C.NAME order by TYPE,aname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Net Purchase Report  From " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F70275":
                    header_n = "Month Wise Sale Summary";
                    SQuery = "select TO_CHAR(A.VCHDATE,'MONTH') AS MONTH_NAME,trim(c.acode) as acode,trim(c.aname) as aname,trim(A.ICODE) as icode,trim(b.iname) as iname,trim(b.cpartno) as partno,trim(b.unit) as unit,sum(nvl(A.IQTYOUT,0)) AS QTY,sum(nvl(A.IAMOUNT,0)) AS BASIC,TO_CHAR(A.VCHDATE,'yyyyMM') AS vdd FROM IVOUCHER A,ITEM B ,FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE  " + xprdrange + " group by TO_CHAR(A.VCHDATE,'MONTH') ,trim(c.aname),trim(b.iname),trim(c.acode),trim(b.cpartno),trim(b.unit),TO_CHAR(A.VCHDATE,'yyyyMM'),trim(A.ICODE) ORDER BY vdd,ANAME,INAME";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70276"://Customer wise, Item wise Sales Summary
                    header_n = "Customer Part wise Sales Summary";
                    SQuery = "select C.MKTGGRP ,trim(a.acode) as acode,trim(C.ANAME) as cust_name,trim(a.icode) as item_code,trim(B.INAME) as item_name,trim(B.UNIT) as unit,nvl(B.CPARTNO,'-') as partno ,SUM(nvl(A.IQTYOUT,0)) AS QTY ,SUM(nvl(A.IAMOUNT,0)) AS BASIC FROM IVOUCHER A, ITEM B,FAMST C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND A.VCHDATE " + xprdrange + " GROUP BY C.MKTGGRP,trim(C.ANAME) ,trim(a.icode) ,trim(B.INAME),trim(a.acode),trim(C.ANAME),trim(B.UNIT),nvl(B.CPARTNO,'-') ORDER BY cust_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70277":
                    header_n = "Customer wise Sales Summary";
                    SQuery = "select trim(a.acode) as cust_code,trim(c.ANAME) as cust_name,C.MKTGGRP ,sum(nvl(A.IQTYOUT,0)) as qty, sum(nvl(A.IAMOUNT,0)) as basic FROM IVOUCHER A ,FAMST C WHERE TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdrange + " group by trim(a.acode),trim(c.aname) ,C.MKTGGRP ORDER BY cust_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70278"://Customer Part wise Sales Summary
                    #region Cust part Wise Sale Report
                    header_n = "Cust part Wise Sale Report";
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("MKT_GRP", typeof(string));
                    ph_tbl.Columns.Add("CUST_CODE", typeof(string));
                    ph_tbl.Columns.Add("CUST_NAME", typeof(string));
                    ph_tbl.Columns.Add("PART_NO", typeof(string));
                    ph_tbl.Columns.Add("QUANTITY", typeof(double));
                    ph_tbl.Columns.Add("VALUE", typeof(double));

                    SQuery = "select  C.MKTGGRP ,trim(B.INAME) as iname,trim(a.acode) as acode,trim(C.ANAME) as aname,B.UNIT,trim(B.CPARTNO) as partno,SUM(nvl(A.IQTYOUT,0)) AS QTY ,SUM(nvl(A.IAMOUNT,0)) AS BASIC FROM IVOUCHER A, ITEM B,FAMST C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + mbr + "' AND TYPE LIKE '4%' AND A.VCHDATE " + xprdrange + " GROUP BY trim(B.INAME),trim(C.ANAME),B.UNIT,trim(B.CPARTNO),C.MKTGGRP,trim(a.acode)  ORDER BY aname";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt2.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt2);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "ACODE"); //MAIN                  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt2, "ACODE='" + dr0["ACODE"] + "'", "", DataViewRowState.CurrentRows);
                            dt1 = new DataTable();
                            dt1 = viewim.ToTable();
                            mq1 = "";
                            mq1 = ""; db = 0; db1 = 0; db2 = 0; db6 = 0; db7 = 0;
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                dr1 = ph_tbl.NewRow();
                                dr1["MKT_GRP"] = dt1.Rows[i]["MKTGGRP"].ToString().Trim();
                                dr1["CUST_CODE"] = dt1.Rows[i]["acode"].ToString().Trim();
                                dr1["CUST_NAME"] = dt1.Rows[i]["aname"].ToString().Trim();
                                dr1["PART_NO"] = dt1.Rows[i]["partno"].ToString().Trim();
                                dr1["QUANTITY"] = dt1.Rows[i]["QTY"].ToString().Trim();
                                db += fgen.make_double(dt1.Rows[i]["QTY"].ToString().Trim());
                                dr1["VALUE"] = dt1.Rows[i]["BASIC"].ToString().Trim();
                                db1 += fgen.make_double(dt1.Rows[i]["BASIC"].ToString().Trim());
                                ph_tbl.Rows.Add(dr1);
                            }
                            dr1 = ph_tbl.NewRow();
                            dr1["PART_NO"] = "Total";
                            dr1["QUANTITY"] = db;
                            dr1["VALUE"] = db1;
                            ph_tbl.Rows.Add(dr1);
                        }
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = ph_tbl;
                    fgen.Fn_open_rptlevelJS(header_n + " From " + fromdt + " To " + todt + "", frm_qstr);
                    #endregion
                    break;

                case "F70279":
                    header_n = "Item Wise Sale Summary";
                    SQuery = "select B.MKTGGRP ,trim(a.icode) as item_code,trim(c.iname) as item_name,trim(nvl(c.cpartno,'-')) as partno,trim(c.unit) as unit,sum(nvl(A.IQTYOUT,0)) as qty, sum(nvl(A.IAMOUNT,0)) as value FROM IVOUCHER A ,item C,famst b WHERE TRIM(A.iCODE) = TRIM(C.iCODE) AND TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdrange + " AND B.MKTGGRP='-' group by trim(a.icode),trim(c.iname),B.MKTGGRP,trim(nvl(c.cpartno,'-')),trim(c.unit) ORDER BY item_name";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70247"://voucher list 
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    fgen.drillQuery(0, "select  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,'-' as gstr,a.vchnum as voucher,to_char(a.vchdate,'dd/mm/yyyy') as dated,sum(a.dramt) as debit,sum(a.cramt) as credits,a.type,a.branchcd from voucher a where a.branchcd='" + mbr + "' and a.type='" + hfcode.Value + "' and a.vchdate " + xprdrange + "  group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.type,a.branchcd,trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') order by voucher", frm_qstr);
                    fgen.drillQuery(1, "select '-' as fstr, trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as gstr,b.aname as particulars,a.dramt as debit,a.cramt as credits,a.naration,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchddate,a.branchcd,a.refnum  from voucher a,famst b where trim(a.acode)=trim(b.acode)", frm_qstr);
                    fgen.Fn_DrillReport("Vouchers' List For the Period " + value1 + " To " + value2 + "", frm_qstr);
                    break;

                case "F70426":
                    SQuery = "SELECT  vchnum as Insurance_doc_No, to_char(vchdate,'dd/MM/yyyy') as Insurance_doc_Date, grpcode as GroupCode, acode As Code, ASSETVAL AS INSURANCE_VALUE,ASSETVAL1 AS INSURANCE_PREMIUM,to_char(instdt,'dd/mm/yyyy') as inst_date,mrr_ref as policy_no ,to_char(life_end,'dd/mm/yyyy') as renewal_date,invno as Invoice_No,to_char(invdate,'dd/MM/yyyy') as Invoice_Date, ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,naration as remarks FROM wb_fa_vch where branchcd='" + mbr + "' and type='11' and instdt " + xprdrange + " order by vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Asset Verified Report", frm_qstr);
                    break;

                case "F70504xxx": //FOR FORMAT FOR THIS REPORT SEE IN MAILID AND CLIENT UNIP
                    #region
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("fstr", typeof(string));  //A
                    ph_tbl.Columns.Add("gstr", typeof(string));  //A
                    ph_tbl.Columns.Add("unit", typeof(string));  //A
                    ph_tbl.Columns.Add("bname", typeof(string));  //A
                    ph_tbl.Columns.Add("Sale_Domestic_40", typeof(string)); //B
                    ph_tbl.Columns.Add("Sale_Input_4A", typeof(string)); //C
                    ph_tbl.Columns.Add("Sale_Pur_retrn_47", typeof(string)); //D
                    ph_tbl.Columns.Add("Sale_Scrap_45", typeof(string));  //E
                    ph_tbl.Columns.Add("capital_sale_4G", typeof(string)); //F
                    ph_tbl.Columns.Add("Total_Sale", typeof(string)); //G
                    ph_tbl.Columns.Add("Inter_Unit_outward_FG", typeof(string));  //H
                    ph_tbl.Columns.Add("Inter_Unit_outward_semi", typeof(string));  //I
                    ph_tbl.Columns.Add("Inter_Unit_outward_RM", typeof(string));  //J
                    ph_tbl.Columns.Add("Inter_Unit_outward_SALE", typeof(string));  //K
                    ph_tbl.Columns.Add("Total_4B_Sale", typeof(string));  //L                 
                    ph_tbl.Columns.Add("all_sale", typeof(string));  //M
                    //////PURCHASE STARTS
                    ph_tbl.Columns.Add("Purchase", typeof(string));  //N
                    ph_tbl.Columns.Add("Pur_Packing", typeof(string));  //O
                    ph_tbl.Columns.Add("Pur_Manuf_Cost", typeof(string));  //P
                    ph_tbl.Columns.Add("Pur_Fixed_assets", typeof(string));  //Q
                    ph_tbl.Columns.Add("Pur_Other_exp", typeof(string));  //R
                    ph_tbl.Columns.Add("Pur_TOTAL", typeof(string));  //TOTAL
                    ph_tbl.Columns.Add("Inter_Unit_inward_FG", typeof(string));  //S
                    ph_tbl.Columns.Add("Inter_Unit_inward_semi", typeof(string));  //T
                    ph_tbl.Columns.Add("Inter_Unit_inward_RM", typeof(string));  //U
                    ph_tbl.Columns.Add("Other_Pur", typeof(string));  //V
                    ph_tbl.Columns.Add("Total_IUTI", typeof(string));  //W
                    ph_tbl.Columns.Add("net_sale", typeof(string));  //X
                    ph_tbl.Columns.Add("net_pur", typeof(string));  //Y
                    ph_tbl.Columns.Add("sale_pur_ratio_a", typeof(string));  //Z
                    ph_tbl.Columns.Add("sale_pur_ratio_b", typeof(string));  //AB

                    #region
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable(); dt7 = new DataTable(); dt8 = new DataTable();
                    dt9 = new DataTable(); dt10 = new DataTable(); dt11 = new DataTable(); dt12 = new DataTable(); dt13 = new DataTable(); dt14 = new DataTable(); dt15 = new DataTable();
                    //GROSS SALE FROM SALE TABLE
                    mq0 = "select branchcd,sum(IAMOUNT) as sale40 from ivoucher where type='40' and vchdate " + xprdrange + "  AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    mq1 = "select branchcd,sum(IAMOUNT) as sale4A from ivoucher where type='4A' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    mq2 = "select branchcd,sum(IAMOUNT) as sale47 from ivoucher where type='47' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "select branchcd,sum(IAMOUNT) as sale45 from ivoucher where type='45' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    mq4 = "select branchcd,sum(IAMOUNT) as sale4G from ivoucher where type='4G' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);
                    //======================================                
                    mq5 = "SELECT BRANCHCD,SUM(IAMOUNT) AS FG_SALE  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate  " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,2) NOT IN ('95','96') GROUP BY BRANCHCD";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq5);

                    mq19 = "SELECT BRANCHCD,SUM(IAMOUNT) AS FG_SALE1  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,4)  IN ('9108','9004','9205','9304','9403','9112') GROUP BY BRANCHCD";
                    dt19 = fgen.getdata(frm_qstr, co_cd, mq19);

                    //////====================================
                    mq6 = "SELECT BRANCHCD,SUM(IAMOUNT) AS SEMI_SALE  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate  " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,2)  IN ('95','96') GROUP BY BRANCHCD";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq6);

                    mq20 = "SELECT BRANCHCD,SUM(IAMOUNT) AS SEMI_SALE1  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,4)  IN ('9108','9004','9205','9304','9403','9112') GROUP BY BRANCHCD";
                    dt20 = fgen.getdata(frm_qstr, co_cd, mq20);
                    ////////========================================
                    mq7 = "SELECT BRANCHCD,SUM(IAMOUNT) AS RM_sale FROM IVOUCHER WHERE TYPE IN ('4B','29') AND VCHDATE " + xprdrange + "  AND SUBSTR(TRIM(ICODE),1,2) IN ('10','11','12','13','14','15','16','17','18','28','29','30') and branchcd!='DD'  GROUP BY BRANCHCD"; //IN CORRUGATION INDUSTRY RAW ICODE STARTS FROM 0 ALSO
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7);

                    mq8 = "SELECT BRANCHCD,SUM(IAMOUNT) AS TOT_4B_sale FROM IVOUCHER WHERE TYPE IN ('4B','29') AND VCHDATE " + xprdrange + " AND BRANCHCD!='DD' GROUP BY BRANCHCD";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8);

                    //=================================== PURCHASE STARTS FROM HERE ====================================
                    mq9 = "SELECT BRANCHCD,SUM(dramt-cramt) AS Purchase  FROM VOUCHER WHERE substr(type,1,1)='5' and type not in ('58','59')  AND SUBSTR(TRIM(ACODE),1,2)='30' and SUBSTR(TRIM(A.rCODE),1,2) not in '02' and acode='300009'  and vchdate  " + xprdrange + " AND BRANCHCD!='DD' GROUP BY BRANCHCD";
                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9);//purchase

                    mq10 = "SELECT BRANCHCD,SUM(dramt-cramt) AS Packing  FROM VOUCHER WHERE BRANCHCD!='DD' AND substr(type,1,1)='5' and type not in ('58','59') AND SUBSTR(TRIM(ACODE),1,2)='41'  and vchdate " + xprdrange + " GROUP BY BRANCHCD";
                    dt10 = fgen.getdata(frm_qstr, co_cd, mq10);//packing cost

                    mq11 = "SELECT BRANCHCD,SUM(dramt-cramt) AS mfg_cost  FROM VOUCHER WHERE BRANCHCD!='DD' AND substr(type,1,1)='5' and type not in ('58','59')  AND SUBSTR(TRIM(ACODE),1,2)='34' and SUBSTR(TRIM(A.rCODE),1,2) not in '02'  and vchdate " + xprdrange + " GROUP BY BRANCHCD";
                    dt11 = fgen.getdata(frm_qstr, co_cd, mq11);//manuf cost

                    mq12 = "SELECT BRANCHCD,SUM(dramt-cramt) AS fa_pur  FROM VOUCHER WHERE BRANCHCD!='DD' AND substr(type,1,1)='5' and type not in ('58','59')  AND SUBSTR(TRIM(ACODE),1,2)='10' and acode not in ('100020')  and vchdate " + xprdrange + " GROUP BY BRANCHCD";
                    dt12 = fgen.getdata(frm_qstr, co_cd, mq12);//fixed asset

                    mq13 = "SELECT BRANCHCD,SUM(dramt-cramt) AS oth_exp  FROM VOUCHER WHERE BRANCHCD!='DD' AND substr(type,1,1)='5' and type not in ('58','59')  AND SUBSTR(TRIM(ACODE),1,2)>'40'  and vchdate " + xprdrange + " GROUP BY BRANCHCD";
                    dt13 = fgen.getdata(frm_qstr, co_cd, mq13);//other exp

                    //mq14 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_FG FROM VOUCHER A LEFT OUTER JOIN IVOUCHER B ON  TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')  AND SUBSTR(TRIM(B.ICODE),1,1)='7' and b.type like '0%'   WHERE SUBSTR(TRIM(A.ACODE),1,2)='02' AND A.TYPE LIKE '5%'  AND A.VCHDATE BETWEEN TO_DATE('01/04/2018','DD/MM/YYYY') AND TO_DATE('31/03/2019','DD/MM/YYYY')  GROUP BY A.BRANCHCD"; //IN THIS LEFT JIN WITH IVCH
                    //mq14 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_inward_FG FROM VOUCHER A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND SUBSTR(TRIM(B.ICODE),1,1)='7' and SUBSTR(TRIM(B.ICODE),1,4) not in ('7001','7103','7202','7304')and b.type like '0%' AND A.VCHDATE " + xprdrange + "  GROUP BY A.BRANCHCD";
                    mq14 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_inward_FG FROM VOUCHER A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND SUBSTR(TRIM(B.ICODE),1,1)='7' and SUBSTR(TRIM(B.ICODE),1,4) not in ('7001','7103','7202','7304')and b.type like '0%' AND A.VCHDATE " + xprdrange + "  GROUP BY A.BRANCHCD";
                    dt14 = fgen.getdata(frm_qstr, co_cd, mq14);

                    //mq15 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_FG FROM VOUCHER A LEFT OUTER JOIN IVOUCHER B ON  TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')  AND SUBSTR(TRIM(B.ICODE),1,4) IN ('1204','1301','7001','7101','7201','7304')  and b.type like '0%' WHERE SUBSTR(TRIM(A.ACODE),1,2)='02' AND A.TYPE LIKE '5%'  AND A.VCHDATE BETWEEN TO_DATE('01/04/2018','DD/MM/YYYY') AND TO_DATE('31/03/2019','DD/MM/YYYY')  GROUP BY A.BRANCHCD"; //IN THIS LEFT JIN WITH IVCH
                    // mq15 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_inward_semi FROM VOUCHER A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND SUBSTR(TRIM(B.ICODE),1,4) IN ('1204','1301','7001','7101','7201','7304')  and b.type like '0%'  AND A.VCHDATE " + xprdrange + " GROUP BY A.BRANCHCD";
                    mq15 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_inward_semi FROM VOUCHER A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND SUBSTR(TRIM(B.ICODE),1,4) IN ('1204','1301','7001','7101','7201','7304')  and b.type like '0%'  AND A.VCHDATE " + xprdrange + " GROUP BY A.BRANCHCD";
                    dt15 = fgen.getdata(frm_qstr, co_cd, mq15);

                    //mq16 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_FG FROM VOUCHER A LEFT OUTER JOIN IVOUCHER B ON  TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')  AND SUBSTR(TRIM(B.ICODE),1,2) IN ('10','11','12','13','14','15','16','17','18','28',29',30')  and b.type like '0%' WHERE SUBSTR(TRIM(A.ACODE),1,2)='02' AND A.TYPE LIKE '5%'  AND A.VCHDATE BETWEEN TO_DATE('01/04/2018','DD/MM/YYYY') AND TO_DATE('31/03/2019','DD/MM/YYYY')  GROUP BY A.BRANCHCD"; //IN THIS LEFT JIN WITH IVCH
                    //mq16 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_inward_rm FROM VOUCHER A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(A.MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND b.type like '0%' AND SUBSTR(TRIM(B.ICODE),1,2) IN ('10','11','12','13','14','15','16','17','18','28','29','30')   AND A.VCHDATE " + xprdrange + " GROUP BY A.BRANCHCD";
                    mq16 = "SELECT A.BRANCHCD,SUM(A.DRAMT-A.CRAMT) AS INTER_inward_rm FROM VOUCHER A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(A.MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND b.type like '0%' AND SUBSTR(TRIM(B.ICODE),1,2) IN ('10','11','12','13','14','15','16','17','18','28','29','30')   AND A.VCHDATE " + xprdrange + " GROUP BY A.BRANCHCD";
                    dt16 = fgen.getdata(frm_qstr, co_cd, mq16);

                    // mq17 = "select a.branchcd,sum(a.dramt-a.cramt) as other_inmward  from voucher A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(A.MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND b.type like '0%' and a.vchdate " + xprdrange + " group by a.branchcd";
                    mq17 = "select a.branchcd,sum(a.dramt-a.cramt) as other_inmward  from voucher A,IVOUCHER B WHERE a.BRANCHCD!='DD' AND TRIM(A.BRANCHCD)||TRIM(A.MRNNUM)||TO_CHAR(A.MRNDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND AND SUBSTR(TRIM(A.ACODE),1,1)>='3' AND SUBSTR(TRIM(A.rCODE),1,2)='02' AND A.TYPE LIKE '5%' AND b.type like '0%' and a.vchdate " + xprdrange + " group by a.branchcd";
                    dt17 = fgen.getdata(frm_qstr, co_cd, mq17);

                    mq18 = "select DISTINCT TYPE1,NAME  from type  where id='B' order by type1";
                    dt18 = fgen.getdata(frm_qstr, co_cd, mq18);
                    ///////////////for add first rows in rpt
                    #endregion

                    if (dt18.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt18);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "TYPE1"); //MAIN                  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt18, "TYPE1='" + dr0["type1"] + "'", "", DataViewRowState.CurrentRows);
                            dtm = new DataTable();
                            dtm = viewim.ToTable();
                            mq1 = "";
                            for (int i = 0; i < dtm.Rows.Count; i++)
                            {
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0;
                                dr2 = ph_tbl.NewRow();
                                dr2["unit"] = dtm.Rows[i]["type1"].ToString().Trim();
                                dr2["bname"] = dtm.Rows[i]["name"].ToString().Trim();
                                dr2["Sale_Domestic_40"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale40")), 2, true);  //b
                                dr2["Sale_Input_4A"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt1, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale4A")), 2, true); //c
                                dr2["Sale_Pur_retrn_47"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt2, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale47")), 2, true); //d
                                dr2["Sale_Scrap_45"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt3, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale45")), 2, true); //e
                                dr2["capital_sale_4G"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt4, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale4G")), 2, true); //f
                                dr2["Total_Sale"] = fgen.make_double(fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()) + fgen.make_double(dr2["Sale_Input_4A"].ToString().Trim()) + fgen.make_double(dr2["Sale_Pur_retrn_47"].ToString().Trim()) + fgen.make_double(dr2["Sale_Scrap_45"].ToString().Trim()) + fgen.make_double(dr2["capital_sale_4G"].ToString().Trim()), 2, true);
                                ///////=============
                                db = fgen.make_double(fgen.seek_iname_dt(dt5, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "FG_SALE"));
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt19, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "FG_SALE1"));
                                db2 = db - db1;
                                // dr2["Inter_Unit_outward_FG"] = fgen.make_double(fgen.seek_iname_dt(dt5, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "FG_SALE")); //h ///old
                                dr2["Inter_Unit_outward_FG"] = fgen.make_double(db2, 2, true);/////////YAHA TAK DONE
                                ///////////////////
                                db3 = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt6, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "SEMI_SALE")), 2);
                                db4 = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt20, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "SEMI_SALE1")), 2);
                                db5 = db3 + db4;
                                // dr2["Inter_Unit_outward_semi"] = fgen.make_double(fgen.seek_iname_dt(dt6, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "SEMI_SALE")); //i ......old
                                dr2["Inter_Unit_outward_semi"] = fgen.make_double(db5, 2, true);
                                /////////////////
                                dr2["Inter_Unit_outward_RM"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt7, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "RM_SALE")), 2, true); //j
                                dr2["Total_4B_Sale"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt8, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "TOT_4B_sale")), 2, true); //l
                                dr2["Inter_Unit_outward_SALE"] = fgen.make_double(fgen.make_double(dr2["Total_4B_Sale"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_FG"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_semi"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_RM"].ToString().Trim()), 2, true);
                                dr2["all_sale"] = fgen.make_double(fgen.make_double(dr2["TOTAL_SALE"].ToString().Trim()) + fgen.make_double(dr2["Total_4B_Sale"].ToString().Trim()), 2, true);
                                //===================================================purchase staRTS========================================================
                                dr2["Purchase"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt9, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "purchase")), 2, true); //n
                                dr2["Pur_Packing"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Packing")), 2, true);  //o
                                dr2["Pur_Manuf_Cost"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt11, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "mfg_cost")), 2, true); //p
                                dr2["Pur_Fixed_assets"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt12, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "fa_pur")), 2, true); //q
                                dr2["Pur_Other_exp"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt13, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "oth_exp")), 2, true);   //r

                                dr2["Pur_TOTAL"] = fgen.make_double(fgen.make_double(dr2["Purchase"].ToString().Trim()) + fgen.make_double(dr2["Pur_Packing"].ToString().Trim()) + fgen.make_double(dr2["Pur_Manuf_Cost"].ToString().Trim()) + fgen.make_double(dr2["Pur_Fixed_assets"].ToString().Trim()) + fgen.make_double(dr2["Pur_Other_exp"].ToString().Trim()), 2, true);
                                dr2["Inter_Unit_inward_FG"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt14, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_FG")), 2, true);
                                dr2["Inter_Unit_inward_semi"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt15, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_semi")), 2, true);
                                dr2["Inter_Unit_inward_RM"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt16, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_RM")), 2, true);

                                dr2["Total_IUTI"] = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt17, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "other_inmward")), 2, true);
                                dr2["Other_Pur"] = fgen.make_double(fgen.make_double(dr2["Total_IUTI"].ToString().Trim()) - (fgen.make_double(dr2["Inter_Unit_inward_FG"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_inward_semi"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_inward_RM"].ToString().Trim())), 2, true);
                                dr2["net_sale"] = fgen.make_double(fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_outward_FG"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_outward_SEMI"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_FG"].ToString().Trim()), 2, true);
                                dr2["net_pur"] = fgen.make_double(fgen.make_double(dr2["Purchase"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_semi"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_RM"].ToString().Trim()) - (fgen.make_double(dr2["Sale_Input_4A"].ToString().Trim()) + fgen.make_double(dr2["Sale_Pur_retrn_47"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_outward_RM"].ToString().Trim())), 2, true);
                                if (fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()) != 0)
                                {
                                    dr2["sale_pur_ratio_a"] = fgen.make_double(fgen.make_double(dr2["Purchase"].ToString().Trim()) * 100 / fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()), 2, true);
                                }
                                else
                                {
                                    dr2["sale_pur_ratio_a"] = 0.00;
                                }
                                /////////////
                                if (fgen.make_double(dr2["net_sale"].ToString().Trim()) != 0)
                                {
                                    dr2["sale_pur_ratio_b"] = fgen.make_double(fgen.make_double(dr2["net_pur"].ToString().Trim()) * 100 / fgen.make_double(dr2["net_sale"].ToString().Trim()), 2, true);
                                }
                                else
                                {
                                    dr2["sale_pur_ratio_b"] = 0;
                                }
                                ph_tbl.Rows.Add(dr2);
                            }
                        }
                    }
                    //if (ph_tbl.Rows.Count > 0)
                    //{
                    //    dr1 = ph_tbl.NewRow();
                    //    foreach (DataColumn dc in ph_tbl.Columns)
                    //    {
                    //        db1 = 0;
                    //        if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3)
                    //        { }
                    //        else
                    //        {
                    //            mq1 = "sum(" + dc.ColumnName + ")";
                    //            db1 += fgen.make_double(ph_tbl.Compute(mq1, "").ToString());
                    //            dr1[dc] = db1;
                    //        }
                    //    }
                    //    dr1[2] = "TOTAL";
                    //    ph_tbl.Rows.InsertAt(dr1, 0);
                    //}

                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        //fgen.Fn_open_rptlevel("All Sale and Purchase Report For the Period " + fromdt + " To " + todt, frm_qstr);
                        fgen.drillQuery(0, "SEND_DT", frm_qstr);
                        fgen.Fn_DrillReport("All Sale and Purchase Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F70504": ///UNIP
                    #region
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("unit", typeof(string));  //A
                    ph_tbl.Columns.Add("bname", typeof(string));
                    ph_tbl.Columns.Add("Sale_Domestic_40", typeof(double)); //B
                    ph_tbl.Columns.Add("Sale_Input_4A", typeof(double)); //C
                    ph_tbl.Columns.Add("Sale_Pur_retrn_47", typeof(double)); //D
                    ph_tbl.Columns.Add("Sale_Scrap_45", typeof(double));  //E
                    ph_tbl.Columns.Add("capital_sale_4G", typeof(double)); //F
                    ph_tbl.Columns.Add("Total_Sale", typeof(double)); //G
                    ph_tbl.Columns.Add("Inter_Unit_outward_FG", typeof(double));  //H
                    ph_tbl.Columns.Add("Inter_Unit_outward_semi", typeof(double));  //I
                    ph_tbl.Columns.Add("Inter_Unit_outward_RM", typeof(double));  //J
                    ph_tbl.Columns.Add("Inter_Unit_outward_Other", typeof(double));  //K
                    ph_tbl.Columns.Add("Total_4B_Sale", typeof(double));  //L                 
                    ph_tbl.Columns.Add("all_sale", typeof(double));  //M
                    //////PURCHASE STARTS
                    ph_tbl.Columns.Add("Purchase", typeof(double));  //N
                    ph_tbl.Columns.Add("Pur_Packing", typeof(double));  //O
                    ph_tbl.Columns.Add("Pur_Manuf_Cost", typeof(double));  //P
                    ph_tbl.Columns.Add("Pur_Fixed_assets", typeof(double));  //Q
                    ph_tbl.Columns.Add("Pur_Other_exp", typeof(double));  //R
                    ph_tbl.Columns.Add("Pur_TOTAL", typeof(double));  //TOTAL
                    ph_tbl.Columns.Add("Inter_Unit_inward_FG", typeof(double));  //S
                    ph_tbl.Columns.Add("Inter_Unit_inward_semi", typeof(double));  //T
                    ph_tbl.Columns.Add("Inter_Unit_inward_RM", typeof(double));  //U
                    ph_tbl.Columns.Add("Other_Pur", typeof(double));  //V
                    ph_tbl.Columns.Add("Total_IUTI", typeof(double));  //W
                    ph_tbl.Columns.Add("net_sale", typeof(double));  //X
                    ph_tbl.Columns.Add("net_pur", typeof(double));  //Y
                    ph_tbl.Columns.Add("sale_pur_ratio_a", typeof(double));  //Z
                    ph_tbl.Columns.Add("sale_pur_ratio_b", typeof(double));  //AB
                    #region
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable(); dt7 = new DataTable(); dt8 = new DataTable();
                    dt9 = new DataTable(); dt10 = new DataTable(); dt11 = new DataTable(); dt12 = new DataTable(); dt13 = new DataTable(); dt14 = new DataTable(); dt15 = new DataTable();
                    //GROSS SALE FROM SALE TABLE
                    mq0 = "select branchcd,sum(IAMOUNT) as sale40 from ivoucher where type='40' and vchdate " + xprdrange + "  AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    mq1 = "select branchcd,sum(IAMOUNT) as sale4A from ivoucher where type='4A' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);

                    mq2 = "select branchcd,sum(IAMOUNT) as sale47 from ivoucher where type='47' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    mq3 = "select branchcd,sum(IAMOUNT) as sale45 from ivoucher where type='45' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    mq4 = "select branchcd,sum(IAMOUNT) as sale4G from ivoucher where type='4G' and vchdate " + xprdrange + " AND BRANCHCD!='DD' group by branchcd ORDER BY BRANCHCD";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);
                    //======================================                
                    mq5 = "SELECT BRANCHCD,SUM(IAMOUNT) AS FG_SALE  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate  " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,2) NOT IN ('95','96') GROUP BY BRANCHCD";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq5);

                    mq19 = "SELECT BRANCHCD,SUM(IAMOUNT) AS FG_SALE1  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,4)  IN ('9108','9112','9004','9205','9304','9403') GROUP BY BRANCHCD";
                    dt19 = fgen.getdata(frm_qstr, co_cd, mq19);
                    //////====================================
                    mq6 = "SELECT BRANCHCD,SUM(IAMOUNT) AS SEMI_SALE  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate  " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,2)  IN ('95','96') GROUP BY BRANCHCD";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq6);

                    mq20 = "SELECT BRANCHCD,SUM(IAMOUNT) AS SEMI_SALE1  FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE IN ('4B','29') AND vchdate " + xprdrange + " AND  SUBSTR(TRIM(ICODE),1,1)='9'  AND SUBSTR(TRIM(ICODE),1,4)  IN ('9108','9112','9004','9205','9304','9403') GROUP BY BRANCHCD";
                    dt20 = fgen.getdata(frm_qstr, co_cd, mq20);
                    ////////========================================
                    mq7 = "SELECT BRANCHCD,SUM(IAMOUNT) AS RM_sale FROM IVOUCHER WHERE TYPE IN ('4B','29') AND VCHDATE " + xprdrange + "  AND SUBSTR(TRIM(ICODE),1,2) IN ('10','11','12','13','14','15','16','17','18','28','29','30') and branchcd!='DD'  GROUP BY BRANCHCD"; //IN CORRUGATION INDUSTRY RAW ICODE STARTS FROM 0 ALSO
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7);

                    mq7 = "SELECT BRANCHCD,SUM(IAMOUNT) AS RM_sale1 FROM IVOUCHER WHERE TYPE IN ('4B','29') AND VCHDATE " + xprdrange + "  AND SUBSTR(TRIM(ICODE),1,1)='7' and branchcd!='DD'  GROUP BY BRANCHCD"; //IN CORRUGATION INDUSTRY RAW ICODE STARTS FROM 0 ALSO
                    dt24 = fgen.getdata(frm_qstr, co_cd, mq7); //also add icode like 7 in RM on 16.11.2018
                    //////

                    mq8 = "SELECT BRANCHCD,SUM(IAMOUNT) AS TOT_4B_sale FROM IVOUCHER WHERE TYPE IN ('4B','29') AND VCHDATE " + xprdrange + " AND BRANCHCD!='DD' GROUP BY BRANCHCD";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8);

                    /////////////////////////purchase starts here==============================                   

                    mq9 = "select branchcd,sum(drmt)-sum(crmt) as Purchase from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and c.type1='30' group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";

                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9);//FOR ALL 30 GRP                 

                    mq9 = "select branchcd,sum(drmt)-sum(crmt) as Purchase1 from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and a.acode='300009'  group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt21 = fgen.getdata(frm_qstr, co_cd, mq9);//only for 300009



                    mq10 = "select branchcd,sum(drmt)-sum(crmt) as Packing from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and c.type1='41' group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt10 = fgen.getdata(frm_qstr, co_cd, mq10);



                    mq11 = "select branchcd,sum(drmt)-sum(crmt) as mfg_cost from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and c.type1='34' group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt11 = fgen.getdata(frm_qstr, co_cd, mq11);


                    mq12 = "select branchcd,sum(drmt)-sum(crmt) as fa_pur from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and c.type1='10' group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt12 = fgen.getdata(frm_qstr, co_cd, mq12);

                    mq12 = "select branchcd,sum(drmt)-sum(crmt) as fa_pur1 from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and a.acode='100020'  group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt22 = fgen.getdata(frm_qstr, co_cd, mq12);

                    mq13 = "select branchcd,sum(drmt)-sum(crmt) as oth_exp from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and c.type1='40' group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt13 = fgen.getdata(frm_qstr, co_cd, mq13);

                    //////////check purcahse                                      

                    mq14 = "select branchcd,sum(inter_fg) as INTER_inward_FG  from (SELECT a.BRANCHCD, ( case when a.ICHGS!=0 then a.IqtYIN*a.ICHGS else  a.iqtyIN*b.irate end) AS INTER_FG FROM IVOUCHER a,item b WHERE trim(a.icode)=trim(b.icode) and a.TYPE IN ('02','0U') AND SUBSTR(TRIM(A.ACODE),1,2)='02'  AND SUBSTR(TRIM(a.ICODE),1,1)='7' and  SUBSTR(TRIM(a.ICODE),1,4) not in ('7001','7103','7202','7304','1204','1301')  AND a.VCHDATE " + xprdrange + "  and A.branchcd!='DD'   ) group by branchcd";
                    dt14 = fgen.getdata(frm_qstr, co_cd, mq14);


                    mq15 = "select branchcd,sum(inter_fg) as INTER_inward_semi  from (SELECT a.BRANCHCD, ( case when a.ICHGS!=0 then a.IqtYIN*a.ICHGS else  a.iqtyIN*b.irate end) AS INTER_FG FROM IVOUCHER a,item b WHERE trim(a.icode)=trim(b.icode) and a.TYPE IN ('02','0U') AND SUBSTR(TRIM(A.ACODE),1,2)='02' and  SUBSTR(TRIM(a.ICODE),1,4) in ('7001','7103','7202','7304','1204','1301')  AND a.VCHDATE " + xprdrange + "  and A.branchcd!='DD'   ) group by branchcd";
                    dt15 = fgen.getdata(frm_qstr, co_cd, mq15);


                    mq16 = "select branchcd,sum(inter_rm) as INTER_inward_rm  from (SELECT a.BRANCHCD, ( case when a.ICHGS!=0 then a.IqtYIN*a.ICHGS else  a.iqtyIN*b.irate end) AS INTER_rm FROM IVOUCHER a,item b WHERE trim(a.icode)=trim(b.icode) and a.TYPE IN ('02','0U') AND SUBSTR(TRIM(A.ACODE),1,2)='02' and  SUBSTR(TRIM(a.ICODE),1,2) IN ('10','11','12','13','14','15','16','17','18','28','29','30')  AND a.VCHDATE " + xprdrange + " and A.branchcd!='DD') group by branchcd";
                    dt16 = fgen.getdata(frm_qstr, co_cd, mq16);

                    mq17 = "select branchcd,sum(inter_rm) as INTER_inward_rm1  from (SELECT a.BRANCHCD, ( case when a.ICHGS!=0 then a.IqtYIN*a.ICHGS else  a.iqtyIN*b.irate end) AS INTER_rm FROM IVOUCHER a,item b WHERE trim(a.icode)=trim(b.icode) and a.TYPE IN ('02','0U') AND SUBSTR(TRIM(A.ACODE),1,2)='02' and  SUBSTR(TRIM(a.ICODE),1,4)  IN ('1204','1301')  AND a.VCHDATE " + xprdrange + " and A.branchcd!='DD') group by branchcd";
                    dt17 = fgen.getdata(frm_qstr, co_cd, mq17);



                    mq22 = "select branchcd,sum(drmt)-sum(crmt) as inw_oth from (select b.bssch as subgrpcode,c.type1 as mgcode,c.name as mgname, a.branchcd,trim(a.acode) as acode,b.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd not in ('88','DD') union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdRange1 + " GROUP BY aCODE,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd!='88' and type like '%' and vchdate " + xprdrange + " GROUP BY ACODE,branchcd) a,famst b,type c where  substr(TRIM(A.acode),1,2)=trim(c.type1)  and c.id='Z'   and trim(a.acode)=trim(b.acode) and a.acode='100020' group by a.branchcd,trim(a.acode),b.aname,c.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) ORDER BY mgcode,aCODE,a.branchcd ) group by branchcd";
                    dt23 = fgen.getdata(frm_qstr, co_cd, mq22);

                    mq18 = "select DISTINCT TYPE1,NAME from type  where id='B' order by type1";
                    dt18 = fgen.getdata(frm_qstr, co_cd, mq18);
                    ///////////////for add first rows in rpt
                    #endregion

                    if (dt18.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt18);
                        dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "TYPE1"); //MAIN                  
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView viewim = new DataView(dt18, "TYPE1='" + dr0["type1"] + "'", "", DataViewRowState.CurrentRows);
                            dtm = new DataTable();
                            dtm = viewim.ToTable();
                            mq1 = "";
                            for (int i = 0; i < dtm.Rows.Count; i++)
                            {
                                db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; double db10 = 0, db11 = 0, db12 = 0, db13 = 0, db14 = 0, db15 = 0;
                                dr2 = ph_tbl.NewRow();
                                dr2["unit"] = dtm.Rows[i]["type1"].ToString().Trim();
                                dr2["bname"] = dtm.Rows[i]["name"].ToString().Trim();
                                dr2["Sale_Domestic_40"] = fgen.make_double(fgen.seek_iname_dt(dt, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale40"));  //b
                                dr2["Sale_Input_4A"] = fgen.make_double(fgen.seek_iname_dt(dt1, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale4A")); //c
                                dr2["Sale_Pur_retrn_47"] = fgen.make_double(fgen.seek_iname_dt(dt2, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale47")); //d
                                dr2["Sale_Scrap_45"] = fgen.make_double(fgen.seek_iname_dt(dt3, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale45")); //e
                                dr2["capital_sale_4G"] = fgen.make_double(fgen.seek_iname_dt(dt4, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "sale4G")); //f
                                dr2["Total_Sale"] = fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()) + fgen.make_double(dr2["Sale_Input_4A"].ToString().Trim()) + fgen.make_double(dr2["Sale_Pur_retrn_47"].ToString().Trim()) + fgen.make_double(dr2["Sale_Scrap_45"].ToString().Trim()) + fgen.make_double(dr2["capital_sale_4G"].ToString().Trim());
                                ///////=============
                                db = fgen.make_double(fgen.seek_iname_dt(dt5, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "FG_SALE"));
                                db1 = fgen.make_double(fgen.seek_iname_dt(dt19, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "FG_SALE1"));
                                db2 = db - db1;

                                dr2["Inter_Unit_outward_FG"] = db2;/////////YAHA TAK DONE
                                ///////////////////
                                db3 = fgen.make_double(fgen.seek_iname_dt(dt6, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "SEMI_SALE"));
                                db4 = fgen.make_double(fgen.seek_iname_dt(dt20, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "SEMI_SALE1"));
                                db5 = db3 + db4;

                                dr2["Inter_Unit_outward_semi"] = db5;
                                /////////////////
                                db14 = fgen.make_double(fgen.seek_iname_dt(dt7, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "RM_SALE"));
                                db15 = fgen.make_double(fgen.seek_iname_dt(dt24, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "RM_SALE1"));
                                dr2["Inter_Unit_outward_RM"] = db14 + db15;
                                dr2["Total_4B_Sale"] = fgen.make_double(fgen.seek_iname_dt(dt8, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "TOT_4B_sale")); //l
                                db6 = fgen.make_double(dr2["Total_4B_Sale"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_FG"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_semi"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_RM"].ToString().Trim());
                                dr2["Inter_Unit_outward_Other"] = db6;
                                dr2["all_sale"] = fgen.make_double(dr2["TOTAL_SALE"].ToString().Trim()) + fgen.make_double(dr2["Total_4B_Sale"].ToString().Trim());
                                //===================================================purchase staRTS========================================================
                                db7 = fgen.make_double(fgen.seek_iname_dt(dt9, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "purchase")); //n
                                db8 = fgen.make_double(fgen.seek_iname_dt(dt21, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "purchase1")); //n
                                dr2["Purchase"] = db7 - db8;
                                dr2["Pur_Packing"] = fgen.make_double(fgen.seek_iname_dt(dt10, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Packing"));  //o
                                dr2["Pur_Manuf_Cost"] = fgen.make_double(fgen.seek_iname_dt(dt11, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "mfg_cost")); //p
                                db10 = fgen.make_double(fgen.seek_iname_dt(dt12, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "fa_pur")); //q
                                db9 = fgen.make_double(fgen.seek_iname_dt(dt22, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "fa_pur1"));
                                dr2["Pur_Fixed_assets"] = db10 - db9;
                                dr2["Pur_Other_exp"] = fgen.make_double(fgen.seek_iname_dt(dt13, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "oth_exp"));   //r
                                dr2["Pur_TOTAL"] = fgen.make_double(dr2["Purchase"].ToString().Trim()) + fgen.make_double(dr2["Pur_Packing"].ToString().Trim()) + fgen.make_double(dr2["Pur_Manuf_Cost"].ToString().Trim()) + fgen.make_double(dr2["Pur_Fixed_assets"].ToString().Trim()) + fgen.make_double(dr2["Pur_Other_exp"].ToString().Trim());
                                dr2["Inter_Unit_inward_FG"] = fgen.make_double(fgen.seek_iname_dt(dt14, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_FG"));
                                dr2["Inter_Unit_inward_semi"] = fgen.make_double(fgen.seek_iname_dt(dt15, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_semi"));
                                //================================
                                db9 = 0; db10 = 0;
                                db9 = fgen.make_double(fgen.seek_iname_dt(dt16, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_RM"));
                                db10 = fgen.make_double(fgen.seek_iname_dt(dt17, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "Inter_inward_RM1"));
                                dr2["Inter_Unit_inward_RM"] = db9 - db10;
                                dr2["Other_Pur"] = fgen.make_double(fgen.seek_iname_dt(dt23, "branchcd='" + dr2["unit"].ToString().Trim() + "'", "inw_oth"));

                                dr2["Total_IUTI"] = fgen.make_double(dr2["Inter_Unit_inward_FG"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_semi"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_RM"].ToString().Trim()) + fgen.make_double(dr2["Other_Pur"].ToString().Trim());
                                ////==================
                                dr2["net_sale"] = fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_outward_FG"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_outward_SEMI"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_inward_FG"].ToString().Trim());
                                dr2["net_pur"] = fgen.make_double(dr2["Purchase"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_semi"].ToString().Trim()) + fgen.make_double(dr2["Inter_Unit_inward_RM"].ToString().Trim()) - fgen.make_double(dr2["Sale_Input_4A"].ToString().Trim()) - fgen.make_double(dr2["Sale_Pur_retrn_47"].ToString().Trim()) - fgen.make_double(dr2["Inter_Unit_outward_RM"].ToString().Trim());
                                if (fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim()) != 0)
                                {
                                    dr2["sale_pur_ratio_a"] = fgen.make_double(dr2["Purchase"].ToString().Trim()) * 100 / fgen.make_double(dr2["Sale_Domestic_40"].ToString().Trim());
                                }
                                else
                                {
                                    dr2["sale_pur_ratio_a"] = 0;
                                }
                                if (fgen.make_double(dr2["net_sale"].ToString().Trim()) != 0)
                                {
                                    dr2["sale_pur_ratio_b"] = fgen.make_double(dr2["net_pur"].ToString().Trim()) * 100 / fgen.make_double(dr2["net_sale"].ToString().Trim());
                                }
                                else
                                {
                                    dr2["sale_pur_ratio_b"] = 0;
                                }
                                ph_tbl.Rows.Add(dr2);
                            }
                        }
                    }
                    dr1 = ph_tbl.NewRow();
                    dr1["bname"] = "All Sale and Purchase Report From " + fromdt + " To " + todt;
                    ph_tbl.Rows.InsertAt(dr1, 0);
                    if (ph_tbl.Rows.Count > 0)
                    {
                        dr1 = ph_tbl.NewRow();
                        foreach (DataColumn dc in ph_tbl.Columns)
                        {
                            db1 = 0;
                            if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 27 || dc.Ordinal == 28)// || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 7 || dc.Ordinal == 8 || dc.Ordinal == 9 || dc.Ordinal == 10 || dc.Ordinal == 11 || dc.Ordinal == 12 || dc.Ordinal == 13)
                            { }
                            else
                            {
                                mq1 = "sum(" + dc.ColumnName + ")";
                                db1 += fgen.make_double(ph_tbl.Compute(mq1, "").ToString());
                                dr1[dc] = db1;
                            }
                        }
                        dr1["sale_pur_ratio_a"] = fgen.make_double(dr1["Purchase"].ToString().Trim()) * 100 / fgen.make_double(dr1["Sale_Domestic_40"].ToString().Trim());
                        dr1["sale_pur_ratio_b"] = fgen.make_double(dr1["net_pur"].ToString().Trim()) * 100 / fgen.make_double(dr1["net_sale"].ToString().Trim());
                        dr1[1] = "TOTAL";
                        ph_tbl.Rows.InsertAt(dr1, 1);
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("All Sale and Purchase Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F70600":
                case "F70602":
                case "F70604":
                case "F70606":
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

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    xbstring = "branchcd='" + mbr + "'";

                    switch (val)
                    {
                        case "F70600":
                            my_rep_head = "Debtors (Bill Wise) Ageing For the period " + value1 + " To " + value2 + "";
                            s_code1 = "16";
                            s_code2 = "D";
                            break;
                        case "F70602":
                            my_rep_head = "Debtors (Customer Wise) Ageing For the period " + value1 + " To " + value2 + "";
                            s_code1 = "16";
                            s_code2 = "S";
                            break;
                        case "F70604":
                            my_rep_head = "Creditors (Bill Wise) Ageing For the period " + value1 + " To " + value2 + "";
                            s_code1 = "06";
                            s_code2 = "D";
                            break;
                        case "F70606":
                            my_rep_head = "Creditors (Supplier Wise) Ageing For the period " + value1 + " To " + value2 + "";
                            s_code1 = "06";
                            s_code2 = "S";
                            break;
                    }

                    col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT opt_Start FROM FIN_RSYS_OPT_PW WHERE branchcd='" + mbr + "' and UPPER(TRIM(OPT_ID))='W2001'", "OPT_START");
                    if (fgen.CheckIsDate(col1) == true)
                    {
                        col1 = col1.Trim().ToUpper();
                    }
                    else
                    {
                        col1 = frm_cDt1;
                    }
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    string eff_Dt = "";
                    string dat_fld = "";
                    string popsql = "";
                    eff_Dt = " vchdate>= to_date('" + col1 + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";

                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";
                    if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
                    else branch_Cd = "branchcd='" + mbr + "'";

                    mq0 = "select branchcd,trim(ACODE) as Acode,trim(upper(INVNO)) as INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,nvl(DRAMT,0) AS DRAMT,nvl(CRAMT,0) AS CRAMT FROM VOUCHER WHERE " + branch_Cd + " and BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)='" + s_code1 + "' ";
                    mq1 = "UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,nvl(DRAMT,0) AS DRAMT,nvl(CRAMT,0) AS CRAMT FROM RECEBAL WHERE " + branch_Cd + " and BRANCHCD!='88' AND BRANCHCD!='DD' and SUBSTR(ACODE,1,2)='" + s_code1 + "' ) GROUP BY branchcd,trim(ACODE),trim(upper(INVNO)),INVDATE having SUM(DRAMT)-SUM(cRAMT)<>0 ";
                    popsql = mq0 + mq1;
                    dat_fld = "(to_DatE('" + todt + "','dd/mm/yyyy')-(a.invdate+nvl(b.pay_num,0))";

                    if (s_code2 == "D")
                    {
                        mq0 = "select trim(n.acode) as FSTR,'-' as GSTR,m.aname as Party,n.Invno,sum(n.total) as Total_Outstanding,sum(n.slab0) as Not_Due,sum(n.slab1) Current_Os,sum(n.slab2) as OVER_30_60,sum(n.slab3) as OVER_61_90,sum(n.slab4) as OVER_90_180,sum(n.slab5) as OVER_181,to_chaR(n.Invdate,'dd/mm/yyyy') as Inv_dt,to_char(n.due_Date,'dd/mm/yyyy') as Due_dtd,m.ADDR1 as Address,trim(n.acode) as ERP_Acode,m.Payment as P_days from (SELECT a.invno,a.invdate,a.invdate+nvl(b.pay_num,0) as Due_date,a.acode,a.dramt-a.cramt as total,(CASE WHEN " + dat_fld + " <0 ) THEN a.dramt-a.cramt else 0 END) as slab0 ,";
                        mq1 = "(CASE WHEN " + dat_fld + " BETWEEN 0 AND 30) THEN a.dramt-a.cramt END) as slab1  ,(CASE WHEN " + dat_fld + " BETWEEN 30 AND 60) THEN a.dramt-a.cramt END) as slab2,(CASE WHEN " + dat_fld + " BETWEEN 60 AND 90) THEN a.dramt-a.cramt END) as slab3,(CASE WHEN " + dat_fld + " BETWEEN 90 AND 180) THEN a.dramt-a.cramt END) as slab4,(CASE WHEN " + dat_fld + " > 180) THEN a.dramt-a.cramt END) as slab5 from  (" + popsql + ") a,famst b where trim(A.acode)=trim(b.acode)) n ,famst m where trim(n.acode)=trim(m.acode) and m.bssch like '" + party_cd + "%' and nvl(m.acode,'-') like '" + part_cd + "%' group by m.aname,m.addr1,m.climit,m.payment,trim(n.acode),m.zcode,n.Invno,n.due_Date,to_chaR(n.Invdate,'dd/mm/yyyy'),to_char(n.due_Date,'dd/mm/yyyy') having sum(n.total)<>0 order by m.aname,n.due_Date,n.Invno";
                    }
                    else
                    {
                        mq0 = "select trim(n.acode) as FSTR,'-' as GSTR,m.aname as Party,m.STATEN as Address,sum(n.total) as Total_Outstanding,sum(n.slab0) as Not_Due,sum(n.slab1) as Current_Os,sum(n.slab2) as OVER_30_60,sum(n.slab3) as OVER_61_90,sum(n.slab4) as OVER_90_180,sum(n.slab5) as OVER_181,trim(n.acode) as ERP_Acode,m.Payment as P_days from (SELECT a.invno,a.invdate,a.invdate+nvl(b.pay_num,0) as Due_date,a.acode,a.dramt-a.cramt as total,(CASE WHEN " + dat_fld + " <0 ) THEN a.dramt-a.cramt else 0 END) as slab0 ,";
                        mq1 = "(CASE WHEN " + dat_fld + " BETWEEN 0 AND 30) THEN a.dramt-a.cramt END) as slab1  ,(CASE WHEN " + dat_fld + " BETWEEN 30 AND 60) THEN a.dramt-a.cramt END) as slab2,(CASE WHEN " + dat_fld + " BETWEEN 60 AND 90) THEN a.dramt-a.cramt END) as slab3,(CASE WHEN " + dat_fld + " BETWEEN 90 AND 180) THEN a.dramt-a.cramt END) as slab4,(CASE WHEN " + dat_fld + " > 180) THEN a.dramt-a.cramt END) as slab5 from  (" + popsql + ") a,famst b where trim(A.acode)=trim(b.acode)) n ,famst m where trim(n.acode)=trim(m.acode) and m.bssch like '" + party_cd + "%' and nvl(m.acode,'-') like '" + part_cd + "%' group by m.aname,m.STATEN,m.climit,m.payment,trim(n.acode),m.zcode having sum(n.total)<>0 order by m.aname";
                    }
                    SQuery = mq0 + mq1;


                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#9#10#11#", "3#4#", "350#150#");


                    mq0 = "select branchcd,trim(ACODE) as Acode,trim(upper(INVNO)) as INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,nvl(DRAMT,0) AS DRAMT,nvl(CRAMT,0) AS CRAMT FROM VOUCHER WHERE " + branch_Cd + " and BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)='" + s_code1 + "' ";
                    mq1 = "UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,nvl(DRAMT,0) AS DRAMT,nvl(CRAMT,0) AS CRAMT FROM RECEBAL WHERE " + branch_Cd + " and BRANCHCD!='88' AND BRANCHCD!='DD' and SUBSTR(ACODE,1,2)='" + s_code1 + "' ) GROUP BY branchcd,trim(ACODE),trim(upper(INVNO)),INVDATE having SUM(DRAMT)-SUM(cRAMT)<>0 ";
                    popsql = mq0 + mq1;
                    dat_fld = "(to_DatE('" + todt + "','dd/mm/yyyy')-(a.invdate+nvl(b.pay_num,0))";


                    mq0 = "select trim(n.acodE)||n.Invno||to_char(n.invdate,'dd/mm/yyyy') as FSTR,trim(n.acode) as GSTR,m.aname as Party,n.Invno,sum(n.total) as Total_Outstanding,sum(n.slab0) as Not_Due,sum(n.slab1) as Current_Os,sum(n.slab2) as OVER_30_60,sum(n.slab3) as OVER_61_90,sum(n.slab4) as OVER_90_180,sum(n.slab5) as OVER_181,to_chaR(n.Invdate,'dd/mm/yyyy') as Inv_dt,to_char(n.due_Date,'dd/mm/yyyy') as Due_dtd,m.ADDR1 as Address,trim(n.acode) as ERP_Acode,m.Payment as P_days from (SELECT a.invno,a.invdate,a.invdate+nvl(b.pay_num,0) as Due_date,a.acode,a.dramt-a.cramt as total,(CASE WHEN " + dat_fld + " <0 ) THEN a.dramt-a.cramt else 0 END) as slab0 ,";
                    mq1 = "(CASE WHEN " + dat_fld + " BETWEEN 0 AND 30) THEN a.dramt-a.cramt END) as slab1  ,(CASE WHEN " + dat_fld + " BETWEEN 30 AND 60) THEN a.dramt-a.cramt END) as slab2,(CASE WHEN " + dat_fld + " BETWEEN 60 AND 90) THEN a.dramt-a.cramt END) as slab3,(CASE WHEN " + dat_fld + " BETWEEN 90 AND 180) THEN a.dramt-a.cramt END) as slab4,(CASE WHEN " + dat_fld + " > 180) THEN a.dramt-a.cramt END) as slab5 from  (" + popsql + ") a,famst b where trim(A.acode)=trim(b.acode)) n ,famst m where trim(n.acode)=trim(m.acode) and m.bssch like '" + party_cd + "%' and nvl(m.acode,'-') like '" + part_cd + "%' group by m.aname,m.addr1,m.climit,m.payment,trim(n.acode),m.zcode,n.Invno,n.due_Date,to_chaR(n.Invdate,'dd/mm/yyyy'),to_char(n.due_Date,'dd/mm/yyyy'),trim(n.acodE)||n.Invno||to_char(n.invdate,'dd/mm/yyyy') having sum(n.total)<>0 order by m.aname,n.due_Date,n.Invno";

                    SQuery = mq0 + mq1;
                    fgen.drillQuery(1, SQuery, frm_qstr, "5#6#7#8#9#10#11#", "3#4#", "350#150#");


                    //fgen.drillQuery(1, "SELECT * FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ACODE)='FSTR' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr, "5#6#7#8#9#10#", "3#4#", "350#150#");
                    //fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.DRAMT) AS DEBIT,(a.CRAMT) AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.ST_ENTFORM,A.BRANCHCD as PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'", frm_qstr);
                    cond = "";


                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;
                case "F70680":
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

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";


                    xbstring = "branchcd='" + mbr + "'";


                    my_rep_head = "Cost Center Report For the period " + value1 + " To " + value2 + "";
                    s_code1 = "0000000";
                    s_code2 = "9ZZZZZZ";
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");

                    mq0 = "select fstr,'-' as gstr,max(Biz_Group) as Biz_Group,max(cc1) as CC_L1,max(cc2) as CC_L2,max(cc3) As CC_L3,sum(amt_sale)As Total_Amount,max(fcoth1) BG_code,max(fcoth2) as CC_L1_code,max(fcoth3) as CC_L2_code,max(fcoth4) as CC_L3_code from (";
                    mq1 = "select a.branchcd||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') As fstr,b.name as Biz_Group,null as cc1,null as cc2,null as cc3,a.amt_sale,a.fcoth1,a.fcoth2,a.fcoth3,a.fcoth4 from wb_pv_head a ,(select name,type1 from typegrp where branchcd!='DD' and id='BZ') b where trim(to_Char(a.fcoth1,'9999'))=trim(b.type1) and a.branchcd='" + mbr + "' and a.type like '5%' and a.vchdate " + xprd2 + " union all select a.branchcd||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') As fstr,null as Biz_Group,b.name as cc1,null as cc2,null as cc3,0 as amt_sale,a.fcoth1,a.fcoth2,a.fcoth3,a.fcoth4 from wb_pv_head a ,(select name,type1 from typegrp where branchcd!='DD' and id='L1') b where trim(to_Char(a.fcoth2,'9999'))=trim(b.type1) and a.branchcd='" + mbr + "' and a.type like '5%' and a.vchdate " + xprd2 + " union all ";
                    mq2 = "select a.branchcd||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') As fstr,null as Biz_Group,null as cc1,b.name as cc2,null as cc3,0 as amt_sale,a.fcoth1,a.fcoth2,a.fcoth3,a.fcoth4 from wb_pv_head a ,(select name,type1 from typegrp where branchcd!='DD' and id='L2') b where trim(to_Char(a.fcoth3,'9999'))=trim(b.type1) and a.branchcd='" + mbr + "' and a.type like '5%' and a.vchdate " + xprd2 + " union all select a.branchcd||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') As fstr,null as Biz_Group,null as cc1,null as cc2,b.name as cc3,0 as amt_sale,a.fcoth1,a.fcoth2,a.fcoth3,a.fcoth4 from wb_pv_head a ,(select name,type1 from typegrp where branchcd!='DD' and id='L3') b where trim(to_Char(a.fcoth4,'9999'))=trim(b.type1) and a.branchcd='" + mbr + "' and a.type like '5%' and a.vchdate " + xprd2 + ") group by fstr";


                    SQuery = mq0 + mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "7#", "3#4#5#6#", "300#200#200#200#100#");
                    fgen.drillQuery(1, "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS DEBITS,SUM(CRAMT) AS CREDITS,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ACODE)='FSTR' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr, "4#5#", "3#4#5#6#", "400#120#120#120");
                    fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,A.DRAMT AS DEBIT,a.CRAMT AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.ST_ENTFORM,A.BRANCHCD as PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'", frm_qstr, "5#6#", "3#4#5#6#7#8#9#10#", "300#90#100#100#50#80#120#50#");
                    cond = "";

                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";

                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;

                case "F70652":
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

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";


                    xbstring = "branchcd='" + mbr + "'";
                    double sal_tot = 0;
                    sal_tot = fgen.make_double(fgen.seek_iname(frm_qstr, co_cd, "Select sum(a.amt_Sale) as tot from sale a where " + xbstring + " and a.type like '4%' and a.vchdate " + xprd2 + "", "TOT"));
                    mq0 = "Select b.Aname as Account,substr(a.acode,1,2) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.Dramt-a.cramt),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.Dramt-a.cramt),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.Dramt-a.cramt),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.Dramt-a.cramt),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.Dramt-a.cramt),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.Dramt-a.cramt),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.Dramt-a.cramt),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.Dramt-a.cramt),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.Dramt-a.cramt),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.Dramt-a.cramt),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.Dramt-a.cramt),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.Dramt-a.cramt),0) as Mar,a.acode from voucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd='" + mbr + "' and a.vchdate " + xprd2 + " and substr(a.acode,1,2)>='29' group by a.acode,b.aname,to_char(vchdate,'yyyymm'),substr(a.acode,1,2)  ";

                    if (OVERSEAS == "N")
                    {
                        mq1 = "Select Acode as fstr,'-' as gstr,Account,to_char(round((sum(April+May+June+July+August+Sept+oct+Nov+Dec+Jan+Feb+Mar)/" + (sal_tot <= 0 ? 1 : sal_tot) + ")*100,2),'999.99')  as Perc_of_Sale,sum(April+May+June+July+August+Sept+oct+Nov+Dec+Jan+Feb+Mar) as Totals,sum(April) as April,sum(May) as May,sum(June) as June,sum(July) as July,sum(August) as August,sum(Sept) as Sept,sum(oct) as Oct,sum(Nov) as Nov,sum(Dec) as Dec,sum(Jan) as Jan,sum(Feb) as Feb,sum(Mar) as Mar,Acode,Grp from (" + mq0 + ") group by Grp,account,acode order by Grp,Account";
                    }
                    else
                    {
                        mq1 = "Select Acode as fstr,'-' as gstr,Account,to_char(round((sum(April+May+June+July+August+Sept+oct+Nov+Dec+Jan+Feb+Mar)/" + (sal_tot <= 0 ? 1 : sal_tot) + ")*100,2),'999.99')  as Perc_of_Sale,sum(April+May+June+July+August+Sept+oct+Nov+Dec+Jan+Feb+Mar) as Totals,sum(Jan) as Jan,sum(Feb) as Feb,sum(Mar) as Mar,sum(April) as April,sum(May) as May,sum(June) as June,sum(July) as July,sum(August) as August,sum(Sept) as Sept,sum(oct) as Oct,sum(Nov) as Nov,sum(Dec) as Dec,Acode,Grp from (" + mq0 + ") group by Grp,account,acode order by Grp,Account";
                    }


                    fgen.drillQuery(0, mq1, frm_qstr, "4#5#6#7#8#9#10#11#12#13#14#15#16#17#", "3#", "300#");
                    fgen.drillQuery(1, "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS DEBITS,SUM(CRAMT) AS CREDITS,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ACODE)='FSTR' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr, "4#5#", "3#4#5#6#", "400#120#120#120");
                    fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,A.DRAMT AS DEBIT,a.CRAMT AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.ST_ENTFORM,A.BRANCHCD as PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'", frm_qstr, "5#6#", "3#4#5#6#7#8#9#10#", "300#90#100#100#50#80#120#50#");
                    cond = "";

                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";

                    my_rep_head = "Expenses Trend Vs Sales (" + sal_tot + ") for the period " + value1 + " To " + value2 + "";

                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;

                case "F05349":
                case "F70650":
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

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";


                    xbstring = "branchcd='" + mbr + "'";
                    //If mhd = "Y" Then
                    //    xbstring = "branchcd not in ('DD','88')"
                    //End If

                    switch (val)
                    {
                        case "F05349":
                        case "F70650":
                            my_rep_head = "Trial Balance for the period " + value1 + " To " + value2 + "";
                            s_code1 = "0000000";
                            s_code2 = "9ZZZZZZ";
                            break;
                    }

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    mq0 = "select trim(a.acode) as FSTR,'-' as GSTR,(case when trim(A.acode)='Total' then 'Report Total' else b.aname end) as Account_Name,sum(a.opening) as Opening_Bal,sum(a.cdr) as Debits,sum(a.ccr) as Credits,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,trim(a.acode) as Acode,substr(trim(a.acode),1,2) as Grp,b.Bssch,a.Dset from ( ";
                    mq1 = "Select 'Total' as Acode, nvl(YR_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos,'S1' as Dset from famstbal where " + branch_Cd + " and acode between '" + s_code1 + "' and '" + s_code2 + "' union all select 'Total' as Acode,dramt-cramt as op,0 as cdr,0 as ccr,0 as clos,'S1' as Dset from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and acode between '" + s_code1 + "' and '" + s_code2 + "' union all select 'Total' as Acode,0 as op,dramt as cdr,cramt as ccr,0 as clos,'S1' as Dset from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprd2 + " and acode between '" + s_code1 + "' and '" + s_code2 + "' union all ";
                    mq2 = "Select acode, nvl(YR_" + year + ",0) as opening,0 as cdr,0 as ccr,0 as clos,'S2' as Dset from famstbal where " + branch_Cd + " and acode between '" + s_code1 + "' and '" + s_code2 + "' union all select acode,dramt-cramt as op,0 as cdr,0 as ccr,0 as clos,'S2' as Dset from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprd1 + " and acode between '" + s_code1 + "' and '" + s_code2 + "' union all select acode,0 as op,dramt as cdr,cramt as ccr,0 as clos,'S2' as Dset from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprd2 + " and acode between '" + s_code1 + "' and '" + s_code2 + "')a left outer join famst b on trim(A.acode)=trim(B.acodE) group by Dset,b.aname,trim(a.acode),substr(trim(a.acode),1,2),b.Bssch  having sum(abs(a.opening))+sum(a.cdr)+sum(a.ccr)!=0 order by Dset,b.Bssch,b.aname";

                    fgen.drillQuery(0, mq0 + mq1 + mq2, frm_qstr, "4#5#6#7#", "3#4#5#6#7#", "400#100#100#100#100#");
                    fgen.drillQuery(1, "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS DEBITS,SUM(CRAMT) AS CREDITS,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ACODE)='FSTR' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr, "4#5#6#", "3#4#5#6#", "200#200#400");
                    fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.DRAMT) AS DEBIT,(a.CRAMT) AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.ST_ENTFORM,A.BRANCHCD as PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'", frm_qstr, "5#6#", "3#4#5#6#7#8#9#10", "220#70#100#100#30#50#200#30#");
                    cond = "";

                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";

                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;


                case "F70710":
                case "F70712":
                case "F70714":
                case "F70716":
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

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                    xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                    xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";


                    xbstring = "branchcd='" + mbr + "'";
                    string sumdtl = "S";
                    string repvty = "4";
                    string repamtfld = "cramt";
                    string repacode = "substr(Acode,1,1)='2'";

                    switch (val)
                    {
                        case "F70710":
                            my_rep_head = "VAT/GST Payable Summary for the period " + value1 + " To " + value2 + "";
                            break;
                        case "F70712":
                            my_rep_head = "VAT/GST Payable Details for the period " + value1 + " To " + value2 + "";
                            sumdtl = "D";
                            break;
                        case "F70714":
                            my_rep_head = "VAT/GST Receivable Summary for the period " + value1 + " To " + value2 + "";
                            repvty = "5";
                            repamtfld = "dramt";
                            repacode = "substr(Acode,1,1) in ('3','4','5')";
                            break;
                        case "F70716":
                            my_rep_head = "VAT/GST Receivable Details for the period " + value1 + " To " + value2 + "";
                            sumdtl = "D";
                            repamtfld = "dramt";
                            repacode = "substr(Acode,1,1) in ('3','4','5')";
                            repvty = "5";
                            break;
                    }

                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    string txcode1 = "";
                    txcode1 = fgen.getOption(frm_qstr, frm_cocd, (repvty == "4" ? "W0083" : "W0084"), "OPT_PARAM");

                    mq0 = "Select type,rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,type||vchnum||to_char(Vchdate,'dd/mm/yyyy') as fstr,invno as vchnum,vchdate from voucher where branchcd='" + mbr + "' and type like '" + repvty + "%' and vchdate " + xprd2 + " and " + repacode + " union all Select type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,type||vchnum||to_char(Vchdate,'dd/mm/yyyy') as fstr,invno as vchnum,vchdate from voucher where branchcd='" + mbr + "' and type like '" + repvty + "%' and vchdate " + xprd2 + " and acode = '" + txcode1.Trim() + "' ";
                    mq1 = "select '-' as fstr,'-' as gstr,'-' as type,'Totals' as Party_Code,sum(a.Sale_amt) as Sale_Amount,sum(a.CGST+a.IGST) as VAT_Amount,'-' as vchnum,null as vchdate,'DS1' as DataSet from (" + mq0 + ") a  union all select '-' as fstr,'-' as gstr,a.type,max(a.acode) as Party_Code,sum(a.Sale_amt) as Sale_Amount,sum(a.CGST+a.IGST) as VAT_Amount,a.vchnum,a.vchdate,'DS2' as DataSet from (" + mq0 + ") a group by a.fstr,a.type,a.vchnum,a.vchdate";

                    if (sumdtl == "S")
                    {

                        mq2 = "select '-' as fstr,'-' as gstr,b.Aname,a.Party_Code,sum(a.Sale_Amount) as Taxable_Amount,sum(a.VAT_Amount) as VAT_Amount,c.Name,b.gst_no as Tax_Number,a.type,a.DataSet from (" + mq1 + ") a left outer join famst b on trim(a.Party_Code)=trim(B.acode) left outer join (Select type1,name from type where id='V') c on trim(a.type)=trim(c.type1)  group by b.Aname,a.Party_Code,c.Name,b.gst_no,a.type,a.DataSet order by a.DataSet,a.type,b.aname";

                        fgen.drillQuery(0, mq2, frm_qstr, "5#6#", "3#4#5#6#7#", "400#80#120#120#200#");
                    }
                    else
                    {
                        mq2 = "select '-' as fstr,'-' as gstr,b.Aname,a.Party_Code,a.Sale_Amount as Taxable_Amount,a.VAT_Amount,c.Name,b.gst_no as Tax_Number,a.type,a.vchnum as Inv_no,to_Char(a.vchdate,'dd/mm/yyyy') as Inv_dt,to_Char(a.vchdate,'yyyymmdd') as VDD,a.DataSet from (" + mq1 + ") a left outer join famst b on trim(a.Party_Code)=trim(B.acode) left outer join (Select type1,name from type where id='V') c on trim(a.type)=trim(c.type1)  order by a.DataSet,a.type,b.aname,to_Char(a.vchdate,'yyyymmdd'),a.vchnum";
                        fgen.drillQuery(0, mq2, frm_qstr, "5#6#", "3#4#5#6#7#", "400#80#120#120#200#");
                    }

                    //fgen.drillQuery(1, "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS DEBITS,SUM(CRAMT) AS CREDITS,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND trim(ACODE)='FSTR' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr, "4#5#", "3#4#5#6#", "400#120#120#120");
                    //fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,A.DRAMT AS DEBIT,a.CRAMT AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.ST_ENTFORM,A.BRANCHCD as PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'", frm_qstr, "5#6#", "3#4#5#6#7#8#9#10#", "300#90#100#100#50#80#120#50#");
                    cond = "";

                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";

                    fgen.Fn_DrillReport(my_rep_head, frm_qstr);
                    break;


                case "F70440":
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
                    SQuery = "select a.vchnum, to_char(c.vchdate,'dd/mm/yyyy') as vchdate,a.acode,a.assetname ,a.assetid,a.grpcode,b.Name as Grp_name,a.locn,c.name as Location_name,to_char(a.instdt,'dd/mm/yyyy') as instdt,a.basiccost,a.install_cost,a.custom_duty,a.other_chrgs,a.original_cost,a.assetsupp,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as edt_dt from wb_fa_pur a,typegrp b,typegrp c where a.branchcd='" + mbr + "' and c.branchcd !='DD' and b.branchcd !='DD' and b.id='FA' and c.id='LF' and a.grpcode like '" + party_cd + "%' and a.locn like '" + part_cd + "%' and trim(a.grpcode)= trim(b.type1) and trim(a.locn)= trim(c.type1) AND A.vchDATE " + xprdrange + " order by a.assetname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Search Fixed Asset Purchased", frm_qstr);
                    break;

                case "F70441":
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
                    SQuery = "select a.acode,b.assetname ,b.assetid,a.grpcode,c.Name as Grp_name,d.locn,d.name as Location_name,a.instdt,a.sale_dt,a.original_cost,a.invno,a.invdate,a.Ent_by,a.ent_Dt,a.Edt_by,a.edt_Dt from wb_fa_vch a,wb_fa_pur b, typegrp c,typegrp d where a.branchcd='" + mbr + "' and c.branchcd !='DD' and d.branchcd !='DD' and b.branchcd='" + mbr + "' and c.id='FA' and d.id='LF' and a.grpcode like '" + party_cd + "%' and a.locn like '" + part_cd + "%' and trim(a.grpcode)= trim(b.type1) and trim(a.locn)= trim(c.type1) and a.branchcd||trim(a.acode)||to_char(a.instdt,'dd/mm/yyyy')=b.branchcd||trim(b.acode)||to_char(b.instdt,'dd/mm/yyyy') AND A.vchDATE " + xprdrange + " order by a.assetname ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Search Fixed Asset Sold", frm_qstr);
                    break;
                case "P70107D":
                    SQuery = "SELECT A.ACODE as code,b.vencode,a.vchdate,a.invdate,a.refnum,'-' as amend,a.icode as purchasing_doc,a.iqtyin,a.irate,a.iamount,a.NO_CASES as hsncode,a.invno,a.invdate as frominvdt,a.invdate as toinvdt,a.EXC_57F4DT as fromgrdate,a.EXC_57F4DT as togrdate from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type='59' and a.vchdate " + xprdrange + " and nvl(a.refnum,'-')!='-' order by a.vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Auto Debit Credit Note Report for " + value1 + " to " + value2 + " ", frm_qstr);
                    break;
                case "P70107C":
                    SQuery = "SELECT A.ACODE as code,b.vencode,a.vchdate,a.invdate,a.refnum,'-' as amend,a.icode as purchasing_doc,a.iqtyin,a.irate,a.iamount,a.NO_CASES as hsncode,a.invno,a.invdate as frominvdt,a.invdate as toinvdt,a.EXC_57F4DT as fromgrdate,a.EXC_57F4DT as togrdate from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type='58' and a.vchdate " + xprdrange + " and nvl(a.refnum,'-')!='-' order by a.vchnum ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Auto Debit Credit Note Report for " + value1 + " to " + value2 + " ", frm_qstr);
                    break;

                case "F70291":
                    #region
                    mq0 = "";
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); ph_tbl = new DataTable();
                    header_n = "FG Register of Trading Goods ";
                    #region
                    ph_tbl.Columns.Add("date", typeof(string));
                    ph_tbl.Columns.Add("FG_Category", typeof(string));
                    ph_tbl.Columns.Add("FG_Code", typeof(string));//ICODE
                    ph_tbl.Columns.Add("FG_Group", typeof(string)); //ITEM GROUP
                    ph_tbl.Columns.Add("Item_Description", typeof(string));
                    ph_tbl.Columns.Add("HSN_Code", typeof(string));
                    ph_tbl.Columns.Add("HSN_Wise_Rate", typeof(double));
                    ph_tbl.Columns.Add("Total_Cost_of_Finished_Goods_Per_Unit", typeof(double));
                    ph_tbl.Columns.Add("Measurable_unit", typeof(string));
                    ph_tbl.Columns.Add("Opening_FG_Qty_Bal", typeof(double));
                    ph_tbl.Columns.Add("Branch_Inward", typeof(double));
                    ph_tbl.Columns.Add("In_House_Production", typeof(double));
                    ph_tbl.Columns.Add("Sales_Return", typeof(double));//Sales Return( LESS IF THIS ISSUE TO PRODUCTION)
                    ph_tbl.Columns.Add("Total_Qty", typeof(double));//Total Qty
                    ///////                  
                    ph_tbl.Columns.Add("Type", typeof(string));
                    ph_tbl.Columns.Add("Doc_No", typeof(string));//Invoice No
                    ph_tbl.Columns.Add("Doc_Dt", typeof(string));//Invoice 
                    ph_tbl.Columns.Add("Doc_Type_of_Challan", typeof(string));//latest colm add
                    ph_tbl.Columns.Add("Chl_No", typeof(string));
                    ph_tbl.Columns.Add("Chl_Dt", typeof(string));
                    ph_tbl.Columns.Add("Credit_No", typeof(string));
                    ph_tbl.Columns.Add("Credit_dt", typeof(string));
                    ph_tbl.Columns.Add("Debit_No", typeof(string));
                    ph_tbl.Columns.Add("Debit_dt", typeof(string));
                    //////////
                    ph_tbl.Columns.Add("Intra_State_Sales", typeof(double)); //Intra State Sales
                    ph_tbl.Columns.Add("Inter_State_Sales", typeof(double));//Inter State Sales
                    ph_tbl.Columns.Add("Export_Sales", typeof(double)); //4F
                    ph_tbl.Columns.Add("Sez_Supplies", typeof(double));//Sez Supplies//4E
                    ph_tbl.Columns.Add("Inter_branch_transfer", typeof(double)); //29 TYPE
                    ph_tbl.Columns.Add("Delivery_Challan", typeof(double));  //Delivery challan ( EXHIBITION/SALE ON APPROVAL BASIS)
                    ph_tbl.Columns.Add("Merchant_Export", typeof(double));//Merchant Export(0.1 % sale)
                    ph_tbl.Columns.Add("Goods_Lost", typeof(double));
                    ph_tbl.Columns.Add("Stolen", typeof(double));
                    ph_tbl.Columns.Add("Destroyed", typeof(double));
                    ph_tbl.Columns.Add("Written_Off", typeof(double));
                    ph_tbl.Columns.Add("Free_Sample_Gift", typeof(double));
                    ph_tbl.Columns.Add("Perssonal_use", typeof(double));
                    ph_tbl.Columns.Add("EOU_Other_Deemed_Supply", typeof(double));
                    ph_tbl.Columns.Add("Production_Reissue", typeof(double));
                    ph_tbl.Columns.Add("Closing_Stk_Qty", typeof(double));
                    ph_tbl.Columns.Add("Closing_Value_of_Stock", typeof(double));
                    ph_tbl.Columns.Add("Taxable_Value", typeof(double));
                    ph_tbl.Columns.Add("CGST", typeof(double));
                    ph_tbl.Columns.Add("SGST", typeof(double));
                    ph_tbl.Columns.Add("IGST", typeof(double));
                    ph_tbl.Columns.Add("Total_Invoice_Value", typeof(double));
                    #endregion
                    #region
                    #region condition
                    cond = ""; cond1 = "";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length < 1)
                    {
                        if (co_cd == "VITR")
                        {
                            cond = " and substr(trim(a.icode),1,1)='9' and substr(trim(a.icode),1,2)!='97'";
                            cond1 = " and substr(trim(icode),1,1)='9' and substr(trim(icode),1,2)!='97'";
                        }
                        else
                        {
                            cond = " and substr(trim(a.icode),1,1)='9'";
                            cond1 = " and substr(trim(icode),1,1)='9'";
                        }
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    #endregion
                    SQuery = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode"; //25/02/2019
                    //select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2018  as opening,0 as cdr,0 as ccr from itembal where branchcd='02'  and length(trim(icode))>4  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='02' AND VCHDATE between to_Date('01/04/2018','dd/mm/yyyy') and to_date('04/02/2019','dd/mm/yyyy')-1  and icode like '9190301045%'  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='02' AND vchdate between to_Date('04/02/2019','dd/mm/yyyy') and to_date('04/02/2019','dd/mm/yyyy')-1 and store='Y'  and icode like '9190301045%' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode)  GROUP BY A.ICODE,trim(b.iname),b.unit,b.irate,substr(a.icode,1,4),b.hscode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //op and cl bal  query

                    mq0 = "select distinct trim(a.icode) as icode,a.iname,substr(trim(a.icode),1,4) as scode,A.UNIT,A.IRATE,nvl(a.icost,0) as cost_of_fgood,b.iname as sname,substr(trim(a.icode),1,5) as fg_grp,a.hscode from item a ,item b where length(trim(a.icode))>=8 " + cond + " and substr(trim(a.icode),1,4)=trim(b.icode) and length(trim(b.icode))='4' order by icode"; //icode like '9%'
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);//item dt   

                    //  mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,sum(iqtyin) as qtyin,SUM(iqtyout) as qty,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y','R','N')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate ORDER BY vdd,TYPE,vchnum,icode asc";
                    mq1 = "select branchcd,type,vchnum,vchdate,vdd,trim(icode) as icode,iopr,mattype,sum(iqtyin) as qtyin,SUM(iqtyout) as qty,srno,sum(iqty_chl) as qty_chl,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  FROM (select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyin,iqtyout,iqty_chl,iamount ,exc_amt,exc_rate ,srno from ivoucher where branchcd='" + mbr + "' and type like '%'  AND TYPE NOT IN ('58','59') and vchdate  " + xprdrange + " " + cond1 + "  and store='Y' UNION ALL select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyin,iqtyout,iqty_chl,iamount ,exc_amt,exc_rate,srno from ivoucher where branchcd='" + mbr + "' and type IN ('58','59') and vchdate  " + xprdrange + " " + cond1 + " ) group by branchcd,type,vchnum,vchdate,VDD ,iopr,mattype,trim(icode),exc_rate,srno ORDER BY vdd,vchnum,type,srno asc";//new query as per mayuri mam
                    //is qry me store 'N' add kiya hai qki credit note ki entry show ni hori thi...need ask to sir
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);//main dt for loop

                    mq2 = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.cdr+a.ccr)=0 and sum(a.opening)!=0 order by icode"; //25/02/2019
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq2);//stk dt in for that item which is not covered anywhere

                    mq3 = "select trim(icode) as icode,sum(iqtyin) as qty,type,sum(iamount) as amt from ivoucher where  branchcd='" + mbr + "' and type >='15' and substr(type,1,1)='1' " + cond1 + " and vchdate " + xprdrange + " group by trim(icode),type order by type,icode";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq3);//in house production

                    mq4 = "select distinct TYPE1,acref, (CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' order by type1";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq4);

                    mq5 = "select trim(a.type1) as fg_Code,trim(a.name) as category from typegrp A where A.id='YX'";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq5); //FOR CATEGORY MASTER

                    mq7 = "select TYPE1,NAME from type where id='M' AND TYPE1 LIKE '2%'";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7); //FOR CHALLAN

                    mq8 = "select TYPE1,NAME,substr(trim(nAME),1,3)||'/'||TRIM(TYPE1) AS FF,TRIM(TYPE1)||'/'||TRIM(NAME) AS TYPE_NAME  from type where type1 like '4%' and id='V' ORDER BY TYPE1";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8); //only type like '4%'

                    mq9 = "select TYPE1,NAME,TBRANCHCD,substr(trim(nAME),1,3)||'/'||TRIM(TYPE1) AS FF,TRIM(TYPE1)||'/'||TRIM(NAME) AS TYPE_NAME  from type where id= 'M' and substr(type1,1,1) in ('0','1','2','3','5')  ORDER BY TYPE1";
                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9); // for typename
                    #endregion
                    mq2 = ""; db_op = 0;

                    if (dt2.Rows.Count > 0)
                    {
                        DataView View1 = new DataView(dt2);
                        dt11 = new DataTable();
                        dt11 = View1.ToTable(true, "icode");
                        foreach (DataRow dr in dt11.Rows)
                        {
                            DataView View2 = new DataView(dt2, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = new DataTable();
                            dt10 = View2.ToTable();
                            db_op = 0; //for opening only
                            for (int i = 0; i < dt10.Rows.Count; i++)
                            {
                                #region
                                mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0;
                                db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db23 = 0;
                                hscode = "";
                                mq2 = dt10.Rows[i]["type"].ToString().Trim();
                                mq3 = dt10.Rows[i]["iopr"].ToString().Trim();
                                mq5 = dt10.Rows[i]["exc_rate"].ToString().Trim();
                                mq6 = dt10.Rows[i]["mattype"].ToString().Trim(); //for new develop fields
                                //=====================================
                                dr1 = ph_tbl.NewRow();
                                dr1["date"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                dr1["FG_Code"] = dt10.Rows[i]["ICODE"].ToString().Trim();
                                dr1["FG_Group"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "FG_GRP");
                                dr1["FG_Category"] = fgen.seek_iname_dt(dt6, "fg_Code='" + dr1["FG_Group"].ToString().Trim() + "'", "category");
                                dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "iname");
                                //dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                                //db21 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                                dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                                hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                                db21 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + hscode.Trim() + "'", "hs_rate"));
                                dr1["HSN_Wise_Rate"] = db21;
                                dr1["Total_Cost_of_Finished_Goods_Per_Unit"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "IRATE"));
                                dr1["Measurable_unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "unit");
                                if (i == 0)
                                {
                                    dr1["Opening_FG_Qty_Bal"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "opening")), 2);
                                }
                                else
                                {
                                    dr1["Opening_FG_Qty_Bal"] = Math.Round(db_op, 2);
                                    // db_op = 0;
                                }
                                switch (mq2)
                                {
                                    #region for type 4
                                    case "40":
                                    case "41":
                                    case "43":
                                    case "44":
                                    case "45":
                                    case "46":
                                    case "49":
                                    case "4A":
                                    case "4K":
                                    case "4L":
                                    case "4T":
                                    case "4W":
                                    case "4Y":
                                    case "4Z":
                                    case "4[":
                                    case "4{":
                                    case "4^":
                                    case "4_":
                                    case "4`":
                                    case "4]":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "48":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        dr1["EOU_Other_Deemed_Supply"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                        // mq4 = dt2.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "42":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Merchant_Export"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "4X":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Free_Sample_Gift"] = fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "4E":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Sez_Supplies"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        //  mq4 = dt2.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "4F":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Export_Sales"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);///change recently
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        //   mq4 = dt2.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    #endregion
                                    #region  FOR CHALLAN type like 2 switch case
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "25":
                                    case "27":
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Chl_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Chl_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Doc_Type_of_Challan"] = fgen.seek_iname_dt(dt7, "type1='" + mq2 + "'", "NAme");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "24":
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Chl_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Chl_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Delivery_Challan"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        dr1["Doc_Type_of_Challan"] = fgen.seek_iname_dt(dt7, "type1='" + mq2 + "'", "NAme");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "29":
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Chl_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Chl_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Inter_branch_transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        dr1["Doc_Type_of_Challan"] = fgen.seek_iname_dt(dt7, "type1='" + mq2 + "'", "NAme");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "50"://for opening only   
                                    case "51":
                                    case "52":
                                    case "53":
                                    case "54":
                                    case "55":
                                    case "57":
                                    case "5A":
                                    case "5B":
                                        dr1["Type"] = "Production_re-issue";// fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    #region credit /debit note
                                    case "58":
                                        dr1["Type"] = "Credit Note";//fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Credit_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Credit_dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim());// - fgen.make_double(dt10.Rows[i]["qty_chl"].ToString().Trim()); //FOR OPENING
                                        db = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "59":
                                        dr1["Type"] = "Debit Note";//fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Debit_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Debit_dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim());// - fgen.make_double(dt10.Rows[i]["qty_chl"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    #endregion
                                    case "04":
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Sales_Return"] = dt10.Rows[i]["qtyin"].ToString().Trim();///old logic
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "0U":
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Branch_Inward"] = dt10.Rows[i]["qtyin"].ToString().Trim();///old logic
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #region for in house production
                                    case "15":
                                    case "16":
                                    case "17":
                                    case "18":
                                    case "19":
                                    case "1A":
                                    case "1B":
                                    case "1C":
                                    case "1D":
                                    case "1E":
                                    case "1F":
                                    case "1G":
                                    case "1H":
                                    case "1J":
                                    case "1M":
                                    case "1N":
                                    case "1O":
                                    case "1S":
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["In_House_Production"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    #region for issue type
                                    case "30":
                                    case "31":
                                    case "33":
                                    case "36":
                                    case "37":
                                    case "38":
                                    case "39":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    case "66":
                                        #region for new develop fields
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        switch (mq6)
                                        {
                                            case "14":
                                                dr1["Destroyed"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                            case "15":
                                                dr1["Stolen"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                break;
                                            case "16":
                                                dr1["Goods_Lost"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                            case "17":
                                                dr1["Written_Off"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                            case "18":
                                                dr1["Perssonal_use"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                        }
                                        #endregion
                                        break;
                                }
                                switch (mq4)
                                {
                                    case "CG":
                                        if (mq2 == "4X" || mq6 == "14" || mq6 == "15" || mq6 == "16" || mq6 == "17" || mq6 == "18")
                                        {
                                            dr1["CGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                            dr1["SGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Intra_State_Sales"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                            dr1["CGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                            dr1["SGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        break;
                                    case "IG":
                                        if (mq2 == "4X" || mq2 == "42")
                                        {
                                            dr1["IGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Inter_State_Sales"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                            dr1["IGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        break;
                                }
                                //switch (mq5)
                                //{                          
                                //    case "0.1":
                                //        dr1["Merchant_Export"] = db1; //if sa;e type 42 and rate is 0.1 then merhant export
                                //        dr1["TYPE"] = dt2.Rows[i]["TYPE"].ToString().Trim();
                                //        break;
                                //}
                                dr1["Total_Invoice_Value"] = Math.Round(fgen.make_double(dr1["Taxable_Value"].ToString().Trim()) + fgen.make_double(dr1["CGST"].ToString().Trim()) + fgen.make_double(dr1["SGST"].ToString().Trim()) + fgen.make_double(dr1["IGST"].ToString().Trim()), 2);
                                dr1["Total_Qty"] = Math.Round(fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dr1["In_House_Production"].ToString().Trim()) + fgen.make_double(dr1["Sales_Return"].ToString().Trim()) + fgen.make_double(dr1["Branch_Inward"].ToString().Trim()), 2);
                                dr1["Branch_Inward"] = Math.Round(fgen.make_double(dr1["Branch_Inward"].ToString().Trim()), 2);//hold...need to ask   
                                db2 = Math.Round(fgen.make_double(dr1["Total_Qty"].ToString().Trim()), 2);
                                ///============
                                db3 = Math.Round(fgen.make_double(dr1["Intra_State_Sales"].ToString().Trim()), 2);
                                db4 = Math.Round(fgen.make_double(dr1["Inter_State_Sales"].ToString().Trim()), 2);
                                db5 = Math.Round(fgen.make_double(dr1["Export_Sales"].ToString().Trim()), 2);
                                db6 = Math.Round(fgen.make_double(dr1["Sez_Supplies"].ToString().Trim()), 2);
                                db7 = Math.Round(fgen.make_double(dr1["Inter_branch_transfer"].ToString().Trim()), 2);
                                db8 = Math.Round(fgen.make_double(dr1["Delivery_Challan"].ToString().Trim()), 2);
                                db9 = Math.Round(fgen.make_double(dr1["Merchant_Export"].ToString().Trim()), 2);
                                db10 = Math.Round(fgen.make_double(dr1["Goods_Lost"].ToString().Trim()), 2);
                                db11 = Math.Round(fgen.make_double(dr1["Stolen"].ToString().Trim()), 2);
                                db12 = Math.Round(fgen.make_double(dr1["Destroyed"].ToString().Trim()), 2);
                                db13 = Math.Round(fgen.make_double(dr1["Written_Off"].ToString().Trim()), 2);
                                db14 = Math.Round(fgen.make_double(dr1["Free_Sample_Gift"].ToString().Trim()), 2);
                                db15 = Math.Round(fgen.make_double(dr1["Perssonal_use"].ToString().Trim()), 2);
                                db16 = Math.Round(fgen.make_double(dr1["EOU_Other_Deemed_Supply"].ToString().Trim()), 2);
                                db23 = Math.Round(fgen.make_double(dr1["Production_Reissue"].ToString().Trim()), 2);
                                db18 = db3 + db4 + db5 + db6 + db7 + db8 + db9 + db10 + db11 + db12 + db13 + db14 + db15 + db16 + db17 + db18 + db23;
                                db19 = db2 - db18;
                                dr1["Closing_Stk_Qty"] = Math.Round(db19, 2);
                                // db_op = db19; //opening stk qty..OLD LOGIC
                                // db_op = db_op + fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim());
                                db20 = db19 * Math.Round(fgen.make_double(dr1["Total_Cost_of_Finished_Goods_Per_Unit"].ToString().Trim()), 2);
                                dr1["Closing_Value_of_Stock"] = db20;
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                        }
                    }
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        #region
                        dr1 = ph_tbl.NewRow(); db22 = 0; string chk_tran = ""; hscode = "";
                        chk_tran = fgen.seek_iname_dt(dt2, "trim(icode)='" + dt3.Rows[i]["ICODE"].ToString().Trim() + "'", "icode");
                        if (chk_tran.Length > 1)
                        {
                        }
                        else
                        {
                            dr1["date"] = "No transaction";// dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["FG_Code"] = dt3.Rows[i]["ICODE"].ToString().Trim();
                            mq6 = dt3.Rows[i]["ICODE"].ToString().Trim().Substring(0, 5);
                            dr1["FG_Group"] = mq6;// fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "FG_GRP");
                            dr1["FG_Category"] = fgen.seek_iname_dt(dt6, "fg_Code='" + mq6 + "'", "category");
                            dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "iname");
                            // dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                            //  mq6 = fgen.seek_iname(frm_qstr, co_cd, "select distinct TYPE1, (CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' and acref='" + dr1["HSN_Code"].ToString().Trim() + "' order by type1", "hs_rate");
                            //db22 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                            dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                            hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                            db22 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + hscode.Trim() + "'", "hs_rate"));
                            dr1["HSN_Wise_Rate"] = db22;
                            dr1["Total_Cost_of_Finished_Goods_Per_Unit"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "irate"));
                            dr1["Measurable_unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "unit");
                            dr1["Opening_FG_Qty_Bal"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()), 2);
                            dr1["Closing_Stk_Qty"] = Math.Round(fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()) - fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            ph_tbl.Rows.Add(dr1);
                        }
                        #endregion
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("" + header_n + " Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F70293":
                    #region
                    #region
                    dt2 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                    dt5 = new DataTable(); dt6 = new DataTable(); dt7 = new DataTable(); dt8 = new DataTable();
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("Date", typeof(string));
                    ph_tbl.Columns.Add("Category", typeof(string));
                    ph_tbl.Columns.Add("Item_Code", typeof(string));
                    ph_tbl.Columns.Add("Item_Description", typeof(string));
                    ph_tbl.Columns.Add("HSN_Code", typeof(string));
                    ph_tbl.Columns.Add("Applicable_GST_Rate", typeof(double));
                    ph_tbl.Columns.Add("Weighted_Average_Cost_of_Goods", typeof(double)); //pic irate from item as on 16 march 2019
                    ph_tbl.Columns.Add("Measurable_Unit", typeof(string));
                    ph_tbl.Columns.Add("Opening_Balance", typeof(double));
                    ////////////
                    ph_tbl.Columns.Add("Inward_Doc_No", typeof(string));
                    ph_tbl.Columns.Add("Inward_Doc_Dt", typeof(string));
                    //ph_tbl.Columns.Add("Inv_No", typeof(string));//Invoice No
                    //ph_tbl.Columns.Add("Inv_Dt", typeof(string));//Invoice 
                    //ph_tbl.Columns.Add("Chl_No", typeof(string));
                    //ph_tbl.Columns.Add("Chl_Dt", typeof(string));
                    //ph_tbl.Columns.Add("Credit_No", typeof(string));
                    //ph_tbl.Columns.Add("Credit_dt", typeof(string));
                    //ph_tbl.Columns.Add("Debit_No", typeof(string));
                    //ph_tbl.Columns.Add("Debit_dt", typeof(string));
                    ////inward Movement
                    ph_tbl.Columns.Add("Intra_State", typeof(double));
                    ph_tbl.Columns.Add("Out_of_State", typeof(double));
                    ph_tbl.Columns.Add("Import", typeof(double));
                    ph_tbl.Columns.Add("Branch_Inward_Transfer", typeof(double));
                    ph_tbl.Columns.Add("Received_from_Jobworker_Inw", typeof(double));
                    ph_tbl.Columns.Add("Production_Reissue", typeof(double));
                    ph_tbl.Columns.Add("Total_Inw_Supply", typeof(double));
                    ph_tbl.Columns.Add("Total_Stock", typeof(double));
                    ////ISSUE To production
                    ph_tbl.Columns.Add("Document_No", typeof(string));
                    ph_tbl.Columns.Add("Quantity_Production_Issue", typeof(double));//rename  Quantity to Quantity_Production_Issue
                    /////////////////outward portion==============================
                    ph_tbl.Columns.Add("Inv_No_", typeof(string));//Invoice No
                    ph_tbl.Columns.Add("Inv_Dt_", typeof(string));//Invoice 
                    ph_tbl.Columns.Add("Chl_No_", typeof(string));
                    ph_tbl.Columns.Add("Chl_Dt_", typeof(string));
                    ph_tbl.Columns.Add("Credit_No_", typeof(string));
                    ph_tbl.Columns.Add("Credit_dt_", typeof(string));
                    ph_tbl.Columns.Add("Debit_No_", typeof(string));
                    ph_tbl.Columns.Add("Debit_dt_", typeof(string));
                    ph_tbl.Columns.Add("Purchase_Return", typeof(double));
                    ///outward movement
                    ph_tbl.Columns.Add("Branch_Outward_Transfer", typeof(double));
                    ph_tbl.Columns.Add("Sent_for_Jobwork", typeof(double));
                    ph_tbl.Columns.Add("Goods_Lost", typeof(double));
                    ph_tbl.Columns.Add("Stolen", typeof(double));
                    ph_tbl.Columns.Add("Destroyed", typeof(double));
                    ph_tbl.Columns.Add("Written_Off", typeof(double));
                    ph_tbl.Columns.Add("Free_Sample_Gift", typeof(double));
                    ph_tbl.Columns.Add("Personal_Use", typeof(double));
                    //ph_tbl.Columns.Add("Production_Reissue", typeof(double));
                    ph_tbl.Columns.Add("Total_Outwar_Supply_of_RM", typeof(double));
                    ph_tbl.Columns.Add("Closing_Balance", typeof(double));
                    ph_tbl.Columns.Add("Closing_Stock_Value", typeof(double));
                    ph_tbl.Columns.Add("Taxable_Value", typeof(double));
                    ph_tbl.Columns.Add("CGST", typeof(double));
                    ph_tbl.Columns.Add("SGST", typeof(double));
                    ph_tbl.Columns.Add("IGST", typeof(double));
                    ph_tbl.Columns.Add("Total_Invoice", typeof(double));

                    header_n = "RM-Stock Register";
                    #endregion
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    cond1 = ""; cond = "";
                    #region
                    //if no selection any box
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length < 1)
                    {
                        cond = " and substr(trim(a.icode),1,1)<'4'";
                        cond1 = " and substr(trim(icode),1,1)<'4'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    #endregion
                    //======================================
                    SQuery = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode"; //25/02/2019                    
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //op and cl bal  query

                    mq0 = "select distinct trim(a.icode) as icode,a.iname,substr(trim(a.icode),1,4) as scode,A.UNIT,A.IRATE,nvl(a.icost,0) as cost_of_fgood,b.iname as sname,substr(trim(a.icode),1,5) as fg_grp,a.hscode from item a ,item b where length(trim(a.icode))>=8  and substr(trim(a.icode),1,1)<'9' " + cond + " and substr(trim(a.icode),1,4)=trim(b.icode) and length(trim(b.icode))='4' order by icode";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);//item dt                                                     

                    //mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,SUM(iqtyout) as qtyout,sum(iqtyin) as iqtyin,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y','R')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate,irate ORDER BY vdd,icode asc"; //store in Y & R ...OLD
                    //mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,SUM(iqtyout) as qtyout,sum(iqtyin) as iqtyin,sum(iqty_chl) as qty_chl,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate,irate ORDER BY vdd,icode asc"; //remove store=r.....AS PER MAYYURI MAM
                    //  mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,srno,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyout as qtyout,iqtyin as iqtyin,iqty_chl as qty_chl,iamount as iamount /*,exc_amt as sgst_Amt*/,exc_rate,(Case when trim(Unit)='CG' then exc_Rate else 0 end) as CGST_RT,(Case when trim(Unit)='CG' then exc_Amt else 0 end) as CGST_amt,(Case when trim(Unit)='CG' then cess_percent else 0 end) as SGST_Rate,(Case when trim(Unit)='CG' then cess_pu else 0 end) as SGST_amt,(Case when trim(Unit)='IG' then exc_rate else 0 end) as IGST_Rt,(Case when trim(Unit)='IG' then exc_amt else 0 end) as IGST_amt,invno,to_char(invdate,'dd/mm/yyyy') as invdate,TRIM(UNIT) AS UNIT,nvl(trim(cavity),0) as cavity,acpt_ud as Accept  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')   ORDER BY vdd,vchnum,type,srno asc"; //remove store=r.....AS PER MAYYURI MAM ///////////////old as per 12 april 19
                    mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,srno,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,post,mattype,iqtyout as qtyout,iqtyin as iqtyin,iqty_chl as qty_chl,iamount as iamount /*,exc_amt as sgst_Amt*/,exc_rate,(Case when trim(Unit)='CG' then exc_Rate else 0 end) as CGST_RT,(Case when trim(iopr)='CG' then exc_Rate else 0 end) as CGST_RT_,(Case when trim(post)=1 then exc_Rate else 0 end) as CGST_RT_1,(Case when trim(Unit)='CG' then exc_Amt else 0 end) as CGST_amt,(Case when trim(iopr)='CG' then exc_Amt else 0 end) as CGST_amt_,(Case when trim(post)=1 then exc_Amt else 0 end) as CGST_amt_1,(Case when trim(Unit)='CG' then cess_percent else 0 end) as SGST_Rate,(Case when trim(iopr)='CG' then cess_percent else 0 end) as SGST_Rate_,(Case when trim(post)=1 then cess_percent else 0 end) as SGST_Rate_1,(Case when trim(Unit)='CG' then cess_pu else 0 end) as SGST_amt,(Case when trim(iopr)='CG' then cess_pu else 0 end) as SGST_amt_,(Case when trim(post)=1 then cess_pu else 0 end) as SGST_amt_1,(Case when trim(Unit)='IG' then exc_rate else 0 end) as IGST_Rt,(Case when trim(iopr)='IG' then exc_rate else 0 end) as IGST_Rt_,(Case when trim(post)=2 then exc_rate else 0 end) as IGST_Rt_1,(Case when trim(Unit)='IG' then exc_amt else 0 end) as IGST_amt,(Case when trim(iopr)='IG' then exc_amt else 0 end) as IGST_amt_,(Case when trim(post)=2 then exc_amt else 0 end) as IGST_amt_1,invno,to_char(invdate,'dd/mm/yyyy') as invdate,TRIM(UNIT) AS UNIT,nvl(trim(cavity),0) as cavity,acpt_ud as Accept  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')   ORDER BY vdd,type,vchnum,srno asc";//new as on 12 apr 19..
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);//main dt for loop

                    mq2 = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.cdr+a.ccr)=0 and sum(a.opening)!=0 order by icode"; //25/02/2019
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq2);//stk dt in for that item which is not covered anywhere 

                    mq3 = "select distinct TYPE1,acref,(CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' order by type1";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq3);

                    mq5 = "select trim(a.ICODE) as fg_Code,trim(a.Iname) as category from ITEM A where LENGTH(trim(a.ICODE)) =4";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq5); //FOR CATEGORY MASTER

                    // mq6 = "Select A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.iamount,round(a.exp_punit,2) as Txb_Chgs,a.unit as TX_type,(Case when trim(A.Unit)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,a.Location as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,TRIM(A.UNIT) AS UNIT from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno";
                    mq6 = "Select A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.iamount,round(a.exp_punit,2) as Txb_Chgs,a.unit as TX_type,(Case when trim(A.Unit)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,a.Location as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,TRIM(A.UNIT) AS UNIT,nvl(trim(a.cavity),0) as cavity from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq6); //hsn wise purcvhase(inward data).............gst module 

                    mq7 = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ to_char(a.vchdate,'DD/MM/YYYY') as Dated,a.Vchnum as MRR_No,b.aname as Supplier,trim(a.invno)||','||trim(a.refnum) as Bill_Chl,c.iname as Item_Name,c.unit,a.iqty_chl as Advised,a.iqtyin+nvl(rej_rw,0) as Rcvd,a.acpt_ud as Accept,a.rej_rw as Reject,a.irate,a.ichgs as Lc,C.cpartno as Code,a.Btchno as Batchno,a.btchdt as Batch_Dt,a.finvno,a.Type,a.tc_no as TC_NO,a.ponum as P_O_No,a.Genum as Gate_Entry,a.gedate as Gate_Date,a.Ent_by,a.Pname as Insp_By,a.Qcdate,a.icode,a.store,a.Mode_tpt,a.Mtime,a.mfgdt,a.expdt,b.addr3,b.rc_num,b.addr1,b.addr2,a.rgpnum,a.rgpdate,a.freight as cl_by,a.o_Deptt,a.st_entform as ewaybillno  from ivoucher a, famst b , item c where a.branchcd='" + mbr + "' and A.type like '0%' and a.vchdate  " + xprdrange + " and a.store<>'R' and TRIM(a.icode)=TRIM(c.icode) and trim(a.acode)=trim(B.acode) and 1=1  order by vchdate,type,vchnum,srno";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq7); //query for mrr report in Inventory module

                    if (dt2.Rows.Count > 0)
                    {
                        DataView View1 = new DataView(dt2);
                        dt9 = new DataTable();
                        dt9 = View1.ToTable(true, "icode");
                        foreach (DataRow dr in dt9.Rows)
                        {
                            DataView View2 = new DataView(dt2, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = new DataTable();
                            dt10 = View2.ToTable();
                            db_op = 0; //for opening only
                            for (int i = 0; i < dt10.Rows.Count; i++)
                            {
                                #region
                                mq2 = ""; mq6 = ""; db10 = 0; string unit = ""; hscode = "";
                                mq2 = dt10.Rows[i]["type"].ToString().Trim();
                                mq6 = dt10.Rows[i]["mattype"].ToString().Trim(); //for new develop fields
                                dr1 = ph_tbl.NewRow();
                                dr1["date"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                dr1["Item_Code"] = dt10.Rows[i]["ICODE"].ToString().Trim();
                                dr1["Category"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "sname");
                                dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iname");
                                //dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                                //   mq6 = fgen.seek_iname(frm_qstr, co_cd, "select distinct TYPE1, (CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' and acref='" + dr1["HSN_Code"].ToString().Trim() + "' order by type1", "hs_rate");
                                //db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                                dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                                hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                                db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + hscode.Trim() + "'", "hs_rate"));
                                dr1["Applicable_GST_Rate"] = db10;
                                dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IRATE"));
                                dr1["Measurable_Unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                if (i == 0)
                                {
                                    dr1["Opening_Balance"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "opening")), 2);
                                }
                                else
                                {
                                    dr1["Opening_Balance"] = Math.Round(db_op, 2);
                                }
                                switch (mq2)
                                {
                                    #region inward portion
                                    #region mrr
                                    case "02":
                                    case "03":
                                    case "04":
                                    case "05":
                                    case "06":
                                    case "08":
                                    case "0B":
                                    case "0D":
                                        //// dr1["Inward_Doc_No"] = dt2.Rows[i]["Vchnum"].ToString().Trim();
                                        //// dr1["Inward_Doc_Dt"] = dt2.Rows[i]["vchdate"].ToString().Trim();
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT"));
                                        //dr1["SGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT"));
                                        //dr1["IGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT"));
                                        ////  dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //// dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        //unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        //if (unit == "IG")
                                        //{
                                        //    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}
                                        //else
                                        //{
                                        //    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}

                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        unit = dt10.Rows[i]["unit"].ToString().Trim();
                                        if (unit == "IG")
                                        {
                                            dr1["Out_of_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Intra_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        break;
                                    case "07":
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //double amt = 0, rate = 0;
                                        //amt = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount"));
                                        //rate = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "cavity"));
                                        //dr1["Taxable_Value"] = Math.Round(amt * rate, 2);
                                        ////dr1["CGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT")), 2);
                                        ////dr1["SGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT")), 2);
                                        //dr1["IGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT")), 2);
                                        ////  dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //dr1["import"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        ////dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////   dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        ////unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        ////if (unit == "IG")
                                        ////{
                                        ////    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        ////else
                                        ////{
                                        ////    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim());//Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()) * fgen.make_double(dt10.Rows[i]["cavity"].ToString().Trim()), 2);
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        dr1["import"] = fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim());
                                        break;
                                    case "0C":
                                        //// dr1["Inward_Doc_No"] = dt2.Rows[i]["Vchnum"].ToString().Trim();
                                        //// dr1["Inward_Doc_Dt"] = dt2.Rows[i]["vchdate"].ToString().Trim();
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT"));
                                        //dr1["SGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT"));
                                        //dr1["IGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT"));
                                        ////  dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //// dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        dr1["Branch_Inward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        break;
                                    case "0U":
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT")), 2);
                                        //dr1["SGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT")), 2);
                                        //dr1["IGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT")), 2);
                                        //// dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        dr1["Branch_Inward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        //  dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        //unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        //if (unit == "IG")
                                        //{
                                        //    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}
                                        //else
                                        //{
                                        //    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}

                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        unit = dt10.Rows[i]["unit"].ToString().Trim();
                                        if (unit == "IG")
                                        {
                                            dr1["Out_of_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Intra_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        break;
                                    case "09":
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT")), 2);
                                        //dr1["SGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT")), 2);
                                        //dr1["IGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT")), 2);
                                        //// dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        dr1["Received_from_Jobworker_Inw"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        ////dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        ////unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        ////if (unit == "IG")
                                        ////{
                                        ////    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        ////else
                                        ////{
                                        ////    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        break;

                                    #endregion
                                    #region for type like 1
                                    case "10":
                                    case "11":
                                    case "12":
                                    case "13":
                                    case "14":
                                        //dr1["Document_No"] = dt10.Rows[i]["Vchnum"].ToString().Trim();//old
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["Vchnum"].ToString().Trim();//26.04.19...new
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();//26.04.19
                                        // dr1["Quantity"] = Math.Round(fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()), 2);
                                        dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING                                      
                                        break;
                                    #endregion
                                    #region issue to production
                                    case "30":
                                    case "31":
                                    case "33":
                                    case "36":
                                    case "37":
                                    case "38":
                                    case "39":
                                        dr1["Document_No"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Quantity_Production_Issue"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    #region challan outward case
                                    case "21":
                                        dr1["Chl_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Chl_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Sent_for_Jobwork"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);//ADD ON 12 APR 19 EVG 5:30..PENDING TO MERGE ..ONLY THIS SINGLE LINE
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_1"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_1"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_1"].ToString().Trim());
                                        break;
                                    case "22":
                                    case "23":
                                    case "24":
                                    case "25":
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);//26.04.2019
                                        dr1["Chl_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Chl_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Branch_Outward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_1"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_1"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_1"].ToString().Trim());
                                        break;
                                    case "29":
                                        dr1["Chl_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Chl_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Branch_Outward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_1"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_1"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_1"].ToString().Trim());
                                        break;
                                    #endregion
                                    #region for outward invoice
                                    #region invoice
                                    case "40":
                                    case "41":
                                    case "42":
                                    case "43":
                                    case "44":
                                    case "45":
                                    case "46":
                                    case "48":
                                    case "49":
                                    case "4A":
                                    case "4B":
                                    case "4C":
                                    case "4D":
                                    case "4E":
                                    case "4F":
                                    case "4G":
                                    case "4J":
                                    case "4K":
                                    case "4L":
                                    case "4T":
                                    case "4U":
                                    case "4V":
                                    case "4W":
                                    case "4X":
                                    case "4Y":
                                    case "4Z":
                                    case "4[":
                                    case "4{":
                                    case "4]":
                                    case "4^":
                                    case "4_":
                                    #endregion
                                    case "4`":
                                        dr1["Inv_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Inv_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "47":
                                        dr1["Inv_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Inv_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Taxable_Value"] = fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()); //...old logic
                                        dr1["Purchase_Return"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    #endregion
                                    #region credit /debit note
                                    case "58":
                                        dr1["Credit_No_"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Credit_dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        break;
                                    case "59":
                                        dr1["Debit_No_"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Debit_dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        break;
                                    #endregion
                                    case "66"://yaha abi data ni h to not sure ki qty kis field me jayegi so ryt now picking iamount
                                        #region for new develop fields
                                        switch (mq6)
                                        {
                                            case "13":
                                                dr1["Destroyed"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "14":
                                                dr1["Destroyed"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "15":
                                                dr1["Stolen"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "16":
                                                dr1["Goods_Lost"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "17":
                                                dr1["Written_Off"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "18":
                                                dr1["Personal_Use"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                        }
                                        #endregion
                                        break;
                                }
                                switch (mq4)
                                {
                                    case "CG": //inward case
                                    case "IG":
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_"].ToString().Trim());//use in outward set type like 4
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_"].ToString().Trim());//use in outward set type like 4                                                            
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_"].ToString().Trim()); //use in outward set type like 4
                                        break;
                                }
                                dr1["Total_Inw_Supply"] = Math.Round(fgen.make_double(dr1["Intra_State"].ToString().Trim()) + fgen.make_double(dr1["Out_of_State"].ToString().Trim()) + fgen.make_double(dr1["import"].ToString().Trim()) + fgen.make_double(dr1["Branch_Inward_Transfer"].ToString().Trim()) + fgen.make_double(dr1["Received_from_Jobworker_Inw"].ToString().Trim()) + fgen.make_double(dr1["Production_Reissue"].ToString().Trim()), 2);
                                dr1["Total_Stock"] = Math.Round(fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dr1["Total_Inw_Supply"].ToString().Trim()), 2);
                                dr1["Total_Outwar_Supply_of_RM"] = Math.Round(fgen.make_double(dr1["Purchase_Return"].ToString().Trim()) + fgen.make_double(dr1["Branch_Outward_Transfer"].ToString().Trim()) + fgen.make_double(dr1["Sent_for_Jobwork"].ToString().Trim()) + fgen.make_double(dr1["Goods_Lost"].ToString().Trim()) + fgen.make_double(dr1["Stolen"].ToString().Trim()) + fgen.make_double(dr1["Destroyed"].ToString().Trim()) + fgen.make_double(dr1["Written_Off"].ToString().Trim()) + fgen.make_double(dr1["Personal_Use"].ToString().Trim()) + fgen.make_double(dr1["Free_Sample_Gift"].ToString().Trim()), 2);
                                if (mq2 == "10" || mq2 == "11" || mq2 == "12" || mq2 == "13" || mq2 == "14")
                                {
                                    dr1["Closing_Balance"] = Math.Round(fgen.make_double(dr1["Total_Stock"].ToString().Trim()) + fgen.make_double(dr1["Quantity_Production_Issue"].ToString().Trim()) - fgen.make_double(dr1["Total_Outwar_Supply_of_RM"].ToString().Trim()), 2);
                                }
                                else
                                {
                                    dr1["Closing_Balance"] = Math.Round(fgen.make_double(dr1["Total_Stock"].ToString().Trim()) - fgen.make_double(dr1["Quantity_Production_Issue"].ToString().Trim()) - fgen.make_double(dr1["Total_Outwar_Supply_of_RM"].ToString().Trim()), 2);
                                }
                                dr1["Closing_Stock_Value"] = Math.Round(fgen.make_double(dr1["Weighted_Average_Cost_of_Goods"].ToString().Trim()) * fgen.make_double(dr1["Closing_Balance"].ToString().Trim()), 2);
                                dr1["Total_Invoice"] = Math.Round(fgen.make_double(dr1["Taxable_Value"].ToString().Trim()) + fgen.make_double(dr1["CGST"].ToString().Trim()) + fgen.make_double(dr1["SGST"].ToString().Trim()) + fgen.make_double(dr1["IGST"].ToString().Trim()), 2);
                                    #endregion
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                        }
                    }
                    //this loop for that items only which are not covered anywhere
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        dr1 = ph_tbl.NewRow(); db10 = 0; string chk_tran = ""; hscode = "";
                        chk_tran = fgen.seek_iname_dt(dt2, "trim(icode)='" + dt3.Rows[i]["ICODE"].ToString().Trim() + "'", "icode");
                        if (chk_tran.Length > 1)
                        {
                        }
                        else
                        {
                            dr1["date"] = "No transaction";// dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["Item_Code"] = dt3.Rows[i]["ICODE"].ToString().Trim();
                            mq6 = dt3.Rows[i]["ICODE"].ToString().Trim().Substring(0, 4);
                            dr1["Category"] = fgen.seek_iname_dt(dt6, "fg_Code='" + mq6 + "'", "category");
                            dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iname");
                            //dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                            //db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                            dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                            hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                            db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + hscode.Trim() + "'", "hs_rate"));
                            dr1["Applicable_GST_Rate"] = db10;
                            dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IRATE"));
                            dr1["Measurable_Unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                            dr1["Opening_Balance"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()), 2);
                            dr1["Closing_Balance"] = Math.Round(fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()) - fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            ph_tbl.Rows.Add(dr1);
                        }
                    }
                    if (ph_tbl.Rows.Count > 0)
                    {
                        Session["send_dt"] = ph_tbl;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("RM Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    #endregion
                    break;

                case "F70295":
                    //FG STOCK SUMMARY
                    #region
                    mq0 = "";
                    dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable(); ph_tbl = new DataTable();
                    header_n = "FG Register of Trading Goods ";
                    #region
                    ph_tbl.Columns.Add("date", typeof(string));
                    ph_tbl.Columns.Add("FG_Category", typeof(string));
                    ph_tbl.Columns.Add("FG_Code", typeof(string));//ICODE
                    ph_tbl.Columns.Add("FG_Group", typeof(string)); //ITEM GROUP
                    ph_tbl.Columns.Add("Item_Description", typeof(string));
                    ph_tbl.Columns.Add("HSN_Code", typeof(string));
                    ph_tbl.Columns.Add("HSN_Wise_Rate", typeof(double));
                    ph_tbl.Columns.Add("Total_Cost_of_Finished_Goods_Per_Unit", typeof(double));
                    ph_tbl.Columns.Add("Measurable_unit", typeof(string));
                    ph_tbl.Columns.Add("Opening_FG_Qty_Bal", typeof(double));
                    ph_tbl.Columns.Add("Branch_Inward", typeof(double));
                    ph_tbl.Columns.Add("In_House_Production", typeof(double));
                    ph_tbl.Columns.Add("Sales_Return", typeof(double));//Sales Return( LESS IF THIS ISSUE TO PRODUCTION)
                    ph_tbl.Columns.Add("Total_Qty", typeof(double));//Total Qty
                    ///////                  
                    ph_tbl.Columns.Add("Type", typeof(string));
                    ph_tbl.Columns.Add("Doc_No", typeof(string));//Invoice No
                    ph_tbl.Columns.Add("Doc_Dt", typeof(string));//Invoice 
                    ph_tbl.Columns.Add("Doc_Type_of_Challan", typeof(string));//latest colm add
                    ph_tbl.Columns.Add("Chl_No", typeof(string));
                    ph_tbl.Columns.Add("Chl_Dt", typeof(string));
                    ph_tbl.Columns.Add("Credit_No", typeof(string));
                    ph_tbl.Columns.Add("Credit_dt", typeof(string));
                    ph_tbl.Columns.Add("Debit_No", typeof(string));
                    ph_tbl.Columns.Add("Debit_dt", typeof(string));
                    //////////
                    ph_tbl.Columns.Add("Intra_State_Sales", typeof(double)); //Intra State Sales
                    ph_tbl.Columns.Add("Inter_State_Sales", typeof(double));//Inter State Sales
                    ph_tbl.Columns.Add("Export_Sales", typeof(double)); //4F
                    ph_tbl.Columns.Add("Sez_Supplies", typeof(double));//Sez Supplies//4E
                    ph_tbl.Columns.Add("Inter_branch_transfer", typeof(double)); //29 TYPE
                    ph_tbl.Columns.Add("Delivery_Challan", typeof(double));  //Delivery challan ( EXHIBITION/SALE ON APPROVAL BASIS)
                    ph_tbl.Columns.Add("Merchant_Export", typeof(double));//Merchant Export(0.1 % sale)
                    ph_tbl.Columns.Add("Goods_Lost", typeof(double));
                    ph_tbl.Columns.Add("Stolen", typeof(double));
                    ph_tbl.Columns.Add("Destroyed", typeof(double));
                    ph_tbl.Columns.Add("Written_Off", typeof(double));
                    ph_tbl.Columns.Add("Free_Sample_Gift", typeof(double));
                    ph_tbl.Columns.Add("Perssonal_use", typeof(double));
                    ph_tbl.Columns.Add("EOU_Other_Deemed_Supply", typeof(double));
                    ph_tbl.Columns.Add("Production_Reissue", typeof(double));
                    ph_tbl.Columns.Add("Closing_Stk_Qty", typeof(double));
                    ph_tbl.Columns.Add("Closing_Value_of_Stock", typeof(double));
                    ph_tbl.Columns.Add("Taxable_Value", typeof(double));
                    ph_tbl.Columns.Add("CGST", typeof(double));
                    ph_tbl.Columns.Add("SGST", typeof(double));
                    ph_tbl.Columns.Add("IGST", typeof(double));
                    ph_tbl.Columns.Add("Total_Invoice_Value", typeof(double));
                    #endregion
                    #region
                    #region
                    cond = ""; cond1 = "";
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length < 1)
                    {
                        cond = " and substr(trim(a.icode),1,1)='9'";
                        cond1 = " and substr(trim(icode),1,1)='9'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    #endregion
                    SQuery = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode"; //25/02/2019
                    //select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_2018  as opening,0 as cdr,0 as ccr from itembal where branchcd='02'  and length(trim(icode))>4  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='02' AND VCHDATE between to_Date('01/04/2018','dd/mm/yyyy') and to_date('04/02/2019','dd/mm/yyyy')-1  and icode like '9190301045%'  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='02' AND vchdate between to_Date('04/02/2019','dd/mm/yyyy') and to_date('04/02/2019','dd/mm/yyyy')-1 and store='Y'  and icode like '9190301045%' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode)  GROUP BY A.ICODE,trim(b.iname),b.unit,b.irate,substr(a.icode,1,4),b.hscode having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //op and cl bal  query

                    mq0 = "select distinct trim(a.icode) as icode,a.iname,substr(trim(a.icode),1,4) as scode,A.UNIT,A.IRATE,nvl(a.icost,0) as cost_of_fgood,b.iname as sname,substr(trim(a.icode),1,5) as fg_grp,a.hscode from item a ,item b where length(trim(a.icode))>=8 " + cond + " and substr(trim(a.icode),1,4)=trim(b.icode) and length(trim(b.icode))='4' order by icode"; //icode like '9%'
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);//item dt   

                    //  mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,sum(iqtyin) as qtyin,SUM(iqtyout) as qty,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y','R','N')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate ORDER BY vdd,TYPE,vchnum,icode asc";
                    mq1 = "select branchcd,type,vchnum,vchdate,vdd,trim(icode) as icode,iopr,mattype,sum(iqtyin) as qtyin,SUM(iqtyout) as qty,srno,sum(iqty_chl) as qty_chl,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  FROM (select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyin,iqtyout,iqty_chl,iamount ,exc_amt,exc_rate ,srno from ivoucher where branchcd='" + mbr + "' and type like '%'  AND TYPE NOT IN ('58','59') and vchdate  " + xprdrange + " " + cond1 + "  and store='Y' UNION ALL select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyin,iqtyout,iqty_chl,iamount ,exc_amt,exc_rate,srno from ivoucher where branchcd='" + mbr + "' and type IN ('58','59') and vchdate  " + xprdrange + " " + cond1 + " ) group by branchcd,type,vchnum,vchdate,VDD ,iopr,mattype,trim(icode),exc_rate,srno ORDER BY vdd,vchnum,type,srno asc";//new query as per mayuri mam
                    //is qry me store 'N' add kiya hai qki credit note ki entry show ni hori thi...need ask to sir
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);//main dt for loop

                    mq2 = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.cdr+a.ccr)=0 and sum(a.opening)!=0 order by icode"; //25/02/2019
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq2);//stk dt in for that item which is not covered anywhere

                    mq3 = "select trim(icode) as icode,sum(iqtyin) as qty,type,sum(iamount) as amt from ivoucher where  branchcd='" + mbr + "' and type >='15' and substr(type,1,1)='1' " + cond1 + " and vchdate " + xprdrange + " group by trim(icode),type order by type,icode";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq3);//in house production

                    mq4 = "select distinct TYPE1,acref, (CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' order by type1";
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq4);

                    mq5 = "select trim(a.type1) as fg_Code,trim(a.name) as category from typegrp A where A.id='YX'";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq5); //FOR CATEGORY MASTER

                    mq7 = "select TYPE1,NAME from type where id='M' AND TYPE1 LIKE '2%'";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq7); //FOR CHALLAN

                    mq8 = "select TYPE1,NAME,substr(trim(nAME),1,3)||'/'||TRIM(TYPE1) AS FF,TRIM(TYPE1)||'/'||TRIM(NAME) AS TYPE_NAME  from type where type1 like '4%' and id='V' ORDER BY TYPE1";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq8); //only type like '4%'

                    mq9 = "select TYPE1,NAME,TBRANCHCD,substr(trim(nAME),1,3)||'/'||TRIM(TYPE1) AS FF,TRIM(TYPE1)||'/'||TRIM(NAME) AS TYPE_NAME  from type where id= 'M' and substr(type1,1,1) in ('0','1','2','3','5')  ORDER BY TYPE1";
                    dt9 = fgen.getdata(frm_qstr, co_cd, mq9); // for typename
                    #endregion
                    mq2 = ""; db_op = 0;

                    if (dt2.Rows.Count > 0)
                    {
                        DataView View1 = new DataView(dt2);
                        dt11 = new DataTable();
                        dt11 = View1.ToTable(true, "icode");
                        foreach (DataRow dr in dt11.Rows)
                        {
                            DataView View2 = new DataView(dt2, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = new DataTable();
                            dt10 = View2.ToTable();
                            db_op = 0; //for opening only
                            for (int i = 0; i < dt10.Rows.Count; i++)
                            {
                                #region
                                mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0;
                                db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db23 = 0;
                                hscode = "";
                                mq2 = dt10.Rows[i]["type"].ToString().Trim();
                                mq3 = dt10.Rows[i]["iopr"].ToString().Trim();
                                mq5 = dt10.Rows[i]["exc_rate"].ToString().Trim();
                                mq6 = dt10.Rows[i]["mattype"].ToString().Trim(); //for new develop fields
                                //=====================================
                                dr1 = ph_tbl.NewRow();
                                dr1["date"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                dr1["FG_Code"] = dt10.Rows[i]["ICODE"].ToString().Trim();
                                dr1["FG_Group"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "FG_GRP");
                                dr1["FG_Category"] = fgen.seek_iname_dt(dt6, "fg_Code='" + dr1["FG_Group"].ToString().Trim() + "'", "category");
                                dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "iname");
                                //dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                                //db21 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                                dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                                hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                                db21 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + hscode.Trim() + "'", "hs_rate"));
                                dr1["HSN_Wise_Rate"] = db21;
                                dr1["Total_Cost_of_Finished_Goods_Per_Unit"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "IRATE"));
                                dr1["Measurable_unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "unit");
                                if (i == 0)
                                {
                                    dr1["Opening_FG_Qty_Bal"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "opening")), 2);
                                }
                                else
                                {
                                    dr1["Opening_FG_Qty_Bal"] = Math.Round(db_op, 2);
                                    // db_op = 0;
                                }
                                switch (mq2)
                                {
                                    #region for type 4
                                    case "40":
                                    case "41":
                                    case "43":
                                    case "44":
                                    case "45":
                                    case "46":
                                    case "49":
                                    case "4A":
                                    case "4K":
                                    case "4L":
                                    case "4T":
                                    case "4W":
                                    case "4Y":
                                    case "4Z":
                                    case "4[":
                                    case "4{":
                                    case "4^":
                                    case "4_":
                                    case "4`":
                                    case "4]":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "48":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        dr1["EOU_Other_Deemed_Supply"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                        // mq4 = dt2.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "42":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Merchant_Export"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "4X":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Free_Sample_Gift"] = fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "4E":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Sez_Supplies"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        //  mq4 = dt2.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "4F":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Export_Sales"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);///change recently
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        //   mq4 = dt2.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    #endregion
                                    #region  FOR CHALLAN type like 2 switch case
                                    case "21":
                                    case "22":
                                    case "23":
                                    case "25":
                                    case "27":
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Chl_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Chl_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Doc_Type_of_Challan"] = fgen.seek_iname_dt(dt7, "type1='" + mq2 + "'", "NAme");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "24":
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Chl_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Chl_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Delivery_Challan"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        dr1["Doc_Type_of_Challan"] = fgen.seek_iname_dt(dt7, "type1='" + mq2 + "'", "NAme");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "29":
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["Chl_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Chl_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Inter_branch_transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        dr1["Doc_Type_of_Challan"] = fgen.seek_iname_dt(dt7, "type1='" + mq2 + "'", "NAme");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "50"://for opening only   
                                    case "51":
                                    case "52":
                                    case "53":
                                    case "54":
                                    case "55":
                                    case "57":
                                    case "5A":
                                    case "5B":
                                        dr1["Type"] = "Production_re-issue";// fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    #region credit /debit note
                                    case "58":
                                        dr1["Type"] = "Credit Note";//fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Credit_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Credit_dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim());// - fgen.make_double(dt10.Rows[i]["qty_chl"].ToString().Trim()); //FOR OPENING
                                        db = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "59":
                                        dr1["Type"] = "Debit Note";//fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Debit_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Debit_dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim());// - fgen.make_double(dt10.Rows[i]["qty_chl"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    #endregion
                                    case "04":
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Sales_Return"] = dt10.Rows[i]["qtyin"].ToString().Trim();///old logic
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()); //FOR OPENING
                                        break;
                                    case "0U":
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["Branch_Inward"] = dt10.Rows[i]["qtyin"].ToString().Trim();///old logic
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #region for in house production
                                    case "15":
                                    case "16":
                                    case "17":
                                    case "18":
                                    case "19":
                                    case "1A":
                                    case "1B":
                                    case "1C":
                                    case "1D":
                                    case "1E":
                                    case "1F":
                                    case "1G":
                                    case "1H":
                                    case "1J":
                                    case "1M":
                                    case "1N":
                                    case "1O":
                                    case "1S":
                                        dr1["Doc_No"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Doc_Dt"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        dr1["Type"] = fgen.seek_iname_dt(dt9, "type1='" + mq2 + "'", "TYPE_NAME");
                                        dr1["In_House_Production"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["QTYIN"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    #region for issue type
                                    case "30":
                                    case "31":
                                    case "33":
                                    case "36":
                                    case "37":
                                    case "38":
                                    case "39":
                                        dr1["Type"] = fgen.seek_iname_dt(dt8, "type1='" + mq2 + "'", "TYPE_NAME");
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    case "66":
                                        #region for new develop fields
                                        db_op = fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()); //FOR OPENING
                                        switch (mq6)
                                        {
                                            case "14":
                                                dr1["Destroyed"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                            case "15":
                                                dr1["Stolen"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                break;
                                            case "16":
                                                dr1["Goods_Lost"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                            case "17":
                                                dr1["Written_Off"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                            case "18":
                                                dr1["Perssonal_use"] = Math.Round(fgen.make_double(dt10.Rows[i]["QTY"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                break;
                                        }
                                        #endregion
                                        break;
                                }
                                switch (mq4)
                                {
                                    case "CG":
                                        if (mq2 == "4X" || mq6 == "14" || mq6 == "15" || mq6 == "16" || mq6 == "17" || mq6 == "18")
                                        {
                                            dr1["CGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                            dr1["SGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Intra_State_Sales"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                            dr1["CGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                            dr1["SGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        break;
                                    case "IG":
                                        if (mq2 == "4X" || mq2 == "42")
                                        {
                                            dr1["IGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Inter_State_Sales"] = Math.Round(fgen.make_double(dt10.Rows[i]["qty"].ToString().Trim()), 2);
                                            dr1["IGST"] = Math.Round(fgen.make_double(dt10.Rows[i]["sgst_amt"].ToString().Trim()), 2);
                                        }
                                        break;
                                }
                                //switch (mq5)
                                //{                          
                                //    case "0.1":
                                //        dr1["Merchant_Export"] = db1; //if sa;e type 42 and rate is 0.1 then merhant export
                                //        dr1["TYPE"] = dt2.Rows[i]["TYPE"].ToString().Trim();
                                //        break;
                                //}
                                dr1["Total_Invoice_Value"] = Math.Round(fgen.make_double(dr1["Taxable_Value"].ToString().Trim()) + fgen.make_double(dr1["CGST"].ToString().Trim()) + fgen.make_double(dr1["SGST"].ToString().Trim()) + fgen.make_double(dr1["IGST"].ToString().Trim()), 2);
                                dr1["Total_Qty"] = Math.Round(fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim()) + fgen.make_double(dr1["In_House_Production"].ToString().Trim()) + fgen.make_double(dr1["Sales_Return"].ToString().Trim()) + fgen.make_double(dr1["Branch_Inward"].ToString().Trim()), 2);
                                dr1["Branch_Inward"] = Math.Round(fgen.make_double(dr1["Branch_Inward"].ToString().Trim()), 2);//hold...need to ask   
                                db2 = Math.Round(fgen.make_double(dr1["Total_Qty"].ToString().Trim()), 2);
                                ///============
                                db3 = Math.Round(fgen.make_double(dr1["Intra_State_Sales"].ToString().Trim()), 2);
                                db4 = Math.Round(fgen.make_double(dr1["Inter_State_Sales"].ToString().Trim()), 2);
                                db5 = Math.Round(fgen.make_double(dr1["Export_Sales"].ToString().Trim()), 2);
                                db6 = Math.Round(fgen.make_double(dr1["Sez_Supplies"].ToString().Trim()), 2);
                                db7 = Math.Round(fgen.make_double(dr1["Inter_branch_transfer"].ToString().Trim()), 2);
                                db8 = Math.Round(fgen.make_double(dr1["Delivery_Challan"].ToString().Trim()), 2);
                                db9 = Math.Round(fgen.make_double(dr1["Merchant_Export"].ToString().Trim()), 2);
                                db10 = Math.Round(fgen.make_double(dr1["Goods_Lost"].ToString().Trim()), 2);
                                db11 = Math.Round(fgen.make_double(dr1["Stolen"].ToString().Trim()), 2);
                                db12 = Math.Round(fgen.make_double(dr1["Destroyed"].ToString().Trim()), 2);
                                db13 = Math.Round(fgen.make_double(dr1["Written_Off"].ToString().Trim()), 2);
                                db14 = Math.Round(fgen.make_double(dr1["Free_Sample_Gift"].ToString().Trim()), 2);
                                db15 = Math.Round(fgen.make_double(dr1["Perssonal_use"].ToString().Trim()), 2);
                                db16 = Math.Round(fgen.make_double(dr1["EOU_Other_Deemed_Supply"].ToString().Trim()), 2);
                                db23 = Math.Round(fgen.make_double(dr1["Production_Reissue"].ToString().Trim()), 2);
                                db18 = db3 + db4 + db5 + db6 + db7 + db8 + db9 + db10 + db11 + db12 + db13 + db14 + db15 + db16 + db17 + db18 + db23;
                                db19 = db2 - db18;
                                dr1["Closing_Stk_Qty"] = Math.Round(db19, 2);
                                // db_op = db19; //opening stk qty..OLD LOGIC
                                // db_op = db_op + fgen.make_double(dr1["Opening_FG_Qty_Bal"].ToString().Trim());
                                db20 = db19 * Math.Round(fgen.make_double(dr1["Total_Cost_of_Finished_Goods_Per_Unit"].ToString().Trim()), 2);
                                dr1["Closing_Value_of_Stock"] = db20;
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                        }
                    }
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        #region
                        dr1 = ph_tbl.NewRow(); db22 = 0; string chk_tran = ""; hscode = "";
                        chk_tran = fgen.seek_iname_dt(dt2, "trim(icode)='" + dt3.Rows[i]["ICODE"].ToString().Trim() + "'", "icode");
                        if (chk_tran.Length > 1)
                        {
                        }
                        else
                        {
                            dr1["date"] = "No transaction";// dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["FG_Code"] = dt3.Rows[i]["ICODE"].ToString().Trim();
                            mq6 = dt3.Rows[i]["ICODE"].ToString().Trim().Substring(0, 5);
                            dr1["FG_Group"] = mq6;// fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "FG_GRP");
                            dr1["FG_Category"] = fgen.seek_iname_dt(dt6, "fg_Code='" + mq6 + "'", "category");
                            dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "iname");
                            // dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                            //  mq6 = fgen.seek_iname(frm_qstr, co_cd, "select distinct TYPE1, (CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' and acref='" + dr1["HSN_Code"].ToString().Trim() + "' order by type1", "hs_rate");
                            //db22 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                            dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                            hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "hscode");
                            db22 = fgen.make_double(fgen.seek_iname_dt(dt5, "acref='" + hscode.Trim() + "'", "hs_rate"));
                            dr1["HSN_Wise_Rate"] = db22;
                            dr1["Total_Cost_of_Finished_Goods_Per_Unit"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "irate"));
                            dr1["Measurable_unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["FG_Code"].ToString().Trim() + "'", "unit");
                            dr1["Opening_FG_Qty_Bal"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()), 2);
                            dr1["Closing_Stk_Qty"] = Math.Round(fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()) - fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            ph_tbl.Rows.Add(dr1);
                        }
                        #endregion
                    }
                    //if (ph_tbl.Rows.Count > 0)
                    //{
                    //    Session["send_dt"] = ph_tbl;
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    //    fgen.Fn_open_rptlevel("" + header_n + " Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    //}
                    #endregion
                    /////////////                 
                    #region for fg summary
                    dtm = new DataTable(); dt11 = new DataTable(); dt10 = new DataTable();
                    //  dtm.Columns.Add("date", typeof(string));
                    dtm.Columns.Add("FG_Category", typeof(string));
                    dtm.Columns.Add("FG_Code", typeof(string));//ICODE
                    dtm.Columns.Add("FG_Group", typeof(string)); //ITEM GROUP
                    dtm.Columns.Add("Item_Description", typeof(string));
                    dtm.Columns.Add("HSN_Code", typeof(string));
                    dtm.Columns.Add("HSN_Wise_Rate", typeof(double));
                    dtm.Columns.Add("Total_Cost_of_Finished_Goods_Per_Unit", typeof(double));
                    dtm.Columns.Add("Measurable_unit", typeof(string));
                    dtm.Columns.Add("Opening_FG_Qty_Bal", typeof(double));
                    dtm.Columns.Add("Branch_Inward", typeof(double));
                    dtm.Columns.Add("In_House_Production", typeof(double));
                    dtm.Columns.Add("Sales_Return", typeof(double));//Sales Return( LESS IF THIS ISSUE TO PRODUCTION)
                    dtm.Columns.Add("Total_Qty", typeof(double));//Total Qty                
                    //////////
                    dtm.Columns.Add("Intra_State_Sales", typeof(double)); //Intra State Sales
                    dtm.Columns.Add("Inter_State_Sales", typeof(double));//Inter State Sales
                    dtm.Columns.Add("Export_Sales", typeof(double)); //4F
                    dtm.Columns.Add("Sez_Supplies", typeof(double));//Sez Supplies//4E
                    dtm.Columns.Add("Inter_branch_transfer", typeof(double)); //29 TYPE
                    dtm.Columns.Add("Delivery_Challan", typeof(double));  //Delivery challan ( EXHIBITION/SALE ON APPROVAL BASIS)
                    dtm.Columns.Add("Merchant_Export", typeof(double));//Merchant Export(0.1 % sale)
                    dtm.Columns.Add("Goods_Lost", typeof(double));
                    dtm.Columns.Add("Stolen", typeof(double));
                    dtm.Columns.Add("Destroyed", typeof(double));
                    dtm.Columns.Add("Written_Off", typeof(double));
                    dtm.Columns.Add("Free_Sample_Gift", typeof(double));
                    dtm.Columns.Add("Perssonal_use", typeof(double));
                    dtm.Columns.Add("EOU_Other_Deemed_Supply", typeof(double));
                    dtm.Columns.Add("Production_Reissue", typeof(double));
                    dtm.Columns.Add("Closing_Stk_Qty", typeof(double));
                    dtm.Columns.Add("Closing_Value_of_Stock", typeof(double));
                    dtm.Columns.Add("Taxable_Value", typeof(double));
                    dtm.Columns.Add("CGST", typeof(double));
                    dtm.Columns.Add("SGST", typeof(double));
                    dtm.Columns.Add("IGST", typeof(double));
                    dtm.Columns.Add("Total_Invoice_Value", typeof(double));

                    if (ph_tbl.Rows.Count > 0)
                    {
                        View1 = new DataView(ph_tbl);
                        dt11 = new DataTable();
                        dt11 = View1.ToTable(true, "FG_Code");
                        foreach (DataRow dr in dt11.Rows)
                        {
                            View2 = new DataView(ph_tbl, "FG_Code='" + dr["FG_Code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = new DataTable();
                            dt10 = View2.ToTable();
                            db_op = 0; //for opening only
                            dr1 = dtm.NewRow();
                            db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db22 = 0; db23 = 0; db24 = 0; db25 = 0; db26 = 0; db27 = 0;
                            for (int i = 0; i < dt10.Rows.Count; i++)
                            {
                                #region
                                // dr1["date"] = dt10.Rows[i]["date"].ToString().Trim();
                                dr1["FG_Category"] = dt10.Rows[i]["FG_Category"].ToString().Trim();
                                dr1["FG_Code"] = dt10.Rows[i]["FG_Code"].ToString().Trim();
                                dr1["FG_Group"] = dt10.Rows[i]["FG_Group"].ToString().Trim();
                                dr1["Item_Description"] = dt10.Rows[i]["Item_Description"].ToString().Trim();
                                // dr1["HSN_Code"] = dt10.Rows[i]["HSN_Code"].ToString().Trim();
                                dr1["HSN_Code"] = "HSN - " + dt10.Rows[i]["HSN_Code"].ToString().Trim();
                                db1 += fgen.make_double(dt10.Rows[i]["Total_Cost_of_Finished_Goods_Per_Unit"].ToString().Trim());
                                dr1["Total_Cost_of_Finished_Goods_Per_Unit"] = db1;
                                dr1["Measurable_unit"] = dt10.Rows[i]["Measurable_unit"].ToString().Trim();
                                if (i == 0)
                                {
                                    db = fgen.make_double(dt10.Rows[i]["Opening_FG_Qty_Bal"].ToString().Trim());
                                }
                                dr1["Opening_FG_Qty_Bal"] = db;
                                dr1["HSN_Wise_Rate"] = dt10.Rows[i]["HSN_Wise_Rate"].ToString().Trim();
                                db2 += fgen.make_double(dt10.Rows[i]["Branch_Inward"].ToString().Trim());
                                dr1["Branch_Inward"] = db2;
                                db3 += fgen.make_double(dt10.Rows[i]["In_House_Production"].ToString().Trim());
                                dr1["In_House_Production"] = db3;
                                db4 += fgen.make_double(dt10.Rows[i]["Sales_Return"].ToString().Trim());
                                dr1["Sales_Return"] = db4;
                                // db5 += fgen.make_double(dt10.Rows[i]["Total_Qty"].ToString().Trim());
                                db5 = db + db2 + db3 + db4;
                                dr1["Total_Qty"] = db5;
                                db6 += fgen.make_double(dt10.Rows[i]["Intra_State_Sales"].ToString().Trim());
                                dr1["Intra_State_Sales"] = db6;
                                db7 += fgen.make_double(dt10.Rows[i]["Inter_State_Sales"].ToString().Trim());
                                dr1["Inter_State_Sales"] = db7;
                                db8 += fgen.make_double(dt10.Rows[i]["Export_Sales"].ToString().Trim());
                                dr1["Export_Sales"] = db8;
                                db9 += fgen.make_double(dt10.Rows[i]["Sez_Supplies"].ToString().Trim());
                                dr1["Sez_Supplies"] = db9;
                                db10 += fgen.make_double(dt10.Rows[i]["Inter_branch_transfer"].ToString().Trim());
                                dr1["Inter_branch_transfer"] = db10;
                                db11 += fgen.make_double(dt10.Rows[i]["Delivery_Challan"].ToString().Trim());
                                dr1["Delivery_Challan"] = db11;
                                db12 += fgen.make_double(dt10.Rows[i]["Merchant_Export"].ToString().Trim());
                                dr1["Merchant_Export"] = db12;
                                db13 += fgen.make_double(dt10.Rows[i]["Goods_Lost"].ToString().Trim());
                                dr1["Goods_Lost"] = db13;
                                db14 += fgen.make_double(dt10.Rows[i]["Stolen"].ToString().Trim());
                                dr1["Stolen"] = db14;
                                db15 += fgen.make_double(dt10.Rows[i]["Destroyed"].ToString().Trim());
                                dr1["Destroyed"] = db15;
                                db16 += fgen.make_double(dt10.Rows[i]["Written_Off"].ToString().Trim());
                                dr1["Written_Off"] = db16;
                                db17 += fgen.make_double(dt10.Rows[i]["Free_Sample_Gift"].ToString().Trim());
                                dr1["Free_Sample_Gift"] = db17;
                                db18 += fgen.make_double(dt10.Rows[i]["Perssonal_use"].ToString().Trim());
                                dr1["Perssonal_use"] = db18;
                                db19 += fgen.make_double(dt10.Rows[i]["EOU_Other_Deemed_Supply"].ToString().Trim());
                                dr1["EOU_Other_Deemed_Supply"] = db19;
                                db20 += fgen.make_double(dt10.Rows[i]["Production_Reissue"].ToString().Trim());
                                dr1["Production_Reissue"] = db20;
                                db21 = fgen.make_double(dt10.Rows[i]["Closing_Stk_Qty"].ToString().Trim());
                                dr1["Closing_Stk_Qty"] = db21;
                                db22 += fgen.make_double(dt10.Rows[i]["Closing_Value_of_Stock"].ToString().Trim());
                                dr1["Closing_Value_of_Stock"] = db22;
                                db23 += fgen.make_double(dt10.Rows[i]["Taxable_Value"].ToString().Trim());
                                dr1["Taxable_Value"] = db23;
                                db24 += fgen.make_double(dt10.Rows[i]["CGST"].ToString().Trim());
                                dr1["CGST"] = db24;
                                db25 += fgen.make_double(dt10.Rows[i]["SGST"].ToString().Trim());
                                dr1["SGST"] = db25;
                                db26 += fgen.make_double(dt10.Rows[i]["IGST"].ToString().Trim());
                                dr1["IGST"] = db26;
                                db27 += fgen.make_double(dt10.Rows[i]["Total_Invoice_Value"].ToString().Trim());
                                dr1["Total_Invoice_Value"] = db27;
                                #endregion
                            }
                            dtm.Rows.Add(dr1);
                        }
                    }
                    #endregion
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("" + header_n + " Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    break;

                case "F70296":
                    //RM STOCK SUMMARY
                    #region
                    #region
                    dt2 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                    dt5 = new DataTable(); dt6 = new DataTable(); dt7 = new DataTable(); dt8 = new DataTable();
                    ph_tbl = new DataTable();
                    ph_tbl.Columns.Add("Date", typeof(string));
                    ph_tbl.Columns.Add("Category", typeof(string));
                    ph_tbl.Columns.Add("Item_Code", typeof(string));
                    ph_tbl.Columns.Add("Item_Description", typeof(string));
                    ph_tbl.Columns.Add("HSN_Code", typeof(string));
                    ph_tbl.Columns.Add("Applicable_GST_Rate", typeof(double));
                    ph_tbl.Columns.Add("Weighted_Average_Cost_of_Goods", typeof(double)); //pic irate from item as on 16 march 2019
                    ph_tbl.Columns.Add("Measurable_Unit", typeof(string));
                    ph_tbl.Columns.Add("Opening_Balance", typeof(double));
                    ////////////
                    ph_tbl.Columns.Add("Inward_Doc_No", typeof(string));
                    ph_tbl.Columns.Add("Inward_Doc_Dt", typeof(string));
                    //ph_tbl.Columns.Add("Inv_No", typeof(string));//Invoice No
                    //ph_tbl.Columns.Add("Inv_Dt", typeof(string));//Invoice 
                    //ph_tbl.Columns.Add("Chl_No", typeof(string));
                    //ph_tbl.Columns.Add("Chl_Dt", typeof(string));
                    //ph_tbl.Columns.Add("Credit_No", typeof(string));
                    //ph_tbl.Columns.Add("Credit_dt", typeof(string));
                    //ph_tbl.Columns.Add("Debit_No", typeof(string));
                    //ph_tbl.Columns.Add("Debit_dt", typeof(string));
                    ////inward Movement
                    ph_tbl.Columns.Add("Intra_State", typeof(double));
                    ph_tbl.Columns.Add("Out_of_State", typeof(double));
                    ph_tbl.Columns.Add("Import", typeof(double));
                    ph_tbl.Columns.Add("Branch_Inward_Transfer", typeof(double));
                    ph_tbl.Columns.Add("Received_from_Jobworker_Inw", typeof(double));
                    ph_tbl.Columns.Add("Production_Reissue", typeof(double));
                    ph_tbl.Columns.Add("Total_Inw_Supply", typeof(double));
                    ph_tbl.Columns.Add("Total_Stock", typeof(double));
                    ////ISSUE To production
                    ph_tbl.Columns.Add("Document_No", typeof(string));
                    ph_tbl.Columns.Add("Quantity", typeof(double));
                    /////////////////outward portion==============================
                    ph_tbl.Columns.Add("Inv_No_", typeof(string));//Invoice No
                    ph_tbl.Columns.Add("Inv_Dt_", typeof(string));//Invoice 
                    ph_tbl.Columns.Add("Chl_No_", typeof(string));
                    ph_tbl.Columns.Add("Chl_Dt_", typeof(string));
                    ph_tbl.Columns.Add("Credit_No_", typeof(string));
                    ph_tbl.Columns.Add("Credit_dt_", typeof(string));
                    ph_tbl.Columns.Add("Debit_No_", typeof(string));
                    ph_tbl.Columns.Add("Debit_dt_", typeof(string));
                    ph_tbl.Columns.Add("Purchase_Return", typeof(double));
                    ///outward movement
                    ph_tbl.Columns.Add("Branch_Outward_Transfer", typeof(double));
                    ph_tbl.Columns.Add("Sent_for_Jobwork", typeof(double));
                    ph_tbl.Columns.Add("Goods_Lost", typeof(double));
                    ph_tbl.Columns.Add("Stolen", typeof(double));
                    ph_tbl.Columns.Add("Destroyed", typeof(double));
                    ph_tbl.Columns.Add("Written_Off", typeof(double));
                    ph_tbl.Columns.Add("Free_Sample_Gift", typeof(double));
                    ph_tbl.Columns.Add("Personal_Use", typeof(double));
                    //ph_tbl.Columns.Add("Production_Reissue", typeof(double));
                    ph_tbl.Columns.Add("Total_Outwar_Supply_of_RM", typeof(double));
                    ph_tbl.Columns.Add("Closing_Balance", typeof(double));
                    ph_tbl.Columns.Add("Closing_Stock_Value", typeof(double));
                    ph_tbl.Columns.Add("Taxable_Value", typeof(double));
                    ph_tbl.Columns.Add("CGST", typeof(double));
                    ph_tbl.Columns.Add("SGST", typeof(double));
                    ph_tbl.Columns.Add("IGST", typeof(double));
                    ph_tbl.Columns.Add("Total_Invoice", typeof(double));

                    header_n = "RM-Stock Register";
                    #endregion
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                    xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                    xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                    cond1 = ""; cond = "";
                    #region
                    //if no selection any box
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length < 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length < 1)
                    {
                        cond = " and substr(trim(a.icode),1,1)<'4'";
                        cond1 = " and substr(trim(icode),1,1)<'4'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1)
                    {
                        cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                        cond1 = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                    }
                    #endregion
                    //======================================
                    SQuery = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode"; //25/02/2019                    
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery); //op and cl bal  query

                    mq0 = "select distinct trim(a.icode) as icode,a.iname,substr(trim(a.icode),1,4) as scode,A.UNIT,A.IRATE,nvl(a.icost,0) as cost_of_fgood,b.iname as sname,substr(trim(a.icode),1,5) as fg_grp,a.hscode from item a ,item b where length(trim(a.icode))>=8  and substr(trim(a.icode),1,1)<'9' " + cond + " and substr(trim(a.icode),1,4)=trim(b.icode) and length(trim(b.icode))='4' order by icode";
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq0);//item dt                                                     

                    //mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,SUM(iqtyout) as qtyout,sum(iqtyin) as iqtyin,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y','R')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate,irate ORDER BY vdd,icode asc"; //store in Y & R ...OLD
                    //mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,srno,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyout as qtyout,iqtyin as iqtyin,iqty_chl as qty_chl,iamount as iamount /*,exc_amt as sgst_Amt*/,exc_rate,(Case when trim(Unit)='CG' then exc_Rate else 0 end) as CGST_RT,(Case when trim(Unit)='CG' then exc_Amt else 0 end) as CGST_amt,(Case when trim(Unit)='CG' then cess_percent else 0 end) as SGST_Rate,(Case when trim(Unit)='CG' then cess_pu else 0 end) as SGST_amt,(Case when trim(Unit)='IG' then exc_rate else 0 end) as IGST_Rt,(Case when trim(Unit)='IG' then exc_amt else 0 end) as IGST_amt,invno,to_char(invdate,'dd/mm/yyyy') as invdate,TRIM(UNIT) AS UNIT,nvl(trim(cavity),0) as cavity,acpt_ud as Accept  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')   ORDER BY vdd,vchnum,type,srno asc"; //remove store=r.....AS PER MAYYURI MAM
                    mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,srno,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,SUM(iqtyout) as qtyout,sum(iqtyin) as iqtyin,sum(iqty_chl) as qty_chl,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate,irate ORDER BY vdd,vchnum,type,srno asc"; //remove store=r.....AS PER MAYYURI MAM
                    //mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,SUM(iqtyout) as qtyout,sum(iqtyin) as iqtyin,sum(iqty_chl) as qty_chl,sum(iamount) as iamount ,sum(exc_amt) as sgst_Amt,exc_rate  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')  group by branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),to_char(vchdate,'yyyymmdd') ,iopr,mattype,trim(icode),exc_rate,irate ORDER BY vdd,icode asc"; //remove store=r.....AS PER MAYYURI MAM
                    //  mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,srno,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,mattype,iqtyout as qtyout,iqtyin as iqtyin,iqty_chl as qty_chl,iamount as iamount /*,exc_amt as sgst_Amt*/,exc_rate,(Case when trim(Unit)='CG' then exc_Rate else 0 end) as CGST_RT,(Case when trim(Unit)='CG' then exc_Amt else 0 end) as CGST_amt,(Case when trim(Unit)='CG' then cess_percent else 0 end) as SGST_Rate,(Case when trim(Unit)='CG' then cess_pu else 0 end) as SGST_amt,(Case when trim(Unit)='IG' then exc_rate else 0 end) as IGST_Rt,(Case when trim(Unit)='IG' then exc_amt else 0 end) as IGST_amt,invno,to_char(invdate,'dd/mm/yyyy') as invdate,TRIM(UNIT) AS UNIT,nvl(trim(cavity),0) as cavity,acpt_ud as Accept  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')   ORDER BY vdd,vchnum,type,srno asc"; //remove store=r.....AS PER MAYYURI MAM ///////////////old as per 12 april 19

                    mq1 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,srno,irate,to_char(vchdate,'yyyymmdd') as vdd,trim(icode) as icode,iopr,post,mattype,iqtyout as qtyout,iqtyin as iqtyin,iqty_chl as qty_chl,iamount as iamount /*,exc_amt as sgst_Amt*/,exc_rate,(Case when trim(Unit)='CG' then exc_Rate else 0 end) as CGST_RT,(Case when trim(iopr)='CG' then exc_Rate else 0 end) as CGST_RT_,(Case when trim(post)=1 then exc_Rate else 0 end) as CGST_RT_1,(Case when trim(Unit)='CG' then exc_Amt else 0 end) as CGST_amt,(Case when trim(iopr)='CG' then exc_Amt else 0 end) as CGST_amt_,(Case when trim(post)=1 then exc_Amt else 0 end) as CGST_amt_1,(Case when trim(Unit)='CG' then cess_percent else 0 end) as SGST_Rate,(Case when trim(iopr)='CG' then cess_percent else 0 end) as SGST_Rate_,(Case when trim(post)=1 then cess_percent else 0 end) as SGST_Rate_1,(Case when trim(Unit)='CG' then cess_pu else 0 end) as SGST_amt,(Case when trim(iopr)='CG' then cess_pu else 0 end) as SGST_amt_,(Case when trim(post)=1 then cess_pu else 0 end) as SGST_amt_1,(Case when trim(Unit)='IG' then exc_rate else 0 end) as IGST_Rt,(Case when trim(iopr)='IG' then exc_rate else 0 end) as IGST_Rt_,(Case when trim(post)=2 then exc_rate else 0 end) as IGST_Rt_1,(Case when trim(Unit)='IG' then exc_amt else 0 end) as IGST_amt,(Case when trim(iopr)='IG' then exc_amt else 0 end) as IGST_amt_,(Case when trim(post)=2 then exc_amt else 0 end) as IGST_amt_1,invno,to_char(invdate,'dd/mm/yyyy') as invdate,TRIM(UNIT) AS UNIT,nvl(trim(cavity),0) as cavity,acpt_ud as Accept  from ivoucher where branchcd='" + mbr + "' and type like '%'  and vchdate " + xprdrange + " " + cond1 + "  and store in ('Y')   ORDER BY vdd,vchnum,type,srno asc";//new as on 12 apr 19..
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq1);//main dt for loop

                    mq2 = "select a.icode as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + year + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + mbr + "'  and length(trim(icode))>4 " + cond1 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + mbr + "' AND VCHDATE " + xprdRange1 + " " + cond1 + "  and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond1 + " GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.cdr+a.ccr)=0 and sum(a.opening)!=0 order by icode"; //25/02/2019
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq2);//stk dt in for that item which is not covered anywhere 

                    mq3 = "select distinct TYPE1,acref,(CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' order by type1";
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq3);

                    mq5 = "select trim(a.ICODE) as fg_Code,trim(a.Iname) as category from ITEM A where LENGTH(trim(a.ICODE)) =4";
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq5); //FOR CATEGORY MASTER

                    // mq6 = "Select A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.iamount,round(a.exp_punit,2) as Txb_Chgs,a.unit as TX_type,(Case when trim(A.Unit)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,a.Location as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,TRIM(A.UNIT) AS UNIT from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno";
                    mq6 = "Select A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.iamount,round(a.exp_punit,2) as Txb_Chgs,a.unit as TX_type,(Case when trim(A.Unit)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,a.Location as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,TRIM(A.UNIT) AS UNIT,nvl(trim(a.cavity),0) as cavity from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type like '0%' and a.vchdate " + xprdrange + " order by a.vchdate,a.vchnum,a.srno";
                    dt7 = fgen.getdata(frm_qstr, co_cd, mq6); //hsn wise purcvhase(inward data).............gst module 

                    mq7 = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ to_char(a.vchdate,'DD/MM/YYYY') as Dated,a.Vchnum as MRR_No,b.aname as Supplier,trim(a.invno)||','||trim(a.refnum) as Bill_Chl,c.iname as Item_Name,c.unit,a.iqty_chl as Advised,a.iqtyin+nvl(rej_rw,0) as Rcvd,a.acpt_ud as Accept,a.rej_rw as Reject,a.irate,a.ichgs as Lc,C.cpartno as Code,a.Btchno as Batchno,a.btchdt as Batch_Dt,a.finvno,a.Type,a.tc_no as TC_NO,a.ponum as P_O_No,a.Genum as Gate_Entry,a.gedate as Gate_Date,a.Ent_by,a.Pname as Insp_By,a.Qcdate,a.icode,a.store,a.Mode_tpt,a.Mtime,a.mfgdt,a.expdt,b.addr3,b.rc_num,b.addr1,b.addr2,a.rgpnum,a.rgpdate,a.freight as cl_by,a.o_Deptt,a.st_entform as ewaybillno  from ivoucher a, famst b , item c where a.branchcd='" + mbr + "' and A.type like '0%' and a.vchdate  " + xprdrange + " and a.store<>'R' and TRIM(a.icode)=TRIM(c.icode) and trim(a.acode)=trim(B.acode) and 1=1  order by vchdate,type,vchnum,srno";
                    dt8 = fgen.getdata(frm_qstr, co_cd, mq7); //query for mrr report in Inventory module

                    if (dt2.Rows.Count > 0)
                    {
                        DataView View1 = new DataView(dt2);
                        dt9 = new DataTable();
                        dt9 = View1.ToTable(true, "icode");
                        foreach (DataRow dr in dt9.Rows)
                        {
                            DataView View2 = new DataView(dt2, "icode='" + dr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = new DataTable();
                            dt10 = View2.ToTable();
                            db_op = 0; //for opening only
                            for (int i = 0; i < dt10.Rows.Count; i++)
                            {
                                #region
                                mq2 = ""; mq6 = ""; db10 = 0; string unit = ""; hscode = "";
                                mq2 = dt10.Rows[i]["type"].ToString().Trim();
                                mq6 = dt10.Rows[i]["mattype"].ToString().Trim(); //for new develop fields
                                dr1 = ph_tbl.NewRow();
                                dr1["date"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                dr1["Item_Code"] = dt10.Rows[i]["ICODE"].ToString().Trim();
                                dr1["Category"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "sname");
                                dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iname");
                                //dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                                //   mq6 = fgen.seek_iname(frm_qstr, co_cd, "select distinct TYPE1, (CASE WHEN nvl(num6,0)=0 then nvl(num4,0)+nvl(num5,0) else num6 end) as hs_rate from typegrp where id='T1' and acref='" + dr1["HSN_Code"].ToString().Trim() + "' order by type1", "hs_rate");
                                //db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                                dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                                hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                                db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + hscode.Trim() + "'", "hs_rate"));
                                dr1["Applicable_GST_Rate"] = db10;
                                dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IRATE"));
                                dr1["Measurable_Unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                if (i == 0)
                                {
                                    dr1["Opening_Balance"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "opening")), 2);
                                }
                                else
                                {
                                    dr1["Opening_Balance"] = Math.Round(db_op, 2);
                                }
                                switch (mq2)
                                {
                                    #region inward portion
                                    #region mrr
                                    case "02":
                                    case "03":
                                    case "04":
                                    case "05":
                                    case "06":
                                    case "08":
                                    case "0B":
                                    case "0D":
                                        //// dr1["Inward_Doc_No"] = dt2.Rows[i]["Vchnum"].ToString().Trim();
                                        //// dr1["Inward_Doc_Dt"] = dt2.Rows[i]["vchdate"].ToString().Trim();
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT"));
                                        //dr1["SGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT"));
                                        //dr1["IGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT"));
                                        ////  dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //// dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        //unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        //if (unit == "IG")
                                        //{
                                        //    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}
                                        //else
                                        //{
                                        //    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}

                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        unit = dt10.Rows[i]["unit"].ToString().Trim();
                                        if (unit == "IG")
                                        {
                                            dr1["Out_of_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Intra_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        break;
                                    case "07":
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //double amt = 0, rate = 0;
                                        //amt = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount"));
                                        //rate = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "cavity"));
                                        //dr1["Taxable_Value"] = Math.Round(amt * rate, 2);
                                        ////dr1["CGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT")), 2);
                                        ////dr1["SGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT")), 2);
                                        //dr1["IGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT")), 2);
                                        ////  dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //dr1["import"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        ////dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////   dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        ////unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        ////if (unit == "IG")
                                        ////{
                                        ////    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        ////else
                                        ////{
                                        ////    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()) * fgen.make_double(dt10.Rows[i]["cavity"].ToString().Trim()), 2);
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        dr1["import"] = fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim());
                                        break;
                                    case "0C":
                                        //// dr1["Inward_Doc_No"] = dt2.Rows[i]["Vchnum"].ToString().Trim();
                                        //// dr1["Inward_Doc_Dt"] = dt2.Rows[i]["vchdate"].ToString().Trim();
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT"));
                                        //dr1["SGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT"));
                                        //dr1["IGST"] = fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT"));
                                        ////  dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //// dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        dr1["Branch_Inward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        break;
                                    case "0U":
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT")), 2);
                                        //dr1["SGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT")), 2);
                                        //dr1["IGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT")), 2);
                                        //// dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        dr1["Branch_Inward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        //  dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        //unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        //if (unit == "IG")
                                        //{
                                        //    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}
                                        //else
                                        //{
                                        //    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        //}

                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        unit = dt10.Rows[i]["unit"].ToString().Trim();
                                        if (unit == "IG")
                                        {
                                            dr1["Out_of_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        else
                                        {
                                            dr1["Intra_State"] = Math.Round(fgen.make_double(dt10.Rows[i]["accept"].ToString().Trim()), 2);
                                        }
                                        break;
                                    case "09":
                                        dr1["Inward_Doc_No"] = dt10.Rows[i]["invno"].ToString().Trim();
                                        dr1["Inward_Doc_Dt"] = dt10.Rows[i]["invdate"].ToString().Trim();
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["CGST_AMT"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["SGST_AMT"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["IGST_AMT"].ToString().Trim());
                                        //dr1["Inward_Doc_No"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invno");
                                        //dr1["Inward_Doc_Dt"] = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "invdate");
                                        //dr1["Taxable_Value"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iamount")), 2);
                                        //dr1["CGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "CGST_AMT")), 2);
                                        //dr1["SGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "SGST_AMT")), 2);
                                        //dr1["IGST"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IGST_AMT")), 2);
                                        //// dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        dr1["Received_from_Jobworker_Inw"] = Math.Round(fgen.make_double(dt10.Rows[i]["iqtyin"].ToString().Trim()), 2);
                                        ////dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(dt2.Rows[i]["irate"].ToString().Trim());
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING
                                        ////unit = fgen.seek_iname_dt(dt7, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                                        ////if (unit == "IG")
                                        ////{
                                        ////    dr1["Out_of_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        ////else
                                        ////{
                                        ////    dr1["Intra_State"] = Math.Round(fgen.make_double(fgen.seek_iname_dt(dt8, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "accept")), 2);
                                        ////}
                                        break;

                                    #endregion
                                    #region for type like 1
                                    case "10":
                                    case "11":
                                    case "12":
                                    case "13":
                                    case "14":
                                        dr1["Document_No"] = dt10.Rows[i]["Vchnum"].ToString().Trim();//
                                        // dr1["Quantity"] = Math.Round(fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()), 2);
                                        dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dt10.Rows[i]["iQTYIN"].ToString().Trim()); //FOR OPENING                                      
                                        break;
                                    #endregion
                                    #region issue to production
                                    case "30":
                                    case "31":
                                    case "33":
                                    case "36":
                                    case "37":
                                    case "38":
                                    case "39":
                                        dr1["Document_No"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Quantity"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        break;
                                    #endregion
                                    #region challan outward case
                                    case "21":
                                        dr1["Chl_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Chl_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Sent_for_Jobwork"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);//ADD ON 12 APR 19 EVG 5:30..PENDING TO MERGE ..ONLY THIS SINGLE LINE
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_1"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_1"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_1"].ToString().Trim());
                                        break;
                                    case "22":
                                    case "23":
                                    case "24":
                                    case "25":
                                        dr1["Chl_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Chl_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Branch_Outward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_1"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_1"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_1"].ToString().Trim());
                                        break;
                                    case "29":
                                        dr1["Chl_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Chl_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Branch_Outward_Transfer"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_1"].ToString().Trim());
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_1"].ToString().Trim());
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_1"].ToString().Trim());
                                        break;
                                    #endregion
                                    #region for outward invoice
                                    #region invoice
                                    case "40":
                                    case "41":
                                    case "42":
                                    case "43":
                                    case "44":
                                    case "45":
                                    case "46":
                                    case "48":
                                    case "49":
                                    case "4A":
                                    case "4B":
                                    case "4C":
                                    case "4D":
                                    case "4E":
                                    case "4F":
                                    case "4G":
                                    case "4J":
                                    case "4K":
                                    case "4L":
                                    case "4T":
                                    case "4U":
                                    case "4V":
                                    case "4W":
                                    case "4X":
                                    case "4Y":
                                    case "4Z":
                                    case "4[":
                                    case "4{":
                                    case "4]":
                                    case "4^":
                                    case "4_":
                                    #endregion
                                    case "4`":
                                        dr1["Inv_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Inv_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    case "47":
                                        dr1["Inv_No_"] = dt10.Rows[i]["Vchnum"].ToString().Trim();
                                        dr1["Inv_Dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                        dr1["Taxable_Value"] = fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()); //...old logic
                                        dr1["Purchase_Return"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                        mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                        break;
                                    #endregion
                                    #region credit /debit note
                                    case "58":
                                        dr1["Credit_No_"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Credit_dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        break;
                                    case "59":
                                        dr1["Debit_No_"] = dt10.Rows[i]["vchnum"].ToString().Trim();
                                        dr1["Debit_dt_"] = dt10.Rows[i]["vchdate"].ToString().Trim();
                                        break;
                                    #endregion
                                    case "66"://yaha abi data ni h to not sure ki qty kis field me jayegi so ryt now picking iamount
                                        #region for new develop fields
                                        switch (mq6)
                                        {
                                            case "13":
                                                dr1["Destroyed"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "14":
                                                dr1["Destroyed"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "15":
                                                dr1["Stolen"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "16":
                                                dr1["Goods_Lost"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "17":
                                                dr1["Written_Off"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                            case "18":
                                                dr1["Personal_Use"] = Math.Round(fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()), 2);
                                                mq4 = dt10.Rows[i]["iopr"].ToString().Trim();
                                                dr1["Taxable_Value"] = Math.Round(fgen.make_double(dt10.Rows[i]["iamount"].ToString().Trim()), 2);
                                                db_op = fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) - fgen.make_double(dt10.Rows[i]["qtyout"].ToString().Trim()); //FOR OPENING
                                                break;
                                        }
                                        #endregion
                                        break;
                                }
                                switch (mq4)
                                {
                                    case "CG": //inward case
                                    case "IG":
                                        dr1["CGST"] = fgen.make_double(dt10.Rows[i]["cgst_amt_"].ToString().Trim());//use in outward set type like 4
                                        dr1["SGST"] = fgen.make_double(dt10.Rows[i]["sgst_amt_"].ToString().Trim());//use in outward set type like 4                                                            
                                        dr1["IGST"] = fgen.make_double(dt10.Rows[i]["igst_amt_"].ToString().Trim()); //use in outward set type like 4
                                        break;
                                }
                                dr1["Total_Inw_Supply"] = Math.Round(fgen.make_double(dr1["Intra_State"].ToString().Trim()) + fgen.make_double(dr1["Out_of_State"].ToString().Trim()) + fgen.make_double(dr1["import"].ToString().Trim()) + fgen.make_double(dr1["Branch_Inward_Transfer"].ToString().Trim()) + fgen.make_double(dr1["Received_from_Jobworker_Inw"].ToString().Trim()), 2);
                                dr1["Total_Stock"] = Math.Round(fgen.make_double(dr1["Opening_Balance"].ToString().Trim()) + fgen.make_double(dr1["Total_Inw_Supply"].ToString().Trim()), 2);
                                dr1["Total_Outwar_Supply_of_RM"] = Math.Round(fgen.make_double(dr1["Purchase_Return"].ToString().Trim()) + fgen.make_double(dr1["Branch_Outward_Transfer"].ToString().Trim()) + fgen.make_double(dr1["Sent_for_Jobwork"].ToString().Trim()) + fgen.make_double(dr1["Goods_Lost"].ToString().Trim()) + fgen.make_double(dr1["Stolen"].ToString().Trim()) + fgen.make_double(dr1["Destroyed"].ToString().Trim()) + fgen.make_double(dr1["Written_Off"].ToString().Trim()) + fgen.make_double(dr1["Personal_Use"].ToString().Trim()) + fgen.make_double(dr1["Free_Sample_Gift"].ToString().Trim()), 2);
                                if (mq2 == "10" || mq2 == "11" || mq2 == "12" || mq2 == "13" || mq2 == "14")
                                {
                                    dr1["Closing_Balance"] = Math.Round(fgen.make_double(dr1["Total_Stock"].ToString().Trim()) + fgen.make_double(dr1["Quantity"].ToString().Trim()) - fgen.make_double(dr1["Total_Outwar_Supply_of_RM"].ToString().Trim()), 2);
                                }
                                else
                                {
                                    dr1["Closing_Balance"] = Math.Round(fgen.make_double(dr1["Total_Stock"].ToString().Trim()) - fgen.make_double(dr1["Quantity"].ToString().Trim()) - fgen.make_double(dr1["Total_Outwar_Supply_of_RM"].ToString().Trim()), 2);
                                }
                                dr1["Closing_Stock_Value"] = Math.Round(fgen.make_double(dr1["Weighted_Average_Cost_of_Goods"].ToString().Trim()) * fgen.make_double(dr1["Closing_Balance"].ToString().Trim()), 2);
                                dr1["Total_Invoice"] = Math.Round(fgen.make_double(dr1["Taxable_Value"].ToString().Trim()) + fgen.make_double(dr1["CGST"].ToString().Trim()) + fgen.make_double(dr1["SGST"].ToString().Trim()) + fgen.make_double(dr1["IGST"].ToString().Trim()), 2);
                                    #endregion
                                ph_tbl.Rows.Add(dr1);
                                #endregion
                            }
                        }
                    }
                    //this loop for that items only which are not covered anywhere
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        dr1 = ph_tbl.NewRow(); db10 = 0; string chk_tran = ""; hscode = "";
                        chk_tran = fgen.seek_iname_dt(dt2, "trim(icode)='" + dt3.Rows[i]["ICODE"].ToString().Trim() + "'", "icode");
                        if (chk_tran.Length > 1)
                        {
                        }
                        else
                        {
                            dr1["date"] = "No transaction";// dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["Item_Code"] = dt3.Rows[i]["ICODE"].ToString().Trim();
                            mq6 = dt3.Rows[i]["ICODE"].ToString().Trim().Substring(0, 4);
                            dr1["Category"] = fgen.seek_iname_dt(dt6, "fg_Code='" + mq6 + "'", "category");
                            dr1["Item_Description"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "iname");
                            //dr1["HSN_Code"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                            //db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + dr1["HSN_Code"].ToString().Trim() + "'", "hs_rate"));
                            dr1["HSN_Code"] = "HSN - " + fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                            hscode = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "hscode");
                            db10 = fgen.make_double(fgen.seek_iname_dt(dt4, "acref='" + hscode.Trim() + "'", "hs_rate"));
                            dr1["Applicable_GST_Rate"] = db10;
                            dr1["Weighted_Average_Cost_of_Goods"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "IRATE"));
                            dr1["Measurable_Unit"] = fgen.seek_iname_dt(dt1, "icode='" + dr1["Item_Code"].ToString().Trim() + "'", "unit");
                            dr1["Opening_Balance"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()), 2);
                            dr1["Closing_Balance"] = Math.Round(fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            dr1["Production_Reissue"] = Math.Round(fgen.make_double(dt3.Rows[i]["opening"].ToString().Trim()) - fgen.make_double(dt3.Rows[i]["closing_Stk"].ToString().Trim()), 2);
                            ph_tbl.Rows.Add(dr1);
                        }
                    }
                    //if (ph_tbl.Rows.Count > 0)
                    //{
                    //    Session["send_dt"] = ph_tbl;
                    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    //    fgen.Fn_open_rptlevel("RM Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    //}
                    #endregion
                    ////////for summary
                    #region headings
                    dtm = new DataTable();
                    dtm.Columns.Add("Category", typeof(string));
                    dtm.Columns.Add("Item_Code", typeof(string));
                    dtm.Columns.Add("Item_Description", typeof(string));
                    dtm.Columns.Add("HSN_Code", typeof(string));
                    dtm.Columns.Add("Applicable_GST_Rate", typeof(double));
                    dtm.Columns.Add("Weighted_Average_Cost_of_Goods", typeof(double)); //pic irate from item as on 16 march 2019
                    dtm.Columns.Add("Measurable_Unit", typeof(string));
                    dtm.Columns.Add("Opening_Balance", typeof(double));
                    dtm.Columns.Add("Intra_State", typeof(double));
                    dtm.Columns.Add("Out_of_State", typeof(double));
                    dtm.Columns.Add("Import", typeof(double));
                    dtm.Columns.Add("Branch_Inward_Transfer", typeof(double));
                    dtm.Columns.Add("Received_from_Jobworker_Inw", typeof(double));
                    dtm.Columns.Add("Production_Reissue", typeof(double));
                    dtm.Columns.Add("Total_Inw_Supply", typeof(double));
                    dtm.Columns.Add("Total_Stock", typeof(double));
                    dtm.Columns.Add("Quantity", typeof(double));
                    dtm.Columns.Add("Purchase_Return", typeof(double));
                    dtm.Columns.Add("Branch_Outward_Transfer", typeof(double));
                    dtm.Columns.Add("Sent_for_Jobwork", typeof(double));
                    dtm.Columns.Add("Goods_Lost", typeof(double));
                    dtm.Columns.Add("Stolen", typeof(double));
                    dtm.Columns.Add("Destroyed", typeof(double));
                    dtm.Columns.Add("Written_Off", typeof(double));
                    dtm.Columns.Add("Free_Sample_Gift", typeof(double));
                    dtm.Columns.Add("Personal_Use", typeof(double));
                    //dtm.Columns.Add("Production_Reissue", typeof(double));
                    dtm.Columns.Add("Total_Outwar_Supply_of_RM", typeof(double));
                    dtm.Columns.Add("Closing_Balance", typeof(double));
                    dtm.Columns.Add("Closing_Stock_Value", typeof(double));
                    dtm.Columns.Add("Taxable_Value", typeof(double));
                    dtm.Columns.Add("CGST", typeof(double));
                    dtm.Columns.Add("SGST", typeof(double));
                    dtm.Columns.Add("IGST", typeof(double));
                    dtm.Columns.Add("Total_Invoice", typeof(double));
                    #endregion
                    if (ph_tbl.Rows.Count > 0)
                    {
                        #region RM SUMMARY
                        View1 = new DataView(ph_tbl);
                        dt11 = new DataTable();
                        dt11 = View1.ToTable(true, "Item_Code");
                        foreach (DataRow dr in dt11.Rows)
                        {
                            View2 = new DataView(ph_tbl, "Item_Code='" + dr["Item_Code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt10 = new DataTable();
                            dt10 = View2.ToTable();
                            db_op = 0; //for opening only
                            dr1 = dtm.NewRow();
                            db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0; db21 = 0; db22 = 0; db23 = 0; db24 = 0; db25 = 0; db26 = 0; db27 = 0;
                            for (int i = 0; i < dt10.Rows.Count; i++)
                            {
                                #region
                                dr1["Category"] = dt10.Rows[i]["Category"].ToString().Trim();
                                dr1["Item_Code"] = dt10.Rows[i]["Item_Code"].ToString().Trim();
                                dr1["Item_Description"] = dt10.Rows[i]["Item_Description"].ToString().Trim();
                                // dr1["HSN_Code"] = dt10.Rows[i]["HSN_Code"].ToString().Trim();
                                dr1["HSN_Code"] = "HSN - " + dt10.Rows[i]["HSN_Code"].ToString().Trim();
                                dr1["Applicable_GST_Rate"] = dt10.Rows[i]["Applicable_GST_Rate"].ToString().Trim();
                                dr1["Weighted_Average_Cost_of_Goods"] = dt10.Rows[i]["Weighted_Average_Cost_of_Goods"].ToString().Trim();
                                dr1["Measurable_Unit"] = dt10.Rows[i]["Measurable_Unit"].ToString().Trim();
                                if (i == 0)
                                {
                                    db = fgen.make_double(dt10.Rows[i]["Opening_Balance"].ToString().Trim());
                                }
                                dr1["Opening_Balance"] = db;
                                db1 += fgen.make_double(dt10.Rows[i]["Intra_State"].ToString().Trim());
                                dr1["Intra_State"] = db1;
                                db2 += fgen.make_double(dt10.Rows[i]["Out_of_State"].ToString().Trim());
                                dr1["Out_of_State"] = db2;
                                db3 += fgen.make_double(dt10.Rows[i]["Import"].ToString().Trim());
                                dr1["Import"] = db3;
                                db4 += fgen.make_double(dt10.Rows[i]["Branch_Inward_Transfer"].ToString().Trim());
                                dr1["Branch_Inward_Transfer"] = db4;
                                db5 += fgen.make_double(dt10.Rows[i]["Received_from_Jobworker_Inw"].ToString().Trim());
                                dr1["Received_from_Jobworker_Inw"] = db5;
                                // db6 += fgen.make_double(dt10.Rows[i]["Total_Inw_Supply"].ToString().Trim());
                                db6 = db1 + db2 + db3 + db4 + db5;
                                dr1["Total_Inw_Supply"] = db6;
                                //db7 += fgen.make_double(dt10.Rows[i]["Total_Stock"].ToString().Trim());
                                db7 = db + db6;
                                dr1["Total_Stock"] = db7;
                                db8 += fgen.make_double(dt10.Rows[i]["Quantity"].ToString().Trim());
                                dr1["Quantity"] = db8;
                                db9 += fgen.make_double(dt10.Rows[i]["Purchase_Return"].ToString().Trim());
                                dr1["Purchase_Return"] = db9;
                                db10 += fgen.make_double(dt10.Rows[i]["Branch_Outward_Transfer"].ToString().Trim());
                                dr1["Branch_Outward_Transfer"] = db10;
                                db11 += fgen.make_double(dt10.Rows[i]["Sent_for_Jobwork"].ToString().Trim());
                                dr1["Sent_for_Jobwork"] = db11;
                                db12 += fgen.make_double(dt10.Rows[i]["Goods_Lost"].ToString().Trim());
                                dr1["Goods_Lost"] = db12;
                                db13 += fgen.make_double(dt10.Rows[i]["Stolen"].ToString().Trim());
                                dr1["Stolen"] = db13;
                                db14 += fgen.make_double(dt10.Rows[i]["Destroyed"].ToString().Trim());
                                dr1["Destroyed"] = db14;
                                db15 += fgen.make_double(dt10.Rows[i]["Written_Off"].ToString().Trim());
                                dr1["Written_Off"] = db15;
                                db16 += fgen.make_double(dt10.Rows[i]["Free_Sample_Gift"].ToString().Trim());
                                dr1["Free_Sample_Gift"] = db16;
                                db17 += fgen.make_double(dt10.Rows[i]["Personal_Use"].ToString().Trim());
                                dr1["Personal_Use"] = db17;
                                db18 += fgen.make_double(dt10.Rows[i]["Production_Reissue"].ToString().Trim());
                                dr1["Production_Reissue"] = db18;
                                // db19 += fgen.make_double(dt10.Rows[i]["Total_Outwar_Supply_of_RM"].ToString().Trim());
                                db19 = db9 + db10 + db11 + db12 + db13 + db14 + db15 + db16 + db17;
                                dr1["Total_Outwar_Supply_of_RM"] = db19;
                                // db20 += fgen.make_double(dt10.Rows[i]["Closing_Balance"].ToString().Trim());
                                db20 = fgen.make_double(dt10.Rows[i]["Closing_Balance"].ToString().Trim());
                                dr1["Closing_Balance"] = db20;
                                db21 += fgen.make_double(dt10.Rows[i]["Closing_Stock_Value"].ToString().Trim());
                                dr1["Closing_Stock_Value"] = db21;
                                db22 += fgen.make_double(dt10.Rows[i]["Taxable_Value"].ToString().Trim());
                                dr1["Taxable_Value"] = db22;
                                db23 += fgen.make_double(dt10.Rows[i]["CGST"].ToString().Trim());
                                dr1["CGST"] = db23;
                                db24 += fgen.make_double(dt10.Rows[i]["SGST"].ToString().Trim());
                                dr1["SGST"] = db24;
                                db25 += fgen.make_double(dt10.Rows[i]["IGST"].ToString().Trim());
                                dr1["IGST"] = db25;
                                db26 += fgen.make_double(dt10.Rows[i]["Total_Invoice"].ToString().Trim());
                                dr1["Total_Invoice"] = db26;
                                #endregion
                            }
                            dtm.Rows.Add(dr1);
                        }
                        #endregion
                    }
                    if (dtm.Rows.Count > 0)
                    {
                        Session["send_dt"] = dtm;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        fgen.Fn_open_rptlevelJS("RM Report For the Period " + fromdt + " To " + todt, frm_qstr);
                    }
                    break;

                case "F70375"://HSN WISE SUMMARY
                    header_n = "Dr./Cr. Note Details";
                    SQuery = "Select a.branchcd||a.type||'-'||A.vchnum as Note_num,to_char(a.vchdate,'dd/mm/yyyy') as Note_dt,b.aname,b.gst_no,b.staten,b.staffcd as St_code,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,c.iname,(CASE WHEN LENGTH(TRIM(A.EXC_57F4))>2 THEN A.EXC_57F4 ELSE c.cpartno END) AS cpartno,c.hscode,a.iqty_chl as qty,a.irate,a.iamount,replace(replace(replace(trim(a.naration),chr(13),''),chr(9),''),chr(10),'') as naration,round(a.iqty_chl*a.iexc_Addl,2) as Txb_Chgs,a.iopr as TX_type,(Case when trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + mbr + "' and a.type in ('58','59') and a.vchdate between to_Date('" + value1 + "','dd/mm/yyyy') and to_date('" + value2 + "','dd/mm/yyyy')  order by to_char(a.vchdate,'dd/mm/yyyy'),a.branchcd||a.type||'-'||A.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "F70375");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_HEADER", header_n);
                    fgen.Fn_open_rptlevel("HSN wise Purchase Non MRR Data for the Period " + value1 + " To " + value2, frm_qstr);
                    break;

                case "F70206":
                    SQuery = "SELECT 'N' AS FSTR, 'DO YOU WANT TO SEE PENDING FOR CHECKING' AS MSG FROM DUAL UNION ALL SELECT 'Y' AS FSTR,'DO YOU WANT TO SEE PENDING FOR APPROVAL' AS MSG  FROM DUAL UNION ALL SELECT 'ALL' AS FSTR, 'DO YOU WANT TO SEE ALL' AS MSG  FROM DUAL";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Select Report Pattern", frm_qstr);
                    break;
                case "F70207":
                    DataTable dticode2 = new DataTable();
                    DataTable Dtmat = new DataTable();
                    Dtmat.Columns.Add("TYPE", typeof(string));
                    Dtmat.Columns.Add("checkby", typeof(string));
                    Dtmat.Columns.Add("app_by", typeof(string));

                    dt = new DataTable();
                    mq0 = "SELECT TYPE1 AS TYPE,NAME FROM TYPE WHERE ID='V' ORDER BY TYPE1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dt1 = new DataTable();
                    mq1 = "SELECT  A.BRANCHCD,A.ACODE,B.USERNAME ,A.FIXEDON,A.ALLOWEDBR,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM POMST A,EVAS B WHERE TRIM(A.ACODE)=TRIM(B.USERID) AND A.BRANCHCD='" + mbr + "' AND A.TYPE='21' AND A.VCHDATE " + xprdrange + " ORDER BY VDD";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                    dt2 = new DataTable();
                    cond = " and ((case when substr(type,1,1) in ('2','3','4') then a.tfccr when type='59' then a.tfccr when substr(type,1,1) in ('1','5','6') then a.tfcdr end)>0 or (case when substr(type,1,1) in ('2','3','4') then a.cramt when type='59' then a.cramt when substr(type,1,1) in ('1','5','6') then a.dramt end)>0)  ";
                    cond1 = "(case when substr(type,1,1) in ('2','3','4') then a.cramt when type='59' then a.cramt when substr(type,1,1) in ('1','5','6') then a.dramt end)";
                    mq2 = "SELECT a.branchcd, A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE," + cond1 + " as amount, A.ENT_BY,TO_CHAR(A.ENT_DATE,'DD/MM/YYYY') AS ENT_DATE,TRIM(B.ANAME) AS ANAME FROM VOUCHER A, FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + mbr + "' AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " " + cond + "";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);

                    foreach (DataRow dr0 in dt.Rows)
                    {
                        mq3 = dr0["TYPE"].ToString().Trim();
                        dr2 = Dtmat.NewRow();
                        dr2["type"] = mq3;
                        foreach (DataRow drdt1 in dt1.Rows)
                        {
                            //string[] arr; string[] arr1;
                            mq4 = drdt1["FIXEDON"].ToString().Trim();
                            //arr = mq4.Split(';');
                            for (int i = 0; i < mq4.Split(';').Length; i++)
                            {
                                if (mq3 == mq4.Split(';')[i])
                                {
                                    dr2["checkby"] += drdt1["USERNAME"].ToString().Trim() + ",";
                                }
                            }
                            mq5 = drdt1["ALLOWEDBR"].ToString().Trim();
                            //arr1 = mq5.Split(';');
                            for (int j = 0; j < mq5.Split(';').Length; j++)
                            {
                                if (mq3 == mq5.Split(';')[j])
                                {
                                    dr2["app_by"] += drdt1["USERNAME"].ToString().Trim() + ",";
                                }
                            }
                        }
                        mq7 = dr2["checkby"].ToString();
                        mq8 = dr2["app_by"].ToString();
                        dr2["checkby"] = mq7.TrimEnd(',');
                        dr2["app_by"] = mq8.TrimEnd(',');
                        Dtmat.Rows.Add(dr2);
                    }

                    dticode2.Columns.Add("BRANCHCD", typeof(string));
                    dticode2.Columns.Add("TYPE", typeof(string));
                    dticode2.Columns.Add("VOUCHER_NO", typeof(string));
                    dticode2.Columns.Add("VOUCHER_DATE", typeof(string));
                    dticode2.Columns.Add("PARTY_CODE", typeof(string));
                    dticode2.Columns.Add("PARTY_NAME", typeof(string));
                    dticode2.Columns.Add("Amount", typeof(string));
                    dticode2.Columns.Add("ENTRY_BY", typeof(string));
                    dticode2.Columns.Add("ENTRY_DATE", typeof(string));
                    dticode2.Columns.Add("CHECKER", typeof(string));
                    dticode2.Columns.Add("APPROVER", typeof(string));

                    if (dt2.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt2);
                        dt4 = new DataTable();
                        dt4 = view1im.ToTable(true, "type");
                        DataTable dticode = new DataTable();
                        DataRow dr4;
                        foreach (DataRow dr3 in dt4.Rows)
                        {
                            DataView view1 = new DataView(dt2, "type='" + dr3["type"] + "'", "", DataViewRowState.CurrentRows);
                            dticode = view1.ToTable();
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                dr4 = dticode2.NewRow();
                                dr4["BRANCHCD"] = dticode.Rows[i]["BRANCHCD"].ToString().Trim();
                                dr4["type"] = dticode.Rows[i]["type"].ToString().Trim();
                                dr4["VOUCHER_NO"] = dticode.Rows[i]["VCHNUM"].ToString().Trim();
                                dr4["VOUCHER_DATE"] = dticode.Rows[i]["VCHDATE"].ToString().Trim();
                                dr4["PARTY_CODE"] = dticode.Rows[i]["ACODE"].ToString().Trim();
                                dr4["PARTY_NAME"] = dticode.Rows[i]["ANAME"].ToString().Trim();
                                dr4["Amount"] = dticode.Rows[i]["Amount"].ToString().Trim();
                                dr4["ENTRY_BY"] = dticode.Rows[i]["ENT_BY"].ToString().Trim();
                                dr4["ENTRY_DATE"] = dticode.Rows[i]["ENT_DATE"].ToString().Trim();
                                dr4["CHECKER"] = fgen.seek_iname_dt(Dtmat, "type='" + dticode.Rows[i]["type"].ToString().Trim() + "'", "app_by");
                                dr4["APPROVER"] = fgen.seek_iname_dt(Dtmat, "type='" + dticode.Rows[i]["type"].ToString().Trim() + "'", "checkby");
                                dticode2.Rows.Add(dr4);
                            }
                        }
                    }
                    Session["send_dt"] = dticode2;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevelJS("Voucher Approval Report For the Period " + fromdt + " To " + todt + " ", frm_qstr);
                    break;

                case "F70377":
                    #region ADVG Outstanding Report
                    dtm = new DataTable();
                    dtm.Columns.Add("Branch_Code", typeof(string));
                    dtm.Columns.Add("Company", typeof(string));
                    mq1 = "SELECT TO_CHAR(FMDATE,'YYYY')||'-'||TO_CHAR(TODATE,'YYYY') AS DATE_ FROM CO WHERE FMDATE>=TO_DATE('01/04/2014','DD/MM/YYYY') ORDER BY FMDATE";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, co_cd, mq1);
                    foreach (DataRow dr in dt1.Rows)
                    {
                        dtm.Columns.Add(dr["DATE_"].ToString(), typeof(double)); ;
                    }
                    dtm.Columns.Add("Total_OS", typeof(double));
                    dtm.Columns.Add("UnClaimed_Amt_Due_To_Legal_Dosseir_BG", typeof(double));
                    dtm.Columns.Add("Net_Recoverable", typeof(double));
                    dtm.Columns.Add("Below_45_Day", typeof(double));
                    dtm.Columns.Add("45-90_Days", typeof(double));
                    dtm.Columns.Add("91-180_Days", typeof(double));
                    dtm.Columns.Add("More_Than_180_Days", typeof(double));
                    dtm.Columns.Add("Billing_Last_12_Month", typeof(double));
                    dtm.Columns.Add("Debtor_Days", typeof(double));
                    dtm.Columns.Add("Weighted_Age", typeof(double));

                    mq0 = "SELECT sum(a.net) as net,a.branchcd,t.name,to_char(a.invdate,'mm/yyyy') as invdate,to_char(a.invdate,'yyyy') as yr,to_char(a.invdate,'yyyymm') as mth FROM recdata a,type t,famst f WHERE trim(a.acode)=trim(f.acode) and trim(a.branchcd)=trim(t.type1) and a.branchcd in ('00','03','04','07') and t.id='B' and substr(trim(a.acode),1,2) in ('16','18') and a.net>0 and a.invdate<=to_date('" + value1 + "','dd/mm/yyyy') group by a.branchcd,t.name,to_char(a.invdate,'mm/yyyy'),to_char(a.invdate,'yyyymm'),to_char(a.invdate,'yyyy') order by branchcd,mth";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, mq0);

                    mq2 = "select trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,sum(a.net) as net,a.branchcd,trim(a.acode) as acode from recdata a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd!='DD' and substr(trim(a.acode),1,2) in ('16','18') and a.net>0 group by a.branchcd,trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),trim(a.acode) order by branchcd,invno,invdate";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, co_cd, mq2);

                    // UN CLAIMED FORM
                    mq3 = "select distinct trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,branchcd,trim(acode) as acode from wb_dosdoc where branchcd!='DD' and (legal='Y' or dossier='Y' or bg='Y')";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, co_cd, mq3);

                    // PICKING NEW YEAR AFTER 2019 FROM CO TABLE
                    mq4 = "select to_char(fmdate,'yyyymm') as fmdate,to_char(todate,'yyyymm') as todate,to_char(fmdate,'yyyy')||'-'||to_char(todate,'yyyy') as yr from co where FMDATE>=TO_DATE('31/03/2020','DD/MM/YYYY')";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, co_cd, mq4);

                    mq5 = "SELECT branchcd,sum(slab1) as below45,sum(slab2) as days45_90,sum(slab3) as days91_180,sum(slab4) as more180 from (select a.branchcd,(case when (to_date('" + value1 + "','dd/mm/yyyy')-a.invdate <45) then net end) as slab1,(case when (to_date('" + value1 + "','dd/mm/yyyy')-a.invdate between 45 and 90) then net end) as slab2,(case when (to_date('" + value1 + "','dd/mm/yyyy')-a.invdate between 91 and 180) then net end) as slab3,(case when (to_date('" + value1 + "','dd/mm/yyyy')-a.invdate > 180) then net end) as slab4,a.invdate,to_char(a.invdate,'yyyymmdd') as vdd from recdata a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd!='DD' and substr(trim(a.acode),1,2) in ('16','18') and a.net>0 order by vdd) group by branchcd order by branchcd";
                    dt5 = new DataTable();
                    dt5 = fgen.getdata(frm_qstr, co_cd, mq5);

                    er1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + value1 + "','dd/mm/yyyy'),-12)+1,'mm'),'mm/YYYY') as lastmth from dual", "lastmth");
                    er2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(trunc(add_months(to_datE('" + value1 + "','dd/mm/yyyy'),-1),'mm'),'mm/YYYY') as prevmth from dual", "prevmth");
                    mq6 = "select sum(net) as net,branchcd from recdata where branchcd!='DD' and substr(trim(acode),1,2) in ('16','18') and net>0 and invdate between to_date('" + er1 + "','mm/yyyy') and to_date('" + er2 + "','mm/yyyy') group by branchcd order by branchcd";
                    mq6 = "select branchcd,sum(bill_tot) as net from sale where branchcd!='DD' and type like '4%' and vchdate between to_date('" + er1 + "','mm/yyyy') and to_date('" + er2 + "','mm/yyyy') group by branchcd order by branchcd";
                    dt6 = new DataTable();
                    dt6 = fgen.getdata(frm_qstr, co_cd, mq6);

                    // TOTAL OUTSTANDING VALUE
                    mq7 = "select branchcd,sum(days*net) as outstanding_value,SUM(NET) AS NET from (select a.branchcd,a.acode,a.invno,a.invdate,a.net,replace(nvl(trim(f.payment),'0'),'-','0') as pterms,to_date('" + value1 + "','dd/mm/yyyy')-to_date(to_char(invdate,'dd/mm/yyyy'),'dd/mm/yyyy') as days from recdata a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd!='DD' and a.net>0 and substr(trim(a.acode),1,2) in ('16','18')) group by branchcd order by branchcd";
                    dticode2 = new DataTable();
                    dticode2 = fgen.getdata(frm_qstr, co_cd, mq7);

                    if (dt.Rows.Count > 0)
                    {
                        view1 = new DataView(dt);
                        dt7 = new DataTable();
                        dt7 = view1.ToTable(true, "branchcd", "name");
                        foreach (DataRow dr3 in dt7.Rows)
                        {
                            view2 = new DataView(dt, "branchcd='" + dr3["branchcd"].ToString().Trim() + "' and name='" + dr3["name"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt8 = new DataTable();
                            dt8 = view2.ToTable();
                            for (int i = 0; i < dt8.Rows.Count; i++)
                            {
                                oporow = dtm.NewRow();
                                oporow["Branch_Code"] = dr3["branchcd"].ToString().Trim();
                                oporow["company"] = dr3["name"].ToString().Trim();
                                if (fgen.make_double(dt8.Rows[i]["yr"].ToString()) <= 2014)
                                {
                                    oporow["2014-2015"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                else if (fgen.make_double(dt8.Rows[i]["yr"].ToString()) == 2015 && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= 201503)
                                {
                                    oporow["2014-2015"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                else if (fgen.make_double(dt8.Rows[i]["mth"].ToString()) >= 201504 && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= 201603)
                                {
                                    oporow["2015-2016"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                else if (fgen.make_double(dt8.Rows[i]["mth"].ToString()) >= 201604 && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= 201703)
                                {
                                    oporow["2016-2017"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                else if (fgen.make_double(dt8.Rows[i]["mth"].ToString()) >= 201704 && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= 201803)
                                {
                                    oporow["2017-2018"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                else if (fgen.make_double(dt8.Rows[i]["mth"].ToString()) >= 201804 && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= 201903)
                                {
                                    oporow["2018-2019"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                else if (fgen.make_double(dt8.Rows[i]["mth"].ToString()) >= 201904 && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= 202003)
                                {
                                    oporow["2019-2020"] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                }
                                for (m = 0; m < dt4.Rows.Count; m++)
                                {
                                    if (fgen.make_double(dt8.Rows[i]["mth"].ToString()) >= fgen.make_double(dt4.Rows[m]["fmdate"].ToString()) && fgen.make_double(dt8.Rows[i]["mth"].ToString()) <= fgen.make_double(dt4.Rows[m]["todate"].ToString()))
                                    {
                                        oporow[dt4.Rows[m]["yr"].ToString()] = fgen.make_double(dt8.Rows[i]["net"].ToString());
                                    }
                                }
                                dtm.Rows.Add(oporow);
                            }
                        }

                        if (dtm.Rows.Count > 0)
                        {
                            view1 = new DataView(dtm);
                            dt7 = new DataTable();
                            dt7 = view1.ToTable(true, "Branch_Code", "company");
                            mdt = new DataTable();
                            mdt = dtm.Clone(); oporow = null;
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0;
                            foreach (DataRow dr in dt7.Rows)
                            {
                                oporow = mdt.NewRow();
                                oporow["Branch_Code"] = dr["Branch_Code"].ToString().Trim();
                                oporow["company"] = dr["company"].ToString().Trim();
                                db2 = 0; db3 = 0;
                                dt9 = new DataTable(); dt10 = new DataTable(); dticode = new DataTable();
                                view2 = new DataView(dtm, "Branch_Code='" + dr["Branch_Code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                dt8 = new DataTable();
                                dt8 = view2.ToTable();
                                if (dt3.Rows.Count > 0)
                                {
                                    // UN CLAIMED FORM'S DATA
                                    dv = new DataView(dt3, "branchcd='" + dr["Branch_Code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dt9 = dv.ToTable();
                                }
                                if (dt5.Rows.Count > 0)
                                {
                                    // SLAB
                                    DataView dv1 = new DataView(dt5, "branchcd='" + dr["Branch_Code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dt10 = dv1.ToTable();
                                }
                                if (dt6.Rows.Count > 0)
                                {
                                    DataView dv2 = new DataView(dt6, "branchcd='" + dr["Branch_Code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dticode = dv2.ToTable();
                                }
                                foreach (DataRow dr3 in dt1.Rows)
                                {
                                    db1 = 0;
                                    for (int l = 0; l < dt8.Rows.Count; l++)
                                    {
                                        if (dt8.Columns[dr3["date_"].ToString()].ToString() == dr3["date_"].ToString())
                                        {
                                            db1 += fgen.make_double(dt8.Rows[l][dr3["date_"].ToString()].ToString());
                                            oporow[dr3["date_"].ToString()] = Math.Round(db1, 2);
                                            db2 += fgen.make_double(dt8.Rows[l][dr3["date_"].ToString()].ToString());
                                        }
                                    }
                                }
                                oporow["Total_OS"] = Math.Round(db2, 2);
                                foreach (DataRow dr2 in dt9.Rows)
                                {
                                    db3 += fgen.make_double(fgen.seek_iname_dt(dt2, "invno='" + dr2["invno"].ToString() + "' and invdate='" + dr2["invdate"].ToString() + "' and acode='" + dr2["acode"].ToString() + "'", "net"));
                                }

                                oporow["UnClaimed_Amt_Due_To_Legal_Dosseir_BG"] = Math.Round(db3, 2);
                                oporow["Net_Recoverable"] = Math.Round(db2 - db3, 2);
                                if (dt10.Rows.Count > 0)
                                {
                                    oporow["Below_45_Day"] = Math.Round(fgen.make_double(dt10.Rows[0]["below45"].ToString()), 2);
                                    oporow["45-90_Days"] = Math.Round(fgen.make_double(dt10.Rows[0]["days45_90"].ToString()), 2);
                                    oporow["91-180_Days"] = Math.Round(fgen.make_double(dt10.Rows[0]["days91_180"].ToString()), 2);
                                    oporow["More_Than_180_Days"] = Math.Round(fgen.make_double(dt10.Rows[0]["more180"].ToString()), 2);
                                }
                                if (dticode.Rows.Count > 0)
                                {
                                    oporow["Billing_Last_12_Month"] = Math.Round(fgen.make_double(dticode.Rows[0]["net"].ToString()), 2);
                                }
                                oporow["Debtor_Days"] = Math.Round(((db2 - db3) * 365) / fgen.make_double(oporow["Billing_Last_12_Month"].ToString()), 2).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0");
                                if (dticode2.Rows.Count > 0)
                                {
                                    db6 = fgen.make_double(fgen.seek_iname_dt(dticode2, "branchcd='" + dr["Branch_Code"].ToString().Trim() + "'", "outstanding_value"));
                                }
                                oporow["Weighted_Age"] = Math.Round(db2 / db6, 6);
                                mdt.Rows.Add(oporow);
                            }
                        }
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    Session["send_dt"] = mdt;
                    fgen.Fn_open_rptlevelJS("Outstanding Report As On Date " + value1 + "", frm_qstr);
                    #endregion
                    break;

                case "M1":
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select acref3,acref4 from typegrp where id='FA' and type1='" + hf1.Value.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        string vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
                        xprdrange = hfcode.Value;
                        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(cramt) as cramt from wb_fa_vch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hfval.Value.Trim() + "'", "cramt");
                        string frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM voucher WHERE BRANCHCD='" + mbr + "' AND type='30' and vchdate " + xprdrange + "", 6, "VCH");
                        fgen.vSave(frm_qstr, frm_cocd, mbr, "30", frm_vnum, Convert.ToDateTime(value1), 1, dt.Rows[0]["acref3"].ToString().Trim(), dt.Rows[0]["acref4"].ToString().Trim(), fgen.make_double(mq1), 0, "-", Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MM/yyyy")), "POST DEP- FOR " + hfval.Value.Substring(4, 6) + " DATED " + Convert.ToDateTime(hfval.Value.Substring(10, 10)).ToString("dd/MM/yyyy") + " GRPCODE " + hf1.Value + "", 0, 0, 1, fgen.make_double(mq1), 0, hfval.Value.Substring(4, 6), Convert.ToDateTime(hfval.Value.Substring(10, 10)), uname, Convert.ToDateTime(vardate), "-", 0, 0, "-", "-", Convert.ToDateTime(vardate), "-", "VOUCHER", "-");
                        fgen.vSave(frm_qstr, frm_cocd, mbr, "30", frm_vnum, Convert.ToDateTime(value1), 2, dt.Rows[0]["acref4"].ToString().Trim(), dt.Rows[0]["acref3"].ToString().Trim(), 0, fgen.make_double(mq1), "-", Convert.ToDateTime(DateTime.Now.Date.ToString("dd/MM/yyyy")), "POST DEP- FOR " + hfval.Value.Substring(4, 6) + " DATED " + Convert.ToDateTime(hfval.Value.Substring(10, 10)).ToString("dd/MM/yyyy") + " GRPCODE " + hf1.Value + "", 0, 0, 1, 0, fgen.make_double(mq1), hfval.Value.Substring(4, 6), Convert.ToDateTime(hfval.Value.Substring(10, 10)), uname, Convert.ToDateTime(vardate), "-", 0, 0, "-", "-", Convert.ToDateTime(vardate), "-", "VOUCHER", "-");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update wb_fa_vch set post='Y' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hfval.Value.Trim() + "' and grpcode='" + hf1.Value.Trim() + "'");
                        fgen.msg("-", "AMSG", "Data is Saved");
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "No Account Code is linked to this Group Code");
                    }
                    break;

                case "F70189":
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    //fgen.drillQuery(0, "SELECT ACODE AS FSTR,'-' AS GSTR,ANAME,ACODE,ADDR1,ADDR2,GRP,MKTGGRP,PNAME,TELNUM,PERSON,STATEN,DISTRICT,MOBILE,EMAIL FROM FAMST ORDER BY ANAME,ACODE", frm_qstr);
                    //fgen.drillQuery(1, "SELECT FSTR||MAX(trim(GSTR)) as fstr,MAX(trim(GSTR)) AS GSTR,MTHNAME,SUM(DRAMT) AS DEBITS,SUM(CRAMT) AS CREDITS,sum(mthsno) as srno FROM (SELECT TRIM(MTHNUM) AS FSTR,NULL AS GSTR,UPPER(TRIM(MTHNAME)) AS MTHNAME,0 AS DRAMT,0 AS CRAMT,mthsno FROM MTHS2 UNION ALL SELECT TRIM(TO_CHAR(VCHDATE,'MM')) AS FSTR,TRIM(aCODe) AS GSTR,TRIM(TO_cHAR(VCHDATE,'MONTH')) as Mthname,(dramt) as debits,(cramt) as credits,0 as mthsno FROM VOUCHER WHERE " + branch_Cd + " AND TYPE LIKE '%' AND VCHDATE " + xprdrange + " AND ACODE='FSTR' ) GROUP BY FSTR,MTHNAME order by srno", frm_qstr);
                    //fgen.drillQuery(2, "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(to_char(a.vchdate,'MM'))||trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.DRAMT) AS DEBIT,(a.CRAMT) AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.TAX,A.STAX,A.ST_ENTFORM,A.BRANCHCD PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprdrange + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR' ", frm_qstr);
                    cond = "";
                    if (hfbr.Value == "ABR") cond = "Consolidated";
                    else cond = "Branch Wise(" + mbr + ")";

                    mq0 = "select substr(A.acode,1,2) as GRP,sum(a.opening) as opening,sum(a.cdr) as debits,sum(a.ccr) as credits,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing from (Select trim(acode) as acode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " and SUBSTR(ACODE,1,2)>'1Z' union all  ";
                    mq1 = "select trim(acode) as acode ,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and SUBSTR(ACODE,1,2)>'1Z' GROUP BY trim(aCODE) union all ";
                    mq2 = "select trim(acode) as acode ,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos  from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and SUBSTR(ACODE,1,2)>'1Z' GROUP BY trim(aCODE))a, famst b where trim(A.acode)=trim(B.acode) group by substr(A.acode,1,2) order by substr(A.acode,1,2)";
                    SQuery = mq0 + mq1 + mq2;
                    SQuery = "select '-' as fstr,'-' as gstr,A.name as Group_Name,A.type1 as Group_Code, to_char(B.Credits,'99999999999.00') as Incomes, to_char(B.Debits,'99999999999.00') as  Expenses from type a ,(" + SQuery + ")  b where a.id='Z' and  trim(A.TYPE1)=trim(B.GRP)  AND abs(b.debits)+abs(b.credits)+abs(b.opening)+abs(b.closing) <>0 AND A.TYPE1>'1Z'";


                    //mq0 = "select substr(A.acode,1,2) as GRP,sum(a.opening) as opening,sum(a.cdr) as debits,sum(a.ccr) as credits,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing from (Select trim(acode) as acode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " and SUBSTR(ACODE,1,2)>'1Z' union all  ";
                    //mq1 = "select trim(acode) as acode ,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and SUBSTR(ACODE,1,2)>'1Z' GROUP BY trim(aCODE) union all ";
                    //mq2 = "select trim(acode) as acode ,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos  from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and SUBSTR(ACODE,1,2)>'1Z' GROUP BY trim(aCODE))a, famst b where trim(A.acode)=trim(B.acode) group by substr(A.acode,1,2) order by substr(A.acode,1,2)";
                    //SQuery = mq0 + mq1 + mq2;
                    //SQuery = "select '-' as fstr,'-' as gstr,A.name as Group_Name,A.type1 as Group_Code,to_char((CASE WHEN (b.closing <0) THEN abs(b.closing) END),'99999999999.00') as Incomes,to_char((CASE WHEN (b.closing >0) THEN b.closing END),'99999999999.00') as Assets from type a ,(" + SQuery + ")  b where a.id='Z' and  trim(A.TYPE1)=trim(B.GRP)  AND abs(b.debits)+abs(b.credits)+abs(b.opening)+abs(b.closing) <>0 AND A.TYPE1>'1Z'";
                    fgen.drillQuery(0, SQuery, frm_qstr, "4#6#", "3#5#", "450#450#");

                    SQuery1 = "SELECT * FROM (SELECT substr(trim(A.ACODE),1,4) AS FSTR,substr(trim(A.ACODE),1,2) AS GSTR,b.name AS ACCOUNT,sum(A.DRAMT) AS DEBIT,sum(a.CRAMT) AS CREDITS  FROM VOUCHER A,typegrp B WHERE substr(trim(a.aCODE),1,4)=TRIM(b.type1) and b.id='A' AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprd2 + " group by substr(trim(A.ACODE),1,4),b.name,substr(trim(A.ACODE),1,2))";

                    SQuery1 = "select b.bssch as fstr,c.type1 as gstr,d.name as account,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl,b.bssch from (Select branchcd,trim(acode) as acode, sum(yr_" + year + ") as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " group by trim(acode),branchcd  union all select branchcd,trim(acode),sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdRange1 + " GROUP BY trim(aCODE),branchcd union all select branchcd,trim(acode),0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " GROUP BY trim(ACODE),branchcd) a,famst b,type c,typegrp d where TRIM(b.bssch)=trim(d.type1) and substr(TRIM(A.acode),1,2)=trim(c.type1) and d.id='A' and c.id='Z' and trim(a.acode)=trim(b.acode)  group by a.branchcd,c.name,d.name,b.bssch,c.type1 having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) order by c.type1";
                    fgen.drillQuery(1, SQuery1, frm_qstr, "5#6#7#8#", "3#", "450#");

                    SQuery2 = "select trim(a.acode) as fstr,b.bssch as gstr,b.aname,c.name as mgname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl, a.branchcd,trim(a.acode) as acode from (Select branchcd,trim(acode) as acode, sum(yr_" + year + ") as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " group by trim(acode),branchcd  union all select branchcd,trim(acode),sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdRange1 + " GROUP BY trim(aCODE),branchcd union all select branchcd,trim(acode),0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " GROUP BY trim(ACODE),branchcd) a,famst b,type c,typegrp d where TRIM(b.bssch)=trim(d.type1) and substr(TRIM(A.acode),1,2)=trim(c.type1) and d.id='A' and c.id='Z' and trim(a.acode)=trim(b.acode) group by a.branchcd,trim(a.acode),b.aname,c.name,d.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0)";
                    fgen.drillQuery(2, SQuery2, frm_qstr, "5#6#7#8#", "3#4#", "250#200#");

                    SQuery3 = "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.DRAMT) AS DEBIT,(a.CRAMT) AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.TAX,A.STAX,A.ST_ENTFORM,A.BRANCHCD PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprd2 + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'";
                    fgen.drillQuery(3, SQuery3, frm_qstr);

                    fgen.Fn_DrillReport("Profit & Loss Statement " + cond + "for the period " + value1 + " To " + value2 + "", frm_qstr);
                    break;
                case "F70156":
                    header_n = "Balance Sheet";
                    //SQuery = "select A.name as Group_Name,A.type1 as Group_Code,to_char((CASE WHEN (b.closing <0) THEN abs(b.closing) END),'9999999990.00') as Liabilities,to_char((CASE WHEN (b.closing >0) THEN b.closing END),'9999999990.00') as Assets   from type a ,(select substr(A.acode,1,2) as GRP,sum(a.opening) as opening,sum(a.cdr) as debits,sum(a.ccr) as credits,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing from (Select trim(acode) as acode, YR_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal where branchcd  in ('" + mbr + "') and SUBSTR(ACODE,1,2)<='1Z' union all  select trim(acode) as acode ,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos from voucher where branchcd  in ('" + mbr + "') and type like '%' and vchdate " + xprd1 + " and SUBSTR(ACODE,1,2)<='1Z' GROUP BY trim(aCODE) union all select trim(acode) as acode ,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos  from voucher where branchcd  in ('" + mbr + "')  and type like '%' and vchdate " + xprdrange + " and SUBSTR(ACODE,1,2)<='1Z' GROUP BY trim(aCODE) ) a, famst b where trim(A.acode)=trim(B.acode) group by substr(A.acode,1,2) order by substr(A.acode,1,2))  b where a.id='Z' and  trim(A.TYPE1)=trim(B.GRP)  AND abs(b.debits)+abs(b.credits)+abs(b.opening)+abs(b.closing) <>0 AND A.TYPE1<='1Z'";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel(header_n + " for the Period " + value1 + " To " + value2, frm_qstr);
                    xprdrange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                    mq0 = "select substr(A.acode,1,2) as GRP,sum(a.opening) as opening,sum(a.cdr) as debits,sum(a.ccr) as credits,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing from (Select trim(acode) as acode, yr_" + year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " and SUBSTR(ACODE,1,2)<='1Z' union all  ";
                    mq1 = "select trim(acode) as acode ,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange1 + " and SUBSTR(ACODE,1,2)<='1Z' GROUP BY trim(aCODE) union all ";
                    mq2 = "select trim(acode) as acode ,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos  from voucher where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " and SUBSTR(ACODE,1,2)<='1Z' GROUP BY trim(aCODE))a, famst b where trim(A.acode)=trim(B.acode) group by substr(A.acode,1,2) order by substr(A.acode,1,2)";
                    SQuery = mq0 + mq1 + mq2;
                    SQuery = "select '-' as fstr,'-' as gstr,A.name as Group_Name,A.type1 as Group_Code,to_char((CASE WHEN (b.closing <0) THEN abs(b.closing) END),'99999999999.00') as Liabilities,to_char((CASE WHEN (b.closing >0) THEN b.closing END),'99999999999.00') as Assets   from type a ,(" + SQuery + ")  b where a.id='Z' and  trim(A.TYPE1)=trim(B.GRP)  AND abs(b.debits)+abs(b.credits)+abs(b.opening)+abs(b.closing) <>0 AND A.TYPE1<='1Z'";

                    fgen.drillQuery(0, SQuery, frm_qstr, "4#6#", "3#5#", "450#450#");
                    SQuery1 = "SELECT * FROM (SELECT substr(trim(A.ACODE),1,4) AS FSTR,substr(trim(A.ACODE),1,2) AS GSTR,b.name AS ACCOUNT,sum(A.DRAMT) AS DEBIT,sum(a.CRAMT) AS CREDITS  FROM VOUCHER A,typegrp B WHERE substr(trim(a.aCODE),1,4)=TRIM(b.type1) and b.id='A' AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprd2 + " group by substr(trim(A.ACODE),1,4),b.name,substr(trim(A.ACODE),1,2))";

                    SQuery1 = "select b.bssch as fstr,c.type1 as gstr,d.name as account,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl,b.bssch from (Select branchcd,trim(acode) as acode, sum(yr_" + year + ") as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " group by trim(acode),branchcd  union all select branchcd,trim(acode),sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdRange1 + " GROUP BY trim(aCODE),branchcd union all select branchcd,trim(acode),0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " GROUP BY trim(ACODE),branchcd) a,famst b,type c,typegrp d where TRIM(b.bssch)=trim(d.type1) and substr(TRIM(A.acode),1,2)=trim(c.type1) and d.id='A' and c.id='Z' and trim(a.acode)=trim(b.acode)  group by a.branchcd,c.name,d.name,b.bssch,c.type1 having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0) order by c.type1";
                    fgen.drillQuery(1, SQuery1, frm_qstr, "5#6#7#8#", "3#", "450#");

                    SQuery2 = "select trim(a.acode) as fstr,b.bssch as gstr,b.aname,c.name as mgname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as drmt,nvl(sum(a.ccr),0) as crmt,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl, a.branchcd,trim(a.acode) as acode from (Select branchcd,trim(acode) as acode, sum(yr_" + year + ") as opening,0 as cdr,0 as ccr,0 as clos from famstbal where " + branch_Cd + " group by trim(acode),branchcd  union all select branchcd,trim(acode),sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdRange1 + " GROUP BY trim(aCODE),branchcd union all select branchcd,trim(acode),0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where " + branch_Cd + " and type like '%' and vchdate " + xprdrange + " GROUP BY trim(ACODE),branchcd) a,famst b,type c,typegrp d where TRIM(b.bssch)=trim(d.type1) and substr(TRIM(A.acode),1,2)=trim(c.type1) and d.id='A' and c.id='Z' and trim(a.acode)=trim(b.acode) group by a.branchcd,trim(a.acode),b.aname,c.name,d.name,b.bssch,c.type1,b.aname having (sum(a.opening)!= 0 or sum(a.cdr)!= 0 or sum(a.ccr)!= 0 or sum(a.clos)!= 0)";
                    fgen.drillQuery(2, SQuery2, frm_qstr, "5#6#7#8#", "3#4#", "250#200#");

                    SQuery3 = "SELECT * FROM (SELECT A.BRANCHCD||A.tYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,trim(A.ACODE) AS GSTR,b.ANAME AS ACCOUNT,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DATED,(A.DRAMT) AS DEBIT,(a.CRAMT) AS CREDITS,A.TYPE,A.VCHNUM,A.NARATION,A.BRANCHCD,A.INVNO,to_char(A.INVDATE,'dd/mm/yyyy') as invdate,A.GRNO,A.REFNUM,A.MRNNUM,A.MRNDATE,A.ENT_BY,A.CCENT,A.BANK_DATE,A.TAX,A.STAX,A.ST_ENTFORM,A.BRANCHCD PL_CODE FROM VOUCHER A,FAMST B WHERE TRIM(a.RCODE)=TRIM(b.ACODE) AND A." + branch_Cd + " AND A.TYPE LIKE '%' AND A.VCHDATE " + xprd2 + " ORDER BY A.VCHNUM) WHERE GSTR='FSTR'";
                    fgen.drillQuery(3, SQuery3, frm_qstr);
                    //vipin
                    fgen.Fn_DrillReport("Balance Sheet as on " + todt + "", frm_qstr);
                    break;
            }
        }
    }
}
