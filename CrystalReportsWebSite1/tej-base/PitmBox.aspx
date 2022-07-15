<%@ Page Language="C#" AutoEventWireup="true" Inherits="PitmBox" CodeFile="PitmBox.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
    
    <link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />    

    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>    

    <link type="text/css" rel="Stylesheet" href="../tej-base/Scripts/colorbox.css" />
    <link rel="stylesheet" type="text/css" href="../tej-base/Styles/fin.css" />
    <link rel="stylesheet" type="text/css" href="../tej-base/Styles/vip_vrm.css" />

    <script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>    
    <script type="text/javascript">
        function closePopup1() {
            $('#ContentPlaceHolder1_btnhideF', window.parent.document).trigger('click');
            parent.$.colorbox.close();
        }
        function onlyclose() {
            parent.$.colorbox.close();
        }
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
                            <h3 class="box-title" id="lblheader" runat="server">Party Code/Item Code Selection</h3>
                        </div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="lbl1" runat="server" class="col-sm-2 control-label" title="lbl1">Party Code</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnAcode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtAcode" type="text" class="form-control clearable" runat="server" placeholder="Party Code" style="height: 28px" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtAname" type="text" class="form-control clearable" runat="server" placeholder="Party Name" style="height: 28px" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Item Code</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnIcode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px;" OnClick="btnIcode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtIcode" type="text" class="form-control clearable" runat="server" placeholder="Item Code" style="height: 28px" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtIname" type="text" class="form-control clearable" runat="server" placeholder="Item Name" style="height: 28px" />
                                </div>
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
        <asp:Button ID="btniBox" runat="server" OnClick="btniBox_Click" Style="display: none" />
    </form>
</body>
</html>
