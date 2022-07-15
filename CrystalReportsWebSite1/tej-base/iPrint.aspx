<%@ Page Language="C#" AutoEventWireup="true" Inherits="iPrint" CodeFile="iPrint.aspx.cs" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tejaxo Report Viewer</title>
    <script src="Scripts/shortcut.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%= Page.ResolveUrl("/Scripts/jquery.min.js") %>"></script>
    <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>
    <script type="text/javascript">
        shortcut.add("Ctrl+*", function () {
            document.getElementById("btnexp").click();
        });
    </script>
    <script type="text/javascript">
        shortcut.add("Ctrl+e", function () {
            document.getElementById("btnexptoexl").click();
        });
    </script>
    <%--<script type="text/javascript">
    shortcut.add("Ctrl+p", function () {
        document.getElementById("btnprint1").click();
    });
</script>--%>

    <script type="text/javascript">
        $(document).keyup(function (event) {
            if (event.keyCode == 27) {
                parent.$.colorbox.close();
            }
            printCrystal();
        });
    </script>
    <script type="text/javascript">
        function closePopup(btn) {
            $(btn, window.parent.document).trigger('click');
            parent.$.colorbox.close();
        }
    </script>
    <script type="text/javascript">
        function printCrystal(divID) {
            var dvReport = document.getElementById(divID);
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1 || navigator.appVersion.indexOf("Trident") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            } else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
    </script>
    <style type="text/css">
        .style1 {
            width: 34px;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            color: #474646;
            font-size: 12px;
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
        </div>
        <asp:HiddenField ID="hfhcid" runat="server" />
        <asp:HiddenField ID="hfval" runat="server" />
    </form>
</body>
</html>
