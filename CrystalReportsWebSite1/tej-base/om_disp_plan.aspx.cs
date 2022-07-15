using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_disp_plan : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

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
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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

                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");
                doc_addl.Value = "-";
                string chk_opt;
                tab3.Visible = false;
                btnPost.Visible = false;
                txtBarCode.Visible = false;
                btnRead.Visible = false;


                doc_addl.Value = "N";
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0003'", "fstr");
                if (chk_opt == "Y")
                {
                    doc_addl.Value = "Y";
                }

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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

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

        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;



        Button2.Visible = false;
        Button3.Visible = false;
        Button4.Visible = false;

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

        //btnlbl4.Enabled = false;
        //btnlbl7.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;

        //btnlbl4.Enabled = true;
        //btnlbl7.Enabled = true;

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

        frm_tabname = "WB_DISP_PLAN";

        frm_vty = "DP";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        

        lblheader.Text = "Dispatch Planning Record";
        
        divWork1.Visible = false;
        divWork2.Visible = false;
        divWork3.Visible = false;
        divWork4.Visible = false;

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        string pastcurrprd = "";
        pastcurrprd = "between to_DatE('" + frm_CDT1 + "','dd/mm/yyyy')-120 and to_DatE('" + frm_CDT2 + "','dd/mm/yyyy')";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "sg2_t8":
                SQuery = "SELECT NAME AS FSTR,NAME AS REASON,TYPE1 AS CODE FROM TYPEgrp WHERE ID='CR' AND BRANCHCD='" + frm_mbr + "' ORDER BY TYPE1 ";
                break;

            case "TACODE":
                string mind_type = "";
                mind_type = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                SQuery = "SELECT NAME AS FSTR,NAME AS Priority_name,TYPE1 AS CODE FROM TYPEgrp WHERE ID='PD' AND BRANCHCD!='DD' ORDER BY TYPE1 ";
                break;
            case "SUPV":

                SQuery = "SELECT USERNAME AS FSTR,USERNAME,USERID,EMAILID FROM EVAS where 1=1 ORDER BY USERNAME";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }

                if (col1.Length <= 0) col1 = "'-'";

                string cond = "";
                cond = " and sum(a.qtyord)-sum(a.salqty)>0";

                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as OPT_ENABLE from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID in ('W1100') order by OPT_ID", "OPT_ENABLE");


                SQuery = "max(A.fstr) as fstr,trim(a.Order_No) as SO_No,to_char(a.Orddt,'dd/mm/yyyy') as SO_DT,b.aname as Customer,max(a.pordno) as po_no,to_char(max(a.cu_chldt)-nvl(b.dlvtime,0),'dd/mm/yyyy') as SHIP_date,max(a.jcno) as job_card,max(to_char(a.porddt,'dd/mm/yyyy')) as po_Dt,sum(a.qtyord)as SO_qty,sum(a.Sch_qty)As Job_Qty,sum(a.prd_qty) as DA_Qty,sum(a.salqty) as Sal_Qty,sum(a.qtyord)-sum(a.salqty) as Bal_qty,(case when sum(A.qtyord)>0 then round(((sum(a.qtyord)-sum(a.salqty))/sum(a.qtyord))*100,2) else 0 end)as Perct ,round((max(a.cu_chldt)-nvl(b.dlvtime,0))-sysdate,0) as Days_Left,max(invdt) as Sales_Dt,max(to_Char(a.jcdt,'dd-Mon-yy')) as job_dt,max(a.work_ordno) As Order_Catg,max(a.icat) as SO_Closed,max(a.plant_cd) as Plant_Code,max(to_char(a.cu_chldt,'dd/mm/yyyy')) as Dlv_date,nvl(b.dlvtime,0) as dlvtime,trim(a.acode) as Acode,b.staten,b.district,b.pincode";
                if (col1 == "Y")
                {
                    SQuery = "select * from (select " + SQuery + " from  (select branchcd||type||ordno||to_char(orddt,'dd/mm/yyyy')||trim(acode)||trim(icode) as fstr,work_ordno,gmt_size,desc1,BUSI_EXPECT,cpartno,ciname,pordno,porddt,0 as stk,a.cu_chldt,a.ordno as Order_No,a.orddt as Orddt,a.acode,a.icode,a.qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,0 as salqty,null as jcdt,null as jcno,null as invdt,icat,trim(nvl(mfginbr,'-')) as Plant_cd,a.irate*(decode(a.curr_Rate,0,1,a.curr_rate)) as fg_rt from somas a where a.branchcd='00' and A.orddt >=to_Date('01/01/2021','dd/mm/yyyy') and a.type!='45' and trim(a.MFGINBR)='" + frm_mbr + "'  union all select null as fstr,null as wo_no,null as aax,null as aab,null as aa,null as cpartno,null as ciname, null as pordno,null as porddt,0 as stk,null as cu_Chldt,substr(convdate,5,6) as Order_No,to_Date(substr(convdate,11,10),'dd/mm/yyyy') as Orddt,a.acode,a.icode,0 as qty_ord,qty as Qty1,0 as Qty2,0 as jcqty,0 as salqty ,vchdate as jcdt,vchnum as jcno,null as invdt,null as icat,null as plant_Cd,0 as fg_Rt from costestimate a where a.branchcd='" + frm_mbr + "' and a.type like '30%' and A.vchdate >=to_Date('01/01/2021','dd/mm/yyyy') and a.srno=0 union all select null as fstr,null as wono,null as aax,null as aab,null as aa,null as cpartno,null as ciname, null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.ponum as Order_No,a.podate as Orddt,a.acode,a.icode,0 as qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,iqtyout as salqty,null as jcdt,null as jcno,vchdate as invdt,null as icat,null as plant_Cd,0 as fg_Rt  from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.vchdate >=to_Date('01/01/2021','dd/mm/yyyy')  union all select null as fstr,null as wo_no,null as aax,null as aab,null as aa,null as cpartno,null as ciname, null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.ordno as Order_No,a.orddt as Orddt,a.acode,a.icode,0 as qtyord,0 as sch_qty,qtysupp as prd_qty,0 as jcqty,0 as salqty,null as jcdt,null as jcno,null as invdt,null as icat,null as plant_Cd,0 as fg_Rt  from despatch a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.packdate >=to_Date('01/01/2021','dd/mm/yyyy')  )a ,famst b, item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(C.icode) group by b.staten,b.district,b.pincode,a.Order_No,a.Orddt,to_char(a.Orddt,'dd/mm/yyyy'),trim(a.acode),nvl(b.dlvtime,0),b.aname having max(a.icat)<>'Y' and max(a.icat) <>'Y' " + cond + " ) where 1=1 order by Days_Left,Customer,SO_NO";
                }
                else
                {
                    SQuery = "select * from (select " + SQuery + " from (select branchcd||type||ordno||to_char(orddt,'dd/mm/yyyy')||trim(acode)||trim(icode) as fstr,work_ordno,gmt_size,desc1,BUSI_EXPECT,cpartno,ciname,pordno,porddt,0 as stk,a.cu_chldt,a.ordno as Order_No,a.orddt as Orddt,a.acode,a.icode,a.qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,0 as salqty,null as jcdt,null as jcno,null as invdt,icat,trim(nvl(mfginbr,'-')) as Plant_cd,a.irate*(decode(a.curr_Rate,0,1,a.curr_rate)) as fg_rt from somas a where a.branchcd='" + frm_mbr + "' and A.orddt >=to_Date('01/01/2021','dd/mm/yyyy') and a.type!='45'  union all select null as fstr,null as wo_no,null as aax,null as aab,null as aa,null as cpartno,null as ciname, null as pordno,null as porddt,0 as stk,null as cu_Chldt,substr(convdate,5,6) as Order_No,to_Date(substr(convdate,11,10),'dd/mm/yyyy') as Orddt,a.acode,a.icode,0 as qty_ord,qty as Qty1,0 as Qty2,0 as jcqty,0 as salqty ,vchdate as jcdt,vchnum as jcno,null as invdt,null as icat,null as plant_Cd,0 as fg_Rt from costestimate a where a.branchcd='" + frm_mbr + "' and a.type like '30%' and A.vchdate >=to_Date('01/01/2021','dd/mm/yyyy') and a.srno=0 union all select null as fstr,null as wono,null as aax,null as aab,null as aa,null as cpartno,null as ciname, null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.ponum as Order_No,a.podate as Orddt,a.acode,a.icode,0 as qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,iqtyout as salqty,null as jcdt,null as jcno,vchdate as invdt,null as icat,null as plant_Cd,0 as fg_Rt  from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.vchdate >=to_Date('01/01/2021','dd/mm/yyyy')  union all select null as fstr,null as wo_no,null as aax,null as aab,null as aa,null as cpartno,null as ciname, null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.ordno as Order_No,a.orddt as Orddt,a.acode,a.icode,0 as qtyord,0 as sch_qty,qtysupp as prd_qty,0 as jcqty,0 as salqty,null as jcdt,null as jcno,null as invdt,null as icat,null as plant_Cd,0 as fg_Rt  from despatch a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.packdate >=to_Date('01/01/2021','dd/mm/yyyy')  )a ,famst b, item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(C.icode) group by b.staten,b.district,b.pincode,a.Order_No,a.Orddt,to_char(a.Orddt,'dd/mm/yyyy'),trim(a.acode),nvl(b.dlvtime,0) as dlvtime,b.aname having max(a.icat)<>'Y' and max(a.icat) <>'Y' " + cond + " ) where 1=1 order by Days_Left,Customer,SO_NO";
                }
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;

            case "SG1_ROW_JOB":
                SQuery = "Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + pastcurrprd + " and a.srno=0 and trim(nvl(a.app_by,'-'))!='-' order by a.vchdate desc,a.vchnum desc";
                break;
            case "SG1_ROW_BTCH":
                SQuery = "select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + DateRange + " and a.srno=0 and trim(nvl(a.app_by,'-'))!='-' order by a.vchdate desc,a.vchnum desc";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";

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
            // if want to ask popup at the time of new
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("select type", frm_qstr);

            // else comment upper code
            set_Val();
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            txtvchnum.Value = frm_vnum;
            txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);
            col1 = "";

            create_tab();
            sg1_add_blankrows();

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            setColHeadings();
            ViewState["sg1"] = sg1_dt;
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //-----------------------------------------------------------------------------------
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

        string chk_indust = "";
        chk_indust = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(upper(opt_param)),2,'0') as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W1000'", "fstr");
        int i;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        for (i = 0; i < sg1.Rows.Count - 0; i++)
         {
             if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
             {
                 if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) <= 0 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0)
                 {
                     Checked_ok = "N";
                     fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Weight / CFT Not Filled Correctly at Line " + (i + 1) + "  !!");

                     return;
                 }

                 if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Length < 10)
                 {
                     Checked_ok = "N";
                     fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Plan Date Date Not Filled at Line " + (i + 1) + "  !!");

                     i = sg1.Rows.Count;
                     return;
                 }
                 else
                 {
                     string curr_dt;
                     string reqd_bydt;
                     curr_dt = Convert.ToDateTime(txtvchdate.Value).ToString("dd/MM/yyyy");
                     reqd_bydt = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text).ToString("dd/MM/yyyy");

                     if (Convert.ToDateTime(curr_dt) > Convert.ToDateTime(reqd_bydt))
                     {
                         Checked_ok = "N";
                         fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Plan Date Cannot be Less Than Current Date, See line No. " + (i + 1) + "  !!");
                         i = sg1.Rows.Count;
                         return;
                     }
                 }
             }
        }


        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Value) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Value) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }


        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        string ok_for_save = "Y"; string err_item, err_msg;


        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        //if (ok_for_save == "N")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' MRR Qty is Exceeding Gate Entry Qty , Please Check item '13' " + err_item);
        //    return;
        //}
        //**************** Stock Check
        //checkStockQty();

        //ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        //err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        //if (ok_for_save == "N")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot issue more the Stock Qty , '13' Remove this row from Grid '13' Please Check item : " + err_item);
        //    return;
        //}

        //string startTime = (txtbox3.Value.Right(10) + " " + txtbox7.Value.PadLeft(2, '0') + ":" + txtbox8.Value.PadLeft(2, '0'));
        //string endTime = (txtvchdate.Value.Right(10) + " " + txtbox10.Value.PadLeft(2, '0') + ":" + txtbox11.Value.PadLeft(2, '0'));

        //double mints = (Convert.ToDateTime(endTime) - Convert.ToDateTime(startTime)).TotalMinutes;
        //hfTime.Value = mints.ToString();

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


    string checkStockQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("fstr", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");



            //col1 = fgen.seek_iname(frm_qstr,frm_cocd,SQuery,"")

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim() + " Stock Qty : " + col1);
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

        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();

        sg4_add_blankrows();
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

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
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
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
        string mind_type = "";
        string plan_base = "Y";
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    //lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Value = frm_vnum;
                    txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    //txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    //txtlbl2.Text = frm_uname;


                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    //btnlbl4.Focus();

                    sg1_dt = new DataTable();
                    create_tab();
                    sg1_add_blankrows();


                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    setColHeadings();
                    ViewState["sg1"] = sg1_dt;

                    sg2_dt = new DataTable();
                    create_tab2();
                    sg2_add_blankrows();
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    setColHeadings();
                    ViewState["sg2"] = sg2_dt;

                    sg3_dt = new DataTable();
                    create_tab3();
                    sg3_add_blankrows();
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    setColHeadings();
                    ViewState["sg3"] = sg3_dt;


                    //-------------------------------------------
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    SQuery = "Select nvl(a.obj_name,'-') as udf_name from udf_config a where trim(a.frm_name)='" + Prg_Id + "' ORDER BY a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    create_tab4();
                    sg4_dr = null;
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;

                            sg4_dr["sg4_t1"] = dt.Rows[i]["udf_name"].ToString().Trim();
                            sg4_dt.Rows.Add(sg4_dr);
                        }
                    }
                    sg4_add_blankrows();
                    ViewState["sg4"] = sg4_dt;
                    sg4.DataSource = sg4_dt;
                    sg4.DataBind();
                    dt.Dispose();
                    sg4_dt.Dispose();
                    //-------------------------------------------


                    break;
                    #endregion
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Editsss":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    //lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
                    break;
                case "Del_E":
                    if (col1 == "") return;
                    if (frm_vty.Left(1) == "3")
                    {
                        string mhd = "";
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(VCHNUM)||'-'||TO_CHAR(vCHDATE,'DD/MM/YYYY') AS ISSUE_DTL FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(REFNUM)||TO_cHAR(REFDATE,'DD/MM/YYYY')='" + col1 + "' ", "ISSUE_DTL");
                        if (mhd != "0")
                        {
                            fgen.msg("-", "AMSG", "Delete not allowed, Issue Entry is already done against this on # " + mhd);
                            return;
                        }
                    }
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
                case "Edit":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();



                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_char(A.ent_Dt,'dd/mm/yyyy') as entdtd from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ";


                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        //oporow["COMPDT"] = txtvchdate.Value.Trim();

                        txtbox3.Value = dt.Rows[i]["DP_CODE"].ToString().Trim();
                        txtbox4.Value = dt.Rows[i]["other1"].ToString().Trim();
                        txtbox5.Value = dt.Rows[i]["other2"].ToString().Trim();
                        txtbox6.Value = dt.Rows[i]["other3"].ToString().Trim();




                        create_tab();
                        sg1_dr = null;

                        dt = new DataTable();
                        SQuery = "SELECT to_chaR(a.plan_Dt,'dd/mm/yyyy') As plan_Dtd,A.*,b.aname,b.staten,b.district,b.pincode FROM "+ frm_tabname+" A,famst B WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='DP' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + col1 + "' ORDER BY A.SRNO ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
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

                            sg1_dr["sg1_f1"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["aname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["district"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["staten"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["pincode"].ToString().Trim();

                            sg1_dr["sg1_t1"] = Convert.ToDateTime(dt.Rows[i]["PLAN_DTD"].ToString().Trim()).ToString("yyy-MM-dd");

                            //sg1_dr["sg1_t1"] = dt.Rows[i]["PLAN_DTD"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["disp_wt"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["disp_cft"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["remarks"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["ordno"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["orddt"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------

                        ////// REEL TABLE
                        ////SQuery = "SELECT A.*,b.iname FROM REELVCH_temp A,item b WHERE trim(a.icodE)=trim(B.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                        ////dt = new DataTable();
                        ////dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        ////create_tab2();
                        ////sg2_dr = null;
                        ////i = 1;
                        ////if (dt.Rows.Count > 0)
                        ////{
                        ////    foreach (DataRow dr in dt.Rows)
                        ////    {
                        ////        sg2_dr = sg2_dt.NewRow();

                        ////        sg2_dr["sg2_srno"] = i;
                        ////        sg2_dr["sg2_h1"] = dr["icode"].ToString().Trim();
                        ////        sg2_dr["sg2_h2"] = dr["icode"].ToString().Trim();
                        ////        sg2_dr["sg2_h3"] = "";
                        ////        sg2_dr["sg2_h4"] = "";
                        ////        sg2_dr["sg2_h5"] = "";

                        ////        sg2_dr["sg2_f1"] = dr["icode"].ToString().Trim();
                        ////        sg2_dr["sg2_f2"] = dr["iname"].ToString().Trim();
                        ////        sg2_dr["sg2_f3"] = "";
                        ////        sg2_dr["sg2_f4"] = "";
                        ////        sg2_dr["sg2_f5"] = "";

                        ////        sg2_dr["sg2_t1"] = dr["kclreelno"].ToString().Trim();
                        ////        sg2_dr["sg2_t2"] = dr["psize"].ToString().Trim();
                        ////        sg2_dr["sg2_t3"] = dr["gsm"].ToString().Trim();
                        ////        sg2_dr["sg2_t4"] = dr["REELWOUT"].ToString().Trim();
                        ////        sg2_dr["sg2_t5"] = dr["irate"].ToString().Trim();
                        ////        sg2_dr["sg2_t6"] = dr["job_no"].ToString().Trim();
                        ////        sg2_dr["sg2_t7"] = dr["reelspec1"].ToString().Trim();
                        ////        sg2_dr["sg2_t8"] = dr["reelspec2"].ToString().Trim();
                        ////        sg2_dr["sg2_t9"] = i.ToString(); ;
                        ////        sg2_dr["sg2_t10"] = dr["rinsp_by"].ToString().Trim();

                        ////        sg2_dt.Rows.Add(sg2_dr);
                        ////        i++;
                        ////    }
                        ////}
                        ////sg2_add_blankrows();
                        ////ViewState["sg2"] = sg2_dt;
                        ////sg2.DataSource = sg2_dt;
                        ////sg2.DataBind();
                        ////dt.Dispose();
                        ////sg2_dt.Dispose();

                        //-----------------------
                        SQuery = "Select nvl(a.udf_name,'-') as udf_name,nvl(a.udf_value,'-') as udf_value from udf_Data a where trim(a.par_tbl)='" + frm_tabname + "' and trim(a.par_fld)='" + mv_col + "' ORDER BY a.srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab4();
                        sg4_dr = null;
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {

                                sg4_dr = sg4_dt.NewRow();
                                sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;

                                sg4_dr["sg4_t1"] = dt.Rows[i]["udf_name"].ToString().Trim();
                                sg4_dr["sg4_t2"] = dt.Rows[i]["udf_value"].ToString().Trim();

                                sg4_dt.Rows.Add(sg4_dr);
                            }
                        }
                        sg4_add_blankrows();
                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                        dt.Dispose();
                        sg4_dt.Dispose();
                        //------------------------

                        ////------------------------
                        //SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and 1=2 ORDER BY A.SRNO ";
                        ////union all Select '-' as icode,to_DatE(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as dlv_Date,0 as budgetcost,'-' as iname,'-' As cpartno,'-' as cdrgno,nvl(b.unit,'-') as Unit from dual              

                        //dt = new DataTable();
                        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        //create_tab3();
                        //sg3_dr = null;
                        //if (dt.Rows.Count > 0)
                        //{
                        //    for (i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        sg3_dr = sg3_dt.NewRow();
                        //        sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                        //        sg3_dr["sg3_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                        //        sg3_dr["sg3_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                        //        sg3_dr["sg3_t1"] = dt.Rows[i]["dlv_Date"].ToString().Trim();
                        //        sg3_dr["sg3_t2"] = dt.Rows[i]["budgetcost"].ToString().Trim();
                        //        sg3_dr["sg3_t3"] = dt.Rows[i]["actualcost"].ToString().Trim();
                        //        sg3_dr["sg3_t4"] = dt.Rows[i]["jobcardrqd"].ToString().Trim();
                        //        sg3_dt.Rows.Add(sg3_dr);
                        //    }
                        //}
                        //sg3_add_blankrows();
                        //ViewState["sg3"] = sg3_dt;
                        //sg3.DataSource = sg3_dt;
                        //sg3.DataBind();
                        //dt.Dispose();
                        //sg3_dt.Dispose();

                        //-----------------------
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_prod_reps(frm_qstr);
                    break;
                case "sg2_t8":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t8")).Text = col2;
                        ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t9")).Focus();


                        hffield.Value = "RACODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Party", frm_qstr);
                    }
                    break;
                case "RACODE":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t10")).Text = col1;
                        ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t10")).Focus();
                    }
                    break;
                case "SUPV":
                    if (col1.Length <= 0) return;
                    //txtbox12.Value = col1;
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtbox3.Value = col1;
                    break;

                case "BTN_15":

                    break;
                case "BTN_16":

                    break;
                case "BTN_17":

                    break;
                case "BTN_18":

                    break;
                case "JOBX":
                    if (col1 == "") return;
                    lblJobNo.Text = col2 + "-" + col3;
                    break;


                case "SG1_ROW_ADD":
                    #region for gridview 1
                    string lastJobnum = "", lastJobDt = "";
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
                            lastJobnum = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            lastJobDt = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();


                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();


                        String pop_qry;

                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        
                        
                        if (col1.Trim().Length < 8) SQuery = "select * from ("+ pop_qry +") a where trim(a.fstr) in (" + col1 + ") ";
                        else SQuery = "select * from ("+ pop_qry +") a where trim(a.fstr) in (" + col1 + ") ";

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

                            sg1_dr["sg1_f1"] = dt.Rows[d]["acode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["customer"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["district"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["staten"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), txtvchdate.Value.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt.Rows[d]["pincode"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = dt.Rows[d]["so_no"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[d]["so_Dt"].ToString().Trim();
                            sg1_dr["sg1_t10"] = "-";



                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //dt.Dispose(); 
                    //sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    setGST();
                    break;
                case "SG2_ROW_ADD":
                    if (col1.Length < 2) return;
                    #region for gridview 2
                    if (col1.Length <= 0) return;
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();

                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();

                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) LIKE '7%' ORDER BY ICODE ";
                        SQuery = "SELECT distinct TRIM(A.ICODE)||trim(A.KCLREELNO) AS FSTR,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,A.KCLREELNO as lot_no,A.COREELNO,B.OPRATE1 as aa,B.OPRATE3 as bb,0 as row_balance,B.UNIT,0 as RATE FROM REELVCH A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) and type like '3%'  ";


                        dt = new DataTable();
                        SQuery = "SELECT * FROM (" + SQuery + ") WHERE FSTR IN (" + col1 + ") ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            for (int x = 0; x < dt.Rows.Count; x++)
                            {
                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                                sg2_dr["sg2_h1"] = dt.Rows[x]["ERPCODE"].ToString().Trim();
                                sg2_dr["sg2_h2"] = dt.Rows[x]["PRODUCT"].ToString().Trim();
                                sg2_dr["sg2_h3"] = "-";
                                sg2_dr["sg2_h4"] = "-";
                                sg2_dr["sg2_h5"] = "-";

                                sg2_dr["sg2_f1"] = dt.Rows[x]["ERPCODE"].ToString().Trim();
                                sg2_dr["sg2_f2"] = dt.Rows[x]["PRODUCT"].ToString().Trim();
                                sg2_dr["sg2_f3"] = "-";
                                sg2_dr["sg2_f4"] = "-";
                                sg2_dr["sg2_f5"] = "-";

                                sg2_dr["sg2_t1"] = dt.Rows[x]["LOT_NO"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dt.Rows[x]["AA"].ToString().Trim();
                                sg2_dr["sg2_t3"] = dt.Rows[x]["BB"].ToString().Trim();
                                sg2_dr["sg2_t4"] = dt.Rows[x]["ROW_BALANCE"].ToString().Trim();
                                sg2_dr["sg2_t5"] = dt.Rows[x]["RATE"].ToString().Trim();


                                sg2_dt.Rows.Add(sg2_dr);
                            }
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    if (sg2_dt != null)
                        ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
                    #endregion
                    setColHeadings();
                    setGST();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }

                    //********* Saving in Hidden Field
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, col1, txtvchdate.Value.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");

                    setColHeadings();
                    break;
                case "SG2_ROW_JOB":
                    if (col1.Length <= 0) return;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t6")).Text = col2;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t7")).Text = col3;

                    hffield.Value = "sg2_t8";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Reason", frm_qstr);
                    break;
                case "SG3_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg3"] != null)
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                            sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg3_dr["sg3_t1"] = "";
                            sg3_dr["sg3_t2"] = "";
                            sg3_dr["sg3_t3"] = "";
                            sg3_dr["sg3_t4"] = "";
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                    }
                    sg3_add_blankrows();

                    ViewState["sg3"] = sg3_dt;
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    dt.Dispose(); sg3_dt.Dispose();
                    ((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    #endregion
                    break;
                case "SG1_ROW_JOB":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Focus();

                    break;
                case "SG1_ROW_BTCH":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    break;

                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;


                case "SG2_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        i = 0;
                        for (i = 0; i < sg2.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();

                            sg2_dr["sg2_srno"] = (i + 1);
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();

                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();

                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg2_add_blankrows();

                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                    }
                    #endregion
                    setColHeadings();
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
                        for (i = 0; i < sg4.Rows.Count - 1; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = (i + 1);

                            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                            sg4_dt.Rows.Add(sg4_dr);
                        }

                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();

                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG3_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        i = 0;
                        for (i = 0; i < sg3.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = (i + 1);
                            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();

                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg3_add_blankrows();

                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
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
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        set_Val();
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");


            SQuery = "Select Substr(a.comp_ref,11,10) As Comp_Dt,a.Shft_Name,A.Deptt_Name,a.Mach_Name,A.Done_hrs,A.Done_Min,a.Issue_Obsv,a.Corr_Act,a.Prev_Act,a.Type,a.Vchnum as Action_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,a.ent_by,a.ent_Dt from " + frm_tabname + " a  where a.branchcd='" + frm_mbr + "' and a.type like 'MS%' and a." + doc_df.Value + " " + PrdRange + " order by a." + doc_df.Value + ",a." + doc_nf.Value + " ";


            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------


            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MS' and " + doc_df.Value + " " + DateRange + " ", "ldt");
            if (last_entdt == "0") { }
            else if (edmode.Value != "Y")
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Value.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Value.ToString() + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (txtvchdate.Value.ToString().Length <= 2) return;
            if (Convert.ToDateTime(txtvchdate.Value.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Value.ToString() + " ,Please Check !!");
            }
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

                        //oDS2 = new DataSet();
                        //oporow2 = null;
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "ivoucherw");

                        oDS4 = new DataSet();
                        oporow4 = null;
                        //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                        oDS5 = new DataSet();
                        oporow5 = null;
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();

                        //save_fun2();

                        //save_fun3();
                        //save_fun4();
                        save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "ivoucherw");


                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Value.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 4)
                                {
                                    save_it = "Y";
                                }
                            }

                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Value.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        //save_fun2();
                        //save_fun3();
                        //save_fun4();
                        save_fun5();
                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");

                            

                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS3, "ivoucherw");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                        fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Value + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Value + " Saved Successfully'13'Do you want to see the Print Preview ?");

                                #region Email Sending Function
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                //html started                            
                                sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                                sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                                sb.Append("<h5>" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR_NAME") + "</h5>");
                                //sb.Append("<br>Dear Sir/Mam,<br> This is to advise that the following <b>" + lblheader.Text + "</b> has been saved by " + frm_uname + ". Dept : " + txtbox10.Value.Trim() + "<br><br>");

                                //table structure
                                sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                                sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                                "<td><b>ERP Code</b></td><td><b>Product</b></td><td><b>Part No.</b></td><td><b>Qty</b></td><td><b>Unit</b></td>");
                                //vipin
                                foreach (GridViewRow gr in sg1.Rows)
                                {
                                    if (gr.Cells[13].Text.Trim().Length > 4)
                                    {
                                        sb.Append("<tr>");
                                        sb.Append("<td>");
                                        sb.Append(gr.Cells[13].Text.Trim());
                                        sb.Append("</td>");
                                        sb.Append("<td>");
                                        sb.Append(gr.Cells[14].Text.Trim());
                                        sb.Append("</td>");
                                        sb.Append("<td>");
                                        sb.Append(gr.Cells[15].Text.Trim());
                                        sb.Append("</td>");
                                        sb.Append("<td>");
                                        sb.Append(((TextBox)gr.FindControl("sg1_t1")).Text.Trim());
                                        sb.Append("</td>");
                                        sb.Append("<td>");
                                        sb.Append(gr.Cells[17].Text.Trim());
                                        sb.Append("</td>");
                                        sb.Append("</tr>");
                                    }
                                }
                                sb.Append("</table></br>");

                                sb.Append("Thanks & Regards");
                                sb.Append("<h5>Note: This Report is Auto generated from Finsys ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                                sb.Append("</body></html>");

                                //send mail
                                string subj = "";
                                if (edmode.Value == "Y") subj = "Edited : ";
                                else subj = "New Entry : ";
                                fgen.send_Activity_mail(frm_qstr, frm_cocd, "Finsys ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);


                                fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr"), frm_uname, edmode.Value);

                                sb.Clear();
                                #endregion
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved !! Please Check Item Detail List once, Is there any item!!");
                                btnsave.Disabled = false;

                            }
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Value.Trim() + "'");
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Value + " " + txtvchdate.Value.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hffield.Value = "SAVED";

                        setColHeadings();
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
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

        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));


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
        if (sg1_dt == null) create_tab();
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


        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
        if (sg2_dt == null) return;
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
        if (sg3_dt == null) return;
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
            setGST();
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

        if (txtvchnum.Value == "-")
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
                    fgen.Fn_open_sseek("Select Batch No.", frm_qstr);
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
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG2_RMV":
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG2_ROW_ADD":
                hffield.Value = "SG2_ROW_ADD";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                col1 = "";
                foreach (GridViewRow gr2 in sg2.Rows)
                {
                    if (col1.Length > 0) col1 += ",'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                    else col1 = "'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                }

                SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) LIKE '7%' ORDER BY ICODE ";
                SQuery = "SELECT distinct TRIM(A.ICODE)||trim(A.KCLREELNO) AS FSTR,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,A.KCLREELNO,A.COREELNO,B.OPRATE1 as Itm_Width,B.OPRATE3 as Thk,0 as weight,B.UNIT,0 as irate FROM REELVCH A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) and trim(a.icode)||trim(a.kclreelno) not in (" + col1 + ") ";


                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_mseek("Select Item", frm_qstr);
                break;
            case "SG2_ROW_JOB":
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                }
                hffield.Value = "SG2_ROW_JOB";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                SQuery = "Select distinct a.vchnum||a.vchdate as fstr,trim(a.Vchnum) as Job_no,to_Char(a.vchdate,'dd/mm/yyyy') as job_Dt,a.type,b.iname as item_name from costestimate a,item b where trim(a.icode)=trim(b.icodE) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + "  and trim(nvl(a.app_by,'-'))!='-' order by trim(a.vchnum)  ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Job No", frm_qstr);
                break;
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":
                if (index < sg3.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG3_ROW_ADD":
                if (index < sg3.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG3_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg4_RMV":
                if (index < sg4.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "sg4_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "sg4_ROW_ADD":
                dt = new DataTable();
                sg4_dt = new DataTable();
                dt = (DataTable)ViewState["sg4"];
                z = dt.Rows.Count - 1;
                sg4_dt = dt.Clone();
                sg4_dr = null;
                i = 0;
                for (i = 0; i < sg4.Rows.Count; i++)
                {
                    sg4_dr = sg4_dt.NewRow();
                    sg4_dr["sg4_srno"] = (i + 1);
                    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                    sg4_dt.Rows.Add(sg4_dr);
                }
                sg4_add_blankrows();
                ViewState["sg4"] = sg4_dt;
                sg4.DataSource = sg4_dt;
                sg4.DataBind();
                break;
        }
    }

    //------------------------------------------------------------------------------------

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        string mind_type = "";
        mind_type = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (Prg_Id == "F39211" || Prg_Id == "F40211")
        {
            fgen.Fn_open_sseek("Select Party ", frm_qstr);
        }
        else
        {
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP") == "SG_TYPE" && (frm_vty == "30" || frm_vty == "31")) fgen.Fn_open_mseek("Select " + (frm_cocd == "MULT" ? "Department" : "Plan"), frm_qstr);
            else
                fgen.Fn_open_sseek("Select " + (mind_type == "05" || mind_type == "06" ? "Department" : "Plan") + "", frm_qstr);
        }

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
        fgen.Fn_open_sseek("Select WIP Section ", frm_qstr);
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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F35014":
                fgen.Fn_open_sseek("Select Item to Convert ", frm_qstr);
                break;
            default:
                fgen.Fn_open_sseek("Select WIP Section ", frm_qstr);
                break;
        }

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
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["branchcd"] = frm_mbr;
                oporow["type"] = "DP";
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Value;

                oporow["Acode"] = sg1.Rows[i].Cells[13].Text.Trim();
                
                oporow["PLAN_DT"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim(),vardate);

                oporow["disp_wt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim());
                oporow["disp_cft"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());
                oporow["remarks"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim(); ;

                oporow["DP_Code"] = txtbox3.Value;
                oporow["other1"] = txtbox4.Value;
                oporow["other2"] = txtbox5.Value;
                oporow["other3"] = txtbox6.Value;

                oporow["ordno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim(); ;
                oporow["orddt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim(); ;

                oporow["SRNO"] = i;

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


                oDS.Tables[0].Rows.Add(oporow);
            }
        }

        ////oporow = oDS.Tables[0].NewRow();

        ////oporow["BRANCHCD"] = frm_mbr;
        ////oporow["TYPE"] = frm_vty;
        ////oporow["VCHNUM"] = frm_vnum;
        ////oporow["VCHDATE"] = txtvchdate.Value.Trim();
        ////oporow["COMP_LOCN"] = "-";

        ////oporow["COMPDT"] = txtvchdate.Value.Trim();

        ////oporow["COMP_REF"] = txtbox3.Value.ToUpper().Trim();
        ////oporow["SHFT_NAME"] = txtbox4.Value.ToUpper().Trim();
        ////oporow["DEPTT_NAME"] = txtbox5.Value.ToUpper().Trim();
        ////oporow["MACH_NAME"] = txtbox6.Value.ToUpper().Trim();

        //////oporow["DONE_HRS"] = txtbox10.Value.ToUpper().Trim();
        //////oporow["DONE_MIN"] = txtbox11.Value.ToUpper().Trim();

        ////oporow["ISSUE_OBSV"] = txtbox20.Text.ToUpper().Trim();
        ////oporow["REMARKS"] = txtbox21.Text.ToUpper().Trim();

        ////oporow["CORR_ACT"] = txtbox22.Text.ToUpper().Trim();
        ////oporow["PREV_ACT"] = txtbox23.Text.ToUpper().Trim();


        ////oporow["SPARE_CONS"] = "-";
        ////oporow["SPARE_COST"] = 0;


        ////oporow["DT_MINS"] = hfTime.Value.toDouble();


        ////if (edmode.Value == "Y")
        ////{
        ////    oporow["eNt_by"] = ViewState["entby"].ToString();
        ////    oporow["eNt_dt"] = ViewState["entdt"].ToString();
        ////    //oporow["edt_by"] = frm_uname;
        ////    //oporow["edt_dt"] = vardate;
        ////}
        ////else
        ////{
        ////    oporow["eNt_by"] = frm_uname;
        ////    oporow["eNt_dt"] = vardate;
        ////    //oporow["edt_by"] = "-";
        ////    //oporow["eDt_dt"] = vardate;
        ////}


        ////oDS.Tables[0].Rows.Add(oporow);

    }
    void save_fun2()
    {

    }
    void save_fun3()
    {
 

    }
    void save_fun4()
    {
    }
    void save_fun5()
    {
        for (i = 0; i < sg4.Rows.Count - 0; i++)
        {
            if (((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Length > 1)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["par_tbl"] = frm_tabname.ToUpper().Trim();
                oporow5["par_fld"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Value.Trim();
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F75115":
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='M' and type1 like '3%' and type1='3M' order by type1";

                break;
        }
    }
    //------------------------------------------------------------------------------------
    void setGST()
    {

    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int z = 3; z <= 7; z++)
            {
                sg2.HeaderRow.Cells[z].Style["display"] = "none";
                e.Row.Cells[z].Style["display"] = "none";
            }

            for (int z = 10; z <= 12; z++)
            {
                sg2.HeaderRow.Cells[z].Style["display"] = "none";
                e.Row.Cells[z].Style["display"] = "none";
            }
        }
    }
    protected void btnPost_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnRead_ServerClick(object sender, EventArgs e)
    {

    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnsupv_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SUPV";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supervisor", frm_qstr);
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        hffield.Value = "sg2_t8";
        hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg2_sg2_t8_", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Reason", frm_qstr);
    }
}