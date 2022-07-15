<%@ Page Language="C#" AutoEventWireup="true" Inherits="drawPrevFull" CodeFile="drawPrevFull.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drawing Preview</title>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scr" runat="server">
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="600000" Enabled="true">
        </asp:Timer>
        <asp:UpdatePanel ID="Panel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <button id="btnpopup" runat="server" style="width: 70px;" onserverclick="btnpopup_ServerClick">
                    Exit</button>
                <asp:Label ID="Label1" runat="server"></asp:Label><br />
                <iframe style="position: absolute; height: 100%; width: 100%;" frameborder="0" id="Iframe1"
                    runat="server" ></iframe>
            </ContentTemplate>
        </asp:UpdatePanel>
        <table style="display: none">
            <tr>
                <td>
                    <asp:TextBox ID="txtentryno" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtuser" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtvchdate" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div style="display: none">
            Issued By
        <asp:Label ID="lbl1" runat="server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp; Issued Till
        <asp:Label ID="lbl3" runat="server"></asp:Label>
        </div>
        <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="edmode" runat="server" />
        <asp:HiddenField ID="hf1" runat="server" />
        <asp:HiddenField ID="hf2" runat="server" />
    </form>
</body>
</html>
