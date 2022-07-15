using System;

public class Upd_2018
{
    fgenDB fgen = new fgenDB();
    Create_Icons ICO = new Create_Icons();
    string mhd = "";

    string MV_CLIENT_GRP = "";


    public void Upd_Oct(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DM0017");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0017','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0017", "DEV_A");


            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_TASK_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_TASK_LOG(branchcd char(2),type char(2),TRCNO char(6),TRCDT date,CCODE char(10) default '-',Client_Name varchar2(60) default '-',Tgt_Days varchar2(10) default '-',Task_Type varchar2(30) default '-',Tsubject varchar2(30) default '-',Team_Member varchar2(50) default '-',Client_Person varchar2(20) default '-',Client_Phone varchar2(30) default '-',Cremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',Task_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',TASK_CLOSE VARCHAR2(1) DEFAULT '-',CURR_STAT VARCHAR2(10) DEFAULT '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_TASK_ACT'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_TASK_ACT(branchcd char(2),type char(2),TACNO char(6),TACDT date,TRCNO char(6),TRCDT date,CCODE char(10) default '-',Client_Name varchar2(60) default '-',Tgt_Days varchar2(10) default '-',Task_Type varchar2(30) default '-',Tsubject varchar2(30) default '-',Team_member varchar2(50) default '-',Client_Person varchar2(20) default '-',Client_Phone varchar2(30) default '-',Time_Taken varchar2(10) DEFAULT '-',Act_mode varchar2(10) DEFAULT '-',Next_Folo number(5) DEFAULT 0,Oremarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',CURR_STAT VARCHAR2(10) DEFAULT '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='UDF_DATA'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table UDF_DATA(branchcd char(2),PAR_TBL varchar2(30) default '-',PAR_FLD varchar2(30) default '-',udf_name varchar2(30) default '-',udf_value varchar2(100) default '-',srno number(4) default 0)");

            mhd = "update fin_msys set web_action='../tej-base/om_dbd_mgrph.aspx' where id='F90142'";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PROJ_SETUP'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_PROJ_SETUP(branchcd char(2),type char(2),vchnum char(6),vchdate date,CCODE Char(10),Cust_NAME varchar2(80) default '-',mod_cd varchar2(10) default '-',mod_name varchar2(50),mod_detail varchar2(100),mod_wtg number(4) default 0,mod_hrs number(4) default 0,mod_add_by varchar2(20) default '-',mod_dlv_by varchar2(20) default '-',mod_tgtdt varchar2(10) default '-',mod_dlvdt varchar2(10) default '-',srno number(4) default 0,remarks varchar2(50) default '-',orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PROJ_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_PROJ_LOG(branchcd char(2),type char(2),vchnum char(6),vchdate date,projno char(6),projdt date,proj_srn number(4),CCODE varchar2(10),Cust_NAME varchar2(80) default '-',mod_detail varchar2(100),mod_wtg number(4) default 0,mod_hrs number(4) default 0,MOD_DLV_BY varchar2(20) default '-',work_Done varchar2(60) default '-',trainee varchar2(30) default '-',trainee_dpt varchar2(30) default '-',train_Dt varchar2(10) default '-',train_hrs number(4) default 0,srno number(4) default 0,remarks varchar2(50) default '-',orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='N' where ID in ('F50235','F50231','F50234','F50228','F50224','F50225','F50222','F50236','F50226','F50257','F50255','F50264','F50241','F50240','F50244','F50223','F50242','F50232','F50227','F50245','F50250','F50251','F50233')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/om_view_sale.aspx' where id in ('F50225','F50226','F50227')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_sale.aspx' where ID='F50241'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='N' where ID in ('F10226','F10225','F10224','F10228','F10233','F10229','F10231','F10237','F10222','F10223','F10230','F10234','F10236','F10235')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='N' where ID in ('F15311','F15312','F15313','F15316','F15317','F15310','F15318')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_purc.aspx' where ID='F15240'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='N' where ID in ('F55133','F55134')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='N' where ID in ('F25149','F25242','F25127','F25128','F25129')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set id='F30108',submenuid='fin30_e1' where text='QA Outward Template'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update sys_config set frm_name='F30111' where frm_name='F30121'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin30_e3' where id in ('F30116','F30121','F30126','F30127')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin30_e4' where id in ('F30131','F30132','F30133','F30134')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin30_e5' where id in ('F30140','F30141','F30142','F30143')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin30_e6' where id in ('F30151','F30152','F30156')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/om_view_prodpm.aspx' where id='F39131'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_SERVICE'", "tname");
            if (mhd == "0" || mhd == "")
            {
                mhd = "CREATE TABLE WB_SERVICE (BRANCHCD CHAR(2) DEFAULT '-',TYPE CHAR(2) DEFAULT '-',VCHNUM CHAR(6) DEFAULT '-',VCHDATE DATE DEFAULT SYSDATE,ACODE CHAR(10) DEFAULT '-',ICODE CHAR(10) DEFAULT '-',PREV_MNT VARCHAR2(30) DEFAULT '-',OCCR_TIME VARCHAR2(10) DEFAULT '-',REASON_FAIL VARCHAR2(40) DEFAULT '-',ADDR1 VARCHAR2(100) DEFAULT '-',ADDR2 VARCHAR2(100) DEFAULT '-',ADDR3 VARCHAR2(100) DEFAULT '-',DGSRNO VARCHAR2(100) DEFAULT '-',ENGNO VARCHAR2(100) DEFAULT '-',INVNO VARCHAR2(25) DEFAULT '-',INVDATE DATE DEFAULT SYSDATE,CONT_PER VARCHAR2(100) DEFAULT '-',TELNO VARCHAR2(100) DEFAULT '-',DESG VARCHAR2(100) DEFAULT '-',EMAIL_ID VARCHAR2(100) DEFAULT '-',SITE_ID VARCHAR2(100) DEFAULT '-',SITE_NAME VARCHAR2(100) DEFAULT '-',ADDR4 VARCHAR2(100) DEFAULT '-',ADDR5 VARCHAR2(100) DEFAULT '-',ADDR6 VARCHAR2(100) DEFAULT '-',CALL_DTL VARCHAR2(100) DEFAULT '-',CUST_PO_NO VARCHAR2(100) DEFAULT '-',CUST_PO_DT DATE DEFAULT SYSDATE,EQUIP VARCHAR2(100) DEFAULT '-',PROB_OBSV VARCHAR2(100) DEFAULT '-',RMK1 VARCHAR2(100) DEFAULT '-',DOCNO VARCHAR2(10) DEFAULT '-',DOCDATE DATE DEFAULT SYSDATE,ENG_DEPUTED VARCHAR2(100) DEFAULT '-',CONT_MODE VARCHAR2(100) DEFAULT '-',DEPUTE_DT DATE DEFAULT SYSDATE,SRV_TYPE VARCHAR2(10) DEFAULT '-',DEALER_NAME VARCHAR2(100) DEFAULT '-',FIRST_PER VARCHAR2(100) DEFAULT '-',CATEGORY VARCHAR2(20) DEFAULT '-',ENG_INSTRUCT VARCHAR2(100) DEFAULT '-',DOC_NO VARCHAR2(100) DEFAULT '-',DOC_DT DATE DEFAULT SYSDATE,CLOSED VARCHAR2(2) DEFAULT '-',TIME_IN VARCHAR2(10) DEFAULT '-',TIME_OUT VARCHAR2(10) DEFAULT '-',NXT_TRGT VARCHAR2(100) DEFAULT '-',WORK_DONE VARCHAR2(100) DEFAULT '-',ENG_RMK VARCHAR2(100) DEFAULT '-',CORR_ACT VARCHAR2(100) DEFAULT '-',PREVEN_ACT VARCHAR2(100) DEFAULT '-',REASON_PEND VARCHAR2(100) DEFAULT '-',SPARES_RQD VARCHAR2(100) DEFAULT '-',SERV_COST NUMBER(15,3) DEFAULT 0,SPARE_COST NUMBER(15,3) DEFAULT 0,MISC_COST NUMBER(15,3) DEFAULT 0,TRAVEL_CONV NUMBER(15,3) DEFAULT 0,RMK2 VARCHAR2(100) DEFAULT '-',ENT_BY VARCHAR2(20) DEFAULT '-',ENT_DT DATE,EDT_BY VARCHAR2(20) DEFAULT '-',EDT_DT DATE,CHK_BY VARCHAR2(20) DEFAULT '-',CHK_DT DATE DEFAULT SYSDATE,APP_BY VARCHAR2(20) DEFAULT '-',APP_DT DATE DEFAULT SYSDATE,REFNUM VARCHAR2(10) DEFAULT '-',REFDATE DATE DEFAULT SYSDATE,HODATE DATE DEFAULT SYSDATE,HMR NUMBER(15,3) DEFAULT 0,REMARKS VARCHAR2(100) DEFAULT '-')";
                fgen.execute_cmd(frm_qstr, frm_cocd, mhd);
            }
        }

        mhd = fgen.chk_RsysUpd("DM0020");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0020','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0020", "DEV_A");
            //for manpower planning ytec report 
            if (frm_cocd == "YTEC")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITWSTAGE", "AREA");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITWSTAGE ADD AREA CHAR(2) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITWSTAGE", "CAVITY_PC");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITWSTAGE ADD CAVITY_PC NUMBER(15,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITWSTAGE", "OP_RATE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITWSTAGE ADD OP_RATE NUMBER(15,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITWSTAGE", "NO_MAN");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITWSTAGE ADD NO_MAN NUMBER(15,2) DEFAULT 0");
            }
            if (frm_cocd == "IAIJ")
            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_SCH_UPD'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SCH_UPD(branchcd char(2) ,type char(2),vchnum char(6),vchdate date,icode char(30) NOT NULL,partno char(40),acode char(10),qty number(15,2),stdate VARCHAR2(20),lineno number(10,2),week number(10,2),vend_code char(10),buy_code  CHAR(10),REMARKS VARCHAR2(150),ent_by VARCHAR2(15),ENT_dT DATE,EDT_BY VARCHAR2(15),EDT_dT  DATE)");
            }
        }
        // new icon method
        mgIcons(frm_qstr, frm_cocd);
        vipinIcons(frm_qstr, frm_cocd);
        pkgIcons(frm_qstr, frm_cocd);
    }

    void vipinIcons(string frm_qstr, string frm_cocd)
    {
        Opts_wfin opts_wfin = new Opts_wfin();
        switch (frm_cocd)
        {
            case "HPPI":
            case "PKGW":
            case "SPPI":

                ICO.add_icon(frm_qstr, "F70556", 3, "Detail Statement", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_Hrm(frm_qstr, frm_cocd);
                opts_wfin.Icon_FA_sys(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F05199A", 3, "Multi-Department Live Charts 2", 3, "../tej-base/deskdash2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F47120", 4, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F50137A", 4, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F50137B", 4, "Truck Entry Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F47125", 4, "Supervisior Master", 3, "../tej-base/personmst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10049'");
                    ICO.add_icon(frm_qstr, "F10049", 2, "Customer Care", 3, "-", "-", "Y", "fin10_ec", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10050'");
                    ICO.add_icon(frm_qstr, "F10050", 3, "Customer Request", 3, "../tej-base/om_cmplnt.aspx", "-", "Y", "fin10_ec", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10051'");
                    ICO.add_icon(frm_qstr, "F10051", 3, "Customer Request Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_ec", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10052'");
                    ICO.add_icon(frm_qstr, "F10052", 3, "Action on Request", 3, "../tej-base/neopaction.aspx", "-", "Y", "fin10_ec", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10052S'");
                    ICO.add_icon(frm_qstr, "F10052S", 3, "Request Summary", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_ec", "fin10_a1", "-", "fa-edit", "Y", "Y");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10053'");
                    ICO.add_icon(frm_qstr, "F10053", 3, "Request Status", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_ec", "fin10_a1", "-", "fa-edit", "Y", "Y");

                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10550'");
                    //ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10550", 3, "Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10551'");
                    ICO.add_icon(frm_qstr, "F10551", 3, "Type of Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10552'");
                    ICO.add_icon(frm_qstr, "F10552", 3, "Department Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10553'");
                    ICO.add_icon(frm_qstr, "F10553", 3, "Person Master", 3, "../tej-base/personmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10554'");
                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10555'");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    //ICO.add_icon(frm_qstr, "F10054", 3, "Request Status Print", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_ee", "fin10_a12", "fin10_MREP1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_ee", "fin10_a1", "fin10_MREP1", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_ee", "fin10_a1", "fin10_MREP1", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10556", 3, "Expense Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                }

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185A", 3, "Duplex Costing", 3, "../tej-base/cost_infi_t.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185B", 3, "Duplex Costing 2", 3, "../tej-base/duplx_cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10188", 3, "Label Costing Sheet", 3, "../tej-base/om_label_costing.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                if (frm_cocd == "SPPI")
                {
                    ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40171", 3, "Label Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");

                    //ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F10191", 3, "Around Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F10192", 3, "Cylinder Costing", 3, "../tej-base/om_Cylind_Cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    //mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CYLINDER'", "TNAME");
                    //if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CYLINDER (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,ACODE CHAR(10),ICODE CHAR(10),SRNO  NUMBER(4),COL1  VARCHAR2(20),COL2  VARCHAR2(20),COL3  VARCHAR2(20),COL4  VARCHAR2(20),COL5 VARCHAR2(20),COL6  VARCHAR2(20),COL7  VARCHAR2(20),COL8  VARCHAR2(20),COL9  VARCHAR2(20),COL10 VARCHAR2(20),COL11 VARCHAR2(20),COL12 VARCHAR2(20),COL13 VARCHAR2(20),COL14 VARCHAR2(20),COL15 VARCHAR2(20),REMARKS VARCHAR2(300),NUM1 NUMBER(20,3),NUM2 NUMBER(20,3),NUM3 NUMBER(20,3),NUM4 NUMBER(20,3),NUM5 NUMBER(20,3),NUM6 NUMBER(20,3),NUM7 NUMBER(20,3),NUM8 NUMBER(20,3),NUM9 NUMBER(20,3),NUM10 NUMBER(20,3),NUM11  NUMBER(20,3),NUM12 NUMBER(20,3),NUM13 NUMBER(20,3),NUM14 NUMBER(20,3),NUM15 NUMBER(20,3),NUM16 NUMBER(20,3),NUM17 NUMBER(20,3),NUM18 NUMBER(20,3),NUM19 NUMBER(20,3),NUM20 NUMBER(20,3),NUM21 NUMBER(20,3),NUM22 NUMBER(20,3),NUM23 NUMBER(20,3),NUM24 NUMBER(20,3),NUM25 NUMBER(20,3),NUM26 NUMBER(20,3),NUM27 NUMBER(20,3),NUM28 NUMBER(20,3),NUM29 NUMBER(20,3),NUM30 NUMBER(20,3),NUM31 NUMBER(20,3),NUM32 NUMBER(20,3),NUM33 NUMBER(20,3),NUM34 NUMBER(20,3),NUM35 NUMBER(20,3),NUM36 NUMBER(20,3),NUM37 NUMBER(20,3),EDT_BY VARCHAR2(20),EDT_DT DATE,NARATION VARCHAR2(150),ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL)");
                }

                ICO.add_icon(frm_qstr, "F70337", 3, "Payment Reminder Letter(Mktg)", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70338", 3, "Debtor Outstanding Report(Mktg)", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                if (frm_cocd == "HPPI")
                {
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_PRECOST'", "TNAME");
                    if (mhd == "0" || mhd == "")
                    {
                        string SQuery = "create table wb_precost ( branchcd varchar2(2) default '-',type varchar2(2) default '-',vchnum varchar2(6) default '-',vchdate date default sysdate,acode varchar2(6) default '-',icode varchar2(8) default '-',aname varchar2(150) default '-',iname varchar2(150) default '-',structure varchar2(30) default '-',print_type varchar2(30) default '-',lpo_no varchar2(30) default '-',order_qty number(20,6) default 0,cyl_amor number(20,6) default 0,color number(20,6) default 0,pet_thick number(20,6) default 0,pet_dens number(20,6) default 0,pet_gsm number(20,6) default 0,pet_rm number(20,6) default 0,pet_price1 number(20,6) default 0,pet_price2 number(20,6) default 0,pet_cost1 number(20,6) default 0,pet_cost2 number(20,6) default 0,met_thick number(20,6) default 0,met_dens number(20,6) default 0,met_gsm number(20,6) default 0,met_rm number(20,6) default 0,met_price1 number(20,6) default 0,met_price2 number(20,6) default 0,met_cost1 number(20,6) default 0,met_cost2 number(20,6) default 0,lpde_thick number(20,6) default 0,lpde_dens number(20,6) default 0,lpde_gsm number(20,6) default 0,lpde_rm number(20,6) default 0,lpde_price1 number(20,6) default 0,lpde_price2 number(20,6) default 0,lpde_cost1 number(20,6) default 0,lpde_cost2 number(20,6) default 0,ink_thick number(20,6) default 0,ink_dens number(20,6) default 0,ink_gsm number(20,6) default 0,ink_rm number(20,6) default 0,ink_price1 number(20,6) default 0,ink_price2 number(20,6) default 0,ink_cost1 number(20,6) default 0,ink_cost2 number(20,6) default 0,adh1_thick number(20,6) default 0,adh1_dens number(20,6) default 0,adh1_gsm number(20,6) default 0,adh1_rm number(20,6) default 0,adh1_price1 number(20,6) default 0,adh1_price2 number(20,6) default 0,adh1_cost1 number(20,6) default 0,adh1_cost2 number(20,6) default 0,adh2_thick number(20,6) default 0,adh2_dens number(20,6) default 0,adh2_gsm number(20,6) default 0,adh2_rm number(20,6) default 0,adh2_price1 number(20,6) default 0,adh2_price2 number(20,6) default 0,adh2_cost1 number(20,6) default 0,adh2_cost2 number(20,6) default 0,tot_gsm number(20,6) default 0,tot_rm number(20,6) default 0,tot_price1 number(20,6) default 0,tot_price2 number(20,6) default 0,wastage number(20,6) default 0,wastage_price1 number(20,6) default 0,wastage_price2 number(20,6) default 0,solvent_price1 number(20,6) default 0,solvent_price2 number(20,6) default 0,zipper1 number(20,6) default 0,zipper2 number(20,6) default 0,zipper3 number(20,6) default 0,zipper4 number(20,6) default 0,packglue1 number(20,6) default 0,packglue2 number(20,6) default 0,packglue3 number(20,6) default 0,packglue4 number(20,6) default 0,packpet1 number(20,6) default 0,packpet2 number(20,6) default 0,packpet3 number(20,6) default 0,packpet4 number(20,6) default 0,ctn number(20,6) default 0,bobbin1 number(20,6) default 0,bobbin2 number(20,6) default 0,tot_rmcostkg1 number(20,6) default 0,tot_rmcostkg2 number(20,6) default 0,convextcost number(20,6) default 0,convexthr number(20,6) default 0,convexttot number(20,6) default 0,convrotocost number(20,6) default 0,convrotohr number(20,6) default 0,convrototot number(20,6) default 0,convbobstcost number(20,6) default 0,convbobsthr number(20,6) default 0,convbobsttot number(20,6) default 0,convcicost number(20,6) default 0,convcihr number(20,6) default 0,convcitot number(20,6) default 0,convlamcost number(20,6) default 0,convlamhr number(20,6) default 0,convlamtot number(20,6) default 0,convslitcost number(20,6) default 0,convslithr number(20,6) default 0,convslittot number(20,6) default 0,convpouchcost number(20,6) default 0,convpouchhr number(20,6) default 0,convpouchtot number(20,6) default 0,convbagchickencost number(20,6) default 0,convbagchickenhr number(20,6) default 0,convbagchickentot number(20,6) default 0,convbaggencost number(20,6) default 0,convbaggenhr number(20,6) default 0,convbaggentot number(20,6) default 0,convtot number(20,6) default 0,convmachcost number(20,6) default 0,convfuel1 number(20,6) default 0,convfuel2 number(20,6) default 0,convfuel3 number(20,6) default 0,convmackg1 number(20,6) default 0,convmackg2 number(20,6) default 0,convpower1 number(20,6) default 0,convpower2 number(20,6) default 0,convcharger1 number(20,6) default 0,convcharger2 number(20,6) default 0,convlabour1 number(20,6) default 0,convlabour2 number(20,6) default 0,convfrght1 number(20,6) default 0,convfrght2 number(20,6) default 0,convtotkg number(20,6) default 0,convprod1 number(20,6) default 0,convprod2 number(20,6) default 0,convmgmt1 number(20,6) default 0,convmgmt2 number(20,6) default 0,convfin1 number(20,6) default 0,convfin2 number(20,6) default 0,convfinaltotkg1 number(20,6) default 0,convfinaltotkg2 number(20,6) default 0,extcost number(20,6) default 0,exthr number(20,6) default 0,exttot number(20,6) default 0,rotocost number(20,6) default 0,rotohr number(20,6) default 0,rototot number(20,6) default 0,bobstcost number(20,6) default 0,bobsthr number(20,6) default 0,bobsttot number(20,6) default 0,cicost number(20,6) default 0,cihr number(20,6) default 0,citot number(20,6) default 0,lamcost number(20,6) default 0,lamhr number(20,6) default 0,lamtot number(20,6) default 0,slitcost number(20,6) default 0,slithr number(20,6) default 0,slittot number(20,6) default 0,pouchcost number(20,6) default 0,pouchhr number(20,6) default 0,pouchtot number(20,6) default 0,bagchickencost number(20,6) default 0,bagchickenhr number(20,6) default 0,bagchickentot number(20,6) default 0,baggencost number(20,6) default 0,baggenhr number(20,6) default 0,baggentot number(20,6) default 0,totcost number(20,6) default 0,labourcostkg number(20,6) default 0,perpcprice number(20,6) default 0,perpcfills number(20,6) default 0,orderpcs number(20,6) default 0,orderkgs number(20,6) default 0,amortize1 number(20,6) default 0,amortize2 number(20,6) default 0,amortize3 number(20,6) default 0,amortize4 number(20,6) default 0,amortize5 number(20,6) default 0,amortize6 number(20,6) default 0,current1 number(20,6) default 0,current2 number(20,6) default 0,current3 number(20,6) default 0,current4 number(20,6) default 0,current5 number(20,6) default 0,current6 number(20,6) default 0,remarks varchar2(100) default '-',cyact number(20,6) default 0,cypaid number(20,6) default 0,cyfills number(20,6) default 0,cyplate number(20,6) default 0,cycircum number(20,6) default 0,cyamortize number(20,6) default 0,cysupp number(20,6) default 0,cyorder number(20,6) default 0,flapw number(20,6) default 0,flapl number(20,6) default 0,flapthick number(20,6) default 0,flapdown number(20,6) default 0,flapl2 number(20,6) default 0,flapthick2 number(20,6) default 0,flapwt number(20,6) default 0,flapdownwt number(20,6) default 0,gluezipper number(20,6) default 0,bagpiece number(20,6) default 0,piecemtr number(20,6) default 0,zippermtr number(20,6) default 0,bagw number(20,6) default 0,bagl number(20,6) default 0,bagwt number(20,6) default 0,packingbagwt number(20,6) default 0,packingmode number(20,6) default 0,pkt number(20,6) default 0,sticker1 number(20,6) default 0,sticker2 number(20,6) default 0,sticker3 number(20,6) default 0,rod1 number(20,6) default 0,rod2 number(20,6) default 0,rod3 number(20,6) default 0,washer1 number(20,6) default 0,washer2 number(20,6) default 0,washer3 number(20,6) default 0,others1 number(20,6) default 0,others2 number(20,6) default 0,others3 number(20,6) default 0,packingtot number(20,6) default 0,for1kg number(20,6) default 0,ent_by varchar2(20) default '-',ent_dt date default sysdate,edt_by varchar2(20) default '-',edt_dt date default sysdate)";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                    }
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_PRECOST_RAW'", "TNAME");
                    if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_PRECOST_RAW (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR(6),VCHDATE DATE,srno number(5),ICODE VARCHAR(8),COLHEAD VARCHAR(5),RMATHEAD VARCHAR(50),NUM1 NUMBER(14,4),NUM2 NUMBER(14,4),NUM3 NUMBER(14,4),NUM4 NUMBER(14,4),NUM5 NUMBER(14,4),NUM6 NUMBER(14,4),NUM7 NUMBER(14,4),NUM8 NUMBER(14,4),ENT_BY VARCHAR(20),ENT_DT DATE,EDT_BY VARCHAR(20),EDT_DT DATE )");

                    ICO.add_icon(frm_qstr, "F10186C", 3, "Detailed Flexible Costing", 3, "../tej-base/om_pre_cost_SPPI.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                }
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F05199A", 3, "Multi-Department Live Charts 2", 3, "../tej-base/deskdash2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25144A", 3, "Caret Register", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");
                opts_wfin.Icon_Maint(frm_qstr, frm_cocd);
                opts_wfin.IconMouldMaint(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10191", 3, "Around Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10192", 3, "Cylinder Costing", 3, "../tej-base/om_Cylind_Cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193", 3, "Paper Rate Master", 3, "../tej-base/om_Matl_Master.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10193V", 3, "Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                //ICO.add_icon(frm_qstr, "F10195", 3, "Trim Wastage", 3, "../tej-base/om_trim_wstg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193Q", 3, "Quality/Foil Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10196", 3, "Label Costing", 3, "../tej-base/om_lbl_cost_MLAB.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10197", 3, "Label Costing (With Cyl)", 3, "../tej-base/om_lbl_cost_SPPI.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                if (frm_cocd == "PKGW")
                {
                    ICO.add_icon(frm_qstr, "F60000", 1, "Customer Support System", 3, "-", "-", "Y", "-", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F93000", 2, "Finsys OMS", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F93100", 3, "OMS Activity", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F93101", 4, "OMS Plan", 3, "../tej-base/om_oms_Plan.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F93106", 4, "OMS Followup", 3, "../tej-base/om_oms_folo.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");

                    ICO.add_icon(frm_qstr, "F93116", 3, "OMS Reports", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F93121", 4, "OMS Person Wise ", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F93126", 4, "OMS Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F93131", 4, "OMS Tgt VS Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F93132", 4, "OMS Team Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F93133", 4, "OMS Client Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                }

                //opts_wfin.iconDrawingModule(frm_qstr, frm_cocd);
                break;
            case "SPKS":
                // corr costing form added on 12/11/18 - on req of Bhupesh
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40171", 3, "Packaging Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40351", 4, "Corrugation Process Plan Detail", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10186", 3, "BOM Cost vs Sale Cost", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                // gate entry added on 26/02/2018 - on req of Bhupesh ji 
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                break;
            case "BONY":
                // vendor side auto dr cr form added on 12/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F70120", 3, "Auto Debit Credit Note(Vendor)", 3, "../tej-base/findCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;
            case "PPCL":
                // task mngmt, Pay Advice form added on 12/11/18 - on req of Bansal Sir
                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15100", 2, "Purchase Activity", 3, "-", "-", "Y", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15106", 3, "Purchase Orders Entry", 3, "../tej-base/om_po_entry.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                break;
            case "PMS":
                // pay advice form added on 12/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "YPPL":
                // added on 17/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                // made by suman
                ICO.add_icon(frm_qstr, "F35229", 4, "Paper Variation Report Code", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                //----------------------------------                               
                break;
            case "OPPL":
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                // added on 17/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                //----------------------------------                               
                break;
            case "SRIS":
                // added on 23/11/18 - replaced wfinsys_erp to tej-wfin at client side
                opts_wfin.IconCustomerRequestSELStyle(frm_qstr, frm_cocd);
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50148", 4, "Outgoing Freight Recording", 3, "../tej-base/rpt.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15470", 2, "Document Keeping", 3, "-", "-", "Y", "fin15_e7", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15471", 3, "Purchase Order Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin15_e7", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15472", 3, "Uploaded PO Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin15_e7", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15473", 3, "Uploaded PO View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin15_e7", "fin15_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F80000", 1, "H.R.M Module", 3, "-", "-", "Y", "-", "fin80_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F83000", 2, "Reports", 3, "-", "-", "Y", "fin83_e1", "fin80_a1", "fin83pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F83001", 3, "Task(s) List", 3, "../tej-base/om_view_hrm.aspx", "-", "Y", "fin83_e1", "fin80_a1", "fin83pp_e1", "fa-edit", "Y", "Y");
                break;
            case "JSHP":
                // added on 03/12/18 - on req of rahul ji
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                break;
            case "SEL":
                // added Attn Uploading on req of bansal ji
                ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85100", 2, "Payroll Activity", 3, "-", "-", "Y", "fin85_e1", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85104", 3, "Daily Attendance Uploading", 3, "../tej-base/om_attn_upl.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10350", 2, "Service Module", 1, "-", "-", "-", "fin10_e8", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10351", 3, "Srv. Req Entry", 1, "../tej-base/bsrv_action.aspx", "-", "-", "fin10_e8", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10352", 3, "Action by HO", 1, "../tej-base/bsrv_action.aspx", "-", "-", "fin10_e8", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10353", 3, "Action by Engineer", 1, "../tej-base/bsrv_action.aspx", "-", "-", "fin10_e8", "fin10_a1", "-", "fa-edit");
                break;
            case "SELH":
                // added Attn Uploading on req of arvind ji
                ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85100", 2, "Payroll Activity", 3, "-", "-", "Y", "fin85_e1", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85104", 3, "Daily Attendance Uploading", 3, "../tej-base/om_attn_upl.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");
                break;
            case "SURY":
                ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20101", 3, "Gate Inward Entry", 3, "../tej-base/om_gate_inw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "UNIQ":
            case "JRAJ":
            case "JGLO":
            case "JGLR":
                // for self data dr cr notes                    
                opts_wfin.Icon_DrCr_self(frm_qstr, frm_cocd);
                opts_wfin.Icon_Visitor(frm_qstr, frm_cocd);
                opts_wfin.Icon_Cust_port(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Supp_port(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                // Bill upload against MRR
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85141", 2, "Salary Reports", 3, "-", "-", "Y", "fin85_e4", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85146", 3, "More Reports(Pay)", 3, "-", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F85234", 4, "Welfare Fund Upload", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");

                break;
            case "IPP":
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                //* for testing only - should remove after testing

                ICO.add_icon(frm_qstr, "F50271", 4, "Invoice E-Mail", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                break;
            case "NAHR":
                // corr costing form added on 21/12/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                // ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25131", 2, "Stock Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25133A", 3, "RM Stock Report Inward vs Outward", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");


                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                break;
            case "MINV":
                // added on 24/12/18 - on req of Bansal Sir
                opts_wfin.Icon_Loan_Req(frm_qstr, frm_cocd);
                opts_wfin.Icon_Cust_port(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Supp_port(frm_qstr, frm_cocd);

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10135S", 3, "Create SDR", 3, "../tej-base/om_upd_sdr.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                opts_wfin.Icon_Purch(frm_qstr, frm_cocd);
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Acctg(frm_qstr, frm_cocd);

                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F80000", 1, "H.R.M Module", 3, "-", "-", "Y", "-", "fin80_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F82700", 2, "Online HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50125", 4, "Make Pick List", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");

                // ------------------------------------------------------------------
                // Leave Request Module
                // ------------------------------------------------------------------
                ICO.add_icon(frm_qstr, "F81000", 2, "Leave Mgmt Module", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F81100", 3, "Leave Mgmt Activity", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F81101", 4, "Leave Request", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F81106", 4, "Leave Req Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F81111", 4, "Leave Req Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F81121", 3, "Leaves Checklist", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin81pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F81126", 4, "Request Checklist", 3, "../tej-base/om_view_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F81127", 4, "Approval Checklist", 3, "../tej-base/om_view_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e2", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F81131", 3, "Leaves Analysis", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin81pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F81132", 4, "Leave Mgmt Dashboard", 3, "../tej-base/om_dbd_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e2", "fa-edit");

                ICO.add_icon(frm_qstr, "F82700", 3, "Online HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "fin82pp_e7", "fa-edit");
                ICO.add_icon(frm_qstr, "F82703", 4, "Leave Request", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin82pp_e7", "fa-edit");
                ICO.add_icon(frm_qstr, "F82705", 4, "Loan Request", 3, "../tej-base/om_loan_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin82pp_e7", "fa-edit");

                ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85121", 2, "Loan/Advance Mgt", 3, "-", "-", "Y", "fin85_e2", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85126", 3, "Employee Advance", 3, "../tej-base/om_pay_Adv.aspx", "-", "-", "fin85_e2", "fin85_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F85127", 3, "Employee Loan", 3, "../tej-base/om_pay_Loan.aspx", "-", "-", "fin85_e2", "fin85_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");


                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                opts_wfin.IconRFQ_PO(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F05125", 3, "PMRC Cost Report (BOM based)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125a", 3, "PMRC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_Visitor(frm_qstr, frm_cocd);

                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10550", 3, "Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10551", 3, "Type of Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10552", 3, "Department Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10553", 3, "Person Master", 3, "../tej-base/personmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

                    Opts_wfin ipp_opts_wfin = new Opts_wfin();
                    ipp_opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                }
                break;
            case "SINT":
                // added on 27/12/18 - on req of Bansal Sir
                opts_wfin.IconCustomerRequestSELStyle(frm_qstr, frm_cocd);
                break;
            case "KPFL":
                // added on 29/12/18 - on req of Virender Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                break;
            case "VICT":
                // added on 29/12/18 - on req of Bansal Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "VMAG":
                // added on 29/12/18 - on req of Bansal Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "RINT":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "XDIL":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "MEL1":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                // added 29/05/20 -- on req of vs sir
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "SKYP":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "MPUD":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "TECD":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "SUNB":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "ARVI":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "CCC":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "FCCL":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "CENL":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "PRES":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "BESO":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "LRFP":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                //27 03 2020 -- Bansal Sir
                opts_wfin.Icon_Supp_port(frm_qstr, frm_cocd);
                opts_wfin.Icon_Cust_port(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25118", 3, "Rejection Entry", 3, "../tej-base/om_cust_rej.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25144A", 3, "Crate Register Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F25144B", 3, "Crate Register Detail", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");

                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F47108", 4, "Target Despatch", 3, "../tej-base/om_disptgt.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CUST_REJ'", "TNAME");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CUST_REJ (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,ACODE CHAR(10),ICODE CHAR(10),SRNO NUMBER(4),RGPNUM CHAR(6),RGPDATE DATE,IQTYREJ NUMBER(12,4),PRNUM VARCHAR2(20),PRDATE DATE,T_DEPTT VARCHAR2(30),REMARKS VARCHAR2(500),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE)");

                break;
            case "KLEX":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "RRP":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "BIOM":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "ADWA":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "JSIN":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "SARN":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "CNS":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "CHEM":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "MPPL":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "PCCL":
                // added on 31/12/18 - on req of SKG Sir
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "AMAR":
                // added on 04/01/19 - on req of Virender Sir
                opts_wfin.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                break;
            case "CMPL":
                // added on 08/01/19 - on req of Ashok Ji
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F71212", 3, "Downtime News", 3, "../tej-base/om_new_dboard.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                //opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                //opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);

                break;
            case "SPP":
                // added on 08/01/19 - on req of Ashok Ji
                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "ADPF":
                // added on 15/01/19 - on req of SKG Sir
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");

                Opts_wfin ADdrcr_honda_icons = new Opts_wfin();
                ADdrcr_honda_icons.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "P70099S", 3, "Upload Old Invoice DR/CR", 3, "../tej-base/autoDrCrSaip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70162", 3, "Monthly Payable Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "KCOR":
                // added on 17/01/19 - on req of Bhupesh Sir
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F40116", 1, "QC Reason Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "SAGE":
            case "SAGM":
                // added on 18/01/19 - on req of Bansal Sir
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10184", 3, "FG Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10194E", 3, "Valuation on BOM Costing(Expendable)", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194F", 3, "Valuation on BOM Costing(Expendable) fifo", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10198", 3, "RM,FG Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10198W", 3, "WIP Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125C", 3, "Sales vs RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F05125D", 3, "Store Variance Report ", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125E", 3, "WIP Variance Report ", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                // added 25/01/19 - on req of bansal sir
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "PCEE":
                // added on 10/07/19 - on req of pkg Sir
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10184", 3, "FG Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125C", 3, "Sales vs RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F05125D", 3, "Store Variance Report ", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125E", 3, "WIP Variance Report ", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                // added 10/07/19 - on req of pkg sir
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70480", 4, "Multi Excel Upload", 3, "../tej-base/om_any_upload.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

                opts_wfin.PremiumEmktgReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F49202", 4, "SO analysis-Customer Qty wise", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49203", 4, "SO analysis-Customer Value wise", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49204", 4, "SO analysis-Customer Qty wise", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49205", 4, "SO analysis-Customer Value wise", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49206", 4, "SO Acceptance", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49207", 4, "SO Acceptance-Detailed", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49208", 4, "SO Shipment Plan", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49209", 4, "SO Monthwise Summary", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49210", 4, "Estimated Delivery Schedule", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49211", 4, "Goods Status ", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55000", 2, "Export Sales Module", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55145", 4, "Export Invoice- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55146", 4, "Packing List- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55111", 4, "Dispatch Advice (Exp.)", 3, "../tej-base/om_Da_entry.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F49140", 3, "Exp.Order Reports", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F49149", 4, "Invoice wise RM Metallurgy", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F55500", 2, "Export Licence Management", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55502", 3, "Export Licence Master", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit");

                ICO.add_icon(frm_qstr, "F55503", 3, "Advance Licence", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55504", 3, "EPCG Licence", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit");
                ICO.add_icon(frm_qstr, "F55505", 3, "Shipping Master", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit");


                ICO.add_icon(frm_qstr, "F55511", 4, "Advance Licence Master ", 3, "../tej-base/om_Advlic_mast.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55512", 4, "Advance Licence Adj-Import ", 3, "../tej-base/om_Implic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55513", 4, "Advance Licence Adj-Export ", 3, "../tej-base/om_Explic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55514", 4, "Advance Licence Report-Import ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55515", 4, "Advance Licence Report-Export ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55516", 4, "Advance Licence Report-Summary ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F55517", 4, "EPCG License Master ", 3, "../tej-base/om_EPCG_Advlic_mast.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55518", 4, "EPCG Import Adj ", 3, "../tej-base/om_EPCG_Implic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55519", 4, "EPCG Export Adj ", 3, "../tej-base/om_EPCG_Explic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55520", 3, "Annexure Custom Filing ", 3, "../tej-base/om_Anex_Cust_Fil.aspx", "-", "-", "fin55_e2", "fin50_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55521", 4, "Container Master ", 3, "../tej-base/om_Contain_detail.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55522", 4, "Shipment Tracking Report ", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55523", 4, "Customer Wise Freight Report ", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F55524", 4, "Forwarding Agent Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55525", 4, "Shipping Line Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55526", 4, "Nature Of Shipment Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55527", 4, "Freight Chart", 3, "../tej-base/om_Freight_Chart.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                break;
            case "WPPL":
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F10186", 3, "BOM Cost vs Sale Cost", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                break;
            case "NIRM":
            case "PRAG":
                // added 31/01/19 - on req of pkg sir
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50101", 4, "Sales Invoice (Dom.)", 3, "../tej-base/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
                break;
            case "UKB":
                // added 08/02/19 - on req of pkg sir
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F05125", 3, "RMC Cost Report (BOM based)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                break;
            case "PRIN":
            case "DEMO":
                ICO.add_icon(frm_qstr, "F10134", 3, "Laminate BOM", 3, "../tej-base/om_bom_lami.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10134A", 3, "Poly BOM", 3, "../tej-base/om_bom_lami.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F05199A", 3, "Multi-Department Live Charts 2", 3, "../tej-base/deskdash2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F05199A", 3, "Multi-Department Live Charts 2", 3, "../tej-base/deskdash2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");

                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Prodn_plast(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25108", 3, "Matl Inward Import", 3, "../tej-base/om_mrr_edi.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                {

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10049", 2, "Customer Care", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10050", 3, "Customer Request", 3, "../tej-base/cmplnt.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10051", 3, "Customer Request Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10052", 3, "Action on Request", 3, "../tej-base/neopaction.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10055", 3, "Old M/C Entry", 3, "../tej-base/oldMcData.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10550", 3, "Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10551", 3, "Type of Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10552", 3, "Department Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10553", 3, "Person Master", 3, "../tej-base/personmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10052S", 3, "Request Summary", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
                    ICO.add_icon(frm_qstr, "F10053", 3, "Request Status", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
                    //ICO.add_icon(frm_qstr, "F10054", 3, "Request Status Print", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");


                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

                    ICO.add_icon(frm_qstr, "F10056", 3, "Lead Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    Opts_wfin ipp_opts_wfin = new Opts_wfin();
                    ipp_opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                }

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50131", 3, "Dom.Sales Checklists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F50137A", 4, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F50137B", 4, "Truck Entry Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50274", 4, "Production Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F47120", 4, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F47125", 4, "Supervisior Master", 3, "../tej-base/personmst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                // prin work - creating icon in merp for testing only
                opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                break;
            case "LOGW":
            case "ROOP":
                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");

                // task mngmt added on 16/10/19 - on req of Bansal Sir
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F05125C", 3, "Sales vs RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "CLPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "PRIN*":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70374", 3, "Pending Voucher List to Upload", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                // added 25/01/19 - on req of bansal sir
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50131", 3, "Dom.Sales Checklists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F50137A", 4, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);


                // ADDED 
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10184", 3, "FG Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125C", 3, "Sales vs RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "DREM":
                // added 22/03/19 - on req of pkg sir
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F05125", 3, "RMC Cost Report (BOM based)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                break;
            case "ATOP":
                // added 27/03/19 - on req of bansal sir
                opts_wfin.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                break;
            case "SAIA":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25108", 3, "Matl Inward Import", 3, "../tej-base/om_mrr_edi.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47161", 3, "Dom.Order Masters", 3, "-", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47118", 4, "Discount Master", 3, "../tej-base/om_discstruc.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F47118R", 4, "Sales Profitability Report", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15100", 2, "Purchase Activity", 3, "-", "-", "Y", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15106", 3, "Purchase Orders Entry", 3, "../tej-base/om_po_entry.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10100", 2, "Items Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10101", 3, "Item Main Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10106", 3, "Item Sub Groups", 3, "../tej-base/Isub_Grp.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10111", 3, "General Items", 3, "../tej-base/item_gen.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10116", 3, "FG/SFG Items", 3, "../tej-base/item_gen.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70172", 3, "Accounts Master", 3, "../tej-base/acct_gen.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10184", 3, "RM,FG Valuation", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                //ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10198", 3, "RM,FG Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                //ICO.add_icon(frm_qstr, "F10198W", 3, "WIP Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                //ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                //ICO.add_icon(frm_qstr, "F05125C", 3, "Sales vs RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_Acctg(frm_qstr, frm_cocd);
                break;
            case "MULT":
                //opts_wfin.Icon_Prodn_plast(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25137A", 4, "Item Status Report", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "STLC":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F39121", 3, "Moulding Prodn Checklists", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F40137", 4, "Trend of Rejection", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40138", 4, "Trend of DownTime", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40126", 4, "Daily Prodn Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40128", 4, "Down Time Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40129", 4, "Rejection Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F39140", 3, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F40143", 4, "Production with Rej % Itemwise", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40145", 4, "Production Log Print", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40132", 4, "Daily Prodn Report", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40133", 4, "Mthly Prodn Report", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");

                // these are correct icons , all above are in prodpp to be shifted to prodpm....
                ICO.add_icon(frm_qstr, "F39190", 4, "Details of Items Produced-Qty", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39192", 4, "Details of Items Rejected-Qty", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);

                // stlc 06/07/2020 - pkg sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");

                //ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F10185A", 3, "Duplex Costing", 3, "../tej-base/cost_infi_t.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F10185B", 3, "Duplex Costing 2", 3, "../tej-base/duplx_cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10188", 3, "Label Costing Sheet", 3, "../tej-base/om_label_costing.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                {
                    ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40171", 3, "Label Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                }

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_PRECOST'", "TNAME");
                if (mhd == "0" || mhd == "")
                {
                    string SQuery = "create table wb_precost ( branchcd varchar2(2) default '-',type varchar2(2) default '-',vchnum varchar2(6) default '-',vchdate date default sysdate,acode varchar2(6) default '-',icode varchar2(8) default '-',aname varchar2(150) default '-',iname varchar2(150) default '-',structure varchar2(30) default '-',print_type varchar2(30) default '-',lpo_no varchar2(30) default '-',order_qty number(20,6) default 0,cyl_amor number(20,6) default 0,color number(20,6) default 0,pet_thick number(20,6) default 0,pet_dens number(20,6) default 0,pet_gsm number(20,6) default 0,pet_rm number(20,6) default 0,pet_price1 number(20,6) default 0,pet_price2 number(20,6) default 0,pet_cost1 number(20,6) default 0,pet_cost2 number(20,6) default 0,met_thick number(20,6) default 0,met_dens number(20,6) default 0,met_gsm number(20,6) default 0,met_rm number(20,6) default 0,met_price1 number(20,6) default 0,met_price2 number(20,6) default 0,met_cost1 number(20,6) default 0,met_cost2 number(20,6) default 0,lpde_thick number(20,6) default 0,lpde_dens number(20,6) default 0,lpde_gsm number(20,6) default 0,lpde_rm number(20,6) default 0,lpde_price1 number(20,6) default 0,lpde_price2 number(20,6) default 0,lpde_cost1 number(20,6) default 0,lpde_cost2 number(20,6) default 0,ink_thick number(20,6) default 0,ink_dens number(20,6) default 0,ink_gsm number(20,6) default 0,ink_rm number(20,6) default 0,ink_price1 number(20,6) default 0,ink_price2 number(20,6) default 0,ink_cost1 number(20,6) default 0,ink_cost2 number(20,6) default 0,adh1_thick number(20,6) default 0,adh1_dens number(20,6) default 0,adh1_gsm number(20,6) default 0,adh1_rm number(20,6) default 0,adh1_price1 number(20,6) default 0,adh1_price2 number(20,6) default 0,adh1_cost1 number(20,6) default 0,adh1_cost2 number(20,6) default 0,adh2_thick number(20,6) default 0,adh2_dens number(20,6) default 0,adh2_gsm number(20,6) default 0,adh2_rm number(20,6) default 0,adh2_price1 number(20,6) default 0,adh2_price2 number(20,6) default 0,adh2_cost1 number(20,6) default 0,adh2_cost2 number(20,6) default 0,tot_gsm number(20,6) default 0,tot_rm number(20,6) default 0,tot_price1 number(20,6) default 0,tot_price2 number(20,6) default 0,wastage number(20,6) default 0,wastage_price1 number(20,6) default 0,wastage_price2 number(20,6) default 0,solvent_price1 number(20,6) default 0,solvent_price2 number(20,6) default 0,zipper1 number(20,6) default 0,zipper2 number(20,6) default 0,zipper3 number(20,6) default 0,zipper4 number(20,6) default 0,packglue1 number(20,6) default 0,packglue2 number(20,6) default 0,packglue3 number(20,6) default 0,packglue4 number(20,6) default 0,packpet1 number(20,6) default 0,packpet2 number(20,6) default 0,packpet3 number(20,6) default 0,packpet4 number(20,6) default 0,ctn number(20,6) default 0,bobbin1 number(20,6) default 0,bobbin2 number(20,6) default 0,tot_rmcostkg1 number(20,6) default 0,tot_rmcostkg2 number(20,6) default 0,convextcost number(20,6) default 0,convexthr number(20,6) default 0,convexttot number(20,6) default 0,convrotocost number(20,6) default 0,convrotohr number(20,6) default 0,convrototot number(20,6) default 0,convbobstcost number(20,6) default 0,convbobsthr number(20,6) default 0,convbobsttot number(20,6) default 0,convcicost number(20,6) default 0,convcihr number(20,6) default 0,convcitot number(20,6) default 0,convlamcost number(20,6) default 0,convlamhr number(20,6) default 0,convlamtot number(20,6) default 0,convslitcost number(20,6) default 0,convslithr number(20,6) default 0,convslittot number(20,6) default 0,convpouchcost number(20,6) default 0,convpouchhr number(20,6) default 0,convpouchtot number(20,6) default 0,convbagchickencost number(20,6) default 0,convbagchickenhr number(20,6) default 0,convbagchickentot number(20,6) default 0,convbaggencost number(20,6) default 0,convbaggenhr number(20,6) default 0,convbaggentot number(20,6) default 0,convtot number(20,6) default 0,convmachcost number(20,6) default 0,convfuel1 number(20,6) default 0,convfuel2 number(20,6) default 0,convfuel3 number(20,6) default 0,convmackg1 number(20,6) default 0,convmackg2 number(20,6) default 0,convpower1 number(20,6) default 0,convpower2 number(20,6) default 0,convcharger1 number(20,6) default 0,convcharger2 number(20,6) default 0,convlabour1 number(20,6) default 0,convlabour2 number(20,6) default 0,convfrght1 number(20,6) default 0,convfrght2 number(20,6) default 0,convtotkg number(20,6) default 0,convprod1 number(20,6) default 0,convprod2 number(20,6) default 0,convmgmt1 number(20,6) default 0,convmgmt2 number(20,6) default 0,convfin1 number(20,6) default 0,convfin2 number(20,6) default 0,convfinaltotkg1 number(20,6) default 0,convfinaltotkg2 number(20,6) default 0,extcost number(20,6) default 0,exthr number(20,6) default 0,exttot number(20,6) default 0,rotocost number(20,6) default 0,rotohr number(20,6) default 0,rototot number(20,6) default 0,bobstcost number(20,6) default 0,bobsthr number(20,6) default 0,bobsttot number(20,6) default 0,cicost number(20,6) default 0,cihr number(20,6) default 0,citot number(20,6) default 0,lamcost number(20,6) default 0,lamhr number(20,6) default 0,lamtot number(20,6) default 0,slitcost number(20,6) default 0,slithr number(20,6) default 0,slittot number(20,6) default 0,pouchcost number(20,6) default 0,pouchhr number(20,6) default 0,pouchtot number(20,6) default 0,bagchickencost number(20,6) default 0,bagchickenhr number(20,6) default 0,bagchickentot number(20,6) default 0,baggencost number(20,6) default 0,baggenhr number(20,6) default 0,baggentot number(20,6) default 0,totcost number(20,6) default 0,labourcostkg number(20,6) default 0,perpcprice number(20,6) default 0,perpcfills number(20,6) default 0,orderpcs number(20,6) default 0,orderkgs number(20,6) default 0,amortize1 number(20,6) default 0,amortize2 number(20,6) default 0,amortize3 number(20,6) default 0,amortize4 number(20,6) default 0,amortize5 number(20,6) default 0,amortize6 number(20,6) default 0,current1 number(20,6) default 0,current2 number(20,6) default 0,current3 number(20,6) default 0,current4 number(20,6) default 0,current5 number(20,6) default 0,current6 number(20,6) default 0,remarks varchar2(100) default '-',cyact number(20,6) default 0,cypaid number(20,6) default 0,cyfills number(20,6) default 0,cyplate number(20,6) default 0,cycircum number(20,6) default 0,cyamortize number(20,6) default 0,cysupp number(20,6) default 0,cyorder number(20,6) default 0,flapw number(20,6) default 0,flapl number(20,6) default 0,flapthick number(20,6) default 0,flapdown number(20,6) default 0,flapl2 number(20,6) default 0,flapthick2 number(20,6) default 0,flapwt number(20,6) default 0,flapdownwt number(20,6) default 0,gluezipper number(20,6) default 0,bagpiece number(20,6) default 0,piecemtr number(20,6) default 0,zippermtr number(20,6) default 0,bagw number(20,6) default 0,bagl number(20,6) default 0,bagwt number(20,6) default 0,packingbagwt number(20,6) default 0,packingmode number(20,6) default 0,pkt number(20,6) default 0,sticker1 number(20,6) default 0,sticker2 number(20,6) default 0,sticker3 number(20,6) default 0,rod1 number(20,6) default 0,rod2 number(20,6) default 0,rod3 number(20,6) default 0,washer1 number(20,6) default 0,washer2 number(20,6) default 0,washer3 number(20,6) default 0,others1 number(20,6) default 0,others2 number(20,6) default 0,others3 number(20,6) default 0,packingtot number(20,6) default 0,for1kg number(20,6) default 0,ent_by varchar2(20) default '-',ent_dt date default sysdate,edt_by varchar2(20) default '-',edt_dt date default sysdate)";
                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                }
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_PRECOST_RAW'", "TNAME");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_PRECOST_RAW (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR(6),VCHDATE DATE,srno number(5),ICODE VARCHAR(8),COLHEAD VARCHAR(5),RMATHEAD VARCHAR(50),NUM1 NUMBER(14,4),NUM2 NUMBER(14,4),NUM3 NUMBER(14,4),NUM4 NUMBER(14,4),NUM5 NUMBER(14,4),NUM6 NUMBER(14,4),NUM7 NUMBER(14,4),NUM8 NUMBER(14,4),ENT_BY VARCHAR(20),ENT_DT DATE,EDT_BY VARCHAR(20),EDT_DT DATE )");

                //ICO.add_icon(frm_qstr, "F10186C", 3, "Detailed Flexible Costing", 3, "../tej-base/om_pre_cost_SPPI.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10191", 3, "Around Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10192", 3, "Cylinder Costing", 3, "../tej-base/om_Cylind_Cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193", 3, "Paper Rate Master", 3, "../tej-base/om_Matl_Master.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10193V", 3, "Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                //ICO.add_icon(frm_qstr, "F10195", 3, "Trim Wastage", 3, "../tej-base/om_trim_wstg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193Q", 3, "Quality/Foil Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10196", 3, "Label Costing", 3, "../tej-base/om_lbl_cost_MLAB.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10197", 3, "Label Costing (With Cyl)", 3, "../tej-base/om_lbl_cost_SPPI.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                break;
            case "OMNI":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50274", 4, "Production Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F05108", 3, "Performance MIS", 3, "../tej-base/om_mis_txt.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "MIRP":// added 08/04/19 - on req of bansal sir                
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_DrCr_self(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25120", 3, "Physical Verification Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F25123", 3, "Reel Stock Vs. Physical Verification", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25119", 3, "Missing Reels in Physical Verification", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");

                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                }

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                opts_wfin.Icon_DrCr_Maruti(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70120", 3, "Auto Debit Credit Note(Vendor)", 3, "../tej-base/findCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;
            case "ERAE":// added 09/04/19 - on req of rahul sir
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "VELV":
            case "NAHR*":
                //testing marketing
                //opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                //opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35100", 3, "Prt/Pkg PPC Activity", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35101", 4, "Job Order Creation", 3, "../tej-base/om_JCard_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");

                ICO.add_icon(frm_qstr, "F10135", 3, "Process Plan (Corrugation)", 3, "../tej-base/om_proc_plan.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
                break;
            case "TGIP":
                ICO.add_icon(frm_qstr, "F40116", 1, "QC Reason Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                //testing marketing
                opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);

                break;
            case "VPML":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "PHGL":
                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50221", 3, "More Reports(Dom.Sales)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F50269", 4, "Customer Cash Discount", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "F47121", 3, "Dom.Sales Approvals", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47126", 4, "Check S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F47127", 4, "Approve S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F49000", 2, "Export Sales Orders", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F49121", 3, "Exp.Sales Approvals", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F49126", 4, "Check S.O. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49127", 4, "Approve S.O. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit", "N", "Y");


                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

                    Opts_wfin ipp_opts_wfin = new Opts_wfin();
                    ipp_opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                }

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                break;
            case "JLAP":
                Opts_wfin drcr_honda_icons = new Opts_wfin();
                drcr_honda_icons.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "P70099S", 3, "Upload Old Invoice DR/CR", 3, "../tej-base/autoDrCrSaip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70162", 3, "Monthly Payable Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "WING":
                mhd = fgen.chk_RsysUpd("DMUL101");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DMUL101') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "DMUL101", "DEV_A");

                    ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F99108", 3, "DSC Activation", 3, "../tej-base/om_dsc_activate.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                }
                break;
            case "AEPL": //23/09/2019
                mhd = fgen.chk_RsysUpd("DMUL101");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DMUL101') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "DMUL101", "DEV_A");

                    ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F99108", 3, "DSC Activation", 3, "../tej-base/om_dsc_activate.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                }
                break;
            case "ELEC":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50101", 4, "Sales Invoice (Dom.)", 3, "../tej-base/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99108", 3, "DSC Activation", 3, "../tej-base/om_dsc_activate.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                //opts_wfin.Icon_Purch(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                //opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Mkt_ord_Exp(frm_qstr, frm_cocd);

                //opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                //opts_wfin.Icon_Mkt_Sale_Exp(frm_qstr, frm_cocd);
                break;
            case "MLGI":
                ICO.add_icon(frm_qstr, "F50101", 4, "Sales Invoice (Dom.)", 3, "../tej-base/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99108", 3, "DSC Activation", 3, "../tej-base/om_dsc_activate.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                //ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Activity", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70348", 3, "Cheque Deposit Slip", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");


                ICO.add_icon(frm_qstr, "F60000", 1, "Customer Support System", 3, "-", "-", "Y", "-", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F93000", 2, "Finsys OMS", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F93100", 3, "OMS Activity", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F93101", 4, "OMS Plan", 3, "../tej-base/om_oms_Plan.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F93106", 4, "OMS Followup", 3, "../tej-base/om_oms_folo.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F93116", 3, "OMS Reports", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F93121", 4, "OMS Person Wise ", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93126", 4, "OMS Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93131", 4, "OMS Tgt VS Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93132", 4, "OMS Team Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F93133", 4, "OMS Client Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                break;
            case "MIND":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10100", 2, "Items Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10130", 2, "Production Masters", 3, "-", "-", "Y", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10131", 3, "Bill of Materials", 3, "../tej-base/om_bom_ent.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

                opts_wfin.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                break;
            case "INFI":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                //15 04 2020
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70298", 4, "Cross Year Accounts Ledger-Print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                break;
            case "SCPL":
                opts_wfin.IconBoxCostSURY(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "MERP":
                // y hatana hai 
                //ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                //ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F50131", 3, "Dom.Sales Checklists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit");
                //ICO.add_icon(frm_qstr, "F50137A", 4, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10049", 2, "Customer Care", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10050", 3, "Customer Request", 3, "../tej-base/om_cmplnt.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10051", 3, "Customer Request Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10052", 3, "Action on Request", 3, "../tej-base/neopaction.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10055", 3, "Old M/C Entry", 3, "../tej-base/oldMcData.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10550", 3, "Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10551", 3, "Type of Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10552", 3, "Department Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10553", 3, "Person Master", 3, "../tej-base/personmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10052S", 3, "Request Summary", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
                    ICO.add_icon(frm_qstr, "F10053", 3, "Request Status", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
                    //ICO.add_icon(frm_qstr, "F10054", 3, "Request Status Print", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");


                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");


                    ICO.add_icon(frm_qstr, "F10056", 3, "Lead Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

                    Opts_wfin ipp_opts_wfin = new Opts_wfin();
                    ipp_opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                }

                // prin work - creating icon in merp for testing only
                opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);


                break;
            case "OTTO":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "KLAS":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25121", 2, "Inventory Checklists", 3, "-", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25130", 3, "Material Tagging Reports", 3, "../tej-base/om_view_invn.aspx", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25130V", 3, "Vessel Transfer Reports", 3, "../tej-base/om_view_invn.aspx", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30364", 2, "QA Master", 3, "-", "-", "-", "fin30_f1", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30368", 3, "Defect Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin30_f1", "fin30_a1", "fin30_QAMST", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");

                //new rep added 04/07
                ICO.add_icon(frm_qstr, "F50140A", 3, "Sales Web Reports", 3, "../tej-base/om_Web_Rpt_KLAS_SALE.aspx", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");


                //ICO.add_icon(frm_qstr, "F25144C", 3, "Challan DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                //ICO.add_icon(frm_qstr, "F25144M", 3, "MRR DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15135P", 3, "P.O. DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F47142S", 4, "S.O. DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50143I", 4, "Invoice DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70146A", 3, "Voucher DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");


                //ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30116", 2, "Quality Checklists", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
                //new rep added 04/07
                ICO.add_icon(frm_qstr, "F30116A", 3, "QA Web Reports", 3, "../tej-base/om_Web_Rpt_KLAS_QA.aspx", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F38050A", 2, "Mfg Web Reports", 3, "../tej-base/om_Web_Rpt_KLAS_MFG.aspx", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                break;
            case "PCON":
                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                break;
            case "PROG":
                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                break;
            case "ZEEP":
                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                // added on 17/01/19 - on req of Bhupesh Sir
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F40116", 1, "QC Reason Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                break;
            case "GCAP":
            case "GDOT":
            case "SEFL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                // 21/01/2020 -- skg sir
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "SEPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                // on req of bansal sir 03/10/2019
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                break;
            case "SVPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25198B", 3, "Single Reel Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F40329", 3, "Batch Wise Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F39257", 4, "Downtime Details Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39258", 4, "Operator Details Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                break;
            case "HIMT":
            case "GLOB":
            case "HIMO":
            case "HIMS":
            case "AARH":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50221", 3, "More Reports(Dom.Sales)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F50275", 4, "Sale Quantity Report", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50276", 4, "State Sales Summary , Monthwise Report With Qty And Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50277", 4, "Statewise, Groupwise, Subgroupwise, Sale Summary Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");

                // 21/01/2020 -- skg sir
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "PANO":
                opts_wfin.PremiumEmktgReport(frm_qstr, frm_cocd);
                opts_wfin.Icon_FA_sys(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F49181", 4, "Export Bill details", 3, "../tej-base/om_exp_reg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F49185", 4, "Import Bill details", 3, "../tej-base/om_imp_reg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F55500", 2, "Export Licence Management", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55502", 3, "Export Licence Master", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit");

                ICO.add_icon(frm_qstr, "F55503", 3, "Advance Licence", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55504", 3, "EPCG Licence", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit");
                ICO.add_icon(frm_qstr, "F55505", 3, "Shipping Master", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit");


                ICO.add_icon(frm_qstr, "F55511", 4, "Advance Licence Master ", 3, "../tej-base/om_Advlic_mast.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55512", 4, "Advance Licence Adj-Import ", 3, "../tej-base/om_Implic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55513", 4, "Advance Licence Adj-Export ", 3, "../tej-base/om_Explic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55514", 4, "Advance Licence Report-Import ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55515", 4, "Advance Licence Report-Export ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55516", 4, "Advance Licence Report-Summary ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F55517", 4, "EPCG License Master ", 3, "../tej-base/om_EPCG_Advlic_mast.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55518", 4, "EPCG Import Adj ", 3, "../tej-base/om_EPCG_Implic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55519", 4, "EPCG Export Adj ", 3, "../tej-base/om_EPCG_Explic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55520", 3, "Annexure Custom Filing ", 3, "../tej-base/om_Anex_Cust_Fil.aspx", "-", "-", "fin55_e2", "fin50_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55521", 4, "Container Master ", 3, "../tej-base/om_Contain_detail.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55522", 4, "Shipment Tracking Report ", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55523", 4, "Customer Wise Freight Report ", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F55524", 4, "Forwarding Agent Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55525", 4, "Shipping Line Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55526", 4, "Nature Of Shipment Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55527", 4, "Freight Chart", 3, "../tej-base/om_Freight_Chart.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                break;
            case "DISP":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39100", 3, "Prodn Activity", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F39140", 3, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F39221", 4, "Manpower Efficiency Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39222", 4, "Machine Efficiency Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39223", 4, "Machine Utlisation", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39224", 4, "Daily Production Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39224A", 4, "Daily Production Report(Excel)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39225", 4, "Shift Production Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F39226", 4, "Production Summary Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39227", 4, "Rejection Analysis Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39228", 4, "Monthly Breakdown Report ", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F39229", 4, "Runner Consumption Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39230", 4, "Rejection tfr Slip", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39231", 4, "Moulding to Component Store", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39232", 4, "Mould Utilization Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F39275", 4, "Production MIS Line Wise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F39183", 4, "Item Below Min. Level (Component Store)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39251", 4, "Goods Imported -Annexure III", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
                // 25/11/2019 - by bansal sir
                opts_wfin.Icon_Visitor(frm_qstr, frm_cocd);
                break;
            case "JSGI":
                opts_wfin.Icon_DrCr_Maruti(frm_qstr, frm_cocd);
                break;
            case "MAYU":
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                break;
            case "ECPL": //19/09/2019 BY PKGS SIR
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15100", 2, "Purchase Activity", 3, "-", "-", "Y", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15101", 3, "Purchase Request Entry", 3, "../tej-base/om_pur_req.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                break;
            case "BEST": //19/09/2019 BY BANSAL SIR
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184", 3, "FG Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "VCL": //19/09/2019 BY BANSAL SIR
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "CRP":
                ICO.add_icon(frm_qstr, "F35101", 4, "Job Order Creation", 3, "../tej-base/om_JCard_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                break;
            case "HGLO":
                opts_wfin.Icon_Hrm(frm_qstr, frm_cocd);
                opts_wfin.Icon_Payr(frm_qstr, frm_cocd);
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85101'");// Attendance Entry // WRITTEN BY MADHVI ON 07 OCT 2019
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85106'");// Salary Preparation // WRITTEN BY MADHVI ON 07 OCT 2019
                break;
            case "SDM":
            case "DLJM":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                opts_wfin.Icon_DrCr_Maruti(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                //F25374
                break;
            case "MCPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185A", 3, "Duplex Costing", 3, "../tej-base/cost_infi_t.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185B", 3, "Duplex Costing 2", 3, "../tej-base/duplx_cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F40329D", 4, "Reel Summary Report GSM, Size Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40329E", 4, "Reel Summary Report GSM, Size, BF Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                break;
            case "STUD":
            case "MLGI*":
                // Bill upload against MRR
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);

                // added on 23/09/2019 
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15131", 2, "Purchase Reports", 3, "-", "-", "Y", "fin15_e3", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15160", 2, "Purch. Check/Approvals", 3, "-", "-", "Y", "fin15_e4", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15165", 3, "Purchase Order Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15166", 3, "Purchase Order Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47140", 3, "Dom.Order Reports", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47121", 3, "Dom.Sales Approvals", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47126", 4, "Check S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F47127", 4, "Approve S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25122", 2, "Approvals", 3, "-", "-", "Y", "fin22_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25122C", 3, "Chllan Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin22_e1", "fin25_a1", "fin22_e1app", "fa-edit");
                ICO.add_icon(frm_qstr, "F25122M", 3, "MRR Approvals", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin22_e1", "fin25_a1", "fin22_e1app", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");

                ICO.add_icon(frm_qstr, "F25144C", 3, "Challan DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25144M", 3, "MRR DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15135P", 3, "P.O. DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F47142S", 4, "S.O. DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50143I", 4, "Invoice DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70146A", 3, "Voucher DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

                break;
            case "TEST":
                ICO.add_icon(frm_qstr, "F45135", 4, "Seminar Registration List", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10556", 3, "Expense Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10283", 4, "Not Checked - In Date", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10284", 4, "Late Coming Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185A", 3, "Duplex Costing", 3, "../tej-base/cost_infi_t.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185B", 3, "Duplex Costing 2", 3, "../tej-base/duplx_cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10188", 3, "Label Costing Sheet", 3, "../tej-base/om_label_costing.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F99123", 3, "Hierarchy Mails Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F60131A", 3, "CSS Costing Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "N");
                break;
            case "MUKP":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                break;
            case "SYDB":
            case "SYDP":
            case "SYDE":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "MLAB":// 12/10/2019 
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10191", 3, "Around Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10192", 3, "Cylinder Costing", 3, "../tej-base/om_Cylind_Cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193", 3, "Paper Rate Master", 3, "../tej-base/om_Matl_Master.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10193V", 3, "Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193Q", 3, "Quality/Foil Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                //ICO.add_icon(frm_qstr, "F10195", 3, "Trim Wastage", 3, "../tej-base/om_trim_wstg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10196", 3, "Label Costing", 3, "../tej-base/om_lbl_cost_MLAB.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CYLINDER'", "TNAME");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CYLINDER (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,ACODE CHAR(10),ICODE CHAR(10),SRNO  NUMBER(4),COL1  VARCHAR2(20),COL2  VARCHAR2(20),COL3  VARCHAR2(20),COL4  VARCHAR2(20),COL5 VARCHAR2(20),COL6  VARCHAR2(20),COL7  VARCHAR2(20),COL8  VARCHAR2(20),COL9  VARCHAR2(20),COL10 VARCHAR2(20),COL11 VARCHAR2(20),COL12 VARCHAR2(20),COL13 VARCHAR2(20),COL14 VARCHAR2(20),COL15 VARCHAR2(20),REMARKS VARCHAR2(300),NUM1 NUMBER(20,3),NUM2 NUMBER(20,3),NUM3 NUMBER(20,3),NUM4 NUMBER(20,3),NUM5 NUMBER(20,3),NUM6 NUMBER(20,3),NUM7 NUMBER(20,3),NUM8 NUMBER(20,3),NUM9 NUMBER(20,3),NUM10 NUMBER(20,3),NUM11  NUMBER(20,3),NUM12 NUMBER(20,3),NUM13 NUMBER(20,3),NUM14 NUMBER(20,3),NUM15 NUMBER(20,3),NUM16 NUMBER(20,3),NUM17 NUMBER(20,3),NUM18 NUMBER(20,3),NUM19 NUMBER(20,3),NUM20 NUMBER(20,3),NUM21 NUMBER(20,3),NUM22 NUMBER(20,3),NUM23 NUMBER(20,3),NUM24 NUMBER(20,3),NUM25 NUMBER(20,3),NUM26 NUMBER(20,3),NUM27 NUMBER(20,3),NUM28 NUMBER(20,3),NUM29 NUMBER(20,3),NUM30 NUMBER(20,3),NUM31 NUMBER(20,3),NUM32 NUMBER(20,3),NUM33 NUMBER(20,3),NUM34 NUMBER(20,3),NUM35 NUMBER(20,3),NUM36 NUMBER(20,3),NUM37 NUMBER(20,3),EDT_BY VARCHAR2(20),EDT_DT DATE,NARATION VARCHAR2(150),ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL)");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185A", 3, "Duplex Costing", 3, "../tej-base/cost_infi_t.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185B", 3, "Duplex Costing 2", 3, "../tej-base/duplx_cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10188", 3, "Label Costing Sheet", 3, "../tej-base/om_label_costing.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                {
                    ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40171", 3, "Label Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                }

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);


                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");


                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                break;
            case "KPPL":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10130", 2, "Production Masters", 3, "-", "-", "Y", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                //16 01 2020 -- BANSAL SIR

                opts_wfin.Icon_Mgmt(frm_qstr, frm_cocd);

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                opts_wfin.Icon_Purch(frm_qstr, frm_cocd);

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);

                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord_Exp(frm_qstr, frm_cocd);

                opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_Sale_Exp(frm_qstr, frm_cocd);

                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                break;
            case "BUPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15131", 2, "Purchase Reports", 3, "-", "-", "Y", "fin15_e3", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15134", 3, "Purchase Schedule Report", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15134A", 3, "Purchase Schedule Vs Email Sent Report", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F10194E", 3, "Valuation on BOM Costing(Expendable)", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194F", 3, "Valuation on BOM Costing(Expendable) fifo", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10198", 3, "RM,FG Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10198W", 3, "WIP Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125C", 3, "Sales vs RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "GTCF":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "PKGW*":
                ICO.add_icon(frm_qstr, "F10134", 3, "Laminate BOM", 3, "../tej-base/om_bom_lami.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F40107", 4, "Label Prodn", 3, "../tej-base/om_corr_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
                break;
            case "PERF":
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25198A", 3, "MRR Reel Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35100", 3, "Prt/Pkg PPC Activity", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35101", 4, "Job Order Creation", 3, "../tej-base/om_JCard_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35106", 4, "Job Order Planning", 3, "../tej-base/om_JPlan_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                break;
            case "JEPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                // 02 01 2020 - Bansal Sir
                opts_wfin.IconMouldMaint(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30110", 2, "Quality Activity", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30114", 3, "In-Proc Quality", 3, "../tej-base/om_qa_lqc.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");

                // gate entry added on 20/02/2020 - on req of rahul ji 
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                break;
            case "PRUB":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40301", 3, "Reports(Detailed)", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40328", 4, "FG Stock Location Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40328R", 4, "RM Stock Location Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                break;
            case "KRML":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "KPIL":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "VPAC":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F40329D", 4, "Reel Summary Report GSM, Size Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40329E", 4, "Reel Summary Report GSM, Size, BF Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

                // 18/04/2020 --
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                opts_wfin.Icon_Cust_port(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Supp_port(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F47120", 4, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);


                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47120", 4, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F47125", 4, "Supervisior Master", 3, "../tej-base/personmst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                break;
            case "SAIL":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50143I", 4, "Invoice DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                break;
            case "PACT":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35100", 3, "Prt/Pkg PPC Activity", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35101", 4, "Job Order Creation", 3, "../tej-base/om_JCard_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                break;
            case "ALIN":

                break;
            case "LNG":
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                break;
            case "SUPR":
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);

                opts_wfin.Icon_Visitor(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                opts_wfin.Icon_Truck_Monitoring(frm_qstr, frm_cocd);

                //ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                //ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                //ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                //ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");                
                break;
            case "ACCR":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                break;
            case "AHPI"://11 03 2020
                opts_wfin.Icon_Prodn_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10184C", 3, "FG Valuation on Process Plan", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "YTEC":
                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F25144C", 3, "Challan DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50143I", 4, "Invoice DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50051", 2, "Invoice Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin50_e1X", "fin50_a1", "-", "fa-edit");
                break;
            case "PPPL":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                opts_wfin.Icon_Prodn_plast(frm_qstr, frm_cocd);

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);
                opts_wfin.Icon_Purch(frm_qstr, frm_cocd);
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.IconRFQ_SO(frm_qstr, frm_cocd);
                opts_wfin.Icon_Acctg(frm_qstr, frm_cocd);
                opts_wfin.Icon_Qlty(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35131", 3, "Prodn PPC Activity", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35136", 4, "Daily Prodn Plan", 3, "../tej-base/om_sday_plan.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F35107", 4, "Machine Planning", 3, "../tej-base/om_mcplan.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F40999", 4, "MRP MIT", 3, "../tej-base/om_dbd_bpln2.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit", "N", "N");

                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);

                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                break;
            case "KESR":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "HARI":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "SACL":
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");

                ICO.add_icon(frm_qstr, "F35228", 4, "Android Production Report", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F35228A", 4, "Android Production QC Report", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F35228B", 4, "Plant Stock Report (SACL Format)", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F35228B1", 4, "Plant Stock Report with Value (SACL Format)", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F35228C", 4, "Plant Pending Order (SACL Format)", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F35228D", 4, "Plant Pending Schedule (SACL Format)", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "N");
                // 21/07/2020 - Bansal sir
                opts_wfin.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                break;
            case "SSPL":
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                break;
            // 17/06/2020
            case "PPPF":
            case "PPRM":
            case "PIPL":
            case "PPPH":
            case "PPPT":
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);
                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);

                opts_wfin.iconDrawingModule(frm_qstr, frm_cocd);

                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_ord(frm_qstr, frm_cocd);
                opts_wfin.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                break;
            case "NAPL":
            case "EZEN":
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                break;
            case "RWPL":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F40329D", 4, "Reel Summary Report GSM, Size Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40329E", 4, "Reel Summary Report GSM, Size, BF Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                break;
            case "HTPC":
                // 06/07/2020
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                break;
            case "REVA":
            case "RIL":
                ICO.add_icon(frm_qstr, "F60000", 1, "Customer Support System", 3, "-", "-", "Y", "-", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F93000", 2, "Finsys OMS", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F93100", 3, "OMS Activity", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F93101", 4, "OMS Plan", 3, "../tej-base/om_oms_Plan.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F93106", 4, "OMS Followup", 3, "../tej-base/om_oms_folo.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F93116", 3, "OMS Reports", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F93121", 4, "OMS Person Wise ", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93126", 4, "OMS Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93131", 4, "OMS Tgt VS Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93132", 4, "OMS Team Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F93133", 4, "OMS Client Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                break;
            case "NPI":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50131", 3, "Dom.Sales Checklists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F50137A", 4, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F50137B", 4, "Truck Entry Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50274", 4, "Production Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F47120", 4, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F47125", 4, "Supervisior Master", 3, "../tej-base/personmst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F40329D", 4, "Reel Summary Report GSM, Size Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40329E", 4, "Reel Summary Report GSM, Size, BF Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                opts_wfin.IconRFQ_SO(frm_qstr, frm_cocd);
                break;
            case "SEAS":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "SWAS": //18/08/2020
                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                // Bill upload against MRR
                opts_wfin.iconInvMrrUpload(frm_qstr, frm_cocd);

                opts_wfin.iconOMSEntry(frm_qstr, frm_cocd);

                opts_wfin.Icon_Crm(frm_qstr, frm_cocd);
                break;
            case "ARUB": //21-08-2020
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                break;
            case "KUNS": // 24/08/2020
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                break;
            case "MEGH":
                // 25/08/2020
                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                break;
            case "ESML":
                // 25/08/2020
                opts_wfin.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                opts_wfin.Icon_Store(frm_qstr, frm_cocd);
                opts_wfin.Icon_gate(frm_qstr, frm_cocd);
                break;
            case "SGRP":
            case "UATS":
            case "UAT2":
                // 02/09/2020
                opts_wfin.IconRFQ_PO(frm_qstr, frm_cocd);

                // for CUSTOMER VENDOR PORTAL    
                opts_wfin.Icon_Supp_port(frm_qstr, frm_cocd);
                opts_wfin.Icon_Cust_port(frm_qstr, frm_cocd);

                opts_wfin.IconRFQ_SO(frm_qstr, frm_cocd);

                opts_wfin.Icon_Leave_Req(frm_qstr, frm_cocd);

                opts_wfin.Icon_TaskmgtWP(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_ee", "fin10_a12", "fin10_MREP1", "fa-edit");
                ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_ee", "fin10_a1", "fin10_MREP1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_ee", "fin10_a1", "fin10_MREP1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10556", 3, "Expense Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");

                //08 10 2020
                opts_wfin.iconDrawingModule(frm_qstr, frm_cocd);
                break;
            //19 09 2020
            case "V2I":
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                // added on 17/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                break;
            //19 09 2020
            case "SREE":
                opts_wfin.ProfitabilityReport(frm_qstr, frm_cocd);
                // added on 17/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                break;
            case "AERO":
                opts_wfin.Icon_Ppc_paper(frm_qstr, frm_cocd);
                break;
            case "NATP": // 08 10 2020            
                // added on 17/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                // made by suman
                ICO.add_icon(frm_qstr, "F35229", 4, "Paper Variation Report Code", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                opts_wfin.icon_ppc_prodReports(frm_qstr, frm_cocd);

                opts_wfin.Icon_Engg(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25124", 3, "Stacking Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15125", 3, "Kanban Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20125", 3, "Invoice Gate Outward Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                //----------------------------------                                              

                opts_wfin.Icon_FA_sys(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                //*****
                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");
                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                    
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10550'");                    
                    ICO.add_icon(frm_qstr, "F10550", 3, "Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10551'");
                    ICO.add_icon(frm_qstr, "F10551", 3, "Type of Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10552'");
                    ICO.add_icon(frm_qstr, "F10552", 3, "Department Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10553'");
                    ICO.add_icon(frm_qstr, "F10553", 3, "Person Master", 3, "../tej-base/personmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10554'");
                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM FIN_MSYS WHERE ID='F10555'");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    //ICO.add_icon(frm_qstr, "F10054", 3, "Request Status Print", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_ee", "fin10_a12", "fin10_MREP1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_ee", "fin10_a1", "fin10_MREP1", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_ee", "fin10_a1", "fin10_MREP1", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10556", 3, "Expense Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_ee", "fin10_a1", "-", "fa-edit");
                }
                break;
        }

        {
            // 21/01/2020 -- skg sir ( icons for all , payment advice)
            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

            // for all  13 / 05 / 2020
            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
            ICO.add_icon(frm_qstr, "F70336", 3, "Balance Confirmation Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
            ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05109", 3, "Delivery Status Report", 3, "../tej-base/om_Delivry_Status.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

            // made by suman
            ICO.add_icon(frm_qstr, "F35229", 4, "Paper Variation Report Code", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F99165", 3, "ERP Data Uploading", 3, "../tej-base/om_upload_dashboard.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F35104", 4, "Job Order Creation(Poly)", 3, "../tej-base/om_JCard_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
        }

        //ICO.add_icon(frm_qstr, "F39102", 4, "Prodn Entry", 3, "../tej-base/om_corr_entry.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");

        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "19/01/2019", "DEV_A", "W0053", "Purchase Order No running for all types? ", "Y", "-");// vipin
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0056", "No. of Invoice Copy? ", "N", "4"); // vipin
        if (frm_cocd == "STUD")
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0057", "Full Name in DSC Printout? ", "Y", "-"); // vipin
        else
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0057", "Full Name in DSC Printout? ", "N", "-"); // vipin


        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_FILE_ATCH'", "TNAME");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_FILE_ATCH (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR(6),VCHDATE DATE DEFAULT SYSDATE,SRNO NUMBER(5),FILE_NAME VARCHAR(50),FILE_PATH VARCHAR(50),FILE_ORIG_NAME VARCHAR(80),REMARKS VARCHAR(100),ENT_BY VARCHAR(20),ENT_DT DATE DEFAULT SYSDATE,EDT_BY VARCHAR(20),EDT_DT DATE DEFAULT SYSDATE )");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_FILE_ATCH", "FILE_ORIG_NAME");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_FILE_ATCH ADD FILE_ORIG_NAME VARCHAR(80) DEFAULT '-' ");

        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_FILE_ATCH MODIFY FILE_NAME VARCHAR(50)");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "TR_NAME");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD TR_NAME VARCHAR2(60)");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "NUM_FMT1");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD NUM_FMT1 VARCHAR2(20)");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "NUM_FMT2");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD NUM_FMT2 VARCHAR2(20)");

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_VACREQ'", "TNAME");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_VACREQ (BRANCHCD CHAR(2),TYPE CHAR(2),VACNO CHAR(6),VACDT DATE,EMPCODE CHAR(10),VREASON1 VARCHAR2(30), VREASON2 VARCHAR2(30),VACFROM  VARCHAR2(10),VACUPTO  VARCHAR2(10),CONT_NAME VARCHAR2(50),CONT_NO  VARCHAR2(20),CONT_EMAIL VARCHAR2(30),VREMARKS VARCHAR2(150),OREMARKS CHAR(150),RESP_SHARED CHAR(1),SRNO NUMBER(4),ORIGNALBR CHAR(2),FILEPATH VARCHAR2(100),FILENAME VARCHAR2(60),LAST_ACTION VARCHAR2(80),LAST_ACTDT VARCHAR2(10),VAC_TIME  CHAR(10),RET_TIME CHAR(10),TOT_DAYS NUMBER(6,2),TIME_IN_HRS CHAR(10),ENT_BY  VARCHAR2(20),ENT_DT  DATE,EDT_BY  VARCHAR2(20),EDT_DT  DATE,APP_BY VARCHAR2(20),APP_DT  DATE)");

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_LEVREQ'", "TNAME");
        if (mhd != "0")
        {
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "LVSECTION");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (LVSECTION VARCHAR2(30))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "DESFROM");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (DESFROM VARCHAR2(30))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "DESTO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (DESTO VARCHAR2(30))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "LVSERVYRNO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (LVSERVYRNO CHAR(10))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "TICKETNO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (TICKETNO VARCHAR2(50))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "AIRLINENAME");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (AIRLINENAME VARCHAR2(50))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "LVADDRESS");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (LVADDRESS VARCHAR2(100))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "EXITREENTRYEMP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (EXITREENTRYEMP CHAR(2))");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEVREQ", "EXITREENTRYFAM");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEVREQ ADD (EXITREENTRYFAM CHAR(2))");
        }

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_LEAD_LOG'", "TNAME");
        if (mhd != "0")
        {
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LEAD_SOURCE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD (LEAD_SOURCE VARCHAR(50), LEAD_PRIORIY VARCHAR(10),LEAD_CITY VARCHAR(40))");
        }

        //07/10/2020
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/10/2020", "DEV_A", "W2029", "Production from 1. Sales Plan, 2. Direct from Sales Order ", "Y", "1");
        //08/10/2020
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "08/10/2020", "DEV_A", "W2030", "Drawing / Artwork module : Pick Party, Item from Comman Master, N for ERP Master", "Y", "1");
        //08/10/2020
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "08/10/2020", "DEV_A", "W2031", "Drawing / Artwork module : Artwork against Lead No.", "N", "2");
    }

    void mgIcons(string frm_qstr, string frm_cocd)
    {
        Opts_wfin WFIN_mgopts = new Opts_wfin();
        switch (frm_cocd)
        {

            case "DLJH":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10100", 2, "Items Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10175", 3, "Item Family Bulk Update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                break;
            case "SGRP":
            case "UATS":
            case "UAT2":
                WFIN_mgopts.Premium_custeval(frm_qstr, frm_cocd);
                WFIN_mgopts.Premium_vehi_maint(frm_qstr, frm_cocd);
                WFIN_mgopts.Premium_legal_soft(frm_qstr, frm_cocd);
                WFIN_mgopts.Premium_kpi_mgmt(frm_qstr, frm_cocd);
                WFIN_mgopts.Premium_salebudget(frm_qstr, frm_cocd);
                WFIN_mgopts.IconRFQ_PO(frm_qstr, frm_cocd);
                WFIN_mgopts.Premium_sman_visit(frm_qstr, frm_cocd);
                break;
            case "MIRP":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10178", 2, "Costing Module : Labels", 3, "-", "-", "Y", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10199", 3, "Offset Label Costing", 3, "../tej-base/om_lbl_cost_SPPI.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193V", 3, "Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10200", 3, "Plate Unit Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10201", 3, "Ink Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10202", 3, "Die Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10203", 3, "Embossing Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10204", 3, "Embossing White/Screen Printing Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10205", 3, "Web Machine Master", 3, "../tej-base/Web_mach_mast.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10206", 3, "Foil Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10207", 3, "Lamination Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_MACH_COST'", "TNAME");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_MACH_COST(branchcd char(2) default '-',type char(2) default '-',vchnum char(6) default '-',vchdate date  default sysdate,MCHNAME varchar2(50),MCHCODE varchar2(30),MCH_COST number(12,2) default 0,MCH_cOST1 NUMBER(12,2) default 0,YR_CONSDER number(8,2) default 0,WRK_HR_PDAY number(8,2) default 0,DAY_WRK_PM number(8,2) default 0,TOT_MTH_YR number(8,2) default 0,TOT_HR number(8,2) default 0,MCH_RT_PHR number(15,8) default 0,OPER_SAL number (15,8) default 0,oper_sal_ph number(15,8) default 0,NO_IMP_PMNT number(8,2) default 0,MX_RMTR_PHR number(8,2) default 0,MX_RMTR_PHR1 number(8,2) default 0,JOB_TIME number(15,8) default 0,SET_TIME number(8,2) default 0,TOT_TIME_JOB number(15,8) default 0,tot_ele_use number(15,8) default 0,elce_chg_phr number(15,8) default 0,tot_mcost number(15,8) default 0,ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(20) NOT NULL,EDT_DT DATE NOT NULL)");
                break;
            case "STLC":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10178", 2, "Costing Module : Labels", 3, "-", "-", "Y", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10199", 3, "Offset Label Costing", 3, "../tej-base/om_lbl_cost_SPPI.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10193V", 3, "Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10200", 3, "Plate Unit Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10201", 3, "Ink Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10202", 3, "Die Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10203", 3, "Embossing Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10204", 3, "Embossing White/Screen Printing Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10205", 3, "Web Machine Master", 3, "../tej-base/Web_mach_mast.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10206", 3, "Foil Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10207", 3, "Lamination Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e9", "fin10_a1", "-", "fa-edit");
                break;
            case "VELV":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50310", 3, "Marketing Reports(Sales Module)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50330", 4, "Generate Invoice- Tungston", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
                break;
            case "OMP":
                WFIN_mgopts.Icon_Visitor(frm_qstr, frm_cocd);
                break;
            case "OMNI":
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50154", 4, "Pending Order Register Weight Wise", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                break;
            case "HPPI":
            case "PKGW":
            case "SPPI":
                WFIN_mgopts.Icon_FA_sys(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Payr(frm_qstr, frm_cocd);
                ///engg-web
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10199", 3, "Offset Label Costing", 3, "../tej-base/om_lbl_cost_SPPI.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10193V", 3, "Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10200", 3, "Plate Unit Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10201", 3, "Ink Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10202", 3, "Die Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10203", 3, "Embossing Varnish Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10204", 3, "Embossing White/Screen Printing Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10205", 3, "Web Machine Master", 3, "../tej-base/Web_mach_mast.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10206", 3, "Foil Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10207", 3, "Lamination Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_MACH_COST'", "TNAME");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_MACH_COST(branchcd char(2) default '-',type char(2) default '-',vchnum char(6) default '-',vchdate date  default sysdate,MCHNAME varchar2(50),MCHCODE varchar2(30),MCH_COST number(12,2) default 0,MCH_cOST1 NUMBER(12,2) default 0,YR_CONSDER number(8,2) default 0,WRK_HR_PDAY number(8,2) default 0,DAY_WRK_PM number(8,2) default 0,TOT_MTH_YR number(8,2) default 0,TOT_HR number(8,2) default 0,MCH_RT_PHR number(15,8) default 0,OPER_SAL number (15,8) default 0,oper_sal_ph number(15,8) default 0,NO_IMP_PMNT number(8,2) default 0,MX_RMTR_PHR number(8,2) default 0,MX_RMTR_PHR1 number(8,2) default 0,JOB_TIME number(15,8) default 0,SET_TIME number(8,2) default 0,TOT_TIME_JOB number(15,8) default 0,tot_ele_use number(15,8) default 0,elce_chg_phr number(15,8) default 0,tot_mcost number(15,8) default 0,ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(20) NOT NULL,EDT_DT DATE NOT NULL)");
                ICO.add_icon(frm_qstr, "F10186C", 3, "Detailed Flexible Costing", 3, "../tej-base/om_pre_cost_SPPI.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CYLINDER'", "TNAME");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CYLINDER (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,ACODE CHAR(10),ICODE CHAR(10),SRNO  NUMBER(4),COL1  VARCHAR2(20),COL2  VARCHAR2(20),COL3  VARCHAR2(20),COL4  VARCHAR2(20),COL5 VARCHAR2(20),COL6  VARCHAR2(20),COL7  VARCHAR2(20),COL8  VARCHAR2(20),COL9  VARCHAR2(20),COL10 VARCHAR2(20),COL11 VARCHAR2(20),COL12 VARCHAR2(20),COL13 VARCHAR2(20),COL14 VARCHAR2(20),COL15 VARCHAR2(20),REMARKS VARCHAR2(300),NUM1 NUMBER(20,3),NUM2 NUMBER(20,3),NUM3 NUMBER(20,3),NUM4 NUMBER(20,3),NUM5 NUMBER(20,3),NUM6 NUMBER(20,3),NUM7 NUMBER(20,3),NUM8 NUMBER(20,3),NUM9 NUMBER(20,3),NUM10 NUMBER(20,3),NUM11  NUMBER(20,3),NUM12 NUMBER(20,3),NUM13 NUMBER(20,3),NUM14 NUMBER(20,3),NUM15 NUMBER(20,3),NUM16 NUMBER(20,3),NUM17 NUMBER(20,3),NUM18 NUMBER(20,3),NUM19 NUMBER(20,3),NUM20 NUMBER(20,3),NUM21 NUMBER(20,3),NUM22 NUMBER(20,3),NUM23 NUMBER(20,3),NUM24 NUMBER(20,3),NUM25 NUMBER(20,3),NUM26 NUMBER(20,3),NUM27 NUMBER(20,3),NUM28 NUMBER(20,3),NUM29 NUMBER(20,3),NUM30 NUMBER(20,3),NUM31 NUMBER(20,3),NUM32 NUMBER(20,3),NUM33 NUMBER(20,3),NUM34 NUMBER(20,3),NUM35 NUMBER(20,3),NUM36 NUMBER(20,3),NUM37 NUMBER(20,3),EDT_BY VARCHAR2(20),EDT_DT DATE,NARATION VARCHAR2(150),ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL)");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185A", 3, "Duplex Costing", 3, "../tej-base/cost_infi_t.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185B", 3, "Duplex Costing 2", 3, "../tej-base/duplx_cost.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185C", 3, "Flexible Costing", 3, "../tej-base/cost_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10187", 3, "Material Master(Label Costing)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10188", 3, "Label Costing Sheet", 3, "../tej-base/om_label_costing.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                {
                    ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40171", 3, "Label Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                }
                break;
            case "AZUR":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/om_vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/om_vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/om_vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "SFLG":
                ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99108", 3, "DSC Activation", 3, "../tej-base/om_dsc_activate.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85141", 2, "Salary Reports", 3, "-", "-", "Y", "fin85_e4", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85146", 3, "More Reports(Pay)", 3, "-", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F85234", 4, "Welfare Fund Upload", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
                break;
            case "SFL2":
            case "SFL1":
                ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F99108", 3, "DSC Activation", 3, "../tej-base/om_dsc_activate.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
                break;
            case "SAIA"://05/11/2019
                ICO.add_icon(frm_qstr, "F50322", 4, "Ord Vs Sales Summ Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50323", 4, "Party wise,Mth wise Grs Sales Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50324", 4, "Party wise,Item gp wise Ord vs Sale Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50327", 4, "MTD Product wise Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                break;
            //case "MLAB":// 18/10/2019 
            //    
            //    break;
            case "MLGA":
                WFIN_mgopts.Icon_Payr(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Hrm(frm_qstr, frm_cocd);
                break;
            case "TCSR":
                WFIN_mgopts.Icon_DrCr_self(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                break;
            case "OPPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;
            case "SEL":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25262", 4, "Material Issue Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                break;
            case "MIPL":
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "DISP":
                ICO.add_icon(frm_qstr, "F39102", 4, "Moulding Entry", 3, "../tej-base/frmMProd_Disp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F39121", 3, "Moulding Prodn Checklists", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit");
                break;
            case "CLPL":
                WFIN_mgopts.PremiumProductionReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F40060", 4, "Label-Rejection Deatiled", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "fin40_pfrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40061", 4, "Label-Rejection Summary", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "fin40_pfrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40062", 4, "Machine Efficiency", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "fin40_pfrep", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40063", 4, "Machine Wise,Shift Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "fin40_pfrep", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");

                break;
            case "KRML":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55000", 2, "Export Sales Module", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55140", 3, "Exp.Sales Reports", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F55162", 4, "Commercial Invoice(Exp)- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55163", 4, "Packing List(Exp)- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                break;
            case "GIPL":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35227", 4, "Job Track Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "HGLO":
                ICO.add_icon(frm_qstr, "F85107", 3, "Salary Preparation-HR", 3, "../tej-base/om_pay_h.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85102", 3, "Attendance Entry-HR", 3, "../tej-base/om_attn_entryh.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85152", 4, "Salary Register(HGLO)", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "MNAME");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD MNAME VARCHAR2(50) default '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "EMERGENCY");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD EMERGENCY VARCHAR2(10) default '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "LIST");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD LIST VARCHAR2(20) default '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "PLANT");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD PLANT VARCHAR2(50) default '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "BRANCH");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD BRANCH VARCHAR2(50) default '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "FIXED_AMT");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD FIXED_AMT NUMBER(15,5) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "HBONUS");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD HBONUS VARCHAR2(10) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "HDAYS");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD HDAYS VARCHAR2(10) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "OTAFTER");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD OTAFTER NUMBER(10,2) DEFAULT 0");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "dt3");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD dt3 number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "dt4");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD dt4 number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "dt5");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD dt5 number(5,2) DEFAULT 0");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "Tot_OT");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD Tot_OT number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "Sunday_pay");
                if (mhd == "0")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD Sunday_pay number(5,2) DEFAULT 0");

                string SQuery = "";
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WBPAYH'", "TNAME");
                if (mhd == "0" || mhd == "")
                {
                    SQuery = "create table wbpayh (branchcd char(2) default '-',type char(2) default '-',vchnum varchar2(6) default '-',vchdate date default sysdate,grade char(2) default '-',empcode varchar2(6) default '-',totdays number(10,2) default 0,srno number(5) default 0 ,sunday number(10,2) default 0,pr_hrs number(10,2) default 0,ot_hrs number(10,2) default 0,fooding_hrs  number(10,2) default 0,tot_2d_hrs  number(10,2) default 0,tot_late_hrs  number(10,2) default 0,tot_fine_hrs  number(10,2) default 0,tot_sleep_hrs  number(10,2) default 0,tot_other_ded_hrs  number(10,2) default 0,pay_hrs number(10,2) default 0,pay_sal number(10,2) default 0,ot number(10,2) default 0,days_ number(10,2) default 0,attn number(10,2) default 0,fooding number(10,2) default 0,prev_mth_add number(10,2) default 0,spl_add number(10,2) default 0,tot_add number(10,2) default 0,ded_2d number(10,2) default 0,late number(10,2) default 0,fine number(10,2) default 0,sleep number(10,2) default 0,oth_ded number(10,2) default 0,advance number(10,2) default 0,prev_mth_sub number(10,2) default 0,spl_sub number(10,2) default 0,tot_ded number(10,2) default 0,gross number(10,2) default 0,payno varchar2(16) default '-',actual_rate number(15,5) default 0,wrkhrs number(7,2) default 0 ,ent_by varchar2(15),ent_dt date,edt_by varchar2(15),edt_dt date)";
                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                }
                break;
            case "ELEC":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50100", 3, "Dom.Sales Activity", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F50101", 4, "Sales Invoice (Dom.)", 3, "../tej-base/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F50143I", 4, "Invoice DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                break;
            case "PCON":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70181", 3, "Multi A/c Master", 3, "../tej-base/om_multi_account.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                ///===============
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40131", 3, "Prt/Pkg Prodn Reports", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40216", 4, "Corrugation DPR", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40217", 4, "Rejection Report DayWise(Corrugation)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40218", 4, "Rejection Report Reason Wise (Corrugation)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40219", 4, "DownTime Report DayWise(Corrugation)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40220", 4, "DownTime Report Reason Wise (Corrugation)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");

                break;
            case "ACCR":
                // added on 24/04/198
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40131", 3, "Prt/Pkg Prodn Reports", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40215", 4, "Production Transfer Sticker", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");
                break;
            case "SOTL":
                // added on 18/04/198
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Payr(frm_qstr, frm_cocd);
                break;
            case "WPPL":
                WFIN_mgopts.Icon_Leave_Req(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Loan_Req(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F50321", 4, "Pending SO-BSR Qty", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit", "N", "N");
                break;
            case "BONY":
                // HSN WISE SUMMARY REPORT
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70375", 4, "HSN wise Purchase Non MRR Data", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                break;
            case "AMAR":
                Opts_wfin WFIN_optsAMAR = new Opts_wfin();
                WFIN_optsAMAR.IconCastingProd(frm_qstr, frm_cocd);
                //Welfare Contribution Report
                ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85141", 2, "Salary Reports", 3, "-", "-", "Y", "fin85_e4", "fin85_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F85146", 3, "More Reports(Pay)", 3, "-", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F85151", 4, "Welfare Contribution Report", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "Y");

                // NEW RFQ Module with casting cost sheet                
                WFIN_optsAMAR.IconRFQ_SO(frm_qstr, frm_cocd);
                break;

            case "YPPL":
                // added on 17/11/18 - on req of Bansal Sir
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40353", 4, "Production Report (As Per Specification)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin352pp_mrep", "fa-edit", "N", "Y");
                break;

            case "SURY":
                Opts_wfin SURY_icons = new Opts_wfin();
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                SURY_icons.IconBoxCostSURY(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Activity", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70348", 3, "Cheque Deposit Slip", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                break;

            case "MINV":
                Opts_wfin MINV_icons = new Opts_wfin();
                MINV_icons.Icon_gate(frm_qstr, frm_cocd);
                MINV_icons.Icon_Purch(frm_qstr, frm_cocd);
                MINV_icons.Icon_Qlty(frm_qstr, frm_cocd);
                MINV_icons.Icon_Store(frm_qstr, frm_cocd);
                MINV_icons.Icon_Mkt_ord(frm_qstr, frm_cocd);
                MINV_icons.Icon_Mkt_ord_Exp(frm_qstr, frm_cocd);
                MINV_icons.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                MINV_icons.Icon_Mkt_Sale_Exp(frm_qstr, frm_cocd);
                MINV_icons.PremiumSalesReport(frm_qstr, frm_cocd);
                MINV_icons.Upd_SYSOPT(frm_qstr, frm_cocd);
                MINV_icons.Icon_Visitor(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30110", 2, "Quality Activity", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30150", 3, "Sample Request Form", 3, "../tej-base/om_samp_req.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
                break;

            case "GRIP":
                //spl_cust for grip
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15131", 2, "Purchase Reports", 3, "-", "-", "Y", "fin15_e3", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15221", 3, "More Reports(Purch.)", 3, "-", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15246", 4, "Job No wise PRsPO>MRR", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F15144", 4, "PR/PO/MRR Work order no wise Report", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
                break;

            case "SAGE":
            case "SINC":
            case "SAGM":
            case "SAGI":
                ICO.add_icon(frm_qstr, "F70480", 4, "Multi Excel Upload", 3, "../tej-base/om_any_upload.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

                Opts_wfin SAGE_icons = new Opts_wfin();
                SAGE_icons.PremiumEmktgReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F49202", 4, "SO analysis-Customer Qty wise", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49203", 4, "SO analysis-Customer Value wise", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49204", 4, "SO analysis-Customer Qty wise", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49205", 4, "SO analysis-Customer Value wise", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49206", 4, "SO Acceptance", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49207", 4, "SO Acceptance-Detailed", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F49208", 4, "SO Shipment Plan", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49209", 4, "SO Monthwise Summary", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49210", 4, "Estimated Delivery Schedule", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49211", 4, "Goods Status ", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55000", 2, "Export Sales Module", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55145", 4, "Export Invoice- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55146", 4, "Packing List- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55111", 4, "Dispatch Advice (Exp.)", 3, "../tej-base/om_Da_entry.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F49140", 3, "Exp.Order Reports", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F49149", 4, "Invoice wise RM Metallurgy", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F55500", 2, "Export Licence Management", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55502", 3, "Export Licence Master", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit");

                ICO.add_icon(frm_qstr, "F55503", 3, "Advance Licence", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55504", 3, "EPCG Licence", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit");
                ICO.add_icon(frm_qstr, "F55505", 3, "Shipping Master", 3, "-", "-", "Y", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit");


                ICO.add_icon(frm_qstr, "F55511", 4, "Advance Licence Master ", 3, "../tej-base/om_Advlic_mast.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55512", 4, "Advance Licence Adj-Import ", 3, "../tej-base/om_Implic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55513", 4, "Advance Licence Adj-Export ", 3, "../tej-base/om_Explic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit");
                ICO.add_icon(frm_qstr, "F55514", 4, "Advance Licence Report-Import ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55515", 4, "Advance Licence Report-Export ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55516", 4, "Advance Licence Report-Summary ", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2tr", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F55517", 4, "EPCG License Master ", 3, "../tej-base/om_EPCG_Advlic_mast.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2mr", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55518", 4, "EPCG Import Adj ", 3, "../tej-base/om_EPCG_Implic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55519", 4, "EPCG Export Adj ", 3, "../tej-base/om_EPCG_Explic.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2epm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55520", 3, "Annexure Custom Filing ", 3, "../tej-base/om_Anex_Cust_Fil.aspx", "-", "-", "fin55_e2", "fin50_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55521", 4, "Container Master ", 3, "../tej-base/om_Contain_detail.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55522", 4, "Shipment Tracking Report ", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55523", 4, "Customer Wise Freight Report ", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F55524", 4, "Forwarding Agent Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55525", 4, "Shipping Line Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55526", 4, "Nature Of Shipment Master ", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F55527", 4, "Freight Chart", 3, "../tej-base/om_Freight_Chart.aspx", "-", "-", "fin55_e2", "fin50_a1", "fin55_e2sm", "fa-edit", "N", "N");
                if (frm_cocd == "SAGM")
                {
                    ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F85141", 2, "Salary Reports", 3, "-", "-", "Y", "fin85_e4", "fin85_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F85232", 4, "Salary Slip Mail", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
                }
                break;
            case "SDM":
            case "DLJM":
                WFIN_mgopts.IconMouldMaint(frm_qstr, frm_cocd);
                break;
            case "RIL":
                // added on 07/01/19 
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "MUKP":
                // added on 07/01/19 
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "KCOR":
                // added on 07/01/19 
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "PRAG":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25121", 2, "Inventory Checklists", 3, "-", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25145", 3, "More Checklists ( Inventory)", 3, "-", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25271", 4, "MRR-J/W challan Tie up Report", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "Y", "N");
                break;
            case "SVPL":
                ICO.add_icon(frm_qstr, "F25255", 3, "WIP Reconciliation Report", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10100", 2, "Items Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10129", 3, "Item Family Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10175", 3, "Item Family Bulk Update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39140", 3, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F39255", 4, "Reason Wise Rejections", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25256", 3, "WIP Reconciliation Report II", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F39500", 3, "Moulding Prodn Master", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit");
                ICO.add_icon(frm_qstr, "F39501", 4, "Zone Master- Production", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39502", 4, "Line Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39503", 4, "Shift Incharge Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39504", 4, "Supervisor Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39505", 4, "Loss Code Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39506", 4, "Loss Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp5_e5", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39100", 3, "Prodn Activity", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F39551", 4, "Production Entry", 3, "../tej-base/om_Prod_SVPL.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit", "N", "Y");

                break;
            case "ADVG":
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_INSPVCH'", "TNAME");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_INSPVCH (BRANCHCD  CHAR(2),TYPE CHAR(2),VCHNUM   CHAR(6),VCHDATE  DATE ,TITLE  VARCHAR2(100),BTCHNO CHAR(20),ACODE  CHAR(10),ICODE  CHAR(10),CPARTNO VARCHAR2(30),GRADE  VARCHAR2(20),SRNO   NUMBER(4),COL1   VARCHAR2(100),COL2   VARCHAR2(100),COL3   VARCHAR2(100),COL4   VARCHAR2(100),COL5   VARCHAR2(100),COL6   VARCHAR2(100),MRRNUM CHAR(6),MRRDATE CHAR(11),BTCHDT CHAR(11),RESULT VARCHAR2(40),OBSV1  VARCHAR2(30),OBSV2  VARCHAR2(30),OBSV3  VARCHAR2(50),OBSV4  VARCHAR2(30),OBSV5  VARCHAR2(30),OBSV6  VARCHAR2(30),OBSV7  VARCHAR2(30),OBSV8  VARCHAR2(50),OBSV9  VARCHAR2(70),OBSV10 VARCHAR2(30),OBSV11 VARCHAR2(30),OBSV12 VARCHAR2(30),OBSV13 VARCHAR2(30),OBSV14 VARCHAR2(30),OBSV15 VARCHAR2(30),CONTPLAN   VARCHAR2(15),SAMPQTY NUMBER(10),WONO   VARCHAR2(30),MATL   VARCHAR2(40),FINISH VARCHAR2(40),OMAX   VARCHAR2(30),OMIN   VARCHAR2(30),LINKFILE  VARCHAR2(200),MFGDATE   VARCHAR2(40),EXPDATE VARCHAR2(40),OBSV16 VARCHAR2(30),OBSV17 VARCHAR2(30),OBSV18 VARCHAR2(30),OBSV19 VARCHAR2(60),OBSV20 VARCHAR2(60),OBSV21 VARCHAR2(60),OBSV22 VARCHAR2(60),OBSV23 VARCHAR2(60),OBSV24 VARCHAR2(30),OBSV25 VARCHAR2(30),OBSV26 VARCHAR2(30),OBSV27 VARCHAR2(30),FIGURE_NO  VARCHAR2(30),CUSTREF VARCHAR2(60),OBSV28 VARCHAR2(30),OBSV29 VARCHAR2(30),APP_BY VARCHAR2(15),APP_DT DATE ,OBSV30 VARCHAR2(30),OBSV31 VARCHAR2(30),REJQTY NUMBER(10,2),DOC_DT DATE ,NUM1   NUMBER(12,3),NUM2   NUMBER(12,3),DTR1   VARCHAR2(20),DTT1   NUMBER(12,3),EQUIP_ID VARCHAR2(35),FOOTNOTE VARCHAR2(250),OBSV32 VARCHAR2(30),OBSV33 VARCHAR2(30),OBSV34 VARCHAR2(30),OBSV35 VARCHAR2(30),OBSV36 VARCHAR2(30),OBSV37 VARCHAR2(30),OBSV38 VARCHAR2(30),OBSV39 VARCHAR2(30),OBSV40 VARCHAR2(30),OBSV41 VARCHAR2(30),OBSV42 VARCHAR2(30),OBSV43 VARCHAR2(30),OBSV44 VARCHAR2(30),OBSV45 VARCHAR2(30),OBSV46 VARCHAR2(30),OBSV47 VARCHAR2(30),OBSV48 VARCHAR2(30),OBSV49 VARCHAR2(30),OBSV50 VARCHAR2(30),EDT_BY VARCHAR2(20),ENT_BY VARCHAR2(20),EDT_DT DATE , ENT_DT DATE)");

                ICO.add_icon(frm_qstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30350", 2, "QA Outward", 3, "-", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30352", 3, "LPE Form", 3, "../tej-base/om_lpe.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30355", 3, "MPE Form", 3, "../tej-base/om_mpe.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30357", 3, "PMI Form", 3, "../tej-base/om_pmi.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30359", 3, "Dim & Visual Exam.(DP Check Valve)", 3, "../tej-base/om_dp.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30361", 3, "Dim & Visual Exam.(BT/BD/BF)", 3, "../tej-base/om_BT_BD_BF.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30362", 3, "Pickling And Passivation", 3, "../tej-base/om_pick_pass.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30363", 3, "Surface Prep.,Paint & Mark", 3, "../tej-base/om_surf_paint.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit");
                ICO.add_icon(frm_qstr, "F30367", 3, "WO Dossier Index", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e1", "fin30_a1", "fin30_QAINSP", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F30364", 2, "QA Outward Master", 3, "-", "-", "-", "fin30_f1", "fin30_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F30365", 3, "Machine Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin30_f1", "fin30_a1", "fin30_QAMST", "fa-edit");
                ICO.add_icon(frm_qstr, "F30366", 3, "Chemical Grade Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin30_f1", "fin30_a1", "fin30_QAMST", "fa-edit");

                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F79000", 2, "Customer Portal", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F79150", 3, "Feature Reports", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e5", "fa-edit");
                ICO.add_icon(frm_qstr, "F79155", 4, "Download Valve T.C.", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e5", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47131", 3, "Dom.Orders Checklists", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47186", 3, "Sales Commission Report", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "Y", "N");


                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25121", 2, "Inventory Checklists", 3, "-", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25145", 3, "More Checklists ( Inventory)", 3, "-", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25169", 3, "Incoming Material Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25170", 3, "Packing Note Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_DOSDOC'", "TNAME");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_dosdoc (BRANCHCD CHAR(2) default '-',TYPE CHAR(2) default '-',VCHNUM CHAR(6) default '-',VCHDATE DATE default sysdate,ACODE CHAR(10) default '-',INVNO VARCHAR2(20) default '-',INVDATE DATE default sysdate,SRNO CHAR(4) default '-',LEGAL VARCHAR2(2) default '-',DOSSIER VARCHAR2(2) default '-',BG VARCHAR2(2) default '-',COL1 VARCHAR2(50) default '-',ENT_BY VARCHAR2(10) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(10) NOT NULL,EDT_DT DATE NOT NULL)");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70376", 2, "Unclaimed Amt Entry Screen", 3, "../tej-base/om_unclaimed.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70377", 3, "Outstanding Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70378", 3, "Daily Collection Report", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;

            case "STUD":
                WFIN_mgopts.PremiumSalesReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F50380", 4, "Order Book Main Group wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "Y");
                ICO.add_icon(frm_qstr, "F50382", 4, "Order Book Sub-Group wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "Y");
                ICO.add_icon(frm_qstr, "F50384", 4, "Order Book Item wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "Y");
                ICO.add_icon(frm_qstr, "F50390", 4, "Order to Invoice Main Group wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F50388", 4, "Order to Invoice Sub-Group wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F50386", 4, "Order to Invoice Item wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F50387", 4, "Balance Order Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15285", 3, "Premium Checklist- Purchase", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "fin15_pchk", "fa-edit");
                ICO.add_icon(frm_qstr, "F15286", 3, "Business Share Report", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_pchk", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70285", 3, "Order Registation Mail", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70206", 3, "Pending Voucher List", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70207", 3, "Voucher List (Assigned to)", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70190", 3, "Voucher Approval Matrix", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F49212", 4, "Invoice Print ", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/om_vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/om_vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/om_vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50051", 2, "Invoice Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin50_e1X", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50054", 2, "SOB(Share of Business) Report", 3, "../tej-base/om_view_sale.aspx", "-", "Y", "fin50_e1X", "fin50_a1", "-", "fa-edit", "Y", "N");
                break;
            case "TEST":
                WFIN_mgopts.IconRFQ_PO(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F49181", 4, "Export Bill details", 3, "../tej-base/om_exp_reg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F49185", 4, "Import Bill details", 3, "../tej-base/om_imp_reg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");

                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "IC0001", "DEV_A");

                    ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F10052S", 3, "Request Summary", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
                    ICO.add_icon(frm_qstr, "F10053", 3, "Request Status", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");

                    ICO.add_icon(frm_qstr, "F10249", 2, "Expense Management", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10250", 3, "Expense Recording", 3, "../tej-base/om_travel_expns.aspx", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");


                    ICO.add_icon(frm_qstr, "F10280", 3, "Reports", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit");
                    ICO.add_icon(frm_qstr, "F10281", 4, "Expense Detail Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F10282", 4, "Expense Detail Lead Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                }
                ICO.add_icon(frm_qstr, "F05000", 1, "Management MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F05100", 2, "Sales MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F05251", 3, "Seminar Data", 3, "../tej-base/gstList.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F45136", 3, "Detailed checkin Report", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F45137", 3, "Daily checkin Report", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F45138", 3, "No check out Report", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
                break;

            case "GCAP":
            case "GDOT":
                //case "SEFL":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25159", 4, "Job Work Raw Material", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25160", 4, "Job Work Finished Goods", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25163", 4, "FG Stock Value Report", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25220", 3, "Item Master Rate update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
                break;

            case "SPIR":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F39121", 3, "Moulding Prodn Checklists", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F40137", 4, "Trend of Rejection", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40138", 4, "Trend of DownTime", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40126", 4, "Daily Prodn Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40128", 4, "Down Time Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40129", 4, "Rejection Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F39140", 3, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F40143", 4, "Production with Rej % Itemwise", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40145", 4, "Production Log Print", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F40132", 4, "Daily Prodn Report", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40133", 4, "Mthly Prodn Report", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");

                // these are correct icons , all above are in prodpp to be shifted to prodpm....
                ICO.add_icon(frm_qstr, "F39190", 4, "Details of Items Produced-Qty", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F39192", 4, "Details of Items Rejected-Qty", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
                break;

            case "XDIL":
                WFIN_mgopts.Icon_FA_sys(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;

            case "RINT":
                // for marketing reports    
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                break;
            case "APTP":
                // for CUSTOMER VENDOR PORTAL    
                WFIN_mgopts.Icon_Supp_port(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Cust_port(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                break;
            case "VITR":
                // for FG GST REPORTS    
                WFIN_mgopts.PremiumFinanceReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70291", 4, "FG Stock Ledger", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70293", 4, "RM Stock Ledger", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70295", 4, "FG Stock Ledger-Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70296", 4, "RM Stock Ledger- Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25220", 3, "Item Master Rate update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50100", 3, "Dom.Sales Activity", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F50103", 4, "Import E-Comm Invoice", 3, "../tej-base/om_mrr_edi.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
                break;
            case "JPPL":
                // for FG GST REPORTS    
                WFIN_mgopts.PremiumFinanceReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70291", 4, "FG Stock Ledger", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70293", 4, "RM Stock Ledger", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70295", 4, "FG Stock Ledger-Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70296", 4, "RM Stock Ledger- Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                break;
            case "HIMT":
            case "GLOB":
            case "HIMO":
            case "HIMS":
            case "AARH":
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15131", 2, "Purchase Reports", 3, "-", "-", "Y", "fin15_e3", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15133", 3, "Purchase Order Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F50278", 4, "Monthly State wise,Group wise Sales Trend", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50279", 4, "Sale Trend- Group wise", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;

            case "WING":
                WFIN_mgopts.Icon_FA_sys(frm_qstr, frm_cocd);
                break;

            case "KPFL":
                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47140", 3, "Dom.Order Reports", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47222", 4, "Order Vs Despatch", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F47235", 4, "Sale Schedule Vs Despatch", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25125", 3, "Phy. Verification Entry", 3, "../tej-base/inv_rdr.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25120", 3, "Physical Verification Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F25123", 3, "Reel Stock Vs. Physical Verification", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25119", 3, "Missing Reels in Physical Verification", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40301", 3, "Production Reports(Detailed)", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40315", 4, "Job Wise Rejection reason Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40318", 4, "Job Wise Downtime reason Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F40324", 4, "Job Wise All Stage Rejection Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;

            case "TMI":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25131", 2, "Stock Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25264", 3, "Stock Statement-Detailed", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F25266", 3, "Stock Summary Batch wise", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

                break;

            case "NAHR":
                ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F15189", 3, "PO Report for mail", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10130", 2, "Production Masters", 3, "-", "-", "Y", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10136", 3, "Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10137", 3, "Ply Flute Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10138", 3, "Mill Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10139", 3, "Colour Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;

            case "KCLG":
                ICO.add_icon(frm_qstr, "F50326", 4, "Generate Invoice- Honda", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50325", 4, "Generate Invoice- Tungston", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                WFIN_mgopts.Icon_Cust_port(frm_qstr, frm_cocd);
                break;

            case "NEOP":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10049", 2, "Customer Care", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10050", 3, "Customer Request", 3, "../tej-base/om_cmplnt.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10051", 3, "Customer Request Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10060", 3, "Customer Complaint Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "fin10_e1mm", "fa-edit");
                ICO.add_icon(frm_qstr, "F10061", 4, "Complaint Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10062", 4, "Complaint Type Master", 3, "../tej-base/om_cmplnt.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10063", 4, "Complaint Division Master", 3, "../tej-base/om_cmplnt.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F10052", 3, "Action on Request", 3, "../tej-base/om_neopaction.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47100", 3, "Dom.Order Activity", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47101", 4, "Master S.O. (Dom.)", 3, "../tej-base/om_so_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47106", 4, "Supply S.O. (Dom.)", 3, "../tej-base/om_so_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                ICO.add_icon(frm_qstr, "F47106", 4, "Quotation Entry", 3, "../tej-base/om_so_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F50270", 4, "Sales Projection Sheet", 3, "../tej-base/om_sopproj.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F50272", 4, "Production SOP", 3, "../tej-base/om_SOP.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                break;

            case "UKB":
                // added 19/03/19 - on req of pkg sir
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25219", 3, "Item Master Min/Max/ROL update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
                break;
            case "CMPL":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55000", 2, "Export Sales Module", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F55140", 3, "Exp.Sales Reports", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F55161", 4, "Packing List(Exp)- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
                break;
            case "VPPL":
                WFIN_mgopts.Icon_Mgmt(frm_qstr, frm_cocd);
                break;
            case "BNPL":
                // for fixed assets records        
                WFIN_mgopts.Icon_FA_sys(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;

            case "PRIN":
                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10143", 3, "Paper RCT Index Master", 3, "../tej-base/om_paper_index.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10144", 3, "Box Costing (B/C/BC-Flute type)", 3, "../tej-base/om_costing_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10145", 3, "Box CS-BS Estimation", 3, "../tej-base/om_csbs_est.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10146", 3, "Caliper-Flute(Costing) Master ", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10147", 3, "Box Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10148", 3, "Paper Index/Rate(Costing) Master", 3, "../tej-base/om_caliper_flute.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10149", 3, "Box Costing (B/C/BC-Flute type)-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10150", 3, "Box CS-BS Estimation-Print", 3, "../tej-base/om_prt_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                break;
            case "BUPL":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F43000", 2, "Auto Comp Production", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F43121", 3, "Auto Comp Prodn Checklists", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F43135", 4, "Production OEE Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit", "N", "Y");
                WFIN_mgopts.Icon_FA_sys(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10184", 3, "FG Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10194", 3, "WIP Valuation on BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F10194E", 3, "Valuation on BOM Costing(Expendable)", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10198", 3, "RM,FG Ageing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");

                break;
            case "AGRM":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F39140", 3, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F39152", 4, "Molding Production Plan Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70200", 2, "Voucher Approval", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70201", 3, "Voucher Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70203", 3, "Voucher Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70204", 3, "Voucher Print", 3, "../tej-base/om_prt_acct.aspx", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
                break;

            case "CCC":
            case "CHEM":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;

            case "IPP":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;

            case "RTEC":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50310", 3, "Marketing Reports(Sales Module)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50328", 4, "Sch-vs-Rcpt-vs-Desp(Detail)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50329", 4, "Sch-vs-Rcpt-vs-Desp(Summary)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
                break;

            case "SAIP":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70270", 4, "Debtors' Ageing (detailed)- print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50143I", 4, "Invoice DSC Print", 3, "../tej-base/om_dsc_fetch.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                break;

            case "KWPL":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");
                break;

            case "SARN":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
                break;

            case "CRP":
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                break;

            case "MLGI":
                WFIN_mgopts.Icon_Hrm(frm_qstr, frm_cocd);
                WFIN_mgopts.Icon_Payr(frm_qstr, frm_cocd);
                break;

            case "VCL":
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40171", 3, "Packaging Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40351", 4, "Corrugation Process Plan Detail", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "N");
                break;
        }
        //11/2018

        mhd = fgen.chk_RsysUpd("DM0021");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0021','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0021", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='Y',BRN='N' where ID in ('F30132','F30121','F30126','F30127')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N',BRN='N' where ID in ('F30224','F30142','F30143')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_Action='../tej-base/om_view_acct.aspx' where id ='F70240'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set prd ='N',brn='N' where id in('F15135','F15140','F15141','F15136','F15244','F15238','F15233','F15235','F15234','F15240','F15239','F15143','F15231','F15232','F15230','F15249','F15247','F15248','F15241','F15242','F15236','F15237','F15228','F15226','F15229','F15225','F15227')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text = 'Sch/Receipt (Qty Based)' where id='F15247'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text = 'Sch/Receipt (Value Based)' where id ='F15248'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text = 'Sch/Receipt (Qty,Value)' where id='F15249'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_MASTER'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_MASTER ( BRANCHCD CHAR(2) DEFAULT '-',ID VARCHAR2(4) DEFAULT '-',VCHNUM CHAR(10) DEFAULT '-', VCHDATE DATE DEFAULT SYSDATE, ACODE CHAR(10) DEFAULT '-',ICODE CHAR(10) DEFAULT '-',NAME VARCHAR2(70) DEFAULT '-',CPARTNO VARCHAR2(30),SRNO NUMBER(4) DEFAULT 0, COL1 VARCHAR2(200) DEFAULT '-',COL2 VARCHAR2(20) DEFAULT '-',COL3 VARCHAR2(50) DEFAULT '-',COL4 VARCHAR2(50) DEFAULT '-',COL5 VARCHAR2(100) DEFAULT '-', COL6 VARCHAR2(100) DEFAULT '-', COL7 VARCHAR2(100) DEFAULT '-', COL8 VARCHAR2(100) DEFAULT '-', COL9 VARCHAR2(100) DEFAULT '-', COL10 VARCHAR2(100) DEFAULT '-',COL11 VARCHAR2(100) DEFAULT '-',COL12 VARCHAR2(100) DEFAULT '-',COL13 VARCHAR2(100) DEFAULT '-',COL14 VARCHAR2(100) DEFAULT '-',COL15 VARCHAR2(100) DEFAULT '-',ENT_BY VARCHAR2(20) DEFAULT '-',ENT_DT DATE DEFAULT SYSDATE,REMARKS VARCHAR2(300) DEFAULT '-',NUM1 NUMBER(20,3) DEFAULT 0,NUM2 NUMBER(20,3) DEFAULT 0,NUM3 NUMBER(20,3) DEFAULT 0,NUM4 NUMBER(20,3) DEFAULT 0,NUM5 NUMBER(20,3) DEFAULT 0,NUM6 NUMBER(20,3) DEFAULT 0,NUM7 NUMBER(15,3) DEFAULT 0,NUM8 NUMBER(15,3) DEFAULT 0,NUM9 NUMBER(15,3) DEFAULT 0,NUM10 NUMBER(15,3) DEFAULT 0,NUM11 NUMBER(15,3) DEFAULT 0,NUM12 NUMBER(15,3) DEFAULT 0,NUM13 NUMBER(15,3) DEFAULT 0,NUM14 NUMBER(15,3) DEFAULT 0,NUM15 NUMBER(15,3) DEFAULT 0,EDT_BY VARCHAR2(20) DEFAULT '-',EDT_DT DATE DEFAULT SYSDATE,NARATION VARCHAR2(150) DEFAULT '-',DATE1 DATE DEFAULT SYSDATE,DATE2 DATE DEFAULT SYSDATE,DOCDATE DATE DEFAULT SYSDATE,IMAGEF varchar2(50), IMAGEPATH varchar2(250) )");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WSR_CTRL", "TYPE");
            if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WSR_CTRL MODIFY TYPE CHAR(4) DEFAULT '-'");

            //FA controls
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W0049", "Icode starting from for FA sale ", "Y", "5");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W0050", "Sale type for FA sale ", "Y", "4D");
            //MRR continous numbering
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W0051", "MRR No running for all types? ", "Y", "-");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N',BRN='N' where ID in ('F47226','F47227','F30143')");

            ICO.add_icon(frm_qstr, "F99122", 3, "Desktop Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99164", 3, "Desktop Rights Master", 3, "../tej-base/om_appr.aspx", "-", "-", "fin99_e5", "fin99_a1", "-", "fa-edit", "N", "Y");

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS MODIFY SEARCH_KEY VARCHAR2(150) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin30_e3' where id in ('F30128')");


            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='CSMST_CRM'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table CSMST_CRM as(Select * from csmst where 1=2)");
        }

        #region pexe table variable
        mhd = fgen.chk_RsysUpd("DM0022");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0022','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0022", "DEV_A");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CO", "VERCHK1");
            if (mhd != "0")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD APP_BY VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify APP_BY VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD APP_DT VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify APP_DT VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD JWQ_CTRL VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify JWQ_CTRL VARCHAR2(1) default '-'");


                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD CHK_BY VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify CHK_BY VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD CHK_DT DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify CHK_DT DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ED_SERV VARCHAR2(30)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ED_SERV VARCHAR2(30) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH1  VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH1  VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD PDISCAMT2 NUMBEr(12,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify PDISCAMT2 NUMBEr(12,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD TXB_FRT NUMBEr(12,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify TXB_FRT NUMBEr(12,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD VALIDUPTO DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify VALIDUPTO DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD PO_TOLR number(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify PO_TOLR number(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH2  VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH2 VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH3 VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH3 VARCHAR2(100) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CUSTGRP  VARCHAR2(35)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CUSTGRP  VARCHAR2(35) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DLVTIME NUMBER(5)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DLVTIME NUMBER(5) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD MED_LIC VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify MED_LIC VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD HUBSTK VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify HUBSTK VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD HR_ML VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify HR_ML VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD APPRV_BY VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify APPRV_BY VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD APPRV_DT DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify APPRV_DT DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD ZONAME  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify ZONAME  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CONTINENT  VARCHAR2(35)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CONTINENT  VARCHAR2(35) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD FIMGLINK VARCHAR(125)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify FIMGLINK VARCHAR(125) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD EMAIL2  VARCHAR2(325)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify EMAIL2  VARCHAR2(325) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CIN_NO  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CIN_NO  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD RTG_SWIFT VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify RTG_SWIFT VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD RTG_TEL  VARCHAR2(45)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify RTG_TEL  VARCHAR2(45) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_TERM  VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_TERM  VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_COD  VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_COD  VARCHAR2(20 default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_NOTE VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_NOTE VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_WAYB  VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_WAYB  VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD OTH_NOTES  VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify OTH_NOTES  VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD GSTPVEXP VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify GSTPVEXP VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD GSTNA VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify GSTNA VARCHAR2(1) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMSTBAL ADD VEN_CODE VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMSTBAL modify VEN_CODE VARCHAR2(10) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP  ADD ORDLINENO VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify ORDLINENO VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD GST_POS VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify GST_POS VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD DOC_TOT NUMBER(14,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify DOC_TOT NUMBER(14,3) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD TPT_NAMES VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify TPT_NAMES VARCHAR2(50) default '-'");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='IVCH_HIST'", "tname");
                if (mhd == "0" || mhd == "") { }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD ORDLINENO  VARCHAR2(25)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify ORDLINENO  VARCHAR2(25) default '-'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD FORM31 VARCHAR2(10)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify FORM31 VARCHAR2(10) default '-'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD DOC_TOT NUMBER(14,3)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify DOC_TOT NUMBER(14,3) default 0");
                }
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD ORDLINENO  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify ORDLINENO  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD FORM31 VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify FORM31 VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD DOC_TOT NUMBER(14,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify DOC_TOT NUMBER(14,3) default 0");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='REELVCH'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table REELVCH (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM  CHAR(6),VCHDATE DATE,ICODE  CHAR(30),SRNO  NUMBER(8),COREELNO VARCHAR2(20),KCLREELNO CHAR(10),REELWIN NUMBER(14,3),REELWOUT  NUMBER(14,3),IRATE  NUMBER(14,3),JOB_NO  CHAR(6),REELSPEC1 CHAR(20),REELSPEC2  VARCHAR2(50),PSIZE NUMBER(11,2),GSM  NUMBER(11,2),ACODE  CHAR(10),GRADE  CHAR(10),REC_ISS CHAR(1),REELHIN NUMBER(14,3),UNLINK CHAR(1),POSTED CHAR(1),JOB_DT CHAR(11),STORE_NO VARCHAR2(10),RINSP_BY  VARCHAR2(20),RLOCN  VARCHAR2(10),UINSP  NUMBER(12,2),REELMTR  NUMBER(12,2),REEL_AT VARCHAR2(20),REEL_REJQTY NUMBER(10,2),PO_NUM VARCHAR2(6),RPAPINSP VARCHAR2(30))");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CUST_PER NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CUST_PER NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CUST_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CUST_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CST_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CST_RATE NUMBER(10,3 default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD LST_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify LST_RATE NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD SHCESS_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify SHCESS_RATE NUMBER(10,3 default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD FRTPAY VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify FRTPAY VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MAINITEM VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MAINITEM VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MAINUNIT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MAINUNIT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MATAC VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MATAC VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD TAXCODE VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify TAXCODE VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD LESSAMT NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify LESSAMT NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD RNDCESS NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify RNDCESS NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD S_LST NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify S_LST NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD EXCB_CHG NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify EXCB_CHG NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ED_EXTRA VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ED_EXTRA VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD PACK_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify PACK_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD INSU_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify INSU_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD FRT_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify FRT_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD WHNAME VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify WHNAME VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ATCH1 VARCHAR2(80)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ATCH1 VARCHAR2(80) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ATCH2 VARCHAR2(80)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ATCH2 VARCHAR2(80) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_GRNO VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_GRNO VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_GRDT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_GRDT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_NAME VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_NAME VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_VNO VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_VNO VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD BE_REFDT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify BE_REFDT VARCHAR2(10) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO ADD IREMARKS VARCHAR2(120)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO modify IREMARKS VARCHAR2(120) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD STATEN  VARCHAR2(30)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify STATEN  VARCHAR2(30) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD CSTAFFCD VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify CSTAFFCD VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD BANK_AC  VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify BANK_AC  VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD IFSC_CD  VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify IFSC_CD  VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD CS_DISTANCE NUMBER(5)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify CS_DISTANCE NUMBER(5) default 0");
            }
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CO", "VERCHK1");
            if (mhd != "0")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD APP_BY VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify APP_BY VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD APP_DT VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify APP_DT VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD JWQ_CTRL VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify JWQ_CTRL VARCHAR2(1) default '-'");


                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD CHK_BY VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify CHK_BY VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD CHK_DT DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify CHK_DT DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ED_SERV VARCHAR2(30)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ED_SERV VARCHAR2(30) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH1  VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH1  VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD PDISCAMT2 NUMBEr(12,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify PDISCAMT2 NUMBEr(12,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD TXB_FRT NUMBEr(12,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify TXB_FRT NUMBEr(12,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD VALIDUPTO DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify VALIDUPTO DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD PO_TOLR number(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify PO_TOLR number(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH2  VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH2 VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH3 VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH3 VARCHAR2(100) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CUSTGRP  VARCHAR2(35)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CUSTGRP  VARCHAR2(35) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DLVTIME NUMBER(5)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DLVTIME NUMBER(5) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD MED_LIC VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify MED_LIC VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD HUBSTK VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify HUBSTK VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD HR_ML VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify HR_ML VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD APPRV_BY VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify APPRV_BY VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD APPRV_DT DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify APPRV_DT DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD ZONAME  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify ZONAME  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CONTINENT  VARCHAR2(35)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CONTINENT  VARCHAR2(35) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD FIMGLINK VARCHAR(125)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify FIMGLINK VARCHAR(125) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD EMAIL2  VARCHAR2(325)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify EMAIL2  VARCHAR2(325) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CIN_NO  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CIN_NO  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD RTG_SWIFT VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify RTG_SWIFT VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD RTG_TEL  VARCHAR2(45)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify RTG_TEL  VARCHAR2(45) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_TERM  VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_TERM  VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_COD  VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_COD  VARCHAR2(20 default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_NOTE VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_NOTE VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_WAYB  VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_WAYB  VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD OTH_NOTES  VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify OTH_NOTES  VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD GSTPVEXP VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify GSTPVEXP VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD GSTNA VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify GSTNA VARCHAR2(1) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMSTBAL ADD VEN_CODE VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMSTBAL modify VEN_CODE VARCHAR2(10) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP  ADD ORDLINENO VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify ORDLINENO VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD GST_POS VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify GST_POS VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD DOC_TOT NUMBER(14,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify DOC_TOT NUMBER(14,3) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD TPT_NAMES VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify TPT_NAMES VARCHAR2(50) default '-'");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='IVCH_HIST'", "tname");
                if (mhd == "0" || mhd == "") { }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD ORDLINENO  VARCHAR2(25)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify ORDLINENO  VARCHAR2(25) default '-'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD FORM31 VARCHAR2(10)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify FORM31 VARCHAR2(10) default '-'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD DOC_TOT NUMBER(14,3)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify DOC_TOT NUMBER(14,3) default 0");
                }

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD ORDLINENO  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify ORDLINENO  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD FORM31 VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify FORM31 VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD DOC_TOT NUMBER(14,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify DOC_TOT NUMBER(14,3) default 0");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='REELVCH'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table REELVCH (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM  CHAR(6),VCHDATE DATE,ICODE  CHAR(30),SRNO  NUMBER(8),COREELNO VARCHAR2(20),KCLREELNO CHAR(10),REELWIN NUMBER(14,3),REELWOUT  NUMBER(14,3),IRATE  NUMBER(14,3),JOB_NO  CHAR(6),REELSPEC1 CHAR(20),REELSPEC2  VARCHAR2(50),PSIZE NUMBER(11,2),GSM  NUMBER(11,2),ACODE  CHAR(10),GRADE  CHAR(10),REC_ISS CHAR(1),REELHIN NUMBER(14,3),UNLINK CHAR(1),POSTED CHAR(1),JOB_DT CHAR(11),STORE_NO VARCHAR2(10),RINSP_BY  VARCHAR2(20),RLOCN  VARCHAR2(10),UINSP  NUMBER(12,2),REELMTR  NUMBER(12,2),REEL_AT VARCHAR2(20),REEL_REJQTY NUMBER(10,2),PO_NUM VARCHAR2(6),RPAPINSP VARCHAR2(30))");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CUST_PER NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CUST_PER NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CUST_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CUST_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CST_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CST_RATE NUMBER(10,3 default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD LST_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify LST_RATE NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD SHCESS_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify SHCESS_RATE NUMBER(10,3 default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD FRTPAY VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify FRTPAY VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MAINITEM VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MAINITEM VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MAINUNIT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MAINUNIT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MATAC VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MATAC VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD TAXCODE VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify TAXCODE VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD LESSAMT NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify LESSAMT NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD RNDCESS NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify RNDCESS NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD S_LST NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify S_LST NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD EXCB_CHG NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify EXCB_CHG NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ED_EXTRA VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ED_EXTRA VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD PACK_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify PACK_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD INSU_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify INSU_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD FRT_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify FRT_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD WHNAME VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify WHNAME VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ATCH1 VARCHAR2(80)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ATCH1 VARCHAR2(80) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ATCH2 VARCHAR2(80)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ATCH2 VARCHAR2(80) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_GRNO VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_GRNO VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_GRDT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_GRDT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_NAME VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_NAME VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_VNO VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_VNO VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD BE_REFDT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify BE_REFDT VARCHAR2(10) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO ADD IREMARKS VARCHAR2(120)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO modify IREMARKS VARCHAR2(120) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD STATEN  VARCHAR2(30)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify STATEN  VARCHAR2(30) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD CSTAFFCD VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify CSTAFFCD VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD BANK_AC  VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify BANK_AC  VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD IFSC_CD  VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify IFSC_CD  VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD CS_DISTANCE NUMBER(5)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify CS_DISTANCE NUMBER(5) default 0");
            }
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CO", "VERCHK1");
            if (mhd != "0")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD APP_BY VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify APP_BY VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD APP_DT VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify APP_DT VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD JWQ_CTRL VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify JWQ_CTRL VARCHAR2(1) default '-'");


                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD CHK_BY VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify CHK_BY VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD CHK_DT DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify CHK_DT DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ED_SERV VARCHAR2(30)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ED_SERV VARCHAR2(30) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH1  VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH1  VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD PDISCAMT2 NUMBEr(12,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify PDISCAMT2 NUMBEr(12,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD TXB_FRT NUMBEr(12,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify TXB_FRT NUMBEr(12,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD VALIDUPTO DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify VALIDUPTO DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD PO_TOLR number(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify PO_TOLR number(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH2  VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH2 VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS ADD ATCH3 VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE POMAS modify ATCH3 VARCHAR2(100) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CUSTGRP  VARCHAR2(35)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CUSTGRP  VARCHAR2(35) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DLVTIME NUMBER(5)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DLVTIME NUMBER(5) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD MED_LIC VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify MED_LIC VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD HUBSTK VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify HUBSTK VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD HR_ML VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify HR_ML VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD APPRV_BY VARCHAR2(15)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify APPRV_BY VARCHAR2(15) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD APPRV_DT DATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify APPRV_DT DATE default SYSDATE");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD ZONAME  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify ZONAME  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CONTINENT  VARCHAR2(35)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CONTINENT  VARCHAR2(35) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD FIMGLINK VARCHAR(125)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify FIMGLINK VARCHAR(125) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD EMAIL2  VARCHAR2(325)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify EMAIL2  VARCHAR2(325) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD CIN_NO  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify CIN_NO  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD RTG_SWIFT VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify RTG_SWIFT VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD RTG_TEL  VARCHAR2(45)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify RTG_TEL  VARCHAR2(45) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_TERM  VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_TERM  VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_COD  VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_COD  VARCHAR2(20 default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_NOTE VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_NOTE VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD DEL_WAYB  VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify DEL_WAYB  VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD OTH_NOTES  VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify OTH_NOTES  VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD GSTPVEXP VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify GSTPVEXP VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD GSTNA VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify GSTNA VARCHAR2(1) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMSTBAL ADD VEN_CODE VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMSTBAL modify VEN_CODE VARCHAR2(10) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP  ADD ORDLINENO VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify ORDLINENO VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD GST_POS VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify GST_POS VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD DOC_TOT NUMBER(14,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify DOC_TOT NUMBER(14,3) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD TPT_NAMES VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify TPT_NAMES VARCHAR2(50) default '-'");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='IVCH_HIST'", "tname");
                if (mhd == "0" || mhd == "") { }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD ORDLINENO  VARCHAR2(25)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify ORDLINENO  VARCHAR2(25) default '-'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD FORM31 VARCHAR2(10)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify FORM31 VARCHAR2(10) default '-'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD DOC_TOT NUMBER(14,3)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify DOC_TOT NUMBER(14,3) default 0");
                }

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD ORDLINENO  VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify ORDLINENO  VARCHAR2(25) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD FORM31 VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify FORM31 VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD DOC_TOT NUMBER(14,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify DOC_TOT NUMBER(14,3) default 0");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='REELVCH'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table REELVCH (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM  CHAR(6),VCHDATE DATE,ICODE  CHAR(30),SRNO  NUMBER(8),COREELNO VARCHAR2(20),KCLREELNO CHAR(10),REELWIN NUMBER(14,3),REELWOUT  NUMBER(14,3),IRATE  NUMBER(14,3),JOB_NO  CHAR(6),REELSPEC1 CHAR(20),REELSPEC2  VARCHAR2(50),PSIZE NUMBER(11,2),GSM  NUMBER(11,2),ACODE  CHAR(10),GRADE  CHAR(10),REC_ISS CHAR(1),REELHIN NUMBER(14,3),UNLINK CHAR(1),POSTED CHAR(1),JOB_DT CHAR(11),STORE_NO VARCHAR2(10),RINSP_BY  VARCHAR2(20),RLOCN  VARCHAR2(10),UINSP  NUMBER(12,2),REELMTR  NUMBER(12,2),REEL_AT VARCHAR2(20),REEL_REJQTY NUMBER(10,2),PO_NUM VARCHAR2(6),RPAPINSP VARCHAR2(30))");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CUST_PER NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CUST_PER NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CUST_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CUST_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD CST_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify CST_RATE NUMBER(10,3 default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD LST_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify LST_RATE NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD SHCESS_RATE NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify SHCESS_RATE NUMBER(10,3 default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD FRTPAY VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify FRTPAY VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MAINITEM VARCHAR2(100)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MAINITEM VARCHAR2(100) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MAINUNIT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MAINUNIT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD MATAC VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify MATAC VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD TAXCODE VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify TAXCODE VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD LESSAMT NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify LESSAMT NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD RNDCESS NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify RNDCESS NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD S_LST NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify S_LST NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD EXCB_CHG NUMBER(10,2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify EXCB_CHG NUMBER(10,2) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ED_EXTRA VARCHAR2(1)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ED_EXTRA VARCHAR2(1) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD PACK_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify PACK_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD INSU_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify INSU_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD FRT_AMT NUMBER(10,3)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify FRT_AMT NUMBER(10,3) default 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD WHNAME VARCHAR2(50)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify WHNAME VARCHAR2(50) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ATCH1 VARCHAR2(80)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ATCH1 VARCHAR2(80) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD ATCH2 VARCHAR2(80)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify ATCH2 VARCHAR2(80) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_GRNO VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_GRNO VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_GRDT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_GRDT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_NAME VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_NAME VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD T_VNO VARCHAR2(20)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify T_VNO VARCHAR2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD BE_REFDT VARCHAR2(10)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL modify BE_REFDT VARCHAR2(10) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO ADD IREMARKS VARCHAR2(120)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO modify IREMARKS VARCHAR2(120) default '-'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD STATEN  VARCHAR2(30)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify STATEN  VARCHAR2(30) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD CSTAFFCD VARCHAR2(2)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify CSTAFFCD VARCHAR2(2) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD BANK_AC  VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify BANK_AC  VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD IFSC_CD  VARCHAR2(40)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify IFSC_CD  VARCHAR2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST ADD CS_DISTANCE NUMBER(5)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST modify CS_DISTANCE NUMBER(5) default 0");
            }
        }
        #endregion pexe table variable
        mhd = fgen.chk_RsysUpd("DM0023");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0023','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0023", "DEV_A");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "05/01/2019", "DEV_B", "W1078", "Start Date for Mould Maintenance ", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "15/01/2019", "DEV_B", "W1079", "Alert % Balance Mould Life (Shots)", "N", "-");
            //fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "14/01/2019", "DEV_B", "W1079", "Mould Maintenance master linked to main finsys master?", "Y", "-");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_MASTER", "CPARTNO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_MASTER ADD CPARTNO VARCHAR2(30) DEFAULT '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_maint_mchplan.aspx' WHERE ID ='F75101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Graph : Sales',submenuid='fin05_e11',param='fin05_mgr3'  WHERE ID ='F05225'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Sales Breakup (Top 10 Parties)',submenuid='fin05_e11',param='fin05_mgr3'  WHERE ID ='F05226'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Sales Vs Coll Month Wise',submenuid='fin05_e11',param='fin05_mgr3'  WHERE ID ='F05229'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table sys_config modify frm_title varchar2(45) default '-' ");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "REASON");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD REASON VARCHAR2(200) DEFAULT '-'");
            //25/03/19 after sagm icon diff

            /// below line is commented by vv, this was wrongly put by some one, profitability report

            //fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='Y' where ID in ('F05124')");
            if (frm_cocd == "SAGM" || frm_cocd == "SAGI")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set form='fin10_a1', submenuid='fin10_e6' where ID='F05125a'");
            }
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/om_view_sys.aspx' where ID in ('F99126','F99127','F99128','F99129')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Maintenance Machine' where ID='F75165'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CACOST'", "TNAME");
            if (mhd != "0")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "CHAPLET");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE wb_cacost add CHAPLET number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "HEATING");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add HEATING number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "MLD_OTHER");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE wb_cacost add MLD_OTHER number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "SLEEVE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add SLEEVE number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "SAND");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add SAND number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "PAINTING");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add PAINTING number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "CONV_OTHER1");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add CONV_OTHER1 number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "CONV_OTHER2");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE wb_cacost add CONV_OTHER2 number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_ICODE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_ICODE varchar2(8) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_FERRO");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_FERRO varchar2(20) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_REC");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_REC number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_REQKG");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_REQKG number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_RATE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_RATE number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_COST");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_COST number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_CONTRI");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_CONTRI number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_PIGIRON");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_PIGIRON number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_REQ");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_REQ number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "GRID_DIFF");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add GRID_DIFF number(20,3) DEFAULT '0'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where ID in ('F82562','F82565','F82567','F82571','F85149','F85148','F85147','F85150','F85151')");
                if (frm_cocd == "SAGM")
                {
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_EXP_FRT'", "TNAME");
                    if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_EXP_FRT (BRANCHCD CHAR(2) NOT NULL,TYPE CHAR(2) NOT NULL,VCHNUM CHAR(6) NOT NULL,VCHDATE DATE NOT NULL,ACODE CHAR(10) NOT NULL,ICODE CHAR(10) NOT NULL,SRNO NUMBER(10),IQTYIN  NUMBER(35,2) NOT NULL,IQTYOUT NUMBER(35,2),IAMOUNT NUMBER(35,2),CIF_VAL NUMBER(35,2),FORIGN_VAL NUMBER(35,2),CONTAINER_NO VARCHAR2(100),CONT_SIZE VARCHAR2(100),CINAME VARCHAR2(135),DESC_ VARCHAR2(200),REFNUM CHAR(15),REFDATE DATE,FLAG  CHAR(2),BILLNO  VARCHAR2(30),BILL_DT  DATE,INVNO VARCHAR2(10),INVDATE DATE,CSCODE  CHAR(10),APP_BY VARCHAR2(20),PBASIS  VARCHAR2(100),REMARK  VARCHAR2(300),TERM  VARCHAR2(100),NUM1  NUMBER(30,2),NUM2 NUMBER(30,2),NUM3  NUMBER(30,2),NUM4 NUMBER(30,2),NUM5  NUMBER(30,2),NUM6  NUMBER(30,2),NUM7 NUMBER(30,2),NUM8 NUMBER(30,2),NUM9 NUMBER(30,2),NUM10 NUMBER(30,2),NUM11 NUMBER(30,2),NUM12   NUMBER(30,2),NUM13 NUMBER(30,2),NUM14 NUMBER(30,2),NUM15 NUMBER(30,2),NUM16 NUMBER(30,2),NUM17 NUMBER(30,2),NUM18 NUMBER(30,2),NUM19 NUMBER(30,2),NUM20 NUMBER(30,2),OBSV1 VARCHAR2(80) DEFAULT '-',OBSV2 VARCHAR2(80) DEFAULT '-',OBSV3 VARCHAR2(80) DEFAULT '-',OBSV4  VARCHAR2(80) DEFAULT '-',OBSV5 VARCHAR2(80) DEFAULT '-',OBSV6 VARCHAR2(80) DEFAULT '-',OBSV7 VARCHAR2(80) DEFAULT '-',OBSV8  VARCHAR2(80) DEFAULT '-',OBSV9  VARCHAR2(80) DEFAULT '-',OBSV10 VARCHAR2(80) DEFAULT '-',OBSV11 VARCHAR2(80) DEFAULT '-',OBSV12 VARCHAR2(80) DEFAULT '-',OBSV13 VARCHAR2(80) DEFAULT '-',OBSV14 VARCHAR2(80) DEFAULT '-',OBSV15 VARCHAR2(80) DEFAULT '-',ENT_BY  VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(15),EDT_DT  DATE NOT NULL,RMK1 VARCHAR2(100) DEFAULT '-',RMK2 VARCHAR2(100) DEFAULT '-',RMK3 VARCHAR2(100) DEFAULT '-',RMK4 VARCHAR2(100) DEFAULT '-',COMM_DESC VARCHAR2(50) DEFAULT '-',DATE1 VARCHAR2(12),DATE2 VARCHAR2(12),DATE3 VARCHAR2(12),DATE4 VARCHAR2(12),DATE5 VARCHAR2(12),DATE6  VARCHAR2(12),DATE7 VARCHAR2(12),DATE8 VARCHAR2(12),TOTSHIP_CHG VARCHAR2(40) DEFAULT '-',TOTCHA_CHG VARCHAR2(40),SHIP_LINE VARCHAR2(40) DEFAULT '-',HSCODE VARCHAR2(30),VCODE VARCHAR2(20),PORT_DESCH VARCHAR2(30),DELV VARCHAR2(30),RECIEPT VARCHAR2(30),PLOAD VARCHAR2(30),PRECIEPT VARCHAR2(30),VCODE2 VARCHAR2(20))");
                }
                fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where id in ('F40126')");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "INTEREST_PER2");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add INTEREST_PER2 number(20,3) DEFAULT '0'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "LINE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add LINE varchar2(10) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "CORE_TYPE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add CORE_TYPE varchar2(20) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "CHILDCODE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add CHILDCODE VARCHAR2(10) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "PARENTCHILD");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST add PARENTCHILD VARCHAR2(20) DEFAULT '-'");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where ID in ('F82562','F82565','F82567','F82571','F85149','F85148','F85147','F85150','F85151')");
            if (frm_cocd == "SAGM")
            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_EXP_FRT'", "TNAME");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_EXP_FRT (BRANCHCD CHAR(2) NOT NULL,TYPE CHAR(2) NOT NULL,VCHNUM CHAR(6) NOT NULL,VCHDATE DATE NOT NULL,ACODE CHAR(10) NOT NULL,ICODE CHAR(10) NOT NULL,SRNO NUMBER(10),IQTYIN  NUMBER(35,2) NOT NULL,IQTYOUT NUMBER(35,2),IAMOUNT NUMBER(35,2),CIF_VAL NUMBER(35,2),FORIGN_VAL NUMBER(35,2),CONTAINER_NO VARCHAR2(100),CONT_SIZE VARCHAR2(100),CINAME VARCHAR2(135),DESC_ VARCHAR2(200),REFNUM CHAR(15),REFDATE DATE,FLAG  CHAR(2),BILLNO  VARCHAR2(30),BILL_DT  DATE,INVNO VARCHAR2(10),INVDATE DATE,CSCODE  CHAR(10),APP_BY VARCHAR2(20),PBASIS  VARCHAR2(100),REMARK  VARCHAR2(300),TERM  VARCHAR2(100),NUM1  NUMBER(30,2),NUM2 NUMBER(30,2),NUM3  NUMBER(30,2),NUM4 NUMBER(30,2),NUM5  NUMBER(30,2),NUM6  NUMBER(30,2),NUM7 NUMBER(30,2),NUM8 NUMBER(30,2),NUM9 NUMBER(30,2),NUM10 NUMBER(30,2),NUM11 NUMBER(30,2),NUM12   NUMBER(30,2),NUM13 NUMBER(30,2),NUM14 NUMBER(30,2),NUM15 NUMBER(30,2),NUM16 NUMBER(30,2),NUM17 NUMBER(30,2),NUM18 NUMBER(30,2),NUM19 NUMBER(30,2),NUM20 NUMBER(30,2),OBSV1 VARCHAR2(80) DEFAULT '-',OBSV2 VARCHAR2(80) DEFAULT '-',OBSV3 VARCHAR2(80) DEFAULT '-',OBSV4  VARCHAR2(80) DEFAULT '-',OBSV5 VARCHAR2(80) DEFAULT '-',OBSV6 VARCHAR2(80) DEFAULT '-',OBSV7 VARCHAR2(80) DEFAULT '-',OBSV8  VARCHAR2(80) DEFAULT '-',OBSV9  VARCHAR2(80) DEFAULT '-',OBSV10 VARCHAR2(80) DEFAULT '-',OBSV11 VARCHAR2(80) DEFAULT '-',OBSV12 VARCHAR2(80) DEFAULT '-',OBSV13 VARCHAR2(80) DEFAULT '-',OBSV14 VARCHAR2(80) DEFAULT '-',OBSV15 VARCHAR2(80) DEFAULT '-',ENT_BY  VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(15),EDT_DT  DATE NOT NULL,RMK1 VARCHAR2(100) DEFAULT '-',RMK2 VARCHAR2(100) DEFAULT '-',RMK3 VARCHAR2(100) DEFAULT '-',RMK4 VARCHAR2(100) DEFAULT '-',COMM_DESC VARCHAR2(50) DEFAULT '-',DATE1 VARCHAR2(12),DATE2 VARCHAR2(12),DATE3 VARCHAR2(12),DATE4 VARCHAR2(12),DATE5 VARCHAR2(12),DATE6  VARCHAR2(12),DATE7 VARCHAR2(12),DATE8 VARCHAR2(12),TOTSHIP_CHG VARCHAR2(40) DEFAULT '-',TOTCHA_CHG VARCHAR2(40),SHIP_LINE VARCHAR2(40) DEFAULT '-',HSCODE VARCHAR2(30),VCODE VARCHAR2(20),PORT_DESCH VARCHAR2(30),DELV VARCHAR2(30),RECIEPT VARCHAR2(30),PLOAD VARCHAR2(30),PRECIEPT VARCHAR2(30),VCODE2 VARCHAR2(20))");
            }
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where id in ('F40126')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79141'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79142'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79144'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79145'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys SET prd='N'  where id='F79139'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set TEXT='Debtors Ageing Summary Report' , prd='Y'  where id='F79134'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set TEXT='Production SOP', prd='N'  where id='F50272'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78144'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78143'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78143'");
            // CUSTOMER PORTAL REPORS
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set web_Action='../tej-base/om_view_cport.aspx'  where id='F79143'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set BRN='N',PRD='N'  where id IN ('F79136','F79111','F79126')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set web_Action='../tej-base/om_view_cport.aspx',prd='Y'  where id='F79122'");
        }

        mhd = fgen.chk_RsysUpd("DM0028");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0028','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0028", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where id in ('F40126')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79141'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79142'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79144'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set web_Action='../tej-base/om_view_cport.aspx' ,prd='N'  where id='F79145'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys SET prd='N'  where id='F79139'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set TEXT='Debtors Ageing Summary Report' , prd='Y'  where id='F79134'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys  set TEXT='Production SOP', prd='N'  where id='F50272'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78144'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78143'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78143'");
            // CUSTOMER PORTAL REPORS
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set web_Action='../tej-base/om_view_cport.aspx'  where id='F79143'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set BRN='N',PRD='N'  where id IN ('F79136','F79111','F79126')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set web_Action='../tej-base/om_view_cport.aspx',prd='Y'  where id='F79122'");
            //-----------------------
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/05/2019", "DEV_A", "W2021", "Tolerance in BOM consumption", "N", "2");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set web_Action='../tej-base/om_view_sale.aspx'  where id='F50321'");

            if (frm_cocd == "HGLO")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "DT1");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD DT1 number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "DT2");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD DT2 number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "DT3");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD DT3 number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "DT4");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD DT4 number(5,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "DT5");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD DT5 number(6,2) DEFAULT 0");
            }
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "28/05/2019", "DEV_A", "W2022", "PF Employee %", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "28/05/2019", "DEV_A", "W2023", "PF Employer %", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "28/05/2019", "DEV_A", "W2024", "ESI Employee %", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "28/05/2019", "DEV_A", "W2025", "ESI Employer %", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "28/05/2019", "DEV_A", "W2026", "No. of times Employer Contri for WF", "N", "2");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set BRN='Y' where id in('F70406','F70407','F70408','F70409','F70412','F70413','F70414','F70416','F70417','F70418','F70432','F70433','F70434','F70435','F70436')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_pay_data.aspx' where id='F85106'");
            //fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_pay_incr.aspx' where id='F85103'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_pay_incr.aspx' where id='F85109'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_Appr.aspx' where id='F85145'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_Appr.aspx' where id='F85143'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_LEAD_LOG'", "TNAME");
            if (mhd != "0")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LSRC");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LSRC VARCHAR2(30) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LDESC");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LDESC VARCHAR2(200) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "REFFBY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD REFFBY VARCHAR2(50) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "ASSG");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD ASSG VARCHAR2(50) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PENQNO");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PENQNO VARCHAR2(6) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "STAGE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD STAGE VARCHAR2(100) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "EXPVAL");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD EXPVAL NUMBER(12,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "SDESC");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD SDESC VARCHAR2(200) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PRIORITY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PRIORITY VARCHAR2(20) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "EXPCDT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD EXPCDT VARCHAR2(10) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LPROB");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LPROB VARCHAR2(10) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "SDT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD SDT VARCHAR2(30) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "NDT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD NDT VARCHAR2(30) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "FCOST");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD FCOST NUMBER(12,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "FCURR");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD FCURR VARCHAR2(6)  DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "EXPCURR");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD EXPCURR VARCHAR2(6) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "REMINDER");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD REMINDER CHAR(1) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "AFOLLOW");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD AFOLLOW  CHAR(1) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PLEAD");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PLEAD CHAR(1) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PMGRP");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PMGRP VARCHAR2(50) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PSGRP");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PSGRP VARCHAR2(200) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "QTY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD QTY  NUMBER(12,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "QRATE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD QRATE NUMBER(12,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "QCURR");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD QCURR VARCHAR2(6) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "UNIT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD UNIT VARCHAR2(20) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "DESC_");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD DESC_ VARCHAR2(500) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PMODE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PMODE VARCHAR2(50) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "PTERM");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD PTERM VARCHAR2(150) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "REMARK");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD REMARK VARCHAR2(200) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "OVAL");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD OVAL NUMBER(12,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "OCURR");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD OCURR VARCHAR2(6) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "CRATE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD CRATE NUMBER(5,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "CORATE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD CORATE NUMBER(5,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "CAMT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD CAMT NUMBER(15,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "COAMT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD COAMT NUMBER(15,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "QVAL");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD QVAL NUMBER(5,3) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LEAD_SRNO");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LEAD_SRNO CHAR(4)");
            }
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_LEAD_ACT'", "TNAME");
            if (mhd != "0")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_ACT", "PMGRP");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_ACT ADD PMGRP VARCHAR2(50) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_ACT", "PSGRP");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_ACT ADD PSGRP VARCHAR2(200) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_ACT", "LEAD_SRNO");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_ACT ADD LEAD_SRNO CHAR(4) DEFAULT '-'");
            }

            if (frm_cocd != "HGLO")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85107'"); // SALARY PREPRATION MADE FOR HGLO IS HIDE FOR ALL CLIENTS EXCEPT HGLO
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85102'"); // ATTENDANCE ENTRY FORM MADE FOR HGLO IS HIDE FOR ALL CLIENTS EXCEPT HGLO
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85152'"); // SALARY REGISTER MADE FOR HGLO IS HIDE FOR ALL CLIENTS EXCEPT HGLO
            }
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85151'"); // WELFARE CONTRIBUTION REPORT (HIDE BECAUSE OF DUPLICATE ICON)        
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85134'"); // PAY CALC. MASTER (NOT REQUIRED)
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_loan_req.aspx' where id='F85127'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_Pt_Config.aspx' where id='F85139'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',brn='N' where id='F82569'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F82583'"); // ONLY ICON
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F85153'"); // ONLY ICON
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F82587'"); // ON HOLD
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F82588'"); // ON HOLD
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_loan_req.aspx' where id='F85126'");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "27/12/2019", "DEV_A", "W2028", "Start Date For Web Payrole", "N", "2");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F82574'"); // ON HOLD
        }

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT id as tname FROM fin_msys WHERE trim(id)='F10186C'", "TNAME");
        if (mhd != "0")
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set submenuid='fin10_e10' where id='F10186C'");
        }
        if (frm_cocd == "SPPI") fgen.execute_cmd(frm_qstr, frm_cocd, "update typegrp set id='^O' where id='M' and upper(trim(name)) like 'FOIL%' or upper(trim(name)) like '%FOIL%'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='fin45CR2_e1' where id='F45162'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Lead Action Master' where id='F45165'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Lead Category Master' where id='F45166'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Lead Source Master' where id='F45168'");


        ICO.add_icon(frm_qstr, "F70193", 3, "Native State Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F70194", 3, "Zone Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Department Master' where id='F85155'");

    }

    void pkgIcons(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DM0018");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0018','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0018", "DEV_A");

            mhd = "update FIN_MSYS set BNR='Y' where upper(text) like '%REQUEST%'";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set BRN='N' where BRN is null");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='Y' where prd is null");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='Y' where id like 'F78%'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='Y' where id like 'F79%'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in ('F10156','F10160','F15126','F15128','F15132','F15133')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15137','F15142','F15143','F15129')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F10228','F10229','F10230','F10233','F10234','F10235','F10236','F10237','F10222','F10223','F10224','F10225','F10226')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F50242','F50244','F50245','F70222','F25242', 'F25242','F70231','F70232','F70233','F70234','F70235','F70236','F70239')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15134')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('15303','F15309','F15310','F15311')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15137','F15142','F15143')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F10228','F10229','F10230','F10233','F10234','F10235','F10236','F10237','F10222','F10223','F10224','F10225','F10226')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id='F15133'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15137','F15142','F15143')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15134','F15304','F15305','F15306')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F15302','F15303','F15304','F15305', 'F15308','F15309','F15310','F15311','F15312','F15313','F15314','F15315','F15316', 'F15317','F15318','F40127','F40128','F40140','F40148','F40141','F40146')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F25134','F40127','F40128','F50222','F50223','F50224','F50225','F50226','F50227','F50228','F50231','F50232','F50233','F50234','F50235','F50236','F85147', 'F85148', 'F85149','F85150','F50240','F50241','F50250','F50251','F50255','F50256','F50257','F50258','F50264','F50264','F50128','F70141')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F25141','F40127','F40128','F50222','F50223','F50224','F50225','F50226','F50227','F50228','F50231','F50232','F50233','F50234','F50235','F50236','F85147', 'F85148', 'F85149','F85150','F50240','F50241','F50250','F50251','F50255','F50256','F50257','F50258','F50264','F50264','F50128')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F15302','F15303','F15304','F15305', 'F15308','F15309','F15310','F15311','F15312','F15313','F15314','F15315','F15316', 'F15317','F15318','F40127','F40128','F40140','F40148','F40141','F40146')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F25134','F40127','F40128','F50222','F50223','F50224','F50225','F50226','F50227','F50228','F50231','F50232','F50233','F50234','F50235','F50236','F85147', 'F85148', 'F85149','F85150','F50240','F50241','F50250','F50251','F50255','F50256','F50257','F50258','F50264','F50264','F50128','F70141')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F25141','F40127','F40128','F50222','F50223','F50224','F50225','F50226','F50227','F50228','F50231','F50232','F50233','F50234','F50235','F50236','F85147', 'F85148', 'F85149','F85150','F50240','F50241','F50250','F50251','F50255','F50256','F50257','F50258','F50264','F50264','F50128')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET PRD='N' WHERE ID in ('F70555','F40139','F40140')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in ('F10156','F10160','F15126','F15128','F15132','F15133','F15134','F70132','F70133','F70223','F70225')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15137','F15142','F15143','F15129','F15222','F15223','F70245','F70253','F70269')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F10228','F10229','F10230','F10233','F10234','F10235','F10236','F10237','F10222','F10223','F10224','F10225','F10226')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F15143','F50242','F50244','F50245','F25242', 'F25242','F70231','F70232','F70233','F70234','F70235','F70236','F70239')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F15302','F15303','F15304','F15305', 'F15308','F15309','F15310','F15311','F15312','F15313','F15314','F15315','F15316', 'F15317','F15318','F40127','F40128','F40140','F40148','F40141','F40146')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F25134','F40127','F40128','F50222','F50223','F50224','F50225','F50226','F50227','F50228','F50231','F50232','F50233','F50234','F50235','F50236','F85147', 'F85148', 'F85149','F85150','F50240','F50241','F50250','F50251','F50255','F50256','F50257','F50258','F50264','F50264','F50128','F70141')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F20132','F20127','F20121','F25141','F40127','F40128','F50222','F50223','F50224','F50225','F50226','F50227','F50228','F50231','F50232','F50233','F50234','F50235','F50236','F85147', 'F85148', 'F85149','F85150','F50240','F50241','F50250','F50251','F50255','F50256','F50257','F50258','F50264','F50264','F50128')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F70237','F70238','F70151')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id='F70410' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where ID in ('F25126','F10156')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set prd='N' WHERE ID IN ('F40128','F40129','F20205')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where id ='F70406'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='Y' where id IN ('F40127','F40346')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N', submenuid='fin70_e4' where id in ('F70137', 'F70228','F70252','F70229','F70230','F70148','F70149','F70241','F70242')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set visi='N' where ID in ('F10156')");
        }

        mhd = fgen.chk_RsysUpd("DM0019");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0019','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0019", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='N' where id in ('F30121','F30142','F30143','F30132')");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHER", "MFGDT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD MFGDT VARCHAR2(10) ");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHER", "EXPDT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD EXPDT VARCHAR2(10) ");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='IVCH_HIST'", "tname");
            if (mhd == "0" || mhd == "") { }
            else
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVCH_HIST", "MFGDT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD MFGDT VARCHAR2(10) ");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVCH_HIST", "EXPDT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST ADD EXPDT VARCHAR2(10) ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify MFGDT VARCHAR2(10) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCH_HIST modify EXPDT VARCHAR2(10) default '-'");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify MFGDT VARCHAR2(10) default '-' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify EXPDT VARCHAR2(10) default '-'");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "CURR_STAT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_ACT", "CURR_STAT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_ACT ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO  MODIFY TERMINAL VARCHAR2(50) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE mailbox2 MODIFY TERMINAL VARCHAR2(50) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE mailbox  MODIFY TERMINAL VARCHAR2(50) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_msys where id='F45141'");
            ICO.add_icon(frm_qstr, "F45141", 4, "Lead Mgmt Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_dbd_mgrid.aspx' where web_action='../tej-base/om_stk_Asys.aspx'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table WB_LEAD_act modify input_From varchar2(50) default '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N',visi='Y' where ID in ('F25152','F25156')");
        }

        if (frm_cocd == "SAGM")
        {
            ICO.add_icon(frm_qstr, "F05108", 3, "Performance MIS", 3, "../tej-base/om_mis_txt.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
        }

        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W2020", "Bom Compulsary for Sales Order", "N", "2");
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W0052", "Sales Order Booking from HO Only? ", "N", "-");
        if (frm_cocd == "SAGM")
        {
            ICO.add_icon(frm_qstr, "F05108", 3, "Performance MIS", 3, "../tej-base/om_mis_txt.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05110", 3, "Order Tracking Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F49000", 2, "Export Sales Orders", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "-", "fa-edit");

            //exp-imp register
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_EXP_IMP'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_EXP_IMP (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR2(6),VCHDATE DATE,ACODE VARCHAR2(6),ICODE VARCHAR2(10),SRNO CHAR(4), ENTRY_NO_BILL VARCHAR2(20),ENTRY_DT_BILL DATE,INVNO VARCHAR2(20),INVDATE DATE,MRRNUM VARCHAR2(6),MRRDATE DATE, CETSHNO VARCHAR2(10),PONUM VARCHAR2(6),PODATE DATE,POQTY NUMBER(20,3),QTY_REC NUMBER(20,3),COUNTRY VARCHAR2(50),DESP_DT DATE,FOREIGN_VAL NUMBER(20,3),AMT_INR NUMBER(20,3),CIFVAL NUMBER(20,3),INSUR_INR NUMBER(20,3),FREIGHT_INR_SB NUMBER(20,3),SHIP_BILLNO VARCHAR2(60),SHIP_BILLDT varchar2(10),SHIP_LEODT varchar2(10),SHIP_LINES VARCHAR2(80),SHIP_LINES_CHG VARCHAR2(10),CONT_NO VARCHAR2(20),IMP_MODE VARCHAR2(30),PORT_CLEARANCE VARCHAR2(20),EXCH_RT NUMBER(20,3),FOB NUMBER(20,3),IMP_EXP_UNDER VARCHAR2(50),DUTY VARCHAR2(20),IGST_PAID NUMBER(20,3),IGST_REC_DT varchar2(10),ADV_LICNO VARCHAR2(60),DBK_CLAIMED_AMT NUMBER(20,3),DBK_REC_DT varchar2(10),CHA VARCHAR2(60),BANK_REF VARCHAR2(30),PYMT_DUE varchar2(10),DELV_DT varchar2(10),REMARKS VARCHAR2(200),IMPORT_TERM VARCHAR2(30),bill_fwd varchar2(10),PYMT_DATE varchar2(10),EXHG_BRC NUMBER(20,3),FREIGHT_INR_SL NUMBER(20,3),INS_PREM NUMBER(20,3),COMM VARCHAR2(10),FOB_INR NUMBER(20,3),FOB_FOREIGN NUMBER(20,3),REMARKS2 VARCHAR2(100),cscode varchar2(6),curr_rate number(20,3),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE)");

            ICO.add_icon(frm_qstr, "F49181", 4, "Export Bill details", 3, "../tej-base/om_exp_reg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49185", 4, "Import Bill details", 3, "../tej-base/om_imp_reg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
        }


        mhd = fgen.chk_RsysUpd("DM0024");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0024','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0024", "DEV_A");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "DESPATCH", "GTAX1");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE DESPATCH add GTAX1 number(10,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "DESPATCH", "GTAX1");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE DESPATCH add GTAX2 number(10,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "DESPATCH", "ORDLINE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE DESPATCH add ORDLINE varchar2(6) DEFAULT '-'");



            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SYS_CONFIG", "OBJ_Caption_Reg");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SYS_CONFIG ADD OBJ_Caption_Reg VARCHAR2(60) DEFAULT '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ivchctrl modify INATURE VARCHAR2(60) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Accounts Master Options' where id='F70171'");


            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0077", "A/c Code for CGST Payable ? ", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0078", "A/c Code for SGST Payable ? ", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0079", "A/c Code for IGST Payable ? ", "N", "-");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0080", "A/c Code for CGST Receivable ? ", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0081", "A/c Code for SGST Receivable ? ", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0082", "A/c Code for IGST Receivable ? ", "N", "-");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0100", "Apply Web Control Panel (W___) ", "N", "-");

            ICO.add_icon(frm_qstr, "F99153", 3, "Shifts Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");


            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text='Company Level Masters' where id='F99150'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,' Activity',' Transactions') where text like '%Activity%' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,' Checklists',' Reports (Searchable)') where mlevel=2 and TExt like '%Checklist%' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,'(Searchable)s','(Searchable)') where mlevel=2 and TExt like '%(Searchable)%' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,'Matl ','Material ') where text like '%Matl %' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,' Config',' Settings') where text like '% Config%' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,' Settingsurationuration',' Settings') where text like '% Settingsurationuration%' ");


            ICO.add_icon(frm_qstr, "F99155", 3, "Upload Items Sub Groups", 3, "../tej-base/om_multi_item.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99157", 3, "Upload Items Masters", 3, "../tej-base/om_multi_item.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99159", 3, "Upload Accounts Masters", 3, "../tej-base/om_multi_account.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99170", 3, "Upload Bill wise AR/AP Lists ", 3, "../tej-base/om_multi_bill.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70192", 3, "States Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70177", 3, "TAX Rates Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text='GSN/VAT/HSN Tax Rates Master' where id='F70177'");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/11/2019", "DEV_A", "W2027", "Member GCC Country", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/11/2019", "DEV_A", "W0065", "Copy Facility in PO Creation", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/11/2019", "DEV_A", "W0066", "PR Button in PO Creation", "N", "2");
        }

        mhd = fgen.chk_RsysUpd("DM0025");
        if (mhd == "0" || mhd == "")
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text='Commercial Invoice (Exp.)' where id='F55106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,'Exp.','Export ') where text like 'Exp.%' ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text=replace(text,'Dom.','Domestic ') where text like 'Dom.%' ");

            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table dsk_Config modify obj_sql varchar2(1750) default '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table dsk_Config modify obj_sql2 varchar2(1750) default '-'");

            ICO.add_icon(frm_qstr, "F45167", 4, "Contacts Status Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "Y");

            ///
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select TNAME from TAB where TNAME='WB_CORRCST_FLUTEM' ", "TNAME");
            if (mhd == "0" || mhd == "")
            { }
            else
            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum from WB_CORRCST_FLUTEM where vchnum='000009' ", "vchnum");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^7','000001',TO_DATE('27/03/2019','DD/MM/YYYY'),'H+0.5W+35','2L+2W+60','-',0,0.85,'200','HALF SLOTTED CONTAINER (HSC)','A = C*D','B200.JPG','D:/FINDEV/tej-WFIN/tej-BASE/UPLOAD/B200.JPG','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^7','000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'H+W+35','2L+2W+60','-',0,6.75,'201','REGULAR SLOTTED CONTAINER (RSC)','A = C*D','B201.JPG','C:/TEJ_ERP/UPLOAD/27_03_2019~B201.JPG','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'B','3','-',1.35,1.35,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000003',TO_DATE('27/03/2019','DD/MM/YYYY'),'C','4','-',1.45,1.45,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^7','000003',TO_DATE('27/03/2019','DD/MM/YYYY'),'H+2W+35','2L+2W+60','-',0,7.75,'203','FULL OVERLAP SLOTTED CONTAINER (FOL)','A = C*D','B203.JPG','C:/TEJ_ERP/UPLOAD/27_03_2019~B203.JPG','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000004',TO_DATE('27/03/2019','DD/MM/YYYY'),'A','4.5','-',1.5,1.5,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^7','000004',TO_DATE('27/03/2019','DD/MM/YYYY'),'2H+0.5W+35','2L+2W+60','-',0,1.39,'225','FULL BOTTOM FILE BOX, HAMPER STYLE.','A = C*D','B225.JPG','D:/FINDEV/tej-WFIN/tej-BASE/UPLOAD/B225.JPG','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000005',TO_DATE('27/03/2019','DD/MM/YYYY'),'BB','6','-',1.35,1.35,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000006',TO_DATE('27/03/2019','DD/MM/YYYY'),'BC','7','-',1.35,1.35,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000007',TO_DATE('27/03/2019','DD/MM/YYYY'),'BA','7.5','-',1.35,1.5,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000008',TO_DATE('27/03/2019','DD/MM/YYYY'),'CA','8.5','-',1.45,1.5,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_FLUTEM VALUES ('00','^4','000009',TO_DATE('27/03/2019','DD/MM/YYYY'),'BC','7','-',1.35,1.45,'-','-','-','-','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'))");
                }
                //mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum from where vchnum='000009' ", "vchnum");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000003',to_date('27/03/2019','dd/mm/yyyy'),'-','20','29.25','28.25','6.5','8.25','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),7.75)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000004',to_date('27/03/2019','dd/mm/yyyy'),'-','22','31.75','29.5','7.5','9.25','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),8.75)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000005',to_date('27/03/2019','dd/mm/yyyy'),'-','24','33','31','8.25','9.25','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),9.75)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000007',to_date('27/03/2019','dd/mm/yyyy'),'-','35','36.75','34.5','10','11.75','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),10.75)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000008',to_date('27/03/2019','dd/mm/yyyy'),'-','45','0','0','11','12','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),0)");
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','^8','000001',to_date('27/03/2019','dd/mm/yyyy'),'0200','0','0','','0','0.847','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),0)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000001',to_date('27/03/2019','dd/mm/yyyy'),'-','16','25','23.5','4.5','5.5','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),5.5)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000006',to_date('27/03/2019','dd/mm/yyyy'),'-','28','34.75','33','9.75','10.75','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),10)");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_RCTM VALUES ('00','B','000002',to_date('27/03/2019','dd/mm/yyyy'),'-','18','27','26.5','5.5','7.5','-','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),6.75)");
                }
            }
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select TNAME from TAB where TNAME='WB_CORRCST_LAYER' ", "TNAME");
            if (mhd == "0" || mhd == "")
            { }
            else
            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum from WB_CORRCST_LAYER where vchnum='00000003' ", "vchnum");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('01','01','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'100','18','2','0.55','0.74','0.39','7.5','5.5','5.5','1.35','27','26.5','26.5','0.979','0.39','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','','0.9','1.64','FLUTE 1')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('01','02','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'100','16','2','0.45','0.45','0.26','5.5','4.5','4.5','1','25','23.5','23.5','0.979','0.26','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','','0.9','1.64','LINER 1')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('01','03','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'100','18','2','0.55','0','0','7.5','5.5','5.5','0','27','26.5','26.5','0.979','0','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','','0.9','1.64','FLUTE 2')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('01','04','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'100','18','0.45','0.55','0','0','7.5','5.5','5.5','0','27','26.5','26.5','0.979','0','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','','0.9','1.64','LINER 2')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('02','00','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'100','16','1','0.55','0.55','0.56','5.5','4.5','5.5','1','25','23.5','25','2.064','0.56','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','','2.02','2.38','TOP PLY')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('02','01','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'100','18','1','0.75','1.01','0.81','7.5','5.5','7.5','1.35','27','23.5','27','2.064','0.81','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','','2.02','2.38','FLUTE 1')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('02','02','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'100','20','1','0.82','0.82','0.65','8.25','6.5','8.25','1','29.25','23.5','29.25','2.064','0.65','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','','2.02','2.38','LINER 1')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('02','03','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'100','28','2','0.98','0','0','10.75','9.75','9.75','0','34.75','23.5','23.5','2.064','0','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','','2.02','2.38','FLUTE 2')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('02','04','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'100','20','0.55','0.65','0','0','8.25','6.5','6.5','0','29.25','23.5','23.5','2.064','0','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','','2.02','2.38','LINER 2')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('03','00','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'250','22','1','2.31','2.31','282.41','9.25','7.5','9.25','1','31.75','23.5','31.75','353.584','282.41','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','','1059.05','8.67','TOP PLY')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('03','01','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'250','22','1','2.31','3.12','381.26','9.25','7.5','9.25','1.35','31.75','23.5','31.75','353.584','381.26','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','','1059.05','8.67','FLUTE 1')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('03','02','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'350','22','1','3.24','3.24','395.38','9.25','7.5','9.25','1','31.75','23.5','31.75','353.584','395.38','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','','1059.05','8.67','LINER 1')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('03','03','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'400','28','2','3.9','0','0','10.75','9.75','9.75','0','34.75','23.5','23.5','353.584','0','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','','1059.05','8.67','FLUTE 2')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('03','04','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'500','35','2.31','5','0','0','11.75','10','10','0','36.75','23.5','23.5','353.584','0','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','','1059.05','8.67','LINER 2')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_LAYER VALUES ('01','00','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'100','16','2','0.45','0.45','0.26','5.5','4.5','4.5','1','25','23.5','23.5','0.979','0.26','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','','0.9','1.64','TOP PLY')");
                }
            }
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select TNAME from TAB where TNAME='WB_CORRCST_CONVC' ", "TNAME");
            if (mhd == "0" || mhd == "")
            { }
            else
            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum from WB_CORRCST_CONVC where vchnum='00000003' ", "vchnum");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','00','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'34','1','STARCH GUM','0.06','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','01','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'170','1','PVA GUM','0.04','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','02','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'7.5','1','POWER','0.01','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','03','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'6','1','FUEL','0.03','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','04','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'70','0','STITCHING PINS','0','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','05','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'150','1','PRINTING INK','0','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','06','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'1.75','1','LABOR','0.07','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','07','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'0.5','1','ADMINISTRATIVE','0.02','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','08','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'0.5','1','TRANSPORTATION','0.02','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('01','09','',TO_DATE('07/09/2019','DD/MM/YYYY'),'00000001',TO_DATE('07/09/2019','DD/MM/YYYY'),'0.1','1','OTHER MATERIALS','0','FINTEAM',TO_DATE('07/09/2019','DD/MM/YYYY'),'',TO_DATE('07/09/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','01','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'170','1','PVA GUM','0.04','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','02','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'7.5','1','POWER','0.03','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','03','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'6','1','FUEL','0.06','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','04','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'70','0','STITCHING PINS','0','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','05','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'150','1','PRINTING INK','0.01','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','06','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'1.75','1','LABOR','0.16','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','07','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'0.5','1','ADMINISTRATIVE','0.04','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','08','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'0.5','1','TRANSPORTATION','0.04','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','09','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'0.1','1','OTHER MATERIALS','0.01','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','00','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'34','1','STARCH GUM','19.36','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('02','00','',TO_DATE('27/03/2019','DD/MM/YYYY'),'00000002',TO_DATE('27/03/2019','DD/MM/YYYY'),'34','1','STARCH GUM','0.12','FINTEAM',TO_DATE('27/03/2019','DD/MM/YYYY'),'',TO_DATE('27/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','01','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'170','1','PVA GUM','0.42','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','02','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'7.5','1','POWER','17.15','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','03','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'6','1','FUEL','35.07','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','04','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'70','0','STITCHING PINS','0','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','05','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'150','1','PRINTING INK','5.72','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','06','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'1.75','1','LABOR','88.95','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','07','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'0.5','1','ADMINISTRATIVE','25.41','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','08','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'0.5','1','TRANSPORTATION','25.41','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO WB_CORRCST_CONVC VALUES ('03','09','',TO_DATE('28/03/2019','DD/MM/YYYY'),'00000003',TO_DATE('28/03/2019','DD/MM/YYYY'),'0.1','1','OTHER MATERIALS','5.08','FINTEAM',TO_DATE('28/03/2019','DD/MM/YYYY'),'',TO_DATE('28/03/2019','DD/MM/YYYY'),'','')");
                }
            }
        }
        ///
    }

    public void Upd_Nov(string frm_qstr, string frm_cocd)
    {
    }

    public void Upd_Dec(string frm_qstr, string frm_cocd)
    {



    }

    public void Upd_Apr(string frm_qstr, string frm_cocd)
    {
        MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP");

        mhd = fgen.chk_RsysUpd("DM0025");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0025','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0025", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set form='fin45_a1' where id  in ('F10049','F10050','F10051','F10052','F10052S','F10053')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set submenuid='fin45_ec' where id  in ('F10049','F10050','F10051','F10052','F10052S','F10053')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set submenu='-' where id  in ('F10050','F10051','F10052','F10052S','F10053')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set form='fin05_a1' where id  in ('F05365','F05366','F05367','F05368','F05369','F05370')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set submenuid='fin05_e11' where id  in ('F05365','F05366','F05367','F05368','F05369','F05370')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text='Finish/SemiFinish Item Master' where id='F10116'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text='Production Processes Master' where id='F10126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set text='General Item Master' where id='F10111'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set form='fin45_a1' where id  in ('F10550','F10551','F10552','F10553','F10554','F10555','F10556')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set submenuid='fin45_ec' where id  in ('F10550','F10551','F10552','F10553','F10554','F10555','F10556')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set form='fin45_a1' where id  in ('F10249','F10250','F10280','F10281','F10282')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "Update fin_msys set submenuid='fin45_ep' where id  in ('F10249','F10250','F10280','F10281','F10282')");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='A/c Master/Vendor Master/Customer Master' where id='F70172'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Sub Groups/Item Sub Groups' where id='F10106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Item Master/Raw Material/BOP' where id='F10111'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Finished Goods/Semi Finish/SFG/FG' where id='F10116'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='BOM/Recipe/Formula' where id='F10131'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PR/Indent/Purchase Requisition' where id='F15101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PO Creation/PO Entry/Vendor PO' where id='F15106'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PR Approval/Approve PR/Approve Indent' where id='F15162'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PO Approval/Approve PO/Approve Purchase Order' where id='F15166'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Vendor Schedule/31 Day Purchase Schedule' where id='F15111'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Gate Entry' where id='F20101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='MRR Entry/GRN Entry/Store Receipt' where id='F25101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Inward Inspection/Quality Check' where id='F30141'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Store Indent/Store Request' where id='F39201'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Store Indent/Store Request' where id='F40201'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Store Issue/Issue Within Factory' where id='F25111'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Return Request/Line Return' where id='F39206'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Return Request/Line Return' where id='F40206'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Store Return/Return Within Factory' where id='F25116'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Challan Entry/Job Work/Outside Jobwork' where id='F25106'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Sales order/Domestic Order/SO Entry' where id='F47106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Approval of SO/SO Approval' where id='F47127'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Approval of PO/PO Approval' where id='F15166'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Customer Schedule/31 Day Despatch Schedule' where id='F47111'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Production Finishing Entry/Basic Production' where id='F39119'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Sales Invoice/Sales Bill' where id='F50101'");

            ICO.add_icon(frm_qstr, "F99154", 3, "Production Types", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set id='F70125' where id='F25250'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set id='F85170' where id='F85146'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Payroll/Salary Module' where id='F85000'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Increment Entry' where id='F85103'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Full & Final Entry' where id='F85156'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set id='F85109' where id='F85103'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_mrsys set id='F70125' where id='F25250'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_mrsys set id='F85170' where id='F85146'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_mrsys set id='F85109' where id='F85103'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Date Wise Sales Achieved',search_key='Date Wise Sales (Drill Down Mode)' where id='F05101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Schedule Vs Despatch Achieved',search_key='Customer Wise Schedule Vs Despatch' where id='F05106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Plant Wise Sales Achieved',search_key='Compare Plant wise Sale Value Performance' where id='F05111'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Customer wise Monthly Sales Tracking',search_key='Customer,Month Wise Values and Charts' where id='F05112'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Customer wise Day Wise Sales Tracking',search_key='Customer,Day Wise Values and Charts' where id='F05116'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Plant wise Monthly Sales Tracking',search_key='Plant,Month Wise Values and Charts' where id='F05113'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Plant wise Day Wise Sales Tracking',search_key='Plant,Day Wise Values and Charts' where id='F05118'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Item wise Monthly Sales Qty Tracking',search_key='Item,Month Wise Quantity and Charts' where id='F05114'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Item wise Day Wise Sales Qty Tracking',search_key='Item,Day Wise Qty and Charts' where id='F05119'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Item wise Monthly Sales Value Tracking',search_key='Item,Month Wise Value and Charts' where id='F05120'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Item wise Day Wise Sales Value Tracking',search_key='Item,Day Wise Values and Charts' where id='F05117'");

        }

        mhd = fgen.chk_RsysUpd("DM0026");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0026','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0026", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Debtors Ageing Report',search_key='Debtors Ageing 30/60/90/180/over 180 Days' where id='F05126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Creditors Ageing Report',search_key='Creditors Ageing 30/60/90/180/over 180 Days' where id='F05127'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Item Main Categories' where id='F10101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Units of Measurement/UOM Master' where id='F10121'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Master for Store Locations' where id='F10125'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Master for Production Processes' where id='F10126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='BOM / Recipe / Formula' where id='F10131'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Child Parts linked to Main Product' where id='F10132'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Routing / Item Wise Stages ' where id='F10133'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Item Rate List of Approved Vendors' where id='F15116'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Item Sub Group Level Purchase Budget' where id='F15117'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Standard Specs for Checking MRR/GRN/SRV' where id='F30101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Standard Specs for Checking In Process Goods' where id='F30106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Standard Specs for Checking FGS / PDIR' where id='F30108'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='QA/Test Report of MRR/GRN/SRV' where id='F30111'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='QA/Test Report of In Process Goods' where id='F30112'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='QA/Test Report of FGS / PDIR' where id='F30113'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Gate In Entry/Material Received at Gate' where id='F20101'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_msys where id='F25250'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PR Checking/Check PR/Check Indent' where id='F15161'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PR Approval/Approve PR/Approve Indent' where id='F15162'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PO Checking/Check PO/Check Purchase Order' where id='F15165'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='PO Approval/Approve PO/Approve Purchase Order' where id='F15166'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Schedule Approval/Approve Purch Schedule' where id='F15171'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Approve Vendor Price List' where id='F15176'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='12 Month Trend of Sales' where id='F05133'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='12 Month Trend of Purchase' where id='F05134'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Purchase Order Types',search_key='Master of PO Types' where id='F15201'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Close Purchase Request/Indents' where id='F15210'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Close Purchase Orders' where id='F15211'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Set Monetary Limits For PO Approval' where id='F15212'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Gate Out Entry/Material Out From Gate' where id='F20106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Gate Reports (Printable)' where id='F20131'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Gate Movement Analysis' where id='F20140'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Visitor Inward Entry' where id='F20234'");
        }

        mhd = fgen.chk_RsysUpd("DM0027");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0027','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0027", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Maint. Planning',search_key='Entry of Maintenance Plan' where id='F75101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Maint. Planned Action',search_key='Maint. Done Against Plan' where id='F75106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Maint. Complaint Action',search_key='Maint. Done Against Complaint' where id='F75111'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Planned Maint. Logs',search_key='Checklist of Maint. Done Agst Plan' where id='F75126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Complaint Maint. Logs',search_key='Checklist of Maint. Done Agst Complaint' where id='F75127'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Maintenance Groups',search_key='Categories of Machinery by Nature/Type' where id='F75162'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Maintenance Machines',search_key='Master Entry of Machinery,Category Wise' where id='F75165'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Purchase Reports (Printable)' where id='F15131'");

            ICO.add_icon(frm_qstr, "F05102", 3, "Month Wise Sales", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Month Wise Sales Achieved',search_key='Month Wise Sales (Drill Down Mode)' where id='F05102'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Quality Reports (Printable)' where id='F30131'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Searchable Report for Gate Inward Entries' where id='F20121'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Searchable Report for Gate Outward Entries' where id='F20126'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Searchable Report for Pending Purchase orders' where id='F20127'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Searchable Report for Pending Returnable Material' where id='F20128'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Vendor,Day Wise Qty Received and Charts' where id='F05151'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Vendor,Month Wise Qty Received and Charts' where id='F05152'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Plant,Day Wise Qty Received and Charts' where id='F05153'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Plant,Month Wise Qty Received and Charts' where id='F05154'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Item,Day Wise Qty Received and Charts' where id='F05155'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Item,Month Wise Qty Received and Charts' where id='F05156'");


            if (frm_cocd == "NEOP" || frm_cocd == "SEL")
            {

            }
            else
            {
                //customer care 
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F10049','F10050','F10051','F10052','F10052S','F10053')");
                //Expense Mgt 
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F10249','F10250','F10280')");

            }

        }

        mhd = fgen.chk_RsysUpd("DM0033");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0028','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0033", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Master Sales Order (Domestic)',search_key='Master SO for Setting Customers Rate List' where id='F47101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Supply Sales Order (Domestic)',search_key='Supply SO for Dispatch Quantity/Open Qty' where id='F47106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Day Wise Sales Schedule (Domestic)',search_key='Entry of 31 Day Customer Sales Schedule' where id='F47111'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Sales Projection For a Month',search_key='Entry of Sales Projection/Sales Target' where id='F47116'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='05) Sales Budget (12 Month)',search_key='Entry of Sales Budget for 12 Months' where id='F47112'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='06) Sales Schedule Entry (Date Based)',search_key='Entry of Sales Schedule for Specific Dates' where id='F47111D'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Domestic Orders Approvals' where id='F47121'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Approve Master S.O (Domestic)',search_key='Approval of Master Sales order' where id='F47127M'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Check Sales Order (Domestic)',search_key='SO Checking/Check a Supply Order' where id='F47126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Approve Sales Order (Domestic)',search_key='SO Approval/Approve a Supply Order' where id='F47127'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Approve Sales Schedule (Domestic)',search_key='Schedule Approval/Approve a Sale Schedule' where id='F47128'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Master S.O. Checklists(Domestic)',search_key='Searchable Report of Master Sales orders' where id='F47132'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Supply S.O. Checklists(Domestic)',search_key='Searchable Report of Domestic Sales orders' where id='F47133'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Supply Sch. Checklists(Domestic)',search_key='Searchable Report of Domestic Sales Schedule' where id='F47134'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Pending Order Checklist(Domestic)',search_key='Searchable Report of Domestic Sales Orders Vs Sales' where id='F47136'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='05) Schedule Vs Dispatch  (Domestic)',search_key='Searchable Report of Domestic Sales Sche. Vs Sales' where id='F47135'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Domestic Order Reports (Searchable)' where id='F47131'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Domestic Order Reports (Printable)' where id='F47140'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Order Vs Dispatch(Domestic)',search_key='Print Report of Order Vs Dispatch' where id='F47222'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Schedule Vs Dispatch(Domestic)',search_key='Print Report of Schedule Vs Dispatch' where id='F47223'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Close Sales Order(Domestic)',search_key='Short Close/Close Sales order(Domestic)' where id='F47162'");



            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Customer Complaint Entry',search_key='Entry of Customer Complaints/Query Received' where id='F61101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Customer Complaint Action',search_key='Entry of Actions Taken on Customer Complaints' where id='F61106'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Complaint Logging Report',search_key='Searchable Report of Customer Complaints/Query Received' where id='F61121'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Complaint Action Taken Report',search_key='Searchable Report of Complaints Action Taken' where id='F61126'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Complaint Action Status Report',search_key='Customer Complaints/Query Status Report (Timeline)' where id='F61131'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Customer Complaint Dashboard',search_key='Dashboard View of Customer Complaints Received' where id='F61141'");

            ICO.add_icon(frm_qstr, "F61108", 4, "03) Customer Complaint Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F61110", 4, "04) Master : Complaint Reasons", 3, "../tej-base/om_tgpop_mst.aspx", "Types of Customer Complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F61111", 4, "05) Master : Complaint Analysis", 3, "../tej-base/om_tgpop_mst.aspx", "Types of Complaint Analysis", "-", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F61112", 4, "06) Master : Complaint Catagories", 3, "../tej-base/om_tgpop_mst.aspx", "Type of Urgency/Priority", "-", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F61133", 4, "04) Customer Wise 12 Month Complaints", 3, "../tej-base/om_MIS_grid.aspx", "12 Month Data of Customer Wise Complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F61134", 4, "05) Customer Wise 31 Day Complaints", 3, "../tej-base/om_MIS_grid.aspx", "31 Day Data of Customer Wise Complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F61136", 4, "06) Reason Wise 12 Month Complaints", 3, "../tej-base/om_MIS_grid.aspx", "12 Month Data of Reason Wise Complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F61137", 4, "07) Reason Wise 31 Day Complaints", 3, "../tej-base/om_MIS_grid.aspx", "31 Day Data of Reason Wise Complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "ACODE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table WB_CCM_LOG add Acode char(10) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "ICODE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table WB_CCM_LOG add Icode char(10) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "INV_REF");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table WB_CCM_LOG add INV_REF varchar2(30) default '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CCM_act MODIFY ACT_MODE VARCHAR2(30) DEFAULT '-'");

        }


        mhd = fgen.chk_RsysUpd("DM0029");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0029','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0029", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Customer Complaints Analysis' where id='F61140'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Customer Complaint Dashboard',search_key='Dashboard View of Customer Complaints Received' where id='F61141'");
            ICO.add_icon(frm_qstr, "F61143", 4, "02) Customer wise Complaint Summary", 3, "../tej-base/om_view_mis.aspx", "Searchable Summary of Customer Wise complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F61145", 4, "03) Reasons wise Complaint Summary", 3, "../tej-base/om_view_mis.aspx", "Searchable Summary of Reason Wise complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F61147", 4, "04) Product wise Complaint Summary", 3, "../tej-base/om_view_mis.aspx", "Searchable Summary of Product Wise complaints", "-", "fin61_e1", "fin45_a1", "fin61CC_e3", "fa-edit");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='Y' where ID in ('F61143','F61145','F61147')");

            // to be made industry wise
            ICO.add_icon(frm_qstr, "F10176", 2, "Costing Module : Corrugation ", 3, "-", "-", "Y", "fin10_e7", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10177", 2, "Costing Module : Printed Cartons", 3, "-", "-", "Y", "fin10_e8", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10178", 2, "Costing Module : Labels ", 3, "-", "-", "Y", "fin10_e9", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10179", 2, "Costing Module : Flexible", 3, "-", "-", "Y", "fin10_e10", "fin10_a1", "-", "fa-edit");
            // to be made industry wise

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set brn='N',prd='N' where ID in ('F10176','F10177','F10178','F10179')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin10_e7' where ID in ('F10185','F10144','F10145','F10146','F10147','F10148','F10149','F10150')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin10_e8' where ID in ('F10185A','F10185B')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin10_e9' where ID in ('F10200','F10187','F10188','F10193V','F10199','F10201','F10202','F10203','F10204','F10205','F10206','F10207')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin10_e10' where ID in ('F10185C')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Costing Module : General' where id='F10181'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Printout of Gate Inward Register' where id='F20132'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Printout of Gate Outward Register' where id='F20133'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='MRR Report / Searchable Material Inward Report' where id='F25126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Challan Report / Searchable Material Outward Report' where id='F25127'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Issue Report / Searchable Material Issue Report' where id='F25128'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Return Report / Searchable Material Return Report' where id='F25129'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Gate Entry Done, MRR Pending' where id='F25138'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Inventory Reports (Printable)' where id='F25140'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Documents Management System' where id='F25370'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Upload MRR/GRN Linked Docs',search_key='Upload Scanned Invoices/TC Linked to MRR' where id='F25371'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Approve MRR/GRN Linked Docs',search_key='Approve Scanned Invoices/TC Linked to MRR' where id='F25372'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='View MRR/GRN Linked Docs',search_key='View Scanned Invoices/TC Linked to MRR' where id='F25373'");




            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Leads Entry Register',search_key='Register of New Leads' where id='F45121'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Leads Followup Register',search_key='Register of Action Taken on Leads' where id='F45126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Leads status Report ',search_key='Leads Status Review (Timeline)' where id='F45131'");
        }
        mhd = fgen.chk_RsysUpd("DM0030");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0030','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0030", "DEV_A");


            ICO.add_icon(frm_qstr, "F45109", 4, "04) Customer Quotation", 3, "../tej-base/om_so_entry.aspx", "Create Quotation for Customer", "-", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F45110", 4, "05) Quotation Approval", 3, "../tej-base/om_appr.aspx", "Approve Quotation for Customer", "-", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit", "N", "N");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Invoice Entry (Domestic)',search_key='Create Sales Invoice/Sales Bill' where id='F50101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Proforma Invoice Entry (Domestic)',search_key='Create Proforma Invoice Domestic' where id='F50106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Dispatch Advice (Domestic)',search_key='Create Delivery Note/Create DA' where id='F50111'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Production Receipt',search_key='Production Finish Entry' where id='F50114'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='05) Generate E Way Bill',search_key='Make E-Way Bill JSON/Auto Mode' where id='F50113'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Supply Sales Order (Exports)',search_key='Supply Order for Exports' where id='F49106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Proforma Sales Invoice (Exports)',search_key='Proforma Invoice for Exports' where id='F49101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Day Wise Sales Schedule (Exports)',search_key='Entry of 31 Day Export Sales Schedule' where id='F49111'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Check Sales Order (Exports)',search_key='Check Export Supply Order' where id='F49126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Approve Sales Order (Exports)',search_key='Approve Export Supply Order' where id='F49127'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Check Proforma Invoice (Exports)',search_key='Check Export Proforma Invoice' where id='F49129'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Approve Proforma Invoice (Exports)',search_key='Approve Export Proforma Invoice' where id='F49130'");
            if (frm_cocd == "STLC")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from FIN_MSYS where ID in ('F10185','F10181')");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Chart of Accounts : A/c Groups' where id='F70173'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Chart of Accounts : A/c Schedules' where id='F70174'");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0083", "A/c Code for VAT Payable ? ", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0084", "A/c Code for VAT Receivable ? ", "N", "-");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "30/10/2019", "DEV_A", "W0090", "7 Digit A/c Code in Web ? ", "N", "-");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table fin_rsys_opt_pw modify opt_text varchar2(200) default '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table despatch modify acode varchar2(10) default '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table despatch modify freight varchar2(30) default '-'");

            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL1") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col1 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL2") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col2 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL3") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col3 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL4") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col4 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL5") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col5 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL6") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col6 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL7") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col7 number(12,2)");
            if (fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "COL8") == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp add col8 number(12,2)");

            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col1 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col2 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col3 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col4 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col5 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col6 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col7 number(12,2) default 0");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table ivoucherp modify col8 number(12,2) default 0");
        }

        mhd = fgen.chk_RsysUpd("DM0031");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0031','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0031", "DEV_A");

            mhd = "update fin_msys set web_action='../tej-base/om_pinv_entry.aspx' where id='F70116'";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PV_HEAD'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_PV_HEAD as (select * from sale where 1=2)");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PV_DTL'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_PV_DTL as (select * from ivoucher where 1=2)");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Accounts Transactions',search_key='Receipts,Payments,Journal,Purchase Vouchers' where id='F70100'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Receipts/Collection' where id='F70101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Payments/Remittance' where id='F70106'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Journal Entry' where id='F70111'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Purchase/Bill Passing' where id='F70116'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Month Wise Budgets',search_key='Set up Ledger Wise Monthly Budgets' where id='F70119'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Cheque Printing',search_key='Print Cheques For Payments' where id='F70555'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Receipt Voucher Checklist',search_key='Searchable Receipt Report' where id='F70126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Payment Voucher Checklist',search_key='Searchable Payment Report' where id='F70127'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Journal Voucher Checklist',search_key='Searchable Journal Report' where id='F70128'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Purchase Voucher Checklist',search_key='Searchable Purchase Report' where id='F70129'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Detailed Statement',search_key='Statement of Account/Ledger with Drill Down' where id='F70556'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Voucher Attachments',search_key='Attach Documents To Vouchers' where id='F70370'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Upload Voucher Attachments',search_key='Option to Attach Documents to Vouchers' where id='F70371'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='View Voucher Attachments',search_key='Option to View Attached Documents to Vouchers' where id='F70373'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Finance/Accounts Module' where id='F70000'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_vch_upload.aspx' where web_Action='../tej-base/vch_upl.aspx'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_vch_view.aspx' where web_Action='../tej-base/vch_vw.aspx'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Fixed Assets , Depreciation Record' where id='F70401'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Accounts Day Books' where id='F70121'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='-',submenuid='fin70_e2',mlevel=3 where id in ('F70231','F70232','F70233','F70234','F70235','F70236') ");

            ICO.add_icon(frm_qstr, "F70130", 3, "Sales Vouchers List", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "N");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Receipt Vouchers List',search_key='Searchable Receipt Report' where id='F70126'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Payment Vouchers List',search_key='Searchable Payment Report' where id='F70127'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Journal Vouchers List',search_key='Searchable Journal Report' where id='F70128'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Purchase Vouchers List',search_key='Searchable Purchase Report' where id='F70129'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Sales Voucher List',search_key='Searchable Sales Report' where id='F70130'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Day Book : Receipt Vouchers',search_key='Print Receipt Daybook' where id='F70231'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Day Book : Payment Vouchers',search_key='Print Payment Daybook' where id='F70232'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Day Book : Journal Vouchers',search_key='Print Journal Daybook' where id='F70233'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Day Book : Purchase Vouchers',search_key='Print Purchase Daybook' where id='F70234'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',text='Day Book : Sales Vouchers',search_key='Print Sales Daybook' where id='F70235'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',search_key='Master : Types of Vouchers' where id='F70176'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',search_key='Master : VAT/GST Rates' where id='F70177'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',search_key='Master : District Names' where id='F70182'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',search_key='Master : Country Names' where id='F70183'");




            if (MV_CLIENT_GRP == "SG_TYPE")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F70118','F70120','F70124','F70124','P70099','P70099a','P70099h','P70106C','P70106D','W90000')");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F70237','F70238','F70151')");
        }

        mhd = fgen.chk_RsysUpd("DM0032");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0032','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0032", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set PRD='N',search_key='Master : State Names' where id='F70192'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_tgpop_mst.aspx',VISI='Y',text='Continent Master',search_key='Master : Continent Names' where id='F70184'");
            ICO.add_icon(frm_qstr, "F70108", 3, "Debit Note Entry", 3, "../tej-base/om_pinv_entry.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70110", 3, "Credit Note Entry", 3, "../tej-base/om_pinv_entry.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70112", 3, "Service Bill Entry", 3, "../tej-base/om_pinv_entry.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70183", 3, "Country Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_pinv_entry.aspx',VISI='Y',search_key='Create a Debit Note' where id='F70108'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_pinv_entry.aspx',VISI='Y',search_key='Create a Credit Note' where id='F70110'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set prd='N' where id in('F70139','F70180')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set form='fin99_a1',submenuid='fin99_e4' where id in('F70181','F70185','F70186','F70187')");

            ICO.add_icon(frm_qstr, "F70162", 2, "Accounts Final Results", 3, "-", "-", "Y", "fin70_e8", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70650", 3, "Review Trial Balance", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70652", 3, "Review Expense Trend", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70654", 3, "Review PNL Account", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70656", 3, "Review Balance Sheet", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70680", 3, "Review Cost Centre Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70164", 2, "Accounts Receivable/Payable", 3, "-", "-", "Y", "fin70_e9", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70600", 3, "Review Receivable Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70602", 3, "Review Receivable Ageing", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70604", 3, "Review Payable Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70606", 3, "Review Payable Ageing", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e9", "fin70_a1", "-", "fa-edit", "N", "N");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Debtors Outstanding Report (Bill Wise)',search_key='Ageing Report Customer Bill Wise' where id='F70600'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Debtors Outstanding Report (Customer Wise)',search_key='Ageing Report Customer Wise' where id='F70602'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Creditors Outstanding Report (Bill Wise)',search_key='Ageing Report Vendor Bill Wise' where id='F70604'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Creditors Outstanding Report (Vendor Wise)',search_key='Ageing Report Vendor Wise' where id='F70606'");

            //ICO.add_icon(frm_qstr, "F70192", 3, "States Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70700", 3, "Cost Centre Masters", 3, "-", "-", "-", "fin70_e5", "fin70_a1", "fin70_CCENT", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70701", 4, "Cost Centre Level 1", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "fin70_CCENT", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70702", 4, "Cost Centre Level 2", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "fin70_CCENT", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70703", 4, "Cost Centre Level 3", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "fin70_CCENT", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70704", 4, "Business Groups", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "fin70_CCENT", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70166", 2, "Accounts VAT/GST Reports", 3, "-", "-", "Y", "fin70_e11", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70710", 3, "Review VAT/GST Payable(Summary)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e11", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70712", 3, "Review VAT/GST Payable(Details)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e11", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70714", 3, "Review VAT/GST Received(Summary)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e11", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70716", 3, "Review VAT/GST Received(Details)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e11", "fin70_a1", "-", "fa-edit", "N", "N");

            if (MV_CLIENT_GRP == "SG_TYPE")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F10552')");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Create/Update ERP User Master' where id='F99161'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Allot/Update ERP Usage Rights' where id='F99162'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='View List of ERP Users' where id='F99163'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Allot/Update Desktop Tiles to Users' where id='F99164'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F70181','F70185','F70186','F70187','F99155','F99157','F99159','F99170')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Update Branch/Plant/Unit Information' where id='F99151'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Create/Update Production Shifts Master' where id='F99153'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Create/Update Production Types' where id='F99154'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='Upload Multiple Accounts/Items/AR/AP/Others ' where id='F99165'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='View Connections to ERP Server' where id='F99141'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='View Important Actions Done in ERP' where id='F99142'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='View Options Available in the ERP' where id='F99143'");
        }

        mhd = fgen.chk_RsysUpd("DM0034");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0032','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0034", "DEV_A");

            ICO.add_icon(frm_qstr, "F45168", 4, "Lead type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45169", 4, "Industry type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45179", 4, "Contact Level Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "Y");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LEAD_TYPE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LEAD_TYPE VARCHAR2(30) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LEAD_STATE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LEAD_STATE VARCHAR2(30) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LEAD_CNTRY");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG ADD LEAD_CNTRY VARCHAR2(30) DEFAULT '-'");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "15/08/2020", "DEV_A", "W0091", "Plant Wise Stage Mapping", "N", "-");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "15/08/2020", "DEV_A", "W0092", "Plant Wise WIP Stages", "N", "-");
            ICO.add_icon(frm_qstr, "F10139A", 3, "Plant WIP Stages", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10135C", 3, "Process Plan (Cartons)", 3, "../tej-base/om_proc_plan.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Trial Balance',web_Action='../tej-base/om_view_acct.aspx',search_key='Searchable 4 Cols Trial Balance' where id='F05349'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Review Trial Balance',web_Action='../tej-base/om_view_acct.aspx',search_key='Searchable 4 Cols Trial Balance' where id='F70650'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Trial Balance 4 Columns',search_key='Print 4 Cols Trial(Op.Bal,Drs,Crs,Closing)' where id='F70151'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Trial Balance 2 Columns',search_key='Print 2 Cols Trial(Closing Balance Only)' where id='F70237'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Trial Balance 6 Columns',search_key='Print 2 Cols Trial(Op.Bal Dr/Cr,Drs,Crs,Closing Dr/Cr)' where id='F70238'");



            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='-',submenuid='fin70_e8',mlevel=3,search_key='Print Balance Sheet Grouped on Schedule' where id='F70148'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='-',submenuid='fin70_e8',mlevel=3,search_key='Print Balance Sheet Grouped on Ledger' where id='F70149'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Leads Contacts Master',search_key='Master of Lead Contact' where id='F45161'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Contact Designation Master',search_key='Designation of the Contact' where id='F45179'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Contact Type Master',search_key='Type of the Contact(New/Existing)' where id='F45167'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Contact Industry Type Master',search_key='Industry of the Contact' where id='F45169'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='05) Lead Stage Master',search_key='Indicate Stage of the Lead' where id='F45151'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='06) Lead Action Master',search_key='Action Taken on the Lead' where id='F45165'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='07) Lead Source Master',search_key='Source of Leads for CRM' where id='F45168'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='08) Lead Category Master',search_key='Category of Leads for CRM' where id='F45166'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Lead/Enquiry Entry',search_key='Record/Register New Leads' where id='F45101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Lead/Enquiry Approval',search_key='Approval of a Lead',param='fin45CR1_e1' where id='F45149'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Lead/Enquiry Followup / Actions',search_key='Record Action Taken on Leads' where id='F45106'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Create Quotation' where id='F45109'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='05) Approve Quotation' where id='F45110'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='06) CRM Target Setting',search_key='Set up Targets of Sales Team' where id='F45107'");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='04) Leads/Enquiry : Industry Wise',search_key='List of Enquiries Industry Wise' where id='F45132'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='05) Leads/Enquiry : Salesman Wise',search_key='List of Enquiries Salesman Wise' where id='F45133'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='06) Leads/Enquiry : Source Wise',search_key='List of Enquiries Source Wise' where id='F45144'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='07) Leads/Enquiry : Country Wise',search_key='List of Enquiries Country Wise' where id='F45145'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='08) Leads/Enquiry : State/Province Wise',search_key='List of Enquiries State/Province Wise' where id='F45146'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='09) Leads/Enquiry : Category Wise',search_key='List of Enquiries Hot/Warm/Cold' where id='F45139'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='10) Leads/Enquiry : Action Wise',search_key='List of Enquiries Action Wise' where id='F45148'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='15) Search CRM Contacts List',search_key='List of Contacts in CRM Database' where id='F45162'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_CRM", "COUNTRYN");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_CRM ADD COUNTRYN VARCHAR2(50) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "CURR_STAT");
            if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_LOG modify CURR_STAT VARCHAR2(30) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_ACT", "CURR_STAT");
            if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEAD_ACT modify CURR_STAT VARCHAR2(30) DEFAULT '-'");

            if (MV_CLIENT_GRP == "SG_TYPE")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F70192','F45147','F45134','F45150')");
            }

            ICO.add_icon(frm_qstr, "F45153", 4, "01) Leads/Enquiry Registered Review", 3, "../tej-base/om_dbd_mgrph.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F45141", 4, "01) Lead Mgmt Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='01) Leads/Enquiry Dashboard Review',search_key='View Lead Dashabord (Review Graphically)' where id='F45141'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='02) Leads/Enquiry Registration Review',search_key='New Lead (Review Graphically)' where id='F45153'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='03) Leads/Enquiry Followup Review',search_key='Lead Followup (Review Graphically)' where id='F45143'");


            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "23/08/2020", "DEV_A", "W0063", "Quotation Party Master from ACCTMST + CRMMST ? ", "N", "-"); // MG

        }

        mhd = fgen.chk_RsysUpd("DM0035");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0035", "DEV_A");



            ICO.add_icon(frm_qstr, "F10127", 3, "Item Dimensions", 3, "-", "-", "-", "fin10_e1", "fin10_a1", "fin10_IDEM", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F10127A", 4, "01) Item Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 1", "-", "fin10_e1", "fin10_a1", "fin10_IDEM", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F10127B", 4, "02) Item Application Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 2", "-", "fin10_e1", "fin10_a1", "fin10_IDEM", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F10127C", 4, "03) Item Class Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 3", "-", "fin10_e1", "fin10_a1", "fin10_IDEM", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F10127D", 4, "04) Item SubClass Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 4", "-", "fin10_e1", "fin10_a1", "fin10_IDEM", "fa-edit", "N", "N");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "REP_DIM4");
            if (mhd == "0")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD rep_dim4 VARCHAR2(25)");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify rep_dim4 VARCHAR2(25) default '-'");
            }

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "COSTESTIMATE", "JHOLD");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE COSTESTIMATE ADD JHOLD varchar2(1) DEFAULT '-'");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "VCHNUM");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD VCHNUM VARCHAR2(6) default '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "VCHDATE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD VCHDATE DATE default sysdate");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "ent_by");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD ent_by VARCHAR2(10) default '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "ent_Dt");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD ent_Dt DATE default sysdate");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "edt_by");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD edt_by VARCHAR2(10) default '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "edt_Dt");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD edt_Dt DATE default sysdate");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEWIP", "acref3");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP ADD ACREF3 VARCHAR2(30) default '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WSR_CTRL MODIFY TYPE VARCHAR2(5) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPEWIP MODIFY TYPE1 VARCHAR2(5) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO MODIFY TYPE VARCHAR2(5) DEFAULT '-'");



            ICO.add_icon(frm_qstr, "F40250", 2, "Production Masters", 3, "-", "-", "Y", "fin41_e1", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F40252", 3, "Stage Wise Rejection Reasons", 3, "../tej-base/om_tgpop_mst.aspx", "Reasons for Rejection", "N", "fin41_e1", "fin40_a1", "fin40pp8_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40254", 3, "Stage Wise Down Time Reasons", 3, "../tej-base/om_tgpop_mst.aspx", "Reasons for Down Time", "N", "fin41_e1", "fin40_a1", "fin40pp8_e1", "fa-edit");

            //party evaluation table template :WB_PEVAL_STD , party wise record :WB_PEVAL_ACT
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PEVAL_STD'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_PEVAL_STD(branchcd char(2),type char(2),vchnum char(6),vchdate date,ACODE char(10) default '-',ICODE char(10) default '-',col1 varchar2(60) default '-',col2 varchar2(100) default '-',col3 varchar2(60) default '-',col4 varchar2(60) default '-',col5 varchar2(60) default '-',col6 varchar2(60) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PEVAL_ACT'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_PEVAL_ACT(branchcd char(2),type char(2),vchnum char(6),vchdate date,ACODE char(10) default '-',ICODE char(10) default '-',col1 varchar2(60) default '-',col2 varchar2(100) default '-',col3 varchar2(60) default '-',col4 varchar2(60) default '-',col5 varchar2(60) default '-',col6 varchar2(60) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_PEVAL_STD", "col9");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_PEVAL_STD ADD col9 VARCHAR2(50) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_PEVAL_STD", "TITLE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_PEVAL_STD ADD TITLE VARCHAR2(100) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_PEVAL_ACT", "col9");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_PEVAL_ACT ADD col9 VARCHAR2(50) default '-'");



            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_PEVAL_ACT", "TITLE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_PEVAL_ACT ADD TITLE VARCHAR2(100) default '-'");

            //party evaluation table template :WB_PEVAL_STD , party wise record :WB_PEVAL_ACT
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_LEAD_ALLOT'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_LEAD_ALLOT(branchcd char(2),type char(2),vchnum char(6),vchdate date,SMCODE char(10) default '-',Lead_no char(6) default '-',Lead_dt date default sysdate,Lead_Vert varchar2(50) default '-',lCustomer varchar2(50) default '-',lCountry varchar2(50) default '-',lContact varchar2(50) default '-',lProduct varchar2(75) default '-',lead_val number(13,2) default 0,lead_catg varchar2(50) default '-',lead_Rmk varchar2(50) default '-',remarks varchar2(100) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set submenuid='fin81_e1' where id in ('F81000','F81100','F81101','F81106','F81111','F81121','F81126','F81127','F81131','F81132')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_FILE_ATCH MODIFY VCHNUM VARCHAR2(10) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table wb_file_atch modify file_path varchar2(100)");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table wb_file_atch modify file_name varchar2(100)");

            ICO.add_icon(frm_qstr, "F45112", 4, "07) Assign Salesman to Lead", 3, "../tej-base/om_lead_sman.aspx", "Assignment of Salesman to Lead", "-", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit", "N", "N");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_qa_templ.aspx' where id='F77004'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_BUDG_CTRL'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_BUDG_CTRL(branchcd char(2),type char(2),vchnum char(6),vchdate date,ACODE char(10) default '-',ICODE char(10) default '-',Totals number(14,3) default 0,mth1 number(14,3) default 0,mth2 number(14,3) default 0,mth3 number(14,3) default 0,mth4 number(14,3) default 0,mth5 number(14,3) default 0,mth6 number(14,3) default 0,mth7 number(14,3) default 0,mth8 number(14,3) default 0,mth9 number(14,3) default 0,mth10 number(14,3) default 0,mth11 number(14,3) default 0,mth12 number(14,3) default 0,srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_BUDG_CTRL", "REMARKS");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_BUDG_CTRL ADD REMARKS VARCHAR2(100) default '-'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='CSMST_PURCH'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table CSMST_PURCH as(Select * from CSMST_CRM where 1=2)");

            ICO.add_icon(frm_qstr, "F15214", 3, "Purchase Contacts Master", 3, "../tej-base/om_crm_Contact.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_VEHI_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_VEHI_LOG(branchcd char(2),type char(2),VEHNO char(6),VEHDT date,DCODE char(10) default '-',VEHI_type varchar2(50) default '-',VEHI_spec varchar2(50) default '-',VEHI_purpose varchar2(25) default '-',VEHI_Chasisno varchar2(25) default '-',VEHI_Regno varchar2(25) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_LEGL_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_LEGL_LOG(branchcd char(2),type char(2),LEGNO char(6),LEGDT date,ACODE char(10) default '-',CASE_type varchar2(50) default '-',CASE_spec varchar2(50) default '-',CASE_purpose varchar2(25) default '-',CASE_Reference varchar2(25) default '-',CASE_Regno varchar2(25) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FAMST", "SHOWINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD SHOWINBR VARCHAR2(40) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify SHOWINBR VARCHAR2(40) default '-'");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "SHOWINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD SHOWINBR VARCHAR2(40) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify SHOWINBR VARCHAR2(40) default '-'");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM_ANX", "SHOWINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM_ANX ADD SHOWINBR VARCHAR2(40) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM_ANX modify SHOWINBR VARCHAR2(40) default '-'");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "MADEINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD MADEINBR VARCHAR2(20) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify MADEINBR VARCHAR2(20) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM_ANX", "MADEINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM_ANX ADD MADEINBR VARCHAR2(20) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM_ANX modify MADEINBR VARCHAR2(20) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FAMST", "SEGNAME");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST ADD SEGNAME VARCHAR2(50) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FAMST modify SEGNAME VARCHAR2(50) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMAS", "MFGINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMAS ADD MFGINBR VARCHAR2(20) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMAS modify MFGINBR VARCHAR2(20) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMASM", "MFGINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASM ADD MFGINBR VARCHAR2(20) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASM modify MFGINBR VARCHAR2(20) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMASQ", "MFGINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASQ ADD MFGINBR VARCHAR2(20) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASQ modify MFGINBR VARCHAR2(20) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMASI", "MFGINBR");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASI ADD MFGINBR VARCHAR2(20) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASI modify MFGINBR VARCHAR2(20) default '-'");

        }
        mhd = fgen.chk_RsysUpd("DM0036");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0036", "DEV_A");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMAS", "SALE_REP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMAS ADD SALE_REP VARCHAR2(30) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMAS modify SALE_REP VARCHAR2(30) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMASM", "SALE_REP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASM ADD SALE_REP VARCHAR2(30) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASM modify SALE_REP VARCHAR2(30) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMASQ", "SALE_REP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASQ ADD SALE_REP VARCHAR2(30) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASQ modify SALE_REP VARCHAR2(30) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SOMASI", "SALE_REP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASI ADD SALE_REP VARCHAR2(30) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SOMASI modify SALE_REP VARCHAR2(30) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHER", "SALE_REP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER ADD SALE_REP VARCHAR2(30) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHER modify SALE_REP VARCHAR2(30) default '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHERP", "SALE_REP");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP ADD SALE_REP VARCHAR2(30) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVOUCHERP modify SALE_REP VARCHAR2(30) default '-'");



            ICO.add_icon(frm_qstr, "F70167", 2, "Transaction Control Module", 3, "-", "-", "Y", "fin70_e12", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70720", 3, "Permit Credit Extension to Party", 3, "../tej-base/om_trans_ctrl.aspx", "-", "-", "fin70_e12", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70722", 3, "Approve Credit Extension to Party", 3, "../tej-base/om_appr.aspx", "-", "-", "fin70_e12", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70724", 3, "Block Transaction with Party", 3, "../tej-base/om_trans_ctrl.aspx", "-", "-", "fin70_e12", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70726", 3, "Approve Transaction Block with Party", 3, "../tej-base/om_appr.aspx", "-", "-", "fin70_e12", "fin70_a1", "-", "fa-edit", "N", "N");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_TRAN_CTRL'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_TRAN_CTRL(branchcd char(2),type char(2),CTRLNO char(6),CTRLDT date,ACODE char(10) default '-',CTRL_type varchar2(50) default '-',CTRL_spec varchar2(50) default '-',CTRL_purpose varchar2(25) default '-',CTRL_Reference varchar2(25) default '-',CTRL_Regno varchar2(25) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_vehi_log.aspx' where id='F75227'");

            ICO.add_icon(frm_qstr, "F70175", 3, "Nature of Accounts (level 1)", 3, "../tej-base/om_view_acct.aspx", "Top level of Chart of Accounts", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "N");


            fgen.save_type(frm_qstr, frm_cocd, "#", "0", "Liabilities Group");
            fgen.save_type(frm_qstr, frm_cocd, "#", "1", "Assets Group");
            fgen.save_type(frm_qstr, frm_cocd, "#", "2", "Incomes Group");
            fgen.save_type(frm_qstr, frm_cocd, "#", "3", "Direct   Expenses (Purchase/Production)");
            fgen.save_type(frm_qstr, frm_cocd, "#", "4", "InDirect Expenses (1)(Admin/HR/Accounts)");
            fgen.save_type(frm_qstr, frm_cocd, "#", "5", "InDirect Expenses (2)(Sales/Others)");
            fgen.save_type(frm_qstr, frm_cocd, "#", "6", "InDirect Expenses (3)(Financial)'");
            fgen.save_type(frm_qstr, frm_cocd, "#", "7", "InDirect Expenses (4)(Taxation)");
            fgen.save_type(frm_qstr, frm_cocd, "#", "8", "InDirect Expenses (5)(Dividend)");
            fgen.save_type(frm_qstr, frm_cocd, "#", "9", "Profit/Loss Group");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='-',submenuid='fin70_e8',mlevel=3 where id='F70151'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='-',submenuid='fin70_e8',mlevel=3 where id='F70237'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='-',submenuid='fin70_e8',mlevel=3 where id='F70238'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Review Balance Sheet',param='-',submenuid='fin70_e8',mlevel=3 where id='F70156'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Review Profit & Loss Account',param='-',submenuid='fin70_e8',mlevel=3 where id='F70189'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_msys where id in ('F70656','F70654')");
            ICO.add_icon(frm_qstr, "F10100A", 3, "Item Classification", 3, "../tej-base/om_tgpop_mst.aspx", "Item Categories for MIS/Valuations", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Item Classification (level 1)' where id='F10100A'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Item Main Groups    (level 2)' where id='F10101'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Item Sub  Groups    (level 3)' where id='F10106'");



            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set Text='Production/Maintenance Machines Master',search_key='Master Entry of Machinery,Section Wise' where id='F75165'");
        }

        ICO.add_icon(frm_qstr, "F10123", 3, "Production Processes (Plant Wise)", 3, "../tej-base/om_tgpop_mst.aspx", "Processes/Operations Plant Wise", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "12/09/2020", "DEV_A", "W0093", "Plant Wise Process/Operation Master", "N", "-");

        if (MV_CLIENT_GRP == "SG_TYPE")
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F10126')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id in ('F47300','F47302','F47305','F47307','F47310','F47313','F47315','F47317','F47320','F47321','F47322','F47323')");
        }

        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITWSTAGE modify stagec CHAR(3) DEFAULT '-'");

        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "12/09/2020", "DEV_A", "W1100", "Sales Order Booked from HO(00) code", "N", "-");

        ICO.add_icon(frm_qstr, "F70170", 3, "Chart of Accounts", 3, "-", "-", "-", "fin70_a5", "fin70_a1", "fin70_COA", "fa-edit", "N", "N");

        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set submenuid='fin70_e5',submenu='Y',visi='Y' where id='F70170'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='fin70_COA',submenuid='fin70_e5',mlevel=4,Text='1) Nature of Accounts (level 1)' where id='F70175'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='fin70_COA',submenuid='fin70_e5',mlevel=4,Text='2) Accounts Groups    (level 2)' where id='F70173'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='fin70_COA',submenuid='fin70_e5',mlevel=4,Text='3) Accounts Schedules (level 3)' where id='F70174'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set param='fin70_COA',submenuid='fin70_e5',mlevel=4,Text='4) Accounts Masters   (level 4)' where id='F70172'");

        ICO.add_icon(frm_qstr, "F70195", 3, "Narration Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

        ICO.add_icon(frm_qstr, "F10133A", 3, "Bill of Materials (Paper Packaging)", 3, "-", "-", "-", "fin10_a5", "fin10_a1", "fin10_PPL", "fa-edit", "N", "N");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Bill of Materials (Paper Packaging)',submenuid='fin10_e2',submenu='-',visi='Y' where id='F10133A'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='1) Process Plan (Corrugated Cartons)',param='fin10_PPL',submenuid='fin10_e2',mlevel=4 where id='F10135'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='2) Process Plan (Mono/Duplex Cartons)',param='fin10_PPL',submenuid='fin10_e2',mlevel=4 where id='F10135C'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='3) Process Plan (Printed Labels)',param='fin10_PPL',submenuid='fin10_e2',mlevel=4 where id='F10135L'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='4) Accessories BOM',param='fin10_PPL',submenuid='fin10_e2',mlevel=4 where id='F10132'");


        ICO.add_icon(frm_qstr, "F10133B", 3, "Bill of Materials (Flexible Packaging)", 3, "-", "-", "-", "fin10_a6", "fin10_a1", "fin20_PPL", "fa-edit", "N", "N");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Bill of Materials (Flexible Packaging)',submenuid='fin10_e2',submenu='-',visi='Y' where id='F10133B'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='1) Process Plan (Flexible)',param='fin20_PPL',submenuid='fin10_e2',mlevel=4 where id='F10135F'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='2) BOM : Poly',param='fin20_PPL',submenuid='fin10_e2',mlevel=4 where id='F10134A'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='3) BOM : Laminate',param='fin20_PPL',submenuid='fin10_e2',mlevel=4 where id='F10134'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='4) BOM : Pouch ',param='fin20_PPL',submenuid='fin10_e2',mlevel=4 where id='F10134B'");

        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set submenuid='fin10_e2' where id='F10123'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Bill of Materials (General)' where id='F10131'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Production WIP Stages (Plant Wise)' where id='F10139A'");
        ICO.add_icon(frm_qstr, "F10139B", 3, "Product SOP Tracker", 3, "../tej-base/om_view_engg.aspx ", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit");


        if (MV_CLIENT_GRP == "SG_TYPE")
        {

            fgen.execute_cmd(frm_qstr, frm_cocd, "update type set BR_CURREN='SAR' where ID='B'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_rsys_OPT set OPT_ENABLE='Y',OPT_PARAM='1' where OPT_id='W0044'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_rsys_OPT set OPT_ENABLE='Y',OPT_PARAM='1',OPT_text='Middle East/GCC/GULF Country' where OPT_id='W2027'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_rsys_OPT_PW set OPT_ENABLE='Y',OPT_PARAM='1',OPT_text='Middle East/GCC/GULF Country' where OPT_id='W2027'");
        }
        else
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_rsys_OPT set OPT_text='Middle East/GCC/GULF Country' where OPT_id='W2027'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_rsys_OPT_PW set OPT_text='Middle East/GCC/GULF Country' where OPT_id='W2027'");
        }

        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "15/09/2020", "DEV_A", "W0067", "Max Amount for NON PO MRR on Gate Entry", "N", "2");


        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='View Item Entries in Columnar Format',submenuid='fin25_e3' where id='F25233'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set search_key='View Stock Summary Detailed Format',submenuid='fin25_e3' where id='F25234'");

        ICO.add_icon(frm_qstr, "F25132A", 3, "Stock Summary (Main Stock)", 3, "../tej-base/om_view_invn.aspx", "Main Stock Summary with (Drill Down)", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F25135A", 3, "Stock Summary (Rejn Stock)", 3, "../tej-base/om_view_invn.aspx", "Rejn Stock Summary with (Drill Down)", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");

        ICO.add_icon(frm_qstr, "F15314A", 3, "Stock Summary (Main Stock)", 3, "../tej-base/om_view_invn.aspx", "Main Stock Summary with (Drill Down)", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");


        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_CRM", "DEALING_IN");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_CRM ADD DEALING_IN varchar2(50) DEFAULT '-'");
        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_PURCH", "DEALING_IN");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_PURCH ADD DEALING_IN varchar2(50) DEFAULT '-'");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_CRM", "CONTACT_OF");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_CRM ADD CONTACT_OF varchar2(50) DEFAULT '-'");
        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_PURCH", "CONTACT_OF");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_PURCH ADD CONTACT_OF varchar2(50) DEFAULT '-'");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_CRM", "SHOWINBR");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_CRM ADD SHOWINBR VARCHAR2(40) ");
        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_CRM modify SHOWINBR VARCHAR2(40) default '-'");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "CSMST_PURCH", "SHOWINBR");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_PURCH ADD SHOWINBR VARCHAR2(40) ");
        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE CSMST_PURCH modify SHOWINBR VARCHAR2(40) default '-'");

        fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_fam_crm as (select branchcd,acode,aname,addr1,addr2,addr3,staten,country,gst_no,person,nvl(deac_by,'-') as deac_by,nvl(PAY_NUM,0) as PAY_NUM,substr(acode,1,2) as Grp,girno,showinbr  from famst where branchcd!='DD' and substr(Acode,1,2) in ('16','02') union all select branchcd,acode,aname,addr1,addr2,addr3,staten,countryn,gst_no,person,'-' as deac_by,0 as PAY_NUM,'16' as grp,'-' as girno,'-' as showinbr from csmst_Crm where branchcd!='DD')");

        fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_fam_vend as (select branchcd,acode,aname,addr1,addr2,addr3,staten,country,gst_no,person,nvl(deac_by,'-') as deac_by,nvl(PAY_NUM,0) as PAY_NUM,substr(acode,1,2) as Grp,girno,showinbr,email,staffcd,telnum  from famst where branchcd!='DD' and substr(Acode,1,2) in ('02','06') union all select branchcd,acode,aname,addr1,addr2,addr3,staten,countryn,gst_no,person,'-' as deac_by,0 as PAY_NUM,'06' as grp,'-' as girno,'-' as showinbr,email,'-' as staffcd,telnum from csmst_purch where branchcd!='DD')");



        if (MV_CLIENT_GRP == "SG_TYPE")
        {
            ICO.add_icon(frm_qstr, "F59000", 2, "Dispatch Vehicle Management", 3, "-", "-", "Y", "fin59_e1", "fin50_a1", "-", "fa-edit");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set mlevel=3,submenuid='fin59_e1' where id in('F47125','F47120','F47124','F50137A','F50137B')");

        }

        ICO.add_icon(frm_qstr, "F57000", 2, "Dispatch Schedules Monitoring", 3, "-", "-", "Y", "fin57_e1", "fin50_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F50252", 4, "Schedule Vs Dispatch (Searchable)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set mlevel=3,submenuid='fin57_e1' where id in('F50240','F50241','F50242','F50244','F50245','F50250','F50251','F50252','F50181')");

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_SMAN_LOG'", "tname");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SMAN_LOG(branchcd char(2),type char(2),SDWNO char(6),SDWDT date,Cl_Src varchar2(60) default '-',Cl_Vert varchar2(60) default '-',Cl_Catg varchar2(30) default '-',Cl_Interest varchar2(30) default '-',Cl_Coname varchar2(20) default '-',Cl_Person varchar2(20) default '-',Cl_Phone varchar2(30) default '-',Cl_email varchar2(30) default '-',Cl_desig varchar2(30) default '-',Cremarks varchar2(100) default '-',Oremarks varCHAR2(100) DEFAULT '-',Expect_val number(12,2) default 0,Expense_val number(12,2) default 0,Next_Action varchar2(10) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");


        ICO.add_icon(frm_qstr, "F99144", 3, "Data Entry Statistics", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45154", 4, "04) Leads Tracking Approval, Conversion, Pending", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit", "N", "Y");

        ICO.add_icon(frm_qstr, "F99175", 2, "Transaction Type Masters", 3, "-", "-", "Y", "fin99_e6", "fin99_a1", "-", "fa-edit");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set mlevel=3,submenuid='fin99_e6' where id in('F99154','F25201','F25203','F25205','F25207','F15201','F50201','F70176','F99153')");

        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Accounts Voucher Types Master',search_key='Types of Vouchers in Accounts' where id='F70176'");

        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Store Inwards Types Master',search_key='Types/Series of Store GRN/MRR' where id='F25201'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Store Outward Types Master',search_key='Types/Series of Store Challans' where id='F25203'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Store Issues Types Master',search_key='Types/Series of Store Issues' where id='F25205'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Store Return Types Master',search_key='Types/Series of Store Returns' where id='F25207'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Sales Order/Invoice Types Master',search_key='Types/Series of Sales Order/Invoices' where id='F50201'");

        //ICO.add_icon(frm_qstr, "F10127A", 3, "Item Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 1", "-", "fin10_e1", "fin10_e1", "-", "fa-edit");
        //ICO.add_icon(frm_qstr, "F10127B", 3, "Item Application Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 2", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
        //ICO.add_icon(frm_qstr, "F10127C", 3, "Item Class Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 3", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
        //ICO.add_icon(frm_qstr, "F10127D", 3, "Item SubClass Master", 3, "../tej-base/om_tgpop_mst.aspx", "Product Dimension 4", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");



    }


}