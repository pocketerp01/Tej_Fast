using System;


public class Dml_wfin
{
    fgenDB fgen = new fgenDB();
    public void chkTab(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        //-------------------------
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME = 'FIN_RSYS_UPD'", "TNAME");
        if (mhd == "0")
        {
            mhd = "create table FIN_RSYS_UPD(IDNO varchar2(10) Default '-',ent_by varchar2(10) default '-',ent_Dt date default sysdate)";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);
        }
        mhd = fgen.chk_RsysUpd("IDNOLE");
        if (mhd == "0" || mhd == "")
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_RSYS_UPD MODIFY IDNO VARCHAR(10)");
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_RSYS_UPD (IDNO) VALUES ('IDNOLE')");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "IDNOLE", "DEV_A");
        }

        //-------------------------

        switch (frm_cocd)
        {
            case "PKGW":
            case "TEST":
            case "MLGI":
                //updates on 02 Marchc
                #region Gen_upds_02march

                mhd = fgen.chk_RsysUpd("DM0007");
                if (mhd == "0" || mhd == "")
                {
                    //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0007','DEV_A',sysdate)");
                    fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0007", "DEV_A");

                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_RSYS_UPD'", "tname");
                    if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table FIN_RSYS_UPD(IDNO varchar2(6) Default '-',ent_by varchar2(10) default '-',ent_Dt date default sysdate)");

                    mhd = fgen.chk_RsysUpd("DM0001");
                    if (mhd == "0" || mhd == "")
                    {
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0001','DEV_A',sysdate)");
                        fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0001", "DEV_A");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CSS_LOG'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CSS_LOG(branchcd char(2),type char(2),CSSNO char(6),CSSDT date,CCODE char(10) default '-',EModule varchar2(30) default '-',EICON varchar2(30) default '-',REQ_TYPE varchar2(20) default '-',ISS_TYPE varchar2(20) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Priority number(2) default 0,remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CSS_ASG'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CSS_ASG(branchcd char(2),type char(2),DSRNO char(6),DSRDT date,CSSNO char(6),CSSDT date,CCODE char(10) default '-',eModule varchar2(30) default '-',EICON varchar2(50) default '-',ASG_ASYS varchar2(50) default '-',Priority number(3) default 0,ASG_DPT VARCHAR2(50),ASG_AGT VARCHAR2(50),remarks varchar2(150) default '-',CSS_STATUS varchar(50) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CSS_ACT'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CSS_ACT(branchcd char(2),type char(2),ACTNO char(6),ACTDT date,DSRNO char(6),DSRDT date,CSSNO char(6),CSSDT date,CCODE char(10) default '-',eModule varchar2(30) default '-',EICON varchar2(50) default '-',Priority number(2) default 0,ASG_DPT VARCHAR2(50),ASG_AGT VARCHAR2(50),remarks varchar2(150) default '-',ACT_STATUS varchar(50) default '-',ACT_DATE date default sysdate,srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_TYPE_MST'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_Type_Mst(branchcd char(2),id char(4),tmstno char(6),tmstdt date,type1 char(4) default '-',Name varchar2(80) default '-',typedpt varchar2(20) default '-',suppfld1 varchar2(50) default '-',suppfld2 varchar2(50) default '-',suppfld3 varchar2(50) default '-',orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "LAST_ACTION");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD Last_Action VARCHAR2(50) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "LAST_ACTDT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD Last_Actdt VARCHAR2(10) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "FAPP_BY");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD FAPP_BY VARCHAR2(15) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "FAPP_DT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD FAPP_DT date DEFAULT sysdate");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "WORK_ACTION");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD WORK_Action VARCHAR2(50) DEFAULT '-'");


                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ASG", "LAST_ACTION");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ASG ADD Last_Action VARCHAR2(50) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ASG", "LAST_ACTDT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ASG ADD Last_Actdt VARCHAR2(10) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ASG", "IMPL_STATUS");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ASG ADD IMPL_status VARCHAR2(50) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ASG", "TASK_COMPL");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ASG ADD TASK_COMPL VARCHAR2(1) DEFAULT '-'");


                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ACT", "TASK_COMPL");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ACT ADD TASK_COMPL VARCHAR2(1) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ACT", "NEXT_TGT_DATE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ACT ADD NEXT_TGT_DATE VARCHAR2(10) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ACT", "FILEPATH");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ACT ADD FILEPATH VARCHAR2(100) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_ACT", "FILENAME");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_ACT ADD FILENAME VARCHAR2(60) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "DIR_COMP");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD DIR_COMP CHAR(1) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CSS_LOG", "WRKRMK");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG ADD WRKRMK CHAR(150) DEFAULT '-'");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_DSL_LOG'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_DSL_LOG(branchcd char(2),type char(2),DSLNO char(6),DSLDT date,DCODE char(10) default '-',CCODE char(10) default '-',EVertical varchar2(30) default '-',EModule varchar2(30) default '-',EICON varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',WRKRMK CHAR(150) DEFAULT '-',DIR_COMP CHAR(1) DEFAULT '-',Epurpose varchar2(20) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CAM_LOG'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CAM_LOG(branchcd char(2),type char(2),CAMNO char(6),CAMDT date,TCODE char(10) default '-',CAM_type varchar2(50) default '-',CAM_spec varchar2(50) default '-',CAM_purpose varchar2(25) default '-',CAM_Durn varchar2(25) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CSS_LOG modify filepath VARCHAR2(100) DEFAULT '-'");


                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "CCM_CLOSE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CCM_LOG ADD CCM_CLOSE VARCHAR2(1) DEFAULT '-'");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CCM_ACT'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CCM_ACT(branchcd char(2),type char(2),CACNO char(6),CACDT date,CCMNO char(6),CCMDT date,Cust_NAME varchar2(80) default '-',comp_type varchar2(30) default '-',Cdescr varchar2(30) default '-',Compcatg varchar2(30) default '-',compOccr varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Input_from varchar2(20) DEFAULT '-',Act_mode varchar2(10) DEFAULT '-',Next_Folo number(5) DEFAULT 0,Oremarks CHAR(150) DEFAULT '-',CCM_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_ACT", "CURR_STAT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CCM_ACT ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "CURR_STAT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CCM_LOG ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");
                    }
                    mhd = fgen.chk_RsysUpd("DM0002");
                    if (mhd == "0" || mhd == "")
                    {
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0002','DEV_A',sysdate)");
                        fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0002", "DEV_A");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_STL_LOG'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_STL_LOG(branchcd char(2),type char(2),STLNO char(6),STLDT date,TCODE char(10) default '-',CCODE char(10) default '-',EVertical varchar2(30) default '-',EModule varchar2(30) default '-',EICON varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',WRKRMK CHAR(150) DEFAULT '-',DIR_COMP CHAR(1) DEFAULT '-',Epurpose varchar2(20) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_OMS_LOG'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_OMS_LOG(branchcd char(2),type char(2),OPLNO char(6),OPLDT date,CCODE char(10) default '-',Month_Amt nUMBER(12,2) default 0,remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_OMS_ACT'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_OMS_ACT(branchcd char(2),type char(2),OACNO char(6),OACDT date,CCODE char(10) default '-',Agree_Amt nUMBER(12,2) default 0,Agree_dt date default sysdate,remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_OMS_LOG", "TCODE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_OMS_LOG ADD TCODE VARCHAR2(10) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_OMS_LOG", "NARATION");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_OMS_LOG ADD NARATION VARCHAR2(200) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_OMS_ACT", "TCODE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_OMS_ACT ADD TCODE VARCHAR2(10) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_OMS_ACT", "NARATION");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_OMS_ACT ADD NARATION VARCHAR2(200) DEFAULT '-'");
                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_OMS_ACT", "ACT_MODE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_OMS_ACT ADD ACT_MODE VARCHAR2(10) DEFAULT '-'");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_ALF_PLAN'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_ALF_PLAN(branchcd char(2),type char(2),ALFNO char(6),ALFDT date,TCODE char(10) default '-',CCODE char(10) default '-',VISIT_dT DATE default SYSDATE,remarks varchar2(100) default '-',NARATION varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                    }

                    mhd = fgen.chk_RsysUpd("DM0003");
                    if (mhd == "0" || mhd == "")
                    {
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0003','DEV_A',sysdate)");
                        fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0003", "DEV_A");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "DPT_CODE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD DPT_CODE VARCHAR2(10) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "DPT_NAME");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD DPT_NAME VARCHAR2(40) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "TR_CODE");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD TR_CODE VARCHAR2(10) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "TR_NAME");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD TR_NAME VARCHAR2(40) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "EDT_BY");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD EDT_BY VARCHAR2(20) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "EDT_DT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD EDT_DT date DEFAULT sysdate");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "chk_BY");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD chk_BY VARCHAR2(20) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "chk_DT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD chk_DT date DEFAULT sysdate");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "app_BY");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD app_BY VARCHAR2(20) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "app_DT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD app_DT date DEFAULT sysdate");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPTRAIN", "NARATION");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPTRAIN ADD NARATION VARCHAR2(150) DEFAULT '-'");

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_LEV_REQ'", "tname");
                        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_LEV_REQ(branchcd char(2),type char(2),LRQNO char(6),LRQDT date,Empcode char(10) default '-',Lreason1 varchar2(30) default '-',Lreason2 varchar2(30) default '-',Levfrom varchar2(10) default '-',Levupto varchar2(10) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',Resp_Shared CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                    }

                    mhd = fgen.chk_RsysUpd("DM0004");
                    if (mhd == "0" || mhd == "")
                    {
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0004','DEV_A',sysdate)");
                        fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0004", "DEV_A");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEV_REQ", "CHK_BY");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEV_REQ ADD CHK_BY VARCHAR2(12) DEFAULT '-'");

                        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEV_REQ", "CHK_DT");
                        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_LEV_REQ ADD CHK_DT date DEFAULT sysdate");

                    }
                }
                break;
                #endregion
            case "MLGA":
                #region mlga_upds
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='PROJ_MAST'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table proj_mast(branchcd char(2),type char(2),vchnum char(6),vchdate date,req_by char(10) default '-',req_name varchar2(50) default '-',acode char(10) default '-',Icode char(10) default '-',name varchar2(80) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate,mapp_by varchar2(15) default '-',mapp_Dt date default sysdate)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='PROJ_DTL'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table proj_DTL(branchcd char(2),type char(2),vchnum char(6),vchdate date,req_by char(10) default '-',req_name varchar2(50) default '-',acode char(10) default '-',Icode char(10) default '-',name varchar2(80) default '-',remarks varchar2(150) default '-',Start_dt varchar2(10) default '-',End_dt varchar2(10) default '-',Proj_Refno varchar2(20) default '-',Proj_Hrs number(10) default 0,srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate,mapp_by varchar2(15) default '-',mapp_Dt date default sysdate)");
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='PROJ_ASGN'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table PROJ_ASGN(branchcd char(2),type char(2),vchnum char(6),vchdate date,Dpcode char(10) default '-',BUcode char(10) default '-',PJcode char(10) default '-',TKcode char(10) default '-',Asgecode char(10) default '-',AsgeName varchar2(30) default '-',Asgrcode char(10) default '-',DPC_no varchar2(15) default '-',EST_Hrs number(10) default 0,Assgn_Dt varchar2(10) default '-',Assgn_time varchar2(10) default '-',Target_Dt varchar2(10) default '-',Alert_Dt varchar2(10) default '-',IAC_Filled varchar2(10) default '-',Others varchar2(10) default '-',Proj_Name varchar2(100) default '-',remarks1 varchar2(150) default '-',remarks2 varchar2(150) default '-',rework varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate,mapp_by varchar2(15) default '-',mapp_Dt date default sysdate)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='PROJ_UPDT'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table PROJ_UPDT(branchcd char(2),type char(2),vchnum char(6),vchdate date,TAvchnum char(6),TAvchdate date,Projcode char(10) default '-',SWcode char(10) default '-',Stcode char(10) default '-',Asgecode char(10) default '-',AsgeName varchar2(30) default '-',Proj_Name varchar2(100) default '-',Ustart_Dt varchar2(10) default '-',Ustart_time varchar2(5) default '-',Uend_Dt varchar2(10) default '-',Uend_Time varchar2(5) default '-',Sstart_Dt varchar2(10) default '-',Sstart_time varchar2(5) default '-',Send_Dt varchar2(10) default '-',Send_Time varchar2(5) default '-',Cad_submit number(5) default 0,Drg_submit number(5) default 0,remarks1 varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate,mapp_by varchar2(15) default '-',mapp_Dt date default sysdate)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='PROJ_DTIME'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table PROJ_DTIME(branchcd char(2),type char(2),vchnum char(6),vchdate date,Projcode char(10) default '-',Proj_Name varchar2(75) default '-',Asgecode char(10) default '-',DTcode char(10) default '-',DTstart_time varchar2(5) default '-',DTend_Time varchar2(5) default '-',DT_Hrs number(10,2) default 0,DT_Remark varchar2(100) default '-',srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate)");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_mast", "log_Ref");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_mast ADD log_ref VARCHAR2(10) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_mast", "Desig");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_mast ADD Desig VARCHAR2(20) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_mast", "hrcost");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_mast ADD hrcost number(10,2) DEFAULT 0");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_updt", "utime");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_updt ADD utime number(10,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_updt", "stime");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_updt ADD stime number(10,2) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_updt", "uhrcost");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_updt ADD uhrcost number(10,2) DEFAULT 0");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_dtime", "uhrcost");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_dtime ADD uhrcost number(10,2) DEFAULT 0");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "proj_dtl", "proj_cost");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE proj_dtl ADD proj_cost number(10,2) DEFAULT 0");
                break;

                #endregion
            case "MSES":
                #region MSES_UPDS
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SYS_CONFIG", "OBJ_READONLY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SYS_CONFIG ADD OBJ_READONLY VARCHAR2(1) DEFAULT 'N'");
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='PROJ_MAST'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table proj_mast(branchcd char(2),type char(2),vchnum char(6),vchdate date,req_by char(10) default '-',req_name varchar2(50) default '-',acode char(10) default '-',Icode char(10) default '-',name varchar2(80) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate,mapp_by varchar2(15) default '-',mapp_Dt date default sysdate)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='TYPE_MAST'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table TYPE_MAST(branchcd char(2),ID char(2),type char(2),vchnum char(6),vchdate date,req_by char(10) default '-',req_name varchar2(50) default '-',acode char(10) default '-',Icode char(10) default '-',name varchar2(80) default '-',remarks varchar2(100) default '-',srno number(4) default 0,orignalbr char(2),ment_by varchar2(15) default '-',ment_Dt date default sysdate,medt_by varchar2(15) default '-',medt_Dt date default sysdate,mapp_by varchar2(15) default '-',mapp_Dt date default sysdate)");
                break;
                #endregion
            default:
                break;
        }

        //updates on 02march
        #region Gen_upds_02_March
        mhd = fgen.chk_RsysUpd("DM0008");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0008','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0008", "DEV_A");

            //updates on 26 Jan
            #region Gen_upds_26jan
            mhd = fgen.chk_RsysUpd("DM0005");
            if (mhd == "0" || mhd == "")
            {
                //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0005','DEV_A',sysdate)");
                fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0005", "DEV_A");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_MSYS'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE FIN_MSYS(ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(180) default '-',ALLOW_LEVEL NUMBER(2),WEB_aCTION VARCHAR2(50) default '-',SEARCH_KEY VARCHAR2(50) default '-',submenu char(1)default 'N',submenuid char(15) default '-',form varchar2(10) default '-',param varchar2(40) default '-',imagef varchar2(50) default '-',CSS varchar2(30) default 'fa-edit',PRD varchar2(1) default '-',BRN varchar2(1) default '-',BNR varchar2(1) default '-')");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_MRSYS'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table FIN_MRSYS(USERID VARCHAR2(10),USERNAME VARCHAR2(30),BRANCHCD CHAR(2),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE,ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(50),ALLOW_LEVEL NUMBER(2),WEB_ACTION  VARCHAR2(50),SEARCH_KEY  vARCHAR2(50),SUBMENU  CHAR(1),SUBMENUID CHAR(15),FORM VARCHAR2(10),PARAM  VARCHAR2(40),USER_COLOR VARCHAR(10) DEFAULT '00578b',IDESC VARCHAR(50) DEFAULT '-',CSS varchar2(30) default 'fa-edit',RCAN_ADD CHAR(1) DEFAULT 'Y',RCAN_EDIT CHAR(1) DEFAULT 'Y',RCAN_DEL CHAR(1) DEFAULT 'Y')");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SYS_CONFIG", "OBJ_READONLY");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SYS_CONFIG ADD OBJ_READONLY CHAR(1) DEFAULT 'N'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "CSS");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "CSS");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MRSYS ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SYS_CONFIG", "OBJ_FMAND");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SYS_CONFIG ADD OBJ_FMAND VARCHAR2(1) DEFAULT 'N'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "MTHLYPLAN", "DCODE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE MTHLYPLAN ADD DCODE VARCHAR2(2) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PROD_SHEET", "DCODE");
                if (mhd == "0")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PROD_SHEET ADD DCODE VARCHAR2(2) ");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PROD_SHEET modify DCODE VARCHAR2(2) default '-'");
                }

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PROD_SHEET", "EDT_BY");
                if (mhd == "0")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PROD_SHEET ADD EDT_BY VARCHAR2(15) ");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PROD_SHEET modify EDT_BY VARCHAR2(15) default '-'");
                }

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PROD_SHEET", "EDT_DT");
                if (mhd == "0")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PROD_SHEET ADD EDT_DT date ");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PROD_SHEET modify EDT_DT date default sysdate");
                }
                if (frm_cocd == "PPI")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_gate_po as (select a.branchcd,a.acode,a.ordno,a.orddt,trim(a.ERP_code) as icode,a.Prate,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link from (select fstr,branchcd,ordno,orddt,trim(AcodE) as Acode,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(check_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode,branchcd,ponum,podate from ivoucherp where branchcd!='DD' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,ordno,orddt having sum(Qtyord)-sum(Soldqty)>0 ) a)");
                }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_gate_po as (select a.branchcd,a.acode,a.ordno,a.orddt,trim(a.ERP_code) as icode,a.Prate,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link from (select fstr,branchcd,ordno,orddt,trim(AcodE) as Acode,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode,branchcd,ponum,podate from ivoucherp where branchcd!='DD' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,ordno,orddt having sum(Qtyord)-sum(Soldqty)>0 ) a)");
                }
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_gate_RGP as (select a.branchcd,a.acode,a.vchnum,a.vchdate,trim(a.ERP_code) as icode,(a.Qtyord) as Sent_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as RGP_link from (select fstr,branchcd,vchnum,vchdate,trim(AcodE) as Acode,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,acode,branchcd,vchnum,vchdate from rgpmst where branchcd!='DD' and type like '2%' and trim(type)!='22' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,acode,branchcd,rgpnum,rgpdate from ivoucherp where branchcd!='DD' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and prnum='RG' )  group by fstr,ERP_code,trim(acode),branchcd,vchnum,vchdate having sum(Qtyord)-sum(Soldqty)>0 ) a)");

                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_mrr_RGP as (select a.branchcd,a.acode,a.vchnum,a.vchdate,trim(a.ERP_code) as icode,(a.Qtyord) as Sent_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as RGP_link from (select fstr,branchcd,vchnum,vchdate,trim(AcodE) as Acode,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,acode,branchcd,vchnum,vchdate from rgpmst where branchcd!='DD' and type like '2%' and trim(type)!='22' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(rej_rw,0) as qtyord,acode,branchcd,rgpnum,rgpdate from ivoucher where branchcd!='DD' and type in ('09','0J') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,vchnum,vchdate having sum(Qtyord)-sum(Soldqty)>0 ) a)");

                mhd = "create or replace view wbvu_PR_4PO as (select branchcd,fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,max(bank) as Deptt,max(delv_item) As delv_item,max(desc_) as desc_ from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,upper(nvl(bank,'-')) As bank,nvl(delv_item,'-') As delv_item,nvl(desc_,'-') as desc_,branchcd from pomas where branchcd!='DD' and type='60' and trim(pflag)!=0 and trim(app_by)!='-' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT to_ChaR(pr_Dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,0 as Qtyord,qtyord,null as bank,null as delv_item,null as desc_,branchcd from pomas where branchcd!='DD' and type like '5%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy'))  group by branchcd,fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0 )  ";
                fgen.execute_cmd(frm_qstr, frm_cocd, mhd);

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where upper(tname)=upper('WB_MAIL_MGR')", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_mail_mgr(branchcd char(2),type char(2),vchnum char(6),vchdate date,RCODE char(10) default '-',ECODE char(10) default '-',Mail_Freq nUMBER(8,2) default 0,Mail_Sent_Dt varchar2(10) default '-',remarks varchar2(50) default '-',naration varchar2(100) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='UDF_DATA'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table UDF_DATA(branchcd char(2),PAR_TBL varchar2(30) default '-',PAR_FLD varchar2(30) default '-',udf_name varchar2(30) default '-',udf_value varchar2(100) default '-',srno number(4) default 0)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_ISS_REQ'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_ISS_REQ(branchcd char(2),type char(2),vchnum char(6),vchdate date,ACODE char(10) default '-',Stage char(10) default '-',ICODE char(10) default '-',no_bdls char(10) default '-',desc_ varchar2(100) default '-',naration varchar2(100) default '-',req_qty number(12,3) default 0,req_wt number(12,3) default 0,jobno varchar2(10) default '-',jobdt date default sysdate,morder number(4) default 0,orignalbr char(2),closed varchar2(1) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEGRP", "VCHNUM");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE typegrp ADD VCHNUM VARCHAR2(6) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEGRP", "EDT_BY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE typegrp ADD EDT_BY VARCHAR2(12) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPEGRP", "EDT_DT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE typegrp ADD EDT_DT date DEFAULT sysdate");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_RSYS_OPT'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table FIN_RSYS_OPT(branchcd char(2),type char(2),vchnum char(6),vchdate date default sysdate,OPT_ID varchar2(6) Default '-',OPT_TEXT varchar2(200) default '-',OPT_ENABLE varchar2(1) default '-',OPT_PARAM varchar2(20) default '-',OPT_PARAM2 varchar2(20) default '-',OPT_EXCL varchar2(20) default '-',ent_by varchar2(10) default '-',ent_Dt date default sysdate,edt_by varchar2(10) default '-',edt_Dt date default sysdate)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='SOMASI'", "tname");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table SOMASI as (select * From somas where 1=2)");
            }
            #endregion

            //updates on 18 feb

            #region Gen_upds_18feb
            mhd = fgen.chk_RsysUpd("DM0006");
            if (mhd == "0" || mhd == "")
            {
                //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0006','DEV_A',sysdate)");
                fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0006", "DEV_A");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "TVCHNUM");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD TVCHNUM VARCHAR2(6) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "TVCHDATE");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE type ADD TVCHDATE date DEFAULT sysdate");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "MENT_BY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD MENT_BY VARCHAR2(15) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "MENT_DT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE type ADD MENT_DT date DEFAULT sysdate");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "MEDT_BY");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD MEDT_BY VARCHAR2(15) DEFAULT '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "MEDT_DT");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE type ADD MEDT_DT date DEFAULT sysdate");


                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SYS_CONFIG", "OBJ_READONLY");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SYS_CONFIG ADD OBJ_READONLY CHAR(1) DEFAULT 'N'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "CSS");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "CSS");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MRSYS ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS modify PARAM varchar2(40) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS modify text varchar2(180) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS modify form varchar2(20) default '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "alter table DBD_TV_CONFIG modify OBJ_READONLY VARCHAR2(10) default '-'");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "DBD_TV_CONFIG", "FRM_NAME");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE DBD_TV_CONFIG ADD FRM_NAME VARCHAR2(50) DEFAULT '-'");



            }
            #endregion


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "DEAC_DT");
            if (mhd == "0")
            {
                //change done 22/08/2020
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM ADD DEAC_DT VARCHAR2(10) ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITEM modify DEAC_DT VARCHAR2(10) default '-' ");
            }


            // to update FIN_MSYS
            mhd = "update FIN_MSYS set web_Action='../tej-base/om_view_sys.aspx' where id in ('F99126','F99127','F99128','F99129')";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);

        }
        #endregion
        //03.03.18

        // ------------------------------------------------------------------
        //General DML
        //fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS add CONSTRAINT finrsys_pk PRIMARY KEY (ID)");
        //fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE fin_rsys_upd add CONSTRAINT finrsysupd_pk PRIMARY KEY (IDNO)");



        mhd = fgen.chk_RsysUpd("DM0009");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0009','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0009", "DEV_A");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='FIN_RSYS_OPT_PW'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table FIN_RSYS_OPT_PW(branchcd char(2),type char(2),vchnum char(6),vchdate date default sysdate,OPT_ID varchar2(6) Default '-',OPT_TEXT varchar2(200) default '-',OPT_ENABLE varchar2(1) default '-',OPT_PARAM varchar2(20) default '-',OPT_PARAM2 varchar2(20) default '-',OPT_EXCL varchar2(20) default '-',ent_by varchar2(10) default '-',ent_Dt date default sysdate,edt_by varchar2(10) default '-',edt_Dt date default sysdate)");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_RSYS_OPT_PW", "OPT_EXCL"); if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "alter table fin_rsys_opt_pw rename column opt_Excl to opt_start");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/frmUmst.aspx' where id='97001'");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "TBRANCHCD"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE type ADD TBRANCHCD VARCHAR2(2) DEFAULT '00'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "TVCHNUM"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE type ADD TVCHNUM VARCHAR2(6) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "TVCHDATE"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE type ADD TVCHDATE date DEFAULT sysdate");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "IVCHNUM"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE item ADD IVCHNUM VARCHAR2(6) ");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "IVCHDATE"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE item ADD IVCHDATE date ");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "IVCHNUM"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE item modify IVCHNUM VARCHAR2(6) default '-' ");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITEM", "IVCHDATE"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE item modify IVCHDATE date default sysdate");

        }

        mhd = fgen.chk_RsysUpd("DM0010");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0010','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0010", "DEV_A");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "PRD"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD PRD VARCHAR2(1) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "BRN"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD BRN VARCHAR2(1) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "BNR"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD BNR VARCHAR2(1) DEFAULT '-'");



            //08.03.18 & 11.03.2018 pkgg


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MSYS", "VISI"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MSYS ADD VISI CHAR(1) DEFAULT 'Y'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FIN_MRSYS", "VISI"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FIN_MRSYS ADD VISI CHAR(1) DEFAULT 'Y'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHER", "COL1"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ivoucher add col1 varchar2(20) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVOUCHER", "mr_gdate"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ivoucher add mr_gdate date");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVCH_HIST", "COL1"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ivch_hist add col1 varchar2(20) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "IVCH_HIST", "mr_gdate"); if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ivch_hist add mr_gdate date ");


            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set Text='BOMs with deactivated Items' where ID='F10228'");

            //21.02.18
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_sale.aspx' where ID='F50241'");
            // post  14.03.2018
            if (frm_cocd == "PPI")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_pr as select a.fstr,a.branchcd,a.ordno,a.orddt,trim(a.icode) as icode, max(a.bank) as deptt,  sum(a.Qtyord) as req_qty,sum(a.ord) as ord_qty,sum(a.Qtyord)- sum(a.ord) as Bal_qty,b.iname, b.unit,b.cpartno from ((SELECT (to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(icode)) as fstr,branchcd,ordno,orddt,trim(icode) as icode,bank, Qtyord,0 as ord from pomas where branchcd!='DD' and type= '60' and trim(pflag)!=0 and (trim(check_by)!='-' or trim(app_by)!='-') )  union all SELECT (to_ChaR(pr_dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(icode)) as fstr,branchcd,pr_no as ordno,pr_dt as orddt,  trim(icode) as icode,null as bank, 0 as Qtyord, qtyord as ord  from pomas where branchcd not in ('DD','AM') and type like '5%' ) a, item b where trim(a.icode)= trim(b.icode) group by a.fstr,a.branchcd,a.ordno,a.orddt,trim(a.icode), b.unit,b.iname,b.cpartno having length(max(a.bank))>1 and sum(a.Qtyord)-sum(a.ord)>0");
            }
            else
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_pr as select a.fstr,a.branchcd,a.ordno,a.orddt,trim(a.icode) as icode, max(a.bank) as deptt,  sum(a.Qtyord) as req_qty,sum(a.ord) as ord_qty,sum(a.Qtyord)- sum(a.ord) as Bal_qty,b.iname, b.unit,b.cpartno,max(a.desc_) as desc_ from ((SELECT (to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(icode)||'-'||trim(pr_srn)) as fstr,branchcd,ordno,orddt,trim(icode) as icode,bank, Qtyord,0 as ord,desc_ from pomas where branchcd!='DD' and type= '60' and trim(pflag)!=0 and (trim(chk_by)!='-' or trim(app_by)!='-') )  union all SELECT (to_ChaR(pr_dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(icode)||'-'||trim(pr_srn)) as fstr,branchcd,pr_no as ordno,pr_dt as orddt,  trim(icode) as icode,null as bank, 0 as Qtyord, qtyord as ord,desc_  from pomas where branchcd not in ('DD','AM') and type like '5%' ) a, item b where trim(a.icode)= trim(b.icode) group by a.fstr,a.branchcd,a.ordno,a.orddt,trim(a.icode), b.unit,b.iname,b.cpartno having length(max(a.bank))>1 and sum(a.Qtyord)-sum(a.ord)>0");
            }

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Purchase Schedule day wise Checklist' where ID ='F15306'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME = 'DBD_MW_CONFIG'", "TNAME");
            if (mhd == "0")
            {
                mhd = "CREATE TABLE DBD_MW_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), VERT_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', FRM_NAME varchar2(50)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(20) default '-', OBJ_SQL VARCHAR2(1000) default '-')";
                fgen.execute_cmd(frm_qstr, frm_cocd, mhd);
            }


            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_so as (select branchcd,TYPE,ordno,orddt,trim(AcodE) as Acode,ERP_code as Icode,max(cu_chldt) as del_date,max(Srate) as Srate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,sum(Qtyord)-sum(Soldqty) as Bal_Qty,fstr,max(amdtno) as amdtno,max(pordno) as pordno,max(porddt) as porddt,max(desc9) as desc9,max(cpartno) as cpartno,max(Srno) As Srno,max(ent_by) as ent_by,max(ent_dt) As ent_dt,max(app_by) As app_by,max(app_dt) As app_dt from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(cdrgno) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((irate*(100-cdisc)/100))*(case when nvl(CURR_RATE,0)=0 then 1 else nvl(CURR_RATE,0) end )  as srate,acode,branchcd,ordno,orddt,cu_chldt,TYPE,nvl(del_Wk,0) as amdtno,pordno,porddt,(CASE WHEN NVL(desc9,'-')!='-' THEN  NVL(desc9,'-') ELSE NVL(CINAME,'-') END) AS DESC9,cpartno,srno,ent_by,ent_dt,app_by,app_dt   from somas where branchcd!='DD' and type like '4%' and trim(icat)!='Y' and (trim(check_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(revis_no) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as SOLDQTY,0 as irate,acode,branchcd,ponum,podate,null as del_date, TYPE,null as amdtno,null as pordno,null as porddt,null as desc9,null as cpartno,null as srno,null as ent_by,null as ent_dt,null as app_by,null as app_dt  from ivoucher where branchcd!='DD' and type like '4%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')) group by fstr,branchcd,ordno,orddt,TYPE,trim(AcodE),ERP_code having sum(Qtyord)-sum(Soldqty)>0 )");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/om_view_sale.aspx' where id in ('F50225','F50226','F50227')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Production Slip' where id='F40140'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_Action='../tej-base/om_prt_acct.aspx' where id in ('F70151','F70152','F70240')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='System Config' where id='F99100'");
        }

        mhd = fgen.chk_RsysUpd("DM0011");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0011','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0011", "DEV_A");

            // post  25.03.2018
            //fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_mrr_po as (select a.branchcd,a.acode,a.ordno,a.orddt,a.del_date,A.TYPE,trim(a.ERP_code) as icode,a.Prate,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link from (select fstr,branchcd,ordno,orddt,max(del_date) as del_date,TYPE,trim(AcodE) as Acode,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt,del_date, TYPE from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate,null as del_date, TYPE  from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,ordno,orddt,del_date,TYPE having sum(Qtyord)-sum(Soldqty)>0 ) a)");
            //fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_inv_so as (select a.branchcd,A.TYPE,a.ordno,a.orddt,a.acode,trim(a.ERP_code) as icode,a.Srate,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Disp_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,a.del_date,trim(a.Fstr) as PO_link from (select fstr,branchcd,ordno,orddt,max(cu_chldt) as del_date,TYPE,trim(AcodE) as Acode,ERP_code,max(Srate) as Srate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(cdrgno) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((irate*(100-cdisc)/100))*(case when nvl(CURR_RATE,0)=0 then 1 else nvl(CURR_RATE,0) end )  as srate,acode,branchcd,ordno,orddt,cu_chldt,TYPE from somas where branchcd!='DD' and type like '4%' and trim(icat)!='Y' and (trim(check_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(revis_no) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord,0 as irate,acode,branchcd,ponum,podate,null as del_date, TYPE  from ivoucher where branchcd!='DD' and type like '4%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,ordno,orddt,TYPE  ) a)");
            // UNCOMMENT
            //if (frm_cocd == "PPI")
            //{
            //    fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_pr as (select a.branchcd,a.ordno,a.orddt,trim(a.icode) as icode, max(a.bank) as deptt,sum(a.Qtyord) as req_qty,sum(a.ord) as ord_qty,sum(a.Qtyord)- sum(a.ord) as Bal_qty,b.iname, b.unit,b.cpartno,a.fstr,max(a.psize) as psize ,max(a.desc_) as desc_,max(a.ent_by) As ent_by,max(a.ent_dt) As ent_dt,max(a.app_by) As app_by,max(a.app_dt) As app_dt  from (SELECT (to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(icode)) as fstr,branchcd,ordno,orddt,trim(icode) as icode,bank, Qtyord,0 as ord,psize,substr(desc_,1,100) As desc_,ent_by,ent_Dt,app_by,app_dt from pomas where branchcd!='DD' and type= '60' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(pflag)!=0 and (trim(check_by)!='-' or trim(app_by)!='-')  union all SELECT (to_ChaR(pr_dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(icode)) as fstr,branchcd,pr_no as ordno,pr_dt as orddt,  trim(icode) as icode,null as bank, 0 as Qtyord, qtyord as ord,null as psize,null As desc_,null as ent_by,null as ent_Dt,null as app_by,null as app_dt   from pomas where branchcd not in ('DD','AM') and type like '5%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') ) a, item b where trim(a.icode)= trim(b.icode) group by a.fstr,a.branchcd,a.ordno,a.orddt,trim(a.icode), b.unit,b.iname,b.cpartno having length(max(a.bank))>1 and sum(a.Qtyord)-sum(a.ord)>0 )");
            //    fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_po as (select branchcd,TYPE,ordno,orddt,trim(AcodE) as Acode,ERP_code as Icode,max(del_Date) as del_date,max(Prate) as Prate,sum(Qtyord) as Qtyord,sum(Soldqty) as rcvdqty,sum(Qtyord)-sum(Soldqty) as Bal_Qty,fstr,max(amdtno) as amdtno,max(del_sch) as del_sch ,max(desc_) as desc_,max(ent_dt) As ent_dt,max(app_by) As app_by,max(app_dt) As app_dt from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  as prate,acode,branchcd,ordno,orddt,del_Date,TYPE,amdtno,nvl(del_Sch,'-') as del_Sch,substr(nvl(desc_,'-'),1,100) as desc_,ent_by,ent_dt,app_by,app_dt from pomas where branchcd not in ('AM','DD') and type like '5%' and nvl(pflag,0)!=1 and (trim(check_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(potype)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(Rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate,null as del_date, poTYPE,null as amdtno,null as del_Sch,null as desc_,null as ent_by,null as ent_dt,null as app_by,null as app_dt  from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and type in ('02','03','07') and store in ('Y','N')) group by fstr,branchcd,ordno,orddt,TYPE,trim(AcodE),ERP_code having sum(Qtyord)-sum(Soldqty)>0 )");
            //}
            //else
            //{
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_pr as (select a.branchcd,a.ordno,a.orddt,trim(a.icode) as icode, max(a.bank) as deptt,sum(a.Qtyord) as req_qty,sum(a.ord) as ord_qty,sum(a.Qtyord)- sum(a.ord) as Bal_qty,b.iname, b.unit,b.cpartno,a.fstr,max(a.psize) as psize ,max(a.desc_) as desc_,max(a.ent_by) As ent_by,max(a.ent_dt) As ent_dt,max(a.app_by) As app_by,max(a.app_dt) As app_dt  from (SELECT (to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(icode)) as fstr,branchcd,ordno,orddt,trim(icode) as icode,bank, Qtyord,0 as ord,psize,substr(desc_,1,100) As desc_,ent_by,ent_Dt,app_by,app_dt from pomas where branchcd!='DD' and type= '60' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(pflag)!=0 and (trim(chk_by)!='-' or trim(app_by)!='-')  union all SELECT (to_ChaR(pr_dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(icode)) as fstr,branchcd,pr_no as ordno,pr_dt as orddt,  trim(icode) as icode,null as bank, 0 as Qtyord, qtyord as ord,null as psize,null As desc_,null as ent_by,null as ent_Dt,null as app_by,null as app_dt   from pomas where branchcd not in ('DD','AM') and type like '5%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') ) a, item b where trim(a.icode)= trim(b.icode) group by a.fstr,a.branchcd,a.ordno,a.orddt,trim(a.icode), b.unit,b.iname,b.cpartno having length(max(a.bank))>1 and sum(a.Qtyord)-sum(a.ord)>0 )");
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_po as (select branchcd,TYPE,ordno,orddt,trim(AcodE) as Acode,ERP_code as Icode,max(del_Date) as del_date,max(Prate) as Prate,sum(Qtyord) as Qtyord,sum(Soldqty) as rcvdqty,sum(Qtyord)-sum(Soldqty) as Bal_Qty,fstr,max(amdtno) as amdtno,max(del_sch) as del_sch ,max(desc_) as desc_,max(ent_dt) As ent_dt,max(app_by) As app_by,max(app_dt) As app_dt from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  as prate,acode,branchcd,ordno,orddt,del_Date,TYPE,amdtno,nvl(del_Sch,'-') as del_Sch,substr(nvl(desc_,'-'),1,100) as desc_,ent_by,ent_dt,app_by,app_dt from pomas where branchcd not in ('AM','DD') and type like '5%' and nvl(pflag,0)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(potype)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(Rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate,null as del_date, poTYPE,null as amdtno,null as del_Sch,null as desc_,null as ent_by,null as ent_dt,null as app_by,null as app_dt  from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and type in ('02','03','07') and store in ('Y','N')) group by fstr,branchcd,ordno,orddt,TYPE,trim(AcodE),ERP_code having sum(Qtyord)-sum(Soldqty)>0 )");
            //}
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_so as (select branchcd,TYPE,ordno,orddt,trim(AcodE) as Acode,ERP_code as Icode,max(cu_chldt) as del_date,max(Srate) as Srate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,sum(Qtyord)-sum(Soldqty) as Bal_Qty,fstr,max(amdtno) as amdtno,max(pordno) as pordno,max(porddt) as porddt,max(desc9) as desc9,max(cpartno) as cpartno,max(Srno) As Srno,max(ent_by) as ent_by,max(ent_dt) As ent_dt,max(app_by) As app_by,max(app_dt) As app_dt from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(cdrgno) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((irate*(100-cdisc)/100))*(case when nvl(CURR_RATE,0)=0 then 1 else nvl(CURR_RATE,0) end )  as srate,acode,branchcd,ordno,orddt,cu_chldt,TYPE,nvl(del_Wk,0) as amdtno,pordno,porddt,(CASE WHEN NVL(desc9,'-')!='-' THEN  NVL(desc9,'-') ELSE NVL(CINAME,'-') END) AS DESC9,cpartno,srno,ent_by,ent_dt,app_by,app_dt   from somas where branchcd!='DD' and type like '4%' and trim(icat)!='Y' and (trim(check_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(revis_no) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as SOLDQTY,0 as irate,acode,branchcd,ponum,podate,null as del_date, TYPE,null as amdtno,null as pordno,null as porddt,null as desc9,null as cpartno,null as srno,null as ent_by,null as ent_dt,null as app_by,null as app_dt  from ivoucher where branchcd!='DD' and type like '4%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')) group by fstr,branchcd,ordno,orddt,TYPE,trim(AcodE),ERP_code having sum(Qtyord)-sum(Soldqty)>0 )");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/om_view_sale.aspx' where id in ('F50225','F50226','F50227')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Production Slip' where id='F40140'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_Action='../tej-base/om_prt_acct.aspx' where id in ('F70151','F70152','F70240')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='System Config' where id='F99100'");


            //new DML COMMANDS 30.4.2018 ONWARD
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHERP", "ORIGINV_NO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHERP ADD ORIGINV_NO VARCHAR2(16) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHERP", "ORIGINV_DT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHERP ADD ORIGINV_DT date DEFAULT sysdate");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHERP", "GSTVCH_NO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHERP ADD GSTVCH_NO VARCHAR2(30) DEFAULT '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_sale_sCH as (select branchcd,Acode,Icode,sch_mth,sum(Qtyord) AS Sch_Qty,sum(Soldqty) as Sale_qty,sum(Qtyord)-sum(Soldqty)as Bal_Qty from (SELECT branchcd,trim(Acode) as Acode,trim(Icode) as Icode,TOTAL AS Qtyord,0 as Soldqty,IRATE,0 as salerate,to_char(vchdate,'yyyymm') as sch_mth from schedule where branchcd!='DD' and type like '4%'  and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT branchcd,trim(Acode) as Acode,trim(Icode) as Icode,0 AS Qtyord,iqtyout as Soldqty,0 as IRATE,round((irate*(100-ichgs)/100),2) as salerate,to_char(vchdate,'yyyymm') as disp_mth from ivoucher where branchcd!='DD' and type like '4%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')) group by branchcd,Acode,Icode,sch_mth)");
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_purc_sCH as (select branchcd,Acode,Icode,sch_mth,sum(Qtyord) AS Sch_Qty,sum(Soldqty) as Purc_qty,sum(rejn) as rejn_qty,sum(Qtyord)-sum(Soldqty)as Bal_Qty from (SELECT branchcd,trim(Acode) as Acode,trim(Icode) as Icode,TOTAL AS Qtyord,0 as Soldqty,0 as rejn,IRATE,0 as salerate,to_char(vchdate,'yyyymm') as sch_mth from schedule where branchcd!='DD' and type like '66%'  and vchdate>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT branchcd,trim(Acode) as Acode,trim(Icode) as Icode,0 AS Qtyord,iqtyin as Soldqty,rej_Rw as Rejn,0 as IRATE,irate as salerate,to_char(vchdate,'yyyymm') as disp_mth from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and store='Y') group by branchcd,Acode,Icode,sch_mth)");

        }


        mhd = fgen.chk_RsysUpd("DM0013");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0013','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0013", "DEV_A");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "CEXC_COMM");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD CEXC_COMM  VARCHAR2(50) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "MSME_NO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD MSME_NO  VARCHAR2(30) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "COUNTRYNM");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD COUNTRYNM  VARCHAR2(30) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "EMAIL5");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD EMAIL5  VARCHAR2(30) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "IFSC_CODE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD IFSC_CODE  VARCHAR2(30) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "BANKADDR1");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD BANKADDR1  VARCHAR2(60) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "EST_CODE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD EST_CODE  VARCHAR2(10) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "MFG_LICNO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD MFG_LICNO  VARCHAR2(60) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "TYPE", "BANK_PF");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE TYPE ADD BANK_PF  VARCHAR2(60) DEFAULT '-'");
        }

        mhd = fgen.chk_RsysUpd("DM0014");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0014','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0014", "DEV_A");

            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from FIN_MSYS where id in ('F85146','F85147','F85148','F85149','F85150','P17006A','F50159G','F05199') ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Closed PO Register', web_action='../tej-base/om_prt_purc.aspx' where ID ='F15244'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_action='../tej-base/om_prt_acct.aspx' where ID ='F70228'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin70_MREP', text='More Checklists (Accounts)',submenuid='fin70_e2', web_action='../tej-base/moreReports.aspx' where ID in ('F70147')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin70_MREP', text= 'More Checklists(Accounts)', web_action='../tej-base/moreReports.aspx', form='fin70_a1',submenuid='fin70_e2' where ID in ('F70147')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view wbvu_pending_po_old as (select branchcd,TYPE,ordno,orddt,trim(AcodE) as Acode,ERP_code as Icode,max(del_Date) as del_date,max(Prate) as Prate,sum(Qtyord) as Qtyord,sum(Soldqty) as rcvdqty,sum(Qtyord)-sum(Soldqty) as Bal_Qty,fstr,max(amdtno) as amdtno,max(del_sch) as del_sch ,max(desc_) as desc_,max(ent_dt) As ent_dt,max(app_by) As app_by,max(app_dt) As app_dt from (SELECT TRIM(TYPE)||trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt)*(case when nvl(wk3,0)=0 then 1 else nvl(wk3,0) end )  as prate,acode,branchcd,ordno,orddt,del_Date,TYPE,amdtno,nvl(del_Sch,'-') as del_Sch,substr(nvl(desc_,'-'),1,100) as desc_,ent_by,ent_dt,app_by,app_dt from pomas where branchcd not in ('AM','DD') and type like '5%' and nvl(pflag,0)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT TRIM(potype)||trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+nvl(Rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate,null as del_date, poTYPE,null as amdtno,null as del_Sch,null as desc_,null as ent_by,null as ent_dt,null as app_by,null as app_dt  from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and type in ('02','03','07') and store in ('Y','N')) group by branchcd,TYPE,ordno,orddt,trim(AcodE),ERP_code,fstr having sum(Qtyord)-sum(Soldqty)>0)");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Item wise Production', web_action='../tej-base/om_prt_prodpp.aspx' where ID ='F40139'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Production Slip' where ID ='F40140'");

            //new DML COMMANDS 29.5.2018 ONWARD
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin47_MREP', web_action='../tej-base/om_prt_smktg.aspx', form='fin47_e3', submenuid='fin47_a1' where ID in ('F47228')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin47_MREP', web_action='../tej-base/om_prt_smktg.aspx', form='fin47_e3', submenuid='fin47_a1' where ID in ('F47229')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin47_MREP', web_action='../tej-base/om_prt_smktg.aspx', form='fin47_e3', submenuid='fin47_a1' where ID in ('F47230')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin47_MREP', web_action='../tej-base/om_prt_smktg.aspx', form='fin47_e3', submenuid='fin47_a1' where ID in ('F47231')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin47_MREP', web_action='../tej-base/om_prt_smktg.aspx', form='fin47_e3', submenuid='fin47_a1' where ID in ('F47232')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set param='fin47_MREP', web_action='../tej-base/om_prt_smktg.aspx', form='fin47_e3', submenuid='fin47_a1' where ID in ('F47233')");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin70_e3',WEB_ACTION='../tej-base/om_view_acct.aspx' where id in ('F70224','F70225')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_acct.aspx' where id in ('F70222','F70223','F70151','F70152')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin70_e2' where id in ('F70226','F70227')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set PRD='Y' where id in ('F70237','F70222', 'F70126','F70127','F70128','F70129','F70228','F25246P')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_acct.aspx' where id in ('F70132','F70133')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Debtors Ageing (detailed)- print', web_action='../tej-base/om_prt_acct.aspx' where ID ='F70270'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Creditors Ageing (detailed)- print', web_action='../tej-base/om_prt_acct.aspx' where ID ='F70271'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Trial Balance 4 Col' where id='F70151'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin70_e4', text='HSN wise FG Stock Summary-Print' where id='F70152'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='HSN Wise FG Stock Summary Checklist' where id='F70138'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_invn.aspx' where id in ('F25147','F25148','F25149')");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Pending Purchase Order Register With Line No.', prd='N' where id='F15142'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_purc.aspx' where id ='F15240'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Schedule (Day Wise) Checklist', prd='N' where id='F15306'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Vendor Wise 12 Month Rates Trend ( Max) Checklist' where id='F15312'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Item Wise 12 Month Rates Trend (Max) Checklist'where id='F15313'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "insert into typegrp(branchcd,id,type1,name)(select distinct '00','A',trim(bssch),'Sch-Name to be Updated '||trim(bssch) from famst where trim(bssch) not in (Select trim(type1) from typegrp where id='A')) ");
            fgen.execute_cmd(frm_qstr, frm_cocd, "insert into type(id,type1,name)(select distinct 'Z',trim(SUBSTR(ACODE,1,2)),'GRP-Name to be Updated '||trim(SUBSTR(ACODE,1,2)) from famst where trim(SUBSTR(ACODE,1,2)) not in (Select trim(type1) from type where id='Z')) ");

            //correction for forms
            // fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Bom Tree View',web_action='../tej-base/om_bom_tree.aspx' where id='F10160'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ASSETVCH", "EDT_BY");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ASSETVCH ADD EDT_BY VARCHAR(50) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ASSETVCH", "EDT_DT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ASSETVCH ADD EDT_DT DATE DEFAULT SYSDATE");



            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_prt_invn.aspx' where id ='F25235'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set WEB_ACTION='../tej-base/om_view_invn.aspx' where id ='F25231'");



            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME LIKE 'EXP_BOOK%'", "TNAME");
            if (mhd == "0" || mhd == "")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE EXP_BOOK (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE DEFAULT SYSDATE,COL1 VARCHAR(20) DEFAULT '-',COL2 VARCHAR(20) DEFAULT '-',COL3 VARCHAR(20) DEFAULT '-',COL4 VARCHAR(20) DEFAULT '-',COL5 VARCHAR(20) DEFAULT '-',COL6 VARCHAR(20) DEFAULT '-',COL7 VARCHAR(20) DEFAULT '-',COL8 VARCHAR(20) DEFAULT '-',COL9 VARCHAR(200) DEFAULT '-',COL10 VARCHAR(120) DEFAULT '-',COL11 VARCHAR(20) DEFAULT '-',COL12 VARCHAR(20) DEFAULT '-',COL13 VARCHAR(20) DEFAULT '-',COL14 VARCHAR(20) DEFAULT '-',COL15 VARCHAR(20) DEFAULT '-',SRNO NUMBER(5) DEFAULT 0,NUM1 NUMBER(12,2) DEFAULT 0,NUM2 NUMBER(12,2) DEFAULT 0,NUM3 NUMBER(12,2) DEFAULT 0,NUM4 NUMBER(12,2) DEFAULT 0,NUM5 NUMBER(12,2) DEFAULT 0,ENT_BY VARCHAR(20) DEFAULT '-',ENT_DT DATE DEFAULT SYSDATE,EDT_BY VARCHAR(20) DEFAULT '-',EDT_DT DATE DEFAULT SYSDATE)");
            }
        }


        mhd = fgen.chk_RsysUpd("DM0015");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0015','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0015", "DEV_A");


            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table FIN_MRSYS modify param varchar2(40) default '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table fin_rsys_opt modify opt_param varchar2(20) default '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table fin_rsys_opt modify opt_text varchar2(200) default '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_rsys_opt SET opt_text='01:MLD/02:SHMETAL/03:CAST/04:FORGE/05:PRT/06:CORR/07:PAINT/08:PHARMA/09:FOOD/10:CAPG/11:RUBB' where opt_id='W0000'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME like 'WB_FA_PUR'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_FA_PUR( BRANCHCD   CHAR(2) Default '-', TYPE   CHAR(2) Default '-', VCHNUM  CHAR(6) Default '-',VCHDATE DATE DEFAULT SYSDATE, GRP    CHAR(50) Default '-',GRPCODE    VARCHAR2(5) Default '-',ACODE   CHAR(10) Default '-',ASSETID  VARCHAR2(20) Default '-',IMAGEF    VARCHAR2(100) Default '-', IMAGEPATH    VARCHAR2(500) Default '-', ASSETNAME   VARCHAR2(100) Default '-', LOCN   VARCHAR2(30) Default '-', DCODE VARCHAR2(6) Default '-',owner varchar(5) Default '-',INSTDT DATE, LIFE_END DATE,LIFE  NUMBER(6,2) default 0,TOTLIFE    NUMBER(8,2) default 0, BALLIFE  NUMBER(8,2) default 0, USED_LIFE  NUMBER(8,2) default 0,BASICCOST  NUMBER(15,2) default 0, INSTALL_COST     NUMBER(13,2) default 0, CUSTOM_DUTY NUMBER(13,2) default 0, OTHER_CHRGS      NUMBER(13,2) default 0, ORIGINAL_COST  NUMBER(15,2) default 0, OP_DEP      NUMBER(15,2) default 0, DEPRPDAY   NUMBER(10,2) default 0, DEPRATE   NUMBER(5,2) default 0, DEPABLEVAL   NUMBER(15,2) default 0, RESIDVAL     NUMBER(15,2) default 0,  DOM_IMP   VARCHAR2(3) Default '-', TANGIBLE  VARCHAR2(3) Default '-', PURENTRY   VARCHAR2(3) Default '-', VOUCHERLINK  VARCHAR2(50) Default '-', ASSETSUPP    VARCHAR2(75) Default '-', ASSETSUPPADD   VARCHAR2(75) Default '-', INVNO    VARCHAR2(25) Default '-', INVDATE  DATE,QUANTITY  NUMBER(7,2) default 0, unit varchar2(20) default '-',WARRANTY VARCHAR2(2) Default '-',WARRANTY_DT varchar2(10) default '-' ,AMC VARCHAR2(1) Default '-', OTHER_REF VARCHAR2(100) Default '-', SALE_DT   VARCHAR2(10) Default '-', COL1 VARCHAR2(50) Default '-',COL2 VARCHAR2(10) Default '-',BLOCK CHAR(3) Default '-', ADDEP CHAR(1) Default '-',ADDDEPP NUMBER(5,2) default 0, ENT_BY  VARCHAR2(10) Default '-', ENT_DT  DATE DEFAULT SYSDATE, EDT_BY VARCHAR2(10) Default '-', EDT_DT   DATE DEFAULT SYSDATE)");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_FA_VCH'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_FA_VCH(BRANCHCD  CHAR(2) Default '-', TYPE CHAR(2) Default '-', VCHNUM  CHAR(6) Default '-', VCHDATE DATE, GRPCODE VARCHAR2(6) Default '-', ACODE  VARCHAR2(10) Default '-', DRAMT NUMBER(20,2), CRAMT  NUMBER(20,2), IQTYIN  NUMBER(20,2), IQTYOUT  NUMBER(20,2), INSTDT  DATE, INVNO  VARCHAR2(10) Default '-', INVDATE  DATE, NARATION  VARCHAR2(100) Default '-', ASSETVAL NUMBER(20,2), ASSETVAL1 NUMBER(20,2), DEPR_WBK NUMBER(20,2), DEPR_OLD  NUMBER(20,2), SALEVALUE  NUMBER(10,2), SRNO   NUMBER(5), DEPR   NUMBER(20,5), FVCHNUM  VARCHAR2(6) Default '-', FVCHDATE  DATE, MRR_REF  VARCHAR2(100) Default '-', SIR_REF   VARCHAR2(30) Default '-', INV_REF  VARCHAR2(30) Default '-', HO_REF  VARCHAR2(30) Default '-', DEPRDAYS NUMBER(10,2), DEPWBK VARCHAR2(3) Default '-',SALE_ENT  VARCHAR2(1) Default '-', IUNIT  VARCHAR2(5) Default '-', SALE_DT varchar2(10) Default '-' ,more180 number(20,2) default 0,less180 number(20,2) default 0,sale_it number(20,2) default 0, block char(3) default'-',ENT_BY VARCHAR2(50) Default '-', ENT_DT DATE,EDT_BY VARCHAR2(50) Default '-',EDT_DT DATE)");


            // tables for app in klassic(nfc)
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SA_CARD'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SA_CARD(BRANCHCD CHAR(2),TYPE  CHAR(2),VCHNUM  VARCHAR2(6),VCHDATE  DATE,LOCATION  VARCHAR2(50),CARD_NO  VARCHAR2(30),REMARKS  VARCHAR2(100),ENT_BY  VARCHAR2(50),ENT_DT DATE)");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SA_ROUTE'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SA_ROUTE(BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR2(6),VCHDATE  DATE,ROUTE_NAME VARCHAR2(50),CARD_NO  VARCHAR2(30),TIME  VARCHAR2(20),ENT_BY VARCHAR2(50),ENT_DT DATE, SRNO NUMBER(38),FLAG   CHAR(1),LOCATION  VARCHAR2(50))");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SA_RECORD'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SA_RECORD(BRANCHCD  CHAR(2),TYPE  CHAR(2),VCHNUM   VARCHAR2(6),VCHDATE DATE,ROUTE_NAME VARCHAR2(50),CARD_NO    VARCHAR2(30),TIME VARCHAR2(20),ENT_BY VARCHAR2(50),ENT_DT   DATE,IMEI  VARCHAR2(50),FLAG  VARCHAR2(1))");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SA_MAIL'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SA_MAIL(branchcd char(2),type char(2),vchnum varchar2(6),vchdate date,emailid varchar2(50),msgto varchar2(50),msg_text varchar2(200),msgdt date,ent_by varchar2(20),ent_dt date)");
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SA_IMG'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SA_IMG(branchcd char(2),type char(2),vchnum varchar2(6),vchdate date,username varchar2(20),imagepath varchar2(50),ent_by varchar2(50), ent_dt date)");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_FA_PUR", "COL1");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_FA_PUR ADD COL1 VARCHAR(50) DEFAULT '-'");
            else
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_FA_PUR MODIFY COL1 VARCHAR(50) DEFAULT '-'");
            }
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "APP_BY");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD APP_BY  VARCHAR2(20) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "APP_DT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD APP_DT  DATE DEFAULT SYSDATE");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "INVNO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD INVNO  VARCHAR2(20) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "INVDATE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD INVDATE  DATE DEFAULT SYSDATE");


            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "FININFO", "TERMINAL");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE FININFO  MODIFY TERMINAL VARCHAR2(100) DEFAULT '-'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_prtg_entry.aspx' WHERE ID ='F40101'");



            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE VCHNUM='000000' AND OPT_ENABLE='Y'", "OPT_PARAM");
            //if (mhd == "06")
            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CORRCST_RCTM'", "TNAME");// type= '^1'
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CORRCST_RCTM (BRANCHCD VARCHAR2(2) DEFAULT '-', type VARCHAR2(2) DEFAULT '-', VCHNUM VARCHAR2(6) DEFAULT '-',VCHDATE DATE, CODE  VARCHAR2(2) DEFAULT '-',BF VARCHAR2(10) DEFAULT '-',HRCTRT VARCHAR2(10) DEFAULT '-',NRCTRT VARCHAR2(10) DEFAULT '-',NRCTI VARCHAR2(10) DEFAULT '-',HRCTI VARCHAR2(10) DEFAULT '-',REM   VARCHAR2(50) DEFAULT '-',ENT_BY  VARCHAR2(30) DEFAULT '-',ENT_DT DATE,EDT_BY   VARCHAR2(30) DEFAULT '-',EDT_DT DATE,GSM  NUMBER(13,3) DEFAULT 0)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CORRCST_LAYER'", "TNAME");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CORRCST_LAYER (CODE VARCHAR2(2),SRNO  VARCHAR2(2),VCHNUM VARCHAR2(6),VCHDATE DATE,TRANNUM  VARCHAR2(10),TRANDT DATE,GSM VARCHAR2(5),BF VARCHAR2(5),RCTGRADE VARCHAR2(5),RCT VARCHAR2(10),T_RCT VARCHAR2(5),COST VARCHAR2(10),HRCTI VARCHAR2(5),NRCTI  VARCHAR2(5),CORRECTINDEX VARCHAR2(5),TF VARCHAR2(5),HRCTR VARCHAR2(5),NRCTR  VARCHAR2(5),CORRECTPAPERRATE  VARCHAR2(5),FACTOR  VARCHAR2(10),COSTPERBOX  VARCHAR2(10),ENT_BY  VARCHAR2(30),ENT_DT DATE,EDT_BY VARCHAR2(30),EDT_DT DATE,REM    VARCHAR2(50),COL1   VARCHAR2(30),TOTCOST VARCHAR2(7),TOTRCT VARCHAR2(5),DESC_ VARCHAR2(30))");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CORRCST_CONVC'", "TNAME");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CORRCST_CONVC(CODE   VARCHAR2(2),SRNO   VARCHAR2(2),VCHNUM VARCHAR2(6),VCHDATE DATE,TRANNUM VARCHAR2(10),TRANDT DATE,RATE VARCHAR2(10),FLAG   VARCHAR2(1),DESC_  VARCHAR2(20),AMT    VARCHAR2(5),ENT_BY VARCHAR2(30),ENT_DT DATE,EDT_BY VARCHAR2(30),EDT_DT DATE,REM    VARCHAR2(50),COL1   VARCHAR2(30))");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CORRCST_FLUTEM'", "TNAME");// type= '^4'= paper flute, type='^7' as box type;                
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CORRCST_FLUTEM( BRANCHCD VARCHAR2(2) DEFAULT '-',TYPE CHAR(2) DEFAULT '-', VCHNUM VARCHAR2(6) DEFAULT '-',VCHDATE DATE,FLUTE  VARCHAR2(10) DEFAULT '-', CALIPER VARCHAR2(20) DEFAULT '-', REM VARCHAR2(50) DEFAULT '-',IND1 NUMBER(8,2), IND2 NUMBER(8,2),BOXTYPECODE VARCHAR2(20) DEFAULT '-',NAME VARCHAR2(100) DEFAULT '-',AREA VARCHAR2(100) DEFAULT '-',IMAGE VARCHAR2(100) DEFAULT '-', IMAGEPATH VARCHAR2(150) DEFAULT '-',ENT_BY VARCHAR2(30) DEFAULT '-', ENT_DT DATE, EDT_BY VARCHAR2(30) DEFAULT '-', EDT_DT DATE)");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CORRCST_TRANS'", "TNAME");// type= '^5'
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CORRCST_TRANS (BRANCHCD VARCHAR2(2) DEFAULT '-', type char(2) DEFAULT '-', CODE VARCHAR2(2) DEFAULT '-',ANAME VARCHAR2(30) DEFAULT '-',INAME VARCHAR2(50) DEFAULT '-',VCHNUM VARCHAR2(6) DEFAULT '-',VCHDATE DATE,TRANNUM VARCHAR2(6) DEFAULT '-',TRANDT DATE,LT VARCHAR(5) DEFAULT '-',WD VARCHAR(10) DEFAULT '-',HT VARCHAR (10) DEFAULT '-',PLY VARCHAR2(5) DEFAULT '-',FLUTE VARCHAR2(10) DEFAULT '-',CS VARCHAR2(10) DEFAULT '-',CALIPER VARCHAR2(10) DEFAULT '-',Z VARCHAR2(5) DEFAULT '-',RQECT VARCHAR2(10) DEFAULT '-',RQBS VARCHAR2(10) DEFAULT '-',RQGSM VARCHAR2(10) DEFAULT '-',DECKLE VARCHAR2(10) DEFAULT '-',LENGTH VARCHAR2(10) DEFAULT '-',AREA VARCHAR2(10) DEFAULT '-',LENGTHWIDTHRATIO VARCHAR2(10) DEFAULT '-',DEPTHFACTOR VARCHAR2(10) DEFAULT '-',L_W_FACTOR VARCHAR2(10) DEFAULT '-',NET_FACTOR VARCHAR2(10) DEFAULT '-',MINECT VARCHAR2(10) DEFAULT '-',MAXECT VARCHAR2(10) DEFAULT '-',AVGECT VARCHAR2(10) DEFAULT '-',MINCS VARCHAR2(10) DEFAULT '-',MAXCS VARCHAR2(10) DEFAULT '-',AVGCS VARCHAR2(10) DEFAULT '-',MINGSM VARCHAR2(10) DEFAULT '-',MAXGSM VARCHAR2(10) DEFAULT '-',AVGGSM VARCHAR2(10) DEFAULT '-',MINBS VARCHAR2(10) DEFAULT '-',MAXBS VARCHAR2(10) DEFAULT '-',AVGBS VARCHAR2(10) DEFAULT '-',MINWT VARCHAR2(10) DEFAULT '-',MAXWT VARCHAR2(10) DEFAULT '-',AVGWT VARCHAR2(10) DEFAULT '-',CONTRIBUTION VARCHAR2(10) DEFAULT '-',CONTAMT VARCHAR2(10) DEFAULT '-',TCONCST VARCHAR2(10) DEFAULT '-',CSTPKG VARCHAR2(10) DEFAULT '-',PAPCST VARCHAR2(10) DEFAULT '-',PAWASTAGE VARCHAR2(10) DEFAULT '-',PAWASTAGEAMT VARCHAR2(10) DEFAULT '-',BOXCOST VARCHAR2(10) DEFAULT '-',ENT_BY VARCHAR2(30) DEFAULT '-',ENT_DT DATE,EDT_BY VARCHAR2(30) DEFAULT '-',EDT_DT DATE,REM VARCHAR(100) DEFAULT '-',COL1 VARCHAR2(30) DEFAULT '-',H_16 NUMBER(13,3), N_16 NUMBER(13,3), N_18  NUMBER(13,3), H_18 NUMBER(13,3), H_20 NUMBER(13,3), N_20  NUMBER(13,3), N_22  NUMBER(13,3), H_22   NUMBER(13,3), N_24  NUMBER(13,3), H_24  NUMBER(13,3), H_28   NUMBER(13,3), N_28  NUMBER(13,3), N_35    NUMBER(13,3), H_35  NUMBER(13,3), H_45   NUMBER(13,3), N_45   NUMBER(13,3))");

                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_CORRCST_CSBS'", "TNAME");// type= '^6'
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_CORRCST_CSBS(BRANCHCD VARCHAR2(2) DEFAULT '-',type char(2) DEFAULT '-', VCHNUM VARCHAR2(6) DEFAULT '-',VCHDATE  DATE,CUSTNAME VARCHAR2(150) DEFAULT '-',ITEMNAME VARCHAR2(100) DEFAULT '-',GROSSWT NUMBER(13,3),STCKHGT  NUMBER(13,3),NOOFBOXES NUMBER(13,3),LOADBOX NUMBER(13,3),STORAGETM_DAYS NUMBER(13,3),STORAGETM_VAL NUMBER(13,3),HUMIDPERCNT NUMBER(13,3),HUMIDVALUE  NUMBER(13,3),COLUMNALGND  NUMBER(13,3),COLUMNMISALGND NUMBER(13,3),INTERLOCKD NUMBER(13,3),OVERHANGED  NUMBER(13,3),DECKBOARD_GAP NUMBER(13,3),EXCESHND NUMBER(13,3),TOT_ENVR_FAC  NUMBER(13,3),REQUIRDBCT  NUMBER(13,3),BOXTYPECODE varchar2(4),LENGTH NUMBER(13,3),WIDTH NUMBER(13,3),HEIGHT NUMBER(13,3),NO_OF_PLIES NUMBER(1),FLUTE_PROFILE VARCHAR2(5),MANF_PROCES VARCHAR2(1),BOARD_CALLIPR NUMBER(13,3),AREA  NUMBER(13,3),REQ_ECT NUMBER(13,3),REQ_RCT  NUMBER(13,3),TOP_PERC NUMBER(13,3),LINER_PERC NUMBER(13,3),FLUTE_PERC NUMBER(13,3),TOPLINER_RCT  NUMBER(13,3),FLUTE1_RCT  NUMBER(13,3),MIDLINER_RCT  NUMBER(13,3),FLUTE2_RCT NUMBER(13,3),INNERLINER_RCT NUMBER(13,3),DEP_FAC_RCT  NUMBER(13,3),TOPLINER_BF NUMBER(13,3),FLUTE1_BF NUMBER(13,3),MIDLINER_BF NUMBER(13,3),FLUTE2_BF NUMBER(13,3),INNERLINER_BF NUMBER(13,3),DEP_FAC_BF NUMBER(13,3),TOPLINER_GSM_BF  NUMBER(13,3),FLUTE1_GSM_BF NUMBER(13,3),MIDLINER_GSM_BF NUMBER(13,3),FLUTE2_GSM_BF NUMBER(13,3),INNERLINER_GSM_BF NUMBER(13,3),DEP_FAC_GSM_BF NUMBER(13,3),TOPLINER_GSM  NUMBER(13,3),FLUTE1_GSM NUMBER(13,3),MIDLINER_GSM NUMBER(13,3),FLUTE2_GSM NUMBER(13,3),INNERLINER_GSM NUMBER(13,3),DEP_FAC_GSM NUMBER(13,3),TOT_BOARD_GSM NUMBER(13,3),TOT_WGHT_CAR NUMBER(13,3),TOT_BOARD_BS NUMBER(13,3),TOT_CS NUMBER(13,3),TOT_RCT_GSM_BF NUMBER(13,3),TOT_ECT_GSM_BF NUMBER(13,3),DIFF_REQ_ECT  NUMBER(13,3),DESC_ VARCHAR2(150),COL1  VARCHAR2(100),REMARKS  VARCHAR2(200),ENT_BY  VARCHAR2(50),ENT_DT DATE,EDT_BY VARCHAR2(50),EDT_DT DATE,NARATION  VARCHAR2(100),REQUIREDSFAC  NUMBER(13,3),ALIGYN  CHAR(1),MISALIGYN CHAR(1),INTERYN  CHAR(1),OVHGYN  CHAR(1),DCKGPYN CHAR(1),EXVHDYN  CHAR(1))");

                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CORRCST_RCTM", "CODE");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CORRCST_RCTM MODIFY CODE VARCHAR2(15) DEFAULT '-'");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CORRCST_LAYER", "TOTCOST");
                if (mhd != "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CORRCST_LAYER MODIFY TOTCOST VARCHAR2(10) DEFAULT '-'");

            }
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set mlevel='3',param='-' WHERE ID IN ('F15141','F15140','F15142','F15250','F15314','F15138','F15302','F15304','F15305','F15307','F15303')");

            //commands 1/10/2018 onwards

            mhd = "update FIN_MSYS set BNR='Y' where upper(text) like '%REQUEST%'";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_bom_tree.aspx' WHERE ID ='F10160'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_LEAD_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_Lead_LOG(branchcd char(2),type char(2),LRCNO char(6),LRCDT date,Lead_dsg char(20) default '-',LVertical varchar2(30) default '-',Ldescr varchar2(30) default '-',lgrade varchar2(30) default '-',lsubject varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',Lead_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_LOG", "LEAD_CLOSE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_lead_LOG ADD LEAD_CLOSE VARCHAR2(1) DEFAULT '-'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_LEAD_ACT'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_Lead_ACT(branchcd char(2),type char(2),LACNO char(6),LACDT date,LRCNO char(6),LRCDT date,Lead_dsg char(20) default '-',LVertical varchar2(30) default '-',Ldescr varchar2(30) default '-',lgrade varchar2(30) default '-',lsubject varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Input_from varchar2(20) DEFAULT '-',Act_mode varchar2(10) DEFAULT '-',Next_Folo number(5) DEFAULT 0,Oremarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_LEAD_ACT", "CURR_STAT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_lead_ACT ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CCM_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CCM_LOG(branchcd char(2),type char(2),CCMNO char(6),CCMDT date,Cust_NAME varchar2(80) default '-',comp_type varchar2(30) default '-',Cdescr varchar2(30) default '-',Compcatg varchar2(30) default '-',compOccr varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',CCM_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            if (frm_cocd == "MLGI" || frm_cocd == "YTEC")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ITWSTAGE_new", "area");
                if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ITWSTAGE_new  ADD AREA CHAR(2) DEFAULT '-',CAVITY_PC NUMBER(15,2) DEFAULT 0,OP_RATE NUMBER(15,2) DEFAULT 0,NO_MAN NUMBER(15,2) DEFAULT 0");
            }

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "CCM_CLOSE");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CCM_LOG ADD CCM_CLOSE VARCHAR2(1) DEFAULT '-'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CCM_LOG'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CCM_LOG(branchcd char(2),type char(2),CCMNO char(6),CCMDT date,Cust_NAME varchar2(80) default '-',comp_type varchar2(30) default '-',Cdescr varchar2(30) default '-',Compcatg varchar2(30) default '-',compOccr varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',CCM_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CCM_LOG", "CURR_STAT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CCM_LOG ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");


        }
        //SHURU
        //new DML COMMANDS 04.11.2018 ONWARD                
        mhd = fgen.chk_RsysUpd("DM0016");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('DM0016','DEV_A',sysdate)");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DM0016", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Fixed Assets-Masters' WHERE ID ='F70427'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Fixed Asset Sale/Disposal Activity' WHERE ID ='F70403'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Depreciation Calculation-Companies Act' WHERE ID ='F70404'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set mlevel=4 , param='fin70_MREPfam' WHERE ID ='F70405'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Assets Revaluation Activity' WHERE ID ='F70423'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Fixed Asset Purchase Activity' WHERE ID ='F70402'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Depreciation Calculation-IT Block wise' WHERE ID ='F70424'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Fixed Asset-Register & Reports' WHERE ID ='F70429'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='List of Additions- Fixed Assets' WHERE ID ='F70409'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Depreciation Chart- Asset Code wise' WHERE ID ='F70406'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='Fixed Assets-Checklists' WHERE ID ='F70430'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_rsys_opt WHERE OPT_ID  in ('W2101','W2102') and OPT_text like '%FA MODULE%'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_rsys_opt WHERE OPT_ID  = 'W2100' and OPT_text like '%DEPRECIATION%'");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_rsys_opt_pw WHERE OPT_ID  in ('W2101','W2102') and OPT_text like '%FA MODULE%'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_rsys_opt_pw WHERE OPT_ID  = 'W2100' and OPT_text like '%DEPRECIATION%'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_action='../tej-base/om_fa_upload.aspx' WHERE ID ='F70405'");
        }


    }
}



