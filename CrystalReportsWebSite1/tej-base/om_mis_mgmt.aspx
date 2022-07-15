<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_mis_mgmt" EnableEventValidation="false" CodeFile="om_mis_mgmt.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
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
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Engg , Purchase , Stores</a></li>
                                <li><a href="#DescTab2" id="A1" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Inward Quality , Sales</a></li>
                                <li><a href="#DescTab3" id="A2" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Finance , Accounts , Others</a></li>
                                
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 40%;" align="left" >
                                                
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnWithotBOMCreation" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnWithotBOMCreation_ServerClick">Items Sold Without BOM Creation</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnItemSoldMML" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnItemSoldMML_ServerClick">Items Sold Without Defining Min/Max Level</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnVendorsIssuePriceList" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnVendorsIssuePriceList_ServerClick">Vendors Issued PO Without Price List</button>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPurchaseOrder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPurchaseOrder_ServerClick">Purchase Order With Rate Increase</button>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPurReq" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPurReq_ServerClick">Purchase Request Pending Purchase Orders</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnPorpr" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnPorpr_ServerClick">Purchase Orders Pending Material Receipt</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnTatDays" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnTatDays_ServerClick">PR to PO to MRR TAT Days</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnChallans" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnChallans_ServerClick">Job Work Challans Pending With Vendors </button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnInv" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnInv_ServerClick">Material Inward Data With Rejection vs Total MRR</button>
                                                    </td>
                                                </tr>

                                                 <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnStoreStock" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnStoreStock_ServerClick">Stores Stock Needing Attention</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnLatehrs" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnLatehrs_ServerClick">Late Hours Material Inwards in Factory</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnMonthTrend" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnLatehrs_ServerClick">Month Trend of Purchase Qty,Rate</button>
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
                                                        <button type="submit" id="BtnInsp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnInsp_ServerClick">Items Received Without Inspection Template</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnItemInsp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnItemInsp_ServerClick">Items Issued Without Detailed Inspection</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPPMData" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPPMData_ServerClick">Purchase Qty Vs Returns(Vendor PPM Data)</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnStockStatus" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnStockStatus_ServerClick">Stores Stock Status(No. Of Days Stock)</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCusPPM" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCusPPM_ServerClick">Sales Qty Vs Returns(Customer PPM Data)</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSaleppm" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSaleppm_ServerClick">Sales Qty Vs Returns(Item PPM Data)</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCustOrd" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCustOrd_ServerClick">Orders from Customers Without Credit Limit</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCustComp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCustComp_ServerClick">Customer Complaints Recording Status </button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnShip" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnShip_ServerClick">Sales Order To Shipment TAT Days </button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnXYPr" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnXYPr_ServerClick">Cust Wise Sales X Period With Y Period</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 40%;" align="left">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnDebt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDebt_ServerClick">Alert! Debators With Credit Balance</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCredit" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCredit_ServerClick">Alert! Creditors With Debit Balance</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnInvPaid" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnInvPaid_ServerClick">Sales Invoices Paid Early than Agreed Terms</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPurInv" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPurInv_ServerClick">Purchase Invoices Paid Early than Agreed Terms</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCredOut" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCredOut_ServerClick">Creditors Outstanding Over 60 days</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnDebOut" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCredOut_ServerClick">Debtors Outstanding Over 60 days</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnFundCol" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnFundCol_ServerClick">Fund Collection Planned Review</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPurNtIss" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPurNtIss_ServerClick">Purchased But Not Issued</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BMrrPen" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BMrrPen_ServerClick">MRR Pending Accounts Entry</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnDocVol" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDocVol_ServerClick">Document Volume Monitoring</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnUserlist" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnUserlist_ServerClick">User List by Labels</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCredit1" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCredit1_ServerClick">Credit Notes to Customers</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
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
