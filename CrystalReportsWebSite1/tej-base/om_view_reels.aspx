﻿<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" CodeFile="om_view_reels.aspx.cs" Inherits="om_view_reels" Title="Tejaxo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 568px; background-color: white"></div>
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfval" runat="server" />
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hfbr" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfaskBranch" runat="server" />
    <asp:HiddenField ID="hfaskPrdRange" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
</asp:Content>
