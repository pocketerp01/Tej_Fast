<%@ Page Language="C#" AutoEventWireup="true" Inherits="confirmOtp" CodeFile="confirmOtp.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
      <link href="../tej-base/Styles/extra.css" rel="stylesheet" type="text/css" />
    <link href="../tej-base/Styles/fin.css" rel="stylesheet" type="text/css" />
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../tej-base/Scripts/jquery.min.js"></script>
    <script type="text/javascript">
        function onlyclose() {
            parent.$.colorbox.close();
        }
        function closePopup(btn) {
            $(btn, window.parent.document).trigger('click');
            parent.$.colorbox.close();
        }
    </script>
    <style type="text/css">
        input {
            border: 0;
            outline: none;
            margin: 0;
            padding: 0;
        }

        .stealthy {
            left: 0;
            margin: 0;
            max-height: 1px;
            max-width: 1px;
            opacity: 0;
            outline: none;
            overflow: hidden;
            pointer-events: none;
            position: absolute;
            top: 0;
            z-index: -1;
        }
    </style>
    <script type="text/javascript">
        function clickEnter(obj, evt) {

            if (evt.keyCode > 0) {
                keyCode = event.keyCode;
            }
            else if (evt.which > 0) {
                keyCode = evt.which;
            }
            else {
                keycode = evt.charCode;
            }
            if (keyCode == 13) {
                document.getElementById(obj).focus();
                document.getElementById(obj).select();
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="mid-signin" style="padding-top: 45px">
            <input id="txtConfirmOtp" runat="server" class="signin-textfield"
                placeholder="Confirm OTP" autocomplete="off" />            

            <div style="padding-top: 10px; padding-bottom: 10px;">
                <asp:Label ID="lblerr" runat="server" Style="font-size: 13px; font-family: Arial; text-decoration: none;" Text="Please Enter OTP!!"></asp:Label>
            </div>

            <div align="center">
                <button id="btnOk" runat="server" accesskey="O" onserverclick="btnchngpwd_ServerClick" style="width: 100px; height: 32px;" class="btnyes"><u>O</u>k</button>&nbsp;&nbsp;
                                    <button id="btnext" runat="server" accesskey="x" onserverclick="btnext_ServerClick" style="width: 100px; height: 32px;" class="btnno">E<u>x</u>it</button>
            </div>
        </div>
    </form>
</body>
</html>
