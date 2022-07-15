<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="neopappmst" Title="Tejaxo" CodeFile="neopappmst.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 1);
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
    </asp:Content>
    <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="content-wrapper">
            <section class="content-header">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <button id="btnedit" runat="server" accesskey="i" class="btn btn-info" style="width: 100px;" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                            <button id="btnsave" runat="server" accesskey="S" class="btn btn-info" style="width: 100px;" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                            <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" Style="width: 100px;" OnClick="btnext_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblhead" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        </td>
                    </tr>
                </table>
            </section>
            <section class="content">
                <div class="row">
                    <section class="col-lg-12 connectedSortable">
                        <div class="panel panel-default">
                            <div id="Tabs" role="tabpanel">
                                <ul class="nav nav-tabs" role="tablist">
                                    <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                                </ul>

                                <div class="tab-content">
                                    <div role="tabpanel" class="tab-pane active" id="DescTab">

                                        <div class="lbBody" id="gridDiv" style="color: White; max-height: 400px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                            <asp:GridView ID="sg1" Width="1200px" runat="server" ForeColor="#333333"
                                                Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false"
                                                OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound">
                                                <rowstyle backcolor="#F7F6F3" forecolor="#333333" cssclass="GridviewScrollItem" />
                                                <alternatingrowstyle backcolor="White" forecolor="#284775" />
                                                <editrowstyle backcolor="#999999" />
                                                <footerstyle backcolor="#5D7B9D" font-bold="True" forecolor="White" />
                                                <headerstyle backcolor="#1797c0" font-bold="True" forecolor="White"
                                                    cssclass="GridviewScrollHeader" />
                                                <pagerstyle backcolor="#284775" forecolor="White" cssclass="GridviewScrollPager" />
                                                <selectedrowstyle backcolor="#E2DED6" font-bold="True" forecolor="#333333" />
                                                <columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>A</HeaderTemplate>
                                                    <HeaderStyle Width="22px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnadd" runat="server" CommandName="Add" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>                                                    
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>D</HeaderTemplate>
                                                    <HeaderStyle Width="22px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                    
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                    <ItemStyle Width="40px" />
                                                    <HeaderStyle Width="40px" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Name</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtname" runat="server" Text='<%#Eval("name") %>' Width="100%" MaxLength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Rate</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtrate" runat="server" Text='<%#Eval("rate") %>' Width="70px" onkeypress="return isDecimalKey(event)" MaxLength="10" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>tk1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tk1" runat="server" Text='<%#Eval("tk1") %>' Width="70px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>tk2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tk2" runat="server" Text='<%#Eval("tk2") %>' Width="70px" Height="12px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>tk3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tk3" runat="server" Text='<%#Eval("tk3") %>' Width="70px" Height="12px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>tk4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="tk4" runat="server" Text='<%#Eval("tk4") %>' Width="70px" Height="12px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>dd1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="dd1" runat="server">
                                                            <asp:ListItem Text="Manual" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Motor" Value="1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>dd2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="dd2" runat="server">
                                                            <asp:ListItem Text="Roller" Value="0"></asp:ListItem>
                                                            <asp:ListItem Text="Wooden" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="Track" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Zebra" Value="3"></asp:ListItem>
                                                            <asp:ListItem Text="Cellular" Value="4"></asp:ListItem>
                                                            <asp:ListItem Text="Apex" Value="5"></asp:ListItem>
                                                            <asp:ListItem Text="Roman" Value="6"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </columns>
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
        <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="edmode" runat="server" />

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
</asp:Content>
