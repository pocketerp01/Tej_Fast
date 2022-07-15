<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="prsn_mst" Title="Tejaxo" CodeFile="prsn_mst.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table width="700px" align="center" style="border-style: groove; border-width: thin; margin-top:5px; ">
    <tr>
    <td colspan="4" class="header" runat="server" id="tdheader">
    Person Master
    </td>
    </tr>
    <tr>
    <td colspan="4" align="center" style="border-style:groove; border-width:thin;">
            <button id="btnnew" runat="server" accesskey="N" onserverclick="btnnew_ServerClick" class="myButton"><u>N</u>ew</button>
            <button id="btnedit" runat="server" accesskey="i" class="myButton" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
            <button id="btnsave" runat="server" accesskey="S" class="myButton" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
            <button id="btndel" runat="server" accesskey="l" class="myButton" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
            <button id="btnlist" runat="server" accesskey="t" class="myButton" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
            <asp:Button ID="btnext" runat="server" Text="Exit" class="myButton" onclick="btnext_Click" 
                    />
                    </td>
    </tr>
    <tr>
    <td class="font_css" runat="server" id="tdname">Name</td><td><asp:TextBox ID="txtname" runat="server" Width="200px" ></asp:TextBox></td>
    <td class="font_css">Code</td>
    <td><asp:TextBox ID="txtvchnum" runat="server" Width="70px" ReadOnly="true"></asp:TextBox></td>
    </tr>
    <tr>
    <td class="font_css" id="tdmobno" runat="server">Email ID</td><td><asp:TextBox ID="txtemailid" Width="200px" runat="server"></asp:TextBox></td>
    <td class="font_css" runat="server" id="tddept">Dept.</td>
    <td class="rounded_corners">
    <asp:DropDownList ID="dd1" runat="server">
    </asp:DropDownList></td>
    </tr>
</table>
<asp:Button ID="btnhideF" runat="server" onclick="btnhideF_Click" style="display:none" />
<asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" style="display:none" />
<asp:HiddenField ID="hffield" runat="server" />
<asp:HiddenField ID="edmode" runat="server" />
<asp:HiddenField ID="hfhcid" runat="server" />
</asp:Content>