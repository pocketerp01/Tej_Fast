<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_truck_imgview" CodeFile="om_truck_imgview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-6" style="display: none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label">EntryNo.</label>
                                <div class="col-sm-8">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-4 control-label">Date</label>
                                <div class="col-sm-8">
                                    <input id="txtvchdate" type="date" class="form-control" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" style="display: none">
                    <div>
                        <div class="box-body">
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                    Style="background-color: #FFFFFF; color: White;" Width="100%" Height="450px" Font-Size="13px"
                                    AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound"
                                    OnRowCommand="sg1_RowCommand">
                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                    <Columns>
                                        <asp:BoundField DataField="sg1_t1" HeaderText="Entry No." />
                                        <asp:BoundField DataField="sg1_t2" HeaderText="Entry Dt." />
                                        <asp:BoundField DataField="sg1_t3" HeaderText="Truck Vendor" />
                                        <asp:BoundField DataField="sg1_t4" HeaderText="Truck No." />
                                        <asp:BoundField DataField="sg1_t5" HeaderText="Truck In Time" />
                                        <asp:BoundField DataField="sg1_t6" HeaderText="Ent By" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>View</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnview1" runat="server" CommandName="SG1_VIEW1" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="sg1_t7" HeaderText="Loading Time" />
                                        <asp:BoundField DataField="sg1_t8" HeaderText="Loading By" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>View</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnview2" runat="server" CommandName="SG1_VIEW2" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="sg1_t9" HeaderText="Loading Completed" />
                                        <asp:BoundField DataField="sg1_t10" HeaderText="Ent By" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>View</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnview3" runat="server" CommandName="SG1_VIEW3" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="sg1_t11" HeaderText="Truck Out" />
                                        <asp:BoundField DataField="sg1_t12" HeaderText="Out By" />

                                        <asp:TemplateField>
                                            <HeaderTemplate>View</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnview4" runat="server" CommandName="SG1_VIEW4" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="sg1_t13" HeaderText="Path" />
                                        <asp:BoundField DataField="sg1_t14" HeaderText="Path" />
                                        <asp:BoundField DataField="sg1_t15" HeaderText="Path" />
                                        <asp:BoundField DataField="sg1_t16" HeaderText="Path" />

                                    </Columns>
                                    <EditRowStyle BackColor="#999999" />
                                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
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
    <script type="text/javascript">

        <%--function run() {
            var grid = $("[id*=sg1].GridviewScrollItem2").length;
            var gridid = document.getElementById("<%= sg1.ClientID%>");
            var URL = "";
            for (var i = 0; i < grid; i++) {
                URL = document.getElementById('ContentPlaceHolder1_sg1_ffq_' + i).value;
                window.open(URL, null);
            }


            //// 8887 is the port number you have launched your serve
            //var URL = "http://127.0.0.1:8887/002.jpg";

            //window.open(URL, null);

        }
        run();--%>
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
