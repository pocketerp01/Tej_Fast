<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="parta_rpt" Title="Tejaxo" CodeFile="parta_rpt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
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
    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <button id="btnnew" runat="server" accesskey="N" class="btn btn-info" onserverclick="btnnew_ServerClick" style="width: 100px; display: none;"><u>N</u>ew</button>
                        <button id="btnedit" runat="server" accesskey="i" class="btn btn-info" onserverclick="btnedit_ServerClick" style="width: 100px; display: none;">Ed<u>i</u>t</button>
                        <button id="btnsave" runat="server" accesskey="S" class="btn btn-info" onserverclick="btnsave_ServerClick" style="width: 100px"><u>S</u>how</button>
                        <button id="btndel" runat="server" accesskey="l" class="btn btn-info" onserverclick="btndel_ServerClick" style="width: 100px; display: none;">De<u>l</u>ete</button>
                        <button id="btnlist" runat="server" accesskey="t" class="btn btn-info" onserverclick="btnlist_ServerClick" style="width: 100px; display: none;">Lis<u>t</u></button>
                        <button id="btnprint" runat="server" accesskey="P" class="btn btn-info" onserverclick="btnprint_ServerClick" style="width: 100px; display: none;"><u>P</u>rint</button>
                        <button id="btncan" runat="server" accesskey="c" class="btn btn-info" style="width: 100px" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
                        <button id="btnext" runat="server" accesskey="x" class="btn btn-info" style="width: 100px" onserverclick="btnext_ServerClick">E<u>x</u>it</button>
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Style="float: right"></asp:Label>
                    </td>
                </tr>

            </table>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtsrch" runat="server" AutoPostBack="true" OnTextChanged="txtsrch_TextChanged"
                                        placeholder="Search here..." Width="100%" Height="32px"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="~/tej-base\images/excel_icon.png"
                                        ToolTip="Export to Excel" Width="30px" Height="30px" OnClick="btnexptoexl_Click" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="GridView1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="400px" Font-Size="13px"
                                            OnRowDataBound="GridView1_RowDataBound" OnRowCreated="GridView1_RowCreated"
                                            OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AutoGenerateColumns="false">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="13px" CssClass="GridviewScrollHeader2" />
                                            <EditRowStyle BackColor="#999999" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:CommandField ButtonType="Image" HeaderText="Sel" HeaderStyle-Width="25px" ShowSelectButton="True"
                                                    SelectImageUrl="~/tej-base/images/tick.png">
                                                    <ItemStyle Width="25px"></ItemStyle>
                                                </asp:CommandField>


                                                <asp:BoundField HeaderText="fstr" DataField="fstr" />
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
                                                <asp:Image ID="imgdata" runat="server" ImageUrl="~/tej-base/images/nodata.gif" AlternateText="No Data Exist" />
                                            </EmptyDataTemplate>
                                        </fin:CoolGridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>

    </div>



    <asp:TextBox ID="txtvchnum" runat="server" Style="display: none"></asp:TextBox>
    <asp:TextBox ID="txtvchdate" runat="server" Style="display: none"></asp:TextBox>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />

    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
</asp:Content>
