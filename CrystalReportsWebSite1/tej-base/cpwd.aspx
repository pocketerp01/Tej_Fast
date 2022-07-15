<%@ Page Language="C#" AutoEventWireup="true" Inherits="cpwd4" CodeFile="cpwd.aspx.cs" %>

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

        function validatePassword() {
            var p = document.getElementById('txtnewpwd').value,
                errors = [];
            if (p.length < 8) {
                errors.push("Your password must be at least 8 characters");
            }
            if (p.search(/[a-zA-Z]/) < 0) {
                errors.push("Your password must contain at least one letter.");
            }
            if (p.search(/\d/) < 0) {
                errors.push("Your password must contain at least one digit.");
            }
            if (p.search(/[@#$%^&+=]/) < 0) {
                errors.push("Your password must contain at least one Special Char.");
            }
            if (errors.length > 0) {
                alert(errors.join("\n"));
                document.getElementById('pwd1').value = "WRONG";
                document.getElementById('txtnewpwd').style.borderColor = "Red";
                return false;
            }
            document.getElementById('pwd1').value = "";
            document.getElementById('txtnewpwd').style.borderColor = "";
            return true;
        }

    </script>
    <style type="text/css">
        input {
            border: 0;
            outline: none;
            margin: 0;
            padding: 0;
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
        <div align="center">
            <div class="signin-area">
                <div class="top-signin">
                </div>
                <div class="mid-signin">
                    <h1 class="sign-text">User Name:
                        <asp:Label ID="lblusername" runat="server"></asp:Label>
                    </h1>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="100%">
                                <asp:TextBox ID="txtoldpwd" runat="server" CssClass="signin-textfield" TextMode="Password"
                                    placeholder="Old Password" ToolTip="Old Password" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%">
                                <asp:TextBox ID="txtnewpwd" runat="server" CssClass="signin-textfield" TextMode="Password"
                                    placeholder="New Password" ToolTip="New Password" MaxLength="50" onblur="validatePassword()"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="100%">
                                <asp:TextBox ID="txtconfpwd" runat="server" CssClass="signin-textfield" TextMode="Password"
                                    placeholder="Confirm Password" ToolTip="Confirm Password" MaxLength="50"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td width="100%" align="center">
                                <asp:Label ID="lblerr" runat="server" Style="color: #f62217; font-size: 13px; font-family: Arial; text-decoration: none;"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="100%" align="center">
                                <button id="btnchngpwd" runat="server" accesskey="C" onserverclick="btnchngpwd_ServerClick" style="width: 100px; height: 32px;" class="btnyes"><u>C</u>ommit</button>&nbsp;&nbsp;
                                    <button id="btnext" runat="server" accesskey="x" onserverclick="btnext_ServerClick" style="width: 100px; height: 32px;" class="btnno">E<u>x</u>it</button>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="bottom-signin">
                </div>
            </div>
            <input id="pwd1" type="text" style="display: none" runat="server" />
        </div>
    </form>
</body>
</html>
