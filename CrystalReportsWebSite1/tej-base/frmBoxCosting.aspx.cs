using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class frmBoxCosting : System.Web.UI.Page
{
    DataTable dtb, dtb1, dt;
    DataTable dt1;
    DataRow dbrow, dr1;
    DataSet oDS;
    string btnval, col1, col2, col3, fill_Date, vip = "",Checked_ok;
    string mq0, pk_error = "Y", chk_rights = "N", tmp_var, DateRange;
    static string EntryMode;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_vchdate, Squery, frm_formID, frm_UserID;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        frm_tabname = "somas_anx";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
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
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    //frm_datrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    tmp_var = "A";
                }
            }
            cmdnew.Focus();

            fill_Date = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                cmdnew.Focus();
                cmbBoxTypes.Items.Clear();
                cmbBoxTypes.Items.Insert(0, "UNIVERSAL");
                cmbBoxTypes.Items.Insert(1, "OVER FLAP RAC");
                cmbBoxTypes.Items.Insert(2, "HALF RAC");
                cmbBoxTypes.Items.Insert(3, "OVER FLAP HALF RAC");
                cmbBoxTypes.Items.Insert(4, "SLEEVE");
                cmbBoxTypes.Items.Insert(5, "TRAY");
                cmbBoxTypes.Items.Insert(6, "SHEET");
            }
            // //myfun();
            //   cal();
        }
    }

    protected void cmdnew_Click(object sender, EventArgs e)
    {
        // for new button popup

        disablectrl();
        //Enable();
        fgen.EnableForm(this.Controls);

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();
        if (chk_rights == "Y")
        {
            hffield.Value = "New";
            make_qry_4_popup();
            cmbBoxTypes.Items.Clear();
            // cmbBoxTypes.Items.Insert(0, "");
            cmbBoxTypes.Items.Insert(0, "UNIVERSAL");
            cmbBoxTypes.Items.Insert(1, "OVER FLAP RAC");
            cmbBoxTypes.Items.Insert(2, "HALF RAC");
            cmbBoxTypes.Items.Insert(3, "OVER FLAP HALF RAC");
            cmbBoxTypes.Items.Insert(4, "SLEEVE");
            cmbBoxTypes.Items.Insert(5, "TRAY");
            cmbBoxTypes.Items.Insert(6, "SHEET");
            fgen.Fn_open_sseek("-", frm_qstr);
            //myfun();
            // cal();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }

    public void disablectrl()
    {
        // for disable/enable some variables
        cmdnew.Disabled = true;
        cmdedit.Disabled = true;
        btnsave.Disabled = false;
        cmddel.Disabled = true;
        cmdprint.Disabled = true;
        btnlist.Disabled = true;
        btnParty.Disabled = false;
        btnItem.Disabled = false;

        btncancel.Visible = true;
        cmdexit.Visible = false;
        btnBFBottom.Disabled = false;
        btnBFFlutes.Disabled = false;
        btnBFMiddle.Disabled = false;
        btnBFTop.Disabled = false;

        //btnhideF.Enabled = true;
        //btnhideF_s.Enabled = true;

    }

    public void enablectrl()
    {
        // for enable/disable some variables
        btnParty.Disabled = true;
        btnItem.Disabled = true;
        cmdnew.Disabled = false;
        cmdedit.Disabled = false;
        btncancel.Visible = false;
        cmddel.Disabled = false;
        btnBFBottom.Disabled = true;
        btnBFFlutes.Disabled = true;
        btnBFMiddle.Disabled = true;
        btnBFTop.Disabled = true;
        cmdexit.Visible = true;
        btnsave.Disabled = true;
        //btnhideF.Enabled = true;
        //btnhideF_s.Enabled = true;

        cmdprint.Disabled = false;
        btnlist.Disabled = false;


    }

    public void clearctrl()
    {
        // for clearing some variables
        hffield.Value = "";
        edmode.Value = "";
    }
    //----------------------------------------------------------------------------------------

    public void set_Val()
    {
        frm_tabname = "Somas_Anx";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAMe", frm_tabname);
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "u_vTY");
    }
    //----------------------------------------------------------------------------------------

    public void make_qry_4_popup()
    {
        // for making query based on button value selected
        btnval = hffield.Value;
        set_Val();
        switch (btnval)
        {

            case "Item":
                frm_sql = "select icode as fstr,iname as Item,icode as erp_code,Irate as Rate  from item where substr(icode,1,1) like '9%'  and length(trim(icode))=8 order by iname";
                break;
            case "Party":
                frm_sql = "Select F.Acode as fstr,F.Aname as PartyName,F.Acode as Code,F.Addr1 as Address From Famst F Where F.Acode like '16%' ORDER BY F.Aname";
                break;
            case "PartyOld_E":
                frm_sql = "Select DISTINCT S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy') AS FSTR, S.VCHNUM,TO_CHAR(S.VCHDATE,'DD/MM/YYYY') AS VOUCHERDATE, F.ANAME AS PARTYNAME,S.ACODE AS PARTYCODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN S.T83 ELSE I.INAME END)AS ITEMNAME FROM  FAMST F,SOMAS_ANX S  LEFT OUTER JOIN ITEM I ON TRIM(I.ICODE) =TRIM(S.ICODE) WHERE  TRIM(F.ACODE)=TRIM(S.ACODE)  and s.branchcd='" + frm_mbr + "' AND S.TYPE='" + frm_vty + "' AND S.VCHDATE " + DateRange + " AND S.ACODE='" + txtPCode.Text + "' ORDER BY S.VCHNUM  DESC";
                break;
            case "State":
                frm_sql = "Select type1 as fstr,name,type1 as Code  From Typegrp Where ID='ES' Order By Name";
                break;

            case "BFTop":
                frm_sql = "Select  DISTINCT S.Icode AS Fstr,I.Iname As SubGroupName,S.Icode As Code ,S.COL16 AS Rate,irate1 as Grade FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'ORDER BY ACODE";
                break;
            case "BFMiddle":
                // frm_sql = "Select Icode AS Fstr,Iname As SubGroupName,Icode As Code,Icat From Item Where Length(Trim(Icode))=4 AND ICODE LIKE '07%' Order By Iname";
                frm_sql = "Select  DISTINCT S.Icode AS Fstr,I.Iname As SubGroupName,S.Icode As Code ,S.COL16 AS Rate,irate1 as Grade FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'ORDER BY ACODE";
                break;
            case "BFBottom":
                // frm_sql = "Select Icode AS Fstr,Iname As SubGroupName,Icode As Code,Icat From Item Where Length(Trim(Icode))=4 AND ICODE LIKE '07%' Order By Iname";
                frm_sql = "Select  DISTINCT S.Icode AS Fstr,I.Iname As SubGroupName,S.Icode As Code ,S.COL16 AS Rate,irate1 as Grade FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'ORDER BY ACODE";
                break;
            case "BFFlutes":
                // frm_sql = "Select Icode AS Fstr,Iname As SubGroupName,Icode As Code,Icat From Item Where Length(Trim(Icode))=4 AND ICODE LIKE '07%' Order By Iname";
                frm_sql = "Select  DISTINCT S.Icode AS Fstr,I.Iname As SubGroupName,S.Icode As Code ,S.COL16 AS Rate,irate1 as Grade FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'ORDER BY ACODE";
                break;
            case "Type":
                frm_sql = "Select '00' AS FSTR,'WITH BREAK-UP' AS SELECTION  ,'00' AS CODE FROM DUAL UNION ALL SELECT '01' AS FSTR,'WITHOUT BREAK-UP' AS SELECTION  ,'01' AS CODE FROM DUAL";
                break;
            case "Row_Add":
            case "Row_Edit":
                frm_sql = "Select x.Type1 as fstr, replace( x.name,'&','') as Stage_Name,x.Type1 as Route_Code from type x  where id='K' and trim(type1) not in " + col2 + " order by x.type1";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "List_E")
                    frm_sql = "Select DISTINCT S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy') AS FSTR, S.VCHNUM,TO_CHAR(S.VCHDATE,'DD/MM/YYYY') AS VOUCHERDATE, F.ANAME AS PARTYNAME,S.ACODE AS PARTYCODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN S.T83 ELSE I.INAME END)AS ITEMNAME FROM  FAMST F,SOMAS_ANX S  LEFT OUTER JOIN ITEM I ON TRIM(I.ICODE) =TRIM(S.ICODE) WHERE  TRIM(F.ACODE)=TRIM(S.ACODE)  and s.branchcd='" + frm_mbr + "' AND S.TYPE='" + frm_vty + "' AND S.VCHDATE " + DateRange + " ORDER BY S.VCHNUM  DESC";
                if (btnval == "New" || btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")

                    frm_sql = "select 'CM' AS FSTR,'Costing' as heading,'CM' as type from dual ";
                if (btnval == "Add" || btnval == "Add_E")
                {
                    if (sg1.Rows.Count > 1)
                    {
                        col1 = ""; col2 = "";
                        foreach (GridViewRow r1 in sg1.Rows)
                        {
                            if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[3].Text.Trim() + "'";
                            else col2 = "'" + r1.Cells[3].Text.Trim() + "'";
                        }
                        frm_sql = "select icode as fstr,iname as Item,icode as erp_code,Irate as Rate  from item where  substr(icode,1,1) like '9%'  and  trim(icode) not in (" + col2 + ") and length(trim(icode))=8 order by iname";
                    }
                    else
                    {
                        frm_sql = "select icode as fstr,iname as Item,icode as erp_code,Irate as Rate from item where substr(icode,1,1) like '9%' and length(trim(icode))=8 order by iname";
                    }
                }
                break;

        }
        if (frm_sql.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        // for save button checking & working
        //myfun();
        cal();
        hffield.Value = "";
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in this form!!");
            return;
        }

        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N" && edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in edit mode!!");
            return;
        }
        else
        {
            fgen.fill_dash(this.Controls);
            if (txtParty.Text == "0")
            {
                fgen.msg("-", "AMSG", "Please Select  Party !! ");
                return;
            }
            if ((txtItem.Text == "0") || (txtItem.Text == ""))
            {
                fgen.msg("-", "AMSG", "Please Select  Item !!");
                return;
            }
            if ((Convert.ToDouble(txtTotal.Text) <= 0) || (txtTotal.Text == "NaN") || (txtTotal.Text == "Infinity"))
            {
                fgen.msg("-", "AMSG", "Total Price Can not be Zero !!");
                return;
            }
            if (sg1.Rows.Count > 1)
            {
                for (int i = 0; i < sg1.Rows.Count - 1; i++)
                {

                    if (Convert.ToString(((TextBox)sg1.Rows[i].FindControl("txtQty")).Text) == "")//txtCol16
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Quantity for Item Code " + sg1.Rows[i].Cells[3].Text.ToString().Trim() + " Can not be Blank."); return;
                    }
                    else if (Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtQty")).Text) <= 0)
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Quantity for Item Code " + sg1.Rows[i].Cells[3].Text.ToString().Trim() + " Can not be Zero or less then Zero "); return;
                    }
                }
            }
            fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
            btnsave.Disabled = true;
        }
    }
    protected void cmdedit_Click(object sender, EventArgs e)
    {
        // for edit button popup
        clearctrl();
        set_Val();
        hffield.Value = "Edit";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
        //myfun();
        //cal();
    }
    protected void cmddel_Click(object sender, EventArgs e)
    {
        // for del button working
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to delete data in this form");
        }
        else
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);
            //myfun();
            // cal();
        }

    }
    //----------------------------------------------------------------------------------------

    protected void cmdexit_Click(object sender, EventArgs e)
    {
        // for exit button working
        //Response.Redirect("~/desktop.aspx?STR=" + frm_qstr);
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    //----------------------------------------------------------------------------------------

    protected void cmdprint_Click(object sender, EventArgs e)
    {
        // for doing print
        hfReport.Value = "";
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        enablectrl();
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    void save_data()
    {
        cal();
        GridCalculation();
        string frm_ent_time = fgen.Fn_curr_dt_time(frm_cocd, frm_qstr);

        dbrow = oDS.Tables[0].NewRow();
        dbrow["BRANCHCD"] = frm_mbr;
        dbrow["TYPE"] = frm_vty;
        dbrow["VCHNUM"] = frm_vnum;

        if (edmode.Value == "Y") dbrow["vchdate"] = ViewState["vchdate"].ToString();
        else dbrow["vchdate"] = System.DateTime.Now.ToString("dd/MM/yyyy");

        if (sg1.Rows.Count == 1)
        {
            #region Costing
            dbrow["Acode"] = txtPCode.Text.Trim().ToUpper();
            dbrow["Icode"] = txtICode.Text.Trim().ToUpper();
            dbrow["T1"] = txtLength.Text.Trim().ToUpper();
            dbrow["T2"] = txtWidth.Text.Trim().ToUpper();
            dbrow["T3"] = txtHeight.Text.Trim().ToUpper();
            dbrow["T4"] = cmbID.Text.Trim().ToUpper();
            dbrow["T5"] = txtDouble.Text.Trim().ToUpper();
            dbrow["T6"] = txtTop.Text.Trim().ToUpper();
            dbrow["T7"] = txtFluteB.Text.Trim().ToUpper();
            dbrow["T8"] = txtMiddle.Text.Trim().ToUpper();
            dbrow["T9"] = txtFluteA.Text.Trim();
            dbrow["T10"] = txtBottom.Text.Trim().ToUpper();
            dbrow["T11"] = txtReel.Text.Trim().ToUpper();
            dbrow["T12"] = txtCut.Text.Trim().ToUpper();
            dbrow["T13"] = txtDeckle.Text.Trim().ToUpper();
            dbrow["T14"] = txtCutSize.Text.Trim().ToUpper();
            dbrow["T15"] = txtSheet.Text.Trim().ToUpper();
            dbrow["T16"] = txtWTop.Text.Trim().ToUpper();

            dbrow["T17"] = txtWMiddle.Text.Trim().ToUpper();
            dbrow["T18"] = txtWBottom.Text.Trim().ToUpper();
            dbrow["T19"] = txtWFlute.Text.Trim().ToUpper();
            dbrow["T20"] = txtSWeight.Text.Trim().ToUpper();
            dbrow["T21"] = txtBS.Text.Trim().ToUpper();
            dbrow["T22"] = txtGSM.Text.Trim().ToUpper();
            dbrow["T23"] = txtECT.Text.Trim().ToUpper();
            dbrow["T24"] = txtBCT.Text.Trim().ToUpper();
            dbrow["T25"] = txtCOBB.Text.Trim().ToUpper();
            dbrow["T26"] = txtMoisture.Text.Trim().ToUpper();
            //dbrow["T27"] = txtPaper .Text.Trim().ToUpper();
            dbrow["T28"] = txtAny.Text.Trim().ToUpper();
            dbrow["T29"] = txtBasic.Text.Trim().ToUpper();
            dbrow["T30"] = txtTotal.Text.Trim().ToUpper();
            dbrow["T31"] = txtMin.Text.Trim().ToUpper();
            dbrow["T32"] = txtPurchase.Text.Trim().ToUpper();
            //dbrow["T33"] = txtBFTop.Text.Trim().ToUpper();
            //dbrow["T34"] = txtBFMiddle.Text.Trim().ToUpper();
            //dbrow["T35"] = txtBFBottom.Text.Trim().ToUpper();
            //dbrow["T36"] = txtBFFlutes.Text.Trim().ToUpper();
            dbrow["T37"] = txtMinQty.Text.Trim().ToUpper();
            //dbrow["T38"] = txtItem.Text.Trim().ToUpper();
            dbrow["T39"] = cmbBoxTypes.Text.Trim().ToUpper();
            dbrow["T40"] = txtBFTop.Text.Trim().ToUpper();
            dbrow["T41"] = txtBFTopRate.Text.Trim().ToUpper();
            dbrow["T42"] = txtBFMiddle.Text.Trim().ToUpper();
            dbrow["T43"] = txtBFMiddleRate.Text.Trim().ToUpper();
            dbrow["T44"] = txtBFBottom.Text.Trim().ToUpper();
            dbrow["T45"] = txtBFBottomRate.Text.Trim().ToUpper();
            dbrow["T46"] = txtBFFlutes.Text.Trim().ToUpper();
            dbrow["T47"] = txtBFFluteRate.Text.Trim().ToUpper();
            dbrow["T48"] = txtRateTop.Text.Trim().ToUpper();
            dbrow["T49"] = txtRateMiddle.Text.Trim().ToUpper();
            dbrow["T50"] = txtRateBottom.Text.Trim().ToUpper();
            dbrow["T51"] = txtRateFlute.Text.Trim().ToUpper();
            dbrow["T52"] = txtMaterial.Text.Trim().ToUpper();
            dbrow["T53"] = txtProcess.Text.Trim().ToUpper();
            dbrow["T54"] = txtProcessRate.Text.Trim().ToUpper();
            dbrow["T55"] = txtBoard.Text.Trim().ToUpper();
            dbrow["T56"] = txtBoardRate.Text.Trim().ToUpper();
            dbrow["T57"] = txtPrinting.Text.Trim().ToUpper();
            dbrow["T58"] = txtPrintingRate.Text.Trim().ToUpper();
            dbrow["T59"] = txtWater.Text.Trim().ToUpper();
            dbrow["T60"] = txtWaterRate.Text.Trim().ToUpper();
            dbrow["T61"] = txtDie.Text.Trim().ToUpper();
            dbrow["T62"] = txtDieRate.Text.Trim().ToUpper();
            dbrow["T63"] = txtStitching.Text.Trim().ToUpper();
            dbrow["T64"] = txtStitchingRate.Text.Trim().ToUpper();
            dbrow["T65"] = txtTaping.Text.Trim().ToUpper();
            dbrow["T66"] = txtTapingRate.Text.Trim().ToUpper();
            dbrow["T67"] = txtPacking.Text.Trim().ToUpper();
            dbrow["T68"] = txtProfit.Text.Trim().ToUpper();
            dbrow["T69"] = txtProfitRate.Text.Trim().ToUpper();
            dbrow["T70"] = txtFreight.Text.Trim().ToUpper();
            dbrow["T71"] = txtFreightRate.Text.Trim().ToUpper();
            dbrow["T72"] = txtPymt.Text.Trim().ToUpper();
            dbrow["T73"] = txtPymtRate.Text.Trim().ToUpper();
            dbrow["T74"] = txtExcise.Text.Trim().ToUpper();
            dbrow["T75"] = txtExciseRate.Text.Trim().ToUpper();
            dbrow["T76"] = txtSales.Text.Trim().ToUpper();
            dbrow["T77"] = txtSalesRate.Text.Trim().ToUpper();
            //dbrow["T78"] = txtFreight.Text.Trim().ToUpper();
            //dbrow["T79"] = txtFreight.Text.Trim().ToUpper();
            //dbrow["T80"] = txtFreight.Text.Trim().ToUpper();
            //dbrow["T81"] = txtFreight.Text.Trim().ToUpper();

            dbrow["T140"] = "0";

            if (edmode.Value == "Y")
            {
                dbrow["ent_by"] = ViewState["ent_by"].ToString();
                dbrow["ent_dt"] = ViewState["ent_Dt"].ToString();
                dbrow["edt_by"] = frm_uname;
                dbrow["edt_dt"] = System.DateTime.Now;
            }
            else
            {
                dbrow["ent_by"] = frm_uname;
                dbrow["ent_dt"] = System.DateTime.Now;
                dbrow["edt_by"] = "-";
                dbrow["edt_dt"] = System.DateTime.Now;
            }
            if (EntryMode == "MANUAL")
            {
                dbrow["T84"] = "MANUAL";
                if (txtItem.Text.Trim().Length > 30)
                {
                    dbrow["T83"] = txtItem.Text.Trim().Substring(0, 29).ToUpper();
                }
                else
                {
                    dbrow["T83"] = txtItem.Text.Trim().ToUpper();
                }
            }
            dbrow["T85"] = txtBfTopG.Text.Trim().ToUpper();
            dbrow["T86"] = txtBfMiddleG.Text.Trim().ToUpper();
            dbrow["T87"] = txtBfBottomG.Text.Trim().ToUpper();
            dbrow["T88"] = txtBfFluteG.Text.Trim().ToUpper();
            oDS.Tables[0].Rows.Add(dbrow);
            #endregion
        }
        else
        {
            for (int i = 0; i < sg1.Rows.Count - 1; i++)
            {
                dbrow = oDS.Tables[0].NewRow();
                dbrow["BRANCHCD"] = frm_mbr;
                dbrow["TYPE"] = frm_vty;
                dbrow["VCHNUM"] = frm_vnum;

                if (edmode.Value == "Y") dbrow["vchdate"] = ViewState["vchdate"].ToString();
                else dbrow["vchdate"] = System.DateTime.Now.ToString("dd/MM/yyyy");

                dbrow["t140"] = i + 1;
                #region Costing
                dbrow["Acode"] = txtPCode.Text.Trim().ToUpper();
                dbrow["Icode"] = txtICode.Text.Trim().ToUpper();

                dbrow["T1"] = txtLength.Text.Trim().ToUpper();
                dbrow["T2"] = txtWidth.Text.Trim().ToUpper();
                dbrow["T3"] = txtHeight.Text.Trim().ToUpper();
                dbrow["T4"] = cmbID.Text.Trim().ToUpper();
                dbrow["T5"] = txtDouble.Text.Trim().ToUpper();
                dbrow["T6"] = txtTop.Text.Trim().ToUpper();
                dbrow["T7"] = txtFluteB.Text.Trim().ToUpper();
                dbrow["T8"] = txtMiddle.Text.Trim().ToUpper();
                dbrow["T9"] = txtFluteA.Text.Trim();
                dbrow["T10"] = txtBottom.Text.Trim().ToUpper();
                dbrow["T11"] = txtReel.Text.Trim().ToUpper();
                dbrow["T12"] = txtCut.Text.Trim().ToUpper();
                dbrow["T13"] = txtDeckle.Text.Trim().ToUpper();
                dbrow["T14"] = txtCutSize.Text.Trim().ToUpper();
                dbrow["T15"] = txtSheet.Text.Trim().ToUpper();
                dbrow["T16"] = txtWTop.Text.Trim().ToUpper();

                dbrow["T17"] = txtWMiddle.Text.Trim().ToUpper();
                dbrow["T18"] = txtWBottom.Text.Trim().ToUpper();
                dbrow["T19"] = txtWFlute.Text.Trim().ToUpper();
                dbrow["T20"] = txtSWeight.Text.Trim().ToUpper();
                dbrow["T21"] = txtBS.Text.Trim().ToUpper();
                dbrow["T22"] = txtGSM.Text.Trim().ToUpper();
                dbrow["T23"] = txtECT.Text.Trim().ToUpper();
                dbrow["T24"] = txtBCT.Text.Trim().ToUpper();
                dbrow["T25"] = txtCOBB.Text.Trim().ToUpper();
                dbrow["T26"] = txtMoisture.Text.Trim().ToUpper();
                //dbrow["T27"] = txtPaper .Text.Trim().ToUpper();
                dbrow["T28"] = txtAny.Text.Trim().ToUpper();
                dbrow["T29"] = txtBasic.Text.Trim().ToUpper();
                dbrow["T30"] = txtTotal.Text.Trim().ToUpper();
                dbrow["T31"] = txtMin.Text.Trim().ToUpper();
                dbrow["T32"] = txtPurchase.Text.Trim().ToUpper();
                //dbrow["T33"] = txtBFTop.Text.Trim().ToUpper();
                //dbrow["T34"] = txtBFMiddle.Text.Trim().ToUpper();
                //dbrow["T35"] = txtBFBottom.Text.Trim().ToUpper();
                //dbrow["T36"] = txtBFFlutes.Text.Trim().ToUpper();
                dbrow["T37"] = txtMinQty.Text.Trim().ToUpper();
                //  dbrow["T38"] = txtItem.Text.Trim().ToUpper();
                dbrow["T39"] = cmbBoxTypes.Text.Trim().ToUpper();
                dbrow["T40"] = txtBFTop.Text.Trim().ToUpper();
                dbrow["T41"] = txtBFTopRate.Text.Trim().ToUpper();
                dbrow["T42"] = txtBFMiddle.Text.Trim().ToUpper();
                dbrow["T43"] = txtBFMiddleRate.Text.Trim().ToUpper();
                dbrow["T44"] = txtBFBottom.Text.Trim().ToUpper();
                dbrow["T45"] = txtBFBottomRate.Text.Trim().ToUpper();
                dbrow["T46"] = txtBFFlutes.Text.Trim().ToUpper();
                dbrow["T47"] = txtBFFluteRate.Text.Trim().ToUpper();
                dbrow["T48"] = txtRateTop.Text.Trim().ToUpper();
                dbrow["T49"] = txtRateMiddle.Text.Trim().ToUpper();
                dbrow["T50"] = txtRateBottom.Text.Trim().ToUpper();
                dbrow["T51"] = txtRateFlute.Text.Trim().ToUpper();
                dbrow["T52"] = txtMaterial.Text.Trim().ToUpper();
                dbrow["T53"] = txtProcess.Text.Trim().ToUpper();
                dbrow["T54"] = txtProcessRate.Text.Trim().ToUpper();
                dbrow["T55"] = txtBoard.Text.Trim().ToUpper();
                dbrow["T56"] = txtBoardRate.Text.Trim().ToUpper();
                dbrow["T57"] = txtPrinting.Text.Trim().ToUpper();
                dbrow["T58"] = txtPrintingRate.Text.Trim().ToUpper();
                dbrow["T59"] = txtWater.Text.Trim().ToUpper();
                dbrow["T60"] = txtWaterRate.Text.Trim().ToUpper();
                dbrow["T61"] = txtDie.Text.Trim().ToUpper();
                dbrow["T62"] = txtDieRate.Text.Trim().ToUpper();
                dbrow["T63"] = txtStitching.Text.Trim().ToUpper();
                dbrow["T64"] = txtStitchingRate.Text.Trim().ToUpper();
                dbrow["T65"] = txtTaping.Text.Trim().ToUpper();
                dbrow["T66"] = txtTapingRate.Text.Trim().ToUpper();
                dbrow["T67"] = txtPacking.Text.Trim().ToUpper();
                dbrow["T68"] = txtProfit.Text.Trim().ToUpper();
                dbrow["T69"] = txtProfitRate.Text.Trim().ToUpper();
                dbrow["T70"] = txtFreight.Text.Trim().ToUpper();
                dbrow["T71"] = txtFreightRate.Text.Trim().ToUpper();
                dbrow["T72"] = txtPymt.Text.Trim().ToUpper();
                dbrow["T73"] = txtPymtRate.Text.Trim().ToUpper();
                dbrow["T74"] = txtExcise.Text.Trim().ToUpper();
                dbrow["T75"] = txtExciseRate.Text.Trim().ToUpper();
                dbrow["T76"] = txtSales.Text.Trim().ToUpper();
                dbrow["T77"] = txtSalesRate.Text.Trim().ToUpper();
                dbrow["T78"] = sg1.Rows[i].Cells[3].Text.Trim().ToUpper();

                //dbrow["T79"] = sg1.Rows[i].Cells[4].Text.Trim().ToUpper();
                dbrow["T80"] = ((TextBox)sg1.Rows[i].FindControl("txtQty")).Text.Trim();
                dbrow["T81"] = ((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text.Trim();
                dbrow["T82"] = txtGrdTotal.Text.Trim().ToUpper();
                if (EntryMode == "MANUAL")
                {
                    dbrow["T84"] = "MANUAL";
                    if (txtItem.Text.Trim().Length > 30)
                    {
                        dbrow["T83"] = txtItem.Text.Trim().Substring(0, 29).ToUpper();
                    }

                    else
                    {
                        dbrow["T83"] = txtItem.Text.Trim().ToUpper();
                    }
                }
                dbrow["T85"] = txtBfTopG.Text.Trim().ToUpper();
                dbrow["T86"] = txtBfMiddleG.Text.Trim().ToUpper();
                dbrow["T87"] = txtBfBottomG.Text.Trim().ToUpper();
                dbrow["T88"] = txtBfFluteG.Text.Trim().ToUpper();
                if (edmode.Value == "Y")
                {
                    dbrow["ent_by"] = ViewState["ent_by"].ToString();
                    dbrow["ent_dt"] = ViewState["ent_Dt"].ToString();
                    dbrow["edt_by"] = frm_uname;
                    dbrow["edt_dt"] = System.DateTime.Now;
                }
                else
                {
                    dbrow["ent_by"] = frm_uname;
                    dbrow["ent_dt"] = System.DateTime.Now;
                    dbrow["edt_by"] = "-";
                    dbrow["edt_dt"] = System.DateTime.Now;
                }
                #endregion
                oDS.Tables[0].Rows.Add(dbrow);
            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        // for doing save action 
        if (hffield.Value == "List_E")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            frm_sql = "Select DISTINCT  S.VCHNUM,TO_CHAR(S.VCHDATE,'DD/MM/YYYY') AS VOCHERDATE, F.ANAME AS PARTYNAME,S.ACODE AS PARTYCODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN S.T83 ELSE I.INAME END)AS ITEMNAME FROM  FAMST F,SOMAS_ANX S  LEFT OUTER JOIN ITEM I ON TRIM(I.ICODE) =TRIM(S.ICODE) WHERE  TRIM(F.ACODE)=TRIM(S.ACODE) and S.branchcd='" + frm_mbr + "' and S.type='" + frm_vty.Trim() + "' and S.vchdate " + DateRange + " order by S.vchnum DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
            fgen.Fn_open_rptlevel("-", frm_qstr);
        }
        else if (hffield.Value == "CMD_REP1")
        {
            Squery = "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum");
            fgen.Fn_open_rptlevel("Stage Entry List", frm_qstr);
        }
        else if (hffield.Value == "LINV" || hffield.Value == "LSO")
        {
            col3 = "S.O. Details for the period ";
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (hffield.Value == "LINV")
            {
                frm_sql = "select distinct  a.vchnum as Bill_No,to_char(a.vchdate,'dd/mm/yyyy') as bil_date,a.icode as ERP_code,b.aname as Party_Name,a.irate as Basic_rate from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.type like '4%' and a.type!='47' and trim(a.icode)='" + txtICode.Text.Trim() + "' and a.vchdate " + DateRange + " and trim(A.acode)='" + txtPCode.Text.Trim() + "' order by a.vchnum desc";
                col3 = "Invoice Details for the period ";
            }
            else if (hffield.Value == "LSO") frm_sql = "select distinct a.ordno as SO_no,to_char(a.orddt,'dd/mm/yyyy') as SO_date,a.icode as erp_code,b.aname as Party_Name,a.irate as Basic_rate from somas a,famst b where trim(a.acode)=trim(b.acode) and a.type like '4%' and a.type!='47' and a.orddt " + DateRange + " and trim(a.icode)='" + txtICode.Text.Trim() + "' and trim(A.acode)='" + txtPCode.Text.Trim() + "' order by a.ordno";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
            fgen.Fn_open_rptlevel(col3 + col1 + " to " + col2, frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            col1 = "";
            set_Val();
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
                        set_Val();
                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE SOMAS_ANX SET BRANCHCD='DD' WHERE branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.Trim() + "'");
                        }

                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";

                        save_data();

                        oDS.Dispose();
                        dbrow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        frm_vchdate = System.DateTime.Now.ToString("dd/MM/yyyy");
                        if (edmode.Value == "Y")
                        {
                            frm_vnum = ViewState["VCHNUM"].ToString().Trim();
                            frm_vchdate = ViewState["vchdate"].ToString().Trim();
                        }
                        else
                        {
                            int i = 0;
                            do
                            {
                                if (frm_mbr.Length != 2 || frm_mbr.Length > 2 || frm_vty.Length != 2 || frm_vty.Length > 2)
                                {
                                    fgen.msg("-", "AMSG", "Connection Error !! Please Login Again.");
                                    return;
                                }

                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("yyyy"), frm_mbr, frm_vty, frm_vnum, frm_vchdate, "", frm_uname);
                                i++;
                            }
                            while (pk_error == "Y");
                        }

                        save_data();

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        //fgen.send_mail("Tejaxo ERP","info@pocketdriver.in","","","ITEWSTAGE",""

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", "Doc No." + frm_vnum + "  Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.Trim().Substring(2, 18) + "'");
                        }
                        else { fgen.msg("-", "AMSG", "Doc No." + frm_vnum + " Saved Successfully "); }
                        ViewState["sg1"] = null; sg1.DataSource = null; sg1.DataBind();
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl();
                        txtICode.Visible = true; btnItem.Visible = true;
                        clearctrl();
                        col1 = "N";
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                    }
                }
            }
        }
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        // for doing multiple work on postback 
        set_Val();
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_Ctrl a where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy')='" + popselected.Value.ToUpper() + "'");
                string A = popselected.Value.Substring(4, 6);
                //fgen.save_info(frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), System.DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, frm_vty, "Costing Sheet Deleted");
                fgen.msg("-", "AMSG", "Details are deleted for  Doc No." + popselected.Value.Substring(4, 6) + "");
                // clearctrl(); 
                fgen.ResetForm(this.Controls);

            }
        }
        else
        {

            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "CMD_REP1":
                    if (col1.Length > 0) { }
                    else return;
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    fgen.Fn_open_prddmp1("Select Date Range for List Of Stage Routing", frm_qstr);
                    break;

                case "New":
                    if (col1.Length > 0) { }
                    else return;
                    clearctrl();
                    set_Val();
                    popselected.Value = col1;
                    frm_vty = col1;

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    cmbBoxTypes.Focus();
                    //  DataTable   dt1 = new DataTable();
                    create_tab();
                    add_blankrows();
                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    dt1.Dispose(); dt1.Dispose();
                    disablectrl();

                    fgen.EnableForm(this.Controls);
                    break;
                case "Del":
                    if (col1.Length > 0) { }
                    else return;
                    clearctrl();
                    set_Val();
                    hffield.Value = "Del_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    frm_vty = col1;
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;
                case "Del_E":
                    if (col1.Length > 0) { }
                    else return;
                    clearctrl();
                    popselected.Value = col1;

                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Edit":
                    if (col1.Length > 0) { }
                    else return;
                    // this is after type selection 
                    clearctrl();
                    set_Val();
                    hffield.Value = "Edit_E";
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    frm_vty = col1;
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;
                case "Edit_E":
                    if (col1.Length > 0) { }
                    else return;
                    popselected.Value = col1;
                    Squery = "SELECT S.*,I.INAME FROM SOMAS_ANX S LEFT OUTER JOIN ITEM I ON TRIM(S.ICODE)=TRIM(I.ICODE) WHERE S.TYPE='" + frm_vty + "' AND S.branchcd||S.type||trim(S.vchnum)||to_char(S.vchdate,'dd/mm/yyyy')='" + col1.Trim() + "'";
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr, frm_cocd, Squery);
                    ViewState["VCHNUM"] = dtb.Rows[0]["VCHNUM"].ToString();
                    ViewState["vchdate"] = Convert.ToDateTime(dtb.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy");
                    ViewState["ent_by"] = dtb.Rows[0]["ent_by"].ToString();
                    ViewState["ent_Dt"] = dtb.Rows[0]["ent_dt"].ToString();

                    txtPCode.Text = dtb.Rows[0]["ACODE"].ToString().Trim();
                    DataTable dtAname = fgen.getdata(frm_qstr, frm_cocd, "Select Aname From Famst Where Acode='" + txtPCode.Text + "'");
                    txtParty.Text = dtAname.Rows[0]["Aname"].ToString().Trim();
                    txtICode.Text = dtb.Rows[0]["ICODE"].ToString().Trim();
                    if (dtb.Rows[0]["T84"].ToString() == "MANUAL")
                    {
                        txtItem.Text = dtb.Rows[0]["T83"].ToString().Trim();
                        EntryMode = "MANUAL";
                    }
                    else
                    {
                        txtItem.Text = dtb.Rows[0]["Iname"].ToString().Trim();
                    }
                    #region Costing
                    txtLength.Text = dtb.Rows[0]["T1"].ToString().Trim();
                    txtWidth.Text = dtb.Rows[0]["T2"].ToString().Trim();
                    txtHeight.Text = dtb.Rows[0]["T3"].ToString().Trim();
                    cmbID.Text = dtb.Rows[0]["T4"].ToString().Trim();
                    txtDouble.Text = dtb.Rows[0]["T5"].ToString().Trim();
                    txtTop.Text = dtb.Rows[0]["T6"].ToString().Trim();
                    txtFluteB.Text = dtb.Rows[0]["T7"].ToString().Trim();
                    txtMiddle.Text = dtb.Rows[0]["T8"].ToString().Trim();
                    txtFluteA.Text = dtb.Rows[0]["T9"].ToString().Trim();
                    txtBottom.Text = dtb.Rows[0]["T10"].ToString().Trim();
                    txtReel.Text = dtb.Rows[0]["T11"].ToString().Trim();
                    txtCut.Text = dtb.Rows[0]["T12"].ToString().Trim();
                    txtDeckle.Text = dtb.Rows[0]["T13"].ToString().Trim();
                    txtCutSize.Text = dtb.Rows[0]["T14"].ToString().Trim();
                    txtSheet.Text = dtb.Rows[0]["T15"].ToString().Trim();
                    txtWTop.Text = dtb.Rows[0]["T16"].ToString().Trim();
                    txtWMiddle.Text = dtb.Rows[0]["T17"].ToString().Trim();
                    txtWBottom.Text = dtb.Rows[0]["T18"].ToString().Trim();
                    txtWFlute.Text = dtb.Rows[0]["T19"].ToString().Trim();
                    txtSWeight.Text = dtb.Rows[0]["T20"].ToString().Trim();
                    txtBS.Text = dtb.Rows[0]["T21"].ToString().Trim();
                    txtGSM.Text = dtb.Rows[0]["T22"].ToString().Trim();
                    txtECT.Text = dtb.Rows[0]["T23"].ToString().Trim();
                    txtBCT.Text = dtb.Rows[0]["T24"].ToString().Trim();
                    txtCOBB.Text = dtb.Rows[0]["T25"].ToString().Trim();
                    txtMoisture.Text = dtb.Rows[0]["T26"].ToString().Trim();
                    //txtPaper.Text = dtb.Rows[0]["T27"].ToString().Trim();
                    txtAny.Text = dtb.Rows[0]["T28"].ToString().Trim();
                    txtBasic.Text = dtb.Rows[0]["T29"].ToString().Trim();
                    txtTotal.Text = dtb.Rows[0]["T30"].ToString().Trim();
                    txtMin.Text = dtb.Rows[0]["T31"].ToString().Trim();
                    txtPurchase.Text = dtb.Rows[0]["T32"].ToString().Trim();
                    //txtBFTop.Text=dtb.Rows[0]["T33"].ToString().Trim();
                    //txtBFMiddle.Text = dtb.Rows[0]["T34"].ToString().Trim();
                    //txtBFBottom.Text = dtb.Rows[0]["T35"].ToString().Trim();
                    //txtBFFlutes.Text = dtb.Rows[0]["T36"].ToString().Trim();
                    txtMinQty.Text = dtb.Rows[0]["T37"].ToString().Trim();

                    // txtItem.Text = dtb.Rows[0]["T38"].ToString().Trim();
                    cmbBoxTypes.Text = dtb.Rows[0]["T39"].ToString().Trim();
                    txtBFTop.Text = dtb.Rows[0]["T40"].ToString().Trim();
                    txtBFTopRate.Text = dtb.Rows[0]["T41"].ToString().Trim();
                    txtBFMiddle.Text = dtb.Rows[0]["T42"].ToString().Trim();
                    txtBFMiddleRate.Text = dtb.Rows[0]["T43"].ToString().Trim();
                    txtBFBottom.Text = dtb.Rows[0]["T44"].ToString().Trim();
                    txtBFBottomRate.Text = dtb.Rows[0]["T45"].ToString().Trim();
                    txtBFFlutes.Text = dtb.Rows[0]["T46"].ToString().Trim();
                    txtBFFluteRate.Text = dtb.Rows[0]["T47"].ToString().Trim();
                    txtRateTop.Text = dtb.Rows[0]["T48"].ToString().Trim();
                    txtRateMiddle.Text = dtb.Rows[0]["T49"].ToString().Trim();
                    txtRateBottom.Text = dtb.Rows[0]["T50"].ToString().Trim();
                    txtRateFlute.Text = dtb.Rows[0]["T51"].ToString().Trim();
                    txtMaterial.Text = dtb.Rows[0]["T52"].ToString().Trim();
                    txtProcess.Text = dtb.Rows[0]["T53"].ToString().Trim();
                    txtProcessRate.Text = dtb.Rows[0]["T54"].ToString().Trim();
                    txtBoard.Text = dtb.Rows[0]["T55"].ToString().Trim();
                    txtBoardRate.Text = dtb.Rows[0]["T56"].ToString().Trim();
                    txtPrinting.Text = dtb.Rows[0]["T57"].ToString().Trim();
                    txtPrintingRate.Text = dtb.Rows[0]["T58"].ToString().Trim();
                    txtWater.Text = dtb.Rows[0]["T59"].ToString().Trim();
                    txtWaterRate.Text = dtb.Rows[0]["T60"].ToString().Trim();
                    txtDie.Text = dtb.Rows[0]["T61"].ToString().Trim();
                    txtDieRate.Text = dtb.Rows[0]["T62"].ToString().Trim();
                    txtStitching.Text = dtb.Rows[0]["T63"].ToString().Trim();
                    txtStitchingRate.Text = dtb.Rows[0]["T64"].ToString().Trim();
                    txtTaping.Text = dtb.Rows[0]["T65"].ToString().Trim();
                    txtTapingRate.Text = dtb.Rows[0]["T66"].ToString().Trim();
                    txtPacking.Text = dtb.Rows[0]["T67"].ToString().Trim();
                    txtProfit.Text = dtb.Rows[0]["T68"].ToString().Trim();
                    txtProfitRate.Text = dtb.Rows[0]["T69"].ToString().Trim();
                    txtFreight.Text = dtb.Rows[0]["T70"].ToString().Trim();
                    txtFreightRate.Text = dtb.Rows[0]["T71"].ToString().Trim();
                    txtPymt.Text = dtb.Rows[0]["T72"].ToString().Trim();
                    txtPymtRate.Text = dtb.Rows[0]["T73"].ToString().Trim();
                    txtExcise.Text = dtb.Rows[0]["T74"].ToString().Trim();
                    txtExciseRate.Text = dtb.Rows[0]["T75"].ToString().Trim();
                    txtSales.Text = dtb.Rows[0]["T76"].ToString().Trim();
                    txtSalesRate.Text = dtb.Rows[0]["T77"].ToString().Trim();
                    txtGrdTotal.Text = dtb.Rows[0]["T82"].ToString().Trim();
                    txtBfTopG.Text = dtb.Rows[0]["T85"].ToString().Trim();
                    txtBfMiddleG.Text = dtb.Rows[0]["T86"].ToString().Trim();
                    txtBfBottomG.Text = dtb.Rows[0]["T87"].ToString().Trim();
                    txtBfFluteG.Text = dtb.Rows[0]["T88"].ToString().Trim();
                    #endregion

                    create_tab();
                    int count = 0;
                    foreach (DataRow dr in dtb.Rows)
                    {
                        dr1 = dt1.NewRow();
                        if (dr["T140"].ToString() != "0")
                        {
                            dr1["srno"] = count + 1;

                            dr1["icode"] = dr["T78"].ToString();
                            DataTable dtGridIname = fgen.getdata(frm_qstr, frm_cocd, "Select iname From Item Where Icode='" + dr1["icode"].ToString().Trim() + "'");
                            if (dtGridIname.Rows.Count > 0)
                            {
                                dr1["iname"] = dtGridIname.Rows[0][0].ToString();
                            }

                            dr1["Irate"] = dr["T81"];
                            dr1["T80"] = dr["T80"];
                            dt1.Rows.Add(dr1);
                            count++;
                        }
                    }
                    add_blankrows();
                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    fgen.EnableForm(this.Controls);
                    //myfun();
                    cal();
                    disablectrl();
                    edmode.Value = "Y";
                    btnParty.Disabled = true;
                    break;
                case "Party":
                    if (col1.Length > 0) { }
                    else return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select aname from famst where trim(acode)='" + col1 + "'");
                    txtPCode.Text = col1;
                    txtParty.Text = dt.Rows[0]["aname"].ToString();
                    hffield.Value = "Party_E";
                    dt.Dispose();
                    //myfun();
                    cal();
                    //  fgen.DisableForm(this.Controls);
                    fgen.msg("-", "CMSG", "Do you want to copy Costing Sheet from old one");
                    // GridCal();
                    // GridCalculation();
                    break;
                case "Party_E":
                    if (col1.Length > 0) { }
                    else return;
                    col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    if (col1 == "Y")
                    {
                        hffield.Value = "PartyOld_E";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("", frm_qstr);
                    }
                    else
                    {
                        DataTable dtShow = fgen.getdata(frm_qstr, frm_cocd, "Select S.* ,I.INAME  FROM SCRATCH S , ITEM I WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "' ORDER BY ACODE");
                        if (dtShow.Rows.Count > 0)
                        {
                            txtProcess.Text = dtShow.Rows[0]["Col3"].ToString().Replace("-", "0");
                            txtBoard.Text = dtShow.Rows[0]["Col4"].ToString().Replace("-", "0");
                            txtPrinting.Text = dtShow.Rows[0]["Col5"].ToString().Replace("-", "0");
                            txtWater.Text = dtShow.Rows[0]["Col6"].ToString().Replace("-", "0");
                            txtDie.Text = dtShow.Rows[0]["Col7"].ToString().Replace("-", "0");
                            txtStitching.Text = dtShow.Rows[0]["Col8"].ToString().Replace("-", "0");
                            txtTaping.Text = dtShow.Rows[0]["Col9"].ToString().Replace("-", "0");
                            txtPacking.Text = dtShow.Rows[0]["Col10"].ToString().Replace("-", "0");
                            txtProfit.Text = dtShow.Rows[0]["Col11"].ToString().Replace("-", "0");
                            txtFreight.Text = dtShow.Rows[0]["Col12"].ToString().Replace("-", "0");
                            txtPymt.Text = dtShow.Rows[0]["Col13"].ToString().Replace("-", "0");
                            txtExcise.Text = dtShow.Rows[0]["Col14"].ToString().Replace("-", "0");
                            txtSales.Text = dtShow.Rows[0]["Col15"].ToString().Replace("-", "0");
                            txtMinQty.Text = dtShow.Rows[0]["Col17"].ToString().Replace("-", "0");
                        }
                        btnItem.Focus();
                        disablectrl();
                        //fgen.EnableForm(this.Controls);
                    }

                    //myfun();
                    cal();
                    break;
                case "PartyOld_E":
                    if (col1.Length > 0) { }
                    else return;
                    popselected.Value = col1;
                    Squery = "SELECT S. *,I.INAME FROM SOMAS_ANX S LEFT OUTER JOIN ITEM I ON TRIM(S.ICODE)=TRIM(I.ICODE) WHERE S.TYPE='" + frm_vty + "' AND S.branchcd||S.type||trim(S.vchnum)||to_char(S.vchdate,'dd/mm/yyyy')='" + col1.Trim() + "'";
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr, frm_cocd, Squery);

                    ViewState["VCHNUM"] = dtb.Rows[0]["VCHNUM"].ToString();
                    ViewState["vchdate"] = Convert.ToDateTime(dtb.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy");
                    ViewState["ent_by"] = dtb.Rows[0]["ent_by"].ToString();
                    ViewState["ent_Dt"] = dtb.Rows[0]["ent_dt"].ToString();

                    txtPCode.Text = dtb.Rows[0]["ACODE"].ToString().Trim();
                    DataTable dtParty = fgen.getdata(frm_qstr, frm_cocd, "Select Aname From Famst Where Acode='" + txtPCode.Text + "'");
                    txtParty.Text = dtParty.Rows[0]["Aname"].ToString().Trim();
                    txtICode.Text = dtb.Rows[0]["ICODE"].ToString().Trim();
                    if (dtb.Rows[0]["T84"].ToString() == "MANUAL")
                    {
                        txtItem.Text = dtb.Rows[0]["T83"].ToString().Trim();
                        EntryMode = "MANUAL";
                    }
                    else
                    {
                        txtItem.Text = dtb.Rows[0]["Iname"].ToString().Trim();
                    }
                    #region Costing
                    txtLength.Text = dtb.Rows[0]["T1"].ToString().Trim();
                    txtWidth.Text = dtb.Rows[0]["T2"].ToString().Trim();
                    txtHeight.Text = dtb.Rows[0]["T3"].ToString().Trim();
                    cmbID.Text = dtb.Rows[0]["T4"].ToString().Trim();
                    txtDouble.Text = dtb.Rows[0]["T5"].ToString().Trim();
                    txtTop.Text = dtb.Rows[0]["T6"].ToString().Trim();
                    txtFluteB.Text = dtb.Rows[0]["T7"].ToString().Trim();
                    txtMiddle.Text = dtb.Rows[0]["T8"].ToString().Trim();
                    txtFluteA.Text = dtb.Rows[0]["T9"].ToString().Trim();
                    txtBottom.Text = dtb.Rows[0]["T10"].ToString().Trim();
                    txtReel.Text = dtb.Rows[0]["T11"].ToString().Trim();
                    txtCut.Text = dtb.Rows[0]["T12"].ToString().Trim();
                    txtDeckle.Text = dtb.Rows[0]["T13"].ToString().Trim();
                    txtCutSize.Text = dtb.Rows[0]["T14"].ToString().Trim();
                    txtSheet.Text = dtb.Rows[0]["T15"].ToString().Trim();
                    txtWTop.Text = dtb.Rows[0]["T16"].ToString().Trim();
                    txtWMiddle.Text = dtb.Rows[0]["T17"].ToString().Trim();
                    txtWBottom.Text = dtb.Rows[0]["T18"].ToString().Trim();
                    txtWFlute.Text = dtb.Rows[0]["T19"].ToString().Trim();
                    txtSWeight.Text = dtb.Rows[0]["T20"].ToString().Trim();
                    txtBS.Text = dtb.Rows[0]["T21"].ToString().Trim();
                    txtGSM.Text = dtb.Rows[0]["T22"].ToString().Trim();
                    txtECT.Text = dtb.Rows[0]["T23"].ToString().Trim();
                    txtBCT.Text = dtb.Rows[0]["T24"].ToString().Trim();
                    txtCOBB.Text = dtb.Rows[0]["T25"].ToString().Trim();
                    txtMoisture.Text = dtb.Rows[0]["T26"].ToString().Trim();
                    //txtPaper.Text = dtb.Rows[0]["T27"].ToString().Trim();
                    txtAny.Text = dtb.Rows[0]["T28"].ToString().Trim();
                    txtBasic.Text = dtb.Rows[0]["T29"].ToString().Trim();
                    txtTotal.Text = dtb.Rows[0]["T30"].ToString().Trim();
                    txtMin.Text = dtb.Rows[0]["T31"].ToString().Trim();
                    txtPurchase.Text = dtb.Rows[0]["T32"].ToString().Trim();
                    txtBFTop.Text = dtb.Rows[0]["T33"].ToString().Trim();
                    txtBFMiddle.Text = dtb.Rows[0]["T34"].ToString().Trim();
                    txtBFBottom.Text = dtb.Rows[0]["T35"].ToString().Trim();
                    txtBFFlutes.Text = dtb.Rows[0]["T36"].ToString().Trim();
                    txtMinQty.Text = dtb.Rows[0]["T37"].ToString().Trim();

                    // txtItem.Text = dtb.Rows[0]["T38"].ToString().Trim();
                    cmbBoxTypes.Text = dtb.Rows[0]["T39"].ToString().Trim();
                    txtBFTop.Text = dtb.Rows[0]["T40"].ToString().Trim();
                    txtBFTopRate.Text = dtb.Rows[0]["T41"].ToString().Trim();
                    txtBFMiddle.Text = dtb.Rows[0]["T42"].ToString().Trim();
                    txtBFMiddleRate.Text = dtb.Rows[0]["T43"].ToString().Trim();
                    txtBFBottom.Text = dtb.Rows[0]["T44"].ToString().Trim();
                    txtBFBottomRate.Text = dtb.Rows[0]["T45"].ToString().Trim();
                    txtBFFlutes.Text = dtb.Rows[0]["T46"].ToString().Trim();
                    txtBFFluteRate.Text = dtb.Rows[0]["T47"].ToString().Trim();
                    txtRateTop.Text = dtb.Rows[0]["T48"].ToString().Trim();
                    txtRateMiddle.Text = dtb.Rows[0]["T49"].ToString().Trim();
                    txtRateBottom.Text = dtb.Rows[0]["T50"].ToString().Trim();
                    txtRateFlute.Text = dtb.Rows[0]["T51"].ToString().Trim();
                    txtMaterial.Text = dtb.Rows[0]["T52"].ToString().Trim();
                    txtProcess.Text = dtb.Rows[0]["T53"].ToString().Trim();
                    txtProcessRate.Text = dtb.Rows[0]["T54"].ToString().Trim();
                    txtBoard.Text = dtb.Rows[0]["T55"].ToString().Trim();
                    txtBoardRate.Text = dtb.Rows[0]["T56"].ToString().Trim();
                    txtPrinting.Text = dtb.Rows[0]["T57"].ToString().Trim();
                    txtPrintingRate.Text = dtb.Rows[0]["T58"].ToString().Trim();
                    txtWater.Text = dtb.Rows[0]["T59"].ToString().Trim();
                    txtWaterRate.Text = dtb.Rows[0]["T60"].ToString().Trim();
                    txtDie.Text = dtb.Rows[0]["T61"].ToString().Trim();
                    txtDieRate.Text = dtb.Rows[0]["T62"].ToString().Trim();
                    txtStitching.Text = dtb.Rows[0]["T63"].ToString().Trim();
                    txtStitchingRate.Text = dtb.Rows[0]["T64"].ToString().Trim();
                    txtTaping.Text = dtb.Rows[0]["T65"].ToString().Trim();
                    txtTapingRate.Text = dtb.Rows[0]["T66"].ToString().Trim();
                    txtPacking.Text = dtb.Rows[0]["T67"].ToString().Trim();
                    txtProfit.Text = dtb.Rows[0]["T68"].ToString().Trim();
                    txtProfitRate.Text = dtb.Rows[0]["T69"].ToString().Trim();
                    txtFreight.Text = dtb.Rows[0]["T70"].ToString().Trim();
                    txtFreightRate.Text = dtb.Rows[0]["T71"].ToString().Trim();
                    txtPymt.Text = dtb.Rows[0]["T72"].ToString().Trim();
                    txtPymtRate.Text = dtb.Rows[0]["T73"].ToString().Trim();
                    txtExcise.Text = dtb.Rows[0]["T74"].ToString().Trim();
                    txtExciseRate.Text = dtb.Rows[0]["T75"].ToString().Trim();
                    txtSales.Text = dtb.Rows[0]["T76"].ToString().Trim();
                    txtSalesRate.Text = dtb.Rows[0]["T77"].ToString().Trim();
                    txtGrdTotal.Text = dtb.Rows[0]["T82"].ToString().Trim();
                    txtBfTopG.Text = dtb.Rows[0]["T85"].ToString().Trim();
                    txtBfMiddleG.Text = dtb.Rows[0]["T86"].ToString().Trim();
                    txtBfBottomG.Text = dtb.Rows[0]["T87"].ToString().Trim();
                    txtBfFluteG.Text = dtb.Rows[0]["T88"].ToString().Trim();
                    #endregion

                    create_tab();
                    int count1 = 0;
                    foreach (DataRow dr in dtb.Rows)
                    {
                        dr1 = dt1.NewRow();
                        if (dr["T140"].ToString() != "0")
                        {
                            dr1["srno"] = count1 + 1;

                            dr1["icode"] = dr["T78"].ToString();
                            DataTable dtGridIname = fgen.getdata(frm_qstr, frm_cocd, "Select iname From Item Where Icode='" + dr1["icode"].ToString().Trim() + "'");
                            if (dtGridIname.Rows.Count > 0)
                            {
                                dr1["iname"] = dtGridIname.Rows[0][0].ToString();
                            }

                            dr1["Irate"] = dr["T81"];
                            dr1["T80"] = dr["T80"];
                            dt1.Rows.Add(dr1);
                            count1++;
                        }
                    }
                    add_blankrows();

                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    // fgen.EnableForm(this.Controls);
                    btnItem.Focus();

                    disablectrl();
                    DataTable dtShow1 = fgen.getdata(frm_qstr, frm_cocd, "Select S.* ,I.INAME  FROM SCRATCH S , ITEM I WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "' ORDER BY ACODE");
                    if (dtShow1.Rows.Count > 0)
                    {
                        txtProcess.Text = dtShow1.Rows[0]["Col3"].ToString().Replace("-", "0");
                        txtBoard.Text = dtShow1.Rows[0]["Col4"].ToString().Replace("-", "0");
                        txtPrinting.Text = dtShow1.Rows[0]["Col5"].ToString().Replace("-", "0");
                        txtWater.Text = dtShow1.Rows[0]["Col6"].ToString().Replace("-", "0");
                        txtDie.Text = dtShow1.Rows[0]["Col7"].ToString().Replace("-", "0");
                        txtStitching.Text = dtShow1.Rows[0]["Col8"].ToString().Replace("-", "0");
                        txtTaping.Text = dtShow1.Rows[0]["Col9"].ToString().Replace("-", "0");
                        txtPacking.Text = dtShow1.Rows[0]["Col10"].ToString().Replace("-", "0");
                        txtProfit.Text = dtShow1.Rows[0]["Col11"].ToString().Replace("-", "0");
                        txtFreight.Text = dtShow1.Rows[0]["Col12"].ToString().Replace("-", "0");
                        txtPymt.Text = dtShow1.Rows[0]["Col13"].ToString().Replace("-", "0");
                        txtExcise.Text = dtShow1.Rows[0]["Col14"].ToString().Replace("-", "0");
                        txtSales.Text = dtShow1.Rows[0]["Col15"].ToString().Replace("-", "0");
                        txtMinQty.Text = dtShow1.Rows[0]["Col17"].ToString().Replace("-", "0");
                        for (int i = 0; i < dtShow1.Rows.Count; i++)
                        {
                            col1 = "";
                            col2 = "";
                            col3 = "";
                            string col4 = "";
                            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select icode from item where branchcd='00' and  upper(iname) like '" + txtBFTop.Text.ToString().Trim().ToUpper() + "%'", "icode");
                            col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select icode from item where branchcd='00' and  upper(iname) like '" + txtBFMiddle.Text.ToString().Trim().ToUpper() + "%'", "icode");
                            col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select icode from item where branchcd='00' and  upper(iname) like '" + txtBFBottom.Text.ToString().Trim().ToUpper() + "%'", "icode");
                            col4 = fgen.seek_iname(frm_qstr, frm_cocd, "select icode from item where branchcd='00' and  upper(iname) like '" + txtBFFlutes.Text.ToString().Trim().ToUpper() + "%'", "icode");
                            if (col1 == dtShow1.Rows[i]["icode"].ToString().Trim())
                            {
                                txtBfTopG.Text = dtShow1.Rows[i]["col16"].ToString().Trim();
                            }
                            if (col2 == dtShow1.Rows[i]["icode"].ToString().Trim())
                            {
                                txtBfMiddleG.Text = dtShow1.Rows[i]["col16"].ToString().Trim();
                            }
                            if (col3 == dtShow1.Rows[i]["icode"].ToString().Trim())
                            {
                                txtBfBottomG.Text = dtShow1.Rows[i]["col16"].ToString().Trim();
                            }
                            if (col4 == dtShow1.Rows[i]["icode"].ToString().Trim())
                            {
                                txtBfFluteG.Text = dtShow1.Rows[i]["col16"].ToString().Trim();
                            }
                        }
                    }
                    //myfun();
                    cal();
                    break;
                case "Item":
                    if (col1.Length > 0) { }
                    else return;
                    txtProcess.Focus();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select Iname from Item where trim(Icode)='" + col1 + "'");
                    txtICode.Text = col1;
                    txtItem.Text = dt.Rows[0]["Iname"].ToString();
                    txtLength.Focus();
                    dt.Dispose();
                    //myfun();
                    cal();
                    break;
                case "BFFlutes":
                    if (col1.Length > 0) { }
                    else return;
                    // DataTable dtBFFlutes = fgen.getdata(frm_cocd, "select Iname from Item where trim(Icode)='" + col1 + "'");

                    DataTable dtFlutRate = fgen.getdata(frm_qstr, frm_cocd, "Select Col16,Iname,irate1  FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'AND I.ICODE='" + col1 + "'ORDER BY ACODE");
                    if (dtFlutRate.Rows.Count > 0)
                    {
                        txtBFFlutes.Text = dtFlutRate.Rows[0]["Iname"].ToString();
                        txtBfFluteG.Text = dtFlutRate.Rows[0]["Col16"].ToString();
                        txtBFFluteRate.Text = dtFlutRate.Rows[0]["irate1"].ToString();
                    }
                    txtBS.Focus();
                    //myfun();
                    cal();
                    // GridCal();
                    //  GridCalculation();
                    break;
                case "BFTop":
                    // DataTable dtBFTop = fgen.getdata(frm_cocd, "select Iname from Item where trim(Icode)='" + col1 + "'");

                    DataTable dtTopRate = fgen.getdata(frm_qstr, frm_cocd, "Select Col16,Iname,irate1  FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'AND I.ICODE='" + col1 + "'ORDER BY ACODE");
                    if (dtTopRate.Rows.Count > 0)
                    {
                        txtBFTop.Text = dtTopRate.Rows[0]["Iname"].ToString();
                        txtBFTopRate.Text = dtTopRate.Rows[0]["irate1"].ToString();
                        txtBfTopG.Text = dtTopRate.Rows[0]["Col16"].ToString();
                    }
                    btnBFMiddle.Focus();
                    //myfun();
                    cal();
                    // GridCal();
                    // GridCalculation();
                    break;
                case "BFBottom":
                    //  DataTable dtBFBottom = fgen.getdata(frm_cocd, "select Iname from Item where trim(Icode)='" + col1 + "'");

                    DataTable dtBottomRate = fgen.getdata(frm_qstr, frm_cocd, "Select Col16,Iname,irate1  FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'AND I.ICODE='" + col1 + "'ORDER BY ACODE");
                    if (dtBottomRate.Rows.Count > 0)
                    {
                        txtBFBottom.Text = dtBottomRate.Rows[0]["Iname"].ToString();
                        txtBFBottomRate.Text = dtBottomRate.Rows[0]["irate1"].ToString();
                        txtBfBottomG.Text = dtBottomRate.Rows[0]["Col16"].ToString();
                    }
                    btnBFFlutes.Focus();
                    //myfun();
                    cal();
                    //  GridCal();
                    // GridCalculation();
                    break;
                case "BFMiddle":
                    if (col1.Length > 0) { }
                    else return;

                    DataTable dtMiddleRate = fgen.getdata(frm_qstr, frm_cocd, "Select Col16 ,Iname,irate1 FROM SCRATCH S,ITEM I  WHERE TRIM(S.ICODE)=TRIM(I.ICODE) AND S.TYPE='CM'  AND S.ACODE='" + txtPCode.Text + "'AND I.ICODE='" + col1 + "'ORDER BY ACODE");
                    if (dtMiddleRate.Rows.Count > 0)
                    {
                        txtBFMiddle.Text = dtMiddleRate.Rows[0]["Iname"].ToString();
                        txtBFMiddleRate.Text = dtMiddleRate.Rows[0]["irate1"].ToString();
                        txtBfMiddleG.Text = dtMiddleRate.Rows[0]["Col16"].ToString();
                    }
                    btnBFBottom.Focus();
                    //myfun();
                    cal();
                    // GridCal();
                    // GridCalculation();
                    break;
                case "Print":
                    if (col1.Length > 0) { }
                    else return;
                    set_Val();
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("-", frm_qstr);
                    break;
                case "Type":
                    if (col1.Length > 0) { }
                    else return;
                    if (col1 == "00")
                    {
                        mq0 = "Y";
                        // frm_sql = "SELECT DISTINCT G.INAME AS GRDINAME,'Costing Sheet' as header,SA.VCHNUM AS DOCNO,TO_CHAR(SA.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, SA.ACODE AS CODE,F.ANAME AS PARTY_NAME,SA.ICODE AS ITEM_CODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN SA.T83 ELSE I.INAME END) AS ITEM_NAME,SA.*,'" + mq0 + "' AS SEL FROM  FAMST F ,SOMAS_ANX SA left Join  ITEM G on TRIM(G.ICODE)=TRIM(SA.T78) left outer join ITEM I on trim(SA.ICODE)=TRIM(I.ICODE) WHERE  TRIM(SA.ACODE)=TRIM(F.ACODE) AND SA.BRANCHCD||SA.TYPE||TRIM(SA.VCHNUM)||TO_CHAr(SA.VCHDATE,'DD/MM/YYYY') in  ('" + ViewState["REPORT"].ToString().Trim() + "')  ORDER BY VCHNUM";
                        frm_sql = "SELECT DISTINCT G.INAME AS GRDINAME,'Costing Sheet' as header,SA.VCHNUM AS DOCNO,TO_CHAR(SA.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, SA.ACODE AS CODE,F.ANAME AS PARTY_NAME,SA.ICODE AS ITEM_CODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN SA.T83 ELSE I.INAME END) AS ITEM_NAME,SA.*,'" + mq0 + "' AS SEL FROM  FAMST F ,SOMAS_ANX SA left Join  ITEM G on TRIM(G.ICODE)=TRIM(SA.T78) left outer join ITEM I on trim(SA.ICODE)=TRIM(I.ICODE) WHERE  TRIM(SA.ACODE)=TRIM(F.ACODE) AND SA.BRANCHCD||SA.TYPE||TRIM(SA.VCHNUM)||TO_CHAr(SA.VCHDATE,'DD/MM/YYYY') in  (" + hfReport.Value.Trim() + ")  ORDER BY VCHNUM";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, frm_sql);
                        fgen.send_cookie("ITEMNAME", dt.Rows[0]["ITEM_NAME"].ToString().Trim());
                        fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, frm_sql, "crptBoxCosting", "crptBoxCosting");
                        mq0 = "";
                    }
                    else
                    {
                        mq0 = "N";
                        //frm_sql = "SELECT DISTINCT G.INAME AS GRDINAME,'Costing Sheet' as header,SA.VCHNUM AS DOCNO,TO_CHAR(SA.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, SA.ACODE AS CODE,F.ANAME AS PARTY_NAME,SA.ICODE AS ITEM_CODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN SA.T83 ELSE I.INAME END) AS ITEM_NAME,SA.*,'" + mq0 + "' AS SEL FROM  FAMST F ,SOMAS_ANX SA left Join  ITEM G on TRIM(G.ICODE)=TRIM(SA.T78) left outer join ITEM I on trim(SA.ICODE)=TRIM(I.ICODE) WHERE  TRIM(SA.ACODE)=TRIM(F.ACODE) AND SA.BRANCHCD||SA.TYPE||TRIM(SA.VCHNUM)||TO_CHAr(SA.VCHDATE,'DD/MM/YYYY') in  ('" + ViewState["REPORT"].ToString().Trim() + "')  ORDER BY VCHNUM";
                        frm_sql = "SELECT DISTINCT G.INAME AS GRDINAME,'Costing Sheet' as header,SA.VCHNUM AS DOCNO,TO_CHAR(SA.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, SA.ACODE AS CODE,F.ANAME AS PARTY_NAME,SA.ICODE AS ITEM_CODE,(CASE WHEN TRIM(NVL(I.INAME ,'-'))='-' THEN SA.T83 ELSE I.INAME END) AS ITEM_NAME,SA.*,'" + mq0 + "' AS SEL FROM  FAMST F ,SOMAS_ANX SA left Join  ITEM G on TRIM(G.ICODE)=TRIM(SA.T78) left outer join ITEM I on trim(SA.ICODE)=TRIM(I.ICODE) WHERE  TRIM(SA.ACODE)=TRIM(F.ACODE) AND SA.BRANCHCD||SA.TYPE||TRIM(SA.VCHNUM)||TO_CHAr(SA.VCHDATE,'DD/MM/YYYY') in  (" + hfReport.Value.Trim() + ")  ORDER BY VCHNUM";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, frm_sql);
                        fgen.send_cookie("ITEMNAME", dt.Rows[0]["ITEM_NAME"].ToString().Trim());
                        fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, frm_sql, "crptBoxCosting", "crptBoxCostingWOB");
                        mq0 = "";
                    }
                    break;
                case "Print_E":
                    if (col1.Length > 0) { }
                    else return;
                    //frm_sql = "SELECT DISTINCT G.INAME AS GRDINAME,'Costing Sheet' as header,SA.VCHNUM AS DOCNO,TO_CHAR(SA.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, SA.ACODE AS CODE,F.ANAME AS PARTY_NAME,SA.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,SA.*  FROM  ITEM I ,FAMST F ,SOMAS_ANX SA left Join  ITEM G on TRIM(G.ICODE)=TRIM(SA.T78) WHERE   TRIM(SA.ICODE)=TRIM(I.ICODE)  AND TRIM(SA.ACODE)=TRIM(F.ACODE) AND SA.BRANCHCD||SA.TYPE||TRIM(SA.VCHNUM)||TO_CHAr(SA.VCHDATE,'DD/MM/YYYY') in  ('" + col1 + "')  ORDER BY VCHNUM";
                    ViewState["REPORT"] = col1;
                    hfReport.Value = col1.Trim();
                    hffield.Value = "Type";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;
                case "List":
                    if (col1.Length > 0) { }
                    else return;
                    hffield.Value = "List_E";
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1.Trim());
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "Add":
                    if (col1.Length > 0) { }
                    else return;
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        dt1 = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        dt1 = dt.Clone();
                        dr1 = null;
                        for (int i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                            dr1["icode"] = dt.Rows[i]["icode"].ToString();
                            dr1["iname"] = dt.Rows[i]["iname"].ToString();
                            //dr1["Aname"] = dt.Rows[i]["Aname"].ToString();
                            //dr1["Acode"] = dt.Rows[i]["Acode"].ToString();
                            dr1["Irate"] = ((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text.Trim();
                            //  dr1["Quantity"] = ((TextBox)sg1.Rows[i].FindControl("txtQty")).Text.Trim();
                            dr1["T80"] = ((TextBox)sg1.Rows[i].FindControl("txtQty")).Text.Trim();
                            //TextBox Qty = (TextBox)(sg1.Rows[i].FindControl("txtCol16"));
                            //Qty.Focus();

                            dt1.Rows.Add(dr1);
                        }
                        if (col1.Trim().Length == 8) frm_sql = "select distinct icode,iname from item where trim(icode) in ('" + col1 + "')";
                        else frm_sql = "select distinct icode,iname,Unit,Irate from item where trim(icode) in (" + col1 + ")";

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, frm_sql);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                            // dr1["Aname"] = dt.Rows[i]["Aname"].ToString();
                            //dr1["ACode"] = dt.Rows[i]["ACode"].ToString();
                            dr1["Irate"] = dt.Rows[i]["Irate"].ToString();
                            // dr1["Quantity"] = "0";
                            dr1["T80"] = "0";
                            dt1.Rows.Add(dr1);
                        }
                    }
                    add_blankrows();

                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    //myfun();
                    cal();
                    // GridCal();
                    // GridCalculation();
                    dt.Dispose(); dt1.Dispose();
                    break;
                case "Add_E":
                    if (col1.Length > 0) { }
                    else return;
                    dt = new DataTable();
                    string a = "Select Irate from item where trim(icode) in (" + col1 + ")";
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select Irate from item where trim(icode) in ('" + col1 + "')");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtCol16")).Text = dt.Rows[0]["Irate"].ToString().Trim();

                    break;
                case "Row_Edit":
                    if (col1.Length > 0) { }
                    else return;
                    // sg1.Rows[Convert.ToInt32(hf1.Text)].Cells[3].Text = col1;
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr, frm_cocd, "select Type1,Name,Type1 as Code from Type where id='K' and trim(type1)='" + col1 + "'");
                    if (dtb.Rows.Count > 0)
                    {
                        //sg1.Rows[Convert.ToInt32(hf1.Text)].Cells[4].Text = dtb.Rows[0]["name"].ToString().Trim();
                        //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Text)].FindControl("txtfld1")).Focus();
                    } //Grid_Col_Tot();
                    break;
                case "Rmv":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {

                        #region
                        if (ViewState["sg1"] != null)
                        {
                            dt = new DataTable();
                            dt1 = new DataTable();
                            dt = (DataTable)ViewState["sg1"];
                            dt1 = dt.Clone();
                            dr1 = null;
                            for (int i = 0; i < dt.Rows.Count - 1; i++)
                            {
                                dr1 = dt1.NewRow();
                                dr1["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                                dr1["icode"] = dt.Rows[i]["icode"].ToString();
                                dr1["iname"] = dt.Rows[i]["iname"].ToString();
                                dr1["Irate"] = ((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text.Trim();
                                dr1["T80"] = ((TextBox)sg1.Rows[i].FindControl("txtQty")).Text.Trim(); ;
                                dt1.Rows.Add(dr1);
                            }
                        }
                        add_blankrows();
                        ViewState["sg1"] = dt1;

                        #endregion
                        dtb = new DataTable();
                        dtb = (DataTable)ViewState["sg1"];
                        dtb.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = dtb;
                        sg1.DataSource = dtb;
                        sg1.DataBind();
                        dtb.Dispose();
                        //myfun();
                        cal();
                        // GridCal();
                        // GridCalculation();
                    }
                    break;

                case "State":
                    if (col1.Length > 0) { }
                    else return;
                    //frm_sql = "Select type1 as fstr,name,type1 as Code  From Typegrp Where ID='ES' Order By Name";
                    popselected.Value = col1;
                    Squery = "select type1,name from Typegrp Where ID='ES' And type1='" + col1.Trim() + "'";
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr, frm_cocd, Squery);
                    //txtState.Text = dtb.Rows[0]["name"].ToString().Trim();
                    break;

            }

        }
    }

    protected void btnParty_Click(object sender, EventArgs e)
    {
        hffield.Value = "Party";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);

        //myfun();
        cal();

    }
    protected void btnItem_Click(object sender, EventArgs e)
    {
        hffield.Value = "Item";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);


        //myfun();
        cal();

    }
    protected void btnBFTop_Click(object sender, EventArgs e)
    {
        hffield.Value = "BFTop";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
        //myfun();
        cal();

    }
    protected void btnBFMiddle_Click(object sender, EventArgs e)
    {
        hffield.Value = "BFMiddle";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);


        //myfun();
        cal();

    }
    protected void btnBFBottom_Click(object sender, EventArgs e)
    {
        hffield.Value = "BFBottom";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);


        //myfun();
        cal();

    }
    protected void btnBFFlutes_Click(object sender, EventArgs e)
    {
        hffield.Value = "BFFlutes";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
        //myfun();
        cal();
    }
    void cal()
    {
        fgen.fill_zero(this.Controls);
        #region Universal
        if (cmbBoxTypes.Text == "UNIVERSAL")
        {

            //vip = vip + "document.getElementById('" + mq0 + "txtReel').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1) + 40) ;";
            //vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +90) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if (txtDeckle.Text != "")
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtHeight.Text.Trim()) + Convert.ToDouble(txtWidth.Text.Trim()) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }

            // vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +90) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if (txtCutSize.Text != "")
            {
                txtCutSize.Text = Math.Round((Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtWidth.Text.Trim())) * 2 + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region Over Flap Rac
        if (cmbBoxTypes.Text == "OVER FLAP RAC")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)*2) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if ((txtDeckle.Text != "") && (txtDeckle.Text != "0.000"))
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtHeight.Text.Trim()) + (Convert.ToDouble(txtWidth.Text.Trim()) * 2) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if (txtCutSize.Text != "")
            {
                txtCutSize.Text = Math.Round((Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtWidth.Text.Trim())) * 2 + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region Half Rac
        if (cmbBoxTypes.Text == "HALF RAC")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)/2) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if ((txtDeckle.Text != "") && (txtDeckle.Text != "0.000"))
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtHeight.Text.Trim()) + (Convert.ToDouble(txtWidth.Text.Trim()) / 2) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }
            // vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +90) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if (txtCutSize.Text != "")
            {
                txtCutSize.Text = Math.Round((Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtWidth.Text.Trim())) * 2 + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region OVER FLAP HALF RAC
        if (cmbBoxTypes.Text == "OVER FLAP HALF RAC")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if ((txtDeckle.Text != "") && (txtDeckle.Text != "0.000"))
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtHeight.Text.Trim()) + (Convert.ToDouble(txtWidth.Text.Trim())) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if (txtCutSize.Text != "")
            {
                txtCutSize.Text = Math.Round((Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtWidth.Text.Trim())) * 2 + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region Sleeve
        if (cmbBoxTypes.Text == "SLEEVE")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if ((txtDeckle.Text != "") && (txtDeckle.Text != "0.000"))
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtHeight.Text.Trim()) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if (txtCutSize.Text != "")
            {
                txtCutSize.Text = Math.Round((Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtWidth.Text.Trim())) * 2 + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region Tray
        if (cmbBoxTypes.Text == "TRAY")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1)*2) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if ((txtDeckle.Text != "") && (txtDeckle.Text != "0.000"))
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtWidth.Text.Trim()) + (Convert.ToDouble(txtHeight.Text.Trim()) * 2) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtHeight').value *1)*2) +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if ((txtCutSize.Text != "") && (txtCutSize.Text != "0.000"))
            {
                txtCutSize.Text = Math.Round((Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtHeight.Text.Trim()) * 2) + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region Sheet
        if (cmbBoxTypes.Text == "SHEET")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)) +  fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            if ((txtDeckle.Text != "") && (txtDeckle.Text != "0.000"))
            {
                txtDeckle.Text = Math.Round(Convert.ToDouble(txtWidth.Text.Trim()) + Convert.ToDouble(txtReel.Text.Trim()), 3).ToString();
            }
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1)) +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
            if ((txtCutSize.Text != "") && (txtCutSize.Text != "0.000"))
            {
                txtCutSize.Text = Math.Round(Convert.ToDouble(txtLength.Text.Trim()) + Convert.ToDouble(txtCut.Text.Trim()), 3).ToString();
            }
        }
        #endregion

        #region All
        vip = vip + "document.getElementById('" + mq0 + "txtSheet').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtDeckle').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtCutSize').value *1))/ 1000000).toFixed(3) ;";
        if ((txtSheet.Text != "") && (txtSheet.Text != "0.000"))
        {
            txtSheet.Text = Math.Round(Convert.ToDouble(txtDeckle.Text.Trim()) * Convert.ToDouble(txtCutSize.Text.Trim()) / 1000000, 3).ToString();
        }
        vip = vip + "document.getElementById('" + mq0 + "txtWTop').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtTop').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1))/ 1000).toFixed(3) ;";
        if ((txtWTop.Text != "") && (txtWTop.Text != "0.000"))
        {
            txtWTop.Text = Math.Round(Convert.ToDouble(txtTop.Text.Trim()) * Convert.ToDouble(txtSheet.Text.Trim()) / 1000, 3).ToString();
        }
        vip = vip + "document.getElementById('" + mq0 + "txtWMiddle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMiddle').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1))/ 1000).toFixed(3) ;";
        if ((txtWMiddle.Text != "") && (txtWMiddle.Text != "0.000"))
        {
            txtWMiddle.Text = Math.Round(Convert.ToDouble(txtMiddle.Text.Trim()) * Convert.ToDouble(txtSheet.Text.Trim()) / 1000, 3).ToString();
        }
        vip = vip + "document.getElementById('" + mq0 + "txtWBottom').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtBottom').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1))/ 1000).toFixed(3) ;";
        if ((txtWBottom.Text != "") && (txtWBottom.Text != "0.000"))
        {
            txtWBottom.Text = Math.Round(Convert.ToDouble(txtBottom.Text.Trim()) * Convert.ToDouble(txtSheet.Text.Trim()) / 1000, 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtWFlute').value = fill_zero(((fill_zero(document.getElementById('" + mq0 + "txtFluteB').value * 1)  + fill_zero(document.getElementById('" + mq0 + "txtFluteA').value *1))*1.5)*fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1)/ 1000).toFixed(3) ;";
        if ((txtWFlute.Text != "") && (txtWFlute.Text != "0.000"))
        {
            txtWFlute.Text = Math.Round((Convert.ToDouble(txtFluteB.Text.Trim()) + Convert.ToDouble(txtFluteA.Text.Trim())) * 1.5 * Convert.ToDouble(txtSheet.Text.Trim()) / 1000, 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtSWeight').value =  fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWTop').value * 1)  + fill_zero(document.getElementById('" + mq0 + "txtWMiddle').value *1) +fill_zero(document.getElementById('" + mq0 + "txtWBottom').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWFlute').value *1)).toFixed(3) ;";
        if ((txtSWeight.Text != "") && (txtSWeight.Text != "0.000") && (txtWMiddle.Text != "") && (txtWTop.Text != "") && (txtWBottom.Text != "") && (txtWFlute.Text != ""))
        {
            txtSWeight.Text = Math.Round(Convert.ToDouble(txtWTop.Text.Trim()) + Convert.ToDouble(txtWMiddle.Text.Trim()) + Convert.ToDouble(txtWBottom.Text.Trim()) + Convert.ToDouble(txtWFlute.Text.Trim()), 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtMin').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtMinQty').value*1) / fill_zero(document.getElementById('" + mq0 + "txtSWeight').value*1)) ;";
        if ((txtMin.Text != "") && (txtMin.Text != "0.000"))
        {
            txtMin.Text = Math.Round(Convert.ToDouble(txtMinQty.Text.Trim()) / Convert.ToDouble(txtSWeight.Text.Trim()), 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtBS').value =fill_zero(((fill_zero(document.getElementById('" + mq0 + "txtBFTopRate').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtTop').value *1))/1000) +fill_zero(((document.getElementById('" + mq0 + "txtBFMiddleRate').value *1)* fill_zero(document.getElementById('" + mq0 + "txtMiddle').value *1))/1000) +fill_zero(((document.getElementById('" + mq0 + "txtBFBottomRate').value *1)* fill_zero(document.getElementById('" + mq0 + "txtBottom').value *1))/1000)).toFixed(3) ;";
        if ((txtBS.Text != "") && (txtBS.Text != "0.000"))
        {
            txtBS.Text = Math.Round((Convert.ToDouble(txtBFTopRate.Text.Trim()) * Convert.ToDouble(txtTop.Text.Trim()) / 1000) + (Convert.ToDouble(txtBFMiddleRate.Text.Trim()) * Convert.ToDouble(txtMiddle.Text.Trim()) / 1000) + (Convert.ToDouble(txtBFBottomRate.Text.Trim()) * Convert.ToDouble(txtBottom.Text.Trim()) / 1000), 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtMin').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtMinQty').value*1) / fill_zero(document.getElementById('" + mq0 + "txtSWeight').value*1)).toFixed(3) ;";
        if ((txtMin.Text != "") && (txtMin.Text != "0.000"))
        {
            // vipin
            txtMin.Text = Math.Round(Convert.ToDouble(txtMinQty.Text.Trim()) / Convert.ToDouble(txtSWeight.Text.Trim()), 3).ToString();
            if ((txtMin.Text != "NaN") && (txtMin.Text != "Infinity"))
            {
                string[] splitToInt = txtMin.Text.Split('.');
                Int64 Length = Convert.ToInt64(splitToInt[0].Length);
                Int64 ConvertToWhole = 0;
                string Substrng = "";
                int ChangeStr = 0;
                string Final = "";
                if (Length > 1)
                {
                    ConvertToWhole = Length - 2;
                    Substrng = txtMin.Text.Substring(0, (int)ConvertToWhole);
                    if (Substrng == "")
                    {
                        Substrng = "0";
                    }
                    ChangeStr = int.Parse(Substrng) + 1;
                    Final = ChangeStr + "00";
                }
                else if (Length == 1)
                {
                    ChangeStr = 1;
                    Final = ChangeStr + "00";

                }
                if ((Convert.ToDouble(Final) - Convert.ToDouble(txtMin.Text.Trim()) == 100) && Convert.ToDouble(txtMin.Text.Trim()) >= 100) { }
                else txtMin.Text = Final;
                //txtMin.Text = Final;
            }
        }
        // vip = vip + "document.getElementById('" + mq0 + "txtPurchase').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtMinQty').value*1) / fill_zero(document.getElementById('" + mq0 + "txtSWeight').value*1)).toFixed(3) ;";
        //if ((txtPurchase.Text != "") && (txtPurchase.Text != "0.000"))
        //{
        //    txtPurchase.Text = Math.Round(Convert.ToDouble(txtMinQty.Text.Trim()) / Convert.ToDouble(txtSWeight.Text.Trim()), 3).ToString();
        //}
        //txtPurchase.Text = txtMin.Text;
        if (txtMin.Text == txtPurchase.Text)
        {
            txtPurchase.Text = txtMin.Text;
        }
        else if (txtPurchase.Text == "0")
        {
            txtPurchase.Text = txtMin.Text;
        }

        vip = vip + "document.getElementById('" + mq0 + "txtGSM').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtTop').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtMiddle').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtBottom').value * 1)+((fill_zero(document.getElementById('" + mq0 + "txtFluteB').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtFluteA').value * 1))*1.45)).toFixed(3);";
        if ((txtGSM.Text != "") && (txtGSM.Text != "0.000"))
        {
            txtGSM.Text = Math.Round(Convert.ToDouble(txtTop.Text.Trim()) + Convert.ToDouble(txtMiddle.Text.Trim()) + Convert.ToDouble(txtBottom.Text.Trim()) + ((Convert.ToDouble(txtFluteB.Text.Trim()) + Convert.ToDouble(txtFluteA.Text.Trim())) * 1.45), 3).ToString();
        }

        //vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //if ((txtProcessRate.Text != "") && (txtProcessRate.Text != "0.000"))
        //{
        //    txtProcessRate.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) * (Convert.ToDouble(txtProcess.Text.Trim()) / 100) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
        //}

        //vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //if ((txtBoardRate.Text != "")&&(txtBoardRate.Text !="0.000"))
        //{
        //    txtBoardRate.Text = Math.Round(Convert.ToDouble(txtSWeight.Text.Trim()) * Convert.ToDouble(txtBoard.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
        //}

        //vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //if ((txtPrintingRate.Text != "")&&(txtPrintingRate.Text !="0.000"))
        //{
        //    txtPrintingRate.Text = Math.Round(Convert.ToDouble(txtSheet.Text.Trim()) * Convert.ToDouble(txtPrinting.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
        //}
        //  originally written
        //if ((txtMinQty.Text != "") && (txtPurchase.Text != ""))
        //{
        //    if ((txtPurchase.Text == "NaN") || (txtPurchase.Text == "Infinity") || (txtMinQty.Text == "NaN") || (txtMinQty.Text == "Infinity"))
        //    {
        //        txtPurchase.Text = "0";
        //        txtMin.Text = "0";
        //    }
        //    if (Convert.ToDecimal(txtPurchase.Text) <= Convert.ToDecimal(txtMinQty.Text))
        //    {
        //        vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //        if ((txtProcessRate.Text != "") && (txtProcessRate.Text != "0.000"))
        //        {
        //            txtProcessRate.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) * (Convert.ToDouble(txtProcess.Text.Trim()) / 100) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
        //        }
        //        vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //        if ((txtBoardRate.Text != "") && (txtBoardRate.Text != "0.000"))
        //        {
        //            txtBoardRate.Text = Math.Round(Convert.ToDouble(txtSWeight.Text.Trim()) * Convert.ToDouble(txtBoard.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
        //        }
        //        vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //        if ((txtPrintingRate.Text != "") && (txtPrintingRate.Text != "0.000"))
        //        {
        //            txtPrintingRate.Text = Math.Round(Convert.ToDouble(txtSheet.Text.Trim()) * Convert.ToDouble(txtPrinting.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
        //        }

        //    }
        //    else
        //    {
        //        vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        //        if ((txtProcessRate.Text != "") && (txtProcessRate.Text != "0.000"))
        //        {
        //            txtProcessRate.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) * (Convert.ToDouble(txtProcess.Text.Trim()) / 100) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtMin.Text.Trim()), 3).ToString();
        //        }
        //        vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        //        if ((txtBoardRate.Text != "") && (txtBoardRate.Text != "0.000"))
        //        {
        //            txtBoardRate.Text = Math.Round(Convert.ToDouble(txtSWeight.Text.Trim()) * Convert.ToDouble(txtBoard.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtMin.Text.Trim()), 3).ToString();
        //        }
        //        vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";

        //        if ((txtPrintingRate.Text != "") && (txtPrintingRate.Text != "0.000"))
        //        {
        //            txtPrintingRate.Text = Math.Round(Convert.ToDouble(txtSheet.Text.Trim()) * Convert.ToDouble(txtPrinting.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtMin.Text.Trim()), 3).ToString();
        //        }
        //    }

        //}

        vip = vip + "document.getElementById('" + mq0 + "txtDieRate').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtDie').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1)).toFixed(3) ;";
        if ((txtDieRate.Text != "") && (txtDieRate.Text != "0.000"))
        {
            txtDieRate.Text = Math.Round(Convert.ToDouble(txtDie.Text.Trim()) * Convert.ToDouble(txtSheet.Text.Trim()), 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtWaterRate').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtWater').value *1)).toFixed(3) ;";
        if ((txtWaterRate.Text != "") && (txtWaterRate.Text != "0.000"))
        {
            txtWaterRate.Text = Math.Round(Convert.ToDouble(txtSheet.Text.Trim()) * Convert.ToDouble(txtWater.Text.Trim()), 3).ToString();
        }

        vip = vip + "document.getElementById('" + mq0 + "txtStitchingRate').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtStitching').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtSWeight').value *1)).toFixed(3) ;";
        if ((txtStitchingRate.Text != "") && (txtStitchingRate.Text != "0.000"))
        {
            txtStitchingRate.Text = Math.Round(Convert.ToDouble(txtStitching.Text.Trim()) * Convert.ToDouble(txtSWeight.Text.Trim()), 3).ToString();
        }
        vip = vip + "document.getElementById('" + mq0 + "txtTapingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1)/1000)*4*fill_zero(document.getElementById('" + mq0 + "txtTaping').value * 1)).toFixed(3);";
        txtTapingRate.Text = Math.Round((Convert.ToDouble(txtHeight.Text.Trim())) / 1000 * 4 * Convert.ToDouble(txtTaping.Text.Trim()), 3).ToString();

        vip = vip + "document.getElementById('" + mq0 + "txtRateTop').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWTop').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfTopG').value * 1)).toFixed(3);";
        txtRateTop.Text = Math.Round(Convert.ToDouble(txtWTop.Text.Trim()) * Convert.ToDouble(txtBfTopG.Text.Trim()), 3).ToString();
        vip = vip + "document.getElementById('" + mq0 + "txtRateMiddle').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWMiddle').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfMiddleG').value * 1)).toFixed(3);";
        txtRateMiddle.Text = Math.Round(Convert.ToDouble(txtWMiddle.Text.Trim()) * Convert.ToDouble(txtBfMiddleG.Text.Trim()), 3).ToString();
        vip = vip + "document.getElementById('" + mq0 + "txtRateBottom').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWBottom').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfBottomG').value * 1)).toFixed(3);";
        txtRateBottom.Text = Math.Round(Convert.ToDouble(txtWBottom.Text.Trim()) * Convert.ToDouble(txtBfBottomG.Text.Trim()), 3).ToString();
        vip = vip + "document.getElementById('" + mq0 + "txtRateFlute').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWFlute').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfFluteG').value * 1)).toFixed(3);";
        txtRateFlute.Text = Math.Round(Convert.ToDouble(txtWFlute.Text.Trim()) * Convert.ToDouble(txtBfFluteG.Text.Trim()), 3).ToString();

        vip = vip + "document.getElementById('" + mq0 + "txtMaterial').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtRateTop').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtRateMiddle').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtRateBottom').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtRateFlute').value * 1)).toFixed(3);";

        // txtMaterial.Text = Math.Round((Convert.ToDouble(txtWTop.Text.Trim()) * Convert.ToDouble(txtRateTop.Text.Trim())) + (Convert.ToDouble(txtWMiddle.Text.Trim()) * Convert.ToDouble(txtRateMiddle.Text.Trim())) + (Convert.ToDouble(txtWBottom.Text.Trim()) * Convert.ToDouble(txtRateBottom.Text.Trim())) + (Convert.ToDouble(txtWFlute.Text.Trim()) * Convert.ToDouble(txtRateFlute.Text.Trim())), 3).ToString();
        txtMaterial.Text = Math.Round(Convert.ToDouble(txtRateTop.Text.Trim()) + Convert.ToDouble(txtRateMiddle.Text.Trim()) + Convert.ToDouble(txtRateBottom.Text.Trim()) + Convert.ToDouble(txtRateFlute.Text.Trim()), 3).ToString();
        //replaced
        if ((txtMinQty.Text != "") && (txtPurchase.Text != ""))
        {
            if ((txtPurchase.Text == "NaN") || (txtPurchase.Text == "Infinity") || (txtMinQty.Text == "NaN") || (txtMinQty.Text == "Infinity"))
            {
                txtPurchase.Text = "0";
                txtMin.Text = "0";
            }
            if (Convert.ToDecimal(txtPurchase.Text) <= Convert.ToDecimal(txtMinQty.Text))
            {
                vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
                if ((txtProcessRate.Text != "") && (txtProcessRate.Text != "0.000"))
                {
                    txtProcessRate.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) * (Convert.ToDouble(txtProcess.Text.Trim()) / 100) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
                }
                vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
                if ((txtBoardRate.Text != "") && (txtBoardRate.Text != "0.000"))
                {
                    txtBoardRate.Text = Math.Round(Convert.ToDouble(txtSWeight.Text.Trim()) * Convert.ToDouble(txtBoard.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
                }
                vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
                if ((txtPrintingRate.Text != "") && (txtPrintingRate.Text != "0.000"))
                {
                    txtPrintingRate.Text = Math.Round(Convert.ToDouble(txtSheet.Text.Trim()) * Convert.ToDouble(txtPrinting.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
                }

            }
            else
            {
                vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
                if ((txtProcessRate.Text != "") && (txtProcessRate.Text != "0.000"))
                {
                    txtProcessRate.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) * (Convert.ToDouble(txtProcess.Text.Trim()) / 100) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtMin.Text.Trim()), 3).ToString();
                }
                vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
                if ((txtBoardRate.Text != "") && (txtBoardRate.Text != "0.000"))
                {
                    txtBoardRate.Text = Math.Round(Convert.ToDouble(txtSWeight.Text.Trim()) * Convert.ToDouble(txtBoard.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtMin.Text.Trim()), 3).ToString();
                }
                vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";

                if ((txtPrintingRate.Text != "") && (txtPrintingRate.Text != "0.000"))
                {
                    txtPrintingRate.Text = Math.Round(Convert.ToDouble(txtSheet.Text.Trim()) * Convert.ToDouble(txtPrinting.Text.Trim()) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtMin.Text.Trim()), 3).ToString();
                }
            }

        }

        //if (cmbBoxTypes.Text == "UNIVERSAL")
        //{
        //    vip = vip + "document.getElementById('" + mq0 + "txtProfitRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtProfit').value *1)/100).toFixed(3);";

        //    txtProfitRate.Text = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim())) * (Convert.ToDouble(txtProfit.Text.Trim()) / 100), 3).ToString();
        //}
        //else
        //{
        vip = vip + "document.getElementById('" + mq0 + "txtProfitRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*fill_zero(document.getElementById('" + mq0 + "txtProfit').value *1)/100).toFixed(3);";

        txtProfitRate.Text = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtPacking.Text.Trim()) + Convert.ToDouble(txtAny.Text.Trim())) * (Convert.ToDouble(txtProfit.Text.Trim()) / 100), 3).ToString();
        //}
        //vip = vip + "document.getElementById('" + mq0 + "txtFreightRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtFreight').value *1)/100).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtFreightRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*fill_zero(document.getElementById('" + mq0 + "txtFreight').value *1)/100).toFixed(3) ;";
        txtFreightRate.Text = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProfitRate.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtPacking.Text.Trim()) + Convert.ToDouble(txtAny.Text.Trim())) * (Convert.ToDouble(txtFreight.Text.Trim()) / 100), 3).ToString();

        // vip = vip + "document.getElementById('" + mq0 + "txtPymtRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtFreightRate').value *1))*2/100 *fill_zero(document.getElementById('" + mq0 + "txtPymt').value *1)/100).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtPymtRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)  + fill_zero(document.getElementById('" + mq0 + "txtFreightRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*2/100 *fill_zero(document.getElementById('" + mq0 + "txtPymt').value *1)).toFixed(3) ;";
        // vip = vip + "document.getElementById('" + mq0 + "txtPymtRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)  + fill_zero(document.getElementById('" + mq0 + "txtFreightRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*2/100 *fill_zero(document.getElementById('" + mq0 + "txtPymt').value *1)/100).toFixed(3) ;";
        string Pymt = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtProfitRate.Text.Trim()) + Convert.ToDouble(txtFreightRate.Text.Trim())), 3).ToString();
        string ss = Math.Round((+Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtPacking.Text.Trim()) + Convert.ToDouble(txtAny.Text.Trim())), 3).ToString();
        double Rate = Convert.ToDouble(Pymt) * 2 / 100;
        double FinalPymt = Rate * (Convert.ToDouble(txtPymt.Text.Trim()));
        txtPymtRate.Text = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProfitRate.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtFreightRate.Text.Trim()) + Convert.ToDouble(txtPacking.Text.Trim()) + Convert.ToDouble(txtAny.Text.Trim())) * 2 / 100 * (Convert.ToDouble(txtPymt.Text.Trim())), 3).ToString();
        // txtPymtRate.Text = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProfitRate.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtFreightRate.Text.Trim()) + Convert.ToDouble(txtPacking.Text.Trim()) + Convert.ToDouble(txtAny.Text.Trim())) * 2 / 100 * (Convert.ToDouble(txtPymt.Text.Trim()) / 100), 3).ToString();
        // txtPymtRate.Text = Math.Round((Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtProfitRate.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtFreightRate.Text.Trim()) + Convert.ToDouble(txtPacking.Text.Trim()) + Convert.ToDouble(txtAny.Text.Trim())) * 2 / 100 * (Convert.ToDouble(txtPymt.Text.Trim())), 3).ToString();
        // txtPymtRate.Text = Math.Round(FinalPymt, 3).ToString();
        // vip = vip + "document.getElementById('" + mq0 + "txtBasic').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtPymtRate').value *1)+ (fill_zero(document.getElementById('" + mq0 + "txtPacking').value *1))+ fill_zero(document.getElementById('" + mq0 + "txtAny').value *1))).toFixed(3);";
        //vip = vip + "document.getElementById('" + mq0 + "txtBasic').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtPymtRate').value *1)+ (fill_zero(document.getElementById('" + mq0 + "txtPacking').value *1)/100)+ fill_zero(document.getElementById('" + mq0 + "txtAny').value *1))).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtBasic').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtPymtRate').value *1)+ (fill_zero(document.getElementById('" + mq0 + "txtPacking').value *1))+ fill_zero(document.getElementById('" + mq0 + "txtAny').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)  + fill_zero(document.getElementById('" + mq0 + "txtFreightRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)).toFixed(3);";
        txtBasic.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) + Convert.ToDouble(txtPymtRate.Text.Trim()) + (Convert.ToDouble(txtPacking.Text.Trim())) + Convert.ToDouble(txtAny.Text.Trim()) + Convert.ToDouble(txtProfitRate.Text.Trim()) + Convert.ToDouble(txtProcessRate.Text.Trim()) + Convert.ToDouble(txtBoardRate.Text.Trim()) + Convert.ToDouble(txtPrintingRate.Text.Trim()) + Convert.ToDouble(txtDieRate.Text.Trim()) + Convert.ToDouble(txtWaterRate.Text.Trim()) + Convert.ToDouble(txtStitchingRate.Text.Trim()) + Convert.ToDouble(txtTapingRate.Text.Trim()) + Convert.ToDouble(txtFreightRate.Text.Trim()), 3).ToString();


        vip = vip + "document.getElementById('" + mq0 + "txtExciseRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtBasic').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtExcise').value *1))/100).toFixed(3);";
        txtExciseRate.Text = Math.Round((Convert.ToDouble(txtBasic.Text.Trim()) * Convert.ToDouble(txtExcise.Text.Trim())) / 100, 3).ToString();
        vip = vip + "document.getElementById('" + mq0 + "txtSalesRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtBasic').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtExciseRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtSales').value *1)/100).toFixed(3);";
        txtSalesRate.Text = Math.Round((Convert.ToDouble(txtBasic.Text.Trim()) + Convert.ToDouble(txtExciseRate.Text.Trim())) * (Convert.ToDouble(txtSales.Text.Trim()) / 100), 3).ToString();
        vip = vip + "document.getElementById('" + mq0 + "txtTotal').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtBasic').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtExciseRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtSalesRate').value *1)).toFixed(3);";
        txtTotal.Text = Math.Round(Convert.ToDouble(txtSalesRate.Text.Trim()) + Convert.ToDouble(txtBasic.Text.Trim()) + Convert.ToDouble(txtExciseRate.Text.Trim()), 3).ToString();
        #endregion
    }

    public void myfun()
    {
        //vip = "";
        //mq0 = "ctl00_ContentPlaceHolder1_";
        // vip = vip + "document.getElementById('" + mq0 + "txtReel').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1) + 40) ;";
        // vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +90) ;";
        mq0 = "ContentPlaceHolder1_";
        vip = "";
        vip = vip + "<script type='text/javascript'>function calculateSum() {";
        vip = vip + " var purchase=0;var minqty=0; var madhvi = 0; var Splitt=[100];var WholeString=''; var Le=0 ;var ConvertToWhole=0 ; var Substrng=0;var ChangeStr=0; var Final=0;var strConvert=0;";
        #region Universal
        if (cmbBoxTypes.Text == "UNIVERSAL")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
        }
        #endregion

        #region Over Flap Rac
        if (cmbBoxTypes.Text == "OVER FLAP RAC")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)*2) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
        }
        #endregion

        #region Half Rac
        if (cmbBoxTypes.Text == "HALF RAC")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)/2) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
        }
        #endregion

        #region OVER FLAP HALF RAC
        if (cmbBoxTypes.Text == "OVER FLAP HALF RAC")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
        }
        #endregion

        #region Sleeve
        if (cmbBoxTypes.Text == "SLEEVE")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero(fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWidth').value *1)) * 2 +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
        }
        #endregion

        #region Tray
        if (cmbBoxTypes.Text == "TRAY")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1)*2) + fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ;";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtHeight').value *1)*2) +fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ;";
        }
        #endregion

        #region Sheet
        if (cmbBoxTypes.Text == "SHEET")
        {
            vip = vip + "document.getElementById('" + mq0 + "txtDeckle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtWidth').value * 1)) +  fill_zero(document.getElementById('" + mq0 + "txtReel').value * 1)).toFixed(3) ; ";
            vip = vip + "if(document.getElementById('" + mq0 + "cmbBoxTypes').options[t.selectedIndex].text=='SHEET'){ ";
            vip = vip + "document.getElementById('" + mq0 + "txtCutSize').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtLength').value * 1)) + fill_zero(document.getElementById('" + mq0 + "txtCut').value * 1)).toFixed(3) ; };";
        }
        #endregion

        vip = vip + "document.getElementById('" + mq0 + "txtSheet').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtDeckle').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtCutSize').value *1))/ 1000000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtWTop').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtTop').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1))/ 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtWMiddle').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMiddle').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1))/ 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtWBottom').value = fill_zero((fill_zero(document.getElementById('" + mq0 + "txtBottom').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1))/ 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtWFlute').value = fill_zero(((fill_zero(document.getElementById('" + mq0 + "txtFluteB').value * 1)  + fill_zero(document.getElementById('" + mq0 + "txtFluteA').value *1))*1.5)*fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1)/ 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtSWeight').value =  fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWTop').value * 1)  + fill_zero(document.getElementById('" + mq0 + "txtWMiddle').value *1) +fill_zero(document.getElementById('" + mq0 + "txtWBottom').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWFlute').value *1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtBS').value =fill_zero(((fill_zero(document.getElementById('" + mq0 + "txtBFTopRate').value * 1)  * fill_zero(document.getElementById('" + mq0 + "txtTop').value *1))/1000) +fill_zero(((document.getElementById('" + mq0 + "txtBFMiddleRate').value *1)* fill_zero(document.getElementById('" + mq0 + "txtMiddle').value *1))/1000) +fill_zero(((document.getElementById('" + mq0 + "txtBFBottomRate').value *1)* fill_zero(document.getElementById('" + mq0 + "txtBottom').value *1))/1000)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtMin').value = fill_zero(fill_zero(document.getElementById('" + mq0 + "txtMinQty').value*1) / fill_zero(document.getElementById('" + mq0 + "txtSWeight').value*1)) ;";
        // vip = vip + "document.getElementById('" + mq0 + "txtPurchase').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "txtMinQty').value*1) / fill_zero(document.getElementById('" + mq0 + "txtSWeight').value*1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtGSM').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtTop').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtMiddle').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtBottom').value * 1)+((fill_zero(document.getElementById('" + mq0 + "txtFluteB').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtFluteA').value * 1))*1.45)).toFixed(3);";

        // vipin
        vip = vip + "WholeString=(document.getElementById('" + mq0 + "txtMin').value*1);";
        vip = vip + "Splitt=WholeString.toString().split('.');";
        vip = vip + "Le=Splitt[0].length;";
        vip = vip + "if (Le > 1){ ";
        vip = vip + "ConvertToWhole=Le-2; if(ConvertToWhole==0) { madhvi = 100 } else {";
        vip = vip + "Substrng=WholeString.toString().substring(0,ConvertToWhole);";
        vip = vip + "strConvert=parseInt(Substrng);";
        vip = vip + "ChangeStr=strConvert+1;";
        vip = vip + "madhvi = ChangeStr+'00'} }";
        vip = vip + " else if (Le ==1){";
        vip = vip + "ChangeStr=1;";
        vip = vip + "madhvi = 100};";
        vip = vip + " if( madhvi - WholeString == 100) {  if ( WholeString >= 100) { document.getElementById('" + mq0 + "txtMin').value = WholeString; }   }  ";
        vip = vip + " else { document.getElementById('" + mq0 + "txtMin').value = madhvi;  }";
        // If condition for Purcase and min
        vip = vip + "purchase=(document.getElementById('" + mq0 + "txtPurchase').value*1);";
        vip = vip + "minqty=(document.getElementById('" + mq0 + "txtMin').value*1);";
        vip = vip + "if(purchase<=minqty){";
        vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        vip = vip + "}";
        vip = vip + "else {";
        vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        vip = vip + "};";
        //if ((txtMinQty.Text != "") && (txtPurchase.Text != ""))
        //    {
        //        if ((txtPurchase.Text == "NaN") || (txtPurchase.Text == "Infinity") || (txtMinQty.Text == "NaN") || (txtMinQty.Text == "Infinity"))
        //        {
        //            txtPurchase.Text = "0";
        //            txtMin.Text = "0";
        //        }
        //        if (Convert.ToDecimal(txtPurchase.Text) <= Convert.ToDecimal(txtMinQty.Text))
        //        {
        //            vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //            vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //            vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtPurchase').value * 1)).toFixed(3) ;";
        //        }
        //        else
        //        {
        //            vip = vip + "document.getElementById('" + mq0 + "txtProcessRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtProcess').value *1)/100)*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        //            vip = vip + "document.getElementById('" + mq0 + "txtBoardRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSWeight').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtBoard').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        //            vip = vip + "document.getElementById('" + mq0 + "txtPrintingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtPrinting').value *1))*fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)/fill_zero(document.getElementById('" + mq0 + "txtMin').value * 1)).toFixed(3) ;";
        //        }
        //    }
        vip = vip + "document.getElementById('" + mq0 + "txtDieRate').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtDie').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtSheet').value *1)).toFixed(3) ;";

        vip = vip + "document.getElementById('" + mq0 + "txtWaterRate').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtSheet').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtWater').value *1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtStitchingRate').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtStitching').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtSWeight').value *1)).toFixed(3) ;";
        // vip = vip + "document.getElementById('" + mq0 + "txtProfitRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtWastageRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtProfit').value *1)/100) ;";
        vip = vip + "document.getElementById('" + mq0 + "txtTapingRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtHeight').value * 1)/1000)*4*fill_zero(document.getElementById('" + mq0 + "txtTaping').value * 1)).toFixed(3);";
        //vip = vip + "document.getElementById('" + mq0 + "txtRateTop').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWTop').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBFTopRate').value * 1)).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtRateTop').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWTop').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfTopG').value * 1)).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtRateMiddle').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWMiddle').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfMiddleG').value * 1)).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtRateBottom').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWBottom').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfBottomG').value * 1)).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtRateFlute').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtWFlute').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtBfFluteG').value * 1)).toFixed(3);";

        // vip = vip + "document.getElementById('" + mq0 + "txtMaterial').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtWTop').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtRateTop').value * 1))+(fill_zero(document.getElementById('" + mq0 + "txtWMiddle').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtRateMiddle').value * 1))+(fill_zero(document.getElementById('" + mq0 + "txtWBottom').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtRateBottom').value * 1))+(fill_zero(document.getElementById('" + mq0 + "txtWFlute').value * 1)*fill_zero(document.getElementById('" + mq0 + "txtRateFlute').value * 1))).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtMaterial').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtRateTop').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtRateMiddle').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtRateBottom').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtRateFlute').value * 1)).toFixed(3);";
        //if (cmbBoxTypes.Text == "UNIVERSAL")
        //{
        //    vip = vip + "document.getElementById('" + mq0 + "txtProfitRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtProfit').value *1)/100).toFixed(3);";
        //}
        //else
        //{
        vip = vip + "document.getElementById('" + mq0 + "txtProfitRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*fill_zero(document.getElementById('" + mq0 + "txtProfit').value *1)/100).toFixed(3);";
        //}

        //vip = vip + "document.getElementById('" + mq0 + "txtProfitRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtProfit').value *1)/100).toFixed(3);";

        vip = vip + "document.getElementById('" + mq0 + "txtFreightRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*fill_zero(document.getElementById('" + mq0 + "txtFreight').value *1)/100).toFixed(3) ;";


        vip = vip + "document.getElementById('" + mq0 + "txtPymtRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)  + fill_zero(document.getElementById('" + mq0 + "txtFreightRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtPacking').value * 1)+fill_zero(document.getElementById('" + mq0 + "txtAny').value * 1))*2/100 *fill_zero(document.getElementById('" + mq0 + "txtPymt').value *1)).toFixed(3) ;";



        //vip = vip + "document.getElementById('" + mq0 + "txtBasic').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtPymtRate').value *1)+ (fill_zero(document.getElementById('" + mq0 + "txtPacking').value *1)/100)+ fill_zero(document.getElementById('" + mq0 + "txtAny').value *1))).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtBasic').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtMaterial').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtPymtRate').value *1)+ (fill_zero(document.getElementById('" + mq0 + "txtPacking').value *1))+ fill_zero(document.getElementById('" + mq0 + "txtAny').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProfitRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtProcessRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtBoardRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtPrintingRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtDieRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtWaterRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtStitchingRate').value *1)  + fill_zero(document.getElementById('" + mq0 + "txtFreightRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtTapingRate').value * 1)).toFixed(3);";

        vip = vip + "document.getElementById('" + mq0 + "txtExciseRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtBasic').value * 1) * fill_zero(document.getElementById('" + mq0 + "txtExcise').value *1))/100).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtSalesRate').value =fill_zero((fill_zero(document.getElementById('" + mq0 + "txtBasic').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtExciseRate').value *1))*fill_zero(document.getElementById('" + mq0 + "txtSales').value *1)/100).toFixed(3);";
        vip = vip + "document.getElementById('" + mq0 + "txtTotal').value =fill_zero(fill_zero(document.getElementById('" + mq0 + "txtBasic').value * 1) + fill_zero(document.getElementById('" + mq0 + "txtExciseRate').value *1)+fill_zero(document.getElementById('" + mq0 + "txtSalesRate').value *1)).toFixed(3);";


        vip = vip + "}";
        vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", vip.ToString(), false);
    }

    protected void cmbBoxTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fgen.fill_zero(this.Controls);
        //  fgen.ResetForm(this.Controls);
        //myfun();
        cal();

    }

    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("Icode", typeof(string)));
        dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
        dt1.Columns.Add(new DataColumn("Irate", typeof(string)));
        // dt1.Columns.Add(new DataColumn("Aname", typeof(string)));
        //dt1.Columns.Add(new DataColumn("Quantity", typeof(string)));
        dt1.Columns.Add(new DataColumn("T80", typeof(string)));
    }

    public void add_blankrows()
    {

        dr1 = dt1.NewRow();

        dr1["Srno"] = dt1.Rows.Count + 1;
        dr1["icode"] = "-";
        dr1["iname"] = "-";
        dr1["Irate"] = "0";
        //dr1["Aname"] = "-";
        //dr1["Quantity"] = "0";
        dr1["T80"] = "0";
        dt1.Rows.Add(dr1);
    }

    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "Rmv":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString(); hffield.Value = "Rmv";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove this item from list");
                }
                break;
            case "Add":
                if (txtPCode.Text == "" || txtParty.Text == "")
                    fgen.msg("-", "AMSG", "Firstly Please Select Party!!");
                else
                {
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        hffield.Value = "Add_E";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "Add";
                        make_qry_4_popup();
                        fgen.Fn_open_mseek("Select Your Product(s)", frm_qstr);
                    }
                    //myfun();
                    cal();
                    //  GridCal();
                    //GridCalculation();
                }
                break;
        }
    }

    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        // GridCal();
        //GridCalculation();
        //}
    }

    //protected void txtPurchase_TextChanged(object sender, EventArgs e)
    //{
    //    //myfun();
    //    cal();
    //}

    public void GridCal()
    {
        vip = ""; mq0 = "ContentPlaceHolder1_";
        vip = vip + "<script type='text/javascript'>function calculateGridSum() {";
        vip = vip + "var Total = 0; var grdTotal=0;";
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            vip = vip + "Total = (fill_zero(document.getElementById('" + mq0 + "sg1_txtQty_" + i + "').value) * fill_zero(document.getElementById('" + mq0 + "sg1_txtCol16_" + i + "').value));";

            vip = vip + "grdTotal = (grdTotal * 1) + (Total * 1) ;";

        }

        vip = vip + "document.getElementById('ContentPlaceHolder1_txtGrdTotal').value = (grdTotal).toFixed(3); ";

        vip = vip + "}";
        vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall122", vip.ToString(), false);

    }

    public void GridCalculation()
    {
        fgen.fill_zero(this.Controls);
        double frm_double_Total = 0, frm_double_Cal = 0;

        for (int i = 0; i < sg1.Rows.Count; i++)
        {

            TextBox txtQty = (TextBox)(sg1.Rows[i].FindControl("txtQty"));
            TextBox txtRate = (TextBox)(sg1.Rows[i].FindControl("txtCol16"));
            frm_double_Cal = Convert.ToDouble(txtQty.Text.Trim()) * Convert.ToDouble(txtRate.Text.Trim());
            frm_double_Total = frm_double_Total + frm_double_Cal;
        }
        txtGrdTotal.Text = Math.Round(frm_double_Total, 3).ToString();
    }

    protected void txtQty_TextChanged(object sender, EventArgs e)
    {
        //myfun();
        cal();
        GridCalculation();

    }

    protected void txtCol16_TextChanged(object sender, EventArgs e)
    {
        //myfun();
        cal();
        GridCalculation();
    }

    protected void txtItem_TextChanged(object sender, EventArgs e)
    {
        txtICode.Visible = false;
        txtICode.Text = "";
        //hffield.Value = "MANUAL";
        EntryMode = "MANUAL";
        btnItem.Visible = false;
    }
    protected void btnLast_ServerClick(object sender, EventArgs e)
    {
        if (txtICode.Text == "" || txtICode.Text == "0") { fgen.msg("-", "AMSG", "Please select Item first"); }
        else
        {
            hffield.Value = "";
            hffield.Value = "LSO";
            fgen.Fn_open_prddmp1("-", frm_qstr);
        }
    }
    protected void btnInvoice_ServerClick(object sender, EventArgs e)
    {
        if (txtICode.Text == "" || txtICode.Text == "0") { fgen.msg("-", "AMSG", "Please select Item first"); }
        else
        {
            hffield.Value = "";
            hffield.Value = "LINV";
            fgen.Fn_open_prddmp1("-", frm_qstr);
        }
    }
}