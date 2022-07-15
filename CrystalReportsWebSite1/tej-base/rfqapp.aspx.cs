using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using System.Drawing;


public partial class rfqapp : System.Web.UI.Page
{
    OracleConnection con = new OracleConnection();
    DataTable dt, dtCol, dt1, dt3, dt4, dt5, dt6, dt7;
    DataSet ds;
    OracleCommand cmd;
    OracleDataAdapter da;
    DateTime presentdate;
    MemoryStream oStream, oStream1;
    IFormatProvider AustralianDateFormat;
    DataTable sg1_dt; DataRow sg1_dr;

    string btnmode, vardate, query, query1, pgname, condition, timeout, DRID, DRTYP, frm_CDT1, todt, mq0, mq1, frm_vty, Prg_Id;
    string tco_cd, frm_qstr, mbr, co_cd, uname, frm_formID, cdt1, cdt2, scode, frm_tabname, frm_cocd, frm_uname, frm_myear, frm_ulvl, frm_mbr, sname, frm_UserID, fromdt, seek, DateRange, headername, daterange, ulevel, mlvl;
    string col1, col2, rptfilepath, rptpath, xmlpath, acessuser, ulvl, smbr, sstring, pageid, appuser; string otp, mobileno, btnval;
    int limit, i;
    string app_col, app_level, app_txt, mail_txt, app_flag, app_status, filename, mypath;
    string fName, fpath, extension, wSeriesControl = "Y";
    StringBuilder sb;
    string fullname, sendtoemail, subject, mailpath, mailport, xmltag, compnay_code, mailmsg, mflag, branchname, col3, col4, col5, col6, col7, fullname1;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            frm_qstr = Request.QueryString["STR"].Trim().ToString().ToUpper();
            if (frm_qstr.Contains("@"))
            {
                frm_formID = frm_qstr.Split('@')[1].ToString();
                pageid = frm_qstr.Split('@')[1].ToString();
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
            //=============old
            //tco_cd = Request.Cookies["CK_COFILEVARS"].Value.ToString();
            //co_cd = tco_cd.Substring(0, 4).Trim();
            //cdt1 = tco_cd.Substring(9, 10);
            //cdt2 = tco_cd.Substring(19, 10);
            //mbr = Request.Cookies["CK_mbr"].Value.ToString();
            //smbr = Request.Cookies["smbr"].Value.ToString();
            //uname = Request.Cookies["UNAME"].Value.ToString();
            //mlvl = Request.Cookies["UL_ACODE"].Value.ToString();
            //ulevel = mlvl.Substring(0, 1);
            //daterange = "between to_Date('" + cdt1 + "','dd/MM/yyyy') and to_Date('" + cdt2 + "','dd/MM/yyyy')";
            //try
            //{
            //    DRID = Request.Cookies["DRID"].Value.ToString();
            //    DRTYP = Request.Cookies["DRTYP"].Value.ToString();
            //}
            //catch { }

            // if (Request.Cookies["menuid"] != null) pageid = Request.Cookies["menuid"].Value.ToString();

            wSeriesControl = fgen.getOptionPW(frm_qstr, frm_cocd, "W2030", "OPT_ENABLE", frm_mbr);

            frm_tabname = "scratch2";

            if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103" || pageid == "40103V*"))
            {
                frm_tabname = "leadmst";
                pgname = "RFQ";
            }
            else if (pageid == "40106" || pageid == "40107" || pageid == "46101" || pageid == "40203" || pageid == "40201" || pageid == "40204a" || pageid == "40205a")
            {
                frm_tabname = "scratch";
                pgname = "REQ";

                if (pageid == "46101")
                    pgname = "Doc No.";
                else if (pageid == "40203" || pageid == "40201" || pageid == "40204a")
                    pgname = "Service Req.";
            }
            if ((frm_cocd == "MMC" || frm_cocd == "ECPL"))
            {
                frm_tabname = "WB_DRAWREC";
                pgname = "Doc No.";
            }
            else if (pageid == "40101" || pageid == "40101V")
                pgname = "Visitor Req.";
            else if ((pageid == "40103*" || pageid == "40103V"))
                pgname = "Gate Out Entry";
            else if ((frm_cocd == "ANYG") && (pageid == "40102" || pageid == "40103"))
                pgname = "Sampling Req.";
            else if (pageid == "40104" || pageid == "40105")
                pgname = "PI";
            else if (pageid == "40108")
                pgname = "Travel Ticket";
            else if (pageid == "40201" || pageid == "40204a")
                pgname = "Tour Advance";



            // con = new OracleConnection(fgen.GetCon(co_cd));

            if (Convert.ToDouble(frm_ulvl) > 1)
                acessuser = uname;
            else
                acessuser = "";

            if (Convert.ToInt32(frm_mbr) < 3) limit = 6;
            else limit = 5;

            if (!IsPostBack)
            {
                txtrows.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('ctl00_ContentPlaceHolder1_btnshow').click();return false;}} else {return true}; ");
                txtsearch.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('ctl00_ContentPlaceHolder1_btnsearch').click();return false;}} else {return true}; ");

                ViewState["SDATA"] = null;

                bindval();

                r1.Visible = false;
                btnfrom.Visible = false;
                txtfrom.Visible = false;
                btnto.Visible = false;
                txtto.Visible = false;

                switch (pageid)
                {
                    case "40102":
                        if (frm_cocd == "ANYG")
                            lblhead.Text = "Sampling Requisition Checking";
                        else
                            lblhead.Text = "Feasibility Study - Technical Approval";
                        break;
                    case "40103":
                        lblhead.Text = "Sampling Requisition Approval";
                        break;
                    case "40103V":
                        if (frm_cocd == "JSGI" || frm_cocd == "DLJM" || frm_cocd == "SDM")
                            lblhead.Text = "Gate Out Entry";
                        else if (frm_cocd == "ANYG*")
                            lblhead.Text = "Sampling Requisition Approval";
                        else if (frm_cocd == "NIRM" || frm_cocd == "PRAG")
                            lblhead.Text = "Feasibility Study - Financial Approval";
                        lblhead.Text = "Gate Out Entry";
                        break;
                    case "40106":
                        lblhead.Text = "Order Requisition Checking";
                        break;
                    case "40107":
                        lblhead.Text = "Order Requisition Approval";
                        break;
                    case "40101":
                    case "40101V":
                        lblhead.Text = "Visitor Requisition Approval";
                        break;
                    case "40203":
                        lblhead.Text = "Service Request Approval";
                        break;
                    case "46101":
                        lblhead.Text = "Resume Reviewal";
                        break;
                    case "46103":
                        lblhead.Text = "PREMAGMA Information Library";
                        break;
                    case "47103":
                        lblhead.Text = "IGES Information Library";
                        break;
                    case "48103":
                        lblhead.Text = "PI Report in Information Library";
                        break;
                    case "49103":
                        lblhead.Text = "First PC Casting Report in Information Library";
                        break;
                    case "52103":
                        lblhead.Text = "SW Information Library";
                        break;
                    case "53103":
                        lblhead.Text = "MAGMA Simulation Library";
                        break;
                    case "F55165":
                        lblhead.Text = "Drawing Information Library";
                        break;
                    case "F55166":
                        lblhead.Text = "Drawing Information Library (only Issued Files)";
                        break;
                    case "43105":
                        lblhead.Text = "Drawing Issue Preview";
                        break;
                    case "40201":
                    case "40204a":
                        lblhead.Text = "Tour Advance Approval";
                        break;
                    case "40205a":
                        lblhead.Text = "Enquiry Closure";
                        break;
                    case "40108":
                        lblhead.Text = "Travel Ticket Clear/Cancellation";
                        break;
                    case "40105":
                        lblhead.Text = "PI - Approval";
                        break;
                    case "40104":
                        lblhead.Text = "PI - Checking";
                        break;
                    case "51103":
                        lblhead.Text = "Trial Information Library";
                        break;
                    case "60412":
                        lblhead.Text = "Quality Information Library";
                        break;
                    case "51503":
                        lblhead.Text = "Method Card Information Library";
                        break;
                    case "60110a":
                        lblhead.Text = "Moulding Plan Information Library";
                        break;
                    case "60113":
                        lblhead.Text = "Moulding Information Library";
                        break;
                    case "60213":
                        lblhead.Text = "Pouring Information Library";
                        break;
                    case "60313":
                        lblhead.Text = "Knock Out Information Library";
                        break;
                    case "42515":
                        lblhead.Text = "Closing Review Information Library";
                        break;
                }
                if ((frm_cocd == "SHOP" || frm_cocd == "SNPX") && pageid == "40102")
                {
                    btnfrom.Visible = true;
                    txtfrom.Visible = true;
                    btnto.Visible = true;
                    txtto.Visible = true;
                }
                Page.Title = lblhead.Text;
            }

        }
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
        if (GridView1.Rows.Count <= 0) return;
        for (int sR = 0; sR < GridView1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = GridView1.HeaderRow.Cells[sR].Text.Trim();

            for (int K = 0; K < GridView1.Rows.Count; K++)
            {
                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    GridView1.Columns[i].HeaderStyle.CssClass = "hidden";
                    GridView1.Rows[K].Cells[i].CssClass = "hidden";
                }
                if (pageid == "F55165" || pageid == "F55166" || frm_cocd == "MMC")
                {
                    GridView1.Columns[1].HeaderStyle.CssClass = "hidden";
                    GridView1.Rows[K].Cells[1].CssClass = "hidden";
                    GridView1.Columns[2].HeaderStyle.CssClass = "hidden";
                    GridView1.Rows[K].Cells[2].CssClass = "hidden";
                }
                #endregion
                //if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)GridView1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
                if (pageid == "43105" || pageid == "F55165" || pageid == "F55166")
                {
                    ((TextBox)GridView1.Rows[K].FindControl("txtdate")).Attributes.Add("display", "none");
                    ((TextBox)GridView1.Rows[K].FindControl("txttout")).Attributes.Add("display", "none");
                    //  GridView1.Columns[13].Visible = false;
                    //  GridView1.Columns[14].Visible = false;
                }

            }
            orig_name = orig_name.ToUpper();
            //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    GridView1.Columns[sR].Visible = false;
                }
                // Setting Heading Name                
                GridView1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    GridView1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    GridView1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }
        // to hide and show to tab panel
        //tab5.Visible = false;

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //--------------------------------------------

    //protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    //if (e.Row.RowType == DataControlRowType.DataRow)
    //    //{
    //    //    for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
    //    //    {
    //    //        for (int j = 0; j < sg1.Columns.Count; j++)
    //    //        {
    //    //            sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
    //    //            if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
    //    //            {
    //    //                sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
    //    //            }
    //    //        }
    //    //    }
    //    //    e.Row.Cells[1].Width = 30;
    //    //    e.Row.Cells[1].Width = 30;
    //    //    e.Row.Cells[3].Width = 30;
    //    //}
    //    //========================================
    //    if (pageid == "46101" || co_cd == "MMC")
    //    {
    //        e.Row.Cells[1].Visible = false;
    //        e.Row.Cells[2].Visible = false;
    //        e.Row.Cells[5].Visible = false;
    //    }
    //    //e.Row.Cells[3].Visible = false;
    //    //e.Row.Cells[4].Visible = false;
    //    //e.Row.Cells[6].Visible = false;
    //    if (pageid == "F55165" || pageid == "51103" || pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313" || pageid == "60412" || pageid == "42515") { }
    //    else
    //        e.Row.Cells[15].Visible = false;

    //    if ((co_cd == "NIRM" || co_cd == "PRAG") && (pageid == "40102" || pageid == "40103"))
    //        e.Row.Cells[3].Visible = true;

    //    if (pageid == "40106" || pageid == "40107")
    //        e.Row.Cells[15].Visible = true;

    //    if (pageid == "40103" && (co_cd == "JSGI" || co_cd == "DLJM" || co_cd == "SDM"))
    //        e.Row.Cells[4].Visible = true;
    //    if (pageid == "40103V") e.Row.Cells[4].Visible = true;

    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        if (pageid == "40106" || pageid == "40107")
    //            e.Row.Cells[19].Text = "Margin %";
    //    }
    //    if (pageid == "40103" && (co_cd == "JSGI" || co_cd == "DLJM" || co_cd == "SDM"))
    //    {
    //        e.Row.Cells[2].Visible = false;
    //        e.Row.Cells[5].Visible = false;
    //        e.Row.Cells[4].Visible = true;
    //    }

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        LinkButton vlink = (LinkButton)e.Row.FindControl("LnkBtnv");
    //        LinkButton mlink = (LinkButton)e.Row.FindControl("LnkBtnd");
    //        mlink.Visible = false;
    //        if (co_cd == "MMC") mlink.Visible = true;
    //        if (pageid == "51103" || pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313" || pageid == "60412" || pageid == "42515") mlink.Text = "View Details";
    //        if (pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313") vlink.Visible = false;

    //        if (co_cd == "MMC")
    //        {
    //            DataTable dt_dp = new DataTable();
    //            //dt_dp = fgen.fill_dt(co_cd, "SELECT * FROM D_RIGHTS WHERE /*BRANCHCD='" + mbr + "' AND*/ TYPE='RI' AND TRIM(COL1)='" + pageid + "' AND TRIM(upper(COL3))='" + uname + "'");
    //            mq0 = "SELECT * FROM D_RIGHTS WHERE /*BRANCHCD='" + frm_mbr + "' AND*/ TYPE='RI' AND TRIM(COL1)='" + pageid + "' AND TRIM(upper(COL3))='" + frm_uname + "'";
    //            dt_dp = fgen.getdata(frm_qstr, frm_cocd, mq0);
    //            if (dt_dp.Rows.Count > 0) { }
    //            else
    //            {
    //                if (ulevel != "0")
    //                {
    //                    vlink.Visible = false;
    //                    mlink.Visible = false;
    //                }
    //            }
    //            foreach (DataRow dr_dp in dt_dp.Rows)
    //            {
    //                if (dr_dp["col1"].ToString().Trim() == pageid.Trim())
    //                {
    //                    if (dr_dp["y_n"].ToString().Trim() == "Y")
    //                        vlink.Visible = true;
    //                    else vlink.Visible = false;
    //                    if (dr_dp["y_n1"].ToString().Trim() == "Y")
    //                        mlink.Visible = true;
    //                    else mlink.Visible = false;
    //                }
    //            }
    //        }
    //    }
    //}
    ////------------------------------------------------------------------------------------
    //protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    string var = e.CommandName.ToString();
    //    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
    //    int index = Convert.ToInt32(GridView1.Rows[rowIndex].RowIndex);
    //    //if (txtdocno.Text == "-")
    //    //{
    //    //    fgen.msg("-", "AMSG", "Doc No. not correct");
    //    //    return;
    //    //}
    //    switch (var)
    //    {
    //        case "SG1_RMV":
    //            if (index < GridView1.Rows.Count - 1)
    //            {
    //                hf1.Value = index.ToString();
    //                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
    //                //----------------------------
    //                hffield.Value = "SG1_RMV";
    //                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
    //                fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Export Invoice From The List");
    //            }
    //            break;

    //        case "SG1_ROW_ADD":
    //            if (index < GridView1.Rows.Count - 1)
    //            {
    //                // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
    //                hf1.Value = index.ToString();
    //                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
    //                //----------------------------
    //                // hffield.Value = "SG1_ROW_ADD_E";
    //                hffield.Value = "TACODE";
    //                hf2.Value = "SG1_ROW_ADD_E";
    //                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
    //                // make_qry_4_popup();
    //                //fgen.Fn_open_sseek("Select Export Invoice", frm_qstr);                  
    //                fgen.Fn_open_prddmp1("-", frm_qstr);
    //            }
    //            else
    //            {
    //                // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
    //                //hffield.Value = "SG1_ROW_ADD";
    //                hffield.Value = "TACODE";
    //                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
    //                fgen.Fn_open_prddmp1("-", frm_qstr);
    //                //make_qry_4_popup();
    //                //fgen.Fn_open_mseek("Select Export Invoice", frm_qstr);
    //            }
    //            break;
    //    }
    //}
    ////------------------------------------------------------------------------------------

    public void disp_data(string scode)
    {
        query = "";
        btnmode = hfbtnmode.Value;
        switch (btnmode)
        {
            case "FR":
                query = "select userid as fstr,username as assignto,userid from evas order by userid";
                break;
            case "TO":
                query = "select acode as fstr,fname as First_Name,Lname as last_name,CNAME AS COMPANY_NAME,mobile,email AS PRIMARY_EMAIL,PEMAIL AS SECONDARY_EMAIL,website,PADDR1 as paddress,PADDR2 as pcountry,PADDR3 as pstate,PADDR4 as pcity,PADDR5 as ppostalcode,PADDR6 as pregion,dept as department from contmst  where  type='TM' order by fname ";
                break;
            case "BR":
                query = "select type1 as fstr,name as Branch_name, type1 as code from type where id='B' order by type1";
                break;
            default:
                if (btnmode == "VI" || btnmode == "DI")
                {
                    if (pageid == "46101" || frm_cocd == "MMC")
                        query = "select '" + scode + "' as ftr,filename,filetype as file_type from filetable where branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') = '" + scode + "'";
                    if ((pageid == "51103" || pageid == "60412" || pageid == "42515") && btnmode == "DI")
                    {
                        if (pageid == "42515")
                        {
                            query = "Select 'FPA Closing Review' as header,a.* from scratch a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'ddmmyyyy')='" + scode + "' order by a.srno";
                        }
                        else
                        {
                            query = "select a.*," + pageid + " as pid  from DRAWREC a where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')='" + scode + "' and trim(nvl(a.t11,'-'))!='-' order by a.dsrno";
                        }
                        DataSet ds = new DataSet();
                        da = new OracleDataAdapter(query, con);
                        da.Fill(ds, "Prepcur");
                        string xmlfile = string.Empty;

                        if (pageid == "60412")
                        {
                            headername = "Quality Entry Print";
                            // ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");                     
                            ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr));
                            xmlfile = Server.MapPath("~/xmlfile/Qplan.xml");
                            ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                            Session["mydataset"] = ds;
                            Response.Cookies["rptfile"].Value = "~/Report/Qplan.rpt";
                        }
                        else if (pageid == "42515")
                        {
                            headername = "FPA Closing Review Print";
                            // ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");
                            ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr));
                            xmlfile = Server.MapPath("~/xmlfile/crpt_PatrnClosing.xml");
                            ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                            Session["mydataset"] = ds;
                            Response.Cookies["rptfile"].Value = "~/Report/crpt_PatrnClosing.rpt";
                        }
                        else
                        {
                            //ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");
                            ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr));
                            xmlfile = Server.MapPath("~/xmlfile/trial.xml");
                            ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                            Session["mydataset"] = ds;
                            Response.Cookies["rptfile"].Value = "~/Report/trial.rpt";
                            headername = "Trial Note Print";
                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
                        query = "";
                    }
                    //else if (pageid == "51503")
                    //    query = "select '" + mbr + "TN" + scode + "' as ftr,filename,filetype as file_type from filetable where branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') = '" + scode + "'";
                }
                break;
        }
        if (query == "") { }
        else
        {
            if (btnmode == "SURE_S") Response.Cookies["popupid"].Value = "FINSYS_S";
            else Response.Cookies["popupid"].Value = "Tejaxo";

            Response.Cookies["seeksql"].Value = query;
        }
    }
    public void sseekfunc(string scode)
    {
        clearcontrol();
        disp_data(scode);
        OpenPopup("SSEEK");
    }
    public void AlertMsg(string msgtype, string msgname)
    {
        switch (msgtype)
        {
            case "AMSG":
                alermsg.InnerHtml = msgname;
                alermsg.Style.Add("display", "block");
                break;
        }
    }
    public void OpenPopup(string popuptype)
    {
        headername = "";
        btnmode = hfbtnmode.Value;
        switch (popuptype)
        {
            case "SSEEK":
                switch (btnmode)
                {
                    case "VI":
                        switch (pageid)
                        {
                            case "46101":
                                headername = "Resume Review";
                                break;
                            case "46103":
                                headername = "PREMAGMA Information Library";
                                break;
                            case "47103":
                                headername = "IGES Information Library";
                                break;
                            case "48103":
                                headername = "PI Report in Information Library";
                                break;
                            case "49103":
                                headername = "First PC Casting Report in Information Library";
                                break;
                            case "43105":
                                headername = "Drawing Issue Preview";
                                break;
                            case "52103":
                                headername = "SW Information Library";
                                break;
                            case "53103":
                                headername = "MAGMA Information Library";
                                break;
                            case "F55165":
                                headername = "Drawing Information Library";
                                break;
                            case "F55166":
                                headername = "Drawing Information Library (only issued files)";
                                break;
                            case "51103":
                                headername = "Trial Information Library";
                                break;
                            case "60412":
                                lblhead.Text = "Quality Information Library";
                                break;
                            case "51503":
                                headername = "Methor Card Information Library";
                                break;
                            case "60110a":
                                lblhead.Text = "Moulding Plan Information Library";
                                break;
                            case "60113":
                                lblhead.Text = "Moulding Plan Information Library";
                                break;
                            case "60213":
                                lblhead.Text = "Pouring Information Library";
                                break;
                            case "60313":
                                lblhead.Text = "Knock Out Information Library";
                                break;
                            case "42515":
                                lblhead.Text = "Closing Review Information Library";
                                break;
                        }
                        break;
                    case "BR":
                        headername = "Branch Master";
                        break;
                    default:
                        if (btnmode == "FR" || btnmode == "TO")
                            headername = "User Master";
                        break;
                }
                break;
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','SSeek.aspx','75%','82%',false);});", true);
    }


    public void BindData(string query)
    {
        lblshow.Text = ""; query1 = "";
        clearcontrol();
        setColHeadings();
        getColHeading();
        create_tab();

        dt = new DataTable();

        if (txtrows.Text == "")
        {
            txtrows.Text = "45";
            query1 = "Select * from(" + query + ") where rownum<=" + txtrows.Text + "";
        }
        else
            query1 = "Select * from(" + query + ") where rownum<=" + txtrows.Text + "";

        dt5 = new DataTable();
        app_col = ""; app_level = ""; app_txt = "";

        if (pageid == "40106")
        {
            app_col = "col37";
            app_txt = "Verify";
        }
        if (pageid == "40107")
        {
            app_col = "col39";
            app_txt = "Approve";
        }

        if (frm_cocd == "SHOP" || frm_cocd == "SNPX")
        {
            mq0 = "select distinct " + app_col + "  from scratch where branchcd = '" + mbr + "' and type='RQ'";
            dt5 = fgen.getdata(frm_qstr, frm_cocd, mq0);
            foreach (DataRow dr in dt5.Rows)
            {
                app_level = dr[0].ToString().Trim();
            }

            if (app_level == uname)
            {
                dt = fgen.getdata(frm_qstr, frm_cocd, query1);
            }
            else
            {
                AlertMsg("AMSG", "Sorry!! you are not authorized to " + app_txt + " order reqisition. ");
                con.Close();
                return;
            }
        }
        else
        {
            dt = fgen.getdata(frm_qstr, frm_cocd, query1);
        }

        // con.Close();
        //if (dt.Rows.Count == 0 && hfbtnmode.Value != "TR")
        //    AlertMsg("AMSG", "No " + pgname + " exists in Database for this user");

        ViewState["SDATA"] = dt;
        ViewState["sg1"] = sg1_dt;
        //sg1.DataSource = dt;
        //sg1.Visible = true;
        //sg1.DataBind();
        create_tab();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
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

                sg1_dr["chkok"] = "-";
                sg1_dr["chkno"] = "-";
                sg1_dr["txtdate"] = "-";
                sg1_dr["txttout"] = "-";
                sg1_dr["txtrsn"] = "-";

                if (pageid == "43105")
                {
                    sg1_dr["sg1_f1"] = dt.Rows[i]["EDN"].ToString();
                    sg1_dr["sg1_f2"] = dt.Rows[i]["edn_date"].ToString();
                    sg1_dr["sg1_f3"] = dt.Rows[i]["drawing_type"].ToString();
                    sg1_dr["sg1_f4"] = dt.Rows[i]["drawing_no"].ToString();
                    sg1_dr["sg1_f5"] = dt.Rows[i]["revision_no"].ToString();
                    sg1_dr["sg1_f6"] = dt.Rows[i]["design_type"].ToString();
                    sg1_dr["sg1_f7"] = dt.Rows[i]["issue_to"].ToString();
                    sg1_dr["sg1_f8"] = dt.Rows[i]["issue_by"].ToString();
                    sg1_dr["sg1_f9"] = dt.Rows[i]["return_target_date"].ToString();
                    sg1_dr["sg1_f10"] = dt.Rows[i]["ent_by"].ToString();
                    sg1_dr["sg1_f11"] = dt.Rows[i]["ent_dt"].ToString();
                    sg1_dr["sg1_f12"] = dt.Rows[i]["vdd"].ToString();
                    sg1_dr["sg1_f13"] = dt.Rows[i]["filepath"].ToString();
                }
                if (pageid == "F55165" || pageid == "F55166")
                {
                    sg1_dr["sg1_f1"] = dt.Rows[i]["entry_no"].ToString();
                    sg1_dr["sg1_f2"] = dt.Rows[i]["entry_date"].ToString();
                    sg1_dr["sg1_f3"] = dt.Rows[i]["customer"].ToString();
                    sg1_dr["sg1_f4"] = dt.Rows[i]["iname"].ToString();
                    sg1_dr["sg1_f5"] = dt.Rows[i]["cpartno"].ToString();
                    sg1_dr["sg1_f6"] = dt.Rows[i]["modal_no"].ToString();
                    sg1_dr["sg1_f7"] = dt.Rows[i]["drawing_no"].ToString();
                    sg1_dr["sg1_f8"] = dt.Rows[i]["revision_no"].ToString();
                    sg1_dr["sg1_f9"] = dt.Rows[i]["drawing_stage"].ToString();
                    sg1_dr["sg1_f10"] = dt.Rows[i]["col1"].ToString();
                    sg1_dr["sg1_f11"] = dt.Rows[i]["ent_by"].ToString();
                    sg1_dr["sg1_f12"] = dt.Rows[i]["ent_dt"].ToString();
                    sg1_dr["sg1_f13"] = dt.Rows[i]["acode"].ToString();
                }
                sg1_dt.Rows.Add(sg1_dr);
            }
        }
        GridView1.DataSource = sg1_dt;
        GridView1.DataBind();
        setColHeadings();
        GridView1.Visible = true;

        if (hfbtnmode.Value == "EX")
        {
            if (dt.Rows.Count > 0)
            {
                headername = lblhead.Text;
                //fgen.ExportData(dt, "ms-excel", "xls", headername);
                fgen.exp_to_excel(dt, "ms-excel", "xls", headername);
            }
        }

        if (pageid == "40103*" || pageid == "40103V")
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                ((TextBox)row.FindControl("txttout")).Text = DateTime.Now.ToString("HH:mm");
            }
        }

        //if (pageid == "40103*" || pageid == "40103V")
        //{
        //    foreach (GridViewRow row in sg1.Rows)
        //    {
        //        ((TextBox)row.FindControl("txttout")).Text = DateTime.Now.ToString("HH:mm");
        //    }
        //}
        lblshow.Text = "Showing " + dt.Rows.Count + " Rows ";
    }

    protected void btnexp_Click(object sender, EventArgs e)
    {
        hfbtnmode.Value = "EX";
        bindval();
    }

    public void clearcontrol()
    {
        //fgen.RemoveTextBoxBorder(this.Page);
        //alermsg.Style.Add("display", "none");
    }
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        lblhead.Text = "Drawing Issue Preview";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_DRAWREC";
        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DE");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        //typePopup = "N";     
    }
    protected void btntrans_Click(object sender, EventArgs e)
    {
        if (pageid == "46101" || frm_cocd == "MMC") return;

        clearcontrol();

        string GetLeadNo = "", reason = "", odate = "", ndate = "", fok = "", fno = "";
        appuser = "";

        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102"))
            appuser = "[T]" + uname;
        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40103"))
            appuser = "[F]" + uname;

        //  con.Open();

        hfbtnmode.Value = "TR";
        foreach (GridViewRow row in GridView1.Rows)
        {
            row.BackColor = Color.Empty;

            bool okChecked = ((CheckBox)row.FindControl("chkok")).Checked;
            bool noChecked = ((CheckBox)row.FindControl("chkno")).Checked;

            if (okChecked && noChecked)
            {
                AlertMsg("AMSG", "Please check in line no " + Convert.ToInt32(row.RowIndex + 1) + " both option OK and NO is checked. ");
                row.BackColor = Color.Yellow;
                return;
            }
            if (okChecked)
            {
                fok = "OK";

                if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103" || pageid == "40103V")) { }
                else appuser = frm_uname;

                if (pageid == "40108") appuser = "CONFIRMED";

                if (GetLeadNo.Length > 0)
                    GetLeadNo += ",";
                GetLeadNo += row.Cells[7].Text.Trim();

                if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103" || pageid == "40103V"))
                {
                    if (((TextBox)row.FindControl("txtdate")).Text.Trim() == "")
                    {
                        AlertMsg("AMSG", "Please enter date in line no " + Convert.ToInt32(row.RowIndex + 1) + ". ");
                        ((TextBox)row.FindControl("txtdate")).BorderColor = Color.Red;
                        row.BackColor = Color.Yellow;
                        return;
                    }
                }
                //VIPIN
                reason = ((TextBox)row.FindControl("txtrsn")).Text.Trim();
                odate = ((TextBox)row.FindControl("txtdate")).Text.Trim();
                timeout = ((TextBox)row.FindControl("txttout")).Text.Trim();

                ((CheckBox)row.FindControl("chkok")).Checked = false;
                ((TextBox)row.FindControl("txtdate")).Text = "";

                ViewState["GNO"] = GetLeadNo;


                if (pageid == "40103" || pageid == "40103V")
                    GetLeadData(row.Cells[6].Text.Trim(), timeout, reason, "");
                else if (pageid == "40108")
                    GetLeadData(row.Cells[6].Text.Trim(), "-", odate, row.Cells[10].Text.Trim() + "," + row.Cells[17].Text.Trim() + "," + "OK" + "," + row.Cells[14].Text.Trim() + "," + row.Cells[15].Text.Trim() + "," + row.Cells[16].Text.Trim() + "," + row.Cells[22].Text.Trim());
                else
                {
                    string gval = "";
                    try
                    {
                        gval = row.Cells[22].Text.Trim();
                    }
                    catch { }
                    GetLeadData(row.Cells[6].Text.Trim(), "-", odate, row.Cells[7].Text.Trim() + "," + row.Cells[8].Text.Trim() + "," + gval + "," + "OK");
                }
            }
            if (noChecked)
            {
                fno = "NO";
                if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103" || pageid == "40103V")) { }
                else appuser = "[U]" + uname;

                if (pageid == "40108") appuser = "CANCEL";

                if (GetLeadNo.Length > 0)
                    GetLeadNo += ",";
                GetLeadNo += row.Cells[7].Text.Trim();

                if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103" || pageid == "40103V"))
                {
                    if (((TextBox)row.FindControl("txtdate")).Text.Trim() == "")
                    {
                        AlertMsg("AMSG", "Please enter date in line no " + Convert.ToInt32(row.RowIndex + 1) + ". ");
                        ((TextBox)row.FindControl("txtdate")).BorderColor = Color.Red;
                        row.BackColor = Color.Yellow;
                        return;
                    }
                }
                if (((TextBox)row.FindControl("txtrsn")).Text.Trim() == "")
                {
                    AlertMsg("AMSG", "Please enter reason in line no " + Convert.ToInt32(row.RowIndex + 1) + ". ");
                    ((TextBox)row.FindControl("txtrsn")).BorderColor = Color.Red;
                    row.BackColor = Color.Yellow;
                    return;
                }

                ndate = ((TextBox)row.FindControl("txtdate")).Text.Trim();
                reason = ((TextBox)row.FindControl("txtrsn")).Text.Trim();

                ((CheckBox)row.FindControl("chkno")).Checked = false;
                ((TextBox)row.FindControl("txtrsn")).Text = "";
                ((TextBox)row.FindControl("txtdate")).Text = "";

                ViewState["GNO"] = GetLeadNo;

                if (pageid == "40108")
                    GetLeadData(row.Cells[6].Text.Trim(), "-", odate, row.Cells[10].Text.Trim() + "," + row.Cells[17].Text.Trim() + "," + "OK" + "," + row.Cells[14].Text.Trim() + "," + row.Cells[15].Text.Trim() + "," + row.Cells[16].Text.Trim() + "," + row.Cells[22].Text.Trim());
                else
                    GetLeadData(row.Cells[6].Text.Trim(), reason, ndate, row.Cells[7].Text.Trim() + "," + row.Cells[8].Text.Trim() + "," + row.Cells[22].Text.Trim() + "," + "NO");
            }
        }

        // con.Close();

        bindval();

        if (ViewState["GNO"] != null) GetLeadNo = (string)ViewState["GNO"];

        app_level = "";

        if (fok == "OK")
        {
            app_level = " has been Approved ";
            if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103")) { }
            else if (pageid == "40106")
                app_level = " has been Verified ";
            else if (pageid == "40102" || pageid == "40104")
                app_level = " has been Checked ";
            else if (pageid == "40103" || pageid == "40103V")
                app_level = " has been Done ";
            else if (pageid == "40108")
                app_level = " has been Cleared ";

        }
        if (fno == "NO") app_level = " has been Rejected ";

        if (fok == "OK" && fno == "NO")
        {
            app_level = " has been Approved/Rejected ";
            if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103")) { }
            else if (pageid == "40106")
                app_level = " has been Verified/Rejected ";
            else if (pageid == "40102" || pageid == "40104")
                app_level = " has been Checked/Rejected ";
            else if (pageid == "40108")
                app_level = " has been Cancelled ";
        }

        if (mflag == "Y") mailmsg = "Email has been Sent Successfully.";
        if (mflag == "N") mailmsg = "**Alert** No Email has been Sent!";

        if (GetLeadNo.Length > 5)
            AlertMsg("AMSG", "Selected " + pgname + " No. " + GetLeadNo + " " + app_level + "" + "<br>" + mailmsg);
        else
            AlertMsg("AMSG", "No " + pgname + " selected for approval ");

        ViewState["GNO"] = null;
    }
    public string send_email(string mflag, string gstr, string fstr, string reason)
    {
        mail_txt = "";

        dt = new DataTable();
        da = new OracleDataAdapter("select nvl(emailID,'-') as emailID FROM evas where upper(trim(username)) in ('" + uname + "')", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            sendtoemail = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (sendtoemail.Length > 0)
                    sendtoemail += ",";
                if (dr["emailid"].ToString().Trim().Length > 4)
                    sendtoemail += dr["emailid"].ToString().Trim();
            }

            col1 = ""; col2 = ""; col3 = ""; col4 = "";

            string[] gval = gstr.Split(',');

            col1 = gval[0].ToString().Trim();
            col2 = gval[1].ToString().Trim();
            col3 = gval[2].ToString().Trim();
            col4 = gval[3].ToString().Trim();
            if (pageid == "40201" || pageid == "40204a" || pageid == "40205a") { }
            else
            {
                col5 = gval[4].ToString().Trim();
                col6 = gval[5].ToString().Trim();
                col7 = gval[6].ToString().Trim();
            }

            dt = new DataTable();
            da = new OracleDataAdapter("select nvl(email,'-') as email FROM empmas where upper(trim(empcode))= '" + col1 + "'", con);
            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                if (sendtoemail.Length > 0)
                    sendtoemail += ",";
                if (dr["email"].ToString().Trim().Length > 4)
                    sendtoemail += dr["email"].ToString().Trim();
            }

            if (sendtoemail.Length > 0)
                sendtoemail += ",";
            sendtoemail += fgen.GetXMLTag("xdeid");

            //endtoemail = fgen.checkemail(sendtoemail);//uncomment

            if (col4 == "OK") mail_txt = "Cleared";
            if (col4 == "NO") mail_txt = "Cancelled";

            if (pageid == "40201" || pageid == "40204a")
                subject = " Tour Advance Against Service Ticket No. " + fstr.Substring(4, 6) + " has been " + mail_txt + ". ";
            else
                subject = " Traveller Booking No. " + fstr.Substring(4, 6) + " has been " + mail_txt + ". ";

            sb = new StringBuilder();
            sb.Append("<html><body>");
            if (pageid == "40201" || pageid == "40204a")
            {
                dt = new DataTable();
                da = new OracleDataAdapter("select col11 ,col52 ,col5 ,remarks ,col39 ,col51 ,col50 ,num3 ,num4 ,col20 ,col53 from scratch where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  '" + fstr + "'", con);
                da.Fill(dt);

                sb.Append("<br>Some Information:<br>");
                sb.Append("<br> Employee Name : <b>" + dt.Rows[0][0].ToString().Trim() + "</b> ");
                sb.Append("<br> Department : <b>" + dt.Rows[0][1].ToString().Trim() + "</b> ");
                sb.Append("<br> Designation : <b>" + dt.Rows[0][2].ToString().Trim() + "</b> ");
                sb.Append("<br> Site Details : <b>" + dt.Rows[0][3].ToString().Trim() + "</b> ");
                sb.Append("<br> No. of days : <b>" + dt.Rows[0][4].ToString().Trim() + "</b> ");
                sb.Append("<br> Visite Date : <b>" + dt.Rows[0][5].ToString().Trim() + "</b> ");
                sb.Append("<br> Expected date of work completion : <b>" + dt.Rows[0][6].ToString().Trim() + "</b> ");
                sb.Append("<br> DA Rate : <b>" + dt.Rows[0][7].ToString().Trim() + "</b> ");
                sb.Append("<br> TA Rate : <b>" + dt.Rows[0][8].ToString().Trim() + "</b> ");
                sb.Append("<br> Instrution to Engineer : <b>" + dt.Rows[0][9].ToString().Trim() + "</b> ");
                sb.Append("<br> To & fro loading for : <b>" + dt.Rows[0][10].ToString().Trim() + "</b> ");
            }
            else
            {
                sb.Append("<br> <b>" + uname + "</b> has booked a Travel Ticket for  <b>" + col1 + "</b> by <b>" + col2 + "</b> to <b>" + col3 + "</b> ");
                if (col5 == col6)
                    sb.Append("<br> One way on : <b>" + col5 + "</b> ");
                else
                    sb.Append("<br> Travel Period : <b>" + col5 + "</b> to <b>" + col6 + "</b> ");
                if (col4 == "NO")
                    sb.Append("<br> Tentative Clients to be covered :  <b>" + col7 + "</b> has been " + mail_txt + " due to <b>" + reason + "</b> ");
                else
                    sb.Append("<br> Tentative Clients to be covered :  <b>" + col7 + "</b> has been " + mail_txt + "");
            }
            sb.Append("<br> One file attached. <br>");
            sb.Append("<br>Requested you to please click link given below to login Tejaxo ERP");


            xmltag = fgen.GetXMLTag("mailip").ToUpper();
            string[] mvar = xmltag.Split('@');
            mailpath = mvar[0].ToString().Trim();
            mailport = mvar[1].ToString().Trim();


            sb.Append("<br><b><a href ='http://" + mailpath + ":" + mailport + "/aspnet_client/'>Tejaxo ERP Link</b></a>");
            sb.Append("<br>");

            sb.Append("<br>Thanks & Regards,");
            branchname = "";
            //branchname = fgen.Getfirmname(co_cd, mbr);///old
            branchname = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='B' and type1='" + frm_mbr + "'", "name");
            sb.Append("<br>" + branchname + "</b>");

            //  presentdate = DateTime.Parse(fgen.InserTime(vardate), AustralianDateFormat);

            col3 = Convert.ToString(presentdate);
            col4 = col3.Substring(11, col3.Length - 11);

            sb.Append("<br>Date :  " + col3.Substring(0, 10) + "");
            sb.Append("<br>Time :  " + col4 + "");

            sb.Append("<br><br><br>");
            sb.Append("</body></html>");

            dt = new DataTable();

            send_crystal_rpt(fstr);

            dt = new DataTable();
            if (pageid == "40201" || pageid == "40204a")
            {
                rptpath = ""; xmlpath = "";

                ds = new DataSet();
                da = new OracleDataAdapter("select a.col1,a.remarks,a.invdate,a.docdate,b.col17 as f1 from scratch a,scratch2 b where a.type='CH' and b.type='ED' and a.col46=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'DDMMYYYY') and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')='" + fstr + "'", con);
                da.Fill(ds, "Prepcur");

                xmlpath = Server.MapPath("~/xmlfile/srvreport.xml");
                ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);

                rptpath = "~/Report/srvreport.rpt";
                CrystalDecisions.CrystalReports.Engine.ReportDocument report;
                report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rptfilepath = Server.MapPath("" + rptpath + "");
                report.Load(rptfilepath);
                report.SetDataSource(ds);
                CRV1.ReportSource = report;
                CRV1.DataBind();

                oStream1 = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                //mflag = fgen.SendMail(co_cd, sendtoemail, sb.ToString(), subject, "Ticket_no_" + fstr.Substring(4, 6), co_cd + ":Tour Advance Creation", dt, uname, oStream, oStream1);//old
                mflag = fgen.send_mail(frm_cocd, sendtoemail, sb.ToString(), "", "", "booking_no_" + fstr.Substring(4, 6), frm_cocd + ":Tarveller Booking Creation", "");//its dummy for remov erroronly
            }
            else
                //mflag = fgen.SendMail(co_cd, sendtoemail, sb.ToString(), subject, "booking_no_" + fstr.Substring(4, 6), co_cd + ":Tarveller Booking Creation", dt, uname, oStream, null);
                mflag = fgen.send_mail(frm_cocd, sendtoemail, sb.ToString(), "", "", "booking_no_" + fstr.Substring(4, 6), frm_cocd + ":Tarveller Booking Creation", "");//its dummy for remov erroronly

        }
        else mflag = "No Email id exist in database for sending mail of Tarveller Booking";

        return mflag;
    }
    public string send_mail(string mflag, string gstr, string fstr, string reason)
    {
        app_col = ""; app_level = ""; app_txt = ""; mail_txt = ""; app_flag = ""; app_status = "";

        if (pageid == "40106")
        {
            app_col = "col39";
            app_level = "Verified";

        }
        if (pageid == "40107")
        {
            app_col = "col37";
            app_level = "Approved";
        }

        dt5 = new DataTable();
        da = new OracleDataAdapter("select distinct " + app_col + "  from scratch where branchcd = '" + mbr + "' and type='RQ'   ", con);
        da.Fill(dt5);

        foreach (DataRow dr in dt5.Rows)
        {
            app_txt = dr[0].ToString();
        }


        col1 = ""; col2 = ""; col3 = ""; col4 = "";

        string[] gval = gstr.Split(',');

        col1 = gval[0].ToString().Trim();
        col2 = gval[1].ToString().Trim();
        col3 = gval[2].ToString().Trim();
        col4 = gval[3].ToString().Trim();

        dt = new DataTable();

        da = new OracleDataAdapter("select nvl(emailID,'-') as emailID FROM evas where upper(trim(username)) in ('" + app_txt + "','" + uname + "','" + col3 + "')", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            sendtoemail = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (sendtoemail.Length > 0)
                    sendtoemail += ",";
                if (dr["emailid"].ToString().Trim().Length > 4)
                    sendtoemail += dr["emailid"].ToString().Trim();
            }

            //  sendtoemail = fgen.checkemail(sendtoemail);



            if (col4 == "OK") mail_txt = "HAS BEEN " + app_level.ToUpper() + ". ";
            if (col4 == "NO") mail_txt = "HAS NOT BEEN " + app_level.ToUpper() + ". ";

            subject = " Tejaxo ERP: ORDER REQ NO. " + col1 + " DATED " + col2 + " " + mail_txt;

            sb = new StringBuilder();
            sb.Append("<html><body>");

            fullname = "";
            fullname = fgen.seek_iname(frm_qstr, frm_cocd, "select NVL(full_name,'-') AS NAME FROM EVAS WHERE UPPER(TRIM(USERNAME))='" + col3 + "'", "NAME");

            if (fullname == "" || fullname == "0") fullname = col3;

            fullname1 = "";
            fullname1 = fgen.seek_iname(frm_qstr, frm_cocd, "select NVL(full_name,'-') AS NAME FROM EVAS WHERE UPPER(TRIM(USERNAME))='" + uname + "'", "NAME");

            if (fullname1 == "" || fullname1 == "0") fullname1 = uname;

            if (pageid == "40106")
            {
                if (col4 == "OK")
                    sb.Append("<br>Order Requisition No. <b>" + col1 + "</b> created by <b>" + fullname + "</b> dated <b>" + col2 + "</b> has been verified by <b>" + fullname1 + "</b> that requires final approval.<br>");
            }
            if (pageid == "40107")
            {
                if (col4 == "OK")
                    sb.Append("<br>Order Requisition No. <b>" + col1 + "</b> raised by <b>" + fullname + "</b> dated <b>" + col2 + "</b> has been verified and Approved by <b>" + fullname1 + "</b>. Please Initiate with Performa Invoice.<br>");
            }
            if (col4 == "NO")
                sb.Append("<br>Order Requisition has not been " + app_level + "<br>");

            branchname = "";
            branchname = (string)Session["BRNAME"];

            xmltag = fgen.GetXMLTag("mailip").ToUpper();
            string[] mvar = xmltag.Split('@');
            mailpath = mvar[0].ToString().Trim();
            mailport = mvar[1].ToString().Trim();

            if (pageid == "40106" && col4 == "OK")
            {
                sb.Append("<br>Requested you to please click link given below to approve and reject order requisition");
                sb.Append("<br><b><a href ='http://" + mailpath + ":" + mailport + "/aspnet_client/Default.aspx?@WEB_STATUS=@APP" + "^" + frm_cocd + "^" + app_txt + "^" + gstr + "^" + reason + "^" + branchname + "&FSTR=" + fstr + "'>Approve</b></a>   ||   ");
                sb.Append("<b><a href ='http://" + mailpath + ":" + mailport + "/aspnet_client/Default.aspx?@WEB_STATUS=@UAPP" + "^" + frm_cocd + "^" + app_txt + "^" + gstr + "^" + reason + "^" + branchname + "&FSTR=" + fstr + "'>Reject</b></a>");
            }
            else
            {
                sb.Append("<br>Requested you to please click link given below to login Finsys CRM");
                sb.Append("<br><b><a href ='http://" + mailpath + ":" + mailport + "/aspnet_client/'>CRM Link</b></a>");
            }
            sb.Append("<br>");
            sb.Append("<br>Thanks & Regards,");

            sb.Append("<br>" + branchname + "</b>");

            //  presentdate = DateTime.Parse(fgen.InserTime(vardate), AustralianDateFormat);
            //  presentdate = vardate;
            col3 = Convert.ToString(presentdate);
            col4 = col3.Substring(11, col3.Length - 11);

            sb.Append("<br>Date :  " + col3.Substring(0, 10) + "");
            sb.Append("<br>Time :  " + col4 + "");

            sb.Append("<br><br><br>");
            sb.Append("</body></html>");


            if (Convert.ToInt32(mbr) < 3) compnay_code = "SHOP";
            else compnay_code = "SNPX";

            dt = new DataTable();
            app_level = "";

            if (pageid == "40106")
            {
                send_crystal_rpt(fstr);
                app_level = "Verification";
            }

            if (pageid == "40107")
                app_level = "Approval";

            dt = new DataTable();
            // mflag = fgen.SendMail(co_cd, sendtoemail, sb.ToString(), subject, "", compnay_code + ":Order Req. " + app_level + "", dt, uname, oStream, null);//old
            mflag = fgen.send_mail(frm_cocd, sendtoemail, sb.ToString(), "", "", "booking_no_" + fstr.Substring(4, 6), frm_cocd + ":Tarveller Booking Creation", "");//its dummy for remov erroronly


        }
        else mflag = "No Email id exist in database for sending mail of " + app_level + "";

        return mflag;
    }
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
        sg1_dt.Columns.Add(new DataColumn("chkok", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("chkno", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("txtdate", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("txttout", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("txtrsn", typeof(string)));

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
        sg1_dt.Columns.Add(new DataColumn("sg1_f12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f13", typeof(string)));       //for filepath
    }
    public void sg1_add_blankrows()
    {
        if (sg1_dr == null) create_tab();
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

        sg1_dr["chkok"] = "-";
        sg1_dr["chkno"] = "-";

        sg1_dr["txtdate"] = "-";
        sg1_dr["txttout"] = "-";
        sg1_dr["txtrsn"] = "-";

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
        sg1_dr["sg1_f12"] = "-";
        sg1_dr["sg1_f13"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
    public void GetLeadData(string scode, string reason, string rdate, string cuser)
    {
        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
            cmd = new OracleCommand("update " + frm_tabname + " set app_by='" + appuser + "',app_Dt=to_date('" + vardate + "','dd/mm/yyyy'),reason='" + reason + "',RDate=to_date('" + rdate + "','dd/mm/yyyy') where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        else if (pageid == "40106" || pageid == "40102" || pageid == "40104")
            cmd = new OracleCommand("update " + frm_tabname + " set chk_by='" + appuser + "',chk_Dt=to_date('" + vardate + "','dd/mm/yyyy'),naration='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        else if (pageid == "40107" || pageid == "40105" || pageid == "40101" || pageid == "40101V" || pageid == "40201" || pageid == "40204a" || pageid == "40203" || pageid == "40205a")
        {
            // Mobile No for JSGI            
            otp = "";
            mobileno = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COL23 FROM SCRATCH2 WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY')='" + scode + "'", "COL23");
            //otp = fgen.gen_otp(co_cd);
            fgen.gen_otp(frm_qstr, frm_cocd);
            if ((frm_cocd == "JSGI") && (pageid == "40101" || pageid == "40101V"))
                cmd = new OracleCommand("update " + frm_tabname + " set col28='" + otp + "', app_by='" + appuser + "', app_Dt=SYSDATE,reason='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
            else
                cmd = new OracleCommand("update " + frm_tabname + " set app_by='" + appuser + "',app_Dt=SYSDATE,reason='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        }
        else if (pageid == "40103" || pageid == "40103V")
        {
            cmd = new OracleCommand("update " + frm_tabname + " set app_by='" + appuser + "', COL38='" + reason + "',reason='" + rdate + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);
        }
        else if (pageid == "40108")
            cmd = new OracleCommand("update " + frm_tabname + " set COL24='" + appuser + "',reason='" + reason + "' where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", con);

        cmd.ExecuteNonQuery();

        if (frm_cocd == "SHOP" || frm_cocd == "SNPX") mflag = send_mail(mflag, cuser, scode, reason);
        if (pageid == "40108" || pageid == "40201" || pageid == "40204a") mflag = send_email(mflag, cuser, scode, reason);

        if ((frm_cocd == "JSGI") && (pageid == "40101" || pageid == "40101V"))
        {
            // vipin
            col2 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(col16) as name from scratch2 where BRANCHCD||TYPE||vchnum||to_char(vchdate,'DDMMYYYY') in  ('" + scode + "')", "name");
            //fgen.send_sms(mobileno, "Dear " + col2 + ", Welcome to " + co_cd + ", Please show this OTP " + otp + " at the Gate.");
            //fgen.send_sms2(co_cd, mobileno, "Dear " + col2 + ", Welcome to " + co_cd + ", Please show this OTP " + otp + " at the Gate.");//old
            //fgen.send_sms(frm_qstr, frm_cocd, "9711510126", "Testing Message", frm_uname);
        }
    }
    protected void btnfrom_Click(object sender, ImageClickEventArgs e)
    {
        hfbtnmode.Value = "FR";
        sseekfunc("");
    }
    protected void btnto_Click(object sender, ImageClickEventArgs e)
    {
        hfbtnmode.Value = "TO";
        sseekfunc("");
    }

    protected void btnmbr_Click(object sender, ImageClickEventArgs e)
    {
        hfbtnmode.Value = "BR";
        sseekfunc("");
    }
    public void OpenMyFile(string fpath, string extension)
    {
        i = 0;
        i = fpath.IndexOf(@"\Uploads");
        fName = fpath.Substring(i, fpath.Length - i);

        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".pdf")
            viewpic(fName);
        else
            viewpic("XXXX");

        if (hfbtnmode.Value == "DI") DownloadFile(fName);
    }

    public void DownloadFile(string filepath)
    {
        //filename = ""; mypath = "";
        //filename = filepath.Remove(0, 9);
        //mypath = Server.MapPath("~" + filepath);
        //Response.Clear();
        //Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
        //Response.ContentType = "application/octet-stream";
        //Response.WriteFile(mypath);
        //Response.Flush();
        //Response.End();
        try
        {
            // string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
            string filePath = Server.MapPath("~" + filepath);
            filePath = @"c:\TEJ_ERP\" + filePath;
            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");//old         
            Session["FileName"] = filepath.Remove(0, 9);
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }

    public void viewpic(string imgpath)
    {
        //Session["MYURL"] = imgpath;
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('Attachment Preview Window','View.aspx','95%','95%');});", true);
        //================new
        //string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));    
        //  ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);

    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    //protected void btnhideF_Click(object sender, EventArgs e)
    //{
    //    scode = ""; sname = ""; seek = "";

    //    if (Request.Cookies["Column1"].Value != null)
    //    {
    //        scode = Request.Cookies["Column1"].Value.ToString().Trim();
    //        scode = scode.Replace("&AMP;", "&").Trim();
    //    }
    //    if (Request.Cookies["Column2"].Value != null)
    //    {
    //        sname = Request.Cookies["Column2"].Value.ToString().Trim();
    //        sname = sname.Replace("&AMP;", "&").Trim();
    //    }
    //    if (Request.Cookies["Column3"].Value != null)
    //    {
    //        seek = Request.Cookies["Column3"].Value.ToString().Trim();
    //        seek = seek.Replace("&AMP;", "&").Trim();
    //    }
    //    btnmode = hfbtnmode.Value;
    //    con.Open();

    //    switch (btnmode)
    //    {
    //        case "FR":
    //            txtfrom.Text = sname;
    //            break;
    //        case "TO":
    //            txtto.Text = sname;
    //            break;
    //        case "BR":
    //            if (smbr.Contains(scode)) { }
    //            else
    //            {
    //                if (Convert.ToInt32(scode) < 3)
    //                    AlertMsg("AMSG", "Please select sun impex branch location");
    //                else
    //                    AlertMsg("AMSG", "Please select Shimla branch location");

    //                txtbname.BorderColor = Color.Red;
    //                return;
    //            }
    //            txtbcode.Text = scode;
    //            txtbname.Text = sname;
    //            break;
    //        default:
    //            if (btnmode == "VI" || btnmode == "DI")
    //            {
    //                if (pageid == "46101" || frm_cocd == "MMC")
    //                {
    //                    i = 0;
    //                    fName = ""; fpath = ""; extension = "";

    //                    dt = new DataTable();
    //                    da = new OracleDataAdapter("select filepath,filetype from filetable where branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') = '" + scode + "' and trim(filename)='" + sname + "'", con);
    //                    da.Fill(dt);
    //                    fpath = dt.Rows[0][0].ToString().Trim();
    //                    extension = dt.Rows[0][1].ToString().Trim();
    //                    OpenMyFile(fpath, extension);
    //                }

    //            }
    //            break;
    //    }
    //    con.Close();
    //    if (btnmode == "FR" || btnmode == "TO")
    //    {
    //        bindval();
    //    }
    //}
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
                //  fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                // fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                //  fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                // fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                // clearctrl(); 
                fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                //  make_qry_4_popup();
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
                case "FR":
                    txtfrom.Text = sname;
                    break;
                case "TO":
                    txtto.Text = sname;
                    break;
                case "BR":
                    if (smbr.Contains(scode)) { }
                    else
                    {
                        if (Convert.ToInt32(scode) < 3)
                            AlertMsg("AMSG", "Please select sun impex branch location");
                        else
                            AlertMsg("AMSG", "Please select Shimla branch location");

                        txtbname.BorderColor = Color.Red;
                        return;
                    }
                    txtbcode.Text = scode;
                    txtbname.Text = sname;
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
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

                case "TICODE":
                    if (col1.Length <= 0) return;
                    break;

                default:
                    if (btnmode == "VI" || btnmode == "DI")
                    {
                        if (pageid == "46101" || frm_cocd == "MMC")
                        {
                            i = 0;
                            fName = ""; fpath = ""; extension = "";

                            dt = new DataTable();
                            da = new OracleDataAdapter("select filepath,filetype from filetable where branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') = '" + scode + "' and trim(filename)='" + sname + "'", con);
                            da.Fill(dt);
                            fpath = dt.Rows[0][0].ToString().Trim();
                            extension = dt.Rows[0][1].ToString().Trim();
                            OpenMyFile(fpath, extension);
                        }

                    }
                    break;
                case "FILE":
                case "DWN":
                    if (col1.Contains("~"))
                    {
                        if (col1.Split('~')[1] == "NO")
                        {
                            fgen.msg("Download not allowed.", "AMSG", "This file is restricted from download/view.");
                            return;
                        }
                        else
                        {
                            string filePath = col1.Split('~')[0];
                            if (hffield.Value == "DWN")
                            {
                                try
                                {
                                    Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                                    Session["FileName"] = filePath;
                                    Response.Write("<script>");
                                    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                                    Response.Write("</script>");
                                }
                                catch { }
                            }
                            else
                            {
                                filePath = filePath.Replace("\\", "/").Replace("UPLOAD", "");
                                //filePath = Server.MapPath(@"../tej-base/Upload/" + filePath);
                                filePath = "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "");
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + filePath + "','90%','90%','Finsys Viewer');", true);
                            }
                        }
                    }
                    break;
            }
        }
    }

    public void bindval()
    {
        condition = "";
        ViewState["SSQUERY"] = null; query = "";

        if ((pageid == "40101" || pageid == "40101V" || pageid == "40201" || pageid == "40204a" || pageid == "40205a" || pageid == "40203") || (frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102"))
            condition = "and nvl(trim(app_by),'-') = '-'";
        else if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40103"))
            condition = " and SUBSTR(trim(app_by),1,3) = '[T]' ";
        else if (pageid == "40102" || pageid == "40104" || pageid == "40106")
            condition = "and nvl(trim(a.chk_by),'-') = '-' and nvl(trim(a.app_by),'-') = '-'";
        else if (pageid == "40103*" || pageid == "40103V")
            condition = "and nvl(app_by,'-') = '-'";
        else if (pageid == "40103" || pageid == "40105" || pageid == "40107")
            condition = "and nvl(trim(a.app_by),'-') = '-' ";
        else if (pageid == "40108")
            condition = "and trim(col24) = 'WAITING'";

        switch (pageid)
        {
            case "F55165":
                // query = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dtype as drawing_type,dno as drawing_no,rno as revision_no,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where BRANCHCD='"+frm_mbr+"' AND type='DE' and  vchdate " + DateRange + " order by dno ,rno  ";
                // Update - vipin
                query = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dtype as drawing_type,dno as drawing_no,rno as revision_no,T9 AS DESIGN_TYPE,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD,filepath,filename from WB_DRAWREC  where branchcd='" + frm_mbr + "' and type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + DRTYP + " order by dno ,rno  ";
                query = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,a.dtype as drawing_type,a.dno as drawing_no,a.rno as revision_no,a.T9 AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD,'-' filepath,'-' filename,B.ANAME AS CUSTOMER,c.INAME,TRIM(c.CPARTNO) AS CPARTNO,a.acode,a.COL1 from WB_DRAWREC a,FAMST B,ITEM C where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.branchcd='" + frm_mbr + "' and A.type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + DRTYP + " order by VDD DESC,A.VCHNUM ";
                query = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,a.dtype as drawing_type,a.dno as drawing_no,a.rno as revision_no,a.dtype AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD,'-' filepath,'-' filename,B.aNAME CUSTOMER,c.aNAME INAME,TRIM(c.cpartno) AS CPARTNO,trim(c.cdrgno) as modal_no,a.acode,a.COL1 from WB_DRAWREC a,FAMST B,ITEM C where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.branchcd='" + frm_mbr + "' and A.type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + DRTYP + " order by VDD DESC,A.VCHNUM ";
                if (wSeriesControl == "Y")
                    query = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,a.dtype as drawing_type,a.dno as drawing_no,a.rno as revision_no,a.dtype AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD,'-' filepath,'-' filename,B.NAME CUSTOMER,c.NAME INAME,TRIM(c.ACREF2) AS CPARTNO,trim(c.acref) as modal_no,a.acode,a.COL1 from WB_DRAWREC a,TYPEGRP B,TYPEGRP C where TRIM(A.ACODE)=TRIM(B.TYPE1) AND B.ID='C1' AND TRIM(A.ICODE)=TRIM(C.TYPE1) AND C.ID='P1' AND A.branchcd='" + frm_mbr + "' and A.type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + DRTYP + " order by VDD DESC,A.VCHNUM ";
                break;
            case "F55166":                
                query = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,a.dtype as drawing_type,a.dno as drawing_no,a.rno as revision_no,a.dtype AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD,'-' filepath,'-' filename,B.ANAME CUSTOMER,c.INAME INAME,TRIM(c.CPARTNO) AS CPARTNO,a.acode,a.COL1 from WB_DRAWREC a,famst B,ITEM C where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.branchcd='" + frm_mbr + "' and A.type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + DRTYP + " and trim(a.vchnum) in (select distinct trim(mrrnum) as fstr FROM OM_DRWG_MAKE where usercode='" + frm_UserID + "' and branchcd='" + frm_mbr + "' and type='IV' and vchnum<>'000000' ) order by VDD DESC,A.VCHNUM ";
                if (wSeriesControl == "Y")
                    query = "SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,a.dtype as drawing_type,a.dno as drawing_no,a.rno as revision_no,a.dtype AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD,'-' filepath,'-' filename,B.NAME CUSTOMER,c.NAME INAME,TRIM(c.ACREF2) AS CPARTNO,a.acode,a.COL1 from WB_DRAWREC a,TYPEGRP B,TYPEGRP C where TRIM(A.ACODE)=TRIM(B.TYPE1) AND B.ID='C1' AND TRIM(A.ICODE)=TRIM(C.TYPE1) AND C.ID='P1' AND A.branchcd='" + frm_mbr + "' and A.type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + DRTYP + " and trim(a.vchnum) in (select distinct trim(mrrnum) as fstr FROM OM_DRWG_MAKE where usercode='" + frm_UserID + "' and branchcd='" + frm_mbr + "' and type='IV' and vchnum<>'000000' ) order by VDD DESC,A.VCHNUM ";
                break;
            case "51103":
                query = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as Trial_Ref,t1 as pattern_no,t3 as revision,t2 as Trial_No_of_pattern,to_Char(vchdate,'dd/mm/yyyy') as Start_Dt,TO_CHAR(to_Date(t10,'dd/mm/yyyy'),'dd/mm/yyyy') as Target_dt,t42 as Completion_Dt,round( TO_DATE(t42,'dd/mm/yyyy')-to_Date(t10,'dd/mm/yyyy')) as delay_days,t41 as Trial_Result from DRAWREC  where branchcd='" + frm_mbr + "' and type='TN' and trim(nvl(edflag,'-'))='Y' /*and vchdate " + daterange + "*/ " + DRID + DRTYP + " order by vchnum ";
                break;
            case "60412":
                query = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as Quality_Plan_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,dno as Order_NO,t1 as pattern_no,t6 as Heat_No,t42 as Completion_Dt from DRAWREC where branchcd='" + frm_mbr + "' and type='MQ' /*and vchdate " + DateRange + "*/ and length(trim(t42))>2 order by vchnum ";
                break;
            case "51503":
                query = "SELECT distinct a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.vchnum as Method_Card_no,to_cHAR(a.VCHDATE,'DD/mm/yyyy') as Method_card_date, a.invno as Trial_Ref,to_cHAR(a.invDATE,'DD/mm/yyyy') as Trial_date,a.t2 as Pattern_no,a.t3 as Revision,a.t5 as Trial_no_of_Pattern,b.t42 as completion_dt,A.ENT_BY,TO_CHAR(a.ent_DT,'DD/MM/YYYY HH:MI AM') AS ENT_dT,to_char(a.vchdate,'YYYYMMDD') AS VDD from DRAWREC a,drawrec b where trim(a.finvno)=b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'ddmmyyyy') and a.branchcd ='" + mbr + "' and a.type='MC' /*and a.vchdate " + daterange + "*/ order by VDD desc,a.vchnum desc ";
                break;
            case "60110a":
                query = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DDMMYYYY') AS FSTR,A.VCHNUM AS MOULD_PLAN_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MOUDL_PL_DT,A.DNO AS ORDERNO,A.T1 AS PATTERNO,A.FINVNO AS MOULD_BOX FROM DRAWREC A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='MB' AND A.VCHDATE " + DateRange + " ORDER BY A.VCHNUM desc";
                break;
            case "60113":
                query = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DDMMYYYY')||TRIM(A.DNO) AS FSTR,A.VCHNUM AS plan_NO,a.t42 as completion_Dt,A.DNO AS ORDERNO,A.T1 AS PATTERNO,a.t6 as heat_no,A.T3 AS LQD_RISER_wT,A.T4 AS CASTING_WT,A.FINVNO AS MOULD_BOX,a.t38 as result FROM DRAWREC A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='M1' /*AND A.VCHDATE " + DateRange + "*/ ORDER BY A.VCHNUM desc";
                break;
            case "60213":
                query = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DDMMYYYY')||TRIM(A.DNO) AS FSTR,A.VCHNUM AS charge_NO,a.t42 as completion_Dt,A.DNO AS ORDERNO,A.T1 AS PATTERNO,a.t6 as heat_no,A.FINVNO AS MOULD_BOX,a.t38 as result,a.t9 as KnockOut_dt_plan  FROM WB_DRAWREC A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='P1' /*AND A.VCHDATE " + DateRange + "*/ ORDER BY A.VCHNUM desc";
                break;
            case "60313":
                query = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DDMMYYYY')||TRIM(A.DNO) AS FSTR,A.VCHNUM AS plan_NO,a.t42 as completion_Dt,A.DNO AS ORDERNO,A.T1 AS PATTERNO,a.t6 as heat_no,A.FINVNO AS MOULD_BOX,a.t38 as result,a.tno as Delay_days FROM WB_DRAWREC A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='K1' /*AND A.VCHDATE " + DateRange + "*/ ORDER BY A.VCHNUM desc";
                break;
            case "46101":
                query = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as doc_no,to_char(vchdate,'dd/mm/yyyy') as doc_date,col5 as name,col11 as father_name,col1 as department,col2 as line,col3 as machine,col4 as Last_Working_Designation,col6 as apply_for,num1 as drawn_Salary,num2 as expected_salary,col7 as academic_qualification,col8 as prof_qualification,col9 as specialization,col24 as Last_Working_Company,col14 as current_Addr,remarks as permanent_addr,col18 as location,col23 as marital_status,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from scratch where branchcd ='" + frm_mbr + "' and type='RB' and  vchdate " + daterange + " order by VDD desc,vchnum desc ";
                break;
            case "46103":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dno as drawing_no,rno as revision_no,sno as serial_no,issue_by as drawn_By,to_char(rdate,'dd/mm/yyyy') as target_draw_Date,to_char(ISSUE_date,'dd/mm/yyyy') as actual_draw_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='SF' /*and  vchdate " + DateRange + "*/   order by VDD desc,vchnum desc ";
                break;
            case "47103":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dno as drawing_no,rno as revision_no,sno as serial_no,issue_by as drawn_By,to_char(rdate,'dd/mm/yyyy') as target_draw_Date,to_char(ISSUE_date,'dd/mm/yyyy') as actual_draw_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='IF' /*and  vchdate " + DateRange + " */  order by VDD desc,vchnum desc ";
                break;
            case "52103":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dno as drawing_no,rno as revision_no,sno as serial_no,issue_by as drawn_By,to_char(rdate,'dd/mm/yyyy') as target_draw_Date,to_char(ISSUE_date,'dd/mm/yyyy') as actual_draw_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='WF' /*and  vchdate " + DateRange + "*/   order by VDD desc,vchnum desc ";
                break;
            case "53103":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dno as drawing_no,rno as revision_no,sno as serial_no,issue_by as RUN_By,to_char(rdate,'dd/mm/yyyy') as target_RUN_Date,to_char(ISSUE_date,'dd/mm/yyyy') as actual_RUN_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='MF' /*and  vchdate " + DateRange + "*/   order by VDD desc,vchnum desc ";
                break;
            case "48103":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,invno as edn,to_char(invdate,'dd/mm/yyyy') as edn_date,dtype as inspection_type,dno as drawing_no,sno as serial_no,rno as revision_no, to_char(rdate,'dd/mm/yyyy') as inspection_target_date,to_char(ISSUE_date,'dd/mm/yyyy') as actual_inspection_date,tno as delay_if_any,ISSUE_to as inspector,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='PE'  order by VDD desc,vchnum desc ";
                break;
            case "49103":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,invno as edn,to_char(invdate,'dd/mm/yyyy') as edn_date,dtype as inspection_type,dno as drawing_no,sno as serial_no,rno as revision_no,to_char(rdate,'dd/mm/yyyy') as inspection_target_date,to_char(ISSUE_date,'dd/mm/yyyy') as actual_inspection_date,tno as delay_if_any,ISSUE_to as inspector,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='CE'   order by VDD desc,vchnum desc ";
                break;
            case "43105":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as EDN,to_char(vchdate,'dd/mm/yyyy') as EDN_date,dtype as drawing_type,dno as drawing_no,rno as revision_no,T9 AS DESIGN_TYPE,issue_to,issue_by,to_char(issue_date,'dd/mm/yyyy') as issue_date, to_char(rdate,'dd/mm/yyyy') as  return_target_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD,filepath,filename from WB_DRAWREC  where branchcd ='" + frm_mbr + "' and type='DI' and  vchdate " + DateRange + " order by VDD desc,vchnum desc";
                break;
            case "40101":
            case "40101V":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,col16 as visitor_name,col15 as comp_name,  to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as exp_time, REMARKS as visit_reason,COL17 AS LOCATION,COL19 AS DEPARTMENT,COL21 AS DESIGNATION,COL22 AS VISITOR_TYPE,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD,'-' AS COL1,'-' AS COL2,'-' AS COL3,'-' COL4 from scratch2  where branchcd ='" + frm_mbr + "' and type='VR' and vchdate " + DateRange + " " + condition + " order by VDD desc,vchnum desc ";
                break;
            case "40201":
            case "40204a":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,INVNO as srv_no,to_char(INVdate,'dd/mm/yyyy') as srv_date,col1 as client,col11 as eng_name,num3 as da_rate,num4 as ta_rate,round((num3+num4)*to_number(COL39)) as amount,ent_by,ent_dt, to_Char(docdate,'dd/mm/yyyy') as depu_date,col5 as designation,col52 as department,REMARKS as site_details,COL39 as no_of_Days,col51 as visit_Date,col50 as work_Date,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, to_char(vchdate,'YYYYMMDD') AS VDD from scratch  where branchcd ='" + frm_mbr + "' and type='CH' and vchdate " + DateRange + " " + condition + " order by VDD desc,vchnum desc ";
                break;
            case "40205a":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,INVNO as srv_no,to_char(INVdate,'dd/mm/yyyy') as srv_date,col1 as client,col11 as eng_name,num3 as da_rate,num4 as ta_rate,round((num3+num4)*to_number(COL39)) as amount,ent_by,ent_dt, to_Char(docdate,'dd/mm/yyyy') as depu_date,col5 as designation,col52 as department,REMARKS as site_details,COL39 as no_of_Days,col51 as visit_Date,col50 as work_Date,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, to_char(vchdate,'YYYYMMDD') AS VDD from scratch  where branchcd ='" + frm_mbr + "' and type='CE' and vchdate " + DateRange + " " + condition + " order by VDD desc,vchnum desc ";
                break;
            case "40203":
                query = "select distinct branchcd||type||vchnum||to_char(vchdate,'ddmmyyyy') as fstr,vchnum as SRV_No,TO_CHAR(vchdate,'DD/MM/YYYY') AS SRV_Date,col1 as customer,col2 as equipment,col13 as problem,col11 as eng_depu,col20 as nature,remarks, ent_by as Created_By,to_char(vchdate,'YYYYMMDD') AS VDD from scratch  where branchcd ='" + mbr + "' and type='CC' and vchdate " + daterange + " " + condition + " order by VDD desc,vchnum desc ";
                break;
            case "40108":
                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,decode(num2,0,COL24,NUM2||' '||COL24) as PNR_STATUS,COL1 AS EMP_NAME,COL7 AS EMP_DEPT,COL8 AS EMP_DESIG,COL17 AS FROM_SOURCE,COL19 AS TO_DESTINATION,TO_CHAR(DOCDATE,'DD/MM/YYYY') AS FROM_DATE,TO_CHAR(TODATE,'DD/MM/YYYY') AS TO_DATE,COL21 AS TRAVEL_MODE,COL22 AS PNR_NO,COL28 AS BUKNG_AGNCY,COL23 AS PAYMENT_MODE,COL26 AS PAYMENT_TYPE,COL25 AS CLIENT,NUM1 AS AMOUNT,REMARKS,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from scratch2  where branchcd ='" + frm_mbr + "' and type='TB' and trim(ent_By) like '" + acessuser + "%' " + condition + " order by VDD desc,vchnum desc ";
                break;
            case "42515":
                query = "SELECT DISTINCT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY') AS FSTR,VCHNUM AS ENTRY_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENTRY_DT,COL1 AS DRAWING_NO,COL2 AS REVISION_NO,COL3 AS HEAT_NO,COL13 AS PRESENT FROM SCRATCH WHERE BRANCHCD='" + mbr + "' AND TYPE='PC' ORDER BY ENTRY_NO DESC";
                query = "SELECT DISTINCT TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DDMMYYYY') AS FSTR,VCHNUM AS ENTRY_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS ENTRY_DT,COL1 AS DRAWING_NO,COL2 AS REVISION_NO,COL3 AS HEAT_NO,COL4 AS STATUS,COL13 AS PRESENT FROM SCRATCH WHERE BRANCHCD='" + mbr + "' AND TYPE='PC' ORDER BY ENTRY_NO DESC";
                break;
            default:
                if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
                    query = "select DISTINCT a.BRANCHCD||a.TYPE||a.vchnum||to_char(a.vchdate,'DDMMYYYY') as fstr,a.VCHNUM AS RFQno,a.PERSON AS CONTact_PERSON,b.email,a.STAGE AS STAGE,a.PRIORITY AS PRIORITY,a.LSRC AS RFQ_source,a.ASSG AS ASSignee,b.PADDR2 as Country,b.PADDR6 as Region,b.website,a.ent_By as created_by, to_char(A.vchdate,'YYYYMMDD') AS VDD FROM LEADMST a,contmst b  where  trim(a.acode)=trim(b.acode) and a.type='GL' and a.branchcd='" + frm_mbr + "' and (upper(trim(a.ASSG)) like '" + txtfrom.Text.Trim() + "%' or upper(trim(a.person)) like '" + txtto.Text.Trim() + "%') " + condition + " order by VDD desc,a.vchnum desc ";
                else if (pageid == "40106" || pageid == "40107")
                    query = "select DISTINCT b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'DDMMYYYY') as fstr, b.vchnum as reqno,to_char(b.vchdate,'dd/mm/yyyy') as reqdt, A.vchnum as enqno,to_char(A.vchdate,'dd/mm/yyyy') as enqDt,a.cyname as buyer_name,d.aname as seller_name,b.col23 as prod_group,a.psgrp as prod_name,a.QTY,a.unit,b.val3 as buying_price,b.val18 as selling_price,b.val19 as margin,b.val23 as margin_amt,a.assg, b.ent_by as enby,b.ent_dt as endt,B.CHK_BY,B.APP_BY, A.cname as branch_name,to_char(B.vchdate,'YYYYMMDD') AS VDD FROM LEADMST A, scratch b,CONTMST C,famst d where trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=(b.invno)||to_char(a.invdate,'dd/mm/yyyy') and TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(b.ACODE)=TRIM(d.ACODE) and a.type='GL' AND B.TYPE='RQ' " + condition.Replace("a.", "b.") + " AND TRIM(A.PSGRP)||A.QTY=TRIM(B.FPSGRP)||B.FQTY  order by VDD desc,B.vchnum desc ";
                else if (pageid == "40103*" || pageid == "40103V")
                    query = "SELECT DISTINCT branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no, ent_dt as entry_date,col16 as visitor_name,col15 as comp_name,col17 as location,col12 as purpose,col19 as department,col21 as designation,to_char(docdate,'dd/mm/yyyy') as last_visited_on,col23 as mobile,col26 as mfg,col29 as serial_no,acode as empid,col1 as name,COL7 as emp_dept,COL8 as emp_desig,REMARKS,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD FROM scratch2 where type='VM' and branchcd ='" + frm_mbr + "' and vchdate " + DateRange + " " + condition + " order by VDD desc";
                else if (pageid == "40102" || pageid == "40103")
                    query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as req_no,to_char(vchdate,'dd/mm/yyyy') as req_date, col14 as client_name,col13 as item_name,icode as item_code,acode as client_code,to_char(docdate,'dd/mm/yyyy') as delivery_date,col16 as design_code,col17 as prod_Code,num2 as order_qty,num3 as req_qty,ent_by,ent_dt,to_char(vchdate,'YYYYMMDD') AS VDD from scratch2  where branchcd ='" + frm_mbr + "' and type='VM' and trim(ent_By) like '" + acessuser + "%' " + condition.Replace("a.", "") + " order by VDD desc,vchnum desc ";
                else if (pageid == "40104" || pageid == "40105")
                    query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as pi_no,to_char(vchdate,'dd/mm/yyyy') as pi_date, col14 as client_name,col13 as item_name,icode as item_code,acode as client_code,to_char(docdate,'dd/mm/yyyy') as dated,col16 as design_code,col17 as prod_Code,num2 as width,num3 as length,num4 as price,ent_by,ent_dt,to_char(vchdate,'YYYYMMDD') AS VDD from scratch2  where branchcd ='" + frm_mbr + "' and type='PI' and trim(ent_By) like '" + acessuser + "%' " + condition.Replace("a.", "") + " order by VDD desc,vchnum desc ";
                break;
        }

        GridView1.DataSource = sg1_dt;
        GridView1.DataBind();
        ViewState["SSQUERY"] = query;
        BindData(query);
    }
    public void crystal_rpt()
    {
        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
            rptpath = "~/Report/RFQ.rpt";
        else if (pageid == "40106" || pageid == "40107")
        {
            if (Convert.ToInt32(mbr) < 3)
                rptpath = "~/Report/BAR_SHOP.rpt";
            else
                rptpath = "~/Report/BAR_SNPX.rpt";
        }
        else if (pageid == "40101" || pageid == "40101V")
            rptpath = "~/Report/vreq.rpt";
        else if (pageid == "40203" || pageid == "40201" || pageid == "40204a" || pageid == "40205a")
            rptpath = "~/Report/cc_req.rpt";
        else if (pageid == "40103" && (frm_cocd == "JSGI" || frm_cocd == "DLJM" || frm_cocd == "SDM"))
            rptpath = "~/Report/vmrec.rpt";
        else if (pageid == "40102" || pageid == "40103")
            rptpath = "~/Report/sreq.rpt";
        else if (pageid == "40104" || pageid == "40105")
            rptpath = "~/Report/pinv.rpt";
        else if (pageid == "40108")
            rptpath = "~/Report/tbook.rpt";
        else if (pageid == "51503") rptpath = "~/Report/mcard.rpt";
        else if (pageid == "60110a") rptpath = "~/Report/mplan.rpt";
        else if (pageid == "60113") rptpath = "~/Report/mplan_c.rpt";
        else if (pageid == "60213") rptpath = "~/Report/pplan_c.rpt";
        else if (pageid == "60313") rptpath = "~/Report/kplan_c.rpt";

        Response.Cookies["rptfile"].Value = rptpath;

        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rptfilepath = Server.MapPath("" + rptpath + "");
        report.Load(rptfilepath);
        report.SetDataSource(ds);
        CRV1.ReportSource = report;
        CRV1.DataBind();
        oStream = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
    }
    public void send_crystal_rpt(string scode)
    {
        if (pageid != "51503" && pageid != "60110a" && pageid != "60113" && pageid != "60213" && pageid != "60313")
        {
            ds = new DataSet();

            if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
                da = new OracleDataAdapter("select DISTINCT a.* ,b.remaRKS,a.person as fname,c.mobile,c.email as pemail,C.PADDR1,C.PADDR2,C.PADDR3,C.PADDR4,C.PADDR5,C.PADDR6 from " + frm_tabname + " a left outer join CONTMST C on TRIM(A.ACODE)=TRIM(c.ACODE), description b where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'DDMMYYYY') and a.branchcd = '" + mbr + "' and a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')='" + scode.Substring(2, 16) + "' order by a.srno", con);
            else if (pageid == "40106" || pageid == "40107")
                da = new OracleDataAdapter("select  DISTINCT b.INAME,b.INAME AS INAME1, b.col17,b.col19,b.col37,b.col39, B.CHK_BY,B.APP_BY,b.val23,B.VAL24,B.VAL25,B.VAL26,B.VAL27,B.VAL28,B.VAL29,B.VAL30,B.VAL31,B.VAL32,B.VAL33,B.VAL34,B.VAL35,B.VAL36,B.VAL37,B.VAL38,B.VAL39,B.VAL40,B.VAL41,B.VAL42,B.VAL43,B.VAL44,B.VAL45,B.VAL46,B.VAL47,b.val48, B.RMK1,B.RMK2,B.RMK3,B.RMK4,B.RMK5,B.RMK6,B.RMK7,B.RMK8,B.RMK9,B.RMK10,B.RMK11,B.RMK12,B.RMK13,B.RMK14,B.RMK15,B.RMK16,B.RMK17,B.RMK18,B.RMK19, b.col20,b.vchnum as reqno,to_char(b.vchdate,'dd/mm/yyyy') as reqdt, b.srno AS SNO,b.val22,b.col15,b.col16, b.col23,b.COL2,b.COL3,b.COL4,b.COL6,b.COL7,b.COL8,b.COL9,b.COL10,b.col40, b.COL12, A.vchnum,A.vchdate,A.cname,A.assg,A.cyname,A.person,C.mobile,'-' as pterm, b.acode as code, b.vchnum as dno,to_char(b.vchdate,'dd/mm/yyyy') as docdt,b.ent_by as enby,b.ent_dt as endt,b.edt_by as edby,b.edt_dt as eddt,b.col5,b.col18, b.col11,b.col24,b.col25,b.col31,b.val1,b.val2,b.val3,b.val4,b.val5,b.val6,b.val7,b.val8,b.val9,b.val10,b.val11,b.val12,b.val13,b.val14,b.val15,b.val16,b.val17,b.val18,b.val19,b.val20,b.val21, 0 as srno,'-' as PSGRP,0 as QTY,'-' as UNIT,0 as QRATE,'-' as QCURR,0 as qval,0 as camt,'-' as chk_dept,'-' as app_dept,'-' as ent_dept FROM LEADMST A, scratch b,CONTMST C where TRIM(A.ACODE)=TRIM(C.ACODE) AND a.vchnum||to_char(a.vchdate,'DDMMYYYY')=b.invno||to_char(b.invdate,'DDMMYYYY') and  b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'DDMMYYYY')='" + scode + "' order by b.srno  ", con);
            else if (pageid == "40102" || pageid == "40103" || pageid == "40104" || pageid == "40105" || pageid == "40101" || pageid == "40101V" || pageid == "40108" || pageid == "40201" || pageid == "40204a" || pageid == "40203" || pageid == "40205a")
                da = new OracleDataAdapter("select * from " + frm_tabname + "  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'", con);

            da.Fill(ds, "Prepcur");

            //  ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");
            ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr));
            if (pageid == "40106" || pageid == "40107")
                //ds = fgen.Type_Data(co_cd, mbr, ds, "PARTY", ds.Tables[0].Rows[0]["code"].ToString().Trim());
                ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr));
            else if (pageid == "40203" || pageid == "40201" || pageid == "40204a" || pageid == "40205a")
                //ds = fgen.Type_Data(co_cd, mbr, ds, "PARTY", ds.Tables[0].Rows[0]["acode"].ToString().Trim());
                ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr));
            else
                // ds = fgen.Type_Data(co_cd, mbr, ds, "PARTY", ""); 
                ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr));

            if (pageid == "40106" || pageid == "40107")
            {
                dt = new DataTable();
                da = new OracleDataAdapter("select DISTINCT A.SRNO,A.PSGRP,A.QTY,A.UNIT,A.QRATE,A.QCURR,A.qval,0 as camt,NVL(A.iname,'-') AS iname FROM leadmst A,SCRATCH B WHERE a.vchnum||to_char(a.vchdate,'DDMMYYYY')=b.invno||to_char(b.invdate,'DDMMYYYY') AND TRIM(A.vchnum)||to_char(A.vchdate,'ddmmyyyy')='" + ds.Tables[0].Rows[0]["vchnum"].ToString().Trim() + ds.Tables[0].Rows[0]["vchdate"].ToString().Trim().Substring(0, 10).Replace("/", "") + "' AND TRIM(A.PSGRP)||A.QTY=TRIM(B.FPSGRP)||B.FQTY  order by A.srno ", con);

                da.Fill(dt);

                i = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    i = Convert.ToInt32(dr["SNO"].ToString().Trim()) - 1;
                    dr["srno"] = dt.Rows[i]["srno"].ToString().Trim();
                    dr["PSGRP"] = dt.Rows[i]["PSGRP"].ToString().Trim();
                    dr["QTY"] = dt.Rows[i]["QTY"].ToString().Trim();
                    dr["UNIT"] = dt.Rows[i]["UNIT"].ToString().Trim();
                    dr["QRATE"] = dt.Rows[i]["QRATE"].ToString().Trim();
                    dr["QCURR"] = dt.Rows[i]["QCURR"].ToString().Trim();
                    dr["qval"] = dt.Rows[i]["qval"].ToString().Trim();
                    dr["camt"] = dt.Rows[i]["camt"].ToString().Trim();
                    dr["ent_dept"] = GetData(dr["enby"].ToString().Trim());
                    dr["chk_dept"] = GetData(dr["CHK_BY"].ToString().Trim());
                    dr["app_dept"] = GetData(dr["APP_BY"].ToString().Trim());
                }

            }
        }
        if (pageid == "51503")
        {
            query = "select a.vchnum as mvchnum,a.vchdate as mvchdate,a.invno as minvno,a.invdate as minvdate,a.t1 as mt1,a.t2 as mt2,a.t3 as mt3,a.t4 as mt4,a.t5 as mt5,a.t6 as mt6,a.t42 as mt42,a.ent_by as ment_by,a.ent_Dt as ment_Dt,b.* from DRAWREC a,drawrec b where trim(a.finvno)=b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'ddmmyyyy') and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'DDMMYYYY')='" + scode + "' order by b.dsrno";
            ds = new DataSet();
            da = new OracleDataAdapter(query, con);
            da.Fill(ds, "Prepcur");

            string xmlfile = string.Empty;
            // ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");
            ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr));
            xmlfile = Server.MapPath("~/xmlfile/mcard.xml");
            ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
            Session["mydataset"] = ds;
            Response.Cookies["rptfile"].Value = "~/Report/mcard.rpt";

            headername = "Method Card Print";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
            query = "";
        }
        if (pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313")
        {
            query = "select a.* from drawrec a where a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'ddmmyyyy')||TRIM(A.DNO)='" + scode + "'";
            ds = new DataSet();
            da = new OracleDataAdapter(query, con);
            da.Fill(ds, "Prepcur");

            string xmlfile = string.Empty;
            //  ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");
            ds.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr));
            if (pageid == "60113")
            {
                xmlfile = Server.MapPath("~/xmlfile/mplan_c.xml");
                ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                Session["mydataset"] = ds;
                Response.Cookies["rptfile"].Value = "~/Report/mplan_C.rpt";
                headername = "Molding Plan Print";
            }
            else if (pageid == "60110a")
            {
                xmlfile = Server.MapPath("~/xmlfile/mplan.xml");
                ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                Session["mydataset"] = ds;
                Response.Cookies["rptfile"].Value = "~/Report/mplan.rpt";
                headername = "Molding Plan Print";
            }
            else if (pageid == "60213")
            {
                xmlfile = Server.MapPath("~/xmlfile/pplan_c.xml");
                ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                Session["mydataset"] = ds;
                Response.Cookies["rptfile"].Value = "~/Report/pplan_c.rpt";
                headername = "Pouring Plan Print";
            }
            else if (pageid == "60313")
            {
                xmlfile = Server.MapPath("~/xmlfile/kplan_c.xml");
                ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);
                Session["mydataset"] = ds;
                Response.Cookies["rptfile"].Value = "~/Report/kplan_c.rpt";
                headername = "Knock Out Plan Print";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
            query = "";
        }
        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
            xmlpath = Server.MapPath("~/xmlfile/lead.xml");
        else if (pageid == "40106" || pageid == "40107")
            xmlpath = Server.MapPath("~/xmlfile/bar.xml");
        else if (pageid == "40101" || pageid == "40101V")
            xmlpath = Server.MapPath("~/xmlfile/vreq.xml");
        else if (pageid == "40203" || pageid == "40201" || pageid == "40204a" || pageid == "40205a")
            xmlpath = Server.MapPath("~/xmlfile/cc_req.xml");
        else if (pageid == "40103" && (frm_cocd == "JSGI" || frm_cocd == "DLJM" || frm_cocd == "SDM"))
            xmlpath = Server.MapPath("~/xmlfile/vmrec.xml");
        else if (pageid == "40102" || pageid == "40103")
            xmlpath = Server.MapPath("~/xmlfile/sreq.xml");
        else if (pageid == "40104" || pageid == "40105")
            xmlpath = Server.MapPath("~/xmlfile/pinv.xml");
        else if (pageid == "40108")
            xmlpath = Server.MapPath("~/xmlfile/tbook.xml");
        else if (pageid == "51503") xmlpath = Server.MapPath("~/xmlfile/mcard.xml");
        else if (pageid == "60110a") xmlpath = Server.MapPath("~/xmlfile/mplan.xml");
        else if (pageid == "60113") xmlpath = Server.MapPath("~/xmlfile/mplan_c.xml");
        else if (pageid == "60213") xmlpath = Server.MapPath("~/xmlfile/pplan_c.xml");
        else if (pageid == "60313") xmlpath = Server.MapPath("~/xmlfile/kplan_c.xml");

        ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);
        Session["mydataset"] = ds;

        crystal_rpt();
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    public string GetData(string val)
    {
        col2 = string.Empty;

        dt3 = new DataTable();
        da = new OracleDataAdapter("select erpdeptt from evas where upper(trim(username)) in ('" + val + "')", con);
        da.Fill(dt3);

        dt4 = new DataTable();
        if (dt3.Rows.Count > 0)
        {
            da = new OracleDataAdapter("select replace(name,'&','') as DepartmenT from TYPE where trim(type1) = '" + dt3.Rows[0][0].ToString().Trim() + "' and id='M' and substr(type1,1,1) in('6')", con);
            da.Fill(dt4);
        }
        if (dt4.Rows.Count > 0)
            col2 = dt4.Rows[0][0].ToString().Trim();
        return col2;
    }


    public void callscript()
    {
        ScriptManager.RegisterStartupScript(GridView1, this.GetType(), "jcall", "gridviewScroll();", true);
    }

    protected void LnkBtnd_Click(object sender, EventArgs e)
    {

        clearcontrol();
        col1 = "";

        LinkButton selectButton = (LinkButton)sender;
        GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        col1 = GridView1.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();

        if (frm_cocd == "MMC")
        {
            hfbtnmode.Value = "DI";
            if (pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313")
            {
                col1 = mbr + "TN" + GridView1.Rows[row.RowIndex].Cells[9].Text.Trim() + GridView1.Rows[row.RowIndex].Cells[10].Text.Trim().Replace("/", "");
                hfbtnmode.Value = "VI";
                con.Open();
                send_crystal_rpt(col1);
                con.Close();
                headername = "" + pgname + " Master Print";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
            }
            else
                sseekfunc(col1);
        }

        //string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));       
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);

    }

    protected void LnkBtnv_Click(object sender, EventArgs e)
    {
        clearcontrol();
        col1 = "";

        LinkButton selectButton = (LinkButton)sender;
        GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        col1 = GridView1.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();

        if (pageid == "46101" || frm_cocd == "MMC")
        {
            hfbtnmode.Value = "VI";
            if (pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313")
            {
                col1 = mbr + "TN" + GridView1.Rows[row.RowIndex].Cells[9].Text.Trim() + GridView1.Rows[row.RowIndex].Cells[10].Text.Trim().Replace("/", "");
                hfbtnmode.Value = "DI";
            }
            sseekfunc(col1);
        }
        else
        {
            //con.Open();
            //send_crystal_rpt(col1);
            //con.Close();
            //headername = "" + pgname + " Master Print";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
            //===============
            if (query == "") { }
            else
            {
                Response.Cookies["seeksql"].Value = query;
                Response.Cookies["headername"].Value = headername;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abcd", "$(document).ready(function(){OpenPopup('" + headername + "','rptlevel2.aspx','90%','97%',false);});", true);

            //
            for (i = 0; i < GridView1.Rows.Count - 1; i++)
            {
                //if (((TextBox)GridView1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-")
                if (GridView1.Rows[i].Cells[28].ToString().Trim() != "-" || GridView1.Rows[i].Cells[28].ToString().Trim() != " ")
                {
                    FilePath.Value = GridView1.Rows[i].Cells[28].ToString().Trim();
                }
                // string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
            }
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {                
        if (pageid == "43105")
        {
            #region
            GridView1.Columns[10].HeaderStyle.Width = 35;
            GridView1.Columns[11].HeaderStyle.Width = 25;
            GridView1.Columns[12].HeaderStyle.Width = 25;///is per no checkbox h or 
            /////====================yaha tak ok hai-=====yaha tak thik aata hai
            e.Row.Cells[13].Visible = false;
            e.Row.Cells[14].Visible = false;
            // e.Row.Cells[13].Attributes.Add("dispay", "none");
            // e.Row.Cells[14].Attributes.Add("dispay", "none");

            // GridView1.Columns[15].ItemStyle.Width = 50;
            //e.Row.Cells[15].Width = new Unit("300px");          
            GridView1.Columns[14].HeaderStyle.Width = 50;
            GridView1.Columns[15].HeaderStyle.Width = 90;
            GridView1.Columns[16].HeaderStyle.Width = 120;
            GridView1.Columns[17].HeaderStyle.Width = 100;
            GridView1.Columns[18].HeaderStyle.Width = 125;
            GridView1.Columns[19].HeaderStyle.Width = 100;
            GridView1.Columns[20].HeaderStyle.Width = 90;
            GridView1.Columns[21].HeaderStyle.Width = 70;
            GridView1.Columns[22].HeaderStyle.Width = 70;
            GridView1.Columns[23].HeaderStyle.Width = 140;
            GridView1.Columns[24].HeaderStyle.Width = 110;
            GridView1.Columns[25].HeaderStyle.Width = 70;
            GridView1.Columns[26].HeaderStyle.Width = 70;
            GridView1.Columns[27].HeaderStyle.Width = 130;


            GridView1.Columns[15].HeaderText = "REASON";
            GridView1.Columns[16].HeaderText = "EDN";
            GridView1.Columns[17].HeaderText = "EDN_DATE";
            GridView1.Columns[18].HeaderText = "DRAWING_TYPE";
            GridView1.Columns[19].HeaderText = "DRAWING_NO";
            GridView1.Columns[20].HeaderText = "REVISION_NO";
            GridView1.Columns[21].HeaderText = "DESIGN_TYPE";
            GridView1.Columns[22].HeaderText = "ISSUE_TO";
            GridView1.Columns[23].HeaderText = "ISSUE_BY";
            GridView1.Columns[24].HeaderText = "RETURN_TARGET_DATE";
            GridView1.Columns[25].HeaderText = "ENT_BY";
            GridView1.Columns[26].HeaderText = "ENT_DT";
            GridView1.Columns[27].HeaderText = "VDD";
            e.Row.Cells[28].Visible = false;
            #endregion
        }
        //
        if (pageid == "43105" || pageid == "F55165" || pageid == "F55166" || pageid == "51103" || pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313" || pageid == "60412" || pageid == "42515") { }
        else
            e.Row.Cells[15].Visible = false;

        if ((frm_cocd == "NIRM" || frm_cocd == "PRAG") && (pageid == "40102" || pageid == "40103"))
            e.Row.Cells[3].Visible = true;

        if (pageid == "40106" || pageid == "40107")
            e.Row.Cells[15].Visible = true;

        if (pageid == "40103" && (frm_cocd == "JSGI" || frm_cocd == "DLJM" || frm_cocd == "SDM"))
            e.Row.Cells[4].Visible = true;
        if (pageid == "40103V") e.Row.Cells[4].Visible = true;

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (pageid == "40106" || pageid == "40107")
                e.Row.Cells[19].Text = "Margin %";
        }
        if (pageid == "40103" && (frm_cocd == "JSGI" || frm_cocd == "DLJM" || frm_cocd == "SDM"))
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[4].Visible = true;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton vlink = (LinkButton)e.Row.FindControl("LnkBtnv");
            LinkButton mlink = (LinkButton)e.Row.FindControl("LnkBtnd");
            mlink.Visible = false;
            if (frm_cocd == "MMC") mlink.Visible = true;
            if (pageid == "51103" || pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313" || pageid == "60412" || pageid == "42515") mlink.Text = "View Details";
            if (pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313") vlink.Visible = false;

            if (frm_cocd == "MMC")
            {
                DataTable dt_dp = new DataTable();
                //dt_dp = fgen.fill_dt(co_cd, "SELECT * FROM D_RIGHTS WHERE /*BRANCHCD='" + mbr + "' AND*/ TYPE='RI' AND TRIM(COL1)='" + pageid + "' AND TRIM(upper(COL3))='" + uname + "'");
                mq0 = "SELECT * FROM D_RIGHTS WHERE /*BRANCHCD='" + mbr + "' AND*/ TYPE='RI' AND TRIM(COL1)='" + pageid + "' AND TRIM(upper(COL3))='" + uname + "'";
                dt_dp = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt_dp.Rows.Count > 0) { }
                else
                {
                    if (ulevel != "0")
                    {
                        vlink.Visible = false;
                        mlink.Visible = false;
                    }
                }
                foreach (DataRow dr_dp in dt_dp.Rows)
                {
                    if (dr_dp["col1"].ToString().Trim() == pageid.Trim())
                    {
                        if (dr_dp["y_n"].ToString().Trim() == "Y")
                            vlink.Visible = true;
                        else vlink.Visible = false;
                        if (dr_dp["y_n1"].ToString().Trim() == "Y")
                            mlink.Visible = true;
                        else mlink.Visible = false;
                    }
                }
            }
        }
    }
    protected void btnshow_Click(object sender, EventArgs e)
    {
        query = "";
        query = (string)ViewState["SSQUERY"];
        BindData(query);
        txtsearch.Text = "";
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        clearcontrol();

        sstring = "";
        dt = new DataTable();

        sstring = txtsearch.Text.Trim().ToString();
        if (sstring == "")
            dt = (DataTable)ViewState["SDATA"];
        else
        {
            query = "";
            query = (string)ViewState["SSQUERY"];

            dt = fgen.getdata(frm_qstr, co_cd, query);

            dt1 = new DataTable();
            dt1 = fgen.searchDataTable(sstring, dt);
            dt = new DataTable();
            dt = dt1;
        }
        if (dt.Rows.Count > 0)
        {
            create_tab();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
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

                sg1_dr["chkok"] = "-";
                sg1_dr["chkno"] = "-";
                sg1_dr["txtdate"] = "-";
                sg1_dr["txttout"] = "-";
                sg1_dr["txtrsn"] = "-";

                if (pageid == "43105")
                {
                    sg1_dr["sg1_f1"] = dt.Rows[i]["EDN"].ToString();
                    sg1_dr["sg1_f2"] = dt.Rows[i]["edn_date"].ToString();
                    sg1_dr["sg1_f3"] = dt.Rows[i]["drawing_type"].ToString();
                    sg1_dr["sg1_f4"] = dt.Rows[i]["drawing_no"].ToString();
                    sg1_dr["sg1_f5"] = dt.Rows[i]["revision_no"].ToString();
                    sg1_dr["sg1_f6"] = dt.Rows[i]["design_type"].ToString();
                    sg1_dr["sg1_f7"] = dt.Rows[i]["issue_to"].ToString();
                    sg1_dr["sg1_f8"] = dt.Rows[i]["issue_by"].ToString();
                    sg1_dr["sg1_f9"] = dt.Rows[i]["return_target_date"].ToString();
                    sg1_dr["sg1_f10"] = dt.Rows[i]["ent_by"].ToString();
                    sg1_dr["sg1_f11"] = dt.Rows[i]["ent_dt"].ToString();
                    sg1_dr["sg1_f12"] = dt.Rows[i]["vdd"].ToString();
                    sg1_dr["sg1_f13"] = dt.Rows[i]["filepath"].ToString();
                }
                if (pageid == "F55165" || pageid == "F55166")
                {
                    sg1_dr["sg1_f1"] = dt.Rows[i]["entry_no"].ToString();
                    sg1_dr["sg1_f2"] = dt.Rows[i]["entry_date"].ToString();
                    sg1_dr["sg1_f3"] = dt.Rows[i]["customer"].ToString();
                    sg1_dr["sg1_f4"] = dt.Rows[i]["iname"].ToString();
                    sg1_dr["sg1_f5"] = dt.Rows[i]["cpartno"].ToString();
                    sg1_dr["sg1_f6"] = dt.Rows[i]["modal_no"].ToString();
                    sg1_dr["sg1_f7"] = dt.Rows[i]["drawing_no"].ToString();
                    sg1_dr["sg1_f8"] = dt.Rows[i]["revision_no"].ToString();
                    sg1_dr["sg1_f9"] = dt.Rows[i]["drawing_stage"].ToString();
                    sg1_dr["sg1_f10"] = dt.Rows[i]["col1"].ToString();
                    sg1_dr["sg1_f11"] = dt.Rows[i]["ent_by"].ToString();
                    sg1_dr["sg1_f12"] = dt.Rows[i]["ent_dt"].ToString();
                    sg1_dr["sg1_f13"] = dt.Rows[i]["acode"].ToString();
                }
                sg1_dt.Rows.Add(sg1_dr);
            }

            GridView1.DataSource = sg1_dt;
            GridView1.DataBind();
            GridView1.Visible = true;

            lblshow.Text = "";
            lblshow.Text = "Shwoing " + dt.Rows.Count + " Rows ";

            setColHeadings();
        }
        else
            AlertMsg("AMSG", "search criteria does not match!");
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(GridView1.Rows[rowIndex].RowIndex);
        switch (var)
        {
            case "View":
            case "DWN":
                if (pageid == "43105")
                {
                    if (GridView1.Rows[index].Cells[28].Text.Trim() != "-" || GridView1.Rows[index].Cells[28].Text.Trim() != " ")
                    {
                        FilePath.Value = GridView1.Rows[index].Cells[28].Text;
                    }
                }
                else
                {
                    if (GridView1.Rows[index].Cells[25].Text.Trim() != "-" || GridView1.Rows[index].Cells[28].Text.Trim() != " ")
                    {
                        FilePath.Value = GridView1.Rows[index].Cells[25].Text;
                    }
                }
                FilePath.Value = "";
                if (FilePath.Value.Length > 2)
                {
                    string filePath = FilePath.Value.Substring(FilePath.Value.ToUpper().IndexOf("UPLOAD"), FilePath.Value.Length - FilePath.Value.ToUpper().IndexOf("UPLOAD"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                }
                else
                {
                    hffield.Value = "FILE";
                    if (var == "DWN")
                        hffield.Value = "DWN";
                    col1 = "SELECT TRIM(a.msgtxt) as fstr,b.aname as customer,c.iname as part_name,a.msgtxt as filename,a.msgdt FROM atchvch a,famst b,item c where trim(a.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' AND A.TYPE='DE' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + GridView1.Rows[index].Cells[16].Text.Trim() + GridView1.Rows[index].Cells[17].Text.Trim() + "' ORDER BY A.MSGDT ";
                    col1 = "SELECT TRIM(f.msgtxt)||'~'||trim(f.MSGTO) as fstr,b.aname as customer,c.iname as part_name,f.msgtxt as filename,f.terminal as design_type,f.MSGFROM as activation,f.msgdt as srno FROM wb_drawrec a,atchvch f,famst b,item c where trim(a.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')=f.branchcd||f.type||trim(F.vchnum)||to_Char(f.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' AND A.TYPE='DE' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + GridView1.Rows[index].Cells[16].Text.Trim() + GridView1.Rows[index].Cells[17].Text.Trim() + "' ORDER BY f.MSGDT ";
                    if (wSeriesControl == "Y")
                        col1 = "SELECT TRIM(f.msgtxt)||'~'||trim(f.MSGTO) as fstr,b.name as customer,c.name as part_name,f.msgtxt as filename,f.terminal as design_type,f.MSGFROM as activation,f.msgdt as srno FROM wb_drawrec a,atchvch f,typegrp b,typegrp c where trim(a.acode)=trim(B.type1) and b.id='C1' and trim(A.icode)=trim(c.type1) and c.id='P1' and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')=f.branchcd||f.type||trim(F.vchnum)||to_Char(f.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' AND A.TYPE='DE' AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + GridView1.Rows[index].Cells[16].Text.Trim() + GridView1.Rows[index].Cells[17].Text.Trim() + "' ORDER BY f.MSGDT ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", col1);
                    fgen.Fn_open_sseek("Select Drawing to View", frm_qstr);
                }

                break;
            case "View1":
                if (index < GridView1.Rows.Count - 1)
                {
                    // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    // hffield.Value = "SG1_ROW_ADD_E";
                    hffield.Value = "View";
                    hf2.Value = "View";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    // make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Export Invoice", frm_qstr);                  
                    // fgen.Fn_open_prddmp1("-", frm_qstr);
                }
                else
                {
                    // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
                    //hffield.Value = "SG1_ROW_ADD";
                    hffield.Value = "TACODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    //make_qry_4_popup();
                    //fgen.Fn_open_mseek("Select Export Invoice", frm_qstr);
                }
                break;
        }
    }
    protected void linkimg_Click(object sender, ImageClickEventArgs e)
    {
        clearcontrol();
        col1 = "";
        //  LinkButton selectButton = (LinkButton)sender;
        ImageButton selectButton = (ImageButton)sender;
        GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        col1 = GridView1.Rows[row.RowIndex].Cells[1].Text.Trim().ToString();
        // col1 = GridView1.Rows[row.RowIndex].Cells[row.RowIndex].Text.ToString().Trim();
        ///=========================
        //ImageButton lbtn = (ImageButton)sender;
        //GridViewRow row = (GridViewRow)lbtn.NamingContainer;
        //if (row != null)
        //{
        //  col1 = Convert.ToString(GridView1.DataKeys[row.RowIndex].Value);
        //}  


        //==================

        //ImageButton ibtn1 = sender as ImageButton;
        //int rowIndex = Convert.ToInt32(ibtn1.Attributes["RowIndex"]);
        //col1 = Convert.ToInt32(row);
        if (pageid == "46101" || frm_cocd == "MMC")
        {
            hfbtnmode.Value = "VI";
            if (pageid == "51503" || pageid == "60110a" || pageid == "60113" || pageid == "60213" || pageid == "60313")
            {
                col1 = mbr + "TN" + GridView1.Rows[row.RowIndex].Cells[9].Text.Trim() + GridView1.Rows[row.RowIndex].Cells[10].Text.Trim().Replace("/", "");
                hfbtnmode.Value = "DI";
            }
            sseekfunc(col1);
        }
        else
        {
            //con.Open();
            //send_crystal_rpt(col1);
            //con.Close();
            //headername = "" + pgname + " Master Print";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','Report.aspx','90%','90%');});", true);
            //===============
            //if (query == "") { }
            //else
            //{
            //    Response.Cookies["seeksql"].Value = query;
            //    Response.Cookies["headername"].Value = headername;
            //}
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abcd", "$(document).ready(function(){OpenPopup('" + headername + "','rptlevel2.aspx','90%','97%',false);});", true);
            //================================
            //string var = e.CommandName.ToString();
            //int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            //int index = Convert.ToInt32(GridView1.Rows[rowIndex].RowIndex);   
            // btnhideF_Click(sender, e);

            for (i = 0; i < GridView1.Rows.Count - 1; i++)
            {
                //if (((TextBox)GridView1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-")
                if (GridView1.Rows[i].Cells[28].Text.Trim() != "-" || GridView1.Rows[i].Cells[28].Text.Trim() != " ")
                {
                    FilePath.Value = GridView1.Rows[i].Cells[28].Text;
                }
                string filePath = FilePath.Value.Substring(FilePath.Value.ToUpper().IndexOf("UPLOAD"), FilePath.Value.Length - FilePath.Value.ToUpper().IndexOf("UPLOAD"));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
            }
        }
    }

    protected void chkok_CheckedChanged(object sender, EventArgs e)
    {

    }
}