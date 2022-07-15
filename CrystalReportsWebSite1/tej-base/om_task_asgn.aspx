﻿<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_task_asgn" CodeFile="om_task_asgn.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
            calculateSum();
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
            function calculateSum() {
                var h1 = document.getElementById('ContentPlaceHolder1_txtlbl6').value;
                var h2 = document.getElementById('ContentPlaceHolder1_txtlbl9a').value;
                document.getElementById('ContentPlaceHolder1_txtlbl12a').value = fill_zero(h1 * 1) - fill_zero(h2 * 1);

                var dd1 = document.getElementById('ContentPlaceHolder1_ddTType');
                var sVal = dd1.options[dd1.selectedIndex].value;
                if (sVal == "Rework") {
                    document.getElementById("ContentPlaceHolder1_divRwk").style.visibility = "visible";
                }
                else {
                    document.getElementById("ContentPlaceHolder1_divRwk").style.visibility = "hidden";
                }
            }

<%--            function calculateSum($element) {

                var toth1 = 0;
                var toth2 = 0;
                var toth3 = 0;
                var goth_tot = 0;
                var row_bas = 0;
                var row_pack = 0;
                var row_frt = 0;
                var row_amtz = 0;
                var row_ed = 0;

                var row_tax = 0;
                var row_atax = 0;

                var row_tax_rt = 0;
                var row_atax_rt = 0;

                var grid_bas = 0;
                var grid_pack = 0;
                var grid_frt = 0;
                var grid_ed = 0;

                var grid_tax = 0;
                var grid_atax = 0;

                var grid_tot = 0;
                var addl_Tx_Mthd = 0;
                var gridSg1 = document.getElementById("<%= sg1.ClientID%>");
                addl_Tx_Mthd = document.getElementById('ContentPlaceHolder1_doc_addl').value

                for (var i = 0; i < gridSg1.rows.length - 1; i++) {

                    var qty = $("input[id*=sg1_t3]");
                    var rate = $("input[id*=sg1_t4]");
                    var disc_per = $("input[id*=sg1_t5]");
                    var disc_val = 0;

                    var pack_perc = $("input[id*=sg1_t6]");
                    var pack_peru = $("input[id*=sg1_t7]");
                    var frt_peru = $("input[id*=sg1_t8]");

                    var tool_Amtz = $("input[id*=sg1_t14]");

                    var ed_per = $("input[id*=sg1_t9]");
                    var ed_val = 0;
                    var tax_per = $("input[id*=sg1_t11]");
                    var atax_per = $("input[id*=sg1_t16]");

                    row_tax_rt = fill_zero(tax_per[i].value);
                    row_atax_rt = fill_zero(atax_per[i].value);

                    row_bas = fill_zero(qty[i].value) * (fill_zero(rate[i].value) * ((100 - fill_zero(disc_per[i].value)) / 100));
                    row_amtz = fill_zero(qty[i].value) * (fill_zero(tool_Amtz[i].value));

                    if (pack_perc[i].value > 0) {
                        row_pack = row_bas * ((fill_zero(pack_perc[i].value)) / 100);
                    }

                    else {
                        row_pack = fill_zero(qty[i].value) * (fill_zero(pack_peru[i].value));
                    }

                    row_frt = fill_zero(qty[i].value) * (fill_zero(frt_peru[i].value));

                    row_ed = (row_bas + row_pack + row_amtz) * ((fill_zero(ed_per[i].value)) / 100);

                    row_tax = ((row_bas + row_pack + row_ed) * ((fill_zero(row_tax_rt)) / 100));

                    if (addl_Tx_Mthd === "1") {
                        row_atax = ((row_tax) * ((fill_zero(row_atax_rt)) / 100));
                    }
                    else {
                        row_atax = ((row_bas + row_pack + row_ed) * ((fill_zero(row_atax_rt)) / 100));
                    }


                    grid_bas = grid_bas + row_bas;
                    grid_pack = grid_pack + row_pack;
                    grid_ed = grid_ed + row_ed;
                    grid_frt = grid_frt + row_frt;
                    grid_tax = grid_tax + row_tax;
                    grid_atax = grid_atax + row_atax;

                }

                document.getElementById('ContentPlaceHolder1_txtlbl20b').value = fill_zero(grid_frt).toFixed(3);

                toth1 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl20b').value);
                toth2 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl21b').value);
                toth3 = fill_zero(document.getElementById('ContentPlaceHolder1_txtlbl22b').value);

                goth_tot = fill_zero(toth1 * 1) + fill_zero(toth2 * 1) + fill_zero(toth3 * 1);
                grid_tot = fill_zero(grid_bas * 1) + fill_zero(grid_pack * 1) + fill_zero(grid_ed * 1) + fill_zero(grid_tax * 1) + fill_zero(grid_atax * 1) + fill_zero(goth_tot * 1);

                document.getElementById('ContentPlaceHolder1_txtlbl24').value = fill_zero(grid_bas).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl25').value = fill_zero(grid_pack).toFixed(3);

                document.getElementById('ContentPlaceHolder1_txtlbl26').value = fill_zero(grid_ed).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl28').value = fill_zero(grid_tax).toFixed(3);

                document.getElementById('ContentPlaceHolder1_txtlbl29').value = fill_zero(grid_atax).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl30').value = fill_zero(goth_tot).toFixed(3);

                document.getElementById('ContentPlaceHolder1_txtlbl31').value = fill_zero(grid_tot).toFixed(3);
            }--%>
        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td><asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align:right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>

                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td style="padding-right: 4px;">
                                        <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                            Enabled="True" TargetControlID="txtvchdate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtvchdate" />
                                    </td>

                                    <td>
                                        <asp:RadioButtonList ID="rdintOff" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="&nbsp; Internal &nbsp;&nbsp;&nbsp;" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="&nbsp; Offload" Value="1"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td style="padding-right: 4px;">
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtlbl4a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtlbl7a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl10" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtlbl10a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl52" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right; display: none" OnClick="btnlbl10_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl52" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtlbl52a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl13" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtlbl13a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl16" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtlbl16a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lblassineecode" runat="server" Text="Assign_To" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnassineecode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnassineecode_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtassineecode" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtassineeName" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Milestone" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnMileStone" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnMileStone_Click" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMileStoneCode" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtMileStone" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>



                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Milestone_Status" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtMilestoneStatus" runat="server" Width="432px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl2" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl3" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr style="border-top: 1px groove; border-left: 1px groove; border-right: 1px groove;">
                                    <td>
                                        <asp:Label ID="lbl6" runat="server" Text="Tot_Hrs" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl6" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl6a" runat="server" Text="To_Give" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl6a" runat="server" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="border-left: 1px groove; border-right: 1px groove;">
                                    <td>
                                        <asp:Label ID="lbl9" runat="server" Text="Assign_Hrs" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl9" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl9a" runat="server" Text="Actual_Hrs" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl9a" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="border-bottom: 1px groove; border-left: 1px groove; border-right: 1px groove;">
                                    <td>
                                        <asp:Label ID="lbl12" runat="server" Text="Diff" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl12" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl12a" runat="server" Text="Diff" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl12a" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl8" placeholder="Date" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender1" runat="server"
                                            Enabled="True" TargetControlID="txtlbl8"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="Maskededitextender1" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtlbl8" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl11" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl5" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRework" runat="server" Text="Task_Type" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddTType" runat="server" onchange="calculateSum();">
                                            <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                            <asp:ListItem Text="Rework" Value="Rework"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl15" runat="server" ReadOnly="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDlbl14" runat="server">
                                            <asp:ListItem Text="-Select-" Value="-Select-"></asp:ListItem>
                                            <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                            <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="I.A._Ch" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl14" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPspE" runat="server" Text="PSP_Element" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPsp" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Close_Project" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlbl5" runat="server">
                                            <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                            <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl17" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl18" runat="server"></asp:TextBox>
                                    </td>
                                </tr>

                            </table>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:TextBox ID="TextName1" runat="server" Width="99%" placeholder="Generic Activity " Style="display: none"></asp:TextBox>
                                Activity : &nbsp;
                                    <asp:DropDownList ID="ddActivity" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddActivity_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            Particulars of Activity : 
                            <asp:TextBox ID="TextName2" runat="server" Width="99%" placeholder="Particulars of Activity " ToolTip="Particulars of Activity"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-12" id="divRwk" runat="server">
                    <div>
                        <div class="box-body">
                            Rework (If Any) : 
                            <asp:TextBox ID="txtrmk" runat="server"   MaxLength="150" Width="99%" placeholder="Rework (If Any)" ToolTip="Rework (If Any)"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <section class="col-lg-12 connectedSortable" style="visibility: hidden">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Milestone</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>

                            </ul>

                            <div class="tab-content">


                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnadd" runat="server" CommandName="SG4_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnrmv" runat="server" CommandName="SG4_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg4_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Field</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' Width="100%" MaxLength="40"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 200px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1500px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1" />
                                                <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2" />
                                                <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3" />
                                                <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4" />
                                                <asp:BoundField DataField="sg1_h5" HeaderText="sg1_h5" />
                                                <asp:BoundField DataField="sg1_h6" HeaderText="sg1_h6" />
                                                <asp:BoundField DataField="sg1_h7" HeaderText="sg1_h7" />
                                                <asp:BoundField DataField="sg1_h8" HeaderText="sg1_h8" />
                                                <asp:BoundField DataField="sg1_h9" HeaderText="sg1_h9" />
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="sg1_srno" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dt</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndt" runat="server" CommandName="SG1_ROW_DT" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Date" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <%--                                                        <asp:TemplateField>
                                                            <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="Maskedit2" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" TargetControlID="sg1_t2" />
                                                                <asp:CalendarExtender ID="txtvchdate_CalendarExtender2" runat="server"
                                                                    Enabled="True" TargetControlID="sg1_t2"
                                                                    Format="dd/MM/yyyy">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Tcode</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btntax" runat="server" CommandName="SG1_ROW_TAX" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Choose Tax" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Milestone</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Hours Completed on Milestone</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%" TextMode="Number" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Milestone Status</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t3" runat="server" Text='<%#Eval("sg2_t3") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ERP_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Dlv_Date</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Sch.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Prod.Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Job_Card</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="1" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
