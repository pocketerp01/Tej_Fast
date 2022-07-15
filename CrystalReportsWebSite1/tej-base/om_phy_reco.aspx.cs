using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OleDb;


public partial class om_phy_reco : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    int kclreelno = 0;
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
    string ext, filesavepath, filename, excelConString;
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
                btnRead.Visible = false;
                //chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0001'", "fstr");
                //if (chk_opt != "Y")
                //{
                //    tab3.Visible = false;
                //    btnPost.Visible = false;
                //}
                //chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0002'", "fstr");
                //if (chk_opt != "Y")
                //{
                //    txtBarCode.Visible = false;
                //    btnRead.Visible = false;
                //}

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
    public static string path = "";
    public static string excelConnectionString = "";
    //------------------------------------------------------------------------------------
    protected void btnupload_Click(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "";
        string excelConString = "";
        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext == ".xls")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                FileUpload1.SaveAs(filesavepath);
                //excelConString = fgen.connStringexcel(filesavepath);
                //excelConString = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + filesavepath + ";Extended Properties=\"Excel 16.0 Xml; HDR = True; IMEX = 0";
                //excelConString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filesavepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";//working
                //excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + "; Extended Properties=Excel 8.0 Xml;HDR=YES;";
                //excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0; HDR = Yes; IMEX = 1\";";
                excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source = " + filesavepath + ";Extended Properties = \"Excel 8.0; HDR = YES; \"";
            }
            else
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                return;
            }
            try
            {
                //string mstr = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source='" + filesavepath + "';Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
                OleDbConn.Open();
                DataTable dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                OleDbConn.Close();
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                OleDbCommand OleDbCmd = new OleDbCommand();
                String Query = "";
                Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                OleDbCmd.CommandText = Query;
                OleDbCmd.Connection = OleDbConn;
                OleDbCmd.CommandTimeout = 0;
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = OleDbCmd;
                objAdapter.SelectCommand.CommandTimeout = 0;
                dt = null;
                dt = new DataTable();
                objAdapter.Fill(dt);
                DataView dv = dt.DefaultView;
                dv.Sort = "Item_Code";
                dt = dv.ToTable();
                Random generator = new Random();
                if (dt.Rows.Count > 0)
                {
                    if (ViewState["sg1"] != null)
                    {
                        DataTable datatdt = new DataTable();
                        sg1_dt = new DataTable();
                        datatdt = (DataTable)ViewState["sg1"];
                        sg1_dt = datatdt.Clone();
                        sg1_dr = null;
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            if (fgen.make_double(dt.Rows[d]["Qty_Physical"].ToString().Trim()) > 0)
                            {
                                string reelno = "", oldreelno = "";

                                int reelnumlen = 6;
                                if (frm_cocd == "KPPL") reelnumlen = 10;
                                reelno = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT KCLREELNO AS VCH,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM REELVCH WHERE " +
                                    "BRANCHCD='" + frm_mbr + "' AND TYPE like '0%' AND VCHDATE " + DateRange + " ORDER BY VDD DESC ", "VCH");
                                oldreelno = reelno;
                                if (ViewState["kclreelno"] != null)
                                    kclreelno = (int)ViewState["kclreelno"];
                                {
                                    sg1_dr = sg1_dt.NewRow();
                                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                                    sg1_dr["sg1_h1"] = dt.Rows[d]["Item_Code"].ToString().Trim().PadLeft(8, '0');
                                    sg1_dr["sg1_h2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                                    sg1_dr["sg1_h3"] = "-";
                                    sg1_dr["sg1_h4"] = "-";
                                    sg1_dr["sg1_h5"] = "-";
                                    sg1_dr["sg1_h6"] = "-";
                                    sg1_dr["sg1_h7"] = "-";
                                    sg1_dr["sg1_h8"] = "-";
                                    sg1_dr["sg1_h9"] = "-";
                                    sg1_dr["sg1_h10"] = "-";
                                    if (kclreelno == 0) kclreelno = 1;
                                    reelno = (reelno.toDouble() + kclreelno).ToString().PadLeft(reelnumlen, '0');

                                    sg1_dr["sg1_f1"] = dt.Rows[d]["Item_Code"].ToString().Trim().PadLeft(8, '0');
                                    sg1_dr["sg1_f2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = dt.Rows[d]["Part_No"].ToString().Trim();
                                    sg1_dr["sg1_f4"] = dt.Rows[d]["Stock_Bal"].ToString().Trim();
                                    sg1_dr["sg1_f5"] = dt.Rows[d]["Unit"].ToString().Trim();
                                    sg1_dr["sg1_t1"] = dt.Rows[d]["Qty_System"].ToString().Trim();
                                    sg1_dr["sg1_t2"] = dt.Rows[d]["Qty_Physical"].ToString().Trim();
                                    sg1_dr["sg1_t3"] = reelno;
                                    sg1_dr["sg1_t4"] = dt.Rows[d]["Supplier_Batch_No"].ToString().Trim();
                                    sg1_dr["sg1_t5"] = dt.Rows[d]["Rate"].ToString().Trim();
                                    sg1_dt.Rows.Add(sg1_dr);
                                    kclreelno++;
                                    ViewState["kclreelno"] = kclreelno;
                                }
                            }
                        }
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        setColHeadings();
                    }
                }
            }
            catch (Exception ex)
            {
                fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In File Uploading");
                fgen.msg("-", "AMSG", "Please Select Excel File only in .xls format!! Or subgroup should be in number only.");
            }
        }
    }

    protected void btnhelp_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        DataTable dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT 'SRNO' AS SRNO,'MAINGP' AS MAINGP,'SUBGP' AS SUBGP," +
            "'ITEM_NAME' AS ITEM_NAME,'HSCODE' AS HSCODE,'PART_NO' AS PART_NO ,'DRG_NO' AS DRG_NO," +
            "'ITEM_NAME_CUST' AS ITEM_NAME_CUST,'PRIMARY_UNIT' AS PRIMARY_UNIT,'SECONDARY_UNIT' AS SECONDARY_UNIT" +
            ",'STD_RATE' AS STD_RATE,'A_B_CCLASS' AS A_B_CCLASS,'LOCN' AS LOCN,'CAT' AS CAT,'GROSS_WT' AS  GROSS_WT" +
            ",'NET_WT' AS NET_WT,'CRITICAL_ITEM' AS CRITICAL_ITEM,'BRAND_OR_REF' AS BRAND_OR_REF,'STANDARD_PACKING' " +
            "AS  STANDARD_PACKING,'SHELF_LIFE_DAYS' AS SHELF_LIFE_DAYS, 'LABR_CHG' AS LABR_CHG, 'ERP_CODE' AS ERP_CODE FROM DUAL");

        if (dt1.Rows.Count > 0)
        {
            Session["send_dt"] = dt1;
            fgen.Fn_open_rptlevel("Download The Excel Format and don't change the columns positions", frm_qstr);
        }
    }

    protected void btnhelp2_ServerClick(object sender, EventArgs e)
    {
        if (txtlbl7.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Mai Group Before export !!");
            return;
        }
        if (ViewState["sg1"] != null)
        {
            sg1_dt = new DataTable();
            DataTable dtprint = new DataTable();
            dtprint = fgen.getdata(frm_qstr, frm_cocd, "select '-' as Srno,'-' as Item_Code,'-' as Item_Name,'-' as Part_No" +
                ",'-' as Stock_Bal,'-' as Unit,'-' as Qty_System,'-' as Qty_Physical,'-' as Our_Batch_No" +
                ",'-' as Supplier_Batch_No,'-' as Rate  FROM DUAL");
            foreach (DataColumn dc in dtprint.Columns)
            {
                dc.MaxLength = 500;
            }
            dtprint.AcceptChanges();
            dtprint = dtprint.Clone();

            dt = (DataTable)ViewState["sg1"];
            z = dt.Rows.Count - 1;
            sg1_dt = dt.Clone();
            sg1_dr = null;
            for (i = 0; i < dt.Rows.Count - 1; i++)
            {
                DataRow drr = dtprint.NewRow();
                drr["Srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                drr["Item_Code"] = dt.Rows[i]["sg1_f1"].ToString();
                drr["Item_Name"] = dt.Rows[i]["sg1_f2"].ToString();
                drr["Part_No"] = dt.Rows[i]["sg1_f3"].ToString();
                drr["Stock_Bal"] = dt.Rows[i]["sg1_f4"].ToString();
                drr["Unit"] = dt.Rows[i]["sg1_f5"].ToString();
                drr["Qty_System"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                drr["Qty_Physical"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                drr["Our_Batch_No"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                drr["Supplier_Batch_No"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                drr["Rate"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                dtprint.Rows.Add(drr);
            }
            if (dtprint.Rows.Count > 0) fgen.exp_to_excel(dtprint, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
            else fgen.msg("-", "AMSG", "No Data to Export"); dtprint.Dispose();
        }
        else fgen.msg("-", "AMSG", "No Data to Export");
    }
    protected void btnhelp3_ServerClick(object sender, EventArgs e)
    {
        if (txtlbl7.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Mai Group Before export !!");
            return;
        }
        //if (ViewState["sg1"] != null)
        //{
        //    sg1_dt = new DataTable();
        //    DataTable dtprint = new DataTable();
        //    dtprint = fgen.getdata(frm_qstr, frm_cocd, "select '-' as Srno,'-' as Item_Code,'-' as Item_Name,'-' as Part_No" +
        //        ",'-' as Stock_Bal,'-' as Unit,'-' as Qty_System,'-' as Qty_Physical,'-' as Our_Batch_No" +
        //        ",'-' as Supplier_Batch_No,'-' as Rate  FROM DUAL");
        //    foreach (DataColumn dc in dtprint.Columns)
        //    {
        //        dc.MaxLength = 500;
        //    }
        //    dtprint.AcceptChanges();
        //    dtprint = dtprint.Clone();

        #region Reel view
        string typstring = txtlbl7.Text.Trim();
        string icodecond = "and substr(icode,1,2) in (" + typstring + ") ";
        string reel_V_tbl = "reelvch";
        string mq0, mq1, mq2;
        string xprd1 = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
        string xprd2 = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
        mq0 = "select icode,REELNO,coreelno,count(reelno) as reels,sum(closing) as Closing from (select trim(a.kclreelno) as Reelno,trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE,COREELNO from (Select '-' as kclreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,COREELNO from REELVCH_OP where branchcd='" + frm_mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,7)='REELOP*' and 1=2 union all  ";
        mq1 = "select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE,COREELNO from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE,COREELNO union all ";
        mq2 = "select kclreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE,COREELNO from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE,COREELNO )a group by trim(a.kclreelno),trim(a.icode),COREELNO having sum(opening)+sum(cdr)-sum(ccr)>0)group by Icode,reelno,coreelno ";
        SQuery = "create or replace view REEL_NOS_" + frm_mbr + " as (SELECT * FROM (" + mq0 + mq1 + mq2 + "))";

        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
        #endregion

        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT b.icode as Item_Code,B.INAME as Item_Name,B.CPARTNO as Part_No,'0' as Stock_Bal,B.Unit" +
            ",NVL(a.closing,0) as Qty_System,NVL(a.closing,0) as Qty_Physical,NVL(a.reelno,'0') as Our_Batch_No,NVL(a.COREELNO,'-') as Supplier_Batch_No,'0' as Rate FROM ITEM B left join " +
            "REEL_NOS_" + frm_mbr + " A ON TRIM(A.ICODE)=TRIM(B.ICODE) WHERE substr(B.icode,1,2)='" + typstring + "' ORDER BY A.ICODE ");

        if (dt.Rows.Count > 0)
        {

            //if (dt.Rows.Count > 0)
            //{
            //    string fname = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm").Trim() + ".csv";
            //    string zipFilePath = "c:\\TEJ_erp\\Upload\\" + fname;
            //    fgen.CreateCSVFile(dt, zipFilePath, ",");
            //    Session["FilePath"] = Session["FileName"] = fname;
            //    Response.Write("<script>");
            //    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            //    Response.Write("</script>");

            //}
            //else fgen.msg("-", "AMSG", "No Data to Export");
            //dt.Dispose();

            fgen.exp_to_excel(dt, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm").Trim());
        }
        else { fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose(); }

        //sg1_dt = dt.Clone();
        //sg1_dr = null;

        //for (i = 0; i < dt.Rows.Count - 1; i++)
        //{
        //    DataRow drr = dtprint.NewRow();
        //    drr["Srno"] = i + 1;
        //    drr["Item_Code"] = dt.Rows[i]["item_code"].ToString();
        //    drr["Item_Name"] = dt.Rows[i]["iname"].ToString();
        //    drr["Part_No"] = dt.Rows[i]["cpartno"].ToString();
        //    drr["Stock_Bal"] = "-";
        //    drr["Unit"] = dt.Rows[i]["unit"].ToString();
        //    drr["Qty_System"] = ((TextBox)sg1.Rows[i].FindControl("Closing")).Text.Trim();
        //    drr["Qty_Physical"] = ((TextBox)sg1.Rows[i].FindControl("Closing")).Text.Trim();
        //    drr["Our_Batch_No"] = ((TextBox)sg1.Rows[i].FindControl("REELNO")).Text.Trim();
        //    drr["Supplier_Batch_No"] = ((TextBox)sg1.Rows[i].FindControl("COREELNO")).Text.Trim();
        //    drr["Rate"] = "0";
        //    dtprint.Rows.Add(drr);
        //}

        //}
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
        switch (Prg_Id)
        {
            case "F39201":
            case "F39206":
            case "F39211":
            case "F25111":
            case "F40201":
            case "F75115":
            case "F25115":
                tab2.Visible = false;
                //tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                break;
        }
        if (Prg_Id == "M12008")
        {
            tab5.Visible = true;
            txtlbl8.Attributes.Remove("readonly");
            txtlbl9.Attributes.Remove("readonly");
        }

        if (Prg_Id == "F35014")
        {
            txtlbl8.Attributes.Remove("readonly");
            txtlbl9.Attributes.Remove("readonly");
        }

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

        frm_tabname = "WIPSTK";
        //btnlist.Visible = false;//old
        if (frm_formID == "F25402") { btnlist.Visible = true; btnprint.Visible = false; }
        else { btnlist.Visible = false; btnprint.Visible = false; }

        frm_vty = "RL";
        lbl1a.Text = "RL";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        FileUpload2.Visible = false;
        Button2.Visible = false;
        Button3.Visible = false;
        Button4.Visible = false;

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
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as Address,'-' as City from dual union all select 'PARTY VEHICLE' as Transporter,'-' as Code,'-' as Address,'-' as City from dual) order by  Transporter";
                break;
            case "BTN_17":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='Y' order by name";
                break;
            case "BTN_18":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='<' order by name";
                break;

            case "BTN_19":
                SQuery = "SELECT '10' as fstr,'As Applicable' as NAME,'10' as Code FROM dual ";
                break;

            case "BTN_20":

                break;
            case "BTN_21":

                break;
            case "BTN_22":

                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                //pop1
                //if (lbl1a.Text == "30" && frm_cocd == "AGRM")

                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='M' AND SUBSTR(TYPE1,1,1) IN ('6','7') order by TYPE1 ";

                break;
            case "TICODE":
                //pop2

                SQuery = "select Acref,name as Wip_Stage ,Acref as Stg_code,Type1 as Srno from typegrp where branchcd='" + frm_mbr + "' and id='WI' order by Acref";

                switch (Prg_Id)
                {
                    case "F35014":
                        SQuery = "SELECT a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.ent_by from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and substr(a.icode,1,1)<'2' and substr(a.icode,1,2) in ('07','08','09') order by a.Iname ";
                        break;
                }

                SQuery = "Select Type1 as fstr,Name,type1 from type where id='Y' order by type1";
                break;
            case "TICODEX":
                SQuery = "select type1,name as State ,type1 as code from type where id='{' order by Name";
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

                //if (col1.Length <= 0) 
                col1 = "'-'";

                SQuery = "SELECT a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.ent_by from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") order by a.Iname ";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_JOB":
                SQuery = "select * from (Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + pastcurrprd + " and a.srno=1 order by a.vchdate desc,a.vchnum desc) where rownum<100";
                SQuery = "SELECT NAME AS FSTR,NAME AS LOCATION,TYPE1 AS CODE FROM TYPEGRP WHERE ID='BN' ORDER BY TYPE1";
                break;
            case "SG1_ROW_BTCH":
                SQuery = "select * from (Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + DateRange + " and a.srno=1 order by a.vchdate desc,a.vchnum desc) where rownum<100";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";
                break;
        }
        if (btnval == "Edit" || btnval == "Del" | btnval == "Print") // if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print"))
        {
            btnval = btnval + "_E";
            hffield.Value = btnval;
            make_qry_4_popup();
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
            // frm_vty = "RP"; //OLD
            frm_vty = "RL";
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);

            sg1_dt = new DataTable();
            create_tab();
            sg1_add_blankrows();


            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            setColHeadings();
            ViewState["sg1"] = sg1_dt;

            col1 = "";

            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (Prg_Id == "F39211")
            {

            }
            else
            {
                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT Type1||'~'||NAME AS Deptt,Type1 AS CODE FROM type where id='M' and trim(Type1) in (select trim(erpdeptt) as fstr from EVAS WHERE USERNAME='" + frm_uname + "' ) ", "Deptt");
                if (col1.Length > 5)
                {
                    txtlbl4.Text = col1.Split('~')[0];
                    txtlbl4a.Text = col1.Split('~')[1];
                }
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "";
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
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
                //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();

                sg1_dt.Rows.Add(sg1_dr);
            }
            sg1_add_blankrows();
            ViewState["sg1"] = sg1_dt;
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
    string checkGridQty()
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + txtlbl2.Text + "-" + txtlbl3.Text;
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;

        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");


            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||vchnum||'-'||to_ChaR(vchdate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,iqty_chl as Qtyord,0 as Soldqty,1 as prate from ivoucherp where branchcd='" + frm_mbr + "' and type like '00%'  and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' union all SELECT trim(icode)||'-'||genum||'-'||to_ChaR(gedate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='0%' and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)", "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
                break;
            }
        }
        return null;
    }

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
        foreach (GridViewRow gr in sg2.Rows)
        {
            if (gr.Cells[3].Text.ToString().Trim().Length > 4 && fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim()) > 0)
            {
                if (col1 != gr.Cells[3].Text.ToString().Trim()) i = 1;
                drQty1 = dtQty1.NewRow();
                drQty1["icode"] = gr.Cells[3].Text.ToString().Trim();
                col1 = gr.Cells[3].Text.ToString().Trim();
                drQty1["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty1["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim());
                drQty1["rcount"] = i;
                dtQty1.Rows.Add(drQty1);
                i++;
            }
        }

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

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from IVOUCHER where branchcd||TRIM(STYLENO)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from REELVCH where branchcd||trim(job_no)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from UDF_DATA a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    txtlbl2.Text = frm_uname;


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
                    fgen.Fn_open_sseek("Select Entry to see list", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_char(A.ent_Dt,'dd/mm/yyyy') as entdtd,nvl(b.Iname,'-') As Iname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') As Icdrgno,nvl(b.unit,'-') as IUnit from " + frm_tabname + " a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.srno";
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["entdtd"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["maincode"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='M' AND TYPE1='" + txtlbl4.Text + "' ", "name");

                        //txtlbl5.Text = dt.Rows[i]["jobno"].ToString().Trim();
                        //txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["jobdt"].ToString().Trim()).ToString("dd/MM/yyyy");

                        //txtlbl9.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["refdate"].ToString().Trim(), vardate)).ToString("dd/MM/yyyy");

                        //txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim(); //OILD
                        txtrmk.Text = dt.Rows[i]["REMARKS"].ToString().Trim();


                        if (Prg_Id == "F35014")
                        {
                            txtlbl8.Text = dt.Rows[i]["rc_qty"].ToString().Trim();
                            txtlbl7.Text = dt.Rows[i]["rcode"].ToString().Trim();
                            txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT INAME FROM ITEM WHERE trim(ICODE)='" + txtlbl7.Text.Trim() + "'", "INAME");
                        }
                        else
                        {
                            txtlbl7.Text = dt.Rows[i]["stage"].ToString().Trim();
                            //txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT name FROM type WHERE id='1' and trim(type1)='" + txtlbl7.Text.Trim() + "'", "name"); //OLD
                            txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT name FROM type WHERE id='Y' and trim(type1)='" + txtlbl7.Text.Trim() + "'", "name");
                        }

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


                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[i]["Icdrgno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["Icdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["IUnit"].ToString().Trim();
                            sg1_dr["sg1_t1"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_t2"] = dt.Rows[i]["ngqty"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["wolink"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["work_ref"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["BOMQ"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["loc_ref"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["remarks"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------

                        // REEL TABLE
                        SQuery = "SELECT A.*,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        i = 1;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                sg2_dr = sg2_dt.NewRow();

                                sg2_dr["sg2_srno"] = i;
                                sg2_dr["sg2_h1"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_h2"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_h3"] = "";
                                sg2_dr["sg2_h4"] = "";
                                sg2_dr["sg2_h5"] = "";

                                sg2_dr["sg2_f1"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_f2"] = dr["iname"].ToString().Trim();
                                sg2_dr["sg2_f3"] = "";
                                sg2_dr["sg2_f4"] = "";
                                sg2_dr["sg2_f5"] = "";

                                sg2_dr["sg2_t1"] = dr["kclreelno"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dr["psize"].ToString().Trim();
                                sg2_dr["sg2_t3"] = dr["gsm"].ToString().Trim();
                                sg2_dr["sg2_t4"] = dr["reelwin"].ToString().Trim();
                                sg2_dr["sg2_t5"] = dr["irate"].ToString().Trim();
                                sg2_dr["sg2_t6"] = dr["coreelno"].ToString().Trim();
                                sg2_dr["sg2_t7"] = dr["reelspec1"].ToString().Trim();
                                sg2_dr["sg2_t8"] = dr["reelspec2"].ToString().Trim();
                                sg2_dr["sg2_t9"] = i.ToString(); ;
                                sg2_dr["sg2_t10"] = "";

                                sg2_dt.Rows.Add(sg2_dr);
                                i++;
                            }
                        }
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose();
                        sg2_dt.Dispose();

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
                    SQuery = "Select a.vchnum as entryno,to_Char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.maincode as code,c.name as deptt,a.srno,a.icode as erpcode,b.iname as product,b.cpartno,b.unit,a.iqtyin as store_qty,a.NGQTY as phy_qty,A.BOMQ AS RATE,a.remarks,to_char(A.ent_Dt,'dd/mm/yyyy') as entdtd from " + frm_tabname + " a left outer join type c ON c.id='M' AND TRIM(a.MAINCODE)=TRIM(C.TYPE1),item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.srno";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "TACODE":
                    //-----------------------------
                    if (col1.Length <= 0) return;
                    string mind_type = "";
                    string plan_base = "Y";
                    mind_type = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                    if (mind_type == "05x" || mind_type == "06")
                    {
                        plan_base = "N";
                    }

                    //if (lbl1a.Text == "30" && frm_cocd == "AGRM")
                    if (lbl1a.Text == "30" && plan_base == "Y")
                    {
                        SQuery = "select to_Char(a.vchdate,'yyyymmdd')||a.vchnum as Fstr,a.vchnum as Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Plan_dt,trim(A.acode) as Stg_cd,trim(A.icode) as ERP_Code,sum(a.plan_qty)-sum(a.issued) as Pending_Qty  from (SELECT acode,vchnum,vchdate,a1 as plan_Qty,0 as issued,icode from prod_Sheet where branchcd='" + frm_mbr + "' and type='11' and vchdate " + DateRange + "  union all SELECT stage,jobno,jobdt,0 as iqtychl,req_qty as issued,icode from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a where to_Char(a.vchdate,'yyyymmdd')||a.vchnum ='" + col1 + "' group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,trim(A.icode),trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate having sum(a.plan_qty)-sum(a.issued)>0 order by fstr";
                        SQuery = "select a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,c.Iname,c.cdrgno,c.cpartno,c.unit,b.Ibcode as ERP_Code,to_char(round(sum(a.Pending_Qty*(b.ibqty/(Case when nvl(b.main_issue_no,0)=0 then 1 else b.main_issue_no end))),3),'9999999.999') as reqd_qty from (" + SQuery + ") a, itemosp b,item c where trim(A.erp_Code)=trim(b.icode) and trim(b.ibcode)=trim(c.icode) group by c.Iname,c.cdrgno,c.cpartno,c.unit,a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,b.Ibcode order by reqd_qty desc ";
                        // hard code
                        if (mind_type == "01")
                        {
                            SQuery = "select to_Char(a.vchdate,'yyyymmdd')||a.vchnum as Fstr,a.vchnum as Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Plan_dt,trim(A.acode) as Stg_cd,trim(A.icode) as ERP_Code,sum(a.plan_qty)-sum(a.issued) as Pending_Qty,sum(a.plan_qty)-sum(a.issued) as reqd_qty,b.iname,b.cpartno,b.unit  from (SELECT acode,vchnum,vchdate,a1 as plan_Qty,0 as issued,icode from prod_Sheet where branchcd='" + frm_mbr + "' and type='11' and vchdate " + DateRange + "  union all SELECT stage,jobno,jobdt,0 as iqtychl,req_qty as issued,icode from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a,item b where trim(a.icode)=trim(b.icode) and to_Char(a.vchdate,'yyyymmdd')||a.vchnum ='" + col1 + "' group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,trim(A.icode),trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate,b.iname,b.cpartno,b.unit having sum(a.plan_qty)-sum(a.issued)>0 order by fstr";
                        }
                        if (mind_type == "11")
                        {
                            SQuery = "select to_Char(a.vchdate,'yyyymmdd')||a.vchnum as Fstr,a.vchnum as Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Plan_dt,trim(A.acode) as Stg_cd,trim(A.icode) as ERP_Code,sum(a.plan_qty)-sum(a.issued) as Pending_Qty  from (SELECT acode,vchnum,vchdate,a1 as plan_Qty,0 as issued,icode from prod_Sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + "  union all SELECT stage,jobno,jobdt,0 as iqtychl,req_qty as issued,icode from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a where to_Char(a.vchdate,'yyyymmdd')||a.vchnum ='" + col1 + "' group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,trim(A.icode),trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate having sum(a.plan_qty)-sum(a.issued)>0 order by fstr";
                            SQuery = "select a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,c.Iname,c.cdrgno,c.cpartno,c.unit,b.Ibcode as ERP_Code,to_char(round(sum(a.Pending_Qty*(b.ibqty/(Case when nvl(b.main_issue_no,0)=0 then 1 else b.main_issue_no end))),3),'9999999.999') as reqd_qty from (" + SQuery + ") a, itemosp b,item c where trim(A.erp_Code)=trim(b.icode) and trim(b.ibcode)=trim(c.icode) group by c.Iname,c.cdrgno,c.cpartno,c.unit,a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,b.Ibcode order by reqd_qty desc ";
                        }
                        if (mind_type == "05")
                        {
                            SQuery = "select to_Char(a.vchdate,'yyyymmdd')||a.vchnum as Fstr,a.vchnum as Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Plan_dt,trim(A.acode) as Stg_cd,trim(A.icode) as ERP_Code,sum(a.plan_qty)-sum(a.issued) as Pending_Qty  from (SELECT '-' as acode,vchnum,vchdate,qty as plan_Qty,0 as issued,icode from costestimate where branchcd='" + frm_mbr + "' and type='30' and vchdate " + DateRange + " and length(Trim(nvl(col9,'-')))>=8 union all SELECT '-' as stage,jobno,jobdt,0 as iqtychl,req_qty as issued,icode from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + ")a where to_Char(a.vchdate,'yyyymmdd')||a.vchnum ='" + col1 + "' group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,trim(A.icode),trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate having sum(a.plan_qty)-sum(a.issued)>0 order by fstr";
                            SQuery = "select a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,c.Iname,c.cdrgno,c.cpartno,c.unit,b.Ibcode as ERP_Code,to_char(round(sum(a.Pending_Qty*(b.ibqty/(Case when nvl(b.main_issue_no,0)=0 then 1 else b.main_issue_no end))),3),'9999999.999') as reqd_qty from (" + SQuery + ") a, itemosp b,item c where trim(A.erp_Code)=trim(b.icode) and trim(b.ibcode)=trim(c.icode) group by c.Iname,c.cdrgno,c.cpartno,c.unit,a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,b.Ibcode order by reqd_qty desc ";

                            SQuery = "select a.Fstr,a.Plan_no,a.Plan_dt,a.Stg_cd,c.Iname,c.cdrgno,c.cpartno,c.unit,a.ERP_Code,a.Pending_qty as reqd_qty from (select to_Char(a.vchdate,'yyyymmdd')||a.vchnum as Fstr,a.vchnum as Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Plan_dt,trim(A.acode) as Stg_cd,trim(A.icode) as ERP_Code,sum(a.plan_qty)-sum(a.issued) as Pending_Qty  from (SELECT '-' as acode,vchnum,vchdate,is_number(col7) as plan_Qty,0 as issued,col9 as icode from costestimate where branchcd='" + frm_mbr + "' and type='30' and vchdate " + DateRange + " and length(Trim(nvl(col9,'-')))>=8 union all SELECT '-' as stage,jobno,jobdt,0 as iqtychl,req_qty as issued,icode from wb_iss_req where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + ")a where to_Char(a.vchdate,'yyyymmdd')||a.vchnum ='" + col1 + "' group by to_Char(a.vchdate,'yyyymmdd')||trim(a.acode)||a.vchnum,trim(A.icode),trim(a.acode),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),a.vchdate having sum(a.plan_qty)-sum(a.issued)>0 order by fstr) a, item c where trim(A.erp_Code)=trim(c.icode) order by reqd_qty desc ";
                        }
                    }
                    else
                    {
                        SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='M' AND SUBSTR(TYPE1,1,1) IN ('6','7') order by TYPE1 ";
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0 && plan_base == "Y" && lbl1a.Text.Substring(0, 1) != "2")
                    {

                        txtlbl4.Text = col1;
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");

                        if (frm_vty.Trim() != "30") return;

                        txtlbl4.Text = "60";
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");

                        txtlbl5.Text = dt.Rows[i]["plan_no"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["plan_Dt"].ToString().Trim();

                        txtrmk.Text = "Plan No." + dt.Rows[i]["plan_no"].ToString().Trim() + " Dt. " + dt.Rows[i]["plan_Dt"].ToString().Trim();


                        txtlbl7.Text = dt.Rows[i]["stg_Cd"].ToString().Trim();
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
                            sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_t1"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["erp_code"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();
                            //sg1_dr["sg1_t1"] = dt.Rows[i]["reqd_qty"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "";
                            if (plan_base == "Y")
                            {
                                sg1_dr["sg1_t8"] = dt.Rows[i]["plan_no"].ToString().Trim();
                                sg1_dr["sg1_t9"] = dt.Rows[i]["plan_dt"].ToString().Trim();
                            }

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
                    else
                    {
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        if (Prg_Id == "F39211")
                        {
                            txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select Aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl4.Text + "'))", "Aname");
                        }
                        else
                        {
                            txtlbl4.Text = col1;
                            txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");
                        }
                    }
                    break;


                    //-----------------------------



                    btnlbl7.Focus();
                    break;
                //-----------------------------



                case "BTN_10":
                    if (col1.Length <= 0) return;
                    txtlbl10.Text = col2;
                    btnlbl11.Focus();
                    break;
                case "BTN_11":
                    if (col1.Length <= 0) return;
                    txtlbl11.Text = col2;
                    btnlbl12.Focus();
                    break;
                case "BTN_12":
                    if (col1.Length <= 0) return;
                    txtlbl12.Text = col2;
                    btnlbl13.Focus();
                    break;
                case "BTN_13":
                    if (col1.Length <= 0) return;
                    txtlbl13.Text = col2;
                    btnlbl14.Focus();
                    break;
                case "BTN_14":
                    if (col1.Length <= 0) return;
                    txtlbl14.Text = col2;
                    break;
                case "BTN_15":

                    break;
                case "BTN_16":

                    break;
                case "BTN_17":


                    fillReelStock();
                    break;
                case "BTN_18":

                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    fillReelStock();
                    break;
                case "TICODEX":
                    if (col1.Length <= 0) return;
                    //txtlbl70.Text = col1;
                    //txtlbl71.Text = col2;
                    txtlbl2.Focus();
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
                            //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();


                        String pop_qry;

                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        if (col1.Trim().Length < 8) SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7 from item a where trim(a.icode) in (" + col1 + ") order by a.iname";
                        else SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7 from item a where trim(a.icode) in (" + col1 + ") order by a.iname";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();
                            sg1_dr["sg1_t1"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            //sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";



                            //if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                            //{
                            //    sg1_dr["sg1_t7"] = dt.Rows[d]["num4"].ToString().Trim();
                            //    sg1_dr["sg1_t8"] = dt.Rows[d]["num5"].ToString().Trim();
                            //}
                            //else
                            //{
                            //    sg1_dr["sg1_t7"] = dt.Rows[d]["num6"].ToString().Trim();
                            //    sg1_dr["sg1_t8"] = "0";
                            //}

                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "-";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";

                            //string mpo_Dt;
                            //mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                            //sg1_dr["sg1_t14"] = mpo_Dt;
                            //sg1_dr["sg1_t15"] = "";
                            //mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                            //sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);


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

                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_h1"] = col1;
                            sg2_dr["sg2_h2"] = col2;
                            sg2_dr["sg2_h3"] = "-";
                            sg2_dr["sg2_h4"] = "-";
                            sg2_dr["sg2_h5"] = "-";

                            sg2_dr["sg2_f1"] = col1;
                            sg2_dr["sg2_f2"] = col2;
                            sg2_dr["sg2_f3"] = "-";
                            sg2_dr["sg2_f4"] = "-";
                            sg2_dr["sg2_f5"] = "-";

                            sg2_dr["sg2_t1"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t4"] = "0";
                            sg2_dr["sg2_t5"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");

                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
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
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, col1, txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");

                    setColHeadings();
                    break;
                case "SG2_ROW_JOB":
                    if (col1.Length <= 0) return;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t6")).Text = col2;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t7")).Text = col3;
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
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text = col1;
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

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();


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
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();

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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string m_vty = "";
            switch (Prg_Id)
            {
                case "F39201":
                case "F40201":
                    m_vty = "3";
                    break;
                case "F39206":
                case "F40206":
                    m_vty = "1";
                    break;
                case "F39211":
                case "F40211":
                    m_vty = "2";
                    break;
            }
            if (m_vty == "2")
            {
                SQuery = "Select a.Type,a.Vchnum as Req_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,c.Aname as Supplier,b.Iname as Item_Name,b.cpartno as Part_No,a.Req_qty ,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt from " + frm_tabname + " a, item b,famst c where a.branchcd='" + frm_mbr + "' and a.type like '" + m_vty + "%' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.morder ";
            }
            else
            {
                //SQuery = "Select a.Type,a.Vchnum as Req_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,c.name as Deptt,b.Iname as Item_Name,b.cpartno as Part_No,a.Req_qty ,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt from " + frm_tabname + " a, item b,type c where c.id='M' and a.branchcd='" + frm_mbr + "' and a.type like '" + m_vty + "%' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.type1) order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.morder ";
                SQuery = "Select a.vchnum as entryno,to_Char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.maincode as code,c.name as deptt,a.srno,a.icode as erpcode,b.iname as product,b.cpartno,b.unit,A.WOLINK AS REELNO,a.iqtyin as store_qty,a.NGQTY as phy_qty,A.BOMQ AS RATE,a.remarks,to_char(A.ent_Dt,'dd/mm/yyyy') as entdtd from " + frm_tabname + " a left outer join type c ON c.id='M' AND TRIM(a.MAINCODE)=TRIM(C.TYPE1),item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a." + doc_df.Value + " " + PrdRange + " ORDER BY entryno,a.srno";

            }


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
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
            if (last_entdt == "0") { }
            else if (edmode.Value != "Y")
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    btnsave.Disabled = false;
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                btnsave.Disabled = false;
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "REELVCH");

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
                        save_fun3();
                        //save_fun4();
                        save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "REELVCH");

                        oDS4.Dispose();
                        oporow4 = null;
                        oDS4 = new DataSet();
                        //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                                {
                                    save_it = "Y";
                                }
                            }

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
                        //save_fun2();
                        save_fun3();
                        //save_fun4();
                        save_fun5();
                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update IVOUCHER set branchcd='DD' where branchcd||TRIM(STYLENO)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update reelvch set branchcd='DD' where branchcd||trim(job_no)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, "IVOUCHER");
                        fgen.save_data(frm_qstr, frm_cocd, oDS3, "REELvch");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                        fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from IVOUCHER where branchcd||TRIM(STYLENO)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from REELvch where branchcd||trim(job_no)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "CMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully'13'Do you want to see the Print Preview ?");

                                //#region Email Sending Function
                                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                ////html started                            
                                //sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                                //sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                                //sb.Append("<h5>" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR_NAME") + "</h5>");
                                //sb.Append("<br>Dear Sir/Mam,<br> This is to advise that the following <b>" + lblheader.Text + "</b> has been saved by " + frm_uname + ". Dept : " + txtlbl4a.Text.Trim() + "<br><br>");

                                ////table structure
                                //sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                                //sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                                //"<td><b>ERP Code</b></td><td><b>Product</b></td><td><b>Part No.</b></td><td><b>Qty</b></td><td><b>Unit</b></td>");
                                ////vipin
                                //foreach (GridViewRow gr in sg1.Rows)
                                //{
                                //    if (gr.Cells[13].Text.Trim().Length > 4)
                                //    {
                                //        sb.Append("<tr>");
                                //        sb.Append("<td>");
                                //        sb.Append(gr.Cells[13].Text.Trim());
                                //        sb.Append("</td>");
                                //        sb.Append("<td>");
                                //        sb.Append(gr.Cells[14].Text.Trim());
                                //        sb.Append("</td>");
                                //        sb.Append("<td>");
                                //        sb.Append(gr.Cells[15].Text.Trim());
                                //        sb.Append("</td>");
                                //        sb.Append("<td>");
                                //        sb.Append(((TextBox)gr.FindControl("sg1_t1")).Text.Trim());
                                //        sb.Append("</td>");
                                //        sb.Append("<td>");
                                //        sb.Append(gr.Cells[17].Text.Trim());
                                //        sb.Append("</td>");
                                //        sb.Append("</tr>");
                                //    }
                                //}
                                //sb.Append("</table></br>");

                                //sb.Append("Thanks & Regards");
                                //sb.Append("<h5>Note: This Report is Auto generated from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                                //sb.Append("</body></html>");

                                ////send mail
                                //string subj = "";
                                //if (edmode.Value == "Y") subj = "Edited : ";
                                //else subj = "New Entry : ";
                                //fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);


                                //fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr"), frm_uname, edmode.Value);

                                //sb.Clear();
                                //#endregion
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "" + frm_vnum + txtvchdate.Text.Trim() + "");
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));

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
        //sg1_dr["sg1_t17"] = "-";
        //sg1_dr["sg1_t18"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
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
                    fgen.Fn_open_sseek("Select Location", frm_qstr);
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

        if (txtvchnum.Text == "-")
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
                SQuery = "SELECT TRIM(A.ICODE) AS FSTR,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,A.KCLREELNO,A.COREELNO,B.OPRATE1,B.OPRATE3,B.UNIT,a.irate FROM REELVCH A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) and trim(a.icode)||trim(a.kclreelno) not in (" + col1 + ") ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Item", frm_qstr);
                break;
            case "SG2_ROW_JOB":
                hffield.Value = "SG2_ROW_JOB";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                SQuery = "Select distinct a.vchnum||a.vchdate as fstr,trim(a.Vchnum) as Job_no,to_Char(a.vchdate,'dd/mm/yyyy') as job_Dt,a.type,b.iname as item_name from costestimate a,item b where trim(a.icode)=trim(b.icodE) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + " order by trim(a.vchnum)  ";

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

        if (txtvchnum.Text == "-")
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

        if (txtvchnum.Text == "-")
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
        if (Prg_Id == "F39211")
        {
            fgen.Fn_open_sseek("Select Vendor ", frm_qstr);
        }
        else
        {
            fgen.Fn_open_sseek("Select Department" + "", frm_qstr);
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
        fgen.Fn_open_mseek("Select Main Group ", frm_qstr);
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
            case "F25402"://change by yogita
                fgen.Fn_open_sseek("Select Item Group ", frm_qstr);
                break;
            default:
                fgen.Fn_open_mseek("Select Item Group", frm_qstr);
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
        set_Val();
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                // oporow["TYPE"] = "RP"; //THIS IS OLD...at set_Val fun RL type fixed so why here RP?? i use type RL AS PER ALREADY fixed at set_Val()
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["maincode"] = txtlbl4.Text.Trim();
                oporow["STAGE"] = txtlbl7.Text.Trim();//item group
                oporow["srno"] = i + 1;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();

                oporow["iqtyin"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                oporow["ngqty"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim());

                oporow["wolink"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                oporow["work_ref"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();

                oporow["BOMQ"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().toDouble();

                oporow["loc_ref"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();

                oporow["remarks"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();

                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = ViewState["entby"].ToString();
                    oporow["eNt_dt"] = ViewState["entdt"].ToString();
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                }
                oporow["INSPECTED"] = "N";
                oporow["REMARKS"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    void save_fun2()
    {
        //         

    }
    void save_fun3()
    {
        double reel_diff = 0;
        sg1_dt = (DataTable)ViewState["sg1"];
        i = 0;
        foreach (DataRow dr in sg1_dt.Rows)
        {
            if (dr["sg1_f1"].ToString().Length > 4)
            {
                reel_diff = dr["sg1_t1"].ToString().toDouble() - dr["sg1_t2"].ToString().toDouble();
                if (reel_diff != 0)
                {
                    oporow3 = oDS3.Tables[0].NewRow();
                    oporow3["branchcd"] = frm_mbr;
                    oporow3["vchnum"] = frm_vnum;
                    oporow3["vchdate"] = txtvchdate.Text;
                    oporow3["REFNum"] = frm_vnum;
                    oporow3["REFdate"] = txtvchdate.Text;

                    oporow3["po_num"] = frm_vty;

                    oporow3["SRNO"] = i + 1;

                    oporow3["rec_iss"] = (reel_diff > 0 ? "C" : "D");
                    oporow3["type"] = (reel_diff > 0 ? "31" : "11");

                    // oporow3["job_no"] = "RP"; //OLD
                    oporow3["job_no"] = "RL";
                    oporow3["job_dt"] = "-";

                    oporow3["icode"] = dr["SG1_F1"].ToString().Trim();
                    oporow3["kclreelno"] = dr["SG1_t3"].ToString().Trim();

                    oporow3["REELWIN"] = (reel_diff > 0 ? 0 : Math.Abs(reel_diff));
                    oporow3["REELWOUT"] = (reel_diff > 0 ? Math.Abs(reel_diff) : 0);

                    oporow3["UNLINK"] = "N";
                    oporow3["POSTED"] = "Y";
                    oporow3["RINSP_BY"] = "REELOP*";

                    string itm_info = fgen.seek_iname(frm_qstr, frm_cocd, "Select oprate1||'~'||oprate3||'~'||abc_Class as val from item where trim(icode)='" + dr["sg1_f1"].ToString().Trim() + "'", "VAL");
                    oporow3["PSIZE"] = fgen.make_int(itm_info.Split('~')[0]);
                    oporow3["gsm"] = fgen.make_int(itm_info.Split('~')[1]);
                    oporow3["grade"] = itm_info.Split('~')[2];

                    oporow3["store_no"] = frm_mbr;
                    oporow3["reelspec2"] = dr["SG1_t9"].ToString().Trim().PadLeft(49);

                    oporow3["irate"] = dr["SG1_t5"].ToString().Trim().toDouble();
                    //rstmp!reelspec1 = checknullc(rsreelr!reelspec1)
                    //rstmp!acode = checknullc(rsreelr!acode)
                    oporow3["coreelno"] = dr["SG1_t4"].ToString().Trim();
                    oporow3["rlocn"] = dr["SG1_t8"].ToString().Trim();
                    oporow3["ACODE"] = dr["sg1_t11"].ToString().Trim();

                    oDS3.Tables[0].Rows.Add(oporow3);



                    oporow2 = oDS2.Tables[0].NewRow();
                    oporow2["branchcd"] = frm_mbr;
                    oporow2["vchnum"] = frm_vnum;
                    oporow2["vchdate"] = txtvchdate.Text;
                    oporow2["refnum"] = frm_vnum;
                    oporow2["refdate"] = txtvchdate.Text;

                    oporow2["STORE"] = "Y";
                    oporow2["ACODE"] = "60";

                    oporow2["icode"] = dr["SG1_F1"].ToString().Trim();

                    //oporow2["po_num"] = frm_vty;

                    oporow2["SRNO"] = i + 1;
                    oporow2["MORDER"] = i + 1;

                    oporow2["iqtyin"] = (reel_diff > 0 ? 0 : Math.Abs(reel_diff));
                    oporow2["iqtyout"] = (reel_diff > 0 ? Math.Abs(reel_diff) : 0);

                    oporow2["rec_iss"] = (reel_diff > 0 ? "C" : "D");

                    oporow2["type"] = (reel_diff > 0 ? "31" : "11");


                    oporow2["PSIZE"] = fgen.make_int(itm_info.Split('~')[0]);
                    oporow2["gsm"] = fgen.make_int(itm_info.Split('~')[1]);

                    //oporow2["STYLENO"] = "RP";//OLD
                    oporow2["STYLENO"] = "RL";
                    //oporow2["grade"] = itm_info.Split('~')[2];

                    oporow2["store_no"] = frm_mbr;
                    oporow2["desc_"] = "Phy.Verif by " + frm_uname + (dr["SG1_t9"].ToString().Trim().Length > 2 ? " , " + dr["SG1_t9"].ToString().Trim() : "");
                    oporow2["naration"] = dr["SG1_t9"].ToString().Trim();
                    oporow2["RCODE"] = dr["SG1_t11"].ToString().Trim();

                    oporow2["ent_by"] = frm_uname;
                    oporow2["ent_dt"] = vardate;
                    oporow2["edt_by"] = "-";
                    oporow2["edt_dt"] = vardate;

                    oDS2.Tables[0].Rows.Add(oporow2);
                    i++;
                }
            }
        }
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
                oporow5["par_fld"] = frm_mbr + lbl1a.Text + frm_vnum + txtvchdate.Text.Trim();
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
            case "F25115":
                SQuery = "SELECT 'RP' as Dtype,'Physical Verification (Lot Wise)' as Name,'RP' as Type from Dual";
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
    { }
    protected void Button1_Click(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
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
        DataTable dtn;
        //fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
        dt = new DataTable();
        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext == ".xls")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                FileUpload1.SaveAs(filesavepath);
                excelConString = fgen.connStringexcel(filesavepath);
                //excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            }
            else if (ext == ".csv")
            {
                filename = "" + DateTime.Now.ToString("ddMMyyhhmmfff");
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\file" + filename + ".csv";

                if (File.Exists(filesavepath))
                    fgen.del_file(filesavepath);

                FileUpload1.SaveAs(filesavepath);
                excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\" + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
            }
            else if (ext == ".xlsx")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xlsx";
                FileUpload1.SaveAs(filesavepath);
                excelConString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
            }
            else
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                return;
            }
            try
            {
                dtn = new DataTable();
                if (ext == ".csv")
                {
                    var allValues = File.ReadAllText(filesavepath).Split('\n');
                    int x = 0, colN = 0;
                    dt = new DataTable();
                    DataRow myRow = null;
                    foreach (string singleRow in allValues)
                    {
                        if (singleRow != "")
                        {
                            var allCols = singleRow.Split(',');
                            colN = 0;
                            if (x != 0) myRow = dt.NewRow();
                            foreach (string cols in allCols)
                            {
                                if (x == 0)
                                {
                                    dt.Columns.Add(cols);
                                }
                                else
                                {
                                    try
                                    {
                                        myRow[colN] = cols;
                                    }
                                    catch { }
                                    colN++;
                                }
                            }
                            if (x != 0) dt.Rows.Add(myRow);
                            x++;
                        }
                    }
                    dtn = dt;
                }
                else
                {
                    OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
                    OleDbConn.Open();
                    dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    OleDbConn.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[i] = row["TABLE_NAME"].ToString();
                        i++;
                    }
                    if (ext == ".csv")
                        excelSheets[0] = "file" + filename + ".csv";
                    OleDbCommand OleDbCmd = new OleDbCommand();
                    String Query = "";
                    Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                    OleDbCmd.CommandText = Query;
                    OleDbCmd.Connection = OleDbConn;
                    OleDbCmd.CommandTimeout = 0;
                    OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                    objAdapter.SelectCommand = OleDbCmd;
                    objAdapter.SelectCommand.CommandTimeout = 0;
                    dt = null;
                    dt = new DataTable();
                    objAdapter.Fill(dt);
                }

                if (dtn.Rows.Count > 0)
                {
                    DataTable dtItem = new DataTable();
                    dtItem = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(ICODE) AS ICODE,INAME,cpartno,UNIT FROM ITEM WHERE LENGTH(TRIM(ICODE))>4 ORDER BY ICODE ");
                    create_tab();
                    foreach (DataRow dr in dtn.Rows)
                    {
                        #region for gridview 1
                        //if (ViewState["sg1"] != null)
                        {

                            {
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                                sg1_dr["sg1_h1"] = dr[2].ToString().PadLeft(8, '0');
                                sg1_dr["sg1_h2"] = fgen.seek_iname_dt(dtItem, "ICODE='" + dr[2].ToString().PadLeft(8, '0') + "'", "iname");
                                sg1_dr["sg1_h3"] = "-";
                                sg1_dr["sg1_h4"] = "-";
                                sg1_dr["sg1_h5"] = "-";
                                sg1_dr["sg1_h6"] = "-";
                                sg1_dr["sg1_h7"] = "-";
                                sg1_dr["sg1_h8"] = "-";
                                sg1_dr["sg1_h9"] = "-";
                                sg1_dr["sg1_h10"] = "-";

                                sg1_dr["sg1_f1"] = dr[2].ToString().PadLeft(8, '0');
                                sg1_dr["sg1_f2"] = fgen.seek_iname_dt(dtItem, "ICODE='" + dr[2].ToString().PadLeft(8, '0') + "'", "iname");
                                sg1_dr["sg1_f3"] = fgen.seek_iname_dt(dtItem, "ICODE='" + dr[2].ToString().PadLeft(8, '0') + "'", "cpartno");
                                //sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();
                                //sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                                sg1_dr["sg1_f5"] = fgen.seek_iname_dt(dtItem, "ICODE='" + dr[2].ToString().PadLeft(8, '0') + "'", "unit");

                                sg1_dr["sg1_t1"] = "";
                                sg1_dr["sg1_t2"] = dr[5].ToString();
                                sg1_dr["sg1_t3"] = dr[4].ToString();
                                sg1_dr["sg1_t4"] = dr[3].ToString();
                                sg1_dr["sg1_t5"] = dr[7].ToString();
                                sg1_dr["sg1_t8"] = dr[8].ToString();
                                sg1_dr["sg1_t9"] = "-";
                                sg1_dr["sg1_t10"] = "-";
                                sg1_dr["sg1_t11"] = dr[1].ToString();
                                sg1_dr["sg1_t12"] = "";
                                sg1_dr["sg1_t13"] = "";

                                //string mpo_Dt;
                                //mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                                //sg1_dr["sg1_t14"] = mpo_Dt;
                                //sg1_dr["sg1_t15"] = "";
                                //mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                                //sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);


                                sg1_dt.Rows.Add(sg1_dr);
                            }
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
                }
            }
            catch(Exception err) { }
        }
    }

    void fillReelStock()
    {
        string typstring = col1;
        string icodecond = "and substr(icode,1,2) in (" + typstring + ") ";
        string reel_V_tbl = "reelvch";
        string mq0, mq1, mq2;
        string xprd1 = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
        string xprd2 = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
        //mq0 = "select icode,count(reelno) as reels,sum(closing) as Closing from (select trim(a.kclreelno) as Reelno,trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE from (Select '-' as kclreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE from REELVCH_OP where branchcd='" + frm_mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,7)='REELOP*' and 1=2 union all  ";
        //mq1 = "select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE union all ";
        //mq2 = "select kclreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE )a group by trim(a.kclreelno),trim(a.icode) having sum(opening)+sum(cdr)-sum(ccr)>0)group by Icode ";
        //SQuery = "create or replace view REEL_NOS_" + frm_mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + "))";

        mq0 = "select icode,REELNO,coreelno,count(reelno) as reels,sum(closing) as Closing from (select trim(a.kclreelno) as Reelno,trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE,COREELNO from (Select '-' as kclreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,COREELNO from REELVCH_OP where branchcd='" + frm_mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,7)='REELOP*' and 1=2 union all  ";
        mq1 = "select kclreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE,COREELNO from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE,COREELNO union all ";
        mq2 = "select kclreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE,COREELNO from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY kclreelno,ICODE,COREELNO )a group by trim(a.kclreelno),trim(a.icode),COREELNO having sum(opening)+sum(cdr)-sum(ccr)>0)group by Icode,reelno,coreelno ";
        SQuery = "create or replace view REEL_NOS_" + frm_mbr + " as (SELECT * FROM (" + mq0 + mq1 + mq2 + "))";

        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);


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
                //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();

                sg1_dt.Rows.Add(sg1_dr);
            }

            dt = new DataTable();

            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.*,B.INAME,B.CPARTNO,B.UNIT,B.CDRGNO FROM REEL_NOS_" + frm_mbr + " A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) ORDER BY A.ICODE ");

            for (int d = 0; d < dt.Rows.Count; d++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                sg1_dr["sg1_h3"] = "-";
                sg1_dr["sg1_h4"] = "-";
                sg1_dr["sg1_h5"] = "-";
                sg1_dr["sg1_h6"] = "-";
                sg1_dr["sg1_h7"] = "-";
                sg1_dr["sg1_h8"] = "-";
                sg1_dr["sg1_h9"] = "-";
                sg1_dr["sg1_h10"] = "-";

                sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                sg1_dr["sg1_f4"] = dt.Rows[d]["CDRGNO"].ToString().Trim();
                sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                sg1_dr["sg1_t1"] = dt.Rows[d]["Closing"].ToString().Trim();
                sg1_dr["sg1_t2"] = dt.Rows[d]["Closing"].ToString().Trim();
                sg1_dr["sg1_t3"] = dt.Rows[d]["REELNO"].ToString().Trim();
                sg1_dr["sg1_t4"] = dt.Rows[d]["COREELNO"].ToString().Trim();
                sg1_dr["sg1_t5"] = "";


                sg1_dr["sg1_t9"] = "";
                sg1_dr["sg1_t10"] = "-";
                sg1_dr["sg1_t11"] = "";
                sg1_dr["sg1_t12"] = "";
                sg1_dr["sg1_t13"] = "";

                sg1_dt.Rows.Add(sg1_dr);
            }
        }
        sg1_add_blankrows();

        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        //dt.Dispose(); 
        //sg1_dt.Dispose();
        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
        #endregion
        setColHeadings();
        setGST();
    }
    protected void txtBarcode_TextChanged(object sender, EventArgs e)
    {
        #region for gridview 1
        string mq1 = "", mq2 = "", mq3 = "";
        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
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
                //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                sg1_dt.Rows.Add(sg1_dr);
                //
                if (mq1.Length > 0) mq1 = mq1 + ",'" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "'";
                else mq1 = "'" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "'";
            }

            //foreach (GridViewRow gr in sg1.Rows)
            //{
            //    if (mq1.Length > 0) mq1 = mq1 + ",'" + gr.Cells[13].Text.Trim() + "'";
            //    else mq1 = "'" + gr.Cells[13].Text.Trim() + "'";
            //}

            dt = new DataTable();
            #region
            //=========
            //dt2 = new DataTable();
            //SQuery = "Select distinct branchcd,type,vchnum,vchdate,wolink from wipstk where branchcd='" + frm_mbr + "' and type='RL' AND VCHDATE " + DateRange + " and wolink='" + txtBarcode.Text + "'";
            //dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //this dt for saved reel no by this form
            //if(dt2.Rows.Count>0)
            //{
            //    for(int i=0;i<dt2.Rows.Count;i++)
            //    {
            //        if (mq3.Length > 0) mq3 = mq3 + ",'" + dt2.Rows[i]["wolink"].ToString().Trim() + "'";
            //        else mq3 = "'" + dt2.Rows[i]["wolink"].ToString().Trim() + "'";                   
            //    }
            //}
            //this is for stop repeation of reel no in grid
            //if (mq1.Trim().Length > 1 || mq3.Trim().Length > 1)
            //{
            //    if(mq3.Trim().Length>1)
            //    {
            //        mq2 = "a.my_reel not in (" + mq1 + "," + mq3 + ") ";
            //    }
            //    else
            //    {
            //        mq2 = "a.my_reel not in (" + mq1 + ") ";

            //    }                              
            //}
            //else
            //{
            //    mq2 = "a.my_reel like '%'";
            //}
            #endregion
            if (mq1.Trim().Length > 1)
            {
                mq2 = "a.my_reel not in (" + mq1 + ")";
            }
            else
            {
                mq2 = "a.my_reel like '%'";
            }
            //========
            String pop_qry;

            pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
            if (col1.Trim().Length < 8) SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7 from item a where trim(a.icode) in (" + col1 + ") order by a.iname";
            else SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7 from item a where trim(a.icode) in (" + col1 + ") order by a.iname";

            //SQuery = "select Iname,cpartno,my_reel as Our_reelno,op as Op_qty,inwd as Inw_Qty,outw as Cons_Qty,closing as Cl_Qty,co_Reel,bfactor ,psize,gsm,Icode,insp_done,rlocn from reel_dstk_" + frm_mbr + " where closing>0 and my_reel='" + txtBarcode.Text.Trim().ToUpper() + "' order by igrp,psize,gsm";//old qry
            SQuery = "select a.Iname,b.cpartno,b.pur_uom,a.my_reel as Our_reelno,a.op as Op_qty,a.inwd as Inw_Qty,a.outw as Cons_Qty,a.closing as Cl_Qty,a.co_Reel,a.bfactor ,a.psize,a.gsm,a.Icode,a.insp_done,a.rlocn from reel_dstk_" + frm_mbr + " a,item b where trim(a.icode)=trim(b.icode) and a.closing>0 and a.my_reel='" + txtBarcode.Text.Trim().ToUpper() + "' order by a.igrp,a.psize,a.gsm";
            SQuery = "select a.Iname,b.cpartno,b.pur_uom,a.my_reel as Our_reelno,a.op as Op_qty,a.inwd as Inw_Qty,a.outw as Cons_Qty,a.closing as Cl_Qty,a.co_Reel,a.bfactor ,a.psize,a.gsm,a.Icode,a.insp_done,a.rlocn from reel_dstk_" + frm_mbr + " a,item b where trim(a.icode)=trim(b.icode) and a.closing>0 and a.my_reel='" + txtBarcode.Text.Trim().ToUpper() + "' and " + mq2 + " order by a.igrp,a.psize,a.gsm";

            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                for (int d = 0; d < dt.Rows.Count; d++)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                    sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_h3"] = "-";
                    sg1_dr["sg1_h4"] = "-";
                    sg1_dr["sg1_h5"] = "-";
                    sg1_dr["sg1_h6"] = "-";
                    sg1_dr["sg1_h7"] = "-";
                    sg1_dr["sg1_h8"] = "-";
                    sg1_dr["sg1_h9"] = "-";
                    sg1_dr["sg1_h10"] = "-";

                    sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                    sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                    //sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();
                    sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                    sg1_dr["sg1_f5"] = dt.Rows[d]["pur_uom"].ToString().Trim();

                    sg1_dr["sg1_t1"] = dt.Rows[d]["cl_qty"].ToString().Trim();
                    sg1_dr["sg1_t2"] = "";
                    sg1_dr["sg1_t3"] = dt.Rows[d]["Our_reelno"].ToString().Trim();
                    sg1_dr["sg1_t4"] = "";
                    sg1_dr["sg1_t5"] = "";

                    sg1_dr["sg1_t9"] = "";
                    sg1_dr["sg1_t10"] = "-";
                    sg1_dr["sg1_t11"] = "";
                    sg1_dr["sg1_t12"] = "";
                    sg1_dr["sg1_t13"] = "";

                    sg1_dt.Rows.Add(sg1_dr);
                }
            }
            else
            {
                fgen.msg("-", "AMSG", "Physical Verification is already done for ReelNo " + mq1 + "");
            }
        }
        sg1_add_blankrows();
        txtBarcode.Text = "";
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        //dt.Dispose(); 
        //sg1_dt.Dispose();
        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
        #endregion
        setColHeadings();
        setGST();
    }
}