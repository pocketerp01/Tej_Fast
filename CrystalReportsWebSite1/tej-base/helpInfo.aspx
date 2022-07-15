<%@ Page Language="C#" AutoEventWireup="true" Inherits="fin_base_helpInfo" CodeFile="helpInfo.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
    <script src="Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <link href="Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" style="font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin-top:40px">
        <div id="F10194" runat="server">            
            <h3 style="text-align: center;"><u>WIP Valuation Report</u></h3>
            <ol>
                <li>RM Cost : This is the Cost per pc of the &ldquo;Material&rdquo; in that product</li>
                <li>Cost with PL : In every Manufacturing Process, there is some Normal Material Loss during the Process. This Process Loss is usually set in the Master at 2% to 10% based on the Production Process. This has been taken at rate in your Masters. If no rate entered, then taken at 6%. So, this is the Cost per pc including PL</li>
                <li>Closing Value : Closing Quantity x Cost per pc including Process Loss</li>
                <li>Cl Stock Weight : Closing Quantity x Net Wt per pc ( or Closing Qnty x Gross Wt) based on the option chosen by you</li>
                <li>Conversion Cost Gross : Conversion Process Cost of All Stages upto this stage. This is based on the Process Cost Rates Master in the ERP. And Multiplied by the Gross Wt.</li>
                <li>Conversion Cost Net : Conversion Process Cost , same as above, on Net Weight Basis</li>
                <li>Stock Value with Conversion Cost : Closing Stock RM, Plus Process Cost &ldquo;Gross&rdquo; basis</li>
            </ol>
            <p>&nbsp;</p>
        </div>
    </form>
</body>
</html>
