<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_finsys_options" EnableEventValidation="false" CodeFile="om_finsys_options.aspx.cs" %>


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
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Tejaxo ERP :: Intenal Audit Option</asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="x" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
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
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">MIS</a></li>
                                <li><a href="#DescTab2" id="A1" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Graphs</a></li>
                                <li><a href="#DescTab3" id="A2" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Consolidated View</a></li>

                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 40%;" align="left">

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSalesData" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSalesData_ServerClick">Daily Sales Data</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCustWiseSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCustWiseSale_ServerClick">Customer Wise Sales Tracking (All)</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSalesTrend" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSalesTrend_ServerClick">Sales Trend Qty,Value, Month Wise</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSalesOrdRec" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSalesOrdRec_ServerClick">Sales Order Recvd Report</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSaleGrpTrend" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSaleGrpTrend_ServerClick">12 Mth Sales Group Wise Trend</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCshVouch" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCshVouch_ServerClick">Approval of Cash Vouchers</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnDebtAge" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDebtAge_ServerClick">Debtors Ageing</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCreditAge" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCreditAge_ServerClick">Creditors Ageing</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPurchTrend" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPurchTrend_ServerClick">Purch. Trend,Qty,Value Monthwise</button>
                                                    </td>
                                                </tr>


                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 40%;" align="left">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnDiRept" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDiRept_ServerClick">DI Report</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSaleAgentWise" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSaleAgentWise_ServerClick">Monthly Sales Achieved Agent Wise</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSaleVsCol" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSaleVsCol_ServerClick">Monthly Sales Vs Collections</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 49%">
                                                        <button type="submit" id="BtnSalesDespatch" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSalesDespatch_ServerClick">Sales Despatches (Basic Value)</button>
                                                    </td>
                                                    <td style="width: 10px"></td>
                                                    <td style="width: 49%">
                                                        <button type="submit" id="BtnSalesOrd" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSalesOrd_ServerClick">Sales Orders</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnPurchOrds" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPurchOrds_ServerClick">Purchase Orders</button>
                                                    </td>
                                                    <td style="width: 10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnProfitLossAc" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnProfitLossAc_ServerClick">Profit and Loss Accounts</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnInwarVal" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnInwarVal_ServerClick">Material / Inward Value</button>
                                                    </td>
                                                    <td style="width: 10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnCollect" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCollect_ServerClick">Collections</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnProdGrpSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnProdGrpSale_ServerClick">Product Group Wise Sale</button>
                                                    </td>
                                                    <td style="width: 10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnPaymnt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPaymnt_ServerClick">Payments</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>


                                        <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                            <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                                Style="background-color: #FFFFFF; color: White;" Width="100%" Height="300px" Font-Size="Small"
                                                AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                                OnRowCommand="sg1_RowCommand">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="sg1_f1" HeaderText="Name of Unit" HeaderStyle-Width="850px" HeaderStyle-BackColor="YellowGreen" ItemStyle-Height="25px" />
                                                    <asp:BoundField DataField="sg1_f2" HeaderText="Today" HeaderStyle-Width="150px" HeaderStyle-BackColor="YellowGreen" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="25px"/>
                                                    <asp:BoundField DataField="sg1_f3" HeaderText="This Month" HeaderStyle-Width="150px" HeaderStyle-BackColor="YellowGreen" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="25px"/>
                                                    <asp:BoundField DataField="sg1_f4" HeaderText="This Year" HeaderStyle-Width="150px" HeaderStyle-BackColor="YellowGreen" ItemStyle-HorizontalAlign="Right" ItemStyle-Height="25px"/>
                                                </Columns>
                                                <EditRowStyle BackColor="#999999" />
                                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" Font-Size="14px"
                                                    CssClass="GridviewScrollHeader2" />
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
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
