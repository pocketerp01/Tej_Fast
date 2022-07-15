using System;
using System.Web;
using System.Data;


    public class Create_Icons
    {
        public string mhd = "", Cls_comp_code = "", val = "", icon_allow = "", pco_cd = "", mulevel, muname;
        fgenDB fgen = new fgenDB();
        private string chk_tab(string Qstr, string sysid)
        {
            mulevel = fgenMV.Fn_Get_Mvar(Qstr, "U_ULEVEL");
            Cls_comp_code = Qstr.Split('^')[0].ToString();
            //if (mulevel == "M") mhd = fgen.seek_iname(Qstr, Cls_comp_code, "Select id from FIN_MRSYS where trim(id)='" + sysid.Trim() + "' and trim(userid)='" + muname + "'", "id");
            //else
            {
                //mhd = fgen.seek_iname(Qstr, Cls_comp_code, "Select id from FIN_MSYS where trim(id)='" + sysid.Trim() + "'", "id");                
                mhd = fgen.seek_iname_dt(fgenMV.iconTableFull, "ID='" + sysid.Trim() + "'", "ID");
            }
            if (mhd == "0") val = "N";
            else val = "Y";
            return val;
        }

        private string chk_tabRights(string Qstr, string sysid)
        {
            mulevel = fgenMV.Fn_Get_Mvar(Qstr, "U_ULEVEL");
            Cls_comp_code = Qstr.Split('^')[0].ToString();
            mhd = fgen.seek_iname(Qstr, Cls_comp_code, "Select id from FIN_MRSYS where trim(id)='" + sysid.Trim() + "' and trim(userid)='" + muname + "'", "id");            
            if (mhd == "0") val = "N";
            else val = "Y";
            return val;
        }
        public void add_icon(string Uniq_Qstr_AddIcon, string id, int lvl, string name, int ulvel, string webaction, string srch_key, string submenu, string submenuid, string form, string param, string CSS_NAME)
        {
            Cls_comp_code = Uniq_Qstr_AddIcon.Split('^')[0].ToString();

            if (chk_tab(Uniq_Qstr_AddIcon, id).Trim() == "N")
            {
                DataSet oDS = new DataSet(); DataRow oporow = null;
                oDS = fgen.fill_schema(Uniq_Qstr_AddIcon, Cls_comp_code, "FIN_MSYS");
                oporow = oDS.Tables[0].NewRow();
                oporow["ID"] = id;
                oporow["MLEVEL"] = lvl;
                oporow["TEXT"] = name;
                oporow["ALLOW_LEVEL"] = ulvel;
                oporow["WEB_aCTION"] = webaction;
                oporow["SEARCH_KEY"] = srch_key;
                oporow["SUBMENU"] = submenu;
                oporow["SUBMENUID"] = submenuid;
                oporow["FORM"] = form;
                oporow["PARAM"] = param;
                if (CSS_NAME.Length > 3) { }
                else CSS_NAME = "fa-edit";
                oporow["CSS"] = CSS_NAME;
                oporow["VISI"] = "Y";
                oDS.Tables[0].Rows.Add(oporow);
                fgen.save_data(Uniq_Qstr_AddIcon, Cls_comp_code, oDS, "FIN_MSYS");
                oDS.Dispose();

                //22/08/2020 (to re fill the table) 
                fgen.execute_cmd(Uniq_Qstr_AddIcon, Cls_comp_code, "COMMIT");

                fgenMV.iconTableFull = new DataTable();
                fgenMV.iconTableFull = fgen.getdata(Uniq_Qstr_AddIcon, Cls_comp_code, "SELECT DISTINCT NVL(ID,'-') AS ID FROM FIN_MSYS ORDER BY NVL(ID,'-')");

            }
        }




        public void add_icon(string Uniq_Qstr_AddIcon, string id, int lvl, string name, int ulvel, string webaction, string srch_key, string submenu, string submenuid, string form, string param, string CSS_NAME, string askBranchPopup, string askPrdRange)
        {
            Cls_comp_code = Uniq_Qstr_AddIcon.Split('^')[0].ToString();
            if (chk_tab(Uniq_Qstr_AddIcon, id).Trim() == "N")
            {
                DataSet oDS = new DataSet(); DataRow oporow = null;
                oDS = fgen.fill_schema(Uniq_Qstr_AddIcon, Cls_comp_code, "FIN_MSYS");
                oporow = oDS.Tables[0].NewRow();
                oporow["ID"] = id;
                oporow["MLEVEL"] = lvl;
                oporow["TEXT"] = name;
                oporow["ALLOW_LEVEL"] = ulvel;
                oporow["WEB_aCTION"] = webaction;
                oporow["SEARCH_KEY"] = srch_key;
                oporow["SUBMENU"] = submenu;
                oporow["SUBMENUID"] = submenuid;
                oporow["FORM"] = form;
                oporow["PARAM"] = param;
                oporow["VISI"] = "Y";
                if (CSS_NAME.Length > 3) { }
                else CSS_NAME = "fa-edit";
                oporow["CSS"] = CSS_NAME;
                if (askBranchPopup == "N") oporow["BRN"] = "N";
                else oporow["BRN"] = "Y";
                if (askPrdRange == "N") oporow["PRD"] = "N";
                else oporow["PRD"] = "Y";
                oporow["VISI"] = "Y";
                oDS.Tables[0].Rows.Add(oporow);
                fgen.save_data(Uniq_Qstr_AddIcon, Cls_comp_code, oDS, "FIN_MSYS");
                oDS.Dispose();
            }
        }
        public void add_icon(string Uniq_Qstr_AddIcon, string id, int lvl, string name, int ulvel, string webaction, string srch_key, string submenu, string submenuid, string form, string param, string CSS_NAME, string askBranchPopup, string askPrdRange, string visi)
        {
            Cls_comp_code = Uniq_Qstr_AddIcon.Split('^')[0].ToString();
            {
                DataSet oDS = new DataSet(); DataRow oporow = null;
                oDS = fgen.fill_schema(Uniq_Qstr_AddIcon, Cls_comp_code, "FIN_MSYS");
                oporow = oDS.Tables[0].NewRow();
                oporow["ID"] = id;
                oporow["MLEVEL"] = lvl;
                oporow["TEXT"] = name;
                oporow["ALLOW_LEVEL"] = ulvel;
                oporow["WEB_aCTION"] = webaction;
                oporow["SEARCH_KEY"] = srch_key;
                oporow["SUBMENU"] = submenu;
                oporow["SUBMENUID"] = submenuid;
                oporow["FORM"] = form;
                oporow["PARAM"] = param;
                oporow["VISI"] = visi;
                if (CSS_NAME.Length > 3) { }
                else CSS_NAME = "fa-edit";
                oporow["CSS"] = CSS_NAME;
                if (askBranchPopup == "N") oporow["BRN"] = "N";
                else oporow["BRN"] = "Y";
                if (askPrdRange == "N") oporow["PRD"] = "N";
                else oporow["PRD"] = "Y";
                oporow["VISI"] = visi.ToUpper();
                oDS.Tables[0].Rows.Add(oporow);
                fgen.save_data(Uniq_Qstr_AddIcon, Cls_comp_code, oDS, "FIN_MSYS");
                oDS.Dispose();
            }
        }
        public void add_iconRights(string Uniq_Qstr_AddIcon, string id, int lvl, string name, int ulvel, string webaction, string srch_key, string submenu, string submenuid, string form, string param, string CSS_NAME)
        {
            Cls_comp_code = Uniq_Qstr_AddIcon.Split('^')[0].ToString();
            string userID = fgenMV.Fn_Get_Mvar(Uniq_Qstr_AddIcon, "U_USERID");
            muname = fgenMV.Fn_Get_Mvar(Uniq_Qstr_AddIcon, "U_UNAME");

            if (chk_tabRights(Uniq_Qstr_AddIcon, id).Trim() == "N")
            {
                DataSet oDS = new DataSet(); DataRow oporow = null;
                oDS = fgen.fill_schema(Uniq_Qstr_AddIcon, Cls_comp_code, "FIN_MRSYS");
                oporow = oDS.Tables[0].NewRow();

                oporow["USERID"] = userID;
                oporow["USERNAME"] = muname;
                oporow["ID"] = id;
                oporow["MLEVEL"] = lvl;
                oporow["TEXT"] = name;
                oporow["ALLOW_LEVEL"] = ulvel;
                oporow["WEB_aCTION"] = webaction;
                oporow["SEARCH_KEY"] = srch_key;
                oporow["SUBMENU"] = submenu;
                oporow["SUBMENUID"] = submenuid;
                oporow["FORM"] = form;
                oporow["PARAM"] = param;
                if (CSS_NAME.Length > 3) { }
                else CSS_NAME = "fa-edit";
                oporow["CSS"] = CSS_NAME;
                oporow["VISI"] = "Y";
                oDS.Tables[0].Rows.Add(oporow);
                fgen.save_data(Uniq_Qstr_AddIcon, Cls_comp_code, oDS, "FIN_MRSYS");
                oDS.Dispose();
            }
        }

        public void chk_icon(string frm_qstr, string frm_cocd)
        {
            Cls_comp_code = frm_cocd;
            mulevel = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
            muname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");


            string mhd1 = "";
            mhd1 = fgen.seek_iname(frm_qstr, frm_cocd, "select idno from FIN_RSYS_UPD where trim(idno)='MSYS101'", "idno");
            if (mhd1 == "0" || mhd1 == "")
            {
                // by pass put on 7/11/2018 to speed up login process
                //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MSYS101') ");
                fgen.add_RsysUpd(frm_qstr, frm_cocd, "MSYS101", "DEV_A");

                fgen.execute_cmd(frm_qstr, frm_cocd, "alter TABLE SR_CTRL modify FINPKFLD VARCHAR2(40)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_MSYS'", "TNAME");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE FIN_MSYS(ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(180) default '-',ALLOW_LEVEL NUMBER(2),WEB_aCTION VARCHAR2(50) default '-',SEARCH_KEY VARCHAR2(50) default '-',submenu char(1)default 'N',submenuid char(15) default '-',form varchar2(10) default '-',param varchar2(40) default '-',imagef varchar2(50) default '-',CSS varchar2(30) default 'fa-edit',PRD varchar2(1) default '-',BRN varchar2(1) default '-',BNR varchar2(1) default '-')");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS MODIFY ID VARCHAR(10) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "IMAGEF"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD IMAGEF VARCHAR(50) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "BRN"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD BRN CHAR(1) DEFAULT 'Y'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "PRD"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD PRD CHAR(1) DEFAULT 'Y'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "VISI"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD VISI CHAR(1) DEFAULT 'Y'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "UPD_BY"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD UPD_BY varchar2(15) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "UPD_DT"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD UPD_DT date DEFAULT sysdate");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_ADD"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "Alter Table FIN_MRSYS add RCAN_ADD VARCHAR2(1) default 'Y'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_EDIT"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "Alter Table FIN_MRSYS add RCAN_EDIT VARCHAR2(1) default 'Y'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "RCAN_DEL"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "Alter Table FIN_MRSYS add RCAN_DEL VARCHAR2(1) default 'Y'");

            }
            fgen.execute_cmd(frm_qstr, frm_cocd, "COMMIT");

            if ((mulevel != "M" && frm_cocd == "LIVN") || frm_cocd != "FINS" || frm_cocd != "MLGA" || frm_cocd != "PKGW")
            {
                //add_icon(frm_qstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
                //add_icon(frm_qstr, "97001", 2, "User Managment", 1, "../tej-base/frmUmst.aspx", "-", "-", "-", "SYSAD", "SYSADM", "-");
                //add_icon(frm_qstr, "97010", 2, "User Rights", 1, "../tej-base/urights.aspx", "-", "-", "-", "SYSAD", "SYSADM", "-");
            }
        }
    }
