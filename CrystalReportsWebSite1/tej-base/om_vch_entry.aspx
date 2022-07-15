<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_vch_entry" CodeFile="om_vch_entry.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {

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
    </script>
    <script type="text/javascript">
        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;

        window.onload = function () {
            UpperBound = parseInt('<%= this.sg1.Rows.Count %>') - 1;
            LowerBound = 0;
            SelectedRowIndex = -1;
        }

        function SelectRow(CurrentRow, RowIndex) {
            if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound) return;

            if (SelectedRow != null) {
                SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
                SelectedRow.style.color = SelectedRow.originalForeColor;
            }

            if (CurrentRow != null) {
                CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
                CurrentRow.originalForeColor = CurrentRow.style.color;
                CurrentRow.style.backgroundColor = '#DCFC5C';
                CurrentRow.style.color = 'Black';
            }

            SelectedRow = CurrentRow;
            SelectedRowIndex = RowIndex;
            setTimeout("SelectedRow.focus();", 0);
        }

        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;

            if (KeyCode == 40)
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            else if (KeyCode == 38)
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            else if (KeyCode == 32) {
                var GridVwHeaderChckbox = document.getElementById("<%=sg1.ClientID %>");
                    if (GridVwHeaderChckbox.rows[SelectedRowIndex + 1].cells[1].getElementsByTagName("INPUT")[0].checked == true)
                        GridVwHeaderChckbox.rows[SelectedRowIndex + 1].cells[1].getElementsByTagName("INPUT")[0].checked = false;
                    else GridVwHeaderChckbox.rows[SelectedRowIndex + 1].cells[1].getElementsByTagName("INPUT")[0].checked = true;

                    calculateSum();
                }
            //return false;
    }
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
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_Click"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_Click">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_Click"><u>S</u>ave</button>
                        <button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_Click"><u>P</u>rint</button>
                        <button type="submit" id="btndel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="l" onserverclick="btndel_Click">De<u>l</u>ete</button>
                        <button type="submit" id="btnlist" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnlist_Click">Lis<u>t</u></button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_Click"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_Click">E<u>x</u>it</button>
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
                                <div class="col-sm-2">
                                    <asp:Label ID="lblrcode" runat="server"></asp:Label>
                                </div>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblrname" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="lbl1" runat="server" class="col-sm-3 control-label" title="lbl1">Vch_No.</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Placeholder="Vch No" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchdate" runat="server" Placeholder="Vch Date" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Party</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnacode" runat="server" ToolTip="Select Party"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Style="width: 22px; float: right" OnClick="btnacode_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtacode" runat="server" placeholder="Code" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtaname" runat="server" placeholder="Party Name" ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">Other Ac</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnothrac" runat="server" ToolTip="Other Account"
                                        ImageUrl="~/tej-base/css/images/bdsearch5.png"
                                        Style="width: 22px; float: right" OnClick="btnothrac_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtothac" runat="server" placeholder="Code" ReadOnly="true"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtothname" runat="server" placeholder="Other A/c Name"
                                        ReadOnly="true" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" Text="Bill_No" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttrefnum" runat="server"
                                        Placeholder="Bill No" CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label8" runat="server" Text="Dated" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtchqdt" runat="server" Placeholder="Bill Date"
                                        CssClass="form-control" Height="28px" TextMode="Date"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label3" runat="server" Text="MRR_No" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtmrrno" runat="server"
                                        Placeholder="MRR No" CssClass="form-control" Height="28px" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label5" runat="server" Text="MRR_Dt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtmrrdt" runat="server" Placeholder="MRR Date"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label6" runat="server" Text="Other_Amount" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtothamt" runat="server"
                                        Placeholder="Other Amount" Style="text-align: right" CssClass="form-control" Height="28px" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label7" runat="server" Text="Balance_Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbalamt" runat="server" Placeholder="Balance Amt"
                                        CssClass="form-control" Style="text-align: right" Height="28px"></asp:TextBox>
                                </div>

                                <asp:Label ID="lblPayTerms" runat="server" Text="Payment Term" CssClass="col-sm-6 control-label" Style="display: none"></asp:Label>


                                <%--                                <asp:Label ID="lblPayTerms" runat="server" Text="Payment Term" CssClass="col-sm-6 control-label"></asp:Label>                                

                                <asp:Label ID="Label5" runat="server" Text="Chq Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txttamt" runat="server" Style="text-align: right"
                                        Placeholder="Chq Amount" CssClass="form-control" Height="28px" onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false"></asp:TextBox>
                                </div>

                                <asp:Label ID="Label10" runat="server" Text="" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    </div>
                                <asp:Label ID="Label6" runat="server" Text="Selected_Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbillamount" runat="server" Placeholder="Selected Amt" Style="text-align: right"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label11" runat="server" Text="" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    </div>

                                <div class="form-group">
                                    <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Other Ac Amt</label>
                                    <div class="col-sm-3">
                                        <asp:TextBox ID="txtothamt" runat="server" placeholder="Oth Amt" Height="28px" CssClass="form-control" Style="text-align: right;"></asp:TextBox>
                                    </div>
                                </div>                            

                                <asp:Label ID="Label7" runat="server" Text="Bal Amt" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtbalamt" runat="server" Placeholder="Balance Amt" Style="text-align: right"
                                        CssClass="form-control" Height="28px"></asp:TextBox>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Item Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="height: 230px; max-height: 230px; max-width: 1305px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" Width="100%" AutoGenerateColumns="False"
                                            Style="background-color: #FFFFFF; color: White;" Font-Size="13px"
                                            OnRowCommand="sg1_RowCommand" OnRowDataBound="sg1_RowDataBound" OnRowCreated="sg1_RowCreated">
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
                                                    <HeaderTemplate>Tick</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="margin-left: 6px;">
                                                            <asp:CheckBox ID="chk1" runat="server" onclick="calculateSum();" />
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="acode" HeaderText="Code" ReadOnly="false"></asp:BoundField>
                                                <asp:BoundField DataField="Invno" HeaderText="Inv.No" ReadOnly="false"></asp:BoundField>
                                                <asp:BoundField DataField="invdate" HeaderText="Inv.Dt" ReadOnly="True"></asp:BoundField>
                                                <asp:BoundField DataField="damt" HeaderText="Dr.Amt" ReadOnly="True" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="camt" HeaderText="Cr.Amt" ReadOnly="True" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:BoundField DataField="net" HeaderText="Net.Amt" ReadOnly="True" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Pass.For</HeaderTemplate>
                                                    <HeaderStyle Width="90px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtpassfor" runat="server" Text='<%#Eval("passamt") %>' onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" MaxLength="10" CssClass="form-control" Height="21px" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Manual Amt.</HeaderTemplate>
                                                    <HeaderStyle Width="90px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtmanualfor" runat="server" Text='<%#Eval("manualamt") %>' onkeyup="calculateSum();" onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Style="text-align: right" MaxLength="10" CssClass="form-control" Height="21px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cumbal" HeaderText="Cum.Bal" ReadOnly="True" ItemStyle-HorizontalAlign="Right"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <HeaderStyle Width="250px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtrmk" runat="server" Text='<%#Eval("rmk") %>' MaxLength="50" CssClass="form-control" Height="21px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab4"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab5"></div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab6"></div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtremarks" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Add Your Remakrs..."></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <label for="exampleInputEmail1">Total Value :</label>
            <label id="lblqtysum" runat="server" style="display: none">0</label>
        </section>
    </div>
    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="popselected" runat="server" />
    <div class="col-sm-8" style="display: none">
        <label for="exampleInputEmail1">Type</label>
        <asp:Label ID="lbltypename" runat="server"></asp:Label>
    </div>
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
</asp:Content>
