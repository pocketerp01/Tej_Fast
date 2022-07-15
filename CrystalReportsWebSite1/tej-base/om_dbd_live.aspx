<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_dbd_live" CodeFile="om_dbd_live.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/highcharts.js" type="text/javascript"> </script>
    <script src="Scripts/highcharts-more.js" type="text/javascript"> </script>
    <script src="Scripts/exporting.js" type="text/javascript"></script>

    <script type="text/javascript">
        function btnBox1() {
            document.getElementById("ContentPlaceHolder1_btnBox1").click();
        };
        function btnBox2() {
            document.getElementById("ContentPlaceHolder1_btnBox2").click();
        };
        function btnBox3() {
            document.getElementById("ContentPlaceHolder1_btnBox3").click();
        };
        function btnBox4() {
            document.getElementById("ContentPlaceHolder1_btnBox4").click();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:Timer ID="timer1" runat="server"
        Enabled="true" Interval="10000" OnTick="timer1_Tick">
    </asp:Timer>

    <div class="content-wrapper">
        <section class="content-header">
            <div style="text-align: right">
                <p id="demo" style="float: left"></p>
                <asp:ImageButton ID="btnLeft" runat="server" ImageUrl="~/tej-base/images/left.png" OnClick="btnLeft_Click" Style="width: 22px; height: 20px;" ToolTip="Left" />
                &nbsp;
                <asp:ImageButton ID="btnPlay" runat="server" ImageUrl="~/tej-base/images/playButton.png" OnClick="btnPlay_Click" Style="width: 25px; height: 25px;" Visible="false" ToolTip="Play"/>
                <asp:ImageButton ID="btnPause" runat="server" ImageUrl="~/tej-base/images/pauseButton.png" OnClick="btnPause_Click" Style="width: 25px; height: 25px;" Visible="false" ToolTip="Pause"/>
                &nbsp;
                <asp:ImageButton ID="btnRight" runat="server" ImageUrl="~/tej-base/images/right.png" OnClick="btnRight_Click" Style="width: 22px; height: 20px;" ToolTip="Right" />
            </div>

            <div class="row" style="display: none">
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-maroon-gradient rounded_corners">
                        <div class="inner">
                            <h3>
                                <asp:Label ID="lblBox1Count" runat="server"></asp:Label>
                            </h3>
                            <p>
                                <asp:Label ID="lblBox1Header" runat="server"></asp:Label>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-tasks"></i>
                        </div>
                        <a href="#" onclick="btnBox1()" class="small-box-footer">More info 
                            <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox1" runat="server" style="display: none" onserverclick="btnBox1_ServerClick"></button>
                    </div>
                </div>

                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-olive-active rounded_corners">
                        <div class="inner">
                            <h3>
                                <asp:Label ID="lblBox2Count" runat="server"></asp:Label>
                            </h3>
                            <p>
                                <asp:Label ID="lblBox2Header" runat="server"></asp:Label>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-suitcase"></i>
                        </div>
                        <a href="#" onclick="btnBox2()" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox2" runat="server" style="display: none" onserverclick="btnBox2_ServerClick"></button>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-orange-active rounded_corners">
                        <div class="inner">
                            <div class="inner">
                                <h3>
                                    <asp:Label ID="lblBox3Count" runat="server"></asp:Label>
                                </h3>
                                <p>
                                    <asp:Label ID="lblBox3Header" runat="server"></asp:Label>
                                </p>
                            </div>
                        </div>
                        <div class="icon">
                            <i class="fa fa-thumbs-o-up"></i>
                        </div>
                        <a href="#" onclick="btnBox3()" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox3" runat="server" style="display: none" onserverclick="btnBox3_ServerClick"></button>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-green-active rounded_corners">
                        <div class="inner">
                            <h3>
                                <asp:Label ID="lblBox4Count" runat="server"></asp:Label>
                            </h3>
                            <p>
                                <asp:Label ID="lblBox4Header" runat="server"></asp:Label>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-balance-scale"></i>
                        </div>
                        <a href="#" onclick="btnBox4()" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox4" runat="server" style="display: none" onserverclick="btnBox4_ServerClick"></button>
                    </div>
                </div>
                <!-- ./col -->
            </div>
            <div class="row">
                <!-- Left col -->
                <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="X-Large" Style="text-align: center" Width="100%"></asp:Label>
                <br />
                <section class="col-lg-6" id="chartDiv1" runat="server">
                    <div class="box box-success rounded_corners">
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart1Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="no-padding">
                            <div class="box-body" id="chart1" style="height: 300px; ">
                            </div>
                        </div>
                    </div>
                </section>
                <section class="col-lg-6" id="chartDiv2" runat="server">
                    <div class="box box-success rounded_corners">
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart2Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body" id="chart2" style="height: 300px; ">
                        </div>
                    </div>
                </section>
            </div>
            <div class="row">
                <!-- Left col -->
                <section class="col-lg-6" id="chartDiv3" runat="server">
                    <div class="box box-success rounded_corners">
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart3Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body" id="chart3" style="height: 300px; ">
                        </div>
                    </div>
                </section>
                <section class="col-lg-6" id="chartDiv4" runat="server">
                    <div class="box box-success rounded_corners">
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart4Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body" id="chart4" style="height: 300px; ">
                        </div>
                    </div>
                </section>
            </div>
        </section>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />    
</asp:Content>

