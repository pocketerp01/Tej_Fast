<%@ Page Language="C#" AutoEventWireup="true" Inherits="ival_ctnid" CodeFile="ival_ctnid.aspx.cs" %>

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
            <table style="font-family: Arial; font-size: 14px; font-weight: bold;" cellpadding="0" cellspacing="0">
                <tr>
                    <td rowspan="10">
                        <img src="images/techInfoCtnID.jpg" alt="" />
                    </td>
                    <td style="width: 200px">Length ID (mm)</td>
                    <td>
                        <asp:TextBox ID="tk1" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Width ID (mm)</td>
                    <td>
                        <asp:TextBox ID="tk2" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Height ID (mm)</td>
                    <td>
                        <asp:TextBox ID="tk3" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Ply</td>
                    <td>
                        <asp:TextBox ID="tk4" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Trimming (Reel Size)</td>
                    <td>
                        <asp:TextBox ID="tk5" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Trimming (Cutting Size)</td>
                    <td>
                        <asp:TextBox ID="tk6" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Joint Flap</td>
                    <td>
                        <asp:TextBox ID="tk7" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Each Flap Addition</td>
                    <td>
                        <asp:TextBox ID="tk8" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>No of Ups</td>
                    <td>
                        <asp:TextBox ID="tk9" runat="server" oncontextmenu="return false;" onpaste="return false" onkeypress="return isDecimalKey(event)" MaxLength="10" Width="100px"></asp:TextBox></td>
                </tr>

                <tr>
                    <td>
                        <button id="btnok" runat="server" accesskey="O" class="btnyes" style="width: 120px; height: 30px; background-color: #3399FF;" onserverclick="btnok_ServerClick"><u>O</u>k</button></td>
                    <td>
                        <button id="btnNo" runat="server" class="btnno" style="width: 120px; height: 30px;" onserverclick="btnNo_ServerClick">Cancel</button></td>
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
            function onlyclosePopup() {
                parent.$.colorbox.close();
            }
        </script>
    </form>
</body>
</html>
