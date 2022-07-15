<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_upload_dashboard" Title="Tejaxo" CodeFile="om_upload_dashboard.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>                   
                    <td style="text-align: left">
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                     <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Tejaxo ERP :: Data Migration Module</asp:Label>
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
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Masters</a></li>
                                <li><a href="#DescTab2" id="A1" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Place Holders</a></li>
                                <li><a href="#DescTab3" id="A2" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Opening Balances</a></li>
                                <li><a href="#DescTab4" id="A3" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Transaction Data</a></li>
                                <li><a href="#DescTab5" id="A4" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Special Option</a></li>
                                <li><a href="#DescTab6" id="A5" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Data Collection</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep1" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep1_ServerClick">Upload HSN Master</button>
                                                    </td>
                                                    <td rowspan="5">
                                                        <img src="../tej-base/images/datamig.jpg" height="300" style="padding-left:200px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnAcSchedule" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnAcSchedule_ServerClick">Upload Accounts Schedules Master</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnItemSubGrp" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnItemSubGrp_ServerClick">Upload Item Sub Group Master</button>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnAcMaster" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnAcMaster_ServerClick">Upload Account Master</button>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnItemMaster" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnItemMaster_ServerClick">Upload Item Master</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnPOPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnPOPlaceHolder_ServerClick">Create Place Holder Purchase Orders</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnGEPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnGEPlaceHolder_ServerClick">Create Place Holder Gate Entry</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnMRRPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnMRRPlaceHolder_ServerClick">Create Place Holder MRR Entry</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnRGPPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnRGPPlaceHolder_ServerClick">Create Place Holder RGP Orders</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="btnSOPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnSOPlaceHolder_ServerClick">Create Place Holder Sales Order</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="btnDAPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnDAPlaceHolder_ServerClick">Create Place Holder Dispatch Advice</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="btnINVPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnINVPlaceHolder_ServerClick">Create Place Holder Invoice Entry</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="btnJCPlaceHolder" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnJCPlaceHolder_ServerClick">Create Place Holder Job Card</button>
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
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep2" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep2_ServerClick">Upload A/C wise Ledger Balance (Op.Bal)</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep3" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep3_ServerClick">Upload Bill Wise Outstandings(Drs)</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep4" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep4_ServerClick">Upload Bill Wise Outstandings(Crs)</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep7" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep7_ServerClick1">Upload Pending Bank Reco Entries </button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="rep5" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep5_ServerClick">Upload Item wise Stock Balance (Op. Bal) </button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="rep8" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep8_ServerClick1">Upload Section Wise WIP Stock (Op.Bal)</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="rep9" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep9_ServerClick1">Upload Batch No. Wise Store Stock </button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="rep10" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep10_ServerClick1">Upload Reel Stock</button>
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
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep11" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep11_ServerClick">Upload Bill of Materials </button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep12" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep12_ServerClick">Upload Inward Quality Templates</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep13" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep13_ServerClick">Upload Outward Quality Templates </button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="rep14" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="rep14_ServerClick">Upload Sales Orders</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="btnPOUpload" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnPOUpload_ServerClick">Upload Purchase Orders</button>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td>
                                                        <button type="submit" id="btnAppVend" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnAppVend_ServerClick">Upload Vendor Approved Price List</button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnLotWiseBalance" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnLotWiseBalance_ServerClick">Auto Create Lot Wise Balance</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnPhyStoreStock" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnPhyStoreStock_ServerClick">Upload Physical Store Stock (Reco)</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnAcctgEnt" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnAcctgEnt_ServerClick">Re-Generate Sales >> Acctg Entries</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <button type="submit" id="btnProdnEntry" class="btn btn-info" style="width: 100%; font-size: medium; font-weight: 600;" runat="server" onserverclick="btnProdnEntry_ServerClick">Re-Generate Sales >> Prodn Entries</button>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div>
                                        <div class="box-body">
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td style="width: 50%"></td>
                                                    <td></td>
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

    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfval" runat="server" />
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hfbr" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hfaskBranch" runat="server" />
    <asp:HiddenField ID="hfid" runat="server" />
    <asp:HiddenField ID="hfaskPrdRange" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />

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
