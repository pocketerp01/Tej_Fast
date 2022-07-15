<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_view_rpt_M1_reps" Title="Tejaxo" CodeFile="om_view_rpt_M1_reps.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 500px; background-color: white"></div>
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfval" runat="server" />
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hfbr" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfaskBranch" runat="server" />
    <asp:HiddenField ID="hfaskPrdRange" runat="server" />
    <asp:HiddenField ID="hfSales" runat="server" />
    <asp:HiddenField ID="hfHead" runat="server" />
    <asp:HiddenField ID="hfParty" runat="server" />
    <asp:HiddenField ID="hfValue" runat="server" />
    <asp:HiddenField ID="hfOpen" runat="server" />
     <asp:HiddenField ID="hfDeptt" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
</asp:Content>
