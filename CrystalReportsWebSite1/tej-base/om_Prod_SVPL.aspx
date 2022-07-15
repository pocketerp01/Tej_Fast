<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_Prod_SVPL" CodeFile="om_Prod_SVPL.aspx.cs" %>

<%--  --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            cal();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Label ID="lblheader" runat="server" Font-Bold="True" Font-Size="X-Large"></asp:Label>
                        (<asp:Label ID="lblname" runat="server" Text="Shift and Line Details" Font-Bold="True" Font-Size="Larger"></asp:Label>)
                    </td>
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
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="lbl1" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Date</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtvchdate" placeholder="Date" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                        Enabled="True" TargetControlID="txtvchdate"
                                        Format="dd/MM/yyyy">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="lbl3" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Shift</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="dd_Stgtest" runat="server" TabIndex="11" Width="100%" Height="30px" OnSelectedIndexChanged="dd_Stgtest_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <asp:Label ID="Label19" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Display_of_Shift_Time</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtdisptime" runat="server" ReadOnly="true" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="100" onkeyup="cal()"></asp:TextBox>
                                </div>

                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label3" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Zone</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnzone" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnzone_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtzcode" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtzname" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label21" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Line</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnline" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnline_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlinecode" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtine" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label22" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Part_Name</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnpart" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnpart_Click" Visible="false" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtpartcode" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtpart" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>




                            <div class="form-group">
                                <asp:Label ID="Label20" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">DownTime_Min </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtloss" runat="server" Width="100%" ReadOnly="true" Style="text-align: right" onkeypress="return isDecimalKey(event)" CssClass="form-control" autocomplete="off" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label23" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Tot_Rejection </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txttot_rej" runat="server" Width="100%" ReadOnly="true" Style="text-align: right" onkeypress="return isDecimalKey(event)" CssClass="form-control" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">

                            <%--                           <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">MC</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtmc" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtmcname" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="form-group">
                                <asp:Label ID="Label10" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">ShiftI/C</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnshift" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnshift_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtshifcode" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtshiftname" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="lbl9" CssClass="col-sm-2 control-label" Font-Size="14px" Font-Bold="True">Supervisor</asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnsupr" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnsupr_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtsupcode" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtsupname" runat="server" Width="100%" CssClass="form-control" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Capacity</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtcapacity" runat="server" Width="100%" ReadOnly="true" Style="text-align: right" onkeypress="return isDecimalKey(event)" CssClass="form-control" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label16" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Line_Efficiency(%)</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtline_Eff" runat="server" Width="100%" ReadOnly="true" Style="text-align: right" onkeypress="return isDecimalKey(event)" CssClass="form-control" onkeyup="cal()"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label17" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">UnAccounted_loss_Min </asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtdisp" runat="server" Width="100%" ReadOnly="true" Style="text-align: right" onkeypress="return isDecimalKey(event)" CssClass="form-control" onkeyup="cal()"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl9" runat="server" Text="lbl9" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Bottle_Neck_CT</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtgct" runat="server" Width="100%" CssClass="form-control" MaxLength="40" Height="35px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">OK_Prod_Qty</asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtprodqty" runat="server" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="10" autocomplete="off"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label13" runat="server" Text="lbl3" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Shift_Close_Status</asp:Label>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="dd_Shift_Status" runat="server" TabIndex="11" Width="100%" Height="30px"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Manpower Detail</a></li>
                                <li><a href="#DescTab2" id="tab2" runat="server" aria-controls="DescTab2" role="tab" data-toggle="tab">Rejection Entry</a></li>
                                <li><a href="#DescTab3" id="tab3" runat="server" aria-controls="DescTab3" role="tab" data-toggle="tab">Down Time Entry</a></li>
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="250px" Font-Size="13px"
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

                                                <%--   <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="../tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Operation" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="../tej-base/images/Btn_remn.png" ImageAlign="Middle" Width="20px" ToolTip="Remove Tag" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <asp:BoundField DataField="sg1_srno" ItemStyle-Width="20px" HeaderText="SrNo" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="Machine" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="Op_code" ItemStyle-Width="80px" />

                                                <%--  <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd1" runat="server" CommandName="SG1_ROW_OP" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Operator" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <asp:BoundField DataField="sg1_f4" HeaderText="Process_name" ItemStyle-Width="80px" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="Cycle_time" ItemStyle-Width="80px" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Employee_Code</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' MaxLength="20" Width="100%" autocomplete="off"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--                                                 <asp:TemplateField>
                                                    <HeaderTemplate>Operator_Name</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' MaxLength="20" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                 <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Cycle_Time</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" ReadOnly="true" onkeypress="return isDecimalKey(event)" Text='<%#Eval("sg1_t3") %>' MaxLength="20" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <%--<asp:TemplateField ItemStyle-Width="140px" Visible="false">
                                                    <HeaderTemplate>Pmi_Test_Sequence_No.</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' Width="100%" MaxLength="30"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Result</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg1_t10" runat="server" Width="100%">
                                                            <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="ACCEPTANCE" Value="ACCEPTANCE"></asp:ListItem>
                                                            <asp:ListItem Text="MISMATCH" Value="MISMATCH"></asp:ListItem>
                                                            <asp:ListItem Text="REJECT" Value="REJECT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:HiddenField ID="cmd1" Value='<%#Eval("sg1_t10") %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
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

                                <div role="tabpanel" class="tab-pane active" id="DescTab2">
                                    <div class="lbBody" style="height: 310px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3); box-shadow: inset 0 0 3px #387bbe,0 0 9px #387bbe;">
                                        <div class="col-md-12">
                                            <div>
                                                <div class="box-body">
                                                    <div class="form-group">
                                                        <%--<asp:Label ID="Label11" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Prod_Qty</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtprodqty" runat="server"  onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
                                                        </div>--%>
                                                        <asp:Label ID="Label7" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Mch_Rejection</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtrej" runat="server" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="8" autocomplete="off" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label ID="Label9" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Casting_Rejcetion</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtcastrej" runat="server" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="8" autocomplete="off" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                        <asp:Label ID="Label18" runat="server" Text="lbl7" onkeypress="return isDecimalKey(event)" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Unprocessed_Rejection</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtunproc_rej" runat="server" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="8" autocomplete="off" onkeyup="cal()"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <%--  <div class="form-group">
                                                        <asp:Label ID="Label19" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Display_of_Shift_Time</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtdisptime" runat="server" onkeypress="return isDecimalKey(event)" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
                                                        </div>
                                                    --%>
                                                    <%-- <asp:Label ID="Label20" runat="server" Text="lbl7" CssClass="col-sm-3 control-label" Font-Size="14px" Font-Bold="True">Display_UnA/c_loss Min</asp:Label>
                                                        <div class="col-sm-3">
                                                            <asp:TextBox ID="txtdisp_loss" runat="server" CssClass="form-control" Width="100%" MaxLength="100"></asp:TextBox>
                                                        </div>--%>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>


                                <div role="tabpanel" class="tab-pane active" id="DescTab3">
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg2" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="1200px" Font-Size="Smaller"
                                            AutoGenerateColumns="False" OnRowDataBound="sg2_RowDataBound"
                                            OnRowCommand="sg2_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnadd" runat="server" CommandName="SG2_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_btnrmv" runat="server" CommandName="SG2_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_srno" HeaderText="Sr.No." />
                                                <asp:BoundField DataField="sg2_f1" HeaderText="Category" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg2_add" runat="server" CommandName="SG2_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg2_f3" HeaderText="Detail" ItemStyle-Width="230" HeaderStyle-Width="230" />
                                                <asp:BoundField DataField="sg2_f4" HeaderText="Loss_Code" />
                                                <asp:BoundField DataField="sg2_f5" HeaderText="CATG_CODE" />

                                                <%--  <asp:TemplateField>
                                                    <HeaderTemplate>Category</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t1" runat="server" MaxLength="25" Text='<%#Eval("sg2_t1") %>' Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                  <asp:TemplateField>
                                                    <HeaderTemplate>Detail</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t2" runat="server" MaxLength="150" Text='<%#Eval("sg2_t2") %>' Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                  <asp:TemplateField>
                                                    <HeaderTemplate>Loss_Code</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t3" runat="server" MaxLength="70" Text='<%#Eval("sg2_t3") %>' Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Time In Min</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg2_t4" runat="server" Text='<%#Eval("sg2_t4") %>' MaxLength="25" onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="cal()" autocomplete="off"></asp:TextBox>
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

        function cal() {
            var t1 = 0; var t2 = 0; var cap = 0; var prod = 0; var tot_Rej = 0; var cast_Rej = 0; var un_Rej = 0; var rej = 0; var line = 0;

            rej = fill_zero(document.getElementById("ContentPlaceHolder1_txtrej").value);
            un_Rej = fill_zero(document.getElementById("ContentPlaceHolder1_txtunproc_rej").value);
            cast_Rej = fill_zero(document.getElementById("ContentPlaceHolder1_txtcastrej").value);
            tot_Rej = (rej * 1) + (un_Rej * 1) + (cast_Rej * 1);//tot rejection
            document.getElementById('ContentPlaceHolder1_txttot_rej').value = (tot_Rej * 1).toFixed(2);

            //================
            var grid = $("[id*=sg2].GridviewScrollItem2").length - 1;
            var gridid = document.getElementById("<%= sg2.ClientID%>");
            for (var i = 0; i < grid; i++) {
                t1 += document.getElementById('ContentPlaceHolder1_sg2_sg2_t4_' + i).value * 1;
            }
            document.getElementById('ContentPlaceHolder1_txtloss').value = (t1 * 1).toFixed(1);

            //===================
            ////display UNA/C LOSS MIN FORMULA========'***=(Shift time-Total loss time entered above-(  Prod Qty * CT based on Part No, Line No/60))
            shift = fill_zero(document.getElementById("ContentPlaceHolder1_txtdisptime").value);
            prod = fill_zero(document.getElementById("ContentPlaceHolder1_txtprodqty").value);
            var grid = $("[id*=sg1].GridviewScrollItem2").length;
            var gridid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < grid - 1 ; i++) {
                var rowI = 0;
                if (i == 0) rowI = 0;
                else rowI = i * 2;

                t2 += (document.getElementById("ContentPlaceHolder1_sg1").rows[rowI].cells[14].innerHTML * 1);
            }
            LOSS = fill_zero(document.getElementById("ContentPlaceHolder1_txtloss").value);
            debugger;
            //========capacity formulla===###=(Shift time-50)*60/(CT based on "Part No", "Line No" from "Ip table 1" with "bottleneck Indicator" as "B")
            CT = fill_zero(document.getElementById("ContentPlaceHolder1_txtgct").value);
            shift1 = ((shift * 1) - 50) / (CT * 1);
            document.getElementById('ContentPlaceHolder1_txtcapacity').value = (shift1 * 1).toFixed(2);
            prod1 = ((shift * 1) - (LOSS * 1) - ((prod * 1) * (CT * 1)));
            document.getElementById('ContentPlaceHolder1_txtdisp').value = (fill_zero(prod1 * 1)).toFixed(2);

            //======line efficieny frmua===$$$=Production Qty/(Capacity as calculated above*85%)        
            cap = fill_zero(document.getElementById("ContentPlaceHolder1_txtcapacity").value);
            line = ((prod * 1) / ((cap * 1) * 85 / 100)) * 100;
            document.getElementById('ContentPlaceHolder1_txtline_Eff').value = (line * 1).toFixed(2);

        }

        function fill_zero(val) { if (isNaN(val)) return 0; if (isFinite(val)) return val; }

    </script>
    <asp:HiddenField ID="TabName" runat="server" />

</asp:Content>
