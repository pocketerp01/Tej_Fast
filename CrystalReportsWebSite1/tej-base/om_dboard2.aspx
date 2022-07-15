<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_dboard2" CodeFile="om_dboard2.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/highcharts.js" type="text/javascript"> </script>
    <script src="Scripts/highcharts-more.js" type="text/javascript"> </script>
    <script src="Scripts/exporting.js" type="text/javascript"></script>

    <script type="text/javascript">
        function btnBox1() {
            document.getElementById("btnBox1").click();
        };
        function btnBox2() {
            document.getElementById("btnBox2").click();
        };
        function btnBox3() {
            document.getElementById("btnBox3").click();
        };
        function btnBox4() {
            document.getElementById("btnBox4").click();
        };
    </script>
    <script>
        //Set the date we're counting down to
        var countDownDate = new Date("Jan 5, 2018 15:37:25").getTime();

        // Update the count down every 1 second
        function CalT() {
            // Get todays date and time
            var now = new Date().getTime();

            // Find the distance between now an the count down date
            var distance = countDownDate - now;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000) - 30;

            // Output the result in an element with id="demo"
            document.getElementById("demo").innerHTML = "Time Left : " + seconds + " s ";
            if (seconds < 0) {
                CalT();
            }
        }
        function btnTimer() {
            //document.getElementById("ContentPlaceHolder1_btnNext").click();
        };
        //var x = setInterval(btnTimer, 10000);
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Timer ID="timer1" runat="server"
        Enabled="true" Interval="10000" OnTick="timer1_Tick">
    </asp:Timer>
    <%--<asp:UpdatePanel ID="upd" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="timer1" EventName="Tick" />
        </Triggers>
        <ContentTemplate>--%>
    <div class="content-wrapper">
        <section class="content-header">
            <div style="text-align: right">
                <p id="demo" style="float: left"></p>
                <asp:ImageButton ID="btnLeft" runat="server" ImageUrl="~/tej-base/images/left.png" OnClick="btnLeft_Click" Style="width: 22px; height: 20px;" ToolTip="Left" />
                &nbsp;
                <asp:ImageButton ID="btnPlay" runat="server" ImageUrl="~/tej-base/images/playButton.png" OnClick="btnPlay_Click" Style="width: 25px; height: 25px;" Visible="false" />
                <asp:ImageButton ID="btnPause" runat="server" ImageUrl="~/tej-base/images/pauseButton.png" OnClick="btnPause_Click" Style="width: 25px; height: 25px;" Visible="false" />
                &nbsp;
                <asp:ImageButton ID="btnRight" runat="server" ImageUrl="~/tej-base/images/right.png" OnClick="btnRight_Click" Style="width: 22px; height: 20px;" ToolTip="Right" />
            </div>
            <div class="row" id="topBoxes" runat="server" style="display: none">

                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-aqua">
                        <div class="inner">
                            <h3>
                                <asp:Label ID="lblBox1Count" runat="server"></asp:Label>
                            </h3>
                            <p>
                                <asp:Label ID="lblBox1Header" runat="server"></asp:Label>

                            </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <a href="#" onclick="btnBox1()" style="display: none" class="small-box-footer">More info 
                            <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox1" runat="server" style="display: none" onserverclick="btnBox1_ServerClick"></button>
                    </div>
                </div>

                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-green">
                        <div class="inner">
                            <h3>
                                <asp:Label ID="lblBox2Count" runat="server"></asp:Label>
                            </h3>
                            <p>
                                <asp:Label ID="lblBox2Header" runat="server"></asp:Label>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-stats-bars"></i>
                        </div>
                        <a href="#" onclick="btnBox2()" style="display: none" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox2" runat="server" style="display: none" onserverclick="btnBox2_ServerClick"></button>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <div class="small-box bg-yellow">
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
                            <i class="fa fa-suitcase"></i>
                        </div>
                        <a href="#" onclick="btnBox3()" style="display: none" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                        <button id="btnBox3" runat="server" style="display: none" onserverclick="btnBox3_ServerClick"></button>
                    </div>
                </div>
                <!-- ./col -->
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-red">
                        <div class="inner">
                            <h3>
                                <asp:Label ID="lblBox4Count" runat="server"></asp:Label>
                            </h3>
                            <p>
                                <asp:Label ID="lblBox4Header" runat="server"></asp:Label>
                            </p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-pie-graph"></i>
                        </div>
                        <a href="#" onclick="btnBox4()" style="display: none" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
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
                    <div>
                        <div class="box-header">
                            <i class="ion ion-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart1Header" runat="server"></asp:Label>
                            </h3>
                            <asp:ImageButton ID="btnZoom1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" ImageAlign="Right" OnClick="btnZoom1_Click" Style="display: none" />
                        </div>
                        <div class="box-body" id="chart1" style="height: 300px">
                        </div>
                    </div>
                </section>
                <section class="col-lg-6" id="chartDiv2" runat="server">
                    <div>
                        <div class="box-header">
                            <i class="ion ion-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart2Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body" id="chart2" style="height: 300px">
                        </div>
                    </div>
                </section>
            </div>
            <div class="row">
                <!-- Left col -->
                <section class="col-lg-6" id="chartDiv3" runat="server">
                    <div>
                        <div class="box-header">
                            <i class="ion ion-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart3Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body" id="chart3" style="height: 300px">
                        </div>
                    </div>
                </section>
                <section class="col-lg-6" id="chartDiv4" runat="server">
                    <div>
                        <div class="box-header">
                            <i class="ion ion-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart4Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="box-body" id="chart4" style="height: 300px">
                        </div>
                    </div>
                </section>

            </div>
        </section>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <button id="btnNext" runat="server" style="display: none" onserverclick="btnNext_ServerClick"></button>
</asp:Content>

