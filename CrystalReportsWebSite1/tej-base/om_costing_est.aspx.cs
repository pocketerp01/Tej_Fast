using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

public partial class om_costing_est : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0;

    DataTable sg1_dt; DataRow sg1_dr;

    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string a,b,c;
    fgenDB fgen = new fgenDB();


    //variables declared by twinkle as per requirement , want to save values of that variables into table.
    string frm_tabname1, frm_tabname2, SQuery1, SQuery2;
    double z, ReqGSM, Deckle, Length, Area;
    double ReqBs, ReqECT;
    double totalcost = 0, totaltrct = 0;
    int caliper = 0;


    // calculate TOP PLY LAYER

    double TGSM = 0, TBF = 0, TRCTGrade = 0;
    double THRCT = 0, TNRCT = 0, TRCT, TTF = 1, T_RCT;
    double TCorrectIndex = 0;
    double TRateperkg_h=0, TRateperkg_N=0, TCorrectPaperRate, TTop_factor, TCostpertop;

    //CALCULATION FLUTE 1
    double F1GSM = 0, F1BF = 0, F1RCTGrade = 0;
    double F1HRCT = 0, F1NRCT = 0, F1RCT, F1TF = 0, F1_RCT;
    double F1CorrectIndex = 0;
    double F1Rateperkg_h=0, F1Rateperkg_N=0, F1CorrectPaperRate, F1Top_factor, F1Costpertop;

    //CALCULATION LINER 1
    double L1GSM = 0, L1BF = 0, L1RCTGrade = 0;
    double L1HRCT = 0, L1NRCT = 0, L1RCT, L1TF = 1, L1_RCT;
    double L1CorrectIndex = 0;
    double L1Rateperkg_h=0, L1Rateperkg_N=0, L1CorrectPaperRate, L1Top_factor, L1Costpertop;

    //CALCULATION FLUTE2
    double F2GSM = 0, F2BF = 0, F2RCTGrade = 0;
    double F2HRCT = 0, F2NRCT = 0, F2RCT, F2TF = 0, F2_RCT;
    double F2CorrectIndex = 0;
    double F2Rateperkg_h=0, F2Rateperkg_N=0, F2CorrectPaperRate, F2Top_factor, F2Costpertop;


    //CALCULATION LINER2

    double L2GSM = 0, L2BF = 0, L2RCTGrade = 0;
    double L2HRCT = 0, L2NRCT = 0, L2RCT, L2TF = 1, L2_RCT;
    double L2CorrectIndex = 0;
    double L2Rateperkg_h, L2Rateperkg_N, L2CorrectPaperRate, L2Top_factor, L2Costpertop;
    //4 th grid
    double l_w_ratio, l_w_factor = 0, net_factor, min_ect, max_ect;
    double avg_ect, min_cs, max_cs, avg_cs, min_gsm, max_gsm, avg_gsm, min_bs, max_bs, avg_bs, min_wt, max_wt, avg_wt;
    double depthfactor;
    // last grid

    double StarchGumRate = 0, StarchGumYN = 0, StarchGumAmt, PVAGumRate = 0, PVAGumYN = 0, PVAGumAmt = 0;
    double PowerRate = 0, PowerYN = 0, PowerAmt = 0, FuelRate = 0, FuelYN = 0, FuelAmt = 0;

    double StichingPinsRate = 0, StichingPinsYN = 0, StichingPinsAmt = 0, PrintingInkRate = 0, PrintingInkYN = 0, PrintingInkAmt = 0;
    double LaborRate = 0, LaborYN = 0, LaborAmt = 0, AdministrativeRate = 0, AdministrativeYN = 0, AdministrativeAmt = 0;
    double TransportationRate = 0, TransportationYN = 0, TransportationAmt = 0, OtherMaterialsRate = 0, OtherMaterialsYN = 0, OtherMaterialsAmt = 0;
    double Contribution = 0, TotalConversionCost = 0, ConversionCostperkg = 0, PaperCost = 0, PaperWastage = 0, BoxIncost = 0;

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
                    lbl1a_Text = "CS";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
              

                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                lblheader.Text = "Box Cost Estimate(B/C/BC)";
                fgen.DisableForm(this.Controls);
                enablectrl();

               
            }
            set_Val();
         
            if (frm_ulvl != "0")
            {
                btndel.Visible = false;
            }
            if (CSR.Length > 1 || frm_ulvl == "3")
            {


            }
            
        }
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btncal.Enabled = false; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;
    }

    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btncal.Enabled = true;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
        btndel.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        btncancel.Visible = true;

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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_CORRCST_TRANS";
        frm_tabname1 = "WB_CORRCST_LAYER";
        frm_tabname2 = "WB_CORRCST_CONVC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "B");
        typePopup = "N";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", frm_tabname1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", frm_tabname2);


    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");

        btnval = hffield.Value;
        if (frm_ulvl == "3") cond = " and trim(a.ENT_BY)='" + frm_uname + "'";
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR.Trim() + "'";
        switch (btnval)
        {
            case "PLYBUT":
                SQuery = "select '3' as fstr,'3' as PLY1,'3' as PLY  from dual union all select '5' as fstr,'5' as PLY1,'5' as PLY  from dual ";
                break;
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" )
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                SQuery = "SELECT DISTINCT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,VCHNUM AS ENTRY_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,Aname as cust_name , iname as item_name FROM WB_CORRCST_TRANS WHERE BRANCHCD='" + frm_mbr + "' ORDER BY VCHNUM DESC ";
                
            if (btnval == "Print_E")
                    SQuery = "select distinct trim(branchcd)||trim(trannum) as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.aname,a.iname from wb_corrcst_trans a where a.branchcd='" + frm_mbr + "' and vchdate " + DateRange + " order by entry_no desc";
                    break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print"))
        {
            btnval = btnval + "_E";
            hffield.Value = btnval;
            make_qry_4_popup();
        }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            if (typePopup == "N")
                newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }

            if (frm_ulvl == "3")
            {
                
            }

        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
      if (col1 == "") return;
      frm_vty = vty;
      string mq1 = "";
       mq1 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE  branchcd='" + frm_mbr + "'";
      frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq1, 6, "VCH");
      txtVchnum.Value = frm_vnum;
      txtVchnum.Value = fgen.next_no(frm_qstr, frm_cocd, "select max(code) as vch from WB_CORRCST_TRANS WHERE  branchcd='" + frm_mbr + "'", 6, "VCH");
      txtVchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

      todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

      //if (Convert.ToDateTime(txtVchdate.Value) > Convert.ToDateTime(todt))
      //{
          txtVchdate.Value = vardate;
      

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fillindex();

        disablectrl();
        fgen.EnableForm(this.Controls);
     
        #endregion
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
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        fgen.fill_dash(this.Controls);

        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txtL.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "length";
        }
        if (txtWid.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "width";
        }

        if (txtHeight.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "height";
        }

        if (txtPly.Value.Trim().Length < 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "ply";
        }
        if (txtFlute.Value.Trim().Length < 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Flute";
        } 
        if (txtCs.Value.Trim().Length < 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "CS";
        }
        if (txtBoxCost.Value.Trim().Length < 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "calculate Box Cost by entering correct masters";
        } 

        if (txtCustomer.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Customer";
        }
        if (txtItem.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "ItemName";
        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        if ((txtBoxCost.Value.Trim() == "") || (txtBoxCost.Value.Trim() == "-"))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  Please press the calculate button first to calculate cost.");
            return;
        }
      
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + "  for Print", frm_qstr);
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
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
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
                case "New":
                    newCase(col1);
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "CS";
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "CS";
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
                    break;
                case "Del_E":
                    if (col1 == "") return;
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    
                    #region Edit Start
                    fillindex();
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string mv_col,mv_col1,mv_col3;
                    mv_col =col1 ;
                    mv_col3=frm_mbr+col1;
                    mv_col1 = col2;
                    //SQuery = "Select a.* from " + frm_tabname + " a where TRIM(A.TYPE1)='" + mv_col + "' and id='B'";
                    SQuery = "SELECT A.* FROM " + frm_tabname + " A WHERE  TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + mv_col + "'";
                    SQuery1 = "SELECT A.* FROM " + frm_tabname1 + " A WHERE  TRIM(a.tranNUM)||TO_CHAR(a.tranDt,'DD/MM/YYYY')='" + mv_col3 + "' order by srno";
                    SQuery2 = "SELECT distinct A.* FROM " + frm_tabname2 + " A WHERE  TRIM(a.tranNUM)||TO_CHAR(a.trandt,'DD/MM/YYYY')='" + mv_col3 + "' order by srno";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtVchdate.Value = Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString()).ToString("dd/MM/yyyy");
                        txtCustomer.Value = dt.Rows[i]["aname"].ToString();
                        txtItem.Value = dt.Rows[i]["iname"].ToString();
                        txtL.Value = dt.Rows[i]["lt"].ToString();
                        txtWid.Value = dt.Rows[i]["wd"].ToString();
                        txtHeight.Value = dt.Rows[i]["ht"].ToString();
                        txtPly.Value = dt.Rows[i]["ply"].ToString();
                        txtFlute.Value = dt.Rows[i]["flute"].ToString();
                        txtCs.Value = dt.Rows[i]["cs"].ToString();
                        txtRateContri.Value = dt.Rows[i]["contribution"].ToString();
                        txtRatePaperWst.Value = dt.Rows[i]["pawastage"].ToString();
                        txtVchnum.Value = mv_col1;
                        txtCaliper.Value = dt.Rows[i]["caliper"].ToString();
                        txtZ.Value = dt.Rows[i]["Z"].ToString();
                        txtECT.Value = dt.Rows[i]["RQECT"].ToString();
                        txtBS.Value = dt.Rows[i]["RQBS"].ToString();
                        txtGSM.Value = dt.Rows[i]["RQGSM"].ToString();
                        txtDeckle.Value = dt.Rows[i]["DECKLE"].ToString();
                        txtLength.Value = dt.Rows[i]["LENGTH"].ToString();
                        txtArea.Value = dt.Rows[i]["AREA"].ToString();
                        txtBoxCost.Value = dt.Rows[i]["BOXCOST"].ToString();
                        txtAmtContri.Value = dt.Rows[i]["CONTAMT"].ToString();
                        txtAmtConvCostperkg.Value = dt.Rows[i]["CSTPKG"].ToString();
                        txtAmtTotalConv.Value = dt.Rows[i]["TCONCST"].ToString();

                        txtAmtPapercost.Value = dt.Rows[i]["PAPCST"].ToString();
                        txtAmtPaperWst.Value = dt.Rows[i]["PAWASTAGEAMT"].ToString();
                        txtRemarks.Value = dt.Rows[i]["rem"].ToString();

                        //getting bus rate from WB_CORRCST_TRANS table

                        txthighRCTRatea.Value = dt.Rows[i]["h_16"].ToString();
                        txtNormalRCTRatea.Value = dt.Rows[i]["n_16"].ToString();
                        txthighRCTRateb.Value = dt.Rows[i]["h_18"].ToString();
                        txtNormalRCTRateb.Value = dt.Rows[i]["n_18"].ToString();
                        txthighRCTRatec.Value = dt.Rows[i]["h_20"].ToString();
                        txtNormalRCTRatec.Value = dt.Rows[i]["n_20"].ToString();
                        txthighRCTRated.Value = dt.Rows[i]["h_22"].ToString();
                        txtNormalRCTRated.Value = dt.Rows[i]["n_22"].ToString();
                        txthighRCTRatee.Value = dt.Rows[i]["h_24"].ToString();
                        txtNormalRCTRatee.Value = dt.Rows[i]["n_24"].ToString();
                        txthighRCTRatef.Value = dt.Rows[i]["h_28"].ToString();
                        txtNormalRCTRatef.Value = dt.Rows[i]["n_28"].ToString();
                        txthighRCTRateg.Value = dt.Rows[i]["h_35"].ToString();
                        txtNormalRCTRateg.Value = dt.Rows[i]["n_35"].ToString();
                        txthighRCTRateh.Value = dt.Rows[i]["h_45"].ToString();
                        txtNormalRCTRateh.Value = dt.Rows[i]["n_45"].ToString();

                        txtwghtmin.Value = dt.Rows[i]["MINWT"].ToString();
                        txtwghtmax.Value = dt.Rows[i]["MAXWT"].ToString();
                        txtwghtavg.Value = dt.Rows[i]["AVGWT"].ToString();
                        txtBSmin.Value = dt.Rows[i]["MINBS"].ToString();
                        txtBSmax.Value = dt.Rows[i]["MAXBS"].ToString();
                        txtBSavg.Value = dt.Rows[i]["AVGBS"].ToString();
                        txtGSMmin.Value = dt.Rows[i]["MINGSM"].ToString();
                        txtGSMmax.Value = dt.Rows[i]["MAXGSM"].ToString();

                        txtGSMavg.Value = dt.Rows[i]["AVGGSM"].ToString();
                        txtECTmin.Value = dt.Rows[i]["MINECT"].ToString();
                        txtECTmax.Value = dt.Rows[i]["MAXECT"].ToString();
                        txtECTavg.Value = dt.Rows[i]["AVGECT"].ToString();


                        txtCSmin.Value = dt.Rows[i]["MINCS"].ToString();
                        txtCSmax.Value = dt.Rows[i]["MAXCS"].ToString();
                        txtCSavg.Value = dt.Rows[i]["AVGCS"].ToString();

                    }

                        if (dt3.Rows.Count > 0)
                        { 
                          for (int i=0; i<dt3.Rows.Count;i++)
                        {
                          
                           if(dt3.Rows[i]["srno"].ToString()=="00")
                         {

                             txtGSM1.Value = dt3.Rows[i]["gsm"].ToString();
                             txtBF1.Value = dt3.Rows[i]["bf"].ToString();
                             txtRCTGrade1.Value = dt3.Rows[i]["rctgrade"].ToString();
                             txtRCT1.Value = dt3.Rows[i]["rct"].ToString();
                             txtTRCT1.Value = dt3.Rows[i]["t_rct"].ToString();
                             txtCost1.Value = dt3.Rows[i]["cost"].ToString();



                         }
                           else if (dt3.Rows[i]["srno"].ToString()=="01")
                         {

                             txtGSM2.Value = dt3.Rows[i]["gsm"].ToString();
                             txtBF2.Value = dt3.Rows[i]["bf"].ToString();
                             txtRCTGrade2.Value = dt3.Rows[i]["rctgrade"].ToString();
                             txtRCT2.Value = dt3.Rows[i]["rct"].ToString();
                             txtTRCT2.Value = dt3.Rows[i]["t_rct"].ToString();
                             txtCost2.Value = dt3.Rows[i]["cost"].ToString();
                             txtTRCTtot.Value= dt3.Rows[i]["totrct"].ToString();
                             txtCosttot.Value = dt3.Rows[i]["totcost"].ToString();
                           
                           
                         }
                         else if (dt3.Rows[i]["srno"].ToString()=="02")
                          {


                              txtGSM3.Value = dt3.Rows[i]["gsm"].ToString();
                              txtBF3.Value = dt3.Rows[i]["bf"].ToString();
                              txtRCTGrade3.Value = dt3.Rows[i]["rctgrade"].ToString();
                              txtRCT3.Value = dt3.Rows[i]["rct"].ToString();
                              txtTRCT3.Value = dt3.Rows[i]["t_rct"].ToString();
                              txtCost3.Value = dt3.Rows[i]["cost"].ToString();
                         
                         
                         }
                         else if (dt3.Rows[i]["srno"].ToString()=="03")
                         {

                             txtGSM4.Value = dt3.Rows[i]["gsm"].ToString();
                             txtBF4.Value = dt3.Rows[i]["bf"].ToString();
                             txtRCTGrade4.Value = dt3.Rows[i]["rctgrade"].ToString();
                             txtRCT4.Value = dt3.Rows[i]["rct"].ToString();
                             txtTRCT4.Value = dt3.Rows[i]["t_rct"].ToString();
                             txtCost4.Value = dt3.Rows[i]["cost"].ToString();
                              
                              
                         }
                         else if (dt3.Rows[i]["srno"].ToString() == "04")
                         {
                             txtGSM5.Value = dt3.Rows[i]["gsm"].ToString();
                             txtBF5.Value = dt3.Rows[i]["bf"].ToString();
                             txtRCTGrade5.Value = dt3.Rows[i]["rctgrade"].ToString();
                             txtRCT5.Value = dt3.Rows[i]["rct"].ToString();
                             txtTRCT5.Value = dt3.Rows[i]["t_rct"].ToString();
                             txtCost5.Value = dt3.Rows[i]["cost"].ToString();
                         
                         }
                        }

                        }

                        if (dt2.Rows.Count > 0)
                        {
                         for (int j=0;j<dt2.Rows.Count;j++)
                         {
                             if (dt2.Rows[j]["srno"].ToString() == "00")
                             {

                             txtRateStrch.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNStrch.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtStrch.Value = dt2.Rows[j]["Amt"].ToString();


                             }

                             else if(dt2.Rows[j]["srno"].ToString() == "01")
                             {
                              txtRatePVA.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNPVA.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtPVA.Value = dt2.Rows[j]["Amt"].ToString();
                             
                             }

                             else if(dt2.Rows[j]["srno"].ToString() == "02")
                             {
                              txtRatePow.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNPow.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtPow.Value = dt2.Rows[j]["Amt"].ToString();
                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "03")
                             {
                             txtRateFuel.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNFuel.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtFuel.Value = dt2.Rows[j]["Amt"].ToString();
                             
                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "04")
                             {
                              txtRateStchPins.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNStchPins.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtStchPins.Value = dt2.Rows[j]["Amt"].ToString();
                             
                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "05")
                             {
                             txtRatePrint.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNPrint.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtPrint.Value = dt2.Rows[j]["Amt"].ToString();

                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "06")
                             {
                              txtRatelabor.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNlabor.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtlabor.Value = dt2.Rows[j]["Amt"].ToString();
                             
                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "07")
                             {
                              txtRateAdmin.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNAdmin.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtAdmin.Value = dt2.Rows[j]["Amt"].ToString();
                             
                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "08")
                             {
                              txtRateTrans.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNTrans.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtTrans.Value = dt2.Rows[j]["Amt"].ToString();
                             
                             }
                             else if(dt2.Rows[j]["srno"].ToString() == "09")
                             {
                             
                             txtRateOtherM.Value = dt2.Rows[j]["Rate"].ToString();
                             txtYNOtherM.Value = dt2.Rows[j]["flag"].ToString();
                             txtAmtOtherM.Value = dt2.Rows[j]["Amt"].ToString();
                            
                             }
                        
                         }
                  
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        edmode.Value = "Y";   
                    }
                    #endregion
                    break;
                case "Print_E":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10144");
                    fgen.fin_engg_reps(frm_qstr);
                    break;
                case "PLYBUT":
                    if (col1.Length <= 0) return;
                    break;

                case "COSTBUT":
                    if (col1.Length <= 0) return;
                    
                    break;

                case "CTRYBUT":
                    if (col1.Length <= 0) return;
                  
                    break;

                case "IVLBUT":
                    if (col1.Length <= 0) return;
                   
                    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "SG1_ROW_ADD":

                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }
                    break;
                case "SG4_ROW_ADD11":
                    break;
                case "SG1_ROW_TAX":

                    break;
                case "SG1_ROW_DT":
                    // ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;

                case "SG4_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg4_dt = new DataTable();
                        dt = (DataTable)ViewState["sg4"];
                        z = dt.Rows.Count - 1;
                        sg4_dt = dt.Clone();
                        sg4_dr = null;
                        i = 0;
                        //for (i = 0; i < sg4.Rows.Count - 1; i++)
                        //{
                        //    sg4_dr = sg4_dt.NewRow();
                        //    sg4_dr["sg4_srno"] = (i + 1);

                        //    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                        //    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                        //    sg4_dt.Rows.Add(sg4_dr);
                        //}

                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();

                        ViewState["sg4"] = sg4_dt;
                        //sg4.DataSource = sg4_dt;
                        //sg4.DataBind();
                    }
                    #endregion

                    break;
            }

        }
        
}
    
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (frm_ulvl == "3") cond = " and trim(a.ccode)='" + frm_uname + "'";
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            SQuery = "SELECT code,trim(aname) as aname,trim(iname) as iname,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,lt as length, wd as width, ht as height,ply , flute, CS, boxcost,to_char(ent_dt,'dd/mm/yyyy') as ent_dt FROM " + frm_tabname + " WHERE branchcd='" + frm_mbr + "' and VCHDATE " + PrdRange + "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------


            //-----------------------------
            i = 0;
            hffield.Value = "";

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
                if (Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                        oDS5 = new DataSet();
                        oporow5 = null;
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);


                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();


                        save_fun5();
                        save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);


                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);


                        if (edmode.Value == "Y")
                        {

                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "Y";


                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        save_fun5();
                        save_fun2();

                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld2 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            ddl_fld1 = ddl_fld2.Substring(0, 6);

                        }
                        else
                        {
                            ddl_fld1 = ddl_fld2;
                        }

                        if (edmode.Value == "Y")
                        {

                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set code='DD' where trim(vchnum)='" + ddl_fld1 + "'AND branchcd='" + frm_mbr + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname1 + " set trannum='DD' where trim(trannum)='" + frm_mbr + ddl_fld1 + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname2 + " set trannum='DD' where trim(trannum)='" + frm_mbr + ddl_fld1 + "'");
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        fgen.save_data(frm_qstr, frm_cocd, oDS5, frm_tabname1);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname2);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname + " where code='DD'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname1 + " where trannum='DD'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname2 + " where trannum='DD'");

                        }
                        else
                        {
                            if (save_it == "Y")
                            {


                                fgen.msg("-", "AMSG", " Entry No " + txtVchnum.Value.Trim() + "Saved Successfully");

                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                    }
                    catch (Exception ex)
                    {


                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                    }
                }
            #endregion
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();


        
    }
    public void create_tab2()
    {


    }

    public void create_tab3()
    {


        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field

        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));

    }

    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field

        sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_item", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));

    }

    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();
     
        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;


       

        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "0";
        sg1_dr["sg1_t4"] = "0";
    
        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {

    }
    public void sg3_add_blankrows()
    {
        sg3_dr = sg3_dt.NewRow();

        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "-";
        sg3_dr["sg3_f2"] = "-";
        sg3_dr["sg3_t1"] = "-";
        sg3_dr["sg3_t2"] = "-";
        sg3_dr["sg3_t3"] = "-";
        sg3_dr["sg3_t4"] = "-";

        sg3_dt.Rows.Add(sg3_dr);
    }

    public void sg4_add_blankrows()
    {
        sg4_dr = sg4_dt.NewRow();


        sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        sg4_dr["sg4_item"] = "-";
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
        sg4_dt.Rows.Add(sg4_dr);
    }

    //------------------------------------------------------------------------------------
    void save_fun()
    {

        calculate();
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        //oporow["Id"] = "B";
        oporow["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4,2);
        oporow["branchcd"] = frm_mbr;
        oporow["aname"] = txtCustomer.Value.ToUpper().Trim();
        oporow["iname"] = txtItem.Value.ToUpper().Trim();
        oporow["vchnum"] = txtVchnum.Value.ToUpper().Trim();
        oporow["vchdate"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
        oporow["trannum"] = txtVchnum.Value.ToUpper().Trim();
        //oporow["trandt"] = txt_cit_name.Value.ToUpper().Trim();//need to revise
        oporow["lt"] = txtL.Value.ToUpper().Trim();
        oporow["wd"] = txtWid.Value.ToUpper().Trim();
        oporow["ht"] = txtHeight.Value.ToUpper().Trim();
        oporow["ply"] = txtPly.Value.ToUpper().Trim();
        oporow["flute"] = txtFlute.Value.ToUpper().Trim();

        oporow["cs"] = txtCs.Value.ToUpper().Trim();
        oporow["caliper"] = caliper;//need to revise
        oporow["z"] = z;//need to revise
        oporow["rqect"] = ReqECT;
        oporow["rqbs"] = ReqBs;
        oporow["rqgsm"] = ReqGSM;
        oporow["deckle"] = Deckle;
        oporow["length"] = Length;
        oporow["area"] = Area;
        oporow["lengthwidthratio"] = l_w_ratio;
        oporow["depthfactor"] = depthfactor;
        oporow["l_w_factor"] = l_w_factor;
        oporow["net_factor"] = net_factor;
        oporow["minect"] = min_ect;
        oporow["maxect"] = max_ect;
        oporow["avgect"] = avg_ect;
        oporow["maxcs"] = max_cs;
        oporow["mincs"] = min_cs;
        oporow["avgcs"] = avg_cs;
        oporow["mingsm"] = min_gsm;
        oporow["maxgsm"] = max_gsm;
        oporow["avggsm"] = avg_gsm;
        oporow["minbs"] = min_bs;
        oporow["maxbs"] = max_bs;
        oporow["avgbs"] = avg_bs;
        oporow["minwt"] = min_wt;
        oporow["maxwt"] = max_wt;
        oporow["avgwt"] = avg_wt;
        oporow["contribution"] = txtRateContri.Value.Trim();
        oporow["contamt"] = Contribution;
        oporow["tconcst"] = TotalConversionCost;
        oporow["cstpkg"] = ConversionCostperkg;
        oporow["papcst"] = PaperCost;
        oporow["pawastage"] = txtRatePaperWst.Value.Trim();
        oporow["pawastageamt"] = PaperWastage;
        oporow["boxcost"] = BoxIncost;
        oporow["rem"] = txtRemarks.Value.ToString().Trim();

        //insert rate value

        oporow["h_16"] = fgen.make_double(txthighRCTRatea.Value.ToString().Trim());
        oporow["n_16"] = fgen.make_double(txtNormalRCTRatea.Value.ToString().Trim());
        oporow["h_18"] = fgen.make_double(txthighRCTRateb.Value.ToString().Trim());
        oporow["n_18"] =  fgen.make_double(txtNormalRCTRateb.Value.ToString().Trim());
        oporow["h_20"] = fgen.make_double(txthighRCTRatec.Value.ToString().Trim());
        oporow["n_20"] =  fgen.make_double(txtNormalRCTRatec.Value.ToString().Trim());
        oporow["h_22"] = fgen.make_double(txthighRCTRated.Value.ToString().Trim());
        oporow["n_22"] =  fgen.make_double(txtNormalRCTRated.Value.ToString().Trim());
        oporow["h_24"] = fgen.make_double(txthighRCTRatee.Value.ToString().Trim());
        oporow["n_24"] =  fgen.make_double(txtNormalRCTRatee.Value.ToString().Trim());
        oporow["h_28"] =fgen.make_double(txthighRCTRatef.Value.ToString().Trim());
        oporow["n_28"] =  fgen.make_double(txtNormalRCTRatef.Value.ToString().Trim());
        oporow["h_35"] = fgen.make_double(txthighRCTRateg.Value.ToString().Trim());
        oporow["n_35"] =  fgen.make_double(txtNormalRCTRateg.Value.ToString().Trim());
        oporow["h_45"] =fgen.make_double(txthighRCTRateh.Value.ToString().Trim());
        oporow["n_45"] = fgen.make_double(txtNormalRCTRateh.Value.ToString().Trim());

        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"].ToString();
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
           
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
           
        }
        oDS.Tables[0].Rows.Add(oporow);

    }

    void save_fun5()
    {

        for (i = 0; i <5; i++)
        {
            oporow5 = oDS5.Tables[0].NewRow();
           
            if (i == 0)
            {
               

                oporow5["gsm"] = txtGSM1.Value.ToString().Trim();
                oporow5["bf"] = txtBF1.Value.ToString().Trim();
                oporow5["rctgrade"] = txtRCTGrade1.Value.ToString().Trim();
                oporow5["SRNO"] = "0" + i;
                oporow5["rct"] = TRCT;
                oporow5["t_rct"] = T_RCT;
                oporow5["cost"] = TCostpertop;
                oporow5["hrcti"] = THRCT;
                oporow5["nrcti"] = TNRCT;
                oporow5["correctindex"] = TCorrectIndex;
                oporow5["tf"] = TTF;
                oporow5["hrctr"] = TRateperkg_h;
                oporow5["nrctr"] = TRateperkg_N;
                oporow5["correctpaperrate"] = TCorrectPaperRate;
                oporow5["factor"] = TTop_factor;
                oporow5["costperbox"] = TCostpertop;
                oporow5["totcost"] = totalcost;
                oporow5["totrct"] = totaltrct;
                oporow5["desc_"] = "TOP PLY";
                oporow5["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow5["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow5["ENT_BY"] = frm_uname;
                oporow5["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow5["ENT_DT"] = vardate;

                
            }


            if (i == 1)
            {
              

                oporow5["gsm"] = txtGSM2.Value.ToString().Trim();
                oporow5["bf"] = txtBF2.Value.ToString().Trim();
                oporow5["rctgrade"] = txtRCTGrade2.Value.ToString().Trim();
                oporow5["SRNO"] = "0" + i;
                oporow5["rct"] = F1RCT;
                oporow5["t_rct"] = F1_RCT;
                oporow5["cost"] = F1Costpertop;
                oporow5["hrcti"] = F1HRCT;
                oporow5["nrcti"] = F1NRCT;
                oporow5["correctindex"] = F1CorrectIndex;
                oporow5["tf"] = F1TF;
                oporow5["hrctr"] = F1Rateperkg_h;
                oporow5["nrctr"] = F1Rateperkg_N;
                oporow5["correctpaperrate"] = F1CorrectPaperRate;
                oporow5["factor"] = F1Top_factor;
                oporow5["costperbox"] = F1Costpertop;
                oporow5["totcost"] = totalcost;
                oporow5["totrct"] = totaltrct;
                oporow5["desc_"] = "FLUTE 1";
                oporow5["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow5["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow5["ENT_BY"] = frm_uname;
                oporow5["ENT_DT"] = vardate;
                oporow5["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);

               
            }

            if (i == 2)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["gsm"] = txtGSM3.Value.ToString().Trim();
                oporow5["bf"] = txtBF3.Value.ToString().Trim();
                oporow5["rctgrade"] = txtRCTGrade3.Value.ToString().Trim();
                oporow5["SRNO"] = "0" + i;
                oporow5["rct"] = L1RCT;
                oporow5["t_rct"] = L1_RCT;
                oporow5["cost"] = L1Costpertop;
                oporow5["hrcti"] = L1HRCT;
                oporow5["nrcti"] = L1NRCT;
                oporow5["correctindex"] = L1CorrectIndex;
                oporow5["tf"] = L1TF;
                oporow5["hrctr"] = L1Rateperkg_h;
                oporow5["nrctr"] = L1Rateperkg_N;
                oporow5["correctpaperrate"] = L1CorrectPaperRate;
                oporow5["factor"] = L1Top_factor;
                oporow5["costperbox"] = L1Costpertop;
                oporow5["totcost"] = totalcost;
                oporow5["totrct"] = totaltrct;
                oporow5["desc_"] = "LINER 1";
                oporow5["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow5["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow5["ENT_BY"] = frm_uname;
                oporow5["ENT_DT"] = vardate;
                oporow5["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
               
            }


            if (i == 3)
            {
               
                oporow5["gsm"] = txtGSM4.Value.ToString().Trim();
                oporow5["bf"] = txtBF4.Value.ToString().Trim();
                oporow5["rctgrade"] = txtRCTGrade4.Value.ToString().Trim();
                oporow5["SRNO"] = "0" + i;
                oporow5["rct"] = F2RCT;
                oporow5["t_rct"] = F2_RCT;
                oporow5["cost"] = F2Costpertop;
                oporow5["hrcti"] = F2HRCT;
                oporow5["nrcti"] = F2NRCT;
                oporow5["correctindex"] = F2CorrectIndex;
                oporow5["tf"] = F2TF;
                oporow5["hrctr"] = F2Rateperkg_h;
                oporow5["nrctr"] = F2Rateperkg_N;
                oporow5["correctpaperrate"] = F2CorrectPaperRate;
                oporow5["factor"] = F2Top_factor;
                oporow5["costperbox"] = F2Costpertop;
                oporow5["totcost"] = totalcost;
                oporow5["totrct"] = totaltrct;
                oporow5["desc_"] = "FLUTE 2";
                oporow5["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow5["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow5["ENT_BY"] = frm_uname;
                oporow5["ENT_DT"] = vardate;
                oporow5["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
               
            }
            if (i == 4)
            {
               

                oporow5["gsm"] = fgen.make_double(txtGSM5.Value.ToString().Trim());
                oporow5["bf"] = fgen.make_double(txtBF5.Value.ToString().Trim());
                oporow5["rctgrade"] =fgen.make_double(txtRCT1.Value.ToString().Trim());
                oporow5["SRNO"] = "0" + i;
                oporow5["rct"] = L2RCT;
                oporow5["t_rct"] = L2_RCT;
                oporow5["cost"] = L2Costpertop;
                oporow5["hrcti"] = L2HRCT;
                oporow5["nrcti"] = L2NRCT;
                oporow5["correctindex"] = L2CorrectIndex;
                oporow5["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow5["tf"] = L2TF;
                oporow5["hrctr"] = L2Rateperkg_h;
                oporow5["nrctr"] = L2Rateperkg_N;
                oporow5["correctpaperrate"] = L2CorrectPaperRate;
                oporow5["factor"] = L2Top_factor;
                oporow5["costperbox"] = L2Costpertop;
                oporow5["totcost"] = totalcost;
                oporow5["totrct"] = totaltrct;
                oporow5["desc_"] = "LINER 2";
                oporow5["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow5["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow5["ENT_BY"] = frm_uname;
                oporow5["ENT_DT"] = vardate;
               
            }
            oDS5.Tables[0].Rows.Add(oporow5);
        }
    }





    void save_fun2()
    {

        for (i = 0; i <10; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            if (i == 0)
            {
                oporow2["rate"] = fgen.make_double(txtRateStrch.Value.ToString().Trim());
                oporow2["flag"] = txtYNStrch.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = StarchGumAmt;
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["desc_"] = "STARCH GUM";
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }


            if (i == 1)
            {
                oporow2["rate"] = fgen.make_double(txtRatePVA.Value.ToString().Trim());
                oporow2["flag"] = txtYNPVA.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = PVAGumAmt;
                oporow2["desc_"] = "PVA GUM";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }

            if (i == 2)
            {
                oporow2["rate"] = fgen.make_double(txtRatePow.Value.ToString().Trim());
                oporow2["flag"] = txtYNPow.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = PowerAmt;
                oporow2["desc_"] = "POWER";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }


            if (i == 3)
            {
                oporow2["rate"] = fgen.make_double(txtRateFuel.Value.ToString().Trim());
                oporow2["flag"] = txtYNFuel.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = FuelAmt;
                oporow2["desc_"] = "FUEL";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }
            if (i == 4)
            {
                oporow2["rate"] = fgen.make_double(txtRateStchPins.Value.ToString().Trim());
                oporow2["flag"] = txtYNStchPins.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = StichingPinsAmt;
                oporow2["desc_"] = "STITCHING PINS";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }

            if (i == 5)
            {
                oporow2["rate"] = fgen.make_double(txtRatePrint.Value.ToString().Trim());
                oporow2["flag"] = txtYNPrint.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = PrintingInkAmt;
                oporow2["desc_"] = "PRINTING INK";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }


            if (i == 6)
            {
                oporow2["rate"] = fgen.make_double(txtRatelabor.Value.ToString().Trim());
                oporow2["flag"] = txtYNlabor.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = LaborAmt;
                oporow2["desc_"] = "LABOR";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }

            if (i == 7)
            {
                oporow2["rate"] = fgen.make_double(txtRateAdmin.Value.ToString().Trim());
                oporow2["flag"] = txtYNAdmin.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = AdministrativeAmt;
                oporow2["desc_"] = "ADMINISTRATIVE";
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }


            if (i == 8)
            {
                oporow2["rate"] = fgen.make_double(txtRateTrans.Value.ToString().Trim());
                oporow2["flag"] = txtYNTrans.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = TransportationAmt;
                oporow2["desc_"] = "TRANSPORTATION";
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }
            if (i == 9)
            {
                oporow2["rate"] = fgen.make_double(txtRateOtherM.Value.ToString().Trim());
                oporow2["flag"] = txtYNOtherM.Value.ToString().Trim();
                oporow2["SRNO"] = "0" + i;
                oporow2["amt"] = OtherMaterialsAmt;
                oporow2["desc_"] = "OTHER MATERIALS";
                oporow2["trannum"] = frm_mbr + txtVchnum.Value.ToUpper().Trim();
                oporow2["trandt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                oporow2["code"] = txtVchnum.Value.ToUpper().Trim().Substring(4, 2);
                oporow2["ENT_BY"] = frm_uname;
                oporow2["ENT_DT"] = vardate;
            }
            oDS2.Tables[0].Rows.Add(oporow2);
        }
    }

 
    void Type_Sel_query()
    {
    }

     protected void btn_ply_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PLYBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select  PLY ", frm_qstr);
    }

    public void calculate()
    {
        if (txtGSM1.Value.Trim() == "" || txtBF1.Value.Trim()=="")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please put values in the Layers Details ");
            return;
        }

        if (txtRateTrans.Value.Trim() == ""|| txtRateStrch.Value.Trim() == ""|| txtRatePVA.Value.Trim() == ""|| txtRatePrint.Value.Trim() == ""|| txtRatePow.Value.Trim() == ""|| txtRateOtherM.Value.Trim() == ""|| txtRatelabor.Value.Trim() == ""|| txtRateFuel.Value.Trim() == ""|| txtRateContri.Value.Trim() == ""|| txtRateAdmin.Value.Trim() == "" )
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Please put values in the Conversion Details else put 0.");
            return;
        }

        //int count = 0;

        string ply = txtPly.Value.Trim();
        if ((txtFlute.Value.Trim() == "") || (txtFlute.Value.Trim() == "-"))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Fields Require Input '13' Please fill all fields ");
            return;
        }

        //SQuery1 = "SELECT caliper,flute FROM WB_CORRCST_FLUTEM WHERE flute='" + txtFlute.Value.ToUpper().Trim() + "'";
        //dt2 = new DataTable();
        //dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

        //if (dt2.Rows.Count == 0)
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Please update the value of caliper and flute from Flute Caliper Master Form. ");
        //    return;
        //}

        string txtfltcaliper = txtFlute.Value.ToUpper().Trim();
        if (txtfltcaliper == "B")
        {
            caliper = 3;
        }
        else if (txtfltcaliper == "C")
        {
            caliper = 4;
        }
            else
        {
            caliper = 7;
        }
        z = 2 * (Convert.ToInt64(txtL.Value.Trim()) + Convert.ToInt64(txtWid.Value.Trim()));
        ReqECT = Convert.ToInt16(txtCs.Value.Trim().ToString()) / (0.599 * (Math.Sqrt(caliper * z)));
        ReqECT = Math.Round(ReqECT, 2);
        ReqBs = (1.75 * (Convert.ToInt16(txtCs.Value.Trim().ToString()) / (0.599 * (Math.Sqrt(caliper * z))))) + 0.77;
        ReqBs = Math.Round(ReqBs, 2);
        ReqGSM =Math.Round( 91 * (Convert.ToInt16(txtCs.Value.Trim().ToString()) / (0.599 * (Math.Sqrt(caliper * z)))) + 143,0);
        Deckle = int.Parse(txtWid.Value) + int.Parse(txtHeight.Value) + 35;
        Length = 2 * (Convert.ToInt64(txtL.Value.Trim()) + Convert.ToInt64(txtWid.Value.Trim())) + 65;
        Area = (Deckle * Length) / 1000000;
        Area = Math.Round(Area, 4);


        //putting values in a textbox fields

        txtCaliper.Value = caliper.ToString().Trim();
        txtZ.Value = z.ToString().Trim();
        txtECT.Value = ReqECT.ToString().Trim();
        txtBS.Value = ReqBs.ToString().Trim();
        txtGSM.Value = ReqGSM.ToString().Trim();
        txtDeckle.Value = Deckle.ToString().Trim();
        txtLength.Value = Length.ToString().Trim();
        txtArea.Value = Area.ToString().Trim();


       // fetch values from textbox into variables

        //GET IT FROM TEXTBOXES INSTEAD OF GRID

        # region FOR TOP PLY CALCULATION

        TGSM = fgen.make_double(txtGSM1.Value.ToString().Trim());
        TBF = fgen.make_double(txtBF1.Value.ToString().Trim());
        F1BF = fgen.make_double(txtBF2.Value.ToString().Trim());
        L1BF = fgen.make_double(txtBF3.Value.ToString().Trim());
        F2BF = fgen.make_double(txtBF4.Value.ToString().Trim());
        L2BF = fgen.make_double(txtBF5.Value.ToString().Trim());

        TRCTGrade = fgen.make_double(txtRCTGrade1.Value.ToString().Trim());
        if (TBF == 0)
        {
            THRCT = 0; TNRCT = 0;
        }
        else
        {
            SQuery = "SELECT BF,HRCTI,NRCTI FROM WB_CORRCST_RCTM WHERE BF='" + TBF + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count < 0)
            {

                fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Index form");
                return;
            }
            THRCT = fgen.make_double(dt.Rows[0]["hrcti"].ToString());
            TNRCT = fgen.make_double(dt.Rows[0]["nrcti"].ToString());
        }

        var valTBF = txtBF1.Value.Trim();
        switch (valTBF)
        {
            case "16":
                TRateperkg_h = fgen.make_double(txthighRCTRatea.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRatea.Value.ToString());
                break;
            case "18":
                TRateperkg_h = fgen.make_double(txthighRCTRateb.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRateb.Value.ToString());
                break;
            case "20":
                TRateperkg_h = fgen.make_double(txthighRCTRatec.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRatec.Value.ToString());
                break;
            case "22":
                TRateperkg_h = fgen.make_double(txthighRCTRated.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRated.Value.ToString());
                break;
            case "24":
                TRateperkg_h = fgen.make_double(txthighRCTRatee.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRatee.Value.ToString());
                break;
            case "28":
                TRateperkg_h = fgen.make_double(txthighRCTRatef.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRatef.Value.ToString());
                break;
            case "35":
                TRateperkg_h = fgen.make_double(txthighRCTRateg.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRateg.Value.ToString());
                break;
            case "45":
                TRateperkg_h = fgen.make_double(txthighRCTRateh.Value.ToString());
                TRateperkg_N = fgen.make_double(txtNormalRCTRateh.Value.ToString());
                break;
            case "default":
                TRateperkg_h = 0;
                TRateperkg_N = 0;
                break;
        }

        var valF1BF = txtBF2.Value.Trim();
        switch (valF1BF)
        {
            case "16":
                F1Rateperkg_h = fgen.make_double(txthighRCTRatea.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRatea.Value.ToString());
                break;
            case "18":
                F1Rateperkg_h = fgen.make_double(txthighRCTRateb.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRateb.Value.ToString());
                break;
            case "20":
                F1Rateperkg_h = fgen.make_double(txthighRCTRatec.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRatec.Value.ToString());
                break;
            case "22":
                F1Rateperkg_h = fgen.make_double(txthighRCTRated.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRated.Value.ToString());
                break;
            case "24":
                F1Rateperkg_h = fgen.make_double(txthighRCTRatee.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRatee.Value.ToString());
                break;
            case "28":
                F1Rateperkg_h = fgen.make_double(txthighRCTRatef.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRatef.Value.ToString());
                break;
            case "35":
                F1Rateperkg_h = fgen.make_double(txthighRCTRateg.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRateg.Value.ToString());
                break;
            case "45":
                F1Rateperkg_h = fgen.make_double(txthighRCTRateh.Value.ToString());
                F1Rateperkg_N = fgen.make_double(txtNormalRCTRateh.Value.ToString());
                break;
            case "default":
                F1Rateperkg_h = 0;
                F1Rateperkg_N = 0;
                break;
        }


        var valL1BF = txtBF3.Value.Trim();
        switch (valL1BF)
        {
            case "16":
                L1Rateperkg_h = fgen.make_double(txthighRCTRatea.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRatea.Value.ToString());
                break;
            case "18":
                L1Rateperkg_h = fgen.make_double(txthighRCTRateb.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRateb.Value.ToString());
                break;
            case "20":
                L1Rateperkg_h = fgen.make_double(txthighRCTRatec.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRatec.Value.ToString());
                break;
            case "22":
                L1Rateperkg_h = fgen.make_double(txthighRCTRated.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRated.Value.ToString());
                break;
            case "24":
                L1Rateperkg_h = fgen.make_double(txthighRCTRatee.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRatee.Value.ToString());
                break;
            case "28":
                L1Rateperkg_h = fgen.make_double(txthighRCTRatef.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRatef.Value.ToString());
                break;
            case "35":
                L1Rateperkg_h = fgen.make_double(txthighRCTRateg.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRateg.Value.ToString());
                break;
            case "45":
                L1Rateperkg_h = fgen.make_double(txthighRCTRateh.Value.ToString());
                L1Rateperkg_N = fgen.make_double(txtNormalRCTRateh.Value.ToString());
                break;
            case "default":
                L1Rateperkg_h = 0;
                L1Rateperkg_N = 0;
                break;
        }

        var valF2BF = txtBF4.Value.Trim();
        switch (valF2BF)
        {
            case "16":
                F2Rateperkg_h = fgen.make_double(txthighRCTRatea.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRatea.Value.ToString());
                break;
            case "18":
                F2Rateperkg_h = fgen.make_double(txthighRCTRateb.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRateb.Value.ToString());
                break;
            case "20":
                F2Rateperkg_h = fgen.make_double(txthighRCTRatec.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRatec.Value.ToString());
                break;
            case "22":
                F2Rateperkg_h = fgen.make_double(txthighRCTRated.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRated.Value.ToString());
                break;
            case "24":
                F2Rateperkg_h = fgen.make_double(txthighRCTRatee.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRatee.Value.ToString());
                break;
            case "28":
                F2Rateperkg_h = fgen.make_double(txthighRCTRatef.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRatef.Value.ToString());
                break;
            case "35":
                F2Rateperkg_h = fgen.make_double(txthighRCTRateg.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRateg.Value.ToString());
                break;
            case "45":
                F2Rateperkg_h = fgen.make_double(txthighRCTRateh.Value.ToString());
                F2Rateperkg_N = fgen.make_double(txtNormalRCTRateh.Value.ToString());
                break;
            case "default":
                F2Rateperkg_h = 0;
                F2Rateperkg_N = 0;
                break;
        }

        var valL2BF = txtBF5.Value.Trim();
        switch (valL2BF)
        {
            case "16":
                L2Rateperkg_h = fgen.make_double(txthighRCTRatea.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRatea.Value.ToString());
                break;
            case "18":
                L2Rateperkg_h = fgen.make_double(txthighRCTRateb.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRateb.Value.ToString());
                break;
            case "20":
                L2Rateperkg_h = fgen.make_double(txthighRCTRatec.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRatec.Value.ToString());
                break;
            case "22":
                L2Rateperkg_h = fgen.make_double(txthighRCTRated.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRated.Value.ToString());
                break;
            case "24":
                L2Rateperkg_h = fgen.make_double(txthighRCTRatee.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRatee.Value.ToString());
                break;
            case "28":
                L2Rateperkg_h = fgen.make_double(txthighRCTRatef.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRatef.Value.ToString());
                break;
            case "35":
                L2Rateperkg_h = fgen.make_double(txthighRCTRateg.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRateg.Value.ToString());
                break;
            case "45":
                L2Rateperkg_h = fgen.make_double(txthighRCTRateh.Value.ToString());
                L2Rateperkg_N = fgen.make_double(txtNormalRCTRateh.Value.ToString());
                break;
            case "default":
                L2Rateperkg_h = 0;
                L2Rateperkg_N = 0;
                break;
        }
        #endregion

        # region FOR FLUTE1 CALCULATION
        F1GSM = fgen.make_double(txtGSM2.Value.ToString().Trim());
        F1RCTGrade = fgen.make_double(txtRCTGrade2.Value.ToString().Trim());
        if (F1BF == 0)
        {
            F1HRCT = 0; F1NRCT = 0;
        }
        else
        {
            SQuery = "SELECT BF,HRCTI,NRCTI FROM WB_CORRCST_RCTM WHERE BF='" + F1BF + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count < 0)
            {

                fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Index form");
                return;
            }
            F1HRCT = fgen.make_double(dt.Rows[0]["hrcti"].ToString());
            F1NRCT = fgen.make_double(dt.Rows[0]["nrcti"].ToString());
        }
        #endregion

        # region FOR LINER1 CALCULATION


        L1GSM = fgen.make_double(txtGSM3.Value.ToString().Trim());
        L1RCTGrade = fgen.make_double(txtRCTGrade3.Value.ToString().Trim());
        if (L1BF == 0)
        {
            L1HRCT = 0; L1NRCT = 0;
        }
        else
        {
            SQuery = "SELECT BF,HRCTI,NRCTI FROM WB_CORRCST_RCTM WHERE BF='" + L1BF + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count < 0)
            {

                fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Index form");
                return;
            }
            L1HRCT = fgen.make_double(dt.Rows[0]["hrcti"].ToString());
            L1NRCT = fgen.make_double(dt.Rows[0]["nrcti"].ToString());
        }



        #endregion
        # region FOR FLUTE2 CALCULATION

        F2GSM = fgen.make_double(txtGSM4.Value.ToString().Trim());
        F2RCTGrade = fgen.make_double(txtRCTGrade4.Value.ToString().Trim());
        if (F2BF == 0)
        {
            F2HRCT = 0; F2NRCT = 0;
        }
        else
        {
            SQuery = "SELECT BF,HRCTI,NRCTI FROM WB_CORRCST_RCTM WHERE BF='" + F2BF + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count < 0)
            {

                fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Index form");
                return;
            }
            F2HRCT = fgen.make_double(dt.Rows[0]["hrcti"].ToString());
            F2NRCT = fgen.make_double(dt.Rows[0]["nrcti"].ToString());
        }

        #endregion
        # region FOR LINER2 CALCULATION


        L2GSM = fgen.make_double(txtGSM5.Value.ToString().Trim());
        L2RCTGrade = fgen.make_double(txtRCTGrade5.Value.ToString().Trim());
        if (L2BF == 0)
        {
            L2HRCT = 0; L2NRCT = 0;
        }
        else
        {
            SQuery = "SELECT BF,HRCTI,NRCTI FROM WB_CORRCST_RCTM WHERE BF='" + L2BF + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count < 0)
            {
                fgen.msg("", "ASMG", "There is no data available for this BUS Factor. Please Update in Paper Index form");
                return;
            }
            L2HRCT = fgen.make_double(dt.Rows[0]["hrcti"].ToString());
            L2NRCT = fgen.make_double(dt.Rows[0]["nrcti"].ToString());
        }
        #endregion

     
        if (TRCTGrade == 1)
        {
            TCorrectIndex = THRCT;
            TCorrectPaperRate = TRateperkg_h;
        }
        else
        {
            TCorrectIndex = TNRCT;
            TCorrectPaperRate = TRateperkg_N;
        }
        TRCT = Math.Round((TGSM * TCorrectIndex) / 1000,2);
        T_RCT = Math.Round(((TGSM * TCorrectIndex) / 1000) * TTF, 2);

        txtRCT1.Value = TRCT.ToString().Trim();
        txtTRCT1.Value = T_RCT.ToString().Trim();

        TTop_factor = ((Deckle - 15) * (Length - 20)) / 100000;
        TCostpertop = (Area * TCorrectPaperRate * TTF * TGSM) / 1000;
        TCostpertop = Math.Round(TCostpertop, 2);

        txtCost1.Value = TCostpertop.ToString().Trim();
      
        //  calculation of flute layer
        if (F1RCTGrade == 1)
        {
            F1CorrectIndex = F1HRCT;
        }
        else
        {
            F1CorrectIndex = F1NRCT;
        }

        if ((txtFlute.Value.ToUpper().Trim() == "B") || (txtFlute.Value.ToUpper().Trim() == "BC"))
        {
            F1TF = 1.35;
        }
        if (txtFlute.Value.ToUpper().Trim() == "C")
        {
            F1TF = 1.45;
        }


        F1RCT = Math.Round((F1GSM * F1CorrectIndex) / 1000,2);
        F1_RCT = Math.Round(((F1GSM * F1CorrectIndex) / 1000)* F1TF, 2);
 
        txtRCT2.Value = F1RCT.ToString().Trim();
        txtTRCT2.Value = F1_RCT.ToString().Trim();

        if (F1RCTGrade == 1)
        {
            F1CorrectPaperRate = F1Rateperkg_h;
        }
        else
        {
            F1CorrectPaperRate = F1Rateperkg_N;
        }

        F1Top_factor = ((Deckle - 15) * (Length - 20)) / 100000;
        F1Costpertop = (Area * F1CorrectPaperRate * F1TF * F1GSM) / 1000;
        F1Costpertop = Math.Round(F1Costpertop, 2);

        txtCost2.Value = F1Costpertop.ToString().Trim();

        // CALCULATION OF LINER 1


        if (L1RCTGrade == 1)
        {
            L1CorrectIndex = L1HRCT;
        }
        else
        {
            L1CorrectIndex = L1NRCT;
        }
        L1RCT = Math.Round((L1GSM * L1CorrectIndex) / 1000,2);
        L1_RCT = Math.Round(((L1GSM * L1CorrectIndex) / 1000) * L1TF,2);

        txtRCT3.Value = L1RCT.ToString().Trim();
        txtTRCT3.Value = L1_RCT.ToString().Trim();

        if (L1RCTGrade == 1)
        {
            L1CorrectPaperRate = L1Rateperkg_h;
        }
        else
        {
            L1CorrectPaperRate = L1Rateperkg_N;
        }


        L1Top_factor = ((Deckle - 15) * (Length - 20)) / 100000;
        L1Costpertop = (Area * L1CorrectPaperRate * L1TF * L1GSM) / 1000;
        L1Costpertop = Math.Round(L1Costpertop, 2);


        txtCost3.Value = L1Costpertop.ToString().Trim();
        //CALCULATION OF FLUTE2



        if (F2RCTGrade == 1)
        {
            F2CorrectIndex = F2HRCT;
        }
        else
        {
            F2CorrectIndex = F2NRCT;
        }
        if (txtPly.Value.Trim() == "3")
        {
            F2TF = 0;
        }
        else
        {
            if (txtFlute.Value.ToUpper().Trim() == "B")
            {
                F2TF = 1.35;
            }
            else
            {
                F2TF = 1.45;
            }
        }
        F2RCT = Math.Round((F2GSM * F2CorrectIndex / 1000), 2);
        F2_RCT = Math.Round((F2GSM * F2CorrectIndex / 1000) * F2TF, 2);


        txtRCT4.Value = F2RCT.ToString().Trim();
        txtTRCT4.Value = F2_RCT.ToString().Trim();

        if (F2RCTGrade == 1)
        {
            F2CorrectPaperRate = F2Rateperkg_h;
        }
        else
        {
            F2CorrectPaperRate = F2Rateperkg_N;
        }

        F2Top_factor = ((Deckle - 15) * (Length - 20)) / 100000;
        F2Costpertop = (Area * F2CorrectPaperRate * F2TF * F2GSM) / 1000;
        F2Costpertop = Math.Round(F2Costpertop, 2);

        txtCost4.Value = F2Costpertop.ToString().Trim();
       
        //CALCULATION OF LINER2

        if (L2RCTGrade == 1)
        {
            L2CorrectIndex = L2HRCT;
        }
        else
        {
            L2CorrectIndex = L2NRCT;
        }
        if(txtPly.Value.Trim()=="3")
        {
            L2TF = 0;
        }
        else
        {
            L2TF = 1;
        }
        L2RCT = Math.Round(L2GSM * L2CorrectIndex / 1000,2);
        L2_RCT = Math.Round((L2GSM * L2CorrectIndex / 1000) * L2TF,2);

        txtRCT5.Value = L2RCT.ToString().Trim();
        txtTRCT5.Value = L2_RCT.ToString().Trim();
        //L2Rateperkg_h = 25.00;
        //L2Rateperkg_N = 23.50;

        if (L2RCTGrade == 1)
        {
            L2CorrectPaperRate = L2Rateperkg_h;
        }
        else
        {
            L2CorrectPaperRate = L2Rateperkg_N;
        }


        L2Top_factor = ((Deckle - 15) * (Length - 20)) / 100000;
        L2Costpertop = Math.Round((Area * L2CorrectPaperRate * L2TF * L2GSM) / 1000,2);
        txtCost5.Value = L2Costpertop.ToString().Trim();
        // end of layer grid
        //check ply value
        //if (ply == "3")
        //{
        //    totaltrct = T_RCT + F1_RCT + L1_RCT;
        //    totalcost = TCostpertop + F1Costpertop + L1Costpertop;

        //}
        //if (ply == "5")
        //{
            totaltrct = T_RCT + F1_RCT + L1_RCT + F2_RCT + L2_RCT;
           // totalcost = TCostpertop + F1Costpertop + L1Costpertop + F2Costpertop + L2Costpertop;

            totalcost = ((TCorrectPaperRate * TTF * TGSM * Area / 1000) + (L2CorrectPaperRate * L2TF * L2GSM * Area / 1000) + (F2CorrectPaperRate * F2TF * F2GSM * Area / 1000)) + (L1CorrectPaperRate * L1TF * L1GSM * Area / 1000) + (F1CorrectPaperRate * F1TF * F1GSM * Area / 1000);

        totaltrct = Math.Round(totaltrct, 2);
        totalcost = Math.Round(totalcost, 2);

        txtTRCTtot.Value = totaltrct.ToString().Trim();
        txtCosttot.Value = totalcost.ToString().Trim();

      


        l_w_ratio = Convert.ToDouble(txtL.Value.Trim()) / Convert.ToDouble(txtWid.Value.Trim());
        depthfactor = Math.Round(2.7059 * Math.Pow(Convert.ToDouble(txtHeight.Value.Trim()), -0.18), 2);
        l_w_factor = Math.Round(1.103 - 0.015 * l_w_ratio - 0.017 * l_w_ratio * l_w_ratio, 2);

        net_factor = Math.Round(l_w_factor * depthfactor, 3);

        min_ect = Math.Round(1.07 * totaltrct, 2);
        max_ect = Math.Round(1.27 * totaltrct, 2);
        avg_ect = Math.Round(1.2 * totaltrct, 2);

        min_cs = Math.Round(0.599 * min_ect * Math.Sqrt(z * caliper) * net_factor);

        max_cs = Math.Round(0.599 * max_ect * Math.Sqrt(z * caliper) * net_factor);

        avg_cs = Math.Round(0.599 * avg_ect * Math.Sqrt(z * caliper) * net_factor);


        avg_gsm = Math.Round(TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF), 0);
        min_gsm = Math.Round((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * 0.95, 0);
        max_gsm = Math.Round((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * 1.05, 0);


        min_bs = Math.Round((((F1GSM * F1BF) + (F2GSM * F2BF)) * (0.2 / 1000)) + (((TGSM * TBF) + (L1GSM * L1BF) + (L2GSM * L2BF)) / 1000), 2);
        max_bs = Math.Round((((F1GSM * F1BF) + (F2GSM * F2BF)) * (0.4 / 1000)) + (((TGSM * TBF) + (L1GSM * L1BF) + (L2GSM * L2BF)) / 1000), 2); ;
        avg_bs = Math.Round((((F1GSM * F1BF) + (F2GSM * F2BF)) * (0.3 / 1000)) + (((TGSM * TBF) + (L1GSM * L1BF) + (L2GSM * L2BF)) / 1000), 2); ;


        avg_wt = Math.Round((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000);

        min_wt = Math.Round(0.95 * avg_wt);
        max_wt = Math.Round(1.05 * avg_wt);

        //putting values from the above result MINIMUM VALUES
        txtwghtmin.Value = min_wt.ToString().Trim();
        txtBSmin.Value = min_bs.ToString().Trim();
        txtGSMmin.Value = min_gsm.ToString().Trim();
        txtECTmin.Value = min_ect.ToString().Trim();
        txtCSmin.Value = min_cs.ToString().Trim();

        //MAXIMUM VALUES

        txtwghtmax.Value = max_wt.ToString().Trim();
        txtBSmax.Value = max_bs.ToString().Trim();
        txtGSMmax.Value = max_gsm.ToString().Trim();
        txtECTmax.Value = max_ect.ToString().Trim();
        txtCSmax.Value = max_cs.ToString().Trim();

        //AVERAGE VALUES
        txtwghtavg.Value = avg_wt.ToString().Trim();
        txtBSavg.Value = avg_bs.ToString().Trim();
        txtGSMavg.Value = avg_gsm.ToString().Trim();
        txtECTavg.Value = avg_ect.ToString().Trim();
        txtCSavg.Value = avg_cs.ToString().Trim();

        StarchGumRate = fgen.make_double(txtRateStrch.Value);
        StarchGumYN = fgen.make_double(txtYNStrch.Value);
        
        ////
        PVAGumRate = fgen.make_double(txtRatePVA.Value);
        PVAGumYN = fgen.make_double(txtYNPVA.Value);
        /////

        PowerRate = fgen.make_double(txtRatePow.Value);
        PowerYN = fgen.make_double(txtYNPow.Value);

        /////////////
        FuelRate = fgen.make_double(txtRateFuel.Value);
        FuelYN = fgen.make_double(txtYNFuel.Value);

        /////
        StichingPinsRate = fgen.make_double(txtRateStchPins.Value);
        StichingPinsYN = fgen.make_double(txtYNStchPins.Value);
        
        /////
        PrintingInkRate = fgen.make_double(txtRatePrint.Value);
        PrintingInkYN = fgen.make_double(txtYNPrint.Value);
        /////

        LaborRate = fgen.make_double(txtRatelabor.Value);
        LaborYN = fgen.make_double(txtYNlabor.Value);
        //////////
        AdministrativeRate = fgen.make_double(txtRateAdmin.Value);
        AdministrativeYN = fgen.make_double(txtYNAdmin.Value);
        /////

        TransportationRate = fgen.make_double(txtRateTrans.Value);
        TransportationYN = fgen.make_double(txtYNTrans.Value);
        ///////

        OtherMaterialsRate = fgen.make_double(txtRateOtherM.Value);
        OtherMaterialsYN = fgen.make_double(txtYNOtherM.Value);
        ///////////

        //CALCULATION OF CONVERSION COST

        StarchGumAmt = (Area * (Convert.ToInt32(txtPly.Value.Trim()) - 1) * 8 * StarchGumRate * StarchGumYN) / 1000;
        PVAGumAmt = ((PVAGumRate / 400) * (Convert.ToDouble(txtHeight.Value.Trim()) / 1000)) * PVAGumYN;
        PowerAmt = ((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) * 0.045 * PowerRate * (PowerYN / 1000);
        FuelAmt = ((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) * 0.115 * FuelRate * (FuelYN / 1000);
        StichingPinsAmt = ((Convert.ToDouble(txtHeight.Value.Trim()) / 50) + 1) * 2 * (StichingPinsRate / 2200) * StichingPinsYN;
        PrintingInkAmt = (((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) / 1000) * (0.75 / 1000) * PrintingInkRate * PrintingInkYN;
        LaborAmt = ((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) * LaborRate * LaborYN / 1000;
        AdministrativeAmt = ((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) * AdministrativeRate * AdministrativeYN / 1000;
        TransportationAmt = ((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) * TransportationRate * TransportationYN / 1000;
        OtherMaterialsAmt = ((TGSM + L1GSM + L2GSM + (F1GSM * F1TF) + (F2GSM * F2TF)) * ((Deckle - 15) * (Length - 20)) / 1000000) * OtherMaterialsRate * OtherMaterialsYN / 1000;
        double p1 = fgen.make_double(txtRateContri.Value.Trim());
        p1 = (p1 / 100);
        Contribution = (StarchGumAmt + PVAGumAmt + PowerAmt + FuelAmt + StichingPinsAmt + PrintingInkAmt + LaborAmt + AdministrativeAmt + TransportationAmt + OtherMaterialsAmt + totalcost) * p1;
        TotalConversionCost = (StarchGumAmt + PVAGumAmt + PowerAmt + FuelAmt + StichingPinsAmt + PrintingInkAmt + LaborAmt + AdministrativeAmt + TransportationAmt + OtherMaterialsAmt + Contribution);
        ConversionCostperkg = (Math.Round(TotalConversionCost,2)) / ( (Deckle * Length * avg_gsm)/ 1000000000);
        PaperCost = totalcost;
        
        double p = fgen.make_double(txtRatePaperWst.Value.Trim());
        p = (p / 100);
        PaperWastage=totalcost * p;
        
        ///

        StarchGumAmt = Math.Round(StarchGumAmt, 2);
        PVAGumAmt = Math.Round(PVAGumAmt, 2);
        PowerAmt = Math.Round(PowerAmt, 2);
        FuelAmt = Math.Round(FuelAmt, 2);
        StichingPinsAmt = Math.Round(StichingPinsAmt, 2);
        PrintingInkAmt = Math.Round(PrintingInkAmt, 2);
        AdministrativeAmt = Math.Round(AdministrativeAmt, 2);
        TransportationAmt = Math.Round(TransportationAmt, 2);
        OtherMaterialsAmt = Math.Round(OtherMaterialsAmt, 2);
        LaborAmt = Math.Round(LaborAmt, 2);
        Contribution = Math.Round(Contribution, 2);
        TotalConversionCost = Math.Round(TotalConversionCost, 2);
        ConversionCostperkg = Math.Round( ConversionCostperkg , 2);
        PaperWastage = Math.Round(PaperWastage, 2);
        BoxIncost = TotalConversionCost + PaperCost + PaperWastage;
        BoxIncost = Math.Round(BoxIncost, 2);

        //putting values in AMT 

        txtAmtStrch.Value = StarchGumAmt.ToString().Trim();
        txtAmtPVA.Value = PVAGumAmt.ToString().Trim();
        txtAmtPow.Value = PowerAmt.ToString().Trim();
        txtAmtFuel.Value = FuelAmt.ToString().Trim();
        txtAmtStchPins.Value = StichingPinsAmt.ToString().Trim();
        txtAmtPrint.Value = PrintingInkAmt.ToString().Trim();
        txtAmtlabor.Value = LaborAmt.ToString().Trim();
        txtAmtAdmin.Value = AdministrativeAmt.ToString().Trim();
        txtAmtTrans.Value = TransportationAmt.ToString().Trim();
        txtAmtOtherM.Value = OtherMaterialsAmt.ToString().Trim();
        txtAmtContri.Value = Contribution.ToString().Trim();
        txtAmtTotalConv.Value = TotalConversionCost.ToString().Trim();
        txtAmtConvCostperkg.Value = ConversionCostperkg.ToString().Trim();
        txtAmtPapercost.Value = PaperCost.ToString().Trim();
        txtAmtPaperWst.Value = PaperWastage.ToString().Trim();
        

        hf1.Value = BoxIncost.ToString();
        txtBoxCost.Value = BoxIncost.ToString();
        string v = hf1.Value;
        fgen.msg("-", "AMSG", "Total Estimated cost per box is Rs. " + v);
    }

    protected void btncal_Click(object sender, EventArgs e)
    {
        calculate();
        btnsave.Disabled = false;
    }

    public void fillindex()
    {
        SQuery = "SELECT BF,HRCTI,NRCTI,HRCTRT,NRCTRT FROM WB_CORRCST_RCTM  where BF not in('0') order by BF";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
             
            //Fetching Bus record 
            txtBFa.Value= dt.Rows[0]["BF"].ToString().Trim();
            txtBFb.Value = dt.Rows[1]["BF"].ToString().Trim();
            txtBFc.Value = dt.Rows[2]["BF"].ToString().Trim();
            txtBFd.Value = dt.Rows[3]["BF"].ToString().Trim();
            txtBFe.Value = dt.Rows[4]["BF"].ToString().Trim();
            txtBFf.Value = dt.Rows[5]["BF"].ToString().Trim();
            txtBFg.Value = dt.Rows[6]["BF"].ToString().Trim();
            txtBFh.Value = dt.Rows[7]["BF"].ToString().Trim();
            
         // fetching high  RCT index

            txthighRCTIndexa.Value = dt.Rows[0]["HRCTI"].ToString().Trim();
            txthighRCTIndexb.Value = dt.Rows[1]["HRCTI"].ToString().Trim();
            txthighRCTIndexc.Value = dt.Rows[2]["HRCTI"].ToString().Trim();
            txthighRCTIndexd.Value = dt.Rows[3]["HRCTI"].ToString().Trim();
            txthighRCTIndexe.Value = dt.Rows[4]["HRCTI"].ToString().Trim();
            txthighRCTIndexf.Value = dt.Rows[5]["HRCTI"].ToString().Trim();
            txthighRCTIndexg.Value = dt.Rows[6]["HRCTI"].ToString().Trim();
            txthighRCTIndexh.Value = dt.Rows[7]["HRCTI"].ToString().Trim();

            //fetching normal RCT index

            txtNormalRCTIndexa.Value = dt.Rows[0]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexb.Value = dt.Rows[1]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexc.Value = dt.Rows[2]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexd.Value = dt.Rows[3]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexe.Value = dt.Rows[4]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexf.Value = dt.Rows[5]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexg.Value = dt.Rows[6]["NRCTI"].ToString().Trim();
            txtNormalRCTIndexh.Value = dt.Rows[7]["NRCTI"].ToString().Trim();
           
            // fetching high RCT Rate
            txthighRCTRatea.Value = dt.Rows[0]["HRCTRT"].ToString().Trim();
            txthighRCTRateb.Value = dt.Rows[1]["HRCTRT"].ToString().Trim();
            txthighRCTRatec.Value = dt.Rows[2]["HRCTRT"].ToString().Trim();
            txthighRCTRated.Value = dt.Rows[3]["HRCTRT"].ToString().Trim();
            txthighRCTRatee.Value = dt.Rows[4]["HRCTRT"].ToString().Trim();
            txthighRCTRatef.Value = dt.Rows[5]["HRCTRT"].ToString().Trim();
            txthighRCTRateg.Value = dt.Rows[6]["HRCTRT"].ToString().Trim();
            txthighRCTRateh.Value = dt.Rows[7]["HRCTRT"].ToString().Trim();
            
            // fetching normal RCT Rate
            txtNormalRCTRatea.Value = dt.Rows[0]["NRCTRT"].ToString().Trim() ;
            txtNormalRCTRateb.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();
            txtNormalRCTRatec.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();
            txtNormalRCTRated.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();
            txtNormalRCTRatee.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();
            txtNormalRCTRatef.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();
            txtNormalRCTRateg.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();
            txtNormalRCTRateh.Value = dt.Rows[0]["NRCTRT"].ToString().Trim();


            //default values
            txtRCTGrade1.Value = "1";
            txtRCTGrade2.Value = "1";
            txtRCTGrade3.Value = "1";
            txtRCTGrade4.Value = "2";
            txtRCTGrade5.Value = "2";
     
            txtYNAdmin.Value = "1";
            txtYNFuel.Value = "1";
            txtYNlabor.Value = "1";
            txtYNOtherM.Value = "1";
            txtYNPow.Value = "1";
            txtYNPrint.Value = "1";
            txtYNPVA.Value = "1";
            txtYNStchPins.Value = "0";
            txtYNStrch.Value = "1";
            txtYNTrans.Value = "1";

            txtRateContri.Value = "10";
            txtRatePaperWst.Value = "5";

            txtL.Value = "100";
            txtWid.Value = "100";
            txtHeight.Value = "100";
            txtCs.Value = "250";
           
            txtGSM1.Value = "100";
            txtGSM2.Value = "100";
            txtGSM3.Value = "100";
            txtGSM4.Value = "100";
            txtGSM5.Value = "100";

            txtRateAdmin.Value = ".50";
            txtRateFuel.Value = "6";
            txtRatelabor.Value = "1.75";
            txtRateOtherM.Value = ".1";
            txtRatePow.Value = "7.5";
            txtRatePrint.Value = "150";
            txtRatePVA.Value = "170";
            txtRateStchPins.Value = "70";
            txtRateStrch.Value = "34";
            txtRateTrans.Value = ".5";

        }
            }
    
    

}