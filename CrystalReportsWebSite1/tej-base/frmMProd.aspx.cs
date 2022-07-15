using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class frmMProd : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3; DataRow oporow; DataSet oDS; int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable dtCol = new DataTable();
    fgenDB fgen = new fgenDB();
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vty2, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, frmTabItemOSP = "ITEMOSP";
    string frm_tabname, frm_tabname2, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, mhd, btchno, rcode;
    double double_val2, double_val1, double_val3;

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
                setColHeading();
            }
            set_Val();
        }
    }
    //------------------------------------------------------------------------------------
    void setColHeading()
    {
        dtCol = new DataTable();
        //dtCol = fgen.getdata(frm_cocd, "SELECT * FROM FORM_CFG WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        ViewState["dtCol"] = dtCol;
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnmachine.Enabled = false; btnshift.Enabled = false; btnstage.Enabled = false;
        create_tab(); sg1_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnmachine.Enabled = true; btnshift.Enabled = true; btnstage.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        switch (frm_formID)
        {
            case "750201":
                lblheader.Text = "Moulding Production";
                frm_tabname2 = "PROD_SHEET"; frm_vty2 = "90";
                frm_tabname = "IVOUCHER"; frm_vty = "15";
                txtstage.Text = "61"; txtstagename.Text = "Moulding";
                txtActualCavity.Attributes.Add("readonly", "readonly");
                txtTargetShot.Attributes.Add("readonly", "readonly");
                txtOkProd.Attributes.Add("readonly", "readonly");
                txtTotProd.Attributes.Add("readonly", "readonly");
                txtTotRej.Attributes.Add("readonly", "readonly");
                txtShotHrs.Attributes.Add("readonly", "readonly");
                btnstage.Visible = false;
                break;
        }
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        btnval = hffield.Value;
        switch (btnval)
        {
            case "sg1_Row_Add":
            case "sg1_Row_Add_E":
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ALTBOM");
                if (col1 == "Y") frmTabItemOSP = "ITEMOSP";
                else frmTabItemOSP = "ITEMOSP2";
                SQuery = "SELECT A.ICODE,B.INAME AS RAW_MAT,B.CPARTNO AS PARTNO,B.UNIT,A.BTCHNO AS BATCH_NO,sum(A.QTY) AS BAL,a.icode as code FROM (SELECT TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,SUM(IQTYOUT) AS QTY FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND SUBSTR(TYPE,1,1) IN ('3','1') AND TYPE!='39' AND STORE='Y' GROUP BY TRIM(ICODE),TRIM(BTCHNO) UNION ALL SELECT TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,SUM(-1*iqtyout) AS QTY FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='39' AND STORE='W' GROUP BY TRIM(ICODE),TRIM(BTCHNO) ) A,ITEM B," + frmTabItemOSP + " C WHERE TRIM(a.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.IBCODe) AND TRIM(C.ICODE)='" + txticode.Text.Trim() + "' group by a.icode,b.iname,b.cpartno,b.unit,a.btchno having sum(a.qty)>0 ORDER BY B.INAME ";
                break;
            case "MCH":
                SQuery = "SELECT TRIM(MCHCODE) AS FSTR,MCHNAME AS MACHINE_NAME,MCHCODE AS MACHINE_CODE,ACODE FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' ORDER BY MCHNAME";
                break;
            case "SHIFT":
                SQuery = "SELECT TRIM(TYPE1) AS FSTR,NAME AS SHIFT,TYPE1 AS CODE,place as shft_min,round(case when place>0 then place/60 else 0 end) as shft_hrs FROM TYPE WHERE ID='D' AND TYPE1 LIKE '1%' ORDER BY code";
                break;
            case "STG":
                SQuery = "SELECT TRIM(TYPE1) AS FSTR,NAME AS STAGE,TYPE1 AS CODE FROM TYPE WHERE ID='1' AND TYPE1 LIKE '6%' ORDER BY NAME";
                break;
            case "TICODE":
                SQuery = "SELECT TRIM(A.ICODE) AS FSTR,B.INAME AS INAME,B.CPARTNO,B.UNIT,TRIM(A.ICODE)||'-'||TRIM(A.STATIONNO) AS CODE,C.INAME AS MLD_NAME,A.CAVITY,A.SHOTS_DAY AS SHOTS FROM MACHMST A,ITEM B,ITEM C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.STATIONNO)=TRIM(c.ICODe) AND A.BRANCHCD = '" + frm_mbr + "' AND A.MCHNUM='" + txtmchcode.Text.Trim() + "' ORDER BY B.INAME";
                break;
            case "OPNAME":
                SQuery = "SELECT DISTINCT EXC_TIME AS FSTR,EXC_TIME AS OPERATOR,'-' AS S FROM PROD_SHEET ORDER BY EXC_TIME";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,b.iname as product,b.cpartno as partno,b.unit,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(a.icodE)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // frm_vnum = fgen.next_no(frm_qstr,frm_cocd,frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname2 + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty2 + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname2 + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty2 + "' AND VCHDATE " + DateRange + "", 6, "vch");  // changed by akshay
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);
            btnmachine.Focus();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");

        dt3 = new DataTable();
        dt3 = fgen.getdata(frm_qstr, frm_cocd, "select NAME as h2_sg2,TYPE1 as h1_sg2,'-' as Obsv1 FROM TYPEWIP WHERE branchcd='" + frm_mbr + "' and ID='DTC" + txtstage.Text + "' ORDER BY TYPE1");
        sg2.DataSource = dt3;
        sg2.DataBind();

        dt3 = new DataTable();
        dt3 = fgen.getdata(frm_qstr, frm_cocd, "select NAME as h2_sg3,TYPE1 as h1_sg3,0 as SCRP1 FROM TYPEWIP WHERE branchcd='" + frm_mbr + "' and ID='RJC" + txtstage.Text + "' ORDER BY TYPE1");
        SG3.DataSource = dt3;
        SG3.DataBind();
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in this form!!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only"); txtvchdate.Focus(); return; }


        if (fgen.make_double(txtOkProd.Text) <= 0)
        {
            double_val3 = 0;
            foreach (GridViewRow gr in sg2.Rows)
            {
                double_val3 += fgen.make_double(((TextBox)gr.FindControl("tkObsv1")).Text);
            }
            if (double_val3 <= 0)
            {
                fgen.msg("-", "AMSG", "Downtime not entered !!");
                return;
            }

            //fgen.msg("-", "AMSG", "Entered Wrong Value SomeWhere, Please Check the Entry and Re-Save!!");
            fgen.msg("-", "SMSG", "Are you sure, you want to Save!!'13'Production Entry not done.");
            return;
        }

        // Grid Batch Qty Check
        i = 0;
        foreach (GridViewRow gr in sg1.Rows)
        {
            dt = new DataTable();
            SQuery = "SELECT TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,SUM(IQTYOUT-IQTYIN) AS QTY FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND SUBSTR(TYPE,1,1) IN ('3','1') and ICODE='" + gr.Cells[3].Text.Trim() + "' and btchno='" + gr.Cells[7].Text.Trim() + "' GROUP BY TRIM(ICODE),TRIM(BTCHNO)";
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (i == 0 && fgen.make_double(txtLump.Text.Trim()) > 0) double_val1 = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text) + fgen.make_double(txtLump.Text.Trim());
            else double_val1 = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text);
            foreach (DataRow dr in dt.Rows)
            {
                ((TextBox)gr.FindControl("sg1_t3")).Text = dr["qtY"].ToString().Trim();
                double_val2 = fgen.make_double(dr["qtY"].ToString().Trim());

                if (double_val1 > double_val2)
                {
                    ((TextBox)gr.FindControl("sg1_t1")).BorderColor = System.Drawing.Color.Red;
                    if (i == 0 && fgen.make_double(txtLump.Text.Trim()) > 0)
                    {
                        fgen.msg("-", "AMSG", "Selected Qty+Lump is More then Batch Qty!!'13'Batch Qty: " + double_val2 + "'13'Total Consumption: " + double_val1 + "'13'Difference : " + (double_val2 - double_val1) + "");
                    }
                    else fgen.msg("-", "AMSG", "Selected Qty is More then Batch Qty!!'13'Row No. " + gr.Cells[2].Text.Trim());
                    return;
                }
            }
            i++;
        }
        // BOM Qty Check
        //col1 = fgen.Fn_Get_Mvar(frm_qstr, "U_ALTBOM");
        //if (col1 == "Y") frmTabItemOSP = "ITEMOSP";
        //else frmTabItemOSP = "ITEMOSP1";
        //dt = new DataTable();
        //SQuery = "SELECT A.IBCODE,B.INAME,b.cpartno,b.unit,A.IBQTY+A.IBWT AS IBQTY FROM " + frmTabItemOSP + " A,ITEM B WHERE TRIM(A.IBCODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)='" + txticode.Text.Trim() + "' ORDER BY A.IBCODE";
        //dt = fgen.getdata(frm_cocd, SQuery);
        //foreach (DataRow dr in dt.Rows)
        //{
        //    double_val1 = 0;
        //    double_val2 = 0;
        //    double_val1 = fgen.make_double(fgen.make_double(fgen.seek_iname_dt(dt, "ibcode='" + dr["ibcode"].ToString().Trim() + "'", "ibqty")) * fgen.make_double(txtTotProd.Text.Trim()), 5);
        //    foreach (GridViewRow gr in sg1.Rows)
        //    {
        //        if (dr["ibcode"].ToString().Trim() == gr.Cells[3].Text.Trim())
        //            double_val2 += fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text);
        //    }
        //    double_val2 = fgen.make_double(double_val2, 5);
        //    if (double_val2 != double_val1)
        //    {
        //        foreach (GridViewRow gr in sg1.Rows)
        //        {
        //            if (gr.Cells[3].Text.Trim() == dr["ibcode"].ToString().Trim()) ((TextBox)gr.FindControl("sg1_t1")).BorderColor = System.Drawing.Color.Red;
        //        }
        //        fgen.msg("-", "AMSG", "Using Qty more/less then Its BOM Qty!!'13'Bom Qty = " + double_val1 + "'13'Qty Using = " + double_val2 + "");
        //        return;
        //    }
        //}

        double_val1 = fgen.make_double(fgen.make_double(txtTotProd.Text.Trim()) * fgen.make_double(txtNetWt.Text.Trim()), 6);
        double_val2 = fgen.make_double(fgen.make_double(txtRRPerPcs.Text) * fgen.make_double(txtActualCavity.Text.Trim()) * fgen.make_double(txtactshot.Text.Trim()), 6);
        double_val3 = 0;
        foreach (GridViewRow gr in sg1.Rows)
        {
            double_val3 += fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text);
        }
        if (fgen.make_double(double_val3, 4) != fgen.make_double(double_val1 + double_val2, 4))
        {
            fgen.msg("-", "AMSG", "Using Qty more/less then Its BOM Qty!!'13'Bom Qty = " + (double_val1 + double_val2) + "'13'Qty Using = " + double_val3 + "'13'Saving not Allowed ");
            return;
        }


        double_val1 = 0; double_val2 = 0; double_val3 = 0;
        double_val1 = Math.Round((fgen.make_double(txtactshot.Text) / fgen.make_double(txtShotHrs.Text)) * 60);
        double_val2 = Math.Round(fgen.make_double(txtWorkingHrs.Text.Split('.')[0]) * 60) + fgen.make_double(txtWorkingHrs.Text.Split('.')[1]);
        foreach (GridViewRow gr in sg2.Rows)
        {
            double_val3 += fgen.make_double(((TextBox)gr.FindControl("tkObsv1")).Text);
        }
        if (double_val3 != fgen.make_double(double_val2 - double_val1, 4))
        {
            fgen.msg("-", "AMSG", "Production Minutes: " + double_val2 + "'13'Working Done: " + double_val1 + "'13'Downtime Shown: " + double_val3 + "'13'Difference: " + (Math.Round(double_val2 - double_val1) - double_val3) + "");
            return;
        }
        if (fgen.make_double(txtSampQty.Text.Trim()) > fgen.make_double(txtOkProd.Text.Trim()))
        {
            fgen.msg("-", "AMSG", "Sample Qty Can not be greater then OK Production!!");
            return;
        }
        if (txtbatchno.Text.Trim().Length <= 1)
        {
            fgen.msg("-", "AMSG", "Please Add Batch No.!!");
            return;
        }
        if (fgen.make_double(txtLump.Text.Trim()) > 0)
        {
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(SRC1) AS SRC1 FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,4) = '" + txticode.Text.Trim().Substring(0, 4) + "' ", "SRC1");
            if (mhd == "0" && mhd.Length > 2)
            {
                fgen.msg("-", "AMSG", "Lump not linked in Product Sub Group Code!!");
                return;
            }
        }
        fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl(); set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to delete data in this form");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        //Response.Redirect("~/desktop.aspx?STR=" + frm_qstr);
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
        create_tab(); sg1_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value; set_Val();
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + edmode.Value + "'");

                // Deleing data from Main Table2
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname2 + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty2 + edmode.Value + "'");
                // Deleing data from Main Table / another type
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "39" + edmode.Value + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "3A" + edmode.Value + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, edmode.Value.Substring(0, 5), edmode.Value.Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Details are deleted for " + lblheader.Text + " No." + edmode.Value.Substring(0, 5) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "MCH":
                    btnmachine.Focus();
                    if (col1 == "") return;
                    txtmchcode.Text = col1;
                    txtmchname.Text = col2;
                    SQuery = "SELECT revis_no as val FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and to_char(vchdate,'dd/mm/yyyy')=TO_CHAR(TO_DATE('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'dd/mm/yyyy') and trim(acode)='" + txtmchcode.Text.Trim() + "' order by vchnum desc,vchdate desc";
                    if (txtshiftcode.Text.Trim().Length > 0) SQuery = "SELECT revis_no as val FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and to_char(vchdate,'dd/mm/yyyy')=TO_CHAR(TO_DATE('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'dd/mm/yyyy') and trim(acode)='" + txtmchcode.Text.Trim() + "' and upper(trim(O_DEPTT))='" + txtshiftcode.Text.Trim().ToUpper() + "' order by vchnum desc,vchdate desc";
                    txtTimeIn.Text = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "val");
                    if (txtTimeIn.Text.Length > 1) txtTimeIn.Attributes.Add("readonly", "readonly");
                    else txtTimeIn.Attributes.Remove("readonly");
                    if (txtTimeIn.Text.Trim().Length > 1)
                    {
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT count(revis_no) as val FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and to_char(vchdate,'dd/mm/yyyy')=TO_CHAR(TO_DATE('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'dd/mm/yyyy') and trim(acode)='" + txtmchcode.Text.Trim() + "' and trim(revis_no)='" + txtTimeIn.Text + "'", "val");
                        if (fgen.make_double(mhd) > 1)
                        {
                            fgen.msg("-", "AMSG", "Entry Not Allowed, This time is already entered!!");
                            return;
                        }
                    }

                    if (edmode.Value == "Y") clearData();
                    btnshift.Focus();
                    break;
                case "SHIFT":
                    btnshift.Focus();
                    if (col1 == "") return;
                    txtshiftcode.Text = col1;
                    txtshiftname.Text = col2;
                    txtWorkingHrs.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                    btnicode.Focus();
                    break;
                case "STG":
                    btnstage.Focus();
                    if (col1 == "") return;
                    txtstage.Text = col1;
                    txtstagename.Text = col2;
                    //z = sg1.Rows.Count;
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    break;
                case "Del":
                    if (col1 == "") return;
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Edit":
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.iname,B.CPARTNO AS RCPARTNO,B.UNIT AS RUNIT,c.iname AS RINAME,c.cpartno,c.unit as unit1 from " + frm_tabname + " a,item b,item c where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(c.icode) and a.branchcd||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + col1 + "' and a.type in ('" + frm_vty + "','39') and a.stage='61' order by a.srno ";
                    ViewState["fstr"] = col1;

                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, "select NAME as h2_sg2,TYPE1 as h1_sg2,'-' as Obsv1 FROM TYPEWIP WHERE branchcd='" + frm_mbr + "' and ID='DTC" + txtstage.Text + "' ORDER BY TYPE1");
                    sg2.DataSource = dt3;
                    sg2.DataBind();

                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, "select NAME as h2_sg3,TYPE1 as h1_sg3,0 as SCRP1 FROM TYPEWIP WHERE branchcd='" + frm_mbr + "' and ID='RJC" + txtstage.Text + "' ORDER BY TYPE1");
                    SG3.DataSource = dt3;
                    SG3.DataBind();

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txticode.Text = fgen.seek_iname_dt(dt, "type='15'", "icode");
                        txtiname.Text = fgen.seek_iname_dt(dt, "type='15'", "iname");

                        txtNetWt.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iweight from item where trim(icodE)='" + txticode.Text.Trim() + "'", "iweight");
                        txtRRPerPcs.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select ioqty from itemosp where trim(icodE)='" + txticode.Text.Trim() + "'", "ioqty");

                        txtmicode.Text = fgen.seek_iname_dt(dt, "type='15'", "rcode");
                        txtminame.Text = fgen.seek_iname_dt(dt, "type='15'", "riname");

                        txtmchcode.Text = fgen.seek_iname_dt(dt, "type='15'", "acode");
                        txtmchname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(MCHCODE) AS FSTR,MCHNAME AS MACHINE_NAME,MCHCODE AS MACHINE_CODE,ACODE FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND TRIM(MCHCODE)='" + txtmchcode.Text.Trim() + "'", "MACHINE_NAME");

                        txtshiftcode.Text = fgen.seek_iname_dt(dt, "type='15'", "O_DEPTT");
                        txtshiftname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='D' AND TYPE1='" + txtshiftcode.Text.Trim() + "'", "NAME");

                        txtstage.Text = fgen.seek_iname_dt(dt, "type='15'", "stage");
                        txtstagename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='1' AND TYPE1='" + txtstage.Text.Trim() + "'", "NAME");

                        txtTimeIn.Text = fgen.seek_iname_dt(dt, "type='15'", "mtime");
                        txtTimeOut.Text = fgen.seek_iname_dt(dt, "type='15'", "revis_no");
                        txtWorkingHrs.Text = fgen.seek_iname_dt(dt, "type='15'", "et_topay");

                        txtbatchno.Text = fgen.seek_iname_dt(dt, "type='15'", "btchno");
                        txtActualCavity.Text = fgen.seek_iname_dt(dt, "type='15'", "ipack");
                        txtRunCavity.Text = fgen.seek_iname_dt(dt, "type='15'", "cavity");

                        txtTargetShot.Text = fgen.seek_iname_dt(dt, "type='15'", "rlprc");
                        txtactshot.Text = fgen.seek_iname_dt(dt, "type='15'", "shots");
                        txtShotHrs.Text = fgen.seek_iname_dt(dt, "type='15'", "segment_");

                        txtOkProd.Text = fgen.seek_iname_dt(dt, "type='15'", "iqtyin");
                        txtTotRej.Text = fgen.seek_iname_dt(dt, "type='15'", "rej_rw");
                        txtLump.Text = fgen.seek_iname_dt(dt, "type='15'", "rej_sdp");
                        txtSampQty.Text = fgen.seek_iname_dt(dt, "type='15'", "rej_sdv");

                        txtoprtr.Text = fgen.seek_iname_dt(dt, "type='15'", "pname");

                        create_tab();
                        sg1_dr = null;
                        DataView dv = new DataView(dt, "type='39' AND naration <> 'LUMPS'", "srno", DataViewRowState.CurrentRows);
                        for (i = 0; i < dv.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = dv[i].Row["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dv[i].Row["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dv[i].Row["RCPARTNO"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dv[i].Row["RUNIT"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dv[i].Row["btchno"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dv[i].Row["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dv[i].Row["naration"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dv[i].Row["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgenMV.Fn_Set_Mvar(frm_qstr, "MEDTBY", dt.Rows[0]["ent_by"].ToString());
                        fgenMV.Fn_Set_Mvar(frm_qstr, "MEDTDT", dt.Rows[0]["ent_dt"].ToString());
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        edmode.Value = "Y";

                        foreach (GridViewRow gr in SG3.Rows)
                        {
                            mhd = fgen.seek_iname_dt(dt, "mode_tpt='" + gr.Cells[0].Text.Trim() + "' and store='R'", "iqtyin");
                            if (mhd != "0") ((TextBox)gr.FindControl("tkSCRP1")).Text = mhd;
                        }

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select * from inspvch where branchcd='" + frm_mbr + "' and type='55' and trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')='" + col1 + "'");

                        foreach (GridViewRow gr in sg2.Rows)
                        {
                            mhd = fgen.seek_iname_dt(dt, "COL1='" + gr.Cells[0].Text.Trim() + "'", "col3");
                            if (mhd != "0") ((TextBox)gr.FindControl("tkObsv1")).Text = mhd;
                        }
                    }
                    #endregion
                    break;
                case "Print":
                    if (col1 == "") return;
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + col1 + ") order by A.vchnum ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "XML File Name", "RPT File Name");
                    break;
                case "TMSG":
                    col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    if (col1 == "Y")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ALTBOM", "Y");
                        chk_bom("ITEMOSP");
                    }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ALTBOM", "N");
                        chk_bom("ITEMOSP2");
                    }
                    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
                    txticode.Text = col1;
                    txtiname.Text = col2;
                    txtmicode.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").Split('-')[1].ToString().Trim();
                    txtminame.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                    txtActualCavity.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");
                    txtTargetShot.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
                    txtShotHrs.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
                    txtRunCavity.Text = txtActualCavity.Text;
                    txtNetWt.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iweight from item where trim(icodE)='" + txticode.Text.Trim() + "'", "iweight");
                    txtRRPerPcs.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select ioqty from itemosp where trim(icodE)='" + txticode.Text.Trim() + "'", "ioqty");
                    if (edmode.Value == "Y") clearData();
                    txtTimeIn.Focus();
                    break;
                case "sg1_Row_Add":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_f1"] = col1;
                        sg1_dr["sg1_f2"] = col2;
                        sg1_dr["sg1_f3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                        sg1_dr["sg1_f4"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        sg1_dr["sg1_f5"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                        sg1_dr["sg1_t1"] = "0";
                        sg1_dr["sg1_t2"] = "";
                        sg1_dr["sg1_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                        sg1_dr["sg1_t4"] = "";
                        sg1_dr["sg1_t5"] = "";
                        sg1_dr["sg1_t6"] = "";
                        sg1_dr["sg1_t7"] = "";
                        sg1_dr["sg1_t8"] = "";

                        sg1_dt.Rows.Add(sg1_dr);
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    break;
                case "sg1_Row_Add_E":
                    if (col1.Length <= 0) return;

                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    break;
                case "sg1_Rmv":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        i = 0;
                        for (i = 0; i < sg1.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = (i + 1);
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[7].Text.Trim();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }
                    #endregion
                    break;
                case "OPNAME":
                    txtoprtr.Text = col1;
                    txtoprtr.Focus();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "List")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            // frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");            
            // frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");   by akshay 
            frm_vty = "15";
            frm_tabname = "IVOUCHER";
            SQuery = "select a.* from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by a.vchnum";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
        }
        else
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                try
                {
                    oDS = new DataSet(); oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();

                    oDS.Dispose(); oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    if (edmode.Value == "Y") frm_vnum = txtvchnum.Text.Trim();
                    else
                    {
                        i = 0;
                        do
                        {
                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from " + frm_tabname2 + " where branchcd='" + frm_mbr + "' and type='" + frm_vty2 + "' and vchdate " + DateRange + "", 6, "vch");
                            pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname2.ToUpper() + frm_mbr + frm_vty2 + frm_vnum + frm_CDT1, frm_mbr, frm_vty2, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                            if (i > 100)
                            {
                                fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                frm_vnum = "000002";
                                pk_error = "N";
                            }
                            i++;
                        }
                        while (pk_error == "Y");
                    }

                    // If Vchnum becomes 000000 then Re-Save
                    if (frm_vnum == "000000") btnhideF_Click(sender, e);
                    oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    save_fun();

                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + ViewState["fstr"].ToString().Trim() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "39" + ViewState["fstr"].ToString().Trim() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "3A" + ViewState["fstr"].ToString().Trim() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname2 + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty2 + ViewState["fstr"].ToString().Trim() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update INSPVCH set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "55" + ViewState["fstr"].ToString().Trim() + "'");
                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    // Inserting in Prod_Sheet
                    oDS.Dispose(); oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);
                    save_fun2();
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname2);

                    save_sg2_sg3();

                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + ViewState["fstr"].ToString() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD39" + ViewState["fstr"].ToString() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD3A" + ViewState["fstr"].ToString() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname2 + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty2 + ViewState["fstr"].ToString() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from INSPVCH where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD55" + ViewState["fstr"].ToString() + "'");
                        fgen.msg("-", "AMSG", "Data Updated Successfully");
                    }
                    else { fgen.msg("-", "AMSG", "Data Saved Successfully"); }
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                }
                catch (Exception ex)
                {
                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        i = 0;
        #region Entry in 15, ivoucher
        btchno = txtbatchno.Text.Trim();
        rcode = txticode.Text.Trim();

        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = txtvchdate.Text.Trim();
        oporow["invno"] = "-";
        oporow["invdate"] = vardate;
        oporow["acode"] = txtmchcode.Text.Trim();

        oporow["icode"] = txticode.Text.Trim();
        oporow["Rcode"] = txtmicode.Text.Trim();

        oporow["srno"] = (i + 1);
        oporow["morder"] = (i + 1);

        oporow["IQTYOUT"] = "0";
        oporow["IQTYIN"] = fgen.make_double(txtOkProd.Text.Trim());
        oporow["STORE"] = "W";
        oporow["rec_iss"] = "D";

        oporow["ORDLINENO"] = txtstagename.Text.Trim();
        oporow["STAGE"] = txtstage.Text.Trim();

        oporow["O_DEPTT"] = txtshiftcode.Text.Trim();
        oporow["BTCHNO"] = btchno;

        oporow["IPACK"] = txtActualCavity.Text.Trim();
        oporow["CAVITY"] = txtRunCavity.Text.Trim();

        oporow["mtime"] = txtTimeIn.Text.Trim();
        oporow["REVIS_NO"] = txtTimeOut.Text.Trim();

        oporow["ET_TOPAY"] = txtWorkingHrs.Text.Trim();

        oporow["RLPRC"] = txtTargetShot.Text.Trim();
        oporow["SHOTS"] = txtactshot.Text.Trim();
        oporow["SEGMENT_"] = txtShotHrs.Text.Trim();

        oporow["desc_"] = txtrmk.Text.Trim();

        oporow["rej_rw"] = fgen.make_double(txtTotRej.Text.Trim());
        oporow["rej_sdp"] = fgen.make_double(txtLump.Text.Trim());
        oporow["rej_sdv"] = fgen.make_double(txtSampQty.Text.Trim());

        oporow["pname"] = txtoprtr.Text.Trim().ToUpper();

        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
            oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
        #endregion
        #region Consuming Raw Material from 39,ivoucher
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = "39";
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["invno"] = "-";
            oporow["invdate"] = vardate;
            oporow["acode"] = txtmchcode.Text.Trim();

            oporow["icode"] = sg1.Rows[i].Cells[3].Text.Trim();
            oporow["Rcode"] = txticode.Text.Trim();

            oporow["srno"] = i;
            oporow["morder"] = i;

            oporow["IQTYOUT"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
            oporow["iqty_chl"] = double_val1;

            oporow["IQTYIN"] = "0";
            oporow["STORE"] = "W";
            oporow["rec_iss"] = "C";

            oporow["btchno"] = sg1.Rows[i].Cells[7].Text.Trim();

            oporow["ORDLINENO"] = txtstagename.Text.Trim();
            oporow["STAGE"] = txtstage.Text.Trim();

            oporow["O_DEPTT"] = txtshiftcode.Text.Trim();

            oporow["IPACK"] = txtActualCavity.Text.Trim();
            oporow["CAVITY"] = txtRunCavity.Text.Trim();

            oporow["SEGMENT_"] = txtShotHrs.Text.Trim();
            oporow["mtime"] = txtTimeIn.Text.Trim();
            oporow["REVIS_NO"] = txtTimeOut.Text.Trim();
            oporow["ET_TOPAY"] = txtWorkingHrs.Text.Trim();

            oporow["RLPRC"] = txtTargetShot.Text.Trim();
            oporow["SHOTS"] = txtactshot.Text.Trim();

            oporow["desc_"] = txtrmk.Text.Trim();
            oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;

            oporow["rej_rw"] = fgen.make_double(txtTotRej.Text.Trim());
            oporow["rej_sdp"] = fgen.make_double(txtLump.Text.Trim());
            oporow["rej_sdv"] = fgen.make_double(txtSampQty.Text.Trim());

            oporow["pname"] = txtoprtr.Text.Trim().ToUpper();

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
            }
            else
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["eDt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);

            if (i == 0 && fgen.make_double(txtLump.Text) > 0)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "39";
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["invno"] = "-";
                oporow["invdate"] = vardate;
                oporow["acode"] = txtmchcode.Text.Trim();

                oporow["icode"] = sg1.Rows[i].Cells[3].Text.Trim();
                oporow["Rcode"] = txticode.Text.Trim();

                oporow["srno"] = i;
                oporow["morder"] = i;

                oporow["IQTYOUT"] = txtLump.Text.Trim();
                oporow["iqty_chl"] = double_val1;

                oporow["IQTYIN"] = "0";
                oporow["STORE"] = "W";
                oporow["rec_iss"] = "C";

                oporow["btchno"] = sg1.Rows[i].Cells[7].Text.Trim();

                oporow["ORDLINENO"] = txtstagename.Text.Trim();
                oporow["STAGE"] = txtstage.Text.Trim();

                oporow["O_DEPTT"] = txtshiftcode.Text.Trim();

                oporow["IPACK"] = txtActualCavity.Text.Trim();
                oporow["CAVITY"] = txtRunCavity.Text.Trim();

                oporow["SEGMENT_"] = txtShotHrs.Text.Trim();
                oporow["mtime"] = txtTimeIn.Text.Trim();
                oporow["REVIS_NO"] = txtTimeOut.Text.Trim();
                oporow["ET_TOPAY"] = txtWorkingHrs.Text.Trim();

                oporow["RLPRC"] = txtTargetShot.Text.Trim();
                oporow["SHOTS"] = txtactshot.Text.Trim();

                oporow["desc_"] = txtrmk.Text.Trim();
                oporow["naration"] = "LUMPS";

                oporow["rej_rw"] = fgen.make_double(txtTotRej.Text.Trim());
                oporow["rej_sdp"] = fgen.make_double(txtLump.Text.Trim());
                oporow["rej_sdv"] = fgen.make_double(txtSampQty.Text.Trim());

                oporow["pname"] = txtoprtr.Text.Trim().ToUpper();

                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                    oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["eDt_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
            }

        }
        #endregion
        #region Lumps A
        if (fgen.make_double(txtLump.Text.Trim()) > 0)
        {
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(SRC1) AS SRC1 FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,4) = '" + txticode.Text.Trim().Substring(0, 4) + "' ", "SRC1");
            if (mhd != "0" && mhd.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "15";
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["Rcode"] = txticode.Text.Trim();
                oporow["icode"] = mhd;

                oporow["srno"] = 1;
                oporow["BTCHNO"] = txtbatchno.Text.Trim();

                oporow["iqtyOUT"] = 0;
                oporow["IQTYIN"] = fgen.make_double(txtLump.Text);
                oporow["STORE"] = "R";
                oporow["STAGE"] = txtstage.Text.Trim();

                oporow["rej_rw"] = fgen.make_double(txtTotRej.Text.Trim());
                oporow["rej_sdp"] = fgen.make_double(txtLump.Text.Trim());
                oporow["rej_sdv"] = fgen.make_double(txtSampQty.Text.Trim());
                oporow["pname"] = txtoprtr.Text.Trim().ToUpper();


                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                    oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["eDt_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
        #endregion

        #region Sample qty tfr
        if (fgen.make_double(txtSampQty.Text.Trim()) > 0)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = "3A";
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["invno"] = "-";
            oporow["invdate"] = vardate;
            oporow["acode"] = txtmchcode.Text.Trim();

            oporow["icode"] = txticode.Text.Trim();
            oporow["Rcode"] = txtmicode.Text.Trim();

            oporow["srno"] = (i + 1);
            oporow["morder"] = (i + 1);

            oporow["IQTYOUT"] = "0";
            oporow["IQTYIN"] = fgen.make_double(txtSampQty.Text.Trim());
            oporow["STORE"] = "W";
            oporow["rec_iss"] = "D";

            oporow["ORDLINENO"] = txtstagename.Text.Trim();
            oporow["STAGE"] = "6A";

            oporow["O_DEPTT"] = txtshiftcode.Text.Trim();
            oporow["BTCHNO"] = btchno;

            oporow["IPACK"] = txtActualCavity.Text.Trim();
            oporow["CAVITY"] = txtRunCavity.Text.Trim();

            oporow["SEGMENT_"] = txtShotHrs.Text.Trim();
            oporow["mtime"] = txtTimeIn.Text.Trim();
            oporow["REVIS_NO"] = txtTimeOut.Text.Trim();
            oporow["ET_TOPAY"] = txtWorkingHrs.Text.Trim();

            oporow["RLPRC"] = txtTargetShot.Text.Trim();
            oporow["SHOTS"] = txtactshot.Text.Trim();

            oporow["desc_"] = txtrmk.Text.Trim();

            oporow["rej_rw"] = fgen.make_double(txtTotRej.Text.Trim());
            oporow["rej_sdp"] = fgen.make_double(txtLump.Text.Trim());
            oporow["rej_sdv"] = fgen.make_double(txtSampQty.Text.Trim());

            oporow["pname"] = txtoprtr.Text.Trim().ToUpper();

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
            }
            else
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["eDt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);

            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = "3A";
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["invno"] = "-";
            oporow["invdate"] = vardate;
            oporow["acode"] = txtmchcode.Text.Trim();

            oporow["icode"] = txticode.Text.Trim();
            oporow["Rcode"] = txtmicode.Text.Trim();

            oporow["srno"] = (i + 1);
            oporow["morder"] = (i + 1);

            oporow["IQTYOUT"] = fgen.make_double(txtSampQty.Text);
            oporow["IQTYIN"] = "0";
            oporow["STORE"] = "W";
            oporow["rec_iss"] = "D";

            oporow["ORDLINENO"] = txtstagename.Text.Trim();
            oporow["STAGE"] = txtstage.Text.Trim();

            oporow["O_DEPTT"] = txtshiftcode.Text.Trim();
            oporow["BTCHNO"] = btchno;

            oporow["IPACK"] = txtActualCavity.Text.Trim();
            oporow["CAVITY"] = txtRunCavity.Text.Trim();

            oporow["SEGMENT_"] = txtShotHrs.Text.Trim();
            oporow["mtime"] = txtTimeIn.Text.Trim();
            oporow["REVIS_NO"] = txtTimeOut.Text.Trim();
            oporow["ET_TOPAY"] = txtWorkingHrs.Text.Trim();

            oporow["RLPRC"] = txtTargetShot.Text.Trim();
            oporow["SHOTS"] = txtactshot.Text.Trim();

            oporow["desc_"] = txtrmk.Text.Trim();
            oporow["rej_rw"] = fgen.make_double(txtTotRej.Text.Trim());
            oporow["rej_sdp"] = fgen.make_double(txtLump.Text.Trim());
            oporow["rej_sdv"] = fgen.make_double(txtSampQty.Text.Trim());

            oporow["pname"] = txtoprtr.Text.Trim().ToUpper();

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
            }
            else
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["eDt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
        #endregion
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        #region Entry in 90, prod_sheet
        btchno = txtbatchno.Text.Trim();
        rcode = txticode.Text.Trim();

        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty2;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = txtvchdate.Text.Trim();
        oporow["acode"] = txtmchcode.Text.Trim();

        oporow["icode"] = txticode.Text.Trim();
        oporow["total"] = fgen.make_double(txtWorkingHrs.Text.Trim());
        oporow["un_melt"] = fgen.make_double(txtTargetShot.Text.Trim());
        oporow["mlt_loss"] = fgen.make_double(txtTotRej.Text.Trim());
        oporow["remarks"] = txtmicode.Text.Trim();
        oporow["iqtyin"] = fgen.make_double(txtOkProd.Text.Trim());
        oporow["mchcode"] = txtmchcode.Text.Trim();
        oporow["shftcode"] = txtshiftcode.Text.Trim();
        oporow["noups"] = fgen.make_double(txtactshot.Text.Trim());
        oporow["lmd"] = txtActualCavity.Text.Trim();
        oporow["BCD"] = txtRunCavity.Text.Trim();
        oporow["ename"] = txtminame.Text.Trim();
        oporow["var_code"] = txtshiftname.Text.Trim();
        oporow["glue_code"] = txtbatchno.Text.Trim();
        oporow["TEMPR"] = txtShotHrs.Text.Trim();
        oporow["exc_time"] = txtoprtr.Text.Trim();

        for (int i = 0; i < SG3.Rows.Count; i++)
        {
            if (i < 20)
            {
                oporow["A" + (i + 1)] = fgen.make_double(((TextBox)SG3.Rows[i].FindControl("tkSCRP1")).Text);
            }
            else break;
        }

        for (int i = 0; i < sg2.Rows.Count; i++)
        {
            if (i < 12)
            {
                oporow["NUM" + (i + 1)] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("tkObsv1")).Text);
            }
            else break;
        }

        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            //oporow["edt_by"] = fgen.Fn_Get_Mvar(frm_qstr, "MEDTBY");
            //oporow["edt_dt"] = fgen.Fn_Get_Mvar(frm_qstr, "MEDTDT");
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            //oporow["edt_by"] = "-";
            //oporow["eDt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
        #endregion
    }
    //------------------------------------------------------------------------------------    
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
        sg1_dr["sg1_f1"] = "-";
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";
        sg1_dr["sg1_f5"] = "-";

        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";
        sg1_dr["sg1_t5"] = "-";
        sg1_dr["sg1_t6"] = "-";
        sg1_dr["sg1_t7"] = "-";
        sg1_dr["sg1_t8"] = "-";
        sg1_dr["sg1_t9"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 30)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 30);
                    }
                }
            }
            ((TextBox)e.Row.FindControl("sg1_t1")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t2")).Width = 350;
            ((TextBox)e.Row.FindControl("sg1_t3")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t4")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t5")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t6")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t7")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t8")).Width = 70;
            ((TextBox)e.Row.FindControl("sg1_t9")).Width = 70;
            // for JavaScript Calculation Formula
            ((TextBox)e.Row.FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
            ((TextBox)e.Row.FindControl("sg1_t9")).Attributes.Add("readonly", "readonly");

            //e.Row.Cells[11].Style["display"] = "none";
            //sg1.HeaderRow.Cells[11].Style["display"] = "none";
            e.Row.Cells[12].Style["display"] = "none";
            sg1.HeaderRow.Cells[12].Style["display"] = "none";
            e.Row.Cells[13].Style["display"] = "none";
            sg1.HeaderRow.Cells[13].Style["display"] = "none";
            e.Row.Cells[14].Style["display"] = "none";
            sg1.HeaderRow.Cells[14].Style["display"] = "none";
            e.Row.Cells[15].Style["display"] = "none";
            sg1.HeaderRow.Cells[15].Style["display"] = "none";
            e.Row.Cells[16].Style["display"] = "none";
            sg1.HeaderRow.Cells[16].Style["display"] = "none";

            sg1.HeaderRow.Cells[3].Text = "Item Code";
            sg1.HeaderRow.Cells[4].Width = 100;
            sg1.HeaderRow.Cells[4].Text = "Item Name";
            sg1.HeaderRow.Cells[4].Width = 300;
            sg1.HeaderRow.Cells[5].Text = "Part No.";
            sg1.HeaderRow.Cells[5].Width = 150;
            sg1.HeaderRow.Cells[6].Text = "UOM";
            sg1.HeaderRow.Cells[6].Width = 100;
            sg1.HeaderRow.Cells[7].Text = "Batch No";
            sg1.HeaderRow.Cells[6].Width = 150;
            sg1.HeaderRow.Cells[8].Text = "Qty";
            sg1.HeaderRow.Cells[9].Text = "Remarks";
            sg1.HeaderRow.Cells[10].Text = "Batch.Qty";
            sg1.HeaderRow.Cells[11].Text = "Comp % ";

            //if (dtCol != null)
            //{
            //    setColHeading();
            //}
            //dtCol = new DataTable();
            //dtCol = (DataTable)ViewState["dtCol"];

            //foreach (DataRow drCol in dtCol.Rows)
            //{
            //    for (int sR = 0; sR < sg1.Columns.Count; sR++)
            //    {
            //        if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == drCol["OBJ_NAME"].ToString().Trim().ToUpper())
            //        {
            //            sg1.HeaderRow.Cells[sR].Text = drCol["OBJ_CAPTION"].ToString().Trim();
            //        }
            //    }
            //}
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg1_Rmv":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString(); hffield.Value = "sg1_Rmv";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove this item from list");
                }
                break;
            case "sg1_Row_Add":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "sg1_Row_Add_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                }
                else
                {
                    hffield.Value = "sg1_Row_Add";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnmachine_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MCH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Machine", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnshift_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SHIFT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Shift", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnstage_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STG";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Stage", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnicode_Click(object sender, ImageClickEventArgs e)
    {
        if (txtmchcode.Text.Trim() != "-" && txtmchcode.Text.Length > 0)
        {
            hffield.Value = "TICODE";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Product Code", frm_qstr);
        }
        else
        {
            hffield.Value = "MCH";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Machine", frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
    void chk_bom(string frmItemOSP)
    {
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.IBCODE,B.INAME,b.cpartno,b.unit,A.IBQTY+A.IBWT AS IBQTY FROM " + frmItemOSP + " A,ITEM B WHERE TRIM(A.IBCODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)='" + txticode.Text.Trim() + "' /*AND NVL(TRIM(A.IOMACHINE),'-')='-'*/ ORDER BY A.IBCODE");
        create_tab();
        sg1_dr = null;
        for (i = 0; i < dt.Rows.Count; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
            sg1_dr["sg1_f1"] = dt.Rows[i]["ibcode"].ToString().Trim();
            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
            sg1_dr["sg1_f4"] = dt.Rows[i]["unit"].ToString().Trim();
            sg1_dr["sg1_f5"] = "-";

            //double_val1 = fgen.make_double(txtRunCavity.Text.Trim()) * fgen.make_double(txtNetWt.Text.Trim());
            //double_val2 = fgen.make_double(txtRRPerPcs.Text) * fgen.make_double(txtActualCavity.Text.Trim()) * fgen.make_double(txtactshot.Text.Trim());

            //sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["ibqty"].ToString().Trim()) * fgen.make_double(txtTotProd.Text.Trim());
            sg1_dr["sg1_t1"] = "0";
            sg1_dr["sg1_t2"] = "";
            sg1_dr["sg1_t3"] = "";
            sg1_dr["sg1_t4"] = "";
            sg1_dr["sg1_t5"] = "";
            sg1_dr["sg1_t6"] = "";
            sg1_dr["sg1_t7"] = "";
            sg1_dr["sg1_t8"] = "";

            sg1_dt.Rows.Add(sg1_dr);
        }

        sg1_add_blankrows();
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        dt.Dispose(); sg1_dt.Dispose();
        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
    }
    //------------------------------------------------------------------------------------
    protected void btnoperator_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "OPNAME";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Operator", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_sg2_sg3()
    {
        #region Rejection Entry
        oDS.Dispose(); oporow = null;
        oDS = new DataSet();
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NVL(PARAMS,'-') AS PARAMS FROM CONTROLS WHERE ID='I90'", "PARAMS");
        mhd = txticode.Text.Trim();
        if (mhd != "0")
        {
            i = 0;
            foreach (GridViewRow gr_sg3 in SG3.Rows)
            {
                if (fgen.make_double(((TextBox)gr_sg3.FindControl("tkSCRP1")).Text.Trim().ToUpper()) > 0)
                {
                    i = 2;
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = frm_vnum;
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["invno"] = "-";
                    oporow["invdate"] = vardate;
                    oporow["acode"] = "-";
                    oporow["MODE_TPT"] = gr_sg3.Cells[0].Text.Trim();

                    oporow["icode"] = mhd;
                    oporow["rcode"] = rcode;

                    oporow["srno"] = 50;
                    oporow["morder"] = 50;
                    oporow["BTCHNO"] = btchno;
                    oporow["purpose"] = "";
                    oporow["BINNO"] = "";
                    oporow["location"] = "";
                    oporow["IQTYOUT"] = 0;
                    oporow["IQTYIN"] = fgen.make_double(((TextBox)gr_sg3.FindControl("tkSCRP1")).Text.Trim());
                    // Scrap
                    oporow["rej_rw"] = fgen.make_double(((TextBox)gr_sg3.FindControl("tkSCRP1")).Text.Trim());

                    // Store in WIP
                    oporow["STORE"] = "R";
                    oporow["rec_iss"] = "C";
                    oporow["ORDLINENO"] = txtstage.Text;
                    oporow["DOCSRNO"] = "-";
                    oporow["STAGE"] = txtstage.Text;

                    oporow["O_DEPTT"] = txtshiftcode.Text.Trim();
                    oporow["BTCHNO"] = btchno;

                    oporow["IPACK"] = txtActualCavity.Text.Trim();
                    oporow["CAVITY"] = txtRunCavity.Text.Trim();

                    oporow["SEGMENT_"] = txtShotHrs.Text.Trim();
                    oporow["mtime"] = txtTimeIn.Text.Trim();
                    oporow["REVIS_NO"] = txtTimeOut.Text.Trim();
                    oporow["ET_TOPAY"] = txtWorkingHrs.Text.Trim();

                    oporow["RLPRC"] = txtTargetShot.Text.Trim();
                    oporow["SHOTS"] = txtactshot.Text.Trim();

                    if (edmode.Value == "Y")
                    {
                        oporow["eNt_by"] = frm_uname;
                        oporow["eNt_dt"] = vardate;
                        oporow["edt_by"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                        oporow["edt_dt"] = fgenMV.Fn_Get_Mvar(frm_qstr, "MEDTDT");
                    }
                    else
                    {
                        oporow["eNt_by"] = frm_uname;
                        oporow["eNt_dt"] = vardate;
                        oporow["edt_by"] = "-";
                        oporow["eDt_dt"] = vardate;
                    }
                    oDS.Tables[0].Rows.Add(oporow);
                }
            }
        }
        if (i == 2) fgen.save_data(frm_qstr, frm_cocd, oDS, "IVOUCHER");
        #endregion

        #region down time region

        oDS.Dispose(); oporow = null;
        oDS = new DataSet();
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, "INSPVCH");
        i = 0;
        foreach (GridViewRow gr2 in sg2.Rows)
        {
            if (fgen.make_double(((TextBox)gr2.FindControl("tkObsv1")).Text) > 0)
            {
                i = 2;
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "55";
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text;
                oporow["COL1"] = gr2.Cells[0].Text.Trim();
                oporow["COL2"] = gr2.Cells[1].Text.Trim();
                oporow["COL3"] = ((TextBox)gr2.FindControl("tkObsv1")).Text.Trim().ToUpper();

                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = frm_vnum;
                    oporow["eNt_dt"] = vardate;
                    //oporow["edt_by"] = fgen.Fn_Get_Mvar(frm_qstr, "MEDTBY");
                    //oporow["edt_dt"] = fgen.Fn_Get_Mvar(frm_qstr, "MEDTDT");
                }
                else
                {
                    oporow["eNt_by"] = frm_vnum;
                    oporow["eNt_dt"] = vardate;
                }

                oDS.Tables[0].Rows.Add(oporow);
            }
        }
        if (i == 2) fgen.save_data(frm_qstr, frm_cocd, oDS, "INSPVCH");
        #endregion
    }
    //------------------------------------------------------------------------------------    
    protected void btnConsume_Click(object sender, EventArgs e)
    {
        hffield.Value = "TMSG";
        fgen.msg("-", "CMSG", "Do you want to choose item from Main Bom'13'No for alternate BOM!!");
        //fgen.Fn_Set_Mvar(frm_qstr, "U_ALTBOM", "Y");
        //chk_bom("ITEMOSP");
    }
    void clearData()
    {
        txtTimeIn.Text = "";
        txtTimeOut.Text = "";
        sg1_dt = new DataTable();
        create_tab(); sg1_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
    }
}