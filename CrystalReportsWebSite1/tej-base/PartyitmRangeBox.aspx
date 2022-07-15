<%@ Page Language="C#" AutoEventWireup="true" Inherits="PartyitmRangeBox" CodeFile="PartyitmRangeBox.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />
    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />
    <link rel="stylesheet" type="text/css" href="../tej-base/Styles/vip_vrm.css" />

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>

    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server" style="margin-top: 40px;">
        <section class="content">
            <div class="row">
                <div class="col-md-6" id="partyBox" runat="server">
                    <div>
                        <div class="box-header with-border">
                            <h2 class="box-title" id="H1" runat="server">Party Group/Sub Group/Party Code Selection</h2>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Party Group</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnPmcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnPmcode_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtPMcode" type="text" class="form-control" runat="server" placeholder="Main Group" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl2">Sub Group</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnPsubCode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnPsubCode_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtPSubCode" type="text" class="form-control" runat="server" placeholder="Sub Group Group" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl3">Start From</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnAcode1_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtAcode1" type="text" class="form-control" runat="server" placeholder="Start From" style="height: 28px" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtAname1" type="text" class="form-control" runat="server" placeholder="Start From" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl3">Ending On</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnAcode2_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtAcode2" type="text" class="form-control clearable" runat="server" placeholder="Ending On" style="height: 28px" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtAname2" type="text" class="form-control clearable" runat="server" placeholder="Ending On" style="height: 28px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="itemBox" runat="server">
                    <div>
                        <div class="box-header with-border">
                            <h3 class="box-title" id="lblheader" runat="server">Item Group/Sub Group/Item Code Selection</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="lbl1" runat="server" class="col-sm-3 control-label" title="lbl1">Item Group</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnMcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnMcode_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtMcode" type="text" class="form-control clearable" runat="server" placeholder="Main Group" style="height: 28px" readonly="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl2" runat="server" class="col-sm-3 control-label" title="lbl2">Sub Group</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnSubCode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnSubCode_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtSubCode" type="text" class="form-control clearable" runat="server" placeholder="Sub Group Group" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl3" runat="server" class="col-sm-3 control-label" title="lbl3">Start From</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnIcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnIcode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtIcode1" type="text" class="form-control clearable" runat="server" placeholder="Start From" style="height: 28px" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtIname1" type="text" class="form-control clearable" runat="server" placeholder="Start From" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl4" runat="server" class="col-sm-3 control-label" title="lbl3">Ending On</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnIcode2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnIcode2_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtIcode2" type="text" class="form-control clearable" runat="server" placeholder="Ending On" style="height: 28px" />
                                </div>
                                <div class="col-sm-5">
                                    <input id="txtIname2" type="text" class="form-control clearable" runat="server" placeholder="Ending On" style="height: 28px" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="branchWiseRdTool" runat="server">
                    <div class="box box-success" style="text-align: center">
                        <div class="box-header with-border">
                            <h4 class="box-title" id="H3" runat="server">RM Price Pick From Same Plant or Across All Plant</h4>
                        </div>
                        <div style="padding-left: 35%">
                            <asp:RadioButtonList ID="rdBranch" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="&nbsp;&nbsp;Same Plant &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="&nbsp;&nbsp;Across All Plant" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="col-md-12" id="wtAvgDiv" runat="server">
                    <div class="box box-success" style="text-align: center">
                        <div class="box-header with-border">
                            <h4 class="box-title">Select your option for Latest Price or Wt Avg Rate</h4>
                        </div>
                        <div style="padding-left: 35%">
                            <asp:RadioButtonList ID="rdRate" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="&nbsp;&nbsp;Last Price &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="&nbsp;&nbsp;Wt Avg Rate" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>
                <div class="col-md-12" id="Div1" runat="server">
                    <div class="box box-success" style="text-align: center">
                        <div class="box-header with-border">
                            <h4 class="box-title">Select Gross Wt or Net Wt</h4> &nbsp;&nbsp; <asp:CheckBox ID="chk1" runat="server" Text="    Show all the Stock (FG + WIP)" />
                        </div>
                        <div style="padding-left: 35%">
                            <asp:RadioButtonList ID="RadioButtonList3" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="&nbsp;&nbsp;Gross Weight&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="0" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="&nbsp;&nbsp;Net Weight" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </div>                
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div>
                        <div class="box-header with-border">
                            <h3 class="box-title" id="H2" runat="server">Date Range Selection</h3>
                        </div>
                        <div class="box-body">
                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table style="margin-left: 20px;">
                                        <tr style="vertical-align: top">
                                            <td>
                                                <table class="nav-justified" style="vertical-align: top">
                                                    <tr style="vertical-align: top">
                                                        <td>
                                                            <span class="font_css" style="font-size: medium">Date From &nbsp;&nbsp;</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtfromdt" runat="server" MaxLength="12" TextMode="Date" TabIndex="1"></asp:TextBox>
                                                            &nbsp;&nbsp;
                                                        </td>

                                                    </tr>
                                                    <tr style="vertical-align: top">
                                                        <td>
                                                            <span class="font_css" style="font-size: medium">Date To &nbsp;</span>
                                                            <td>
                                                                <asp:TextBox ID="txttodt" runat="server" MaxLength="12" TextMode="Date" TabIndex="2"></asp:TextBox></td>
                                                    </tr>
                                                    <tr style="vertical-align: bottom">
                                                        <td colspan="2">
                                                            <br />
                                                            <br />
                                                            <br />
                                                            <asp:RadioButtonList ID="rdPDF" runat="server" CssClass="font_css" RepeatDirection="Horizontal">
                                                                <asp:ListItem Text="PDF View &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Selected="True" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="Direct View" Value="1"></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top">
                                                <table>
                                                    <tr style="vertical-align: top">
                                                        <td rowspan="2">
                                                            <asp:RadioButtonList ID="RadioButtonList1" runat="server" CssClass="font_css" TabIndex="3"
                                                                AutoPostBack="True" BackColor="#BDEDFF" Width="160px"
                                                                OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                                                                <asp:ListItem>Y.T.D.(Year To Date)</asp:ListItem>
                                                                <asp:ListItem>M.T.D.(Month To Date)</asp:ListItem>
                                                                <asp:ListItem>Previous Month</asp:ListItem>
                                                                <asp:ListItem>Next Month</asp:ListItem>
                                                                <asp:ListItem>Yesterday</asp:ListItem>
                                                                <asp:ListItem>Today</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                        <td rowspan="2">
                                                            <asp:RadioButtonList ID="RadioButtonList2" runat="server" CssClass="font_css" Style="height: 162px;"
                                                                AutoPostBack="True" BackColor="#BDEDFF" Width="160px"
                                                                OnSelectedIndexChanged="RadioButtonList2_SelectedIndexChanged">
                                                                <asp:ListItem>Current Month</asp:ListItem>
                                                                <asp:ListItem>First Qtr</asp:ListItem>
                                                                <asp:ListItem>Second Qtr</asp:ListItem>
                                                                <asp:ListItem>Third Qtr</asp:ListItem>
                                                                <asp:ListItem>Fourth Qtr</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <hr />
                <div class="col-md-2">
                </div>
                <div class="col-md-12">
                    <div style="text-align: center">
                        <button id="btnsubmit" onserverclick="btnsubmit_ServerClick" runat="server" class="btn btn-info" accesskey="S" style="width: 100px"><u>S</u>ubmit</button>
                        <button id="btnexit" onserverclick="btnexit_ServerClick" runat="server" class="btn btn-default" accesskey="x" style="width: 100px">E<u>x</u>it</button>
                        <asp:Label ID="lblerr" runat="server" CssClass="font_css"></asp:Label>
                    </div>
                </div>
            </div>
        </section>
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:Button ID="btniBox" runat="server" OnClick="btniBox_Click" Style="display: none" />

        <script type="text/javascript">
            function closePopup1() { $("#ContentPlaceHolder1_btnhideF_s", window.parent.document).trigger("click"), parent.$.colorbox.close() }
            function closePopup2() { $("#ContentPlaceHolder1_btnhideF", window.parent.document).trigger("click"), parent.$.colorbox.close() }
            function onlyclose() { parent.$.colorbox.close() }
            $(document).ready(function () { $("input").not($(":image")).keypress(function (e) { if (13 == e.keyCode && (iname = $(this).val(), "Submit" !== iname)) { var t = $(this).parents("form:eq(0),body").find("button,input,textarea,select,image"), n = t.index(this); return n > -1 && n + 1 < t.length && (t.eq(n + 1).focus(), t.eq(n + 1).select()), !1 } }) });
        </script>
    </form>
</body>
</html>
