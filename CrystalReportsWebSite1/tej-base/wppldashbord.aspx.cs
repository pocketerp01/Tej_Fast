using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;


public partial class wppldashbord : System.Web.UI.Page
{
    fgenDB fgen = new fgenDB();
    string co_cd, uname, mbr, ulvl, year, vardate, fromdt, todt, xprdrange, SQuery, val, value1, value2, value3, cDT1, cDT2, cldt, mq8;
    int i = 0;
    DataTable sg1_dt, dt, dt1, dt2 = new DataTable();
    DataRow sg1_dr;
    string frm_url, frm_qstr, frm_formID;

    protected void Page_Load(object sender, EventArgs e)
    {
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
            }
        }
    }

    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("sg1_Srno", typeof(Int32)));
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f11", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();
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
        sg1_dr["sg1_f11"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
    //-----------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ViewState["USERID"] = e.Row.Cells[3].Text;
            if (e.Row.Cells[3].Text.Length >= 25)
            {
                e.Row.Cells[3].ToolTip = ViewState["USERID"].ToString();
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                e.Row.Cells[0].ForeColor = System.Drawing.Color.White;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string type1 = e.Row.Cells[9].Text;
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (type1 == "DK")
                    {
                        e.Row.Cells[10].Text = "Not Done";
                        cell.BackColor = System.Drawing.Color.Red;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                    if (type1 == "TA")
                    {
                        e.Row.Cells[10].Text = "Done";
                        cell.BackColor = System.Drawing.Color.Green;
                        cell.ForeColor = System.Drawing.Color.White;
                    }
                }
            }
            sg1.HeaderRow.Cells[0].Text = "Sr. No";
            sg1.HeaderRow.Cells[0].Width = 30;
            sg1.HeaderRow.Cells[1].Text = "Entry No";
            sg1.HeaderRow.Cells[1].Width = 50;
            sg1.HeaderRow.Cells[2].Text = "Entry Date";
            sg1.HeaderRow.Cells[2].Width = 50;
            sg1.HeaderRow.Cells[3].Text = "Task Action No";
            sg1.HeaderRow.Cells[3].Width = 50;
            sg1.HeaderRow.Cells[4].Text = "Task Action Date";
            sg1.HeaderRow.Cells[4].Width = 50;
            sg1.HeaderRow.Cells[5].Text = "User Id";
            sg1.HeaderRow.Cells[5].Width = 150;
            sg1.HeaderRow.Cells[6].Text = "Subject";
            sg1.HeaderRow.Cells[6].Width = 150;
            sg1.HeaderRow.Cells[7].Text = "PRIORITY";
            sg1.HeaderRow.Cells[7].Width = 50;
            sg1.HeaderRow.Cells[8].Text = "Message";
            sg1.HeaderRow.Cells[8].Width = 170;
            sg1.HeaderRow.Cells[9].Text = "Type";
            sg1.HeaderRow.Cells[9].Width = 50;
            sg1.HeaderRow.Cells[10].Text = "DONE/NOT DONE";
            sg1.HeaderRow.Cells[10].Width = 50;
            sg1.HeaderRow.Cells[11].Text = "Task Type";
            sg1.HeaderRow.Cells[11].Width = 50;
        }
    }
    //--------------------------------------------------------------------------   
    public void fillgrid()
    {
        #region
        val = hfhcid.Value.Trim();

        {
            cDT1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(fmdate,'dd/mm/yyyy') as fromdt from co where code='" + co_cd + year + "'", "fromdt");
            cDT2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(todate,'dd/mm/yyyy') as todate from co where code='" + co_cd + year + "'", "todate");

            xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

        #endregion
            SQuery = "SELECT DISTINCT BRANCHCD AS FSTR,TYPE,VCHNUM AS ENTRY_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,COL1 AS USERID,COL14 AS SUBJECT,COL4 AS PRIORITY,REMARKS AS MESSAGE,(case when nvl(trim(col17),'-')='-' then 'NORMAL TASK' else col17 end) as task_type  FROM SCRATCH WHERE BRANCHCD='" + mbr + "' AND TYPE='DK' AND VCHDATE " + xprdrange + " AND TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') NOT IN (SELECT TRIM(COL6)||TO_CHAR(COL48,'DD/MM/YYYY') AS ID FROM SCRATCH2 WHERE branchcd='" + mbr + "' AND TYPE='TA' and col48 " + xprdrange + ") ORDER BY VCHNUM ";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, co_cd, SQuery);
            SQuery = "SELECT DISTINCT BRANCHCD AS FSTR,TYPE,COL6 AS ENTRY_NO,TO_CHAR(COL48,'DD/MM/YYYY') AS ENTRY_DATE,vchnum AS Actionno,to_char(vchdate,'dd/mm/yyyy') as action_dt,COL2 AS USERID,COL14 AS SUBJECT,COL4 AS PRIORITY,REMARKS AS MESSAGE,'-' as Done FROM SCRATCH2 WHERE BRANCHCD='" + mbr + "' AND TYPE='TA' AND COL48 " + xprdrange + " order by vchnum";
            SQuery = "SELECT DISTINCT a.BRANCHCD AS FSTR,a.TYPE,a.COL6 AS ENTRY_NO,TO_CHAR(a.COL48,'DD/MM/YYYY') AS ENTRY_DATE,a.vchnum AS Actionno,to_char(a.vchdate,'dd/mm/yyyy') as action_dt,b.username AS USERID,a.COL14 AS SUBJECT,a.COL4 AS PRIORITY,a.REMARKS AS MESSAGE,'-' as Done FROM SCRATCH2 a,evas b WHERE trim(a.COL2)=trim(B.userid) and a.BRANCHCD='" + mbr + "' AND a.TYPE='TA' AND a.COL48 " + xprdrange + " order by a.vchnum";
            dt1 = new DataTable();
            dt1 = fgen.getdata(frm_qstr, co_cd, SQuery);
            mq8 = "SELECT DISTINCT BRANCHCD AS FSTR,TYPE,VCHNUM AS ENTRY_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,(case when nvl(trim(col17),'-')='-' then 'NORMAL TASK' else col17 end) as task_type FROM SCRATCH WHERE BRANCHCD='" + mbr + "' AND TYPE='DK' AND VCHDATE " + xprdrange + " order by vchnum";
            DataTable dt3 = new DataTable();
            dt3 = fgen.getdata(frm_qstr, co_cd, mq8);
            dt2 = new DataTable();
            dt2.Merge(dt);
            dt2.Merge(dt1);
            create_tab();
            sg1_dr = null;
            for (i = 0; i < dt2.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_srno"] = i + 1;
                sg1_dr["sg1_f1"] = dt2.Rows[i]["ENTRY_NO"].ToString().Trim();
                sg1_dr["sg1_f2"] = dt2.Rows[i]["ENTRY_DATE"].ToString().Trim();

                if (dt2.Columns.Contains("Actionno"))
                {
                    sg1_dr["sg1_f3"] = dt2.Rows[i]["Actionno"].ToString().Trim();
                    sg1_dr["sg1_f4"] = dt2.Rows[i]["action_dt"].ToString().Trim();
                }

                sg1_dr["sg1_f5"] = dt2.Rows[i]["USERID"].ToString().Trim();


                sg1_dr["sg1_f6"] = dt2.Rows[i]["SUBJECT"].ToString().Trim();
                sg1_dr["sg1_f7"] = dt2.Rows[i]["PRIORITY"].ToString().Trim();
                sg1_dr["sg1_f8"] = dt2.Rows[i]["MESSAGE"].ToString().Trim();
                sg1_dr["sg1_f9"] = dt2.Rows[i]["TYPE"].ToString().Trim();
                
                if (dt2.Columns.Contains("Done"))
                    sg1_dr["sg1_f10"] = dt2.Rows[i]["Done"].ToString().Trim();

                try
                {
                    if (dt2.Rows[i]["task_type"].ToString().Trim().Length > 1)
                    {
                        sg1_dr["sg1_f11"] = dt2.Rows[i]["task_type"].ToString().Trim();
                    }
                    else
                    {
                        sg1_dr["sg1_f11"] = fgen.seek_iname_dt(dt3, "entry_no='" + dt2.Rows[i]["entry_no"].ToString().Trim() + "' and entry_date='" + dt2.Rows[i]["entry_date"].ToString().Trim() + "'", "task_type");
                    }
                }
                catch
                {
                    sg1_dr["sg1_f11"] = fgen.seek_iname_dt(dt3, "entry_no='" + dt2.Rows[i]["entry_no"].ToString().Trim() + "' and entry_date='" + dt2.Rows[i]["entry_date"].ToString().Trim() + "'", "task_type");
                }
                sg1_dt.Rows.Add(sg1_dr);
            }
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            ViewState["sg1"] = sg1_dt;

        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
    }
}
