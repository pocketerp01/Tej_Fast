<%@ Control Language="C#" AutoEventWireup="true" Inherits="fin_base_controls_deskBox" CodeFile="deskBox.ascx.cs" %>
<link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
<link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />

<script src="../tej-base/Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
<script src="../tej-base/Scripts/jquery.colorbox-min.js" type="text/javascript"></script>
<script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
<script src="../tej-base/Scripts/highcharts.js" type="text/javascript"> </script>
<script src="../tej-base/Scripts/exporting.js" type="text/javascript"></script>
<script src="../tej-base/Scripts/temp.js" type="text/javascript"></script>
<style type="text/css">
    .grad {
        background-image: linear-gradient(to bottom,#b9ebef,#4fadb6);
        /*background-image: linear-gradient(to bottom,yellow,red);*/
    }

    .grad1 {
        background-image: linear-gradient(to bottom,rgba(243,227,226,.12),#fff);
    }

        /* if want to remove on hover color : remove or comment below tag */
        .grad1:hover {
            background-color: #f6dce0;
        }

    .zoom {
        transition: transform .2s;
        margin: 0 auto;
    }

        .zoom:hover {
            -ms-transform: scale(1.05);
            -webkit-transform: scale(1.05);
            transform: scale(1.05);
            s font-weight: 600;
            border-color: ActiveBorder;
        }

    .overlay {
        position: absolute;
        bottom: 0;
        left: 0;
        right: 0;
        overflow: hidden;
        height: 0;
        transition: .3s ease;
    }

    .box-body:hover .overlay {
        height: 18%;
        left: 40%;
    }
</style>
<script type="text/javascript">
    function JSFunction() {
        <%--__doPostBack('<%= myUpdatePanel.ClientID  %>', '');--%>
    }
    $(function () {
        //showDsk();        
    });

    function changeText() {
        var texttos = "Preparing the Dashboard";
        var dot = ".";
        setInterval(function () {
            document.getElementById("ContentPlaceHolder1_deskBox_changeText").innerHTML = texttos + dot;
            dot = dot + ".";
        }, 500);
    }

    function showDsk() {
        var qstr = document.getElementById("<%=hfQstr.ClientID %>").value;
        $.ajax({
            type: "POST",
            url: "/tej-base/jcpSave.asmx/getJsonData?STR=" + qstr + "",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            failure: function (response) {
                alert(response.d);
            },
            error: function (response) {
                alert(response.d);
            }
        });
    }
    function OnSuccess(response) {
        debugger;
        var xmlDoc = $.parseXML(response.d);
        var xml = $(xmlDoc);
        var customers = xml.find("Tiles");
        var row = $("#ListBox1 table").eq(0).clone(true);
        $("#ListBox1 table").eq(0).remove();
        $.each(customers, function () {
            var customer = $(this);
            debugger;
            $("f1", row).eq(0).html($(this).find("DB_QUERY").text());
            //$("#ListBox1]").append(row);
            //row = $("#ListBox1 table").clone(true);
        });
    };
</script>
<script type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });
    function EndRequestHandler(sender, args) {
        if (args.get_error() != undefined) {
            args.set_errorHandled(true);
        }
    }
</script>
<div class="content-wrapper grad" style="overflow: auto; height: 100%;">
    <asp:UpdatePanel ID="upd1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <section class="content-header">
                <div id="changeText" style="font-size: medium; text-align: center;" runat="server"></div>
                <div class="row" style="margin: 2px;">
                    <asp:Button ID="br" runat="server" Text="1111111111" OnClick="br_Click" Visible="false" />
                    <asp:ListView ID="ListBox1" runat="server" OnSelectedIndexChanging="ListBox1_SelectedIndexChanging"
                        OnItemDataBound="ListBox1_ItemDataBound">
                        <ItemTemplate>
                            <div class="col-md-2 zoom" id="mainBox" runat="server">
                                <div class="box box-default">
                                    <%--#520404--%>
                                    <div class="box-body grad1" style="min-height: 150px; color: #333;">
                                        <div style="display: none">
                                            <asp:Label ID="lbl" runat="server" Text='<%# Eval("fstr") %>'></asp:Label>
                                        </div>
                                        <div style="font-size: 16px;" id="f1" runat="server">
                                            <%# Eval("field1") %>
                                        </div>
                                        <div style="font-size: 12px;" id="f2" runat="server">
                                            <%# Eval("field2") %>
                                        </div>
                                        <div id="brLine" runat="server">
                                            <br />
                                            <br />
                                        </div>
                                        <div style="font-size: 14px; float: right" id="f3" runat="server">
                                            <%# Eval("field3") %>
                                        </div>
                                        <br />
                                        <div style="font-size: 14px; float: right" id="f4" runat="server">
                                            <%# Eval("field4") %>
                                        </div>

                                        <div id="chart1" runat="server" style="margin: 0 auto;"></div>

                                        <br />
                                        <div id="overLay" runat="server" class="overlay" style="margin-left: 10px;">
                                            <asp:ImageButton ID="sel" runat="server" CommandName="Select" ImageUrl="~/tej-base/images/submenu3.png" Width="22px" ToolTip="Click Me for More Details" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </section>
            <asp:HiddenField ID="hfQstr" runat="server" />
            <asp:Timer ID="time1" runat="server" OnTick="time1_Tick"></asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
