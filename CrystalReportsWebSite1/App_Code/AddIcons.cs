using System;


public class AddIcons
{
    fgenDB fgen = new fgenDB();
    Create_Icons ICO = new Create_Icons();
    public void add(string frm_qstr, string frm_cocd)
    {
        ICO.Cls_comp_code = frm_cocd;
        switch (frm_cocd)
        {
            
            case "TEST":
                string mhd = "";
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select idno from FIN_RSYS_UPD where trim(idno)='TEST001'", "idno");                
                if (mhd == "0" || mhd == "")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "INSERT INTO FIN_rSYS_UPD (IDNO) VALUES ('TEST001') ");
                    // ------------------------------------------------------------------
                    //important icons for new developments
                    // ------------------------------------------------------------------

                    ICO.add_icon(frm_qstr, "F39131T", 3, "mdboard", 3, "../tej-base/mdboard.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F39131U", 3, "User ID Card", 3, "../tej-base/rpt.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                    

                    ICO.add_icon(frm_qstr, "F35106a", 3, "Job Order Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F35108", 3, "Job Card Monitoring", 3, "../tej-base/om_dbd_mgrid.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F35108g", 3, "Job Card Monitoring(Graph)", 3, "../tej-base/om_dbd_mgrph.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "P17006A", 3, "Dashboard (Multi-Module)", 3, "../tej-base/om_dboard2.aspx", "-", "-", "fin05_e9", "fin05_a1", "-", "fa-edit", "N", "Y");

                    //important icons for new developments


                    // ------------------------------------------------------------------
                    // Customer Support System Menus
                    // ------------------------------------------------------------------

                    ICO.add_icon(frm_qstr, "F60000", 1, "Customer Support System", 3, "-", "-", "Y", "-", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60100", 2, "CSS Activity", 3, "-", "-", "Y", "fin60_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60101", 3, "CSS Logging", 3, "../tej-base/om_css_log.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60106", 3, "CSS Assignment", 3, "../tej-base/om_css_asg.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60111", 3, "CSS Action", 3, "../tej-base/om_css_act.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F60116", 2, "CSS Reports", 3, "-", "-", "Y", "fin60_e2", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60121", 3, "CSS Log List", 3, "../tej-base/rpt_DevA.aspx", "-", "Y", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60126", 3, "CSS Assignment List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60131", 3, "CSS Actions List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "Y");

                    ICO.add_icon(frm_qstr, "F60140", 2, "CSS Dashboards", 3, "-", "-", "Y", "fin60_e3", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60141", 3, "CSS Log Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin60_e3", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60146", 3, "CSS Assign Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin60_e3", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60151", 3, "CSS Action Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin60_e3", "fin60_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F60156", 2, "CSS Masters", 3, "-", "-", "Y", "fin60_e4", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60161", 3, "CSS Status Master", 3, "../tej-base/om_Typ_mst.aspx", "-", "-", "fin60_e4", "fin60_a1", "-", "fa-edit");

                    ICO.add_icon(frm_qstr, "F60171", 2, "CSS Clearance", 3, "-", "-", "Y", "fin60_e5", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60176", 3, "CSS Clearance (Client)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin60_e5", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60181", 3, "CSS Clearance (Asgnor)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin60_e5", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F60186", 3, "Action Clearance (Asgnor)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin60_e5", "fin60_a1", "-", "fa-edit");

                    // ------------------------------------------------------------------
                    // CSS reports
                    // ------------------------------------------------------------------

                    ICO.add_icon(frm_qstr, "F60150", 3, "More Reports(CSS)", 3, "-", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "N");
                    ICO.add_icon(frm_qstr, "F60132", 4, "CSS Status Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60133", 4, "CSS Pending Assignment", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60134", 4, "CSS Pending Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60135", 4, "CSS Pending Closure", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60136", 4, "CSS Action Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60137", 4, "CSS Assignee Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60138", 4, "CSS 31 Day Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60139", 4, "CSS 12 Mth Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60142", 4, "CSS 31 Day Team Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");

                    ICO.add_icon(frm_qstr, "F60153", 4, "CSS Count,Time Analysis", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F60154", 4, "CSS Pending Team Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");

                    //--------------------------------
                    //ALF Monitoring System
                    //--------------------------------
                    ICO.add_icon(frm_qstr, "F92000", 2, "Tejaxo ALF", 3, "-", "-", "Y", "fin92_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F92100", 3, "ALF Planning", 3, "-", "-", "Y", "fin92_e1", "fin60_a1", "fin92pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F92101", 4, "Record ALF plan", 3, "../tej-base/om_alf_plan.aspx", "-", "-", "fin92_e1", "fin60_a1", "fin92pp_e1", "fa-edit");

                    ICO.add_icon(frm_qstr, "F92116", 3, "ALF Plan Reports", 3, "-", "-", "Y", "fin92_e1", "fin60_a1", "fin92pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F92121", 4, "ALF Plan List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin92_e1", "fin60_a1", "fin92pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F92126", 4, "ALF Plan Vs Actual", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin92_e1", "fin60_a1", "fin92pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F92131", 4, "ALF Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin92_e1", "fin60_a1", "fin92pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F92127", 4, "ALF 31 Day Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin92_e1", "fin60_a1", "fin92pp_e2", "fa-edit", "N", "Y");

                    // ------------------------------------------------------------------
                    // Customer O/s Monitoring
                    // ------------------------------------------------------------------
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

                    // ------------------------------------------------------------------
                    // Software Training Guide
                    // ------------------------------------------------------------------
                    ICO.add_icon(frm_qstr, "F94000", 2, "Tejaxo STL", 3, "-", "-", "Y", "fin94_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F94100", 3, "STL Activity", 3, "-", "-", "Y", "fin94_e1", "fin60_a1", "fin94pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F94101", 4, "Record STL", 3, "../tej-base/om_stl_log.aspx", "-", "-", "fin94_e1", "fin60_a1", "fin94pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F94106", 4, "Approve STL", 3, "../tej-base/om_appr.aspx", "-", "-", "fin94_e1", "fin60_a1", "fin94pp_e1", "fa-edit");

                    ICO.add_icon(frm_qstr, "F94116", 3, "STL Reports", 3, "-", "-", "Y", "fin94_e1", "fin60_a1", "fin94pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F94121", 4, "Module Wise STL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin94_e1", "fin60_a1", "fin94pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F94126", 4, "Vertical Wise STL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin94_e1", "fin60_a1", "fin94pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F94131", 4, "Customer Wise STL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin94_e1", "fin60_a1", "fin94pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F94132", 4, "STL Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin94_e1", "fin60_a1", "fin94pp_e2", "fa-edit");

                    //------------------------------------------------------------------
                    //ERP Implementation Path
                    //------------------------------------------------------------------

                    //ICO.add_icon(frm_qstr, "F95100", 2, "ERP Implementation Goals", 3, "-", "-", "Y", "fin95_e1", "fin60_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F95101", 3, "ERP Module List", 3, "../tej-base/om_Typ_mst.aspx", "-", "-", "fin95_e1", "fin60_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F95106", 3, "ERP Mile Stones", 3, "../tej-base/om_Typ_mst.aspx", "-", "-", "fin95_e1", "fin60_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F95111", 3, "ERP Delivery Plan", 3, "../tej-base/om_erp_plan.aspx", "-", "-", "fin95_e1", "fin60_a1", "-", "fa-edit");

                    //ICO.add_icon(frm_qstr, "F95126", 2, "ERP Implementation Record", 3, "-", "-", "Y", "fin95_e1", "fin60_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F95131", 3, "ERP Delv. Record", 3, "../tej-base/om_erp_delv.aspx", "-", "-", "fin95_e1", "fin60_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F95132", 3, "ERP Delv. Approval(HO)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin95_e1", "fin60_a1", "-", "fa-edit");
                    //ICO.add_icon(frm_qstr, "F95136", 3, "ERP Delv. Approval(Client)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin95_e1", "fin60_a1", "-", "fa-edit");

                    // ------------------------------------------------------------------
                    // Developed Software Library
                    // ------------------------------------------------------------------

                    ICO.add_icon(frm_qstr, "F96000", 2, "Tejaxo DSL", 3, "-", "-", "Y", "fin96_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F96100", 3, "DSL Activity", 3, "-", "-", "Y", "fin96_e1", "fin60_a1", "fin96pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F96101", 4, "Record DSL", 3, "../tej-base/om_dsl_log.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F96106", 4, "Approve DSL", 3, "../tej-base/om_appr.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F96107", 4, "DSL Library", 3, "../tej-base/infolib.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e1", "fa-edit");

                    ICO.add_icon(frm_qstr, "F96116", 3, "DSL Reports", 3, "-", "-", "Y", "fin96_e1", "fin60_a1", "fin96pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F96121", 4, "Developer Wise DSL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F96126", 4, "Vertical Wise DSL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F96131", 4, "Customer Wise DSL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F96132", 4, "DSL Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin96_e1", "fin60_a1", "fin96pp_e2", "fa-edit");

                    // ------------------------------------------------------------------
                    // Master Equipment List , company asset 
                    // ------------------------------------------------------------------

                    ICO.add_icon(frm_qstr, "F97000", 2, "Tejaxo CAM", 3, "-", "-", "Y", "fin97_e1", "fin60_a1", "-", "fa-edit");
                    ICO.add_icon(frm_qstr, "F97100", 3, "CAM Activity", 3, "-", "-", "Y", "fin97_e1", "fin60_a1", "fin97pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F97101", 4, "Record CAM", 3, "../tej-base/om_CAM_log.aspx", "-", "-", "fin97_e1", "fin60_a1", "fin97pp_e1", "fa-edit");
                    ICO.add_icon(frm_qstr, "F97106", 4, "Approve CAM", 3, "../tej-base/om_appr.aspx", "-", "-", "fin97_e1", "fin60_a1", "fin97pp_e1", "fa-edit");

                    ICO.add_icon(frm_qstr, "F97116", 3, "CAM Reports", 3, "-", "-", "Y", "fin97_e1", "fin60_a1", "fin97pp_e2", "fa-edit");
                    ICO.add_icon(frm_qstr, "F97121", 4, "CAM Log List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin97_e1", "fin60_a1", "fin97pp_e2", "fa-edit", "N", "Y");
                    ICO.add_icon(frm_qstr, "F97132", 4, "CAM Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin97_e1", "fin60_a1", "fin97pp_e2", "fa-edit");


                    if (frm_cocd == "TEST")
                    {
                        //ICO.add_icon(frm_qstr, "P19005", 1, "PTS Admin", 3, "-", "-", "Y", "finpts_a", "finptsadm", "-", "fa-edit");
                        //ICO.add_icon(frm_qstr, "P19005A", 2, "User Rights", 3, "../tej-base/om_dboard.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");
                        //ICO.add_icon(frm_qstr, "M20016", 2, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");

                        //ICO.add_icon(frm_qstr, "F60102", 2, "CSS Logging2", 3, "../tej-base/om_css_log2.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");
                        //ICO.add_icon(frm_qstr, "F60137A", 2, "CSS Assignee Status1", 3, "../tej-base/rpt_DevA.aspx", "-", "Y", "fin60_e2", "fin60_a2", "-", "fa-edit", "Y", "Y");                          
                    }
                }

                // ------------------------------------------------------------------
                // Tejaxo Delivery Mgmt
                // ------------------------------------------------------------------

                
                ICO.add_icon(frm_qstr, "F95000", 2, "Tejaxo Project Delivery Mgmt", 3, "-", "-", "Y", "fin95_e1", "fin60_a1", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "F95100", 3, "Target Setting Activity", 3, "-", "-", "Y", "fin95_e1", "fin60_a1", "fin95pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F95101", 4, "Project Delivery Setup", 3, "../tej-base/om_Proj_setup.aspx", "-", "-", "fin95_e1", "fin60_a1", "fin95pp_e1", "fa-edit");
                ICO.add_icon(frm_qstr, "F95106", 4, "Project Delivery Updation", 3, "../tej-base/om_Proj_log.aspx", "-", "-", "fin95_e1", "fin60_a1", "fin95pp_e1", "fa-edit");

                ICO.add_icon(frm_qstr, "F95116", 3, "Project Delivery Reports", 3, "-", "-", "Y", "fin95_e1", "fin60_a1", "fin95pp_e2", "fa-edit");
                ICO.add_icon(frm_qstr, "F95121", 4, "Project Delivery Completion", 3, "../tej-base/om_view_css.aspx", "-", "-", "fin95_e1", "fin60_a1", "fin95pp_e2", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F95132", 4, "Project Delivery Pendency", 3, "../tej-base/om_view_css.aspx", "-", "-", "fin95_e1", "fin60_a1", "fin95pp_e2", "fa-edit", "N", "N");
                ICO.add_icon(frm_qstr, "F95133", 4, "Project Delivery Vs Actual", 3, "../tej-base/om_view_css.aspx", "-", "-", "fin95_e1", "fin60_a1", "fin95pp_e2", "fa-edit", "N", "N");

                break;
            case "AVON*":
                ICO.add_icon(frm_qstr, "700001", 1, "Engineering", 1, "-", "-", "-", "-", "matplan", "-", "fa-laptop");
                ICO.add_icon(frm_qstr, "700025", 2, "Box Costing", 1, "frmBoxCosting.aspx", "-", "-", "-", "matplan", "GO", "-");
                ICO.add_icon(frm_qstr, "700035", 2, "Party/CostSheet Master", 1, "frmBoxMaster.aspx", "-", "-", "-", "matplan", "GO", "-");

                ICO.add_icon(frm_qstr, "700001", 1, "Engineering", 1, "-", "-", "-", "-", "matplan", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "700025", 2, "Box Costing", 1, "frmBoxCosting.aspx", "-", "-", "-", "matplan", "GO", "fa-edit");
                ICO.add_icon(frm_qstr, "700035", 2, "Party/CostSheet Master", 1, "frmBoxMaster.aspx", "-", "-", "-", "matplan", "GO", "fa-edit");
                ICO.add_icon(frm_qstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
                ICO.add_icon(frm_qstr, "97010", 2, "User Rights", 1, "urights.aspx", "-", "-", "-", "SYSAD", "SYSADM", "fa-edit");
                ICO.add_icon(frm_qstr, "99000", 1, "Reports", 1, "-", "-", "-", "-", "FRPT", "-", "fa-files-o");
                ICO.add_icon(frm_qstr, "99001", 2, "Tasks Status", 1, "rpt.aspx", "-", "-", "-", "FRPT", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "99001A", 2, "Daily Prod", 1, "rpt.aspx", "-", "-", "-", "FRPT", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "99001B", 2, "Tasks Status Dept Wise", 1, "rpt.aspx", "-", "-", "-", "FRPT", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "99700", 1, "Special Option", 1, "-", "-", "-", "-", "DAK", "-", "fa-book");
                ICO.add_icon(frm_qstr, "99701", 2, "Assign Tasks", 1, "dak.aspx", "-", "-", "-", "DAK", "DAK", "fa-edit");
                ICO.add_icon(frm_qstr, "99702", 2, "Approve Tasks", 1, "appr.aspx", "-", "-", "-", "DAK", "DAK", "fa-edit");
                ICO.add_icon(frm_qstr, "99800", 1, "Masters", 1, "-", "-", "-", "-", "MST", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "99801", 2, "Dept Master", 1, "dpt_mst.aspx", "-", "-", "-", "MST", "MST", "fa-edit");
                ICO.add_icon(frm_qstr, "99802", 2, "Person Master", 1, "prsn_mst.aspx", "-", "-", "-", "MST", "MST", "fa-edit");
                break;
            case "MLGA":
            case "MSES":
            case "TEST**":
                //ICO.add_icon(frm_qstr, "P11000", 1, "PTS System", 3, "-", "-", "Y", "finpts_e", "finpts", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P11001", 1, "Project Tracking", 3, "-", "-", "Y", "finpts_e", "finptsa", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P11001A", 2, "Project Creation", 3, "../tej-base/om_task_mast.aspx", "-", "-", "finpts_e", "finptsa", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P11001C", 2, "Task Assignment", 3, "../tej-base/om_task_asgn.aspx", "-", "-", "finpts_e", "finptsa", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P11001E", 2, "Lead Managerial Hrs", 3, "../tej-base/frmLeadManage.aspx", "-", "-", "finpts_e", "finptsa", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P12001", 1, "Task Tracking", 3, "-", "-", "Y", "finpts_t", "finptstr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P12001A", 2, "Time Tracking", 3, "../tej-base/om_task_Updt.aspx", "-", "-", "finpts_t", "finptstr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P12001C", 2, "Task List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_t", "finptstr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P12001E", 2, "Leave Update", 3, "../tej-base/frmLeaveUpd.aspx", "-", "-", "finpts_t", "finptstr", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P13003", 1, "PTS Masters", 3, "-", "-", "Y", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003A", 2, "Business Units", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003B", 2, "Activity Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003C", 2, "Task Type Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003D", 2, "Designation Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003D1", 2, "Customer Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003E", 2, "Software Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003F", 2, "Documentation Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003F1", 2, "Documentation Status Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003G", 2, "Down Time Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003I", 2, "Status Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "P13003K", 2, "Assignor Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "P13003M", 2, "Assignee Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                //ICO.add_icon(frm_qstr, "P13003M1", 2, "Offload Assignee Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003N", 2, "Milestone Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003N1", 2, "Milestone Status Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003O", 2, "Department Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P13003Q", 2, "Proj.Category Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P15005", 1, "PTS Reports", 3, "-", "-", "Y", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005A", 2, "Man Hour Utilization", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005B", 2, "Parameter Wise Summary Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005C", 2, "Billed Hours Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005D", 2, "Project Documentation Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005E", 2, "Resource Efficiency", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005G", 2, "Down Time Analysis", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005I", 2, "Project Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005K", 2, "Budget Vs Actual Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005M", 2, "Budget/Actual/Billed Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005M1", 2, "Milestone vs Actual Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005O", 2, "Productivity", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005Q", 2, "Profitability", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005S", 2, "Performance", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005T", 2, "Pending Activity Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005U", 2, "Estimate Vs Actual Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005W", 2, "Down Time Reason Analysis", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005X", 2, "Assignment Status Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005Y", 2, "Project budgeted/Actual Revenue", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P15005Z", 2, "Report Builder", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P17005", 1, "PTS MIS", 3, "-", "-", "Y", "finpts_s", "finptsmi", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P17005A", 2, "Dash Board (Client)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P17005C", 2, "Dash Board (Overall)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P17005E", 2, "Graph : Utilization", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P17005G", 2, "Graph : Performance", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P17005I", 2, "Graph : DownTime", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");

                ICO.add_icon(frm_qstr, "P18005", 1, "PTS Logs", 3, "-", "-", "Y", "finpts_l", "finptsml", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P18005A", 2, "Project Log Book", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_l", "finptsml", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P18005C", 2, "Task Assigned Log Book", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_l", "finptsml", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P18005E", 2, "Task Reported Log Book", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_l", "finptsml", "-", "fa-edit");


                ICO.add_icon(frm_qstr, "P19005", 1, "PTS Admin", 3, "-", "-", "Y", "finpts_a", "finptsadm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "P19005A", 2, "User Rights", 3, "../tej-base/urights.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "M20016", 2, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");
                ICO.add_icon(frm_qstr, "M20028", 2, "UDFs Config", 3, "../tej-base/om_forms.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");


                //////ICO.add_icon(frm_qstr, "M20001", 1, "System Controls", 1, "-", "-", "-", "-", "Tejaxomain", "-", "fa-edit");
                //////ICO.add_icon(frm_qstr, "M20016", 2, "Form Configurations", 1, "../tej-base/om_forms.aspx", "-", "-", "-", "finsysmain", "-", "fa-edit");
                //////ICO.add_icon(frm_qstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");

                //              break;

                //   case "MLGA":
                ////ICO.add_icon(frm_qstr, "M20011", 2, "OMSO", 1, "../tej-base/om_so.aspx", "-", "-", "-", "finsysmain", "-", "fa-edit");

                ////ICO.add_icon(frm_qstr, "M20001", 1, "System Controls", 1, "-", "-", "-", "-", "finsysmain", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "M20016", 2, "Form Configurations", 1, "../tej-base/om_forms.aspx", "-", "-", "-", "finsysmain", "-", "fa-edit");

                ////ICO.add_icon(frm_qstr, "S11001", 1, "MPA Module", 1, "-", "-", "-", "-", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S11005", 2, "MPA Entry", 3, "-", "-", "Y", "finmpa_e", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S11005A", 3, "Record Efforts", 3, "../tej-base/om_effort_Rec.aspx", "-", "-", "finmpa_e", "finmpa", "-", "fa-edit");

                ////ICO.add_icon(frm_qstr, "S13008", 2, "MPA Masters", 3, "-", "-", "Y", "finmpa_m", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S13008A", 3, "Customer Master", 3, "../tej-base/om_types.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S13008B", 3, "Employee Master", 3, "../tej-base/om_types.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S13008C", 3, "Efforts Master", 3, "../tej-base/om_types.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S13008D", 3, "Cust. Effort Target", 3, "../tej-base/om_wrk_link.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");

                ////ICO.add_icon(frm_qstr, "S15115", 2, "MPA Reports", 3, "-", "-", "Y", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115A", 3, "Customer Effort Summary", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115B", 3, "Customer Employee Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115C", 3, "Employee Effort Summary", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115D", 3, "Customer Monthly Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115D1", 3, "All Master Form", 3, "../tej-base/allMaster.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115E", 3, "Customer Employee Monthly Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115F", 3, "Target Vs Actual Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115I", 3, "D/D Sale Report", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");

                ////ICO.add_icon(frm_qstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
                ////ICO.add_icon(frm_qstr, "S15115G", 3, "Client Dashboard (Client)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////ICO.add_icon(frm_qstr, "S15115H", 3, "Dashboard (MLG)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");

                //ICO.add_icon(frm_qstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
                //ICO.add_icon(frm_qstr, "97010", 2, "User Rights", 1, "../tej-base/urights.aspx", "-", "-", "-", "SYSAD", "SYSADM", "-");

                break;

        }
    }
}