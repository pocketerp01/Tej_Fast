<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_typop_mst" CodeFile="om_typop_mst.aspx.cs" %>

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
                    </td>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label></td>
                </tr>
            </table>
        </section>

        <section class="content">
            <div class="row">

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">Opt_No</label>
                                <div class="col-sm-8">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" placeholder="Opt No." readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-4 control-label" title="lbl1">Opt_Date</label>
                                <div class="col-sm-8">
                                    <input id="txtvchdate" type="text" class="form-control" runat="server" placeholder="Opt Date" readonly="readonly" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl2" runat="server" class="col-sm-4 control-label" title="lbl1">Master Name</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl2" type="text" class="form-control" runat="server" placeholder="Master Name" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="lbl3" runat="server" class="col-sm-3 control-label" title="lbl1">Type Code</label>
                                <div class="col-sm-1" id="divPersonName" runat="server">
                                    <asp:ImageButton ID="btnPersonName" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btntype_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlbl3" type="text" class="form-control"  runat="server" placeholder="Type Code" maxlength="2" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lbl4" runat="server" class="col-sm-3 control-label" title="lbl1">A/c Code</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnactg_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlbl4" type="text" class="form-control" runat="server" readonly="readonly" placeholder="A/c Code" maxlength="6" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="LBL_Itclass" runat="server" class="col-sm-3 control-label" title="lbl1">Class Code</label>
                                <div class="col-sm-1" id="div12" runat="server">
                                    <asp:ImageButton ID="BTN_Itclass" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btn_ITC_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="TXT_Itclass" type="text" class="form-control" runat="server" readonly="readonly" placeholder="Class Code" maxlength="6" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="d3" runat="server">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Grp Code</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btntype_Click" Visible="false" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="Text1" type="text" class="form-control" runat="server" maxlength="2" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Make Ready Time</label>
                                <div class="col-sm-1" id="div3" runat="server">
                                    <asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnactg_Click" Visible="false" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="Text2" type="text" class="form-control" runat="server" maxlength="6" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-4 control-label" title="lbl1">Prd.Time(/1000ms)</label>
                                <%--                                //<asp:Label ID="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">Parameter_3</asp:Label>--%>

                                <div class="col-sm-8">
                                    <input id="Text3" type="text" class="form-control" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6" id="d4" runat="server">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-3 control-label" title="lbl1">M/C Code</label>
                                <div class="col-sm-1" id="div4" runat="server">
                                    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btntype_Click" Visible="false" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="Text4" type="text" class="form-control" runat="server" maxlength="2" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Main Grp Ccode</label>
                                <div class="col-sm-1" id="div5" runat="server">
                                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnactg_Click" Visible="false" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="Text5" type="text" class="form-control" runat="server" maxlength="6" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-4 control-label" title="lbl1">Sheet/Ctn(0=Sht,1=Ctn)</label>
                                <%--                                //<asp:Label ID="Label2" runat="server" class="col-sm-4 control-label" title="lbl1">Parameter_3</asp:Label>--%>

                                <div class="col-sm-8">
                                    <input id="Text6" type="text" class="form-control" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">List</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab7" id="tab7" runat="server" aria-controls="DescTab7" role="tab" data-toggle="tab">Add/Edit Types</a></li>
                                <li><a href="#DescTab8" id="tab8" runat="server" aria-controls="DescTab8" role="tab" data-toggle="tab">Cheque Settings</a></li>
                            
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg5" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>

                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand" Visible="false">
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
                                                    <HeaderTemplate>Type</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Name</HeaderTemplate>
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
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="300px" Font-Size="Smaller"
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
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl10" runat="server" Text="lbl10" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl10" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl10_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl10" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl11" runat="server" Text="lbl11" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl11" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl11_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl11" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl12" runat="server" Text="lbl12" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl12" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl12_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl12" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl13" runat="server" Text="lbl13" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl13" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl13_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl13" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl14" runat="server" Text="lbl14" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl14" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl14_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl14" runat="server" Width="350px"></asp:TextBox>

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
                                                                <asp:Label ID="lbl15" runat="server" Text="lbl15" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl15" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl15_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl15" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl16" runat="server" Text="lbl16" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl16" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl16_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl16" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl17" runat="server" Text="lbl17" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl17" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl17_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl17" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl18" runat="server" Text="lbl18" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl18" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl18_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl18" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl19" runat="server" Text="lbl19" CssClass="col-sm-2 control-label"></asp:Label></td>
                                                            <td>
                                                                <asp:ImageButton ID="btnlbl19" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl19_Click" /></td>
                                                            <td>

                                                                <asp:TextBox ID="txtlbl19" runat="server" Width="350px"></asp:TextBox>

                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div class="lbBody" style="height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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
                                    <div class="lbBody" style="height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
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

                            <div role="tabpanel" class="tab-pane" id="DescTab7">
                            <div class="col-md-6">
                               <div>
                                <div class="box-body" id="div6" runat="server">
                                    
                                    <div class="form-group">
                                        <label id="Label10" runat="server" class="col-sm-3 control-label" title="lbl1">A/C Code</label>
                                        <div class="col-sm-1" id="div7" runat="server">
                                        <asp:ImageButton ID="BtnAct" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="BtnAct_Click" />
                                        </div>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="txtact" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1">CHQ[SBI/BOI/CITI]</label>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="txtchk" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label12" runat="server" class="col-sm-3 control-label" title="lbl1">Alow User</label>
                                        <div class="col-sm-1" id="div8" runat="server">
                                        <asp:ImageButton ID="BtnUser" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="BtnUser_Click1" />
                                        </div>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="txtUser" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label13" runat="server" class="col-sm-4 control-label" title="lbl1">PREFIX/Ac.NO.</label>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="Text7" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label14" runat="server" class="col-sm-4 control-label" title="lbl1">Chq.No</label>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="txtchqno" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Show In Unit</label>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="txtunit" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label id="Label16" runat="server" class="col-sm-4 control-label" title="lbl1">GST Sup Type</label>
                                        <div class="col-sm-8">
                                            <input style="height: 28px" id="txtGstSub" type="text" class="form-control" runat="server" placeholder="" />                                  
                                        </div>
                                    </div>
                                </div>
                            </div>
                            </div>
                            
                            </div> 

                            <div class="tab-pane" id="DescTab8">
                                <div class="col-md-6">
                                    <div>
                                        <div class="box-body" id="div9" runat="server">
                                            <div class="form-group">
                                            <label id="Label17" runat="server" class="col-sm-4 control-label" title="lbl1">Top For A/C Payee</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="txtacpaytop" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label19" runat="server" class="col-sm-4 control-label" title="lbl1">Top For Date</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtDatetop" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label21" runat="server" class="col-sm-4 control-label" title="lbl1">Top for Party Name</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtPrtyNme" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label23" runat="server" class="col-sm-4 control-label" title="lbl1">Top for Amount(In words)</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAmttop" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label25" runat="server" class="col-sm-4 control-label" title="lbl1">Top for Amt in Figures</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAmtFigtop" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label27" runat="server" class="col-sm-4 control-label" title="lbl1">Top for Firm Name</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtFrmName" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label31" runat="server" class="col-sm-4 control-label" title="lbl1">Top For Auth. Signatory</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAuthSign" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label33" runat="server" class="col-sm-4 control-label" title="lbl1">Top For A/C Number</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAcNo" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            
                                        </div>
                                    </div>
                                </div>


                                <div class="col-md-6">
                                    <div>
                                        <div class="box-body" id="div10" runat="server">
                                            <div class="form-group">
                                            <label id="Label18" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="txtleftac" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label20" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtLeftDt" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label22" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtPrtyNamelft" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)"/>                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label24" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAmtLft" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label26" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAmtfglft" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label28" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtFrmLft" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)"/>                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label32" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAuthSiglft" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)"/>                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label34" runat="server" class="col-sm-4 control-label" title="lbl1">Left</label>
                                            <div class="col-sm-8">
                                                <input style="height: 28px" id="TxtAcNoLft" type="text" class="form-control" runat="server" placeholder="" onkeypress="return isDecimalKey(event)"/>                                  
                                            </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div>
                                        <div class="box-body" id="div11" runat="server">
                                            <div class="form-group">
                                            <label id="Label35" runat="server" class="col-sm-3 control-label" title="lbl1" style="font-size:smaller">Caption for Auth. Signatory/Partner etc</label>
                                            <div class="col-sm-9">
                                                <input style="height: 28px" id="TxtCap" type="text" class="form-control" runat="server" placeholder="" width="600px" />                                  
                                            </div>
                                            </div>
                                            <div class="form-group">
                                            <label id="Label36" runat="server" class="col-sm-12 control-label" title="lbl1" style="font-size:smaller">
                                            <br />Change Approx 75-100 Pixel at a time.<br />
                                            Increase Left Value to Shift towards Right.<br />
                                            Increase Top value to Shift towards Bottom.<br />
                                            Put 0 Top and 0 Left for No Printing.

                                            </label>
                                            
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
