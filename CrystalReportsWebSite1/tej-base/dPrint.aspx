<%@ Page Language="C#" AutoEventWireup="true" Inherits="fin_base_dPrint" CodeFile="dPrint.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tejaxo Report Viewer</title>
    <script type="text/javascript" src="<%= Page.ResolveUrl("/Scripts/jquery.min.js") %>"></script>
    <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>


</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td style="padding-left: 40px; padding-bottom: 10px;">
                    <asp:Button ID="btnPrintToPrinter" runat="server" OnClick="btnPrintToPrinter_Click" Text="Direct Print to Printer" Height="30px" Width="180px" Style="background-color: orange; border: none; color: white;" />
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                            AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                            Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
                    </div>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hfhcid" runat="server" />
        <asp:HiddenField ID="hfval" runat="server" />
        <asp:HiddenField ID="hfclose" runat="server" />
    </form>
</body>
</html>
