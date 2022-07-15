using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;

public partial class om_final_qtn : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
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

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            //btnprint.Visible = false;
            typePopup = "N";
        }
    }
    //------------------------------------------------------------------------------------
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
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            getColHeading();
        }
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
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
        doc_nf.Value = "ordno";
        doc_df.Value = "orddt";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "somasq";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "FQ");
        lblheader.Text = "Component Cost + Tooling Cost";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
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

            case "item":
                SQuery = "select distinct trim(a.icode) as fstr,trim(a.icode) as item_code,trim(b.iname) as item_name ,b.unit,trim(b.cpartno) as part_no from itwstage a, item b where trim(a.icode)=trim(b.icode) and length(trim(a.icode))>4 and substr((trim(a.icode),1,2)>='7' order by trim(a.icode)";
                break;

            case "RFQ":
                //SQuery = "select TRIM(A.FSTR) AS FSTR,TRIM(A.VCHNUM) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,TRIM(A.ICODE) AS ITEM_CODE,TRIM(I.INAME) AS ITEM_NAME,TRIM(A.ACODE) AS CUSTOMER_CODE,TRIM(B.ANAME) AS CUSTOMER_NAME from (select branchcd||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,vchdate,icode,ACODE,1 AS QTY from wb_cacost where branchcd='" + frm_mbr + "' and type ='CA01' union all select distinct branchcd||'CA01'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO AS VCHNUM,INVDATE AS VCHDATE,icode,ACODE,-1 AS QTY from wb_porfq WHERE  branchcd='" + frm_mbr + "' and type='FQ') a,item i,FAMST B where trim(a.icode)=trim(i.icode) AND trim(a.Acode)=trim(B.Acode) group by TRIM(A.FSTR) ,A.VCHNUM,A.VCHDATE,A.ICODE,A.ACODE,I.INAME,B.ANAME HAVING SUM(A.QTY)>0 ORDER BY FSTR";
                SQuery = "select TRIM(A.FSTR) AS FSTR,TRIM(A.VCHNUM) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,TRIM(A.ICODE) AS ITEM_CODE,TRIM(I.INAME) AS ITEM_NAME,TRIM(A.ACODE) AS CUSTOMER_CODE,TRIM(B.ANAME) AS CUSTOMER_NAME from (select branchcd||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,vchdate,icode,ACODE,1 AS QTY from wb_cacost where branchcd='" + frm_mbr + "' and type ='CA01' and nvl(trim(app_by),'-')!='C' union all select distinct branchcd||'CA01'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO AS VCHNUM,INVDATE AS VCHDATE,icode,ACODE,-1 AS QTY from somasq WHERE  branchcd='" + frm_mbr + "' and type='FQ') a,item i,FAMST B where trim(a.icode)=trim(i.icode) AND trim(a.Acode)=trim(B.Acode) group by TRIM(A.FSTR) ,trim(A.VCHNUM),TO_CHAR(A.VCHDATE,'DD/MM/YYYY'),trim(A.ICODE),trim(A.ACODE),trim(I.INAME),trim(B.ANAME) HAVING SUM(A.QTY)>0 ORDER BY FSTR";
                SQuery = "SELECT TRIM(A.FSTR) AS FSTR,TRIM(A.RFQ_NO) AS RFQ_NO,TRIM(A.TYPE) AS TYPE,TRIM(A.RFQ_DATE) AS RFQ_DATE,TRIM(A.ACODE) AS CUST_CODE,TRIM(F.ANAME) AS CUSTOMER,TRIM(A.ICODE) AS ITEM_CODE,TRIM(I.INAME) AS ITEM_NAME FROM(SELECT DISTINCT TRIM(PBASIS) AS FSTR,(CASE WHEN SUBSTR(TRIM(PBASIS),3,2)='EC' THEN 'ENG. CHANGE NOTIFICATION' ELSE 'ENQUIRY REGISTER' END) AS TYPE,SUBSTR(TRIM(PBASIS),5,6) AS RFQ_NO,SUBSTR(TRIM(PBASIS),11,10) AS RFQ_DATE,ACODE,ICODE,1 AS QTY FROM WB_CACOST  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='CA01' UNION ALL SELECT DISTINCT TRIM(DESP_TO) AS FSTR,(CASE WHEN SUBSTR(TRIM(DESP_TO),3,2)='EC' THEN 'ENG. CHANGE NOTIFICATION' ELSE 'ENQUIRY REGISTER' END) AS TYPE,SUBSTR(TRIM(DESP_TO),5,6) AS RFQ_NO,SUBSTR(TRIM(DESP_TO),11,10) AS RFQ_DATE,ACODE,ICODE,-1 AS QTY FROM SOMASQ WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='FQ')A,FAMST F,ITEM I WHERE TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) GROUP BY TRIM(A.FSTR),TRIM(A.RFQ_NO),TRIM(A.RFQ_DATE),TRIM(A.ACODE),TRIM(F.ANAME),TRIM(A.ICODE),TRIM(I.INAME),TRIM(A.TYPE) HAVING SUM(QTY)>0 ORDER BY FSTR";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                    else col1 = "'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                //SQuery = "Select icode as fstr, Ciname,Cpartno,icode,cdrgno from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtgrade.Text.Trim() + "' order by Srno";
                SQuery = "";

                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                SQuery = "";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                // SQuery = "select distinct A.ORDNO from " + frm_tabname + " a,ITEM I WHERE trim(a.Icode)=trim(I.Icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.ORDDT  " + DateRange + " ORDER BY ORDNO";
                SQuery = "SELECT distinct TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS ENTRY_NO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ENTRY_DATE,trim(a.acode) as cust_code,f.aname as customer,trim(a.icode) as item_code,trim(b.iname) as component_name,b.cpartno as component_part,to_char(a.orddt,'yyyymmdd') as vdd FROM " + frm_tabname + " A, item B,famst f WHERE TRIM(A.iCODE)=TRIM(B.iCODE) and trim(a.acode)=trim(f.acode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' ORDER BY vdd desc,entry_no DESC";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno as entry_no,to_char(a.orddt,'dd/mm/yyyy') as entry_dt,trim(a.acode) as cust_code,f.aname as customer,trim(a.icode) as item_code,trim(b.iname) as component_name,b.cpartno as component_part,to_char(a.orddt,'yyyymmdd') as vdd FROM " + frm_tabname + " A, item B,famst f WHERE TRIM(A.iCODE)=TRIM(B.iCODE) and trim(a.acode)=trim(f.acode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' ORDER BY vdd desc,entry_no DESC";
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

        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_vty = "FQ";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            //txtvchnum.Text = frm_vnum;
            //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        //create_tab2();
        //sg2_add_blankrows();
        //sg2.DataSource = sg2_dt;
        //sg2.DataBind();
        //ViewState["sg2"] = null;
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        cal();
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }
        if (txtRfqno.Text == "" || txtRfqno.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select RFQ No."); txtvchdate.Focus();
            return;
        }
        if (txtacode.Text == "" || txtacode.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select Customer"); txtvchdate.Focus();
            return;
        }
        if (sg1.Rows.Count < 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One Attachment");
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
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
        sg1.DataSource = null;
        sg1.DataBind();
        ViewState["sg1"] = null;
        sg2.DataSource = null;
        sg2.DataBind();
        ViewState["sg2"] = null;
        setColHeadings();
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // for deleting test flag field from last table i.e type CA01
                mq4 = "select trim(a.desp_to) as desp_to,trim(a.pordno) as pordno from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq5 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "pordno");
                mq6 = "update wb_cacost set test='-' where branchcd||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + mq5 + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, mq6);

                // for deleting test flag field from first table i.e type EC or ER
                mq7 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "desp_to");
                mq8 = "update wb_sorfq set test='C' where branchcd||trim(type)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + mq7 + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, mq8);

                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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

                case "COPY_OLD":
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
                    lbl1a.Text = col1;
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
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.* ,trim(c.iname) as iname,c.cpartno as partno,b.aname,d.iname as childname,d.cpartno as childpartno from item c,famst b," + frm_tabname + " a left join item d on trim(a.busi_potent)=trim(d.icode) where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode)  and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtRfqno.Text = dt.Rows[0]["invno"].ToString().Trim();
                        txtRfqdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtpartno.Text = dt.Rows[0]["partno"].ToString().Trim();
                        txtmatl.Text = dt.Rows[0]["ciname"].ToString().Trim();
                        txtmchcost.Text = dt.Rows[0]["QTYSUPP"].ToString().Trim();
                        txtfoundcost.Text = dt.Rows[0]["QTYORD"].ToString().Trim();

                        txtbop.Text = dt.Rows[0]["CD"].ToString().Trim();
                        txttoolcost.Text = dt.Rows[0]["qtybal"].ToString().Trim();
                        txtcastprice.Text = dt.Rows[0]["irate"].ToString().Trim();
                        txtheattmt.Text = dt.Rows[0]["TD"].ToString().Trim();
                        txtmchprice.Text = dt.Rows[0]["DELIVERY"].ToString().Trim();
                        txtpack.Text = dt.Rows[0]["INSPCHG"].ToString().Trim();
                        txtcomp.Text = dt.Rows[0]["OTHAMT3"].ToString().Trim();
                        txtpaintcost.Text = dt.Rows[0]["OTHAMT2"].ToString().Trim();
                        txtasembcost.Text = dt.Rows[0]["OTHAMT1"].ToString().Trim();

                        txtforwrd.Text = dt.Rows[0]["RLPRC"].ToString().Trim();
                        txtpymtterm.Text = dt.Rows[0]["class"].ToString().Trim();
                        txtrmbase.Text = dt.Rows[0]["ORD_ALERT"].ToString().Trim();
                        txtcastwght.Text = dt.Rows[0]["PVT_MARK"].ToString().Trim();
                        txtquoteval.Text = dt.Rows[0]["CO_ORIG"].ToString().Trim();
                        txtdelterm.Text = dt.Rows[0]["HS_CODE"].ToString().Trim();
                        txtrmk1.Text = dt.Rows[0]["DESC0"].ToString().Trim();
                        txtrmk2.Text = dt.Rows[0]["DESC1"].ToString().Trim();
                        txtrmk3.Text = dt.Rows[0]["DESC2"].ToString().Trim();
                        txtrmk4.Text = dt.Rows[0]["DESC3"].ToString().Trim();
                        txtrmk5.Text = dt.Rows[0]["DESC4"].ToString().Trim();
                        txtrmk6.Text = dt.Rows[0]["DESC5"].ToString().Trim();
                        txtrmk7.Text = dt.Rows[0]["DESC6"].ToString().Trim();
                        txtrmk8.Text = dt.Rows[0]["DESC7"].ToString().Trim();
                        txtrmk9.Text = dt.Rows[0]["DESC8"].ToString().Trim();
                        txtrmk10.Text = dt.Rows[0]["DESC9"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        txtFstr2.Text = dt.Rows[0]["DESP_TO"].ToString().Trim();
                        txtFinalCompCost.Text = dt.Rows[0]["inst3"].ToString().Trim();
                        mq5 = "select a.kindattn,a.st31no,a.atch2,a.atch3,'-' as desc_,(case when type='ER' then 'ENQUIRY REGISTER' else 'ENG. CHANGE NOTIFICATION' end) as type from wb_sorfq a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + dt.Rows[0]["DESP_TO"].ToString().Trim() + "' union all SELECT a.kindattn,a.st31no,a.atch2,a.atch3,a.desc_,'RESPOND FOUNDRY ('||AMD_NO||')' AS TYPE from wb_sorfq a where branchcd='" + frm_mbr + "' and type='RF' and trim(pordno)='" + dt.Rows[0]["DESP_TO"].ToString().Trim() + "' union all SELECT a.kindattn,a.st31no,a.atch2,a.atch3,a.desc_,'M/C SHOP FOUNDRY ('||AMD_NO||')' AS TYPE from wb_sorfq a where branchcd='" + frm_mbr + "' and type='M2' and trim(pbasis)='" + dt.Rows[0]["DESP_TO"].ToString().Trim() + "'";
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, mq5);
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt4.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = dt4.Rows[i]["kindattn"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt4.Rows[i]["st31no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt4.Rows[i]["atch2"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt4.Rows[i]["atch3"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt4.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt4.Rows[i]["type"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = sg1_dt;
                        for (int i = 0; i < sg1.Rows.Count; i++)
                        {
                            string hf = ((HiddenField)sg1.Rows[i].FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                            }
                            sg1.Rows[i].Cells[0].Enabled = false;
                            ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Enabled = false;
                            ((FileUpload)sg1.Rows[i].FindControl("FileUpload1")).Enabled = false;
                            ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Enabled = false;
                        }
                        //mq0 = "select trim(a.amd_no) as icode,trim(i.iname) as iname,trim(i.cpartno) as cpartno,SUM(PDISCAMT2) AS mchprice,SUM(QTYORD) AS mchcost,SUM(QTYSUPP) AS FOUNDCOST,SUM(WK1) AS CASTWGHT,sum(vendor) as vendor from(select amd_no,pdiscamt2,qtyord,0 as wk1,0 as qtysupp,0 as vendor from wb_sorfq where branchcd='" + frm_mbr + "' and type ='MC' and trim(pbasis)='" + dt.Rows[0]["DESP_TO"].ToString().Trim() + "' and srno='1' union all select amd_no,0 as pdiscamt2,0 as qtyord,wk1,qtysupp,0 as vendor from wb_sorfq where branchcd='" + frm_mbr + "' and type ='RF' and trim(pordno)='" + dt.Rows[0]["DESP_TO"].ToString().Trim() + "' union all select distinct childcode,0 as pdiscamt2,0 as qtyord,0 as wk1,0 as qtysupp,vendor from wb_cacost where branchcd='" + frm_mbr + "' and type ='CA01' and trim(pbasis)='" + dt.Rows[0]["DESP_TO"].ToString().Trim() + "')a left join item i on trim(a.amd_no)=trim(i.icode) group by trim(a.amd_no),trim(i.iname),trim(i.cpartno)";
                        //dt2 = new DataTable();
                        //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        create_tab2();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_t1"] = dt.Rows[i]["busi_potent"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt.Rows[i]["childpartno"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt.Rows[i]["childname"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt.Rows[i]["inst1"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt.Rows[i]["ciname"].ToString().Trim();
                            sg2_dr["sg2_t6"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            sg2_dr["sg2_t8"] = dt.Rows[i]["qtybal"].ToString().Trim();
                            sg2_dr["sg2_t9"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg2_dr["sg2_t10"] = dt.Rows[i]["delivery"].ToString().Trim();
                            sg2_dr["sg2_t11"] = dt.Rows[i]["basic"].ToString().Trim();
                            sg2_dr["sg2_t12"] = dt.Rows[i]["excise"].ToString().Trim();
                            sg2_dr["sg2_t13"] = dt.Rows[i]["inspchg"].ToString().Trim();
                            sg2_dr["sg2_t14"] = dt.Rows[i]["othamt3"].ToString().Trim();
                            sg2_dr["sg2_t15"] = dt.Rows[i]["inst2"].ToString().Trim();
                            sg2_dr["sg2_t16"] = dt.Rows[i]["ipack"].ToString().Trim();
                            sg2_dr["sg2_t17"] = dt.Rows[i]["packing"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        ViewState["sg2"] = sg2_dt;

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        btnlbl4.Enabled = false;
                        txtvchdate.Enabled = false;
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_smktg_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    #region
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    //SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,trim(a.srno) as morder1,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.vchdate,'dd/mm/yyyy') as pvchdate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)='" + col1 + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //txtlbl4.Text = col1;
                        //txtlbl4a.Text = col2;
                    }
                    dt.Dispose();
                    // SQuery = "Select * from inspmst a where a.branchcd='" + frm_mbr + "' and a.icode='" + txtlbl7.Text + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE AS COL1,NAME AS COL2, DEPTT_TEXT AS COL3,DESG_TEXT AS COL4,TO_CHAR(DTJOIN,'dd/MM/yyyy') AS COL6,ENT_DT,ENT_BY from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
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
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["col6"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        //edmode.Value = "Y";
                    }
                    #endregion
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

                case "RFQ":
                    if (col1.Length <= 0) return;
                    SQuery = "select trim(fstr) as fstr,TRIM(A.VCHNUM) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,TRIM(A.ICODE) AS ITEM_CODE,TRIM(I.INAME) AS ITEM_NAME,TRIM(A.ACODE) AS CUSTOMER_CODE,TRIM(B.ANAME) AS CUSTOMER_NAME,i.cpartno ,b.payment from (select branchcd||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,vchdate,icode,ACODE,1 AS QTY from wb_cacost where branchcd='" + frm_mbr + "' and type ='CA01' union all select distinct branchcd||'CA01'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO AS VCHNUM,INVDATE AS VCHDATE,icode,ACODE,-1 AS QTY from somasq WHERE  branchcd='" + frm_mbr + "' and type='FQ') a,item i,FAMST B where trim(a.icode)=trim(i.icode) AND trim(a.Acode)=trim(B.Acode) AND FSTR='" + col1 + "' group by TRIM(A.FSTR) ,A.VCHNUM,A.VCHDATE,A.ICODE,A.ACODE,I.INAME,B.ANAME,i.cpartno,b.payment HAVING SUM(A.QTY)>0 ORDER BY FSTR";
                    SQuery = "select a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,trim(a.icode) as item_code,trim(i.iname) as item_name,trim(a.acode) as customer_code,trim(b.aname) as customer_name,i.cpartno,b.payment,a.pordno,a.pbasis,a.vendor from wb_cacost a,item i,famst b where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(b.acode) and trim(a.pbasis)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        // BOTH ER,EC HAS MORE THAN ONE ATTACHMENT THAT'S WHY SRNO='1' IS APPLIED IN QUERY
                        mq1 = "select distinct trim(pr_no) as mflag,sum(pexc) as totchild from wb_Sorfq where trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' and srno='1' group by trim(pr_no)";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        // MC SHOP DATA CAN HAVE MULTIPLE ROWS BASED ON THE ITEM STAGES THAT'S WHY SRNO='1' IS APPLIED IN QUERY
                        mq6 = "select trim(pbasis) as er_ec_no,sum(pdisc) as totchild from wb_Sorfq where branchcd='" + frm_mbr + "' and type='MC' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' and srno='1' group by trim(pbasis)";
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, mq6);
                        mq7 = "select rtrim(xmlagg(xmlelement(e,replace(amd_no,'-',null)||',')).extract('//text()').extract('//text()'),',') as child from wb_Sorfq where branchcd='" + frm_mbr + "' and type='MC' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' and srno='1'";
                        if (dt2.Rows.Count > 0)
                        {
                            double ec_er_totchild = fgen.make_double(dt2.Rows[0]["totchild"].ToString().Trim());
                            double mc_totchild = 0;
                            if (dt1.Rows.Count > 0)
                            {
                                mc_totchild = fgen.make_double(dt1.Rows[0]["totchild"].ToString().Trim());
                            }
                            double leftchild = ec_er_totchild - mc_totchild;
                            mq9 = fgen.seek_iname(frm_qstr, frm_cocd, mq7, "child");
                            if (dt2.Rows[0]["mflag"].ToString().Trim() == "-")
                            {
                                if (leftchild > 0)
                                {
                                    if (mq9.Length > 1)
                                    {
                                        fgen.msg("-", "-", "Machine Shop Foundry Is Done Only For (" + mq9 + ")'13' Another SF Codes Are Pending for Machine Shop Foundry");
                                    }
                                    else
                                    {
                                        fgen.msg("-", "-", "Machine Shop Foundry Has Not Done For This Enquiry");
                                    }
                                    return;
                                }
                            }
                            else if (leftchild > 0)
                            {
                                fgen.msg("-", "-", "Machine Shop Foundry Is Done Only For (" + mq9 + ")'13' Another SF Codes Are Pending for Machine Shop Foundry");
                                return;
                            }
                        }
                        else
                        {
                            fgen.msg("-", "-", "Machine Shop Foundry Has Not Done For This Enquiry");
                            return;
                        }
                        txtRfqno.Text = dt.Rows[0]["ENTRY_NO"].ToString().Trim();
                        txtRfqdate.Text = dt.Rows[0]["ENTRY_DATE"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["CUSTOMER_CODE"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["CUSTOMER_NAME"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["ITEM_CODE"].ToString().Trim();
                        txtiname.Text = dt.Rows[0]["ITEM_NAME"].ToString().Trim();
                        txtpartno.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtpymtterm.Text = dt.Rows[0]["payment"].ToString().Trim();
                        txtcastprice.Text = dt.Rows[0]["vendor"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["fstr"].ToString().Trim();
                        txtFstr2.Text = dt.Rows[0]["pbasis"].ToString().Trim();
                        //mq0 = "select vendor,trim(invno)||to_char(invdate,'dd/mm/yyyy') as mcentry,pbasis from wb_cacost where branchcd||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                        //txtcastprice.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "vendor");
                        //orginal mq1 = "select DISTINCT PDISCAMT2 ,QTYORD,trim(invno)||TO_CHAR(invdate,'dd/mm/yyyy') as rfqentry from wb_Sorfq where branchcd='" + frm_mbr + "' and type='MC' and trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + fgen.seek_iname(frm_qstr, frm_cocd, mq0, "mcentry") + "'";
                        mq1 = "select trim(pbasis) AS ER_EC_NO,SUM(PDISCAMT2) AS mchprice,SUM(QTYORD) AS mchcost from wb_Sorfq where branchcd='" + frm_mbr + "' and type='MC' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' AND SRNO='1' GROUP BY TRIM(PBASIS)";
                        //orginal mq2 = "SELECT QTYSUPP,WK1 FROM WB_SORFQ WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='RF' and trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + fgen.seek_iname(frm_qstr, frm_cocd, mq1, "rfqentry") + "' ";
                        mq2 = "SELECT SUM(QTYSUPP) AS FOUNDCOST,SUM(WK1) AS CASTWGHT,TRIM(PORDNO) AS ER_EC_NO FROM WB_SORFQ WHERE branchcd='" + frm_mbr + "' and type='RF' and trim(pordno)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' group by trim(pordno)";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        if (dt2.Rows.Count > 0)
                        {
                            //txtmchcost.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "QTYORD");
                            //txtmchprice.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "PDISCAMT2");
                            txtmchcost.Text = dt2.Rows[0]["mchcost"].ToString().Trim();
                            txtmchprice.Text = dt2.Rows[0]["mchprice"].ToString().Trim();
                        }
                        if (dt3.Rows.Count > 0)
                        {
                            //txtfoundcost.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "QTYSUPP");
                            //txtcastwght.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "WK1");
                            txtfoundcost.Text = dt3.Rows[0]["FOUNDCOST"].ToString().Trim();
                            txtcastwght.Text = dt3.Rows[0]["CASTWGHT"].ToString().Trim();
                        }
                        txttoolcost.Text = Math.Round(fgen.make_double(txtmchcost.Text) + fgen.make_double(txtfoundcost.Text), 0).ToString();
                        //txtFstr2.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "pbasis");
                        //mq3 = "select round(sum(pdiscamt2*txb_frt),2) as bop_tot from wb_sorfq where branchcd='" + frm_mbr + "' and type='M1' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "'";
                        //txtbop.Text = fgen.seek_iname(frm_qstr, frm_cocd, mq3, "bop_tot");
                        mq5 = "select a.kindattn,a.st31no,a.atch2,a.atch3,'-' as desc_,(case when type='ER' then 'ENQUIRY REGISTER' else 'ENG. CHANGE NOTIFICATION' end) as type from wb_sorfq a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' union all SELECT a.kindattn,a.st31no,a.atch2,a.atch3,a.desc_,'RESPOND FOUNDRY ('||AMD_NO||')' AS TYPE from wb_sorfq a where branchcd='" + frm_mbr + "' and type='RF' and trim(pordno)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' union all SELECT a.kindattn,a.st31no,a.atch2,a.atch3,a.desc_,'M/C SHOP FOUNDRY ('||AMD_NO||')' AS TYPE from wb_sorfq a where branchcd='" + frm_mbr + "' and type='M2' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "'";
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, mq5);
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt4.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = dt4.Rows[i]["kindattn"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt4.Rows[i]["st31no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt4.Rows[i]["atch2"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt4.Rows[i]["atch3"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt4.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt4.Rows[i]["type"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = sg1_dt;
                        for (int i = 0; i < sg1.Rows.Count; i++)
                        {
                            string hf = ((HiddenField)sg1.Rows[i].FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                            }
                            sg1.Rows[i].Cells[0].Enabled = false;
                            ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Enabled = false;
                            ((FileUpload)sg1.Rows[i].FindControl("FileUpload1")).Enabled = false;
                            ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Enabled = false;
                        }
                        mq0 = "select trim(a.amd_no) as icode,trim(i.iname) as iname,trim(i.cpartno) as cpartno,sum(a.PDISCAMT2) AS mchprice,sum(a.QTYORD) AS mchcost,sum(a.QTYSUPP) AS FOUNDCOST,sum(a.WK1) AS CASTWGHT,sum(a.vendor) as vendor,max(matgrade) as matgrade from(select amd_no,pdiscamt2,qtyord,0 as wk1,0 as qtysupp,0 as vendor,'-' as matgrade from wb_sorfq where branchcd='" + frm_mbr + "' and type ='MC' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' and srno='1' union all select amd_no,0 as pdiscamt2,0 as qtyord,wk1,qtysupp,0 as vendor,trim(tr_insur) as matgrade from wb_sorfq where branchcd='" + frm_mbr + "' and type ='RF' and trim(pordno)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "' union all select distinct childcode,0 as pdiscamt2,0 as qtyord,0 as wk1,0 as qtysupp,vendor,'-' as matgrade from wb_cacost where branchcd='" + frm_mbr + "' and type ='CA01' and trim(pbasis)='" + dt.Rows[0]["pbasis"].ToString().Trim() + "')a left join item i on trim(a.amd_no)=trim(i.icode) group by trim(a.amd_no),trim(i.iname),trim(i.cpartno) order by icode";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        create_tab2();
                        mq10 = "select trim(ibcode) as ibcode,ibqty from itemosp where icode='" + txticode.Text.Trim() + "'";
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, mq10);

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_t1"] = dt2.Rows[i]["icode"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt2.Rows[i]["cpartno"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt2.Rows[i]["iname"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt2.Rows[i]["CASTWGHT"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt2.Rows[i]["matgrade"].ToString().Trim();
                            sg2_dr["sg2_t6"] = dt2.Rows[i]["FOUNDCOST"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt2.Rows[i]["mchcost"].ToString().Trim();
                            sg2_dr["sg2_t8"] = fgen.make_double(dt2.Rows[i]["FOUNDCOST"].ToString().Trim()) + fgen.make_double(dt2.Rows[i]["mchcost"].ToString().Trim());
                            sg2_dr["sg2_t9"] = dt2.Rows[i]["vendor"].ToString().Trim();
                            sg2_dr["sg2_t10"] = dt2.Rows[i]["mchprice"].ToString().Trim();
                            sg2_dr["sg2_t11"] = "";
                            sg2_dr["sg2_t12"] = "";
                            sg2_dr["sg2_t13"] = "";
                            sg2_dr["sg2_t15"] = "";
                            sg2_dr["sg2_t16"] = fgen.seek_iname_dt(dt4, "ibcode='" + dt2.Rows[i]["icode"].ToString().Trim() + "'", "ibqty");
                            sg2_dr["sg2_t17"] = fgen.make_double(dt2.Rows[i]["CASTWGHT"].ToString().Trim()) * fgen.make_double(sg2_dr["sg2_t16"].ToString().Trim());
                            sg2_dr["sg2_t14"] = (fgen.make_double(dt2.Rows[i]["vendor"].ToString().Trim()) + fgen.make_double(dt2.Rows[i]["mchprice"].ToString().Trim())) * fgen.make_double(sg2_dr["sg2_t16"].ToString().Trim());
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        ViewState["sg2"] = sg2_dt;
                    }
                    txtmatl.Focus();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight,b.cdrgno from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and a.invno='" + txtlbl4.Text.Trim() + "' and upper(a.finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||trim(a.desc_) ='" + col1.Trim() + "' order by Tag_no";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = dt.Rows[d]["cdrgno"].ToString().Trim();

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = (fgen.make_double(dt.Rows[d]["weight"].ToString().Trim())).ToString();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["Tag_no"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[d]["INAME"].ToString().Trim();
                    }
                    setColHeadings();
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text);
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.ToString();
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
                        SQuery = "select a.type,a.vchnum,a.icode,a.srno,a.stagec,a.mtime,a.opcode,b.name from itwstage a ,type b  where trim(a.stagec)=trim(b.type1) and b.id='[' and  trim(a.icode) '" + col1 + "'";
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
                            sg1_dr["sg1_h9"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h10"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[d]["weight"].ToString().Trim());
                            sg1_dr["sg1_t2"] = dt.Rows[d]["Tag_no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    #endregion
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
                        for (i = 0; i < dt.Rows.Count - 1; i++)
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
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
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
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = sg1_dt;
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
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "sELECT distinct trim(a.ordno) as Entry_No,to_char(a.orddt,'dd/mm/yyyy') as entry_Dt,a.acode as customer_code,trim(b.aname) as customer_name,a.icode as item_code,i.iname as component_name,i.cpartno as Component_part_no,a.invno as rfq_mc_no,to_char(a.invdate,'dd/mm/yyyy') as rfq_mc_date,a.class as payment_term,a.ORD_ALERT as rm_base,a.CO_ORIG as quote_validity,a.HS_CODE as delivery_term,a.DESC0 as remarks1,a.DESC1 as remarks2,a.DESC2 as remarks3,a.DESC3 as remarks4,a.DESC4 as remarks5,a.DESC5 as remarks6,a.DESC6 as remarks7,a.DESC7 as remarks8,a.DESC8 as remarks9,a.DESC9 as remarks10 ,a.Ent_by,to_char(a.Ent_Dt,'dd/mm/yyyy') as ent_Dt,to_char(a.orddt,'yyyymmdd') as vdd FROM " + frm_tabname + " a,item i,famst b WHERE trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(b.acode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.orddt  " + PrdRange + " ORDER BY vdd DESC,ENTRY_NO DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period " + fromdt + " to " + todt, frm_qstr);
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
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            }
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
                if (col1 == "Y" && Checked_ok == "Y")
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
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";

                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }
                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        string mycmd4 = ""; // SAVING FLAG IN ER ENTRY
                        mycmd4 = "update WB_SORFQ set TEST='Q' where branchcd||type||trim(ordno)||to_char(orddt,'dd/MM/yyyy')='" + txtFstr2.Text.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, mycmd4);

                        string mycmd3 = ""; // SAVING FLAG IN COSTING ENTRY
                        mycmd3 = "update WB_CACOST set TEST='Q' where branchcd||type||trim(VCHNUM)||to_char(VCHDATE,'dd/MM/yyyy')='" + txtFstr.Text.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, mycmd3);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); sg1.DataSource = null; sg1.DataBind(); ViewState["sg1"] = null; sg2.DataSource = null; sg2.DataBind(); ViewState["sg2"] = null;
                    }
                    catch (Exception ex)
                    {
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N"; btnsave.Disabled = false;
                    }
            #endregion
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "RFQ";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select RFQ Entry", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl23_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
    }
    //------------------------------------------------------------------------------------   
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        double db = 0;
        //for (i = 0; i < sg1.Rows.Count - 1; i++)
        //{
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["ordno"] = frm_vnum.Trim().ToUpper();
        oporow["orddt"] = txtvchdate.Text.Trim().ToUpper();
        oporow["SRNO"] = i + 1;
        oporow["invno"] = txtRfqno.Text.Trim().ToUpper();
        oporow["invdate"] = Convert.ToDateTime(txtRfqdate.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
        oporow["acode"] = txtacode.Text.Trim().ToUpper();
        oporow["icode"] = txticode.Text.Trim().ToUpper();
        oporow["ciname"] = txtmatl.Text.Trim().ToUpper();
        oporow["class"] = txtpymtterm.Text.Trim().ToUpper();//vc15
        oporow["QTYORD"] = Math.Round(fgen.make_double(txtfoundcost.Text.Trim().ToUpper()), 2);//n17
        oporow["QTYSUPP"] = Math.Round(fgen.make_double(txtmchcost.Text.Trim().ToUpper()), 2);//n12
        oporow["qtybal"] = Math.Round(fgen.make_double(txttoolcost.Text.Trim().ToUpper()), 2);//n12
        oporow["irate"] = Math.Round(fgen.make_double(txtcastprice.Text.Trim().ToUpper()), 2);//n17
        oporow["DELIVERY"] = Math.Round(fgen.make_double(txtmchprice.Text.Trim().ToUpper()), 2);//n12
        oporow["TD"] = Math.Round(fgen.make_double(txtheattmt.Text.Trim().ToUpper()), 2);//n7
        oporow["CD"] = Math.Round(fgen.make_double(txtbop.Text.Trim().ToUpper()), 2);//n7
        oporow["INSPCHG"] = Math.Round(fgen.make_double(txtpack.Text.Trim().ToUpper()), 2);//n12     
        oporow["OTHAMT1"] = Math.Round(fgen.make_double(txtasembcost.Text.Trim().ToUpper()), 2);//n10
        oporow["OTHAMT2"] = Math.Round(fgen.make_double(txtpaintcost.Text.Trim().ToUpper()), 2);//n10
        oporow["RLPRC"] = Math.Round(fgen.make_double(txtforwrd.Text.Trim().ToUpper()), 2);//n10                
        oporow["OTHAMT3"] = Math.Round(fgen.make_double(txtcomp.Text.Trim().ToUpper()), 2);//n10        
        oporow["ORD_ALERT"] = txtrmbase.Text.Trim().ToUpper();//vc50
        oporow["PVT_MARK"] = Math.Round(fgen.make_double(txtcastwght.Text.Trim().ToUpper()), 2);//vc150
        oporow["CO_ORIG"] = Math.Round(fgen.make_double(txtquoteval.Text.Trim().ToUpper()), 2);//vc20
        oporow["HS_CODE"] = txtdelterm.Text.Trim().ToUpper();//vc20
        oporow["DESC0"] = txtrmk1.Text.Trim().ToUpper();//100
        oporow["DESC1"] = txtrmk2.Text.Trim().ToUpper();//100
        oporow["DESC2"] = txtrmk3.Text.Trim().ToUpper();//100
        oporow["DESC3"] = txtrmk4.Text.Trim().ToUpper();//100
        oporow["DESC4"] = txtrmk5.Text.Trim().ToUpper();//100
        oporow["DESC5"] = txtrmk6.Text.Trim().ToUpper();//100
        oporow["DESC6"] = txtrmk7.Text.Trim().ToUpper();//100
        oporow["DESC7"] = txtrmk8.Text.Trim().ToUpper();//100
        oporow["DESC8"] = txtrmk9.Text.Trim().ToUpper();//100
        oporow["DESC9"] = txtrmk10.Text.Trim().ToUpper();//100  
        oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();
        oporow["DESP_TO"] = txtFstr2.Text.Trim().ToUpper();
        oporow["remark"] = txtrmk.Text.Trim().ToUpper(); //800
        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"].ToString();
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["edt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
        // }        
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg2.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["ordno"] = frm_vnum.Trim().ToUpper();
            oporow["orddt"] = txtvchdate.Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            oporow["invno"] = txtRfqno.Text.Trim().ToUpper();
            oporow["invdate"] = Convert.ToDateTime(txtRfqdate.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["acode"] = txtacode.Text.Trim().ToUpper();
            oporow["icode"] = txticode.Text.Trim().ToUpper();
            oporow["ciname"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper();
            oporow["class"] = txtpymtterm.Text.Trim().ToUpper();//vc15
            oporow["QTYORD"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper()), 2);//n17
            oporow["QTYSUPP"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper()), 2);//n12
            oporow["qtybal"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim().ToUpper()), 2);//n12
            oporow["irate"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim().ToUpper()), 2);//n17
            oporow["DELIVERY"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim().ToUpper()), 2);//n12
            oporow["TD"] = Math.Round(fgen.make_double(txtheattmt.Text.Trim().ToUpper()), 2);//n7
            oporow["CD"] = Math.Round(fgen.make_double(txtbop.Text.Trim().ToUpper()), 2);//n7
            oporow["INSPCHG"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t13")).Text.Trim().ToUpper()), 2);//n12     
            oporow["OTHAMT1"] = Math.Round(fgen.make_double(txtasembcost.Text.Trim().ToUpper()), 2);//n10
            oporow["OTHAMT2"] = Math.Round(fgen.make_double(txtpaintcost.Text.Trim().ToUpper()), 2);//n10
            oporow["RLPRC"] = Math.Round(fgen.make_double(txtforwrd.Text.Trim().ToUpper()), 2);//n10                
            oporow["OTHAMT3"] = Math.Round(fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t14")).Text.Trim().ToUpper()), 2);//n10        
            oporow["ORD_ALERT"] = txtrmbase.Text.Trim().ToUpper();//vc50
            oporow["PVT_MARK"] = Math.Round(fgen.make_double(txtcastwght.Text.Trim().ToUpper()), 2);//vc150
            oporow["CO_ORIG"] = Math.Round(fgen.make_double(txtquoteval.Text.Trim().ToUpper()), 2);//vc20
            oporow["HS_CODE"] = txtdelterm.Text.Trim().ToUpper();//vc20
            oporow["DESC0"] = txtrmk1.Text.Trim().ToUpper();//100
            oporow["DESC1"] = txtrmk2.Text.Trim().ToUpper();//100
            oporow["DESC2"] = txtrmk3.Text.Trim().ToUpper();//100
            oporow["DESC3"] = txtrmk4.Text.Trim().ToUpper();//100
            oporow["DESC4"] = txtrmk5.Text.Trim().ToUpper();//100
            oporow["DESC5"] = txtrmk6.Text.Trim().ToUpper();//100
            oporow["DESC6"] = txtrmk7.Text.Trim().ToUpper();//100
            oporow["DESC7"] = txtrmk8.Text.Trim().ToUpper();//100
            oporow["DESC8"] = txtrmk9.Text.Trim().ToUpper();//100
            oporow["DESC9"] = txtrmk10.Text.Trim().ToUpper();//100  
            oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();
            oporow["DESP_TO"] = txtFstr2.Text.Trim().ToUpper();
            oporow["remark"] = txtrmk.Text.Trim().ToUpper(); //800
            oporow["busi_potent"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper();
            oporow["basic"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t11")).Text.Trim().ToUpper());
            oporow["excise"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t12")).Text.Trim().ToUpper());
            oporow["inst1"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
            oporow["inst2"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t15")).Text.Trim().ToUpper());
            oporow["inst3"] = fgen.make_double(txtFinalCompCost.Text);
            oporow["Ipack"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t16")).Text.Trim().ToUpper());
            oporow["packing"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t17")).Text.Trim().ToUpper());
            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "FQ");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void txt_TextChanged(object sender, EventArgs e)
    {
    }
    //------------------------------------------------------------------------------------
    protected void btnProdReport_Click(object sender, EventArgs e)
    {
        hffield.Value = "ProdRep";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------  
    protected void btnicode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Item";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void cal()
    {
        //txttoolcost.Text = Math.Round(fgen.make_double(txtmchcost.Text) + fgen.make_double(txtfoundcost.Text)).ToString();
        //txtcomp.Text = Math.Round(fgen.make_double(txtcastprice.Text.Trim()) + fgen.make_double(txtmchprice.Text.Trim()) + fgen.make_double(txtheattmt.Text.Trim()) + fgen.make_double(txtbop.Text.Trim()) + fgen.make_double(txtpack.Text.Trim()) + fgen.make_double(txtasembcost.Text.Trim()) + fgen.make_double(txtpaintcost.Text.Trim()) + fgen.make_double(txtforwrd.Text.Trim()), 2).ToString();
        ////    txtcomp.Text = "9230.06";
        double casting = 0, machining = 0, bop = 0, freight = 0, packaging = 0, other_chg = 0, componentcost = 0;
        double heat = 0, assembly = 0, painting = 0, finalcost = 0; double total = 0, bomqty = 0;
        for (int i = 0; i < sg2.Rows.Count; i++)
        {
            componentcost = 0;
            casting = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim());
            machining = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim());
            bop = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t11")).Text.Trim());
            freight = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t12")).Text.Trim());
            packaging = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t13")).Text.Trim());
            other_chg = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t15")).Text.Trim());
            bomqty = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t16")).Text.Trim());
            componentcost = (casting + machining + bop + freight + packaging + other_chg) * bomqty;
            total += componentcost;
            ((TextBox)sg2.Rows[i].FindControl("sg2_t14")).Text = (Math.Round(componentcost, 2)).ToString();
        }
        heat = fgen.make_double(txtheattmt.Text);
        assembly = fgen.make_double(txtasembcost.Text);
        painting = fgen.make_double(txtpaintcost.Text);
        finalcost = heat + assembly + painting + total;
        txtFinalCompCost.Text = (Math.Round(finalcost, 2)).ToString();
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg1.Columns[0].HeaderStyle.Width = 50;
            sg1.Columns[1].HeaderStyle.Width = 70;
            sg1.Columns[2].HeaderStyle.Width = 50;
            sg1.Columns[3].HeaderStyle.Width = 180;
            sg1.Columns[4].HeaderStyle.Width = 180;
            sg1.Columns[5].HeaderStyle.Width = 300;
            sg1.Columns[6].HeaderStyle.Width = 180;
            sg1.Columns[7].HeaderStyle.Width = 180;
            sg1.Columns[8].HeaderStyle.Width = 170;
            sg1.Columns[8].HeaderStyle.Width = 170;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = 0;
        if (var == "SG1_UPLD")
        {
            rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;
        }
        else
        {
            rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        }
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        string filePath = "";
        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                sg1.Rows[index].Cells[5].Text = "-";
                sg1.Rows[index].Cells[6].Text = "-";
                break;

            case "SG1_DWN":
                filePath = sg1.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }
                break;

            case "SG1_VIEW":
                if (sg1.Rows[index].Cells[6].Text.Trim().Length > 1)
                {
                    filePath = sg1.Rows[index].Cells[6].Text.Substring(sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg1.Rows[index].Cells[6].Text.ToUpper().Length - sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                }
                break;

            case "SG1_UPLD":
                string UploadedFile = ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).FileName;
                string filepath = @"c:\TEJ_ERP\UPLOAD\";
                string fileName = txtvchnum.Text.Trim() + fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY") + frm_CDT1.Replace(@"/", "_") + "~" + UploadedFile;
                filepath = filepath + fileName;
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(filepath);
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
                sg1.Rows[index].Cells[5].Text = UploadedFile;
                sg1.Rows[index].Cells[6].Text = filepath;
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnUpload_Click(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t10", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t11", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t12", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t13", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t14", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t15", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t16", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t17", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        if (sg2_dt != null)
        {
            sg2_dr = sg2_dt.NewRow();
            sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
            sg2_dr["sg2_t1"] = "-";
            sg2_dr["sg2_t2"] = "-";
            sg2_dr["sg2_t3"] = "-";
            sg2_dr["sg2_t4"] = "-";
            sg2_dr["sg2_t5"] = "-";
            sg2_dr["sg2_t6"] = "-";
            sg2_dr["sg2_t7"] = "-";
            sg2_dr["sg2_t8"] = "-";
            sg2_dr["sg2_t9"] = "-";
            sg2_dr["sg2_t10"] = "-";
            sg2_dr["sg2_t11"] = "-";
            sg2_dr["sg2_t12"] = "-";
            sg2_dr["sg2_t13"] = "-";
            sg2_dr["sg2_t14"] = "-";
            sg2_dr["sg2_t15"] = "-";
            sg2_dr["sg2_t16"] = "-";
            sg2_dr["sg2_t17"] = "-";
            sg2_dt.Rows.Add(sg2_dr);
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg2.Columns[0].HeaderStyle.Width = 30;
            sg2.Columns[1].HeaderStyle.Width = 30;
            sg2.Columns[2].HeaderStyle.Width = 50;
        }
    }
    //------------------------------------------------------------------------------------
}