<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_sales_reports" EnableEventValidation="false" CodeFile="om_sales_reports.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">

        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Extensive Sales Reporting</asp:Label>
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
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Sales Registers</a></li>
                                <li><a href="#DescTab2" id="A1" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comparison and Analysis</a></li>
                                <li><a href="#DescTab3" id="A2" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Schedule Based Reports</a></li>
                                <li><a href="#DescTab4" id="A3" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">More Reports</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 40%;" align="left">

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnGrossAmt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnGrossAmt_ServerClick">Item Qty , Value Monthwise Report</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnDisSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDisSale_ServerClick">District Wise Sales</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnState" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnState_ServerClick">State Wise Sales</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnHSNSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnHSNSale_ServerClick">HSN Wise Sales Summary</button>
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
                                                        <button type="submit" id="BtnItemMonthDesp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnItemMonthDesp_ServerClick">ITEM Wise Despatch Value</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCustDesp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCustDesp_ServerClick">Selected Party Wise 12 Monthly Qty</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnQtyComp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnQtyComp_ServerClick">Quaterly Comparison of Sales Qty</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnPartyItem" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPartyItem_ServerClick">Party,Item 12 Monthly Despatch Qty and Value</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnZoneDet" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnZoneDet_ServerClick">Zone Wise Details</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCont" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCont_ServerClick">Continent Wise Details</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSalesGrpRpt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSalesGrpRpt_ServerClick">Sales Grp Wise Sales Report</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnGrossRpt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnGrossRpt_ServerClick">12 Month Gross Report</button>
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
                                                        <button type="submit" id="BtnSerchMth" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSerchMth_ServerClick">Schedule Vs Disptach (With %)(Mthly Searchable)</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnCusTren" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCusTren_ServerClick">Customer Trend Wt Wise</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnShedDisp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnShedDisp_ServerClick">Schedule Vs Dispatch Variable Period</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnClosedSO" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnClosedSO_ServerClick">Closed S.O Report</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnQtyComp2" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnQtyComp2_ServerClick">Scheduled Qty Comparison 12 Month</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSchValComp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSchValComp_ServerClick">Schedule Value Comparison 12 Month</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnLastvsCurr" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnLastvsCurr_ServerClick">Scheduled Qty Comparison Last Yr vs Current Yr</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="BtnSchVal" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSchVal_ServerClick">Schedule Value Comparison Last Yr vs Current Yr</button>
                                                    </td>
                                                </tr>

                                            </table>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnDayWiseSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDayWiseSale_ServerClick">Date Wise Sales</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnStateQty" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnStateQty_ServerClick">State Wise 12 Month, Qty Value</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnCustWiseSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnCustWiseSale_ServerClick">Date Wise Sales : Customer Wise</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnStateQtyGrp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnStateQtyGrp_ServerClick">State Wise , Grp, Sub Grp, 12 Month Qty, Value</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnDateWiseItem" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDateWiseItem_ServerClick">Date Wise Sales : Item Wise</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnYrWiseSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnYrWiseSale_ServerClick">Year Wise Month Wise Sale Report</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnDateWiseLine" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnDateWiseLine_ServerClick">Date Wise Sales : Line Wise</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnMthSchedule" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnMthSchedule_ServerClick">Mth Schedule Vs Sales Compliance Value</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnItemInv" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnItemInv_ServerClick">Item Wise Invoice Wise Line Wise Report</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnComVsTax" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnComVsTax_ServerClick">Common Inv vs Tax Invoice Report</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnInvWiseRpt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnInvWiseRpt_ServerClick">Invoice Wise Report</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnSummInvDbt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSummInvDbt_ServerClick">Summary Data For Invoice Debit Note.</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnInvWiseRptTot" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnInvWiseRptTot_ServerClick">Invoice Wise Report(Totals)</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnEwayBill" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnEwayBill_ServerClick">Eway Bill Data Format.</button>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <button type="submit" id="BtnPriceSumm" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnPriceSumm_ServerClick">Price Summary Month wise</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td>
                                                        <button type="submit" id="BtnMastWT" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnMastWT_ServerClick">Invoice Details with Master WT.</button></td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnSchvsDispNew" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnSchvsDispNew_ServerClick">Sales Register New</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnTarrifChk" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnTarrifChk_ServerClick">Tariff Check</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td></td>
                                                </tr>


                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnMainGrpSale" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnMainGrpSale_ServerClick">Main Grp , Sub Grp 12 Months Qty , Sale Value</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td></td>
                                                </tr>

                                                <tr>
                                                    <td >
                                                        <button type="submit" id="BtnOrdValue" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="BtnOrdValue_ServerClick">Main Grp , Sub Grp 12 Months Qty , Order Value</button>
                                                    </td>
                                                    <td style="width:10px"></td>
                                                    <td ></td>
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
    <asp:HiddenField ID="hf2" runat="server" />
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
