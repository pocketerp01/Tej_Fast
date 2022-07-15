<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="findDrCr" CodeFile="findDrCr.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);
        });
        function gridviewScroll(gridId, gridDiv, headerFreeze, rowFreeze) {
            $(gridId).gridviewScroll({
                width: gridDiv.offsetWidth - 20,
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
                        <button type="submit" id="btnlist1" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnlist1_ServerClick">Full List</button>
                        <button type="submit" id="btnanex" class="btn btn-info" style="width: 100px;" runat="server" accesskey="t" onserverclick="btnanex_ServerClick">Anne<u>x</u>ure</button>
                        <button type="submit" id="btnlist2" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnlist2_ServerClick">List 2</button>
                        <button type="submit" id="btnlist3" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnlist3_ServerClick">Summary</button>
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
                                <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">Entry_No</label>
                                <label id="lblVty" runat="server" class="col-sm-1 control-label" title="lbl1">DC</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchnum" runat="server" placeholder="Entry No" ReadOnly="true" CssClass="form-control" Height="28px" ></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" CssClass="form-control" Height="28px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-2 control-label" title="lbl1">Customer</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnAcode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnAcode_Click" Style="width: 22px; float: right;" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtacode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtAname" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-2 control-label" title="lbl1">Item</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnIcode" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnIcode_Click" Style="width: 22px; float: right;" />
                                </div>
                                <div class="col-sm-3">
                                    <input id="txtIcode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtIname" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label10" runat="server" class="col-sm-3 control-label" title="lbl1">Part Name</label>
                                <div class="col-sm-9">
                                    <input id="txtCpartno" type="text" readonly="true" class="form-control" runat="server" placeholder="Part Name" maxlength="4" style="height: 28px;" />
                                </div>                             
                            </div>

                              <div class="form-group">
                                   <label id="Label13" runat="server" class="col-sm-3 control-label" title="lbl1">Old Tax : </label>
                                     <div class="col-sm-2">                                  
                                    <asp:CheckBox ID="chktOldTax" runat="server" />
                                </div>
                                    <label id="Label14" runat="server" class="col-sm-3 control-label" title="lbl1">Calculate TCS : </label>
                                     <div class="col-sm-2">                                  
                                    <asp:CheckBox ID="chktcs" runat="server" />
                                </div>
                                  </div>

                            <div class="form-group">
                                <button type="submit" id="btnVerify" class="btn btn-info" style="width: 100px; display:none" runat="server" onserverclick="btnVerify_ServerClick" >Verify DB</button>
                                </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-3 control-label" title="lbl1">D/N C/N Reason</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnDNCN" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnDNCN_Click" Style="width: 22px; float: right;" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtDnCnCode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtDnCnName" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">GST Class</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnGstClass" runat="server" ImageUrl="~/tej-base/css/images/bdsearch5.png" OnClick="btnGstClass_Click" Style="width: 22px; float: right;" />
                                </div>
                                <div class="col-sm-2">
                                    <input id="txtGstClassCode" type="text" readonly="true" class="form-control" runat="server" placeholder="Code" maxlength="4" style="height: 28px;" />
                                </div>
                                <div class="col-sm-6">
                                    <input id="txtGstClassName" type="text" readonly="true" class="form-control" runat="server" placeholder="Name" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-2 control-label" title="lbl1">PoNo.</label>
                                <div class="col-sm-4">
                                    <input id="txtponum" type="text" class="form-control" runat="server" placeholder="Po. No." style="height: 28px;" />
                                </div>
                                <label id="Label5" runat="server" class="col-sm-2 control-label" title="lbl1">Dt.</label>
                                <div class="col-sm-4">
                                    <input id="txtPodt" type="text" readonly="true" class="form-control" runat="server" placeholder="Date" maxlength="4" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-2 control-label" title="lbl1">Old Rate</label>
                                <div class="col-sm-4">
                                    <input id="txtOldRate" type="text" class="form-control" runat="server" placeholder="Old Rate" maxlength="8" style="height: 28px;" />
                                </div>
                                <label id="Label4" runat="server" class="col-sm-2 control-label" title="lbl1">New Rate</label>
                                <div class="col-sm-4">
                                    <input id="txtNrate" type="text" class="form-control" runat="server" placeholder="New Rate" maxlength="8" style="height: 28px;" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-2 control-label" title="lbl1">CGST/IGST</label>
                                <div class="col-sm-4">
                                    <input id="txtCgst" type="text" class="form-control" runat="server" placeholder="CGST/IGST" maxlength="8" style="height: 28px;" />
                                </div>
                                <label id="Label12" runat="server" class="col-sm-2 control-label" title="lbl1">SGST</label>
                                <div class="col-sm-4">
                                    <input id="txtSgst" type="text" class="form-control" runat="server" placeholder="SGST" maxlength="8" style="height: 28px;" />
                                </div>                                
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Item Details</a></li>
                                <li>
                                    <asp:Label ID="lblRowCount" runat="server"></asp:Label>
                                </li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Reel Details</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Lot.Dtl</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                                <li><a href="#DescTab6" id="tab6" runat="server" aria-controls="DescTab6" role="tab" data-toggle="tab">UDF Data</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; height: 200px; overflow: hidden; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" runat="server" Width="1550px" AutoGenerateColumns="false"
                                            Style="background-color: #FFFFFF; color: White;" Font-Size="13px">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797C0" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:BoundField DataField="sg1_h1" HeaderText="sg1_h1"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h2" HeaderText="sg1_h2"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h3" HeaderText="sg1_h3"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h4" HeaderText="sg1_h4"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h5" HeaderText="sg1_h5"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h6" HeaderText="sg1_h6"></asp:BoundField>
                                                <%--<asp:BoundField DataField="sg1_h7" HeaderText="sg1_h7"></asp:BoundField>--%>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>sg1_h7</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_h7" runat="server" Text='<%#Eval("sg1_h7") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_h8" HeaderText="sg1_h8"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h9" HeaderText="sg1_h9"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_h10" HeaderText="sg1_h10"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_SrNo" HeaderText="sg1_SrNo"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="sg1_f1"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_f2" HeaderText="sg1_f1"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f1"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f1"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f1"></asp:BoundField>

                                                <asp:BoundField DataField="sg1_t1" HeaderText="sg1_t1"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t2" HeaderText="sg1_t2"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t3" HeaderText="sg1_t3"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t4" HeaderText="sg1_t4"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t5" HeaderText="sg1_t5"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t6" HeaderText="sg1_t6"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t7" HeaderText="sg1_t7"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t8" HeaderText="sg1_t8"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t9" HeaderText="sg1_t9"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t10" HeaderText="sg1_t10"></asp:BoundField>

                                                <asp:BoundField DataField="sg1_t11" HeaderText="sg1_t11"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t12" HeaderText="sg1_t12"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t13" HeaderText="sg1_t13"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t14" HeaderText="sg1_t14"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t15" HeaderText="sg1_t15"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t16" HeaderText="sg1_t16"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t17" HeaderText="sg1_t17"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t18" HeaderText="sg1_t18"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t19" HeaderText="sg1_t19"></asp:BoundField>
                                                <asp:BoundField DataField="sg1_t20" HeaderText="sg1_t20"></asp:BoundField>

                                            </Columns>

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks"></asp:TextBox>
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
    <asp:HiddenField ID="hfCNote" runat="server" />

    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;" CssClass="col-sm-1 control-label"></asp:Label>
    <asp:Label ID="txtcustPo" runat="server" Style="visibility: hidden"></asp:Label>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
