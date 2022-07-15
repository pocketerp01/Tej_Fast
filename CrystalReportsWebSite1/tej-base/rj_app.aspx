<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="rj_app" Title="Tejaxo" CodeFile="rj_app.aspx.cs" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="Scripts/jquery-ui.min.js"></script>
    <script src="Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //gridviewScroll('#ContentPlaceHolder1_sg1', gridDiv, 1, 8);
            //calculateSum();
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: (gridDiv.offsetWidth).toFixed(),
                height: (gridDiv.offsetHeight - 10).toFixed(),
                headerrowcount: headerFreeze,
                freezesize: rowFreeze,
                barhovercolor: "#959a9e",
                barcolor: "#3399FF"
            });
        }

        function CheckAll(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=sg1.ClientID %>");
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length;
            for (var i = 0; i < gridCount; i++) {
                document.getElementById('ContentPlaceHolder1_sg1_chkapp_' + i).checked = Checkbox.checked;
            }
        }

    <%--    function CheckAll(gvExample, colIndex) {
            var GridView = document.getElementById("<%=sg1.ClientID %>");
            for (var i = 1; i < GridView.rows.length; i++) {
                var chb = GridView.rows[i].cells[colIndex].getElementsByTagName("input")[0];
                chb.checked = gvExample.checked;
            }
        }--%>

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


    </script>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            
            
        });
    function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
        $(gridId).gridviewScroll({
            width: gridDiv.offsetWidth,
            height: gridDiv.offsetHeight,
            headerrowcount: headerFreeze,
            freezesize: rowFreeze,
            barhovercolor: "#959a9e",
            barcolor: "#959a9e",
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

    </script>--%>
    <style type="text/css">
        .grd {
            margin-bottom: 100px;
            margin-left: 1000px;
            margin-right: 50px;
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
                    <td>

                        <button id="btnnew" runat="server" class="btn btn-info" onserverclick="btnnew_ServerClick" accesskey="t" style="width: 100px;">S<u>t</u>art</button>
                        <button id="btnsave" runat="server" class="btn btn-info" onserverclick="btnsave_ServerClick" accesskey="S" style="width: 100px;"><u>S</u>ave</button>
                        <asp:Button ID="btnext" runat="server" Text="Exit" class="btn btn-info" OnClick="btnext_Click" Style="width: 100px;" />
                    </td>
                    <td style="float: right">

                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Task Managment"></asp:Label>
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
                                            <asp:TextBox ID="txtsearch" runat="server" TabIndex="1" CssClass="txtsrch" onblur="Change(this, event)" onfocus="Change(this, event)"
                                                placeholder="Enter here to search" ToolTip="Enter here to search" Height="35px" Width="250px"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="srch" runat="server" ImageUrl="~/tej-base/images/search-button.png"
                                                Width="120px" Height="31px" TabIndex="2" ToolTip="Click to Search"
                                                OnClick="srch_Click" /></td>


                                        <td style="float: right">Show Rows</td>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="tkrow" runat="server" Width="56px" CssClass="txtcss2" onkeypress="return isDecimalKey(event)"
                                                Style="text-align: right;" Height="30px" Text="20" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <div class="lbBody" id="gridDiv" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                    <%--<div id="gridDiv" id="order_details_grid" style="height:400px; max-height:400px; max-width:1290px; overflow:auto; box-shadow:0 2px 4px rgba(127,127,127,.3);box-shadow:inset 0 0 3px #387bbe,0 0 9px #387bbe;">--%>
                                    <fin:CoolGridView ID="sg1" runat="server" Width="100%" Height="400px" AutoGenerateColumns="false"
                                        OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand"
                                        GridLines="Both" Style="background-color: #b8e3f1; color: #000; font-family: 'Segoe UI'" Font-Size="11px">
                                        <RowStyle BackColor="White" CssClass="GridviewScrollItem2" />
                                        <HeaderStyle Height="28px" Font-Size="11px" BackColor="#b8e3f1" ForeColor="#000" />

                                        <Columns>
                                            <%--  <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt" runat="server" Visible="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="O.K.">
                                                <HeaderStyle Width="40px" />
                                                <ItemStyle Width="40px" />
                                                <HeaderTemplate>
                                                    Ok                                                
                                                    <%--                                                    <asp:CheckBox ID="chkappall" runat="server" onclick="CheckAll(this,1);" />--%>
                                                    <input id="Checkbox2" type="checkbox" onclick="CheckAll(this);" runat="server" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkapp" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Close" HeaderStyle-Height="40px">
                                                <ItemStyle Width="40px" />
                                                <HeaderStyle Width="40px" />
                                                <HeaderTemplate>
                                                    No                                                    
                                                    <asp:CheckBox ID="chkrejall" runat="server" Style="display: none" />
                                                </HeaderTemplate>
                                                <ItemStyle />
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkrej" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Completed on">
                                                <ItemStyle Width="90px" />
                                                <HeaderStyle Width="40px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtcompdt" TextMode="Date" runat="server" Width="140px" CssClass="textboxStyle" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
                                                    <%--   <cc1:CalendarExtender ID="CalendarExtender1" runat="server" 
                Enabled="True" TargetControlID="txtcompdt" 
                Format="dd/MM/yyyy">
            </cc1:CalendarExtender>
           <cc1:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999" 
                            MaskType="Date" TargetControlID="txtcompdt" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnvw" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" CommandName="Show" ImageAlign="Middle" Width="20px" ToolTip="Show" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="v1" HeaderStyle-Width="40px">
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
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks">
                                                <HeaderStyle Width="200px" />
                                                <ItemStyle Width="200px" />
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtreason" runat="server" Width="200px" CssClass="textboxStyle" onblur="Change(this, event)" onfocus="Change(this, event)"></asp:TextBox>
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
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
</asp:Content>
