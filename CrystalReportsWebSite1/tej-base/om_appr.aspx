<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_appr78" Title="Tejaxo" Async="true" CodeFile="om_appr.aspx.cs" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
        });    
        function openLink() {
            var str = document.getElementById('ContentPlaceHolder1_lbllink').value;
            var res = str.split("~");
            for (var i = 0; i < res.length; i++) {
                window.open(res[i], "_blank");
            }
            document.getElementById('ContentPlaceHolder1_lbllink').value = "";
        }

        function checkAll(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=sg1.ClientID %>");
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length;
            for (var i = 0; i < gridCount; i++) {
                document.getElementById('ContentPlaceHolder1_sg1_chkapp_' + i).checked = Checkbox.checked;
                document.getElementById('ContentPlaceHolder1_sg1_chkrej_' + i).checked = false;
            }
        }
        function meAppcheck(Checkbox) {
            if (Checkbox.checked)
                document.getElementById(Checkbox.id.replace("chkapp", "chkrej")).checked = false;
        }
        function meRejcheck(Checkbox) {
            debugger;
            if (Checkbox.checked)
                document.getElementById(Checkbox.id.replace("chkrej", "chkapp")).checked = false;
        }
        function checkItem_All(objRef, colIndex) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var selectAll = GridView.rows[0].cells[colIndex].getElementsByTagName("input")[0];
            if (!objRef.checked) {
                selectAll.checked = false;
            }
            else {
                var checked = true;
                for (var i = 1; i < GridView.rows.length; i++) {
                    var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                    if (!chb.checked) {
                        checked = false;
                        break;
                    }
                }
                selectAll.checked = checked;
            }
        }
        function runProg() {
            setTimeout(updateProgress, 100);
        }

        function updateProgress() {
            $.ajax({
                type: "POST",
                url: "~/om_appr.aspx/GetText",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (msg) {
                    // TODO: revert the line below in your actual code
                    //$("#progressbar").progressbar("option", "value", msg.d);
                    $("#lblStatus").text(msg.d);
                    if (msg.d < 100) {
                        setTimeout(updateProgress, 100);
                    }
                }
            });
        }

    </script>

    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            // TODO: revert the line below in your actual code
            $("#progressbar").progressbar();

        });

        function runProg() {
            setTimeout(updateProgress, 100);
        }


        function updateProgress() {
            $.ajax({
                type: "POST",
                url: "om_appr.aspx/GetText",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (msg) {
                    // TODO: revert the line below in your actual code
                    //$("#progressbar").progressbar("option", "value", msg.d);
                    $("#lblStatus").text(msg.d);
                    if (msg.d < 100) {
                        debugger;
                        setTimeout(updateProgress, 100);
                    }
                }
            });
        }


    </script>

    <style type="text/css">
        .grd {
            margin-bottom: 100px;
            margin-left: 1000px;
            margin-right: 50px;
        }

        .ChkBoxClass input {
            width: 18px;
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <div class="content-wrapper">
        <section class="content-header">
            <%--<div class="box-footer">--%>
            <table style="width: 100%">
                <tr>

                   
                    <td style="text-align: left">
                        <asp:Button ID="btnGetData" runat="server" Text="Get Data" OnClick="btnGetData_Click" OnClientClick="runProg()" Visible="false" />

                        <button id="btnnew" runat="server" class="btn btn-info" onserverclick="btnnew_ServerClick" accesskey="t" style="width: 100px;">S<u>t</u>art</button>
                        <button id="btnsave" runat="server" class="btn btn-info" onserverclick="btnsave_ServerClick" accesskey="S" style="width: 100px;"><u>S</u>ave</button>
                        <button id="btnList" runat="server" class="btn btn-info" onserverclick="btnList_ServerClick" accesskey="l" style="width: 100px;"><u>L</u>ist</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" OnClick="btnext_Click" Style="width: 100px;" />
                    </td>
                     <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Task Managment"></asp:Label>
                        <asp:Label ID="lblsmallheader" runat="server" Font-Bold="True" Font-Size="Small" BackColor="WhiteSmoke" Text=""></asp:Label>
                    </td>
                </tr>

            </table>
            <%--</div>                --%>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch"
                                                placeholder="Enter here to search" ToolTip="Enter here to search" Height="30px" Width="250px"></asp:TextBox>
                                        </td>
                                        <td>&nbsp;
                                            <asp:ImageButton ID="srch" runat="server" ImageUrl="~/tej-base/images/search-button.png"
                                                Width="140px" Height="30px" TabIndex="2" ToolTip="Click to Search"
                                                OnClick="srch_Click" /></td>


                                        <td style="float: right"></td>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="tkrow" runat="server" Width="56px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                                                Style="text-align: right; display: none" Height="30px" Text="20"></asp:TextBox></td>
                                        <td style="float: right">
                                            <asp:Label ID="lblTotcount" runat="server" Style="font-size: 10px; font-family: 'Arial Unicode MS'"></asp:Label>
                                            <asp:Label ID="lblF1" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                            <ul class="nav nav-tabs" role="tablist" id="ul1" runat="server">
                                                <li>
                                                    <asp:Button ID="Button1" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 1" Style="margin-left: 30px;" OnClick="Button1_Click" />
                                                </li>
                                                <li>
                                                    <asp:Button ID="Button2" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 2" Style="margin-left: 5px;" OnClick="Button2_Click" />
                                                </li>
                                                <li>
                                                    <asp:Button ID="Button3" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 3" Style="margin-left: 5px;" OnClick="Button3_Click" />
                                                </li>
                                                <li>
                                                    <asp:Button ID="Button4" runat="server" CssClass="bg-green btn-foursquare" Text="Btn 4" Style="margin-left: 5px;" OnClick="Button4_Click" />
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                                <div class="lbBody" id="gridDiv" style="color: White; margin-top: 10px; min-height: 430px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <%--<div id="gridDiv" id="order_details_grid" style="height:400px; max-height:400px; max-width:1290px; overflow:auto; box-shadow:0 2px 4px rgba(127,127,127,.3);box-shadow:inset 0 0 3px #387bbe,0 0 9px #387bbe;">--%>
                                    <asp:HiddenField ID="SelectedGridCellIndex" runat="server" Value="-1" />
                                    <fin:CoolGridView ID="sg1" runat="server" Width="100%" Height="430px" OnSelectedIndexChanged="sg1_SelectedIndexChanged"
                                        OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand" Font-Size="11px" CellPadding="1" ForeColor="#333333"
                                        GridLines="Both" Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false"
                                        AllowPaging="true" PageSize="30" OnPageIndexChanging="sg1_PageIndexChanging">

                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                        <EditRowStyle BackColor="#999999" />
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />

                                        <Columns>
                                            <asp:TemplateField HeaderText="O.K." HeaderStyle-Width="40px">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    Ok
                                                <br />
                                                    <asp:CheckBox ID="chkappall" runat="server" onclick="checkAll(this);" />
                                                </HeaderTemplate>
                                                <ItemStyle Width="20px" BackColor="Green" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkapp" runat="server" onclick="meAppcheck(this);" CssClass="ChkBoxClass" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Close" HeaderStyle-Height="40px">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    No
                                                   <br />
                                                    <asp:CheckBox ID="chkrejall" runat="server" onclick="checkAll(this);" Style="display: none" />
                                                </HeaderTemplate>
                                                <ItemStyle BackColor="Red" />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkrej" runat="server" onclick="meRejcheck(this);" CssClass="ChkBoxClass" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed on" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcompdt" TextMode="Date" runat="server" Width="140px" CssClass="textboxStyle"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnvw" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" CommandName="Show" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Info" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnv1" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" CommandName="btnv1" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="v2" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnv2" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" CommandName="btnv2" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="v3" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnv3" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" CommandName="btnv3" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                    <asp:Label ID="lblSeprt" runat="server"></asp:Label>
                                                    <asp:ImageButton ID="btnv4" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" CommandName="btnv4" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtreason" runat="server" Width="100%" CssClass="textboxStyle"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="fstr" DataField="fstr" />
                                            <asp:BoundField HeaderText="sg1_f1" DataField="sg1_f1" />
                                            <asp:BoundField HeaderText="sg1_f2" DataField="sg1_f2" />
                                            <asp:BoundField HeaderText="sg1_f3" DataField="sg1_f3" />
                                            <asp:BoundField HeaderText="sg1_f4" DataField="sg1_f4" />
                                            <asp:BoundField HeaderText="sg1_f5" DataField="sg1_f5" />
                                            <asp:BoundField HeaderText="sg1_f6" DataField="sg1_f6" />
                                            <asp:BoundField HeaderText="sg1_f7" DataField="sg1_f7" />
                                            <asp:BoundField HeaderText="sg1_f8" DataField="sg1_f8" />
                                            <asp:BoundField HeaderText="sg1_f9" DataField="sg1_f9" />
                                            <asp:BoundField HeaderText="sg1_f10" DataField="sg1_f10" />
                                            <asp:BoundField HeaderText="sg1_f11" DataField="sg1_f11" />
                                            <asp:BoundField HeaderText="sg1_f12" DataField="sg1_f12" />
                                            <asp:BoundField HeaderText="sg1_f13" DataField="sg1_f13" />
                                            <asp:BoundField HeaderText="sg1_f14" DataField="sg1_f14" />
                                            <asp:BoundField HeaderText="sg1_f15" DataField="sg1_f15" />
                                            <asp:BoundField HeaderText="sg1_f16" DataField="sg1_f16" />
                                            <asp:BoundField HeaderText="sg1_f17" DataField="sg1_f17" />
                                            <asp:BoundField HeaderText="sg1_f18" DataField="sg1_f18" />
                                            <asp:BoundField HeaderText="sg1_f19" DataField="sg1_f19" />
                                            <asp:BoundField HeaderText="sg1_f20" DataField="sg1_f20" />
                                            <asp:BoundField HeaderText="sg1_f21" DataField="sg1_f21" />
                                            <asp:BoundField HeaderText="sg1_f22" DataField="sg1_f22" />
                                            <asp:BoundField HeaderText="sg1_f23" DataField="sg1_f23" />
                                            <asp:BoundField HeaderText="sg1_f24" DataField="sg1_f24" />
                                            <asp:BoundField HeaderText="sg1_f25" DataField="sg1_f25" />
                                            <asp:BoundField HeaderText="sg1_f26" DataField="sg1_f26" />
                                            <asp:BoundField HeaderText="sg1_f27" DataField="sg1_f27" />
                                            <asp:BoundField HeaderText="sg1_f28" DataField="sg1_f28" />
                                            <asp:BoundField HeaderText="sg1_f29" DataField="sg1_f29" />
                                            <asp:BoundField HeaderText="sg1_f30" DataField="sg1_f30" />
                                            <asp:BoundField HeaderText="sg1_f31" DataField="sg1_f31" />
                                            <asp:BoundField HeaderText="sg1_f32" DataField="sg1_f32" />
                                            <asp:BoundField HeaderText="sg1_f33" DataField="sg1_f33" />
                                            <asp:BoundField HeaderText="sg1_f34" DataField="sg1_f34" />
                                            <asp:BoundField HeaderText="sg1_f35" DataField="sg1_f35" />
                                            <asp:BoundField HeaderText="sg1_f36" DataField="sg1_f36" />
                                            <asp:BoundField HeaderText="sg1_f37" DataField="sg1_f37" />
                                            <asp:BoundField HeaderText="sg1_f38" DataField="sg1_f38" />
                                            <asp:BoundField HeaderText="sg1_f39" DataField="sg1_f39" />
                                            <asp:BoundField HeaderText="sg1_f40" DataField="sg1_f40" />
                                            <asp:BoundField HeaderText="sg1_f41" DataField="sg1_f41" />
                                            <asp:BoundField HeaderText="sg1_f42" DataField="sg1_f42" />
                                            <asp:BoundField HeaderText="sg1_f43" DataField="sg1_f43" />
                                            <asp:BoundField HeaderText="sg1_f44" DataField="sg1_f44" />
                                            <asp:BoundField HeaderText="sg1_f45" DataField="sg1_f45" />
                                            <asp:BoundField HeaderText="sg1_f46" DataField="sg1_f46" />
                                            <asp:BoundField HeaderText="sg1_f47" DataField="sg1_f47" />
                                            <asp:BoundField HeaderText="sg1_f48" DataField="sg1_f48" />
                                            <asp:BoundField HeaderText="sg1_f49" DataField="sg1_f49" />
                                            <asp:BoundField HeaderText="sg1_f50" DataField="sg1_f50" />

                                        </Columns>
                                        <EmptyDataRowStyle BackColor="White" ForeColor="Red" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <EmptyDataTemplate>
                                            <asp:Image ID="imgdata" runat="server" ImageUrl="~/images/DataNotFound.jpg" AlternateText="No Data Exist" Width="400px" />
                                        </EmptyDataTemplate>
                                    </fin:CoolGridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <asp:HiddenField ID="hfqry" runat="server" />

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />

    <asp:Label ID="lblStatus" runat="server"></asp:Label>
    <asp:HiddenField ID="lbllink" runat="server" />
    <asp:HiddenField ID="hfClickEvent" runat="server" />

</asp:Content>
