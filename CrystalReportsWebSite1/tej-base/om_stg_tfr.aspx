<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_stg_tfr" Title="Tejaxo" CodeFile="om_stg_tfr.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="content-wrapper">
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
                        <button type="submit" id="btncan" runat="server" accesskey="c" style="width: 100px;" class="btn btn-info" onserverclick="btncan_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnext" runat="server" accesskey="x" style="width: 100px;" class="btn btn-info" onserverclick="btnext_ServerClick">E<u>x</u>it</button>
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
                            <div class="form-group">
                                <label id="lbl1" runat="server" class="col-sm-3 control-label" title="lbl1">Doc.No</label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100px" Placeholder="Doc.No" CssClass="tkbox"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" runat="server" Width="80px" Placeholder="Doc.Date" CssClass="textarea"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                                <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">Stage From</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnstgfrom" runat="server" ToolTip="Select Stage from Transfer"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Style="width: 22px; float: right" OnClick="btnstgfrom_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtstgfcode" runat="server" placeholder="Code" ReadOnly="true" Width="70px" CssClass="tkbox"></asp:TextBox>
                                    <asp:TextBox ID="txtstgfname" runat="server" placeholder="Stage From" ReadOnly="true" Width="250px" CssClass="tkbox"></asp:TextBox>
                                </div>
                                <label id="Label3" runat="server" class="col-sm-2 control-label" title="lbl1">Stage To</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnstgto" runat="server" ToolTip="Select Stage to Transfer"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Style="width: 22px; float: right" OnClick="btnstgto_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtstgtcode" runat="server" placeholder="Code"
                                        Width="70px" CssClass="tkbox" ReadOnly="true"></asp:TextBox>

                                    <asp:TextBox ID="txtstgtname" runat="server" placeholder="Stage To"
                                        ReadOnly="true" Width="250px" CssClass="tkbox"></asp:TextBox>
                                </div>
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Barcode</label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtbarcode" runat="server" AutoPostBack="true" Width="250px"
                                        OnTextChanged="txtbarcode_TextChanged"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <h4>1. Select Stage From (from where material to sent)
                                <br />
                                2. Select To From (where material to be rcvd)
                                <br />
                                3. Select Material
                                <br />
                                4. Save and Continue..
                            </h4>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="lbBody" style="color: White; height: 250px; max-height: 250px; max-width: 1305px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                            <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False"
                                ForeColor="#333333" Style="background-color: #FFFFFF; color: White;"
                                OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound">
                                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <EditRowStyle BackColor="#999999" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>A</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnadd" runat="server" CommandName="Row_Add" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                        </ItemTemplate>
                                        <ItemStyle Width="11px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>D</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                        </ItemTemplate>
                                        <ItemStyle Width="11px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                        <ItemStyle Width="10px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="icode" HeaderText="ERP Code" ReadOnly="True">
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="iname" HeaderText="Item Name" ReadOnly="True">
                                        <HeaderStyle Width="350px" />
                                        <ItemStyle Width="350px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cpartno" HeaderText="Part No." ReadOnly="True">
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="unit" HeaderText="Unit" ReadOnly="True"></asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Remarks</HeaderTemplate>
                                        <HeaderStyle Width="80px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtfld1" runat="server" Width="90px" Text='<%#Eval("tfld1") %>' MaxLength="50" CssClass="tkbox" ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>Qty Tfr</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtfld2" runat="server" Width="70px" Text='<%#Eval("tfld2") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" MaxLength="10" CssClass="tkbox"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="poqty" HeaderText="Batch.No" ReadOnly="True" />
                                    <asp:BoundField DataField="tfld2" HeaderText="Qty.Prod" ReadOnly="True" />
                                    <asp:BoundField DataField="tfld3" HeaderText="Binno" ReadOnly="True" />
                                </Columns>
                                <HeaderStyle BackColor="#1797c0" ForeColor="White"
                                    CssClass="GridviewScrollHeader" Font-Bold="True" />
                                <RowStyle CssClass="grdrow" BackColor="#F7F6F3" ForeColor="#333333" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                Remarks                                
                        <div class="col-sm-12">
                            <asp:TextBox ID="txtremarks" runat="server" TextMode="MultiLine" Width="99%" CssClass="tkbox"></asp:TextBox>
                        </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </section>
    <div id="divylb" runat="server" align="center" style="font-size: medium">
        Marking Instructions
                      <table style="border-style: groove; border-width: thin">
                          <tr>
                              <td>1. Item Name </td>
                              <td>:</td>
                              <td><u>
                                  <asp:Label ID="lblpartno" runat="server"></asp:Label></u></td>
                          </tr>
                          <tr>
                              <td>2. LC</td>
                              <td>:</td>
                              <td><u>
                                  <asp:Label ID="lbllcno" runat="server"></asp:Label></u></td>
                          </tr>
                          <tr>
                              <td>3. YLB</td>
                              <td>:</td>
                              <td><u>
                                  <asp:Label ID="lblylb" runat="server"></asp:Label></u></td>
                          </tr>
                      </table>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="popselected" runat="server" />
</asp:Content>
