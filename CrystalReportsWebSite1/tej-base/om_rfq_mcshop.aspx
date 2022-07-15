<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_rfq_mcshop" CodeFile="om_rfq_mcshop.aspx.cs" %>

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
                    <td style="text-align: right">
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
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Entry No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-3">
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
                                <asp:Label ID="Label19" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">RFQ_No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnrfq" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnrfq_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRfqNo" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label20" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">RFQ_Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRfqDate" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">SF Code</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnChild" runat="server" ToolTip="Select RFQ" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnChild_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtChildCode" CssClass="form-control" runat="server" MaxLength="8" ReadOnly="true" Width="100%" />
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtChildName" CssClass="form-control" runat="server" ReadOnly="true" Width="100%" />
                                    <asp:TextBox ID="txtParentChild" CssClass="form-control" runat="server" ReadOnly="true" Width="100%" Visible="false" />
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1" style="visibility: hidden;">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtFstr" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtFstr2" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    <asp:TextBox ID="txtTest" runat="server" Width="100%" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Component_Name</asp:Label>
                                <div class="col-sm-1" style="visibility: hidden">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtIcode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtIname" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Component_Part_No</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtCpart" runat="server" MaxLength="30" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Component_Drg_No</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtDrg" runat="server" CssClass="form-control" ReadOnly="true" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">M/C Wt.</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtM_C_WT" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(event)" MaxLength="8"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">BOP Parts</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Attachments</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="240px" Font-Size="13px"
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

                                                <asp:TemplateField ItemStyle-Width="5px">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Stage" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Stage" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Operation_No" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Machine" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Operation_Description" ItemStyle-Width="180px" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="SetUp_Time" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Unit" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="QtyIssue" Visible="false" />

                                                <asp:TemplateField ItemStyle-Width="15px">
                                                    <HeaderTemplate>SetUp_Time</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" onkeypress="return isDecimalKey(event)" ReadOnly="true" Text='<%#Eval("sg1_t1") %>' onChange="calqty()" Width="100%" MaxLength="7"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>MHR</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" onkeypress="return isDecimalKey(event)" onChange="calqty()" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="4"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>Cost</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" ReadOnly="true" onkeypress="return isDecimalKey(event)" onChange="calqty()" Text='<%#Eval("sg1_t3") %>' Width="100%" MaxLength="9"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px" Visible="false">
                                                    <HeaderTemplate>SetUp</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' ReadOnly="true" Width="100%" MaxLength="100"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Operation_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>SetUp_Time</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>MHR</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Cost</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="7"></asp:TextBox>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <fin:CoolGridView ID="sg2" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Height="240px" Font-Size="13px"
                                        AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound"
                                        OnRowCommand="sg2_RowCommand">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Add</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Del</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />
                                            <asp:TemplateField>
                                                <HeaderTemplate>Part Name</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t1" runat="server" Text='<%#Eval("sg2_t1") %>' Width="100%" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Part No.</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t2" runat="server" Text='<%#Eval("sg2_t2") %>' Width="100%" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Supplier Name</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t3" runat="server" Text='<%#Eval("sg2_t3") %>' Width="100%" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Qty</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t4" runat="server" Text='<%#Eval("sg2_t4") %>' Width="100%" MaxLength="5" onChange="calqty()" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Item Rate</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t5" runat="server" Text='<%#Eval("sg2_t5") %>' Width="100%" MaxLength="5" onChange="calqty()" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Amount</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t8" runat="server" Text='<%#Eval("sg2_t8") %>' Width="100%" MaxLength="5" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Remarks 1</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t6" runat="server" Text='<%#Eval("sg2_t6") %>' Width="100%" MaxLength="100"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Remarks 2</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg2_t7" runat="server" Text='<%#Eval("sg2_t7") %>' Width="100%" MaxLength="100"></asp:TextBox>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" id="gridDiv1" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="240px" Font-Size="13px"
                                            AutoGenerateColumns="false" OnRowDataBound="sg3_RowDataBound"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Attachment" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Download</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btndown" runat="server" CommandName="SG3_DWN" ImageUrl="~/tej-base/images/Save.png" Width="20px" ImageAlign="Middle" ToolTip="Download Attachment" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>View</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnview" runat="server" CommandName="SG3_VIEW" ImageUrl="~/tej-base/images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View Attachment" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Drawing_Type</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' Width="100%" MaxLength="50" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Yes/No</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg3_t2" runat="server" Width="100%">
                                                            <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="YES" Value="YES"></asp:ListItem>
                                                            <asp:ListItem Text="NO" Value="NO"></asp:ListItem>
                                                            <asp:ListItem Text="CONDITIONALLY_APPROVE" Value="CONDITIONALLY_APPROVE"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="cmd1" Value='<%#Eval("sg3_t2") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="sg3_t3" HeaderText="File Name" />
                                                <asp:BoundField DataField="sg3_t4" HeaderText="File Path" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>FileUpload</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:FileUpload ID="FileUpload1" runat="server" EnableViewState="true" onChange="FileUploadCall(this)" ToolTip="Do not Use Special Characters for File Name"/>
                                                        <asp:Button ID="btnUpload" runat="server" CommandName="SG3_UPLD" Text="OK" OnClick="btnUpload_Click" Style="display: none" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t5" runat="server" Text='<%#Eval("sg3_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
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

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Total_Cost</asp:Label>
                                <div class="col-sm-2" style="visibility: hidden;">
                                    <asp:TextBox ID="txttcperc" onkeypress="return isDecimalKey(event)" MaxLength="5" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txttotcost1" ReadOnly="true" MaxLength="14" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Efficiency<span style="font-size:smaller;">%</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txteffperc" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txteff" ReadOnly="true" MaxLength="10" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Rejection <span style="font-size:smaller;">%</span> </asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtrejperc" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtrej" ReadOnly="true" MaxLength="8" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label21" runat="server" Text="lbl7" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True">Tool_Cost</asp:Label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txttoolcost" onkeypress="return isDecimalKey(event)" MaxLength="7" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Overheads<span style="font-size:smaller;">%</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtoverperc" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtover" ReadOnly="true" MaxLength="7" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label16" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Profits<span style="font-size:smaller;">%</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtprofperc" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtprf" ReadOnly="true" MaxLength="7" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">ICC<span style="font-size:smaller;">%</span></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txticcperc" onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="2" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txticc" ReadOnly="true" MaxLength="7" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label18" runat="server" Text="lbl7" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Total_Cost/Part</asp:Label>
                                <div class="col-sm-2" style="visibility: hidden;">
                                    <asp:TextBox ID="TextBox1" onkeypress="return isDecimalKey(event)" MaxLength="3" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txttotcost2" ReadOnly="true" MaxLength="7" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" MaxLength="300" Width="99%" CssClass="form-control" placeholder="Remarks"></asp:TextBox>
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
            var colTot = 0; var gsm8 = 0; var gsm9 = 0; var amt = 0; var totamt = 0; var grandtot = 0;
            var tot; var qty = 0;
            var TotalOutput = 0; var rate = 0;
            var rej = 0; var over = 0; var profit = 0; var icc = 0; var eff = 0; var effperc = 0;
            var grid = $("[id*=sg1].GridviewScrollItem2").length;
            var gridid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid - 1; i++) {
                gsm1 = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value);

                gsm2 = fill_zero(document.getElementById('ContentPlaceHolder1_sg1_sg1_t2_' + i).value);
                gsm3 = gsm1 * gsm2;
                TotalOutput += gsm3;

                document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value = fill_zero(gsm3).toFixed(2);
                document.getElementById('ContentPlaceHolder1_txttotcost1').value = fill_zero(TotalOutput).toFixed(2);// total cost1
                var k = 0;
                k = document.getElementById('ContentPlaceHolder1_txteffperc').value.length;
                if (k >= 1) {
                    gsm5 = document.getElementById('ContentPlaceHolder1_txteffperc').value / 100;
                    effperc = (1 + (1 - gsm5));
                    eff = TotalOutput * effperc;
                    document.getElementById('ContentPlaceHolder1_txteff').value = fill_zero(eff).toFixed(2);//efficiency
                }
                var l = 0;
                l = document.getElementById('ContentPlaceHolder1_txtrejperc').value.length;
                if (l >= 1) {
                    gsm6 = document.getElementById('ContentPlaceHolder1_txtrejperc').value;

                    rej = TotalOutput * gsm6 / 100;
                    document.getElementById('ContentPlaceHolder1_txtrej').value = fill_zero(rej).toFixed(2);//rejection
                }
                var m = 0;
                m = document.getElementById('ContentPlaceHolder1_txtoverperc').value.length;
                if (m >= 1) {
                    gsm7 = document.getElementById('ContentPlaceHolder1_txtoverperc').value;
                    over = TotalOutput * gsm7 / 100;
                    document.getElementById('ContentPlaceHolder1_txtover').value = fill_zero(over).toFixed(2);//over
                }
                var n = 0;
                n = document.getElementById('ContentPlaceHolder1_txtprofperc').value.length;
                if (n >= 1) {
                    gsm8 = document.getElementById('ContentPlaceHolder1_txtprofperc').value;
                    profit = TotalOutput * gsm8 / 100;
                    document.getElementById('ContentPlaceHolder1_txtprf').value = fill_zero(profit).toFixed(2);//profit
                }
                var p = 0;
                p = document.getElementById('ContentPlaceHolder1_txticcperc').value.length;
                if (p >= 1) {
                    gsm9 = document.getElementById('ContentPlaceHolder1_txticcperc').value;
                    icc = TotalOutput * gsm9 / 100;
                    document.getElementById('ContentPlaceHolder1_txticc').value = fill_zero(icc).toFixed(2);//icc
                }
                tot = eff + rej + over + profit + icc;
                //document.getElementById('ContentPlaceHolder1_txttotcost2').value = fill_zero(tot).toFixed(2);//total      
            }
            var grid2 = $("[id*=sg2].GridviewScrollItem2").length;
            var gridid2 = document.getElementById("<%= sg2.ClientID%>");
            for (var i = 0; i < grid2; i++) {
                amt = 0;
                qty = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t4_' + i).value);
                rate = fill_zero(document.getElementById('ContentPlaceHolder1_sg2_sg2_t5_' + i).value);
                amt = qty * rate;
                totamt += amt;
                document.getElementById('ContentPlaceHolder1_sg2_sg2_t8_' + i).value = fill_zero(amt).toFixed(2);
            }
            grandtot = totamt + tot;
            document.getElementById('ContentPlaceHolder1_txttotcost2').value = fill_zero(grandtot).toFixed(2);//total   
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

    <asp:HiddenField ID="TabName" runat="server" />

</asp:Content>
