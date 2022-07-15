<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master2.master" AutoEventWireup="true" Inherits="om_inc_ded" CodeFile="om_inc_ded.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
        });        
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
                                <asp:Label ID="Opt_No" runat="server" Text="Entry_No"  CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox id="txtvchnum" type="text" CssClass="form-control" runat="server" readonly="true" />
                                </div>
                                                                                      
                                <asp:Label ID="Opt_Date" runat="server" Text="Entry_Date"  CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox id="txtvchdate" type="text" CssClass="form-control" runat="server" Width="100%" />
                                    <asp:CalendarExtender ID="txtvchdate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtvchdate"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtvchdate" />
                                </div>
                            </div>
                            <div class="form-group">
                                           <asp:Label ID="Label5" runat="server" Text="Grade"  CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btngrade" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btngrade_Click" />
                                    </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtgrade" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtgradenm" runat="server" ReadOnly="true" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                    </div>
                                    </div>
                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="Eff_From"  CssClass="col-sm-3 control-label"></asp:Label>
                                
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtlbl4_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtlbl4"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEdit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl4" />
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="Eff_upto"  CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl4a" runat="server" CssClass="form-control" Width="100%"  MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtlbl4a_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtlbl4a"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl4a" />
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl7" runat="server" Text="Weekly Off"  CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl7" runat="server" CssClass="form-control" MaxLength="5" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <asp:TextBox ID="txtlbl7a" runat="server" CssClass="form-control" Width="100%" MaxLength="15" ReadOnly="true"></asp:TextBox>
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
                                <asp:Label ID="lbl2" runat="server" Text="Min_Working_Hrs/Day"  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" Width="100%" MaxLength="4" onkeypress="return isDecimalKey(event)"></asp:TextBox>                                    
                                </div>
                                <asp:Label ID="lbl5" runat="server" Text="Max_Working_Hrs/Day"  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl5" runat="server" CssClass="form-control"  ReadOnly="false" MaxLength="4" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>                                
                            </div>
                            <div class="form-group">
                               <asp:Label ID="lbl3" runat="server" Text="1st Half From"  CssClass="col-sm-3 control-label"></asp:Label>
                                 <div class="col-sm-3">
                                     <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" TextMode="Time" onkeypress="return isDecimalKey(event)" MaxLength="8" ></asp:TextBox>                                     
                                </div>
                                <asp:Label ID="Label3" runat="server" Text="2nd Half From"  CssClass="col-sm-3 control-label"></asp:Label>                                
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtunit" runat="server" CssClass="form-control" TextMode="Time" ReadOnly="false"  MaxLength="8"></asp:TextBox>
                                </div>
                                 </div>
                            <div class="form-group">
                                <asp:Label ID="lbl6" runat="server" Text="1st Half Upto"   CssClass="col-sm-3 control-label"></asp:Label>
                                 <div class="col-sm-3">
                                     <asp:TextBox ID="txtlbl6" runat="server" CssClass="form-control" TextMode="Time" ReadOnly="false" MaxLength="8"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label4" runat="server" Text="2nd Half Upto"  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtfc" runat="server" CssClass="form-control" TextMode="Time" ReadOnly="false" MaxLength="8"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                           <asp:Label ID="Label6" runat="server" Text="Deactivated Y/N"  CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtstatus" runat="server" CssClass="form-control" MaxLength="1" Text="N" ForeColor="Red"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label12" runat="server" Text="(Y:Close,N:Open)" Font-Size="Small"  CssClass="col-sm-6 control-label" ForeColor="Red"></asp:Label>
                                
                            </div>
                        </div>
                    </div>
                </div>

                <section class="col-lg-12 connectedSortable">
                    <div class="panel panel-default">
                        <div id="Tabs" role="tabpanel">
                            <ul class="nav nav-tabs" role="tablist">
                                <li><a href="#DescTab" id="tab1" runat="server" aria-controls="DescTab" role="tab" data-toggle="tab">Income Details</a></li>                                                             
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Deduction Details</a></li> 
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="330px" Font-Size="13px"
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

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Earning" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Earning" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="ER" />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="HSN_Code"  Visible="false"  />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" Visible="false"/>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Income Heads</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="text" Width="100%" onChange="calqty()" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>PF</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" MaxLength="22"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t2" runat="server"  Width="100%"  />
                                                       <asp:HiddenField ID="cmd2" Value='<%#Eval("sg1_t2") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Er_PF</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="25"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t3" runat="server" Width="100%" />
                                                        <asp:HiddenField ID="cmd3" Value='<%#Eval("sg1_t3") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>ER_VPF</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="25"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t4" runat="server"  Width="100%" />
                                                        <asp:HiddenField ID="cmd4" Value='<%#Eval("sg1_t4") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>VPF</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="calculateSum()" MaxLength="25"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t5" runat="server"  Width="100%" />
                                                        <asp:HiddenField ID="cmd5" Value='<%#Eval("sg1_t5") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>ESI</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="25" onkeyup="calculateSum()"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t6" runat="server"  Width="100%" />
                                                        <asp:HiddenField ID="cmd6" Value='<%#Eval("sg1_t6") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>J</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnJob" runat="server" CommandName="SG1_ROW_JOB" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Select Job No." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>WF</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" Style="text-align: right" onkeyup="calculateSum()"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t8" runat="server" Width="100%" />
                                                        <asp:HiddenField ID="cmd8" Value='<%#Eval("sg1_t8") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>PT</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" onkeyup="calculateSum()"></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t9" runat="server"  Width="100%" />
                                                        <asp:HiddenField ID="cmd9" Value='<%#Eval("sg1_t9") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                              
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>J</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnbtch" runat="server" CommandName="SG1_ROW_BTCH" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Select Batch No." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>OT</HeaderTemplate>
                                                    <ItemTemplate>
                                                       <%-- <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" ></asp:TextBox>--%>
                                                        <asp:CheckBox ID="sg1_t11" runat="server"  Width="100%" />
                                                        <asp:HiddenField ID="cmd11" Value='<%#Eval("sg1_t11") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>OT2</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="sg1_t14" runat="server"  Width="100%" />
                                                        <asp:HiddenField ID="cmd14" Value='<%#Eval("sg1_t14") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>EL</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <%--<asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" ></asp:TextBox>--%>
                                                         <asp:CheckBox ID="sg1_t12" runat="server"  Width="100%" ></asp:CheckBox>
                                                        <asp:HiddenField ID="cmd12" Value='<%#Eval("sg1_t12") %>'  runat="server"/>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>ER_Proportion</HeaderTemplate>
                                                    <ItemTemplate>
                                                      <asp:DropDownList ID="ddern" OnSelectedIndexChanged="ddern_SelectedIndexChanged" runat="server" Width="100%" AutoPostBack="true">
                                                           <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                            <asp:ListItem Text="WORKDAYS" Value="WORKDAYS"></asp:ListItem>
                                                            <asp:ListItem Text="TOTDAYS" Value="TOTDAYS"></asp:ListItem>
                                                           <asp:ListItem Text="OTHER" Value="OTHER"></asp:ListItem>
                                                      </asp:DropDownList>
                                                         <asp:HiddenField ID="cmd13" Value='<%#Eval("sg1_t13") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Other</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtddern" runat="server" Text='<%#Eval("txtddern") %>' Width="100%" MaxLength="2" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t15</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t15" runat="server" Text='<%#Eval("sg1_t15") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t16</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t16" runat="server" Text='<%#Eval("sg1_t16") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0"  ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>                                                       

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="330px" Font-Size="13px"
                                            AutoGenerateColumns="False" OnRowDataBound="sg3_RowDataBound"
                                            OnRowCommand="sg3_RowCommand">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>                                              
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Add</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnadd" runat="server" CommandName="SG3_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg3_btnrmv" runat="server" CommandName="SG3_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg3_Srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg3_f1" HeaderText="Ded"/>
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Export_Item" Visible="false" />

                                                <asp:TemplateField >
                                                    <HeaderTemplate>Deduction_Head</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' MaxLength="10" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Deduction_Type</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="50" Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Rate</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' onChange="calqty()" onkeypress="return isDecimalKey(event)" MaxLength="6" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Employer_%</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' onkeypress="return isDecimalKey(event)" MaxLength="6" Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField >
                                                    <HeaderTemplate>Max_Limit</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t5" runat="server" Text='<%#Eval("sg3_t5") %>' onkeypress="return isDecimalKey(event)" MaxLength="5" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Proportionate</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="sg3_t6" OnSelectedIndexChanged="sg3_t6_SelectedIndexChanged" runat="server" Width="100%" AutoPostBack="true">
                                                           <asp:ListItem Text="PLEASE SELECT" Value="PLEASE SELECT"></asp:ListItem>
                                                             <asp:ListItem Text="N.A." Value="N.A."></asp:ListItem>
                                                            <asp:ListItem Text="WORKDAYS" Value="WORKDAYS"></asp:ListItem>
                                                            <asp:ListItem Text="TOTDAYS" Value="TOTDAYS"></asp:ListItem>
                                                           <asp:ListItem Text="OTHER" Value="OTHER"></asp:ListItem>
                                                      </asp:DropDownList>
                                                         <asp:HiddenField ID="cmd15" Value='<%#Eval("sg3_t6") %>'  runat="server"/>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Other</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t7" runat="server" Text='<%#Eval("sg3_t7") %>' onkeypress="return isDecimalKey(event)" MaxLength="2" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Value(US$)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t8" runat="server" Text='<%#Eval("sg3_t8") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" >
                                                    <HeaderTemplate>Value(US$)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t9" runat="server" Text='<%#Eval("sg3_t9") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <EditRowStyle BackColor="#999999" />
                                            <FooterStyle BackColor="#5D7B9D"  ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0"  ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6"  ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>                             
                            </div>
                        </div>
                    </div>
                </section>

                <div class="col-md-6">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label2" runat="server" Text="OT1" CssClass="col-sm-1 control-label"></asp:Label>
                                    <asp:Label ID="Label7" runat="server" Text="No._of_Times"  Font-Size="Small" ForeColor="Red" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtovertm" runat="server" CssClass="form-control" Width="100%" MaxLength="8" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="Label8" runat="server" Text="Dividing_Factor"  Font-Size="Small" ForeColor="Red" CssClass="col-sm-1 control-label"></asp:Label>
                                    </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddOT" runat="server" TabIndex="11" Width="100%" Height="30px" OnSelectedIndexChanged="ddOT_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                <div class="col-sm-2">
                                           <asp:TextBox ID="txtOTErn" runat="server" CssClass="form-control" Width="100%" MaxLength="20" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                           </div>
                            </div>
                        </div>
                    </div>
                    </div>
                
                 <div class="col-md-6">
                    <div>
                        <div class="box-body">
                 <div class="form-group">
                                <asp:Label ID="Label15" runat="server" Text="OT2"  CssClass="col-sm-1 control-label"></asp:Label>
                                    <asp:Label ID="Label16" runat="server" Text="No._of_Times"  Font-Size="Small" ForeColor="Red" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtovertm2" runat="server" CssClass="form-control" Width="100%" MaxLength="8" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Label ID="Label17" runat="server" Text="Dividing_Factor"  Font-Size="Small" ForeColor="Red" CssClass="col-sm-1 control-label"></asp:Label>
                                    </div>
                                <div class="col-sm-3">
                                    <asp:DropDownList ID="ddOT2" runat="server" TabIndex="11" Width="100%" Height="30px" OnSelectedIndexChanged="ddOT2_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                <div class="col-sm-2">
                                           <asp:TextBox ID="txtOTErn2" runat="server" CssClass="form-control" Width="100%" MaxLength="20" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                           </div>
                            </div>
                            </div></div></div>              

                <div class="col-md-6" style="display:none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label9" runat="server" Text="PF_PDay"   CssClass="col-sm-3 control-label" ToolTip="Specify the Creteria For per Number of Days"></asp:Label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="DDPF" OnSelectedIndexChanged="DDPF_SelectedIndexChanged" runat="server" TabIndex="11" Width="100%" Height="30px" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                       <div class="col-sm-4">
                                           <asp:TextBox ID="txtddpf" runat="server" CssClass="form-control" Width="100%" MaxLength="2" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                           </div>
                            </div>                                  
                        </div>
                    </div>
                    </div>
              
                <div class="col-md-6" style="display:none">
                    <div>
                        <div class="box-body">
                <div class="form-group">
                                <asp:Label ID="Label10" runat="server" Text="ESI_PDay"  CssClass="col-sm-3 control-label" ToolTip="Specify the Creteria For per Number of Days"></asp:Label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="DDESI" OnSelectedIndexChanged="DDESI_SelectedIndexChanged" runat="server" TabIndex="11" Width="100%" Height="30px" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                      <div class="col-sm-4">
                                           <asp:TextBox ID="txtddesi" runat="server" CssClass="form-control" Width="100%" MaxLength="2" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                           </div>
                            </div>
                            </div></div></div>

                <div class="col-md-6" style="display:none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="WF_PerDay"   CssClass="col-sm-3 control-label" ToolTip="Specify the Creteria For per Number of Days"></asp:Label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="DDWF" OnSelectedIndexChanged="DDWF_SelectedIndexChanged" runat="server" TabIndex="11" Width="100%" Height="30px" AutoPostBack="true"></asp:DropDownList>
                                    </div> 
                                    <div class="col-sm-4">
                                           <asp:TextBox ID="txtddwf" runat="server" CssClass="form-control" Width="100%" MaxLength="2" BorderStyle="Solid" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                           </div>                               
                            </div> 
                            </div></div></div>

                <div class="col-md-6" style="display:none">
                    <div>
                        <div class="box-body">
                            <div class="form-group">
                                <asp:Label ID="Label13" runat="server" Text="Earning_to_be_Proportionate"   CssClass="col-sm-4 control-label"></asp:Label>
                                <asp:Label ID="Label14" runat="server" Text="WORKDAYS/ "  Font-Size="Small" ForeColor="Red" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    
                                    </div> 
                                <div class="col-sm-3">
                                           
                                           </div>
                                </div>
                                </div></div></div>
                <%--<div class="col-md-12" style="visibility:hidden">
                    <div>
                        <div class="box-body">
                            <asp:TextBox ID="txtrmk" runat="server" Width="99%" TextMode="MultiLine" onkeyup="max_length(this,200)" placeholder="Remarks"></asp:TextBox>
                        </div>
                    </div>
                </div>--%>

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
        function calqty() {
            debugger;

            var imqty = 0, imval = 0, expqty = 0, expval = 0;

            var grid = document.getElementById("<%= sg1.ClientID%>");
            for (var i = 0; i < sg1.Rows.Count - 1; i++) {
                imqty = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t3_' + i).value));
                alert(imqty);
                imval = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg1_sg1_t4_' + i).value));
            }
            for (var k = 0; k < sg3.Rows.Count - 1; k++) {
                expqty = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg3_sg3_t3_' + k).value));
                expval = fill_zero(Number(document.getElementById('ContentPlaceHolder1_sg3_sg3_t4_' + k).value));
            }
        }
        function fill_zero(val) {
            if (isNaN(val)) return 0; if (isFinite(val)) return val;
        }

   </script>

    <asp:HiddenField ID="TabName" runat="server" />
</asp:Content>
