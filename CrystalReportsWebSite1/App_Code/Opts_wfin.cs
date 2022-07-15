using System;


public class Opts_wfin
{
    fgenDB fgen = new fgenDB();
    Create_Icons ICO = new Create_Icons();
    string mhd = "";


    public void Icon_Mgmt(string frm_qstr, string frm_cocd)
    {
        fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE fin_rsys_upd modify IDNO varchar2(10)");
        //--------------------------------
        //Mgmt MIS System
        //--------------------------------
        string mhd = "";

        mhd = fgen.chk_RsysUpd("MGT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MGT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "MGT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F05000", 1, "Management MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05100", 2, "Sales MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05101", 3, "Day Wise Sales", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05106", 3, "Schedule Vs Achieved", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05111", 3, "Plant wise Sales", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05121", 2, "Accounting MIS", 3, "-", "-", "Y", "fin05_e2", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05126", 3, "Debtors Ageing", 3, "../tej-base/om_mis_grid.aspx", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05127", 3, "Creditor Ageing", 3, "../tej-base/om_mis_grid.aspx", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05128", 4, "Top 10 Debtors", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "fin05_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05129", 4, "Top 10 Creditors", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "fin05_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05130", 4, "Cash more than Rs. 10,000", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "fin05_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05131", 4, "Creditors with Debit Balance", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "fin05_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05132", 4, "Debtors with Credit Balance", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "fin05_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05133", 3, "Sales Trend", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05134", 3, "Purchase Trend", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05135", 3, "More Reports ( Acctg MIS)", 3, "-", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05140", 2, "Production MIS", 3, "-", "-", "Y", "fin05_e3", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05141", 3, "Production Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05142", 3, "Consumption Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05143", 3, "Downtime Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05144", 3, "Rejection Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05161", 2, "Stores MIS", 3, "-", "-", "Y", "fin05_e4", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05162", 3, "Inward Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05165", 3, "Outward Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05166", 3, "Issuance Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05167", 3, "Returns Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05190", 2, "Management Dashboards", 3, "-", "-", "Y", "fin05_e9", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05191", 3, "Purchase Req Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05192", 3, "Purchase Ord Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05193", 3, "Gate Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05194", 3, "Material Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05195", 3, "Material Issue Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05196", 3, "QA Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05197", 3, "Sales Orders Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05198", 3, "Sales Dispatch Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
        }
        //ICO.add_icon(frm_qstr, "F05199", 3, "Multi-Department Live Charts", 3, "../tej-base/om_dbd_live_tv.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");

        mhd = fgen.chk_RsysUpd("MGT102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MGT102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "MGT102", "DEV_A");

            ICO.add_icon(frm_qstr, "F05199", 3, "Multi-Department Live Charts", 3, "../tej-base/om_dboard2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05199A", 3, "Multi-Department Live Charts 2", 3, "../tej-base/deskdash2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05112", 3, "Customer(Month) wise Sales Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05113", 3, "Plant(Day) wise Sales Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05114", 3, "Item(Month) wise Sales Qty Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05116", 3, "Item(Day) wise Sales Qty Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05150", 2, "Purchase MIS", 3, "-", "-", "Y", "fin05_e5", "fin05_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05151", 3, "Vendor(Day) Wise Purchase Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e5", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05152", 3, "Vendor(Month) Wise Purchase Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e5", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05153", 3, "Plant(Day) Wise Purchase Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e5", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05154", 3, "Plant(Month) Wise Purchase Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e5", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05155", 3, "Item(Day) Wise Purchase Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e5", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05156", 3, "Item(Month) Wise Purchase Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e5", "fin05_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05117", 3, "Customer(Day) Wise Sales Tracking", 3, "../tej-base/om_mis_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05118", 3, "Plant(Month) wise Sales", 3, "../tej-base/om_mis_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05119", 3, "Item(Day) wise Sales Value Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05120", 3, "Item(Month) wise Sales Value Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05174", 3, "Inward Tracking", 3, "-", "-", "Y", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit");
            ICO.add_icon(frm_qstr, "F05175", 3, "Outward  Tracking", 3, "-", "-", "Y", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit");
            ICO.add_icon(frm_qstr, "F05176", 3, "Issuance  Tracking", 3, "-", "-", "Y", "fin05_e4", "fin05_a1", "fin05_misiss", "fa-edit");
            ICO.add_icon(frm_qstr, "F05177", 3, "Returns  Tracking", 3, "-", "-", "Y", "fin05_e4", "fin05_a1", "fin05_misret", "fa-edit");

            ICO.add_icon(frm_qstr, "F05168", 4, "Inward Item(Day) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05169", 4, "Inward Item(Month) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05170", 4, "Inwward Party(Day) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05171", 4, "Inward Party(Month) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05172", 4, "Inward Plant(Day) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05173", 4, "Inward Plant(Month) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misin", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F05178", 4, "Outward Item(Day) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05179", 4, "Outward Item(Month) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05180", 4, "Outward Party(Day) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05181", 4, "Outward Party(Month) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05182", 4, "Outward Plant(Day) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05183", 4, "Outward Plant(Month) wise Tracking", 3, "../tej-base/om_MIS_grid.aspx", "-", "-", "fin05_e4", "fin05_a1", "fin05_misout", "fa-edit", "N", "Y");
        }
        //ICO.add_icon(frm_qstr, "F05225", 3, "Graph Sales", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "fin05_misgs", "fa-edit", "N", "Y");
        //ICO.add_icon(frm_qstr, "F05226", 4, "Sales Breakup (Top 10 Parties)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "fin05_misgs", "fa-edit", "N", "Y");
        //ICO.add_icon(frm_qstr, "F05229", 4, "Sales Breakup (Top 10 Parties)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "fin05_misgs", "fa-edit", "N", "Y");

        // addded vipin
        ICO.add_icon(frm_qstr, "F05300", 3, "Delivery Status Report", 3, "../tej-base/om_Delivry_Status.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F05302", 3, "ERP Road Map / Monitoring", 3, "../tej-base/om_RoadMap.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");
        // addded vipin 25/08/2020
        ICO.add_icon(frm_qstr, "F05308", 3, "Finsys Internal / Deptt Audit", 3, "../tej-base/om_mis_mgmt.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit");

        mhd = fgen.chk_RsysUpd("MGT103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MGT103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "MGT103", "DEV_A");
            //jmobile icons
            ICO.add_icon(frm_qstr, "F05200", 2, "Mobile Graphs", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F05203", 3, "Daily MIS", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05202", 4, "Sales", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05205", 4, "Material Inward", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05208", 4, "Collection", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05211", 4, "Payments", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr1", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05213", 3, "Monthly MIS", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05214", 4, "Sales", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05217", 4, "Material Inward", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05220", 4, "Collection", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05223", 4, "Payments", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr2", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05225", 3, "Graph : Sales", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05226", 4, "Sales Breakup (Top 10 Parties)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05229", 4, "Sales Vs Coll Month Wise", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05232", 4, "New So Receive Trend", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05235", 4, "Comparison of CY Sales To Last Year (Totals)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05238", 4, "Comparison Of CY Sales To Last Year (Month On Month)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05241", 4, "Schedule Vs Dispatch", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05244", 4, "Party Wise Sales Up", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05247", 4, "Party Wise Sales Down", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05250", 4, "Item Wise Sales Up", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05253", 4, "Item Wise Sales Down", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05256", 4, "Speedometer: Avg No. Of Active Customers In A Month", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05259", 4, "Speedometer: Sales By Value Percent Of Monthly Average", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05262", 4, "Speedometer: Yearly Target", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr3", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05263", 3, "Graph : Finance", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05264", 4, "Pie Chart :Purc Breakup", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05267", 4, "Month Wise Debtor Closing Balance", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05270", 4, "Collection Trend", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05273", 4, "Expense Trend", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05276", 4, "Sales Trend", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr4", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05277", 3, "Graph : Salaries", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr5", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05278", 4, "Salary Breakup Dept Wise", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr5", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05281", 4, "Salary Breakup Desg Wise", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr5", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05284", 4, "Salary Breakup Grade Wise", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr5", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05287", 4, "Salary Trend", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr5", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05288", 3, "Graph : Production", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05289", 4, "Main QA Problems", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05292", 4, "Main Downtime Reasons", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05295", 4, "Monthly Prod PPM", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05298", 4, "Monthly Downtime In Hrs", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05301", 4, "Pending Sales Schedules Vs Job Card YTD (Month Wise)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05304", 4, "Pending Job Cards Not Yet Closed YTD (Month Wise)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05307", 4, "Pending Sales Schedules Vs Job Card YTD (Party Wise Top 10)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05310", 4, "Pending Job Cards (Not Yet Closed) YTD (Party Wise Top 10)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr6", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05311", 3, "Grid : Production", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05312", 4, "Job Cards Not Made", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05315", 4, "Issues Against Job Card (Reqd Vs Actual )", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05318", 4, "Jobcard Completion Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05321", 4, "Jobcard Completion Report", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05324", 4, "Pending Sales Schedules (Job Card Not Made) YT", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05327", 4, "Pending Job Cards (Not Yet Closed) YTD Grid", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05330", 4, "Sales Schedule Vs Shipment Made", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mgr7", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05331", 3, "Graph : QC", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr8", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05332", 4, "Customer Wise Rejection PPM", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg8", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05335", 4, "Customer Wise Rejection Percent", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg8", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05338", 4, "Sales Vs Rejection", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg8", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F05339", 3, "Financial Statistics", 3, "-", "-", "Y", "fin05_e11", "fin05_a1", "fin05_mgr9", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F05340", 4, "Debtors Ageing 30-60-90", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg9", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05343", 4, "Payments Recently Made", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg9", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05346", 4, "P & L -12 Month Trend", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg9", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05349", 4, "Trial Balance", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg9", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05352", 4, "Funds Flow", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg9", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F05355", 4, "Cash Book Entry", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e11", "fin05_a1", "fin05_mg9", "fa-edit", "N", "N");

            //jmobile dashboards
            ICO.add_icon(frm_qstr, "F05365", 3, "Sales Graphs", 3, "../tej-base/om_dbd_gendb_google.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05366", 3, "Finance Graphs", 3, "../tej-base/om_dbd_gendb_google.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05367", 3, "Salaries Graphs", 3, "../tej-base/om_dbd_gendb_google.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05368", 3, "Production Graphs", 3, "../tej-base/om_dbd_gendb_google.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05369", 3, "Pending Production Graphs", 3, "../tej-base/om_dbd_gendb_google.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F05370", 3, "QC Graphs", 3, "../tej-base/om_dbd_gendb_google.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");
        }
    }

    public void Icon_Engg(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Engg Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("ENG101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('ENG101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "ENG101", "DEV_A");

            ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10100", 2, "Items Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10101", 3, "Item Main Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10106", 3, "Item Sub Groups", 3, "../tej-base/Isub_Grp.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10111", 3, "General Items", 3, "../tej-base/item_gen.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10116", 3, "FG/SFG Items", 3, "../tej-base/item_gen.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10121", 3, "Units Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10125", 3, "Rack / Bin / Location Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10126", 3, "Process Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10130", 2, "Production Masters", 3, "-", "-", "Y", "fin10_e2", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10131", 3, "Bill of Materials", 3, "../tej-base/om_bom_ent.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10132", 3, "Accesories BOM", 3, "../tej-base/om_bom_ent.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10133", 3, "Process Mapping", 3, "../tej-base/om_proc_map.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10134", 3, "Laminate BOM", 3, "../tej-base/om_bom_lami.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10134A", 3, "Poly BOM", 3, "../tej-base/om_bom_lami.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10134B", 3, "Pouch BOM", 3, "../tej-base/om_bom_lami.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10135", 3, "Process Plan (Corrugation)", 3, "../tej-base/om_proc_plan.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10135F", 3, "Process Plan (Flexible)", 3, "../tej-base/om_proc_plan.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10135L", 3, "Process Plan (Labels)", 3, "../tej-base/om_proc_plan.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10140", 2, "Master Approvals", 3, "-", "-", "Y", "fin10_e3", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10141", 3, "Item Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin10_e3", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10142", 3, "BOM. Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin10_e3", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10143", 3, "Process Plan Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin10_e3", "fin10_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10152", 3, "Masters Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10156", 3, "List of Items", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10160", 3, "BOM. Tree View", 3, "../tej-base/om_bom_tree.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10161", 3, "Item Tree View", 3, "../tej-base/om_item_tree.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10171", 2, "Master Analysis", 3, "-", "-", "Y", "fin10_e5", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10173", 3, "Masters Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin10_e5", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10174", 3, "Masters Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin10_e5", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10221", 3, "More Reports(Engg/Devl.)", 3, "-", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F10222", 4, "List of Items With BOM", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10223", 4, "List of Items Without BOM", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10224", 4, "Items in Multiple BOMs", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10225", 4, "BOM Where Parent/Child Match", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10226", 4, "BOM Items Without Sales Order", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10227", 4, "Items Not Used in During DTD", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10228", 4, "Boms  Not Used in During DTD", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10229", 4, "List of Deactivated Items", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10230", 4, "List of Un Approved Items", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10231", 4, "List of Items With Selected Fields", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10232", 4, "List of BOMS With Selected Fields", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10233", 4, "Items Without Min/Max/ROL", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10234", 4, "Min/Max/ROL of Items", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10235", 4, "Similar Parent Code BOMs", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10236", 4, "Similar Child Code in same BOMs", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10237", 4, "List of FG Linked SF items without BOM", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10238", 4, "OSP BOM Search ", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F10239", 4, "SF BOM Search ", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10183", 3, "BOM Costing", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F10185", 3, "Corrugation Costing", 3, "../tej-base/cost_corr.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
        }
        mhd = fgen.chk_RsysUpd("ENG103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('ENG103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "ENG103", "DEV_A");

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
        }
    }

    public void Icon_Engg_boxCostingAVONStyle(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Engg Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("ENG102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('ENG102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "ENG102", "DEV_A");

            ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10128", 3, "Party/CostSheet Master", 3, "../tej-base/frmBoxMaster.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit");


            ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10189", 3, "Box Costing", 3, "../tej-base/frmBoxCosting.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10190", 3, "Box Master Rate Updation", 3, "../tej-base/frmBoxMasterUpdate.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

            //ICO.add_icon(frm_qstr, "F10128", 3, "Party/CostSheet Master", 3, "../tej-base/frmBoxMaster.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
            //ICO.add_icon(frm_qstr, "F10128", 3, "Party/CostSheet Master", 3, "../tej-base/frmBoxMaster.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
        }
    }

    public void Icon_Purch(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Purchase Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("PUR101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PUR101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PUR101", "DEV_A");

            ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15100", 2, "Purchase Activity", 3, "-", "-", "Y", "fin15_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15101", 3, "Purchase Request Entry", 3, "../tej-base/om_pur_req.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15106", 3, "Purchase Orders Entry", 3, "../tej-base/om_po_entry.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15111", 3, "Purchase Schedule Entry", 3, "../tej-base/om_pur_sch.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15116", 3, "Vendor Price List Entry", 3, "../tej-base/om_app_vend.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15117", 3, "Purchase Budget Entry", 3, "../tej-base/om_pur_budg.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15126", 3, "Purchase Request Checklists", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15127", 3, "Purchase Orders Checklists", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15128", 3, "Purchase Schedule Checklists", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15129", 3, "Approved Price Checklists", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15131", 2, "Purchase Reports", 3, "-", "-", "Y", "fin15_e3", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15132", 3, "Purchase Requisition Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15133", 3, "Purchase Order Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15134", 3, "Purchase Schedule Report", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15135", 3, "Approved Price Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15136", 4, "Closed PR Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15137", 4, "Import Purchase Order Print", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15138", 4, "Pending PO checklist(Old Data)", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");



            ICO.add_icon(frm_qstr, "F15141", 4, "PR Vs PO Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15143", 4, "PO Vs MRR Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15151", 2, "Purchase Analysis", 3, "-", "-", "Y", "fin15_e5", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15152", 3, "Purch. Requisition Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15156", 3, "Purch. Order Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15158", 3, "Purchase Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15159", 3, "Purchase Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F15160", 2, "Purch. Check/Approvals", 3, "-", "-", "Y", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15161", 3, "Purchase Request Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15162", 3, "Purchase Request Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15165", 3, "Purchase Order Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15166", 3, "Purchase Order Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15171", 3, "Purchase Schedule Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15176", 3, "Price List Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F15200", 2, "Purchase Masters", 3, "-", "-", "Y", "fin15_e6", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15201", 3, "P.Order Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15203", 3, "Currency Type Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15205", 3, "Price Basis Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15207", 3, "Insurance Term Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15209", 3, "Freight Term Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F15210", 3, "P.R. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15211", 3, "P.O. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15212", 3, "P.O. Approval Level Master", 3, "../tej-base/om_poapprlvl.aspx", "-", "-", "fin15_e6", "fin15_a1", "-", "fa-edit", "N", "Y");

            //'../tej-base/moreReports.aspx'
            ICO.add_icon(frm_qstr, "F15221", 3, "More Reports(Purch.)", 3, "-", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15222", 4, "Sch Vs Rcpt Day Wise", 3, "../tej-base/om_prt_purc.aspx", "31 Day Tracker ", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15223", 4, "Sch Vs Rcpt Total Basis", 3, "../tej-base/om_prt_purc.aspx", "Summary of Sch Vs Rcpt", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15225", 4, "TAT PR Vs PO", 3, "../tej-base/om_view_purc.aspx", "Turn Around Time PR VS PO", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15226", 4, "TAT PO Vs MRR", 3, "../tej-base/om_view_purc.aspx", "Turn Around Time PO Vs MRR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15227", 4, "TAT PR VS PO Vs MRR", 3, "../tej-base/om_view_purc.aspx", "Turn Around Time PR Vs PO Vs MRR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15228", 4, "TAT MRR Vs MRIR", 3, "../tej-base/om_view_purc.aspx", "Turn Around Time MRR Vs MRIR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15229", 4, "TAT PR Approval VS PR", 3, "../tej-base/om_view_purc.aspx", "Turn Around Time PR Approval Vs PR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15230", 4, "Price Comparison Chart Vendor Wise", 3, "../tej-base/om_prt_purc.aspx", "Compare Prices Vendor Wise", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15231", 4, "Price Comparison Chart Item Wise", 3, "../tej-base/om_prt_purc.aspx", "Compare Prices Item Wise", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15232", 4, "Price Comparison Chart Plant Wise", 3, "../tej-base/om_prt_purc.aspx", "Compare Prices Plant Wise,Item Wise", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15233", 4, "Gate Inward Checklist", 3, "../tej-base/om_view_purc.aspx", "Material Recvd on Gate", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15234", 4, "Matl Inward Checklist", 3, "../tej-base/om_view_purc.aspx", "Gate Entry -> MRR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15235", 4, "Matl Consumption Report", 3, "../tej-base/om_prt_purc.aspx", "Review Matl Consumption", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15236", 4, "Supplier,Item Wise 12 Month P.O. Qty", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15237", 4, "Supplier,Item Wise 12 Month P.O. Value", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15238", 4, "Delivery Date Vs Rcpt Date", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15239", 4, "PO Items with Rate Inc/Decrease", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15240", 4, "PO Items with Qty. Inc/Decrease", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15241", 4, "Supplier history Card", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15242", 4, "Supplier Rating Card", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15243", 4, "Multi Plant Pending Orders", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15244", 4, "Closed Purchase Order Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F15247", 4, "Schedule vs Despatch (Qty based)", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15248", 4, "Schedule vs Despatch (Value based)", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15249", 4, "Schedule vs Despatch ( Qty + Value Based)", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15250", 4, "Pending PO Report(Old Data-Print)", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15251", 4, "Import PO Register- FC", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15302", 4, "Pending PR Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15303", 4, "Pending PO Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15304", 4, "Pending Schedule (Day Wise) Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15305", 4, "Pending Schedule (Month Wise) Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15306", 4, "Schedule (Day Wise) Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15307", 4, "Pending Schedule (Vendor Wise) Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15308", 4, "Closed PR Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15309", 4, "Closed PO Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15310", 4, "Cancelled PO Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F15311", 4, "PO Amendment History Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15312", 4, "Vendor Wise 12 Month Rates Trend ( Max) Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15313", 4, "Item Wise 12 Month Rates Trend (Max) Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F15315", 4, "PO Delivery Date Vs Rcpt Date Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15316", 4, "PO Delivery Date Based Monthly Calender", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15317", 4, "Purchase Schedule Delivery Exp. during DTD ", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F15318", 4, "Purchase Orders Delivery Exp. during DTD ", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
            //F15319 same report as  F15318
            //ICO.add_icon(frm_qstr, "F15319", 4, "Purchase Orders Nearing Expiry ", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");


        }
        ICO.add_icon(frm_qstr, "F15120", 3, "Vendor Performance", 3, "../tej-base/om_supplier_rating.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
        mhd = fgen.chk_RsysUpd("PUR102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PUR102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PUR102", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_msys where mlevel=4 and id in ('F15302','F15303','F15140','F15142','F15314')");
            //ICO.add_icon(frm_qstr, "F15301", 3, "More Checklists(Purch.)", 3, "-", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15302", 3, "Pending PR Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15303", 3, "Pending PO Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15314", 3, "PR vs PO Vs MRR Checklist", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F15140", 3, "Pending PR Register", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_msys where id in ('F15245')"); ICO.add_icon(frm_qstr, "F15142", 3, "Pending Purchase Order Register With Line No.", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F15245", 3, "Stock summary + Analysis", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F15180", 3, "Purchase Order Tracking", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15181", 3, "Purchase Schedule Tracking", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15182", 3, "Purchase Amendment Tracking", 3, "../tej-base/om_view_purc.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "N");
        }
        //ICO.add_icon(frm_qstr, "F15191", 3, "MRR Costing", 3, "../tej-base/om_mrr_costing.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit");

    }

    public void Icon_gate(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Gate Inward, Outward
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("GAT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('GAT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "GAT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20101", 3, "Gate Inward Entry", 3, "../tej-base/om_gate_inw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20106", 3, "Gate Outward Entry", 3, "../tej-base/om_gate_outw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");


            ICO.add_icon(frm_qstr, "F20116", 2, "Gate Checklists", 3, "-", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20121", 3, "Gate Inward Checklist", 3, "../tej-base/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20126", 3, "Gate Outward Checklist", 3, "../tej-base/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20127", 3, "Gate PO Checklist", 3, "../tej-base/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20128", 3, "Gate RGP Checklist", 3, "../tej-base/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F20131", 2, "Gate Reports", 3, "-", "-", "Y", "fin20_e3", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20132", 3, "Gate Inward Register", 3, "../tej-base/om_prt_gate.aspx", "-", "-", "fin20_e3", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20133", 3, "Gate Outward Register", 3, "../tej-base/om_prt_gate.aspx", "-", "-", "fin20_e3", "fin20_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F20140", 2, "Gate Analysis", 3, "-", "-", "Y", "fin20_e4", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20141", 3, "Gate Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin20_e4", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20142", 3, "Gate Outward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin20_e4", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20158", 3, "Gate Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin20_e4", "fin20_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20159", 3, "Gate Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin20_e4", "fin20_a1", "-", "fa-edit", "N", "Y");

        }

        ICO.add_icon(frm_qstr, "F20130", 3, "Invoice Gate out by scanning", 3, "../tej-base/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");

    }

    public void Icon_security_nfc(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("NFC101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('NFC101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "NFC101", "DEV_A");


            ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20200", 2, "Premium Features-NFC Patrolling App ", 3, "-", "-", "Y", "fin20_e9", "fin20_a1", "-", "fa-edit");
            //ICO.add_icon(frm_qstr, "F20201", 3, "Route allocation", 3, "-", "-", "-", "fin20_e5", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            //ICO.add_icon(frm_qstr, "F20202", 3, "NFC Patrolling App- Reports ", 3, "-", "-", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20203", 3, "Patrolling Route Tie up Report", 3, "../tej-base/om_view_gate.aspx", "Report showing Target & Actual Route", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20204", 3, "Patrolling Report-Route wise", 3, "../tej-base/om_view_gate.aspx", "Actual patrolling Route followed", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20205", 3, "View Route Image Log", 3, "../tej-base/om_view_gate.aspx", "Images captured while patrolling", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20206", 3, "View Registered Routes", 3, "../tej-base/om_view_gate.aspx", "Registered Routes", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20207", 3, "View Registered Cards", 3, "../tej-base/om_view_gate.aspx", "Registered Cards", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20208", 3, "Delete Patrolling Route", 3, "../tej-base/om_view_gate.aspx", "Delete Route", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20209", 3, "Delete NFC Card", 3, "../tej-base/om_view_gate.aspx", "delete Card", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20210", 3, "Patrolling Report-Person wise", 3, "../tej-base/om_view_gate.aspx", "Actual patrolling done with Time stamp", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20211", 3, "Patrolling Report-Date wise", 3, "../tej-base/om_view_gate.aspx", "Actual patrolling done datewise", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F20212", 3, "Drill Down Patrolling Report", 3, "../tej-base/om_view_gate.aspx", "Drill down datewise detailed report", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20213", 3, "Summary Patrolling Report", 3, "../tej-base/om_view_gate.aspx", "Datewise Summary Report for selected period", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F20214", 3, "Edit Registered Card", 3, "../tej-base/om_sa_editcard.aspx", "", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F20215", 3, "Edit Route", 3, "../tej-base/om_sa_editcard.aspx", "", "-", "fin20_e9", "fin20_a1", "fin20_NFCA", "fa-edit", "N", "Y");

        }
    }


    public void Icon_Visitor(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Visitor Gate System
        // ------------------------------------------------------------------
        //form name change reqd

        string mhd = "";
        mhd = fgen.chk_RsysUpd("VISI101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('VISI101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "VISI101", "DEV_A");

            ICO.add_icon(frm_qstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20231", 2, "Visitor Module", 3, "-", "-", "Y", "fin20_e5", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20232", 3, "Visitor Requisition", 3, "../tej-base/vreq.aspx", "-", "-", "fin20_e5", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20233", 3, "Visitor Req Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin20_e5", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20234", 3, "Visitor Entry", 3, "../tej-base/vmrec.aspx", "-", "-", "fin20_e5", "fin20_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F20235", 3, "Visitor Outward Entry", 3, "../tej-base/om_appr.aspx", "-", "-", "fin20_e5", "fin20_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F20236", 3, "Reports", 3, "-", "-", "-", "fin20_e5", "fin20_a1", "fin20_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F20310", 4, "Visitor Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin20_e5", "fin20_a1", "fin20_mrep", "fa-edit", "Y", "Y");
            ICO.add_icon(frm_qstr, "F20311", 4, "Month Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin20_e5", "fin20_a1", "fin20_mrep", "fa-edit", "Y", "Y");
            ICO.add_icon(frm_qstr, "F20312", 4, "Status Wise Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin20_e5", "fin20_a1", "fin20_mrep", "fa-edit", "Y", "Y");
            ICO.add_icon(frm_qstr, "F20314", 4, "Visitor Movement Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin20_e5", "fin20_a1", "fin20_mrep", "fa-edit", "Y", "Y");
        }
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='TYPEMST'", "TNAME");
        string SQuery = "";
        if (mhd == "0" || mhd == "")
        {
            SQuery = "CREATE TABLE TYPEMST (BRANCHCD CHAR(2),ID CHAR(2),TYPE1 VARCHAR(6),ACODE VARCHAR(4),NAME VARCHAR(70),REMARKS VARCHAR(150),ENT_BY VARCHAR(20),ENT_DT DATE DEFAULT SYSDATE,EDT_BY VARCHAR(20),EDT_DT DATE DEFAULT SYSDATE,STATUS CHAR(1),NFLAG CHAR(1))";
            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
        }
    }

    public void Icon_Store(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Inventory Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("STR101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('STR101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "STR101", "DEV_A");

            ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25101", 3, "Matl Inward Entry", 3, "../tej-base/om_mrr_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25106", 3, "Matl Outward Entry", 3, "../tej-base/om_chl_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25111", 3, "Matl Issue Entry", 3, "../tej-base/om_iss_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25116", 3, "Matl Return Entry", 3, "../tej-base/om_ret_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F25245A", 3, "FG Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25245R", 3, "Return Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25245S", 3, "Rcv Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25121", 2, "Inventory Checklists", 3, "-", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25126", 3, "Matl Inward Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25127", 3, "Matl Outward Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25128", 3, "Matl Issue Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25129", 3, "Matl Return Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25131", 2, "Stock Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25132", 3, "Stock Ledger", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25133", 3, "Stock Summary", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25134", 3, "Stock Min-Max", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F25135", 3, "Store Stock Value", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25136", 3, "OSP J/wrk Stock Value", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25141", 3, "Matl Inward Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25142", 3, "Matl Outward Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25143", 3, "Matl Issue Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25144", 3, "Matl Return Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25145", 3, "More Checklists ( Inventory)", 3, "-", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25146", 3, "More Reports( Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25147", 4, "FG Stock Summary ( Item wise)", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25148", 4, "FG Stock Summary ( HSN wise)", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25149", 4, "FG Valuation", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25151", 2, "OSP.Jobwork Reports", 3, "-", "-", "Y", "fin25_e5", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25152", 3, "Vendor Jobwork Register", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e5", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25156", 3, "Vendor Jobwork Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e5", "fin25_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F25161", 2, "Cust.Jobwork Reports", 3, "-", "-", "Y", "fin25_e6", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25162", 3, "Cust. Jobwork Register", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25165", 3, "Cust. Jobwork Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25171", 2, "Inventory Analysis", 3, "-", "-", "Y", "fin25_e7", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25176", 3, "Matl Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25181", 3, "Matl Issue Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25191", 3, "Stock Data Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F25192", 3, "Stores Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25193", 3, "Stores Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25201", 3, "Inward Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25203", 3, "Outward Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25205", 3, "Issue Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25207", 3, "Return Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25209", 3, "Department Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F25214", 3, "Multi Item Master", 3, "../tej-base/om_multi_item.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25215", 3, "Reel wise stock upload", 3, "../tej-base/om_multi_reel.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25216", 3, "Batch wise stock upload", 3, "../tej-base/om_multi_batch.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25217", 3, "Item Master Balance update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25218", 3, "Stock Qty update", 3, "../tej-base/om_multi_billitem.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

            //ICO.add_icon(frm_qstr, "F25221", 3, "More Reports(Inventory)", 3, "-", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25222", 4, "Deptt Wise Issue Summary", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25223", 4, "Deptt Wise Issue Comparison", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25230", 4, "Rejn Stock Summary Item Wise", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25231", 4, "Rejn Stock Summary Vendor Wise", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25232", 4, "Rejn Stock Ledger", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25245", 4, "Matl. Location Report", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");



            ICO.add_icon(frm_qstr, "F25235", 4, "Short / Excess Supplies", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25236", 4, "Stock Ageing Report", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25237", 4, "Supplier,Item Wise 12 Month Purch. Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25238", 4, "Group, Item Wise 12 Month Purchase Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25239", 4, "Deptt,Item Wise 12 Month Consumption Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25240", 4, "Group, Item Wise 12 Month Consumption Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25241", 4, "Non Moving Item Report", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25242", 4, "Inward Supplies with Rejection", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

            //spl_cust

            ICO.add_icon(frm_qstr, "F25246P", 3, "Production vs Rcv Report", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F25246P", 3, "Production vs Rcv Report", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25137", 4, "Cross Year Stock Ledger", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

        }
        mhd = fgen.chk_RsysUpd("STR102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('STR102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "STR102", "DEV_A");

            ICO.add_icon(frm_qstr, "F25198A", 3, "MRR Reel Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25198B", 3, "Single Reel Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25198C", 3, "Return Reel Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F25198", 3, "Prodn Sticker", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25139", 4, "Pending Stock (Qty & Value) RGP Wise", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25117", 3, "Job Work Reconciliation", 3, "../tej-base/om_job_report.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F25219", 3, "Item Master Min/Max/ROL update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F25247", 4, "Supplier Summary Vendor Wise", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25248", 4, "Supplier Rejection Item Movement", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from fin_msys where id in ('F25138','F25233','F25234')");
            ICO.add_icon(frm_qstr, "F25234", 3, "Stock summary + Analysis", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F25233", 3, "Item Review Analysis", 3, "../tej-base/om_ActIt_review.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25138", 3, "Gate Entry Pending MRR Entry", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25260", 3, "Issue Request Pending Store Issue", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25261", 3, "Return Request Pending Store Rcpt", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25157", 3, "Job work Register(21-09)", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e5", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25158", 3, "RGP vs MRR(23-0J)", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e5", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F25168", 4, "RM Closing Stock Details", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25167", 4, "RM Closing Stock Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F25220", 3, "Item Master Rate update", 3, "../tej-base/om_multi_item_upt.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit");

        }



        mhd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
        if (mhd == "05" || mhd == "06" || mhd == "12" || mhd == "13")
        {
            mhd = fgen.chk_RsysUpd("STR103");
            if (mhd == "0" || mhd == "")
            {
                //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('STR103') ");
                fgen.add_RsysUpd(frm_qstr, frm_cocd, "STR103", "DEV_A");

                ICO.add_icon(frm_qstr, "F25380", 2, "Reel/Roll Reports", 3, "-", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25381", 3, "Reel/Roll Stocks Report", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25383", 3, "Reel/Roll Receipt Report", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25385", 3, "Reel/Roll Issue Report", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25387", 3, "Reel/Roll Return Report", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25395", 3, "Reel/Roll Tag (Size : A4)", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25396", 3, "Reel/Roll Tag (Size : A5)", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25397", 3, "Reel/Roll Tag (Size : Small)", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e10", "fin25_a1", "-", "fa-edit");


                ICO.add_icon(frm_qstr, "F25400", 2, "Physical Verification Menu", 3, "-", "-", "Y", "fin25_e11", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25402", 3, "Physical Verification Method", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e11", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25404", 3, "Books Vs Physical Stock Report", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e11", "fin25_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F25406", 3, "Report of Mismatched Reels/Rolls", 3, "../tej-base/om_view_reels.aspx", "-", "Y", "fin25_e11", "fin25_a1", "-", "fa-edit");
            }
        }
    }
    public void iconInvMrrUpload(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F25370", 2, "Document Keeping", 3, "-", "-", "Y", "fin25_e9", "fin25_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F25371", 3, "MRR Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin25_e9", "fin25_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F25372", 3, "MRR Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin25_e9", "fin25_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F25373", 3, "MRR View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin25_e9", "fin25_a1", "-", "fa-edit");
    }
    public void iconFinanceVoucherUpload(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/om_vch_upload.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/om_vch_view.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
    }

    public void iconOMSEntry(string frm_qstr, string frm_cocd)
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

    public void Icon_Qlty(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Finsys Q.A. System
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("QLT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('QLT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "QLT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30100", 2, "Quality Templates", 3, "-", "-", "Y", "fin30_e1", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30101", 3, "QA Inwards Template", 3, "../tej-base/om_qa_templ.aspx", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30106", 3, "QA In-Proc Template", 3, "../tej-base/om_qa_templ.aspx", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30108", 3, "QA Outward Template", 3, "../tej-base/om_qa_templ.aspx", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F30110", 2, "Quality Activity", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30111", 3, "QA Inwards Test Report", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30112", 3, "QA In-Proc Test Report", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30113", 3, "QA Outward Test Report", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");


            ICO.add_icon(frm_qstr, "F30116", 2, "Quality Checklists", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30121", 3, "QA Inwards Checklist", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30126", 3, "QA In-Proc Checklist", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30127", 3, "QA Outward Checklist", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F30131", 2, "Quality Reports", 3, "-", "-", "Y", "fin30_e3", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30132", 3, "QA Inwards Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30133", 3, "QA In-Proc Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30134", 3, "QA Outward Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F30140", 2, "Quality (Basic)", 3, "-", "-", "Y", "fin30_e4", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30141", 3, "Basic Inward Quality", 3, "../tej-base/om_qa_bas.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30144", 3, "FG Quality", 3, "../tej-base/om_qa_bas.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30142", 3, "Inward QA Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F30143", 3, "Inward QA Rejn Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F30151", 2, "Quality Analysis", 3, "-", "-", "Y", "fin30_e6", "fin30_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F30152", 3, "QA Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin30_e6", "fin30_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30156", 3, "QA Outward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin30_e6", "fin30_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30158", 3, "Quality Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin30_e6", "fin30_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30159", 3, "Quality Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin30_e6", "fin30_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F30221", 3, "More Reports(Quality)", 3, "-", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F30222", 4, "Supplier History Card", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30223", 4, "Supplier Rating Report", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30224", 4, "Inward Supplies with Rejection", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F30225", 4, "Suppliers 12 Month Rejn Trend", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30226", 4, "Group,Item Wise 12 Month Rejn Trend ", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30227", 4, "Deptt,Item Wise 12 Month Line Rejn ", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30228", 4, "Chart Showing Instances of Inward Rejn", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F30229", 4, "Chart Showing Instances of Line Rejn", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");

        }
        ICO.add_icon(frm_qstr, "F30128", 3, "MRR Pending Inspection", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");

        ICO.add_icon(frm_qstr, "F30233", 4, "Inspection Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
    }

    public void Icon_Ppc_paper(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // PPC Module :PAPER 
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("PPCPP101");
        ICO.add_icon(frm_qstr, "F50112", 3, "Receive FGs Stock", 3, "../tej-base/om_recv_fg.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PPCPP101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PPCPP101", "DEV_A");

            ICO.add_icon(frm_qstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F35050", 2, "Packaging/Printing PPC ", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F35100", 3, "Prt/Pkg PPC Activity", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35101", 4, "Job Order Creation", 3, "../tej-base/om_JCard_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35106", 4, "Job Order Planning", 3, "../tej-base/om_JPlan_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");

            ICO.add_icon(frm_qstr, "F35121", 3, "Prt/Pkg PPC Checklists", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin352pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35126", 4, "Daily Prodn Checklist(Pt)", 3, "../tej-base/om_view_ptppc.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin352pp_mrep", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F35127", 4, "Mthly Prodn Checklist(Pt)", 3, "../tej-base/om_view_ptppc.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin352pp_mrep", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F35131", 3, "Prodn PPC Activity", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35132", 4, "Sales Plan Entry", 3, "../tej-base/om_splan_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35133", 4, "Prodn Plan Entry", 3, "../tej-base/om_pplan_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35134", 4, "Day Wise Plan Entry", 3, "../tej-base/om_dplan_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35135", 4, "SF Prodn Plan Entry", 3, "../tej-base/om_sfplan_entry.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35136", 4, "Daily Prodn Plan", 3, "../tej-base/om_sday_plan.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F35138", 4, "Plan for MRP", 3, "../tej-base/om_plan_mrp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin353pp_mrep", "fa-edit");

            ICO.add_icon(frm_qstr, "F35140", 3, "Prodn PPC Checklists", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin354pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35141", 4, "Daily Prodn Checklist(Gn)", 3, "../tej-base/om_view_gnppc.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin354pp_mrep", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F35142", 4, "Mthly Prodn Checklist(Gn)", 3, "../tej-base/om_view_gnppc.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin354pp_mrep", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F35240", 3, "Prt/Pkg Reports", 3, "-", "-", "Y", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35226", 4, "Parta Report", 3, "../tej-base/parta_rpt.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin355pp_mrep", "fa-edit", "N", "Y");
        }
        mhd = fgen.chk_RsysUpd("PPCPP102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PPCPP102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PPCPP102", "DEV_A");

            ICO.add_icon(frm_qstr, "F35107", 4, "Machine Planning", 3, "../tej-base/om_mcplan.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35110", 4, "Job Order Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
            ICO.add_icon(frm_qstr, "F35128", 4, "Job Planning Checklist", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin352pp_mrep", "fa-edit", "N", "Y");
        }
        ICO.add_icon(frm_qstr, "F35111", 4, "Job Order Closure / Re-Call", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");

        ICO.add_icon(frm_qstr, "F35109", 4, "Shop Work Load", 3, "../tej-base/om_shopwork_load.aspx", "-", "-", "fin35pp_e1", "fin35_a1", "fin351pp_mrep", "fa-edit");
    }

    public void Icon_Prodn_paper(string frm_qstr, string frm_cocd)
    {
        //------------------------
        // print/corr prodn
        //------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("PRDPP101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPP101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDPP101", "DEV_A");

            ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F40100", 3, "Prt/Pkg Activity", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40101", 4, "Printing Prodn", 3, "../tej-base/om_prtg_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40106", 4, "Corrugation Prodn", 3, "../tej-base/om_corr_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40107", 4, "Label Prodn", 3, "../tej-base/om_corr_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40111", 4, "Ptg/Pkg Process Prodn", 3, "../tej-base/om_prtg_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F40112", 4, "Sorting Packing", 3, "../tej-base/om_sor_pak.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40113", 4, "Paper_cutting", 3, "../tej-base/om_pap_cut.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F40121", 3, "Prt/Pkg Prodn Checklists", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40126", 4, "Daily Prodn Checklist(PP)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40127", 4, "Mthly Prodn Checklist(PP)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40128", 4, "Down Time Checklist(PP)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40129", 4, "Rejection Checklist(PP)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40130", 4, "Operator wise Production Rejection", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40131", 3, "Prt/Pkg Prodn Reports", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40132", 4, "Daily Prodn Report(PP)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40133", 4, "Mthly Prodn Report(PP)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40134", 4, "Consumption Report(PP)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40135", 4, "Wastages Report(PP)", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40136", 4, "Corrugation plan v/s Rejection data", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40137", 4, "Trend of Rejection(D-T-D)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40138", 4, "Trend of DownTime(D-T-D)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40139", 4, "Item wise Production", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F40140", 4, "Production Slip", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40141", 4, "Production Summary", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40142", 4, "Details of Items  produced", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40143", 4, "Production with Rej % Itemwise", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40144", 4, "Production slip", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40145", 4, "Corrugation Print", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40146", 4, "Sorting & Packing Print", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40147", 4, "Costing Print", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40148", 4, "Production Summary Monthwise", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40149", 4, "Production Summary Machine wise", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40150", 4, "Details of Items  rejected", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_a1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40171", 3, "Packaging Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40172", 4, "Prtg. Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40173", 4, "Corr. Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40200", 3, "Material Requests", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp5_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40201", 4, "Matl Issue Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp5_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40206", 4, "Matl Return Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp5_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40211", 4, "Matl JobWork Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp5_e1", "fa-edit");
        }

        icon_ppc_prodReports(frm_qstr, frm_cocd);
        mhd = fgen.chk_RsysUpd("PRDPP103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPP103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDPP103", "DEV_A");

            ICO.add_icon(frm_qstr, "F40114", 4, "SF Production", 3, "../tej-base/sf_prod.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40122", 3, "Job Order Status", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "N", "fin40_e1", "fin40_a1", "fin40pp2_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40114", 4, "SF Production", 3, "../tej-base/sf_prod.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40171", 3, "Packaging Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");

            // WIP Columnar Stock Report
            ICO.add_icon(frm_qstr, "F40350", 4, "WIP Columnar Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F40115", 4, "Paper_Inspection", 3, "../tej-base/om_pap_insp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
        }
    }

    public void icon_ppc_prodReports(string frm_qstr, string frm_cocd)
    {
        string mhd = fgen.chk_RsysUpd("PRDPP102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPP102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDPP102", "DEV_A");

            ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F40050", 2, "Packaging Production", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F40301", 3, "Production Reports(Detailed)", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40302", 4, "Estimate Projection", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            //ICO.add_icon(frm_qstr, "F40303", 4, "Parta Report List", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40304", 4, "Date Wise Consumption", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40305", 4, "Percentage Party & BF Wise Consumption", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40306", 4, "BF wise Consumption", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40307", 4, "Date,Party,Item Wise Consumption", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40308", 4, "Daily Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40309", 4, "Item Wise Details", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40310", 4, "Party Wise Purchased Qty", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40311", 4, "Corrugation Order Vs Production Balance", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F40312", 4, "Month Wise M/C Wise Downtime reason Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40313", 4, "Reason Yearly Downtime Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40314", 4, "Down time Reasn Wise 31 day", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40315", 4, "Job Wise Rejection reason Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40316", 4, "Item Wise Rejection reason Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40317", 4, "Month Wise Rejection reason Report(NOS/KGS)", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40318", 4, "Job Wise Downtime reason Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40319", 4, "Date,Reason wise Downtime  Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40320", 4, "Job Wise All Stage Production Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40321", 4, "Item Wise All Stage Production Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40322", 4, "Job/Stage Wise Pending Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40323", 4, "Item/Stage Wise Pending Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40324", 4, "Job Wise All Stage Rejection Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40325", 4, "Item Wise All Stage Rejection Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40326", 4, "RM Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40326R", 4, "Reel Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40326M", 4, "Missing Reel Physical Verification Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40327", 4, "FG Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40328", 4, "FG Stock Location Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40328R", 4, "RM Stock Location Wise", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40329", 4, "Reel Wise Reel Location", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40330", 4, "BF,GSM,Reel Wise Production Report Detailed", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");


            ICO.add_icon(frm_qstr, "F40331", 4, "Corrugation Order Balance", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40332", 4, "Physical Verification Records", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40333", 4, "Sale Projection Vs Production Qty", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40334", 4, "Job Order Not Planned", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40335", 4, "Delivery Plan-All", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40336", 4, "Pending Delivery Plan", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40337", 4, "Shift Wise Production Rejection", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40338", 4, "Production Vs Despatch Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40339", 4, "Daily Issuance Vs Consumption", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40340", 4, "Job Wise Wastage Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40341", 4, "Capacity Vs Production", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40342", 4, "Plan Vs Production", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40343", 4, "Production Vs Completion", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40344", 4, "Production Plan", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40345", 4, "Sales Plan", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40346", 4, "Work Order Summary", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp6_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40347", 4, "Production Rejection Drill Down", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40348", 4, "Production Down Time Drill Down", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F40349", 4, "Production Drill Down", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40352", 4, "Job Wise Issue , Consumption", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F40352", 4, "Job/Stage Wise Pending Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "Y", "Y");
            ICO.add_icon(frm_qstr, "F40354", 3, "Corrugation MIS", 3, "../tej-base/om_corr_mis_rpt.aspx", "-", "Y", "fin40_e1", "fin40_x1", "fin40pp6_x1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40355", 4, "Corrugation Weight Summary", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F40356", 4, "Corrugation DPR", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "Y");
        }

        mhd = fgen.chk_RsysUpd("PRDPP104");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPP104') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDPP104", "DEV_A");

            ICO.add_icon(frm_qstr, "F40114", 4, "SF Production", 3, "../tej-base/sf_prod.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40122", 3, "Job Order Status", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "N", "fin40_e1", "fin40_x1", "fin40pp2_x1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40350", 4, "WIP Columnar Stock Report", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F40115", 4, "Paper_Inspection", 3, "../tej-base/om_pap_insp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp1_e1", "fa-edit");

            //costing label
            ICO.add_icon(frm_qstr, "F40171", 3, "Label Prodn Analysis", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40116", 3, "Label Costing", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40117", 4, "Label Costing Master", 3, "../tej-base/om_label_ms.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40118", 4, "Label Costing Form", 3, "../tej-base/om_label_ts.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp7_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F40351", 4, "Corrugation Process Plan Detail", 3, "../tej-base/om_view_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp4_e1", "fa-edit", "N", "N");
        }

    }

    public void Icon_Ppc_plast(string frm_qstr, string frm_cocd)
    {

    }
    public void Icon_Prodn_plast(string frm_qstr, string frm_cocd)
    {

        // ------------------------------------------------------------------
        // Plastic/rubber Prodn Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("PRDPM101");

        ICO.add_icon(frm_qstr, "F39102", 4, "Prodn Entry", 3, "../tej-base/om_corr_entry.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");

        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPM101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDPM101", "DEV_A");

            ICO.add_icon(frm_qstr, "F39000", 2, "Moulding Production", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F39100", 3, "Prodn Activity", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39101", 4, "Moulding Entry", 3, "../tej-base/om_mldp_entry.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39106", 4, "Painting Entry", 3, "../tej-base/om_mldp_entry.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39111", 4, "Assembly Entry", 3, "../tej-base/om_mldp_entry.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39116", 4, "SF Prodn Entry", 3, "../tej-base/om_prod_sffg.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39117", 4, "Inter Stage Tfr", 3, "../tej-base/om_stg_tfr.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39118", 4, "FG Prodn Entry", 3, "../tej-base/om_prod_sffg.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F39119", 4, "Prodn Entry(Std)", 3, "../tej-base/om_prod_bas.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp1_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F39121", 3, "Moulding Prodn Checklists", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F39126", 4, "Moulding Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39127", 4, "Painting Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39128", 4, "Assembly Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39129", 4, "Down Time Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39130", 4, "Rejection Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            //39131 present in pod-reps and prodpm reps also
            ICO.add_icon(frm_qstr, "F39131", 4, "Prodn (Std) Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");


            ICO.add_icon(frm_qstr, "F39140", 3, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F39141", 4, "Moulding Register", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39142", 4, "Painting Register", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39143", 4, "Assembly Register", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39144", 4, "Down Time Reports(Mld)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39146", 4, "Rejection Reports(Mld)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39150", 4, "More Reports(Mld)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F39171", 3, "Moulding Prodn Analysis", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp4_e4", "fa-edit");
            ICO.add_icon(frm_qstr, "F39172", 4, "Moulding Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp4_e4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39173", 4, "Painting Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp4_e4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39174", 4, "Assembly Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp4_e4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39176", 4, "Moulding OEE Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp4_e4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39119Z", 4, "New Dashboard", 3, "../tej-base/om_dbd_gendb3.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp4_e4", "fa-edit");

            ICO.add_icon(frm_qstr, "F39200", 3, "Material Requests", 3, "-", "-", "Y", "fin39_e1", "fin40_a1", "fin39pp9_e9", "fa-edit");
            ICO.add_icon(frm_qstr, "F39201", 4, "Matl Issue Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp9_e9", "fa-edit");
            ICO.add_icon(frm_qstr, "F39206", 4, "Matl Return Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp9_e9", "fa-edit");
            ICO.add_icon(frm_qstr, "F39211", 4, "Matl JobWork Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp9_e9", "fa-edit");
        }


        if (frm_cocd == "KESR")
        {
            ICO.add_icon(frm_qstr, "F39152", 4, "Molding Production Plan Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
        }

        mhd = fgen.chk_RsysUpd("PRDPM102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDPM102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDPM102", "DEV_A");

            ICO.add_icon(frm_qstr, "F39221", 4, "Manpower Efficiency Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39222", 4, "Machine Efficiency Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39223", 4, "Machine Utlisation", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39224", 4, "Daily Production Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39225", 4, "Shift Production Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F39226", 4, "Production Summary Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39227", 4, "Rejection Analysis Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39228", 4, "Monthly Breakdown Report ", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F39229", 4, "Runner Consumption Report", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39230", 4, "Rejection tfr Slip", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39231", 4, "Moulding to Component Store", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39232", 4, "Mould Utilization Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F39151", 4, "More Checklists(Mld)", 4, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F39153", 4, "Prod,Rej,OEE Day Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39154", 4, "Prod,Rej,OEE Shift Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39155", 4, "Prod,Rej,OEE Month Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39156", 4, "Prod,Rej,OEE Day+Supr Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39157", 4, "Prod,Rej,OEE M/C Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39158", 4, "Prod,Rej,OEE Item Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39159", 4, "Prod,Rej,OEE Year Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39160", 4, "Prod,Rej,OEE M/C+Item Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39161", 4, "Prod,Rej,OEE M/C+Item+Shift Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39162", 4, "Work Order Compliance Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39163", 4, "Rejectin (First + Second Stage) Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39164", 4, "Consumption Done Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39165", 4, "Month Wise Prod. Quantitative Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39166", 4, "Production Register Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39167", 4, "Month Wise Tool Change Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39168", 4, "Year Wise Tool Change Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39169", 4, "Item Wise Tool Change Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39170", 4, "Supervisor Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39181", 4, "Supervisor ,Shift Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39182", 4, "Supervisor ,Shift ,M/c Wise Checklist", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F39233", 4, "Rejection Reason Analysis Report (Month Wise)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39234", 4, "Rejection Reason Analysis Report (Item Wise)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39235", 4, "Rejection Reason Analysis Report  (Machine Wise)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39236", 4, "Rejection Reason Analysis Report (Shift Wise)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39237", 4, "Rejection Reason Analysis Report (Sub Group Wise)", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39238", 4, "Production Summary Report- Shift & Machine Wise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39239", 4, "Prodn,Rej,OEE(M/c,Item,Year) Report", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39240", 4, "Moulding Detailed Production Print", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");

            //repots in prodpp also
            // ICO.add_icon(frm_qstr, "F40140", 4, "Production Slip", 3, "../tej-base/om_prt_prodpp.aspx", "-", "-", "fin40_e1", "fin40_a1", "fin40pp3_e1", "fa-edit", "N", "y");
            ICO.add_icon(frm_qstr, "F39241", 4, "Production Summary", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39242", 4, "Details of Items  produced", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39243", 4, "Production with Rej % Itemwise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39244", 4, "Costing Print", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39245", 4, "Production Summary Monthwise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39246", 4, "Production Summary Machine wise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39247", 4, "Details of Items  rejected", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F39248", 4, "Down time Month wise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39249", 4, "Down time Machine wise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39250", 4, "Down time Mth,M/c,item wise", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F39183", 4, "Item Below Min. Level (Component Store)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp2_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F39251", 4, "Goods Imported -Annexure III", 3, "../tej-base/om_prt_prodpm.aspx", "-", "-", "fin39_e1", "fin40_a1", "fin39pp3_e3", "fa-edit", "N", "N");
        }
    }

    public void Icon_Ppc_Metal(string frm_qstr, string frm_cocd)
    {


    }
    public void Icon_Prodn_Metal(string frm_qstr, string frm_cocd)
    {
        //------------------------
        // Sheet metal prodn
        //------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("PRDSM101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PRDSM101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRDSM101", "DEV_A");

            ICO.add_icon(frm_qstr, "F43000", 2, "Auto Comp Production", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F43100", 3, "Prodn Activity", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F43101", 4, "Press Shop Entry", 3, "../tej-base/om_mldp_entry.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F43106", 4, "Paint Shop Entry", 3, "../tej-base/om_mldp_entry.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F43111", 4, "Assy. Shop Entry", 3, "../tej-base/om_mldp_entry.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F43116", 4, "SF Prodn Entry", 3, "../tej-base/om_prod_sffg.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F43117", 4, "Inter Stage Tfr", 3, "../tej-base/om_stg_tfr.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F43118", 4, "FG Prodn Entry", 3, "../tej-base/om_prod_sffg.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F43121", 3, "Auto Comp Prodn Checklists", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F43126", 4, "Press Shop Checklist(SM)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43127", 4, "Paint Shop Checklist(SM)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43129", 4, "Down Time Checklist(SM)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43130", 4, "Rejection Checklist(SM)", 3, "../tej-base/om_view_prodpm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F43140", 3, "Auto Comp Prodn Reports", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "fin43pp1_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F43141", 4, "Press Shop Register(SM)", 3, "../tej-base/om_prt_prodsm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43142", 4, "Paint Shop Register(SM)", 3, "../tej-base/om_prt_prodsm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43143", 4, "Assy. Shop Register(SM)", 3, "../tej-base/om_prt_prodsm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43144", 4, "Down Time Reports(SM)", 3, "../tej-base/om_prt_prodsm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43146", 4, "Rejection Reports(SM)", 3, "../tej-base/om_prt_prodsm.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e3", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F43171", 3, "Auto Comp Prodn Analysis", 3, "-", "-", "Y", "fin43_e4", "fin40_a1", "fin43pp1_e4", "fa-edit");
            ICO.add_icon(frm_qstr, "F43172", 4, "Press Shop Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43173", 4, "Paint Shop Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e4", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F43174", 4, "Assy. Shop Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e4", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F43200", 3, "Material Requests", 3, "-", "-", "Y", "fin43_e1", "fin40_a1", "fin43pp1_e9", "fa-edit");
            ICO.add_icon(frm_qstr, "F43201", 4, "Matl Issue Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e9", "fa-edit");
            ICO.add_icon(frm_qstr, "F43206", 4, "Matl Return Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e9", "fa-edit");
            ICO.add_icon(frm_qstr, "F43211", 4, "Matl JobWork Request", 3, "../tej-base/om_prd_req.aspx", "-", "-", "fin43_e1", "fin40_a1", "fin43pp1_e9", "fa-edit");
        }
    }

    public void Icon_Crm(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Pre Sale / Lead Managament
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("CRM101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('CRM101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "CRM101", "DEV_A");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F45050", 2, "CRM Module", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F45100", 3, "CRM Activity", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45101", 4, "Lead Logging", 3, "../tej-base/om_lead_log.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45106", 4, "Lead Followup", 3, "../tej-base/om_lead_act.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45107", 4, "Leads Target Setting", 3, "../tej-base/om_crm_tgt.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR1_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F45116", 3, "CRM Reports", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45121", 4, "Lead Log List", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45126", 4, "Lead Followup List", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45131", 4, "Lead Status List", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45132", 4, "Industry Wise Leads", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45133", 4, "Sales Agent Wise Leads", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45134", 4, "Target Vs Actual Leads", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F45140", 3, "CRM Dashboards", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45141", 4, "Lead Mgmt Dashboard", 3, "../tej-base/om_dbd_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit");



            ////ICO.add_icon(frm_qstr, "F45156", 1, "CRM Masters", 3, "-", "-", "Y", "fin45_e4", "fin45_a4", "-", "fa-edit");
            ////ICO.add_icon(frm_qstr, "F45161", 2, "CRM Status Master", 3, "../tej-base/om_Typ_mst.aspx", "-", "Y", "fin45_e4", "fin45_a4", "-", "fa-edit");

            // ------------------------------------------------------------------
            // Customer Complaint Redressal
            // ------------------------------------------------------------------

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
        }
        mhd = fgen.chk_RsysUpd("CRM102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('CRM102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "CRM102", "DEV_A");

            ICO.add_icon(frm_qstr, "F45143", 4, "CRM Leads Followup Review", 3, "../tej-base/om_dbd_mgrph.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR3_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45160", 3, "CRM Masters", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45161", 4, "CRM Contacts Master", 3, "../tej-base/om_crm_Contact.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F45162", 4, "CRM Contacts Lists", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F45165", 4, "Lead Status Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "N");
        }
        ICO.add_icon(frm_qstr, "F45166", 4, "Analysis Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit", "N", "N");


        ICO.add_icon(frm_qstr, "F45170", 3, "Marketing Structure", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "fin45CR5_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F45172", 4, "01) RSM Masters", 3, "../tej-base/om_tgpop_mst.aspx", "Regional Sales Managers Master", "-", "fin45_e1", "fin45_a1", "fin45CR5_e1", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F45174", 4, "02) ASM Masters", 3, "../tej-base/om_tgpop_mst.aspx", "Area Sales Managers Master", "-", "fin45_e1", "fin45_a1", "fin45CR5_e1", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F45176", 4, "03) TSM Masters", 3, "../tej-base/om_tgpop_mst.aspx", "Territory Sales Managers Master", "-", "fin45_e1", "fin45_a1", "fin45CR5_e1", "fa-edit", "N", "N");

        ICO.add_icon(frm_qstr, "F45139", 4, "Type Wise Lead Analysis", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45144", 4, "Source Wise Lead Analysis", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45145", 4, "Country Wise Lead Analysis", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45146", 4, "State Wise Lead Analysis", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45147", 4, "Action taken Wise Lead Analysis", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45148", 4, "Lead Action Stage Wise Analysis", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45149", 4, "Lead Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F45150", 4, "Lead History", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR2_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F45151", 4, "Lead Action Stage Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin45_e1", "fin45_a1", "fin45CR4_e1", "fa-edit");

    }

    public void Icon_Mkt_ord(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Sales order Management ( Dom)
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DOMSO101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DOMSO101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DOMSO101", "DEV_A");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47100", 3, "Dom.Order Activity", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47101", 4, "Master S.O. (Dom.)", 3, "../tej-base/om_so_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47106", 4, "Supply S.O. (Dom.)", 3, "../tej-base/om_so_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47111", 4, "Sales Schedule", 3, "../tej-base/om_sale_sch.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47111D", 4, "Schedule Day Wise", 3, "../tej-base/om_day_sch_DG.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47112", 4, "Sales Budget", 3, "../tej-base/om_sale_budg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47116", 4, "Sales Projection", 3, "../tej-base/om_splan_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

            //-> to correct 
            ICO.add_icon(frm_qstr, "F47121", 3, "Dom.Sales Approvals", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47126", 4, "Check S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47127", 4, "Approve S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47127M", 4, "Approve Master S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47128", 4, "Sales Schedule Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit");

            ICO.add_icon(frm_qstr, "F47131", 3, "Dom.Orders Checklists", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47132", 4, "Master S.O. Checklists(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47133", 4, "Supply S.O. Checklists(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47134", 4, "Supply Sch. Checklists(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47135", 4, "Schedule Vs Dispatch  (Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47136", 4, "Pending Order Checklist(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F47140", 3, "Dom.Order Reports", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47141", 4, "All Order Register(Dom.)", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47142", 4, "Pending Order Register(Dom.)", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F47222", 4, "Order Vs Dispatch", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47223", 4, "Schedule Vs Dispatch", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47239", 4, "Bill Wise Shipment", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47224", 4, "Schedule Status (Daily)", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47225", 4, "Schedule Status (Monthly)", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47226", 4, "Rate Trend Chart Product Wise", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47227", 4, "Rate Trend Chart Customer Wise", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F47228", 4, "Bill wise Month wise Sales Detail", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47229", 4, "Month wise Sales Summary", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47230", 4, "Customer wise, Item wise Sales Summary", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47231", 4, "Customer wise Sales Summary", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47232", 4, "Customer Part wise Sales Summary", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47233", 4, "Item wise Sales Summary", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin70_e1", "fin45_a1", "fin47_e4pp", "fa-edit", "N", "Y");


            ICO.add_icon(frm_qstr, "F47151", 3, "Dom.Order Analysis", 3, "-", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47152", 4, "Dom.Orders Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47153", 4, "12 Month S.O.Qty Trend(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47154", 4, "12 Month S.O.Value Trend(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47155", 4, "12 Month Sch.Qty Trend(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47156", 4, "12 Month Sch.Value Trend(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47158", 4, "Dom.Mktg Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47159", 4, "Dom.Mktg Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit");

            ICO.add_icon(frm_qstr, "F47161", 3, "Dom.Order Masters", 3, "-", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47162", 4, "S.O.Closure (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47163", 4, "Currency Type Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47164", 4, "Contract Terms Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47165", 4, "Payment Terms Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F47166", 4, "Sales Projection Vs Sales Qty", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e5pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47117", 4, "Sale Schedule Bulk Upload", 3, "../tej-base/om_multi_saleSch_Upl.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
        }
    }

    public void Icon_Truck_Monitoring(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Sales order Management ( Dom)
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("TRU101");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "TRU101", "DEV_A");

            ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F50550", 2, "Truck Monitoring", 3, "-", "-", "Y", "fin50_tr1", "fin50_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47125", 3, "Supervisior Master", 3, "../tej-base/personmst.aspx", "-", "-", "fin50_tr1", "fin50_a1", "fin50tr_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F50551", 3, "Truck Assignment", 3, "../tej-base/om_Truck_Dtl.aspx", "-", "-", "fin50_tr1", "fin50_a1", "fin50tr_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F50137A", 3, "Truck Details", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_tr1", "fin50_a1", "fin50tr_e1", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50137B", 3, "Truck Entry Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_tr1", "fin50_a1", "fin50tr_e1", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50555", 3, "Truck Attachment View", 3, "../tej-base/om_truck_imgview.aspx", "-", "-", "fin50_tr1", "fin50_a1", "fin50tr_e1", "fa-edit");
        }
    }

    public void Icon_Mkt_ord_Exp(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Sales order Management (Exp)
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("EXPSO101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('EXPSO101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "EXPSO101", "DEV_A");

            ICO.add_icon(frm_qstr, "F49000", 2, "Export Sales Orders", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F49100", 3, "Exp.Order Activity", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49101", 4, "Proforma Inv. (Exp.)", 3, "../tej-base/om_eso_entry.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49106", 4, "Supply S.O. (Exp.)", 3, "../tej-base/om_eso_entry.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49111", 4, "Sales Schedule(Exp.)", 3, "../tej-base/om_sale_sch.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e1pp", "fa-edit");

            ICO.add_icon(frm_qstr, "F49121", 3, "Exp.Sales Approvals", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49126", 4, "Check S.O. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F49127", 4, "Approve S.O. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F49128", 4, "Sales Schedule Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49129", 4, "Check P.I. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F49130", 4, "Approve P.I. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e2pp", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F49131", 3, "Exp.Orders Checklists", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e3pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49132", 4, "Sales Order Checklists(Exp.)", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e3pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F49133", 4, "Customer Orders(Exp.)", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e3pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F49134", 4, "Product. Orders(Exp.)", 3, "../tej-base/om_view_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e3pp", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F49140", 3, "Exp.Order Reports", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49141", 4, "All Order Register(Exp.)", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F49142", 4, "Pending Order Register(Exp.)", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F49143", 4, "Export Proforma Invoice Print", 3, "../tej-base/om_prt_emktg.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e4pp", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F49151", 3, "Exp.Order Analysis", 3, "-", "-", "-", "fin49_e1", "fin45_a1", "fin49_e5pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F49152", 4, "Exp.Orders Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e5pp", "fa-edit");
        }
        ICO.add_icon(frm_qstr, "F49159", 4, "Exp.Mktg Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin49_e1", "fin45_a1", "fin49_e5pp", "fa-edit");
    }

    public void Icon_Mkt_Sale(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Domestic Sales Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DOMSL101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DOMSL101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DOMSL101", "DEV_A");

            ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F50100", 3, "Dom.Sales Activity", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F50101", 4, "Sales Invoice (Dom.)", 3, "../tej-base/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F50106", 4, "Proforma Invoice (Dom.)", 3, "../tej-base/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F50111", 4, "Dispatch Advice (Dom.)", 3, "../tej-base/om_Da_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");

            //ICO.add_icon(frm_qstr, "F50144", 4, "Domestic Proforma Invoice Print", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "-", "fa-edit", "N", "N");            

            ICO.add_icon(frm_qstr, "F50113", 4, "E Way Bill Update", 3, "../tej-base/om_eway_bill.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F50114", 4, "Prodn Entry(Std)", 3, "../tej-base/om_prod_bas.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F50115", 4, "Invoice Reach Record", 3, "../tej-base/om_Inv_Reach.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F50121", 3, "Dom.Orders CheckLists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F50126", 4, "Order Data Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50127", 4, "Pending Order Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50128", 4, "Pending Sch. Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F50129", 4, "Sales Data / DMP", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit", "Y", "Y");

            ICO.add_icon(frm_qstr, "F50131", 3, "Dom.Sales Checklists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F50132", 4, "Sales Data Checklists(Dom.)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50133", 4, "Customer Wise Sales(Dom.)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50134", 4, "Product. Wise Sales(Dom.)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e3", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F50140", 3, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit");
            ICO.add_icon(frm_qstr, "F50141", 4, "Sales Register(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50142", 4, "Customer Wise Reg.(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50143", 4, "Product. Wise Reg.(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F50151", 3, "Dom.Sales Analysis", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit");
            ICO.add_icon(frm_qstr, "F50152", 4, "Dom.Sales Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit");
            ICO.add_icon(frm_qstr, "F50156", 4, "Plant Wise Sales", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50158", 4, "Dom.Sales Live M.I.S", 3, "../tej-base/om_dbd_live.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit");
            ICO.add_icon(frm_qstr, "F50159", 4, "Dom.Sales Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit");

            ICO.add_icon(frm_qstr, "F50200", 3, "Dom.Sales Masters", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit");
            ICO.add_icon(frm_qstr, "F50201", 4, "Sale Inv. Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50203", 4, "Currency Type Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50205", 4, "Contract Terms Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50207", 4, "Payment Terms Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50208", 4, "Consignee Master", 3, "../tej-base/om_csmst.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50209", 4, "Mode of Transport Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e6", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50221", 3, "More Reports(Dom.Sales)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50222", 4, "Party Wise Total Sales Summary(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50223", 4, "Product Wise Total Sales Summary(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50224", 4, "Party Wise 12 Month Sales Qty(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50225", 4, "Party Wise 12 Month Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50226", 4, "Product Wise 12 Month Sales Qty(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50227", 4, "Product Wise 12 Month Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50228", 4, "31 Day Wise Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50231", 4, "District Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50232", 4, "State Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50233", 4, "Zone Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50234", 4, "Marketing Person Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50235", 4, "Customer Group Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50236", 4, "Product Sub Group Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50240", 4, "Schedule Vs Dispatch 31 Day", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50241", 4, "Schedule Vs Dispatch 12 Month", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50242", 4, "Schedule Vs Prodn Vs Dispatch Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50244", 4, "Schedule Vs Dispatch Cust Wise Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50245", 4, "Schedule Vs Dispatch Cust Wise,Item Wise Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50250", 4, "Schedule Vs Dispatch Qty Year on Year", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50251", 4, "Schedule Vs Dispatch Value Year on Year", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50255", 4, "Products Where Sales are Growing", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50256", 4, "Customers Where Sales are Growing ", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50257", 4, "Products Where Sales are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50258", 4, "Customers Where Sales are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");

            //repeat icon seen 22/8/2020
            //ICO.add_icon(frm_qstr, "F50257", 4, "Products Where Schedule are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F50258", 4, "Customers Where Schedule are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F50264", 4, "Products Wise Sales Vs Returns , PPM", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50265", 4, "Customer,Product Wise Sales Vs Returns, PPM", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "Y");

            //#~#to be check 
            ICO.add_icon(frm_qstr, "F50159G", 3, "DOM.Sales Review (Graph)", 3, "../tej-base/om_dbd_mgrph.aspx", "-", "-", "fin50_e5", "fin50_a1", "-", "fa-edit", "N", "Y");
        }

        // added vipin 25/08 -- form made by suman
        ICO.add_icon(frm_qstr, "F50160", 4, "Dom.Sales Special Report", 3, "../tej-base/om_finsys_options.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit");
        ICO.add_icon(frm_qstr, "F50161", 4, "Dom.Sales All Report", 3, "../tej-base/om_sales_reports.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e5", "fa-edit");

        mhd = fgen.chk_RsysUpd("DOMSL102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DOMSL102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DOMSL102", "DEV_A");

            if (frm_cocd == "ERAL")
            {
                ICO.add_icon(frm_qstr, "F50266", 4, "Mat lying with godown-invoice wise summary", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50267", 4, "Mat lying with godown-item wise detail", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F50268", 4, "Mat lying with godown-item wise summary", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50pp_e4", "fa-edit", "N", "N");
            }

            ICO.add_icon(frm_qstr, "F50135", 3, "Stock summary + Analysis", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50136", 3, "Dom.Mktg Review System", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F50180", 3, "Sales Order Tracking", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50181", 3, "Sales Schedule Tracking", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit", "N", "N");
        }
        mhd = fgen.chk_RsysUpd("DOMSL103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DOMSL103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DOMSL103", "DEV_A");

            ICO.add_icon(frm_qstr, "F50310", 3, "Marketing Reports(Sales Module)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50311", 4, "11 Col with items", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50312", 4, "10 Col sales except items", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50313", 4, "2 Col Party & Gross amount", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F50314", 4, "5 Col Basic GST Gross", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50315", 4, "Top X selling items", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F50316", 4, "Country wise Sales", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "N");
            //ICO.add_icon(frm_qstr, "F50317", 4, "Sales & Rejection Summary", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F50318", 4, "11 Col with items", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F50319", 4, "11 Col with items", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F50320", 4, "11 Col with items", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e47", "fa-edit", "N", "Y");
        }

    }

    public void Icon_Mkt_Sale_Exp(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Export Sales Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("EXPSL101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('EXPSL101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "EXPSL101", "DEV_A");

            ICO.add_icon(frm_qstr, "F55000", 2, "Export Sales Module", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55100", 3, "Exp.Sales Activity", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F55101", 4, "Sales Invoice (Exp.)", 3, "../tej-base/om_einv_entry.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F55106", 4, "Proforma Invoice (Exp.)", 3, "../tej-base/om_einv_entry.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F55121", 3, "Exp.Orders CheckLists", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F55126", 4, "Order Data Checklist", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F55127", 4, "Pending Order Checklist", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F55128", 4, "Pending Sch. Checklist", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e2", "fa-edit");

            ICO.add_icon(frm_qstr, "F55131", 3, "Exp.Sales Checklists", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F55132", 4, "Sales Data Checklists(Exp.)", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F55133", 4, "Customer Wise Sales(Exp.)", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F55134", 4, "Product. Wise Sales(Exp.)", 3, "../tej-base/om_view_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e3", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F55140", 3, "Exp.Sales Reports", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit");
            ICO.add_icon(frm_qstr, "F55141", 4, "Sales Register(Exp.)", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F55142", 4, "Customer Wise Reg.(Exp.)", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F55143", 4, "Product. Wise Reg.(Exp.)", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F55151", 3, "Exp.Sales Analysis", 3, "-", "-", "Y", "fin55_e1", "fin50_a1", "fin55pp_e5", "fa-edit");
            ICO.add_icon(frm_qstr, "F55152", 4, "Exp.Sales Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e5", "fa-edit");
        }
        ICO.add_icon(frm_qstr, "F55145", 4, "Export Invoice- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F55146", 4, "Packing List- Print", 3, "../tej-base/om_prt_esale.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e4", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F55111", 4, "Dispatch Advice (Exp.)", 3, "../tej-base/om_Da_entry.aspx", "-", "-", "fin55_e1", "fin50_a1", "fin55pp_e1", "fa-edit");
    }

    public void Icon_Acctg(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Accounts Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("ACT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('ACT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "ACT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Activity", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70101", 3, "Receipt Vouchers", 3, "../tej-base/om_rcpt_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70106", 3, "Payment Vouchers", 3, "../tej-base/om_rcpt_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F70111", 3, "Journal Vouchers", 3, "../tej-base/om_jour_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70116", 3, "Purchase Vouchers", 3, "../tej-base/om_vch_entry.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70117", 3, "Bank Reco.", 3, "../tej-base/om_bank_reco.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70119", 3, "Expense Budget", 3, "../tej-base/om_exp_budg.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");


            ICO.add_icon(frm_qstr, "F70121", 2, "Accounts Checklists", 3, "-", "-", "Y", "fin70_e2", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70124", 3, "Verify Auto Dr/Cr Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F70126", 3, "Rcpts. Checklists", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70127", 3, "Pymts. Checklists", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70128", 3, "J.V.   Checklists", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70129", 3, "Purch. Checklists", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70131", 2, "Accounts Registers", 3, "-", "-", "Y", "fin70_e3", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70132", 3, "Rcpts. Register", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70133", 3, "Pymts. Register", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70134", 3, "J.V.   Register", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70135", 3, "Purch. Register", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70136", 3, "Cheque Issue Register", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70137", 3, "Bank Reco. Print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70141", 3, "Statement of A/c", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70556", 3, "Detail Statement", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

            ICO.add_icon(frm_qstr, "F70142", 3, "Group Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70143", 3, "Type Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70144", 3, "Receivable Ageing ", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70146", 3, "Payable Ageing ", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70147", 3, "More Checklists(Accounts)", 3, "-", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70148", 4, "Balance Sheet Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70149", 4, "Balance Sheet Detail", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70151", 3, "Trial Balance- 4 Col", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70156", 3, "Balance Sheet", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70161", 3, "Yearly Comparison", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70172", 3, "Accounts Master", 3, "../tej-base/acct_gen.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70173", 3, "Accounts Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70176", 3, "Voucher Types", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70174", 3, "Accounts Schedules", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70180", 3, "Block/Cancel Voucher", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");



            ICO.add_icon(frm_qstr, "F70181", 3, "Multi A/c Master", 3, "../tej-base/om_multi_account.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70182", 3, "District Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70183", 3, "Country Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70185", 3, "Party wise balance update", 3, "../tej-base/om_multi_account_upt.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70186", 3, "Bill wise Dr/ Cr balance upload", 3, "../tej-base/om_multi_bill.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70187", 3, "Accounts Balance update", 3, "../tej-base/om_multi_balance.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");



            ICO.add_icon(frm_qstr, "F70221", 3, "More Reports(Accounts)", 3, "-", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70222", 4, "Cash Book", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70223", 4, "Bank Book", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70224", 4, "Sales Register", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70225", 4, "Purchase Register", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            // 70226 ICON AVAILABLE FOR OTHER REPORT AS THE FORM IS ON 70282
            //ICO.add_icon(frm_qstr, "F70226", 4, "Accounts Review", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70227", 4, "Net Sales Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70228", 4, "Expense Trend", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70229", 4, "P & L Trend Mthly", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70230", 4, "P & L Trend Qtrly", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70231", 4, "Day Book:Rcpts", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70232", 4, "Day Book:Payments", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70233", 4, "Day Book:Journal", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70234", 4, "Day Book:Sales", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70235", 4, "Day Book:Purchase", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70236", 4, "Day Book:Cash", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70239", 4, "Day Book:Bank", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70237", 4, "Trial Balance 2 Col", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70238", 4, "Trial Balance 6 Col", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70241", 4, "P & L Schedule wise", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70242", 4, "P & L Detailed", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70243", 4, "Int on unsecured Loan- Daywise", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70244", 4, "Int on unsecured Loan- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "Y", "N");

            ICO.add_icon(frm_qstr, "F70245", 4, "Purchase Register (Print)", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70246", 4, "Sale Register(Print)", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70247", 4, "Voucher List( Drill Down)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70248", 4, "Cash more than Rs. 10,000", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70249", 4, "Top 10 Debtors", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70250", 4, "Top 10 Creditors", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70251", 4, "Creditors with Debit Balance", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70252", 4, "Sales Trend", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70253", 4, "Purchase Trend", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70254", 4, "Print Purchase Voucher(s)(non 50)", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70255", 4, "Print GST Purchase Voucher with MRR details", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70256", 4, "Bill wise Month wise Sales Detail", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70257", 4, "Month wise Sales Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70258", 4, "Customer wise Sales Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70259", 4, "Collection Trend- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70260", 4, "Collection Trend Party wise- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70261", 4, "Purchase Trend - Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70262", 4, "Purchase Trend Party wise- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70263", 4, "Sales Trend- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70264", 4, "Sales Trend Party wise- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70265", 4, "Expenses Trend - Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70266", 4, "Expense Trend Group wise- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70267", 4, "Profit Trend Branchwise- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70268", 4, "Profit Trend Consolidated- Graph", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70269", 4, "Accounts Ledger", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70270", 4, "Debtors' Ageing (detailed)- print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70271", 4, "Creditors' Ageing (detailed)- print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70272", 4, "Net Sales Report- Print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70273", 4, "Net Purchase Report", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70274", 4, "Net Purchase Report- Print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70275", 4, "Month wise Sales Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70276", 4, "Customer wise, Item wise Sales Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70277", 4, "Customer wise Sales Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70278", 4, "Customer Part wise Sales Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70279", 4, "Item wise Sales Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70280", 4, "Creditors Ageing (detailed with on account)- print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70281", 4, "Debtors Ageing (detailed with on account)- print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");


            ICO.add_icon(frm_qstr, "F70298", 4, "Cross Year Accounts Ledger-Print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "Y", "N");
            ICO.add_icon(frm_qstr, "F70299", 4, "Sales Summary with Debit Credit Notes-Summary ", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70300", 4, "Sales Summary with Debit Credit Notes-Detailed", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70301", 4, "Sales Trend (HSN Qty wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70302", 4, "Sales Trend (HSN Value wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70303", 4, "Sales Trend (HSN Qty wise)- Detailed", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70304", 4, "Sales Trend (HSN Value wise)- Detailed", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70305", 4, "Sales Trend (HSN, Type Qty wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70306", 4, "Sales Trend (HSN, Type Value wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70307", 4, "Sales Trend (HSN, Party Qty wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70308", 4, "Sales Trend (HSN, Party Value wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70309", 4, "Purchase Trend (HSN Qty wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70310", 4, "Purchase Trend (HSN Value wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70311", 4, "Purchase Trend (HSN Qty wise)- Detailed", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70312", 4, "Purchase Trend (HSN Value wise)- Detailed", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70313", 4, "Purchase Trend (HSN, Type Qty wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70314", 4, "Purchase Trend (HSN, Type Value wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70315", 4, "Purchase Trend (HSN, Party Qty wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70316", 4, "Purchase Trend (HSN, Party Value wise)- Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70317", 4, "HSN wise Sales Summary( Drill Down)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70318", 4, "Sales Summary( Drill Down)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70319", 4, "Accounts Ledger( Drill Down)", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");


            ICO.add_icon(frm_qstr, "F70555", 3, "Chq Printing", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70370", 2, "Document Keeping", 3, "-", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70371", 3, "Voucher Upload", 3, "../tej-base/vch_upl.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70372", 3, "Voucher Approval", 3, "../tej-base/vch_apr.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70373", 3, "Voucher View", 3, "../tej-base/vch_vw.aspx", "-", "Y", "fin70_e7", "fin70_a1", "-", "fa-edit");


            //#~#to be check 
            // ICON DOUBLE ICO.add_icon(frm_qstr, "F70148", 4, "Debit Note checklist", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70138", 3, "HSN wise FG Stock Summary-checklist", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70139", 3, "Purch. Voucher", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70184", 3, "Accounts Master", 3, "../tej-base/acct_gen.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70150", 3, "Debit Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70188", 3, "GST Rate Sale Invoice wise Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70152", 3, "HSN wise FG Stock Summary", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70153", 3, "Tax Rate wise basic tax Summary", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");

        }
        mhd = fgen.chk_RsysUpd("ACT102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('ACT102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "ACT102", "DEV_A");

            ICO.add_icon(frm_qstr, "F70282", 4, "Accounts Review Analysis", 3, "../tej-base/om_ActIt_review.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70177", 3, "TAX Rates Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70189", 3, "P & L Account", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set visi='N' where id='F70184'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set text='GST/HSN TAX Rates Master' where id='F70177'");
        }
        //ICO.add_icon(frm_qstr, "F25250", 3, "MRR Costing", 3, "../tej-base/om_mrr_costing.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70335", 3, "Payment Reminder Letter", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "Y", "N");

    }


    //a5 view HSN wise Non MRR purchase

    public void Icon_FA_sys(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // FA register Module
        // ------------------------------------------------------------------

        string mhd = "";
        mhd = fgen.chk_RsysUpd("FAR101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('FAR101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "FAR101", "DEV_A");

            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70401", 2, "Fixed Asset Module", 3, "-", "-", "Y", "fin70_e6", "fin70_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F70402", 3, "Fixed Asset Purchase Activity", 3, "../tej-base/om_fixed_asset_pur.aspx", "-", "-", "fin70_e6", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70403", 3, "Fixed Asset Sale/Disposal Activity", 3, "../tej-base/om_fixed_asset_sale.aspx", "-", "-", "fin70_e6", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70404", 3, "Depreciation Calculation-Companies Act", 3, "../tej-base/om_depr_comp.aspx", "-", "-", "fin70_e6", "fin70_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70424", 3, "Depreciation Calculation-IT Block wise", 3, "../tej-base/om_depr_it.aspx", "-", "-", "fin70_e6", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70415", 3, "Depreciation Write Back", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e6", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70423", 3, "Assets Revaluation Activity", 3, "../tej-base/om_asset_adjust.aspx", "-", "-", "fin70_e6", "fin70_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70427", 3, "Fixed Assets-Masters", 3, "-", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfam", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70419", 4, "Fixed Asset IT Block Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfam", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70420", 4, "Fixed Asset Companies Act Group Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfam", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70421", 4, "Fixed Asset Location Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfam", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70405", 4, "Fixed Asset Excel Upload", 3, "../tej-base/om_fa_upload.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfam", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70428", 3, "Asset Verification Module", 3, "-", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPav", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70410", 4, "Print Asset Identification Tags", 3, "../tej-base/om_prt_acct.aspx", "Print Tags for Identification", "-", "fin70_e6", "fin70_a1", "fin70_MREPav", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70411", 4, "Asset Tag configuration Form", 3, "../tej-base/om_asset_stkr.aspx", "Tag Configuration Form", "-", "fin70_e6", "fin70_a1", "fin70_MREPav", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70426", 4, "Asset Insurance Report", 3, "../tej-base/om_view_acct.aspx", "Assets Insurance Details", "-", "fin70_e6", "fin70_a1", "fin70_MREPav", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70422", 4, "Assets Tie Up Report ", 3, "../tej-base/om_view_acct.aspx", "Physical and Booked Assets Reconciliation Report", "-", "fin70_e6", "fin70_a1", "fin70_MREPav", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F70429", 3, "Fixed Asset-Register & Reports", 3, "-", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70406", 4, "Depreciation Chart- Asset Code wise", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart detailed grouped by acode", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70407", 4, "Fixed Asset Register", 3, "../tej-base/om_prt_acct.aspx", "FA installed & sold during period", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70408", 4, "Sold Asset Register", 3, "../tej-base/om_prt_acct.aspx", "FA sold during period", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70409", 4, "List of Additions- Fixed Assets", 3, "../tej-base/om_prt_acct.aspx", "FA installed during period", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70413", 4, "Depreciation Chart- Group wise Summary", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart-CA detailed gp by groups", "Y", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70416", 4, "Depreciation Chart- Location wise Summary", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart-CA detailed gp by Location", "Y", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70417", 4, "Depreciation Chart- Department wise Summary", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart-CA detailed gp by Department", "Y", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70418", 4, "Depreciation Chart- Block wise Summary", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart-CA detailed gp by Block", "Y", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70412", 4, "Depreciation Chart-Co. Act", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart- Schedule II Companies Act,2013", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70414", 4, "Fixed Asset Ledger", 3, "../tej-base/om_prt_acct.aspx", "FA Ledger with image", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70425", 4, "Depreciation Chart-IT Act", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart- Income Tax Act,1961", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");

        }

        mhd = fgen.chk_RsysUpd("FAR102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('FAR102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "FAR102", "DEV_A");

            ICO.add_icon(frm_qstr, "F70430", 3, "Fixed Assets-Checklists", 3, "-", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfach", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70431", 4, "Assets Covered under Warranty", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfach", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70432", 4, "Fixed Asset Register- Location", 3, "../tej-base/om_prt_acct.aspx", "Existing FA Register location wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70433", 4, "Fixed Asset Register- Department", 3, "../tej-base/om_prt_acct.aspx", "Existing FA Register department wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70434", 4, "Fixed Asset Register- Owner wise", 3, "../tej-base/om_prt_acct.aspx", "Existing FA Register Owner wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70435", 4, "Fixed Asset Register- Warranty", 3, "../tej-base/om_prt_acct.aspx", "Existing FA Register Warranty wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F70436", 4, "Depreciation Chart-IT Act", 3, "../tej-base/om_prt_acct.aspx", "Dep Chart- Income Tax Act Asset Block wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F70437", 4, "Fixed Asset IT Block Opening WDV", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfam", "fa-edit", "N", "N");
        }
        mhd = fgen.chk_RsysUpd("FAR103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('FAR103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "FAR103", "DEV_A");

            ICO.add_icon(frm_qstr, "F70438", 4, "Search Fixed Asset Purchased", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfach", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70439", 4, "Search Fixed Asset Sold", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfach", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70440", 4, "Accounts-FAR Purchased Assets tie-up", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfach", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F70441", 4, "Accounts-FAR Sold Assets tie-up", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e6", "fin70_a1", "fin70_MREPfach", "fa-edit", "N", "N");
        }
        ICO.add_icon(frm_qstr, "F70442", 4, "Fixed Asset WDV", 3, "../tej-base/om_prt_acct.aspx", "FA WDV on selected Date", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "N");
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME = 'WB_FA_STKR'", "TNAME");
        if (mhd == "0")
        {
            mhd = "create table WB_FA_STKR (BRANCHCD CHAR(2) default '-',VCHNUM CHAR(6) default '-',VCHDATE DATE,TYPE CHAR(2) default '-',ASSETID VARCHAR2(6) default '-',FIELD1 VARCHAR2(50) default '-',FIELD2 VARCHAR2(50)default '-',FIELD3 VARCHAR2(50) default '-',ENT_BY VARCHAR2(50)default '-',ENT_DT DATE ,EDT_BY VARCHAR2(50)default '-',EDT_DT DATE)";
            fgen.execute_cmd(frm_qstr, frm_cocd, mhd);
        }
        ICO.add_icon(frm_qstr, "F70443", 4, "Monthly Dep Detailed", 3, "../tej-base/om_view_acct.aspx", "Depreciation Asset wise Month wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F70444", 4, "Monthly Dep Summary", 3, "../tej-base/om_view_acct.aspx", "Depreciation Summary wise Month wise", "-", "fin70_e6", "fin70_a1", "fin70_MREPfar", "fa-edit", "N", "N");

    }

    public void Icon_DrCr_Maruti(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DMUL101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DMUL101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DMUL101", "DEV_A");

            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70099", 3, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70099h", 3, "All Inv File Uploading", 3, "../tej-base/autoDrCrPip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Icon_DrCr_Honda(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DHON101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DHON101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DHON101", "DEV_A");

            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70099h", 3, "All Inv File Uploading", 3, "../tej-base/autoDrCrPip.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Icon_DrCr_Toyota(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DTOY101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DTOY101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DTOY101", "DEV_A");

            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70099a", 3, "All Inv/Excel File Uploading", 3, "../tej-base/fupl1.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Icon_DrCr_self(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DSEL101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DSEL101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DSEL101", "DEV_A");

            ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "P70106C", 3, "Credit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "P70106D", 3, "Debit Note", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Icon_Hrm(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Training Module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("HRM101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('HRM101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "HRM101", "DEV_A");

            // ------------------------------------------------------------------
            // Training Module
            // ------------------------------------------------------------------

            ICO.add_icon(frm_qstr, "F80000", 1, "H.R.M Module", 3, "-", "-", "Y", "-", "fin80_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F80050", 2, "Training Management", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F80100", 3, "Training Activity", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin80pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F80101", 4, "Training Need Identify", 3, "../tej-base/om_train_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F80106", 4, "Training Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F80111", 4, "Training Done Entry", 3, "../tej-base/om_train_done.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F80121", 3, "Training Checklist", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin80pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F80126", 4, "Trng Rqmt Checklist", 3, "../tej-base/om_view_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F80127", 4, "Trng Done Checklist", 3, "../tej-base/om_view_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F80128", 4, "Trng Need Vs Done", 3, "../tej-base/om_view_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F80131", 3, "Training Analysis", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin80pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F80132", 4, "Training Dashboard", 3, "../tej-base/om_dbd_hrm.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F80116", 4, "Training Topics", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin80pp_e3", "fa-edit");

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

            // ------------------------------------------------------------------
            // Self Sevice Docs Module
            // ------------------------------------------------------------------
            ICO.add_icon(frm_qstr, "F82000", 2, "Document Mgmt Module", 3, "-", "-", "Y", "fin82_e1", "fin80_a1", "fin82pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F82100", 3, "Employee Docs", 3, "-", "-", "Y", "fin82_e1", "fin80_a1", "fin82pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F82101", 4, "Upload Tax Docs", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin82_e1", "fin80_a1", "fin82pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F82106", 4, "Approve Tax Docs", 3, "../tej-base/om_appr.aspx", "-", "-", "fin82_e1", "fin80_a1", "fin82pp_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F82121", 3, "Document Checklist", 3, "-", "-", "Y", "fin82_e1", "fin80_a1", "fin82pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F82126", 4, "Tax Docs Checklist", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin82_e1", "fin80_a1", "fin82pp_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F82131", 3, "Document Masters", 3, "-", "-", "Y", "fin82_e1", "fin80_a1", "fin82pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F82132", 4, "Tax Docs Masters", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin82_e1", "fin80_a1", "fin82pp_e3", "fa-edit");
        }
        // ICO.add_icon(frm_qstr, "F82700", 2, "Online HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "fin82pp_e7", "fa-edit");
        //ICO.add_icon(frm_qstr, "F82703", 4, "Leave Request", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin82pp_e7", "fa-edit");
        // ICO.add_icon(frm_qstr, "F82705", 4, "Loan Request", 3, "../tej-base/om_loan_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin82pp_e7", "fa-edit");

    }

    public void Icon_Payr(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // payroll module
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("PAY101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('PAY101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PAY101", "DEV_A");

            ICO.add_icon(frm_qstr, "F85000", 1, "Pay/Salary Module", 3, "-", "-", "Y", "-", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85100", 2, "Payroll Activity", 3, "-", "-", "Y", "fin85_e1", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85101", 3, "Attendance Entry", 3, "../tej-base/om_attn_entry.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85106", 3, "Salary Preparation", 3, "../tej-base/om_pay_data.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F85121", 2, "Loan/Advance Mgt", 3, "-", "-", "Y", "fin85_e2", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85126", 3, "Employee Advance", 3, "../tej-base/om_loan_req.aspx", "-", "-", "fin85_e2", "fin85_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F85127", 3, "Employee Loan", 3, "../tej-base/om_loan_req.aspx", "-", "-", "fin85_e2", "fin85_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F85131", 2, "Payroll Masters", 3, "-", "-", "Y", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85132", 3, "Employee Master", 3, "../tej-base/om_emp_mas.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "Y");
            // ICO.add_icon(frm_qstr, "F85133", 3, "Department Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F85133", 3, "Designation Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F85134", 3, "Pay Calc. Master", 3, "../tej-base/om_sal_mast.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F85141", 2, "Salary Reports", 3, "-", "-", "Y", "fin85_e4", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85142", 3, "Salary Register", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85146", 3, "More Reports(Pay)", 3, "-", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85147", 4, "List of Joining", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85148", 4, "List of Birthdays", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85149", 4, "Joining & Leaving Register", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85150", 4, "Monthly Attendance Register", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
        }

        // pay and employee reports  
        mhd = fgen.chk_RsysUpd("HRM102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('HRM102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "HRM102", "DEV_A");

            ICO.add_icon(frm_qstr, "F82500", 2, "H.R.M. Printable Reports", 3, "-", "-", "Y", "fin82_e5", "fin80_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F82501", 3, "List of Blood Group", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82503", 3, "Category wise Summary", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82505", 3, "Identity Card", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82507", 3, "New Joining Card", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82510", 3, "List of Landine ", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82512", 3, "List of Mobile ", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82514", 3, "Pay Summary", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82515", 3, "Pay Register Quarterly", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82516", 3, "Pay Trend Section Wise", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82517", 3, "Net Pay Trend Deptt wise", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82519", 3, "Salary Rate Report", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82521", 3, "Combined Pay Summary", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82523", 3, "Gross Pay Trend Dept Wise", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82525", 3, "Pay Summary", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82527", 3, "Salary Slips", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82530", 3, "Address List", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F82550", 2, "H.R.M. Checklist", 3, "-", "-", "Y", "fin82_e6", "fin80_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F82552", 3, "Search Employee Master", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82555", 3, "EL Record", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82557", 3, "Grade wise Absense", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F82559", 3, "Grade wise Leaves", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F82562", 3, "Pay Roll Trend- Deptt/Desg Wise,With H/C", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            //ICO.add_icon(frm_qstr, "F82565", 3, "Section wise drill down Summary", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82565", 3, "Deduction Trend", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82567", 3, "Pay Summary", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82569", 3, "Category Designation Deptt Wise ", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82571", 3, "Welfare Report", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82572", 3, "Summary of Present ,EL,CL,SL Emp Wise", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F82573", 3, "List of Leaving", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82574", 3, "New Joining", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82575", 3, "Appraisal Format", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82576", 3, "Confirmation Letter", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82577", 3, "Appointment Letter", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82578", 3, "Gross Pay Register", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82579", 3, "HR Strength Vs Sales Report", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82580", 3, "Master Update Log", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82581", 3, "Section Wise Summary", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin82_e6", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82582", 3, "OT & Incentives", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82583", 3, "Bonus Calculations", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82584", 3, "Annual Income Summ(Head Wise)", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82585", 3, "Welfare Fund Statement", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82586", 3, "Salary Compare Last Mth Vs Curr Mth", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82587", 3, "Late Coming Register", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F82588", 3, "31 Day Late Coming Report", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin82_e5", "fin80_a1", "-", "fa-edit", "N", "N");
        }

        mhd = fgen.chk_RsysUpd("HRM103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('HRM103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "HRM103", "DEV_A");

            //ICO.add_icon(frm_qstr, "F85151", 4, "Welfare Contribution Report", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85135", 3, "Leave Master", 3, "../tej-base/om_lvsetup.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F85138", 3, "Salary Master", 3, "../tej-base/om_inc_ded.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85139", 3, "Prof Tax Master", 3, "../tej-base/om_Pt_Config.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85140", 3, "Employer PF Master", 3, "../tej-base/om_Pf_Config.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85143", 3, "Increment Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85144", 3, "Minimum Wage Master", 3, "../tej-base/om_lvsetup.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85145", 3, "Employee Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F85109", 3, "Pay Increment", 3, "../tej-base/om_pay_incr.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85153", 4, "WF Wages Upload", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            /////
            ICO.add_icon(frm_qstr, "F85231", 4, "Anniversary List", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "fin85_MREP", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85235", 3, "Employee Grade Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "ESI_SAL");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD ESI_SAL NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "ESI_AMT_CS");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD ESI_AMT_CS NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "PF_SAL");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD PF_SAL NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "PF_RT_CS");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD PF_RT_CS NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "PF_AMT_CS");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD PF_AMT_CS NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "PF_RT_ES");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD PF_RT_ES NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "WF_SAL");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD WF_SAL NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "WF_RT_CS");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD WF_RT_CS NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "WF_AMT_CS");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD WF_AMT_CS NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "WF_RT_ES");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD WF_RT_ES NUMBER(15,5) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "MASTVCH");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD MASTVCH VARCHAR2(16) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "AGE");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD AGE NUMBER(5,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "SELVCH");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD SELVCH VARCHAR2(16) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "MATERNITY");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD MATERNITY VARCHAR2(10) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "dt1");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD dt1 number(5,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "dt2");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD dt2 number(5,2) DEFAULT 0");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "ATTN", "Tot_ded");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE ATTN ADD Tot_ded number(5,1) DEFAULT 0");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "APP_BY");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "alter table PAYLOAN add APP_BY varchar2(20) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "APP_DT");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD APP_DT DATE DEFAULT SYSDATE");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "INST_ST_DT");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD INST_ST_DT DATE DEFAULT SYSDATE");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "CUR_LOAN");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD CUR_LOAN VARCHAR(5) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "OS_AMT");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD OS_AMT NUMBER(10,2) DEFAULT 0");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYINCR", "EFF_DATE");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYINCR ADD EFF_DATE DATE DEFAULT SYSDATE");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYINCR", "EFF_UPTO");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYINCR ADD EFF_UPTO DATE DEFAULT SYSDATE");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYINCR", "TYPE");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYINCR ADD TYPE VARCHAR(2) DEFAULT '-'");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYINCR", "PAYLINK");
            if (mhd == "0")
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYINCR ADD PAYLINK VARCHAR2(250) DEFAULT '-'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SELMAST'", "TNAME");
            string SQuery = "";
            if (mhd == "0" || mhd == "")
            {
                SQuery = "CREATE TABLE WB_SELMAST(BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,GRADE CHAR(2),ED_FLD CHAR(5),ED_NAME CHAR(10),MORDER NUMBER(2),EFF_FROM DATE,EFF_TO DATE,WEEK_OFF VARCHAR2(4),PF_YN VARCHAR2(2),VPF_YN VARCHAR2(2),ESI_YN VARCHAR2(2),WF_YN VARCHAR2(2),PT_YN VARCHAR2(2),OT_YN VARCHAR2(2),EL_YN VARCHAR2(2),MINHRS VARCHAR2(20),MAXHRS VARCHAR2(20),FST_START VARCHAR2(20),SSFT_START VARCHAR2(20),FST_END VARCHAR2(20),SSFT_END VARCHAR2(20),ENT_BY VARCHAR2(30),ENT_DT DATE,EDT_BY VARCHAR2(30),EDT_DT DATE,OT_DAYS VARCHAR2(30),OT_DIV VARCHAR2(30),DAYN VARCHAR2(20),RATE NUMBER(10,2) DEFAULT '0',ICAT VARCHAR2(1) DEFAULT '-',PF_DIV VARCHAR2(25),ESI_DIV VARCHAR2(25),WF_DIV VARCHAR2(25),EMPR_RATE NUMBER(10,2),MAX_LMT NUMBER(10,2),ERN_DIV VARCHAR2(25))";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_PTAX'", "TNAME");
            if (mhd == "0" || mhd == "")
            {
                SQuery = "CREATE TABLE WB_PTAX (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,GRADE CHAR(2),ED_FLD CHAR(5),ED_NAME CHAR(10),STATEN CHAR(4),EFF_FROM DATE,EFF_TO DATE,SAL_FRM NUMBER(10,2),MORDER NUMBER(2),SAL_UPTO NUMBER(10,2),MTH01 NUMBER(10,2),MTH02 NUMBER(10,2),MTH03 NUMBER(10,2),MTH04 NUMBER(10,2),MTH05 NUMBER(10,2),MTH06 NUMBER(10,2),MTH07 NUMBER(10,2),MTH08 NUMBER(10,2),MTH09 NUMBER(10,2),MTH10 NUMBER(10,2),MTH11 NUMBER(10,2),MTH12 NUMBER(10,2),M_TOT NUMBER(10,2),ICAT VARCHAR2(1),ENT_BY VARCHAR2(30),ENT_DT DATE,EDT_BY VARCHAR2(30),EDT_DT DATE,REMARK VARCHAR2(80),CATEGORY VARCHAR2(30))";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
            ICO.add_icon(frm_qstr, "F85154", 3, "Multi Emp Upload Master", 3, "../tej-base/om_multi_empmas.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "N");
            //            ICO.add_icon(frm_qstr, "F85155", 3, "Designation Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F85156", 3, "F & F", 3, "../tej-base/om_pay_data.aspx", "-", "-", "fin85_e1", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85157", 3, "Employee Pending Confirmation List", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            //------
            ICO.add_icon(frm_qstr, "F85158", 3, "PF Register", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85159", 3, "Member Wise Salary Report", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85160", 3, "PF Reg in CSV Format", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85161", 3, "ESI Return", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85162", 3, "Gratuity Report", 3, "../tej-base/om_prt_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F85163", 3, "Welfare Fund Report", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_SELMAST'", "TNAME");
            if (mhd != "0")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_SELMAST", "OT_DAYS2");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_SELMAST ADD OT_DAYS2 NUMBER(10,4) DEFAULT 0");
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_SELMAST", "OT_DIV2");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_SELMAST ADD OT_DIV2 VARCHAR2(30) DEFAULT '-'");
            }
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "HOURS2");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD HOURS2 NUMBER(6,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "VERO2");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD VERO2 NUMBER(6,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "VERO_ON");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD VERO_ON NUMBER(6,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "VERO_ON2");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD VERO_ON2 NUMBER(6,2) DEFAULT 0");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAY", "BRANCH_ACT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAY ADD BRANCH_ACT VARCHAR2(2) DEFAULT '-'");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "EMPMAS", "BRANCH_ACT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE EMPMAS ADD BRANCH_ACT VARCHAR2(2) DEFAULT '-'");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME = 'WB_EMPMAS_DTL'", "TNAME");
            if (mhd == "0")
            {
                mhd = "create table wb_empmas_dtl (branchcd char(2) default '-',grade char(2) default '-',empcode varchar2(6) default '-',document_type varchar2(30) default '-',doc_no varchar2(30) default '-',issue_dt varchar2(10) default '-',expiry_dt varchar2(10)  default '-',iss_from varchar2(50) default '-',remarks varchar2(100) default '-',filename varchar2(100) default '-',filepath varchar2(250) default '-',ent_by varchar2(20) default '-',ent_dt date default sysdate)";
                fgen.execute_cmd(frm_qstr, frm_cocd, mhd);
            }
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME = 'WB_SELMAST'", "TNAME");
            if (mhd != "0")
            {
                mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_SELMAST", "OT2_YN");
                if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_SELMAST ADD OT2_YN VARCHAR2(2) DEFAULT '-'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_SELMAST MODIFY MINHRS NUMBER(4,2) DEFAULT 0");
                fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_SELMAST MODIFY MAXHRS NUMBER(4,2) DEFAULT 0");
            }
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_SELMAST", "DED_DIV");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_SELMAST ADD DED_DIV VARCHAR2(25) DEFAULT '-'");

            ICO.add_icon(frm_qstr, "F85164", 3, "Holiday Master", 3, "../tej-base/om_hmas.aspx", "-", "-", "fin85_e3", "fin85_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F85165", 3, "Salary TDS Trend", 3, "../tej-base/om_view_pay.aspx", "-", "-", "fin85_e4", "fin85_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Icon_Cust_port(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Customer Portal
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("CPORT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('CPORT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "CPORT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F79000", 2, "Customer Portal", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F79100", 3, "Customer Orders", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F79101", 4, "Status :Sales.Orders(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79106", 4, "Status :Sales.Schedule(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F79121", 3, "Despatch Performance", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F79126", 4, "Order Dt Vs Despatch Dt.(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit", "N", "N");
            //ICO.add_icon(frm_qstr, "F79127", 4, "Sch. Dt Vs Despatch Dt.(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79128", 4, "Customer Dashboard.(Portal)", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79111", 4, "Despatch Trends(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");

            //ICO.add_icon(frm_qstr, "F79131", 2, "Supplier Dues", 3, "-", "-", "Y", "fin79_e3", "fin79_a1", "-", "fa-edit");
            //ICO.add_icon(frm_qstr, "F79132", 3, "Due Bill Status(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin79_e3", "fin79_a1", "-", "fa-edit");
        }
        mhd = fgen.chk_RsysUpd("CPORT102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('CPORT102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "CPORT102", "DEV_A");

            ICO.add_icon(frm_qstr, "F79131", 3, "Portal Reports", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F79132", 4, "Statement of Account", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F79133", 4, "Ageing Statement ", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F79134", 4, "31 Day Sales Qty", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F79135", 4, "Schedule Vs Despatch", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79136", 4, "Order Vs Despatch", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "N");
            //ICO.add_icon(frm_qstr, "F79137", 4, "Bill Wise Shipment", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79138", 4, "Schedule Vs Despatch Daily Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79139", 4, "Schedule Vs Despatch Monthly Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F79140", 3, "Portal Checklists", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");
            //ICO.add_icon(frm_qstr, "F79141", 4, "Master S.O. Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79142", 4, "Supply S.O. Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79143", 4, "Supply Sch. Checklists", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79144", 4, "Schedule Vs Despatch Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F79145", 4, "Pending Order Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit", "N", "N");


            ICO.add_icon(frm_qstr, "F79122", 4, "Despatch Report", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F79123", 4, "Order Vs. Despatch Report", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit", "N", "N");
        }

        fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET PRD='N' WHERE ID='F79106'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET PRD='N' WHERE ID='F79138'");
    }

    public void Icon_Cust_port_new(string frm_qstr, string frm_cocd)
    {
        // FOR GIVING RIGHTS
        string username = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
        string chk_usr = fgen.seek_iname(frm_qstr, frm_cocd, "select username from fin_mrsys where trim(userid)='" + username + "' and trim(id)='F79123' ", "username");
        if (chk_usr == "0" || chk_usr == "")
        //mhd = fgen.chk_RsysUpd("CPORT103");// not making for new id hence commented
        //if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('CPORT103') ");
            ICO.add_iconRights(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79000", 2, "Customer Portal", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79100", 3, "Customer Orders", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79101", 4, "Status :Sales.Orders(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79106", 4, "Status :Sales.Schedule(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F79121", 3, "Despatch Performance", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79126", 4, "P.O. Dt Vs Despatch Dt.(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79127", 4, "Sch. Dt Vs Despatch Dt.(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79128", 4, "Customer Dashboard.(Portal)", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79111", 4, "Despatch Trends(Portal)", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F79131", 3, "Reports", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79132", 4, "Statement of Account", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79133", 4, "Ageing Statement ", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79134", 4, "31 Day Sales Qty", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            //ICO.add_iconRights(frm_qstr, "F79135", 4, "Schedule Vs Despatch", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79136", 4, "Order Vs Despatch", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            //ICO.add_iconRights(frm_qstr, "F79137", 4, "Bill Wise Shipment", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79138", 4, "Schedule Vs Despatch Daily Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79139", 4, "Schedule Vs Despatch Monthly Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e3", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F79140", 3, "Checklists", 3, "-", "-", "Y", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79141", 4, "Master S.O. Checklists", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79142", 4, "Supply S.O. Checklists", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79143", 4, "Supply Sch. Checklists", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79144", 4, "Schedule Vs Despatch Checklists", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79145", 4, "Pending Order Checklists", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e4", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F79122", 4, "Despatch Report", 3, "../tej-base/om_view_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F79123", 4, "Order Vs. Despatch Report", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e2", "fa-edit");
        }

        switch (frm_cocd)
        {
            case "SGRP":
            case "UATS":
            case "UAT2":
                ICO.add_iconRights(frm_qstr, "F79109", 4, "Drawing / Artwork Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit");
                break;
        }
    }

    public void Icon_Supp_port(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Supplier Portal
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("SPORT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('SPORT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "SPORT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F78000", 2, "Supplier Portal", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F78100", 3, "Supplier Orders", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F78100A", 4, "Purch.Orders Print", 3, "../tej-base/om_prt_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F78101", 4, "Status :Purch.Orders(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F78106", 4, "Status :Purch.Schedule(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F78108", 4, "My Shipment (Your MRR)", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F78110", 4, "Goods Pending in QC", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F78112", 4, "Goods with Matl Shortage", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F78143", 4, "Schedule Vs Reciept Daily Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F78144", 4, "Schedule Vs Reciept Monthly Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F78121", 3, "Supplier Performance", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F78126", 4, "P.O. Dt Vs Rcpt Dt.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F78127", 4, "Sch. Dt Vs Rcpt Dt.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F78128", 4, "Rcpt Vs Accpt Qty.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F78131", 3, "Supplier Dues", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            //ICO.add_icon(frm_qstr, "F78132", 4, "Supplier Bill Status(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F78133", 4, "Supplier Dashboard(Portal)", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F78139", 4, "Statement of Account", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F78140", 4, "Ageing Statement ", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F78135", 4, "Supply P.O. Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F78136", 4, "Supply Sch. Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F78137", 4, "Order Vs Reciept Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F78138", 4, "Pending Order Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F78141", 4, "My Debit Note", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F78142", 4, "My Credit Note", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "Y");
        }
        ICO.add_icon(frm_qstr, "F78145", 4, "My Payment Advice", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit", "N", "N");

        ICO.add_icon(frm_qstr, "F78113", 4, "ASN Entry", 3, "../tej-base/om_ASN_PO.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
    }

    public void Icon_Supp_port_new(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Supplier Portal
        // ------------------------------------------------------------------
        // FOR GIVING RIGHTS
        string username = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
        string chk_usr = fgen.seek_iname(frm_qstr, frm_cocd, "select username from fin_mrsys where trim(userid)='" + username + "' and trim(id)='F78138' ", "username");
        if (chk_usr == "0")
        {
            ICO.add_iconRights(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78000", 2, "Supplier Portal", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78100", 3, "Supplier Orders", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78100A", 4, "Purch.Orders Print", 3, "../tej-base/om_prt_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78101", 4, "Status :Purch.Orders(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78106", 4, "Status :Purch.Schedule(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78108", 4, "My Shipment (Your MRR)", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78110", 4, "Goods Pending in QC", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78112", 4, "Goods with Matl Shortage", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F78121", 3, "Supplier Performance", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78126", 4, "P.O. Dt Vs Rcpt Dt.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78127", 4, "Sch. Dt Vs Rcpt Dt.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78128", 4, "Rcpt Vs Accpt Qty.(Portal)", 3, "../tej-base/om_prt_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F78131", 3, "Supplier Dues", 3, "-", "-", "Y", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78132", 4, "Supplier Bill Status(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78133", 4, "Supplier Dashboard(Portal)", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78139", 4, "Statement of Account", 3, "../tej-base/om_prt_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78140", 4, "Ageing Statement ", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78143", 4, "Schedule Vs Reciept Daily Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78144", 4, "Schedule Vs Reciept Monthly Trend", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F78135", 4, "Supply P.O. Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78136", 4, "Supply Sch. Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78137", 4, "Order Vs Reciept Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78138", 4, "Pending Order Checklists", 3, "../tej-base/om_prt_cport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e2", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F78141", 4, "My Debit Note", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F78142", 4, "My Credit Note", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
        }

        ICO.add_iconRights(frm_qstr, "F78113", 4, "ASN Entry", 3, "../tej-base/om_ASN_PO.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e1", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F78145", 4, "My Payment Advice", 3, "../tej-base/om_view_sport.aspx", "-", "-", "fin78_e1", "fin15_a1", "fin78pp_e3", "fa-edit");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78128'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET WEB_aCTION='../tej-base/om_prt_sport.aspx' WHERE ID='F78139'");

        fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='Y',BRN='N',web_Action='../tej-base/om_view_sport.aspx' where id='F78136'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_view_sport.aspx' where id='F78126'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='Y',BRN='N',web_Action='../tej-base/om_view_sport.aspx' where id='F78135'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update fin_msys set web_Action='../tej-base/om_view_sport.aspx' ,prd='Y',BRN='N' WHERE ID='F78137'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78128'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "update  fin_msys set prd='N',BRN='N',web_Action='../tej-base/om_prt_sport.aspx'  where id='F78138'");

    }

    public void Icon_Taskmgt(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // ERP Task Management
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("TASK101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('TASK101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "TASK101", "DEV_A");

            ICO.add_icon(frm_qstr, "F90000", 1, "Task Management", 3, "-", "-", "Y", "-", "fin90_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F90100", 2, "Task Activity", 3, "-", "-", "Y", "fin90_e1", "fin90_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F90101", 3, "Task Assign Entry", 3, "../tej-base/om_task_log.aspx", "-", "-", "fin90_e1", "fin90_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F90106", 3, "Task Action Entry", 3, "../tej-base/om_task_act.aspx", "-", "-", "fin90_e1", "fin90_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F90116", 2, "Tasks Reports", 3, "-", "-", "Y", "fin90_e2", "fin90_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F90121", 3, "Task Assigment List", 3, "../tej-base/om_view_task.aspx", "-", "-", "fin90_e2", "fin90_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F90126", 3, "Task Activity List", 3, "../tej-base/om_view_task.aspx", "-", "-", "fin90_e2", "fin90_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F90131", 3, "Assigment Vs Action", 3, "../tej-base/om_view_task.aspx", "-", "-", "fin90_e2", "fin90_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F90136", 3, "Pending Tasks List", 3, "../tej-base/om_view_task.aspx", "-", "-", "fin90_e2", "fin90_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F90140", 2, "Task Dashboards", 3, "-", "-", "Y", "fin90_e3", "fin90_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F90141", 3, "Task Mgmt Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin90_e3", "fin90_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F90142", 3, "Task Mgmt Status", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin90_e3", "fin90_a1", "-", "fa-edit");
        }
        ICO.add_icon(frm_qstr, "F90109", 3, "Task Completion Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin90_e1", "fin90_a1", "-", "fa-edit");
    }

    public void Icon_TaskmgtWP(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // ERP Task Management
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("TASK102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('TASK102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "TASK102", "DEV_A");

            ICO.add_icon(frm_qstr, "W90000", 1, "Task Monitoring", 3, "-", "-", "Y", "-", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90100", 2, "Task Activity", 3, "-", "-", "Y", "fin90_p1", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90101", 3, "Task Assign Entry", 3, "../tej-base/dak.aspx", "-", "-", "fin90_p1", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90109", 3, "Task Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin90_p1", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90106", 3, "Task Action Entry", 3, "../tej-base/dak1_task.aspx", "-", "-", "fin90_p1", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90108", 3, "Task Action Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin90_p1", "fin90_w1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "W90116", 2, "Tasks Reports", 3, "-", "-", "Y", "fin90_p2", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90121", 3, "Task Status Report", 3, "../tej-base/rpt.aspx", "-", "-", "fin90_p2", "fin90_w1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "W90140", 2, "Task Dashboards", 3, "-", "-", "Y", "fin90_p3", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90141", 3, "Task Mgmt Dashboard", 3, "../tej-base/wppldashbord.aspx", "-", "-", "fin90_p3", "fin90_w1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "W90240", 2, "Masters", 3, "-", "-", "Y", "fin90_m3", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90241", 3, "Dept Master", 3, "../tej-base/dpt_mst.aspx", "-", "-", "fin90_m3", "fin90_w1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "W90242", 3, "Person Master", 3, "../tej-base/prsn_mst.aspx", "-", "-", "fin90_m3", "fin90_w1", "-", "fa-edit");
        }
    }

    public void Icon_Maint(string frm_qstr, string frm_cocd)
    {

        //--------------------------------
        //Maint System
        //--------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("MNT101");
        if (mhd == "0" || mhd == "")
        {


            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MNT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "MNT101", "DEV_A");

            ICO.add_icon(frm_qstr, "F75000", 1, "Maintenance Module", 3, "-", "-", "Y", "fin75_e1", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75100", 2, "Maint. Activity", 3, "-", "-", "Y", "fin75_e1", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75101", 3, "Maint. Planning", 3, "../tej-base/om_maint_mchplan.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75106", 3, "Maint. Planned Action", 3, "../tej-base/om_maint_done.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75111", 3, "Maint. Complaint Action", 3, "../tej-base/om_comp_Act.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F75121", 2, "Maintenance Logs", 3, "-", "-", "Y", "fin75_e2", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75126", 3, "Planned Maint. Logs", 3, "../tej-base/om_view_maint.aspx", "-", "-", "fin75_e2", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75127", 3, "Complaint Maint. Logs", 3, "../tej-base/om_view_maint.aspx", "-", "-", "fin75_e2", "fin75_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F75140", 2, "Maintenance Reports", 3, "-", "-", "Y", "fin75_e3", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75141", 3, "Section Wise B/Down Report", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75142", 3, "Depart. Wise B/Down Report", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75143", 3, "Machine Wise B/Down Report", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75144", 3, "Reason Wise B/Down Report", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F75171", 2, "Maintenance Analysis", 3, "-", "-", "Y", "fin75_e4", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75172", 3, "Maintenance Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin75_e4", "fin75_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F75161", 2, "Maintenance Masters", 3, "-", "-", "Y", "fin75_e5", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75162", 3, "Maintenance Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin75_e5", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75165", 3, "Machines Master", 3, "../tej-base/om_maint_mach.aspx", "-", "-", "fin75_e5", "fin75_a1", "-", "fa-edit", "N", "Y");
        }

        // VV 20/04/2020
        ICO.add_icon(frm_qstr, "F75113", 3, "Maint. Complaint", 3, "../tej-base/om_frmPmcard.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");

        // VV 22/04/2020
        mhd = fgen.chk_RsysUpd("UPV101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('UPV101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "UPV101", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET WEB_aCTION='../tej-base/om_view_maint.aspx' WHERE ID='F75141'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET WEB_aCTION='../tej-base/om_view_maint.aspx' WHERE ID='F75142'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET WEB_aCTION='../tej-base/om_view_maint.aspx' WHERE ID='F75143'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE FIN_MSYS SET WEB_aCTION='../tej-base/om_view_maint.aspx' WHERE ID='F75144'");
        }

        mhd = fgen.chk_RsysUpd("MNT102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MNT102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "MNT102", "DEV_A");

            ICO.add_icon(frm_qstr, "F75145", 3, "Maintenance Plan Report", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75146", 3, "Maintenance Done Report", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75147", 3, "Maintenance Planned vs. Done", 3, "../tej-base/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
        }

    }


    public void Icon_SysConfig(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // System Admin Options
        // ------------------------------------------------------------------
        string mhd = "";
        mhd = fgen.chk_RsysUpd("SCON101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('SCON101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "SCON101", "DEV_A");

            ICO.add_icon(frm_qstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99101", 3, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99106", 3, "UDFs Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F99110", 3, "Dbd Config(TV)", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99111", 3, "Reps Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99112", 3, "Modules Dbd Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99114", 3, "Plant Level Config", 3, "../tej-base/om_opt_mst_pw.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99115", 3, "Features Config", 3, "../tej-base/om_mnu_opts.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F99116", 3, "Dbd Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99117", 3, "ERP System Config", 3, "../tej-base/om_opt_mst.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99118", 3, "View Log File", 3, "../tej-base/logView.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99119", 3, "Mails Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99120", 3, "Notification Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99122", 3, "Desktop Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F99121", 2, "System Reports", 3, "-", "-", "Y", "fin99_e2", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99126", 3, "New Items Opened", 3, "../tej-base/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99127", 3, "New A/cs Opened", 3, "../tej-base/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99128", 3, "Item Master Edited", 3, "../tej-base/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99129", 3, "A/c Master Edited", 3, "../tej-base/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99130", 3, "More Reports(System)", 3, "-", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F99231", 4, "Data Entry Stats (Purchase)", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99232", 4, "Data Entry Stats (Stores)", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99233", 4, "Data Entry Stats (Sales)", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99234", 4, "Data Entry Stats (Accounts)", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99235", 4, "Data Entry Stats (Production)", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99241", 4, "Who Did What", 3, "../tej-base/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99242", 4, "Similar Name Accounts", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99243", 4, "Similar Name Items", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F99140", 2, "System Tracking", 3, "-", "-", "Y", "fin99_e3", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99141", 3, "ERP Sessions", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99142", 3, "ERP Tracking", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99143", 3, "Locate Options", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F99150", 2, "Corporate Masters", 3, "-", "-", "Y", "fin99_e4", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99151", 3, "Branch/Unit Master", 3, "../tej-base/om_br_mst.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");

            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='Dbd Config(TV)' where id='F99110'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "update FIN_MSYS set text='ERP System Config' where id='F99117'");

            ICO.add_icon(frm_qstr, "F99160", 2, "Users & Rights Management", 3, "-", "-", "Y", "fin99_e5", "fin99_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F99161", 3, "Users Master", 3, "../tej-base/frmUmst.aspx", "-", "-", "fin99_e5", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99162", 3, "Users Rights", 3, "../tej-base/urights.aspx", "-", "-", "fin99_e5", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99163", 3, "Users List", 3, "../tej-base/om_view_sys.aspx", "-", "-", "fin99_e5", "fin99_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F99164", 3, "Desktop Rights Master", 3, "../tej-base/om_appr.aspx", "-", "-", "fin99_e5", "fin99_a1", "-", "fa-edit", "N", "Y");
        }
        ICO.add_icon(frm_qstr, "F99124", 3, "Report Mailing (Automail)", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
    }

    public void Upd_SYSOPT(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("WOPT101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('WOPT101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "WOPT101", "DEV_A");

            fgen.execute_cmd(frm_qstr, frm_cocd, "alter table fin_rsys_opt modify opt_text varchar2(200) default '-'");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0000", "01:Mldg/02:ShMetal/03:Casting/04:Forging/05:Prt/06:Corr/07:Paint/08:Pharma/09:Food/10:Capg./11:Rubber", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "24/12/2017", "DEV_A", "W0001", "Reel Grid in MRR/CHL/ISS", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "24/12/2017", "DEV_A", "W0002", "Bar Code Read Option in MRR/CHL/ISS", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "26/12/2017", "DEV_A", "W0003", "Job No. Reqd in Issue System", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "14/01/2017", "DEV_A", "W0004", "OMS Based on Finance Data", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "14/01/2017", "DEV_A", "W0005", "Line No. Based PR Vs PO", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "14/01/2017", "DEV_A", "W0006", "Line No. Based PO Vs Gate Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "14/01/2017", "DEV_A", "W0007", "Line No. Based PO Vs MRR Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "14/01/2017", "DEV_A", "W0008", "Line No. Based SO Vs INV Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "21/01/2017", "DEV_A", "W0009", "Show 9 Series Items in P.R. Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "21/01/2017", "DEV_A", "W0010", "Show 9 Series Items in Issue Entry", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0011", "Print QR Code on Gate Inw", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0012", "Print QR Code on Purch Ord", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0013", "Print QR Code on Purch sch", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0014", "Print QR Code on M.R.R.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0015", "Print QR Code on RGP/Chl", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0016", "Print QR Code on MRR.Tag", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0017", "Print QR Code on Prof.Inv", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0018", "Print QR Code on Sal.Order", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0019", "Print QR Code on Sale.Inv", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0020", "Print QR Code on Exp.PI", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0021", "Print QR Code on Exp.Ord", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0022", "Print QR Code on Exp.Inv", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0023", "Print QR Code on Payment Adv", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0024", "Print QR Code on Bal.Conf.Let", "N", "2");


            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0030", "OTP Mail Option during Login", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0031", "OTP SMS Option during Login", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/02/2018", "DEV_A", "W0032", "Request Based Issue System", "Y", "1");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0033", "Allow Item Repeat in BOM Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0034", "Allow Item Repeat in P.R Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0035", "Allow Item Repeat in P.O Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0036", "Allow Item Repeat in S.O Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0037", "Allow Item Repeat in Inv Entry", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0038", "Allow Item Repeat in Std Prod.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "25/03/2018", "DEV_A", "W0040", "Allow Item Repeat in D.A Entry", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0041", "Welfare Fund(HR) Applicable", "Y", "1");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0042", "PR Check Before PR Approval Reqd", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0043", "PO Check Before PO Approval Reqd", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0044", "DA Reqd For Sales Invoice", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0045", "Batch Selection Reqd in D.A.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0046", "Batch Selection Reqd in Inv.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W0047", "SO Check Before SO Approval Reqd", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "19/01/2019", "DEV_A", "W0053", "Purchase Order No running for all types? ", "Y", "-"); // vipin
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0054", "Cust/Vend OTP Mail Option during Login", "Y", "1");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0055", "Cust/Vend OTP SMS Option during Login", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0056", "No. of Invoice Copy? ", "N", "4"); // vipin
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0057", "Full Name in DSC Printout? ", "N", "-"); // vipin

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1001", "Rolling Freeze Days BOM", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1002", "Rolling Freeze Days Proc.Plan", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1003", "Rolling Freeze Days Stage Mapping", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1011", "Rolling Freeze Days P.R.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1012", "Rolling Freeze Days P.O.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1013", "Rolling Freeze Days P:Sch.", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1021", "Rolling Freeze Days G.Ent", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1022", "Rolling Freeze Days G.Out", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1031", "Rolling Freeze Days MRR", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1032", "Rolling Freeze Days CHL", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1033", "Rolling Freeze Days ISS", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1034", "Rolling Freeze Days RETU", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1041", "Rolling Freeze Days Q.A.(Basic)", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1042", "Rolling Freeze Days Q.A.(Templ)", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1043", "Rolling Freeze Days Q.A.(Report)", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1051", "Rolling Freeze Days Std.Prodn", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1052", "Rolling Freeze Days Adv.Prodn", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1053", "Rolling Freeze Days Stg.Tranfer", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1061", "Rolling Freeze Days P.I.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1062", "Rolling Freeze Days Mst.S.O.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1063", "Rolling Freeze Days Supply.S.O.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1064", "Rolling Freeze Days Invoice", "N", "2");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1071", "Rolling Freeze Days Rcpts", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1072", "Rolling Freeze Days Pymts", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1073", "Rolling Freeze Days J.V.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1074", "Rolling Freeze Days P.V.", "N", "2");


            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2001", "Effective Date for Accounts", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2002", "Effective Date for Gate ", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2003", "Effective Date for Stores", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2004", "Effective Date for Reel Wise Stock", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2005", "Effective Date for Lot Wise Stock", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2006", "Effective Date for P.P.C.", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2007", "Effective Date for Production", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2015", "Currency For Branch", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2016", "Comma Seprator[I]nd / [U]sa", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W2017", "Indian GST Applicable", "Y", "1");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "11/03/2018", "DEV_A", "W2018", "Allow All Items in Sales Order", "N", "2");
        }

        mhd = fgen.chk_RsysUpd("WOPT102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('WOPT102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "WOPT102", "DEV_A");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W0048", "Request Based Challan System", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "15/08/2018", "DEV_B", "W1075", "% Residual Value-Depreciation", "Y", "1");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "25/08/2018", "DEV_B", "W1076", "Start date for FA module-C.Act dep", "Y", "1");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "25/08/2018", "DEV_B", "W1077", "Start date for FA module-IT Block", "Y", "1");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "09/11/2018", "DEV_A", "W2019", "Batch Wise Invoice Creation", "N", "2");
        }
        mhd = fgen.chk_RsysUpd("WOPT103");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('WOPT103') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "WOPT103", "DEV_A");

            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0054", "Cust/Vend OTP Mail Option during Login", "Y", "1");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "07/02/2018", "DEV_A", "W0055", "Cust/Vend OTP SMS Option during Login", "N", "2");
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0058", "Salary Calculations in centralised Branch? ", "N", "-"); // MG
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0059", "OT in Reg? ", "N", "-"); // MG
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0060", "Round Off Salaries? ", "N", "-"); // MG
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0061", "Round Off Gross Salary? ", "N", "-"); // MG
            fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "13/09/2019", "DEV_A", "W0062", "Visitor Gate Entry(EMPMAS-1,EVAS-2)? ", "N", "-"); // MG
        }
        fgen.save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "20/07/2020", "DEV_A", "W1000", "01:Mldg/02:ShMetal/03:Casting/04:Forging/05:Prt/06:Corr/07:Paint/08:Pharma/09:Food/10:Capg./11:Rubber", "N", "2");
    }

    public void Icon_Prodrx(string frm_qstr, string frm_cocd)
    {
        //--------------------------------
        //Production Rx
        //--------------------------------
        ICO.add_icon(frm_qstr, "F38000", 2, "Coating Production", 3, "-", "-", "Y", "fin38_e1", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F38500", 3, "Barcode_Generation", 3, "-", "-", "Y", "fin38_e1", "fin40_a1", "fin38pp1_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F38501", 4, "Mixing Barcodes", 3, "../tej-base/om_prt_prodrx.aspx", "-", "-", "fin38_e1", "fin40_a1", "fin38pp1_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F38502", 4, "Production Printing Store", 3, "../tej-base/om_view_prodrx.aspx", "-", "-", "fin38_e1", "fin40_a1", "fin38pp1_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F38503", 4, "Production Pigment Store", 3, "../tej-base/om_view_prodrx.aspx", "-", "-", "fin38_e1", "fin40_a1", "fin38pp1_e1", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F38504", 4, "Production Main Mixing Store", 3, "../tej-base/om_view_prodrx.aspx", "-", "-", "fin38_e1", "fin40_a1", "fin38pp1_e1", "fa-edit", "N", "Y");

        ICO.add_icon(frm_qstr, "F38050", 2, "Web Reports (All Reports)", 3, "../tej-base/om_Web_Rpt_KLAS.aspx", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
    }

    public void ProfitabilityReport(string frm_qstr, string frm_cocd)
    {
        ///Profitability Report        
        ICO.add_icon(frm_qstr, "F05000", 1, "Management MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F05100", 2, "Sales MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F05124", 3, "Profitability Report (BOM based)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F05125a", 3, "RMC Cost Report (BOM based) Expandable", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F05124J", 3, "Profitability Report (Job Card Wise)", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
    }

    public void Desk_Tiles(string frm_qstr, string frm_cocd, string branchCode)
    {
        //--------------------------------
        //desktop tiles
        //--------------------------------
        string dsrno = "";
        string mhd = "";


        string home_curr = "Rs";
        string home_divider = "100000";
        string home_div_iden = "Lakh";
        string numbr_fmt = "999,999,999.99";
        string numbr_fmt2 = "999,999,999";

        mhd = fgen.chk_RsysUpd("TILE101" + branchCode);
        if (mhd == "0" || mhd == "")
        {
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP") == "SG_TYPE" || frm_cocd == "HPPI")
            {
                // checking for sg group only, we need to add 2 more fields in branch master
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BR_CURREN||'~'||'1000'||'~'||'000'||'~'||NUM_FMT1||'~'||NUM_FMT2 AS FSTR FROM TYPE WHERE ID='B' AND TYPE1='" + branchCode + "' ", "FSTR");
                if (mhd != "0")
                {
                    home_curr = (mhd.Split('~')[0] == "" || mhd.Split('~')[0] == "-" || mhd.Split('~')[0] == "0") ? home_curr : mhd.Split('~')[0];
                    home_divider = (mhd.Split('~')[1] == "" || mhd.Split('~')[1] == "-" || mhd.Split('~')[1] == "0") ? home_divider : mhd.Split('~')[1];
                    home_div_iden = (mhd.Split('~')[2] == "" || mhd.Split('~')[2] == "-" || mhd.Split('~')[2] == "0") ? home_div_iden : mhd.Split('~')[2];
                    numbr_fmt = (mhd.Split('~')[3] == "" || mhd.Split('~')[3] == "-" || mhd.Split('~')[3] == "0") ? numbr_fmt : mhd.Split('~')[3];
                    numbr_fmt2 = (mhd.Split('~')[4] == "" || mhd.Split('~')[4] == "-" || mhd.Split('~')[4] == "0") ? numbr_fmt2 : mhd.Split('~')[4];
                }
            }

            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('TILE101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "TILE101" + branchCode, "DEV_A");

            dsrno = "11";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sal.order", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Sales Order: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT distinct a.Ordno as doc_no,a.ent_dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(a.total,'99,99,99,999') as doc_Value,a.orddt from somas a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by a.orddt desc,a.Ordno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sal.order", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Ordno as doc_no,to_char(a.Orddt,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_Char(a.total,'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.orddt,'yyyymmdd') as VDD from somas a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.ordno desc) where rownum<50");

            dsrno = "12";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sale.Inv", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Sale Invoice: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(a.bill_tot,'99,99,99,999') as doc_Value,a.vchdate from sale a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) order by a.vchdate desc ,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sale.Inv", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(a.bill_tot,'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from Sale a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc ,a.vchnum desc) where rownum<50");

            dsrno = "13";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Order", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "`  as fstr,`Last Purch Order: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT distinct a.Ordno as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(a.rate_cd,'99,99,99,999') as doc_Value,a.orddt from Pomas a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `5%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by a.orddt desc,a.ordno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Order", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Ordno as doc_no,to_char(a.Orddt,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(a.rate_cd,'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.orddt,'yyyymmdd') as VDD  from Pomas a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `5%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.ordno desc) where rownum<50");

            dsrno = "14";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Ind", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Purch Ind.: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Request By : ` ||(doc_value) as Mdata4  from (SELECT distinct a.Ordno as doc_no,a.ent_Dt as Doc_Dt,substr(a.Bank,1,10) as Doc_party,a.ent_by as doc_Value,a.orddt from Pomas a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `60%` AND a.orddt DT_RANGE  order by a.orddt desc,a.ordno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Ind", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Ordno as doc_no,to_char(a.Orddt,'dd/mm/yyyy') as Doc_Dt,a.Bank as Deptt_Name,a.ent_by,a.app_by,to_char(a.orddt,'yyyymmdd') as VDD  from Pomas a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `60%` AND a.orddt DT_RANGE order by vdd desc,a.ordno desc) where rownum<50");

            dsrno = "15";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Gate.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Gate Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucherp a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `00%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Gate.Entry", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucherp a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `00%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.Vchnum,a.ent_by,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "16";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "MRR.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last MRR Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.type!='04' and a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "MRR.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.type!='04' and a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "17";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Sch", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Purch Sch: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from Schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `66%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Sch", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') as VDD from schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `66%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "18";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Gate.WPO", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Inw(NonPO): ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucherp a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `00%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) and upper(trim(a.prnum))='OT' group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Gate.WPO", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucherp a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `00%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) and upper(trim(a.prnum))='OT' group by a.Vchnum,a.ent_by,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "19";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "MRR.Imp", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Import: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.type='07' and a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "MRR.Imp", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.type='07' and a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "30";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "CHL.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Chl Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.iqtyout*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `2%` AND a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "CHL.Entry", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.iqtyout*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `2%` AND a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "31";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Iss.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Issue Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.name,1,10) as Doc_party,to_char(sum(a.iqtyout*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `3%` AND a.type!='36' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.type1) and b.id='M' group by a.ent_Dt,a.vchnum,a.vchdate,b.name order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Iss.Entry", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.name as Deptt_Name,to_char(sum(a.iqtyout*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `3%` AND a.type!='36' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.type1) and b.id='M' group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.name,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "32";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ret.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Store Retu: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.name,1,10) as Doc_party,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` AND a.type<'15' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.type1) and b.id='M' group by a.ent_Dt,a.vchnum,a.vchdate,b.name order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ret.Entry", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.name as Deptt_Name,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` AND a.type<'15' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.type1) and b.id='M' group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.name,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "33";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "BOM.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last BOM Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Product : ` ||(Doc_Party) as Mdata3,`Made By : ` ||(doc_value) as Mdata4  from (SELECT distinct a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.iname,1,10) as Doc_party,a.ent_by as doc_Value,a.vchdate from itemosp a,item b where trim(A.icode)=trim(B.icode) and a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `BM%` and a.vchdate DT_RANGE order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "BOM.Entry", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.iname as Product_Name,a.ent_by as Entry_By,to_char(a.Vchdate,'yyyymmdd') as VDD from itemosp a,item b where trim(A.icode)=trim(B.icode) and a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `BM%` and a.vchdate DT_RANGE order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "34";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prod.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Prodn Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.name,1,10) as Doc_party,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` AND a.type>='15' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.type1) and b.id='M' group by a.ent_Dt,a.vchnum,a.vchdate,b.name order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prod.Entry", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.name as Deptt_Name,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD,A.ACODE from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` AND a.type>='15' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.type1) and b.id='M' group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.name,a.ent_by,to_char(a.Vchdate,'yyyymmdd'),A.ACODE order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "20";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sale_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_sale/" + home_divider + "),2) AS QTYOUT,`Invoices(" + home_curr + " " + home_div_iden + ")` as col3,`column` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM sale A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `4%` and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sale_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_Sale/" + home_divider + "),2) AS basic_val,ROUND(SUM((amt_exc+rvalue)/" + home_divider + "),2) AS GST_Val,ROUND(SUM(bill_tot/" + home_divider + "),2) AS Total_val FROM sale A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `4%` and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");

            dsrno = "21";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Inw_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_sale/" + home_divider + "),2) AS QTYOUT,`Material MRR Value (" + home_curr + " " + home_div_iden + ")` as col3,`bar` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` as Col5 FROM ivchctrl A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `0%` and type!='04' and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Inw_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_Sale/" + home_divider + "),2) AS basic_val,ROUND(SUM((amt_exc+rvalue)/" + home_divider + "),2) AS GST_Val,ROUND(SUM(bill_tot/" + home_divider + "),2) AS Total_val FROM ivchctrl A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `0%` and type!='04' and  vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");

            dsrno = "22";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Rejn_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_sale/" + home_divider + "),2) AS QTYOUT,`Sales Return (" + home_curr + " " + home_div_iden + ") ` as col3,`spline` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` as Col5 FROM ivchctrl A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `04%` and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Rejn_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_Sale/" + home_divider + "),2) AS basic_val,ROUND(SUM((amt_exc+rvalue)/" + home_divider + "),2) AS GST_Val,ROUND(SUM(bill_tot/" + home_divider + "),2) AS Total_val FROM ivchctrl A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `04%` and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");

            dsrno = "23";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Exp_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_sale/" + home_divider + "),2) AS QTYOUT,`Export Invoices (" + home_curr + " " + home_div_iden + ")` as col3,`line` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` as Col5 FROM sale A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `4F%` and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Exp_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(VCHDATE,`Mon`) AS YR,ROUND(SUM(amt_Sale/" + home_divider + "),2) AS basic_val,ROUND(SUM((amt_exc+rvalue)/" + home_divider + "),2) AS GST_Val,ROUND(SUM(bill_tot/" + home_divider + "),2) AS Total_val FROM sale A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `4F%` and vchdate DT_RANGE GROUP BY TO_cHAR(VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(vchdate,`YYYYMM`)");


            dsrno = "35";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Qa.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last QA Done: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`QA By : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.qc_date as Doc_Dt,substr(b.aname,1,10) as Doc_party,a.pname as doc_Value,a.vchdate from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.vchdate DT_RANGE and a.store in ('Y','N') and trim(A.acode)=trim(B.acode) AND a.inspected='Y' and a.pname!='-' order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Qa.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,c.Iname,to_char(sum(a.iqtyin),'99,99,99,999') as Inw_Qty,c.unit,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as Inw_Value,a.ent_by,a.Pname as Insp_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,famst b,item c  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.vchdate DT_RANGE and a.store in ('Y') and trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,c.iname,c.unit,a.pname,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "36";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Rejn.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Rejn Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Rej By : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.qc_date as Doc_Dt,substr(b.aname,1,10) as Doc_party,a.pname as doc_Value,a.vchdate  from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.vchdate DT_RANGE and a.store in ('Y') and a.rej_rw>0 and trim(A.acode)=trim(B.acode) AND a.inspected='Y' and a.pname!='-' order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Rejn.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,c.Iname,to_char(sum(a.rej_rw),'99,99,99,999') as Rej_Qty,c.unit,to_char(sum(a.rej_rw*nvl(a.irate,0)),'99,99,99,999') as Rej_Value,a.ent_by,a.Pname as Insp_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,famst b,item c  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.vchdate DT_RANGE and a.store in ('Y') and a.rej_rw>0 and trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,c.iname,c.unit,a.pname,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "37";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "CustRet.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Sales Rejn: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate  from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.type='04' and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode)  and a.iqty_chl>0 group by a.ent_Dt,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "CustRet.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.iqty_chl*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from ivoucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `0%` AND a.type='04' and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and a.iqty_chl>0 group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "38";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Colln.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Collection: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Date as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(abs(sum(a.cramt)-sum(a.dramt)),'99,99,99,999') as doc_Value,a.vchdate  from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2)='16' group by a.ent_Date,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Colln.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(abs(sum(a.cramt)-sum(a.dramt)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2)='16' group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd')   order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "39";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pymt.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Payments: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Date as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(abs(sum(a.dramt)-sum(a.cramt)),'99,99,99,999') as doc_Value,a.vchdate  from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `2%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2) in ('05','06') group by a.ent_Date,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pymt.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(abs(sum(a.dramt)-sum(a.cramt)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `2%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2) in ('05','06') group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd')   order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "40";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "P.v.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Purch.Vch: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Date as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(abs(sum(a.cramt)-sum(a.dramt)),'99,99,99,999') as doc_Value,a.vchdate  from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `5%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2) in ('05','06') group by a.ent_Date,a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "P.v.Entry", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(abs(sum(a.cramt)-sum(a.dramt)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `5%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2) in ('05','06') group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd')   order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "41";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "PI/QUOT", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Quote/PI: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT distinct a.Ordno as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(a.total,'99,99,99,999') as doc_Value,a.orddt from somasq a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by a.orddt desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "PI/QUOT", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Ordno as doc_no,to_char(a.Orddt,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_Char(a.total,'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.orddt,'yyyymmdd') as VDD from somasq a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.ordno desc) where rownum<50");

            dsrno = "42";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sale.Sch", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Sales Sch: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.vchdate,to_char(a.vchdate,'yyymmdd') as vdd from Schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `46%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.ent_dt,a.vchnum,a.vchdate,b.aname order by vdd desc ,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sale.Sch", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') as VDD from schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `46%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname order by vdd desc ,a.vchnum desc) where rownum<50");

            dsrno = "43";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Sch", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Purch Sch: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.ent_Dt as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value from Schedule,a.vchdate a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `66%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.vchnum,a.ent_Dt,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Sch", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') as VDD from schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `66%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");


        }
        mhd = fgen.chk_RsysUpd("TILE102");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('TILE102') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "TILE102", "DEV_A");

            dsrno = "51";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Iss.Req", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Issue Request: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Request by : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.name,1,10) as Doc_party,a.ent_by as doc_Value from wb_iss_req a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `3%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.type1) and b.id='M' group by a.vchnum,a.vchdate,b.name,a.ent_by order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Iss.Req", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.name as Deptt_Name,c.Iname,a.req_Qty,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from wb_iss_req a,type b,item c  where trim(A.icode)=trim(c.icode) and a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `3%` AND a.type!='36' and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.type1) and b.id='M' order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "52";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Retn.Req", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Return Request: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Request by : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.name,1,10) as Doc_party,a.ent_by as doc_Value from wb_iss_req a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.type1) and b.id='M' group by a.vchnum,a.vchdate,b.name,a.ent_by order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Retn.Req", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.name as Deptt_Name,c.Iname,a.req_Qty,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from wb_iss_req a,type b,item c  where trim(A.icode)=trim(c.icode) and a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.type1) and b.id='M'  order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "53";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "App.Venl", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last APL Upd: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Total Items : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.aname,1,10) as Doc_party,count(*) as doc_Value from appvendvch a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `10%` and a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "App.Venl", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Approved_Vendor,c.Iname,a.Irate,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD,A.ACODE from appvendvch a,famst b,item c  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `10%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "54";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Usr.Mgt", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last User Made: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Level Given : ` ||(doc_value) as Mdata4  from (SELECT a.userid as doc_no,a.ent_Dt as Doc_Dt,Username as Doc_party,ulevel as doc_Value from evas a where a.BRANCHCD=`BR_VAR` order by a.ent_Dt desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Usr.Mgt", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.userid as User_ID,to_char(a.ent_Dt,'dd/mm/yyyy') as Made_om,a.Username ,a.Deptt as Department,a.ent_by,to_char(a.ent_Dt,'yyyymmdd') as VDD from evas a where a.BRANCHCD=`BR_VAR` order by vdd desc,a.userid desc) where rownum<50");

            dsrno = "55";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadLog", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Log: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Lead Grade : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lrcno as doc_no,a.lrcdt as Doc_Dt,a.ldescr as Doc_party,lgrade as doc_Value from wb_lead_log a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LR%` AND a.lrcdt DT_RANGE order by a.lrcdt desc,a.lrcno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadLog", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lrcno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,ldescr as Party_Name,lgrade as Lead_grade,a.ent_by,a.ent_dt as VDD from wb_lead_log a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LR%` AND a.lrcdt DT_RANGE order by vdd desc,a.lrcno desc) where rownum<50");


            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE17'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE17'");

            //tiles
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE34'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE34'");

            dsrno = "34";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prod.Entry", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Prodn Entry: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Deptt : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.name,1,10) as Doc_party,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as doc_Value from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` AND a.type>='15' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.vcode)=trim(B.type1) and b.id='M' group by a.vchnum,a.vchdate,b.name order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prod.Entry", "POP_TILE" + dsrno, "select * from (SELECT `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.name as Deptt_Name,to_char(sum(a.iqtyin*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD,A.ACODE from ivoucher a,type b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` AND a.type>='15' and a.vchdate DT_RANGE and a.store in ('Y') and trim(A.vcode)=trim(B.type1) and b.id='M' group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.name,a.ent_by,to_char(a.Vchdate,'yyyymmdd'),A.ACODE order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE43'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE43'");

            dsrno = "43";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Sch", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Purch Sch: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Total Value : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.aname,1,10) as Doc_party,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value from Schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `66%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.vchnum,a.vchdate,b.aname order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pur.Sch", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(sum(a.total*nvl(a.irate,0)),'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') as VDD from schedule a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `66%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE44'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE44'");

            dsrno = "44";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Tmp", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Insp.Temp: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Item : ` ||(Doc_Party) as Mdata3,`Made By : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.iname,1,10) as Doc_party,a.ent_by as doc_Value from inspmst a,item b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.icode)=trim(B.icode) order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Tmp", "POP_TILE" + dsrno, "select * from (SELECT  distinct `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.iname as Item_Name,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') as VDD from inspmst a,item b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.icode)=trim(B.icode) order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE45'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE45'");

            dsrno = "45";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Rep", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Insp.Rep: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Item : ` ||(Doc_Party) as Mdata3,`Made By : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.iname,1,10) as Doc_party,a.ent_by as doc_Value from inspvch a,item b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.icode)=trim(B.icode) order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Rep", "POP_TILE" + dsrno, "select * from (SELECT  distinct `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.Aname as Party_Name,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from inspvch a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE56'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE56'");

            dsrno = "56";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Leadact", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Action: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Action : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lacno as doc_no,a.lacdt as Doc_Dt,a.ldescr as Doc_party,substr(oremarks,1,10) as doc_Value from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND a.lacdt DT_RANGE order by a.lacdt desc,a.lacno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Leadact", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lacno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,a.ldescr as Party_Name,substr(oremarks,1,40) as Last_rmk,a.ent_by,a.ent_dt as VDD from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND a.lacdt DT_RANGE order by vdd desc,a.lacno desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE57'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE57'");

            dsrno = "57";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Won: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Action : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lacno as doc_no,a.lacdt as Doc_Dt,a.ldescr as Doc_party,substr(oremarks,1,10) as doc_Value from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_stat)='WON'  order by a.lacdt desc,a.lacno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lacno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,a.ldescr as Party_Name,substr(oremarks,1,40) as Last_rmk,a.ent_by,a.ent_dt as VDD from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_Stat)='WON' order by vdd desc,a.lacno desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE58'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE58'");

            dsrno = "58";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Lost: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Action : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lacno as doc_no,a.lacdt as Doc_Dt,a.ldescr as Doc_party,substr(oremarks,1,10) as doc_Value from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_stat)='LOST'  order by a.lacdt desc,a.lacno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lacno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,a.ldescr as Party_Name,substr(oremarks,1,40) as Last_rmk,a.ent_by,a.ent_dt as VDD from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_Stat)='LOST' order by vdd desc,a.lacno desc) where rownum<50");


            dsrno = "08";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sal.Ach", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Sales MTD : ` ||(VAL1) as Mdata,`Sales Today : ` ||(VAL2) as Mdata2,`Sales Prev Mth : ` ||(VAL3) as Mdata3,`Comparison : ` ||((CASE WHEN VAL3>0 THEN ROUND((VAL1/VAL3)*100,2) ELSE 0 END))||` %` as Mdata4  from (SELECT SUM(NVL(val1,0)) AS VAL1,SUM(NVL(val2,0)) AS VAL2,SUM(NVL(val3,0)) AS VAL3 FROM (SELECT ROUND(sum(amt_sale)/" + home_divider + ",2) as val1,0 as val2,0 as val3 from sale where branchcd=`BR_VAR` and to_char(vchdate,'yyyymm')=to_char(ADD_MONTHS(SYSDATE,0),'yyyymm') and type not in (`45`,`47`) union all SELECT 0 as val1,ROUND(sum(amt_sale)/" + home_divider + ",2) as val2,0 as val3 from sale where branchcd=`BR_VAR` and to_char(vchdate,`yyyymmdd`)=to_char(sysdate,`yyyymmdd`) and type not in (`45`,`47`) union all SELECT 0 as val1,0 as val2,ROUND(sum(amt_sale)/" + home_divider + ",2) as val3 from sale where branchcd=`BR_VAR` and to_char(vchdate,'yyyymm')=to_char(ADD_MONTHS(SYSDATE,-1),`yyyymm`) and type not in (`45`,`47`))) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Sal.Ach", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Ordno as doc_no,to_char(a.Orddt,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_Char(a.total,'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.orddt,'yyyymmdd') as VDD from somas a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.ordno desc) where rownum<50");

            dsrno = "09";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Coll.Ach", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Collection MTD : ` ||(VAL1) as Mdata,`Collection Today : ` ||(VAL2) as Mdata2,`Collection Prev Mth : ` ||(VAL3) as Mdata3,`Comparison : ` ||((CASE WHEN VAL3>0 THEN ROUND((VAL1/VAL3)*100,2) ELSE 0 END))||` %` as Mdata4  from (SELECT SUM(NVL(val1,0)) AS VAL1,SUM(NVL(val2,0)) AS VAL2,SUM(NVL(val3,0)) AS VAL3 FROM (SELECT ROUND(sum(cramt-dramt)/" + home_divider + ",2) as val1,0 as val2,0 as val3 from voucher where branchcd=`BR_VAR` and to_char(vchdate,'yyyymm')=to_char(ADD_MONTHS(SYSDATE,0),'yyyymm') and type like '1%' and substr(acode,1,2)='16' union all SELECT 0 as val1,ROUND(sum(cramt-dramt)/" + home_divider + ",2) as val2,0 as val3 from voucher where branchcd=`BR_VAR` and to_char(vchdate,`yyyymmdd`)=to_char(sysdate,`yyyymmdd`) and type like '1%' and substr(acode,1,2)='16' union all SELECT 0 as val1,0 as val2,ROUND(sum(cramt-dramt)/" + home_divider + ",2) as val3 from voucher where branchcd=`BR_VAR` and to_char(vchdate,'yyyymm')=to_char(ADD_MONTHS(SYSDATE,-1),`yyyymm`) and type like '1%' and substr(acode,1,2)='16')) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Coll.Ach", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(abs(sum(a.cramt)-sum(a.dramt)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `1%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2)='16' group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd')   order by vdd desc,a.vchnum desc) where rownum<50");

            dsrno = "10";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pymt.Ach", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Payments MTD : ` ||(VAL1) as Mdata,`Payments Today : ` ||(VAL2) as Mdata2,`Payments Prev Mth : ` ||(VAL3) as Mdata3,`Comparison : ` ||((CASE WHEN VAL3>0 THEN ROUND((VAL1/VAL3)*100,2) ELSE 0 END))||` %` as Mdata4  from (SELECT SUM(NVL(val1,0)) AS VAL1,SUM(NVL(val2,0)) AS VAL2,SUM(NVL(val3,0)) AS VAL3 FROM (SELECT ROUND(sum(dramt-cramt)/" + home_divider + ",2) as val1,0 as val2,0 as val3 from voucher where branchcd=`BR_VAR` and to_char(vchdate,'yyyymm')=to_char(ADD_MONTHS(SYSDATE,0),'yyyymm') and type like '2%' and substr(acode,1,2) in ('05','06') union all SELECT 0 as val1,ROUND(sum(dramt-cramt)/" + home_divider + ",2) as val2,0 as val3 from voucher where branchcd=`BR_VAR` and to_char(vchdate,`yyyymmdd`)=to_char(sysdate,`yyyymmdd`) and type like '2%' and substr(acode,1,2) in ('05','06') union all SELECT 0 as val1,0 as val2,ROUND(sum(dramt-cramt)/" + home_divider + ",2) as val3 from voucher where branchcd=`BR_VAR` and to_char(vchdate,'yyyymm')=to_char(ADD_MONTHS(SYSDATE,-1),`yyyymm`) and type like '2%' and substr(acode,1,2) in ('05','06'))) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Pymt.Ach", "POP_TILE" + dsrno, "select * from (SELECT  `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_char(abs(sum(a.dramt)-sum(a.cramt)),'99,99,99,999') as doc_Value,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from voucher a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `2%` and a.vchdate DT_RANGE  and trim(A.acode)=trim(B.acode) and substr(a.acode,1,2) in ('05','06') group by a.Vchnum,to_char(a.Vchdate,'dd/mm/yyyy'),b.aname,a.ent_by,to_char(a.Vchdate,'yyyymmdd')   order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE44'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE44'");

            dsrno = "44";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Tmp", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Insp.Temp: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Item : ` ||(Doc_Party) as Mdata3,`Made By : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.iname,1,10) as Doc_party,a.ent_by as doc_Value from inspmst a,item b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.icode)=trim(B.icode) order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Tmp", "POP_TILE" + dsrno, "select * from (SELECT  distinct `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.iname as Item_Name,a.ent_by,a.app_by,to_char(a.Vchdate,'yyyymmdd') as VDD from inspmst a,item b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.icode)=trim(B.icode) order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE45'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE45'");

            dsrno = "45";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Rep", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Insp.Rep: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Item : ` ||(Doc_Party) as Mdata3,`Made By : ` ||(doc_value) as Mdata4  from (SELECT a.vchnum as doc_no,a.vchdate as Doc_Dt,substr(b.iname,1,10) as Doc_party,a.ent_by as doc_Value from inspvch a,item b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.icode)=trim(B.icode) order by a.vchdate desc,a.vchnum desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Ins.Rep", "POP_TILE" + dsrno, "select * from (SELECT  distinct `TILE" + dsrno + "` as fstr,a.Vchnum as doc_no,to_char(a.Vchdate,'dd/mm/yyyy') as Doc_Dt,b.Aname as Party_Name,a.ent_by,to_char(a.Vchdate,'yyyymmdd') as VDD from inspvch a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `20%` AND a.vchdate DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.vchnum desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE56'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE56'");

            dsrno = "56";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Leadact", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Action: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Action : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lacno as doc_no,a.lacdt as Doc_Dt,a.ldescr as Doc_party,substr(oremarks,1,10) as doc_Value from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND a.lacdt DT_RANGE order by a.lacdt desc,a.lacno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Leadact", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lacno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,a.ldescr as Party_Name,substr(oremarks,1,40) as Last_rmk,a.ent_by,a.ent_dt as VDD from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND a.lacdt DT_RANGE order by vdd desc,a.lacno desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE57'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE57'");

            dsrno = "57";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Won: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Action : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lacno as doc_no,a.lacdt as Doc_Dt,a.ldescr as Doc_party,substr(oremarks,1,10) as doc_Value from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_stat)='WON'  order by a.lacdt desc,a.lacno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lacno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,a.ldescr as Party_Name,substr(oremarks,1,40) as Last_rmk,a.ent_by,a.ent_dt as VDD from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_Stat)='WON' order by vdd desc,a.lacno desc) where rownum<50");

            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='TXT_TILE58'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from dsk_Config where obj_name='POP_TILE58'");


            dsrno = "58";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Last Lead:Lost: ` ||(Doc_no) as Mdata,`Made On : ` ||(doc_Dt) as Mdata2,`Party : ` ||(Doc_Party) as Mdata3,`Action : ` ||(doc_value) as Mdata4  from (SELECT distinct a.lacno as doc_no,a.lacdt as Doc_Dt,a.ldescr as Doc_party,substr(oremarks,1,10) as doc_Value from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_stat)='LOST'  order by a.lacdt desc,a.lacno desc) where rownum<2");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "LeadWon", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.lacno as doc_no,to_char(a.lrcdt,'dd/mm/yyyy') as Doc_Dt,a.ldescr as Party_Name,substr(oremarks,1,40) as Last_rmk,a.ent_by,a.ent_dt as VDD from wb_lead_act a where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `LA%` AND upper(a.curr_Stat)='LOST' order by vdd desc,a.lacno desc) where rownum<50");


            //seekSql = "Select to_char(a.vchdate,'dd/mm/yyyy') as doc_date,round(sum((a.tot_dt-nvl(a.num15,0)))/60,2) as totaldt,round(sum(a.total*a.fm_Fact),0) as totalwrk,sum((a.iqtyin+nvl(a.mlt_loss,0))*b.iweight) as Tot_Prodn_kg,sum(a.iqtyin) as Tot_ok,sum(nvl(a.mlt_loss,0)) as Tot_rej,sum((Case when a.ntempr=0 then 0 else round((((a.total*a.fm_Fact)-((a.tot_dt-nvl(a.num15,0))/60))*a.ntempr),2) end)) as tgt_shot ,sum((Case when a.ntempr=0 then 0 else (a.noups*a.fm_Fact) end)) as act_shot,round(sum(a.total*a.fm_Fact),2) as rep_hrs  from prod_sheet a,item b where trim(A.icode)=trim(b.icode) and a.branchcd='" & mbr & "' and a.type='90' and to_chaR(a.vchdate,'yyyymm')='" & Format(MDT1, "yyyymm") & "' group by to_char(a.vchdate,'dd/mm/yyyy') order by to_char(a.vchdate,'dd/mm/yyyy')"
            string dt_form = "";
            string dt_vty = "";
            if (frm_cocd == "SDM" || frm_cocd == "DLJM")
            {
                dt_form = "round(sum((a.tot_dt-nvl(a.num15,0)))/60,2)";
                dt_vty = "90";
                dsrno = "24";
                fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "DTM_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(a.VCHDATE,`Mon`) AS YR," + dt_form + " AS DT_HRs,`Down Time(Hrs) (`||to_char(vchdate,`YYYYMM`)||`)` as col3,`line` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM prod_Sheet A WHERE a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `" + dt_vty + "%` and a.vchdate DT_RANGE GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");
                fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "DTM_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(a.VCHDATE,`Mon`) AS YR," + dt_form + " AS Down_time_hrs FROM prod_Sheet A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `" + dt_vty + "%` and a.vchdate DT_RANGE GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(a.vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");

                dsrno = "25";
                dt_form = "round(sum((Case when a.ntempr=0 then 0 else (a.noups*a.fm_Fact) end)),0)";
                fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prd_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(a.VCHDATE,`Mon`) AS YR," + dt_form + " AS Prod_shots,`Prodn Shots(Count) (`||to_char(vchdate,`YYYYMM`)||`)` as col3,`pie` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM prod_Sheet A WHERE a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `" + dt_vty + "%` and a.vchdate DT_RANGE GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");
                fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prd_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(a.VCHDATE,`Mon`) AS YR," + dt_form + " AS Prod_shots FROM prod_Sheet A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `" + dt_vty + "%` and a.vchdate DT_RANGE GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(a.vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");
            }

            if (frm_cocd == "MCPL")
            {
                //dt_form = "round(sum((a.tot_dt-nvl(a.num15,0)))/60,2)";
                //dt_vty = "90";

                dsrno = "24";
                fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "JOB_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(a.VCHDATE,`Mon`) AS YR,count(*) AS No_of_Jobs,`Job Cards(Count) (`||to_char(vchdate,`YYYYMM`)||`)` as col3,`line` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM costestimate A WHERE a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `30%` and a.vchdate DT_RANGE and a.srno=1 GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");
                fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "JOB_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(a.VCHDATE,`Mon`) AS YR,count(*) AS No_of_Job_Cards FROM costestimate A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `30%` and a.vchdate DT_RANGE and a.srno=1 GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(a.vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");

                //dsrno = "25";
                //dt_form = "round(sum((Case when a.ntempr=0 then 0 else (a.noups*a.fm_Fact) end)),0)";
                //fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prd_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(a.VCHDATE,`Mon`) AS YR," + dt_form + " AS Prod_shots,`Prodn Shots(Count) (`||to_char(vchdate,`YYYYMM`)||`)` as col3,`pie` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM prod_Sheet A WHERE a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `" + dt_vty + "%` and a.vchdate DT_RANGE GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");
                //fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "Prd_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(a.VCHDATE,`Mon`) AS YR," + dt_form + " AS Prod_shots FROM prod_Sheet A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `" + dt_vty + "%` and a.vchdate DT_RANGE GROUP BY TO_cHAR(a.VCHDATE,`Mon`),to_Char(a.vchdate,`YYYYMM`) order by to_Char(a.vchdate,`YYYYMM`)");
            }

            string rt_fld = "";
            rt_fld = "( qtyord*(decode(nvl(wk3,0),0,1,wk3)*((prate*(100-pdisc)/100))-nvl(pdiscamt,0)) )";

            dsrno = "26";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "PO_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(orddt,`Mon`) AS YR,ROUND(SUM(" + rt_fld + "/" + home_divider + "),2) AS PO_VAL,`(Dom)PO Issued Data(" + home_curr + " " + home_div_iden + ") (`||to_char(orddt,`YYYYMM`)||`)` as col3,`bar` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM pomas A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `5%` and a.type!='54' and orddt DT_RANGE GROUP BY TO_cHAR(orddt,`Mon`),to_Char(orddt,`YYYYMM`) order by to_Char(orddt,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "PO_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(orddt,`Mon`) AS YR,ROUND(SUM(" + rt_fld + "/" + home_divider + "),2) AS PO_val,count(*) as PO_Lines FROM pomas A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `5%` and a.type!='54' and orddt DT_RANGE GROUP BY TO_cHAR(orddt,`Mon`),to_Char(orddt,`YYYYMM`) order by to_Char(orddt,`YYYYMM`)");

            dsrno = "27";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "IPO_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(orddt,`Mon`) AS YR,ROUND(SUM(" + rt_fld + "/" + home_divider + "),2) AS PO_VAL,`(Imp)PO Issued Data(" + home_curr + " " + home_div_iden + ") (`||to_char(orddt,`YYYYMM`)||`)` as col3,`pie` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM pomas A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `5%` and a.type='54' and orddt DT_RANGE GROUP BY TO_cHAR(orddt,`Mon`),to_Char(orddt,`YYYYMM`) order by to_Char(orddt,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "IPO_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(orddt,`Mon`) AS YR,ROUND(SUM(" + rt_fld + "/" + home_divider + "),2) AS PO_val,count(*) as PO_Lines FROM pomas A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `5%` and a.type='54' and orddt DT_RANGE GROUP BY TO_cHAR(orddt,`Mon`),to_Char(orddt,`YYYYMM`) order by to_Char(orddt,`YYYYMM`)");

            rt_fld = "( qtyord*(decode(nvl(curr_Rate,0),0,1,curr_Rate)*((irate*(100-cdisc)/100))-0) )";
            dsrno = "28";
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "ISO_Graph", "GRAPH" + dsrno, "SELECT TO_cHAR(orddt,`Mon`) AS YR,ROUND(SUM(" + rt_fld + "/" + home_divider + "),2) AS PO_VAL,`Sales Order Rcvd Data(" + home_curr + " " + home_div_iden + ") (`||to_char(orddt,`YYYYMM`)||`)` as col3,`pie` AS COL4,`GRAPH`||`$`||`GRAPH" + dsrno + "` AS COL5  FROM Somas A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `4%` and orddt DT_RANGE GROUP BY TO_cHAR(orddt,`Mon`),to_Char(orddt,`YYYYMM`) order by to_Char(orddt,`YYYYMM`)");
            fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "ISO_Graph", "POP_GRAPH" + dsrno, "SELECT 'dtb' as fstr,TO_cHAR(orddt,`Mon`) AS YR,ROUND(SUM(" + rt_fld + "/" + home_divider + "),2) AS SO_val,count(*) as PO_Lines FROM Somas A WHERE BRANCHCD=`BR_VAR` AND TYPE LIKE `4%` and orddt DT_RANGE GROUP BY TO_cHAR(orddt,`Mon`),to_Char(orddt,`YYYYMM`) order by to_Char(orddt,`YYYYMM`)");

        }
        ////dsrno = "07";
        ////string mqry = "";
        ////mqry = "select count(*) as cnt,sum(balq)*rtd as balval from select max(dlv_Dt) as dlvdt,val1,sum(val2)-sum(val3) as delq,max(irate) as rtd from (SELECT to_char(cu_chldt,'yyyymmdd') as Dlv_DT,trim(Acode)||trim(icode)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')  as val1,qtyord as val2,0 as val3,irate from somas where branchcd='03' and type like '4%' and qtyord>0  union all SELECT null as Dlv_DT,trim(Acode)||trim(icode)||trim(ponum)||to_char(podate,'dd/mm/yyyy')  as val1,0 as val2,iqtyout as val3,null as irate from ivoucher where branchcd='03' and type like '4%' ) group by val1 having sum(val2)-sum(val3)>0";

        ////fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "SO.DLV", "TXT_TILE" + dsrno, "select `TILE" + dsrno + "` as fstr,`Sales MTD : ` ||(VAL1) as Mdata,`Sales Today : ` ||(VAL2) as Mdata2,`Sales Prev Mth : ` ||(VAL3) as Mdata3,`Comparison : ` ||((CASE WHEN VAL3>0 THEN ROUND((VAL1/VAL3)*100,2) ELSE 0 END))||` %` as Mdata4  from (SELECT SUM(NVL(val1,0)) AS VAL1,SUM(NVL(val2,0)) AS VAL2,SUM(NVL(val3,0)) AS VAL3 FROM (SELECT to_char(cu_chldt,'yyyymmdd') as Dlv_DT,trim(Acode)||trim(icode)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')  as val1,qtyord as val2,0 as val3 from somas where branchcd=`BR_VAR` and type like '4%' and orddt DT_RANGE  and qtyord>0  union all SELECT null as Dlv_DT,trim(Acode)||trim(icode)||trim(ponum)||to_char(podate,'dd/mm/yyyy')  as val1,0 as val2,iqtyout as val3 from ivoucher where branchcd=`BR_VAR` and type like '4%' and vchdate DT_RANGE )) where rownum<2");
        ////fgen.Dsk_Tile_save(frm_cocd, frm_qstr, "SO.DLV", "POP_TILE" + dsrno, "select * from (SELECT distinct `TILE" + dsrno + "` as fstr,a.Ordno as doc_no,to_char(a.Orddt,'dd/mm/yyyy') as Doc_Dt,b.aname as Party_Name,to_Char(a.total,'99,99,99,999') as doc_Value,a.ent_by,a.app_by,to_char(a.orddt,'yyyymmdd') as VDD from somas a,famst b  where a.BRANCHCD=`BR_VAR` AND a.TYPE LIKE `4%` AND a.orddt DT_RANGE and trim(A.acode)=trim(B.acode) order by vdd desc,a.ordno desc) where rownum<50");
    }

    public void Icon_Mkt_ord_for_customer(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Sales order Management ( Dom) for Customer 
        // ------------------------------------------------------------------
        string mhd = "";
        //mhd = fgen.chk_RsysUpd("DCUST101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DCUST101') ");

            ICO.add_iconRights(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47100", 3, "Dom.Order Activity", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47106", 4, "Supply S.O. (Dom.)", 3, "../tej-base/om_so_entry.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e1pp", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F47140", 3, "Dom.Order Reports", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47222", 4, "Order Vs Dispatch", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47223", 4, "Schedule Vs Dispatch", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47239", 4, "Bill Wise Shipment", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F47131", 3, "Dom.Orders Checklists", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47136", 4, "Pending Order Checklist(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F47131", 3, "Dom.Orders Checklists", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47132", 4, "Master S.O. Checklists(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47133", 4, "Supply S.O. Checklists(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47134", 4, "Supply Sch. Checklists(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47135", 4, "Schedule Vs Dispatch  (Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47136", 4, "Pending Order Checklist(Dom.)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e3pp", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F47140", 3, "Dom.Order Reports", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47141", 4, "All Order Register(Dom.)", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F47142", 4, "Pending Order Register(Dom.)", 3, "../tej-base/om_prt_smktg.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e4pp", "fa-edit");

            //***********

            ICO.add_iconRights(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50121", 3, "Dom.Orders CheckLists", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50126", 4, "Order Data Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50127", 4, "Pending Order Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50128", 4, "Pending Sch. Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e2", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F50221", 3, "More Reports(Dom.Sales)", 3, "-", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50228", 4, "31 Day Wise Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit");

            ICO.add_iconRights(frm_qstr, "F50240", 4, "Schedule Vs Dispatch 31 Day", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50241", 4, "Schedule Vs Dispatch 12 Month", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit");
            ICO.add_iconRights(frm_qstr, "F50242", 4, "Schedule Vs Prodn Vs Dispatch Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e1", "fin50_a1", "fin50pp_e44", "fa-edit");
        }
    }

    public void Icon_Mkt_ord_for_vendor(string frm_qstr, string frm_cocd)
    {
        // ------------------------------------------------------------------
        // Purchase order Management for vendors
        // ------------------------------------------------------------------
        ICO.add_iconRights(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F15100", 2, "Purchase Activity", 3, "-", "-", "Y", "fin15_e1", "fin15_a1", "-", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F15106", 3, "Purchase Orders Entry", 3, "../tej-base/om_po_entry.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
    }

    public void PremiumSalesReport(string frm_qstr, string frm_cocd)
    {
        ///Premium Sales Reports        
        ICO.add_icon(frm_qstr, "F50000", 1, "Sales & Despatch Management", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F50050", 2, "Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F50300", 3, "Premium Features-Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e7", "fa-edit");
        ICO.add_icon(frm_qstr, "F50303", 3, "Premium Reports- Domestic Sales", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "fin50pp_e45", "fa-edit");
    }
    public void PremiumFinanceReport(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70501", 2, "Premium Features- Finance/Acctg", 3, "-", "-", "Y", "fin70_e8", "fin70_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F70502", 3, "Premium Reports-Finance/Acctg", 3, "-", "-", "Y", "fin70_e8", "fin70_a1", "fin70pp_e46", "fa-edit");
        ICO.add_icon(frm_qstr, "F70147", 3, "More Checklists(Accounts)", 3, "-", "-", "-", "fin70_e8", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
    }
    public void PremiumProductionReport(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F40052", 2, "Premium Features - Production", 3, "-", "-", "Y", "fin40_e2", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F40059", 3, "Premium Reports - Production ", 3, "-", "-", "-", "fin40_e2", "fin40_a1", "fin40_pfrep", "fa-edit");
    }
    public void PremiumEnggReport(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F10301", 2, "Premium Features - Engg/Planning", 3, "-", "-", "Y", "fin10_e7", "fin10_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F10306", 3, "Premium Reports - Engg/Planning ", 3, "-", "-", "Y", "fin10_e7", "fin10_a1", "fin10_pfrep", "fa-edit");
    }
    public void IconCustomerRequestSELStyle(string frm_qstr, string frm_cocd)
    {
        string mhd = fgen.chk_RsysUpd("CREQ01");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('CREQ01') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "CREQ01", "DEV_A");


            ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10049", 2, "Customer Care", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10050", 3, "Customer Request", 3, "../tej-base/cmplnt.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10051", 3, "Customer Request Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10052", 3, "Action on Request", 3, "../tej-base/neopaction.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10055", 3, "Old M/C Entry", 3, "../tej-base/oldMcData.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10550", 3, "Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10551", 3, "Type of Request Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10552", 3, "Division Master", 3, "../tej-base/neopappmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10553", 3, "Person Master", 3, "../tej-base/personmst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10554", 3, "Visit Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F10555", 3, "Information Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F10052S", 3, "Request Summary", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
            ICO.add_icon(frm_qstr, "F10053", 3, "Request Status", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit", "Y", "Y");
        }
    }

    public void IconCustomerRequestforCustomer(string frm_qstr, string frm_cocd)
    {
        ICO.add_iconRights(frm_qstr, "F10000", 1, "Engg/Masters Module", 3, "-", "-", "Y", "-", "fin10_a1", "-", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F10049", 2, "Customer Care", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F10050", 3, "Customer Request", 3, "../tej-base/cmplnt.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F10052S", 3, "Request Summary", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
        ICO.add_iconRights(frm_qstr, "F10053", 3, "Request Status", 3, "../tej-base/om_view_task.aspx", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");
    }
    public void IconCastingProd(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F40000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F41000", 2, "Casting Production", 3, "-", "-", "Y", "fin41_e1", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F41001", 3, "Tool Breakage Form", 3, "../tej-prodcast-web/tbrk.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F41002", 3, "More Reports - Casting Prod.", 3, "-", "-", "-", "fin41_e1", "fin40_a1", "fin41pp_rep", "fa-edit");
        ICO.add_icon(frm_qstr, "F41003", 3, "Tool Breakage Report", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F41004", 3, "Opr wise Stock", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F41005", 4, "Opr wise Rej and Supp", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "fin41pp_rep", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F41006", 4, "SF Plan vs Prod Plan", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "fin41pp_rep", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F41007", 3, "Schedule vs SF Plan", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit", "N", "Y");
        ICO.add_icon(frm_qstr, "F41008", 3, "Rejection Report", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F41011", 3, "Dept wise Stock Transfer Report", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F41012", 3, "Date Wise Prod. Report", 3, "../tej-prodcast-reps/om_view_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F41013", 4, "Tool History Card", 3, "../tej-prodcast-reps/om_prt_prodcast.aspx", "-", "-", "fin41_e1", "fin40_a1", "fin41pp_rep", "fa-edit", "N", "N");

        ICO.add_icon(frm_qstr, "F41009", 3, "Shift Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F41010", 3, "Fixture Details", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin41_e1", "fin40_a1", "-", "fa-edit");
    }

    public void IconBoxCostSURY(string frm_qstr, string frm_cocd)
    {
        ICO.add_icon(frm_qstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F10181", 2, "Product Costing", 3, "-", "-", "Y", "fin10_e6", "fin10_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F10241", 3, "Costing Masters", 3, "-", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");

        ICO.add_icon(frm_qstr, "F10242", 4, "Lamination Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10243", 4, "Printing Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10244", 4, "UV Printing Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10245", 4, "Screen Printing Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10246", 4, "Micro Printing Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10247", 4, "Drip off Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10248", 4, "Spot UV Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10249", 4, "Foiling Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10250", 4, "Punching Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10251", 4, "Embossing Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10252", 4, "Wastage Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10253", 4, "Delivery Charges Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10254", 4, "Payment Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10255", 4, "Gloss Varnish Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");
        ICO.add_icon(frm_qstr, "F10257", 4, "Paper Master", 3, "../tej-base/om_wbtgpop_mst.aspx", "-", "-", "fin10_e6", "fin10_a1", "fin10_ecm", "fa-edit");

        ICO.add_icon(frm_qstr, "F10256", 3, "Box(Laminated) Costing- Calculator", 3, "../tej-base/om_cost_bprint.aspx", "-", "-", "fin10_e6", "fin10_a1", "-", "fa-edit");

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_TRAN_COST'", "TNAME");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, " CREATE TABLE WB_TRAN_COST (branchcd char(2)default '-',type char(4)default '-',vchnum varchar2(6)default '-',vchdate date default sysdate,icode varchar2(8),iname varchar2(150),acode varchar2(6) default '-',aname varchar2(75) default '-',col1 varchar2(70) default '-',num1 number(20,3) default 0 ,num2 number(20,3) default 0 ,num3 number(20,3) default 0,num4 number(20,3) default 0,num5 number(20,3) default 0,col2 varchar2(70) default '-',num6 number(20,3) default 0,num7 number(20,3) default 0,num8 number(20,3) default 0,num9 number(20,3) default 0,num10 number(20,3) default 0,col3 varchar2(70) default '-',num11 number(20,3) default 0,num12 number(20,3) default 0,num13 number(20,3) default 0,num14 number(20,3) default 0,num15 number(20,3) default 0,num16 number(20,3) default 0,col4 varchar2(70) default '-',col5 varchar2(70) default '-',num17 number(20,3) default 0,num18 number(20,3) default 0,num19 number(20,3) default 0,num20 number(20,3) default 0,col6 varchar2(70) default '-',col7 varchar2(70) default '-',num21 number(20,3) default 0,num22 number(20,3) default 0,num23 number(20,3) default 0,num24 number(20,3) default 0,col8 varchar2(70) default '-',col9 varchar2(70) default '-',num25 number(20,3) default 0,num26 number(20,3) default 0,num27 number(20,3) default 0,num28 number(20,3) default 0,num29 number(20,3) default 0,col10 varchar2(70) default '-',col11 varchar2(70) default '-',num30 number(20,3) default 0,col12 varchar2(70) default '-',col13 varchar2(70) default '-',num31 number(20,3) default 0,col14 varchar2(70) default '-',col15 varchar2(70) default '-',num32 number(20,3) default 0,num33 number(20,3) default 0,col16 varchar2(70) default '-',col17 varchar2(70) default '-',num34 number(20,3) default 0,col18 varchar2(70) default '-',col19 varchar2(70) default '-',num35 number(20,3) default 0,col20 varchar2(70) default '-',col21 varchar2(70) default '-',num36 number(20,3) default 0,num37 number(20,3) default 0,col22 varchar2(70) default '-',num38 number(20,3) default 0,col23 varchar2(70) default '-',num39 number(20,3) default 0,col24 varchar2(70) default '-',num40 number(20,3) default 0,num41 number(20,3) default 0,col25 varchar2(70) default '-',num42 number(20,3) default 0,col26 varchar2(70) default '-',num43 number(20,3) default 0,col27 varchar2(70) default '-',num44 number(20,3) default 0,num45 number(20,3) default 0,col28 varchar2(70) default '-',num46 number(20,3) default 0,num47 number(20,3) default 0,num48 number(20,3) default 0,col29 varchar2(70) default '-',num49 number(20,3) default 0,num50 number(20,3) default 0,col30 varchar2(70) default '-',num51 number(20,3) default 0,num52 number(20,3) default 0,num53 number(20,3) default 0,col31 varchar2(70) default '-',num54 number(20,3) default 0,num55 number(20,3) default 0,col32 varchar2(70) default '-',num56 number(20,3) default 0,num57 number(20,3) default 0,num58 number(20,3) default 0,col33 varchar2(70) default '-',num59 number(20,3) default 0,num60 number(20,3) default 0,num61 number(20,3) default 0,num62 number(20,3) default 0,col34 varchar2(70) default '-',num63 number(20,3) default 0,col35 varchar2(70) default '-',num64 number(20,3) default 0,col36 varchar2(70) default '-',num65 number(20,3) default 0,num66 number(20,3) default 0,col37 varchar2(70) default '-',num67 number(20,3) default 0,col38 varchar2(70) default '-',num68 number(20,3) default 0,col39 varchar2(70) default '-',num69 number(20,3) default 0,num70 number(20,3) default 0,col40 varchar2(70) default '-',num71 number(20,3) default 0,col41 varchar2(70) default '-',num72 number(20,3) default 0,col42 varchar2(70) default '-',num73 number(20,3) default 0,num74 number(20,3) default 0,col43 varchar2(70) default '-',num75 number(20,3) default 0,col44 varchar2(70) default '-',num76 number(20,3) default 0,col45 varchar2(70) default '-',num77 number(20,3) default 0,num78 number(20,3) default 0,grossamt number(20,3) default 0,col46 varchar2(70) default '-',num79 number(20,3) default 0,num80 number(20,3) default 0,col47 varchar2(70) default '-',num81 number(20,3) default 0,num82 number(20,3) default 0,col48 varchar2(70) default '-',num83 number(20,3) default 0,num84 number(20,3) default 0,total number(20,3) default 0,col49 varchar2(70) default '-',num85 number(20,3) default 0,col50 varchar2(70) default '-',num86 number(20,3) default 0,paytot number(20,3) default 0,col51 varchar2(70) default '-',num87 number(20,3) default 0,col52 varchar2(70) default '-',num88 number(20,3) default 0,grandtot number(20,3) default 0,ENT_BY VARCHAR2(10) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(10) NOT NULL,EDT_DT DATE NOT NULL)");
    }

    public void IconMouldMaint(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("MLDM101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('MLDM101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "MLDM101", "DEV_A");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_MAINT'", "TNAME");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_MAINT(BRANCHCD CHAR(2),TYPE CHAR(4),VCHNUM CHAR(6),VCHDATE DATE,TITLE VARCHAR2(100),BTCHNO CHAR(20),ACODE CHAR(10),ICODE CHAR(10),CPARTNO VARCHAR2(30),GRADE VARCHAR2(20),SRNO NUMBER(4),COL1 VARCHAR2(100),COL2 VARCHAR2(100),COL3 VARCHAR2(100),COL4 VARCHAR2(100),COL5 VARCHAR2(100),COL6 VARCHAR2(100),COL7 VARCHAR2(100),COL8 VARCHAR2(100),COL9 VARCHAR2(100),COL10 VARCHAR2(100),COL11 VARCHAR2(100),COL12 VARCHAR2(100),COL13 VARCHAR2(100),COL14 VARCHAR2(100),COL15 VARCHAR2(100),DATE1 DATE,DATE2 DATE,RESULT VARCHAR2(250),OBSV1 VARCHAR2(15),OBSV2 VARCHAR2(15),OBSV3 VARCHAR2(50),OBSV4 VARCHAR2(15),OBSV5 VARCHAR2(15),OBSV6 VARCHAR2(15),OBSV7 VARCHAR2(15),OBSV8 VARCHAR2(15),OBSV9 VARCHAR2(15),OBSV10 VARCHAR2(15),OBSV11 VARCHAR2(15),OBSV12 VARCHAR2(15),OBSV13 VARCHAR2(15),OBSV14 VARCHAR2(15),OBSV15 VARCHAR2(15),ENT_BY VARCHAR2(20),ENT_DT DATE,NUM1 NUMBER(15,3),NUM2 NUMBER(15,3),NUM3 NUMBER(15,3),NUM4 NUMBER(15,3),NUM5 NUMBER(15,3),EDT_BY VARCHAR2(20),EDT_DT DATE,REMARKS VARCHAR2(300))");

            ICO.add_icon(frm_qstr, "F75000", 1, "Maintenance Module", 3, "-", "-", "Y", "fin75_e1", "fin75_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F75148", 2, "Mould Maintenance", 3, "-", "-", "Y", "fin75_e9", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75149", 3, "Mould Detailed Specifications", 3, "../tej-base/om_mld_mast.aspx", "Detailed Health Specifications", "-", "fin75_e9", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75150", 3, "Mould Preventive Maintenance Plan", 3, "../tej-base/om_maint_plan.aspx", "Automatic Preventive Monthwise Plan Entry", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75151", 3, "Mould Preventive Maintenance Entry", 3, "../tej-base/om_maint_Act.aspx", "To record Mould wise Preventive Maintenance", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75152", 3, "Mould Break Down Entry", 3, "../tej-base/om_maint_break_qa.aspx", "Form to enter Break Down", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75153", 3, "Mould OK for Production Entry", 3, "../tej-base/om_maint_plan.aspx", "Mould OK for production after break down", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75155", 3, "Mould Health Maintenance Plan", 3, "../tej-base/om_maint_plan.aspx", "Automatic Health Monthwise Plan Entry", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75156", 3, "Mould Health Maintenance Record", 3, "../tej-base/om_maint_Act.aspx", "To record Mould wise Health Maintenance", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75181", 3, "Mould B/D Production Approval Entry", 3, "../tej-base/om_maint_break_qa.aspx", "Mould Quality check-1 entry after break down", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75182", 3, "Mould B/D Quality Approval Entry", 3, "../tej-base/om_maint_break_qa.aspx", "Mould Quality check-2 entry after break down", "-", "fin75_e9", "fin75_a1", "-", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F75154", 3, "Mould Preventive Maintenance Reports", 3, "-", "-", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75164", 4, "Mould PM Day wise Plan Report", 3, "../tej-base/om_prt_maint.aspx", "Mould wise Day wise PM Print", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75160", 4, "Mould PM Day wise Plan Checklist", 3, "../tej-base/om_view_maint.aspx", "Mould wise Day wise PM Grid", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75168", 4, "Mould PM Plan Vs Actual(Month wise)", 3, "../tej-base/om_prt_maint.aspx", "Month wise PM Plan vs Actual Comparision", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75170", 4, "Mould PM Plan Vs Actual(Day wise)", 3, "../tej-base/om_prt_maint.aspx", "Day wise PM Plan vs Actual Comparision", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75185", 4, "Balance PM Life", 3, "../tej-base/om_view_maint.aspx", "Mould wise Balance PM Life on selected Date", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75189", 4, "Mould Planned pending Maintenance-PM", 3, "../tej-base/om_prt_maint.aspx", "Mould planned for PM but not maintained within the month", "-", "fin75_e9", "fin75_a1", "fin75_mmpm", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F75175", 3, "Mould Health Maintenance Reports ", 3, "-", "-", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75158", 4, "Mould HM Day wise Plan Checklist", 3, "../tej-base/om_view_maint.aspx", "Mould wise Day wise HM Grid", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75167", 4, "Mould HM Plan Vs Actual(Month wise)", 3, "../tej-base/om_prt_maint.aspx", "Mould wise Month wise HM Plan vs Actual Comparision", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75169", 4, "Mould HM Plan Vs Actual(Day wise)", 3, "../tej-base/om_prt_maint.aspx", "Mould wise Day wise HM Plan vs Actual Comparision", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75163", 4, "Mould HM Day wise Plan Report", 3, "../tej-base/om_prt_maint.aspx", "Mould wise Day wise HM Print", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75186", 4, "Balance HM Life", 3, "../tej-base/om_view_maint.aspx", "Mould wise Balance HM Life on selected Date", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75190", 4, "Mould Planned pending Maintenance- HM", 3, "../tej-base/om_view_maint.aspx", "Mould planned for PM but not maintained within the month", "-", "fin75_e9", "fin75_a1", "fin75_mmhm", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F75166", 3, "Mould Breakdown Reports", 3, "-", "-", "-", "fin75_e9", "fin75_a1", "fin75_mmbk", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75198", 4, "Mould Breakdown Reason Report", 3, "../tej-base/om_prt_maint.aspx", "Mould wise Breakdown reason in selected time period", "-", "fin75_e9", "fin75_a1", "fin75_mmbk", "fa-edit", "N", "Y");
            //ICO.add_icon(frm_qstr, "F75184", 4, "Mould Breakdown Report", 3, "../tej-base/om_view_maint.aspx", "Mould wise Breakdown reason in selected time period", "-", "fin75_e9", "fin75_a1", "fin75_mmbk", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75191", 4, "Mould Breakdown pending OK for Production", 3, "../tej-base/om_view_maint.aspx", "Mould Breakdown pending  for OK for Production", "-", "fin75_e9", "fin75_a1", "fin75_mmbk", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75192", 4, "Mould OK pending Production Approval", 3, "../tej-base/om_view_maint.aspx", "Mould OK for Production pending for Quality1", "-", "fin75_e9", "fin75_a1", "fin75_mmbk", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75193", 4, "Mould OK pending Quality Approval", 3, "../tej-base/om_view_maint.aspx", "Mould Quality1 pending for Quality2", "-", "fin75_e9", "fin75_a1", "fin75_mmbk", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F75177", 3, "More Reports ( Mould Maintenance)", 3, "-", "-", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75157", 4, "Mould Maintenance parts consumed", 3, "../tej-base/om_view_maint.aspx", "Parts Consumed in Moulds maintenance during selected time period", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75197", 4, "Mould Deatils Checklist", 3, "../tej-base/om_view_maint.aspx", "Mould specifications as entered in detailed specifications", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75176", 4, "Mould Health Checklist", 3, "../tej-base/om_view_maint.aspx", "Mould wise production & health report during Financial Year", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75183", 4, "New Mould Installed", 3, "../tej-base/om_view_maint.aspx", "New Moulds Installed during selected Period", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75187", 4, "Balance Mould Production Life", 3, "../tej-base/om_view_maint.aspx", "Balance production life mould wise on selected date", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75188", 4, "Moulds without detailed specs", 3, "../tej-base/om_view_maint.aspx", "Moulds for which detailed specifications not entered", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F75200", 4, "Mould Breakdown Monthly Instances Graph", 3, "../tej-base/om_view_maint.aspx", "Showing Month wise Mould Breakdowns", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75202", 4, "Mould Maintenance Monthly Cost Graph", 3, "../tej-base/om_view_maint.aspx", "Showing Month wise Costs Incurred", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75204", 4, "Mould Maintenance Monthly Count,Cost Data", 3, "../tej-base/om_view_maint.aspx", "Showing Month wise Count,Costs Data", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75206", 4, "Mould Total Life,Usage Record ", 3, "../tej-base/om_view_maint.aspx", "Showing Total Life,Usage Data", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F75208", 3, "Mould Disposed Entry", 3, "../tej-base/om_mould_disp.aspx", "Mould Disposed Specifications", "-", "fin75_e9", "fin75_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F75209", 3, "Mould Preventive Planned but not Maintained", 3, "../tej-base/om_view_maint.aspx", "Mould Preventive Planned but not Maintained", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75210", 3, "Mould Health Planned but not Maintained", 3, "../tej-base/om_view_maint.aspx", "Mould Health Planned but not Maintained", "-", "fin75_e9", "fin75_a1", "fin75_mmm", "fa-edit", "N", "N");

        }
    }

    public void PremiumEmktgReport(string frm_qstr, string frm_cocd)
    {
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_EXP_IMP'", "TNAME");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_EXP_IMP (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR2(6),VCHDATE  DATE,ACODE VARCHAR2(6),ICODE VARCHAR2(10),SRNO CHAR(4),ENTRY_NO_BILL VARCHAR2(20),ENTRY_DT_BILL DATE,INVNO VARCHAR2(20),INVDATE DATE,MRRNUM  VARCHAR2(6),MRRDATE DATE,CETSHNO VARCHAR2(10),PONUM VARCHAR2(6),PODATE DATE,POQTY  NUMBER(20,3),QTY_REC NUMBER(20,3),COUNTRY VARCHAR2(50),DESP_DT DATE,FOREIGN_VAL NUMBER(20,3),AMT_INR NUMBER(20,3),CIFVAL NUMBER(20,3),INSUR_INR NUMBER(20,3),FREIGHT_INR_SB NUMBER(20,3),SHIP_BILLNO  VARCHAR2(60),SHIP_BILLDT VARCHAR2(10),SHIP_LEODT VARCHAR2(10),SHIP_LINES  VARCHAR2(80),SHIP_LINES_CHG VARCHAR2(10),CONT_NO VARCHAR2(20),IMP_MODE VARCHAR2(30),PORT_CLEARANCE VARCHAR2(20),EXCH_RT  NUMBER(20,3),FOB NUMBER(20,3),IMP_EXP_UNDER VARCHAR2(50),DUTY VARCHAR2(20),IGST_PAID NUMBER(20,3),IGST_REC_DT VARCHAR2(10),ADV_LICNO  VARCHAR2(60),DBK_CLAIMED_AMT  NUMBER(20,3),DBK_REC_DT VARCHAR2(10),CHA  VARCHAR2(60),BANK_REF VARCHAR2(30),PYMT_DUE VARCHAR2(10),DELV_DT  VARCHAR2(10),REMARKS VARCHAR2(200),IMPORT_TERM  VARCHAR2(30),BILL_FWD VARCHAR2(10),PYMT_DATE VARCHAR2(10),EXHG_BRC   NUMBER(20,3),FREIGHT_INR_SL NUMBER(20,3),INS_PREM NUMBER(20,3),COMM  VARCHAR2(10),FOB_INR  NUMBER(20,3),FOB_FOREIGN  NUMBER(20,3),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY  VARCHAR2(20),EDT_DT DATE,REMARKS2 VARCHAR2(100),CSCODE VARCHAR2(6),CURR_RATE NUMBER(20,3))");
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_LICREC'", "TNAME");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_LICREC( BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE  DATE,ACODE CHAR(10) DEFAULT '-',ICODE CHAR(10) DEFAULT '-',HSCODE VARCHAR2(15),LICNO CHAR(15) NOT NULL,LICDT  DATE ,SRNO NUMBER(5),QTYIN  NUMBER(25,2) ,QTYOUT  NUMBER(25,2),IAMOUNT NUMBER(25,2),CIF_VAL NUMBER(25,2),BALQTY NUMBER(25,2),BALCIF_VAL  NUMBER(25,2),FOB_VAL NUMBER(25,2),CINAME VARCHAR2(135),DESC_ VARCHAR2(200),REFNUM CHAR(15),REFDATE DATE,FLAG CHAR(2),BILLNO VARCHAR2(30),BILL_DT DATE,INVNO VARCHAR2(10),INVDATE DATE,CSCODE CHAR(10),APP_BY VARCHAR2(20),PBASIS VARCHAR2(100),ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE,EDT_BY VARCHAR2(15) NOT NULL,EDT_DT DATE,REMARK VARCHAR2(300),TERM   VARCHAR2(100),FGCODE VARCHAR2(150),RMCODE VARCHAR2(150),EXPVALID DATE,IMPVALID DATE,DGFT_FILE VARCHAR2(80),VAL_ADD  NUMBER(30,3),VAL_USD  NUMBER(30,3),WAST_PERC NUMBER(30,3),IMP_QTY NUMBER(30,3),EXP_QTY NUMBER(30,3),EXP_VAL  NUMBER(30,3),IMP_VAL  NUMBER(30,3),NUM1  NUMBER(30,2),NUM2 NUMBER(30,2),NUM3 NUMBER(30,2),NUM4 NUMBER(30,2),NUM5 NUMBER(30,2),WAST_PERC2 NUMBER(30,2),NUM6 NUMBER(30,2),NUM7 NUMBER(30,2),NUM8 NUMBER(30,2),NUM9 NUMBER(30,2),NUM10 NUMBER(30,2),OBSV1 VARCHAR2(80),OBSV2 VARCHAR2(80),OBSV3 VARCHAR2(80),OBSV4 VARCHAR2(80),OBSV5 VARCHAR2(80))");
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_EXP_FRT'", "TNAME");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_EXP_FRT(BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,ACODE CHAR(10),ICODE CHAR(10),SRNO NUMBER(10),IQTYIN NUMBER(35,2),IQTYOUT NUMBER(35,2),IAMOUNT NUMBER(35,2),CIF_VAL NUMBER(35,2),FORIGN_VAL NUMBER(35,2),CONTAINER_NO VARCHAR2(100),CONT_SIZE VARCHAR2(100),CINAME VARCHAR2(135),DESC_  VARCHAR2(200),REFNUM CHAR(15),REFDATE DATE,FLAG CHAR(2),BILLNO VARCHAR2(30),BILL_DT DATE,INVNO  VARCHAR2(10),INVDATE DATE,CSCODE CHAR(10),APP_BY VARCHAR2(20),PBASIS VARCHAR2(100),REMARK VARCHAR2(300),TERM  VARCHAR2(100),NUM1 NUMBER(30,2),NUM2 NUMBER(30,2),NUM3 NUMBER(30,2),NUM4 NUMBER(30,2),NUM5 NUMBER(30,2),NUM6 NUMBER(30,2),NUM7 NUMBER(30,2),NUM8 NUMBER(30,2),NUM9 NUMBER(30,2),NUM10 NUMBER(30,2),NUM11 NUMBER(30,2),NUM12 NUMBER(30,2),NUM13 NUMBER(30,2),NUM14 NUMBER(30,2),NUM15 NUMBER(30,2),NUM16 NUMBER(30,2),NUM17 NUMBER(30,2),NUM18 NUMBER(30,2),NUM19 NUMBER(30,2),NUM20 NUMBER(30,2),OBSV1 VARCHAR2(80),OBSV2 VARCHAR2(80),OBSV3 VARCHAR2(80),OBSV4 VARCHAR2(80),OBSV5 VARCHAR2(80),OBSV6 VARCHAR2(80),OBSV7 VARCHAR2(80),OBSV8 VARCHAR2(80),OBSV9 VARCHAR2(80),OBSV10 VARCHAR2(80),OBSV11 VARCHAR2(80),OBSV12 VARCHAR2(80),OBSV13 VARCHAR2(80),OBSV14 VARCHAR2(80),OBSV15 VARCHAR2(80),ENT_BY VARCHAR2(20) NOT NULL,ENT_DT DATE NOT NULL,EDT_BY VARCHAR2(15) NOT NULL,EDT_DT DATE NOT NULL,RMK1  VARCHAR2(100),RMK2   VARCHAR2(100),RMK3 VARCHAR2(100),RMK4 VARCHAR2(100),COMM_DESC VARCHAR2(50),DATE1 VARCHAR2(12),DATE2 VARCHAR2(12),DATE3 VARCHAR2(12),DATE4 VARCHAR2(12),DATE5  VARCHAR2(12),DATE6 VARCHAR2(12),DATE7  VARCHAR2(12),DATE8 VARCHAR2(12),TOTSHIP_CHG VARCHAR2(40),TOTCHA_CHG VARCHAR2(40),SHIP_LINE VARCHAR2(40),HSCODE VARCHAR2(30),VCODE VARCHAR2(20),PORT_DESCH VARCHAR2(30),DELV VARCHAR2(30),RECIEPT VARCHAR2(30),PLOAD  VARCHAR2(30),PRECIEPT VARCHAR2(30),VCODE2 VARCHAR2(20),SHIP_DT DATE)");


        ///Premium Emktg Reports        
        ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F49000", 2, "Export Sales Orders", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F49200", 3, "Exp.Sales View Reports", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e6pp", "fa-edit");
        ICO.add_icon(frm_qstr, "F49201", 3, "Exp.Sales Printable Reports", 3, "-", "-", "Y", "fin49_e1", "fin45_a1", "fin49_e7pp", "fa-edit");
    }

    public void Premium_custeval(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("CEVAL101");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "CEVAL101", "DEV_A");

            ICO.add_icon(frm_qstr, "F77000", 2, "Customer Evaluation", 3, "-", "-", "Y", "fin77_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F77002", 3, "Customer Evaluation Template", 3, "../tej-base/om_qa_templ.aspx", "-", "-", "fin77_e1", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77004", 3, "Customer Evaluation Entry", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin77_e1", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77006", 3, "Customer Evaluation Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin77_e1", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77008", 3, "Customer Evaluation Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin77_e1", "fin45_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Premium_salebudget(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("SBUDG101");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "SBUDG101", "DEV_A");

            ICO.add_icon(frm_qstr, "F77100", 2, "Sales Budget/Targets", 3, "-", "-", "Y", "fin77_e2", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F77101", 3, "Define Sales Segments", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77103", 3, "Sales Budget Entry Segment Wise", 3, "../tej-base/om_mth_budg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77106", 3, "Sales Budget Entry SalesPerson Wise", 3, "../tej-base/om_mth_budg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77109", 3, "Sales Budget Report Segment Wise", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77112", 3, "Sales Budget Report SalesPerson Wise", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
        }
        ICO.add_icon(frm_qstr, "F77114", 3, "Sales Person Monthly Target Vs Actual", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F77116", 3, "Sales Person Order,Sale (Day,Month,Year)", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
        ICO.add_icon(frm_qstr, "F77118", 3, "Sales Person Monthly Expense Report", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin77_e2", "fin45_a1", "-", "fa-edit", "N", "N");
    }
    public void Premium_sman_visit(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("SMANV101");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "SMANV101", "DEV_A");

            ICO.add_icon(frm_qstr, "F77200", 2, "Sales Person Portal", 3, "-", "-", "Y", "fin772_e2", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F77201", 3, "Sales Person Daily Entry", 3, "../tej-base/om_sman_log.aspx", "-", "-", "fin772_e2", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77203", 3, "Sales Person DayWise Report", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin772_e2", "fin45_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F77206", 3, "Sales Person Monthly Visit Report", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin772_e2", "fin45_a1", "-", "fa-edit", "N", "N");

        }
        ICO.add_icon(frm_qstr, "F77207", 3, "Sales Person Visit Register", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin772_e2", "fin45_a1", "-", "fa-edit", "N", "N");
    }

    public void Premium_vehi_maint(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("VMAINT101");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "VMAINT101", "DEV_A");


            ICO.add_icon(frm_qstr, "F75225", 2, "Vehicle Maintenance Module", 3, "-", "-", "Y", "fin75_e6", "fin75_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F75227", 3, "Vehicle Master Creation", 3, "../tej-base/om_vehi_log.aspx", "-", "-", "fin75_e6", "fin75_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75229", 3, "Vehicle Inspection Checklist", 3, "../tej-base/om_qa_templ.aspx", "-", "-", "fin75_e6", "fin75_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F75231", 3, "Vehicle Inspection Record", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin75_e6", "fin75_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75233", 3, "Vehicle Maintenance Record", 3, "../tej-base/om_vehi_mirec.aspx", "-", "-", "fin75_e6", "fin75_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F75235", 3, "Vehicle Incident Record", 3, "../tej-base/om_vehi_mirec.aspx", "-", "-", "fin75_e6", "fin75_a1", "-", "fa-edit", "N", "N");

        }
        //F75228 :: "../tej-base/om_lgl_log.aspx"
    }

    public void Premium_legal_soft(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("LCELL101");
        if (mhd == "0" || mhd == "")
        {
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "LCELL101", "DEV_A");

            ICO.add_icon(frm_qstr, "F93201", 2, "Legal Cell Data Management", 3, "-", "-", "Y", "fin60_L1", "fin60_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F93203", 3, "Legal Cell Entry", 3, "-", "-", "Y", "fin60_L1", "fin60_a1", "fin61LG_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F93205", 4, "Start Legal Case", 3, "../tej-base/om_lgl_log.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F93207", 4, "Approve Legal Case", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e1", "fa-edit");
            ICO.add_icon(frm_qstr, "F93209", 4, "Followup Legal Case", 3, "../tej-base/om_lgl_act.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e1", "fa-edit");

            ICO.add_icon(frm_qstr, "F93211", 3, "Legal Cell Reports", 3, "-", "-", "Y", "fin60_L1", "fin60_a1", "fin61LG_e2", "fa-edit");
            ICO.add_icon(frm_qstr, "F93213", 4, "Legal Cases List", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F93215", 4, "Legal Cases Followup List", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F93217", 4, "Legal Cases Status ", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e2", "fa-edit", "N", "Y");

            ICO.add_icon(frm_qstr, "F93221", 3, "Legal Cell Review", 3, "-", "-", "Y", "fin60_L1", "fin60_a1", "fin61LG_e3", "fa-edit");
            ICO.add_icon(frm_qstr, "F93223", 4, "Legal Cell Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin60_L1", "fin60_a1", "fin61LG_e3", "fa-edit");



        }
    }

    public void Premium_kpi_mgmt(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("KPIM101");
        if (mhd == "0" || mhd == "")
        {
            //cow
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "KPIM101", "DEV_A");

            // ------------------------------------------------------------------
            // KPI Create,Assign,Report Module
            // ------------------------------------------------------------------
            ICO.add_icon(frm_qstr, "F82300", 2, "KPI Management Module", 3, "-", "-", "Y", "fin84_e1", "fin80_a1", "fin84_kp1", "fa-edit");
            ICO.add_icon(frm_qstr, "F82302", 3, "KPI Masters", 3, "-", "-", "Y", "fin84_e1", "fin80_a1", "fin84_kp1", "fa-edit");
            ICO.add_icon(frm_qstr, "F82304", 4, "Create KPI Master", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin84_e1", "fin80_a1", "fin84_kp1", "fa-edit");
            ICO.add_icon(frm_qstr, "F82306", 4, "Assign KPI Activity", 3, "../tej-base/om_appr.aspx", "-", "-", "fin84_e1", "fin80_a1", "fin84_kp1", "fa-edit");

            ICO.add_icon(frm_qstr, "F82312", 3, "Review KPI Reports", 3, "-", "-", "Y", "fin84_e1", "fin80_a1", "fin84_kp2", "fa-edit");
            ICO.add_icon(frm_qstr, "F82314", 4, "Person Wise KPI", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin84_e1", "fin80_a1", "fin84_kp2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F82316", 4, "Department Wise KPI", 3, "../tej-base/om_view_crm.aspx", "-", "-", "fin84_e1", "fin80_a1", "fin84_kp2", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F82318", 4, "Review KPI Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin84_e1", "fin80_a1", "fin84_kp2", "fa-edit", "N", "Y");

        }
    }

    public void IconRFQ_PO(string frm_qstr, string frm_cocd)
    {
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_PORFQ'", "TNAME");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE WB_PORFQ (BRANCHCD  CHAR(2) NOT NULL ,TYPE CHAR(2) NOT NULL ,ORDNO CHAR(6) NOT NULL,ORDDT DATE NOT NULL,ACODE CHAR(10) NOT NULL,UNIT VARCHAR2(10) NOT NULL,PRATE NUMBER(14,3) NOT NULL,PDISC NUMBER(5,2) NOT NULL,PEXC NUMBER(5,2) NOT NULL ,PTAX NUMBER(5,2) NOT NULL,PAMT NUMBER(12,2) NOT NULL ,PSIZE  VARCHAR2(15) NOT NULL,QTYORD NUMBER(12,3) NOT NULL,QTYSUPP NUMBER(12,3) NOT NULL,QTYBAL NUMBER(14,4) NOT NULL ,PORDNO VARCHAR2(30) NOT NULL ,PORDDT DATE NOT NULL,INVNO VARCHAR2(10) NOT NULL,INVDATE DATE NOT NULL,DELIVERY NUMBER(3) NOT NULL,DEL_MTH NUMBER(10,2) NOT NULL,DEL_WK NUMBER(7) NOT NULL ,DEL_DATE DATE NOT NULL ,DELV_TERM  VARCHAR2(150) NOT NULL ,TERM VARCHAR2(1000) NOT NULL ,INST VARCHAR2(1000) NOT NULL,REFDATE DATE NOT NULL,MODE_TPT VARCHAR2(50) NOT NULL,TR_INSUR VARCHAR2(50) NOT NULL ,DESP_TO  VARCHAR2(150) NOT NULL ,FREIGHT VARCHAR2(50) NOT NULL ,DOC_THR  VARCHAR2(100) NOT NULL ,PACKING VARCHAR2(45) NOT NULL ,PAYMENT VARCHAR2(200) NOT NULL ,BANK   VARCHAR2(50) NOT NULL ,REMARK VARCHAR2(300) NOT NULL,DESC_ VARCHAR2(400),STAX VARCHAR2(50) NOT NULL,EXC VARCHAR2(50) NOT NULL,IOPR VARCHAR2(35) NOT NULL ,PR_NO VARCHAR2(6) NOT NULL,AMD_NO VARCHAR2(15) NOT NULL,DEL_SCH VARCHAR2(15) NOT NULL,TAX VARCHAR2(2) NOT NULL ,ICODE CHAR(30) NOT NULL,WK1 NUMBER(10,2) NOT NULL,WK2 NUMBER(10,2) NOT NULL,WK3  NUMBER(10,2) NOT NULL ,WK4   NUMBER(10,2) NOT NULL ,VEND_WT  NUMBER(10,2) NOT NULL ,STORE_NO   VARCHAR2(10),ENT_BY VARCHAR2(15) NOT NULL,ENT_DT DATE NOT NULL ,EDT_BY VARCHAR2(15) NOT NULL,EDT_DT DATE NOT NULL,APP_BY VARCHAR2(20) NOT NULL ,APP_DT  DATE NOT NULL ,ISSUE_NO   NUMBER(3) NOT NULL ,PFLAG    NUMBER(1),PR_DT  DATE,TEST   VARCHAR2(50),PBASIS VARCHAR2(100),RATE_OK NUMBER(12,5),RATE_CD NUMBER(15,5),RATE_REJ NUMBER(12,5),SRNO NUMBER(5),PCESS NUMBER(12,5),DELV_ITEM VARCHAR2(20),NXTMTH NUMBER(12,5),TRANSPORTER VARCHAR2(45),CSCODE CHAR(10),EFFDATE DATE,ST38NO VARCHAR2(45),NXTMTH2 NUMBER(12,5),CURRENCY VARCHAR2(30),PEXCAMT NUMBER(10,2),PDISCAMT NUMBER(10,2),AMDTNO NUMBER(2),ORIGNALBR CHAR(2),GSM  NUMBER(15,2),CINAME VARCHAR2(135),LANDCOST  NUMBER(12,4),O_PRATE NUMBER(14,3),O_QTY NUMBER(12,3),CHL_REF VARCHAR2(20),OTHAC1 VARCHAR2(10),OTHAC2 VARCHAR2(10),OTHAC3 VARCHAR2(10),OTHAMT1 NUMBER(11,3),OTHAMT2 NUMBER(11,3),OTHAMT3 NUMBER(11,3),ST31NO VARCHAR2(45),D18NO VARCHAR2(20),TDISC_AMT NUMBER(12,4),CSCODE1 VARCHAR2(10),BILLCODE VARCHAR2(10),KINDATTN VARCHAR2(50),PREFSOURCE VARCHAR2(100),POPREFIX   VARCHAR2(20),RATE_DIFF  VARCHAR2(100),RATE_COMM   NUMBER(10,2),SPLRMK VARCHAR2(100),PDAYS NUMBER(4),EMAIL_STATUS VARCHAR2(1),CHK_BY VARCHAR2(15),CHK_DT DATE,VALIDUPTO DATE,ED_SERV CHAR(10),ATCH1 VARCHAR2(100),PDISCAMT2 NUMBER(12,2),TXB_FRT NUMBER(12,2),ATCH2 VARCHAR2(100),ATCH3 VARCHAR2(100),PO_TOLR NUMBER(10,2))");

        mhd = fgen.chk_RsysUpd("PRFQ101");
        if (mhd == "0" || mhd == "")
        {
            //cow
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "PRFQ101", "DEV_A");

            ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15600", 2, "RFQ Module", 3, "-", "-", "Y", "fin15_e7", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15601", 3, "Vendor Quotation Entry", 3, "../tej-base/om_po_entry.aspx", "-", "-", "fin15_e7", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15603", 3, "Terms and Conditions", 3, "../tej-base/om_terms_cond.aspx", "Invoice, PO, SO, RFQ terms and condition", "-", "fin15_e7", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15605", 3, "Vendor Quotation Comparison", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e7", "fin15_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15607", 3, "Vendor Quotation Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e7", "fin15_a1", "-", "fa-edit", "N", "N");
        }
    }

    public void Icon_Leave_Req(string frm_qstr, string frm_cocd)
    {
        // Leave Request Module
        // ------------------------------------------------------------------

        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_LEVREQ'", "tname");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_LEVREQ(branchcd char(2),type char(2),LRQNO char(6),LRQDT date,Empcode char(10) default '-',Lreason1 varchar2(30) default '-',Lreason2 varchar2(30) default '-',Levfrom varchar2(10) default '-',Levupto varchar2(10) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',Resp_Shared CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',LV_TIME CHAR(10) default '-',RET_TIME CHAR(10) DEFAULT '-',TOT_DAYS NUMBER(6,2) DEFAULT 0 ,TIME_IN_HRS CHAR(10) DEFAULT '-',ent_by varchar2(15) default '-',ent_Dt date default sysdate,edt_by varchar2(15) default '-',edt_Dt date default sysdate,app_by varchar2(15) default '-',app_Dt date default sysdate)");

        ICO.add_icon(frm_qstr, "F80000", 1, "H.R.M Module", 3, "-", "-", "Y", "-", "fin80_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F82700", 2, "Online HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F81000", 2, "Leave Mgmt Module", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F81100", 3, "Leave Mgmt Activity", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F82701", 3, "My HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "fin82pp_e7", "fa-edit");
        ICO.add_icon(frm_qstr, "F81101", 4, "Leave Request", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F82703", 4, "My Leave Request", 3, "../tej-base/om_leave_req.aspx", "-", "-", "fin82_e7", "fin80_a1", "fin82pp_e7", "fa-edit");
        ICO.add_icon(frm_qstr, "F81104", 4, "Leave Request Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin80_e1", "fin80_a1", "fin81pp_e1", "fa-edit");
    }

    public void Icon_Loan_Req(string frm_qstr, string frm_cocd)
    {
        // Loan Request Module
        // ------------------------------------------------------------------
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_PAYLOAN'", "tname");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE wb_payloan(BRANCHCD CHAR(2) default '-',TYPE CHAR(2) default '-',VCHNUM CHAR(6) default '-',VCHDATE DATE,EMPCODE  CHAR(10) default 0,DRAMT   NUMBER(10,2) default 0,CRAMT   NUMBER(10,2) default 0,INSTAMT NUMBER(10,2) default 0,DEPTT   CHAR(15) default '-',GRADE   CHAR(2) default '-',CURRSAL NUMBER(10,2) default 0,REMARK  VARCHAR2(50) default '-',ENT_BY  VARCHAR2(20) default '-',ENT_DT  DATE,EDT_BY  VARCHAR2(20) default '-',EDT_DT  DATE,OS_AMT  NUMBER(12,2) default 0,CUR_LOAN VARCHAR2(10) default '-',INST_ST_DT DATE,app_by VARCHAR2(15) default '-',app_dt  Date,Rej_Remarks varchar2(150) default '-',DISBURSE VARCHAR2(1) default '-')");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "OS_AMT");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD OS_AMT NUMBER(12,2) DEFAULT 0");
        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "CUR_LOAN");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD CUR_LOAN VARCHAR2(10) DEFAULT '-'");
        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "PAYLOAN", "INST_ST_dT");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE PAYLOAN ADD INST_ST_dT DATE");

        ICO.add_icon(frm_qstr, "F80000", 1, "H.R.M Module", 3, "-", "-", "Y", "-", "fin80_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F82700", 2, "Online HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F81500", 2, "Loan Mgmt Module", 3, "-", "-", "Y", "fin80_e8", "fin80_a1", "-", "fa-edit");
        ICO.add_icon(frm_qstr, "F81505", 3, "Loan Mgmt Activity", 3, "-", "-", "Y", "fin80_e8", "fin80_a1", "fin81pp_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F82701", 3, "My HRM Module", 3, "-", "-", "Y", "fin82_e7", "fin80_a1", "fin82pp_e7", "fa-edit");
        ICO.add_icon(frm_qstr, "F81510", 4, "Loan Request", 3, "../tej-base/om_loan_req.aspx", "-", "-", "fin80_e8", "fin80_a1", "fin81pp_e1", "fa-edit");
        ICO.add_icon(frm_qstr, "F82711", 4, "My Loan Request", 3, "../tej-base/om_loan_req.aspx", "-", "-", "fin82_e7", "fin80_a1", "fin82pp_e7", "fa-edit");
        ICO.add_icon(frm_qstr, "F81511", 4, "Loan Request Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin80_e8", "fin80_a1", "fin81pp_e1", "fa-edit");
    }

    public void IconCastingCost(string frm_qstr, string frm_cocd)
    {
        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_CACOST'", "tname");
        if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_CACOST( branchcd varchar2(2),type varchar2(4),vchnum varchar2(6),vchdate date,acode varchar2(6),icode varchar2(10),invno varchar2(6),invdate date,material varchar2(50) default '-',length number(20,3) default 0,width number(20,3) default 0,height number(20,3) default 0,cast number(20,3) default 0,cast_mould number(20,3) default 0,bunch number(20,3) default 0,actual number(20,3) default 0,pattern number(20,3) default 0,rej number(20,3) default 0,net_eff number(20,3) default 0,mixer number(20,3) default 0,moulding_rate number(20,3) default 0,labour number(20,3) default 0,maint number(20,3) default 0,fettling number(20,3) default 0,interest number(20,3) default 0,depr number(20,3) default 0,others number(20,3) default 0,stotal number(20,3) default 0,gtotal number(20,3) default 0,cast_rt number(20,3) default 0,electricity number(20,3) default 0,auxulary number(20,3) default 0,melting number(20,3) default 0,power number(20,3) default 0,core_wt number(20,3) default 0,core_rt number(20,3) default 0,core_rej number(20,3) default 0,core_cost number(20,3) default 0,fcons number(20,3) default 0,fcontri number(20,3) default 0,frate number(20,3) default 0,fwt number(20,3) default 0,fsi number(20,3) default 0,fmn number(20,3) default 0,fc number(20,3) default 0,fmoly number(20,3) default 0,pcons number(20,3) default 0,pcontri number(20,3) default 0,prate number(20,3) default 0,pwt number(20,3) default 0,psi number(20,3) default 0,pmn number(20,3) default 0,pc number(20,3) default 0,pmoly number(20,3) default 0,scons number(20,3) default 0,scontri number(20,3) default 0,srate number(20,3) default 0,swt number(20,3) default 0,ssi number(20,3) default 0,smn number(20,3) default 0,sc number(20,3) default 0,smoly number(20,3) default 0,ccons number(20,3) default 0,ccontri number(20,3) default 0,crate number(20,3) default 0,cwt number(20,3) default 0,csi number(20,3) default 0,cmn number(20,3) default 0,cc number(20,3) default 0,cmoly number(20,3) default 0,totcons number(20,3) default 0,totcontri number(20,3) default 0,totwt number(20,3) default 0,totsi number(20,3) default 0,totmn number(20,3) default 0,totc number(20,3) default 0,totmoly number(20,3) default 0,rsi number(20,3) default 0,rmn number(20,3) default 0,rc number(20,3) default 0,rmoly number(20,3) default 0,dsi number(20,3) default 0,dmn number(20,3) default 0,dc number(20,3) default 0,dmoly number(20,3) default 0,met_contri number(20,3) default 0,fesi_rec number(20,3) default 0,fesi_req number(20,3) default 0,fesi_rt number(20,3) default 0,fesi_cost number(20,3) default 0,femn_rec number(20,3) default 0,femn_req number(20,3) default 0,femn_rt number(20,3) default 0,femn_cost number(20,3) default 0,csc_rec number(20,3) default 0,csc_req number(20,3) default 0,csc_rt number(20,3) default 0,csc_cost number(20,3) default 0,moly_rec number(20,3) default 0,moly_req number(20,3) default 0,moly_rt number(20,3) default 0,moly_cost number(20,3) default 0,fesimg_rec number(20,3) default 0,fesimg_req number(20,3) default 0,fesimg_rt number(20,3) default 0,fesimg_cost number(20,3) default 0,ferro_tot number(20,3) default 0,meta_tot number(20,3) default 0,stage_wt number(20,3) default 0,melting_loss number(20,3) default 0,melting_stage_wt number(20,3) default 0,mas_alloy1 number(20,3) default 0,mas_alloy2 number(20,3) default 0,mas_alloy3 number(20,3) default 0,mas_alloy_wt number(20,3) default 0,innoculation1 number(20,3) default 0,innoculation2 number(20,3) default 0,innoculation3 number(20,3) default 0,innoculation_wt number(20,3) default 0,yield_ret number(20,3) default 0,tot_metallic_rt number(20,3) default 0,profit1 number(20,3) default 0,profit2 number(20,3) default 0,over_head1 number(20,3) default 0,over_head2 number(20,3) default 0,tot_cast_rt_oh number(20,3) default 0,tot_cast_rt number(20,3) default 0,trans_cost number(20,3) default 0,tool number(20,3) default 0,tot_mach_cost number(20,3) default 0,packing number(20,3) default 0,heat number(20,3) default 0,final number(20,3) default 0,interest_per number(20,3) default 0,vendor number(20,3) default 0,ent_by varchar2(20),ent_dt date,edt_by varchar2(20),edt_dt date)");

        ICO.add_icon(frm_qstr, "F47319", 4, "Costing sheet Entry", 3, "../tej-base/om_ca_cost.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
        ICO.add_icon(frm_qstr, "F47324", 4, "Power & Conversion Master", 3, "../tej-base/om_ca_master.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e11pp", "fa-edit");
        ICO.add_icon(frm_qstr, "F47325", 4, "Metal Recovery Master", 3, "../tej-base/om_ca_master.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e11pp", "fa-edit");
        ICO.add_icon(frm_qstr, "F47326", 4, "Box Size Master", 3, "../tej-base/om_ca_master.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e11pp", "fa-edit");
        ICO.add_icon(frm_qstr, "F47327", 4, "Core Type Master", 3, "../tej-base/om_ca_master.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e11pp", "fa-edit");
    }

    public void IconRFQ_SO(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("SORFQ101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('SORFQ101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "SORFQ101", "DEV_A");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select tname from tab where tname='WB_SORFQ'", "tname");
            if (mhd == "0" || mhd == "") fgen.execute_cmd(frm_qstr, frm_cocd, "create table WB_SORFQ(BRANCHCD CHAR(2) ,TYPE CHAR(2),ORDNO CHAR(6),ORDDT DATE ,ACODE CHAR(10) NOT NULL,UNIT  VARCHAR2(10),OTCOST2 NUMBER(14,3),PDISC NUMBER(5,2),PEXC NUMBER(5,2),PTAX NUMBER(5,2),OTCOST3 NUMBER(12,2),PSIZE  VARCHAR2(15) NOT NULL,QTYORD NUMBER(12,3) NOT NULL,QTYSUPP  NUMBER(12,3) NOT NULL,QTYBAL NUMBER(14,4),PORDNO VARCHAR2(30),PORDDT DATE,INVNO VARCHAR2(10),INVDATE DATE,DELIVERY NUMBER(3),DEL_MTH  NUMBER(10,2) NOT NULL,DEL_WK  NUMBER(7) NOT NULL,DEL_DATE DATE NOT NULL ,DELV_TERM VARCHAR2(150),TERM VARCHAR2(1000),INST  VARCHAR2(1000) NOT NULL,REFDATE DATE NOT NULL,MODE_TPT VARCHAR2(50),TR_INSUR VARCHAR2(50),DESP_TO VARCHAR2(150),FREIGHT  VARCHAR2(50) NOT NULL,DOC_THR VARCHAR2(100),PACKING  VARCHAR2(45),PAYMENT  VARCHAR2(200) NOT NULL,BANK  VARCHAR2(50) NOT NULL,REMARK VARCHAR2(300),DESC_  VARCHAR2(400),STAX VARCHAR2(50),EXC VARCHAR2(50) NOT NULL,IOPR  VARCHAR2(35),PR_NO  VARCHAR2(6) NOT NULL,AMD_NO VARCHAR2(15),DEL_SCH  VARCHAR2(15),TAX  VARCHAR2(2) NOT NULL,ICODE CHAR(30) NOT NULL,WK1 NUMBER(15,2),WK2 NUMBER(10,2),WK3 NUMBER(10,2),WK4  NUMBER(10,2),VEND_WT  NUMBER(10,2),STORE_NO VARCHAR2(10),ENT_BY VARCHAR2(15),ENT_DT DATE,EDT_BY VARCHAR2(15),EDT_DT  DATE,APP_BY VARCHAR2(20),APP_DT  DATE,ISSUE_NO  NUMBER(3) NOT NULL,PFLAG NUMBER(1),PR_DT DATE,TEST VARCHAR2(50),PBASIS VARCHAR2(100),RATE_OK NUMBER(12,5),RATE_CD  NUMBER(15,5),RATE_REJ NUMBER(12,5),SRNO  NUMBER(5),PCESS NUMBER(12,5),DELV_ITEM VARCHAR2(20),NXTMTH  NUMBER(12,5),TRANSPORTER VARCHAR2(45),CSCODE CHAR(10),EFFDATE DATE,ST38NO VARCHAR2(45),NXTMTH2 NUMBER(12,5),CURRENCY  VARCHAR2(30),PEXCAMT NUMBER(10,2),PDISCAMT NUMBER(10,2),AMDTNO NUMBER(2),ORIGNALBR CHAR(2),GSM NUMBER(15,2),CINAME  VARCHAR2(135),IRATE NUMBER(12,4),OTCOST1 NUMBER(14,3),O_QTY NUMBER(12,3),CHL_REF VARCHAR2(20),OTHAC1 VARCHAR2(10),OTHAC2  VARCHAR2(10),OTHAC3 VARCHAR2(10),OTHAMT1 NUMBER(11,3),OTHAMT2 NUMBER(11,3),OTHAMT3  NUMBER(11,3),ST31NO VARCHAR2(45),D18NO VARCHAR2(20),TDISC_AMT NUMBER(12,4),CSCODE1 VARCHAR2(10),BILLCODE VARCHAR2(10),KINDATTN  VARCHAR2(50),PREFSOURCE VARCHAR2(100),POPREFIX VARCHAR2(20),RATE_DIFF VARCHAR2(100),RATE_COMM  NUMBER(10,2),SPLRMK VARCHAR2(100),PDAYS NUMBER(4),EMAIL_STATUS VARCHAR2(1),CHK_BY VARCHAR2(15),CHK_DT DATE,VALIDUPTO DATE,ED_SERV CHAR(10),ATCH1 VARCHAR2(100),PDISCAMT2 NUMBER(12,2),TXB_FRT NUMBER(12,2),ATCH2 VARCHAR2(150),ATCH3 VARCHAR2(170),PO_TOLR NUMBER(10,2))");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "PORDNO");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST ADD PORDNO VARCHAR2(30)");

            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "PORDDT");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST ADD PORDDT DATE");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "PBASIS");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST ADD PBASIS VARCHAR2(30)");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "TEST");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST ADD TEST VARCHAR2(10)");
            mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "WB_CACOST", "APP_BY");
            if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_CACOST ADD APP_BY VARCHAR2(30)");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47300", 2, "RFQ (Sales) Management", 3, "-", "-", "Y", "fin47_e10", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47302", 3, "RFQ (Sales)Activity", 3, "-", "-", "Y", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47305", 3, "RFQ (Sales) Masters", 3, "-", "-", "Y", "fin47_e10", "fin45_a1", "fin47_e11pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47307", 3, "RFQ (Sales) Reports", 3, "-", "-", "Y", "fin47_e10", "fin45_a1", "fin47_e12pp", "fa-edit");

            ICO.add_icon(frm_qstr, "F47310", 4, "New RFQ(Sales) Entry", 3, "../tej-base/om_enq_ent.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47313", 4, "Engg Change Note Entry", 3, "../tej-base/om_enq_ent.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47315", 4, "RFQ (Sales) Foundry Response", 3, "../tej-base/om_rfq_ResFound.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47317", 4, "RFQ (Sales) Machine Response", 3, "../tej-base/om_rfq_mcshop.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47321", 4, "Quotation Entry", 3, "../tej-base/om_final_qtn.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47322", 4, "Quotation Status", 3, "../tej-base/om_view_smktg.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F47320", 4, "Close Enquiry", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47323", 4, "Quotation Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e10", "fin45_a1", "fin47_e10pp", "fa-edit");
        }

    }
    public void iconApprovals(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("APPR101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('APPR101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "APPR101", "DEV_A");

            ICO.add_icon(frm_qstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F15160", 2, "Purch. Check/Approvals", 3, "-", "-", "Y", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15161", 3, "Purchase Request Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15162", 3, "Purchase Request Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15165", 3, "Purchase Order Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15166", 3, "Purchase Order Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15171", 3, "Purchase Schedule Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15176", 3, "Price List Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F15200", 2, "Purchase Masters", 3, "-", "-", "Y", "fin15_e6", "fin15_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F15210", 3, "P.R. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "N");
            ICO.add_icon(frm_qstr, "F15211", 3, "P.O. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "N");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47000", 2, "Domestic Sales Orders", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F47121", 3, "Dom.Sales Approvals", 3, "-", "-", "Y", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47126", 4, "Check S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47127", 4, "Approve S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit", "N", "Y");
            ICO.add_icon(frm_qstr, "F47128", 4, "Sales Schedule Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e2pp", "fa-edit");

            ICO.add_icon(frm_qstr, "F47161", 3, "Dom.Order Masters", 3, "-", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit");
            ICO.add_icon(frm_qstr, "F47162", 4, "S.O.Closure (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e1", "fin45_a1", "fin47_e6pp", "fa-edit");
        }
    }

    public void iconDrawingModule(string frm_qstr, string frm_cocd)
    {
        string mhd = "";
        mhd = fgen.chk_RsysUpd("DRW101");
        if (mhd == "0" || mhd == "")
        {
            //fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('DRW101') ");
            fgen.add_RsysUpd(frm_qstr, frm_cocd, "DRW101", "DEV_A");

            ICO.add_icon(frm_qstr, "F45000", 1, "CRM & Orders Management", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55050", 2, "Drawing / Artwork Management", 3, "-", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55160", 3, "Drawing / Artwork Entry", 3, "../tej-base/draw.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55160A", 3, "Drawing / Artwork Approval", 3, "../tej-base/om_appr.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55160R", 3, "Drawing / Artwork Request", 3, "../tej-base/om_req_frm.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            //ICO.add_icon(frm_qstr, "F55163", 3, "Drawing Issue Entry", 3, "../tej-base/drawissue.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55164", 3, "Drawing / Artwork Issue", 3, "../tej-base/frmDrawIss.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55165", 3, "Drawing / Artwork Information Library", 3, "../tej-base/rfqapp.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55166", 3, "Drawing / Artwork Preview", 3, "../tej-base/rfqapp.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");
            ICO.add_icon(frm_qstr, "F55168", 3, "Drawing / Artwork Preview Dashboard", 3, "../tej-base/drawPrevDash.aspx", "-", "Y", "fin45_d1", "fin45_a1", "-", "fa-edit");

            ICO.add_icon(frm_qstr, "F55250", 3, "Drawing / Artwork Master Files", 3, "-", "-", "Y", "fin45_d1", "fin45_a1", "fin45d1_d1", "fa-edit");
            ICO.add_icon(frm_qstr, "F55252", 4, "Drawing / Artwork Design Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin45_d1", "fin45_a1", "fin45d1_d1", "fa-edit");
            ICO.add_icon(frm_qstr, "F55254", 4, "Drawing / Artwork Type Master", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin45_d1", "fin45_a1", "fin45d1_d1", "fa-edit");

            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='WB_DRAWREC'", "TNAME");
            string SQuery = "";
            if (mhd == "0" || mhd == "")
            {
                SQuery = "CREATE TABLE WB_DRAWREC(BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE,ACODE CHAR(10),ICODE CHAR(10),SRNO NUMBER(4),COL1 VARCHAR2(100),COL2 VARCHAR(50),COL3 VARCHAR2(50),COL4 VARCHAR2(50),COL5 VARCHAR2(100),COL6 VARCHAR2(50),COL7 VARCHAR2(50),COL8 VARCHAR2(50),COL9 VARCHAR2(50),COL10 VARCHAR2(50),dno VARCHAR2(50),rno VARCHAR2(50),TNO VARCHAR2(50),ENT_BY VARCHAR2(20),ENT_DT DATE,REMARKS VARCHAR2(220),DOCDATE DATE,T1 VARCHAR2(60),T2 VARCHAR2(15),T3 VARCHAR2(50),T4 VARCHAR2(15),T5 VARCHAR2(20),T6 VARCHAR2(25),T7 VARCHAR2(30),T8 VARCHAR2(15),T9 VARCHAR2(15),T10 VARCHAR2(50),T11 VARCHAR2(50),T12 VARCHAR2(50),T13 VARCHAR2(50),T14 VARCHAR2(50),T15 VARCHAR2(50),T16 VARCHAR2(50),T17 VARCHAR2(50),T18 VARCHAR2(50),T19 VARCHAR2(50),T20 VARCHAR2(50),NUM1 NUMBER(20,6),NUM2 NUMBER(12,3),NUM3 NUMBER(12,3),NUM4 NUMBER(10,3),NUM5 NUMBER(12,3),NUM6 NUMBER(12,3),NUM7 NUMBER(10,3),NUM8 NUMBER(10,3),NUM9 NUMBER(12,3),NUM10 NUMBER(12,3),EDT_BY VARCHAR2(20),EDT_DT DATE,INVNO VARCHAR2(10),INVDATE DATE,CHK_BY VARCHAR2(15),CHK_DT DATE,NUM11 NUMBER(10,2),NUM12 NUMBER(10,2),NUM13 NUMBER(10,2),NUM14 NUMBER(10,2),NUM15 NUMBER(10,2),issue_to VARCHAR2(50),issue_by VARCHAR2(50),issue_date DATE,finvno VARCHAR2(100),RDATE DATE,rcpt_date DATE,DTYPE VARCHAR2(100),FILEPATH VARCHAR2(100),FILENAME VARCHAR2(100))";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TNAME FROM TAB WHERE TNAME='OM_DRWG_MAKE'", "TNAME");
            if (mhd == "0" || mhd == "")
            {
                SQuery = "create table OM_DRWG_MAKE (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR2(6),VCHDATE DATE,SRNO NUMBER(5),MRRNUM VARCHAR2(6),MRRDATE DATE,ACODE VARCHAR2(6),ICODE VARCHAR2(10),UNIT VARCHAR2(2),IRATE NUMBER(14,2),USERCODE VARCHAR2(20),ISSUESTARTDT DATE,ISSUEENDDT DATE,STARTTIME VARCHAR2(30),ENDTIME VARCHAR2(30),ISSUETIME CHAR(30),REMARKS VARCHAR2(100),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE)";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
        }

        ICO.add_icon(frm_qstr, "F79109", 4, "Drawing / Artwork Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin79_e1", "fin45_a1", "fin79pp_e1", "fa-edit");

        ICO.add_icon(frm_qstr, "F55256", 4, "Customer Master(Drawing / Artwork)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin45_d1", "fin45_a1", "fin45d1_d1", "fa-edit");
        ICO.add_icon(frm_qstr, "F55257", 4, "Product Master(Drawing / Artwork)", 3, "../tej-base/om_tgpop_mst.aspx", "-", "Y", "fin45_d1", "fin45_a1", "fin45d1_d1", "fa-edit");

        if (fgen.check_filed_name(frm_qstr, frm_cocd, "WB_DRAWREC", "APP_BY") == "0")
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_DRAWREC ADD APP_BY VARCHAR(20)");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE WB_DRAWREC ADD APP_DT DATE");
        }
    }
}