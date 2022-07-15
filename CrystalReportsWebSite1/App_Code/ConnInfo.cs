using System;
using System.Web;
using System.IO;
using System.Xml;
using Fin_WFinConn_DLL;


    public class ConnInfo
    {
        public static string cd { get; set; }
        public static string srv { get; set; }
        public static string IP { get; set; }
        public static string nPwd { get; set; }        
        public static string _port = "1521";
        static string sp_cd { get; set; }
        public static XmlDocument xDocxml = new XmlDocument();
        public static XmlNodeList xGetval;
        public static string dtOnly = "N";
        public static string connString(string co_cd)
        {
            string always = "";
            string path = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
            string path2 = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
            string str = "";
            try
            {
                cd = co_cd;
                if (File.Exists(path)) { }
                else path = path2;
                using (StreamReader sr_fgen = new StreamReader(path))
                {
                    str = sr_fgen.ReadToEnd().Trim();
                    if (str.Contains("\r")) str = str.Replace("\r", ",");
                    if (str.Contains("\n")) str = str.Replace("\n", ",");
                    str = str.Replace(",,", ",");
                    if(co_cd == null || co_cd == "" || co_cd == "0") co_cd= str.Split(',')[0];
                    IP = str.Split(',')[1];
                    srv = str.Split(',')[2];
                    always = str.Split(',')[4];

                    if (str.Contains("3881") || str.Contains("4881") || str.Contains("5881") || str.Contains("6881") || str.Contains("7881"))
                    { sp_cd = str.Split(',')[3]; }
                    sr_fgen.Dispose();
                }
                if (always.Trim().Contains("oraclealways"))
                {
                    return connString_Wallet(co_cd);
                }
                dtOnly = "N";
                if (GetXMLTag(co_cd + "_AL") == "Y")
                {
                    dtOnly = "Y";
                    if (GetXMLTag(co_cd) != "0")
                    {
                        IP = GetXMLTag(co_cd);
                    }
                    else if (GetXMLTag(co_cd + "_IP") != "0")
                    {
                        IP = GetXMLTag(co_cd + "_IP");
                        srv = GetXMLTag(co_cd + "_SN");
                        if (srv == "0") srv = "XE";
                    }
                }
            }
            catch { }
            string cPwd = "";
            co_cd = co_cd.ToUpper();
            string ID = "PD" + co_cd;
            nPwd = "" + co_cd + "PD";

            if (co_cd == "SGRP"|| co_cd == "DISP" || co_cd == "SRPF"|| co_cd == "PGTL")
            {
                nPwd = "" + co_cd + "FIN";
                ID = "FIN" + co_cd;
            }
            
         
            //cPwd = Fin_Conn_DLL.GetConnPwd(co_cd, IP, srv);
            if (cPwd.Length > 0) nPwd = cPwd;

            if (IP.Contains(":"))
            {
                _port = IP.Split(':')[1];
                IP = IP.Split(':')[0];
            }
            //if (ID == "PDUMED")
            //{
            //    ConnInfo.IP = "14.99.238.114";
            //}
            string constr = "Data Source=(DESCRIPTION="
                 + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + ConnInfo.IP.Trim() + ")(PORT=" + _port + ")))"
                 + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + ConnInfo.srv + ")));"
                 + "User Id= " + ID + "; Password= " + nPwd + ";";
            return constr;
        }
    public static string GETBOOL(int ind)
    {
        string always = "";
        string path = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
        string path2 = HttpRuntime.AppDomainAppPath + "\\mytns.txt";
        string str = "";
        try
        {
            
            if (File.Exists(path)) { }
            else path = path2;
            using (StreamReader sr_fgen = new StreamReader(path))
            {
                str = sr_fgen.ReadToEnd().Trim();
                if (str.Contains("\r")) str = str.Replace("\r", ",");
                if (str.Contains("\n")) str = str.Replace("\n", ",");
                str = str.Replace(",,", ",");               
                always = str.Split(',')[ind];

                sr_fgen.Dispose();
            }            
        }
        catch { }
     
        return always;
    }
    public static string connStringManual(string co_cd, string ip, string serviceName)
        {
            co_cd = co_cd.ToUpper();
            nPwd = "" + co_cd + "FIN";
            string cPwd = "";
            cPwd = Fin_Conn_DLL.GetConnPwd(co_cd, ip, serviceName);

            if (cPwd.Length > 0) nPwd = cPwd;
            if (ip.Contains(":"))
            {
                _port = ip.Split(':')[1];
                ip = ip.Split(':')[0];
            }
            string constr = "Data Source=(DESCRIPTION="
                 + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + ip + ")(PORT=" + _port + ")))"
                 + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + serviceName + ")));"
                 + "User Id= FIN" + co_cd + "; Password= " + nPwd + ";";
            return constr;
        }
        public static string connStringSys(string username, string password, string ip, string serviceName)
        {
            string constr = "Data Source=(DESCRIPTION="
                 + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST= " + ip + ")(PORT=1521)))"
                 + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + serviceName + ")));"
                 + "User Id= " + username + "; Password= " + password + ";";
            return constr;
        }
        public static string GetXMLTag(string xmlval)
        {
            string strval = "0";
            string xmlFilePath = (@"C:\ipinfo.xml");
            try
            {
                xDocxml.Load(xmlFilePath);
                xGetval = xDocxml.GetElementsByTagName(xmlval);
                strval = xGetval[0].InnerText.Trim();
            }
            catch { }
            return strval;
        }

        public static string connString_Wallet(string pco_cd)
        {

            string tpath = "Wallet_DB202101062240";



            string conString = "User Id=admin;Password=Baghel#12345;";
            string path = HttpContext.Current.Server.MapPath("~/" + tpath);
            //Enter port, host name or IP, service name, and wallet directory for your Oracle Autonomous DB.
            conString += "Data Source=(description=(address=(protocol=tcps)(port=1522)(host=adb.ap-mumbai-1.oraclecloud.com))" +
                "(connect_data=(service_name=ansuj5efkmfech0_db202101062240_high.adb.oraclecloud.com))(SECURITY = (MY_WALLET_DIRECTORY = " + path + ")));";


            return conString;

        }
    }
