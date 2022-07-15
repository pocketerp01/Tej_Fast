<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="mkt_ppc1" CodeFile="mkt_ppc1.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 1);
            //calculateSum();
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth,
                height: gridDiv.offsetHeight,
                headerrowcount: headerFreeze,
                freezesize: rowFreeze,
                barhovercolor: "#3399FF",
                barcolor: "#3399FF",
                startVertical: $("#<%=hfGridView1SV.ClientID%>").val(),
                startHorizontal: $("#<%=hfGridView1SH.ClientID%>").val(),
                onScrollVertical: function (delta) {
                    $("#<%=hfGridView1SV.ClientID%>").val(delta);
                },
                onScrollHorizontal: function (delta) {
                    $("#<%=hfGridView1SH.ClientID%>").val(delta);
                }
            });
            }

            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <button id="btnnew" runat="server" accesskey="S" onserverclick="btnnew_ServerClick" class="btn btn-info" style="width: 100px;"><u>S</u>how</button>
                        <button id="btnfull" runat="server" accesskey="c" onserverclick="btnfull_ServerClick" class="btn btn-info" style="width: 100px;">Full S<u>c</u>reen</button>
                        <button id="btnstop" runat="server" accesskey="c" onserverclick="btnstop_Click" class="btn btn-info" style="width: 100px;">S<u>tart</u></button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" Style="width: 100px;" OnClick="btnext_Click" />

                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Style="float: right" Font-Size="X-Large"></asp:Label></td>
                </tr>
            </table>
        </section>
        <section class="content">
            <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="timer1" EventName="Tick" />
                </Triggers>
                <ContentTemplate>

                    <div>
                        <asp:Label ID="lblstage" runat="server" Font-Size="20px" ForeColor="Black" Style="float: inherit"></asp:Label>
                        <%-- &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <asp:Label ID="lbltoday" runat="server" Font-Size="20px" ForeColor="Black" Style="float: inherit"></asp:Label>
                        <%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                        <asp:Label ID="lbl1" runat="server" Font-Size="20px" ForeColor="Black" Style="float: inherit"></asp:Label>

                    </div>

                    <asp:Timer ID="timer1" runat="server"
                        Enabled="true" Interval="6000" OnTick="timer1_Tick">
                    </asp:Timer>

                    <div class="lbBody" id="gridDiv" style="color: White; height: 500px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">

                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                            Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="true" PageSize="15" AllowPaging="true" OnRowDataBound="sg1_RowDataBound">
                            <RowStyle BackColor="#F7F6F3" ForeColor="Black" Height="20px" />
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />

                        </asp:GridView>

                    </div>


                    <div id="container" style="width: 100%; height: 100%; margin: 0 auto;"></div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </section>
    </div>

    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hfhcid" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />

</asp:Content>
