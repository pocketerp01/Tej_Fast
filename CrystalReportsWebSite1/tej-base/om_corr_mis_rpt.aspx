<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_corr_mis_rpt" Title="Tejaxo" CodeFile="om_corr_mis_rpt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper" style="background-color: #ededed"">
        <section class="content-header">
            <section class="content">
                <div class="row">
                    <div class="col-md-2"></div>
                    <div class="col-md-8">
                        <div style="text-align: center; background-color:#dddada; padding-top:10px; " >
                            <img src="../tej-base/images/corr_mis.png" alt="" height="250" width="852" />

                            <section class="col-lg-12 connectedSortable" style="background-color:#dddada;">
                                <div class="panel panel-default">
                                    <div id="Tabs" role="tabpanel">
                                        <ul class="nav nav-tabs" role="tablist">
                                            <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Planning, Production Related Reports</a></li>
                                            <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Down time, Rejection Related Reports</a></li>
                                            <li> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnExit" CssClass="bg-red btn-foursquare" runat="server" Text="Exit" OnClick="btnExit_Click" /> </li>
                                        </ul>

                                        <div class="tab-content">
                                            <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                                <div class="lbBody" style="height: 200px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                                    <div class="col-md-6">
                                                        <div class="box">
                                                            <div class="box-body">
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button19" class="btn bg-aqua-active" style="width: 300px;" runat="server" onserverclick="Button19_ServerClick">Corr. Plan Vs Production Data</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button1" class="btn bg-orange-active" style="width: 300px;" runat="server" onserverclick="Button1_ServerClick">Job Completion %</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button2" class="btn bg-maroon-active" style="width: 300px;" runat="server" onserverclick="Button2_ServerClick">Target Prodn Vs Achieved Prodn</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button3" class="btn bg-green-active" style="width: 300px;" runat="server" onserverclick="Button3_ServerClick">Job Wise Wastage Recorded</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button4" class="btn btn-info" style="width: 300px;" runat="server" onserverclick="Button4_ServerClick">Job Wise Issue, Consumption</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="box">
                                                            <div class="box-body">
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button5" class="btn bg-aqua-active" style="width: 300px;" runat="server">Corrugation DPR</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button6" class="btn bg-orange-active" style="width: 300px;" runat="server" onserverclick="Button6_ServerClick">Daily Issuance Vs Consumption</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button7" class="btn bg-maroon-active" style="width: 300px;" runat="server" onserverclick="Button7_ServerClick">Production Vs Dispatch Report</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button8" class="btn bg-green-active" style="width: 300px;" runat="server" onserverclick="Button8_ServerClick">Shift Wise Production , Rejection</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button9" class="btn btn-info" style="width: 300px;" runat="server" onserverclick="Button9_ServerClick">Corrugation Wt Summary</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div role="tabpanel" class="tab-pane" id="DescTab2">
                                                <div class="lbBody" style="height: 200px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                                    <div class="col-md-6">
                                                        <div class="box">
                                                            <div class="box-body">
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button10" class="btn bg-aqua-active" style="width: 300px;" runat="server" onserverclick="Button10_ServerClick">Rejection Reason Data</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button11" class="btn bg-orange-active" style="width: 300px;" runat="server" onserverclick="Button11_ServerClick">Trend of Down Time (D-T-D)</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button12" class="btn bg-maroon-active" style="width: 300px;" runat="server" onserverclick="Button12_ServerClick">Operator Wise Production, Rejection</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button13" class="btn bg-olive-active" style="width: 300px;" runat="server" onserverclick="Button13_ServerClick">Corrugation Production vs Rejection Data</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-6">
                                                        <div class="box">
                                                            <div class="box-body">
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button15" class="btn bg-aqua-active" style="width: 300px;" runat="server" onserverclick="Button15_ServerClick">Down Time Reason Data</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button16" class="btn bg-orange-active" style="width: 300px;" runat="server" onserverclick="Button16_ServerClick">Trend of Rejection (D-T-D)</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button17" class="btn bg-maroon-active" style="width: 300px;" runat="server">Rejection Reports (Printable)</button>
                                                                </div>
                                                                <div class="col-sm-12">
                                                                    <button type="submit" id="Button18" class="btn bg-olive-active" style="width: 300px;" runat="server">Down Time Report (Printable)</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </section>

                        </div>
                    </div>
                    <div class="col-md-2"></div>
                </div>
            </section>
        </section>
    </div>
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfval" runat="server" />
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hfbr" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfaskBranch" runat="server" />
    <asp:HiddenField ID="hfaskPrdRange" runat="server" />
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
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
</asp:Content>
