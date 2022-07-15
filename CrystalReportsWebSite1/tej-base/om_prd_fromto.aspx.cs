using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;


public partial class om_prd_fromto : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-", Value9 = "-", Value10 = "-";
    string HCID, co_cd; int col_count = 0;
    string frm_qstr, frm_url, frm_cocd, frm_mbr, frm_formID, YR_SL, year;
    string Today1 = "";
    string Prg_Id="";
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
                }
            }
            //--------------------------            
            co_cd = frm_cocd;

            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_XID");
            query1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");

            string boxType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BOXTYPE");
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
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (!Page.IsPostBack)
            {
                txtfromdt.Text = Convert.ToDateTime("01/04/" + year).ToString("yyyy-MM-dd");
                txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");

                switch (Prg_Id)
                {
                    case "F25132":
                    case "F25133":
                    case "F25134":
                        lbl_I.InnerText = "Item Main Group";
                        lbl_II.InnerText = "Item Code";
                        break;

                    case "F39119":
                        lbl_I.InnerText = "Prodn type";
                        lbl_II.InnerText = "Item Code";
                        break;

                    case "F10111":
                    case "F10116":
                    case "F10131":
                    case "F10133":
                    case "F10156":
                        lbl_I.InnerText = "Item Main Group";
                        lbl_II.InnerText = "Item Sub Group";
                        break;
                    case "F15126":
                    case "F15132":
                    case "F15308":
                    case "F15101":
                    case "F15302":
                    case "F15138":
                    case "F25111":
                        lbl_I.InnerText = "Select Deptt";

                        lbl_II.InnerText = "Select Item Group";
                        break;
                    case "F15128":
                    case "F15127":
                    case "F15129":
                    case "F47111":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item Group";

                        break;

                    case "F47126":
                    case "F49126":
                    case "F47127":
                    case "F49127":
                    case "F55126":
                    case "F55127":
                    case "F55128":
                    case "F55129":
                    
                    case "F47132":
                    case "F47133":
                    case "F47134":
                    case "F47135":
                    case "F47136":
                    case "F15133":
                    case "F47155":
                    case "F47156":

                    case "F15303":
                    case "F15309":
                    case "F15310":
                    case "F15311":
                    case "F15315":
                    case "F15316":
                    case "F15304":
                    case "F15305":
                    case "F15306":
                    case "F47101":
                    case "F47106":
                    case "F50101":
                    case "F47141":
                    case "F47142":
                    case "F15314":
                    case "F25101":
                    case "F20101":
                    case "F20121":
                    case "F15106":
                    case "F30141":
                    case "F20127":
                    case "F25106":
                    case "F20132":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item ";
                        break;
                    
                    case "F15307":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;

                    case "F70172":
                        lbl_I.InnerText = "Actg Main Group";

                        lbl_II.InnerText = "Actg Schedule ";
                        break;

                    // ADDED BY MADHVI ON 6TH APRIL 2018
                   //---------------------------------
                    case "F50126":
                    case "F50127":
                    case "F50132":
                    case "F50133":
                    case "F50134":
                    case "F50141":
                    case "F50142":
                    case "F50143":
                    case "F50223":
                    case "F50224":
                    case "F50225":
                    case "F50226":
                    case "F50227":
                    case "F50228":
                    case "F50265":
                    case "F50128":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item ";
                        break;

                    case "F50222":
                        lbl_I.InnerText = "Select Party";
                        //lbl_II.InnerText = "Select Item ";
                        itemBox.Visible = false;
                        break;

                    case "F50231":
                        lbl_I.InnerText = "Select District";
                        itemBox.Visible = false;
                        break;

                    case "F50232":
                        lbl_I.InnerText = "Select State";
                        itemBox.Visible = false;
                        break;

                    case "F50233":
                        lbl_I.InnerText = "Select Zone";
                        itemBox.Visible = false;
                        break;

                    case "F50234":
                        lbl_I.InnerText = "Select Mktg Person";
                        itemBox.Visible = false;
                        break;

                    case "F50235":
                        lbl_I.InnerText = "Select Cust. Grp.";
                        itemBox.Visible = false;
                        break;

                    case "F50236":
                        lbl_I.InnerText = "Select Main Grp.";
                        lbl_II.InnerText = "Select Sub Grp.";
                        break;

                    case "F50250":
                    case "F50251":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;

                    case "F50255":
                    case "F50257":
                    case "F50264":
                        lbl_I.InnerText = "Select Item";
                        itemBox.Visible = false;
                        break;

                    case "F50256":
                    case "F50258":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;
                    //------------------------------

                    // ADDED BY MADHVI ON 9TH APR 2018 INVENTORY MODULE
                    case "F25126":
                    case "F25127":
                    case "F25141":
                    case "F25142":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item ";
                        break;

                    case "F25128":
                    case "F25129":
                    case "F25143":
                    case "F25144":
                        lbl_I.InnerText = "Select Deptt.";
                        lbl_II.InnerText = "Select Item ";
                        break;

                    case "M1": // ON 14 MAY 2018
                        lbl_I.InnerText = "Select Supp.";
                        itemBox.Visible = false;
                        break;

                    case "M2": // ON 14 MAY 2018
                        lbl_I.InnerText = "Select Supp.";
                        lbl_II.InnerText = "Select Item ";
                        break;
                    //------------------------------

                    // ADDED BY MADHVI ON 14TH MAY 2018 DOMESTIC SALES MODULE
                    case "F47226":
                        lbl_I.InnerText = "Select Item";
                        itemBox.Visible = false;
                        break;

                    case "F47227":
                        lbl_I.InnerText = "Select Customer";
                        itemBox.Visible = false;
                        break;
                    //------------------------------
                    case "F25233":// ITEM REVIEW FORM BY MADHVI
                        lbl_I.InnerText = "Select Main Grp";
                        lbl_II.InnerText = "Select Report Options";
                        break;

                    case "F70282": // ACCOUNT REVIEW FORM BY MADHVI
                        lbl_I.InnerText = "Select Acct Grp";
                        itemBox.Visible = false;
                        break;
                    default:
                        break;
                }
            
            }
            txtfromdt.Attributes.Add("onkeypress", "return clickEnter('" + txttodt.ClientID + "', event)");
            txttodt.Attributes.Add("onkeypress", "return clickEnter('" + btnsubmit.ClientID + "', event)");

            btnPmcode.Focus();
        }
    }
    void makequery4popup()
    {
        string squery = "";
        string cond = " like '%'";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (hffield.Value)
        {
            case "PMCODE":
                cond = " like '%'";
                switch (Prg_Id)
                {

                    case "F10111":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y' and substr(a.type1,1,1) <'9' order by a.Type1";
                        break;
                    case "F10116":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y' and substr(a.type1,1,1) >='9' order by a.Type1";
                        break;
                    case "F10131":
                    case "F10133":
                    case "F10156":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y'  order by a.Type1";
                        break;

                    case "F15126":
                    case "F15132":
                    case "F15308":
                    case "F15101":
                    case "F15302":
                    case "F15138":
                    case "F25111":
                    squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='M' and a.type1 like '6%' order by a.Type1";
                        break;
                    case "F15128":
                    case "F15127":
                    case "F15129":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06') order by a.Aname ";
                        break;
                    case "F47111":
                    case "F47134":
                    case "F47135":

                    case "F47155":
                    case "F47156":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,schedule b where trim(A.acode)=trim(B.acodE) and b.branchcd='"+ frm_mbr +"' and b.type='46' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;
                    case "F15133":
                    case "F15303":
                    case "F15304":
                    case "F15305":
                    case "F15306":
                    case "F15309":
                    case "F15310":
                    case "F15311":
                    case "F15315":
                    case "F15316":
                    case "F15106":
                    case "F15307":
                    case "F20127":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,pomas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '5%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;

                    case "F47132":
                    case "F47101":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,somasm b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;
                    case "F50101":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,sale b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;

                    case "F47133":
                    case "F47106":
                    case "F47126":
                    case "F49126":
                    case "F47127":
                    case "F49127":
                    case "F47141":
                    case "F47142":
                    case "F47136":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,somas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;


                    case "F70172":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Account_code from type a where a.id='Z'  order by a.Type1";
                        break;

                    // ADDED BY MADHVI ON 6TH APRIL 2018 
                    //--------------------------------------
                    case "F50126":
                    case "F50127":
                    case "F50128":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,somas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F50132":
                    case "F50133":
                    case "F50134":
                    case "F50141":
                    case "F50142":
                    case "F50143":
                    case "F50222":
                    case "F50223":
                    case "F50224":
                    case "F50225":
                    case "F50226":
                    case "F50227":
                    case "F50228":
                    case "F50240":
                    case "F50241":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25101":
                    case "F30141":                  
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25106":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '2%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;
                    case "F20121":
                    case "F20101":
                    case "F20132":                    
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucherp b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F39119":
                        squery = "SELECT distinct trim(b.type1) as FStr,b.Name as Prodn_Type,trim(b.type1) as Grp_Code from ivoucher a,type b where a.branchcd='" + frm_mbr + "' and trim(A.type)=trim(B.type1) and b.id='M' and a.type like '1%' and  a.type>='15'  order by b.Name";
                        break;

                    case "F25132":
                    case "F25133":
                    case "F25134":
                        squery = "SELECT distinct substr(a.icode,1,2) as FStr,b.Name as Item_Grp,trim(b.type1) as Grp_Code from ivoucher a,type b where a.branchcd='" + frm_mbr + "' and substr(A.icode,1,2)=trim(B.type1) and b.id='Y' and a.type like '%' order by trim(b.type1)";
                        break;

                    case "F50231":
                        squery = "SELECT DISTINCT TRIM(DISTRICT) AS FSTR,DISTRICT,'-' AS S FROM FAMST WHERE SUBSTR(TRIM(ACODE),1,2)='16' AND LENGTH(TRIM(DISTRICT))>1 ORDER BY FSTR";
                        break;

                    case "F50232":
                        squery = "SELECT DISTINCT TRIM(STATEN) AS FSTR,STATEN AS STATE,'-' AS S FROM FAMST WHERE SUBSTR(TRIM(ACODE),1,2)='16' AND LENGTH(TRIM(STATEN))>1 ORDER BY FSTR";
                        break;

                    case "F50233":
                        squery = "SELECT DISTINCT TRIM(ZONAME) AS FSTR,ZONAME AS ZONE,'-' AS S FROM FAMST WHERE SUBSTR(TRIM(ACODE),1,2)='16' AND LENGTH(TRIM(ZONAME))>1 ORDER BY FSTR";
                        break;

                    case "F50234":
                        squery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,TYPE1 AS CODE,NAME FROM TYPEGRP WHERE ID='A' AND TYPE1 LIKE '16%' ORDER BY FSTR";
                        break;

                    case "F50235":
                        squery = "SELECT DISTINCT TRIM(MKTGGRP) AS FSTR,MKTGGRP AS CUSTOMER_GRP,'-' AS S FROM FAMST WHERE SUBSTR(TRIM(ACODE),1,2)='16' AND LENGTH(TRIM(MKTGGRP))>1 ORDER BY FSTR";
                        break;

                    case "F50236":
                        squery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,TYPE1 AS CODE,NAME FROM TYPE WHERE ID='Y' AND TYPE1 LIKE '9%' ORDER BY FSTR";
                        break;

                    case "F50250":
                    case "F50251":
                    case "F50256":
                    case "F50258":
                    case "F50265":
                    case "F47227":// 15 MAY 2018
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F50255":
                    case "F50257":
                    case "F50264":
                    case "F47226": // 14 MAY 2018
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '4%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;                   
                    //---------------------------------------

                    // ADDED BY MADHVI ON 9TH APRIL 2018 INVENTORY MODULE
                    //--------------------------------------
                    case "F25126":
                    case "F25141":
                    case "M1": // ON 14 MAY 2018
                    case "M2":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25127":
                    case "F25142":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '2%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25128":
                    case "F25129":
                    case "F25143":
                    case "F25144":
                        squery = "SELECT distinct trim(a.TYPE1) as FStr,a.name as deptt,a.type1 as code from type a where a.type1 like '6%' and a.id='M' order by fstr";
                        break;
                    //---------------------------------------
                    case "F25233": // ITEM REVIEW FORM BY MADHVI
                        squery = "SELECT TRIM(TYPE1) AS FSTR,TYPE1,NAME FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        break;
                    case "F70282": // ACCOUNT REVIEW FORM BY MADHVI
                        squery = "SELECT TRIM(TYPE1) AS FSTR,TYPE1,NAME FROM TYPE WHERE ID='Z' ORDER BY TYPE1";
                        break;
                    default:
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname ";
                        break;
                }
                break;

            case "MCODE":

                cond = " like '%'";
                switch (Prg_Id)
                {
                    case "F10111":
                    case "F10116":
                    case "F10131":
                    case "F10133":
                    case "F10156":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))=4 and substr(a.icode,1,2)='" + txtacode.Value + "' order by a.Iname ";
                        break;
                    case "F39119":
                        squery = "SELECT distinct trim(b.icode) as FStr,b.IName as Item_name,trim(a.icode) as ERP_Code,b.Cpartno,B.Cdrgno from ivoucher a,Item b where a.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and a.type like '1%' and  a.type>='15'  order by b.IName";
                        break;
                    case "F25132":
                    case "F25133":
                    case "F25134":
                        squery = "SELECT distinct trim(b.icode) as FStr,b.IName as Item_name,trim(a.icode) as ERP_Code,b.Cpartno,B.Cdrgno from ivoucher a,Item b where a.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and a.type like '%' order by b.IName";
                        break;

                    case "F15126":
                    case "F15127":
                    case "F15128":
                    case "F15129":
                    case "F15132":
                    case "F15302":
                    case "F15308":
                    case "F25111":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Main_grp_code from type a where a.id='Y' order by a.Type1";
                        break;
                    case "F15101":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y' order by a.Type1";

                        break;
                    case "F47111":
                        squery = "SELECT distinct a.type1 as Fstr,A.Name,A.type1 as Main_grp_code from type a,schedule b where trim(A.type1)=substr(b.icode,1,2) and b.branchcd='" + frm_mbr + "' and b.type='46' and a.id='Y' order by a.Type1";
                        break;

                    case "F47132":
                    case "F47101":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,somasm b where b.branchcd='"+ frm_mbr+"' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F50101":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '4%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;

                    case "F25101":
                    case "F30141":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '0%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;

                    case "F25106":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '2%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F20121":
                    case "F20101":
                    case "F20132":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucherp b where b.branchcd='" + frm_mbr + "' and b.type like '0%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;

                    // ADDED BY MADHVI ON 6TH APRIL 2018
                    //----------------------------
                    case "F50132":
                    case "F50133":
                    case "F50134":
                    case "F50141":
                    case "F50142":
                    case "F50143":
                    case "F50222":
                    case "F50223":
                    case "F50224":
                    case "F50225":
                    case "F50226":
                    case "F50227":
                    case "F50228":
                    case "F50265":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '4%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;

                    case "F50126":
                    case "F50127":
                    case "F50128":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,somas b where b.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;

                    case "F50236":
                        squery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS CODE,INAME FROM ITEM WHERE LENGTH(TRIM(ICODE))=4 AND SUBSTR(TRIM(ICODE),1,2)='" + txtacode.Value + "'  ORDER BY FSTR";
                        break;
                    //----------------------------------

                    // ADDED BY MADHVI ON 9TH APRIL 2018 INVENTORY MODULE
                    //----------------------------
                    case "F25126":
                    case "F25141":
                    case "M2": // ON 14 MAY 2018
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '0%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by FStr ";
                        break;

                    case "F25127":
                    case "F25142":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '2%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by FStr ";
                        break;

                    case "F25128":
                    case "F25143":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '3%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by FStr ";
                        break;

                    case "F25129":
                    case "F25144":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '1%' and type<'15' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by FStr ";
                        break;
                    //----------------------------------

                    case "F47133":
                    case "F47106":
                    case "F47126":
                    case "F49126":
                    case "F47127":
                    case "F49127":
                    case "F47141":
                    case "F47142":
                    case "F47136":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,somas b where b.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F47134":
                    case "F47135":
                    case "F47155":
                    case "F47156":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,schedule b where b.branchcd='" + frm_mbr + "' and b.type='46' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F15133":
                    case "F15309":
                    case "F15310":
                    case "F15311":
                    case "F15315":
                    case "F15316":
                    case "F15106":
                    case "F15304":
                    case "F15307":
                    case "F20127":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,pomas b where b.branchcd='" + frm_mbr + "' and b.type like '5%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;

                    case "F70172":
                        squery = "SELECT distinct a.Type1 as FStr,a.Name as Schedule_Name,a.Type1 as Sch_Code,a.ent_by,a.edt_by from Typegrp a where a.id='A' and a.branchcd!='DD' and substr(a.type1,1,2)='" + txtacode.Value + "' order by a.Type1 ";
                        break;
                    case "F25233": // ITEM REVIEW FORM BY MADHVI
                        squery = "SELECT 'Y' AS FSTR,'STORE' AS REPORT_OPTIONS,'-' AS S FROM DUAL UNION ALL SELECT 'R' AS FSTR,'REJECTION' AS REPORT_OPTIONS,'-' AS S FROM DUAL";
                        break;
                    default:
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                }
                break;

            //case "SCODE":
            //    cond = " like '%'";
            //    if (txtMcode.Value.Length > 1) cond += " and substr(trim(icode),1,2) = '" + txtMcode.Value + "'";
            //    squery = "select icode as FSTR,iname as product,icode as CODE from ITEM WHERE LENGTH(TRIM(ICODE))<8 AND ICODE " + cond + " order by icode";
            //    break;
            //case "PSCODE":
            //    cond = " like '%'";
            //    if (txtMcode.Value.Length > 1) cond += " and trim(type1) = '" + txtMcode.Value + "'";
            //    squery = "select TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPEgrp WHERE ID='A' AND TYPE1 " + cond + " order by type1";
            //    break;
            //case "ICODE1":

            //    cond = " like '%'";
            //    if (txtMcode.Value.Length > 1) cond += " and trim(icode) like '" + txtMcode.Value + "%'";
            //    if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
            //    squery = "select icode as FSTR,iname as product,icode as erpcode,cpartno,unit from ITEM WHERE LENGTH(TRIM(ICODE))>4 AND ICODE " + cond + " order by icode";
            //    break;
            //case "ACODE1":
            //    cond = " like '%'";
            //    if (txtMcode.Value.Length > 1) cond += " and trim(acode) = '" + txtMcode.Value + "%'";
            //    if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
            //    squery = "select acode as FSTR,aname as product,acode as code,addr1,email from famst WHERE aCODE " + cond + " order by acode";
            //    break;
            //case "ICODE2":
            //    cond = " like '%'";
            //    if (txtMcode.Value.Length > 1) cond += " and trim(icode) like '" + txtMcode.Value + "%'";
            //    if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
            //    squery = "select icode as FSTR,iname as product,icode as erpcode,cpartno,unit from ITEM WHERE LENGTH(TRIM(ICODE))>4 AND ICODE " + cond + " order by icode";
            //    break;
            //case "ACODE2":
            //    if (txtMcode.Value.Length > 1) cond += " and trim(acode) = '" + txtMcode.Value + "%'";
            //    if (txtSubCode.Value.Length > 1) cond = " like '" + txtSubCode.Value + "%'";
            //    squery = "select acode as FSTR,aname as product,acode as code,addr1,email from famst WHERE aCODE " + cond + " order by acode";
            //    break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOX");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btniBox_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "PMCODE":
                txtacode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnPsubCode.Focus();
                //else btnPmcode.Focus();
                break;

            case "MCODE":
                txticode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                //if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
                //{
                //    txtIcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "010001";
                //    txtIcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "999999"; txtSubCode.Value = "";
                //    btnSubCode.Focus();
                //}
                //else btnMcode.Focus();
                break;
            //case "SCODE":
            //    txtSubCode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            //    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
            //    {
            //        txtIcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "0001";
            //        txtIcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "9999";
            //        btnIcode.Focus();
            //    }
            //    else btnSubCode.Focus();
            //    break;
            //case "PSCODE":
            //    txtPSubCode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            //    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnAcode1.Focus();
            //    else btnPsubCode.Focus();
            //    break;
            //case "ICODE1":
            //    txtIcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            //    txtIname1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            //    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
            //    {
            //        txtIcode2.Value = txtIcode1.Value;
            //        txtIname2.Value = txtIname1.Value;
            //        btnIcode2.Focus();
            //    }
            //    else btnIcode.Focus();
            //    break;
            //case "ACODE1":
            //    txtAcode1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            //    txtAname1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            //    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1)
            //    {
            //        txtAcode2.Value = txtAcode1.Value;
            //        txtAname2.Value = txtAname2.Value;
            //        btnAcode2.Focus();
            //    }
            //    else btnAcode1.Focus();
            //    break;
            //case "ICODE2":
            //    txtIcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            //    txtIname2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            //    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnsubmit.Focus();
            //    else btnIcode2.Focus();
            //    break;
            //case "ACODE2":
            //    txtAcode2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
            //    txtAname2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            //    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 1) btnMcode.Focus();
            //    else btnAcode2.Focus();
            //    break;
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", txtacode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", txticode.Value.Trim());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT1", Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT2", Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DAYRANGE", " between to_date('01/" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");

        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
    }
    protected void btnMcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MCODE";
        makequery4popup();
    }
    //protected void btnSubCode_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "SCODE";
    //    makequery4popup();
    //}
    //protected void btnIcode_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "ICODE1";
    //    makequery4popup();
    //}
    //protected void btnIcode2_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "ICODE2";
    //    makequery4popup();
    //}
    protected void btnPmcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PMCODE";
        makequery4popup();
    }
    //protected void btnPsubCode_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "PSCODE";
    //    makequery4popup();
    //}
    //protected void btnAcode1_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "ACODE1";
    //    makequery4popup();
    //}
    //protected void btnAcode2_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "ACODE2";
    //    makequery4popup();
    //}
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

}