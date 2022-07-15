<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_vch_view" Title="Tejaxo" CodeFile="om_vch_view.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style2 {
            width: 69px;
        }

        .style3 {
            width: 70px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="bSubBlock brandSecondaryBrd secondaryPalette" align="left" style="background-image: url('css/images/bgTop.gif');">

        <div class="content-wrapper">
            <section class="content-header">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>

                        <td style="text-align:right">
                            <button id="btnview" runat="server" accesskey="v" class="btn btn-info" style="width: 100px" onserverclick="btnview_ServerClick"><u>V</u>iew</button>
                            <button id="btnlist" runat="server" accesskey="t" class="btn btn-info" style="width: 100px" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                            <button id="btncan" runat="server" accesskey="c" class="btn btn-info" style="width: 100px" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
                            <button id="btnext" runat="server" accesskey="x" class="btn btn-info" style="width: 100px" onserverclick="btnext_ServerClick">E<u>x</u>it</button>
                        </td>
                    </tr>
                </table>
            </section>

            <section class="content">
                <div class="row">
                    <div class="col-md-6">
                        <div>
                            <div class="box-body">

                                <div class="form-group">
                                    <label id="mrheading" runat="server" class="col-sm-3 control-label">Voucher</label>
                                    <div class="col-sm-1">
                                        <asp:ImageButton ID="btnvchnum" runat="server" ToolTip="Preview MRR"
                                            ImageUrl="~/tej-base\css/images/bdsearch5.png" ReadOnly="true"
                                            Style="width: 25px; height: 25px; float: right" OnClick="btnvchnum_Click" />
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtvchnum" runat="server" Width="100%" placeholder="Voucher No" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>

                                    </div>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtvchdt" runat="server" Width="100%" placeholder="Voucher Date" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label id="Label3" runat="server" class="col-sm-4 control-label">Party Code/ Name</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtacode" runat="server" Width="100%" placeholder="Party Code" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtaname" runat="server" Width="100%" placeholder="Name" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div>
                            <div class="box-body">
                                <div class="form-group">
                                    <label id="Type" runat="server" class="col-sm-3 control-label">Type/Name</label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txttype" runat="server" Width="100%" placeholder="Type" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-7">
                                        <asp:TextBox ID="txttypename" runat="server" Width="100%" placeholder="Name" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label id="billdate" runat="server" class="col-sm-3 control-label">Bill No./Date</label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtbillno" runat="server" Width="100%" placeholder="Bill No." onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtbilldt" runat="server" Width="100%" placeholder="Date" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
                                    </div>
                                    <label id="Amount" runat="server" class="col-sm-2 control-label">Amount</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtamt" runat="server" Width="100%" placeholder="Amount" onfocus="Change(this, event)" onblur="Change(this, event)" ReadOnly="true" Height="30px"></asp:TextBox>
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
                                        <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                            <asp:GridView ID="sg1" Width="100%" runat="server" ForeColor="#333333"
                                                Style="background-color: #FFFFFF; color: White;" OnRowCommand="sg1_RowCommand" AutoGenerateColumns="false">
                                                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White"
                                                    CssClass="GridviewScrollHeader" />
                                                <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>R</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base\images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" Style="display: none" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="11px" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="srno" HeaderText="Sr No." ReadOnly="True">
                                                        <ItemStyle Width="30px" />
                                                    </asp:BoundField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>D</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" ImageUrl="~/tej-base\images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="30px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>V</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnview" runat="server" CommandName="View" ImageUrl="~/tej-base\images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="30px" />
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="filno" HeaderText="File Name" ReadOnly="True"></asp:BoundField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>

                </div>
            </section>
        </div>



        <%--<div class="toolsContentLeft">
                   <div class="bSubBlock brandSecondaryBrd secondaryPalette" style="background-image: url('css/images/bgTop.gif');">
                <div align="left" style="color: #1797c0; background-image: url(images/bgTop.gif); font-size: medium; font-weight: bold;">
                                                    &nbsp;<asp:Image ID="Image1" runat="server" Height="24px" ImageUrl="~/css/images/transfer.jpg" Width="28px" />
                                                    <asp:Label ID="lblheader" runat="server"></asp:Label>
                                                    </div>
<div class="toolsContentLeft">
            <div class="bSubBlock brandSecondaryBrd secondaryPalette">
                <div class="lbBody">
                <table width="100%">
                <tr style="background-color: #CDE8F0">
                <td>
                <span id="mrheading" runat="server"></span>
                <asp:ImageButton ID="btnvchnum" runat="server" ToolTip="Preview MRR"
                ImageUrl="~/css/images/info.png" ReadOnly="true"
                        style="width:25px; height:25px; float:right" onclick="btnvchnum_Click" />
                </td>
                <td class="style2">
                <asp:TextBox ID="txtvchnum1" runat="server" Width="70px" placeholder="Voucher No" ReadOnly="true"></asp:TextBox>
                </td>
                <td><asp:TextBox ID="txtvchdt1" runat="server" Width="80px" placeholder="Voucher Date" ReadOnly="true"></asp:TextBox></td>
                <td>Type
                    / Name</td>
                <td class="style3">
                <asp:TextBox ID="txttype1" runat="server" Width="70px" ReadOnly="true" placeholder="Type" ></asp:TextBox>
                </td>
                <td colspan="3"><asp:TextBox ID="txttypename1" runat="server" Width="90%" ReadOnly="true" placeholder="Type Name"></asp:TextBox></td>
                </tr>
                <tr>
                <td>Party Code / Name</td>
                <td class="style2">
                    <asp:TextBox ID="txtacode1" runat="server" Width="70px" ReadOnly="true" placeholder="Code"></asp:TextBox>
                    </td>
                <td >
                <asp:TextBox ID="txtaname1" runat="server" Width="300px" ReadOnly="true" placeholder="Name"></asp:TextBox>
                </td>
                <td>Bill No. / Date</td>
                <td class="style3">
                <asp:TextBox ID="txtbillno1" runat="server" Width="70px" placeholder="Bill No" ReadOnly="true"></asp:TextBox>
                    </td>
                <td><asp:TextBox ID="txtbilldt1" runat="server" Width="80px" ReadOnly="true"
                        placeholder="Bill Date"></asp:TextBox></td>
                <td>Amount</td>
                <td><asp:TextBox ID="txtamt1" runat="server" Width="80px" style="text-align:right" ReadOnly="true"
                        placeholder="Amount"></asp:TextBox></td>
                </tr>

                <tr>
                <td class="style1" colspan="8">
                <div class="lbBody" style="color:White; height:180px; max-height:180px; overflow:auto; box-shadow:0 2px 4px rgba(127,127,127,.3);box-shadow:inset 0 0 3px #387bbe,0 0 9px #387bbe;">
        <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="false"
CellPadding="2" ForeColor="#333333"
        GridLines="Both" style="background-color: #FFFFFF; color:White; font-size: small;"
                        onrowdatabound="sg1_RowDataBound" onrowcommand="sg1_RowCommand"  >
<RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem"  />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager"/>
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
        <EditRowStyle BackColor="#999999"  />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
<HeaderStyle BackColor="#1797C0" ForeColor="White" Height="20px"/>
<RowStyle CssClass="grdrow" />
<Columns>
<asp:TemplateField>
<HeaderTemplate>R</HeaderTemplate>
<ItemTemplate>
<asp:ImageButton ID="btnrmv1" runat="server" CommandName="Rmv" ImageUrl="~/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" />
    </ItemTemplate>
<ItemStyle Width="11px" />
</asp:TemplateField>

<asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True" >
    <ItemStyle Width="10px" />
    </asp:BoundField>

    <asp:TemplateField>
<HeaderTemplate>D</HeaderTemplate>
<ItemTemplate>
<asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
    </ItemTemplate>
<ItemStyle Width="11px" />
</asp:TemplateField>

<asp:TemplateField>
<HeaderTemplate>V</HeaderTemplate>
<ItemTemplate>
<asp:ImageButton ID="btnview" runat="server" CommandName="View" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
    </ItemTemplate>
<ItemStyle Width="11px" />
</asp:TemplateField>

    <asp:BoundField DataField="filno" HeaderText="File Name" ReadOnly="True" >
    </asp:BoundField>
</Columns>
            </asp:GridView>
         </div>
                </td>
                </tr>
                </table>
                </div>
                </div>
                </div>
                </div>
                </div>
                </div>--%>
        <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
        <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="edmode" runat="server" />
        <asp:HiddenField ID="hf_form_mode" runat="server" />

    </div>
</asp:Content>
