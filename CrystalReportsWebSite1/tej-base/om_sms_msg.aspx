<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_sms_msg" EnableEventValidation="false" CodeFile="om_sms_msg.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
        });
        function openfileDialog() {
            $("#FileUpload2").click();
        }
        function submitFile() {
            $("#<%= BtnAttach2.ClientID%>").click();
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                   
                    <td style="text-align: left">
                        <button type="submit" id="btnnew" class="btn btn-info" style="width: 100px;" runat="server" accesskey="N" onserverclick="btnnew_ServerClick"><u>N</u>ew</button>
                        <%--<button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="i" onserverclick="btnedit_ServerClick">Ed<u>i</u>t</button>--%>
                        <button type="submit" id="btnsave" class="btn btn-info" runat="server" accesskey="s" onserverclick="btnsave_ServerClick"><u>S</u>end Message</button>
                        <button type="submit" id="Btnhisout" class="btn btn-info" runat="server" onserverclick="Btnhisout_ServerClick">H<u>i</u>story Outbox</button>
                        <button type="submit" id="BtnhisIn" class="btn btn-info" runat="server" onserverclick="BtnhisIn_ServerClick">H<u>i</u>story Inbox</button>
                        <button type="submit" id="btncancel" class="btn btn-info" style="width: 100px;" runat="server" accesskey="c" onserverclick="btncancel_ServerClick">C<u>a</u>ncel</button>
                        <button type="submit" id="btnedit" class="btn btn-info" style="width: 100px;" runat="server" onserverclick="btnedit_ServerClick">U<u>p</u>date</button>


                        <button type="submit" id="btnexit" class="btn btn-info" style="width: 100px;" runat="server" accesskey="X" onserverclick="btnexit_ServerClick">E<u>x</u>it</button>
                    </td>
                     <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large">Tejaxo ERP User's Messaging System</asp:Label>

                        <%--<img src="../tej-base/images/shopworkload.jpeg" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="Small">This Screen is Used to Send and Receive ERP-Wide Mails</asp:Label></td>
                </tr>
            </table>
        </section>
    </div>
    <div class="content-wrapper">

        <section class="content">
            <div class="row">
                <div class="col-lg-6 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab4" id="A4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Inbox(Incoming Messages)</a></li>
                                <%--<button type="submit" id="btnTraExc" class="btn-success" style="width: 150px; float: right" runat="server" onserverclick="btnTraExc_ServerClick1">Transfer To Excel</button></li>--%>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Height="450px" Width="100%" Font-Size="Small"
                                            AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound"
                                            OnRowCommand="sg1_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>OK</HeaderTemplate>
                                                    <HeaderStyle Width="20px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chk" AutoPostBack="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:BoundField DataField="sg1_Srno" HeaderText="Sr.No" ItemStyle-HorizontalAlign="right" />--%>
                                                <asp:BoundField DataField="sg1_f1" HeaderText="From" ItemStyle-Height="10px" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Subject" ItemStyle-Height="10px" HeaderStyle-Width="280px" />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="Date" ItemStyle-Height="10px" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="Post" ItemStyle-Height="10px" HeaderStyle-Width="120px" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="File" ItemStyle-Height="10px" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f6" HeaderText="Sender" ItemStyle-Height="10px" />

                                                <%--<asp:CommandField SelectText="Select" ShowSelectButton="true" Visible="false" />--%>
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>


                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-lg-6 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs1" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab5" id="A5" runat="server" aria-controls="DescTab5" role="tab" data-toggle="tab">Outbox(Outgoing Messages)</a></li>

                                <%--<button type="submit" id="btnTraExc" class="btn-success" style="width: 150px; float: right" runat="server" onserverclick="btnTraExc_ServerClick1">Transfer To Excel</button></li>--%>
                            </ul>
                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab5">
                                    <div style="height: 450px">
                                        <div class="box-body" id="div1" runat="server">
                                            <div class="form-group">
                                                <label id="Label4" runat="server" class="col-sm-3 control-label" title="lbl1">Msg No.</label>

                                                <div class="col-sm-8">
                                                    <input style="height: 28px" id="MsgNo" type="text" class="form-control" runat="server" placeholder="" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label id="Label26" runat="server" class="col-sm-2 control-label" title="lbl1">To</label>
                                                <div class="col-sm-1">
                                                    <asp:ImageButton ID="BtnTo" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="BtnTo_Click" />
                                                </div>
                                                <div class="col-sm-8">
                                                    <input style="height: 28px; display: none;" id="TxtToCode" type="text" runat="server" />
                                                    <input style="height: 28px" id="TxtTo" type="text" class="form-control" runat="server" placeholder="" readonly />

                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label id="Label2" runat="server" class="col-sm-2 control-label" title="lbl1">CC</label>
                                                <div class="col-sm-1">
                                                    <asp:ImageButton ID="BtnCC" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="BtnCC_Click" />
                                                </div>
                                                <div class="col-sm-8">
                                                    <input style="height: 28px; display: none;" id="TxtCCCode" type="text" runat="server" />
                                                    <input style="height: 28px" id="TxtCC" type="text" class="form-control" runat="server" placeholder="" readonly />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label id="Label21" runat="server" class="col-sm-3 control-label" title="lbl1">Attach</label>

                                                <div class="col-sm-9">
                                                    <table>
                                                        <tr id="Tr1" runat="server">
                                                            <td>
                                                                <asp:FileUpload ID="FileUpload2" runat="server" Visible="true" onchange="submitFile()" /></td>
                                                            <td>
                                                                <asp:TextBox ID="TxtAttach2" runat="server" MaxLength="100" placeholder="Path Upto 100 Char"></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                    <asp:Button ID="BtnAttach2" runat="server" Text="File" OnClick="BtnAttach2_Click" Width="50px" Style="display: none" />
                                                    <%--Style="display: none"--%>
                                                    <asp:Label ID="Label19" runat="server"></asp:Label>
                                                    <asp:Label ID="Label20" runat="server"></asp:Label>
                                                    <asp:ImageButton ID="BtnView2" runat="server" ImageUrl="~/tej-base/images/preview-file.png" OnClick="BtnView2_Click" Visible="false" />
                                                    <asp:ImageButton ID="BtnDown2" runat="server" ImageUrl="~/tej-base/images/Save.png" OnClick="BtnDown2_Click" Visible="false" />
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label id="Label3" runat="server" class="col-sm-3 control-label" title="lbl1">Message</label>

                                                <div class="col-sm-9">
                                                    <textarea runat="server" id="TxtMsg" rows="13" cols="60" maxlength="350"></textarea>
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
    <script type="text/javascript">
        //$(function () {
        //    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "DescTab";
        //    $('#Tabs a[href="#' + tabName + '"]').tab('show');
        //    $("#Tabs a").click(function () {
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //});
    </script>
    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
