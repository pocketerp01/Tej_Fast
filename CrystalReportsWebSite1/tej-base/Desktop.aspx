<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="Desktop" CodeFile="Desktop.aspx.cs" %>

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
                  <%--      <img src="../bg-image/image_desktop.jpg" style="padding-left:190px;width: 1116px; height: 100%; z-index: 1" />--%>

        <%--<img src="../bg-image/logo.jpg" style="width: 200px; height: 100px; position: absolute; top: 8%; right: 44%; z-index: 2"  />--%>
    </div>
</asp:Content>

