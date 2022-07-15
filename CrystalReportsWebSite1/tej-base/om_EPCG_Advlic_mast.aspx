<%@ Page Language="C#" MasterPageFile="~/tej-base/Fin_Master.master" AutoEventWireup="true" Inherits="om_EPCG_Advlic_mast" CodeFile="om_EPCG_Advlic_mast.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            calculateSum();
        });
        function calculateSum() {
<%--            var totCGST = 0;
                var totSGST = 0;
                var IAMOUNT = 0;
                var TOTAMT = 0;
                var grid = document.getElementById("<%= sg1.ClientID%>");
                for (var i = 0; i < grid.rows.length - 1; i++) {
                    var Qty = $("input[id*=sg1_t2]");
                    var Rate = $("input[id*=sg1_t5]");


                    var CGST = $("input[id*=sg1_t7]");


                    var SGST = $("input[id*=sg1_t8]");

                    //var DiscRs = $("input[id*=sg1_t9]");

                    var AMOUNT = 0;
                    var tax1 = 0;
                    var tax2 = 0;
                    AMOUNT = (Qty[i].value * 1) * (Rate[i].value * 1);



                    document.getElementById('ContentPlaceHolder1_sg1_sg1_t6_' + i).value = fill_zero(AMOUNT).toFixed(3);

                    IAMOUNT += fill_zero(AMOUNT);
                    if ((CGST[i].value * 1) > 0) {
                        totCGST += (AMOUNT * (CGST[i].value * 1) / 100);
                        tax1 = (AMOUNT * (CGST[i].value * 1) / 100);
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t17_' + i).value = fill_zero(tax1).toFixed(3);

                    }
                    if ((SGST[i].value * 1) > 0) {
                        totSGST += (AMOUNT * (SGST[i].value * 1) / 100);
                        tax2 = (AMOUNT * (SGST[i].value * 1) / 100);
                        document.getElementById('ContentPlaceHolder1_sg1_sg1_t18_' + i).value = fill_zero(tax2).toFixed(3);

                    }
                }

                document.getElementById('ContentPlaceHolder1_txtlbl25').value = fill_zero(IAMOUNT).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl27').value = fill_zero(totCGST).toFixed(3);
                document.getElementById('ContentPlaceHolder1_txtlbl29').value = fill_zero(totSGST).toFixed(3);

                TOTAMT = fill_zero(IAMOUNT) + fill_zero(totCGST) + fill_zero(totSGST);
                document.getElementById('ContentPlaceHolder1_txtlbl31').value = fill_zero(TOTAMT).toFixed(3);--%>
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
                                <label id="Opt_No" runat="server" class="col-sm-2 control-label" title="lbl1">Opt_No</label>
                                <div class="col-sm-1">
                                    <asp:Label ID="lbl1a" runat="server" Text="TC" Style="width: 22px; float: right;" CssClass="col-sm-1 control-label"></asp:Label>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox id="txtvchnum" type="text" CssClass="form-control" runat="server" placeholder="Opt No." readonly="true" />
                                </div>
                        
                                <label id="Opt_Date" runat="server" class="col-sm-2 control-label" title="lbl1">Opt_Date</label>
                                <div class="col-sm-4">
                                    <asp:TextBox id="txtvchdate" type="text" CssClass="form-control" runat="server" placeholder="Opt Date" Width="100%" readonly="true" />                                    
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lbl4" runat="server" Text="Validity_Till" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl4" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtlbl4_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtlbl4"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEdit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl4" />
                                </div>
                                <asp:Label ID="Label1" runat="server" Text="Last_Date" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
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
                                <asp:Label ID="lbl7" runat="server" Text="Export_Obligation" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-1" style="display:none;">
                                    <asp:ImageButton ID="btnlbl7" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btnlbl7_Click" />
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl7" runat="server" CssClass="form-control" MaxLength="12" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                                <asp:Label ID="Label2" runat="server" Text="No_Of_Times" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl7a" runat="server" CssClass="form-control" Width="100%" MaxLength="5" onkeypress="return isDecimalKey(event)"></asp:TextBox>
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
                                <asp:Label ID="lbl2" runat="server" Text="Licence_No" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl2" runat="server" CssClass="form-control" Width="100%" MaxLength="15" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl3" runat="server" Text="Licence_Date" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                 <div class="col-sm-4">
                                     <asp:TextBox ID="txtlbl3" runat="server" CssClass="form-control" MaxLength="10" ></asp:TextBox>
                                     <asp:CalendarExtender ID="txtlbl3_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtlbl3"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="Maskedit1" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl3" />
                                </div>
                            </div>

                            <div class="form-group">
                               <asp:Label ID="Label3" runat="server" Text="Date_Of_First_Clearance" Font-Bold="true" CssClass="col-sm-3 control-label"></asp:Label>
                               <div class="col-sm-3">
                                   <asp:TextBox ID="txtlbl31" runat="server" CssClass="form-control" Width="100%"  MaxLength="10"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtlbl31_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtlbl31"
                                                Format="dd/MM/yyyy">
                                            </asp:CalendarExtender>
                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99/99/9999"
                                                MaskType="Date" TargetControlID="txtlbl31" />
                               </div>
                               <asp:Label ID="Label4" runat="server" Text="Average_Exp" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                   <asp:TextBox ID="txtlbl3a" runat="server" CssClass="form-control" Width="100%"  MaxLength="12" onkeypress="return isDecimalKey(event)"></asp:TextBox>
                                    </div>
                           </div>
                            
                            <div class="form-group">
                               <asp:Label ID="lbl5" runat="server" Text="F_Curr" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>

                                <div class="col-sm-1">
                                    <asp:ImageButton ID="btncurr" runat="server" ImageUrl="../tej-base/css/images/bdsearch5.png" Style="width: 22px; float: right;" OnClick="btncurr_Click"  />
                                    </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtlbl5" runat="server" CssClass="form-control" MaxLength="12" ReadOnly="true"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl6" runat="server" Text="Total_Value" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                 <div class="col-sm-4">
                                     <asp:TextBox ID="txtlbl6" runat="server" CssClass="form-control" ReadOnly="true" onChange="calqty()" MaxLength="12"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group" style="display:none;">
                                <asp:Label ID="lbl8" runat="server" Text="Export_Quantity" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl8" runat="server" CssClass="form-control" ReadOnly="true" onChange="calqty()" MaxLength="12"></asp:TextBox>
                                </div>
                                <asp:Label ID="lbl9" runat="server" Text="Export_Value" Font-Bold="true" CssClass="col-sm-2 control-label"></asp:Label>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtlbl9" runat="server" CssClass="form-control" ReadOnly="true" onChange="calqty()" MaxLength="12"></asp:TextBox>
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
                                <li><a href="#DescTab4" id="tab4" runat="server" aria-controls="DescTab4" role="tab" data-toggle="tab">Export Details</a></li> 
                            </ul>

                            <div class="tab-content">
                                <div role="tabpanel" class="tab-pane active" id="DescTab">
                                    <div class="lbBody" id="gridDiv" style="color: White; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg1" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="300px" Font-Size="13px"
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
                                                        <asp:ImageButton ID="sg1_btnadd" runat="server" CommandName="SG1_ROW_ADD" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Insert Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Del</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnrmv" runat="server" CommandName="SG1_RMV" ImageUrl="~/tej-base/images/Btn_remn.png" Width="20px" ImageAlign="Middle" ToolTip="Remove Item" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="sg1_srno" HeaderText="Sr.No" />
                                                <asp:BoundField DataField="sg1_f1" HeaderText="HSN_Code"  Visible="false"  />
                                                <asp:BoundField DataField="sg1_f2" HeaderText="HSN_Code"  Visible="false"  />
                                                <asp:BoundField DataField="sg1_f3" HeaderText="sg1_f3" Visible="false" />
                                                <asp:BoundField DataField="sg1_f4" HeaderText="sg1_f4" Visible="false" />
                                                <asp:BoundField DataField="sg1_f5" HeaderText="sg1_f5" Visible="false"/>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Item_Code</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t1" runat="server" Text='<%#Eval("sg1_t1") %>' onkeypress="return isDecimalKey(event)" Width="100%" onChange="calqty()" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Item_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t2" runat="server" Text='<%#Eval("sg1_t2") %>' Width="100%" onChange="calqty()" MaxLength="135"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>Quantity</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t3" runat="server" Text='<%#Eval("sg1_t3") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>CIF_Value(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t4" runat="server" Text='<%#Eval("sg1_t4") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>CIF_Value(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t5" runat="server" Text='<%#Eval("sg1_t5") %>' onkeypress="return isDecimalKey(event)" Width="100%" onkeyup="calculateSum()" MaxLength="12"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>Duty_Save(FC)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t6" runat="server" Text='<%#Eval("sg1_t6") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField >
                                                    <HeaderTemplate>Duty_Save(INR)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t8" runat="server" Text='<%#Eval("sg1_t8") %>' onkeypress="return isDecimalKey(event)" Width="100%" MaxLength="10"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t9</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t9" runat="server" Text='<%#Eval("sg1_t9") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%" onkeyup="calculateSum()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                              
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>J</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="sg1_btnbtch" runat="server" CommandName="SG1_ROW_BTCH" ImageAlign="Middle" ImageUrl="~/tej-base/images/Btn_addn.png" Width="20px" ToolTip="Select Batch No." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t11</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t11" runat="server" Text='<%#Eval("sg1_t11") %>' Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t12</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t12" runat="server" Text='<%#Eval("sg1_t12") %>' Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t13</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t13" runat="server" Text='<%#Eval("sg1_t13") %>' Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>sg1_t14</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg1_t14" runat="server" Text='<%#Eval("sg1_t14") %>' onkeypress="return isDecimalKey(event)" oncontextmenu="return false;" onpaste="return false" Width="100%"></asp:TextBox>
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
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White" CssClass="GridviewScrollHeader2" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager2" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem2" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>
                                    </div>
                                </div>                                                       

                                <div role="tabpanel" class="tab-pane active" id="DescTab4">
                                    <div class="lbBody" style="height: 250px; overflow: auto; box-shadow: 0 2px 4px rgba(127,127,127,.3);">
                                        <fin:CoolGridView ID="sg3" runat="server" ForeColor="#333333"
                                            Style="background-color: #FFFFFF; color: White;" Width="100%" Height="350px" Font-Size="13px"
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
                                                <asp:BoundField DataField="sg3_f1" HeaderText="HSN_Code" Visible="false"/>
                                                <asp:BoundField DataField="sg3_f2" HeaderText="Export_Item" Visible="false" />

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>Item_Code</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t1" runat="server" Text='<%#Eval("sg3_t1") %>' onkeypress="return isDecimalKey(event)" MaxLength="22" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>Export_Item_Description</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t2" runat="server" Text='<%#Eval("sg3_t2") %>' MaxLength="100" Width="100%" ></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Quantity(Kgs)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t3" runat="server" Text='<%#Eval("sg3_t3") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%" onChange="calqty()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>Value(US$)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t4" runat="server" Text='<%#Eval("sg3_t4") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%" onChange="calqty()"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField >
                                                    <HeaderTemplate>Wastage_Percentage</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t5" runat="server" Text='<%#Eval("sg3_t5") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false">
                                                    <HeaderTemplate>t6</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t6" runat="server" Text='<%#Eval("sg3_t6") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false" >
                                                    <HeaderTemplate>Value(US$)</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="sg3_t7" runat="server" Text='<%#Eval("sg3_t7") %>' onkeypress="return isDecimalKey(event)" MaxLength="25" Width="100%"></asp:TextBox>
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
                                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#1797c0" Font-Bold="True" ForeColor="White"
                                                CssClass="GridviewScrollHeader" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" CssClass="GridviewScrollPager" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" CssClass="GridviewScrollItem" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        </fin:CoolGridView>
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
