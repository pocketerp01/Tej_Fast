using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_ActIt_review : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt1, dt2, dt3, dt4,dt5, dticode, dticode2, dtm;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataRow dr1;
    DataTable dtCol = new DataTable();
    string Prg_Id, party_cd = "", part_cd = "";
    double db1, db2, db3, db4, db5, db6, db7, db8, db9, db10, db;
    string DateRange;
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, cDT1, cDT2, xprdrange1, mq0, mq1, mq2, mq3, mq4, mq5,mq6,mq7,mq8,mq9,mq10;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    DataView dv, dv1;
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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
            }
            set_Val();
        }
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = false; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F25233":
                lblheader.Text = "Item Review";
                break;

            case "F70282":
                lblheader.Text = "Account Review";
                break;

            case "F10305":
                lblheader.Text = "ManPower Planning";
                break;
        }
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            default:
                SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.hscode,0 AS BAL,NULL AS btchno,NULL AS BTCHDT,a.irate from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") order by a.Iname ";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        if (frm_formID == "F10305")
        {
            hffield.Value = frm_formID;
            SQuery = "SELECT MTHNUM AS FSTR,MTHNUM,MTHNAME FROM MTHS";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_mseek("Select Months", frm_qstr);
        }
        else
        {
            fgen.Fn_open_Act_itm_prd("-", frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["sg1"];
        // fgen.exp_to_excel(dtCol, ".xls", "", "ff.xls");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1_dt = new DataTable();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        //--
        string CP_BTN;
        CP_BTN = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_FROM");
        string CP_HF1;
        CP_HF1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
        hf1.Value = CP_HF1;
        if (CP_BTN.Trim().Length > 1)
        {
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
            {
                btnval = CP_BTN;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "0");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "0");
        //--
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {

        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "F10305":
                    hf1.Value = col1;
                    doc_nf.Value = col3;
                    btnhideF_s_Click(sender, e);
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
        part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
        cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
        dr1 = null;

        DateRange = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
        if (frm_formID == "F25233")
        {
            #region Item Review
            if (party_cd.Length != 2)
            {
                fgen.msg("-", "AMSG", "Please Select Main Group");
                return;
            }
            else if (part_cd.Length != 1)
            {
                fgen.msg("-", "AMSG", "Please Select Either 'Store' Or 'Rejection'");
                return;
            }
            dt3 = new DataTable();
            dt3.Columns.Add("PartNo", typeof(string));
            dt3.Columns.Add("Code", typeof(string));
            dt3.Columns.Add("Name", typeof(string));
            dt3.Columns.Add("Unit", typeof(string));
            //dt3.Columns.Add("HSNCode", typeof(string));
            dt3.Columns.Add("OpBal", typeof(double));
            dt3.Columns.Add("ClBal", typeof(double));

            SQuery = "select sum(a.iqtyin) as iqtyin,sum(a.iqtyout) as iqtyout,sum(iqtyin)-sum(iqtyout) as bal ,a.type,t.name from ivoucher a LEFT JOIN type t ON trim(a.type)=trim(t.type1) and t.ID='M' where a.branchcd='" + frm_mbr + "' and substr(trim(a.icode),1,2)='" + party_cd + "' and a.vchdate " + DateRange + " and a.store='" + part_cd + "' group by a.type,t.name order by a.type";
            dt1 = new DataTable();
            dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            foreach (DataRow dr in dt1.Rows)
            {
                dt3.Columns.Add(dr["type"].ToString().Trim(), typeof(double));
            }
            dt3.Columns.Add("NetColTot", typeof(double));

            // STOCK QUERY
            dt2 = new DataTable();
            xprdrange1 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
            if (part_cd == "Y")
            {
                mq0 = "select a.branchcd,trim(a.icode) as icode,i.iname,i.unit,i.cpartno,i.hscode,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + DateRange + " and store='Y' GROUP BY ICODE,branchcd )a,item i where trim(a.icode)=trim(i.icode) and substr(trim(a.icode),1,2)='" + party_cd + "' and LENGTH(tRIM(a.ICODE))>=8 group by a.branchcd,trim(a.icode),i.iname,i.unit,i.cpartno,i.hscode having (sum(a.opening)!=0  or sum(a.cdr)!=0 or sum(a.ccr)!=0) ORDER BY ICODE";
            }
            else
            {
                mq0 = "select a.branchcd,trim(a.icode) as icode,i.iname,i.unit,i.cpartno,i.hscode,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='R'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + DateRange + " and store='R' GROUP BY ICODE,branchcd )a,item i where trim(a.icode)=trim(i.icode) and substr(trim(a.icode),1,2)='" + party_cd + "' and LENGTH(tRIM(a.ICODE))>=8 group by a.branchcd,trim(a.icode),i.iname,i.unit,i.cpartno,i.hscode having (sum(a.opening)!=0  or sum(a.cdr)!=0 or sum(a.ccr)!=0) ORDER BY ICODE";
            }
            dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

            // DETAILS
            dt = new DataTable();
            mq1 = "select trim(a.icode) as icode,sum(a.iqtyin) as iqtyin,sum(a.iqtyout) as iqtyout,trim(a.type) as type,a.store from ivoucher a where a.branchcd='" + frm_mbr + "' and substr(trim(a.icode),1,2)='" + party_cd + "' and a.vchdate " + DateRange + " and a.store='" + part_cd + "' group by trim(a.icode),trim(a.type),a.store order by icode";
            dt = fgen.getdata(frm_qstr, frm_cocd, mq1);

            if (dt2.Rows.Count > 0)
            {
                dv = new DataView(dt2);
                dticode = new DataTable();
                dticode = dv.ToTable(true, "icode", "iname", "unit", "cpartno", "opening", "cl");
                foreach (DataRow dr0 in dticode.Rows)
                {
                    dticode2 = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dv1 = new DataView(dt, "icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dticode2 = dv1.ToTable();
                    }
                    db1 = 0; db2 = 0;
                    dr1 = dt3.NewRow();
                    if (dticode2.Rows.Count == 0)
                    {
                        dr1["partno"] = dr0["cpartno"].ToString().Trim();
                        dr1["code"] = dr0["icode"].ToString().Trim();
                        dr1["name"] = dr0["iname"].ToString().Trim();
                        dr1["unit"] = dr0["unit"].ToString().Trim();
                        dr1["opbal"] = fgen.make_double(dr0["opening"].ToString());
                        dr1["clbal"] = fgen.make_double(dr0["cl"].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < dticode2.Rows.Count; i++)
                        {
                            dr1["partno"] = dr0["cpartno"].ToString().Trim();
                            dr1["code"] = dr0["icode"].ToString().Trim();
                            dr1["name"] = dr0["iname"].ToString().Trim();
                            dr1["unit"] = dr0["unit"].ToString().Trim();
                            dr1["opbal"] = fgen.make_double(dr0["opening"].ToString());
                            dr1["clbal"] = fgen.make_double(dr0["cl"].ToString());
                            mq2 = dticode2.Rows[i]["type"].ToString().Trim();
                            //if (fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString()) > 0)
                            //{
                            //    dr1[mq2] = fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString());
                            //}
                            //if (fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString()) > 0)
                            //{
                            //    dr1[mq2] = "-" + fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString());
                            //}
                            dr1[mq2] = fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString()) - fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString());
                            db1 += fgen.make_double(dticode2.Rows[i]["iqtyin"].ToString());
                            db2 += fgen.make_double(dticode2.Rows[i]["iqtyout"].ToString());
                            dr1["netcoltot"] = db1 - db2;
                        }
                    }
                    dt3.Rows.Add(dr1);
                }
            }
            #endregion
        }
        else if (frm_formID == "F70282")
        {
            #region Account Review
            if (party_cd.Length != 2)
            {
                fgen.msg("-", "AMSG", "Please Select Account Group");
                return;
            }
            dt3 = new DataTable();
            dt3.Columns.Add("PartNo", typeof(string));
            dt3.Columns.Add("Code", typeof(string));
            dt3.Columns.Add("Name", typeof(string));
            dt3.Columns.Add("OpBal", typeof(double));
            dt3.Columns.Add("ClBal", typeof(double));

            SQuery = "select sum(a.dramt) as iqtyin,sum(a.cramt) as iqtyout,sum(dramt) - sum(cramt) as bal,a.type,t.name from voucher a LEFT JOIN type t ON trim(a.type)=trim(t.type1) and t.ID='V' where a.branchcd='" + frm_mbr + "' and substr(trim(a.acode),1,2)='" + party_cd + "' and a.vchdate " + DateRange + " group by a.type,t.name order by a.type";
            dt1 = new DataTable();
            dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            foreach (DataRow dr in dt1.Rows)
            {
                dt3.Columns.Add(dr["type"].ToString().Trim(), typeof(double));
            }
            dt3.Columns.Add("NetColTot", typeof(double));

            // STOCK QUERY
            dt2 = new DataTable();
            xprdrange1 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
            mq0 = "select a.branchcd,trim(a.acode) as acode,f.aname,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.acode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal a where a.branchcd='" + frm_mbr + "' union all select branchcd,acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos FROM VOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " GROUP BY acode,branchcd union all select branchcd,acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from VOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + DateRange + " GROUP BY acode,branchcd )a,famst f WHERE trim(a.acode)=trim(f.acode) and substr(trim(a.acode),1,2)='" + party_cd + "' having (sum(a.opening)!=0  or sum(a.cdr)!=0 or sum(a.ccr)!=0) group by a.branchcd,trim(a.acode),f.aname ORDER BY f.aname";
            dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

            // DETAILS
            dt = new DataTable();
            mq1 = "select acode,sum(dramt) as dramt,sum(cramt) as cramt,type from voucher where branchcd='" + frm_mbr + "' and vchdate " + DateRange + " and substr(trim(acode),1,2)='" + party_cd + "' group by acode,type order by acode";
            dt = fgen.getdata(frm_qstr, frm_cocd, mq1);

            if (dt2.Rows.Count > 0)
            {
                dv = new DataView(dt2);
                dticode = new DataTable();
                dticode = dv.ToTable(true, "acode", "aname", "opening", "cl");
                foreach (DataRow dr0 in dticode.Rows)
                {
                    dticode2 = new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        dv1 = new DataView(dt, "acode='" + dr0["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dticode2 = dv1.ToTable();
                    }
                    db1 = 0; db2 = 0;
                    dr1 = dt3.NewRow();
                    if (dticode2.Rows.Count == 0)
                    {
                        dr1["partno"] = "-";
                        dr1["code"] = dr0["acode"].ToString().Trim();
                        dr1["name"] = dr0["aname"].ToString().Trim();
                        dr1["opbal"] = fgen.make_double(dr0["opening"].ToString());
                        dr1["clbal"] = fgen.make_double(dr0["cl"].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < dticode2.Rows.Count; i++)
                        {
                            dr1["partno"] = "-";
                            dr1["code"] = dr0["acode"].ToString().Trim();
                            dr1["name"] = dr0["aname"].ToString().Trim();
                            dr1["opbal"] = fgen.make_double(dr0["opening"].ToString());
                            dr1["clbal"] = fgen.make_double(dr0["cl"].ToString());
                            mq2 = dticode2.Rows[i]["type"].ToString().Trim();
                            dr1[mq2] = fgen.make_double(dticode2.Rows[i]["dramt"].ToString()) - fgen.make_double(dticode2.Rows[i]["cramt"].ToString());
                            db1 += fgen.make_double(dticode2.Rows[i]["dramt"].ToString());
                            db2 += fgen.make_double(dticode2.Rows[i]["cramt"].ToString());
                            dr1["netcoltot"] = db1 - db2;
                        }
                    }
                    dt3.Rows.Add(dr1);
                }
            }
            #endregion
        }
        else if (frm_formID == "F10305")
        {
            #region Man Power Planning
            dtm = new DataTable();
            dtm.Columns.Add("Line", typeof(string)); //1
            dtm.Columns.Add("Area", typeof(string)); //2
            dtm.Columns.Add("Item_Code", typeof(string)); //3
            dtm.Columns.Add("Part_Number", typeof(string)); //3
            dtm.Columns.Add("Part_Name", typeof(string)); //4
            dtm.Columns.Add("Process", typeof(string)); //5
            dtm.Columns.Add("Unit", typeof(string)); //6
            dtm.Columns.Add("Unit1", typeof(string)); //7
            dtm.Columns.Add("Cycle_Time", typeof(string)); //8
            dtm.Columns.Add("Cavity_No_OF_Pcs", typeof(string)); //9 
            dtm.Columns.Add("Operating_Rate_Per", typeof(string)); //10
            dtm.Columns.Add("Attendance_Per", typeof(string)); //11
            dtm.Columns.Add("No_of_Manpower_Deployed", typeof(string)); //12
            dtm.Columns.Add("Cycle_Piece", typeof(string)); //13
            dtm.Columns.Add("Manhours", typeof(double)); //14

            string next_year = "", dd2 = "";
            string[] arr = hf1.Value.Split(',');
            int counter = 0; string dd1 = "";
            counter = arr.Length;

            for (int l = 0; l < counter; l++)
            {
                if (Convert.ToInt32(arr[l].ToString().Replace("'", "")) <= 3)
                {
                    dd2 = arr[l].ToString().Replace("'", "") + (Convert.ToInt32(frm_myear) + 1).ToString();
                }
                else
                {
                    dd2 = arr[l].ToString().Replace("'", "") + frm_myear;
                }
                next_year = ",'" + dd2 + "'";
                dd1 = dd1 + next_year;
            }

            dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable();
            mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = "";
            string year1 = System.DateTime.Now.Year.ToString();
            SQuery = "SELECT TRIM(ICODE) AS ICODE,TRIM(INAME) AS INAME,TRIM(CPARTNO) AS PART,TRIM(UNIT) AS UNIT FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1)>='9' AND  LENGTH(TRIM(ICODE))>=8 ORDER BY ICODE";
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); ///item table

            mq0 = "select name,type1 from typegrp where id='^7'";//type change karni hai
            dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0); // for area

            mq1 = "select trim(a.icode) as icode,a.stagec,a.mtime as cycle_time,a.area,a.lineno,a.CAVITY_PC,OP_RATE,a.NO_MAN,B.NAME as process from itwstage a,TYPE B  where TRIM(A.icode) in (select distinct icode from (select trim(icode) as icode from pschedule where branchcd='" + frm_mbr + "' and type='15' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9' union all select trim(icode) as icode from mthlyplan WHERE BRAnchcd='" + frm_mbr + "' and type='10' and to_char(vchdate,'MMyyyy') IN (" + dd1.TrimStart(',') + ") and substr(trim(icode),1,1)='9')) and a.branchcd='" + frm_mbr + "' and a.type='10'  AND TRIM(A.STAGEC)=TRIM(B.TYPE1) and b.id='K' order by a.area asc";   //  ....in this qry only 9 series item will come...and as per client also                       
            dt2 = fgen.getdata(frm_qstr, frm_cocd, mq1);  //UNION OF PSHEDULE AND MTHLYPLAN TABLE......main dt

            mq3 = "SELECT A.ICODE,SUM(A.TARGET) AS TARGET,B.MTHNAME,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ FROM MTHLYPLAN A ,mths b  WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='10'  AND TO_CHAR(A.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=TRIM(B.MTHNUM)  GROUP BY A.ICODE,A.VCHDATE,B.MTHNAME ,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd ";
            dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);  //plan dt

            mq4 = "select trim(a.icode) as icode ,SUM(a.TOTAL) AS TOTAL,b.mthname ,TO_CHAR(A.VCHDATE,'MM/YYYY') AS VDD,TO_CHAR(A.VCHDATE,'MM') as mth_ from PSCHEDULE a ,mths b where a.BRANCHCD='" + frm_mbr + "' and a.type='15' and TO_CHAR(a.VCHDATE,'MMyyyy') IN (" + dd1.TrimStart(',') + ")  and to_char(a.vchdate,'MM')=trim(b.mthnum) group by trim(a.icode),b.mthname,TO_CHAR(A.VCHDATE,'MM/YYYY'),TO_CHAR(A.VCHDATE,'MM') ORDER BY vdd"; //this qry used when need to show month
            dt4 = new DataTable();
            dt4 = fgen.getdata(frm_qstr, frm_cocd, mq4);  //schedule dt  

            mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='B26'", "params");

            mq5 = "select name,type1 from type where id='1' and  type1>'69' and type1!='6R'";
            dt5 = fgen.getdata(frm_qstr, frm_cocd, mq5); //for line, line no 

            if (dt2.Rows.Count > 0)
            {
                counter = arr.Length;
                for (int k = 0; k < counter; k++)
                {
                    dtm.Columns.Add("Vulcanisation" + arr[k].Replace("'", "_") + "", typeof(double));
                    dtm.Columns.Add("Transfer" + arr[k].Replace("'", "_") + "", typeof(double));
                    dtm.Columns.Add("Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                    dtm.Columns.Add("Transfer_ManPower_Req" + arr[k].Replace("'", "_") + "", typeof(double));
                }

                mq1 = "";
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    #region
                    dr1 = dtm.NewRow();
                    double db10 = 0, db11 = 0;
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; mq5 = ""; mq6 = "";
                    dr1["Area"] = fgen.seek_iname_dt(dt1, "type1='" + dt2.Rows[i]["area"].ToString().Trim() + "'", "name");
                    dr1["Line"] = fgen.seek_iname_dt(dt5, "type1='" + dt2.Rows[i]["Lineno"].ToString().Trim() + "'", "name"); //abi
                    dr1["Item_Code"] = dt2.Rows[i]["icode"].ToString().Trim();
                    dr1["Part_Number"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "PART");
                    dr1["Part_Name"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                    dr1["Process"] = dt2.Rows[i]["process"].ToString().Trim(); //fgen.seek_iname_dt(dt3, "icode='" + dtm1.Rows[i]["icode"].ToString().Trim() + "'", "process");
                    dr1["Unit"] = fgen.seek_iname_dt(dt, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "unit");
                    dr1["Unit1"] = dr1["unit"].ToString().Trim() + "/" + "PCE";
                    dr1["Cycle_Time"] = fgen.make_double(dt2.Rows[i]["cycle_time"].ToString().Trim());
                    dr1["Cavity_No_OF_Pcs"] = fgen.make_double(dt2.Rows[i]["CAVITY_PC"].ToString().Trim());
                    dr1["Operating_Rate_Per"] = fgen.make_double(dt2.Rows[i]["OP_RATE"].ToString().Trim());
                    dr1["Attendance_Per"] = mq10;
                    dr1["No_of_Manpower_Deployed"] = fgen.make_double(dt2.Rows[i]["NO_MAN"].ToString().Trim());
                    db4 = fgen.make_double(dr1["Cycle_Time"].ToString().Trim());
                    db5 = fgen.make_double(dr1["Cavity_No_OF_Pcs"].ToString().Trim());
                    db6 = fgen.make_double(dr1["Operating_Rate_Per"].ToString().Trim());
                    db7 = fgen.make_double(dr1["Attendance_Per"].ToString().Trim());
                    db8 = fgen.make_double(dr1["No_of_Manpower_Deployed"].ToString().Trim());
                    if (db5 == 0 || db6 == 0 || db7 == 0)
                    {
                        dr1["Cycle_Piece"] = 0;
                    }
                    else
                    {
                        db9 = ((db4 / db5 / db6 / db7) * db8) * 100;
                        dr1["Cycle_Piece"] = Math.Round(db9, 5);
                    }
                    db3 = fgen.make_double(dr1["Cycle_Piece"].ToString().Trim());

                    mq5 = "";
                    for (int k = 0; k < counter; k++)
                    {
                        mq9 = "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + "";
                        db = fgen.make_double(fgen.seek_iname_dt(dt4, mq9, "TOTAL")); //for  Vulcanisation
                        db1 = fgen.make_double(fgen.seek_iname_dt(dt3, "icode='" + dt2.Rows[i]["icode"].ToString().Trim() + "' and mth_=" + arr[k] + " ", "TARGET"));//for transfer                                     
                        dr1["Vulcanisation" + arr[k].Replace("'", "_") + ""] = db;
                        dr1["Transfer" + arr[k].Replace("'", "_") + ""] = db1;
                        if (db3 == 0)
                        {
                            dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                            dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                        }
                        else
                        {
                            // dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db * db3 / 3600);
                            dr1["Vulcanisation_ManPower_Req" + arr[k].Replace("'", "_") + ""] = 0;
                            dr1["Transfer_ManPower_Req" + arr[k].Replace("'", "_") + ""] = Math.Ceiling(db1 * db3 / 3600);
                        }
                        db10 = fgen.make_double(dr1["Transfer" + arr[k].Replace("'", "_") + ""].ToString());
                        if (db3 == 0 || db10 == 0)
                        {
                            dr1["Manhours"] = 0;
                        }
                        else
                        {
                            dr1["Manhours"] = Math.Round((db3) * db10 / 3600, 5);
                        }
                    }
                    dtm.Rows.Add(dr1);
                    #endregion
                }
            }
            ////for add row on top for total
            if (dtm.Rows.Count > 0)
            {
                dr1 = dtm.NewRow();
                foreach (DataColumn dc in dtm.Columns)
                {
                    db1 = 0;
                    if (dc.Ordinal == 0 || dc.Ordinal == 1 || dc.Ordinal == 2 || dc.Ordinal == 3 || dc.Ordinal == 4 || dc.Ordinal == 5 || dc.Ordinal == 6 || dc.Ordinal == 7 || dc.Ordinal == 8 || dc.Ordinal == 9 || dc.Ordinal == 10 || dc.Ordinal == 11 || dc.Ordinal == 12 || dc.Ordinal == 13 || dc.Ordinal == 14)
                    { }
                    else
                    {
                        mq1 = "sum(" + dc.ColumnName + ")";
                        db1 += fgen.make_double(dtm.Compute(mq1, "").ToString());
                        dr1[dc] = db1;
                    }
                }
                dr1[2] = "TOTAL";
                dtm.Rows.InsertAt(dr1, 0);
            }
            #endregion

            dt3 = new DataTable();
            dt3 = dtm.Copy();
        }
        if (dt3.Rows.Count > 0)
        {
            dr1 = null;
            dr1 = dt3.NewRow();
            lblRowsCount.Text = "Total Rows : " + dt3.Rows.Count;
            if (frm_formID != "F10305")
            {
                lblDate.Text = "For the Period : " + fromdt + " To " + todt;
                foreach (DataRow dr2 in dt1.Rows)
                {
                    mq5 = dr2["type"].ToString().Trim();
                    dr1[mq5] = fgen.make_double(dr2["bal"].ToString());
                }

                dr1["name"] = "Net Col. Total";
                dt3.Rows.InsertAt(dr1, 0);
                z = 0;
                foreach (DataColumn dc in dt3.Columns)
                {
                    int abc = dc.Ordinal;
                    if (frm_formID == "F25233")
                    {
                        z = 5;
                    }
                    else if (frm_formID == "F70282")
                    {
                        z = 4;
                    }
                    if (abc > z)
                    {
                        string name = dc.ToString().Trim();
                        string myname = fgen.seek_iname_dt(dt1, "type='" + name + "'", "name");
                        try
                        {
                            if (myname != "0")
                            {
                                dt3.Columns[abc].ColumnName = "(" + name + ") " + myname;
                            }
                        }
                        catch { }
                    }
                }
            }
            ViewState["sg1"] = dt3;
            sg1.DataSource = dt3;
            sg1.DataBind();
            if (dt3.Rows.Count > 0)
            {
                sg1.Rows[1].Enabled = false;
            }
        }
        else
        {
            fgen.msg("-", "AMSG", "No Activity During the Selected Period");
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            z = 0;
            if (frm_formID == "F25233")
            {
                z = 6;
            }
            else if (frm_formID == "F70282")
            {
                z = 5;
            }
            if (frm_formID == "F10305")
            {
                for (int i = z; i < e.Row.Cells.Count; i++)
                {
                    TableCell cell = e.Row.Cells[i];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                    cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell.ToolTip = "You can click this cell";
                    cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                }
            }
            else
            {
                // IN ITEM & ACCOUNT REVIEW LAST COLUMN SHOWS THE TOTAL THAT'S WHY CLICK EVENT IS DISABLED
                for (int i = z; i < e.Row.Cells.Count - 1; i++)
                {
                    TableCell cell = e.Row.Cells[i];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                    cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell.ToolTip = "You can click this cell";
                    cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                }
            }

            // COLUMN ALINGMENT
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                int CheckDataType = System.Text.RegularExpressions.Regex.Matches(e.Row.Cells[i].Text, @"[a-zA-Z]").Count;
                if (CheckDataType != 0)
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;
                }
                else
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
    }
    //------------------------------------------------------------------------------------  
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        SQuery = "";
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text; // dynamic heading
        mq1 = selectedRow.Cells[1].Text.Trim(); // icode or acode
        mq2 = "";
        mq3 = selectedRow.Cells[2].Text.Trim();
        mq4 = selectedRow.Cells[4].Text.Trim();

        party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
        part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        DateRange = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

        if (frm_formID == "F25233")
        {
            mq2 = "Details of the Selected Item : " + mq3 + " (" + mq1 + ")";
            SQuery = "select a.type as fstr,a.type,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.vchnum,a.acode,f.aname as refer_name,a.iqtyin,a.iqtyout,a.o_deptt,a.store,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.stage from ivoucher a left join famst f on trim(a.acode)=trim(f.acode) where a.branchcd='" + frm_mbr + "' and type='" + mq0.Substring(1, 2) + "' and a.icode='" + mq1 + "' and vchdate " + DateRange + " and a.store='" + part_cd + "' order by vchnum";
        }
        else if (frm_formID == "F70282")
        {
            mq2 = "Details of the Selected Account : " + mq3 + " (" + mq1 + ")";
            SQuery = "select a.type as fstr,f.aname as refer_name,a.type,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.vchnum,a.acode,a.dramt,a.cramt,a.ent_by,to_char(a.ent_date,'dd/mm/yyyy') as ent_dt,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.refnum,a.naration from voucher a left join famst f on trim(a.rcode)=trim(f.acode) where a.branchcd='" + frm_mbr + "' and type='" + mq0.Substring(1, 2) + "' and a.acode='" + mq1 + "' and vchdate " + DateRange + " order by vchnum";
        }
        else if (frm_formID == "F10305")
        {
            #region Man Power Planning
            string year = System.DateTime.Now.Year.ToString();
            if (selectedCellIndex == 5)
            {
                mq2 = "Stage Mapping Details Of The Item : " + mq4 + " (" + mq3 + ")";
                SQuery = "select a.vchnum as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.stagec as code,t.name as stage,a.srno from itwstage a,type t where trim(a.stagec)=trim(t.type1) and t.id='K' and a.type='10' and trim(a.icode)='" + selectedRow.Cells[2].Text.Trim() + "' order by a.srno";
            }
            else if (selectedCellIndex == 4)
            {
                mq2 = "BOM Details Of The Item : " + mq4 + " (" + mq3 + ")";
                fgen.drillQuery(0, "select trim(a.ibcode) as fstr,'-' as gstr, a.icode as main_code,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ibcode as child_code,i.iname as child_name,i.cpartno as child_partno,i.unit,a.ibqty as main_qty,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when a.edt_by='-' then '-' else to_char(a.edt_dt,'dd/mm/yyyy') end) as edt_dt from itemosp a,item i where trim(a.ibcode)=trim(i.icode) and  a.branchcd='" + frm_mbr + "' and a.type='BM' and a.icode='" + mq3 + "' order by a.srno", frm_qstr);
                fgen.drillQuery(1, "select trim(a.ibcode) as fstr,trim(a.icode) as gstr,a.ibcode as child_code, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode as main_code,i.iname as child_name,i.cpartno as child_partno,i.unit,a.ibqty as main_qty,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when a.edt_by='-' then '-' else to_char(a.edt_dt,'dd/mm/yyyy') end) as edt_dt from itemosp a,item i where trim(a.ibcode)=trim(i.icode) and  a.branchcd='" + frm_mbr + "' and a.type='BM' order by a.srno", frm_qstr);
                fgen.drillQuery(2, "select trim(a.ibcode) as fstr,trim(a.icode) as gstr,a.icode as main_code, a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ibcode as child_code,i.iname as child_name,i.cpartno as child_partno,i.unit,a.ibqty as main_qty,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when a.edt_by='-' then '-' else to_char(a.edt_dt,'dd/mm/yyyy') end) as edt_dt from itemosp a,item i where trim(a.ibcode)=trim(i.icode) and  a.branchcd='" + frm_mbr + "' and a.type='BM' order by a.srno", frm_qstr);
                fgen.Fn_DrillReport(mq2, frm_qstr);
            }
            else if (selectedCellIndex == 1)
            {
                #region
                 dtm = new DataTable();
                 if (ViewState["sg1"] != null)
                 {
                     dtm = (DataTable)ViewState["sg1"];
                     DataTable ph_tbl = new DataTable();
                     ph_tbl = new DataTable();
                     ph_tbl.Columns.Add("Area", typeof(string)); //2
                     ph_tbl.Columns.Add("Line", typeof(string)); //2
                     ph_tbl.Columns.Add("Item_Code", typeof(string)); //3
                     ph_tbl.Columns.Add("Part_Number", typeof(string)); //3
                     ph_tbl.Columns.Add("Part_Name", typeof(string)); //4                         
                     ph_tbl.Columns.Add("Unit", typeof(string)); //6                     
                     ph_tbl.Columns.Add("No_of_Manpower_Deployed", typeof(double)); //12

                     if (dtm.Rows.Count > 0)
                     {
                         DataView view1 = new DataView(dtm);
                         DataTable dtdrsim = new DataTable();
                         dtdrsim = view1.ToTable(true, "area"); //MAIN   
                         mq1 = "";
                         foreach (DataRow dr0 in dtdrsim.Rows)
                         {
                             DataView viewim = new DataView(dtm, "area='" + dr0["area"] + "'", "", DataViewRowState.CurrentRows);
                             dt4 = new DataTable();
                             dt4 = viewim.ToTable();
                             dr1 = ph_tbl.NewRow();
                             db = 0; db1 = 0; db2 = 0;
                             for (int i = 0; i < dt4.Rows.Count; i++)
                             {
                                 dr1["Line"] = dt4.Rows[i]["Line"].ToString().Trim();
                                 dr1["Item_Code"] = dt4.Rows[i]["Item_Code"].ToString().Trim();
                                 dr1["Part_Number"] = dt4.Rows[i]["Part_Number"].ToString().Trim();
                                 dr1["Part_Name"] = dt4.Rows[i]["Part_Name"].ToString().Trim();
                                 dr1["Unit"] = dt4.Rows[i]["Unit"].ToString().Trim();
                                 db += fgen.make_double(dt4.Rows[i]["No_of_Manpower_Deployed"].ToString().Trim());
                                 dr1["No_of_Manpower_Deployed"] = db;
                             }
                             if (dt4.Rows.Count > 0)
                             {
                                 ph_tbl.Rows.Add(dr1);
                             }
                         }
                         dt3 = new DataTable();
                         dt3 = ph_tbl.Copy();
                         if (dt3.Rows.Count > 0)
                         {
                             dt3.Columns.Remove("Line");
                             dt3.Columns.Remove("Item_Code");
                             dt3.Columns.Remove("Part_Number");
                             dt3.Columns.Remove("Part_Name");
                             dt3.Columns.Remove("Unit");
                         }
                         fgen.Fn_FillChart(frm_cocd, frm_qstr, "Area With Man Power", "line", "", "", dt3, "");
                     }
                 }
                #endregion
            }
            else if (mq0.Contains("Vulcanisation"))
            {
                if (mq0.Contains("Vulcanisation_ManPower_Req"))
                {
                    dt2 = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
                        dt2 = (DataTable)ViewState["sg1"];
                        dt3 = new DataTable();
                        string[] months = hf1.Value.Split(',');
                        int i = 0;
                        dt3.Columns.Add("Name", typeof(string));
                        dt3.Columns.Add("Vulcanisation", typeof(double));
                        dt3.Columns.Add("Code", typeof(string));

                        for (i = 0; i < months.Length; i++)
                        {
                            sg1_dr = dt3.NewRow();
                            sg1_dr["Code"] = months[i].Replace("'", "");
                            sg1_dr["Vulcanisation"] = dt2.Rows[0]["Vulcanisation_ManPower_Req" + months[i].Replace("'", "_")].ToString();
                            sg1_dr["Name"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + months[i].ToString().Trim().Replace("'", "") + "'", "MTHNAME");
                            dt3.Rows.Add(sg1_dr);
                        }
                        fgen.Fn_FillChart(frm_cocd, frm_qstr, "ManPower Planning (Vulcanisation)", "line", "", "", dt3, "");
                    }                                        
                }
                else
                {
                    mq2 = "Vulcanisation Details Of The Item : " + mq4 + " (" + mq3 + ")";
                    SQuery = "select a.vchnum as fstr,a.vchnum,to_char(a.vchdate,'Mon yyyy') as month,a.total,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when a.edt_by='-' then '-' else to_char(a.edt_dt,'dd/mm/yyyy') end) as edt_dt from pschedule a where a.branchcd='" + frm_mbr + "' and a.type='15' and to_char(a.vchdate,'mmyyyy')='" + mq0.Substring(14, 2) + year + "' and a.icode='" + mq3 + "'";
                }
            }
            else if (mq0.Contains("Transfer"))
            {
                if (mq0.Contains("Transfer_ManPower_Req"))
                {
                    dt2 = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
                        dt2 = (DataTable)ViewState["sg1"];
                        dt3 = new DataTable();
                        string[] months = hf1.Value.Split(',');
                        int i = 0;
                        dt3.Columns.Add("Name", typeof(string));
                        dt3.Columns.Add("Transfer", typeof(double));
                        dt3.Columns.Add("Code", typeof(string));

                        for (i = 0; i < months.Length; i++)
                        {
                            sg1_dr = dt3.NewRow();
                            sg1_dr["Code"] = months[i].Replace("'", "");
                            sg1_dr["Transfer"] = dt2.Rows[0]["Transfer_ManPower_Req" + months[i].Replace("'", "_")].ToString();
                            sg1_dr["Name"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MTHNAME FROM MTHS WHERE MTHNUM='" + months[i].ToString().Trim().Replace("'", "") + "'", "MTHNAME");
                            dt3.Rows.Add(sg1_dr);
                        }
                        fgen.Fn_FillChart(frm_cocd, frm_qstr, "ManPower Planning (Transfer)", "line", "", "", dt3, "");
                    }
                }
                else
                {
                    mq2 = "Transfer Details Of The Item : " + mq4 + " (" + mq3 + ")";
                    SQuery = "select a.vchnum as fstr,a.vchnum,to_char(a.vchdate,'Mon yyyy') as month,a.qtyupd,a.target,a.actual,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.edt_by,(case when a.edt_by='-' then '-' else to_char(a.edt_dt,'dd/mm/yyyy') end) as edt_dt from mthlyplan a where branchcd='" + frm_mbr + "' and a.type='10' and to_char(a.vchdate,'mmyyyy')='" + mq0.Substring(9, 2) + year + "' and a.icode='" + mq3 + "'";
                }
            }
            #endregion
        }
        if (SQuery.Length > 1)
        {
           fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "-");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek(mq2, frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
}