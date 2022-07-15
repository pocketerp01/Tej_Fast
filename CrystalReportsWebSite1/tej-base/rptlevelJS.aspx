<%@ Page Language="C#" AutoEventWireup="true" Inherits="rptlevelJS" CodeFile="rptlevelJS.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <script src="Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/jquery.handsontable.full.js"></script>
    <link rel="Stylesheet" type="text/css" href="Styles/jquery.handsontable.full.css" />

    <%--<script src="../tej-base/scripts/jquery.min.js" type="text/javascript"></script>--%>
    <script src='<%=ResolveUrl("~/crystalreportviewers13/js/crviewer/crv.js")%>' type="text/javascript"></script>

    <link href="Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <link href="css/GridviewScroll2.css" rel="stylesheet" type="text/css" />
    <%--<link href="Styles/fin.css" rel="stylesheet" type="text/css" />--%>

    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />

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
    </style>

    <script src="Scripts/temp.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).keyup(function (event) {
            debugger;
            if (event.keyCode == 27) {
                parent.$.colorbox.close();
                return;
            }
            //function exptoexcel(o) { $(o, window.parent.document).trigger("click"), parent.$.colorbox.close() }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 35px;">
            <table width="100%">
                <tr>
                    <td>
                        <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch"
                            placeholder="Enter here to search" ToolTip="Enter here to search" OnTextChanged="txtsearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                        <asp:ImageButton ID="srch" runat="server" ImageUrl="~/tej-base/images/search-button.png"
                            Width="120px" Height="27px" ToolTip="Click to Search" OnClick="srch_Click" Style="display: none" />
                    </td>
                    <td>
                        <asp:Label ID="lblTitle" runat="server" Text="Note : PL standes for Process Loss, Closing Stock Includes Process Loss" Font-Size="Smaller" Visible="false"></asp:Label>
                    </td>
                    <td style="float: right">
                        <span class="font_css">Show Rows</span>&nbsp;&nbsp;
                        <asp:TextBox ID="tkrow" runat="server" Width="40px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)" OnTextChanged="tkrow_TextChanged"
                            Style="text-align: right;" Text="200" AutoPostBack="true"></asp:TextBox>
                        &nbsp;&nbsp;
                    <asp:Label ID="lblTotcount" runat="server" Style="font-size: 10px; font-family: 'Arial Unicode MS'"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" id="tdDiv" runat="server">
                        <div id="datadiv" style="overflow: scroll; width: auto; height: 470px;" runat="server" class="handsontable"
                            data-originalstyle="width: auto; height: 470px; overflow: scroll">
                        </div>

                        <div id="div2" runat="server" align="center" style="margin-top: 20px">
                            No Data Found
                        </div>
                    </td>
                    <td colspan="3" id="tdGrid" runat="server">
                        <fin:CoolGridView ID="GridView1" runat="server"
                            Style="font-family: Arial, Helvetica, sans-serif; font-size: small;" OnPageIndexChanging="GridView1_PageIndexChanging"
                            CellPadding="1" ForeColor="#333333" AutoGenerateColumns="false"
                            PageSize="200" AllowPaging="true" Height="450px" OnRowDataBound="GridView1_RowDataBound">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="4" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Previous" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle CssClass="pagination-ys" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader2" />
                            <EditRowStyle BackColor="#999999" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                            <EmptyDataTemplate>
                                <asp:Image ID="imgdata" runat="server" ImageUrl="~/tej-base/images/nodata.gif" AlternateText="No Data Exists For the Selected Criteria" />
                            </EmptyDataTemplate>
                            <Columns>

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
                        </fin:CoolGridView>

                    </td>
                </tr>
            </table>
            <table style="padding-top: 10px;">
                <tr>
                    <td style="display: none">
                        <span style="font-size: small; font-weight: 700; font-family: 'Courier New', Courier, monospace; padding-left: 10px; color: #1797c0">Ctrl + E for<br />
                            export to Excel</span>
                    </td>
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
                        <asp:ImageButton ID="btnHelp" runat="server" ImageUrl="~/tej-base/images/help2.png" OnClick="btnHelp_Click"
                            Style="margin-top: 0" Width="30px" Height="32px" ToolTip="Help on this Report" />
                        <asp:ImageButton ID="btnPrint" runat="server" ImageUrl="~/tej-base/images/print_btn.ico" OnClick="btnPrint_Click"
                            Style="margin-top: 0" Width="30px" Height="32px" ToolTip="Print this Report" />
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
        <asp:HiddenField ID="hfqry" runat="server" />
        <asp:HiddenField ID="hdata" runat="server" />
        <asp:HiddenField ID="hfLevel" runat="server" />
        <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />
    </form>
</body>
</html>
