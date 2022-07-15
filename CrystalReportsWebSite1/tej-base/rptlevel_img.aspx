<%@ Page Language="C#" AutoEventWireup="true" Inherits="rptlevel_img" CodeFile="rptlevel_img.aspx.cs" %>

<!DOCTYPE html>

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

    <script src="Scripts/temp.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).keyup(function (event) {
            debugger;
            if (event.keyCode == 27) {
                parent.$.colorbox.close();
                return;
            }
        });
    </script>

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="content" style="margin-top: 35px;">
            <section class="content-header">
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
                    <tr style="background-color:#ecf0f5">
                        <td colspan="6">
                            <section class="content" >
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:ListView ID="ListBox1" runat="server">
                                            <ItemTemplate>
                                                <div>
                                                    <div class="box-body">
                                                        <div class="form-group">
                                                            <%--<div style="display: none">
                                                <asp:Label ID="lbl" runat="server" Text='<%# Eval("fstr") %>'></asp:Label>
                                            </div>--%>
                                                            <div class="col-sm-6">
                                                                <%# Eval("field1") %>
                                                            </div>
                                                            <div class="col-sm-6">
                                                                <asp:Image ID="imgViewer" runat="server" ImageUrl='<%#Eval("imgsrc") %>' Width="250px" Height="150px" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                            </section>

                            <fin:CoolGridView ID="GridView1" runat="server"
                                Style="font-family: Arial, Helvetica, sans-serif; font-size: small; display: none" OnPageIndexChanging="GridView1_PageIndexChanging"
                                PageSize="200" AllowPaging="true" Height="450px" Width="1200px" OnRowDataBound="GridView1_RowDataBound">
                                <PagerSettings Position="Bottom" />

                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader2" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
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
            </section>
        </div>
        <asp:HiddenField ID="hfqry" runat="server" />
        <asp:HiddenField ID="hdata" runat="server" />
        <asp:Button ID="btnhide" runat="server" OnClick="btnhide_Click" Style="display: none" />
    </form>
</body>
</html>
