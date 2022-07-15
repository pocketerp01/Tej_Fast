<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_item_tree" CodeFile="om_item_tree.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">

        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>

                    <td>
                        <asp:Label ID="lblitem" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label></td>

                    <td>

                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px; float: right;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-4">
                    <div>
                        <div class="box-body" style="max-height: 500px; overflow: auto;">
                            <asp:TreeView ID="TreeView1" runat="server" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
                            </asp:TreeView>
                        </div>
                    </div>
                </div>

                <div class="col-md-8">
                    <div>
                        <div class="box-body">
                            <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                    Style="background-color: #FFFFFF; color: White;" Width="100%" Height="480px" Font-Size="13px"
                                    AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound">

                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                    <Columns>
                                        <asp:BoundField ItemStyle-Width="80px" HeaderStyle-Width="80px" DataField="icode" HeaderText="Erp Code" />
                                        <asp:BoundField ItemStyle-Width="300px" HeaderStyle-Width="300px" DataField="iname" HeaderText="Product" />
                                        <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="cpartno" HeaderText="Part No." />
                                        <asp:BoundField ItemStyle-Width="40px" HeaderStyle-Width="40px" DataField="unit" HeaderText="UOM" />
                                        <asp:BoundField ItemStyle-Width="70px" HeaderStyle-Width="70px" DataField="cdrgno" HeaderText="Drw No" />
                                        <asp:BoundField ItemStyle-Width="70px" HeaderStyle-Width="70px" DataField="irate" HeaderText="Rate" />
                                        <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="ent_dt" HeaderText="Ent Date" />
                                        <asp:BoundField ItemStyle-Width="70px" HeaderStyle-Width="70px" DataField="ent_by" HeaderText="Ent By" />
                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                    <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                </fin:CoolGridView>

                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </section>
    </div>
</asp:Content>
