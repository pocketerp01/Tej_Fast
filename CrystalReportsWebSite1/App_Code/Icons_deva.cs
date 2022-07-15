using System;


public class Icons_DevA
{
    fgenDB fgen = new fgenDB();
    Create_Icons ICO = new Create_Icons();
    public void add(string frm_qstr, string frm_cocd)
    {
        string mhd;
        ICO.Cls_comp_code = frm_cocd;
        string MV_CLIENT_GRP = "";
        MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP");

        Opts_wfin WFIN_opts = new Opts_wfin();
        WFIN_opts.Icon_SysConfig(frm_qstr, frm_cocd);
        WFIN_opts.Upd_SYSOPT(frm_qstr, frm_cocd);        
        WFIN_opts.Icon_Mgmt(frm_qstr, frm_cocd);
        WFIN_opts.iconApprovals(frm_qstr, frm_cocd);

        switch (frm_cocd)
        {
            // case "BUPL":
            case "MLGI":
            case "ALIN":
            case "ANYG":
            case "DISP*":
            case "DLJM":
            case "PCJS":
            case "RELI":
            case "SRPF":
            case "SINT*":
            case "PKGW":
            case "TEST":
            case "AGRM":
            case "DREM":
            case "MCPL*":
            //case "SDM":
            case "SAGM":
            case "PCEE*":
            case "SAGI":
            case "KUNS*":
            case "MTPL":
            case "IPP":
            case "MULT":
            case "NITP":
            case "HENA":
            case "BNPL":
            case "KPAC":
            case "MLGA":
            case "HPPI":
            case "SPPI":
            case "DEMO":
            case "VCL":
            case "SGRP":
            case "UATS":
            case "UAT2":
                WFIN_opts.Icon_Engg(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Purch(frm_qstr, frm_cocd);
                WFIN_opts.Icon_gate(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Store(frm_qstr, frm_cocd);
                WFIN_opts.Icon_Qlty(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Ppc_paper(frm_qstr, frm_cocd);
                WFIN_opts.Icon_Prodn_paper(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Prodn_plast(frm_qstr, frm_cocd);
                //WFIN_opts.Icon_Prodn_Metal(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Mkt_ord(frm_qstr, frm_cocd);
                WFIN_opts.Icon_Mkt_ord_Exp(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Mkt_Sale(frm_qstr, frm_cocd);
                WFIN_opts.Icon_Mkt_Sale_Exp(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Acctg(frm_qstr, frm_cocd);
                WFIN_opts.Icon_FA_sys(frm_qstr, frm_cocd);


                //WFIN_opts.Icon_Hrm(frm_qstr, frm_cocd);
                if (MV_CLIENT_GRP == "SG_TYPE")
                {
                    WFIN_opts.Icon_Payr(frm_qstr, frm_cocd);

                }



                WFIN_opts.Icon_Cust_port(frm_qstr, frm_cocd);
                // WFIN_opts.Icon_Supp_port(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Maint(frm_qstr, frm_cocd);
                WFIN_opts.Icon_Taskmgt(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Crm(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Visitor(frm_qstr, frm_cocd);

                //WFIN_opts.Icon_Mgmt(frm_qstr, frm_cocd);

                //WFIN_opts.Icon_security_nfc(frm_qstr, frm_cocd);
                //---------------------------------------------------------

                //removed 22/08/2020 : not for all
                //WFIN_opts.Icon_DrCr_Toyota(frm_qstr, frm_cocd);
                //removed 22/08/2020 : not for all    
                //WFIN_opts.Icon_DrCr_self(frm_qstr, frm_cocd);


                mhd = fgen.chk_RsysUpd("IC0021");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('IC0021','DEV_A',sysdate)");
                    ICO.add_icon(frm_qstr, "F40999", 4, "MIT", 3, "../tej-base/om_dbd_bpln2.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F70120", 3, "Auto Debit Credit Note(Vendor)", 3, "../tej-base/findCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F47240", 4, "Payment Advice", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F35108", 4, "Order / Line Planning", 3, "../tej-base/om_mcplan.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");

                    WFIN_opts.iconInvMrrUpload(frm_qstr, frm_cocd);
                    ICO.add_icon(frm_qstr, "F15212", 3, "P.O. Approval Level Master", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
                }

                //if (frm_cocd == "SDM" || frm_cocd == "DLJM")
                {
                    Opts_wfin sdm_icons = new Opts_wfin();
                    sdm_icons.ProfitabilityReport(frm_qstr, frm_cocd);
                    //removed 22/08/2020 : not for all
                    //sdm_icons.Icon_DrCr_Maruti(frm_qstr, frm_cocd);                    
                }


                WFIN_opts.Icon_TaskmgtWP(frm_qstr, frm_cocd);

                WFIN_opts.iconFinanceVoucherUpload(frm_qstr, frm_cocd);
                WFIN_opts.iconInvMrrUpload(frm_qstr, frm_cocd);

                WFIN_opts.Icon_Hrm(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F05199A", 3, "Multi-Department Live Charts 2", 3, "../tej-base/deskdash2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F47120", 4, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47124", 4, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F50137A", 4, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F50137B", 4, "Truck Entry Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F47125", 4, "Supervisior Master", 3, "../tej-base/personmst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
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

                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F40171", 3, "Label Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");


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

                WFIN_opts.Icon_Maint(frm_qstr, frm_cocd);
                WFIN_opts.IconMouldMaint(frm_qstr, frm_cocd);

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
                ICO.add_icon(frm_qstr, "F60000", 1, "Customer Support System", 3, "-", "-", "Y", "-", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F93000", 2, "Tejaxo OMS", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F93100", 3, "OMS Activity", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F93101", 4, "OMS Plan", 3, "../tej-base/om_oms_Plan.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F93106", 4, "OMS Followup", 3, "../tej-base/om_oms_folo.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F93116", 3, "OMS Reports", 3, "-", "-", "Y", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F93121", 4, "OMS Person Wise ", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93126", 4, "OMS Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93131", 4, "OMS Tgt VS Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F93132", 4, "OMS Team Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F93133", 4, "OMS Client Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e1", "fin60_a1", "fin93pp_e2", "fa-edit");

                //WFIN_opts.IconCustomerRequestSELStyle(frm_qstr, frm_cocd);                
                break;

            case "PPAP":
                ICO.add_icon(frm_qstr, "P70000", 1, "Finance", 3, "-", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70100", 2, "Accounting Activity", 3, "-", "-", "Y", "finfina_r1", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "P70099", 2, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70099a", 2, "Toyota/All Inv File Uploading", 3, "../tej-base/fupl1.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P70099R", 2, "Voucher Verify", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Marketing", 3, "-", "-", "Y", "finsmktg_s", "finsmktg", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50035", 2, "ISD Invoice Entry", 3, "../tej-base/om_inv.aspx", "-", "-", "finsmktg_s", "finsmktg", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50024", 2, "Royality Report", 1, "../tej-base/rpt.aspx", "-", "-", "finsmktg_s", "finsmktg", "-", "fa-edit", "Y", "Y");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25219", 3, "Item Master Min/Max/ROL update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                break;

            case "TGIP":
            case "PTI":
                ICO.add_icon(frm_qstr, "P70000", 1, "Finance", 3, "-", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70100", 2, "Accounting Activity", 3, "-", "-", "Y", "finfina_r1", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "P70099", 2, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70099a", 2, "Toyota/All Inv File Uploading", 3, "../tej-base/fupl1.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P70099R", 2, "Voucher Verify", 3, "../tej-base/om_view_acct.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Marketing", 3, "-", "-", "Y", "finsmktg_s", "finsmktg", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50035", 2, "ISD Invoice Entry", 3, "../tej-base/om_inv.aspx", "-", "-", "finsmktg_s", "finsmktg", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50024", 2, "Royality Report", 1, "../tej-base/rpt.aspx", "-", "-", "finsmktg_s", "finsmktg", "-", "fa-edit", "Y", "Y");
                break;

            case "ANYG*":
                // for gate inward module 
                Opts_wfin gate_icons = new Opts_wfin();
                gate_icons.Icon_gate(frm_qstr, frm_cocd);
                gate_icons.ProfitabilityReport(frm_qstr, frm_cocd);
                break;

            case "IPP*":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                Opts_wfin ipp_icons = new Opts_wfin();
                ipp_icons.Icon_DrCr_self(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "P70099h", 3, "All Inv File Uploading", 3, "../tej-base/autoDrCrPip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");

                ipp_icons.ProfitabilityReport(frm_qstr, frm_cocd);


                break;
            case "SVPL":
                mhd = fgen.chk_RsysUpd("IC0020");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "insert into FIN_RSYS_UPD values ('IC0020','DEV_A',sysdate)");

                    Opts_wfin Ico_wfin1 = new Opts_wfin();
                    Ico_wfin1.Icon_gate(frm_qstr, frm_cocd);


                    // ------------------------------------------------------------------
                    // Inventory Module
                    // ------------------------------------------------------------------
                    ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F25101", 3, "Matl Inward Entry", 3, "../tej-base/om_mrr_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F25121", 2, "Inventory Checklists", 3, "-", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F25126", 3, "Matl Inward Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");

                    ICO.add_icon(frm_qstr, "F25131", 2, "Stock Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F25132", 3, "Stock Ledger", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F25133", 3, "Stock Summary", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F25134", 3, "Stock Min-Max", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");

                    ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F25141", 3, "Matl Inward Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                    //********
                    ICO.add_icon(frm_qstr, "F25245A", 3, "FG Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F25245R", 3, "Return Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F25245RA", 3, "Return Sticker 2", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F25245S", 3, "Rcv Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

                    ICO.add_icon(frm_qstr, "F25149B", 3, "Pending for Bond", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "Y", "Y");
                    ICO.add_icon(frm_qstr, "F25149S", 3, "Pending for Bond Summ", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "Y", "Y");

                    ICO.add_icon(frm_qstr, "F25150", 3, "Pending for QA", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "Y", "Y");
                    ICO.add_icon(frm_qstr, "F25150S", 3, "Pending for QA Summ", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "Y", "Y");

                    ICO.add_icon(frm_qstr, "F39131U", 3, "User ID Card", 3, "../tej-base/rpt.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                }

                ICO.add_icon(frm_qstr, "F25151", 3, "Pending for Return", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "Y", "Y");
                ICO.add_icon(frm_qstr, "F25151L", 3, "Return List", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "Y", "Y");

                Opts_wfin drcr_maruti_icons1 = new Opts_wfin();
                drcr_maruti_icons1.Icon_DrCr_Maruti(frm_qstr, frm_cocd);
                break;
            case "LOGW":
            case "ROOP":
            case "MEGA":
            case "PPPF":
                // for maruti , self data dr cr notes
                Opts_wfin drcr_maruti_icons = new Opts_wfin();
                drcr_maruti_icons.Icon_DrCr_Maruti(frm_qstr, frm_cocd);
                break;
            case "SKHM":
                Opts_wfin drcr_maruti_icons_1 = new Opts_wfin();
                drcr_maruti_icons_1.Icon_DrCr_Maruti(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70120", 3, "Auto Debit Credit Note(Vendor)", 3, "../tej-base/findCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70110C", 3, "Credit Note(Vendor)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "P70110D", 3, "Debit Note(Vendor)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                break;
            case "SUNB":
            case "PPIN":
            case "PPI":
                // for self data dr cr notes    
                Opts_wfin drcr_Self_icons = new Opts_wfin();
                drcr_Self_icons.Icon_DrCr_self(frm_qstr, frm_cocd);
                break;
            case "BONY":
            case "SFAB":
            case "ARVI":
                // for toyota file , self data dr cr notes    
                Opts_wfin drcr_toyo_icons = new Opts_wfin();
                //drcr_toyo_icons.Icon_DrCr_Toyota(frm_qstr, frm_cocd);
                drcr_toyo_icons.Icon_DrCr_Honda(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70120", 3, "Auto Debit Credit Note(Vendor)", 3, "../tej-base/findCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;

            case "SAIP":
            case "SAIL":
                mhd = fgen.chk_RsysUpd("SAIP1");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('SAIP1') ");

                    ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "P70099h", 3, "All Inv File Uploading", 3, "../tej-base/autoDrCrPip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET WEB_ACTION='../tej-base/autoDrCrSaip.aspx' WHERE ID='P70099h'");

                    ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "Y", "N");
                    ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "Y", "N");
                }
                ICO.add_icon(frm_qstr, "F70118B", 3, "Bajaj Auto DN/CN", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70107C", 3, "Bajaj Debit Note Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "P70107D", 3, "Bajaj Credit Note Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F50051", 2, "Invoice Checking", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin50_e1X", "fin50_a1", "-", "fa-edit");
                break;

            case "MEGH":
                // ------------------------------------------------------------------
                // Inventory Module
                // ------------------------------------------------------------------
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25211", 2, "Reel Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25212", 3, "Reel Stock", 3, "rpt.aspx", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25213", 3, "Reel Stock Summ", 3, "rpt.aspx", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F40116", 1, "QC Reason Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
                break;

            case "PPPL":
            case "PIPL":
            case "NIRM":
                // for Honda file , self data dr cr notes    
                Opts_wfin drcr_honda_icons = new Opts_wfin();
                drcr_honda_icons.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "P70099S", 3, "Upload Old Invoice DR/CR", 3, "../tej-base/autoDrCrSaip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;

            case "CLPL":
            case "SPIR":
                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50150", 4, "Daily Sales Summary Person Wise", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F50149", 4, "Sales Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50155", 4, "Collection Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F50157", 4, "Buying House", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25214", 3, "Multi Item Master", 3, "../tej-base/om_multi_item.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F20101", 3, "Gate Inward Entry", 3, "../tej-base/om_gate_inw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                break;
            case "RRP":
            case "ADMC":
                // for Honda file , self data dr cr notes    
                Opts_wfin drcr_honda_icons1 = new Opts_wfin();
                drcr_honda_icons1.Icon_DrCr_Honda(frm_qstr, frm_cocd);
                break;

            case "ERAL":
                // for self data dr cr notes
                Opts_wfin drcr_Self_icons1 = new Opts_wfin();
                drcr_Self_icons1.Icon_DrCr_self(frm_qstr, frm_cocd);

                // for payment advice
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
                ICO.add_icon(frm_qstr, "F50266", 4, "Mat lying with godown-invoice wise summary", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50267", 4, "Mat lying with godown-item wise detail", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50268", 4, "Mat lying with godown-item wise summary", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                break;


            case "SEL":
                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
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
                ICO.add_icon(frm_qstr, "F25217", 3, "Item Master Balance update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
                break;
            case "VAPL":
                mhd = fgen.chk_RsysUpd("IC0001");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('IC0001') ");
                    ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F35500", 2, "Monitoring Report", 3, "-", "-", "Y", "fin35_e5", "fin35_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F35506", 3, "Extrusion Report", 3, "../tej-base/mkt_ppc1.aspx", "-", "Y", "fin35_e5", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F35507", 3, "Cutting Report", 3, "../tej-base/mkt_ppc1.aspx", "-", "Y", "fin35_e5", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F35508", 3, "Packing Report", 3, "../tej-base/mkt_ppc1.aspx", "-", "Y", "fin35_e5", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F35509", 3, "Production Performance Report", 3, "../tej-base/mkt_ppc1.aspx", "-", "Y", "fin35_e5", "fin35_a1", "-", "fa-edit");

                }
                break;
            case "GCAP":
            case "GDOT":
            case "SEFL":
                // for gate records
                Opts_wfin Ico_wfin2 = new Opts_wfin();
                Ico_wfin2.Icon_gate(frm_qstr, frm_cocd);
                // for accounts st of a/c 
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F70501", 2, "Premium options- Finance/Acctg", 3, "-", "-", "Y", "fin70_e8", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70502", 3, "Customised Reports-Finance/Acctg", 3, "-", "-", "Y", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit");
                //ICO.add_icon(frm_qstr, "F70505", 4, "Sale Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y"); report not made
                ICO.add_icon(frm_qstr, "F70506", 4, "Sale Party wise Item wise", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70507", 4, "Sale Item wise Party wise", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70508", 4, "Purchase Party wise Item wise", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70509", 4, "Purchase Item wise Party wise", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y");
                break;

            case "PRPL":
                // for fixed assets records        
                Opts_wfin Ico_PRPL = new Opts_wfin();
                Ico_PRPL.Icon_FA_sys(frm_qstr, frm_cocd);
                break;

            case "SFL2":
            case "SFLG":
                // dr cr note module
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70099h", 3, "All Inv File Uploading", 3, "../tej-base/autoDrCrPip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "15000", 1, "Manufacturing", 1, "-", "-", "-", "-", "MANU", "-", "-", "N", "N");
                ICO.add_icon(frm_qstr, "15100", 2, "Reports", 1, "-", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                ICO.add_icon(frm_qstr, "15192", 3, "Production Schedule Status", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                ICO.add_icon(frm_qstr, "15193", 3, "Plan / Daily Production Report", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "Y");
                ICO.add_icon(frm_qstr, "15194", 3, "Jobwork Pending List Item Wise", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                ICO.add_icon(frm_qstr, "15195", 3, "Jobwork Pending Summary(Detail)", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                ICO.add_icon(frm_qstr, "15196", 3, "Jobwork Pending Summary", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                ICO.add_icon(frm_qstr, "15197", 3, "Jobwork Pending Summary Challan Wise", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                break;
            case "SFL1":
                // dr cr note module
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70099h", 3, "All Inv File Uploading", 3, "../tej-base/autoDrCrPip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70099", 3, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P70100", 2, "Accounting Activity", 3, "-", "-", "Y", "finfina_r1", "finfina_r", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");

                ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
                break;

            case "KPACX":
                // for gate records
                Opts_wfin Ico_KPAC = new Opts_wfin();
                Ico_KPAC.Icon_gate(frm_qstr, frm_cocd);
                Ico_KPAC.Icon_Store(frm_qstr, frm_cocd);
                Ico_KPAC.Icon_Cust_port(frm_qstr, frm_cocd);
                Ico_KPAC.Icon_Supp_port(frm_qstr, frm_cocd);
                Ico_KPAC.Icon_Mkt_ord(frm_qstr, frm_cocd);
                Ico_KPAC.IconCustomerRequestSELStyle(frm_qstr, frm_cocd);
                Ico_KPAC.icon_ppc_prodReports(frm_qstr, frm_cocd);
                //Ico_KPAC.Icon_Ppc_paper(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35108", 4, "Order / Line Planning", 3, "../tej-base/om_mcplan.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");

                // for accounts st of a/c 
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F61000", 2, "Customer Complaint Module", 3, "-", "-", "Y", "fin61_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F61100", 3, "Complaint Activity", 3, "-", "-", "Y", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F61101", 4, "Complaint Log", 3, "../tej-base/om_ccm_log.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F61106", 4, "Complaint Action", 3, "../tej-base/om_ccm_act.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F61116", 3, "Customer Complaint Reports", 3, "-", "-", "Y", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F61121", 4, "Complaint Log List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F61126", 4, "Complaint Action List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F61131", 4, "Complaint Status List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e2", "fa-edit", "N", "Y");

                ICO.add_icon(frm_qstr, "F61140", 3, "Customer Complaint Dashboards", 3, "-", "-", "Y", "fin61_e1", "fin45_a1", "fin61CC_e3", "fa-edit");
                ICO.add_icon(frm_qstr, "F61141", 4, "CCM Mgmt Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin61_e1", "fin45_a1", "fin61CC_e3", "fa-edit");
                break;
            case "UKB":
            case "SARC":
            case "PPCL*":
                // for accounts st of a/c 
                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F70240", 3, "Payment Advice", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");

                ICO.add_icon(frm_qstr, "F40999", 4, "MIT", 3, "../tej-base/om_dbd_bpln2.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                break;
            case "STUD":
                Opts_wfin WFIN_opts1 = new Opts_wfin();
                WFIN_opts1.Icon_Visitor(frm_qstr, frm_cocd);
                break;
            case "MAYU":
                Opts_wfin WFIN_opts2 = new Opts_wfin();
                WFIN_opts2.Icon_Visitor(frm_qstr, frm_cocd);
                WFIN_opts2.Icon_TaskmgtWP(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "AMAR":
                Opts_wfin WFIN_optsAMAR = new Opts_wfin();
                WFIN_optsAMAR.Icon_Visitor(frm_qstr, frm_cocd);
                WFIN_optsAMAR.Icon_TaskmgtWP(frm_qstr, frm_cocd);
                WFIN_optsAMAR.IconCastingProd(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25194", 3, "Stg Tfr Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

                //ICO.add_icon(frm_qstr, "F25194a", 3, "Payment Advice (Inv)", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                break;
            case "SAGM*":
            case "SAGI*":
                Opts_wfin wfin_optsSAGM = new Opts_wfin();
                wfin_optsSAGM.Icon_Visitor(frm_qstr, frm_cocd);
                break;
            case "SPKS":
                ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
                ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
                break;
            case "NEOP":
                Opts_wfin wfin_optsNEOP = new Opts_wfin();
                wfin_optsNEOP.Icon_Visitor(frm_qstr, frm_cocd);
                break;
            case "DISP":
                mhd = fgen.chk_RsysUpd("PRDPP101");
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPP101') ");
                    ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F39100", 3, "Prodn Activity", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                }
                break;
            case "JSGI":
                ICO.add_icon(frm_qstr, "F40999", 4, "MIT", 3, "../tej-base/om_dbd_bpln2.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
                break;
            case "AVON":
                Opts_wfin wfin_optsAVON = new Opts_wfin();
                wfin_optsAVON.Icon_Engg_boxCostingAVONStyle(frm_qstr, frm_cocd);

                wfin_optsAVON.Icon_Taskmgt(frm_qstr, frm_cocd);

                wfin_optsAVON.ProfitabilityReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                break;
            //new icon from here onward 
            case "NITC":
                Opts_wfin icon_nitc = new Opts_wfin();
                icon_nitc.PremiumSalesReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50306", 4, "Auto Invoice Details Upload", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit", "N", "N");
                break;

            case "ABOX":
                Opts_wfin ABOX_PROD_REPS = new Opts_wfin();
                ABOX_PROD_REPS.Icon_Prodn_paper(frm_qstr, frm_cocd);
                break;

            case "BUPL":
                Opts_wfin wfin_optsBUPL = new Opts_wfin();
                wfin_optsBUPL.PremiumFinanceReport(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F70503", 4, "PV MRR Tie up Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y");

                wfin_optsBUPL.Icon_Visitor(frm_qstr, frm_cocd);
                break;

            case "KLAS":
                Opts_wfin wfin_optsKLAS = new Opts_wfin();
                wfin_optsKLAS.Icon_security_nfc(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25117", 3, "Job Work Reconciliation", 3, "../tej-base/om_job_report.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                wfin_optsKLAS.Icon_Prodrx(frm_qstr, frm_cocd);
                ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                break;

            case "GRG":
                ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25217", 3, "Item Master Balance update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F47100", 3, "Dom.Order Activity", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                ICO.add_icon(frm_qstr, "F47117", 4, "Sale Schedule Bulk Upload", 3, "../tej-base/om_multi_saleSch_Upl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
                break;

            case "YTEC":
                Opts_wfin opts_ytec = new Opts_wfin();
                opts_ytec.Icon_DrCr_Maruti(frm_qstr, frm_cocd);
                opts_ytec.PremiumEnggReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F10302", 4, "Sales Plan Item not in stage mapping", 3, "../tej-base/om_view_engg.aspx", "items planned(Sales) but not in stage mapping", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10303", 4, "Vulcanisation Plan Item not in stage mapping", 3, "../tej-base/om_view_engg.aspx", "items planned(Vulcanisation) but not in stage mapping", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "Y");
                ICO.add_icon(frm_qstr, "F10304", 4, "Manpower Planning", 3, "../tej-base/om_view_engg.aspx", "Report based on sales & vulcanisation plan", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10305", 3, "Manpower Planning Review", 3, "../tej-base/om_ActIt_review.aspx", "Items Review Form", "-", "fin10_e7", "fin10_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F10307", 4, "Manpower Planning-Area wise Summary", 3, "../tej-base/om_view_engg.aspx", "Area wise summary Report based on sales plan", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10308", 4, "Area Master", 3, "../tej-base/om_tgpop_mst.aspx", "Area Master Creation form", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10309", 4, "Pie Chart-Manpower Planning", 3, "../tej-base/om_view_engg.aspx", "Pie Chart for Manpower ", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F10310", 4, "Man Hours Calculation Graph ", 3, "../tej-base/om_view_engg.aspx", "Line Chart for Total Manhours ", "-", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit", "N", "N");
                break;

            case "UNIP":
                Opts_wfin opts_unip = new Opts_wfin();
                opts_unip.PremiumFinanceReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F70504", 4, "Sale Purchase Summary Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit", "N", "Y");
                break;

            case "XDIL":
            case "RINT":
                // for fixed assets records        
                Opts_wfin Ico_RINT = new Opts_wfin();
                Ico_RINT.Icon_FA_sys(frm_qstr, frm_cocd);
                break;

            case "IAIJ":
                // for self data dr cr notes    
                Opts_wfin icon_iaij = new Opts_wfin();
                icon_iaij.Icon_DrCr_self(frm_qstr, frm_cocd);
                icon_iaij.PremiumSalesReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50301", 4, "Generate Invoice File", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50302", 4, "Auto generate 14day Sales Schedule(Ford)", 3, "../tej-base/om_ssch_autoupd.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit");
                ICO.add_icon(frm_qstr, "F50304", 4, "Auto generate 6 mth Sales Forecast(Ford)", 3, "../tej-base/om_ssch_autoupd.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit");
                break;

            case "SARN":
                Opts_wfin sarn_icons = new Opts_wfin();
                sarn_icons.ProfitabilityReport(frm_qstr, frm_cocd);
                break;

            case "JSIN":
            case "WING":
            case "VELV":
            case "AEPL":
                Opts_wfin jsin_icons = new Opts_wfin();
                jsin_icons.ProfitabilityReport(frm_qstr, frm_cocd);
                break;

            case "WPPL":
                //case "PRPL":
                Opts_wfin icon_wppl = new Opts_wfin();
                icon_wppl.PremiumSalesReport(frm_qstr, frm_cocd);

                ICO.add_icon(frm_qstr, "F50308", 4, "Generate Invoice- Tradeshift", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50307", 4, "SG-TG Report", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit", "N", "Y");
                break;

            case "PGEL":
                Opts_wfin pgel_icons = new Opts_wfin();
                pgel_icons.Icon_Maint(frm_qstr, frm_cocd);
                pgel_icons.IconMouldMaint(frm_qstr, frm_cocd);
                break;

        }


        //mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select idno from FIN_RSYS_UPD where trim(idno)='UPD101'", "idno");
        //if (mhd == "0" || mhd == "")
        //{
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('UPD101') ");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Day Wise Sales' where id='F05101'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Month Wise Sales',id='F05102' where id='F05106'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Plant Wise Sales',id='F05103' where id='F05111'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Master S.O. Checklists(Dom.)' where id='F47132'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Supply S.O. Checklists(Dom.)' where id='F47133'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Supply Sch. Checklists(Dom.)' where id='F47134'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Masters Reports' where id='F10151'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin10_e5' where id='F10152'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text=replace(text,'Live M.I.S','Live Graphs') where text like '%Live M.I.S%'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set web_Action='../tej-base/om_view_acct.aspx',Text='Block/Cancel Invoice' where ID='F70179'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin50_e4' where id='F50156'");
        //    fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set submenuid='fin47_e4' where id in ('F47153','F47154','F47155','F47156')");
        //}

        //new icon FR5OM COMMAND from here onward 

        Opts_wfin sysconfigforAll = new Opts_wfin();
        sysconfigforAll.Icon_SysConfig(frm_qstr, frm_cocd);
        //new icon from here onward 
    }
}