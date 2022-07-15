<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_view_mis" Title="Tejaxo" CodeFile="om_view_mis.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <div style="height: 500px; background-color: white"></div>--%>
    <div id="div4" align="center" data-role="controlgroup" data-type="horizontal" runat="server" style="background-color: white">
        <input type='button' class="btn btn-info" id="g1" value='Go Faster' onclick='changeTemp(1)' runat="server" />
        <input type='button' class="btn btn-info" id="g2" value='Slow Down' onclick='changeTemp(-1)' runat="server" style="background-color: white" />
        <input type='button' onclick='openEditor()' value='Open Editor' data-theme='f' id="g3" runat="server" style="background-color: white" />
    </div>
    <div id="div2" align="center" runat="server" style="background-color: white; font-size: large; font-family: Arial, Helvetica, sans-serif">
    </div>
    <div id="div5" align="center" runat="server" style="background-color: white; font-size: large; font-family: Arial, Helvetica, sans-serif">
    </div>
    <div id="chart" style="width: 100%; height: 500px; background-color: white"></div>
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfval" runat="server" />
    <asp:HiddenField ID="hfcode" runat="server" />
    <asp:HiddenField ID="hfbr" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfaskBranch" runat="server" />
    <asp:HiddenField ID="hfaskPrdRange" runat="server" />
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />

    <asp:HiddenField ID="hdnChartData" runat="server" />
    <asp:HiddenField ID="hdnHAxisTitle_Bar" runat="server" />
    <asp:HiddenField ID="hdnVAxisTitle_Bar" runat="server" />
</asp:Content>
