<%@ Page Language="C#" AutoEventWireup="True" CodeFile="drillDownpr.aspx.cs" Inherits="drillDownpr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tejaxo</title>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <script type="text/javascript" src="../tej-base/pmgrid/jquery.js"></script>
    <link rel="stylesheet" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.9.1/themes/base/jquery-ui.css" />

    <%--<script type="text/javascript" src="../tej-base/pmgrid/jquery-ui.min.js"></script>--%>

    <!--PQ Grid files-->
    <link rel="stylesheet" href="../tej-base/pmgrid/pqgridseek.min.css" />
    <link rel="stylesheet" href="../tej-base/pmgrid/pqgrid.ui.min.css" />
    <link rel="stylesheet" href="../tej-base/pmgrid/pqselect.min.css" />
    <link rel='stylesheet' href='../tej-base/pmgrid/themes/bootstrap/pqgrid.css' />

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>

    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />
    <script src="../tej-base/pmgrid/pqgrid.dev.js" type="text/javascript"></script>
    <script src="../tej-base/pmgrid/pqselect.min.js" type="text/javascript"></script>

    <!--jqueryui touch punch for touch devices-->
    <!--PQ Grid bootstrap theme-->

    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <!--jsZip for zip and xlsx export-->
    <script src="../tej-base/pmgrid/jsZip-2.5.0/jszip.min.js" type="text/javascript"></script>
    <script src="../tej-base/pmgrid/jszip-2.5.0/filesaver.js" type="text/javascript"></script>

    <style type="text/css">
        .pq-grid {
            font-size: 11px;
            font-weight: 600;
        }

        tr.green td {
            background: lightgreen;
            font-weight: 600;
        }
    </style>
    <script type="text/javascript">
        function makeFocus(event) {
            if (event.keyCode == 40) {
                $("#gridDiv").pqGrid("setSelection", { rowIndx: 0, dataIndx: 0 });
            }
        }
        $(document).keyup(function (event) {
            if (event.keyCode == 27) {
                document.getElementById("btnBack").click();
            }
            $('#txtsearch').keypress(function (event) {
                var keycode = (event.keyCode ? event.keyCode : event.which);
                if (keycode == '13') {
                    document.getElementById("srch").click()
                    return;
                }
            });
        });
        function onlyClose() {
            parent.$.colorbox.close();
        }
    </script>
    <script type="text/javascript">


        window.addEventListener("keydown", function (e) {
            // space and arrow keys
            if ([38, 40].indexOf(e.keyCode) > -1) {
                e.preventDefault();
            }
        }, false);

        function listenAndOpen() {
            var output = document.getElementById("txtsearch");
            var output2 = document.getElementById("txtsearch");

            // get action element reference
            var action = document.getElementById("action");
            // new speech recognition object
            var SpeechRecognition = SpeechRecognition || webkitSpeechRecognition;
            var recognition = new SpeechRecognition();

            // This runs when the speech recognition service starts
            recognition.onstart = function () {
                action.innerHTML = "<small>listening, please speak...</small>";
            };

            recognition.onspeechend = function () {
                action.innerHTML = "<small>stopped listening,hope you are done...</small>";
                recognition.stop();
            }
            // This runs when the speech recognition service returns result
            recognition.onresult = function (event) {
                action.innerHTML = "";
                var transcript = event.results[0][0].transcript;
                var confidence = event.results[0][0].confidence;
                output.value = transcript;
                output2.value = transcript;
                document.getElementById("srch").click();
            };
            action.innerHTML = "";
            // start recognition
            recognition.start();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 40px;">
            <div style="margin-left: 10px">
                <div class="row">
                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="col-sm-12" style="padding-bottom: 5px;">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 320px">
                                            <table>
                                                <tr>
                                                    <td style="width: 30px">
                                                        <img id="imgMicrophone" runat="server" alt="Speak" src="~/tej-base/images/microphone_b.png" title="Speak here" style="cursor: pointer; float: left; width: 25px" onclick="listenAndOpen()" accesskey="t" />
                                                        <span id="action" style="color: black; position: fixed; top: 35px;"></span>
                                                    </td>

                                                    <td style="padding-right: 5px">
                                                        <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" AutoPostBack="false"
                                                            AutoCompleteType="Disabled" EnableViewState="false" onkeyup="makeFocus(event)"
                                                            placeholder="Enter here to search" ToolTip="Enter here to search"></asp:TextBox></td>
                                                    <td style="padding-right: 5px">
                                                        <asp:ImageButton ID="srch" runat="server" ImageUrl="~/tej-base/images/search-button.png"
                                                            Width="100px" Height="27px" ToolTip="Click to Search" OnClick="srch_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td id="divChkview" runat="server" style="width: 100px">
                                            <asp:CheckBox ID="chkVchView" runat="server" Text="Voucher View" Checked="true" />
                                        </td>
                                        <td style="min-width: 200px">
                                            <label id="lblMsg" runat="server" style="font-family: Verdana; font-size: smaller; color: #000; position: fixed; top: 35px; background-color: lightgoldenrodyellow"></label>
                                            <label id="lbl1" runat="server" class="control-label" title="lbl1" style="font-family: Calibri; font-size: 11px; color: #000;">
                                                Showing For
                                            </label>
                                            <label id="lblMsgSel" runat="server" class="control-label" style="font-family: Calibri; font-size: 12px; color: #000;"></label>
                                        </td>
                                        <td style="width: 150px; text-align: right">
                                            <table style="text-align: right">
                                                <tr>
                                                    <td>
                                                        <label id="lblTotcount" runat="server" style="font-size: 10px; font-family: Verdana; font-weight: 600; background-color: #b5e6cc; position: fixed; top: 35px; display: none"></label>
                                                        <label id="Label1" runat="server" style="font-family: Verdana; font-size: smaller; color: #999999;">Show Rows</label>
                                                        <asp:TextBox ID="tkrow" runat="server" Width="40px" CssClass="txtcss2" onkeypress="return isDecimalKey(event);"
                                                            Style="text-align: right;" Height="10px" Text="10000"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding-left: 5px; width: 40px">
                                            <asp:ImageButton ID="btnBack" runat="server" ImageUrl="~/tej-base/images/Previous.JPG" ToolTip="Back" Width="38px" OnClick="btnBack_Click" />
                                            <asp:ImageButton ID="btnreFill" runat="server" ImageUrl="~/tej-base/images/Previous.JPG" ToolTip="Back" Width="38px" OnClick="btnreFill_Click" Style="display: none" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>



                    <div class="col-md-12">
                        <div class="form-group">
                            <div class="lbBody" id="gridDiv" style="color: White; margin-right: 15px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                            </div>
                        </div>
                    </div>
                </div>

                <table width="100%">
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnexptoexl" runat="server" ImageUrl="~/tej-base/images/excel_icon.png"
                                ToolTip="Export to Excel" Width="30px" Height="30px" OnClick="btnexptoexl_Click" />
                            <asp:ImageButton ID="btnexptocsv" runat="server" ImageUrl="~/tej-base/images/csv_icon.png"
                                ToolTip="Export to CSV" Width="30px" Height="30px" OnClick="btnexptocsv_Click" Visible="false" />
                            <asp:ImageButton ID="btnexptopdf" runat="server" ImageUrl="~/tej-base/images/pdf_icon.png"
                                ToolTip="Export to PDF" Width="30px" Height="30px" OnClick="btnexptopdf_Click" Visible="false" />
                            <asp:ImageButton ID="btnexptoword" runat="server"
                                ImageUrl="~/tej-base/images/Word-2-icon.png" ToolTip="Export to Word"
                                Style="margin-top: 0" Width="30px" Height="28px" OnClick="btnexptoword_Click" Visible="false" />
                            <div id="divArab" runat="server">
                                Export to Arabic Format :  
                            <asp:ImageButton ID="btnExcel2" runat="server" ImageUrl="~/tej-base/images/excel_icon.png"
                                ToolTip="Export to Excel" Width="30px" Height="30px" OnClick="btnExcel2_Click" />
                            </div>
                            <asp:Label ID="lblGPNP" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <div id="divPL" runat="server">
                                <asp:Button ID="btnRep1" runat="server" CssClass="bg-blue btn-foursquare" Text="P and L Printout - Schedule level" OnClick="btnRep1_Click" />
                                <asp:Button ID="btnRep2" runat="server" CssClass="bg-blue btn-foursquare" Text="P and L Printout - Ledger Level" OnClick="btnRep2_Click" />
                                <asp:Button ID="btnRep3" runat="server" CssClass="bg-blue btn-foursquare" Text="P and L Trend - Quarterly" OnClick="btnRep3_Click" />
                                <asp:Button ID="btnRep4" runat="server" CssClass="bg-blue btn-foursquare" Text="P and L Trend - Monthly" OnClick="btnRep4_Click" />
                            </div>
                            <div id="divBS" runat="server">
                                <asp:Button ID="btnRep5" runat="server" CssClass="bg-blue btn-foursquare" Text="Balance Sheet Summary" OnClick="btnRep5_Click" />
                                <asp:Button ID="btnRep6" runat="server" CssClass="bg-blue btn-foursquare" Text="Balance Sheet Detail" OnClick="btnRep6_Click" />
                                <asp:Button ID="btnRep7" runat="server" CssClass="bg-blue btn-foursquare" Text="Format 2" OnClick="btnRep7_Click" />

                            </div>
                            <div id="trDetail" runat="server">
                                <asp:Button ID="Button1" runat="server" CssClass="bg-blue btn-foursquare" Text="View (Print Preview)" OnClick="btnRep12_Click" />
                                <asp:Button ID="Button2" runat="server" CssClass="bg-blue btn-foursquare" Text="View O/S Bill Wise" OnClick="btnRep22_Click" />
                                <asp:Button ID="Button3" runat="server" CssClass="bg-blue btn-foursquare" Text="A/c Wise Summary" OnClick="btnRep32_Click" Visible="false" />
                                <asp:Button ID="Button4" runat="server" CssClass="bg-blue btn-foursquare" Text="MRR Report" OnClick="btnRep42_Click" />
                            </div>

                        </td>
                        <td style="padding-left: 20px">
                            <span id="txtSpan" runat="server"></span>
                        </td>
                        <td style="float: right; vertical-align: top" id="finOpt" runat="server">
                            <asp:Label ID="lblOpBal" runat="server" Text="" Style="font-family: 'Century Gothic'; font-weight: 500; font-size: 15px; background-color: #b1f66f; padding: 5px"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblTotDr" runat="server" Text="" Style="font-family: 'Century Gothic'; font-weight: 500; font-size: 15px; background-color: #b1f66f; padding: 5px"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblTotCr" runat="server" Text="" Style="font-family: 'Century Gothic'; font-weight: 500; font-size: 15px; background-color: #b1f66f; padding: 5px"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblTotBal" runat="server" Text="" Style="font-family: 'Century Gothic'; font-weight: 500; font-size: 15px; background-color: #b1f66f; padding: 5px"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hfqry" runat="server" />
        <asp:HiddenField ID="hfLevel" runat="server" />
        <asp:HiddenField ID="hdata" runat="server" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="hfOpening" runat="server" />
        <asp:HiddenField ID="hfACode" runat="server" />
        <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:HiddenField ID="hfvalSelected" runat="server" />
        <asp:Button ID="btnGrid" runat="server" OnClick="btnGrid_Click" Style="display: none" />
        <asp:Button ID="btnhide11" runat="server" OnClick="btnBack_Click" Style="display: none" />
        <%--<div style="display: none">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
                AutoDataBind="true" OnUnload="CrystalReportViewer1_Unload" HasCrystalLogo="False"
                Height="50px" Width="350px" Style="margin-left: 30px;" EnableDrillDown="false" />
        </div>--%>
    </form>
</body>
</html>
