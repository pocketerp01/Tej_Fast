<%@ Page Language="C#" AutoEventWireup="true" Inherits="msg1" CodeFile="msg1.aspx.cs" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title></title>
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport" />

    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="upd1" runat="server" EnableCdn="true"></asp:ScriptManager>
        <div style="margin-top: 38px;">
            <table id="tmsg" runat="server" align="center">
                <tr>
                    <td style="height: 72px; width: 350px;">
                        <asp:Label ID="lblmsg" runat="server" Text=""
                            Style="font-size: 13px; font-family: Arial, Helvetica, sans-serif; color: #333333"></asp:Label>
                    </td>
                </tr>
                <tr id="trconf" runat="server" align="center">
                    <td>
                        <button id="btnyes" runat="server" class="btnyes" accesskey="Y" style="width: 92px; height: 30px;" onserverclick="btnyes_ServerClick"><u>Y</u>es</button>
                        <button id="btnno" runat="server" class="btnno" accesskey="N" style="width: 92px; height: 30px;" onserverclick="btnno_ServerClick"><u>N</u>o</button>
                        <button id="btn3" runat="server" class="btnyes" accesskey="3" style="width: 92px; height: 30px;" onserverclick="btn3_ServerClick"><u>3</u></button></td>
                </tr>
                <tr id="tralert" runat="server" align="center">
                    <td>
                        <button id="btnok" runat="server" class="btnyes" accesskey="O" style="width: 92px; height: 30px;" onserverclick="btnok_ServerClick"><u>O</u>k</button></td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfval" runat="server" />

        <script type="text/javascript">
            function closePopup() { $("#ContentPlaceHolder1_btnhideF", window.parent.document).trigger("click"), parent.$.colorbox.close() } function closePopup2() { $("#ContentPlaceHolder1_btnhideF_s", window.parent.document).trigger("click"), parent.$.colorbox.close() } function closePopup3() { $("#btnhideF", window.parent.document).trigger("click"), parent.$.colorbox.close() } function closePopup4() { $("#btnhideF_s", window.parent.document).trigger("click"), parent.$.colorbox.close() } function closePopup5() { $("#btnhideF", window.parent.document).trigger("click"), parent.$.colorbox.close() }
        </script>
    </form>
</body>
</html>
