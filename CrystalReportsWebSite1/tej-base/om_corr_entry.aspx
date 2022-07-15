<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_corr_entry" CodeFile="om_corr_entry.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
            gridviewScroll('#<%=sg3.ClientID%>', gridDiv1, 1, 1);
            calTotal();
            calqty();
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
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btnsprint" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnsprint_ServerClick">Sticker</button>
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
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl1" runat="server" Text="BatchNo." CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtvchnum" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-3">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                        </div>
                                        <div class="col-sm-3">
                                            <asp:Button ID="btnchkstage" Text="Check Stage" runat="server" class="bg-green btn-foursquare" Font-Bold="True" Font-Size="10pt" OnClick="btnchkstage_Click" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl4" runat="server" Text="Process" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />

                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4" runat="server" Width="80px" ReadOnly="true" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl4a" runat="server" Width="370px" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl7" runat="server" Text="Shift" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label></td>
                                    <td>
                                        <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" /></td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7" runat="server" Width="80px" ReadOnly="true" MaxLength="2"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl7a" runat="server" Width="370px" ReadOnly="true" MaxLength="15"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <%--<div class="box-body">--%>
                        <div class="box-body">
                            <table style="width: 100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl2" runat="server" Text="Ent_By" Font-Size="14px" Font-Bold="True"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtlbl2" runat="server" Width="150px" ReadOnly="true"></asp:TextBox>
                                    </td>

                                    <td>
                                        <asp:Label ID="lbl3" runat="server" Text="Ent_Dt" Font-Size="14px" Font-Bold="True"></asp:Label>

                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtlbl3" runat="server" Width="169px" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnprdnrep" Text="Prodn Rep" runat="server" class="bg-green btn-foursquare" Font-Bold="True" Font-Size="10pt" OnClick="btnprdnrep_Click" />
                                    </td>
                                    <%--                                    
                                    <td>
                                        <div class="col-sm-4">
                                            <asp:TextBox ID="txtlbl3" runat="server"></asp:TextBox>
                                        </div>
                                    </td>--%>
                                </tr>
                                <tr id="trCorr1" runat="server">
                                    <td>
                                        <asp:Label ID="lbl5" runat="server" Text="GSM" Font-Size="14px" Font-Bold="True"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl5" runat="server" Width="150px" MaxLength="9"></asp:TextBox>

                                    </td>
                                    <td>
                                        <asp:Label ID="lbl6" runat="server" Text="Trim(KG)" Font-Size="14px" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txtlbl6" runat="server" Width="169px" MaxLength="9"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr id="trCorr2" runat="server">
                                    <td>
                                        <asp:Label ID="lbl8" runat="server" Text="Fala" Font-Size="14px" Font-Bold="True"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl8" runat="server" Width="150px" MaxLength="9"></asp:TextBox>                                       
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl9" runat="server" Text="Core" Font-Size="14px" Font-Bold="True"></asp:Label></td>
                                    <td>

                                        <asp:TextBox ID="txtlbl9" runat="server" Width="80px" MaxLength="9" ReadOnly="True"></asp:TextBox>

                                        <asp:TextBox ID="txtCoreCal" runat="server" Width="63px" placeholder="Entr Core" MaxLength="9"></asp:TextBox>

                                    </td>
                                    <td>

                                        <asp:Button ID="btnCal" Text="Cal" runat="server" Width="40px" class="bg-green" OnClick="btnCal_Click" />

                                    </td>
                                </tr>
                                <tr id="trPoly1" runat="server">
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text="ByProduct:Ink" Font-Size="12px" Font-Bold="True"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtByProdInk" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label21" runat="server" Text="ByProduct:Thinner" Font-Size="12px" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtByProdThin" runat="server" Width="169px" MaxLength="9"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trPoly2" runat="server">
                                    <td>
                                        <asp:Label ID="Label22" runat="server" Text="Rcyl_Scrap" Font-Size="12px" Font-Bold="True"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtRcylScrap" runat="server" Width="150px" MaxLength="9"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label23" runat="server" Text="N/Rcyl_Scrap" Font-Size="12px" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNonRcylScrap" runat="server" Width="169px" MaxLength="9"></asp:TextBox>
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
                                <li><a href="#DescTabMain" id="tabMain" runat="server" aria-controls="DescTabMain" role="tab" data-toggle="tab">Details</a></li>
                                <li><a href="#DescTab1" id="tab1" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Input</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Output</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Rejection Data</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Down Time Data</a></li>
                                <%--<li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Others</a></li>
                                  <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Others1</a></li>--%>
                                <li style="padding-left: 50px;">
                                    <asp:Label ID="Label15" runat="server" Text="Total Input" Font-Size="12px" Font-Bold="True" Height="28px"></asp:Label>
                                    <asp:TextBox ID="txtTotInput" runat="server" Height="27px" MaxLength="6" ReadOnly="true" Style="text-align: right" Width="80px"></asp:TextBox>
                                    <%--<asp:TextBox id="txtTotInput" runat="server" maxlength="6"  />--%>
                                    <asp:Label ID="Label16" runat="server" Text="TotalOutput" Font-Size="12px" Font-Bold="True" Height="28px"></asp:Label>
                                    <asp:TextBox ID="txtTotOutput" runat="server" Height="27px" MaxLength="6" ReadOnly="true" Style="text-align: right" Width="80px"></asp:TextBox>
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label17" runat="server" Text="Curr_Stk" ForeColor="Red" Font-Size="11px" Font-Bold="True"></asp:Label>
                                </li>
                                <li>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lblStkval" runat="server" Text="Stk_val" ForeColor="Red" Font-Size="11px" Font-Bold="True"></asp:Label>
                                </li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTabMain">
                                    <div class="lbBody" style="height: 370px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label1" runat="server" Text="Machine" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl40" runat="server" Height="28px" Width="100px" MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtlbl40a" runat="server" Height="28px" MaxLength="50" ReadOnly="true" Width="338px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label13" runat="server" Text="TeamLeader" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton8" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl41" runat="server" Height="28px"  Width="100px" MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-7">
                                                        <asp:TextBox ID="txtlbl41a" runat="server" Height="28px" MaxLength="20" ReadOnly="true" Width="338px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label2" runat="server" Text="PlanNo." CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl42" runat="server" Height="28px" Width="101px" MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label3" runat="server" Text="PlanDt." CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-5">
                                                        <asp:TextBox ID="txtlbl49" runat="server" Height="28px" MaxLength="6" ReadOnly="true" Width="231px"></asp:TextBox>
                                                    </div>

                                                    <asp:Label ID="Label4" runat="server" Text="JobNo." CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" Visible="false" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl43" runat="server" Height="28px" Width="101px" MaxLength="6" ReadOnly="true"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label14" runat="server" Text="JobDt." CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-5">
                                                        <asp:TextBox ID="txtlbl14" runat="server" Height="28px" MaxLength="6" ReadOnly="true" Width="231px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label18" runat="server" Text="Number of Operator" CssClass="col-sm-3 control-label" Font-Size="13px" Height="28px"></asp:Label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtNumOfOperator" runat="server" Height="26px" MaxLength="3" Width="80px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label19" runat="server" Text="No_of_Helper" CssClass="col-sm-2 control-label" Font-Size="13px" Height="28px"></asp:Label>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtNumOfHelper" runat="server" Height="26px" MaxLength="3" Width="80px"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <asp:Label ID="Label9" runat="server" Text="ItemName" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-10">
                                                        <asp:TextBox ID="txtitemname" runat="server" Height="28px" MaxLength="10" ReadOnly="true" Width="480px"></asp:TextBox>
                                                        <asp:TextBox ID="txtJobCardWt" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                                    </div>

                                                    <asp:Label ID="Label8" runat="server" Text="ItemCode" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>


                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl47" runat="server" Height="28px" MaxLength="10" ReadOnly="true" Width="100px"></asp:TextBox>
                                                    </div>

                                                    <asp:Label ID="Label10" runat="server" Text="PSize" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>

                                                    <div class="col-sm-6">
                                                        <asp:TextBox ID="txtlbl48" runat="server" Height="28px" MaxLength="7" onkeypress="return isDecimalKey(event)" onpaste="return false" ReadOnly="true" Width="100px"></asp:TextBox>
                                                    </div>

                                                    <asp:Label ID="Label5" runat="server" Text="Opr1" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" />
                                                    </div>

                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl44" runat="server" Height="28px" ReadOnly="true" Width="100px"></asp:TextBox>
                                                    </div>

                                                    <asp:Label ID="Label6" runat="server" Text="Opr2" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl45" runat="server" Height="28px" ReadOnly="true" Width="100px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label7" runat="server" Text="Opr3" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-1">
                                                        <asp:ImageButton ID="ImageButton7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" />
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl46" runat="server" Height="28px" ReadOnly="true" Width="80px"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl44a" runat="server" Height="28px" MaxLength="30"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl45a" runat="server" Height="28px" MaxLength="30"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl46a" runat="server" Height="28px" MaxLength="30" Width="100px"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="Label11" runat="server" Text="StartTime" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    <div class="col-sm-2">
                                                        <asp:TextBox ID="txtlbl50" runat="server" Height="28px" MaxLength="8" TextMode="Time" Width="100px"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Label ID="Label12" runat="server" Text="EndTime" Font-Size="14px" Font-Bold="True" Height="28px"></asp:Label>
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <asp:TextBox ID="txtlbl51" runat="server" Height="28px" MaxLength="8" TextMode="Time" Width="100px"></asp:TextBox>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <asp:Button ID="btnShowMaterial" Text="Show Material" runat="server" class="bg-green btn-foursquare" Font-Bold="True" Font-Size="10pt" OnClick="btnShowMaterial_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12">
                                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 370px; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <%-- <div class="col-md-12">--%>
                                        <div class="lbBody" id="gridDiv" style="color: White; height: 370px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                            <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                                Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px"
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

                                                    <asp:BoundField DataField="sg1_srno" HeaderText="Srno" />
                                                    <asp:BoundField DataField="sg1_f1" HeaderText="Stg" />
                                                    <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                    <asp:BoundField DataField="sg1_f3" HeaderText="Unit" />
                                                    <asp:BoundField DataField="sg1_f4" HeaderText="ItemCode" ItemStyle-Width="80px" />
                                                    <asp:BoundField DataField="sg1_f5" HeaderText="InputName" ItemStyle-Width="370px" />
                                                    <asp:BoundField DataField="sg1_f6" HeaderText="No.ofPkg" />
                                                    <%--  <asp:BoundField DataField="sg1_f7" HeaderText="Qty" />--%>


                                                    <asp:TemplateField ItemStyle-Width="80px">
                                                        <HeaderTemplate>Qty</HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="sg1_f7" runat="server" Text='<%#Eval("sg1_f7") %>' Style="text-align: right" onpaste="return false" onKeyUp="calTotal();" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="sg1_f8" HeaderText="sg1_f8" />
                                                    <asp:BoundField DataField="sg1_f9" HeaderText="Post" />
                                                    <%--
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                    <%-- <asp:TemplateField>
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
                                                    <%--  <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Style="text-align: right" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                    <ItemTemplate>

                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="120px" TextMode="Date"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%"></asp:TextBox>
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
                                                </asp:TemplateField>--%>
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
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <%--  <div class="col-md-12">--%>
                                    <div class="lbBody" id="gridDiv1" style="height: 370px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="Smaller"
                                            AutoGenerateColumns="False"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemStyle Width="30px" />
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemStyle Width="30px" />
                                                    <HeaderStyle Width="30px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg3_srno" HeaderText="Sr.No" ItemStyle-Width="30px" HeaderStyle-Width="30px" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="ItemCode" ItemStyle-Width="80px" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Item_Name" ItemStyle-Width="370px" />
                                                <%-- <asp:BoundField DataField="sg3_f3" HeaderText="Qty" />--%>
                                                <%--  <asp:BoundField DataField="sg3_f5" HeaderText="Total" />--%>

                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>Qty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="7" Width="100%" onKeyUp="calqty()" onkeypress="return isDecimalKey(event)" onpaste="return false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_f4" HeaderText="Pkt" ItemStyle-Width="50px" HeaderStyle-Width="50px" />
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>Total</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_f5" runat="server" Text='<%#Eval("sg3_f5") %>' MaxLength="7" Width="100%" onkeypress="return isDecimalKey(event)" onpaste="return false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_f6" HeaderText="Batch/LotNo." />
                                                <%-- <asp:TemplateField>
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
                                                </asp:TemplateField>--%>
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


                                <%--        </div>--%>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 370px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Rejection" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Rejection" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />
                                                <asp:BoundField DataField="sg2_f1" HeaderText="Specification" />
                                                <asp:BoundField DataField="sg2_f2" HeaderText="RejnCode" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>RejnQty</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" onpaste="return false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Conditions</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" Visible="false" Text='<%#Eval("sg2_t2") %>' Width="100%"></asp:TextBox>
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
                                    <div class="lbBody" style="height: 370px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnadd" runat="server" CommandName="SG4_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert DownTime" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg4_btnrmv" runat="server" CommandName="SG4_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove DownTime" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg4_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg4_f1" HeaderText="D/tDescr" />
                                                <asp:BoundField DataField="sg4_f2" HeaderText="D/tCode" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>D/t(Mins)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' MaxLength="10" Width="100%" ReadOnly="true"></asp:TextBox>
                                                        <%--  <asp:MaskedEditExtender ID="Maskedit2" runat="server" Mask="99:99:99"
                                                            MaskType="Time" TargetControlID="sg4_t1" MessageValidatorTip="true" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>D/t(From)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t2" runat="server" Text='<%#Eval("sg4_t2") %>' MaxLength="10" TextMode="Time" Width="100%" onkeyup="mytime()"></asp:TextBox>
                                                        <%--  <asp:MaskedEditExtender ID="Maskedit3" runat="server" Mask="99:99:99"
                                                            MaskType="Time" TargetControlID="sg4_t2" MessageValidatorTip="true" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>D/t(To)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t3" runat="server" Text='<%#Eval("sg4_t3") %>' MaxLength="10" Width="100%" TextMode="Time" onkeyup="mytime()"></asp:TextBox>
                                                        <%--  <asp:MaskedEditExtender ID="Maskedit4" runat="server" Mask="99:99:99"
                                                            MaskType="Time" TargetControlID="sg4_t3" MessageValidatorTip="true" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Why?</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t4" runat="server" Text='<%#Eval("sg4_t4") %>' MaxLength="15" Width="100%" onkeyup="mytime()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>CntrMeasure</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t5" runat="server" Text='<%#Eval("sg4_t5") %>' MaxLength="15" Width="100%"></asp:TextBox>
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
                                <%--<div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <%--<div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl40" runat="server" Text="lbl40" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl40" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl41" runat="server" Text="lbl41" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl41" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl42" runat="server" Text="lbl42" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl42" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl43" runat="server" Text="lbl43" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl43" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl44" runat="server" Text="lbl44" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl44" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl45" runat="server" Text="lbl45" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl45" runat="server" Width="370px"></asp:TextBox>
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
                                                                <asp:Label ID="lbl46" runat="server" Text="lbl46" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl46" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl47" runat="server" Text="lbl47" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl47" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl48" runat="server" Text="lbl48" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl48" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl49" runat="server" Text="lbl49" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl49" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl50" runat="server" Text="lbl50" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl50" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl51" runat="server" Text="lbl51" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <div class="col-sm-4">
                                                                    <asp:TextBox ID="txtlbl51" runat="server" Width="370px"></asp:TextBox>
                                                                </div>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>--%>
                            </div>
                            <%--<div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <%--<div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="GridView1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound"

                                            OnRowCommand="GridView1_RowCommand" >
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
                                    </div>--%>
                            <%-- </div>--%>
                        </div>
                    </div>
                    <!--</div>-->
                </section>

                <div>
                    <asp:Label ID="lblinput" runat="server" Font-Bold="True" Font-Size="small" Visible="false"></asp:Label>
                </div>
                <div>
                    <asp:Label ID="lblitem" runat="server" Style="text-align: right" Font-Size="small" Visible="false"></asp:Label>
                </div>
            </div>
        </section>
        <%-- </div>--%>

        <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
        <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
        <asp:HiddenField ID="hffield" runat="server" />
        <asp:HiddenField ID="doc_nf" runat="server" />
        <asp:HiddenField ID="doc_df" runat="server" />
        <asp:HiddenField ID="doc_vty" runat="server" />
        <asp:HiddenField ID="doc_addl" runat="server" />
        <asp:HiddenField ID="edmode" runat="server" />
        <asp:HiddenField ID="hf1" runat="server" />
        <asp:HiddenField ID="hf2" runat="server" />

        <asp:HiddenField ID="hfGridView1SV" runat="server" />
        <asp:HiddenField ID="hfGridView1SH" runat="server" />
        <script type="text/javascript">
            $(function () {
                var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTabMain";
                $('#Tabs a[href="#' + tabName + '"]').tab('show');
                $("#Tabs a").click(function () {
                    $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
                });
            });
        </script>

        <script>

            function txtChange(txt) {
                debugger;
                alert("hello");
                var grid = document.getElementById('ContentPlaceHolder1_sg3');
                for (var i = 0; i < grid.rows.length; i++) {
                    var qty = $("input[id*=sg1_t1]");
                    alert(qty[i].val);
                    var tot = qty[i].val * grid.rows[i].cells[5];
                    alert(tot);
                    grid.rows[i].cells[6].val = tot;




                    //        var i, CellValue, Row;
                    //        i = parseInt(rowindex) + 1;

                    //        var table = document.getElementById('ContentPlaceHolder1_sg3');

                    //Row = table.rows[i].cells[0];

                    //CellValue = Row.innerHTML;

                    //Row.children[0].hidden = true;

                    //alert(CellValue);

                }
            }
        </script>
        <script>
            function calqty() {
                var rowTot = 0;
                var colTot = 0;
                var tot;
                var TotalOutput = 0;
                var grid = document.getElementById("<%= sg3.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    colTot = fill_zero(document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value) * 1;
                    TotalOutput += fill_zero(document.getElementById('ContentPlaceHolder1_sg3_sg3_t1_' + i).value) * 1;
                    tot = grid.rows[i + 1].cells[6].innerText;
                    rowTot = colTot * tot;
                    document.getElementById('ContentPlaceHolder1_sg3_sg3_f5_' + i).value = fill_zero(rowTot);
                }
                document.getElementById('ContentPlaceHolder1_txtTotOutput').value = fill_zero(TotalOutput);
            }
            function fill_zero(val) {
                if (isNaN(val)) return 0; if (isFinite(val)) return val;
            }

        </script>
        <script>
            function calrejection() {
                var rowTot = 0;
                var colTot = 0;
                var grid = document.getElementById("<%= sg1.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    colTot = 0;
                    // loop for 31 col
                    // col total is 31 columns total
                    for (var k = 8; k < 17; k++) {
                        colTot += fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t' + (k) + '_' + i).value) * 1;
                    }
                    // row total is total of total_qty field row wise
                    rowTot += colTot;
                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t5_' + i).value = fill_zero(colTot);
                }

            }
            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        </script>

        <script>
            function mytime() {
                var timeIn = 0, timeOut = 0;
                var grid = document.getElementById("<%= sg4.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    timeOut = document.getElementById("ContentPlaceHolder1_sg4_sg4_t3_" + i).value;
                    timeIn = document.getElementById("ContentPlaceHolder1_sg4_sg4_t2_" + i).value;


                    if (timeIn.includes(":")) {
                        var timeIn1 = timeIn.split(":");
                        var t2 = timeIn1[1];
                        if ((t2 * 1) > 60) t2 = 59;
                        timeIn = timeIn1[0] * 60 + (t2 * 1);
                    }

                    if (timeOut.includes(":")) {
                        var timeOut1 = timeOut.split(":");
                        var to2 = timeOut1[1];
                        if ((to2 * 1) > 60) to2 = 59;
                        timeOut = timeOut1[0] * 60 + (to2 * 1);
                    }


                    var n = (((timeOut - timeIn)).toFixed());
                    var min = n % 60;
                    var hour = (n - min) / 60;
                    if (hour.toString().length < 2) hour = hour * 60;
                    if (min.toString().length < 2) min = min;

                    document.getElementById("ContentPlaceHolder1_sg4_sg4_t1_" + i).value = fill_zero(hour) + fill_zero(min);
                }

            }
            function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        </script>

        <script>
            function calTotal() {
                var TotInput = 0;
                var grid = document.getElementById("<%= sg1.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    TotInput += fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_f7_' + i).value) * 1;
                }
                document.getElementById('ContentPlaceHolder1_txtTotInput').value = fill_zero(TotInput);
            }
            function fill_zero(val) {
                if (isNaN(val)) return 0; if (isFinite(val)) return val;
            }

        </script>

        <asp:HiddenField ID="TabName" runat="server" />
    </div>
</asp:Content>
