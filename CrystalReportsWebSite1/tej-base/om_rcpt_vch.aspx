<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_rcpt_vch" CodeFile="om_rcpt_vch.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            if (gridCount > 1) {
                calculateSum();
            }
        });
        function calculateSum() {
            var vp = 0; var vp1 = 0; var fill_amt = 0;
            //var gridCount = parseInt("<%= this.sg1.Rows.Count %>") - 1;
            var totDr = 0, totCr = 0;
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            var zk = 0;
            if (document.getElementById("ContentPlaceHolder1_lbl1a").innerText.substring(0, 1) == 1)
                totDr = Number(document.getElementById("ContentPlaceHolder1_txttamt").value);
            debugger
            for (var i = 0; i < gridCount; i++) {
                zk = i;
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;
                var chk_result = document.getElementById("ContentPlaceHolder1_sg1_chk1_" + zk).checked;
                if (chk_result == true) {
                    document.getElementById('ContentPlaceHolder1_hf2').value = zk;
                    if (document.getElementById("ContentPlaceHolder1_lbl1a").innerText.substring(0, 1) == 1) {
                        if (document.getElementById("ContentPlaceHolder1_txttrefnum").value == "" || document.getElementById("ContentPlaceHolder1_txttrefnum").value == "-") {
                            document.getElementById("ContentPlaceHolder1_hf1").value = "CHQMSG";
                            makeZeroAllTxt();
                            openBox();
                            break;
                        }
                        if ((document.getElementById("ContentPlaceHolder1_txttamt").value * 1) <= 0) {
                            document.getElementById("ContentPlaceHolder1_hf1").value = "CHQAMSG";
                            makeZeroAllTxt();
                            openBox();
                            break;
                        }
                    }

                    if (document.getElementById("ContentPlaceHolder1_lbl1a").innerText.substring(0, 1) == 2) {
                        if (document.getElementById("ContentPlaceHolder1_txttrefnum").value == "" || document.getElementById("ContentPlaceHolder1_txttrefnum").value == "-") {
                            document.getElementById("ContentPlaceHolder1_hf1").value = "CHQMSG";
                            makeZeroAllTxt();
                            openBox();
                            break;
                        }
                    }

                    //if (fill_zero(document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value) <= 0)
                    document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value = fill_zero(document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[9].innerHTML).toFixed(2);
                  

                    //debugger;
                    var tval = 0.00;
                    if (document.getElementById("ContentPlaceHolder1_sg1_dd2_" + zk + "").value == "DR") {
                        var tval = 0.00;
                        if (Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value) > 0) {

                            tval= Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value);
                        }
                        else {
                            tval= Number(document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value);
                        }

                        if (tval < 0) {
                            totCr += tval;
                            vp1 -= (tval);
                        }
                        else {
                            totDr += tval;
                            vp1 = (tval*2);
                        }
                       
                        //var tamt;
                        //if (fill_zero(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value) > 0) {
                        //    tamt = Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value).toFixed(2);
                        //    //if (document.getElementById('ContentPlaceHolder1_edmode').value=="Y")
                        //}
                        //else {
                        //    tamt = Number(document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value).toFixed(2);
                        //}
                        //if (tamt < 0) {
                        //    fill_amt = -tamt;
                        //}
                    }
                    else {
                        tval = 0.00;
                        if (Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value) > 0) {
                            tval= Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value);
                        }
                        else {
                            tval= Number(document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value);
                        }
                        if (tval > 0) {
                            totCr += tval;
                            vp1 = (tval);
                        }
                        else {
                            totDr += tval;
                            vp1 -= -(tval * 2);
                        }
                        
                       
                        //var tamt;
                        //if (fill_zero(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value) > 0) {
                        //    fill_amt = Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value).toFixed(2);
                        //    //if (document.getElementById('ContentPlaceHolder1_edmode').value=="Y")
                        //}
                        //else {
                        //    fill_amt = Number(document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value).toFixed(2);
                        //}
                    }

                    debugger;
                    document.getElementById("ContentPlaceHolder1_lblTotDr").innerText = totDr.toFixed(2);
                    document.getElementById("ContentPlaceHolder1_lblTotCr").innerText = totCr.toFixed(2);
                    document.getElementById("ContentPlaceHolder1_lblDiff").innerText = (totDr - totCr).toFixed(2);
                }
                else {
                    document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "").value = 0; fill_amt = 0;
                    document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value = 0; fill_amt = 0;
                }
                
                vp = Number(vp) + Number(document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value);
                //vp1 = Number(vp1) + Number(fill_amt);

                //var passfor = document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value;
                //if ((passfor * 1) < 0)
                //    document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + zk + "").value = (passfor * -1);
            }

            document.getElementById("ContentPlaceHolder1_lblqtysum").innerHTML = Number(vp).toFixed(2);
            document.getElementById("ContentPlaceHolder1_txtbillamount").value = Number(vp1).toFixed(2);

            if (document.getElementById("ContentPlaceHolder1_lbl1a").innerText.substring(0, 1) == 2) {
                debugger;
                document.getElementById("ContentPlaceHolder1_txttamt").value = (totDr - totCr).toFixed(2);
            }
            else {
                document.getElementById("ContentPlaceHolder1_txtbalamt").value = (fill_zero(document.getElementById("ContentPlaceHolder1_txtbillamount").value) - (fill_zero(document.getElementById("ContentPlaceHolder1_txttamt").value) + fill_zero(document.getElementById("ContentPlaceHolder1_txtothamt").value))).toFixed(2);
            }

            if (document.getElementById("ContentPlaceHolder1_lbl1a").innerText.substring(0, 1) == 1) {
                if (((document.getElementById("ContentPlaceHolder1_txtbillamount").value * 1) > ((document.getElementById("ContentPlaceHolder1_txttamt").value * 1) + (document.getElementById("ContentPlaceHolder1_txtothamt").value * 1))) && document.getElementById("ContentPlaceHolder1_hf1").value != "BALEXCEED") {
                    //document.getElementById("ContentPlaceHolder1_hf1").value = "BALEXCEED";
                    //openBox();
                    return;
                }
            }
        }
        function makeTick(indx) {
            if (fill_zero(document.getElementById(indx.id).value) != 0 || (document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + (indx.id.replace("ContentPlaceHolder1_sg1_txtmanualfor_", ""))).value * 1) != 0) {
                document.getElementById("ContentPlaceHolder1_sg1_chk1_" + (indx.id.replace("ContentPlaceHolder1_sg1_txtmanualfor_", ""))).checked = true;
            }
            else {
                document.getElementById("ContentPlaceHolder1_sg1_chk1_" + (indx.id.replace("ContentPlaceHolder1_sg1_txtmanualfor_", ""))).checked = false;
            }
            calculateSum();
        }
        function makeZeroAllTxt() {
            var gridCountxx = parseInt("<%= this.sg1.Rows.Count %>") - 1;
            for (var xx = 0; xx < gridCountxx; xx++) {
                document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + xx).value = 0;
                document.getElementById("ContentPlaceHolder1_sg1_chk1_" + xx + "").checked = false;
            }
        }
        function checkNetAmt() {
            var gridCountxx = parseInt("<%= this.sg1.Rows.Count %>") - 1;
            for (var xx = 0; xx < gridCountxx; xx++) {
                var rowI = 0;
                if (xx == 0) rowI = 0;
                else rowI = xx * 2;
                if (Number(document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[9].innerHTML) > 0) {
                    if (Number(document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + xx).value) > Number(document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[9].innerHTML)) {
                        document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + xx).style.borderColor = "Red";
                        document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + xx).value = Number(document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[9].innerHTML).toFixed(2);
                    }
                    else document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + xx).style.borderColor = "Grey";
                }
            }
        }
        function openBox() {
            document.getElementById('ContentPlaceHolder1_btnGridPop').click();
        }
        function insertRow(tk) {
            var ctlr = false;
            if (event.keyCode == 17) ctlr = true;
            if (ctrl == true && event.keyCode == 73) {
                document.getElementById('ContentPlaceHolder1_hf1').value = tk.id;
                document.getElementById('ContentPlaceHolder1_btnGridPop').click()
            }
        }
        function makeTick(indx) {
            if (fill_zero(document.getElementById(indx.id).value) != 0 || (document.getElementById("ContentPlaceHolder1_sg1_txtpassfor_" + (indx.id.replace("ContentPlaceHolder1_sg1_txtmanualfor_", ""))).value * 1) != 0) {
                document.getElementById("ContentPlaceHolder1_sg1_chk1_" + (indx.id.replace("ContentPlaceHolder1_sg1_txtmanualfor_", ""))).checked = true;
            }
            else {
                document.getElementById("ContentPlaceHolder1_sg1_chk1_" + (indx.id.replace("ContentPlaceHolder1_sg1_txtmanualfor_", ""))).checked = false;
            }
            calculateSum();
        }
        function makeZeroAllTxt() {
            var gridCountxx = parseInt("<%= this.sg1.Rows.Count %>") - 1;
            for (var xx = 0; xx < gridCountxx; xx++) {
                document.getElementById("ContentPlaceHolder1_sg1_txtmanualfor_" + xx).value = 0;
                document.getElementById("ContentPlaceHolder1_sg1_chk1_" + xx + "").checked = false;
            }
        }
        function openBox() {
            document.getElementById('ContentPlaceHolder1_btnGridPop').click();
        }
        function insertRow(tk) {
            var ctlr = false;
            if (event.keyCode == 17) ctlr = true;
            if (ctrl == true && event.keyCode == 73) {
                document.getElementById('ContentPlaceHolder1_hf1').value = tk.id;
                document.getElementById('ContentPlaceHolder1_btnGridPop').click()
            }
        }
        function closePopup() {
            $("#btnBack", window.parent.document).trigger("click"), parent.$.colorbox.close()
        }

        function openfileDialog() {
            $("#ContentPlaceHolder1_xmlUpload").click();
        }
        function submitFile() {
            $("#<%= btnImport.ClientID%>").click();
        };
    </script>

    <script type="text/javascript">
        <%--var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            UpperBound = parseInt('<%= this.sg1.Rows.Count %>') - 1;
            LowerBound = 0;
            SelectedRowIndex = -1;
        }

        function SelectRow(CurrentRow, RowIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            else if (KeyCode == 32) {
                var GridVwHeaderChckbox = document.getElementById("<%=sg1.ClientID %>");
                    if ($('#ContentPlaceHolder1_sg1_txtrmk_' + SelectedRowIndex).is(':focus') == false) {
                        if (GridVwHeaderChckbox.rows[SelectedRowIndex + 1].cells[1].getElementsByTagName("INPUT")[0].checked == true)
                            GridVwHeaderChckbox.rows[SelectedRowIndex + 1].cells[1].getElementsByTagName("INPUT")[0].checked = false;
                        else GridVwHeaderChckbox.rows[SelectedRowIndex + 1].cells[1].getElementsByTagName("INPUT")[0].checked = true;

                        document.getElementById('ContentPlaceHolder1_hf2').value = SelectedRowIndex;
                        calculateSum();
                    }
                }
            //return false;
    }--%>
    </script>
    <style type="text/css">
        .ChkBoxClass input {
            width: 18px;
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_Click"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_Click">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_Click"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_Click"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_Click">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_Click">Lis<u>t</u></button>
                        <button type="submit" id="btnAtch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnAtch_ServerClick">Attachment</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_Click"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_Click">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="lbl1" runat="server" class="col-sm-2 control-label" title="lbl1">Voucher_No.</label>
                                <label id="lbl1a" runat="server" class="col-sm-1 control-label" title="lbl1a">TC</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtvchnum" runat="server" Placeholder="Vch No" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>

                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">Dated</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" CssClass="form-control" Height="28px" Width="100px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>

                                <div class="form-group">
                                    <div class="form-group">
                                        <asp:Label ID="lblPayTerms" runat="server" Text="Terms" CssClass="col-sm-1 control-label"></asp:Label>
                                    </div>
                                </div>

                            </div>

                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-3 control-label" title="lbl1">Bank_Account</label>

                                <div class="col-sm-2">
                                    <asp:TextBox ID="tbank_code" runat="server" placeholder="Code" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="tbank_name" runat="server" placeholder="Bank Name" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Party/Account</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ToolTip="Select Party"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Style="width: 22px; float: right" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtacode" runat="server" placeholder="Code" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtaname" runat="server" placeholder="Party Name" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>


                            <%--                            <div class="form-group">
                                <div class="form-group">
                                    <asp:Label ID="lblrcode" runat="server" CssClass="col-sm-2 control-label"></asp:Label>
                                    <asp:Label ID="lblrname" runat="server" CssClass="col-sm-7 control-label"></asp:Label>
                                    <asp:Label ID="lblPayTerms" runat="server" Text="Payment Term" CssClass="col-sm-3 control-label"></asp:Label>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">

                                <asp:Label ID="Label4" runat="server" Text="Chq/DD#" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttrefnum" runat="server"
                                        Placeholder="Chq/DD" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label8" runat="server" Text="Dated" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtchqdt" runat="server" Placeholder="Chq/DD Dt"
                                        CssClass="form-control" Height="28px" TextMode="Date"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label10" runat="server" Text="Bank_Balance" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="tbank_bal" runat="server"
                                        ReadOnly="true" Style="text-align: right" Height="28px" Placeholder="Bank_Bal." CssClass="form-control"></asp:TextBox>
                                </div>

                                <asp:Label ID="lbl6" runat="server" Text="Party_Balance" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="tparty_bal" runat="server"
                                        ReadOnly="true" Style="text-align: right" Height="28px" Placeholder="Party_Bal." CssClass="form-control"></asp:TextBox>
                                </div>


                                <asp:Label ID="Label3" runat="server" Text="Bank_Amount" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttamt" runat="server"
                                        Placeholder="Bank Amount" CssClass="form-control" Style="text-align: right" Height="28px" onfocusout="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label5" runat="server" Text="Selected_Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbillamount" runat="server" Style="text-align: right" Placeholder="Selected Amt"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>





                                <asp:Label ID="Label7" runat="server" Text="Balance_Amt" CssClass="col-sm-2 control-label" Style="display: none"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbalamt" runat="server" Placeholder="Balance Amt"
                                        CssClass="form-control" Style="text-align: right; display: none" Height="28px"></asp:TextBox>
                                </div>

                                <%--                                <asp:Label ID="lblPayTerms" runat="server" Text="Payment Term" CssClass="col-sm-6 control-label"></asp:Label>                                

                                <asp:Label ID="Label5" runat="server" Text="Chq Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttamt" runat="server" Style="text-align: right"
                                        Placeholder="Chq Amount" CssClass="form-control" Height="28px" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label10" runat="server" Text="" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    </div>
                                <asp:Label ID="Label6" runat="server" Text="Selected_Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbillamount" runat="server" Placeholder="Selected Amt" Style="text-align: right"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label11" runat="server" Text="" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    </div>

                                <div class="form-group">
                                    <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Other Ac Amt</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtothamt" runat="server" placeholder="Oth Amt" Height="28px" CssClass="form-control" Style="text-align: right;"></asp:TextBox>
                                    </div>
                                </div>                            

                                <asp:Label ID="Label7" runat="server" Text="Bal Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbalamt" runat="server" Placeholder="Balance Amt" Style="text-align: right"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Invoice Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Amortz.Dtl</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li style="vertical-align: top">Forex 
                                    <asp:ImageButton ID="btnForxRate" runat="server" ToolTip="Select Party"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Style="width: 22px; display: none" OnClick="btnForxRate_Click" />
                                    <asp:TextBox ID="txtCurrn" runat="server" Width="50px"></asp:TextBox>
                                    <asp:TextBox ID="txtCurrnRate" runat="server" Width="50px"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Button ID="btnExport" runat="server" CssClass="bg-green btn-foursquare" Text="Export Format" Style="margin-left: 20px;" OnClick="btnExport_Click" />
                                </li>
                                <li>
                                    <button id="btnSelFile" class="bg-green btn-foursquare" style="margin-left: 20px;" onclick='openfileDialog();  return false;'>Import Bills</button>

                                </li>
                                <li style="margin-left: 30%">Total DR Amt :
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblTotDr" runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Total CR Amt :
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblTotCr" runat="server"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Difference :
                                    &nbsp;&nbsp;
                                    <asp:Label ID="lblDiff" runat="server"></asp:Label>
                                </li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" Width="100%" Height="290px" AutoGenerateColumns="False"
                                            Style="background-color: #FFFFFF; color: White;" Font-Size="12px" OnPageIndexChanging="sg1_PageIndexChanging" PageSize="200" AllowPaging="true" PagerSettings-Mode="NumericFirstLast" PagerSettings-FirstPageText="First" PagerSettings-LastPageText="Last"
                                            OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound" OnRowCreated="sg1_RowCreated">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>A</HeaderTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnadd" runat="server" CommandName="Row_Add" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="11px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>D</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderStyle Width="35px" />
                                                    <HeaderTemplate>Tick</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="margin-left: 6px;">
                                                            <asp:CheckBox ID="chk1" runat="server" onclick="calculateSum();" CssClass="ChkBoxClass" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" BackColor="Green" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="acode" HeaderText="Code" ReadOnly="false" HeaderStyle-Width="80px"></asp:BoundField>
                                                <asp:BoundField DataField="aname" HeaderText="Account" HeaderStyle-Width="200px" ReadOnly="false"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Inv No</HeaderTemplate>
                                                    <HeaderStyle Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInvno" runat="server" Text='<%#Eval("Invno") %>' MaxLength="20" CssClass="form-control" Height="28px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Inv Date</HeaderTemplate>
                                                    <HeaderStyle Width="140px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtInvDt" runat="server" Text='<%#Eval("invdate") %>' CssClass="form-control" Height="28px" TextMode="Date"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="damt" HeaderText="Bill.Amt" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="camt" HeaderText="Adjusted.Amt" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="net" HeaderText="Net.Amt" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Pass.For</HeaderTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtpassfor" runat="server" Text='<%#Eval("passamt") %>' onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" MaxLength="10" CssClass="form-control" Height="28px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Manual Amt.</HeaderTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtmanualfor" runat="server" Text='<%#Eval("manualamt") %>' onfocusout="makeTick(this);" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onkeyup="checkNetAmt();" onfocus="calculateSum();" onpaste="return false" Style="text-align: right" MaxLength="10" CssClass="form-control" Height="28px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cumbal" HeaderText="Cum.Bal" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dr/Cr</HeaderTemplate>
                                                    <HeaderStyle Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hfdd" runat="server" Value='<%#Eval("hfdd") %>' />
                                                        <select id="dd2" runat="server" style="height: 28px; width: 100%" onchange="calculateSum();">
                                                            <option value="DR">DR</option>
                                                            <option value="CR">CR</option>
                                                        </select>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <HeaderStyle Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtrmk" runat="server" Text='<%#Eval("rmk") %>' MaxLength="50" CssClass="form-control" Height="28px" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onfocus="calculateSum();" onpaste="return false" Style="text-align: right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>TDS(if any)</HeaderTemplate>
                                                    <HeaderStyle Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtdedn" runat="server" Text='<%#Eval("TaxDedn") %>' MaxLength="50" CssClass="form-control" Height="28px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Due_Dt</HeaderTemplate>
                                                    <HeaderStyle Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDueDt" runat="server" Text='<%#Eval("duedt") %>' onkeyup="calculateSum();" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                                        <asp:HiddenField ID="hfM" runat="server" Value='<%#Eval("hfM") %>' />
                                                        <asp:HiddenField ID="hfChk" runat="server" Value='<%#Eval("hfChk") %>' />
                                                        <asp:HiddenField ID="hfLock" runat="server" Value='<%#Eval("hfLock") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>F</HeaderTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnForx" runat="server" CommandName="btnForx" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="11px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="orig_fx_bal" HeaderText="Forex_Rate(Original)" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="curr_fx_bal" HeaderText="Forex_Rate(Current)" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="orig_fx_amt" HeaderText="Forex_Amt(Current)" ReadOnly="True" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="br_acode" HeaderText="BR_ACODE" ReadOnly="True" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
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
                                <div role="tabpanel" class="tab-pane active" id="DescTab2"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 290px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                                        <div class="box-body">

                                            <asp:Label ID="Label14" runat="server" Text="Charge_to_Code" CssClass="col-sm-1 control-label"></asp:Label>
                                            <div class="col-sm-1">
                                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnexp_Click" />
                                            </div>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txt_expcode" runat="server" Height="26px" Placeholder="Charge_to_Code" CssClass="form-control"></asp:TextBox>
                                            </div>

                                            <asp:Label ID="Label15" runat="server" Text="Charge_to_Name" CssClass="col-sm-1 control-label"></asp:Label>

                                            <div class="col-sm-5">
                                                <asp:TextBox ID="txt_expname" runat="server" Height="26px" Placeholder="Charge_to_Name" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>


                                        <fin:CoolGridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1300px" Height="230px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." HeaderStyle-Width="40px" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Amortize_Date</HeaderTemplate>
                                                    <HeaderStyle Width="600px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Amortize_Amount</HeaderTemplate>
                                                    <HeaderStyle Width="500px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab4"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab6"></div>
                            </div>
                        </div>
                    </div>
                </section>
                
                <div class="col-md-6" id="div3" runat="server">
                    <div>
                        <div class="box-body">
                            <label id="lbltxtrmk" runat="server" class="col-sm-12 control-label" title="lbl1">
                                Remarks / Narration (upto 200 char) :</label>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 20px">
                                        <asp:ImageButton ID="btnRmk" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px;" runat="server" OnClick="btnRmk_Click" ToolTip="Select Narration" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtremarks" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks / Narration"></asp:TextBox></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="divWork1" runat="server">
                    <div>
                        <div class="box-body">
                            <asp:Label ID="Label12" runat="server" Text="Person_Name" CssClass="col-sm-2 control-label"></asp:Label>
                            <div class="col-sm-1">
                                <asp:ImageButton ID="btnDelMode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnemp_Click" />
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="tslip_Name" runat="server" Height="26px" Placeholder="Person_Name" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="box-body">
                            <asp:Label ID="Label13" runat="server" Text="Rcpt_Book_No" CssClass="col-sm-3 control-label"></asp:Label>

                            <div class="col-sm-4">
                                <asp:TextBox ID="tslip_no" runat="server" Height="26px" Placeholder="Collection Rcpt No." CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
            <label for="exampleInputEmail1">Total Value :</label>
            <label id="lblqtysum" runat="server" style="display: none">0</label>
        </section>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hf3" runat="server" />
    <asp:HiddenField ID="hfHOPayRcvConcept" runat="server" />
    <asp:HiddenField ID="hfHObr" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="popselected" runat="server" />
    <asp:HiddenField ID="hfBillMSG" runat="server" />
    <div class="col-sm-8" style="display: none">
        <label for="exampleInputEmail1">Type</label>
        <asp:Label ID="lbltypename" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:Button ID="btnGridPop" runat="server" OnClick="btnGridPop_Click" Style="display: none" />
    <asp:Label ID="Label6" runat="server" Text="Other_Amount" CssClass="col-sm-2 control-label" Style="display: none"></asp:Label>
    <div class="col-sm-4">

        <asp:TextBox ID="txtothamt" runat="server"
            Placeholder="Other Amount" Style="text-align: right; display: none" CssClass="form-control" Height="28px" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
    </div>
    <div class="col-sm-6">
        <asp:TextBox ID="txtothname" runat="server" placeholder="Other A/c Name"
            ReadOnly="true" CssClass="form-control" Height="28px" Style="display: none"></asp:TextBox>
    </div>

    <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">-</label>
    <div class="col-sm-1">
        <asp:ImageButton ID="btnlbl7" runat="server" ToolTip="Other Account"
            ImageUrl="~/tej-base/css/images/bdsearch5.png"
            Style="width: 22px; float: right; display: none" OnClick="btnlbl7_Click" />
    </div>
    <div class="col-sm-3">
        <asp:TextBox ID="txtothac" runat="server" placeholder="Code" ReadOnly="true"
            CssClass="form-control" Height="28px" Style="display: none"></asp:TextBox>
    </div>


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
    <asp:FileUpload ID="xmlUpload" runat="server" onchange="submitFile()" Style="display: none" />
    <asp:Button ID="btnImport" runat="server" CssClass="bg-green btn-foursquare" Text="Import Bills" Style="margin-left: 20px; display: none" OnClick="btnImport_Click" />
</asp:Content>
