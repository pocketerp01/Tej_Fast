using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;


public partial class om_prt_smktg : System.Web.UI.Page
{
	string val, value1, value2, value3, HCID, co_cd, SQuery, xprdrange, fromdt, todt, mbr, branch_Cd, xprd1, xprd2, xprd3, cond, CSR;
	string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, yr_fld, cDT1, cDT2, year, header_n, uname, ulvl, btfld, yr1, yr2;
	string tbl_flds, rep_flds, table1, table2, table3, table4, datefld, sortfld, joinfld;
	int i0, i1, i2, i3, i4; DateTime date1, date2; DataSet ds, ds3; DataTable dt, dt1, dt2, dt3, mdt, dticode, dticode2;
	double month, to_cons, itot_stk, itv; DataRow oporow, ROWICODE, ROWICODE2; DataView dv;
	string opbalyr, param, eff_Dt, xprdrange1, cldt = "";
	string er1, er2, er3, er4, er5, er6, er7, er8, er9, er10, er11, er12, frm_qstr, frm_formID;
	string ded1, ded2, ded3, ded4, ded5, ded6, ded7, ded8, ded9, ded10, ded11, ded12, col1;
	string frm_AssiID;
	string frm_UserID;
	fgenDB fgen = new fgenDB();

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
				CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
			}

			hfhcid.Value = frm_formID;

			if (!Page.IsPostBack)
			{
				col1 = fgen.seek_iname(frm_qstr, co_cd, "SELECT BRN||'~'||PRD AS PP FROM FIN_MSYS WHERE UPPER(TRIM(ID))='" + frm_formID + "'", "PP");
				if (col1.Length > 1)
				{
					hfaskBranch.Value = col1.Split('~')[0].ToString();
					hfaskPrdRange.Value = col1.Split('~')[1].ToString();
				}
				show_data();
			}
		}
	}

	public void show_data()
	{
		HCID = hfhcid.Value.Trim(); SQuery = ""; fgen.send_cookie("MPRN", "N");
		fgen.send_cookie("REPLY", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL6", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", "");
		fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", "");

		// asking for Branch Consolidate Popup
		if (hfaskBranch.Value == "Y")
		{ hfaskBranch.Value = "Y"; fgen.msg("-", "CMSG", "Do you want to see consolidate report'13'(No for branch wise)"); }
		else if (hfaskBranch.Value == "N" && hfaskPrdRange.Value == "Y") fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);//YE LINE MANE CMNT KI H FOR VIEW SALE PRINTOUT ONLY
		else
		{
			// else if we want to ask another query / another msg / date range etc.
			header_n = "";
			switch (HCID)
			{
				case "F47141":
				case "F47142":
					SQuery = "select trim(type1) as fstr,name,type1 as code from type where ID='V' AND type1 like '4%' AND TYPE1 NOT IN ('4F','47') ORDER BY code";
					header_n = "Select Sale Type";
					break;

				case "F47226":
				case "F47227":
					fgen.Fn_open_Act_itm_prd("-", frm_qstr);
					break;
				case "F47240":
					SQuery = "select TRIM(type1) as fstr,name,type1 as code from type where id='V' and type1 like '2%' ORDER BY code";
					header_n = "Select Type";
					fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "Y");
					break;

					//made and merged by yogita
				case "F47224"://sch status daily
				case "F47225": // sch status monthly
					fgen.Fn_open_Act_itm_prd("-", frm_qstr);
					break;
				
			}
			if (SQuery.Length > 1)
			{
				fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
				fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
				if (HCID == "F47240") fgen.Fn_open_sseek(header_n, frm_qstr);
				else fgen.Fn_open_mseek(header_n, frm_qstr);
			}
		}
	}

	protected void btnhideF_Click(object sender, EventArgs e)
	{
		val = hfhcid.Value.Trim();
		fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
		// if coming after SEEK popup
		if (fgenMV.Fn_Get_Mvar(frm_qstr, "ANP").ToString().Trim() == "Y")
		{
			value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
			col1 = value1;
			hfcode.Value = "";
			if (val == "F47240")
			{
				// bydefault it will ask for prdRange popup
				switch (val)
				{
					case "F47240":
						if (col1.Length < 2) return;
						hfcode.Value = col1;
						fgen.Fn_open_prddmp1("-", frm_qstr);
						break;
					default:
						hfcode.Value = value1;
						fgen.Fn_open_Act_itm_prd("-", frm_qstr);
						break;
				}
			}
		}
		else if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Length > 0)
		{
			value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
			hfcode.Value = "";
			hfcode.Value = value1;
			col1 = value1;
			if (val == "F47141" || val == "F47142")
			{
				// bydefault it will ask for prdRange popup
				hfcode.Value = value1;
				fgen.Fn_open_Act_itm_prd("-", frm_qstr);
			}
			else
			{
				switch (val)
				{
					case "F47240":
						fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
						fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hfcode.Value);
						fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
						fgen.fin_acct_reps(frm_qstr);
						break;
				}
			}
		}
		// else if branch selection box opens then it comes here
		else if (Request.Cookies["REPLY"].Value.Length > 0)
		{
			value1 = Request.Cookies["REPLY"].Value.ToString().Trim();
			switch (val)
			{
				default:
					// After Branch Consolidate Report  **************
					// it will ask prdDmp after branch code selection
					if (hfaskBranch.Value == "Y")
					{
						if (value1 == "Y") hfbr.Value = "ABR";
						else hfbr.Value = "";
						fgen.Fn_open_Act_itm_prd("-", frm_qstr);
					}
					break;
			}
		}
	}

	protected void btnhideF_s_Click(object sender, EventArgs e)
	{
		val = hfhcid.Value.Trim();
		//if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Length > 0 || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").Length > 0)
		{
			value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
			value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
			value3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");

			fromdt = value1;
			todt = value2;
			cldt = value3;

			cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
			cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

			xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
			xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
			xprd2 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + cDT2 + "','dd/mm/yyyy')";
			yr_fld = year;

			co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");


			if (hfbr.Value == "ABR") branch_Cd = "branchcd not in ('DD','88')";
			else branch_Cd = "branchcd='" + mbr + "'";

			string party_cd = "";
			string part_cd = "";
			party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
			part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
			if (party_cd.Trim().Length <= 1)
			{
				party_cd = "%";
			}
			if (part_cd.Trim().Length <= 1)
			{
				part_cd = "%";
			}

			// after prdDmp this will run            
			switch (val)
			{
				case "F47141":
					fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47141");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47142":
					fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl1", hfcode.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl2", party_cd);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COl3", part_cd);
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47142");
					fgen.fin_smktg_reps(frm_qstr);
					break;
				case "F47222":
					// ORDER Vs Dispatch              
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47222");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47223":
					// Schedule Vs Dispatch              
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47223");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47226": // 14 may 2018 BY MADHVI
					// Rate Trend Chart Product Wise             
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47226");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47227": // 14 may 2018 BY MADHVI
					// Rate Trend Chart Customer Wise             
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47227");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47228":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47228");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47229":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47229");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47230":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47230");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47231":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47231");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47232":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47232");
					fgen.fin_smktg_reps(frm_qstr);
					break;

				case "F47233":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F47233");
					fgen.fin_smktg_reps(frm_qstr);
					break;
				case "F47240":
					fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", hfcode.Value);
					SQuery = "SELECT DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) as fstr,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.aname as party,a.acode AS CODE,b.email FROM voucher a, famst b where trim(a.acode)=trim(b.acodE) and substr(a.acode,1,2) in ('05','06') and nvl(trim(b.email),'-')!='-' and a.branchcd='" + mbr + "' and a.vchdate " + xprdrange + " and a.type='" + hfcode.Value + "' ORDER BY b.aname";
					fgenMV.Fn_Set_Mvar(frm_qstr, "ANP", "N");
					fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
					fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
					fgen.Fn_open_mseek(header_n, frm_qstr);
					break;

				case "F47224":
				case "F47225":
					fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", val);
					fgen.fin_smktg_reps(frm_qstr);
					break;
			}
		}
	}
}