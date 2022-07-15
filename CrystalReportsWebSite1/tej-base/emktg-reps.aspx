<%@ Page Language="C#" AutoEventWireup="true" Inherits="emktg_reps" CodeFile="emktg-reps.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tejaxo Report Viewer</title>
    <script src='<%=ResolveUrl("~/tej-base/Scripts/shortcut.js")%>' type="text/javascript"></script>
    <script type="text/javascript" src='<%=ResolveUrl("~/tej-base/Scripts/jquery.min.js")%>'></script>
    <script src='<%=ResolveUrl("../crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>
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
    <script type="text/javascript">
        shortcut.add("Ctrl+p", function () {
            document.getElementById("btnprint1").click();
        });
    </script>

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
    <form id="form1" runat="server" style="padding-top: 10px;">
        <asp:ScriptManager runat="server" ID="SRC1" EnableCdn="true"></asp:ScriptManager>
        <div style="width: 100%;" align="center" id="divReportViewer" runat="server">
            <table>
                <tr>
                    <td valign="top" style="position: fixed;" id="tdprint" runat="server">
                        <asp:ImageButton ID="btnprint1" runat="server" ImageUrl="~/tej-base/images/print_btn.ico"
                            Width="30px" Height="30px"
                            ToolTip="Print (Ctrl + P)" OnClick="btnprint1_Click" autopostback="false" />
                        <br />
                        <asp:Button ID="pr" Text="s" runat="server" OnClientClick="javascript:printCrystal('div1');" Style="display: none" />
                        <asp:ImageButton ID="btnexptopdf" runat="server" ImageUrl="~/tej-base/images/pdf_icon.png" OnClick="btnexptopdf_Click" ToolTip="Export to PDF" Width="30px" Height="30px" />
                        <br />
                        <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="~/tej-base/images/excel_icon.png" ToolTip="Export to Excel" Width="30px" Height="30px"
                            OnClick="btnexptoexl_Click" />
                        <br />
                        <asp:ImageButton ID="btnexptoword" runat="server"
                            ImageUrl="~/tej-base/images/Word-2-icon.png" ToolTip="Export to Word"
                            OnClick="btnexptoword_Click" Style="margin-top: 0px" Width="30px" Height="30px" />
                    </td>
                    <td valign="top">
                        <div id="div1" runat="server" align="center">
                            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                                AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                                HasExportButton="False" HasDrillUpButton="False" HasPrintButton="False"
                                Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table style="width: 900px">
                <tr id="No_Data_Found" runat="server" style="border-style: groove; border-width: thin; align-content: center">
                    <td style="text-align: center; align-content: center; font-size: large; height: 500px; width: 800px" class="style1">There is No /Inconsistent Data for Requested Report</td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfhcid" runat="server" />
        <asp:HiddenField ID="hfval" runat="server" />
    </form>
</body>
</html>
