<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_Da_entry" MaintainScrollPositionOnPostback="true" CodeFile="om_Da_entry.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            calculateSum();
        });
        function openBox(tk) {
            if (event.keyCode == 13) {
                document.getElementById('ContentPlaceHolder1_hf1').value = tk.id;
                if (document.getElementById('ContentPlaceHolder1_hfcocd').value == "AERO") {
                    document.getElementById('ContentPlaceHolder1_btnGridPop').click();
                }
            }
        }
        function check(days) {
            var grid = $("[id*=sg1].GridviewScrollItem2").length - 1;

            for (var i = 0; i < grid; i++) {
                var xbarcodeval = document.getElementById('ContentPlaceHolder1_sg1_sg1_t2_' + i).value;
                //if (document.getElementById('ContentPlaceHolder1_txtscanbarcode').value == xbarcodeval) {
                //    //document.getElementById('ContentPlaceHolder1_txtscanbarcode').value = "";
                //    alert("The row for this barcode has already been entered");
                //    document.getElementById('ContentPlaceHolder1_txtscanbarcode').focus();
                //    return;
                //}

            }
        }
        function makeQty() {
            var convertPkgToNumb = document.getElementById('ContentPlaceHolder1_hfW18').value;
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            for (var i = 0; i < gridCount; i++) {
                var rowI0 = 0;
                if (i == 0) rowI0 = 0;
                else rowI0 = i * 2;
                var stdPacking = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI0].cells[2].innerHTML;
                var noOfPack = $("input[id*=sg1_t1]");
                if (fill_zero(stdPacking) <= 0) stdPacking = 1;
                if (convertPkgToNumb == "Y") {
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value = fill_zero(noOfPack[i].value * stdPacking);
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t20_' + i).value = noOfPack[i].value + " X " + stdPacking;
                }
            }
            accsBOMQty();
        }
        function makeBatch() {
            var convertPkgToNumb = document.getElementById('ContentPlaceHolder1_hfW18').value;
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            for (var i = 0; i < gridCount; i++) {
                var rowI0 = 0;
                if (i == 0) rowI0 = 0;
                else rowI0 = i * 2;
                var stdPacking = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI0].cells[2].innerHTML;
                var noOfPack = $("input[id*=sg1_t1]");
                var qty = $("input[id*=sg1_t3]");
                var totBatch = 0;
                if (fill_zero(stdPacking) <= 0) stdPacking = 1;
                if (convertPkgToNumb == "Y") {
                    totBatch = Math.trunc(qty[i].value / stdPacking) + ((qty[i].value % stdPacking) > 0 ? 1 : 0);

                    var roundBatchQty = Math.trunc(qty[i].value / stdPacking);
                    var restQty = qty[i].value - (stdPacking * Math.trunc(qty[i].value / stdPacking));
                    debugger;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value = fill_zero(totBatch);
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t20_' + i).value = roundBatchQty + " X " + stdPacking + "" + ((restQty * 1) > 0 ? " / 1 X " + restQty : "");
                }
            }
            accsBOMQty();
        }

        function accsBOMQty() {
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            for (var i = 0; i < gridCount; i++) {
                debugger;
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;
                var accs = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[4].innerHTML;
                var Qty = $("input[id*=sg1_t3]");
                var acscCode = $("input[id*=sg1_t22]");
                if (accs == "ACCS") {

                    var gridCountx = $("[id*=sg1].GridviewScrollItem2").length - 1;
                    for (var i0 = 0; i0 < gridCountx; i0++) {
                        var rowI0 = 0;
                        if (i0 == 0) rowI0 = 0;
                        else rowI0 = i0 * 2;
                        var accsx = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI0].cells[4].innerHTML;
                        var acscxCode = document.getElementById('ContentPlaceHolder1_sg1_sg1_t22_' + i0).value;
                        if (accsx == "ACCS-C") {
                            if (acscCode[i].value.length > 8) {
                                if (acscCode[i].value.toString().substring(0, 8) == acscxCode.toString().substring(0, 8)) {
                                    var ibqty = acscxCode.split("~")[1];
                                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i0).value = (ibqty * Qty[i].value);
                                }
                            }
                        }
                    }
                }
            }
        }
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

            var grid = $("[id*=sg1].GridviewScrollItem2").length - 1;

            makeBatch();
            // here , it should be like upper line 
                <%--var grid = document.getElementById("<%= sg1.ClientID%>");--%>
            for (var i = 0; i < grid; i++) {
                var Qty = $("input[id*=sg1_t3]");
                var Rate = $("input[id*=sg1_t4]");
                var DiscPer = $("input[id*=sg1_t5]");

                var CGST = $("input[id*=sg1_t7]");
                var SGST = $("input[id*=sg1_t8]");
                var tool_pu = $("input[id*=sg1_t11]");

                var AMOUNT = 0;

                var tax1 = 0;
                var tax2 = 0;

                var stdPacking = 1;
                //debugger;
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;
                stdPacking = document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[2].innerHTML;
                if (fill_zero(stdPacking) <= 0) stdPacking = 1;

                AMOUNT = (Qty[i].value * 1) * (Rate[i].value * 1);
                tool_amtz = (Qty[i].value * 1) * (tool_pu[i].value * 1);

                txb_val = AMOUNT + tool_amtz;

                if (fill_zero(DiscPer[i].value) > 0) {
                    AMOUNT = AMOUNT - (AMOUNT * DiscPer[i].value / 100)
                }
                //else AMOUNT = AMOUNT - fill_zero(DiscRs[i].value);

                //document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value = Math.round(fill_zero(Qty[i].value / stdPacking));

                document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value = fill_zero(AMOUNT).toFixed(3);

                IAMOUNT += fill_zero(AMOUNT);
                if ((CGST[i].value * 1) > 0) {
                    //totCGST += (AMOUNT * (CGST[i].value * 1) / 100);

                    tax1 = ((AMOUNT + tool_amtz) * (CGST[i].value * 1) / 100);

                    totCGST = totCGST + tax1;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = fill_zero(tax1).toFixed(3);
                }
                if ((SGST[i].value * 1) > 0) {

                    tax2 = ((AMOUNT + tool_amtz) * (SGST[i].value * 1) / 100);
                    totSGST = totSGST + tax2;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = fill_zero(tax2).toFixed(3);

                }
            }

            document.getElementById('ContentPlaceHolder1_txtlbl25').value = fill_zero(IAMOUNT).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtlbl27').value = fill_zero(totCGST).toFixed(3);
            document.getElementById('ContentPlaceHolder1_txtlbl29').value = fill_zero(totSGST).toFixed(3);

            TOTAMT = fill_zero(IAMOUNT) + fill_zero(totCGST) + fill_zero(totSGST);

            tcs_val = document.getElementById('ContentPlaceHolder1_txtTCS').value;
            if (tcs_val > 0) {
                tcs_amt = (tcs_val * TOTAMT) / 100;
                document.getElementById('ContentPlaceHolder1_txtTCSA').value = tcs_amt;
                TOTAMT += tcs_amt;
            }

            document.getElementById('ContentPlaceHolder1_txtlbl31').value = fill_zero(TOTAMT).toFixed(3);
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
                        <button type="submit" id="btnAtch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnAtch_ServerClick">Attachment</button>
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
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4a" runat="server" Width="350px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7a" runat="server" Width="350px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>




                                <tr>
                                    <td>
                                        <asp:Label ID="lbl70" runat="server" Text="P.O.S" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl70" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl70_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl70" runat="server" Width="80px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl71" runat="server" Width="350px" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
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
                            <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"></asp:Label></td>
                             <div class="col-sm-4">
                                 <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                             </div>
                            <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-2 control-label"></asp:Label></td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl5" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                            <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-2 control-label"></asp:Label></td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl6" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                            <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label></td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl8" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                            <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label"></asp:Label></td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl9" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                            <asp:Label ID="Label1" runat="server" Text="Our_State/Province" CssClass="col-sm-2 control-label"></asp:Label></td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl72" runat="server" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                            <asp:Label ID="Label2" runat="server" Text="Cust_State/Province" CssClass="col-sm-2 control-label"></asp:Label></td>
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
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li id="btnItemSelection" runat="server">
                                    <asp:Button ID="btnIcode" runat="server" CssClass="bg-green btn-foursquare" Text="Select Item Code" OnClick="btnIcode_Click" />
                                    <asp:Label ID="lblSODetail" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hfsonum" runat="server" />
                                    <asp:HiddenField ID="hfOrderQty" runat="server" />
                                    <asp:HiddenField ID="hfIrate" runat="server" />
                                </li>
                                <li>&nbsp;
                                    <asp:TextBox type="text" ID="txtscanbarcode" runat="server" Width="220px" TextMode="MultiLine" placeholder="Scan Barcode here" OnTextChanged="txtscanbarcode_TextChanged" onblur="check(this)"></asp:TextBox>
                                </li>

                                <li>
                                    <asp:Button ID="Button1" runat="server" CssClass="bg-green btn-foursquare" Text="Scan Barcode" Style="margin-left: 30px;" OnClick="Button1_Click" />
                                </li>
                                <li>
                                    <asp:Button ID="Button2" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 2" Style="margin-left: 5px;" OnClick="Button2_Click" />
                                </li>
                                <li>
                                    <asp:Button ID="Button3" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 3" Style="margin-left: 5px;" OnClick="Button3_Click" />
                                </li>
                                <li>
                                    <asp:Button ID="Button4" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 4" Style="margin-left: 5px;" OnClick="Button4_Click" />
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
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
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
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" CssClass="form-control" Height="25px" onkeyup="makeQty()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndt" runat="server" CommandName="SG1_ROW_DT" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Date" />
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
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="makeBatch(); accsBOMQty(); calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()" CssClass="form-control" Height="25px"></asp:TextBox>
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
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%" onkeyup="calculateSum(); openBox(this);" CssClass="form-control" Height="25px"></asp:TextBox>
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
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
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
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
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
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
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
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
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
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
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
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ERP_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dlv_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sch.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Prod.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Card</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="1" Width="100%" CssClass="form-control" Height="25px"></asp:TextBox>
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
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true" CssClass="form-control" Height="25px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" MaxLength="40" CssClass="form-control" Height="25px"></asp:TextBox>
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
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl15" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl16" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl17" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl18" runat="server" Width="350px" CssClass="form-control" Height="25px"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>

                                <tr id="netwt" runat="server">
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Net_Weight" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" Visible="false" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" />

                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtnetwt" runat="server" Width="350px" Style="text-align: right" CssClass="form-control" Height="25px" MaxLength="10"></asp:TextBox>
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
                                <asp:TextBox ID="txtlbl26" runat="server" CssClass="form-control" Height="25px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl27" runat="server" Text="lbl27" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl27" runat="server" Style="text-align: right" CssClass="form-control" Height="25px"></asp:TextBox>
                            </div>
                            <asp:Label ID="lbl28" runat="server" Text="lbl28" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtlbl28" runat="server" CssClass="form-control" Height="25px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right"></asp:TextBox>
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
                            </div>
                            <div class="form-group" id="groswt" runat="server">
                                <asp:Label ID="Label4" runat="server" Text="Gross_Weight" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtgroswt" runat="server" Style="text-align: right" MaxLength="10" CssClass="form-control" Height="25px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" placeholder="Remarks" CssClass="form-control"></asp:TextBox>
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
    <asp:HiddenField ID="doc_hosopw" runat="server" />
    <asp:HiddenField ID="doc_hoso" runat="server" />
    <asp:HiddenField ID="doc_GST" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:HiddenField ID="hfW120" runat="server" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="hfW18" runat="server" />
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:TextBox ID="txtTCS" runat="server" Style="text-align: right; display: none"></asp:TextBox>
    <asp:TextBox ID="txtTCSA" runat="server" Style="text-align: right; display: none"></asp:TextBox>
    <asp:Button ID="btnGridPop" runat="server" OnClick="btnGridPop_Click" Style="display: none" />
    <asp:HiddenField ID="hfcocd" runat="server" />
</asp:Content>
