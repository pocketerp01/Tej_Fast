using System;
using System.IO;
using System.Web;
using Fin_WFinConn_DLL;


    public static class fgenCO
    {
        static string sp_cd = "";
        public static string connStr { get; set; }
        static StreamReader srfgenCO;
        /// <summary>
        /// To get company code from mytns file, it checks mytns file in c:\
        /// if not found in c:\ then it goes and file in c:\TEJ_ERP
        /// </summary>
        /// <returns>Company Code</returns>
        ///  
        public static string GetCO_CD()
        {
            string path = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
            string path2 = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
            string str = "";

            if (File.Exists(path)) srfgenCO = new StreamReader(path);
            else if (File.Exists(path2)) srfgenCO = new StreamReader(path2);

            str = srfgenCO.ReadToEnd().Trim();
            if (str.Contains("\r")) str = str.Replace("\r", ",");
            if (str.Contains("\n")) str = str.Replace("\n", ",");
            str = str.Replace(",,", ",");

            string Co = str.Split(',')[0].ToUpper();

            if (str.Contains("3881") || str.Contains("4881") || str.Contains("5881") || str.Contains("6881") || str.Contains("7881"))
            { sp_cd = str.Split(',')[3]; }

            srfgenCO.Close();
            return Co;
        }
        public static string GetServerIP()
        {
            string path = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
            string path2 = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
            string str = "";

            if (File.Exists(path)) srfgenCO = new StreamReader(path);
            else if (File.Exists(path2)) srfgenCO = new StreamReader(path2);

            str = srfgenCO.ReadToEnd().Trim();

            str = str.Contains("\r") ? str.Replace("\r", ",") : "";
            str = str.Contains("\n") ? str.Replace("\n", ",") : "";
            str = str.Replace(",,", ",");

            string IP = str.Split(',')[1].ToUpper();

            sp_cd = (str.Contains("3881") || str.Contains("4881") || str.Contains("5881") || str.Contains("6881") || str.Contains("7881")) ? str.Split(',')[3] : "";

            srfgenCO.Close();
            return IP;
        }
        //public static string GetCO_CD()
        //{
        //    string path = @"C:\mytns.txt";
        //    string path2 = @"c:\TEJ_ERP\mytns.txt";
        //    string str = "";

        //    if (File.Exists(path)) srfgenCO = new StreamReader(path);
        //    else if (File.Exists(path2)) srfgenCO = new StreamReader(path2);

        //    str = srfgenCO.ReadToEnd().Trim();
        //    if (str.Contains("\r")) str = str.Replace("\r", ",");
        //    if (str.Contains("\n")) str = str.Replace("\n", ",");
        //    str = str.Replace(",,", ",");

        //    string Co = str.Split(',')[0].ToUpper();

        //    if (str.Contains("3881") || str.Contains("4881") || str.Contains("5881") || str.Contains("6881") || str.Contains("7881"))
        //    { sp_cd = str.Split(',')[3]; }

        //    srfgenCO.Close();
        //    return Co;
        //}
        //public static string GetServerIP()
        //{
        //    string path = @"C:\mytns.txt";
        //    string path2 = @"c:\TEJ_ERP\mytns.txt";
        //    string str = "";

        //    if (File.Exists(path)) srfgenCO = new StreamReader(path);
        //    else if (File.Exists(path2)) srfgenCO = new StreamReader(path2);

        //    str = srfgenCO.ReadToEnd().Trim();

        //    str = str.Contains("\r") ? str.Replace("\r", ",") : "";
        //    str = str.Contains("\n") ? str.Replace("\n", ",") : "";
        //    str = str.Replace(",,", ",");

        //    string IP = str.Split(',')[1].ToUpper();

        //    sp_cd = (str.Contains("3881") || str.Contains("4881") || str.Contains("5881") || str.Contains("6881") || str.Contains("7881")) ? str.Split(',')[3] : "";

        //    srfgenCO.Close();
        //    return IP;
        //}
        /// <summary>
        /// Making the connection String
        /// </summary>
        /// <param name="co_cd">The valued entered by User</param>
        /// <returns>Constr with connection string</returns>
        public static string Con2OLE(string co_cd)
        {
            try
            {
                //constr = "Data Source=(DESCRIPTION="
                //+ "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + ConnInfo.IP.Trim() + ")(PORT=1521)))"
                //+ "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + ConnInfo.srv + ")));"
                //+ "User Id= FIN" + co_cd + "; Password= " + ConnInfo.nPwd + ";";
            }
            catch { }
            return null;
        }
    /// <summary>
    /// Check Company Code, the entred company code is valid or not
    /// </summary>
    /// <param name="co_cd">The valued entered by User</param>
    /// <returns>Retruns Company Name</returns>
    public static string chk_co(string co_cd)
    {
        string fname = "";
        switch (co_cd.Trim())
        {
            case "DREW"://1-04/02/2021           
                fname = "DURGA ENGINEERING WORKS";
                break;
            case "PGTL"://1-04/02/2021           
                fname = "TEJAXO TEST LTD.";
                break;
            case "KRAM"://1-30/09/2021           
                fname = "KESHORAM MANUFACTURING PVT. LTD.";
                break;
            case "MASS"://1-16/09/2021           
                fname = "MAS TECHNOLOGY";
                break;
            case "MAST"://1-25/03/2021           
                fname = "MAS TECHNOLOGY SYTEM PVT.LTD.";
                break;
            case "DESH"://1-02/09/2021           
                fname = "RPRAHARI MULTIMEDIA PVT.LTD.";
                break;
            case "IIL"://1-11/02/2021           
                fname = "Al QADA CLAIMS RECOVERY SERVICES";
                break;
            case "TEJAXO"://1-11/02/2021           
                fname = "POCKETDRIVER PVT LTD";
                break;
            case "UMED"://1-26/12/2020            
                fname = "UMED DEMO INTERNATIONAL";
                break;
            case "SHRE"://1-26/12/2020            
                fname = "SHREE NARAYAN AGENCIES";
                break;
            case "KRSM"://1-28/12/2020            
                fname = "KRS MULTILUB PVT.LTD.";
                break;
            case "DISP"://1-28/12/2020            
                fname = "TEJAXO TECHNOLOGIES";
                break;
            case "SRPF"://1-28/12/2020            
                fname = "TEJAXO TECHNOLOGIES";
                break;
            case "SPAC"://1-28/12/2020            
                fname = "SKYPACK INDIA";
                break;
            case "SOFGEN"://26/01/2022            
                fname = "SOFGEN INFOTECH PVT.LTD.";
                break;
            case "SGEN"://26/01/2022            
                fname = "SOFGEN INFOTECH PVT.LTD.";
                break;
            case "DURG"://26/01/2022            
                fname = "DURGA ENGINEERING WORKS.";
                break;
            default:
                fname = "XXXX";
                break;
        }
        if (sp_cd.Trim().Length > 0)
        {
            if (sp_cd == "1112") fname = "DEMO AUTO COMPONENTS PVT. LTD";
            else if (sp_cd == "2112") fname = "DEMO PACKAGING PVT. LTD";
            else if (sp_cd == "3112") fname = "DEMO PHARMACEUITCAL LTD";
            else if (sp_cd == "4112") fname = "DEMO INDIA LTD";
            else if (sp_cd == "5112") fname = "DEMO PLASTICS LTD";
            else if (sp_cd == "6112") fname = "DEMO FORGINGS LTD";
            else if (sp_cd == "7112") fname = "DEMO LABELS LTD";
        }
        return fname;
    }
        public static void chk_grp(string co_cd, out string compgroup)
        {
            switch (co_cd.Trim())
            {
                case "DREW":          
                case "KRAM":
                case "DESH":     
                case "IIL":    
                case "PGTL":    
                case "TEJAXO":
                case "UMED":        
                case "SHRE":        
                case "KRSM":        
                case "DISP":        
                case "SRPF":        
                case "MASS":       
                case "MAST":       
                case "SPAC": 
                    compgroup = "T";
                    break;
                case "SOFGEN":
                case "SGEN":
                    compgroup = "S";
                    break;
                default:
                    compgroup = "0";
                    break;
            }
        }
        public static string chk_Akito(string username, string firmname)
        {
            string fname = firmname;
            switch (username)
            {
                case "tej-COMP":
                    fname = "AKITO KOWA AUTO COMPONENTS PVT. LTD";
                    break;
                case "tej-PHARMA":
                    fname = "AKITO KOWA PHARMACEUITCAL LTD";
                    break;
                case "tej-PACK":
                    fname = "AKITO KOWA PACKAGING PVT. LTD";
                    break;
                case "tej-INDIA":
                    fname = "AKITO KOWA INDIA LTD";
                    break;
                case "tej-PLAST":
                    fname = "AKITO KOWA PLASTICS LTD";
                    break;
                case "tej-FORG":
                    fname = "AKITO KOWA FORGINGS LTD";
                    break;
                case "tej-LABEL":
                    fname = "AKITO KOWA LABELS LTD";
                    break;
            }
            return fname;
        }

    }
