<%@ Page Language="C#" AutoEventWireup="true" Inherits="ival_PROD" CodeFile="ival_PROD.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <style type="text/css">
        .auto-style3 {
            color: #000000;
            font-family: Arial, Helvetica, sans-serif;
            font-weight: 700;
            font-size: 12px;
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        //$(function () {
        //    var $inp = $('.cls');
        //    $inp.bind('keydown', function (e) {
        //        var key = e.which;
        //        if (key == 13) {
        //            e.preventDefault();
        //            var nxtIdx = $inp.index(this) + 1;
        //            $(".cls:eq(" + nxtIdx + ")").focus();
        //        }
        //    });
        //});
    </script>
    <script type="text/javascript">
        $(document).ready(function () { $("input").not($(":button")).keypress(function (e) { if (13 == e.keyCode && (iname = $(this).val(), "Submit" !== iname)) { var t = $(this).parents("form:eq(0),body").find("button,input,textarea,select,textbox"), n = t.index(this); return n > -1 && n + 1 < t.length && (e.preventDefault(), t.eq(n + 1).focus(), t.eq(n + 1).select(), e.keyCode = 9), !1 } }) });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 40px;">
            <table align="center">
                <tr>
                    <td class="auto-style3" id="tdcaption" runat="server">Total Qty
                    </td>
                </tr>
                <tr>
                    <td id="tdtk" runat="server" align="center">
                        <asp:TextBox ID="tk1" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" Style="float: left" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" id="td3" runat="server">Per Roll Qty</td>
                </tr>
                <tr>
                    <td id="td4" runat="server" align="center">
                        <asp:TextBox ID="tk2" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" Style="float: left" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>


                <tr align="center">
                    <td>
                        <button id="btnok" runat="server" accesskey="O" class="btnyes" style="width: 92px; height: 30px; background-color: #3399FF;" onserverclick="btnok_ServerClick"><u>O</u>k</button></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblerr" runat="server" ForeColor="#FF3300" Font-Size="Small" Style="color: #FF0000; font-weight: 700; font-size: medium"></asp:Label>
                    </td>
                </tr>

                <tr style="display: none">
                    <td class="auto-style3" id="td5" runat="server">Rolls
                    </td>
                </tr>
                <tr style="display: none">
                    <td id="td6" runat="server" align="center">
                        <asp:TextBox ID="tk3" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" Style="float: left" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>

            </table>
        </div>
        <asp:HiddenField ID="hfval" runat="server" />
        <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>
        <script type="text/javascript">
            function closePopup() {
                $('#ContentPlaceHolder1_btnhideF', window.parent.document).trigger('click');
                parent.$.colorbox.close();
            }
        </script>
    </form>
</body>
</html>
