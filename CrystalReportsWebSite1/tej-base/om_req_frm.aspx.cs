using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_req_frm : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", cmd_query;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    String pop_qry;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_IndType, wSeriesControl = "";
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
                    frm_IndType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                }
                else Response.Redirect("~/login.aspx");
            }
            wSeriesControl = "Y";
            if (!Page.IsPostBack)
            {

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
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
        if (dtCol == null) return;
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

            for (int K = 0; K < sg1.Rows.Count; K++)
            {
                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                //((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

            }
            orig_name = orig_name.ToUpper();
            //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    sg1.Columns[sR].Visible = false;
                }
                // Setting Heading Name
                sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }



        // to hide and show to tab panel



        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F25111":
                //tab2.Visible = false;
                //tab3.Visible = false;
                //tab4.Visible = false;
                //tab5.Visible = false;
                break;
        }
        if (Prg_Id == "M12008")
        {
            //tab5.Visible = true;
            //txtlbl8.Attributes.Remove("readonly");
            //txtlbl9.Attributes.Remove("readonly");
        }
        lblheader.Text = "Drawing Request Entry Form";
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();

        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();


    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_DRAW_REQ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";

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
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='1'";
                break;
            case "BTN_11":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='2'";
                break;
            case "BTN_12":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='3'";
                break;
            case "BTN_13":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='4'";
                break;
            case "BTN_14":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='H' and substr(type1,1,1)='1'";
                break;
            case "BTN_15":
                SQuery = "Select Type1 as fstr,Name,Type1 as Code,Addr1 as Owner,vchnum as Veh_type from type where id='G' and substr(type1,1,1)='2'  order by name,addr1";
                break;
            case "BTN_16":
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as Address,'-' as City from dual union all select 'PARTY VEHICLE' as Transporter,'-' as Code,'-' as Address,'-' as City from dual) order by  Transporter";
                break;
            case "BTN_17":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='>' order by name";
                break;
            case "BTN_18":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='<' order by name";
                break;

            case "BTN_19":
                SQuery = "SELECT '10' as fstr,'As Applicable' as NAME,'10' as Code FROM dual ";
                break;

            case "BTN_20":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_21":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_22":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                //pop1
                col1 = "";

                if (doc_addl.Value == "Y")
                {
                    SQuery = "select to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum as Fstr,a.vchnum as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as req_dt,b.Name as Deptt_Name,trim(a.acode) as Dept_Cd,trim(A.stage) as WIP_Stg,sum(a.iqty_chl)-sum(a.issued) as Pending_Qty,max(a.ent_by) As Request_by,round(sysdate-a.vchdate,0) as Pend_Days from (SELECT acode,stage,vchnum,vchdate,icode,ent_by,jobno,jobdt,req_qty as iqty_chl,0 as issued from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and nvl(closed,'-')!='Y' union all SELECT acode,stage,refnum,refdate,icode,null as entby,invno,invdate,0 as iqty_chl,iqtyout as issued from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a,type b where b.id='M' and trim(A.acode)=trim(B.type1) group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,b.Name,trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate,trim(A.stage) having sum(a.iqty_chl)-sum(a.issued)>0 order by fstr";
                }
                else
                {
                    SQuery = "select to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum as Fstr,a.vchnum as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as req_dt,b.Name as Deptt_Name,trim(a.acode) as Dept_Cd,trim(A.stage) as WIP_Stg,sum(a.iqty_chl)-sum(a.issued) as Pending_Qty,max(a.ent_by) As Request_by,round(sysdate-a.vchdate,0) as Pend_Days  from (SELECT acode,stage,vchnum,vchdate,icode,ent_by,'-' as jobno,vchdate as jobdt,req_qty as iqty_chl,0 as issued from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and nvl(closed,'-')!='Y' union all SELECT acode,stage,refnum,refdate,icode,null as entby,'-' as invno,vchdate,0 as iqty_chl,iqtyout as issued from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a,type b where b.id='M' and trim(A.acode)=trim(B.type1) group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,b.Name,trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate,trim(A.stage) having sum(a.iqty_chl)-sum(a.issued)>0 order by fstr";
                }

                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='M' AND SUBSTR(TYPE1,1,1) IN ('6','7') order by TYPE1 ";
                break;
            case "TICODE":
                //pop2
                SQuery = "select type1,name as State ,type1 as code from type where id='1' order by Name";
                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
                break;
            case "TICODEX":
                SQuery = "select type1,name as State ,type1 as code from type where id='{' order by Name";
                break;
            case "ACODE":
                SQuery = "select acode as fstr, aname as customer,acode AS code from famst where substr(trim(acode),1,2) in ('16') order by acode,aname";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                SQuery = "select a.vchnum||trim(a.icode)||to_char(vchdate,'dd/mm/yyyy') as fstr, a.icode,b.iname,b.cpartno,b.cdrgno,b.unit,a.vchnum,a.vchdate,a.dno,a.tno,a.rno,acode from wb_drawrec a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and vchdate " + DateRange + " order by a.vchdate desc  ";
                SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(F.MSGTXT) AS FSTR,A.VCHNUM AS ENT_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENT_DT,B.ANAME AS CUSTOMER,C.INAME AS PART_NAME,A.COL1 AS ECNO,f.terminal as design_type,a.t9 as drawing_stage,A.DNO as Part_No,a.acode,F.MSGTXT AS FILENAME FROM WB_DRAWREC A,FAMST B,ITEM C,ATCHVCH F WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=F.BRANCHCD||F.TYPE||tRIM(F.VCHNUM)||TO_CHAR(F.VCHDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in ('DE','PI','CI') and upper(trim(f.MSGFROM))='ACTIVATE' ORDER BY A.VCHNUM DESC,F.MSGTXT";
                if (wSeriesControl == "Y")
                    SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(F.MSGTXT) AS FSTR,A.VCHNUM AS ENT_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENT_DT,B.name AS CUSTOMER,C.NAME AS PART_NAME,A.COL1 AS ECNO,f.terminal as design_type,a.t9 as drawing_stage,A.DNO as Part_No,a.acode,F.MSGTXT AS FILENAME FROM WB_DRAWREC A,TYPEGRP B,TYPEGRP C,ATCHVCH F WHERE TRIM(A.ACODE)=TRIM(b.TYPE1) AND B.ID='C1' AND TRIM(A.ICODE)=TRIM(C.TYPE1) AND C.ID='P1' AND A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=F.BRANCHCD||F.TYPE||tRIM(F.VCHNUM)||TO_CHAR(F.VCHDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in ('DE','PI','CI') and upper(trim(f.MSGFROM))='ACTIVATE' ORDER BY A.VCHNUM DESC,F.MSGTXT";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_JOB":
                SQuery = "select * from (Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + DateRange + " and a.srno=1 order by a.vchdate desc,a.vchnum desc) where rownum<100";
                break;
            case "SG1_ROW_BTCH":
                SQuery = "select * from (Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + DateRange + " and a.srno=1 order by a.vchdate desc,a.vchnum desc) where rownum<100";

                col1 = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.Trim();
                col2 = "'-'";
                foreach (GridViewRow gr1 in sg1.Rows)
                {
                    if (gr1.Cells[13].Text.Trim().Length > 3 && ((TextBox)gr1.FindControl("sg1_t11")).Text.Trim().Length > 2)
                        col2 += "," + "'" + ((TextBox)gr1.FindControl("sg1_t11")).Text.Trim() + ((TextBox)gr1.FindControl("sg1_t12")).Text.Trim() + "'";
                }
                SQuery = "select a.icode,a.icode as erpcode,b.iname as product,b.cpartno,a.qty,a.btchno,a.btchdt from (select icode,sum(iqtyin) as qty,btchno,btchdt from (select trim(icodE) as icode,iqtyin,btchno,btchdt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and store='Y' and trim(icode)='" + col1 + "' union all select trim(icodE) as icode,-1*iqtyout,btchno,btchdt from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and store='Y' and trim(icode)='" + col1 + "') group by icode,btchno,btchdt having sum(iqtyin)>0) a,item b where trim(a.icode)=trim(B.icode) and a.btchno||a.btchdt not in (" + col2.TrimStart(',') + ")  order by a.icode,a.btchno,a.btchdt";
                break;
            case "sg1_t4":
                SQuery = "SELECT trim(a.ordno)||'-'||to_Char(a.orddt,'dd/mm/yyyy') as fstr,B.aname as customer,a.ordno,to_Char(a.orddt,'dd/mm/yyyy') as orddt,a.icode as erpcode,c.iname as product from somas a,famst b, item c where trim(a.acode)=trim(b.acodE) and trim(a.icode)=trim(C.icodE) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.orddt " + DateRange + " order by a.ordno,orddt";
                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                SQuery = "select  a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.icode,b.iname,a.col2 as Req_By,a.col4 as Deptt,a.vchnum,to_char(vchdate,'dd/mm/yyyy' ) as vchdate from wb_draw_req a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='DR' and a.vchdate " + DateRange + " order by a.vchdate desc";
                if (wSeriesControl == "Y")
                    SQuery = "select  a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.icode,b.name as iname,a.col2 as Req_By,a.col4 as Deptt,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy' ) as vchdate from wb_draw_req a , typegrp b where trim(a.icode)=trim(b.type1) and b.id='P1' and a.branchcd='" + frm_mbr + "' and a.type='DR' and a.vchdate " + DateRange + " order by a.vchdate desc";
                break;
            case "EMP":
                SQuery = "SELECT USERID AS FSTR,USERNAME AS NAME,USERID AS CODE FROM EVAS ORDER BY USERNAME";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                {
                    SQuery = "select  a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.icode,b.iname,a.col2 as Req_By,a.col4 as Deptt,a.vchnum,to_char(vchdate,'dd/mm/yyyy' ) as vchdate from wb_draw_req a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='DR' and a.vchdate " + DateRange + " order by a.vchdate desc";
                    if (wSeriesControl == "Y")
                        SQuery = "select  a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.icode,b.name iname,a.col2 as Req_By,a.col4 as Deptt,a.vchnum,to_char(vchdate,'dd/mm/yyyy' ) as vchdate from wb_draw_req a , typegrp b where trim(a.icode)=trim(b.type1) and b.id='P1' and a.branchcd='" + frm_mbr + "' and a.type='DR' and a.vchdate " + DateRange + " order by a.vchdate desc";
                }
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {

            TxtAttach2.Text = "";
            Label20.Text = "";
            if (typePopup == "N") newCase(frm_vty);


            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            lbl1a.Text = frm_vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "vch");
            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            txtentby.Text = frm_uname;
            txtentdt.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);

            txtlbl5.Text = frm_UserID;
            txtlbl21.Text = frm_uname;
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        TxtAttach2.Text = "";
        Label20.Text = "";
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Request No Edit", frm_qstr);

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
        if (txtlbl4.Text.Trim().Length < 2)
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Department Not Filled Correctly !!");
        }

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1033", txtvchdate.Text.Trim());
        if (chk_freeze == "1")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
            return;
        }
        if (chk_freeze == "2")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }


        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        string ok_for_save = "Y"; string err_item, err_msg;



        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        if (ok_for_save == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' MRR Qty is Exceeding Gate Entry Qty , Please Check item '13' " + err_item);
            return;
        }

        //**************** Stock Check


        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        if (ok_for_save == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot issue more the Stock Qty , Please Check item : " + err_item);
            return;
        }

        if (txtlbl4.Text == "-" || txtlbl4.Text == "" || txtlbl4.Text == "0")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Department !!");
            return;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------


    string reelGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("rcount", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["icode"] = gr.Cells[13].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                drQty["rcount"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t4")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }

        DataTable dtQty1 = new DataTable();
        dtQty1.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty1.Columns.Add(new DataColumn("iname", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("rcount", typeof(decimal)));
        DataRow drQty1 = null;
        col1 = "";
        i = 1;


        object sm, sm1;

        DataView distQty = new DataView(dtQty, "", "icode", DataViewRowState.CurrentRows);
        DataTable dtQty2 = new DataTable();
        dtQty2 = distQty.ToTable(true, "icode");

        foreach (DataRow drQty2 in dtQty2.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            sm1 = dtQty1.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Qty : " + sm.ToString() + "'13'Grid 2 Qty : " + sm1.ToString());
                break;
            }

            sm = dtQty1.Compute("max(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            sm1 = dtQty.Compute("sum(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0 && fgen.make_double(sm.ToString()) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Count : " + sm.ToString() + "'13'Grid 2 Count : " + sm1.ToString());
                break;
            }
        }
        return null;
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

        sg1_dt = new DataTable();
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        sg4_dt = new DataTable();

        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        TxtAttach2.Text = "";
        Label20.Text = "";

        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        ViewState["sg4"] = null;

        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                SQuery = "delete from " + frm_tabname + " a where branchcd='" + frm_mbr + "' and type='DR' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                //fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, "US", lblheader.Text.Trim() + " Deleted");
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
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    //txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    btnlbl4.Focus();

                    sg1_dt = new DataTable();
                    create_tab();
                    sg1_add_blankrows();


                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    setColHeadings();
                    ViewState["sg1"] = sg1_dt;



                    //-------------------------------------------

                    break;
                    #endregion
                case "Del":
                    if (col1 == "") return;
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Edit":
                    if (col1 == "") return;
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    SQuery = "select  a.*,b.iname,b.cpartno,b.cdrgno,b.unit from wb_draw_req a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='DR' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ";
                    if (wSeriesControl == "Y")
                        SQuery = "select  a.*,b.name as iname,b.acref2 as cpartno,b.acref3 as cdrgno,'-' unit from wb_draw_req a , typegrp b where trim(a.icode)=trim(b.type1) and b.id='P1' and a.branchcd='" + frm_mbr + "' and a.type='DR' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[i]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[i]["ent_dt"].ToString();
                        txtentby.Text = dt.Rows[i]["ent_by"].ToString();
                        txtentdt.Text = Convert.ToDateTime(dt.Rows[i]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtvchnum.Text = dt.Rows[i]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        lbl1a.Text = dt.Rows[i]["Type"].ToString().Trim();
                        //txtlbl12.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        //txtlbl13.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(acode)='" + dt.Rows[i]["Acode"].ToString().Trim() + "'", "aname");
                        txtlbl5.Text = dt.Rows[i]["col1"].ToString().Trim();
                        txtlbl21.Text = dt.Rows[i]["col2"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[i]["col3"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["col4"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[i]["col5"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["col6"].ToString().Trim();
                        //TxtAttach2.Text = dt.Rows[i]["FileName"].ToString().Trim();

                        txtrmk.Text = dt.Rows[i]["Remarks"].ToString().Trim();

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


                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["Cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["col8"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["col9"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["dno"].ToString().Trim();
                            sg1_dr["sg1_f9"] = dt.Rows[i]["rno"].ToString().Trim();
                            sg1_dr["sg1_f10"] = dt.Rows[i]["tno"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":

                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
                case "sg1_t4":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = "F/O #" + col1;
                    }
                    break;
                case "EMP":
                    if (col1.Length > 1)
                    {
                        txtlbl5.Text = col1;
                        txtlbl21.Text = col2.Length > 20 ? col2.Substring(0, 19) : col2;
                        txtrmk.Text = "Matl Issued to " + col2;
                    }
                    break;
                case "ACODE":
                    txtlbl12.Text = col1;
                    txtlbl13.Text = col2;

                    break;
                case "TACODE":
                    //-----------------------------
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    if (doc_addl.Value == "Y")
                    {
                        SQuery = "select a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as Fstr,b.Iname,b.Cpartno,b.cdrgno,b.unit,sum(a.iqty_chl)-sum(a.issued) as Pending,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as ERP_Code,max(a.ent_by) As Ind_by,max(a.desc_) As desc_,trim(a.jobno) as JobNo,to_char(a.jobdt,'dd/mm/yyyy') as jobdt,trim(a.acode) as acode,trim(a.stage) As wstage from (SELECT acode,stage,vchnum,vchdate,icode,ent_by,jobno,jobdt,req_qty as iqty_chl,0 as issued,desc_ from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and nvl(closed,'-')!='Y' union all SELECT acode,stage,refnum,refdate,icode,null as entby,invno,invdate,0 as iqty_chl,iqtyout as issued,null as desc_ from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a,item b where trim(A.icode)=trim(B.icode) and to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum='" + col1 + "' group by a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')||trim(a.icode),b.Iname,b.Cpartno,b.cdrgno,b.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode),trim(a.jobno),to_char(a.jobdt,'dd/mm/yyyy'),trim(a.acode),trim(a.stage)  having sum(a.iqty_chl)-sum(a.issued)>0  order by b.iname";
                    }
                    else
                    {
                        SQuery = "select a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as Fstr,b.Iname,b.Cpartno,b.cdrgno,b.unit,sum(a.iqty_chl)-sum(a.issued) as Pending,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as ERP_Code,max(a.ent_by) As Ind_by,max(a.desc_) As desc_,max(a.jobno) as JobNo,to_char(max(a.jobdt),'dd/mm/yyyy') as jobdt,trim(a.acode) as acode,trim(a.stage) As wstage from (SELECT acode,stage,vchnum,vchdate,icode,ent_by,'-' as jobno,vchdate as jobdt,req_qty as iqty_chl,0 as issued,desc_ from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and nvl(closed,'-')!='Y' union all SELECT acode,stage,refnum,refdate,icode,null as entby,'-' as invno,null as invdate,0 as iqty_chl,iqtyout as issued,null as desc_ from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a,item b where trim(A.icode)=trim(B.icode) and to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum='" + col1 + "' group by a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')||trim(a.icode),b.Iname,b.Cpartno,b.cdrgno,b.unit,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.icode),trim(a.acode),trim(a.stage)  having sum(a.iqty_chl)-sum(a.issued)>0  order by b.iname";
                    }

                    //SQuery = "Select b.iname,b.cpartno as icpartno,b.cdrgno as icdrgno,b.unit as iunit,a.morder,a.*,to_chaR(a.vchdate,'dd/mm/yyyy') as reqdtd,to_chaR(a.jobdt,'dd/mm/yyyy') as jobdtd,to_chaR(a.ent_dt,'dd/mm/yyyy') as entdtd from wb_iss_req a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum='" + frm_mbr + lbl1a.Text + col1 + "'  ORDER BY A.morder";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {


                        //txtlbl2.Text = dt.Rows[i]["vchnum"].ToString().Trim();
                        //txtlbl3.Text = dt.Rows[i]["vchdate"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");

                        txtlbl5.Text = dt.Rows[i]["Ind_by"].ToString().Trim();
                        //txtlbl6.Text = frm_uname;

                        txtlbl7.Text = dt.Rows[i]["wstage"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='1' and trim(upper(type1))=upper(Trim('" + txtlbl7.Text + "'))", "name");


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

                            sg1_dr["sg1_f1"] = dt.Rows[i]["erp_Code"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["erp_code"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["Pending"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["Pending"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = dt.Rows[i]["jobno"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["jobdt"].ToString().Trim();

                            sg1_dr["sg1_t14"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["vchdate"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["ind_by"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        //edmode.Value = "Y";
                    }
                    break;


                //-----------------------------



                case "BTN_10":
                    if (col1.Length <= 0) return;
                    //txtlbl10.Text = col2;
                    //btnlbl11.Focus();
                    break;

                case "BTN_15":

                    break;
                case "BTN_16":

                    break;
                case "BTN_17":

                    break;
                case "BTN_18":

                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "TICODEX":
                    if (col1.Length <= 0) return;
                    //txtlbl70.Text = col1;
                    //txtlbl71.Text = col2;
                    //txtlbl2.Focus();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return; dt = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["sg1_f8"].ToString();
                            sg1_dr["sg1_f9"] = dt.Rows[i]["sg1_f9"].ToString();
                            sg1_dr["sg1_f10"] = dt.Rows[i]["sg1_f10"].ToString();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        pop_qry = "";
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");

                        // IN PLACE OF FIELD NAME, VALUE OF VCHNUM AND VCHDATE IS GOING PLEASE CHECK BY MADHVI ON 10/12/2018
                        if (col1.Trim().Length < 8) SQuery = "select * from (" + pop_qry + ") where fstr in ('" + col1 + "') ";
                        else SQuery = "select * from (" + pop_qry + ") where fstr in (" + col1 + ") ";

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

                            sg1_dr["sg1_f1"] = dt.Rows[d]["part_no"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["part_name"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["ecno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["design_type"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["drawing_stage"].ToString().Trim();

                            sg1_dr["sg1_f6"] = dt.Rows[d]["ent_no"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[d]["ent_dt"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[d]["filename"].ToString().Trim();
                            sg1_dr["sg1_f9"] = dt.Rows[d]["acode"].ToString().Trim();
                            sg1_dr["sg1_f10"] = dt.Rows[d]["customer"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    //setGST();
                    break;
                case "SG2_ROW_ADD1":
                    hffield.Value = "SG2_ROW_ADD";

                    break;
                case "SG2_ROW_ADD":
                    if (col1.Length < 2) return;

                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }
                    pop_qry = "";
                    pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                    if (col1.Trim().Length < 8) SQuery = "select a.vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr, a.icode,b.iname,b.cpartno,b.cdrgno,b.unit,a.vchnum,a.vchdate,a.dno,a.tno,a.rno,acode from wb_drawrec a , item b where a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') = '" + col1 + "' and trim(a.icode)=trim(b.icode)  ";
                    else SQuery = "select a.vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr, a.icode,b.iname,b.cpartno,b.cdrgno,b.unit,a.vchnum,a.vchdate,a.dno,a.tno,a.rno,acode from wb_drawrec a , item b where a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') = '" + col1 + "' and trim(a.icode)=trim(b.icode)  ";

                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    setColHeadings();
                    break;
                case "SG2_ROW_JOB":
                    if (col1.Length <= 0) return;

                    break;
                case "SG3_ROW_ADD":
                    #region for gridview 1

                    #endregion
                    break;
                case "SG1_ROW_JOB":
                    if (col1.Length <= 2) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    break;
                case "SG1_ROW_BTCH":
                    if (col1.Length <= 2) return;
                    if (col1.Contains(","))
                    {
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
                                if (Convert.ToInt32(hf1.Value) == i)
                                {
                                    col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                                    col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");
                                    double d1 = 0, d2 = 0;
                                    for (z = 0; z < col2.Split(',').Length; z++)
                                    {
                                        sg1_dr = sg1_dt.NewRow();
                                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                                        sg1_dr["sg1_h1"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text;
                                        sg1_dr["sg1_h2"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text;
                                        sg1_dr["sg1_h3"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[2].Text;
                                        sg1_dr["sg1_h4"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text;
                                        sg1_dr["sg1_h5"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text;
                                        sg1_dr["sg1_h6"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text;
                                        sg1_dr["sg1_h7"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text;
                                        sg1_dr["sg1_h8"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text;
                                        sg1_dr["sg1_h9"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text;
                                        sg1_dr["sg1_h10"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text;

                                        sg1_dr["sg1_f1"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                                        sg1_dr["sg1_f2"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text;
                                        sg1_dr["sg1_f3"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text;
                                        sg1_dr["sg1_f4"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text;
                                        sg1_dr["sg1_f5"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text;

                                        if (Convert.ToInt32(hf1.Value) == z)
                                        {
                                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text.Trim();
                                            d1 = fgen.make_double(((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text.Trim());
                                        }
                                        else sg1_dr["sg1_t1"] = "0";
                                        sg1_dr["sg1_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").Split(',')[z].ToString().Trim().Replace("'", "");
                                        d2 += fgen.make_double(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").Split(',')[z].ToString().Trim().Replace("'", ""));
                                        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text.Trim();
                                        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text.Trim();
                                        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text.Trim();
                                        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text.Trim();
                                        //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                                        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text.Trim();
                                        sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text.Trim();
                                        //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                                        sg1_dr["sg1_t11"] = col2.Split(',')[z].ToString().Trim().Replace("'", "");
                                        sg1_dr["sg1_t12"] = col3.Split(',')[z].ToString().Trim().Replace("'", "");
                                        sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t13")).Text.Trim();
                                        sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t14")).Text.Trim();
                                        sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t15")).Text.Trim();
                                        sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text.Trim();
                                        sg1_dt.Rows.Add(sg1_dr);
                                    }

                                    sg1_dr = sg1_dt.NewRow();
                                    sg1_dr["sg1_f2"] = "Total";
                                    sg1_dr["sg1_t1"] = d1;
                                    sg1_dr["sg1_t2"] = d2;
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                                else
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
                                    sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                                    sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                                    sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                                    sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                                    sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                                    sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                                    sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                                    sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                                    sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                                    sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                                    //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                                    sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                                    sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                                    //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                                    sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                                    sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                                    sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                                    sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                                    sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                                    sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                            }
                        }

                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        setColHeadings();
                        //setGST();
                    }
                    else
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "").Replace("'", "");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "").Replace("'", "");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "").Replace("'", "");
                    }
                    break;

                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;


                case "SG2_RMV":
                    #region Remove Row from GridView

                    #endregion
                    //setColHeadings();
                    break;
                case "SG4_RMV":
                    #region Remove Row from GridView

                    #endregion
                    //setColHeadings();
                    break;

                case "SG3_RMV":
                    #region Remove Row from GridView
                    #endregion
                    // setColHeadings();
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
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[19].Text.Trim();
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[20].Text.Trim();
                            sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[21].Text.Trim();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[22].Text.Trim();


                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        if (edmode.Value == "Y")
                        {
                            //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }
                        else
                        {
                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }

                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            // added 22/04/2020 :: VV
            SQuery = "select  a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.icode,b.iname,a.col2 as Req_By,a.col4 as Deptt,a.vchnum,to_char(vchdate,'dd/mm/yyyy' ) as vchdate from wb_draw_req a , item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='DR' and a.vchdate " + PrdRange + " order by a.vchdate desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevelIMG("List of " + lblheader.Text + "", frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            if (edmode.Value == "Y")
            {

            }
            else
            {
            }

            i = 0;
            setColHeadings();

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    //oDs1 = new DataSet();
                    //oDs1 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);
                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();


                    oDS.Dispose();
                    //oDs1.Dispose();
                    oporow = null;
                    oDS = new DataSet();

                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        save_it = "Y";
                        frm_vnum = txtvchnum.Text;
                    }
                    else
                    {
                        save_it = "N";
                        save_it = "Y";

                        if (save_it == "Y")
                        {
                            i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='DR' order by vchdate desc ", 6, "VCH");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='DR'  order by vchdate desc ", 6, "VCH");
                                    pk_error = "N";
                                    i = 0;
                                }
                                i++;
                            }
                            while (pk_error == "Y");
                        }
                    }

                    // If Vchnum becomes 000000 then Re-Save
                    if (frm_vnum == "000000") btnhideF_Click(sender, e);

                    save_fun();
                    int xcountrows = 0;
                    xcountrows = sg1.Rows.Count;

                    if (edmode.Value == "Y")
                    {
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd='" + frm_mbr + "' and type='DR' and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' ";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        cmd_query = "delete from " + frm_tabname + " where branchcd='DD' and type='DR' and  vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'  ";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        fgen.msg("-", "AMSG", "Record Updated Successfully");



                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully ");
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
            #endregion
            }
            else btnsave.Disabled = false;
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h10", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f10", typeof(string)));


    }
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h5", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
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
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));

    }

    //------------------------------------------------------------------------------------
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
        sg1_dr["sg1_f6"] = "-";
        sg1_dr["sg1_f7"] = "-";
        sg1_dr["sg1_f8"] = "-";
        sg1_dr["sg1_f9"] = "-";
        sg1_dr["sg1_f10"] = "-";


        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
        if (sg2_dt == null) create_tab2();
        sg2_dr = sg2_dt.NewRow();

        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;

        sg2_dr["sg2_h1"] = "-";
        sg2_dr["sg2_h2"] = "-";
        sg2_dr["sg2_h3"] = "-";
        sg2_dr["sg2_h4"] = "-";
        sg2_dr["sg2_h5"] = "-";

        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";

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

        sg2_dt.Rows.Add(sg2_dr);
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
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
        sg4_dt.Rows.Add(sg4_dr);
    }



    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }

            //setGST();
            //if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
            //{
            //    sg1.HeaderRow.Cells[24].Text = "CGST";
            //    sg1.HeaderRow.Cells[25].Text = "SGST/UTGST";
            //}
            //else
            //{
            //    sg1.HeaderRow.Cells[24].Text = "IGST";
            //    sg1.HeaderRow.Cells[25].Text = "-";
            //}
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG1_ROW_JOB":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_JOB";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Job No.", frm_qstr);
                }
                break;
            case "SG1_ROW_BTCH":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_BTCH";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Batch No.", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD":

                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------


    //------------------------------------------------------------------------------------

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Request Slip ", frm_qstr);
    }
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_19";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }



    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type ", frm_qstr);
    }

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        string inv_St_dt = ""; string srno;

        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        srno = fgen.seek_iname(frm_qstr, frm_cocd, "select max(srno) as srno  from wb_draw_req", "srno");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {

            oporow = oDS.Tables[0].NewRow();
            oporow["srno"] = Convert.ToInt32(srno) + 1;
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = lbl1a.Text.Substring(0, 2);
            oporow["vchnum"] = txtvchnum.Text.ToString().Trim();
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["col1"] = txtlbl5.Text.Trim();
            oporow["col2"] = txtlbl21.Text.Trim();
            oporow["col3"] = txtlbl4.Text.Trim();
            oporow["col4"] = txtlbl4a.Text.Trim();
            oporow["col5"] = txtlbl7.Text.Trim();
            oporow["col6"] = txtlbl7a.Text.Trim();
            oporow["Acode"] = txtlbl12.Text.Trim();

            if (TxtAttach2.Text.Length > 0)
            {
                oporow["Filename"] = TxtAttach2.Text.Trim();
            }

            oporow["acode"] = sg1.Rows[i].Cells[21].Text.Trim();
            oporow["Icode"] = sg1.Rows[i].Cells[13].Text.Trim();
            oporow["col8"] = sg1.Rows[i].Cells[18].Text.Trim();
            oporow["col9"] = sg1.Rows[i].Cells[19].Text.Trim();
            oporow["dno"] = sg1.Rows[i].Cells[20].Text.Trim();
            oporow["rno"] = sg1.Rows[i].Cells[21].Text.Trim();
            oporow["tno"] = sg1.Rows[i].Cells[22].Text.Trim();



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
                oporow["eDt_dt"] = vardate;

            }
            oporow["Remarks"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oDS.Tables[0].Rows.Add(oporow);

        }
    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='M' and type1 like '3%' and type1!='36' order by type1";
    }
    //------------------------------------------------------------------------------------




    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg1_t4_"))
        {
            hffield.Value = "sg1_t4";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t4_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select W.O Number", frm_qstr);
        }
    }
    protected void btnReqBy_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "EMP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Requested By ", frm_qstr);
    }

    void newCase(string vty)
    {
        #region
        vty = "DR";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        disablectrl();
        fgen.EnableForm(this.Controls);


        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();


        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        sg1_dt.Dispose();
        set_Val();
        #endregion
    }
    protected void btndraw_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void BtnView2_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Label20.Text.Substring(Label20.Text.ToUpper().IndexOf("UPLOAD"), Label20.Text.Length - Label20.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }
    protected void BtnDown2_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filePath = Label20.Text.Substring(Label20.Text.ToUpper().IndexOf("UPLOAD"), Label20.Text.Length - Label20.Text.ToUpper().IndexOf("UPLOAD"));
            //Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");//old
            Session["FilePath"] = Label20.Text;
            Session["FileName"] = TxtAttach2.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }
    protected void BtnAttach2_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";
        filepath = Server.MapPath("~/tej-base/UPLOAD/");
        FileUpload2.Visible = true;
        if (FileUpload2.HasFile)
        {
            TxtAttach2.Text = FileUpload2.FileName;
            //filepath = filepath + "_" + txtdocno.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;//old
            filepath = filepath + "_" + frm_mbr + "_" + "DR" + "_" + txtvchnum.Text.Trim() + "_" + txtvchdate.Text.Replace(@"/", "_") + "~" + FileUpload2.FileName;
            //  Attch.PostedFile.SaveAs(filepath);
            FileUpload2.PostedFile.SaveAs(filepath);
            filepath = Server.MapPath("~/tej-base/UPLOAD/") + "_" + frm_mbr + "_" + "DR" + "_" + txtvchnum.Text.Trim() + "_" + txtvchdate.Text.Replace(@"/", "_") + "~" + FileUpload2.FileName;
            FileUpload2.PostedFile.SaveAs(filepath);
            Label20.Text = filepath;
            TxtAttach2.Text = filepath;
            BtnView2.Visible = true;
            BtnDown2.Visible = true;
        }
        else
        {
            Label20.Text = "";
        }
    }
    protected void BtnCust_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
}