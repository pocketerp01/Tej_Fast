using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class frmBoxMasterUpdate : System.Web.UI.Page
{
    DataTable dtb, dtb1;
    DataRow dbrow, dr1;
    DataSet oDS;
    DataTable dt, dt1; DataRow oporow; 
    string btnval, col1, col2, col3, fill_Date, vip = "",DateRange,Checked_ok = "Y";
    string mq0, pk_error = "Y", chk_rights = "N", tmp_var, frm_formID;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_UserID;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        frm_tabname = "Scratch";
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
                        frm_qstr = frm_qstr.Split('@')[0].ToString().Trim();
                    }

                    DataTable ddt = new DataTable();
                    ddt = fgenMV.Fn_Mvar_Rows(frm_qstr);
                    if (ddt.Rows.Count > 0)
                    {
                        frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                        frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                        frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                      //  frm_datrange = fgen.Fn_Get_Mvar(frm_qstr, "U_prdRANGE");
                        //frm_datrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                        frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                        tmp_var = "A";
                    }
                }
            }
            cmdnew.Focus();
            fill_Date = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                cmdnew.Focus();
                //set_Val();


            }
            cmdedit.Visible = false;
            cmdprint.Visible = false;
            cmddel.Visible = false;
        }
    }

    protected void cmdnew_Click(object sender, EventArgs e)
    {
        // for new button popup

        disablectrl();
        //Enable();
        fgen.EnableForm(this.Controls);
       
        chk_rights = fgen.Fn_chk_can_add(frm_qstr,frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();
        if (chk_rights == "Y")
        {
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('SSEEK.aspx?STR=" + frm_url + "','80%','80%','Pocketdriver Limited');", true);
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
        btnParty.Disabled = false ;
        btncancel.Visible = true;
        cmdexit.Visible = false;
    }

    public void enablectrl()
    {
        // for enable/disable some variables
        cmdnew.Disabled = false;
        cmdedit.Disabled = true;
        btncancel.Visible = false;
        cmddel.Disabled = true;
        btnParty.Disabled = true;
        cmdexit.Visible = true;
        btnsave.Disabled = true;
        cmdprint.Disabled = true;
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
        
        frm_tabname = "Scratch";
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
            case "CMD_REP2":
                break;
            case "Party":
                frm_sql = "Select F.Acode as fstr,F.Aname as PartyName,F.Acode as Code,F.Addr1 as Address From Famst F Where F.Acode like '16%' ORDER BY F.Aname";
                break;

            case "State":
                frm_sql = "Select type1 as fstr,name,type1 as Code  From Typegrp Where ID='ES' Order By Name";
                break;
            
            case "Row_Add":
            case "Row_Edit":
                //if (sg1.Rows.Count > 1)
                //{
                //    col1 = "";
                //    col2 = "";
                //    foreach (GridViewRow r1 in sg1.Rows)
                //    {
                //        if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[3].Text.Trim() + "'";
                //        else col2 = "'" + r1.Cells[3].Text.Trim() + "'";
                //    }
                //    col2 = "(" + col2 + ")";
                //}
                //else col2 = " ('A')";
                frm_sql = "Select x.Type1 as fstr, replace( x.name,'&','') as Stage_Name,x.Type1 as Route_Code from type x  where id='K' and trim(type1) not in " + col2 + " order by x.type1";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")

                  //  frm_sql = "Select DISTINCT S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy')||TRIM(S.ACODE) AS FSTR,S.VCHNUM,F.ANAME AS PARTYNAME,S.ACODE AS CODE,S.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,S.COL16 AS OLD_RATE,S.COL18 AS NEW_RATE FROM SCRATCH S,FAMST F,ITEM I WHERE  TRIM(F.ACODE)=TRIM(S.ACODE) AND TRIM(S.ICODE)=TRIM(I.ICODE) AND S.BRANCHCD='" + frm_mbr + "' AND S.TYPE='CM' AND NVL(S.COL18,'0')!=0 AND S.VCHDATE " + frm_datrange + " ORDER BY S.VCHNUM DESC";
                    frm_sql = "Select DISTINCT S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy')||TRIM(S.ACODE) AS FSTR,S.VCHNUM,F.ANAME AS PARTYNAME,S.ACODE AS CODE FROM SCRATCH S,FAMST F,ITEM I WHERE  TRIM(F.ACODE)=TRIM(S.ACODE) AND TRIM(S.ICODE)=TRIM(I.ICODE) AND S.BRANCHCD='" + frm_mbr + "' AND S.TYPE='CM'  AND S.VCHDATE " + DateRange + " ORDER BY S.VCHNUM DESC";
               
                    if (btnval == "New" || btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")

                    frm_sql = "select 'CM' AS FSTR,'Rate Updation' as heading,'CM' as type from dual ";
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
                                frm_sql = "SELECT DISTINCT TRIM(A.BRANCHCD)||TRIM(A.ACODE)||trim(A.icode) AS FSTR,A.ICODE AS erp_code,I.INAME AS ITEM,A.COL16 AS OLD_RATE FROM SCRATCH A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND TRIM(A.ICODE) not in (" + col2 + ")  AND ACODE='" + txtPCode.Text + "' ORDER BY A.ICODE";
                                //frm_sql = "select icode as fstr,iname as Item,icode as erp_code from item where substr(icode,1,1) in ('1','2','3','4','5','6','7','8') and trim(icode) not in (" + col2 + ") and length(trim(icode))=8 order by iname";
                            }
                            else
                                frm_sql = "SELECT DISTINCT TRIM(A.BRANCHCD)||TRIM(A.ACODE)||trim(A.icode) AS FSTR,A.ICODE AS erp_code,I.INAME AS ITEM,A.COL16 AS OLD_RATE FROM SCRATCH A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND TYPE='CM'  AND ACODE='" + txtPCode.Text + "' ORDER BY A.ICODE";
                                //frm_sql = "select icode as fstr,iname as Item,icode as erp_code,Irate,Unit,Icat from item where trim(icode)like '07%' and length(trim(icode))=4 order by iname";
                           // frm_sql = "select icode as fstr,iname as Item,icode as erp_code,Irate,Unit,Icat from item where substr(icode,1,1) in ('1','2','3','4','5','6','7','8') and length(trim(icode))=8 order by iname";
                        }
                
                break;
        }
        //}

        if (frm_sql.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        // for save button checking & working
        chk_rights = fgen.Fn_chk_can_add(frm_qstr,frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in this form!!");
            return;
        }

        chk_rights = fgen.Fn_chk_can_edit(frm_qstr,frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N" && edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in edit mode!!");
            return;
        }
        else
        {
            fgen.fill_zero(this.Controls);
            
            // int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
            //if (dhd == 0)
            //{ fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }
            //if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fgen.Fn_Get_Mvar(frm_qstr, "U_CDT1")) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(fgen.Fn_Get_Mvar(frm_qstr, "U_CDT2")))
            //{ fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only"); txtvchdate.Focus(); return; }
            if (sg1.Rows.Count < 0)
            { fgen.msg("-", "AMSG", "No Item to Save!!'13'Please Select Some item first"); return; }

            for (int i = 0; i < sg1.Rows.Count ; i++)
            {
                TextBox t = (TextBox)(sg1.Rows[i].FindControl("txtCol17"));
                string a = t.Text;
                if (Convert.ToString(((TextBox)sg1.Rows[i].FindControl("txtCol17")).Text) == "-")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",New Rate for Item Code " + sg1.Rows[i].Cells[3].Text.ToString().Trim() + " Can not be Blank."); return;
                }
                else if (Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtCol17")).Text) <= 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",New Rate for Item Code " + sg1.Rows[i].Cells[3].Text.ToString().Trim() + " Can not be Zero or less then Zero "); return;
                }
                fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                btnsave.Disabled = true;
            }
        }
    }
    protected void cmdedit_Click(object sender, EventArgs e)
    {
        // for edit button popup
        clearctrl();
        //fgen.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
        set_Val();
        hffield.Value = "Edit";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void cmddel_Click(object sender, EventArgs e)
    {
        // for del button working
        chk_rights = fgen.Fn_chk_can_del(frm_qstr,frm_cocd, frm_UserID, frm_formID);
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
        ViewState["sg1"] = null;
        sg1.DataSource = null;
        sg1.DataBind();
    }

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }

    void save_data()
    {
        string frm_ent_time = fgen.Fn_curr_dt_time(frm_cocd, frm_qstr);

        #region Master Rate Updation
        oDS = new DataSet();
        oDS = fgen.fill_schema(frm_qstr,frm_cocd, frm_tabname);
        dbrow = null;
        dt1 = new DataTable();
        dt1 = (DataTable)ViewState["sg1"];
        DataTable dtgrd = new DataTable();
        dtgrd = dt1.Clone();
        tmp_var = "";
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            mq0 = "SELECT DISTINCT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR, I.INAME,F.ANAME,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VDD,A.* FROM SCRATCH A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "'  AND A.TYPE='" + frm_vty + "' AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE)='" + sg1.Rows[i].Cells[10].Text.Trim() + "' ORDER BY A.ACODE";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr,frm_cocd, mq0);
            frm_sql = "UPDATE SCRATCH SET BRANCHCD='DD' WHERE TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||TRIM(ACODE)||TRIM(ICODE)='" + sg1.Rows[i].Cells[10].Text.Trim() + "'";
            fgen.execute_cmd(frm_qstr,frm_cocd, frm_sql);
            dbrow = oDS.Tables[0].NewRow();
            oporow = dtgrd.NewRow();
            dbrow["BRANCHCD"] = frm_mbr;
            dbrow["TYPE"] = frm_vty;
            //dbrow["VCHNUM"] = sg1.Rows[i].Cells[10].Text.Substring(4, 6);
            //dbrow["vchdate"] = sg1.Rows[i].Cells[10].Text.Substring(10, 10);
            dbrow["VCHNUM"] = dt.Rows[0]["VCHNUM"].ToString().Trim();
            dbrow["vchdate"] = dt.Rows[0]["VDD"].ToString().Trim();
            dbrow["srno"] = (i + 1);
            dbrow["Acode"] = sg1.Rows[i].Cells[5].Text.Trim().ToUpper();
            tmp_var += ",'" + sg1.Rows[i].Cells[5].Text.Trim().ToUpper() + "'";
            dbrow["Icode"] = sg1.Rows[i].Cells[3].Text.Trim().ToUpper();
            dbrow["Col3"] = dt.Rows[0]["COL3"].ToString().Trim();//PROCESS
            dbrow["Col4"] = dt.Rows[0]["Col4"].ToString().Trim();//BOARD
            dbrow["Col5"] = dt.Rows[0]["Col5"].ToString().Trim();//PRINTING
            dbrow["Col6"] = dt.Rows[0]["Col6"].ToString().Trim();//WATER
            dbrow["Col7"] = dt.Rows[0]["Col7"].ToString().Trim();//DIE
            dbrow["Col8"] = dt.Rows[0]["Col8"].ToString().Trim();//STITCHING
            dbrow["Col9"] = dt.Rows[0]["Col9"].ToString().Trim();//TAPING
            dbrow["Col10"] = dt.Rows[0]["Col10"].ToString().Trim();//PACiING
            dbrow["Col11"] = dt.Rows[0]["Col11"].ToString().Trim();//PROFIT
            dbrow["Col12"] = dt.Rows[0]["Col12"].ToString().Trim();//FREIGHT
            dbrow["Col13"] = dt.Rows[0]["Col13"].ToString().Trim();//PAYMENT
            dbrow["Col14"] = dt.Rows[0]["Col14"].ToString().Trim();//EXCISE
            dbrow["Col15"] = dt.Rows[0]["Col15"].ToString().Trim();//SALES
           // dbrow["Col16"] = ((TextBox)(sg1.Rows[i].FindControl("txtCol16"))).Text.Trim();// OLD RATE
            dbrow["Col17"] = dt.Rows[0]["Col17"].ToString().Trim();// MIN QTY
           // dbrow["Col18"] = ((TextBox)(sg1.Rows[i].FindControl("txtCol17"))).Text.Trim();// NEW RATE
            dbrow["Col16"] = ((TextBox)(sg1.Rows[i].FindControl("txtCol17"))).Text.Trim();// NEW RATE
            dbrow["Col19"] = "UPDATED RATE";
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
                dbrow["edt_by"] = frm_uname;
                dbrow["edt_dt"] = System.DateTime.Now;
            }

            oDS.Tables[0].Rows.Add(dbrow);
            // ADDING GRD DATA TO dtgrd
            oporow["icode"] = sg1.Rows[i].Cells[3].Text.Trim();
            oporow["iname"] = sg1.Rows[i].Cells[4].Text.Trim();
            oporow["acode"] = sg1.Rows[i].Cells[5].Text.Trim();
            oporow["aname"] = sg1.Rows[i].Cells[6].Text.Trim();
            oporow["col16"] = ((TextBox)(sg1.Rows[i].FindControl("txtCol16"))).Text.Trim();
            oporow["col17"] = ((TextBox)(sg1.Rows[i].FindControl("txtCol17"))).Text.Trim();
            dtgrd.Rows.Add(oporow);
        }
        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
        dt.Dispose();
        dt1.Dispose();
        #endregion

        #region Costing Sheet Rate Updation
       // frm_sql = "Select DISTINCT S.branchcd||S.type||TRIM(S.ACODE) AS FSTR,S.VCHNUM,F.ANAME AS PARTYNAME,S.ACODE AS CODE,S.ICODE,S.COL16,S.COL18,I.INAME FROM SCRATCH S,FAMST F,ITEM I WHERE  TRIM(F.ACODE)=TRIM(S.ACODE) AND TRIM(S.ICODE)=TRIM(I.ICODE) AND S.BRANCHCD='" + frm_mbr + "' AND S.TYPE='CM' AND NVL(S.COL18,'0')!=0 AND S.VCHDATE " + frm_datrange + " ORDER BY S.VCHNUM DESC";
        #region Rate Update in Costing Table
        frm_sql = "SELECT  DISTINCT T40,T42,T44,T46,ACODE,t85,t86,t87,t88,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE FROM SOMAS_ANX WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND ACODE IN(" + tmp_var.TrimStart(',') + ")";
        dtb1 = new DataTable();
        dtb1 = fgen.getdata(frm_qstr,frm_cocd, frm_sql);
        if (dt1.Rows.Count > 0)
        {
            DataView view = new DataView(dtb1);
            dtb = new DataTable();
            dtb = view.ToTable(true, "acode", "vchnum", "vchdate");
            foreach (DataRow dr0 in dtb.Rows)
            {
               // DataRow drrow1 = dtm11.NewRow();
                DataView viewim = new DataView(dtb1, "acode='" + dr0["acode"].ToString().Trim() + "' and vchnum='" + dr0["vchnum"].ToString().Trim() + "' and vchdate='" + dr0["vchdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                dt1 = new DataTable();
                dt1 = viewim.ToTable();
                btnval += ",'" + dr0["acode"].ToString().Trim() + dr0["vchnum"].ToString().Trim() + dr0["vchdate"].ToString().Trim() + "'";
                for (int i = 0; i < dtgrd.Rows.Count; i++)
                {
                    for (int k = 0; k < dt1.Rows.Count; k++)
                    {
                        if (dtgrd.Rows[i]["iname"].ToString().Trim() + dtgrd.Rows[i]["acode"].ToString().Trim() == dt1.Rows[k]["t40"].ToString().Trim() + dt1.Rows[k]["acode"].ToString().Trim())
                        {
                            mq0 = "UPDATE SOMAS_ANX SET T85='" + dtgrd.Rows[i]["COL17"].ToString().Trim() + "' WHERE T40='" + dtgrd.Rows[i]["iname"].ToString().Trim() + "' AND ACODE='" + dtgrd.Rows[i]["acode"].ToString().Trim() + "' AND BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND VCHNUM='" + dt1.Rows[k]["VCHNUM"].ToString().Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dt1.Rows[k]["VCHDATE"].ToString().Trim() + "'";
                            fgen.execute_cmd(frm_qstr,frm_cocd, mq0);
                        }
                        if (dtgrd.Rows[i]["iname"].ToString().Trim() + dtgrd.Rows[i]["acode"].ToString().Trim() == dt1.Rows[k]["t42"].ToString().Trim() + dt1.Rows[k]["acode"].ToString().Trim())
                        {
                            mq0 = "UPDATE SOMAS_ANX SET T86='" + dtgrd.Rows[i]["COL17"].ToString().Trim() + "' WHERE T42='" + dtgrd.Rows[i]["iname"].ToString().Trim() + "' AND ACODE='" + dtgrd.Rows[i]["acode"].ToString().Trim() + "' AND BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND VCHNUM='" + dt1.Rows[k]["VCHNUM"].ToString().Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dt1.Rows[k]["VCHDATE"].ToString().Trim() + "'";
                            fgen.execute_cmd(frm_qstr,frm_cocd, mq0);
                        }
                        if (dtgrd.Rows[i]["iname"].ToString().Trim() + dtgrd.Rows[i]["acode"].ToString().Trim() == dt1.Rows[k]["t44"].ToString().Trim() + dt1.Rows[k]["acode"].ToString().Trim())
                        {
                            mq0 = "UPDATE SOMAS_ANX SET T87='" + dtgrd.Rows[i]["COL17"].ToString().Trim() + "' WHERE T44='" + dtgrd.Rows[i]["iname"].ToString().Trim() + "' AND ACODE='" + dtgrd.Rows[i]["acode"].ToString().Trim() + "' AND BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND VCHNUM='" + dt1.Rows[k]["VCHNUM"].ToString().Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dt1.Rows[k]["VCHDATE"].ToString().Trim() + "'";
                            fgen.execute_cmd(frm_qstr,frm_cocd, mq0);
                        }
                        if (dtgrd.Rows[i]["iname"].ToString().Trim() + dtgrd.Rows[i]["acode"].ToString().Trim() == dt1.Rows[k]["t46"].ToString().Trim() + dt1.Rows[k]["acode"].ToString().Trim())
                        {
                            mq0 = "UPDATE SOMAS_ANX SET T88='" + dtgrd.Rows[i]["COL17"].ToString().Trim() + "' WHERE T46='" + dtgrd.Rows[i]["iname"].ToString().Trim() + "' AND ACODE='" + dtgrd.Rows[i]["acode"].ToString().Trim() + "' AND BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND VCHNUM='" + dt1.Rows[k]["VCHNUM"].ToString().Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + dt1.Rows[k]["VCHDATE"].ToString().Trim() + "'";
                            fgen.execute_cmd(frm_qstr,frm_cocd, mq0);
                        }
                    }
                }
            }
        }
#endregion


        #region Cal
        //    frm_sql = "SELECT DISTINCT TRIM(A.branchcd)||TRIM(A.type)||TRIM(A.ACODE) AS FSTR,F.ANAME AS PARTYNAME,S.ACODE AS CODE,S.ICODE AS SICODE,S.COL16,S.COL18,I.INAME,A.* FROM SCRATCH S,FAMST F,ITEM I,SOMAS_ANX A WHERE  TRIM(F.ACODE)=TRIM(S.ACODE) AND TRIM(S.ICODE)=TRIM(I.ICODE) AND S.BRANCHCD='" + frm_mbr + "' AND S.TYPE='CM' AND NVL(S.COL18,'0')!=0 AND TRIM(S.branchcd)||TRIM(S.type)||TRIM(S.ACODE)=TRIM(A.branchcd)||TRIM(A.type)||TRIM(A.ACODE) AND A.VCHDATE " + frm_datrange + " AND S.ACODE IN(" + tmp_var.TrimStart(',') + ") ORDER BY CODE";
        frm_sql = "SELECT DISTINCT A.*,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VDD FROM SOMAS_ANX A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='CM' AND  A.VCHDATE " + DateRange + " AND A.ACODE IN(" + tmp_var.TrimStart(',') + ") ORDER BY A.ACODE,A.VCHNUM";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, frm_sql);
        dt1 = new DataTable();
        dr1 = null;
        dt1 = dt.Clone();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dr1 = dt1.NewRow();
            fgen.fill_zero(this.Controls);
            dr1["BRANCHCD"] = dt.Rows[i]["BRANCHCD"].ToString().Trim();
            dr1["TYPE"] = dt.Rows[i]["TYPE"].ToString().Trim();
            dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
            dr1["VCHDATE"] = dt.Rows[i]["VDD"].ToString().Trim();
            dr1["ACODE"] = dt.Rows[i]["ACODE"].ToString().Trim();
            dr1["ICODE"] = dt.Rows[i]["ICODE"].ToString().Trim();
           // btnval += ",'" + dt.Rows[i]["acode"].ToString().Trim() + dt.Rows[i]["vchnum"].ToString().Trim() + dt.Rows[i]["vchdate"].ToString().Trim() + "'";
            dr1["T1"] = dt.Rows[i]["T1"].ToString().Trim();
            dr1["T2"] = dt.Rows[i]["T2"].ToString().Trim();
            dr1["T3"] = dt.Rows[i]["T3"].ToString().Trim();
            dr1["T4"] = dt.Rows[i]["T4"].ToString().Trim();
            dr1["T5"] = dt.Rows[i]["T5"].ToString().Trim();
            dr1["T6"] = dt.Rows[i]["T6"].ToString().Trim();
            dr1["T7"] = dt.Rows[i]["T7"].ToString().Trim();
            dr1["T8"] = dt.Rows[i]["T8"].ToString().Trim();
            dr1["T9"] = dt.Rows[i]["T9"].ToString().Trim();
            dr1["T10"] = dt.Rows[i]["T10"].ToString().Trim();
            dr1["T11"] = dt.Rows[i]["T11"].ToString().Trim();
            dr1["T12"] = dt.Rows[i]["T12"].ToString().Trim();
            dr1["T19"] = dt.Rows[i]["T19"].ToString().Trim();
            dr1["T23"] = dt.Rows[i]["T23"].ToString().Trim();
            dr1["T24"] = dt.Rows[i]["T24"].ToString().Trim();
            dr1["T25"] = dt.Rows[i]["T25"].ToString().Trim();
            dr1["T26"] = dt.Rows[i]["T26"].ToString().Trim();
            dr1["T28"] = dt.Rows[i]["T28"].ToString().Trim();
            dr1["T37"] = dt.Rows[i]["T37"].ToString().Trim();
            dr1["T39"] = dt.Rows[i]["T39"].ToString().Trim();
            dr1["T40"] = dt.Rows[i]["T40"].ToString().Trim();
            dr1["T41"] = dt.Rows[i]["T41"].ToString().Trim();
            dr1["T42"] = dt.Rows[i]["T42"].ToString().Trim();
            dr1["T43"] = dt.Rows[i]["T43"].ToString().Trim();
            dr1["T44"] = dt.Rows[i]["T44"].ToString().Trim();
            dr1["T45"] = dt.Rows[i]["T45"].ToString().Trim();
            dr1["T46"] = dt.Rows[i]["T46"].ToString().Trim();
            dr1["T47"] = dt.Rows[i]["T47"].ToString().Trim();
            dr1["T53"] = dt.Rows[i]["T53"].ToString().Trim();
            dr1["T55"] = dt.Rows[i]["T55"].ToString().Trim();
            dr1["T57"] = dt.Rows[i]["T57"].ToString().Trim();
            dr1["T59"] = dt.Rows[i]["T59"].ToString().Trim();
            dr1["T61"] = dt.Rows[i]["T61"].ToString().Trim();
            dr1["T62"] = dt.Rows[i]["T62"].ToString().Trim();
            dr1["T65"] = dt.Rows[i]["T65"].ToString().Trim();
            dr1["T67"] = dt.Rows[i]["T67"].ToString().Trim();
            dr1["T68"] = dt.Rows[i]["T68"].ToString().Trim();
            dr1["T70"] = dt.Rows[i]["T70"].ToString().Trim();
            dr1["T72"] = dt.Rows[i]["T72"].ToString().Trim();
            dr1["T74"] = dt.Rows[i]["T74"].ToString().Trim();
            dr1["T76"] = dt.Rows[i]["T76"].ToString().Trim();
            dr1["T78"] = dt.Rows[i]["T78"].ToString().Trim();
            dr1["T80"] = dt.Rows[i]["T80"].ToString().Trim();
            dr1["T81"] = dt.Rows[i]["T81"].ToString().Trim();
            dr1["T82"] = dt.Rows[i]["T82"].ToString().Trim();
            dr1["T83"] = dt.Rows[i]["T83"].ToString().Trim();
            dr1["T84"] = dt.Rows[i]["T84"].ToString().Trim();
            dr1["T85"] = dt.Rows[i]["T85"].ToString().Trim();
            dr1["T86"] = dt.Rows[i]["T86"].ToString().Trim();
            dr1["T87"] = dt.Rows[i]["T87"].ToString().Trim();
            dr1["T88"] = dt.Rows[i]["T88"].ToString().Trim();
            dr1["T140"] = dt.Rows[i]["T140"].ToString().Trim();
            dr1["ENT_BY"] = dt.Rows[i]["ENT_BY"].ToString().Trim();
            dr1["ENT_DT"] = dt.Rows[i]["ENT_DT"].ToString().Trim();
            dr1["EDT_BY"] = dt.Rows[i]["EDT_BY"].ToString().Trim();
            dr1["EDT_DT"] = dt.Rows[i]["EDT_DT"].ToString().Trim();
            #region Universal
            if (dt.Rows[i]["T39"].ToString().Trim() == "UNIVERSAL")
            {
                //DECKLE  // HEIGHT // WIDTH // REEL
                //dr1["T13"] = Math.Round(fgen.return_double(dt.Rows[i]["T3"].ToString().Trim()) + fgen.return_double(dt.Rows[i]["T2"].ToString().Trim()) + fgen.return_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T2"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                //CUT SIZE    //LENGTH // WIDTH// CUT
                //dr1["T14"] = Math.Round((fgen.return_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.return_double(dt.Rows[i]["T2"].ToString().Trim())) * 2 + fgen.return_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
                dr1["T14"] = Math.Round((fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T2"].ToString().Trim())) * 2 + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region Over Flap Rac
            if (dt.Rows[i]["T39"].ToString().Trim() == "OVER FLAP RAC")
            {
                //DECKLE  // HEIGHT // WIDTH // REEL
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) + (fgen.make_double(dt.Rows[i]["T2"].ToString().Trim()) * 2) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                //CUT SIZE    //LENGTH // WIDTH// CUT
                dr1["T14"] = Math.Round((fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T2"].ToString().Trim())) * 2 + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region Half Rac
            if (dt.Rows[i]["T39"].ToString().Trim() == "HALF RAC")
            {
                //   txtDeckle.Text = Math.Round(fgen.return_double(txtHeight.Text.Trim()) + (fgen.return_double(txtWidth.Text.Trim()) / 2) + fgen.return_double(txtReel.Text.Trim()), 3).ToString();
                //   txtCutSize.Text = Math.Round((fgen.return_double(txtLength.Text.Trim()) + fgen.return_double(txtWidth.Text.Trim())) * 2 + fgen.return_double(txtCut.Text.Trim()), 3).ToString();
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) + (fgen.make_double(dt.Rows[i]["T2"].ToString().Trim()) / 2) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                dr1["T14"] = Math.Round((fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T2"].ToString().Trim())) * 2 + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region OVER FLAP HALF RAC
            if (dt.Rows[i]["T39"].ToString().Trim() == "OVER FLAP HALF RAC")
            {
                // txtDeckle.Text = Math.Round(fgen.return_double(txtHeight.Text.Trim()) + (fgen.return_double(txtWidth.Text.Trim())) + fgen.return_double(txtReel.Text.Trim()), 3).ToString();
                // txtCutSize.Text = Math.Round((fgen.return_double(txtLength.Text.Trim()) + fgen.return_double(txtWidth.Text.Trim())) * 2 + fgen.return_double(txtCut.Text.Trim()), 3).ToString();
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) + (fgen.make_double(dt.Rows[i]["T2"].ToString().Trim())) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                dr1["T14"] = Math.Round((fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T2"].ToString().Trim())) * 2 + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region Sleeve
            if (dt.Rows[i]["T39"].ToString().Trim() == "SLEEVE")
            {
                //txtDeckle.Text = Math.Round(fgen.return_double(txtHeight.Text.Trim()) + fgen.return_double(txtReel.Text.Trim()), 3).ToString();
                //txtCutSize.Text = Math.Round((fgen.return_double(txtLength.Text.Trim()) + fgen.return_double(txtWidth.Text.Trim())) * 2 + fgen.return_double(txtCut.Text.Trim()), 3).ToString();
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                dr1["T14"] = Math.Round((fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T2"].ToString().Trim())) * 2 + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region Tray
            if (dt.Rows[i]["T39"].ToString().Trim() == "TRAY")
            {
                // txtDeckle.Text = Math.Round(fgen.return_double(txtWidth.Text.Trim()) + (fgen.return_double(txtHeight.Text.Trim()) * 2) + fgen.return_double(txtReel.Text.Trim()), 3).ToString();
                // txtCutSize.Text = Math.Round((fgen.return_double(txtLength.Text.Trim()) + fgen.return_double(txtHeight.Text.Trim()) * 2) + fgen.return_double(txtCut.Text.Trim()), 3).ToString();
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T2"].ToString().Trim()) + (fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) * 2) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                dr1["T14"] = Math.Round((fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T3"].ToString().Trim()) * 2) + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region Sheet
            if (dt.Rows[i]["T39"].ToString().Trim() == "SHEET")
            {
                //txtDeckle.Text = Math.Round(fgen.return_double(txtWidth.Text.Trim()) + fgen.return_double(txtReel.Text.Trim()), 3).ToString();
                //txtCutSize.Text = Math.Round(fgen.return_double(txtLength.Text.Trim()) + fgen.return_double(txtCut.Text.Trim()), 3).ToString();
                dr1["T13"] = Math.Round(fgen.make_double(dt.Rows[i]["T2"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T11"].ToString().Trim()), 3).ToString();
                dr1["T14"] = Math.Round(fgen.make_double(dt.Rows[i]["T1"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T12"].ToString().Trim()), 3).ToString();
            }
            #endregion

            #region All

            //txtSheet.Text = Math.Round(fgen.return_double(txtDeckle.Text.Trim()) * fgen.return_double(txtCutSize.Text.Trim()) / 1000000, 3).ToString();
            dr1["T15"] = Math.Round(fgen.make_double(dt.Rows[i]["T13"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T14"].ToString().Trim()) / 1000000, 3).ToString();

            //txtWTop.Text = Math.Round(fgen.return_double(txtTop.Text.Trim()) * fgen.return_double(txtSheet.Text.Trim()) / 1000, 3).ToString();
            dr1["T16"] = Math.Round(fgen.make_double(dt.Rows[i]["T6"].ToString()) * fgen.make_double(dr1["T15"].ToString().Trim()) / 1000, 3).ToString();

            //  txtWMiddle.Text = Math.Round(fgen.return_double(txtMiddle.Text.Trim()) * fgen.return_double(txtSheet.Text.Trim()) / 1000, 3).ToString();
            dr1["T17"] = Math.Round(fgen.make_double(dt.Rows[i]["T8"].ToString().Trim()) * fgen.make_double(dr1["T15"].ToString().Trim()) / 1000, 3).ToString();

            //txtWBottom.Text = Math.Round(fgen.return_double(txtBottom.Text.Trim()) * fgen.return_double(txtSheet.Text.Trim()) / 1000, 3).ToString();
            dr1["T18"] = Math.Round(fgen.make_double(dt.Rows[i]["T10"].ToString().Trim()) * fgen.make_double(dr1["T15"].ToString().Trim()) / 1000, 3).ToString();

            // txtBS.Text = Math.Round((fgen.return_double(txtBFTopRate.Text.Trim()) * fgen.return_double(txtTop.Text.Trim()) / 1000) + (fgen.return_double(txtBFMiddleRate.Text.Trim()) * fgen.return_double(txtMiddle.Text.Trim()) / 1000) + (fgen.return_double(txtBFBottomRate.Text.Trim()) * fgen.return_double(txtBottom.Text.Trim()) / 1000), 3).ToString();
            dr1["T21"] = Math.Round((fgen.make_double(dt.Rows[i]["T41"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T6"].ToString().Trim()) / 1000) + (fgen.make_double(dt.Rows[i]["T43"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T8"].ToString().Trim()) / 1000) + (fgen.make_double(dt.Rows[i]["T45"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T10"].ToString().Trim()) / 1000), 3).ToString();

            //   txtSWeight.Text = Math.Round(fgen.return_double(txtWTop.Text.Trim()) + fgen.return_double(txtWMiddle.Text.Trim()) + fgen.return_double(txtWBottom.Text.Trim()) + fgen.return_double(txtWFlute.Text.Trim()), 3).ToString();
            dr1["T20"] = Math.Round(fgen.make_double(dr1["T16"].ToString().Trim()) + fgen.make_double(dr1["T17"].ToString().Trim()) + fgen.make_double(dr1["T18"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T19"].ToString().Trim()), 3).ToString();

            // txtMin.Text = Math.Round(fgen.return_double(txtMinQty.Text.Trim()) / fgen.return_double(txtSWeight.Text.Trim()), 3).ToString();
            dr1["T31"] = Math.Round(fgen.make_double(dt.Rows[i]["T37"].ToString().Trim()) / fgen.make_double(dr1["T20"].ToString().Trim()), 3).ToString();

            //if ((txtMin.Text != "") && (txtMin.Text != "0.000"))
            //{
            // vipin
            //txtMin.Text = Math.Round(fgen.return_double(txtMinQty.Text.Trim()) / fgen.return_double(txtSWeight.Text.Trim()), 3).ToString();
            dr1["T31"] = Math.Round(fgen.make_double(dt.Rows[i]["T37"].ToString().Trim()) / fgen.make_double(dr1["T20"].ToString().Trim()), 3).ToString();
            //if ((txtMin.Text != "NaN") && (txtMin.Text != "Infinity"))
            //{
            //string[] splitToInt = dt.Rows[i]["T31"].ToString().Trim().Split('.');
            string[] splitToInt = dr1["T31"].ToString().Trim().Split('.');
            Int64 Length = Convert.ToInt64(splitToInt[0].Length);
            Int64 ConvertToWhole = 0;
            string Substrng = "";
            int ChangeStr = 0;
            string Final = "";
            if (Length > 1)
            {
                ConvertToWhole = Length - 2;
                // Substrng = txtMin.Text.Substring(0, (int)ConvertToWhole);
               // last final Substrng = dt.Rows[i]["T31"].ToString().Trim().Substring(0, (int)ConvertToWhole);
                Substrng = dr1["T31"].ToString().Trim().Substring(0, (int)ConvertToWhole);
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
            //if ((fgen.return_double(Final) - fgen.return_double(txtMin.Text.Trim()) == 100) && fgen.return_double(txtMin.Text.Trim()) >= 100) { }
            //else txtMin.Text = Final;
            if ((fgen.make_double(Final) - fgen.make_double(dr1["T31"].ToString().Trim().Trim()) == 100) && fgen.make_double(dr1["T31"].ToString().Trim().Trim()) >= 100) { }
            else dr1["T31"] = Final;
            //    }
            //}

            // if (txtMin.Text == txtPurchase.Text)
           // if (dt.Rows[i]["T31"].ToString().Trim() == dt.Rows[i]["T32"].ToString().Trim())
            if (dr1["T31"].ToString().Trim() == dt.Rows[i]["T32"].ToString().Trim())
            {
                // txtPurchase.Text = txtMin.Text;
                dr1["T32"] = dt.Rows[i]["T31"].ToString().Trim();
            }
            // else if (txtPurchase.Text == "0")
            else if (dt.Rows[i]["T32"].ToString().Trim() == "0")
            {
                //txtPurchase.Text = txtMin.Text;
                dr1["T32"] = dt.Rows[i]["T31"].ToString().Trim();
            }
            else
            {
                dr1["T32"] = dt.Rows[i]["T31"].ToString().Trim();

            }
            // txtGSM.Text = Math.Round(fgen.return_double(txtTop.Text.Trim()) + fgen.return_double(txtMiddle.Text.Trim()) + fgen.return_double(txtBottom.Text.Trim()) + ((fgen.return_double(txtFluteB.Text.Trim()) + fgen.return_double(txtFluteA.Text.Trim())) * 1.45), 3).ToString();
            dr1["T22"] = Math.Round(fgen.make_double(dt.Rows[i]["T6"].ToString().Trim().Trim()) + fgen.make_double(dt.Rows[i]["T8"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T10"].ToString().Trim()) + ((fgen.make_double(dt.Rows[i]["T7"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T9"].ToString().Trim())) * 1.45), 3).ToString();

           // txtDieRate.Text = Math.Round(Convert.ToDouble(txtDie.Text.Trim()) * Convert.ToDouble(txtSheet.Text.Trim()), 3).ToString();
            dr1["T62"] = Math.Round(fgen.make_double(dt.Rows[i]["T61"].ToString().Trim()) * fgen.make_double(dr1["T15"].ToString().Trim().Trim()), 3).ToString();

            //  txtWaterRate.Text = Math.Round(fgen.return_double(txtSheet.Text.Trim()) * fgen.return_double(txtWater.Text.Trim()), 3).ToString();
            dr1["T60"] = Math.Round(fgen.make_double(dr1["T15"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T59"].ToString().Trim().Trim()), 3).ToString();
            dr1["T63"] = dt.Rows[i]["T63"].ToString();
            // txtStitchingRate.Text = Math.Round(fgen.return_double(txtStitching.Text.Trim()) * fgen.return_double(txtSWeight.Text.Trim()), 3).ToString();
            dr1["T64"] = Math.Round(fgen.make_double(dt.Rows[i]["T63"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T20"].ToString().Trim()), 3).ToString();

            // txtTapingRate.Text = Math.Round((fgen.return_double(txtHeight.Text.Trim())) / 1000 * 4 * fgen.return_double(txtTaping.Text.Trim()), 3).ToString();
            dr1["T66"] = Math.Round((fgen.make_double(dt.Rows[i]["T3"].ToString().Trim())) / 1000 * 4 * fgen.make_double(dt.Rows[i]["T65"].ToString()), 3).ToString();

            // txtRateTop.Text = Math.Round(fgen.return_double(txtWTop.Text.Trim()) * fgen.return_double(txtBfTopG.Text.Trim()), 3).ToString();
            dr1["T48"] = Math.Round(fgen.make_double(dr1["T16"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T85"].ToString().Trim()), 3).ToString();

            // txtRateMiddle.Text = Math.Round(fgen.return_double(txtWMiddle.Text.Trim()) * fgen.return_double(txtBfMiddleG.Text.Trim()), 3).ToString();
            dr1["T49"] = Math.Round(fgen.make_double(dr1["T17"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T86"].ToString().Trim()), 3).ToString();

            //  txtRateBottom.Text = Math.Round(fgen.return_double(txtWBottom.Text.Trim()) * fgen.return_double(txtBfBottomG.Text.Trim()), 3).ToString();
            dr1["T50"] = Math.Round(fgen.make_double(dr1["T18"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T87"].ToString().Trim().Trim()), 3).ToString();

            //txtRateFlute.Text = Math.Round(fgen.return_double(txtWFlute.Text.Trim()) * fgen.return_double(txtBfFluteG.Text.Trim()), 3).ToString();
            dr1["T51"] = Math.Round(fgen.make_double(dt.Rows[i]["T19"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T88"].ToString().Trim()), 3).ToString();

            // txtMaterial.Text = Math.Round(fgen.return_double(txtRateTop.Text.Trim()) + fgen.return_double(txtRateMiddle.Text.Trim()) + fgen.return_double(txtRateBottom.Text.Trim()) + fgen.return_double(txtRateFlute.Text.Trim()), 3).ToString();
            dr1["T52"] = Math.Round(fgen.make_double(dr1["T48"].ToString().Trim()) + fgen.make_double(dr1["T49"].ToString().Trim()) + fgen.make_double(dr1["T50"].ToString().Trim()) + fgen.make_double(dr1["T51"].ToString().Trim()), 3).ToString();

            // if ((txtMinQty.Text != "") && (txtPurchase.Text != ""))
            if ((dt.Rows[i]["T37"].ToString().Trim() != "") && (dr1["T32"].ToString().Trim() != ""))
            {
                //if ((txtPurchase.Text == "NaN") || (txtPurchase.Text == "Infinity") || (txtMinQty.Text == "NaN") || (txtMinQty.Text == "Infinity"))
                //{
                //    txtPurchase.Text = "0";
                //    txtMin.Text = "0";
                //}
                // if (Convert.ToDecimal(txtPurchase.Text) <= Convert.ToDecimal(txtMinQty.Text))
                if (fgen.make_double(dr1["T32"].ToString().Trim()) <= fgen.make_double(dt.Rows[i]["T37"].ToString().Trim()))
                {
                    //  txtProcessRate.Text = Math.Round(Convert.ToDouble(txtMaterial.Text.Trim()) * (Convert.ToDouble(txtProcess.Text.Trim()) / 100) * Convert.ToDouble(txtMin.Text.Trim()) / Convert.ToDouble(txtPurchase.Text.Trim()), 3).ToString();
                    dr1["T54"] = Math.Round(fgen.make_double(dr1["T52"].ToString().Trim()) * (fgen.make_double(dt.Rows[i]["T53"].ToString().Trim()) / 100) * fgen.make_double(dr1["T31"].ToString().Trim()) / fgen.make_double(dr1["T32"].ToString().Trim()), 3).ToString();

                    //txtBoardRate.Text = Math.Round(fgen.return_double(txtSWeight.Text.Trim()) * fgen.return_double(txtBoard.Text.Trim()) * fgen.return_double(txtMin.Text.Trim()) / fgen.return_double(txtPurchase.Text.Trim()), 3).ToString();
                    dr1["T56"] = Math.Round(fgen.make_double(dr1["T20"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T55"].ToString().Trim()) * fgen.make_double(dr1["T31"].ToString().Trim()) / fgen.make_double(dr1["T32"].ToString().Trim()), 3).ToString();

                    //txtPrintingRate.Text = Math.Round(fgen.return_double(txtSheet.Text.Trim()) * fgen.return_double(txtPrinting.Text.Trim()) * fgen.return_double(txtMin.Text.Trim()) / fgen.return_double(txtPurchase.Text.Trim()), 3).ToString();
                    dr1["T58"] = Math.Round(fgen.make_double(dr1["T15"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T57"].ToString().Trim()) * fgen.make_double(dr1["T31"].ToString().Trim()) / fgen.make_double(dr1["T32"].ToString().Trim()), 3).ToString();

                }
                else
                {
                    // txtProcessRate.Text = Math.Round(fgen.return_double(txtMaterial.Text.Trim()) * (fgen.return_double(txtProcess.Text.Trim()) / 100) * fgen.return_double(txtMin.Text.Trim()) / fgen.return_double(txtMin.Text.Trim()), 3).ToString();
                    dr1["T54"] = Math.Round(fgen.make_double(dr1["T52"].ToString().Trim()) * (fgen.make_double(dt.Rows[i]["T53"].ToString().Trim()) / 100) * fgen.make_double(dr1["T31"].ToString().Trim()) / fgen.make_double(dr1["T31"].ToString().Trim()), 3).ToString();

                    // txtBoardRate.Text = Math.Round(fgen.return_double(txtSWeight.Text.Trim()) * fgen.return_double(txtBoard.Text.Trim()) * fgen.return_double(txtMin.Text.Trim()) / fgen.return_double(txtMin.Text.Trim()), 3).ToString();
                    dr1["T56"] = Math.Round(fgen.make_double(dr1["T20"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T56"].ToString().Trim()) * fgen.make_double(dr1["T31"].ToString().Trim()) / fgen.make_double(dr1["T31"].ToString().Trim()), 3).ToString();

                    //   txtPrintingRate.Text = Math.Round(fgen.return_double(txtSheet.Text.Trim()) * fgen.return_double(txtPrinting.Text.Trim()) * fgen.return_double(txtMin.Text.Trim()) / fgen.return_double(txtMin.Text.Trim()), 3).ToString();
                    dr1["T58"] = Math.Round(fgen.make_double(dr1["T15"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T57"].ToString().Trim()) * fgen.make_double(dr1["T31"].ToString().Trim()) / fgen.make_double(dr1["T31"].ToString().Trim()), 3).ToString();
                }

            }


            // txtProfitRate.Text = Math.Round((fgen.return_double(txtMaterial.Text.Trim()) + fgen.return_double(txtProcessRate.Text.Trim()) + fgen.return_double(txtBoardRate.Text.Trim()) + fgen.return_double(txtPrintingRate.Text.Trim()) + fgen.return_double(txtDieRate.Text.Trim()) + fgen.return_double(txtWaterRate.Text.Trim()) + fgen.return_double(txtStitchingRate.Text.Trim()) + fgen.return_double(txtTapingRate.Text.Trim()) + fgen.return_double(txtPacking.Text.Trim()) + fgen.return_double(txtAny.Text.Trim())) * (fgen.return_double(txtProfit.Text.Trim()) / 100), 3).ToString();
            dr1["T69"] = Math.Round((fgen.make_double(dr1["T52"].ToString().Trim()) + fgen.make_double(dr1["T54"].ToString().Trim()) + fgen.make_double(dr1["T56"].ToString().Trim()) + fgen.make_double(dr1["T58"].ToString().Trim()) + fgen.make_double(dr1["T62"].ToString().Trim()) + fgen.make_double(dr1["T60"].ToString().Trim()) + fgen.make_double(dr1["T64"].ToString().Trim()) + fgen.make_double(dr1["T66"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T67"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T28"].ToString().Trim())) * (fgen.make_double(dt.Rows[i]["T68"].ToString().Trim()) / 100), 3).ToString();

            // txtFreightRate.Text = Math.Round((fgen.return_double(txtMaterial.Text.Trim()) + fgen.return_double(txtProfitRate.Text.Trim()) + fgen.return_double(txtProcessRate.Text.Trim()) + fgen.return_double(txtBoardRate.Text.Trim()) + fgen.return_double(txtPrintingRate.Text.Trim()) + fgen.return_double(txtDieRate.Text.Trim()) + fgen.return_double(txtWaterRate.Text.Trim()) + fgen.return_double(txtStitchingRate.Text.Trim()) + fgen.return_double(txtTapingRate.Text.Trim()) + fgen.return_double(txtPacking.Text.Trim()) + fgen.return_double(txtAny.Text.Trim())) * (fgen.return_double(txtFreight.Text.Trim()) / 100), 3).ToString();
            dr1["T71"] = Math.Round((fgen.make_double(dr1["T52"].ToString().Trim()) + fgen.make_double(dr1["T69"].ToString().Trim()) + fgen.make_double(dr1["T54"].ToString().Trim()) + fgen.make_double(dr1["T56"].ToString().Trim()) + fgen.make_double(dr1["T58"].ToString().Trim()) + fgen.make_double(dr1["T62"].ToString().Trim()) + fgen.make_double(dr1["T60"].ToString().Trim()) + fgen.make_double(dr1["T64"].ToString().Trim()) + fgen.make_double(dr1["T66"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T67"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T28"].ToString().Trim())) * (fgen.make_double(dt.Rows[i]["T70"].ToString().Trim()) / 100), 3).ToString();

            // txtPymtRate.Text = Math.Round((fgen.return_double(txtMaterial.Text.Trim()) + fgen.return_double(txtProfitRate.Text.Trim()) + fgen.return_double(txtProcessRate.Text.Trim()) + fgen.return_double(txtBoardRate.Text.Trim()) + fgen.return_double(txtPrintingRate.Text.Trim()) + fgen.return_double(txtDieRate.Text.Trim()) + fgen.return_double(txtWaterRate.Text.Trim()) + fgen.return_double(txtStitchingRate.Text.Trim()) + fgen.return_double(txtTapingRate.Text.Trim()) + fgen.return_double(txtFreightRate.Text.Trim()) + fgen.return_double(txtPacking.Text.Trim()) + fgen.return_double(txtAny.Text.Trim())) * 2 / 100 * (fgen.return_double(txtPymt.Text.Trim())), 3).ToString();
            dr1["T73"] = Math.Round((fgen.make_double(dr1["T52"].ToString().Trim()) + fgen.make_double(dr1["T69"].ToString().Trim()) + fgen.make_double(dr1["T54"].ToString().Trim()) + fgen.make_double(dr1["T56"].ToString().Trim()) + fgen.make_double(dr1["T58"].ToString().Trim()) + fgen.make_double(dr1["T62"].ToString().Trim()) + fgen.make_double(dr1["T60"].ToString().Trim()) + fgen.make_double(dr1["T64"].ToString().Trim()) + fgen.make_double(dr1["T66"].ToString().Trim()) + fgen.make_double(dr1["T71"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T67"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["T28"].ToString().Trim())) * 2 / 100 * (fgen.make_double(dt.Rows[i]["T72"].ToString().Trim())), 3).ToString();

            //   txtBasic.Text = Math.Round(fgen.return_double(txtMaterial.Text.Trim()) + fgen.return_double(txtPymtRate.Text.Trim()) + (fgen.return_double(txtPacking.Text.Trim())) + fgen.return_double(txtAny.Text.Trim()) + fgen.return_double(txtProfitRate.Text.Trim()) + fgen.return_double(txtProcessRate.Text.Trim()) + fgen.return_double(txtBoardRate.Text.Trim()) + fgen.return_double(txtPrintingRate.Text.Trim()) + fgen.return_double(txtDieRate.Text.Trim()) + fgen.return_double(txtWaterRate.Text.Trim()) + fgen.return_double(txtStitchingRate.Text.Trim()) + fgen.return_double(txtTapingRate.Text.Trim()) + fgen.return_double(txtFreightRate.Text.Trim()), 3).ToString();
            dr1["T29"] = Math.Round(fgen.make_double(dr1["T52"].ToString().Trim()) + fgen.make_double(dr1["T73"].ToString().Trim()) + (fgen.make_double(dt.Rows[i]["T67"].ToString().Trim())) + fgen.make_double(dt.Rows[i]["T28"].ToString().Trim()) + fgen.make_double(dr1["T69"].ToString().Trim()) + fgen.make_double(dr1["T54"].ToString().Trim()) + fgen.make_double(dr1["T56"].ToString().Trim()) + fgen.make_double(dr1["T58"].ToString().Trim()) + fgen.make_double(dr1["T62"].ToString().Trim()) + fgen.make_double(dr1["T60"].ToString().Trim()) + fgen.make_double(dr1["T64"].ToString().Trim()) + fgen.make_double(dr1["T66"].ToString().Trim()) + fgen.make_double(dr1["T71"].ToString().Trim()), 3).ToString();

            // txtExciseRate.Text = Math.Round((fgen.return_double(txtBasic.Text.Trim()) * fgen.return_double(txtExcise.Text.Trim())) / 100, 3).ToString();
            dr1["T75"] = Math.Round((fgen.make_double(dr1["T29"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["T74"].ToString().Trim())) / 100, 3).ToString();

            //  txtSalesRate.Text = Math.Round((fgen.return_double(txtBasic.Text.Trim()) + fgen.return_double(txtExciseRate.Text.Trim())) * (fgen.return_double(txtSales.Text.Trim()) / 100), 3).ToString();
            dr1["T77"] = Math.Round((fgen.make_double(dr1["T29"].ToString().Trim()) + fgen.make_double(dr1["T75"].ToString().Trim())) * (fgen.make_double(dt.Rows[i]["T76"].ToString().Trim()) / 100), 3).ToString();

            // txtTotal.Text = Math.Round(fgen.return_double(txtSalesRate.Text.Trim()) + fgen.return_double(txtBasic.Text.Trim()) + fgen.return_double(txtExciseRate.Text.Trim()), 3).ToString();
            dr1["T30"] = Math.Round(fgen.make_double(dr1["T77"].ToString().Trim()) + fgen.make_double(dr1["T29"].ToString().Trim()) + fgen.make_double(dr1["T75"].ToString().Trim()), 3).ToString();
            #endregion
            dt1.Rows.Add(dr1);
        }
        #endregion

        // UPDATING THE ALREADY SAVED COSTING WITH ANOTHER TYPE IN SAME TABLE FOR RECOVERY PURPOSE BUT IT HAS UPDATED ITEM RATES WITH TYPE='CU'
        #region Costing
        oDS = new DataSet();
        oDS = fgen.fill_schema(frm_qstr,frm_cocd, "SOMAS_ANX");
        dbrow = null;
        vip = "DELETE FROM SOMAS_ANX WHERE  BRANCHCD='" + frm_mbr + "' AND TYPE='CU' AND TRIM(ACODE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') IN (" + btnval.Trim().TrimStart(',') + ")";
        fgen.execute_cmd(frm_qstr,frm_cocd, vip);
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            mq0 = "UPDATE SOMAS_ANX SET TYPE='CU' WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND VCHNUM='" + dt1.Rows[i]["VCHNUM"].ToString().Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + Convert.ToDateTime(dt1.Rows[i]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy") + "' AND ACODE='" + dt1.Rows[i]["ACODE"].ToString().Trim() + "'";
            fgen.execute_cmd(frm_qstr,frm_cocd, mq0);
            //vip = "UPDATE SOMAS_ANX SET BRANCHCD='DD' WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='CM' AND VCHNUM='" + dt1.Rows[i]["VCHNUM"].ToString().Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + Convert.ToDateTime(dt1.Rows[i]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy") + "' AND ACODE='" + dt1.Rows[i]["ACODE"].ToString().Trim() + "'";
            //fgen.execute_cmd(frm_cocd, vip);
            dbrow = oDS.Tables[0].NewRow();
            dbrow["BRANCHCD"] = dt1.Rows[i]["BRANCHCD"].ToString().Trim();
            dbrow["TYPE"] = dt1.Rows[i]["TYPE"].ToString().Trim();
            dbrow["VCHNUM"] = dt1.Rows[i]["VCHNUM"].ToString().Trim();
            dbrow["vchdate"] = dt1.Rows[i]["VCHDATE"].ToString().Trim();
            dbrow["Acode"] = dt1.Rows[i]["ACODE"].ToString().Trim();
            dbrow["Icode"] = dt1.Rows[i]["ICODE"].ToString().Trim();
            dbrow["T1"] = dt1.Rows[i]["T1"].ToString().Trim().ToUpper();//LENGTH
            dbrow["T2"] = dt1.Rows[i]["T2"].ToString().Trim().ToUpper();//WIdt1H
            dbrow["T3"] = dt1.Rows[i]["T3"].ToString().Trim().ToUpper();//HEIGHT
            dbrow["T4"] = dt1.Rows[i]["T4"].ToString().Trim().ToUpper();//CMD
            dbrow["T5"] = dt1.Rows[i]["T5"].ToString().Trim().ToUpper();//DOUBLE
            dbrow["T6"] = dt1.Rows[i]["T6"].ToString().Trim().ToUpper();//TOP
            dbrow["T7"] = dt1.Rows[i]["T7"].ToString().Trim().ToUpper();//FLUTEB
            dbrow["T8"] = dt1.Rows[i]["T8"].ToString().Trim().ToUpper();//MIDDLE
            dbrow["T9"] = dt1.Rows[i]["T9"].ToString().Trim().ToUpper();//FLUTEA
            dbrow["T10"] = dt1.Rows[i]["T10"].ToString().Trim().ToUpper();//BOTTOM
            dbrow["T11"] = dt1.Rows[i]["T11"].ToString().Trim().ToUpper();//REEL
            dbrow["T12"] = dt1.Rows[i]["T12"].ToString().Trim().ToUpper();//CUT
            dbrow["T13"] = dt1.Rows[i]["T13"].ToString().Trim().ToUpper();//DECKLE
            dbrow["T14"] = dt1.Rows[i]["T14"].ToString().Trim().ToUpper();//CUTSIZE
            dbrow["T15"] = dt1.Rows[i]["T15"].ToString().Trim().ToUpper();//SHEET
            dbrow["T16"] = dt1.Rows[i]["T16"].ToString().Trim().ToUpper();//WTOP
            dbrow["T17"] = dt1.Rows[i]["T17"].ToString().Trim().ToUpper();//WMIDDLE
            dbrow["T18"] = dt1.Rows[i]["T18"].ToString().Trim().ToUpper();//WBOTTOM
            dbrow["T19"] = dt1.Rows[i]["T19"].ToString().Trim().ToUpper();//WFLUTE
            dbrow["T20"] = dt1.Rows[i]["T20"].ToString().Trim().ToUpper();//SWEIGHT
            dbrow["T21"] = dt1.Rows[i]["T21"].ToString().Trim().ToUpper();//BS
            dbrow["T22"] = dt1.Rows[i]["T22"].ToString().Trim().ToUpper();//GSM
            dbrow["T23"] = dt1.Rows[i]["T23"].ToString().Trim().ToUpper();//ECT
            dbrow["T24"] = dt1.Rows[i]["T24"].ToString().Trim().ToUpper();//BCT
            dbrow["T25"] = dt1.Rows[i]["T25"].ToString().Trim().ToUpper();//COBB
            dbrow["T26"] = dt1.Rows[i]["T26"].ToString().Trim().ToUpper();//MOISTURE
            dbrow["T28"] = dt1.Rows[i]["T28"].ToString().Trim().ToUpper();//ANY
            dbrow["T29"] = dt1.Rows[i]["T29"].ToString().Trim().ToUpper();//BASIC
            dbrow["T30"] = dt1.Rows[i]["T30"].ToString().Trim().ToUpper();//TOTAL
            dbrow["T31"] = dt1.Rows[i]["T31"].ToString().Trim().ToUpper();//MIN
            dbrow["T32"] = dt1.Rows[i]["T32"].ToString().Trim().ToUpper();//PURCHASE
            dbrow["T37"] = dt1.Rows[i]["T37"].ToString().Trim().ToUpper();//MIN QTY
            dbrow["T39"] = dt1.Rows[i]["T39"].ToString().Trim().ToUpper();//CMB TYPES
            dbrow["T48"] = dt1.Rows[i]["T48"].ToString().Trim().ToUpper();// RATE TOP
            dbrow["T49"] = dt1.Rows[i]["T49"].ToString().Trim().ToUpper();// RATE MIDDLE
            dbrow["T50"] = dt1.Rows[i]["T50"].ToString().Trim().ToUpper();// RATE BOTTOM
            dbrow["T51"] = dt1.Rows[i]["T51"].ToString().Trim().ToUpper();// RATE FLUTE
            dbrow["T52"] = dt1.Rows[i]["T52"].ToString().Trim().ToUpper();// MATERIAL
            dbrow["T53"] = dt1.Rows[i]["T53"].ToString().Trim().ToUpper();// PROCESS
            dbrow["T54"] = dt1.Rows[i]["T54"].ToString().Trim().ToUpper();// PROCESS RATE
            dbrow["T55"] = dt1.Rows[i]["T55"].ToString().Trim().ToUpper();// BOARD
            dbrow["T56"] = dt1.Rows[i]["T56"].ToString().Trim().ToUpper();// BOARD RATE
            dbrow["T57"] = dt1.Rows[i]["T57"].ToString().Trim().ToUpper();// PRINTING
            dbrow["T58"] = dt1.Rows[i]["T58"].ToString().Trim().ToUpper();//PRINTING RATE
            dbrow["T59"] = dt1.Rows[i]["T59"].ToString().Trim().ToUpper();//WATER
            dbrow["T60"] = dt1.Rows[i]["T60"].ToString().Trim().ToUpper();//WATER RATE
            dbrow["T61"] = dt1.Rows[i]["T61"].ToString().Trim().ToUpper();// DIE
            dbrow["T62"] = dt1.Rows[i]["T62"].ToString().Trim().ToUpper();//DIE RATE
            dbrow["T63"] = dt1.Rows[i]["T63"].ToString().Trim().ToUpper();//STITCHING
            dbrow["T64"] = dt1.Rows[i]["T64"].ToString().Trim().ToUpper();//STITCHING RATE
            dbrow["T65"] = dt1.Rows[i]["T65"].ToString().Trim().ToUpper();//TAPING
            dbrow["T66"] = dt1.Rows[i]["T66"].ToString().Trim().ToUpper();//TAPING RATE
            dbrow["T67"] = dt1.Rows[i]["T67"].ToString().Trim().ToUpper();//PACKING
            dbrow["T68"] = dt1.Rows[i]["T68"].ToString().Trim().ToUpper();//PROFIT
            dbrow["T69"] = dt1.Rows[i]["T69"].ToString().Trim().ToUpper();//PROFIT RATE
            dbrow["T70"] = dt1.Rows[i]["T70"].ToString().Trim().ToUpper();//FRIGHT
            dbrow["T71"] = dt1.Rows[i]["T71"].ToString().Trim().ToUpper();//FRIGHT RATE
            dbrow["T72"] = dt1.Rows[i]["T72"].ToString().Trim().ToUpper();//PAYMENT
            dbrow["T73"] = dt1.Rows[i]["T73"].ToString().Trim().ToUpper();//PAYMEN RATE
            dbrow["T74"] = dt1.Rows[i]["T74"].ToString().Trim().ToUpper();//EXCISE
            dbrow["T75"] = dt1.Rows[i]["T75"].ToString().Trim().ToUpper();//EXCISE RATE
            dbrow["T76"] = dt1.Rows[i]["T76"].ToString().Trim().ToUpper();//SALES
            dbrow["T77"] = dt1.Rows[i]["T77"].ToString().Trim().ToUpper();//SALES RATE
            dbrow["T78"] = dt1.Rows[i]["T78"].ToString().Trim(); //GRD ICODE
            if (dt1.Rows[i]["T78"].ToString().Trim() != "")
            {
                dbrow["T140"] = dt1.Rows[i]["T140"].ToString().Trim();
            }
            else
            {
                dbrow["T140"] = dt1.Rows[i]["T140"].ToString().Trim();
            }
            dbrow["T80"] = dt1.Rows[i]["T80"].ToString().Trim(); //GRD QTY
            dbrow["T81"] = dt1.Rows[i]["T81"].ToString().Trim();//GRD RATE
            dbrow["T82"] = dt1.Rows[i]["T82"].ToString().Trim().ToUpper();// GRD TOTAL
            if (dt1.Rows[i]["T84"].ToString().Trim() == "MANUAL")
            {
                dbrow["T84"] = "MANUAL";
                if (dt1.Rows[i]["T83"].ToString().Trim().Length > 30)
                {
                    dbrow["T83"] = dt1.Rows[i]["T83"].ToString().Trim().Substring(0, 29).ToUpper();
                }

                else
                {
                    dbrow["T83"] = dt1.Rows[i]["T83"].ToString().Trim().ToUpper();
                }
            }
            dbrow["T40"] = dt1.Rows[i]["T40"].ToString().Trim().ToUpper();//BF TOP
            dbrow["T41"] = dt1.Rows[i]["T41"].ToString().Trim().ToUpper();//BFTOP RATE
            dbrow["T85"] = dt1.Rows[i]["T85"].ToString().Trim().ToUpper();//BF TOPG
            dbrow["T42"] = dt1.Rows[i]["T42"].ToString().Trim().ToUpper();//BF MIDDLE
            dbrow["T43"] = dt1.Rows[i]["T43"].ToString().Trim().ToUpper();//BF MIDDLE RATE 
            dbrow["T86"] = dt1.Rows[i]["T86"].ToString().Trim().ToUpper();//BF MIDDLEG
            dbrow["T44"] = dt1.Rows[i]["T44"].ToString().Trim().ToUpper();//BF BOTTOM
            dbrow["T45"] = dt1.Rows[i]["T45"].ToString().Trim().ToUpper();//BF BOTOM RATE
            dbrow["T87"] = dt1.Rows[i]["T87"].ToString().Trim().ToUpper();//BF BOTTOMG
            dbrow["T46"] = dt1.Rows[i]["T46"].ToString().Trim().ToUpper();// BF FLUTES 
            dbrow["T47"] = dt1.Rows[i]["T47"].ToString().Trim().ToUpper();//BF FLUTES RATE
            dbrow["T88"] = dt1.Rows[i]["T88"].ToString().Trim().ToUpper();//BF FLUTEG
            dbrow["T139"] = "UPDATED COSTING";
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
                dbrow["edt_by"] = frm_uname;
                dbrow["edt_dt"] = System.DateTime.Now;
            }
            oDS.Tables[0].Rows.Add(dbrow);
        }
        fgen.save_data(frm_qstr, frm_cocd, oDS, "SOMAS_ANX");
        //fgen.execute_cmd(frm_cocd, "DELETE FROM SOMAS_ANX WHERE BRANCHCD='DD' AND TYPE='CM'");
        #endregion
       
        #endregion

        #region Updated Costing Sheet are saving in new table
        #endregion
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        // for doing save action 
        if (hffield.Value == "List_E")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            frm_sql = "Select DISTINCT S.VCHNUM,TO_CHAR(S.VCHDATE,'DD/MM/YYYY') AS VOCHER_DATE, F.ANAME AS PARTYNAME,S.ACODE AS PARTYCODE,S.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,S.COL16 AS NEW_RATE FROM  FAMST F,SCRATCH S , ITEM I WHERE TRIM(I.ICODE) =TRIM(S.ICODE) AND TRIM(F.ACODE)=TRIM(S.ACODE) and S.branchcd='" + frm_mbr + "' and S.type='" + frm_vty.Trim() + "' and s.col19='UPDATED RATE' order by S.vchnum DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
            fgen.Fn_open_rptlevel("-", frm_qstr);
        }
        else if (hffield.Value == "CMD_REP1")
        {
            frm_sql = "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum");
            fgen.Fn_open_rptlevel("Stage Entry List", frm_qstr);
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
                        save_data();
                        fgen.msg("-", "AMSG", "Rates are Updated Successfully.");
                        string S = "delete from " + frm_tabname + " where branchcd='DD' and type='CM'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and type='CM'");
                        ViewState["sg1"] = null; sg1.DataSource = null; sg1.DataBind();
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl();
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

                fgen.execute_cmd(frm_qstr,frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||TRIM(A.ACODE)='" + popselected.Value + "'");
                fgen.execute_cmd(frm_qstr,frm_cocd, "delete from wsr_Ctrl a where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy')='" + popselected.Value.ToUpper().Substring(20, 6) + "'");
                string A = popselected.Value.Substring(4, 6);
                fgen.save_info(frm_qstr,frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), System.DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, frm_vty, "Mater Rate Updation Deleted");
                fgen.msg("-", "AMSG", "Details are deleted for Doc No." + popselected.Value.Substring(4, 6) + "");
                // clearctrl(); 
                fgen.ResetForm(this.Controls);

               // fgen.execute_cmd(frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
               // fgen.save_info(frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), popselected.Value.Substring(10, 10), frm_uname, popselected.Value.Substring(2, 2), "Item Routing DELETED");
               // fgen.msg("-", "AMSG", "Details are deleted for Item Routing Entry " + popselected.Value.Substring(4, 6) + "");
               //// clearctrl(); 
               // fgen.ResetForm(this.Controls);
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
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    fgen.Fn_open_prddmp1("Select Date Range for List Of Stage Routing", frm_qstr);
                    break;

                case "New":
                    clearctrl();
                    hffield.Value = "New_E";
                    set_Val();
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.msg("-", "CMSG", "Do you want to see all parties or (No for selected parties)");
                    break;
                case "New_E":
                    col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    if (col1 == "Y")
                    {
                        create_tab();
                        frm_sql = "SELECT DISTINCT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR, A.ICODE,I.INAME,A.COL16,A.ACODE,F.ANAME FROM SCRATCH A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "'  AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " ORDER BY A.ACODE";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr,frm_cocd, frm_sql);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["Aname"] = dt.Rows[i]["Aname"].ToString();
                            dr1["Acode"] = dt.Rows[i]["Acode"].ToString();
                            dr1["Col16"] = dt.Rows[i]["COL16"].ToString();
                            dr1["Col17"] = "0";
                            dr1["Hidden"] = dt.Rows[i]["Fstr"].ToString();
                            dt1.Rows.Add(dr1);
                        }
                        // add_blankrows();
                        ViewState["sg1"] = dt1;
                        sg1.DataSource = dt1;
                        sg1.DataBind();
                        dt.Dispose(); dt1.Dispose();
                        disablectrl();
                        fgen.EnableForm(this.Controls);
                    }
                    else
                    {
                        hffield.Value = "New_E1";
                        frm_sql = "SELECT DISTINCT A.ACODE AS FSTR,A.ACODE AS PARTY_CODE,F.ANAME AS PARTY_NAME FROM SCRATCH A,FAMST F WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " ORDER BY A.ACODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
                        fgen.Fn_open_mseek("Select Party", frm_qstr);
                    }
                    break;
                case "New_E1":
                    create_tab();
                    frm_sql = "SELECT DISTINCT TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR, A.ICODE,I.INAME,A.COL16,A.ACODE,F.ANAME FROM SCRATCH A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "'  AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " AND A.ACODE IN (" + col1 + ") ORDER BY A.ACODE,A.ICODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr,frm_cocd, frm_sql);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["srno"] = dt1.Rows.Count + 1;
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                        dr1["Aname"] = dt.Rows[i]["Aname"].ToString();
                        dr1["Acode"] = dt.Rows[i]["Acode"].ToString();
                        dr1["Col16"] = dt.Rows[i]["COL16"].ToString();
                        dr1["Col17"] = "0";
                        dr1["Hidden"] = dt.Rows[i]["Fstr"].ToString();
                        dt1.Rows.Add(dr1);
                    }
                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    dt.Dispose(); dt1.Dispose();
                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    break;
                case "Del":
                    clearctrl();
                    set_Val();
                    hffield.Value = "Del_E";
                    //popselected.Value = col1;
                    //frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    frm_vty = col1;
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;
                case "Del_E":
                    clearctrl();
                    popselected.Value = col1;
                    //fgen.execute_cmd(frm_cocd, "delete from " + frm_tabname + " a where TYPE='"+frm_vty+"' AND branchcd||type||trim(VCHNUM)||to_char(VCHDATE,'dd/mm/yyyy')='" + popselected.Value + "'");
                    hffield.Value = "D";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    break;
                case "Edit":
                    // this is after type selection 
                    clearctrl();
                    set_Val();
                    hffield.Value = "Edit_E";

                    // txtvty.Text = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    frm_vty = col1;
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
                    break;
                case "Edit_E":
                    popselected.Value = col1;
                    frm_sql = "SELECT S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy')||TRIM(S.ACODE)||TRIM(S.ICODE) AS FSTR,I.INAME, S.* FROM SCRATCH S, ITEM I WHERE TRIM(I.ICODE)=TRIM(S.ICODE) AND S.TYPE='" + frm_vty + "' AND  S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy')||TRIM(S.ACODE)= '" + col1.Trim() + "'";
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr,frm_cocd, frm_sql);
                    // Filing textbox of the form
                    if (dtb.Rows.Count > 0)
                    {
                        ViewState["VCHNUM"] = dtb.Rows[0]["VCHNUM"].ToString();
                        ViewState["ent_by"] = dtb.Rows[0]["ent_by"].ToString();
                        ViewState["ent_Dt"] = dtb.Rows[0]["ent_dt"].ToString();

                        //txtSCode .Text = dtb.Rows[0]["ICODE"].ToString().Trim();
                        // DataTable dtIname=fgen.getdata(frm_cocd,"Select Iname  From Item Where Icode='"+txtSCode.Text+"'");
                        // txtSName.Text = dtIname.Rows[0]["Iname"].ToString().Trim();
                        //txtPCode.Text = dtb.Rows[0]["ACODE"].ToString().Trim();
                        //DataTable dtAname = fgen.getdata(frm_cocd, "Select Aname From Famst Where Acode='" + txtPCode.Text + "'");
                        //txtParty.Text = dtAname.Rows[0]["Aname"].ToString().Trim();
                        ////txtPaper .Text = dtb.Rows[0]["COL1"].ToString().Trim();
                        ////txtImp.Text = dtb.Rows[0]["COL2"].ToString().Trim();
                        //txtProcess.Text = dtb.Rows[0]["COL3"].ToString().Trim();
                        //txtBoard.Text = dtb.Rows[0]["COL4"].ToString().Trim();
                        //txtPrinting.Text = dtb.Rows[0]["COL5"].ToString().Trim();
                        //txtWater.Text = dtb.Rows[0]["COL6"].ToString().Trim();
                        //txtDie.Text = dtb.Rows[0]["COL7"].ToString().Trim();
                        //txtStitching.Text = dtb.Rows[0]["COL8"].ToString().Trim();
                        //txtTaping.Text = dtb.Rows[0]["COL9"].ToString().Trim();
                        //txtPacking.Text = dtb.Rows[0]["COL10"].ToString().Trim();
                        //txtProfit.Text = dtb.Rows[0]["COL11"].ToString().Trim();
                        //txtFreight.Text = dtb.Rows[0]["COL12"].ToString().Trim();
                        //txtPymt.Text = dtb.Rows[0]["COL13"].ToString().Trim();
                        //txtExcise.Text = dtb.Rows[0]["COL14"].ToString().Trim();
                        //txtSales.Text = dtb.Rows[0]["COL15"].ToString().Trim();
                        //txtMinimumQty.Text = dtb.Rows[0]["Col17"].ToString();
                        create_tab();
                        foreach (DataRow dr in dtb.Rows)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dr["srno"].ToString();
                            dr1["icode"] = dr["icode"].ToString();
                            dr1["iname"] = dr["iname"].ToString();
                            dr1["CoL16"] = dr["CoL16"];
                            dr1["ACODE"] = dr["ACODE"].ToString();
                            dr1["ANAME"] = fgen.seek_iname(frm_qstr,frm_cocd, "SELECT DISTINCT ANAME FROM FAMST WHERE ACODE='" + dr["ACODE"].ToString().Trim() + "'", "ANAME");
                            dr1["COL17"] = dr["COL16"].ToString();
                            dr1["HIDDEN"] = dr["FSTR"];
                            dt1.Rows.Add(dr1);
                        }

                        //add_blankrows();

                        ViewState["sg1"] = dt1;
                        sg1.DataSource = dt1;
                        sg1.DataBind();
                    }
                    dt1 = new DataTable();

                    fgen.EnableForm(this.Controls);
                    clearctrl(); disablectrl();
                    dtb.Dispose();
                    dt1.Dispose();
                    edmode.Value = "Y";
                    btnParty.Disabled = true;
                    break;
                case "Party":
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr,frm_cocd, "select aname from famst where trim(acode)='" + col1 + "'");
                    txtPCode.Text = col1;
                    txtParty.Text = dt.Rows[0]["aname"].ToString();
                    if (sg1.Rows.Count > 0)
                    {
                        ((ImageButton)(sg1.Rows[0].FindControl("btnadd"))).Focus();
                    }
                    dt.Dispose();
                    break;
                case "Print":

                    set_Val();
                    hffield.Value = "Print_E";
                    frm_vty = col1;
                    // txtvty.Text = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("-", frm_qstr);
                    break;
                case "Print_E":
                    //  frm_sql = "SELECT S.VCHNUM AS DOCNO,TO_CHAR(S.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, S.ACODE AS CODE,F.ANAME AS PARTY_NAME,S.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,S.COL17 AS MINIMUM_ORDER_QTY , S.COL16 AS ITEM_RATE ,S.COL3 AS PROCESS_WASTAGE,S.COL4 AS BOARD_MAKING_CHARGES,S.COL5 AS PRINTING_SLOTTING,S.COL6 AS WATER_RESISTANCE_COATING,S.COL7 AS DIE_CUTTING,S.COL8 AS STITCHING_FLAP_COSTING ,S.COL9 AS TAPING_BINDING_CLOTH,S.COL10 AS PACKING,S.COL11 AS PROFIT_MARGIN,S.COL12 AS FREIGHT,S.COL13 AS PAYMENT_TERMS,S.COL14 AS EXCISES,S.COL15 AS SALES_Tax FROM SCRATCH S, ITEM I ,FAMST F WHERE TRIM(S.ICODE)=TRIM(I.ICODE)  AND TRIM(S.ACODE)=TRIM(F.ACODE) AND S.TYPE='" + frm_vty + "' AND S.BRANCHCD||S.TYPE||TRIM(S.VCHNUM)||TO_CHAr(S.VCHDATE,'DD/MM/YYYY') in (" + col1 + ") ORDER BY VCHNUM";
                    //    dt = fgen.getdata(frm_cocd,frm_sql);
                    //    fgen.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
                    //     fgen.Fn_open_rptlevel(frm_cocd, frm_qstr);

                    //dt.Dispose();   
                    break;
                case "List":
                    hffield.Value = "List_E";
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1.Trim());
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "Add":
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
                            dr1["Col16"] = ((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text.Trim();
                            dr1["Col17"] = ((TextBox)sg1.Rows[i].FindControl("txtCol17")).Text.Trim();
                            TextBox Qty = (TextBox)(sg1.Rows[i].FindControl("txtCol17"));
                            Qty.Focus();
                            dt1.Rows.Add(dr1);
                        }
                        if (col1.Trim().Length == 8) frm_sql = "SELECT DISTINCT A.ICODE,ACODE,COL16 FROM SCRATCH A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND  TYPE='CM' AND TRIM(A.BRANCHCD)||TRIM(A.ACODE)||trim(A.icode) in (" + col1 + ")";
                        else frm_sql = "SELECT DISTINCT A.ICODE,I.INAME,COL16 FROM SCRATCH A,ITEM I WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND  TYPE='CM' AND TRIM(A.BRANCHCD)||TRIM(A.ACODE)||trim(A.icode) in (" + col1 + ")";

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr,frm_cocd, frm_sql);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                            // dr1["Aname"] = dt.Rows[i]["Aname"].ToString();
                            dr1["Col16"] = dt.Rows[i]["COL16"].ToString();
                            dr1["Col17"] = "0";
                            dt1.Rows.Add(dr1);
                        }
                    }
                    add_blankrows();
                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    dt.Dispose(); dt1.Dispose();
                    break;
                case "Add_E":
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;


                    break;
                case "Row_Edit":
                    // sg1.Rows[Convert.ToInt32(hf1.Text)].Cells[3].Text = col1;
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr,frm_cocd, "select Type1,Name,Type1 as Code from Type where id='K' and trim(type1)='" + col1 + "'");
                    if (dtb.Rows.Count > 0)
                    {
                        //sg1.Rows[Convert.ToInt32(hf1.Text)].Cells[4].Text = dtb.Rows[0]["name"].ToString().Trim();
                        //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Text)].FindControl("txtfld1")).Focus();
                    } //Grid_Col_Tot();
                    break;
                case "Rmv":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {

                        dtb = new DataTable();
                        dtb = (DataTable)ViewState["sg1"];
                        dtb.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = dtb;
                        sg1.DataSource = dtb;
                        sg1.DataBind();
                        dtb.Dispose();
                    } //Grid_Col_Tot();
                    break;

                case "State":
                    //frm_sql = "Select type1 as fstr,name,type1 as Code  From Typegrp Where ID='ES' Order By Name";
                    popselected.Value = col1;
                    frm_sql = "select type1,name from Typegrp Where ID='ES' And type1='" + col1.Trim() + "'";
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr,frm_cocd, frm_sql);
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
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        DataTable dt = fgen.getdata(frm_qstr,frm_cocd, "Select Icode AS Fstr,Iname As SubGroupName,Icode As Code,Icat From Item Where Length(Trim(Icode))=4 AND ICODE LIKE '07%'");
       
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "Rmv":
                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString(); hffield.Value = "Rmv";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove this item from list");
                }
                break;
            case "Add":
                if (txtPCode .Text == "" || txtParty .Text == "")
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
                    //this.cal();
                }
                break;
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Cells[10].Style["display"] = "none";
            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[10].Style["display"] = "none";
            e.Row.Cells[9].Style["display"] = "none";
            sg1.HeaderRow.Cells[9].Style["display"] = "none";
        }
    }
    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("Icode", typeof(string)));
        dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
        dt1.Columns.Add(new DataColumn("Acode", typeof(string)));
        dt1.Columns.Add(new DataColumn("Aname", typeof(string)));
        dt1.Columns.Add(new DataColumn("Col17", typeof(string)));
        dt1.Columns.Add(new DataColumn("Col16", typeof(string)));
        dt1.Columns.Add(new DataColumn("Hidden", typeof(string)));
        dt1.Columns.Add(new DataColumn("Costing", typeof(string)));
    }
    public void add_blankrows()
    {
        dr1 = dt1.NewRow();
        dr1["Srno"] = dt1.Rows.Count + 1;
        dr1["icode"] = "-";
        dr1["iname"] = "-";
        dr1["Acode"] = "-";
        dr1["Aname"] = "-";
        dr1["Col17"] = "0";
        dr1["Col16"] = "0";
        dr1["Costing"] = "0";
        dr1["Hidden"] = "-";
        dt1.Rows.Add(dr1);
    }
  
}