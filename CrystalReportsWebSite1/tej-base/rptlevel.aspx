<%@ Page Language="C#" AutoEventWireup="true" Inherits="rptlevel" CodeFile="rptlevel.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <title>Finsys</title>
    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <link href="../tej-base/css/GridviewScroll2.css" type="text/css" rel="Stylesheet" />
    <%--<script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />--%>
    <link rel="stylesheet" type="text/css" href="../tej-base/Styles/vip_vrm.css" />

    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(document).keyup(function (event) {
            if (event.keyCode == 27) {
                parent.$.colorbox.close();
                //document.getElementById("btnBack").click();
            }
        });
        function onlyClose() {
            parent.$.colorbox.close();
        }
    </script>
    <script type="text/javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var SelectedCol = null;
        var SelectedColIndex = null;
        var LeftBound = null;
        var RightBound = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            UpperBound = (parseInt('<%= this.sg1.Rows.Count %>') * 2) - 1;
            RightBound = parseInt('<%= this.sg1.Columns.Count %>') - 1;
            LowerBound = 0; LeftBound = 0;
            SelectedRowIndex = -1;
            SelectedColIndex = 1;
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

            if (KeyCode == 40) {
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            }
            else if (KeyCode == 38) {
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            }

            return false;
        }
        function Search_Gridview(strKey, strGV) {
            var strData = strKey.value.toLowerCase().split(" ");
            var tblData = document.getElementById(strGV);
            var rowData;
            if (strKey.value.includes('%')) return;
            for (var i = 1; i < tblData.rows.length; i++) {
                rowData = tblData.rows[i].innerHTML;
                var styleDisplay = 'none';
                for (var j = 0; j < strData.length; j++) {
                    if (rowData.toLowerCase().indexOf(strData[j]) >= 0)
                        styleDisplay = '';
                    else {
                        styleDisplay = 'none';
                        break;
                    }
                }
                tblData.rows[i].style.display = styleDisplay;
            }
        }
    </script>
    <style type="text/css">
        ::-webkit-scrollbar {
            width: 8px;
            height: 8px;
        }

        ::-webkit-scrollbar-track {
            background: #f1f1f1;
        }

        ::-webkit-scrollbar-thumb {
            background: #888;
        }

            ::-webkit-scrollbar-thumb:hover {
                background: #3d98af;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 35px;">
            <div style="margin-left: 15px">
                <div class="row">
                    <div class="col-md-12" style="display: none">
                        <div class="form-group">
                            <label id="lbl1" runat="server" class="col-sm-1 control-label" title="lbl1" style="font-family: Arial; font-size: smaller; color: #000;">
                                Showing For</label>
                            <label id="lblMsgSel" runat="server" class="col-sm-11 control-label" style="font-family: Arial; font-size: smaller; color: #000;"></label>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="form-group">

                            <div class="col-sm-12" style="padding-bottom: 10px;">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="btnBack" runat="server" ImageUrl="~/tej-base/images/Previous.JPG" ToolTip="Back" Width="40px" OnClick="btnBack_Click" Style="display: none" />
                                        </td>
                                        <td style="padding-right: 5px">
                                            <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" AutoPostBack="true" OnTextChanged="txtsearch_TextChanged"
                                                placeholder="Enter here to search" ToolTip="Enter here to search"></asp:TextBox>
                                        </td>
                                        <td style="padding-right: 10px">
                                            <asp:ImageButton ID="srch" runat="server" ImageUrl="~/tej-base/images/search-button.png"
                                                Width="120px" Height="27px" ToolTip="Click to Search" OnClick="srch_Click" />
                                        </td>
                                        <td>
                                            <label id="lblMsg" runat="server" style="font-family: Verdana; font-size: smaller; color: #999999; "></label>
                                        </td>
                                        <td style="width: 40%"></td>
                                        <td style="padding-right: 10px">
                                            <label id="Label1" runat="server" style="font-family: Verdana; font-size: smaller; color: #999999;">Show Rows</label>
                                        </td>
                                        <td style="padding-right: 20px">
                                            <asp:TextBox ID="tkrow" runat="server" Width="40px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                                                Style="text-align: right;" Height="27px" Text="100"></asp:TextBox>
                                        </td>
                                        <td>
                                            <label id="lblTotcount" runat="server" style="font-size: 10px; font-family: Verdana"></label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>



                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="lbBody" id="gridDiv" style="color: White; margin-right: 15px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                <fin:CoolGridView ID="sg1" runat="server" Height="488px"
                                    OnRowDataBound="sg1_RowDataBound" OnSelectedIndexChanged="sg1_SelectedIndexChanged" OnRowCreated="sg1_RowCreated" CellPadding="1" ForeColor="#333333"
                                    Style="font-size: small" AutoGenerateColumns="false" AllowSorting="true" >
                                    <RowStyle BackColor="#F7F6F3" ForeColor="Black" CssClass="GridviewScrollItem2" />                                
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader2" />
                                    <EditRowStyle BackColor="#999999" />
                                    <AlternatingRowStyle BackColor="White" ForeColor="Black" />
                                    <Columns>
                                        <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                            SelectImageUrl="~/tej-base/images/tick.png">
                                            <ItemStyle Width="25px"></ItemStyle>
                                        </asp:CommandField>

                                        <asp:BoundField HeaderText="sg1_f1" DataField="sg1_f1" />
                                        <asp:BoundField HeaderText="sg1_f2" DataField="sg1_f2" />
                                        <asp:BoundField HeaderText="sg1_f3" DataField="sg1_f3" />
                                        <asp:BoundField HeaderText="sg1_f4" DataField="sg1_f4" />
                                        <asp:BoundField HeaderText="sg1_f5" DataField="sg1_f5" />
                                        <asp:BoundField HeaderText="sg1_f6" DataField="sg1_f6" />
                                        <asp:BoundField HeaderText="sg1_f7" DataField="sg1_f7" />
                                        <asp:BoundField HeaderText="sg1_f8" DataField="sg1_f8" />
                                        <asp:BoundField HeaderText="sg1_f9" DataField="sg1_f9" />
                                        <asp:BoundField HeaderText="sg1_f10" DataField="sg1_f10" />
                                        <asp:BoundField HeaderText="sg1_f11" DataField="sg1_f11" />
                                        <asp:BoundField HeaderText="sg1_f12" DataField="sg1_f12" />
                                        <asp:BoundField HeaderText="sg1_f13" DataField="sg1_f13" />
                                        <asp:BoundField HeaderText="sg1_f14" DataField="sg1_f14" />
                                        <asp:BoundField HeaderText="sg1_f15" DataField="sg1_f15" />
                                        <asp:BoundField HeaderText="sg1_f16" DataField="sg1_f16" />
                                        <asp:BoundField HeaderText="sg1_f17" DataField="sg1_f17" />
                                        <asp:BoundField HeaderText="sg1_f18" DataField="sg1_f18" />
                                        <asp:BoundField HeaderText="sg1_f19" DataField="sg1_f19" />
                                        <asp:BoundField HeaderText="sg1_f20" DataField="sg1_f20" />
                                        <asp:BoundField HeaderText="sg1_f21" DataField="sg1_f21" />
                                        <asp:BoundField HeaderText="sg1_f22" DataField="sg1_f22" />
                                        <asp:BoundField HeaderText="sg1_f23" DataField="sg1_f23" />
                                        <asp:BoundField HeaderText="sg1_f24" DataField="sg1_f24" />
                                        <asp:BoundField HeaderText="sg1_f25" DataField="sg1_f25" />
                                        <asp:BoundField HeaderText="sg1_f26" DataField="sg1_f26" />
                                        <asp:BoundField HeaderText="sg1_f27" DataField="sg1_f27" />
                                        <asp:BoundField HeaderText="sg1_f28" DataField="sg1_f28" />
                                        <asp:BoundField HeaderText="sg1_f29" DataField="sg1_f29" />
                                        <asp:BoundField HeaderText="sg1_f30" DataField="sg1_f30" />
                                        <asp:BoundField HeaderText="sg1_f31" DataField="sg1_f31" />
                                        <asp:BoundField HeaderText="sg1_f32" DataField="sg1_f32" />
                                        <asp:BoundField HeaderText="sg1_f33" DataField="sg1_f33" />
                                        <asp:BoundField HeaderText="sg1_f34" DataField="sg1_f34" />
                                        <asp:BoundField HeaderText="sg1_f35" DataField="sg1_f35" />
                                        <asp:BoundField HeaderText="sg1_f36" DataField="sg1_f36" />
                                        <asp:BoundField HeaderText="sg1_f37" DataField="sg1_f37" />
                                        <asp:BoundField HeaderText="sg1_f38" DataField="sg1_f38" />
                                        <asp:BoundField HeaderText="sg1_f39" DataField="sg1_f39" />
                                        <asp:BoundField HeaderText="sg1_f40" DataField="sg1_f40" />
                                        <asp:BoundField HeaderText="sg1_f41" DataField="sg1_f41" />
                                        <asp:BoundField HeaderText="sg1_f42" DataField="sg1_f42" />
                                        <asp:BoundField HeaderText="sg1_f43" DataField="sg1_f43" />
                                        <asp:BoundField HeaderText="sg1_f44" DataField="sg1_f44" />
                                        <asp:BoundField HeaderText="sg1_f45" DataField="sg1_f45" />
                                        <asp:BoundField HeaderText="sg1_f46" DataField="sg1_f46" />
                                        <asp:BoundField HeaderText="sg1_f47" DataField="sg1_f47" />
                                        <asp:BoundField HeaderText="sg1_f48" DataField="sg1_f48" />
                                        <asp:BoundField HeaderText="sg1_f49" DataField="sg1_f49" />
                                        <asp:BoundField HeaderText="sg1_f50" DataField="sg1_f50" />


                                    </Columns>
                                    <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <EmptyDataTemplate>
                                        <asp:Image ID="imgdata" runat="server" ImageUrl="~/tej-base/images/nodata.gif" AlternateText="No Data Exists For the Selected Criteria" />
                                    </EmptyDataTemplate>
                                </fin:CoolGridView>
                            </div>
                        </div>
                    </div>
                </div>

                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="~/tej-base/images/excel_icon.png"
                                ToolTip="Export to Excel" Width="30px" Height="30px" OnClick="btnexptoexl_Click" />
                            <asp:ImageButton ID="btnexptocsv" runat="server" ImageUrl="~/tej-base/images/csv_icon.png"
                                ToolTip="Export to CSV" Width="30px" Height="30px" OnClick="btnexptocsv_Click" />
                            <asp:ImageButton ID="btnexptopdf" runat="server" ImageUrl="~/tej-base/images/pdf_icon.png"
                                ToolTip="Export to PDF" Width="30px" Height="30px" OnClick="btnexptopdf_Click" />
                            <asp:ImageButton ID="btnexptoword" runat="server"
                                ImageUrl="~/tej-base/images/Word-2-icon.png" ToolTip="Export to Word"
                                Style="margin-top: 0" Width="30px" Height="28px" OnClick="btnexptoword_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hfqry" runat="server" />
        <asp:HiddenField ID="hfLevel" runat="server" />
        <asp:HiddenField ID="hdata" runat="server" />
        <asp:Button ID="btnhide11" runat="server" OnClick="btnBack_Click" Style="display: none" />
    </form>
</body>
</html>
