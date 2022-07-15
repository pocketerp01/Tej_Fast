<%@ Page Language="C#" AutoEventWireup="true" Inherits="om_bom_tree" MasterPageFile="~/tej-base/Fin_Master2.master" CodeFile="om_bom_tree.aspx.cs" %>

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
                        <asp:Label ID="lblitem" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblicode" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                    </td>
                    <td>
                        <button type="submit" id="Button1" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
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
                            <div class="lbBody">
                                <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                    <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Height="480px" Font-Size="13px"
                                        AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="icode" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="iname" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="unit" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="ibqty" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="cpartno" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="cdrgno" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="ent_dt" />
                                            <asp:BoundField ItemStyle-Width="150px" HeaderStyle-Width="150px" DataField="ent_by" />
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
</asp:Content>
