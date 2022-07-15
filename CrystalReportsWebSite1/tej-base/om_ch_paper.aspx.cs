using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;


public partial class om_ch_paper : System.Web.UI.Page
{
    string SQuery = "", frm_mbr = "", DateRange = "", frm_CDT1 = "", frm_CDT2 = "", frm_myear = "";
    string HCID, frm_url, frm_qstr, frm_formID, frm_cocd;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
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
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
                HCID = frm_formID;
                if (HCID == null)
                {
                    HCID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                }
            }

            if (!Page.IsPostBack)
            {
                if (HCID == "F10135")
                {
                    txtLenFrom.Text = "0";
                    txtLenTo.Text = "1000";
                    txtWidthFrom.Text = "0";
                    txtWidthTo.Text = "1000";
                    txtGSMFrom.Text = "0";
                    txtGSMTo.Text = "1000";
                }
                else
                {
                    //why hard code ?
                    txtLenFrom.Text = "0";
                    txtLenTo.Text = "1000";
                    txtWidthFrom.Text = "0";
                    txtWidthTo.Text = "1000";
                    txtGSMFrom.Text = "0";
                    txtGSMTo.Text = "1000";
                }
            }
        }

        txtsearch.Attributes["onkeydown"] = "if (event.keyCode == 40) { $('[TabIndex=1]').focus(); }";
    }

    protected void btnlbl_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SUBGROUP";
        SQuery = "Select trim(icode) as fstr,Iname,Icode from item where substr(icode,1,2) in ('01','02','06','07','08','09','70','80','81') and length(Trim(icode))=4 and  substr(icode,1,2) <='15' order by icode";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOX");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_mseek("Select Sub Group", frm_qstr);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        hffield.Value = "Paper";
        frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
        frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
        frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
        string col1 = "";
        if (txtlblSubGroup.Text.Trim().Length > 1)
        {
            col1 = " and substr(trim(icode),1,4) in (" + txtlblSubGroup.Text + ")";
        }
        SQuery = "select trim(a.iname) as fstr,a.* from (select x.iname,x.icode, x.opening as opening , nvl(y.cdbts,0) as  Receipts, nvl(y.ccdts,0) as Issues ,x.opening+nvl(y.cdbts,0)-nvl(y.ccdts,0) as Closing,x.oprate1 as PSize,x.oprate2 as Pwidth,x.oprate3 as Pgsm,x.opening+nvl(y.cdbts,0)-nvl(y.ccdts,0) as xClosing,X.CPARTNO,x.maker as W_Used from (select a.icode,a.iname,a.maker,a.cpartno,a.opening_bal+nvl(b.newop,0) opening,a.oprate1,a.oprate2,a.oprate3 from (select substr(f.icode,1,2) as grp,f.icode ,f.maker,f.cpartno,F.oprate1,f.oprate2,f.oprate3,f.iname , sum(nvl(fb.op,0)) as opening_bal from item f left outer join  (Select icode,sum(YR_" + frm_myear + ") as op from itembal where branchcd ='" + frm_mbr + "' group by icode) fb on trim(f.icode)=trim(fb.icode) where (oprate1 between " + txtLenFrom.Text + " and " + txtLenTo.Text + ") and (oprate2 between " + txtWidthFrom.Text + " and " + txtWidthTo.Text + ") and (oprate3 between " + txtGSMFrom.Text + " and " + txtGSMTo.Text + ") group by substr(f.icode,1,2),f.icode,f.iname,f.maker,f.cpartno,F.oprate1,f.oprate2,f.oprate3 ) a left outer join (select v.icode,nvl(sum(v.iqtyin),0)-nvl(sum(v.iqtyout),0) newop from ivoucher v where v.store='Y' and v.branchcd ='" + frm_mbr + "' and v.vchdate between TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') and TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') -1 group by v.icode) b on trim(a.icode)=trim(b.icode) ) x left outer join (select v.icode,sum(v.iqtyin) cdbts,sum(v.iqtyout)ccdts from ivoucher v where v.store='Y'  and v.branchcd ='" + frm_mbr + "' and v.vchdate between TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') and TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY') group by v.icode) y on trim(x.icode)=trim(y.icode)) a where substr(icode,1,2) in ('01','02','06','07','08','09','70','80','81') " + col1 + " order by icode";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "IBOX");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

        DataTable dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            ViewState["sg1"] = dt;
            sg1.DataSource = dt;
            sg1.DataBind();

            lblTotcount.Text = "Total Rows : " + sg1.Rows.Count;
        }


        //if (HCID == "F10135" || HCID == "F10135C") fgen.Fn_open_sseek("Select Paper", frm_qstr);
        //else fgen.Fn_open_mseek("Select Paper", frm_qstr);
    }

    protected void btnExit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "onlyclose();", true);
    }

    protected void btniBox_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "SUBGROUP":
                txtlblSubGroup.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                break;

            case "Paper":
                txtlblPaper.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                btnsubmit.Focus();
                break;
        }
    }

    protected void btnsubmit_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", txtlblSubGroup.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", txtlblPaper.Text.Trim());
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = sg1.SelectedRow;

        string Value1 = row.Cells[3].Text.Trim();
        string Value2 = row.Cells[2].Text.Trim();
        int iColumnas = sg1.HeaderRow.Cells.Count - 1;

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", Value1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", Value2);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", row.Cells[3].Text.Trim());

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTYCODE", txtlblSubGroup.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_PARTCODE", Value1);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup1();", true);
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";
            e.Row.ToolTip = "Click to select this row.";
            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[1].Style["display"] = "none";
        }
    }
    string gvSortExpression { get; set; }
    protected void sg1_Sorting(object sender, GridViewSortEventArgs e)
    {
        var D = (DataTable)ViewState["sg1"];
        Image sortImage = new Image();
        if (ViewState["sortDir"] == null) ViewState["sortDir"] = " ASC";
        if ((string)ViewState["sortDir"] == " ASC")
        {
            ViewState["sortDir"] = " DESC";
            D.DefaultView.Sort = e.SortExpression + (string)ViewState["sortDir"];
        }
        else
        {
            ViewState["sortDir"] = " ASC";
            D.DefaultView.Sort = e.SortExpression + (string)ViewState["sortDir"];
        }
        gvSortExpression = e.SortExpression;
        sg1.DataSource = D;
        sg1.DataBind();
    }
    protected void srch_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dt1 = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        string col1 = "";
        if (txtlblSubGroup.Text.Trim().Length > 1)
        {
            col1 = " and substr(trim(icode),1,4) in (" + txtlblSubGroup.Text + ")";
        }
        SQuery = "select trim(a.iname) as fstr,a.* from (select x.iname,x.icode, x.opening as opening , nvl(y.cdbts,0) as  Receipts, nvl(y.ccdts,0) as Issues ,x.opening+nvl(y.cdbts,0)-nvl(y.ccdts,0) as Closing,x.oprate1 as PSize,x.oprate2 as Pwidth,x.oprate3 as Pgsm,x.opening+nvl(y.cdbts,0)-nvl(y.ccdts,0) as xClosing,X.CPARTNO,x.maker as W_Used from (select a.icode,a.iname,a.maker,a.cpartno,a.opening_bal+nvl(b.newop,0) opening,a.oprate1,a.oprate2,a.oprate3 from (select substr(f.icode,1,2) as grp,f.icode ,f.maker,f.cpartno,F.oprate1,f.oprate2,f.oprate3,f.iname , sum(nvl(fb.op,0)) as opening_bal from item f left outer join  (Select icode,sum(YR_" + frm_myear + ") as op from itembal where branchcd ='" + frm_mbr + "' group by icode) fb on trim(f.icode)=trim(fb.icode) where (oprate1 between " + txtLenFrom.Text + " and " + txtLenTo.Text + ") and (oprate2 between " + txtWidthFrom.Text + " and " + txtWidthTo.Text + ") and (oprate3 between " + txtGSMFrom.Text + " and " + txtGSMTo.Text + ") group by substr(f.icode,1,2),f.icode,f.iname,f.maker,f.cpartno,F.oprate1,f.oprate2,f.oprate3 ) a left outer join (select v.icode,nvl(sum(v.iqtyin),0)-nvl(sum(v.iqtyout),0) newop from ivoucher v where v.store='Y' and v.branchcd ='" + frm_mbr + "' and v.vchdate between TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') and TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') -1 group by v.icode) b on trim(a.icode)=trim(b.icode) ) x left outer join (select v.icode,sum(v.iqtyin) cdbts,sum(v.iqtyout)ccdts from ivoucher v where v.store='Y'  and v.branchcd ='" + frm_mbr + "' and v.vchdate between TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') and TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY') group by v.icode) y on trim(x.icode)=trim(y.icode)) a where substr(icode,1,2) in ('01','02','06','07','08','09','70','80','81') " + col1 + " order by icode";
        {
            dt1 = new DataTable();
            dt1 = fgen.search_vip1(frm_qstr, frm_cocd, SQuery, txtsearch.Text.Trim().ToUpper(), dt);
        }
        ViewState["sg1"] = dt1;
        if (dt1 != null)
        {
            sg1.DataSource = dt1;
            sg1.DataBind(); dt1.Dispose();
            lblTotcount.Text = "Total Rows : " + sg1.Rows.Count;
        }
        else
        {
            sg1.DataSource = null;
            sg1.DataBind();
        }
    }
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0) e.Row.TabIndex = 1;
            else e.Row.TabIndex = 2;
            if (Convert.ToDouble(e.Row.RowIndex.ToString()) == 0) e.Row.Attributes["onfocus"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); }";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
}