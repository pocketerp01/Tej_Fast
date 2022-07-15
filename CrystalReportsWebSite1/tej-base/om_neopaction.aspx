<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_neopaction" CodeFile="om_neopaction.aspx.cs" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../tej-base/Scripts/gridviewScroll.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //  gridviewScroll('#<%=sg1.ClientID%>', gridDiv, 1, 5);

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


        <section class="col-lg-12 connectedSortable">
            <div class="panel panel-default">
                <div id="Tabs" role="tabpanel">
                    <ul class="nav nav-tabs" role="tablist">
                        <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Action Taken</a></li>
                        <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Complaint Information</a></li>
                    </ul>

                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="DescTab">

                            <div class="col-md-12">
                                <div>
                                    <div class="box-body">
                                        <div class="form-group">
                                            <asp:Label ID="Label23" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Party</asp:Label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtsacode" runat="server" EnableViewState="true" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtsaname" runat="server" EnableViewState="true" Width="100%" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label ID="Label1" runat="server" Text="Item Name" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <div class="col-sm-2">
                                                <asp:TextBox ID="txtsicode" runat="server" EnableViewState="true" ReadOnly="true" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-7">
                                                <asp:TextBox ID="txtsiname" runat="server" EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                            </div>
                                        </div>                                     

                                        <div class="form-group">
                                            <asp:Label ID="tdrply" runat="server" Text="Reply to Customer" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtrply" runat="server" EnableViewState="true" Width="100%" CssClass="form-control"  TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label ID="Label3" runat="server" Text="Action Taken" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtcorrective" runat="server" Placehoder="Corrective Action" EnableViewState="true" Width="100%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label ID="Label4" runat="server" Text="Corrective &amp; Preventive Action" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtpreventive" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label ID="Label5" runat="server" Text="Fact Finding" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtfact" runat="server" Placehoder="Fact Finding" EnableViewState="true" Width="100%" CssClass="form-control"  TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group" runat="server" id="SEL1">
                                            <label class="col-sm-3 control-label" id="Label17" runat="server">Information to Our Team</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtInform" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-sm-3 control-label">
                                            <asp:Label ID="Label6" runat="server" Text="Entry No"  Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px;"></asp:Label>
                                                </div>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtvchnum" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <asp:Label ID="Label7" runat="server" Text="Entry Dt" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True"></asp:Label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtvchdate" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtvchdate" Format="dd/MM/yyyy">
                                                </asp:CalendarExtender>
                                                <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                    MaskType="Date" TargetControlID="txtvchdate" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <asp:Label ID="Label8" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Status</asp:Label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="dd1" runat="server" CssClass="form-control" Width="100%">
                                                    <asp:ListItem Text="Closed" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group" runat="server" id="SEL2">
                                            <label class="col-sm-3 control-label">Cost(If any)</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" Width="100%" MaxLength="20" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                            </div>
                                            <label class="col-sm-3 control-label">Our Person</label>
                                            <div class="col-sm-3">
                                                <asp:TextBox ID="txtPerson" runat="server" CssClass="form-control" Height="28px" MaxLength="30"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div role="tabpanel" class="tab-pane active" id="DescTab2">
                            <div class="lbBody" style="height: 200px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                <div class="col-md-6">
                                    <div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <asp:Label ID="tdinvoice" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">InvoiceNo</asp:Label>
                                                <div class="col-sm-3">
                                                    <asp:TextBox ID="txtinvno" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                                <asp:Label ID="tdinvoiceDT" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">InvoiceDt</asp:Label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtinvdate" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                    <%--</div>
                                     <div class="col-sm-1">--%>
                                                    <span id="spnjobno" runat="server"><b>Job_No</b></span>
                                                    <%--</div>
                                     <div class="col-sm-2">--%>
                                                    <asp:TextBox ID="txtjobno" runat="server" ReadOnly="true" Width="100%" CssClass="form-control" Placeholder="JobNo."></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="Label11" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Party</asp:Label>
                                                <div class="col-sm-3">
                                                    <asp:TextBox ID="txtacode" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtaname" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="Label12" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Item</asp:Label>
                                                <div class="col-sm-3">
                                                    <asp:TextBox ID="txticode" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-7">
                                                    <asp:TextBox ID="txtiname" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>

                                               <div class="form-group" runat="server" id="DivAddress">
                                                        <label class="col-sm-2 control-label">Address</label>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtPaddr" runat="server" ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                 <div class="form-group" runat="server" id="tdbatch1">
                                                        <label class="col-sm-2 control-label" id="lblBatch" runat="server">Batch No.</label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtinvbtch" runat="server" ReadOnly="True" CssClass="form-control" Height="32px"></asp:TextBox>
                                                        </div>
                                                        <label class="col-sm-3 control-label"></label>
                                                        <div class="col-sm-4">
                                                        </div>
                                                    </div>

                                            <div class="form-group" runat="server" id="MadeBy">
                                                <asp:Label ID="Label19" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">MadeBy</asp:Label>
                                                <div class="col-sm-3">
                                                    <asp:TextBox ID="txtent_by" runat="server" EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                                <asp:Label ID="Label2" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtent_dt" runat="server" EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div>
                                        <div class="box-body">
                                            <div class="form-group">
                                                <asp:Label ID="Label13" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Complaint_No</asp:Label>
                                                <div class="col-sm-2">
                                                    <asp:TextBox ID="txtcvchnum" runat="server" EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                                <asp:Label ID="Label14" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Complaint_Dt</asp:Label>
                                                <div class="col-sm-4">
                                                    <asp:TextBox ID="txtcvchdate" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="Label15" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">TypeofComplaint</asp:Label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="ddntrofcmlnt" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>


                                            <div class="form-group">
                                                <asp:Label ID="Label16" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">NatureofComplaint</asp:Label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtntrcmpln" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <asp:Label ID="tddivision" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">DivisonofComplaint</asp:Label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="dddivisioncmltn" runat="server"  EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="30" ReadOnly="True"></asp:TextBox>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                    Style="background-color: #FFFFFF; color: White;" Width="100%" Height="280px" Font-Size="13px"
                                    AutoGenerateColumns="False" OnRowDataBound="sg1_RowDataBound" OnRowCommand="sg1_RowCommand">
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
                                        <asp:TemplateField>
                                            <HeaderTemplate>Add</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Tag" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Del</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Tag" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="sg1_srno" HeaderText="SrNo" />
                                        <asp:BoundField DataField="sg1_f1" HeaderText="Application" />
                                        <asp:BoundField DataField="sg1_f2" HeaderText="Icode" Visible="false" />
                                        <asp:BoundField DataField="sg1_f3" HeaderText="ERPcode" Visible="false" />
                                        <asp:BoundField DataField="sg1_f4" HeaderText="Name" Visible="false" />
                                        <asp:BoundField DataField="sg1_f5" HeaderText="Unit" Visible="false" />
                                        <asp:BoundField DataField="sg1_f6" HeaderText="QtyIssue" Visible="false" />
                                        <asp:TemplateField  HeaderStyle-Width="850px">
                                            <HeaderTemplate>Remarks</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' Width="100%" MaxLength="175" ReadOnly="True"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>Identification_Tag_No</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t2" Visible="false" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" ReadOnly="true" MaxLength="30"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>Heat_No</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t3" Visible="false" runat="server" Text='<%#Eval("sg1_t3") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Job_Description</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t4" Visible="false" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="100"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>Size_Of_Indication</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t5" Visible="false" runat="server" Text='<%#Eval("sg1_t5") %>' Width="100%" MaxLength="50"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Interpretation</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t6" Visible="false" runat="server" Text='<%#Eval("sg1_t6") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>Remarks</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t7" Visible="false" runat="server" Text='<%#Eval("sg1_t7") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="sg1_t8" Visible="false" runat="server" Text='<%#Eval("sg1_t8") %>' Width="100%"></asp:TextBox>
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

                            <div class="col-md-12">
                                <div>
                                    <div class="box-body">

                                        <div class="form-group">
                                            <asp:Label ID="Label18" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Remarks</asp:Label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="txtrmk" runat="server" EnableViewState="true" Width="100%" CssClass="form-control" MaxLength="150" ReadOnly="True"></asp:TextBox>
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
