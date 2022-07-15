using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_corr_mis_rpt : System.Web.UI.Page
{
    string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, DateRange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
    string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld, WB_TABNAME;
    int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm, mdt, dticode, dticode2, dtdrsim, dtm1, dtraw;
    double month, to_cons, itot_stk, itv, d, iqtyout_sum; DataRow oporow, ROWICODE, ROWICODE2, dr2; DataView dv, view1im;
    string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
    string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
    double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0, db9 = 0, db10 = 0, db11 = 0, db12 = 0, db13 = 0, db14 = 0, db15 = 0, db16 = 0, db17 = 0, db18 = 0, db19 = 0, db20 = 0, db21 = 0, db22 = 0, db23 = 0, db24 = 0, db25 = 0, db26 = 0, db27 = 0, db28 = 0, db29 = 0, db30 = 0;
    string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1, r10 = "";
    int cnt = 0, cnt1 = 0;
    string frm_AssiID;
    string party_cd, part_cd;
    string frm_UserID, inspvchtab;
    fgenDB fgen = new fgenDB();
    DataSet dsRep;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
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
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
            }

            hfhcid.Value = frm_formID;
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    { }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    { }
    void open_prodpp(string buttonID)
    {
        Page p = (Page)HttpContext.Current.CurrentHandler;
        string fil_loc = ("../tej-prodpp-reps/om_view_prodpp.aspx");
        Session["mymst"] = "Y";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + frm_qstr + "@" + buttonID + "','98%','98%','" + "" + "');", true);
    }

    protected void Button19_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40342");
    }
    protected void Button7_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40338");
    }
    protected void Button8_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40337");
    }
    protected void Button6_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40339");
    }
    protected void Button3_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40340");
    }
    protected void Button2_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40341");
    }
    protected void Button1_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40343");
    }
    protected void Button11_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40138");
    }
    protected void Button16_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40137");
    }
    protected void Button12_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40130");
    }
    protected void Button13_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40136");
    }
    protected void Button15_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40128");
    }
    protected void Button4_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40352");
    }
    protected void Button10_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40129");
    }
    protected void Button9_ServerClick(object sender, EventArgs e)
    {
        open_prodpp("F40355");
    }
    protected void btnExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
}