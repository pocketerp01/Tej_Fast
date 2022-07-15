<%@ Page Language="C#" AutoEventWireup="true" Inherits="om_klas_val" CodeFile="om_klas_val.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Finsys</title>
    <link rel="stylesheet" href="../tej-base/Styles/vip_vrm.css" />    
    <script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/jquery.colorbox.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>

    <script type="text/javascript">
        function closePopup() {

            $('#ContentPlaceHolder1_btnhideF', window.parent.document).trigger('click');
            parent.$.colorbox.close();

        }
        $(document).ready(function () {
        });
    </script>



</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 50px;">
            <table align="center">
                <tr>
                    <td class="font_css" colspan="2" align="center">Please Fill the Values
                    </td>
                </tr>
                <tr>
                    <td class="font_css">Bucket 1
                    </td>
                    <td>
                        <asp:TextBox ID="tk1" runat="server" Width="70px" MaxLength="5" Text="300"
                            onkeypress="return isDecimalKey(event)" oncontextmenu="return false;"
                            onpaste="return false" Style="text-align: right"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="font_css">Bucket 2
                    </td>
                    <td>
                        <asp:TextBox ID="tk2" runat="server" Width="70px" MaxLength="5" Text="500"
                            onkeypress="return isDecimalKey(event)" oncontextmenu="return false;"
                            onpaste="return false" Style="text-align: right"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="font_css">Bucket 3
                    </td>
                    <td>
                        <asp:TextBox ID="tk3" runat="server" Width="70px" MaxLength="5" Text="750"
                            onkeypress="return isDecimalKey(event)" oncontextmenu="return false;"
                            onpaste="return false" Style="text-align: right"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="font_css">Bucket 4
                    </td>
                    <td>
                        <asp:TextBox ID="tk4" runat="server" Width="70px" MaxLength="5" Text="1000"
                            onkeypress="return isDecimalKey(event)" oncontextmenu="return false;"
                            onpaste="return false" Style="text-align: right"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="font_css">Bucket 5
                    </td>
                    <td>
                        <asp:TextBox ID="tk5" runat="server" Width="70px" MaxLength="5" Text="1500"
                            onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right"></asp:TextBox>
                    </td>
                </tr>
                <tr align="center">
                    <td class="font_css" colspan="2">
                        <button id="btnok" runat="server" class="btnyes" accesskey="O" style="width: 92px; height: 30px;" onserverclick="btnok_ServerClick"><u>O</u>k</button></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblerr" runat="server" ForeColor="#FF3300" Font-Size="Small"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfval" runat="server" />
    </form>
</body>
</html>
