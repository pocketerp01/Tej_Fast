using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.UI.WebControls.WebParts;


//SDR===FORM ID ON STATIC
//SCRATCH_12APRIL_20  TABLE BACKUP BEFORE SAVING

public partial class om_upd_sdr : System.Web.UI.Page
{
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    DataTable dtCol = new DataTable();
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataRow dr1, oporow;
    int i = 0, z = 0;
    DataSet oDS = new DataSet();
    OracleConnection consql = new OracleConnection();
    OracleCommand command1;
    OracleDataAdapter da, da1;
    System.DateTime sedate, presentdate, vchdate, sigdt, hsdt, invdate;
    int nflag,pflag, opt, opt1, opt2, opt3, opt4, opt5, opt6;
    string msg, scode, sname, vchnum, Seeksql, popvar, co_cd, tco_cd, mbr, Sedtby, Sedt, col1, col2, col3, typePopup = "Y";
    string uname, vardate, strsort, btnmode, daterange, cdt1, cdt2, olddate, Dept, btnval, SQuery;
    string br_name, br_addr, br_addr1, br_place, br_tele, br_fax, view_name, br_addr2, firm, ulvl, dmllvl, filepath, mq0, mq1, mq2, sender_id, pwd, vsmtp, xvip, xport, co_cd_fgen, merror;
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url,fromdt,todt, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    int ssl, port;
    MailMessage mail;
    SmtpClient smtp;
    HttpCookie HP;
    MailMessage message;
    MemoryStream oStream;
    ReportDocument repDoc = new ReportDocument();
    ReportDocument report;

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
                    Dept = fgen.seek_iname(frm_qstr, co_cd, "select deptt from evas where username='" + frm_uname + "'", "deptt");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //if (Convert.ToInt32(frm_ulvl) > 1)
                //{
                //    btnedit.Visible = false;
                //    btndel.Visible = false;
                //}
                //else
                //{
                //    btnedit.Visible = true;
                //    btndel.Visible = true;
                //}
                fgen.DisableForm(this.Controls);              
                disablectrl();
                btnnew.Disabled = false;
                btnedit.Disabled = false;
                btndel.Disabled = false;
                btnprint.Disabled = false;
                btnlist.Disabled = false;
              //  btnexit.Enabled = true;
                btnnew.Focus();
                MultiView1.ActiveViewIndex = 0;
                if (Dept.Length >= 1)
                {
                    if (Dept.Substring(0, 1) == "R")
                    {
                        fgen.DisableForm(this.Controls);
                    }
                    else
                    {
                        //fgen.EnableForm(this.Controls);
                        //txtDraft.Enabled = false;
                        //txtDraftDt.Enabled = false;
                        //txtFeasDoc.Enabled = false;
                        //txtFeasRev.Enabled = false;
                        //txtFeasDate.Enabled = false;
                        //txtSDR2.Enabled = false;
                        //txtSDR2Date.Enabled = false;
                        //txtCust2.Enabled = false;
                        //sg1.Enabled = false;
                        //sg1.DataBind();
                        //txti.Enabled = false;
                        //txtii.Enabled = false;
                        //txtiii.Enabled = false;
                        //txtiiii.Enabled = false;
                        //txtfeedback.Enabled = false;
                        //txtclosed.Enabled = false;
                        //txtclosed2.Enabled = false;                        
                    }                    
                }               
                //===============             
                doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
                attch1.Visible = false;
            }
            
            attch1.Visible = true;
            txtatthment1.Visible = true;
            btnView1.Visible = true;
            txtFeasDoc.Text = "F/MKTG/02";
            txtFeasRev.Text = "0";
            txtFeasDate.Text = "14/04/2017";                                   
            }
            setColHeadings();
            set_Val();
           
    }    

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
    //    else
    //    {
    //        tco_cd = Request.Cookies["CK_COFILEVARS"].Value.ToString();
    //        mbr = Request.Cookies["CK_mbr"].Value.ToString();
    //        // mbr = "39";
    //        co_cd = tco_cd.Substring(0, 4);
    //        cdt1 = tco_cd.Substring(9, 10);
    //        cdt2 = tco_cd.Substring(19, 10);
    //        uname = Request.Cookies["UNAME"].Value.ToString();
    //        col1 = Request.Cookies["UL_ACODE"].Value.ToString();
    //        ulvl = col1.Substring(0, 1);
    //        //ulvl = "2";
    //        dmllvl = Request.Cookies["DML_LVL"].Value.ToString();
    //        daterange = "between to_Date('" + cdt1 + "','dd/MM/yyyy') and to_Date('" + cdt2 + "','dd/MM/yyyy')";
    //        Dept = fgen.seek_iname(frm_qstr,co_cd, "select deptt from evas where username='" + uname + "'", "deptt");
            
    //        // Dept = "R : R&esearch && Development";
    //        if (!IsPostBack)
    //        {
    //            //if (Convert.ToInt32(ulvl) > 1)
    //            //{
    //            //    btnedit.Visible = false;
    //            //    btndel.Visible = false;
    //            //}
    //            //else
    //            //{
    //            //    btnedit.Visible = true;
    //            //    btndel.Visible = true;
    //            //}
    //            fgen.DisableForm(this.Controls);
    //            disable_btn();
    //            btnnew.Disabled = false;
    //            btnedit.Disabled = false;
    //            btndel.Disabled = false;
    //            btnprint.Disabled = false;
    //            btnlist.Disabled = false;
    //            btnexit.Enabled = true;
    //            btnnew.Focus();
    //            MultiView1.ActiveViewIndex = 0;
    //            if (Dept.Length >= 1)
    //            {
    //                if (Dept.Substring(0, 1) == "R")
    //                {
    //                    fgen.DisableForm(this.Controls);
    //                }
    //                else
    //                {
    //                    //fgen.EnableForm(this.Controls);
    //                    //txtDraft.Enabled = false;
    //                    //txtDraftDt.Enabled = false;
    //                    //txtFeasDoc.Enabled = false;
    //                    //txtFeasRev.Enabled = false;
    //                    //txtFeasDate.Enabled = false;
    //                    //txtSDR2.Enabled = false;
    //                    //txtSDR2Date.Enabled = false;
    //                    //txtCust2.Enabled = false;
    //                    //sg1.Enabled = false;
    //                    //sg1.DataBind();
    //                    //txti.Enabled = false;
    //                    //txtii.Enabled = false;
    //                    //txtiii.Enabled = false;
    //                    //txtiiii.Enabled = false;
    //                    //txtfeedback.Enabled = false;
    //                    //txtclosed.Enabled = false;
    //                    //txtclosed2.Enabled = false;
    //                }
    //            }
    //        }
    //        txtFeasDoc.Text = "F/MKTG/02";
    //        txtFeasRev.Text = "0";
    //        txtFeasDate.Text = "14/04/2017";

    //    }
    //}

 

    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true;btnexit.Visible = true; btncancel.Visible = false;      
        btnprint.Disabled = false; btnlist.Disabled = false; btnsubmit.Disabled = true;
      //  create_tab();      
        //sg1.DataSource = sg1_dt; sg1.DataBind();
        //if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    void getColHeading()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            dtCol = fgen.getdata(frm_qstr, frm_cocd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_SYS_COM_QRY") + " WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        }
        ViewState["d" + frm_qstr + frm_formID] = dtCol;
    }
    //------------------------------------------------------------------------------------
    void setColHeadings()
    {
    //    dtCol = new DataTable();
    //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
    //    if (dtCol == null || dtCol.Rows.Count <= 0)
    //    {
    //        getColHeading();
    //    }
    //    dtCol = new DataTable();
    //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];

    //    if (dtCol == null) return;
    //    if (sg1.Rows.Count <= 0) return;
    //    for (int sR = 0; sR < sg1.Columns.Count; sR++)
    //    {
    //        string orig_name;
    //        double tb_Colm;
    //        tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
    //        orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

    //        for (int K = 0; K < sg1.Rows.Count; K++)
    //        {
    //            #region hide hidden columns
    //            for (int i = 0; i < 10; i++)
    //            {
    //                sg1.Columns[i].HeaderStyle.CssClass = "hidden";
    //                sg1.Rows[K].Cells[i].CssClass = "hidden";
    //            }
    //            #endregion
    //            if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
    //        }
    //        orig_name = orig_name.ToUpper();
    //        //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
    //        if (sR == tb_Colm)
    //        {
    //            // hidding column
    //            if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
    //            {
    //                sg1.Columns[sR].Visible = false;
    //            }
    //            // Setting Heading Name
    //            sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
    //            // Setting Col Width
    //            string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
    //            if (fgen.make_double(mcol_width) > 0)
    //            {
    //                sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
    //                sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
    //            }
    //        }
    //    }

    //    //txtlbl8.Attributes.Add("readonly", "readonly");
    //    //txtlbl9.Attributes.Add("readonly", "readonly");



    //    //// to hide and show to tab panel
    //    //tab5.Visible = false;
    //    //tab4.Visible = false;
    //    //tab3.Visible = false;
    //    //tab2.Visible = false;

    //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    //    //switch (Prg_Id)
    //    //{
    //    //    case "M09024":
    //    //    case "M10003":
    //    //    case "M11003":
    //    //    case "M10012":
    //    //    case "M11012":
    //    //    case "M12008":
    //    //        tab3.Visible = false;
    //    //        tab4.Visible = false;
    //    //        break;
    //    //}
    //    //if (Prg_Id == "M12008")
    //    //{
    //    //    tab5.Visible = true;
    //    //    txtlbl8.Attributes.Remove("readonly");
    //    //    txtlbl9.Attributes.Remove("readonly");
    //    //}
    //    fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //--------------------------------------
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnsubmit.Disabled = false;
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
        lblheader.Text = "SDR FORM";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "scratch";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "ES");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        //typePopup = "N";
    }
    public void Next_No()
    {
        string count = "";
        Int64 i = 0;
        string SD = "Select max(VCHNUM) as vch from scratch where branchcd='" + frm_mbr.Substring(0, 2) + "' AND TYPE='ES' and VCHdate " + DateRange + "";
        count = fgen.seek_iname(frm_qstr, co_cd, "Select max(VCHNUM) as vch from scratch where branchcd='" + frm_mbr.Substring(0, 2) + "' AND TYPE='ES' and VCHdate " + DateRange + "", "vch").Trim();

        if (count == "" || count == "0")
        {
            i = 1;
        }
        else
        {
            i = Convert.ToInt64(count);
            i++;
        }
        string result = fgen.padlc(i, 6);
        txtsdrno.Text = result.ToString();
        txtDraft.Text = result.ToString();
    }

    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "BTN_10":
                break;
            case "BTN_11":
                break;
            case "BTN_12":
                break;
            case "BTN_13":
                break;
            case "BTN_14":
                break;
            case "BTN_15":
                break;
            case "BTN_16":
                break;
            case "BTN_17":
                break;
            case "BTN_18":
                break;
            case "BTN_19":
                break;
            case "SURE":
                SQuery = "Select 'YES' as col1,'Yes,Please' as Text,'Record Will be Deleted' as Action from dual union all Select 'NO' as col1,'No,Do Not' as Text,'Record Will Not be Deleted' as Action from dual";
                break;
            case "ST":
                SQuery = "Select 'S' as code,'Sales' as Text,'Status' as Action from dual union all Select 'R' as code,'RND' as Text,'Status' as Action from dual union all Select 'E' as code,'Excuted' as Text,'Status' as Action from dual union all Select 'C' as code,'Closed' as Text,'Status' as Action from dual";
                break;
            case "SURE_S":
            case "SURE_S1":
                SQuery = "Select 'YES' as col1,'Yes,Please' as Text,'Start Saving Now' as Action from dual union all Select 'No' as col1,'No,wish to change' as Text,'Please Change Now' as Action from dual ";
                break;
            case "Type":
                SQuery = "select 'Label' as fstr,'Liquid Paint' as Choice,'-' as s from dual union all select 'Powder' as fstr,'Powder Coating' as Choice,'-' as s from dual";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and trim(type1) not in (" + col1 + ")";
                }
                else
                {
                    col1 = "";
                }
                SQuery = "select type1 as fstr,name as proc_name,type1 as code from type where id='K' " + col1 + " order by code";
                break;
           
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD"||btnval=="Print_E")
          //    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Mapping_no,to_char(a.vchdate,'dd/mm/yyyy') as Map_Dt,b.IName as Product_Name,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,b.icode as ERP_Code,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,Item b where trim(A.icode)=trim(B.Icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                SQuery = "select distinct BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') as fstr, VCHNUM as DRAFT_No,TO_CHAr(VCHDATE,'DD/MM/YYYY') as DRAFT_Date,sdr_no,to_char(sdr_date,'dd/mm/yyyy') as sdr_date,type,col1 as Cust_Name,col2 as address,col48 as contact_person,col14 as telephone,col7 as email,prod_cat,TO_CHAR(VCHdate,'YYYYMMDD') AS VDD from scratch WHERE  branchcd='" + frm_mbr + "' AND type='" + frm_vty + "' and VCHDATE " + DateRange + " order by VDD desc ,VCHNUM desc";
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
      void Type_Sel_query()
      {
          Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
          switch (Prg_Id)
          {
              case "F10133":
                  SQuery = "SELECT '10' AS FSTR,'Process Mapping' as NAME,'10' AS CODE FROM dual";
                  break;
          }
      }
      protected void btnnew_ServerClick(object sender, EventArgs e)
      {
          chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
          clearctrl();
          if (chk_rights == "Y")
          {
              // if want to ask popup at the time of new            
              hffield.Value = "Type";//Type............
              if (typePopup == "N") newCase(frm_vty);
              else
              {
                  make_qry_4_popup();
                  fgen.Fn_open_sseek("-", frm_qstr);
                  enablectrl();//testing
              }         
          }
          else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
      }
      void newCase(string vty)
      {
          #region
          vty = "10";
          frm_vty = vty;
          fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
         // lbl1a.Text = vty;
          frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
         // txtvchnum.Text = frm_vnum;
         // txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
         // txtlbl2.Text = frm_uname;
         // txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        //  txtlbl5.Text = "-";
        //  txtlbl6.Text = "-";
          disablectrl();
          fgen.EnableForm(this.Controls);
          //btnlbl4.Focus();         
          fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
          hffield.Value = "NEW_E";
          #endregion
      }

      protected void btnedit_ServerClick(object sender, EventArgs e)
      {
          chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
          clearctrl();
          if (chk_rights == "Y")
          {
              hffield.Value = "Edit";
              typePopup = "N";
              make_qry_4_popup();
              fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
          }
          else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
      }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            typePopup = "N";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }

    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        typePopup = "N";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {      
        hffield.Value = "List";    
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
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
        lblUpload.Text = "";
     //   setColHeadings();
    }

    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        nflag = 0;
        nflag = (int)Date_check(nflag);
        if (nflag == 1) { }
        else
        {
            nflag = (int)Check_All_Fields(nflag);
            if (nflag == 1) { }
            else
            {
                fill_dash(this.Controls);               
                disablectrl();
                btnexit.Disabled = false;
                hffield.Value = "SURE_S1";
                lbledmode.Value = "";
                make_qry_4_popup();
                btnsubmit.Disabled = false;
                btnsave.Disabled = false;
                ScriptManager.RegisterStartupScript(btnsave, this.GetType(), "abc", "$(document).ready(function(){openSSeek2();});", true);
                fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                Request.Cookies["Column1"].Value = "Yes";
                btnhideF_S_Click(null, null);
            }
        }
    }

    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
       fgen.fill_dash(this.Controls);      
      
         if (txtcust.Text == "-" || txtcust.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please fill Customer Name !!");            
             return;
         }
         if (txtcontact.Text == "-" || txtcontact.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Contact No. !!");
             return;
         }
         if (txtaddr1.Text == "-" || txtaddr1.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Address !!");
             return;
         }
         if (txttel.Text == "-" || txttel.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Telephone No. !!");
             return;
         }
         if (txtemail.Text == "-" || txtemail.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Email!!");
             return;
         }
         if (txtnature.Text == "-" || txtnature.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Nature of Business!!");
             return;
         }
         if (txtjusti.Text == "-" || txtjusti.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Justification of Development!!");
             return;
         }
         if (txtproduct.Text == "-" || txtproduct.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Product Type!!");
             return;
         }
         if (txtshade.Text == "-" || txtshade.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Shade!!");
             return;
         }
         if (txtfinish.Text == "-" || txtfinish.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Finish!!");
             return;
         }
         if (txtgloss.Text == "-" || txtgloss.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill GLoss 20*/60*!!");
             return;
         }
         if (txtsubs.Text == "-" || txtsubs.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Substrate Type!!");
             return;
         }
         if (HFOPT.Value != "PC")
         {
             if (txtpre.Text == "-" || txtpre.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Pre-Treatment Type!!");
                 return;                 
             }

             if (txtmethod.Text == "-" || txtmethod.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Application Method!!");
                 return;
             }

             if (txtthinner.Text == "-" || txtthinner.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Application Viscosity!!");
                 return;
             }

             if (txtIntake.Text == "-" || txtIntake.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Thinner Intake Online!!");
                 return;
             }

             if (txtSpecific.Text == "-" || txtSpecific.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Specific Liquid Properties!!");
                 return;
             }

             if (txtdft.Text == "-" || txtdft.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill DFT STD Specified by Customer!!");
                 return;
             }
         }
         else
         {
             if (txtpre.Text == "-" || txtpre.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Pre-Treatment System & Type!!");
                 return;
             }
             if (txtdft.Text == "-" || txtdft.Text == "")
             {
                 fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Type of Oven!!");
                 return;
             }
         }
         if (txtsystem.Text == "-" || txtsystem.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Application System!!");
             return;
         }

         if (txtBanking.Text == "-" || txtBanking.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Banking Schedule!!");
             return;
         }

         if (txtsst.Text == "-" || txtsst.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill SST Required pls Specify HRS!!");
             return;
         }

         if (txtAccelerated.Text == "-" || txtAccelerated.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Accelerated Weathering Test!!");
             return;
         }

         if (txtAny.Text == "-" || txtAny.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Any Other Potential Test!!");
             return;
         }
         if (txtVolume.Text == "-" || txtVolume.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Volume of Total Business Potential of Customer!!");
             return;
         }

         if (txtValue.Text == "-" || txtValue.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Value of Total Business Potential of Customer!!");
             return;
         }

         if (txtimmid.Text == "-" || txtimmid.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Business Expected (Immediate)!!");
             return;
         }

         if (txtlong.Text == "-" || txtlong.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Business Expected (Long)!!");
             return;
         }

         if (txtFutureVol.Text == "-" || txtFutureVol.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Future Business Volume!!");
             return;
         }

         if (txtFutureVal.Text == "-" || txtFutureVal.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Future Business Value!!");
             return;
         }

         if (txt1.Text == "-" || txt1.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Present Supplier 1!!");
             return;
         }

         if (Textpr1.Text == "-" || Textpr1.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Present Supplier 1 Basic Price!!");
             return;
         }

         if (txt11.Text == "-" || txt11.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Present Supplier 2!!");
             return;
         }

         if (Textpr2.Text == "-" || Textpr2.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Present Supplier 2 Basic Price!!");
             return;
         }

         if (txtBasicPrice.Text == "-" || txtBasicPrice.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Basic Price at which Business Can be Obtained!!");
             return;
         }

         if (txtPymt.Text == "-" || txtPymt.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Payment Terms of Customer!!");
             return;
         }
         if (txtQty.Text == "-" || txtQty.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Qty!!");
             return;
         }

         if (txttime.Text == "-" || txttime.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Time Frame for Development!!");
             return;
         }

         if (txtSampleQty.Text == "-" || txtSampleQty.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Sample Qty Required For Trail!!");
             return;
         }

         if (txtSuggest.Text == "-" || txtSuggest.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Suggest Final Name of the Product!!");
             return;
         }

         if (txtaddition.Text == "-" || txtaddition.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Additional Information(If Any):!!");
             return;
         }

         if (txtreques.Text == "-" || txtreques.Text == "")
         {
             fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Requested By BDM Name!!");
             return;
         }
         if (Dept.Length >= 1)
         {
             if (Dept.Substring(0, 1) == "R")
             {
                 if (txtApproval.Text == "-" || txtApproval.Text == "")
                 {
                     fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill  Approval From R&D Head Date & Time !!");
                     return;
                 }

                 if (txtRefusal2.Text == "-" || txtRefusal2.Text == "")
                 {
                     fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Fill Remarks for Refusal!!");
                     return;
                 }
             }
         }
         disablectrl();
         lblname.Value = "SURE_S";

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }  

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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3")
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
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            else
            {
                //btnlbl4.Focus();
            }
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "N":
                    #region
                    if (sname.Length == 6)
                    {
                        fgen.ResetForm(this.Controls);
                        da = new OracleDataAdapter("Select branchcd, vchnum,col54,col55,ENQ_STATUS, col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2, PROD_cAT, to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59,col6,col7,col8,col9,col10,col12,col23,col48,col40,col41,col24,col26,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') ='" + scode + "'", consql);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtbranch.Text = dt.Rows[0]["branchcd"].ToString().Trim();
                            txtenqno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                            txtreques.Text = dt.Rows[0]["col3"].ToString().Trim();
                            txtcontact.Text = dt.Rows[0]["col48"].ToString().Trim();
                            txtcust.Text = dt.Rows[0]["col1"].ToString().Trim();
                            txtaddr1.Text = dt.Rows[0]["col2"].ToString().Trim();
                            txtaddr2.Text = dt.Rows[0]["col11"].ToString().Trim();
                            txtaddr3.Text = dt.Rows[0]["col5"].ToString().Trim();
                            txttel.Text = dt.Rows[0]["col14txtSuggest"].ToString().Trim();
                            txtfax.Text = dt.Rows[0]["col6"].ToString().Trim();
                            txtemail.Text = dt.Rows[0]["col7"].ToString().Trim();
                            txtnature.Text = dt.Rows[0]["col8"].ToString().Trim();
                            txtjusti.Text = dt.Rows[0]["col9"].ToString().Trim();
                            txtproduct.Text = dt.Rows[0]["col10"].ToString().Trim();
                            txtshade.Text = dt.Rows[0]["col12"].ToString().Trim();
                            txtrecomm.Text = dt.Rows[0]["col21"].ToString().Trim();
                            rbcust.SelectedValue = dt.Rows[0]["col23"].ToString().Trim();
                            rbcust0.SelectedValue = dt.Rows[0]["col24"].ToString().Trim();
                            rbcust1.SelectedValue = dt.Rows[0]["col26"].ToString().Trim();
                            txtsolid.Text = dt.Rows[0]["col56"].ToString().Trim();
                            HFOPT.Value = dt.Rows[0]["PROD_cAT"].ToString().Trim();
                            txtbusin.Text = dt.Rows[0]["col57"].ToString().Trim();
                            txtthinner.Text = dt.Rows[0]["col15"].ToString().Trim();
                            string strprice = dt.Rows[0]["col55"].ToString().Trim();
                            if (strprice == "BP") strprice = "0";
                            if (strprice == "SP") strprice = "1";
                            rbcust3.SelectedValue = strprice;
                            string strstatus = dt.Rows[0]["ENQ_STATUS"].ToString().Trim();
                            if (strstatus == "NEW") strstatus = "0";
                            if (strstatus == "EXISTING") strstatus = "1";
                            rbdeve.SelectedValue = strstatus;
                            Textpr1.Text = dt.Rows[0]["num1"].ToString().Trim();
                            Textpr2.Text = dt.Rows[0]["num2"].ToString().Trim();
                            txtsystem.Text = dt.Rows[0]["col18"].ToString().Trim();
                            if (HFOPT.Value == "LP")
                            {
                                txtdft.Text = dt.Rows[0]["col16"].ToString().Trim();
                                txtsst.Text = dt.Rows[0]["col17"].ToString().Trim();
                                txthrs.Text = dt.Rows[0]["col19"].ToString().Trim();
                                lblhead.Text = "Liquid Paint Division";
                                lblbrh.Visible = false; lblbrno.Visible = false;
                                lblgloss.Visible = false; txtgloss.Visible = false;
                                lblsalt.Visible = false; txtsalt.Visible = false;
                                lbltest.Visible = false; txttest.Visible = false;
                                //lblthinner.Text = "Application Viscosity/Thinner Intake";
                                //lblsystem.Text = "Application System(Include DFT,Flash Off Time,Baking Schedule):";
                                //lblsolid.Text = "Solid/Metallic/Candy/Other";
                                lbldft.Visible = true; txtdft.Visible = true;
                                lblsst.Visible = true; txtsst.Visible = true;
                                lblhrs.Visible = true; txthrs.Visible = true;
                                lblliquid.Visible = true; txtliquid.Visible = true;
                                lbldry.Visible = true; txtdry.Visible = true;
                            }
                            if (HFOPT.Value == "PC")
                            {
                                txtgloss.Text = dt.Rows[0]["col16"].ToString().Trim();
                                txtsalt.Text = dt.Rows[0]["col17"].ToString().Trim();
                                txttest.Text = dt.Rows[0]["col19"].ToString().Trim();
                                lblhead.Text = "Powder Coating Division";
                                lblbrh.Visible = true; lblbrno.Visible = true;
                                lblgloss.Visible = true; txtgloss.Visible = true;
                                lblsalt.Visible = true; txtsalt.Visible = true;
                                lbltest.Visible = true; txttest.Visible = true;
                                lblthinner.Text = "Type of Oven and Baking Schedule";
                                lblsystem.Text = "Application System:";
                                lblsolid.Text = "Requirement Desired";
                                lbldft.Visible = false; txtdft.Visible = false;
                                lblsst.Visible = false; txtsst.Visible = false;
                                lblhrs.Visible = false; txthrs.Visible = false;
                                lblliquid.Visible = false; txtliquid.Visible = false;
                                lbldry.Visible = false; txtdry.Visible = false;
                            }
                            txtfinish.Text = dt.Rows[0]["col28"].ToString().Trim();
                            txtsubs.Text = dt.Rows[0]["col27"].ToString().Trim();
                            txtpre.Text = dt.Rows[0]["col40"].ToString().Trim();
                            txtmethod.Text = dt.Rows[0]["col41"].ToString().Trim();
                            txtliquid.Text = dt.Rows[0]["col22"].ToString().Trim();
                            txtdry.Text = dt.Rows[0]["col35"].ToString().Trim();
                            txtimmid.Text = dt.Rows[0]["col37"].ToString().Trim();
                            txtlong.Text = dt.Rows[0]["col39"].ToString().Trim();
                            txt1.Text = dt.Rows[0]["col25"].ToString().Trim();
                            txt11.Text = dt.Rows[0]["col20"].ToString().Trim();
                            txttime.Text = dt.Rows[0]["col13"].ToString().Trim();
                            txtaddition.Text = dt.Rows[0]["col59"].ToString().Trim();
                            txtpsample.Text = dt.Rows[0]["col54"].ToString().Trim();
                            txtbasic.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        }
                        fgen.EnableForm(this.Controls);
                       // enable_btn();
                        enablectrl();
                        rbcust.Enabled = false;
                        rbcust0.Enabled = false;
                        rbcust1.Enabled = false;
                        rbspeci.Enabled = false;
                        txtdate1.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
                        txtsign.Text = txtdate1.Text;
                        txtmddate.Text = txtdate1.Text;
                        txthmdate.Text = txtdate1.Text;
                        btnnew.Disabled = true;
                        btnedit.Disabled = true;
                        btndel.Disabled = true;
                        btnprint.Disabled = true;
                        btnlist.Disabled = true;
                       // btnexit.Text = "Cancel";
                        Next_No();
                    }
                    #endregion
                    break;
                case "Y":
                case"Edit_E":
                    #region
                    sname = col2;
                    scode = col1;
                    mq1 = "";
                    if (sname.Length == 6)
                    {
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr,co_cd, "select sdr_no,sdr_date from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + scode + "'");
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["sdr_no"].ToString() != " ")
                            {
                                if (frm_ulvl != "0" && frm_ulvl != "1")
                                {//Dear " + frm_uname + ",You do not have rights to add new entry for this form!!
                                    //fgen.msg("-", "AMSG", "Dear " + uname + " , You do not have rights for Editing."); return;
                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Dear " + frm_uname + " , This Entry Cannot Be Edited.','Tejaxo ERP Alert Message');});</script>", false);
                                }
                                #region Allowed
                                else
                                {
                                    dt = new DataTable();
                                    // mq1 = "Select a.acode,a.branchcd,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,NVL(COL4,'-') AS COL4, ebr,col55,nvl(PROD_cAT,'LP') as PROD_cAT,col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,  invno,to_char(invdate,'dd/mm/yyyy') as invdate, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,col1,a.col2,a.col3,col21,a.col11,a.col5,col59,a.col6,a.col7,a.col8,a.col9,a.col10,col12,col23,col24,col26,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,remarks,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,TO_CHAr(SYSDATE,'DD/MM/YYYY')) AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,b.acode as name from scratch a,inspmst b where trim(a.col30)=trim(b.acode) and a.BRANCHCD||a.TYPE||TRIM(a.VCHNUM)||TO_CHAr(a.VCHDATE,'DD/MM/YYYY') ='" + scode + "'";
                                   // da = new OracleDataAdapter("Select acode,EMAIL_ID,branchcd,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,NVL(COL4,'-') AS COL4, ebr,col55,nvl(PROD_cAT,'LP') as PROD_cAT,col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,  invno,to_char(invdate,'dd/mm/yyyy') as invdate, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54, vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59,col6,col7,col8,col9,col10,col12,col23,col24,col26,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,remarks,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,TO_CHAr(SYSDATE,'DD/MM/YYYY')) AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,APP_BY,TO_CHAR(APP_DT,'DD/MM/YYYY') AS APP_DT from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + scode + "' ORDER BY COL30", consql);
                                    SQuery = "Select acode,EMAIL_ID,branchcd,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,NVL(COL4,'-') AS COL4, ebr,col55,nvl(PROD_cAT,'LP') as PROD_cAT,col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,  invno,to_char(invdate,'dd/mm/yyyy') as invdate, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54, vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59,col6,col7,col8,col9,col10,col12,col23,col24,col26,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,remarks,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,TO_CHAr(SYSDATE,'DD/MM/YYYY')) AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,APP_BY,TO_CHAR(APP_DT,'DD/MM/YYYY') AS APP_DT,filepath,filename from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + scode + "' ORDER BY COL30";
                                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);                                    
                                    if (dt.Rows.Count > 0)
                                    {
                                        txtbranch.Text = fgen.seek_iname(frm_qstr,co_cd, "SELECT NAME FROM TYPE WHERE ID='B' AND TYPE1='" + dt.Rows[0]["BRANCHCD"].ToString().Trim() + "'", "NAME");
                                        txtUserid.Text = dt.Rows[0]["acode"].ToString().Trim();
                                        txtUserName.Text = fgen.seek_iname(frm_qstr,co_cd, "select username from evas where userid='" + txtUserid.Text.Trim() + "'", "username");
                                        txtenqno.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                                        txtdate.Text = dt.Rows[0]["INVDATE"].ToString().Trim();
                                        txtDraft.Text = dt.Rows[0]["VCHNUM"].ToString().Trim();
                                        txtDraftDt.Text = dt.Rows[0]["VCHDATE"].ToString().Trim();
                                        txtsdrno.Text = dt.Rows[0]["sdr_no"].ToString().Trim();
                                        txtdate1.Text = dt.Rows[0]["sdr_date"].ToString().Trim();
                                        txtSDR2.Text = dt.Rows[0]["sdr_no"].ToString().Trim();
                                        txtSDR2Date.Text = dt.Rows[0]["sdr_date"].ToString().Trim();
                                        HFOLDDT.Value = txtdate1.Text;
                                        txtreques.Text = dt.Rows[0]["col3"].ToString().Trim();
                                        txtcontact.Text = dt.Rows[0]["col48"].ToString().Trim();
                                        txtpre.Text = dt.Rows[0]["col85"].ToString().Trim();
                                        txtmethod.Text = dt.Rows[0]["col41"].ToString().Trim();
                                        txtpsample.Text = dt.Rows[0]["col54"].ToString().Trim();
                                        txtcust.Text = dt.Rows[0]["col1"].ToString().Trim();
                                        txtCust2.Text = dt.Rows[0]["col1"].ToString().Trim();
                                        txtaddr1.Text = dt.Rows[0]["col2"].ToString().Trim();
                                        txtaddr2.Text = dt.Rows[0]["col11"].ToString().Trim();
                                        txtaddr3.Text = dt.Rows[0]["col5"].ToString().Trim();
                                        txttel.Text = dt.Rows[0]["col14"].ToString().Trim();
                                        txtfax.Text = dt.Rows[0]["col6"].ToString().Trim();
                                        txtemail.Text = dt.Rows[0]["col7"].ToString().Trim();
                                        txtnature.Text = dt.Rows[0]["col8"].ToString().Trim();
                                        txtjusti.Text = dt.Rows[0]["col9"].ToString().Trim();
                                        txtproduct.Text = dt.Rows[0]["col10"].ToString().Trim();
                                        txtshade.Text = dt.Rows[0]["col12"].ToString().Trim();
                                        txtrecomm.Text = dt.Rows[0]["col21"].ToString().Trim();
                                        rbcust.SelectedValue = dt.Rows[0]["col23"].ToString().Trim();
                                        rbcust0.SelectedValue = dt.Rows[0]["col24"].ToString().Trim();
                                        rbcust1.SelectedValue = dt.Rows[0]["col26"].ToString().Trim();
                                        txtsolid.Text = dt.Rows[0]["col56"].ToString().Trim();
                                        txtbusin.Text = dt.Rows[0]["col57"].ToString().Trim();
                                        txtthinner.Text = dt.Rows[0]["col15"].ToString().Trim();
                                        txtsystem.Text = dt.Rows[0]["col18"].ToString().Trim();
                                        //HFOPT.Value = fgen.seek_iname(co_cd, "Select nvl(PROD_cAT,'LP') as PROD_cAT from scratch where type='EQ' and vchnum='" + txtenqno.Text + "' and to_DatE(to_char(VCHDATE,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + txtdate.Text + "','dd/MM/yyyy')", "prod_Cat");
                                        HFOPT.Value = dt.Rows[0]["PROD_CAT"].ToString().Trim();
                                        if (HFOPT.Value == "LP")
                                        {
                                            txtdft.Text = dt.Rows[0]["col16"].ToString().Trim();
                                            txtsst.Text = dt.Rows[0]["col17"].ToString().Trim();
                                            txtgloss.Text = dt.Rows[0]["col19"].ToString().Trim();
                                            lblhead.Text = "Liquid Paint Division";
                                            lblProduct.Text = "Product Type (ST/AD/TSA/2KPU/2U/1K Epoxy/Thinner/Other)";
                                            lblShade.Text = "Shade (Solid/ Metallic/Candy/Other)";
                                            lblPre.Text = "Pre-Treatment Type";
                                            lblsystem.Text = "Application System Flash off Time Between Primer and Paint";
                                            lbldft.Text = "DFT STD Specified by Customer(For Composites System, Each Coat)";
                                            lblSampleQty.Text = "Sample Qty Required For Trail (Standard 1- Ltr.)for Extra Qty of Sample Pls Specify";
                                            lblIntake.Visible = true;
                                            lblthinner.Visible = true;
                                            lblMethod.Visible = true;
                                            txtIntake.Visible = true;
                                            txtthinner.Visible = true;
                                            txtmethod.Visible = true;
                                            lblSpecific.Visible = true;
                                            txtSpecific.Visible = true;
                                            lblbasic.Visible = true;
                                            txtbasic.Visible = true;
                                            lblCust3.Visible = true;
                                            rbcust3.Visible = true;
                                        }
                                        if (HFOPT.Value == "PC")
                                        {
                                            txtdft.Text = dt.Rows[0]["col16"].ToString().Trim();
                                            txtsst.Text = dt.Rows[0]["col17"].ToString().Trim();
                                            txtgloss.Text = dt.Rows[0]["col19"].ToString().Trim();
                                            lblhead.Text = "Powder Coating Division";
                                            lblProduct.Text = "Product Type (PP/EP/Others)";
                                            lblShade.Text = "Shade (Solid/ Metallic/Trans/Other)";
                                            lblPre.Text = "Pre-Treatment System & Type";
                                            lblsystem.Text = "Application System";
                                            lbldft.Text = "Type of Oven";
                                            lblSampleQty.Text = "Sample Qty Required For Trail (Standard - 250 Grams)for Extra Qty of Sample Pls Specify";
                                            lblIntake.Visible = false;
                                            lblthinner.Visible = false;
                                            lblMethod.Visible = false;
                                            txtIntake.Visible = false;
                                            txtthinner.Visible = false;
                                            txtmethod.Visible = false;
                                            lblSpecific.Visible = false;
                                            txtSpecific.Visible = false;
                                            lblbasic.Visible = false;
                                            txtbasic.Visible = false;
                                            lblCust3.Visible = false;
                                            rbcust3.Visible = false;
                                        }
                                        if (txtAttch.Text.Length > 1)
                                        {
                                        lblUpload.Text = dt.Rows[0]["filepath"].ToString().Trim();
                                        mq1 = dt.Rows[0]["filename"].ToString().Trim().Split('~')[1];
                                        txtAttch.Text = mq1;                                          
                                        }
                                        else if (dt.Rows[0]["filepath"].ToString().Trim().Length>1)
                                        {
                                           lblUpload.Text = dt.Rows[0]["filepath"].ToString().Trim();
                                            txtAttch.Text = dt.Rows[0]["filename"].ToString().Trim();                                         
                                        }
                                        txtfinish.Text = dt.Rows[0]["col28"].ToString().Trim();
                                        txtsubs.Text = dt.Rows[0]["col27"].ToString().Trim();
                                        txtliquid.Text = dt.Rows[0]["col22"].ToString().Trim();
                                        txtdry.Text = dt.Rows[0]["col35"].ToString().Trim();
                                        txtimmid.Text = dt.Rows[0]["col37"].ToString().Trim();
                                        txtlong.Text = dt.Rows[0]["col39"].ToString().Trim();
                                        txt1.Text = dt.Rows[0]["col25"].ToString().Trim();
                                        txt11.Text = dt.Rows[0]["col20"].ToString().Trim();
                                        txttime.Text = dt.Rows[0]["col13"].ToString().Trim();
                                        txtaddition.Text = dt.Rows[0]["col59"].ToString().Trim();
                                        //  txtbasic.Text = dt.Rows[0]["remarks"].ToString().Trim();
                                        //txtprod.Text = dt.Rows[0]["col30"].ToString().Trim();
                                        //txtdpperiod.Text = dt.Rows[0]["col31"].ToString().Trim();
                                        //txtdpcost.Text = dt.Rows[0]["col32"].ToString().Trim();
                                        //txtsample.Text = dt.Rows[0]["col33"].ToString().Trim();
                                        //txtfordp.Text = dt.Rows[0]["col34"].ToString().Trim();
                                        //txtforreg.Text = dt.Rows[0]["col36"].ToString().Trim();
                                        txtfortest.Text = dt.Rows[0]["col38"].ToString().Trim();
                                        txtremark.Text = dt.Rows[0]["col40"].ToString().Trim();
                                        string strval = dt.Rows[0]["col52"].ToString().Trim();
                                        if (strval == "0" || strval == "1") { }
                                        else strval = "0";
                                        rbminor.SelectedValue = strval;
                                        //  txtprodcode.Text = dt.Rows[0]["col53"].ToString().Trim();
                                        txthead.Text = dt.Rows[0]["col43"].ToString().Trim();
                                        txtsign.Text = dt.Rows[0]["docdate"].ToString().Trim();
                                        txti.Text = dt.Rows[0]["col44"].ToString().Trim();
                                        txtii.Text = dt.Rows[0]["col45"].ToString().Trim();
                                        txtiii.Text = dt.Rows[0]["col46"].ToString().Trim();
                                        txtmddate.Text = dt.Rows[0]["col47"].ToString().Trim();
                                        // txtfeedback.Text = dt.Rows[0]["col51"].ToString().Trim();
                                        if (dt.Rows[0]["EMAIL_ID"].ToString().Trim() == "") { rbspeci.SelectedValue = "1"; }
                                        else
                                        {
                                            rbspeci.SelectedValue = dt.Rows[0]["EMAIL_ID"].ToString().Trim().Replace("-", "1").Replace(" ", "1");
                                        }
                                        txtAllAttachments.Text = dt.Rows[0]["col51"].ToString(); hf2.Value = dt.Rows[0]["remarks"].ToString();
                                        if (dt.Rows[0]["email_id"].ToString() == "0")
                                        {
                                            Attch.Visible = true;
                                            txtAllAttachments.Visible = true;
                                        }
                                        txtclosed.Text = dt.Rows[0]["col49"].ToString().Trim();
                                        txthmdate.Text = dt.Rows[0]["col50"].ToString().Trim();
                                        rbcust3.SelectedValue = dt.Rows[0]["col55"].ToString().Trim();
                                        Textpr1.Text = dt.Rows[0]["num1"].ToString().Trim();
                                        Textpr2.Text = dt.Rows[0]["num2"].ToString().Trim();
                                        txtprodname.Text = dt.Rows[0]["PROD_NAME"].ToString().Trim();
                                        //txtstatus.Text = dt.Rows[0]["HO_STATUS"].ToString().Trim();
                                        txtstatus.Text = dt.Rows[0]["COL4"].ToString().Trim();
                                        txtIntake.Text = dt.Rows[0]["COL60"].ToString().Trim();
                                        txtBanking.Text = dt.Rows[0]["COL61"].ToString().Trim();
                                        txtSpecific.Text = dt.Rows[0]["COL62"].ToString().Trim();
                                        txtAccelerated.Text = dt.Rows[0]["COL63"].ToString().Trim();
                                        txtAny.Text = dt.Rows[0]["COL64"].ToString().Trim();
                                        txtVolume.Text = dt.Rows[0]["COL65"].ToString().Trim();
                                        txtValue.Text = dt.Rows[0]["COL66"].ToString().Trim();
                                        txtFutureVol.Text = dt.Rows[0]["COL67"].ToString().Trim();
                                        txtFutureVal.Text = dt.Rows[0]["COL68"].ToString().Trim();
                                        txtBasicPrice.Text = dt.Rows[0]["COL69"].ToString().Trim();
                                        txtPymt.Text = dt.Rows[0]["COL70"].ToString().Trim();
                                        txtQty.Text = dt.Rows[0]["COL71"].ToString().Trim();
                                        txtSampleQty.Text = dt.Rows[0]["COL72"].ToString().Trim();
                                        txtSuggest.Text = dt.Rows[0]["COL73"].ToString().Trim();
                                        txtApproval.Text = dt.Rows[0]["COL74"].ToString().Trim();
                                        // txtDateTime.Text = dt.Rows[0]["COL75"].ToString().Trim();
                                        txtRefusal1.Text = dt.Rows[0]["COL76"].ToString().Trim();
                                        txtRefusal2.Text = dt.Rows[0]["COL77"].ToString().Trim();
                                        txtRegular.Text = dt.Rows[0]["COL78"].ToString().Trim();
                                        txtCostSheet.Text = dt.Rows[0]["COL79"].ToString().Trim();
                                        // txtSampleSize.Text = dt.Rows[0]["COL80"].ToString().Trim();
                                        txtEstimated.Text = dt.Rows[0]["COL81"].ToString().Trim();
                                        txtFormat.Text = dt.Rows[0]["COL82"].ToString().Trim();
                                        txtRev.Text = dt.Rows[0]["COL83"].ToString().Trim();
                                        txtEffDate.Text = dt.Rows[0]["COL84"].ToString().Trim();
                                        txtiiii.Text = dt.Rows[0]["COL86"].ToString().Trim();
                                        txtAppAdd.Text = dt.Rows[0]["COL87"].ToString().Trim();
                                        txtfeedback.Text = dt.Rows[0]["col80"].ToString().Trim();
                                        txtFeasDoc.Text = dt.Rows[0]["col32"].ToString().Trim();
                                        txtFeasRev.Text = dt.Rows[0]["col33"].ToString().Trim();
                                        txtFeasDate.Text = dt.Rows[0]["col34"].ToString().Trim();
                                        txtclosed2.Text = dt.Rows[0]["col36"].ToString().Trim();
                                    }
                                    create_tab();
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        dr1 = dt1.NewRow();
                                        dr1["srno"] = dt1.Rows.Count + 1;
                                        dr1["code"] = dt.Rows[i]["col30"].ToString().Trim();
                                        dr1["name"] = fgen.seek_iname(frm_qstr,co_cd, "select col1 from inspmst where type='SF' and acode='" + dt.Rows[i]["col30"].ToString().Trim() + "'", "col1");
                                        dr1["yes"] = dt.Rows[i]["col53"].ToString().Trim();
                                        dr1["remarks"] = dt.Rows[i]["col31"].ToString().Trim();
                                        dt1.Rows.Add(dr1);
                                    }
                                    ViewState["sg1"] = dt1;
                                    sg1.DataSource = dt1;
                                    sg1.DataBind();
                                    Sedtby = dt.Rows[0]["ent_by"].ToString().Trim();
                                    Sedt = dt.Rows[0]["ent_dt"].ToString().Trim();
                                    ViewState["ENTBY"] = Sedtby;
                                    ViewState["ENTDT"] = Sedt;
                                    ViewState["APP_BY"] = dt.Rows[0]["APP_BY"].ToString().Trim();
                                    ViewState["APP_DT"] = dt.Rows[0]["APP_DT"].ToString().Trim();
                                    fgen.EnableForm(this.Controls);
                                    btnexit.InnerText = "Cancel";//uncommnet
                                    lbledmode.Value = "Y";                                  
                                    MultiView1.ActiveViewIndex = 0;
                                    fgen.EnableForm(this.Controls);
                                    disablectrl();
                                    setColHeadings();
                                    edmode.Value = "Y";
                                }
                                #endregion
                            }
                            else
                            {
                                #region
                                dt = new DataTable();
                                //da = new OracleDataAdapter("Select acode,EMAIL_ID,branchcd,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,NVL(COL4,'-') AS COL4, ebr,col55,nvl(PROD_cAT,'LP') as PROD_cAT,col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,  invno,to_char(invdate,'dd/mm/yyyy') as invdate, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54, vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59,col6,col7,col8,col9,col10,col12,col23,col24,col26,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,remarks,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,TO_CHAr(SYSDATE,'DD/MM/YYYY')) AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,APP_BY,TO_CHAR(APP_DT,'DD/MM/YYYY') AS APP_DT from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + scode + "' ORDER BY COL30", consql);
                                SQuery = "Select acode,EMAIL_ID,branchcd,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,NVL(COL4,'-') AS COL4, ebr,col55,nvl(PROD_cAT,'LP') as PROD_cAT,col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,  invno,to_char(invdate,'dd/mm/yyyy') as invdate, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54, vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59,col6,col7,col8,col9,col10,col12,col23,col24,col26,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,remarks,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,TO_CHAr(SYSDATE,'DD/MM/YYYY')) AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,APP_BY,TO_CHAR(APP_DT,'DD/MM/YYYY') AS APP_DT,filepath,filename from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + scode + "' ORDER BY COL30";
                                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                if (dt.Rows.Count > 0)
                                {
                                    //txtbranch.Text = dt.Rows[0]["ebr"].ToString().Trim();
                                    txtbranch.Text = fgen.seek_iname(frm_qstr,co_cd, "SELECT NAME FROM TYPE WHERE ID='B' AND TYPE1='" + dt.Rows[0]["BRANCHCD"].ToString().Trim() + "'", "NAME");
                                    txtUserid.Text = dt.Rows[0]["acode"].ToString().Trim();
                                    txtUserName.Text = fgen.seek_iname(frm_qstr,co_cd, "select username from evas where userid='" + txtUserid.Text.Trim() + "'", "username");
                                    txtenqno.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                                    txtdate.Text = dt.Rows[0]["INVDATE"].ToString().Trim();
                                    txtDraft.Text = dt.Rows[0]["VCHNUM"].ToString().Trim();
                                    txtDraftDt.Text = dt.Rows[0]["VCHDATE"].ToString().Trim();
                                    txtsdrno.Text = dt.Rows[0]["sdr_no"].ToString().Trim();
                                    txtSDR2.Text = dt.Rows[0]["sdr_no"].ToString().Trim();
                                    txtSDR2Date.Text = dt.Rows[0]["sdr_date"].ToString().Trim();
                                    txtdate1.Text = dt.Rows[0]["sdr_date"].ToString().Trim();
                                    HFOLDDT.Value = txtdate1.Text;
                                    txtreques.Text = dt.Rows[0]["col3"].ToString().Trim();
                                    txtcontact.Text = dt.Rows[0]["col48"].ToString().Trim();
                                    txtpre.Text = dt.Rows[0]["col85"].ToString().Trim();
                                    txtmethod.Text = dt.Rows[0]["col41"].ToString().Trim();
                                    txtpsample.Text = dt.Rows[0]["col54"].ToString().Trim();
                                    txtcust.Text = dt.Rows[0]["col1"].ToString().Trim();
                                    txtCust2.Text = dt.Rows[0]["col1"].ToString().Trim();
                                    txtaddr1.Text = dt.Rows[0]["col2"].ToString().Trim();
                                    txtaddr2.Text = dt.Rows[0]["col11"].ToString().Trim();
                                    txtaddr3.Text = dt.Rows[0]["col5"].ToString().Trim();
                                    txttel.Text = dt.Rows[0]["col14"].ToString().Trim();
                                    txtfax.Text = dt.Rows[0]["col6"].ToString().Trim();
                                    txtemail.Text = dt.Rows[0]["col7"].ToString().Trim();
                                    txtnature.Text = dt.Rows[0]["col8"].ToString().Trim();
                                    txtjusti.Text = dt.Rows[0]["col9"].ToString().Trim();
                                    txtproduct.Text = dt.Rows[0]["col10"].ToString().Trim();
                                    txtshade.Text = dt.Rows[0]["col12"].ToString().Trim();
                                    txtrecomm.Text = dt.Rows[0]["col21"].ToString().Trim();
                                    rbcust.SelectedValue = dt.Rows[0]["col23"].ToString().Trim();
                                    rbcust0.SelectedValue = dt.Rows[0]["col24"].ToString().Trim();
                                    rbcust1.SelectedValue = dt.Rows[0]["col26"].ToString().Trim();
                                    txtsolid.Text = dt.Rows[0]["col56"].ToString().Trim();
                                    txtbusin.Text = dt.Rows[0]["col57"].ToString().Trim();
                                    txtthinner.Text = dt.Rows[0]["col15"].ToString().Trim();
                                    txtsystem.Text = dt.Rows[0]["col18"].ToString().Trim();
                                    //HFOPT.Value = fgen.seek_iname(co_cd, "Select nvl(PROD_cAT,'LP') as PROD_cAT from scratch where type='EQ' and vchnum='" + txtenqno.Text + "' and to_DatE(to_char(VCHDATE,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + txtdate.Text + "','dd/MM/yyyy')", "prod_Cat");
                                    HFOPT.Value = dt.Rows[0]["PROD_CAT"].ToString().Trim();
                                    if (HFOPT.Value == "LP")
                                    {
                                        //txthrs.Text = dt.Rows[0]["col19"].ToString().Trim();
                                        //lblbrh.Visible = false; lblbrno.Visible = false;
                                        // lblgloss.Visible = false; txtgloss.Visible = false;
                                        //lblsalt.Visible = false; txtsalt.Visible = false;
                                        //lbltest.Visible = false; txttest.Visible = false;
                                        //lblthinner.Text = "Application Viscosity/Thinner Intake";
                                        //lblsystem.Text = "Application System(Include DFT,Flash Off Time,Baking Schedule):";
                                        //lblsolid.Text = "Solid/Metallic/Candy/Other";
                                        //  lbldft.Visible = true; txtdft.Visible = true;
                                        //lblsst.Visible = true; txtsst.Visible = true;
                                        //lblhrs.Visible = true; txthrs.Visible = true;
                                        //lblliquid.Visible = true; txtliquid.Visible = true;
                                        //lbldry.Visible = true; txtdry.Visible = true;
                                        txtdft.Text = dt.Rows[0]["col16"].ToString().Trim();
                                        txtsst.Text = dt.Rows[0]["col17"].ToString().Trim();
                                        txtgloss.Text = dt.Rows[0]["col19"].ToString().Trim();
                                        lblhead.Text = "Liquid Paint Division";
                                        lblProduct.Text = "Product Type (ST/AD/TSA/2KPU/2U/1K Epoxy/Thinner/Other)";
                                        lblShade.Text = "Shade (Solid/ Metallic/Candy/Other)";
                                        lblPre.Text = "Pre-Treatment Type";
                                        lblsystem.Text = "Application System Flash off Time Between Primer and Paint";
                                        lbldft.Text = "DFT STD Specified by Customer(For Composites System, Each Coat)";
                                        lblSampleQty.Text = "Sample Qty Required For Trail (Standard 1- Ltr.)for Extra Qty of Sample Pls Specify";
                                        lblIntake.Visible = true;
                                        lblthinner.Visible = true;
                                        lblMethod.Visible = true;
                                        txtIntake.Visible = true;
                                        txtthinner.Visible = true;
                                        txtmethod.Visible = true;
                                        lblSpecific.Visible = true;
                                        txtSpecific.Visible = true;
                                        lblbasic.Visible = true;
                                        txtbasic.Visible = true;
                                        lblCust3.Visible = true;
                                        rbcust3.Visible = true;
                                    }
                                    if (HFOPT.Value == "PC")
                                    {
                                        //txtgloss.Text = dt.Rows[0]["col16"].ToString().Trim();
                                        //txtsalt.Text = dt.Rows[0]["col17"].ToString().Trim();
                                        //txtsalt.Text = dt.Rows[0]["col19"].ToString().Trim();
                                        //lblbrh.Visible = true; lblbrno.Visible = true;
                                        // lblgloss.Visible = true; txtgloss.Visible = true;
                                        //lblsalt.Visible = true; txtsalt.Visible = true;
                                        //lbltest.Visible = true; txttest.Visible = true;
                                        //lblthinner.Text = "Type of Oven and Baking Schedule";
                                        //lblsystem.Text = "Application System:";
                                        //lblsolid.Text = "Requirement Desired";
                                        //lbldft.Visible = false; txtdft.Visible = false;
                                        // lblsst.Visible = false; txtsst.Visible = false;
                                        //lblhrs.Visible = false; txthrs.Visible = false;
                                        //lblliquid.Visible = false; txtliquid.Visible = false;
                                        //lbldry.Visible = false; txtdry.Visible = false;
                                        txtdft.Text = dt.Rows[0]["col16"].ToString().Trim();
                                        txtsst.Text = dt.Rows[0]["col17"].ToString().Trim();
                                        txtgloss.Text = dt.Rows[0]["col19"].ToString().Trim();
                                        lblhead.Text = "Powder Coating Division";
                                        lblProduct.Text = "Product Type (PP/EP/Others)";
                                        lblShade.Text = "Shade (Solid/ Metallic/Trans/Other)";
                                        lblPre.Text = "Pre-Treatment System & Type";
                                        lblsystem.Text = "Application System";
                                        lbldft.Text = "Type of Oven";
                                        lblSampleQty.Text = "Sample Qty Required For Trail (Standard - 250 Grams)for Extra Qty of Sample Pls Specify";
                                        lblIntake.Visible = false;
                                        lblthinner.Visible = false;
                                        lblMethod.Visible = false;
                                        txtIntake.Visible = false;
                                        txtthinner.Visible = false;
                                        txtmethod.Visible = false;
                                        lblSpecific.Visible = false;
                                        txtSpecific.Visible = false;
                                        lblbasic.Visible = false;
                                        txtbasic.Visible = false;
                                        lblCust3.Visible = false;
                                        rbcust3.Visible = false;
                                    }
                                    if (txtAttch.Text.Length > 1)
                                    {
                                        lblUpload.Text = dt.Rows[0]["filepath"].ToString().Trim();
                                        mq1 = dt.Rows[0]["filename"].ToString().Trim().Split('~')[1];
                                        txtAttch.Text = mq1;           
                                    }
                                    else if (dt.Rows[0]["filepath"].ToString().Trim().Length > 1)
                                    {
                                        lblUpload.Text = dt.Rows[0]["filepath"].ToString().Trim();
                                        txtAttch.Text = dt.Rows[0]["filename"].ToString().Trim();
                                    }
                                    txtfinish.Text = dt.Rows[0]["col28"].ToString().Trim();
                                    txtsubs.Text = dt.Rows[0]["col27"].ToString().Trim();
                                    txtliquid.Text = dt.Rows[0]["col22"].ToString().Trim();
                                    txtdry.Text = dt.Rows[0]["col35"].ToString().Trim();
                                    txtimmid.Text = dt.Rows[0]["col37"].ToString().Trim();
                                    txtlong.Text = dt.Rows[0]["col39"].ToString().Trim();
                                    txt1.Text = dt.Rows[0]["col25"].ToString().Trim();
                                    txt11.Text = dt.Rows[0]["col20"].ToString().Trim();
                                    txttime.Text = dt.Rows[0]["col13"].ToString().Trim();
                                    txtaddition.Text = dt.Rows[0]["col59"].ToString().Trim();
                                    // txtbasic.Text = dt.Rows[0]["remarks"].ToString().Trim();
                                    //txtprod.Text = dt.Rows[0]["col30"].ToString().Trim();
                                    //txtdpperiod.Text = dt.Rows[0]["col31"].ToString().Trim();
                                    //txtdpcost.Text = dt.Rows[0]["col32"].ToString().Trim();
                                    //txtsample.Text = dt.Rows[0]["col33"].ToString().Trim();
                                    //txtfordp.Text = dt.Rows[0]["col34"].ToString().Trim();
                                    //txtforreg.Text = dt.Rows[0]["col36"].ToString().Trim();
                                    txtfortest.Text = dt.Rows[0]["col38"].ToString().Trim();
                                    txtremark.Text = dt.Rows[0]["col40"].ToString().Trim();
                                    string strval = dt.Rows[0]["col52"].ToString().Trim();
                                    if (strval == "0" || strval == "1") { }
                                    else strval = "0";
                                    rbminor.SelectedValue = strval;
                                    // txtprodcode.Text = dt.Rows[0]["col53"].ToString().Trim();
                                    txthead.Text = dt.Rows[0]["col43"].ToString().Trim();
                                    txtsign.Text = dt.Rows[0]["docdate"].ToString().Trim();
                                    txti.Text = dt.Rows[0]["col44"].ToString().Trim();
                                    txtii.Text = dt.Rows[0]["col45"].ToString().Trim();
                                    txtiii.Text = dt.Rows[0]["col46"].ToString().Trim();
                                    txtmddate.Text = dt.Rows[0]["col47"].ToString().Trim();
                                    //  txtfeedback.Text = dt.Rows[0]["col51"].ToString().Trim();
                                    if (dt.Rows[0]["EMAIL_ID"].ToString().Trim() == "") { rbspeci.SelectedValue = "1"; }
                                    else
                                    {
                                        rbspeci.SelectedValue = dt.Rows[0]["EMAIL_ID"].ToString().Trim().Replace("-", "1").Replace(" ", "1");
                                    }
                                    txtAllAttachments.Text = dt.Rows[0]["col51"].ToString(); hf2.Value = dt.Rows[0]["remarks"].ToString();
                                    if (dt.Rows[0]["email_id"].ToString() == "0")
                                    {
                                        Attch.Visible = true;
                                        txtAllAttachments.Visible = true;
                                        btnDown.Visible = true;
                                    }
                                    txtclosed.Text = dt.Rows[0]["col49"].ToString().Trim();
                                    txthmdate.Text = dt.Rows[0]["col50"].ToString().Trim();
                                    rbcust3.SelectedValue = dt.Rows[0]["col55"].ToString().Trim();
                                    Textpr1.Text = dt.Rows[0]["num1"].ToString().Trim();
                                    Textpr2.Text = dt.Rows[0]["num2"].ToString().Trim();
                                    txtprodname.Text = dt.Rows[0]["PROD_NAME"].ToString().Trim();
                                    //txtstatus.Text = dt.Rows[0]["HO_STATUS"].ToString().Trim();
                                    txtstatus.Text = dt.Rows[0]["COL4"].ToString().Trim();
                                    //string strstatus = dt.Rows[0]["ENQ_STATUS"].ToString().Trim();
                                    //if (strstatus == "NEW") strstatus = "0";
                                    //if (strstatus == "EXISTING") strstatus = "1";
                                    //rbcust3.SelectedValue = strstatus;

                                    txtIntake.Text = dt.Rows[0]["COL60"].ToString().Trim();
                                    txtBanking.Text = dt.Rows[0]["COL61"].ToString().Trim();
                                    txtSpecific.Text = dt.Rows[0]["COL62"].ToString().Trim();
                                    txtAccelerated.Text = dt.Rows[0]["COL63"].ToString().Trim();
                                    txtAny.Text = dt.Rows[0]["COL64"].ToString().Trim();
                                    txtVolume.Text = dt.Rows[0]["COL65"].ToString().Trim();
                                    txtValue.Text = dt.Rows[0]["COL66"].ToString().Trim();
                                    txtFutureVol.Text = dt.Rows[0]["COL67"].ToString().Trim();
                                    txtFutureVal.Text = dt.Rows[0]["COL68"].ToString().Trim();
                                    txtBasicPrice.Text = dt.Rows[0]["COL69"].ToString().Trim();
                                    txtPymt.Text = dt.Rows[0]["COL70"].ToString().Trim();
                                    txtQty.Text = dt.Rows[0]["COL71"].ToString().Trim();
                                    txtSampleQty.Text = dt.Rows[0]["COL72"].ToString().Trim();
                                    txtSuggest.Text = dt.Rows[0]["COL73"].ToString().Trim();
                                    txtApproval.Text = dt.Rows[0]["COL74"].ToString().Trim();
                                    // txtDateTime.Text = dt.Rows[0]["COL75"].ToString().Trim();
                                    txtRefusal1.Text = dt.Rows[0]["COL76"].ToString().Trim();
                                    txtRefusal2.Text = dt.Rows[0]["COL77"].ToString().Trim();
                                    txtRegular.Text = dt.Rows[0]["COL78"].ToString().Trim();
                                    txtCostSheet.Text = dt.Rows[0]["COL79"].ToString().Trim();
                                    //  txtSampleSize.Text = dt.Rows[0]["COL80"].ToString().Trim();
                                    txtEstimated.Text = dt.Rows[0]["COL81"].ToString().Trim();
                                    txtFormat.Text = dt.Rows[0]["COL82"].ToString().Trim();
                                    txtRev.Text = dt.Rows[0]["COL83"].ToString().Trim();
                                    txtEffDate.Text = dt.Rows[0]["COL84"].ToString().Trim();
                                    txtiiii.Text = dt.Rows[0]["COL86"].ToString().Trim();
                                    txtAppAdd.Text = dt.Rows[0]["COL87"].ToString().Trim();
                                    txtfeedback.Text = dt.Rows[0]["col80"].ToString().Trim();
                                    txtFeasDoc.Text = dt.Rows[0]["col32"].ToString().Trim();
                                    txtFeasRev.Text = dt.Rows[0]["col33"].ToString().Trim();
                                    txtFeasDate.Text = dt.Rows[0]["col34"].ToString().Trim();
                                    txtclosed2.Text = dt.Rows[0]["col36"].ToString().Trim();
                                }
                                create_tab();
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dr1 = dt1.NewRow();
                                    dr1["srno"] = dt1.Rows.Count + 1;
                                    dr1["code"] = dt.Rows[i]["col30"].ToString().Trim();
                                    dr1["name"] = fgen.seek_iname(frm_qstr,co_cd, "select col1 from inspmst where type='SF' and acode='" + dt.Rows[i]["col30"].ToString().Trim() + "'", "col1");
                                    dr1["yes"] = dt.Rows[i]["col53"].ToString().Trim();
                                    dr1["remarks"] = dt.Rows[i]["col31"].ToString().Trim();
                                    dt1.Rows.Add(dr1);
                                }
                                ViewState["sg1"] = dt1;
                                sg1.DataSource = dt1;
                                sg1.DataBind();
                                Sedtby = dt.Rows[0]["ent_by"].ToString().Trim();
                                Sedt = dt.Rows[0]["ent_dt"].ToString().Trim();
                                ViewState["ENTBY"] = Sedtby;
                                ViewState["ENTDT"] = Sedt;
                                ViewState["APP_BY"] = dt.Rows[0]["APP_BY"].ToString().Trim();
                                ViewState["APP_DT"] = dt.Rows[0]["APP_DT"].ToString().Trim();
                                fgen.EnableForm(this.Controls);
                               // btnexit.Text = "Cancel";
                                lbledmode.Value = "Y";                               
                                enablectrl();
                                MultiView1.ActiveViewIndex = 0;
                                btnedit.Disabled = true;
                                btnnew.Disabled = true;
                                btndel.Disabled = true;
                                btnprint.Disabled = true;
                                btnlist.Disabled = true;
                                #endregion
                            }
                            if (Dept.Length >= 1)
                            {
                                if (Dept.Substring(0, 1) == "R")
                                {
                                    fgen.DisableForm(this.Controls);
                                    txtDraft.Enabled = true;
                                    txtDraftDt.Enabled = true;
                                    txtFeasDoc.Enabled = true;
                                    txtFeasRev.Enabled = true;
                                    txtFeasDate.Enabled = true;
                                    txtSDR2.Enabled = true;
                                    txtSDR2Date.Enabled = true;
                                    txtCust2.Enabled = true;
                                    sg1.Enabled = true;
                                    sg1.DataBind();
                                    txti.Enabled = true;
                                    txtii.Enabled = true;
                                    txtiii.Enabled = true;
                                    txtiiii.Enabled = true;
                                    txtfeedback.Enabled = true;
                                    txtclosed.Enabled = true;
                                    txtclosed2.Enabled = true;
                                    txtApproval.Enabled = true;
                                    txtRefusal2.Enabled = true;
                                }
                                else
                                {
                                    fgen.EnableForm(this.Controls);
                                    txtDraft.Enabled = false;
                                    txtDraftDt.Enabled = false;
                                    txtFeasDoc.Enabled = false;
                                    txtFeasRev.Enabled = false;
                                    txtFeasDate.Enabled = false;
                                    txtSDR2.Enabled = false;
                                    txtSDR2Date.Enabled = false;
                                    txtCust2.Enabled = false;
                                    sg1.Enabled = false;
                                    sg1.DataBind();
                                    txti.Enabled = false;
                                    txtii.Enabled = false;
                                    txtiii.Enabled = false;
                                    txtiiii.Enabled = false;
                                    txtfeedback.Enabled = false;
                                    txtclosed.Enabled = false;
                                    txtclosed2.Enabled = false;
                                    txtApproval.Enabled = false;
                                    txtRefusal2.Enabled = false;
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                case "D":
                    #region
                    if (sname.Length == 6)
                    {
                        ViewState["fstr"] = scode;
                        ViewState["fstr1"] = sname;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr,co_cd, "select sdr_no,sdr_date from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + scode + "'");
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["sdr_no"].ToString() != " ")
                            {
                                if (ulvl != "0" && ulvl != "1")
                                {
                                    //  fgen.msg("-", "AMSG", "Dear " + uname + " , You do not have rights for Deleting."); return;
                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Dear " + uname + " , This Entry Cannot Be Deleted.','Tejaxo ERP Alert Message');});</script>", false);
                                }
                                else
                                {
                                    lblname.Value = "SURE";
                                  //  SendQuery();
                                    make_qry_4_popup();
                                    ScriptManager.RegisterStartupScript(btnhideF, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
                                    //fgen.send_cookie("xid", "Tejaxo");
                                    //fgen.send_cookie("seeksql", Seeksql);
                                    //fgen.send_cookie("srchSql", Seeksql);
                                    //fgen.open_sseek("-");
                                }
                            }
                            else
                            {
                                lblname.Value = "SURE";
                               // SendQuery();
                                make_qry_4_popup();
                                ScriptManager.RegisterStartupScript(btnhideF, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
                                //fgen.send_cookie("xid", "Tejaxo");
                                //fgen.send_cookie("seeksql", Seeksql);
                                //fgen.send_cookie("srchSql", Seeksql);
                                //fgen.open_sseek("-");
                            }
                        }
                    }
                    else
                    {
                        col1 = (string)ViewState["fstr"].ToString(); col2 = (string)ViewState["fstr1"].ToString();
                        if (scode == "YES")
                        {
                          //  popvar = fgen.Con2OLE(frm_qstr,co_cd);
                            consql.ConnectionString = popvar;
                            consql.Open();
                            command1 = new OracleCommand("delete from scratch where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAr(VCHDATE,'DD/MM/YYYY') ='" + col1 + "'", consql);
                            command1.ExecuteNonQuery();
                            consql.Close();
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('SDR No. " + col2 + " Has Been Deleted.','Tejaxo ERP Alert Message');});</script>", false);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('SDR No. " + col2 + " Has Not Been Deleted.','Tejaxo ERP Alert Message');});</script>", false);
                        }
                        ViewState["fstr"] = null; ViewState["fstr1"] = null;
                        fgen.DisableForm(this.Controls);
                        lblname.Value = "";
                        fgen.ResetForm(this.Controls);
                        lbledmode.Value = "";
                        MultiView1.ActiveViewIndex = 0;
                    }
                    #endregion
                    break;
                case "P":
                    #region
                    if (scode.Length >= 6)
                    {
                        dt = new DataTable();
                        // string ebrcode = fgen.seek_iname(co_cd, "Select ebr from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + scode + "')", "ebr");
                        // // dt = fgen.TypeInfo(co_cd, ebrcode);
                        // dt = fgen.getdata(co_cd, ebrcode);
                        string ebrcode = fgen.seek_iname(frm_qstr,co_cd, "Select BRANCHCD from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + scode + "')", "BRANCHCD");
                        dt = fgen.getdata(frm_qstr,co_cd, "SELECT TYPE1,NAME,ADDR,ADDR1,ADDR2,PLACE,TELE,FAX FROM TYPE WHERE ID='B' AND TYPE1='" + ebrcode + "'");
                        if (dt.Rows.Count > 0)
                        {
                            br_name = dt.Rows[0]["name"].ToString().Trim();
                            br_addr = dt.Rows[0]["addr"].ToString().Trim();
                            br_addr1 = dt.Rows[0]["addr1"].ToString().Trim();
                            br_addr2 = dt.Rows[0]["addr2"].ToString().Trim();
                            br_place = dt.Rows[0]["place"].ToString().Trim();
                            br_tele = dt.Rows[0]["tele"].ToString().Trim();
                            br_fax = dt.Rows[0]["fax"].ToString().Trim();
                        }

                        string prod_cat1 = fgen.seek_iname(frm_qstr,co_cd, "select nvl(PROD_cAT,'LP') as PROD_cAT from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + scode + "')", "prod_cat");
                        HFOPT.Value = prod_cat1;

                      //  popvar = fgen.Con2OLE(co_cd);
                     //   consql.ConnectionString = popvar;

                        //firm = fgen.CHECK_CO(co_cd);
                        //DataSet ds = new DataSet();



                        //string dd = "Select A.ACODE,E.USERNAME,I.COL1 AS MASTER,a.branchcd,A.ebr,nvl(A.PROD_cAT,'LP') as PROD_cAT,nvl(A.PROD_NAME,'-') as PROD_NAME,nvl(A.HO_STATUS,'-') as HO_STATUS,A.COL4 AS MDNAME,A.col56,nvl(A.col57,'-') as col57, nvl(A.num1,0) as num1,nvl(A.num2,0) as num2,A.ENQ_STATUS,(CASE WHEN trim(NVL(A.col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(A.EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(A.col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(A.col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,A.invno,to_char(A.invdate,'dd/mm/yyyy') as invdate, NVL(A.COL51,'-') AS COL51,NVL(A.COL52,'0') AS COL52,NVL(A.COL53,'-') AS COL53,A.COL54,A.vchnum,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,A.col1,A.col2,A.col3,A.col21, A.col11,A.col5,A.col59 AS COL4,A.col6,A.col7,A.col8,A.col9,A.col10,A.col12,A.col27,A.col15,A.col18,nvl(A.col16,'-') as col16,nvl(A.col17,'-') as col17,nvl(A.col19,'-') as col19,A.col28,A.col22,A.col35,A.col37,A.col39,A.col25,A.col20,A.col13,A.col14,A.remarks,A.col40,NVL(A.col30,'-') AS COL30,NVL(A.col31,'-') AS COL31,NVL(A.col32,'-') AS COL32,NVL(A.col33,'-') AS COL33,NVL(A.col34,'-') AS col34,NVL(A.col36,'-') AS col36,NVL(A.col38,'-') AS col38,A.col41,A.col42,NVL(A.col43,'-') AS col43,TO_CHAR(NVL(A.docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(A.col44,'-') AS  col44,NVL(A.col45,'-') AS col45,NVL(A.col46,'-') AS col46,NVL(A.COL47,'-') AS col47,NVL(A.col48,'-') AS col48,NVL(A.col49,'-') AS col49,TO_CHAR(NVL(A.COL50,SYSDATE),'DD/MM/YYYY') as col50,A.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_dt,'" + br_name + "' as typename,'" + br_addr + "' as addr,'" + br_addr1 + "' as addr1,'" + br_addr2 + "' as addr2,'" + br_place + "' as place,'" + br_tele + "' as tele,'" + br_fax + "' as fax,'" + firm + "' as firm,NVL(A.col60,'-') AS col60,NVL(A.col61,'-') AS col61,NVL(A.col62,'-') AS col62,NVL(A.col63,'-') AS col63,NVL(A.col64,'-') AS col64,NVL(A.col65,'-') AS col65,NVL(A.col66,'-') AS col66,NVL(A.col67,'-') AS col67,NVL(A.col68,'-') AS col68,NVL(A.col69,'-') AS col69, NVL(A.col70,'-') AS col70,NVL(A.col71,'-') AS col71,NVL(A.col72,'-') AS col72,NVL(A.col73,'-') AS col73,NVL(A.col74,'-') AS col74,NVL(A.col75,'-') AS col75,NVL(A.col76,'-') AS col76,NVL(A.col77,'-') AS col77,NVL(A.col78,'-') AS col78,NVL(A.col79,'-') AS col79,NVL(A.col80,'-') AS col80,NVL(A.col81,'-') AS col81,NVL(A.col82,'-') AS col82,NVL(A.col83,'-') AS col83,NVL(A.col84,'-') AS col84,NVL(A.col85,'-') AS col85,NVL(A.col86,'-') AS col86,NVL(A.col87,'-') AS col87,A.SDR_NO,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from EVAS E,scratch A left join inspmst i on trim(a.col30)=trim(i.acode) and i.type='SF' where TRIM(A.ACODE)=TRIM(E.USERID) AND A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_CHAr(A.vchdate,'DD/MM/YYYY') in ('" + scode + "') order by a.col30";
                        //da1 = new OracleDataAdapter("Select A.ACODE,E.USERNAME,I.COL1 AS MASTER,a.branchcd,A.ebr,nvl(A.PROD_cAT,'LP') as PROD_cAT,nvl(A.PROD_NAME,'-') as PROD_NAME,nvl(A.HO_STATUS,'-') as HO_STATUS,A.COL4 AS MDNAME,A.col56,nvl(A.col57,'-') as col57, nvl(A.num1,0) as num1,nvl(A.num2,0) as num2,A.ENQ_STATUS,(CASE WHEN trim(NVL(A.col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(A.EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(A.col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(A.col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,A.invno,to_char(A.invdate,'dd/mm/yyyy') as invdate, NVL(A.COL51,'-') AS COL51,NVL(A.COL52,'0') AS COL52,NVL(A.COL53,'-') AS COL53,A.COL54,A.vchnum,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,A.col1,A.col2,A.col3,A.col21, A.col11,A.col5,A.col59 AS COL4,A.col6,A.col7,A.col8,A.col9,A.col10,A.col12,A.col27,A.col15,A.col18,nvl(A.col16,'-') as col16,nvl(A.col17,'-') as col17,nvl(A.col19,'-') as col19,A.col28,A.col22,A.col35,A.col37,A.col39,A.col25,A.col20,A.col13,A.col14,A.remarks,A.col40,NVL(A.col30,'-') AS COL30,NVL(A.col31,'-') AS COL31,NVL(A.col32,'-') AS COL32,NVL(A.col33,'-') AS COL33,NVL(A.col34,'-') AS col34,NVL(A.col36,'-') AS col36,NVL(A.col38,'-') AS col38,A.col41,A.col42,NVL(A.col43,'-') AS col43,TO_CHAR(NVL(A.docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(A.col44,'-') AS  col44,NVL(A.col45,'-') AS col45,NVL(A.col46,'-') AS col46,NVL(A.COL47,'-') AS col47,NVL(A.col48,'-') AS col48,NVL(A.col49,'-') AS col49,TO_CHAR(NVL(A.COL50,SYSDATE),'DD/MM/YYYY') as col50,A.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_dt,'" + br_name + "' as typename,'" + br_addr + "' as addr,'" + br_addr1 + "' as addr1,'" + br_addr2 + "' as addr2,'" + br_place + "' as place,'" + br_tele + "' as tele,'" + br_fax + "' as fax,'" + firm + "' as firm,NVL(A.col60,'-') AS col60,NVL(A.col61,'-') AS col61,NVL(A.col62,'-') AS col62,NVL(A.col63,'-') AS col63,NVL(A.col64,'-') AS col64,NVL(A.col65,'-') AS col65,NVL(A.col66,'-') AS col66,NVL(A.col67,'-') AS col67,NVL(A.col68,'-') AS col68,NVL(A.col69,'-') AS col69, NVL(A.col70,'-') AS col70,NVL(A.col71,'-') AS col71,NVL(A.col72,'-') AS col72,NVL(A.col73,'-') AS col73,NVL(A.col74,'-') AS col74,NVL(A.col75,'-') AS col75,NVL(A.col76,'-') AS col76,NVL(A.col77,'-') AS col77,NVL(A.col78,'-') AS col78,NVL(A.col79,'-') AS col79,NVL(A.col80,'-') AS col80,NVL(A.col81,'-') AS col81,NVL(A.col82,'-') AS col82,NVL(A.col83,'-') AS col83,NVL(A.col84,'-') AS col84,NVL(A.col85,'-') AS col85,NVL(A.col86,'-') AS col86,NVL(A.col87,'-') AS col87,A.SDR_NO,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from EVAS E,scratch A left join inspmst i on trim(a.col30)=trim(i.acode) and i.type='SF' where TRIM(A.ACODE)=TRIM(E.USERID) AND A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_CHAr(A.vchdate,'DD/MM/YYYY') in ('" + scode + "') order by a.col30", consql);
                        ////ORIGINAL da1 = new OracleDataAdapter("Select branchcd,ebr,nvl(PROD_cAT,'LP') as PROD_cAT,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,COL4 AS MDNAME, col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,(CASE WHEN trim(NVL(col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,  invno,to_char(invdate,'dd/mm/yyyy') as invdate,nvl(PROD_NAME,'-') as PROD_NAME, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54, vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59 AS COL4,col6,col7,col8,col9,col10,col12,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col27,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,remarks,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,'-') AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,'" + br_name + "' as typename,'" + br_addr + "' as addr,'" + br_addr1 + "' as addr1,'" + br_addr2 + "' as addr2,'" + br_place + "' as place,'" + br_tele + "' as tele,'" + br_fax + "' as fax,'" + firm + "' as firm,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + scode + "')", consql);
                        //da1.Fill(ds, "Prepcur");
                        //string str2 = Server.MapPath("~/xmlreport/Enquiry.xml");
                        //ds.WriteXml(str2, XmlWriteMode.WriteSchema);
                        //Session["mydataset"] = ds;
                        //if (HFOPT.Value == "LP") HP = new HttpCookie("xCRFILE", "~/Report/SDR.rpt");
                        //if (HFOPT.Value == "PC") HP = new HttpCookie("xCRFILE", "~/Report/SDR_.rpt");
                        //Response.Cookies.Add(HP);


                        //btnexit.Text = "Cancel";
                       // ScriptManager.RegisterStartupScript(btnhideF, this.GetType(), "abcd", "$(document).ready(function(){openFORM('Frm_Report.aspx');});", true);
                        //if (HFOPT.Value == "LP") { fgen.Fn_Print_Report_BYDS(co_cd, "", mbr, "Enquiry", "SDR", ds); }
                        //else
                        //{
                        //    fgen.Fn_Print_Report_BYDS(co_cd, "", mbr, "Enquiry", "SDR_", ds);
                        //}

                    }
                    #endregion
                    break;
                case "LI":
                    #region
                    if (scode.Length > 6)
                    {
                        strsort = "VDD desc,vchnum desc";
                        //    Seeksql = "select  VCHNUM as SDR_No,TO_CHAr(VCHdate,'DD/MM/YYYY') as SDR_Date, col1 as Cust_Name,PROD_CAT AS CATEGORY,ENQ_STATUS AS DEVELOPMENT,  INVNO as Enq_No,TO_CHAr(INVDATE,'DD/MM/YYYY') as Enq_Date,col2 as addr1,col11 as add2, col5 as addr3,col3 as req_by,col21 as recom_by,type, TO_CHAR(VCHdate,'YYYYMMDD') AS VDD from scratch WHERE type='ES' and branchcd='" + mbr + "' AND VCHDATE " + scode + "";
                        if (ulvl != "0" && ulvl != "1")
                        {
                            //Seeksql = "select  SDR_No as SDR_No,TO_CHAr(SDR_DATE,'DD/MM/YYYY') as SDR_Date, col1 as Cust_Name,PROD_CAT AS CATEGORY,ENQ_STATUS AS DEVELOPMENT,col2 as addr1,col11 as add2, col5 as addr3,col3 as req_by,col21 as recom_by,type, TO_CHAR(VCHdate,'YYYYMMDD') AS VDD from scratch WHERE type='ES' and branchcd='" + mbr + "' AND VCHDATE " + scode + " AND ENT_BY='" + uname + "'";
                            Seeksql = "SELECT DISTINCT A.SDR_NO AS FSTR, A.SDR_NO ,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,A.COL1 AS CUST_NAME,A.PROD_CAT AS CATEGORY,A.COL9 AS TYPE_OF_DEVELOPMENT,A.COL11 AS CITY,A.COL3 AS RECOM_BY,B.HO_STATUS AS STATUS,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH A LEFT JOIN SCRATCH B ON TRIM(A.SDR_NO)||TO_CHAR(A.SDR_DATE,'DD/MM/YYYY')=TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')  AND B.BRANCHCD='" + mbr + "' AND B.TYPE='EU' WHERE  A.BRANCHCD='" + mbr + "' AND A.TYPE='ES' AND A.VCHDATE " + scode + " AND LENGTH(A.SDR_NO)>2 AND A.ENT_BY='" + uname + "'";
                        }
                        else
                        {
                            //  Seeksql = "select  SDR_No as SDR_No,TO_CHAr(SDR_DATE,'DD/MM/YYYY') as SDR_Date, col1 as Cust_Name,PROD_CAT AS CATEGORY,ENQ_STATUS AS DEVELOPMENT,col2 as addr1,col11 as add2, col5 as addr3,col3 as req_by,col21 as recom_by,type, TO_CHAR(VCHdate,'YYYYMMDD') AS VDD from scratch WHERE type='ES' and branchcd='" + mbr + "' AND VCHDATE " + scode + "";\
                            Seeksql = "SELECT DISTINCT A.SDR_NO AS FSTR, A.SDR_NO ,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE,A.COL1 AS CUST_NAME,A.PROD_CAT AS CATEGORY,A.COL9 AS TYPE_OF_DEVELOPMENT,A.COL11 AS CITY,A.COL3 AS RECOM_BY,B.HO_STATUS AS STATUS,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH A LEFT JOIN SCRATCH B ON TRIM(A.SDR_NO)||TO_CHAR(A.SDR_DATE,'DD/MM/YYYY')=TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')  AND B.BRANCHCD='" + mbr + "' AND B.TYPE='EU' WHERE  A.BRANCHCD='" + mbr + "' AND A.TYPE='ES' AND A.VCHDATE " + scode + " AND LENGTH(A.SDR_NO)>2";
                        }
                        if (Seeksql == "") { }
                        else
                        {
                            HP = new HttpCookie("srchSql", "" + Seeksql + "");
                            Response.Cookies.Add(HP);
                            HP = new HttpCookie("Sort", "" + strsort + "");
                            Response.Cookies.Add(HP);
                        }
                        ScriptManager.RegisterStartupScript(btnhideF, this.GetType(), "abcd", "$(document).ready(function(){openFORM('rptlevel2.aspx');});", true);
                        lbledmode.Value = "";
                        lblname.Value = "";
                        //fgen.send_cookie("xid", "Tejaxo");
                        //fgen.send_cookie("seeksql", Seeksql);
                        //fgen.send_cookie("srchSql", Seeksql);
                        //fgen.open_sseek("-");
                    }
                    #endregion
                    break;

                case "Type":
                    #region
                    fgen.ResetForm(this.Controls);
                    scode = col1;
                    if (scode == "Label")
                    {
                        HFOPT.Value = "LP";
                        lblhead.Text = "Liquid Paint Division";
                        lblProduct.Text = "Product Type (ST/AD/TSA/2KPU/2U/1K Epoxy/Thinner/Other)";
                        lblShade.Text = "Shade (Solid/ Metallic/Candy/Other)";
                        lblPre.Text = "Pre-Treatment Type";
                        lblsystem.Text = "Application System Flash off Time Between Primer and Paint";
                        lbldft.Text = "DFT STD Specified by Customer(For Composites System, Each Coat)";
                        lblSampleQty.Text = "Sample Qty Required For Trail (Standard 1- Ltr.)for Extra Qty of Sample Pls Specify";
                        lblIntake.Visible = true;
                        lblthinner.Visible = true;
                        lblMethod.Visible = true;
                        txtIntake.Visible = true;
                        txtthinner.Visible = true;
                        txtmethod.Visible = true;
                        lblSpecific.Visible = true;
                        txtSpecific.Visible = true;
                        lblbasic.Visible = true;
                        txtbasic.Visible = true;
                        lblCust3.Visible = true;
                        rbcust3.Visible = true;
                        txtFormat.Text = "F/MKTG/01A";
                        txtRev.Text = "0";
                        txtEffDate.Text = "14/04/2017";
                    }
                    else
                    {
                        HFOPT.Value = "PC";
                        lblhead.Text = "Powder Coating Division";
                        lblProduct.Text = "Product Type (PP/EP/Others)";
                        lblShade.Text = "Shade (Solid/ Metallic/Trans/Other)";
                        lblPre.Text = "Pre-Treatment System & Type";
                        lblsystem.Text = "Application System";
                        lbldft.Text = "Type of Oven";
                        lblSampleQty.Text = "Sample Qty Required For Trail (Standard - 250 Grams)for Extra Qty of Sample Pls Specify";
                        lblIntake.Visible = false;
                        lblthinner.Visible = false;
                        lblMethod.Visible = false;
                        txtIntake.Visible = false;
                        txtthinner.Visible = false;
                        txtmethod.Visible = false;
                        lblSpecific.Visible = false;
                        txtSpecific.Visible = false;
                        lblbasic.Visible = false;
                        txtbasic.Visible = false;
                        lblCust3.Visible = false;
                        rbcust3.Visible = false;
                        txtFormat.Text = "F/MKTG/01B";
                        txtRev.Text = "0";
                        txtEffDate.Text = "14/04/2017";
                    }
                    rbdeve.Enabled = true;
                    btnnew.Disabled = true;
                    btnedit.Disabled = true;
                    btndel.Disabled = true;
                    btnprint.Disabled = true;
                    btnlist.Disabled = true;
                    btnsave.Disabled = false;
                    btnsubmit.Disabled = false;
                    //rbcust.Enabled = true;
                    //rbcust0.Enabled = true;
                    //rbcust1.Enabled = true;
                    //rbcust3.Enabled = true;
                    //rbdeve.Enabled = true;
                    //rbminor.Enabled = true;
                    //rbspeci.Enabled = true;
                    //btnexit.Text = "Cancel";
                    fgen.EnableForm(this.Controls);
                    Next_No();
                    txtbranch.Text = fgen.seek_iname(frm_qstr,co_cd, "select name from type where id='B' and type1='" + frm_mbr.Substring(0, 2) + "'", "NAME");
                    txtenqno.Text = fgen.next_no(frm_qstr, co_cd, "select max(INVNO) as vch from scratch where branchcd='" + frm_mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + DateRange + "", 6, "VCH");                
                    txtdate.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
                    //  enable_btn();
                    //rbcust.Enabled = false;
                    //rbcust0.Enabled = false;
                    //rbcust1.Enabled = false;
                    //rbspeci.Enabled = false;
                    txtdate1.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
                    txtsign.Text = txtdate1.Text;
                    txtmddate.Text = txtdate1.Text;
                    txthmdate.Text = txtdate1.Text;
                    txtDraftDt.Text = txtdate1.Text;
                    txtUserName.Text = uname;
                    txtUserid.Text = fgen.seek_iname(frm_qstr,co_cd, "select userid from evas where username='" + frm_uname + "'", "userid");
                    create_tab();
                    mq0 = "select distinct acode,col1 from inspmst where type='SF' order by acode";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr,co_cd, mq0);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["srno"] = dt1.Rows.Count + 1;
                        dr1["code"] = dt.Rows[i]["acode"].ToString().Trim();
                        dr1["name"] = dt.Rows[i]["col1"].ToString().Trim();
                        dr1["yes"] = "-";
                        dr1["remarks"] = "-";
                        dt1.Rows.Add(dr1);
                    }
                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();
                    txtFeasDoc.Text = "F/MKTG/02";
                    txtFeasRev.Text = "0";
                    txtFeasDate.Text = "14/04/2017";
                    // txtEffDate.Text = fgen.CurrDate(co_cd);
                    if (Dept.Length >= 1)
                    {
                        if (Dept.Substring(0, 1) == "R")
                        {
                            fgen.DisableForm(this.Controls);
                            txtDraft.Enabled = true;
                            txtDraftDt.Enabled = true;
                            txtFeasDoc.Enabled = true;
                            txtFeasRev.Enabled = true;
                            txtFeasDate.Enabled = true;
                            txtSDR2.Enabled = true;
                            txtSDR2Date.Enabled = true;
                            txtCust2.Enabled = true;
                            sg1.Enabled = true;
                            sg1.DataBind();
                            txti.Enabled = true;
                            txtii.Enabled = true;
                            txtiii.Enabled = true;
                            txtiiii.Enabled = true;
                            txtfeedback.Enabled = true;
                            txtclosed.Enabled = true;
                            txtclosed2.Enabled = true;
                            txtApproval.Enabled = true;
                            txtRefusal2.Enabled = true;
                        }
                        else
                        {
                            fgen.EnableForm(this.Controls);
                            txtDraft.Enabled = false;
                            txtDraftDt.Enabled = false;
                            txtFeasDoc.Enabled = false;
                            txtFeasRev.Enabled = false;
                            txtFeasDate.Enabled = false;
                            txtSDR2.Enabled = false;
                            txtSDR2Date.Enabled = false;
                            txtCust2.Enabled = false;
                            sg1.Enabled = false;
                            sg1.DataBind();
                            txti.Enabled = false;
                            txtii.Enabled = false;
                            txtiii.Enabled = false;
                            txtiiii.Enabled = false;
                            txtfeedback.Enabled = false;
                            txtclosed.Enabled = false;
                            txtclosed2.Enabled = false;
                            txtApproval.Enabled = false;
                            txtRefusal2.Enabled = false;
                        }
                    }
                    #endregion
                    break;
                case "New":
                    newCase(col1);
                    break;

                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.name,c.iname,c.cpartno,c.cdrgno,c.unit,to_char(a.ent_Dt,'dd/mm/yyyy') as pent_dt,to_char(a.edt_Dt,'dd/mm/yyyy') as papp_dt from " + frm_tabname + " a,type b,item c where trim(a.stagec)=trim(b.type1) and b.id='K' and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    //SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        //txtlbl4.Text = dt.Rows[i]["Icode"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["Iname"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        //txtlbl3.Text = dt.Rows[i]["pent_dt"].ToString().Trim();

                        //txtlbl5.Text = dt.Rows[i]["cpartno"].ToString().Trim();
                        //txtlbl6.Text = dt.Rows[i]["cdrgno"].ToString().Trim();

                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");



                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        //    sg1_dr["sg1_h1"] = "-";
                        //    sg1_dr["sg1_h2"] = "-";
                        //    sg1_dr["sg1_h3"] = "-";
                        //    sg1_dr["sg1_h4"] = "-";
                        //    sg1_dr["sg1_h5"] = "-";
                        //    sg1_dr["sg1_h6"] = "-";
                        //    sg1_dr["sg1_h7"] = "-";
                        //    sg1_dr["sg1_h8"] = "-";
                        //    sg1_dr["sg1_h9"] = "-";
                        //    sg1_dr["sg1_h10"] = "-";

                        //    sg1_dr["sg1_f1"] = dt.Rows[i]["stagec"].ToString().Trim();
                        //    sg1_dr["sg1_f2"] = dt.Rows[i]["Name"].ToString().Trim();
                        //    sg1_dr["sg1_f3"] = "-";
                        //    sg1_dr["sg1_f4"] = "-";
                        //    sg1_dr["sg1_f5"] = "-";

                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["mtime"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = dt.Rows[i]["mtime1"].ToString().Trim();
                        //    sg1_dr["sg1_t3"] = dt.Rows[i]["opcode"].ToString().Trim();
                        //    sg1_dr["sg1_t4"] = "-";
                        //    sg1_dr["sg1_t5"] = dt.Rows[i]["remarks"].ToString().Trim();
                        //    sg1_dr["sg1_t6"] = dt.Rows[i]["tshots"].ToString().Trim();
                        //    sg1_dr["sg1_t7"] = dt.Rows[i]["pcpshot"].ToString().Trim();
                        //    sg1_dr["sg1_t8"] = dt.Rows[i]["fm_Fact"].ToString().Trim();


                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}

                        //ViewState["sg1"] = sg1_dt;
                        //sg1_add_blankrows();
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        //dt.Dispose(); sg1_dt.Dispose();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                    }
                    #endregion
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                   // lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
                    break;

                case "Del_E":
                    if (col1 == "") return;
                    clearctrl();
                   // edmode.Value = col1;
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

                case "Edit_E_OLD":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.name,c.iname,c.cpartno,c.cdrgno,c.unit,to_char(a.ent_Dt,'dd/mm/yyyy') as pent_dt,to_char(a.edt_Dt,'dd/mm/yyyy') as papp_dt,p.mchname from " + frm_tabname + " a left join pmaint p on trim(a.opcode)=trim(p.acode)||'/'||trim(p.srno) and p.branchcd='" + frm_mbr + "' and p.type='10',type b,item c where trim(a.stagec)=trim(b.type1) and b.id='K' and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtDraft.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        //txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        //txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        //txtlbl3.Text = dt.Rows[i]["pent_Dt"].ToString().Trim();
                        //txtlbl4.Text = dt.Rows[i]["icode"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["iname"].ToString().Trim();
                        //txtlbl5.Text = dt.Rows[i]["cpartno"].ToString().Trim();
                        //txtlbl6.Text = dt.Rows[i]["cdrgno"].ToString().Trim();
                        //txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        //    sg1_dr["sg1_h1"] = "-";
                        //    sg1_dr["sg1_h2"] = "-";
                        //    sg1_dr["sg1_h3"] = "-";
                        //    sg1_dr["sg1_h4"] = "-";
                        //    sg1_dr["sg1_h5"] = "-";
                        //    sg1_dr["sg1_h6"] = "-";
                        //    sg1_dr["sg1_h7"] = "-";
                        //    sg1_dr["sg1_h8"] = "-";
                        //    sg1_dr["sg1_h9"] = "-";
                        //    sg1_dr["sg1_h10"] = "-";
                        //    sg1_dr["sg1_f1"] = dt.Rows[i]["stagec"].ToString().Trim();
                        //    sg1_dr["sg1_f2"] = dt.Rows[i]["Name"].ToString().Trim();
                        //    sg1_dr["sg1_f3"] = "-";
                        //    sg1_dr["sg1_f4"] = "-";
                        //    sg1_dr["sg1_f5"] = "-";
                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["mtime"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = dt.Rows[i]["mtime1"].ToString().Trim();
                        //    sg1_dr["sg1_t3"] = dt.Rows[i]["opcode"].ToString().Trim();
                        //    sg1_dr["sg1_t4"] = dt.Rows[i]["MCHNAME"].ToString().Trim();
                        //    sg1_dr["sg1_t5"] = dt.Rows[i]["remarks"].ToString().Trim();
                        //    sg1_dr["sg1_t6"] = dt.Rows[i]["tshots"].ToString().Trim();
                        //    sg1_dr["sg1_t7"] = dt.Rows[i]["pcpshot"].ToString().Trim();
                        //    sg1_dr["sg1_t8"] = dt.Rows[i]["fm_Fact"].ToString().Trim();
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}
                        //ViewState["sg1"] = sg1_dt;
                        //sg1_add_blankrows();
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        //dt.Dispose(); sg1_dt.Dispose();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_engg_reps(frm_qstr);
                    break;             

                case "BTN_10":
                    break;
                case "BTN_11":
                    break;
                case "BTN_12":
                    break;
                case "BTN_13":
                    break;
                case "BTN_14":
                    break;
                case "BTN_15":
                    break;
                case "BTN_16":
                    break;
                case "BTN_17":
                    break;
                case "BTN_18":
                    break;
                case "BTN_19":
                    break;
             
                case "SG1_ROW_ADD":
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
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
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
                        dt = new DataTable();
                        if (col1.Length > 2) SQuery = "select type1,name,rate from type where id='K' and trim(type1) in (" + col1 + ") order by type1";
                        else SQuery = "select type1,name,rate from type where id='K' and trim(type1)='" + col1 + "' order by type1";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = dt.Rows[d]["type1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["name"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[d]["rate"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    ViewState["sg1"] = sg1_dt;
                    sg1_add_blankrows();
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "select type1,name,rate from type where id='K' and trim(type1)='" + col1 + "' order by type1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = dt.Rows[0]["type1"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = dt.Rows[0]["name"].ToString().Trim();
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["type1"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["name"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[0]["rate"].ToString().Trim();
                    }
                    setColHeadings();
                    break;

                case "SG1_ADD_MAC":
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
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
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
                        dt = new DataTable();
                        SQuery = "select trim(acode)||'/'||trim(srno) as fstr,mchname as Machine_Name,trim(acode)||'/'||trim(srno) as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' and trim(acode)||'/'||trim(srno)='" + col1 + "' order by acode,srno";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["machine_code"].ToString().Trim();
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[0]["machine_name"].ToString().Trim();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;                               

                case "SG1_RMV":
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
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("&amp;", "&");
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        //if (edmode.Value == "Y")
                        //{
                        //    //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();
                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}
                        //else
                        //{
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        // }
                        ViewState["sg1"] = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1.Rows[i].Cells[12].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;
            }
        }
    }

    public void sg1_add_blankrows()
    {
        if (sg1_dt == null) return;
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_h1"] = "-";
        sg1_dr["sg1_h2"] = "-";
        sg1_dr["sg1_h3"] = "-";
        sg1_dr["sg1_h4"] = "-";
        sg1_dr["sg1_h5"] = "-";
        sg1_dr["sg1_h6"] = "-";
        sg1_dr["sg1_h7"] = "-";
        sg1_dr["sg1_h8"] = "-";
        sg1_dr["sg1_h9"] = "-";
        sg1_dr["sg1_h10"] = "-";
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
        sg1_dr["sg1_t10"] = "-";
        sg1_dr["sg1_t11"] = "-";
        sg1_dr["sg1_t12"] = "-";
        sg1_dr["sg1_t13"] = "-";
        sg1_dr["sg1_t14"] = "-";
        sg1_dr["sg1_t15"] = "-";
        sg1_dr["sg1_t16"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    ///=============
    protected void btnhideF_S_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");           
            SQuery = "Select A.ACODE,E.USERNAME,I.COL1 AS MASTER,(CASE when nvl(A.PROD_cAT,'-')='LP' THEN '(Liquid Paint Division)' ELSE '(Powder Coating Division)' END) AS PROD_CATG, nvl(A.PROD_cAT,'LP') as PROD_cAT,nvl(A.PROD_NAME,'-') as PROD_NAME,nvl(A.HO_STATUS,'-') as HO_STATUS,A.COL4 AS MDNAME,A.col56,nvl(A.col57,'-') as col57, nvl(A.num1,0) as num1,nvl(A.num2,0) as num2,A.ENQ_STATUS,(CASE WHEN trim(NVL(A.col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(A.EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(A.col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(A.col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,A.invno,to_char(A.invdate,'dd/mm/yyyy') as invdate, NVL(A.COL51,'-') AS COL51,NVL(A.COL52,'0') AS COL52,NVL(A.COL53,'-') AS COL53,A.COL54,A.vchnum,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,A.col1,A.col2,A.col3,A.col21, A.col11,A.col5,A.col59 AS COL4,A.col6,A.col7,A.col8,A.col9,A.col10,A.col12,A.col27,A.col15,A.col18,nvl(A.col16,'-') as col16,nvl(A.col17,'-') as col17,nvl(A.col19,'-') as col19,A.col28,A.col22,A.col35,A.col37,A.col39,A.col25,A.col20,A.col13,A.col14,A.remarks,A.col40,NVL(A.col30,'-') AS COL30,NVL(A.col31,'-') AS COL31,NVL(A.col32,'-') AS COL32,NVL(A.col33,'-') AS COL33,NVL(A.col34,'-') AS col34,NVL(A.col36,'-') AS col36,NVL(A.col38,'-') AS col38,A.col41,A.col42,NVL(A.col43,'-') AS col43,TO_CHAR(NVL(A.docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(A.col44,'-') AS  col44,NVL(A.col45,'-') AS col45,NVL(A.col46,'-') AS col46,NVL(A.COL47,'-') AS col47,NVL(A.col48,'-') AS col48,NVL(A.col49,'-') AS col49,TO_CHAR(NVL(A.COL50,SYSDATE),'DD/MM/YYYY') as col50,A.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_dt,NVL(A.col60,'-') AS col60,NVL(A.col61,'-') AS col61,NVL(A.col62,'-') AS col62,NVL(A.col63,'-') AS col63,NVL(A.col64,'-') AS col64,NVL(A.col65,'-') AS col65,NVL(A.col66,'-') AS col66,NVL(A.col67,'-') AS col67,NVL(A.col68,'-') AS col68,NVL(A.col69,'-') AS col69, NVL(A.col70,'-') AS col70,NVL(A.col71,'-') AS col71,NVL(A.col72,'-') AS col72,NVL(A.col73,'-') AS col73,NVL(A.col74,'-') AS col74,NVL(A.col75,'-') AS col75,NVL(A.col76,'-') AS col76,NVL(A.col77,'-') AS col77,NVL(A.col78,'-') AS col78,NVL(A.col79,'-') AS col79,NVL(A.col80,'-') AS col80,NVL(A.col81,'-') AS col81,NVL(A.col82,'-') AS col82,NVL(A.col83,'-') AS col83,NVL(A.col84,'-') AS col84,NVL(A.col85,'-') AS col85,NVL(A.col86,'-') AS col86,NVL(A.col87,'-') AS col87,A.SDR_NO,TO_CHAR(A.SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from EVAS E," + frm_tabname + " A left join inspmst i on trim(a.col30)=trim(i.acode) and i.type='SF' where TRIM(A.ACODE)=TRIM(E.USERID) AND a.branchcd='" + frm_mbr + "' and a.type='ES' and a.vchdate  " + DateRange + "  order by a.col30";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For The Period Of " + fromdt + " To " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            string last_entdt;
            //checks
            if (edmode.Value == "Y")
            {
            }
            else
            {
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
                if (last_entdt == "0")
                { }
                else
                {
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtDraftDt.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtDraftDt.Text.ToString() + ",Please Check !!");
                    }
                }
            }
            ////last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            ////if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            ////{
            ////    Checked_ok = "N";
            ////    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            ////}
            //-----------------------------
            i = 0;
            hffield.Value = "";
            setColHeadings();

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

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtDraft.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            //save_it = "N";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            save_it = "Y";
                            // }
                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtDraftDt.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            //vchnum = txtDraft.Text.Trim();
                            //sedate = DateTime.Parse(Sedt, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
                            ////popvar = fgen.Con2OLE(co_cd);
                            //consql.ConnectionString = popvar;
                            //consql.Open();
                            //command1 = new OracleCommand("update scratch set branchcd='DD' where branchcd='" + mbr + "' AND TYPE='ES' and VCHNUM='" + vchnum + "' and to_DatE(to_char(VCHDATE,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + olddate + "','dd/MM/yyyy')", consql);
                            //command1.ExecuteNonQuery();
                            //consql.Close();
                            //consql.Open();
                            //command1 = new OracleCommand("Commit", consql);
                            //command1.ExecuteNonQuery();
                            //consql.Close();
                          
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            //=================
                            if (lblname.Value == "SURE_S1")
                            {
                                if (txtsdrno.Text.Length > 1) { }
                                else
                                {
                                    // SDR No Generation
                                   // txtsdrno.Text = fgen.Gen_No(co_cd, "select max(sdr_no) as vch from scratch where branchcd='" + mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + daterange + "", "VCH", 6);
                                    txtsdrno.Text = frm_mbr + fgen.next_no(frm_qstr, co_cd, "select MAX(SUBSTR(sdr_no,3,4)) as vch from scratch where branchcd='" + frm_mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + DateRange + "", 4, "VCH");
                                    txtdate1.Text = vardate;
                                }
                            }
                            vchnum = txtDraft.Text;
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtDraft.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" +  fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(2,18) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            lblUpload.Text = "";
                        }

                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "yogita@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully");
                                lblUpload.Text = "";
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtDraft.Text + txtDraftDt.Text.Trim(), frm_uname, edmode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtDraftDt.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings();
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                    }
            #endregion
                }
            }
        }
    }
    //-----------------------------------------------------
    //protected void btnhideF_S_Click(object sender, EventArgs e)
    //{
    //    col1 = "";
    //    col1 = Request.Cookies["Column1"].Value.ToString().Trim();
    //    if (col1 == "No")
    //    {
    //       // enable_btn();
    //        enablectrl();
    //        btnnew.Disabled = true;
    //        btnedit.Disabled = true;
    //        btndel.Disabled = true;
    //        btnprint.Disabled = true;
    //        btnlist.Disabled = true;
    //        return;
    //    }   
    //    vardate = fgen.Fn_curr_dt(co_cd, frm_qstr);
    //    hsdt = DateTime.Parse(txthmdate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //    sigdt = DateTime.Parse(txtsign.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //    presentdate = DateTime.Parse(vardate, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //    //invdate = DateTime.Parse(txtdate1.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //    invdate = DateTime.Parse(txtDraftDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //    vchdate = DateTime.Parse(txtdate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //    Sedtby = (string)ViewState["ENTBY"]; Sedt = (string)ViewState["ENTDT"];
    //    olddate = HFOLDDT.Value.ToString();
    //    if (lbledmode.Value == "Y")
    //    {
    //        vchnum = txtDraft.Text.Trim();
    //        sedate = DateTime.Parse(Sedt, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
    //        //popvar = fgen.Con2OLE(co_cd);
    //        consql.ConnectionString = popvar;
    //        consql.Open();
    //        command1 = new OracleCommand("update scratch set branchcd='DD' where branchcd='" + mbr + "' AND TYPE='ES' and VCHNUM='" + vchnum + "' and to_DatE(to_char(VCHDATE,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + olddate + "','dd/MM/yyyy')", consql);
    //        command1.ExecuteNonQuery();
    //        consql.Close();
    //        consql.Open();
    //        command1 = new OracleCommand("Commit", consql);
    //        command1.ExecuteNonQuery();
    //        consql.Close();
    //        if (lblname.Value == "SURE_S1")
    //        {
    //            if (txtsdrno.Text.Length > 1) { }
    //            else
    //            {
    //                // SDR No Generation
    //                // txtsdrno.Text = fgen.Gen_No(co_cd, "select max(sdr_no) as vch from scratch where branchcd='" + mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + daterange + "", "VCH", 6);
    //                txtsdrno.Text = mbr + fgen.next_no(frm_qstr, co_cd, "select MAX(SUBSTR(sdr_no,3,4)) as vch from scratch where branchcd='" + frm_mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + DateRange + "", 4, "VCH");
    //                txtdate1.Text = vardate;
    //            }
    //        }
    //        vchnum = txtDraft.Text;
    //    }
    //    else
    //    {
    //        Next_No();
    //        //vchnum = txtsdrno.Text.Trim();
    //        vchnum = txtDraft.Text;
    //    }
    //    // SDR No Generation

    //    if (lblname.Value == "SURE_S1")
    //    {
    //        if (txtsdrno.Text.Length > 1) { }
    //        else
    //        {
    //            // txtsdrno.Text = fgen.Gen_No(co_cd, "select max(sdr_no) as vch from scratch where branchcd='" + mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + daterange + "", "VCH", 6);
    //            txtsdrno.Text = mbr + fgen.next_no(frm_qstr, co_cd, "select MAX(SUBSTR(sdr_no,3,4)) as vch from scratch where branchcd='" + frm_mbr.Substring(0, 2) + "' and type='ES' AND VCHDATE " + DateRange + "", 4, "VCH");
    //            txtdate1.Text = vardate;
    //        }
    //    }

    //    fgen.fill_dash(this.Controls);
    //    //popvar = fgen.Con2OLE(co_cd);
        
     


    //    if (lbledmode.Value == "Y")
    //    {
    //      //  popvar = fgen.Con2OLE(co_cd);
    //        consql.ConnectionString = popvar;
    //        consql.Open();
    //        command1 = new OracleCommand("delete from scratch where branchcd='DD' and type='ES' and VCHNUM='" + vchnum + "' and to_DatE(to_char(VCHDATE,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + olddate + "','dd/MM/yyyy')", consql);
    //        command1.ExecuteNonQuery();
    //        consql.Close();
    //        consql.Open();
    //        command1 = new OracleCommand("Commit", consql);
    //        command1.ExecuteNonQuery();
    //        consql.Close();
    //        //  Dept = "R";
    //        if (Dept.Length >= 1)
    //        {
    //            if (Dept.Substring(0, 1) != "R")
    //            {
    //                if (txtsdrno.Text.Length == 6)
    //                {
    //                    mailbody();
    //                    send_mail_new(co_cd, "", "mdobriyal@maharanipaints.com,nmathurkar@maharanipaints.com,itsupport@maharanipaints.com,sanant@maharanipaints.com", "", "", "Tejaxo ERP: SDR Approval required For " + txtcust.Text.Trim() + " from " + txtbranch.Text + " Branch", ViewState["XMAIL"].ToString());
    //                    if (merror == "1")
    //                    {
    //                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Updated Successfully And Mail Send Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //                    }
    //                    else
    //                    {
    //                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Updated Successfully And Mail Has Not Been Send Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //                    }
    //                }
    //                else
    //                {
    //                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Updated Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //                }
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Updated Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //            }
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Updated Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //        }
    //    }
    //    else
    //    {
    //        if (txtsdrno.Text.Length == 6)
    //        {
    //            mailbody();
    //            send_mail_new(co_cd, "", "mdobriyal@maharanipaints.com,nmathurkar@maharanipaints.com,itsupport@maharanipaints.com,sanant@maharanipaints.com", "", "", "Tejaxo ERP: SDR Approval required For " + txtcust.Text.Trim() + " from " + txtbranch.Text + " Branch", ViewState["XMAIL"].ToString());
    //            if (merror == "1")
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Saved Successfully And Mail Send Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Saved Successfully And Mail Has Not Been Send Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //            }
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft No." + vchnum + " , dated " + txtdate1.Text.Trim() + " with SDR No. " + txtsdrno.Text + " , dated " + txtdate1.Text.Trim() + " Has Been Saved Successfully.','Tejaxo ERP Alert Message');});</script>", false);
    //        }
    //    }

    //    lbledmode.Value = "";
    //    lblname.Value = "";
    //    HFOLDDT.Value = "";
    //    HFOPT.Value = "";
    //   // btnexit.Text = "Exit";
    //    //disable_btn();
    //    disablectrl();
    //    btnnew.Disabled = false;
    //    btnedit.Disabled = false;
    //    btndel.Disabled = false;
    //    btnprint.Disabled = false;
    //    btnlist.Disabled = false;
    //    //btnexit.Enabled = true;
    //    btnexit.Disabled = false;
    //    MultiView1.ActiveViewIndex = 0;
    //    fgen.DisableForm(this.Controls);
    //    fgen.ResetForm(this.Controls);
    //    txtAllAttachments.Visible = false;
    //    Attch.Visible = false;
    //    btnDown.Visible = false;
    //    sg1.DataSource = null;
    //    sg1.DataBind();
    //}
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        hsdt = DateTime.Parse(txthmdate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
        sigdt = DateTime.Parse(txtsign.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
        presentdate = DateTime.Parse(vardate, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
        //invdate = DateTime.Parse(txtdate1.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
        invdate = DateTime.Parse(txtDraftDt.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
        vchdate = DateTime.Parse(txtdate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat);
        Sedtby = (string)ViewState["ENTBY"]; Sedt = (string)ViewState["ENTDT"];
        olddate = HFOLDDT.Value.ToString();
        /////
        //consql.ConnectionString = popvar;
        //da = new OracleDataAdapter(new OracleCommand("SELECT * FROM scratch where 1=2 ", consql));
        //OracleCommandBuilder cb = new OracleCommandBuilder(da);
        //da.FillSchema(oDS, SchemaType.Source);
        //DataTable pTable = oDS.Tables["Table"];
        //pTable.TableName = "scratch";
        oporow = oDS.Tables[0].NewRow();
        opt = Convert.ToInt32(rbcust.SelectedValue);
        opt1 = Convert.ToInt32(rbcust0.SelectedValue);
        opt2 = Convert.ToInt32(rbcust1.SelectedValue);
        opt3 = Convert.ToInt32(rbspeci.SelectedValue);
        opt4 = Convert.ToInt32(rbminor.SelectedValue);
        opt5 = Convert.ToInt32(rbcust3.SelectedValue);
        opt6 = Convert.ToInt32(rbdeve.SelectedValue);

        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = vardate;
            oporow["invno"] = txtenqno.Text.Trim();
            oporow["invdate"] = vardate;
            oporow["branchcd"] = frm_mbr;
            // oporow["ebr"] = txtbranch.Text.Trim();
            if (opt3 == 1)
            {
                oporow["email_id"] = "-";
            }
            else
            {
                oporow["email_id"] = opt3;
            }
            //(CASE WHEN trim(NVL(EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end
            oporow["type"] = "ES";
            oporow["col3"] = txtreques.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col1"] = txtcust.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col2"] = txtaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col11"] = txtaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col5"] = txtaddr3.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col14"] = txttel.Text.Trim();
            oporow["col6"] = txtfax.Text.Trim();
            oporow["col7"] = txtemail.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col8"] = txtnature.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col9"] = txtjusti.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col10"] = txtproduct.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col12"] = txtshade.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col23"] = opt;
            oporow["col24"] = opt1;
            oporow["col26"] = opt2;
            oporow["col57"] = txtbusin.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col56"] = txtsolid.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col15"] = txtthinner.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col18"] = txtsystem.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["PROD_CAT"] = HFOPT.Value;

            if (HFOPT.Value == "PC")
            {
                //oporow["col16"] = txtgloss.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["col17"] = txtsalt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["col19"] = txttest.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["col16"] = txtdft.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["col17"] = txtsst.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["col19"] = txtgloss.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            }
            else if (HFOPT.Value == "LP")
            {
                oporow["col16"] = txtdft.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["col17"] = txtsst.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["col19"] = txthrs.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["col19"] = txtgloss.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            }
            oporow["col28"] = txtfinish.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col27"] = txtsubs.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col22"] = txtliquid.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col35"] = txtdry.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col37"] = txtimmid.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col39"] = txtlong.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col25"] = txt1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col20"] = txt11.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col21"] = txtrecomm.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col13"] = txttime.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col59"] = txtaddition.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //  oporow["remarks"] = txtbasic.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            if (edmode.Value == "Y")
            {
                oporow["remarks"] = hf2.Value;
            }
            else
            {
                oporow["remarks"] = lblUpload.Text;
            }
            //oporow["col30"] = txtprod.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["col31"] = txtdpperiod.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["col32"] = txtdpcost.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["col33"] = txtsample.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["col34"] = txtfordp.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["col36"] = txtforreg.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");

            oporow["col30"] = sg1.Rows[i].Cells[3].Text.Trim();// code
            oporow["col31"] = ((TextBox)sg1.Rows[i].FindControl("txtremarks")).Text.Trim();// remarks
            oporow["col32"] = txtFeasDoc.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");////1
            oporow["col33"] = txtFeasRev.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");//2
            oporow["col34"] = txtFeasDate.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");///3
            oporow["col36"] = txtclosed2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");///5
            oporow["col38"] = txtfortest.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col40"] = txtremark.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col52"] = opt4;
            oporow["srno"] = i + 1;
            // oporow["col53"] = txtprodcode.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");//yes
            oporow["col53"] = ((TextBox)sg1.Rows[i].FindControl("txtyes")).Text.Trim();
            oporow["col43"] = txthead.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["docdate"] = sigdt;
            oporow["col44"] = txti.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col45"] = txtii.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col46"] = txtiii.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col47"] = txtmddate.Text.Trim();
            // oporow["col51"] = txtfeedback.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col51"] = txtAllAttachments.Text.Trim();
            if (txtAttch.Text.Length > 1)
            {
                oporow["filepath"] = lblUpload.Text.Trim();
                oporow["filename"] = txtAttch.Text.Trim();
            }
            else if (lblUpload.Text.Length > 1)
            {
                oporow["filepath"] = lblUpload.Text.Trim();
                oporow["filename"] = lblUpload.Text.Trim().Split('~')[1];
            }
            else
            {
                oporow["filepath"] = "-";
                oporow["filename"] = "-";
            }
            oporow["col49"] = txtclosed.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col50"] = hsdt;
            oporow["num1"] = fgen.make_double(Textpr1.Text.Trim());
            oporow["num2"] = fgen.make_double(Textpr2.Text.Trim());
            if (opt6 == 0) oporow["ENQ_STATUS"] = "NEW";
            if (opt6 == 1) oporow["ENQ_STATUS"] = "EXISTING";
            oporow["col48"] = txtcontact.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["col40"] = txtpre.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col85"] = txtpre.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col41"] = txtmethod.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col54"] = txtpsample.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["col55"] = opt5;

            oporow["PROD_NAME"] = txtprodname.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            //oporow["HO_STATUS"] = txtstatus.Text.ToUpper().ToString().Trim();
            oporow["COL4"] = txtstatus.Text.ToUpper().ToString().Trim();
            // new columns
            oporow["acode"] = txtUserid.Text.Trim();
            oporow["COL60"] = txtIntake.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL61"] = txtBanking.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL62"] = txtSpecific.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL63"] = txtAccelerated.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL64"] = txtAny.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL65"] = txtVolume.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL66"] = txtValue.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL67"] = txtFutureVol.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL68"] = txtFutureVal.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL69"] = txtBasicPrice.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL70"] = txtPymt.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL71"] = txtQty.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL72"] = txtSampleQty.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL73"] = txtSuggest.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL74"] = txtApproval.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            // oporow["COL75"] = txtDateTime.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL76"] = txtRefusal1.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL77"] = txtRefusal2.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL78"] = txtRegular.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL79"] = txtCostSheet.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            // oporow["COL80"] = txtSampleSize.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");////4
            oporow["COL80"] = txtfeedback.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL81"] = txtEstimated.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL82"] = txtFormat.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL83"] = txtRev.Text.Trim().ToUpper().Replace("'", " ").Replace("\"", " ");
            oporow["COL84"] = txtEffDate.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL86"] = txtiiii.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["COL87"] = txtAppAdd.Text.ToUpper().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["SDR_NO"] = txtsdrno.Text.Replace("-", " ");
            oporow["SDR_DATE"] = txtdate1.Text;
            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = Sedtby;
                oporow["ent_dt"] = sedate;
                oporow["edt_by"] = uname;
                oporow["edt_dt"] = presentdate;
                oporow["APP_BY"] = (string)ViewState["APP_BY"];
                if ((string)ViewState["APP_DT"] == null || (string)ViewState["APP_DT"] == "")
                {
                    oporow["APP_DT"] = presentdate;
                }
                else
                {
                    oporow["APP_DT"] = (string)ViewState["APP_DT"];
                }
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = presentdate;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = presentdate;
            }

            oDS.Tables[0].Rows.Add(oporow);
        }      
       // da.Update(oDS, "scratch");
    }

    public int Date_check(int setflag)
    {
        if ((txtdate1.Text == "__/__/____" || txtdate1.Text == "")) txtdate1.Text = fgen.Fn_curr_dt(co_cd, frm_qstr); //fgen.CurrDate(co_cd);
        if ((txtsign.Text == "__/__/____" || txtsign.Text == "")) txtsign.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
        if ((txtmddate.Text == "__/__/____" || txtmddate.Text == "")) txtmddate.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
        if ((txthmdate.Text == "__/__/____" || txthmdate.Text == "")) txthmdate.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
        // if ((txtEffDate.Text == "__/__/____" || txtEffDate.Text == "")) txtEffDate.Text = fgen.CurrDate(co_cd);
        // if ((txtDateTime.Text == "__/__/____" || txtDateTime.Text == "")) txtDateTime.Text = fgen.CurrDate(co_cd);
        if ((txtDraftDt.Text == "__/__/____" || txtDraftDt.Text == "")) txtDraftDt.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);
     //   pflag = fgen.DATECHECK(txtdate1.Text);
        pflag = fgen.ChkDate(txtdate1.Text.ToString());       
        if (pflag == 0) { }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid SDR Date','Tejaxo ERP Alert Message');});</script>", false);
            txtdate1.Text = fgen.Fn_curr_dt(co_cd, frm_qstr);            
            txtdate1.Focus();
            nflag = 1;
        }
        pflag = fgen.ChkDate(txtsign.Text.ToString());          
        if (pflag == 0) { }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid Sign/Date','Tejaxo ERP Alert Message');});</script>", false);
            txtsign.Text = fgen.Fn_curr_dt(co_cd, frm_qstr); txtsign.Focus();
            nflag = 1;
        }      
        pflag = fgen.ChkDate(txtmddate.Text.ToString());      
        if (pflag == 0) { }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid M.D Sign Date','Tejaxo ERP Alert Message');});</script>", false);
            txtmddate.Text = fgen.Fn_curr_dt(co_cd, frm_qstr); txtmddate.Focus();
            nflag = 1;
        }     
        pflag = fgen.ChkDate(txthmdate.Text.ToString());    
        if (pflag == 0) { }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid Head Marketing Sign Date','Tejaxo ERP Alert Message');});</script>", false);
            txthmdate.Text = fgen.Fn_curr_dt(co_cd, frm_qstr); txthmdate.Focus();
            nflag = 1;
        }
        //pflag = fgen.DATECHECK(txtEffDate.Text);
        //if (pflag == 0) { }
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid Effective Date','Tejaxo ERP Alert Message');});</script>", false);
        //    txtEffDate.Text = fgen.CurrDate(co_cd); txtEffDate.Focus();
        //    nflag = 1;
        // }
        //pflag = fgen.DATECHECK(txtDateTime.Text);
        //if (pflag == 0) { }
        //else
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid Date Time','Tejaxo ERP Alert Message');});</script>", false);
        //    txtDateTime.Text = fgen.CurrDate(co_cd); txtDateTime.Focus();
        //    nflag = 1;
        //}    
        pflag = fgen.ChkDate(txtDraftDt.Text.ToString());    
        if (pflag == 0) { }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Invalid Draft Date','Tejaxo ERP Alert Message');});</script>", false);
            txtDraftDt.Text = fgen.Fn_curr_dt(co_cd, frm_qstr); txtDraftDt.Focus();
            nflag = 1;
        }
        DateTime dt = DateTime.ParseExact(txtdate.Text, "dd/MM/yyyy", null);
        //DateTime dt2 = DateTime.ParseExact(txtdate1.Text, "dd/MM/yyyy", null);
        DateTime dt2 = DateTime.ParseExact(txtDraftDt.Text, "dd/MM/yyyy", null);
        if (dt2.Date < dt.Date)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Draft date cannot less than Enquiry date.','Tejaxo ERP Alert Message');});</script>", false);
            nflag = 1;
        }
        return nflag;
    }

    protected void cmd_btn_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 0;
    }

    protected void cmd_btn2_Click(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = 1;
    }

    protected void btnstaus_Click(object sender, EventArgs e)
    {
        lblname.Value = "ST";
      //  SendQuery();
        make_qry_4_popup();
        ScriptManager.RegisterStartupScript(btnstaus, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
    }

    public string fill_dash(ControlCollection control)
    {
        string value = "0";
        foreach (System.Web.UI.Control c in control)
        {
            if (c is TextBox)
            {
                if ((((TextBox)c).Text.Trim() == null) || (((TextBox)c).Text.Trim() == "") || (((TextBox)c).Text.Trim() == "-"))
                {

                    ((TextBox)c).BackColor = Color.Red;

                    return ((TextBox)c).ID;

                }
            }
            //else
            //{
            //    if (c.HasControls()) fill_dash(c.Controls);
            //}
        }

        return value;
    }

    protected int Check_All_Fields(int setflag)
    {
        //if (txtFormat.Text == "-" || txtFormat.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please fill Format No.','Tejaxo ERP Alert Message');});</script>", false);
        //    txtFormat.Focus();
        //    nflag = 1;
        //}

        //if (txtRev.Text == "-" || txtRev.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Revision No.','Tejaxo ERP Alert Message');});</script>", false);
        //    txtRev.Focus();
        //    nflag = 1;
        //}

        //if (txtEffDate.Text == "-" || txtEffDate.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Effective Date','Tejaxo ERP Alert Message');});</script>", false);
        //    txtEffDate.Focus();
        //    nflag = 1;
        //}

        if (txtcust.Text == "-" || txtcust.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Customer Name','Tejaxo ERP Alert Message');});</script>", false);
            txtcust.Focus();
            nflag = 1;
        }

        if (txtcontact.Text == "-" || txtcontact.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Contact No.','Tejaxo ERP Alert Message');});</script>", false);
            txtcontact.Focus();
            nflag = 1;
        }

        if (txtaddr1.Text == "-" || txtaddr1.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Address','Tejaxo ERP Alert Message');});</script>", false);
            txtaddr1.Focus();
            nflag = 1;
        }

        if (txttel.Text == "-" || txttel.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Telephone No.','Tejaxo ERP Alert Message');});</script>", false);
            txttel.Focus();
            nflag = 1;
        }

        if (txtemail.Text == "-" || txtemail.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Email','Tejaxo ERP Alert Message');});</script>", false);
            txtemail.Focus();
            nflag = 1;
        }

        if (txtnature.Text == "-" || txtnature.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Nature of Business','Tejaxo ERP Alert Message');});</script>", false);
            txtnature.Focus();
            nflag = 1;
        }

        if (txtjusti.Text == "-" || txtjusti.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Justification of Development','Tejaxo ERP Alert Message');});</script>", false);
            txtjusti.Focus();
            nflag = 1;
        }

        if (txtproduct.Text == "-" || txtproduct.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Product Type','Tejaxo ERP Alert Message');});</script>", false);
            txtproduct.Focus();
            nflag = 1;
        }

        if (txtshade.Text == "-" || txtshade.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Shade','Tejaxo ERP Alert Message');});</script>", false);
            txtshade.Focus();
            nflag = 1;
        }

        if (txtfinish.Text == "-" || txtfinish.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Finish','Tejaxo ERP Alert Message');});</script>", false);
            txtfinish.Focus();
            nflag = 1;
        }

        if (txtgloss.Text == "-" || txtgloss.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill GLoss 20*/60*','Tejaxo ERP Alert Message');});</script>", false);
            txtgloss.Focus();
            nflag = 1;
        }

        if (txtsubs.Text == "-" || txtsubs.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Substrate Type','Tejaxo ERP Alert Message');});</script>", false);
            txtsubs.Focus();
            nflag = 1;
        }

        if (HFOPT.Value != "PC")
        {
            if (txtpre.Text == "-" || txtpre.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Pre-Treatment Type','Tejaxo ERP Alert Message');});</script>", false);
                txtpre.Focus();
                nflag = 1;
            }

            if (txtmethod.Text == "-" || txtmethod.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Application Method ','Tejaxo ERP Alert Message');});</script>", false);
                txtmethod.Focus();
                nflag = 1;
            }

            if (txtthinner.Text == "-" || txtthinner.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Application Viscosity','Tejaxo ERP Alert Message');});</script>", false);
                txtthinner.Focus();
                nflag = 1;
            }

            if (txtIntake.Text == "-" || txtIntake.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Thinner Intake Online','Tejaxo ERP Alert Message');});</script>", false);
                txtIntake.Focus();
                nflag = 1;
            }

            if (txtSpecific.Text == "-" || txtSpecific.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Specific Liquid Properties','Tejaxo ERP Alert Message');});</script>", false);
                txtSpecific.Focus();
                nflag = 1;
            }

            if (txtdft.Text == "-" || txtdft.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill DFT STD Specified by Customer','Tejaxo ERP Alert Message');});</script>", false);
                txtdft.Focus();
                nflag = 1;
            }

        }
        else
        {
            if (txtpre.Text == "-" || txtpre.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Pre-Treatment System & Type','Tejaxo ERP Alert Message');});</script>", false);
                txtpre.Focus();
                nflag = 1;
            }

            if (txtdft.Text == "-" || txtdft.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Type of Oven','Tejaxo ERP Alert Message');});</script>", false);
                txtdft.Focus();
                nflag = 1;
            }

        }
        if (txtsystem.Text == "-" || txtsystem.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Application System','Tejaxo ERP Alert Message');});</script>", false);
            txtsystem.Focus();
            nflag = 1;
        }

        if (txtBanking.Text == "-" || txtBanking.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Banking Schedule','Tejaxo ERP Alert Message');});</script>", false);
            txtBanking.Focus();
            nflag = 1;
        }

        if (txtsst.Text == "-" || txtsst.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill SST Required pls Specify HRS','Tejaxo ERP Alert Message');});</script>", false);
            txtsst.Focus();
            nflag = 1;
        }

        if (txtAccelerated.Text == "-" || txtAccelerated.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Accelerated Weathering Test','Tejaxo ERP Alert Message');});</script>", false);
            txtAccelerated.Focus();
            nflag = 1;
        }

        if (txtAny.Text == "-" || txtAny.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Any Other Potential Test','Tejaxo ERP Alert Message');});</script>", false);
            txtAny.Focus();
            nflag = 1;
        }

        //if (txtbusin.Text == "-" || txtbusin.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Business Potential','Tejaxo ERP Alert Message');});</script>", false);
        //    txtbusin.Focus();
        //    nflag = 1;
        //}

        if (txtVolume.Text == "-" || txtVolume.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Volume of Total Business Potential of Customer','Tejaxo ERP Alert Message');});</script>", false);
            txtVolume.Focus();
            nflag = 1;
        }

        if (txtValue.Text == "-" || txtValue.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Value of Total Business Potential of Customer','Tejaxo ERP Alert Message');});</script>", false);
            txtValue.Focus();
            nflag = 1;
        }

        if (txtimmid.Text == "-" || txtimmid.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Business Expected (Immediate)','Tejaxo ERP Alert Message');});</script>", false);
            txtimmid.Focus();
            nflag = 1;
        }

        if (txtlong.Text == "-" || txtlong.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Business Expected (Long)','Tejaxo ERP Alert Message');});</script>", false);
            txtlong.Focus();
            nflag = 1;
        }

        if (txtFutureVol.Text == "-" || txtFutureVol.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Future Business Volume','Tejaxo ERP Alert Message');});</script>", false);
            txtFutureVol.Focus();
            nflag = 1;
        }

        if (txtFutureVal.Text == "-" || txtFutureVal.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Future Business Value','Tejaxo ERP Alert Message');});</script>", false);
            txtFutureVal.Focus();
            nflag = 1;
        }

        if (txt1.Text == "-" || txt1.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Present Supplier 1','Tejaxo ERP Alert Message');});</script>", false);
            txt1.Focus();
            nflag = 1;
        }

        if (Textpr1.Text == "-" || Textpr1.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Present Supplier 1 Basic Price','Tejaxo ERP Alert Message');});</script>", false);
            Textpr1.Focus();
            nflag = 1;
        }

        if (txt11.Text == "-" || txt11.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Present Supplier 2','Tejaxo ERP Alert Message');});</script>", false);
            txt11.Focus();
            nflag = 1;
        }

        if (Textpr2.Text == "-" || Textpr2.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Present Supplier 2 Basic Price','Tejaxo ERP Alert Message');});</script>", false);
            Textpr2.Focus();
            nflag = 1;
        }

        if (txtBasicPrice.Text == "-" || txtBasicPrice.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Basic Price at which Business Can be Obtained','Tejaxo ERP Alert Message');});</script>", false);
            txtBasicPrice.Focus();
            nflag = 1;
        }

        if (txtPymt.Text == "-" || txtPymt.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Payment Terms of Customer','Tejaxo ERP Alert Message');});</script>", false);
            txtPymt.Focus();
            nflag = 1;
        }

        //if (txtpsample.Text == "-" || txtpsample.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Competitors Product Sample','Tejaxo ERP Alert Message');});</script>", false);
        //    txtpsample.Focus();
        //    nflag = 1;
        //}

        if (txtQty.Text == "-" || txtQty.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Qty','Tejaxo ERP Alert Message');});</script>", false);
            txtQty.Focus();
            nflag = 1;
        }

        if (txttime.Text == "-" || txttime.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Time Frame for Development','Tejaxo ERP Alert Message');});</script>", false);
            txttime.Focus();
            nflag = 1;
        }

        if (txtSampleQty.Text == "-" || txtSampleQty.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Sample Qty Required For Trail','Tejaxo ERP Alert Message');});</script>", false);
            txtSampleQty.Focus();
            nflag = 1;
        }

        if (txtSuggest.Text == "-" || txtSuggest.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Suggest Final Name of the Product ','Tejaxo ERP Alert Message');});</script>", false);
            txtSuggest.Focus();
            nflag = 1;
        }

        if (txtaddition.Text == "-" || txtaddition.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Additional Information(If Any): ','Tejaxo ERP Alert Message');});</script>", false);
            txtaddition.Focus();
            nflag = 1;
        }

        if (txtreques.Text == "-" || txtreques.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Requested By BDM Name ','Tejaxo ERP Alert Message');});</script>", false);
            txtreques.Focus();
            nflag = 1;
        }

        //if (txtrecomm.Text == "-" || txtrecomm.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Approved By (Marketing Head Name)','Tejaxo ERP Alert Message');});</script>", false);
        //    txtrecomm.Focus();
        //    nflag = 1;
        //}

        //if (txtRefusal1.Text == "-" || txtRefusal1.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Remarks for Refusal','Tejaxo ERP Alert Message');});</script>", false);
        //    txtRefusal1.Focus();
        //    nflag = 1;bt
        //}

        if (Dept.Length >= 1)
        {
            if (Dept.Substring(0, 1) == "R")
            {
                if (txtApproval.Text == "-" || txtApproval.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Approval From R&D Head Date & Time ','Tejaxo ERP Alert Message');});</script>", false);
                    txtApproval.Focus();
                    nflag = 1;
                }

                if (txtRefusal2.Text == "-" || txtRefusal2.Text == "")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Remarks for Refusal','Tejaxo ERP Alert Message');});</script>", false);
                    txtRefusal2.Focus();
                    nflag = 1;
                }
            }
        }
        #region Second Tab
        //if (txtprod.Text == "-" || txtprod.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Product Requirement Clear and achievable ','Tejaxo ERP Alert Message');});</script>", false);
        //    txtprod.Focus();
        //    nflag = 1;
        //}

        //if (txtprodcode.Text == "-" || txtprodcode.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Exisiting Product Code','Tejaxo ERP Alert Message');});</script>", false);
        //    txtprodcode.Focus();
        //    nflag = 1;
        //}

        //if (txtdpperiod.Text == "-" || txtdpperiod.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Development Period','Tejaxo ERP Alert Message');});</script>", false);
        //    txtdpperiod.Focus();
        //    nflag = 1;
        //}

        //if (txtdpcost.Text == "-" || txtdpcost.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Development Cost','Tejaxo ERP Alert Message');});</script>", false);
        //    txtdpcost.Focus();
        //    nflag = 1;
        //}

        //if (txtRegular.Text == "-" || txtRegular.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Regular Cost of Manufacture','Tejaxo ERP Alert Message');});</script>", false);
        //    txtRegular.Focus();
        //    nflag = 1;
        //}

        //if (txtCostSheet.Text == "-" || txtCostSheet.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Cost Sheet Reference','Tejaxo ERP Alert Message');});</script>", false);
        //    txtCostSheet.Focus();
        //    nflag = 1;
        //}

        //if (txtSampleSize.Text == "-" || txtSampleSize.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill No. of Samples/ Sample size','Tejaxo ERP Alert Message');});</script>", false);
        //    txtSampleSize.Focus();
        //    nflag = 1;
        //}

        //if (txtfordp.Text == "-" || txtfordp.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill For Development','Tejaxo ERP Alert Message');});</script>", false);
        //    txtfordp.Focus();
        //    nflag = 1;
        //}

        //if (txtforreg.Text == "-" || txtforreg.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill For Regular Production ','Tejaxo ERP Alert Message');});</script>", false);
        //    txtforreg.Focus();
        //    nflag = 1;
        //}

        //if (txtfortest.Text == "-" || txtfortest.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill For Testing','Tejaxo ERP Alert Message');});</script>", false);
        //    txtfortest.Focus();
        //    nflag = 1;
        //}

        //if (txtEstimated.Text == "-" || txtEstimated.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Total estimated Capital Cost ','Tejaxo ERP Alert Message');});</script>", false);
        //    txtEstimated.Focus();
        //    nflag = 1;
        //}

        //if (txthead.Text == "-" || txthead.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Head R&D','Tejaxo ERP Alert Message');});</script>", false);
        //    txthead.Focus();
        //    nflag = 1;
        //}

        //if (txtsign.Text == "-" || txtsign.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill  Sig/Date','Tejaxo ERP Alert Message');});</script>", false);
        //    txtsign.Focus();
        //    nflag = 1;
        //}

        //if (txti.Text == "-" || txti.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill CFT Members Name 1','Tejaxo ERP Alert Message');});</script>", false);
        //    txti.Focus();
        //    nflag = 1;
        //}

        //if (txtii.Text == "-" || txtii.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill CFT Members Name 2','Tejaxo ERP Alert Message');});</script>", false);
        //    txtii.Focus();
        //    nflag = 1;
        //}

        //if (txtiii.Text == "-" || txtiii.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill CFT Members Name 3','Tejaxo ERP Alert Message');});</script>", false);
        //    txtiii.Focus();
        //    nflag = 1;
        //}

        //if (txtiiii.Text == "-" || txtiiii.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill CFT Members Name 4','Tejaxo ERP Alert Message');});</script>", false);
        //    txtiiii.Focus();
        //    nflag = 1;
        //}

        //if (txtAppAdd.Text == "-" || txtAppAdd.Text == "")
        //{
        //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "<script>$(document).ready(function(){jAlert('Please Fill Approval of Additional Capital Cost','Tejaxo ERP Alert Message');});</script>", false);
        //    txtAppAdd.Focus();
        //    nflag = 1;
        //}
        #endregion
        return nflag;
    }

    protected void rbspeci_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbspeci.SelectedItem.Text == "Attached")
        {
            //Attch.Visible = true;
            img.Visible = true;
            attch1.Visible = true;
            txtatthment1.Visible = true; 
            //txtatch.Visible = true;          
            disablectrl();         
        }
        else
        {
            img.Visible = true;
            attch1.Visible = true;
            txtatthment1.Visible = true; 
            //img.Visible = false;
            //attch1.Visible = false;
            ////txtatch.Visible = false;
            //txtatthment1.Visible = false;           
            ////  Attch.Visible = false;
            ////     txtAllAttachments.Visible = false;
            
        }
    }
    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        // ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/tej_erp/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }

    protected void btnAtt_Click(object sender, EventArgs e)
    {
         string filepath = @"c:\TEJ_ERP\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        filepath = Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + "_" + txtDraft.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            filepath = Server.MapPath("~/tej-base/UPLOAD/") + "_" + txtDraft.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            lblUpload.Text = filepath;           
            btnView1.Visible = true;           
            btnDown.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
        ///=======================================
        //filepath = Server.MapPath("~/UPLOADS/");
        //hffield.Value = filepath;
        //Attch.Visible = true;
        //lblUpload.Text = "";
        //txtAllAttachments.Text = "";
        //hf2.Value = "";
        //if (Attch.HasFile)
        //{
        //    txtAllAttachments.Text = txtAllAttachments.Text + "," + Attch.FileName;
        //    txtAllAttachments.Text = txtAllAttachments.Text.TrimStart(',');
        //    txtAllAttachments.Visible = true;
        //    filepath = filepath + "\\" + co_cd.Trim() + mbr + txtDraft.Text + "~" + Attch.FileName;
        //    hf2.Value = hf2.Value + "," + filepath;
        //    Attch.PostedFile.SaveAs(filepath);
        //    lblUpload.Text = filepath;
        //    lblUpload.Text = hf2.Value.TrimStart(',');
        //    btnDown.Visible = true;
        //}
        //else
        //{
        //    lblUpload.Text = "";
        //    lblShow.Text = "";
        //}

    }

    protected void btnDown_Click(object sender, EventArgs e)
    {
        //if (txtAllAttachments.Text.Length > 1)
        //{
        //    string filePath = Server.MapPath("~/UPLOADS/") + co_cd.Trim() + mbr + txtDraft.Text + "~" + txtAllAttachments.Text;
        //    Response.ContentType = ContentType;
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
        //    Response.WriteFile(filePath);
        //    Response.End();
        //}
        //=======================
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));

            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }

    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }

    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[4].Text.ToString().Length > 45)
            {
                e.Row.Cells[4].ToolTip = e.Row.Cells[4].Text;
                e.Row.Cells[4].Text = e.Row.Cells[4].Text.ToString().Substring(0, 45) + "...";
            }
            e.Row.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
            sg1.HeaderRow.Cells[1].Style["display"] = "none";
        }
    }

    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("code", typeof(string)));
        dt1.Columns.Add(new DataColumn("name", typeof(string)));
        dt1.Columns.Add(new DataColumn("yes", typeof(string)));
        dt1.Columns.Add(new DataColumn("remarks", typeof(string)));
    }

    public void add_blankrows()
    {
        dr1 = dt1.NewRow();
        dr1["Srno"] = dt1.Rows.Count + 1;
        dr1["code"] = "-";
        dr1["name"] = "-";
        dr1["yes"] = "-";
        dr1["remarks"] = "-";
        dt1.Rows.Add(dr1);
    }

    public void chk_email_info(string co_cd, string check_file)
    {
        string str, path;

        if (co_cd.Substring(0, 1) == "A" || co_cd.Substring(0, 1) == "B" || co_cd.Substring(0, 1) == "C" || co_cd.Substring(0, 1) == "D" || co_cd.Substring(0, 1) == "E")
        {
            sender_id = "erp1@pocketdriver.in";
            pwd = "erp_2016";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "F" || co_cd.Substring(0, 1) == "G" || co_cd.Substring(0, 1) == "H" || co_cd.Substring(0, 1) == "I" || co_cd.Substring(0, 1) == "J")
        {
            sender_id = "erp2@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "K" || co_cd.Substring(0, 1) == "L" || co_cd.Substring(0, 1) == "M" || co_cd.Substring(0, 1) == "N" || co_cd.Substring(0, 1) == "O")
        {
            sender_id = "erp3@pocketdriver.in";
            pwd = "erp_2016";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "P" || co_cd.Substring(0, 1) == "Q" || co_cd.Substring(0, 1) == "R" || co_cd.Substring(0, 1) == "S" || co_cd.Substring(0, 1) == "T")
        {
            sender_id = "erp4@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "U" || co_cd.Substring(0, 1) == "V" || co_cd.Substring(0, 1) == "W" || co_cd.Substring(0, 1) == "X" || co_cd.Substring(0, 1) == "Y" || co_cd.Substring(0, 1) == "Z")
        {
            sender_id = "erp4@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        xvip = "1";
        xport = "465";
        path = @"c:\TEJ_ERP\email_info.txt";
        if (check_file == "2")
        {
            // Checking for Second file
            path = @"c:\TEJ_ERP\email_info2.txt";
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                str = sr.ReadToEnd().Trim();
                if (str.Contains("\r")) str = str.Replace("\r", ",");
                if (str.Contains("\n")) str = str.Replace("\n", ",");
                str = str.Replace(",,", ",");
                if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                else
                {
                    sender_id = str.Split(',')[0].ToString().Trim();
                    pwd = str.Split(',')[1].ToString().Trim();
                    vsmtp = str.Split(',')[2].ToString().Trim();
                    xvip = str.Split(',')[3].ToString().Trim();
                    xport = str.Split(',')[4].ToString().Trim();
                    //ViewState["CCMID"] = str.Split('=')[1].ToString().Trim();
                }
            }
            else
            {
                StreamWriter tw = File.AppendText(path);
                tw.WriteLine("Email From");
                tw.WriteLine("Password");
                tw.WriteLine("SMTP");
                tw.WriteLine("SSL==> 1 if True, 0 if false");
                tw.WriteLine("PORT");
                //tw.WriteLine("CC=");
                tw.Close();
            }
            ssl = Convert.ToInt32(xvip);
            port = Convert.ToInt32(xport);
        }
        else
        {
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                str = sr.ReadToEnd().Trim();
                if (str.Contains("\r")) str = str.Replace("\r", ",");
                if (str.Contains("\n")) str = str.Replace("\n", ",");
                str = str.Replace(",,", ",");
                if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                else
                {
                    sender_id = str.Split(',')[0].ToString().Trim();
                    pwd = str.Split(',')[1].ToString().Trim();
                    vsmtp = str.Split(',')[2].ToString().Trim();
                    xvip = str.Split(',')[3].ToString().Trim();
                    xport = str.Split(',')[4].ToString().Trim();
                    //ViewState["CCMID"] = str.Split('=')[1].ToString().Trim();
                }
            }
            else
            {
                StreamWriter tw = File.AppendText(path);
                tw.WriteLine("Email From");
                tw.WriteLine("Password");
                tw.WriteLine("SMTP");
                tw.WriteLine("SSL==> 1 if True, 0 if false");
                tw.WriteLine("PORT");
                //tw.WriteLine("CC=");
                tw.Close();
            }
            ssl = Convert.ToInt32(xvip);
            port = Convert.ToInt32(xport);
        }
    }

    public string send_mail_new(string co_cd, string name, string to, string Cc, string Bcc, string subj, string body)
    {
        merror = ""; string[] mul;
        try
        {
            co_cd_fgen = co_cd;
            mail = new MailMessage();

            chk_email_info(co_cd_fgen, "1");
            //mail.From = new MailAddress(name + "<" + sender_id + ">");
            mail.From = new MailAddress(sender_id);

            mail.Subject = subj;
            mail.Body = body;
            mail.IsBodyHtml = true;
            if (to.Contains(",") || to.Contains(";"))
            {
                to = to.Replace(";", ",");
                mul = to.Split(',');
                foreach (string mul_id in mul)
                {
                    mail.To.Add(new MailAddress(mul_id));
                }
            }
            else
            {
                //   to = "madhvi@pocketdriver.in";
                to = to.Replace(";", ""); to = to.Replace(",", "");
                mail.To.Add(new MailAddress(to));
            }
            if (Cc.Trim().Length > 0)
            {
                if (Cc.Contains(",") || Cc.Contains(";"))
                {
                    Cc = Cc.Replace(";", ",");
                    mul = Cc.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.CC.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                    mail.CC.Add(new MailAddress(Cc));
                }
            }
            if (Bcc.Trim().Length > 0)
            {
                if (Bcc.Contains(",") || Bcc.Contains(";"))
                {
                    Bcc = Bcc.Replace(";", ",");
                    mul = Bcc.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.Bcc.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                    mail.Bcc.Add(new MailAddress(Bcc));
                }
            }
            //message.Attachments.Add(new Attachment(oStream, "SDR_" + txtsdrno.Text + ".pdf"));
            Attachment();
            Attachment atchfile = new Attachment(report.ExportToStream(ExportFormatType.PortableDocFormat), co_cd + "SDR_" + txtsdrno.Text + ".pdf");
            mail.Attachments.Add(atchfile);

            smtp = new SmtpClient();
            {
                System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(sender_id, pwd);

                smtp.UseDefaultCredentials = false;
                //***********************************
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;                
                smtp.Credentials = SMTPUserInfo;
            }
            smtp.Send(mail);
            merror = "1";

            //FILL_ERR("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
        }
        catch (Exception ex)
        {
            merror = "0";
            FILL_ERR(co_cd_fgen + " " + ex.Message);
            FILL_ERR("Rcv ID: " + to);
            FILL_ERR("Sender ID: " + sender_id);
        }
        mail.Dispose(); smtp = null;
        return merror;
    }
    public void FILL_ERR(string msg)
    {
        string ppath = @"c:\TEJ_ERP\err.txt";
        try
        {
            if (File.Exists(ppath))
            {
                StreamWriter w = File.AppendText(ppath);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
            else
            {
                StreamWriter w = new StreamWriter(ppath, true);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
        }
        catch { }
    }
    public void mailbody()
    {
        string xmail_body = "", reques = "", sprice = "", basic_price = "", prod_type = "", shade = "", immid = "";
        string cust_name = "", mbrname = "";// Subject = "",xsmpt_client = "", xemail_fromt_user_password = "";
        //  Subject = "Tejaxo ERP: SDR Approval required For " + cust_name + " from " + mbrname + " Branch";

        //xsmpt_client = "smtp.gmail.com";
        // xemail_fromt_user_password = "shade48";

        xmail_body = "<html><body><h1></h1>";
        xmail_body = xmail_body + "<br>";


        sprice = "Basic";

        reques = txtreques.Text.Trim();
        mbrname = txtbranch.Text.Trim();
        cust_name = txtcust.Text.Trim();
        prod_type = txtproduct.Text.Trim();
        shade = txtshade.Text.Trim();
        immid = txtQty.Text.Trim();
        basic_price = txtBasicPrice.Text;
        xmail_body = xmail_body + "<B><font face='calibri' size='3' > Dear Sir,</B>";
        xmail_body = xmail_body + "<BR><B>Greetings of the day !!!</B><br>";
        xmail_body = xmail_body + "<BR>I had reviewed the Attached SDR generated by <b>" + uname + "</b> from <b>" + mbrname + "</b> <br>";
        xmail_body = xmail_body + "<BR>with reference to Customer Name <b>" + cust_name + "</b> for Product Type <b>" + prod_type + "</b> and shade <b>" + shade + "</b><br>";
        xmail_body = xmail_body + "<BR>We have offered price of Rs <b>" + basic_price + " " + sprice + "</b> in respect to quantity of <b> " + immid + "</b><br>";
        // xmail_body = xmail_body + "<br>& long term quantity of <b>" + long_term + ".</b><br>";
        xmail_body = xmail_body + "<br><b>Kindly accord your approval.</b>";
        xmail_body = xmail_body + "<br><b>Thanks & Regards,</b>";
        xmail_body = xmail_body + "<BR><b>" + uname + "</b>";
        xmail_body = xmail_body + "<br>Please click link given below to access the Approval Screen";
        xmail_body = xmail_body + "<br>";
        xmail_body = xmail_body + "<a href ='http://103.254.98.130:1517/ASPNET_CLIENT/'>" + co_cd + " Web Link</a></font>";
        xmail_body = xmail_body + "</body></html>";
        ViewState["XMAIL"] = xmail_body;
    }

    private void Attachment()
    {
        scode = mbr + "ES" + txtDraft.Text.Trim() + txtDraftDt.Text.Trim();
        dt = new DataTable();
        string ebrcode = fgen.seek_iname(frm_qstr,co_cd, "Select BRANCHCD from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + scode + "')", "BRANCHCD");
        dt = fgen.getdata(frm_qstr,co_cd, "SELECT TYPE1,NAME,ADDR,ADDR1,ADDR2,PLACE,TELE,FAX FROM TYPE WHERE ID='B' AND TYPE1='" + ebrcode + "'");
        if (dt.Rows.Count > 0)
        {
            br_name = dt.Rows[0]["name"].ToString().Trim();
            br_addr = dt.Rows[0]["addr"].ToString().Trim();
            br_addr1 = dt.Rows[0]["addr1"].ToString().Trim();
            br_addr2 = dt.Rows[0]["addr2"].ToString().Trim();
            br_place = dt.Rows[0]["place"].ToString().Trim();
            br_tele = dt.Rows[0]["tele"].ToString().Trim();
            br_fax = dt.Rows[0]["fax"].ToString().Trim();
        }
        string prod_cat1 = fgen.seek_iname(frm_qstr,co_cd, "select nvl(PROD_cAT,'LP') as PROD_cAT from scratch where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + scode + "')", "prod_cat");
        HFOPT.Value = prod_cat1;
     //   popvar = fgen.Con2OLE(co_cd);
        consql.ConnectionString = popvar;
       // firm = fgen.CHECK_CO(co_cd);
       
        DataSet ds = new DataSet();
        da1 = new OracleDataAdapter("Select A.ACODE,E.USERNAME,a.branchcd,ebr,nvl(PROD_cAT,'LP') as PROD_cAT,nvl(PROD_NAME,'-') as PROD_NAME,nvl(HO_STATUS,'-') as HO_STATUS,COL4 AS MDNAME, col56,nvl(col57,'-') as col57, nvl(num1,0) as num1,nvl(num2,0) as num2,ENQ_STATUS,(CASE WHEN trim(NVL(col55,'0')) = '0' THeN 'Basic' else 'Selling' end ) as col55,(CASE WHEN trim(NVL(EMAIL_ID,'-')) = '-' THeN 'Not Attached' else 'Attached' end ) as EMAIL_ID,(CASE WHEN trim(NVL(col26,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col26,(CASE WHEN trim(NVL(col23,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col23,(CASE WHEN trim(NVL(col24,'0')) = '0' THeN 'Available' else 'Not Available' end ) as col24,  invno,to_char(invdate,'dd/mm/yyyy') as invdate, NVL(COL51,'-') AS COL51,NVL(COL52,'0') AS COL52,NVL(COL53,'-') AS COL53, COL54, vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col21, col11,col5,col59 AS COL4,col6,col7,col8,col9,col10,col12,col27,col15,col18,nvl(col16,'-') as col16,nvl(col17,'-') as col17,nvl(col19,'-') as col19,col28,col22,col35,col37,col39,col25,col20,col13,col14,remarks,col40,NVL(col30,'-') AS COL30,NVL(col31,'-') AS COL31,NVL(col32,'-') AS COL32,NVL(col33,'-') AS COL33,NVL(col34,'-') AS col34,NVL(col36,'-') AS col36,NVL(col38,'-') AS col38,col41,col42,NVL(col43,'-') AS col43,TO_CHAR(NVL(docdate,SYSDATE),'DD/MM/YYYY') as docdate,NVL(col44,'-') AS  col44,NVL(col45,'-') AS col45,NVL(col46,'-') AS col46,NVL(COL47,'-') AS col47,NVL(col48,'-') AS col48,NVL(col49,'-') AS col49,TO_CHAR(NVL(COL50,SYSDATE),'DD/MM/YYYY') as col50,A.ent_by,to_char(A.ent_dt,'dd/mm/yyyy') as ent_dt,'" + br_addr + "' as addr,'" + br_addr1 + "' as addr1,'" + br_addr2 + "' as addr2,'" + br_place + "' as place,'" + br_tele + "' as tele,'" + br_fax + "' as fax,'" + firm + "' as firm,NVL(col60,'-') AS col60,NVL(col61,'-') AS col61,NVL(col62,'-') AS col62,NVL(col63,'-') AS col63,NVL(col64,'-') AS col64,NVL(col65,'-') AS col65,NVL(col66,'-') AS col66,NVL(col67,'-') AS col67,NVL(col68,'-') AS col68,NVL(col69,'-') AS col69, NVL(col70,'-') AS col70,NVL(col71,'-') AS col71,NVL(col72,'-') AS col72,NVL(col73,'-') AS col73,NVL(col74,'-') AS col74,NVL(col75,'-') AS col75,NVL(col76,'-') AS col76,NVL(col77,'-') AS col77,NVL(col78,'-') AS col78,NVL(col79,'-') AS col79,NVL(col80,'-') AS col80,NVL(col81,'-') AS col81,NVL(col82,'-') AS col82,NVL(col83,'-') AS col83,NVL(col84,'-') AS col84,NVL(col85,'-') AS col85,NVL(col86,'-') AS col86,NVL(col87,'-') AS col87,SDR_NO,TO_CHAR(SDR_DATE,'DD/MM/YYYY') AS SDR_DATE from scratch A,EVAS E where TRIM(A.ACODE)=TRIM(E.USERID) AND A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_CHAr(A.vchdate,'DD/MM/YYYY') in ('" + scode + "') order by a.col30", consql);
        da1.Fill(ds, "Prepcur");
        string str2 = Server.MapPath("~/xmlreport/Enquiry.xml");
        ds.WriteXml(str2, XmlWriteMode.WriteSchema);
        Session["mydataset"] = ds;
        if (HFOPT.Value == "LP") HP = new HttpCookie("xCRFILE", "~/Report/SDR.rpt");
        if (HFOPT.Value == "PC") HP = new HttpCookie("xCRFILE", "~/Report/SDR_.rpt");
        Response.Cookies.Add(HP);
        string xCRFILE = Request.Cookies["xCRFILE"].Value;
        report = new ReportDocument();
        string rptfile = Server.MapPath("" + xCRFILE + "");
        report.Load(rptfile);
        report.Refresh();
        report.SetDataSource(ds);

    }

}

// ALTER TABLE FINMINV.SCRATCH ADD FILEPATH VARCHAR2(100);
// ALTER TABLE FINMINV.SCRATCH ADD FILENAME VARCHAR2(100);

