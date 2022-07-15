<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_label_costing" CodeFile="om_label_costing.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            calculateSum();
        });
        function calculateSum() {
            document.getElementById('ContentPlaceHolder1_txtReqWidth').value = (((document.getElementById('ContentPlaceHolder1_txtLblHeight').value * 1) + (document.getElementById('ContentPlaceHolder1_txtGapAcross').value * 1)) * (document.getElementById('ContentPlaceHolder1_txtUpsAcross').value * 1)).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtWidthtoUsed').value = ((document.getElementById('ContentPlaceHolder1_txtReqWidth').value * 1) + 10).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtLMtrs').value = (((document.getElementById('ContentPlaceHolder1_txtLblWidth').value * 1) + (document.getElementById('ContentPlaceHolder1_txtGapAround').value * 1)) * ((document.getElementById('ContentPlaceHolder1_txtOrderQty').value * 1) / (document.getElementById('ContentPlaceHolder1_txtUpsAcross').value * 1)) / 1000).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtSettingWaste').value = ((document.getElementById('ContentPlaceHolder1_txtNoOfColor').value * 35) * ((document.getElementById('ContentPlaceHolder1_txtVariant').value * 1) + (document.getElementById('ContentPlaceHolder1_txtSetting').value * 1))).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtInSqm').value = (((document.getElementById('ContentPlaceHolder1_txtLMtrs').value * 1) + ((document.getElementById('ContentPlaceHolder1_txtSettingWaste').value * 1))) * ((document.getElementById('ContentPlaceHolder1_txtWidthtoUsed').value / 1000))).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtWstgValue').value = ((document.getElementById('ContentPlaceHolder1_txtInSqm').value * 1) * ((document.getElementById('ContentPlaceHolder1_txtWstgPer').value / 100))).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtTotSQM').value = ((document.getElementById('ContentPlaceHolder1_txtInSqm').value * 1) + ((document.getElementById('ContentPlaceHolder1_txtWstgValue').value * 1))).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtMaterial1Price').value = document.getElementById('ContentPlaceHolder1_ddMaterial1').value.toUpperCase();
            document.getElementById('ContentPlaceHolder1_txtMaterial2Price').value = document.getElementById('ContentPlaceHolder1_ddMaterial2').value.toUpperCase();

            if (document.getElementById('ContentPlaceHolder1_ddRibbon').value.toUpperCase() == "YES")
                document.getElementById('ContentPlaceHolder1_txtMaterial1Value').value = ((0.5 + (document.getElementById('ContentPlaceHolder1_txtMaterial1Price').value * 1)) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1)).toFixed(2);
            else
                document.getElementById('ContentPlaceHolder1_txtMaterial1Value').value = ((document.getElementById('ContentPlaceHolder1_txtMaterial1Price').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1)).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtMaterial2Value').value = ((document.getElementById('ContentPlaceHolder1_txtMaterial2Price').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1)).toFixed(2);

            if ((document.getElementById('ContentPlaceHolder1_txtVariant').value * 1) == 0)
                document.getElementById('ContentPlaceHolder1_txtInkPrice').value = "0";
            else
                document.getElementById('ContentPlaceHolder1_txtInkPrice').value = "0.4";

            if ((document.getElementById('ContentPlaceHolder1_txtNoOfColor').value * 1) == 0) document.getElementById('ContentPlaceHolder1_txtInkValue').value = "0"
            else document.getElementById('ContentPlaceHolder1_txtInkValue').value = ((document.getElementById('ContentPlaceHolder1_txtInkPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1)).toFixed(2);

            var uv = 0;
            if (document.getElementById('ContentPlaceHolder1_ddVarnish').value.toUpperCase() == "UV")
                uv = "0.13";
            else if (document.getElementById('ContentPlaceHolder1_ddVarnish').value.toUpperCase() == "MATT")
                uv = "0.3";
            else if (document.getElementById('ContentPlaceHolder1_ddVarnish').value.toUpperCase() == "SPOT")
                uv = "0.13";

            document.getElementById('ContentPlaceHolder1_txtVarnishPrice').value = uv;
            if (document.getElementById('ContentPlaceHolder1_ddVarnish').value.toUpperCase() == "SPOT")
                document.getElementById('ContentPlaceHolder1_txtVarnishValue').value = (((document.getElementById('ContentPlaceHolder1_txtVarnishPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1)) + 50).toFixed(2);
            else
                document.getElementById('ContentPlaceHolder1_txtVarnishValue').value = (((document.getElementById('ContentPlaceHolder1_txtVarnishPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            var varnish = 0;
            if (document.getElementById('ContentPlaceHolder1_ddHighVarnish').value.toUpperCase() == "FULL")
                varnish = "1";
            else if (document.getElementById('ContentPlaceHolder1_ddHighVarnish').value.toUpperCase() == "HALF")
                varnish = "0.5";

            document.getElementById('ContentPlaceHolder1_txtHighVarnishPrice').value = varnish;
            document.getElementById('ContentPlaceHolder1_txtHighVarnishValue').value = (((document.getElementById('ContentPlaceHolder1_txtHighVarnishPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            var lamination = 0;
            if (document.getElementById('ContentPlaceHolder1_ddLamination').value.toUpperCase() == "GLOSS")
                lamination = "0.3";
            else if (document.getElementById('ContentPlaceHolder1_ddLamination').value.toUpperCase() == "MATT")
                lamination = "0.4";
            else if (document.getElementById('ContentPlaceHolder1_ddLamination').value.toUpperCase() == "BOPP")
                lamination = "0.5";

            document.getElementById('ContentPlaceHolder1_txtLaminationPrice').value = lamination;
            document.getElementById('ContentPlaceHolder1_txtLaminationValue').value = (((document.getElementById('ContentPlaceHolder1_txtLaminationPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            var screenPrice = 0;
            if (document.getElementById('ContentPlaceHolder1_ddScreen').value.toUpperCase() == "100%")
                screenPrice = "0.4";
            else if (document.getElementById('ContentPlaceHolder1_ddScreen').value.toUpperCase() == "75%")
                screenPrice = "0.3";
            else if (document.getElementById('ContentPlaceHolder1_ddScreen').value.toUpperCase() == "50%")
                screenPrice = "0.2";
            else if (document.getElementById('ContentPlaceHolder1_ddScreen').value.toUpperCase() == "25%")
                screenPrice = "0.1";

            document.getElementById('ContentPlaceHolder1_txtScreenPrice').value = screenPrice;
            document.getElementById('ContentPlaceHolder1_txtScreenValue').value = (((document.getElementById('ContentPlaceHolder1_txtScreenPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            var emobss = "0";            
            if (document.getElementById('ContentPlaceHolder1_ddEmbossing').value.toUpperCase() == "YES")
                emobss = "0.5";

            document.getElementById('ContentPlaceHolder1_txtEmbossingPrice').value = emobss;
            document.getElementById('ContentPlaceHolder1_txtEmbossingValue').value = (((document.getElementById('ContentPlaceHolder1_txtEmbossingPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);


            var hotfoil = 0;
            if (document.getElementById('ContentPlaceHolder1_ddFoilStamp').value.toUpperCase() == "FULL")
                hotfoil = "0.7";
            else if (document.getElementById('ContentPlaceHolder1_ddFoilStamp').value.toUpperCase() == "HALF")
                hotfoil = "0.35";

            document.getElementById('ContentPlaceHolder1_txtFoilStampPrice').value = hotfoil;
            document.getElementById('ContentPlaceHolder1_txtFoilStampValue').value = (((document.getElementById('ContentPlaceHolder1_txtFoilStampPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            var coldfoil = 0;
            if (document.getElementById('ContentPlaceHolder1_ddColdFoilStamp').value.toUpperCase() == "FULL")
                coldfoil = "1";
            else if (document.getElementById('ContentPlaceHolder1_ddColdFoilStamp').value.toUpperCase() == "HALF")
                coldfoil = "0.5";

            document.getElementById('ContentPlaceHolder1_txtddColdFoilStampPrice').value = coldfoil;
            document.getElementById('ContentPlaceHolder1_txtddColdFoilStampValue').value = (((document.getElementById('ContentPlaceHolder1_txtddColdFoilStampPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            var rainfoil = 0;
            if (document.getElementById('ContentPlaceHolder1_ddRainBoxFoil').value.toUpperCase() == "FULL")
                rainfoil = "2.4";
            else if (document.getElementById('ContentPlaceHolder1_ddRainBoxFoil').value.toUpperCase() == "HALF")
                rainfoil = "1.2";

            document.getElementById('ContentPlaceHolder1_txtRainBoxFoilPrice').value = rainfoil;
            document.getElementById('ContentPlaceHolder1_txtRainBoxFoilValue').value = (((document.getElementById('ContentPlaceHolder1_txtRainBoxFoilPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtTotSQM').value * 1))).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtPlateCostPrice').value = ((document.getElementById('ContentPlaceHolder1_txtWidthtoUsed').value * 1) > 321) ? "6.5" : "4";
            document.getElementById('ContentPlaceHolder1_txtPlateCostValue').value = (((document.getElementById('ContentPlaceHolder1_txtPlateCostPrice').value * 1) * (document.getElementById('ContentPlaceHolder1_txtNoOfColor').value * 1))).toFixed(2);


            var prodtime = 0;
            var idelspeed = 45;
            var dwnTime = (45 + ((document.getElementById('ContentPlaceHolder1_txtSettingWaste').value * 1) * 0.2143)).toFixed(2);
            var oprSpeed = ((document.getElementById('ContentPlaceHolder1_txtUpsAcross').value * 1) * (1000 / (document.getElementById('ContentPlaceHolder1_txtLblWidth').value * 1)) * (idelspeed * 1));
            debugger;

            prodtime = ((((document.getElementById('ContentPlaceHolder1_txtOrderQty').value * 1) / oprSpeed) / 60) * (document.getElementById('ContentPlaceHolder1_txtSetting').value * 1) + (dwnTime / 60)).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtProdTimePerH').value = prodtime;

            document.getElementById('ContentPlaceHolder1_txtFront2').value = (prodtime * 125).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtProdJob').value = ((document.getElementById('ContentPlaceHolder1_txtMaterial1Value').value * 1) + (document.getElementById('ContentPlaceHolder1_txtMaterial2Value').value * 1) + (document.getElementById('ContentPlaceHolder1_txtInkValue').value * 1) +
                (document.getElementById('ContentPlaceHolder1_txtVarnishValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtHighVarnishValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtLaminationValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtScreenValue').value * 1) +
                (document.getElementById('ContentPlaceHolder1_txtEmbossingValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtFoilStampValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtddColdFoilStampValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtRainBoxFoilValue').value * 1) +
                (document.getElementById('ContentPlaceHolder1_txtPlateCostValue').value * 1) + (document.getElementById('ContentPlaceHolder1_txtFront2').value * 1)).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtRmcCostperK').value = ((document.getElementById('ContentPlaceHolder1_txtProdJob').value * 1) / (document.getElementById('ContentPlaceHolder1_txtOrderQty').value) * 1000).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtJobRmc').value = (((document.getElementById('ContentPlaceHolder1_txtRmcCostperK').value * 1) / ((document.getElementById('ContentPlaceHolder1_txtPricePerK').value))) * 100).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtFront3').value = (100 * (document.getElementById('ContentPlaceHolder1_txtRmcCostperK').value * 1) / ((document.getElementById('ContentPlaceHolder1_txtMchnCost').value * 1) / 100) / 100).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtValueAED').value = ((document.getElementById('ContentPlaceHolder1_txtSetting').value * 1) + (document.getElementById('ContentPlaceHolder1_txtOrderQty').value * 1) * (document.getElementById('ContentPlaceHolder1_txtPricePerK').value) / 1000).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtTotValue').value = ((document.getElementById('ContentPlaceHolder1_txtValueAED').value * 1) + ((document.getElementById('ContentPlaceHolder1_txtValueAED').value * 1) * (document.getElementById('ContentPlaceHolder1_txtPer').value * 1) / 100)).toFixed(2);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                   <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                     <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Costing Sheet (Label)"></asp:Label></td>
                    
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-6">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl1" runat="server" Text="Entry_No." CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="Product" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="Customer" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <label>RMC Cost / 1000</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRmcCostperK" runat="server" Width="120px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>

                                    <td>
                                        <label>Price / 1000</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPricePerK" runat="server" Width="120px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Prod.Time / Hrs</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProdTimePerH" runat="server" Width="120px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <label>Value / AED</label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtValueAED" runat="server" Width="120px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label>Tax %</label></td>
                                    <td>
                                        <asp:TextBox ID="txtPer" runat="server" Width="30px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" onkeyup="calculateSum()" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                        <asp:TextBox ID="txtTotValue" runat="server" Width="86px"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist" style="display: none">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe; margin: 5px; padding: 5px;">
                                        <table style="vertical-align: top">
                                            <tr>
                                                <td>
                                                    <table style="max-width: 90%">
                                                        <tr>
                                                            <td style="min-width: 180px">
                                                                <label>Order Qty</label>
                                                            </td>
                                                            <td style="min-width: 180px">
                                                                <asp:TextBox ID="txtOrderQty" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td style="min-width: 180px">
                                                                <label>Material 1</label>
                                                            </td>
                                                            <td style="min-width: 180px">
                                                                <asp:DropDownList ID="ddMaterial1" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()"></asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaterial1Price" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaterial1Value" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Label Height</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLblHeight" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Material 2</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddMaterial2" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()"></asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaterial2Price" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaterial2Value" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Label Width</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLblWidth" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Ink</label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtInkPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtInkValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>UPS Across</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtUpsAcross" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Varnish</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddVarnish" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="UV" Value="UV"></asp:ListItem>
                                                                    <asp:ListItem Text="MATT" Value="MATT"></asp:ListItem>
                                                                    <asp:ListItem Text="SPOT" Value="SPOT"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVarnishPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVarnishValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>No of Repeats</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNoofRepeats" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>High Emb. Varnish (Screen)</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddHighVarnish" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Full" Value="Full"></asp:ListItem>
                                                                    <asp:ListItem Text="Half" Value="Half"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHighVarnishPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtHighVarnishValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>2nd /3rd  Variant</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtVariant" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Lamination</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddLamination" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Gloss" Value="Gloss"></asp:ListItem>
                                                                    <asp:ListItem Text="Matt" Value="Matt"></asp:ListItem>
                                                                    <asp:ListItem Text="BOPP" Value="BOPP"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLaminationPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLaminationValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>2nd /3rd  Setting</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSetting" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Screen</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddScreen" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="100%" Value="100%"></asp:ListItem>
                                                                    <asp:ListItem Text="75%" Value="75%"></asp:ListItem>
                                                                    <asp:ListItem Text="50%" Value="50%"></asp:ListItem>
                                                                    <asp:ListItem Text="25%" Value="25%"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtScreenPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtScreenValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>No of Colors</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNoOfColor" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Embossing</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddEmbossing" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmbossingPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtEmbossingValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Gap Across</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtGapAcross" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Hot Foil Stamp</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddFoilStamp" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Full" Value="Full"></asp:ListItem>
                                                                    <asp:ListItem Text="Half" Value="Half"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFoilStampPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFoilStampValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Gap Around</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtGapAround" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Cold Foil Stamp</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddColdFoilStamp" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Full" Value="Full"></asp:ListItem>
                                                                    <asp:ListItem Text="Half" Value="Half"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtddColdFoilStampPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtddColdFoilStampValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Req. Width</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtReqWidth" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Rainbow Foil</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddRainBoxFoil" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Full" Value="Full"></asp:ListItem>
                                                                    <asp:ListItem Text="Half" Value="Half"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRainBoxFoilPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRainBoxFoilValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Width to be used</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtWidthtoUsed" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Plates Cost</label>
                                                            </td>
                                                            <td></td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlateCostPrice" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtPlateCostValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>in L-Mtrs</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtLMtrs" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" Font-Bold="true"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>RIBBON COST</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddRibbon" runat="server" Font-Size="Smaller" Height="25px" Width="150px" onchange="calculateSum()">
                                                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-top-style: groove;">
                                                                <label>Machine cost</label>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-top-style: groove;">
                                                                <asp:TextBox ID="txtMchnCost" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>Setting waste if</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSettingWaste" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Front</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtFront1" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-bottom-style: groove;">
                                                                <asp:TextBox ID="txtFront2" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-bottom-style: groove;">
                                                                <asp:TextBox ID="txtFront3" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td>
                                                                <label>In SQM</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtInSqm" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <label>Back</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBack1" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-top-style: groove;">
                                                                <label>Total Prod Cost</label>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-top-style: groove;">
                                                                <label>Job RMC</label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <label>Wstg % / Sqm</label>
                                                            </td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txtWstgPer" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="30px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" BackColor="#BDEDFF"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtWstgValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="69px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>
                                                                <label style="font-weight: bold">Total SQM</label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTotSQM" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px" Font-Bold="true"></asp:TextBox>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-bottom-style: groove;">
                                                                <asp:TextBox ID="txtProdJob" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                            <td style="border-left-style: groove; border-right-style: groove; border-bottom-style: groove;">
                                                                <asp:TextBox ID="txtJobRmc" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align: top">
                                                    <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                                        Style="background-color: #FFFFFF; color: White;" Height="400px" Width="350px" Font-Size="13px"
                                                        AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                                        OnRowCommand="sg1_RowCommand">
                                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                        <Columns>
                                                            <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1" />
                                                            <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2" />
                                                            <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3" />
                                                            <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4" />
                                                            <asp:BoundField DataField="sg1_h5" HeaderText="sg1_h5" />
                                                            <asp:BoundField DataField="sg1_h6" HeaderText="sg1_h6" />
                                                            <asp:BoundField DataField="sg1_h7" HeaderText="sg1_h7" />
                                                            <asp:BoundField DataField="sg1_h8" HeaderText="sg1_h8" />
                                                            <asp:BoundField DataField="sg1_h9" HeaderText="sg1_h9" />
                                                            <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>Add</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>Del</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" />
                                                            <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                            <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                            <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                            <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                            <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Style="text-align: right" onkeypress="return isDecimalKey(event)" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Style="text-align: right" onkeypress="return isDecimalKey(event)" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>Dt</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="sg1_btnalt" runat="server" CommandName="SG1_ROW_ALT" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Alt Item" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Style="text-align: right" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Style="text-align: right" onkeypress="return isDecimalKey(event)" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Style="text-align: right" onkeypress="return isDecimalKey(event)" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Style="text-align: right" onkeypress="return isDecimalKey(event)" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>

                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>


                                                        </Columns>
                                                        <EditRowStyle BackColor="#999999" />
                                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                    </fin:CoolGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl19_Click" />
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Terms</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ERP_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dlv_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sch.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Prod.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Card</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="1" Width="100px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="150" Width="99%" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
