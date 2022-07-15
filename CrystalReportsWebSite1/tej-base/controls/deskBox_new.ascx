<%@ Control Language="C#" AutoEventWireup="true" Inherits="fin_base_controls_deskBox89" CodeFile="deskBox_new.ascx.cs" %>
<link rel="stylesheet" href="../tej-base/bootstrap/css/bootstrap.min.css" />
<link rel="stylesheet" href="../tej-base/dist/css/AdminLTE.min.css" />

<script src="../tej-base/Scripts/jquery-ui.min.js" type="text/javascript"></script>
<script src="../tej-base/Scripts/highcharts.js" type="text/javascript"> </script>
<script src="../tej-base/Scripts/exporting.js" type="text/javascript"></script>
<style type="text/css">
    .grad {
        /*background-image: linear-gradient(to bottom,#b9ebef,#4fadb6);*/
        background-color: #F3E6AB;
        /*background-image: linear-gradient(to bottom,#6b6877,#fff );
        background-image: linear-gradient(to bottom,#b9ebef,#4fadb6);*/
    }

    .grad2 {
        /*background-image: linear-gradient(to bottom,#b9ebef,#4fadb6);*/
        background-color: #FFDEAD;
    }

    .grad1 {
        background-image: linear-gradient(to bottom,rgba(243,227,226,.12),#fff);
    }

        /* if want to remove on hover color : remove or comment below tag */
        .grad1:hover {
            background-color: #f6dce0;
        }

    .shadow {
        box-shadow: 3px 2px 15px 0 #584e4e;
    }

    .zoom {
        transition: transform .2s;
        margin: 0 auto;
    }

        .zoom:hover {
            -ms-transform: scale(1.05);
            -webkit-transform: scale(1.05);
            transform: scale(1.05);
            font-weight: 500;
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
        height: 30px;
        background-color: #e6e5e5;
    }
</style>
<script type="text/javascript">
    function showDsk() {
        $.ajax({
            type: "POST",
            url: '/deskBox.ascx/fillBox',
            data: {},
            success: function () {
                alert('done');
            }
        });
    }
</script>
<div id="deskBackG" runat="server" class="content-wrapper grad" style="overflow: auto; height: 100%; font-family: 'Source Sans Pro','Helvetica Neue',Helvetica,Arial,sans-serif;">
    <section class="content-header">
        <div class="row" style="margin: 2px;">
            <asp:Button ID="br" runat="server" Text="1111111111" Style="display: none" />
            <asp:ListView ID="ListBox1" runat="server" OnSelectedIndexChanging="ListBox1_SelectedIndexChanging"
                OnItemDataBound="ListBox1_ItemDataBound" OnDataBound="ListBox1_DataBound">
                <ItemTemplate>
                    <div class="col-md-2 zoom" id="mainBox" runat="server">
                        <div class="box shadow">
                            <%--#520404--%>
                            <div id="boxBodyC" runat="server" class="box-body">
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
                                <div id="Div1" runat="server" style="margin-top: 2px">
                                    <br />
                                </div>
                                <div style="font-size: 14px; float: right" id="f4" runat="server">
                                    <%# Eval("field4") %>
                                </div>

                                <div id="chart1" runat="server" style="margin: 0 auto;"></div>
                                <div id="overLay" runat="server" class="overlay" style="text-align: center">
                                    <asp:ImageButton ID="sel" runat="server" CommandName="Select" ImageUrl="~/tej-base/images/submenu3.png" Width="22px" ToolTip="Click Me for More Details" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:ListView>
        </div>

    </section>
</div>
