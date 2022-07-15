<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_sman_log" CodeFile="om_sman_log.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
<%--            $("#<%= btnAtt.ClientID%>").click();--%>
        };
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

                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick"><u>C</u>ancel</button>
                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                        <button type="submit" id="btnAtch" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnAtch_ServerClick">Attachment</button>
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
                                <label id="Label1" runat="server" class="col-sm-4 control-label" title="lbl1">SPR_No.</label>
                                <div class="col-sm-4">
                                    <input id="txtvchnum" type="text" class="form-control" runat="server" placeholder="SPR No." readonly="readonly" />
                                </div>
                                <div class="col-sm-4">
                                    <input id="txtvchdate" type="text" class="form-control" runat="server" placeholder="SPR Date" readonly="readonly" />
                                </div>
                            </div>


                            <div class="form-group">
                                <label id="Label8" runat="server" class="col-sm-3 control-label" title="lbl1">Lead_Source</label>
                                <div class="col-sm-1" id="div2" runat="server">
                                    <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnLdtype_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlsource" type="text" class="form-control" runat="server" placeholder="Source" maxlength="40" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label5" runat="server" class="col-sm-3 control-label" title="lbl1">Industry/Vertical</label>
                                <div class="col-sm-1" id="div1" runat="server">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnIndus_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlvert" type="text" class="form-control" runat="server" placeholder="Industry" maxlength="40" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label7" runat="server" class="col-sm-3 control-label" title="lbl1">Lead_Category</label>
                                <div class="col-sm-1" id="div4" runat="server">
                                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnanalysis_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlbl9" type="text" class="form-control" runat="server" placeholder="Category" maxlength="40" />
                                </div>
                            </div>


                              <div class="form-group">
                                    <label id="csubject" runat="server" class="col-sm-4 control-label" title="lbl1">Lead_Interest</label>
                                    <div class="col-sm-8">
                                        <input id="Lsubject" type="text" class="form-control" runat="server" placeholder="Lead_Interest" maxlength="30" />
                                    </div>
                              </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label6" runat="server" class="col-sm-4 control-label" title="lbl1">Name of Company</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl8" type="text" class="form-control" runat="server" placeholder="Name of Co." maxlength="30" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label2" runat="server" class="col-sm-3 control-label" title="lbl1">Contact Name</label>
                                <div class="col-sm-1" id="divPersonName" runat="server">
                                    <asp:ImageButton ID="btnPersonName" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnPersonName_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlbl2" type="text" class="form-control" runat="server" placeholder="Contact Name" maxlength="30" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label9" runat="server" class="col-sm-3 control-label" title="lbl1">Contact_level</label>
                                <div class="col-sm-1" id="divCocd" runat="server">
                                    <asp:ImageButton ID="btnCocd" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnCocd_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <input id="txtlbl4" type="text" class="form-control" runat="server" placeholder="Contact Level" maxlength="4" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-4 control-label" title="lbl1">Contact No.</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl3" type="text" class="form-control" runat="server" placeholder="Contact No." maxlength="10" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-4 control-label" title="lbl1">Email Id</label>
                                <div class="col-sm-8">
                                    <input id="txtlbl5" type="text" class="form-control" runat="server" placeholder="Mail ID" maxlength="30" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
                
                <div class="col-md-6" id="div3" runat="server">
                    <div>
                        <div class="box-body">
                            <label id="lbltxtrmk" runat="server" class="col-sm-4 control-label" title="lbl1">Client Remarks :</label>
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" MaxLength="225" placeholder="Our Remarks "></asp:TextBox>
                        </div>
                    </div>
                </div>
                
                <div class="col-md-6" id="divWork1" runat="server">
                    <div>
                        <div class="box-body">
                            <label id="Label15" runat="server" class="col-sm-4 control-label" title="lbl1">Our Remarks :</label>
                            <asp:TextBox ID="txtWrkRmk" runat="server" Width="99%" TextMode="MultiLine" MaxLength="225" placeholder="Our Remarks "></asp:TextBox>
                        </div>
                    </div>
                </div>


                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <div class="form-group">
                                <label id="Label21" runat="server" class="col-sm-4 control-label" title="lbl1">Approx_Value</label>
                                <div class="col-sm-8">
                                    <input id="txtleadval" type="text" class="form-control" runat="server" placeholder="Approx_Value" maxlength="10" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
                



                <div class="col-md-6">
                    <div>
                        <div class="box-body">



                            <div class="form-group">
                                <label id="Label22" runat="server" class="col-sm-4 control-label" title="lbl1">Expense Incurred</label>
                                <div class="col-sm-8">
                                    <input id="txtexpense" type="text" class="form-control" runat="server" placeholder="Expense Incurred" maxlength="10" />
                                </div>
                            </div>


                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">UDF Data</a></li>


                            </ul>

                            <div class="tab-content">


                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 150px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg4" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg4_RowDataBound"
                                            OnRowCommand="sg4_RowCommand">
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
                                                    <HeaderTemplate>UDF_Field</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg4_t1" runat="server" Text='<%#Eval("sg4_t1") %>' Width="100%" ReadOnly="true"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>UDF_Value</HeaderTemplate>
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
