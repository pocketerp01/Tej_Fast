<%@ Page Language="C#" AutoEventWireup="true" CodeFile="invn-reps.aspx.cs" Inherits="invn_reps" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tejaxo Report Viewer</title>
    <script src="../crystalreportviewers13/js/crviewer/crv.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server" style="padding-top: 10px;">
        <div style="width: 100%;" align="center" id="divReportViewer" runat="server">
            <table>
                <tr>
                    <td valign="top" style="position: fixed;" id="tdprint" runat="server">
                        <asp:ImageButton ID="btnprint1" runat="server" ImageUrl="~/tej-base/images/print_btn.ico"
                            Width="30px" Height="30px"
                            ToolTip="Print (Ctrl + P)" OnClick="btnprint1_Click" autopostback="false" />
                        <br />
                        <asp:Button ID="pr" Text="s" runat="server" Style="display: none" />
                        <asp:ImageButton ID="btnexptopdf" runat="server" ImageUrl="~/tej-base/images/pdf_icon.png" OnClick="btnexptopdf_Click" ToolTip="Export to PDF" Width="30px" Height="30px" />
                        <br />
                        <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="~/tej-base/images/excel_icon.png" ToolTip="Export to Excel" Width="30px" Height="30px"
                            OnClick="btnexptoexl_Click" />
                        <br />
                        <asp:ImageButton ID="btnexptoword" runat="server"
                            ImageUrl="~/tej-base/images/Word-2-icon.png" ToolTip="Export to Word"
                            OnClick="btnexptoword_Click" Style="margin-top: 0px" Width="30px" Height="30px" />
                          <br />
                        <asp:ImageButton ID="btnPurExcel" runat="server" ImageUrl="~/tej-base/images/excel_icon.png" ToolTip="Export to Excel" Width="30px" Height="30px"
                            OnClick="btnPurExcel_Click" />
                    </td>
                    <td valign="top">
                        <div id="div1" align="center">
                                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                                AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                                Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table style="width: 900px">
                <tr id="No_Data_Found" runat="server" style="border-style: groove; border-width: thin; align-content: center">
                    <td style="text-align: center; align-content: center; font-size: large; height: 500px; width: 800px" class="style1"><strong>There is No /Inconsistent Data for Requested Report</strong></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfhcid" runat="server" />
        <asp:HiddenField ID="hfval" runat="server" />
        <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />

        <script src="../tej-base/Scripts/shortcut.js" type="text/javascript"></script>

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

        <%--        <script type="text/javascript">
            function load() {
                //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(closePopup);
            }
            $(document).keyup(function (event) {
                if (event.keyCode == 27) {
                    parent.$.colorbox.close();
                }
            });
        </script>--%>
        <script type="text/javascript">
            function closePopup() {
                parent.$.colorbox.close();
            }
            $(document).keyup(function (e) { 27 == e.keyCode && document.getElementById("btnhide").click() });

            function closePopupmy(o) { $(o, window.parent.document).trigger("click"), parent.$.colorbox.close() }

            function closePopup2() {
                alert("No Data Exist!!");
                parent.$.colorbox.close();
            }
        </script>
    </form>
</body>
</html>
