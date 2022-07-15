using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class statement_ac : System.Web.UI.Page
{
    DataTable dt;
    string query1, Value1 = "-", Value2 = "-", Value3 = "-", Value4 = "-", Value5 = "-", Value6 = "-", Value7 = "-", Value8 = "-", Value9 = "-", Value10 = "-";
    string HCID, co_cd; int col_count = 0; string mq0 = ""; string multi_opt = "N";
    string frm_qstr, frm_url, frm_cocd, frm_mbr, frm_formID, YR_SL, year;
    string Today1 = "", frm_cDt1, frm_cDt2, frm_ulvl, frm_uname, frm_UserID;
    string Prg_Id = "";
    string PrgRep_Id = "";
    string mq1 = ""; string mq2 = "";
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
                        frm_formID = frm_qstr.Split('@')[1].ToString();
                        frm_qstr = frm_qstr.Split('@')[0].ToString();
                        
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", frm_formID);
                    }
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    YR_SL = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
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
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            PrgRep_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMREPID");

            if (!Page.IsPostBack)
            {

                txtfromdt.Text = Convert.ToDateTime(frm_cDt1).ToString("yyyy-MM-dd");
                txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");

                //txtfromdt.Text = Convert.ToDateTime("01/04/" + year).ToString("yyyy-MM-dd");
                //txttodt.Text = Convert.ToDateTime(Today1).ToString("yyyy-MM-dd");




                switch (Prg_Id)
                {
                    case "F95133":
                    case "F95101":
                    case "F95106":
                    case "F90142":
                        lbl_I.InnerText = "Client Code";
                        lbl_II.InnerText = "Team Code";
                        break;

                    case "F25132":
                    case "F25133":
                    case "F25134":
                        lbl_I.InnerText = "Item Main Group";
                        lbl_II.InnerText = "Item Code";
                        break;
                    case "F35107":
                        lbl_I.InnerText = "Customer";
                        lbl_II.InnerText = "Product";
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
                    case "F15245":
                    case "F25234":
                    case "F50135":
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
                    case "F25260":
                    case "F25261":
                        lbl_I.InnerText = "Select Deptt";
                        lbl_II.InnerText = "Select Item Group";
                        if (PrgRep_Id == "F15101_3")
                        {
                            lbl_I.InnerText = "Select Item MainGrp";
                            lbl_II.InnerText = "Select Item SubGroup";
                        }
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
                    case "F25101":
                    case "F20101":
                    case "F20121":
                    case "F15106":
                    case "F30141":
                    case "F20127":
                    case "F25106":
                    case "F20132":
                    case "F30142":
                    case "F30143":
                    case "F30132":
                    case "F30121":
                    case "F25152":
                    case "F25156":
                    case "F25162":
                    case "F25165":

                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item ";
                        break;
                    case "F70556":
                    case "F70172":
                        lbl_I.InnerText = "Actg Main Group";
                        lbl_II.InnerText = "Actg Schedule ";
                        break;
                    case "F70151":
                    case "F70231":
                    case "F70232":
                    case "F70233":
                    case "F70234":
                    case "F70235":
                    case "F70236":
                    case "F70126":
                    case "F70127":
                    case "F70128":
                    case "F70129":
                    case "F70237":
                    case "F70238":
                    case "F70130":
                        lbl_I.InnerText = "Select Type";
                        lbl_II.InnerText = "Select Account";
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
                    case "F50224":
                    case "F50228":
                    case "F50265":
                    case "F50128":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item ";
                        break;

                    case "F15307":
                    case "F50222":
                    case "F70281":
                        lbl_I.InnerText = "Select Party";
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

                    case "F25232":
                    case "F50236":
                        lbl_I.InnerText = "Select Main Grp.";
                        lbl_II.InnerText = "Select Sub Grp.";
                        break;

                    case "F25245":
                        lbl_I.InnerText = "Select Main Grp.";
                        itemBox.Visible = false;
                        break;

                    case "F50225":
                    case "F50256":
                    case "F50258":
                    case "F50250":
                    case "F50251":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;

                    case "F50255":
                    case "F50257":
                    case "F50264":
                    case "F70291":
                    case "F70293":
                        lbl_I.InnerText = "Select Item";
                        itemBox.Visible = false;
                        break;

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

                    case "F25247": // ON 14 MAY 2018
                        lbl_I.InnerText = "Select Supp.";
                        itemBox.Visible = false;
                        break;

                    case "F25248": // ON 14 MAY 2018
                        lbl_I.InnerText = "Select Supp.";
                        lbl_II.InnerText = "Select Item ";
                        break;
                    //------------------------------

                    // ADDED BY MADHVI ON 14TH MAY 2018 DOMESTIC SALES MODULE
                    case "F47226":
                    case "F50226":
                    case "F50223":
                    case "F50227":
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
                    case "F39251":
                        lbl_I.InnerText = "Select MRR";
                        lbl_II.InnerText = "Select Choice";
                        break;
                    case "F38501": // 
                        lbl_I.InnerText = "Select_Store";
                        lbl_II.InnerText = "Select_Voucher";
                        break;

                    case "F50301":
                    case "F50271":
                        lbl_I.InnerText = "Select Invoice";
                        itemBox.Visible = false;
                        break;
                    case "F70438":
                    case "F70439":
                    case "F70440":
                    case "F70441":
                        lbl_I.InnerText = "Select Group";
                        lbl_II.InnerText = "Select Location";
                        break;
                    case "F50306":
                    case "F50308":
                    case "F49149":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Invoice";
                        break;
                    // ADDED BY MADHVI ON 13TH NOV 2018 PURCHASE MODULE
                    case "F15135":
                    case "F15233":
                    case "F15234":
                    case "F15240":
                    case "F15238":
                    case "F15239":
                    case "F15231":
                    case "F15232":
                    case "F15230":
                    case "F15241":
                    case "F15236":
                    case "F15237":
                    case "F15318":
                    case "F15249":
                    case "F15247":
                    case "F15248":
                    case "F15134":
                    case "F15143":
                    case "F15142":
                    case "F15250":
                    case "F15251":
                        lbl_I.InnerText = "Select Supp.";
                        lbl_II.InnerText = "Select Item";
                        break;

                    case "F15140":
                    case "F15229":
                    case "F15141":
                    case "F15136":
                    case "F15235":
                    case "F15314":
                        lbl_I.InnerText = "Select Main Grp.";
                        lbl_II.InnerText = "Select Sub Grp.";
                        break;

                    case "F15244":
                    case "F15228":
                    case "F15226":
                    case "F15225":
                    case "F15227":
                        lbl_I.InnerText = "Select Item";
                        itemBox.Visible = false;
                        break;

                    case "F15222":
                    case "F15223":
                        lbl_I.InnerText = "Select Supp.";
                        lbl_II.InnerText = "Select Item";
                        datebox.Visible = false;
                        break;
                    case "F50245":
                        datebox.Visible = false;
                        break;
                    case "F50244":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        datebox.Visible = false;
                        break;

                    case "F50242":
                        lbl_I.InnerText = "Select Item";
                        itemBox.Visible = false;
                        datebox.Visible = false;
                        break;

                    //------------------------------

                    case "F40351": // KPAC PROCESS PLAN REPORT
                        lbl_I.InnerText = "Select Cust.";
                        lbl_II.InnerText = "Select Item";
                        break;

                    case "F49202":
                    case "F49203":
                    case "F49204":
                    case "F49205":

                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Country";
                        break;

                    case "F15601":
                        lbl_I.InnerText = "Select Supp.";
                        lbl_II.InnerText = "Select Item";
                        break;

                    case "F70348":
                        lbl_I.InnerText = "Select Bank";
                        lbl_II.InnerText = "Select Party";
                        break;

                    case "F50313":
                    case "F47322":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;
                    case "F30367":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select WO";
                        datebox.Visible = false;
                        break;
                    case "F55523":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;
                    case "F70201":
                    case "F70204":
                    case "F70203":
                    case "F50051":
                    case "F25122C":
                    case "F25122M":
                        lbl_I.InnerText = "Select Type";
                        itemBox.Visible = false;
                        break;
                    case "F79144":
                    case "F79143":
                    case "F79145":
                    case "F79141":
                    case "F79142":
                        lbl_I.InnerText = "Select Item";
                        itemBox.Visible = false;
                        break;

                    case "F50325":
                    case "F50326":
                    case "F50330"://CSV REPROT FOR VELVIN
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Invoice";
                        break;

                    case "F50321":
                        lbl_I.InnerText = "Select Item";
                        lbl_II.InnerText = "Select Order";
                        break;

                    case "F39255": // REASON WISE REJECTIONS (SVPL)
                        lbl_I.InnerText = "Select Family";
                        lbl_II.InnerText = "Select Item";
                        break;
                    case "F50269":
                        lbl_I.InnerText = "Select Party";
                        itemBox.Visible = false;
                        break;

                    case "F70285":
                        lbl_I.InnerText = "Select Type";
                        lbl_II.InnerText = "Select Order";
                        datebox.Visible = false;
                        break;
                    case "F50277":
                    case "F50275":
                    case "F50276":
                    case "F50278":
                    case "F50279":
                        lbl_I.InnerText = "Select Main Group";
                        lbl_II.InnerText = "Select Sub Group";
                        break;
                    case "F25169":
                        lbl_I.InnerText = "Select Vendor";
                        itemBox.Visible = false;
                        break;
                    case "F25170":
                        lbl_I.InnerText = "Select Customer";
                        itemBox.Visible = false;
                        break;
                    case "F50273":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item ";
                        break;
                    case "F15211":
                        lbl_I.InnerText = "Select P.O. Type";
                        lbl_II.InnerText = "Select Vendor Code";
                        break;
                    case "F25159":
                    case "F25160":
                        lbl_I.InnerText = "Select Supplier";
                        lbl_II.InnerText = "Select Item";
                        break;
                    case "F25163":
                        lbl_I.InnerText = "Select Main Group";
                        lbl_II.InnerText = "Select Sub Group";
                        break;
                    case "F25262":
                        lbl_I.InnerText = "Select WO No";
                        itemBox.Visible = false;
                        break;

                    case "F40063":
                        lbl_I.InnerText = "Select Dept";
                        lbl_II.InnerText = "Select Machine";
                        break;

                    case "F45136":
                        lbl_I.InnerText = "Select User";
                        itemBox.Visible = false;
                        break;


                    case "F50323":
                    case "F50324":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Product";
                        break;

                    case "F50322":
                        lbl_I.InnerText = "Select Product";
                        itemBox.Visible = false;
                        break;

                    case "F50328":
                    case "F50329":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Item";
                        break;

                    case "F50154":
                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Product";
                        break;

                    //==================================new working======================================
                    case "RPT1":
                    case "RPT13":
                    case "RPT14":
                    case "RPT17":
                    case "RPT18":
                    case "RPT19":
                    case "RPT21":
                    case "RPT25":
                    case "RPT28":
                        lbl_II.InnerText = "Select Party";
                        lbl_I.InnerText = "Select Schedule";
                        break;
                    case "RPT2":
                    case "RPT3":
                    case "RPT5":
                    case "RPT7":
                    case "RPT8":
                    case "RPT9":
                    case "RPT10":
                        lbl_I.InnerText = "Select Schedule";
                        lbl_II.InnerText = "Select Type";
                        break;
                    case "RPT4":
                    case "RPT6":
                    case "RPT27":
                        lbl_I.InnerText = "Select Prod_Group";
                        lbl_II.InnerText = "Select Schedule";
                        break;

                    case "RPT11":
                        lbl_I.InnerText = "Select Item";
                        lbl_II.InnerText = "Select Schedule";
                        break;
                    case "RPT12":
                        lbl_I.InnerText = "Select Sale Group";
                        lbl_II.InnerText = "Select Type";
                        break;


                    case "RPT16":
                    case "RPT20":

                        lbl_I.InnerText = "Select Party";
                        lbl_II.InnerText = "Select Schedule";
                        break;
                    case "RPT15":
                    case "RPT26":
                        lbl_I.InnerText = "Select Item";
                        lbl_II.InnerText = "Select Schedule";
                        break;
                    case "RPT22":
                    case "RPT23":
                    case "RPT24":
                        lbl_I.InnerText = "Select Schedule";
                        lbl_II.InnerText = "Select Type";
                        break;
                    case "F35228C":
                    case "F35228D":
                        partyBox.Visible = false;
                        break;
                    case "F39551":
                        lbl_I.InnerText = "Select Line No.";
                        lbl_II.InnerText = "Select Part No.";
                        UpdatePanel1.Visible = false;
                        H2.Visible = false;
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
        string _popHeading = "-";
        string squery = "";
        string cond = " like '%'";
        string multi_opt = "N"; string zprd = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrgRep_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMREPID");
        cond = "acode" + (frm_ulvl == "M" ? "='" + frm_uname + "'" : " like '%'");

        string xvty = "";

        switch (Prg_Id)
        {


            case "F70231":
            case "F70126":
                xvty = "1";
                break;
            case "F70236":
            case "F70232":
            case "F70127":
                xvty = "2";
                break;
            case "F70233":
            case "F70128":
                xvty = "3";
                break;
            case "F70234":
            case "F70130":
                xvty = "4";
                break;
            case "F70235":
            case "F70129":
                xvty = "5";
                break;
        }
        switch (hffield.Value)
        {
            case "PMCODE":
                switch (Prg_Id)
                {
                    case "F50277":
                    case "F50276":
                        squery = "SELECT DISTINCT TYPE1 AS FSTR,TYPE1 AS MGCODE,NAME  FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        break;
                    case "F70231":
                    case "F70232":
                    case "F70233":
                    case "F70234":
                    case "F70235":
                    case "F70236":
                    case "F70126":
                    case "F70127":
                    case "F70128":
                    case "F70129":
                    case "F70130":
                        multi_opt = "Y";
                        squery = "SELECT distinct trim(a.Type) as FStr,b.Name as Voucher_Type_Name,a.Type as Voucher_Type from Voucher A,type b WHERE a.branchcd='" + frm_mbr + "' and a.type like '" + xvty + "%' and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and trim(a.type)=trim(b.type1) and b.id='V' order by A.type";
                        break;
                    case "F50278":
                        squery = "SELECT DISTINCT TYPE1 AS FSTR,TYPE1 AS MGCODE,NAME  FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        break;
                    case "F15211":
                        squery = "SELECT DISTINCT TYPE1 AS FSTR,TYPE1 AS MGCODE,NAME  FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' ORDER BY TYPE1";
                        break;
                    case "F50386": //ITEM WISE WISE...stud report
                    case "F50388"://SUBGROUP WISE
                    case "F50390"://MAIN GROUP WISE
                        squery = "Select distinct acode as fstr,aname as Account_Name,acode,addr1,addr2  from famst where substr(Acode,1,2) in ('16','02') order by fstr";
                        multi_opt = "Y";
                        _popHeading = "Select Party Code";
                        break;

                    case "F95133":
                    case "F95101":
                    case "F95106":
                    case "F90142":
                        squery = "SELECT DISTINCT USERNAME ,USERNAME AS COCD,FULL_NAME AS company_name FROM EVAS WHERE userid>'000060' and NVL(USERNAME,'-')!='-' ORDER BY USERNAME";
                        break;
                    case "F10111":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y' and substr(a.type1,1,1) <'9' order by a.Type1";
                        break;
                    case "F10116":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y' and substr(a.type1,1,1) >='9' order by a.Type1";
                        break;
                    case "F10131":
                    case "F10133":
                    case "F10156":
                    case "F15245":
                    case "F25234":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y'  order by a.Type1";
                        break;
                    case "F50135":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='Y' and substr(a.type1,1,1)>'8' order by a.Type1";
                        break;
                    case "F15126":
                    case "F15132":
                    case "F15308":
                    case "F15101":
                    case "F15302":
                    case "F15138":
                    case "F25111":
                    case "F25260":
                    case "F25261":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Deptt_code from type a where a.id='M' and a.type1 like '6%' order by a.Type1";
                        if (PrgRep_Id == "F15101_3")
                        {
                            squery = "SELECT DISTINCT TYPE1 AS FSTR,TYPE1 AS MGCODE,NAME  FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        }
                        break;
                    case "F15128":
                    case "F15127":
                    case "F15129":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06') order by a.Aname ";
                        break;
                    case "F47111":
                    case "F47134":
                    case "F47135":

                    case "F47155":
                    case "F47156":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,schedule b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type='46' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
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
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,pomas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '5%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;

                    case "F47132":
                    case "F47101":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,somasm b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;
                    case "F50101":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,sale b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;

                    case "F47133":
                    case "F47106":
                    case "F47126":
                    case "F49126":
                    case "F47127":
                    case "F49127":
                    case "F47141":
                    case "F47136":
                    case "F35107":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,somas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2 and a." + cond + " order by a.Aname ";
                        break;
                    case "F47142":
                        multi_opt = "Y";
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,somas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2 and a." + cond + " order by a.Aname ";
                        break;

                    case "F70151":
                    case "F70237":
                    case "F70238":
                    case "F70556":
                    case "F70172":
                        multi_opt = "Y";
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Account_code from type a where a.id='Z'  order by a.Type1";
                        break;
                    case "F70201":
                    case "F70204":
                    case "F70203":
                        cond = "";
                        if (frm_cocd == "STUD")
                        {
                            mq0 = "FIXEDON";
                            if (Prg_Id == "F70201")
                                mq0 = "allowedbr";
                            string col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select " + mq0 + " as col1,allowedbr from pomst where type='21' and trim(acode)='" + frm_UserID + "'", "col1");
                            mq0 = "";
                            {
                                foreach (string s in col1.Split(';'))
                                {
                                    mq0 += ",'" + s + "'";
                                }
                                mq0 = mq0.TrimStart(',');
                            }
                            cond += " and type1 in (" + mq0 + ")";
                        }
                        if (frm_formID == "F70204") cond = " and substr(type1,1,1)!='4' ";
                        squery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='V' " + cond + " order by type1";
                        break;
                    case "F50051":
                    case "F25122C":
                    case "F25122M":
                        cond = " and substr(type1,1,1)='4' ";
                        squery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='V' " + cond + " order by type1";
                        if (frm_formID == "F25122C")
                        {
                            cond = " and substr(type1,1,1)='2' ";
                            squery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='M' " + cond + " order by type1";
                        }
                        if (frm_formID == "F25122M")
                        {
                            cond = " and substr(type1,1,1)='0' ";
                            squery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='M' " + cond + " order by type1";
                        }
                        break;
                    // ADDED BY MADHVI ON 6TH APRIL 2018 
                    //--------------------------------------
                    case "F50126":
                    case "F50127":
                    case "F50128":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,somas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F50132":
                    case "F50133":
                    case "F50134":
                    case "F50141":
                    case "F50142":
                    case "F50143":
                    case "F50222":
                    case "F50224":
                    case "F50225":
                    case "F50228":
                    case "F50240":
                    case "F50241":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25101":
                    case "F30141":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25106":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '2%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;
                    case "F20121":
                    case "F20101":
                    case "F20132":
                    case "F30132":
                    case "F30121":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucherp b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;
                    case "F25152":
                    case "F25156":
                    case "F25162":
                    case "F25165":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '2%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
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
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;
                    case "F50226":
                    case "F50223":
                    case "F50227":
                    case "F50255":
                    case "F50257":
                    case "F50264":
                    case "F47226": // 14 MAY 2018                    
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and b.type like '4%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F70291":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode  from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>=8 AND TRIM(A.ICODE) LIKE '9%' order by a.ICODE";
                        break;
                    case "F70293":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode  from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>=8 AND substr(trim(a.icode),1,1)<'4' order by a.ICODE";
                        break;
                    case "F50242":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    //---------------------------------------

                    // ADDED BY MADHVI ON 9TH APRIL 2018 INVENTORY MODULE
                    //--------------------------------------
                    case "F25126":
                    case "F25141":
                    case "F25247": // ON 14 MAY 2018
                    case "F25248":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;

                    case "F25232":
                    case "F25245":
                        squery = "SELECT distinct trim(a.TYPE1) as FStr,a.name,a.type1 as code from type a where  a.id='Y' order by fstr";
                        break;

                    case "F25127":
                    case "F25142":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '2%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by fstr";
                        break;
                    case "F50308":
                        multi_opt = "Y";
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and substr(a.acode,1,2)='16' order by fstr";
                        break;

                    case "F50306":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,ivoucher b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and substr(a.acode,1,2)='16' order by fstr";
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
                    case "F39251":
                        squery = "SELECT TRIM(B.TC_NO)  AS FSTR,A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDATE,A.T_GRNO AS BILL_ENTRY_NO,A.T_GRDT AS BILLDATE,B.TC_NO FROM IVCHCTRL A,IVOUCHER B  WHERE A.branchcd='" + frm_mbr + "' AND A.TYPE='07' and A.VCHDATE between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND B.TC_NO!='-'";
                        break;

                    //case "F15142":
                    //    squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst A WHERE SUBSTR(TRIM(A.ACODE),1,2) IN ('05','06') order by a.Aname ";
                    //    break;

                    case "F38501":
                        squery = "SELECT 'TB' AS FSTR,'Printing' as store,'TB' as type from dual union all SELECT 'GB' AS FSTR,'Pigment' as Store,'GB' as type from dual  union all  SELECT 'MB' AS FSTR,'Mixing' as Store,'MB' as type from dual";
                        break;
                    case "F70438":
                    case "F70439":
                    case "F70440":
                    case "F70441":
                        squery = "Select  Type1 as fstr,Type1 as Code,Name as Particulars from TYPEGRP where branchcd !='DD' and id='FA' order by type1";
                        break;

                    ////IAIJ
                    case "F50301":
                        zprd = " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        squery = "SELECT DISTINCT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE ,B.ANAME AS PARTY FROM IVOUCHER A,FAMST B WHERE A.branchcd='" + frm_mbr + "' AND A.TYPE LIKE '4%'  and trim(a.acode)=trim(b.acode) AND A.VCHDATE " + zprd + " ORDER BY VCHNUM DESC";
                        break;
                    case "F50271":
                        zprd = " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        squery = "SELECT DISTINCT TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE ,B.ANAME AS PARTY FROM IVOUCHER A,FAMST B WHERE A.branchcd='" + frm_mbr + "' AND A.TYPE LIKE '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim() + "%'  and trim(a.acode)=trim(b.acode) AND A.VCHDATE " + zprd + " ORDER BY VCHNUM DESC";
                        multi_opt = "Y";
                        break;
                    // ADDED BY MADHVI ON 12TH NOV 2018 QUALITY MODULE
                    case "F30142":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst A,ivoucher b WHERE trim(a.acode)=trim(b.acode) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and b.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and b.inspected='Y' and b.store in ('Y','N') order by Account_Name";
                        break;

                    case "F30143":
                        squery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst A,ivoucher b WHERE trim(a.acode)=trim(b.acode) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and b.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and b.inspected='Y' and b.store ='R' order by Account_Name";
                        break;
                    //------------------------------
                    // ADDED BY MADHVI ON 13TH NOV 2018 PURCHASE MODULE
                    case "F15135":
                        squery = "select DISTINCT trim(a.acode) as FSTR,b.aname as aname,A.Acode,b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from appvendvch a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='10' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                        break;

                    //case "F15140":
                    //    squery = "select DISTINCT trim(a.ICode) as FSTR,a.iname as iname,trim(a.ICode) AS ICODE,trim(a.cpartno) as part, a.unit from wbvu_pending_pr a where a.branchcd='" + frm_mbr + "' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by iname";
                    //    break;
                    case "F15140":
                    case "F15141":
                    case "F15136":
                    case "F15235":
                    case "F15314":
                    case "F15229":
                        squery = "select trim(type1) as fstr,name as main_grp,trim(type1) as code from type where id='Y' order by code";
                        break;

                    case "F15244":
                        squery = "select distinct trim(a.icode) as fstr,b.iname as iname,trim(a.icode) as icode,trim(b.cpartno) as part,b.unit from pomas a,item b where  trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.pflag='1' order by iname";
                        break;

                    case "F15233":
                        squery = "Select distinct trim(a.acode) as fstr,b.aname as aname,a.acode,b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from ivoucherp a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='00' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                        break;

                    case "F15234":
                        squery = "Select distinct trim(a.acode) as fstr,b.aname as aname,a.acode,b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from ivoucher a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '0%' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.store not in ('R','W') order by aname";
                        break;

                    case "F15240":
                        squery = "Select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.qtyord<>a.wk1 and a.wk1<>0 order by aname";
                        break;

                    case "F15239":
                        squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and nvl(trim(a.nxtmth),0)!='0' and a.prate != a.nxtmth and a.app_by !='-' and substr(a.icode,1,2) != '59' order by aname";
                        break;

                    case "F15231":
                    case "F15232":
                    case "F15230":
                    case "F15236":
                    case "F15237":
                    case "F15238":
                    case "F15142":
                    case "F15250":
                        squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode) as acode,b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and  a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                        break;

                    case "F15222":
                    case "F15223":
                    case "F15249":
                    case "F15247":
                    case "F15248":
                    case "F15134":
                        squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode) as acode,b.email,b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from schedule a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='66' and a.vchdate between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') order by aname";
                        if (frm_cocd == "BUPL")
                            squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode) as acode,b.email,b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from schedule a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='66' and a.vchdate between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') AND trim(NVL(A.APP_BY,'-'))!='-' order by aname";
                        multi_opt = "Y";
                        break;

                    case "F25241":
                        squery = "distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                        break;

                    case "F15228":
                        squery = "select distinct trim(a.icode) as fstr,b.iname as iname,trim(a.icode) as icode,trim(b.cpartno) as part,b.unit from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.inspected='Y' and trim(nvl(a.pname,'-'))!='-' and length(Trim(nvl(a.pname,'-')))>1 order by iname";
                        break;

                    case "F15226":
                        squery = "select distinct trim(a.icode) as fstr,b.iname as iname,trim(a.icode) as icode,trim(b.cpartno) as part,b.unit from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' /*and a.podate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.ponum!='000000' and length(Trim(nvl(a.ponum,'-')))>1 order by iname";
                        break;

                    case "F15225":
                        squery = "select distinct trim(a.icode) as fstr,b.iname as iname,trim(a.icode) as icode,trim(b.cpartno) as part,b.unit from pomas a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.pr_dt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and length(Trim(nvl(a.pr_no,'-')))>1 order by iname";
                        break;

                    case "F15227":
                        squery = "select distinct trim(a.icode) as fstr,b.iname as iname,trim(a.icode) as icode,trim(b.cpartno) as part,b.unit from ivoucher a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' /*and a.rtn_date between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and length(Trim(nvl(a.prnum,'-')))>1 order by iname";
                        break;

                    case "F15318":
                        squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.del_date between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                        break;

                    //case "F15229":
                    //    squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                    //    break;

                    case "F15143":
                        squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and  a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/  and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') order by aname";
                        break;

                    case "F15251":
                        squery = "select distinct trim(a.acode) as fstr,B.aname as aname,trim(a.acode),b.Addr1,b.Addr2,b.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.ent_by,b.edt_by from pomas a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='54' /*and  a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by aname";
                        break;
                    //------------------------------

                    case "F40351":
                        squery = "Select distinct trim(a.acode) as fstr,f.aname as customer_name,a.acode as code from inspmst a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='70' order by customer_name";
                        break;
                    case "F49202": //for view Reports of qty and values export
                    case "F49203"://for view Reports of qty and values export
                    case "F49204":
                    case "F49205":
                        squery = "select trim(acode) as fstr ,trim(acode) as acode,trim(aname) as party_name from famst where substr(acode,1,2) in ('16','02') order by acode ";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
                        fgen.Fn_open_mseek("", frm_qstr);
                        break;
                    case "F70141":
                        squery = "SELECT TRIM(TYPE1) AS FSTR,NAME,TYPE1 AS CODE,ADDR1,ADDR2 FROM TYPE WHERE ID='B' ORDER BY TYPE1";
                        multi_opt = "Y";
                        _popHeading = "Select Branch Code";
                        break;
                    case "F15601":// RFQ
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,WB_PORFQ b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '5%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        break;
                    default:
                        if (Prg_Id == "F70281")
                        {
                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 3)
                                cond = "bssch='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                        }
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and a." + cond + " order by a.Aname ";
                        if (Prg_Id == "F70298" || Prg_Id == "F70281") multi_opt = "Y";
                        break;
                    case "F70348":
                        squery = "select trim(acode) as fstr,acode as code,aname as bank_name,addr1 from famst where acode like '12%' and trim(acode) !='120000' order by code";
                        _popHeading = "Select Bank";
                        break;

                    case "F25266":
                        squery = "SELECT TRIM(ICODE) AS FSTR,ICODE AS SUBGRP,INAME AS NAME FROM ITEM WHERE LENGTH(TRIM(ICODE))=4 ORDER BY ICODE";
                        break;

                    case "F70506":
                    case "F70507":
                    case "F70508":
                    case "F70509":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where substr(trim(acode),1,2) in ('05','06','02','16') and  length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname,a.acode";
                        multi_opt = "Y";
                        _popHeading = "Select Customer";
                        break;
                    case "F49149":
                        multi_opt = "Y";
                        squery = "select distinct trim(a.acode) as fstr ,trim(a.acode) as acode,trim(b.aname) as party_name from ivoucherp a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='4F' and a.refdate BETWEEN TO_DATE('" + frm_cDt1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_cDt2 + "','DD/MM/YYYY') order by acode";
                        break;
                    case "F47322":
                        squery = "select distinct trim(a.acode) asfstr,  trim(a.acode) as customer_code,trim(b.aname) as customer_name, trim(b.addr1) as addr1,trim(b.addr2) as addr2 from wb_sorfq a, famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type in ('ER','EC') and orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') order by customer_code";
                        break;
                    case "F30367":
                        squery = "select distinct trim(acode) as fstr,  trim(acode) as customer_code,trim(aname) as customer_name, trim(addr1) as addr1,trim(addr2) as addr2 from famst where length(trim(acode))>'5' and substr(trim(acode),1,2) IN ('16','18')";
                        break;
                    case "F55523":
                        squery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,TRIM(A.ACODE) AS cUSTOMER_CODE ,TRIM(B.ANAME) AS CUSTOMER_NAME FROM WB_EXP_FRT A ,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='04' AND A.TYPE='10' ";
                        multi_opt = "Y";
                        break;

                    case "F50321":
                        mq0 = "";
                        mq1 = Convert.ToDateTime(txtfromdt.Text).ToString("dd/MM/yyyy");
                        mq2 = Convert.ToDateTime(txttodt.Text).ToString("dd/MM/yyyy");
                        mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                        squery = "select distinct TRIM(a.icode) as fstr,trim(a.icode) as erpcode,trim(b.iname) as item_name,b.cpartno,b.unit  from somas a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and substr(trim(a.type),1,1)='4' and a.type!='47' and a.orddt between to_date('" + mq1 + "','dd/mm/yyyy') and to_date('" + mq2 + "','dd/mm/yyyy')  and trim(a.acode) in (" + mq0 + ") order by fstr";
                        multi_opt = "Y";
                        break;

                    #region for customer portal
                    case "F79144":
                    case "F79143":
                        squery = "Select distinct a.icode as fstr,a.icode,b.iname ,b.cpartno from ivoucher a ,item b where trim(a.icode)=trim(b.icode) and a.acode like '" + frm_uname + "%' and a.branchcd!='DD' AND A.TYPE LIKE '4%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "F79145":
                        squery = "select a.icode as fstr,a.icode,b.iname,b.cpartno from wbvu_pending_so a,item b where trim(a.icode)=trim(b.icode) and a.branchcd!='DD' and a.acode LIKE '" + frm_uname + "%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "F79141":
                        squery = "select a.icode as fstr,a.icode,b.iname,b.cpartno from SOMASM a,item b where trim(a.icode)=trim(b.icode) and a.branchcd!='DD' and a.type like '4%' and a.type!='4F' and a.acode LIKE '" + frm_uname + "%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "F79142":
                        squery = "select a.icode as fstr,a.icode,b.iname,b.cpartno from SOMAS a,item b where trim(a.icode)=trim(b.icode) and a.branchcd!='DD' and a.type like '4%' and a.type!='4F' and a.acode LIKE '" + frm_uname + "%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "F50325"://TUNGSTON
                    case "F50326"://honda
                    case "F50330"://CSV REPROT FOR VELVIN
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where substr(trim(acode),1,2)='16' and  length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname,a.acode";
                        break;

                    #endregion

                    case "F39255":
                        squery = "select trim(type1) as fstr,trim(type1) as code,trim(name) as family from typegrp where id='^8' order by code";
                        break;
                    case "F50269":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where substr(trim(acode),1,2)='16' and  length(trim(nvl(a.deac_by,'-'))) <2 order by a.acode,a.aname";
                        multi_opt = "Y";
                        break;

                    case "F70285":
                        squery = "select trim(type1) as fstr,type1 as code,name from type where id='V' and type1 like '4%' order by type1";
                        break;
                    case "F50275":
                        squery = "select trim(type1) as fstr,type1 as main_code,name  from type where id='Y' order by type1";
                        break;

                    case "F25169":
                        squery = "SELECT DISTINCT a.Acode as FStr,trim(A.ACODE) AS VENDOR_CODE,TRIM(B.ANAME) AS VENDOR_NAME FROM RGPMST A ,FAMST B WHERE A.BRANCHCD='03' AND A.TYPE LIKE '2%' AND TRIM(A.ACODE)=TRIM(B.ACODE) ORDER BY A.ACODE";
                        multi_opt = "Y";
                        break;
                    case "F25170":
                        squery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,TRIM(A.ACODE) AS CUSTOMER_CODE,TRIM(B.ANAME) AS CUSTOMER_NAME  FROM SCRATCH A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='PN' ";
                        multi_opt = "Y";
                        break;
                    case "F50273":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a,somas b where trim(A.acode)=trim(B.acodE) and b.branchcd='" + frm_mbr + "' and b.type like '4%' and  length(trim(nvl(a.deac_by,'-'))) <2  order by a.Aname ";
                        multi_opt = "Y";
                        break;

                    case "F25159":
                        squery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,TRIM(A.ACODE) AS cUSTOMER_CODE ,TRIM(B.ANAME) AS CUSTOMER_NAME FROM ivoucher A ,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='08' order by cUSTOMER_CODE";
                        break;

                    case "F25160":
                        squery = "SELECT DISTINCT TRIM(A.ACODE) AS FSTR,TRIM(A.ACODE) AS cUSTOMER_CODE ,TRIM(B.ANAME) AS CUSTOMER_NAME FROM ivoucher A ,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='25' order by cUSTOMER_CODE";
                        break;

                    case "F50279":
                        squery = "SELECT DISTINCT TYPE1 AS FSTR,TYPE1 AS MGCODE,NAME  FROM TYPE WHERE ID='Y' ORDER BY TYPE1";
                        multi_opt = "Y";
                        break;
                    case "F25163":
                        squery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,TYPE1 AS CODE,NAME FROM TYPE WHERE ID='Y' AND TYPE1 LIKE '9%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;
                    case "F25262":
                        squery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vch_date,trim(a.acode) as acode,trIM(A.RCODE) AS ITEM_CODE,TRIM(B.INAME) AS ITEM_NAME,trim(a.freight) as wo_no,sum(a.iqty_chl) as req_qty,sum(a.iqtyout) as issue_qty from ivoucher a ,item b  where trim(a.Rcode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '3%' and a.type!='39' AND A.VCHDATE BETWEEN TO_DATE('" + frm_cDt1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_cDt2 + "','DD/MM/YYYY') group by trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy'),a.type,trim(a.vchnum),to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.freight),trIM(A.RCODE),TRIM(B.INAME)";
                        multi_opt = "Y";
                        break;

                    case "F40063":
                        squery = "select TYPE1 AS FSTR,NAME AS DEPT,TYPE1 AS CODE from type WHERE ID ='M' AND TYPE1 LIKE '6%' ORDER BY TYPE1";//dept qry
                        multi_opt = "Y";
                        break;

                    case "F45136":
                        squery = "select DISTINCT ent_by AS FSTR,ENT_BY From exp_book where branchcd='" + frm_mbr + "' and type='EB' and vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') order by ent_by";
                        break;


                    case "F50323":
                        squery = "select distinct TRIM(A.acode) AS FSTR,TRIM(A.ACODE) AS CUST_CODE,TRIM(B.ANAME) AS CUSTOMER from ivoucher A,FAMST B where A.BRANCHCD='" + frm_mbr + "' AND A.type like '4%' and type!='47' and a.vchdate between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') AND TRIM(A.ACODE)=TRIM(B.ACODE)";
                        multi_opt = "Y";
                        break;


                    case "F50324":
                        squery = "select distinct TRIM(A.acode) AS FSTR,TRIM(A.ACODE) AS CUST_CODE,TRIM(B.ANAME) AS CUSTOMER from somas A,FAMST B where A.BRANCHCD='" + frm_mbr + "' AND A.type like '4%' and type!='47' and a.orddt between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') AND TRIM(A.ACODE)=TRIM(B.ACODE)";
                        multi_opt = "Y";
                        break;

                    case "F50322":
                        squery = "select distinct substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as code,b.name as product from SOMAS a,type b where substr(trim(a.icode),1,2)=trim(b.type1) and b.id='Y' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.orddt between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') order by fstr";
                        multi_opt = "Y";
                        break;

                    case "F50328":
                    case "F50329":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where substr(trim(a.acode),1,2)='16'  and length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname";
                        break;

                    case "F50154":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by from famst a where substr(trim(a.acode),1,2)='16'  and length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname";
                        break;

                    case "F70600":
                    case "F70602":
                        squery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,TYPE1 AS CODE,NAME FROM TYPEGRP WHERE ID='A' AND TYPE1 LIKE '16%' ORDER BY FSTR";
                        break;

                    case "F70604":
                    case "F70606":
                        squery = "SELECT DISTINCT TRIM(TYPE1) AS FSTR,TYPE1 AS CODE,NAME FROM TYPEGRP WHERE ID='A' AND TYPE1 LIKE '06%' ORDER BY FSTR";
                        break;
                    //===============NEW WORKING FOR DLJM
                    //case "RPT1":
                    //     squery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode as Party_Code from famst a where substr(trim(a.acode),1,2)='16'  and length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname";
                    //    multi_opt = "Y";
                    //    break;

                    case "RPT13":
                    case "RPT14":
                    case "RPT17":
                    case "RPT18":
                    case "RPT19":
                    case "RPT21":
                    case "RPT25":
                    case "RPT28":
                    case "RPT1":
                    case "RPT2":
                    case "RPT3":
                    case "RPT5":
                    case "RPT7":
                    case "RPT8":
                    case "RPT9":
                    case "RPT10":
                    case "RPT12":
                    case "RPT22":
                    case "RPT23":
                    case "RPT24":
                        squery = "select TYPE1 AS FSTR,TYPE1 AS sch_code,NAME from typegrp where ID='A' and type1 like '16%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "RPT4":
                    case "RPT6":
                    case "RPT27":
                        squery = "select type1 as fstr,type1 as main_Grp,name from type where id='Y' and type1='93' ORDER BY TYPE1"; //
                        multi_opt = "Y";
                        break;

                    case "RPT11":
                        squery = "select trim(icode) as fstr,trim(icode) as item_code,trim(iname) as item_name from item where length(trim(icode))>=8 and substr(trim(icode),1,2)='93' order by icode desc";
                        multi_opt = "Y";
                        break;

                    case "RPT16":
                    case "RPT20":
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode as Party_Code from famst a where substr(trim(a.acode),1,2)='16'  and length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname";
                        multi_opt = "Y";
                        break;
                    case "RPT15":
                    case "RPT26":
                        squery = "select trim(icode) as fstr,trim(icode) as item_code,trim(iname) as item_name from item where length(trim(icode))>=8 and substr(trim(icode),1,2)='93' order by icode desc";
                        multi_opt = "Y";
                        break;
                    case "F39551":
                        hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_RCOL10");
                        //squery = "SELECT distinct trim(a.lineno),trim(a.lineno) as Line_No,a.icode  FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + txtzcode.Text.Trim() + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' order by trim(a.lineno)";
                        squery = "SELECT distinct trim(a.lineno),trim(a.lineno) as Line_No,trim(b.name) as name FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + hf1.Value + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' order by trim(a.lineno)";
                        break;
                }

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOXS");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
                if (multi_opt == "Y")
                {
                    fgen.Fn_open_mseek(_popHeading, frm_qstr);
                }
                else
                {
                    fgen.Fn_open_sseek(_popHeading, frm_qstr);
                }
                break;


            case "MCODE":
                switch (Prg_Id)
                {
                    case "F70231":
                    case "F70232":
                    case "F70233":
                    case "F70234":
                    case "F70235":
                    case "F70236":
                    case "F70126":
                    case "F70127":
                    case "F70128":
                    case "F70129":
                    case "F70130":
                        squery = "SELECT distinct trim(a.Acode) as FStr,b.AName as Account_Name,a.Acode as Account_Code from Voucher A,Famst b WHERE a.branchcd='" + frm_mbr + "' and a.type in (" + txtacode.Value.Trim() + ") and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and trim(a.acode)=trim(b.acode)  order by B.Aname";
                        break;

                    case "F50277":
                    case "F50276":
                        if (txtacode.Value.Length > 1)
                        { cond = " and SUBSTR(TRIM(A.ICODE),1,2) ='" + txtacode.Value + "'"; }
                        else
                        {
                            cond = " and a.icode like '%'";
                        }
                        squery = "SELECT DISTINCT SUBSTR(TRIM(A.ICODE),1,4) AS FSTR,SUBSTR(TRIM(A.ICODE),1,4) AS SUBCODE,B.INAME FROM IVOUCHER A,ITEM B WHERE SUBSTR(TRIM(A.ICODE),1,4) =TRIM(B.ICODE) AND LENGTH(TRIM(B.ICODE))=4  AND  A.TYPE LIKE '4%' AND a.TYPE!='47' " + cond + " ORDER BY SUBCODE";
                        multi_opt = "Y";
                        break;

                    case "F50278":
                        if (txtacode.Value.Length > 1)
                        { cond = " and SUBSTR(TRIM(A.ICODE),1,2) ='" + txtacode.Value + "'"; }
                        else
                        {
                            cond = " and a.icode like '%'";
                        }
                        squery = "SELECT DISTINCT SUBSTR(TRIM(A.ICODE),1,4) AS FSTR,SUBSTR(TRIM(A.ICODE),1,4) AS SUBCODE,B.INAME FROM IVOUCHER A,ITEM B WHERE SUBSTR(TRIM(A.ICODE),1,4) =TRIM(B.ICODE) AND LENGTH(TRIM(B.ICODE))=4  AND  A.TYPE LIKE '4%' AND a.TYPE!='47' " + cond + " ORDER BY SUBCODE";
                        multi_opt = "Y";
                        break;

                    case "F50386": //ITEM WISE WISE                                     
                        squery = "Select DISTINCT  icode as fstr,iname as item_name,icode,cpartno,unit from item where substr(icode,1,1) like '9%'";
                        multi_opt = "Y";
                        _popHeading = "Select Item/Grp Code";
                        break;

                    case "F50388"://SUBGROUP WISE
                        squery = "Select DISTINCT  substr(trim(a.icode),1,4) as fstr,a.iname as item_name, substr(trim(a.icode),1,4) as sub_code,a.cpartno,a.unit from item  a where substr(a.icode,1,1) like '9%' and length(trim(a.icode))=4 order by sub_code";
                        multi_opt = "Y";
                        _popHeading = "Select Item/Grp Code";
                        break;

                    case "F50390"://MAIN GROUP WISE
                        squery = "SELECT TYPE1 AS FSTR,TYPE1 AS CODE,NAME FROM TYPE WHERE ID='Y'  AND TYPE1 LIKE '9%' ORDER BY TYPE1";
                        multi_opt = "Y";
                        _popHeading = "Select Item/Grp Code";
                        break;



                    case "F95133":
                    case "F95101":
                    case "F95106":
                    case "F90142":
                        squery = "SELECT DISTINCT USERNAME ,USERNAME AS COCD,FULL_NAME AS company_name FROM EVAS WHERE userid<'000060' and NVL(USERNAME,'-')!='-' ORDER BY USERNAME";
                        break;

                    case "F10111":
                    case "F10116":
                    case "F10131":
                    case "F10133":
                    case "F10156":
                    case "F15245":
                    case "F25234":
                    case "F50135":
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
                    case "F25260":
                    case "F25261":

                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as Main_grp_code from type a where a.id='Y' order by a.Type1";
                        break;
                    case "F15101":
                        squery = "SELECT a.type1 as Fstr,A.Name,A.type1 as ItemGroup_code from type a where a.id='Y' order by a.Type1";
                        if (PrgRep_Id == "F15101_3")
                        {
                            if (txtacode.Value.Length > 1)
                            { cond = " and SUBSTR(TRIM(A.ICODE),1,2) ='" + txtacode.Value + "'"; }
                            else
                            {
                                cond = " and a.icode like '%'";
                            }
                            squery = "SELECT trim(a.icode) AS FSTR,TRIM(A.ICODE) AS Sub_Grp_Code,A.INAME as Sub_Grp_Name FROM ITEM A WHERE LENGTH(TRIM(a.ICODE))=4  " + cond + " ORDER BY Sub_Grp_Code";
                            multi_opt = "N";
                            break;
                        }

                        break;
                    case "F47111":
                        squery = "SELECT distinct a.type1 as Fstr,A.Name,A.type1 as Main_grp_code from type a,schedule b where trim(A.type1)=substr(b.icode,1,2) and b.branchcd='" + frm_mbr + "' and b.type='46' and a.id='Y' order by a.Type1";
                        break;

                    case "F47132":
                    case "F47101":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,somasm b where b.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
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
                    case "F30132":
                    case "F30121":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucherp b where b.branchcd='" + frm_mbr + "' and b.type like '0%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F25152":
                    case "F25156":
                    case "F25162":
                    case "F25165":

                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,ivoucher b where b.branchcd='" + frm_mbr + "' and (b.type like '2%' or b.type like '0%') and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
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
                    case "F47136":
                    case "F35107":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,somas b where b.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F47142":
                        multi_opt = "Y";
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

                    case "F70151":
                    case "F70237":
                    case "F70238":
                    case "F70172":
                    case "F70556":
                        multi_opt = "Y";
                        if (txtacode.Value == "")
                        {
                            squery = "SELECT distinct a.Type1 as FStr,a.Name as Schedule_Name,a.Type1 as Sch_Code,a.ent_by,a.edt_by from Typegrp a where a.id='A' and a.branchcd!='DD'  order by a.Type1 ";
                        }
                        else
                        {
                            squery = "SELECT distinct a.Type1 as FStr,a.Name as Schedule_Name,a.Type1 as Sch_Code,a.ent_by,a.edt_by from Typegrp a where a.id='A' and a.branchcd!='DD' and substr(a.type1,1,2) in (" + txtacode.Value + ") order by a.Type1 ";
                        }


                        break;
                    case "F25233": // ITEM REVIEW FORM BY MADHVI
                        squery = "SELECT 'Y' AS FSTR,'STORE' AS REPORT_OPTIONS,'-' AS S FROM DUAL UNION ALL SELECT 'R' AS FSTR,'REJECTION' AS REPORT_OPTIONS,'-' AS S FROM DUAL";
                        break;
                    case "F39251":
                        squery = "SELECT 'YES' AS FSTR,'YES' AS CHOICE,'ALL' AS MESSAGE FROM DUAL UNION ALL SELECT 'NO' AS FSTR,'NO' AS CHOICE,'PENDING' AS MESSAGE FROM DUAL";
                        break;
                    case "F38501":
                        multi_opt = "Y";
                        zprd = " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        squery = "Select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')||trim(a.icode) as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,trim(b.iname) as itemname,to_char(a.vchdate,'yyyymmdd') as vdd from  ivoucherw a ,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and type ='" + txtacode.Value.Trim() + "'  and a.vchdate " + zprd + " order by vdd desc , a.vchnum desc";
                        break;
                    case "F70438":
                    case "F70439":
                    case "F70440":
                    case "F70441":
                        squery = "Select type1 as fstr, type1 as Code, name as Location_name from typegrp where id='LF' order by type1 ";
                        break;
                    case "F50308":
                        multi_opt = "Y";
                        zprd = " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        squery = "select distinct trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode)||trim(invno)||to_char(invdate,'dd/mm/yyyy') as fstr , invno as invoice_no,to_char(invdate,'dd/mm/yyyy') as invoice_date,trim(vchnum) as vchnum ,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + zprd + " and acode in (" + txtacode.Value + ") order by invno  desc";
                        break;
                    case "F50306":
                        zprd = " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        squery = "select distinct trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode) as fstr , invno as invoice_no,to_char(invdate,'dd/mm/yyyy') as invoice_date,trim(vchnum) as vchnum ,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + zprd + " and acode = '" + txtacode.Value + "' order by invno  desc";
                        break;
                    // ADDED BY MADHVI ON 12TH NOV 2018 QUALITY MODULE
                    case "F30142":
                        squery = "SELECT distinct trim(a.icode) as FStr,a.iname as item_Name,a.icode,a.cpartno,a.unit from item A,ivoucher b WHERE trim(a.icode)=trim(b.icode) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and b.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and b.inspected='Y' and b.store in ('Y','N') order by item_name";
                        break;
                    case "F30143":
                        squery = "SELECT distinct trim(a.icode) as FStr,a.iname as item_Name,a.icode,a.cpartno,a.unit from item A,ivoucher b WHERE trim(a.icode)=trim(b.icode) and b.branchcd='" + frm_mbr + "' and b.type like '0%' and b.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and b.inspected='Y' and b.store ='R' order by item_name";
                        break;
                    //------------------------------
                    // ADDED BY MADHVI ON 13TH NOV 2018 PURCHASE MODULE
                    case "F15135":
                        squery = "select DISTINCT trim(a.icode) as FSTR,trim(B.iname) as iname, trim(a.icode) AS ICODE,trim(B.cpartno) as cpartno,B.unit from appvendvch a,ITEM b where trim(a.Icode)=trim(b.Icode) and a.branchcd='" + frm_mbr + "' and a.type='10' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by Iname";
                        break;
                    case "F15140":
                    case "F15141":
                    case "F15136":
                    case "F15235":
                    case "F15314":
                    case "F15229":
                    case "F35228C":
                    case "F35228D":
                        squery = "select trim(icode) as fstr,iname as sub_grp,trim(icode) as code from item where length(trim(icode))=4 order by code";
                        break;

                    case "F15233":
                        squery = "Select DISTINCT trim(a.icode) as FSTR,c.iname as Item_name,A.Icode as Icode,c.cpartno as CPartNo,c.unit as UOM from ivoucherp a,item c where  trim(A.icode)=trim(C.icode) and a.branchcd='" + frm_mbr + "' and a.type='00' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by Item_name";
                        break;

                    case "F15234":
                        squery = "Select DISTINCT trim(a.icode) as FSTR,c.iname as Item_name,a.icode as Item_Code,c.cpartno as CPartNo,c.unit as UOM from ivoucher a,item c where trim(A.icode)=trim(C.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.store  not in ('R','W') order by item_name";
                        break;

                    case "F15240":
                        squery = "Select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and a.qtyord<>a.wk1 and a.wk1<>0 order by item_name";
                        break;

                    case "F15239":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ and nvl(trim(a.nxtmth),0)!='0' and a.prate != a.nxtmth and a.app_by !='-' and substr(a.icode,1,2) != '59' order by item_name";
                        break;

                    case "F15231":
                    case "F15232":
                    case "F15230":
                    case "F15236":
                    case "F15237":
                    case "F15238":
                    case "F15142":
                    case "F15250":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by item_name";
                        break;

                    case "F15222":
                    case "F15223":
                    case "F15249":
                    case "F15247":
                    case "F15248":
                    case "F15134":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from schedule a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type ='66' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by item_name";
                        break;

                    case "F25241":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from ivoucher a,item c where trim(a.acode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' /*and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by item_name";
                        break;

                    case "F15318":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and a.del_date between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/ order by item_name";
                        break;

                    //case "F15229":
                    //    squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' order by item_name";
                    //    break;

                    case "F15143":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' /*and  a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/  and trim(a.pflag)!=1 and (trim(a.chk_by)!='-' or trim(a.app_by)!='-') order by item_name";
                        break;

                    case "F15251":
                        squery = "select distinct trim(a.icode) as fstr,trim(C.iname) as item_name,trim(a.icode) as icode,trim(c.cpartno) as cpartno,c.unit from pomas a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='54' /*and  a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')*/  order by item_name";
                        break;
                    //------------------------------
                    case "F25232":
                        squery = "SELECT distinct trim(a.Icode) as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where length(Trim(a.icode))=4 and substr(trim(a.icode),1,2)='" + txtacode.Value + "' order by FStr";
                        break;

                    case "F40351":
                        squery = "Select distinct trim(a.icode) as fstr,i.iname as item_name,a.icode as code from inspmst a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='70' order by item_name";
                        break;

                    case "F49202": //for view Reports of qty and values export
                    case "F49203"://for view Reports of qty and values export
                    case "F49204":
                    case "F49205":
                        squery = "select country as fstr, country from famst where substr(acode,1,2) in ('16','02') and length(trim(country))>1 order by fstr";
                        break;
                    case "F70141":
                        multi_opt = "Y";
                        _popHeading = "Select Party";
                        squery = "SELECT TRIM(ACODE) AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2,GST_NO as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + " FROM FAMST  ORDER BY ACODE";
                        break;
                    case "F15601":// rfq
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,WB_PORFQ b where b.branchcd='" + frm_mbr + "' and b.type like '5%' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                    case "F70348":
                    case "F15211":
                        multi_opt = "Y";
                        _popHeading = "Select Party";
                        squery = "SELECT DISTINCT trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by FROM famst a,voucher b where trim(a.acode)=trim(b.acode) and b.branchcd='" + frm_mbr + "' and b.type like '1%' AND substr(trim(A.ACODE),1,2) in ('02','05','06','16') order by a.acode,Account_Name";
                        if (Prg_Id == "F15211")
                            squery = "SELECT DISTINCT trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.ent_by,a.edt_by FROM famst a,pomas b where trim(a.acode)=trim(b.acode) and b.branchcd='" + frm_mbr + "' and b.type like '5%' AND substr(trim(A.ACODE),1,2) in ('02','05','06') order by a.acode,Account_Name";
                        break;

                    case "F70506":
                    case "F70507":
                    case "F70508":
                    case "F70509":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 and substr(trim(a.icode),1,2)>='7' order by FSTR";
                        //squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 and substr(trim(a.icode),1,2)<'7' order by FSTR";
                        squery = "select distinct a.type1 as FStr,a.type1 as code,a.name as Main_gp_Name from type a where a.id='Y' order by a.type1";
                        _popHeading = "Select Main Item Group";
                        multi_opt = "Y";
                        break;
                    case "F25266":
                        squery = "SELECT TRIM(ICODE) AS FSTR,ICODE AS ITEM_CODE,INAME AS NAME FROM ITEM WHERE SUBSTR(TRIM(ICODE),0,4)='" + txtacode.Value + "' AND LENGTH(TRIM(ICODE))=8 ORDER BY ICODE";
                        multi_opt = "Y";
                        _popHeading = "Select Item";
                        break;
                    case "F49149":
                        zprd = " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                        //squery = "select distinct a.tc_no ,a.tc_no as inv_no , to_char(a.refdate,'dd/mm/yyyy') as invdate,a.acode as customer_code ,trim(b.aname) as customer_name  from ivoucherp a,famst b  where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='4F' and a.refdate "+zprd+" and trim(a.acode) in (" + txtacode.Value + ")";
                        squery = "select distinct trim(a.vchnum) as fstr, trim(a.vchnum) as invoice_no,a.tc_no as Refrence_no , to_char(a.refdate,'dd/mm/yyyy') as invdate,a.acode as customer_code ,trim(b.aname) as customer_name  from ivoucherp a,famst b  where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='4F' and a.refdate " + zprd + " and trim(a.acode) in (" + txtacode.Value + ")";
                        multi_opt = "Y";
                        _popHeading = "Select Invoice";
                        break;

                    case "F30367":
                        squery = "Select Distinct a.org_invno as fstr,a.ordno as order_no,to_char(a.orddt,'dd/mm/yyyy') as order_dt,a.org_invno as WO_NO,a.acode,a.work_ordno as project,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b where trim(a.acodE)=trim(b.acodE) and  a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='4' and trim(a.acode)='" + txtacode.Value + "' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by wo_no desc";
                        multi_opt = "Y";
                        _popHeading = "Select WO";
                        break;

                    case "F50325":
                    case "F50326":
                    case "F50330"://CSV REPROT FOR VELVIN
                        squery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as invno,to_char(a.vchdate,'dd/mm/yyyy') as inv_date from ivoucher a where a.branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and type!='4F' and a.vchdate between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and trim(a.acode) like '" + txtacode.Value + "%' order by invno asc";
                        multi_opt = "Y";
                        break;

                    case "F50321":
                        mq0 = "";
                        mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                        mq1 = Convert.ToDateTime(txtfromdt.Text).ToString("dd/MM/yyyy");
                        mq2 = Convert.ToDateTime(txttodt.Text).ToString("dd/MM/yyyy");
                        if (txtacode.Value.Length > 2)
                        { cond = " and trim(icode) in (" + txtacode.Value + ")"; }
                        else
                        {
                            cond = " and substr(trim(icode),1,1)='9'";
                        }
                        squery = "select distinct trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr, ordno ,to_char(orddt,'dd/mm/yyyy') as orddt from somas where branchcd='" + frm_mbr + "' and substr(trim(type),1,1)='4' and type!='47' and orddt between to_date('" + mq1 + "','dd/mm/yyyy') and to_date('" + mq2 + "','dd/mm/yyyy') AND trim(ACODE) in (" + mq0 + ") " + cond + "";
                        multi_opt = "Y";
                        break;

                    case "F39255":
                        multi_opt = "Y";
                        cond = "";
                        if (txtacode.Value.Trim().Length > 0)
                        {
                            cond = " and i.bfactor='" + txtacode.Value.Trim() + "'";
                        }
                        squery = "select distinct trim(a.icode) as fstr,trim(a.icode) as code,trim(i.iname) as item from multivch a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='RR' and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') " + cond + " order by code";
                        break;

                    case "F70285":
                        multi_opt = "Y";
                        //cond = "";
                        //if (txtacode.Value.Trim().Length > 0)
                        //{
                        //    cond = " and a.type='" + txtacode.Value.Trim() + "'";
                        //}
                        squery = "select distinct trim(a.branchcd)||trim(a.type)||TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') as fstr,trim(a.ordno) as so_no,TO_CHAR(A.orddt,'DD/MM/YYYY') as dated,a.acode as customer_code,f.aname as customer,to_char(a.orddt,'yyyymmdd') as vdd from somasq a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.orddt between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and a.type='" + txtacode.Value.Trim() + "' order by vdd desc,so_no desc";
                        break;

                    case "F50275":
                        if (txtacode.Value.Length <= 1)
                        {
                            cond = "and substr(trim(b.icode),1,2) like '%'";
                        }
                        else
                        {
                            cond = "and substr(trim(b.icode),1,2) ='" + txtacode.Value + "'";
                        }
                        squery = "select distinct trim(b.icode) as fstr, b.icode ,b.iname as subgroup_name from ivoucher a , item b where trim(substr(a.icode,1,4))=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and type!='47' and length(trim(b.icode))=4 " + cond + " order by icode";
                        multi_opt = "Y";
                        _popHeading = "Select Sub Group";
                        break;
                    case "F50273":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a,somas b where b.branchcd='" + frm_mbr + "' and trim(A.icode)=trim(B.icode) and length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        multi_opt = "Y";
                        break;

                    case "F25159":
                        squery = "SELECT distinct TRIM(a.ICODE) AS FSTR,a.ICODE AS ITEM_CODE,i.INAME AS NAME FROM ivoucher a, ITEM i WHERE trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='08'  ORDER BY ITEM_CODE";
                        break;

                    case "F25160":
                        squery = "SELECT distinct TRIM(a.ICODE) AS FSTR,a.ICODE AS ITEM_CODE,i.INAME AS NAME FROM ivoucher a, ITEM i WHERE trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type IN ('13','25') ORDER BY ITEM_CODE";
                        break;

                    case "F50279":
                        if (txtacode.Value.Length > 1)
                        { cond = " and SUBSTR(TRIM(A.ICODE),1,2) in (" + txtacode.Value + ")"; }
                        else
                        {
                            cond = " and a.icode like '%'";
                        }
                        squery = "SELECT DISTINCT SUBSTR(TRIM(A.ICODE),1,4) AS FSTR,SUBSTR(TRIM(A.ICODE),1,4) AS SUBCODE,B.INAME FROM IVOUCHER A,ITEM B WHERE SUBSTR(TRIM(A.ICODE),1,4) =TRIM(B.ICODE) AND LENGTH(TRIM(B.ICODE))=4  AND  A.TYPE LIKE '4%' AND a.TYPE!='47' " + cond + " ORDER BY SUBCODE";
                        multi_opt = "Y";
                        break;
                    case "F25163":
                        if (txtacode.Value.Length > 1)
                        { cond = " and SUBSTR(TRIM(ICODE),1,2) in (" + txtacode.Value + ")"; }
                        else
                        {
                            cond = " and icode like '%'";
                        }
                        squery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,ICODE AS CODE,INAME FROM ITEM WHERE LENGTH(TRIM(ICODE))=4 " + cond + "  ORDER BY FSTR";
                        multi_opt = "Y";
                        break;


                    case "F40063":
                        squery = "SELECT TRIM(MCHCODE) AS FSTR,MCHNAME AS MACH_NAME,MCHCODE AS MCH_CODE,ACODE AS SECTION_CODE,VCHNUM  FROM PMAINT WHERE BRANCHCD='" + frm_mbr + "' and vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') AND NVL(trim(VCHNUM),'-')!='-' ORDER BY MCHCODE ";//MACHINE POPUP
                        multi_opt = "Y";
                        break;

                    case "F50323":
                        squery = "select distinct substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as code,b.name as product from ivoucher a,type b where substr(trim(a.icode),1,2)=trim(b.type1) and b.id='Y' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.vchdate between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') order by fstr";
                        multi_opt = "Y";
                        break;

                    case "F50324":
                        squery = "select distinct substr(trim(a.icode),1,2) as fstr,substr(trim(a.icode),1,2) as code,b.name as product from SOMAS a,type b where substr(trim(a.icode),1,2)=trim(b.type1) and b.id='Y' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.orddt between to_date('" + Convert.ToDateTime(txtfromdt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and to_date('" + Convert.ToDateTime(txttodt.Text).ToString("yyyy-MM-dd") + "','yyyy-MM-dd') order by fstr";
                        multi_opt = "Y";
                        break;

                    case "F50328":
                    case "F50329":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where substr(trim(a.icode),1,1)='9' and   length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.iname";
                        break;

                    case "F50154":
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where substr(trim(a.icode),1,1)='9' and   length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.iname";
                        break;
                    case "F70600":
                    case "F70602":
                        squery = "SELECT DISTINCT TRIM(ACODE) AS FSTR,ANAME,Acode,BSSCH FROM famst WHERE acode like '16%' and SUBSTR(TRIM(bssch),1,4) like '" + txtacode.Value + "%'  ORDER BY FSTR";
                        break;

                    case "F70604":
                    case "F70606":
                        squery = "SELECT DISTINCT TRIM(ACODE) AS FSTR,ANAME,Acode,BSSCH FROM famst WHERE acode like '06%' and SUBSTR(TRIM(bssch),1,4) like '" + txtacode.Value + "%'  ORDER BY FSTR";
                        break;


                    ///===========================DLJM

                    case "RPT2":
                    case "RPT3":
                    case "RPT5":
                    case "RPT7":
                    case "RPT8":
                    case "RPT9":
                    case "RPT10":
                    case "RPT12":
                    case "RPT22":
                    case "RPT23":
                    case "RPT24":
                        //squery = "select DISTINCT TYPE1 AS FSTR,TYPE1 AS CODE,NAME from type where id='V' AND TYPE1 LIKE '4%' ORDER BY FSTR";
                        squery = "select DISTINCT a.TYPE AS FSTR,a.TYPE AS CODE,b.NAME from sale a,type b where trim(a.type)=trim(b.type1) and b.id='V' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')  ordER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "RPT1":
                    case "RPT13":
                    case "RPT14":
                    case "RPT17":
                    case "RPT18":
                    case "RPT19":
                    case "RPT21":
                    case "RPT25":
                    case "RPT28":
                        if (txtacode.Value.Length > 1)
                        {
                            cond = " a.bssch in (" + txtacode.Value + ")";
                        }
                        else
                        {
                            cond = "a.acode like '16%'";
                        }
                        squery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode as Party_Code from famst a where " + cond + " and length(trim(nvl(a.deac_by,'-'))) <2 order by a.Aname";
                        multi_opt = "Y";
                        break;

                    case "RPT4":
                    case "RPT6":
                    case "RPT11":
                    case "RPT15":
                    case "RPT16":
                    case "RPT20":
                    case "RPT26":
                    case "RPT27":
                        squery = "select TYPE1 AS FSTR,TYPE1 AS sch_code,NAME from typegrp where ID='A' and type1 like '16%' ORDER BY FSTR";
                        multi_opt = "Y";
                        break;

                    case "F39551":
                        mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_RCOL10");
                        squery = "SELECT distinct trim(a.icode) as fstr,trim(c.cpartno) as Cpartno,trim(c.iname) as desc_,trim(a.icode) as icode  FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + hf1.Value + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' and a.lineno='" + txtacode.Value + "'  order by trim(c.cpartno)";
                        break;

                    default:
                        squery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Cpartno,a.Cdrgno,a.Unit,a.ent_by,a.edt_by from Item a where  length(trim(nvl(a.deac_by,'-'))) <2  and length(Trim(a.icode))>4 order by a.Iname ";
                        break;
                }
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOXS");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", squery);
        if (multi_opt == "Y")
        {
            fgen.Fn_open_mseek(_popHeading, frm_qstr);
        }
        else
        {
            fgen.Fn_open_sseek(_popHeading, frm_qstr);
        }

    }
    protected void btniBox_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "PMCODE":
                txtacode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                break;

            case "MCODE":
                txticode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                break;
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }
    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        PrgRep_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMREPID");

        if (Prg_Id != "F50321" && Prg_Id != "F70298")
        {
            if (Convert.ToDateTime(frm_cDt1) > Convert.ToDateTime(txtfromdt.Text) || Convert.ToDateTime(frm_cDt2) < Convert.ToDateTime(txtfromdt.Text)
               || Convert.ToDateTime(frm_cDt2) < Convert.ToDateTime(txttodt.Text) || Convert.ToDateTime(frm_cDt1) > Convert.ToDateTime(txttodt.Text))
            {
                fgen.msg("-", "AMSG", "Please Select Date Range with in Current Financial Year.");
                return;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", txtacode.Value.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", txticode.Value.Trim());

        if (Prg_Id == "F25266")
        {
            if (txtacode.Value.Trim().Length < 4 || txticode.Value.Trim().Length < 6)
            {
                fgen.msg("-", "AMSG", "Please Select Sub Group First And Then Item!!"); return;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT1", Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MDT2", Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy"));
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PRDRANGE", " between to_date('" + Convert.ToDateTime(txtfromdt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DAYRANGE", " between to_date('01/" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("MM/yyyy") + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(txttodt.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')");

        if (rdPDF.SelectedValue == "0") Value1 = "Y";
        else Value1 = "N";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", Value1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
        fgen.fin_acct_reps(frm_qstr);
        //switch (HCID)
        //{
        //    case "FINSYS**":
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
        //        break;
        //    default:
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
        //        break;
        //}
    }
    protected void btnMcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MCODE";
        makequery4popup();
    }
    protected void btnPmcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PMCODE";
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