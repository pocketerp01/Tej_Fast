<%@ Page Language="C#" AutoEventWireup="true" Inherits="Frm_Report" CodeFile="Frm_Report.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
    <script src="Scripts/shortcut.js" type="text/javascript"></script>
    <script src="../tej-base/scripts/jquery.min.js" type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>
    <script lang="javascript" type="text/javascript">
        var size = 2;
        var id = 0;
        function submitFile() {
            $("#<%= btnupload.ClientID%>").click();
            ProgressBar();
        };
        function ProgressBar() {
            if (document.getElementById('<%=fupl.ClientID %>').value != "") {
                document.getElementById("divProgress").style.display = "block";
                document.getElementById("divUpload").style.display = "block";
                id = setInterval("progress()", 20);
                return true;
            }
            else {
                alert("Select a file to upload");
                return false;
            }
        }

        function progress() {
            size = size + 1;
            if (size > 199) {
                clearTimeout(id);
            }
            document.getElementById("divProgress").style.width = size + "pt";
            document.getElementById("<%=lblPercentage.ClientID %>").
                firstChild.data = parseInt(size / 2) + "%";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server" style="padding-top: 30px;">
         <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnableCdn="true">
        </asp:ScriptManager>
        <div style="width: 100%;" align="center">
            <table>
                <tr id="tremail" runat="server" style="border-style: groove; border-width: thin">
                    <td>
                        <asp:ImageButton ID="btnsendmail" runat="server" ImageUrl="~/tej-base/images/Send_mail.png"
                            Width="30px" Height="30px" ToolTip="Send E-Mail"
                            OnClick="btnsendmail_Click" />
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <td align="left" class="style1">CC:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtemailcc" runat="server" Width="350px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="left" class="style1">BCC:</td>
                                <td align="left">
                                    <asp:TextBox ID="txtemailbcc" runat="server" Width="350px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="left" class="style1">Attachment</td>
                                <td align="left">
                                    <asp:FileUpload ID="fupl" runat="server" />

                                    <div id="divUpload" style="display: none; align-items: center;" runat="server">
                                        <div id="Div3" style="width: 200pt; height: 15px; border: solid 1pt gray" runat="server">
                                            <div id="divProgress" runat="server" style="width: 1pt; height: 15px; background-color: #1797c0; display: none">
                                            </div>
                                        </div>
                                        <div style="width: 200pt; text-align: center;">
                                            <asp:Label ID="lblPercentage" runat="server" Text="Label"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnupload" runat="server" Text="Upload" class="myButton" OnClick="btnupload_Click" Style="display: none" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
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
                    </td>
                    <td valign="top">
                        <div id="div1" runat="server" align="center">
                            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                                AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                                Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" runat="server" align="center">
            <img src="images/nodata.gif" title="No Data Found" alt="" />
        </div>
        <asp:GridView ID="sg1" runat="server" Style="display: none"></asp:GridView>
        <asp:Button ID="btnexp" runat="server" OnClick="btnexp_Click" Style="display: none" />
    </form>
</body>
</html>
