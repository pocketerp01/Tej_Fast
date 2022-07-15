<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_pre_cost_SPPI" CodeFile="om_pre_cost_SPPI.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    
                    <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnrefresh" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnrefresh_ServerClick">Refresh</button>
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="16px"><span><b>(Trim Wastage Part)</b></span></asp:Label></td>
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
                                    <asp:Label ID="lbl1a" runat="server" Text="LC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label18" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtvchdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label19" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Customer_Name</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnparty" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnparty_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtacode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="6"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtaname" runat="server" CssClass="form-control" Width="100%" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Item_Name</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnicode" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnicode_Click" />
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txticode" ReadOnly="true" runat="server" CssClass="form-control" Width="100%" MaxLength="8"></asp:TextBox>
                                </div>
                                <div class="col-sm-7">
                                    <asp:TextBox ID="txtiname" runat="server" CssClass="form-control" Width="100%" MaxLength="150"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Structure</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtStructure" runat="server" BackColor="LightBlue" autocomplete="off" MaxLength="30" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label4" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Print_Type_(Roto/Flexo)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPrintType" runat="server" BackColor="LightBlue" autocomplete="off" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Order_Qty<span style="font-size:x-small;">(kg)</span> </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtOrder" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" MaxLength="7" CssClass="form-control" Width="100%" Style="text-align: right"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label12" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">CYL Amortized Qty</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtCYL" runat="server" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" autocomplete="off" onkeyup="cal()" MaxLength="7" CssClass="form-control" Width="100%" Style="text-align: right"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label54" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">No._of_Colours</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtColour" runat="server" BackColor="LightBlue" CssClass="form-control" Width="100%" onkeypress="return isDecimalKey(event)" Style="text-align: right" MaxLength="7"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label81" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">LPO_Number/Pro.Inv.</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtLPO" runat="server" BackColor="LightBlue" autocomplete="off" CssClass="form-control" Width="100%" MaxLength="30"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Raw Material</a></li>
                                <li><a href="#DescTab1" id="tab2" runat="server" aria-controls="DescTab1" role="tab" data-toggle="tab">Conversion Cost</a></li>
                                <li><a href="#DescTab2" id="tab3" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Total Cost / Piece</a></li>
                                <li><a href="#DescTab3" id="tab4" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Cylinder</a></li>
                                <li><a href="#DescTab4" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Flap</a></li>
                                <li><a href="#DescTab5" id="tab6" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Bag/Pouch</a></li>
                                <li><a href="#DescTab6" id="tab7" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Packing Details</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 450px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div>
                                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="450px" Font-Size="13px"
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
                                                                    <HeaderTemplate>sg1_t1</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeydown="openBox(this);" onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t2</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t3</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" onkeypress="return isDecimalKey(event)" onkeyup="cal()"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t4</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" onkeypress="return isDecimalKey(event)" onkeyup="cal()"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t5</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" onkeypress="return isDecimalKey(event)" onkeyup="cal()"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t6</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" onkeyup="cal()" Width="100%"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t7</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="cal()"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t8</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="cal()"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="cal()"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>sg1_t10</HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="cal()"></asp:TextBox>
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


                                                <div class="box-body" style="display: none">
                                                    <div class="form-group">
                                                        <label id="Label61" runat="server" class="col-sm-2 control-label">Raw Materials</label>
                                                        <label id="Label15" runat="server" class="col-sm-1 control-label">Thickness</label>
                                                        <label id="Label16" runat="server" class="col-sm-1 control-label">Density</label>
                                                        <label id="Label22" runat="server" class="col-sm-1 control-label">GSM</label>
                                                        <label id="Label23" runat="server" class="col-sm-1 control-label">RM Mix %</label>
                                                        <label id="Label26" runat="server" class="col-sm-2 control-label">RM Unit Price (USD)</label>
                                                        <label id="Label27" runat="server" class="col-sm-2 control-label">RM Unit Price (AED)</label>
                                                        <label id="Label28" runat="server" class="col-sm-1 control-label">Cost/Kg(USD)</label>
                                                        <label id="Label32" runat="server" class="col-sm-1 control-label">Cost/Kg(AED)</label>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label127" runat="server" class="col-sm-2 control-label">Pet</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPetThick" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPetDensity" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPetGSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPetRM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPetUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPetAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPetKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPetKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label33" runat="server" class="col-sm-2 control-label">Met Pet</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMetThick" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMetDensity" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMetGSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMetRM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtMetUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtMetAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMetKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMetKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label34" runat="server" class="col-sm-2 control-label">LDPE Transparent</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLPDEThick" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLPDEDensity" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLPDEGSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLPDERM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtLPDEUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtLPDEAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLPDEKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLPDEKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label35" runat="server" class="col-sm-2 control-label">Ink</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtInkThick" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtInkDensity" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtInkGSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtInkRM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtInkUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtInkAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtInkKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtInkKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label36" runat="server" class="col-sm-2 control-label">Adhesive 1 (S.L)</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh1Thick" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh1Density" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh1GSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh1RM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAdh1USD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAdh1AED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh1KgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh1KgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label37" runat="server" class="col-sm-2 control-label">Adhesive 2 (S.L)</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh2Thick" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh2Density" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh2GSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh2RM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAdh2USD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAdh2AED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh2KgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAdh2KgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label38" runat="server" class="col-sm-2 control-label">Total</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtTotGSM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtTotRM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtTotKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtTotKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label39" runat="server" class="col-sm-2 control-label">Wastage</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtWastageRM" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtWastageKGUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtWastageKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label40" runat="server" class="col-sm-2 control-label">Solvent</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <label id="Label123" runat="server" class="col-sm-2 control-label">Meter in Kgs</label>
                                                        <label id="Label125" runat="server" class="col-sm-2 control-label">Price</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtSolventKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtSolventKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label41" runat="server" class="col-sm-2 control-label">Zipper</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtZipperUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtZipperAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtZipperKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtZipperKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label42" runat="server" class="col-sm-2 control-label">Packing - Glue</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPackingUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPackingAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackingKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackingKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label43" runat="server" class="col-sm-2 control-label">Packing - Pet - Strip</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPackUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPackAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right" Visible="false"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label44" runat="server" class="col-sm-2 control-label">Packing - CTN</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackCTN" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label45" runat="server" class="col-sm-2 control-label">Packing - Bobbin & Others</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackBobbin1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPackBobbin2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label60" runat="server" class="col-sm-2 control-label">Total RM Cost/Kg</label>
                                                        <div class="col-sm-8" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox142" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtTotRMKgUSD" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtTotRMKgAED" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab1">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label3" runat="server" class="col-sm-2 control-label">Machine Cost</label>
                                                        <label id="Label6" runat="server" class="col-sm-2 control-label">Standard Cost (Per Hour)</label>
                                                        <label id="Label7" runat="server" class="col-sm-1 control-label">Hour</label>
                                                        <label id="Label8" runat="server" class="col-sm-1 control-label">Total Cost</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label9" runat="server" class="col-sm-2 control-label">Machine(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMachine1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMachine2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label21" runat="server" class="col-sm-2 control-label">Extrusion (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnExtrusionAED" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnExtrusionAED_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvExtCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvExtHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvExtTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label14" runat="server" class="col-sm-2 control-label">Power(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPower1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtPower2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label13" runat="server" class="col-sm-2 control-label">Printing-Roto (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnPrintingROTOAed" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPrintingROTOAed_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinRotoCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinRotoHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinRotoTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label20" runat="server" class="col-sm-2 control-label">Fuel_Charge(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtFuel1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtFuel2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label17" runat="server" class="col-sm-2 control-label">Printing-BOBST (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnPrintBobstAED" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPrintBobstAED_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinBobstCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinBobstHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinBobstTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label25" runat="server" class="col-sm-2 control-label">Labour_Cost(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLabourCost1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtLabourCost2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label24" runat="server" class="col-sm-2 control-label">Printing-CI (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnPrintCIAed" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPrintCIAed_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinCICost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinCIHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPrinCITot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label30" runat="server" class="col-sm-2 control-label">Freight(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtFreight1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtFreight2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label29" runat="server" class="col-sm-2 control-label">Lamination (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnLaminationAED" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnLaminationAED_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvLamCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvLamHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvLamTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox127" runat="server" CssClass="form-control" Width="100%" MaxLength="7"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label31" runat="server" class="col-sm-2 control-label">Slitting (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnConvSlittingCost" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnConvSlittingCost_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvSlittingCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvSlittingHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvSlittingTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox128" runat="server" CssClass="form-control" Width="100%" MaxLength="7"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label48" runat="server" class="col-sm-2 control-label">Pouching (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnConvPouchingCost" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnConvPouchingCost_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPouchingCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPouchingHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvPouchingTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label49" runat="server" class="col-sm-2 control-label">Total_Conversion_Cost(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvTotCostKg" runat="server" CssClass="form-control" Width="100%" MaxLength="7"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label50" runat="server" class="col-sm-2 control-label">Bag-Chicken (AED/HR)</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnBagChicken" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnBagChicken_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvBagChickenCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvBagChickenHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvBagChickenTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox129" runat="server" CssClass="form-control" Width="100%" MaxLength="7"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label52" runat="server" class="col-sm-2 control-label">Bag General</label>
                                                        <div class="col-sm-1">
                                                            <asp:ImageButton ID="btnConvBagGen" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnConvBagGen_Click" />
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvBagGeneralCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvBagGeneralHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvBagGeneralTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-6" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox130" runat="server" CssClass="form-control" Width="100%" MaxLength="7"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label55" runat="server" class="col-sm-2 control-label">Total_Cost(In AED)</label>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label56" runat="server" class="col-sm-2 control-label">Tot_Prod_Cost_Excluding_Mgmt&Finance_Cost</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMgmtFin1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMgmtFin2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label46" runat="server" class="col-sm-2 control-label">Machine_Cost/Kg</label>
                                                        <div class="col-sm-3">
                                                        </div>

                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMach1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label47" runat="server" class="col-sm-2 control-label">Management_Cost(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMgmtCost1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtMgmtCost2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label51" runat="server" class="col-sm-2 control-label">Fuel (AED/HRS)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtConvFuelCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvFuelHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvFuelTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <label id="Label53" runat="server" class="col-sm-2 control-label">Finance_Cost(Per_Kg)</label>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtFin1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtFin2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label59" runat="server" class="col-sm-2 control-label">Total Cost/Kg</label>
                                                        <div class="col-sm-8" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox141" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvTotKg1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtConvTotKg2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label62" runat="server" class="col-sm-2 control-label">Labour Cost</label>
                                                        <label id="Label63" runat="server" class="col-sm-2 control-label">Standard Cost (Per Hour)</label>
                                                        <label id="Label64" runat="server" class="col-sm-2 control-label">Hour</label>
                                                        <label id="Label65" runat="server" class="col-sm-2 control-label">Total Cost</label>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox147" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label74" runat="server" class="col-sm-2 control-label">Extrusion (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtExtCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtExtHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtExtTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label id="Label75" runat="server" class="col-sm-2 control-label">Per_Pc_Price(AED)</label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPerPcPrice" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label91" runat="server" class="col-sm-2 control-label">Printing-Roto (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinRotoCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinRotoHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinRotoTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label id="Label92" runat="server" class="col-sm-2 control-label">Per_Pc_Price(FILS)</label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPerPcPriceFils" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label93" runat="server" class="col-sm-2 control-label">Printing-BOBST (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinBobstCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinBobstHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinBobstTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label id="Label94" runat="server" class="col-sm-2 control-label">Order_in_Pcs</label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtOrderPcs" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label95" runat="server" class="col-sm-2 control-label">Printing-CI (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinCICost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinCIHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPrinCITot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <label id="Label98" runat="server" class="col-sm-2 control-label">Order_in_Kg</label>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtOrderKg" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label100" runat="server" class="col-sm-2 control-label">Lamination (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtLamCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtLamHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtLamTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox172" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label105" runat="server" class="col-sm-2 control-label">Slitting (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtSlittingCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtSlittingHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtSlittingTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox176" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label106" runat="server" class="col-sm-2 control-label">Pouching (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPouchingCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPouchingHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtPouchingTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox180" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label108" runat="server" class="col-sm-2 control-label">Bag-Chicken (AED/HR)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtBagChickenCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtBagChickenHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtBagChickenTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox185" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label109" runat="server" class="col-sm-2 control-label">Bag General</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtBagGeneralCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtBagGeneralHr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtBagGeneralTot" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox189" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label69" runat="server" class="col-sm-2 control-label">Total Cost (In AED)</label>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtTotalCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox163" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label107" runat="server" class="col-sm-2 control-label">Labour Cost (AED/Kg)</label>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtLabourCost" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-4" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox191" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label111" runat="server" class="col-sm-2 control-label">Description</label>
                                                        <label id="Label112" runat="server" class="col-sm-1 control-label">RMC</label>
                                                        <label id="Label113" runat="server" class="col-sm-1 control-label">GP(Only RM)</label>
                                                        <label id="Label114" runat="server" class="col-sm-1 control-label">GP(RM+Other_Cost)</label>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <label id="Label115" runat="server" class="col-sm-2 control-label">Selling_Price(Per Kg) AED</label>
                                                        <label id="Label116" runat="server" class="col-sm-2 control-label">Selling_Price(Per Kg) USD</label>
                                                        <label id="Label117" runat="server" class="col-sm-2 control-label">RM_Cost(Per Kg)</label>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label110" runat="server" class="col-sm-2 control-label">Based On Amortized Order</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAmortized1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAmortized2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtAmortized3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAmortized4" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAmortized5" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtAmortized6" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label118" runat="server" class="col-sm-2 control-label">Based On Current Order</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCurrent1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCurrent2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCurrent3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-1">
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtCurrent4" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtCurrent5" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtCurrent6" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label119" runat="server" class="col-sm-2 control-label">Remarks</label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label66" runat="server" class="col-sm-3 control-label">Cylinder Cost Actual</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyAct" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label68" runat="server" class="col-sm-3 control-label">Cylinder Cost Paid</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyPaid" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label67" runat="server" class="col-sm-3 control-label">Cylinder Cost (Fils/sq.cm)</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyFills" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label99" runat="server" class="col-sm-3 control-label">Cylinder / Plate Width</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyWidth" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label120" runat="server" class="col-sm-3 control-label">Cylinder / Plate Circumference</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyCircum" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label121" runat="server" class="col-sm-3 control-label">Cylinder Cost Amortize</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyAmor" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-8" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox211" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label124" runat="server" class="col-sm-3 control-label">Cylinder / Plate Supplier</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCySupp" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <div class="col-sm-8" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox212" runat="server" CssClass="form-control" Width="100%" MaxLength="7" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label122" runat="server" class="col-sm-3 control-label">Cylinder Cost On Actual Order Qty</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtCyOrder" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label70" runat="server" class="col-sm-2 control-label">Flap Up Width - mm</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapW" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label71" runat="server" class="col-sm-2 control-label">Flap Length - mm</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapL" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label72" runat="server" class="col-sm-2 control-label">Flap Thickness</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapThickness" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label73" runat="server" class="col-sm-2 control-label">Flap Down Width - mm</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapDown" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label76" runat="server" class="col-sm-2 control-label">Flap Length - mm</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapL2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label77" runat="server" class="col-sm-2 control-label">Flap Thickness</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapThickness2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label78" runat="server" class="col-sm-2 control-label">Flap Up Weight - gms</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapWt" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label126" runat="server" class="col-sm-2 control-label">Flap Down Weight - gms</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtFlapDownWt" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label128" runat="server" class="col-sm-2 control-label">Glue/Zipper Weight-Gms (Fix)</label>
                                                        <div class="col-sm-2">
                                                            <asp:TextBox ID="txtGlue" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label57" runat="server" class="col-sm-3 control-label">Bag/Pouch Piece per Kg</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtBagPieceKg" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label82" runat="server" class="col-sm-3 control-label">Piece Per Mtr</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtBagPieceMtr" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label83" runat="server" class="col-sm-3 control-label">No. of Zipper Mtr in 1 Kg</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtZipper" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label58" runat="server" class="col-sm-3 control-label">Bag/Pouch Width (Mtr)</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtBagWidth" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label79" runat="server" class="col-sm-3 control-label">Bag/Pouch Length (Mtr)</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtBagLength" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label80" runat="server" class="col-sm-3 control-label">Bag/Pouch Weight (GMS)</label>
                                                        <div class="col-sm-1">
                                                            <asp:TextBox ID="txtBagWeight" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab6">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <label id="Label84" runat="server" class="col-sm-3 control-label">Bag Weight</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtPackingWt" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <label id="Label85" runat="server" class="col-sm-2 control-label" style="text-align: center">Price</label>
                                                        <div class="col-sm-1" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox26" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <label id="Label86" runat="server" class="col-sm-2 control-label" style="text-align: center">Pieces Required</label>
                                                        <div class="col-sm-1" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox27" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label87" runat="server" class="col-sm-3 control-label">Packing Mode /Pkt / Roll / Wicket </label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtPackingMode" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label88" runat="server" class="col-sm-3 control-label">Pkt / Weight</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtPackingPkt" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label89" runat="server" class="col-sm-3 control-label">Sticker</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtSticker1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtSticker2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtSticker3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label90" runat="server" class="col-sm-3 control-label">Rod</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtRod1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtRod2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtRod3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label96" runat="server" class="col-sm-3 control-label">Washer</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtWasher1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtWasher2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtWasher3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label97" runat="server" class="col-sm-3 control-label">Others</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtOther1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtOther2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtOther3" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label101" runat="server" class="col-sm-3 control-label">Total</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtPackTotal" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" Width="100%" MaxLength="7" BackColor="LightBlue" onkeypress="return isDecimalKey(event)" Style="text-align: right" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label id="Label102" runat="server" class="col-sm-3 control-label">For 1 Kg</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txt1Kg" runat="server" CssClass="form-control" Width="100%" MaxLength="7" ReadOnly="true" onkeypress="return isDecimalKey(event)" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                        </div>
                                                        <div class="col-sm-3" style="visibility: hidden">
                                                        </div>
                                                    </div>
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
                            <div class="form-group">
                                <asp:Label ID="Label10" runat="server" CssClass="col-sm-12 control-label" Font-Size="14px" Font-Bold="True">NOTE : Blue Textbox for Data Entry Fields , Grey TextBox for Automatic Calculation and Selected value through Popup button</asp:Label>
                            </div>

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
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hf3" runat="server" />
    <asp:HiddenField ID="hf4" runat="server" />
    <asp:HiddenField ID="hf5" runat="server" />

    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });

        function cal() {

            var CyAct = 0, CyAmortize = 0, CyOrder = 0, FlapWeight = 0, FlapDownWeight = 0, BagPieceKg = 0, BagPieceMtr = 0, Zipper = 0, BagWeight = 0, ConvExtTot = 0, ConvPrinRotoTot = 0, ConvPrinBobstTot = 0, ConvPrinCITot = 0, ConvLamTot = 0, ConvSlittingTot = 0, ConvPouchingTot = 0, ConvBagChickenTot = 0, ConvBagGeneralTot = 0, ConvTot = 0, Mach1 = 0, ConvFuelHr = 0, Machine1 = 0, Power1 = 0, Fuel1 = 0, LabourCost1 = 0, Freight1 = 0, ConvTotCostKg = 0, ConvTotKg1 = 0, ConvTotKg2 = 0, ExtTot = 0, PrinRotoTot = 0, PrinBobstTot = 0, PrinCITot = 0, LamTot = 0, SlittingTot = 0, PouchingTot = 0, BagChickenTot = 0, BagGeneralTot = 0, TotalCost = 0, LabourCost = 0, PerPcPrice = 0, PerPcPriceFils = 0, OrderKg = 0, Amortized6 = 0, Current6 = 0, Amortized1 = 0, Amortized2 = 0, Amortized3 = 0, Amortized5 = 0, Current1 = 0, Current2 = 0, Current3 = 0, Current5 = 0, MgmtFin2 = 0;
            var PetGSM = 0, MetGSM = 0, LdpeGSM = 0, InkGSM = 0, Adh1GSM = 0, Adh2GSM = 0, TotGSM = 0, PetRM = 0, MetRM = 0, InkRM = 0, LdpeRM = 0, Adh1RM = 0, Adh2RM = 0, TotRM = 0;
            var PetAED = 0, PetCostUSD = 0, PetCostAED = 0, MetAED = 0, MetCostUSD = 0, MetCostAED = 0, LdpeAED = 0, LdpeCostUSD = 0, LdpeCostAED = 0, InkAED = 0, InkCostUSD = 0, InkCostAED = 0, Adh1AED = 0, Adh1CostUSD = 0, Adh1CostAED = 0, Adh2AED = 0, Adh2CostUSD = 0, Adh2CostAED = 0, TotCostUSD = 0, TotCostAED = 0;
            var SolventKgUSD = 0, WastageKGUSD = 0, WastageKGAED = 0, ZipperKgUSD = 0, ZipperKgAED = 0, PackingKgUSD = 0, PackingKgAED = 0, PackKgUSD = 0, PackKgAED = 0, TotRmKgUSD = 0, TotRmKgAED = 0;
            var PackingBagWt = 0, PackingPkt = 0, Sticker = 0, Rod = 0, Washer = 0, Others = 0, TotPacking = 0, For1Kg = 0;

            CyAct = (fill_zero(document.getElementById("ContentPlaceHolder1_txtCyWidth").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtCyCircum").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtColour").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtCyFills").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtCyAct').value = (CyAct * 1).toFixed(1);
            CyAmortize = ((CyAct * 1) - (fill_zero(document.getElementById("ContentPlaceHolder1_txtCyPaid").value) * 1)) / (fill_zero(document.getElementById("ContentPlaceHolder1_txtCYL").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtCyAmor').value = (CyAmortize * 1).toFixed(5);
            CyOrder = ((CyAct * 1) - (fill_zero(document.getElementById("ContentPlaceHolder1_txtCyPaid").value) * 1)) / (fill_zero(document.getElementById("ContentPlaceHolder1_txtOrder").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtCyOrder').value = (CyOrder * 1).toFixed(2);

            FlapWeight = (fill_zero(document.getElementById("ContentPlaceHolder1_txtFlapW").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtFlapL").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtFlapThickness").value) * 1) * 0.925;
            document.getElementById('ContentPlaceHolder1_txtFlapWt').value = (FlapWeight * 1).toFixed(2);
            FlapDownWeight = (fill_zero(document.getElementById("ContentPlaceHolder1_txtFlapDown").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtFlapL2").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtFlapThickness2").value) * 1) * 0.925;
            document.getElementById('ContentPlaceHolder1_txtFlapDownWt').value = (FlapDownWeight * 1).toFixed(2);

            //PetGSM = (fill_zero(document.getElementById("ContentPlaceHolder1_txtPetThick").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtPetDensity").value) * 1);
            //MetGSM = (fill_zero(document.getElementById("ContentPlaceHolder1_txtMetThick").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtMetDensity").value) * 1);
            //LdpeGSM = (fill_zero(document.getElementById("ContentPlaceHolder1_txtLPDEThick").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtLPDEDensity").value) * 1);
            //InkGSM = fill_zero(document.getElementById("ContentPlaceHolder1_txtInkGSM").value);
            //Adh1GSM = fill_zero(document.getElementById("ContentPlaceHolder1_txtAdh1GSM").value);
            //Adh2GSM = fill_zero(document.getElementById("ContentPlaceHolder1_txtAdh2GSM").value);
            //TotGSM = (PetGSM * 1) + (MetGSM * 1) + (LdpeGSM * 1) + (InkGSM * 1) + (Adh1GSM * 1) + (Adh2GSM * 1);

            //document.getElementById('ContentPlaceHolder1_txtPetGSM').value = (PetGSM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtMetGSM').value = (MetGSM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtLPDEGSM').value = (LdpeGSM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtTotGSM').value = (TotGSM * 1).toFixed(2);            

            //PetRM = ((PetGSM * 1) / (TotGSM * 1)) * 100;
            //MetRM = ((MetGSM * 1) / (TotGSM * 1)) * 100;
            //LdpeRM = ((LdpeGSM * 1) / (TotGSM * 1)) * 100;
            //InkRM = ((InkGSM * 1) / (TotGSM * 1)) * 100;
            //Adh1RM = ((Adh1GSM * 1) / (TotGSM * 1)) * 100;
            //Adh2RM = ((Adh2GSM * 1) / (TotGSM * 1)) * 100;
            //TotRM = (PetRM * 1) + (MetRM * 1) + (LdpeRM * 1) + (InkRM * 1) + (Adh1RM * 1) + (Adh2RM * 1);

            //document.getElementById('ContentPlaceHolder1_txtPetRM').value = (PetRM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtMetRM').value = (MetRM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtLPDERM').value = (LdpeRM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtInkRM').value = (InkRM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh1RM').value = (Adh1RM * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh2RM').value = (Adh2RM * 1).toFixed(2);
            //            document.getElementById('ContentPlaceHolder1_txtTotRM').value = (TotRM * 1).toFixed(2);

            //PetAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtPetUSD").value) * 1) * 3.675;
            //MetAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtMetUSD").value) * 1) * 3.675;
            //LdpeAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtLPDEUSD").value) * 1) * 3.675;
            //InkAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtInkUSD").value) * 1) * 3.675;
            //Adh1AED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtAdh1USD").value) * 1) * 3.675;
            //Adh2AED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtAdh2USD").value) * 1) * 3.675;

            //document.getElementById('ContentPlaceHolder1_txtPetAED').value = (PetAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtMetAED').value = (MetAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtLPDEAED').value = (LdpeAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtInkAED').value = (InkAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh1AED').value = (Adh1AED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh2AED').value = (Adh2AED * 1).toFixed(2);

            //PetCostUSD = ((PetRM * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtPetUSD").value) * 1)) / 100;
            //MetCostUSD = ((MetRM * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtMetUSD").value) * 1)) / 100;
            //LdpeCostUSD = ((LdpeRM * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtLPDEUSD").value) * 1)) / 100;
            //InkCostUSD = ((InkRM * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtInkUSD").value) * 1)) / 100;
            //Adh1CostUSD = ((Adh1RM * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtAdh1USD").value) * 1)) / 100;
            //Adh2CostUSD = ((Adh2RM * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtAdh2USD").value) * 1)) / 100;
            //TotCostUSD = (PetCostUSD * 1) + (MetCostUSD * 1) + (LdpeCostUSD * 1) + (InkCostUSD * 1) + (Adh1CostUSD * 1) + (Adh2CostUSD * 1);

            //document.getElementById('ContentPlaceHolder1_txtPetKgUSD').value = (PetCostUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtMetKgUSD').value = (MetCostUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtLPDEKgUSD').value = (LdpeCostUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtInkKgUSD').value = (InkCostUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh1KgUSD').value = (Adh1CostUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh2KgUSD').value = (Adh2CostUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtTotKgUSD').value = (TotCostUSD * 1).toFixed(2);

            //PetCostAED = (PetCostUSD * 1) * 3.675;
            //MetCostAED = (MetCostUSD * 1) * 3.675;
            //LdpeCostAED = (LdpeCostUSD * 1) * 3.675;
            //InkCostAED = (InkCostUSD * 1) * 3.675;
            //Adh1CostAED = (Adh1CostUSD * 1) * 3.675;
            //Adh2CostAED = (Adh2CostUSD * 1) * 3.675;
            //TotCostAED = (PetCostAED * 1) + (MetCostAED * 1) + (LdpeCostAED * 1) + (InkCostAED * 1) + (Adh1CostAED * 1) + (Adh2CostAED * 1);

            //document.getElementById('ContentPlaceHolder1_txtPetKgAED').value = (PetCostAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtMetKgAED').value = (MetCostAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtLPDEKgAED').value = (LdpeCostAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtInkKgAED').value = (InkCostAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh1KgAED').value = (Adh1CostAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtAdh2KgAED').value = (Adh2CostAED * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtTotKgAED').value = (TotCostAED * 1).toFixed(2);

            //WastageKGUSD = ((TotCostUSD * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtWastageRM").value) * 1)) / 100;
            //document.getElementById('ContentPlaceHolder1_txtWastageKGUSD').value = (WastageKGUSD * 1).toFixed(2);
            //WastageKGAED = (WastageKGUSD * 1) * 3.675;
            //document.getElementById('ContentPlaceHolder1_txtWastageKgAED').value = (WastageKGAED * 1).toFixed(2);
            //SolventKgUSD = (fill_zero(document.getElementById("ContentPlaceHolder1_txtSolventKgAED").value) * 1) / 3.675;
            //document.getElementById('ContentPlaceHolder1_txtSolventKgUSD').value = (SolventKgUSD * 1).toFixed(2);

            var sgf = "ContentPlaceHolder1_sg1";
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length;
            TotGSM = 0;
            for (var i = 0; i < gridCount; i++) {
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;

                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T1") {
                    document.getElementById(sgf + '_sg1_t5_' + i).value = (document.getElementById(sgf + '_sg1_t3_' + i).value * document.getElementById(sgf + '_sg1_t4_' + i).value).toFixed(4);
                }
                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T1" || document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T2") {
                    TotGSM += (document.getElementById(sgf + '_sg1_t5_' + i).value * 1);
                }
            }

            document.getElementById('ContentPlaceHolder1_txtTotGSM').value = TotGSM;

            BagWeight = ((fill_zero(document.getElementById("ContentPlaceHolder1_txtBagWidth").value) * 1) * ((fill_zero(document.getElementById("ContentPlaceHolder1_txtBagLength").value) * 1) * 2) * (TotGSM * 1)) + (FlapWeight * 1) + (FlapDownWeight * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtGlue").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtBagWeight').value = (BagWeight * 1).toFixed(2);
            BagPieceKg = 1000 / (BagWeight * 1);
            document.getElementById('ContentPlaceHolder1_txtBagPieceKg').value = (BagPieceKg * 1).toFixed(2);
            BagPieceMtr = 1 / (fill_zero(document.getElementById("ContentPlaceHolder1_txtBagWidth").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtBagPieceMtr').value = (BagPieceMtr * 1).toFixed(2)
            Zipper = (BagPieceKg * 1) / (BagPieceMtr * 1);
            document.getElementById('ContentPlaceHolder1_txtZipper').value = (Zipper * 1).toFixed(2);

            myGridCal();

            document.getElementById('ContentPlaceHolder1_txtZipperUSD').value = (Zipper * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPackingUSD').value = (Zipper * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPackUSD').value = (Zipper * 1).toFixed(2);

            ZipperKgAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtZipperAED").value) * 1) * (Zipper * 1);
            PackingKgAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtPackingAED").value) * 1) * (Zipper * 1);
            PackKgAED = (fill_zero(document.getElementById("ContentPlaceHolder1_txtPackAED").value) * 1) * (Zipper * 1);
            ZipperKgUSD = (ZipperKgAED * 1) / 3.675;
            PackingKgUSD = (PackingKgAED * 1) / 3.675;
            //PackKgUSD = (PackKgAED * 1) / 3.675;

            document.getElementById('ContentPlaceHolder1_txtZipperKgAED').value = (ZipperKgAED * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPackingKgAED').value = (PackingKgAED * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPackKgAED').value = (PackKgAED * 1).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtZipperKgUSD').value = (ZipperKgUSD * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPackingKgUSD').value = (PackingKgUSD * 1).toFixed(2);
            //document.getElementById('ContentPlaceHolder1_txtPackKgUSD').value = (PackKgUSD * 1).toFixed(2);

            TotRmKgUSD = (TotCostUSD * 1) + (WastageKGUSD * 1) + (SolventKgUSD * 1) + (ZipperKgUSD * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtPackBobbin1").value) * 1);
            //document.getElementById('ContentPlaceHolder1_txtTotRMKgUSD').value = (TotRmKgUSD * 1).toFixed(2);

            PackingBagWt = (BagWeight * 1) / 1000;
            document.getElementById('ContentPlaceHolder1_txtPackingWt').value = (PackingBagWt * 1).toFixed(6);
            PackingPkt = (PackingBagWt * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtPackingMode").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtPackingPkt').value = (PackingPkt * 1).toFixed(4);
            Sticker = (fill_zero(document.getElementById("ContentPlaceHolder1_txtSticker2").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtSticker3").value) * 1);
            Rod = (fill_zero(document.getElementById("ContentPlaceHolder1_txtRod2").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtRod3").value) * 1);
            Washer = (fill_zero(document.getElementById("ContentPlaceHolder1_txtWasher2").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtWasher3").value) * 1);
            Others = (fill_zero(document.getElementById("ContentPlaceHolder1_txtOther2").value) * 1) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtOther3").value) * 1);
            TotPacking = (Sticker * 1) + (Rod * 1) + (Washer * 1) + (Others * 1);
            document.getElementById('ContentPlaceHolder1_txtSticker1').value = (Sticker * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtRod1').value = (Rod * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtWasher1').value = (Washer * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtOther1').value = (Others * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPackTotal').value = (TotPacking * 1).toFixed(2);
            For1Kg = 1 / (PackingPkt * 1) * (TotPacking * 1);
            document.getElementById('ContentPlaceHolder1_txt1Kg').value = (For1Kg * 1).toFixed(8);
            document.getElementById("ContentPlaceHolder1_txtPackBobbin2").value = (For1Kg * 1).toFixed(2);

            TotRmKgAED = (TotCostAED * 1) + (WastageKGAED * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtSolventKgAED").value) * 1) + (ZipperKgAED * 1) + (PackingKgAED * 1) + (PackKgAED * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtPackCTN").value) * 1) + (For1Kg * 1);
            //document.getElementById('ContentPlaceHolder1_txtTotRMKgAED').value = (TotRmKgAED * 1).toFixed(2);

            ConvExtTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvExtCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvExtHr").value);
            ConvPrinRotoTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPrinRotoCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPrinRotoHr").value);
            ConvPrinBobstTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPrinBobstCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPrinBobstHr").value);
            ConvPrinCITot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPrinCICost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPrinCIHr").value);
            ConvLamTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvLamCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvLamHr").value);
            ConvSlittingTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvSlittingCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvSlittingHr").value);
            ConvPouchingTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPouchingCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvPouchingHr").value);
            ConvBagChickenTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvBagChickenCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvBagChickenHr").value);
            ConvBagGeneralTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtConvBagGeneralCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtConvBagGeneralHr").value);
            ConvTot = (ConvExtTot * 1) + (ConvPrinRotoTot * 1) + (ConvPrinBobstTot * 1) + (ConvPrinCITot * 1) + (ConvLamTot * 1) + (ConvSlittingTot * 1) + (ConvPouchingTot * 1) + (ConvBagChickenTot * 1) + (ConvBagGeneralTot * 1);
            document.getElementById('ContentPlaceHolder1_txtConvExtTot').value = (ConvExtTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvPrinRotoTot').value = (ConvPrinRotoTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvPrinBobstTot').value = (ConvPrinBobstTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvPrinCITot').value = (ConvPrinCITot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvLamTot').value = (ConvLamTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvSlittingTot').value = (ConvSlittingTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvPouchingTot').value = (ConvPouchingTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvBagChickenTot').value = (ConvBagChickenTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvBagGeneralTot').value = (ConvBagGeneralTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtConvTot').value = (ConvTot * 1).toFixed(2);
            Mach1 = (ConvTot * 1) / (fill_zero(document.getElementById("ContentPlaceHolder1_txtOrder").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtMach1').value = (Mach1 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtMachine2').value = (Mach1 * 1).toFixed(2);
            ConvFuelHr = (fill_zero(document.getElementById("ContentPlaceHolder1_txtConvFuelCost").value) * 1) * 14;
            document.getElementById('ContentPlaceHolder1_txtConvFuelHr').value = (ConvFuelHr * 1).toFixed(2);
            Machine1 = (Mach1 * 1) / 3.675;
            document.getElementById('ContentPlaceHolder1_txtMachine1').value = (Machine1 * 1).toFixed(2);
            Power1 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtPower2").value) * 1) / 3.675;
            document.getElementById('ContentPlaceHolder1_txtPower1').value = (Power1 * 1).toFixed(2);
            Fuel1 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtFuel2").value) * 1) / 3.675;
            document.getElementById('ContentPlaceHolder1_txtFuel1').value = (Fuel1 * 1).toFixed(2);
            MgmtFin2 = (TotRmKgAED * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtConvTotCostKg").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtMgmtFin2').value = (MgmtFin2 * 1).toFixed(2);
            ConvTotKg1 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtMgmtFin1").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtMgmtCost1").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtFin1").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtConvTotKg1').value = (ConvTotKg1 * 1).toFixed(2);
            ConvTotKg2 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtMgmtFin2").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtMgmtCost2").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtFin2").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtConvTotKg2').value = (ConvTotKg2 * 1).toFixed(2);

            ExtTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtExtCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtExtHr").value);
            PrinRotoTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtPrinRotoCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtPrinRotoHr").value);
            PrinBobstTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtPrinBobstCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtPrinBobstHr").value);
            PrinCITot = fill_zero(document.getElementById("ContentPlaceHolder1_txtPrinCICost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtPrinCIHr").value);
            LamTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtLamCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtLamHr").value);
            SlittingTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtSlittingCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtSlittingHr").value);
            PouchingTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtPouchingCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtPouchingHr").value);
            BagChickenTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtBagChickenCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtBagChickenHr").value);
            BagGeneralTot = fill_zero(document.getElementById("ContentPlaceHolder1_txtBagGeneralCost").value) * fill_zero(document.getElementById("ContentPlaceHolder1_txtBagGeneralHr").value);

            document.getElementById('ContentPlaceHolder1_txtExtTot').value = (ExtTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPrinRotoTot').value = (PrinRotoTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPrinBobstTot').value = (PrinBobstTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPrinCITot').value = (PrinCITot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtLamTot').value = (LamTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtSlittingTot').value = (SlittingTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtPouchingTot').value = (PouchingTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtBagChickenTot').value = (BagChickenTot * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtBagGeneralTot').value = (BagGeneralTot * 1).toFixed(2);
            TotalCost = (ExtTot * 1) + (PrinRotoTot * 1) + (PrinBobstTot * 1) + (PrinCITot * 1) + (LamTot * 1) + (SlittingTot * 1) + (PouchingTot * 1) + (BagChickenTot * 1) + (BagGeneralTot * 1);
            document.getElementById('ContentPlaceHolder1_txtTotalCost').value = (TotalCost * 1).toFixed(2);
            LabourCost = (TotalCost * 1) / (fill_zero(document.getElementById("ContentPlaceHolder1_txtOrder").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtLabourCost').value = (LabourCost * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtCurrent4').value = (fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1);
            Amortized6 = (ConvTotKg2 * 1) + (CyAmortize * 1);
            Current6 = (ConvTotKg2 * 1) + (CyOrder * 1);
            Amortized5 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1) / 3.675;
            Current5 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtCurrent4").value) * 1) / 3.675;
            Amortized3 = (((fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1) - (Amortized6 * 1)) / (fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1)) * 100;
            Amortized2 = (((fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1) - ((CyAmortize * 1) + (TotRmKgAED * 1))) / fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value)) * 100;
            Current2 = (1 - ((CyOrder + TotRmKgAED) / fill_zero(document.getElementById("ContentPlaceHolder1_txtCurrent4").value))) * 100;
            Amortized1 = (Amortized6 / fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value)) * 100;
            Current1 = (Current6 / fill_zero(document.getElementById("ContentPlaceHolder1_txtCurrent4").value)) * 100;
            Current3 = 100 - Current1;

            document.getElementById('ContentPlaceHolder1_txtAmortized6').value = (Amortized6 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtCurrent6').value = (Current6 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtAmortized5').value = (Amortized5 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtCurrent5').value = (Current5 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtAmortized3').value = (Amortized3 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtCurrent3').value = (Current3 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtAmortized2').value = (Amortized2 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtCurrent2').value = (Current2 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtAmortized1').value = (Amortized1 * 1).toFixed(2);
            document.getElementById('ContentPlaceHolder1_txtCurrent1').value = (Current1 * 1).toFixed(2);
            PerPcPrice = (fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1) / (BagPieceKg * 1);
            document.getElementById('ContentPlaceHolder1_txtPerPcPrice').value = (PerPcPrice * 1).toFixed(3);
            PerPcPriceFils = (fill_zero(document.getElementById("ContentPlaceHolder1_txtAmortized4").value) * 1) / (BagPieceKg * 1) * 100;
            document.getElementById('ContentPlaceHolder1_txtPerPcPriceFils').value = (PerPcPriceFils * 1).toFixed(3);
            OrderKg = (fill_zero(document.getElementById("ContentPlaceHolder1_txtOrderPcs").value) * 1) * (BagWeight * 1) / 1000;
            document.getElementById('ContentPlaceHolder1_txtOrderKg').value = (OrderKg * 1).toFixed(2);

            document.getElementById('ContentPlaceHolder1_txtLabourCost2').value = (fill_zero(document.getElementById("ContentPlaceHolder1_txtLabourCost").value) * 1);
            LabourCost1 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtLabourCost2").value) * 1) / 3.675;
            document.getElementById('ContentPlaceHolder1_txtLabourCost1').value = (LabourCost1 * 1).toFixed(2);
            Freight1 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtFreight2").value) * 1) / 3.675;
            document.getElementById('ContentPlaceHolder1_txtFreight1').value = (Freight1 * 1).toFixed(2);
            ConvTotCostKg = (Mach1 * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtPower2").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtFuel2").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtLabourCost2").value) * 1) + (fill_zero(document.getElementById("ContentPlaceHolder1_txtFreight2").value) * 1);
            document.getElementById('ContentPlaceHolder1_txtConvTotCostKg').value = (ConvTotCostKg * 1).toFixed(2);
        }

        function myGridCal() {
            var sgf = "ContentPlaceHolder1_sg1";
            var gridCount = $("[id*=sg1].GridviewScrollItem2").length;
            var totGSM = 0, totPer = 0, costUSD = 0, costAEd = 0, gTotUSd = 0, gTotAed = 0;
            var aedPrice = 3.675;
            for (var i = 0; i < gridCount; i++) {
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;

                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T1") {
                    document.getElementById(sgf + '_sg1_t5_' + i).value = (document.getElementById(sgf + '_sg1_t3_' + i).value * document.getElementById(sgf + '_sg1_t4_' + i).value).toFixed(4);
                }
                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T1" || document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T2") {
                    totGSM += (document.getElementById(sgf + '_sg1_t5_' + i).value * 1);
                    document.getElementById(sgf + '_sg1_t8_' + i).value = (document.getElementById(sgf + '_sg1_t7_' + i).value * aedPrice).toFixed(2);

                    document.getElementById(sgf + '_sg1_t9_' + i).value = ((document.getElementById(sgf + '_sg1_t7_' + i).value * document.getElementById(sgf + '_sg1_t6_' + i).value) / 100).toFixed(2);
                    document.getElementById(sgf + '_sg1_t10_' + i).value = (document.getElementById(sgf + '_sg1_t9_' + i).value * aedPrice).toFixed(2);
                }

                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "C" && document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "TOTAL") {
                    document.getElementById(sgf + '_sg1_t5_' + i).value = totGSM.toFixed(4);
                    document.getElementById('ContentPlaceHolder1_txtTotGSM').value = totGSM.toFixed(4);
                }
                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T4") {
                    debugger;
                    document.getElementById(sgf + '_sg1_t9_' + i).value = (document.getElementById(sgf + '_sg1_t10_' + i).value / aedPrice).toFixed(2);
                    gTotAed += (document.getElementById(sgf + '_sg1_t10_' + i).value * 1);
                }
                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T4" && document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() != "SOLVENT") {
                    document.getElementById(sgf + '_sg1_t7_' + i).value = document.getElementById('ContentPlaceHolder1_txtZipper').value;
                    document.getElementById(sgf + '_sg1_t10_' + i).value = (document.getElementById(sgf + '_sg1_t7_' + i).value * document.getElementById(sgf + '_sg1_t8_' + i).value).toFixed(2);
                }
                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T5") {
                    gTotAed += (document.getElementById(sgf + '_sg1_t10_' + i).value * 1);
                }
            }
            for (var i = 0; i < gridCount; i++) {
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;

                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T1" || document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "T2") {
                    document.getElementById(sgf + '_sg1_t6_' + i).value = ((document.getElementById(sgf + '_sg1_t5_' + i).value / totGSM) * 100).toFixed(2);
                    totPer += document.getElementById(sgf + '_sg1_t6_' + i).value * 1;
                    costUSD += document.getElementById(sgf + '_sg1_t9_' + i).value * 1;
                    costAEd += document.getElementById(sgf + '_sg1_t10_' + i).value * 1;

                }
                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "C" && document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "TOTAL") {
                    gTotAed += (document.getElementById(sgf + '_sg1_t10_' + i).value * 1);
                    document.getElementById(sgf + '_sg1_t6_' + i).value = totPer.toFixed(2);
                    document.getElementById(sgf + '_sg1_t9_' + i).value = costUSD.toFixed(2);
                    document.getElementById(sgf + '_sg1_t10_' + i).value = costAEd.toFixed(2);
                    document.getElementById('ContentPlaceHolder1_txtTotRM').value = totPer.toFixed(2);
                }

                if (document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "WASTAGE") {
                    gTotAed += (document.getElementById(sgf + '_sg1_t10_' + i).value * 1);
                    document.getElementById(sgf + '_sg1_t9_' + i).value = ((document.getElementById(sgf + '_sg1_t6_' + i).value * costUSD) / 100).toFixed(2);
                    document.getElementById(sgf + '_sg1_t10_' + i).value = (document.getElementById(sgf + '_sg1_t9_' + i).value * aedPrice).toFixed(2);
                }
                if (document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "WASTAGE" || document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "TOTAL" || document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "ZIPPER" || document.getElementById(sgf).rows[rowI].cells[1].innerHTML.toUpperCase() == "SOLVENT") {
                    gTotUSd += (document.getElementById(sgf + '_sg1_t9_' + i).value * 1);
                }

                if (document.getElementById(sgf).rows[rowI].cells[0].innerHTML == "C2") {

                    document.getElementById(sgf + '_sg1_t9_' + i).value = gTotUSd.toFixed(2);
                    document.getElementById('ContentPlaceHolder1_txtTotRMKgUSD').value = gTotUSd.toFixed(2);

                    document.getElementById(sgf + '_sg1_t10_' + i).value = gTotAed.toFixed(2);
                    document.getElementById('ContentPlaceHolder1_txtTotRMKgAED').value = gTotAed.toFixed(2);
                }
            }
        }

        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }
        function openBox(tk) {
            var ctlr = false;
            if (event.keyCode == 17) ctlr = true;
            if (ctrl = true && event.keyCode == 73) {
                document.getElementById('ContentPlaceHolder1_hf2').value = "A";
                document.getElementById('ContentPlaceHolder1_hf1').value = tk.id;
                document.getElementById('ContentPlaceHolder1_btnGridPop').click()
            }
            if (event.keyCode == 13) {
                document.getElementById('ContentPlaceHolder1_hf2').value = "I";
                document.getElementById('ContentPlaceHolder1_hf1').value = tk.id;
                document.getElementById('ContentPlaceHolder1_btnGridPop').click()
            }
        }

        $(document).ready(function () { cal(); });
    </script>

    <asp:Button ID="btnGridPop" runat="server" OnClick="btnGridPop_Click" Style="display: none" />
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
