<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_shopwork_load" EnableEventValidation="false" CodeFile="om_shopwork_load.aspx.cs" %>


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
                        <img src="../tej-base/images/shopworkload.jpeg" />
                    </td>
                    <td style="text-align: right">
                        <%--<button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>--%>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnlist_ServerClick"><u>S</u>how Data</button>
                        <%--<button type="submit" id="btnCamera" class="btn btn-info" style="width: 100px;" runat="server" accesskey="A" onserverclick="btnCamera_ServerClick">C<u>a</u>mera</button>--%>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnJobOrdStat" class="btn btn-info" style="width: 150px;" runat="server" accesskey="l" onserverclick="btnJobOrdStat_ServerClick">Job Order Status</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
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
                                <li><a href="#DescTab4" id="A4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">
                                    <asp:Label runat="server" ID="showcap"></asp:Label>
                                </a></li>
                                <li>&nbsp;&nbsp;
                                    <button type="submit" id="btnTraExc" class="btn-success" style="width: 150px; float: right" runat="server" onserverclick="btnTraExc_ServerClick1">Transfer To Excel</button></li>

                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Height="200px" Width="100%" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand" OnSelectedIndexChanged="sg1_SelectedIndexChanged">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_Srno" HeaderText="Sr.No" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Prcoess Name" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Code" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Total" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Day 1" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Day 2" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="Day 3" />
                                                <asp:BoundField DataField="sg1_f7" HeaderText="Day 4" />
                                                <asp:BoundField DataField="sg1_f8" HeaderText="Day 5" />
                                                <asp:BoundField DataField="sg1_f9" HeaderText="Day 6" />
                                                <asp:BoundField DataField="sg1_f10" HeaderText="Day 7" />
                                                <asp:BoundField DataField="sg1_f11" HeaderText="Day 8" />
                                                <asp:BoundField DataField="sg1_f12" HeaderText="Day 9" />
                                                <asp:BoundField DataField="sg1_f13" HeaderText="Day 10" />
                                                <asp:BoundField DataField="sg1_f14" HeaderText="Day 11" />
                                                <asp:BoundField DataField="sg1_f15" HeaderText="Day 12" />
                                                <asp:BoundField DataField="sg1_f16" HeaderText="Day 13" />
                                                <asp:BoundField DataField="sg1_f17" HeaderText="Day 14" />
                                                <asp:BoundField DataField="sg1_f18" HeaderText="Day 15" />
                                                <asp:BoundField DataField="sg1_f19" HeaderText="Day 16" />
                                                <asp:BoundField DataField="sg1_f20" HeaderText="Day 17" />
                                                <asp:BoundField DataField="sg1_f21" HeaderText="Day 18" />
                                                <asp:BoundField DataField="sg1_f22" HeaderText="Day 19" />
                                                <asp:BoundField DataField="sg1_f23" HeaderText="Day 20" />
                                                <asp:BoundField DataField="sg1_f24" HeaderText="Day 21" />
                                                <asp:BoundField DataField="sg1_f25" HeaderText="Day 22" />
                                                <asp:BoundField DataField="sg1_f26" HeaderText="Day 23" />
                                                <asp:BoundField DataField="sg1_f27" HeaderText="Day 24" />
                                                <asp:BoundField DataField="sg1_f28" HeaderText="Day 25" />
                                                <asp:BoundField DataField="sg1_f29" HeaderText="Day 26" />
                                                <asp:BoundField DataField="sg1_f30" HeaderText="Day 27" />
                                                <asp:BoundField DataField="sg1_f31" HeaderText="Day 28" />
                                                <asp:BoundField DataField="sg1_f32" HeaderText="Day 29" />
                                                <asp:BoundField DataField="sg1_f33" HeaderText="Day 30" />
                                                <asp:BoundField DataField="sg1_f34" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f35" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f36" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f37" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f38" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f39" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f40" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f41" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f42" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f43" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f44" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f45" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f46" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f47" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f48" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f49" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f50" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f51" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f52" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f53" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f54" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f55" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f56" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f57" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f58" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f59" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f60" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f61" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f62" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f63" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f64" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f65" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f66" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f67" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f68" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f69" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f70" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f71" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f72" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f73" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f74" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f75" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f76" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f77" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f78" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f79" HeaderText="" />
                                                <asp:BoundField DataField="sg1_f80" HeaderText="" />

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
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <%--<asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>--%>
                                
                            </td>
                            <td style="text-align: right"></td>
                        </tr>
                    </table>
                </section>


            </div>

            <div class="row">

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tab1" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab4" id="A1" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Item-wise Details</a></li>
                                <li>
                                    <button type="submit" id="btntrsexc2" class="btn-success" style="width: 150px;" runat="server" accesskey="l" onserverclick="btntrsexc2_ServerClick">Transfer To Excel</button>
                                </li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg2" runat="server" Width="100%" Height="200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>


                                                <asp:BoundField DataField="sg2_Srno" HeaderText="Sr.No" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField DataField="sg2_f1" HeaderText="Item Name" />
                                                <asp:BoundField DataField="sg2_f2" HeaderText="ERP Code" />
                                                <asp:BoundField DataField="sg2_f3" HeaderText="Part No" />
                                                <asp:BoundField DataField="sg2_f4" HeaderText="Job No." />
                                                <asp:BoundField DataField="sg2_f5" HeaderText="Job.Dt" />
                                                <asp:BoundField DataField="sg2_f6" HeaderText="Job.Qty" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField DataField="sg2_f7" HeaderText="Time" ItemStyle-HorizontalAlign="right" />
                                                <asp:BoundField DataField="sg2_f8" HeaderText="Delv.DT" />


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

                </section>


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
