<%@ Page Title="" Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="frmdrawIss" EnableEventValidation="false" CodeFile="frmdrawIss.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {

            $("[id$=sg1_t1]").datepicker({
                showOn: "button",
                buttonImage: "css/images/calendar.gif",
                buttonImageOnly: true,
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                autoSize: true,
                showAnim: "fold"
            });

            $("[id$=sg1_t2]").datepicker({
                showOn: "button",
                buttonImage: "css/images/calendar.gif",
                buttonImageOnly: true,
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                autoSize: true,
                showAnim: "fold"
            });


        });


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
                        <%--<button type="submit" id="btnReport" class="btn btn-info" style="width: 100px;" runat="server" accesskey="P" onserverclick="btnReport_ServerClick"><u>R</u>eport</button>--%>
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
                        <div class="box-body" id="div1" runat="server">
                            <%-- <div class="form-group">
                                <label id="Label11" runat="server" class="col-sm-4 control-label" title="lbl1"><u>Request/Complaint Section</u></label>
                            </div>--%>
                            <div class="form-group">

                                <label id="lbl1" runat="server" class="col-sm-4 control-label" title="lbl1">Entry No</label>

                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtvchnum" runat="server" ReadOnly="true" placeholder="Entry No"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Request Slip</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnReq" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnReq_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtreqno" runat="server" placeholder="Request No"
                                        ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtreqdt" runat="server" placeholder="Request Date"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="lbl3" runat="server" class="col-sm-3 control-label" title="lbl1">Users</label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnuser" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnuser_Click" />
                                </div>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="TxtUserCode" ReadOnly="true" placeholder="User ID" runat="server" />
                                    <asp:TextBox ID="txtuser" runat="server" ReadOnly="true" placeholder="UserName"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <div class="box-body" id="div2" runat="server">
                            <%--<div class="form-group">
                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Action Taken Section</label>
                            </div>--%>
                            <div class="form-group">
                                <label id="lbl4" runat="server" class="col-sm-3 control-label" title="lbl1">Entry Date</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtvchdate" runat="server" placeholder="Entry Date" ReadOnly="true"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                        Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtvchdate">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label1" runat="server" class="col-sm-3 control-label" title="lbl1">Created By</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtentby" runat="server" placeholder="Created By"
                                        MaxLength="20" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtentdt" runat="server" placeholder="Created Date"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Modified By</label>
                                <div class="col-sm-8">
                                    <asp:TextBox ID="txtedtby" runat="server" placeholder="Modified By"
                                        MaxLength="20" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtedtdt" runat="server" placeholder="Modified Date"
                                        ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>


                <section class="col-lg-12 connectedSortable" style="display: none">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Form Details</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Comm.Terms</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Addl.Terms</a></li>
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Delv.Sch</a></li>
                                <li><a href="#DescTab5" id="tab5" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inv.Dtl</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div>
                                                <div class="box-body">
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                            <div class="col-md-6">
                                                <div>
                                                    <div class="box-body">
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div>
                                                    <div class="box-body">
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab4" id="A4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Select Items</a></li>
                            </ul>

                            <div role="tabpanel" class="tab-pane-active" id="DescTab4">
                                <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                    <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                        Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="Small"
                                        AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                        OnRowCommand="sg1_RowCommand">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>

                                            <asp:TemplateField>
                                                <HeaderTemplate>A</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>D</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="sg1_Srno" HeaderText="Sr.No" />
                                            <asp:BoundField DataField="sg1_f1" HeaderText="Code" />
                                            <asp:BoundField DataField="sg1_f2" HeaderText="Name" />
                                            <asp:BoundField DataField="sg1_f3" HeaderText="Unit" />
                                            <asp:BoundField DataField="sg1_f4" HeaderText="Rate" />
                                            <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5"></asp:BoundField>
                                            <asp:BoundField DataField="sg1_f6" HeaderText="sg1_f6"></asp:BoundField>
                                            <asp:BoundField DataField="sg1_f7" HeaderText="sg1_f7"></asp:BoundField>
                                            <asp:TemplateField ItemStyle-CssClass="chk1">
                                                <HeaderTemplate>
                                                    sg1_chkbox1
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="sg1_chkbox1" runat="server" Width="30PX" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    sg1_chkbox2
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="sg1_chkbox2" runat="server" Style="text-align: center" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Issue Start Date</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t1" runat="server" MaxLength="10" Text='<%#Eval("sg1_t1") %>' placeholder="Issue Start Date" onpaste="return false" Width="100%" TextMode="Date"></asp:TextBox>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <HeaderTemplate>Issue End Date</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="10" placeholder="Issue End Date" onpaste="return false" TextMode="Date"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>Start Time</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%"
                                                        MaxLength="10" onpaste="return false"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="Maskededitextender3" runat="server" Mask="99:99" MaskType="Time"
                                                        TargetControlID="sg1_t3" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>End Time</HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%"
                                                        MaxLength="10" onpaste="return false"></asp:TextBox>
                                                    <cc1:MaskedEditExtender ID="Maskededitextender4" runat="server" Mask="99:99" MaskType="Time"
                                                        TargetControlID="sg1_t4" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Issue Time
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="sg1_fstr" runat="server" Text='<%#Eval("sg1_fstr") %>'
                                                        MaxLength="10" placeholder="Issue Time" Visible="false" onpaste="return false" Style="text-align: Right"></asp:TextBox>
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
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div>
                                <div class="box-body" id="div3" runat="server">
                                    <div class="form-group">
                                        <label id="Label5" runat="server" class="col-sm-1 control-label" title="lbl1">Remarks(100 Char)</label>
                                        <div class="col-sm-11">
                                            <asp:TextBox ID="txtrmk" Width="100%" placeholder="Enter Remarks" runat="server"
                                                MaxLength="100" Height="70px"></asp:TextBox>
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
    <input id="pwd1" runat="server" style="display: none" />
    <asp:HiddenField ID="hfGridView1SV" runat="server" />
    <asp:HiddenField ID="hfGridView1SH" runat="server" />
</asp:Content>
