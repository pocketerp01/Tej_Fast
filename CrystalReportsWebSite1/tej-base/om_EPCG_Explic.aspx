<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_EPCG_Explic" CodeFile="om_EPCG_Explic.aspx.cs" %>

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
                                <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry No.</asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    <asp:TextBox ID="txtgrade" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                    <asp:TextBox ID="txtIcode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" Visible="false"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Entry_Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>


                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Licence_No</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl2" runat="server" Text="lbl2" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Licence_Date</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlicdt" placeholder="Date" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="Export_Obligation" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-1" style="display: none;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" onChange="cal1()" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl7" runat="server" CssClass="form-control" MaxLength="80" ReadOnly="true" Style="text-align:right"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label15" runat="server" Text="No_Of_Times" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl7a" runat="server" CssClass="form-control" Width="100%" MaxLength="25" ReadOnly="true" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1" style="visibility: hidden;">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlicvalid" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                    <asp:TextBox ID="txtacode" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtaname" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True" Visible="false">Licence_Qty</asp:Label>
                                <div class="col-sm-4" style="display: none">
                                    <asp:TextBox ID="txtcurrqty" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="25" Style="text-align:right"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl6" runat="server" Text="lbl6" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Licence_Val</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcurrval" ReadOnly="true" runat="server" MaxLength="25" CssClass="form-control" Width="100%" Style="text-align:right"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Selected_Value</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl3" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" Style="text-align:right"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">HSN_Code</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl5" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddsurfprep" runat="server" TabIndex="11" Width="100%" Height="35px" CssClass="rounded_corners">                                                
                                            </asp:DropDownList>--%>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txthsname" ReadOnly="true" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group">
                                <%--  <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Selected_Value</asp:Label>
                                 <div class="col-sm-4">
                              <asp:TextBox ID="txtlbl3" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" ></asp:TextBox>                                            
                                        </div>--%>
                                <asp:Label ID="Label19" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Exp_Obligation_Import</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_exp_oblig_imp" runat="server" ReadOnly="true" onChange="cal1()" CssClass="form-control" Width="100%" Style="text-align:right"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label4" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Bal_Value</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtbalqty" runat="server" ReadOnly="true" CssClass="form-control" Width="100%" MaxLength="22" Style="text-align:right"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label18" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">R/M_Item_Desc</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtitemdesc" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <asp:Label ID="Label16" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Ent_by</asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtent_by" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Ent_Dt</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtent_dt" runat="server" ReadOnly="true" CssClass="form-control" Width="100%"></asp:TextBox>
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
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel/Lot Dtl</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">                               
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="270px" Font-Size="13px"
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

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Item</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnitem" runat="server" CommandName="SG1_ROW_ITEM" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Select Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Item_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t10" runat="server" Text='<%#Eval("sg1_t10") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true" MaxLength="135"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="5px">
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Invoices" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Invoices" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Ship_Bill_No" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Ship_Bill_Date" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Invoice_no" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Invoice_Date" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Icode" ItemStyle-Width="80px" Visible="false" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="Item_Name" Visible="false" />

                                                <asp:TemplateField ItemStyle-Width="15px">
                                                    <HeaderTemplate>Direct_Exports(USD)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onChange="caltotalvalue()" Width="100%" MaxLength="22" ReadOnly="true" Style="text-align:right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>FOB_Value_Rs</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="28" Style="text-align:right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField ItemStyle-Width="110px">
                                                    <HeaderTemplate>FOB_Value_FC</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" MaxLength="28" Style="text-align:right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-Width="80px">
                                                    <HeaderTemplate>FOB_Value_BRC</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="28" Style="text-align:right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>FOB_BRC_US$</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>'  Width="100%" MaxLength="28" Style="text-align:right"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Interpretation</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Remarks</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t7" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>abcd</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%" onkeypress="return isDecimalKey(event)" MaxLength="7"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Item_Desc</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' Width="100%" onkeypress="return isDecimalKey(event)" ReadOnly="true"></asp:TextBox>
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

                <div class="col-md-12" style="display: none;">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Footer_Notes</asp:Label>
                                <div class="col-sm-11">
                                    <asp:TextBox ID="txtfootnote" MaxLength="200" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-12">
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
    <asp:HiddenField ID="hf2" runat="server" />
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

        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

        function caltotalvalue() {
            var rowTot = 0;
            var colTot = 0;
            var min = 0;
            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid.rows.length - 1; i++) {
                colTot += fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t1_' + i).value));
                //row total is total of total_qty field row wise
                document.getElementById('ContentPlaceHolder1_txtlbl3').value = fill_zero(colTot);
                min = fill_zero(document.getElementById('ContentPlaceHolder1_txt_exp_oblig_imp').value);
                document.getElementById('ContentPlaceHolder1_txtbalqty').value = min - colTot;
            }
        }

    </script>

    <asp:HiddenField ID="TabName" runat="server" />

</asp:Content>
