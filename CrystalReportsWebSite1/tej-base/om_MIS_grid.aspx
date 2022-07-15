<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_MIS_grid" CodeFile="om_MIS_grid.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../tej-base/Scripts/highcharts.js" type="text/javascript"> </script>
    <script src="../tej-base/Scripts/highcharts-more.js" type="text/javascript"> </script>
    <script src="../tej-base/Scripts/exporting.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
        });
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
            UpperBound = parseInt('<%= this.sg1.Rows.Count %>') - 1;
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

            if (KeyCode == 40) SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38) SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);

            return false;
        }
    </script>

    <style type="text/css">
        .grad {
            background-image: linear-gradient(to right,#f1e847,#f3ebab);
            padding: 2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="22px"></asp:Label>
                        <asp:Label ID="lblDate" runat="server" Style="font-size: 10px;"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSearch" runat="server" Style="float: right" placeholder="Search..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" CssClass="form-control" Height="28px"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 70px;" runat="server" accesskey="s" onserverclick="btnnew_ServerClick"><u>S</u>how</button>
                        <button type="submit" id="Graph" class="btn btn-info" style="width: 70px;" runat="server" accesskey="X" onserverclick="Graph_ServerClick">G<u>r</u>aph</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 70px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 70px;" runat="server" onserverclick="btnsave_ServerClick"><u>E</u>xcel</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 70px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 70px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 70px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Sta<u>t</u>us</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 70px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 70px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12" id="div1" runat="server">
                    <div style="padding-left: 5px">
                        <div class="box-header" style="display: none">
                            <div style="text-align: left" class="col-sm-4">
                                <asp:Label ID="lblTotcount" runat="server" Style="font-size: 14px;" Font-Bold="True"></asp:Label>

                                <asp:Label ID="lblPageCount" runat="server" Style="font-size: 14px;" Font-Bold="True"></asp:Label>
                            </div>
                            <div style="text-align: center" class="col-sm-4">
                            </div>
                            <div class="col-md-4">
                            </div>
                            <h3 class="box-title" style="display: none">
                                <b>
                                    <asp:Label ID="lblSg1" runat="server" CssClass="grad" Visible="false"></asp:Label>
                                </b>
                                <br />
                            </h3>
                        </div>

                        <div id="divMISReportsButton" runat="server">
                            <asp:Button ID="Button1" runat="server" CssClass="btn-warning" Text="Pending PO" OnClick="Button1_Click" />
                            <asp:Button ID="Button2" runat="server" CssClass="btn-danger" Text="Pending PO" OnClick="Button2_Click" />
                            <asp:Button ID="Button3" runat="server" CssClass="btn-success" Text="Pending PO" OnClick="Button3_Click" />
                            <asp:Button ID="Button4" runat="server" CssClass="btn-success" Text="Pending PO" OnClick="Button1_Click" />
                            <asp:Button ID="Button5" runat="server" CssClass="btn-success" Text="Pending PO" OnClick="Button2_Click" />
                            <asp:Button ID="Button6" runat="server" CssClass="btn-default" Text="Pending PO" OnClick="Button3_Click" />

                            <asp:Button ID="btnRefresh" runat="server" CssClass="btn-success" Text="Refresh" Style="margin-left: 70px;" Width="120px" OnClick="btnRefresh_Click" />
                        </div>
                        <div id="divMISReportsFilter" runat="server">
                            <asp:ImageButton ID="btnFilter1" runat="server" Style="vertical-align: middle" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="btnFilter1_Click" />
                            <asp:TextBox ID="txtFilter1" runat="server" Width="70px"></asp:TextBox>

                            <asp:ImageButton ID="btnFilter2" runat="server" Style="vertical-align: middle" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="btnFilter2_Click" />
                            <asp:TextBox ID="txtFilter2" runat="server" Width="70px"></asp:TextBox>

                            <asp:ImageButton ID="btnFilter3" runat="server" Style="vertical-align: middle" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="btnFilter3_Click" />
                            <asp:TextBox ID="txtFilter3" runat="server" Width="70px"></asp:TextBox>

                            <asp:ImageButton ID="btnFilter4" runat="server" Style="vertical-align: middle" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="btnFilter4_Click" />
                            <asp:TextBox ID="txtFilter4" runat="server" Width="70px"></asp:TextBox>

                            <asp:ImageButton ID="btnFilter5" runat="server" Style="vertical-align: middle" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="btnFilter5_Click" />
                            <asp:TextBox ID="txtFilter5" runat="server" Width="70px"></asp:TextBox>

                            <asp:ImageButton ID="btnFilter6" runat="server" Style="vertical-align: middle" ImageUrl="../tej-base/css/images/bdsearch5.png" OnClick="btnFilter6_Click" />
                            <asp:TextBox ID="txtFilter6" runat="server" Width="70px"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv" style="color: White; height: 400px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                        Width="100%" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg1_RowDataBound" OnSelectedIndexChanged="sg1_SelectedIndexChanged"
                                        OnRowCommand="sg1_RowCommand" OnRowCreated="sg1_RowCreated" OnPageIndexChanging="sg1_PageIndexChanging" PageSize="18" AllowPaging="true" AllowSorting="True" OnSorting="sg1_Sorting">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="~/tej-base/images/tick.png">
                                                <HeaderStyle Width="25px"></HeaderStyle>
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="#f3ebf1" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                        <PagerSettings FirstPageImageUrl="~/tej-base/css/images/lefts.png" LastPageImageUrl="~/tej-base/css/images/rights.png" Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                        <SelectedRowStyle BackColor="#e0e0e0" Font-Bold="True" ForeColor="#333333" />
                                    </fin:CoolGridView>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="box box-success" style="background-color: burlywood">
                        <asp:Label ID="lblxx" runat="server" Text="Line Chart : Click the Code   |   Drill Down : Click Name      " Font-Size="Small" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
