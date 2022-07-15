<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="True" Inherits="om_pinv_entry2" CodeFile="om_pinv_entry2.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script lang="javaScript" type="text/javascript" src="../crystalreportviewers13/js/crviewer/crv.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //if (typeof (Sys) !== 'undefined') {
            //    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (sender, args) {
            //        { { scopeName } } _init();
            //    });
            //}
            calculateSum();
            funTotalDRCR();
        });
        function makeQty() {
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            //for (var i = 0; i < gridCount; i++) {
            //    var stdPacking = document.getElementById("ContentPlaceHolder1_sg1").rows[i].cells[2].innerHTML;
            //    var noOfPack = $("input[id*=sg1_t1]");
            //    if (fill_zero(stdPacking) <= 0) stdPacking = 1;
            //    document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value = fill_zero(noOfPack[i].value * stdPacking);
            //}
        }
    

        //function calculateSum() { }

        function calculateSum() {
            var totCGST = 0;
            var totSGST = 0;
            var IAMOUNT = 0;

            var TOTAMT = 0;
            var tool_pu = 0;
            var tool_amtz = 0;
            var txb_val = 0;

            var tcs_amt = 0;
            var tcs_val = 0;

            var cashDisc = 0;
            var totCashDisc = 0;
            var txtcashDisc = 0;
            var rowWiseTotDisc = 0;
            var LC = 0;
            var defaultCGST = 0;
            var defaultSGST = 0;
            //txtcashDisc = document.getElementById('ContentPlaceHolder1_txtCashDisc').value;
            var passFullQty = document.getElementById("ContentPlaceHolder1_hfw122").value;
            var checkTax = "Y";
            var roundoffTax = "N";
            var roundUpto = 2;
            if (document.getElementById("ContentPlaceHolder1_chkTax").checked) {
                checkTax = "Y";
            }
            else checkTax = "N";
            if (document.getElementById("ContentPlaceHolder1_chkRoundTax").checked) {
                roundoffTax = "Y";
            }
            else roundoffTax = "N";
            var highestPercentCGST = 0, highestPercentSGST = 0;

            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            for (var i = 0; i < gridCount; i++) {

                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;
                var stdPacking = 1;
                stdPacking = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[1].innerHTML;
                LC = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[2].innerHTML;
                if (fill_zero(stdPacking) <= 0) stdPacking = 1;
                var Qty = document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value;
                if (passFullQty == "Y")
                    Qty = document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value;
                var Rate = $("input[id*=sg1_t4]");
                var DiscPer = $("input[id*=sg1_t5]");

                var CGST = $("input[id*=sg1_t7]");
                var SGST = $("input[id*=sg1_t8]");
                var impTaxAmt = $("input[id*=sg1_t10]");
                var tool_pu = $("input[id*=sg1_t11]");
                var lesDicRs = $("input[id*=sg1_t26]");


                var AMOUNT = 0;

                var tax1 = 0;
                var tax2 = 0;
                //if ((LC * 1) != 0 && document.getElementById('ContentPlaceHolder1_lbl1a').innerText != "56") AMOUNT = (Qty[i].value * 1) * (LC * 1);
                //else
                AMOUNT = (Qty * 1) * (Rate[i].value * 1);
                tool_amtz = (Qty * 1) * (tool_pu[i].value * 1);

                txb_val = AMOUNT + tool_amtz;

                if (fill_zero(DiscPer[i].value) > 0) {
                    AMOUNT = AMOUNT - (AMOUNT * DiscPer[i].value / 100)
                }
                if (fill_zero(lesDicRs[i].value) > 0) {
                    AMOUNT = AMOUNT - fill_zero(lesDicRs[i].value);
                }
                //else AMOUNT = AMOUNT - fill_zero(DiscRs[i].value);

                //if (document.getElementById('ContentPlaceHolder1_hfcocd').value != "MULT")
                //    document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value = Math.round(fill_zero(Qty[i].value / stdPacking));

                rowWiseTotDisc = (AMOUNT * (txtcashDisc / 100)).toFixed(2);
                if (txtcashDisc > 0)
                    AMOUNT = AMOUNT - rowWiseTotDisc;
                document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value = fill_zero(AMOUNT).toFixed(2);

                IAMOUNT += fill_zero(AMOUNT);
                //debugger;
                roundUpto = 2;
                if (roundoffTax == "Y")
                    roundUpto = 0;

                //debugger;
                if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText == "56" && Number(impTaxAmt[i].value) > 0 && checkTax == "Y") {

                    tax1 = ((Number(impTaxAmt[i].value) + Number(tool_amtz)) * (CGST[i].value * 1) / 100).toFixed(roundUpto);

                    totCGST = ((totCGST * 1) + (tax1 * 1)).toFixed(roundUpto);
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = fill_zero(tax1 * 1).toFixed(roundUpto);
                }
                else if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText != "42" && checkTax == "Y") {
                    if ((CGST[i].value * 1) > 0) {
                        //totCGST += (AMOUNT * (CGST[i].value * 1) / 100);

                        tax1 = ((AMOUNT + tool_amtz) * (CGST[i].value * 1) / 100).toFixed(roundUpto);

                        totCGST = ((totCGST * 1) + (tax1 * 1)).toFixed(2);
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = fill_zero(tax1 * 1).toFixed(roundUpto);
                    }
                    if ((SGST[i].value * 1) > 0) {
                        tax2 = ((AMOUNT + tool_amtz) * (SGST[i].value * 1) / 100).toFixed(roundUpto);
                        totSGST = ((totSGST * 1) + (tax2 * 1)).toFixed(2);
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = fill_zero(tax2 * 1).toFixed(roundUpto);
                    }
                }
                else {
                    tax1 = 0; tax2 = 0; totCGST = 0; totSGST = 0;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = 0;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = 0;
                }

                document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[4].innerHTML = rowWiseTotDisc;
                totCashDisc += fill_zero(document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[2].innerHTML) * 1;

                defaultCGST = CGST[i].value;
                defaultSGST = SGST[i].value;

                if (Number(defaultCGST) > Number(highestPercentCGST))
                    highestPercentCGST = Number(defaultCGST);

                if (Number(defaultSGST) > Number(highestPercentSGST))
                    highestPercentSGST = Number(defaultSGST);
            }

            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl24').value) > 0) {
                IAMOUNT += fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl24').value) * 1;
                if (fill_zero(defaultCGST) > 0) {
                    totCGST = (totCGST * 1);
                    tax1 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl24').value) * (fill_zero(defaultCGST) / 100);
                    totCGST += fill_zero(tax1) * 1;
                }
                if (fill_zero(defaultSGST) > 0) {
                    totSGST = (totSGST * 1);
                    tax2 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl24').value) * (fill_zero(defaultSGST) / 100);
                    totSGST += fill_zero(tax2) * 1;
                }
            }
            var addedC = "N", addedS = "N";
            if (fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl24').value) > 0) {
                //debugger;
                totCGST = 0;
                totSGST = 0;
                var OthChargesTaxable = Number(document.getElementById('ContentPlaceHolder1_txtlbl24').value);
                gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
                for (var i = 0; i < gridCount; i++) {

                    var rowI = 0;
                    if (i == 0) rowI = 0;
                    else rowI = i * 2;
                    var stdPacking = 1;

                    if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText == "56" && Number(impTaxAmt[i].value) > 0 && checkTax == "Y") {

                        tax1 = ((Number(impTaxAmt[i].value) + Number(tool_amtz)) * (CGST[i].value * 1) / 100).toFixed(3);

                        totCGST = ((totCGST * 1) + (tax1 * 1)).toFixed(3);
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = fill_zero(tax1 * 1).toFixed(3);
                    }
                    else if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText != "42" && checkTax == "Y") {
                        if ((CGST[i].value * 1) > 0) {
                            //totCGST += (AMOUNT * (CGST[i].value * 1) / 100);
                            if (highestPercentCGST == CGST[i].value && addedC == "N") {
                                tax1 = ((Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value) + tool_amtz + OthChargesTaxable) * (CGST[i].value * 1) / 100).toFixed(3);
                                addedC = "Y";
                            }
                            else tax1 = ((Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value) + tool_amtz) * (CGST[i].value * 1) / 100).toFixed(3);

                            totCGST = ((totCGST * 1) + (tax1 * 1)).toFixed(3);
                            document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = fill_zero(tax1 * 1).toFixed(3);
                        }
                        if ((SGST[i].value * 1) > 0) {
                            if (highestPercentSGST == SGST[i].value && addedS == "N") {
                                tax2 = ((Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value) + tool_amtz + OthChargesTaxable) * (SGST[i].value * 1) / 100).toFixed(3);
                                addedS = "Y";
                            }
                            else
                                tax2 = ((Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value) + tool_amtz) * (SGST[i].value * 1) / 100).toFixed(3);
                            totSGST = ((totSGST * 1) + (tax2 * 1)).toFixed(3);
                            document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = fill_zero(tax2 * 1).toFixed(3);
                        }
                    }
                    else {
                        tax1 = 0; tax2 = 0; totCGST = 0; totSGST = 0;
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = 0;
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = 0;
                    }
                }
            }


            totCGST = (totCGST * 1).toFixed(2);
            totSGST = (totSGST * 1).toFixed(2);

            //document.getElementById('ContentPlaceHolder1_txtCashDiscValue').value = totCashDisc.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtlbl25').value = Number(IAMOUNT).toFixed(roundUpto);
            document.getElementById('ContentPlaceHolder1_txtlbl27').value = Number(totCGST).toFixed(roundUpto);
            document.getElementById('ContentPlaceHolder1_txtlbl29').value = Number(totSGST).toFixed(roundUpto);

            totCashDisc = 0;
            if (totCashDisc > 0) {
                TOTAMT = ((Number(IAMOUNT).toFixed(roundUpto) * 1) - Number(totCashDisc)) + (Number(totCGST).toFixed(roundUpto) * 1) + (Number(totSGST).toFixed(roundUpto) * 1);
            }
            else {
                TOTAMT = (Number(IAMOUNT).toFixed(roundUpto) * 1) + (Number(totCGST).toFixed(roundUpto) * 1) + (Number(totSGST).toFixed(roundUpto) * 1);
            }
            tcs_val = document.getElementById('ContentPlaceHolder1_txtTCSA').value;
            if (tcs_val > 0) {
                tcs_amt = Number(tcs_val);
                TOTAMT += tcs_amt;
            }

            var tdsPer = 0; var tdsVal = 0;
            var cutTDS = "N";
            cutTDS = document.getElementById('ContentPlaceHolder1_txtCutTDS').value;
            tdsPer = document.getElementById('ContentPlaceHolder1_txtTDSPer').value;
            if (tdsPer > 0 && cutTDS == "Y") {
                tdsVal = fill_zero(IAMOUNT * (tdsPer / 100));
                //txtCutTDS
                document.getElementById('ContentPlaceHolder1_txtTDSAmt').value = tdsVal.toFixed(2);
            }
            else document.getElementById('ContentPlaceHolder1_txtTDSAmt').value = "0";

            document.getElementById('ContentPlaceHolder1_txtlbl31').value = Number(TOTAMT).toFixed(roundUpto);
        }

        function funTotalDRCR() {
            //txttotCr
            //
            var totDR = 0;
            var totCR = 0;
            var totRows = parseInt('<%= this.sg3.Rows.Count %>') - 1;
            for (var i = 0; i < totRows; i++) {
                totDR += (document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i + '').value * 1);
                totCR += (document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i + '').value * 1);
            }

            document.getElementById('ContentPlaceHolder1_txttotDr').innerText = totDR.toFixed(2);
            document.getElementById('ContentPlaceHolder1_txttotCr').innerText = totCR.toFixed(2);

            document.getElementById('ContentPlaceHolder1_txttotDiff').innerText = Math.round(totDR - totCR, 2);

            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            var Qty = 0;
            for (var i = 0; i < gridCount; i++) {

                Qty += document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value * 1;

            }

            document.getElementById('ContentPlaceHolder1_txtTotQty').innerText = Math.round(Qty, 2);
        }

        function closePopup() {
            
            $("#btnBack", window.parent.document).trigger("click"), parent.$.colorbox.close()
            //document.getElementById("btnBack").click();
            //parent.$.colorbox.close();
        }
        function makeRCMCalc() {
            if (Number(document.getElementById('ContentPlaceHolder1_txtImpTaxValue').value) > 0) {
                //document.getElementById('ContentPlaceHolder1_txtRcmAmount').value = Number(document.getElementById('ContentPlaceHolder1_txtImpTaxValue').value) * (0.15);
            }
        }
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
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
                        <button type="submit" id="btnatch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnatch_ServerClick">Attachment</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>

                    </td>

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
                                        <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100px" CssClass="form-control" Height="25px"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                        </div>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4a" runat="server" Width="350px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7a" runat="server" Width="350px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl70" runat="server" Text="Place_of_supply" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl70" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl70_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl70" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl71" runat="server" Width="350px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>

                                <tr id="liBarcode" runat="server">
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="Barcode" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td></td>
                                    <td colspan="3">
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtBarCode" runat="server" OnTextChanged="txtBarCode_TextChanged" Width="300px" AutoPostBack="true"></asp:TextBox>
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
                            <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"></asp:Label>

                            <%--                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>--%>

                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl3" placeholder="Date" runat="server" Width="100px" CssClass="form-control" Height="25px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" runat="server"
                                    Enabled="True" TargetControlID="txtlbl3"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtlbl3" />
                            </div>



                            <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-1 control-label"></asp:Label>
                            <div class="col-sm-1">
                                <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="ImageButton5_Click" />
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl5" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-2 control-label"></asp:Label>

                            <%--                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl6" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>--%>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl6" placeholder="Date" runat="server" Width="100px" CssClass="form-control" Height="25px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" runat="server"
                                    Enabled="True" TargetControlID="txtlbl6"
                                    Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                    MaskType="Date" TargetControlID="txtlbl6" />
                            </div>


                            <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl8" runat="server" CssClass="form-control" Height="25px" ReadOnly="true"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl9" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>

                            <asp:Label ID="Label16" runat="server" Text="Chl_No" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtChlno" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="Label17" runat="server" Text="Chl_Dt" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtChlDt" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>

                            <asp:Label ID="Label1" runat="server" Text="Our_State" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl72" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="Label2" runat="server" Text="Cust_State" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl73" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>

                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">See Voucher</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li>
                                    <asp:Button ID="btncheckTax" runat="server" CssClass="bg-green btn-foursquare" Text="Check & Calculate Tax" Style="left: 30px;" OnClick="btncheckTax_Click" ToolTip="If Tax is not corrected, click on this button and correct the tax amount" />
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkTax" runat="server" Text="&nbsp;&nbsp;Calculate TAX" onchange="calculateSum()" Checked="true" />
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkRoundTax" runat="server" Text="&nbsp;&nbsp;TAX Round off" onchange="calculateSum()" />
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkITC" runat="server" Text="&nbsp;&nbsp;ITC Allowed" Checked="true" />
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;
                                    <asp:CheckBox ID="chkTCS" runat="server" Text="&nbsp;&nbsp;Charge TCS" Checked="true" />
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbltotDr" Text="Total Dr Amount : " runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="txttotDr" Text="0" runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbltotCr" Text="Total Cr Amount : " runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="txttotCr" Text="0" runat="server"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label15" Text="Diff Amount : " runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="txttotDiff" Text="0" runat="server"></asp:Label>


                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label18" Text="Tot Qty : " runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="txtTotQty" Text="0" runat="server"></asp:Label>
                                </li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1" />
                                                <asp:BoundField DataField="sg1_h2" ItemStyle-Width="50px" HeaderStyle-Width="50px" HeaderText="TotOthLC" />
                                                <asp:BoundField DataField="sg1_h3" ItemStyle-Width="50px" HeaderStyle-Width="50px" HeaderText="LC" />
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
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" HeaderStyle-Width="70px" ItemStyle-Width="70px" />


                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndt" runat="server" CommandName="SG1_ROW_DT" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Date" />
                                                        &nbsp;&nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;
                                                        <asp:ImageButton ID="sg1_costcenter" runat="server" CommandName="SG1_COST" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Cost Center" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="40px" />
                                                    <ItemStyle Width="40px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" CssClass="form-control" Height="25px" onkeyup="makeQty()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t17</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t17" runat="server" Text='<%#Eval("sg1_t17") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t18</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t18" runat="server" Text='<%#Eval("sg1_t18") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t19</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t19" runat="server" Text='<%#Eval("sg1_t19") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t20</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t20" runat="server" Text='<%#Eval("sg1_t20") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t21</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t21" runat="server" Text='<%#Eval("sg1_t21") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t22</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t22" runat="server" Text='<%#Eval("sg1_t22") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t23</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t23" runat="server" Text='<%#Eval("sg1_t23") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t24</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t24" runat="server" Text='<%#Eval("sg1_t24") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t25</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t25" runat="server" Text='<%#Eval("sg1_t25") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t26</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t26" runat="server" Text='<%#Eval("sg1_t26") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tcode</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btntax" runat="server" CommandName="SG1_ROW_TAX" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Choose Tax" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl10" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl11" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl12" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl13" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl14" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
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
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Terms</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe; max-height: 250px; overflow: scroll">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" HeaderStyle-Width="40px" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="Code" HeaderStyle-Width="100px" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Account_Name" HeaderStyle-Width="350px" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dr.Amt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="150px" CssClass="form-control" Height="25px" onkeypress="funTotalDRCR()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Cr.Amt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="150px" CssClass="form-control" Height="25px" onkeypress="funTotalDRCR()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ref.No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Ref.Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Naration</HeaderTemplate>
                                                    <HeaderStyle Width="250px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t5" runat="server" Text='<%#Eval("sg3_t5") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl40" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl41" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl42" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl43" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl44" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl45" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
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
                                                                    <asp:TextBox ID="txtlbl46" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl47" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl48" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl49" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl50" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl51" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnadd" runat="server" CommandName="SG4_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnrmv" runat="server" CommandName="SG4_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg4_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Field</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </section>


                <%--                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" placeholder="Remarks" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>--%>


                <div class="col-md-6">
                    <div>
                        <div class="box-body" id="invDiv" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl15" runat="server" Text="lbl15"></asp:Label>
                                        <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl15" runat="server" Width="250px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>


                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Reason(Dr/Cr Note)"></asp:Label>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnbiz_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbizgrp" runat="server" Width="150px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl16" runat="server" Text="lbl16"></asp:Label><asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl16" runat="server" Width="250px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Voucher Class"></asp:Label>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncc1_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtcc_1" runat="server" Width="180px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>


                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl17" runat="server" Text="lbl17"></asp:Label>
                                        <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl17" runat="server" Width="250px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTDS" runat="server" Text="TDS(%)"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTDSPer" runat="server" Width="50px" Style="margin-left: 10px;" CssClass="form-control" Height="25px" MaxLength="5" onkeypress="calculateSum()"></asp:TextBox></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTDSAmt" runat="server" Width="100px" CssClass="form-control" Height="25px" MaxLength="5" onkeypress="calculateSum()"></asp:TextBox>
                                    </td>
                                    <td style="display: none">
                                        <asp:Label ID="Label9" runat="server" Text="C.Centre(Level 2)"></asp:Label>
                                        <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncc2_Click" />
                                    </td>
                                    <td style="display: none">
                                        <asp:TextBox ID="txtcc_2" runat="server" Width="150px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl18" runat="server" Text="lbl18"></asp:Label>
                                        <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>

                                    <td>
                                        <asp:TextBox ID="txtlbl18" runat="server" Width="250px" CssClass="form-control" Height="25px"></asp:TextBox></td>
                                    <td>

                                        <asp:Label ID="lblTCS" runat="server" Text=" TCS"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTCSA" runat="server" Width="100px" CssClass="form-control" Height="25px" onkeypress="calculateSum()" Style="text-align: right;"></asp:TextBox>
                                    </td>

                                    <td style="display: none">
                                        <asp:Label ID="Label10" runat="server" Text="C.Centre(Level 3)"></asp:Label>
                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncc3_Click" />
                                    </td>
                                    <td style="display: none">
                                        <asp:TextBox ID="txtcc_3" runat="server" Width="150px" CssClass="form-control" Height="25px"></asp:TextBox>
                                    </td>



                                </tr>
                            </table>
                        </div>

                        <div class="box-body" id="multDiv" runat="server">
                            <table style="width: 100%">
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Insurance Charges"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="insuChk" runat="server" onclick="calculateSum()" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInsuCharge" runat="server" onkeypress="calculateSum()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Old Balance"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkOldBal" runat="server" onclick="calculateSum()" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOldBalance" runat="server" onkeypress="calculateSum()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Other Chrg"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkOthChrg" runat="server" onclick="calculateSum()" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOthChrg" runat="server" onkeypress="calculateSum()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Advance Rcvd"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAdvRcvd" runat="server" onclick="calculateSum()" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAdvRcvd" runat="server" onkeypress="calculateSum()"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>


                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="lbl24" runat="server" Text="lbl24" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl24" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl25" runat="server" Text="lbl25" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl25" runat="server" Style="text-align: right" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl26" runat="server" Text="lbl26" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl26" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl27" runat="server" Text="lbl27" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl27" runat="server" Style="text-align: right" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl28" runat="server" Text="lbl28" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl28" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl29" runat="server" Text="lbl29" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl29" runat="server" Style="text-align: right" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl30" runat="server" Text="lbl30" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl30" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl31" runat="server" Text="lbl31" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl31" runat="server" Style="text-align: right" CssClass="form-control" Height="25px"></asp:TextBox>
                                <asp:TextBox ID="txtTax" runat="server" Style="display: none" Text="Y"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-12" id="divRmk" runat="server">
                    <div>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td></td>
                                    <td style="border-top: groove; border-left: groove; text-align: center">View</td>
                                    <td style="border-top: groove; text-align: center">View Attachments 
                                    </td>
                                    <td style="border-top: groove; border-right: groove; text-align: center">Examples
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 65%" rowspan="4">
                                        <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" placeholder="Remarks" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td style="border-left: groove;">
                                        <asp:Button ID="btnGEView" runat="server" CssClass="btn-default" Text="View G.E." Width="120px" OnClick="btnGEView_Click" ToolTip="Gate Entry Print preview" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGE" runat="server" CssClass="btn-default" Text="View Attachments" OnClick="btnGE_Click" ToolTip="See the attached file in GE" />
                                    </td>
                                    <td style="border-right: groove">Photos of Material at gate/Guard/Truck</td>
                                </tr>
                                <tr>

                                    <td style="border-left: groove">
                                        <asp:Button ID="btnMRRView" runat="server" CssClass="btn-default" Text="View GRN/MRR" Width="120px" OnClick="btnMRRView_Click" ToolTip="GRN/MRR Print preview" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnMRR" runat="server" CssClass="btn-default" Text="View Attachments" OnClick="btnMRR_Click" ToolTip="See the attached file in MRR" />
                                    </td>

                                    <td style="border-right: groove">Vendor bill copy, GR, Weigh Slip</td>
                                </tr>
                                <tr>
                                    <td style="border-left: groove; border-bottom: groove">
                                        <asp:Button ID="btnPOView" runat="server" CssClass="btn-default" Text="View P.O." Width="120px" OnClick="btnPOView_Click" ToolTip="Purchase Order Print preview" />
                                    </td>
                                    <td style="border-bottom: groove">
                                        <asp:Button ID="btnPO" runat="server" CssClass="btn-default" Text="View Attachments" OnClick="btnPO_Click" ToolTip="See the attached file in PO" />
                                    </td>

                                    <td style="border-bottom: groove; border-right: groove">Brochure, Quotation</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-6" id="divRcm" runat="server">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="Label12" runat="server" Text="Custom_Duty" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtImpTaxValue" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" CssClass="form-control" onkeyup="makeRCMCalc();" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="Label11" runat="server" Text="RCM_Amount" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtRcmAmount" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" CssClass="form-control" onkeyup="makeRCMCalc();" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="Label13" runat="server" Text="Clearing_Charges" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtFr" runat="server" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </section>
    </div>

    <div style="display: none">
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
            AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
            Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_hoso" runat="server" />
    <asp:HiddenField ID="doc_GST" runat="server" />
    <asp:HiddenField ID="brPrefixWithInvNo" runat="server" />

    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="hfRoundOff" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfcocd" runat="server" />
    <asp:HiddenField ID="hfw122" runat="server" />

    <asp:HiddenField ID="hfPacking" runat="server" />
    <asp:HiddenField ID="hfInsurance" runat="server" />
    <asp:HiddenField ID="hfFrieght" runat="server" />
    <asp:HiddenField ID="hfOther" runat="server" />

    <asp:HiddenField ID="hf150" runat="server" />
    <asp:HiddenField ID="hf151" runat="server" />

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
    <asp:TextBox ID="txtTCS" runat="server" Style="text-align: right; display: none"></asp:TextBox>
    <asp:TextBox ID="txtCutTDS" runat="server" Style="text-align: right; display: none"></asp:TextBox>
</asp:Content>
