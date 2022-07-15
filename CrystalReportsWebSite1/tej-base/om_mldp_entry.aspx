<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_mldp_entry" CodeFile="om_mldp_entry.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            calculateSum();
        });
        function calculateSum($element) {
            var shotsperhr = 0; var wrkperhr = 0; var runcavity = 0; var actualShots = 0;
            var totProd = 0; var totRej = 0; var actCavity = 0; var scrap = 0;
            var dTime = 0; var shotPerMin = 0; var timeIn = 0; var timeOut = 0;
            var targetShot = 0; var actualShot = 0; var txtOkProd = 0;
            var doubleVal1 = 0; var doubleVal2 = 0; var txtNetWt = 0; var txtRRPerPcs = 0;
            shotsperhr = fill_zero(document.getElementById('ContentPlaceHolder1_txtShotHrs').value);
            runcavity = fill_zero(document.getElementById('ContentPlaceHolder1_txtRunCavity').value);
            actCavity = fill_zero(document.getElementById('ContentPlaceHolder1_txtActualCavity').value);
            actualShots = fill_zero(document.getElementById('ContentPlaceHolder1_txtactshot').value);
            timeIn = (document.getElementById('ContentPlaceHolder1_txtTimeIn').value);
            timeOut = (document.getElementById('ContentPlaceHolder1_txtTimeOut').value);

            //***************************
            if (timeIn.toString().length > 1) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = timeIn + ":";
            if (timeOut.toString().length > 1) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = timeOut + ":";
            if (timeIn.includes(".")) {
                var timeIn1 = timeIn.split(".");
                var t2 = timeIn1[1];
                if ((t2 * 1) > 60) t2 = 59;
                document.getElementById('ContentPlaceHolder1_txtTimeIn').value = timeIn1[0] + ":" + t2;
                if (timeIn1[0].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = "";
                if (timeIn1[1].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = "";
                if ((timeIn1[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = "00" + ":" + timeIn1[1];
                if ((timeIn1[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = timeIn1[0] + ":" + "00";
                timeIn = timeIn1[0] * 60 + (t2 * 1);
            }
            else if (timeIn.includes(":")) {
                var timeIn1 = timeIn.split(":");
                var t2 = timeIn1[1];
                if ((t2 * 1) > 60) t2 = 59;
                document.getElementById('ContentPlaceHolder1_txtTimeIn').value = timeIn1[0] + ":" + t2;
                if (timeIn1[0].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = "";
                if (timeIn1[1].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = "";
                if ((timeIn1[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = "00" + ":" + timeIn1[1];
                if ((timeIn1[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_txtTimeIn').value = timeIn1[0] + ":" + "00";
                timeIn = timeIn1[0] * 60 + (t2 * 1);
            }
            else {
                timeIn = fill_zero(timeIn);
                if ((timeIn * 1) > 23) {
                    document.getElementById('ContentPlaceHolder1_txtTimeIn').value = 0;
                    timeIn = 0;
                }
                timeIn = timeIn * 60;
            }
            //***************************                                    
            if (timeOut.includes(".")) {
                var timeOut1 = timeOut.split(".");
                var to2 = timeOut1[1];
                if ((to2 * 1) > 60) to2 = 59;
                document.getElementById('ContentPlaceHolder1_txtTimeOut').value = timeOut1[0] + ":" + to2;
                if (timeOut1[0].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = "";
                if (timeOut1[1].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = "";
                if ((timeOut1[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = "00" + ":" + timeOut1[1];
                if ((timeOut1[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = timeOut1[0] + ":" + "00";
                timeOut = timeOut1[0] * 60 + (to2 * 1);
                if (((timeIn * 1) > (12 * 60)) && ((timeOut1[0] * 1) < 24 && (timeOut1[0] * 1) > 12)) {
                    timeOut = 0;
                    timeOut = (timeOut1[0] * 60) + (to2 * 1);
                }
                else if ((timeIn * 1) > (12 * 60)) {
                    timeOut = 0;
                    timeOut = (timeOut1[0] * 60) + (24 * 60) + (to2 * 1);
                }
            }
            else if (timeOut.includes(":")) {
                var timeOut1 = timeOut.split(":");
                var to2 = timeOut1[1];
                if ((to2 * 1) > 60) to2 = 59;
                document.getElementById('ContentPlaceHolder1_txtTimeOut').value = timeOut1[0] + ":" + to2;
                if (timeOut1[0].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = "";
                if (timeOut1[1].length > 2) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = "";
                if ((timeOut1[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = "00" + ":" + timeOut1[1];
                if ((timeOut1[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_txtTimeOut').value = timeOut1[0] + ":" + "00";
                timeOut = timeOut1[0] * 60 + (to2 * 1);
                if (((timeIn * 1) > (12 * 60)) && ((timeOut1[0] * 1) < 24 && (timeOut1[0] * 1) > 12)) {
                    timeOut = 0;
                    timeOut = (timeOut1[0] * 60) + (to2 * 1);
                }
                else if ((timeIn * 1) > (12 * 60)) {
                    timeOut = 0;
                    timeOut = (timeOut1[0] * 60) + (24 * 60) + (to2 * 1);
                }
            }
            else {
                timeOut = fill_zero(timeOut);
                if ((timeOut * 1) > 23) {
                    document.getElementById('ContentPlaceHolder1_txtTimeOut').value = 0;
                    timeOut = 0;
                }
                else if ((timeIn * 1) > (12 * 60) && timeOut > 0 && timeOut < 13) {
                    timeOut = (timeOut * 60) + (24 * 60);
                }
                else {
                    timeOut = (timeOut * 60);
                }
            }
            //***************************
            var n = (((timeOut - timeIn)).toFixed());
            var min = n % 60;
            var hour = (n - min) / 60;
            if (hour.toString().length < 2) hour = "0" + hour;
            if (min.toString().length < 2) min = "0" + min;
            document.getElementById('ContentPlaceHolder1_txtWorkingHrs').value = hour + "." + min;

            wrkperhr = fill_zero(n) / 60;
            var grid = document.getElementById("<%= SG3.ClientID%>");
            scrap = 0;
            if (grid != null) {
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    //var txtAmountReceive = $("input[id*=tkSCRP1]");
                    if (i >= 0) {
                        scrap += fill_zero(document.getElementById('ContentPlaceHolder1_SG3_tkSCRP1_' + i).value);
                    }
                    //scrap += (txtAmountReceive[i].value * 1);
                }
            }
            if (fill_zero((runcavity) * 1) > fill_zero((actCavity) * 1)) {
                document.getElementById('ContentPlaceHolder1_txtRunCavity').value = actCavity;
                alert("Running Cavity Can not be greater then Actual Cavity");
                return;
            }

            shotPerMin = fill_zero((60 / shotsperhr).toFixed(2));

            var grid2 = document.getElementById("<%= sg2.ClientID%>");
            var totDMin = 0;
            if (grid2 != null) {
                for (var i = 0; i < grid2.rows.length - 1; i++) {
                    totDMin += fill_zero(document.getElementById('ContentPlaceHolder1_sg2_tkObsv1_' + i).value);
                }
            }

            //document.getElementById('ContentPlaceHolder1_txtTargetShot').value = (fill_zero((shotsperhr * wrkperhr).toFixed()) - (totDMin * shotPerMin).toFixed());
            document.getElementById('ContentPlaceHolder1_txtTargetShot').value = (fill_zero((shotsperhr * wrkperhr).toFixed()) - 0);


            targetShot = fill_zero(document.getElementById('ContentPlaceHolder1_txtTargetShot').value);
            actualShot = fill_zero(document.getElementById('ContentPlaceHolder1_txtactshot').value);

            if ((wrkperhr * 1) > 0) {
                if ((fill_zero(actualShot) * 1) > fill_zero((targetShot) * 1) + fill_zero((targetShot) * 0.05)) {
                    document.getElementById('ContentPlaceHolder1_txtactshot').value = targetShot;
                    alert("Actual Shots Can not be greater then Target Shot. (5% Tollrance is allowed only)");
                    return;
                }
            }
            if (targetShot > 0) {
                document.getElementById('ContentPlaceHolder1_txtTotProd').value = fill_zero((runcavity * actualShots).toFixed());
            }
            totProd = fill_zero(document.getElementById('ContentPlaceHolder1_txtTotProd').value);
            document.getElementById('ContentPlaceHolder1_txtTotRej').value = fill_zero(scrap).toFixed();
            totRej = fill_zero(document.getElementById('ContentPlaceHolder1_txtTotRej').value);
            document.getElementById('ContentPlaceHolder1_txtOkProd').value = fill_zero((totProd - totRej).toFixed());
            document.getElementById('ContentPlaceHolder1_txtDtime').value = fill_zero((((shotsperhr * wrkperhr).toFixed(2) - actualShots) * shotPerMin).toFixed());
            txtOkProd = document.getElementById('ContentPlaceHolder1_txtOkProd').value;
            txtNetWt = document.getElementById('ContentPlaceHolder1_txtNetWt').value;
            txtRRPerPcs = document.getElementById('ContentPlaceHolder1_txtRRPerPcs').value;
            doubleVal1 = fill_zero((totProd * txtNetWt)).toFixed(6);
            doubleVal2 = fill_zero(txtRRPerPcs * actCavity * actualShot);
            document.getElementById('ContentPlaceHolder1_lblTotReqWt').innerHTML = fill_zero((doubleVal1 * 1) + (doubleVal2 * 1)).toFixed(4);

            doubleVal1 = 0;
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length - 1;
            for (var i = 0; i < gridCount; i++) {
                var txtAmountReceive1 = $("input[id*=sg1_t1]");
                doubleVal1 += fill_zero(txtAmountReceive1[i].value * 1);
            }
            document.getElementById('ContentPlaceHolder1_lblTotInputWt').innerHTML = fill_zero(doubleVal1 * 1).toFixed(4);
            document.getElementById('ContentPlaceHolder1_lblDiff').innerHTML = (fill_zero(document.getElementById('ContentPlaceHolder1_lblTotReqWt').innerHTML * 1) - fill_zero(document.getElementById('ContentPlaceHolder1_lblTotInputWt').innerHTML * 1)).toFixed(4);

            for (var i = 0; i < gridCount; i++) {
                txtAmountReceive1 = $("input[id*=sg1_t1]");
                if (fill_zero(txtAmountReceive1[i].value * 1) > 0) {
                    doubleVal2 = (fill_zero(txtAmountReceive1[i].value * 1) / doubleVal1 * 100).toFixed(2);
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t4_' + i).value = doubleVal2;
                }
            }

        }
        function fill_zero(val) {
            try {
                val = val.replace(/"/g, '');
            }
            catch (err) { }
            try {
                val = val * 1;
            }
            catch (err) { val = 0; }
            if (isNaN(val)) return 0;
            if (isFinite(val)) return val;
        }
        function zeroActualShots() {
            document.getElementById('ContentPlaceHolder1_txtactshot').value = 0;
            calculateSum();
        }
        function onlyNumber(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 47 && charCode < 58)
                return true;
            else {
                return false;
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }
        .auto-style2 {
            font-weight: bold;
        }
        .auto-style3 {
            height: 38px;
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
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
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
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td class="auto-style1">
                                        <asp:Label ID="lblEntry" runat="server" Text="Entry No." CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td colspan="5" class="auto-style1">
                                        <asp:TextBox ID="txtvchnum" runat="server" Width="20%" ReadOnly="true" placeholder="Entry No"></asp:TextBox>
                                        <asp:TextBox ID="txtvchdate" runat="server" Width="20%" placeholder="Entry Dt"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMachine" runat="server" Text="Machine Name" CssClass="auto-style2"></asp:Label>
                                        <asp:ImageButton ID="btnmachine" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                            ToolTip="Select Machine" Style="width: 22px; float: right;" OnClick="btnmachine_Click" />
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtmchcode" runat="server" ReadOnly="true" Width="20%" placeholder="Code"></asp:TextBox>
                                        <asp:TextBox ID="txtmchname" runat="server" ReadOnly="true" Width="70%" placeholder="Machine Name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style3">
                                        <asp:Label ID="lblShift" runat="server" Text="Shift" CssClass="auto-style2"></asp:Label>
                                        <asp:ImageButton ID="btnshift" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                            ToolTip="Select Shift" Style="width: 22px; float: right;" OnClick="btnshift_Click" />
                                    </td>
                                    <td colspan="5" class="auto-style3">
                                        <asp:TextBox ID="txtshiftcode" runat="server" ReadOnly="true" Width="20%" placeholder="Code"></asp:TextBox>
                                        <asp:TextBox ID="txtshiftname" runat="server" ReadOnly="true" Width="70%" placeholder="Shift Name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStage" runat="server" CssClass="auto-style2">Stage</asp:Label>
                                        <asp:ImageButton ID="btnstage" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                            ToolTip="Select Stage" Style="width: 22px; float: right;" OnClick="btnstage_Click" />
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtstage" runat="server" Width="20%" ReadOnly="true" placeholder="Code"></asp:TextBox>
                                        <asp:TextBox ID="txtstagename" runat="server" Width="70%" placeholder="Stage Name"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProduct" runat="server" Text="Product" CssClass="auto-style2"></asp:Label>
                                        <asp:ImageButton ID="btnicode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                            ToolTip="Select Product" Style="width: 22px; float: right;" OnClick="btnicode_Click" />
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txticode" runat="server" Width="20%" ReadOnly="true" placeholder="Code"></asp:TextBox>
                                        <asp:TextBox ID="txtiname" runat="server" Width="70%" placeholder="Product Name" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblMould" runat="server" Text="Mould" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td colspan="5">
                                        <asp:TextBox ID="txtmicode" runat="server" Width="20%" ReadOnly="true" placeholder="Code"></asp:TextBox>
                                        <asp:TextBox ID="txtminame" runat="server" Width="70%" placeholder="Mould Name" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTime" runat="server" Text="Time In/Out (24 hr format)" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTimeIn" runat="server" Width="86px" placeholder="Time In" onkeyup="zeroActualShots();" onkeypress="return onlyNumber(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="5"></asp:TextBox>
                                        <asp:TextBox ID="txtTimeOut" runat="server" Width="80px" placeholder="Time Out" onkeyup="zeroActualShots();" onkeypress="return onlyNumber(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="5"></asp:TextBox>
                                    </td>
                                    <td><b>Net Wt</b></td>
                                    <td>
                                        <asp:TextBox ID="txtNetWt" runat="server" placeholder="Net Wt" Width="70px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td><b>RR Wt/Pc</b></td>
                                    <td>
                                        <asp:TextBox ID="txtRRPerPcs" runat="server" placeholder="RR Wt/Pc" Width="70px" ReadOnly="true"></asp:TextBox>
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
                                        <asp:Label ID="lblOpe" runat="server" Text="Operator" CssClass="auto-style2"></asp:Label>
                                        <asp:ImageButton ID="btnoperator" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                            ToolTip="Select Operator" Style="width: 22px; float: right;" OnClick="btnoperator_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtoprtr" runat="server" Width="50%" placeholder="Operator"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblBatch" runat="server" Text="Batch No" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtbatchno" runat="server" Width="50%" placeholder="Batchno"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblSpeed" runat="server" Text="Speed/Min" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtActualCavity" runat="server" Width="50%" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblActual" runat="server" Text="Actual Speed" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRunCavity" runat="server" Width="50%" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblShots" runat="server" Text="Shots/Hrs" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShotHrs" runat="server" Width="50%" placeholder="Shots/Hrs" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblHrs" runat="server" Text="Working Hrs" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWorkingHrs" runat="server" Width="50%" placeholder="Working Hrs" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Label ID="lblTarget" runat="server" Text="lblTarget" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTargetShot" runat="server" Width="50%" placeholder="Shots" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label id="lblActPcs" runat="server" Text="Actual Pcs" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtactshot" runat="server" Width="50%" placeholder="Acutal Shots" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" onkeyup="calculateSum();"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTot" runat="server" Text="Total Production" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotProd" runat="server" Width="50%" placeholder="Tot Prod" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRej" runat="server" Text="Total Rejection" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotRej" runat="server" Width="50%" placeholder="Tot Rej" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOk" runat="server" Text="Ok Production" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOkProd" runat="server" Width="50%" placeholder="Ok Prod" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl" runat="server" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:TextBox ID="TextBox4" runat="server" Width="50%" placeholder="Working Hrs" ></asp:TextBox>--%>
                                        <button id="btnConsume" runat="server" onserverclick="btnConsume_Click">Bom Consume</button>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lblLumps" runat="server" Text="Total Lumps" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLump" runat="server" Width="50%" placeholder="Lumps" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label id="lblSample" runat="server" Text="Sample Qty" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSampQty" runat="server" Width="50%" placeholder="Sample Qty" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr id="Lunch_Excess" runat="server">
                                    <td>
                                        <asp:Label ID="lblLunch" runat="server" Text="Lunch/Tea" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLunch" runat="server" Width="50%" placeholder="Lunch/Tea in Min" onkeypress="return isDecimalKey(event)" MaxLength="6" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                    <td style="display:none">
                                        <asp:Label ID="lblExcess" runat="server" Text="Excess/Short" CssClass="auto-style2"></asp:Label>
                                    </td>
                                    <td style="display:none">
                                        <asp:TextBox ID="txtExcess" runat="server" Width="50%" placeholder="Excess/Short" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <section class="col-lg-12 connectedSortable">
                    <div class="nav-tabs-custom">
                        <ul class="nav nav-tabs pull-left">
                            <li class="active"><a href="#DescTab" data-toggle="tab">Description</a></li>
                            <li><a href="#RejectionTab" data-toggle="tab">Rejection Reason</a></li>
                            <li><a href="#DownTab" data-toggle="tab">Down Time Reason</a></li>
                            <li style="display: none">
                                <a href="#">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Total Req Wt.
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <b>
                                       <asp:Label ID="lblTotReqWt" runat="server"></asp:Label></b>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Total Input Wt.                                    
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <b>
                                       <asp:Label ID="lblTotInputWt" runat="server"></asp:Label></b>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    Diff
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <b>
                                       <asp:Label ID="lblDiff" runat="server"></asp:Label></b>
                                </a>
                            </li>
                        </ul>
                        <div class="tab-content no-padding">
                            <div class="chart tab-pane active" id="DescTab" style="position: relative; height: 160px;">
                                <div class="lbBody" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                    <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Height="150px" Font-Size="Smaller"
                                        AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                        OnRowCommand="sg1_RowCommand">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>A</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="sg1_Row_Add" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>D</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="sg1_Rmv" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="sg1_srno" HeaderText="Srno" />
                                            <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                            <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                            <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                            <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                            <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)" onkeyup="calculateSum();" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>'></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                            CssClass="GridviewScrollHeader2" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </fin:CoolGridView>
                                </div>
                            </div>
                            <div class="chart tab-pane" id="RejectionTab" style="position: relative; height: 160px;">
                                <div class="lbBody" style="color: White; max-height: 160px; max-width: 1310px; overflow: scroll; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="SG3" Width="100%" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="False"
                                        Font-Size="Smaller">
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="h1_sg3" HeaderText="Code" ReadOnly="True" />
                                            <asp:BoundField DataField="h2_sg3" HeaderText="Name" ReadOnly="True" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>Scrap</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tkSCRP1" runat="server" Width="80px" Text='<%#Eval("SCRP1") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" onkeyup="calculateSum();" Style="text-align: right" MaxLength="10"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                            CssClass="GridviewScrollHeader" Font-Size="Small" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="chart tab-pane" id="DownTab" style="position: relative; height: 160px;">
                                <div class="lbBody" style="color: White; max-height: 200px; max-width: 1310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <asp:GridView ID="sg2" Width="100%" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="False"
                                        Font-Size="Smaller">
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="h1_sg2" HeaderText="Code" ReadOnly="True" />
                                            <asp:BoundField DataField="h2_sg2" HeaderText="Name" ReadOnly="True" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>Minutes</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="tkObsv1" runat="server" Width="250px" Text='<%#Eval("Obsv1") %>' onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="3"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                            CssClass="GridviewScrollHeader" Font-Size="Small" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
                            <asp:TextBox ID="txtDtime" runat="server" Style="display: none"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
</asp:Content>
