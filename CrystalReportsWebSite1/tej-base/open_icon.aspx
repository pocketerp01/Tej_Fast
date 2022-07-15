<%@ Page Language="C#" AutoEventWireup="true" Inherits="open_icon" CodeFile="open_icon.aspx.cs" %>

<%@ Register Assembly="IdeaSparx.CoolControls.Web" Namespace="IdeaSparx.CoolControls.Web"
    TagPrefix="fin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Finsys</title>

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />

    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <link href="../tej-base/css/GridviewScroll2.css" type="text/css" rel="Stylesheet" />
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        var gridHeight = 353;
        var gridWidth = 1070;
        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(gridviewScroll);
        }
        $(document).ready(function () {
            gridWidth = $("#gridDiv").width() - 0;
            if ($(window).width() > 1070 && gridWidth < 1070) gridWidth = 1070
            if (gridWidth > 1070 && $(window).width() < 1070) gridWidth = 1070;
            gridviewScroll();

            var val = $('#txtsearch').val();
            $('#txtsearch').val('');
            $('#txtsearch').val(val);
            $('#txtsearch').focus();
        });
        window.addEventListener("keydown", function (e) {
            // space and arrow keys
            if ([38, 40].indexOf(e.keyCode) > -1) {
                e.preventDefault();
            }
        }, false);
        $(document).keyup(function (e) { 27 == e.keyCode && document.getElementById("btnhide").click() });
        function gridviewScroll() { $("#GridView1").gridviewScroll({ width: gridWidth, height: gridHeight, startHorizontal: 1, headerrowcount: 1, wheelstep: 5, barhovercolor: "#0e7192", barcolor: "#0e7192" }) }
        function Search_Gridview(e, n) { var r, o = e.value.toLowerCase().split(" "), a = document.getElementById(n); if (!e.value.includes("%")) for (var i = 1; i < a.rows.length; i++) { r = a.rows[i].innerHTML; for (var t = "none", l = 0; l < o.length; l++) { if (!(r.toLowerCase().indexOf(o[l]) >= 0)) { t = "none"; break } t = "" } a.rows[i].style.display = t } }
        function closePopup(o) { $(o, window.parent.document).trigger("click"), parent.$.colorbox.close() }
        function onlyclose() { parent.$.colorbox.close(); }

        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            UpperBound = parseInt('<%= this.GridView1.Rows.Count %>') * 2;
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
            else if (KeyCode == 33)
                for (var i = 0; i < 10; i++) {
                    SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
                }
            else if (KeyCode == 34) {
                for (var i = 0; i < 10; i++) {
                    SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
                }
            }

            return false;
        }

        function grid_sorting() {
            $("th").click(function () {
                var columnIndex = $(this).index();
                var tdArray = $(this).closest("table").find("tr td:nth-child(" + (columnIndex + 1) + ")");
                tdArray.sort(function (p, n) {
                    var pData = $(p).text();
                    var nData = $(n).text();
                    return pData < nData ? -1 : 1;
                });
                tdArray.each(function () {
                    var row = $(this).parent();
                    $("#GridView1").append(row);
                });
            });
        }

        function getData() {
            $.ajax({
                url: '../tej-base/open_icon.aspx/fill_data',
                dataType: "JSON",
                method: "post",
                data: JSON.stringify(),
                success: function (data) {
                    var e1 = $("#tbl1 tbody");
                    e1.empty();
                    e1.append('<tr><td>' + data.id + '</td></tr>');
                    //$(data).each(function (index, msy) {
                    //    e1.append('<tr><td>' + msy.id + '</td></tr>');
                    //});
                },
                error: function (err) {
                    console.log(err);
                    alert(err);
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scr" runat="server"></asp:ScriptManager>
        <div style="margin-top: 30px;">
            <div>
                <div class="col-md-12">
                    <div class="form-group" style="padding: 5px;">
                        <table style="width: 98%">
                            <tr>
                                <td style="padding: 5px;">
                                    <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch"
                                        placeholder="Enter here to search" onkeyup="Search_Gridview(this, 'GridView1')" ToolTip="Enter here to search"></asp:TextBox>
                                </td>
                                <td style="padding-top: 3px">
                                    <asp:ImageButton ID="srch" runat="server" OnClick="srch_Click" ImageUrl="images/search-button.png" Width="110px" Height="27px" ToolTip="Click to Search" />
                                </td>
                                <td style="width: 40%"></td>
                                <td style="padding-right: 10px;"><span class="font_css">Show Rows</span></td>
                                <td>
                                    <asp:TextBox ID="tkrow" runat="server" Width="40px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                                        Style="text-align: right;" Text="200" OnTextChanged="tkrow_TextChanged"></asp:TextBox>
                                    &nbsp;&nbsp;
                    <asp:Label ID="lblTotcount" runat="server" Style="font-size: 10px; font-family: 'Arial Unicode MS'"></asp:Label>
                                </td>
                                <td>&nbsp;&nbsp;
                                    <button id="btnClose" runat="server" accesskey="C" onserverclick="btnClose_ServerClick" class="bg-green"><u>C</u>lose</button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="form-group">
                        <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                            <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView ID="GridView1" runat="server"
                                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="100%"
                                        Style="font-size: small" CellPadding="1" ForeColor="#000"
                                        AllowSorting="true" OnSorting="GridView1_Sorting1"
                                        OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated" AutoGenerateColumns="false">

                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader2" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle CssClass="hidden" />
                                                <HeaderStyle CssClass="hidden" />
                                            </asp:CommandField>
                                            <asp:BoundField HeaderText="Id" DataField="fstr" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                            <asp:BoundField HeaderText="Web" DataField="web_Action" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                            <asp:BoundField HeaderText="Text" DataField="text" />
                                            <asp:BoundField HeaderText="Description" DataField="SEARCH_KEY" />
                                            <asp:BoundField HeaderText="Form ID" DataField="id" />
                                        </Columns>
                                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <EmptyDataTemplate>
                                            <asp:Image ID="imgdata" runat="server" ImageUrl="images/nodata.gif" AlternateText="No Data Exist" />
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfWidth" runat="server" />
        <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />
    </form>
</body>
</html>
