<%@ Page Language="C#" AutoEventWireup="true" Inherits="SSeek" CodeFile="SSeek - old Copy.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Finsys</title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    
    <script src="tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <link href="../tej-base/css/GridviewScroll.css" type="text/css" rel="Stylesheet" />
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function load() {
            //Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(EndRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(gridviewScroll);
        }
        $(document).ready(function () {
            gridviewScroll();
            gridRowsSelection();
        });
        window.addEventListener("keydown", function (e) {
            // space and arrow keys
            if ([38, 40].indexOf(e.keyCode) > -1) {
                e.preventDefault();
            }
        }, false);
        //function EndRequestHandler()
        {
            //gridviewScroll();
            $(document).keyup(function (e) { 27 == e.keyCode && document.getElementById("btnhide").click() });
            function gridviewScroll() { $("#GridView1").gridviewScroll({ width: 870, height: 380, startHorizontal: 1, headerrowcount: 1, wheelstep: 5, barhovercolor: "#0e7192", barcolor: "#0e7192" }) }
            function Search_Gridview(e, n) { var r, o = e.value.toLowerCase().split(" "), a = document.getElementById(n); if (!e.value.includes("%")) for (var i = 1; i < a.rows.length; i++) { r = a.rows[i].innerHTML; for (var t = "none", l = 0; l < o.length; l++) { if (!(r.toLowerCase().indexOf(o[l]) >= 0)) { t = "none"; break } t = "" } a.rows[i].style.display = t } }
            function closePopup(o) { $(o, window.parent.document).trigger("click"), parent.$.colorbox.close() }
            var SelectedRow = null;
            var SelectedRowIndex = null;
            var UpperBound = null;
            var LowerBound = null;

            function gridRowsSelection() {
                UpperBound = parseInt('<%= this.GridView1.Rows.Count %>') - 1;
                LowerBound = 0;
                SelectedRowIndex = -1;
            }

            function SelectRow(CurrentRow, RowIndex) {
                if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) return;

                if (SelectedRow != null) {
                    SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                    SelectedRow.style.color = SelectedRow.originalForeColor;
                }

                if (CurrentRow != null) {
                    CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                    CurrentRow.originalForeColor = CurrentRow.style.color;
                    CurrentRow.style.backgroundColor = '#DCFC5C';
                    CurrentRow.style.color = 'Black';
                }

                SelectedRow = CurrentRow;
                SelectedRowIndex = RowIndex;
                setTimeout("SelectedRow.focus();", 0);
            }

            function SelectSibling(e) {
                var e = e ? e : window.event;
                var KeyCode = e.which ? e.which : e.keyCode;
                if (KeyCode == 40)
                    SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
                else if (KeyCode == 38)
                    SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

                return false;
            }
        }
    </script>
</head>
<body onload="load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scr1" runat="server"></asp:ScriptManager>
        <table style="margin-top: 35px; width: 100%;">
            <tr>
                <td>
                    <div style="padding-left: 5px;">
                        <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" AutoCompleteType="Disabled" EnableViewState="false"
                            placeholder="Enter here to search" ToolTip="Enter here to search" onkeyup="Search_Gridview(this, 'GridView1')"></asp:TextBox>

                        <asp:ImageButton ID="srch" runat="server" OnClick="srch_Click" ImageUrl="images/search-button.png" Width="120px" Height="27px" ToolTip="Click to Search" Style="display: none" />

                    </div>
                </td>
                <td style="float: right">
                    <span class="font_css">Show Rows</span> &nbsp;&nbsp;
                    <asp:TextBox ID="tkrow" runat="server" Width="40px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                        Style="text-align: right;" Text="100" OnTextChanged="tkrow_TextChanged" AutoPostBack="true"></asp:TextBox>
                    &nbsp;&nbsp;
                    <asp:Label ID="lblTotcount" runat="server" Style="font-size: 10px; font-family: 'Arial Unicode MS'"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                        <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:GridView ID="GridView1" runat="server"
                                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="100%"
                                    Style="font-family: Arial, Helvetica, sans-serif; font-size: small"
                                    OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"
                                    AllowSorting="true" OnSorting="GridView1_Sorting1">

                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                    <Columns>
                                        <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                            SelectImageUrl="images/tick.png">
                                            <ItemStyle Width="25px"></ItemStyle>
                                        </asp:CommandField>
                                    </Columns>
                                    <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <EmptyDataTemplate>
                                        <asp:Image ID="imgdata" runat="server" ImageUrl="images/nodata.gif" AlternateText="No Data Exist" />
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="images/excel_icon.png"
                        ToolTip="Export to Excel" Width="30px" Height="30px" OnClick="btnexptoexl_Click" />
                    <asp:ImageButton ID="btnexptopdf" runat="server" ImageUrl="images/pdf_icon.png"
                        ToolTip="Export to PDF" Width="30px" Height="30px" OnClick="btnexptopdf_Click" />
                    <asp:ImageButton ID="btnexptoword" runat="server"
                        ImageUrl="images/Word-2-icon.png" ToolTip="Export to Word"
                        Style="margin-top: 0" Width="30px" Height="28px" OnClick="btnexptoword_Click" />
                </td>
            </tr>
        </table>

        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />

    </form>
</body>

</html>
