<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="moreReports" Title="Tejaxo" CodeFile="moreReports.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="height: 500px; background-color: white"></div>    

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" style="display:none" />
</asp:Content>
