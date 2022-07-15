<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="wppldashbord" CodeFile="wppldashbord.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .yourclass {
            display: inline-block;
            float: right;
            width: 100%;
            border: groove;
            background-color: aliceblue;
            border-bottom-left-radius: 10px;
        }

        .grid {
            border: 3px solid black;
            border-color: #1797c0;
        }

        .auto-style1 {
            border: 3px solid #1797c0;
            width: 102%;
        }

        .auto-style2 {
            width: 102%;
        }
    </style>
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <button id="btnsave" runat="server" accesskey="S" class="btn btn-info" style="width: 100px" onserverclick="btnsave_ServerClick">
                            <u>S</u>tart</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" Style="width: 100px" OnClick="btnext_Click" />
                    </td>
                    <td>
                        <asp:Label ID="lblheader" Text="Dash Board for Task Action Management"  Font-Size="Large"
                            runat="server"></asp:Label>
                        <asp:Label ID="Label1" runat="server" Style="float: right"></asp:Label>
                    </td>
                </tr>
            </table>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <table style="border: groove">
                                <tr style="border: groove; width: 100%">
                                    <td class="auto-style1">
                                        <asp:Label ID="lbldash" runat="server" Width="100px"></asp:Label>
                                        <div class="lbBody" style="color: White; height: 390px; max-height: 380px; max-width: 1300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                            <asp:GridView ID="sg1" runat="server" ForeColor="#333333" Style="background-color: red; color: White;"
                                                Width="100%" Font-Size="Smaller" AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                                ShowHeader="True">
                                                <HeaderStyle BackColor="#ff33cc" Font-Bold="True" ForeColor="White" Font-Size="1.2em"
                                                    Height="30" />
                                                <%-- <ItemStyle BackColor="Yellow" /></ItemStyle>--%>
                                                <AlternatingRowStyle BackColor="#ff33cc" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="sg1_srno" HeaderText="Srno" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f1" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f2" HeaderText="" ReadOnly="true" ItemStyle-Width="200px" />
                                                    <asp:BoundField DataField="sg1_f3" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f4" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f5" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f6" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f7" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f8" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f9" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f10" HeaderText="" ReadOnly="true" />
                                                    <asp:BoundField DataField="sg1_f11" HeaderText="" ReadOnly="true" />
                                                </Columns>
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="border: groove; width: 100%">
                                    <td class="auto-style2"></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <asp:HiddenField ID="hffromdt" runat="server" />
    <asp:HiddenField ID="hftodt" runat="server" />
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
</asp:Content>
