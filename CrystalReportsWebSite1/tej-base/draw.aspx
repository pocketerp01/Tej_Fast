<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="draw" Title="Tejaxo" CodeFile="draw.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function openfileDialog() {
            $("#Attch").click();
        }
        function submitFile() {
            $("#<%= btnAtt.ClientID%>").click();
        };
    </script>

    <%-- <style type="text/css">
        .style1
        {
            height: 21px;
        }
    </style>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>
                        <button type="submit" id="btnsave" class="btn btn-info" style="width: 100px;" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>ave</button>
                        <%--<button type="submit" id="btnprint" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnprint_ServerClick"><u>P</u>rint</button>--%>
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
                                    <asp:Label ID="lbl1a" runat="server" Text="DE" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtdocno" runat="server" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Entry No."></asp:TextBox>
                                </div>
                                <div class="col-sm-5">
                                    <asp:TextBox ID="txtdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtdate_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtdate" Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999" MaskType="Date" TargetControlID="txtdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Customer</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnCust" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnCust_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtAcode" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Code"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtAName" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Name"></asp:TextBox>
                                </div>

                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label19" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Part_no</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btndno" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnctye_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtIcode" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Code"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtdno" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Part Number"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label8" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Part_Name</asp:Label>
                                <div class="col-sm-1">
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtIname" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Product / Part Name"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Revision_No.</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtrno" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="Revision"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtRdt" runat="server" CssClass="form-control" Width="150px" MaxLength="50" TextMode="Date" placeholder="Date"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label9" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">ECN_No.</asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtECNO" runat="server" CssClass="form-control" Width="100%" MaxLength="50" placeholder="ECN No"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <asp:Label ID="Label1" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Drawing_Type</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btndtype" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btndtype_Click" />
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtdtype" runat="server" CssClass="form-control" Width="100%" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label4" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Created_By</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtpre" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Created By"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Modified_By</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtedit" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Modified By"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Remarks<br/>(max 200 char)</asp:Label>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" Width="100%" Height="105px" TextMode="MultiLine" placeholder="Remarks / Information / Specification"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-12" id="divLeadInfo" runat="server">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Lead No. / Date</asp:Label>
                                <div class="col-sm-1">
                                    <asp:TextBox ID="txtLeadNO" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Lead No."></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtLeadDT" runat="server" CssClass="form-control" Width="120px" ReadOnly="true" placeholder="Date"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label10" runat="server" CssClass="col-sm-1 control-label" Font-Size="14px" Font-Bold="True">Subject</asp:Label>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" Width="100%" ReadOnly="true" placeholder="Subject"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label11" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Customer Remarks / Specification</asp:Label>
                                <div class="col-sm-10">
                                    <asp:TextBox ID="txtClientRemarks" runat="server" CssClass="form-control" Width="100%" TextMode="MultiLine" ReadOnly="true" placeholder="Remarks / Specification"></asp:TextBox>
                                </div>
                                <div id="divCustRej" runat="server" style="display: none">
                                    <asp:Label ID="Label12" runat="server" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Last Drawing / Rejection Remarks</asp:Label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtRejRemarks" runat="server" CssClass="form-control" Width="100%" TextMode="MultiLine" ReadOnly="true" placeholder="Remarks / Specification"></asp:TextBox>
                                    </div>
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
                                <li>
                                    <asp:FileUpload ID="Attch" runat="server" Visible="true" onchange="submitFile()" />
                                </li>
                                <li>
                                    <asp:TextBox ID="txtAttch" runat="server" MaxLength="100" placeholder="Path Upto 100 Char" Style="display: none"></asp:TextBox>
                                </li>
                                <li>
                                    <asp:Button ID="btnAtt" runat="server" Text="File" OnClick="btnAtt_Click" Width="50px" Style="display: none" />
                                </li>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="color: White; max-height: 300px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <asp:GridView ID="sg1" Width="100%" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" AutoGenerateColumns="false" OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand">
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>R</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnrmv" runat="server" CommandName="Rmv" ImageUrl="~/tej-base\images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove It" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="srno" HeaderText="Srno" ReadOnly="True">
                                                    <ItemStyle Width="10px" />
                                                    <HeaderStyle Width="10px" />
                                                </asp:BoundField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>D</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btndnlwd" runat="server" CommandName="Dwl" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/tej-base\images/save.png" Width="22px" ImageAlign="Middle" ToolTip="Download file" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>V</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnview" runat="server" CommandName="View" CommandArgument='<%# Eval("FILNO") %>' ImageUrl="~/tej-base\images/preview-file.png" Width="20px" ImageAlign="Middle" ToolTip="View file" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" />
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="filno" HeaderText="File Name" ReadOnly="True"></asp:BoundField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Design Stage</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddStage" runat="server">
                                                            <asp:ListItem Text="RFQ" Value="RFQ"></asp:ListItem>
                                                            <asp:ListItem Text="Development" Value="Development"></asp:ListItem>
                                                            <asp:ListItem Text="Mass_Production" Value="Mass_Production"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="hfStage" runat="server" Value='<%#Eval("stage") %>'></asp:HiddenField>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Design Type</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddDesign" runat="server"></asp:DropDownList>
                                                        <asp:HiddenField ID="hfdesign" runat="server" Value='<%#Eval("design") %>'></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Activation</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddActive" runat="server"></asp:DropDownList>
                                                        <asp:HiddenField ID="hfactive" runat="server" Value='<%#Eval("dactive") %>'></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Downloadable</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddDwnl" runat="server"></asp:DropDownList>
                                                        <asp:HiddenField ID="hfdown" runat="server" Value='<%#Eval("candown") %>'></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <div class="col-md-12" style="display: none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <table>
                                    <tr id="attch1" runat="server">
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>

                                <asp:Label ID="lblShow" runat="server"></asp:Label>
                                <asp:Label ID="lblUpload" runat="server"></asp:Label>

                                <asp:ImageButton ID="btnView1" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="btnView1_Click" Visible="false" />
                                <asp:ImageButton ID="btnDown" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="btnDown_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>

    <asp:Button ID="btnhideF" runat="server" OnClick="btnhideF_Click" Style="display: none" />
    <asp:Button ID="btnhideF_s" runat="server" OnClick="btnhideF_s_Click" Style="display: none" />
    <asp:HiddenField ID="doc_nf" runat="server" />
    <asp:HiddenField ID="doc_df" runat="server" />
    <asp:HiddenField ID="doc_vty" runat="server" />
    <asp:HiddenField ID="doc_addl" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hffield" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <asp:HiddenField ID="lbledmode" runat="server" />
    <asp:HiddenField ID="lblname" runat="server" />
    <asp:HiddenField ID="hfLead" runat="server" />
    <asp:HiddenField ID="HFOPT" runat="server" />
    <asp:HiddenField ID="hf2" runat="server" />
    <asp:HiddenField ID="hfdept" runat="server" />
    <asp:HiddenField ID="hfbtnmode" runat="server" />
    <asp:HiddenField ID="TabName" runat="server" />
    <asp:Button ID="btnOKTarget" runat="server" Text="!" OnClick="btnOKTarget_Click"
        Style="display: none;" />
    <asp:Button ID="btnCancelTarget" runat="server" Text="!" OnClick="btnCancelTarget_Click"
        Style="display: none;" />
    <script type="text/javascript">
        $(function () {
            var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
            $('#Tabs a[href="#' + tabName + '"]').tab('show');
            $("#Tabs a").click(function () {
                $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
            });
        });
    </script>
    <asp:HiddenField ID="HiddenField1" runat="server" />
</asp:Content>
