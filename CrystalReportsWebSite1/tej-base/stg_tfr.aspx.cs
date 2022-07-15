using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class stg_tfr : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vchnum, vardate, fromdt, todt, prdRange, DateRange, svty, merr = "0", vip = "", mq0, mq1, pk_error = "Y", chk_rights = "N", mhd;
    string val1, val2;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    DataTable dt, dt1; DataRow oporow, dr1; int i, z = 0, opt = 0; DataSet oDS;
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
            DateRange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnnew.Focus(); set_Val();
            }
            cal();
            //txtdrvname.Attributes.Add("onkeypress", "return clickEnter('" + sg1.ClientID + "', event)");
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnext.Visible = true; btncan.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnstgfrom.Enabled = false; btnstgto.Enabled = false;
        create_tab(); add_blankrows();
        sg1.DataSource = dt1; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; dt1.Dispose();
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnext.Visible = false; btncan.Visible = true;
        btnstgfrom.Enabled = true; btnstgto.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    public void set_Val()
    {
        lblheader.Text = "WIP TFR Entry";
        frm_tabname = "ivoucher"; frm_vty = "3A";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        if (frm_cocd == "MANU") svty = "10";
        else svty = "15";
        if (frm_cocd == "GTCF")
        {
            divylb.Visible = true;
            txtremarks.Height = 80;
        }
        else
        {
            divylb.Visible = false;
        }
        btnprint.Visible = false;
        btnlist.Visible = false;
    }
    public void make_qry_4_popup()
    {
        btnval = hffield.Value; set_Val();
        //vty = popselected.Value.Trim();
        switch (btnval)
        {
            case "SFROM":
                SQuery = "Select type1 as fstr,name as stage,type1 as code,'WIP' as stg from type where id='1' order by type1";
                break;
            case "STO":
                SQuery = "Select type1 as fstr,name as stage,type1 as code,'WIP' as stg from type where id='1' and type1 not in ('" + txtstgfcode.Text.Trim() + "','%') order by type1";
                break;
            case "PARTY_POP":
                SQuery = "select distinct a.Type1 as fstr,a.Name as Department,a.Type1 as Code from Type a where id='M' and a.type1 like '6%' ORDER BY a.name";
                break;
            case "Row_Add":
            case "Row_Edit":
                if (sg1.Rows.Count > 1)
                {
                    col1 = ""; col2 = "";
                    foreach (GridViewRow r1 in sg1.Rows)
                    {
                        if (frm_cocd == "MANU")
                        {
                            if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[9].Text.Trim() + "'";
                            else col2 = "'" + r1.Cells[9].Text.Trim() + "'";
                        }
                        else
                        {
                            if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[3].Text.Trim() + "'";
                            else col2 = "'" + r1.Cells[3].Text.Trim() + "'";
                        }
                    }
                    col2 = "(" + col2 + ")";
                }
                else col2 = " ('A')";
                SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) AS FSTR,C.INAME AS PRODUCT,A.ICODE AS CODE,C.CPARTNO AS PARTNO,C.UNIT,SUM(A.IQTYIN)-SUM(A.IQTYOUT) AS BAL,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM (SELECT A.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,TRIM(A.ICODE) AS ICODE,TRIM(A.BTCHNO) AS BTCHNO,A.IQTYIN,0 AS IQTYOUT FROM IVOUCHER A WHERE BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + svty + "' AND A.STAGE='" + txtstgfcode.Text.Trim() + "' AND A.VCHDATE " + DateRange + " AND A.STORE='W' UNION ALL SELECT A.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,TRIM(A.ICODE) AS ICODE,TRIM(A.BTCHNO) AS BTCHNO,0 AS IQTYIN,A.IQTYOUT FROM IVOUCHER A WHERE BRANCHCD='" + frm_mbr + "' AND A.TYPE='3A' AND A.STAGE='" + txtstgfcode.Text.Trim() + "' AND A.VCHDATE " + DateRange + " AND A.STORE='W')  A,ITEM C WHERE TRIM(A.ICODE)=TRIM(C.ICODE) GROUP BY A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE),C.INAME,A.ICODE,C.CPARTNO,C.UNIT,TO_CHAR(A.VCHDATE,'YYYYMMDD') HAVING SUM(A.IQTYIN)-SUM(A.IQTYOUT)>0 ORDER BY VDD DESC";
                SQuery = "select trim(a.icode) as fstr,c.iname as item_name,a.icode as item_code,c.cpartno,c.unit,a.btchno as Batch,a.binno as ABC,sum(a.iqtyin)-sum(a.iqtyout) as balance,trim(a.icode)||trim(A.btchno) as bcode from (select trim(icode) as icode,btchno AS btchno,iqtyin,0 as iqtyout,binno from ivoucher where branchcd='" + frm_mbr + "' and type='" + svty + "' and stage='" + txtstgfcode.Text.Trim() + "'  and store='W' union all select trim(icode) as icode,btchno AS btchno,0 as iqtyin,iqtyout,binno from ivoucher where branchcd='" + frm_mbr + "' and type='3A' and stage='" + txtstgfcode.Text.Trim() + "'  and store='W') a,item c where trim(a.icode)=trim(c.icode) group by c.iname,a.icode,c.cpartno,c.unit,a.btchno,a.binno,trim(a.icode)||trim(A.btchno) having sum(a.iqtyin)-sum(a.iqtyout)>0 order by a.icode";
                //SQuery = "select trim(a.icode)||trim(a.btchno)||a.invno||to_Char(a.invdate,'dd/mm/yyyy') as fstr,c.iname as item_name,a.icode as item_code,c.cpartno,c.unit,a.btchno as Batch,a.binno as ABC,sum(a.iqtyin)-sum(a.iqtyout) as balance,a.invno as job_no,to_char(a.invdate,'dd/mm/yyyy') as job_dt,trim(a.icode)||trim(A.btchno) as bcode,revis_no as Tracking_No from (select trim(icode) as icode,btchno AS btchno,iqtyin,0 as iqtyout,binno,invno,invdate,revis_no from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and stage='" + txtstgfcode.Text.Trim() + "'  and store='W' union all select trim(icode) as icode,btchno AS btchno,0 as iqtyin,iqtyout,binno,invno,invdate,revis_no from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and stage='" + txtstgfcode.Text.Trim() + "'  and store='W') a,item c where trim(a.icode)=trim(c.icode) group by c.iname,revis_no,a.icode,c.cpartno,c.unit,a.btchno,a.binno,trim(a.icode)||trim(A.btchno),a.invno,to_Char(a.invdate,'dd/mm/yyyy'),trim(a.icode)||trim(a.btchno)||a.invno||to_Char(a.invdate,'dd/mm/yyyy') having sum(a.iqtyin)-sum(a.iqtyout)>0 order by a.icode";
                SQuery = "select trim(a.icode)||A.REVIS_NO as fstr,c.iname as item_name,a.icode as item_code,c.cpartno,'0' as Batch,'0' as ABC,c.unit,a.bal as balance" +
                    ",a.revis_no as Tracking_No from (" + fgen.WIPSTKQry(frm_cocd, frm_qstr, frm_mbr, fromdt, todt) + ") a,item c where trim(a.icode)=trim(c.icode) and a.stage='" + txtstgfcode.Text.Trim() + "' order by a.icode";
                break;
            default:
                if (btnval == "New" || btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                {
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Doc_no,to_char(a.vchdate,'dd/mm/yyyy') as doc_Dt,b.iname as Product,a.icode as code,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                    if (frm_cocd == "MANU") SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Doc_no,to_char(a.vchdate,'dd/mm/yyyy') as doc_Dt,b.iname as Product,a.icode as code,a.btchno as Batchno,a.purpose as heatno,a.acode as from_stg,a.iopr as to_stg,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and a.iqtyout>0 order by vdd desc,a.vchnum desc";
                }
                break;
        }
        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl(); set_Val();
        if (chk_rights == "Y")
        {
            frm_vty = col1;
            clearctrl(); set_Val();
            popselected.Value = col1;
            vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
            txtvchnum.Text = vchnum; txtvchdate.Text = vardate;
            disablectrl(); btnstgfrom.Focus();
            fgen.EnableForm(this.Controls);

            if (frm_cocd == "MANU")
            {
                create_tab();
                add_blankrows();
                sg1.DataSource = dt1;
                sg1.DataBind();
                ViewState["sg1"] = dt1;
                txtbarcode.Focus();
            }
            else
            {
                hffield.Value = "SFROM";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Stage From", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl(); set_Val();
        hffield.Value = "Edit";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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
            int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
            if (dhd == 0)
            { fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }
            if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
            { fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only"); txtvchdate.Focus(); return; }

            if (frm_cocd == "SRPF" && txtstgfcode.Text == "62")
            {
                foreach (GridViewRow gr1 in sg1.Rows)
                {
                    if (gr1.Cells[3].Text.Trim().Length > 2)
                    {
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select to_Char(edt_Dt,'DD/MM/YYYY HH24:mi') as ltm from ivoucher where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and stage='" + txtstgfcode.Text + "' AND trim(BTCHNO)='" + gr1.Cells[9].Text.Trim() + "' and trim(icode)='" + gr1.Cells[3].Text.Trim() + "'", "");
                        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT LEAD_TIME FROM item where trim(icode)='" + gr1.Cells[3].Text.Trim() + "'", "");

                        if (col2.Length > 0)
                        {
                            DateTime d1 = new DateTime();
                            d1 = Convert.ToDateTime(col1);

                            DateTime d2 = DateTime.Now;

                            TimeSpan ts = d2.Subtract(d1);

                            if (fgen.make_double(col2) > fgen.make_double(ts.Hours.ToString()))
                            {
                                fgen.msg("-", "AMSG", "Curing Time is less then Allowed Time!!Can Not Save.");
                                return;
                            }
                        }
                    }
                }
            }

            if (sg1.Rows.Count > 1)
            {
                z = 0;
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[3].Text.Trim().Length > 4)
                    {
                        if (Convert.ToDouble(((TextBox)gr.FindControl("txtfld2")).Text) > Convert.ToDouble(gr.Cells[10].Text.Trim()))
                        {
                            z = 1;
                            opt = gr.RowIndex;
                            break;
                        }
                    }
                }
                if (z == 0)
                {
                    fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                    btnsave.Disabled = true;
                }
                else
                {
                    ((TextBox)sg1.Rows[opt].FindControl("txtfld2")).BorderColor = System.Drawing.Color.Red;
                    fgen.msg("-", "AMSG", "Qty can not be greater then Balance Qty'13'Check in Row No. " + (opt + 1) + "'13'Row Balance Qty is " + sg1.Rows[opt].Cells[10].Text.Trim() + "");
                }
            }
            else fgen.msg("-", "AMSG", "No Data to Save");
        }
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to delete data in this form");
        }
        else
        {
            clearctrl(); set_Val();
            hffield.Value = "Del";
            popselected.Value = frm_vty;
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);
        }
    }
    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr, false);

    }
    protected void btncan_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        dt1 = new DataTable();
        create_tab(); add_blankrows();
        sg1.DataSource = dt1; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; dt1.Dispose();
        ViewState["sg1"] = null;
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value; set_Val();
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Delete from Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                // Delete from Table WSR_CTRL
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                // Save in Fin Info Table
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), popselected.Value.Substring(10, 10), frm_uname, popselected.Value.Substring(2, 2), "Stage TFR Entry DELETED");

                fgen.msg("-", "AMSG", "Details are deleted for Prodn Entry " + popselected.Value.Substring(4, 6) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else
        {
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

                switch (btnval)
                {
                    case "New":
                        frm_vty = col1;
                        clearctrl(); set_Val();
                        popselected.Value = col1;
                        vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                        txtvchnum.Text = vchnum; txtvchdate.Text = vardate;
                        disablectrl();
                        fgen.EnableForm(this.Controls);
                        break;
                    //case "Del":
                    //    clearctrl();
                    //    popselected.Value = col1;
                    //    hffield.Value = "Del_E";
                    //    make_qry_4_popup();
                    //    fgen.open_sseek("Select WIP TFR Entry to delete");
                    //    break;
                    case "Del":
                        if (col1 == "") return;
                        clearctrl();
                        popselected.Value = col1;
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                        hffield.Value = "D";
                        break;
                    //case "Edit":
                    //    // this is after type selection 
                    //    clearctrl(); set_Val();
                    //    hffield.Value = "Edit_E";
                    //    popselected.Value = col1;
                    //    make_qry_4_popup();
                    //    fgen.open_sseek("Select Prodn Entry");
                    //    break;
                    case "Edit":
                        // this is after gate entry selection
                        if (col1 == "") return;
                        popselected.Value = col1;
                        SQuery = "select a.*,to_char(a.invdate,'dd/mm/yyyy') as invdatee,b.NAME as aname,c.iname,c.cpartno,c.maker,c.unit from " + frm_tabname + " a,type b,item c where b.id='M' and trim(a.acodE)=trim(B.type1) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' and a.iqtyOUT>0 order by a.morder ";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        // Filing textbox of the form
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                        txtstgfcode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtstgfname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='1' and trim(type1)='" + dt.Rows[0]["acode"].ToString().Trim() + "'", "name");
                        txtstgtcode.Text = dt.Rows[0]["iopr"].ToString().Trim();
                        txtstgtname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='1' and trim(type1)='" + dt.Rows[0]["iopr"].ToString().Trim() + "'", "name");
                        ViewState["ent_by"] = dt.Rows[0]["ent_by"].ToString().Trim();
                        ViewState["ent_Dt"] = dt.Rows[0]["ent_dt"].ToString().Trim();
                        create_tab();
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (Convert.ToDouble(dr["IQTYOUT"].ToString().Trim()) > 0)
                            {
                                dr1 = dt1.NewRow();
                                dr1["srno"] = dr["srno"];
                                dr1["icode"] = dr["icode"].ToString().Trim();
                                dr1["iname"] = dr["iname"].ToString().Trim();
                                dr1["cpartno"] = dr["cpartno"].ToString().Trim();
                                dr1["unit"] = dr["unit"].ToString().Trim();

                                dr1["tfld2"] = dr["iqtyOUT"].ToString().Trim();
                                dr1["tfld3"] = dr["desc_"].ToString().Trim();
                                dr1["tfld4"] = dr["invno"].ToString().Trim();
                                dr1["tfld5"] = dr["invdatee"].ToString().Trim();
                                dr1["tfld6"] = dr["revis_no"].ToString().Trim();

                                if (frm_cocd == "GTCF")
                                {
                                    dr1["poqty"] = "0";
                                    dr1["tfld1"] = dr["purpose"].ToString().Trim();
                                }
                                else
                                {
                                    dr1["poqty"] = dr["btchno"].ToString().Trim();
                                    dr1["tfld1"] = dr["purpose"].ToString().Trim();
                                    dr1["tfld3"] = dr["binno"].ToString().Trim();
                                }

                                dt1.Rows.Add(dr1);
                                if (frm_cocd == "GTCF")
                                {
                                    lblpartno.Text = dr["CPARTNO"].ToString().Trim();
                                    lbllcno.Text = dr["maker"].ToString().Trim();
                                    lblylb.Text = dr["binno"].ToString().Trim();
                                }
                            }
                        }
                        add_blankrows();
                        sg1.DataSource = dt1; sg1.DataBind();
                        ViewState["sg1"] = dt1;
                        fgen.EnableForm(this.Controls);
                        clearctrl(); disablectrl();
                        edmode.Value = "Y";
                        break;
                    case "PARTY_POP":
                        dt1 = new DataTable();
                        create_tab(); add_blankrows();
                        sg1.DataSource = dt1; sg1.DataBind(); ViewState["sg1"] = dt1;
                        break;
                    case "SFROM":
                        if (col1.ToString().Length < 2) return;
                        txtstgfcode.Text = col1; txtstgfname.Text = col2;
                        btnstgto.Focus();
                        // Opening STO Popup
                        hffield.Value = "STO";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Stage To", frm_qstr);
                        break;
                    case "STO":
                        if (col1.ToString().Length < 2) return;
                        txtstgtcode.Text = col1; txtstgtname.Text = col2;
                        create_tab(); add_blankrows();

                        sg1.DataSource = dt1; sg1.DataBind(); ViewState["sg1"] = dt1;
                        ((ImageButton)sg1.Rows[0].FindControl("btnadd")).Focus();

                        // Opening Item Popup
                        hf1.Value = "1";
                        hffield.Value = "Row_Add";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                        break;
                    case "Print":
                        clearctrl(); set_Val();
                        hffield.Value = "Print_E";
                        popselected.Value = col1;
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                        break;
                    case "Print_E":
                        //SQuery = "select a.vchnum from " + frm_tabname + " a where a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                        //fgen.Print_Report(co_cd,frm_mbr, SQuery, "new_inspmt", "new_inspmt");
                        break;
                    case "List":
                        popselected.Value = col1;
                        fgen.Fn_open_prddmp1("Select Date Range for G.E. List", frm_qstr);
                        break;
                    case "Row_Add":
                        add_data_grid(col1);
                        break;
                    case "Row_Edit":
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                        dt = new DataTable();
                        SQuery = "select * from (select c.iname ,a.icode,c.cpartno,c.unit,a.btchno ,sum(a.iqtyin)-sum(a.iqtyout) as balance from ivoucher a,item c where trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type in ('" + svty + "','" + frm_vty + "','39') and a.stage='" + txtstgfcode.Text.Trim() + "' and a.vchdate " + DateRange + " and a.store='W' group by c.iname,a.icode,c.cpartno,c.unit,a.btchno having sum(a.iqtyin)-sum(a.iqtyout)>0 ) where trim(a.icode)||trim(a.btchno)||trim(a.binno) ='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = dt.Rows[0]["iname"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = dt.Rows[0]["cpartno"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = dt.Rows[0]["unit"].ToString().Trim();
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtfld2")).Text = dt.Rows[0]["balance"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = dt.Rows[0]["btchno"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[10].Text = dt.Rows[0]["balance"].ToString().Trim();
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtfld1")).Focus();
                        }
                        myfun();
                        break;
                    case "Rmv":
                        if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        {
                            dt = new DataTable();
                            dt = (DataTable)ViewState["sg1"];
                            dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                            ViewState["sg1"] = dt;
                            sg1.DataSource = dt;
                            sg1.DataBind();
                            dt.Dispose();
                        }
                        myfun();
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "List")
        {
            fromdt = Request.Cookies["Value1"].Value.ToString().Trim().Replace("&amp", "");
            todt = Request.Cookies["Value2"].Value.ToString().Trim().Replace("&amp", "");
            prdRange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
            SQuery = "select vchnum,vchdate,acode,iqty_chk,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + prdRange + " order by vchdate,vchnum";
            // This cookie to send query to RPT level form
            fgen.send_cookie("seekSql", SQuery);
            fgen.Fn_open_rptlevel("List", frm_qstr);
        }
        else
        {
            col1 = ""; set_Val();
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                try
                {
                    set_Val();
                    //vty = popselected.Value;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    // This is for checking that, is it ready to save the data
                    vchnum = "000000";
                    save_fun();

                    oDS.Dispose(); oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
                    else
                    {
                        string doc_is_ok = "";
                        vchnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, "vchnum", "vchdate", frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, frm_formID);
                        doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                        if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                    }
                    save_fun();

                    if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.Trim() + "'");
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", "Data Updated Successfully");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.ToString().Substring(2, 18) + "'");
                    }
                    else { fgen.msg("-", "AMSG", "Data Saved Successfully"); }
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                    col1 = "N";
                }
                catch (Exception ex)
                {
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
        }
    }
    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("Icode", typeof(string)));
        dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
        dt1.Columns.Add(new DataColumn("Cpartno", typeof(string)));
        dt1.Columns.Add(new DataColumn("unit", typeof(string)));

        dt1.Columns.Add(new DataColumn("poqty", typeof(string)));
        dt1.Columns.Add(new DataColumn("tfld1", typeof(string)));
        dt1.Columns.Add(new DataColumn("tfld2", typeof(string)));
        dt1.Columns.Add(new DataColumn("tfld3", typeof(string)));
        dt1.Columns.Add(new DataColumn("tfld4", typeof(string)));
        dt1.Columns.Add(new DataColumn("tfld5", typeof(string)));
        dt1.Columns.Add(new DataColumn("tfld6", typeof(string)));

    }
    public void add_blankrows()
    {
        dr1 = dt1.NewRow();

        dr1["Srno"] = dt1.Rows.Count + 1;
        dr1["icode"] = "-";
        dr1["iname"] = "-";
        dr1["Cpartno"] = "-";
        dr1["unit"] = "-";
        dr1["poqty"] = "0";
        dr1["tfld1"] = "0";
        dr1["tfld2"] = "-";
        dr1["tfld3"] = "-";
        dr1["tfld4"] = "-";
        dr1["tfld5"] = "-";
        dr1["tfld6"] = "-";

        dt1.Rows.Add(dr1);
    }
    protected void btnacode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PARTY_POP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Department Name", frm_qstr);
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
            case "Row_Add":
                if (txtstgfcode.Text == "" || txtstgfcode.Text == "0") fgen.msg("-", "AMSG", "First Please Select From Stage!!");
                else if (txtstgtcode.Text == "" || txtstgtcode.Text == "0") fgen.msg("-", "AMSG", "First Please Select To Stage!!");
                else
                {
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        hffield.Value = "Row_Edit";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "Row_Add";
                        make_qry_4_popup();
                        //fgen.open_mseek("Select Your Product(s)");
                        fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                    }
                    this.cal();
                }
                break;
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (frm_cocd == "GTCF")
            {
                e.Row.Cells[0].Style["display"] = "none";
                sg1.HeaderRow.Cells[0].Style["display"] = "none";
                e.Row.Cells[1].Style["display"] = "none";
                sg1.HeaderRow.Cells[1].Style["display"] = "none";

                ((TextBox)e.Row.FindControl("txtfld1")).Width = 130;
                sg1.HeaderRow.Cells[7].Text = "Job Card No.";
                sg1.HeaderRow.Cells[7].Width = 120;
                sg1.HeaderRow.Cells[8].Width = 60;
                sg1.HeaderRow.Cells[9].Text = "Pallat No.";
                sg1.HeaderRow.Cells[11].Text = "YLB";
            }
            if (frm_cocd == "MANU")
            {
                sg1.HeaderRow.Cells[7].Text = "Heat No.";
            }
        }
    }
    void cal()
    {
        double vp = 0, vp1 = 0;
        for (int zk = 0; zk < sg1.Rows.Count - 1; zk++)
        {
            //vp1 = Convert.ToDouble(((TextBox)sg1.Rows[zk].FindControl("txtfld1")).Text.Trim());
            vp += vp1;
        }
    }
    public void myfun()
    {
        vip = "";
        vip = vip + "<script type='text/javascript'>function calculateSum() {";
        vip = vip + "var vp=0;";
        mq0 = "";
        for (int zk = 2; zk < sg1.Rows.Count + 1; zk++)
        {
            if (mq0.Length > 0) mq0 = mq0 + "+ (fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_sg1_ctl0" + (zk * 1) + "_txtchlqty').value)*1)";
            else mq0 = "(fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_sg1_ctl0" + (zk * 1) + "_txtchlqty').value)*1)";
            //vip = vip + "vp=vp + (fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_sg1_ctl0" + (zk * 1) + "_txtchlqty').value)*1);";
        }
        vip = vip + "vp=" + mq0 + ";";
        //vip = vip + "vp=(fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_sg1_ctl02_txtchlqty').value)*1) + (fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_sg1_ctl03_txtchlqty').value)*1) + (fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_sg1_ctl04_txtchlqty').value)*1) ;";
        vip = vip + "document.getElementById('ctl00_ContentPlaceHolder1_lblqtysum').innerHTML = vp; ";
        vip = vip + "}";
        vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", vip.ToString(), false);
    }
    protected void btnh_ServerClick(object sender, EventArgs e)
    {
        ((ImageButton)sg1.Rows[0].FindControl("btnadd")).Focus();
    }
    //save_click
    void save_fun()
    {
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if (sg1.Rows[i].Cells[3].Text.Trim().Length > 4)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = vchnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["acode"] = txtstgfcode.Text.Trim();
                oporow["stage"] = txtstgfcode.Text.Trim();
                oporow["srno"] = (i + 1);
                oporow["morder"] = (i + 1);
                oporow["RCODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                oporow["ICODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                oporow["iqtyin"] = 0;
                oporow["iqty_chl"] = 0;
                oporow["iqtyout"] = ((TextBox)sg1.Rows[i].FindControl("txtfld2")).Text.Trim();
                oporow["PURPOSE"] = ((TextBox)sg1.Rows[i].FindControl("txtfld1")).Text.Trim().Replace("-", "");
                oporow["btchno"] = sg1.Rows[i].Cells[9].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                oporow["binno"] = sg1.Rows[i].Cells[11].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                oporow["rec_iss"] = "C";
                oporow["iopr"] = txtstgtcode.Text.Trim();
                oporow["INSPECTED"] = "N";

                oporow["invno"] = sg1.Rows[i].Cells[12].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                oporow["invdate"] = sg1.Rows[i].Cells[13].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                oporow["revis_no"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");

                oporow["desc_"] = "Tfr to " + txtstgtname.Text + " from " + txtstgfname.Text + "";

                oporow["naration"] = txtremarks.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["rej_rw"] = 0;
                oporow["acpt_ud"] = 0;
                oporow["store"] = "W";

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_dt"] = ViewState["ent_Dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dt"] = System.DateTime.Now;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_dt"] = System.DateTime.Now;
                    oporow["edt_by"] = "-";
                    oporow["edt_dt"] = System.DateTime.Now;
                }
                oDS.Tables[0].Rows.Add(oporow);

                //oporow = oDS.Tables[0].NewRow();
                //oporow["BRANCHCD"] = frm_mbr;
                //oporow["TYPE"] = frm_vty;
                //oporow["vchnum"] = vchnum;
                //oporow["vchdate"] = txtvchdate.Text.Trim();

                //oporow["acode"] = txtstgtcode.Text.Trim();
                //oporow["stage"] = txtstgtcode.Text.Trim();
                //oporow["srno"] = (i + 1);
                //oporow["morder"] = (i + 1);
                //oporow["RCODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                //oporow["ICODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                //oporow["iqtyin"] = ((TextBox)sg1.Rows[i].FindControl("txtfld2")).Text.Trim();
                //oporow["iqty_chl"] = 0;
                //oporow["iqtyout"] = 0;
                //oporow["rec_iss"] = "D";
                //oporow["iopr"] = "-";
                //oporow["acpt_ud"] = ((TextBox)sg1.Rows[i].FindControl("txtfld2")).Text.Trim();
                //oporow["PURPOSE"] = ((TextBox)sg1.Rows[i].FindControl("txtfld1")).Text.Trim().Replace("-", "");
                //oporow["btchno"] = sg1.Rows[i].Cells[9].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                //oporow["BINNO"] = sg1.Rows[i].Cells[11].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                //oporow["invno"] = sg1.Rows[i].Cells[12].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                //oporow["invdate"] = sg1.Rows[i].Cells[13].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                //oporow["revis_no"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                //oporow["desc_"] = "Tfr from " + txtstgfname.Text + " to " + txtstgtname.Text + "";
                //oporow["naration"] = txtremarks.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["rej_rw"] = 0;
                //oporow["store"] = "W";
                //oporow["INSPECTED"] = "N";
                //if (edmode.Value == "Y")
                //{
                //    oporow["ent_by"] = ViewState["ent_by"].ToString();
                //    oporow["ent_dt"] = ViewState["ent_Dt"].ToString();
                //    oporow["edt_by"] = frm_uname;
                //    oporow["edt_dt"] = System.DateTime.Now;
                //}
                //else
                //{
                //    oporow["ent_by"] = frm_uname;
                //    oporow["ent_dt"] = System.DateTime.Now;
                //    oporow["edt_by"] = "-";
                //    oporow["edt_dt"] = System.DateTime.Now;
                //}
                //oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    protected void btnstgfrom_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SFROM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Stage From", frm_qstr);
    }
    protected void btnstgto_Click(object sender, ImageClickEventArgs e)
    {
        if (txtstgfcode.Text.Trim().Length > 1)
        {
            hffield.Value = "STO";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Stage To", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Please First Select Stage From!!");
    }
    void add_data_grid(string pop_value)
    {
        set_Val();
        col1 = pop_value;
        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();
            dt1 = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            z = dt.Rows.Count - 1;
            dt1 = dt.Clone();
            dr1 = null;

            col3 = "";
            foreach (GridViewRow Grr in sg1.Rows)
            {
                if (Grr.Cells[9].Text.Trim().ToString().Trim().ToUpper().Replace("&NBSP;", "").Length > 1) val1 = Grr.Cells[9].Text.Trim().ToString().Trim();
                if (Grr.Cells[11].Text.Trim().ToString().Trim().ToUpper().Replace("&NBSP;", "").Length > 1) val2 = Grr.Cells[11].Text.Trim().ToString().Trim();
                if (col3.Length > 0) col3 = col3 + ",'" + val1 + val2 + "'";
                else
                {
                    if ((val1 + val2).ToString().Length > 0)
                        col3 = "'" + val1 + val2 + "'";
                }
            }
            for (i = 0; i < dt.Rows.Count - 1; i++)
            {
                dr1 = dt1.NewRow();
                dr1["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                dr1["icode"] = dt.Rows[i]["icode"].ToString();
                dr1["iname"] = dt.Rows[i]["iname"].ToString();
                dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString();
                dr1["unit"] = dt.Rows[i]["unit"].ToString();
                dr1["poqty"] = dt.Rows[i]["poqty"].ToString();
                dr1["tfld1"] = ((TextBox)sg1.Rows[i].FindControl("txtfld1")).Text.Trim();
                dr1["tfld2"] = ((TextBox)sg1.Rows[i].FindControl("txtfld2")).Text.Trim();
                dr1["tfld3"] = dt.Rows[i]["tfld3"].ToString();
                try
                {
                    dr1["tfld4"] = dt.Rows[i]["tfld4"].ToString();
                    dr1["tfld5"] = dt.Rows[i]["tfld5"].ToString();
                    dr1["tfld6"] = dt.Rows[i]["tfld6"].ToString();
                }
                catch
                {
                }


                dt1.Rows.Add(dr1);
            }
            if (frm_cocd == "MANU")
            {
                if (col3.Length > 3) col3 = " and trim(btchno)||trim(binno) not in (" + col3 + ") ";
                if (col3.Length > 3)
                {
                    mhd = fgen.seek_iname_dt(dt1, "poqty+tfld3='" + col1.Trim().ToUpper() + "'", "poqty");
                    if (mhd != "0")
                    {
                        mhd = fgen.seek_iname_dt(dt1, "poqty+tfld3='" + col1.Trim().ToUpper() + "'", "srno");
                        fgen.msg("-", "AMSG", "Scanned Bar Code is already exist in List");
                        return;
                    }
                    mhd = fgen.seek_iname_dt(dt1, "tfld3='" + col1.Trim().ToUpper() + "'", "tfld3");
                    if (mhd != "0")
                    {
                        mhd = fgen.seek_iname_dt(dt1, "tfld3='" + col1.Trim().ToUpper() + "'", "srno");
                        fgen.msg("-", "AMSG", "Scanned Bar Code is already exist in List");
                        return;
                    }
                }
            }
            SQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            SQuery = "select * from (" + SQuery + ") where trim(fstr) ='" + col1 + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                dr1 = dt1.NewRow();
                dr1["srno"] = dt1.Rows.Count + 1;
                dr1["icode"] = dt.Rows[i]["item_code"].ToString().Trim();
                dr1["iname"] = dt.Rows[i]["item_name"].ToString().Trim();
                dr1["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                dr1["unit"] = dt.Rows[i]["unit"].ToString().Trim();
                dr1["poqty"] = dt.Rows[i]["batch"].ToString().Trim();
                dr1["tfld1"] = "-";
                dr1["tfld2"] = dt.Rows[i]["BALANCE"].ToString().Trim();
                dr1["tfld3"] = dt.Rows[i]["batch"].ToString().Trim();
                //dr1["tfld4"] = dt.Rows[i]["job_no"].ToString().Trim();
                //dr1["tfld5"] = dt.Rows[i]["job_dt"].ToString().Trim();
                try
                {

                    dr1["tfld4"] = dt.Rows[i]["Tracking_No"].ToString().Trim().Substring(0, 6);
                    dr1["tfld5"] = dt.Rows[i]["Tracking_No"].ToString().Trim().Substring(6, 10);

                }
                catch (Exception ee)
                {

                }
                dr1["tfld6"] = dt.Rows[i]["Tracking_No"].ToString().Trim();
                dt1.Rows.Add(dr1);
                if (frm_cocd == "GTCF")
                {
                    //lblpartno.Text = dt.Rows[i]["CPARTNO"].ToString().Trim();
                    //lbllcno.Text = dt.Rows[i]["maker"].ToString().Trim();
                    //lblylb.Text = dt.Rows[i]["binno"].ToString().Trim();
                }
            }
        }
        else return;
        add_blankrows();
        ViewState["sg1"] = dt1;
        sg1.DataSource = dt1;
        sg1.DataBind();
        dt.Dispose(); dt1.Dispose(); myfun();
        ((TextBox)sg1.Rows[z].FindControl("txtfld1")).Focus();
    }
    protected void txtbarcode_TextChanged(object sender, EventArgs e)
    {
        fgen.fill_dash(this.Controls); mhd = "";
        if (txtstgfcode.Text.Trim() == "-" || txtstgtcode.Text.Trim() == "-" || txtstgfcode.Text.Trim() == "" || txtstgtcode.Text.Trim() == "")
        {
            SQuery = "SELECT MAX(TO_CHAR(VCHDATE,'YYYYMMDD')||TRIM(VCHNUM)||TO_CHAR(ENT_dT,'YYYYMMDDHHMISS')||STAGE) AS SS,TO_CHAr(VCHDATE,'YYYYMMDD') AS VDD,trim(icodE)||'-'||STAGE as STAGE FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND VCHDATE > to_Date('01/04/2010','dd/mm/yyyy') AND TRIM(icode)||TRIM(btchno)='" + txtbarcode.Text.Trim() + "' AND STORE='W' GROUP BY trim(icodE)||'-'||STAGE,TO_CHAr(VCHDATE,'YYYYMMDD') ORDER BY VDD DESC,STAGE DESC";
            if (frm_cocd == "MANU") SQuery = "SELECT MAX(TO_CHAR(VCHDATE,'YYYYMMDD')||TRIM(VCHNUM)||TO_CHAR(ENT_dT,'YYYYMMDDHHMISS')||STAGE) AS SS,TO_CHAr(VCHDATE,'YYYYMMDD') AS VDD,trim(icodE)||'-'||STAGE as STAGE FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND VCHDATE > to_Date('01/04/2010','dd/mm/yyyy') AND TRIM(BTCHNO)||TRIM(BINNO)='" + txtbarcode.Text.Trim() + "' AND STORE='W' GROUP BY trim(icodE)||'-'||STAGE,TO_CHAr(VCHDATE,'YYYYMMDD') ORDER BY VDD DESC,STAGE DESC";
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "STAGE");
            try
            {
                if (mhd.Substring(0, 1) != "0")
                {
                    txtstgfcode.Text = mhd.Split('-')[1].ToString().Trim();
                    txtstgfname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where type1='" + mhd.Split('-')[1].ToString().Trim() + "' and id='1'", "name");
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND SUBSTR(ICODE,1,4)='" + mhd.Split('-')[0].ToString().Trim().Substring(0, 4) + "' AND STAGEC>" + mhd.Split('-')[1].ToString().Trim() + " ORDER BY SRNO", "STAGEC");
                    txtstgtcode.Text = mhd;
                    txtstgtname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where type1='" + mhd + "' and id='1'", "name");
                }
                else
                {
                    txtbarcode.Text = "";
                    txtbarcode.Focus();
                    fgen.msg("-", "AMSG", "Please Select Stages First!!");
                    return;
                }
            }
            catch
            {
                txtbarcode.Text = "";
                txtbarcode.Focus();
                fgen.msg("-", "AMSG", "Please Select Stages First/Stage Not Linked!!");
                return;
            }
        }
        if (txtbarcode.Text.Trim().Length > 0)
        {
            foreach (GridViewRow gr1 in sg1.Rows)
            {
                if ((gr1.Cells[9].Text.Trim().ToString().Replace("0", "").Replace("-", "") + gr1.Cells[11].Text.Trim().ToString().Replace("0", "").Replace("-", "")) == txtbarcode.Text.Trim())
                {
                    z = 6;
                    break;
                }
            }
            if (z == 6) ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Already in List')", true);
            else add_data_grid(txtbarcode.Text.Trim());
        }
        txtbarcode.Text = "";
        txtbarcode.Focus();
    }
}