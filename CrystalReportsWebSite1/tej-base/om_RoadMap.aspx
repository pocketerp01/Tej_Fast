<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_RoadMap" EnableEventValidation="false" CodeFile="om_RoadMap.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <%--<asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>--%>
                        <%--<img src="../tej-base/images/shopworkload.jpeg" />--%>
                    </td>
                    <td style="text-align: right">
                        <%--<button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>--%>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnlist_ServerClick">A<u>s</u>sign Task</button>
                        <%--<button type="submit" id="btnCamera" class="btn btn-info" style="width: 100px;" runat="server" accesskey="A" onserverclick="btnCamera_ServerClick">C<u>a</u>mera</button>--%>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 150px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick">V<u>i</u>ew Task Status</button>

                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                </tr>
            </table>
        </section>
    </div>

    <div class="content-wrapper">

        <section class="content">
            <div class="row">

                <div class="col-lg-2 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Height="500px" Width="100%" Font-Size="Larger"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand" OnSelectedIndexChanged="sg1_SelectedIndexChanged">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>


                                                <%--<asp:BoundField DataField="sg1_Srno" HeaderText="Sr.No" ItemStyle-HorizontalAlign="right" />--%>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="MileStones" HeaderStyle-BackColor="YellowGreen" ItemStyle-Height="36px" />

                                                <asp:CommandField SelectText="Select" ShowSelectButton="true" Visible="false" />
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>


                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-lg-10 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs1" role="tabpanel">
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg2" runat="server" Width="100%" Height="500px" Font-Size="Larger"
                                            AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:BoundField DataField="sg2_f1" HeaderText="Particulars" HeaderStyle-Width="400px" HeaderStyle-BackColor="YellowGreen" ItemStyle-Height="60px" />
                                                <asp:BoundField DataField="sg2_f2" HeaderText="Target" HeaderStyle-Width="300px" HeaderStyle-BackColor="YellowGreen" ItemStyle-Height="60px" />
                                                <asp:BoundField DataField="sg2_f3" HeaderText="Actual " HeaderStyle-Width="300px" HeaderStyle-BackColor="YellowGreen" ItemStyle-Height="60px" />
                                                <asp:BoundField DataField="sg2_f4" HeaderText="% Done" HeaderStyle-Width="300px" HeaderStyle-BackColor="YellowGreen" ItemStyle-Height="60px" />


                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>

                                    </div>
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
    <input id="pwd1" runat="server" style="display: none" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <script type="text/javascript">
        //$(function () {
        //    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
        //    $('#Tabs a[href="#' + tabName + '"]').tab('show');
        //    $("#Tabs a").click(function () {
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //});
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
