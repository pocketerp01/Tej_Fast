<%@ Page Language="C#" AutoEventWireup="true" Inherits="RangeBox" CodeFile="RangeBox.aspx.cs" %>

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

    <script type="text/javascript">
        function closePopup1() { $("#ContentPlaceHolder1_btnhideF", window.parent.document).trigger("click"), parent.$.colorbox.close() }
        function closePopup2() { $("#ContentPlaceHolder1_btnhideF_s", window.parent.document).trigger("click"), parent.$.colorbox.close() }
        function onlyclose() { parent.$.colorbox.close() }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="margin-top: 40px;">
        <section class="content">
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title" id="lblheader" runat="server">Range Selection</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="lbl1" runat="server" class="col-sm-3 control-label" title="lbl1">Range From</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnFrom" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnFrom_Click" />
                                </div>
                                <div class="col-sm-4">
                                    <input id="txtFromVch" type="text" class="form-control clearable" runat="server" placeholder="Entry No." style="height: 28px" />
                                </div>
                                <div class="col-sm-4">
                                    <input id="txtFromVchdt" type="text" class="form-control clearable" runat="server" placeholder="Entry Date" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Range to</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnTo" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnTo_Click" />
                                </div>
                                <div class="col-sm-4">
                                    <input id="txtToVch" type="text" class="form-control clearable" runat="server" placeholder="Entry No." style="height: 28px" />
                                </div>
                                <div class="col-sm-4">
                                    <input id="txtToVchdt" type="text" class="form-control clearable" runat="server" placeholder="Entry Date" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">Party Code</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnAcode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtAcode" type="text" class="form-control clearable" runat="server" placeholder="Code" style="height: 28px" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtAname" type="text" class="form-control clearable" runat="server" placeholder="Name" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <asp:CheckBox ID="chkOrig" runat="server" Text="Orignal Copy" />
                                &nbsp;&nbsp;
                                <asp:CheckBox ID="chkDup" runat="server" Text="Duplicate Copy" />
                            </div>
                            <div class="form-group">
                                <asp:RadioButtonList ID="rdPDF" runat="server" CssClass="font_css" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="PDF View &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Selected="True" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Direct View" Value="1"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="box-footer" style="text-align: center">
                            <button id="btnsubmit" onserverclick="btnsubmit_ServerClick" runat="server" class="btn btn-info" accesskey="S" style="width: 100px"><u>S</u>ubmit</button>
                            <button id="btnexit" onserverclick="btnexit_ServerClick" runat="server" class="btn btn-default" accesskey="x" style="width: 100px">E<u>x</u>it</button>
                        </div>
                    </div>
                </div>
            </div>
        </section>

        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="hf1" runat="server" />
        <asp:Button ID="btniBox" runat="server" OnClick="btniBox_Click" Style="display: none" />
    </form>
</body>
</html>
