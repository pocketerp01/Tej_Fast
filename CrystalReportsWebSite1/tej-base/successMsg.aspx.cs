using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;


public partial class successMsg : System.Web.UI.Page
{

    fgenDB fgen = new fgenDB();
    string frm_qstr = Guid.NewGuid().ToString().Substring(0, 20).ToUpper();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string[] merc_hash_vars_seq;
            string merc_hash_string = string.Empty;
            string merc_hash = string.Empty;
            string order_id = string.Empty;
            string amount = "";
            string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

            string sTTxlwww = Request.Cookies["sTTxlwww"].Value.Trim();

            if (Request.Form["status"] == "success")
            {
                merc_hash_vars_seq = hash_seq.Split('|');
                Array.Reverse(merc_hash_vars_seq);
                merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + Request.Form["status"];

                foreach (string merc_hash_var in merc_hash_vars_seq)
                {
                    merc_hash_string += "|";
                    merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");

                }
                merc_hash = Generatehash512(merc_hash_string).ToLower();



                if (merc_hash != Request.Form["hash"])
                {
                    //Response.Write("Hash value did not matched");

                }
                else
                {
                    order_id = Request.Form["txnid"];
                    amount = Request.Form["amount"];

                    string vNmxxRQM = Request.Cookies["vNmxxRQM"].Value.Trim();
                    string tNmxxRQM = Request.Cookies["tNmxxRQM"].Value.Trim();

                    if ((fgen.make_double(amount) == fgen.make_double(EncryptDecrypt.Decrypt(vNmxxRQM))) && (order_id == EncryptDecrypt.Decrypt(tNmxxRQM)))
                    {
                        //fgen.msg("-", "AMSG", "Payment Success!!'13'Transaction ID : " + order_id + "'13'Amount Paid : " + amount);
                        fgen.execute_cmd(frm_qstr, "TEST", "UPDATE GST SET PSTATUS='Y' , TXNID='" + order_id + "' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + EncryptDecrypt.Decrypt(sTTxlwww) + "' ");
                        lblErr.Text = "Payment Succeed!! <br/> Transaction ID : " + order_id + " <br/> Amount Paid : " + amount;
                    }
                    else
                    {
                        fgen.execute_cmd(frm_qstr, "TEST", "UPDATE GST SET PSTATUS='N' , TXNID='" + order_id + "' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + EncryptDecrypt.Decrypt(sTTxlwww) + "' ");

                        fgen.msg("-", "AMSG", "Payment Not Succeed!!");
                    }
                }
            }

            else
            {
                order_id = Request.Form["txnid"];
                amount = Request.Form["amount"];
                fgen.execute_cmd(frm_qstr, "TEST", "UPDATE GST SET PSTATUS='N' ,TXNID='" + order_id + "' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + EncryptDecrypt.Decrypt(sTTxlwww) + "' ");
                lblErr.Text = "Payment Not Succeed!! <br/> Transaction ID : " + order_id + " <br/> Amount : " + amount;
            }
        }
        catch (Exception ex)
        {
            fgen.msg("-", "AMSG", "Payment Not Succeed!!");
        }
    }

    public string Generatehash512(string text)
    {

        byte[] message = Encoding.UTF8.GetBytes(text);

        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue;
        SHA512Managed hashString = new SHA512Managed();
        string hex = "";
        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;

    }
}