<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_dbd_mgrid" CodeFile="om_dbd_mgrid.aspx.cs" %>

<%@ Register Assembly="IdeaSparx.CoolControls.Web" Namespace="IdeaSparx.CoolControls.Web"
    TagPrefix="vv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 2);
            //calculateSum();
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth,
                height: gridDiv.offsetHeight,
                headerrowcount: headerFreeze,
                freezesize: rowFreeze,

                barhovercolor: "#3399FF",
                barcolor: "#3399FF",

                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }
            });
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
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>

                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnnew_ServerClick"><u>S</u>how</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Sta<u>t</u>us</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnClose" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btnClose_ServerClick">Close Job</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-7">
                    <div>
                        <div class="box-header">
                            <h4 class="box-title">
                                <b>
                                    <asp:Label ID="lblSg1" runat="server" CssClass="grad"></asp:Label>
                                </b>
                                <asp:Label ID="lblTotcount" runat="server" Style="font-size: smaller;"></asp:Label>
                            </h4>
                            <span style="float: right; padding-right: 10px;">
                                <asp:ImageButton ID="btnExpToExcel" runat="server" OnClick="btnExpToExcel_Click" ImageUrl="~/tej-base/images/excel_icon.png"
                                    ToolTip="Export to Excel" Width="25px" Height="25px" />
                            </span>
                            <span style="float: right; padding-right: 10px;">
                                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" CssClass="form-control" Height="24px"></asp:TextBox>
                            </span>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv" style="color: White; height: 508px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                        Width="1500px" Font-Size="Small"
                                        AutoGenerateColumns="true" OnRowDataBound="sg1_RowDataBound" OnSelectedIndexChanged="sg1_SelectedIndexChanged"
                                        OnRowCommand="sg1_RowCommand" OnRowCreated="sg1_RowCreated" OnPageIndexChanging="sg1_PageIndexChanging" PageSize="30" AllowPaging="true">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <HeaderStyle Width="25px"></HeaderStyle>

                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="#f3ebf1" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerSettings FirstPageImageUrl="~/tej-base/css/images/lefts.png" LastPageImageUrl="~/tej-base/css/images/rights.png" Mode="NumericFirstLast" />
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#e0e0e0" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" id="grid2" runat="server">
                    <div>
                        <div class="box-header">
                            <h3 class="box-title grad">
                                <b>
                                    <asp:Label ID="lblSg2" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkSg2" runat="server" OnClick="lnkSg2_Click" Text="(CLICK)"></asp:LinkButton>
                                </b>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv1" runat="server" style="color: White; height: 115px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:HiddenField ID="SelectedGridCellIndex2" runat="server" Value="-1" />
                                    <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="400px" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg2_RowDataBound" OnSelectedIndexChanged="sg2_SelectedIndexChanged"
                                        OnRowCreated="sg2_RowCreated">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" id="grid3" runat="server">
                    <div>
                        <div class="box-header">
                            <h3 class="box-title grad">
                                <b>
                                    <asp:Label ID="lblSg3" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkSg3" runat="server" OnClick="lnkSg3_Click" Text="(CLICK)"></asp:LinkButton>
                                </b>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv3" runat="server" style="color: White; height: 115px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:HiddenField ID="SelectedGridCellIndex3" runat="server" Value="-1" />
                                    <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="400px" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg3_RowDataBound" OnSelectedIndexChanged="sg3_SelectedIndexChanged" OnRowCreated="sg3_RowCreated">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" id="grid4" runat="server">
                    <div>
                        <div class="box-header">
                            <h3 class="box-title grad">
                                <b>
                                    <asp:Label ID="lblSg4" runat="server"></asp:Label>
                                </b>
                            </h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv4" runat="server" style="color: White; height: 115px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:HiddenField ID="SelectedGridCellIndex4" runat="server" Value="-1" />
                                    <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="400px" Font-Size="Smaller"
                                        AutoGenerateColumns="true" OnRowDataBound="sg4_RowDataBound" OnSelectedIndexChanged="sg4_SelectedIndexChanged" OnRowCreated="sg4_RowCreated">
                                        <Columns>
                                            <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                SelectImageUrl="images/tick.png">
                                                <ItemStyle Width="25px"></ItemStyle>
                                            </asp:CommandField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
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
