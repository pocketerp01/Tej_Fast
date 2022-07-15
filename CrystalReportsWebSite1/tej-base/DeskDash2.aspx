<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="DeskDash2" CodeFile="DeskDash2.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/highcharts.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/highcharts-more.js" type="text/javascript"></script>
    <script src="../tej-base/Scripts/exporting.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Timer ID="timer1" runat="server"
        Enabled="true" Interval="10000" OnTick="timer1_Tick">
    </asp:Timer>

    <div class="content-wrapper">
        <section class="content-header">
            <div style="text-align: center">
                <asp:ImageButton ID="imgprev" runat="server" Height="30px" ImageUrl="~/tej-base/images/left.png"
                    OnClick="imgprev_Click" Style="float: left;" Width="50px" />
                <asp:ImageButton ID="imgnext" runat="server" Height="30px" ImageUrl="~/tej-base/images/right.png"
                    OnClick="imgnext_Click" Style="float: right;" Width="50px" />
                <asp:Label ID="lblhead" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
            </div>
            <div class="row" style="min-height: 500px">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <section class="col-lg-6" id="chart1" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="chart2" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-12" id="chart3" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="chart4" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="chart5" runat="server" style="height: 250px; padding: 2px;"></section>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <section class="col-lg-6" id="Div1" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div2" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-12" id="Div3" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-12" id="Div4" runat="server" style="height: 250px; padding: 2px;"></section>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                        <section class="col-lg-6" id="Div6" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div7" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div8" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div5" runat="server" style="height: 250px; padding: 2px;"></section>
                    </asp:View>
                    <asp:View ID="View4" runat="server">
                        <section class="col-lg-12" id="Div9" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div10" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div11" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div12" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div13" runat="server" style="height: 250px; padding: 2px;"></section>
                    </asp:View>
                    <asp:View ID="View5" runat="server">
                        <section class="col-lg-12" id="Div14" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-12" id="Div15" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div16" runat="server" style="height: 250px; padding: 2px;"></section>
                        <section class="col-lg-6" id="Div17" runat="server" style="height: 250px; padding: 2px;"></section>
                    </asp:View>
                </asp:MultiView>
                <asp:HiddenField ID="hfval" runat="server" />
            </div>
        </section>
    </div>
</asp:Content>
