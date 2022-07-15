<%@ Page Language="C#" AutoEventWireup="true" Inherits="ival" CodeFile="ival.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
    <link href="../tej-base/Styles/vip_vrm.css" rel="stylesheet" type="text/css" />
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 40px;">
            <table align="center">
                <tr>
                    <td class="font_css" id="tdcaption" runat="server" align="center">Please Enter your Input
                    </td>
                </tr>
                <tr>
                    <td id="valper" runat="server" align="center" style="width: 100px;" class="rounded_corners">
                        <asp:RadioButtonList ID="rd1" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Rs" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="%" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td id="tdtk" runat="server" align="center">
                        <asp:TextBox ID="tk1" runat="server" oncontextmenu="return false;" ></asp:TextBox>
                    </td>
                </tr>
                <tr align="center" style="width:100%">
                    <td>
                        <button id="btnok" runat="server" class="btnyes" accesskey="O" style="width: 92px; height: 30px;" onserverclick="btnok_ServerClick"><u>O</u>k</button>                                   
                        <button id="btnExit" runat="server" class="btnyes" accesskey="X" style="width: 92px; height: 30px;" onserverclick="btnExit_ServerClick">E<u>x</u>it</button></td> 
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblerr" runat="server" ForeColor="#FF3300" Font-Size="Small"></asp:Label>
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
            function closePopup2() {
                $('#ContentPlaceHolder1_btnhideF_s', window.parent.document).trigger('click');
                parent.$.colorbox.close();
            }
            function onlyclose() {
                parent.$.colorbox.close();
                        }
        </script>
    </form>
</body>
</html>
