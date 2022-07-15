<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_final_qtn" CodeFile="om_final_qtn.aspx.cs" %>
<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
           <%-- gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);--%>

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
                            <div class="form-group">
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    <asp:TextBox ID="txtgrade" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label19" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">RFQ_No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRfqno" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label20" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">RFQ_Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtRfqdate" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <asp:Label ID="Label4" runat="server" Text="lbl7" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Work_Order_Line_Item_No</asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtFstr" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtFstr2" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1" style="visibility: hidden;">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtaname" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Item_name</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txticode" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtiname" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                           
                            <div class="form-group">
                                 <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True" Visible="false">Material</asp:Label>
                                <div class="col-sm-10" style="display:none">
                                    <asp:TextBox ID="txtmatl" runat="server" Width="100%" CssClass="form-control" MaxLength="80"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                             <div class="form-group">
                                <asp:Label ID="Label21" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Part_No</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtpartno" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                </div>                                
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Pymt_Terms</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtpymtterm" runat="server" MaxLength="15" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <asp:Label ID="Label11" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Foundry_Tooling_Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtfoundcost" runat="server" CssClass="form-control" ReadOnly="true" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="12"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label14" runat="server" Text="lbl2" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Machine_Tooling_Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtmchcost" runat="server" ReadOnly="true" onkeypress="return isDecimalKey(event)" MaxLength="9" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display:none">
                                <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Total_Tooling_Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttoolcost" runat="server" ReadOnly="true" onkeypress="return isDecimalKey(event)" MaxLength="9" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Casting_Price(INR/Kg)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcastprice" runat="server" MaxLength="5" onkeypress="return isDecimalKey(event)" onChange="calqty()" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>


                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" Visible="false">Machining_Price</asp:Label>
                                <div class="col-sm-3" style="display:none">
                                    <asp:TextBox ID="txtmchprice" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="5"></asp:TextBox>
                                </div>
                                 
                                <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Heat_Treatmnt(Rs.12/Kg)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtheattmt" runat="server" CssClass="form-control" onChange="calCost()" onkeypress="return isDecimalKey(event)" MaxLength="5" Width="100%"></asp:TextBox>
                                </div>
                               <asp:Label ID="Label25" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Assembly_Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtasembcost" runat="server" CssClass="form-control" onChange="calCost()" onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="5"></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group"  style="display:none">
                                <asp:Label ID="Label24" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">BOP</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtbop" runat="server" CssClass="form-control" Width="100%" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="5" ReadOnly="true"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label23" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Packaging</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpack" runat="server" CssClass="form-control" Width="100%" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="5"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Painting_Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpaintcost" runat="server" CssClass="form-control" onChange="calCost()" Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="7"></asp:TextBox>
                                </div>
                                  <asp:Label ID="Label16" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Final_Component_Cost</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtFinalCompCost" runat="server" CssClass="form-control" ReadOnly="true" onkeypress="return isDecimalKey(event)" MaxLength="8" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group"  style="display:none">
                                <asp:Label ID="Label26" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Forwarding(FLS_Binola)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtforwrd" runat="server" Width="100%" onChange="calqty()" CssClass="form-control" onkeypress="return isDecimalKey(event)" MaxLength="5"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label27" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Component_Cost(INR)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcomp" runat="server" CssClass="form-control" onChange="calqty()" ReadOnly="true" onkeypress="return isDecimalKey(event)" MaxLength="8"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">SF Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Attachments</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <fin:CoolGridView ID="sg2" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Height="240px" Font-Size="13px"
                                        AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField visible="false">
                                                <HeaderTemplate>Add</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField visible="false">
                                                <HeaderTemplate>Del</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />
                                            <asp:TemplateField>
                                                <HeaderTemplate>SF_Code</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%"  ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Part No.</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%"  ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Part Name</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t3" runat="server" Text='<%#Eval("sg2_t3") %>' Width="100%"  ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Casting_Wt.</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t4" runat="server" Text='<%#Eval("sg2_t4") %>' Width="100%" MaxLength="5" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Quantity</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t16" runat="server" Text='<%#Eval("sg2_t16") %>' Width="100%" MaxLength="5" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Tot_Casting_Wt.</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t17" runat="server" Text='<%#Eval("sg2_t17") %>' Width="100%" MaxLength="5" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Material</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t5" runat="server" Text='<%#Eval("sg2_t5") %>' Width="100%" MaxLength="80"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Foundry_Tooling</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t6" runat="server" Text='<%#Eval("sg2_t6") %>' Width="100%"  onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Machining_Tooling</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t7" runat="server" Text='<%#Eval("sg2_t7") %>' Width="100%"  onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>Tooling_Cost(INR)</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t8" runat="server" Text='<%#Eval("sg2_t8") %>' Width="100%"  onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>Casting_Price</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t9" runat="server" Text='<%#Eval("sg2_t9") %>' Width="100%" onchange="calCost()"  onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>Mac_Price/Comp(INR)</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t10" runat="server" Text='<%#Eval("sg2_t10") %>' Width="100%" onchange="calCost()" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>BOP</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t11" runat="server" Text='<%#Eval("sg2_t11") %>' Width="100%" onchange="calCost()" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>Freight_Cost</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t12" runat="server" Text='<%#Eval("sg2_t12") %>' Width="100%" onchange="calCost()" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>Packaging</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t13" runat="server" Text='<%#Eval("sg2_t13") %>' Width="100%" onchange="calCost()" MaxLength="10" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Other_Charges</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t15" runat="server" Text='<%#Eval("sg2_t15") %>' Width="100%"  onchange="calCost()" MaxLength="7" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>Component_Cost(INR)</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t14" runat="server" Text='<%#Eval("sg2_t14") %>' Width="100%" onchange="calCost()" MaxLength="7" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </fin:CoolGridView>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 240px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="lbl103" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">RM_Base</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmbase" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group" style="display:none">
                                                        <asp:Label ID="lbl104" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Casting_Weight</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtcastwght" onkeypress="return isDecimalKey(event)" runat="server" CssClass="form-control" MaxLength="7" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label12" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Quote_Validity</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtquoteval" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label13" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Delivery_Terms</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtdelterm" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label8" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks1</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk1" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label5" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks2</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk2" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label9" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks3</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk3" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label10" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks4</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk4" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="lbl12" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks5</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk5" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label6" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks6</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk6" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks7</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk7" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label28" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks8</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk8" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label29" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks9</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk9" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <asp:Label ID="Label30" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks10</asp:Label>
                                                        <div class="col-sm-9">
                                                            <asp:TextBox ID="txtrmk10" runat="server" MaxLength="100" ReadOnly="false" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="270px" Font-Size="13px"
                                            AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Attachment" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Download</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btndown" runat="server" CommandName="SG1_DWN" ImageUrl="~/tej-base/images/Save.png" Width="20px" ImageAlign="Middle" ToolTip="Download Attachment" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>View</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnview" runat="server" CommandName="SG1_VIEW" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Drawing_Type</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Yes/No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg1_t2" runat="server" Width="100%">
                                                            <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                            <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                                            <asp:ListItem Text="CONDITIONALLY_APPROVE" Value="CONDITIONALLY_APPROVE"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="cmd1" Value='<%#Eval("sg1_t2") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg1_t6" HeaderText="RFQ Type" />
                                                <asp:BoundField DataField="sg1_t3" HeaderText="File Name" />
                                                <asp:BoundField DataField="sg1_t4" HeaderText="File Path" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>FileUpload</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:FileUpload ID="FileUpload1" runat="server" EnableViewState="true" onChange="FileUploadCall(this)" />
                                                        <asp:Button ID="btnUpload" runat="server" CommandName="SG1_UPLD" Text="OK" OnClick="btnUpload_Click" Style="display: none" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <div class="col-md-12" style="display:none">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="300" Width="99%" CssClass="form-control" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
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

    <script>
        function calqty() {
            debugger;
            var rowTot = 0; var gsm1 = 0; var gsm2 = 0; var gsm3 = 0; var gsm4 = 0; var gsm5 = 0; var gsm6 = 0; var gsm7 = 0;
            var colTot = 0; var gsm8 = 0; var gsm9 = 0;
            var tot;
            var TotalOutput = 0;
            var rej = 0; var over = 0; var profit = 0; var icc = 0; var eff = 0; var effperc = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            //  for (var i = 0; i < grid.rows.length - 1; i++) {
            // gsm1 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_txtmchcost' + i).value));                
            //gsm2 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_txtfoundcost' + i).value));
            //TotalOutput = gsm1 + gsm2;
            //document.getElementById('ContentPlaceHolder1_txttoolcost').value = fill_zero(TotalOutput).toFixed(0);// total TOOLING cost1
            //  gsm3 = fill_zero(Number(document.getElementById('ContentPlaceHolder1_' + i).value))
            gsm1 = document.getElementById('ContentPlaceHolder1_txtcastprice').value * 1;
            gsm2 = document.getElementById('ContentPlaceHolder1_txtmchprice').value * 1;
            gsm3 = document.getElementById('ContentPlaceHolder1_txtheattmt').value * 1;
            gsm4 = document.getElementById('ContentPlaceHolder1_txtbop').value * 1;

            gsm5 = document.getElementById('ContentPlaceHolder1_txtpack').value * 1;
            gsm6 = document.getElementById('ContentPlaceHolder1_txtasembcost').value * 1;
            gsm7 = document.getElementById('ContentPlaceHolder1_txtpaintcost').value * 1;
            gsm8 = document.getElementById('ContentPlaceHolder1_txtforwrd').value * 1;

            gsm9 += gsm1 + gsm2 + gsm3 + gsm4 + gsm5 + gsm6 + gsm7 + gsm8;

            document.getElementById('ContentPlaceHolder1_txtcomp').value = fill_zero(gsm9).toFixed(2);
            //txtcomp.Text =  

            //  }           
        }
        function FileUploadCall(fileUpload) {
            if (fileUpload.value != '') {
                var a = $(fileUpload).next("[id*='btnUpload']");
                a.click();
            }
        }
        function fill_zero(val) {
            if (isNaN(val)) return 0; if (isFinite(val)) return val;
        }

   </script>

    <script >
        function calCost() {
            var casting = 0; var machining = 0; var bop = 0; var freight = 0; var packaging = 0; var other_chg = 0; var componentcost = 0;
            var heat = 0; var assembly = 0; var painting = 0; var finalcost = 0; var total = 0; var bomqty = 0;
            var grid = $("[id*=sg2].GridviewScrollItem2").length;
            var gridid = document.getElementById("<%= sg2.ClientID%>");
            for (var i = 0; i < grid; i++) {
                componentcost = 0;
                casting = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t9_' + i).value);
                machining = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t10_' + i).value);
                bop = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t11_' + i).value);
                freight = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t12_' + i).value);
                packaging = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t13_' + i).value);
                other_chg = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t15_' + i).value);
                bomqty = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t16_' + i).value);
                componentcost = (casting * 1 + machining * 1 + bop * 1 + freight * 1 + packaging * 1 + other_chg * 1) * bomqty * 1;
                total += componentcost;
                document.getElementById('ContentPlaceHolder1_sg2_sg2_t14_' + i).value = fill_zero(componentcost).toFixed(2);
            }
            heat = fill_zero(document.getElementById('ContentPlaceHolder1_txtheattmt').value * 1);
            assembly = fill_zero(document.getElementById('ContentPlaceHolder1_txtasembcost').value * 1);
            painting = fill_zero(document.getElementById('ContentPlaceHolder1_txtpaintcost').value * 1);
            finalcost = heat * 1 + assembly * 1 + painting * 1 + total * 1;
            document.getElementById('ContentPlaceHolder1_txtFinalCompCost').value = fill_zero(finalcost).toFixed(2);
        }
    </script>
  
    <asp:HiddenField ID="TabName" runat="server" />

</asp:Content>