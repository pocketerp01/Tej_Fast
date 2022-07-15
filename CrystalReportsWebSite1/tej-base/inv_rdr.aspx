<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="inv_rdr" Title="Tejaxo" CodeFile="inv_rdr.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%;">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" accesskey="N" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnsave" accesskey="S" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <button type="submit" id="btndel" accesskey="l" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btndel_ServerClick">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_ServerClick">Lis<u>t</u></button>
                        <button type="submit" id="btncan" accesskey="C" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnext" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnext_ServerClick">E<u>x</u>it</button>

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
                                <label id="tdentryno" runat="server" class="col-sm-4 control-label">Entry No.</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true" placeholder="Entry No" Width="100%" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="tdbarcode" runat="server" class="col-sm-4 control-label">Bar_Code_Value</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtinv" runat="server" Width="100%" AutoPostBack="true" OnTextChanged="txtinv_TextChanged" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="tdentrydt" runat="server" class="col-sm-4 control-label">Entry Date</label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchdate" runat="server" ReadOnly="true" placeholder="Entry Date" Width="100%" CssClass="form-control" Height="28px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="MaskedEdit2" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="tdreelname" runat="server" class="col-sm-4 control-label">Reel BarCode</label>
                                <div class="col-sm-6" id="tdreeltxt" runat="server">
                                    <asp:TextBox ID="txtreel" runat="server" Width="100%" AutoPostBack="true" OnTextChanged="txtreel_TextChanged" Height="30px"></asp:TextBox>
                                </div>
                            </div>
                            <div id="reelloc" runat="server" class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label">Reel Location</label>
                                <div class="col-sm-6">
                                    <asp:DropDownList ID="ddreelloc" runat="server" Width="100%" Height="30px"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12" id="trlbl" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-6 control-label" style="border-style: groove; border-bottom-width: thin" align="center">Wt Req.</label>
                                <label id="Label2" runat="server" class="col-sm-6 control-label" style="border-style: groove; border-bottom-width: thin" align="center">Wt Iss.</label>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblwtrq" class="col-sm-6 control-label" runat="server" Style="text-align: center" Font-Bold="true"></asp:Label>
                                <asp:Label ID="lblwtis" class="col-sm-6 control-label" runat="server" Style="text-align: center" Font-Bold="true"></asp:Label>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbljobname" runat="server" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="tr1" runat="server" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Item Details</a></li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="250px" Font-Size="13px"
                                            AutoGenerateColumns="false">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f2" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="sg1_f6" />
                                                <asp:BoundField DataField="sg1_f7" HeaderText="sg1_f7" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        Qty Found
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_tk1" runat="server" Text='<%#Eval("sg1_tk1") %>' Width="100%"></asp:TextBox>
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

            </div>
        </section>
    </div>


    <%--<asp:TextBox ID="tktemp" runat="server" Width="1px" Height="1px"></asp:TextBox>--%>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
</asp:Content>
