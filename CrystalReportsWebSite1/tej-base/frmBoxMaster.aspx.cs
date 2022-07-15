using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class frmBoxMaster : System.Web.UI.Page
{
    DataTable dtb, dtb1;
    DataRow dbrow, dr1;
    DataSet oDS;
    DataTable dt, dt1; DataRow oporow;
    string btnval, col1, col2, col3, fill_Date, vip = "",Checked_ok;
    string mq0, pk_error = "Y", chk_rights = "N", tmp_var, frm_formID, Squery;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_datrange, frm_UserID;
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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    //  frm_datrange = fgen.Fn_Get_Mvar(frm_qstr, "U_prdRANGE");
                    frm_datrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");

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
                //set_Val();


            }

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
        //cmdlist.Disabled = true;
        btnParty.Disabled = false;

        btncancel.Visible = true;
        cmdexit.Visible = false;

        //btnhideF.Enabled = true;
        //btnhideF_s.Enabled = true;
    }

    public void enablectrl()
    {
        // for enable/disable some variables

        cmdnew.Disabled = false;
        cmdedit.Disabled = false;
        btncancel.Visible = false;
        cmddel.Disabled = false;
        btnParty.Disabled = true;
        cmdexit.Visible = true;
        btnsave.Disabled = true;
        //btnhideF.Enabled = true;
        //btnhideF_s.Enabled = true;

        cmdprint.Disabled = false;
        //cmdlist.Disabled = false;


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

                    //frm_sql = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Route_no,to_char(a.vchdate,'dd/mm/yyyy') as Route_Dt,b.IName as Item_Name,a.Type as s_code,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                    frm_sql = "Select DISTINCT S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy') AS FSTR,S.VCHNUM,F.ANAME AS PARTYNAME,S.ACODE AS CODE FROM SCRATCH S , FAMST F WHERE  TRIM(F.ACODE)=TRIM(S.ACODE) AND S.TYPE='" + frm_vty + "' ORDER BY S.VCHNUM  DESC";

                if (btnval == "New" || btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")

                    frm_sql = "select 'CM' AS FSTR,'Party Master' as heading,'CM' as type from dual ";
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
                        frm_sql = "select icode as fstr,iname as Item,icode as erp_code from item where trim(icode)like '07%' and trim(icode) not in (" + col2 + ") and length(trim(icode))=4 order by iname";
                        //frm_sql = "select icode as fstr,iname as Item,icode as erp_code from item where substr(icode,1,1) in ('1','2','3','4','5','6','7','8') and trim(icode) not in (" + col2 + ") and length(trim(icode))=8 order by iname";
                    }
                    else frm_sql = "select icode as fstr,iname as Item,icode as erp_code,Irate,Unit,Icat from item where trim(icode)like '07%' and length(trim(icode))=4 order by iname";
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
            if (txtPCode.Text == "0")
            {
                fgen.msg("-", "AMSG", "Please Select  Party !! ");
                return;
            }

            if (edmode.Value == "")
            {
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr,frm_cocd, "Select a.aname from famst a,scratch b where b.branchcd='" + frm_mbr + "' and b.type='CM' and trim(a.acode)=trim(b.acode) and trim(b.acode)='" + txtPCode.Text + "' ");
                if (dt.Rows.Count > 0)
                {
                    fgen.msg("-", "AMSG", "Prices for this party already exists.Please select any other party");
                    return;
                }
            }
            if (Convert.ToDouble(txtProcess.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Process Wastage Rate can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtPacking.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Packing Rate can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtProfit.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Profit Margin Rate can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtFreight.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Freight Rate can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtPymt.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Payment Terms  can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtExcise.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Excise Rate can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtSales.Text) > 100)
            {
                fgen.msg("-", "AMSG", "Sales Tax Rate can not be greater than 100"); return;
            }
            else if (Convert.ToDouble(txtMinimumQty.Text) == 0 || (txtMinimumQty.Text == ""))
            {
                fgen.msg("-", "AMSG", "Please Fill Minimum Qty."); return;
            }
            // int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
            //if (dhd == 0)
            //{ fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }
            //if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fgen.Fn_Get_Mvar(frm_qstr, "U_CDT1")) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(fgen.Fn_Get_Mvar(frm_qstr, "U_CDT2")))
            //{ fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only"); txtvchdate.Focus(); return; }
            if (sg1.Rows.Count <= 1)
            { fgen.msg("-", "AMSG", "No Item to Save!!'13'Please Select Some item first"); return; }

            for (int i = 0; i < sg1.Rows.Count - 1; i++)
            {
                TextBox t = (TextBox)(sg1.Rows[i].FindControl("txtCol16"));
                string a = t.Text;
                if (Convert.ToString(((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text) == "-")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Rate for Item Code " + sg1.Rows[i].Cells[3].Text.ToString().Trim() + " Can not be Blank."); return;
                }
                else if (Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text) <= 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Rate for Item Code " + sg1.Rows[i].Cells[3].Text.ToString().Trim() + " Can not be Zero or less then Zero "); return;
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
        dbrow = null;
        dt1 = new DataTable();
        dt1 = (DataTable)ViewState["sg1"];
        for (int i = 0; i < sg1.Rows.Count - 1; i++)
        {
            dbrow = oDS.Tables[0].NewRow();
            dbrow["BRANCHCD"] = frm_mbr;
            dbrow["TYPE"] = frm_vty;
            dbrow["VCHNUM"] = frm_vnum;
            dbrow["vchdate"] = fgen.seek_iname(frm_qstr,frm_cocd, "Select to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as dt from dual", "dt");
            dbrow["srno"] = (i + 1);
            #region Master

            dbrow["Acode"] = txtPCode.Text.Trim().ToUpper();

            dbrow["Icode"] = sg1.Rows[i].Cells[3].Text.Trim().ToUpper();

            //dbrow["Col1"] = txtPaper.Text.Trim().ToUpper();
            //dbrow["Col2"] = txtImp .Text.Trim().ToUpper();
            dbrow["Col3"] = txtProcess.Text.Trim().ToUpper();
            dbrow["Col4"] = txtBoard.Text.Trim().ToUpper();
            dbrow["Col5"] = txtPrinting.Text.Trim().ToUpper();
            dbrow["Col6"] = txtWater.Text.Trim().ToUpper();
            dbrow["Col7"] = txtDie.Text.Trim().ToUpper();
            dbrow["Col8"] = txtStitching.Text.Trim().ToUpper();
            dbrow["Col9"] = txtTaping.Text.Trim();
            dbrow["Col10"] = txtPacking.Text.Trim().ToUpper();
            dbrow["Col11"] = txtProfit.Text.Trim().ToUpper();
            dbrow["Col12"] = txtFreight.Text.Trim().ToUpper();
            dbrow["Col13"] = txtPymt.Text.Trim().ToUpper();
            dbrow["Col14"] = txtExcise.Text.Trim().ToUpper();
            dbrow["Col15"] = txtSales.Text.Trim().ToUpper();
            TextBox txt = (TextBox)sg1.Rows[i].FindControl("txtCol16");
            dbrow["Col16"] = txt.Text.Trim().ToUpper();
            dbrow["Col17"] = txtMinimumQty.Text.Trim().ToUpper();
            // dbrow["Edt_dt"] = frm_ent_time;
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

        //dbrow["hcut"] = ((TextBox)sg1.Rows[i].FindControl("txtfld9")).Text.Trim(); ;
        //dbrow["mtime2"] = ((TextBox)sg1.Rows[i].FindControl("txtfld10")).Text.Trim(); ;


        //if (edmode.Text == "Y")
        //{
        //    oporow["ent_by"] = ViewState["ent_by"].ToString();
        //    oporow["ent_dt"] = ViewState["ent_Dt"].ToString();
        //    oporow["edt_by"] = uname;
        //    oporow["edt_dt"] = System.DateTime.Now;
        //}
        //else
        //{
        //    oporow["ent_by"] = uname;
        //    oporow["ent_dt"] = System.DateTime.Now;
        //    oporow["edt_by"] = "-";
        //    oporow["edt_dt"] = System.DateTime.Now;
        //}
        //    }b
        //}                
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        // for doing save action 
        if (hffield.Value == "List")
        {
            Squery = "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", Squery);
            fgen.Fn_open_rptlevel("-", frm_qstr);
        }
        else if (hffield.Value == "CMD_REP1")
        {
            Squery = "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum";
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
                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE SCRATCH SET BRANCHCD='DD' WHERE BRANCHCD='" + frm_mbr + "' and type='CM' and TRIM(VCHNUM)= '" + ViewState["VCHNUM"].ToString().Trim() + "'");
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
                        if (edmode.Value == "Y")
                        {
                            frm_vnum = ViewState["VCHNUM"].ToString().Trim();
                        }
                        else
                        {
                            int i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(substr(VCHNUM,4,3))+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + frm_datrange + "", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, frm_vty, frm_vnum, System.DateTime.Now.Date.ToString("dd/MM/yyyy"), "", frm_uname);
                                i++;
                                // Bypass for the time
                                pk_error = "N";
                            }
                            while (pk_error == "Y");
                        }

                        save_data();

                        if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(VCHNUM)||to_char(VCHDATE,'dd/mm/yyyy')='" + popselected.Value.Trim() + "'");
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        //fgen.send_mail("Tejaxo ERP","info@pocketdriver.in","","","ITEWSTAGE",""

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", "Doc No." + frm_vnum + "  Updated Successfully");
                            string S = "delete from " + frm_tabname + " where branchcd='DD' and branchcd||type||trim(VCHNUM)||to_char(VCHDATE,'dd/mm/yyyy')='" + popselected.Value.ToString() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' aND trim(VCHNUM)='" + ViewState["VCHNUM"].ToString().Trim() + "'");
                        }
                        else { fgen.msg("-", "AMSG", "Doc No." + frm_vnum + " Saved Successfully "); }
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

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_Ctrl a where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy')='" + popselected.Value.ToUpper() + "'");
                string A = popselected.Value.Substring(4, 6);
                //fgen.save_info(frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), System.DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, frm_vty, "Purchase Requisition Deleted");
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
                    //txtvty.Text = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    disablectrl();
                    btnParty.Focus();
                    create_tab();
                    add_blankrows();
                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    dt1.Dispose(); //dt.Dispose();
                    fgen.EnableForm(this.Controls);
                    break;
                case "Del":
                    if (col1.Length > 0) { }
                    else return;
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
                    if (col1.Length > 0) { }
                    else return;
                    clearctrl();
                    popselected.Value = col1;
                    //fgen.execute_cmd(frm_cocd, "delete from " + frm_tabname + " a where TYPE='"+frm_vty+"' AND branchcd||type||trim(VCHNUM)||to_char(VCHDATE,'dd/mm/yyyy')='" + popselected.Value + "'");
                    hffield.Value = "D";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");

                    break;
                case "Edit":
                    if (col1.Length > 0) { }
                    else return;
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
                    if (col1.Length > 0) { }
                    else return;
                    popselected.Value = col1;


                    //Squery = "select a.*,b.NAME,c.iname from " + frm_tabname + " a,type b,item c where b.id='K' and trim(a.stagec)=trim(B.type1) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.srno ";
                    Squery = "SELECT I.INAME, S.* FROM SCRATCH S, ITEM I WHERE TRIM(I.ICODE)=TRIM(S.ICODE)  AND S.TYPE='" + frm_vty + "' AND  S.branchcd||S.type||trim(S.VCHNUM)||to_char(S.VCHDATE,'dd/mm/yyyy') = '" + col1.Trim() + "'";
                    dtb = new DataTable();
                    dtb = fgen.getdata(frm_qstr, frm_cocd, Squery);
                    // Filing textbox of the form
                    ViewState["VCHNUM"] = dtb.Rows[0]["VCHNUM"].ToString();
                    ViewState["ent_by"] = dtb.Rows[0]["ent_by"].ToString();
                    ViewState["ent_Dt"] = dtb.Rows[0]["ent_dt"].ToString();

                    //txtSCode .Text = dtb.Rows[0]["ICODE"].ToString().Trim();
                    // DataTable dtIname=fgen.getdata(frm_cocd,"Select Iname  From Item Where Icode='"+txtSCode.Text+"'");
                    // txtSName.Text = dtIname.Rows[0]["Iname"].ToString().Trim();
                    txtPCode.Text = dtb.Rows[0]["ACODE"].ToString().Trim();
                    DataTable dtAname = fgen.getdata(frm_qstr, frm_cocd, "Select Aname From Famst Where Acode='" + txtPCode.Text + "'");
                    txtParty.Text = dtAname.Rows[0]["Aname"].ToString().Trim();

                    //txtPaper .Text = dtb.Rows[0]["COL1"].ToString().Trim();
                    //txtImp.Text = dtb.Rows[0]["COL2"].ToString().Trim();
                    txtProcess.Text = dtb.Rows[0]["COL3"].ToString().Trim();
                    txtBoard.Text = dtb.Rows[0]["COL4"].ToString().Trim();
                    txtPrinting.Text = dtb.Rows[0]["COL5"].ToString().Trim();
                    txtWater.Text = dtb.Rows[0]["COL6"].ToString().Trim();
                    txtDie.Text = dtb.Rows[0]["COL7"].ToString().Trim();
                    txtStitching.Text = dtb.Rows[0]["COL8"].ToString().Trim();
                    txtTaping.Text = dtb.Rows[0]["COL9"].ToString().Trim();
                    txtPacking.Text = dtb.Rows[0]["COL10"].ToString().Trim();
                    txtProfit.Text = dtb.Rows[0]["COL11"].ToString().Trim();
                    txtFreight.Text = dtb.Rows[0]["COL12"].ToString().Trim();
                    txtPymt.Text = dtb.Rows[0]["COL13"].ToString().Trim();
                    txtExcise.Text = dtb.Rows[0]["COL14"].ToString().Trim();
                    txtSales.Text = dtb.Rows[0]["COL15"].ToString().Trim();
                    txtMinimumQty.Text = dtb.Rows[0]["Col17"].ToString();
                    create_tab();
                    foreach (DataRow dr in dtb.Rows)
                    {
                        dr1 = dt1.NewRow();
                        dr1["srno"] = dr["srno"].ToString();
                        dr1["icode"] = dr["icode"].ToString();
                        dr1["iname"] = dr["iname"].ToString();
                        dr1["CoL16"] = dr["CoL16"];
                        dt1.Rows.Add(dr1);
                    }
                    add_blankrows();

                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();

                    dt1 = new DataTable();

                    fgen.EnableForm(this.Controls);
                    clearctrl(); disablectrl();
                    dtb.Dispose();
                    dt1.Dispose();
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
                    txtProcess.Focus();
                    dt.Dispose();
                    break;
                case "Print":
                    if (col1.Length > 0) { }
                    else return;
                    set_Val();
                    hffield.Value = "Print_E";
                    frm_vty = col1;
                    // txtvty.Text = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("-", frm_qstr);
                    break;
                case "Print_E":
                    if (col1.Length > 0) { }
                    else return;
                    frm_sql = "SELECT S.VCHNUM AS DOCNO,TO_CHAR(S.VCHDATE,'DD/MM/YYYY') AS DOC_DATE, S.ACODE AS CODE,F.ANAME AS PARTY_NAME,S.ICODE AS ITEM_CODE,I.INAME AS ITEM_NAME,S.COL17 AS MINIMUM_ORDER_QTY , S.COL16 AS ITEM_RATE ,S.COL3 AS PROCESS_WASTAGE,S.COL4 AS BOARD_MAKING_CHARGES,S.COL5 AS PRINTING_SLOTTING,S.COL6 AS WATER_RESISTANCE_COATING,S.COL7 AS DIE_CUTTING,S.COL8 AS STITCHING_FLAP_COSTING ,S.COL9 AS TAPING_BINDING_CLOTH,S.COL10 AS PACKING,S.COL11 AS PROFIT_MARGIN,S.COL12 AS FREIGHT,S.COL13 AS PAYMENT_TERMS,S.COL14 AS EXCISES,S.COL15 AS SALES_Tax FROM SCRATCH S, ITEM I ,FAMST F WHERE TRIM(S.ICODE)=TRIM(I.ICODE)  AND TRIM(S.ACODE)=TRIM(F.ACODE) AND S.TYPE='" + frm_vty + "' AND S.BRANCHCD||S.TYPE||TRIM(S.VCHNUM)||TO_CHAr(S.VCHDATE,'DD/MM/YYYY') in (" + col1 + ") ORDER BY VCHNUM";

                    dt = fgen.getdata(frm_qstr, frm_cocd, frm_sql);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", frm_sql);
                    fgen.Fn_open_rptlevel("", frm_qstr);                    
                    dt.Dispose();
                    break;
                case "List":
                    if (col1.Length > 0) { }
                    else return;
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
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
                            dr1["Col16"] = ((TextBox)sg1.Rows[i].FindControl("txtCol16")).Text.Trim();

                            TextBox Qty = (TextBox)(sg1.Rows[i].FindControl("txtCol16"));
                            Qty.Focus();

                            dt1.Rows.Add(dr1);
                        }
                        if (col1.Trim().Length == 8) frm_sql = "select distinct icode,iname from item where trim(icode) in ('" + col1 + "')";
                        else frm_sql = "select distinct icode,iname,Unit from item where trim(icode) in (" + col1 + ") and icode like '07%'";

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
                            dr1["Col16"] = "0";

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
                    if (col1.Length > 0) { }
                    else return;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;


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

                        dtb = new DataTable();
                        dtb = (DataTable)ViewState["sg1"];
                        dtb.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = dtb;
                        //sg1.DataSource = dtb;
                        //sg1.DataBind();
                        dtb.Dispose();
                    } //Grid_Col_Tot();
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
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        DataTable dt = fgen.getdata(frm_qstr, frm_cocd, "Select Icode AS Fstr,Iname As SubGroupName,Icode As Code,Icat From Item Where Length(Trim(Icode))=4 AND ICODE LIKE '07%'");
        // txtSCode .Text = dt.Rows[0][0].ToString();
        // txtSName.Text = dt.Rows[0][1].ToString();
        //Select Iname  From Item Where Icode=
        //DataTable dt = fgen.getdata(frm_cocd, "Select Acode as fstr,Aname,Acode as Code,Addr1 From Famst Where Acode like '16%'");
        //txtPCode.Text = dt.Rows[0][0].ToString();
        //txtParty.Text = dt.Rows[0][1].ToString();
        //Select Aname From Famst Where Acode=
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
                    //this.cal();
                }
                break;
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[5].Style["display"] = "none";
            //e.Row.Cells[6].Style["display"] = "none";
            //e.Row.Cells[7].Style["display"] = "none";
            //sg1.HeaderRow.Cells[5].Style["display"] = "none";
            //sg1.HeaderRow.Cells[6].Style["display"] = "none";
            //sg1.HeaderRow.Cells[7].Style["display"] = "none";
            //if (frm_cocd == "FIND" && Convert.ToDouble(ulvl) > 0)
            //{
            //    e.Row.Cells[9].Style["display"] = "none";
            //    sg1.HeaderRow.Cells[9].Style["display"] = "none";
            //}

            //if (ulvl == "M" && (frm_cocd == "LIVN" || frm_cocd == "JSGI"))
            //{
            //    ((TextBox)e.Row.FindControl("txtdisc")).ReadOnly = true;
            //    ((TextBox)e.Row.FindControl("txtrate")).ReadOnly = true;
            //}
            //else if (ulvl != "0" && (frm_cocd == "NEOP"))
            //{
            //    ((TextBox)e.Row.FindControl("txtdisc")).ReadOnly = true;
            //    ((TextBox)e.Row.FindControl("txtrate")).ReadOnly = true;
            //}
            //else
            //{
            //    ((TextBox)e.Row.FindControl("txtdisc")).ReadOnly = false;
            //    ((TextBox)e.Row.FindControl("txtrate")).ReadOnly = false;
            //}
        }
    }
    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("Icode", typeof(string)));
        dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
        // dt1.Columns.Add(new DataColumn("Acode", typeof(string)));
        // dt1.Columns.Add(new DataColumn("Aname", typeof(string)));
        dt1.Columns.Add(new DataColumn("Col16", typeof(string)));

    }
    public void add_blankrows()
    {

        dr1 = dt1.NewRow();

        dr1["Srno"] = dt1.Rows.Count + 1;
        dr1["icode"] = "-";
        dr1["iname"] = "-";
        //dr1["Acode"] = "-";
        //dr1["Aname"] = "-";
        dr1["Col16"] = "0";

        dt1.Rows.Add(dr1);
    }
}