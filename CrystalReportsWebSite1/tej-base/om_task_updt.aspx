<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_task_updt" CodeFile="om_task_updt.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
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
            function calculateSum() {

                var gridSg3 = document.getElementById("<%= sg3.ClientID%>");
                for (var i = 0; i < gridSg3.rows.length - 1; i++) {
                    var time1 = $("input[id*=sg3_t1]");
                    var time2 = $("input[id*=sg3_t2]");
                    var varTime1 = 0; var varTime2 = 0;
                    if (varTime1.length > 1) {
                        document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = time1[i].value + ":";
                    }
                    if (varTime2.length > 1) {
                        document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = time2[i].value + ":";
                    }
                    varTime1 = time1[i].value;
                    varTime2 = time2[i].value;
                    var t2 = 0;
                    //**************
                    if (varTime1.includes(".")) {
                        var timeIn1 = varTime1.split(".");
                        t2 = timeIn1[1];
                        if ((t2 * 1) > 60) t2 = 59;
                        document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = timeIn1[0] + ":" + t2;
                        if (timeIn1[0].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = "";
                        if (timeIn1[1].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = "";
                        if ((timeIn1[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = "00" + ":" + timeIn1[1];
                        if ((timeIn1[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = timeIn1[0] + ":" + "00";
                        varTime1 = timeIn1[0] * 60 + (t2 * 1);
                    }
                    else if (varTime1.includes(":")) {
                        var timeIn1 = varTime1.split(":");
                        t2 = timeIn1[1];
                        if ((t2 * 1) > 60) t2 = 59;
                        document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = timeIn1[0] + ":" + t2;
                        if (timeIn1[0].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = "";
                        if (timeIn1[1].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = "";
                        if ((timeIn1[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = "00" + ":" + timeIn1[1];
                        if ((timeIn1[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = timeIn1[0] + ":" + "00";
                        varTime1 = timeIn1[0] * 60 + (t2 * 1);
                    }
                    else {
                        varTime1 = fill_zero(varTime1);
                        if ((varTime1 * 1) > 23) {
                            document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value = 0;
                            varTime1 = 0;
                        }
                        varTime1 = varTime1 * 60;
                    }
                    //**************
                    if (varTime2.includes(".")) {
                        var timeIn2 = varTime2.split(".");
                        t2 = timeIn2[1];
                        if ((t2 * 1) > 60) t2 = 59;
                        document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = timeIn2[0] + ":" + t2;
                        if (timeIn2[0].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = "";
                        if (timeIn2[1].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = "";
                        if ((timeIn2[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = "00" + ":" + timeIn2[1];
                        if ((timeIn2[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = timeIn2[0] + ":" + "00";
                        varTime2 = timeIn2[0] * 60 + (t2 * 1);
                    }
                    else if (varTime2.includes(":")) {
                        var timeIn2 = varTime2.split(":");
                        t2 = timeIn2[1];
                        if ((t2 * 1) > 60) t2 = 59;
                        document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = timeIn2[0] + ":" + t2;
                        if (timeIn2[0].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = "";
                        if (timeIn2[1].length > 2) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = "";
                        if ((timeIn2[0] * 1) > 23) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = "00" + ":" + timeIn2[1];
                        if ((timeIn2[1] * 1) > 59) document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = timeIn2[0] + ":" + "00";
                        varTime2 = timeIn2[0] * 60 + (t2 * 1);
                    }
                    else {
                        varTime2 = fill_zero(varTime2);
                        if ((varTime2 * 1) > 23) {
                            document.getElementById('ContentPlaceHolder1_sg3_sg3_t2_' + i).value = 0;
                            varTime2 = 0;
                        }
                        varTime2 = varTime2 * 60;
                    }

                    var n = (((varTime2 - varTime1)).toFixed());
                    var min = n % 60;
                    var hour = (n - min) / 60;
                    if (hour.toString().length < 2) hour = "0" + hour;
                    if (min.toString().length < 2) min = "0" + min;
                    document.getElementById('ContentPlaceHolder1_sg3_sg3_t3_' + i).value = hour + "." + min;
                }
            }
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
                                        <asp:Label ID="lbl101" runat="server" Text="lbl101" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                            Enabled="True" TargetControlID="txtvchdate"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="Maskedit1x" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtvchdate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl104" runat="server" Text="lbl104" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl104" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl104_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl104" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl104a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl107" runat="server" Text="lbl107" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl107" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl107_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl107" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl107a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none;">
                                    <td>Task_Run_No.</td>
                                    <td></td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTaskRunNo" runat="server"></asp:TextBox>
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
                                        <asp:Label ID="lbl102" runat="server" Text="lbl102" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl102" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtlbl102_CalendarExtender" runat="server"
                                            Enabled="True" TargetControlID="txtlbl102"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="txtlbl102_Maskedit1" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtlbl102" />

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl103" runat="server" Text="lbl103" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl103" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl105" runat="server" Text="lbl105" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl105" placeholder="Date" runat="server" Width="100px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtlbl105_CalendarExtender" runat="server"
                                            Enabled="True" TargetControlID="txtlbl105"
                                            Format="dd/MM/yyyy">
                                        </asp:CalendarExtender>
                                        <asp:MaskedEditExtender ID="txtlbl105_Maskedit1" runat="server" Mask="99/99/9999"
                                            MaskType="Date" TargetControlID="txtlbl105" />

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl106" runat="server" Text="lbl106" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl106" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl108" runat="server" Text="lbl108" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl108" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl109" runat="server" Text="lbl109" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl109" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Observation" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtobs" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Marks_Ups" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtups" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Upload_Status
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rad1" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="&nbsp;&nbsp; Yes &nbsp;&nbsp;" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="&nbsp;&nbsp;No&nbsp;&nbsp;" Value="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>QC_Checklist
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rad2" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Text="&nbsp;&nbsp; Yes &nbsp;&nbsp;" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="&nbsp;&nbsp;No&nbsp;&nbsp;" Value="No"></asp:ListItem>
                                        </asp:RadioButtonList>
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
                                        <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtvchnum1" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtvchdate1" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl4a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl7a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl10" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl10a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl13" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl13a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right; display: none" OnClick="btnlbl16_Click" /></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl16" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl16a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl110" runat="server" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl110" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" /></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl110" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>

                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtlbl110a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl111" runat="server" Text="Milestone" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl111" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" /></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl111" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl111a" runat="server" Width="350px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label4" runat="server" Text="Milestone_Status" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtMilestoneStatus" runat="server" Width="432px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label5" runat="server" Text="Activity" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtActivity" runat="server" Width="432px" ReadOnly="true"></asp:TextBox>
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
                                        <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl3" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl6a" runat="server" Text="To_Give" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl6a" runat="server" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="10"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl6" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl9a" runat="server" Text="Actual_Hrs" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl9a" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl9" runat="server"></asp:TextBox>
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
                                        <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl2" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl5" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl8" runat="server"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl11" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl12" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl14" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl15" runat="server"></asp:TextBox>

                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:Label ID="lbl17A" runat="server" Text="Task_Type" CssClass="col-sm-2 control-label"></asp:Label>
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtlbl17" runat="server"></asp:TextBox>

                                    </td>
                                    <td style="display: none">
                                        <asp:Label ID="lbl18A" runat="server" Text="Task_Priority" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td style="display: none">

                                        <asp:TextBox ID="txtlbl18" runat="server"></asp:TextBox>

                                    </td>
                                </tr>

                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl112" runat="server" Text="lbl112" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl112" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl113" runat="server" Text="lbl113" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl113" runat="server"></asp:TextBox>

                                    </td>
                                </tr>

                                <tr style="display: none">
                                    <td>
                                        <asp:Label ID="lbl114" runat="server" Text="lbl114" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl114" runat="server"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl115" runat="server" Text="lbl115" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl115" runat="server"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td>Rework_Reason</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtlbl116" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Particulars_Activity</td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TextName2" runat="server" ReadOnly="true" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">

                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Down.Time</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">UDF Data</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>

                            </ul>

                            <div class="tab-content">


                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 100px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand" OnRowDataBound="sg3_RowDataBound">
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
                                                <asp:BoundField DataField="sg3_f1" HeaderText="DT_Code" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="DT_Name" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Downtime_Start</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Downtime_End</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Total_Downtime(24 HR format)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' MaxLength="10" Width="100%" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                                        <asp:maskededitextender id="sg3_mt3" runat="server"
                                                            targetcontrolid="sg3_t3" acceptampm="false" masktype="Time"
                                                            mask="99:99" autocomplete="False" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' MaxLength="50" Width="100%"></asp:TextBox>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False"
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
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
                                                    <HeaderTemplate>Terms</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%"></asp:TextBox>
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


                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="txtlbl40" runat="server" Width="350px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl41" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl42" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl43" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl44" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl45" runat="server" Width="350px"></asp:TextBox>

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
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl46" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl47" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl48" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl49" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl50" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl51" runat="server" Width="350px"></asp:TextBox>
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


                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="TextName1" runat="server" Width="99%" placeholder="Description of Activity "></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server"   MaxLength="150" Width="99%" placeholder="Rework (If Any)"></asp:TextBox>
                        </div>
                    </div>
                </div>

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
