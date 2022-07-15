using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;

//  F25374
//RPT_NEW

public partial class om_upload_dashboard : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld;
    int i0, i1, i2, i3, i4, v = 0; DateTime date1, date2; DataSet ds, ds3, oDS;
    DataTable dt, ph_tbl, dt1, dt2, dt3, dt4, dt5, dtm, mdt, mdt1, vdt, dtPo, fmdt, dt_dist, dt_dist1, dticode, dticode2 = new DataTable();
    DataRow dro, dr1, dro1 = null;
    double month, to_cons, itot_stk, itv, db9, db8, db7, db6, db5, db4, db3, db2, db1, db; DataRow oporow, ROWICODE, ROWICODE2; DataView dv, mvdview, vdview, vdview1, dist1_view, sort_view;
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
                case "F10156":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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
        val = hfid.Value.Trim();
        fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        // if coming after SEEK popup
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
        {
            value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
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
                        hf2.Value = value1;
                        fgen.Fn_open_prddmp1("-", frm_qstr);
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
            }
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

        }
    }
    protected void rep1_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "HSNCODE";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "HSNCODE");
        open_EDIForm();
    }
    protected void rep2_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "ACOPBAL";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "ACOPBAL");
        open_EDIForm();
    }
    protected void rep3_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "BILLWISEOUTDR";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "BILLWISEOUTDR");
        open_EDIForm();
    }
    protected void rep4_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "BILLWISEOUTCR";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "BILLWISEOUTCR");
        open_EDIForm();
    }
    protected void rep5_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "ITEMWISEOPBL";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "ITEMWISEOPBL");
        open_EDIForm();
    }
    protected void rep6_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "ItemwiseOpBl";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "SS24");
        open_EDIForm();
    }
    protected void rep7_ServerClick1(object sender, EventArgs e)
    {
    }
    protected void rep8_ServerClick1(object sender, EventArgs e)
    {
        hfid.Value = "WIPSTKOP";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "WIPSTKOP");
        open_EDIForm();
    }
    protected void rep9_ServerClick1(object sender, EventArgs e)
    {
        hfid.Value = "STORESTKBATCHWISE";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "STORESTKBATCHWISE");
        open_EDIForm();
    }
    protected void rep10_ServerClick1(object sender, EventArgs e)
    {
        hfid.Value = "REELSTOCK";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "REELSTOCK");
        open_EDIForm();
    }
    protected void rep11_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "BOM";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "BOM");
        open_EDIForm();
    }
    protected void rep12_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "INWQCTEMP");
        open_EDIForm();
    }
    protected void rep13_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "OUTQCTEMP");
        open_EDIForm();
    }
    protected void rep14_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "SALEORDER");
        open_EDIForm();
    }

    void open_EDIForm()
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;
        string fil_loc = ("../tej-base/om_mrr_edi.aspx");
        Session["mymst"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + frm_qstr + "','98%','98%','" + "" + "');", true);
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Session["mymst"] = null;
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    protected void btnAcSchedule_ServerClick(object sender, EventArgs e)
    {
        hfid.Value = "BSSCH_UPL";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "BSSCH_UPL");
        open_EDIForm();
    }
    protected void btnItemSubGrp_ServerClick(object sender, EventArgs e)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;
        string fil_loc = ("../tej-base/om_multi_item.aspx");
        Session["mymst"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + frm_qstr + "@F99155','98%','98%','" + "" + "');", true);
    }
    protected void btnAcMaster_ServerClick(object sender, EventArgs e)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;
        string fil_loc = ("../tej-base/om_multi_account.aspx");
        Session["mymst"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + frm_qstr + "@F99159','98%','98%','" + "" + "');", true);
    }
    protected void btnItemMaster_ServerClick(object sender, EventArgs e)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;
        string fil_loc = ("../tej-base/om_multi_item.aspx");
        Session["mymst"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + frm_qstr + "@F99157','98%','98%','" + "" + "');", true);
    }
    protected void btnPOPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnGEPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnMRRPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnRGPPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnSOPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnDAPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnINVPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnJCPlaceHolder_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnPOUpload_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnAppVend_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnLotWiseBalance_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnPhyStoreStock_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnAcctgEnt_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnProdnEntry_ServerClick(object sender, EventArgs e)
    {

    }
}