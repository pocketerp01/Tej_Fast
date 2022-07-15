using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text;

//15109 ID ON VAPL LOCAL
public partial class mkt_ppc1 : System.Web.UI.Page
{
    string SQuery, co_cd, uname, frm_mbr, vardate, fromdt, todt, DateRange, year, ulvl;
    string mthplan, pageid; int ind;
    double totprod, totplan, totrej, drej, doth, totamt, totv1, totv2, totv3, totv4, totv5, totv6, totv7;
    string xprdrange, segment, rej, pachv, rachv, columname, rval1, rval2, rval3, rval4, rval5, rval6, vtype, ptype, vrejection, vname;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10;
    DataTable dt, dt3, dt4, dt5;
    string frm_frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", PrdRange, cmd_query;
    // fgen_fun fgen = new fgen_fun();
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            btnnew.Focus();
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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    //todt = "10/05/2017";
                    if (frm_formID != null)
                    { hfhcid.Value = frm_formID; }

                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    pageid = frm_formID;
                }
                else Response.Redirect("~/login.aspx");
            }
            fill_header();

            if (!Page.IsPostBack)
            {
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_frm_mbr + "'))", "add_tx");
                //  doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                btnfull.Visible = false; btnstop.Disabled = true;
                enablectrl(); btnnew.Focus(); timer1.Enabled = false;
            }
            //  setColHeadings();
            //set_Val();
        }
    }

    void getColHeading()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            dtCol = fgen.getdata(frm_qstr, frm_cocd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_SYS_COM_QRY") + " WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        }
        ViewState["d" + frm_qstr + frm_formID] = dtCol;
    }
    public void DisplayData()
    {
        dt3 = new DataTable();
        pageid = hfhcid.Value; //old
        pageid = frm_formID;
        if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
            GetData();
        if (pageid == "F35509")
        {
            dt = new DataTable();
            string[] menuid = { "F35509", "40104", "40105" };
            int ictr = 0;
            foreach (string mid in menuid)
            {
                pageid = mid;
                GetData();

                if (dt3.Rows.Count <= 0)
                {
                    fgen.msg("-", "AMSG", "No Record Exist"); return;
                }

                if (ictr == 0) dt = dt3.Clone();

                if (pageid == "F35509")
                {
                    vname = "EXTRUSION";
                }

                if (pageid == "40104")
                    vname = "CUTTING:";
                if (pageid == "40105")
                    vname = "SCREENING:";
                lblheader.Text = vname;

                dt.Rows.Add("00", vname, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                foreach (DataRow dr in dt3.Rows)
                {
                    DataRow crow = dt.NewRow();
                    crow.ItemArray = dr.ItemArray;
                    dt.Rows.Add(crow);
                }
                ictr++;
            }
            dt.Rows.Add("00", "Overall % Rej.(Ext.+Cutting+Screening)", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
            rachv = string.Empty;
            for (int i = 1; i < 33; i++)
            {
                if (i == 32) columname = "total";
                else columname = "d" + i;

                if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                {
                    drej = 0;
                    foreach (DataRow drw in dt.Rows)
                    {
                        if (drw[1].ToString().Trim().Contains("Rejection %"))
                            drej += Convert.ToDouble(drw[columname].ToString().Replace("%", ""));
                    }
                    rachv = Convert.ToString(drej);
                }

                if (rachv == "Infinity" || rachv == "NaN") rachv = "0";
                if (Convert.ToDouble(rachv) > 0) rachv = rachv.Remove(rachv.Length - 1);
                dt.Rows[dt.Rows.Count - 1][columname] = rachv + "%";
            }
            dt3 = dt;
        }
        if (dt3.Rows.Count == 0) fgen.msg("-", "AMSG", "No record exist!");
        else
        {
            for (int o = 1; o <= 31; o++)
            {

                dt3.Columns["d" + o + ""].ColumnName = o.ToString();
            }
            DataRow drr = dt3.NewRow();
            drr["segment"] = "Rejection Reasons as Under";
            dt3.Rows.InsertAt(drr, 5);
            DataRow drr1 = dt3.NewRow();
            drr1["segment"] = "Critical Ratio";
            dt3.Rows.InsertAt(drr1, 12);
            DataRow drr2 = dt3.NewRow();
            drr2["segment"] = "Rejection Reasons as Under";
            dt3.Rows.InsertAt(drr2, 20);
            DataRow drr3 = dt3.NewRow();
            drr3["segment"] = "Critical Ratio";
            dt3.Rows.InsertAt(drr3, 27);
            DataRow drr4 = dt3.NewRow();
            drr4["segment"] = "Rejection Reasons as Under";
            dt3.Rows.InsertAt(drr4, 35);
            DataRow drr5 = dt3.NewRow();
            drr5["segment"] = "Critical Ratio";
            dt3.Rows.InsertAt(drr5, 42);
            sg1.DataSource = dt3;
            sg1.DataBind();
        }
    }
    public void GetData()
    {
        //fromdt = Request.Cookies["Value1"].Value.ToString().Trim();
        //todt = Request.Cookies["Value2"].Value.ToString().Trim();
        DateRange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
        vardate = todt;

        mthplan = string.Empty;

        dt5 = new DataTable();
        dt4 = new DataTable();
        dt3 = new DataTable();

        if (pageid == "F35506" || pageid == "F35509")
        {
            vtype = "62";
            ptype = "15";
        }
        if (pageid == "F35507" || pageid == "40104")
        {
            vtype = "63";
            ptype = "16";
        }
        if (pageid == "F35508" || pageid == "40105")
        {
            vtype = "64";
            ptype = "17";
        }
        vrejection = "replace(TRIM(col7),'-','0')||'@'||replace(TRIM(col14),'-','0')||'@'||replace(TRIM(col9),'-','0')||'@'||replace(TRIM(col10),'-','0')||'@'||replace(TRIM(col16),'-','0')||'@'||replace(TRIM(col17),'-','0') as rejection";

        vardate = vardate.Replace("/", "");
        string cdate = vardate.Substring(4, 4) + vardate.Substring(2, 2);
        dt5 = fgen.getdata(frm_qstr, frm_cocd, "select sum(total),sum(day1) as d1,sum(day2) as d2,sum(day3) as d3,sum(day4) as d4,sum(day5) as d5,sum(day6) as d6,sum(day7) as d7,sum(day8) as d8,sum(day9) as d9,sum(day10) as d10,sum(day11) as d11,sum(day12) as d12,sum(day13) as d13,sum(day14) as d14,sum(day15) as d15,sum(day16) as d16,sum(day17) as d17,sum(day18) as d18,sum(day19) as d19,sum(day20) as d20,sum(day21) as d21,sum(day22) as d22,sum(day23) as d23,sum(day24) as d24,sum(day25) as d25,sum(day26) as d26,sum(day27) as d27,sum(day28) as d28,sum(day29) as d29,sum(day30) as d30,sum(day31) as d31 from pschedule WHERE branchcd='" + frm_frm_mbr + "' and TYPE='" + ptype + "' AND to_char(vchdate,'yyyymm')='" + cdate + "' ");
        if (dt5.Rows.Count > 0) mthplan = dt5.Rows[0][0].ToString().Trim();
        else mthplan = "0";

        if (dt5.Rows[0][0].ToString().Trim() == "") mthplan = "0";

        if (vtype == "62") mq1 = mthplan.ToString();
        if (vtype == "63") mq2 = mthplan.ToString();
        if (vtype == "64") mq3 = mthplan.ToString();


        totprod = 0; totplan = 0; totrej = 0; drej = 0; doth = 0; totamt = 0; totv1 = 0; totv2 = 0; totv3 = 0; totv4 = 0; totv5 = 0; totv6 = 0; totv7 = 0;
        segment = string.Empty; rej = string.Empty; pachv = string.Empty; rachv = string.Empty; columname = string.Empty;
        rval1 = string.Empty; rval2 = string.Empty; rval3 = string.Empty; rval4 = string.Empty; rval5 = string.Empty; rval6 = string.Empty;
        int day = 0;

        string[] rejec;

        if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
            SQuery = "SELECT  segment,ITEM,ICODE,PART_NO, PLAN,vchnum,vchdate FROM (SELECT '01' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,'0' AS plan,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '02' AS segment,'-' as ITEM,ICODE AS ICODE,'-' AS PART_NO, replace(TRIM(col5),'-','0') as production,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '03' AS segment,'-' as ITEM,ICODE AS ICODE,'-' AS PART_NO," + vrejection + ",vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE a WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' ) ORDER BY segment||vchdate";
        if (pageid == "F35509" || pageid == "40104" || pageid == "40105" || pageid == "97022")
            SQuery = "SELECT  segment,ITEM,ICODE,PART_NO, PLAN,vchnum,vchdate FROM (SELECT '01' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,'0' AS plan,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '02' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col4),'-','0') as INPUT,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='25' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '03' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col5),'-','0') as OUTPUT,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '04' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO," + vrejection + ",vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '05' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col7),'-','0') as COL1,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '06' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col14),'-','0') as COL2,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '07' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col9),'-','0') as COL3,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '08' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col10),'-','0') as COL4,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '09' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col16),'-','0') as COL5,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' UNION ALL SELECT '10' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col17),'-','0') as COL6,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + cdate + "' ) ORDER BY segment||vchdate";

        dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        if (dt4.Rows.Count > 0)
        {
            if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
            {
                dt4.Rows.Add("04", "-", "-", "-", "0", "-", "-");
                dt4.Rows.Add("05", "-", "-", "-", "0", "-", "-");
            }
            if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
            {
                dt4.Rows.Add("11", "-", "-", "-", "0", "-", "-");
                dt4.Rows.Add("12", "-", "-", "-", "0", "-", "-");
            }

            string[] columns = { "fstr", "segcode", "Segment", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11", "D12", "D13", "D14", "D15", "D16", "D17", "D18", "D19", "D20", "D21", "D22", "D23", "D24", "D25", "D26", "D27", "D28", "D29", "D30", "D31", "Total" };
            foreach (string colm in columns)
            {
                dt3.Columns.Add(colm, typeof(String));
            }
            int icount = 0;
            int jcount = 0;
            foreach (DataRow dr in dt4.Rows)
            {
                DataRow[] row = dt3.Select("fstr  ='" + dr[0].ToString().Trim() + dr[6].ToString().Trim() + "'");
                DataRow[] rows = dt3.Select("segcode  ='" + dr[0].ToString().Trim() + "'");

                if (row.Length > 0 || rows.Length > 0)
                {
                    rej = string.Empty; rval1 = string.Empty; rval2 = string.Empty; rval3 = string.Empty; rval4 = string.Empty; rval5 = string.Empty; rval6 = string.Empty;

                    if (row.Length <= 0) jcount = 0;

                    if (row.Length <= 0 && jcount == 0)
                    {
                        totprod = 0; totplan = 0; totrej = 0; drej = 0; day = 0; totamt = 0; doth = 0; totv1 = 0; totv2 = 0; totv3 = 0; totv4 = 0; totv5 = 0; totv6 = 0; totv7 = 0;
                        dt3.Rows[icount - 1]["fstr"] = dr[0].ToString().Trim() + dr[6].ToString().Trim();
                        jcount++;
                    }

                    rej = dr[4].ToString().Trim();
                    if (rej.Contains("@"))
                    {
                        drej = 0;
                        rejec = rej.Split('@');
                        rval1 = rejec[0].ToString().Trim();
                        rval2 = rejec[1].ToString().Trim();
                        rval3 = rejec[2].ToString().Trim();
                        rval4 = rejec[3].ToString().Trim();
                        rval5 = rejec[4].ToString().Trim();
                        rval6 = rejec[5].ToString().Trim();
                        drej = Convert.ToDouble(rval1) + Convert.ToDouble(rval2) + Convert.ToDouble(rval3) + Convert.ToDouble(rval4) + Convert.ToDouble(rval5) + Convert.ToDouble(rval6);
                        rej = drej.ToString();
                    }
                    if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                    {

                        switch (dr[0].ToString().Trim())
                        {
                            case "01":
                                totplan = totplan + Convert.ToDouble(rej);
                                break;
                            case "02":
                                totprod = totprod + Convert.ToDouble(rej);
                                break;
                            case "03":
                                totrej = totrej + Convert.ToDouble(rej);
                                break;
                        }
                    }
                    if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                    {
                        switch (dr[0].ToString().Trim())
                        {
                            case "01":
                                totplan = totplan + Convert.ToDouble(rej);
                                break;
                            case "02":
                                totv1 = totv1 + Convert.ToDouble(rej);
                                break;
                            case "03":
                                totprod = totprod + Convert.ToDouble(rej);
                                break;
                            case "04":
                                totrej = totrej + Convert.ToDouble(rej);
                                break;
                            case "05":
                                totv2 = totv2 + Convert.ToDouble(rej);
                                break;
                            case "06":
                                totv3 = totv3 + Convert.ToDouble(rej);
                                break;
                            case "07":
                                totv4 = totv4 + Convert.ToDouble(rej);
                                break;
                            case "08":
                                totv5 = totv5 + Convert.ToDouble(rej);
                                break;
                            case "09":
                                totv6 = totv6 + Convert.ToDouble(rej);
                                break;
                            case "10":
                                totv7 = totv7 + Convert.ToDouble(rej);
                                break;
                        }
                    }
                    for (int i = 1; i < 32; i++)
                    {
                        if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                        {
                            if (dr[0].ToString().Trim() == "04" || dr[0].ToString().Trim() == "05") day = 0;
                            else
                                day = Convert.ToInt32(dr[6].ToString().Trim().Substring(0, 2));
                        }
                        if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                        {
                            if (dr[0].ToString().Trim() == "11" || dr[0].ToString().Trim() == "12") day = 0;
                            else
                                day = Convert.ToInt32(dr[6].ToString().Trim().Substring(0, 2));
                        }
                        if (day == i)
                        {
                            if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                            {
                                switch (dr[0].ToString().Trim())
                                {
                                    case "01":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totplan;
                                        else
                                            row[0]["d" + i] = totplan;
                                        break;
                                    case "02":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totprod;
                                        else
                                            row[0]["d" + i] = totprod;
                                        break;
                                    case "03":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totrej;
                                        else
                                            row[0]["d" + i] = totrej;
                                        break;
                                }
                            }
                            if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                            {
                                switch (dr[0].ToString().Trim())
                                {
                                    case "01":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totplan;
                                        else
                                            row[0]["d" + i] = totplan;
                                        break;
                                    case "02":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv1;
                                        else
                                            row[0]["d" + i] = totv1;
                                        break;
                                    case "03":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totprod;
                                        else
                                            row[0]["d" + i] = totprod;
                                        break;
                                    case "04":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totrej;
                                        else
                                            row[0]["d" + i] = totrej;
                                        break;
                                    case "05":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv2;
                                        else
                                            row[0]["d" + i] = totv2;
                                        break;
                                    case "06":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv3;
                                        else
                                            row[0]["d" + i] = totv3;
                                        break;
                                    case "07":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv4;
                                        else
                                            row[0]["d" + i] = totv4;
                                        break;
                                    case "08":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv5;
                                        else
                                            row[0]["d" + i] = totv5;
                                        break;
                                    case "09":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv6;
                                        else
                                            row[0]["d" + i] = totv6;
                                        break;
                                    case "10":
                                        if (row.Length <= 0)
                                            dt3.Rows[icount - 1]["d" + i] = totv7;
                                        else
                                            row[0]["d" + i] = totv7;
                                        break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    DataRow drow = dt3.NewRow();
                    drow["fstr"] = dr[0].ToString().Trim() + dr[6].ToString().Trim();
                    drow["segcode"] = dr[0].ToString().Trim();

                    rej = string.Empty; rval1 = string.Empty; rval2 = string.Empty; rval3 = string.Empty; rval4 = string.Empty; rval5 = string.Empty; rval6 = string.Empty;
                    totprod = 0; totplan = 0; totrej = 0; drej = 0; day = 0; doth = 0; totamt = 0; totv1 = 0; totv2 = 0; totv3 = 0; totv4 = 0; totv5 = 0; totv6 = 0; totv7 = 0;

                    rej = dr[4].ToString().Trim();

                    if (rej.Contains("@"))
                    {
                        drej = 0;
                        rejec = rej.Split('@');
                        rval1 = rejec[0].ToString().Trim();
                        rval2 = rejec[1].ToString().Trim();
                        rval3 = rejec[2].ToString().Trim();
                        rval4 = rejec[3].ToString().Trim();
                        rval5 = rejec[4].ToString().Trim();
                        rval6 = rejec[5].ToString().Trim();
                        drej = Convert.ToDouble(rval1) + Convert.ToDouble(rval2) + Convert.ToDouble(rval3) + Convert.ToDouble(rval4) + Convert.ToDouble(rval5) + Convert.ToDouble(rval6);
                        rej = drej.ToString();
                    }

                    if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                    {
                        switch (dr[0].ToString().Trim())
                        {
                            case "01":
                                segment = "Daily Plan";
                                totplan = totplan + Convert.ToDouble(rej);
                                break;
                            case "02":
                                segment = "Daily Production";
                                totprod = totprod + Convert.ToDouble(rej);
                                break;
                            case "03":
                                segment = "Daily Rejection";
                                totrej = totrej + Convert.ToDouble(rej);
                                break;
                            case "04":
                                segment = "Production Achievement";
                                break;
                            case "05":
                                segment = "Rejection %";
                                break;
                        }
                    }
                    if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                    {
                        switch (dr[0].ToString().Trim())
                        {
                            case "01":
                                segment = "Plan(kg)";
                                totplan = totplan + Convert.ToDouble(rej);
                                break;
                            case "02":
                                segment = "Input(kg)";
                                totv1 = totv1 + Convert.ToDouble(rej);
                                break;
                            case "03":
                                segment = "OK output(Kg)";
                                totprod = totprod + Convert.ToDouble(rej);
                                break;
                            case "04":
                                segment = "Daily Rejection (Kgs) ";
                                totrej = totrej + Convert.ToDouble(rej);
                                break;
                            case "05":
                                if (pageid == "F35509")
                                    segment = "Butt.Wt(Kg)";
                                if (pageid == "40104")
                                    segment = "Set Up Scrap(Kg)";
                                if (pageid == "40105")
                                    segment = "Dent(Kg)";
                                totv2 = totv2 + Convert.ToDouble(rej);
                                break;
                            case "06":
                                if (pageid == "F35509")
                                    segment = "Tube with Zinc(Kg)";
                                if (pageid == "40104")
                                    segment = "ECT Mark(Kg)";
                                if (pageid == "40105")
                                    segment = "Line/Scratch(Kg)";
                                totv3 = totv3 + Convert.ToDouble(rej);
                                break;
                            case "07":
                                if (pageid == "F35509")
                                    segment = "Tube Without Zinc(Kg)";
                                if (pageid == "40104")
                                    segment = "Handling(Kg)";
                                if (pageid == "40105")
                                    segment = "Tube Rust(Kg)";
                                totv4 = totv4 + Convert.ToDouble(rej);
                                break;
                            case "08":
                                if (pageid == "F35509")
                                    segment = "QA.Sample(Kg)";
                                if (pageid == "40104")
                                    segment = "Winding Problem(Kg)";
                                if (pageid == "40105")
                                    segment = "Tube Pin Hole(Kg)";
                                totv5 = totv5 + Convert.ToDouble(rej);
                                break;
                            case "09":
                                if (pageid == "F35509")
                                    segment = "Billet Scrap(Kg)";
                                if (pageid == "40104")
                                    segment = "Tube Pin Hole(Kg)";
                                if (pageid == "40105")
                                    segment = "Radius Damage(Kg)";
                                totv6 = totv6 + Convert.ToDouble(rej);
                                break;
                            case "10":
                                if (pageid == "F35509")
                                    segment = "Other Scrap(Kg)";
                                if (pageid == "40104" || pageid == "40105")
                                    segment = "Other Tube Scrap(Kg)";
                                totv7 = totv7 + Convert.ToDouble(rej);
                                break;
                            case "11":
                                segment = "Production Achievement";
                                break;
                            case "12":
                                segment = "Rejection %";
                                break;

                        }
                    }

                    drow["Segment"] = segment;

                    for (int k = 1; k < 32; k++)
                    {
                        drow["d" + k] = "0";
                    }

                    for (int i = 1; i < 32; i++)
                    {
                        if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                        {
                            if (dr[0].ToString().Trim() == "04" || dr[0].ToString().Trim() == "05") day = 0;
                            else
                                day = Convert.ToInt32(dr[6].ToString().Trim().Substring(0, 2));
                        }
                        if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                        {
                            if (dr[0].ToString().Trim() == "11" || dr[0].ToString().Trim() == "12") day = 0;
                            else
                                day = Convert.ToInt32(dr[6].ToString().Trim().Substring(0, 2));
                        }


                        if (day == i)
                        {
                            if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                            {
                                switch (dr[0].ToString().Trim())
                                {
                                    case "01":
                                        drow["d" + i] = totplan;
                                        break;
                                    case "02":
                                        drow["d" + i] = totprod;
                                        break;
                                    case "03":
                                        drow["d" + i] = totrej;
                                        break;
                                }
                            }
                            if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                            {
                                switch (dr[0].ToString().Trim())
                                {
                                    case "01":
                                        drow["d" + i] = totplan;
                                        break;
                                    case "02":
                                        drow["d" + i] = totv1;
                                        break;
                                    case "03":
                                        drow["d" + i] = totprod;
                                        break;
                                    case "04":
                                        drow["d" + i] = totrej;
                                        break;
                                    case "05":
                                        drow["d" + i] = totv2;
                                        break;
                                    case "06":
                                        drow["d" + i] = totv3;
                                        break;
                                    case "07":
                                        drow["d" + i] = totv4;
                                        break;
                                    case "08":
                                        drow["d" + i] = totv5;
                                        break;
                                    case "09":
                                        drow["d" + i] = totv6;
                                        break;
                                    case "10":
                                        drow["d" + i] = totv7;
                                        break;

                                }
                            }
                        }
                    }
                    drow["total"] = totamt;
                    dt3.Rows.Add(drow);
                    icount++;
                }
            }
            for (int m = 1; m < 32; m++)
            {
                if (dt5.Rows[0]["d" + m].ToString().Trim() == "") dt3.Rows[0]["d" + m] = "0";
                else
                    dt3.Rows[0]["d" + m] = dt5.Rows[0]["d" + m];
            }
            foreach (DataRow row1 in dt3.Rows)
            {
                totamt = 0;
                for (int i = 1; i < 32; i++)
                {
                    totamt += System.Math.Round(Convert.ToDouble(row1["d" + i]), 2);
                }
                row1["total"] = totamt;
            }
            for (int i = 1; i < 33; i++)
            {
                if (i == 32) columname = "total";
                else columname = "d" + i;

                if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                {
                    pachv = Convert.ToString(System.Math.Round((Convert.ToDouble(dt3.Rows[1][columname]) / Convert.ToDouble(dt3.Rows[0][columname]) * 100), 3));
                    rachv = Convert.ToString(System.Math.Round((Convert.ToDouble(dt3.Rows[2][columname]) / Convert.ToDouble(dt3.Rows[1][columname]) * 100), 3));
                }

                if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                {
                    pachv = Convert.ToString(System.Math.Round((Convert.ToDouble(dt3.Rows[2][columname]) / Convert.ToDouble(dt3.Rows[0][columname]) * 100), 3));
                    rachv = Convert.ToString(System.Math.Round((Convert.ToDouble(dt3.Rows[3][columname]) / Convert.ToDouble(dt3.Rows[1][columname]) * 100), 3));
                }

                if (pachv == "Infinity" || pachv == "NaN") pachv = "0";
                if (rachv == "Infinity" || rachv == "NaN") rachv = "0";

                if (Convert.ToDouble(pachv) > 0) pachv = pachv.Remove(pachv.Length - 1);
                if (Convert.ToDouble(rachv) > 0) rachv = rachv.Remove(rachv.Length - 1);

                if (pageid == "F35506" || pageid == "F35507" || pageid == "F35508")
                {
                    dt3.Rows[3][columname] = pachv + "%";
                    dt3.Rows[4][columname] = rachv + "%";
                }
                if (pageid == "F35509" || pageid == "40104" || pageid == "40105")
                {
                    dt3.Rows[10][columname] = pachv + "%";
                    dt3.Rows[11][columname] = rachv + "%";
                }
            }
            dt3.Columns.RemoveAt(0);
        }
    }
    // neW tWO
    public void enablectrl()
    {
        btnnew.Disabled = false; btnstop.Disabled = true;
        btnext.Text = " Exit "; btnext.Enabled = true;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnstop.Disabled = false;
        btnext.Text = "Cancel"; btnext.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    public void fill_header()
    {
        switch (hfhcid.Value.Trim())
        {
            case "F35506":
                lblheader.Text = "Extrusion Report";
                break;
            case "F35507":
                lblheader.Text = "Cutting Report";
                break;
            case "F35508":
                lblheader.Text = "Packing Report";
                break;
            case "F35509":
                lblheader.Text = "Production Performance Report";
                break;
        }

        if (sg1.Rows.Count > 0)
        {
            sg1.Rows[0].Cells[1].Width = 200;

            for (int z = 0; z <= sg1.Rows.Count - 1; z++)
            {
                for (int i = 2; i <= sg1.Rows[z].Cells.Count - 1; i++)
                {
                    sg1.Rows[z].Cells[i].ForeColor = System.Drawing.Color.DarkBlue;
                    sg1.Rows[z].Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    sg1.Rows[z].Cells[i].Width = 80;
                }
            }
        }
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        //  fgen.open_prddmp1("-");
        DisplayData();
        disablectrl();
        if (sg1.Rows.Count > 0)
        {
            timer1.Enabled = true;
            btnfull.Disabled = true;
            btnstop.Disabled = false;
            btnstop.InnerText = "Stop";
            ind = sg1.Columns.Count;
        }
    }
    protected void btnfull_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ") Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        else
        {
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            clearctrl();
            enablectrl();
            sg1.DataSource = null;
            sg1.DataBind();
            timer1.Enabled = false;
            lbl1.Text = "";
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        fromdt = Request.Cookies["Value1"].Value.ToString().Trim();
        todt = Request.Cookies["Value2"].Value.ToString().Trim();
        DateRange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
        //fill_grid();
        //if (sg1.Rows.Count > 0)
        //{
        //    timer1.Enabled = true;
        //    btnfull.Disabled = false;
        //    btnstop.Text = "Start";
        //    ind = sg1.Columns.Count;
        //}

        //else fgen.msg("-", "AMSG", "No Data exist");
        DisplayData();
        disablectrl();
        if (sg1.Rows.Count > 0)
        {
            timer1.Enabled = true;
            btnfull.Disabled = true;
            btnstop.Disabled = false;
            btnstop.InnerText = "Stop";
            ind = sg1.Columns.Count;
        }
        else fgen.msg("-", "AMSG", "No Data exist");

    }
    protected void timer1_Tick(object sender, EventArgs e)
    {
        tick(false);
        fill_header();
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[Convert.ToInt32(vardate.ToString().Substring(0, 2)) + 1].BackColor = System.Drawing.Color.Yellow;
            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Font.Size = 12;

            fill_header();
        }
    }
    protected void btnstop_Click(object sender, EventArgs e)
    {
        Timer t = timer1;
        if (btnstop.InnerText == "Stop")
        {
            btnstop.InnerText = "Start";
            t.Interval = 999999999;
            //timer1.Enabled = false;
            tick(true);
        }
        else
        {

            t.Interval = 6000;
            btnstop.InnerText = "Stop";
            //timer1.Enabled = true;
            tick(true);
        }
    }

    public void Chart_5data(string chartname, string xunit, string tempcat, string chart_type, string chartsub, string[] dataArray, string[] NameArrary)
    {



        StringBuilder sb = new StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append(@"$(function () {");
        sb.Append(@"$('#container').highcharts({");
        sb.Append(@"        chart: {");
        sb.Append(@"            type: '" + chart_type + "'");
        sb.Append(@"       , margin: [0, 0, 100, 0]  },");
        sb.Append(@"        title: {");
        sb.Append(@"            text: '" + chartname + "'");
        sb.Append(@"        },");
        sb.Append(@"        subtitle: {");
        sb.Append(@"            text: '" + chartsub + "'");
        sb.Append(@"        },");
        sb.Append(@"        xAxis: {");
        sb.Append(@"            categories: [" + tempcat + "],");
        sb.Append(@"            title: {");
        sb.Append(@"                text: null");
        sb.Append(@"            }");
        sb.Append(@"        },");
        sb.Append(@"        yAxis: {");
        sb.Append(@"            min: 0,");
        sb.Append(@"            title: {");
        sb.Append(@"                text: 'Figure in (" + xunit + ")',");
        sb.Append(@"                align: 'high'");
        sb.Append(@"            },");
        sb.Append(@"            labels: {");
        sb.Append(@"                overflow: 'justify'");
        sb.Append(@"            }");
        sb.Append(@"        },");
        sb.Append(@"        tooltip: {");
        sb.Append(@"            valueSuffix: ' " + xunit + "'");
        sb.Append(@"        },");
        sb.Append(@"        plotOptions: {");
        sb.Append(@"            '" + chart_type + "': {");
        sb.Append(@"                dataLabels: {");
        sb.Append(@"                    color: '#000000', enabled: true");

        //////////////////////////////////////////////////////
        sb.Append(@"  , formatter : function() {");
        sb.Append(@"     return this.y + '" + xunit + "';");
        sb.Append(@"                     }");


        ////////////////////////////////////////////////////////
        //////////////////////////////


        sb.Append(@"                }");
        sb.Append(@"            }");
        sb.Append(@"        },");
        sb.Append(@"        legend: {");
        sb.Append(@"            ");
        sb.Append(@"            align: 'center',");
        sb.Append(@"            verticalAlign: 'bottom',");
        sb.Append(@"            x: 0,");
        sb.Append(@"            y: 0,");
        sb.Append(@"            floating: true,");
        sb.Append(@"            borderWidth: 1,");
        sb.Append(@"            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor || '#FFFFFF'),");
        sb.Append(@"            shadow: true");
        sb.Append(@"        },");
        sb.Append(@"        credits: {");
        sb.Append(@"            enabled: false");
        sb.Append(@"        },");
        sb.Append(@"        series: [");
        for (int i = 0; i < NameArrary.Length; i++)
        {
            if (NameArrary[i].ToString().Trim() != null)
            {

                sb.Append(@"            { name: '" + NameArrary[i].ToString().Trim() + "',");
                sb.Append(@"            data: [" + dataArray[i].ToString().Trim() + "]");
                sb.Append(@"        }");

                if (i != NameArrary.Length - 1)
                {
                    sb.Append(@" ,");
                }

            }
        }
        sb.Append(@"    ]");
        sb.Append(@"    });");
        sb.Append(@"});");
        sb.Append(@"</script>");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);
    }

    public void Chart_Pareto(string chartname, string xunit, string tempcat, string chart_type, string chartsub, string[] dataArray, string[] NameArrary)
    {
        StringBuilder sb = new StringBuilder();

        ///////////////////////////////////////
        sb.Append(@"<script type='text/javascript'>");

        sb.Append(@" var chart;");
        sb.Append(@"$(document).ready(function () {");
        sb.Append(@" chart = new Highcharts.Chart({");
        sb.Append(@"chart: {");
        sb.Append(@"     renderTo: 'container'");
        sb.Append(@"   },");
        sb.Append(@"credits: {");
        sb.Append(@"enabled: false");
        sb.Append(@"},");
        sb.Append(@"legend: {");
        sb.Append(@"layout: 'horizontal',");
        sb.Append(@"verticalAlign: 'bottom'");
        sb.Append(@"},");
        sb.Append(@"title: {");
        sb.Append(@"text: '" + chartname + "'");
        sb.Append(@"},");
        sb.Append(@"tooltip: {");
        sb.Append(@"formatter: function () {");
        sb.Append(@"if (this.series.name == 'Accumulated') {");
        sb.Append(@"return this.y + '%';");
        sb.Append(@"}");
        sb.Append(@"return this.x + '<br/>' + '<b> ' + this.y.toString().replace('.', ',') + ' </b>';");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"xAxis: {");
        sb.Append(@"categories: [" + tempcat + "]");
        sb.Append(@"},");
        sb.Append(@"yAxis: [{");
        sb.Append(@"title: {");
        sb.Append(@"text: ''");
        sb.Append(@"}");
        sb.Append(@"}, {");
        sb.Append(@"labels: {");
        sb.Append(@"formatter: function () {");
        sb.Append(@"return this.value + '%';");
        sb.Append(@"}");
        sb.Append(@"},");
        sb.Append(@"max: 100,");
        sb.Append(@"min: 0,");
        sb.Append(@"opposite: true,");
        sb.Append(@"plotLines: [{");
        sb.Append(@"color: '#89A54E',");
        sb.Append(@"dashStyle: 'shortdash',");
        sb.Append(@"value: 80,");
        sb.Append(@"width: 3,");
        sb.Append(@"zIndex: 10");
        sb.Append(@"}],");
        sb.Append(@"title: {");
        sb.Append(@"text: ''");
        sb.Append(@"}");
        sb.Append(@"}],");

        sb.Append(@"series: [{");
        sb.Append(@"data: [" + dataArray[0].ToString() + "],");
        sb.Append(@"name: 'Options',");
        sb.Append(@"type: 'column'");
        sb.Append(@"}, {");
        sb.Append(@"data: [" + NameArrary[0].ToString() + "],");
        sb.Append(@"name: 'Accumulated',");
        sb.Append(@"type: 'spline',");
        sb.Append(@"yAxis: 1,");
        sb.Append(@"id: 'accumulated'");
        sb.Append(@"}]");
        sb.Append(@"},function(chart){");

        sb.Append(@"var x = 0.8 * chart.plotWidth;");

        sb.Append(@"chart.renderer.path([");
        sb.Append(@"'M',");
        sb.Append(@"x, chart.plotTop,");
        sb.Append(@"'L',");
        sb.Append(@"x, chart.plotTop + chart.plotHeight");
        sb.Append(@"]).attr({");
        sb.Append(@"'stroke-width': 2,");
        sb.Append(@"stroke: 'red',");
        sb.Append(@"id: 'vert',");
        sb.Append(@"'stroke-dasharray':'5,5',");
        sb.Append(@"zIndex: 2000");
        sb.Append(@"}).add();");

        sb.Append(@"});");
        sb.Append(@"});");

        sb.Append(@"</script>");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);
    }

    //public void pareto_chart()
    //{

    //    StringBuilder sb = new StringBuilder();

    //    sb.Append(@"<script type='text/javascript'>");

    //    sb.Append(@" var chart;");
    //    sb.Append(@"$(document).ready(function () {");
    //    sb.Append(@" chart = new Highcharts.Chart({");
    //    sb.Append(@"chart: {");
    //    sb.Append(@"     renderTo: 'container'");
    //    sb.Append(@"   },");
    //    sb.Append(@"credits: {");
    //    sb.Append(@"enabled: false");
    //    sb.Append(@"},");
    //    sb.Append(@"legend: {");
    //    sb.Append(@"layout: 'horizontal',");
    //    sb.Append(@"verticalAlign: 'bottom'");
    //    sb.Append(@"},");
    //    sb.Append(@"title: {");
    //    sb.Append(@"text: ''");
    //    sb.Append(@"},");
    //    sb.Append(@"tooltip: {");
    //    sb.Append(@"formatter: function () {");
    //    sb.Append(@"if (this.series.name == 'Accumulated') {");
    //    sb.Append(@"return this.y + '%';");
    //    sb.Append(@"}");
    //    sb.Append(@"return this.x + '<br/>' + '<b> ' + this.y.toString().replace('.', ',') + ' </b>';");
    //    sb.Append(@"}");
    //    sb.Append(@"},");
    //    sb.Append(@"xAxis: {");
    //    sb.Append(@"categories: ['E', 'D', 'B', 'A']");
    //    sb.Append(@"},");
    //    sb.Append(@"yAxis: [{");
    //    sb.Append(@"title: {");
    //    sb.Append(@"text: ''");
    //    sb.Append(@"}");
    //    sb.Append(@"}, {");
    //    sb.Append(@"labels: {");
    //    sb.Append(@"formatter: function () {");
    //    sb.Append(@"return this.value + '%';");
    //    sb.Append(@"}");
    //    sb.Append(@"},");
    //    sb.Append(@"max: 100,");
    //    sb.Append(@"min: 0,");
    //    sb.Append(@"opposite: true,");
    //    sb.Append(@"plotLines: [{");
    //    sb.Append(@"color: '#89A54E',");
    //    sb.Append(@"dashStyle: 'shortdash',");
    //    sb.Append(@"value: 80,");
    //    sb.Append(@"width: 3,");
    //    sb.Append(@"zIndex: 10");
    //    sb.Append(@"}],");
    //    sb.Append(@"title: {");
    //    sb.Append(@"text: ''");
    //    sb.Append(@"}");
    //    sb.Append(@"}],");
    //    sb.Append(@"series: [{");
    //    sb.Append(@"data: [5.6000000000, 5.1000000000, 2.8000000000, 1.3000000000],");
    //    sb.Append(@"name: 'Options',");
    //    sb.Append(@"type: 'column'");
    //    sb.Append(@"}, {");
    //    sb.Append(@"data: [38, 72, 91, 100],");
    //    sb.Append(@"name: 'Accumulated',");
    //    sb.Append(@"type: 'spline',");
    //    sb.Append(@"yAxis: 1,");
    //    sb.Append(@"id: 'accumulated'");
    //    sb.Append(@"}]");
    //    sb.Append(@"},function(chart){");


    //    sb.Append(@"var x = 0.8 * chart.plotWidth;");

    //    sb.Append(@"chart.renderer.path([");
    //    sb.Append(@"'M',");
    //    sb.Append(@"x, chart.plotTop,");
    //    sb.Append(@"'L',");
    //    sb.Append(@"x, chart.plotTop + chart.plotHeight");
    //    sb.Append(@"]).attr({");
    //    sb.Append(@"'stroke-width': 2,");
    //    sb.Append(@"stroke: 'red',");
    //    sb.Append(@"id: 'vert',");
    //    sb.Append(@"'stroke-dasharray':'5,5',");
    //    sb.Append(@"zIndex: 2000");
    //    sb.Append(@"}).add();");

    //    sb.Append(@"});");
    //    sb.Append(@"});");

    //    sb.Append(@"</script>");

    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);


    //}

    public void show1stgraph()
    {

        //fromdt = Request.Cookies["Value1"].Value.ToString().Trim();
        //      todt = Request.Cookies["Value2"].Value.ToString().Trim();
        xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";

        string tempcat = string.Empty;
        string data1 = String.Empty;
        string data2 = string.Empty;
        string data3 = string.Empty;
        String data4 = string.Empty;
        string data5 = string.Empty;
        string xdata = string.Empty;
        string vtype = string.Empty;
        string[] NameArray = new string[5];

        String[] DataArray = new string[5];




        for (int j = 0; j <= 4; j++)
        {
            if (j == 0)
            {
                vtype = "62";

            }
            if (j == 1)
            {
                vtype = "63";

            } if (j == 2)
            {
                vtype = "64";

            }
            if (j <= 2)
            {

                SQuery = "SELECT to_char(vchdate,'YYMM') as mname,ICODE as ICODE,replace(TRIM(col7),'-','0')||'@'||replace(TRIM(col14),'-','0')||'@'||replace(TRIM(col9),'-','0')||'@'||replace(TRIM(col10),'-','0')||'@'||replace(TRIM(col16),'-','0')||'@'||replace(TRIM(col17),'-','0') as rejection,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and vchdate " + xprdrange + " ";

                DataTable dtram1 = new DataTable();
                dtram1.Dispose();
                dtram1 = chartdata1(SQuery, "rejection", "mname");
                SQuery = "SELECT to_char(vchdate,'YYMM') as mname,ICODE as ICODE,replace(TRIM(col5),'-','0') as OUTPUT,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and vchdate " + xprdrange + "";
                DataTable dtram2 = new DataTable();
                dtram2.Dispose();
                dtram2 = chartdata1(SQuery, "output", "mname");
                dtram1.Merge(dtram2);


                DataView dvme = new DataView(dtram1);
                DataTable dt0 = dvme.ToTable(true, new string[] { "mname" });
                dt0.Columns.Add(new DataColumn("rejperc", typeof(double)));

                for (int i = 0; i < dt0.Rows.Count; i++)
                {
                    dt0.Rows[i]["rejperc"] = Math.Round(Convert.ToDouble(dtram1.Compute(" (sum(rejection)/( sum(rejection)+sum(output)))*100", "mname=" + dt0.Rows[i]["mname"] + "")), 2);

                }

                tempcat = string.Empty;
                xdata = string.Empty;
                foreach (DataRow dr in dt0.Rows)
                {
                    tempcat = tempcat + ",'" + return_Month(Convert.ToString(dr["mname"])) + "'";
                    xdata = xdata + "," + Convert.ToString(dr["rejperc"]);
                }

                tempcat = tempcat.Remove(0, 1);
                xdata = xdata.Remove(0, 1);
                if (j == 0)
                {

                    data1 = xdata;
                    NameArray[j] = "Extrusion % Rej";
                    DataArray[j] = data1;
                }
                if (j == 1)
                {

                    data2 = xdata;
                    NameArray[j] = "Cutting % Rej";
                    DataArray[j] = data2;
                }
                if (j == 2)
                {

                    data3 = xdata;
                    NameArray[j] = "Screening % Rej";
                    DataArray[j] = data3;
                }


            }
            else if (j == 3)
            {
                string[] t1 = tempcat.Split(',');
                NameArray[j] = "Total % Rej";
                for (int k = 0; k < t1.Length; k++)
                {
                    string[] d1 = data1.Split(',');
                    string[] d2 = data2.Split(',');
                    string[] d3 = data3.Split(',');

                    data4 = data4 + "," + (Convert.ToDouble(d1[k]) + Convert.ToDouble(d2[k]) + Convert.ToDouble(d3[k]));
                }
                DataArray[j] = data4.Remove(0, 1);
            }
            else if (j == 4)
            {
                string[] t1 = tempcat.Split(',');
                NameArray[j] = "Target % Rej";
                for (int k = 0; k < t1.Length; k++)
                {
                    data5 = data5 + "," + Convert.ToDouble("15");
                }
                DataArray[j] = data5.Remove(0, 1);
            }
        }
        fgen.send_cookie("Chart_Name", "Rejection Graphs");
        fgen.send_cookie("Chart_Type", "line");
        fgen.send_cookie("Chart_Subject", "Rejection");
        Session["Data_Array"] = DataArray;
        Session["Name_Array"] = NameArray;
        fgen.send_cookie("Tempcat", tempcat);
        fgen.send_cookie("Unit", "%");
        Chart_5data("Rejection Charts", "%", tempcat, "line", "Rejection Graph", DataArray, NameArray);
    }

    // public void show2ndgraph( String vtype)
    // {

    //    string  tempcat = string.Empty;
    //  string   data1 = String.Empty;
    // string    xdata = string.Empty;

    // string segment = string.Empty;
    //     string[] NameArray = new string[1];

    //  string[] DataArray = new string[1];


    //////  fromdt = Request.Cookies["Value1"].Value.ToString().Trim();
    ////  todt = Request.Cookies["Value2"].Value.ToString().Trim();
    //  xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";


    //     DateTime date = Convert.ToDateTime(todt);
    //     string xprd1 = date.ToString("yyyyMM");

    //     for (int j = 0; j < 1; j++)
    //     {
    //         if (j == 0)
    //         {
    //             vtype = vtype;

    //         }
    //         if (j <= 2)
    //         {

    //             SQuery = "SELECT '01' AS mname,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col7),'-','0') as Rejection,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '02' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col14),'-','0') as COL2,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '03' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col9),'-','0') as COL3,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '04' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col10),'-','0') as COL4,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '05' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col16),'-','0') as COL5,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '06' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col17),'-','0') as COL6,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "'";

    //             //SQuery = "SELECT to_char(vchdate,'YYMM') as mname,ICODE as ICODE,replace(TRIM(col7),'-','0')||'@'||replace(TRIM(col14),'-','0')||'@'||replace(TRIM(col9),'-','0')||'@'||replace(TRIM(col10),'-','0')||'@'||replace(TRIM(col16),'-','0')||'@'||replace(TRIM(col17),'-','0') as rejection,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and vchdate " + xprdrange + " ";


    //             DataTable dtram1 = new DataTable();
    //             dtram1.Dispose();
    //             dtram1 = chartdata1(SQuery, "rejection", "mname");


    //             DataView dataview = dtram1.DefaultView;
    //             dataview.Sort = "Rejection desc";
    //             dtram1 = dataview.ToTable();

    //             DataView dvme = new DataView(dtram1);
    //             DataTable dt0 = dvme.ToTable(true, new string[] { "mname" });
    //             dt0.Columns.Add(new DataColumn("rejection", typeof(double)));

    //             dt0.Columns["mname"].MaxLength = 50;
    //             for (int i = 0; i < dt0.Rows.Count; i++)
    //             {
    //                 dt0.Rows[i]["rejection"] = Math.Round(Convert.ToDouble(dtram1.Compute("sum(rejection)", "mname=" + dt0.Rows[i]["mname"] + "")), 2);

    //             }


    //             foreach (DataRow dr in dt0.Rows)
    //             {
    //                 switch (dr["mname"].ToString().Trim())
    //                 {
    //                     case "01":
    //                         if (vtype == "62")
    //                             segment = "Butt.Wt(Kg)";
    //                         if (vtype == "63")
    //                             segment = "Set Up Scrap(Kg)";
    //                         if (vtype == "64")
    //                             segment = "Dent(Kg)";
    //                         break;
    //                     case "02":
    //                         if (vtype == "62")
    //                             segment = "Tube with Zinc(Kg)";
    //                         if (vtype == "63")
    //                             segment = "ECT Mark(Kg)";
    //                         if (vtype == "64")
    //                             segment = "Line/Scratch(Kg)";

    //                         break;
    //                     case "03":
    //                         if (vtype == "62")
    //                             segment = "Tube Without Zinc(Kg)";
    //                         if (vtype == "63")
    //                             segment = "Handling(Kg)";
    //                         if (vtype == "64")
    //                             segment = "Tube Rust(Kg)";

    //                         break;
    //                     case "04":
    //                         if (vtype == "62")
    //                             segment = "QA.Sample(Kg)";
    //                         if (vtype == "63")
    //                             segment = "Winding Problem(Kg)";
    //                         if (vtype == "64")
    //                             segment = "Tube Pin Tube(Kg)";

    //                         break;
    //                     case "05":
    //                         if (vtype == "62")
    //                             segment = "Billet Scrap(Kg)";
    //                         if (vtype == "63")
    //                             segment = "Tube Pin Hole(Kg)";
    //                         if (vtype == "64")
    //                             segment = "Radius Damage(Kg)";

    //                         break;
    //                     case "06":
    //                         if (vtype == "62")
    //                             segment = "Other Scrap(Kg)";
    //                         if (vtype == "63" || vtype == "64")
    //                             segment = "Other Tube Scrap(Kg)";

    //                         break;

    //                 }

    //                 dr["mname"] = segment;

    //             }


    //             tempcat = string.Empty;
    //             xdata = string.Empty;
    //             foreach (DataRow dr in dt0.Rows)
    //             {
    //                 tempcat = tempcat + ",'" + Convert.ToString(dr["mname"]) + "'";
    //                 xdata = xdata + "," + Convert.ToString(dr["rejection"]);
    //             }

    //             tempcat = tempcat.Remove(0, 1);
    //             xdata = xdata.Remove(0, 1);
    //             if (j == 0)
    //             {

    //                 data1 = xdata;
    //                 if (vtype == "62") NameArray[j] = "Extrusion Rej";
    //                 if (vtype == "63") NameArray[j] = "Cutting Rej";
    //                 if (vtype == "64") NameArray[j] = "Screening Rej";

    //                 DataArray[j] = data1;
    //             }



    //         }

    //     }

    //     //fgen.send_cookie("Chart_Name", "Rejection Graphs");
    //     //fgen.send_cookie("Chart_Type", "column");
    //     //fgen.send_cookie("Chart_Subject", "Rejection on Extrusion");
    //     //Session["Data_Array"] = DataArray;
    //     //Session["Name_Array"] = NameArray;
    //     //fgen.send_cookie("Tempcat", tempcat);
    //     //fgen.send_cookie("Unit", "Kg");

    //     Chart_5data("Rejection Charts", "Kgs", tempcat, "column", "Rejection Graph", DataArray, NameArray);


    //     //fgen.open_chartlevel_dataArray("Tejaxo ERP Graphs", "Rejection Charts", "%", tempcat, "line", "Rejection Graph", DataArray, NameArray);


    // }
    public void show_Pareto(String vtype)
    {
        string tempcat = string.Empty;
        string data1 = String.Empty;
        string xdata = string.Empty;

        string segment = string.Empty;
        string[] NameArray = new string[1];

        string[] DataArray = new string[1];
        string[] DataArray2 = new string[1];
        string[] DataArray3 = new string[1];

        ////  fromdt = Request.Cookies["Value1"].Value.ToString().Trim();
        //  todt = Request.Cookies["Value2"].Value.ToString().Trim();
        xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
        DateTime date = Convert.ToDateTime(todt);
        string xprd1 = date.ToString("yyyyMM");

        for (int j = 0; j < 1; j++)
        {
            if (j == 0)
            {
                vtype = vtype;
            }
            if (j <= 2)
            {

                SQuery = "SELECT '01' AS mname,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col7),'-','0') as Rejection,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '02' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col14),'-','0') as COL2,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '03' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col9),'-','0') as COL3,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '04' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col10),'-','0') as COL4,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '05' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col16),'-','0') as COL5,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "' UNION ALL SELECT '06' AS segment,'-' as ITEM,ICODE as ICODE,'-' AS PART_NO,replace(TRIM(col17),'-','0') as COL6,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and to_char(vchdate,'yyyymm')='" + xprd1 + "'";

                //SQuery = "SELECT to_char(vchdate,'YYMM') as mname,ICODE as ICODE,replace(TRIM(col7),'-','0')||'@'||replace(TRIM(col14),'-','0')||'@'||replace(TRIM(col9),'-','0')||'@'||replace(TRIM(col10),'-','0')||'@'||replace(TRIM(col16),'-','0')||'@'||replace(TRIM(col17),'-','0') as rejection,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate FROM COSTESTIMATE  WHERE branchcd='" + frm_mbr + "' and COL21='" + vtype + "' and type='40' and vchdate " + xprdrange + " ";

                DataTable dtram1 = new DataTable();
                dtram1.Dispose();
                dtram1 = chartdata1(SQuery, "rejection", "mname");

                DataView dataview = dtram1.DefaultView;
                dataview.Sort = "Rejection desc";
                dtram1 = dataview.ToTable();

                DataView dvme = new DataView(dtram1);
                DataTable dt0 = dvme.ToTable(true, new string[] { "mname" });
                dt0.Columns.Add(new DataColumn("rejection", typeof(double)));

                dt0.Columns["mname"].MaxLength = 50;
                for (int i = 0; i < dt0.Rows.Count; i++)
                {
                    dt0.Rows[i]["rejection"] = Math.Round(Convert.ToDouble(dtram1.Compute("sum(rejection)", "mname=" + dt0.Rows[i]["mname"] + "")), 2);
                }

                foreach (DataRow dr in dt0.Rows)
                {
                    switch (dr["mname"].ToString().Trim())
                    {
                        case "01":
                            if (vtype == "62")
                                segment = "Butt.Wt(Kg)";
                            if (vtype == "63")
                                segment = "Set Up Scrap(Kg)";
                            if (vtype == "64")
                                segment = "Dent(Kg)";
                            break;
                        case "02":
                            if (vtype == "62")
                                segment = "Tube with Zinc(Kg)";
                            if (vtype == "63")
                                segment = "ECT Mark(Kg)";
                            if (vtype == "64")
                                segment = "Line/Scratch(Kg)";

                            break;
                        case "03":
                            if (vtype == "62")
                                segment = "Tube Without Zinc(Kg)";
                            if (vtype == "63")
                                segment = "Handling(Kg)";
                            if (vtype == "64")
                                segment = "Tube Rust(Kg)";

                            break;
                        case "04":
                            if (vtype == "62")
                                segment = "QA.Sample(Kg)";
                            if (vtype == "63")
                                segment = "Winding Problem(Kg)";
                            if (vtype == "64")
                                segment = "Tube Pin Tube(Kg)";
                            break;
                        case "05":
                            if (vtype == "62")
                                segment = "Billet Scrap(Kg)";
                            if (vtype == "63")
                                segment = "Tube Pin Hole(Kg)";
                            if (vtype == "64")
                                segment = "Radius Damage(Kg)";
                            break;
                        case "06":
                            if (vtype == "62")
                                segment = "Other Scrap(Kg)";
                            if (vtype == "63" || vtype == "64")
                                segment = "Other Tube Scrap(Kg)";

                            break;
                    }
                    dr["mname"] = segment;
                }
                tempcat = string.Empty;
                xdata = string.Empty;
                foreach (DataRow dr in dt0.Rows)
                {
                    tempcat = tempcat + ",'" + Convert.ToString(dr["mname"]) + "'";
                    xdata = xdata + "," + Convert.ToString(dr["rejection"]);
                }
                tempcat = tempcat.Remove(0, 1);
                xdata = xdata.Remove(0, 1);
                if (j == 0)
                {

                    data1 = xdata;
                    if (vtype == "62") NameArray[j] = "Extrusion Rej";
                    if (vtype == "63") NameArray[j] = "Cutting Rej";
                    if (vtype == "64") NameArray[j] = "Screening Rej";

                    DataArray[j] = data1;
                }
                double rsum = 0;
                double psum = 0;
                string dra1 = "";
                string drA2 = "";
                string[] dtar = DataArray[0].Split(',');
                for (int p = 0; p < dtar.Length; p++)
                {
                    rsum = rsum + fgen.make_double(dtar[p].ToString().Trim());
                }
                string[] dtar2 = DataArray[0].Split(',');
                for (int q = 0; q < dtar2.Length; q++)
                {
                    string abc = (Math.Round(((fgen.make_double(dtar2[q].ToString()) / rsum) * 100), 2)).ToString();
                    dra1 = dra1 + "," + abc;
                }
                dra1 = dra1.Remove(0, 1);
                string[] dtar3 = dra1.Split(',');

                for (int r = 0; r < dtar3.Length; r++)
                {
                    drA2 = drA2 + "," + (psum + fgen.make_double(dtar3[r].ToString())).ToString();
                    psum = psum + fgen.make_double(dtar3[r].ToString());
                }
                drA2 = drA2.Remove(0, 1);
                DataArray3[0] = drA2;
            }
        }

        // pareto_chart();

        Chart_Pareto("Rejection Charts", "Kgs", tempcat, "column", "Rejection Graph", DataArray, DataArray3);

        //fgen.open_chartlevel_dataArray("Tejaxo ERP Graphs", "Rejection Charts", "%", tempcat, "line", "Rejection Graph", DataArray, NameArray);
    }
    public DataTable chartdata1(string query, string columnname, string group_by)
    {
        DataTable dtGroup = new DataTable();
        double rejtot = 0;
        double prodtot = 0;
        string[] rejec;
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, query);
        string rval1, rval2, rval3, rval4, rval5, rval6, rejstr;
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][columnname].ToString().Trim().Contains("@"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    mq0 = dr[columnname].ToString().Trim();
                    rejec = mq0.Split('@');
                    rval1 = Convert.ToString(rejec[0].Trim());
                    rval1 = (fgen.make_double(string.IsNullOrEmpty(rval1) ? "0" : rval1)).ToString();
                    rval2 = rejec[1].ToString().Trim().Replace(" ", "0");
                    rval3 = rejec[2].ToString().Trim().Replace(" ", "0");
                    rval4 = rejec[3].ToString().Trim().Replace(" ", "0");
                    rval5 = rejec[4].ToString().Trim().Replace(" ", "0");
                    rval6 = rejec[5].ToString().Trim().Replace(" ", "0");
                    rejtot = make_double(rval1) + make_double(rval2) + make_double(rval3) +
                    make_double(rval4) + make_double(rval5) + make_double(rval6);
                    rejstr = rejtot.ToString();
                    dr[columnname] = rejstr;
                }
            }
            DataView dv4 = new DataView(dt);
            dtGroup = dv4.ToTable(true, new string[] { group_by });
            dtGroup.Columns.Add(new DataColumn("Rejection", typeof(Double)));
            dtGroup.Columns.Add(new DataColumn("output", typeof(Double)));
            dtGroup.Columns.Add(new DataColumn("rejperc", typeof(Double)));

            for (int j = 0; j < dtGroup.Rows.Count; j++)
            {
                string check = dtGroup.Rows[j][group_by].ToString();
                double mnttot = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][group_by].ToString().Trim() == check)
                    {
                        mnttot = mnttot + Convert.ToDouble(dt.Rows[i][columnname].ToString().Trim());
                    }
                }
                dtGroup.Rows[j][columnname] = mnttot;
            }
        }
        dt.Dispose();
        return dtGroup;
    }
    public double make_double(string val)
    {
        double result = 0;
        try
        {
            result = Convert.ToDouble(val);
        }
        catch
        {
            result = 0;
        }
        return result;
    }
    public String return_Month(string val)
    {
        String yy = val.Substring(0, 2);
        String mm = val.Substring(2, 2);
        int input = Convert.ToInt16(mm);
        String Result = string.Empty;
        switch (input)
        {

            case 1:
                Result = "Jan";
                break;
            case 2:
                Result = "Feb";
                break;
            case 3:
                Result = "Mar";
                break;
            case 4:
                Result = "Apr";
                break;
            case 5:
                Result = "May";
                break;
            case 6:
                Result = "Jun";
                break;
            case 7:
                Result = "Jul";
                break;
            case 8:
                Result = "Aug";
                break;
            case 9:
                Result = "Sep";
                break;
            case 10:
                Result = "Oct";
                break;
            case 11:
                Result = "Nov";
                break;
            case 12:
                Result = "Dec";
                break;
        }
        Result = Result + "-" + yy;
        return Result;
    }

    public void tick(Boolean postback)
    {
        if (ViewState["lastindex"] == null)
        {
            ViewState["lastindex"] = 0;
        }
        int pcount = sg1.PageCount;
        int pIndex = sg1.PageIndex;
        Int32 idd = (Int32)ViewState["lastindex"];
        if (postback)
        {
            pIndex = pIndex - 1;
            idd = idd - 1;
            ViewState["lastindex"] = idd;
        }
        if (pIndex.ToString() == "2")
        {
            ViewState["lastindex"] = 2;
        }

        // mq10 = Request.Cookies["lastindex"].Value.ToString().Trim();
        if (idd == 2)
        {
            //fgen.send_cookie("lastindex",(Convert.ToDecimal(Request.Cookies["lastindex"].Value.ToString().Trim()) + 1).ToString());

            ViewState["lastindex"] = idd + 1;
            sg1.DataSource = null;
            sg1.DataBind();
            sg1.Visible = false;
            lblstage.Text = "Monthly Rejection % Graph for Current Year";
            show1stgraph();
            return;
        }
        else if (idd == 3)
        {
            //fgen.send_cookie("lastindex", (Convert.ToDecimal(Request.Cookies["lastindex"].Value.ToString().Trim()) + 1).ToString());

            ViewState["lastindex"] = idd + 1;
            lblstage.Text = "Extrusion Rejection Graph for Current Month";
            show_Pareto("62");
            return;
        }
        else if (idd == 4)
        {
            //fgen.send_cookie("lastindex", (Convert.ToDecimal(Request.Cookies["lastindex"].Value.ToString().Trim()) + 1).ToString());

            ViewState["lastindex"] = idd + 1;
            lblstage.Text = "Cutting Rejection Graph for Current Month";
            show_Pareto("63");
            return;
        }
        else if (idd == 5)
        {
            //fgen.send_cookie("lastindex", (Convert.ToDecimal(Request.Cookies["lastindex"].Value.ToString().Trim()) + 1).ToString());
            ViewState["lastindex"] = idd + 1;
            lblstage.Text = "Screening Rejection Graph for Current Month";
            show_Pareto("64");
            return;
        }
        else if (idd == 6)
        {
            //fgen.send_cookie("lastindex", "0");
            ViewState["lastindex"] = 0;
            pIndex = 0;
            pcount = 4;
            sg1.Visible = true;
        }
        //if (pIndex == pcount)
        //{

        //}
        if (pIndex != pcount)
        {
            sg1.PageIndex = pIndex + 1;
            DisplayData();
            upd.Update();
        }
        if (pIndex == pcount - 1)
        {
            ViewState["lastindex"] = pIndex;
            pIndex = pIndex - pcount;
            sg1.PageIndex = pIndex + 1;
            DisplayData();
            upd.Update();
        }
        if (hfhcid.Value.Trim() == "F35509")
        {
            if (sg1.Rows.Count > 0)
            {
                foreach (GridViewRow r in sg1.Rows)
                {
                    if (r.Cells[1].Text.Trim() == "Rejection Reasons as Under" || r.Cells[1].Text.Trim() == "Critical Ratio")
                    {
                        r.BackColor = System.Drawing.Color.FromName("#58D3F7");
                        // r.BackColor = System.Drawing.Color.Purple;
                    }

                    if (r.Cells[1].Text.Trim() == "EXTRUSION")
                    {
                        lbl1.Text = "Monthly Plan: " + mq1;
                        lblstage.Text = "Stage:- EXTRUSION";
                        lbltoday.Text = "Today:- " + DateTime.Now.Date.ToShortDateString();
                        r.BackColor = System.Drawing.Color.Aqua;
                    }
                    if (r.Cells[1].Text.Trim() == "CUTTING:")
                    {
                        lbl1.Text = "Monthly Plan: " + mq2;
                        lblstage.Text = "Stage:- CUTTING";
                        lbltoday.Text = "Today:- " + DateTime.Now.Date.ToShortDateString();
                        r.BackColor = System.Drawing.Color.Aqua;
                    }
                    if (r.Cells[1].Text.Trim() == "SCREENING:")
                    {
                        lbl1.Text = "Monthly Plan: " + mq3;
                        lblstage.Text = "Stage:- SCREENING";
                        lbltoday.Text = "Today:- " + DateTime.Now.Date.ToShortDateString();
                        r.BackColor = System.Drawing.Color.Aqua;
                    }

                    if (r.Cells[1].Text.Trim() == "Overall % Rej.(Ext.+Cutting+Screening)")
                    {
                        lbl1.Text = "";
                        lblstage.Text = "Overall % Rej.(Ext.+Cutting+Screening)";
                        lbltoday.Text = "Today:- " + DateTime.Now.Date.ToShortDateString();

                    }

                    if (r.Cells[1].Text.Trim() == "Rejection %" || r.Cells[1].Text.Trim() == "Overall % Rej.(Ext.+Cutting+Screening)".Trim())
                    {

                        for (int i = 2; i < r.Cells.Count; i++)
                        {

                            decimal cellText = decimal.Parse(r.Cells[i].Text.Replace("%", ""));

                            if (lblstage.Text == "Stage:- EXTRUSION")
                            {
                                mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "Select acref  from typegrp where id='R1' and trim(type1)=(select trim(max(type1))  from typegrp where id='R1'  and trim(acref3)='62')", "acref");
                                if (cellText <= Convert.ToDecimal(mq10))
                                {
                                    r.Cells[i].BackColor = System.Drawing.Color.FromName("#81F781");
                                }
                                else
                                { r.Cells[i].BackColor = System.Drawing.Color.FromName("#FA5882"); }
                            }
                            else if (lblstage.Text == "Stage:- CUTTING")
                            {
                                mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "Select acref  from typegrp where id='R1' and trim(type1)=(select trim(max(type1))  from typegrp where id='R1'  and trim(acref3)='63')", "acref");
                                if (cellText <= Convert.ToDecimal(mq10))
                                {
                                    r.Cells[i].BackColor = System.Drawing.Color.FromName("#81F781");
                                }
                                else
                                { r.Cells[i].BackColor = System.Drawing.Color.FromName("#FA5882"); }
                            }
                            else if (lblstage.Text == "Stage:- SCREENING")
                            {
                                mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "Select acref  from typegrp where id='R1' and trim(type1)=(select trim(max(type1))  from typegrp where id='R1'  and trim(acref3)='64')", "acref");
                                if (cellText <= Convert.ToDecimal(mq10))
                                {
                                    r.Cells[i].BackColor = System.Drawing.Color.FromName("#81F781");
                                }
                                else
                                { r.Cells[i].BackColor = System.Drawing.Color.FromName("#FA5882"); }
                            }
                            else if (lblstage.Text.Trim() == "Overall % Rej.(Ext.+Cutting+Screening)".Trim())
                            {
                                if (cellText <= 15)
                                {
                                    r.Cells[i].BackColor = System.Drawing.Color.FromName("#81F781");
                                }
                                else
                                { r.Cells[i].BackColor = System.Drawing.Color.FromName("#FA5882"); }
                            }
                        }

                    }
                }
            }
        }
        fill_header();
    }

}