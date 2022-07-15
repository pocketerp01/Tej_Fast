using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class fin_ppc_web_om_mcplan : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xprdrange1, mq2;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    double JobQty = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    fgenDB fgen = new fgenDB();
    protected string ArrayStore = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        btnnew.Focus();
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
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
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                setVal();
                //fillJob();
            }
            hfQstr.Value = frm_qstr;
            hfFormID.Value = frm_formID;
        }
    }
    void setVal()
    {
        switch (frm_formID)
        {
            case "F35107":
                lblheader.Text = "Machine Wise Planning";
                break;
            case "F35108":
                lblheader.Text = "Order / Line Planning";
                break;
        }
    }
    void fillJob()
    {
        dt = new DataTable();

        string party_cd = "";
        string part_cd = "";
        party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
        part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

        SQuery = "SELECT * FROM (SELECT a.job_no||'-'||a.job_dt||'-'||a.job_Qty||'-'||((a.job_qty / 1000)*Req_time)||'-'||a.stage_code||'-'||a.icode as fstr,a.job_no as vchnum,a.job_Dt as vchdate,a.icode,b.iname,b.cpartno,b.unit,a.job_qty as qty,a.planned,a.stage_name,a.stage_code,a.Req_time from (select trim(job_no) as job_no,trim(job_Dt) as job_Dt,max(Name) as Stage_name,trim(Stage_code) as Stage_code,trim(icode) as icode,sum(qty) as job_Qty,sum(planned) as planned,round(max(Req_time),2) as Req_time,max(srno) as Jsrno from (select c.name,b.stagec as Stage_code,round(b.mtime1,2) as Req_time,a.vchnum as job_no,to_char(a.vchdate,'dd/mm/yyyy') as job_Dt,a.icode,a.qty,0 as planned,b.srno from costestimate a,itwstage b,type c where trim(b.stagec)=trim(c.type1) and c.id='K' and trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PrdRange + " and a.srno=1 and b.stagec!='08' and trim(nvl(a.app_by,'-'))!='-' and trim(nvl(a.status,'-'))!='Y' and trim(nvl(a.status,'-'))!='Y' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' union all select null as name,stage,0 as Req_time,job_no,job_Dt,icode,0 as jobqty,a1 as pla_qty ,null as srno from prod_Sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + PrdRange + ") group by trim(Stage_code),trim(job_no),trim(job_Dt),trim(icode) having sum(qty)-sum(planned)>0) a,item b where trim(a.icode)=trim(b.icode) order by a.job_no,a.job_dt,b.iname,a.Jsrno) WHERE ROWNUM<201 ";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dt = new DataTable();
        sg1_dt.Columns.Add("fstr", typeof(string));
        for (int i = 0; i < 11; i++)
        {
            sg1_dt.Columns.Add("sg1_f" + (i + 1), typeof(string));
        }
        sg1_dr = null;
        foreach (DataRow dr in dt.Rows)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["fstr"] = dr["fstr"].ToString().Trim();
            sg1_dr["sg1_f1"] = dr["QTY"].ToString().Trim();
            sg1_dr["sg1_f2"] = dr["vchnum"].ToString().Trim();
            sg1_dr["sg1_f3"] = dr["VCHDATE"].ToString().Trim();

            sg1_dr["sg1_f4"] = dr["planned"].ToString().Trim();
            sg1_dr["sg1_f5"] = dr["req_time"].ToString().Trim();

            sg1_dr["sg1_f6"] = dr["INAME"].ToString().Trim();
            sg1_dr["sg1_f7"] = dr["CPARTNO"].ToString().Trim();
            sg1_dr["sg1_f8"] = dr["UNIT"].ToString().Trim();

            sg1_dr["sg1_f9"] = dr["stage_name"].ToString().Trim();
            sg1_dr["sg1_f10"] = dr["stage_code"].ToString().Trim();
            sg1_dr["sg1_f11"] = dr["icode"].ToString().Trim();

            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        sg2_dt = new DataTable();
        sg2_dt.Columns.Add("sg2_f1", typeof(string));
        sg2_dr = null;

        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(MCHCODE)||'-'||TRIM(MCHNAME) AS sg2_f1,mchcode from pmaint where branchcd='" + frm_mbr + "' and type='10' order by MCHNAME ");
        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            sg2_dr = sg2_dt.NewRow();
            sg2_dr["sg2_f1"] = dt2.Rows[i]["sg2_f1"].ToString();
            sg2_dt.Rows.Add(sg2_dr);
        }

        sg2.DataSource = sg2_dt;
        sg2.DataBind();

    }
    void fillOrders()
    {
        dt = new DataTable();

        string party_cd = "";
        string part_cd = "";
        string bal_query = "";
        party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
        part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");


        //Select acode,icode,job_no,job_Dt,a1,0 as sold from prod_Sheet where branchcd='" & mbr & "' and type='OP' union all Select acode,icode,ponum,to_char(podate,'dd/mm/yyyy'),0 as a1,iqtyout as sold from ivoucher where branchcd='" & mbr & "' and type like '4%' and vchdate " & xprd3 & "
        //SQuery = "SELECT * FROM (SELECT A.ORDNO||'-'||A.ORDDT||'-'||A.QTY||'-'|| (A.QTY * IS_NUMBER(B.oprate1))||'-'||'61'||'-'||a.icode AS FSTR,A.ORDNO,A.ORDDT,A.ICODE,B.INAME,B.CPARTNO,B.UNIT,A.QTY,A.QTY as planned,'-' AS CLIENTNAME,'-' NAME2,B.oprate1 AS req_time FROM (SELECT TRIM(ORDNO) AS ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY') AS ORDDT,TRIM(ICODE) AS ICODE,QTYORD AS QTY,0 AS DELVQTY FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND type !='45' and ORDDT " + PrdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ORDER BY ORDNO DESC) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) )  ";

        bal_query = "select ORDNO,ORDDT,ICODE,Acode,sum(QTY) As OQTY,sum(DELVQTY) as DELVQTY,sum(planned) as planned,(sum(QTY)-sum(DELVQTY))-sum(planned) As QTY,max(podt) as podt from (SELECT TRIM(ORDNO) AS ORDNO,TO_CHAR(ORDDT,'DD/MM/YYYY') AS ORDDT,TRIM(ICODE) AS ICODE,TRIM(aCODE) AS aCODE,QTYORD AS QTY,0 AS DELVQTY,0 as planned,TO_CHAR(ORDDT,'YYYYMMDD') AS podt FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND type !='45' and ORDDT " + PrdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all SELECT TRIM(ponum) AS ORDNO,TO_CHAR(podate,'DD/MM/YYYY') AS ORDDT,TRIM(ICODE) AS ICODE,TRIM(aCODE) AS aCODE,0 AS QTY,iqtyout AS DELVQTY,0 as planned,TO_CHAR(podate,'YYYYMMDD') AS podt  FROM ivoucher WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND type !='45' and vchdate " + PrdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all SELECT TRIM(job_no) AS ORDNO,trim(job_Dt) AS ORDDT,TRIM(ICODE) AS ICODE,TRIM(aCODE) AS aCODE,0 AS QTY,0 AS DELVQTY,a1 as planned,null as podt FROM prod_sheet WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'OP%'  and acode like '" + party_cd + "%' and icode like '" + part_cd + "%') group by ORDNO,ORDDT,ICODE,Acode having (sum(QTY)-sum(DELVQTY))-sum(planned)>0 ";

        SQuery = "SELECT * FROM (SELECT A.ORDNO||'-'||A.ORDDT||'-'||A.QTY||'-'|| (A.QTY * IS_NUMBER(B.oprate5))||'-'||'61'||'-'||a.icode AS FSTR,A.ORDNO,A.ORDDT,A.ICODE,B.INAME,B.CPARTNO,B.UNIT,A.QTY,A.QTY as planned,'-' AS CLIENTNAME,'-' NAME2,B.oprate5 AS req_time,a.podt FROM (" + bal_query + ") A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) order by a.podt)  ";
        
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dt = new DataTable();
        sg1_dt.Columns.Add("fstr", typeof(string));
        for (int i = 0; i < 11; i++)
        {
            sg1_dt.Columns.Add("sg1_f" + (i + 1), typeof(string));
        }
        sg1_dr = null;
        foreach (DataRow dr in dt.Rows)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["fstr"] = dr["fstr"].ToString().Trim();
            sg1_dr["sg1_f1"] = dr["QTY"].ToString().Trim();
            sg1_dr["sg1_f2"] = dr["ORDNO"].ToString().Trim();
            sg1_dr["sg1_f3"] = dr["ORDDT"].ToString().Trim();

            sg1_dr["sg1_f4"] = "0";
            sg1_dr["sg1_f5"] = dr["req_time"].ToString().Trim();

            sg1_dr["sg1_f6"] = dr["INAME"].ToString().Trim();
            sg1_dr["sg1_f7"] = dr["CPARTNO"].ToString().Trim();
            sg1_dr["sg1_f8"] = dr["UNIT"].ToString().Trim();

            sg1_dr["sg1_f9"] = dr["CLIENTNAME"].ToString().Trim();
            sg1_dr["sg1_f10"] = dr["NAME2"].ToString().Trim();
            sg1_dr["sg1_f11"] = dr["icode"].ToString().Trim();

            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        sg2_dt = new DataTable();
        sg2_dt.Columns.Add("sg2_f1", typeof(string));
        sg2_dr = null;

        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(MCHCODE)||'-'||TRIM(MCHNAME) AS sg2_f1,mchcode from pmaint where branchcd='" + frm_mbr + "' and type='10' and trim(acode) like '14%' order by MCHNAME ");
        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            sg2_dr = sg2_dt.NewRow();
            sg2_dr["sg2_f1"] = dt2.Rows[i]["sg2_f1"].ToString();
            sg2_dt.Rows.Add(sg2_dr);
        }

        sg2.DataSource = sg2_dt;
        sg2.DataBind();

    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (frm_formID)
            {
                case "F35107":
                    sg1.HeaderRow.Cells[0].Text = "Job Detail";
                    sg1.HeaderRow.Cells[1].Text = "Job Qty";
                    sg1.HeaderRow.Cells[2].Text = "Job No";
                    sg1.HeaderRow.Cells[3].Text = "Job Dt";

                    sg1.HeaderRow.Cells[4].Text = "Planned";
                    sg1.HeaderRow.Cells[5].Text = "Time/1000";

                    sg1.HeaderRow.Cells[6].Text = "Product";
                    sg1.HeaderRow.Cells[7].Text = "Part No";

                    sg1.HeaderRow.Cells[8].Text = "UOM";


                    sg1.HeaderRow.Cells[9].Text = "Stage";
                    sg1.HeaderRow.Cells[10].Text = "Code";
                    sg1.HeaderRow.Cells[11].Text = "ERP Code";
                    break;
                case "F35108":
                    sg1.HeaderRow.Cells[0].Text = "Job Detail";
                    sg1.HeaderRow.Cells[1].Text = "Job Qty";
                    sg1.HeaderRow.Cells[2].Text = "Job No";
                    sg1.HeaderRow.Cells[3].Text = "Job Dt";

                    sg1.HeaderRow.Cells[4].Text = "Planned";
                    sg1.HeaderRow.Cells[5].Text = "Time/pc";

                    sg1.HeaderRow.Cells[6].Text = "Product";
                    sg1.HeaderRow.Cells[7].Text = "Part No";

                    sg1.HeaderRow.Cells[8].Text = "UOM";


                    sg1.HeaderRow.Cells[9].Text = "Stage";
                    sg1.HeaderRow.Cells[10].Text = "Code";
                    sg1.HeaderRow.Cells[11].Text = "ERP Code";
                    break;

            }
        }
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (frm_formID)
            {
                case "F35107":
                case "F35108":
                    sg2.HeaderRow.Cells[0].Text = "Machine/Line Name";
                    sg2.HeaderRow.Cells[1].Text = "Tot.Time";
                    sg2.HeaderRow.Cells[2].Text = "1st Slot";
                    sg2.HeaderRow.Cells[3].Text = "2nd Slot";
                    sg2.HeaderRow.Cells[4].Text = "3rd Slot";

                    sg2.HeaderRow.Cells[5].Text = "4th Slot";
                    sg2.HeaderRow.Cells[6].Text = "5th Slot";
                    sg2.HeaderRow.Cells[7].Text = "6th Slot";

                    sg2.HeaderRow.Cells[8].Text = "7th Slot";
                    sg2.HeaderRow.Cells[9].Text = "8th Slot";
                    sg2.HeaderRow.Cells[10].Text = "9th Slot";

                    sg2.HeaderRow.Cells[11].Text = "10th Slot";
                    sg2.HeaderRow.Cells[12].Text = "11th Slot";
                    sg2.HeaderRow.Cells[13].Text = "12th Slot";
                    break;
            }
        }
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            switch (frm_formID)
            {
                case "F35107":
                    hffield.Value = "Shift";
                    {
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Shift", frm_qstr);
                    }
                    break;
                default:
                    hffield.Value = "New";
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
            }

        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Del";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
    }
    void make_qry_4_popup()
    {
        switch (hffield.Value)
        {
            case "New":
                SQuery = "";
                break;

            case "Shift":
                SQuery = "SELECT TYPE1||'-'||NAME AS FSTR,NAME,TYPE1 AS CODE,PLACE AS Available_time FROM TYPE WHERE ID='D' AND SUBSTR(TYPE1,1,1)='1' ORDER BY TYPE1 ";
                break;

            case "Print":
            case "Del":
                SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,A.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.shftcode,a.prevcode as shift,a.ename,to_char(a.vchdate,'yyyymmdd') as vdd from prod_sheet A ,item b where TRIM(a.icode)=trim(b.icode)  AND a.branchcd='" + frm_mbr + "' and a.type='90' and a.vchdate " + DateRange + "  order by vdd desc,a.vchnum desc";
                break;

            case "OtherPrint":
                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.name,a.ename as Machine,a.job_no,A.JOB_dT,a.ent_by,a.prevcode from prod_sheet a ,(select NAME,type1 from type where id='K' order by TYPE1 ) b where a.stage=b.type1 and a.branchcd='" + frm_mbr + "' AND a.type='90' and a.VCHDATE  " + DateRange + "  and a.vchnum<>'000000' order by a.vchnum desc";
                break;
        }
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    public void clearctrl()
    {
        hffield.Value = "";
    }
    protected void btnShift_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Shift";
        {
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
        col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
        col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

        switch (hffield.Value)
        {
            case "Shift":
                txtShiftName.Text = col1;
                hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                txtShiftTime.Text = hf1.Value;
                hffield.Value = "New";
                fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                break;

            case "Print":
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                fgen.fin_prod_reps(frm_qstr);
                break;

            case "Del":
                fgen.execute_cmd(frm_qstr, frm_cocd, "Delete from prod_sheet where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "Delete from wsr_ctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' AND FINPKFLD LIKE 'PROD_SHEET%'");
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, col2, col3, frm_uname, "90", lblheader.Text);
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + col2 + "");
                clearctrl(); fgen.ResetForm(this.Controls);
                break;

            case "OtherPrint":
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                fgen.fin_prodpp_reps(frm_qstr);
                break;
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "New":
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                switch (frm_formID)
                {
                    case "F35107":
                        fillJob();
                        break;
                    case "F35108":
                        fillOrders();
                        break;
                }
                btndel.Disabled = true;
                btnprint.Disabled = true;
                btnOtherPrint.Disabled = true;
                break;
        }
    }
    protected void btnhideSave_Click(object sender, EventArgs e)
    {
        fgen.msg("-", "AMSG", "Data Saved Successfully!!");
        btndel.Disabled = true;
        btnprint.Disabled = true;
    }
    protected void btnOtherPrint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "OtherPrint";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
}