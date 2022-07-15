<%@ Page Language="C#" AutoEventWireup="true" Inherits="SSeek8" CodeFile="SSeek.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Tejaxo</title>

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />

    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <link href="../tej-base/css/GridviewScroll.css" rel="stylesheet" type="text/css" />
    <%--<link href="../tej-base/css/GridviewScroll.css" type="text/css" rel="Stylesheet" />--%>
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 0;
            border-top: 1px groove;
        }

            .pagination-ys table > tbody > tr > td {
                display: inline-table;
            }

        body {
            background-color: GHOSTWHITE;
        }

        .pagination-ys table > tbody > tr > td > a,
        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            line-height: 1.42857143;
            text-decoration: none;
            color: #000;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            margin-left: -1px;
        }

        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            line-height: 1.42857143;
            text-decoration: none;
            margin-left: -1px;
            z-index: 2;
            color: #5F9F9F;
            background-color: #faedae;
            border-color: #dddddd;
            cursor: default;
        }

        .pagination-ys table > tbody > tr > td:first-child > a,
        .pagination-ys table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-bottom-left-radius: 4px;
            border-top-left-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td:last-child > a,
        .pagination-ys table > tbody > tr > td:last-child > span {
            border-bottom-right-radius: 4px;
            border-top-right-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td > a:hover,
        .pagination-ys table > tbody > tr > td > span:hover,
        .pagination-ys table > tbody > tr > td > a:focus,
        .pagination-ys table > tbody > tr > td > span:focus {
            color: #97310e;
            background-color: #eeeeee;
            border-color: #dddddd;
        }

        body {
            background-color: ghostwhite;
        }

        .GridviewScrollHeader TH, .GridviewScrollHeader TD, .GridviewScrollHeader TR {
            background-color: #0a437f;
            font-family: monospace !important;
        }

        .txtsrch {
            padding: 5px 1px 5px 30px;
            border-radius: 3px;
            border: 1px solid #278898;
        }

            .txtsrch:focus {
                outline-color: #278898;
                background-color: white;
            }

        body, th, table, .GridCellDiv, .cboxTitle {
            font-family: monospace !important;
        }

        input {
            font-size: inherit !important;
        }
    </style>

    <script type="text/javascript">

        var gridHeight = 380;
        var gridWidth = 840;
        function load() {
            //Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(EndRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(gridviewScroll);
        }
        $(document).ready(function () {
            gridWidth = $("#gridDiv").width() - 0;
            gridHeight = $(window).height() - 140;
            if ($(window).width() > 840 && gridWidth < 840) gridWidth = 840
            if (gridWidth > 840 && $(window).width() < 840) gridWidth = 840;
            $("#hfFormWidth").val(gridWidth);
            $("#hfFormHeight").val(gridHeight);
            gridviewScroll();
            gridRowsSelection();

            var val = $('#txtsearch').val();
            $('#txtsearch').val('');
            $('#txtsearch').val(val);
            $('#txtsearch').focus();
            //$(".GridviewScrollHeader").css("background-color", "rgba(34,148,91)");

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
            function gridviewScroll() { $("#GridView1").gridviewScroll({ width: gridWidth, height: gridHeight, startHorizontal: 1, headerrowcount: 1, wheelstep: 5, barhovercolor: "#8b7575", barcolor: "#d3caca" }) }
            //function Search_Gridview(e, n) { var r, o = e.value.toLowerCase().split(" "), a = document.getElementById(n); if (!e.value.includes("%")) for (var i = 1; i < a.rows.length; i++) { r = a.rows[i].innerHTML; for (var t = "none", l = 0; l < o.length; l++) { if (!(r.toLowerCase().indexOf(o[l]) >= 0)) { t = "none"; break } t = "" } a.rows[i].style.display = t } }
            function Search_Gridview(e, n) { }
            function closePopup(o) {
                debugger
                $(o, window.parent.document).trigger("click"), parent.$.colorbox.close()
            }
            function OnlyClose() { parent.$.colorbox.close() }
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
                    CurrentRow.style.backgroundColor = '#ffc10759';
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
        }

    </script>
</head>
<body onload="load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <%--<asp:ScriptManager ID="scr1" runat="server"></asp:ScriptManager>--%>
        <section class="content" style="margin-top: 30px;">
            <div class="row">
                <div class="col-md-12">
                    <div class="box-primary">
                        <div class="col-sm-12">
                            <table style="width: 100%">
                                <tr style="vertical-align: central">
                                    <td style="width: 200px">
                                        <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" Width="100%" AutoCompleteType="Disabled" EnableViewState="false"
                                            placeholder="Enter here to search" ToolTip="Enter here to search" onkeyup="Search_Gridview(this, 'GridView1')"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="srch" runat="server" OnClick="srch_Click" ImageUrl="~/tej-base/css/images/selectimg1.png" Width="140px" Style="margin-top: 3px" ToolTip="Click to Search" />
                                    </td>
                                    <td>
                                        <span class="font_css" style="margin-left: 40%">Show Rows</span>
                                        <asp:TextBox ID="tkrow" runat="server" Width="40px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                                            Style="text-align: right;" Text="100" OnTextChanged="tkrow_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <asp:Label ID="lblTotcount" runat="server" Style="font-size: 10px; font-family: 'Arial Unicode MS'"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12" style="margin-top: 5px;">
                <div class="box-primary">
                    <div class="form-group">
                        <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                            <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:GridView ID="GridView1" runat="server"
                                        OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="100%" AllowPaging="true"
                                        Style="font-family: Arial, Helvetica, sans-serif; font-size: small" OnPageIndexChanging="GridView1_PageIndexChanging1" PageSize="30"
                                        OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"
                                        AllowSorting="true" OnSorting="GridView1_Sorting1">
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
                                        <RowStyle BackColor="GhostWhite" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle CssClass="pagination-ys" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#0a437f" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <%--22945b old header color--%>
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="Black" />

                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <EmptyDataTemplate>
                                            <asp:Image ID="imgdata" runat="server" ImageUrl="../images/norecords.png" AlternateText="No Data Exist" />
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-12" runat="server">
                <div class="box-primary">
                    <div class="form-group">
                        <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="images/BDexcel.png"
                            ToolTip="Export to Excel" Width="25px" Height="25px" OnClick="btnexptoexl_Click" />
                        <asp:ImageButton ID="btnexptopdf" runat="server" ImageUrl="images/BDpdf.png"
                            ToolTip="Export to PDF" Width="25px" Height="25px" OnClick="btnexptopdf_Click" />
                        <asp:ImageButton ID="btnexptoword" runat="server"
                            ImageUrl="images/BDword.png" ToolTip="Export to Word"
                            Style="margin-top: 0" Width="25px" Height="25px" OnClick="btnexptoword_Click" />
                    </div>
                </div>
            </div>

        </section>
        <asp:HiddenField ID="hfFormWidth" runat="server" />
        <asp:HiddenField ID="hfFormHeight" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />

    </form>
</body>

</html>
