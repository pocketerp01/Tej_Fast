using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Data;

/// <summary>
/// Summary description for jcpSave
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class jcpSave : System.Web.Services.WebService
{
    fgenDB fgen = new fgenDB();
    string frm_qstr, frm_cocd, frm_uname, frm_mbr, frm_tabname, frm_vty, DateRange, frm_vnum, pk_error, vchField, vchdtField, vardate, frm_CDT1, frm_PageName, frm_formID, squery;
    DataRow oporow;
    DataSet oDS;

    public jcpSave()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }


    [WebMethod]
    public string getJsonData()
    {
        frm_qstr = this.Context.Request.QueryString["STR"];
        frm_cocd = frm_qstr.Split('^')[0];
        frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
        frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
        squery = "select * from (select db_query,SRNO,obj_name,rownum as rno from (SELECT trim(a.obj_caption)||'@'||trim(a.obj_SQL) as db_query,A.SRNO,A.obj_name FROM DSK_CONFIG a where SUBSTR(upper(obj_name),1,3) IN ('TXT') ORDER BY A.obj_name)) order by rno";
        //List<myList> details = new List<myList>();
        DataTable dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, squery);
        DataSet ds = new DataSet();
        dt.TableName = "Tiles";
        ds.Tables.Add(dt);

        return ds.GetXml();
    }

    [WebMethod]
    public void saveData(List<JobCardDetails> JC)
    {
        frm_qstr = this.Context.Request.QueryString["STR"];
        frm_cocd = frm_qstr.Split('^')[0];
        frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
        frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
        frm_tabname = "PROD_SHEET";
        frm_vty = "90";
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        frm_vnum = "";
        pk_error = "";
        vchField = "VCHNUM";
        vchdtField = "VCHDATE";
        vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
        frm_PageName = "JC Planning";
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (frm_formID == "F35108") frm_vty = "OP";

        int i = 0;
        if (frm_formID != "F35108")
        {
            do
            {
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + vchField + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + vchdtField + " " + DateRange + "", 6, "vch");
                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, vardate, "", frm_uname);
                if (i > 20)
                {
                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + vchField + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + vchdtField + " " + DateRange + " ", 6, "vch");
                    pk_error = "N";
                    i = 0;
                }
                i++;
            }
            while (pk_error == "Y");
        }
        oporow = null;
        oDS = new DataSet();
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
        double totTime = 0;
        string mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME||'~'||RATE AS VAL FROM TYPE WHERE ID='K' AND TYPE1='" + JC[0].stageCode + "'", "VAL");
        string stageName = "";
        string acode = "-";
        string checkLoadedDays = "0";
        string nextDate = DateTime.Now.ToString("dd/MM/yyyy");
        double timeD = 0;
        if (mhd.Length > 1)
        {
            stageName = mhd.Split('~')[0];
            timeD = fgen.make_double(mhd.Split('~')[1]);
        }
        for (int j = 0; j < JC.Count; j++)
        {
            totTime += fgen.make_double(JC[j].MachineTime);
        }

        for (int j = 0; j < JC.Count; j++)
        {
            vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            if (frm_formID == "F35108")
            {
                oporow = null;
                oDS = new DataSet();
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                i = 0;
                do
                {
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + vchField + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + vchdtField + " " + DateRange + "", 6, "vch");
                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, vardate, "", frm_uname);
                    if (i > 20)
                    {
                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + vchField + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + vchdtField + " " + DateRange + " ", 6, "vch");
                        pk_error = "N";
                        i = 0;
                    }
                    i++;
                }
                while (pk_error == "Y");


                stageName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MCHNAME FROM PMAINt WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND trim(MCHCODE)='" + JC[j].machineNo.Split('-')[0].ToString().Trim() + "'", "MCHNAME");
                acode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(ACODE) AS ACODE FROM SOMAS WHERE BRANCHCD||TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')||TRIM(ICODE)='" + frm_mbr + JC[j].jobno + JC[j].jobdt + JC[j].iCode + "' ", "ACODE");
                checkLoadedDays = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(DAY_LOADED) AS ACODE FROM VU_LINE_BUSY WHERE upper(trim(LINE_NAME))='" + stageName.Trim().ToUpper() + "' ", "ACODE");
                if (fgen.make_double(checkLoadedDays) == 0)
                {
                    checkLoadedDays = (fgen.make_double(JC[j].MachineTime) / 60 / 22).ToString();
                }
                else
                {
                    checkLoadedDays = (fgen.make_double(checkLoadedDays) + fgen.make_double(JC[j].MachineTime) / 60 / 22).ToString();
                }
                nextDate = DateTime.Now.AddDays(fgen.make_double(checkLoadedDays)).ToString("dd/MM/yyyy");
            }
            #region Saving
            oporow = oDS.Tables[0].NewRow();

            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow[vchField] = frm_vnum;
            oporow[vchdtField] = vardate;
            oporow["ACODE"] = acode;
            oporow["ICODE"] = JC[j].iCode;
            oporow["A1"] = fgen.make_double(JC[j].qty);
            oporow["A2"] = "0";
            oporow["A3"] = "0";
            oporow["A4"] = "0";
            oporow["A5"] = "0";
            oporow["A6"] = JC[j].qty;
            oporow["A7"] = "0";
            oporow["A8"] = "0";

            oporow["TOTAL"] = timeD;

            oporow["UN_MELT"] = JC[j].MachineTime;
            oporow["MLT_LOSS"] = "0";
            oporow["FLAG"] = "0";
            oporow["SRNO"] = fgen.padlc((j + 1), 2);
            oporow["REMARKS"] = "-";
            oporow["STAGE"] = JC[j].stageCode;
            oporow["IQTYIN"] = "0";
            oporow["IQTYOUT"] = JC[j].qty;
            oporow["SUBCODE"] = JC[j].shiftTime;
            oporow["MCHCODE"] = JC[j].machineNo.Split('-')[0];
            oporow["PREVSTAGE"] = "-";
            oporow["PREVCODE"] = JC[j].shiftName;
            oporow["EMPCODE"] = "1";
            oporow["SHFTCODE"] = JC[j].shiftCode;
            oporow["NOUPS"] = "0";
            oporow["JOB_NO"] = JC[j].jobno;
            oporow["JOB_DT"] = JC[j].jobdt;
            oporow["A9"] = "0";
            oporow["A10"] = "0";
            oporow["A11"] = "0";
            oporow["A12"] = "0";
            oporow["LMD"] = "0";
            oporow["BCD"] = totTime;
            oporow["TSLOT"] = "0";

            oporow["MCSTART"] = "0";
            oporow["MCSTOP"] = "0";

            oporow["ENAME"] = stageName;
            oporow["VAR_CODE"] = totTime;

            oporow["GLUE_CODE"] = "-";
            oporow["FILM_CODE"] = "N";
            oporow["REMARKS2"] = "-";
            oporow["NARATION"] = "-";
            oporow["NUM1"] = "0";
            oporow["NUM2"] = "0";
            oporow["NUM3"] = "0";
            oporow["NUM4"] = "0";
            oporow["NUM5"] = "0";
            oporow["NUM6"] = "0";
            oporow["NUM7"] = "0";
            oporow["NUM8"] = "0";
            oporow["NUM9"] = "0";
            oporow["NUM10"] = "0";
            oporow["ENT_BY"] = frm_uname;
            oporow["ENT_DT"] = DateTime.Now;
            oporow["WO_NO"] = "-";
            oporow["WO_DT"] = Convert.ToDateTime(nextDate);
            oporow["NUM11"] = "0";
            oporow["NUM12"] = "0";
            oporow["MTIME"] = "-";
            oporow["EXC_TIME"] = "-";
            oporow["TEMPR"] = frm_mbr;
            oporow["IRATE"] = "0";
            oporow["MSEQ"] = "0";
            oporow["A13"] = "0";
            oporow["A14"] = "0";
            oporow["A15"] = "0";
            oporow["A16"] = "0";
            oporow["A17"] = "0";
            oporow["A18"] = "0";
            oporow["A19"] = "0";
            oporow["A20"] = "0";
            oporow["FM_FACT"] = "1";
            oporow["PCPSHOT"] = "1";
            oporow["PBTCHNO"] = "-";
            oporow["OPR_DTL"] = "-";
            oporow["OEE_R"] = "0";

            oporow["HCUT"] = "0";

            oporow["ALSTTIM"] = "0";
            oporow["ALTCTIM"] = "0";
            oporow["CUST_REF"] = "-";
            oporow["CELL_REF"] = "-";
            oporow["CELL_REFN"] = "-";

            if (frm_formID != "F35108")
            {
                oporow["A21"] = "0";
                oporow["A22"] = "0";
                oporow["A23"] = "0";
                oporow["A24"] = "0";
                oporow["A25"] = "0";
                oporow["A26"] = "0";
                oporow["A27"] = "0";
                oporow["A28"] = "0";
                oporow["A29"] = "0";
                oporow["A30"] = "0";
            }
            oporow["NTEMPR"] = "0";
            oporow["TOT_DT"] = "0";
            oporow["DCODE"] = "-";
            oporow["EDT_BY"] = "-";
            oporow["EDT_DT"] = DateTime.Now;

            try
            {
                oDS.Tables[0].Rows.Add(oporow);
            }
            catch (Exception ex) { fgen.FILL_ERR(ex.Message); }
            if (frm_formID == "F35108")
            {
                fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
            }
            #endregion
        }
        if (frm_formID != "F35108")
            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
    }

    [WebMethod]
    public void saveGE(List<mySgClass> jcList)
    {
        frm_qstr = this.Context.Request.QueryString["STR"];
        frm_cocd = frm_qstr.Split('^')[0];
        frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
        frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
        frm_tabname = "IVOUCHERP";
        frm_vty = "00";
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        frm_vnum = "";
        pk_error = "";
        vchField = "VCHNUM";
        vchdtField = "VCHDATE";
        vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
        frm_PageName = "GE";
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        int i = 0;
        do
        {
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + vchField + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + vchdtField + " " + DateRange + "", 6, "vch");
            pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, vardate, "", frm_uname);
            if (i > 20)
            {
                fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + vchField + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + vchdtField + " " + DateRange + " ", 6, "vch");
                pk_error = "N";
                i = 0;
            }
            i++;
        }
        while (pk_error == "Y");

        oporow = null;
        oDS = new DataSet();
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

        for (int x = 0; x < jcList.Count; x++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = DateTime.Now.ToString("dd/MM/yyyy");

            oporow["ACODE"] = jcList[x].acode;
            oporow["ICODE"] = jcList[x].sg1_f1;

            oporow["PRNUM"] = "OT";            

            oporow["spexc_Amt"] = jcList[x].sg1_t1.toDouble();
            oporow["IQTY_CHL"] = jcList[x].sg1_t2.toDouble();
            oporow["iqty_chlwt"] = jcList[x].sg1_t3.toDouble();
            oporow["iqty_WT"] = jcList[x].sg1_t4.toDouble();
            oporow["IRATE"] = jcList[x].sg1_t5.toDouble();
            oporow["DESC_"] = jcList[x].sg1_t6.toDouble();
            oporow["rej_sdp"] = jcList[x].sg1_t7.toDouble();

            oporow["ENT_BY"] = "FINTEAM";
            oporow["ENT_DT"] = DateTime.Now.ToString("dd/MM/yyyy");
            oporow["EDT_BY"] = "-";
            oporow["EDT_DT"] = DateTime.Now.ToString("dd/MM/yyyy");

            oDS.Tables[0].Rows.Add(oporow);
        }
        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

    }
}


public class mySgClass
{
    public string sg1_h1 { get; set; }
    public string sg1_h2 { get; set; }
    public string sg1_h3 { get; set; }
    public string sg1_h4 { get; set; }
    public string sg1_h5 { get; set; }
    public string sg1_h6 { get; set; }
    public string sg1_h7 { get; set; }
    public string sg1_h8 { get; set; }
    public string sg1_h9 { get; set; }
    public string sg1_h10 { get; set; }

    public string sg1_f1 { get; set; }
    public string sg1_f2 { get; set; }
    public string sg1_f3 { get; set; }
    public string sg1_f4 { get; set; }
    public string sg1_f5 { get; set; }
    public string sg1_f6 { get; set; }
    public string sg1_f7 { get; set; }
    public string sg1_f8 { get; set; }
    public string sg1_f9 { get; set; }
    public string sg1_f10 { get; set; }    

    public string sg1_t1 { get; set; }
    public string sg1_t2 { get; set; }
    public string sg1_t3 { get; set; }
    public string sg1_t4 { get; set; }
    public string sg1_t5 { get; set; }
    public string sg1_t6 { get; set; }
    public string sg1_t7 { get; set; }
    public string sg1_t8 { get; set; }
    public string sg1_t9 { get; set; }
    public string sg1_t10 { get; set; }
    public string sg1_t11 { get; set; }
    public string sg1_t12 { get; set; }
    public string sg1_t13 { get; set; }
    public string sg1_t14 { get; set; }
    public string sg1_t15 { get; set; }
    public string sg1_t16 { get; set; }
    public string sg1_t17 { get; set; }
    public string sg1_t18 { get; set; }
    public string sg1_t19 { get; set; }
    public string sg1_t20 { get; set; }
    public string sg1_t21 { get; set; }
    public string sg1_t22 { get; set; }
    public string sg1_t23 { get; set; }
    public string sg1_t24 { get; set; }
    public string sg1_t25 { get; set; }
    public string sg1_t26 { get; set; }
    public string sg1_t27 { get; set; }
    public string sg1_t28 { get; set; }
    public string sg1_t29 { get; set; }
    public string sg1_t30 { get; set; }
    public string sg1_t31 { get; set; }
    public string sg1_t32 { get; set; }
    public string sg1_t33 { get; set; }
    public string sg1_t34 { get; set; }
    public string sg1_t35 { get; set; }
    public string sg1_t36 { get; set; }
    public string sg1_t37 { get; set; }
    public string sg1_t38 { get; set; }
    public string sg1_t39 { get; set; }
    public string sg1_t40 { get; set; }

    public string vchdate { get; set; }
    public string acode { get; set; }
    public string icode { get; set; }    
}
public class JobCardDetails
{
    public string machineNo { get; set; }
    public string qty { get; set; }
    public string jobno { get; set; }
    public string jobdt { get; set; }
    public string MachineTime { get; set; }
    public string stageCode { get; set; }
    public string iCode { get; set; }
    public string shiftCode { get; set; }
    public string shiftName { get; set; }
    public string shiftTime { get; set; }
}


public class myList
{
    public string col1 { get; set; }
    public string col2 { get; set; }
    public string col3 { get; set; }
    public string col4 { get; set; }
    public string col5 { get; set; }
    public string col6 { get; set; }
}