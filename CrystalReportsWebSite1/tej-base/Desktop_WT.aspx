<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="Desktop_WT" EnableEventValidation="false" CodeFile="Desktop_WT.aspx.cs" %>

<%@ Register Src="~/tej-base/controls/deskBox.ascx" TagPrefix="fin" TagName="deskBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () { reSize(); });
        $(window).resize(function () { reSize(); });
        function reSize() { $("#divContent").height(($(window).height() - 120)); }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="divContent" style="min-height: 500px; background-color: ghostwhite;">
              <%--  <img src="../bg-image/image_desktop.jpg" style="padding-left:190px;width: 1116px; height: 100%; z-index: 1" />--%>
        <%--<fin:deskBox runat="server" ID="deskBox" />--%>
    </div>
</asp:Content>