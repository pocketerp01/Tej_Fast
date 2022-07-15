using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class Product
{
    private string productID;
    private string productName;
    private string productAction;

    public string ProductID
    {
        get { return this.productID; }
        set { this.productID = value; }
    }

    public string ProductName
    {
        get { return this.productName; }
        set { this.productName = value; }
    }
    public string ProductAction
    {
        get { return this.productAction; }
        set { this.productAction = value; }
    }

    public Product(string productName, string productID, string productAction)
    {
        this.productName = productName;
        this.productID = productID;
        this.productAction = productAction;
    }
    public Product()
    {
    }
}