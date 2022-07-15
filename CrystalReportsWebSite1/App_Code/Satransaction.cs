using Models;
using System;
using System.Collections.Generic;
using System.Data;

using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Oracletransaction
/// </summary>
public class Satransaction
{
    sgenFun sgen;
    private OracleConnection myconn1;

    //MyOracleTransaction transaction = null;
    OracleTransaction transaction = null;

    //private MyOracleConnection myconn1;

    //MyOracleCommand cmd;
    OracleCommand cmd;

    public Satransaction(string userCode, string Myguid)
    {
        //Multiton multiton = Multiton.GetInstance(Myguid);
        sgen = new sgenFun(Myguid);
        System.Environment.SetEnvironmentVariable("ORA_NCHAR_LITERAL_REPLACE", "TRUE");
        //myconn1 = new MyOracleConnection(sgen.connStringmyOracle(userCode));
        myconn1 = new OracleConnection(ConnInfo.connString(userCode));
        myconn1.Open();
        //myconn1 = multiton.OConn;
        transaction = myconn1.BeginTransaction(IsolationLevel.Serializable);
        //cmd = new MyOracleCommand();
        cmd = new OracleCommand();
        cmd.Connection = myconn1;
        cmd.Transaction = transaction;
    }

    public bool Execute_cmd(string command)
    {
        bool data = true;
        try
        {

            cmd.CommandText = command;
            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            //((WebViewPage)WebPageContext.Current.Page).ViewBag.scripCall += ex.Message;
            data = false;
        }
        return data;
    }
    public string Execute_cmd(string command, string hh)
    {
        string data = "1";
        try
        {
            cmd.CommandText = command;
            data = cmd.ExecuteNonQuery().ToString();
        }
        catch (Exception ex) { data = ex.Message; }
        return data;
    }

    public void Commit()
    {
        transaction.Commit();
        transaction.Dispose();
        myconn1.Close();
    }

    public void Rollback()
    {
        transaction.Rollback();
        myconn1.Close();
    }
}