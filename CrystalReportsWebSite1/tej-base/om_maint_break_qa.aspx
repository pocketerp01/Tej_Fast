<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_maint_break_qa" CodeFile="om_maint_break_qa.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                         <%--<div class="box-body">--%>
                         <div class="box-body">
                             <div class="form-group">
                                 <asp:Label ID="lbl1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-1">
                                     <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-2 control-label"></asp:Label>
                                 </div>
                                 <div class="col-sm-3">
                                     <asp:TextBox ID="txtvchnum" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                                 <div class="col-sm-5">
                                            <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control" Font-Size="Small"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                        </div>
                             </div>
                             <div class="form-group">
                                 <asp:Label ID="lbl4" runat="server" Text="lbl4" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-1">
                                     <asp:ImageButton ID="btnlbl4" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl4_Click" />
                                 </div>
                                 <div class="col-sm-3">
                                     <asp:TextBox ID="txtlbl4" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                                 <div class="col-sm-5">
                                     <asp:TextBox ID="txtlbl4a" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                             </div>

                             <div class="form-group" style="display:none;">  <%--//ACREF--%>
                                 <asp:Label ID="Label1" runat="server" Text="lbl9" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-3" style="display:none;">
                                     <asp:TextBox ID="txtlbl9" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                                 <div class="col-sm-5" style="display:none;">
                                     <asp:TextBox ID="txtlbl9a" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                             </div>
                             
                           <div class="form-group">
                                 <asp:Label ID="lbl7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-1">
                                     <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                 </div>
                                 <div class="col-sm-3">
                                     <asp:TextBox ID="txtlbl7" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                                 <div class="col-sm-5">
                                     <asp:TextBox ID="txtlbl7a" runat="server" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                 </div>
                             </div>
                             <div class="form-group">
                                 <asp:Label ID="lbl20" runat="server" Text="lbl20" CssClass="col-sm-4 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-3">
                                 <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" ReadOnly="false" placeholder="DD/MM/YYYY" Width="100%" TextMode="Date" Font-Size="Small"></asp:TextBox>
                                 </div>
                                  
                                 <div class="col-sm-5">
                                 <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" ReadOnly="false" placeholder="HH/MM/SS" Width="100%" TextMode="Time"></asp:TextBox>
                                 </div>
                                 </div>
                                                        
                        </div>
                    </div>
                </div>

             <div class="col-md-6">
                    <div>
                         <%--<div class="box-body">--%>
                         <div class="box-body">
                             <div class="form-group">
                                 <asp:Label ID="lbl5" runat="server" Text="lbl5" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-9">
                                 <asp:TextBox ID="txtlbl5" runat="server" CssClass="form-control" Width="80%" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                 </div>

                                
                             </div>
                             <div class="form-group">
                                 <asp:Label ID="lbl8" runat="server" Text="lbl8" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-9">
                                     <asp:TextBox ID="txtlbl8" runat="server" CssClass="form-control" Width="80%"></asp:TextBox>                                                                      
                                 </div>
                             </div>
                             <div class="form-group">
                                 <asp:Label ID="lblResult" runat="server" Text="lblResult" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                 <div class="col-sm-9">
                                     <asp:TextBox ID="txtResult" runat="server" CssClass="form-control" Width="80%" MaxLength="1" placeholder="Enter Y For Accept , N For Reject"></asp:TextBox>                                                                      
                                 </div>

                             </div>
                             <asp:Label ID="lbl2" runat="server"  CssClass="col-sm-12 control-label" ForeColor="Black" Font-Size="20px" Font-Bold="True"></asp:Label>

                        </div>
                    </div>
                </div>

             <div class="col-md-12">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server"  MaxLength="150"  Width="99%" TextMode="MultiLine" placeholder="Remarks"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable" id="AllTabs" runat="server">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">List</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <asp:GridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Font-Size="13px">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
    <asp:HiddenField ID="lstvch1" runat="server" />
    <asp:HiddenField ID="lstvch2" runat="server" />
    <asp:HiddenField ID="edmode" runat="server" />
    <asp:HiddenField ID="hf1" runat="server" />
    <input id="pwd1" runat="server" style="display: none" />
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
