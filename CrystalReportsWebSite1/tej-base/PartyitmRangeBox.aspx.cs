using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class PartyitmRangeBox : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-", Value9 = "-", Value10 = "-";
    string HCID, co_cd, boxType = ""; int col_count = 0;
    string frm_qstr, frm_url, frm_cocd, frm_mbr, frm_formID, YR_SL, year, frm_cDt1, frm_cDt2;
    string spl_series = "%";
    string Today1 = "", uname, ulvl;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            //-----------------
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (frm_url.Contains("STR"))
            {
                if (Request.QueryString["STR"].Length > 0)
                {
                    frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
                    if (frm_qstr.Contains("@"))
                    {
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                        frm_formID = frm_qstr.Split('@')[0].ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    YR_SL = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
            }
            //--------------------------            
            co_cd = frm_cocd;

            chk1.Visible = false;
            //Div1.Attributes.Add("class", "col-md-12");

            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID2");
            boxType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BOXTYPE");
            lblerr.Text = "";
            Today1 = DateTime.Now.ToString("dd/MM/");

            if (Convert.ToInt32(DateTime.Now.ToString("MM")) >= 1 && Convert.ToInt32(DateTime.Now.ToString("MM")) < 4)
            {
                Today1 = Today1 + Convert.ToString(Convert.ToDouble(YR_SL) + 1);
                YR_SL = Convert.ToString(Convert.ToDouble(YR_SL) + 1);
            }
            else
            {
                Today1 = Today1 + Convert.ToString(Convert.ToDouble(YR_SL));
                YR_SL = Convert.ToString(Convert.ToDouble(YR_SL));
            }
            //Today1 = Today1 + YR_SL;

            if (!Page.IsPostBack)
            {
                txtfromdt.Text = Convert.ToDateTime(frm_cDt1).ToString("yyyy-MM-dd");
                txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");
            }
            txtfromdt.Attributes.Add("onkeypress", "return clickEnter('" + txttodt.ClientID + "', event)");
            txttodt.Attributes.Add("onkeypress", "return clickEnter('" + btnsubmit.ClientID + "', event)");

            btnPmcode.Focus();

            //if (!Page.IsPostBack)
            {
                wtAvgDiv.Visible = false;
                branchWiseRdTool.Visible = false;
                Div1.Visible = false;
                switch (frm_formID)
                {
                    case "F70291":
                    case "F70293":
                    case "F70295":
                    case "F70296":
                    case "F25132": // STOCK LEDGER                                        
                        partyBox.Visible = false;
                        wtAvgDiv.Visible = false;
                        btnMcode.Focus();
                        break;
                    case "F05125":
                    case "F05125A":
                    case "F05125C":
                    case "F10184":
                    case "F10184C":
                    case "F10194":
                    case "F10194E":
                    case "F10198":
                    case "F10198W":
                    case "F05125E":
                    case "F05125D":
                    case "F10194F":
                        rdPDF.Visible = false;
                        branchWiseRdTool.Visible = true;
                        spl_series = "%";
                        partyBox.Visible = false;
                        if (frm_formID == "F10184" || frm_formID == "F10194" || frm_formID == "F05125E" || frm_formID == "F05125C" || frm_formID == "F05125A" || frm_formID == "F05125" || frm_formID == "F10198" || frm_formID == "F10198W" || frm_formID == "F10194E")
                        {
                            wtAvgDiv.Visible = true;
                            branchWiseRdTool.Attributes.Add("class", "col-md-6");
                            wtAvgDiv.Attributes.Add("class", "col-md-6");
                            chk1.Visible = true;
                        }
                        if (frm_formID == "F10194" || frm_formID == "F05125E" || frm_formID == "F10198" || frm_formID == "F10198W" || frm_formID == "F10194E")
                        {
                            Div1.Visible = true;
                        }
                        //if (frm_formID == "F10194") spl_series = "7%";
                        btnMcode.Focus();
                        break;
                    case "F05115":
                        if (frm_formID == "F05115")
                        {
                            wtAvgDiv.Visible = true;
                            branchWiseRdTool.Attributes.Add("class", "col-md-6");
                            wtAvgDiv.Attributes.Add("class", "col-md-6");
                        }
                        spl_series = "%";
                        branchWiseRdTool.Visible = true;
                        if (frm_cocd == "DREM")
                            partyBox.Visible = false;
                        break;
                    default:
                        spl_series = "%";
                        wtAvgDiv.Visible = false;
                        branchWiseRdTool.Visible = false;
                        break;
                }
            }
        }
    }
    void makequery4popup()
    {
        string squery = "";
        string cond = " like '" + spl_series + "'";

        switch (hffield.Value)
        {
            case "MCODE":
                if (frm_formID == "F70291" || frm_formID == "F70295")
                {
                    if (co_cd == "JPPL")
                    {
                        squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' AND TYPE1 like '9%' AND type1!='97' order by TYPE1";
                    }
                    else
                    {
                        squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' AND TYPE1 like '9%' order by TYPE1";
                    }
                }
                else if (frm_formID == "F70293" || frm_formID == "F70296")
                {
                    squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' AND substr(type1,1,1)<'4' order by TYPE1";
                }
                else
                {
                    squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Y' AND TYPE1 " + cond + " order by TYPE1";
                }
                break;
            case "PMCODE":
                squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Z' AND TYPE1 " + cond + " order by TYPE1";
                break;
            case "SCODE":
                if (txtMcode.Value.Length > 1) cond += " and substr(trim(icode),1,2) = '" + txtMcode.Value + "'";
                else if (frm_formID == "F70291" || frm_formID == "F70295")
                {
                    cond = " like '9%'";
                }
                if (frm_formID == "F70291" || frm_formID == "F70295")
                {
                    if (co_cd == "JPPL")
                    {
                        squery = "select distinct icode as FSTR,iname as product,icode as CODE from ITEM WHERE LENGTH(TRIM(ICODE))=4 AND ICODE " + cond + " and substr(trim(icode),1,2)!='97' order by icode";
                    }
                    else
                    {
                        squery = "select distinct icode as FSTR,iname as product,icode as CODE from ITEM WHERE LENGTH(TRIM(ICODE))=4 AND ICODE " + cond + " order by icode";
                    }
                }
                else if (frm_formID == "F70293" || frm_formID == "F70296")
                {
                    if (txtMcode.Value.Length > 1)
                    {
                        cond += " and substr(trim(icode),1,2) = '" + txtMcode.Value + "'";
                        squery = "select  distinct icode as FSTR,iname as product,icode as CODE from ITEM WHERE LENGTH(TRIM(ICODE))<8 AND ICODE " + cond + " order by icode";
                    }
                    else
                    {
                        squery = "select distinct icode as FSTR,iname as product,icode as CODE from ITEM WHERE LENGTH(TRIM(ICODE))=4 AND substr(trim(icode),1,1)<'4' order by icode";
                    }
                }
                else { squery = "select icode as FSTR,iname as product,icode as CODE from ITEM WHERE LENGTH(TRIM(ICODE))<8 AND ICODE " + cond + " order by icode"; }
                break;
            case "PSCODE":
                if (txtMcode.Value.Length > 1) cond += " and trim(type1) = '" + txtMcode.Value + "'";
                squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPEgrp WHERE ID='A' AND TYPE1 " + cond + " order by type1";
                break;
            case "ICODE1":
                if (txtMcode.Value.Length > 1) cond += " and trim(icode) like '" + txtMcode.Value + "%'";
                if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
                squery = "select icode as FSTR,iname as product,icode as erpcode,cpartno,unit from ITEM WHERE LENGTH(TRIM(ICODE))>4 AND ICODE " + cond + " order by icode";
                break;
            case "ACODE1":
                if (txtMcode.Value.Length > 1) cond += " and trim(acode) = '" + txtMcode.Value + "%'";
                if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
                cond = ulvl == "M" ? uname : cond;
                squery = "select acode as FSTR,aname as product,acode as code,addr1,email from famst WHERE aCODE " + cond + " order by acode";
                break;
            case "ICODE2":
                if (txtMcode.Value.Length > 1) cond += " and trim(icode) like '" + txtMcode.Value + "%'";
                if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
                squery = "select icode as FSTR,iname as product,icode as erpcode,cpartno,unit from ITEM WHERE LENGTH(TRIM(ICODE))>4 AND ICODE " + cond + " order by icode";
                break;
            case "ACODE2":
                if (txtMcode.Value.Length > 1) cond += " and trim(acode) = '" + txtMcode.Value + "%'";
                if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
                cond = ulvl == "M" ? uname : cond;
                squery = "select acode as FSTR,aname as product,acode as code,addr1,email from famst WHERE aCODE " + cond + " order by acode";
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOX");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btniBox_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "MCODE":
                txtMcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
                {
                    txtIcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "010001";
                    txtIcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "999999"; txtSubCode.Value = "";
                    btnSubCode.Focus();
                }
                else btnMcode.Focus();
                break;
            case "PMCODE":
                txtPMcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnPsubCode.Focus();
                else btnPmcode.Focus();
                break;
            case "SCODE":
                txtSubCode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
                {
                    txtIcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "0001";
                    txtIcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "9999";
                    btnIcode.Focus();
                }
                else btnSubCode.Focus();
                break;
            case "PSCODE":
                txtPSubCode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnAcode1.Focus();
                else btnPsubCode.Focus();
                break;
            case "ICODE1":
                txtIcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtIname1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
                {
                    txtIcode2.Value = txtIcode1.Value;
                    txtIname2.Value = txtIname1.Value;
                    btnIcode2.Focus();
                }
                else btnIcode.Focus();
                break;
            case "ACODE1":
                txtAcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtAname1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
                {
                    txtAcode2.Value = txtAcode1.Value;
                    txtAname2.Value = txtAname2.Value;
                    btnAcode2.Focus();
                }
                else btnAcode1.Focus();
                break;
            case "ICODE2":
                txtIcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtIname2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnsubmit.Focus();
                else btnIcode2.Focus();
                break;
            case "ACODE2":
                txtAcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                txtAname2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnMcode.Focus();
                else btnAcode2.Focus();
                break;
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(frm_cDt1) > Convert.ToDateTime(txtfromdt.Text) || Convert.ToDateTime(frm_cDt2) < Convert.ToDateTime(txtfromdt.Text)
           || Convert.ToDateTime(frm_cDt2) < Convert.ToDateTime(txttodt.Text) || Convert.ToDateTime(frm_cDt1) > Convert.ToDateTime(txttodt.Text))
        {
            fgen.msg("-", "AMSG", "Please Select Date Range with in Current Financial Year.");
            return;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR1", txtMcode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR2", txtSubCode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR3", txtIcode1.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR4", txtIcode2.Value.Trim());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR5", txtPMcode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR6", txtPSubCode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR7", txtAcode1.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR8", txtAcode2.Value.Trim());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT1", Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT2", Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DAYRANGE", " between to_date('01/" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");

        if (rdPDF.SelectedValue == "0") Value1 = "Y";
        else Value1 = "N";
        if (rdBranch.SelectedValue == "1") Value2 = "Y";
        else Value2 = "N";
        if (rdRate.SelectedValue == "0") Value3 = "Y";
        else Value3 = "N";
        if (RadioButtonList3.SelectedValue == "0") Value4 = "Y";
        else Value4 = "N";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR9", Value2);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR10", Value3);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR11", Value4);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", Value1);
        Value5 = "N";
        if (chk1.Checked) Value5 = "Y";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COLR12", Value5);

        switch (HCID)
        {
            case "Tejaxo":
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
                break;
            default:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
                break;
        }
    }
    protected void btnMcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MCODE";
        makequery4popup();
    }
    protected void btnSubCode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SCODE";
        makequery4popup();
    }
    protected void btnIcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ICODE1";
        makequery4popup();
    }
    protected void btnIcode2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ICODE2";
        makequery4popup();
    }
    protected void btnPmcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PMCODE";
        makequery4popup();
    }
    protected void btnPsubCode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PSCODE";
        makequery4popup();
    }
    protected void btnAcode1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE1";
        makequery4popup();
    }
    protected void btnAcode2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE2";
        makequery4popup();
    }
    void checkYearDate()
    {
        string cdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
        string cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
        if (Convert.ToDateTime(cdt2) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
        {
            txtfromdt.Text = Convert.ToDateTime(cdt1.ToString()).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(cdt2.ToString()).ToString("yyyy-MM-dd");
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList2.ClearSelection();
        string cdt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
        string cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

        if (Convert.ToDateTime(cdt2) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
        {
            checkYearDate();
            return;
        }

        if (RadioButtonList1.SelectedIndex == 0)
        {
            //Y.T.D            
            txtfromdt.Text = Convert.ToDateTime(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1")).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 1)
        {
            //M.T.D         
            txtfromdt.Text = Convert.ToDateTime("01/" + Today1.Substring(3, 3).ToString().Trim() + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 2)
        {
            //Pr.Mnth                        
            txtfromdt.Text = Convert.ToDateTime("01" + "/" + DateTime.Now.ToString("MM") + "/" + YR_SL).AddMonths(-1).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(txtfromdt.Text).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 3)
        {
            //Next.Mnth       
            txtfromdt.Text = Convert.ToDateTime("01" + "/" + DateTime.Now.ToString("MM") + "/" + YR_SL).AddMonths(1).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(txtfromdt.Text).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
        }
        else if (RadioButtonList1.SelectedIndex == 4)
        {
            //Yestrdy
            txtfromdt.Text = Convert.ToDateTime(Today1).AddDays(-1).ToString("yyyy-MM-dd");
            txttodt.Text = txtfromdt.Text;
        }
        else if (RadioButtonList1.SelectedIndex == 5)
        {
            //Today
            txtfromdt.Text = Convert.ToDateTime(Today1.ToString()).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime(Today1.ToString()).ToString("yyyy-MM-dd");
        }
    }
    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList1.ClearSelection();
        YR_SL = fgenMV.Fn_Get_Mvar(frm_qstr, "U_year");
        string cdt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

        if (RadioButtonList2.SelectedIndex == 0)
        {
            //curr.mnth 
            if (Convert.ToDateTime(cdt2) < Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")))
            {
                checkYearDate();
                return;
            }
            txtfromdt.Text = Convert.ToDateTime("01/" + DateTime.Now.ToString("MM/yyyy")).ToString("yyyy-MM-dd");
            string lastd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT LAST_DAY(SYSDATE) AS lastd FROM DUAL", "lastd");
            txttodt.Text = Convert.ToDateTime(lastd).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 1)
        {
            //FirstQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/04/" + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("30/06/" + YR_SL).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 2)
        {
            //SecQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/07/" + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("30/09/" + YR_SL).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 3)
        {
            //ThirdQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/10/" + YR_SL).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("31/12/" + YR_SL).ToString("yyyy-MM-dd");
        }
        if (RadioButtonList2.SelectedIndex == 4)
        {
            //FourthQtr.mnth
            txtfromdt.Text = Convert.ToDateTime("01/01/" + Convert.ToString(Convert.ToDecimal(YR_SL) + 1).Trim()).ToString("yyyy-MM-dd");
            txttodt.Text = Convert.ToDateTime("31/03/" + Convert.ToString(Convert.ToDecimal(YR_SL) + 1).Trim()).ToString("yyyy-MM-dd");
        }
    }
}