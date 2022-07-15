<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_dbd_gendb_google" CodeFile="om_dbd_gendb_google.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/highcharts.js" type="text/javascript"> </script>
    <script src="Scripts/highcharts-more.js" type="text/javascript"> </script>
    <script src="Scripts/exporting.js" type="text/javascript"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

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
        //5000 300000
    </script>
    <style>
        .charts {
            width: 100%;
        }

        #chart_wrap {
            position: relative;
            padding-bottom: 100%;
            height: 0;
            overflow: hidden;
        }

        #chart1 {
            /*position: absolute;
    top: 0;
    left: 0;
    width:100%;
    height:500px;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Timer ID="timer1" runat="server"
        Enabled="true" Interval="60000" OnTick="timer1_Tick">
    </asp:Timer>
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right">
                        <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="~/tej-base/images/Previous.JPG" Style="height: 30px" OnClick="btnPrev_Click" />
                        <asp:ImageButton ID="btnNext" runat="server" ImageUrl="~/tej-base/images/next.JPG" Style="height: 30px" OnClick="btnNext_Click" />
                    </td>
                </tr>
            </table>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-lg-3 col-xs-6" style="display: none">
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

                <div class="col-lg-3 col-xs-6" style="display: none">
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
                <div class="col-lg-3 col-xs-6" style="display: none">
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
                <div class="col-lg-3 col-xs-6" style="display: none">
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
                        <div class="chart">
                            <div class="box-body" id="chart1" style="height: 250px; width: 85%">
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
                        <div class="chart">
                            <div class="box-body" id="chart2" style="height: 250px; width: 85%">
                            </div>
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
                        <div class="box-body" id="chart3" style="height: 250px; width: 85%">
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
                        <div class="chart">
                            <div class="box-body" id="chart4" style="height: 250px; width: 85%">
                            </div>
                        </div>
                    </div>
                </section>
                <section class="col-lg-6" id="chartDiv5" runat="server">
                    <div class="box box-success rounded_corners">
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart5Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="chart">
                            <div class="box-body" id="chart5" style="height: 250px; width: 85%">
                            </div>
                        </div>
                    </div>
                </section>
                <section class="col-lg-6" id="chartDiv6" runat="server">
                    <div class="box box-success rounded_corners">
                        <div class="box-header">
                            <i class="fa fa-clipboard"></i>
                            <h3 class="box-title">
                                <asp:Label ID="lblChart6Header" runat="server"></asp:Label>
                            </h3>
                        </div>
                        <div class="chart">
                            <div class="box-body" id="chart6" style="height: 250px; width: 85%">
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hdnChartData" runat="server" />
    <asp:HiddenField ID="hdnHAxisTitle_Bar" runat="server" />
    <asp:HiddenField ID="hdnVAxisTitle_Bar" runat="server" />
</asp:Content>
