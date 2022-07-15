<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_file_atch" Title="Tejaxo" CodeFile="om_file_atch.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
        function closePopup() { parent.$.colorbox.close() }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtdocno" runat="server" Visible="false"></asp:TextBox>
                        <asp:TextBox ID="txtdate" runat="server" Visible="false"></asp:TextBox>

                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                </tr>
                <tr>
                    <td col2="2">
                        <asp:Label ID="lblHeading" runat="server" Font-Size="Medium"></asp:Label>
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
                                <li>
                                    <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />
                                </li>
                                <li>
                                    <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" Style="display: none"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Button ID="btnAtt" runat="server" Text="File" OnClick="btnAtt_Click" Width="50px" Style="display: none" />
                                </li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" Width="100%" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>R</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base\images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                    <ItemStyle Width="10px" />
                                                    <HeaderStyle Width="10px" />
                                                </asp:BoundField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>D</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" CommandArgument='<%# Eval("filename") %>' ImageUrl="~/tej-base\images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>V</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnview" runat="server" CommandName="View" CommandArgument='<%# Eval("filename") %>' ImageUrl="~/tej-base\images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="filename" HeaderText="File Name"></asp:BoundField>
                                                <asp:BoundField DataField="fileorgname" HeaderText="File Original Name (Max 80 Char)"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks (100 char)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRmk" runat="server" Value='<%#Eval("remarks") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="TabName" runat="server" />
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
