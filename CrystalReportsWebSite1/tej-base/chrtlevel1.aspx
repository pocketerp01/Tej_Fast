<%@ Page Language="C#" AutoEventWireup="true" Inherits="chrtlevel1" CodeFile="chrtlevel1.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>

    <%--<script src="Scripts/JQuery-1.8.3.js" type="text/javascript"></script>--%>
    <script src="Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="Scripts/highcharts.js" type="text/javascript"> </script>
    <script src="Scripts/funnel.js" type="text/javascript"> </script>
    <script src="Scripts/highcharts-more.js" type="text/javascript"> </script>
    <script src="Scripts/exporting.js" type="text/javascript"></script>
     <script src="Scripts/drilldown.js" type="text/javascript"></script>    
    <%--<link href="Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <link href="Styles/fin.css" rel="stylesheet" type="text/css" />--%>

</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:35px;">
    <%--border-style: groove; border-color: #0099FF;--%>
    <div id="container" style="width: 100%; height: 100%; margin:0 auto; "></div>
    </div>
    </form>
</body>
</html>
